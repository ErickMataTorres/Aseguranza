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
    public partial class Turnos : Form
    {
        public Turnos()
        {
            InitializeComponent();
        }

        private void Turnos_Load(object sender, EventArgs e)
        {
            dgvTurnos.DataSource = Clases.Turno.ConsultarTurnos(txtBuscar.Text);
            dgvTurnos.AutoResizeColumns();
            ValidarBotones();
        }
        private void ValidarBotones()
        {
            if (dgvTurnos.Rows.Count == 0)
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
            Ventanas.TurnosVentana ventana = new Ventanas.TurnosVentana(null!);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Turnos_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Clases.Turno turno = new Clases.Turno();
            turno.Id = int.Parse(dgvTurnos.CurrentRow.Cells["Id"].Value.ToString()!);
            turno.Nombre = dgvTurnos.CurrentRow.Cells["Nombre"].Value.ToString()!;
            Ventanas.TurnosVentana ventana = new Ventanas.TurnosVentana(turno);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Turnos_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Clases.Mensaje respuesta;
            if (MessageBox.Show("¿Está seguro de borrar el turno seleccionada?", caption: "Confirmar borrado", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = int.Parse(dgvTurnos.CurrentRow.Cells["Id"].Value.ToString()!);
                respuesta = Clases.Turno.BorrarTurno(id);
                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvTurnos.DataSource = Clases.Turno.ConsultarTurnos(txtBuscar.Text);
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
                dgvTurnos.DataSource = Clases.Turno.ConsultarTurnos(txtBuscar.Text);
                ValidarBotones();
            }
        }
    }
}
