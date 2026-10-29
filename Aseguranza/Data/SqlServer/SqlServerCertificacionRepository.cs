using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerCertificacionRepository
        : ICertificacionRepository
    {
        public ExpirationDashboardResult ConsultarVencimientos(
            ExpirationDashboardRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            request.Validate();
            using SqlConnection conexion = Conexion.Conectar();
            conexion.Open();
            using (SqlCommand validar = conexion.CreateCommand())
            {
                validar.CommandText = """
                    IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
                        SELECT 0;
                    ELSE
                        SELECT CASE WHEN EXISTS
                            (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 15)
                            THEN 1 ELSE 0 END;
                    """;
                if (Convert.ToInt32(validar.ExecuteScalar()) != 1)
                    throw new InvalidOperationException(
                        "La base SQL Server requiere la migración " +
                        "15_VencimientosCertificaciones.sql antes de consultar vencimientos.");
            }

            using SqlCommand comando = new SqlCommand(
                "spConsultarVencimientosCertificaciones", conexion)
            {
                CommandType = CommandType.StoredProcedure
            };
            comando.Parameters.Add("@Activo", SqlDbType.Bit).Value =
                request.Active.HasValue ? request.Active.Value : DBNull.Value;
            comando.Parameters.Add("@Categoria", SqlDbType.Int).Value =
                (int)request.Category;
            comando.Parameters.Add("@HorizonteDias", SqlDbType.Int).Value =
                request.HorizonDays;
            comando.Parameters.Add("@IdPlanta", SqlDbType.Int).Value =
                request.PlantId.HasValue ? request.PlantId.Value : DBNull.Value;
            comando.Parameters.Add("@IdLinea", SqlDbType.Int).Value =
                request.LineId.HasValue ? request.LineId.Value : DBNull.Value;
            comando.Parameters.Add("@IdTurno", SqlDbType.Int).Value =
                request.ShiftId.HasValue ? request.ShiftId.Value : DBNull.Value;
            comando.Parameters.Add("@IdProceso", SqlDbType.Int).Value =
                request.ProcessId.HasValue ? request.ProcessId.Value : DBNull.Value;
            comando.Parameters.Add("@TextoBuscar", SqlDbType.VarChar, 100).Value =
                (request.Search ?? string.Empty).Trim();
            comando.Parameters.Add("@FechaReferencia", SqlDbType.Date).Value =
                request.AsOfDate.HasValue
                    ? request.AsOfDate.Value.Date : DBNull.Value;
            SqlParameter pageNumber = comando.Parameters.Add(
                "@PageNumber", SqlDbType.Int);
            pageNumber.Direction = ParameterDirection.InputOutput;
            pageNumber.Value = request.ValidPageNumber;
            comando.Parameters.Add("@PageSize", SqlDbType.Int).Value =
                request.ValidPageSize;
            SqlParameter totalCount = comando.Parameters.Add(
                "@TotalCount", SqlDbType.Int);
            totalCount.Direction = ParameterDirection.Output;

            DataSet data = new DataSet();
            using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                adaptador.Fill(data);
            if (data.Tables.Count != 2 || data.Tables[0].Rows.Count != 1)
                throw new InvalidOperationException(
                    "La consulta de vencimientos devolvió un resultado inesperado.");

            DataRow resumen = data.Tables[0].Rows[0];
            ExpirationDashboardSummary summary = new ExpirationDashboardSummary(
                Convert.ToInt32(resumen["ExpiredEvents"]),
                Convert.ToInt32(resumen["UpcomingEvents"]),
                Convert.ToInt32(resumen["AffectedWorkers"]),
                Convert.ToInt32(resumen["AffectedProcesses"]));
            List<ExpirationEventRow> items = new();
            foreach (DataRow fila in data.Tables[1].Rows)
            {
                items.Add(LeerFilaVencimiento(fila));
            }
            return new ExpirationDashboardResult(items,
                Convert.ToInt32(totalCount.Value),
                Convert.ToInt32(pageNumber.Value),
                request.ValidPageSize, summary);
        }

        public ExpirationReportResult ObtenerVencimientosParaReporte(
            ExpirationReportRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            request.Validate();
            using SqlConnection conexion = Conexion.Conectar();
            conexion.Open();
            using (SqlCommand validar = conexion.CreateCommand())
            {
                validar.CommandText = """
                    IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
                        SELECT 0;
                    ELSE
                        SELECT CASE WHEN EXISTS
                            (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 16)
                            THEN 1 ELSE 0 END;
                    """;
                if (Convert.ToInt32(validar.ExecuteScalar()) != 1)
                    throw new InvalidOperationException(
                        "La base SQL Server requiere la migración " +
                        "16_ReporteVencimientosCertificaciones.sql antes de exportar vencimientos.");
            }
            using SqlCommand comando = new(
                "spExportarVencimientosCertificaciones", conexion)
            {
                CommandType = CommandType.StoredProcedure
            };
            comando.Parameters.Add("@Activo", SqlDbType.Bit).Value =
                request.Active.HasValue ? request.Active.Value : DBNull.Value;
            comando.Parameters.Add("@Categoria", SqlDbType.Int).Value = (int)request.Category;
            comando.Parameters.Add("@HorizonteDias", SqlDbType.Int).Value = request.HorizonDays;
            comando.Parameters.Add("@IdPlanta", SqlDbType.Int).Value =
                request.PlantId.HasValue ? request.PlantId.Value : DBNull.Value;
            comando.Parameters.Add("@IdLinea", SqlDbType.Int).Value =
                request.LineId.HasValue ? request.LineId.Value : DBNull.Value;
            comando.Parameters.Add("@IdTurno", SqlDbType.Int).Value =
                request.ShiftId.HasValue ? request.ShiftId.Value : DBNull.Value;
            comando.Parameters.Add("@IdProceso", SqlDbType.Int).Value =
                request.ProcessId.HasValue ? request.ProcessId.Value : DBNull.Value;
            comando.Parameters.Add("@TextoBuscar", SqlDbType.VarChar, 100).Value =
                (request.Search ?? string.Empty).Trim();
            comando.Parameters.Add("@FechaReferencia", SqlDbType.Date).Value =
                request.AsOfDate.Date;

            DataSet data = new();
            using (SqlDataAdapter adaptador = new(comando)) adaptador.Fill(data);
            if (data.Tables.Count != 2 || data.Tables[0].Rows.Count != 1)
                throw new InvalidOperationException(
                    "La exportación de vencimientos devolvió un resultado inesperado.");
            DataRow resumen = data.Tables[0].Rows[0];
            ExpirationDashboardSummary summary = new(
                Convert.ToInt32(resumen["ExpiredEvents"]),
                Convert.ToInt32(resumen["UpcomingEvents"]),
                Convert.ToInt32(resumen["AffectedWorkers"]),
                Convert.ToInt32(resumen["AffectedProcesses"]));
            List<ExpirationEventRow> items = new();
            foreach (DataRow fila in data.Tables[1].Rows)
                items.Add(LeerFilaVencimiento(fila));
            return new ExpirationReportResult(items, summary, request.AsOfDate.Date);
        }

        private static ExpirationEventRow LeerFilaVencimiento(DataRow fila) => new(
                    Convert.ToInt32(fila["CertificationId"]),
                    Convert.ToInt32(fila["WorkerId"]),
                    Convert.ToString(fila["WorkerNumber"]) ?? string.Empty,
                    Convert.ToString(fila["WorkerName"]) ?? string.Empty,
                    Convert.ToInt32(fila["PlantId"]),
                    Convert.ToString(fila["PlantName"]) ?? string.Empty,
                    fila["LineId"] == DBNull.Value ? null
                        : Convert.ToInt32(fila["LineId"]),
                    fila["LineName"] == DBNull.Value ? null
                        : Convert.ToString(fila["LineName"]),
                    Convert.ToInt32(fila["ShiftId"]),
                    Convert.ToString(fila["ShiftName"]) ?? string.Empty,
                    Convert.ToInt32(fila["ProcessId"]),
                    Convert.ToString(fila["ProcessName"]) ?? string.Empty,
                    Convert.ToDateTime(fila["CertificationDate"]),
                    Convert.ToDateTime(fila["ExpirationDate"]),
                    Convert.ToInt32(fila["DaysUntilExpiration"]),
                    (ExpirationEventState)Convert.ToInt32(fila["EventState"]),
                    Convert.ToBoolean(fila["Active"]));

        public DataTable ConsultarVerificacionNoReloj(
            string noReloj)
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarVerificacionNoReloj",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@NoReloj",
                SqlDbType.VarChar,
                50).Value =
                    noReloj?.Trim() ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();
            adaptador.Fill(tabla);

            return tabla;
        }

        public DataTable ConsultarPorTrabajador(
            int idTrabajador,
            string textoBuscar)
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarCertificacionesPorTrabajador",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@IdTrabajador",
                SqlDbType.Int).Value =
                    idTrabajador;

            comando.Parameters.Add(
                "@TextoBuscar",
                SqlDbType.VarChar,
                100).Value =
                    textoBuscar ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();
            adaptador.Fill(tabla);

            return tabla;
        }

        public Mensaje Guardar(
            Certificacion certificacion)
        {
            Mensaje? validacion =
                Validar(certificacion);

            if (validacion is not null)
            {
                return validacion;
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spGuardarCertificacion",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@IdTrabajador",
                    SqlDbType.Int).Value =
                        certificacion.IdTrabajador;

                comando.Parameters.Add(
                    "@IdProceso",
                    SqlDbType.Int).Value =
                        certificacion.IdProceso;

                comando.Parameters.Add(
                    "@FechaCertificacion",
                    SqlDbType.Date).Value =
                        certificacion.FechaCertificacion.Date;

                comando.Parameters.Add(
                    "@IdCertificador",
                    SqlDbType.Int).Value =
                        certificacion.IdCertificador;

                SqlParameter parametroComentario =
                    comando.Parameters.Add(
                        "@Comentario",
                        SqlDbType.VarChar,
                        300);

                parametroComentario.Value =
                    string.IsNullOrWhiteSpace(
                        certificacion.Comentario)
                        ? DBNull.Value
                        : certificacion.Comentario.Trim();

                conexion.Open();

                using SqlDataReader lector =
                    comando.ExecuteReader();

                return LeerMensaje(lector);
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
                        "La certificación seleccionada no es válida."
                };
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spBorrarCertificacion",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value = id;

                conexion.Open();

                using SqlDataReader lector =
                    comando.ExecuteReader();

                return LeerMensaje(lector);
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static Mensaje? Validar(
            Certificacion certificacion)
        {
            if (certificacion.IdTrabajador <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El trabajador seleccionado no es válido."
                };
            }

            if (certificacion.IdProceso <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un proceso válido."
                };
            }

            if (certificacion.IdCertificador <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un certificador válido."
                };
            }

            if (certificacion.FechaCertificacion ==
                default)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La fecha de certificación no es válida."
                };
            }

            if (certificacion.Comentario?.Length > 300)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El comentario no puede exceder 300 caracteres."
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
                Id = Convert.ToInt32(
                    lector["Id"]),

                Nombre = Convert.ToString(
                    lector["Nombre"]) ?? string.Empty
            };
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
