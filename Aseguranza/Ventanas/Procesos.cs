using Aseguranza.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Procesos : Form
    {
        // =========================================================
        // ESTADO
        // =========================================================

        private Label? _lblRegistros;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Procesos()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void Procesos_Load(
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

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                "Procesos",
                new Size(
                    920,
                    560));

            DoubleBuffered =
                true;

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Procesos",
                    "Administra los procesos de certificación y su vigencia");

            // =====================================================
            // TARJETA
            // =====================================================

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

            // =====================================================
            // BUSCADOR - LABEL
            // =====================================================

            lblBuscar.Text =
                "Buscar proceso";

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

            // =====================================================
            // BUSCADOR - CONTENEDOR
            // =====================================================

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
                            pnlContenido.ClientSize.Width - 40,
                            39),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            // =====================================================
            // BUSCADOR - ICONO
            // =====================================================

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

            // =====================================================
            // BUSCADOR - TEXTBOX
            // =====================================================

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
                "Escribe el nombre o descripción del proceso...";

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

            // =====================================================
            // CONTADOR
            // =====================================================

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
                        "0 procesos registrados",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9.5F)
                };

            // =====================================================
            // DATAGRIDVIEW
            // =====================================================

            DataGridViewStyler.ApplyCatalogStyle(
                dgvProcesos);

            // =====================================================
            // CONTENEDOR TABLA
            // =====================================================

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

            dgvProcesos.Dock =
                DockStyle.Fill;

            dgvProcesos.BorderStyle =
                BorderStyle.None;

            pnlTabla.Controls.Add(
                dgvProcesos);

            // =====================================================
            // BOTONES
            // =====================================================

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

            // =====================================================
            // AGREGAR CONTROLES
            // =====================================================

            pnlContenido.Controls.Add(
                lblBuscar);

            pnlContenido.Controls.Add(
                pnlBuscar);

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
        // CARGAR DATOS
        // =========================================================

        private void IniciarTodo()
        {
            dgvProcesos.DataSource =
                Clases.Proceso
                    .ConsultarProcesos(
                        txtBuscar.Text.Trim());

            ConfigurarColumnas();

            ActualizarContador();

            if (dgvProcesos.Rows.Count > 0)
            {
                dgvProcesos.ClearSelection();

                dgvProcesos.Rows[0]
                    .Selected =
                        true;

                DataGridViewCell? primeraCeldaVisible =
                    dgvProcesos.Rows[0]
                        .Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvProcesos.CurrentCell =
                        primeraCeldaVisible;
                }
            }
            else
            {
                dgvProcesos.ClearSelection();

                dgvProcesos.CurrentCell =
                    null;
            }

            ValidarBotones();
        }

        // =========================================================
        // COLUMNAS
        // =========================================================

        private void ConfigurarColumnas()
        {
            if (dgvProcesos.Columns.Contains(
                "Id"))
            {
                dgvProcesos.Columns["Id"]!
                    .Visible =
                        false;
            }

            if (dgvProcesos.Columns.Contains(
                "Nombre"))
            {
                dgvProcesos.Columns["Nombre"]!
                    .HeaderText =
                        "PROCESO";

                dgvProcesos.Columns["Nombre"]!
                    .AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;

                dgvProcesos.Columns["Nombre"]!
                    .FillWeight =
                        30;
            }

            if (dgvProcesos.Columns.Contains(
                "Descripcion"))
            {
                dgvProcesos.Columns["Descripcion"]!
                    .HeaderText =
                        "DESCRIPCIÓN";

                dgvProcesos.Columns["Descripcion"]!
                    .AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;

                dgvProcesos.Columns["Descripcion"]!
                    .FillWeight =
                        50;
            }

            if (dgvProcesos.Columns.Contains(
                "VigenciaMeses"))
            {
                dgvProcesos.Columns["VigenciaMeses"]!
                    .HeaderText =
                        "VIGENCIA (MESES)";

                dgvProcesos.Columns["VigenciaMeses"]!
                    .AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.Fill;

                dgvProcesos.Columns["VigenciaMeses"]!
                    .FillWeight =
                        20;

                dgvProcesos.Columns["VigenciaMeses"]!
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                dgvProcesos.Columns["VigenciaMeses"]!
                    .HeaderCell.Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
            }
        }

        // =========================================================
        // CONTADOR
        // =========================================================

        private void ActualizarContador()
        {
            if (_lblRegistros is null)
            {
                return;
            }

            int cantidad =
                dgvProcesos.Rows.Count;

            _lblRegistros.Text =
                cantidad switch
                {
                    0 =>
                        "No hay procesos registrados",

                    1 =>
                        "1 proceso registrado",

                    _ =>
                        $"{cantidad} procesos registrados"
                };
        }

        // =========================================================
        // BOTONES
        // =========================================================

        private void ValidarBotones()
        {
            bool hayRegistros =
                dgvProcesos.Rows.Count > 0;

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
        // OBTENER PROCESO
        // =========================================================

        private Clases.Proceso?
            ObtenerProcesoSeleccionado()
        {
            if (dgvProcesos.CurrentRow is null)
            {
                return null;
            }

            object? idValor =
                dgvProcesos
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
                    dgvProcesos
                        .CurrentRow
                        .Cells["Nombre"]
                        .Value)
                ?? string.Empty;

            string descripcion =
                Convert.ToString(
                    dgvProcesos
                        .CurrentRow
                        .Cells["Descripcion"]
                        .Value)
                ?? string.Empty;

            int vigenciaMeses =
                Convert.ToInt32(
                    dgvProcesos
                        .CurrentRow
                        .Cells["VigenciaMeses"]
                        .Value);

            return new Clases.Proceso
            {
                Id =
                    Convert.ToInt32(
                        idValor),

                Nombre =
                    nombre,

                Descripcion =
                    descripcion,

                VigenciaMeses =
                    vigenciaMeses
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
            using ProcesosVentana ventana =
                new ProcesosVentana(
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
            Clases.Proceso? proceso =
                ObtenerProcesoSeleccionado();

            if (proceso is null)
            {
                MessageBox.Show(
                    "Seleccione un proceso.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using ProcesosVentana ventana =
                new ProcesosVentana(
                    proceso);

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
            Clases.Proceso? proceso =
                ObtenerProcesoSeleccionado();

            if (proceso is null)
            {
                MessageBox.Show(
                    "Seleccione un proceso.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult confirmacion =
                MessageBox.Show(
                    $"¿Está seguro de eliminar el proceso \"{proceso.Nombre}\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmacion !=
                DialogResult.Yes)
            {
                return;
            }

            Clases.Mensaje respuesta =
                Clases.Proceso
                    .BorrarProceso(
                        proceso.Id);

            MessageBox.Show(
                respuesta.Nombre,
                respuesta.Id == 1
                    ? "Operación completada"
                    : "No se pudo eliminar",
                MessageBoxButtons.OK,
                respuesta.Id == 1
                    ? MessageBoxIcon.Information
                    : MessageBoxIcon.Error);

            IniciarTodo();
        }

        // =========================================================
        // BUSCADOR
        // =========================================================

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

        private void dgvProcesos_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewCell? primeraCeldaVisible =
                dgvProcesos.Rows[e.RowIndex]
                    .Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            if (primeraCeldaVisible is not null)
            {
                dgvProcesos.CurrentCell =
                    primeraCeldaVisible;
            }

            Clases.Proceso? proceso =
                ObtenerProcesoSeleccionado();

            if (proceso is null)
            {
                return;
            }

            using ProcesosVentana ventana =
                new ProcesosVentana(
                    proceso);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                IniciarTodo();
            }
        }
    }
}