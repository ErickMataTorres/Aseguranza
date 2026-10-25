using Aseguranza.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class ProcesosVentana : Form
    {
        // =========================================================
        // DATOS
        // =========================================================

        private Clases.Proceso? procesoActual;

        private bool _estiloAplicado;


        private readonly ErrorProvider _epValidacion =
            new ErrorProvider();

        private Panel _pnlNombre =
            null!;

        private Panel _pnlDescripcion =
            null!;

        private Panel _pnlVigencia =
            null!;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ProcesosVentana(
            Clases.Proceso? proceso)
        {
            InitializeComponent();

            _epValidacion.ContainerControl =
                this;

            _epValidacion.BlinkStyle =
                ErrorBlinkStyle.NeverBlink;

            procesoActual =
                proceso;

            if (procesoActual is not null)
            {
                txtNombre.Text =
                    procesoActual.Nombre;

                txtDescripcion.Text =
                    procesoActual.Descripcion;

                txtVigencia.Text =
                    procesoActual
                        .VigenciaMeses
                        .ToString();
            }

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void ProcesosVentana_Load(
            object sender,
            EventArgs e)
        {
            txtNombre.Focus();

            txtNombre.SelectAll();
        }

        // =========================================================
        // ESTILO
        // =========================================================

        private void AplicarEstiloVisual()
        {
            if (_estiloAplicado)
            {
                return;
            }

            _estiloAplicado =
                true;

            SuspendLayout();

            string titulo =
                procesoActual is null
                    ? "Agregar proceso"
                    : "Modificar proceso";

            string subtitulo =
                procesoActual is null
                    ? "Registra un nuevo proceso de certificación"
                    : "Actualiza la información del proceso seleccionado";

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                titulo,
                new Size(
                    620,
                    520));

            DoubleBuffered =
                true;

            KeyPreview =
                true;

            AcceptButton =
                btnAceptar;

            CancelButton =
                btnRegresar;

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    titulo,
                    subtitulo,
                    titleX: 30,
                    titleY: 15,
                    subtitleX: 32,
                    subtitleY: 52);

            // =====================================================
            // TARJETA
            // =====================================================

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        106),
                    new Size(
                        ClientSize.Width - 40,
                        ClientSize.Height - 126));

            pnlContenido.Name =
                "pnlContenido";

            // =====================================================
            // NOMBRE - LABEL
            // =====================================================

            lblNombre.Text =
                "Nombre del proceso *";

            lblNombre.AutoSize =
                true;

            lblNombre.Location =
                new Point(
                    22,
                    22);

            lblNombre.ForeColor =
                AppColors.TextPrimary;

            lblNombre.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // NOMBRE - CONTENEDOR
            // =====================================================

            _pnlNombre =
                new Panel
                {
                    Name =
                        "pnlNombre",

                    Location =
                        new Point(
                            22,
                            50),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 44,
                            42),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            // =====================================================
            // NOMBRE - TEXTBOX
            // =====================================================

            txtNombre.Location =
                new Point(
                    12,
                    10);

            txtNombre.Size =
                new Size(
                    _pnlNombre.ClientSize.Width - 24,
                    25);

            txtNombre.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            txtNombre.BorderStyle =
                BorderStyle.None;

            txtNombre.BackColor =
                Color.White;

            txtNombre.ForeColor =
                AppColors.TextPrimary;

            txtNombre.Font =
                AppFonts.Light(11F);

            txtNombre.PlaceholderText =
                "Ej. INSPECCIÓN";

            InputStyler.ApplyOutlinedInput(
                _pnlNombre,
                txtNombre);


            txtNombre.TextChanged +=
                (_, _) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        _epValidacion.SetError(
                            _pnlNombre,
                            string.Empty);
                    }
                };

            _pnlNombre.Controls.Add(
                txtNombre);

            _epValidacion.SetIconAlignment(
                _pnlNombre,
                ErrorIconAlignment.MiddleRight);

            _epValidacion.SetIconPadding(
                _pnlNombre,
                0);

            // =====================================================
            // DESCRIPCIÓN - LABEL
            // =====================================================

            lblDescripcion.Text =
                "Descripción *";

            lblDescripcion.AutoSize =
                true;

            lblDescripcion.Location =
                new Point(
                    22,
                    111);

            lblDescripcion.ForeColor =
                AppColors.TextPrimary;

            lblDescripcion.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // DESCRIPCIÓN - CONTENEDOR
            // =====================================================

            _pnlDescripcion =
                new Panel
                {
                    Name =
                        "pnlDescripcion",

                    Location =
                        new Point(
                            22,
                            139),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 44,
                            42),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            // =====================================================
            // DESCRIPCIÓN - TEXTBOX
            // =====================================================

            txtDescripcion.Location =
                new Point(
                    12,
                    10);

            txtDescripcion.Size =
                new Size(
                    _pnlDescripcion.ClientSize.Width - 24,
                    25);

            txtDescripcion.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            txtDescripcion.BorderStyle =
                BorderStyle.None;

            txtDescripcion.BackColor =
                Color.White;

            txtDescripcion.ForeColor =
                AppColors.TextPrimary;

            txtDescripcion.Font =
                AppFonts.Light(11F);

            txtDescripcion.PlaceholderText =
                "Describe brevemente el proceso...";

            InputStyler.ApplyOutlinedInput(
                _pnlDescripcion,
                txtDescripcion);


            txtDescripcion.TextChanged +=
                (_, _) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtDescripcion.Text))
                    {
                        _epValidacion.SetError(
                            _pnlDescripcion,
                            string.Empty);
                    }
                };

            _pnlDescripcion.Controls.Add(
                txtDescripcion);

            _epValidacion.SetIconAlignment(
                _pnlDescripcion,
                ErrorIconAlignment.MiddleRight);

            _epValidacion.SetIconPadding(
                _pnlDescripcion,
                0);

            // =====================================================
            // VIGENCIA - LABEL
            // =====================================================

            lblVigencia.Text =
                "Vigencia (meses) *";

            lblVigencia.AutoSize =
                true;

            lblVigencia.Location =
                new Point(
                    22,
                    200);

            lblVigencia.ForeColor =
                AppColors.TextPrimary;

            lblVigencia.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // VIGENCIA - CONTENEDOR
            // =====================================================

            _pnlVigencia =
                new Panel
                {
                    Name =
                        "pnlVigencia",

                    Location =
                        new Point(
                            22,
                            228),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 44,
                            42),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            // =====================================================
            // VIGENCIA - TEXTBOX
            // =====================================================

            txtVigencia.Location =
                new Point(
                    12,
                    10);

            txtVigencia.Size =
                new Size(
                    _pnlVigencia.ClientSize.Width - 24,
                    25);

            txtVigencia.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            txtVigencia.BorderStyle =
                BorderStyle.None;

            txtVigencia.BackColor =
                Color.White;

            txtVigencia.ForeColor =
                AppColors.TextPrimary;

            txtVigencia.Font =
                AppFonts.Light(11F);

            txtVigencia.PlaceholderText =
                "Ej. 12";

            InputStyler.ApplyOutlinedInput(
                _pnlVigencia,
                txtVigencia);


            txtVigencia.TextChanged +=
                (_, _) =>
                {
                    if (!string.IsNullOrWhiteSpace(txtVigencia.Text))
                    {
                        _epValidacion.SetError(
                            _pnlVigencia,
                            string.Empty);
                    }
                };

            _pnlVigencia.Controls.Add(
                txtVigencia);

            _epValidacion.SetIconAlignment(
                _pnlVigencia,
                ErrorIconAlignment.MiddleRight);

            _epValidacion.SetIconPadding(
                _pnlVigencia,
                0);

            // =====================================================
            // AYUDA
            // =====================================================

            Label lblAyuda =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            22,
                            280),

                    Text =
                        "La vigencia determina durante cuántos meses será válida la certificación.",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9F)
                };

            // =====================================================
            // BOTONES
            // =====================================================

            ButtonStyler.Apply(
                btnAceptar,
                procesoActual is null
                    ? "Guardar"
                    : "Actualizar",
                AppColors.Primary,
                AppIcons.Save);

            ButtonStyler.Apply(
                btnRegresar,
                "Cancelar",
                AppColors.Neutral,
                AppIcons.Back);

            int yBotones =
                pnlContenido.ClientSize.Height - 60;

            btnAceptar.Location =
                new Point(
                    22,
                    yBotones);

            btnRegresar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    btnRegresar.Width -
                    22,
                    yBotones);

            btnAceptar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            btnRegresar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            // =====================================================
            // AGREGAR CONTROLES
            // =====================================================

            pnlContenido.Controls.Add(
                lblNombre);

            pnlContenido.Controls.Add(
                _pnlNombre);

            pnlContenido.Controls.Add(
                lblDescripcion);

            pnlContenido.Controls.Add(
                _pnlDescripcion);

            pnlContenido.Controls.Add(
                lblVigencia);

            pnlContenido.Controls.Add(
                _pnlVigencia);

            pnlContenido.Controls.Add(
                lblAyuda);

            pnlContenido.Controls.Add(
                btnAceptar);

            pnlContenido.Controls.Add(
                btnRegresar);

            pnlContenido.BringToFront();

            pnlCabecera.BringToFront();

            ResumeLayout(false);

            PerformLayout();
        }

        // =========================================================
        // VALIDACIÓN
        // =========================================================

        private void LimpiarErroresValidacion()
        {
            _epValidacion.SetError(
                _pnlNombre,
                string.Empty);

            _epValidacion.SetError(
                _pnlDescripcion,
                string.Empty);

            _epValidacion.SetError(
                _pnlVigencia,
                string.Empty);
        }

        private bool ValidarInformacion(
            out int vigenciaMeses)
        {
            LimpiarErroresValidacion();

            vigenciaMeses =
                0;

            bool esValido =
                true;

            Control? primerControlInvalido =
                null;

            if (string.IsNullOrWhiteSpace(
                    txtNombre.Text))
            {
                _epValidacion.SetError(
                    _pnlNombre,
                    "Capture el nombre del proceso.");

                primerControlInvalido =
                    txtNombre;

                esValido =
                    false;
            }

            if (string.IsNullOrWhiteSpace(
                    txtDescripcion.Text))
            {
                _epValidacion.SetError(
                    _pnlDescripcion,
                    "Capture la descripción del proceso.");

                primerControlInvalido ??=
                    txtDescripcion;

                esValido =
                    false;
            }

            string vigenciaTexto =
                txtVigencia.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    vigenciaTexto))
            {
                _epValidacion.SetError(
                    _pnlVigencia,
                    "Capture la vigencia en meses.");

                primerControlInvalido ??=
                    txtVigencia;

                esValido =
                    false;
            }
            else if (!int.TryParse(
                         vigenciaTexto,
                         out vigenciaMeses) ||
                     vigenciaMeses <= 0)
            {
                _epValidacion.SetError(
                    _pnlVigencia,
                    "La vigencia debe ser un número entero mayor que cero.");

                primerControlInvalido ??=
                    txtVigencia;

                esValido =
                    false;
            }

            if (!esValido)
            {
                primerControlInvalido?.Focus();

                if (ReferenceEquals(
                        primerControlInvalido,
                        txtVigencia))
                {
                    txtVigencia.SelectAll();
                }

                AppDialog.ShowWarning(
                    this,
                    "Información incompleta",
                    "Revise los campos marcados antes de guardar.");
            }

            return esValido;
        }

        // =========================================================
        // GUARDAR / ACTUALIZAR
        // =========================================================

        private void btnAceptar_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidarInformacion(
                    out int vigenciaMeses))
            {
                return;
            }

            string nombre =
                txtNombre.Text.Trim();

            string descripcion =
                txtDescripcion.Text.Trim();

            string nombreNormalizado =
                nombre.ToUpperInvariant();

            int idExcluir =
                procesoActual?.Id ??
                0;

            if (Clases.Proceso.ExisteNombre(
                    nombreNormalizado,
                    idExcluir))
            {
                AppDialog.ShowWarning(
                    this,
                    "Proceso duplicado",
                    "Ya existe un proceso con el nombre \"" +
                    nombreNormalizado +
                    "\".");

                _epValidacion.SetError(
                    _pnlNombre,
                    "Ya existe un proceso con este nombre.");

                txtNombre.Focus();
                txtNombre.SelectAll();

                return;
            }

            Clases.Proceso proceso =
                procesoActual ??
                new Clases.Proceso();

            proceso.Nombre =
                nombreNormalizado;

            proceso.Descripcion =
                descripcion.ToUpperInvariant();

            proceso.VigenciaMeses =
                vigenciaMeses;

            Clases.Mensaje respuesta =
                proceso.GuardarProceso();

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
        // CANCELAR
        // =========================================================

        private void btnRegresar_Click(
            object sender,
            EventArgs e)
        {
            DialogResult =
                DialogResult.Cancel;

            Close();
        }

        // =========================================================
        // TECLADO
        // =========================================================

        protected override bool ProcessCmdKey(
            ref Message msg,
            Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult =
                    DialogResult.Cancel;

                Close();

                return true;
            }

            return base.ProcessCmdKey(
                ref msg,
                keyData);
        }

        // =========================================================
        // SOLO NÚMEROS EN VIGENCIA
        // =========================================================

        private void textBox2_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar ==
                (char)Keys.Enter)
            {
                e.Handled =
                    true;

                btnAceptar.PerformClick();

                return;
            }

            if (!char.IsDigit(
                    e.KeyChar) &&
                !char.IsControl(
                    e.KeyChar))
            {
                e.Handled =
                    true;
            }
        }
    }
}