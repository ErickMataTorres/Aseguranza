using Aseguranza.Clases;
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
        }

        private void ExpedienteTrabajadorVentana_Load(object sender, EventArgs e)
        {
            lblTitulo.Text = $"Expediente: {noReloj} - {nombreTrabajador}";

            ConfigurarGrid();
            CargarExpediente();
            CargarCamaras();

            btnIniciarCamara.Text = "Iniciar cámara";
            btnCapturar.Enabled = false;
        }

        private void ConfigurarGrid()
        {
            dgvExpediente.AllowUserToAddRows = false;
            dgvExpediente.AllowUserToDeleteRows = false;
            dgvExpediente.ReadOnly = true;
            dgvExpediente.MultiSelect = false;
            dgvExpediente.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExpediente.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarExpediente()
        {
            DataTable dt = Clases.ExpedienteTrabajador.ConsultarExpedienteTrabajador(idTrabajador);
            dgvExpediente.DataSource = dt;

            if (dgvExpediente.Columns.Contains("Id"))
                dgvExpediente.Columns["Id"].Visible = false;

            if (dgvExpediente.Columns.Contains("IdTrabajador"))
                dgvExpediente.Columns["IdTrabajador"].Visible = false;

            if (dgvExpediente.Columns.Contains("RutaArchivo"))
                dgvExpediente.Columns["RutaArchivo"].HeaderText = "Ubicación";

            if (dgvExpediente.Columns.Contains("NombreOriginal"))
                dgvExpediente.Columns["NombreOriginal"].HeaderText = "Nombre original";

            if (dgvExpediente.Columns.Contains("NombreArchivo"))
                dgvExpediente.Columns["NombreArchivo"].HeaderText = "Archivo guardado";

            if (dgvExpediente.Columns.Contains("Extension"))
                dgvExpediente.Columns["Extension"].HeaderText = "Extensión";

            if (dgvExpediente.Columns.Contains("TipoArchivo"))
                dgvExpediente.Columns["TipoArchivo"].HeaderText = "Tipo";

            if (dgvExpediente.Columns.Contains("Comentario"))
                dgvExpediente.Columns["Comentario"].HeaderText = "Comentario";

            if (dgvExpediente.Columns.Contains("FechaRegistro"))
                dgvExpediente.Columns["FechaRegistro"].HeaderText = "Fecha registro";

            if (dgvExpediente.Columns.Contains("FechaModificacion"))
                dgvExpediente.Columns["FechaModificacion"].HeaderText = "Fecha modificación";
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
                    MessageBox.Show(
                        "No se encontraron cámaras disponibles.",
                        "Cámara",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al cargar cámaras",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FuenteVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            if (pbCamara.InvokeRequired)
            {
                pbCamara.BeginInvoke(new MethodInvoker(delegate
                {
                    pbCamara.Image?.Dispose();
                    pbCamara.Image = frame;
                }));
            }
            else
            {
                pbCamara.Image?.Dispose();
                pbCamara.Image = frame;
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
                    MessageBox.Show(
                        respuesta.Nombre,
                        "Resultado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    txtComentario.Clear();
                    CargarExpediente();
                }
                else
                {
                    if (File.Exists(rutaDestino))
                        File.Delete(rutaDestino);

                    MessageBox.Show(
                        respuesta.Nombre,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch
            {
                if (File.Exists(rutaDestino))
                    File.Delete(rutaDestino);

                throw;
            }
        }


        private void DetenerCamara()
        {
            try
            {
                if (fuenteVideo != null)
                {
                    if (fuenteVideo.IsRunning)
                    {
                        fuenteVideo.SignalToStop();
                        fuenteVideo.WaitForStop();
                    }

                    fuenteVideo.NewFrame -= FuenteVideo_NewFrame;
                    fuenteVideo = null;
                }

                camaraActiva = false;

                btnIniciarCamara.Text = "Iniciar cámara";
                btnCapturar.Enabled = false;
                cbCamaras.Enabled = true;

                pbCamara.Image?.Dispose();
                pbCamara.Image = null;
            }
            catch
            {
                camaraActiva = false;

                btnIniciarCamara.Text = "Iniciar cámara";
                btnCapturar.Enabled = false;
                cbCamaras.Enabled = true;
            }
        }

        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar archivo para expediente",
                Filter = "Archivos permitidos|*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx|Todos los archivos|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
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
                    MessageBox.Show(
                        respuesta.Nombre,
                        "Resultado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    txtComentario.Clear();
                    CargarExpediente();
                }
                else
                {
                    if (File.Exists(rutaDestino))
                        File.Delete(rutaDestino);

                    MessageBox.Show(
                        respuesta.Nombre,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al adjuntar archivo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            string? rutaArchivo = ObtenerRutaArchivoSeleccionado();

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                return;

            if (!File.Exists(rutaArchivo))
            {
                MessageBox.Show(
                    "El archivo no existe en la ubicación guardada.",
                    "Archivo no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

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
                MessageBox.Show(
                    error.Message,
                    "Error al abrir archivo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnReemplazar_Click(object sender, EventArgs e)
        {
            int? idExpediente = ObtenerIdSeleccionado();

            if (idExpediente == null)
                return;

            string? rutaAnterior = ObtenerRutaArchivoSeleccionado();

            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar archivo de reemplazo",
                Filter = "Archivos permitidos|*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.doc;*.docx;*.xls;*.xlsx|Todos los archivos|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
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

                    MessageBox.Show(
                        respuesta.Nombre,
                        "Resultado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    txtComentario.Clear();
                    CargarExpediente();
                }
                else
                {
                    if (File.Exists(rutaDestino))
                        File.Delete(rutaDestino);

                    MessageBox.Show(
                        respuesta.Nombre,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al reemplazar archivo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? idExpediente = ObtenerIdSeleccionado();

            if (idExpediente == null)
                return;

            string? rutaArchivo = ObtenerRutaArchivoSeleccionado();

            DialogResult confirmacion = MessageBox.Show(
                "¿Seguro que desea eliminar este archivo del expediente?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
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
                        MessageBox.Show(
                            "El registro se eliminó del expediente, pero el archivo físico no se pudo borrar. Es posible que esté abierto.",
                            "Advertencia",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                MessageBox.Show(
                    respuesta.Nombre,
                    "Resultado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarExpediente();
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

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int? ObtenerIdSeleccionado()
        {
            if (dgvExpediente.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione un archivo del expediente.",
                    "Sin selección",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return null;
            }

            if (!dgvExpediente.Columns.Contains("Id"))
            {
                MessageBox.Show(
                    "No se encontró la columna Id.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                MessageBox.Show(
                    "Seleccione un archivo del expediente.",
                    "Sin selección",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return null;
            }

            if (!dgvExpediente.Columns.Contains("RutaArchivo"))
            {
                MessageBox.Show(
                    "No se encontró la columna RutaArchivo.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                MessageBox.Show(
                    "No hay cámaras disponibles.",
                    "Cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cbCamaras.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Seleccione una cámara.",
                    "Cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

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
                cbCamaras.Enabled = false;
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al iniciar cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (pbCamara.Image == null)
            {
                MessageBox.Show(
                    "No hay imagen de la cámara para capturar.",
                    "Cámara",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using Bitmap fotoCapturada = new Bitmap(pbCamara.Image);

                string nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmmss}_FotoExpediente.jpg";

                GuardarImagenEnExpediente(
                    fotoCapturada,
                    nombreArchivo,
                    txtComentario.Text.Trim()
                );
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al capturar foto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Seleccionar imagen para expediente",
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

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
                MessageBox.Show(
                    error.Message,
                    "Error al seleccionar imagen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
    }
}