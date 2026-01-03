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
    public partial class Localidades : Form
    {
        public Localidades()
        {
            InitializeComponent();
        }

        private void Localidades_Load(object sender, EventArgs e)
        {
            IniciarTodo();
        }

        private void IniciarTodo()
        {
            dgvLocalidades.DataSource = Clases.Localidad.ConsultarLocalidades(txtBuscar.Text);
            dgvLocalidades.AutoResizeColumns();
            ValidarBotones();
        }

        private void ValidarBotones()
        {
            if (dgvLocalidades.Rows.Count == 0)
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
            Ventanas.LocalidadesVentana ventana = new Ventanas.LocalidadesVentana(null!);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Localidades_Load(sender, e);
                ValidarBotones();
            }
        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {

            Clases.Mensaje respuesta;
            if (MessageBox.Show("¿Está seguro de borrar la localidad seleccionada?", caption: "Confirmar borrado", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = int.Parse(dgvLocalidades.CurrentRow.Cells["Id"].Value.ToString()!);
                respuesta = Clases.Localidad.BorrarLocalidad(id);
                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvLocalidades.DataSource = Clases.Localidad.ConsultarLocalidades(txtBuscar.Text);
                ValidarBotones();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Clases.Localidad localidad = new Clases.Localidad();
            localidad.Id = int.Parse(dgvLocalidades.CurrentRow.Cells["Id"].Value.ToString()!);
            localidad.Nombre = dgvLocalidades.CurrentRow.Cells["Nombre"].Value.ToString()!;
            Ventanas.LocalidadesVentana ventana = new Ventanas.LocalidadesVentana(localidad);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Localidades_Load(sender, e);
                ValidarBotones();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvLocalidades.DataSource = Clases.Localidad.ConsultarLocalidades(txtBuscar.Text);
                dgvLocalidades.AutoResizeColumns();
                //ValidarBotones();
            }
        }
    }
}
