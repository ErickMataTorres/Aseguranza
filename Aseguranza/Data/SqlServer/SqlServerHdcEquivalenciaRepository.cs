using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerHdcEquivalenciaRepository
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
                FROM dbo.EquivalenciaPlantaHdc AS E
                INNER JOIN dbo.Planta AS P
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
                FROM dbo.EquivalenciaTurnoHdc AS E
                INNER JOIN dbo.Turno AS T
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
                FROM dbo.EquivalenciaLineaHdc AS E
                LEFT JOIN dbo.Linea AS L
                    ON L.Id = E.IdLinea
                LEFT JOIN dbo.Planta AS P
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
                using SqlConnection conexion =
                    Conexion.Conectar();

                conexion.Open();

                using SqlCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.Planta
                        WHERE Id = @IdPlanta
                    )
                    BEGIN
                        THROW 50001, 'La planta seleccionada no existe.', 1;
                    END;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM dbo.EquivalenciaPlantaHdc
                        WHERE CodigoLocalidadHdc = @CodigoLocalidadHdc
                    )
                    BEGIN
                        UPDATE dbo.EquivalenciaPlantaHdc
                        SET
                            IdPlanta = @IdPlanta,
                            Activo = 1
                        WHERE CodigoLocalidadHdc = @CodigoLocalidadHdc;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO dbo.EquivalenciaPlantaHdc
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
                        );
                    END;
                    """;

                comando.Parameters.Add(
                    "@CodigoLocalidadHdc",
                    SqlDbType.VarChar,
                    50).Value =
                        codigoLocalidadHdc.ToUpperInvariant();

                comando.Parameters.Add(
                    "@IdPlanta",
                    SqlDbType.Int).Value =
                        idPlanta;

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
                using SqlConnection conexion =
                    Conexion.Conectar();

                conexion.Open();

                using SqlCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.Turno
                        WHERE Id = @IdTurno
                    )
                    BEGIN
                        THROW 50002, 'El turno seleccionado no existe.', 1;
                    END;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM dbo.EquivalenciaTurnoHdc
                        WHERE ValorTurnoHdc = @ValorTurnoHdc
                    )
                    BEGIN
                        UPDATE dbo.EquivalenciaTurnoHdc
                        SET
                            IdTurno = @IdTurno,
                            Activo = 1
                        WHERE ValorTurnoHdc = @ValorTurnoHdc;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO dbo.EquivalenciaTurnoHdc
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
                        );
                    END;
                    """;

                comando.Parameters.Add(
                    "@ValorTurnoHdc",
                    SqlDbType.VarChar,
                    50).Value =
                        valorTurnoHdc.ToUpperInvariant();

                comando.Parameters.Add(
                    "@IdTurno",
                    SqlDbType.Int).Value =
                        idTurno;

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
                using SqlConnection conexion =
                    Conexion.Conectar();

                conexion.Open();

                using SqlCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DECLARE @IdPlantaEquivalente INT;

                    SELECT
                        @IdPlantaEquivalente = IdPlanta
                    FROM dbo.EquivalenciaPlantaHdc
                    WHERE CodigoLocalidadHdc = @CodigoLocalidadHdc
                      AND Activo = 1;

                    IF @IdPlantaEquivalente IS NULL
                    BEGIN
                        THROW 50003, 'Primero configure la equivalencia de planta.', 1;
                    END;

                    IF @Accion = 'MAPEAR'
                    BEGIN
                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.Linea
                            WHERE Id = @IdLinea
                              AND IdPlanta = @IdPlantaEquivalente
                        )
                        BEGIN
                            THROW 50004, 'La línea seleccionada no pertenece a la planta equivalente.', 1;
                        END;
                    END;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM dbo.EquivalenciaLineaHdc
                        WHERE CodigoLocalidadHdc = @CodigoLocalidadHdc
                          AND ValorLineaHdc = @ValorLineaHdc
                    )
                    BEGIN
                        UPDATE dbo.EquivalenciaLineaHdc
                        SET
                            Accion = @Accion,
                            IdLinea =
                                CASE
                                    WHEN @Accion = 'MAPEAR'
                                        THEN @IdLinea
                                    ELSE NULL
                                END,
                            Activo = 1
                        WHERE CodigoLocalidadHdc = @CodigoLocalidadHdc
                          AND ValorLineaHdc = @ValorLineaHdc;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO dbo.EquivalenciaLineaHdc
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
                            CASE
                                WHEN @Accion = 'MAPEAR'
                                    THEN @IdLinea
                                ELSE NULL
                            END,
                            1
                        );
                    END;
                    """;

                comando.Parameters.Add(
                    "@CodigoLocalidadHdc",
                    SqlDbType.VarChar,
                    50).Value =
                        codigoLocalidadHdc.ToUpperInvariant();

                comando.Parameters.Add(
                    "@ValorLineaHdc",
                    SqlDbType.VarChar,
                    100).Value =
                        valorLineaHdc.ToUpperInvariant();

                comando.Parameters.Add(
                    "@Accion",
                    SqlDbType.VarChar,
                    20).Value =
                        accion;

                comando.Parameters.Add(
                    "@IdLinea",
                    SqlDbType.Int).Value =
                        idLinea.HasValue
                            ? idLinea.Value
                            : DBNull.Value;

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

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT NoReloj
                FROM dbo.Trabajador;
                """;

            conexion.Open();

            using SqlDataReader lector =
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

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando =
                conexion.CreateCommand();

            comando.CommandText =
                sql;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(
                    comando);

            conexion.Open();

            adaptador.Fill(
                tabla);

            return tabla;
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
