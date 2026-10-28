using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseguranza.Clases
{
    // WebView2 is STA-bound; the caller invokes this on the WinForms UI thread.
    public static class ExpirationReportPdfWriter
    {
        public static async Task WriteAsync(Form owner, string html, string destination)
        {
            ArgumentNullException.ThrowIfNull(owner);
            string tempHtml = Path.Combine(Path.GetTempPath(),
                $"Aseguranza_Vencimientos_{Guid.NewGuid():N}.html");
            string tempPdf = destination + $".{Guid.NewGuid():N}.tmp";
            using WebView2 browser = new()
            {
                Size = new Size(1, 1),
                Visible = false,
                TabStop = false
            };
            owner.Controls.Add(browser);
            try
            {
                await File.WriteAllTextAsync(tempHtml, html, new UTF8Encoding(false));
                await browser.EnsureCoreWebView2Async();
                CoreWebView2 core = browser.CoreWebView2 ??
                    throw new InvalidOperationException("WebView2 no se inicializó.");
                TaskCompletionSource<bool> navigation = new(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                void Completed(object? sender, CoreWebView2NavigationCompletedEventArgs e)
                {
                    core.NavigationCompleted -= Completed;
                    if (e.IsSuccess) navigation.TrySetResult(true);
                    else navigation.TrySetException(new InvalidOperationException(
                        $"WebView2 no pudo cargar el reporte: {e.WebErrorStatus}."));
                }
                core.NavigationCompleted += Completed;
                core.Navigate(new Uri(tempHtml).AbsoluteUri);
                await navigation.Task;
                await core.ExecuteScriptAsync("document.fonts.ready.then(() => Promise.all(Array.from(document.images, i => i.complete ? Promise.resolve() : new Promise(r => { i.onload = r; i.onerror = r; }))))");
                CoreWebView2PrintSettings settings = core.Environment.CreatePrintSettings();
                settings.Orientation = CoreWebView2PrintOrientation.Landscape;
                settings.ShouldPrintHeaderAndFooter = false;
                settings.ShouldPrintBackgrounds = true;
                using Stream pdf = await core.PrintToPdfStreamAsync(settings);
                if (pdf is null || !pdf.CanRead)
                    throw new InvalidOperationException("WebView2 no devolvió un PDF válido.");
                await using (FileStream output = new(tempPdf, FileMode.CreateNew,
                    FileAccess.Write, FileShare.None, 81920, useAsync: true))
                {
                    await pdf.CopyToAsync(output);
                    await output.FlushAsync();
                }
                if (new FileInfo(tempPdf).Length == 0)
                    throw new InvalidOperationException("El PDF generado está vacío.");
                File.Move(tempPdf, destination, overwrite: true);
            }
            finally
            {
                owner.Controls.Remove(browser);
                TryDelete(tempHtml);
                TryDelete(tempPdf);
            }
        }

        private static void TryDelete(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); }
            catch (IOException) { /* Retain original rendering error. */ }
            catch (UnauthorizedAccessException) { /* Retain original rendering error. */ }
        }
    }
}
