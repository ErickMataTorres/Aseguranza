using System.Linq;
using System.ComponentModel;
using Aseguranza.Clases;
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
        private readonly Font _boldFont = new Font("Arial", 9F, FontStyle.Bold);

        public CertificacionesVentana(Trabajador trabajador)
        {
            InitializeComponent();
            trabajadorActual = trabajador;
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
            dgvCertificaciones.AllowUserToAddRows = false;
            dgvCertificaciones.AllowUserToDeleteRows = false;
            dgvCertificaciones.AllowUserToResizeRows = false;

            dgvCertificaciones.ReadOnly = true;
            dgvCertificaciones.MultiSelect = false;
            dgvCertificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCertificaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCertificaciones.RowHeadersVisible = false;

            dgvCertificaciones.EnableHeadersVisualStyles = false;
            dgvCertificaciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 70, 140);
            dgvCertificaciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCertificaciones.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvCertificaciones.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCertificaciones.ColumnHeadersHeight = 32;

            dgvCertificaciones.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvCertificaciones.DefaultCellStyle.ForeColor = Color.Black;
            dgvCertificaciones.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvCertificaciones.GridColor = Color.LightGray;
            dgvCertificaciones.RowTemplate.Height = 28;
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

                    MessageBox.Show(
                        "No se pudo cargar la foto del trabajador.\n\n" +
                        "Ruta: " + trabajadorActual.RutaFoto + "\n\n" +
                        "Detalle: " + ex.Message,
                        "Foto no disponible",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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
                btnAnular.BackColor = SystemColors.Control;

                lblInfoAnulacion.Visible = false;
                lblInfoAnulacion.Text = "";
            }

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

                bool estaAnulada = false;

                if (dgvCertificaciones.Columns.Contains("EstaAnulada"))
                {
                    object valorAnulada = row.Cells["EstaAnulada"].Value;

                    if (valorAnulada != null && valorAnulada != DBNull.Value)
                    {
                        string valorTexto = valorAnulada.ToString()!.Trim().ToLower();

                        estaAnulada =
                            valorTexto == "1" ||
                            valorTexto == "true" ||
                            valorTexto == "si" ||
                            valorTexto == "sí";
                    }
                }

                if (estaAnulada)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(126, 87, 194); // Morado
                    row.DefaultCellStyle.ForeColor = Color.White;
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(94, 53, 177); // Morado más fuerte
                    row.DefaultCellStyle.SelectionForeColor = Color.White;
                    row.DefaultCellStyle.Font = _boldFont;
                    continue;
                }

                if (!dgvCertificaciones.Columns.Contains("DiasRestantes"))
                    continue;

                object valor = row.Cells["DiasRestantes"].Value;

                if (valor == null || valor == DBNull.Value)
                    continue;

                int dias = Convert.ToInt32(valor);

                if (dias < 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.SelectionBackColor = Color.IndianRed;
                }
                else if (dias <= 30)
                {
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                    row.DefaultCellStyle.SelectionBackColor = Color.Goldenrod;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
                }

                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
                row.DefaultCellStyle.Font = _boldFont;
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
                MessageBox.Show(
                    "Seleccione una certificación.",
                    "Sin selección",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (CertificacionSeleccionadaEstaAnulada())
            {
                MessageBox.Show(
                    "Esta certificación se encuentra anulada. No se puede modificar ni renovar mientras tenga una anulación activa.",
                    "Certificación anulada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

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

            Ventanas.CertificacionesTrabajadorVentana ventana =
                new Ventanas.CertificacionesTrabajadorVentana(certificacion, trabajadorActual);

            ventana.ShowDialog();

            if (ventana.DialogResult == DialogResult.OK)
            {
                RefrescarVentana(idCertificacionSeleccionada);
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            Ventanas.CertificacionesTrabajadorVentana ventana =
                new Ventanas.CertificacionesTrabajadorVentana(null!, trabajadorActual);

            ventana.ShowDialog();

            if (ventana.DialogResult == DialogResult.OK)
            {
                RefrescarVentana();
            }
        }

        private void btnBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione una certificación.",
                    "Sin selección",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (CertificacionSeleccionadaEstaAnulada())
            {
                MessageBox.Show(
                    "Esta certificación se encuentra anulada. Primero debe eliminar la anulación antes de borrar la certificación.",
                    "Certificación anulada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string proceso = "";

            if (dgvCertificaciones.Columns.Contains("Proceso"))
                proceso = dgvCertificaciones.CurrentRow.Cells["Proceso"].Value?.ToString() ?? "";

            DialogResult confirmacion = MessageBox.Show(
                $"¿Deseas eliminar esta certificación?\n\n" +
                $"Proceso: {proceso}\n" +
                $"Trabajador: {trabajadorActual.NoReloj} - {trabajadorActual.Nombre}",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes)
                return;

            int id = Convert.ToInt32(dgvCertificaciones.CurrentRow.Cells["Id"].Value);

            Clases.Mensaje respuesta = Clases.Certificacion.BorrarCertificacion(id);

            if (respuesta.Id == 1)
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Resultado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarCertificaciones(txtBuscar.Text.Trim());
            }
            else
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "No se pudo eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                MessageBox.Show(
                    "No se encontró la información del trabajador.",
                    "Trabajador no encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            ExpedienteTrabajadorVentana ventana = new ExpedienteTrabajadorVentana(
                trabajadorActual.Id,
                trabajadorActual.NoReloj ?? "",
                trabajadorActual.Nombre ?? ""
            );

            ventana.ShowDialog();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione una certificación para anular.",
                    "Sin selección",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int idCertificacion = Convert.ToInt32(
                dgvCertificaciones.CurrentRow.Cells["Id"].Value
            );

            string proceso = dgvCertificaciones.CurrentRow.Cells["Proceso"].Value?.ToString() ?? "";

            CertificacionAnulacionVentana ventana = new CertificacionAnulacionVentana(
                idCertificacion,
                trabajadorActual.NoReloj ?? "",
                trabajadorActual.Nombre ?? "",
                proceso
            );

            ventana.ShowDialog();

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
            if (dgvCertificaciones.CurrentRow == null || dgvCertificaciones.Rows.Count == 0)
            {
                btnAnular.Text = "Anular";
                btnAnular.Enabled = false;
                btnAnular.BackColor = SystemColors.Control;
                return;
            }

            btnAnular.Enabled = true;

            if (CertificacionSeleccionadaEstaAnulada())
            {
                btnAnular.Text = "Ver anulación";
                btnAnular.BackColor = Color.FromArgb(126, 87, 194);
                btnAnular.ForeColor = Color.White;
            }
            else
            {
                btnAnular.Text = "Anular";
                btnAnular.BackColor = SystemColors.Control;
                btnAnular.ForeColor = Color.Black;
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
                MessageBox.Show(
                    "No se encontró el número de reloj del trabajador.",
                    "Información incompleta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var ventana = new Verificaciones(trabajadorActual.NoReloj);
            ventana.ShowDialog();
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