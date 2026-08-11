using System.Linq;
using System.ComponentModel;
using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CertificacionesVentana : Form
    {
        private readonly Trabajador trabajadorActual;
        private readonly Font _boldFont = AppFonts.Regular(9.5F, FontStyle.Bold);

        private static readonly Color ColorVigente =
            Color.FromArgb(198, 239, 206);

        private static readonly Color ColorVigenteTexto =
            Color.FromArgb(0, 97, 0);

        private static readonly Color ColorVigenteSeleccion =
            Color.FromArgb(111, 193, 126);

        private static readonly Color ColorPorVencer =
            Color.FromArgb(255, 235, 156);

        private static readonly Color ColorPorVencerTexto =
            Color.FromArgb(128, 92, 0);

        private static readonly Color ColorPorVencerSeleccion =
            Color.FromArgb(236, 187, 58);

        private static readonly Color ColorVencida =
            Color.FromArgb(255, 199, 206);

        private static readonly Color ColorVencidaTexto =
            Color.FromArgb(156, 0, 6);

        private static readonly Color ColorVencidaSeleccion =
            Color.FromArgb(235, 106, 120);

        private static readonly Color ColorAnulada =
            Color.FromArgb(221, 214, 254);

        private static readonly Color ColorAnuladaTexto =
            Color.FromArgb(91, 33, 182);

        private static readonly Color ColorAnuladaSeleccion =
            Color.FromArgb(167, 139, 250);

        private static readonly Color ColorAccionAnular =
            Color.FromArgb(202, 138, 4);

        private static readonly Color ColorAccionAnulada =
            Color.FromArgb(109, 76, 176);

        public CertificacionesVentana(Trabajador trabajador)
        {
            InitializeComponent();
            trabajadorActual = trabajador;

            DoubleBuffered = true;

            AplicarEstiloVisual();
        }

        // =====================================================
        // INTERFAZ VISUAL
        // =====================================================
        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            FormStyler.ApplyBase(
                this,
                "Certificaciones del trabajador",
                new Size(
                    1180,
                    720));

            FormStyler.CreateHeader(
                this,
                "Certificaciones del trabajador",
                "Administra, renueva y consulta las certificaciones del personal",
                height: 100,
                titleX: 38,
                titleY: 18,
                subtitleX: 40,
                subtitleY: 58);

            // El encabezado antiguo del Designer se conserva,
            // pero ya no se muestra.
            pbContec.Visible = false;
            lblVerificador.Visible = false;

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        120),
                    new Size(
                        1140,
                        580),
                    radius: 14);

            // =================================================
            // SECCIÓN DEL TRABAJADOR
            // =================================================

            Panel pnlTrabajador =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            18),

                    Size =
                        new Size(
                            1100,
                            150),

                    BackColor =
                        AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlTrabajador,
                10);

            pnlContenido.Controls.Add(
                pnlTrabajador);

            Label lblTituloTrabajador =
                new Label
                {
                    AutoSize = true,
                    Text = "Información del trabajador",
                    Location = new Point(18, 14),
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(13F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            pnlTrabajador.Controls.Add(
                lblTituloTrabajador);

            ConfigurarEtiquetaCampo(
                lblNoReloj,
                "No. Reloj",
                new Point(18, 51));

            ConfigurarEtiquetaValor(
                lblMostrarNoReloj,
                new Point(95, 49),
                new Size(110, 24));

            ConfigurarEtiquetaCampo(
                lblNombre,
                "Nombre",
                new Point(225, 51));

            ConfigurarEtiquetaValor(
                lblMostrarNombre,
                new Point(286, 49),
                new Size(560, 24));

            ConfigurarEtiquetaCampo(
                lblLocalidad,
                "Localidad",
                new Point(18, 91));

            ConfigurarEtiquetaValor(
                lblMostrarLocalidad,
                new Point(95, 89),
                new Size(120, 24));

            ConfigurarEtiquetaCampo(
                lblTurno,
                "Turno",
                new Point(230, 91));

            ConfigurarEtiquetaValor(
                lblMostrarTurno,
                new Point(280, 89),
                new Size(75, 24));

            ConfigurarEtiquetaCampo(
                lblPlanta,
                "Planta",
                new Point(380, 91));

            ConfigurarEtiquetaValor(
                lblMostrarPlanta,
                new Point(430, 89),
                new Size(110, 24));

            ConfigurarEtiquetaCampo(
                lblLinea,
                "Línea",
                new Point(570, 91));

            ConfigurarEtiquetaValor(
                lblMostrarLinea,
                new Point(615, 89),
                new Size(220, 24));

            // Reubicar etiquetas existentes dentro de la sección.
            pnlTrabajador.Controls.Add(lblNoReloj);
            pnlTrabajador.Controls.Add(lblMostrarNoReloj);
            pnlTrabajador.Controls.Add(lblNombre);
            pnlTrabajador.Controls.Add(lblMostrarNombre);
            pnlTrabajador.Controls.Add(lblLocalidad);
            pnlTrabajador.Controls.Add(lblMostrarLocalidad);
            pnlTrabajador.Controls.Add(lblTurno);
            pnlTrabajador.Controls.Add(lblMostrarTurno);
            pnlTrabajador.Controls.Add(lblPlanta);
            pnlTrabajador.Controls.Add(lblMostrarPlanta);
            pnlTrabajador.Controls.Add(lblLinea);
            pnlTrabajador.Controls.Add(lblMostrarLinea);

            Panel pnlFoto =
                new Panel
                {
                    Location = new Point(855, 6),
                    Size = new Size(225, 140),
                    BackColor = Color.White
                };

            InputStyler.ApplyOutlinedInput(
                pnlFoto,
                pictureBox1,
                radius: 8,
                borderColor: AppColors.BorderMedium);

            pictureBox1.Location = new Point(5, 5);
            pictureBox1.Size = new Size(215, 130);
            pictureBox1.BackColor = Color.White;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pnlFoto.Controls.Add(
                pictureBox1);

            pnlTrabajador.Controls.Add(
                pnlFoto);

            // =================================================
            // RESUMEN Y LEYENDA
            // =================================================

            lblResumenCertificaciones.Parent =
                pnlContenido;

            lblResumenCertificaciones.Location =
                new Point(
                    20,
                    181);

            lblResumenCertificaciones.Size =
                new Size(
                    610,
                    26);

            lblResumenCertificaciones.Font =
                AppFonts.Regular(
                    9.7F,
                    FontStyle.Bold);

            lblResumenCertificaciones.ForeColor =
                AppColors.TextPrimary;

            lblResumenCertificaciones.BackColor =
                Color.Transparent;

            ConfigurarLeyendaCertificaciones(
                pnlContenido);

            // =================================================
            // BÚSQUEDA
            // =================================================

            lblBuscar.Parent =
                pnlContenido;

            lblBuscar.Text =
                "Buscar certificación";

            lblBuscar.AutoSize =
                true;

            lblBuscar.Location =
                new Point(
                    20,
                    215);

            lblBuscar.ForeColor =
                AppColors.TextPrimary;

            lblBuscar.Font =
                AppFonts.Regular(
                    9.5F,
                    FontStyle.Bold);

            Panel pnlBuscar =
                new Panel
                {
                    Location = new Point(20, 238),
                    Size = new Size(890, 40)
                };

            txtBuscar.Parent =
                pnlBuscar;

            txtBuscar.BorderStyle =
                BorderStyle.None;

            txtBuscar.Location =
                new Point(
                    12,
                    10);

            txtBuscar.Size =
                new Size(
                    864,
                    22);

            txtBuscar.Font =
                AppFonts.Light(10F);

            txtBuscar.ForeColor =
                AppColors.TextPrimary;

            txtBuscar.BackColor =
                Color.White;

            txtBuscar.PlaceholderText =
                "Proceso, fecha, comentario o certificador...";

            InputStyler.ApplyOutlinedInput(
                pnlBuscar,
                txtBuscar);

            pnlContenido.Controls.Add(
                pnlBuscar);

            AplicarBotonTexto(
                btnLimpiarBusqueda,
                "Limpiar",
                AppColors.Neutral,
                170);

            btnLimpiarBusqueda.Size =
                new Size(
                    170,
                    40);

            btnLimpiarBusqueda.Parent =
                pnlContenido;

            btnLimpiarBusqueda.Location =
                new Point(
                    930,
                    238);

            // =================================================
            // GRID
            // =================================================

            dgvCertificaciones.Parent =
                pnlContenido;

            dgvCertificaciones.Location =
                new Point(
                    20,
                    292);

            dgvCertificaciones.Size =
                new Size(
                    1100,
                    190);

            // =================================================
            // INFORMACIÓN DE ANULACIÓN
            // =================================================

            lblInfoAnulacion.Parent =
                pnlContenido;

            lblInfoAnulacion.Location =
                new Point(
                    20,
                    490);

            lblInfoAnulacion.Size =
                new Size(
                    1100,
                    32);

            lblInfoAnulacion.BackColor =
                ColorAnulada;

            lblInfoAnulacion.ForeColor =
                ColorAnuladaTexto;

            lblInfoAnulacion.Font =
                AppFonts.Regular(
                    9.2F,
                    FontStyle.Bold);

            lblInfoAnulacion.TextAlign =
                ContentAlignment.MiddleLeft;

            lblInfoAnulacion.Padding =
                new Padding(
                    10,
                    0,
                    10,
                    0);

            RoundedControlHelper.ApplyRoundedRegion(
                lblInfoAnulacion,
                6);

            // =================================================
            // BOTONES INFERIORES
            // =================================================

            ConfigurarBotonesAccion(
                pnlContenido);

            ResumeLayout(
                true);
        }

        private static void ConfigurarEtiquetaCampo(
            Label label,
            string texto,
            Point location)
        {
            label.Text = texto;
            label.AutoSize = true;
            label.Location = location;
            label.ForeColor = AppColors.TextSecondary;
            label.Font = AppFonts.Regular(10F, FontStyle.Bold);
            label.BackColor = Color.Transparent;
        }

        private static void ConfigurarEtiquetaValor(
            Label label,
            Point location,
            Size size)
        {
            label.AutoSize = false;
            label.Location = location;
            label.Size = size;
            label.ForeColor = AppColors.TextPrimary;
            label.Font = AppFonts.Regular(12F, FontStyle.Bold);
            label.BackColor = Color.Transparent;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoEllipsis = true;
        }

        private void ConfigurarLeyendaCertificaciones(
            Control parent)
        {
            pnlLeyendaCertificaciones.Parent =
                parent;

            pnlLeyendaCertificaciones.Location =
                new Point(
                    660,
                    176);

            pnlLeyendaCertificaciones.Size =
                new Size(
                    460,
                    34);

            pnlLeyendaCertificaciones.BackColor =
                Color.Transparent;

            ConfigurarItemLeyenda(
                lblColorVigente,
                lblTextoVigente,
                ColorVigente,
                "Vigente",
                0);

            ConfigurarItemLeyenda(
                lblColorPorVencer,
                lblTextoPorVencer,
                ColorPorVencer,
                "Por vencer",
                104);

            ConfigurarItemLeyenda(
                lblColorVencida,
                lblTextoVencida,
                ColorVencida,
                "Vencida",
                238);

            ConfigurarItemLeyenda(
                lblColorAnulada,
                lblTextoAnulada,
                ColorAnulada,
                "Anulada",
                347);
        }

        private static void ConfigurarItemLeyenda(
            Label indicador,
            Label texto,
            Color color,
            string descripcion,
            int x)
        {
            indicador.Location =
                new Point(
                    x,
                    9);

            indicador.Size =
                new Size(
                    15,
                    15);

            indicador.BackColor =
                color;

            indicador.Text =
                string.Empty;

            RoundedControlHelper.ApplyRoundedRegion(
                indicador,
                4);

            texto.Text =
                descripcion;

            texto.AutoSize =
                true;

            texto.Location =
                new Point(
                    x + 21,
                    8);

            texto.ForeColor =
                AppColors.TextSecondary;

            texto.Font =
                AppFonts.Light(9F);

            texto.BackColor =
                Color.Transparent;
        }

        private void ConfigurarBotonesAccion(
            Control parent)
        {
            ButtonStyler.Apply(
                btnAgregar,
                "Agregar",
                AppColors.Primary,
                AppIcons.Add,
                width: 135,
                height: 42);

            ButtonStyler.Apply(
                btnModificar,
                "Modificar / Renovar",
                AppColors.Secondary,
                AppIcons.Edit,
                width: 170,
                height: 42);

            ButtonStyler.Apply(
                btnBorrar,
                "Borrar",
                AppColors.Danger,
                AppIcons.Delete,
                width: 125,
                height: 42);

            AplicarBotonTexto(
                btnExpediente,
                "Expediente",
                AppColors.Secondary,
                145);

            AplicarBotonTexto(
                btnAnular,
                "Anular",
                ColorAccionAnular,
                145);

            AplicarBotonTexto(
                btnCredencial,
                "Credencial",
                AppColors.Primary,
                145);

            ButtonStyler.Apply(
                btnRegresar,
                "Regresar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 135,
                height: 42);

            Button[] botones =
            {
                btnAgregar,
                btnModificar,
                btnBorrar,
                btnExpediente,
                btnAnular,
                btnCredencial,
                btnRegresar
            };

            foreach (Button boton in botones)
            {
                boton.Parent = parent;
            }

            int y = 530;

            btnAgregar.Location = new Point(20, y);
            btnModificar.Location = new Point(165, y);
            btnBorrar.Location = new Point(345, y);
            btnExpediente.Location = new Point(480, y);
            btnAnular.Location = new Point(635, y);
            btnCredencial.Location = new Point(790, y);
            btnRegresar.Location = new Point(945, y);
        }

        private static void AplicarBotonTexto(
            Button button,
            string texto,
            Color color,
            int width)
        {
            button.Text = texto;
            button.Size = new Size(width, 42);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(color, 0.05F);
            button.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(color, 0.10F);
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
            button.Font = AppFonts.Regular(10F, FontStyle.Bold);
            button.Image = null;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Padding = Padding.Empty;

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                7);
        }

        private void ActualizarEstiloBotonesPorEstado()
        {
            ButtonStyler.UpdateEnabledState(
                btnModificar,
                AppColors.Secondary);

            ButtonStyler.UpdateEnabledState(
                btnBorrar,
                AppColors.Danger);

            if (btnCredencial.Enabled)
            {
                btnCredencial.BackColor =
                    AppColors.Primary;

                btnCredencial.ForeColor =
                    Color.White;
            }
            else
            {
                btnCredencial.BackColor =
                    AppColors.DisabledBackground;

                btnCredencial.ForeColor =
                    AppColors.DisabledText;
            }

            ActualizarEstadoBotonAnular();
        }

        // =====================================================
        // LOAD
        // =====================================================
        private void CertificacionesVentana_Load(object sender, EventArgs e)
        {
            dgvCertificaciones.DataBindingComplete -= dgvCertificaciones_DataBindingComplete!;
            dgvCertificaciones.DataBindingComplete += dgvCertificaciones_DataBindingComplete!;

            dgvCertificaciones.DataError -= dgvCertificaciones_DataError!;
            dgvCertificaciones.DataError += dgvCertificaciones_DataError!;

            ConfigurarGridCertificaciones();
            ConfigurarToolTips();

            RefrescarVentana();
        }
        private void dgvCertificaciones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(txtBuscar, "Buscar certificaciones por proceso, fecha, comentario o certificador.");
            ttAyuda.SetToolTip(dgvCertificaciones, "Doble clic sobre una certificación vigente, vencida o por vencer para modificar o renovar.");
            ttAyuda.SetToolTip(btnAgregar, "Agregar una nueva certificación al trabajador.");
            ttAyuda.SetToolTip(btnModificar, "Modificar o renovar la certificación seleccionada.");
            ttAyuda.SetToolTip(btnBorrar, "Eliminar la certificación seleccionada.");
            ttAyuda.SetToolTip(btnExpediente, "Abrir expediente del trabajador.");
            ttAyuda.SetToolTip(btnAnular, "Anular, ver o modificar la anulación de la certificación seleccionada.");
            ttAyuda.SetToolTip(btnCredencial, "Imprimir o generar la credencial del trabajador.");
            ttAyuda.SetToolTip(lblResumenCertificaciones, "Resumen de certificaciones del trabajador actual.");
            ttAyuda.SetToolTip(pnlLeyendaCertificaciones, "Leyenda de colores de acuerdo al estado de cada certificación.");
            ttAyuda.SetToolTip(btnLimpiarBusqueda, "Limpia la búsqueda y vuelve a mostrar todas las certificaciones del trabajador.");
        }

        private void RefrescarVentana(int idCertificacionSeleccionar = 0)
        {
            MostrarDatosTrabajador();
            CargarCertificaciones(txtBuscar.Text.Trim(), idCertificacionSeleccionar);
        }

        private void ConfigurarGridCertificaciones()
        {
            DataGridViewStyler.ApplyCatalogStyle(
                dgvCertificaciones,
                headerHeight: 38,
                rowHeight: 36);

            dgvCertificaciones.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvCertificaciones.DefaultCellStyle.SelectionForeColor =
                AppColors.TextPrimary;

            dgvCertificaciones.BackgroundColor =
                Color.White;

            dgvCertificaciones.ContextMenuStrip =
                cmsCertificaciones;

            cmsCertificaciones.Font =
                AppFonts.Light(9.5F);

            cmsCertificaciones.BackColor =
                Color.White;

            cmsCertificaciones.ForeColor =
                AppColors.TextPrimary;
        }

        private void SeleccionarPrimero()
        {
            if (dgvCertificaciones.Rows.Count == 0)
                return;

            dgvCertificaciones.ClearSelection();
            dgvCertificaciones.Rows[0].Selected = true;

            dgvCertificaciones.CurrentCell = dgvCertificaciones.Rows[0].Cells
                .Cast<DataGridViewCell>()
                .First(c => c.Visible);
        }

        private void SeleccionarCertificacionPorId(int idCertificacion)
        {
            if (idCertificacion <= 0)
            {
                SeleccionarPrimero();
                return;
            }

            if (!dgvCertificaciones.Columns.Contains("Id"))
            {
                SeleccionarPrimero();
                return;
            }

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow)
                    continue;

                object valorId = row.Cells["Id"].Value;

                if (valorId == null || valorId == DBNull.Value)
                    continue;

                int idFila = Convert.ToInt32(valorId);

                if (idFila == idCertificacion)
                {
                    dgvCertificaciones.ClearSelection();
                    row.Selected = true;

                    foreach (DataGridViewCell celda in row.Cells)
                    {
                        if (celda.Visible)
                        {
                            dgvCertificaciones.CurrentCell = celda;
                            break;
                        }
                    }

                    ActualizarInfoAnulacionSeleccionada();
                    ActualizarEstadoBotonAnular();

                    return;
                }
            }

            SeleccionarPrimero();
        }

        private Image CargarImagenSinBloquearArchivo(string ruta)
        {
            using FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            using Image imagenTemporal = Image.FromStream(fs);

            return new Bitmap(imagenTemporal);
        }

        // =====================================================
        // MOSTRAR DATOS DEL TRABAJADOR
        // =====================================================
        private void MostrarDatosTrabajador()
        {
            lblMostrarNoReloj.Text = trabajadorActual.NoReloj;
            lblMostrarNombre.Text = trabajadorActual.Nombre;
            lblMostrarLocalidad.Text = trabajadorActual.NombreLocalidad;
            lblMostrarTurno.Text = trabajadorActual.NombreTurno;
            lblMostrarPlanta.Text = trabajadorActual.NombrePlanta;
            lblMostrarLinea.Text = trabajadorActual.NombreLinea;

            // Cargar imagen de forma segura y sin bloquear archivo
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            if (!string.IsNullOrWhiteSpace(trabajadorActual.RutaFoto) &&
                File.Exists(trabajadorActual.RutaFoto))
            {
                try
                {
                    pictureBox1.Image = CargarImagenSinBloquearArchivo(trabajadorActual.RutaFoto);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch (Exception ex)
                {
                    pictureBox1.Image = null;

                    AppDialog.ShowWarning(
                        this,
                        "Foto no disponible",
                        "No se pudo cargar la foto del trabajador.\n\n" +
                        "Ruta: " + trabajadorActual.RutaFoto + "\n\n" +
                        "Detalle: " + ex.Message);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        // =====================================================
        // CARGAR CERTIFICACIONES
        // =====================================================
        private void CargarCertificaciones(string textoBuscar = "", int idCertificacionSeleccionar = 0)
        {
            dgvCertificaciones.DataSource = null;
            dgvCertificaciones.Columns.Clear();
            dgvCertificaciones.AutoGenerateColumns = true;

            dgvCertificaciones.DataSource =
                Certificacion.ConsultarCertificacionesPorTrabajador(
                    trabajadorActual.Id,
                    textoBuscar);


            OcultarColumnas();
            ConfigurarEncabezados();

            PintarCertificaciones();
            AsignarToolTipsCertificaciones();
            ActualizarResumenCertificaciones();

            if (dgvCertificaciones.Rows.Count > 0)
            {
                if (idCertificacionSeleccionar > 0)
                    SeleccionarCertificacionPorId(idCertificacionSeleccionar);
                else
                    SeleccionarPrimero();

                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
                btnCredencial.Enabled = true;
                btnAnular.Enabled = true;

                ActualizarInfoAnulacionSeleccionada();
                ActualizarEstadoBotonAnular();
            }
            else
            {
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
                btnCredencial.Enabled = false;
                btnAnular.Enabled = false;
                btnAnular.Text = "Anular";

                lblInfoAnulacion.Visible = false;
                lblInfoAnulacion.Text = "";
            }

            ActualizarEstiloBotonesPorEstado();

            txtBuscar.Focus();
        }

        private void OcultarColumnas()
        {
            string[] columnasOcultas =
            {
        "Id",
        "IdProceso",
        "IdTrabajador",
        "IdCertificador",

        "EstaAnulada",
        "IdAnulacion",
        "TipoAnulacion",
        "FechaInicioAnulacion",
        "FechaFinAnulacion",
        "EsPermanente",
        "ComentarioAnulacion"
    };

            foreach (var col in columnasOcultas)
            {
                if (dgvCertificaciones.Columns.Contains(col))
                    dgvCertificaciones.Columns[col].Visible = false;
            }
        }

        private void ConfigurarEncabezados()
        {
            if (dgvCertificaciones.Columns.Contains("Proceso"))
            {
                dgvCertificaciones.Columns["Proceso"].HeaderText = "Proceso";
                dgvCertificaciones.Columns["Proceso"].FillWeight = 110;
                dgvCertificaciones.Columns["Proceso"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvCertificaciones.Columns.Contains("FechaCertificacion"))
            {
                dgvCertificaciones.Columns["FechaCertificacion"].HeaderText = "Fecha certificación";
                dgvCertificaciones.Columns["FechaCertificacion"].FillWeight = 120;
                dgvCertificaciones.Columns["FechaCertificacion"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dgvCertificaciones.Columns["FechaCertificacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvCertificaciones.Columns.Contains("FechaVencimiento"))
            {
                dgvCertificaciones.Columns["FechaVencimiento"].HeaderText = "Fecha vencimiento";
                dgvCertificaciones.Columns["FechaVencimiento"].FillWeight = 120;
                dgvCertificaciones.Columns["FechaVencimiento"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dgvCertificaciones.Columns["FechaVencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvCertificaciones.Columns.Contains("DiasRestantes"))
            {
                dgvCertificaciones.Columns["DiasRestantes"].HeaderText = "Días restantes";
                dgvCertificaciones.Columns["DiasRestantes"].FillWeight = 90;
                dgvCertificaciones.Columns["DiasRestantes"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvCertificaciones.Columns.Contains("Comentario"))
            {
                dgvCertificaciones.Columns["Comentario"].HeaderText = "Comentario";
                dgvCertificaciones.Columns["Comentario"].FillWeight = 180;
            }

            if (dgvCertificaciones.Columns.Contains("NombreCertificador"))
            {
                dgvCertificaciones.Columns["NombreCertificador"].HeaderText = "Certificador";
                dgvCertificaciones.Columns["NombreCertificador"].FillWeight = 180;
            }
        }

        private void ActualizarResumenCertificaciones()
        {
            int total = 0;
            int vigentes = 0;
            int porVencer = 0;
            int vencidas = 0;
            int anuladas = 0;

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow)
                    continue;

                total++;

                bool estaAnulada = false;

                if (dgvCertificaciones.Columns.Contains("EstaAnulada"))
                {
                    object valorAnulada = row.Cells["EstaAnulada"].Value;

                    if (valorAnulada != null && valorAnulada != DBNull.Value)
                    {
                        string texto = valorAnulada.ToString()!.Trim().ToLower();

                        estaAnulada = texto == "1"
                            || texto == "true"
                            || texto == "si"
                            || texto == "sí";
                    }
                }

                if (estaAnulada)
                {
                    anuladas++;
                    continue;
                }

                if (!dgvCertificaciones.Columns.Contains("DiasRestantes"))
                    continue;

                object valorDias = row.Cells["DiasRestantes"].Value;

                if (valorDias == null || valorDias == DBNull.Value)
                    continue;

                int dias = Convert.ToInt32(valorDias);

                if (dias < 0)
                    vencidas++;
                else if (dias <= 30)
                    porVencer++;
                else
                    vigentes++;
            }

            lblResumenCertificaciones.Text =
                $"Total: {total} | Vigentes: {vigentes} | Por vencer: {porVencer} | Vencidas: {vencidas} | Anuladas: {anuladas}";
        }

        // =====================================================
        // PINTAR ESTADO (VIGENTE / POR VENCER / VENCIDA)
        // =====================================================
        private void dgvCertificaciones_DataBindingComplete(
            object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            PintarCertificaciones();
        }

        private void PintarCertificaciones()
        {
            if (dgvCertificaciones.Rows.Count == 0)
                return;

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow)
                    continue;

                bool estaAnulada =
                    ObtenerBoolCelda(
                        row,
                        "EstaAnulada");

                if (estaAnulada)
                {
                    row.DefaultCellStyle.BackColor =
                        ColorAnulada;

                    row.DefaultCellStyle.ForeColor =
                        ColorAnuladaTexto;

                    row.DefaultCellStyle.SelectionBackColor =
                        ColorAnuladaSeleccion;

                    row.DefaultCellStyle.SelectionForeColor =
                        ColorAnuladaTexto;

                    row.DefaultCellStyle.Font =
                        _boldFont;

                    continue;
                }

                if (!dgvCertificaciones.Columns.Contains(
                        "DiasRestantes"))
                {
                    continue;
                }

                object valor =
                    row.Cells["DiasRestantes"].Value;

                if (valor == null ||
                    valor == DBNull.Value)
                {
                    continue;
                }

                int dias =
                    Convert.ToInt32(
                        valor);

                if (dias < 0)
                {
                    row.DefaultCellStyle.BackColor =
                        ColorVencida;

                    row.DefaultCellStyle.ForeColor =
                        ColorVencidaTexto;

                    row.DefaultCellStyle.SelectionBackColor =
                        ColorVencidaSeleccion;

                    row.DefaultCellStyle.SelectionForeColor =
                        ColorVencidaTexto;
                }
                else if (dias <= 30)
                {
                    row.DefaultCellStyle.BackColor =
                        ColorPorVencer;

                    row.DefaultCellStyle.ForeColor =
                        ColorPorVencerTexto;

                    row.DefaultCellStyle.SelectionBackColor =
                        ColorPorVencerSeleccion;

                    row.DefaultCellStyle.SelectionForeColor =
                        ColorPorVencerTexto;
                }
                else
                {
                    row.DefaultCellStyle.BackColor =
                        ColorVigente;

                    row.DefaultCellStyle.ForeColor =
                        ColorVigenteTexto;

                    row.DefaultCellStyle.SelectionBackColor =
                        ColorVigenteSeleccion;

                    row.DefaultCellStyle.SelectionForeColor =
                        ColorVigenteTexto;
                }

                row.DefaultCellStyle.Font =
                    _boldFont;
            }
        }


        // =====================================================
        // BUSCAR CERTIFICACIONES
        // =====================================================
        private void txtBuscarCertificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CargarCertificaciones(txtBuscar.Text.Trim());
                e.Handled = true;
            }
        }

        // =====================================================
        // BOTONES
        // =====================================================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar_Click_1(sender, e);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            btnModificar_Click_1(sender, e);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            btnBorrar_Click_1(sender, e);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Sin selección",
                    "Seleccione una certificación.");

                return;
            }

            if (CertificacionSeleccionadaEstaAnulada())
            {
                AppDialog.ShowWarning(
                    this,
                    "Certificación anulada",
                    "Esta certificación se encuentra anulada. No se puede modificar ni renovar mientras tenga una anulación activa.");

                return;
            }

            int idCertificacionSeleccionada = Convert.ToInt32(dgvCertificaciones.CurrentRow.Cells["Id"].Value);

            Clases.Certificacion certificacion = new Clases.Certificacion();
            certificacion.Id = int.Parse(dgvCertificaciones.CurrentRow.Cells["Id"].Value.ToString()!);
            certificacion.FechaCertificacion = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaCertificacion"].Value.ToString()!);
            certificacion.FechaVencimiento = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaVencimiento"].Value.ToString()!);
            certificacion.IdProceso = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdProceso"].Value.ToString()!);
            certificacion.IdCertificador = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdCertificador"].Value.ToString()!);
            certificacion.Comentario = dgvCertificaciones.CurrentRow.Cells["Comentario"].Value.ToString()!;

            using Ventanas.CertificacionesTrabajadorVentana ventana =
                new Ventanas.CertificacionesTrabajadorVentana(
                    certificacion,
                    trabajadorActual);

            ventana.ShowDialog(this);

            if (ventana.DialogResult == DialogResult.OK)
            {
                RefrescarVentana(idCertificacionSeleccionada);
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            using Ventanas.CertificacionesTrabajadorVentana ventana =
                new Ventanas.CertificacionesTrabajadorVentana(
                    null!,
                    trabajadorActual);

            ventana.ShowDialog(this);

            if (ventana.DialogResult == DialogResult.OK)
            {
                RefrescarVentana();
            }
        }

        private void btnBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Sin selección",
                    "Seleccione una certificación.");

                return;
            }

            if (CertificacionSeleccionadaEstaAnulada())
            {
                AppDialog.ShowWarning(
                    this,
                    "Certificación anulada",
                    "Esta certificación se encuentra anulada. Primero debe eliminar la anulación antes de borrar la certificación.");

                return;
            }

            string proceso = "";

            if (dgvCertificaciones.Columns.Contains("Proceso"))
                proceso = dgvCertificaciones.CurrentRow.Cells["Proceso"].Value?.ToString() ?? "";

            bool confirmacion =
                AppDialog.Confirm(
                    this,
                    "Confirmar eliminación",
                    $"¿Deseas eliminar esta certificación?\n\n" +
                    $"Proceso: {proceso}\n" +
                    $"Trabajador: {trabajadorActual.NoReloj} - {trabajadorActual.Nombre}",
                    "Eliminar");

            if (!confirmacion)
                return;

            int id = Convert.ToInt32(dgvCertificaciones.CurrentRow.Cells["Id"].Value);

            Clases.Mensaje respuesta = Clases.Certificacion.BorrarCertificacion(id);

            if (respuesta.Id == 1)
            {
                AppDialog.ShowInfo(
                    this,
                    "Resultado",
                    respuesta.Nombre);

                CargarCertificaciones(txtBuscar.Text.Trim());
            }
            else
            {
                AppDialog.ShowError(
                    this,
                    "No se pudo eliminar",
                    respuesta.Nombre);
            }
        }

        private void dgvCertificaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            btnModificar_Click_1(sender, e);
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CargarCertificaciones(txtBuscar.Text.Trim());
                e.Handled = true;
            }
        }


        private void btnExpediente_Click(object sender, EventArgs e)
        {
            if (trabajadorActual == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Trabajador no encontrado",
                    "No se encontró la información del trabajador.");

                return;
            }

            using ExpedienteTrabajadorVentana ventana =
                new ExpedienteTrabajadorVentana(
                    trabajadorActual.Id,
                    trabajadorActual.NoReloj ?? "",
                    trabajadorActual.Nombre ?? "");

            ventana.ShowDialog(this);
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Sin selección",
                    "Seleccione una certificación para anular.");

                return;
            }

            int idCertificacion = Convert.ToInt32(
                dgvCertificaciones.CurrentRow.Cells["Id"].Value
            );

            string proceso = dgvCertificaciones.CurrentRow.Cells["Proceso"].Value?.ToString() ?? "";

            using CertificacionAnulacionVentana ventana =
                new CertificacionAnulacionVentana(
                    idCertificacion,
                    trabajadorActual.NoReloj ?? "",
                    trabajadorActual.Nombre ?? "",
                    proceso);

            ventana.ShowDialog(this);

            if (ventana.DialogResult == DialogResult.OK)
            {
                CargarCertificaciones(txtBuscar.Text.Trim(), idCertificacion);
            }
        }

        private bool CertificacionSeleccionadaEstaAnulada()
        {
            if (dgvCertificaciones.CurrentRow == null)
                return false;

            if (!dgvCertificaciones.Columns.Contains("EstaAnulada"))
                return false;

            object valor = dgvCertificaciones.CurrentRow.Cells["EstaAnulada"].Value;

            if (valor == null || valor == DBNull.Value)
                return false;

            string valorTexto = valor.ToString()!.Trim().ToLower();

            return valorTexto == "1"
                || valorTexto == "true"
                || valorTexto == "si"
                || valorTexto == "sí";
        }

        private void ActualizarInfoAnulacionSeleccionada()
        {
            if (dgvCertificaciones.CurrentRow == null)
            {
                lblInfoAnulacion.Visible = false;
                lblInfoAnulacion.Text = "";
                return;
            }

            if (!CertificacionSeleccionadaEstaAnulada())
            {
                lblInfoAnulacion.Visible = false;
                lblInfoAnulacion.Text = "";
                return;
            }

            string tipo = ObtenerValorCeldaSeleccionada("TipoAnulacion");
            string fechaInicio = ObtenerFechaCeldaSeleccionada("FechaInicioAnulacion");
            string fechaFin = ObtenerFechaCeldaSeleccionada("FechaFinAnulacion");
            string comentario = ObtenerValorCeldaSeleccionada("ComentarioAnulacion");

            bool esPermanente = ObtenerBoolCeldaSeleccionada("EsPermanente");

            if (esPermanente)
                fechaFin = "Permanente";

            lblInfoAnulacion.Text =
                $"  Certificación anulada | Tipo: {tipo} | Inicio: {fechaInicio} | Fin: {fechaFin} | Motivo: {comentario}";

            lblInfoAnulacion.Visible = true;
        }

        private string ObtenerValorCeldaSeleccionada(string columna)
        {
            if (dgvCertificaciones.CurrentRow == null)
                return "";

            if (!dgvCertificaciones.Columns.Contains(columna))
                return "";

            object valor = dgvCertificaciones.CurrentRow.Cells[columna].Value;

            if (valor == null || valor == DBNull.Value)
                return "";

            return valor.ToString() ?? "";
        }

        private string ObtenerFechaCeldaSeleccionada(string columna)
        {
            if (dgvCertificaciones.CurrentRow == null)
                return "";

            if (!dgvCertificaciones.Columns.Contains(columna))
                return "";

            object valor = dgvCertificaciones.CurrentRow.Cells[columna].Value;

            if (valor == null || valor == DBNull.Value)
                return "";

            if (DateTime.TryParse(valor.ToString(), out DateTime fecha))
                return fecha.ToString("dd/MM/yyyy");

            return valor.ToString() ?? "";
        }

        private bool ObtenerBoolCeldaSeleccionada(string columna)
        {
            if (dgvCertificaciones.CurrentRow == null)
                return false;

            if (!dgvCertificaciones.Columns.Contains(columna))
                return false;

            object valor = dgvCertificaciones.CurrentRow.Cells[columna].Value;

            if (valor == null || valor == DBNull.Value)
                return false;

            string texto = valor.ToString()!.Trim().ToLower();

            return texto == "1"
                || texto == "true"
                || texto == "si"
                || texto == "sí";
        }

        private void dgvCertificaciones_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarInfoAnulacionSeleccionada();
            ActualizarEstadoBotonAnular();
        }

        private void AsignarToolTipsCertificaciones()
        {
            if (dgvCertificaciones.Rows.Count == 0)
                return;

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow)
                    continue;

                bool estaAnulada = ObtenerBoolCelda(row, "EstaAnulada");

                string proceso = ObtenerValorCelda(row, "Proceso");
                string fechaCertificacion = ObtenerFechaCelda(row, "FechaCertificacion");
                string fechaVencimiento = ObtenerFechaCelda(row, "FechaVencimiento");
                string certificador = ObtenerValorCelda(row, "NombreCertificador");
                string comentario = ObtenerValorCelda(row, "Comentario");

                string tooltip;

                if (estaAnulada)
                {
                    string tipo = ObtenerValorCelda(row, "TipoAnulacion");
                    string fechaInicio = ObtenerFechaCelda(row, "FechaInicioAnulacion");
                    string fechaFin = ObtenerFechaCelda(row, "FechaFinAnulacion");
                    string comentarioAnulacion = ObtenerValorCelda(row, "ComentarioAnulacion");

                    bool esPermanente = ObtenerBoolCelda(row, "EsPermanente");

                    if (esPermanente)
                        fechaFin = "Permanente";

                    tooltip =
                        $"Certificación anulada\n\n" +
                        $"Proceso: {proceso}\n" +
                        $"Tipo: {tipo}\n" +
                        $"Inicio: {fechaInicio}\n" +
                        $"Fin: {fechaFin}\n" +
                        $"Motivo: {comentarioAnulacion}";
                }
                else
                {
                    string estado = "Sin estado";
                    string diasTexto = "";

                    if (dgvCertificaciones.Columns.Contains("DiasRestantes"))
                    {
                        object valorDias = row.Cells["DiasRestantes"].Value;

                        if (valorDias != null && valorDias != DBNull.Value)
                        {
                            int dias = Convert.ToInt32(valorDias);
                            diasTexto = dias.ToString();

                            if (dias < 0)
                                estado = "Vencida";
                            else if (dias <= 30)
                                estado = "Por vencer";
                            else
                                estado = "Vigente";
                        }
                    }

                    tooltip =
                        $"Certificación\n\n" +
                        $"Proceso: {proceso}\n" +
                        $"Estado: {estado}\n" +
                        $"Fecha certificación: {fechaCertificacion}\n" +
                        $"Fecha vencimiento: {fechaVencimiento}\n" +
                        $"Días restantes: {diasTexto}\n" +
                        $"Certificador: {certificador}\n" +
                        $"Comentario: {comentario}";
                }

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.ToolTipText = tooltip;
                }
            }
        }

        private string ObtenerValorCelda(DataGridViewRow row, string columna)
        {
            if (!dgvCertificaciones.Columns.Contains(columna))
                return "";

            object valor = row.Cells[columna].Value;

            if (valor == null || valor == DBNull.Value)
                return "";

            return valor.ToString() ?? "";
        }

        private string ObtenerFechaCelda(DataGridViewRow row, string columna)
        {
            if (!dgvCertificaciones.Columns.Contains(columna))
                return "";

            object valor = row.Cells[columna].Value;

            if (valor == null || valor == DBNull.Value)
                return "";

            if (DateTime.TryParse(valor.ToString(), out DateTime fecha))
                return fecha.ToString("dd/MM/yyyy");

            return valor.ToString() ?? "";
        }

        private bool ObtenerBoolCelda(DataGridViewRow row, string columna)
        {
            if (!dgvCertificaciones.Columns.Contains(columna))
                return false;

            object valor = row.Cells[columna].Value;

            if (valor == null || valor == DBNull.Value)
                return false;

            string texto = valor.ToString()!.Trim().ToLower();

            return texto == "1"
                || texto == "true"
                || texto == "si"
                || texto == "sí";
        }

        private void ActualizarEstadoBotonAnular()
        {
            if (dgvCertificaciones.CurrentRow == null ||
                dgvCertificaciones.Rows.Count == 0)
            {
                btnAnular.Text =
                    "Anular";

                btnAnular.Enabled =
                    false;

                btnAnular.BackColor =
                    AppColors.DisabledBackground;

                btnAnular.ForeColor =
                    AppColors.DisabledText;

                return;
            }

            btnAnular.Enabled =
                true;

            if (CertificacionSeleccionadaEstaAnulada())
            {
                btnAnular.Text =
                    "Ver anulación";

                btnAnular.BackColor =
                    ColorAccionAnulada;

                btnAnular.ForeColor =
                    Color.White;

                btnAnular.FlatAppearance.MouseOverBackColor =
                    ControlPaint.Dark(
                        ColorAccionAnulada,
                        0.05F);

                btnAnular.FlatAppearance.MouseDownBackColor =
                    ControlPaint.Dark(
                        ColorAccionAnulada,
                        0.10F);
            }
            else
            {
                btnAnular.Text =
                    "Anular";

                btnAnular.BackColor =
                    ColorAccionAnular;

                btnAnular.ForeColor =
                    Color.White;

                btnAnular.FlatAppearance.MouseOverBackColor =
                    ControlPaint.Dark(
                        ColorAccionAnular,
                        0.05F);

                btnAnular.FlatAppearance.MouseDownBackColor =
                    ControlPaint.Dark(
                        ColorAccionAnular,
                        0.10F);
            }
        }

        private void dgvCertificaciones_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView.HitTestInfo hit = dgvCertificaciones.HitTest(e.X, e.Y);

            if (hit.RowIndex < 0)
                return;

            dgvCertificaciones.ClearSelection();

            DataGridViewRow fila = dgvCertificaciones.Rows[hit.RowIndex];
            fila.Selected = true;

            foreach (DataGridViewCell celda in fila.Cells)
            {
                if (celda.Visible)
                {
                    dgvCertificaciones.CurrentCell = celda;
                    break;
                }
            }

            ActualizarInfoAnulacionSeleccionada();
            ActualizarEstadoBotonAnular();
        }

        private void tsmModificarRenovar_Click(object sender, EventArgs e)
        {
            btnModificar_Click_1(sender, e);
        }

        private void tsmAnularVer_Click(object sender, EventArgs e)
        {
            btnAnular_Click(sender, e);
        }

        private void tsmBorrarCertificacion_Click(object sender, EventArgs e)
        {
            btnBorrar_Click_1(sender, e);
        }

        private void cmsCertificaciones_Opening(object sender, CancelEventArgs e)
        {
            bool haySeleccion = dgvCertificaciones.CurrentRow != null
        && dgvCertificaciones.Rows.Count > 0;

            tsmModificarRenovar.Enabled = haySeleccion;
            tsmAnularVer.Enabled = haySeleccion;
            tsmBorrarCertificacion.Enabled = haySeleccion;

            if (!haySeleccion)
                return;

            if (CertificacionSeleccionadaEstaAnulada())
            {
                tsmModificarRenovar.Text = "Modificar / Renovar";
                tsmModificarRenovar.Enabled = false;

                tsmAnularVer.Text = "Ver anulación";
                tsmBorrarCertificacion.Enabled = false;
            }
            else
            {
                tsmModificarRenovar.Text = "Modificar / Renovar";
                tsmModificarRenovar.Enabled = true;

                tsmAnularVer.Text = "Anular";
                tsmBorrarCertificacion.Enabled = true;
            }
        }

        private void btnCredencial_Click(object sender, EventArgs e)
        {
            if (trabajadorActual == null || string.IsNullOrWhiteSpace(trabajadorActual.NoReloj))
            {
                AppDialog.ShowWarning(
                    this,
                    "Información incompleta",
                    "No se encontró el número de reloj del trabajador.");

                return;
            }

            using Verificaciones ventana =
                new Verificaciones(
                    trabajadorActual.NoReloj);

            ventana.ShowDialog(this);
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarCertificaciones();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtBuscar.Clear();
                CargarCertificaciones();

                e.Handled = true;
            }
        }
    }
}