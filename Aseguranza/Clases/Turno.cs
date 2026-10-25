using Aseguranza.Data;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class Turno
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public static DataTable ConsultarTurnos(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearTurnoRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarTurno(int id)
        {
            return RepositorioFactory
                .CrearTurnoRepository()
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

            DataTable turnos =
                ConsultarTurnos(
                    string.Empty);

            if (!turnos.Columns.Contains(
                    "Nombre"))
            {
                return false;
            }

            foreach (DataRow fila in turnos.Rows)
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

                if (turnos.Columns.Contains(
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

        public Mensaje GuardarTurno()
        {
            return RepositorioFactory
                .CrearTurnoRepository()
                .Guardar(this);
        }
    }
}