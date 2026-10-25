using Aseguranza.Data;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class Linea
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int IdPlanta { get; set; }

        public string? NombrePlanta { get; set; }

        public static DataTable ConsultarLineas(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static DataTable ConsultarLineasPorPlanta(
            int idPlanta)
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .ConsultarPorPlanta(idPlanta);
        }

        public static Mensaje BorrarLinea(int id)
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .Borrar(id);
        }

        // =========================================================
        // VALIDAR DUPLICADO POR PLANTA
        // =========================================================

        public static bool ExisteNombreEnPlanta(
            int idPlanta,
            string nombre,
            int idExcluir = 0)
        {
            string nombreNormalizado =
                (nombre ?? string.Empty)
                    .Trim();

            if (idPlanta <= 0 ||
                string.IsNullOrWhiteSpace(
                    nombreNormalizado))
            {
                return false;
            }

            DataTable lineas =
                ConsultarLineasPorPlanta(
                    idPlanta);

            if (!lineas.Columns.Contains(
                    "Nombre"))
            {
                return false;
            }

            foreach (DataRow fila in lineas.Rows)
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

                if (lineas.Columns.Contains(
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

        public Mensaje GuardarLinea()
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .Guardar(this);
        }
    }
}