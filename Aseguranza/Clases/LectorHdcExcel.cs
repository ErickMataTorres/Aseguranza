using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aseguranza.Clases
{
    public static class LectorHdcExcel
    {
        private static readonly string[] EncabezadosRequeridos =
        {
            "Empleado",
            "Nombre",
            "Localidad",
            "Turno",
            "F Servicio",
            "Departamento",
            "LINEA",
            "Puesto",
            "Categoria",
            "POSITION",
            "FUNCTION",
            "Proceso",
            "DPTO"
        };

        public static ResultadoAnalisisHdc Analizar(
            string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException(
                    "Seleccione un archivo HDC válido.");
            }

            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException(
                    "No se encontró el archivo seleccionado.",
                    rutaArchivo);
            }

            using XLWorkbook libro =
                new XLWorkbook(rutaArchivo);

            IXLWorksheet? hoja =
                libro.Worksheets
                    .FirstOrDefault(
                        item =>
                            string.Equals(
                                item.Name,
                                "HDC",
                                StringComparison.OrdinalIgnoreCase));

            if (hoja is null)
            {
                throw new InvalidOperationException(
                    "El archivo no contiene una hoja llamada HDC.");
            }

            Dictionary<string, int> columnas =
                ObtenerColumnas(hoja);

            ValidarEncabezados(columnas);

            ResultadoAnalisisHdc resultado =
                new ResultadoAnalisisHdc
                {
                    NombreHoja =
                        hoja.Name
                };

            int ultimaFila =
                hoja.LastRowUsed()?.RowNumber()
                ?? 1;

            for (int numeroFila = 2;
                 numeroFila <= ultimaFila;
                 numeroFila++)
            {
                RegistroHdc registro =
                    CrearRegistro(
                        hoja,
                        columnas,
                        numeroFila);

                if (FilaEstaVacia(registro))
                {
                    continue;
                }

                ValidarRegistro(registro);

                resultado.Registros.Add(
                    registro);
            }

            MarcarDuplicados(
                resultado.Registros);

            resultado.TotalRegistros =
                resultado.Registros.Count;
resultado.LocalidadesDistintas =
                resultado.Registros
                    .Select(
                        item =>
                            item.LocalidadHdc)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .Count();

            resultado.TurnosDistintos =
                resultado.Registros
                    .Select(
                        item =>
                            item.TurnoHdc)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .Count();

            resultado.LineasHdcDistintas =
                resultado.Registros
                    .Select(
                        item =>
                            item.LineaHdc)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .Count();

            return resultado;
        }

        private static Dictionary<string, int>
            ObtenerColumnas(
                IXLWorksheet hoja)
        {
            Dictionary<string, int> columnas =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            int ultimaColumna =
                hoja.LastColumnUsed()?.ColumnNumber()
                ?? 0;

            for (int columna = 1;
                 columna <= ultimaColumna;
                 columna++)
            {
                string encabezado =
                    hoja.Cell(
                            1,
                            columna)
                        .GetFormattedString()
                        .Trim();

                if (string.IsNullOrWhiteSpace(
                    encabezado))
                {
                    continue;
                }

                if (!columnas.ContainsKey(
                    encabezado))
                {
                    columnas.Add(
                        encabezado,
                        columna);
                }
            }

            return columnas;
        }

        private static void ValidarEncabezados(
            IReadOnlyDictionary<string, int> columnas)
        {
            string[] faltantes =
                EncabezadosRequeridos
                    .Where(
                        encabezado =>
                            !columnas.ContainsKey(
                                encabezado))
                    .ToArray();

            if (faltantes.Length == 0)
            {
                return;
            }

            throw new InvalidOperationException(
                "La hoja HDC no contiene todas las columnas requeridas." +
                Environment.NewLine +
                Environment.NewLine +
                "Faltan: " +
                string.Join(
                    ", ",
                    faltantes));
        }

        private static RegistroHdc CrearRegistro(
            IXLWorksheet hoja,
            IReadOnlyDictionary<string, int> columnas,
            int numeroFila)
        {
            return new RegistroHdc
            {
                NumeroFilaExcel =
                    numeroFila,

                Empleado =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Empleado"),

                Nombre =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Nombre"),

                LocalidadHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Localidad"),

                TurnoHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Turno"),

                FechaServicio =
                    LeerFecha(
                        hoja,
                        columnas,
                        numeroFila,
                        "F Servicio"),

                DepartamentoHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Departamento"),

                LineaHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "LINEA"),

                PuestoHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Puesto"),

                CategoriaHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Categoria"),

                PositionHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "POSITION"),

                FunctionHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "FUNCTION"),

                ProcesoHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "Proceso"),

                DptoHdc =
                    LeerTexto(
                        hoja,
                        columnas,
                        numeroFila,
                        "DPTO")
            };
        }

        private static string LeerTexto(
            IXLWorksheet hoja,
            IReadOnlyDictionary<string, int> columnas,
            int fila,
            string encabezado)
        {
            int columna =
                columnas[encabezado];

            return hoja.Cell(
                    fila,
                    columna)
                .GetFormattedString()
                .Trim();
        }

        private static string LeerFecha(
            IXLWorksheet hoja,
            IReadOnlyDictionary<string, int> columnas,
            int fila,
            string encabezado)
        {
            IXLCell celda =
                hoja.Cell(
                    fila,
                    columnas[encabezado]);

            if (celda.DataType ==
                XLDataType.DateTime)
            {
                DateTime fecha =
                    celda.GetDateTime();

                return fecha.ToString(
                    "dd/MM/yyyy");
            }

            return celda
                .GetFormattedString()
                .Trim();
        }

        private static bool FilaEstaVacia(
            RegistroHdc registro)
        {
            return
                string.IsNullOrWhiteSpace(
                    registro.Empleado) &&
                string.IsNullOrWhiteSpace(
                    registro.Nombre) &&
                string.IsNullOrWhiteSpace(
                    registro.LocalidadHdc) &&
                string.IsNullOrWhiteSpace(
                    registro.TurnoHdc) &&
                string.IsNullOrWhiteSpace(
                    registro.LineaHdc);
        }

        private static void ValidarRegistro(
            RegistroHdc registro)
        {
            List<string> problemas =
                new List<string>();

            if (string.IsNullOrWhiteSpace(
                registro.Empleado))
            {
                problemas.Add(
                    "Empleado vacío");
            }

            if (string.IsNullOrWhiteSpace(
                registro.Nombre))
            {
                problemas.Add(
                    "Nombre vacío");
            }

            if (string.IsNullOrWhiteSpace(
                registro.LocalidadHdc))
            {
                problemas.Add(
                    "Localidad HDC vacía");
            }

            if (string.IsNullOrWhiteSpace(
                registro.TurnoHdc))
            {
                problemas.Add(
                    "Turno HDC vacío");
            }

            if (problemas.Count == 0)
            {
                registro.EstadoArchivo =
                    "Listo";

                registro.Estado =
                    "Pendiente";

                registro.Observacion =
                    string.Empty;

                return;
            }

            registro.EstadoArchivo =
                "Revisar";

            registro.Estado =
                "Revisar";

            registro.Observacion =
                string.Join(
                    "; ",
                    problemas);
        }

        private static void MarcarDuplicados(
            IReadOnlyCollection<RegistroHdc> registros)
        {
            HashSet<string> empleadosDuplicados =
                registros
                    .Where(
                        item =>
                            !string.IsNullOrWhiteSpace(
                                item.Empleado))
                    .GroupBy(
                        item =>
                            item.Empleado.Trim(),
                        StringComparer.OrdinalIgnoreCase)
                    .Where(
                        grupo =>
                            grupo.Count() > 1)
                    .Select(
                        grupo =>
                            grupo.Key)
                    .ToHashSet(
                        StringComparer.OrdinalIgnoreCase);

            if (empleadosDuplicados.Count == 0)
            {
                return;
            }

            foreach (RegistroHdc registro in registros)
            {
                if (!empleadosDuplicados.Contains(
                    registro.Empleado))
                {
                    continue;
                }

                registro.EstadoArchivo =
                    "Duplicado";

                registro.Estado =
                    "Duplicado";

                registro.Observacion =
                    string.IsNullOrWhiteSpace(
                        registro.Observacion)
                        ? "Número de empleado duplicado dentro del HDC."
                        : registro.Observacion +
                          "; Número de empleado duplicado dentro del HDC.";
            }
        }
    }
}
