using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerPlantaRepository : IPlantaRepository
    {
        public DataTable Consultar(string textoBuscar)
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarPlantas",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

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

        public Mensaje Guardar(Planta planta)
        {
            if (string.IsNullOrWhiteSpace(planta.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre = "El nombre de la planta es obligatorio."
                };
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spGuardarPlanta",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value = planta.Id;

                comando.Parameters.Add(
                    "@Nombre",
                    SqlDbType.VarChar,
                    100).Value = planta.Nombre.Trim();

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
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spBorrarPlanta",
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
                Id = Convert.ToInt32(lector["Id"]),
                Nombre = Convert.ToString(lector["Nombre"]) ?? string.Empty
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