using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SoundShrink_Desktop
{
    public static class RoundedCornersHelper
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public static void ApplyRoundedBorder(this Control control, int radius)
        {
            IntPtr ptr = CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius);
            System.Drawing.Region region = System.Drawing.Region.FromHrgn(ptr);
            control.Region = region;
        }
    }
}