using System;
using System.Collections.Generic;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop.Algorithms
{
    public class DPCM : ICompressionAlgorithm
    {
        private CompressionResult _result;
        private float _quantStep;
        private int _bitsPerSample;

        public string AlgorithmName => $"Differential PCM (DPCM) - {_bitsPerSample}-bit";

        public DPCM(CompressionSettings settings = null)
        {
            settings = settings ?? new CompressionSettings();

            if (settings.BitsPerSample < 2 || settings.BitsPerSample > 16)
                throw new ArgumentException("BitsPerSample must be between 2 and 16");

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

            int totalBits = (samples.Length - 1) * _bitsPerSample;
            int dataBytes = (totalBits + 7) / 8;

            int headerSize = 18;
            byte[] result = new byte[headerSize + 2 + dataBytes];

            Buffer.BlockCopy(BitConverter.GetBytes(samples.Length), 0, result, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(_bitsPerSample), 0, result, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((int)(_quantStep * 1000000)), 0, result, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(headerSize), 0, result, 12, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, result, 14, 4); 

            short firstSample = (short)Math.Max(short.MinValue, Math.Min(short.MaxValue, (int)(samples[0] * 32767)));
            Buffer.BlockCopy(BitConverter.GetBytes(firstSample), 0, result, headerSize, 2);

            byte[] compressedData = new byte[dataBytes];
            int bitPosition = 0;
            float previousSample = samples[0];
            for (int i = 1; i < samples.Length; i++)
            {
                float difference = samples[i] - previousSample;
                int quantizedDiff = (int)(difference / _quantStep);
                int maxValue = (1 << (_bitsPerSample - 1)) - 1;
                int minValue = -(1 << (_bitsPerSample - 1));
                quantizedDiff = Math.Max(minValue, Math.Min(maxValue, quantizedDiff));
                uint unsignedDiff = (uint)(quantizedDiff - minValue);

                int byteIndex = bitPosition / 8;
                int bitOffset = bitPosition % 8;
                for (int bit = 0; bit < _bitsPerSample; bit++)
                {
                    if ((unsignedDiff & (1u << bit)) != 0)
                    {
                        int currentBitPos = bitPosition + bit;
                        compressedData[currentBitPos / 8] |= (byte)(1 << (currentBitPos % 8));
                    }
                }
                bitPosition += _bitsPerSample;
                previousSample = samples[i];
                if (progress != null && i % 1000 == 0) progress.Report((int)((double)i / samples.Length * 100));
            }
            progress?.Report(100);

            Buffer.BlockCopy(compressedData, 0, result, headerSize + 2, compressedData.Length);
            _result = new CompressionResult { OriginalSize = originalSize, CompressedSize = result.Length, CompressionRatio = (double)originalSize / result.Length, ProcessingTime = DateTime.Now - startTime };
            return result;
        }

        public byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels)
        {
            int sampleCount = BitConverter.ToInt32(compressedData, 0);
            int savedBitsPerSample = BitConverter.ToInt32(compressedData, 4);
            int savedQuantStepInt = BitConverter.ToInt32(compressedData, 8);
            int headerSize = BitConverter.ToInt16(compressedData, 12);
            int savedSampleRate = BitConverter.ToInt32(compressedData, 14); 

            _bitsPerSample = savedBitsPerSample;
            _quantStep = savedQuantStepInt / 1000000.0f;

            short firstSample = BitConverter.ToInt16(compressedData, headerSize);
            float previousSample = firstSample / 32767.0f;

            var samples = new float[sampleCount];
            samples[0] = previousSample;
            int bitPosition = 0;
            int dataStartPos = headerSize + 2;

            for (int i = 1; i < sampleCount; i++)
            {
                uint unsignedDiff = 0;
                for (int bit = 0; bit < _bitsPerSample; bit++)
                {
                    int currentBitPos = bitPosition + bit;
                    int byteIndex = dataStartPos + (currentBitPos / 8);
                    int bitOffset = currentBitPos % 8;
                    if ((compressedData[byteIndex] & (1 << bitOffset)) != 0) unsignedDiff |= (1u << bit);
                }
                int minValue = -(1 << (_bitsPerSample - 1));
                int quantizedDiff = (int)unsignedDiff + minValue;
                float difference = quantizedDiff * _quantStep;
                float currentSample = previousSample + difference;
                samples[i] = currentSample;
                previousSample = currentSample;
                bitPosition += _bitsPerSample;
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