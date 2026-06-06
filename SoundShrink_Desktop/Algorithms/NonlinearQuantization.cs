using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class NonlinearQuantization : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private readonly int _quantizationLevels;
        private readonly double _mu;

        public string AlgorithmName => $"Nonlinear Quantization (Mu-Law) - {_quantizationLevels} levels";

        public NonlinearQuantization(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();
            _quantizationLevels = settings.QuantizationLevels;
            _mu = 255.0;
        }

        public NonlinearQuantization(int quantizationLevels, double mu = 255.0)
        {
            _quantizationLevels = quantizationLevels;
            _mu = mu;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            float[] samples = BytesToFloats(audioData);
            byte[] compressed = new byte[samples.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                compressed[i] = MuLawEncode(samples[i]);

                
                if (progress != null && i % 1000 == 0)
                {
                    int percent = (int)((double)i / samples.Length * 100);
                    progress.Report(percent);
                }
            }

            progress?.Report(100);

           
            byte[] result = new byte[4 + compressed.Length];
            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, result, 0, 4);
            Buffer.BlockCopy(compressed, 0, result, 4, compressed.Length);

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
            int headerSize = 4;

            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                samples[i] = MuLawDecode(compressedData[headerSize + i]);
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