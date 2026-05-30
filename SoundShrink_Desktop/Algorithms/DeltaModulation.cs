using System;
using System.Collections.Generic;

namespace SoundShrink_Desktop.Algorithms
{
    /// <summary>
    /// Delta Modulation (DM) - Academic Implementation
    /// خوارزمية ضغط تنبؤية تفقدية تحول كل عينة صوتية إلى بت واحد (1-bit)
    /// تعتمد على مكامل (Integrator) بخطوة ثابتة للتنبؤ بالقيمة التالية.
    /// مناسبة جداً للأغراض التعليمية ومقارنة أداء الخوارزميات التنبؤية.
    /// </summary>
    public class DeltaModulation : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private readonly double _stepSize;

        public string AlgorithmName => "Delta Modulation (DM)";

        /// <param name="stepSize">حجم الخطوة الثابتة للمكامل. القيمة الافتراضية 0.05 مناسبة للإشارات الطبيعية [-1, 1]</param>
        public DeltaModulation(double stepSize = 0.05)
        {
            _stepSize = stepSize;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            // 1️ تحويل البايتات الخام إلى عينات Float حقيقية
            float[] samples = BytesToFloats(audioData);

            // 2️ إنشاء تيار البتات (1 بت لكل عينة)
            var bits = new List<bool>(samples.Length);
            double predicted = 0.0;

            foreach (float sample in samples)
            {
                // مقارنة العينة الحالية مع القيمة المتوقعة
                bool bit = sample >= predicted;
                bits.Add(bit);

                // تحديث التنبؤ (Integrator)
                predicted += bit ? _stepSize : -_stepSize;
            }

            // 3️⃣ تحضير هيكل البيانات المضغوطة
            // Header: 4 بايتات لتخزين عدد العينات
            // Data: البتات مضغوطة بمعدل 8 بتات في كل بايت
            int headerSize = 4;
            int dataSize = (bits.Count + 7) / 8; // تقريب للأعلى
            byte[] compressed = new byte[headerSize + dataSize];

            // كتابة عدد العينات في الـ Header
            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, compressed, 0, 4);

            // تعبئة البتات في المصفوفة (Big-Endian داخل كل بايت)
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    compressed[headerSize + (i / 8)] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            // 4️⃣ تسجيل إحصائيات الضغط
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
            // 1️ قراءة عدد العينات من الـ Header
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            int headerSize = 4;

            // 2️⃣ إعادة بناء الإشارة من تيار البتات
            float[] reconstructed = new float[sampleCount];
            double current = 0.0;

            for (int i = 0; i < sampleCount; i++)
            {
                // استخراج البت الموافق
                bool bit = (compressedData[headerSize + (i / 8)] & (1 << (7 - (i % 8)))) != 0;

                // تطبيق خطوة المكامل بنفس الاتجاه
                current += bit ? _stepSize : -_stepSize;
                reconstructed[i] = (float)current;
            }

            // 3️⃣ تحويل العينات المعاد بناؤها إلى صيغة Byte متوافقة مع واجهة المشروع
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