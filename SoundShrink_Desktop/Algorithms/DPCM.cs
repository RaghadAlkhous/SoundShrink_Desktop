using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    /// <summary>
    /// Differential Pulse Code Modulation (DPCM) - Academic Implementation
    /// تخزن الفرق بين العينات المتتالية بعد تكميمه (Quantization) باستخدام خطوة محددة.
    /// العينة الأولى تُخزن كمرجع (16-bit)، والباقي تُخزن كفروقات مكممة إلى 16-bit.
    /// </summary>
    public class DPCM : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private readonly float _quantStep;
        private readonly int _bitsPerSample;

        public string AlgorithmName => $"Differential PCM (DPCM) - {_bitsPerSample}-bit";

        /// <summary>
        /// Constructor يقبل إعدادات الضغط من واجهة المستخدم
        /// </summary>
        /// <param name="settings">إعدادات الضغط (اختياري - يستخدم القيم الافتراضية إذا لم يتم تمريرها)</param>
        public DPCM(CompressionSettings settings = null)
        {
            // استخدام الإعدادات الممررة أو القيم الافتراضية
            settings = settings ?? new CompressionSettings();
            _bitsPerSample = settings.BitsPerSample;

            // حساب خطوة التكميم بناءً على عدد البتات
            // النطاق الكلي للإشارة = 2.0 (من -1 إلى +1)
            // عدد المستويات = 2^BitsPerSample
            // خطوة التكميم = النطاق / عدد المستويات
            int levels = (int)Math.Pow(2, _bitsPerSample);
            _quantStep = 2.0f / levels;
        }

        /// <summary>
        /// Constructor قديم يقبل خطوة التكميم مباشرة (للتوافق مع الكود القديم)
        /// </summary>
        /// <param name="quantStep">حجم خطوة التكميم</param>
        public DPCM(float quantStep)
        {
            _quantStep = quantStep;
            _bitsPerSample = 16; // افتراضي
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            float[] samples = BytesToFloats(audioData);
            var output = new List<byte>();

            // Header: عدد العينات (4 بايتات)
            output.AddRange(BitConverter.GetBytes(samples.Length));

            // العينة الأولى تُخزن كمرجع (تحويل إلى short مع حماية من التشبع)
            short firstSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(samples[0] * 32767)));
            output.AddRange(BitConverter.GetBytes(firstSample));

            float previousSample = samples[0];
            for (int i = 1; i < samples.Length; i++)
            {
                float difference = samples[i] - previousSample;
                // تكميم الفرق باستخدام خطوة التكميم المحسوبة
                short quantizedDiff = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(difference / _quantStep)));
                output.AddRange(BitConverter.GetBytes(quantizedDiff));
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
            // قراءة عدد العينات من الـ Header
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            var samples = new List<float>(sampleCount);

            // استعادة العينة المرجعية
            short firstSample = BitConverter.ToInt16(compressedData, 4);
            float previousSample = firstSample / 32767.0f;
            samples.Add(previousSample);

            // إعادة بناء العينات من الفروقات المكممة
            for (int i = 6; i < compressedData.Length; i += 2)
            {
                short quantizedDiff = BitConverter.ToInt16(compressedData, i);
                float difference = quantizedDiff * _quantStep; // فك التكميم
                float currentSample = previousSample + difference;
                samples.Add(currentSample);
                previousSample = currentSample;
            }

            // تحويل إلى Byte[] متوافق مع واجهة المشروع
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