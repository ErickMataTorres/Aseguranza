using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class LocalidadesVentana : Form
    {
        // =========================================================
        // DATOS
        // =========================================================

        private Clases.Localidad? localidadActual;

        // =========================================================
        // PALETA
        // =========================================================

        private static readonly Color AzulPrincipal =
            Color.FromArgb(36, 63, 149);

        private static readonly Color FondoAplicacion =
            Color.FromArgb(245, 247, 250);

        private static readonly Color TextoPrincipal =
            Color.FromArgb(31, 41, 55);

        private static readonly Color TextoSecundario =
            Color.FromArgb(100, 116, 139);

        private static readonly Color BordeNormal =
            Color.FromArgb(148, 163, 184);

        private static readonly Color GrisBoton =
            Color.FromArgb(75, 85, 99);

        // =========================================================
        // ICONOS
        // =========================================================

        private static readonly string FuenteIconos =
            ObtenerFuenteIconos();

        private const string IconoGuardar =
            "\uE74E";

        private const string IconoRegresar =
            "\uE72B";

        // =========================================================
        // ESTADO VISUAL
        // =========================================================

        private bool _estiloAplicado;

        private Panel? _pnlNombre;

        private bool _nombreTieneFoco;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public LocalidadesVentana(
            Clases.Localidad? localidad)
        {
            InitializeComponent();

            localidadActual =
                localidad;

            if (localidadActual is not null)
            {
                txtNombre.Text =
                    localidadActual.Nombre;
            }

            StartPosition =
                FormStartPosition.CenterParent;

            AplicarEstiloVisual();
        }

        // =========================================================
        // CARGA
        // =========================================================

        private void LocalidadesVentana_Load(
            object sender,
            EventArgs e)
        {
            txtNombre.Focus();
            txtNombre.SelectAll();
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

            _estiloAplicado = true;

            SuspendLayout();

            Text =
                localidadActual is null
                    ? "Agregar localidad"
                    : "Modificar localidad";

            ClientSize =
                new Size(620, 340);

            BackColor =
                FondoAplicacion;

            DoubleBuffered =
                true;

            Font =
                new Font(
                    "Arial Nova Light",
                    10F,
                    FontStyle.Regular);

            FormBorderStyle =
                FormBorderStyle.FixedSingle;

            MaximizeBox = false;
            MinimizeBox = false;

            AcceptButton =
                btnAceptar;

            CancelButton =
                btnRegresar;

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlCabecera =
                new Panel
                {
                    Location =
                        new Point(0, 0),

                    Size =
                        new Size(
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
                    AutoSize =
                        true,

                    Location =
                        new Point(30, 15),

                    Text =
                        localidadActual is null
                            ? "Agregar localidad"
                            : "Modificar localidad",

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
                    AutoSize =
                        true,

                    Location =
                        new Point(32, 52),

                    Text =
                        localidadActual is null
                            ? "Registra una nueva localidad en el sistema"
                            : "Actualiza la información de la localidad seleccionada",

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

            // =====================================================
            // TARJETA
            // =====================================================

            Panel pnlContenido =
                new Panel
                {
                    Location =
                        new Point(20, 106),

                    Size =
                        new Size(
                            ClientSize.Width - 40,
                            ClientSize.Height - 126),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            AplicarBordeRedondeado(
                pnlContenido,
                12);

            Controls.Add(
                pnlContenido);

            // =====================================================
            // LABEL NOMBRE
            // =====================================================

            lblNombre.Text =
                "Nombre de la localidad";

            lblNombre.AutoSize =
                true;

            lblNombre.Location =
                new Point(22, 22);

            lblNombre.ForeColor =
                TextoPrincipal;

            lblNombre.Font =
                new Font(
                    "Arial Nova",
                    10F,
                    FontStyle.Bold);

            // =====================================================
            // PANEL TEXTBOX
            // =====================================================

            Panel pnlNombre =
                new Panel
                {
                    Location =
                        new Point(22, 50),

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

            _pnlNombre =
                pnlNombre;

            AplicarBordeRedondeado(
                pnlNombre,
                7);

            pnlNombre.Paint +=
                DibujarBordeNombre;

            // =====================================================
            // TEXTBOX
            // =====================================================

            txtNombre.Location =
                new Point(12, 10);

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
                TextoPrincipal;

            txtNombre.Font =
                new Font(
                    "Arial Nova Light",
                    11F,
                    FontStyle.Regular);

            txtNombre.PlaceholderText =
                "Ej. Los Mochis";

            txtNombre.Enter +=
                TxtNombre_Enter;

            txtNombre.Leave +=
                TxtNombre_Leave;

            pnlNombre.Click +=
                (_, _) =>
                {
                    txtNombre.Focus();
                };

            pnlNombre.Controls.Add(
                txtNombre);

            // =====================================================
            // TEXTO DE AYUDA
            // =====================================================

            Label lblAyuda =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(22, 100),

                    Text =
                        "El nombre se guardará automáticamente en mayúsculas.",

                    ForeColor =
                        TextoSecundario,

                    Font =
                        new Font(
                            "Arial Nova Light",
                            9F,
                            FontStyle.Regular)
                };

            // =====================================================
            // BOTONES
            // =====================================================

            ConfigurarBoton(
                btnAceptar,
                localidadActual is null
                    ? "Guardar"
                    : "Actualizar",
                AzulPrincipal,
                IconoGuardar);

            ConfigurarBoton(
                btnRegresar,
                "Cancelar",
                GrisBoton,
                IconoRegresar);

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
            // AGREGAR A TARJETA
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
        // TEXTBOX - FOCO
        // =========================================================

        private void TxtNombre_Enter(
            object? sender,
            EventArgs e)
        {
            _nombreTieneFoco =
                true;

            _pnlNombre?.Invalidate();
        }

        private void TxtNombre_Leave(
            object? sender,
            EventArgs e)
        {
            _nombreTieneFoco =
                false;

            _pnlNombre?.Invalidate();
        }

        private void DibujarBordeNombre(
            object? sender,
            PaintEventArgs e)
        {
            if (_pnlNombre is null)
            {
                return;
            }

            e.Graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            Rectangle rectangulo =
                new Rectangle(
                    0,
                    0,
                    _pnlNombre.Width - 1,
                    _pnlNombre.Height - 1);

            using GraphicsPath ruta =
                CrearRutaRedondeada(
                    rectangulo,
                    7);

            Color colorBorde =
                _nombreTieneFoco
                    ? AzulPrincipal
                    : BordeNormal;

            float grosor =
                _nombreTieneFoco
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

        // =========================================================
        // BOTONES
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

        // =========================================================
        // GUARDAR
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
                    "El nombre de la localidad no puede estar vacío.",
                    "Dato requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNombre.Focus();

                return;
            }

            Clases.Localidad localidad =
                localidadActual ??
                new Clases.Localidad();

            localidad.Nombre =
                nombre.ToUpper();

            Clases.Mensaje respuesta =
                localidad.GuardarLocalidad();

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
        // EVENTOS EXISTENTES DEL DESIGNER
        // =========================================================

        private void btnAceptar_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
        }

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

        private void txtNombre_TextChanged(
            object sender,
            EventArgs e)
        {
        }

        // =========================================================
        // BORDES REDONDEADOS
        // =========================================================

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

                Rectangle rectangulo =
                    new Rectangle(
                        0,
                        0,
                        control.Width,
                        control.Height);

                using GraphicsPath ruta =
                    CrearRutaRedondeada(
                        rectangulo,
                        radio);

                Region? regionAnterior =
                    control.Region;

                control.Region =
                    new Region(ruta);

                regionAnterior?.Dispose();
            }

            ActualizarRegion();

            control.Resize +=
                (_, _) =>
                {
                    ActualizarRegion();
                };
        }

        private static GraphicsPath CrearRutaRedondeada(
            Rectangle rectangulo,
            int radio)
        {
            GraphicsPath ruta =
                new GraphicsPath();

            int diametro =
                radio * 2;

            Rectangle arco =
                new Rectangle(
                    rectangulo.X,
                    rectangulo.Y,
                    diametro,
                    diametro);

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

        // =========================================================
        // ICONOS
        // =========================================================

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
            int tamano)
        {
            Bitmap bitmap =
                new Bitmap(
                    24,
                    24);

            using Graphics graphics =
                Graphics.FromImage(
                    bitmap);

            graphics.Clear(
                Color.Transparent);

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
    }
}