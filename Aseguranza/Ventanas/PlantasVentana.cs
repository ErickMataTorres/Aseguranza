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

        public PlantasVentana(
            Clases.Planta? planta)
        {
            InitializeComponent();

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
                "Nombre de la planta";

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

            // =====================================================
            // TEXTBOX
            // =====================================================

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
                "Ej. MCH1";

            InputStyler.ApplyOutlinedInput(
                pnlNombre,
                txtNombre);

            pnlNombre.Controls.Add(
                txtNombre);

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
        // GUARDAR / ACTUALIZAR
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
                MessageBox.Show(
                    "El nombre de la planta no puede estar vacío.",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNombre.Focus();

                return;
            }

            Clases.Planta planta =
                plantaActual ??
                new Clases.Planta();

            planta.Nombre =
                nombre.ToUpper();

            Clases.Mensaje respuesta =
                planta.GuardarPlanta();

            if (respuesta.Id == 1 ||
                respuesta.Id == 2)
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Operación completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult =
                    DialogResult.OK;

                Close();

                return;
            }

            MessageBox.Show(
                respuesta.Nombre,
                "No se pudo guardar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
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