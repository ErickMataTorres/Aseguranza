using Aseguranza.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Trabajadores : Form
    {
        // =========================================================
        // ESTADO VISUAL
        // =========================================================

        private Label? _lblRegistros;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Trabajadores()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void Trabajadores_Load(
            object sender,
            EventArgs e)
        {
            CargarTrabajadores();
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
                "Trabajadores",
                new Size(
                    1100,
                    700));

            DoubleBuffered =
                true;

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Trabajadores",
                    "Administra el personal registrado en el sistema");

            // =====================================================
            // TARJETA PRINCIPAL
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
            // LABEL BUSCAR
            // =====================================================

            lblBuscar.Text =
                "Buscar trabajador";

            lblBuscar.AutoSize =
                true;

            lblBuscar.Location =
                new Point(
                    20,
                    20);

            lblBuscar.ForeColor =
                AppColors.TextPrimary;

            lblBuscar.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // CONTENEDOR BUSCADOR
            // =====================================================

            Panel pnlBuscar =
                new Panel
                {
                    Name =
                        "pnlBuscar",

                    Location =
                        new Point(
                            20,
                            47),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            42),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            // =====================================================
            // ICONO BUSCAR
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

                    Location =
                        new Point(
                            3,
                            2),

                    Size =
                        new Size(
                            36,
                            37),

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
            // TEXTBOX BUSCAR
            // =====================================================

            txtBuscar.Location =
                new Point(
                    40,
                    9);

            txtBuscar.Size =
                new Size(
                    pnlBuscar.ClientSize.Width - 52,
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
                "Escribe nombre, número de reloj, localidad, turno, planta o línea...";

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
                            101),

                    Text =
                        "0 trabajadores registrados",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9.5F)
                };

            // =====================================================
            // DATAGRIDVIEW
            // =====================================================

            DataGridViewStyler.ApplyCatalogStyle(
                dgvTrabajadores);

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
                            127),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            pnlContenido.ClientSize.Height - 205),

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

            dgvTrabajadores.Dock =
                DockStyle.Fill;

            dgvTrabajadores.BorderStyle =
                BorderStyle.None;

            pnlTabla.Controls.Add(
                dgvTrabajadores);

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
        // CARGAR TRABAJADORES
        // =========================================================

        private void CargarTrabajadores()
        {
            dgvTrabajadores.DataSource =
                Clases.Trabajador
                    .ConsultarTrabajadores(
                        txtBuscar.Text.Trim());

            ConfigurarColumnas();

            ActualizarContador();

            if (dgvTrabajadores.Rows.Count > 0)
            {
                dgvTrabajadores.ClearSelection();

                dgvTrabajadores.Rows[0]
                    .Selected =
                        true;

                DataGridViewCell? primeraCeldaVisible =
                    dgvTrabajadores.Rows[0]
                        .Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvTrabajadores.CurrentCell =
                        primeraCeldaVisible;
                }
            }
            else
            {
                dgvTrabajadores.ClearSelection();

                dgvTrabajadores.CurrentCell =
                    null;
            }

            ValidarBotones();
        }

        // =========================================================
        // COLUMNAS
        // =========================================================

        private void ConfigurarColumnas()
        {
            OcultarColumna(
                "Id");

            OcultarColumna(
                "RutaFoto");

            OcultarColumna(
                "IdLocalidad");

            OcultarColumna(
                "IdTurno");

            OcultarColumna(
                "IdPlanta");

            OcultarColumna(
                "IdLinea");

            ConfigurarColumna(
                "NoReloj",
                "NO. RELOJ",
                14);

            ConfigurarColumna(
                "Nombre",
                "NOMBRE",
                34);

            ConfigurarColumna(
                "NombreLocalidad",
                "LOCALIDAD",
                13);

            ConfigurarColumna(
                "NombreTurno",
                "TURNO",
                10);

            ConfigurarColumna(
                "NombrePlanta",
                "PLANTA",
                12);

            ConfigurarColumna(
                "NombreLinea",
                "LÍNEA",
                17);
        }

        private void OcultarColumna(
            string nombre)
        {
            if (!dgvTrabajadores.Columns.Contains(
                nombre))
            {
                return;
            }

            dgvTrabajadores.Columns[nombre]!
                .Visible =
                    false;
        }

        private void ConfigurarColumna(
            string nombre,
            string encabezado,
            float peso)
        {
            if (!dgvTrabajadores.Columns.Contains(
                nombre))
            {
                return;
            }

            DataGridViewColumn columna =
                dgvTrabajadores.Columns[nombre]!;

            columna.Visible =
                true;

            columna.HeaderText =
                encabezado;

            columna.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            columna.FillWeight =
                peso;
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
                dgvTrabajadores.Rows.Count;

            _lblRegistros.Text =
                cantidad switch
                {
                    0 =>
                        "No hay trabajadores registrados",

                    1 =>
                        "1 trabajador registrado",

                    _ =>
                        $"{cantidad} trabajadores registrados"
                };
        }

        // =========================================================
        // VALIDAR BOTONES
        // =========================================================

        private void ValidarBotones()
        {
            bool hayRegistros =
                dgvTrabajadores.Rows.Count > 0;

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
        // OBTENER TRABAJADOR SELECCIONADO
        // =========================================================

        private Clases.Trabajador?
            ObtenerTrabajadorSeleccionado()
        {
            if (dgvTrabajadores.CurrentRow is null)
            {
                return null;
            }

            object? idValor =
                ObtenerValorCelda(
                    "Id");

            if (idValor is null ||
                idValor == DBNull.Value)
            {
                return null;
            }

            return new Clases.Trabajador
            {
                Id =
                    Convert.ToInt32(
                        idValor),

                NoReloj =
                    ObtenerTextoCelda(
                        "NoReloj"),

                Nombre =
                    ObtenerTextoCelda(
                        "Nombre"),

                RutaFoto =
                    ObtenerTextoCelda(
                        "RutaFoto"),

                IdLocalidad =
                    ObtenerEnteroCelda(
                        "IdLocalidad"),

                IdTurno =
                    ObtenerEnteroCelda(
                        "IdTurno"),

                IdPlanta =
                    ObtenerEnteroCelda(
                        "IdPlanta"),

                IdLinea =
                    ObtenerEnteroCelda(
                        "IdLinea")
            };
        }

        private object? ObtenerValorCelda(
            string nombreColumna)
        {
            if (dgvTrabajadores.CurrentRow is null ||
                !dgvTrabajadores.Columns.Contains(
                    nombreColumna))
            {
                return null;
            }

            return dgvTrabajadores
                .CurrentRow
                .Cells[nombreColumna]
                .Value;
        }

        private string ObtenerTextoCelda(
            string nombreColumna)
        {
            object? valor =
                ObtenerValorCelda(
                    nombreColumna);

            if (valor is null ||
                valor == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(
                valor)
                ?? string.Empty;
        }

        private int ObtenerEnteroCelda(
            string nombreColumna)
        {
            object? valor =
                ObtenerValorCelda(
                    nombreColumna);

            if (valor is null ||
                valor == DBNull.Value)
            {
                return 0;
            }

            try
            {
                return Convert.ToInt32(
                    valor);
            }
            catch
            {
                return 0;
            }
        }

        // =========================================================
        // AGREGAR
        // =========================================================

        private void btnAgregar_Click(
            object sender,
            EventArgs e)
        {
            using TrabajadoresVentana ventana =
                new TrabajadoresVentana(
                    null!);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                CargarTrabajadores();
            }
        }

        // =========================================================
        // MODIFICAR
        // =========================================================

        private void btnModificar_Click(
            object sender,
            EventArgs e)
        {
            Clases.Trabajador? trabajador =
                ObtenerTrabajadorSeleccionado();

            if (trabajador is null)
            {
                MessageBox.Show(
                    "Seleccione un trabajador.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using TrabajadoresVentana ventana =
                new TrabajadoresVentana(
                    trabajador);

            if (ventana.ShowDialog(this) ==
                DialogResult.OK)
            {
                CargarTrabajadores();
            }
        }

        // =========================================================
        // ELIMINAR
        // =========================================================

        private void btnBorrar_Click(
            object sender,
            EventArgs e)
        {
            Clases.Trabajador? trabajador =
                ObtenerTrabajadorSeleccionado();

            if (trabajador is null)
            {
                MessageBox.Show(
                    "Seleccione un trabajador.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string nombreMostrar =
                string.IsNullOrWhiteSpace(
                    trabajador.Nombre)
                    ? trabajador.NoReloj ?? string.Empty
                    : trabajador.Nombre;

            DialogResult confirmacion =
                MessageBox.Show(
                    $"¿Está seguro de eliminar al trabajador \"{nombreMostrar}\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmacion !=
                DialogResult.Yes)
            {
                return;
            }

            string noReloj =
                trabajador.NoReloj ??
                string.Empty;

            Clases.Mensaje respuesta =
                Clases.Trabajador
                    .BorrarTrabajador(
                        trabajador.Id);

            if (respuesta.Id != 1)
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "No se pudo eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // =====================================================
            // RESPALDO DE ARCHIVOS DEL TRABAJADOR
            // =====================================================

            try
            {
                string carpetaRespaldo =
                    Clases.RutasArchivos
                        .MoverCarpetaTrabajadorAEliminados(
                            noReloj);

                if (!string.IsNullOrWhiteSpace(
                    carpetaRespaldo))
                {
                    MessageBox.Show(
                        "La carpeta física del trabajador se movió a respaldo:\n\n" +
                        carpetaRespaldo,
                        "Respaldo generado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "El trabajador fue eliminado de la base de datos, " +
                    "pero no fue posible mover su carpeta al respaldo.\n\n" +
                    ex.Message,
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            MessageBox.Show(
                respuesta.Nombre,
                "Operación completada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            CargarTrabajadores();
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
        // BUSCAR
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

            CargarTrabajadores();
        }

        // =========================================================
        // DOBLE CLIC
        // =========================================================

        private void dgvTrabajadores_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            dgvTrabajadores.ClearSelection();

            dgvTrabajadores.Rows[e.RowIndex]
                .Selected =
                    true;

            DataGridViewCell? primeraCeldaVisible =
                dgvTrabajadores.Rows[e.RowIndex]
                    .Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            if (primeraCeldaVisible is not null)
            {
                dgvTrabajadores.CurrentCell =
                    primeraCeldaVisible;
            }

            btnModificar_Click(
                sender,
                e);
        }
    }
}