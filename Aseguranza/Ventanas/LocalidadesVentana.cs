using Aseguranza.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class LocalidadesVentana : Form
    {
        // =========================================================
        // DATOS
        // =========================================================

        private Clases.Localidad? localidadActual;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public LocalidadesVentana(
            Clases.Localidad? localidad)
        {
            InitializeComponent();

            localidadActual =
                localidad;

            if (localidadActual is not null)
            {
                txtNombre.Text =
                    localidadActual.Nombre;
            }

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void LocalidadesVentana_Load(
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
                localidadActual is null
                    ? "Agregar localidad"
                    : "Modificar localidad";

            string subtitulo =
                localidadActual is null
                    ? "Registra una nueva localidad en el sistema"
                    : "Actualiza la información de la localidad seleccionada";

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
                "Nombre de la localidad";

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

            Panel pnlNombre =
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
                    pnlNombre.ClientSize.Width - 24,
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
                "Ej. Los Mochis";

            InputStyler.ApplyOutlinedInput(
                pnlNombre,
                txtNombre);

            pnlNombre.Controls.Add(
                txtNombre);

            // -----------------------------------------------------
            // TEXTO AYUDA
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
                localidadActual is null
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
                pnlNombre);

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
        // GUARDAR
        // =========================================================

        private void btnAceptar_Click(
            object sender,
            EventArgs e)
        {
            string nombre =
                txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                nombre))
            {
                AppDialog.ShowWarning(
                    this,
                    "Dato requerido",
                    "El nombre de la localidad no puede estar vacío.");

                txtNombre.Focus();

                return;
            }

            string nombreNormalizado =
                nombre.ToUpperInvariant();

            int idExcluir =
                localidadActual?.Id ??
                0;

            if (Clases.Localidad.ExisteNombre(
                    nombreNormalizado,
                    idExcluir))
            {
                AppDialog.ShowWarning(
                    this,
                    "Localidad duplicada",
                    "Ya existe una localidad con el nombre \"" +
                    nombreNormalizado +
                    "\".");

                txtNombre.Focus();
                txtNombre.SelectAll();

                return;
            }

            Clases.Localidad localidad =
                localidadActual ??
                new Clases.Localidad();

            localidad.Nombre =
                nombreNormalizado;

            Clases.Mensaje respuesta =
                localidad.GuardarLocalidad();

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
        // EVENTOS DEL DESIGNER
        // =========================================================

        private void btnAceptar_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
        }

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

        private void txtNombre_TextChanged(
            object sender,
            EventArgs e)
        {
        }

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

    }
}