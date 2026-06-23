using Aseguranza.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Aseguranza.Ventanas
{
    public partial class TrabajadoresVentana : Form
    {
        private bool capturaDesdeCamara = false;
        private Clases.Trabajador? trabajadorActual;
        private string? rutaFotoSeleccionada;

        private FilterInfoCollection? dispositivosVideo;
        private VideoCaptureDevice? camara;
        private Bitmap? fotoCapturada;

        private bool guardadoCorrectamente = false;

        private bool guardando = false;

        private bool cargandoDatos = false;

        public string NoRelojGuardado { get; private set; } = "";
        public TrabajadoresVentana(Clases.Trabajador trabajador)
        {
            InitializeComponent();
            if (trabajador != null)
            {
                trabajadorActual = trabajador;
                //txtNombre.Text = localidad.Nombre;
            }
        }

        private void TrabajadoresVentana_Load(object sender, EventArgs e)
        {
            ConfigurarModoVentana();
            ConfigurarToolTips();

            CargarComboBox();
            CargarCamaras();

            if (pictureBox1.Image == null)
                ActualizarEstadoFoto("SIN_FOTO");

            ActualizarBotonesCamara();

            MostrarEstado("Listo.");
        }

        private void MostrarEstado(string mensaje, bool esAdvertencia = false)
        {
            tsslEstado.Text = mensaje;

            if (esAdvertencia)
                tsslEstado.ForeColor = Color.DarkOrange;
            else
                tsslEstado.ForeColor = Color.DimGray;
        }

        private void CargarLineasPorPlanta(int idPlanta, int idLineaSeleccionar = 0)
        {
            cbLinea.DataSource = null;
            cbLinea.Enabled = false;

            var lineas = Clases.Linea.ConsultarLineasPorPlanta(idPlanta);

            cbLinea.DataSource = lineas;
            cbLinea.ValueMember = "Id";
            cbLinea.DisplayMember = "Nombre";
            cbLinea.Enabled = true;

            if (idLineaSeleccionar > 0)
            {
                cbLinea.SelectedValue = idLineaSeleccionar;
            }
            else
            {
                cbLinea.SelectedIndex = -1;
            }
        }

        private void ActualizarEstadoFotoSegunImagenActual()
        {
            if (pictureBox1.Image == null)
            {
                ActualizarEstadoFoto("SIN_FOTO");
                return;
            }

            if (capturaDesdeCamara)
            {
                ActualizarEstadoFoto("CAPTURADA");
                return;
            }

            if (trabajadorActual != null &&
                !string.IsNullOrWhiteSpace(rutaFotoSeleccionada) &&
                rutaFotoSeleccionada == trabajadorActual.RutaFoto)
            {
                ActualizarEstadoFoto("EXISTENTE");
                return;
            }

            if (!string.IsNullOrWhiteSpace(rutaFotoSeleccionada))
            {
                ActualizarEstadoFoto("SELECCIONADA");
                return;
            }

            ActualizarEstadoFoto("EXISTENTE");
        }

        private void MostrarImagenEnPictureBox(Image imagen)
        {
            if (pictureBox1.Image != null)
            {
                Image imagenAnterior = pictureBox1.Image;
                pictureBox1.Image = null;
                imagenAnterior.Dispose();
            }

            pictureBox1.Image = imagen;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private int ObtenerIdCombo(ComboBox combo)
        {
            if (combo.SelectedValue == null)
                return 0;

            if (combo.SelectedValue is int id)
                return id;

            if (int.TryParse(combo.SelectedValue.ToString(), out int valor))
                return valor;

            return 0;
        }

        private bool HayCambiosSinGuardar()
        {
            // Si es trabajador nuevo
            if (trabajadorActual == null)
            {
                return
                    !string.IsNullOrWhiteSpace(txtNoReloj.Text) ||
                    !string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    cbLocalidad.SelectedIndex != -1 ||
                    cbTurno.SelectedIndex != -1 ||
                    cbPlanta.SelectedIndex != -1 ||
                    cbLinea.SelectedIndex != -1 ||
                    pictureBox1.Image != null ||
                    capturaDesdeCamara ||
                    !string.IsNullOrWhiteSpace(rutaFotoSeleccionada) ||
                    (camara != null && camara.IsRunning);
            }

            // Si es modificación de trabajador existente
            string noRelojActual = txtNoReloj.Text.Trim();
            string nombreActual = txtNombre.Text.Trim().ToUpper();

            string noRelojOriginal = trabajadorActual.NoReloj?.Trim() ?? "";
            string nombreOriginal = trabajadorActual.Nombre?.Trim().ToUpper() ?? "";

            if (noRelojActual != noRelojOriginal)
                return true;

            if (nombreActual != nombreOriginal)
                return true;

            if (ObtenerIdCombo(cbLocalidad) != trabajadorActual.IdLocalidad)
                return true;

            if (ObtenerIdCombo(cbTurno) != trabajadorActual.IdTurno)
                return true;

            if (ObtenerIdCombo(cbPlanta) != trabajadorActual.IdPlanta)
                return true;

            if (ObtenerIdCombo(cbLinea) != trabajadorActual.IdLinea)
                return true;

            if (capturaDesdeCamara)
                return true;

            if (!string.IsNullOrWhiteSpace(rutaFotoSeleccionada) &&
                rutaFotoSeleccionada != trabajadorActual.RutaFoto)
                return true;

            if (camara != null && camara.IsRunning)
                return true;

            return false;
        }

        private void LimpiarErroresValidacion()
        {
            epValidacion.SetError(txtNoReloj, "");
            epValidacion.SetError(txtNombre, "");
            epValidacion.SetError(cbLocalidad, "");
            epValidacion.SetError(cbTurno, "");
            epValidacion.SetError(cbPlanta, "");
            epValidacion.SetError(cbLinea, "");
            epValidacion.SetError(pictureBox1, "");
        }

        private void ConfigurarModoVentana()
        {
            if (trabajadorActual == null)
            {
                lblTitulo.Text = "Registrar trabajador";
                Text = "Registrar trabajador";
                btnGuardar.Text = "Guardar";
            }
            else
            {
                lblTitulo.Text = "Modificar trabajador";
                Text = "Modificar trabajador";
                btnGuardar.Text = "Guardar cambios";
            }
        }

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(txtNoReloj, "Capture el número de reloj del trabajador.");
            ttAyuda.SetToolTip(txtNombre, "Capture el nombre completo del trabajador.");
            ttAyuda.SetToolTip(cbLocalidad, "Seleccione la localidad del trabajador.");
            ttAyuda.SetToolTip(cbTurno, "Seleccione el turno del trabajador.");
            ttAyuda.SetToolTip(cbPlanta, "Seleccione la planta del trabajador.");
            ttAyuda.SetToolTip(cbLinea, "Seleccione la línea del trabajador.");

            ttAyuda.SetToolTip(cbCamaras, "Seleccione la cámara que desea utilizar.");
            ttAyuda.SetToolTip(
    pictureBox1,
    "Vista previa de la fotografía. Doble clic o clic derecho para más opciones.");
            ttAyuda.SetToolTip(btnIniciarCamara, "Inicia o detiene la cámara.");
            ttAyuda.SetToolTip(btnCapturar, "Captura la imagen actual de la cámara.");
            ttAyuda.SetToolTip(btnSeleccionar, "Selecciona una imagen desde el equipo.");

            ttAyuda.SetToolTip(btnGuardar, "Guarda la información del trabajador.");
            ttAyuda.SetToolTip(btnRegresar, "Cierra esta ventana sin guardar cambios.");
            ttAyuda.SetToolTip(btnQuitarFoto, "Quita la fotografía actual para capturar o seleccionar otra.");
            ttAyuda.SetToolTip(btnRecargarCamaras, "Recargar cámaras disponibles.");
            ttAyuda.SetToolTip(
            lblCamposObligatorios,
            "Los campos marcados con * son necesarios para guardar el trabajador.");
        }

        private void LimpiarFotoActual()
        {
            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: false);
            }

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            if (fotoCapturada != null)
            {
                fotoCapturada.Dispose();
                fotoCapturada = null;
            }

            capturaDesdeCamara = false;
            rutaFotoSeleccionada = null;

            ActualizarEstadoFoto("SIN_FOTO");
            ActualizarBotonesCamara();

            epValidacion.SetError(pictureBox1, "Capture o seleccione una fotografía.");

            MostrarEstado("Fotografía quitada. Capture o seleccione una nueva.", true);
        }

        private void ActualizarEstadoFoto(string estado)
        {
            switch (estado)
            {
                case "SIN_FOTO":
                    lblEstadoFoto.Text = "Foto: sin fotografía";
                    lblEstadoFoto.ForeColor = Color.DimGray;
                    break;

                case "CAMARA_ACTIVA":
                    lblEstadoFoto.Text = "Foto: cámara activa";
                    lblEstadoFoto.ForeColor = Color.FromArgb(0, 70, 140);
                    break;

                case "CAPTURADA":
                    lblEstadoFoto.Text = "Foto: capturada desde cámara";
                    lblEstadoFoto.ForeColor = Color.FromArgb(0, 120, 40);
                    break;

                case "SELECCIONADA":
                    lblEstadoFoto.Text = "Foto: imagen seleccionada";
                    lblEstadoFoto.ForeColor = Color.FromArgb(0, 120, 40);
                    break;

                case "EXISTENTE":
                    lblEstadoFoto.Text = "Foto: fotografía existente";
                    lblEstadoFoto.ForeColor = Color.FromArgb(0, 120, 40);
                    break;

                default:
                    lblEstadoFoto.Text = "Foto: sin fotografía";
                    lblEstadoFoto.ForeColor = Color.DimGray;
                    break;
            }
        }

        private void GuardarImagenComoJpgConFondoBlanco(Image imagenOriginal, string rutaDestino)
        {
            int maxAncho = 800;
            int maxAlto = 800;

            double proporcionAncho = (double)maxAncho / imagenOriginal.Width;
            double proporcionAlto = (double)maxAlto / imagenOriginal.Height;
            double proporcion = Math.Min(proporcionAncho, proporcionAlto);

            int nuevoAncho = (int)(imagenOriginal.Width * proporcion);
            int nuevoAlto = (int)(imagenOriginal.Height * proporcion);

            if (nuevoAncho <= 0) nuevoAncho = 1;
            if (nuevoAlto <= 0) nuevoAlto = 1;

            using Bitmap imagenFinal = new Bitmap(
                nuevoAncho,
                nuevoAlto,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            imagenFinal.SetResolution(96, 96);

            using (Graphics g = Graphics.FromImage(imagenFinal))
            {
                g.Clear(Color.White);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                g.DrawImage(imagenOriginal, 0, 0, nuevoAncho, nuevoAlto);
            }

            imagenFinal.Save(rutaDestino, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private Image CargarImagenSinBloquearArchivo(string ruta)
        {
            using FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            using Image imagenTemporal = Image.FromStream(fs);

            return new Bitmap(imagenTemporal);
        }
        private void CargarCamaras()
        {
            try
            {
                dispositivosVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (dispositivosVideo.Count == 0)
                {
                    cbCamaras.DataSource = null;
                    cbCamaras.Enabled = false;
                    btnIniciarCamara.Enabled = false;
                    btnCapturar.Enabled = false;

                    ActualizarEstadoFotoSegunImagenActual();

                    MostrarEstado("No se detectaron cámaras. Puede seleccionar una imagen.", true);

                    return;
                }

                cbCamaras.DataSource = dispositivosVideo;
                cbCamaras.DisplayMember = "Name";
                cbCamaras.SelectedIndex = 0;

                cbCamaras.Enabled = true;
                btnIniciarCamara.Enabled = true;
                btnCapturar.Enabled = true;
            }
            catch (Exception ex)
            {
                cbCamaras.DataSource = null;
                cbCamaras.Enabled = false;
                btnIniciarCamara.Enabled = false;
                btnCapturar.Enabled = false;

                ActualizarEstadoFotoSegunImagenActual();

                MostrarEstado("No se pudieron cargar las cámaras.", true);

                MessageBox.Show(
                    "No se pudieron cargar las cámaras.\n\n" +
                    "Detalle: " + ex.Message,
                    "Error de cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        private void CargarTrabajador()
        {
            if (trabajadorActual == null)
                return;

            cargandoDatos = true;

            txtNoReloj.Text = trabajadorActual.NoReloj;
            txtNoReloj.Enabled = false;

            txtNombre.Text = trabajadorActual.Nombre;
            cbLocalidad.SelectedValue = trabajadorActual.IdLocalidad;
            cbTurno.SelectedValue = trabajadorActual.IdTurno;

            cbPlanta.SelectedValue = trabajadorActual.IdPlanta;

            cargandoDatos = false;

            CargarLineasPorPlanta(
                trabajadorActual.IdPlanta,
                trabajadorActual.IdLinea);

            if (!string.IsNullOrEmpty(trabajadorActual.RutaFoto) &&
                File.Exists(trabajadorActual.RutaFoto))
            {
                MostrarImagenEnPictureBox(
                    CargarImagenSinBloquearArchivo(trabajadorActual.RutaFoto));

                rutaFotoSeleccionada = trabajadorActual.RutaFoto;
                capturaDesdeCamara = false;

                ActualizarEstadoFoto("EXISTENTE");
            }
        }


        private void CargarComboBox()
        {
            cbLocalidad.DataSource = Clases.Localidad.ConsultarLocalidades(string.Empty);
            cbLocalidad.ValueMember = "Id";
            cbLocalidad.DisplayMember = "Nombre";

            cbTurno.DataSource = Clases.Turno.ConsultarTurnos(string.Empty);
            cbTurno.ValueMember = "Id";
            cbTurno.DisplayMember = "Nombre";

            cbPlanta.DataSource = Clases.Planta.ConsultarPlantas(string.Empty);
            cbPlanta.ValueMember = "Id";
            cbPlanta.DisplayMember = "Nombre";

            if (trabajadorActual != null)
            {
                CargarTrabajador();
            }
            else
            {
                cbLocalidad.SelectedIndex = -1;
                cbTurno.SelectedIndex = -1;
                cbPlanta.SelectedIndex = -1;
                cbLinea.DataSource = null;
                cbLinea.Enabled = false;
            }
        }
        private bool ValidarInformacion()
        {
            LimpiarErroresValidacion();

            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtNoReloj.Text))
            {
                epValidacion.SetError(txtNoReloj, "Capture el número de reloj.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                epValidacion.SetError(txtNombre, "Capture el nombre del trabajador.");
                esValido = false;
            }

            if (cbLocalidad.SelectedIndex == -1 || cbLocalidad.SelectedValue == null)
            {
                epValidacion.SetError(cbLocalidad, "Seleccione la localidad.");
                esValido = false;
            }

            if (cbTurno.SelectedIndex == -1 || cbTurno.SelectedValue == null)
            {
                epValidacion.SetError(cbTurno, "Seleccione el turno.");
                esValido = false;
            }

            if (cbPlanta.SelectedIndex == -1 || cbPlanta.SelectedValue == null)
            {
                epValidacion.SetError(cbPlanta, "Seleccione la planta.");
                esValido = false;
            }

            if (cbLinea.SelectedIndex == -1 || cbLinea.SelectedValue == null)
            {
                epValidacion.SetError(cbLinea, "Seleccione la línea.");
                esValido = false;
            }

            if (pictureBox1.Image == null)
            {
                epValidacion.SetError(pictureBox1, "Capture o seleccione una fotografía.");
                esValido = false;
            }

            if (!esValido)
            {
                MessageBox.Show(
                    "Hay información incompleta. Revise los campos marcados.",
                    "Información incompleta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return esValido;
        }

        private void DetenerCamara(bool limpiarImagen)
        {
            if (camara != null)
            {
                if (camara.IsRunning)
                {
                    camara.NewFrame -= Camara_NewFrame; // 🔴 CLAVE
                    camara.SignalToStop();
                    camara.WaitForStop();
                }

                camara = null; // 🔴 LIBERAR
            }

            if (limpiarImagen && pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            btnIniciarCamara.Text = "Iniciar cámara";
            ActualizarBotonesCamara();
        }


        private void ResetearUsoCamara()
        {
            DetenerCamara(limpiarImagen: true);

            dispositivosVideo = null;

            if (fotoCapturada != null)
            {
                fotoCapturada.Dispose();
                fotoCapturada = null;
            }

            capturaDesdeCamara = false;
            rutaFotoSeleccionada = null;
        }



        private void btnRegresar_Click(object sender, EventArgs e)
        {
            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: false);
            }

            Close();
        }

        private void cbPlanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            epValidacion.SetError(cbPlanta, "");
        }

        private void cbPlanta_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cargandoDatos)
                return;

            if (cbPlanta.SelectedValue is int idPlanta)
            {
                CargarLineasPorPlanta(idPlanta);
            }
            else
            {
                cbLinea.DataSource = null;
                cbLinea.Enabled = false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (guardando)
                return;

            if (camara != null && camara.IsRunning)
            {
                MessageBox.Show(
                    "La cámara está activa. Primero capture la fotografía o detenga la cámara antes de guardar.",
                    "Cámara activa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!ValidarInformacion())
                return;

            guardando = true;
            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            MostrarEstado("Guardando trabajador...");

            try
            {
                Clases.Trabajador trabajador = trabajadorActual ?? new Clases.Trabajador();

                trabajador.NoReloj = txtNoReloj.Text.Trim();
                trabajador.Nombre = txtNombre.Text.Trim().ToUpper();

                Clases.RutasArchivos.CrearCarpetasTrabajador(trabajador.NoReloj!);

                string rutaDestino = Clases.RutasArchivos.ObtenerRutaFotoPerfil(trabajador.NoReloj!);

                // =========================
                // MANEJO CORRECTO DE FOTO
                // =========================

                if (capturaDesdeCamara && fotoCapturada != null)
                {
                    GuardarImagenComoJpgConFondoBlanco(fotoCapturada, rutaDestino);

                    trabajador.RutaFoto = rutaDestino;
                }
                else if (!string.IsNullOrEmpty(rutaFotoSeleccionada))
                {
                    if (rutaFotoSeleccionada != rutaDestino)
                    {
                        using Image imagenSeleccionada = CargarImagenSinBloquearArchivo(rutaFotoSeleccionada);

                        GuardarImagenComoJpgConFondoBlanco(imagenSeleccionada, rutaDestino);
                    }

                    trabajador.RutaFoto = rutaDestino;
                }
                else if (trabajadorActual != null)
                {
                    trabajador.RutaFoto = trabajadorActual.RutaFoto;
                }
                else
                {
                    MessageBox.Show(
                        "Debe capturar o seleccionar una fotografía.",
                        "Foto requerida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // =========================
                // DATOS RESTANTES
                // =========================

                trabajador.IdLocalidad = (int)cbLocalidad.SelectedValue!;
                trabajador.IdTurno = (int)cbTurno.SelectedValue!;
                trabajador.IdLinea = (int)cbLinea.SelectedValue!;

                Clases.Mensaje respuesta = trabajador.GuardarTrabajador();

                if (respuesta.Id == 1 || respuesta.Id == 3)
                {
                    MessageBox.Show(
                        respuesta.Nombre,
                        "Resultado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    NoRelojGuardado = txtNoReloj.Text.Trim();

                    guardadoCorrectamente = true;

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        respuesta.Nombre,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    MostrarEstado("No se pudo guardar el trabajador.", true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al guardar el trabajador.\n\n" +
                    "Detalle: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                MostrarEstado("Ocurrió un error al guardar.", true);
            }
            finally
            {
                if (!guardadoCorrectamente)
                {
                    guardando = false;
                    btnGuardar.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            // Si la cámara está activa, detenerla y limpiar imagen
            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: true);
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar fotografía",
                Filter = "Imágenes (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Image imagenTemporal = CargarImagenSinBloquearArchivo(ofd.FileName);

                MostrarImagenEnPictureBox(imagenTemporal);

                rutaFotoSeleccionada = ofd.FileName;
                capturaDesdeCamara = false;

                if (fotoCapturada != null)
                {
                    fotoCapturada.Dispose();
                    fotoCapturada = null;
                }

                ActualizarEstadoFoto("SELECCIONADA");
                epValidacion.SetError(pictureBox1, "");

                MostrarEstado("Imagen seleccionada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar la imagen seleccionada.\n\n" +
                    "Verifique que el archivo sea una imagen válida JPG, PNG o BMP.\n\n" +
                    "Detalle: " + ex.Message,
                    "Imagen no válida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                MostrarEstado("No se pudo cargar la imagen seleccionada.", true);
                rutaFotoSeleccionada = null;
                capturaDesdeCamara = false;
            }
        }
        private void TrabajadoresVentana_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (guardadoCorrectamente)
                return;

            if (!HayCambiosSinGuardar())
                return;

            DialogResult respuesta = MessageBox.Show(
                "Hay información capturada sin guardar.\n\n" +
                "¿Desea cerrar la ventana y descartar los cambios?",
                "Cambios sin guardar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (respuesta != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: false);
            }
        }



        private void Camara_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            try
            {
                if (IsDisposed || pictureBox1.IsDisposed)
                {
                    frame.Dispose();
                    return;
                }

                if (pictureBox1.InvokeRequired)
                {
                    pictureBox1.BeginInvoke(new Action(() =>
                    {
                        if (IsDisposed || pictureBox1.IsDisposed)
                        {
                            frame.Dispose();
                            return;
                        }

                        MostrarImagenEnPictureBox(frame);
                    }));
                }
                else
                {
                    MostrarImagenEnPictureBox(frame);
                }
            }
            catch
            {
                frame.Dispose();
            }
        }

        private void btnTomar_Click(object sender, EventArgs e)
        {
            // Si la cámara ya está encendida, la detenemos
            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: true);

                capturaDesdeCamara = false;

                if (fotoCapturada != null)
                {
                    fotoCapturada.Dispose();
                    fotoCapturada = null;
                }

                if (string.IsNullOrWhiteSpace(rutaFotoSeleccionada))
                    ActualizarEstadoFoto("SIN_FOTO");
                else
                    ActualizarEstadoFoto("SELECCIONADA");

                ActualizarBotonesCamara();

                MostrarEstado("Cámara detenida.");

                return;
            }

            // Validar que exista selección
            if (cbCamaras.SelectedItem == null)
            {
                MessageBox.Show(
                    "Seleccione una cámara.",
                    "Cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                MostrarEstado("Seleccione una cámara para iniciar.", true);
                return;
            }

            // Obtener cámara seleccionada
            FilterInfo camaraSeleccionada = (FilterInfo)cbCamaras.SelectedItem;

            camara = new VideoCaptureDevice(camaraSeleccionada.MonikerString);
            camara.NewFrame += Camara_NewFrame;
            camara.Start();

            capturaDesdeCamara = false;

            if (fotoCapturada != null)
            {
                fotoCapturada.Dispose();
                fotoCapturada = null;
            }

            ActualizarBotonesCamara();
            ActualizarEstadoFoto("CAMARA_ACTIVA");

            MostrarEstado("Cámara activa. Capture la fotografía antes de guardar.", true);
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (camara == null || !camara.IsRunning)
            {
                MessageBox.Show(
                    "La cámara no está activa.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            fotoCapturada = (Bitmap)pictureBox1.Image.Clone();
            capturaDesdeCamara = true;

            rutaFotoSeleccionada = null;

            // Detiene cámara pero NO borra la imagen
            DetenerCamara(limpiarImagen: false);
            ActualizarEstadoFoto("CAPTURADA");
            ActualizarBotonesCamara();

            epValidacion.SetError(pictureBox1, "");

            MostrarEstado("Fotografía capturada correctamente.");
            ActualizarBotonesCamara();
            epValidacion.SetError(pictureBox1, "");

        }

        private void TrabajadoresVentana_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResetearUsoCamara();
        }

        private void cbLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            epValidacion.SetError(cbLocalidad, "");
        }

        private void txtNoReloj_TextChanged(object sender, EventArgs e)
        {
            epValidacion.SetError(txtNoReloj, "");
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            int posicion = txtNombre.SelectionStart;

            string textoMayusculas = txtNombre.Text.ToUpper();

            if (txtNombre.Text != textoMayusculas)
            {
                txtNombre.Text = textoMayusculas;
                txtNombre.SelectionStart = Math.Min(posicion, txtNombre.Text.Length);
            }
        }

        private void cbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            epValidacion.SetError(cbTurno, "");
        }

        private void cbLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            epValidacion.SetError(cbLinea, "");
        }

        private void txtNoReloj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = true;
                return;
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = true;
            }
        }

        private void MostrarVistaPreviaFoto()
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show(
                    "No hay fotografía para mostrar.",
                    "Vista previa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            Form ventanaFoto = new Form
            {
                Text = "Vista previa de fotografía",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(650, 650),
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            PictureBox pbVista = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black,
                Image = new Bitmap(pictureBox1.Image)
            };

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Dock = DockStyle.Bottom,
                Height = 42
            };

            btnCerrar.Click += (s, e) => ventanaFoto.Close();

            ventanaFoto.Controls.Add(pbVista);
            ventanaFoto.Controls.Add(btnCerrar);

            ventanaFoto.FormClosed += (s, e) =>
            {
                if (pbVista.Image != null)
                {
                    pbVista.Image.Dispose();
                    pbVista.Image = null;
                }
            };

            ventanaFoto.ShowDialog(this);
        }

        private void ComboBox_KeyPress_Avanzar(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SelectNextControl((Control)sender, true, true, true, true);
                e.Handled = true;
            }
        }

        private void ActualizarBotonesCamara()
        {
            bool camaraActiva = camara != null && camara.IsRunning;

            btnCapturar.Enabled = camaraActiva;
            btnIniciarCamara.Text = camaraActiva ? "Detener cámara" : "Iniciar cámara";
        }

        private void btnQuitarFoto_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
    "¿Desea quitar la fotografía actual?",
    "Quitar fotografía",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            LimpiarFotoActual();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            MostrarVistaPreviaFoto();
        }

        private void tsmVerFotoGrande_Click(object sender, EventArgs e)
        {
            MostrarVistaPreviaFoto();
        }

        private void tsmSeleccionarFoto_Click(object sender, EventArgs e)
        {
            btnSeleccionar_Click(sender, e);
        }

        private void tsmQuitarFoto_Click(object sender, EventArgs e)
        {
            btnQuitarFoto_Click(sender, e);
        }

        private void cmsFoto_Opening(object sender, CancelEventArgs e)
        {
            bool hayFoto = pictureBox1.Image != null;

            tsmVerFotoGrande.Enabled = hayFoto;
            tsmQuitarFoto.Enabled = hayFoto;
        }

        private void btnRecargarCamaras_Click(object sender, EventArgs e)
        {
            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: false);
            }

            cbCamaras.DataSource = null;
            dispositivosVideo = null;

            CargarCamaras();
            ActualizarBotonesCamara();

            if (cbCamaras.Items.Count > 0)
            {
                MostrarEstado("Cámaras recargadas correctamente.");
            }
            else
            {
                MostrarEstado("No se detectaron cámaras. Puede seleccionar una imagen.", true);
            }
        }
    }
}