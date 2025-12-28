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
    public partial class Plantas : Form
    {
        public Plantas()
        {
            InitializeComponent();
        }

        private void Plantas_Load(object sender, EventArgs e)
        {
            dgvPlantas.DataSource = Clases.Planta.ConsultarPlantas(txtBuscar.Text);
            dgvPlantas.AutoResizeColumns();
            ValidarBotones();
        }
        private void ValidarBotones()
        {
            if (dgvPlantas.Rows.Count == 0)
            {
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
            }
            else
            {
                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Ventanas.PlantasVentana ventana = new Ventanas.PlantasVentana(null!);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Plantas_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Clases.Planta planta = new Clases.Planta();
            planta.Id = int.Parse(dgvPlantas.CurrentRow.Cells["Id"].Value.ToString()!);
            planta.Nombre = dgvPlantas.CurrentRow.Cells["Nombre"].Value.ToString()!;
            Ventanas.PlantasVentana ventana = new Ventanas.PlantasVentana(planta);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Plantas_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Clases.Mensaje respuesta;
            if (MessageBox.Show("¿Está seguro de borrar la localidad seleccionada?", caption: "Confirmar borrado", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = int.Parse(dgvPlantas.CurrentRow.Cells["Id"].Value.ToString()!);
                respuesta = Clases.Planta.BorrarPlanta(id);
                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvPlantas.DataSource = Clases.Planta.ConsultarPlantas(txtBuscar.Text);
                ValidarBotones();
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvPlantas.DataSource = Clases.Planta.ConsultarPlantas(txtBuscar.Text.Trim());
                dgvPlantas.AutoResizeColumns();
            }
        }
    }
}
