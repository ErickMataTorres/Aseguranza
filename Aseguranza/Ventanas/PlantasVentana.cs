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
    public partial class PlantasVentana : Form
    {
        private Clases.Planta? plantaActual;
        public PlantasVentana(Clases.Planta planta)
        {
            InitializeComponent();
            if (planta != null)
            {
                plantaActual = planta;
                txtNombre.Text = planta.Nombre;
            }
        }

        private void PlantasVentana_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Clases.Planta planta = this.plantaActual ?? new Clases.Planta();
            planta.Nombre = txtNombre.Text.ToUpper().Trim();
            Clases.Mensaje respuesta = planta.GuardarPlanta();
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
    }
}
