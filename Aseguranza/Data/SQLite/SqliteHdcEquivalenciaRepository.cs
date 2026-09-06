using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteHdcEquivalenciaRepository
        : IHdcEquivalenciaRepository
    {
        public DataTable ConsultarPlantas()
        {
            return ConsultarTabla(
                """
                SELECT
                    E.Id,
                    E.CodigoLocalidadHdc,
                    E.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    E.Activo,
                    E.Comentario
                FROM EquivalenciaPlantaHdc AS E
                INNER JOIN Planta AS P
                    ON P.Id = E.IdPlanta
                WHERE E.Activo = 1
                ORDER BY E.CodigoLocalidadHdc;
                """);
        }

        public DataTable ConsultarTurnos()
        {
            return ConsultarTabla(
                """
                SELECT
                    E.Id,
                    E.ValorTurnoHdc,
                    E.IdTurno,
                    T.Nombre AS NombreTurno,
                    E.Activo,
                    E.Comentario
                FROM EquivalenciaTurnoHdc AS E
                INNER JOIN Turno AS T
                    ON T.Id = E.IdTurno
                WHERE E.Activo = 1
                ORDER BY E.ValorTurnoHdc;
                """);
        }

        public DataTable ConsultarLineas()
        {
            return ConsultarTabla(
                """
                SELECT
                    E.Id,
                    E.CodigoLocalidadHdc,
                    E.ValorLineaHdc,
                    E.Accion,
                    E.IdLinea,
                    L.Nombre AS NombreLinea,
                    L.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    E.Activo,
                    E.Comentario
                FROM EquivalenciaLineaHdc AS E
                LEFT JOIN Linea AS L
                    ON L.Id = E.IdLinea
                LEFT JOIN Planta AS P
                    ON P.Id = L.IdPlanta
                WHERE E.Activo = 1
                ORDER BY
                    E.CodigoLocalidadHdc,
                    E.ValorLineaHdc;
                """);
        }

        public Mensaje GuardarPlanta(
            string codigoLocalidadHdc,
            int idPlanta)
        {
            codigoLocalidadHdc =
                (codigoLocalidadHdc ?? string.Empty)
                    .Trim();

            if (string.IsNullOrWhiteSpace(
                    codigoLocalidadHdc) ||
                idPlanta <= 0)
            {
                return Error(
                    "Seleccione una localidad HDC y una planta válida.");
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (!ExisteId(
                    conexion,
                    "Planta",
                    idPlanta))
                {
                    return Error(
                        "La planta seleccionada no existe.");
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    INSERT INTO EquivalenciaPlantaHdc
                    (
                        CodigoLocalidadHdc,
                        IdPlanta,
                        Activo
                    )
                    VALUES
                    (
                        @CodigoLocalidadHdc,
                        @IdPlanta,
                        1
                    )
                    ON CONFLICT(CodigoLocalidadHdc)
                    DO UPDATE SET
                        IdPlanta = excluded.IdPlanta,
                        Activo = 1;
                    """;

                comando.Parameters.AddWithValue(
                    "@CodigoLocalidadHdc",
                    codigoLocalidadHdc.ToUpperInvariant());

                comando.Parameters.AddWithValue(
                    "@IdPlanta",
                    idPlanta);

                comando.ExecuteNonQuery();

                return Ok(
                    "Equivalencia de planta guardada correctamente.");
            }
            catch (Exception ex)
            {
                return Error(
                    "No se pudo guardar la equivalencia de planta. " +
                    ex.Message);
            }
        }

        public Mensaje GuardarTurno(
            string valorTurnoHdc,
            int idTurno)
        {
            valorTurnoHdc =
                (valorTurnoHdc ?? string.Empty)
                    .Trim();

            if (string.IsNullOrWhiteSpace(
                    valorTurnoHdc) ||
                idTurno <= 0)
            {
                return Error(
                    "Seleccione un turno HDC y un turno del sistema.");
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (!ExisteId(
                    conexion,
                    "Turno",
                    idTurno))
                {
                    return Error(
                        "El turno seleccionado no existe.");
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    INSERT INTO EquivalenciaTurnoHdc
                    (
                        ValorTurnoHdc,
                        IdTurno,
                        Activo
                    )
                    VALUES
                    (
                        @ValorTurnoHdc,
                        @IdTurno,
                        1
                    )
                    ON CONFLICT(ValorTurnoHdc)
                    DO UPDATE SET
                        IdTurno = excluded.IdTurno,
                        Activo = 1;
                    """;

                comando.Parameters.AddWithValue(
                    "@ValorTurnoHdc",
                    valorTurnoHdc.ToUpperInvariant());

                comando.Parameters.AddWithValue(
                    "@IdTurno",
                    idTurno);

                comando.ExecuteNonQuery();

                return Ok(
                    "Equivalencia de turno guardada correctamente.");
            }
            catch (Exception ex)
            {
                return Error(
                    "No se pudo guardar la equivalencia de turno. " +
                    ex.Message);
            }
        }

        public Mensaje GuardarLinea(
            string codigoLocalidadHdc,
            string valorLineaHdc,
            string accion,
            int? idLinea)
        {
            codigoLocalidadHdc =
                (codigoLocalidadHdc ?? string.Empty)
                    .Trim();

            valorLineaHdc =
                (valorLineaHdc ?? string.Empty)
                    .Trim();

            accion =
                (accion ?? string.Empty)
                    .Trim()
                    .ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(
                    codigoLocalidadHdc) ||
                string.IsNullOrWhiteSpace(
                    valorLineaHdc))
            {
                return Error(
                    "Seleccione localidad HDC y línea HDC.");
            }

            if (accion != "MAPEAR" &&
                accion != "SIN_ASIGNAR" &&
                accion != "IGNORAR")
            {
                return Error(
                    "La acción seleccionada no es válida.");
            }

            if (accion == "MAPEAR" &&
                (!idLinea.HasValue ||
                 idLinea.Value <= 0))
            {
                return Error(
                    "Seleccione la línea del sistema que corresponde.");
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                int idPlanta =
                    ObtenerIdPlantaEquivalente(
                        conexion,
                        codigoLocalidadHdc);

                if (idPlanta <= 0)
                {
                    return Error(
                        "Primero configure la equivalencia de planta.");
                }

                if (accion == "MAPEAR" &&
                    !LineaPertenecePlanta(
                        conexion,
                        idLinea!.Value,
                        idPlanta))
                {
                    return Error(
                        "La línea seleccionada no pertenece a la planta equivalente.");
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    INSERT INTO EquivalenciaLineaHdc
                    (
                        CodigoLocalidadHdc,
                        ValorLineaHdc,
                        Accion,
                        IdLinea,
                        Activo
                    )
                    VALUES
                    (
                        @CodigoLocalidadHdc,
                        @ValorLineaHdc,
                        @Accion,
                        @IdLinea,
                        1
                    )
                    ON CONFLICT(
                        CodigoLocalidadHdc,
                        ValorLineaHdc
                    )
                    DO UPDATE SET
                        Accion = excluded.Accion,
                        IdLinea = excluded.IdLinea,
                        Activo = 1;
                    """;

                comando.Parameters.AddWithValue(
                    "@CodigoLocalidadHdc",
                    codigoLocalidadHdc.ToUpperInvariant());

                comando.Parameters.AddWithValue(
                    "@ValorLineaHdc",
                    valorLineaHdc.ToUpperInvariant());

                comando.Parameters.AddWithValue(
                    "@Accion",
                    accion);

                comando.Parameters.AddWithValue(
                    "@IdLinea",
                    accion == "MAPEAR"
                        ? idLinea!.Value
                        : DBNull.Value);

                comando.ExecuteNonQuery();

                return Ok(
                    "Equivalencia de línea guardada correctamente.");
            }
            catch (Exception ex)
            {
                return Error(
                    "No se pudo guardar la equivalencia de línea. " +
                    ex.Message);
            }
        }

        public HashSet<string>
            ConsultarNumerosReloj()
        {
            HashSet<string> resultado =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT NoReloj
                FROM Trabajador;
                """;

            using SqliteDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                string valor =
                    Convert.ToString(
                        lector["NoReloj"])
                    ?.Trim()
                    ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(
                    valor))
                {
                    resultado.Add(
                        valor);
                }
            }

            return resultado;
        }

        private static DataTable ConsultarTabla(
            string sql)
        {
            DataTable tabla =
                new DataTable();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText =
                sql;

            using SqliteDataReader lector =
                comando.ExecuteReader();

            // SQLite puede inferir restricciones incorrectas al usar
            // DataTable.Load(), especialmente cuando una equivalencia
            // SIN_ASIGNAR/IGNORAR deja IdLinea, NombreLinea, IdPlanta
            // y NombrePlanta en NULL. Por eso construimos el esquema
            // explícitamente y cargamos las filas de forma manual.
            for (int i = 0; i < lector.FieldCount; i++)
            {
                string nombreColumna =
                    lector.GetName(i);

                Type tipoColumna =
                    nombreColumna switch
                    {
                        "Id" => typeof(int),
                        "IdPlanta" => typeof(int),
                        "IdTurno" => typeof(int),
                        "IdLinea" => typeof(int),
                        "Activo" => typeof(int),
                        _ => typeof(string)
                    };

                DataColumn columna =
                    tabla.Columns.Add(
                        nombreColumna,
                        tipoColumna);

                columna.AllowDBNull =
                    true;
            }

            while (lector.Read())
            {
                DataRow fila =
                    tabla.NewRow();

                for (int i = 0; i < lector.FieldCount; i++)
                {
                    if (lector.IsDBNull(i))
                    {
                        fila[i] =
                            DBNull.Value;

                        continue;
                    }

                    string nombreColumna =
                        lector.GetName(i);

                    fila[i] =
                        nombreColumna switch
                        {
                            "Id" or
                            "IdPlanta" or
                            "IdTurno" or
                            "IdLinea" or
                            "Activo" =>
                                Convert.ToInt32(
                                    lector.GetValue(i)),

                            _ =>
                                Convert.ToString(
                                    lector.GetValue(i))
                                ?? string.Empty
                        };
                }

                tabla.Rows.Add(
                    fila);
            }

            return tabla;
        }

        private static bool ExisteId(
            SqliteConnection conexion,
            string tabla,
            int id)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText =
                $"SELECT COUNT(*) FROM {tabla} WHERE Id = @Id;";

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static int ObtenerIdPlantaEquivalente(
            SqliteConnection conexion,
            string codigoLocalidadHdc)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT IdPlanta
                FROM EquivalenciaPlantaHdc
                WHERE CodigoLocalidadHdc = @Codigo
                  AND Activo = 1
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@Codigo",
                codigoLocalidadHdc.ToUpperInvariant());

            object? resultado =
                comando.ExecuteScalar();

            if (resultado is null ||
                resultado == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(
                resultado);
        }

        private static bool LineaPertenecePlanta(
            SqliteConnection conexion,
            int idLinea,
            int idPlanta)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Linea
                WHERE Id = @IdLinea
                  AND IdPlanta = @IdPlanta;
                """;

            comando.Parameters.AddWithValue(
                "@IdLinea",
                idLinea);

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                idPlanta);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static Mensaje Ok(
            string mensaje)
        {
            return new Mensaje
            {
                Id = 1,
                Nombre = mensaje
            };
        }

        private static Mensaje Error(
            string mensaje)
        {
            return new Mensaje
            {
                Id = 0,
                Nombre = mensaje
            };
        }
    }
}
