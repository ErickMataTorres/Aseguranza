using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Localidades : Form
    {
        // =========================================================
        // PALETA VISUAL
        // =========================================================

        private static readonly Color AzulPrincipal =
            Color.FromArgb(36, 63, 149);       // #243F95

        private static readonly Color AzulSecundario =
            Color.FromArgb(59, 89, 152);

        private static readonly Color FondoAplicacion =
            Color.FromArgb(245, 247, 250);

        private static readonly Color FondoFilaAlterna =
            Color.FromArgb(248, 250, 252);

        private static readonly Color TextoPrincipal =
            Color.FromArgb(31, 41, 55);

        private static readonly Color TextoSecundario =
            Color.FromArgb(100, 116, 139);

        private static readonly Color BordeSuave =
            Color.FromArgb(226, 232, 240);

        private static readonly Color RojoEliminar =
            Color.FromArgb(220, 53, 69);

        private static readonly Color GrisBoton =
            Color.FromArgb(75, 85, 99);

        private Label? _lblRegistros;

        private bool _estiloAplicado;


        private static readonly string FuenteIconos =
    ObtenerFuenteIconos();

        private const string IconoBuscar = "\uE721";
        private const string IconoAgregar = "\uE710";
        private const string IconoModificar = "\uE70F";
        private const string IconoEliminar = "\uE74D";
        private const string IconoRegresar = "\uE72B";

        private Panel? _pnlBuscar;
        private bool _buscarTieneFoco;
        public Localidades()
        {
            InitializeComponent();

            StartPosition =
                FormStartPosition.CenterParent;

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

            _estiloAplicado = true;

            SuspendLayout();

            // -----------------------------------------------------
            // FORMULARIO
            // -----------------------------------------------------

            Text = "Localidades";

            ClientSize = new Size(920, 560);

            BackColor =
                FondoAplicacion;

            DoubleBuffered = true;

            Font =
                new Font(
                    "Arial Nova Light",
                    10F,
                    FontStyle.Regular);

            FormBorderStyle =
                FormBorderStyle.FixedSingle;

            MaximizeBox = false;
            MinimizeBox = false;

            // -----------------------------------------------------
            // CABECERA PRINCIPAL
            // -----------------------------------------------------

            Panel pnlCabecera =
                new Panel
                {
                    Name = "pnlCabeceraVisual",
                    Location = new Point(0, 0),
                    Size = new Size(
                        ClientSize.Width,
                        88),
                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right,
                    BackColor =
                        AzulPrincipal
                };

            Label lblTitulo =
                new Label
                {
                    AutoSize = true,
                    Location =
                        new Point(32, 14),

                    Text =
                        "Localidades",

                    ForeColor =
                        Color.White,

                    Font =
                        new Font(
                            "Arial Nova",
                            19F,
                            FontStyle.Bold)
                };

            Label lblSubtitulo =
                new Label
                {
                    AutoSize = true,
                    Location =
                        new Point(34, 51),

                    Text =
                        "Administra el catálogo de localidades del sistema",

                    ForeColor =
                        Color.FromArgb(
                            225,
                            231,
                            255),

                    Font =
                        new Font(
                            "Arial Nova Light",
                            10F,
                            FontStyle.Regular)
                };

            pnlCabecera.Controls.Add(
                lblTitulo);

            pnlCabecera.Controls.Add(
                lblSubtitulo);

            Controls.Add(
                pnlCabecera);

            // -----------------------------------------------------
            // TARJETA DE CONTENIDO
            // -----------------------------------------------------

            Panel pnlContenido =
                new Panel
                {
                    Name = "pnlContenido",

                    Location =
                        new Point(20, 105),

                    Size =
                        new Size(
                            ClientSize.Width - 40,
                            ClientSize.Height - 125),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            // Borde gris suave de la tarjeta.


            Controls.Add(
                pnlContenido);

            AplicarBordeRedondeado(
    pnlContenido,
    12);

            // -----------------------------------------------------
            // ETIQUETA BUSCAR
            // -----------------------------------------------------

            lblBuscar.Text =
                "Buscar localidad";

            lblBuscar.AutoSize =
                true;

            lblBuscar.Location =
                new Point(20, 17);

            lblBuscar.ForeColor =
                TextoPrincipal;

            lblBuscar.Font =
                new Font(
                    "Arial Nova",
                    10F,
                    FontStyle.Bold);

            // -----------------------------------------------------
            // CONTENEDOR DEL BUSCADOR
            // -----------------------------------------------------

            Panel pnlBuscar =
                new Panel
                {
                    Name =
                        "pnlBuscar",

                    Location =
                        new Point(20, 42),

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

            // Guardamos la referencia para poder
            // cambiar el borde cuando recibe foco.
            _pnlBuscar =
                pnlBuscar;

            // Forma redondeada del buscador.
            AplicarBordeRedondeado(
                pnlBuscar,
                7);

            // El borde se dibuja con nuestro método,
            // que cambiará de gris a azul cuando tenga foco.
            pnlBuscar.Paint +=
                DibujarBordeBuscador;

            // -----------------------------------------------------
            // ICONO BUSCAR
            // -----------------------------------------------------

            Label lblIconoBuscar =
                new Label
                {
                    Name =
                        "lblIconoBuscar",

                    Text =
                        IconoBuscar,

                    AutoSize =
                        false,

                    Size =
                        new Size(36, 36),

                    Location =
                        new Point(2, 1),

                    TextAlign =
                        ContentAlignment.MiddleCenter,

                    ForeColor =
                        TextoSecundario,

                    Font =
                        new Font(
                            FuenteIconos,
                            14F,
                            FontStyle.Regular)
                };

            // -----------------------------------------------------
            // TEXTBOX BUSCAR
            // -----------------------------------------------------

            txtBuscar.Location =
                new Point(39, 8);

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
                TextoPrincipal;

            txtBuscar.Font =
                new Font(
                    "Arial Nova Light",
                    11F,
                    FontStyle.Regular);

            txtBuscar.PlaceholderText =
                "Escribe el nombre de la localidad...";

            txtBuscar.Enter +=
    Buscador_Enter;

            txtBuscar.Leave +=
                Buscador_Leave;

            pnlBuscar.Click +=
                (_, _) =>
                {
                    txtBuscar.Focus();
                };

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
            // CONTADOR DE REGISTROS
            // -----------------------------------------------------

            _lblRegistros =
                new Label
                {
                    Name =
                        "lblRegistros",

                    AutoSize =
                        true,

                    Location =
                        new Point(20, 92),

                    Text =
                        "0 localidades registradas",

                    ForeColor =
                        TextoSecundario,

                    Font =
                        new Font(
                            "Arial Nova Light",
                            9.5F,
                            FontStyle.Regular)
                };

            // -----------------------------------------------------
            // DATAGRIDVIEW
            // -----------------------------------------------------

            ConfigurarDataGridView();

            // -----------------------------------------------------
            // CONTENEDOR DE LA TABLA
            // -----------------------------------------------------

            Panel pnlTabla =
                new Panel
                {
                    Name =
                        "pnlTabla",

                    Location =
                        new Point(20, 118),

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

            AplicarBordeRedondeado(
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
                        CrearRutaRedondeada(
                            rectangulo,
                            6);

                    using Pen lapiz =
                        new Pen(
                            Color.FromArgb(
                                203,
                                213,
                                225),
                            1F);

                    e.Graphics.DrawPath(
                        lapiz,
                        ruta);
                };


            dgvLocalidades.Location =
    new Point(1, 1);

            dgvLocalidades.Size =
                new Size(
                    pnlTabla.ClientSize.Width - 2,
                    pnlTabla.ClientSize.Height - 2);

            dgvLocalidades.Dock =
                DockStyle.Fill;

            dgvLocalidades.BorderStyle =
                BorderStyle.None;

            pnlTabla.Controls.Add(
                dgvLocalidades);


            // -----------------------------------------------------
            // BOTONES
            // -----------------------------------------------------

            ConfigurarBoton(
                btnAgregar,
                "Agregar",
                AzulPrincipal,
                IconoAgregar);

            ConfigurarBoton(
                btnModificar,
                "Modificar",
                AzulSecundario,
                IconoModificar);

            ConfigurarBoton(
                btnBorrar,
                "Eliminar",
                RojoEliminar,
                IconoEliminar);

            ConfigurarBoton(
                btnRegresar,
                "Regresar",
                GrisBoton,
                IconoRegresar);

            int yBotones =
                pnlContenido.ClientSize.Height - 58;

            btnAgregar.Location =
                new Point(20, yBotones);

            btnModificar.Location =
                new Point(170, yBotones);

            btnBorrar.Location =
                new Point(320, yBotones);

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
            // REUBICAR CONTROLES DENTRO DE LA TARJETA
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


        private static void AplicarBordeRedondeado(
    Control control,
    int radio)
        {
            void ActualizarRegion()
            {
                if (control.Width <= 0 ||
                    control.Height <= 0)
                {
                    return;
                }

                int diametro = radio * 2;

                using GraphicsPath ruta =
                    new GraphicsPath();

                ruta.StartFigure();

                ruta.AddArc(
                    0,
                    0,
                    diametro,
                    diametro,
                    180,
                    90);

                ruta.AddArc(
                    control.Width - diametro,
                    0,
                    diametro,
                    diametro,
                    270,
                    90);

                ruta.AddArc(
                    control.Width - diametro,
                    control.Height - diametro,
                    diametro,
                    diametro,
                    0,
                    90);

                ruta.AddArc(
                    0,
                    control.Height - diametro,
                    diametro,
                    diametro,
                    90,
                    90);

                ruta.CloseFigure();

                control.Region =
                    new Region(ruta);
            }

            ActualizarRegion();

            control.Resize +=
                (_, _) =>
                {
                    ActualizarRegion();
                };
        }


        // =========================================================
        // ESTILO DEL DATAGRIDVIEW
        // =========================================================

        private void ConfigurarDataGridView()
        {
            dgvLocalidades.BackgroundColor =
                Color.White;

            dgvLocalidades.BorderStyle =
                BorderStyle.None;

            dgvLocalidades.GridColor =
                BordeSuave;

            dgvLocalidades.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dgvLocalidades.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            dgvLocalidades.RowHeadersVisible =
                false;

            dgvLocalidades.AllowUserToAddRows =
                false;

            dgvLocalidades.AllowUserToDeleteRows =
                false;

            dgvLocalidades.AllowUserToResizeRows =
                false;

            dgvLocalidades.MultiSelect =
                false;

            dgvLocalidades.ReadOnly =
                true;

            dgvLocalidades.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvLocalidades.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // -----------------------------------------------------
            // ENCABEZADO
            // -----------------------------------------------------

            dgvLocalidades.EnableHeadersVisualStyles =
                false;

            dgvLocalidades.ColumnHeadersHeight =
                40;

            dgvLocalidades.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .BackColor =
                    AzulPrincipal;

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .ForeColor =
                    Color.White;

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .SelectionBackColor =
                    AzulPrincipal;

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .SelectionForeColor =
                    Color.White;

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .Font =
                    new Font(
                        "Arial Nova",
                        10F,
                        FontStyle.Bold);

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .Alignment =
                    DataGridViewContentAlignment.MiddleLeft;

            dgvLocalidades
                .ColumnHeadersDefaultCellStyle
                .Padding =
                    new Padding(
                        8,
                        0,
                        0,
                        0);

            // -----------------------------------------------------
            // FILAS
            // -----------------------------------------------------

            dgvLocalidades
                .DefaultCellStyle
                .BackColor =
                    Color.White;

            dgvLocalidades
                .DefaultCellStyle
                .ForeColor =
                    TextoPrincipal;

            dgvLocalidades
                .DefaultCellStyle
                .Font =
                    new Font(
                        "Arial Nova Light",
                        10F,
                        FontStyle.Regular);

            dgvLocalidades
                .DefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(
                        224,
                        231,
                        255);

            dgvLocalidades
                .DefaultCellStyle
                .SelectionForeColor =
                    TextoPrincipal;

            dgvLocalidades
                .DefaultCellStyle
                .Padding =
                    new Padding(
                        8,
                        0,
                        8,
                        0);

            dgvLocalidades
                .AlternatingRowsDefaultCellStyle
                .BackColor =
                    FondoFilaAlterna;

            dgvLocalidades.RowTemplate.Height =
                38;
        }

        // =========================================================
        // ESTILO DE BOTONES
        // =========================================================

        private void ConfigurarBoton(
    Button boton,
    string texto,
    Color colorFondo,
    string icono)
        {
            boton.Text =
                texto;

            boton.Size =
                new Size(138, 42);

            boton.FlatStyle =
                FlatStyle.Flat;

            boton.FlatAppearance.BorderSize =
                0;

            boton.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(
                    colorFondo,
                    0.05F);

            boton.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(
                    colorFondo,
                    0.10F);

            boton.BackColor =
                colorFondo;

            boton.ForeColor =
                Color.White;

            boton.UseVisualStyleBackColor =
                false;

            boton.Cursor =
                Cursors.Hand;

            boton.Font =
                new Font(
                    "Arial Nova",
                    10F,
                    FontStyle.Bold);

            boton.Image =
                CrearIcono(
                    icono,
                    Color.White,
                    16);

            boton.ImageAlign =
                ContentAlignment.MiddleLeft;

            boton.TextAlign =
                ContentAlignment.MiddleCenter;

            boton.TextImageRelation =
                TextImageRelation.ImageBeforeText;

            boton.Padding =
                new Padding(
                    12,
                    0,
                    12,
                    0);

            AplicarBordeRedondeado(
                boton,
                7);
        }


        private void ActualizarEstadoBoton(
    Button boton,
    Color colorActivo)
        {
            if (boton.Enabled)
            {
                boton.BackColor =
                    colorActivo;

                boton.ForeColor =
                    Color.White;

                return;
            }

            boton.BackColor =
                Color.FromArgb(
                    203,
                    213,
                    225);

            boton.ForeColor =
                Color.FromArgb(
                    100,
                    116,
                    139);
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
                    .Selected = true;

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
            }

            ValidarBotones();
        }

        private void ConfigurarColumnas()
        {
            if (dgvLocalidades.Columns.Contains(
                "Id"))
            {
                dgvLocalidades.Columns["Id"]
                    .Visible = false;
            }

            if (dgvLocalidades.Columns.Contains(
                "Nombre"))
            {
                dgvLocalidades.Columns["Nombre"]
                    .HeaderText =
                        "LOCALIDAD";

                dgvLocalidades.Columns["Nombre"]
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
                        "No hay localidades registradas",

                    1 =>
                        "1 localidad registrada",

                    _ =>
                        $"{cantidad} localidades registradas"
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

            ActualizarEstadoBoton(
                btnModificar,
                AzulSecundario);

            ActualizarEstadoBoton(
                btnBorrar,
                RojoEliminar);

            txtBuscar.Focus();
        }

        // =========================================================
        // OBTENER LOCALIDAD SELECCIONADA
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
            using Ventanas.LocalidadesVentana ventana =
                new Ventanas.LocalidadesVentana(
                    null!);

            if (ventana.ShowDialog() ==
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
                MessageBox.Show(
                    "Seleccione una localidad.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using Ventanas.LocalidadesVentana ventana =
                new Ventanas.LocalidadesVentana(
                    localidad);

            if (ventana.ShowDialog() ==
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
                MessageBox.Show(
                    "Seleccione una localidad.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult confirmacion =
                MessageBox.Show(
                    $"¿Está seguro de eliminar la localidad " +
                    $"\"{localidad.Nombre}\"?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirmacion !=
                DialogResult.Yes)
            {
                return;
            }

            Clases.Mensaje respuesta =
                Clases.Localidad
                    .BorrarLocalidad(
                        localidad.Id);

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
            // Por ahora la búsqueda se ejecuta
            // al presionar Enter.
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

            e.Handled = true;

            IniciarTodo();
        }

        // =========================================================
        // DOBLE CLIC EN LA TABLA
        // =========================================================

        private void dgvLocalidades_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            dgvLocalidades.CurrentCell =
                dgvLocalidades.Rows[e.RowIndex]
                    .Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(
                        celda =>
                            celda.Visible);

            Clases.Localidad? localidad =
                ObtenerLocalidadSeleccionada();

            if (localidad is null)
            {
                return;
            }

            using Ventanas.LocalidadesVentana ventana =
                new Ventanas.LocalidadesVentana(
                    localidad);

            if (ventana.ShowDialog() ==
                DialogResult.OK)
            {
                IniciarTodo();
            }
        }

        private static string ObtenerFuenteIconos()
        {
            using InstalledFontCollection fuentes =
                new InstalledFontCollection();

            bool existeFluent =
                fuentes.Families.Any(
                    fuente =>
                        fuente.Name.Equals(
                            "Segoe Fluent Icons",
                            StringComparison.OrdinalIgnoreCase));

            return existeFluent
                ? "Segoe Fluent Icons"
                : "Segoe MDL2 Assets";
        }

        private static Bitmap CrearIcono(
            string glifo,
            Color color,
            int tamano = 16)
        {
            Bitmap bitmap =
                new Bitmap(24, 24);

            using Graphics graphics =
                Graphics.FromImage(bitmap);

            graphics.Clear(
                Color.Transparent);

            graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            graphics.TextRenderingHint =
                TextRenderingHint.AntiAliasGridFit;

            using Font fuente =
                new Font(
                    FuenteIconos,
                    tamano,
                    FontStyle.Regular,
                    GraphicsUnit.Pixel);

            TextRenderer.DrawText(
                graphics,
                glifo,
                fuente,
                new Rectangle(
                    0,
                    0,
                    bitmap.Width,
                    bitmap.Height),
                color,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding);

            return bitmap;
        }


        private GraphicsPath CrearRutaRedondeada(
    Rectangle rectangulo,
    int radio)
        {
            GraphicsPath ruta =
                new GraphicsPath();

            int diametro =
                radio * 2;

            Rectangle arco =
                new Rectangle(
                    rectangulo.Location,
                    new Size(
                        diametro,
                        diametro));

            ruta.AddArc(
                arco,
                180,
                90);

            arco.X =
                rectangulo.Right -
                diametro;

            ruta.AddArc(
                arco,
                270,
                90);

            arco.Y =
                rectangulo.Bottom -
                diametro;

            ruta.AddArc(
                arco,
                0,
                90);

            arco.X =
                rectangulo.Left;

            ruta.AddArc(
                arco,
                90,
                90);

            ruta.CloseFigure();

            return ruta;
        }

        private void DibujarBordeBuscador(
            object? sender,
            PaintEventArgs e)
        {
            if (_pnlBuscar is null)
            {
                return;
            }

            e.Graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            Rectangle rectangulo =
                new Rectangle(
                    0,
                    0,
                    _pnlBuscar.Width - 1,
                    _pnlBuscar.Height - 1);

            using GraphicsPath ruta =
                CrearRutaRedondeada(
                    rectangulo,
                    7);

            Color colorBorde =
                _buscarTieneFoco
                    ? AzulPrincipal
                    : Color.FromArgb(
                        148,
                        163,
                        184);

            float grosor =
                _buscarTieneFoco
                    ? 1.8F
                    : 1F;

            using Pen lapiz =
                new Pen(
                    colorBorde,
                    grosor);

            e.Graphics.DrawPath(
                lapiz,
                ruta);
        }

        private void Buscador_Enter(
            object? sender,
            EventArgs e)
        {
            _buscarTieneFoco = true;

            _pnlBuscar?.Invalidate();
        }

        private void Buscador_Leave(
            object? sender,
            EventArgs e)
        {
            _buscarTieneFoco = false;

            _pnlBuscar?.Invalidate();
        }


    }
}