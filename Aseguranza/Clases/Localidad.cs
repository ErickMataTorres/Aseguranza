using Aseguranza.Data;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class Localidad
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public static DataTable ConsultarLocalidades(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearLocalidadRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarLocalidad(int id)
        {
            return RepositorioFactory
                .CrearLocalidadRepository()
                .Borrar(id);
        }

        // =========================================================
        // VALIDAR DUPLICADO
        // =========================================================

        public static bool ExisteNombre(
            string nombre,
            int idExcluir = 0)
        {
            string nombreNormalizado =
                (nombre ?? string.Empty)
                    .Trim();

            if (string.IsNullOrWhiteSpace(
                    nombreNormalizado))
            {
                return false;
            }

            DataTable localidades =
                ConsultarLocalidades(
                    string.Empty);

            if (!localidades.Columns.Contains(
                    "Nombre"))
            {
                return false;
            }

            foreach (DataRow fila in localidades.Rows)
            {
                string nombreExistente =
                    Convert.ToString(
                        fila["Nombre"])
                        ?.Trim()
                    ?? string.Empty;

                if (!string.Equals(
                        nombreExistente,
                        nombreNormalizado,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int idExistente =
                    0;

                if (localidades.Columns.Contains(
                        "Id") &&
                    fila["Id"] != DBNull.Value)
                {
                    idExistente =
                        Convert.ToInt32(
                            fila["Id"]);
                }

                if (idExistente !=
                    idExcluir)
                {
                    return true;
                }
            }

            return false;
        }

        public Mensaje GuardarLocalidad()
        {
            return RepositorioFactory
                .CrearLocalidadRepository()
                .Guardar(this);
        }
    }
}