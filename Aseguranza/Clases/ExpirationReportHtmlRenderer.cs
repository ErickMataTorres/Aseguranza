using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Aseguranza.Clases
{
    public sealed record ExpirationReportLabels(
        string Personal, string Category, string Horizon, string Plant,
        string Line, string Shift, string Process, string Search);

    public static class ExpirationReportHtmlRenderer
    {
        private static string E(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);

        public static string Render(ExpirationReportResult report,
            ExpirationReportLabels labels, DateTime generatedAt)
        {
            ArgumentNullException.ThrowIfNull(report);
            ArgumentNullException.ThrowIfNull(labels);
            StringBuilder html = new(Math.Max(8192, report.Items.Count * 420));
            html.Append("""
                <!doctype html><html lang="es"><head><meta charset="utf-8">
                <title>Reporte de vencimientos de certificaciones</title>
                <style>
                @page { size: letter landscape; margin: 13mm 10mm 15mm; }
                body { font: 8.5pt Arial,sans-serif; color:#233247; margin:0; }
                header { border-bottom:2px solid #173a65; padding-bottom:6mm; margin-bottom:5mm; }
                .logo { max-width:31mm; max-height:14mm; float:right; }
                h1 { font-size:16pt; color:#173a65; margin:0 0 2mm; }
                .meta { color:#526174; font-size:8pt; }
                .filters { display:grid; grid-template-columns:repeat(4,1fr); gap:2mm 5mm; margin:4mm 0; }
                .filters div { overflow-wrap:anywhere; }
                .summary { display:flex; gap:3mm; margin:4mm 0 5mm; page-break-inside:avoid; }
                .metric { border:1px solid #bdcbd9; border-top:2px solid #173a65;
                          padding:2mm; flex:1; }
                .metric strong { display:block; font-size:12pt; color:#173a65; }
                table { width:100%; border-collapse:collapse; table-layout:fixed; }
                thead { display:table-header-group; }
                tr { break-inside:avoid; page-break-inside:avoid; }
                th { background:#173a65; color:white; text-align:left; font-size:7.4pt; padding:2mm 1mm; }
                td { border-bottom:1px solid #d9e0e8; padding:1.5mm 1mm; vertical-align:top;
                     overflow-wrap:anywhere; }
                tr.expired td:first-child { border-left:2px solid #a84343; }
                tr.upcoming td:first-child { border-left:2px solid #b87917; }
                .state-expired { color:#8c3030; font-weight:bold; }
                .state-upcoming { color:#805615; font-weight:bold; }
                .report-note { margin-top:4mm; color:#526174; font-size:7pt; }
                </style></head><body><header>
                """);
            using (Stream? logo = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Aseguranza.Resources.LogoContecComprimida.png"))
            {
                if (logo is not null)
                {
                    using MemoryStream bytes = new();
                    logo.CopyTo(bytes);
                    html.Append("<img class='logo' alt='Contec' src='data:image/png;base64,")
                        .Append(Convert.ToBase64String(bytes.ToArray())).Append("'>");
                }
            }
            html.Append("<h1>Reporte de vencimientos de certificaciones</h1><div class='meta'>Generado: ")
                .Append(E(generatedAt.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture)))
                .Append(" · Fecha de referencia: ")
                .Append(E(report.AsOfDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)))
                .Append("</div></header><section class='filters'>");
            Filter("Personal", labels.Personal); Filter("Mostrar", labels.Category);
            Filter("Horizonte", labels.Horizon); Filter("Planta", labels.Plant);
            Filter("Línea", labels.Line); Filter("Turno", labels.Shift);
            Filter("Proceso", labels.Process); Filter("Búsqueda", labels.Search);
            html.Append("</section><section class='summary'>");
            Metric("Eventos vencidos", report.Summary.ExpiredEvents);
            Metric("Eventos próximos", report.Summary.UpcomingEvents);
            Metric("Trabajadores en reporte", report.Summary.AffectedWorkers);
            Metric("Procesos en reporte", report.Summary.AffectedProcesses);
            html.Append("</section><table><colgroup>")
                .Append("<col style='width:7%'><col style='width:13%'><col style='width:7%'>")
                .Append("<col style='width:9%'><col style='width:7%'><col style='width:13%'>")
                .Append("<col style='width:10%'><col style='width:10%'><col style='width:8%'>")
                .Append("<col style='width:10%'><col style='width:6%'></colgroup>")
                .Append("<thead><tr><th>No. Reloj</th><th>Trabajador</th><th>Planta</th>")
                .Append("<th>Línea / Área</th><th>Turno</th><th>Proceso</th>")
                .Append("<th>Fecha certificación</th><th>Fecha vencimiento</th>")
                .Append("<th>Días</th><th>Estado</th><th>Personal</th></tr></thead><tbody>");
            foreach (ExpirationEventRow row in report.Items)
            {
                bool expired = row.EventState == ExpirationEventState.Vencida;
                html.Append(expired ? "<tr class='expired'>" : "<tr class='upcoming'>");
                Cell(row.WorkerNumber); Cell(row.WorkerName); Cell(row.PlantName);
                Cell(row.LineName ?? "SIN ASIGNAR"); Cell(row.ShiftName);
                Cell(row.ProcessName);
                Cell(row.CertificationDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture));
                Cell(row.ExpirationDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture));
                Cell(Days(row.DaysUntilExpiration));
                html.Append(expired ? "<td class='state-expired'>VENCIDA</td>" :
                    "<td class='state-upcoming'>PRÓXIMA A VENCER</td>");
                Cell(row.Active ? "Activo" : "Inactivo");
                html.Append("</tr>");
            }
            html.Append("</tbody></table><p class='report-note'>Total de eventos en este reporte: ")
                .Append(report.Items.Count.ToString("N0", CultureInfo.CurrentCulture))
                .Append("</p></body></html>");
            return html.ToString();

            void Filter(string name, string value) => html.Append("<div><b>").Append(E(name))
                .Append(":</b> ").Append(E(value)).Append("</div>");
            void Metric(string name, int count) => html.Append("<div class='metric'><strong>")
                .Append(count.ToString("N0", CultureInfo.CurrentCulture)).Append("</strong>")
                .Append(E(name)).Append("</div>");
            void Cell(string? value) => html.Append("<td>").Append(E(value)).Append("</td>");
        }

        private static string Days(int days) => days switch
        {
            < 0 => $"{-days} día{(days == -1 ? "" : "s")} vencida",
            0 => "Vence hoy",
            1 => "1 día",
            _ => $"{days} días"
        };
    }
}
