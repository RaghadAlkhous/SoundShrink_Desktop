using System;
using System.Collections.Generic;

namespace SoundShrink_Desktop.Algorithms
{
    /// <summary>
    /// Nonlinear Quantization (Mu-Law) - Academic Implementation
    /// ضغط باستخدام التكميم اللوغاريتمي غير الخطي (ITU-T G.711 Mu-Law).
    /// يقلل دقة العينات عالية السعة ويزيد دقة العينات منخفضة السعة، مما يحسن SNR للأصوات الهادئة.
    /// يحول كل عينة 32-bit float إلى بايت واحد 8-bit.
    /// </summary>
    public class NonlinearQuantization : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private readonly int _quantizationLevels;
        private readonly double _mu;

        public string AlgorithmName => "Nonlinear Quantization (Mu-Law)";

        /// <param name="quantizationLevels">عدد مستويات التكميم. الافتراضي 256 (8-bit).</param>
        /// <param name="mu">معامل الانضغاط اللوغاريتمي. القيمة القياسية 255.0.</param>
        public NonlinearQuantization(int quantizationLevels = 256, double mu = 255.0)
        {
            _quantizationLevels = quantizationLevels;
            _mu = mu;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            float[] samples = BytesToFloats(audioData);
            byte[] compressed = new byte[samples.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                compressed[i] = MuLawEncode(samples[i]);
            }

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
            float[] samples = new float[compressedData.Length];
            for (int i = 0; i < compressedData.Length; i++)
            {
                samples[i] = MuLawDecode(compressedData[i]);
            }

            byte[] output = new byte[samples.Length * 4];
            Buffer.BlockCopy(samples, 0, output, 0, output.Length);
            return output;
        }

        public CompressionResult GetCompressionStats() => _result;

        /// <summary>
        /// تشفير Mu-Law: تحويل عينة float مطبعة [-1, 1] إلى قيمة 8-bit مضغوطة
        /// </summary>
        private byte MuLawEncode(float sample)
        {
            // حماية من القيم خارج النطاق المطبع
            double normalized = Math.Max(-1.0, Math.Min(1.0, sample));

            // تطبيق صيغة الانضغاط اللوغاريتمي
            double compressed = Math.Log(1 + _mu * Math.Abs(normalized)) / Math.Log(1 + _mu);

            // استعادة الإشارة السالبة
            if (normalized < 0)
                compressed = -compressed;

            // تكميم إلى عدد المستويات المحدد
            int quantized = (int)((compressed + 1) / 2 * (_quantizationLevels - 1));
            return (byte)Math.Max(0, Math.Min(_quantizationLevels - 1, quantized));
        }

        /// <summary>
        /// فك تشفير Mu-Law: استعادة العينة المطبعة من القيمة 8-bit المضغوطة
        /// </summary>
        private float MuLawDecode(byte encoded)
        {
            // تحويل إلى نطاق [-1, 1]
            double normalized = (double)encoded / (_quantizationLevels - 1) * 2 - 1;

            // استخراج الإشارة
            double sign = normalized < 0 ? -1 : 1;
            normalized = Math.Abs(normalized);

            // تطبيق صيغة التوسيع العكسي
            double expanded = (Math.Pow(1 + _mu, normalized) - 1) / _mu;

            return (float)(sign * expanded);
        }

        private float[] BytesToFloats(byte[] bytes)
        {
            int sampleCount = bytes.Length / 4;
            float[] samples = new float[sampleCount];
            Buffer.BlockCopy(bytes, 0, samples, 0, sampleCount * 4);
            return samples;
        }
    }
}