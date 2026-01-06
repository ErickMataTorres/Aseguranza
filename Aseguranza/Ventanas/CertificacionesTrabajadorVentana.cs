using Aseguranza.Clases;
using System;
using System.Data;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CertificacionesTrabajadorVentana : Form
    {
        private Clases.Certificacion? certificacionActual;
        private Clases.Trabajador? trabajadorActual;

        public CertificacionesTrabajadorVentana(Certificacion certificacion, Trabajador trabajador)
        {
            InitializeComponent();
            if(certificacion != null)
            {
                certificacionActual = certificacion;
                //cbProcesos.SelectedValue = certificacion.IdProceso;
                //dtpFechaCertificacion.Value = certificacion.FechaCertificacion;
                //dtpFechaVencimiento.Value = certificacion.FechaVencimiento;
                //cbCertificadores.SelectedValue = certificacion.IdCertificador;
                //txtComentario.Text = certificacion.Comentario;
            }
            trabajadorActual = trabajador;
        }
        private void CertificacionesTrabajadorVentana_Load(object sender, EventArgs e)
        {
            cbProcesos.DataSource = Clases.Proceso.ConsultarProcesos("");
            cbProcesos.DisplayMember = "Nombre";
            cbProcesos.ValueMember = "Id";
            cbCertificadores.DataSource = Clases.Certificador.ConsultarCertificadores("");
            cbCertificadores.DisplayMember = "NombreTrabajador";
            cbCertificadores.ValueMember = "Id";

            if (certificacionActual == null)
            {
                cbProcesos.SelectedIndex = -1;
                dtpFechaCertificacion.Value = DateTime.Today;
                cbCertificadores.SelectedIndex = -1;
                txtComentario.Text = string.Empty;
            }
            else
            {
                cbProcesos.SelectedValue = certificacionActual.IdProceso;
                dtpFechaCertificacion.Value = certificacionActual.FechaCertificacion;
                cbCertificadores.SelectedValue = certificacionActual.IdCertificador;
                txtComentario.Text = certificacionActual.Comentario;
            }
            cbProcesos.Focus();

        }


    





        // =====================================================
        // FECHAS
        // =====================================================
        private void cbProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarFechaVencimiento();
        }

        private void dtpFechaCertificacion_ValueChanged(object sender, EventArgs e)
        {
            ActualizarFechaVencimiento();
        }

        private void ActualizarFechaVencimiento()
        {
            if (cbProcesos.SelectedItem == null)
                return;

            int vigenciaMeses = Convert.ToInt32(
                ((DataRowView)cbProcesos.SelectedItem)["VigenciaMeses"]);

            dtpFechaVencimiento.Value =
                dtpFechaCertificacion.Value.AddMonths(vigenciaMeses);
        }

        // =====================================================
        // BOTÓN ACEPTAR
        // =====================================================
        private void btnAceptar_Click(object sender, EventArgs e)
        {

            if (cbProcesos.SelectedIndex == -1 || cbCertificadores.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Debes seleccionar un proceso y un certificador",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            DateTime fechaCert = dtpFechaCertificacion.Value;

            if (fechaCert < new DateTime(1753, 1, 1))
            {
                MessageBox.Show(
                    "La fecha de certificación no es válida",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            Clases.Certificacion certificacion = new Clases.Certificacion();
            certificacion.IdTrabajador = trabajadorActual!.Id;
            certificacion.IdProceso = Convert.ToInt32(cbProcesos.SelectedValue);
            certificacion.FechaCertificacion = dtpFechaCertificacion.Value;
            certificacion.IdCertificador = Convert.ToInt32(cbCertificadores.SelectedValue);
            certificacion.Comentario = txtComentario.Text.Trim();

            Clases.Mensaje respuesta = certificacion.GuardarCertificacion();
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

            DialogResult = DialogResult.OK;
            Close();
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================
        private bool Validar()
        {
            if (cbProcesos.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un proceso.");
                return false;
            }

            if (cbCertificadores.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un certificador.");
                return false;
            }

            if (dtpFechaVencimiento.Value <= dtpFechaCertificacion.Value)
            {
                MessageBox.Show("La fecha de vencimiento no es válida.");
                return false;
            }

            return true;
        }

        // =====================================================
        // REGRESAR
        // =====================================================
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
