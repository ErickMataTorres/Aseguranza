using Aseguranza.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Certificadores : Form
    {
        // =========================================================
        // ESTADO VISUAL
        // =========================================================

        private Label? _lblRegistros;
        private Label? _lblSinFoto;
        private bool _estiloAplicado;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Certificadores()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void Certificadores_Load(
            object sender,
            EventArgs e)
        {
            CargarCertificadores();

            LimpiarSeleccionTrabajador();
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

            // =====================================================
            // FORMULARIO
            // =====================================================

            FormStyler.ApplyBase(
                this,
                "Certificadores",
                new Size(
                    1100,
                    760));

            DoubleBuffered = true;

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Certificadores",
                    "Administra los trabajadores autorizados como certificadores");

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
            // PANEL DEL TRABAJADOR
            // =====================================================

            Panel pnlTrabajador =
                new Panel
                {
                    Name =
                        "pnlTrabajador",

                    Location =
                        new Point(
                            20,
                            20),

                    Size =
                        new Size(
                            760,
                            210),

                    BackColor =
                        AppColors.SectionBackground,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlTrabajador,
                10);

            // =====================================================
            // TÍTULO SELECCIÓN
            // =====================================================

            Label lblTituloTrabajador =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            15),

                    Text =
                        "Seleccionar trabajador",

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            13F,
                            FontStyle.Bold)
                };

            Label lblSubtituloTrabajador =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            39),

                    Text =
                        "Busca un trabajador antes de agregarlo como certificador.",

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(9F)
                };

            // =====================================================
            // NO. RELOJ
            // =====================================================

            lblNoReloj.Text =
                "No. Reloj";

            lblNoReloj.AutoSize =
                true;

            lblNoReloj.Location =
                new Point(
                    18,
                    70);

            lblNoReloj.ForeColor =
                AppColors.TextPrimary;

            lblNoReloj.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            Panel pnlNoReloj =
                new Panel
                {
                    Name =
                        "pnlNoReloj",

                    Location =
                        new Point(
                            18,
                            94),

                    Size =
                        new Size(
                            190,
                            42),

                    BackColor =
                        Color.White
                };

            txtNoReloj.Enabled =
                true;

            txtNoReloj.ReadOnly =
                false;

            txtNoReloj.Location =
                new Point(
                    12,
                    10);

            txtNoReloj.Size =
                new Size(
                    pnlNoReloj.ClientSize.Width - 24,
                    25);

            txtNoReloj.BorderStyle =
                BorderStyle.None;

            txtNoReloj.BackColor =
                Color.White;

            txtNoReloj.ForeColor =
                AppColors.TextPrimary;

            txtNoReloj.Font =
                AppFonts.Light(11F);

            txtNoReloj.PlaceholderText =
                "Escribe No. Reloj...";

            InputStyler.ApplyOutlinedInput(
                pnlNoReloj,
                txtNoReloj);

            pnlNoReloj.Controls.Add(
                txtNoReloj);

            txtNoReloj.TextChanged +=
                (_, _) =>
                {
                    LimpiarDetallesTrabajador();
                };

            // =====================================================
            // BOTONES DE BÚSQUEDA / SELECCIÓN
            // =====================================================

            ButtonStyler.Apply(
                btnBuscar,
                "Buscar",
                AppColors.Primary,
                AppIcons.Search,
                width: 120);

            btnBuscar.Location =
                new Point(
                    220,
                    94);

            // =====================================================
            // BOTÓN SELECCIONAR
            // =====================================================

            Button btnSeleccionar =
                new Button();

            ButtonStyler.Apply(
                btnSeleccionar,
                "Seleccionar",
                AppColors.Secondary,
                AppIcons.Search,
                width: 150);

            btnSeleccionar.Location =
                new Point(
                    352,
                    94);

            btnSeleccionar.Click +=
                (_, _) =>
                {
                    SeleccionarTrabajadorDesdeVentana();
                };

            // =====================================================
            // BOTÓN AGREGAR CERTIFICADOR
            // =====================================================

            ButtonStyler.Apply(
                btnAgregar,
                "Agregar certificador",
                AppColors.Primary,
                AppIcons.Add,
                width: 228);

            btnAgregar.Location =
                new Point(
                    514,
                    94);

            // =====================================================
            // NOMBRE
            // =====================================================

            ConfigurarEtiquetaCampo(
                lblNombre,
                "Nombre:",
                18,
                151);

            ConfigurarEtiquetaValor(
                lblMostrarNombre,
                88,
                151);

            // =====================================================
            // LOCALIDAD
            // =====================================================

            ConfigurarEtiquetaCampo(
                lblLocalidad,
                "Localidad:",
                18,
                180);

            ConfigurarEtiquetaValor(
                lblMostrarLocalidad,
                110,
                180);

            // =====================================================
            // TURNO
            // =====================================================

            ConfigurarEtiquetaCampo(
                lblTurno,
                "Turno:",
                230,
                180);

            ConfigurarEtiquetaValor(
                lblMostrarTurno,
                282,
                180);

            // =====================================================
            // PLANTA
            // =====================================================

            ConfigurarEtiquetaCampo(
                lblPlanta,
                "Planta:",
                390,
                180);

            ConfigurarEtiquetaValor(
                lblMostrarPlanta,
                445,
                180);

            // =====================================================
            // LÍNEA
            // =====================================================

            ConfigurarEtiquetaCampo(
                lblLinea,
                "Línea:",
                550,
                180);

            ConfigurarEtiquetaValor(
                lblMostrarLinea,
                598,
                180);

            // =====================================================
            // AGREGAR CONTROLES PANEL TRABAJADOR
            // =====================================================

            pnlTrabajador.Controls.Add(
                lblTituloTrabajador);

            pnlTrabajador.Controls.Add(
                lblSubtituloTrabajador);

            pnlTrabajador.Controls.Add(
                lblNoReloj);

            pnlTrabajador.Controls.Add(
                pnlNoReloj);

            pnlTrabajador.Controls.Add(
                btnBuscar);

            pnlTrabajador.Controls.Add(
                btnSeleccionar);

            pnlTrabajador.Controls.Add(
                btnAgregar);

            pnlTrabajador.Controls.Add(
                lblNombre);

            pnlTrabajador.Controls.Add(
                lblMostrarNombre);

            pnlTrabajador.Controls.Add(
                lblLocalidad);

            pnlTrabajador.Controls.Add(
                lblMostrarLocalidad);

            pnlTrabajador.Controls.Add(
                lblTurno);

            pnlTrabajador.Controls.Add(
                lblMostrarTurno);

            pnlTrabajador.Controls.Add(
                lblPlanta);

            pnlTrabajador.Controls.Add(
                lblMostrarPlanta);

            pnlTrabajador.Controls.Add(
                lblLinea);

            pnlTrabajador.Controls.Add(
                lblMostrarLinea);

            // =====================================================
            // PANEL FOTOGRAFÍA
            // =====================================================

            Panel pnlFoto =
                new Panel
                {
                    Name =
                        "pnlFoto",

                    Location =
                        new Point(
                            800,
                            20),

                    Size =
                        new Size(
                            240,
                            210),

                    BackColor =
                        AppColors.SectionBackground,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Right
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlFoto,
                10);

            Label lblTituloFoto =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            15),

                    Text =
                        "Fotografía",

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            13F,
                            FontStyle.Bold)
                };

            pictureBox1.Location =
                new Point(
                    18,
                    48);

            pictureBox1.Size =
                new Size(
                    204,
                    144);

            pictureBox1.SizeMode =
                PictureBoxSizeMode.Zoom;

            pictureBox1.BorderStyle =
                BorderStyle.None;

            pictureBox1.BackColor =
                Color.White;

            RoundedControlHelper.ApplyRoundedRegion(
                pictureBox1,
                8);

            _lblSinFoto =
                new Label
                {
                    AutoSize =
                        false,

                    Location =
                        pictureBox1.Location,

                    Size =
                        pictureBox1.Size,

                    Text =
                        "Sin fotografía",

                    TextAlign =
                        ContentAlignment.MiddleCenter,

                    BackColor =
                        Color.White,

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(10F)
                };

            RoundedControlHelper.ApplyRoundedRegion(
                _lblSinFoto,
                8);

            pnlFoto.Controls.Add(
                lblTituloFoto);

            pnlFoto.Controls.Add(
                pictureBox1);

            pnlFoto.Controls.Add(
                _lblSinFoto);

            // =====================================================
            // BUSCADOR DE CERTIFICADORES
            // =====================================================

            lblBuscar.Text =
                "Buscar certificador";

            lblBuscar.AutoSize =
                true;

            lblBuscar.Location =
                new Point(
                    20,
                    248);

            lblBuscar.ForeColor =
                AppColors.TextPrimary;

            lblBuscar.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);

            Panel pnlBuscar =
                new Panel
                {
                    Name =
                        "pnlBuscar",

                    Location =
                        new Point(
                            20,
                            273),

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
                    273);

            btnLimpiar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnLimpiar.Click +=
                (_, _) =>
                {
                    txtBuscar.Clear();

                    CargarCertificadores();

                    txtBuscar.Focus();
                };

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
                "Escribe nombre, número de reloj, turno, planta o línea...";

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
                            323),

                    Text =
                        "Total: 0 certificadores",

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            10F,
                            FontStyle.Bold)
                };

            // =====================================================
            // DATAGRIDVIEW
            // =====================================================

            DataGridViewStyler.ApplyCatalogStyle(
                dgvCertificadores);

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
                            349),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            pnlContenido.ClientSize.Height - 427),

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

            dgvCertificadores.Dock =
                DockStyle.Fill;

            dgvCertificadores.BorderStyle =
                BorderStyle.None;

            pnlTabla.Controls.Add(
                dgvCertificadores);

            // =====================================================
            // BOTONES INFERIORES
            // =====================================================

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

            btnBorrar.Location =
                new Point(
                    20,
                    yBotones);

            btnRegresar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    btnRegresar.Width -
                    20,
                    yBotones);

            btnBorrar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            btnRegresar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            // =====================================================
            // AGREGAR CONTROLES PRINCIPALES
            // =====================================================

            pnlContenido.Controls.Add(
                pnlTrabajador);

            pnlContenido.Controls.Add(
                pnlFoto);

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
                btnBorrar);

            pnlContenido.Controls.Add(
                btnRegresar);

            pnlContenido.BringToFront();

            pnlCabecera.BringToFront();

            ResumeLayout(false);

            PerformLayout();
        }

        // =========================================================
        // CONFIGURAR ETIQUETAS
        // =========================================================

        private static void ConfigurarEtiquetaCampo(
            Label label,
            string texto,
            int x,
            int y)
        {
            label.Text =
                texto;

            label.AutoSize =
                true;

            label.Location =
                new Point(
                    x,
                    y);

            label.ForeColor =
                AppColors.TextSecondary;

            label.Font =
                AppFonts.Regular(
                    10F,
                    FontStyle.Bold);
        }

        private static void ConfigurarEtiquetaValor(
            Label label,
            int x,
            int y)
        {
            label.AutoSize =
                true;

            label.Location =
                new Point(
                    x,
                    y);

            label.ForeColor =
                AppColors.TextPrimary;

            label.Font =
                AppFonts.Regular(
                    12F,
                    FontStyle.Bold);
        }

        // =========================================================
        // CARGAR CERTIFICADORES
        // =========================================================

        private void CargarCertificadores()
        {
            dgvCertificadores.DataSource =
                Clases.Certificador
                    .ConsultarCertificadores(
                        txtBuscar.Text.Trim());

            ConfigurarColumnas();

            ActualizarContador();

            if (dgvCertificadores.Rows.Count > 0)
            {
                dgvCertificadores.ClearSelection();

                dgvCertificadores.Rows[0]
                    .Selected =
                        true;

                DataGridViewCell? primeraCeldaVisible =
                    dgvCertificadores.Rows[0]
                        .Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvCertificadores.CurrentCell =
                        primeraCeldaVisible;
                }
            }
            else
            {
                dgvCertificadores.ClearSelection();

                dgvCertificadores.CurrentCell =
                    null;
            }

            ValidarBotonEliminar();
        }

        // =========================================================
        // CONFIGURAR COLUMNAS
        // =========================================================

        private void ConfigurarColumnas()
        {
            OcultarColumna(
                "Id");

            OcultarColumna(
                "IdTrabajador");

            OcultarColumna(
                "RutaFoto");

            OcultarColumna(
                "IdTurno");

            OcultarColumna(
                "IdPlanta");

            OcultarColumna(
                "IdLinea");

            ConfigurarColumna(
                "NoReloj",
                "NO. RELOJ",
                15);

            // La consulta actual devuelve NombreTrabajador.
            // Se mantiene compatibilidad con "Nombre".

            if (dgvCertificadores.Columns.Contains(
                "NombreTrabajador"))
            {
                ConfigurarColumna(
                    "NombreTrabajador",
                    "NOMBRE",
                    35);

                if (dgvCertificadores.Columns.Contains(
                    "Nombre"))
                {
                    dgvCertificadores.Columns["Nombre"]!
                        .Visible =
                            false;
                }
            }
            else
            {
                ConfigurarColumna(
                    "Nombre",
                    "NOMBRE",
                    35);
            }

            ConfigurarColumna(
                "NombreTurno",
                "TURNO",
                15);

            ConfigurarColumna(
                "NombrePlanta",
                "PLANTA",
                15);

            ConfigurarColumna(
                "NombreLinea",
                "LÍNEA",
                20);
        }

        private void OcultarColumna(
            string nombre)
        {
            if (!dgvCertificadores.Columns.Contains(
                nombre))
            {
                return;
            }

            dgvCertificadores.Columns[nombre]!
                .Visible =
                    false;
        }

        private void ConfigurarColumna(
            string nombre,
            string encabezado,
            float peso)
        {
            if (!dgvCertificadores.Columns.Contains(
                nombre))
            {
                return;
            }

            DataGridViewColumn columna =
                dgvCertificadores.Columns[nombre]!;

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
                dgvCertificadores.Rows.Count;

            _lblRegistros.Text =
                cantidad switch
                {
                    0 =>
                        "Total: 0 certificadores",

                    1 =>
                        "Total: 1 certificador",

                    _ =>
                        $"Total: {cantidad} certificadores"
                };
        }

        // =========================================================
        // MOSTRAR TRABAJADOR
        // =========================================================

        private void MostrarTrabajador(
            Clases.Trabajador trabajador)
        {
            txtNoReloj.Text =
                trabajador.NoReloj ??
                string.Empty;

            lblMostrarNombre.Text =
                trabajador.Nombre ??
                "—";

            lblMostrarLocalidad.Text =
                trabajador.NombreLocalidad ??
                "—";

            lblMostrarTurno.Text =
                trabajador.NombreTurno ??
                "—";

            lblMostrarPlanta.Text =
                trabajador.NombrePlanta ??
                "—";

            lblMostrarLinea.Text =
                trabajador.NombreLinea ??
                "—";

            MostrarFotografia(
                trabajador.RutaFoto);

            btnAgregar.Enabled =
                true;

            ButtonStyler.UpdateEnabledState(
                btnAgregar,
                AppColors.Primary);
        }

        // =========================================================
        // LIMPIAR SELECCIÓN
        // =========================================================

        private void LimpiarSeleccionTrabajador()
        {
            txtNoReloj.Text =
                string.Empty;

            LimpiarDetallesTrabajador();
        }

        private void LimpiarDetallesTrabajador()
        {
            lblMostrarNombre.Text =
                "—";

            lblMostrarLocalidad.Text =
                "—";

            lblMostrarTurno.Text =
                "—";

            lblMostrarPlanta.Text =
                "—";

            lblMostrarLinea.Text =
                "—";

            MostrarFotografia(
                null);

            btnAgregar.Enabled =
                false;

            ButtonStyler.UpdateEnabledState(
                btnAgregar,
                AppColors.Primary);
        }

        // =========================================================
        // REINICIAR
        // =========================================================

        private void ReiniciarPantalla()
        {
            txtBuscar.Text =
                string.Empty;

            LimpiarSeleccionTrabajador();

            CargarCertificadores();

            txtNoReloj.Focus();
        }

        // =========================================================
        // FOTOGRAFÍA
        // =========================================================

        private void MostrarFotografia(
            string? rutaFoto)
        {
            pictureBox1.ImageLocation =
                null;

            if (string.IsNullOrWhiteSpace(
                    rutaFoto) ||
                !File.Exists(
                    rutaFoto))
            {
                pictureBox1.Visible =
                    false;

                if (_lblSinFoto is not null)
                {
                    _lblSinFoto.Visible =
                        true;

                    _lblSinFoto.BringToFront();
                }

                return;
            }

            pictureBox1.ImageLocation =
                rutaFoto;

            pictureBox1.Visible =
                true;

            if (_lblSinFoto is not null)
            {
                _lblSinFoto.Visible =
                    false;
            }
        }

        // =========================================================
        // VALIDAR BOTÓN ELIMINAR
        // =========================================================

        private void ValidarBotonEliminar()
        {
            bool hayRegistros =
                dgvCertificadores.Rows.Count > 0;

            btnBorrar.Enabled =
                hayRegistros;

            ButtonStyler.UpdateEnabledState(
                btnBorrar,
                AppColors.Danger);
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
        // NO. RELOJ - ENTER
        // =========================================================

        private void txtNoReloj_KeyPress(
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

            BuscarTrabajadorPorNoReloj();
        }

        // =========================================================
        // BUSCAR TRABAJADOR
        // =========================================================

        private void BuscarTrabajadorPorNoReloj()
        {
            string noReloj =
                txtNoReloj.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    noReloj))
            {
                AppDialog.ShowWarning(
                    this,
                    "Dato requerido",
                    "Ingrese un número de reloj.");

                txtNoReloj.Focus();

                return;
            }

            Clases.Trabajador? trabajador =
                Clases.Trabajador
                    .ConsultarTrabajador(
                        noReloj);

            if (trabajador is null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Trabajador no encontrado",
                    "No se encontró un trabajador con el número de reloj indicado.");

                LimpiarDetallesTrabajador();

                txtNoReloj.SelectAll();
                txtNoReloj.Focus();

                return;
            }

            MostrarTrabajador(
                trabajador);
        }

        // =========================================================
        // SELECCIONAR TRABAJADOR
        // =========================================================

        private void btnBuscar_Click(
            object sender,
            EventArgs e)
        {
            BuscarTrabajadorPorNoReloj();
        }

        private void SeleccionarTrabajadorDesdeVentana()
        {
            using BuscarTrabajadores ventana =
                new BuscarTrabajadores();

            if (ventana.ShowDialog(this) !=
                DialogResult.OK ||
                ventana.TrabajadorSeleccionado is null)
            {
                return;
            }

            MostrarTrabajador(
                ventana.TrabajadorSeleccionado);
        }

        // =========================================================
        // AGREGAR CERTIFICADOR
        // =========================================================

        private void btnAgregar_Click(
            object sender,
            EventArgs e)
        {
            string noReloj =
                txtNoReloj.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                noReloj))
            {
                AppDialog.ShowWarning(
                    this,
                    "Dato requerido",
                    "Debe seleccionar un trabajador primero.");

                btnBuscar.Focus();

                return;
            }

            Clases.Certificador certificador =
                new Clases.Certificador
                {
                    NoReloj =
                        noReloj
                };

            Clases.Mensaje respuesta =
                certificador
                    .GuardarCertificador();

            if (respuesta.Id == 1)
            {
                AppDialog.ShowInfo(
                    this,
                    "Operación completada",
                    respuesta.Nombre);

                ReiniciarPantalla();

                return;
            }

            AppDialog.ShowWarning(
                this,
                "No se pudo agregar",
                respuesta.Nombre);

            LimpiarSeleccionTrabajador();
        }

        // =========================================================
        // ELIMINAR CERTIFICADOR
        // =========================================================

        private void btnBorrar_Click(
            object sender,
            EventArgs e)
        {
            if (dgvCertificadores.Rows.Count == 0 ||
                dgvCertificadores.CurrentRow is null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Aviso",
                    "No hay un certificador seleccionado para eliminar.");

                return;
            }

            object? idValor =
                dgvCertificadores
                    .CurrentRow
                    .Cells["Id"]
                    .Value;

            if (idValor is null ||
                idValor == DBNull.Value)
            {
                return;
            }

            int id =
                Convert.ToInt32(
                    idValor);

            string nombre =
                ObtenerNombreCertificadorSeleccionado();

            string mensajeConfirmacion =
                string.IsNullOrWhiteSpace(
                    nombre)
                    ? "¿Está seguro de eliminar el certificador seleccionado?"
                    : $"¿Está seguro de eliminar a \"{nombre}\" de la lista de certificadores?";

            bool confirmacion =
                AppDialog.Confirm(
                    this,
                    "Confirmar eliminación",
                    mensajeConfirmacion,
                    "Eliminar");

            if (!confirmacion)
            {
                return;
            }

            Clases.Mensaje respuesta =
                Clases.Certificador
                    .BorrarCertificador(
                        id);

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

            ReiniciarPantalla();
        }

        // =========================================================
        // OBTENER NOMBRE DEL CERTIFICADOR
        // =========================================================

        private string ObtenerNombreCertificadorSeleccionado()
        {
            if (dgvCertificadores.CurrentRow is null)
            {
                return string.Empty;
            }

            if (dgvCertificadores.Columns.Contains(
                "NombreTrabajador"))
            {
                return Convert.ToString(
                    dgvCertificadores
                        .CurrentRow
                        .Cells["NombreTrabajador"]
                        .Value)
                    ?? string.Empty;
            }

            if (dgvCertificadores.Columns.Contains(
                "Nombre"))
            {
                return Convert.ToString(
                    dgvCertificadores
                        .CurrentRow
                        .Cells["Nombre"]
                        .Value)
                    ?? string.Empty;
            }

            return string.Empty;
        }

        // =========================================================
        // BUSCADOR CERTIFICADORES
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

            CargarCertificadores();
        }
    }
}