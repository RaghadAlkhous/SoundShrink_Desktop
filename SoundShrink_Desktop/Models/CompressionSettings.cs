using System;

namespace SoundShrink_Desktop.Models
{
    public class CompressionSettings
    {
        public int QuantizationLevels { get; set; } = 256;

        public int BitsPerSample { get; set; } = 16;

        public int SampleRate { get; set; } = 0;

        public double StepSize { get; set; } = 0.1;

        public double InitialStepSize { get; set; } = 0.1;

        public double StepSizeMultiplier { get; set; } = 1.5;

        public double PredictionCoefficient { get; set; } = 0.9;

        public static CompressionSettings Default => new CompressionSettings();
    }
}