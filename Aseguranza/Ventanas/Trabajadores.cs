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
    public partial class Trabajadores : Form
    {
        public Trabajadores()
        {
            InitializeComponent();
        }

        private void Trabajadores_Load(object sender, EventArgs e)
        {
            dgvTrabajadores.DataSource = Clases.Trabajador.ConsultarTrabajadores(string.Empty);
            dgvTrabajadores.AutoResizeColumns();

            dgvTrabajadores.Columns["Id"].Visible = false;
            dgvTrabajadores.Columns["RutaFoto"].Visible = false;
            dgvTrabajadores.Columns["IdLocalidad"].Visible = false;
            dgvTrabajadores.Columns["IdTurno"].Visible = false;
            dgvTrabajadores.Columns["IdPlanta"].Visible = false;
            dgvTrabajadores.Columns["IdLinea"].Visible = false;
            if (dgvTrabajadores.Rows.Count > 0)
            {
                dgvTrabajadores.ClearSelection();
                dgvTrabajadores.Rows[0].Selected = true;
                dgvTrabajadores.CurrentCell = dgvTrabajadores.Rows[0].Cells
                    .Cast<DataGridViewCell>()
                    .First(c => c.Visible);
            }

            ValidarBotones();
        }
        private void ValidarBotones()
        {
            if (dgvTrabajadores.Rows.Count == 0)
            {
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
            }
            else
            {
                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
            }
            txtBuscar.Focus();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Ventanas.TrabajadoresVentana ventana = new Ventanas.TrabajadoresVentana(null!);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Trabajadores_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Clases.Trabajador trabajador = new Clases.Trabajador();
            trabajador.Id = int.Parse(dgvTrabajadores.CurrentRow.Cells["Id"].Value.ToString()!);
            trabajador.NoReloj = dgvTrabajadores.CurrentRow.Cells["NoReloj"].Value.ToString()!;
            trabajador.Nombre = dgvTrabajadores.CurrentRow.Cells["Nombre"].Value.ToString()!;
            trabajador.RutaFoto = dgvTrabajadores.CurrentRow.Cells["RutaFoto"].Value.ToString()!;
            trabajador.IdLocalidad = int.Parse(dgvTrabajadores.CurrentRow.Cells["IdLocalidad"].Value.ToString()!);
            trabajador.IdTurno = int.Parse(dgvTrabajadores.CurrentRow.Cells["IdTurno"].Value.ToString()!);
            trabajador.IdPlanta = int.Parse(dgvTrabajadores.CurrentRow.Cells["IdPlanta"].Value.ToString()!);
            trabajador.IdLinea = int.Parse(dgvTrabajadores.CurrentRow.Cells["IdLinea"].Value.ToString()!);
            Ventanas.TrabajadoresVentana ventana = new Ventanas.TrabajadoresVentana(trabajador);
            ventana.ShowDialog();
            if (ventana.DialogResult == DialogResult.OK)
            {
                Trabajadores_Load(sender, e);
                ValidarBotones();
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de borrar el trabajador seleccionado?\nTambién se eliminará su foto.", "Confirmar borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int id = int.Parse(dgvTrabajadores.CurrentRow.Cells["Id"].Value.ToString()!);
            string? rutaFoto = dgvTrabajadores.CurrentRow.Cells["RutaFoto"].Value?.ToString();

            Clases.Mensaje respuesta = Clases.Trabajador.BorrarTrabajador(id);

            if (respuesta.Id == 1)
            {
                // 🗑️ Eliminar foto física
                try
                {
                    if (!string.IsNullOrEmpty(rutaFoto) && File.Exists(rutaFoto))
                    {
                        File.Delete(rutaFoto);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El trabajador fue eliminado, pero no se pudo borrar la foto.\n" + ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvTrabajadores.DataSource = Clases.Trabajador.ConsultarTrabajadores(string.Empty);
                ValidarBotones();
            }
            else
            {
                MessageBox.Show(respuesta.Nombre, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Clases.Mensaje respuesta;
            //if (MessageBox.Show("¿Está seguro de borrar el trabajador seleccionada?", caption: "Confirmar borrado", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    int id = int.Parse(dgvTrabajadores.CurrentRow.Cells["Id"].Value.ToString()!);
            //    string rutaFoto = dgvTrabajadores.CurrentRow.Cells["RutaFoto"].Value.ToString()!;
            //    respuesta = Clases.Trabajador.BorrarTrabajador(id);
            //    MessageBox.Show(respuesta.Nombre, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dgvTrabajadores.DataSource = Clases.Trabajador.ConsultarTrabajadores();
            //    ValidarBotones();
            //}
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvTrabajadores.DataSource = Clases.Trabajador.ConsultarTrabajadores(txtBuscar.Text);
                dgvTrabajadores.AutoResizeColumns();
                //ValidarBotones();
            }
        }

        private void dgvTrabajadores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnModificar_Click(sender, e);
        }
    }
}
