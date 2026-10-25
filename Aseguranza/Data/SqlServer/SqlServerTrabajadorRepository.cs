using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerTrabajadorRepository
        : ITrabajadorRepository
    {
        public Trabajador? ConsultarPorNumeroReloj(
            string noReloj)
        {
            if (string.IsNullOrWhiteSpace(noReloj))
            {
                return null;
            }

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarTrabajador",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@NoReloj",
                SqlDbType.VarChar,
                10).Value =
                    noReloj.Trim();

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            if (!TieneColumna(
                lector,
                "NoReloj"))
            {
                return null;
            }

            return new Trabajador
            {
                Id =
                    LeerEntero(
                        lector,
                        "Id"),

                NoReloj =
                    LeerTexto(
                        lector,
                        "NoReloj"),

                Nombre =
                    LeerTexto(
                        lector,
                        "Nombre"),

                RutaFoto =
                    LeerTexto(
                        lector,
                        "RutaFoto"),

                IdLocalidad =
                    0,

                NombreLocalidad =
                    null,

                IdTurno =
                    LeerEntero(
                        lector,
                        "IdTurno"),

                NombreTurno =
                    LeerPrimerTextoDisponible(
                        lector,
                        "NombreTurno",
                        "Turno"),

                IdPlanta =
                    LeerEntero(
                        lector,
                        "IdPlanta"),

                NombrePlanta =
                    LeerPrimerTextoDisponible(
                        lector,
                        "NombrePlanta",
                        "Planta"),

                IdLinea =
                    LeerEntero(
                        lector,
                        "IdLinea"),

                NombreLinea =
                    LeerPrimerTextoDisponible(
                        lector,
                        "NombreLinea",
                        "Linea")
                    ?? "SIN ASIGNAR"
            };
        }

        public DataTable Consultar(
            string textoBuscar)
        {
            DataTable tabla =
                new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarTrabajadores",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@TextoBuscar",
                SqlDbType.VarChar,
                100).Value =
                    textoBuscar ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(
                    comando);

            conexion.Open();

            adaptador.Fill(
                tabla);

            return tabla;
        }

        public DataTable ConsultarParaImportacionHdc()
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando =
                conexion.CreateCommand();

            // Lectura masiva, sin límite y sin modificar datos.
            comando.CommandText = """
                SELECT
                    T.Id,
                    T.NoReloj,
                    T.Nombre,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    T.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea
                FROM dbo.Trabajador AS T
                INNER JOIN dbo.Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN dbo.Planta AS P
                    ON P.Id = T.IdPlanta
                LEFT JOIN dbo.Linea AS Li
                    ON Li.Id = T.IdLinea
                ORDER BY T.NoReloj;
                """;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();

            adaptador.Fill(tabla);

            return tabla;
        }

        public DataTable ConsultarEstadoCertificacion(
            string mostrarPor,
            string textoBuscar)
        {
            DataTable tabla =
                new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarTrabajadoresEstadoCertificacion",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@MostrarPor",
                SqlDbType.VarChar,
                20).Value =
                    mostrarPor ?? string.Empty;

            comando.Parameters.Add(
                "@TextoBuscar",
                SqlDbType.VarChar,
                100).Value =
                    textoBuscar ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(
                    comando);

            conexion.Open();

            adaptador.Fill(
                tabla);

            return tabla;
        }

        public Mensaje Guardar(
            Trabajador trabajador)
        {
            Mensaje? validacion =
                Validar(
                    trabajador);

            if (validacion is not null)
            {
                return validacion;
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spGuardarTrabajador",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value =
                        trabajador.Id;

                comando.Parameters.Add(
                    "@NoReloj",
                    SqlDbType.VarChar,
                    10).Value =
                        trabajador.NoReloj!
                            .Trim();

                comando.Parameters.Add(
                    "@Nombre",
                    SqlDbType.VarChar,
                    200).Value =
                        trabajador.Nombre!
                            .Trim()
                            .ToUpperInvariant();

                comando.Parameters.Add(
                    "@RutaFoto",
                    SqlDbType.VarChar,
                    300).Value =
                        string.IsNullOrWhiteSpace(
                            trabajador.RutaFoto)
                            ? DBNull.Value
                            : trabajador.RutaFoto.Trim();

                comando.Parameters.Add(
                    "@IdTurno",
                    SqlDbType.Int).Value =
                        trabajador.IdTurno;

                comando.Parameters.Add(
                    "@IdPlanta",
                    SqlDbType.Int).Value =
                        trabajador.IdPlanta;

                comando.Parameters.Add(
                    "@IdLinea",
                    SqlDbType.Int).Value =
                        trabajador.IdLinea > 0
                            ? trabajador.IdLinea
                            : DBNull.Value;

                conexion.Open();

                using SqlDataReader lector =
                    comando.ExecuteReader();

                return LeerMensaje(
                    lector);
            }
            catch (Exception ex)
            {
                return CrearMensajeError(
                    ex);
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
                        "El trabajador seleccionado no es válido."
                };
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spBorrarTrabajador",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value =
                        id;

                conexion.Open();

                using SqlDataReader lector =
                    comando.ExecuteReader();

                return LeerMensaje(
                    lector);
            }
            catch (Exception ex)
            {
                return CrearMensajeError(
                    ex);
            }
        }

        private static Mensaje? Validar(
            Trabajador trabajador)
        {
            if (string.IsNullOrWhiteSpace(
                trabajador.NoReloj))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El número de reloj es obligatorio."
                };
            }

            if (trabajador.NoReloj.Trim().Length > 10)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El número de reloj no puede exceder 10 caracteres."
                };
            }

            if (string.IsNullOrWhiteSpace(
                trabajador.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre del trabajador es obligatorio."
                };
            }

            if (trabajador.IdTurno <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un turno válido."
                };
            }

            if (trabajador.IdPlanta <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar una planta válida."
                };
            }

            return null;
        }

        private static Mensaje LeerMensaje(
            SqlDataReader lector)
        {
            if (!lector.Read())
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La operación no devolvió un resultado."
                };
            }

            return new Mensaje
            {
                Id =
                    LeerEntero(
                        lector,
                        "Id"),

                Nombre =
                    LeerTexto(
                        lector,
                        "Nombre")
                    ?? string.Empty
            };
        }

        private static int LeerEntero(
            SqlDataReader lector,
            string columna)
        {
            if (!TieneColumna(
                    lector,
                    columna) ||
                lector[columna] is DBNull)
            {
                return 0;
            }

            return Convert.ToInt32(
                lector[columna]);
        }

        private static string? LeerTexto(
            SqlDataReader lector,
            string columna)
        {
            if (!TieneColumna(
                    lector,
                    columna) ||
                lector[columna] is DBNull)
            {
                return null;
            }

            return Convert.ToString(
                lector[columna]);
        }

        private static string?
            LeerPrimerTextoDisponible(
                SqlDataReader lector,
                params string[] columnas)
        {
            foreach (string columna in columnas)
            {
                if (TieneColumna(
                    lector,
                    columna))
                {
                    return LeerTexto(
                        lector,
                        columna);
                }
            }

            return null;
        }

        private static bool TieneColumna(
            SqlDataReader lector,
            string nombreColumna)
        {
            for (int indice = 0;
                 indice < lector.FieldCount;
                 indice++)
            {
                if (lector.GetName(indice).Equals(
                    nombreColumna,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static Mensaje CrearMensajeError(
            Exception excepcion)
        {
            return new Mensaje
            {
                Id = 0,
                Nombre =
                    "Ocurrió un error al acceder a SQL Server. " +
                    excepcion.Message
            };
        }
    }
}
