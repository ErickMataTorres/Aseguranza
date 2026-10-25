using Aseguranza.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();

            DoubleBuffered =
                true;

            AplicarEstiloVisual();
        }

        // =========================================================
        // INTERFAZ PRINCIPAL
        // =========================================================

        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                "Sistema de Certificaciones",
                new Size(
                    1040,
                    650));

            StartPosition =
                FormStartPosition.CenterScreen;

            // Conservamos el MenuStrip original del Designer,
            // pero queda oculto porque ahora utilizaremos
            // el dashboard como navegación principal.

            menuStrip1.Visible =
                false;

            MainMenuStrip =
                null;

            // =====================================================
            // CABECERA
            // =====================================================

            FormStyler.CreateHeader(
                this,
                "Sistema de Certificaciones",
                "Administración y control de certificaciones del personal",
                height: 110,
                titleX: 38,
                titleY: 20,
                subtitleX: 40,
                subtitleY: 61);

            // =====================================================
            // CONTENEDOR PRINCIPAL
            // =====================================================

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        125),
                    new Size(
                        1000,
                        505),
                    radius: 14);

            // =====================================================
            // ACCESO PRINCIPAL
            // =====================================================

            pnlContenido.Controls.Add(
                CrearTituloSeccion(
                    "Acceso principal",
                    26,
                    20));

            pnlContenido.Controls.Add(
                CrearSubtituloSeccion(
                    "Accede rápidamente a las funciones principales del sistema.",
                    26,
                    47));

            // =====================================================
            // TRABAJADORES
            // =====================================================

            pnlContenido.Controls.Add(
                CrearTarjetaNavegacion(
                    "Trabajadores",
                    "Administra los datos generales, asignaciones y fotografía del personal.",
                    new Point(
                        26,
                        82),
                    trabajadorToolStripMenuItem_Click));

            // =====================================================
            // CERTIFICACIONES
            // =====================================================

            pnlContenido.Controls.Add(
                CrearTarjetaNavegacion(
                    "Certificaciones",
                    "Consulta el estado del personal y administra sus certificaciones.",
                    new Point(
                        350,
                        82),
                    btnCertificaciones_Click));

            // =====================================================
            // VERIFICACIONES
            // =====================================================

            pnlContenido.Controls.Add(
                CrearTarjetaNavegacion(
                    "Verificaciones",
                    "Consulta certificaciones individuales y visualiza credenciales.",
                    new Point(
                        674,
                        82),
                    verificadorToolStripMenuItem_Click));

            // =====================================================
            // ADMINISTRACIÓN
            // =====================================================

            pnlContenido.Controls.Add(
                CrearTituloSeccion(
                    "Administración",
                    26,
                    235));

            pnlContenido.Controls.Add(
                CrearSubtituloSeccion(
                    "Configura los catálogos utilizados por el sistema.",
                    26,
                    262));

            // =====================================================
            // PRIMERA FILA
            // =====================================================

            pnlContenido.Controls.Add(
                CrearBotonAdministracion(
                    "Localidades",
                    new Point(
                        26,
                        301),
                    certificarToolStripMenuItem_Click));

            pnlContenido.Controls.Add(
                CrearBotonAdministracion(
                    "Turnos",
                    new Point(
                        350,
                        301),
                    localidadToolStripMenuItem_Click));

            pnlContenido.Controls.Add(
                CrearBotonAdministracion(
                    "Plantas",
                    new Point(
                        674,
                        301),
                    plantaToolStripMenuItem_Click));

            // =====================================================
            // SEGUNDA FILA
            // =====================================================

            pnlContenido.Controls.Add(
                CrearBotonAdministracion(
                    "Líneas",
                    new Point(
                        26,
                        359),
                    lineaToolStripMenuItem_Click));

            pnlContenido.Controls.Add(
                CrearBotonAdministracion(
                    "Procesos",
                    new Point(
                        350,
                        359),
                    procesoToolStripMenuItem_Click));

            pnlContenido.Controls.Add(
                CrearBotonAdministracion(
                    "Certificadores",
                    new Point(
                        674,
                        359),
                    certificadorToolStripMenuItem_Click));

            // =====================================================
            // SEPARADOR INFERIOR
            // =====================================================

            Panel separador =
                new Panel
                {
                    Location =
                        new Point(
                            26,
                            424),

                    Size =
                        new Size(
                            940,
                            1),

                    BackColor =
                        AppColors.Border
                };

            pnlContenido.Controls.Add(
                separador);

            // =====================================================
            // TEXTO INFERIOR
            // =====================================================

            Label lblPie =
                new Label
                {
                    AutoSize =
                        true,

                    Text =
                        "Sistema de administración de certificaciones",

                    Location =
                        new Point(
                            26,
                            452),

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            9F),

                    BackColor =
                        Color.Transparent
                };

            pnlContenido.Controls.Add(
                lblPie);

            // =====================================================
            // CONFIGURACIÓN
            // =====================================================

            Button btnConfiguracion =
                new Button();

            ButtonStyler.Apply(
                btnConfiguracion,
                "⚙  Configuración",
                AppColors.Secondary,
                string.Empty,
                width: 176,
                height: 42);

            btnConfiguracion.Location =
                new Point(
                    640,
                    442);

            btnConfiguracion.Click +=
                (_, _) =>
                {
                    using Ventanas.ConfiguracionVentana ventana =
                        new Ventanas.ConfiguracionVentana();

                    ventana.ShowDialog(
                        this);
                };

            pnlContenido.Controls.Add(
                btnConfiguracion);

            // =====================================================
            // SALIR
            // =====================================================

            Button btnSalir =
                new Button();

            ButtonStyler.Apply(
                btnSalir,
                "Salir",
                AppColors.Neutral,
                AppIcons.Back,
                width: 138,
                height: 42);

            btnSalir.Location =
                new Point(
                    828,
                    442);

            btnSalir.Click +=
                (_, _) =>
                {
                    Close();
                };

            pnlContenido.Controls.Add(
                btnSalir);

            ResumeLayout(
                true);
        }

        // =========================================================
        // TÍTULO DE SECCIÓN
        // =========================================================

        private static Label CrearTituloSeccion(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                AutoSize =
                    true,

                Text =
                    texto,

                Location =
                    new Point(
                        x,
                        y),

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Regular(
                        12.5F,
                        FontStyle.Bold),

                BackColor =
                    Color.Transparent
            };
        }

        // =========================================================
        // SUBTÍTULO DE SECCIÓN
        // =========================================================

        private static Label CrearSubtituloSeccion(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                AutoSize =
                    true,

                Text =
                    texto,

                Location =
                    new Point(
                        x,
                        y),

                ForeColor =
                    AppColors.TextSecondary,

                Font =
                    AppFonts.Light(
                        9.5F),

                BackColor =
                    Color.Transparent
            };
        }

        // =========================================================
        // TARJETA DE NAVEGACIÓN
        // =========================================================

        private static Panel CrearTarjetaNavegacion(
    string titulo,
    string descripcion,
    Point location,
    EventHandler accion)
        {
            Color colorNormal =
                AppColors.SectionBackground;

            Color colorHover =
                AppColors.HoverBackground;

            Panel card =
                new Panel
                {
                    Location =
                        location,

                    Size =
                        new Size(
                            292,
                            125),

                    BackColor =
                        colorNormal,

                    Cursor =
                        Cursors.Hand,

                    Tag =
                        "NavigationCard"
                };

            RoundedControlHelper.ApplyRoundedRegion(
                card,
                10);

            // =====================================================
            // INDICADOR AZUL
            // =====================================================

            Panel indicador =
                new Panel
                {
                    Location =
                        new Point(
                            0,
                            0),

                    Size =
                        new Size(
                            6,
                            125),

                    BackColor =
                        AppColors.Primary,

                    Cursor =
                        Cursors.Hand
                };

            // =====================================================
            // TÍTULO
            // =====================================================

            Label lblTitulo =
                new Label
                {
                    AutoSize =
                        true,

                    Text =
                        titulo,

                    Location =
                        new Point(
                            24,
                            18),

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            13F,
                            FontStyle.Bold),

                    BackColor =
                        Color.Transparent,

                    Cursor =
                        Cursors.Hand
                };

            // =====================================================
            // DESCRIPCIÓN
            // =====================================================

            Label lblDescripcion =
                new Label
                {
                    AutoSize =
                        false,

                    Text =
                        descripcion,

                    Location =
                        new Point(
                            24,
                            50),

                    Size =
                        new Size(
                            228,
                            55),

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            9.5F),

                    BackColor =
                        Color.Transparent,

                    Cursor =
                        Cursors.Hand
                };

            // =====================================================
            // FLECHA
            // =====================================================

            Label lblFlecha =
                new Label
                {
                    AutoSize =
                        true,

                    Text =
                        "›",

                    Location =
                        new Point(
                            260,
                            43),

                    ForeColor =
                        AppColors.Primary,

                    Font =
                        AppFonts.Regular(
                            22F,
                            FontStyle.Bold),

                    BackColor =
                        Color.Transparent,

                    Cursor =
                        Cursors.Hand
                };

            card.Controls.Add(
                indicador);

            card.Controls.Add(
                lblTitulo);

            card.Controls.Add(
                lblDescripcion);

            card.Controls.Add(
                lblFlecha);

            // =====================================================
            // ACTIVAR HOVER
            // =====================================================

            void ActivarHover(
                object? sender,
                EventArgs e)
            {
                RestablecerTarjetasNavegacion(
                    card.Parent);

                card.BackColor =
                    colorHover;
            }

            // =====================================================
            // COMPROBAR SALIDA REAL
            // =====================================================

            void RevisarSalidaHover(
                object? sender,
                EventArgs e)
            {
                if (card.IsDisposed)
                {
                    return;
                }

                card.BeginInvoke(
                    new Action(
                        () =>
                        {
                            if (card.IsDisposed)
                            {
                                return;
                            }

                            Rectangle areaTarjeta =
                                card.RectangleToScreen(
                                    card.ClientRectangle);

                            if (!areaTarjeta.Contains(
                                    Cursor.Position))
                            {
                                card.BackColor =
                                    colorNormal;

                                card.Invalidate();
                            }
                        }));
            }

            // =====================================================
            // EJECUTAR ACCIÓN
            // =====================================================

            void EjecutarAccion(
                object? sender,
                EventArgs e)
            {
                RestablecerTarjetasNavegacion(
                    card.Parent);

                card.Refresh();

                try
                {
                    accion(
                        sender ?? card,
                        e);
                }
                finally
                {
                    if (!card.IsDisposed)
                    {
                        Rectangle areaTarjeta =
                            card.RectangleToScreen(
                                card.ClientRectangle);

                        card.BackColor =
                            areaTarjeta.Contains(
                                Cursor.Position)
                                ? colorHover
                                : colorNormal;

                        card.Invalidate();
                    }
                }
            }

            // =====================================================
            // EVENTOS
            // =====================================================

            Control[] controlesInteractivos =
            {
        card,
        indicador,
        lblTitulo,
        lblDescripcion,
        lblFlecha
    };

            foreach (Control control in controlesInteractivos)
            {
                control.Click +=
                    EjecutarAccion;

                control.MouseEnter +=
                    ActivarHover;

                control.MouseLeave +=
                    RevisarSalidaHover;
            }

            return card;
        }


        private static void RestablecerTarjetasNavegacion(
    Control? contenedor)
        {
            if (contenedor is null)
            {
                return;
            }

            foreach (Control control in contenedor.Controls)
            {
                if (control is Panel panel &&
                    Equals(
                        panel.Tag,
                        "NavigationCard"))
                {
                    panel.BackColor =
                        AppColors.SectionBackground;

                    panel.Invalidate();
                }
            }
        }

        // =========================================================
        // BOTÓN DE ADMINISTRACIÓN
        // =========================================================

        private static Button CrearBotonAdministracion(
    string texto,
    Point location,
    EventHandler accion)
        {
            Button button =
                new Button
                {
                    Text =
                        texto,

                    Location =
                        location,

                    Size =
                        new Size(
                            292,
                            46),

                    FlatStyle =
                        FlatStyle.Flat,

                    BackColor =
                        AppColors.SectionBackground,

                    ForeColor =
                        AppColors.TextPrimary,

                    Cursor =
                        Cursors.Hand,

                    Font =
                        AppFonts.Regular(
                            10F,
                            FontStyle.Bold),

                    TextAlign =
                        ContentAlignment.MiddleLeft,

                    Padding =
                        new Padding(
                            18,
                            0,
                            12,
                            0),

                    UseVisualStyleBackColor =
                        false
                };

            button.FlatAppearance.BorderSize =
                1;

            button.FlatAppearance.BorderColor =
                AppColors.BorderMedium;

            button.FlatAppearance.MouseOverBackColor =
                AppColors.HoverBackground;

            button.FlatAppearance.MouseDownBackColor =
                AppColors.PressedBackground;

            // Cuando el cursor entra en un botón inferior,
            // cualquier tarjeta superior vuelve a su estado normal.
            button.MouseEnter +=
                (_, _) =>
                {
                    RestablecerTarjetasNavegacion(
                        button.Parent);
                };

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                7);

            button.Click +=
                accion;

            return button;
        }

        // =========================================================
        // CERTIFICACIONES
        // =========================================================

        private void btnCertificaciones_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Certificaciones ventana =
                new Ventanas.Certificaciones();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // LOCALIDADES
        // =========================================================

        private void certificarToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Localidades ventana =
                new Ventanas.Localidades();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // TURNOS
        // =========================================================

        private void localidadToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Turnos ventana =
                new Ventanas.Turnos();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // PLANTAS
        // =========================================================

        private void plantaToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Plantas ventana =
                new Ventanas.Plantas();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // LÍNEAS
        // =========================================================

        private void lineaToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Lineas ventana =
                new Ventanas.Lineas();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // TRABAJADORES
        // =========================================================

        private void trabajadorToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Trabajadores ventana =
                new Ventanas.Trabajadores();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // CERTIFICADORES
        // =========================================================

        private void certificadorToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Certificadores ventana =
                new Ventanas.Certificadores();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // PROCESOS
        // =========================================================

        private void procesoToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Procesos ventana =
                new Ventanas.Procesos();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // CERTIFICACIONES - EVENTO ORIGINAL DEL MENÚ
        // =========================================================

        private void certificacionToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Certificaciones ventana =
                new Ventanas.Certificaciones();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // VERIFICACIONES
        // =========================================================

        private void verificadorToolStripMenuItem_Click(
            object? sender,
            EventArgs e)
        {
            using Ventanas.Verificaciones ventana =
                new Ventanas.Verificaciones();

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // LOAD
        // =========================================================

        private void MenuPrincipal_Load(
            object? sender,
            EventArgs e)
        {
            // La interfaz moderna se configura
            // desde AplicarEstiloVisual().
        }
    }
}