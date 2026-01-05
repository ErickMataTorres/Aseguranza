using Aseguranza.Clases;
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
    public partial class TurnosVentana : Form
    {
        private Clases.Turno? turnoActual;
        public TurnosVentana(Clases.Turno turno)
        {
            InitializeComponent();
            if (turno != null)
            {
                turnoActual = turno;
                txtNombre.Text = turno.Nombre;
            }
        }

        private void TurnosVentana_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("El nombre del turno no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Clases.Turno turno = this.turnoActual ?? new Clases.Turno();
            turno.Nombre = txtNombre.Text.ToUpper().Trim();
            Clases.Mensaje respuesta = turno.GuardarTurno();
            if (respuesta.Id == 1 || respuesta.Id == 2)
            {
                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(respuesta.Nombre, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                btnAceptar_Click(sender, e);
            }
        }
    }
}
