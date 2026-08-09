using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class InputStyler
    {
        public static void ApplyOutlinedInput(
            Panel container,
            Control input,
            int radius = 7,
            Color? borderColor = null,
            Color? focusColor = null)
        {
            Color normalBorder =
                borderColor ??
                AppColors.InputBorder;

            Color activeBorder =
                focusColor ??
                AppColors.Primary;

            container.BackColor =
                Color.White;

            RoundedControlHelper.ApplyRoundedRegion(
                container,
                radius);

            container.Paint +=
                (_, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    Rectangle rectangle =
                        new Rectangle(
                            0,
                            0,
                            container.Width - 1,
                            container.Height - 1);

                    using GraphicsPath path =
                        RoundedControlHelper
                            .CreateRoundedPath(
                                rectangle,
                                radius);

                    Color currentColor =
                        input.Focused
                            ? activeBorder
                            : normalBorder;

                    float width =
                        input.Focused
                            ? 1.8F
                            : 1F;

                    using Pen pen =
                        new Pen(
                            currentColor,
                            width);

                    e.Graphics.DrawPath(
                        pen,
                        path);
                };

            input.Enter +=
                (_, _) =>
                {
                    container.Invalidate();
                };

            input.Leave +=
                (_, _) =>
                {
                    container.Invalidate();
                };

            container.Click +=
                (_, _) =>
                {
                    input.Focus();
                };
        }
    }
}