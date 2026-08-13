using System;
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

                    e.Graphics.PixelOffsetMode =
                        PixelOffsetMode.HighQuality;

                    Color currentColor =
                        input.Focused
                            ? activeBorder
                            : normalBorder;

                    float borderWidth =
                        input.Focused
                            ? 1.8F
                            : 1.25F;

                    // Dibujamos el borde un píxel hacia adentro.
                    // Antes se pintaba exactamente sobre el límite del Panel
                    // y GDI+ recortaba parte de los lados superior/izquierdo,
                    // especialmente cuando el control no tenía el foco.
                    Rectangle rectangle =
                        new Rectangle(
                            1,
                            1,
                            Math.Max(
                                1,
                                container.ClientSize.Width - 3),
                            Math.Max(
                                1,
                                container.ClientSize.Height - 3));

                    using GraphicsPath path =
                        RoundedControlHelper
                            .CreateRoundedPath(
                                rectangle,
                                Math.Max(
                                    1,
                                    radius - 1));

                    using Pen pen =
                        new Pen(
                            currentColor,
                            borderWidth);

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

            container.Resize +=
                (_, _) =>
                {
                    RoundedControlHelper.ApplyRoundedRegion(
                        container,
                        radius);

                    container.Invalidate();
                };

            input.EnabledChanged +=
                (_, _) =>
                {
                    container.Invalidate();
                };

            // Fuerza un primer repintado completo cuando el estilo
            // acaba de aplicarse, sin esperar a que el usuario haga foco.
            container.Invalidate();
        }
    }
}