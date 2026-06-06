using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class DPCM : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private readonly float _quantStep;
        private readonly int _bitsPerSample;

        public string AlgorithmName => $"Differential PCM (DPCM) - {_bitsPerSample}-bit";

        public DPCM(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();
            _bitsPerSample = settings.BitsPerSample;
            int levels = (int)Math.Pow(2, _bitsPerSample);
            _quantStep = 2.0f / levels;
        }

        public DPCM(float quantStep)
        {
            _quantStep = quantStep;
            _bitsPerSample = 16;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;

            float[] samples = BytesToFloats(audioData);
            var output = new List<byte>();

            output.AddRange(BitConverter.GetBytes(samples.Length));

            short firstSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(samples[0] * 32767)));
            output.AddRange(BitConverter.GetBytes(firstSample));

            float previousSample = samples[0];
            for (int i = 1; i < samples.Length; i++)
            {
                float difference = samples[i] - previousSample;
                short quantizedDiff = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(difference / _quantStep)));
                output.AddRange(BitConverter.GetBytes(quantizedDiff));
                previousSample = samples[i];

                if (progress != null && i % 1000 == 0)
                {
                    int percent = (int)((double)i / samples.Length * 100);
                    progress.Report(percent);
                }
            }

            progress?.Report(100);

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

            short firstSample = BitConverter.ToInt16(compressedData, 4);
            float previousSample = firstSample / 32767.0f;
            samples.Add(previousSample);

            for (int i = 6; i < compressedData.Length; i += 2)
            {
                short quantizedDiff = BitConverter.ToInt16(compressedData, i);
                float difference = quantizedDiff * _quantStep;
                float currentSample = previousSample + difference;
                samples.Add(currentSample);
                previousSample = currentSample;
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