using Aseguranza.Clases;
using System.Drawing;
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
        private bool tieneAnulacionActivaActual = false;

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
            ConfigurarToolTips();

            ActualizarResumenAnulacion();
        }

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(cbTipoAnulacion, "Seleccione cuánto tiempo estará anulada la certificación.");
            ttAyuda.SetToolTip(dtpFechaInicio, "Fecha desde la cual inicia la anulación.");
            ttAyuda.SetToolTip(dtpFechaFin, "Fecha hasta la cual estará anulada. Se desactiva si la anulación es permanente.");
            ttAyuda.SetToolTip(txtComentario, "Explique claramente el motivo de la anulación.");
            ttAyuda.SetToolTip(btnGuardar, "Guardar o modificar la anulación.");
            ttAyuda.SetToolTip(btnEliminar, "Eliminar la anulación activa de esta certificación.");
            ttAyuda.SetToolTip(btnRegresar, "Cerrar esta ventana.");
            ttAyuda.SetToolTip(lblEstadoAnulacion, "Indica si esta certificación tiene una anulación activa.");
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

            btnGuardar.Text = "Guardar anulación";
            btnEliminar.Enabled = false;

            MostrarEstadoAnulacion(false);
        }

        private void CargarAnulacionActual()
        {
            try
            {
                anulacionActual = CertificacionAnulacion.ConsultarAnulacionPorCertificacion(idCertificacion);

                if (anulacionActual == null)
                {
                    MostrarEstadoAnulacion(false);
                    return;
                }

                MostrarEstadoAnulacion(true);

                btnGuardar.Text = "Modificar anulación";
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

                    dtpFechaFin.Enabled = cbTipoAnulacion.Text == "Hasta fecha";
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
            ActualizarResumenAnulacion();
        }

        private void dtpFechaInicio_ValueChanged(object sender, EventArgs e)
        {
            ActualizarFechaFinSegunTipo();
            ActualizarResumenAnulacion();
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
                string accion = anulacionActual == null
                    ? "guardó"
                    : "modificó";

                MessageBox.Show(
                    $"La anulación se {accion} correctamente.\n\nTrabajador:\n{noReloj} - {nombreTrabajador}\n\nProceso:\n{proceso}",
                    "Anulación registrada",
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
                $"¿Seguro que desea eliminar la anulación activa de esta certificación?\n\nTrabajador:\n{noReloj} - {nombreTrabajador}\n\nProceso:\n{proceso}\n\nEsta acción permitirá volver a modificar o renovar la certificación.",
                "Confirmar eliminación de anulación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            Mensaje respuesta = CertificacionAnulacion.EliminarCertificacionAnulacion(anulacionActual.Id);

            if (respuesta.Id == 1)
            {
                MessageBox.Show(
                    $"La anulación se eliminó correctamente.\n\nTrabajador:\n{noReloj} - {nombreTrabajador}\n\nProceso:\n{proceso}",
                    "Anulación eliminada",
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

        private void MostrarEstadoAnulacion(bool tieneAnulacionActiva)
        {
            tieneAnulacionActivaActual = tieneAnulacionActiva;

            lblEstadoAnulacion.Enabled = true;
            lblEstadoAnulacion.AutoSize = false;
            lblEstadoAnulacion.TextAlign = ContentAlignment.MiddleCenter;
            lblEstadoAnulacion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            if (tieneAnulacionActiva)
            {
                lblEstadoAnulacion.Text = "Estado: Anulación activa";
                lblEstadoAnulacion.BackColor = Color.FromArgb(228, 209, 255);
                lblEstadoAnulacion.ForeColor = Color.FromArgb(74, 20, 140);
            }
            else
            {
                lblEstadoAnulacion.Text = "Estado: Sin anulación activa";
                lblEstadoAnulacion.BackColor = Color.FromArgb(220, 245, 225);
                lblEstadoAnulacion.ForeColor = Color.FromArgb(0, 120, 40);
            }

            lblEstadoAnulacion.Refresh();
        }

        private void ActualizarResumenAnulacion()
        {
            if (cbTipoAnulacion.SelectedItem == null)
            {
                lblResumenAnulacion.Text = "Resumen: Sin datos capturados";
                return;
            }

            string tipo = cbTipoAnulacion.Text;
            string fechaInicio = dtpFechaInicio.Value.ToString("dd/MM/yyyy");
            string fechaFin = tipo == "Permanente"
                ? "Sin fecha fin"
                : dtpFechaFin.Value.ToString("dd/MM/yyyy");

            string comentario = txtComentario.Text.Trim();

            if (string.IsNullOrWhiteSpace(comentario))
                comentario = "Sin comentario capturado";

            lblResumenAnulacion.Text =
                $"Resumen: {tipo} | Inicio: {fechaInicio} | Fin: {fechaFin} | Motivo: {comentario}";
        }

        private void dtpFechaFin_ValueChanged(object sender, EventArgs e)
        {
            ActualizarResumenAnulacion();
        }

        private void txtComentario_TextChanged(object sender, EventArgs e)
        {
            ActualizarResumenAnulacion();
        }
    }
}