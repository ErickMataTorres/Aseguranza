using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class RoundedControlHelper
    {
        public static void ApplyRoundedRegion(
            Control control,
            int radius)
        {
            ArgumentNullException.ThrowIfNull(control);

            void UpdateRegion()
            {
                if (control.Width <= 0 ||
                    control.Height <= 0)
                {
                    return;
                }

                Rectangle rectangle =
                    new Rectangle(
                        0,
                        0,
                        control.Width - 1,
                        control.Height - 1);

                using GraphicsPath path =
                    CreateRoundedPath(
                        rectangle,
                        radius);

                Region? previousRegion =
                    control.Region;

                control.Region =
                    new Region(path);

                previousRegion?.Dispose();
            }

            UpdateRegion();

            control.Resize +=
                (_, _) =>
                {
                    UpdateRegion();
                };
        }

        public static GraphicsPath CreateRoundedPath(
            Rectangle rectangle,
            int radius)
        {
            GraphicsPath path =
                new GraphicsPath();

            if (rectangle.Width <= 0 ||
                rectangle.Height <= 0)
            {
                return path;
            }

            int safeRadius =
                Math.Max(
                    0,
                    Math.Min(
                        radius,
                        Math.Min(
                            rectangle.Width,
                            rectangle.Height) / 2));

            int diameter =
                safeRadius * 2;

            if (diameter <= 0)
            {
                path.AddRectangle(
                    rectangle);

                return path;
            }

            Rectangle arc =
                new Rectangle(
                    rectangle.X,
                    rectangle.Y,
                    diameter,
                    diameter);

            path.AddArc(
                arc,
                180,
                90);

            arc.X =
                rectangle.Right -
                diameter;

            path.AddArc(
                arc,
                270,
                90);

            arc.Y =
                rectangle.Bottom -
                diameter;

            path.AddArc(
                arc,
                0,
                90);

            arc.X =
                rectangle.Left;

            path.AddArc(
                arc,
                90,
                90);

            path.CloseFigure();

            return path;
        }
    }
}