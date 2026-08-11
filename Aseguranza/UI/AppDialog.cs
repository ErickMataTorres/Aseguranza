using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public sealed class AppDialog : Form
    {
        private AppDialog(
            string title,
            string message,
            Color accentColor,
            string primaryText,
            DialogResult primaryResult,
            string? secondaryText = null,
            DialogResult secondaryResult = DialogResult.Cancel)
        {
            Text = title;

            ClientSize = new Size(
                540,
                250);

            StartPosition =
                FormStartPosition.Manual;

            FormBorderStyle =
                FormBorderStyle.FixedDialog;

            MaximizeBox = false;
            MinimizeBox = false;

            ShowInTaskbar = false;

            BackColor =
                AppColors.AppBackground;

            Font =
                AppFonts.Light(10F);

            // =====================================================
            // MENSAJE
            // =====================================================

            Label lblMessage =
                new Label
                {
                    AutoSize = false,

                    Text = message,

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Light(10.5F),

                    Location =
                        new Point(28, 28),

                    Size =
                        new Size(484, 135),

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            // =====================================================
            // BOTONES
            // =====================================================

            Button btnPrimary =
                CrearBoton(
                    primaryText,
                    accentColor,
                    primaryResult);

            btnPrimary.Location =
                new Point(
                    380,
                    185);

            Controls.Add(
                lblMessage);

            Controls.Add(
                btnPrimary);

            if (!string.IsNullOrWhiteSpace(
                    secondaryText))
            {
                Button btnSecondary =
                    CrearBoton(
                        secondaryText,
                        AppColors.Neutral,
                        secondaryResult);

                btnSecondary.Location =
                    new Point(
                        240,
                        185);

                Controls.Add(
                    btnSecondary);

                CancelButton =
                    btnSecondary;

                // Para una operación destructiva,
                // Cancelar recibe el foco inicialmente.
                Shown +=
                    (_, _) =>
                    {
                        btnSecondary.Focus();
                    };
            }
            else
            {
                AcceptButton =
                    btnPrimary;

                CancelButton =
                    btnPrimary;
            }
        }

        private static Button CrearBoton(
            string text,
            Color backgroundColor,
            DialogResult dialogResult)
        {
            Button button =
                new Button
                {
                    Text = text,

                    DialogResult =
                        dialogResult,

                    Size =
                        new Size(
                            120,
                            40),

                    FlatStyle =
                        FlatStyle.Flat,

                    BackColor =
                        backgroundColor,

                    ForeColor =
                        Color.White,

                    Cursor =
                        Cursors.Hand,

                    Font =
                        AppFonts.Regular(
                            10F,
                            FontStyle.Bold),

                    UseVisualStyleBackColor =
                        false
                };

            button.FlatAppearance.BorderSize =
                0;

            RoundedControlHelper
                .ApplyRoundedRegion(
                    button,
                    7);

            return button;
        }

        // =========================================================
        // POSICIONAR SIEMPRE DENTRO DEL MONITOR VISIBLE
        // =========================================================

        private void PosicionarSobre(
            Form owner)
        {
            Rectangle areaVisible =
                Screen
                    .FromControl(owner)
                    .WorkingArea;

            Rectangle ownerBounds =
                owner.Bounds;

            int x =
                ownerBounds.Left +
                ((ownerBounds.Width - Width) / 2);

            int y =
                ownerBounds.Top +
                ((ownerBounds.Height - Height) / 2);

            x =
                Math.Max(
                    areaVisible.Left,
                    Math.Min(
                        x,
                        areaVisible.Right - Width));

            y =
                Math.Max(
                    areaVisible.Top,
                    Math.Min(
                        y,
                        areaVisible.Bottom - Height));

            Location =
                new Point(
                    x,
                    y);
        }

        // =========================================================
        // CONFIRMACIÓN
        // =========================================================

        public static bool Confirm(
            Form owner,
            string title,
            string message,
            string confirmText = "Confirmar")
        {
            using AppDialog dialog =
                new AppDialog(
                    title,
                    message,
                    AppColors.Danger,
                    confirmText,
                    DialogResult.Yes,
                    "Cancelar",
                    DialogResult.No);

            dialog.PosicionarSobre(
                owner);

            return dialog.ShowDialog(
                owner) ==
                DialogResult.Yes;
        }

        // =========================================================
        // INFORMACIÓN
        // =========================================================

        public static void ShowInfo(
            Form owner,
            string title,
            string message)
        {
            using AppDialog dialog =
                new AppDialog(
                    title,
                    message,
                    AppColors.Primary,
                    "Aceptar",
                    DialogResult.OK);

            dialog.PosicionarSobre(
                owner);

            dialog.ShowDialog(
                owner);
        }

        // =========================================================
        // ADVERTENCIA
        // =========================================================

        public static void ShowWarning(
            Form owner,
            string title,
            string message)
        {
            using AppDialog dialog =
                new AppDialog(
                    title,
                    message,
                    Color.FromArgb(
                        217,
                        119,
                        6),
                    "Aceptar",
                    DialogResult.OK);

            dialog.PosicionarSobre(
                owner);

            dialog.ShowDialog(
                owner);
        }

        // =========================================================
        // ERROR
        // =========================================================

        public static void ShowError(
            Form owner,
            string title,
            string message)
        {
            using AppDialog dialog =
                new AppDialog(
                    title,
                    message,
                    AppColors.Danger,
                    "Aceptar",
                    DialogResult.OK);

            dialog.PosicionarSobre(
                owner);

            dialog.ShowDialog(
                owner);
        }
    }
}