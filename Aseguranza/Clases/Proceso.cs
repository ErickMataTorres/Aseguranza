using Aseguranza.Data;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class Proceso
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public int VigenciaMeses { get; set; }

        public static DataTable ConsultarProcesos(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearProcesoRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarProceso(int id)
        {
            return RepositorioFactory
                .CrearProcesoRepository()
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

            DataTable procesos =
                ConsultarProcesos(
                    string.Empty);

            if (!procesos.Columns.Contains(
                    "Nombre"))
            {
                return false;
            }

            foreach (DataRow fila in procesos.Rows)
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

                if (procesos.Columns.Contains(
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

        public Mensaje GuardarProceso()
        {
            return RepositorioFactory
                .CrearProcesoRepository()
                .Guardar(this);
        }
    }
}