using Aseguranza.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class LineasVentana : Form
    {
        // =========================================================
        // DATOS
        // =========================================================

        private Clases.Linea? lineaActual;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public LineasVentana(
            Clases.Linea? linea)
        {
            InitializeComponent();

            lineaActual =
                linea;

            if (lineaActual is not null)
            {
                txtNombre.Text =
                    lineaActual.Nombre;
            }

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void LineasVentana_Load(
            object sender,
            EventArgs e)
        {
            CargarPlantas();

            if (lineaActual is null)
            {
                cbPlantas.SelectedIndex =
                    -1;

                cbPlantas.Focus();
            }
            else
            {
                cbPlantas.SelectedValue =
                    lineaActual.IdPlanta;

                txtNombre.Focus();

                txtNombre.SelectAll();
            }
        }

        // =========================================================
        // CARGAR PLANTAS
        // =========================================================

        private void CargarPlantas()
        {
            cbPlantas.DataSource =
                Clases.Planta
                    .ConsultarPlantas(
                        string.Empty);

            cbPlantas.DisplayMember =
                "Nombre";

            cbPlantas.ValueMember =
                "Id";
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
                lineaActual is null
                    ? "Agregar línea"
                    : "Modificar línea";

            string subtitulo =
                lineaActual is null
                    ? "Registra una nueva línea y asígnala a una planta"
                    : "Actualiza la línea y su planta asignada";

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                titulo,
                new Size(
                    620,
                    420));

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
            // LABEL PLANTA
            // =====================================================

            lblPlanta.Text =
                "Planta";

            lblPlanta.AutoSize =
                true;

            lblPlanta.Location =
                new Point(
                    22,
                    22);

            lblPlanta.ForeColor =
                AppColors.TextPrimary;

            lblPlanta.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // CONTENEDOR COMBOBOX
            // =====================================================

            Panel pnlPlanta =
                new Panel
                {
                    Name =
                        "pnlPlanta",

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
            // COMBOBOX PLANTAS
            // =====================================================

            cbPlantas.Location =
                new Point(
                    8,
                    7);

            cbPlantas.Size =
                new Size(
                    pnlPlanta.ClientSize.Width - 16,
                    28);

            cbPlantas.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            ComboBoxStyler.ApplyOutlinedComboBox(
                pnlPlanta,
                cbPlantas);

            pnlPlanta.Controls.Add(
                cbPlantas);

            // =====================================================
            // LABEL NOMBRE
            // =====================================================

            lblNombre.Text =
                "Nombre de la línea";

            lblNombre.AutoSize =
                true;

            lblNombre.Location =
                new Point(
                    22,
                    111);

            lblNombre.ForeColor =
                AppColors.TextPrimary;

            lblNombre.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // CONTENEDOR NOMBRE
            // =====================================================

            Panel pnlNombre =
                new Panel
                {
                    Name =
                        "pnlNombre",

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
            // TEXTBOX NOMBRE
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
                "Ej. ENSAMBLE 1";

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
                            191),

                    Text =
                        "Selecciona la planta a la que pertenece la línea.",

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
                lineaActual is null
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
                lblPlanta);

            pnlContenido.Controls.Add(
                pnlPlanta);

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

            if (cbPlantas.SelectedIndex < 0)
            {
                AppDialog.ShowWarning(
                    this,
                    "Dato requerido",
                    "Seleccione una planta.");

                cbPlantas.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                nombre))
            {
                AppDialog.ShowWarning(
                    this,
                    "Dato requerido",
                    "El nombre de la línea no puede estar vacío.");

                txtNombre.Focus();

                return;
            }

            if (cbPlantas.SelectedValue is null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Dato requerido",
                    "No fue posible obtener la planta seleccionada.");

                cbPlantas.Focus();

                return;
            }

            int idPlanta =
                Convert.ToInt32(
                    cbPlantas.SelectedValue);

            string nombreNormalizado =
                nombre.ToUpperInvariant();

            int idExcluir =
                lineaActual?.Id ??
                0;

            if (Clases.Linea.ExisteNombreEnPlanta(
                    idPlanta,
                    nombreNormalizado,
                    idExcluir))
            {
                string nombrePlanta =
                    cbPlantas.Text.Trim();

                AppDialog.ShowWarning(
                    this,
                    "Línea duplicada",
                    "Ya existe la línea \"" +
                    nombreNormalizado +
                    "\" en la planta \"" +
                    nombrePlanta +
                    "\".");

                txtNombre.Focus();
                txtNombre.SelectAll();

                return;
            }

            Clases.Linea linea =
                lineaActual ??
                new Clases.Linea();

            linea.IdPlanta =
                idPlanta;

            linea.Nombre =
                nombreNormalizado;

            Clases.Mensaje respuesta =
                linea.GuardarLinea();

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