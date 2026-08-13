using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public static class FormStyler
    {
        private const string CompanyLogoResourceName =
            "Aseguranza.Resources.LogoContecComprimida.png";

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

            AplicarIconoAplicacion(
                form);

            CreateFooter(
                form);
        }

        // =========================================================
        // ICONO DE APLICACIÓN
        // =========================================================

        private static void AplicarIconoAplicacion(
            Form form)
        {
            try
            {
                Icon? applicationIcon =
                    Icon.ExtractAssociatedIcon(
                        Application.ExecutablePath);

                if (applicationIcon is null)
                {
                    return;
                }

                form.Icon =
                    (Icon)applicationIcon.Clone();

                applicationIcon.Dispose();

                form.ShowIcon =
                    true;
            }
            catch
            {
                // La ausencia del icono nunca debe impedir
                // que una ventana pueda abrirse.
            }
        }

        // =========================================================
        // CABECERA
        // =========================================================

        public static Panel CreateHeader(
            Control parent,
            string title,
            string subtitle,
            int height = 110,
            int titleX = 32,
            int titleY = 14,
            int subtitleX = 34,
            int subtitleY = 51,
            bool showCompanyLogo = true)
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
                            FontStyle.Bold),

                    BackColor =
                        Color.Transparent
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
                        AppFonts.Light(10F),

                    BackColor =
                        Color.Transparent
                };

            header.Controls.Add(
                titleLabel);

            header.Controls.Add(
                subtitleLabel);

            if (showCompanyLogo)
            {
                AddCompanyLogo(
                    header,
                    height);
            }

            parent.Controls.Add(
                header);

            return header;
        }

        // =========================================================
        // LOGO DE LA EMPRESA
        // =========================================================

        private static void AddCompanyLogo(
            Panel header,
            int headerHeight)
        {
            Image? logo =
                LoadCompanyLogo();

            if (logo is null)
            {
                return;
            }

            int containerSize =
                Math.Min(
                    88,
                    Math.Max(
                        64,
                        headerHeight - 12));

            Panel logoContainer =
                new Panel
                {
                    Name =
                        "pnlLogoEmpresa",

                    Size =
                        new Size(
                            containerSize,
                            containerSize),

                    Location =
                        new Point(
                            header.ClientSize.Width -
                            containerSize -
                            24,
                            Math.Max(
                                8,
                                (headerHeight -
                                 containerSize) / 2)),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Right,

                    BackColor =
                        Color.White
                };

            RoundedControlHelper.ApplyRoundedRegion(
                logoContainer,
                12);

            PictureBox pictureLogo =
                new PictureBox
                {
                    Name =
                        "pbLogoEmpresa",

                    Location =
                        new Point(
                            6,
                            6),

                    Size =
                        new Size(
                            containerSize - 12,
                            containerSize - 12),

                    Image =
                        logo,

                    SizeMode =
                        PictureBoxSizeMode.Zoom,

                    BackColor =
                        Color.White,

                    TabStop =
                        false
                };

            logoContainer.Controls.Add(
                pictureLogo);

            header.Controls.Add(
                logoContainer);

            logoContainer.BringToFront();
        }

        private static Image? LoadCompanyLogo()
        {
            try
            {
                Assembly assembly =
                    Assembly.GetExecutingAssembly();

                using Stream? stream =
                    assembly.GetManifestResourceStream(
                        CompanyLogoResourceName);

                if (stream is null)
                {
                    return null;
                }

                using Image temporaryImage =
                    Image.FromStream(
                        stream);

                return new Bitmap(
                    temporaryImage);
            }
            catch
            {
                // La ausencia del logo nunca debe impedir
                // que una ventana pueda abrirse.
                return null;
            }
        }

        // =========================================================
        // PIE DE APLICACIÓN
        // =========================================================

        public static void CreateFooter(
            Form form)
        {
            string footerText =
                $"Desarrollado por Erick Mata | Versión {GetApplicationVersion()}";

            const string footerLabelName =
                "lblAppFooter";

            const string footerStatusName =
                "tslAppFooter";

            void EnsureFooter()
            {
                if (form.IsDisposed ||
                    form.Disposing)
                {
                    return;
                }

                StatusStrip? statusStrip =
                    form.Controls
                        .OfType<StatusStrip>()
                        .FirstOrDefault(
                            control =>
                                control.Visible);

                if (statusStrip is not null)
                {
                    // Si previamente se había creado el footer como Label,
                    // lo ocultamos para no duplicar el texto.
                    if (form.Controls[
                            footerLabelName]
                        is Label existingLabel)
                    {
                        existingLabel.Visible =
                            false;
                    }

                    ToolStripItem? existingItem =
                        statusStrip.Items[
                            footerStatusName];

                    if (existingItem is
                        ToolStripStatusLabel existingStatus)
                    {
                        existingStatus.Text =
                            footerText;

                        return;
                    }

                    ToolStripStatusLabel footerStatus =
                        new ToolStripStatusLabel
                        {
                            Name =
                                footerStatusName,

                            Text =
                                footerText,

                            ForeColor =
                                AppColors.TextSecondary,

                            Spring =
                                true,

                            TextAlign =
                                ContentAlignment.MiddleRight
                        };

                    statusStrip.Items.Add(
                        footerStatus);

                    return;
                }

                Label footerLabel;

                if (form.Controls[
                        footerLabelName]
                    is Label existingFooter)
                {
                    footerLabel =
                        existingFooter;

                    footerLabel.Visible =
                        true;

                    footerLabel.Text =
                        footerText;
                }
                else
                {
                    footerLabel =
                        new Label
                        {
                            Name =
                                footerLabelName,

                            AutoSize =
                                true,

                            Text =
                                footerText,

                            Anchor =
                                AnchorStyles.Right |
                                AnchorStyles.Bottom,

                            ForeColor =
                                AppColors.TextSecondary,

                            Font =
                                AppFonts.Light(
                                    9F),

                            BackColor =
                                Color.Transparent
                        };

                    form.Controls.Add(
                        footerLabel);
                }

                footerLabel.Location =
                    new Point(
                        Math.Max(
                            12,
                            form.ClientSize.Width -
                            footerLabel.Width -
                            14),
                        Math.Max(
                            0,
                            form.ClientSize.Height -
                            footerLabel.Height -
                            5));

                footerLabel.BringToFront();
            }

            // Lo intentamos inmediatamente para las ventanas normales.
            EnsureFooter();

            // Algunas ventanas ejecutan Controls.Clear() después de ApplyBase().
            // Shown ocurre después de terminar de construir la interfaz,
            // por lo que aquí restauramos el footer si fue removido.
            form.Shown +=
                (_, _) =>
                {
                    EnsureFooter();
                };

            form.SizeChanged +=
                (_, _) =>
                {
                    EnsureFooter();
                };

            form.ControlAdded +=
                (_, _) =>
                {
                    if (!form.IsHandleCreated ||
                        form.IsDisposed ||
                        form.Disposing)
                    {
                        return;
                    }

                    // Evita intervenir mientras todavía se está creando
                    // la colección inicial de controles. Al mostrarse la
                    // ventana se hace la sincronización definitiva.
                    if (form.Visible)
                    {
                        EnsureFooter();
                    }
                };
        }

        private static string GetApplicationVersion()
        {
            Version? version =
                Assembly
                    .GetEntryAssembly()?
                    .GetName()
                    .Version;

            if (version is null)
            {
                return "1.0.0";
            }

            return $"{version.Major}.{version.Minor}.{version.Build}";
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
