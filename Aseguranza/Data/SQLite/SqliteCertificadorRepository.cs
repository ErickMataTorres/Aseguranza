using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteCertificadorRepository
        : ICertificadorRepository
    {
        public DataTable Consultar(string textoBuscar)
        {
            DataTable tabla = new DataTable();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    C.Id,
                    C.IdTrabajador,
                    T.NoReloj,
                    T.Nombre AS NombreTrabajador,
                    T.RutaFoto,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    T.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    COALESCE(
                        Li.Nombre,
                        'SIN ASIGNAR'
                    ) AS NombreLinea
                FROM Certificador AS C
                INNER JOIN Trabajador AS T
                    ON T.Id = C.IdTrabajador
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Planta AS P
                    ON P.Id = T.IdPlanta
                LEFT JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                WHERE T.NoReloj
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR T.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR Tu.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR P.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR COALESCE(
                          Li.Nombre,
                          'SIN ASIGNAR'
                      ) LIKE @TextoBuscar COLLATE NOCASE
                ORDER BY T.Nombre;
                """;

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");

            using SqliteDataReader lector =
                comando.ExecuteReader();

            /*
             * No usamos DataTable.Load(lector).
             *
             * Con Microsoft.Data.Sqlite, Load puede inferir metadatos
             * de columna que después provocan que WinForms trate
             * NombreLinea como un tipo distinto de string.
             *
             * Definimos el esquema explícitamente y cargamos
             * las filas manualmente.
             */
            PrepararTablaConsulta(
                tabla);

            while (lector.Read())
            {
                DataRow fila =
                    tabla.NewRow();

                fila["Id"] =
                    Convert.ToInt32(
                        lector.GetInt64(0));

                fila["IdTrabajador"] =
                    Convert.ToInt32(
                        lector.GetInt64(1));

                fila["NoReloj"] =
                    lector.IsDBNull(2)
                        ? string.Empty
                        : lector.GetString(2);

                fila["NombreTrabajador"] =
                    lector.IsDBNull(3)
                        ? string.Empty
                        : lector.GetString(3);

                fila["RutaFoto"] =
                    lector.IsDBNull(4)
                        ? DBNull.Value
                        : lector.GetString(4);

                fila["IdTurno"] =
                    Convert.ToInt32(
                        lector.GetInt64(5));

                fila["NombreTurno"] =
                    lector.IsDBNull(6)
                        ? string.Empty
                        : lector.GetString(6);

                fila["IdPlanta"] =
                    Convert.ToInt32(
                        lector.GetInt64(7));

                fila["NombrePlanta"] =
                    lector.IsDBNull(8)
                        ? string.Empty
                        : lector.GetString(8);

                fila["IdLinea"] =
                    lector.IsDBNull(9)
                        ? DBNull.Value
                        : Convert.ToInt32(
                            lector.GetInt64(9));

                fila["NombreLinea"] =
                    lector.IsDBNull(10)
                        ? "SIN ASIGNAR"
                        : lector.GetString(10);

                tabla.Rows.Add(
                    fila);
            }

            return tabla;
        }

        private static void PrepararTablaConsulta(
            DataTable tabla)
        {
            tabla.Columns.Add(
                "Id",
                typeof(int));

            tabla.Columns.Add(
                "IdTrabajador",
                typeof(int));

            tabla.Columns.Add(
                "NoReloj",
                typeof(string));

            tabla.Columns.Add(
                "NombreTrabajador",
                typeof(string));

            DataColumn columnaRutaFoto =
                tabla.Columns.Add(
                    "RutaFoto",
                    typeof(string));

            columnaRutaFoto.AllowDBNull =
                true;

            tabla.Columns.Add(
                "IdTurno",
                typeof(int));

            tabla.Columns.Add(
                "NombreTurno",
                typeof(string));

            tabla.Columns.Add(
                "IdPlanta",
                typeof(int));

            tabla.Columns.Add(
                "NombrePlanta",
                typeof(string));

            DataColumn columnaIdLinea =
                tabla.Columns.Add(
                    "IdLinea",
                    typeof(int));

            columnaIdLinea.AllowDBNull =
                true;

            tabla.Columns.Add(
                "NombreLinea",
                typeof(string));
        }

        public Mensaje Guardar(
            Certificador certificador)
        {
            if (string.IsNullOrWhiteSpace(
                certificador.NoReloj))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El número de reloj es obligatorio."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                int idTrabajador =
                    ConsultarIdTrabajador(
                        conexion,
                        transaccion,
                        certificador.NoReloj.Trim());

                if (idTrabajador <= 0)
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El trabajador no existe."
                    };
                }

                if (YaEsCertificador(
                    conexion,
                    transaccion,
                    idTrabajador))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "El trabajador ya es certificador."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction = transaccion;

                comando.CommandText = """
                    INSERT INTO Certificador
                    (
                        IdTrabajador
                    )
                    VALUES
                    (
                        @IdTrabajador
                    );
                    """;

                comando.Parameters.AddWithValue(
                    "@IdTrabajador",
                    idTrabajador);

                comando.ExecuteNonQuery();

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha guardado correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        public Mensaje Borrar(int id)
        {
            if (id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El certificador seleccionado no es válido."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (!ExisteCertificador(conexion, id))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El certificador seleccionado no existe."
                    };
                }

                if (TieneCertificacionesRelacionadas(
                    conexion,
                    id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay " +
                            "certificaciones asociadas a este certificador."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Certificador
                    WHERE Id = @Id;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    id);

                comando.ExecuteNonQuery();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha borrado correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static int ConsultarIdTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string noReloj)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT Id
                FROM Trabajador
                WHERE NoReloj = @NoReloj COLLATE NOCASE
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                noReloj);

            object? resultado =
                comando.ExecuteScalar();

            if (resultado is null ||
                resultado is DBNull)
            {
                return 0;
            }

            return Convert.ToInt32(resultado);
        }

        private static bool YaEsCertificador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificador
                WHERE IdTrabajador = @IdTrabajador;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteCertificador(
            SqliteConnection conexion,
            int id)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificador
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool TieneCertificacionesRelacionadas(
            SqliteConnection conexion,
            int idCertificador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificacion
                WHERE IdCertificador = @IdCertificador;
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificador",
                idCertificador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static Mensaje CrearMensajeError(
            Exception excepcion)
        {
            return new Mensaje
            {
                Id = 0,
                Nombre =
                    "Ocurrió un error al acceder a SQLite. " +
                    excepcion.Message
            };
        }
    }
}