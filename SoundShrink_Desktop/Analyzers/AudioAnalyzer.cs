using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace SoundShrink_Desktop.Analyzers
{
    public class AudioAnalyzer
    {
        private const int FFT_SIZE = 1024;
        private readonly float[] _fftBuffer;
        private int _bufferPosition;
        private readonly object _lockObject = new object();

        public List<float> WaveSamples { get; private set; } = new List<float>();
        public List<double> SpectrumData { get; private set; } = new List<double>();

        public int MaxSamples { get; set; } = 2000;

        public AudioAnalyzer()
        {
            _fftBuffer = new float[FFT_SIZE];
        }

        public void AddSample(float sample)
{
    lock (_lockObject)
    {
        if (WaveSamples.Count >= MaxSamples * 2)
        {
            WaveSamples.RemoveAt(0);
        }
        
        WaveSamples.Add(sample);

        _fftBuffer[_bufferPosition] = sample;
        _bufferPosition++;
        
        if (_bufferPosition >= FFT_SIZE)
        {
            _bufferPosition = 0;
            CalculateSpectrum();
        }
    }
}
        private void CalculateSpectrum()
        {
            Complex[] fftData = new Complex[FFT_SIZE];
            for (int i = 0; i < FFT_SIZE; i++)
            {
                fftData[i] = new Complex(_fftBuffer[i], 0);
            }

            Fourier.Forward(fftData, FourierOptions.Default);

            SpectrumData.Clear();
            int binsCount = 48;
            int samplesPerBin = (FFT_SIZE / 2) / binsCount;

            for (int i = 0; i < binsCount; i++)
            {
                double avgMagnitude = 0;
                for (int j = 0; j < samplesPerBin; j++)
                {
                    int index = i * samplesPerBin + j;
                    if (index < fftData.Length)
                        avgMagnitude += fftData[index].Magnitude;
                }
                avgMagnitude /= samplesPerBin;
                SpectrumData.Add(Math.Log10(avgMagnitude + 1) * 220);
            }
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                WaveSamples.Clear();
                SpectrumData.Clear();
                Array.Clear(_fftBuffer, 0, _fftBuffer.Length);
                _bufferPosition = 0;
            }
        }

        public List<float> GetWaveSamples()
        {
            lock (_lockObject)
            {
                return new List<float>(WaveSamples);
            }
        }

        public List<double> GetSpectrumData()
        {
            lock (_lockObject)
            {
                return new List<double>(SpectrumData);
            }
        }
    }
}