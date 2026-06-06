using System;

namespace SoundShrink_Desktop.Algorithms
{
    public interface ICompressionAlgorithm
    {
        string AlgorithmName { get; }

        byte[] Compress(byte[] audioData, int sampleRate, int bitsPerSample, int channels, IProgress<int> progress = null);

        byte[] Decompress(byte[] compressedData, int sampleRate, int bitsPerSample, int channels);

        CompressionResult GetCompressionStats();
    }

    public class CompressionResult
    {
        public long OriginalSize { get; set; }
        public long CompressedSize { get; set; }
        public double CompressionRatio { get; set; }
        public TimeSpan ProcessingTime { get; set; }
    }
}