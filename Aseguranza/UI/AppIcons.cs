using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class AppIcons
    {
        // =========================================================
        // GLIFOS
        // =========================================================

        public const string Search =
            "\uE721";

        public const string Add =
            "\uE710";

        public const string Edit =
            "\uE70F";

        public const string Delete =
            "\uE74D";

        public const string Back =
            "\uE72B";

        public const string Save =
            "\uE74E";

        // =========================================================
        // FUENTE
        // =========================================================

        public static readonly string FontFamilyName =
            ResolverFuenteIconos();

        private static string ResolverFuenteIconos()
        {
            using InstalledFontCollection fuentes =
                new InstalledFontCollection();

            foreach (FontFamily familia in fuentes.Families)
            {
                if (familia.Name.Equals(
                    "Segoe Fluent Icons",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return familia.Name;
                }
            }

            foreach (FontFamily familia in fuentes.Families)
            {
                if (familia.Name.Equals(
                    "Segoe MDL2 Assets",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return familia.Name;
                }
            }

            return "Segoe UI Symbol";
        }

        // =========================================================
        // CREACIÓN DEL ICONO
        // =========================================================

        public static Bitmap CreateBitmap(
            string glyph,
            Color color,
            int fontSize = 16,
            int canvasSize = 24)
        {
            Bitmap bitmap =
                new Bitmap(
                    canvasSize,
                    canvasSize);

            using Graphics graphics =
                Graphics.FromImage(bitmap);

            graphics.Clear(
                Color.Transparent);

            graphics.TextRenderingHint =
                TextRenderingHint.AntiAliasGridFit;

            using Font fuente =
                new Font(
                    FontFamilyName,
                    fontSize,
                    FontStyle.Regular,
                    GraphicsUnit.Pixel);

            TextRenderer.DrawText(
                graphics,
                glyph,
                fuente,
                new Rectangle(
                    0,
                    0,
                    bitmap.Width,
                    bitmap.Height),
                color,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding);

            return bitmap;
        }
    }
}