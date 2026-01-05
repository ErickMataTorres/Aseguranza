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
    public partial class BuscarTrabajadores : Form
    {
        public event Action<Clases.Trabajador>? trabajadorSeleccionado;
        public BuscarTrabajadores()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BuscarTrabajadores_Load(object sender, EventArgs e)
        {
            dgvTrabajadores.DataSource = Clases.Trabajador.ConsultarTrabajadores(txtBuscar.Text);
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
            txtBuscar.Focus();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvTrabajadores.DataSource = Clases.Trabajador.ConsultarTrabajadores(txtBuscar.Text);
            }
        }

        private void dgvTrabajadores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            DataRowView row = (DataRowView)dgvTrabajadores.Rows[e.RowIndex].DataBoundItem;
            Clases.Trabajador t = new Clases.Trabajador
            {
                NoReloj = row["NoReloj"].ToString()!,
                Nombre = row["Nombre"].ToString()!,
                RutaFoto = row["RutaFoto"].ToString()!,
                NombreLocalidad = row["NombreLocalidad"].ToString()!,
                NombreTurno = row["NombreTurno"].ToString()!,
                NombrePlanta = row["NombrePlanta"].ToString()!,
                NombreLinea = row["NombreLinea"].ToString()!
            };
            trabajadorSeleccionado?.Invoke(t);
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(dgvTrabajadores.SelectedRows.Count == 0)
            {
                return;
            }
            DataRowView row = (DataRowView)dgvTrabajadores.SelectedRows[0].DataBoundItem;
            Clases.Trabajador t = new Clases.Trabajador
            {
                NoReloj = row["NoReloj"].ToString()!,
                Nombre = row["Nombre"].ToString()!,
                RutaFoto = row["RutaFoto"].ToString()!,
                NombreLocalidad = row["NombreLocalidad"].ToString()!,
                NombreTurno = row["NombreTurno"].ToString()!,
                NombrePlanta = row["NombrePlanta"].ToString()!,
                NombreLinea = row["NombreLinea"].ToString()!
            };
            trabajadorSeleccionado?.Invoke(t);
            this.Close();
        }
    }
}
