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

                Activo =
                    LeerActivo(lector),

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
            string textoBuscar,
            bool? activo)
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

            comando.Parameters.Add("@Activo", SqlDbType.Bit).Value =
                activo.HasValue ? activo.Value : DBNull.Value;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(
                    comando);

            conexion.Open();

            adaptador.Fill(
                tabla);

            ValidarColumnaActivo(tabla);

            return tabla;
        }

        public PagedTableResult ConsultarPaginado(
            WorkerPageRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            using SqlConnection conexion = Conexion.Conectar();
            using SqlCommand comando = new SqlCommand(
                "spConsultarTrabajadores", conexion)
            {
                CommandType = CommandType.StoredProcedure
            };
            comando.Parameters.Add("@TextoBuscar", SqlDbType.VarChar, 100).Value =
                request.Search?.Trim() ?? string.Empty;
            comando.Parameters.Add("@Activo", SqlDbType.Bit).Value =
                request.Active.HasValue ? request.Active.Value : DBNull.Value;
            SqlParameter numeroPagina = comando.Parameters.Add(
                "@PageNumber", SqlDbType.Int);
            numeroPagina.Direction = ParameterDirection.InputOutput;
            numeroPagina.Value = request.ValidPageNumber;
            comando.Parameters.Add("@PageSize", SqlDbType.Int).Value =
                request.ValidPageSize;
            SqlParameter total = comando.Parameters.Add(
                "@TotalCount", SqlDbType.Int);
            total.Direction = ParameterDirection.Output;

            DataTable tabla = new DataTable();
            using SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            conexion.Open();
            adaptador.Fill(tabla);
            ValidarColumnaActivo(tabla);

            if (total.Value is null or DBNull ||
                numeroPagina.Value is null or DBNull)
            {
                throw new InvalidOperationException(
                    "La lista de trabajadores requiere la migración SQL Server 13.");
            }

            return new PagedTableResult(
                tabla,
                Convert.ToInt32(total.Value),
                Convert.ToInt32(numeroPagina.Value),
                request.ValidPageSize);
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
                    COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea,
                    T.Activo
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

            using (SqlCommand validarEstado = conexion.CreateCommand())
            {
                validarEstado.CommandText = """
                    SELECT CASE WHEN
                        COL_LENGTH(N'dbo.Trabajador', N'Activo') IS NOT NULL
                        AND OBJECT_ID(N'dbo.TrabajadorEstadoHistorial', N'U') IS NOT NULL
                        THEN 1 ELSE 0 END;
                    """;

                if (Convert.ToInt32(validarEstado.ExecuteScalar()) != 1)
                {
                    throw new InvalidOperationException(
                        "La base SQL Server requiere la migración " +
                        "10_TrabajadorEstado.sql antes de analizar HDC.");
                }
            }

            adaptador.Fill(tabla);

            ValidarColumnaActivo(tabla);

            return tabla;
        }

        public DataTable ConsultarEstadoCertificacion(
            string mostrarPor,
            string textoBuscar,
            bool? activo)
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

            comando.Parameters.Add("@Activo", SqlDbType.Bit).Value =
                activo.HasValue ? activo.Value : DBNull.Value;

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

        public bool CambiarEstado(
            int idTrabajador,
            bool activo,
            string origen,
            int? idImportacionHdc = null,
            string? motivo = null)
        {
            using SqlConnection conexion = Conexion.Conectar();
            conexion.Open();
            using SqlTransaction transaccion =
                conexion.BeginTransaction(IsolationLevel.Serializable);
            bool cambiado = CambiarEstadoEnTransaccion(
                conexion, transaccion, idTrabajador, activo,
                origen, idImportacionHdc, motivo);
            transaccion.Commit();
            return cambiado;
        }

        internal static bool CambiarEstadoEnTransaccion(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int idTrabajador,
            bool activo,
            string origen,
            int? idImportacionHdc = null,
            string? motivo = null)
        {
            TrabajadorEstadoHistorial.ValidarCambio(
                idTrabajador, origen, idImportacionHdc, motivo);
            using SqlCommand comando = conexion.CreateCommand();
            comando.Transaction = transaccion;
            comando.CommandText = """
                SELECT Activo FROM dbo.Trabajador WITH (UPDLOCK, HOLDLOCK)
                WHERE Id = @Id;
                """;
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = idTrabajador;

            object? valor = comando.ExecuteScalar();
            if (valor is null || valor == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "No se encontró el trabajador cuyo estado se intenta cambiar.");
            }

            bool anterior = Convert.ToBoolean(valor);
            if (anterior == activo)
            {
                return false;
            }

            comando.CommandText = """
                UPDATE dbo.Trabajador SET Activo = @Activo
                WHERE Id = @Id AND Activo = @Anterior;
                """;
            comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;
            comando.Parameters.Add("@Anterior", SqlDbType.Bit).Value = anterior;
            if (comando.ExecuteNonQuery() != 1)
            {
                throw new InvalidOperationException(
                    "El estado del trabajador cambió durante la operación.");
            }

            comando.CommandText = """
                INSERT INTO dbo.TrabajadorEstadoHistorial
                    (IdTrabajador, ActivoAnterior, ActivoNuevo,
                     FechaCambioUtc, Origen, IdImportacionHdc, Motivo)
                VALUES
                    (@Id, @Anterior, @Activo,
                     @FechaCambioUtc, @Origen, @IdImportacionHdc, @Motivo);
                """;
            comando.Parameters.Add("@FechaCambioUtc", SqlDbType.DateTime2)
                .Value = DateTime.UtcNow;
            comando.Parameters.Add("@Origen", SqlDbType.VarChar, 20)
                .Value = origen;
            comando.Parameters.Add("@IdImportacionHdc", SqlDbType.Int)
                .Value = idImportacionHdc.HasValue
                    ? idImportacionHdc.Value
                    : DBNull.Value;
            comando.Parameters.Add("@Motivo", SqlDbType.NVarChar, 500)
                .Value = string.IsNullOrWhiteSpace(motivo)
                    ? DBNull.Value
                    : motivo.Trim();
            comando.ExecuteNonQuery();
            return true;
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

        private static bool LeerActivo(SqlDataReader lector)
        {
            if (!TieneColumna(lector, "Activo") ||
                lector["Activo"] is DBNull)
            {
                throw new InvalidOperationException(
                    "La consulta de trabajadores no devuelve Activo. " +
                    "Aplique la actualización SQL Server de estado de trabajadores.");
            }

            return Convert.ToBoolean(lector["Activo"]);
        }

        private static void ValidarColumnaActivo(DataTable tabla)
        {
            if (!tabla.Columns.Contains("Activo"))
            {
                throw new InvalidOperationException(
                    "La consulta de trabajadores no devuelve Activo. " +
                    "Aplique la actualización SQL Server de estado de trabajadores.");
            }
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
