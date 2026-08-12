using Aseguranza.Clases;
using Aseguranza.UI;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class ExpedienteTrabajadorVentana : Form
    {
        private readonly int idTrabajador;
        private readonly string noReloj;
        private readonly string nombreTrabajador;

        private FilterInfoCollection? dispositivosVideo;
        private VideoCaptureDevice? fuenteVideo;
        private bool camaraActiva = false;

        public ExpedienteTrabajadorVentana(int idTrabajador, string noReloj, string nombreTrabajador)
        {
            InitializeComponent();

            this.idTrabajador = idTrabajador;
            this.noReloj = noReloj;
            this.nombreTrabajador = nombreTrabajador;

            DoubleBuffered = true;

            AplicarEstiloVisual();
        }

        private void ExpedienteTrabajadorVentana_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarExpediente();
            CargarCamaras();

            btnIniciarCamara.Text = "Iniciar cámara";
            btnCapturar.Enabled = false;
            ButtonStyler.UpdateEnabledState(
                btnCapturar,
                AppColors.Primary);

            ConfigurarToolTips();
        }


        // =========================================================
        // INTERFAZ MODERNA
        // =========================================================

        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            FormStyler.ApplyBase(
                this,
                "Expediente del trabajador",
                new Size(
                    1200,
                    760));

            FormStyler.CreateHeader(
                this,
                "Expediente del trabajador",
                "Administra archivos, documentos, fotografías y evidencias del personal",
                height: 105,
                titleX: 44,
                titleY: 18,
                subtitleX: 46,
                subtitleY: 59);

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        125),
                    new Size(
                        1160,
                        615),
                    radius: 14);

            // Los GroupBox originales se conservan en el Designer,
            // pero la interfaz moderna utiliza paneles propios.
            gbInformacion.Visible = false;
            gbArchivos.Visible = false;
            gbCamara.Visible = false;
            gbAcciones.Visible = false;
            lblTitulo.Visible = false;

            // =====================================================
            // INFORMACIÓN DEL TRABAJADOR
            // =====================================================

            Panel pnlTrabajador =
                CrearPanelSeccion(
                    pnlContenido,
                    new Point(20, 18),
                    new Size(1120, 82));

            pnlTrabajador.Controls.Add(
                CrearTituloSeccion(
                    "Información del trabajador",
                    new Point(18, 12)));

            pnlTrabajador.Controls.Add(
                CrearEtiquetaCampo(
                    "No. Reloj",
                    new Point(18, 48)));

            pnlTrabajador.Controls.Add(
                CrearEtiquetaValor(
                    noReloj,
                    new Point(95, 45),
                    new Size(120, 28)));

            pnlTrabajador.Controls.Add(
                CrearEtiquetaCampo(
                    "Nombre",
                    new Point(235, 48)));

            pnlTrabajador.Controls.Add(
                CrearEtiquetaValor(
                    nombreTrabajador,
                    new Point(298, 45),
                    new Size(780, 28)));

            // =====================================================
            // ARCHIVOS DEL EXPEDIENTE
            // =====================================================

            Panel pnlArchivos =
                CrearPanelSeccion(
                    pnlContenido,
                    new Point(20, 116),
                    new Size(730, 410));

            pnlArchivos.Controls.Add(
                CrearTituloSeccion(
                    "Archivos del expediente",
                    new Point(18, 14)));

            lblResumenExpediente.Parent = pnlArchivos;
            lblResumenExpediente.Location = new Point(18, 46);
            lblResumenExpediente.AutoSize = true;
            lblResumenExpediente.Font = AppFonts.Regular(10F, FontStyle.Bold);
            lblResumenExpediente.ForeColor = AppColors.TextPrimary;
            lblResumenExpediente.BackColor = Color.Transparent;

            dgvExpediente.Parent = pnlArchivos;
            dgvExpediente.Location = new Point(18, 74);
            dgvExpediente.Size = new Size(694, 286);
            dgvExpediente.Anchor = AnchorStyles.None;

            lblArchivoSeleccionado.Parent = pnlArchivos;
            lblArchivoSeleccionado.Location = new Point(18, 370);
            lblArchivoSeleccionado.Size = new Size(694, 24);
            lblArchivoSeleccionado.Font = AppFonts.Regular(10F, FontStyle.Bold);
            lblArchivoSeleccionado.ForeColor = AppColors.TextSecondary;
            lblArchivoSeleccionado.BackColor = Color.Transparent;
            lblArchivoSeleccionado.AutoEllipsis = true;

            // =====================================================
            // CÁMARA / VISTA PREVIA
            // =====================================================

            Panel pnlCamara =
                CrearPanelSeccion(
                    pnlContenido,
                    new Point(770, 116),
                    new Size(370, 410));

            pnlCamara.Controls.Add(
                CrearTituloSeccion(
                    "Cámara / Vista previa",
                    new Point(18, 14)));

            lblCamaras.Parent = pnlCamara;
            lblCamaras.Text = "Cámara";
            lblCamaras.Location = new Point(18, 48);
            lblCamaras.AutoSize = true;
            lblCamaras.Font = AppFonts.Regular(10F, FontStyle.Bold);
            lblCamaras.ForeColor = AppColors.TextPrimary;
            lblCamaras.BackColor = Color.Transparent;

            Panel pnlComboCamara =
                new Panel
                {
                    Location = new Point(18, 70),
                    Size = new Size(334, 42),
                    BackColor = Color.White
                };

            cbCamaras.Parent = pnlComboCamara;
            cbCamaras.Location = new Point(7, 8);
            cbCamaras.Size = new Size(320, 27);
            cbCamaras.Anchor = AnchorStyles.None;

            ComboBoxStyler.ApplyOutlinedComboBox(
                pnlComboCamara,
                cbCamaras);

            pnlCamara.Controls.Add(pnlComboCamara);

            Panel pnlVistaPrevia =
                new Panel
                {
                    Location = new Point(18, 124),
                    Size = new Size(334, 178),
                    BackColor = Color.White
                };

            pbCamara.Parent = pnlVistaPrevia;
            pbCamara.Location = new Point(4, 4);
            pbCamara.Size = new Size(326, 170);
            pbCamara.Anchor = AnchorStyles.None;
            pbCamara.BorderStyle = BorderStyle.None;
            pbCamara.BackColor = Color.White;
            pbCamara.SizeMode = PictureBoxSizeMode.Zoom;

            InputStyler.ApplyOutlinedInput(
                pnlVistaPrevia,
                pbCamara,
                radius: 8,
                borderColor: AppColors.BorderMedium);

            pnlCamara.Controls.Add(pnlVistaPrevia);

            ButtonStyler.Apply(
                btnIniciarCamara,
                "Iniciar cámara",
                AppColors.Secondary,
                AppIcons.Search,
                width: 190,
                height: 40);

            btnIniciarCamara.Parent = pnlCamara;
            btnIniciarCamara.Location = new Point(18, 316);
            btnIniciarCamara.Anchor = AnchorStyles.None;

            ButtonStyler.Apply(
                btnCapturar,
                "Capturar",
                AppColors.Primary,
                AppIcons.Save,
                width: 130,
                height: 40);

            btnCapturar.Parent = pnlCamara;
            btnCapturar.Location = new Point(222, 316);
            btnCapturar.Anchor = AnchorStyles.None;

            ButtonStyler.Apply(
                btnSeleccionar,
                "Seleccionar imagen",
                AppColors.Secondary,
                AppIcons.Add,
                width: 334,
                height: 40);

            btnSeleccionar.Parent = pnlCamara;
            btnSeleccionar.Location = new Point(18, 364);
            btnSeleccionar.Anchor = AnchorStyles.None;

            // =====================================================
            // COMENTARIO Y ACCIONES
            // =====================================================

            Panel pnlAcciones =
                CrearPanelSeccion(
                    pnlContenido,
                    new Point(20, 540),
                    new Size(1120, 58));

            lblComentario.Parent = pnlAcciones;
            lblComentario.Text = "Comentario";
            lblComentario.Location = new Point(18, 10);
            lblComentario.AutoSize = true;
            lblComentario.Font = AppFonts.Regular(10F, FontStyle.Bold);
            lblComentario.ForeColor = AppColors.TextPrimary;
            lblComentario.BackColor = Color.Transparent;

            Panel pnlComentario =
                new Panel
                {
                    Location = new Point(108, 8),
                    Size = new Size(290, 42),
                    BackColor = Color.White
                };

            txtComentario.Parent = pnlComentario;
            txtComentario.Location = new Point(8, 6);
            txtComentario.Size = new Size(274, 30);
            txtComentario.BorderStyle = BorderStyle.None;
            txtComentario.Font = AppFonts.Light(10.5F);
            txtComentario.BackColor = Color.White;
            txtComentario.ForeColor = AppColors.TextPrimary;
            txtComentario.ScrollBars = ScrollBars.Vertical;

            InputStyler.ApplyOutlinedInput(
                pnlComentario,
                txtComentario);

            pnlAcciones.Controls.Add(pnlComentario);

            ButtonStyler.Apply(
                btnAdjuntar,
                "Adjuntar",
                AppColors.Primary,
                AppIcons.Add,
                width: 135,
                height: 42);

            btnAdjuntar.Parent = pnlAcciones;
            btnAdjuntar.Location = new Point(408, 8);
            btnAdjuntar.Anchor = AnchorStyles.None;

            ButtonStyler.Apply(
                btnAbrir,
                "Abrir",
                AppColors.Secondary,
                AppIcons.Search,
                width: 110,
                height: 42);

            btnAbrir.Parent = pnlAcciones;
            btnAbrir.Location = new Point(553, 8);
            btnAbrir.Anchor = AnchorStyles.None;

            ButtonStyler.Apply(
                btnReemplazar,
                "Reemplazar",
                AppColors.Secondary,
                AppIcons.Edit,
                width: 150,
                height: 42);

            btnReemplazar.Parent = pnlAcciones;
            btnReemplazar.Location = new Point(673, 8);
            btnReemplazar.Anchor = AnchorStyles.None;

            ButtonStyler.Apply(
                btnEliminar,
                "Eliminar",
                AppColors.Danger,
                AppIcons.Delete,
                width: 130,
                height: 42);

            btnEliminar.Parent = pnlAcciones;
            btnEliminar.Location = new Point(833, 8);
            btnEliminar.Anchor = AnchorStyles.None;

            ButtonStyler.Apply(
                btnRegresar,
                "Regresar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 130,
                height: 42);

            btnRegresar.Parent = pnlAcciones;
            btnRegresar.Location = new Point(973, 8);
            btnRegresar.Anchor = AnchorStyles.None;

            // =====================================================
            // MENÚ CONTEXTUAL
            // =====================================================

            cmsExpediente.Font = AppFonts.Light(10F);
            cmsExpediente.ShowImageMargin = false;

            ResumeLayout(true);
        }

        private static Panel CrearPanelSeccion(
            Control parent,
            Point location,
            Size size)
        {
            Panel panel =
                new Panel
                {
                    Location = location,
                    Size = size,
                    BackColor = AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                panel,
                10);

            parent.Controls.Add(panel);

            return panel;
        }

        private static Label CrearTituloSeccion(
            string texto,
            Point location)
        {
            return new Label
            {
                AutoSize = true,
                Text = texto,
                Location = location,
                ForeColor = AppColors.TextPrimary,
                Font = AppFonts.Regular(13F, FontStyle.Bold),
                BackColor = Color.Transparent
            };
        }

        private static Label CrearEtiquetaCampo(
            string texto,
            Point location)
        {
            return new Label
            {
                AutoSize = true,
                Text = texto,
                Location = location,
                ForeColor = AppColors.TextSecondary,
                Font = AppFonts.Regular(10F, FontStyle.Bold),
                BackColor = Color.Transparent
            };
        }

        private static Label CrearEtiquetaValor(
            string texto,
            Point location,
            Size size)
        {
            return new Label
            {
                AutoSize = false,
                Text = texto,
                Location = location,
                Size = size,
                ForeColor = AppColors.TextPrimary,
                Font = AppFonts.Regular(12F, FontStyle.Bold),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };
        }


        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(btnAdjuntar, "Adjuntar un archivo al expediente del trabajador.");
            ttAyuda.SetToolTip(btnAbrir, "Abrir el archivo seleccionado.");
            ttAyuda.SetToolTip(btnReemplazar, "Reemplazar el archivo seleccionado por otro.");
            ttAyuda.SetToolTip(btnEliminar, "Eliminar el archivo seleccionado del expediente.");
            ttAyuda.SetToolTip(btnRegresar, "Cerrar esta ventana.");

            ttAyuda.SetToolTip(btnIniciarCamara, "Iniciar o detener la cámara.");
            ttAyuda.SetToolTip(btnCapturar, "Capturar una foto desde la cámara y guardarla en el expediente.");
            ttAyuda.SetToolTip(btnSeleccionar, "Seleccionar una imagen desde la computadora y guardarla en el expediente.");

            ttAyuda.SetToolTip(txtComentario, "Comentario que se guardará junto con el archivo adjuntado, reemplazado o capturado.");
            ttAyuda.SetToolTip(
    dgvExpediente,
    "Doble clic para abrir un archivo. Clic derecho para más opciones. Los archivos en rojo no se encontraron en la carpeta guardada."
);
            ttAyuda.SetToolTip(pbCamara, "Vista previa de imágenes o vista de la cámara.");
        }


        private void ConfigurarGrid()
        {
            DataGridViewStyler.ApplyCatalogStyle(
                dgvExpediente,
                headerHeight: 40,
                rowHeight: 38);

            dgvExpediente.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvExpediente.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            dgvExpediente.DefaultCellStyle.SelectionBackColor =
                AppColors.Selection;

            dgvExpediente.DefaultCellStyle.SelectionForeColor =
                AppColors.TextPrimary;
        }

        private void CargarExpediente()
        {
            DataTable dt =
                Clases.ExpedienteTrabajador
                    .ConsultarExpedienteTrabajador(
                        idTrabajador);

            dgvExpediente.DataSource =
                dt;

            OcultarColumnasExpediente();
            ConfigurarColumnasExpediente();

            PintarArchivosFaltantes();

            ActualizarResumenExpediente();

            SincronizarSeleccionExpediente();
        }

        // =========================================================
        // SINCRONIZAR SELECCIÓN DEL EXPEDIENTE
        // =========================================================

        private void SincronizarSeleccionExpediente()
        {
            if (dgvExpediente.Rows.Count == 0)
            {
                dgvExpediente.ClearSelection();

                dgvExpediente.CurrentCell =
                    null;

                ActualizarEstadoBotones();
                ActualizarArchivoSeleccionado();
                MostrarVistaPreviaSeleccionada();

                return;
            }

            if (dgvExpediente.CurrentRow is null)
            {
                dgvExpediente.ClearSelection();

                DataGridViewRow primeraFila =
                    dgvExpediente.Rows[0];

                primeraFila.Selected =
                    true;

                DataGridViewCell? primeraCeldaVisible =
                    primeraFila.Cells
                        .Cast<DataGridViewCell>()
                        .FirstOrDefault(
                            celda =>
                                celda.Visible);

                if (primeraCeldaVisible is not null)
                {
                    dgvExpediente.CurrentCell =
                        primeraCeldaVisible;
                }
            }

            ActualizarEstadoBotones();
            ActualizarArchivoSeleccionado();
            MostrarVistaPreviaSeleccionada();

            // WinForms puede terminar de establecer CurrentRow después
            // de asignar el DataSource. Esta segunda sincronización
            // garantiza que los botones queden correctos desde el inicio.
            if (IsHandleCreated &&
                !IsDisposed &&
                !Disposing)
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

                            if (dgvExpediente.Rows.Count > 0 &&
                                dgvExpediente.CurrentRow is null)
                            {
                                dgvExpediente.ClearSelection();

                                DataGridViewRow primeraFila =
                                    dgvExpediente.Rows[0];

                                primeraFila.Selected =
                                    true;

                                DataGridViewCell? primeraCeldaVisible =
                                    primeraFila.Cells
                                        .Cast<DataGridViewCell>()
                                        .FirstOrDefault(
                                            celda =>
                                                celda.Visible);

                                if (primeraCeldaVisible is not null)
                                {
                                    dgvExpediente.CurrentCell =
                                        primeraCeldaVisible;
                                }
                            }

                            ActualizarEstadoBotones();
                            ActualizarArchivoSeleccionado();
                            MostrarVistaPreviaSeleccionada();
                        }));
            }
        }

        private void ActualizarArchivoSeleccionado()
        {
            if (dgvExpediente.CurrentRow == null || dgvExpediente.Rows.Count == 0)
            {
                lblArchivoSeleccionado.Text = "Archivo seleccionado: ninguno";
                return;
            }

            string nombreArchivo = "sin nombre";

            if (dgvExpediente.Columns.Contains("NombreOriginal"))
            {
                nombreArchivo = dgvExpediente.CurrentRow.Cells["NombreOriginal"].Value?.ToString() ?? "sin nombre";
            }

            lblArchivoSeleccionado.Text = $"Archivo seleccionado: {nombreArchivo}";
        }

        private void ActualizarResumenExpediente()
        {
            int totalArchivos = dgvExpediente.Rows.Count;

            lblResumenExpediente.Text = totalArchivos == 1
                ? "Archivos: 1 archivo registrado"
                : $"Archivos: {totalArchivos} archivos registrados";
        }

        private void OcultarColumnasExpediente()
        {
            string[] columnasOcultas =
            {
        "Id",
        "IdTrabajador",
        "NombreArchivo"
    };

            foreach (string columna in columnasOcultas)
            {
                if (dgvExpediente.Columns.Contains(columna))
                    dgvExpediente.Columns[columna].Visible = false;
            }
        }


        private void ConfigurarColumnasExpediente()
        {
            if (dgvExpediente.Columns.Contains("NombreOriginal"))
            {
                dgvExpediente.Columns["NombreOriginal"].HeaderText = "Archivo";
                dgvExpediente.Columns["NombreOriginal"].FillWeight = 190;
            }

            if (dgvExpediente.Columns.Contains("Extension"))
            {
                dgvExpediente.Columns["Extension"].HeaderText = "Ext.";
                dgvExpediente.Columns["Extension"].FillWeight = 55;
                dgvExpediente.Columns["Extension"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvExpediente.Columns.Contains("RutaArchivo"))
            {
                // La ruta completa sigue disponible para abrir ubicación,
                // copiar ruta y demás acciones, pero no ocupa espacio
                // en la tabla principal.
                dgvExpediente.Columns["RutaArchivo"].Visible = false;
            }

            if (dgvExpediente.Columns.Contains("TipoArchivo"))
            {
                dgvExpediente.Columns["TipoArchivo"].HeaderText = "Tipo";
                dgvExpediente.Columns["TipoArchivo"].FillWeight = 85;
                dgvExpediente.Columns["TipoArchivo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvExpediente.Columns.Contains("Comentario"))
            {
                dgvExpediente.Columns["Comentario"].HeaderText = "Comentario";
                dgvExpediente.Columns["Comentario"].FillWeight = 155;
            }

            if (dgvExpediente.Columns.Contains("FechaRegistro"))
            {
                dgvExpediente.Columns["FechaRegistro"].HeaderText = "Fecha registro";
                dgvExpediente.Columns["FechaRegistro"].FillWeight = 120;
                dgvExpediente.Columns["FechaRegistro"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvExpediente.Columns["FechaRegistro"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvExpediente.Columns.Contains("FechaModificacion"))
            {
                dgvExpediente.Columns["FechaModificacion"].HeaderText = "Fecha modificación";
                dgvExpediente.Columns["FechaModificacion"].FillWeight = 120;
                dgvExpediente.Columns["FechaModificacion"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvExpediente.Columns["FechaModificacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void ActualizarEstadoBotones()
        {
            bool hayArchivoSeleccionado =
                dgvExpediente.CurrentRow != null &&
                dgvExpediente.Rows.Count > 0;

            btnAbrir.Enabled = hayArchivoSeleccionado;
            btnReemplazar.Enabled = hayArchivoSeleccionado;
            btnEliminar.Enabled = hayArchivoSeleccionado;

            ButtonStyler.UpdateEnabledState(
                btnAbrir,
                AppColors.Secondary);

            ButtonStyler.UpdateEnabledState(
                btnReemplazar,
                AppColors.Secondary);

            ButtonStyler.UpdateEnabledState(
                btnEliminar,
                AppColors.Danger);
        }

        private void CargarCamaras()
        {
            try
            {
                cbCamaras.Items.Clear();

                dispositivosVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach (FilterInfo dispositivo in dispositivosVideo)
                {
                    cbCamaras.Items.Add(dispositivo.Name);
                }

                if (cbCamaras.Items.Count > 0)
                {
                    cbCamaras.SelectedIndex = 0;
                }
                else
                {
                    AppDialog.ShowInfo(
                        this,
                        "Cámara",
                        "No se encontraron cámaras disponibles.");
                }
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al cargar cámaras",
                    error.Message);
            }
        }

        private void FuenteVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame =
                (Bitmap)eventArgs.Frame.Clone();

            if (!camaraActiva ||
                IsDisposed ||
                pbCamara.IsDisposed)
            {
                frame.Dispose();
                return;
            }

            try
            {
                if (pbCamara.InvokeRequired)
                {
                    pbCamara.BeginInvoke(
                        new MethodInvoker(
                            delegate
                            {
                                if (!camaraActiva ||
                                    IsDisposed ||
                                    pbCamara.IsDisposed)
                                {
                                    frame.Dispose();
                                    return;
                                }

                                pbCamara.Image?.Dispose();
                                pbCamara.Image = frame;
                            }));
                }
                else
                {
                    if (!camaraActiva)
                    {
                        frame.Dispose();
                        return;
                    }

                    pbCamara.Image?.Dispose();
                    pbCamara.Image = frame;
                }
            }
            catch
            {
                frame.Dispose();
            }
        }

        private void GuardarImagenEnExpediente(Bitmap imagen, string nombreArchivo, string comentario)
        {
            string carpetaDestino = Clases.RutasArchivos.ObtenerCarpetaExpedienteFotos(noReloj);
            Directory.CreateDirectory(carpetaDestino);

            string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

            try
            {
                imagen.Save(rutaDestino, ImageFormat.Jpeg);

                Clases.ExpedienteTrabajador expediente = new Clases.ExpedienteTrabajador
                {
                    IdTrabajador = idTrabajador,
                    NombreOriginal = nombreArchivo,
                    NombreArchivo = nombreArchivo,
                    Extension = ".jpg",
                    RutaArchivo = rutaDestino,
                    TipoArchivo = "Imagen",
                    Comentario = comentario
                };

                Mensaje respuesta = expediente.GuardarExpedienteTrabajador();

                if (respuesta.Id == 1)
                {
                    AppDialog.ShowInfo(
                        this,
                        "Operación completada",
                        respuesta.Nombre);

                    txtComentario.Clear();
                    CargarExpediente();
                }
                else
                {
                    if (File.Exists(rutaDestino))
                        File.Delete(rutaDestino);

                    AppDialog.ShowError(
                        this,
                        "Error",
                        respuesta.Nombre);
                }
            }
            catch
            {
                if (File.Exists(rutaDestino))
                    File.Delete(rutaDestino);

                throw;
            }
        }


        private void DetenerCamara(
            bool conservarImagenActual = false)
        {
            try
            {
                camaraActiva = false;

                if (fuenteVideo != null)
                {
                    fuenteVideo.NewFrame -=
                        FuenteVideo_NewFrame;

                    if (fuenteVideo.IsRunning)
                    {
                        fuenteVideo.SignalToStop();
                        fuenteVideo.WaitForStop();
                    }

                    fuenteVideo = null;
                }

                btnIniciarCamara.Text =
                    "Iniciar cámara";

                btnCapturar.Enabled =
                    false;

                ButtonStyler.UpdateEnabledState(
                    btnCapturar,
                    AppColors.Primary);

                cbCamaras.Enabled =
                    true;

                if (!conservarImagenActual)
                {
                    pbCamara.Image?.Dispose();
                    pbCamara.Image = null;

                    MostrarVistaPreviaSeleccionada();
                }
            }
            catch
            {
                camaraActiva = false;

                btnIniciarCamara.Text =
                    "Iniciar cámara";

                btnCapturar.Enabled =
                    false;

                ButtonStyler.UpdateEnabledState(
                    btnCapturar,
                    AppColors.Primary);

                cbCamaras.Enabled =
                    true;
            }
        }

        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar archivo para expediente",
                Filter = "Archivos permitidos|*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx|Todos los archivos|*.*"
            };

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            string rutaOrigen = ofd.FileName;
            string extension = Path.GetExtension(rutaOrigen).ToLower();
            string tipoArchivo = Clases.ExpedienteTrabajador.ObtenerTipoArchivo(extension);

            string carpetaDestino = ObtenerCarpetaDestino(extension);
            Directory.CreateDirectory(carpetaDestino);

            string nombreOriginal = Path.GetFileName(rutaOrigen);
            string nombreSinExtension = Path.GetFileNameWithoutExtension(rutaOrigen);
            string nombreSeguro = LimpiarNombreArchivo(nombreSinExtension);

            string nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmmss}_{nombreSeguro}{extension}";
            string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

            try
            {
                File.Copy(rutaOrigen, rutaDestino, false);

                Clases.ExpedienteTrabajador expediente = new Clases.ExpedienteTrabajador
                {
                    IdTrabajador = idTrabajador,
                    NombreOriginal = nombreOriginal,
                    NombreArchivo = nombreArchivo,
                    Extension = extension,
                    RutaArchivo = rutaDestino,
                    TipoArchivo = tipoArchivo,
                    Comentario = txtComentario.Text.Trim()
                };

                Mensaje respuesta = expediente.GuardarExpedienteTrabajador();

                if (respuesta.Id == 1)
                {
                    AppDialog.ShowInfo(
                        this,
                        "Operación completada",
                        respuesta.Nombre);

                    txtComentario.Clear();
                    CargarExpediente();
                }
                else
                {
                    if (File.Exists(rutaDestino))
                        File.Delete(rutaDestino);

                    AppDialog.ShowError(
                        this,
                        "Error",
                        respuesta.Nombre);
                }
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al adjuntar archivo",
                    error.Message);
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            string? rutaArchivo = ObtenerRutaArchivoSeleccionado();

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                return;

            if (!File.Exists(rutaArchivo))
            {
                AppDialog.ShowWarning(
                    this,
                    "Archivo no encontrado",
                    "El archivo no existe en la ubicación guardada.\n\n" +
                    $"Ruta: {rutaArchivo}\n\n" +
                    "Es posible que haya sido eliminado, movido o que no tenga acceso a la carpeta.");

                return;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = rutaArchivo,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al abrir archivo",
                    error.Message);
            }
        }

        private void btnReemplazar_Click(object sender, EventArgs e)
        {
            int? idExpediente = ObtenerIdSeleccionado();

            if (idExpediente == null)
                return;

            string? rutaAnterior = ObtenerRutaArchivoSeleccionado();

            string nombreArchivoActual = "archivo seleccionado";

            if (dgvExpediente.CurrentRow != null && dgvExpediente.Columns.Contains("NombreOriginal"))
            {
                nombreArchivoActual = dgvExpediente.CurrentRow.Cells["NombreOriginal"].Value?.ToString() ?? "archivo seleccionado";
            }

            bool confirmacion =
                AppDialog.Confirm(
                    this,
                    "Confirmar reemplazo",
                    "Va a reemplazar el siguiente archivo:\n\n" +
                    $"{nombreArchivoActual}\n\n" +
                    "¿Desea continuar?",
                    "Reemplazar");

            if (!confirmacion)
                return;



            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar archivo de reemplazo",
                Filter = "Archivos permitidos|*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx|Todos los archivos|*.*"
            };

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            string rutaOrigen = ofd.FileName;
            string extension = Path.GetExtension(rutaOrigen).ToLower();
            string tipoArchivo = Clases.ExpedienteTrabajador.ObtenerTipoArchivo(extension);

            string carpetaDestino = ObtenerCarpetaDestino(extension);
            Directory.CreateDirectory(carpetaDestino);

            string nombreOriginal = Path.GetFileName(rutaOrigen);
            string nombreSinExtension = Path.GetFileNameWithoutExtension(rutaOrigen);
            string nombreSeguro = LimpiarNombreArchivo(nombreSinExtension);

            string nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmmss}_{nombreSeguro}{extension}";
            string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

            try
            {
                File.Copy(rutaOrigen, rutaDestino, false);

                Clases.ExpedienteTrabajador expediente = new Clases.ExpedienteTrabajador
                {
                    Id = idExpediente.Value,
                    IdTrabajador = idTrabajador,
                    NombreOriginal = nombreOriginal,
                    NombreArchivo = nombreArchivo,
                    Extension = extension,
                    RutaArchivo = rutaDestino,
                    TipoArchivo = tipoArchivo,
                    Comentario = txtComentario.Text.Trim()
                };

                Mensaje respuesta = expediente.ReemplazarExpedienteTrabajador();

                if (respuesta.Id == 1)
                {
                    if (!string.IsNullOrWhiteSpace(rutaAnterior)
                        && File.Exists(rutaAnterior)
                        && rutaAnterior != rutaDestino)
                    {
                        try
                        {
                            File.Delete(rutaAnterior);
                        }
                        catch
                        {
                            // Si no se puede borrar el anterior porque está abierto,
                            // no detenemos el proceso. El nuevo ya quedó guardado.
                        }
                    }

                    AppDialog.ShowInfo(
                        this,
                        "Operación completada",
                        respuesta.Nombre);

                    txtComentario.Clear();
                    CargarExpediente();
                }
                else
                {
                    if (File.Exists(rutaDestino))
                        File.Delete(rutaDestino);

                    AppDialog.ShowError(
                        this,
                        "Error",
                        respuesta.Nombre);
                }
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al reemplazar archivo",
                    error.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? idExpediente = ObtenerIdSeleccionado();

            if (idExpediente == null)
                return;

            string? rutaArchivo = ObtenerRutaArchivoSeleccionado();

            string nombreArchivo = "archivo seleccionado";

            if (dgvExpediente.CurrentRow != null && dgvExpediente.Columns.Contains("NombreOriginal"))
            {
                nombreArchivo = dgvExpediente.CurrentRow.Cells["NombreOriginal"].Value?.ToString() ?? "archivo seleccionado";
            }

            bool confirmacion =
                AppDialog.Confirm(
                    this,
                    "Confirmar eliminación",
                    "¿Seguro que desea eliminar este archivo del expediente?\n\n" +
                    $"Archivo: {nombreArchivo}",
                    "Eliminar");

            if (!confirmacion)
                return;

            Mensaje respuesta = Clases.ExpedienteTrabajador.EliminarExpedienteTrabajador(idExpediente.Value);

            if (respuesta.Id == 1)
            {
                if (!string.IsNullOrWhiteSpace(rutaArchivo) && File.Exists(rutaArchivo))
                {
                    try
                    {
                        File.Delete(rutaArchivo);
                    }
                    catch
                    {
                        AppDialog.ShowWarning(
                            this,
                            "Advertencia",
                            "El registro se eliminó del expediente, pero el archivo físico no se pudo borrar. Es posible que esté abierto.");
                    }
                }

                AppDialog.ShowInfo(
                    this,
                    "Operación completada",
                    respuesta.Nombre);

                CargarExpediente();
            }
            else
            {
                AppDialog.ShowError(
                    this,
                    "Error",
                    respuesta.Nombre);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int? ObtenerIdSeleccionado()
        {
            if (dgvExpediente.CurrentRow == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Sin selección",
                    "Seleccione un archivo del expediente.");

                return null;
            }

            if (!dgvExpediente.Columns.Contains("Id"))
            {
                AppDialog.ShowError(
                    this,
                    "Error",
                    "No se encontró la columna Id.");

                return null;
            }

            object valor = dgvExpediente.CurrentRow.Cells["Id"].Value;

            if (valor == null || valor == DBNull.Value)
                return null;

            return Convert.ToInt32(valor);
        }

        private string? ObtenerRutaArchivoSeleccionado()
        {
            if (dgvExpediente.CurrentRow == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Sin selección",
                    "Seleccione un archivo del expediente.");

                return null;
            }

            if (!dgvExpediente.Columns.Contains("RutaArchivo"))
            {
                AppDialog.ShowError(
                    this,
                    "Error",
                    "No se encontró la columna RutaArchivo.");

                return null;
            }

            object valor = dgvExpediente.CurrentRow.Cells["RutaArchivo"].Value;

            if (valor == null || valor == DBNull.Value)
                return null;

            return valor.ToString();
        }

        private string ObtenerCarpetaDestino(string extension)
        {
            extension = extension.ToLower().Trim();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                return Clases.RutasArchivos.ObtenerCarpetaExpedienteFotos(noReloj);

            if (extension == ".pdf")
                return Clases.RutasArchivos.ObtenerCarpetaExpedientePDFs(noReloj);

            return Clases.RutasArchivos.ObtenerCarpetaExpedienteDocumentos(noReloj);
        }

        private string LimpiarNombreArchivo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "ARCHIVO";

            char[] caracteresInvalidos = Path.GetInvalidFileNameChars();

            string limpio = new string(
                texto
                    .Trim()
                    .Where(c => !caracteresInvalidos.Contains(c))
                    .ToArray()
            );

            limpio = limpio.Replace(" ", "_");

            return string.IsNullOrWhiteSpace(limpio) ? "ARCHIVO" : limpio;
        }

        private void btnIniciarCamara_Click(object sender, EventArgs e)
        {
            if (camaraActiva)
            {
                DetenerCamara();
                return;
            }

            if (dispositivosVideo == null || dispositivosVideo.Count == 0)
            {
                AppDialog.ShowWarning(
                    this,
                    "Cámara",
                    "No hay cámaras disponibles.");

                return;
            }

            if (cbCamaras.SelectedIndex < 0)
            {
                AppDialog.ShowWarning(
                    this,
                    "Cámara",
                    "Seleccione una cámara.");

                return;
            }

            try
            {
                fuenteVideo = new VideoCaptureDevice(dispositivosVideo[cbCamaras.SelectedIndex].MonikerString);
                fuenteVideo.NewFrame += FuenteVideo_NewFrame;
                fuenteVideo.Start();

                camaraActiva = true;

                btnIniciarCamara.Text = "Detener cámara";
                btnCapturar.Enabled = true;
                ButtonStyler.UpdateEnabledState(
                    btnCapturar,
                    AppColors.Primary);
                cbCamaras.Enabled = false;
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al iniciar cámara",
                    error.Message);
            }
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (pbCamara.Image == null)
            {
                AppDialog.ShowWarning(
                    this,
                    "Cámara",
                    "No hay imagen de la cámara para capturar.");

                return;
            }

            try
            {
                using Bitmap fotoCapturada =
                    new Bitmap(
                        pbCamara.Image);

                // Al capturar, la cámara debe detenerse para liberar
                // el dispositivo y dejar claro qué imagen se tomó.
                DetenerCamara(
                    conservarImagenActual: true);

                string nombreArchivo =
                    $"{DateTime.Now:yyyyMMdd_HHmmss}_FotoExpediente.jpg";

                GuardarImagenEnExpediente(
                    fotoCapturada,
                    nombreArchivo,
                    txtComentario.Text.Trim());

                // CargarExpediente puede actualizar la selección del grid.
                // Volvemos a mostrar explícitamente la foto capturada para
                // que la vista previa quede congelada en esa imagen.
                pbCamara.Image?.Dispose();

                pbCamara.Image =
                    new Bitmap(
                        fotoCapturada);
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al capturar foto",
                    error.Message);
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar imagen para expediente",
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            // Si el usuario eligió una imagen externa, dejamos de usar
            // la cámara como fuente de vista previa. Así liberamos el
            // dispositivo y la imagen seleccionada puede mostrarse.
            if (camaraActiva || fuenteVideo?.IsRunning == true)
            {
                DetenerCamara();
            }

            try
            {
                string rutaOrigen = ofd.FileName;

                string nombreSinExtension = Path.GetFileNameWithoutExtension(rutaOrigen);
                string nombreSeguro = LimpiarNombreArchivo(nombreSinExtension);

                string nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmmss}_{nombreSeguro}.jpg";

                using FileStream fs = new FileStream(rutaOrigen, FileMode.Open, FileAccess.Read);
                using Image imagenSeleccionada = Image.FromStream(fs);
                using Bitmap imagenFinal = new Bitmap(imagenSeleccionada);

                GuardarImagenEnExpediente(
                    imagenFinal,
                    nombreArchivo,
                    txtComentario.Text.Trim()
                );
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al seleccionar imagen",
                    error.Message);
            }
        }

        private void ExpedienteTrabajadorVentana_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetenerCamara();

            if (pbCamara.Image != null)
            {
                pbCamara.Image.Dispose();
                pbCamara.Image = null;
            }
        }

        private void dgvExpediente_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBotones();
            ActualizarArchivoSeleccionado();
            MostrarVistaPreviaSeleccionada();
        }


        private void MostrarVistaPreviaSeleccionada()
        {
            if (camaraActiva)
                return;

            string? rutaArchivo = ObtenerRutaArchivoSeleccionadoSinMensaje();

            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                LimpiarVistaPrevia();
                return;
            }

            if (!File.Exists(rutaArchivo))
            {
                LimpiarVistaPrevia();
                return;
            }

            string extension = Path.GetExtension(rutaArchivo).ToLower();

            if (!EsImagen(extension))
            {
                LimpiarVistaPrevia();
                return;
            }

            try
            {
                using FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                using Image imagenTemporal = Image.FromStream(fs);
                Bitmap imagenClonada = new Bitmap(imagenTemporal);

                pbCamara.Image?.Dispose();
                pbCamara.Image = imagenClonada;
            }
            catch
            {
                LimpiarVistaPrevia();
            }
        }

        private void LimpiarVistaPrevia()
        {
            if (camaraActiva)
                return;

            pbCamara.Image?.Dispose();
            pbCamara.Image = null;
        }


        private string? ObtenerRutaArchivoSeleccionadoSinMensaje()
        {
            if (dgvExpediente.CurrentRow == null)
                return null;

            if (!dgvExpediente.Columns.Contains("RutaArchivo"))
                return null;

            object valor = dgvExpediente.CurrentRow.Cells["RutaArchivo"].Value;

            if (valor == null || valor == DBNull.Value)
                return null;

            return valor.ToString();
        }


        private bool EsImagen(string extension)
        {
            extension = extension.ToLower().Trim();

            return extension == ".jpg"
                || extension == ".jpeg"
                || extension == ".png"
                || extension == ".bmp";
        }

        private void dgvExpediente_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            btnAbrir_Click(sender, e);
        }

        private void dgvExpediente_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView.HitTestInfo hit = dgvExpediente.HitTest(e.X, e.Y);

            if (hit.RowIndex < 0)
                return;

            dgvExpediente.ClearSelection();

            DataGridViewRow fila = dgvExpediente.Rows[hit.RowIndex];
            fila.Selected = true;

            DataGridViewCell? celdaVisible = null;

            foreach (DataGridViewCell celda in fila.Cells)
            {
                if (celda.Visible)
                {
                    celdaVisible = celda;
                    break;
                }
            }

            if (celdaVisible != null)
            {
                dgvExpediente.CurrentCell = celdaVisible;
            }
        }

        private void cmsExpediente_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hayArchivoSeleccionado = dgvExpediente.CurrentRow != null
    && dgvExpediente.Rows.Count > 0;

            tsmAbrir.Enabled = hayArchivoSeleccionado;
            tsmAbrirUbicacion.Enabled = hayArchivoSeleccionado;
            tsmReemplazar.Enabled = hayArchivoSeleccionado;
            tsmEliminar.Enabled = hayArchivoSeleccionado;
            tsmCopiarRuta.Enabled = hayArchivoSeleccionado;
        }

        private void tsmAbrir_Click(object sender, EventArgs e)
        {
            btnAbrir_Click(sender, e);
        }

        private void tsmReemplazar_Click(object sender, EventArgs e)
        {
            btnReemplazar_Click(sender, e);
        }

        private void tsmEliminar_Click(object sender, EventArgs e)
        {
            btnEliminar_Click(sender, e);
        }

        private void tsmCopiarRuta_Click(object sender, EventArgs e)
        {
            string? rutaArchivo = ObtenerRutaArchivoSeleccionadoSinMensaje();

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                return;

            Clipboard.SetText(rutaArchivo);

            AppDialog.ShowInfo(
                this,
                "Copiar ruta",
                "Ruta copiada al portapapeles.");
        }

        private void tsmAbrirUbicacion_Click(object sender, EventArgs e)
        {
            string? rutaArchivo = ObtenerRutaArchivoSeleccionadoSinMensaje();

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                return;

            if (!File.Exists(rutaArchivo))
            {
                AppDialog.ShowWarning(
                    this,
                    "Archivo no encontrado",
                    "El archivo no existe en la ubicación guardada.");

                return;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{rutaArchivo}\"",
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception error)
            {
                AppDialog.ShowError(
                    this,
                    "Error al abrir ubicación",
                    error.Message);
            }
        }

        private void PintarArchivosFaltantes()
        {
            if (!dgvExpediente.Columns.Contains("RutaArchivo"))
                return;

            foreach (DataGridViewRow row in dgvExpediente.Rows)
            {
                if (row.IsNewRow)
                    continue;

                object valorRuta = row.Cells["RutaArchivo"].Value;

                if (valorRuta == null || valorRuta == DBNull.Value)
                    continue;

                string rutaArchivo = valorRuta.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(rutaArchivo))
                    continue;

                if (!File.Exists(rutaArchivo))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 226, 226);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 202, 202);
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                }
            }
        }

    }
}