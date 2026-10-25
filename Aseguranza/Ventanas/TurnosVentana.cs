using Aseguranza.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class TurnosVentana : Form
    {
        // =========================================================
        // DATOS
        // =========================================================

        private Clases.Turno? turnoActual;

        private bool _estiloAplicado;


        private readonly ErrorProvider _epValidacion =
            new ErrorProvider();

        private Panel _pnlNombre =
            null!;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public TurnosVentana(
            Clases.Turno? turno)
        {
            InitializeComponent();

            _epValidacion.ContainerControl =
                this;

            _epValidacion.BlinkStyle =
                ErrorBlinkStyle.NeverBlink;

            turnoActual =
                turno;

            if (turnoActual is not null)
            {
                txtNombre.Text =
                    turnoActual.Nombre;
            }

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void TurnosVentana_Load(
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
                turnoActual is null
                    ? "Agregar turno"
                    : "Modificar turno";

            string subtitulo =
                turnoActual is null
                    ? "Registra un nuevo turno en el sistema"
                    : "Actualiza la información del turno seleccionado";

            // -----------------------------------------------------
            // FORMULARIO
            // -----------------------------------------------------

            FormStyler.ApplyBase(
                this,
                titulo,
                new Size(
                    620,
                    340));

            DoubleBuffered =
                true;

            KeyPreview =
                true;

            AcceptButton =
                btnAceptar;

            CancelButton =
                btnRegresar;

            // -----------------------------------------------------
            // CABECERA
            // -----------------------------------------------------

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    titulo,
                    subtitulo,
                    titleX: 30,
                    titleY: 15,
                    subtitleX: 32,
                    subtitleY: 52);

            // -----------------------------------------------------
            // TARJETA
            // -----------------------------------------------------

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

            // -----------------------------------------------------
            // LABEL NOMBRE
            // -----------------------------------------------------

            lblNombre.Text =
                "Nombre del turno *";

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

            // -----------------------------------------------------
            // CONTENEDOR TEXTBOX
            // -----------------------------------------------------

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

            // -----------------------------------------------------
            // TEXTBOX
            // -----------------------------------------------------

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
                "Ej. Primer turno";

            InputStyler.ApplyOutlinedInput(
                _pnlNombre,
                txtNombre);


            txtNombre.TextChanged +=
                (_, _) =>
                {
                    if (!string.IsNullOrWhiteSpace(
                            txtNombre.Text))
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

            // -----------------------------------------------------
            // AYUDA
            // -----------------------------------------------------

            Label lblAyuda =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            22,
                            100),

                    Text =
                        "El nombre se guardará automáticamente en mayúsculas.",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9F)
                };

            // -----------------------------------------------------
            // BOTONES
            // -----------------------------------------------------

            ButtonStyler.Apply(
                btnAceptar,
                turnoActual is null
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

            // -----------------------------------------------------
            // AGREGAR CONTROLES
            // -----------------------------------------------------

            pnlContenido.Controls.Add(
                lblNombre);

            pnlContenido.Controls.Add(
                _pnlNombre);

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
        }

        private bool ValidarInformacion()
        {
            LimpiarErroresValidacion();

            if (!string.IsNullOrWhiteSpace(
                    txtNombre.Text))
            {
                return true;
            }

            _epValidacion.SetError(
                _pnlNombre,
                "Capture el nombre del turno.");

            txtNombre.Focus();

            AppDialog.ShowWarning(
                this,
                "Información incompleta",
                "Hay información incompleta. Revise el campo marcado.");

            return false;
        }

        // =========================================================
        // GUARDAR / ACTUALIZAR
        // =========================================================

        private void btnAceptar_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidarInformacion())
            {
                return;
            }

            string nombre =
                txtNombre.Text.Trim();

            string nombreNormalizado =
                nombre.ToUpperInvariant();

            int idExcluir =
                turnoActual?.Id ??
                0;

            if (Clases.Turno.ExisteNombre(
                    nombreNormalizado,
                    idExcluir))
            {
                AppDialog.ShowWarning(
                    this,
                    "Turno duplicado",
                    "Ya existe un turno con el nombre \"" +
                    nombreNormalizado +
                    "\".");

                _epValidacion.SetError(
                    _pnlNombre,
                    "Ya existe un turno con este nombre.");

                txtNombre.Focus();
                txtNombre.SelectAll();

                return;
            }

            Clases.Turno turno =
                turnoActual ??
                new Clases.Turno();

            turno.Nombre =
                nombreNormalizado;

            Clases.Mensaje respuesta =
                turno.GuardarTurno();

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
        // ENTER EN NOMBRE
        // =========================================================

        private void txtNombre_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar ==
                (char)Keys.Enter)
            {
                e.Handled =
                    true;

                btnAceptar.PerformClick();
            }
        }
    }
}