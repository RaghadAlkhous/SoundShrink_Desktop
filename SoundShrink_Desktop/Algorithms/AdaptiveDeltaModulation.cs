using System;
using System.Collections.Generic;

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

        /// <param name="initialStepSize">الخطوة الابتدائية للمكامل. الافتراضي 0.05 يناسب الإشارات المطبعة [-1, 1]</param>
        /// <param name="stepSizeMultiplier">معامل التضخيم/التخفيض عند تغير اتجاه البت. الافتراضي 1.5</param>
        /// <param name="minStepSize">الحد الأدنى للخطوة لمنع الاستقرار الزائد (Granular Noise).</param>
        /// <param name="maxStepSize">الحد الأقصى للخطوة لمنع التشويه (Slope Overload).</param>
        public AdaptiveDeltaModulation(
            double initialStepSize = 0.05,
            double stepSizeMultiplier = 1.5,
            double minStepSize = 0.005,
            double maxStepSize = 0.5)
        {
            _initialStepSize = initialStepSize;
            _stepSizeMultiplier = stepSizeMultiplier;
            _minStepSize = minStepSize;
            _maxStepSize = maxStepSize;
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

                // 🔁 آلية التكيف: إذا استمر الاتجاه → نكبر الخطوة، إذا عكس الاتجاه → نصغرها
                if (i > 0)
                {
                    stepSize = currentBit == previousBit
                        ? stepSize * _stepSizeMultiplier
                        : stepSize / _stepSizeMultiplier;

                    // 🛡️ حماية من القيم المتطرفة (Slope Overload & Granular Noise)
                    stepSize = Math.Max(_minStepSize, Math.Min(stepSize, _maxStepSize));
                }

                previousBit = currentBit;
            }

            // 3️⃣ تعبئة البتات في بايتات (Bit Packing)
            int headerSize = 4;
            int dataSize = (bits.Count + 7) / 8; // تقريب للأعلى
            byte[] compressed = new byte[headerSize + dataSize];

            // كتابة عدد العينات في الـ Header لضمان فك ضغط متطابق
            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, compressed, 0, 4);

            // تعبئة البتات بترتيب MSB-first داخل كل بايت
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

            // 2️⃣ إعادة بناء الإشارة بنفس المنطق التكيفي المستخدم في الضغط
            float[] reconstructed = new float[sampleCount];
            double current = 0.0;
            double stepSize = _initialStepSize;
            bool? previousBit = null;

            for (int i = 0; i < sampleCount; i++)
            {
                // استخراج البت المقابل
                bool bit = (compressedData[headerSize + (i / 8)] & (1 << (7 - (i % 8)))) != 0;

                // تطبيق خطوة المكامل
                current += bit ? stepSize : -stepSize;
                reconstructed[i] = (float)current;

                // 🔁 تطبيق نفس آلية التكيف بالضبط
                if (previousBit.HasValue)
                {
                    stepSize = bit == previousBit.Value
                        ? stepSize * _stepSizeMultiplier
                        : stepSize / _stepSizeMultiplier;

                    stepSize = Math.Max(_minStepSize, Math.Min(stepSize, _maxStepSize));
                }

                previousBit = bit;
            }

            // 3️ تحويل العينات المعاد بناؤها إلى Byte[] متوافق مع واجهة المشروع
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