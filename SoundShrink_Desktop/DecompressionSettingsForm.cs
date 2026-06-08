using System;
using System.Drawing;
using System.Windows.Forms;
using SoundShrink_Desktop.Models;

namespace SoundShrink_Desktop
{
    public partial class DecompressionSettingsForm : Form
    {
        private readonly string _algorithmName;
        private readonly CompressionSettings _originalSettings;
        private readonly int _originalSampleRate;
        private readonly int _originalChannels;
        private readonly int _originalBitsPerSample;

        public bool UseOriginalSettings { get; private set; } = true;

        public int SampleRate =>
            UseOriginalSettings ? _originalSampleRate : 44100;

        public int Channels =>
            UseOriginalSettings ? _originalChannels : 2;

        public int BitsPerSample =>
            UseOriginalSettings ? _originalBitsPerSample : 16;

        public CompressionSettings AlgorithmSettings
        {
            get
            {
                if (UseOriginalSettings)
                    return _originalSettings;

                var defaultSettings = new CompressionSettings();

                if (_algorithmName.Contains("Nonlinear"))
                {
                    defaultSettings.QuantizationLevels = 256;
                }
                else if (_algorithmName.Contains("DPCM"))
                {
                    defaultSettings.BitsPerSample = 16;
                }
                else if (_algorithmName.Contains("Predictive"))
                {
                    defaultSettings.PredictionCoefficient = 0.90;
                }
                else if (_algorithmName.Contains("Adaptive Delta"))
                {
                    defaultSettings.InitialStepSize = 0.1;
                    defaultSettings.StepSizeMultiplier = 1.5;
                }
                else if (_algorithmName.Contains("Delta Modulation"))
                {
                    defaultSettings.StepSize = 0.1;
                }

                return defaultSettings;
            }
        }

        public DecompressionSettingsForm(
            string algorithmName,
            int sampleRate,
            int channels,
            int bitsPerSample,
            CompressionSettings originalSettings)
        {
            _algorithmName = algorithmName;
            _originalSampleRate = sampleRate;
            _originalChannels = channels;
            _originalBitsPerSample = bitsPerSample;
            _originalSettings = originalSettings ?? new CompressionSettings();

            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Text = $"Decompression Options - {_algorithmName}";

            algoLabel.Text = $"Algorithm: {_algorithmName}";

            settingsInfo.Text =
                $"Original Settings:\n" +
                $"• Sample Rate: {_originalSampleRate} Hz\n" +
                $"• Channels: {(_originalChannels == 1 ? "Mono" : "Stereo")}\n" +
                $"• Bits Per Sample: {_originalBitsPerSample}\n" +
                GetAlgorithmSettingsText();
        }

        private string GetAlgorithmSettingsText()
        {
            if (_algorithmName.Contains("Nonlinear"))
            {
                return $"• Quantization Levels: {_originalSettings.QuantizationLevels}";
            }
            else if (_algorithmName.Contains("DPCM"))
            {
                return $"• Bits Per Sample: {_originalSettings.BitsPerSample}";
            }
            else if (_algorithmName.Contains("Predictive"))
            {
                return $"• Prediction Coefficient: {_originalSettings.PredictionCoefficient:F2}";
            }
            else if (_algorithmName.Contains("Adaptive Delta"))
            {
                return
                    $"• Initial Step Size: {_originalSettings.InitialStepSize:F3}\n" +
                    $"• Step Multiplier: {_originalSettings.StepSizeMultiplier:F2}";
            }
            else if (_algorithmName.Contains("Delta Modulation"))
            {
                return $"• Step Size: {_originalSettings.StepSize:F3}";
            }

            return string.Empty;
        }

        private void radioOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOriginal.Checked)
            {
                UseOriginalSettings = true;
            }
        }

        private void radioDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDefault.Checked)
            {
                UseOriginalSettings = false;
            }
        }
    }
}