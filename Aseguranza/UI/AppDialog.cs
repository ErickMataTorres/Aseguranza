using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public sealed class AppDialog : Form
    {
        // =========================================================
        // TIPOS DE DIÁLOGO
        // =========================================================

        private enum DialogType
        {
            Information,
            Confirmation,
            Warning,
            Error
        }

        // =========================================================
        // CAMPOS
        // =========================================================

        private readonly Panel _pnlCard;
        private readonly Label _lblMessage;
        private readonly Button _btnPrimary;
        private readonly Button? _btnSecondary;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        private AppDialog(
            string title,
            string message,
            DialogType dialogType,
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
                string.Empty;

            ClientSize =
                new Size(
                    650,
                    370);

            StartPosition =
                FormStartPosition.Manual;

            FormBorderStyle =
                FormBorderStyle.None;

            MaximizeBox =
                false;

            MinimizeBox =
                false;

            ShowInTaskbar =
                false;

            KeyPreview =
                true;

            BackColor =
                AppColors.BorderMedium;

            Font =
                AppFonts.Light(
                    10F);

            Padding =
                new Padding(
                    1);

            // =====================================================
            // ESC
            // =====================================================

            KeyDown +=
                (_, e) =>
                {
                    if (e.KeyCode == Keys.Escape &&
                        CancelButton is Button btnCancel)
                    {
                        btnCancel.PerformClick();

                        e.Handled =
                            true;
                    }
                };

            // =====================================================
            // TARJETA PRINCIPAL
            // =====================================================

            _pnlCard =
                new Panel
                {
                    Dock =
                        DockStyle.Fill,

                    BackColor =
                        AppColors.CardBackground
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
                        58,

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
                            13F,
                            FontStyle.Bold),

                    Location =
                        new Point(
                            22,
                            17),

                    BackColor =
                        Color.Transparent
                };

            pnlHeader.Controls.Add(
                lblTitle);

            _pnlCard.Controls.Add(
                pnlHeader);

            // =====================================================
            // ICONO
            // =====================================================

            Panel pnlIcon =
                CrearIcono(
                    dialogType);

            pnlIcon.Location =
                new Point(
                    299,
                    84);

            _pnlCard.Controls.Add(
                pnlIcon);

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
                        AppFonts.Regular(
                            13F),

                    Location =
                        new Point(
                            40,
                            148),

                    Size =
                        new Size(
                            570,
                            105),

                    TextAlign =
                        ContentAlignment.MiddleCenter,

                    BackColor =
                        Color.Transparent
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

                _pnlCard.Controls.Add(
                    _btnSecondary);

                _btnSecondary.Location =
                    new Point(
                        192,
                        295);

                _btnPrimary.Location =
                    new Point(
                        336,
                        295);

                AcceptButton =
                    _btnPrimary;

                CancelButton =
                    _btnSecondary;

                // En una confirmación destructiva,
                // Cancelar recibe el foco inicialmente.
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

                _btnPrimary.Location =
                    new Point(
                        261,
                        295);

                AcceptButton =
                    _btnPrimary;

                CancelButton =
                    _btnPrimary;
            }
        }

        // =========================================================
        // CREAR ICONO
        // =========================================================

        private static Panel CrearIcono(
            DialogType dialogType)
        {
            string simbolo;

            Color colorPrincipal;

            Color colorFondo;

            switch (dialogType)
            {
                case DialogType.Confirmation:

                    simbolo =
                        "?";

                    colorPrincipal =
                        AppColors.Primary;

                    colorFondo =
                        Color.FromArgb(
                            224,
                            231,
                            255);

                    break;

                case DialogType.Warning:

                    simbolo =
                        "!";

                    colorPrincipal =
                        Color.FromArgb(
                            202,
                            138,
                            4);

                    colorFondo =
                        Color.FromArgb(
                            254,
                            243,
                            199);

                    break;

                case DialogType.Error:

                    simbolo =
                        "×";

                    colorPrincipal =
                        AppColors.Danger;

                    colorFondo =
                        Color.FromArgb(
                            254,
                            226,
                            226);

                    break;

                default:

                    simbolo =
                        "✓";

                    colorPrincipal =
                        AppColors.Primary;

                    colorFondo =
                        Color.FromArgb(
                            224,
                            231,
                            255);

                    break;
            }

            Panel panel =
                new Panel
                {
                    Size =
                        new Size(
                            52,
                            52),

                    BackColor =
                        Color.Transparent
                };

            panel.Paint +=
                (_, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    Rectangle circulo =
                        new Rectangle(
                            1,
                            1,
                            panel.Width - 3,
                            panel.Height - 3);

                    using SolidBrush fondoBrush =
                        new SolidBrush(
                            colorFondo);

                    e.Graphics.FillEllipse(
                        fondoBrush,
                        circulo);

                    using Font fuente =
                        AppFonts.Regular(
                            20F,
                            FontStyle.Bold);

                    TextRenderer.DrawText(
                        e.Graphics,
                        simbolo,
                        fuente,
                        circulo,
                        colorPrincipal,
                        TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.NoPadding);
                };

            return panel;
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
                            11F,
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
                    DialogType.Confirmation,
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
                    DialogType.Information,
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
            Color warningColor =
                Color.FromArgb(
                    217,
                    119,
                    6);

            using AppDialog dialog =
                new AppDialog(
                    title,
                    message,
                    DialogType.Warning,
                    warningColor,
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
                    DialogType.Error,
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