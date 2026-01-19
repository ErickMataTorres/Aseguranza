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
            MostrarDatosTrabajador();
            CargarCertificaciones();

            dgvCertificaciones.DataBindingComplete += dgvCertificaciones_DataBindingComplete!;
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
            dgvCertificaciones.DataSource =
                Certificacion.ConsultarCertificacionesPorTrabajador(
                    trabajadorActual.Id,
                    textoBuscar);

            dgvCertificaciones.AutoResizeColumns();
            OcultarColumnas();

            if (dgvCertificaciones.Rows.Count > 0)
            {
                dgvCertificaciones.ClearSelection();
                dgvCertificaciones.Rows[0].Selected = true;
            }

            txtBuscar.Focus();

            PintarCertificaciones();
        }

        private void OcultarColumnas()
        {
            string[] columnasOcultas =
            {
                "Id",
                "IdProceso",
                "IdTrabajador",
                "IdCertificador"
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
            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.Cells["DiasRestantes"].Value == null)
                    continue;

                int dias = Convert.ToInt32(row.Cells["DiasRestantes"].Value);

                if (dias < 0)
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                else if (dias <= 30)
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                else
                    row.DefaultCellStyle.BackColor = Color.LightGreen;

                row.DefaultCellStyle.ForeColor = Color.Black;
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
            Clases.Certificacion certificacion = new Clases.Certificacion();
            certificacion.Id = int.Parse(dgvCertificaciones.CurrentRow.Cells["Id"].Value.ToString()!);
            certificacion.FechaCertificacion = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaCertificacion"].Value.ToString()!);
            certificacion.FechaVencimiento = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaVencimiento"].Value.ToString()!);
            certificacion.IdProceso = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdProceso"].Value.ToString()!);
            certificacion.Comentario = dgvCertificaciones.CurrentRow.Cells["Comentario"].Value.ToString()!;
            Ventanas.CertificacionesTrabajadorVentana ventana = new Ventanas.CertificacionesTrabajadorVentana(certificacion, trabajadorActual);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                CertificacionesVentana_Load(sender, e);
                //ValidarBotones();
            }
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
            Clases.Certificacion certificacion = new Clases.Certificacion();
            certificacion.Id = int.Parse(dgvCertificaciones.CurrentRow.Cells["Id"].Value.ToString()!);
            certificacion.FechaCertificacion = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaCertificacion"].Value.ToString()!);
            certificacion.FechaVencimiento = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaVencimiento"].Value.ToString()!);
            certificacion.IdProceso = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdProceso"].Value.ToString()!);
            certificacion.IdCertificador = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdCertificador"].Value.ToString()!);
            certificacion.Comentario = dgvCertificaciones.CurrentRow.Cells["Comentario"].Value.ToString()!;
            Ventanas.CertificacionesTrabajadorVentana ventana = new Ventanas.CertificacionesTrabajadorVentana(certificacion, trabajadorActual);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                CertificacionesVentana_Load(sender, e);
                //ValidarBotones();
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            Ventanas.CertificacionesTrabajadorVentana ventana = new Ventanas.CertificacionesTrabajadorVentana(null!, trabajadorActual);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                CertificacionesVentana_Load(sender, e);
                //ValidarBotones();
            }
        }

        private void btnBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvCertificaciones.CurrentRow == null)
                return;

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
            Clases.Certificacion certificacion = new Clases.Certificacion();
            certificacion.Id = int.Parse(dgvCertificaciones.CurrentRow.Cells["Id"].Value.ToString()!);
            certificacion.FechaCertificacion = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaCertificacion"].Value.ToString()!);
            certificacion.FechaVencimiento = DateTime.Parse(dgvCertificaciones.CurrentRow.Cells["FechaVencimiento"].Value.ToString()!);
            certificacion.IdProceso = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdProceso"].Value.ToString()!);
            certificacion.IdCertificador = int.Parse(dgvCertificaciones.CurrentRow.Cells["IdCertificador"].Value.ToString()!);
            certificacion.Comentario = dgvCertificaciones.CurrentRow.Cells["Comentario"].Value.ToString()!;
            Ventanas.CertificacionesTrabajadorVentana ventana = new Ventanas.CertificacionesTrabajadorVentana(certificacion, trabajadorActual);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                CertificacionesVentana_Load(sender, e);
                //ValidarBotones();
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
    }
}
