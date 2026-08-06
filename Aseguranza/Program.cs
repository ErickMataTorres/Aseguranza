using Aseguranza.Clases;

namespace Aseguranza
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            bool inicializarSqlite = Array.Exists(
                args,
                argumento => argumento.Equals(
                    "--init-sqlite",
                    StringComparison.OrdinalIgnoreCase));

            if (inicializarSqlite)
            {
                InicializarBaseSqlite();
                return;
            }

            Application.Run(new MenuPrincipal());
        }

        private static void InicializarBaseSqlite()
        {
            try
            {
                InicializadorSqlite.Inicializar();

                if (!InicializadorSqlite.EstaInicializada())
                {
                    throw new InvalidOperationException(
                        "El archivo SQLite fue creado, pero no se pudo verificar su esquema.");
                }

                string rutaBaseDatos =
                    ConfiguracionSistema.ObtenerRutaSqlite();

                MessageBox.Show(
                    "La base SQLite se creó y verificó correctamente." +
                    Environment.NewLine +
                    Environment.NewLine +
                    rutaBaseDatos,
                    "Inicialización SQLite",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible inicializar la base SQLite." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "Error de inicialización SQLite",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}