using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Lineas : Form
    {
        public Lineas()
        {
            InitializeComponent();
        }

        private void Lineas_Load(object sender, EventArgs e)
        {
            IniciarCosas();
        }
        private void IniciarCosas()
        {
            dgvLineas.DataSource = Clases.Linea.ConsultarLineas(string.Empty);
            dgvLineas.AutoResizeColumns();

            dgvLineas.Columns["Id"].Visible = false;
            dgvLineas.Columns["IdPlanta"].Visible = false;
            if (dgvLineas.Rows.Count > 0)
            {
                dgvLineas.ClearSelection();
                dgvLineas.Rows[0].Selected = true;
                dgvLineas.CurrentCell = dgvLineas.Rows[0].Cells
                    .Cast<DataGridViewCell>()
                    .First(c => c.Visible);
            }

            ValidarBotones();
        }

        private void ValidarBotones()
        {
            if (dgvLineas.Rows.Count == 0)
            {
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
            }
            else
            {
                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
            }
            txtBuscar.Focus();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbPlantas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbPlantas_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            Ventanas.LineasVentana ventana = new Ventanas.LineasVentana(null!);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Lineas_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Clases.Linea linea = new Clases.Linea();
            linea.Id = int.Parse(dgvLineas.CurrentRow.Cells["Id"].Value.ToString()!);
            linea.Nombre = dgvLineas.CurrentRow.Cells["Nombre"].Value.ToString()!;
            linea.IdPlanta = int.Parse(dgvLineas.CurrentRow.Cells["IdPlanta"].Value.ToString()!);
            Ventanas.LineasVentana ventana = new Ventanas.LineasVentana(linea);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Lineas_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Clases.Mensaje respuesta;
            if (MessageBox.Show("¿Está seguro de borrar la localidad seleccionada?", caption: "Confirmar borrado", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = int.Parse(dgvLineas.CurrentRow.Cells["Id"].Value.ToString()!);
                respuesta = Clases.Linea.BorrarLinea(id);
                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvLineas.DataSource = Clases.Linea.ConsultarLineas(string.Empty);
                ValidarBotones();
            }
        }

        private void btnRegresar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvLineas.DataSource = Clases.Linea.ConsultarLineas(txtBuscar.Text);
                dgvLineas.AutoResizeColumns();
            }
        }

        private void dgvLineas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnModificar_Click(sender, e);
        }
    }
}
