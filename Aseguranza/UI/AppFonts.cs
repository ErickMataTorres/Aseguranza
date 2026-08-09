using System;
using System.Drawing;
using System.Drawing.Text;

namespace Aseguranza.UI
{
    public static class AppFonts
    {
        public static readonly string RegularFamily =
            ResolverFuente(
                "Arial Nova",
                "Arial");

        public static readonly string LightFamily =
            ResolverFuente(
                "Arial Nova Light",
                "Arial Nova",
                "Arial");

        public static Font Regular(
            float size,
            FontStyle style = FontStyle.Regular)
        {
            return new Font(
                RegularFamily,
                size,
                style);
        }

        public static Font Light(
            float size,
            FontStyle style = FontStyle.Regular)
        {
            return new Font(
                LightFamily,
                size,
                style);
        }

        private static string ResolverFuente(
            params string[] candidatas)
        {
            using InstalledFontCollection fuentes =
                new InstalledFontCollection();

            foreach (string candidata in candidatas)
            {
                foreach (FontFamily familia in fuentes.Families)
                {
                    if (familia.Name.Equals(
                        candidata,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return familia.Name;
                    }
                }
            }

            return FontFamily.GenericSansSerif.Name;
        }
    }
}