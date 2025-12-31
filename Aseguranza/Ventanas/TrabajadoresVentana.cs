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
            CargarComboBox();
            CargarCamaras();
        }
        private void CargarCamaras()
        {
            dispositivosVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (dispositivosVideo.Count == 0)
            {
                MessageBox.Show("No se detectaron cámaras.");
                return;
            }

            cbCamaras.DataSource = dispositivosVideo;
            cbCamaras.DisplayMember = "Name";
            cbCamaras.SelectedIndex = 0;
        }
        private void CargarTrabajador()
        {
            txtNoReloj.Text = trabajadorActual!.NoReloj.ToString();
            txtNoReloj.Enabled = false;

            txtNombre.Text = trabajadorActual.Nombre;
            cbLocalidad.SelectedValue = trabajadorActual.IdLocalidad;
            cbTurno.SelectedValue = trabajadorActual.IdTurno;
            cbPlanta.SelectedValue = trabajadorActual.IdPlanta;
            cbLinea.SelectedValue = trabajadorActual.IdLinea;

            // Si el trabajador ya tiene foto guardada
            if (!string.IsNullOrEmpty(trabajadorActual.RutaFoto) &&
                File.Exists(trabajadorActual.RutaFoto))
            {
                // Liberar imagen previa si existiera
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

                using (var fs = new FileStream(
                    trabajadorActual.RutaFoto,
                    FileMode.Open,
                    FileAccess.Read))
                {
                    pictureBox1.Image = Image.FromStream(fs);
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                // 🔴 CLAVE: marcar que ya existe una foto válida
                rutaFotoSeleccionada = trabajadorActual.RutaFoto;
                capturaDesdeCamara = false;
            }
        }


        private void CargarComboBox()
        {
            cbLocalidad.DataSource = Clases.Localidad.ConsultarLocalidades(txtNombre.Text);
            cbLocalidad.ValueMember = "Id";
            cbLocalidad.DisplayMember = "Nombre";
            cbTurno.DataSource = Clases.Turno.ConsultarTurnos(txtNombre.Text);
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
            if (string.IsNullOrWhiteSpace(txtNoReloj.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                cbLocalidad.SelectedIndex == -1 ||
                cbTurno.SelectedIndex == -1 ||
                cbPlanta.SelectedIndex == -1 ||
                cbLinea.SelectedIndex == -1 ||
                pictureBox1.Image == null)
            {
                MessageBox.Show(
                    "Por favor, complete toda la información requerida.",
                    "Información incompleta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
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
            this.Close();
        }

        private void cbPlanta_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbPlanta_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbPlanta.SelectedValue is int idPlanta)
            {
                cbLinea.DataSource = Clases.Linea.ConsultarLineasPorPlanta(idPlanta);
                cbLinea.ValueMember = "Id";
                cbLinea.DisplayMember = "Nombre";
                cbLinea.SelectedIndex = -1;
                cbLinea.Enabled = true;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!ValidarInformacion())
                return;

            Clases.Trabajador trabajador = trabajadorActual ?? new Clases.Trabajador();

            trabajador.NoReloj = int.Parse(txtNoReloj.Text.Trim());
            trabajador.Nombre = txtNombre.Text.Trim().ToUpper();

            string carpetaFotos = @"C:\Aseguranza\Fotos\";
            Directory.CreateDirectory(carpetaFotos);

            string nombreArchivo = $"{trabajador.NoReloj}_{trabajador.Nombre}.jpg";
            string rutaDestino = Path.Combine(carpetaFotos, nombreArchivo);

            // =========================
            // MANEJO CORRECTO DE FOTO
            // =========================

            if (capturaDesdeCamara && fotoCapturada != null)
            {
                // Foto nueva desde cámara
                fotoCapturada.Save(
                    rutaDestino,
                    System.Drawing.Imaging.ImageFormat.Jpeg);

                trabajador.RutaFoto = rutaDestino;
            }
            else if (!string.IsNullOrEmpty(rutaFotoSeleccionada))
            {
                // Foto seleccionada o existente
                if (rutaFotoSeleccionada != rutaDestino)
                {
                    File.Copy(rutaFotoSeleccionada, rutaDestino, true);
                }

                trabajador.RutaFoto = rutaDestino;
            }
            else if (trabajadorActual != null)
            {
                // 🔴 CLAVE: no cambió la foto, conservar la existente
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

            if (respuesta.Id == 1 || respuesta.Id == 2)
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Resultado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

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
                Filter = "Imágenes (*.jpg;*.png)|*.jpg;*.png"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                rutaFotoSeleccionada = ofd.FileName;
                capturaDesdeCamara = false;

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }

                using (var fs = new FileStream(rutaFotoSeleccionada, FileMode.Open, FileAccess.Read))
                {
                    pictureBox1.Image = Image.FromStream(fs);
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        private void TrabajadoresVentana_FormClosing(object sender, FormClosingEventArgs e)
        {

        }



        private void Camara_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            pictureBox1.Image = frame;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnTomar_Click(object sender, EventArgs e)
        {
            // Si la cámara ya está encendida, la detenemos
            if (camara != null && camara.IsRunning)
            {
                DetenerCamara(limpiarImagen: true);
                return;
            }

            // Validar que exista selección
            if (cbCamaras.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una cámara.");
                return;
            }

            // Obtener cámara seleccionada
            FilterInfo camaraSeleccionada = (FilterInfo)cbCamaras.SelectedItem;

            camara = new VideoCaptureDevice(camaraSeleccionada.MonikerString);
            camara.NewFrame += Camara_NewFrame;
            camara.Start();

            btnIniciarCamara.Text = "Detener cámara";
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

            // Detiene cámara pero NO borra la imagen
            DetenerCamara(limpiarImagen: false);

        }

        private void TrabajadoresVentana_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResetearUsoCamara();
        }
    }
}