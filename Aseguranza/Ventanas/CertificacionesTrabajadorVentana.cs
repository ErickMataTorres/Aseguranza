using Aseguranza.Clases;
using System;
using System.Data;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CertificacionesTrabajadorVentana : Form
    {
        private Clases.Certificacion? certificacionActual;

        public CertificacionesTrabajadorVentana(Certificacion certificacion)
        {
            InitializeComponent();
            if(certificacion != null)
            {
                certificacionActual = certificacion;
                cbProcesos.SelectedValue = certificacion.IdProceso;
                dtpFechaCertificacion.Value = certificacion.FechaCertificacion;
                dtpFechaVencimiento.Value = certificacion.FechaVencimiento;
                cbCertificadores.SelectedValue = certificacion.IdCertificador;
                txtComentario.Text = certificacion.Comentario;
            }
        }
        private void CertificacionesTrabajadorVentana_Load(object sender, EventArgs e)
        {
            cbProcesos.DataSource = Clases.Proceso.ConsultarProcesos("");
            cbProcesos.DisplayMember = "Nombre";
            cbProcesos.ValueMember = "Id";
            cbCertificadores.DataSource = Clases.Certificador.ConsultarCertificadores("");
            cbCertificadores.DisplayMember = "Nombre";
            cbCertificadores.ValueMember = "Id";
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
