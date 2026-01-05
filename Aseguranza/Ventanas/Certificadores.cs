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
        private void LimpiarCampos()
        {
            txtNoReloj.Text = string.Empty;
            lblMostrarNombre.Text = string.Empty;
            pictureBox1.ImageLocation = string.Empty;
            lblMostrarLocalidad.Text = string.Empty;
            lblMostrarTurno.Text = string.Empty;
            lblMostrarPlanta.Text = string.Empty;
            lblMostrarLinea.Text = string.Empty;
            txtBuscar.Text = string.Empty;
            btnAgregar.Enabled = false;
            txtNoReloj.Focus();
        }

        private void Certificadores_Load(object sender, EventArgs e)
        {
            dgvCertificadores.DataSource = Clases.Certificador.ConsultarCertificadores(txtBuscar.Text);
            dgvCertificadores.AutoResizeColumns();

            dgvCertificadores.Columns["Id"].Visible = false;
            dgvCertificadores.Columns["IdTrabajador"].Visible = false;
            dgvCertificadores.Columns["RutaFoto"].Visible = false;
            dgvCertificadores.Columns["IdTurno"].Visible = false;
            dgvCertificadores.Columns["IdPlanta"].Visible = false;
            dgvCertificadores.Columns["IdLinea"].Visible = false;
            if (dgvCertificadores.Rows.Count > 0)
            {
                dgvCertificadores.ClearSelection();
                dgvCertificadores.Rows[0].Selected = true;
                dgvCertificadores.CurrentCell = dgvCertificadores.Rows[0].Cells
                    .Cast<DataGridViewCell>()
                    .First(c => c.Visible);
            }

            LimpiarCampos();
        }

        private void txtNoReloj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtNoReloj.Text == string.Empty)
                {
                    return;
                }
                Clases.Trabajador t = Clases.Trabajador.ConsultarTrabajador(txtNoReloj.Text);
                if (t != null)
                {
                    lblMostrarNombre.Text = t.Nombre;
                    pictureBox1.ImageLocation = t.RutaFoto;
                    lblMostrarLocalidad.Text = t.NombreLocalidad;
                    lblMostrarTurno.Text = t.NombreTurno;
                    lblMostrarPlanta.Text = t.NombrePlanta;
                    lblMostrarLinea.Text = t.NombreLinea;
                    btnAgregar.Enabled = true;
                }
                else
                {
                    MessageBox.Show("El trabajador no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LimpiarCampos();
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtNoReloj.Text == string.Empty)
            {
                MessageBox.Show("Debe seleccionar un número de reloj primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Clases.Certificador certificador = new Clases.Certificador();
            certificador.NoReloj = txtNoReloj.Text;
            Clases.Mensaje mensaje = certificador.GuardarCertificador();
            if (mensaje.Id == 1)
            {
                MessageBox.Show(mensaje.Nombre, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Certificadores_Load(sender, e);
            }
            else
            {
                MessageBox.Show(mensaje.Nombre, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (dgvCertificadores.Rows.Count == 0)
            {
                MessageBox.Show("No hay certificadores para borrar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = int.Parse(dgvCertificadores.CurrentRow.Cells["Id"].Value.ToString()!);
            if (MessageBox.Show("¿Está seguro de borrar el certificador seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show(Clases.Certificador.BorrarCertificador(id).Nombre, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Certificadores_Load(sender, e);
                LimpiarCampos();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Ventanas.BuscarTrabajadores ventana = new Ventanas.BuscarTrabajadores();
            ventana.trabajadorSeleccionado += (Clases.Trabajador t) =>
            {
                txtNoReloj.Text = t.NoReloj;
                lblMostrarNombre.Text = t.Nombre;
                pictureBox1.ImageLocation = t.RutaFoto;
                lblMostrarLocalidad.Text = t.NombreLocalidad;
                lblMostrarTurno.Text = t.NombreTurno;
                lblMostrarPlanta.Text = t.NombrePlanta;
                lblMostrarLinea.Text = t.NombreLinea;
                btnAgregar.Enabled = true;


                if (File.Exists(t.RutaFoto))
                    pictureBox1.ImageLocation = t.RutaFoto;
                else
                    pictureBox1.Image = null;

            };
            ventana.ShowDialog();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                dgvCertificadores.DataSource = Clases.Certificador.ConsultarCertificadores(txtBuscar.Text);
            }
        }
    }
}
