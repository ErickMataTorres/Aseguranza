using Aseguranza.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Turnos : Form
    {
        // =========================================================
        // ESTADO
        // =========================================================

        private Label? _lblRegistros;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Turnos()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void Turnos_Load(
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

            _estiloAplicado = true;

            SuspendLayout();

            // -----------------------------------------------------
            // FORMULARIO
            // -----------------------------------------------------

            FormStyler.ApplyBase(
                this,
                "Turnos",
                new Size(
                    920,
                    560));

            DoubleBuffered = true;

            // -----------------------------------------------------
            // CABECERA
            // -----------------------------------------------------

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Turnos",
                    "Administra el catálogo de turnos del sistema");

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
                "Buscar turno";

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
                            pnlContenido.ClientSize.Width - 40,
                            39),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
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
                "Escribe el nombre del turno...";

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
                        "0 turnos registrados",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9.5F)
                };

            // -----------------------------------------------------
            // DATAGRIDVIEW
            // -----------------------------------------------------

            DataGridViewStyler.ApplyCatalogStyle(
                dgvTurnos);

            // -----------------------------------------------------
            // CONTENEDOR DE TABLA
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

            dgvTurnos.Dock =
                DockStyle.Fill;

            dgvTurnos.BorderStyle =
                BorderStyle.None;

            pnlTabla.Controls.Add(
                dgvTurnos);

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
            dgvTurnos.DataSource =
                Clases.Turno
                    .ConsultarTurnos(
                        txtBuscar.Text.Trim());

            ConfigurarColumnas();

            ActualizarContador();

            if (dgvTurnos.Rows.Count > 0)
            {
                dgvTurnos.ClearSelection();

                dgvTurnos.Rows[0]
                    .Selected =
                        true;

                DataGridViewCell? primeraCeldaVisible =
                    dgvTurnos.Rows[0]
                        .Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvTurnos.CurrentCell =
                        primeraCeldaVisible;
                }
            }
            else
            {
                dgvTurnos.ClearSelection();

                dgvTurnos.CurrentCell =
                    null;
            }

            ValidarBotones();
        }

        private void ConfigurarColumnas()
        {
            if (dgvTurnos.Columns.Contains(
                "Id"))
            {
                dgvTurnos.Columns["Id"]!
                    .Visible =
                        false;
            }

            if (dgvTurnos.Columns.Contains(
                "Nombre"))
            {
                dgvTurnos.Columns["Nombre"]!
                    .HeaderText =
                        "TURNO";

                dgvTurnos.Columns["Nombre"]!
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
                dgvTurnos.Rows.Count;

            _lblRegistros.Text =
                cantidad switch
                {
                    0 =>
                        "No hay turnos registrados",

                    1 =>
                        "1 turno registrado",

                    _ =>
                        $"{cantidad} turnos registrados"
                };
        }

        private void ValidarBotones()
        {
            bool hayRegistros =
                dgvTurnos.Rows.Count > 0;

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
        // OBTENER TURNO SELECCIONADO
        // =========================================================

        private Clases.Turno?
            ObtenerTurnoSeleccionado()
        {
            if (dgvTurnos.CurrentRow is null)
            {
                return null;
            }

            object? idValor =
                dgvTurnos.CurrentRow
                    .Cells["Id"]
                    .Value;

            if (idValor is null ||
                idValor == DBNull.Value)
            {
                return null;
            }

            string nombre =
                Convert.ToString(
                    dgvTurnos.CurrentRow
                        .Cells["Nombre"]
                        .Value)
                ?? string.Empty;

            return new Clases.Turno
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
            using TurnosVentana ventana =
                new TurnosVentana(
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
            Clases.Turno? turno =
                ObtenerTurnoSeleccionado();

            if (turno is null)
            {
                MessageBox.Show(
                    "Seleccione un turno.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using TurnosVentana ventana =
                new TurnosVentana(
                    turno);

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
            Clases.Turno? turno =
                ObtenerTurnoSeleccionado();

            if (turno is null)
            {
                MessageBox.Show(
                    "Seleccione un turno.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult confirmacion =
                MessageBox.Show(
                    $"¿Está seguro de eliminar el turno \"{turno.Nombre}\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmacion !=
                DialogResult.Yes)
            {
                return;
            }

            Clases.Mensaje respuesta =
                Clases.Turno
                    .BorrarTurno(
                        turno.Id);

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

        private void dgvTurnos_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewCell? primeraCeldaVisible =
                dgvTurnos.Rows[e.RowIndex]
                    .Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            if (primeraCeldaVisible is not null)
            {
                dgvTurnos.CurrentCell =
                    primeraCeldaVisible;
            }

            Clases.Turno? turno =
                ObtenerTurnoSeleccionado();

            if (turno is null)
            {
                return;
            }

            using TurnosVentana ventana =
                new TurnosVentana(
                    turno);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                IniciarTodo();
            }
        }
    }
}