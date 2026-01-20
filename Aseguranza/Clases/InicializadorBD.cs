using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class InicializadorBD
    {
        public static void Inicializar()
        {
            string masterConn =
                @"Server=.;Database=master;Trusted_Connection=True;TrustServerCertificate = True;";

            string scriptPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Database",
                "AseguranzaBD.sql"
            );

            if (!File.Exists(scriptPath))
                throw new Exception("No se encontró el script de la base de datos.");

            string script = File.ReadAllText(scriptPath);

            // 🔹 Separar por GO
            var comandos = script
                .Split(new[] { "\r\nGO\r\n", "\nGO\n", "\rGO\r" },
                       StringSplitOptions.RemoveEmptyEntries);

            using SqlConnection conn = new SqlConnection(masterConn);
            conn.Open();

            foreach (string comando in comandos)
            {
                if (string.IsNullOrWhiteSpace(comando))
                    continue;

                using SqlCommand cmd = new SqlCommand(comando, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
