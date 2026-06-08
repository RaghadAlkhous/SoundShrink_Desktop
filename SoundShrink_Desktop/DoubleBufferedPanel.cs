using System.Drawing;
using System.Windows.Forms;

namespace SoundShrink_Desktop
{
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            this.BackColor = Color.FromArgb(20, 20, 25);
        }
    }
}
