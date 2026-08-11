using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CertificacionesTrabajadorVentana : Form
    {
        private Clases.Certificacion? certificacionActual;
        private Clases.Trabajador? trabajadorActual;
        private Button? _btnSelectorFechaCertificacion;
        private Panel? _pnlControlesDatosOcultos;

        public CertificacionesTrabajadorVentana(
            Certificacion? certificacion,
            Trabajador trabajador)
        {
            InitializeComponent();

            certificacionActual = certificacion;
            trabajadorActual = trabajador;

            DoubleBuffered = true;

            AplicarEstiloVisual();
        }

        // =========================================================
        // INTERFAZ
        // =========================================================

        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            bool esEdicion =
                certificacionActual is not null;

            string titulo =
                esEdicion
                    ? "Modificar / renovar certificación"
                    : "Registrar certificación";

            string subtitulo =
                esEdicion
                    ? "Actualiza la fecha, el certificador y la información de la certificación."
                    : "Captura la información necesaria para certificar al trabajador.";

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                titulo,
                new Size(
                    800,
                    660));

            Controls.Clear();

            // =====================================================
            // CONTROLES DE DATOS OCULTOS
            // =====================================================

            // Los DateTimePicker originales del Designer se conservan
            // únicamente como controles de datos. Permanecen dentro de
            // un contenedor invisible para que Windows nunca pueda
            // mostrar su calendario nativo.
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

            // =====================================================
            // CABECERA
            // =====================================================

            FormStyler.CreateHeader(
                this,
                titulo,
                subtitulo,
                height: 105,
                titleX: 38,
                titleY: 20,
                subtitleX: 40,
                subtitleY: 61);

            // =====================================================
            // TARJETA PRINCIPAL
            // =====================================================

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        125),
                    new Size(
                        760,
                        510),
                    radius: 14,
                    anchor:
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right);

            // =====================================================
            // INFORMACIÓN DEL TRABAJADOR
            // =====================================================

            Panel pnlTrabajador =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            18),

                    Size =
                        new Size(
                            720,
                            82),

                    BackColor =
                        AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlTrabajador,
                10);

            Label lblTituloTrabajador =
                new Label
                {
                    AutoSize = true,
                    Text = "Trabajador",
                    Location = new Point(18, 12),
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(13F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            Label lblNoRelojTitulo =
                CrearEtiquetaCampo(
                    "No. Reloj",
                    new Point(18, 48));

            Label lblNoRelojValor =
                CrearEtiquetaValor(
                    trabajadorActual?.NoReloj ?? "—",
                    new Point(94, 45),
                    new Size(125, 28));

            Label lblNombreTitulo =
                CrearEtiquetaCampo(
                    "Nombre",
                    new Point(242, 48));

            Label lblNombreValor =
                CrearEtiquetaValor(
                    trabajadorActual?.Nombre ?? "—",
                    new Point(306, 45),
                    new Size(392, 28));

            pnlTrabajador.Controls.Add(
                lblTituloTrabajador);

            pnlTrabajador.Controls.Add(
                lblNoRelojTitulo);

            pnlTrabajador.Controls.Add(
                lblNoRelojValor);

            pnlTrabajador.Controls.Add(
                lblNombreTitulo);

            pnlTrabajador.Controls.Add(
                lblNombreValor);

            pnlContenido.Controls.Add(
                pnlTrabajador);

            // =====================================================
            // DATOS DE LA CERTIFICACIÓN
            // =====================================================

            Panel pnlCertificacion =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            116),

                    Size =
                        new Size(
                            720,
                            336),

                    BackColor =
                        AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlCertificacion,
                10);

            Label lblTituloCertificacion =
                new Label
                {
                    AutoSize = true,
                    Text = "Información de la certificación",
                    Location = new Point(18, 14),
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(13F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            Label lblAyudaCertificacion =
                new Label
                {
                    AutoSize = true,
                    Text = esEdicion
                        ? "El proceso permanece bloqueado durante la modificación o renovación."
                        : "Selecciona el proceso, certificador y fecha de certificación.",
                    Location = new Point(18, 43),
                    ForeColor = AppColors.TextSecondary,
                    Font = AppFonts.Light(10F),
                    BackColor = Color.Transparent
                };

            pnlCertificacion.Controls.Add(
                lblTituloCertificacion);

            pnlCertificacion.Controls.Add(
                lblAyudaCertificacion);

            // =====================================================
            // PROCESO
            // =====================================================

            ConfigurarEtiquetaFormulario(
                lblProceso,
                "Proceso *",
                new Point(18, 78));

            Panel pnlProceso =
                CrearPanelCampo(
                    18,
                    101,
                    326,
                    42);

            ConfigurarComboBox(
                cbProcesos,
                pnlProceso);

            pnlCertificacion.Controls.Add(
                lblProceso);

            pnlCertificacion.Controls.Add(
                pnlProceso);

            // =====================================================
            // CERTIFICADOR
            // =====================================================

            ConfigurarEtiquetaFormulario(
                lblCertificador,
                "Certificador *",
                new Point(376, 78));

            Panel pnlCertificador =
                CrearPanelCampo(
                    376,
                    101,
                    326,
                    42);

            ConfigurarComboBox(
                cbCertificadores,
                pnlCertificador);

            pnlCertificacion.Controls.Add(
                lblCertificador);

            pnlCertificacion.Controls.Add(
                pnlCertificador);

            // =====================================================
            // FECHA DE CERTIFICACIÓN
            // =====================================================

            ConfigurarEtiquetaFormulario(
                lblFechaCertificacion,
                "Fecha de certificación *",
                new Point(18, 158));

            Panel pnlFechaCertificacion =
                CrearPanelCampo(
                    18,
                    181,
                    326,
                    42);

            ConfigurarSelectorFecha(
                dtpFechaCertificacion,
                pnlFechaCertificacion,
                habilitado: true);

            pnlCertificacion.Controls.Add(
                lblFechaCertificacion);

            pnlCertificacion.Controls.Add(
                pnlFechaCertificacion);

            // =====================================================
            // FECHA DE VENCIMIENTO
            // =====================================================

            ConfigurarEtiquetaFormulario(
                lblFechaVencimiento,
                "Fecha de vencimiento",
                new Point(376, 158));

            Panel pnlFechaVencimiento =
                CrearPanelCampo(
                    376,
                    181,
                    326,
                    42);

            ConfigurarSelectorFecha(
                dtpFechaVencimiento,
                pnlFechaVencimiento,
                habilitado: false);

            pnlCertificacion.Controls.Add(
                lblFechaVencimiento);

            pnlCertificacion.Controls.Add(
                pnlFechaVencimiento);

            // =====================================================
            // COMENTARIO
            // =====================================================

            ConfigurarEtiquetaFormulario(
                lblComentario,
                "Comentario",
                new Point(18, 238));

            Panel pnlComentario =
                CrearPanelCampo(
                    18,
                    261,
                    684,
                    58);

            ConfigurarTextBoxMultilinea(
                txtComentario,
                pnlComentario);

            pnlCertificacion.Controls.Add(
                lblComentario);

            pnlCertificacion.Controls.Add(
                pnlComentario);

            pnlContenido.Controls.Add(
                pnlCertificacion);

            // =====================================================
            // BOTONES
            // =====================================================

            ButtonStyler.Apply(
                btnAceptar,
                esEdicion
                    ? "Guardar cambios"
                    : "Guardar",
                AppColors.Primary,
                AppIcons.Save,
                width: esEdicion
                    ? 190
                    : 150,
                height: 42);

            btnAceptar.Location =
                new Point(
                    20,
                    466);

            ButtonStyler.Apply(
                btnRegresar,
                "Cancelar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 150,
                height: 42);

            btnRegresar.Location =
                new Point(
                    590,
                    466);

            btnRegresar.DialogResult =
                DialogResult.Cancel;

            pnlContenido.Controls.Add(
                btnAceptar);

            pnlContenido.Controls.Add(
                btnRegresar);

            AcceptButton =
                btnAceptar;

            CancelButton =
                btnRegresar;

            ResumeLayout(
                true);
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

        private static Label CrearEtiquetaValor(
            string texto,
            Point location,
            Size size)
        {
            return new Label
            {
                AutoSize = false,
                Text = texto,
                Location = location,
                Size = size,
                ForeColor = AppColors.TextPrimary,
                Font = AppFonts.Regular(12F, FontStyle.Bold),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };
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
            comboBox.Location =
                new Point(
                    8,
                    7);

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

        private void ConfigurarSelectorFecha(
            DateTimePicker dateTimePicker,
            Panel container,
            bool habilitado)
        {
            // El DateTimePicker original se conserva exclusivamente como
            // control de datos para no cambiar la lógica existente.
            // IMPORTANTE: no se agrega al panel visible. Se mueve a un
            // contenedor invisible para impedir por completo que Windows
            // muestre el calendario nativo.
            dateTimePicker.Parent?.Controls.Remove(
                dateTimePicker);

            dateTimePicker.Visible = false;
            dateTimePicker.TabStop = false;
            dateTimePicker.Enabled = habilitado;
            dateTimePicker.Location = new Point(0, 0);
            dateTimePicker.Size = new Size(1, 1);

            if (_pnlControlesDatosOcultos is null)
            {
                throw new InvalidOperationException(
                    "No se inicializó el contenedor de controles de datos ocultos.");
            }

            _pnlControlesDatosOcultos.Controls.Add(
                dateTimePicker);

            // Se fuerza nuevamente después de cambiar de Parent.
            dateTimePicker.Visible = false;

            container.BackColor = Color.White;

            RoundedControlHelper.ApplyRoundedRegion(
                container,
                7);

            Label lblFecha =
                new Label
                {
                    AutoSize = false,
                    Location = new Point(12, 7),
                    Size = new Size(
                        habilitado
                            ? container.Width - 62
                            : container.Width - 24,
                        28),
                    ForeColor = habilitado
                        ? AppColors.TextPrimary
                        : AppColors.TextSecondary,
                    Font = AppFonts.Regular(11F),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                    Cursor = habilitado
                        ? Cursors.Hand
                        : Cursors.Default
                };

            Button? btnCalendario = null;

            if (habilitado)
            {
                btnCalendario =
                    CrearBotonCalendario();

                btnCalendario.Location =
                    new Point(
                        container.Width - 44,
                        6);

                btnCalendario.Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Right;

                container.Controls.Add(
                    btnCalendario);

                if (ReferenceEquals(
                        dateTimePicker,
                        dtpFechaCertificacion))
                {
                    _btnSelectorFechaCertificacion =
                        btnCalendario;
                }
            }

            container.Controls.Add(
                lblFecha);

            // Asegura que el botón quede por encima del label.
            btnCalendario?.BringToFront();

            void SincronizarTexto()
            {
                lblFecha.Text =
                    dateTimePicker.Value
                        .ToString("dd/MM/yyyy");
            }

            SincronizarTexto();

            dateTimePicker.ValueChanged +=
                (_, _) =>
                {
                    SincronizarTexto();
                };

            if (habilitado &&
                btnCalendario is not null)
            {
                EventHandler abrirCalendario =
                    (_, _) =>
                    {
                        if (AppDatePickerDialog.TrySelectDate(
                                this,
                                dateTimePicker.Value.Date,
                                out DateTime fechaSeleccionada,
                                fechaMinima:
                                    new DateTime(1753, 1, 1),
                                fechaMaxima:
                                    new DateTime(9998, 12, 31)))
                        {
                            dateTimePicker.Value =
                                fechaSeleccionada.Date;
                        }
                    };

                container.Cursor =
                    Cursors.Hand;

                container.Click +=
                    abrirCalendario;

                lblFecha.Click +=
                    abrirCalendario;

                btnCalendario.Click +=
                    abrirCalendario;

                btnCalendario.Enter +=
                    (_, _) =>
                    {
                        container.Invalidate();
                    };

                btnCalendario.Leave +=
                    (_, _) =>
                    {
                        container.Invalidate();
                    };
            }

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
                        RoundedControlHelper
                            .CreateRoundedPath(
                                rect,
                                7);

                    Color colorBorde =
                        btnCalendario is not null &&
                        btnCalendario.Focused
                            ? AppColors.Primary
                            : AppColors.InputBorder;

                    float grosor =
                        btnCalendario is not null &&
                        btnCalendario.Focused
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

                    e.Graphics.FillRectangle(
                        brush,
                        13,
                        16,
                        2,
                        2);

                    e.Graphics.FillRectangle(
                        brush,
                        17,
                        16,
                        2,
                        2);

                    e.Graphics.FillRectangle(
                        brush,
                        21,
                        16,
                        2,
                        2);
                };

            return button;
        }

        private static void ConfigurarTextBoxMultilinea(
            TextBox textBox,
            Panel container)
        {
            textBox.Location =
                new Point(
                    10,
                    8);

            textBox.Size =
                new Size(
                    container.Width - 20,
                    container.Height - 16);

            textBox.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            textBox.Multiline =
                true;

            textBox.BorderStyle =
                BorderStyle.None;

            textBox.BackColor =
                Color.White;

            textBox.ForeColor =
                AppColors.TextPrimary;

            textBox.Font =
                AppFonts.Light(10.5F);

            textBox.ScrollBars =
                ScrollBars.Vertical;

            InputStyler.ApplyOutlinedInput(
                container,
                textBox);

            container.Controls.Add(
                textBox);
        }

        // =========================================================
        // LOAD
        // =========================================================

        private void CertificacionesTrabajadorVentana_Load(
            object sender,
            EventArgs e)
        {
            cbProcesos.DataSource =
                Clases.Proceso.ConsultarProcesos("");

            cbProcesos.DisplayMember =
                "Nombre";

            cbProcesos.ValueMember =
                "Id";

            cbCertificadores.DataSource =
                Clases.Certificador.ConsultarCertificadores("");

            cbCertificadores.DisplayMember =
                "NombreTrabajador";

            cbCertificadores.ValueMember =
                "Id";

            if (certificacionActual is null)
            {
                cbProcesos.SelectedIndex =
                    -1;

                dtpFechaCertificacion.Value =
                    DateTime.Today;

                cbCertificadores.SelectedIndex =
                    -1;

                txtComentario.Text =
                    string.Empty;

                cbProcesos.Enabled =
                    true;

                cbProcesos.Focus();
            }
            else
            {
                cbProcesos.SelectedValue =
                    certificacionActual.IdProceso;

                dtpFechaCertificacion.Value =
                    certificacionActual.FechaCertificacion;

                cbCertificadores.SelectedValue =
                    certificacionActual.IdCertificador;

                txtComentario.Text =
                    certificacionActual.Comentario ??
                    string.Empty;

                cbProcesos.Enabled =
                    false;

                ActualizarFechaVencimiento();

                cbCertificadores.Focus();
            }
        }

        // =========================================================
        // FECHAS
        // =========================================================

        private void cbProcesos_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ActualizarFechaVencimiento();
        }

        private void dtpFechaCertificacion_ValueChanged(
            object sender,
            EventArgs e)
        {
            ActualizarFechaVencimiento();
        }

        private void ActualizarFechaVencimiento()
        {
            if (cbProcesos.SelectedItem is not DataRowView proceso)
            {
                return;
            }

            if (proceso.Row.Table.Columns.Contains(
                    "VigenciaMeses") == false)
            {
                return;
            }

            object valorVigencia =
                proceso["VigenciaMeses"];

            if (valorVigencia is null ||
                valorVigencia == DBNull.Value)
            {
                return;
            }

            if (!int.TryParse(
                    valorVigencia.ToString(),
                    out int vigenciaMeses))
            {
                return;
            }

            dtpFechaVencimiento.Value =
                dtpFechaCertificacion.Value
                    .AddMonths(
                        vigenciaMeses);
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        private void btnAceptar_Click(
            object sender,
            EventArgs e)
        {
            if (!Validar())
            {
                return;
            }

            if (trabajadorActual is null ||
                trabajadorActual.Id <= 0)
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "No se encontró un trabajador válido.");

                return;
            }

            if (dtpFechaCertificacion.Value <
                new DateTime(1753, 1, 1))
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "La fecha de certificación no es válida.");

                return;
            }

            Certificacion certificacion =
                certificacionActual ??
                new Certificacion();

            certificacion.IdTrabajador =
                trabajadorActual.Id;

            certificacion.IdProceso =
                Convert.ToInt32(
                    cbProcesos.SelectedValue);

            certificacion.FechaCertificacion =
                dtpFechaCertificacion.Value.Date;

            certificacion.FechaVencimiento =
                dtpFechaVencimiento.Value.Date;

            certificacion.IdCertificador =
                Convert.ToInt32(
                    cbCertificadores.SelectedValue);

            certificacion.Comentario =
                string.IsNullOrWhiteSpace(
                    txtComentario.Text)
                    ? null
                    : txtComentario.Text.Trim();

            Mensaje respuesta =
                certificacion.GuardarCertificacion();

            if (respuesta.Id == 1 ||
                respuesta.Id == 2)
            {
                AppDialog.ShowInfo(
                    this,
                    "Operación completada",
                    respuesta.Nombre);

                DialogResult =
                    DialogResult.OK;

                Close();

                return;
            }

            AppDialog.ShowError(
                this,
                "No se pudo guardar",
                respuesta.Nombre);
        }

        // =========================================================
        // VALIDACIONES
        // =========================================================

        private bool Validar()
        {
            if (cbProcesos.SelectedIndex < 0 ||
                cbProcesos.SelectedValue is null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "Selecciona un proceso.");

                cbProcesos.Focus();

                return false;
            }

            if (cbCertificadores.SelectedIndex < 0 ||
                cbCertificadores.SelectedValue is null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "Selecciona un certificador.");

                cbCertificadores.Focus();

                return false;
            }

            if (dtpFechaVencimiento.Value.Date <=
                dtpFechaCertificacion.Value.Date)
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "La fecha de vencimiento no es válida.");

                _btnSelectorFechaCertificacion?.Focus();

                return false;
            }

            return true;
        }

        // =========================================================
        // CANCELAR
        // =========================================================

        private void btnRegresar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
