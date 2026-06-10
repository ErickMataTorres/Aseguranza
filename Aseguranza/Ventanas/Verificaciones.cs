using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Web.WebView2.Core;

namespace Aseguranza.Ventanas
{
    public partial class Verificaciones : Form
    {
        public Verificaciones()
        {
            InitializeComponent();
        }

        private async void Verificaciones_Load(object sender, EventArgs e)
        {
            await InicializarWebViewAsync();
            MostrarSoloVistaHtml();
            await MostrarHtmlInicialAsync();

            txtNoReloj.Clear();
            txtNoReloj.Focus();
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
        }

        private async void txtNoReloj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            e.Handled = true;

            string noReloj = txtNoReloj.Text.Trim();
            if (string.IsNullOrWhiteSpace(noReloj))
                return;

            LimpiarVistaBusqueda();

            DataTable dt = Clases.Certificacion.ConsultarVerificacionNoReloj(noReloj);

            if (dt == null || dt.Rows.Count == 0)
            {
                MostrarVistaCompleta();
                await MostrarHtmlInicialAsync();

                MessageBox.Show("No se encontraron certificaciones.");

                txtNoReloj.SelectAll();
                txtNoReloj.Focus();
                return;
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
            await MostrarCertificacionHtmlAsync();

            // ✅ deja listo el textbox para escribir otro número encima
            txtNoReloj.SelectAll();
            txtNoReloj.Focus();
        }

        private void txtNoReloj_TextChanged(object sender, EventArgs e)
        {
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
            pnlAdelante.Visible = true;
            pnlReverso.Visible = true;

            //webViewCertificacion.Location = new Point(123, 465);
            //webViewCertificacion.Size = new Size(906, 318);


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

        private async Task MostrarCertificacionHtmlAsync()
        {
            if (dgvCertificaciones.Rows.Count == 0) return;

            if (webViewCertificacion.CoreWebView2 == null)
                await InicializarWebViewAsync();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string rutaHtml = Path.Combine(baseDir, "Plantillas", "Certificacion.html");

            if (!File.Exists(rutaHtml))
            {
                MessageBox.Show("No se encontró la plantilla HTML: " + rutaHtml);
                return;
            }

            string html = File.ReadAllText(rutaHtml, Encoding.UTF8);

            string reloj = txtNoReloj.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            html = html.Replace("{{RELOJ}}", reloj);
            html = html.Replace("{{EMPLEADO}}", nombre);
            html = html.Replace("{{NOMBRE}}", nombre);
            html = html.Replace("{{PROCESOS_CERTIFICADOS}}", GenerarProcesosCertificadosHtmlFijos());
            html = html.Replace("{{FOTO_HTML}}", ObtenerFotoHtml());
            html = html.Replace("{{TABLA_PROCESOS}}", GenerarTablaProcesosHtml());

            webViewCertificacion.NavigateToString(html);
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

                object diasObj = row.Cells["DiasRestantes"]?.Value;
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
                if (row.IsNewRow) continue;
                if (numero > maxFilas) break;

                string proceso = System.Net.WebUtility.HtmlEncode(ObtenerTextoCelda(row, "Proceso"));
                string fechaCert = System.Net.WebUtility.HtmlEncode(ObtenerTextoCelda(row, "FechaCertificacion"));
                string nombreCertificadorCompleto = ObtenerTextoCelda(row, "NombreCertificador");
                string certificador = System.Net.WebUtility.HtmlEncode(AbreviarNombreCertificador(nombreCertificadorCompleto));
                string vigencia = System.Net.WebUtility.HtmlEncode(ObtenerTextoCelda(row, "FechaVencimiento"));

                object diasObj = row.Cells["DiasRestantes"]?.Value;
                bool noCertificado = false;

                if (diasObj != null && diasObj != DBNull.Value)
                {
                    int dias = Convert.ToInt32(diasObj);
                    noCertificado = dias < 0 && string.IsNullOrWhiteSpace(fechaCert);
                }

                string estiloFila = noCertificado
                    ? "background:#dc2626;color:white;font-weight:bold;"
                    : "background:#efefef;color:black;";

                if (string.IsNullOrWhiteSpace(proceso))
                    proceso = "";

                if (string.IsNullOrWhiteSpace(fechaCert))
                    fechaCert = noCertificado ? "NO CERTIFICADO" : "";

                if (string.IsNullOrWhiteSpace(certificador))
                    certificador = noCertificado ? "NO CERTIFICADO" : "";

                if (string.IsNullOrWhiteSpace(vigencia))
                    vigencia = noCertificado ? "NO CERTIFICADO" : "";

                if (noCertificado && string.IsNullOrWhiteSpace(proceso))
                    proceso = "NO CERTIFICADO";

                sb.Append($@"
<tr style='{estiloFila}'>
    <td>{numero}</td>
    <td>{proceso}</td>
    <td>{fechaCert}</td>
    <td>{certificador}</td>
    <td>{vigencia}</td>
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

        private string ObtenerTextoCelda(DataGridViewRow row, string nombreColumna)
        {
            if (!dgvCertificaciones.Columns.Contains(nombreColumna))
                return "";

            object valor = row.Cells[nombreColumna]?.Value;

            if (valor == null || valor == DBNull.Value)
                return "";

            if (valor is DateTime fecha)
                return fecha.ToString("dd/MM/yy");

            if (DateTime.TryParse(valor.ToString(), out DateTime fechaParseada))
                return fechaParseada.ToString("dd/MM/yy");

            return valor.ToString();
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

        private async Task GenerarPdfCredencialAsync()
        {
            if (webViewCertificacion.CoreWebView2 == null)
                await InicializarWebViewAsync();

            string reloj = txtNoReloj.Text.Trim();
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(reloj) || string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("No hay datos suficientes para generar la credencial.", "Información");
                return;
            }

            string nombreArchivo = LimpiarTextoParaArchivo(nombre);

            string archivo = $"Credencial_{reloj}_{nombreArchivo}.pdf";

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Guardar credencial";
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = archivo;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                bool resultado = await webViewCertificacion.CoreWebView2.PrintToPdfAsync(saveFileDialog.FileName);

                if (resultado)
                    MessageBox.Show("Credencial generada correctamente.", "Información");
                else
                    MessageBox.Show("No se pudo generar el PDF.", "Información");
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

        private async void btnImprimirHtml_Click(object sender, EventArgs e)
        {
            await GenerarPdfCredencialAsync();
        }
        private async Task ImprimirDirectoAsync()
        {
            if (webViewCertificacion.CoreWebView2 == null)
                await InicializarWebViewAsync();

            await webViewCertificacion.CoreWebView2.ExecuteScriptAsync("window.print();");
        }
    }
}