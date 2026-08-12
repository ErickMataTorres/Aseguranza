using Aseguranza.Data;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class Planta
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public static DataTable ConsultarPlantas(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearPlantaRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarPlanta(int id)
        {
            return RepositorioFactory
                .CrearPlantaRepository()
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

            DataTable plantas =
                ConsultarPlantas(
                    string.Empty);

            if (!plantas.Columns.Contains(
                    "Nombre"))
            {
                return false;
            }

            foreach (DataRow fila in plantas.Rows)
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

                if (plantas.Columns.Contains(
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

        public Mensaje GuardarPlanta()
        {
            return RepositorioFactory
                .CrearPlantaRepository()
                .Guardar(this);
        }
    }
}