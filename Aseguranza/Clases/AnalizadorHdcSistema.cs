using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aseguranza.Clases
{
    public static class AnalizadorHdcSistema
    {
        public static void Clasificar(ResultadoAnalisisHdc resultado)
        {
            DataTable equivalenciasPlanta =
                HdcEquivalencia.ConsultarPlantas();

            DataTable equivalenciasTurno =
                HdcEquivalencia.ConsultarTurnos();

            DataTable equivalenciasLinea =
                HdcEquivalencia.ConsultarLineas();

            // Una sola lectura de Trabajador para los ~10 mil registros HDC.
            DataTable trabajadoresActuales =
                Trabajador.ConsultarTrabajadoresParaImportacionHdc();

            Dictionary<string, DataRow> plantas =
                CrearDiccionario(
                    equivalenciasPlanta,
                    "CodigoLocalidadHdc");

            Dictionary<string, DataRow> turnos =
                CrearDiccionario(
                    equivalenciasTurno,
                    "ValorTurnoHdc");

            Dictionary<string, DataRow> lineas =
                equivalenciasLinea.Rows
                    .Cast<DataRow>()
                    .GroupBy(
                        fila => CrearClaveLinea(
                            Convert.ToString(fila["CodigoLocalidadHdc"]),
                            Convert.ToString(fila["ValorLineaHdc"])),
                        StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo.First(),
                        StringComparer.OrdinalIgnoreCase);

            Dictionary<string, DataRow> trabajadores =
                trabajadoresActuales.Rows
                    .Cast<DataRow>()
                    .Where(fila =>
                        !string.IsNullOrWhiteSpace(
                            Convert.ToString(fila["NoReloj"])))
                    .GroupBy(
                        fila => Normalizar(
                            Convert.ToString(fila["NoReloj"])),
                        StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo.First(),
                        StringComparer.OrdinalIgnoreCase);

            foreach (RegistroHdc registro in resultado.Registros)
            {
                LimpiarResolucion(registro);

                if (registro.EstadoArchivo == "Duplicado")
                {
                    registro.Estado = "Duplicado";
                    continue;
                }

                if (registro.EstadoArchivo == "Revisar")
                {
                    registro.Estado = "Revisar";
                    continue;
                }

                string localidad = Normalizar(registro.LocalidadHdc);
                string turno = Normalizar(registro.TurnoHdc);
                string linea = Normalizar(registro.LineaHdc);

                if (!plantas.TryGetValue(localidad, out DataRow? planta))
                {
                    registro.Estado = "Sin equivalencia de planta";
                    AgregarObservacion(
                        registro,
                        "La localidad HDC no tiene equivalencia de planta.");
                    continue;
                }

                registro.IdPlantaSistema =
                    Convert.ToInt32(planta["IdPlanta"]);

                registro.PlantaSistema =
                    Convert.ToString(planta["NombrePlanta"]) ??
                    string.Empty;

                if (!turnos.TryGetValue(turno, out DataRow? filaTurno))
                {
                    registro.Estado = "Sin equivalencia de turno";
                    AgregarObservacion(
                        registro,
                        "El turno HDC no tiene equivalencia.");
                    continue;
                }

                registro.IdTurnoSistema =
                    Convert.ToInt32(filaTurno["IdTurno"]);

                registro.TurnoSistema =
                    Convert.ToString(filaTurno["NombreTurno"]) ??
                    string.Empty;

                string claveLinea =
                    CrearClaveLinea(localidad, linea);

                if (!lineas.TryGetValue(claveLinea, out DataRow? filaLinea))
                {
                    registro.Estado = "Sin equivalencia de línea";
                    AgregarObservacion(
                        registro,
                        "La línea HDC no tiene equivalencia.");
                    continue;
                }

                registro.AccionLinea =
                    Convert.ToString(filaLinea["Accion"])
                        ?.Trim()
                        .ToUpperInvariant()
                    ?? string.Empty;

                if (registro.AccionLinea == "IGNORAR")
                {
                    registro.Estado = "Ignorar";
                    registro.LineaSistema = "IGNORAR";
                    continue;
                }

                if (registro.AccionLinea == "SIN_ASIGNAR")
                {
                    // Línea resuelta como opcional; NO es un error.
                    registro.IdLineaSistema = 0;
                    registro.LineaSistema = "SIN ASIGNAR";
                }
                else if (registro.AccionLinea == "MAPEAR")
                {
                    if (filaLinea["IdLinea"] == DBNull.Value)
                    {
                        registro.Estado = "Revisar";
                        AgregarObservacion(
                            registro,
                            "La equivalencia de línea no tiene una línea del sistema.");
                        continue;
                    }

                    int idPlantaLinea =
                        filaLinea["IdPlanta"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(filaLinea["IdPlanta"]);

                    if (idPlantaLinea != registro.IdPlantaSistema)
                    {
                        registro.Estado = "Revisar";
                        AgregarObservacion(
                            registro,
                            "La línea equivalente pertenece a otra planta.");
                        continue;
                    }

                    registro.IdLineaSistema =
                        Convert.ToInt32(filaLinea["IdLinea"]);

                    registro.LineaSistema =
                        Convert.ToString(filaLinea["NombreLinea"]) ??
                        string.Empty;
                }
                else
                {
                    registro.Estado = "Revisar";
                    AgregarObservacion(
                        registro,
                        "La acción configurada para la línea no es válida.");
                    continue;
                }

                ClasificarContraTrabajadorActual(
                    registro,
                    trabajadores);
            }

            ActualizarContadores(resultado);
        }

        private static void ClasificarContraTrabajadorActual(
            RegistroHdc registro,
            Dictionary<string, DataRow> trabajadores)
        {
            string noReloj = Normalizar(registro.Empleado);

            if (!trabajadores.TryGetValue(noReloj, out DataRow? actual))
            {
                registro.ExisteTrabajador = false;
                registro.Estado = "Nuevo";
                return;
            }

            registro.ExisteTrabajador = true;
            registro.IdTrabajadorSistema = LeerEntero(actual, "Id");

            registro.NombreActualSistema =
                LeerTexto(actual, "Nombre");

            registro.IdTurnoActualSistema =
                LeerEntero(actual, "IdTurno");

            registro.TurnoActualSistema =
                LeerTexto(actual, "NombreTurno");

            registro.IdPlantaActualSistema =
                LeerEntero(actual, "IdPlanta");

            registro.PlantaActualSistema =
                LeerTexto(actual, "NombrePlanta");

            registro.IdLineaActualSistema =
                LeerEntero(actual, "IdLinea");

            registro.LineaActualSistema =
                LeerTexto(actual, "NombreLinea");

            AplicarReglaLineaParaTrabajadorExistente(
                registro);

            List<string> cambios = new List<string>();

            if (!string.Equals(
                    Normalizar(registro.Nombre),
                    Normalizar(registro.NombreActualSistema),
                    StringComparison.OrdinalIgnoreCase))
            {
                cambios.Add("Nombre");
            }

            if (registro.IdPlantaSistema !=
                registro.IdPlantaActualSistema)
            {
                cambios.Add("Planta");
            }

            if (registro.IdTurnoSistema !=
                registro.IdTurnoActualSistema)
            {
                cambios.Add("Turno");
            }

            if (registro.IdLineaSistema !=
                registro.IdLineaActualSistema)
            {
                cambios.Add("Línea");
            }

            registro.CambiosDetectados =
                string.Join(", ", cambios);

            registro.Estado =
                cambios.Count > 0
                    ? "Actualizar"
                    : "Sin cambios";
        }

        private static void AplicarReglaLineaParaTrabajadorExistente(
            RegistroHdc registro)
        {
            if (!string.Equals(
                    registro.AccionLinea,
                    "SIN_ASIGNAR",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // SIN_ASIGNAR significa que el HDC no administrará una línea
            // concreta para un trabajador ya existente.
            //
            // Si la planta no cambia, conservamos la línea actual.
            if (registro.IdPlantaSistema ==
                registro.IdPlantaActualSistema)
            {
                registro.IdLineaSistema =
                    registro.IdLineaActualSistema;

                registro.LineaSistema =
                    registro.IdLineaActualSistema > 0 &&
                    !string.IsNullOrWhiteSpace(
                        registro.LineaActualSistema)
                        ? registro.LineaActualSistema
                        : "SIN ASIGNAR";

                return;
            }

            // Si la planta sí cambia no podemos conservar una línea de la
            // planta anterior, porque dejaríamos una combinación
            // Planta/Línea inconsistente. En ese caso la línea queda NULL.
            registro.IdLineaSistema = 0;
            registro.LineaSistema = "SIN ASIGNAR";
        }

        private static Dictionary<string, DataRow> CrearDiccionario(
            DataTable tabla,
            string columnaClave)
        {
            return tabla.Rows
                .Cast<DataRow>()
                .GroupBy(
                    fila => Normalizar(
                        Convert.ToString(fila[columnaClave])),
                    StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.First(),
                    StringComparer.OrdinalIgnoreCase);
        }

        private static string CrearClaveLinea(
            string? localidad,
            string? linea)
        {
            return Normalizar(localidad) + "|" + Normalizar(linea);
        }

        private static string Normalizar(string? valor)
        {
            return (valor ?? string.Empty)
                .Trim()
                .ToUpperInvariant();
        }

        private static int LeerEntero(
            DataRow fila,
            string columna)
        {
            if (!fila.Table.Columns.Contains(columna) ||
                fila[columna] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(fila[columna]);
        }

        private static string LeerTexto(
            DataRow fila,
            string columna)
        {
            if (!fila.Table.Columns.Contains(columna) ||
                fila[columna] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(fila[columna]) ??
                string.Empty;
        }

        private static void LimpiarResolucion(RegistroHdc registro)
        {
            registro.Estado = "Pendiente";
            registro.ExisteTrabajador = false;
            registro.IdTrabajadorSistema = 0;

            registro.IdPlantaSistema = 0;
            registro.PlantaSistema = string.Empty;

            registro.IdTurnoSistema = 0;
            registro.TurnoSistema = string.Empty;

            registro.AccionLinea = string.Empty;
            registro.IdLineaSistema = 0;
            registro.LineaSistema = string.Empty;

            registro.NombreActualSistema = string.Empty;
            registro.IdPlantaActualSistema = 0;
            registro.PlantaActualSistema = string.Empty;
            registro.IdTurnoActualSistema = 0;
            registro.TurnoActualSistema = string.Empty;
            registro.IdLineaActualSistema = 0;
            registro.LineaActualSistema = string.Empty;
            registro.CambiosDetectados = string.Empty;

            if (registro.EstadoArchivo == "Listo")
            {
                registro.Observacion = string.Empty;
            }
        }

        private static void AgregarObservacion(
            RegistroHdc registro,
            string mensaje)
        {
            if (string.IsNullOrWhiteSpace(registro.Observacion))
            {
                registro.Observacion = mensaje;
                return;
            }

            registro.Observacion += "; " + mensaje;
        }

        private static void ActualizarContadores(
            ResultadoAnalisisHdc resultado)
        {
            resultado.TotalRegistros = resultado.Registros.Count;
            resultado.Nuevos = Contar(resultado, "Nuevo");
            resultado.Actualizados = Contar(resultado, "Actualizar");
            resultado.SinCambios = Contar(resultado, "Sin cambios");

            resultado.Existentes =
                resultado.Actualizados +
                resultado.SinCambios;

            resultado.SinEquivalenciaPlanta =
                Contar(resultado, "Sin equivalencia de planta");

            resultado.SinEquivalenciaTurno =
                Contar(resultado, "Sin equivalencia de turno");

            resultado.SinEquivalenciaLinea =
                Contar(resultado, "Sin equivalencia de línea");

            resultado.SinAsignar =
                resultado.Registros.Count(item =>
                    string.Equals(
                        item.AccionLinea,
                        "SIN_ASIGNAR",
                        StringComparison.OrdinalIgnoreCase));

            resultado.Ignorados = Contar(resultado, "Ignorar");
            resultado.RegistrosRevisar = Contar(resultado, "Revisar");
            resultado.RegistrosDuplicados = Contar(resultado, "Duplicado");
        }

        private static int Contar(
            ResultadoAnalisisHdc resultado,
            string estado)
        {
            return resultado.Registros.Count(item =>
                string.Equals(
                    item.Estado,
                    estado,
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}
