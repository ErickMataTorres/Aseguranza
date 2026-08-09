using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class ButtonStyler
    {
        public static void Apply(
            Button button,
            string text,
            Color backgroundColor,
            string icon,
            int width = 138,
            int height = 42)
        {
            button.Text =
                text;

            button.Size =
                new Size(
                    width,
                    height);

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(
                    backgroundColor,
                    0.05F);

            button.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(
                    backgroundColor,
                    0.10F);

            button.BackColor =
                backgroundColor;

            button.ForeColor =
                Color.White;

            button.UseVisualStyleBackColor =
                false;

            button.Cursor =
                Cursors.Hand;

            button.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            button.Image =
                AppIcons.CreateBitmap(
                    icon,
                    Color.White,
                    16);

            button.ImageAlign =
                ContentAlignment.MiddleLeft;

            button.TextAlign =
                ContentAlignment.MiddleCenter;

            button.TextImageRelation =
                TextImageRelation.ImageBeforeText;

            button.Padding =
                new Padding(
                    12,
                    0,
                    12,
                    0);

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                7);
        }

        public static void UpdateEnabledState(
            Button button,
            Color activeColor)
        {
            if (button.Enabled)
            {
                button.BackColor =
                    activeColor;

                button.ForeColor =
                    Color.White;

                return;
            }

            button.BackColor =
                AppColors.DisabledBackground;

            button.ForeColor =
                AppColors.DisabledText;
        }
    }
}