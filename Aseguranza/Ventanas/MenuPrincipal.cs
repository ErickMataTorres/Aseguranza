using Aseguranza.Clases;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Aseguranza
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnCertificaciones_Click(object sender, EventArgs e)
        {
            Ventanas.Certificaciones ventana = new Ventanas.Certificaciones();
            ventana.ShowDialog();
        }

        private void certificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Localidades ventana = new Ventanas.Localidades();
            ventana.ShowDialog();
        }

        private void localidadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Turnos ventana = new Ventanas.Turnos();
            ventana.ShowDialog();
        }

        private void plantaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Plantas ventana = new Ventanas.Plantas();
            ventana.ShowDialog();
        }

        private void lineaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Lineas ventana = new Ventanas.Lineas();
            ventana.ShowDialog();
        }

        private void trabajadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Trabajadores ventana = new Ventanas.Trabajadores();
            ventana.ShowDialog();
        }

        private void certificadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Certificadores ventana = new Ventanas.Certificadores();
            ventana.ShowDialog();
        }

        private void procesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Procesos ventana = new Ventanas.Procesos();
            ventana.ShowDialog();
        }

        private void certificacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Certificaciones ventana = new Ventanas.Certificaciones();
            ventana.ShowDialog();
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    InicializadorBD.Inicializar();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(
            //        "Error inicializando la base de datos:\n" + ex.Message,
            //        "Error crítico",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error
            //    );

            //    Application.Exit();
            //}
        }
        public static void VerificarBaseDatos()
        {
            string masterConn =
                @"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True;";

            string scriptPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Database",
                "AseguranzaBD.sql"
            );

            if (!File.Exists(scriptPath))
                throw new Exception("No se encontró el script de la base de datos.");

            string script = File.ReadAllText(scriptPath);

            using (SqlConnection conn = new SqlConnection(masterConn))
            {
                conn.Open();

                string checkDb = @"
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AseguranzaBD')
            BEGIN
                CREATE DATABASE AseguranzaBD;
            END";

                new SqlCommand(checkDb, conn).ExecuteNonQuery();
            }

            // Ejecutar script completo
            string dbConn =
                @"Server=.\SQLEXPRESS;Database=AseguranzaBD;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(script, conn);
                cmd.ExecuteNonQuery();
            }
        }

        private void verificadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Verificaciones ventana = new Ventanas.Verificaciones();
            ventana.ShowDialog();
        }
    }
}
