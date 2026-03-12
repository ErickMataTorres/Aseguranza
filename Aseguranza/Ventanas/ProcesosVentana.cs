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
    public partial class ProcesosVentana : Form
    {
        private Clases.Proceso? procesoActual;
        public ProcesosVentana(Clases.Proceso proceso)
        {
            InitializeComponent();
            if (proceso != null)
            {
                procesoActual = proceso;
                txtNombre.Text = proceso.Nombre;
                txtDescripcion.Text = proceso.Descripcion;
                txtVigencia.Text = proceso.VigenciaMeses.ToString();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == string.Empty || txtDescripcion.Text == string.Empty || txtVigencia.Text == string.Empty)
            {
                MessageBox.Show("Los campos deben completarse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Clases.Proceso proceso = this.procesoActual ?? new Clases.Proceso();
            proceso.Nombre = txtNombre.Text.ToUpper().Trim();
            proceso.Descripcion = txtDescripcion.Text.ToUpper().Trim();
            proceso.VigenciaMeses = int.Parse(txtVigencia.Text.Trim());
            Clases.Mensaje respuesta = proceso.GuardarProceso();
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

        private void ProcesosVentana_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAceptar_Click(sender, e);
            }
            // Permitir solo números y la tecla de borrar (Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Bloquea la tecla
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
