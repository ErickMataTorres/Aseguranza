using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class FormStyler
    {
        // =========================================================
        // FORMULARIO BASE
        // =========================================================

        public static void ApplyBase(
            Form form,
            string title,
            Size clientSize)
        {
            form.Text =
                title;

            form.ClientSize =
                clientSize;

            form.BackColor =
                AppColors.AppBackground;

            form.Font =
                AppFonts.Light(10F);

            form.FormBorderStyle =
                FormBorderStyle.FixedSingle;

            form.MaximizeBox =
                false;

            form.MinimizeBox =
                false;

            form.StartPosition =
                FormStartPosition.CenterParent;
        }

        // =========================================================
        // CABECERA
        // =========================================================

        public static Panel CreateHeader(
            Control parent,
            string title,
            string subtitle,
            int height = 88,
            int titleX = 32,
            int titleY = 14,
            int subtitleX = 34,
            int subtitleY = 51)
        {
            Panel header =
                new Panel
                {
                    Name =
                        "pnlCabeceraVisual",

                    Location =
                        new Point(
                            0,
                            0),

                    Size =
                        new Size(
                            parent.ClientSize.Width,
                            height),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right,

                    BackColor =
                        AppColors.Primary
                };

            Label titleLabel =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            titleX,
                            titleY),

                    Text =
                        title,

                    ForeColor =
                        Color.White,

                    Font =
                        AppFonts.Regular(
                            19F,
                            FontStyle.Bold)
                };

            Label subtitleLabel =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            subtitleX,
                            subtitleY),

                    Text =
                        subtitle,

                    ForeColor =
                        AppColors.HeaderSubtitle,

                    Font =
                        AppFonts.Light(10F)
                };

            header.Controls.Add(
                titleLabel);

            header.Controls.Add(
                subtitleLabel);

            parent.Controls.Add(
                header);

            return header;
        }

        // =========================================================
        // TARJETA BLANCA
        // =========================================================

        public static Panel CreateCard(
            Control parent,
            Point location,
            Size size,
            int radius = 12,
            AnchorStyles anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right)
        {
            Panel card =
                new Panel
                {
                    Location =
                        location,

                    Size =
                        size,

                    BackColor =
                        AppColors.CardBackground,

                    Anchor =
                        anchor
                };

            RoundedControlHelper.ApplyRoundedRegion(
                card,
                radius);

            parent.Controls.Add(
                card);

            return card;
        }
    }
}