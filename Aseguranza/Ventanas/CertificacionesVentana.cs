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

            RefrescarVentana();
        }
        private void dgvCertificaciones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void RefrescarVentana()
        {
            MostrarDatosTrabajador();
            CargarCertificaciones();
            SeleccionarPrimero();
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

            // Cargar imagen de forma segura
            if (!string.IsNullOrWhiteSpace(trabajadorActual.RutaFoto) &&
                File.Exists(trabajadorActual.RutaFoto))
            {
                using var fs = new FileStream(
                    trabajadorActual.RutaFoto,
                    FileMode.Open,
                    FileAccess.Read);

                pictureBox1.Image = Image.FromStream(fs);
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        // =====================================================
        // CARGAR CERTIFICACIONES
        // =====================================================
        private void CargarCertificaciones(string textoBuscar = "")
        {
            dgvCertificaciones.DataSource = null;
            dgvCertificaciones.Columns.Clear();
            dgvCertificaciones.AutoGenerateColumns = true;

            dgvCertificaciones.DataSource =
                Certificacion.ConsultarCertificacionesPorTrabajador(
                    trabajadorActual.Id,
                    textoBuscar);

            dgvCertificaciones.AutoResizeColumns();
            OcultarColumnas();

            PintarCertificaciones();

            if (dgvCertificaciones.Rows.Count > 0)
            {
                dgvCertificaciones.ClearSelection();
                dgvCertificaciones.Rows[0].Selected = true;

                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
                btnImprimir.Enabled = true;
                btnAnular.Enabled = true;
            }
            else
            {
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
                btnImprimir.Enabled = false;
                btnAnular.Enabled = false;
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
                    row.DefaultCellStyle.BackColor = Color.Orange;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
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

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {

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
                RefrescarVentana();
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

            if (MessageBox.Show(
                "¿Deseas eliminar esta certificación?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            int id = Convert.ToInt32(dgvCertificaciones.CurrentRow.Cells["Id"].Value);

            Clases.Certificacion.BorrarCertificacion(id);

            CargarCertificaciones(txtBuscar.Text.Trim());
        }

        private void dgvCertificaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
                return;

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
                RefrescarVentana();
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CargarCertificaciones(txtBuscar.Text.Trim());
                e.Handled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Ventanas.CredencialCertificacion ventana = new Ventanas.CredencialCertificacion(trabajadorActual);
            ventana.ShowDialog();
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
                CargarCertificaciones(txtBuscar.Text.Trim());
            }
        }

        private void dgvCertificaciones_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            PintarCertificaciones();
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
    }
}