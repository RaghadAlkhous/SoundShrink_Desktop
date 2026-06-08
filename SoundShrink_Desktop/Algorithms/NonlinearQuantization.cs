using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class NonlinearQuantization : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private  int _quantizationLevels;
        private  double _mu;

        public string AlgorithmName => $"Nonlinear Quantization (Mu-Law) - {_quantizationLevels} levels";

        public NonlinearQuantization(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();

            if (settings.QuantizationLevels < 2 || settings.QuantizationLevels > 256)
                throw new ArgumentException("QuantizationLevels must be between 2 and 256");

            _quantizationLevels = settings.QuantizationLevels;
            _mu = 255.0;
        }

        public NonlinearQuantization(int quantizationLevels, double mu = 255.0)
        {
            if (quantizationLevels < 2 || quantizationLevels > 256)
                throw new ArgumentException("quantizationLevels must be between 2 and 256");

            _quantizationLevels = quantizationLevels;
            _mu = mu;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            float[] samples = BytesToFloats(audioData);

            int bitsPerSample_compressed = (int)Math.Ceiling(Math.Log(_quantizationLevels, 2));
            int totalBits = samples.Length * bitsPerSample_compressed;
            int compressedSize = (totalBits + 7) / 8;

            byte[] compressed = new byte[compressedSize];

            // ✅ Bit Packing
            int bitPosition = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                byte encodedValue = MuLawEncode(samples[i]);

                int byteIndex = bitPosition / 8;
                int bitOffset = bitPosition % 8;

                if (bitOffset + bitsPerSample_compressed <= 8)
                {
                    compressed[byteIndex] |= (byte)(encodedValue << bitOffset);
                }
                else
                {
                    int bitsInFirstByte = 8 - bitOffset;
                    compressed[byteIndex] |= (byte)((encodedValue & ((1 << bitsInFirstByte) - 1)) << bitOffset);
                    compressed[byteIndex + 1] = (byte)(encodedValue >> bitsInFirstByte);
                }

                bitPosition += bitsPerSample_compressed;

                if (progress != null && i % 1000 == 0)
                {
                    int percent = (int)((double)i / samples.Length * 100);
                    progress.Report(percent);
                }
            }

            progress?.Report(100);

            int headerSize = 20;
            byte[] result = new byte[headerSize + compressed.Length];

            // حفظ المعلومات في الـ Header
            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, result, 0, 4);           // [0-3]: sampleCount
            Buffer.BlockCopy(BitConverter.GetBytes(_quantizationLevels), 0, result, 4, 4);      // [4-7]: quantizationLevels
            Buffer.BlockCopy(BitConverter.GetBytes((int)_mu), 0, result, 8, 4);                 // [8-11]: mu
            Buffer.BlockCopy(BitConverter.GetBytes(bitsPerSample_compressed), 0, result, 12, 4);// [12-15]: bitsPerSample

            // ✅ إضافة Sample Rate للـ Header
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, result, 16, 4);              // [16-19]: sampleRate

            Buffer.BlockCopy(compressed, 0, result, headerSize, compressed.Length);

            _result = new CompressionResult
            {
                OriginalSize = originalSize,
                CompressedSize = result.Length,
                CompressionRatio = (double)originalSize / result.Length,
                ProcessingTime = DateTime.Now - startTime
            };

            return result;
        }

        public byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels)
        {
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            int savedQuantLevels = BitConverter.ToInt32(compressedData, 4);
            int savedMu = BitConverter.ToInt32(compressedData, 8);
            int bitsPerSample_compressed = BitConverter.ToInt32(compressedData, 12);
            int savedSampleRate = BitConverter.ToInt32(compressedData, 16); // ✅ Sample Rate

            int headerSize = 20; // ✅
            _quantizationLevels = savedQuantLevels;
            _mu = savedMu;

            float[] samples = new float[sampleCount];
            int bitPosition = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                int byteIndex = headerSize + (bitPosition / 8);
                int bitOffset = bitPosition % 8;
                byte encodedValue;
                if (bitOffset + bitsPerSample_compressed <= 8)
                    encodedValue = (byte)((compressedData[byteIndex] >> bitOffset) & ((1 << bitsPerSample_compressed) - 1));
                else
                {
                    int bitsInFirstByte = 8 - bitOffset;
                    byte lowBits = (byte)(compressedData[byteIndex] >> bitOffset);
                    byte highBits = (byte)(compressedData[byteIndex + 1] & ((1 << (bitsPerSample_compressed - bitsInFirstByte)) - 1));
                    encodedValue = (byte)(lowBits | (highBits << bitsInFirstByte));
                }
                samples[i] = MuLawDecode(encodedValue);
                bitPosition += bitsPerSample_compressed;
            }

            byte[] output = new byte[samples.Length * 4];
            Buffer.BlockCopy(samples, 0, output, 0, output.Length);
            return output;
        }

        public CompressionResult GetCompressionStats() => _result;

        private byte MuLawEncode(float sample)
        {
            double normalized = Math.Max(-1.0, Math.Min(1.0, sample));
            double compressed = Math.Log(1 + _mu * Math.Abs(normalized)) / Math.Log(1 + _mu);

            if (normalized < 0)
                compressed = -compressed;

            int quantized = (int)((compressed + 1) / 2 * (_quantizationLevels - 1));
            return (byte)Math.Max(0, Math.Min(_quantizationLevels - 1, quantized));
        }

        private float MuLawDecode(byte encoded)
        {
            double normalized = (double)encoded / (_quantizationLevels - 1) * 2 - 1;
            double sign = normalized < 0 ? -1 : 1;
            normalized = Math.Abs(normalized);
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