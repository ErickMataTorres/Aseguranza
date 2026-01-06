using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Certificaciones : Form
    {
        private readonly Font _boldFont = new Font("Arial", 9F, FontStyle.Bold);
        private Color _colorOriginal;

        public Certificaciones()
        {
            InitializeComponent();
        }

        private void Certificaciones_Load(object sender, EventArgs e)
        {
            // Valores del ComboBox (asegúrate que coincidan con SQL)
            cbMostrarPor.SelectedIndex = 0; // "Todas"

            // Eventos
            dgvTrabajadores.CellFormatting += dgvTrabajadores_CellFormatting!;
            cbMostrarPor.SelectedIndexChanged += cbMostrarPor_SelectedIndexChanged!;

            // Carga inicial
            CargarTrabajadores();
        }

        // ============================
        // CARGA CENTRALIZADA
        // ============================
        private void CargarTrabajadores()
        {
            string mostrarPor = cbMostrarPor.SelectedItem?.ToString() ?? "Todas";
            string textoBuscar = txtBuscar.Text.Trim();

            dgvTrabajadores.DataSource =
                Clases.Trabajador.ConsultarTrabajadoresEstadoCertificacion(
                    mostrarPor, textoBuscar);

            dgvTrabajadores.AutoResizeColumns();

            txtBuscar.Focus();

            OcultarColumnas();
            SeleccionarPrimeraFila();
        }




        // ============================
        // UI HELPERS
        // ============================
        private void OcultarColumnas()
        {
            string[] columnasOcultas =
            {
                "Id",
                "RutaFoto",
                "IdLocalidad",
                "IdTurno",
                "IdPlanta",
                "IdLinea"
            };

            foreach (string col in columnasOcultas)
            {
                if (dgvTrabajadores.Columns.Contains(col))
                    dgvTrabajadores.Columns[col].Visible = false;
            }
        }

        private void SeleccionarPrimeraFila()
        {
            if (dgvTrabajadores.Rows.Count == 0)
                return;

            dgvTrabajadores.ClearSelection();
            dgvTrabajadores.Rows[0].Selected = true;

            dgvTrabajadores.CurrentCell = dgvTrabajadores.Rows[0].Cells
                .Cast<DataGridViewCell>()
                .First(c => c.Visible);
        }

        // ============================
        // EVENTOS
        // ============================
        private void cbMostrarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTrabajadores();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CargarTrabajadores();
                e.Handled = true;
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvTrabajadores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                AbrirCertificaciones();
        }

        private void btnCertificaciones_Click(object sender, EventArgs e)
        {
            AbrirCertificaciones();
        }

        // ============================
        // ABRIR VENTANA CERTIFICACIONES
        // ============================
        private void AbrirCertificaciones()
        {
            if (dgvTrabajadores.CurrentRow == null)
                return;

            DataGridViewRow row = dgvTrabajadores.CurrentRow;

            var trabajador = new Clases.Trabajador
            {
                Id = Convert.ToInt32(row.Cells["Id"].Value),
                NoReloj = row.Cells["NoReloj"].Value?.ToString() ?? "",
                Nombre = row.Cells["Nombre"].Value?.ToString() ?? "",
                RutaFoto = row.Cells["RutaFoto"].Value?.ToString() ?? "",

                IdLocalidad = Convert.ToInt32(row.Cells["IdLocalidad"].Value),
                NombreLocalidad = row.Cells["NombreLocalidad"].Value?.ToString() ?? "",

                IdTurno = Convert.ToInt32(row.Cells["IdTurno"].Value),
                NombreTurno = row.Cells["NombreTurno"].Value?.ToString() ?? "",

                IdPlanta = Convert.ToInt32(row.Cells["IdPlanta"].Value),
                NombrePlanta = row.Cells["NombrePlanta"].Value?.ToString() ?? "",

                IdLinea = Convert.ToInt32(row.Cells["IdLinea"].Value),
                NombreLinea = row.Cells["NombreLinea"].Value?.ToString() ?? ""
            };

            using var ventana = new Ventanas.CertificacionesVentana(trabajador);
            ventana.ShowDialog();

            // Refrescar al cerrar
            CargarTrabajadores();
        }

        // ============================
        // COLORES POR ESTADO
        // ============================
        private void dgvTrabajadores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTrabajadores.Columns[e.ColumnIndex].Name != "EstadoCertificacion")
                return;

            if (e.Value == null)
                return;

            string estado = e.Value.ToString()!;
            DataGridViewRow row = dgvTrabajadores.Rows[e.RowIndex];

            row.DefaultCellStyle.ForeColor = Color.Black;
            row.DefaultCellStyle.Font = _boldFont;

            switch (estado)
            {
                case "Vigente":
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    break;

                case "Por vencer":
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                    break;

                case "Vencida":
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    break;

                case "Sin certificar":
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    break;
            }
        }

        private void dgvTrabajadores_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTrabajadores_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
