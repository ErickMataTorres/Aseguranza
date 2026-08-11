using Aseguranza.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Certificaciones : Form
    {
        // =========================================================
        // ESTADO VISUAL
        // =========================================================

        private readonly Font _boldFont =
            AppFonts.Regular(
                9.5F,
                FontStyle.Bold);

        private bool _estiloAplicado;

        private Panel? _pnlAcciones;

        // =========================================================
        // COLORES DE ESTADO
        // =========================================================

        private static readonly Color VigenteFondo =
            Color.FromArgb(
                198,
                239,
                206);

        private static readonly Color VigenteSeleccion =
            Color.FromArgb(
                34,
                177,
                76);

        private static readonly Color VigenteTexto =
            Color.FromArgb(
                24,
                94,
                45);

        private static readonly Color PorVencerFondo =
            Color.FromArgb(
                255,
                235,
                156);

        private static readonly Color PorVencerSeleccion =
            Color.FromArgb(
                230,
                180,
                0);

        private static readonly Color PorVencerTexto =
            Color.FromArgb(
                120,
                82,
                0);

        private static readonly Color VencidaFondo =
            Color.FromArgb(
                255,
                199,
                206);

        private static readonly Color VencidaSeleccion =
            Color.FromArgb(
                230,
                55,
                70);

        private static readonly Color VencidaTexto =
            Color.FromArgb(
                140,
                30,
                40);

        private static readonly Color SinCertificarFondo =
            Color.FromArgb(
                218,
                225,
                235);

        private static readonly Color SinCertificarSeleccion =
            Color.FromArgb(
                90,
                105,
                125);        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Certificaciones()
        {
            InitializeComponent();

            AplicarEstiloVisual();
        }

        // =========================================================
        // LOAD
        // =========================================================

        private void Certificaciones_Load(
            object sender,
            EventArgs e)
        {
            /*
             * CellFormatting y SelectionChanged ya están
             * conectados desde Certificaciones.Designer.cs.
             *
             * cbMostrarPor no tiene su evento conectado
             * en el Designer, por eso lo conectamos aquí.
             */

            if (cbMostrarPor.SelectedIndex < 0)
            {
                cbMostrarPor.SelectedIndex =
                    0;
            }

            cbMostrarPor.SelectedIndexChanged -=
                cbMostrarPor_SelectedIndexChanged;

            cbMostrarPor.SelectedIndexChanged +=
                cbMostrarPor_SelectedIndexChanged;

            lblResumenEstados.Text =
                "Total: 0   |   Vigentes: 0   |   " +
                "Por vencer: 0   |   Vencidas: 0   |   " +
                "Sin certificar: 0";

            lblTrabajadorSeleccionado.Text =
                "Seleccionado: ninguno";

            ConfigurarToolTips();

            CargarTrabajadores();
        }

        // =========================================================
        // ESTILO VISUAL
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
                "Certificaciones",
                new Size(
                    1180,
                    650));

            DoubleBuffered =
                true;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            UpdateStyles();

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Certificaciones",
                    "Consulta y administra las certificaciones del personal");

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
            // PANEL DE FILTROS
            // =====================================================

            Panel pnlFiltros =
                new Panel
                {
                    Name =
                        "pnlFiltros",

                    Location =
                        new Point(
                            20,
                            18),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            112),

                    BackColor =
                        AppColors.AlternateRow,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlFiltros,
                10);

            // =====================================================
            // MOSTRAR POR
            // =====================================================

            lblMostrarPor.Text =
                "Mostrar por";

            lblMostrarPor.AutoSize =
                true;

            lblMostrarPor.Location =
                new Point(
                    18,
                    13);

            lblMostrarPor.ForeColor =
                AppColors.TextPrimary;

            lblMostrarPor.Font =
                AppFonts.Regular(
                    9.5F,
                    FontStyle.Bold);

            Panel pnlMostrarPor =
                new Panel
                {
                    Name =
                        "pnlMostrarPor",

                    Location =
                        new Point(
                            18,
                            39),

                    Size =
                        new Size(
                            220,
                            42),

                    BackColor =
                        Color.White
                };

            cbMostrarPor.Location =
                new Point(
                    8,
                    7);

            cbMostrarPor.Size =
                new Size(
                    pnlMostrarPor.ClientSize.Width - 16,
                    28);

            cbMostrarPor.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            ComboBoxStyler.ApplyOutlinedComboBox(
                pnlMostrarPor,
                cbMostrarPor);

            pnlMostrarPor.Controls.Add(
                cbMostrarPor);

            // =====================================================
            // BUSCADOR
            // =====================================================

            lblBuscar.Text =
                "Buscar trabajador";

            lblBuscar.AutoSize =
                true;

            lblBuscar.Location =
                new Point(
                    258,
                    13);

            lblBuscar.ForeColor =
                AppColors.TextPrimary;

            lblBuscar.Font =
                AppFonts.Regular(
                    9.5F,
                    FontStyle.Bold);

            int anchoBotonBuscar =
                130;

            int anchoBotonLimpiar =
                140;

            int margenDerecho =
                18;

            int espacio =
                10;

            int xLimpiar =
                pnlFiltros.ClientSize.Width -
                margenDerecho -
                anchoBotonLimpiar;

            int xBuscarBoton =
                xLimpiar -
                espacio -
                anchoBotonBuscar;

            int anchoBuscador =
                xBuscarBoton -
                espacio -
                258;

            Panel pnlBuscar =
                new Panel
                {
                    Name =
                        "pnlBuscar",

                    Location =
                        new Point(
                            258,
                            39),

                    Size =
                        new Size(
                            anchoBuscador,
                            42),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
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
                AppFonts.Light(
                    10.5F);

            txtBuscar.PlaceholderText =
                "No. Reloj, nombre, localidad, turno, planta o línea...";

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
            // BOTÓN BUSCAR
            // =====================================================

            ButtonStyler.Apply(
                btnBuscar,
                "Buscar",
                AppColors.Primary,
                AppIcons.Search,
                width: anchoBotonBuscar);

            btnBuscar.Location =
                new Point(
                    xBuscarBoton,
                    39);

            btnBuscar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            // =====================================================
            // BOTÓN LIMPIAR
            // =====================================================

            ButtonStyler.Apply(
                btnLimpiarBusqueda,
                "Limpiar",
                AppColors.Neutral,
                string.Empty,
                width: anchoBotonLimpiar);

            btnLimpiarBusqueda.Image =
                null;

            btnLimpiarBusqueda.Location =
                new Point(
                    xLimpiar,
                    39);

            btnLimpiarBusqueda.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            // =====================================================
            // AVISO
            // =====================================================

            lblAvisoLimite.Location =
                new Point(
                    18,
                    84);

            lblAvisoLimite.Size =
                new Size(
                    pnlFiltros.ClientSize.Width - 36,
                    22);

            lblAvisoLimite.ForeColor =
                AppColors.TextSecondary;

            lblAvisoLimite.Font =
                AppFonts.Light(
                    9F);

            lblAvisoLimite.TextAlign =
                ContentAlignment.MiddleLeft;

            lblAvisoLimite.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // =====================================================
            // AGREGAR FILTROS
            // =====================================================

            pnlFiltros.Controls.Add(
                lblMostrarPor);

            pnlFiltros.Controls.Add(
                pnlMostrarPor);

            pnlFiltros.Controls.Add(
                lblBuscar);

            pnlFiltros.Controls.Add(
                pnlBuscar);

            pnlFiltros.Controls.Add(
                btnBuscar);

            pnlFiltros.Controls.Add(
                btnLimpiarBusqueda);

            pnlFiltros.Controls.Add(
                lblAvisoLimite);

            // =====================================================
            // RESUMEN
            // =====================================================

            lblResumenEstados.Location =
                new Point(
                    20,
                    140);

            lblResumenEstados.Size =
                new Size(
                    630,
                    26);

            lblResumenEstados.ForeColor =
                AppColors.TextPrimary;

            lblResumenEstados.Font =
                AppFonts.Regular(
                    9.5F,
                    FontStyle.Bold);

            lblResumenEstados.TextAlign =
                ContentAlignment.MiddleLeft;

            // =====================================================
            // LEYENDA
            // =====================================================

            ConfigurarLeyendaEstados();

            pnlLeyendaEstados.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    pnlLeyendaEstados.Width -
                    20,
                    136);

            pnlLeyendaEstados.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            lblLeyendaEstados.Visible =
                false;

            // =====================================================
            // TRABAJADOR SELECCIONADO
            // =====================================================

            lblTrabajadorSeleccionado.Location =
                new Point(
                    20,
                    170);

            lblTrabajadorSeleccionado.Size =
                new Size(
                    pnlContenido.ClientSize.Width - 40,
                    25);

            lblTrabajadorSeleccionado.ForeColor =
                AppColors.TextSecondary;

            lblTrabajadorSeleccionado.Font =
                AppFonts.Light(
                    9.5F);

            lblTrabajadorSeleccionado.TextAlign =
                ContentAlignment.MiddleLeft;

            lblTrabajadorSeleccionado.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // =====================================================
            // BARRA INFERIOR FIJA
            // =====================================================

            _pnlAcciones =
                new Panel
                {
                    Name =
                        "pnlAcciones",

                    Dock =
                        DockStyle.Bottom,

                    Height =
                        62,

                    BackColor =
                        Color.White
                };

            _pnlAcciones.Paint +=
                PnlAcciones_Paint;

            /*
             * Primero agregamos la barra a la tarjeta.
             * Así WinForms calcula correctamente su ancho
             * antes de colocar el botón Regresar.
             */

            pnlContenido.Controls.Add(
                _pnlAcciones);

            // =====================================================
            // BOTONES DE ACCIÓN
            // =====================================================

            ButtonStyler.Apply(
    btnAgregarTrabajador,
    "Agregar",
    AppColors.Primary,
    AppIcons.Add,
    width: 138);

            ButtonStyler.Apply(
                btnModificarTrabajador,
                "Modificar",
                AppColors.Secondary,
                AppIcons.Edit,
                width: 148);

            ButtonStyler.Apply(
                btnBorrarTrabajador,
                "Eliminar",
                AppColors.Danger,
                AppIcons.Delete,
                width: 138);

            ButtonStyler.Apply(
                btnCertificaciones,
                "Certificaciones",
                AppColors.Primary,
                AppIcons.Search,
                width: 175);

            ButtonStyler.Apply(
                btnVistaPreviaCredencial,
                "Vista previa",
                AppColors.Secondary,
                AppIcons.Search,
                width: 155);

            ButtonStyler.Apply(
                btnImprimirCredencial,
                "Imprimir",
                AppColors.Neutral,
                string.Empty,
                width: 130);

            btnImprimirCredencial.Image =
                null;

            ButtonStyler.Apply(
                btnRegresar,
                "Regresar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 138);

            int yBoton =
    10;

            int xBoton =
                20;

            btnAgregarTrabajador.Location =
                new Point(
                    xBoton,
                    yBoton);

            xBoton +=
                btnAgregarTrabajador.Width + 10;

            btnModificarTrabajador.Location =
                new Point(
                    xBoton,
                    yBoton);

            xBoton +=
                btnModificarTrabajador.Width + 10;

            btnBorrarTrabajador.Location =
                new Point(
                    xBoton,
                    yBoton);

            xBoton +=
                btnBorrarTrabajador.Width + 10;

            btnCertificaciones.Location =
                new Point(
                    xBoton,
                    yBoton);

            xBoton +=
                btnCertificaciones.Width + 10;

            btnVistaPreviaCredencial.Location =
                new Point(
                    xBoton,
                    yBoton);

            xBoton +=
                btnVistaPreviaCredencial.Width + 10;

            btnImprimirCredencial.Location =
                new Point(
                    xBoton,
                    yBoton);

            btnRegresar.Location =
                new Point(
                    _pnlAcciones.ClientSize.Width -
                    btnRegresar.Width -
                    20,
                    yBoton);

            btnRegresar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            _pnlAcciones.Controls.Add(
                btnAgregarTrabajador);

            _pnlAcciones.Controls.Add(
                btnModificarTrabajador);

            _pnlAcciones.Controls.Add(
                btnBorrarTrabajador);

            _pnlAcciones.Controls.Add(
                btnCertificaciones);

            _pnlAcciones.Controls.Add(
                btnVistaPreviaCredencial);

            _pnlAcciones.Controls.Add(
                btnImprimirCredencial);

            _pnlAcciones.Controls.Add(
                btnRegresar);

            // =====================================================
            // TABLA
            // =====================================================

            ConfigurarGridTrabajadores();

            int yTabla =
                201;

            int altoTabla =
                pnlContenido.ClientSize.Height -
                yTabla -
                _pnlAcciones.Height -
                10;

            if (altoTabla < 100)
            {
                altoTabla =
                    100;
            }

            Panel pnlTabla =
                new Panel
                {
                    Name =
                        "pnlTabla",

                    Location =
                        new Point(
                            20,
                            yTabla),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            altoTabla),

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
            // AGREGAR CONTROLES A TARJETA
            // =====================================================

            pnlContenido.Controls.Add(
                pnlFiltros);

            pnlContenido.Controls.Add(
                lblResumenEstados);

            pnlContenido.Controls.Add(
                pnlLeyendaEstados);

            pnlContenido.Controls.Add(
                lblTrabajadorSeleccionado);

            pnlContenido.Controls.Add(
                pnlTabla);

            /*
             * La barra siempre debe quedar por encima
             * de cualquier otro control.
             */

            _pnlAcciones.BringToFront();

            pnlFiltros.BringToFront();

            pnlContenido.BringToFront();

            pnlCabecera.BringToFront();

            ResumeLayout(
                false);

            PerformLayout();
        }

        // =========================================================
        // BORDE SUPERIOR DE LA BARRA DE ACCIONES
        // =========================================================

        private void PnlAcciones_Paint(
            object? sender,
            PaintEventArgs e)
        {
            if (_pnlAcciones is null)
            {
                return;
            }

            using Pen pen =
                new Pen(
                    AppColors.BorderMedium,
                    1F);

            e.Graphics.DrawLine(
                pen,
                0,
                0,
                _pnlAcciones.Width,
                0);
        }

        // =========================================================
        // LEYENDA DE ESTADOS
        // =========================================================

        private void ConfigurarLeyendaEstados()
        {
            pnlLeyendaEstados.Size =
                new Size(
                    440,
                    34);

            pnlLeyendaEstados.BackColor =
                Color.Transparent;

            ConfigurarCuadroLeyenda(
                lblColorVigente,
                VigenteSeleccion,
                0);

            ConfigurarTextoLeyenda(
                lblTextoVigente,
                "Vigente",
                22);

            ConfigurarCuadroLeyenda(
                lblColorPorVencer,
                PorVencerSeleccion,
                100);

            ConfigurarTextoLeyenda(
                lblTextoPorVencer,
                "Por vencer",
                122);

            ConfigurarCuadroLeyenda(
                lblColorVencida,
                VencidaSeleccion,
                226);

            ConfigurarTextoLeyenda(
                lblTextoVencida,
                "Vencida",
                248);

            ConfigurarCuadroLeyenda(
                lblColorSinCertificar,
                SinCertificarSeleccion,
                326);

            ConfigurarTextoLeyenda(
                lblTextoSinCertificar,
                "Sin certificar",
                348);
        }

        private static void ConfigurarCuadroLeyenda(
            Label label,
            Color color,
            int x)
        {
            label.Location =
                new Point(
                    x,
                    8);

            label.Size =
                new Size(
                    16,
                    16);

            label.BackColor =
                color;

            label.BorderStyle =
                BorderStyle.None;

            RoundedControlHelper.ApplyRoundedRegion(
                label,
                4);
        }

        private static void ConfigurarTextoLeyenda(
            Label label,
            string texto,
            int x)
        {
            label.Text =
                texto;

            label.AutoSize =
                true;

            label.Location =
                new Point(
                    x,
                    7);

            label.ForeColor =
                AppColors.TextSecondary;

            label.Font =
                AppFonts.Light(
                    9F);
        }

        // =========================================================
        // CONFIGURACIÓN DEL DATAGRIDVIEW
        // =========================================================

        private void ConfigurarGridTrabajadores()
        {
            DataGridViewStyler.ApplyCatalogStyle(
                dgvTrabajadores,
                headerHeight: 40,
                rowHeight: 36);

            dgvTrabajadores.AllowUserToAddRows =
                false;

            dgvTrabajadores.AllowUserToDeleteRows =
                false;

            dgvTrabajadores.AllowUserToResizeRows =
                false;

            dgvTrabajadores.ReadOnly =
                true;

            dgvTrabajadores.MultiSelect =
                false;

            dgvTrabajadores.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvTrabajadores.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvTrabajadores.RowHeadersVisible =
                false;

            dgvTrabajadores.ContextMenuStrip =
                cmsTrabajadores;

            dgvTrabajadores
                .ColumnHeadersDefaultCellStyle
                .Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
        }

        // =========================================================
        // CARGAR TRABAJADORES
        // =========================================================

        private void CargarTrabajadores(
            string noRelojSeleccionar = "")
        {
            string mostrarPor =
                cbMostrarPor
                    .SelectedItem?
                    .ToString()
                ?? "Todas";

            string textoBuscar =
                txtBuscar.Text.Trim();

            dgvTrabajadores.SuspendLayout();

            try
            {
                dgvTrabajadores.DataSource =
                    Clases.Trabajador
                        .ConsultarTrabajadoresEstadoCertificacion(
                            mostrarPor,
                            textoBuscar);

                OcultarColumnas();

                ConfigurarEncabezados();

                if (!string.IsNullOrWhiteSpace(
                        noRelojSeleccionar))
                {
                    SeleccionarTrabajadorPorNoReloj(
                        noRelojSeleccionar);
                }
                else
                {
                    SeleccionarPrimero();
                }

                ActualizarResumenEstados();

                ActualizarAvisoLimiteRegistros();

                SincronizarSeleccionUI();
            }
            finally
            {
                dgvTrabajadores.ResumeLayout();
            }

            /*
             * Algunos cambios de selección de DataGridView
             * terminan de procesarse después de asignar el
             * DataSource. Esta segunda sincronización evita
             * que los botones queden deshabilitados aunque
             * visualmente exista una fila seleccionada.
             */

            ProgramarSincronizacionSeleccion();

            txtBuscar.Focus();
        }

        // =========================================================
        // COLUMNAS OCULTAS
        // =========================================================

        private void OcultarColumnas()
        {
            string[] columnasOcultas =
            {
                "Id",
                "RutaFoto",
                "IdLocalidad",
                "IdTurno",
                "IdPlanta",
                "IdLinea"
            };

            foreach (string columna in
                columnasOcultas)
            {
                if (!dgvTrabajadores.Columns.Contains(
                        columna))
                {
                    continue;
                }

                dgvTrabajadores
                    .Columns[columna]!
                    .Visible =
                        false;
            }
        }

        // =========================================================
        // ENCABEZADOS
        // =========================================================

        private void ConfigurarEncabezados()
        {
            ConfigurarColumna(
                "NoReloj",
                "NO. RELOJ",
                70,
                DataGridViewContentAlignment.MiddleCenter);

            ConfigurarColumna(
                "Nombre",
                "TRABAJADOR",
                220,
                DataGridViewContentAlignment.MiddleLeft);

            ConfigurarColumna(
                "NombreLocalidad",
                "LOCALIDAD",
                90,
                DataGridViewContentAlignment.MiddleCenter);

            ConfigurarColumna(
                "NombreTurno",
                "TURNO",
                70,
                DataGridViewContentAlignment.MiddleCenter);

            ConfigurarColumna(
                "NombrePlanta",
                "PLANTA",
                80,
                DataGridViewContentAlignment.MiddleCenter);

            ConfigurarColumna(
                "NombreLinea",
                "LÍNEA",
                90,
                DataGridViewContentAlignment.MiddleCenter);

            ConfigurarColumna(
                "EstadoCertificacion",
                "ESTADO",
                100,
                DataGridViewContentAlignment.MiddleCenter);
        }

        private void ConfigurarColumna(
            string nombre,
            string encabezado,
            float peso,
            DataGridViewContentAlignment alineacion)
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

            columna.DefaultCellStyle.Alignment =
                alineacion;

            columna.HeaderCell.Style.Alignment =
                alineacion;
        }

        // =========================================================
        // AVISO DE CANTIDAD / LÍMITE
        // =========================================================

        private void ActualizarAvisoLimiteRegistros()
        {
            int total =
                dgvTrabajadores.Rows.Count;

            string textoBuscar =
                txtBuscar.Text.Trim();

            if (total >= 300 &&
                string.IsNullOrWhiteSpace(
                    textoBuscar))
            {
                lblAvisoLimite.Text =
                    "Aviso: se muestran los primeros 300 trabajadores. " +
                    "Use Buscar para localizar trabajadores fuera de esta lista.";

                lblAvisoLimite.ForeColor =
                    Color.FromArgb(
                        180,
                        100,
                        0);

                lblAvisoLimite.Visible =
                    true;

                return;
            }

            if (total >= 300 &&
                !string.IsNullOrWhiteSpace(
                    textoBuscar))
            {
                lblAvisoLimite.Text =
                    $"Mostrando hasta 300 resultados para la búsqueda: \"{textoBuscar}\".";

                lblAvisoLimite.ForeColor =
                    Color.FromArgb(
                        180,
                        100,
                        0);

                lblAvisoLimite.Visible =
                    true;

                return;
            }

            lblAvisoLimite.Text =
                total switch
                {
                    0 =>
                        "No se encontraron trabajadores.",

                    1 =>
                        "Mostrando 1 trabajador.",

                    _ =>
                        $"Mostrando {total} trabajadores."
                };

            lblAvisoLimite.ForeColor =
                AppColors.TextSecondary;

            lblAvisoLimite.Visible =
                true;
        }

        // =========================================================
        // SELECCIONAR PRIMER REGISTRO
        // =========================================================

        private void SeleccionarPrimero()
        {
            if (dgvTrabajadores.Rows.Count == 0)
            {
                dgvTrabajadores.ClearSelection();

                dgvTrabajadores.CurrentCell =
                    null;

                return;
            }

            dgvTrabajadores.ClearSelection();

            DataGridViewRow fila =
                dgvTrabajadores.Rows[0];

            fila.Selected =
                true;

            DataGridViewCell? primeraCeldaVisible =
                fila.Cells
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

        // =========================================================
        // SELECCIONAR TRABAJADOR POR NO. RELOJ
        // =========================================================

        private void SeleccionarTrabajadorPorNoReloj(
            string noReloj)
        {
            noReloj =
                noReloj.Trim();

            if (string.IsNullOrWhiteSpace(
                    noReloj))
            {
                SeleccionarPrimero();

                return;
            }

            if (!dgvTrabajadores.Columns.Contains(
                    "NoReloj"))
            {
                SeleccionarPrimero();

                return;
            }

            foreach (DataGridViewRow fila in
                dgvTrabajadores.Rows)
            {
                if (fila.IsNewRow)
                {
                    continue;
                }

                string noRelojFila =
                    fila.Cells["NoReloj"]
                        .Value?
                        .ToString()?
                        .Trim()
                    ?? string.Empty;

                if (!string.Equals(
                        noRelojFila,
                        noReloj,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                dgvTrabajadores.ClearSelection();

                fila.Selected =
                    true;

                DataGridViewCell? primeraCeldaVisible =
                    fila.Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvTrabajadores.CurrentCell =
                        primeraCeldaVisible;
                }

                SincronizarSeleccionUI();

                return;
            }

            MessageBox.Show(
                "No se encontró el trabajador en la lista actual.\n\n" +
                $"No. Reloj buscado: {noReloj}",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            SeleccionarPrimero();
        }

        // =========================================================
        // OBTENER FILA SELECCIONADA
        // =========================================================

        private DataGridViewRow?
            ObtenerFilaSeleccionada()
        {
            /*
             * Primero usamos CurrentRow.
             * Si WinForms todavía no actualizó CurrentRow,
             * usamos SelectedRows como respaldo.
             */

            if (dgvTrabajadores.CurrentRow is not null &&
                !dgvTrabajadores.CurrentRow.IsNewRow)
            {
                return dgvTrabajadores.CurrentRow;
            }

            if (dgvTrabajadores.SelectedRows.Count > 0)
            {
                DataGridViewRow fila =
                    dgvTrabajadores.SelectedRows[0];

                if (!fila.IsNewRow)
                {
                    return fila;
                }
            }

            return null;
        }

        // =========================================================
        // SINCRONIZACIÓN DE LA SELECCIÓN
        // =========================================================

        private void SincronizarSeleccionUI()
        {
            ActualizarTrabajadorSeleccionado();

            ActualizarBotonesTrabajador();
        }

        private void ProgramarSincronizacionSeleccion()
        {
            if (!IsHandleCreated ||
                IsDisposed ||
                Disposing)
            {
                return;
            }

            try
            {
                BeginInvoke(
                    new Action(
                        () =>
                        {
                            if (IsDisposed ||
                                Disposing)
                            {
                                return;
                            }

                            SincronizarSeleccionUI();

                            dgvTrabajadores.Invalidate();
                        }));
            }
            catch
            {
                /*
                 * Puede ocurrir únicamente si la ventana
                 * está cerrándose mientras existe un
                 * BeginInvoke pendiente.
                 */
            }
        }

        // =========================================================
        // TEXTO DE TRABAJADOR SELECCIONADO
        // =========================================================

        private void ActualizarTrabajadorSeleccionado()
        {
            DataGridViewRow? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                lblTrabajadorSeleccionado.Text =
                    "Seleccionado: ninguno";

                return;
            }

            string noReloj =
                ObtenerTextoFila(
                    fila,
                    "NoReloj");

            string nombre =
                ObtenerTextoFila(
                    fila,
                    "Nombre");

            if (string.IsNullOrWhiteSpace(
                    noReloj) &&
                string.IsNullOrWhiteSpace(
                    nombre))
            {
                lblTrabajadorSeleccionado.Text =
                    "Seleccionado: ninguno";

                return;
            }

            lblTrabajadorSeleccionado.Text =
                $"Seleccionado: {noReloj} - {nombre}";
        }

        // =========================================================
        // HABILITAR / DESHABILITAR BOTONES
        // =========================================================

        private void ActualizarBotonesTrabajador()
        {
            bool haySeleccion =
                ObtenerFilaSeleccionada()
                is not null;

            btnModificarTrabajador.Enabled =
                haySeleccion;

            btnBorrarTrabajador.Enabled =
                haySeleccion;

            btnCertificaciones.Enabled =
                haySeleccion;

            btnVistaPreviaCredencial.Enabled =
                haySeleccion;

            btnImprimirCredencial.Enabled =
                haySeleccion;

            ButtonStyler.UpdateEnabledState(
                btnModificarTrabajador,
                AppColors.Secondary);

            ButtonStyler.UpdateEnabledState(
                btnBorrarTrabajador,
                AppColors.Danger);

            ButtonStyler.UpdateEnabledState(
                btnCertificaciones,
                AppColors.Primary);

            ButtonStyler.UpdateEnabledState(
                btnVistaPreviaCredencial,
                AppColors.Secondary);

            ButtonStyler.UpdateEnabledState(
                btnImprimirCredencial,
                AppColors.Neutral);
        }

        // =========================================================
        // OBTENER TRABAJADOR SELECCIONADO
        // =========================================================

        private Clases.Trabajador?
            ObtenerTrabajadorSeleccionado()
        {
            DataGridViewRow? fila =
                ObtenerFilaSeleccionada();

            if (fila is null)
            {
                return null;
            }

            return new Clases.Trabajador
            {
                Id =
                    ObtenerEnteroFila(
                        fila,
                        "Id"),

                NoReloj =
                    ObtenerTextoFila(
                        fila,
                        "NoReloj"),

                Nombre =
                    ObtenerTextoFila(
                        fila,
                        "Nombre"),

                RutaFoto =
                    ObtenerTextoFila(
                        fila,
                        "RutaFoto"),

                IdLocalidad =
                    ObtenerEnteroFila(
                        fila,
                        "IdLocalidad"),

                NombreLocalidad =
                    ObtenerTextoFila(
                        fila,
                        "NombreLocalidad"),

                IdTurno =
                    ObtenerEnteroFila(
                        fila,
                        "IdTurno"),

                NombreTurno =
                    ObtenerTextoFila(
                        fila,
                        "NombreTurno"),

                IdPlanta =
                    ObtenerEnteroFila(
                        fila,
                        "IdPlanta"),

                NombrePlanta =
                    ObtenerTextoFila(
                        fila,
                        "NombrePlanta"),

                IdLinea =
                    ObtenerEnteroFila(
                        fila,
                        "IdLinea"),

                NombreLinea =
                    ObtenerTextoFila(
                        fila,
                        "NombreLinea")
            };
        }

        // =========================================================
        // HELPERS DE CELDAS
        // =========================================================

        private static string ObtenerTextoFila(
            DataGridViewRow fila,
            string columna)
        {
            if (fila.DataGridView is null ||
                !fila.DataGridView.Columns.Contains(
                    columna))
            {
                return string.Empty;
            }

            object? valor =
                fila.Cells[columna]
                    .Value;

            if (valor is null ||
                valor == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(
                valor)
                ?? string.Empty;
        }

        private static int ObtenerEnteroFila(
            DataGridViewRow fila,
            string columna)
        {
            if (fila.DataGridView is null ||
                !fila.DataGridView.Columns.Contains(
                    columna))
            {
                return 0;
            }

            object? valor =
                fila.Cells[columna]
                    .Value;

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
        // FORMATO DE FILAS SEGÚN ESTADO
        // =========================================================

        private void dgvTrabajadores_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (!dgvTrabajadores.Columns.Contains(
                    "EstadoCertificacion"))
            {
                return;
            }

            DataGridViewRow fila =
                dgvTrabajadores.Rows[
                    e.RowIndex];

            /*
             * Reiniciamos siempre la apariencia antes
             * de aplicar un estado. Esto evita que el
             * reciclado visual del DataGridView deje
             * colores incorrectos en otras filas.
             */

            fila.DefaultCellStyle.BackColor =
                e.RowIndex % 2 == 0
                    ? Color.White
                    : AppColors.AlternateRow;

            fila.DefaultCellStyle.ForeColor =
                AppColors.TextPrimary;

            fila.DefaultCellStyle.SelectionBackColor =
                AppColors.Selection;

            fila.DefaultCellStyle.SelectionForeColor =
                AppColors.TextPrimary;

            fila.DefaultCellStyle.Font =
                _boldFont;

            object? valorEstado =
                fila.Cells["EstadoCertificacion"]
                    .Value;

            if (valorEstado is null ||
                valorEstado == DBNull.Value)
            {
                return;
            }

            string estado =
                valorEstado.ToString()
                ?? string.Empty;

            switch (estado)
            {
                case "Vigente":

                    fila.DefaultCellStyle.BackColor =
                        VigenteFondo;

                    fila.DefaultCellStyle.ForeColor =
                        VigenteTexto;

                    fila.DefaultCellStyle.SelectionBackColor =
                        VigenteSeleccion;

                    fila.DefaultCellStyle.SelectionForeColor =
                        Color.White;

                    break;

                case "Por vencer":

                    fila.DefaultCellStyle.BackColor =
                        PorVencerFondo;

                    fila.DefaultCellStyle.ForeColor =
                        PorVencerTexto;

                    fila.DefaultCellStyle.SelectionBackColor =
                        PorVencerSeleccion;

                    fila.DefaultCellStyle.SelectionForeColor =
                        Color.Black;

                    break;

                case "Vencida":

                    fila.DefaultCellStyle.BackColor =
                        VencidaFondo;

                    fila.DefaultCellStyle.ForeColor =
                        VencidaTexto;

                    fila.DefaultCellStyle.SelectionBackColor =
                        VencidaSeleccion;

                    fila.DefaultCellStyle.SelectionForeColor =
                        Color.White;

                    break;

                case "Sin certificar":

                    fila.DefaultCellStyle.BackColor =
                        SinCertificarFondo;

                    fila.DefaultCellStyle.ForeColor =
                        AppColors.TextPrimary;

                    fila.DefaultCellStyle.SelectionBackColor =
                        SinCertificarSeleccion;

                    fila.DefaultCellStyle.SelectionForeColor =
                        Color.White;

                    break;
            }
        }

        // =========================================================
        // RESUMEN DE ESTADOS
        // =========================================================

        private void ActualizarResumenEstados()
        {
            int total =
                dgvTrabajadores.Rows.Count;

            int vigentes =
                0;

            int porVencer =
                0;

            int vencidas =
                0;

            int sinCertificar =
                0;

            foreach (DataGridViewRow fila in
                dgvTrabajadores.Rows)
            {
                if (fila.IsNewRow)
                {
                    continue;
                }

                if (!dgvTrabajadores.Columns.Contains(
                        "EstadoCertificacion"))
                {
                    continue;
                }

                string estado =
                    fila.Cells["EstadoCertificacion"]
                        .Value?
                        .ToString()
                    ?? string.Empty;

                switch (estado)
                {
                    case "Vigente":

                        vigentes++;

                        break;

                    case "Por vencer":

                        porVencer++;

                        break;

                    case "Vencida":

                        vencidas++;

                        break;

                    case "Sin certificar":

                        sinCertificar++;

                        break;
                }
            }

            lblResumenEstados.Text =
                $"Total: {total}   |   " +
                $"Vigentes: {vigentes}   |   " +
                $"Por vencer: {porVencer}   |   " +
                $"Vencidas: {vencidas}   |   " +
                $"Sin certificar: {sinCertificar}";
        }

        // =========================================================
        // CAMBIO DE FILTRO
        // =========================================================

        private void cbMostrarPor_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            CargarTrabajadores();
        }

        // =========================================================
        // BÚSQUEDA
        // =========================================================

        private void EjecutarBusqueda()
        {
            CargarTrabajadores();
        }

        private void btnBuscar_Click(
            object sender,
            EventArgs e)
        {
            EjecutarBusqueda();
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

            EjecutarBusqueda();
        }

        private void txtBuscar_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode !=
                Keys.Escape)
            {
                return;
            }

            RestablecerBusqueda();

            e.Handled =
                true;

            e.SuppressKeyPress =
                true;
        }

        // =========================================================
        // LIMPIAR BÚSQUEDA
        // =========================================================

        private void btnLimpiarBusqueda_Click(
            object sender,
            EventArgs e)
        {
            RestablecerBusqueda();
        }

        private void RestablecerBusqueda()
        {
            txtBuscar.Clear();

            cbMostrarPor.SelectedIndexChanged -=
                cbMostrarPor_SelectedIndexChanged;

            if (cbMostrarPor.Items.Contains(
                    "Todas"))
            {
                cbMostrarPor.SelectedItem =
                    "Todas";
            }
            else if (cbMostrarPor.Items.Count > 0)
            {
                cbMostrarPor.SelectedIndex =
                    0;
            }

            cbMostrarPor.SelectedIndexChanged +=
                cbMostrarPor_SelectedIndexChanged;

            CargarTrabajadores();
        }

        // =========================================================
        // CAMBIO DE SELECCIÓN
        // =========================================================

        private void dgvTrabajadores_SelectionChanged(
            object sender,
            EventArgs e)
        {
            SincronizarSeleccionUI();
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

            DataGridViewRow fila =
                dgvTrabajadores.Rows[
                    e.RowIndex];

            fila.Selected =
                true;

            DataGridViewCell? primeraCeldaVisible =
                fila.Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            if (primeraCeldaVisible is not null)
            {
                dgvTrabajadores.CurrentCell =
                    primeraCeldaVisible;
            }

            SincronizarSeleccionUI();

            AbrirCertificaciones();
        }

        // =========================================================
        // ABRIR CERTIFICACIONES
        // =========================================================

        private void btnCertificaciones_Click(
            object sender,
            EventArgs e)
        {
            AbrirCertificaciones();
        }

        private void AbrirCertificaciones()
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

            if (!TryObtenerNoReloj(
                    trabajador,
                    out string noReloj))
            {
                MessageBox.Show(
                    "El trabajador seleccionado no tiene un número de reloj válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using CertificacionesVentana ventana =
                new CertificacionesVentana(
                    trabajador);

            ventana.ShowDialog(
                this);

            CargarTrabajadores(
                noReloj);
        }

        // =========================================================
        // AGREGAR TRABAJADOR
        // =========================================================

        private void btnAgregarTrabajador_Click(
            object sender,
            EventArgs e)
        {
            using TrabajadoresVentana ventana =
                new TrabajadoresVentana(
                    null!);

            if (ventana.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            string noRelojNuevo =
                ventana.NoRelojGuardado;

            if (string.IsNullOrWhiteSpace(
                    noRelojNuevo))
            {
                MessageBox.Show(
                    "El trabajador se guardó, pero no se recibió el No. Reloj " +
                    "para seleccionarlo automáticamente.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                CargarTrabajadores();

                return;
            }

            /*
             * El trabajador recién agregado podría quedar
             * fuera de los primeros 300 registros.
             *
             * Por eso lo buscamos directamente por
             * su número de reloj.
             */

            txtBuscar.Text =
                noRelojNuevo;

            cbMostrarPor.SelectedIndexChanged -=
                cbMostrarPor_SelectedIndexChanged;

            if (cbMostrarPor.Items.Contains(
                    "Todas"))
            {
                cbMostrarPor.SelectedItem =
                    "Todas";
            }
            else if (cbMostrarPor.Items.Count > 0)
            {
                cbMostrarPor.SelectedIndex =
                    0;
            }

            cbMostrarPor.SelectedIndexChanged +=
                cbMostrarPor_SelectedIndexChanged;

            CargarTrabajadores(
                noRelojNuevo);
        }

        // =========================================================
        // MODIFICAR TRABAJADOR
        // =========================================================

        private void btnModificarTrabajador_Click(
            object sender,
            EventArgs e)
        {
            Clases.Trabajador? trabajador =
                ObtenerTrabajadorSeleccionado();

            if (trabajador is null)
            {
                MessageBox.Show(
                    "Seleccione un trabajador para modificar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using TrabajadoresVentana ventana =
                new TrabajadoresVentana(
                    trabajador);

            if (ventana.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            if (TryObtenerNoReloj(
                    trabajador,
                    out string noReloj))
            {
                CargarTrabajadores(
                    noReloj);
            }
            else
            {
                CargarTrabajadores();
            }
        }

        // =========================================================
        // ELIMINAR TRABAJADOR
        // =========================================================

        private void btnBorrarTrabajador_Click(
    object sender,
    EventArgs e)
        {
            Clases.Trabajador? trabajador =
                ObtenerTrabajadorSeleccionado();

            if (trabajador is null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Aviso",
                    "Seleccione un trabajador para borrar.");

                return;
            }

            if (!TryObtenerNoReloj(
                    trabajador,
                    out string noReloj))
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "El trabajador seleccionado no tiene un número de reloj válido.");

                return;
            }

            bool confirmar =
                AppDialog.Confirm(
                    this,
                    "Confirmar eliminación",
                    "¿Está seguro de borrar al trabajador?\n\n" +
                    $"No. Reloj: {noReloj}\n" +
                    $"Nombre: {trabajador.Nombre}",
                    "Eliminar");

            if (!confirmar)
            {
                return;
            }

            Clases.Mensaje respuesta =
                Clases.Trabajador
                    .BorrarTrabajador(
                        trabajador.Id);

            if (respuesta.Id != 1)
            {
                AppDialog.ShowError(
                    this,
                    "Error",
                    respuesta.Nombre);

                return;
            }

            // =====================================================
            // RESPALDO DE CARPETA FÍSICA
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
                    AppDialog.ShowInfo(
                        this,
                        "Respaldo generado",
                        "La carpeta física del trabajador se movió a respaldo:\n\n" +
                        carpetaRespaldo);
                }
            }
            catch (Exception ex)
            {
                AppDialog.ShowWarning(
                    this,
                    "Advertencia",
                    "El trabajador fue eliminado de la base de datos, " +
                    "pero no fue posible mover su carpeta al respaldo.\n\n" +
                    ex.Message);
            }

            AppDialog.ShowInfo(
                this,
                "Operación completada",
                respuesta.Nombre);

            CargarTrabajadores();
        }

        // =========================================================
        // VALIDAR NO. RELOJ
        // =========================================================

        private static bool TryObtenerNoReloj(
            Clases.Trabajador trabajador,
            out string noReloj)
        {
            noReloj =
                trabajador.NoReloj?
                    .Trim()
                ?? string.Empty;

            return noReloj.Length > 0;
        }

        // =========================================================
        // VISTA PREVIA DE CREDENCIAL
        // =========================================================

        private void btnVistaPreviaCredencial_Click(
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

            if (!TryObtenerNoReloj(
                    trabajador,
                    out string noReloj))
            {
                MessageBox.Show(
                    "El trabajador seleccionado no tiene un número de reloj válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using Verificaciones ventana =
                new Verificaciones(
                    noReloj);

            ventana.ShowDialog(
                this);
        }

        // =========================================================
        // IMPRIMIR CREDENCIAL
        // =========================================================

        private void btnImprimirCredencial_Click(
    object sender,
    EventArgs e)
        {
            Clases.Trabajador? trabajador =
                ObtenerTrabajadorSeleccionado();

            if (trabajador is null)
            {
                AppDialog.ShowInfo(
                    this,
                    "Aviso",
                    "Seleccione un trabajador.");

                return;
            }

            if (!TryObtenerNoReloj(
                    trabajador,
                    out string noReloj))
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "El trabajador seleccionado no tiene un número de reloj válido.");

                return;
            }

            using Verificaciones ventana =
                new Verificaciones(
                    noReloj,
                    imprimirAutomaticamente: true,
                    ocultarVentanaAlImprimir: true,
                    guardarDirectoEnDescargas: true);

            ventana.ShowDialog(
                this);

            if (!string.IsNullOrWhiteSpace(
                    ventana.RutaPdfGenerado))
            {
                AppDialog.ShowInfo(
                    this,
                    "Credencial generada",
                    "La credencial se generó correctamente.\n\n" +
                    $"Archivo:\n{ventana.RutaPdfGenerado}");

                return;
            }

            if (!string.IsNullOrWhiteSpace(
                    ventana.ErrorGeneracionPdf))
            {
                AppDialog.ShowWarning(
                    this,
                    "No fue posible generar la credencial",
                    ventana.ErrorGeneracionPdf);
            }
        }

        // =========================================================
        // CLIC DERECHO SOBRE DATAGRIDVIEW
        // =========================================================

        private void dgvTrabajadores_MouseDown(
            object sender,
            MouseEventArgs e)
        {
            if (e.Button !=
                MouseButtons.Right)
            {
                return;
            }

            DataGridView.HitTestInfo hit =
                dgvTrabajadores.HitTest(
                    e.X,
                    e.Y);

            if (hit.RowIndex < 0)
            {
                return;
            }

            dgvTrabajadores.ClearSelection();

            DataGridViewRow fila =
                dgvTrabajadores.Rows[
                    hit.RowIndex];

            fila.Selected =
                true;

            DataGridViewCell? primeraCeldaVisible =
                fila.Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            if (primeraCeldaVisible is not null)
            {
                dgvTrabajadores.CurrentCell =
                    primeraCeldaVisible;
            }

            SincronizarSeleccionUI();
        }

        // =========================================================
        // MENÚ CONTEXTUAL
        // =========================================================

        private void cmsTrabajadores_Opening(
            object sender,
            CancelEventArgs e)
        {
            bool haySeleccion =
                ObtenerFilaSeleccionada()
                is not null;

            tsmVerCertificaciones.Enabled =
                haySeleccion;

            tsmVistaPreviaCredencial.Enabled =
                haySeleccion;

            tsmImprimirCredencial.Enabled =
                haySeleccion;

            tsmModificarTrabajador.Enabled =
                haySeleccion;
        }

        private void tsmVerCertificaciones_Click(
            object sender,
            EventArgs e)
        {
            AbrirCertificaciones();
        }

        private void tsmVistaPreviaCredencial_Click(
            object sender,
            EventArgs e)
        {
            btnVistaPreviaCredencial_Click(
                sender,
                e);
        }

        private void tsmImprimirCredencial_Click(
            object sender,
            EventArgs e)
        {
            btnImprimirCredencial_Click(
                sender,
                e);
        }

        private void tsmModificarTrabajador_Click(
            object sender,
            EventArgs e)
        {
            btnModificarTrabajador_Click(
                sender,
                e);
        }

        // =========================================================
        // EVENTOS DEL DATAGRIDVIEW
        // =========================================================

        private void dgvTrabajadores_CellMouseEnter(
            object sender,
            DataGridViewCellEventArgs e)
        {
            /*
             * Handler conservado porque el Designer
             * actualmente lo tiene registrado.
             */
        }

        private void dgvTrabajadores_CellMouseLeave(
            object sender,
            DataGridViewCellEventArgs e)
        {
            /*
             * Handler conservado porque el Designer
             * actualmente lo tiene registrado.
             */
        }

        // =========================================================
        // TOOLTIPS
        // =========================================================

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(
                cbMostrarPor,
                "Filtra los trabajadores por estado de certificación.");

            ttAyuda.SetToolTip(
                txtBuscar,
                "Buscar por No. Reloj, nombre, localidad, turno, planta o línea.");

            ttAyuda.SetToolTip(
                btnBuscar,
                "Realiza la búsqueda capturada.");

            ttAyuda.SetToolTip(
                btnLimpiarBusqueda,
                "Limpia la búsqueda y restablece el filtro.");

            ttAyuda.SetToolTip(
                dgvTrabajadores,
                "Doble clic sobre un trabajador para abrir sus certificaciones.");

            ttAyuda.SetToolTip(
                btnAgregarTrabajador,
                "Registra un nuevo trabajador.");

            ttAyuda.SetToolTip(
                btnModificarTrabajador,
                "Modifica el trabajador seleccionado.");

            ttAyuda.SetToolTip(
                btnBorrarTrabajador,
                "Elimina el trabajador seleccionado.");

            ttAyuda.SetToolTip(
                btnCertificaciones,
                "Abre las certificaciones del trabajador seleccionado.");

            ttAyuda.SetToolTip(
                btnVistaPreviaCredencial,
                "Abre la vista previa de la credencial.");

            ttAyuda.SetToolTip(
                btnImprimirCredencial,
                "Genera e imprime la credencial.");

            ttAyuda.SetToolTip(
                lblResumenEstados,
                "Resumen de estados correspondiente a los trabajadores mostrados.");

            ttAyuda.SetToolTip(
                lblTrabajadorSeleccionado,
                "Trabajador seleccionado actualmente.");

            ttAyuda.SetToolTip(
                lblAvisoLimite,
                "Indica la cantidad de registros mostrados y si se alcanzó el límite.");

            ttAyuda.SetToolTip(
                btnRegresar,
                "Cierra esta ventana.");
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
    }
}