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
    public partial class Procesos : Form
    {
        public Procesos()
        {
            InitializeComponent();
        }

        private void Procesos_Load(object sender, EventArgs e)
        {
            dgvProcesos.DataSource = Clases.Proceso.ConsultarProcesos(txtBuscar.Text);
            dgvProcesos.AutoResizeColumns();
            dgvProcesos.Columns["Id"].Visible = false;
            if (dgvProcesos.Rows.Count > 0)
            {
                dgvProcesos.ClearSelection();
                dgvProcesos.Rows[0].Selected = true;
                dgvProcesos.CurrentCell = dgvProcesos.Rows[0].Cells
                    .Cast<DataGridViewCell>()
                    .First(c => c.Visible);
            }
            ValidarBotones();
        }

        private void ValidarBotones()
        {
            if (dgvProcesos.Rows.Count == 0)
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Ventanas.ProcesosVentana ventana = new Ventanas.ProcesosVentana(null!);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Procesos_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Clases.Proceso proceso = new Clases.Proceso();
            proceso.Id = int.Parse(dgvProcesos.CurrentRow.Cells["Id"].Value.ToString()!);
            proceso.Nombre = dgvProcesos.CurrentRow.Cells["Nombre"].Value.ToString()!;
            proceso.Descripcion = dgvProcesos.CurrentRow.Cells["Descripcion"].Value.ToString()!;
            proceso.VigenciaMeses = int.Parse(dgvProcesos.CurrentRow.Cells["VigenciaMeses"].Value.ToString()!);
            Ventanas.ProcesosVentana ventana = new Ventanas.ProcesosVentana(proceso);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Procesos_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Clases.Mensaje respuesta;
            if (MessageBox.Show("¿Está seguro de borrar el proceso seleccionada?", caption: "Confirmar borrado", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = int.Parse(dgvProcesos.CurrentRow.Cells["Id"].Value.ToString()!);
                respuesta = Clases.Proceso.BorrarProceso(id);
                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvProcesos.DataSource = Clases.Proceso.ConsultarProcesos(txtBuscar.Text);
                ValidarBotones();
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvProcesos.DataSource = Clases.Proceso.ConsultarProcesos(txtBuscar.Text);
                ValidarBotones();
            }
        }

        private void dgvProcesos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnModificar_Click(sender, e);
        }
    }
}
