using Aseguranza.UI;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class BuscarTrabajadores : Form
    {
        // =========================================================
        // EVENTO
        // =========================================================

        public event Action<Clases.Trabajador>?
            trabajadorSeleccionado;

        // =========================================================
        // ESTADO VISUAL
        // =========================================================

        private Label? _lblRegistros;

        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public BuscarTrabajadores()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void BuscarTrabajadores_Load(
            object sender,
            EventArgs e)
        {
            CargarTrabajadores();

            txtBuscar.Focus();
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
                "Buscar trabajador",
                new Size(
                    1000,
                    650));

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
                    "Buscar trabajador",
                    "Localiza y selecciona un trabajador registrado en el sistema");

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
            // ETIQUETA BUSCAR
            // =====================================================

            label1.Text =
                "Buscar trabajador";

            label1.AutoSize =
                true;

            label1.Location =
                new Point(
                    20,
                    20);

            label1.ForeColor =
                AppColors.TextPrimary;

            label1.Font =
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
                            pnlContenido.ClientSize.Width - 190,
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
            // BOTÓN LIMPIAR
            // =====================================================

            Button btnLimpiar =
                new Button();

            ButtonStyler.Apply(
                btnLimpiar,
                "Limpiar",
                AppColors.Neutral,
                icon: null,
                width: 130,
                height: 42);

            btnLimpiar.Name =
                "btnLimpiar";

            btnLimpiar.Location =
                new Point(
                    pnlContenido.ClientSize.Width - 150,
                    47);

            btnLimpiar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnLimpiar.Click +=
                (_, _) =>
                {
                    txtBuscar.Clear();

                    CargarTrabajadores();

                    txtBuscar.Focus();
                };

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
                        "0 trabajadores encontrados",

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
            // PANEL TABLA
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
                btnAceptar,
                "Seleccionar",
                AppColors.Primary,
                AppIcons.Search,
                width: 160);

            ButtonStyler.Apply(
                btnRegresar,
                "Cancelar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 138);

            int yBotones =
                pnlContenido.ClientSize.Height - 58;

            btnAceptar.Location =
                new Point(
                    20,
                    yBotones);

            btnRegresar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    btnRegresar.Width -
                    20,
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
                label1);

            pnlContenido.Controls.Add(
                pnlBuscar);

            pnlContenido.Controls.Add(
                btnLimpiar);

            pnlContenido.Controls.Add(
                _lblRegistros);

            pnlContenido.Controls.Add(
                pnlTabla);

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

            ValidarBotonSeleccionar();
        }

        // =========================================================
        // CONFIGURAR COLUMNAS
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
                        "No se encontraron trabajadores",

                    1 =>
                        "1 trabajador mostrado",

                    _ =>
                        $"{cantidad} trabajadores mostrados"
                };
        }

        // =========================================================
        // VALIDAR BOTÓN
        // =========================================================

        private void ValidarBotonSeleccionar()
        {
            bool hayTrabajadores =
                dgvTrabajadores.Rows.Count > 0;

            btnAceptar.Enabled =
                hayTrabajadores;

            ButtonStyler.UpdateEnabledState(
                btnAceptar,
                AppColors.Primary);
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

            SeleccionarTrabajadorActual();
        }

        // =========================================================
        // BOTÓN SELECCIONAR
        // =========================================================

        private void btnAceptar_Click(
            object sender,
            EventArgs e)
        {
            SeleccionarTrabajadorActual();
        }

        // =========================================================
        // SELECCIONAR TRABAJADOR
        // =========================================================

        private void SeleccionarTrabajadorActual()
        {
            if (dgvTrabajadores.CurrentRow is null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Aviso",
                    "Seleccione un trabajador.");

                return;
            }

            if (dgvTrabajadores
                    .CurrentRow
                    .DataBoundItem
                is not DataRowView fila)
            {
                AppDialog.ShowWarning(
                    this,
                    "Aviso",
                    "No fue posible obtener la información del trabajador seleccionado.");

                return;
            }

            Clases.Trabajador trabajador =
                CrearTrabajadorDesdeFila(
                    fila);

            trabajadorSeleccionado?.Invoke(
                trabajador);

            DialogResult =
                DialogResult.OK;

            Close();
        }

        // =========================================================
        // CREAR TRABAJADOR DESDE FILA
        // =========================================================

        private static Clases.Trabajador
            CrearTrabajadorDesdeFila(
                DataRowView fila)
        {
            return new Clases.Trabajador
            {
                Id =
                    ObtenerEntero(
                        fila,
                        "Id"),

                NoReloj =
                    ObtenerTexto(
                        fila,
                        "NoReloj"),

                Nombre =
                    ObtenerTexto(
                        fila,
                        "Nombre"),

                RutaFoto =
                    ObtenerTexto(
                        fila,
                        "RutaFoto"),

                IdLocalidad =
                    ObtenerEntero(
                        fila,
                        "IdLocalidad"),

                NombreLocalidad =
                    ObtenerTexto(
                        fila,
                        "NombreLocalidad"),

                IdTurno =
                    ObtenerEntero(
                        fila,
                        "IdTurno"),

                NombreTurno =
                    ObtenerTexto(
                        fila,
                        "NombreTurno"),

                IdPlanta =
                    ObtenerEntero(
                        fila,
                        "IdPlanta"),

                NombrePlanta =
                    ObtenerTexto(
                        fila,
                        "NombrePlanta"),

                IdLinea =
                    ObtenerEntero(
                        fila,
                        "IdLinea"),

                NombreLinea =
                    ObtenerTexto(
                        fila,
                        "NombreLinea")
            };
        }

        private static string ObtenerTexto(
            DataRowView fila,
            string columna)
        {
            if (!fila.Row.Table.Columns.Contains(
                columna))
            {
                return string.Empty;
            }

            object valor =
                fila[columna];

            if (valor is null ||
                valor == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(
                valor)
                ?? string.Empty;
        }

        private static int ObtenerEntero(
            DataRowView fila,
            string columna)
        {
            if (!fila.Row.Table.Columns.Contains(
                columna))
            {
                return 0;
            }

            object valor =
                fila[columna];

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
    }
}