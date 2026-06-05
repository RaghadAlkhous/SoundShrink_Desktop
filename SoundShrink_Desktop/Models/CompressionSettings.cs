using System;

namespace SoundShrink_Desktop.Models
{
    /// <summary>
    /// كلاس يحتوي على إعدادات الضغط لكل الخوارزميات
    /// </summary>
    public class CompressionSettings
    {
        // === إعدادات مشتركة ===

        /// <summary>
        /// مستويات التكميم (لـ Nonlinear Quantization)
        /// القيم: 16, 32, 64, 128, 256, 512, 1024
        /// </summary>
        public int QuantizationLevels { get; set; } = 256;

        /// <summary>
        /// عدد البتات لكل عينة (لـ DPCM)
        /// القيم: 4, 8, 12, 16, 24, 32
        /// </summary>
        public int BitsPerSample { get; set; } = 16;

        // === إعدادات Delta Modulation ===

        /// <summary>
        /// حجم الخطوة (لـ Delta Modulation)
        /// القيم: 0.01 إلى 1.0
        /// </summary>
        public double StepSize { get; set; } = 0.1;

        // === إعدادات Adaptive Delta Modulation ===

        /// <summary>
        /// حجم الخطوة الأولي (لـ Adaptive Delta Modulation)
        /// القيم: 0.01 إلى 1.0
        /// </summary>
        public double InitialStepSize { get; set; } = 0.1;

        /// <summary>
        /// معامل مضاعفة حجم الخطوة (لـ Adaptive Delta Modulation)
        /// القيم: 1.1 إلى 2.0
        /// </summary>
        public double StepSizeMultiplier { get; set; } = 1.5;

        // === إعدادات Predictive Differential Coding ===

        /// <summary>
        /// معامل التنبؤ (لـ Predictive Differential Coding)
        /// القيم: 0.1 إلى 1.0
        /// </summary>
        public double PredictionCoefficient { get; set; } = 0.9;

        /// <summary>
        /// إنشاء إعدادات افتراضية
        /// </summary>
        public static CompressionSettings Default => new CompressionSettings();
    }
}