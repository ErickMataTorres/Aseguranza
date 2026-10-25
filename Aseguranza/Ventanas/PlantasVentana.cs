using Aseguranza.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class PlantasVentana : Form
    {
        private Clases.Planta? plantaActual;

        private bool _estiloAplicado;


        private readonly ErrorProvider _epValidacion =
            new ErrorProvider();

        private Panel _pnlNombre =
            null!;


        public PlantasVentana(
            Clases.Planta? planta)
        {
            InitializeComponent();

            _epValidacion.ContainerControl =
                this;

            _epValidacion.BlinkStyle =
                ErrorBlinkStyle.NeverBlink;

            plantaActual =
                planta;

            if (plantaActual is not null)
            {
                txtNombre.Text =
                    plantaActual.Nombre;
            }

            AplicarEstiloVisual();
        }

        private void PlantasVentana_Load(
            object sender,
            EventArgs e)
        {
            txtNombre.Focus();

            txtNombre.SelectAll();
        }

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
                plantaActual is null
                    ? "Agregar planta"
                    : "Modificar planta";

            string subtitulo =
                plantaActual is null
                    ? "Registra una nueva planta en el sistema"
                    : "Actualiza la información de la planta seleccionada";

            // =====================================================
            // FORMULARIO
            // =====================================================

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
            // LABEL
            // =====================================================

            lblNombre.Text =
                "Nombre de la planta *";

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
            // CONTENEDOR TEXTBOX
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
            // TEXTBOX
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
                "Ej. MCH1";

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
                            100),

                    Text =
                        "El nombre se guardará automáticamente en mayúsculas.",

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
                plantaActual is null
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
                "Capture el nombre de la planta.");

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
                plantaActual?.Id ??
                0;

            if (Clases.Planta.ExisteNombre(
                    nombreNormalizado,
                    idExcluir))
            {
                AppDialog.ShowWarning(
                    this,
                    "Planta duplicada",
                    "Ya existe una planta con el nombre \"" +
                    nombreNormalizado +
                    "\".");

                _epValidacion.SetError(
                    _pnlNombre,
                    "Ya existe una planta con este nombre.");

                txtNombre.Focus();
                txtNombre.SelectAll();

                return;
            }

            Clases.Planta planta =
                plantaActual ??
                new Clases.Planta();

            planta.Nombre =
                nombreNormalizado;

            Clases.Mensaje respuesta =
                planta.GuardarPlanta();

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
        // ENTER
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