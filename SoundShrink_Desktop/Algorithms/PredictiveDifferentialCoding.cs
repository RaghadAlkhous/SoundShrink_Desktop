using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class PredictiveDifferentialCoding : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private double _predictionCoeff;
        private float _quantStep;
        private int _bitsPerSample;

        public string AlgorithmName => $"Predictive Differential Coding (PDC) - StepSize={_quantStep:F3}";

        public PredictiveDifferentialCoding(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();

            int levels = settings.QuantizationLevels > 1 ? settings.QuantizationLevels : 65536; // الافتراضي 16-bit
            if (levels < 2) levels = 2;
            if (levels > 65536) levels = 65536;

            _quantStep = (float)(2.0 / levels); 
            _bitsPerSample = (int)Math.Ceiling(Math.Log(levels, 2));
            _predictionCoeff = settings.PredictionCoefficient;
        }

        public PredictiveDifferentialCoding(double predictionCoeff, float quantStep = 0.1f)
        {
            if (predictionCoeff < 0.0 || predictionCoeff > 1.0)
                throw new ArgumentException("predictionCoeff must be between 0.0 and 1.0");

            if (quantStep <= 0.0 || quantStep > 1.0)
                throw new ArgumentException("quantStep must be between 0.0 and 1.0");

            _predictionCoeff = predictionCoeff;
            _quantStep = quantStep;
            _bitsPerSample = 16;
        }

        public byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null)
        {
            var startTime = DateTime.Now;
            long originalSize = audioData.Length;
            float[] samples = BytesToFloats(audioData);

            int headerSize = 22;
            byte[] result = new byte[headerSize + 2 + (samples.Length - 1) * 2];

            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, result, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((int)(_predictionCoeff * 1000000)), 0, result, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((int)(_quantStep * 1000000)), 0, result, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(_bitsPerSample), 0, result, 12, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(headerSize), 0, result, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, result, 18, 4); 

            short firstSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(samples[0] * 32767)));
            Buffer.BlockCopy(BitConverter.GetBytes(firstSample), 0, result, headerSize, 2);

            int position = headerSize + 2;
            float previousSample = samples[0];
            for (int i = 1; i < samples.Length; i++)
            {
                double predictedValue = previousSample * _predictionCoeff;
                float error = samples[i] - (float)predictedValue;
                short quantizedError = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(error / _quantStep)));
                Buffer.BlockCopy(BitConverter.GetBytes(quantizedError), 0, result, position, 2);
                position += 2;
                previousSample = samples[i];
                if (progress != null && i % 1000 == 0) progress.Report((int)((double)i / samples.Length * 100));
            }
            progress?.Report(100);

            _result = new CompressionResult { OriginalSize = originalSize, CompressedSize = result.Length, CompressionRatio = (double)originalSize / result.Length, ProcessingTime = DateTime.Now - startTime };
            return result;
        }

        public byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels)
        {
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            int savedPredictionCoeffInt = BitConverter.ToInt32(compressedData, 4);
            int savedQuantStepInt = BitConverter.ToInt32(compressedData, 8);
            int savedBitsPerSample = BitConverter.ToInt32(compressedData, 12);
            int headerSize = BitConverter.ToInt16(compressedData, 16);
            int savedSampleRate = BitConverter.ToInt32(compressedData, 18); 

            _predictionCoeff = savedPredictionCoeffInt / 1000000.0;
            _quantStep = savedQuantStepInt / 1000000.0f;
            _bitsPerSample = savedBitsPerSample;

            short firstSample = BitConverter.ToInt16(compressedData, headerSize);
            float previousSample = firstSample / 32767.0f;
            var samples = new float[sampleCount];
            samples[0] = previousSample;
            int position = headerSize + 2;

            for (int i = 1; i < sampleCount; i++)
            {
                double predictedValue = previousSample * _predictionCoeff;
                short quantizedError = BitConverter.ToInt16(compressedData, position);
                float error = quantizedError * _quantStep;
                float reconstructed = (float)(predictedValue + error);
                samples[i] = reconstructed;
                previousSample = reconstructed;
                position += 2;
            }

            byte[] output = new byte[samples.Length * 4];
            Buffer.BlockCopy(samples, 0, output, 0, output.Length);
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