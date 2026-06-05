using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    /// <summary>
    /// Predictive Differential Coding (PDC) - Academic Implementation
    /// يستخدم معامل تنبؤ خطي (Linear Prediction) لتقدير العينة التالية، ثم يخزن الخطأ المكمم.
    /// يجمع بين كفاءة DPCM ودقة التنبؤ الطيفي للإشارات الصوتية المترابطة.
    /// </summary>
    public class PredictiveDifferentialCoding : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private readonly double _predictionCoeff;
        private readonly float _quantStep;

        public string AlgorithmName => $"Predictive Differential Coding (PDC) - α={_predictionCoeff:F2}";

        /// <summary>
        /// Constructor يقبل إعدادات الضغط من واجهة المستخدم
        /// </summary>
        /// <param name="settings">إعدادات الضغط (اختياري - يستخدم القيم الافتراضية إذا لم يتم تمريرها)</param>
        public PredictiveDifferentialCoding(CompressionSettings settings = null)
        {
            // استخدام الإعدادات الممررة أو القيم الافتراضية
            settings = settings ?? new CompressionSettings();

            _predictionCoeff = settings.PredictionCoefficient;
            _quantStep = 0.01f; // قيمة ثابتة لخطوة التكميم
        }

        /// <summary>
        /// Constructor قديم يقبل المعاملات مباشرة (للتوافق مع الكود القديم)
        /// </summary>
        public PredictiveDifferentialCoding(double predictionCoeff, float quantStep = 0.01f)
        {
            _predictionCoeff = predictionCoeff;
            _quantStep = quantStep;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            float[] samples = BytesToFloats(audioData);
            var output = new List<byte>();

            // Header: عدد العينات
            output.AddRange(BitConverter.GetBytes(samples.Length));

            // العينة الأولى كمرجع
            short firstSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(samples[0] * 32767)));
            output.AddRange(BitConverter.GetBytes(firstSample));

            float previousSample = samples[0];
            for (int i = 1; i < samples.Length; i++)
            {
                // التنبؤ بالقيمة الحالية بناءً على العينة السابقة
                double predictedValue = previousSample * _predictionCoeff;
                float error = samples[i] - (float)predictedValue;

                // تكميم الخطأ إلى 16-bit
                short quantizedError = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(error / _quantStep)));
                output.AddRange(BitConverter.GetBytes(quantizedError));

                previousSample = samples[i];
            }

            _result = new CompressionResult
            {
                OriginalSize = originalSize,
                CompressedSize = output.Count,
                CompressionRatio = (double)originalSize / output.Count,
                ProcessingTime = DateTime.Now - startTime
            };

            return output.ToArray();
        }

        public byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels)
        {
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            var samples = new List<float>(sampleCount);

            // استعادة العينة المرجعية
            short firstSample = BitConverter.ToInt16(compressedData, 4);
            float previousSample = firstSample / 32767.0f;
            samples.Add(previousSample);

            // إعادة البناء من الأخطاء المكممة
            for (int i = 6; i < compressedData.Length; i += 2)
            {
                double predictedValue = previousSample * _predictionCoeff;
                short quantizedError = BitConverter.ToInt16(compressedData, i);
                float error = quantizedError * _quantStep; // فك التكميم

                float reconstructed = (float)(predictedValue + error);
                samples.Add(reconstructed);
                previousSample = reconstructed;
            }

            float[] resultArray = samples.ToArray();
            byte[] output = new byte[resultArray.Length * 4];
            Buffer.BlockCopy(resultArray, 0, output, 0, output.Length);

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