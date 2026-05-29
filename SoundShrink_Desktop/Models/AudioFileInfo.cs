using System;

namespace SoundShrink_Desktop.Models
{
    public class AudioFileInfo
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public TimeSpan Duration { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        public int BitsPerSample { get; set; }
        public int AverageBytesPerSecond { get; set; }
    }
}