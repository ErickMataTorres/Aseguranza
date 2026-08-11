using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Web.WebView2.Core;
using Aseguranza.UI;

namespace Aseguranza.Ventanas
{
    public partial class Verificaciones : Form
    {
        private readonly string _noRelojInicial = "";
        private readonly bool _imprimirAutomaticamente = false;
        private readonly bool _ocultarVentanaAlImprimir = false;
        private readonly bool _guardarDirectoEnDescargas = false;

        private string _noRelojCredencialActual = "";

        // Evita ejecuciones simultáneas de impresión si el usuario
        // hace doble clic o WebView2 tarda en responder.
        private bool _generandoPdf = false;

        // Diagnóstico de procesos WebView2.
        private bool _eventosWebViewRegistrados = false;
        private bool _webViewConFalloFatal = false;
        private string _ultimoFalloWebView = "";

        public string? RutaPdfGenerado { get; private set; }
        public string? ErrorGeneracionPdf { get; private set; }
        public Verificaciones()
        {
            InitializeComponent();
            AplicarEstiloVisual();
        }

        public Verificaciones(
            string noReloj,
            bool imprimirAutomaticamente = false,
            bool ocultarVentanaAlImprimir = false,
            bool guardarDirectoEnDescargas = false)
        {
            InitializeComponent();

            _noRelojInicial = noReloj;
            _imprimirAutomaticamente = imprimirAutomaticamente;
            _ocultarVentanaAlImprimir = ocultarVentanaAlImprimir;
            _guardarDirectoEnDescargas = guardarDirectoEnDescargas;

            AplicarEstiloVisual();

            if (_ocultarVentanaAlImprimir)
            {
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                Location = new Point(-20000, -20000);
            }
        }

        // =========================================================
        // INTERFAZ MODERNA
        // =========================================================

        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            FormStyler.ApplyBase(
                this,
                "Verificaciones",
                new Size(
                    1180,
                    760));

            DoubleBuffered =
                true;

            // Controles heredados de la vista WinForms antigua.
            // Se conservan porque participan en la generación de
            // la credencial, pero ya no forman parte de la UI visible.
            lblVerificador.Visible =
                false;

            lblNoReloj.Visible =
                false;

            lblTrabajador.Visible =
                false;

            pnlAdelante.Visible =
                false;

            pnlReverso.Visible =
                false;

            FormStyler.CreateHeader(
                this,
                "Verificación de certificaciones",
                "Consulta al trabajador y genera la vista previa de su credencial.",
                height: 105,
                titleX: 38,
                titleY: 20,
                subtitleX: 40,
                subtitleY: 61);

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        125),
                    new Size(
                        1140,
                        615),
                    radius: 14);

            // =====================================================
            // BÚSQUEDA
            // =====================================================

            Panel pnlBusqueda =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            18),

                    Size =
                        new Size(
                            1100,
                            112),

                    BackColor =
                        AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlBusqueda,
                10);

            pnlContenido.Controls.Add(
                pnlBusqueda);

            Label lblTituloBusqueda =
                new Label
                {
                    AutoSize =
                        true,

                    Text =
                        "Buscar trabajador",

                    Location =
                        new Point(
                            18,
                            12),

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            13F,
                            FontStyle.Bold),

                    BackColor =
                        Color.Transparent
                };

            pnlBusqueda.Controls.Add(
                lblTituloBusqueda);

            Label lblCampoReloj =
                CrearEtiquetaCampo(
                    "No. Reloj *",
                    new Point(
                        18,
                        46));

            pnlBusqueda.Controls.Add(
                lblCampoReloj);

            Panel pnlNoReloj =
                new Panel
                {
                    Location =
                        new Point(
                            18,
                            68),

                    Size =
                        new Size(
                            190,
                            40),

                    BackColor =
                        Color.White
                };

            txtNoReloj.Parent =
                pnlNoReloj;

            txtNoReloj.Location =
                new Point(
                    10,
                    9);

            txtNoReloj.Size =
                new Size(
                    170,
                    24);

            txtNoReloj.BorderStyle =
                BorderStyle.None;

            txtNoReloj.Multiline =
                false;

            txtNoReloj.BackColor =
                Color.White;

            txtNoReloj.ForeColor =
                AppColors.TextPrimary;

            txtNoReloj.Font =
                AppFonts.Regular(
                    11F);

            InputStyler.ApplyOutlinedInput(
                pnlNoReloj,
                txtNoReloj,
                radius: 7);

            pnlBusqueda.Controls.Add(
                pnlNoReloj);

            Label lblCampoNombre =
                CrearEtiquetaCampo(
                    "Trabajador",
                    new Point(
                        226,
                        46));

            pnlBusqueda.Controls.Add(
                lblCampoNombre);

            Panel pnlNombreBusqueda =
                new Panel
                {
                    Location =
                        new Point(
                            226,
                            68),

                    Size =
                        new Size(
                            390,
                            40),

                    BackColor =
                        Color.White
                };

            txtNombre.Parent =
                pnlNombreBusqueda;

            txtNombre.Location =
                new Point(
                    10,
                    9);

            txtNombre.Size =
                new Size(
                    370,
                    24);

            txtNombre.BorderStyle =
                BorderStyle.None;

            txtNombre.Multiline =
                false;

            txtNombre.ReadOnly =
                true;

            txtNombre.BackColor =
                Color.White;

            txtNombre.ForeColor =
                AppColors.TextPrimary;

            txtNombre.Font =
                AppFonts.Regular(
                    11F,
                    FontStyle.Bold);

            InputStyler.ApplyOutlinedInput(
                pnlNombreBusqueda,
                txtNombre,
                radius: 7,
                borderColor: AppColors.BorderMedium);

            pnlBusqueda.Controls.Add(
                pnlNombreBusqueda);

            Button btnBuscar =
                new Button();

            ButtonStyler.Apply(
                btnBuscar,
                "Buscar",
                AppColors.Primary,
                AppIcons.Search,
                width: 120,
                height: 40);

            btnBuscar.Location =
                new Point(
                    635,
                    68);

            btnBuscar.Click +=
                async (_, _) =>
                {
                    await BuscarDesdeInterfazAsync();
                };

            pnlBusqueda.Controls.Add(
                btnBuscar);

            Button btnLimpiar =
                new Button();

            ButtonStyler.Apply(
                btnLimpiar,
                "Limpiar",
                AppColors.Neutral,
                string.Empty,
                width: 120,
                height: 40);

            if (btnLimpiar.Image is not null)
            {
                Image imagenTemporal =
                    btnLimpiar.Image;

                btnLimpiar.Image =
                    null;

                imagenTemporal.Dispose();
            }

            btnLimpiar.Padding =
                new Padding(
                    12,
                    0,
                    12,
                    0);

            btnLimpiar.Location =
                new Point(
                    765,
                    68);

            btnLimpiar.Click +=
                async (_, _) =>
                {
                    await LimpiarBusquedaDesdeInterfazAsync();
                };

            pnlBusqueda.Controls.Add(
                btnLimpiar);

            ButtonStyler.Apply(
                btnImprimirHtml,
                "Imprimir PDF",
                AppColors.Primary,
                AppIcons.Save,
                width: 165,
                height: 40);

            btnImprimirHtml.Parent =
                pnlBusqueda;

            btnImprimirHtml.Location =
                new Point(
                    915,
                    68);

            ActualizarEstadoBotonImprimir(
                false);

            // =====================================================
            // VISTA PREVIA
            // =====================================================

            Label lblVistaPrevia =
                new Label
                {
                    AutoSize =
                        true,

                    Text =
                        "Vista previa de credencial",

                    Location =
                        new Point(
                            20,
                            145),

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            13F,
                            FontStyle.Bold),

                    BackColor =
                        Color.Transparent
                };

            pnlContenido.Controls.Add(
                lblVistaPrevia);

            Panel pnlVista =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            175),

                    Size =
                        new Size(
                            1100,
                            410),

                    BackColor =
                        Color.White,

                    Padding =
                        new Padding(
                            1)
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlVista,
                10);

            pnlVista.Paint +=
                (_, e) =>
                {
                    using Pen pen =
                        new Pen(
                            AppColors.BorderMedium);

                    Rectangle rect =
                        new Rectangle(
                            0,
                            0,
                            pnlVista.Width - 1,
                            pnlVista.Height - 1);

                    using var path =
                        RoundedControlHelper.CreateRoundedPath(
                            rect,
                            10);

                    e.Graphics.DrawPath(
                        pen,
                        path);
                };

            webViewCertificacion.Parent =
                pnlVista;

            webViewCertificacion.Dock =
                DockStyle.Fill;

            webViewCertificacion.Margin =
                Padding.Empty;

            webViewCertificacion.DefaultBackgroundColor =
                Color.White;

            pnlContenido.Controls.Add(
                pnlVista);

            ResumeLayout(
                true);
        }

        private static Label CrearEtiquetaCampo(
            string texto,
            Point location)
        {
            return new Label
            {
                AutoSize =
                    true,

                Text =
                    texto,

                Location =
                    location,

                ForeColor =
                    AppColors.TextSecondary,

                Font =
                    AppFonts.Regular(
                        10F,
                        FontStyle.Bold),

                BackColor =
                    Color.Transparent
            };
        }

        private void ActualizarEstadoBotonImprimir(
            bool habilitado)
        {
            btnImprimirHtml.Enabled =
                habilitado;

            ButtonStyler.UpdateEnabledState(
                btnImprimirHtml,
                AppColors.Primary);
        }

        private async Task BuscarDesdeInterfazAsync()
        {
            string noReloj =
                txtNoReloj.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    noReloj))
            {
                AppDialog.ShowWarning(
                    this,
                    "Validación",
                    "Ingrese un número de reloj.");

                txtNoReloj.Focus();

                return;
            }

            UseWaitCursor =
                true;

            try
            {
                await ConsultarYMostrarCredencialAsync(
                    noReloj);
            }
            catch (Exception ex)
            {
                _noRelojCredencialActual =
                    string.Empty;

                ActualizarEstadoBotonImprimir(
                    false);

                AppDialog.ShowError(
                    this,
                    "Error al consultar",
                    "No fue posible consultar las certificaciones.\n\n" +
                    ex.Message);
            }
            finally
            {
                UseWaitCursor =
                    false;

                txtNoReloj.SelectAll();

                txtNoReloj.Focus();
            }
        }

        private async Task LimpiarBusquedaDesdeInterfazAsync()
        {
            _noRelojCredencialActual =
                string.Empty;

            LimpiarVistaBusqueda();

            txtNoReloj.Clear();

            ActualizarEstadoBotonImprimir(
                false);

            await MostrarHtmlInicialAsync();

            txtNoReloj.Focus();
        }

        private async void Verificaciones_Load(object sender, EventArgs e)
        {
            await InicializarWebViewAsync();
            MostrarSoloVistaHtml();
            await MostrarHtmlInicialAsync();

            if (!string.IsNullOrWhiteSpace(_noRelojInicial))
            {
                txtNoReloj.Text = _noRelojInicial;

                bool credencialGenerada = await ConsultarYMostrarCredencialAsync(_noRelojInicial);

                if (_imprimirAutomaticamente)
                {
                    if (credencialGenerada)
                    {
                        BeginInvoke(new Action(async () =>
                        {
                            try
                            {
                                await EsperarRenderWebViewAsync();

                                await GenerarPdfCredencialAsync(
                                    !_guardarDirectoEnDescargas,
                                    mostrarMensajes: false);
                            }
                            catch (Exception ex)
                            {
                                ErrorGeneracionPdf =
                                    "Ocurrió un error al generar la credencial.\n\n" +
                                    ex.Message;
                            }
                            finally
                            {
                                Close();
                            }
                        }));
                    }
                    else
                    {
                        BeginInvoke(new Action(Close));
                    }
                }

                txtNoReloj.SelectAll();
                txtNoReloj.Focus();
                return;
            }

            txtNoReloj.Clear();
            txtNoReloj.Focus();
        }

        private async Task<bool> ConsultarYMostrarCredencialAsync(string noReloj)
        {
            if (string.IsNullOrWhiteSpace(noReloj))
                return false;

            LimpiarVistaBusqueda();

            DataTable dt = Clases.Certificacion.ConsultarVerificacionNoReloj(noReloj);

            if (dt == null || dt.Rows.Count == 0)
            {
                _noRelojCredencialActual =
                    string.Empty;

                ActualizarEstadoBotonImprimir(
                    false);

                await MostrarHtmlInicialAsync();

                if (_imprimirAutomaticamente && _ocultarVentanaAlImprimir)
                {
                    ErrorGeneracionPdf =
                        "No se encontraron certificaciones para el trabajador seleccionado.";
                }
                else
                {
                    AppDialog.ShowInfo(
                        this,
                        "Sin certificaciones",
                        "No se encontraron certificaciones para el trabajador seleccionado.");
                }

                txtNoReloj.SelectAll();
                txtNoReloj.Focus();

                return false;
            }

            dgvCertificaciones.AutoGenerateColumns = true;
            dgvCertificaciones.DataSource = dt;

            string nombre = dt.Rows[0]["Nombre"]?.ToString() ?? "";

            txtNombre.Text = nombre;
            lblMostrarNoReloj.Text = noReloj;
            lblMostrarNombre.Text = nombre;

            OcultarColumnas();
            PintarCertificaciones();
            GenerarProcesosCertificados();

            MostrarSoloVistaHtml();

            bool htmlGenerado =
                await MostrarCertificacionHtmlAsync();

            if (!htmlGenerado)
            {
                _noRelojCredencialActual =
                    string.Empty;

                ActualizarEstadoBotonImprimir(
                    false);

                return false;
            }

            _noRelojCredencialActual =
                noReloj;

            ActualizarEstadoBotonImprimir(
                true);

            return true;
        }

        private Task NavegarHtmlAsync(string html)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void Handler(object? sender, CoreWebView2NavigationCompletedEventArgs e)
            {
                webViewCertificacion.CoreWebView2.NavigationCompleted -= Handler;
                tcs.TrySetResult(true);
            }

            webViewCertificacion.CoreWebView2.NavigationCompleted += Handler;
            webViewCertificacion.NavigateToString(html);

            return tcs.Task;
        }
        private void MostrarSoloVistaHtml()
        {
            pnlAdelante.Visible = false;
            pnlReverso.Visible = false;

            //webViewCertificacion.Location = new Point(123, 138);
            //webViewCertificacion.Size = new Size(1034, 645);


        }
        private async Task MostrarHtmlInicialAsync()
        {
            if (webViewCertificacion.CoreWebView2 == null)
                await InicializarWebViewAsync();

            webViewCertificacion.NavigateToString(@"
    <html>
    <body style='font-family:Arial;display:flex;justify-content:center;align-items:center;height:100vh;margin:0;background:#f3f4f6;'>
        <div style='text-align:center;color:#374151;'>
            <h2 style='margin-bottom:8px;'>Vista previa de credencial</h2>
            <p>Ingrese un número de reloj para generar la credencial.</p>
        </div>
    </body>
    </html>");
        }
        private void LimpiarVistaBusqueda()
        {
            dgvCertificaciones.DataSource = null;
            dgvCertificaciones.Columns.Clear();

            txtNombre.Clear();
            lblMostrarNoReloj.Text = "";
            lblMostrarNombre.Text = "";
            flowProcesosCertificados.Controls.Clear();
        }

        private async Task InicializarWebViewAsync()
        {
            await webViewCertificacion.EnsureCoreWebView2Async();

            RegistrarEventosWebView2();
        }

        private void RegistrarEventosWebView2()
        {
            if (_eventosWebViewRegistrados ||
                webViewCertificacion.CoreWebView2 is null)
            {
                return;
            }

            webViewCertificacion.CoreWebView2.ProcessFailed +=
                CoreWebView2_ProcessFailed;

            _eventosWebViewRegistrados =
                true;
        }

        private void CoreWebView2_ProcessFailed(
            object? sender,
            CoreWebView2ProcessFailedEventArgs e)
        {
            try
            {
                string tipo =
                    e.ProcessFailedKind.ToString();

                string razon =
                    e.Reason.ToString();

                bool fatal =
                    tipo.Equals(
                        "BrowserProcessExited",
                        StringComparison.OrdinalIgnoreCase) ||
                    tipo.Equals(
                        "RenderProcessExited",
                        StringComparison.OrdinalIgnoreCase);

                if (fatal)
                {
                    _webViewConFalloFatal =
                        true;

                    _ultimoFalloWebView =
                        $"{tipo} / {razon} / ExitCode: {e.ExitCode}";
                }

                RegistrarLogWebView2(
                    e);
            }
            catch
            {
                // Nunca permitimos que el propio diagnóstico
                // provoque otra excepción durante ProcessFailed.
            }
        }

        private void RegistrarLogWebView2(
            CoreWebView2ProcessFailedEventArgs e)
        {
            string carpetaLogs =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "Aseguranza",
                    "Logs");

            Directory.CreateDirectory(
                carpetaLogs);

            string rutaLog =
                Path.Combine(
                    carpetaLogs,
                    "webview2.log");

            StringBuilder sb =
                new StringBuilder();

            sb.AppendLine(
                "============================================================");

            sb.AppendLine(
                $"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

            sb.AppendLine(
                $"ProcessFailedKind: {e.ProcessFailedKind}");

            sb.AppendLine(
                $"Reason: {e.Reason}");

            sb.AppendLine(
                $"ExitCode: {e.ExitCode}");

            sb.AppendLine(
                $"ProcessDescription: {e.ProcessDescription}");

            sb.AppendLine(
                $"FailureSourceModulePath: {e.FailureSourceModulePath}");

            if (webViewCertificacion.CoreWebView2 is not null)
            {
                sb.AppendLine(
                    $"Runtime: {webViewCertificacion.CoreWebView2.Environment.BrowserVersionString}");

                sb.AppendLine(
                    $"FailureReportFolder: {webViewCertificacion.CoreWebView2.Environment.FailureReportFolderPath}");
            }

            sb.AppendLine();

            File.AppendAllText(
                rutaLog,
                sb.ToString(),
                Encoding.UTF8);
        }

        private async void txtNoReloj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            e.Handled = true;

            await BuscarDesdeInterfazAsync();
        }

        private void txtNoReloj_TextChanged(object sender, EventArgs e)
        {
            string textoActual =
                txtNoReloj.Text.Trim();

            bool coincideConCredencial =
                !string.IsNullOrWhiteSpace(
                    _noRelojCredencialActual) &&
                textoActual.Equals(
                    _noRelojCredencialActual,
                    StringComparison.OrdinalIgnoreCase);

            ActualizarEstadoBotonImprimir(
                coincideConCredencial);
        }

        private void OcultarColumnas()
        {
            string[] columnasOcultas =
            {
        "Id",
        "IdTrabajador",
        "Nombre",
        "RutaFoto",
        "IdProceso",
        "Comentario",
        "IdCertificador",
        "FechaVencimiento",
        "NombreCertificador"
    };

            foreach (string col in columnasOcultas)
            {
                if (dgvCertificaciones.Columns.Contains(col))
                    dgvCertificaciones.Columns[col].Visible = false;
            }
        }

        private void PintarCertificaciones()
        {
            if (!dgvCertificaciones.Columns.Contains("DiasRestantes")) return;

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow) continue;

                var v = row.Cells["DiasRestantes"].Value;
                if (v == null || v == DBNull.Value) continue;

                int dias = Convert.ToInt32(v);

                if (dias < 0)
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                else if (dias <= 30)
                    row.DefaultCellStyle.BackColor = Color.Khaki;
                else
                    row.DefaultCellStyle.BackColor = Color.LightGreen;

                row.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void MostrarVistaCompleta()
        {
            // La interfaz moderna utiliza únicamente WebView2 como
            // vista visible. Los paneles WinForms antiguos se
            // conservan ocultos como soporte de datos.
            pnlAdelante.Visible =
                false;

            pnlReverso.Visible =
                false;
        }
        private void GenerarProcesosCertificados()
        {
            flowProcesosCertificados.Controls.Clear();

            if (!dgvCertificaciones.Columns.Contains("Proceso")) return;
            if (!dgvCertificaciones.Columns.Contains("DiasRestantes")) return;

            HashSet<string> procesosMostrados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow) continue;

                var procesoObj = row.Cells["Proceso"].Value;
                string proceso = procesoObj?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(proceso)) continue;

                if (!procesosMostrados.Add(proceso)) continue;

                var diasObj = row.Cells["DiasRestantes"].Value;
                if (diasObj == null || diasObj == DBNull.Value) continue;

                int diasRestantes = Convert.ToInt32(diasObj);

                Color color = ObtenerColorPorVencimiento(diasRestantes);

                CrearLabelProceso(proceso, diasRestantes, color);
            }
        }

        private Color ObtenerColorPorVencimiento(int dias)
        {
            if (dias < 0)
                return Color.FromArgb(231, 76, 60);

            if (dias <= 30)
                return Color.FromArgb(241, 196, 15);

            return Color.FromArgb(31, 117, 0);
        }

        private void CrearLabelProceso(string proceso, int dias, Color color)
        {
            Label lbl = new Label();
            lbl.Text = proceso;
            lbl.AutoSize = false;
            lbl.Width = 100;
            lbl.Height = 20;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.BackColor = color;
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Arial", 9F, FontStyle.Bold);
            lbl.Margin = new Padding(6);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Cursor = Cursors.Hand;

            ToolTip tip = new ToolTip();
            tip.SetToolTip(lbl, $"Días restantes: {dias}");

            flowProcesosCertificados.Controls.Add(lbl);
        }

        private async Task<bool> MostrarCertificacionHtmlAsync()
        {
            if (dgvCertificaciones.Rows.Count == 0)
                return false;

            if (webViewCertificacion.CoreWebView2 == null)
                await InicializarWebViewAsync();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string rutaHtml = Path.Combine(baseDir, "Plantillas", "Certificacion.html");

            if (!File.Exists(rutaHtml))
            {
                string mensaje =
                    "No se encontró la plantilla HTML necesaria para generar la credencial.\n\n" +
                    rutaHtml;

                if (_imprimirAutomaticamente &&
                    _ocultarVentanaAlImprimir)
                {
                    ErrorGeneracionPdf =
                        mensaje;
                }
                else
                {
                    AppDialog.ShowError(
                        this,
                        "Plantilla no encontrada",
                        mensaje);
                }

                return false;
            }

            string html = File.ReadAllText(rutaHtml, Encoding.UTF8);

            string reloj = txtNoReloj.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            html = html.Replace("{{RELOJ}}", reloj);
            html = html.Replace("{{EMPLEADO}}", nombre);
            html = html.Replace("{{NOMBRE}}", nombre);
            html = html.Replace("{{PROCESOS_CERTIFICADOS}}", GenerarProcesosCertificadosHtmlDinamicos());
            html = html.Replace("{{FOTO_HTML}}", ObtenerFotoHtml());
            html = html.Replace("{{TABLA_PROCESOS}}", GenerarTablaProcesosHtml());

            await NavegarHtmlAsync(html);

            return true;
        }

        private async Task EsperarRenderWebViewAsync()
        {
            if (webViewCertificacion.CoreWebView2 is null)
            {
                await InicializarWebViewAsync();
            }

            var coreWebView2 =
                webViewCertificacion.CoreWebView2;

            if (coreWebView2 is null)
            {
                throw new InvalidOperationException(
                    "No fue posible inicializar WebView2.");
            }

            // Pequeña espera para que WebView termine de pintar
            // la credencial antes de generar el PDF.
            await Task.Delay(1200);

            try
            {
                await coreWebView2.ExecuteScriptAsync(
                    "document.body.offsetHeight;");
            }
            catch
            {
                // Si falla el script, no detenemos la impresión.
            }
        }

        private string GenerarProcesosCertificadosHtmlFijos()
        {
            string[] procesosBase =
            {
        "INSP", "P/E", "CLIPS", "AMARRES", "PROTECT.",
        "DUMMYS", "TSK", "PICAS", "MOLEX", "ANTENAS"
    };

            Dictionary<string, int> estadoProcesos = ObtenerEstadoProcesos();
            StringBuilder sb = new StringBuilder();

            foreach (string proceso in procesosBase)
            {
                string color = "#FFFFFF";
                string colorTexto = "#000000";
                string peso = "800";

                if (estadoProcesos.ContainsKey(proceso))
                {
                    int dias = estadoProcesos[proceso];
                    color = ObtenerColorHtmlPorVencimiento(dias);
                    colorTexto = "#FFFFFF";
                }

                sb.Append($@"
<div style='
    background:{color};
    color:{colorTexto};
    font-weight:{peso};
'>
    {System.Net.WebUtility.HtmlEncode(proceso)}
</div>");
            }

            return sb.ToString();
        }

        private string GenerarProcesosCertificadosHtmlDinamicos()
        {
            StringBuilder sb = new StringBuilder();

            if (!dgvCertificaciones.Columns.Contains("Proceso"))
                return "";

            if (!dgvCertificaciones.Columns.Contains("DiasRestantes"))
                return "";

            HashSet<string> procesosMostrados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string proceso = row.Cells["Proceso"]?.Value?.ToString()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(proceso))
                    continue;

                if (!procesosMostrados.Add(proceso))
                    continue;

                object? diasObj =
                    row.Cells["DiasRestantes"]?.Value;

                if (diasObj == null || diasObj == DBNull.Value)
                    continue;

                int diasRestantes = Convert.ToInt32(diasObj);

                string colorFondo = ObtenerColorHtmlPorVencimiento(diasRestantes);
                string procesoHtml = System.Net.WebUtility.HtmlEncode(proceso);

                string colorTexto =
    diasRestantes >= 0 &&
    diasRestantes <= 30
        ? "#000000"
        : "#FFFFFF";

                sb.Append($@"
    <div style='
        background:{colorFondo};
        color:{colorTexto};
    '>
        {procesoHtml}
    </div>
");
            }

            if (sb.Length == 0)
            {
                return @"
    <div style='
        background:#dc2626;
        color:white;
    '>
        SIN CERTIFICACIONES
    </div>
";
            }

            return sb.ToString();
        }

        private Dictionary<string, int> ObtenerEstadoProcesos()
        {
            Dictionary<string, int> procesos = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (!dgvCertificaciones.Columns.Contains("Proceso")) return procesos;
            if (!dgvCertificaciones.Columns.Contains("DiasRestantes")) return procesos;

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow) continue;

                string procesoOriginal = row.Cells["Proceso"]?.Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(procesoOriginal)) continue;

                object? diasObj =
                    row.Cells["DiasRestantes"]?.Value;
                if (diasObj == null || diasObj == DBNull.Value) continue;

                int dias = Convert.ToInt32(diasObj);
                string procesoMapeado = MapearProcesoBase(procesoOriginal);

                if (string.IsNullOrWhiteSpace(procesoMapeado)) continue;

                if (!procesos.ContainsKey(procesoMapeado))
                {
                    procesos.Add(procesoMapeado, dias);
                }
                else
                {
                    if (dias < procesos[procesoMapeado])
                        procesos[procesoMapeado] = dias;
                }
            }

            return procesos;
        }

        private string MapearProcesoBase(string proceso)
        {
            string p = proceso.Trim().ToUpper();

            if (p.Contains("INSP")) return "INSP";
            if (p.Contains("P/E")) return "P/E";
            if (p.Contains("CLIPS")) return "CLIPS";
            if (p.Contains("AMARRE")) return "AMARRES";
            if (p.Contains("PROTECT")) return "PROTECT.";
            if (p.Contains("DUMMY")) return "DUMMYS";
            if (p.Contains("TSK")) return "TSK";
            if (p.Contains("PICA")) return "PICAS";
            if (p.Contains("MOLEX")) return "MOLEX";
            if (p.Contains("ANTENA")) return "ANTENAS";

            return "";
        }

        private string ObtenerColorHtmlPorVencimiento(int dias)
        {
            if (dias < 0)
                return "#E74C3C";

            if (dias <= 30)
                return "#F1C40F";

            return "#1F7500";
        }

        private string GenerarTablaProcesosHtml()
        {
            StringBuilder sb = new StringBuilder();

            int numero = 1;
            int maxFilas = 8;

            foreach (DataGridViewRow row in dgvCertificaciones.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (numero > maxFilas)
                    break;

                string proceso =
                    System.Net.WebUtility.HtmlEncode(
                        ObtenerTextoCelda(
                            row,
                            "Proceso"));

                string fechaCert =
                    System.Net.WebUtility.HtmlEncode(
                        ObtenerTextoCelda(
                            row,
                            "FechaCertificacion"));

                string nombreCertificadorCompleto =
                    ObtenerTextoCelda(
                        row,
                        "NombreCertificador");

                string certificador =
                    System.Net.WebUtility.HtmlEncode(
                        AbreviarNombreCertificador(
                            nombreCertificadorCompleto));

                string vigencia =
                    System.Net.WebUtility.HtmlEncode(
                        ObtenerTextoCelda(
                            row,
                            "FechaVencimiento"));

                object? diasObj =
                    row.Cells["DiasRestantes"]?.Value;

                bool noCertificado = false;

                int? diasRestantes = null;

                if (diasObj is not null &&
                    diasObj != DBNull.Value)
                {
                    int dias =
                        Convert.ToInt32(diasObj);

                    diasRestantes = dias;

                    noCertificado =
                        dias < 0 &&
                        string.IsNullOrWhiteSpace(
                            fechaCert);
                }

                string estiloFila =
                    noCertificado
                        ? "background:#dc2626;color:white;font-weight:bold;"
                        : "background:#efefef;color:black;";

                if (string.IsNullOrWhiteSpace(proceso))
                {
                    proceso = "";
                }

                if (string.IsNullOrWhiteSpace(fechaCert))
                {
                    fechaCert =
                        noCertificado
                            ? "NO CERTIFICADO"
                            : "";
                }

                if (string.IsNullOrWhiteSpace(certificador))
                {
                    certificador =
                        noCertificado
                            ? "NO CERTIFICADO"
                            : "";
                }

                if (string.IsNullOrWhiteSpace(vigencia))
                {
                    vigencia =
                        noCertificado
                            ? "NO CERTIFICADO"
                            : "";
                }

                if (noCertificado &&
                    string.IsNullOrWhiteSpace(proceso))
                {
                    proceso =
                        "NO CERTIFICADO";
                }

                /*
                 * Color únicamente para la celda VIGENCIA.
                 *
                 * Verde   = vigente
                 * Amarillo = 30 días o menos
                 * Rojo    = vencida
                 */
                string estiloVigencia =
                    string.Empty;

                if (!noCertificado &&
                    diasRestantes.HasValue &&
                    !string.IsNullOrWhiteSpace(vigencia))
                {
                    string colorFondo =
                        ObtenerColorHtmlPorVencimiento(
                            diasRestantes.Value);

                    string colorTexto;

                    if (diasRestantes.Value >= 0 &&
                        diasRestantes.Value <= 30)
                    {
                        // Amarillo: texto negro para mejor contraste.
                        colorTexto = "#000000";
                    }
                    else
                    {
                        // Verde o rojo.
                        colorTexto = "#FFFFFF";
                    }

                    estiloVigencia =
                        $"background:{colorFondo};" +
                        $"color:{colorTexto};" +
                        "font-weight:700;";
                }

                sb.Append($@"
<tr style='{estiloFila}'>
    <td>{numero}</td>
    <td>{proceso}</td>
    <td>{fechaCert}</td>
    <td>{certificador}</td>
    <td style='{estiloVigencia}'>{vigencia}</td>
</tr>");

                numero++;
            }

            while (numero <= maxFilas)
            {
                sb.Append($@"
<tr>
    <td>{numero}</td>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
</tr>");

                numero++;
            }

            return sb.ToString();
        }

        private string ObtenerTextoCelda(
            DataGridViewRow row,
            string nombreColumna)
        {
            if (!dgvCertificaciones.Columns.Contains(
                nombreColumna))
            {
                return string.Empty;
            }

            object? valor =
                row.Cells[nombreColumna]?.Value;

            if (valor is null ||
                valor == DBNull.Value)
            {
                return string.Empty;
            }

            if (valor is DateTime fecha)
            {
                return fecha.ToString("dd/MM/yy");
            }

            string texto =
                Convert.ToString(valor)
                ?? string.Empty;

            if (DateTime.TryParse(
                texto,
                out DateTime fechaParseada))
            {
                return fechaParseada.ToString(
                    "dd/MM/yy");
            }

            return texto;
        }

        private string AbreviarNombreCertificador(string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return "";

            string[] partes = nombreCompleto
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0)
                return "";

            // Primer apellido o primera palabra completa
            string resultado = partes[0].ToUpper();

            // Iniciales del resto
            for (int i = 1; i < partes.Length; i++)
            {
                resultado += " " + char.ToUpper(partes[i][0]) + ".";
            }

            return resultado;
        }

        private string ObtenerFotoHtml()
        {
            if (dgvCertificaciones.Rows.Count > 0 && dgvCertificaciones.Columns.Contains("RutaFoto"))
            {
                string ruta = dgvCertificaciones.Rows[0].Cells["RutaFoto"]?.Value?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(ruta) && File.Exists(ruta))
                {
                    string extension = Path.GetExtension(ruta).ToLower();
                    string mimeType = "image/jpeg";

                    if (extension == ".png") mimeType = "image/png";
                    else if (extension == ".bmp") mimeType = "image/bmp";
                    else if (extension == ".webp") mimeType = "image/webp";

                    byte[] bytes = File.ReadAllBytes(ruta);
                    string base64 = Convert.ToBase64String(bytes);

                    return $"<img src='data:{mimeType};base64,{base64}' style='width:100%;height:100%;object-fit:cover;' />";
                }
            }

            if (pictureBox1.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    string base64 = Convert.ToBase64String(ms.ToArray());
                    return $"<img src='data:image/jpeg;base64,{base64}' style='width:100%;height:100%;object-fit:cover;' />";
                }
            }

            return "<span style='font-size:12px;'>SIN FOTO</span>";
        }

        private async Task GenerarPdfCredencialAsync(
            bool pedirRuta = true,
            bool mostrarMensajes = true)
        {
            RutaPdfGenerado =
                null;

            ErrorGeneracionPdf =
                null;

            string reloj =
                txtNoReloj.Text.Trim();

            string nombre =
                txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(reloj) ||
                string.IsNullOrWhiteSpace(nombre))
            {
                ErrorGeneracionPdf =
                    "No hay datos suficientes para generar la credencial.";

                if (mostrarMensajes)
                {
                    AppDialog.ShowWarning(
                        this,
                        "Información insuficiente",
                        ErrorGeneracionPdf);
                }

                return;
            }

            string nombreArchivo =
                LimpiarTextoParaArchivo(
                    nombre);

            string archivo =
                $"Credencial_{reloj}_{nombreArchivo}.pdf";

            string rutaDestino;

            // Elegimos primero la ruta. Así no tocamos WebView2
            // mientras el usuario está entrando al SaveFileDialog.
            if (pedirRuta)
            {
                using SaveFileDialog saveFileDialog =
                    new SaveFileDialog();

                saveFileDialog.Title =
                    "Guardar credencial";

                saveFileDialog.Filter =
                    "Archivo PDF (*.pdf)|*.pdf";

                saveFileDialog.FileName =
                    archivo;

                saveFileDialog.AddExtension =
                    true;

                saveFileDialog.DefaultExt =
                    "pdf";

                // Desactivamos la confirmación nativa de sobrescritura.
                // En algunas cadenas de formularios modales de WinForms,
                // esa segunda ventana de Windows puede quedar detrás del
                // formulario propietario. La confirmación la manejamos
                // nosotros con AppDialog.
                saveFileDialog.OverwritePrompt =
                    false;

                if (saveFileDialog.ShowDialog(this) !=
                    DialogResult.OK)
                {
                    return;
                }

                rutaDestino =
                    saveFileDialog.FileName;

                if (File.Exists(
                        rutaDestino))
                {
                    bool reemplazar =
                        AppDialog.Confirm(
                            this,
                            "Archivo existente",
                            "Ya existe un archivo con ese nombre." +
                            Environment.NewLine +
                            Environment.NewLine +
                            Path.GetFileName(
                                rutaDestino) +
                            Environment.NewLine +
                            Environment.NewLine +
                            "¿Desea reemplazarlo?",
                            "Reemplazar");

                    if (!reemplazar)
                    {
                        return;
                    }
                }
            }
            else
            {
                string carpetaDescargas =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.UserProfile),
                        "Downloads");

                if (!Directory.Exists(
                        carpetaDescargas))
                {
                    carpetaDescargas =
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.DesktopDirectory);
                }

                rutaDestino =
                    ObtenerRutaDisponible(
                        Path.Combine(
                            carpetaDescargas,
                            archivo));
            }

            if (webViewCertificacion.CoreWebView2 is null)
            {
                await InicializarWebViewAsync();
            }

            if (_webViewConFalloFatal)
            {
                throw new InvalidOperationException(
                    "WebView2 presentó un fallo de proceso antes de imprimir." +
                    Environment.NewLine +
                    Environment.NewLine +
                    _ultimoFalloWebView);
            }

            await EsperarRenderWebViewAsync();

            CoreWebView2? coreWebView2 =
                webViewCertificacion.CoreWebView2;

            if (coreWebView2 is null)
            {
                throw new InvalidOperationException(
                    "No fue posible inicializar WebView2 para generar el PDF.");
            }

            string rutaTemporal =
                rutaDestino +
                ".tmp";

            try
            {
                if (File.Exists(
                        rutaTemporal))
                {
                    File.Delete(
                        rutaTemporal);
                }

                // WebView2 genera el PDF en memoria. La escritura
                // física del archivo la hacemos nosotros con .NET.
                using Stream pdfStream =
                    await coreWebView2.PrintToPdfStreamAsync(
                        null);

                if (pdfStream is null ||
                    !pdfStream.CanRead)
                {
                    throw new InvalidOperationException(
                        "WebView2 no devolvió datos PDF válidos.");
                }

                await using (
                    FileStream archivoPdf =
                        new FileStream(
                            rutaTemporal,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None,
                            bufferSize: 81920,
                            useAsync: true))
                {
                    await pdfStream.CopyToAsync(
                        archivoPdf);

                    await archivoPdf.FlushAsync();
                }

                File.Move(
                    rutaTemporal,
                    rutaDestino,
                    overwrite: true);

                RutaPdfGenerado =
                    rutaDestino;

                if (mostrarMensajes)
                {
                    AppDialog.ShowInfo(
                        this,
                        "Credencial generada",
                        "La credencial se generó correctamente." +
                        Environment.NewLine +
                        Environment.NewLine +
                        $"Archivo:{Environment.NewLine}{rutaDestino}");
                }
            }
            catch
            {
                try
                {
                    if (File.Exists(
                            rutaTemporal))
                    {
                        File.Delete(
                            rutaTemporal);
                    }
                }
                catch
                {
                    // La limpieza del temporal nunca debe ocultar
                    // la excepción original.
                }

                throw;
            }
        }

        private string LimpiarTextoParaArchivo(string texto)
        {
            string limpio = texto.ToUpper().Trim();

            foreach (char c in Path.GetInvalidFileNameChars())
                limpio = limpio.Replace(c.ToString(), "");

            limpio = limpio.Replace(",", "");
            limpio = limpio.Replace(".", "");
            limpio = limpio.Replace("  ", " ");
            limpio = limpio.Replace(" ", "_");

            return limpio;
        }

        private string ObtenerRutaDisponible(string ruta)
        {
            if (!File.Exists(ruta))
                return ruta;

            string carpeta = Path.GetDirectoryName(ruta) ?? "";
            string nombre = Path.GetFileNameWithoutExtension(ruta);
            string extension = Path.GetExtension(ruta);

            int contador = 2;
            string nuevaRuta;

            do
            {
                nuevaRuta = Path.Combine(carpeta, $"{nombre}_{contador}{extension}");
                contador++;
            }
            while (File.Exists(nuevaRuta));

            return nuevaRuta;
        }

        private async void btnImprimirHtml_Click(object sender, EventArgs e)
        {
            // Un evento async void no debe dejar escapar excepciones:
            // si WebView2 o el diálogo de guardado falla, WinForms puede
            // considerar la excepción no controlada y cerrar el proceso.
            if (_generandoPdf)
            {
                return;
            }

            _generandoPdf =
                true;

            try
            {
                ActualizarEstadoBotonImprimir(
                    false);

                UseWaitCursor =
                    true;

                await GenerarPdfCredencialAsync(
                    pedirRuta: true,
                    mostrarMensajes: true);
            }
            catch (Exception ex)
            {
                RutaPdfGenerado =
                    null;

                ErrorGeneracionPdf =
                    "Ocurrió un error al generar el PDF de la credencial.\n\n" +
                    ex.Message;

                System.Diagnostics.Debug.WriteLine(
                    ex);

                if (!IsDisposed &&
                    IsHandleCreated)
                {
                    AppDialog.ShowError(
                        this,
                        "Error al imprimir",
                        ErrorGeneracionPdf);
                }
            }
            finally
            {
                _generandoPdf =
                    false;

                UseWaitCursor =
                    false;

                if (!IsDisposed &&
                    IsHandleCreated)
                {
                    string textoActual =
                        txtNoReloj.Text.Trim();

                    bool coincideConCredencial =
                        !string.IsNullOrWhiteSpace(
                            _noRelojCredencialActual) &&
                        textoActual.Equals(
                            _noRelojCredencialActual,
                            StringComparison.OrdinalIgnoreCase);

                    ActualizarEstadoBotonImprimir(
                        coincideConCredencial);
                }
            }
        }
        private async Task ImprimirDirectoAsync()
        {
            if (webViewCertificacion.CoreWebView2 is null)
            {
                await InicializarWebViewAsync();
            }

            var coreWebView2 =
                webViewCertificacion.CoreWebView2;

            if (coreWebView2 is null)
            {
                throw new InvalidOperationException(
                    "No fue posible inicializar WebView2 para imprimir.");
            }

            await coreWebView2.ExecuteScriptAsync(
                "window.print();");
        }
    }
}