using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SoundShrink_Desktop.Models
{
    public class CompressionMonitor
    {
        public int ProgressPercentage { get; set; }
        public double CompressionRatio { get; set; }
        public double ProcessingSpeedMBps { get; set; }
        public long ProcessedBytes { get; set; }
        public long TotalBytes { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public TimeSpan EstimatedRemaining { get; set; }
        public bool IsCancelled { get; set; }
    }
}