using Aseguranza.Clases;
using Aseguranza.UI;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public sealed class ConfiguracionVentana : Form
    {
        private readonly ComboBox cbProveedor =
            new ComboBox();

        private readonly TextBox txtServidor =
            new TextBox();

        private readonly TextBox txtBaseDatos =
            new TextBox();

        private readonly ComboBox cbAutenticacion =
            new ComboBox();

        private readonly TextBox txtUsuario =
            new TextBox();

        private readonly TextBox txtContrasena =
            new TextBox();

        private readonly CheckBox chkMostrarContrasena =
            new CheckBox();

        private readonly CheckBox chkEncrypt =
            new CheckBox();

        private readonly CheckBox chkTrustServerCertificate =
            new CheckBox();

        private readonly TextBox txtRutaSqlite =
            new TextBox();

        private readonly TextBox txtRutaArchivos =
            new TextBox();

        private readonly Panel pnlSqlServer =
            new Panel();

        private readonly Panel pnlSqlite =
            new Panel();

        private readonly Button btnProbar =
            new Button();

        private readonly Button btnGuardar =
            new Button();

        private readonly Button btnCancelar =
            new Button();

        private Panel pnlContenido =
            null!;

        private Label lblRutaArchivos =
            null!;

        private Panel pnlRutaArchivos =
            null!;

        private Button btnExaminarArchivos =
            null!;

        private Label lblRutaConfiguracion =
            null!;

        private SqlConnectionStringBuilder
            sqlBuilderBase =
                new SqlConnectionStringBuilder();

        private bool cargandoDatos;

        private readonly bool modoConfiguracionInicial;

        public ConfiguracionVentana(
            bool modoConfiguracionInicial = false)
        {
            this.modoConfiguracionInicial =
                modoConfiguracionInicial;

            DoubleBuffered =
                true;

            KeyPreview =
                true;

            AplicarEstiloVisual();
            CargarConfiguracionActual();
        }

        // =========================================================
        // INTERFAZ
        // =========================================================

        private void AplicarEstiloVisual()
        {
            SuspendLayout();

            string tituloVentana =
                modoConfiguracionInicial
                    ? "Configuración inicial"
                    : "Configuración";

            string subtituloVentana =
                modoConfiguracionInicial
                    ? "Configura la conexión y las rutas antes de comenzar a utilizar el sistema."
                    : "Configura la conexión de base de datos y las rutas utilizadas por el sistema.";

            FormStyler.ApplyBase(
                this,
                tituloVentana,
                new Size(
                    800,
                    760));

            FormStyler.CreateHeader(
                this,
                tituloVentana,
                subtituloVentana,
                height: 110,
                titleX: 38,
                titleY: 20,
                subtitleX: 40,
                subtitleY: 61);

            pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        130),
                    new Size(
                        760,
                        590),
                    radius: 14,
                    anchor:
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right |
                        AnchorStyles.Bottom);

            // =====================================================
            // PROVEEDOR
            // =====================================================

            pnlContenido.Controls.Add(
                CrearEtiqueta(
                    "Tipo de base de datos *",
                    24,
                    20));

            Panel pnlProveedor =
                CrearContenedorInput(
                    new Point(
                        24,
                        44),
                    new Size(
                        712,
                        42));

            cbProveedor.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cbProveedor.Items.AddRange(
                new object[]
                {
                    "SQL Server / Azure SQL",
                    "SQLite"
                });

            ConfigurarControlDentroDePanel(
                cbProveedor,
                pnlProveedor);

            ComboBoxStyler.ApplyOutlinedComboBox(
                pnlProveedor,
                cbProveedor);

            cbProveedor.SelectedIndexChanged +=
                (_, _) =>
                {
                    if (!cargandoDatos)
                    {
                        ActualizarProveedorVisible();
                    }
                };

            pnlContenido.Controls.Add(
                pnlProveedor);

            if (modoConfiguracionInicial)
            {
                Label lblConfiguracionInicial =
                    new Label
                    {
                        AutoSize =
                            true,

                        Location =
                            new Point(
                                24,
                                90),

                        Text =
                            "Esta configuración es necesaria para iniciar la aplicación.",

                        ForeColor =
                            AppColors.TextSecondary,

                        Font =
                            AppFonts.Light(
                                8.5F),

                        BackColor =
                            Color.Transparent
                    };

                pnlContenido.Controls.Add(
                    lblConfiguracionInicial);
            }

            // =====================================================
            // SQL SERVER / AZURE SQL
            // =====================================================

            ConfigurarPanelSqlServer();

            pnlSqlServer.Location =
                new Point(
                    24,
                    104);

            pnlSqlServer.Size =
                new Size(
                    712,
                    280);

            pnlSqlServer.BackColor =
                AppColors.SectionBackground;

            RoundedControlHelper.ApplyRoundedRegion(
                pnlSqlServer,
                10);

            pnlContenido.Controls.Add(
                pnlSqlServer);

            // =====================================================
            // SQLITE
            // =====================================================

            ConfigurarPanelSqlite();

            pnlSqlite.Location =
                new Point(
                    24,
                    104);

            pnlSqlite.Size =
                new Size(
                    712,
                    160);

            pnlSqlite.BackColor =
                AppColors.SectionBackground;

            RoundedControlHelper.ApplyRoundedRegion(
                pnlSqlite,
                10);

            pnlContenido.Controls.Add(
                pnlSqlite);

            // =====================================================
            // RUTA DE ARCHIVOS
            // =====================================================

            lblRutaArchivos =
                CrearEtiqueta(
                    "Carpeta de fotografías y expedientes",
                    24,
                    406);

            pnlContenido.Controls.Add(
                lblRutaArchivos);

            pnlRutaArchivos =
                CrearContenedorInput(
                    new Point(
                        24,
                        430),
                    new Size(
                        570,
                        42));

            ConfigurarTextBoxDentroDePanel(
                txtRutaArchivos,
                pnlRutaArchivos);

            InputStyler.ApplyOutlinedInput(
                pnlRutaArchivos,
                txtRutaArchivos);

            pnlContenido.Controls.Add(
                pnlRutaArchivos);

            btnExaminarArchivos =
                new Button();

            ButtonStyler.Apply(
                btnExaminarArchivos,
                "Examinar",
                AppColors.Secondary,
                string.Empty,
                width: 130,
                height: 42);

            btnExaminarArchivos.Location =
                new Point(
                    606,
                    430);

            btnExaminarArchivos.Click +=
                (_, _) =>
                {
                    SeleccionarCarpetaArchivos();
                };

            pnlContenido.Controls.Add(
                btnExaminarArchivos);

            lblRutaConfiguracion =
                new Label
                {
                    AutoSize =
                        false,

                    Location =
                        new Point(
                            24,
                            482),

                    Size =
                        new Size(
                            712,
                            36),

                    Text =
                        "La configuración se guarda en: " +
                        ConfiguracionSistema
                            .ObtenerRutaArchivoConfiguracion(),

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            8.5F),

                    BackColor =
                        Color.Transparent
                };

            pnlContenido.Controls.Add(
                lblRutaConfiguracion);

            // =====================================================
            // BOTONES
            // =====================================================

            ButtonStyler.Apply(
                btnProbar,
                "Probar conexión",
                AppColors.Secondary,
                string.Empty,
                width: 155,
                height: 42);

            btnProbar.Location =
                new Point(
                    24,
                    528);

            btnProbar.Click +=
                async (_, _) =>
                {
                    await ProbarConexionAsync();
                };

            pnlContenido.Controls.Add(
                btnProbar);

            ButtonStyler.Apply(
                btnGuardar,
                "Guardar",
                AppColors.Primary,
                string.Empty,
                width: 138,
                height: 42);

            btnGuardar.Location =
                new Point(
                    440,
                    528);

            btnGuardar.Click +=
                (_, _) =>
                {
                    GuardarConfiguracion();
                };

            pnlContenido.Controls.Add(
                btnGuardar);

            ButtonStyler.Apply(
                btnCancelar,
                modoConfiguracionInicial
                    ? "Salir"
                    : "Cancelar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 146,
                height: 42);

            btnCancelar.Location =
                new Point(
                    590,
                    528);

            btnCancelar.Click +=
                (_, _) =>
                {
                    DialogResult =
                        DialogResult.Cancel;

                    Close();
                };

            pnlContenido.Controls.Add(
                btnCancelar);

            AcceptButton =
                btnGuardar;

            CancelButton =
                btnCancelar;

            ResumeLayout(
                true);
        }

        private void ConfigurarPanelSqlServer()
        {
            pnlSqlServer.Controls.Clear();

            pnlSqlServer.Controls.Add(
                CrearTituloSeccion(
                    "SQL Server / Azure SQL",
                    20,
                    16));

            pnlSqlServer.Controls.Add(
                CrearSubtituloSeccion(
                    "Configura el servidor y el método de autenticación.",
                    20,
                    42));

            // Servidor
            pnlSqlServer.Controls.Add(
                CrearEtiqueta(
                    "Servidor *",
                    20,
                    74));

            Panel pnlServidor =
                CrearContenedorInput(
                    new Point(
                        20,
                        98),
                    new Size(
                        320,
                        42));

            ConfigurarTextBoxDentroDePanel(
                txtServidor,
                pnlServidor);

            InputStyler.ApplyOutlinedInput(
                pnlServidor,
                txtServidor);

            pnlSqlServer.Controls.Add(
                pnlServidor);

            // Base de datos
            pnlSqlServer.Controls.Add(
                CrearEtiqueta(
                    "Base de datos *",
                    360,
                    74));

            Panel pnlBaseDatos =
                CrearContenedorInput(
                    new Point(
                        360,
                        98),
                    new Size(
                        332,
                        42));

            ConfigurarTextBoxDentroDePanel(
                txtBaseDatos,
                pnlBaseDatos);

            InputStyler.ApplyOutlinedInput(
                pnlBaseDatos,
                txtBaseDatos);

            pnlSqlServer.Controls.Add(
                pnlBaseDatos);

            // Autenticación
            pnlSqlServer.Controls.Add(
                CrearEtiqueta(
                    "Autenticación",
                    20,
                    152));

            Panel pnlAutenticacion =
                CrearContenedorInput(
                    new Point(
                        20,
                        176),
                    new Size(
                        320,
                        42));

            cbAutenticacion.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cbAutenticacion.Items.AddRange(
                new object[]
                {
                    "Autenticación de Windows",
                    "Usuario y contraseña"
                });

            ConfigurarControlDentroDePanel(
                cbAutenticacion,
                pnlAutenticacion);

            ComboBoxStyler.ApplyOutlinedComboBox(
                pnlAutenticacion,
                cbAutenticacion);

            cbAutenticacion.SelectedIndexChanged +=
                (_, _) =>
                {
                    ActualizarAutenticacion();
                };

            pnlSqlServer.Controls.Add(
                pnlAutenticacion);

            // Usuario
            pnlSqlServer.Controls.Add(
                CrearEtiqueta(
                    "Usuario",
                    360,
                    152));

            Panel pnlUsuario =
                CrearContenedorInput(
                    new Point(
                        360,
                        176),
                    new Size(
                        160,
                        42));

            ConfigurarTextBoxDentroDePanel(
                txtUsuario,
                pnlUsuario);

            InputStyler.ApplyOutlinedInput(
                pnlUsuario,
                txtUsuario);

            pnlSqlServer.Controls.Add(
                pnlUsuario);

            // Contraseña
            pnlSqlServer.Controls.Add(
                CrearEtiqueta(
                    "Contraseña",
                    532,
                    152));

            Panel pnlContrasena =
                CrearContenedorInput(
                    new Point(
                        532,
                        176),
                    new Size(
                        160,
                        42));

            ConfigurarTextBoxDentroDePanel(
                txtContrasena,
                pnlContrasena);

            txtContrasena.UseSystemPasswordChar =
                true;

            InputStyler.ApplyOutlinedInput(
                pnlContrasena,
                txtContrasena);

            pnlSqlServer.Controls.Add(
                pnlContrasena);

            chkMostrarContrasena.AutoSize =
                true;

            chkMostrarContrasena.Location =
                new Point(
                    532,
                    224);

            chkMostrarContrasena.Text =
                "Mostrar contraseña";

            chkMostrarContrasena.ForeColor =
                AppColors.TextSecondary;

            chkMostrarContrasena.Font =
                AppFonts.Light(
                    8.5F);

            chkMostrarContrasena.CheckedChanged +=
                (_, _) =>
                {
                    txtContrasena
                        .UseSystemPasswordChar =
                        !chkMostrarContrasena.Checked;
                };

            pnlSqlServer.Controls.Add(
                chkMostrarContrasena);

            chkEncrypt.AutoSize =
                true;

            chkEncrypt.Location =
                new Point(
                    20,
                    234);

            chkEncrypt.Text =
                "Encrypt";

            chkEncrypt.ForeColor =
                AppColors.TextPrimary;

            chkEncrypt.Font =
                AppFonts.Regular(
                    9F);

            pnlSqlServer.Controls.Add(
                chkEncrypt);

            chkTrustServerCertificate.AutoSize =
                true;

            chkTrustServerCertificate.Location =
                new Point(
                    110,
                    234);

            chkTrustServerCertificate.Text =
                "Confiar en certificado del servidor";

            chkTrustServerCertificate.ForeColor =
                AppColors.TextPrimary;

            chkTrustServerCertificate.Font =
                AppFonts.Regular(
                    9F);

            pnlSqlServer.Controls.Add(
                chkTrustServerCertificate);
        }

        private void ConfigurarPanelSqlite()
        {
            pnlSqlite.Controls.Clear();

            pnlSqlite.Controls.Add(
                CrearTituloSeccion(
                    "SQLite",
                    20,
                    16));

            pnlSqlite.Controls.Add(
                CrearSubtituloSeccion(
                    "Selecciona el archivo de base de datos que utilizará la aplicación.",
                    20,
                    42));

            pnlSqlite.Controls.Add(
                CrearEtiqueta(
                    "Archivo SQLite *",
                    20,
                    76));

            Panel pnlRutaSqlite =
                CrearContenedorInput(
                    new Point(
                        20,
                        100),
                    new Size(
                        520,
                        42));

            ConfigurarTextBoxDentroDePanel(
                txtRutaSqlite,
                pnlRutaSqlite);

            InputStyler.ApplyOutlinedInput(
                pnlRutaSqlite,
                txtRutaSqlite);

            pnlSqlite.Controls.Add(
                pnlRutaSqlite);

            Button btnExaminarSqlite =
                new Button();

            ButtonStyler.Apply(
                btnExaminarSqlite,
                "Examinar",
                AppColors.Secondary,
                string.Empty,
                width: 130,
                height: 42);

            btnExaminarSqlite.Location =
                new Point(
                    552,
                    100);

            btnExaminarSqlite.Click +=
                (_, _) =>
                {
                    SeleccionarArchivoSqlite();
                };

            pnlSqlite.Controls.Add(
                btnExaminarSqlite);
        }

        // =========================================================
        // CARGAR CONFIGURACIÓN
        // =========================================================

        private void CargarConfiguracionActual()
        {
            cargandoDatos =
                true;

            try
            {
                ProveedorBaseDatos proveedor =
                    ConfiguracionSistema
                        .ObtenerProveedorBaseDatos();

                cbProveedor.SelectedIndex =
                    proveedor ==
                        ProveedorBaseDatos.SQLite
                            ? 1
                            : 0;

                txtRutaSqlite.Text =
                    ConfiguracionSistema
                        .ObtenerRutaSqliteConfigurada();

                txtRutaArchivos.Text =
                    ConfiguracionSistema
                        .ObtenerRutaArchivosConfigurada();

                string cadenaConexion =
                    ConfiguracionSistema
                        .ObtenerCadenaConexion();

                try
                {
                    sqlBuilderBase =
                        new SqlConnectionStringBuilder(
                            cadenaConexion);
                }
                catch
                {
                    sqlBuilderBase =
                        new SqlConnectionStringBuilder();
                }

                txtServidor.Text =
                    sqlBuilderBase.DataSource ??
                    string.Empty;

                txtBaseDatos.Text =
                    sqlBuilderBase.InitialCatalog ??
                    string.Empty;

                cbAutenticacion.SelectedIndex =
                    sqlBuilderBase.IntegratedSecurity
                        ? 0
                        : 1;

                txtUsuario.Text =
                    sqlBuilderBase.UserID ??
                    string.Empty;

                txtContrasena.Text =
                    sqlBuilderBase.Password ??
                    string.Empty;

                string valorEncrypt =
                    sqlBuilderBase["Encrypt"]?
                        .ToString()
                    ?? string.Empty;

                chkEncrypt.Checked =
                    !valorEncrypt.Equals(
                        "False",
                        StringComparison.OrdinalIgnoreCase) &&
                    !valorEncrypt.Equals(
                        "Optional",
                        StringComparison.OrdinalIgnoreCase);

                chkTrustServerCertificate.Checked =
                    sqlBuilderBase
                        .TrustServerCertificate;
            }
            finally
            {
                cargandoDatos =
                    false;
            }

            ActualizarProveedorVisible();
            ActualizarAutenticacion();

            cbProveedor.Focus();
        }

        // =========================================================
        // ESTADO DE CONTROLES
        // =========================================================

        private void ActualizarProveedorVisible()
        {
            bool usarSqlite =
                cbProveedor.SelectedIndex ==
                1;

            pnlSqlServer.Visible =
                !usarSqlite;

            pnlSqlite.Visible =
                usarSqlite;

            int desplazamientoInicial =
                modoConfiguracionInicial
                    ? 20
                    : 0;

            pnlSqlServer.Location =
                new Point(
                    24,
                    104 + desplazamientoInicial);

            pnlSqlite.Location =
                new Point(
                    24,
                    104 + desplazamientoInicial);

            SuspendLayout();

            try
            {
                if (usarSqlite)
                {
                    ClientSize =
                        new Size(
                            800,
                            640 + desplazamientoInicial);

                    pnlContenido.Size =
                        new Size(
                            760,
                            470 + desplazamientoInicial);

                    lblRutaArchivos.Location =
                        new Point(
                            24,
                            286 + desplazamientoInicial);

                    pnlRutaArchivos.Location =
                        new Point(
                            24,
                            310 + desplazamientoInicial);

                    btnExaminarArchivos.Location =
                        new Point(
                            606,
                            310 + desplazamientoInicial);

                    lblRutaConfiguracion.Location =
                        new Point(
                            24,
                            362 + desplazamientoInicial);

                    btnProbar.Location =
                        new Point(
                            24,
                            408 + desplazamientoInicial);

                    btnGuardar.Location =
                        new Point(
                            440,
                            408 + desplazamientoInicial);

                    btnCancelar.Location =
                        new Point(
                            590,
                            408 + desplazamientoInicial);
                }
                else
                {
                    ClientSize =
                        new Size(
                            800,
                            760 + desplazamientoInicial);

                    pnlContenido.Size =
                        new Size(
                            760,
                            590 + desplazamientoInicial);

                    lblRutaArchivos.Location =
                        new Point(
                            24,
                            406 + desplazamientoInicial);

                    pnlRutaArchivos.Location =
                        new Point(
                            24,
                            430 + desplazamientoInicial);

                    btnExaminarArchivos.Location =
                        new Point(
                            606,
                            430 + desplazamientoInicial);

                    lblRutaConfiguracion.Location =
                        new Point(
                            24,
                            482 + desplazamientoInicial);

                    btnProbar.Location =
                        new Point(
                            24,
                            528 + desplazamientoInicial);

                    btnGuardar.Location =
                        new Point(
                            440,
                            528 + desplazamientoInicial);

                    btnCancelar.Location =
                        new Point(
                            590,
                            528 + desplazamientoInicial);
                }

                pnlContenido.Invalidate();
            }
            finally
            {
                ResumeLayout(
                    true);
            }
        }

        private void ActualizarAutenticacion()
        {
            bool autenticacionWindows =
                cbAutenticacion.SelectedIndex ==
                0;

            txtUsuario.Enabled =
                !autenticacionWindows;

            txtContrasena.Enabled =
                !autenticacionWindows;

            chkMostrarContrasena.Enabled =
                !autenticacionWindows;

            if (autenticacionWindows)
            {
                chkMostrarContrasena.Checked =
                    false;
            }
        }

        // =========================================================
        // CONSTRUIR CADENA SQL SERVER
        // =========================================================

        private SqlConnectionStringBuilder
            ConstruirCadenaSqlServer()
        {
            SqlConnectionStringBuilder builder;

            try
            {
                builder =
                    new SqlConnectionStringBuilder(
                        sqlBuilderBase
                            .ConnectionString);
            }
            catch
            {
                builder =
                    new SqlConnectionStringBuilder();
            }

            builder.DataSource =
                txtServidor.Text.Trim();

            builder.InitialCatalog =
                txtBaseDatos.Text.Trim();

            bool autenticacionWindows =
                cbAutenticacion.SelectedIndex ==
                0;

            builder.IntegratedSecurity =
                autenticacionWindows;

            if (autenticacionWindows)
            {
                builder.UserID =
                    string.Empty;

                builder.Password =
                    string.Empty;
            }
            else
            {
                builder.UserID =
                    txtUsuario.Text.Trim();

                builder.Password =
                    txtContrasena.Text;
            }

            builder["Encrypt"] =
                chkEncrypt.Checked
                    ? "True"
                    : "False";

            builder.TrustServerCertificate =
                chkTrustServerCertificate.Checked;

            if (builder.ConnectTimeout <= 0)
            {
                builder.ConnectTimeout =
                    15;
            }

            return builder;
        }

        // =========================================================
        // VALIDACIÓN
        // =========================================================

        private bool ValidarConfiguracion(
            bool mostrarMensajes)
        {
            if (cbProveedor.SelectedIndex < 0)
            {
                if (mostrarMensajes)
                {
                    AppDialog.ShowWarning(
                        this,
                        "Dato requerido",
                        "Seleccione el tipo de base de datos.");
                }

                return false;
            }

            bool usarSqlite =
                cbProveedor.SelectedIndex ==
                1;

            if (usarSqlite)
            {
                if (string.IsNullOrWhiteSpace(
                        txtRutaSqlite.Text))
                {
                    if (mostrarMensajes)
                    {
                        AppDialog.ShowWarning(
                            this,
                            "Dato requerido",
                            "Seleccione la ruta del archivo SQLite.");
                    }

                    txtRutaSqlite.Focus();

                    return false;
                }

                return true;
            }

            if (string.IsNullOrWhiteSpace(
                    txtServidor.Text))
            {
                if (mostrarMensajes)
                {
                    AppDialog.ShowWarning(
                        this,
                        "Dato requerido",
                        "Ingrese el servidor de SQL Server o Azure SQL.");
                }

                txtServidor.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    txtBaseDatos.Text))
            {
                if (mostrarMensajes)
                {
                    AppDialog.ShowWarning(
                        this,
                        "Dato requerido",
                        "Ingrese el nombre de la base de datos.");
                }

                txtBaseDatos.Focus();

                return false;
            }

            bool autenticacionWindows =
                cbAutenticacion.SelectedIndex ==
                0;

            if (!autenticacionWindows)
            {
                if (string.IsNullOrWhiteSpace(
                        txtUsuario.Text))
                {
                    if (mostrarMensajes)
                    {
                        AppDialog.ShowWarning(
                            this,
                            "Dato requerido",
                            "Ingrese el usuario de SQL Server o Azure SQL.");
                    }

                    txtUsuario.Focus();

                    return false;
                }

                if (string.IsNullOrWhiteSpace(
                        txtContrasena.Text))
                {
                    if (mostrarMensajes)
                    {
                        AppDialog.ShowWarning(
                            this,
                            "Dato requerido",
                            "Ingrese la contraseña.");
                    }

                    txtContrasena.Focus();

                    return false;
                }
            }

            return true;
        }

        // =========================================================
        // PROBAR CONEXIÓN
        // =========================================================

        private async Task ProbarConexionAsync()
        {
            if (!ValidarConfiguracion(
                    mostrarMensajes: true))
            {
                return;
            }

            btnProbar.Enabled =
                false;

            ButtonStyler.UpdateEnabledState(
                btnProbar,
                AppColors.Secondary);

            try
            {
                bool usarSqlite =
                    cbProveedor.SelectedIndex ==
                    1;

                if (usarSqlite)
                {
                    string ruta =
                        ConfiguracionSistema
                            .ResolverRutaSistema(
                                txtRutaSqlite.Text.Trim());

                    if (!File.Exists(
                            ruta))
                    {
                        AppDialog.ShowWarning(
                            this,
                            "Archivo SQLite no encontrado",
                            "El archivo seleccionado todavía no existe." +
                            Environment.NewLine +
                            Environment.NewLine +
                            "Guarde la configuración y utilice la inicialización SQLite para crear la base de datos.");

                        return;
                    }

                    SqliteConnectionStringBuilder builder =
                        new SqliteConnectionStringBuilder
                        {
                            DataSource =
                                ruta,

                            Mode =
                                SqliteOpenMode.ReadWrite,

                            ForeignKeys =
                                true,

                            Pooling =
                                true,

                            DefaultTimeout =
                                10
                        };

                    await using SqliteConnection conexionSqlite =
                        new SqliteConnection(
                            builder.ToString());

                    await conexionSqlite.OpenAsync();

                    AppDialog.ShowInfo(
                        this,
                        "Conexión correcta",
                        "La conexión con SQLite se realizó correctamente.");

                    return;
                }

                SqlConnectionStringBuilder sqlBuilder =
                    ConstruirCadenaSqlServer();

                SqlConnectionStringBuilder pruebaBuilder =
                    new SqlConnectionStringBuilder(
                        sqlBuilder.ConnectionString)
                    {
                        ConnectTimeout =
                            10
                    };

                await using SqlConnection conexionSqlServer =
                    new SqlConnection(
                        pruebaBuilder.ConnectionString);

                await conexionSqlServer.OpenAsync();

                AppDialog.ShowInfo(
                    this,
                    "Conexión correcta",
                    "La conexión con SQL Server / Azure SQL se realizó correctamente.");
            }
            catch (Exception ex)
            {
                AppDialog.ShowError(
                    this,
                    "No se pudo conectar",
                    ex.Message);
            }
            finally
            {
                btnProbar.Enabled =
                    true;

                ButtonStyler.UpdateEnabledState(
                    btnProbar,
                    AppColors.Secondary);
            }
        }

        // =========================================================
        // GUARDAR
        // =========================================================

        private void GuardarConfiguracion()
        {
            if (!ValidarConfiguracion(
                    mostrarMensajes: true))
            {
                return;
            }

            try
            {
                ProveedorBaseDatos proveedor =
                    cbProveedor.SelectedIndex ==
                    1
                        ? ProveedorBaseDatos.SQLite
                        : ProveedorBaseDatos.SqlServer;

                string cadenaConexion =
                    ConstruirCadenaSqlServer()
                        .ConnectionString;

                ConfiguracionSistema
                    .GuardarConfiguracion(
                        proveedor,
                        cadenaConexion,
                        txtRutaSqlite.Text.Trim(),
                        txtRutaArchivos.Text.Trim());

                AppDialog.ShowInfo(
                    this,
                    "Configuración guardada",
                    "La configuración se guardó correctamente." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Las nuevas operaciones utilizarán esta configuración.");

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                AppDialog.ShowError(
                    this,
                    "No se pudo guardar",
                    ex.Message);
            }
        }

        // =========================================================
        // EXAMINAR
        // =========================================================

        private void SeleccionarArchivoSqlite()
        {
            using SaveFileDialog dialogo =
                new SaveFileDialog
                {
                    Title =
                        "Seleccionar base de datos SQLite",

                    Filter =
                        "Base de datos SQLite (*.db)|*.db|Todos los archivos (*.*)|*.*",

                    DefaultExt =
                        "db",

                    AddExtension =
                        true,

                    OverwritePrompt =
                        false,

                    CheckPathExists =
                        true
                };

            string rutaActual =
                txtRutaSqlite.Text.Trim();

            if (!string.IsNullOrWhiteSpace(
                    rutaActual))
            {
                try
                {
                    string rutaActualResuelta =
                        ConfiguracionSistema
                            .ResolverRutaSistema(
                                rutaActual);

                    string? carpeta =
                        Path.GetDirectoryName(
                            rutaActualResuelta);

                    string archivo =
                        Path.GetFileName(
                            rutaActualResuelta);

                    if (!string.IsNullOrWhiteSpace(
                            carpeta) &&
                        Directory.Exists(
                            carpeta))
                    {
                        dialogo.InitialDirectory =
                            carpeta;
                    }

                    if (!string.IsNullOrWhiteSpace(
                            archivo))
                    {
                        dialogo.FileName =
                            archivo;
                    }
                }
                catch
                {
                    // Si la ruta actual no es válida,
                    // simplemente abrimos el diálogo normalmente.
                }
            }

            if (dialogo.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            txtRutaSqlite.Text =
                ConfiguracionSistema
                    .ConvertirRutaParaConfiguracion(
                        dialogo.FileName);
        }

        private void SeleccionarCarpetaArchivos()
        {
            using FolderBrowserDialog dialogo =
                new FolderBrowserDialog
                {
                    Description =
                        "Seleccione la carpeta donde se guardarán fotografías, expedientes y documentos.",

                    UseDescriptionForTitle =
                        true,

                    ShowNewFolderButton =
                        true
                };

            string rutaActual =
                txtRutaArchivos.Text.Trim();

            if (!string.IsNullOrWhiteSpace(
                    rutaActual))
            {
                string rutaActualResuelta =
                    ConfiguracionSistema
                        .ResolverRutaSistema(
                            rutaActual);

                if (Directory.Exists(
                        rutaActualResuelta))
                {
                    dialogo.InitialDirectory =
                        rutaActualResuelta;
                }
            }

            if (dialogo.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            txtRutaArchivos.Text =
                ConfiguracionSistema
                    .ConvertirRutaParaConfiguracion(
                        dialogo.SelectedPath);
        }

        // =========================================================
        // HELPERS DE UI
        // =========================================================

        private static Label CrearEtiqueta(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                AutoSize =
                    true,

                Location =
                    new Point(
                        x,
                        y),

                Text =
                    texto,

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Regular(
                        9.5F,
                        FontStyle.Bold),

                BackColor =
                    Color.Transparent
            };
        }

        private static Label CrearTituloSeccion(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                AutoSize =
                    true,

                Location =
                    new Point(
                        x,
                        y),

                Text =
                    texto,

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Regular(
                        12.5F,
                        FontStyle.Bold),

                BackColor =
                    Color.Transparent
            };
        }

        private static Label CrearSubtituloSeccion(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                AutoSize =
                    true,

                Location =
                    new Point(
                        x,
                        y),

                Text =
                    texto,

                ForeColor =
                    AppColors.TextSecondary,

                Font =
                    AppFonts.Light(
                        9F),

                BackColor =
                    Color.Transparent
            };
        }

        private static Panel CrearContenedorInput(
            Point location,
            Size size)
        {
            return new Panel
            {
                Location =
                    location,

                Size =
                    size,

                BackColor =
                    Color.White
            };
        }

        private static void ConfigurarTextBoxDentroDePanel(
            TextBox textBox,
            Panel panel)
        {
            textBox.BorderStyle =
                BorderStyle.None;

            textBox.Location =
                new Point(
                    12,
                    11);

            textBox.Size =
                new Size(
                    panel.Width - 24,
                    22);

            textBox.Font =
                AppFonts.Regular(
                    10F);

            textBox.BackColor =
                Color.White;

            textBox.ForeColor =
                AppColors.TextPrimary;

            panel.Controls.Add(
                textBox);
        }

        private static void ConfigurarControlDentroDePanel(
            Control control,
            Panel panel)
        {
            control.Location =
                new Point(
                    8,
                    7);

            control.Size =
                new Size(
                    panel.Width - 16,
                    28);

            control.Font =
                AppFonts.Regular(
                    10F);

            control.BackColor =
                Color.White;

            control.ForeColor =
                AppColors.TextPrimary;

            panel.Controls.Add(
                control);
        }

        // =========================================================
        // TECLADO
        // =========================================================

        protected override bool ProcessCmdKey(
            ref Message msg,
            Keys keyData)
        {
            if (keyData ==
                Keys.Escape)
            {
                DialogResult =
                    DialogResult.Cancel;

                Close();

                return true;
            }

            return base.ProcessCmdKey(
                ref msg,
                keyData);
        }
    }
}
