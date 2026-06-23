using System.IO;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class Certificaciones : Form
    {
        private readonly Font _boldFont = new Font("Arial", 9F, FontStyle.Bold);
        private Color _colorOriginal;

        public Certificaciones()
        {
            InitializeComponent();
        }

        private void Certificaciones_Load(object sender, EventArgs e)
        {
            // Valores del ComboBox
            cbMostrarPor.SelectedIndex = 0; // "Todas"

            // Eventos
            dgvTrabajadores.CellFormatting += dgvTrabajadores_CellFormatting!;
            dgvTrabajadores.SelectionChanged += dgvTrabajadores_SelectionChanged!;
            //dgvTrabajadores.MouseDown += dgvTrabajadores_MouseDown!;
            cbMostrarPor.SelectedIndexChanged += cbMostrarPor_SelectedIndexChanged!;

            //tsmVerCertificaciones.Click += tsmVerCertificaciones_Click!;
            //tsmVistaPreviaCredencial.Click += tsmVistaPreviaCredencial_Click!;
            //tsmImprimirCredencial.Click += tsmImprimirCredencial_Click!;
            //tsmModificarTrabajador.Click += tsmModificarTrabajador_Click!;
            //cmsTrabajadores.Opening += cmsTrabajadores_Opening!;

            // Textos iniciales
            lblResumenEstados.Text = "Total: 0 | Vigentes: 0 | Por vencer: 0 | Vencidas: 0 | Sin certificar: 0";
            lblTrabajadorSeleccionado.Text = "Seleccionado: ninguno";

            txtBuscar.PlaceholderText = "Buscar por No. Reloj, nombre, planta, línea...";

            // Configuración visual
            ConfigurarGridTrabajadores();
            ConfigurarToolTips();

            // Carga inicial
            CargarTrabajadores();
        }

        private void ActualizarAvisoLimiteRegistros()
        {
            int total = dgvTrabajadores.Rows.Count;
            string textoBuscar = txtBuscar.Text.Trim();

            if (total >= 300 && string.IsNullOrWhiteSpace(textoBuscar))
            {
                lblAvisoLimite.Text =
                    "Aviso: se muestran los primeros 300 trabajadores. Use Buscar para localizar trabajadores fuera de esta lista.";
                lblAvisoLimite.ForeColor = Color.FromArgb(190, 100, 0);
                lblAvisoLimite.Visible = true;
                return;
            }

            if (total >= 300 && !string.IsNullOrWhiteSpace(textoBuscar))
            {
                lblAvisoLimite.Text =
                    $"Mostrando hasta 300 resultados para la búsqueda: \"{textoBuscar}\".";
                lblAvisoLimite.ForeColor = Color.FromArgb(190, 100, 0);
                lblAvisoLimite.Visible = true;
                return;
            }

            lblAvisoLimite.Text = $"Mostrando {total} trabajador(es).";
            lblAvisoLimite.ForeColor = Color.DimGray;
            lblAvisoLimite.Visible = true;
        }

        // ============================
        // CARGA CENTRALIZADA
        // ============================
        private void CargarTrabajadores(string noRelojSeleccionar = "")
        {
            string mostrarPor = cbMostrarPor.SelectedItem?.ToString() ?? "Todas";
            string textoBuscar = txtBuscar.Text.Trim();

            dgvTrabajadores.DataSource =
                Clases.Trabajador.ConsultarTrabajadoresEstadoCertificacion(
                    mostrarPor, textoBuscar);

            OcultarColumnas();
            ConfigurarEncabezados();

            if (!string.IsNullOrWhiteSpace(noRelojSeleccionar))
                SeleccionarTrabajadorPorNoReloj(noRelojSeleccionar);
            else
                SeleccionarPrimero();

            ActualizarResumenEstados();
            ActualizarTrabajadorSeleccionado();
            ActualizarAvisoLimiteRegistros();
            ActualizarBotonesTrabajador();

            txtBuscar.Focus();
        }




        // ============================
        // UI HELPERS
        // ============================
        private void OcultarColumnas()
        {
            string[] columnasOcultas =
            {
                "Id",
                "RutaFoto",
                "IdLocalidad",
                "IdTurno",
                "IdPlanta",
                "IdLinea"
            };

            foreach (string col in columnasOcultas)
            {
                if (dgvTrabajadores.Columns.Contains(col))
                    dgvTrabajadores.Columns[col].Visible = false;
            }
        }


        private void EjecutarBusqueda()
        {
            CargarTrabajadores();
        }


        private void ConfigurarEncabezados()
        {
            if (dgvTrabajadores.Columns.Contains("NoReloj"))
            {
                dgvTrabajadores.Columns["NoReloj"].HeaderText = "No. Reloj";
                dgvTrabajadores.Columns["NoReloj"].FillWeight = 70;
                dgvTrabajadores.Columns["NoReloj"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvTrabajadores.Columns.Contains("Nombre"))
            {
                dgvTrabajadores.Columns["Nombre"].HeaderText = "Trabajador";
                dgvTrabajadores.Columns["Nombre"].FillWeight = 220;
            }

            if (dgvTrabajadores.Columns.Contains("NombreLocalidad"))
            {
                dgvTrabajadores.Columns["NombreLocalidad"].HeaderText = "Localidad";
                dgvTrabajadores.Columns["NombreLocalidad"].FillWeight = 90;
                dgvTrabajadores.Columns["NombreLocalidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvTrabajadores.Columns.Contains("NombreTurno"))
            {
                dgvTrabajadores.Columns["NombreTurno"].HeaderText = "Turno";
                dgvTrabajadores.Columns["NombreTurno"].FillWeight = 70;
                dgvTrabajadores.Columns["NombreTurno"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvTrabajadores.Columns.Contains("NombrePlanta"))
            {
                dgvTrabajadores.Columns["NombrePlanta"].HeaderText = "Planta";
                dgvTrabajadores.Columns["NombrePlanta"].FillWeight = 80;
                dgvTrabajadores.Columns["NombrePlanta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvTrabajadores.Columns.Contains("NombreLinea"))
            {
                dgvTrabajadores.Columns["NombreLinea"].HeaderText = "Línea";
                dgvTrabajadores.Columns["NombreLinea"].FillWeight = 90;
                dgvTrabajadores.Columns["NombreLinea"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvTrabajadores.Columns.Contains("EstadoCertificacion"))
            {
                dgvTrabajadores.Columns["EstadoCertificacion"].HeaderText = "Estado";
                dgvTrabajadores.Columns["EstadoCertificacion"].FillWeight = 100;
                dgvTrabajadores.Columns["EstadoCertificacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void SeleccionarPrimero()
        {
            if (dgvTrabajadores.Rows.Count == 0)
                return;

            dgvTrabajadores.ClearSelection();
            dgvTrabajadores.Rows[0].Selected = true;

            dgvTrabajadores.CurrentCell = dgvTrabajadores.Rows[0].Cells
                .Cast<DataGridViewCell>()
                .First(c => c.Visible);
        }

        private void SeleccionarTrabajadorPorNoReloj(string noReloj)
        {
            noReloj = noReloj.Trim();

            if (string.IsNullOrWhiteSpace(noReloj))
            {
                SeleccionarPrimero();
                return;
            }

            if (!dgvTrabajadores.Columns.Contains("NoReloj"))
            {
                SeleccionarPrimero();
                return;
            }

            foreach (DataGridViewRow row in dgvTrabajadores.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string noRelojFila = row.Cells["NoReloj"].Value?.ToString()?.Trim() ?? "";

                if (string.Equals(noRelojFila, noReloj, StringComparison.OrdinalIgnoreCase))
                {
                    dgvTrabajadores.ClearSelection();
                    row.Selected = true;

                    foreach (DataGridViewCell celda in row.Cells)
                    {
                        if (celda.Visible)
                        {
                            dgvTrabajadores.CurrentCell = celda;
                            break;
                        }
                    }

                    ActualizarTrabajadorSeleccionado();
                    return;
                }
            }

            MessageBox.Show(
                $"No se encontró el trabajador recién agregado en la lista.\n\nNo. Reloj buscado: {noReloj}",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            SeleccionarPrimero();
        }

        // ============================
        // EVENTOS
        // ============================
        private void cbMostrarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTrabajadores();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                EjecutarBusqueda();
                e.Handled = true;
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvTrabajadores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                AbrirCertificaciones();
        }

        private void btnCertificaciones_Click(object sender, EventArgs e)
        {
            AbrirCertificaciones();
        }

        // ============================
        // ABRIR VENTANA CERTIFICACIONES
        // ============================
        private void AbrirCertificaciones()
        {
            Clases.Trabajador? trabajador = ObtenerTrabajadorSeleccionado();

            if (trabajador == null)
            {
                MessageBox.Show("Seleccione un trabajador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ventana = new Ventanas.CertificacionesVentana(trabajador);
            ventana.ShowDialog();

            CargarTrabajadores(trabajador.NoReloj);
        }

        // ============================
        // COLORES POR ESTADO
        // ============================
        private void dgvTrabajadores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (!dgvTrabajadores.Columns.Contains("EstadoCertificacion"))
                return;

            DataGridViewRow row = dgvTrabajadores.Rows[e.RowIndex];

            object valorEstado = row.Cells["EstadoCertificacion"].Value;

            if (valorEstado == null || valorEstado == DBNull.Value)
                return;

            string estado = valorEstado.ToString() ?? "";

            row.DefaultCellStyle.ForeColor = Color.Black;
            row.DefaultCellStyle.Font = _boldFont;

            switch (estado)
            {
                case "Vigente":
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
                    row.DefaultCellStyle.SelectionForeColor = Color.White;
                    break;

                case "Por vencer":
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                    row.DefaultCellStyle.SelectionBackColor = Color.Goldenrod;
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                    break;

                case "Vencida":
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.SelectionBackColor = Color.IndianRed;
                    row.DefaultCellStyle.SelectionForeColor = Color.White;
                    break;

                case "Sin certificar":
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.SelectionBackColor = Color.Gray;
                    row.DefaultCellStyle.SelectionForeColor = Color.White;
                    break;
            }
        }


        private void ActualizarResumenEstados()
        {
            int total = dgvTrabajadores.Rows.Count;
            int vigentes = 0;
            int porVencer = 0;
            int vencidas = 0;
            int sinCertificar = 0;

            foreach (DataGridViewRow row in dgvTrabajadores.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (!dgvTrabajadores.Columns.Contains("EstadoCertificacion"))
                    continue;

                string estado = row.Cells["EstadoCertificacion"].Value?.ToString() ?? "";

                switch (estado)
                {
                    case "Vigente":
                        vigentes++;
                        break;

                    case "Por vencer":
                        porVencer++;
                        break;

                    case "Vencida":
                        vencidas++;
                        break;

                    case "Sin certificar":
                        sinCertificar++;
                        break;
                }
            }

            lblResumenEstados.Text =
                $"Total: {total} | Vigentes: {vigentes} | Por vencer: {porVencer} | Vencidas: {vencidas} | Sin certificar: {sinCertificar}";
        }

        private void ActualizarTrabajadorSeleccionado()
        {
            if (dgvTrabajadores.CurrentRow == null || dgvTrabajadores.Rows.Count == 0)
            {
                lblTrabajadorSeleccionado.Text = "Seleccionado: ninguno";
                return;
            }

            string noReloj = "";
            string nombre = "";

            if (dgvTrabajadores.Columns.Contains("NoReloj"))
                noReloj = dgvTrabajadores.CurrentRow.Cells["NoReloj"].Value?.ToString() ?? "";

            if (dgvTrabajadores.Columns.Contains("Nombre"))
                nombre = dgvTrabajadores.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";

            lblTrabajadorSeleccionado.Text = $"Seleccionado: {noReloj} - {nombre}";
        }

        private void ConfigurarGridTrabajadores()
        {
            dgvTrabajadores.AllowUserToAddRows = false;
            dgvTrabajadores.AllowUserToDeleteRows = false;
            dgvTrabajadores.AllowUserToResizeRows = false;
            dgvTrabajadores.ReadOnly = true;
            dgvTrabajadores.MultiSelect = false;
            dgvTrabajadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrabajadores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTrabajadores.RowHeadersVisible = false;

            dgvTrabajadores.BackgroundColor = Color.WhiteSmoke;
            dgvTrabajadores.BorderStyle = BorderStyle.FixedSingle;
            dgvTrabajadores.GridColor = Color.LightGray;

            dgvTrabajadores.EnableHeadersVisualStyles = false;
            dgvTrabajadores.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 70, 140);
            dgvTrabajadores.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTrabajadores.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvTrabajadores.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTrabajadores.ColumnHeadersHeight = 32;

            dgvTrabajadores.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvTrabajadores.DefaultCellStyle.ForeColor = Color.Black;
            dgvTrabajadores.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvTrabajadores.RowTemplate.Height = 28;
        }

        private void ConfigurarToolTips()
        {
            ttAyuda.SetToolTip(cbMostrarPor, "Filtra trabajadores por estado de certificación.");
            ttAyuda.SetToolTip(txtBuscar, "Buscar por No. Reloj, nombre, localidad, turno, planta o línea.");
            ttAyuda.SetToolTip(dgvTrabajadores, "Doble clic sobre un trabajador para abrir sus certificaciones.");
            ttAyuda.SetToolTip(btnCertificaciones, "Abrir las certificaciones del trabajador seleccionado.");
            ttAyuda.SetToolTip(btnRegresar, "Cerrar esta ventana.");
            ttAyuda.SetToolTip(lblResumenEstados, "Resumen de trabajadores mostrados según el filtro actual.");
            ttAyuda.SetToolTip(lblTrabajadorSeleccionado, "Trabajador actualmente seleccionado.");
            ttAyuda.SetToolTip(lblAvisoLimite, "El sistema puede mostrar solo una parte de los registros. Use Buscar para localizar trabajadores específicos.");
            ttAyuda.SetToolTip(btnLimpiarBusqueda, "Limpia la búsqueda y vuelve a mostrar la lista general.");
            ttAyuda.SetToolTip(btnBuscar, "Busca trabajadores según el texto capturado.");
        }

        private void dgvTrabajadores_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTrabajadores_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTrabajadores_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarTrabajadorSeleccionado();
            ActualizarBotonesTrabajador();
        }

        private Clases.Trabajador? ObtenerTrabajadorSeleccionado()
        {
            if (dgvTrabajadores.CurrentRow == null)
                return null;

            DataGridViewRow row = dgvTrabajadores.CurrentRow;

            if (row.IsNewRow)
                return null;

            return new Clases.Trabajador
            {
                Id = Convert.ToInt32(row.Cells["Id"].Value),
                NoReloj = row.Cells["NoReloj"].Value?.ToString() ?? "",
                Nombre = row.Cells["Nombre"].Value?.ToString() ?? "",
                RutaFoto = row.Cells["RutaFoto"].Value?.ToString() ?? "",

                IdLocalidad = Convert.ToInt32(row.Cells["IdLocalidad"].Value),
                NombreLocalidad = row.Cells["NombreLocalidad"].Value?.ToString() ?? "",

                IdTurno = Convert.ToInt32(row.Cells["IdTurno"].Value),
                NombreTurno = row.Cells["NombreTurno"].Value?.ToString() ?? "",

                IdPlanta = Convert.ToInt32(row.Cells["IdPlanta"].Value),
                NombrePlanta = row.Cells["NombrePlanta"].Value?.ToString() ?? "",

                IdLinea = Convert.ToInt32(row.Cells["IdLinea"].Value),
                NombreLinea = row.Cells["NombreLinea"].Value?.ToString() ?? ""
            };
        }

        private void btnAgregarTrabajador_Click(object sender, EventArgs e)
        {
            using var ventana = new Ventanas.TrabajadoresVentana(null!);

            if (ventana.ShowDialog() == DialogResult.OK)
            {
                string noRelojNuevo = ventana.NoRelojGuardado;

                if (string.IsNullOrWhiteSpace(noRelojNuevo))
                {
                    MessageBox.Show(
                        "El trabajador se guardó, pero no se recibió el No. Reloj para seleccionarlo automáticamente.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    CargarTrabajadores();
                    return;
                }

                // El trabajador nuevo puede no venir en los primeros 300,
                // por eso lo buscamos directamente por No. Reloj.
                txtBuscar.Text = noRelojNuevo;

                if (cbMostrarPor.Items.Contains("Todas"))
                    cbMostrarPor.SelectedItem = "Todas";

                CargarTrabajadores(noRelojNuevo);
            }
        }

        private void btnModificarTrabajador_Click(object sender, EventArgs e)
        {
            Clases.Trabajador? trabajador = ObtenerTrabajadorSeleccionado();

            if (trabajador == null)
            {
                MessageBox.Show("Seleccione un trabajador para modificar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ventana = new Ventanas.TrabajadoresVentana(trabajador);

            if (ventana.ShowDialog() == DialogResult.OK)
            {
                CargarTrabajadores(trabajador.NoReloj);
            }
        }

        private void btnBorrarTrabajador_Click(object sender, EventArgs e)
        {
            Clases.Trabajador? trabajador = ObtenerTrabajadorSeleccionado();

            if (trabajador == null)
            {
                MessageBox.Show("Seleccione un trabajador para borrar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Está seguro de borrar al trabajador?\n\n" +
                $"No. Reloj: {trabajador.NoReloj}\n" +
                $"Nombre: {trabajador.Nombre}",
                "Confirmar borrado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            Clases.Mensaje respuesta = Clases.Trabajador.BorrarTrabajador(trabajador.Id);

            if (respuesta.Id == 1)
            {

                try
                {
                    string carpetaRespaldo = Clases.RutasArchivos.MoverCarpetaTrabajadorAEliminados(trabajador.NoReloj);

                    if (!string.IsNullOrWhiteSpace(carpetaRespaldo))
                    {
                        MessageBox.Show(
                            "El trabajador fue eliminado correctamente.\n\n" +
                            "La carpeta física del trabajador se movió a respaldo:\n" +
                            carpetaRespaldo,
                            "Respaldo generado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "El trabajador fue eliminado de la base de datos, pero no se pudo mover su carpeta física.\n\n" +
                        "Detalle: " + ex.Message,
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                MessageBox.Show(respuesta.Nombre, "Resultado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarTrabajadores();
            }
            else
            {
                MessageBox.Show(respuesta.Nombre, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActualizarBotonesTrabajador()
        {
            bool haySeleccion = dgvTrabajadores.CurrentRow != null
                && dgvTrabajadores.Rows.Count > 0;

            btnModificarTrabajador.Enabled = haySeleccion;
            btnBorrarTrabajador.Enabled = haySeleccion;
            btnCertificaciones.Enabled = haySeleccion;
        }

        private void btnVistaPreviaCredencial_Click(object sender, EventArgs e)
        {
            Clases.Trabajador? trabajador = ObtenerTrabajadorSeleccionado();

            if (trabajador == null)
            {
                MessageBox.Show("Seleccione un trabajador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ventana = new Verificaciones(trabajador.NoReloj);
            ventana.ShowDialog();
        }

        private void btnImprimirCredencial_Click(object sender, EventArgs e)
        {
            Clases.Trabajador? trabajador = ObtenerTrabajadorSeleccionado();

            if (trabajador == null)
            {
                MessageBox.Show("Seleccione un trabajador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ventana = new Verificaciones(
                trabajador.NoReloj,
                imprimirAutomaticamente: true,
                ocultarVentanaAlImprimir: true,
                guardarDirectoEnDescargas: true);

            ventana.ShowDialog();
        }

        private void dgvTrabajadores_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView.HitTestInfo hit = dgvTrabajadores.HitTest(e.X, e.Y);

            if (hit.RowIndex < 0)
                return;

            dgvTrabajadores.ClearSelection();

            DataGridViewRow fila = dgvTrabajadores.Rows[hit.RowIndex];
            fila.Selected = true;

            foreach (DataGridViewCell celda in fila.Cells)
            {
                if (celda.Visible)
                {
                    dgvTrabajadores.CurrentCell = celda;
                    break;
                }
            }

            ActualizarTrabajadorSeleccionado();
        }

        private void tsmVerCertificaciones_Click(object sender, EventArgs e)
        {
            AbrirCertificaciones();
        }

        private void tsmVistaPreviaCredencial_Click(object sender, EventArgs e)
        {
            btnVistaPreviaCredencial_Click(sender, e);
        }

        private void tsmImprimirCredencial_Click(object sender, EventArgs e)
        {
            btnImprimirCredencial_Click(sender, e);
        }

        private void tsmModificarTrabajador_Click(object sender, EventArgs e)
        {
            btnModificarTrabajador_Click(sender, e);
        }

        private void cmsTrabajadores_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool haySeleccion = dgvTrabajadores.CurrentRow != null
            && dgvTrabajadores.Rows.Count > 0;

            tsmVerCertificaciones.Enabled = haySeleccion;
            tsmVistaPreviaCredencial.Enabled = haySeleccion;
            tsmImprimirCredencial.Enabled = haySeleccion;
            tsmModificarTrabajador.Enabled = haySeleccion;
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();

            if (cbMostrarPor.Items.Contains("Todas"))
                cbMostrarPor.SelectedItem = "Todas";

            CargarTrabajadores();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtBuscar.Clear();

                if (cbMostrarPor.Items.Contains("Todas"))
                    cbMostrarPor.SelectedItem = "Todas";

                CargarTrabajadores();

                e.Handled = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            EjecutarBusqueda();
        }
    }
}
