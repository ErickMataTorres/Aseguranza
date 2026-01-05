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
    public partial class LineasVentana : Form
    {
        private Clases.Linea? lineaActual;
        public LineasVentana(Clases.Linea linea)
        {
            InitializeComponent();
            if (linea != null)
            {
                lineaActual = linea;
                txtNombre.Text = linea.Nombre;
            }
        }

        private void LineasVentana_Load(object sender, EventArgs e)
        {
            cbPlantas.DataSource = Clases.Planta.ConsultarPlantas(string.Empty);
            cbPlantas.DisplayMember = "Nombre";
            cbPlantas.ValueMember = "Id";
            if (lineaActual == null)
            {
                cbPlantas.SelectedIndex = -1;
            }
            else
            {
                cbPlantas.SelectedValue = lineaActual.IdPlanta;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cbPlantas.SelectedIndex == -1 || txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Debe completar los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Clases.Linea linea = this.lineaActual ?? new Clases.Linea();
            linea.IdPlanta = (int)cbPlantas.SelectedValue!;
            linea.Nombre = txtNombre.Text.ToUpper().Trim();
            Clases.Mensaje respuesta = linea.GuardarLinea();
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
