using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class AdaptiveDeltaModulation : ICompressionAlgorithm
    {
        private CompressionResult _result;

        // معلمات التحكم في التكيف
        private readonly double _initialStepSize;
        private readonly double _stepSizeMultiplier;
        private readonly double _minStepSize;
        private readonly double _maxStepSize;

        public string AlgorithmName => "Adaptive Delta Modulation (ADM)";

        /// <summary>
        /// Constructor يقبل إعدادات الضغط من واجهة المستخدم
        /// </summary>
        /// <param name="settings">إعدادات الضغط (اختياري - يستخدم القيم الافتراضية إذا لم يتم تمريرها)</param>
        public AdaptiveDeltaModulation(CompressionSettings settings = null)
        {
            // استخدام الإعدادات الممررة أو القيم الافتراضية
            settings = settings ?? new CompressionSettings();

            _initialStepSize = settings.InitialStepSize;
            _stepSizeMultiplier = settings.StepSizeMultiplier;

            // قيم ثابتة للحماية من التشويه والضوضاء
            _minStepSize = 0.005;  // الحد الأدنى للخطوة
            _maxStepSize = 0.5;    // الحد الأقصى للخطوة
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            // 1️⃣ تحويل البيانات الخام إلى عينات Float
            float[] samples = BytesToFloats(audioData);

            // 2️⃣ تيار البتات المتكيف
            var bits = new List<bool>(samples.Length);
            double predicted = 0.0;
            double stepSize = _initialStepSize;
            bool previousBit = false;

            for (int i = 0; i < samples.Length; i++)
            {
                // اتخاذ القرار: هل العينة الحالية أكبر أم أصغر من التنبؤ؟
                bool currentBit = samples[i] >= predicted;
                bits.Add(currentBit);

                // تحديث المكامل (Integrator)
                predicted += currentBit ? stepSize : -stepSize;

                //  آلية التكيف: إذا استمر الاتجاه → نكبر الخطوة، إذا عكس الاتجاه → نصغرها
                if (i > 0)
                {
                    stepSize = currentBit == previousBit
                        ? stepSize * _stepSizeMultiplier
                        : stepSize / _stepSizeMultiplier;

                    //  حماية من القيم المتطرفة (Slope Overload & Granular Noise)
                    stepSize = Math.Max(_minStepSize, Math.Min(stepSize, _maxStepSize));
                }

                previousBit = currentBit;
            }

            // 3️⃣ تعبئة البتات في بايتات (Bit Packing)
            int headerSize = 4;
            int dataSize = (bits.Count + 7) / 8;
            byte[] compressed = new byte[headerSize + dataSize];

            // كتابة عدد العينات في الـ Header
            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, compressed, 0, 4);

            // تعبئة البتات بترتيب MSB-first
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    compressed[headerSize + (i / 8)] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            // 4️⃣ تسجيل النتائج
            _result = new CompressionResult
            {
                OriginalSize = originalSize,
                CompressedSize = compressed.Length,
                CompressionRatio = (double)originalSize / compressed.Length,
                ProcessingTime = DateTime.Now - startTime
            };

            return compressed;
        }

        public byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels)
        {
            // 1️⃣ قراءة عدد العينات من الـ Header
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            int headerSize = 4;

            // 2️⃣ إعادة بناء الإشارة
            float[] reconstructed = new float[sampleCount];
            double current = 0.0;
            double stepSize = _initialStepSize;
            bool? previousBit = null;

            for (int i = 0; i < sampleCount; i++)
            {
                // استخراج البت
                bool bit = (compressedData[headerSize + (i / 8)] & (1 << (7 - (i % 8)))) != 0;

                // تطبيق خطوة المكامل
                current += bit ? stepSize : -stepSize;
                reconstructed[i] = (float)current;

                // 🔁 تطبيق نفس آلية التكيف
                if (previousBit.HasValue)
                {
                    stepSize = bit == previousBit.Value
                        ? stepSize * _stepSizeMultiplier
                        : stepSize / _stepSizeMultiplier;

                    stepSize = Math.Max(_minStepSize, Math.Min(stepSize, _maxStepSize));
                }

                previousBit = bit;
            }

            // 3️⃣ تحويل إلى Byte[]
            byte[] output = new byte[reconstructed.Length * 4];
            Buffer.BlockCopy(reconstructed, 0, output, 0, output.Length);

            return output;
        }

        public CompressionResult GetCompressionStats() => _result;

        private float[] BytesToFloats(byte[] bytes)
        {
            int sampleCount = bytes.Length / 4;
            float[] samples = new float[sampleCount];
            Buffer.BlockCopy(bytes, 0, samples, 0, sampleCount * 4);
            return samples;
        }
    }
}