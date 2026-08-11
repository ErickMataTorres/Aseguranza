using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public sealed class AppDialog : Form
    {
        private readonly Panel _pnlCard;
        private readonly Label _lblMessage;
        private readonly Button _btnPrimary;
        private readonly Button? _btnSecondary;

        private AppDialog(
            string title,
            string message,
            Color accentColor,
            string primaryText,
            DialogResult primaryResult,
            string? secondaryText = null,
            DialogResult secondaryResult = DialogResult.Cancel)
        {
            // =====================================================
            // FORMULARIO
            // =====================================================

            Text =
                title;

            ClientSize =
                new Size(
                    650,
                    340);

            StartPosition =
                FormStartPosition.Manual;

            FormBorderStyle =
                FormBorderStyle.FixedDialog;

            MaximizeBox =
                false;

            MinimizeBox =
                false;

            ShowInTaskbar =
                false;

            BackColor =
                AppColors.AppBackground;

            Font =
                AppFonts.Light(
                    10.5F);

            Padding =
                new Padding(
                    16);

            // =====================================================
            // TARJETA PRINCIPAL
            // =====================================================

            _pnlCard =
                new Panel
                {
                    Dock =
                        DockStyle.Fill,

                    BackColor =
                        Color.White
                };

            Controls.Add(
                _pnlCard);

            // =====================================================
            // HEADER
            // =====================================================

            Panel pnlHeader =
                new Panel
                {
                    Dock =
                        DockStyle.Top,

                    Height =
                        70,

                    BackColor =
                        AppColors.Primary
                };

            Label lblTitle =
                new Label
                {
                    AutoSize =
                        true,

                    Text =
                        title,

                    ForeColor =
                        Color.White,

                    Font =
                        AppFonts.Regular(
                            15F,
                            FontStyle.Bold),

                    Location =
                        new Point(
                            22,
                            21)
                };

            pnlHeader.Controls.Add(
                lblTitle);

            _pnlCard.Controls.Add(
                pnlHeader);

            // =====================================================
            // MENSAJE
            // =====================================================

            _lblMessage =
                new Label
                {
                    AutoSize =
                        false,

                    Text =
                        message,

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Light(
                            12F),

                    Location =
                        new Point(
                            26,
                            95),

                    Size =
                        new Size(
                            590,
                            125),

                    TextAlign =
                        ContentAlignment.MiddleLeft
                };

            _pnlCard.Controls.Add(
                _lblMessage);

            // =====================================================
            // BOTÓN PRINCIPAL
            // =====================================================

            _btnPrimary =
                CrearBoton(
                    primaryText,
                    accentColor,
                    primaryResult);

            _btnPrimary.Location =
                new Point(
                    478,
                    252);

            _pnlCard.Controls.Add(
                _btnPrimary);

            // =====================================================
            // BOTÓN SECUNDARIO
            // =====================================================

            if (!string.IsNullOrWhiteSpace(
                    secondaryText))
            {
                _btnSecondary =
                    CrearBoton(
                        secondaryText,
                        AppColors.Neutral,
                        secondaryResult);

                _btnSecondary.Location =
                    new Point(
                        330,
                        252);

                _pnlCard.Controls.Add(
                    _btnSecondary);

                AcceptButton =
                    _btnPrimary;

                CancelButton =
                    _btnSecondary;

                Shown +=
                    (_, _) =>
                    {
                        _btnSecondary.Focus();
                    };
            }
            else
            {
                _btnSecondary =
                    null;

                AcceptButton =
                    _btnPrimary;

                CancelButton =
                    _btnPrimary;
            }

            // =====================================================
            // BORDE SUAVE DE LA TARJETA
            // =====================================================

            _pnlCard.Paint +=
                (_, e) =>
                {
                    using Pen pen =
                        new Pen(
                            Color.FromArgb(
                                220,
                                226,
                                236));

                    Rectangle rect =
                        new Rectangle(
                            0,
                            0,
                            _pnlCard.Width - 1,
                            _pnlCard.Height - 1);

                    e.Graphics.DrawRectangle(
                        pen,
                        rect);
                };
        }

        // =========================================================
        // CREAR BOTÓN
        // =========================================================

        private static Button CrearBoton(
            string text,
            Color backgroundColor,
            DialogResult dialogResult)
        {
            Button button =
                new Button
                {
                    Text =
                        text,

                    DialogResult =
                        dialogResult,

                    Size =
                        new Size(
                            128,
                            44),

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
                            10.5F,
                            FontStyle.Bold),

                    UseVisualStyleBackColor =
                        false
                };

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

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                8);

            return button;
        }

        // =========================================================
        // POSICIONAR SOBRE OWNER
        // =========================================================

        private void PosicionarSobre(
            Form owner)
        {
            Rectangle areaVisible =
                Screen
                    .FromControl(
                        owner)
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
        // CONFIRMAR
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