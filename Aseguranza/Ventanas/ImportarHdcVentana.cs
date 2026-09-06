using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public sealed class ImportarHdcVentana : Form
    {
        private readonly TextBox txtRutaArchivo =
            new TextBox();

        private readonly Button btnExaminar =
            new Button();

        private readonly Button btnAnalizar =
            new Button();

        private readonly Button btnEquivalencias =
            new Button();

        private readonly Button btnImportar =
            new Button();

        private readonly Button btnCerrar =
            new Button();

        private readonly DataGridView dgvRegistros =
            new DataGridView();

        private readonly Label lblTotal =
            CrearLabelResumen();

        private readonly Label lblNuevos =
            CrearLabelResumen();

        private readonly Label lblActualizar =
            CrearLabelResumen();

        private readonly Label lblSinCambios =
            CrearLabelResumen();

        private readonly Label lblSinPlanta =
            CrearLabelResumen();

        private readonly Label lblSinTurno =
            CrearLabelResumen();

        private readonly Label lblSinLinea =
            CrearLabelResumen();

        private readonly Label lblRevisar =
            CrearLabelResumen();

        private readonly Label lblEstado =
            new Label();

        private bool analizando;

        private ResultadoAnalisisHdc?
            resultadoActual;

        private string?
            filtroEstadoActual;

        private readonly ToolTip toolTipResumen =
            new ToolTip();

        public ImportarHdcVentana()
        {
            ConfigurarVentana();

            ConstruirInterfaz();

            ConfigurarTabla();

            ActualizarResumen(
                null);

            ConfigurarFiltrosResumen();
        }

        private void ConfigurarFiltrosResumen()
        {
            ConfigurarFiltroResumen(
                lblTotal,
                null,
                "Mostrar todos los registros");

            ConfigurarFiltroResumen(
                lblNuevos,
                "Nuevo",
                "Mostrar solo registros nuevos");

            ConfigurarFiltroResumen(
                lblActualizar,
                "Actualizar",
                "Mostrar solo registros que requieren actualización");

            ConfigurarFiltroResumen(
                lblSinCambios,
                "Sin cambios",
                "Mostrar solo registros sin cambios");

            ConfigurarFiltroResumen(
                lblSinPlanta,
                "Sin equivalencia de planta",
                "Mostrar solo registros sin equivalencia de planta");

            ConfigurarFiltroResumen(
                lblSinTurno,
                "Sin equivalencia de turno",
                "Mostrar solo registros sin equivalencia de turno");

            ConfigurarFiltroResumen(
                lblSinLinea,
                "Sin equivalencia de línea",
                "Mostrar solo registros sin equivalencia de línea");

            ConfigurarFiltroResumen(
                lblRevisar,
                "__REVISAR__",
                "Mostrar registros que requieren revisión o están duplicados");
        }

        private void ConfigurarFiltroResumen(
            Label label,
            string? filtro,
            string ayuda)
        {
            label.Cursor =
                Cursors.Hand;

            label.Tag =
                filtro ?? "__TODOS__";

            toolTipResumen.SetToolTip(
                label,
                ayuda);

            label.Click +=
                lblResumen_Click;
        }

        private void lblResumen_Click(
            object? sender,
            EventArgs e)
        {
            if (sender is not Label label ||
                resultadoActual is null)
            {
                return;
            }

            string valor =
                Convert.ToString(
                    label.Tag)
                ?? "__TODOS__";

            filtroEstadoActual =
                valor == "__TODOS__"
                    ? null
                    : valor;

            AplicarFiltroActual();
        }

        private void AplicarFiltroActual()
        {
            if (resultadoActual is null)
            {
                dgvRegistros.DataSource =
                    null;

                return;
            }

            var registros =
                resultadoActual.Registros
                    .AsEnumerable();

            string descripcionFiltro =
                "Todos";

            if (!string.IsNullOrWhiteSpace(
                    filtroEstadoActual))
            {
                if (filtroEstadoActual ==
                    "__REVISAR__")
                {
                    registros =
                        registros.Where(
                            item =>
                                string.Equals(
                                    item.Estado,
                                    "Revisar",
                                    StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(
                                    item.Estado,
                                    "Duplicado",
                                    StringComparison.OrdinalIgnoreCase));

                    descripcionFiltro =
                        "Revisar / Duplicados";
                }
                else
                {
                    registros =
                        registros.Where(
                            item =>
                                string.Equals(
                                    item.Estado,
                                    filtroEstadoActual,
                                    StringComparison.OrdinalIgnoreCase));

                    descripcionFiltro =
                        filtroEstadoActual;
                }
            }

            var lista =
                registros.ToList();

            dgvRegistros.DataSource =
                null;

            dgvRegistros.DataSource =
                lista;

            if (dgvRegistros.Rows.Count > 0)
            {
                dgvRegistros.ClearSelection();

                dgvRegistros.Rows[0]
                    .Selected =
                        true;

                dgvRegistros.CurrentCell =
                    dgvRegistros.Rows[0]
                        .Cells[0];
            }

            lblEstado.Text =
                $"Filtro: {descripcionFiltro}. " +
                $"Mostrando {lista.Count:N0} de " +
                $"{resultadoActual.TotalRegistros:N0} registros.";
        }

        private void ConfigurarVentana()
        {
            FormStyler.ApplyBase(
                this,
                "Importar HDC",
                new Size(
                    1360,
                    800));

            StartPosition =
                FormStartPosition.CenterParent;

            MinimumSize =
                new Size(
                    1160,
                    700);

            DoubleBuffered =
                true;
        }

        private void ConstruirInterfaz()
        {
            Panel pnlCabecera =
                FormStyler.CreateHeader(
                    this,
                    "Importar trabajadores desde HDC",
                    "Analiza la hoja HDC, aplica equivalencias y clasifica los registros antes de importar.");

            Panel pnlContenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        105),
                    new Size(
                        ClientSize.Width - 40,
                        ClientSize.Height - 125));

            pnlContenido.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            Panel pnlArchivo =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            20),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            112),

                    BackColor =
                        AppColors.SectionBackground,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlArchivo,
                10);

            Label lblArchivo =
                new Label
                {
                    Text =
                        "Archivo HDC",

                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            14),

                    ForeColor =
                        AppColors.TextPrimary,

                    Font =
                        AppFonts.Regular(
                            10F,
                            FontStyle.Bold)
                };

            Label lblAyuda =
                new Label
                {
                    Text =
                        "Analiza equivalencias y clasifica cada registro. Solo se importarán los registros preparados cuando confirmes.",

                    AutoSize =
                        true,

                    Location =
                        new Point(
                            18,
                            36),

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            9F)
                };

            Panel pnlRuta =
                new Panel
                {
                    Location =
                        new Point(
                            18,
                            62),

                    Size =
                        new Size(
                            pnlArchivo.ClientSize.Width - 565,
                            40),

                    BackColor =
                        Color.White,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            txtRutaArchivo.Location =
                new Point(
                    12,
                    9);

            txtRutaArchivo.Size =
                new Size(
                    pnlRuta.ClientSize.Width - 24,
                    24);

            txtRutaArchivo.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            txtRutaArchivo.BorderStyle =
                BorderStyle.None;

            txtRutaArchivo.ReadOnly =
                true;

            txtRutaArchivo.BackColor =
                Color.White;

            txtRutaArchivo.ForeColor =
                AppColors.TextPrimary;

            txtRutaArchivo.Font =
                AppFonts.Light(
                    10F);

            txtRutaArchivo.PlaceholderText =
                "Selecciona el archivo HDC.xlsx...";

            InputStyler.ApplyOutlinedInput(
                pnlRuta,
                txtRutaArchivo);

            pnlRuta.Controls.Add(
                txtRutaArchivo);

            ButtonStyler.Apply(
                btnExaminar,
                "Examinar",
                AppColors.Secondary,
                icon: null,
                width: 125,
                height: 40);

            btnExaminar.Location =
                new Point(
                    pnlArchivo.ClientSize.Width - 529,
                    62);

            btnExaminar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnExaminar.Click +=
                btnExaminar_Click;

            ButtonStyler.Apply(
                btnAnalizar,
                "Analizar HDC",
                AppColors.Primary,
                icon: null,
                width: 150,
                height: 40);

            btnAnalizar.Location =
                new Point(
                    pnlArchivo.ClientSize.Width - 392,
                    62);

            btnAnalizar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnAnalizar.Click +=
                btnAnalizar_Click;

            ButtonStyler.Apply(
                btnEquivalencias,
                "Equivalencias",
                AppColors.Secondary,
                icon: null,
                width: 205,
                height: 40);

            btnEquivalencias.Location =
                new Point(
                    pnlArchivo.ClientSize.Width - 224,
                    62);

            btnEquivalencias.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnEquivalencias.Enabled =
                false;

            btnEquivalencias.Click +=
                btnEquivalencias_Click;

            pnlArchivo.Controls.Add(
                lblArchivo);

            pnlArchivo.Controls.Add(
                lblAyuda);

            pnlArchivo.Controls.Add(
                pnlRuta);

            pnlArchivo.Controls.Add(
                btnExaminar);

            pnlArchivo.Controls.Add(
                btnAnalizar);

            pnlArchivo.Controls.Add(
                btnEquivalencias);

            Panel pnlResumen =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            145),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            78),

                    BackColor =
                        AppColors.SectionBackground,

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlResumen,
                10);

            int x =
                18;

            AgregarResumen(
                pnlResumen,
                lblTotal,
                "Registros",
                ref x,
                125);

            AgregarResumen(
                pnlResumen,
                lblNuevos,
                "Nuevos",
                ref x,
                105);

            AgregarResumen(
                pnlResumen,
                lblActualizar,
                "Actualizar",
                ref x,
                115);

            AgregarResumen(
                pnlResumen,
                lblSinCambios,
                "Sin cambios",
                ref x,
                120);

            AgregarResumen(
                pnlResumen,
                lblSinPlanta,
                "Sin planta",
                ref x,
                110);

            AgregarResumen(
                pnlResumen,
                lblSinTurno,
                "Sin turno",
                ref x,
                105);

            AgregarResumen(
                pnlResumen,
                lblSinLinea,
                "Sin línea",
                ref x,
                105);

            AgregarResumen(
                pnlResumen,
                lblRevisar,
                "Revisar",
                ref x,
                100);

            Panel pnlTabla =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            238),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            pnlContenido.ClientSize.Height - 310),

                    BackColor =
                        Color.White,

                    Padding =
                        new Padding(
                            1),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlTabla,
                6);

            dgvRegistros.Dock =
                DockStyle.Fill;

            pnlTabla.Controls.Add(
                dgvRegistros);

            lblEstado.AutoSize =
                true;

            lblEstado.Location =
                new Point(
                    20,
                    pnlContenido.ClientSize.Height - 57);

            lblEstado.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            lblEstado.ForeColor =
                AppColors.TextSecondary;

            lblEstado.Font =
                AppFonts.Light(
                    9F);

            lblEstado.Text =
                "Seleccione un archivo HDC para comenzar.";

            ButtonStyler.Apply(
                btnImportar,
                "Importar preparados",
                AppColors.Primary,
                icon: null,
                width: 190,
                height: 40);

            btnImportar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    140 -
                    190 -
                    32,
                    pnlContenido.ClientSize.Height - 62);

            btnImportar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            btnImportar.Enabled =
                false;

            btnImportar.Click +=
                btnImportar_Click;

            ButtonStyler.Apply(
                btnCerrar,
                "Cerrar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 140,
                height: 40);

            btnCerrar.Location =
                new Point(
                    pnlContenido.ClientSize.Width -
                    btnCerrar.Width -
                    20,
                    pnlContenido.ClientSize.Height - 62);

            btnCerrar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            btnCerrar.Click +=
                (_, _) =>
                {
                    Close();
                };

            pnlContenido.Controls.Add(
                pnlArchivo);

            pnlContenido.Controls.Add(
                pnlResumen);

            pnlContenido.Controls.Add(
                pnlTabla);

            pnlContenido.Controls.Add(
                lblEstado);

            pnlContenido.Controls.Add(
                btnImportar);

            pnlContenido.Controls.Add(
                btnCerrar);

            pnlContenido.BringToFront();

            pnlCabecera.BringToFront();
        }

        private void ConfigurarTabla()
        {
            DataGridViewStyler.ApplyCatalogStyle(
                dgvRegistros);

            dgvRegistros.AutoGenerateColumns =
                false;

            dgvRegistros.AllowUserToAddRows =
                false;

            dgvRegistros.AllowUserToDeleteRows =
                false;

            dgvRegistros.ReadOnly =
                true;

            dgvRegistros.MultiSelect =
                false;

            dgvRegistros.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvRegistros.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgvRegistros.ScrollBars =
                ScrollBars.Both;

            AgregarColumna(
                "Estado",
                "ESTADO",
                170);

            AgregarColumna(
                "NumeroFilaExcel",
                "FILA",
                55);

            AgregarColumna(
                "Empleado",
                "EMPLEADO",
                95);

            AgregarColumna(
                "Nombre",
                "NOMBRE",
                250);

            AgregarColumna(
                "LocalidadHdc",
                "LOCALIDAD HDC",
                105);

            AgregarColumna(
                "PlantaSistema",
                "PLANTA SISTEMA",
                125);

            AgregarColumna(
                "TurnoHdc",
                "TURNO HDC",
                90);

            AgregarColumna(
                "TurnoSistema",
                "TURNO SISTEMA",
                125);

            AgregarColumna(
                "LineaHdc",
                "LÍNEA HDC",
                100);

            AgregarColumna(
                "LineaSistema",
                "LÍNEA SISTEMA",
                135);

            AgregarColumna(
                "CambiosDetectados",
                "CAMBIOS",
                170);

            AgregarColumna(
                "FechaServicio",
                "F. SERVICIO",
                105);

            AgregarColumna(
                "DepartamentoHdc",
                "DEPARTAMENTO",
                120);

            AgregarColumna(
                "PuestoHdc",
                "PUESTO",
                120);

            AgregarColumna(
                "CategoriaHdc",
                "CATEGORÍA",
                90);

            AgregarColumna(
                "PositionHdc",
                "POSITION",
                110);

            AgregarColumna(
                "FunctionHdc",
                "FUNCTION",
                110);

            AgregarColumna(
                "ProcesoHdc",
                "PROCESO",
                110);

            AgregarColumna(
                "DptoHdc",
                "DPTO",
                80);

            AgregarColumna(
                "Observacion",
                "OBSERVACIÓN",
                270);

            dgvRegistros.CellFormatting +=
                dgvRegistros_CellFormatting;
        }

        private void AgregarColumna(
            string propiedad,
            string encabezado,
            int ancho)
        {
            DataGridViewTextBoxColumn columna =
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName =
                        propiedad,

                    Name =
                        propiedad,

                    HeaderText =
                        encabezado,

                    Width =
                        ancho,

                    SortMode =
                        DataGridViewColumnSortMode.Automatic
                };

            dgvRegistros.Columns.Add(
                columna);
        }

        private static Label CrearLabelResumen()
        {
            return new Label
            {
                AutoSize =
                    false,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Regular(
                        12F,
                        FontStyle.Bold)
            };
        }

        private static void AgregarResumen(
            Panel panel,
            Label labelValor,
            string titulo,
            ref int x,
            int ancho)
        {
            Label labelTitulo =
                new Label
                {
                    AutoSize =
                        true,

                    Location =
                        new Point(
                            x,
                            11),

                    Text =
                        titulo,

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            8.5F)
                };

            labelValor.Location =
                new Point(
                    x,
                    31);

            labelValor.Size =
                new Size(
                    ancho - 12,
                    30);

            panel.Controls.Add(
                labelTitulo);

            panel.Controls.Add(
                labelValor);

            x +=
                ancho;
        }

        private void btnExaminar_Click(
            object? sender,
            EventArgs e)
        {
            if (analizando)
            {
                return;
            }

            using OpenFileDialog dialogo =
                new OpenFileDialog
                {
                    Title =
                        "Seleccionar archivo HDC",

                    Filter =
                        "Archivos de Excel (*.xlsx;*.xlsm)|*.xlsx;*.xlsm",

                    CheckFileExists =
                        true,

                    Multiselect =
                        false
                };

            if (dialogo.ShowDialog(this) !=
                DialogResult.OK)
            {
                return;
            }

            txtRutaArchivo.Text =
                dialogo.FileName;

            resultadoActual =
                null;

            filtroEstadoActual =
                null;

            dgvRegistros.DataSource =
                null;

            btnEquivalencias.Enabled =
                false;

            btnImportar.Enabled =
                false;

            ActualizarResumen(
                null);

            lblEstado.Text =
                "Archivo seleccionado. Presione Analizar HDC.";
        }

        private async void btnAnalizar_Click(
            object? sender,
            EventArgs e)
        {
            if (analizando)
            {
                return;
            }

            string ruta =
                txtRutaArchivo.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                ruta))
            {
                AppDialog.ShowWarning(
                    this,
                    "Archivo HDC",
                    "Seleccione primero un archivo de Excel.");

                return;
            }

            analizando =
                true;

            btnExaminar.Enabled =
                false;

            btnAnalizar.Enabled =
                false;

            btnEquivalencias.Enabled =
                false;

            btnImportar.Enabled =
                false;

            btnCerrar.Enabled =
                false;

            Cursor =
                Cursors.WaitCursor;

            lblEstado.Text =
                "Analizando HDC y aplicando equivalencias...";

            try
            {
                ResultadoAnalisisHdc resultado =
                    await Task.Run(
                        () =>
                        {
                            ResultadoAnalisisHdc analisis =
                                LectorHdcExcel
                                    .Analizar(
                                        ruta);

                            AnalizadorHdcSistema
                                .Clasificar(
                                    analisis);

                            return analisis;
                        });

                resultadoActual =
                    resultado;

                MostrarResultado(
                    resultado);

                btnEquivalencias.Enabled =
                    true;
            }
            catch (Exception ex)
            {
                resultadoActual =
                    null;

                filtroEstadoActual =
                    null;

                dgvRegistros.DataSource =
                    null;

                ActualizarResumen(
                    null);

                lblEstado.Text =
                    "No se pudo analizar el archivo.";

                AppDialog.ShowError(
                    this,
                    "No se pudo analizar HDC",
                    ex.Message);
            }
            finally
            {
                analizando =
                    false;

                btnExaminar.Enabled =
                    true;

                btnAnalizar.Enabled =
                    true;

                btnCerrar.Enabled =
                    true;

                if (resultadoActual is not null)
                {
                    btnEquivalencias.Enabled =
                        true;
                }

                ActualizarDisponibilidadImportar();

                Cursor =
                    Cursors.Default;
            }
        }

        private void btnEquivalencias_Click(
            object? sender,
            EventArgs e)
        {
            if (resultadoActual is null)
            {
                return;
            }

            using AdministrarEquivalenciasHdcVentana ventana =
                new AdministrarEquivalenciasHdcVentana(
                    resultadoActual);

            ventana.ShowDialog(
                this);

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                lblEstado.Text =
                    "Actualizando clasificación con las equivalencias...";

                AnalizadorHdcSistema
                    .Clasificar(
                        resultadoActual);

                MostrarResultado(
                    resultadoActual);
            }
            catch (Exception ex)
            {
                AppDialog.ShowError(
                    this,
                    "No se pudo actualizar",
                    ex.Message);
            }
            finally
            {
                Cursor =
                    Cursors.Default;
            }
        }

        private async void btnImportar_Click(
            object? sender,
            EventArgs e)
        {
            if (analizando ||
                resultadoActual is null)
            {
                return;
            }

            int nuevos =
                resultadoActual.Nuevos;

            int actualizados =
                resultadoActual.Actualizados;

            int sinCambios =
                resultadoActual.SinCambios;

            int preparados =
                nuevos +
                actualizados +
                sinCambios;

            int pendientes =
                resultadoActual.SinEquivalenciaPlanta +
                resultadoActual.SinEquivalenciaTurno +
                resultadoActual.SinEquivalenciaLinea +
                resultadoActual.RegistrosRevisar +
                resultadoActual.RegistrosDuplicados;

            if (preparados <= 0)
            {
                AppDialog.ShowInfo(
                    this,
                    "Sin registros preparados",
                    "No hay registros listos para importar.");

                return;
            }

            ProveedorBaseDatos proveedorActual =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            string mensajeSeguridad =
                proveedorActual ==
                ProveedorBaseDatos.SQLite
                    ? "Antes de modificar SQLite se creará un respaldo automático."
                    : "SQL Server / Azure SQL se modificará dentro de una sola transacción. " +
                      "La aplicación no genera un archivo .bak automáticamente; " +
                      "confirme que cuenta con un respaldo o punto de restauración válido.";

            bool confirmar =
                AppDialog.Confirm(
                    this,
                    "Confirmar importación HDC",
                    "Se realizará una importación transaccional con los siguientes registros:" +
                    Environment.NewLine +
                    Environment.NewLine +
                    $"Nuevos: {nuevos:N0}" +
                    Environment.NewLine +
                    $"Actualizar: {actualizados:N0}" +
                    Environment.NewLine +
                    $"Sin cambios: {sinCambios:N0}" +
                    Environment.NewLine +
                    Environment.NewLine +
                    $"Pendientes excluidos: {pendientes:N0}" +
                    Environment.NewLine +
                    $"Ignorados: {resultadoActual.Ignorados:N0}" +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Los registros pendientes o ignorados NO se modificarán." +
                    Environment.NewLine +
                    "Las fotografías, expedientes y certificaciones existentes se conservarán." +
                    Environment.NewLine +
                    Environment.NewLine +
                    mensajeSeguridad +
                    Environment.NewLine +
                    Environment.NewLine +
                    "¿Desea continuar?",
                    "Importar");

            if (!confirmar)
            {
                return;
            }

            string ruta =
                txtRutaArchivo.Text.Trim();

            ResultadoImportacionHdc? importacion =
                null;

            analizando =
                true;

            btnExaminar.Enabled =
                false;

            btnAnalizar.Enabled =
                false;

            btnEquivalencias.Enabled =
                false;

            btnImportar.Enabled =
                false;

            btnCerrar.Enabled =
                false;

            Cursor =
                Cursors.WaitCursor;

            lblEstado.Text =
                "Importando trabajadores HDC. No cierre la aplicación...";

            try
            {
                ResultadoAnalisisHdc resultadoAImportar =
                    resultadoActual;

                importacion =
                    await Task.Run(
                        () =>
                            HdcImportacion.Importar(
                                ruta,
                                resultadoAImportar));

                lblEstado.Text =
                    "Importación completada. Actualizando la previsualización...";

                try
                {
                    ResultadoAnalisisHdc resultadoNuevo =
                        await Task.Run(
                            () =>
                            {
                                ResultadoAnalisisHdc analisis =
                                    LectorHdcExcel
                                        .Analizar(
                                            ruta);

                                AnalizadorHdcSistema
                                    .Clasificar(
                                        analisis);

                                return analisis;
                            });

                    resultadoActual =
                        resultadoNuevo;

                    filtroEstadoActual =
                        null;

                    MostrarResultado(
                        resultadoNuevo);
                }
                catch (Exception exActualizacion)
                {
                    lblEstado.Text =
                        "La importación se completó, pero no se pudo refrescar la previsualización.";

                    AppDialog.ShowWarning(
                        this,
                        "Importación completada",
                        "Los datos se guardaron correctamente, pero falló la actualización " +
                        "de la previsualización." +
                        Environment.NewLine +
                        Environment.NewLine +
                        exActualizacion.Message);
                }

                string detalleRespaldo =
                    string.IsNullOrWhiteSpace(
                        importacion.RutaRespaldo)
                        ? "Respaldo previo: administrado externamente por SQL Server / Azure SQL."
                        : "Respaldo previo:" +
                          Environment.NewLine +
                          importacion.RutaRespaldo;

                AppDialog.ShowInfo(
                    this,
                    "Importación HDC completada",
                    $"Importación #{importacion.IdImportacion:N0} completada correctamente." +
                    Environment.NewLine +
                    Environment.NewLine +
                    $"Nuevos insertados: {importacion.NuevosInsertados:N0}" +
                    Environment.NewLine +
                    $"Actualizados: {importacion.Actualizados:N0}" +
                    Environment.NewLine +
                    $"Sin cambios: {importacion.SinCambios:N0}" +
                    Environment.NewLine +
                    $"Excluidos: {importacion.Excluidos:N0}" +
                    Environment.NewLine +
                    $"Ignorados: {importacion.Ignorados:N0}" +
                    Environment.NewLine +
                    Environment.NewLine +
                    detalleRespaldo);
            }
            catch (Exception ex)
            {
                lblEstado.Text =
                    "No se realizó la importación. La transacción fue revertida.";

                AppDialog.ShowError(
                    this,
                    "No se pudo importar HDC",
                    ex.Message +
                    Environment.NewLine +
                    Environment.NewLine +
                    "No se conservaron cambios parciales en la base de datos.");
            }
            finally
            {
                analizando =
                    false;

                btnExaminar.Enabled =
                    true;

                btnAnalizar.Enabled =
                    true;

                btnCerrar.Enabled =
                    true;

                btnEquivalencias.Enabled =
                    resultadoActual is not null;

                ActualizarDisponibilidadImportar();

                Cursor =
                    Cursors.Default;
            }
        }

        private void ActualizarDisponibilidadImportar()
        {
            if (analizando ||
                resultadoActual is null)
            {
                btnImportar.Enabled =
                    false;

                return;
            }

            int preparados =
                resultadoActual.Nuevos +
                resultadoActual.Actualizados +
                resultadoActual.SinCambios;

            btnImportar.Enabled =
                preparados > 0;
        }

        private void MostrarResultado(
            ResultadoAnalisisHdc resultado)
        {
            ActualizarResumen(
                resultado);

            ActualizarDisponibilidadImportar();

            int pendientes =
                resultado.SinEquivalenciaPlanta +
                resultado.SinEquivalenciaTurno +
                resultado.SinEquivalenciaLinea +
                resultado.RegistrosRevisar +
                resultado.RegistrosDuplicados;

            int preparados =
                resultado.Nuevos +
                resultado.Actualizados +
                resultado.SinCambios;

            if (string.IsNullOrWhiteSpace(
                    filtroEstadoActual))
            {
                dgvRegistros.DataSource =
                    null;

                dgvRegistros.DataSource =
                    resultado.Registros;

                lblEstado.Text =
                    $"Análisis completado. Hoja: {resultado.NombreHoja}. " +
                    $"Preparados: {preparados:N0}. " +
                    $"Pendientes: {pendientes:N0}. " +
                    $"Ignorados: {resultado.Ignorados:N0}. " +
                    "No se realizó ningún cambio en Trabajador.";

                if (dgvRegistros.Rows.Count > 0)
                {
                    dgvRegistros.ClearSelection();

                    dgvRegistros.Rows[0]
                        .Selected =
                            true;
                }

                return;
            }

            AplicarFiltroActual();
        }

        private void ActualizarResumen(
            ResultadoAnalisisHdc? resultado)
        {
            if (resultado is null)
            {
                lblTotal.Text = "0";
                lblNuevos.Text = "0";
                lblActualizar.Text = "0";
                lblSinCambios.Text = "0";
                lblSinPlanta.Text = "0";
                lblSinTurno.Text = "0";
                lblSinLinea.Text = "0";
                lblRevisar.Text = "0";
                return;
            }

            lblTotal.Text =
                resultado.TotalRegistros
                    .ToString("N0");

            lblNuevos.Text =
                resultado.Nuevos
                    .ToString("N0");

            lblActualizar.Text =
                resultado.Actualizados
                    .ToString("N0");

            lblSinCambios.Text =
                resultado.SinCambios
                    .ToString("N0");

            lblSinPlanta.Text =
                resultado.SinEquivalenciaPlanta
                    .ToString("N0");

            lblSinTurno.Text =
                resultado.SinEquivalenciaTurno
                    .ToString("N0");

            lblSinLinea.Text =
                resultado.SinEquivalenciaLinea
                    .ToString("N0");

            lblRevisar.Text =
                (
                    resultado.RegistrosRevisar +
                    resultado.RegistrosDuplicados
                ).ToString("N0");
        }

        private void dgvRegistros_CellFormatting(
            object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvRegistros.Columns[e.ColumnIndex]
                    .Name != "Estado")
            {
                return;
            }

            string estado =
                Convert.ToString(
                    e.Value)
                ?? string.Empty;

            e.CellStyle.Font =
                AppFonts.Regular(
                    9F,
                    FontStyle.Bold);

            if (estado == "Nuevo")
            {
                e.CellStyle.ForeColor =
                    Color.FromArgb(
                        0,
                        110,
                        45);
            }
            else if (estado == "Actualizar")
            {
                e.CellStyle.ForeColor =
                    Color.DarkOrange;
            }
            else if (estado == "Sin cambios")
            {
                e.CellStyle.ForeColor =
                    AppColors.Primary;
            }
            else if (estado == "Ignorar")
            {
                e.CellStyle.ForeColor =
                    AppColors.TextSecondary;
            }
            else if (estado.StartsWith(
                         "Sin equivalencia",
                         StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor =
                    Color.DarkOrange;
            }
            else
            {
                e.CellStyle.ForeColor =
                    AppColors.Danger;
            }
        }
    }
}
