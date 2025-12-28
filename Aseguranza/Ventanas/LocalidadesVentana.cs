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
    public partial class LocalidadesVentana : Form
    {
        private Clases.Localidad? localidadActual;

        public LocalidadesVentana(Clases.Localidad localidad)
        {
            InitializeComponent();
            if (localidad != null)
            {
                localidadActual = localidad;
                txtNombre.Text = localidad.Nombre;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Clases.Localidad localidad = this.localidadActual ?? new Clases.Localidad();
            localidad.Nombre = txtNombre.Text.ToUpper().Trim();
            Clases.Mensaje respuesta = localidad.GuardarLocalidad();
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

        private void LocalidadesVentana_Load(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
