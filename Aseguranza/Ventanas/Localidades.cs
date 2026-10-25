using Aseguranza.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Localidades : Form
    {
        // =========================================================
        // ESTADO
        // =========================================================

        private Label? _lblRegistros;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Localidades()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void Localidades_Load(
            object sender,
            EventArgs e)
        {
            IniciarTodo();
        }

        // =========================================================
        // ESTILO GENERAL
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

            // -----------------------------------------------------
            // FORMULARIO
            // -----------------------------------------------------

            FormStyler.ApplyBase(
                this,
                "Localidades",
                new Size(
                    920,
                    560));

            DoubleBuffered =
                true;

            // -----------------------------------------------------
            // CABECERA
            // -----------------------------------------------------

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Localidades",
                    "Administra el catálogo de localidades del sistema");

            // -----------------------------------------------------
            // TARJETA PRINCIPAL
            // -----------------------------------------------------

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        105),
                    new Size(
                        ClientSize.Width - 40,
                        ClientSize.Height - 125));

            pnlContenido.Name =
                "pnlContenido";

            // -----------------------------------------------------
            // ETIQUETA BUSCAR
            // -----------------------------------------------------

            lblBuscar.Text =
                "Buscar localidad";

            lblBuscar.AutoSize =
                true;

            lblBuscar.Location =
                new Point(
                    20,
                    17);

            lblBuscar.ForeColor =
                AppColors.TextPrimary;

            lblBuscar.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // -----------------------------------------------------
            // CONTENEDOR BUSCADOR
            // -----------------------------------------------------

            Panel pnlBuscar =
                new Panel
                {
                    Name =
                        "pnlBuscar",

                    Location =
                        new Point(
                            20,
                            42),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 200,
                            39),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            Button btnLimpiar =
                new Button();

            ButtonStyler.Apply(
                btnLimpiar,
                "Limpiar",
                AppColors.Neutral,
                icon: null,
                width: 140,
                height: 39);

            btnLimpiar.Name =
                "btnLimpiar";

            btnLimpiar.Location =
                new Point(
                    pnlContenido.ClientSize.Width - 160,
                    42);

            btnLimpiar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnLimpiar.Click +=
                (_, _) =>
                {
                    txtBuscar.Clear();

                    IniciarTodo();

                    txtBuscar.Focus();
                };

            // -----------------------------------------------------
            // ICONO BUSCAR
            // -----------------------------------------------------

            Label lblIconoBuscar =
                new Label
                {
                    Name =
                        "lblIconoBuscar",

                    Text =
                        AppIcons.Search,

                    AutoSize =
                        false,

                    Size =
                        new Size(
                            36,
                            36),

                    Location =
                        new Point(
                            2,
                            1),

                    TextAlign =
                        ContentAlignment.MiddleCenter,

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        new Font(
                            AppIcons.FontFamilyName,
                            14F,
                            FontStyle.Regular)
                };

            // -----------------------------------------------------
            // TEXTBOX BUSCAR
            // -----------------------------------------------------

            txtBuscar.Location =
                new Point(
                    39,
                    8);

            txtBuscar.Size =
                new Size(
                    pnlBuscar.ClientSize.Width - 50,
                    25);

            txtBuscar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            txtBuscar.BorderStyle =
                BorderStyle.None;

            txtBuscar.BackColor =
                Color.White;

            txtBuscar.ForeColor =
                AppColors.TextPrimary;

            txtBuscar.Font =
                AppFonts.Light(11F);

            txtBuscar.PlaceholderText =
                "Escribe el nombre de la localidad...";

            InputStyler.ApplyOutlinedInput(
                pnlBuscar,
                txtBuscar);

            lblIconoBuscar.Click +=
                (_, _) =>
                {
                    txtBuscar.Focus();
                };

            pnlBuscar.Controls.Add(
                lblIconoBuscar);

            pnlBuscar.Controls.Add(
                txtBuscar);

            // -----------------------------------------------------
            // CONTADOR
            // -----------------------------------------------------

            _lblRegistros =
                new Label
                {
                    Name =
                        "lblRegistros",

                    AutoSize =
                        true,

                    Location =
                        new Point(
                            20,
                            92),

                    Text =
                        "Total: 0 localidades",

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            10F,
                            FontStyle.Bold)
                };

            // -----------------------------------------------------
            // DATAGRIDVIEW
            // -----------------------------------------------------

            DataGridViewStyler.ApplyCatalogStyle(
                dgvLocalidades);

            // -----------------------------------------------------
            // CONTENEDOR TABLA
            // -----------------------------------------------------

            Panel pnlTabla =
                new Panel
                {
                    Name =
                        "pnlTabla",

                    Location =
                        new Point(
                            20,
                            118),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            pnlContenido.ClientSize.Height - 196),

                    BackColor =
                        Color.White,

                    Padding =
                        new Padding(1),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlTabla,
                6);

            pnlTabla.Paint +=
                (_, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    Rectangle rectangulo =
                        new Rectangle(
                            0,
                            0,
                            pnlTabla.Width - 1,
                            pnlTabla.Height - 1);

                    using GraphicsPath ruta =
                        RoundedControlHelper
                            .CreateRoundedPath(
                                rectangulo,
                                6);

                    using Pen lapiz =
                        new Pen(
                            AppColors.BorderMedium,
                            1F);

                    e.Graphics.DrawPath(
                        lapiz,
                        ruta);
                };

            dgvLocalidades.Dock =
                DockStyle.Fill;

            dgvLocalidades.BorderStyle =
                BorderStyle.None;

            pnlTabla.Controls.Add(
                dgvLocalidades);

            // -----------------------------------------------------
            // BOTONES
            // -----------------------------------------------------

            ButtonStyler.Apply(
                btnAgregar,
                "Agregar",
                AppColors.Primary,
                AppIcons.Add);

            ButtonStyler.Apply(
                btnModificar,
                "Modificar",
                AppColors.Secondary,
                AppIcons.Edit);

            ButtonStyler.Apply(
                btnBorrar,
                "Eliminar",
                AppColors.Danger,
                AppIcons.Delete);

            ButtonStyler.Apply(
                btnRegresar,
                "Regresar",
                AppColors.Neutral,
                AppIcons.Back);

            int yBotones =
                pnlContenido.ClientSize.Height - 58;

            btnAgregar.Location =
                new Point(
                    20,
                    yBotones);

            btnModificar.Location =
                new Point(
                    170,
                    yBotones);

            btnBorrar.Location =
                new Point(
                    320,
                    yBotones);

            btnRegresar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    btnRegresar.Width -
                    20,
                    yBotones);

            btnAgregar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            btnModificar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            btnBorrar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            btnRegresar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            // -----------------------------------------------------
            // AGREGAR CONTROLES
            // -----------------------------------------------------

            pnlContenido.Controls.Add(
                lblBuscar);

            pnlContenido.Controls.Add(
                pnlBuscar);

            pnlContenido.Controls.Add(
                btnLimpiar);

            pnlContenido.Controls.Add(
                _lblRegistros);

            pnlContenido.Controls.Add(
                pnlTabla);

            pnlContenido.Controls.Add(
                btnAgregar);

            pnlContenido.Controls.Add(
                btnModificar);

            pnlContenido.Controls.Add(
                btnBorrar);

            pnlContenido.Controls.Add(
                btnRegresar);

            pnlContenido.BringToFront();

            pnlCabecera.BringToFront();

            ResumeLayout(false);

            PerformLayout();
        }

        // =========================================================
        // DATOS
        // =========================================================

        private void IniciarTodo()
        {
            dgvLocalidades.DataSource =
                Clases.Localidad
                    .ConsultarLocalidades(
                        txtBuscar.Text.Trim());

            ConfigurarColumnas();

            ActualizarContador();

            if (dgvLocalidades.Rows.Count > 0)
            {
                dgvLocalidades.ClearSelection();

                dgvLocalidades.Rows[0]
                    .Selected =
                        true;

                DataGridViewCell? primeraCeldaVisible =
                    dgvLocalidades.Rows[0]
                        .Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvLocalidades.CurrentCell =
                        primeraCeldaVisible;
                }
            }
            else
            {
                dgvLocalidades.ClearSelection();

                dgvLocalidades.CurrentCell =
                    null;
            }

            ValidarBotones();
        }

        private void ConfigurarColumnas()
        {
            if (dgvLocalidades.Columns.Contains(
                "Id"))
            {
                dgvLocalidades.Columns["Id"]!
                    .Visible =
                        false;
            }

            if (dgvLocalidades.Columns.Contains(
                "Nombre"))
            {
                dgvLocalidades.Columns["Nombre"]!
                    .HeaderText =
                        "LOCALIDAD";

                dgvLocalidades.Columns["Nombre"]!
                    .AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ActualizarContador()
        {
            if (_lblRegistros is null)
            {
                return;
            }

            int cantidad =
                dgvLocalidades.Rows.Count;

            _lblRegistros.Text =
                cantidad switch
                {
                    0 =>
                        "Total: 0 localidades",

                    1 =>
                        "Total: 1 localidad",

                    _ =>
                        $"Total: {cantidad} localidades"
                };
        }

        private void ValidarBotones()
        {
            bool hayRegistros =
                dgvLocalidades.Rows.Count > 0;

            btnModificar.Enabled =
                hayRegistros;

            btnBorrar.Enabled =
                hayRegistros;

            ButtonStyler.UpdateEnabledState(
                btnModificar,
                AppColors.Secondary);

            ButtonStyler.UpdateEnabledState(
                btnBorrar,
                AppColors.Danger);

            txtBuscar.Focus();
        }

        // =========================================================
        // OBTENER SELECCIÓN
        // =========================================================

        private Clases.Localidad?
            ObtenerLocalidadSeleccionada()
        {
            if (dgvLocalidades.CurrentRow is null)
            {
                return null;
            }

            object? idValor =
                dgvLocalidades
                    .CurrentRow
                    .Cells["Id"]
                    .Value;

            if (idValor is null ||
                idValor == DBNull.Value)
            {
                return null;
            }

            string nombre =
                Convert.ToString(
                    dgvLocalidades
                        .CurrentRow
                        .Cells["Nombre"]
                        .Value)
                ?? string.Empty;

            return new Clases.Localidad
            {
                Id =
                    Convert.ToInt32(
                        idValor),

                Nombre =
                    nombre
            };
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
        // AGREGAR
        // =========================================================

        private void btnAgregar_Click(
            object sender,
            EventArgs e)
        {
            using LocalidadesVentana ventana =
                new LocalidadesVentana(
                    null);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                IniciarTodo();
            }
        }

        // =========================================================
        // MODIFICAR
        // =========================================================

        private void btnModificar_Click(
            object sender,
            EventArgs e)
        {
            Clases.Localidad? localidad =
                ObtenerLocalidadSeleccionada();

            if (localidad is null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Aviso",
                    "Seleccione una localidad.");

                return;
            }

            using LocalidadesVentana ventana =
                new LocalidadesVentana(
                    localidad);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                IniciarTodo();
            }
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        private void btnBorrar_Click(
            object sender,
            EventArgs e)
        {
            Clases.Localidad? localidad =
                ObtenerLocalidadSeleccionada();

            if (localidad is null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Aviso",
                    "Seleccione una localidad.");

                return;
            }

            bool confirmacion =
                AppDialog.Confirm(
                    this,
                    "Confirmar eliminación",
                    "¿Está seguro de eliminar la localidad?" +
                    Environment.NewLine +
                    Environment.NewLine +
                    localidad.Nombre,
                    "Eliminar");

            if (!confirmacion)
            {
                return;
            }

            Clases.Mensaje respuesta =
                Clases.Localidad
                    .BorrarLocalidad(
                        localidad.Id);

            if (respuesta.Id == 1)
            {
                AppDialog.ShowInfo(
                    this,
                    "Operación completada",
                    respuesta.Nombre);
            }
            else
            {
                AppDialog.ShowError(
                    this,
                    "No se pudo eliminar",
                    respuesta.Nombre);
            }

            IniciarTodo();
        }

        // =========================================================
        // BUSCADOR
        // =========================================================

        private void txtBuscar_TextChanged(
            object sender,
            EventArgs e)
        {
            // La búsqueda se ejecuta al presionar Enter.
        }

        private void txtBuscar_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (e.KeyChar !=
                (char)Keys.Enter)
            {
                return;
            }

            e.Handled =
                true;

            IniciarTodo();
        }

        // =========================================================
        // DOBLE CLIC
        // =========================================================

        private void dgvLocalidades_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewCell? primeraCeldaVisible =
                dgvLocalidades.Rows[e.RowIndex]
                    .Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            if (primeraCeldaVisible is not null)
            {
                dgvLocalidades.CurrentCell =
                    primeraCeldaVisible;
            }

            Clases.Localidad? localidad =
                ObtenerLocalidadSeleccionada();

            if (localidad is null)
            {
                return;
            }

            using LocalidadesVentana ventana =
                new LocalidadesVentana(
                    localidad);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                IniciarTodo();
            }
        }
    }
}