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
    public partial class Certificadores : Form
    {
        public Certificadores()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Certificadores_Load(object sender, EventArgs e)
        {
            dgvCertificadores.DataSource = Clases.Certificador.ConsultarCertificadores();
        }

        private void txtNoReloj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                Clases.Trabajador t = Clases.Trabajador.ConsultarTrabajador(int.Parse(txtNoReloj.Text));
                lblMostrarNombre.Text = t.Nombre;
                pictureBox1.ImageLocation = t.RutaFoto;
                lblMostrarLocalidad.Text = t.NombreLocalidad;
                lblMostrarTurno.Text = t.NombreTurno;
                lblMostrarPlanta.Text = t.NombrePlanta;
                lblMostrarLinea.Text = t.NombreLinea;
            }
        }
    }
}
