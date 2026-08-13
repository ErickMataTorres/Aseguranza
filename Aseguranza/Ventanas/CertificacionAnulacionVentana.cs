using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CertificacionAnulacionVentana : Form
    {
        private readonly int idCertificacion;
        private readonly string noReloj;
        private readonly string nombreTrabajador;
        private readonly string proceso;

        private CertificacionAnulacion? anulacionActual;
        private bool tieneAnulacionActivaActual;

        private Panel? _pnlControlesDatosOcultos;
        private Panel? _pnlFechaInicio;
        private Panel? _pnlFechaFin;

        private Panel _pnlTipo =
            null!;

        private Panel _pnlComentarioInput =
            null!;

        private readonly ErrorProvider _epValidacion =
            new ErrorProvider();

        private Label? _lblFechaInicioValor;
        private Label? _lblFechaFinValor;
        private Button? _btnFechaInicio;
        private Button? _btnFechaFin;

        public CertificacionAnulacionVentana(
            int idCertificacion,
            string noReloj,
            string nombreTrabajador,
            string proceso)
        {
            InitializeComponent();

            _epValidacion.ContainerControl =
                this;

            _epValidacion.BlinkStyle =
                ErrorBlinkStyle.NeverBlink;

            this.idCertificacion = idCertificacion;
            this.noReloj = noReloj;
            this.nombreTrabajador = nombreTrabajador;
            this.proceso = proceso;

            DoubleBuffered = true;

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void CertificacionAnulacionVentana_Load(
            object sender,
            EventArgs e)
        {
            lblTrabajador.Text =
                $"{noReloj} - {nombreTrabajador}";

            lblProceso.Text =
                proceso;

            CargarTiposAnulacion();
            ConfigurarEstadoInicial();
            CargarAnulacionActual();
            ConfigurarToolTips();

            ActualizarFechaVisualInicio();
            ActualizarFechaVisualFin();
            ActualizarResumenAnulacion();
        }

        // =========================================================
        // INTERFAZ MODERNA
        // =========================================================

        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            FormStyler.ApplyBase(
                this,
                "Anulación de certificación",
                new Size(
                    900,
                    690));

            FormStyler.CreateHeader(
                this,
                "Anulación de certificación",
                "Administra una anulación temporal o permanente de la certificación.",
                height: 100,
                titleX: 40,
                titleY: 18,
                subtitleX: 42,
                subtitleY: 58);

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        120),
                    new Size(
                        860,
                        550),
                    radius: 14);

            // Los GroupBox originales se conservan en el Designer,
            // pero la interfaz moderna usa paneles propios.
            gbInformacion.Visible = false;
            gbDatosAnulacion.Visible = false;
            gbComentario.Visible = false;
            lblTitulo.Visible = false;

            // =====================================================
            // CONTENEDOR INVISIBLE DE DATOS
            // =====================================================

            _pnlControlesDatosOcultos =
                new Panel
                {
                    Visible = false,
                    TabStop = false,
                    Location = new Point(-10000, -10000),
                    Size = new Size(1, 1)
                };

            Controls.Add(
                _pnlControlesDatosOcultos);

            _pnlControlesDatosOcultos.SendToBack();

            PrepararDateTimePickerOculto(
                dtpFechaInicio);

            PrepararDateTimePickerOculto(
                dtpFechaFin);

            // =====================================================
            // INFORMACIÓN DE LA CERTIFICACIÓN
            // =====================================================

            Panel pnlInformacion =
                new Panel
                {
                    Location = new Point(20, 16),
                    Size = new Size(820, 100),
                    BackColor = AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlInformacion,
                10);

            Label lblTituloInformacion =
                new Label
                {
                    AutoSize = true,
                    Text = "Información de la certificación",
                    Location = new Point(18, 13),
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(13F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            Label lblTrabajadorTitulo =
                CrearEtiquetaCampo(
                    "Trabajador",
                    new Point(18, 49));

            ConfigurarEtiquetaValor(
                lblTrabajador,
                new Point(105, 45),
                new Size(485, 28));

            Label lblProcesoTitulo =
                CrearEtiquetaCampo(
                    "Proceso",
                    new Point(18, 76));

            ConfigurarEtiquetaValor(
                lblProceso,
                new Point(86, 72),
                new Size(500, 28));

            ConfigurarEstadoVisual();

            lblEstadoAnulacion.Location =
                new Point(610, 25);

            lblEstadoAnulacion.Size =
                new Size(190, 48);

            pnlInformacion.Controls.Add(
                lblTituloInformacion);

            pnlInformacion.Controls.Add(
                lblTrabajadorTitulo);

            pnlInformacion.Controls.Add(
                lblTrabajador);

            pnlInformacion.Controls.Add(
                lblProcesoTitulo);

            pnlInformacion.Controls.Add(
                lblProceso);

            pnlInformacion.Controls.Add(
                lblEstadoAnulacion);

            pnlContenido.Controls.Add(
                pnlInformacion);

            // =====================================================
            // DATOS DE ANULACIÓN
            // =====================================================

            Panel pnlDatos =
                new Panel
                {
                    Location = new Point(20, 132),
                    Size = new Size(820, 170),
                    BackColor = AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlDatos,
                10);

            Label lblTituloDatos =
                new Label
                {
                    AutoSize = true,
                    Text = "Datos de anulación",
                    Location = new Point(18, 13),
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(13F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            Label lblAyudaDatos =
                new Label
                {
                    AutoSize = true,
                    Text = "Selecciona el tipo de anulación y define el periodo correspondiente.",
                    Location = new Point(18, 42),
                    ForeColor = AppColors.TextSecondary,
                    Font = AppFonts.Light(10F),
                    BackColor = Color.Transparent
                };

            pnlDatos.Controls.Add(
                lblTituloDatos);

            pnlDatos.Controls.Add(
                lblAyudaDatos);

            // Tipo
            ConfigurarEtiquetaFormulario(
                lblTipo,
                "Tipo de anulación *",
                new Point(18, 76));

            _pnlTipo =
                CrearPanelCampo(
                    18,
                    99,
                    245,
                    42);

            ConfigurarComboBox(
                cbTipoAnulacion,
                _pnlTipo);

            ConfigurarIndicadorValidacion(
                _pnlTipo);

            pnlDatos.Controls.Add(
                lblTipo);

            pnlDatos.Controls.Add(
                _pnlTipo);

            // Fecha inicio
            ConfigurarEtiquetaFormulario(
                lblFechaInicio,
                "Fecha de inicio *",
                new Point(285, 76));

            _pnlFechaInicio =
                CrearPanelCampo(
                    285,
                    99,
                    245,
                    42);

            ConfigurarSelectorFechaInicio(
                _pnlFechaInicio);

            ConfigurarIndicadorValidacion(
                _pnlFechaInicio);

            pnlDatos.Controls.Add(
                lblFechaInicio);

            pnlDatos.Controls.Add(
                _pnlFechaInicio);

            // Fecha fin
            ConfigurarEtiquetaFormulario(
                lblFechaFin,
                "Fecha de fin",
                new Point(552, 76));

            _pnlFechaFin =
                CrearPanelCampo(
                    552,
                    99,
                    250,
                    42);

            ConfigurarSelectorFechaFin(
                _pnlFechaFin);

            ConfigurarIndicadorValidacion(
                _pnlFechaFin);

            pnlDatos.Controls.Add(
                lblFechaFin);

            pnlDatos.Controls.Add(
                _pnlFechaFin);

            pnlContenido.Controls.Add(
                pnlDatos);

            // =====================================================
            // MOTIVO
            // =====================================================

            Panel pnlComentario =
                new Panel
                {
                    Location = new Point(20, 318),
                    Size = new Size(820, 104),
                    BackColor = AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlComentario,
                10);

            Label lblTituloComentario =
                new Label
                {
                    AutoSize = true,
                    Text = "Motivo de la anulación *",
                    Location = new Point(18, 12),
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(13F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            _pnlComentarioInput =
                CrearPanelCampo(
                    18,
                    43,
                    784,
                    48);

            ConfigurarTextBoxMultilinea(
                txtComentario,
                _pnlComentarioInput);

            ConfigurarIndicadorValidacion(
                _pnlComentarioInput);

            pnlComentario.Controls.Add(
                lblTituloComentario);

            pnlComentario.Controls.Add(
                _pnlComentarioInput);

            pnlContenido.Controls.Add(
                pnlComentario);

            // =====================================================
            // RESUMEN
            // =====================================================

            lblResumenAnulacion.Parent =
                pnlContenido;

            lblResumenAnulacion.Location =
                new Point(20, 438);

            lblResumenAnulacion.Size =
                new Size(820, 40);

            lblResumenAnulacion.BorderStyle =
                BorderStyle.None;

            lblResumenAnulacion.BackColor =
                Color.FromArgb(237, 233, 254);

            lblResumenAnulacion.ForeColor =
                Color.FromArgb(91, 33, 182);

            lblResumenAnulacion.Font =
                AppFonts.Regular(9.5F, FontStyle.Bold);

            lblResumenAnulacion.TextAlign =
                ContentAlignment.MiddleLeft;

            lblResumenAnulacion.Padding =
                new Padding(14, 0, 14, 0);

            lblResumenAnulacion.AutoEllipsis =
                true;

            RoundedControlHelper.ApplyRoundedRegion(
                lblResumenAnulacion,
                6);

            // =====================================================
            // BOTONES
            // =====================================================

            btnGuardar.Parent =
                pnlContenido;

            btnEliminar.Parent =
                pnlContenido;

            btnRegresar.Parent =
                pnlContenido;

            ButtonStyler.Apply(
                btnGuardar,
                "Guardar anulación",
                AppColors.Primary,
                AppIcons.Save,
                width: 182,
                height: 42);

            ButtonStyler.Apply(
                btnEliminar,
                "Eliminar anulación",
                AppColors.Danger,
                AppIcons.Delete,
                width: 178,
                height: 42);

            ButtonStyler.Apply(
                btnRegresar,
                "Regresar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 140,
                height: 42);

            btnGuardar.Location =
                new Point(20, 494);

            btnEliminar.Location =
                new Point(214, 494);

            btnRegresar.Location =
                new Point(700, 494);

            btnGuardar.Anchor =
                AnchorStyles.Left |
                AnchorStyles.Bottom;

            btnEliminar.Anchor =
                AnchorStyles.Left |
                AnchorStyles.Bottom;

            btnRegresar.Anchor =
                AnchorStyles.Right |
                AnchorStyles.Bottom;

            ResumeLayout(true);
        }

        // =========================================================
        // HELPERS VISUALES
        // =========================================================

        private static Label CrearEtiquetaCampo(
            string texto,
            Point location)
        {
            return new Label
            {
                AutoSize = true,
                Text = texto,
                Location = location,
                ForeColor = AppColors.TextSecondary,
                Font = AppFonts.Regular(10F, FontStyle.Bold),
                BackColor = Color.Transparent
            };
        }

        private static void ConfigurarEtiquetaValor(
            Label label,
            Point location,
            Size size)
        {
            label.AutoSize = false;
            label.Location = location;
            label.Size = size;
            label.ForeColor = AppColors.TextPrimary;
            label.Font = AppFonts.Regular(12F, FontStyle.Bold);
            label.BackColor = Color.Transparent;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoEllipsis = true;
        }

        private static void ConfigurarEtiquetaFormulario(
            Label label,
            string texto,
            Point location)
        {
            label.Text = texto;
            label.AutoSize = true;
            label.Location = location;
            label.ForeColor = AppColors.TextPrimary;
            label.Font = AppFonts.Regular(10F, FontStyle.Bold);
            label.BackColor = Color.Transparent;
        }

        private static Panel CrearPanelCampo(
            int x,
            int y,
            int width,
            int height)
        {
            return new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.White
            };
        }

        private static void ConfigurarComboBox(
            ComboBox comboBox,
            Panel container)
        {
            comboBox.Parent?.Controls.Remove(
                comboBox);

            comboBox.Location =
                new Point(8, 7);

            comboBox.Size =
                new Size(
                    container.Width - 16,
                    27);

            comboBox.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            ComboBoxStyler.ApplyOutlinedComboBox(
                container,
                comboBox);

            container.Controls.Add(
                comboBox);
        }

        private static void ConfigurarTextBoxMultilinea(
            TextBox textBox,
            Panel container)
        {
            textBox.Parent?.Controls.Remove(
                textBox);

            textBox.Location =
                new Point(10, 7);

            textBox.Size =
                new Size(
                    container.Width - 20,
                    container.Height - 14);

            textBox.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            textBox.Multiline = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = Color.White;
            textBox.ForeColor = AppColors.TextPrimary;
            textBox.Font = AppFonts.Regular(10.5F);

            container.Controls.Add(
                textBox);

            InputStyler.ApplyOutlinedInput(
                container,
                textBox);
        }

        private void PrepararDateTimePickerOculto(
            DateTimePicker dateTimePicker)
        {
            dateTimePicker.Parent?.Controls.Remove(
                dateTimePicker);

            dateTimePicker.Visible = false;
            dateTimePicker.TabStop = false;
            dateTimePicker.Location = new Point(0, 0);
            dateTimePicker.Size = new Size(1, 1);

            if (_pnlControlesDatosOcultos is null)
            {
                throw new InvalidOperationException(
                    "No se inicializó el contenedor de controles de datos ocultos.");
            }

            _pnlControlesDatosOcultos.Controls.Add(
                dateTimePicker);

            dateTimePicker.Visible = false;
        }

        private void ConfigurarSelectorFechaInicio(
            Panel container)
        {
            container.BackColor = Color.White;

            RoundedControlHelper.ApplyRoundedRegion(
                container,
                7);

            _lblFechaInicioValor =
                CrearLabelFecha(
                    container,
                    conBoton: true);

            _btnFechaInicio =
                CrearBotonCalendario();

            _btnFechaInicio.Location =
                new Point(
                    container.Width - 44,
                    6);

            _btnFechaInicio.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            container.Controls.Add(
                _lblFechaInicioValor);

            container.Controls.Add(
                _btnFechaInicio);

            _btnFechaInicio.BringToFront();

            EventHandler abrir =
                (_, _) => AbrirSelectorFechaInicio();

            container.Cursor = Cursors.Hand;
            _lblFechaInicioValor.Cursor = Cursors.Hand;

            container.Click += abrir;
            _lblFechaInicioValor.Click += abrir;
            _btnFechaInicio.Click += abrir;

            ConfigurarBordeSelectorFecha(
                container,
                _btnFechaInicio);
        }

        private void ConfigurarSelectorFechaFin(
            Panel container)
        {
            container.BackColor = Color.White;

            RoundedControlHelper.ApplyRoundedRegion(
                container,
                7);

            _lblFechaFinValor =
                CrearLabelFecha(
                    container,
                    conBoton: true);

            _btnFechaFin =
                CrearBotonCalendario();

            _btnFechaFin.Location =
                new Point(
                    container.Width - 44,
                    6);

            _btnFechaFin.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            container.Controls.Add(
                _lblFechaFinValor);

            container.Controls.Add(
                _btnFechaFin);

            _btnFechaFin.BringToFront();

            EventHandler abrir =
                (_, _) => AbrirSelectorFechaFin();

            container.Click += abrir;
            _lblFechaFinValor.Click += abrir;
            _btnFechaFin.Click += abrir;

            ConfigurarBordeSelectorFecha(
                container,
                _btnFechaFin);
        }

        private static Label CrearLabelFecha(
            Panel container,
            bool conBoton)
        {
            return new Label
            {
                AutoSize = false,
                Location = new Point(12, 7),
                Size = new Size(
                    conBoton
                        ? container.Width - 62
                        : container.Width - 24,
                    28),
                ForeColor = AppColors.TextPrimary,
                Font = AppFonts.Regular(11F),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
        }

        private static Button CrearBotonCalendario()
        {
            Button button =
                new Button
                {
                    Text = string.Empty,
                    Size = new Size(36, 30),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = AppColors.Primary,
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false,
                    TabStop = true
                };

            button.FlatAppearance.BorderSize = 0;

            button.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(
                    AppColors.Primary,
                    0.05F);

            button.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(
                    AppColors.Primary,
                    0.10F);

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                6);

            button.Paint +=
                (_, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    using Pen pen =
                        new Pen(
                            Color.White,
                            1.6F);

                    Rectangle cuerpo =
                        new Rectangle(
                            10,
                            9,
                            16,
                            14);

                    e.Graphics.DrawRectangle(
                        pen,
                        cuerpo);

                    e.Graphics.DrawLine(
                        pen,
                        10,
                        13,
                        26,
                        13);

                    e.Graphics.DrawLine(
                        pen,
                        14,
                        7,
                        14,
                        11);

                    e.Graphics.DrawLine(
                        pen,
                        22,
                        7,
                        22,
                        11);

                    using SolidBrush brush =
                        new SolidBrush(
                            Color.White);

                    e.Graphics.FillRectangle(brush, 13, 16, 2, 2);
                    e.Graphics.FillRectangle(brush, 17, 16, 2, 2);
                    e.Graphics.FillRectangle(brush, 21, 16, 2, 2);
                };

            return button;
        }

        private static void ConfigurarBordeSelectorFecha(
            Panel container,
            Button button)
        {
            button.Enter +=
                (_, _) => container.Invalidate();

            button.Leave +=
                (_, _) => container.Invalidate();

            container.Paint +=
                (_, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    Rectangle rect =
                        new Rectangle(
                            0,
                            0,
                            container.Width - 1,
                            container.Height - 1);

                    using GraphicsPath path =
                        RoundedControlHelper.CreateRoundedPath(
                            rect,
                            7);

                    Color colorBorde =
                        button.Focused
                            ? AppColors.Primary
                            : AppColors.InputBorder;

                    float grosor =
                        button.Focused
                            ? 1.8F
                            : 1F;

                    using Pen pen =
                        new Pen(
                            colorBorde,
                            grosor);

                    e.Graphics.DrawPath(
                        pen,
                        path);
                };
        }

        // =========================================================
        // INDICADORES DE VALIDACIÓN
        // =========================================================

        private void ConfigurarIndicadorValidacion(
            Control control)
        {
            _epValidacion.SetIconAlignment(
                control,
                ErrorIconAlignment.MiddleRight);

            _epValidacion.SetIconPadding(
                control,
                2);
        }

        private void LimpiarErroresValidacion()
        {
            _epValidacion.SetError(
                _pnlTipo,
                string.Empty);

            if (_pnlFechaInicio is not null)
            {
                _epValidacion.SetError(
                    _pnlFechaInicio,
                    string.Empty);
            }

            if (_pnlFechaFin is not null)
            {
                _epValidacion.SetError(
                    _pnlFechaFin,
                    string.Empty);
            }

            _epValidacion.SetError(
                _pnlComentarioInput,
                string.Empty);
        }

        // =========================================================
        // SELECTORES DE FECHA
        // =========================================================

        private void AbrirSelectorFechaInicio()
        {
            if (AppDatePickerDialog.TrySelectDate(
                    this,
                    dtpFechaInicio.Value.Date,
                    out DateTime fechaSeleccionada,
                    fechaMinima: new DateTime(1753, 1, 1),
                    fechaMaxima: new DateTime(9998, 12, 31)))
            {
                dtpFechaInicio.Value =
                    fechaSeleccionada.Date;
            }
        }

        private void AbrirSelectorFechaFin()
        {
            if (cbTipoAnulacion.Text != "Hasta fecha")
            {
                return;
            }

            if (AppDatePickerDialog.TrySelectDate(
                    this,
                    dtpFechaFin.Value.Date,
                    out DateTime fechaSeleccionada,
                    fechaMinima: dtpFechaInicio.Value.Date,
                    fechaMaxima: new DateTime(9998, 12, 31)))
            {
                dtpFechaFin.Value =
                    fechaSeleccionada.Date;
            }
        }

        private void ActualizarFechaVisualInicio()
        {
            if (_lblFechaInicioValor is null)
            {
                return;
            }

            _lblFechaInicioValor.Text =
                dtpFechaInicio.Value
                    .ToString("dd/MM/yyyy");
        }

        private void ActualizarFechaVisualFin()
        {
            if (_lblFechaFinValor is null)
            {
                return;
            }

            string tipo =
                cbTipoAnulacion.SelectedItem is null
                    ? string.Empty
                    : cbTipoAnulacion.Text;

            if (tipo == "Permanente")
            {
                _lblFechaFinValor.Text =
                    "Sin fecha fin";
            }
            else
            {
                _lblFechaFinValor.Text =
                    dtpFechaFin.Value
                        .ToString("dd/MM/yyyy");
            }

            bool editable =
                tipo == "Hasta fecha";

            if (_pnlFechaFin is not null)
            {
                _pnlFechaFin.Cursor =
                    editable
                        ? Cursors.Hand
                        : Cursors.Default;

                _pnlFechaFin.BackColor =
                    editable
                        ? Color.White
                        : AppColors.AppBackground;
            }

            if (_lblFechaFinValor is not null)
            {
                _lblFechaFinValor.Cursor =
                    editable
                        ? Cursors.Hand
                        : Cursors.Default;

                _lblFechaFinValor.ForeColor =
                    editable
                        ? AppColors.TextPrimary
                        : AppColors.TextSecondary;
            }

            if (_btnFechaFin is not null)
            {
                _btnFechaFin.Visible =
                    editable;

                _btnFechaFin.Enabled =
                    editable;
            }
        }

        // =========================================================
        // TOOL TIPS
        // =========================================================

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(
                cbTipoAnulacion,
                "Seleccione cuánto tiempo estará anulada la certificación.");

            if (_pnlFechaInicio is not null)
            {
                ttAyuda.SetToolTip(
                    _pnlFechaInicio,
                    "Fecha desde la cual inicia la anulación.");
            }

            if (_btnFechaInicio is not null)
            {
                ttAyuda.SetToolTip(
                    _btnFechaInicio,
                    "Seleccionar fecha de inicio.");
            }

            if (_pnlFechaFin is not null)
            {
                ttAyuda.SetToolTip(
                    _pnlFechaFin,
                    "La fecha fin se calcula automáticamente, excepto para 'Hasta fecha'.");
            }

            if (_btnFechaFin is not null)
            {
                ttAyuda.SetToolTip(
                    _btnFechaFin,
                    "Seleccionar fecha de fin.");
            }

            ttAyuda.SetToolTip(
                txtComentario,
                "Explique claramente el motivo de la anulación.");

            ttAyuda.SetToolTip(
                btnGuardar,
                "Guardar o modificar la anulación.");

            ttAyuda.SetToolTip(
                btnEliminar,
                "Eliminar la anulación activa de esta certificación.");

            ttAyuda.SetToolTip(
                btnRegresar,
                "Cerrar esta ventana.");

            ttAyuda.SetToolTip(
                lblEstadoAnulacion,
                "Indica si esta certificación tiene una anulación activa.");
        }

        // =========================================================
        // TIPOS DE ANULACIÓN
        // =========================================================

        private void CargarTiposAnulacion()
        {
            cbTipoAnulacion.Items.Clear();

            cbTipoAnulacion.Items.Add("1 día");
            cbTipoAnulacion.Items.Add("1 semana");
            cbTipoAnulacion.Items.Add("15 días");
            cbTipoAnulacion.Items.Add("1 mes");
            cbTipoAnulacion.Items.Add("3 meses");
            cbTipoAnulacion.Items.Add("Hasta fecha");
            cbTipoAnulacion.Items.Add("Permanente");

            cbTipoAnulacion.SelectedIndex = 0;
        }

        // =========================================================
        // ESTADO INICIAL
        // =========================================================

        private void ConfigurarEstadoInicial()
        {
            dtpFechaInicio.Value =
                DateTime.Today;

            dtpFechaFin.Value =
                DateTime.Today.AddDays(1);

            btnGuardar.Text =
                "Guardar anulación";

            btnEliminar.Enabled =
                false;

            MostrarEstadoAnulacion(
                false);

            ActualizarEstadoBotonEliminar();
            ActualizarFechaVisualInicio();
            ActualizarFechaVisualFin();
        }

        // =========================================================
        // CARGAR ANULACIÓN EXISTENTE
        // =========================================================

        private void CargarAnulacionActual()
        {
            try
            {
                anulacionActual =
                    CertificacionAnulacion
                        .ConsultarAnulacionPorCertificacion(
                            idCertificacion);

                if (anulacionActual == null)
                {
                    MostrarEstadoAnulacion(
                        false);

                    ActualizarEstadoBotonEliminar();
                    return;
                }

                MostrarEstadoAnulacion(
                    true);

                btnGuardar.Text =
                    "Modificar anulación";

                btnEliminar.Enabled =
                    true;

                txtComentario.Text =
                    anulacionActual.Comentario ??
                    string.Empty;

                dtpFechaInicio.Value =
                    anulacionActual.FechaInicio;

                if (anulacionActual.EsPermanente)
                {
                    cbTipoAnulacion.SelectedItem =
                        "Permanente";
                }
                else
                {
                    cbTipoAnulacion.SelectedItem =
                        anulacionActual.TipoAnulacion ??
                        "Hasta fecha";

                    if (anulacionActual.FechaFin.HasValue)
                    {
                        dtpFechaFin.Value =
                            anulacionActual.FechaFin.Value;
                    }
                }

                ActualizarFechaFinSegunTipo();
                ActualizarEstadoBotonEliminar();
                ActualizarFechaVisualInicio();
                ActualizarFechaVisualFin();
                LimpiarErroresValidacion();
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al consultar anulación",
                    error.Message);
            }
        }

        // =========================================================
        // EVENTOS DE FECHA / TIPO
        // =========================================================

        private void cmbTipoAnulacion_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cbTipoAnulacion.SelectedItem is not null)
            {
                _epValidacion.SetError(
                    _pnlTipo,
                    string.Empty);
            }

            if (cbTipoAnulacion.Text == "Permanente" &&
                _pnlFechaFin is not null)
            {
                _epValidacion.SetError(
                    _pnlFechaFin,
                    string.Empty);
            }

            ActualizarFechaFinSegunTipo();
            ActualizarFechaVisualFin();
            ActualizarResumenAnulacion();
        }

        private void dtpFechaInicio_ValueChanged(
            object sender,
            EventArgs e)
        {
            if (_pnlFechaInicio is not null)
            {
                _epValidacion.SetError(
                    _pnlFechaInicio,
                    string.Empty);
            }

            ActualizarFechaVisualInicio();
            ActualizarFechaFinSegunTipo();
            ActualizarFechaVisualFin();
            ActualizarResumenAnulacion();
        }

        private void dtpFechaFin_ValueChanged(
            object sender,
            EventArgs e)
        {
            if (_pnlFechaFin is not null &&
                dtpFechaFin.Value.Date >=
                dtpFechaInicio.Value.Date)
            {
                _epValidacion.SetError(
                    _pnlFechaFin,
                    string.Empty);
            }

            ActualizarFechaVisualFin();
            ActualizarResumenAnulacion();
        }

        private void txtComentario_TextChanged(
            object sender,
            EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(
                    txtComentario.Text))
            {
                _epValidacion.SetError(
                    _pnlComentarioInput,
                    string.Empty);
            }

            ActualizarResumenAnulacion();
        }

        private void ActualizarFechaFinSegunTipo()
        {
            if (cbTipoAnulacion.SelectedItem == null)
            {
                return;
            }

            string tipo =
                cbTipoAnulacion.Text;

            switch (tipo)
            {
                case "1 día":
                    dtpFechaFin.Value =
                        dtpFechaInicio.Value.Date.AddDays(1);
                    break;

                case "1 semana":
                    dtpFechaFin.Value =
                        dtpFechaInicio.Value.Date.AddDays(7);
                    break;

                case "15 días":
                    dtpFechaFin.Value =
                        dtpFechaInicio.Value.Date.AddDays(15);
                    break;

                case "1 mes":
                    dtpFechaFin.Value =
                        dtpFechaInicio.Value.Date.AddMonths(1);
                    break;

                case "3 meses":
                    dtpFechaFin.Value =
                        dtpFechaInicio.Value.Date.AddMonths(3);
                    break;

                case "Hasta fecha":
                    if (dtpFechaFin.Value.Date <
                        dtpFechaInicio.Value.Date)
                    {
                        dtpFechaFin.Value =
                            dtpFechaInicio.Value.Date;
                    }
                    break;

                case "Permanente":
                    break;
            }

            ActualizarFechaVisualFin();
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        private void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidarInformacion())
            {
                return;
            }

            string tipo =
                cbTipoAnulacion.Text;

            bool esPermanente =
                tipo == "Permanente";

            DateTime? fechaFin =
                esPermanente
                    ? null
                    : dtpFechaFin.Value.Date;

            CertificacionAnulacion anulacion =
                new CertificacionAnulacion
                {
                    IdCertificacion = idCertificacion,
                    TipoAnulacion = tipo,
                    FechaInicio = dtpFechaInicio.Value.Date,
                    FechaFin = fechaFin,
                    EsPermanente = esPermanente,
                    Comentario = txtComentario.Text.Trim()
                };

            Mensaje respuesta;

            if (anulacionActual == null)
            {
                respuesta =
                    anulacion
                        .GuardarCertificacionAnulacion();
            }
            else
            {
                anulacion.Id =
                    anulacionActual.Id;

                respuesta =
                    anulacion
                        .ModificarCertificacionAnulacion();
            }

            if (respuesta.Id == 1)
            {
                string accion =
                    anulacionActual == null
                        ? "guardó"
                        : "modificó";

                AppDialog.ShowInfo(
                    this,
                    "Anulación registrada",
                    $"La anulación se {accion} correctamente.\n\n" +
                    $"{noReloj} - {nombreTrabajador}\n" +
                    $"Proceso: {proceso}");

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            else
            {
                AppDialog.ShowError(
                    this,
                    "Error",
                    respuesta.Nombre);
            }
        }

        // =========================================================
        // VALIDACIÓN
        // =========================================================

        private bool ValidarInformacion()
        {
            LimpiarErroresValidacion();

            bool esValido =
                true;

            Control? primerControlInvalido =
                null;

            if (cbTipoAnulacion.SelectedItem is null)
            {
                _epValidacion.SetError(
                    _pnlTipo,
                    "Seleccione el tipo de anulación.");

                primerControlInvalido =
                    cbTipoAnulacion;

                esValido =
                    false;
            }

            if (dtpFechaInicio.Value.Date <
                new DateTime(1753, 1, 1))
            {
                if (_pnlFechaInicio is not null)
                {
                    _epValidacion.SetError(
                        _pnlFechaInicio,
                        "Seleccione una fecha de inicio válida.");
                }

                primerControlInvalido ??=
                    _btnFechaInicio;

                esValido =
                    false;
            }

            if (cbTipoAnulacion.SelectedItem is not null &&
                cbTipoAnulacion.Text != "Permanente" &&
                dtpFechaFin.Value.Date <
                dtpFechaInicio.Value.Date)
            {
                if (_pnlFechaFin is not null)
                {
                    _epValidacion.SetError(
                        _pnlFechaFin,
                        "La fecha fin no puede ser menor que la fecha de inicio.");
                }

                primerControlInvalido ??=
                    _btnFechaFin;

                esValido =
                    false;
            }

            if (string.IsNullOrWhiteSpace(
                    txtComentario.Text))
            {
                _epValidacion.SetError(
                    _pnlComentarioInput,
                    "Escriba el motivo de la anulación.");

                primerControlInvalido ??=
                    txtComentario;

                esValido =
                    false;
            }

            if (!esValido)
            {
                primerControlInvalido?.Focus();

                AppDialog.ShowWarning(
                    this,
                    "Información incompleta",
                    "Hay información incompleta o inválida. Revise los campos marcados.");
            }

            return esValido;
        }

        // =========================================================
        // ELIMINAR ANULACIÓN
        // =========================================================

        private void btnEliminar_Click(
            object sender,
            EventArgs e)
        {
            if (anulacionActual == null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Sin anulación",
                    "No existe una anulación activa para eliminar.");

                return;
            }

            bool confirmacion =
                AppDialog.Confirm(
                    this,
                    "Confirmar eliminación de anulación",
                    "¿Seguro que desea eliminar la anulación activa de esta certificación?\n\n" +
                    $"{noReloj} - {nombreTrabajador}\n" +
                    $"Proceso: {proceso}\n\n" +
                    "Esta acción permitirá volver a modificar o renovar la certificación.",
                    "Eliminar");

            if (!confirmacion)
            {
                return;
            }

            Mensaje respuesta =
                CertificacionAnulacion
                    .EliminarCertificacionAnulacion(
                        anulacionActual.Id);

            if (respuesta.Id == 1)
            {
                AppDialog.ShowInfo(
                    this,
                    "Anulación eliminada",
                    "La anulación se eliminó correctamente.\n\n" +
                    $"{noReloj} - {nombreTrabajador}\n" +
                    $"Proceso: {proceso}");

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            else
            {
                AppDialog.ShowError(
                    this,
                    "Error",
                    respuesta.Nombre);
            }
        }

        // =========================================================
        // REGRESAR
        // =========================================================

        private void btnRegresar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        // =========================================================
        // ESTADO DE ANULACIÓN
        // =========================================================

        private void ConfigurarEstadoVisual()
        {
            lblEstadoAnulacion.BorderStyle =
                BorderStyle.None;

            lblEstadoAnulacion.AutoSize =
                false;

            lblEstadoAnulacion.TextAlign =
                ContentAlignment.MiddleCenter;

            lblEstadoAnulacion.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            RoundedControlHelper.ApplyRoundedRegion(
                lblEstadoAnulacion,
                8);
        }

        private void MostrarEstadoAnulacion(
            bool tieneAnulacionActiva)
        {
            tieneAnulacionActivaActual =
                tieneAnulacionActiva;

            if (tieneAnulacionActiva)
            {
                lblEstadoAnulacion.Text =
                    "Anulación activa";

                lblEstadoAnulacion.BackColor =
                    Color.FromArgb(
                        221,
                        214,
                        254);

                lblEstadoAnulacion.ForeColor =
                    Color.FromArgb(
                        91,
                        33,
                        182);
            }
            else
            {
                lblEstadoAnulacion.Text =
                    "Sin anulación activa";

                lblEstadoAnulacion.BackColor =
                    Color.FromArgb(
                        220,
                        245,
                        225);

                lblEstadoAnulacion.ForeColor =
                    Color.FromArgb(
                        0,
                        120,
                        40);
            }

            lblEstadoAnulacion.Refresh();
        }

        private void ActualizarEstadoBotonEliminar()
        {
            ButtonStyler.UpdateEnabledState(
                btnEliminar,
                AppColors.Danger);
        }

        // =========================================================
        // RESUMEN
        // =========================================================

        private void ActualizarResumenAnulacion()
        {
            if (cbTipoAnulacion.SelectedItem == null)
            {
                lblResumenAnulacion.Text =
                    "Resumen: Sin datos capturados";

                return;
            }

            string tipo =
                cbTipoAnulacion.Text;

            string fechaInicio =
                dtpFechaInicio.Value
                    .ToString("dd/MM/yyyy");

            string fechaFin =
                tipo == "Permanente"
                    ? "Sin fecha fin"
                    : dtpFechaFin.Value
                        .ToString("dd/MM/yyyy");

            string comentario =
                txtComentario.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    comentario))
            {
                comentario =
                    "Sin comentario capturado";
            }

            lblResumenAnulacion.Text =
                $"Resumen: {tipo} | Inicio: {fechaInicio} | Fin: {fechaFin} | Motivo: {comentario}";
        }
    }
}
