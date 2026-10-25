using Aseguranza.Clases;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseguranza
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            ConfigurarRegistroGlobalDeErrores();

            // En una publicación portable prepara las carpetas base
            // sin crear todavía configuracion.txt.
            ConfiguracionSistema
                .AsegurarEstructuraPortableBasica();

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

            bool probarRegistroLog = Array.Exists(
                args,
                argumento => argumento.Equals(
                    "--test-log",
                    StringComparison.OrdinalIgnoreCase));

            if (probarRegistroLog)
            {
                ProbarRegistroLog();
                return;
            }

            if (!Clases.ConfiguracionSistema
                    .ExisteArchivoConfiguracion())
            {
                using Ventanas.ConfiguracionVentana configuracionInicial =
                    new Ventanas.ConfiguracionVentana(
                        modoConfiguracionInicial: true);

                configuracionInicial.StartPosition =
                    FormStartPosition.CenterScreen;

                DialogResult resultadoConfiguracion =
                    configuracionInicial.ShowDialog();

                if (resultadoConfiguracion !=
                    DialogResult.OK)
                {
                    return;
                }
            }

            ConfiguracionSistema
                .AsegurarEstructuraPortableConfigurada();

            Application.Run(
                new MenuPrincipal());
        }

        private static void ConfigurarRegistroGlobalDeErrores()
        {
            Application.SetUnhandledExceptionMode(
                UnhandledExceptionMode.CatchException);

            Application.ThreadException +=
                (_, e) =>
                {
                    RegistrarErrorGlobal(
                        "Application.ThreadException",
                        e.Exception);
                };

            AppDomain.CurrentDomain.UnhandledException +=
                (_, e) =>
                {
                    Exception? exception =
                        e.ExceptionObject as Exception;

                    RegistrarErrorGlobal(
                        "AppDomain.UnhandledException" +
                        $" | IsTerminating={e.IsTerminating}",
                        exception);
                };

            TaskScheduler.UnobservedTaskException +=
                (_, e) =>
                {
                    RegistrarErrorGlobal(
                        "TaskScheduler.UnobservedTaskException",
                        e.Exception);

                    e.SetObserved();
                };
        }

        private static void RegistrarErrorGlobal(
            string origen,
            Exception? exception)
        {
            try
            {
                string carpetaLogs =
                    ConfiguracionSistema
                        .ObtenerRutaLogs();

                Directory.CreateDirectory(
                    carpetaLogs);

                string rutaLog =
                    Path.Combine(
                        carpetaLogs,
                        "aplicacion.log");

                StringBuilder sb =
                    new StringBuilder();

                sb.AppendLine(
                    "============================================================");

                sb.AppendLine(
                    $"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

                sb.AppendLine(
                    $"Origen: {origen}");

                if (exception is not null)
                {
                    sb.AppendLine(
                        $"Tipo: {exception.GetType().FullName}");

                    sb.AppendLine(
                        $"Mensaje: {exception.Message}");

                    sb.AppendLine(
                        "StackTrace:");

                    sb.AppendLine(
                        exception.StackTrace ?? "(sin stack trace)");

                    if (exception.InnerException is not null)
                    {
                        sb.AppendLine(
                            "InnerException:");

                        sb.AppendLine(
                            exception.InnerException.ToString());
                    }
                }
                else
                {
                    sb.AppendLine(
                        "No se recibió una instancia Exception.");
                }

                sb.AppendLine();

                File.AppendAllText(
                    rutaLog,
                    sb.ToString(),
                    Encoding.UTF8);
            }
            catch
            {
                // Un fallo en el sistema de logs nunca debe
                // provocar otro error en la aplicación.
            }
        }

        // =========================================================
        // PRUEBA MANUAL DEL SISTEMA DE LOGS
        // =========================================================

        private static void ProbarRegistroLog()
        {
            string carpetaLogs =
                ConfiguracionSistema
                    .ObtenerRutaLogs();

            string rutaLog =
                Path.Combine(
                    carpetaLogs,
                    "aplicacion.log");

            try
            {
                Exception excepcionPrueba =
                    new Exception(
                        "Prueba del sistema de logs en modo " +
                        (ConfiguracionSistema.EsModoPortable()
                            ? "portable."
                            : "normal."));

                RegistrarErrorGlobal(
                    "Prueba manual de log (--test-log)",
                    excepcionPrueba);

                if (!File.Exists(
                        rutaLog))
                {
                    throw new IOException(
                        "La prueba terminó, pero no se encontró el archivo aplicacion.log.");
                }

                MessageBox.Show(
                    "La prueba del sistema de logs se realizó correctamente." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Modo: " +
                    (ConfiguracionSistema.EsModoPortable()
                        ? "Portable"
                        : "Normal") +
                    Environment.NewLine +
                    Environment.NewLine +
                    rutaLog,
                    "Prueba de logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible completar la prueba del sistema de logs." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Ruta esperada:" +
                    Environment.NewLine +
                    rutaLog,
                    "Error en prueba de logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
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
