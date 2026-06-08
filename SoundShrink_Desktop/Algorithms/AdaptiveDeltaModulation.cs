using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class AdaptiveDeltaModulation : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private double _initialStepSize;
        private double _stepSizeMultiplier;
        private double _minStepSize;
        private double _maxStepSize;

        public string AlgorithmName => "Adaptive Delta Modulation (ADM)";

        public AdaptiveDeltaModulation(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();

            int levels = settings.QuantizationLevels > 1 ? settings.QuantizationLevels : 256;
            if (levels < 2) levels = 2;

            if (settings.InitialStepSize > 0)
                _initialStepSize = settings.InitialStepSize;
            else
                _initialStepSize = 2.0 / levels;

            if (settings.StepSizeMultiplier <= 1.0)
                throw new ArgumentException("StepSizeMultiplier must be greater than 1.0");

            _stepSizeMultiplier = settings.StepSizeMultiplier;
            _minStepSize = 0.005;
            _maxStepSize = 0.5;
        }
        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;
            float[] samples = BytesToFloats(audioData);
            var bits = new List<bool>(samples.Length);
            double predicted = 0.0;
            double stepSize = _initialStepSize;
            bool previousBit = false;

            for (int i = 0; i < samples.Length; i++)
            {
                bool currentBit = samples[i] >= predicted;
                bits.Add(currentBit);
                predicted += currentBit ? stepSize : -stepSize;

                if (i > 0)
                {
                    stepSize = currentBit == previousBit ? stepSize * _stepSizeMultiplier : stepSize / _stepSizeMultiplier;
                    stepSize = Math.Max(_minStepSize, Math.Min(stepSize, _maxStepSize));
                }
                previousBit = currentBit;
                if (progress != null && i % 1000 == 0) progress.Report((int)((double)i / samples.Length * 100));
            }
            progress?.Report(100);

            int headerSize = 32;
            int dataSize = (bits.Count + 7) / 8;
            byte[] compressed = new byte[headerSize + dataSize];

            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, compressed, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(_initialStepSize), 0, compressed, 4, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(_stepSizeMultiplier), 0, compressed, 12, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(_minStepSize), 0, compressed, 20, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, compressed, 28, 4); 

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
            double savedInitialStep = BitConverter.ToDouble(compressedData, 4);
            double savedMultiplier = BitConverter.ToDouble(compressedData, 12);
            double savedMinStep = BitConverter.ToDouble(compressedData, 20);
            int savedSampleRate = BitConverter.ToInt32(compressedData, 28); 

            _initialStepSize = savedInitialStep;
            _stepSizeMultiplier = savedMultiplier;
            _minStepSize = savedMinStep;
            _maxStepSize = 0.5;
            int headerSize = 32; 

            float[] reconstructed = new float[sampleCount];
            double current = 0.0;
            double stepSize = _initialStepSize;
            bool? previousBit = null;

            for (int i = 0; i < sampleCount; i++)
            {
                bool bit = (compressedData[headerSize + (i / 8)] & (1 << (7 - (i % 8)))) != 0;
                current += bit ? stepSize : -stepSize;
                reconstructed[i] = (float)current;

                if (previousBit.HasValue)
                {
                    stepSize = bit == previousBit.Value ? stepSize * _stepSizeMultiplier : stepSize / _stepSizeMultiplier;
                    stepSize = Math.Max(_minStepSize, Math.Min(stepSize, _maxStepSize));
                }
                previousBit = bit;
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