using Aseguranza.Clases;
using System;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CertificacionAnulacionVentana : Form
    {
        private readonly int idCertificacion;
        private readonly string noReloj;
        private readonly string nombreTrabajador;
        private readonly string proceso;

        private CertificacionAnulacion? anulacionActual;

        public CertificacionAnulacionVentana(
            int idCertificacion,
            string noReloj,
            string nombreTrabajador,
            string proceso)
        {
            InitializeComponent();

            this.idCertificacion = idCertificacion;
            this.noReloj = noReloj;
            this.nombreTrabajador = nombreTrabajador;
            this.proceso = proceso;
        }

        private void CertificacionAnulacionVentana_Load(object sender, EventArgs e)
        {
            lblTrabajador.Text = $"Trabajador: {noReloj} - {nombreTrabajador}";
            lblProceso.Text = $"Proceso: {proceso}";

            CargarTiposAnulacion();
            ConfigurarEstadoInicial();
            CargarAnulacionActual();
        }

        private void CargarTiposAnulacion()
        {
            cbTipoAnulacion.Items.Clear();

            cbTipoAnulacion.Items.Add("1 día");
            cbTipoAnulacion.Items.Add("1 semana");
            cbTipoAnulacion.Items.Add("15 días");
            cbTipoAnulacion.Items.Add("1 mes");
            cbTipoAnulacion.Items.Add("3 meses");
            cbTipoAnulacion.Items.Add("Hasta fecha");
            cbTipoAnulacion.Items.Add("Permanente");

            cbTipoAnulacion.SelectedIndex = 0;
        }

        private void ConfigurarEstadoInicial()
        {
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today.AddDays(1);

            dtpFechaInicio.Enabled = true;
            dtpFechaFin.Enabled = false;

            btnGuardar.Text = "Guardar";
            btnEliminar.Enabled = false;
        }

        private void CargarAnulacionActual()
        {
            try
            {
                anulacionActual = CertificacionAnulacion.ConsultarAnulacionPorCertificacion(idCertificacion);

                if (anulacionActual == null)
                    return;

                btnGuardar.Text = "Modificar";
                btnEliminar.Enabled = true;

                txtComentario.Text = anulacionActual.Comentario ?? "";

                dtpFechaInicio.Value = anulacionActual.FechaInicio;

                if (anulacionActual.EsPermanente)
                {
                    cbTipoAnulacion.SelectedItem = "Permanente";
                    dtpFechaFin.Enabled = false;
                }
                else
                {
                    cbTipoAnulacion.SelectedItem = anulacionActual.TipoAnulacion ?? "Hasta fecha";

                    if (anulacionActual.FechaFin.HasValue)
                    {
                        dtpFechaFin.Value = anulacionActual.FechaFin.Value;
                    }

                    dtpFechaFin.Enabled = (cbTipoAnulacion.Text == "Hasta fecha");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al consultar anulación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cmbTipoAnulacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarFechaFinSegunTipo();
        }

        private void dtpFechaInicio_ValueChanged(object sender, EventArgs e)
        {
            ActualizarFechaFinSegunTipo();
        }

        private void ActualizarFechaFinSegunTipo()
        {
            if (cbTipoAnulacion.SelectedItem == null)
                return;

            string tipo = cbTipoAnulacion.Text;

            dtpFechaFin.Enabled = false;

            switch (tipo)
            {
                case "1 día":
                    dtpFechaFin.Value = dtpFechaInicio.Value.Date.AddDays(1);
                    break;

                case "1 semana":
                    dtpFechaFin.Value = dtpFechaInicio.Value.Date.AddDays(7);
                    break;

                case "15 días":
                    dtpFechaFin.Value = dtpFechaInicio.Value.Date.AddDays(15);
                    break;

                case "1 mes":
                    dtpFechaFin.Value = dtpFechaInicio.Value.Date.AddMonths(1);
                    break;

                case "3 meses":
                    dtpFechaFin.Value = dtpFechaInicio.Value.Date.AddMonths(3);
                    break;

                case "Hasta fecha":
                    dtpFechaFin.Enabled = true;

                    if (dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date)
                    {
                        dtpFechaFin.Value = dtpFechaInicio.Value.Date;
                    }

                    break;

                case "Permanente":
                    dtpFechaFin.Enabled = false;
                    break;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarInformacion())
                return;

            string tipo = cbTipoAnulacion.Text;
            bool esPermanente = tipo == "Permanente";

            DateTime? fechaFin = esPermanente
                ? null
                : dtpFechaFin.Value.Date;

            CertificacionAnulacion anulacion = new CertificacionAnulacion
            {
                IdCertificacion = idCertificacion,
                TipoAnulacion = tipo,
                FechaInicio = dtpFechaInicio.Value.Date,
                FechaFin = fechaFin,
                EsPermanente = esPermanente,
                Comentario = txtComentario.Text.Trim()
            };

            Mensaje respuesta;

            if (anulacionActual == null)
            {
                respuesta = anulacion.GuardarCertificacionAnulacion();
            }
            else
            {
                anulacion.Id = anulacionActual.Id;
                respuesta = anulacion.ModificarCertificacionAnulacion();
            }

            if (respuesta.Id == 1)
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Resultado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ValidarInformacion()
        {
            if (cbTipoAnulacion.SelectedItem == null)
            {
                MessageBox.Show(
                    "Seleccione el tipo de anulación.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (cbTipoAnulacion.Text != "Permanente"
                && dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date)
            {
                MessageBox.Show(
                    "La fecha fin no puede ser menor que la fecha inicio.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtComentario.Text))
            {
                MessageBox.Show(
                    "Debe escribir un comentario indicando por qué se anuló la certificación.",
                    "Comentario requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtComentario.Focus();
                return false;
            }

            return true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (anulacionActual == null)
            {
                MessageBox.Show(
                    "No existe una anulación activa para eliminar.",
                    "Sin anulación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "¿Seguro que desea eliminar la anulación de esta certificación?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            Mensaje respuesta = CertificacionAnulacion.EliminarCertificacionAnulacion(anulacionActual.Id);

            if (respuesta.Id == 1)
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Resultado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(
                    respuesta.Nombre,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}