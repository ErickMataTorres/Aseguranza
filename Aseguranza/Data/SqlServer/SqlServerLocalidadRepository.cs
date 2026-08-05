using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerLocalidadRepository : ILocalidadRepository
    {
        public DataTable Consultar(string textoBuscar)
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion = Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarLocalidades",
                conexion);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@TextoBuscar",
                SqlDbType.VarChar,
                100).Value = textoBuscar ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();
            adaptador.Fill(tabla);

            return tabla;
        }

        public Mensaje Guardar(Localidad localidad)
        {
            if (string.IsNullOrWhiteSpace(localidad.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre = "El nombre de la localidad es obligatorio."
                };
            }

            try
            {
                using SqlConnection conexion = Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spGuardarLocalidad",
                    conexion);

                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value = localidad.Id;

                comando.Parameters.Add(
                    "@Nombre",
                    SqlDbType.VarChar,
                    100).Value = localidad.Nombre.Trim();

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
            try
            {
                using SqlConnection conexion = Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spBorrarLocalidad",
                    conexion);

                comando.CommandType = CommandType.StoredProcedure;

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

        private static Mensaje LeerMensaje(SqlDataReader lector)
        {
            if (!lector.Read())
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre = "La operación no devolvió un resultado."
                };
            }

            return new Mensaje
            {
                Id = Convert.ToInt32(lector["Id"]),
                Nombre = Convert.ToString(lector["Nombre"])
            };
        }

        private static Mensaje CrearMensajeError(Exception excepcion)
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