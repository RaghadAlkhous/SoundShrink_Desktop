using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class DeltaModulation : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private double _stepSize; 

        public string AlgorithmName => "Delta Modulation (DM)";

        public DeltaModulation(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();

            if (settings.StepSize <= 0)
                throw new ArgumentException("StepSize must be greater than 0");

            _stepSize = settings.StepSize;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;
            float[] samples = BytesToFloats(audioData);
            var bits = new List<bool>(samples.Length);
            double predicted = 0.0;

            for (int i = 0; i < samples.Length; i++)
            {
                bool bit = samples[i] >= predicted;
                bits.Add(bit);
                predicted += bit ? _stepSize : -_stepSize;
                if (progress != null && i % 1000 == 0) progress.Report((int)((double)i / samples.Length * 100));
            }
            progress?.Report(100);

            // ✅ Header Size: 16 bytes
            int headerSize = 16;
            int dataSize = (bits.Count + 7) / 8;
            byte[] compressed = new byte[headerSize + dataSize];

            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, compressed, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(_stepSize), 0, compressed, 4, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, compressed, 12, 4); // ✅ Sample Rate

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i]) compressed[headerSize + (i / 8)] |= (byte)(1 << (7 - (i % 8)));
            }

            _result = new CompressionResult { OriginalSize = originalSize, CompressedSize = compressed.Length, CompressionRatio = (double)originalSize / compressed.Length, ProcessingTime = DateTime.Now - startTime };
            return compressed;
        }

        public byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels)
        {
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            double savedStepSize = BitConverter.ToDouble(compressedData, 4);
            int savedSampleRate = BitConverter.ToInt32(compressedData, 12); 

            _stepSize = savedStepSize;
            int headerSize = 16; // ✅

            float[] reconstructed = new float[sampleCount];
            double current = 0.0;
            for (int i = 0; i < sampleCount; i++)
            {
                bool bit = (compressedData[headerSize + (i / 8)] & (1 << (7 - (i % 8)))) != 0;
                current += bit ? _stepSize : -_stepSize;
                reconstructed[i] = (float)current;
            }

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