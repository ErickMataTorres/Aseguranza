using AForge.Video;
using AForge.Video.DirectShow;
using Aseguranza.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class TrabajadoresVentana : Form
    {
        // =========================================================
        // DATOS DEL TRABAJADOR
        // =========================================================

        private Clases.Trabajador? trabajadorActual;

        public string NoRelojGuardado
        {
            get;
            private set;
        } = string.Empty;

        // =========================================================
        // FOTOGRAFÍA / CÁMARA
        // =========================================================

        private bool capturaDesdeCamara;

        private string? rutaFotoSeleccionada;

        private FilterInfoCollection? dispositivosVideo;

        private VideoCaptureDevice? camara;

        private Bitmap? fotoCapturada;

        // =========================================================
        // ESTADO
        // =========================================================

        private bool guardadoCorrectamente;

        private bool guardando;

        private bool cargandoDatos;

        private bool estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public TrabajadoresVentana(
            Clases.Trabajador? trabajador)
        {
            InitializeComponent();

            trabajadorActual =
                trabajador;

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void TrabajadoresVentana_Load(
            object sender,
            EventArgs e)
        {
            ConfigurarModoVentana();

            ConfigurarToolTips();

            CargarComboBox();

            CargarCamaras();

            if (pictureBox1.Image is null)
            {
                ActualizarEstadoFoto(
                    "SIN_FOTO");
            }

            ActualizarBotonesCamara();

            MostrarEstado(
                "Listo.");

            if (trabajadorActual is null)
            {
                txtNoReloj.Focus();
            }
            else
            {
                txtNombre.Focus();
                txtNombre.SelectAll();
            }
        }

        // =========================================================
        // DISEÑO VISUAL
        // =========================================================

        private void AplicarEstiloVisual()
        {
            if (estiloAplicado)
            {
                return;
            }

            estiloAplicado =
                true;

            SuspendLayout();

            string titulo =
                trabajadorActual is null
                    ? "Registrar trabajador"
                    : "Modificar trabajador";

            string subtitulo =
                trabajadorActual is null
                    ? "Captura la información general y fotografía del trabajador"
                    : "Actualiza la información general y fotografía del trabajador";

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                titulo,
                new Size(
                    1040,
                    740));

            DoubleBuffered =
                true;

            AcceptButton =
                btnGuardar;

            CancelButton =
                btnRegresar;

            // =====================================================
            // OCULTAR CONTENEDORES ANTIGUOS
            // =====================================================

            lblTitulo.Visible =
                false;

            gbDatosTrabajador.Visible =
                false;

            gbFotoTrabajador.Visible =
                false;

            pnlAcciones.Visible =
                false;

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    titulo,
                    subtitulo);

            // =====================================================
            // TARJETA PRINCIPAL
            // =====================================================

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        105),
                    new Size(
                        ClientSize.Width - 40,
                        ClientSize.Height - 150));

            pnlContenido.Name =
                "pnlContenido";

            // =====================================================
            // PANEL DATOS DEL TRABAJADOR
            // =====================================================

            Panel pnlDatos =
                new Panel
                {
                    Name =
                        "pnlDatosTrabajador",

                    Location =
                        new Point(
                            20,
                            20),

                    Size =
                        new Size(
                            560,
                            455),

                    BackColor =
                        AppColors.AlternateRow
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlDatos,
                10);

            Label lblTituloDatos =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            15),

                    Text =
                        "Información del trabajador",

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            11F,
                            FontStyle.Bold)
                };

            Label lblSubtituloDatos =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            39),

                    Text =
                        "Completa los datos generales y la asignación actual.",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9F)
                };

            pnlDatos.Controls.Add(
                lblTituloDatos);

            pnlDatos.Controls.Add(
                lblSubtituloDatos);

            // =====================================================
            // NO. RELOJ
            // =====================================================

            ConfigurarLabelCampo(
                lblNoReloj,
                "No. Reloj *",
                18,
                72);

            Panel pnlNoReloj =
                CrearPanelCampo(
                    18,
                    97,
                    250);

            ConfigurarTextBox(
                txtNoReloj,
                pnlNoReloj,
                "Ej. 89981");

            pnlDatos.Controls.Add(
                lblNoReloj);

            pnlDatos.Controls.Add(
                pnlNoReloj);

            // =====================================================
            // NOMBRE
            // =====================================================

            ConfigurarLabelCampo(
                lblNombre,
                "Nombre completo *",
                286,
                72);

            Panel pnlNombre =
                CrearPanelCampo(
                    286,
                    97,
                    256);

            ConfigurarTextBox(
                txtNombre,
                pnlNombre,
                "Nombre completo del trabajador");

            pnlDatos.Controls.Add(
                lblNombre);

            pnlDatos.Controls.Add(
                pnlNombre);

            // =====================================================
            // LOCALIDAD
            // =====================================================

            ConfigurarLabelCampo(
                lblLocalidad,
                "Localidad *",
                18,
                158);

            Panel pnlLocalidad =
                CrearPanelCampo(
                    18,
                    183,
                    250);

            ConfigurarComboBox(
                cbLocalidad,
                pnlLocalidad);

            pnlDatos.Controls.Add(
                lblLocalidad);

            pnlDatos.Controls.Add(
                pnlLocalidad);

            // =====================================================
            // TURNO
            // =====================================================

            ConfigurarLabelCampo(
                lblTurno,
                "Turno *",
                286,
                158);

            Panel pnlTurno =
                CrearPanelCampo(
                    286,
                    183,
                    256);

            ConfigurarComboBox(
                cbTurno,
                pnlTurno);

            pnlDatos.Controls.Add(
                lblTurno);

            pnlDatos.Controls.Add(
                pnlTurno);

            // =====================================================
            // PLANTA
            // =====================================================

            ConfigurarLabelCampo(
                lblPlanta,
                "Planta *",
                18,
                244);

            Panel pnlPlanta =
                CrearPanelCampo(
                    18,
                    269,
                    250);

            ConfigurarComboBox(
                cbPlanta,
                pnlPlanta);

            pnlDatos.Controls.Add(
                lblPlanta);

            pnlDatos.Controls.Add(
                pnlPlanta);

            // =====================================================
            // LÍNEA
            // =====================================================

            ConfigurarLabelCampo(
                lblLinea,
                "Línea *",
                286,
                244);

            Panel pnlLinea =
                CrearPanelCampo(
                    286,
                    269,
                    256);

            ConfigurarComboBox(
                cbLinea,
                pnlLinea);

            pnlDatos.Controls.Add(
                lblLinea);

            pnlDatos.Controls.Add(
                pnlLinea);

            // =====================================================
            // CAMPOS OBLIGATORIOS
            // =====================================================

            lblCamposObligatorios.Text =
                "* Campos obligatorios";

            lblCamposObligatorios.AutoSize =
                true;

            lblCamposObligatorios.Location =
                new Point(
                    18,
                    335);

            lblCamposObligatorios.ForeColor =
                AppColors.Danger;

            lblCamposObligatorios.Font =
                AppFonts.Light(9F);

            pnlDatos.Controls.Add(
                lblCamposObligatorios);

            Label lblAyudaDatos =
                new Label
                {
                    AutoSize =
                        false,

                    Location =
                        new Point(
                            18,
                            366),

                    Size =
                        new Size(
                            520,
                            55),

                    Text =
                        "La línea disponible depende de la planta seleccionada. " +
                        "Al modificar un trabajador, el número de reloj permanece bloqueado.",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9F)
                };

            pnlDatos.Controls.Add(
                lblAyudaDatos);

            // =====================================================
            // PANEL FOTOGRAFÍA
            // =====================================================

            Panel pnlFoto =
                new Panel
                {
                    Name =
                        "pnlFotoTrabajador",

                    Location =
                        new Point(
                            600,
                            20),

                    Size =
                        new Size(
                            380,
                            455),

                    BackColor =
                        AppColors.AlternateRow
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlFoto,
                10);

            Label lblTituloFoto =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            15),

                    Text =
                        "Fotografía del trabajador *",

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            11F,
                            FontStyle.Bold)
                };

            pnlFoto.Controls.Add(
                lblTituloFoto);

            // =====================================================
            // CÁMARA
            // =====================================================

            lblCamaras.Text =
                "Cámara";

            lblCamaras.AutoSize =
                true;

            lblCamaras.Location =
                new Point(
                    18,
                    51);

            lblCamaras.ForeColor =
                AppColors.TextPrimary;

            lblCamaras.Font =
                AppFonts.Regular(
                    9F,
                    FontStyle.Bold);

            pnlFoto.Controls.Add(
                lblCamaras);

            Panel pnlCamara =
                CrearPanelCampo(
                    18,
                    73,
                    294);

            cbCamaras.Location =
                new Point(
                    8,
                    7);

            cbCamaras.Size =
                new Size(
                    pnlCamara.ClientSize.Width - 16,
                    28);

            cbCamaras.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            ComboBoxStyler.ApplyOutlinedComboBox(
                pnlCamara,
                cbCamaras);

            pnlCamara.Controls.Add(
                cbCamaras);

            pnlFoto.Controls.Add(
                pnlCamara);

            // =====================================================
            // RECARGAR CÁMARAS
            // =====================================================

            ConfigurarBotonFoto(
                btnRecargarCamaras,
                "↻",
                AppColors.Secondary,
                42);

            btnRecargarCamaras.Location =
                new Point(
                    320,
                    73);

            btnRecargarCamaras.Font =
                AppFonts.Regular(
                    14F,
                    FontStyle.Bold);

            pnlFoto.Controls.Add(
                btnRecargarCamaras);

            // =====================================================
            // FOTOGRAFÍA / PREVISUALIZACIÓN
            // =====================================================

            pictureBox1.Location =
                new Point(
                    18,
                    125);

            pictureBox1.Size =
                new Size(
                    344,
                    185);

            pictureBox1.BackColor =
                Color.White;

            pictureBox1.BorderStyle =
                BorderStyle.None;

            pictureBox1.SizeMode =
                PictureBoxSizeMode.Zoom;

            RoundedControlHelper.ApplyRoundedRegion(
                pictureBox1,
                8);

            pnlFoto.Controls.Add(
                pictureBox1);

            // =====================================================
            // ESTADO FOTO
            // =====================================================

            lblEstadoFoto.Location =
                new Point(
                    18,
                    316);

            lblEstadoFoto.Size =
                new Size(
                    344,
                    22);

            lblEstadoFoto.TextAlign =
                ContentAlignment.MiddleLeft;

            lblEstadoFoto.Font =
                AppFonts.Light(9F);

            pnlFoto.Controls.Add(
                lblEstadoFoto);

            // =====================================================
            // BOTONES DE CÁMARA
            // =====================================================

            ConfigurarBotonFoto(
                btnIniciarCamara,
                "Iniciar cámara",
                AppColors.Secondary,
                166);

            btnIniciarCamara.Location =
                new Point(
                    18,
                    343);

            ConfigurarBotonFoto(
                btnCapturar,
                "Capturar",
                AppColors.Primary,
                166);

            btnCapturar.Location =
                new Point(
                    196,
                    343);

            ConfigurarBotonFoto(
                btnSeleccionar,
                "Seleccionar imagen",
                AppColors.Secondary,
                166);

            btnSeleccionar.Location =
                new Point(
                    18,
                    393);

            ConfigurarBotonFoto(
                btnQuitarFoto,
                "Quitar foto",
                AppColors.Danger,
                166);

            btnQuitarFoto.Location =
                new Point(
                    196,
                    393);

            pnlFoto.Controls.Add(
                btnIniciarCamara);

            pnlFoto.Controls.Add(
                btnCapturar);

            pnlFoto.Controls.Add(
                btnSeleccionar);

            pnlFoto.Controls.Add(
                btnQuitarFoto);

            // =====================================================
            // BOTONES PRINCIPALES
            // =====================================================

            ButtonStyler.Apply(
                btnGuardar,
                trabajadorActual is null
                    ? "Guardar"
                    : "Guardar cambios",
                AppColors.Primary,
                AppIcons.Save,
                width: 200);

            ButtonStyler.Apply(
                btnRegresar,
                "Cancelar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 150);

            int yBotones =
                pnlContenido.ClientSize.Height - 60;

            btnGuardar.Location =
                new Point(
                    20,
                    yBotones);

            btnRegresar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    btnRegresar.Width -
                    20,
                    yBotones);

            btnGuardar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            btnRegresar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            // =====================================================
            // AGREGAR A TARJETA
            // =====================================================

            pnlContenido.Controls.Add(
                pnlDatos);

            pnlContenido.Controls.Add(
                pnlFoto);

            pnlContenido.Controls.Add(
                btnGuardar);

            pnlContenido.Controls.Add(
                btnRegresar);

            // =====================================================
            // STATUS STRIP
            // =====================================================

            ssEstado.BackColor =
                AppColors.CardBackground;

            ssEstado.ForeColor =
                AppColors.TextSecondary;

            ssEstado.SizingGrip =
                false;

            tsslEstado.Font =
                AppFonts.Light(9F);

            tsslEstado.ForeColor =
                AppColors.TextSecondary;

            ssEstado.BringToFront();

            pnlContenido.BringToFront();

            pnlCabecera.BringToFront();

            ResumeLayout(false);

            PerformLayout();
        }

        // =========================================================
        // HELPERS VISUALES
        // =========================================================

        private static void ConfigurarLabelCampo(
            Label label,
            string texto,
            int x,
            int y)
        {
            label.Text =
                texto;

            label.AutoSize =
                true;

            label.Location =
                new Point(
                    x,
                    y);

            label.ForeColor =
                AppColors.TextPrimary;

            label.Font =
                AppFonts.Regular(
                    9.5F,
                    FontStyle.Bold);
        }

        private static Panel CrearPanelCampo(
            int x,
            int y,
            int ancho)
        {
            return new Panel
            {
                Location =
                    new Point(
                        x,
                        y),

                Size =
                    new Size(
                        ancho,
                        42),

                BackColor =
                    Color.White
            };
        }

        private static void ConfigurarTextBox(
            TextBox textBox,
            Panel panel,
            string placeholder)
        {
            textBox.Location =
                new Point(
                    12,
                    10);

            textBox.Size =
                new Size(
                    panel.ClientSize.Width - 24,
                    25);

            textBox.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            textBox.BorderStyle =
                BorderStyle.None;

            textBox.BackColor =
                Color.White;

            textBox.ForeColor =
                AppColors.TextPrimary;

            textBox.Font =
                AppFonts.Light(10.5F);

            textBox.PlaceholderText =
                placeholder;

            InputStyler.ApplyOutlinedInput(
                panel,
                textBox);

            panel.Controls.Add(
                textBox);
        }

        private static void ConfigurarComboBox(
            ComboBox comboBox,
            Panel panel)
        {
            comboBox.Location =
                new Point(
                    8,
                    7);

            comboBox.Size =
                new Size(
                    panel.ClientSize.Width - 16,
                    28);

            comboBox.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            ComboBoxStyler.ApplyOutlinedComboBox(
                panel,
                comboBox);

            panel.Controls.Add(
                comboBox);
        }

        private static void ConfigurarBotonFoto(
            Button boton,
            string texto,
            Color colorFondo,
            int ancho)
        {
            boton.Text =
                texto;

            boton.Size =
                new Size(
                    ancho,
                    40);

            boton.FlatStyle =
                FlatStyle.Flat;

            boton.FlatAppearance.BorderSize =
                0;

            boton.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(
                    colorFondo,
                    0.05F);

            boton.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(
                    colorFondo,
                    0.10F);

            boton.BackColor =
                colorFondo;

            boton.ForeColor =
                Color.White;

            boton.Font =
                AppFonts.Regular(
                    9F,
                    FontStyle.Bold);

            boton.Cursor =
                Cursors.Hand;

            boton.UseVisualStyleBackColor =
                false;

            RoundedControlHelper.ApplyRoundedRegion(
                boton,
                7);
        }

        // =========================================================
        // ESTADO GENERAL
        // =========================================================

        private void MostrarEstado(
            string mensaje,
            bool esAdvertencia = false)
        {
            tsslEstado.Text =
                mensaje;

            tsslEstado.ForeColor =
                esAdvertencia
                    ? Color.DarkOrange
                    : AppColors.TextSecondary;
        }

        // =========================================================
        // CARGAR LÍNEAS POR PLANTA
        // =========================================================

        private void CargarLineasPorPlanta(
            int idPlanta,
            int idLineaSeleccionar = 0)
        {
            cbLinea.DataSource =
                null;

            cbLinea.Enabled =
                false;

            var lineas =
                Clases.Linea
                    .ConsultarLineasPorPlanta(
                        idPlanta);

            cbLinea.DataSource =
                lineas;

            cbLinea.ValueMember =
                "Id";

            cbLinea.DisplayMember =
                "Nombre";

            cbLinea.Enabled =
                true;

            if (idLineaSeleccionar > 0)
            {
                cbLinea.SelectedValue =
                    idLineaSeleccionar;
            }
            else
            {
                cbLinea.SelectedIndex =
                    -1;
            }
        }

        // =========================================================
        // ESTADO DE FOTOGRAFÍA
        // =========================================================

        private void ActualizarEstadoFotoSegunImagenActual()
        {
            if (pictureBox1.Image is null)
            {
                ActualizarEstadoFoto(
                    "SIN_FOTO");

                return;
            }

            if (capturaDesdeCamara)
            {
                ActualizarEstadoFoto(
                    "CAPTURADA");

                return;
            }

            if (trabajadorActual is not null &&
                !string.IsNullOrWhiteSpace(
                    rutaFotoSeleccionada) &&
                rutaFotoSeleccionada ==
                    trabajadorActual.RutaFoto)
            {
                ActualizarEstadoFoto(
                    "EXISTENTE");

                return;
            }

            if (!string.IsNullOrWhiteSpace(
                rutaFotoSeleccionada))
            {
                ActualizarEstadoFoto(
                    "SELECCIONADA");

                return;
            }

            ActualizarEstadoFoto(
                "EXISTENTE");
        }

        private void ActualizarEstadoFoto(
            string estado)
        {
            switch (estado)
            {
                case "SIN_FOTO":

                    lblEstadoFoto.Text =
                        "Foto: sin fotografía";

                    lblEstadoFoto.ForeColor =
                        AppColors.TextSecondary;

                    break;

                case "CAMARA_ACTIVA":

                    lblEstadoFoto.Text =
                        "Foto: cámara activa";

                    lblEstadoFoto.ForeColor =
                        AppColors.Primary;

                    break;

                case "CAPTURADA":

                    lblEstadoFoto.Text =
                        "Foto: capturada desde cámara";

                    lblEstadoFoto.ForeColor =
                        Color.FromArgb(
                            0,
                            120,
                            40);

                    break;

                case "SELECCIONADA":

                    lblEstadoFoto.Text =
                        "Foto: imagen seleccionada";

                    lblEstadoFoto.ForeColor =
                        Color.FromArgb(
                            0,
                            120,
                            40);

                    break;

                case "EXISTENTE":

                    lblEstadoFoto.Text =
                        "Foto: fotografía existente";

                    lblEstadoFoto.ForeColor =
                        Color.FromArgb(
                            0,
                            120,
                            40);

                    break;

                default:

                    lblEstadoFoto.Text =
                        "Foto: sin fotografía";

                    lblEstadoFoto.ForeColor =
                        AppColors.TextSecondary;

                    break;
            }
        }

        // =========================================================
        // MOSTRAR IMAGEN
        // =========================================================

        private void MostrarImagenEnPictureBox(
            Image imagen)
        {
            if (pictureBox1.Image is not null)
            {
                Image imagenAnterior =
                    pictureBox1.Image;

                pictureBox1.Image =
                    null;

                imagenAnterior.Dispose();
            }

            pictureBox1.Image =
                imagen;

            pictureBox1.SizeMode =
                PictureBoxSizeMode.Zoom;
        }

        // =========================================================
        // OBTENER ID DE COMBO
        // =========================================================

        private static int ObtenerIdCombo(
            ComboBox combo)
        {
            if (combo.SelectedValue is null)
            {
                return 0;
            }

            try
            {
                return Convert.ToInt32(
                    combo.SelectedValue);
            }
            catch
            {
                return 0;
            }
        }

        // =========================================================
        // CAMBIOS SIN GUARDAR
        // =========================================================

        private bool HayCambiosSinGuardar()
        {
            if (trabajadorActual is null)
            {
                return
                    !string.IsNullOrWhiteSpace(
                        txtNoReloj.Text) ||

                    !string.IsNullOrWhiteSpace(
                        txtNombre.Text) ||

                    cbLocalidad.SelectedIndex != -1 ||

                    cbTurno.SelectedIndex != -1 ||

                    cbPlanta.SelectedIndex != -1 ||

                    cbLinea.SelectedIndex != -1 ||

                    pictureBox1.Image is not null ||

                    capturaDesdeCamara ||

                    !string.IsNullOrWhiteSpace(
                        rutaFotoSeleccionada) ||

                    (camara is not null &&
                     camara.IsRunning);
            }

            string noRelojActual =
                txtNoReloj.Text.Trim();

            string nombreActual =
                txtNombre.Text
                    .Trim()
                    .ToUpper();

            string noRelojOriginal =
                trabajadorActual
                    .NoReloj?
                    .Trim()
                ?? string.Empty;

            string nombreOriginal =
                trabajadorActual
                    .Nombre?
                    .Trim()
                    .ToUpper()
                ?? string.Empty;

            if (noRelojActual !=
                noRelojOriginal)
            {
                return true;
            }

            if (nombreActual !=
                nombreOriginal)
            {
                return true;
            }

            if (ObtenerIdCombo(
                    cbLocalidad) !=
                trabajadorActual.IdLocalidad)
            {
                return true;
            }

            if (ObtenerIdCombo(
                    cbTurno) !=
                trabajadorActual.IdTurno)
            {
                return true;
            }

            if (ObtenerIdCombo(
                    cbPlanta) !=
                trabajadorActual.IdPlanta)
            {
                return true;
            }

            if (ObtenerIdCombo(
                    cbLinea) !=
                trabajadorActual.IdLinea)
            {
                return true;
            }

            if (capturaDesdeCamara)
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(
                    rutaFotoSeleccionada) &&
                rutaFotoSeleccionada !=
                    trabajadorActual.RutaFoto)
            {
                return true;
            }

            if (camara is not null &&
                camara.IsRunning)
            {
                return true;
            }

            return false;
        }

        // =========================================================
        // VALIDACIÓN
        // =========================================================

        private void LimpiarErroresValidacion()
        {
            epValidacion.SetError(
                txtNoReloj,
                string.Empty);

            epValidacion.SetError(
                txtNombre,
                string.Empty);

            epValidacion.SetError(
                cbLocalidad,
                string.Empty);

            epValidacion.SetError(
                cbTurno,
                string.Empty);

            epValidacion.SetError(
                cbPlanta,
                string.Empty);

            epValidacion.SetError(
                cbLinea,
                string.Empty);

            epValidacion.SetError(
                pictureBox1,
                string.Empty);
        }

        private bool ValidarInformacion()
        {
            LimpiarErroresValidacion();

            bool esValido =
                true;

            if (string.IsNullOrWhiteSpace(
                txtNoReloj.Text))
            {
                epValidacion.SetError(
                    txtNoReloj,
                    "Capture el número de reloj.");

                esValido =
                    false;
            }

            if (string.IsNullOrWhiteSpace(
                txtNombre.Text))
            {
                epValidacion.SetError(
                    txtNombre,
                    "Capture el nombre del trabajador.");

                esValido =
                    false;
            }

            if (cbLocalidad.SelectedIndex == -1 ||
                cbLocalidad.SelectedValue is null)
            {
                epValidacion.SetError(
                    cbLocalidad,
                    "Seleccione la localidad.");

                esValido =
                    false;
            }

            if (cbTurno.SelectedIndex == -1 ||
                cbTurno.SelectedValue is null)
            {
                epValidacion.SetError(
                    cbTurno,
                    "Seleccione el turno.");

                esValido =
                    false;
            }

            if (cbPlanta.SelectedIndex == -1 ||
                cbPlanta.SelectedValue is null)
            {
                epValidacion.SetError(
                    cbPlanta,
                    "Seleccione la planta.");

                esValido =
                    false;
            }

            if (cbLinea.SelectedIndex == -1 ||
                cbLinea.SelectedValue is null)
            {
                epValidacion.SetError(
                    cbLinea,
                    "Seleccione la línea.");

                esValido =
                    false;
            }

            if (pictureBox1.Image is null)
            {
                epValidacion.SetError(
                    pictureBox1,
                    "Capture o seleccione una fotografía.");

                esValido =
                    false;
            }

            if (!esValido)
            {
                MessageBox.Show(
                    "Hay información incompleta. Revise los campos marcados.",
                    "Información incompleta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return esValido;
        }

        // =========================================================
        // MODO DE VENTANA
        // =========================================================

        private void ConfigurarModoVentana()
        {
            if (trabajadorActual is null)
            {
                Text =
                    "Registrar trabajador";

                btnGuardar.Text =
                    "Guardar";
            }
            else
            {
                Text =
                    "Modificar trabajador";

                btnGuardar.Text =
                    "Guardar cambios";
            }
        }

        // =========================================================
        // TOOLTIPS
        // =========================================================

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(
                txtNoReloj,
                "Capture el número de reloj del trabajador.");

            ttAyuda.SetToolTip(
                txtNombre,
                "Capture el nombre completo del trabajador.");

            ttAyuda.SetToolTip(
                cbLocalidad,
                "Seleccione la localidad del trabajador.");

            ttAyuda.SetToolTip(
                cbTurno,
                "Seleccione el turno del trabajador.");

            ttAyuda.SetToolTip(
                cbPlanta,
                "Seleccione la planta del trabajador.");

            ttAyuda.SetToolTip(
                cbLinea,
                "Seleccione la línea del trabajador.");

            ttAyuda.SetToolTip(
                cbCamaras,
                "Seleccione la cámara que desea utilizar.");

            ttAyuda.SetToolTip(
                pictureBox1,
                "Vista previa de la fotografía. Doble clic o clic derecho para más opciones.");

            ttAyuda.SetToolTip(
                btnIniciarCamara,
                "Inicia o detiene la cámara.");

            ttAyuda.SetToolTip(
                btnCapturar,
                "Captura la imagen actual de la cámara.");

            ttAyuda.SetToolTip(
                btnSeleccionar,
                "Selecciona una imagen desde el equipo.");

            ttAyuda.SetToolTip(
                btnGuardar,
                "Guarda la información del trabajador.");

            ttAyuda.SetToolTip(
                btnRegresar,
                "Cierra esta ventana sin guardar cambios.");

            ttAyuda.SetToolTip(
                btnQuitarFoto,
                "Quita la fotografía actual.");

            ttAyuda.SetToolTip(
                btnRecargarCamaras,
                "Recarga las cámaras disponibles.");

            ttAyuda.SetToolTip(
                lblCamposObligatorios,
                "Los campos marcados con * son necesarios para guardar el trabajador.");
        }

        // =========================================================
        // LIMPIAR FOTO
        // =========================================================

        private void LimpiarFotoActual()
        {
            if (camara is not null &&
                camara.IsRunning)
            {
                DetenerCamara(
                    limpiarImagen: false);
            }

            if (pictureBox1.Image is not null)
            {
                pictureBox1.Image.Dispose();

                pictureBox1.Image =
                    null;
            }

            if (fotoCapturada is not null)
            {
                fotoCapturada.Dispose();

                fotoCapturada =
                    null;
            }

            capturaDesdeCamara =
                false;

            rutaFotoSeleccionada =
                null;

            ActualizarEstadoFoto(
                "SIN_FOTO");

            ActualizarBotonesCamara();

            epValidacion.SetError(
                pictureBox1,
                "Capture o seleccione una fotografía.");

            MostrarEstado(
                "Fotografía quitada. Capture o seleccione una nueva.",
                true);
        }

        // =========================================================
        // GUARDAR FOTO JPG
        // =========================================================

        private static void GuardarImagenComoJpgConFondoBlanco(
            Image imagenOriginal,
            string rutaDestino)
        {
            const int maxAncho =
                800;

            const int maxAlto =
                800;

            double proporcionAncho =
                (double)maxAncho /
                imagenOriginal.Width;

            double proporcionAlto =
                (double)maxAlto /
                imagenOriginal.Height;

            double proporcion =
                Math.Min(
                    proporcionAncho,
                    proporcionAlto);

            int nuevoAncho =
                (int)(
                    imagenOriginal.Width *
                    proporcion);

            int nuevoAlto =
                (int)(
                    imagenOriginal.Height *
                    proporcion);

            if (nuevoAncho <= 0)
            {
                nuevoAncho =
                    1;
            }

            if (nuevoAlto <= 0)
            {
                nuevoAlto =
                    1;
            }

            using Bitmap imagenFinal =
                new Bitmap(
                    nuevoAncho,
                    nuevoAlto,
                    System.Drawing.Imaging
                        .PixelFormat
                        .Format24bppRgb);

            imagenFinal.SetResolution(
                96,
                96);

            using Graphics graphics =
                Graphics.FromImage(
                    imagenFinal);

            graphics.Clear(
                Color.White);

            graphics.InterpolationMode =
                System.Drawing.Drawing2D
                    .InterpolationMode
                    .HighQualityBicubic;

            graphics.SmoothingMode =
                System.Drawing.Drawing2D
                    .SmoothingMode
                    .HighQuality;

            graphics.PixelOffsetMode =
                System.Drawing.Drawing2D
                    .PixelOffsetMode
                    .HighQuality;

            graphics.CompositingQuality =
                System.Drawing.Drawing2D
                    .CompositingQuality
                    .HighQuality;

            graphics.DrawImage(
                imagenOriginal,
                0,
                0,
                nuevoAncho,
                nuevoAlto);

            imagenFinal.Save(
                rutaDestino,
                System.Drawing.Imaging
                    .ImageFormat
                    .Jpeg);
        }

        private static Image CargarImagenSinBloquearArchivo(
            string ruta)
        {
            using FileStream stream =
                new FileStream(
                    ruta,
                    FileMode.Open,
                    FileAccess.Read);

            using Image imagenTemporal =
                Image.FromStream(
                    stream);

            return new Bitmap(
                imagenTemporal);
        }

        // =========================================================
        // CÁMARAS
        // =========================================================

        private void CargarCamaras()
        {
            try
            {
                dispositivosVideo =
                    new FilterInfoCollection(
                        FilterCategory.VideoInputDevice);

                if (dispositivosVideo.Count == 0)
                {
                    cbCamaras.DataSource =
                        null;

                    cbCamaras.Enabled =
                        false;

                    btnIniciarCamara.Enabled =
                        false;

                    btnCapturar.Enabled =
                        false;

                    ActualizarEstadoFotoSegunImagenActual();

                    MostrarEstado(
                        "No se detectaron cámaras. Puede seleccionar una imagen.",
                        true);

                    return;
                }

                cbCamaras.DataSource =
                    dispositivosVideo;

                cbCamaras.DisplayMember =
                    "Name";

                cbCamaras.SelectedIndex =
                    0;

                cbCamaras.Enabled =
                    true;

                btnIniciarCamara.Enabled =
                    true;

                btnCapturar.Enabled =
                    false;
            }
            catch (Exception ex)
            {
                cbCamaras.DataSource =
                    null;

                cbCamaras.Enabled =
                    false;

                btnIniciarCamara.Enabled =
                    false;

                btnCapturar.Enabled =
                    false;

                ActualizarEstadoFotoSegunImagenActual();

                MostrarEstado(
                    "No se pudieron cargar las cámaras.",
                    true);

                MessageBox.Show(
                    "No se pudieron cargar las cámaras.\n\n" +
                    "Detalle: " +
                    ex.Message,
                    "Error de cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // =========================================================
        // CARGAR TRABAJADOR
        // =========================================================

        private void CargarTrabajador()
        {
            if (trabajadorActual is null)
            {
                return;
            }

            cargandoDatos =
                true;

            txtNoReloj.Text =
                trabajadorActual.NoReloj;

            txtNoReloj.Enabled =
                false;

            txtNombre.Text =
                trabajadorActual.Nombre;

            cbLocalidad.SelectedValue =
                trabajadorActual.IdLocalidad;

            cbTurno.SelectedValue =
                trabajadorActual.IdTurno;

            cbPlanta.SelectedValue =
                trabajadorActual.IdPlanta;

            cargandoDatos =
                false;

            CargarLineasPorPlanta(
                trabajadorActual.IdPlanta,
                trabajadorActual.IdLinea);

            if (!string.IsNullOrEmpty(
                    trabajadorActual.RutaFoto) &&
                File.Exists(
                    trabajadorActual.RutaFoto))
            {
                MostrarImagenEnPictureBox(
                    CargarImagenSinBloquearArchivo(
                        trabajadorActual.RutaFoto));

                rutaFotoSeleccionada =
                    trabajadorActual.RutaFoto;

                capturaDesdeCamara =
                    false;

                ActualizarEstadoFoto(
                    "EXISTENTE");
            }
        }

        // =========================================================
        // COMBOBOX
        // =========================================================

        private void CargarComboBox()
        {
            cbLocalidad.DataSource =
                Clases.Localidad
                    .ConsultarLocalidades(
                        string.Empty);

            cbLocalidad.ValueMember =
                "Id";

            cbLocalidad.DisplayMember =
                "Nombre";

            cbTurno.DataSource =
                Clases.Turno
                    .ConsultarTurnos(
                        string.Empty);

            cbTurno.ValueMember =
                "Id";

            cbTurno.DisplayMember =
                "Nombre";

            cbPlanta.DataSource =
                Clases.Planta
                    .ConsultarPlantas(
                        string.Empty);

            cbPlanta.ValueMember =
                "Id";

            cbPlanta.DisplayMember =
                "Nombre";

            if (trabajadorActual is not null)
            {
                CargarTrabajador();
            }
            else
            {
                cbLocalidad.SelectedIndex =
                    -1;

                cbTurno.SelectedIndex =
                    -1;

                cbPlanta.SelectedIndex =
                    -1;

                cbLinea.DataSource =
                    null;

                cbLinea.Enabled =
                    false;
            }
        }

        // =========================================================
        // DETENER / RESETEAR CÁMARA
        // =========================================================

        private void DetenerCamara(
            bool limpiarImagen)
        {
            if (camara is not null)
            {
                if (camara.IsRunning)
                {
                    camara.NewFrame -=
                        Camara_NewFrame;

                    camara.SignalToStop();

                    camara.WaitForStop();
                }

                camara =
                    null;
            }

            if (limpiarImagen &&
                pictureBox1.Image is not null)
            {
                pictureBox1.Image.Dispose();

                pictureBox1.Image =
                    null;
            }

            ActualizarBotonesCamara();
        }

        private void ResetearUsoCamara()
        {
            DetenerCamara(
                limpiarImagen: true);

            dispositivosVideo =
                null;

            if (fotoCapturada is not null)
            {
                fotoCapturada.Dispose();

                fotoCapturada =
                    null;
            }

            capturaDesdeCamara =
                false;

            rutaFotoSeleccionada =
                null;
        }

        // =========================================================
        // REGRESAR
        // =========================================================

        private void btnRegresar_Click(
            object sender,
            EventArgs e)
        {
            if (camara is not null &&
                camara.IsRunning)
            {
                DetenerCamara(
                    limpiarImagen: false);
            }

            Close();
        }

        // =========================================================
        // PLANTA → LÍNEA
        // =========================================================

        private void cbPlanta_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            epValidacion.SetError(
                cbPlanta,
                string.Empty);
        }

        private void cbPlanta_SelectedValueChanged(
            object sender,
            EventArgs e)
        {
            if (cargandoDatos)
            {
                return;
            }

            int idPlanta =
                ObtenerIdCombo(
                    cbPlanta);

            if (idPlanta > 0)
            {
                CargarLineasPorPlanta(
                    idPlanta);
            }
            else
            {
                cbLinea.DataSource =
                    null;

                cbLinea.Enabled =
                    false;
            }
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            if (guardando)
            {
                return;
            }

            if (camara is not null &&
                camara.IsRunning)
            {
                MessageBox.Show(
                    "La cámara está activa. Primero capture la fotografía o detenga la cámara antes de guardar.",
                    "Cámara activa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!ValidarInformacion())
            {
                return;
            }

            guardando =
                true;

            btnGuardar.Enabled =
                false;

            Cursor =
                Cursors.WaitCursor;

            MostrarEstado(
                "Guardando trabajador...");

            try
            {
                Clases.Trabajador trabajador =
                    trabajadorActual ??
                    new Clases.Trabajador();

                trabajador.NoReloj =
                    txtNoReloj.Text.Trim();

                trabajador.Nombre =
                    txtNombre.Text
                        .Trim()
                        .ToUpper();

                Clases.RutasArchivos
                    .CrearCarpetasTrabajador(
                        trabajador.NoReloj!);

                string rutaDestino =
                    Clases.RutasArchivos
                        .ObtenerRutaFotoPerfil(
                            trabajador.NoReloj!);

                // ===============================================
                // FOTOGRAFÍA
                // ===============================================

                if (capturaDesdeCamara &&
                    fotoCapturada is not null)
                {
                    GuardarImagenComoJpgConFondoBlanco(
                        fotoCapturada,
                        rutaDestino);

                    trabajador.RutaFoto =
                        rutaDestino;
                }
                else if (!string.IsNullOrEmpty(
                    rutaFotoSeleccionada))
                {
                    if (rutaFotoSeleccionada !=
                        rutaDestino)
                    {
                        using Image imagenSeleccionada =
                            CargarImagenSinBloquearArchivo(
                                rutaFotoSeleccionada);

                        GuardarImagenComoJpgConFondoBlanco(
                            imagenSeleccionada,
                            rutaDestino);
                    }

                    trabajador.RutaFoto =
                        rutaDestino;
                }
                else if (trabajadorActual is not null)
                {
                    trabajador.RutaFoto =
                        trabajadorActual.RutaFoto;
                }
                else
                {
                    MessageBox.Show(
                        "Debe capturar o seleccionar una fotografía.",
                        "Foto requerida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // ===============================================
                // DATOS
                // ===============================================

                trabajador.IdLocalidad =
                    ObtenerIdCombo(
                        cbLocalidad);

                trabajador.IdTurno =
                    ObtenerIdCombo(
                        cbTurno);

                trabajador.IdPlanta =
                    ObtenerIdCombo(
                        cbPlanta);

                trabajador.IdLinea =
                    ObtenerIdCombo(
                        cbLinea);

                Clases.Mensaje respuesta =
                    trabajador.GuardarTrabajador();

                if (respuesta.Id == 1 ||
                    respuesta.Id == 3)
                {
                    MessageBox.Show(
                        respuesta.Nombre,
                        "Resultado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    NoRelojGuardado =
                        txtNoReloj.Text.Trim();

                    guardadoCorrectamente =
                        true;

                    DialogResult =
                        DialogResult.OK;

                    Close();
                }
                else
                {
                    MessageBox.Show(
                        respuesta.Nombre,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    MostrarEstado(
                        "No se pudo guardar el trabajador.",
                        true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al guardar el trabajador.\n\n" +
                    "Detalle: " +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                MostrarEstado(
                    "Ocurrió un error al guardar.",
                    true);
            }
            finally
            {
                if (!guardadoCorrectamente)
                {
                    guardando =
                        false;

                    btnGuardar.Enabled =
                        true;

                    Cursor =
                        Cursors.Default;
                }
            }
        }

        // =========================================================
        // SELECCIONAR FOTO
        // =========================================================

        private void btnSeleccionar_Click(
            object sender,
            EventArgs e)
        {
            if (camara is not null &&
                camara.IsRunning)
            {
                DetenerCamara(
                    limpiarImagen: true);
            }

            using OpenFileDialog ofd =
                new OpenFileDialog
                {
                    Title =
                        "Seleccionar fotografía",

                    Filter =
                        "Imágenes (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
                };

            if (ofd.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            try
            {
                Image imagenTemporal =
                    CargarImagenSinBloquearArchivo(
                        ofd.FileName);

                MostrarImagenEnPictureBox(
                    imagenTemporal);

                rutaFotoSeleccionada =
                    ofd.FileName;

                capturaDesdeCamara =
                    false;

                if (fotoCapturada is not null)
                {
                    fotoCapturada.Dispose();

                    fotoCapturada =
                        null;
                }

                ActualizarEstadoFoto(
                    "SELECCIONADA");

                epValidacion.SetError(
                    pictureBox1,
                    string.Empty);

                MostrarEstado(
                    "Imagen seleccionada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar la imagen seleccionada.\n\n" +
                    "Verifique que el archivo sea una imagen válida JPG, PNG o BMP.\n\n" +
                    "Detalle: " +
                    ex.Message,
                    "Imagen no válida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                MostrarEstado(
                    "No se pudo cargar la imagen seleccionada.",
                    true);

                rutaFotoSeleccionada =
                    null;

                capturaDesdeCamara =
                    false;
            }
        }

        // =========================================================
        // CIERRE
        // =========================================================

        private void TrabajadoresVentana_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            if (guardadoCorrectamente)
            {
                return;
            }

            if (!HayCambiosSinGuardar())
            {
                return;
            }

            DialogResult respuesta =
                MessageBox.Show(
                    "Hay información capturada sin guardar.\n\n" +
                    "¿Desea cerrar la ventana y descartar los cambios?",
                    "Cambios sin guardar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (respuesta !=
                DialogResult.Yes)
            {
                e.Cancel =
                    true;

                return;
            }

            if (camara is not null &&
                camara.IsRunning)
            {
                DetenerCamara(
                    limpiarImagen: false);
            }
        }

        private void TrabajadoresVentana_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            ResetearUsoCamara();
        }

        // =========================================================
        // FRAME DE CÁMARA
        // =========================================================

        private void Camara_NewFrame(
            object sender,
            NewFrameEventArgs eventArgs)
        {
            Bitmap frame =
                (Bitmap)eventArgs.Frame.Clone();

            try
            {
                if (IsDisposed ||
                    pictureBox1.IsDisposed)
                {
                    frame.Dispose();

                    return;
                }

                if (pictureBox1.InvokeRequired)
                {
                    pictureBox1.BeginInvoke(
                        new Action(
                            () =>
                            {
                                if (IsDisposed ||
                                    pictureBox1.IsDisposed)
                                {
                                    frame.Dispose();

                                    return;
                                }

                                MostrarImagenEnPictureBox(
                                    frame);
                            }));
                }
                else
                {
                    MostrarImagenEnPictureBox(
                        frame);
                }
            }
            catch
            {
                frame.Dispose();
            }
        }

        // =========================================================
        // INICIAR / DETENER CÁMARA
        // =========================================================

        private void btnTomar_Click(
            object sender,
            EventArgs e)
        {
            if (camara is not null &&
                camara.IsRunning)
            {
                DetenerCamara(
                    limpiarImagen: true);

                capturaDesdeCamara =
                    false;

                if (fotoCapturada is not null)
                {
                    fotoCapturada.Dispose();

                    fotoCapturada =
                        null;
                }

                if (string.IsNullOrWhiteSpace(
                    rutaFotoSeleccionada))
                {
                    ActualizarEstadoFoto(
                        "SIN_FOTO");
                }
                else
                {
                    ActualizarEstadoFoto(
                        "SELECCIONADA");
                }

                ActualizarBotonesCamara();

                MostrarEstado(
                    "Cámara detenida.");

                return;
            }

            if (cbCamaras.SelectedItem is null)
            {
                MessageBox.Show(
                    "Seleccione una cámara.",
                    "Cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                MostrarEstado(
                    "Seleccione una cámara para iniciar.",
                    true);

                return;
            }

            FilterInfo camaraSeleccionada =
                (FilterInfo)cbCamaras.SelectedItem;

            camara =
                new VideoCaptureDevice(
                    camaraSeleccionada.MonikerString);

            camara.NewFrame +=
                Camara_NewFrame;

            camara.Start();

            capturaDesdeCamara =
                false;

            if (fotoCapturada is not null)
            {
                fotoCapturada.Dispose();

                fotoCapturada =
                    null;
            }

            ActualizarBotonesCamara();

            ActualizarEstadoFoto(
                "CAMARA_ACTIVA");

            MostrarEstado(
                "Cámara activa. Capture la fotografía antes de guardar.",
                true);
        }

        // =========================================================
        // CAPTURAR FOTO
        // =========================================================

        private void btnCapturar_Click(
            object sender,
            EventArgs e)
        {
            if (camara is null ||
                !camara.IsRunning)
            {
                MessageBox.Show(
                    "La cámara no está activa.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (pictureBox1.Image is null)
            {
                MessageBox.Show(
                    "Todavía no se ha recibido una imagen de la cámara.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            fotoCapturada =
                new Bitmap(
                    pictureBox1.Image);

            capturaDesdeCamara =
                true;

            rutaFotoSeleccionada =
                null;

            DetenerCamara(
                limpiarImagen: false);

            ActualizarEstadoFoto(
                "CAPTURADA");

            ActualizarBotonesCamara();

            epValidacion.SetError(
                pictureBox1,
                string.Empty);

            MostrarEstado(
                "Fotografía capturada correctamente.");
        }

        // =========================================================
        // EVENTOS VALIDACIÓN
        // =========================================================

        private void cbLocalidad_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            epValidacion.SetError(
                cbLocalidad,
                string.Empty);
        }

        private void txtNoReloj_TextChanged(
            object sender,
            EventArgs e)
        {
            epValidacion.SetError(
                txtNoReloj,
                string.Empty);
        }

        private void txtNombre_TextChanged(
            object sender,
            EventArgs e)
        {
            epValidacion.SetError(
                txtNombre,
                string.Empty);

            int posicion =
                txtNombre.SelectionStart;

            string textoMayusculas =
                txtNombre.Text.ToUpper();

            if (txtNombre.Text !=
                textoMayusculas)
            {
                txtNombre.Text =
                    textoMayusculas;

                txtNombre.SelectionStart =
                    Math.Min(
                        posicion,
                        txtNombre.Text.Length);
            }
        }

        private void cbTurno_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            epValidacion.SetError(
                cbTurno,
                string.Empty);
        }

        private void cbLinea_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            epValidacion.SetError(
                cbLinea,
                string.Empty);
        }

        // =========================================================
        // ENTER / NÚMERO DE RELOJ
        // =========================================================

        private void txtNoReloj_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar ==
                (char)Keys.Enter)
            {
                SelectNextControl(
                    (Control)sender,
                    true,
                    true,
                    true,
                    true);

                e.Handled =
                    true;

                return;
            }

            if (!char.IsControl(
                    e.KeyChar) &&
                !char.IsDigit(
                    e.KeyChar))
            {
                e.Handled =
                    true;
            }
        }

        private void txtNombre_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar ==
                (char)Keys.Enter)
            {
                SelectNextControl(
                    (Control)sender,
                    true,
                    true,
                    true,
                    true);

                e.Handled =
                    true;
            }
        }

        private void ComboBox_KeyPress_Avanzar(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar ==
                (char)Keys.Enter)
            {
                SelectNextControl(
                    (Control)sender,
                    true,
                    true,
                    true,
                    true);

                e.Handled =
                    true;
            }
        }

        // =========================================================
        // VISTA PREVIA
        // =========================================================

        private void MostrarVistaPreviaFoto()
        {
            if (pictureBox1.Image is null)
            {
                MessageBox.Show(
                    "No hay fotografía para mostrar.",
                    "Vista previa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using Form ventanaFoto =
                new Form
                {
                    Text =
                        "Vista previa de fotografía",

                    StartPosition =
                        FormStartPosition.CenterParent,

                    Size =
                        new Size(
                            650,
                            650),

                    MinimizeBox =
                        false,

                    MaximizeBox =
                        false,

                    FormBorderStyle =
                        FormBorderStyle.FixedDialog,

                    BackColor =
                        AppColors.AppBackground
                };

            PictureBox pbVista =
                new PictureBox
                {
                    Dock =
                        DockStyle.Fill,

                    SizeMode =
                        PictureBoxSizeMode.Zoom,

                    BackColor =
                        Color.Black,

                    Image =
                        new Bitmap(
                            pictureBox1.Image)
                };

            Button btnCerrar =
                new Button();

            ButtonStyler.Apply(
                btnCerrar,
                "Cerrar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 140);

            btnCerrar.Dock =
                DockStyle.Bottom;

            btnCerrar.Height =
                44;

            btnCerrar.Click +=
                (_, _) =>
                {
                    ventanaFoto.Close();
                };

            ventanaFoto.Controls.Add(
                pbVista);

            ventanaFoto.Controls.Add(
                btnCerrar);

            ventanaFoto.FormClosed +=
                (_, _) =>
                {
                    if (pbVista.Image is not null)
                    {
                        pbVista.Image.Dispose();

                        pbVista.Image =
                            null;
                    }
                };

            ventanaFoto.ShowDialog(
                this);
        }

        // =========================================================
        // ESTADO BOTONES CÁMARA
        // =========================================================

        private void ActualizarBotonesCamara()
        {
            bool camaraActiva =
                camara is not null &&
                camara.IsRunning;

            bool hayCamaraDisponible =
                cbCamaras.Enabled &&
                cbCamaras.Items.Count > 0;

            btnIniciarCamara.Enabled =
                hayCamaraDisponible;

            btnCapturar.Enabled =
                camaraActiva;

            btnIniciarCamara.Text =
                camaraActiva
                    ? "Detener cámara"
                    : "Iniciar cámara";

            btnCapturar.BackColor =
                btnCapturar.Enabled
                    ? AppColors.Primary
                    : AppColors.DisabledBackground;

            btnCapturar.ForeColor =
                btnCapturar.Enabled
                    ? Color.White
                    : AppColors.DisabledText;

            btnIniciarCamara.BackColor =
                btnIniciarCamara.Enabled
                    ? AppColors.Secondary
                    : AppColors.DisabledBackground;

            btnIniciarCamara.ForeColor =
                btnIniciarCamara.Enabled
                    ? Color.White
                    : AppColors.DisabledText;
        }

        // =========================================================
        // QUITAR FOTO
        // =========================================================

        private void btnQuitarFoto_Click(
            object sender,
            EventArgs e)
        {
            DialogResult respuesta =
                MessageBox.Show(
                    "¿Desea quitar la fotografía actual?",
                    "Quitar fotografía",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (respuesta !=
                DialogResult.Yes)
            {
                return;
            }

            LimpiarFotoActual();
        }

        // =========================================================
        // DOBLE CLIC FOTO
        // =========================================================

        private void pictureBox1_DoubleClick(
            object sender,
            EventArgs e)
        {
            MostrarVistaPreviaFoto();
        }

        // =========================================================
        // MENÚ CONTEXTUAL FOTO
        // =========================================================

        private void tsmVerFotoGrande_Click(
            object sender,
            EventArgs e)
        {
            MostrarVistaPreviaFoto();
        }

        private void tsmSeleccionarFoto_Click(
            object sender,
            EventArgs e)
        {
            btnSeleccionar_Click(
                sender,
                e);
        }

        private void tsmQuitarFoto_Click(
            object sender,
            EventArgs e)
        {
            btnQuitarFoto_Click(
                sender,
                e);
        }

        private void cmsFoto_Opening(
            object sender,
            CancelEventArgs e)
        {
            bool hayFoto =
                pictureBox1.Image is not null;

            tsmVerFotoGrande.Enabled =
                hayFoto;

            tsmQuitarFoto.Enabled =
                hayFoto;
        }

        // =========================================================
        // RECARGAR CÁMARAS
        // =========================================================

        private void btnRecargarCamaras_Click(
            object sender,
            EventArgs e)
        {
            if (camara is not null &&
                camara.IsRunning)
            {
                DetenerCamara(
                    limpiarImagen: false);
            }

            cbCamaras.DataSource =
                null;

            dispositivosVideo =
                null;

            CargarCamaras();

            ActualizarBotonesCamara();

            if (cbCamaras.Items.Count > 0)
            {
                MostrarEstado(
                    "Cámaras recargadas correctamente.");
            }
            else
            {
                MostrarEstado(
                    "No se detectaron cámaras. Puede seleccionar una imagen.",
                    true);
            }
        }
    }
}