using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public sealed class AdministrarEquivalenciasHdcVentana
        : Form
    {
        private readonly ResultadoAnalisisHdc resultado;

        private readonly ComboBox cboLocalidadHdc =
            CrearCombo();

        private readonly ComboBox cboPlantaSistema =
            CrearCombo();

        private readonly ComboBox cboTurnoHdc =
            CrearCombo();

        private readonly ComboBox cboTurnoSistema =
            CrearCombo();

        private readonly ComboBox cboLocalidadLineaHdc =
            CrearCombo();

        private readonly ComboBox cboLineaHdc =
            CrearCombo();

        private readonly ComboBox cboAccionLinea =
            CrearCombo();

        private readonly ComboBox cboLineaSistema =
            CrearCombo();

        private readonly Label lblPlantaLinea =
            new Label();

        private readonly DataGridView dgvPlantas =
            CrearGrid();

        private readonly DataGridView dgvTurnos =
            CrearGrid();

        private readonly DataGridView dgvLineas =
            CrearGrid();

        private readonly DataGridView dgvDiagnosticoLocalidades =
            CrearGrid();

        private readonly DataGridView dgvDetalleLocalidad =
            CrearGrid();

        private readonly Label lblDiagnosticoSeleccion =
            new Label();

        private readonly Label lblDiagnosticoResumen =
            new Label();

        private readonly Label lblDiagnosticoTurnos =
            new Label();

        private readonly Label lblDiagnosticoLineas =
            new Label();

        private readonly Label lblDiagnosticoDepartamentos =
            new Label();

        private readonly Button btnUsarLocalidadDiagnostico =
            new Button();

        private readonly ComboBox cboDiagnosticoTurnoHdc =
            CrearCombo();

        private readonly Label lblDiagnosticoCruceTurno =
            new Label();

        private readonly DataGridView dgvDiagnosticoTurnosGlobal =
            CrearGrid();

        private readonly Label lblDiagnosticoTurnosGlobalResumen =
            new Label();

        private readonly Button btnUsarTurnoGlobal =
            new Button();

        private Dictionary<string, DataRow>?
            trabajadoresDiagnosticoIndice;

        public AdministrarEquivalenciasHdcVentana(
            ResultadoAnalisisHdc resultado)
        {
            this.resultado =
                resultado;

            ConfigurarVentana();

            ConstruirInterfaz();

            CargarOrigenes();

            CargarCatalogos();

            CargarEquivalencias();
        }

        private void ConfigurarVentana()
        {
            FormStyler.ApplyBase(
                this,
                "Equivalencias HDC",
                new Size(
                    1120,
                    720));

            StartPosition =
                FormStartPosition.CenterParent;

            MinimumSize =
                new Size(
                    1000,
                    650);

            DoubleBuffered =
                true;
        }

        private void ConstruirInterfaz()
        {
            Panel cabecera =
                FormStyler.CreateHeader(
                    this,
                    "Administrar equivalencias HDC",
                    "Relaciona los códigos externos del HDC con los catálogos internos del sistema.");

            Panel contenido =
                FormStyler.CreateCard(
                    this,
                    new Point(
                        20,
                        105),
                    new Size(
                        ClientSize.Width - 40,
                        ClientSize.Height - 125));

            contenido.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            TabControl tabs =
                new TabControl
                {
                    Location =
                        new Point(
                            18,
                            18),

                    Size =
                        new Size(
                            contenido.ClientSize.Width - 36,
                            contenido.ClientSize.Height - 88),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right,

                    Font =
                        AppFonts.Regular(
                            10F)
                };

            TabPage tabPlantas =
                new TabPage(
                    "Plantas");

            TabPage tabTurnos =
                new TabPage(
                    "Turnos");

            TabPage tabLineas =
                new TabPage(
                    "Líneas");

            TabPage tabDiagnostico =
                new TabPage(
                    "Diagnóstico");

            TabPage tabTurnosGlobal =
                new TabPage(
                    "Turnos global");

            ConstruirTabPlantas(
                tabPlantas);

            ConstruirTabTurnos(
                tabTurnos);

            ConstruirTabLineas(
                tabLineas);

            ConstruirTabDiagnostico(
                tabDiagnostico);

            ConstruirTabDiagnosticoTurnosGlobal(
                tabTurnosGlobal);

            tabs.TabPages.Add(
                tabPlantas);

            tabs.TabPages.Add(
                tabTurnos);

            tabs.TabPages.Add(
                tabLineas);

            tabs.TabPages.Add(
                tabDiagnostico);

            tabs.TabPages.Add(
                tabTurnosGlobal);

            Button btnCerrar =
                new Button();

            ButtonStyler.Apply(
                btnCerrar,
                "Cerrar",
                AppColors.Neutral,
                AppIcons.Back,
                width: 140,
                height: 40);

            btnCerrar.Location =
                new Point(
                    contenido.ClientSize.Width -
                    btnCerrar.Width -
                    18,
                    contenido.ClientSize.Height - 55);

            btnCerrar.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            btnCerrar.Click +=
                (_, _) =>
                {
                    DialogResult =
                        DialogResult.OK;

                    Close();
                };

            contenido.Controls.Add(
                tabs);

            contenido.Controls.Add(
                btnCerrar);

            contenido.BringToFront();

            cabecera.BringToFront();
        }

        private void ConstruirTabPlantas(
            TabPage tab)
        {
            Label titulo =
                CrearTitulo(
                    "Localidad HDC → Planta del sistema",
                    20,
                    20);

            Label lblOrigen =
                CrearEtiqueta(
                    "Localidad HDC",
                    20,
                    62);

            cboLocalidadHdc.Location =
                new Point(
                    20,
                    85);

            cboLocalidadHdc.Size =
                new Size(
                    220,
                    32);

            Label lblDestino =
                CrearEtiqueta(
                    "Planta del sistema",
                    260,
                    62);

            cboPlantaSistema.Location =
                new Point(
                    260,
                    85);

            cboPlantaSistema.Size =
                new Size(
                    250,
                    32);

            Button btnGuardar =
                new Button();

            ButtonStyler.Apply(
                btnGuardar,
                "Guardar equivalencia",
                AppColors.Primary,
                icon: null,
                width: 190,
                height: 38);

            btnGuardar.Location =
                new Point(
                    535,
                    81);

            btnGuardar.Click +=
                (_, _) =>
                    GuardarPlanta();

            dgvPlantas.Location =
                new Point(
                    20,
                    145);

            dgvPlantas.Size =
                new Size(
                    tab.ClientSize.Width - 40,
                    tab.ClientSize.Height - 165);

            dgvPlantas.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            tab.Controls.Add(
                titulo);

            tab.Controls.Add(
                lblOrigen);

            tab.Controls.Add(
                cboLocalidadHdc);

            tab.Controls.Add(
                lblDestino);

            tab.Controls.Add(
                cboPlantaSistema);

            tab.Controls.Add(
                btnGuardar);

            tab.Controls.Add(
                dgvPlantas);
        }

        private void ConstruirTabTurnos(
            TabPage tab)
        {
            Label titulo =
                CrearTitulo(
                    "Turno HDC → Turno del sistema",
                    20,
                    20);

            Label lblOrigen =
                CrearEtiqueta(
                    "Turno HDC",
                    20,
                    62);

            cboTurnoHdc.Location =
                new Point(
                    20,
                    85);

            cboTurnoHdc.Size =
                new Size(
                    220,
                    32);

            Label lblDestino =
                CrearEtiqueta(
                    "Turno del sistema",
                    260,
                    62);

            cboTurnoSistema.Location =
                new Point(
                    260,
                    85);

            cboTurnoSistema.Size =
                new Size(
                    250,
                    32);

            Button btnGuardar =
                new Button();

            ButtonStyler.Apply(
                btnGuardar,
                "Guardar equivalencia",
                AppColors.Primary,
                icon: null,
                width: 190,
                height: 38);

            btnGuardar.Location =
                new Point(
                    535,
                    81);

            btnGuardar.Click +=
                (_, _) =>
                    GuardarTurno();

            dgvTurnos.Location =
                new Point(
                    20,
                    145);

            dgvTurnos.Size =
                new Size(
                    tab.ClientSize.Width - 40,
                    tab.ClientSize.Height - 165);

            dgvTurnos.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            tab.Controls.Add(
                titulo);

            tab.Controls.Add(
                lblOrigen);

            tab.Controls.Add(
                cboTurnoHdc);

            tab.Controls.Add(
                lblDestino);

            tab.Controls.Add(
                cboTurnoSistema);

            tab.Controls.Add(
                btnGuardar);

            tab.Controls.Add(
                dgvTurnos);
        }

        private void ConstruirTabLineas(
            TabPage tab)
        {
            Label titulo =
                CrearTitulo(
                    "Línea HDC → Línea del sistema",
                    20,
                    20);

            Label lblLocalidad =
                CrearEtiqueta(
                    "Localidad HDC",
                    20,
                    60);

            cboLocalidadLineaHdc.Location =
                new Point(
                    20,
                    83);

            cboLocalidadLineaHdc.Size =
                new Size(
                    170,
                    32);

            cboLocalidadLineaHdc.SelectedIndexChanged +=
                (_, _) =>
                    LocalidadLineaCambio();

            Label lblLinea =
                CrearEtiqueta(
                    "Línea HDC",
                    205,
                    60);

            cboLineaHdc.Location =
                new Point(
                    205,
                    83);

            cboLineaHdc.Size =
                new Size(
                    170,
                    32);

            Label lblAccion =
                CrearEtiqueta(
                    "Acción",
                    390,
                    60);

            cboAccionLinea.Location =
                new Point(
                    390,
                    83);

            cboAccionLinea.Size =
                new Size(
                    155,
                    32);

            cboAccionLinea.Items.AddRange(
                new object[]
                {
                    "MAPEAR",
                    "SIN_ASIGNAR",
                    "IGNORAR"
                });

            cboAccionLinea.SelectedIndex =
                0;

            cboAccionLinea.SelectedIndexChanged +=
                (_, _) =>
                    ActualizarEstadoLineaSistema();

            Label lblDestino =
                CrearEtiqueta(
                    "Línea del sistema",
                    560,
                    60);

            cboLineaSistema.Location =
                new Point(
                    560,
                    83);

            cboLineaSistema.Size =
                new Size(
                    210,
                    32);

            lblPlantaLinea.AutoSize =
                true;

            lblPlantaLinea.Location =
                new Point(
                    20,
                    122);

            lblPlantaLinea.ForeColor =
                AppColors.TextSecondary;

            lblPlantaLinea.Font =
                AppFonts.Light(
                    9F);

            Button btnGuardar =
                new Button();

            ButtonStyler.Apply(
                btnGuardar,
                "Guardar equivalencia",
                AppColors.Primary,
                icon: null,
                width: 190,
                height: 38);

            btnGuardar.Location =
                new Point(
                    790,
                    79);

            btnGuardar.Click +=
                (_, _) =>
                    GuardarLinea();

            Button btnMarcarPendientes =
                new Button();

            ButtonStyler.Apply(
                btnMarcarPendientes,
                "Pendientes → SIN ASIGNAR",
                AppColors.Secondary,
                icon: null,
                width: 260,
                height: 34);

            btnMarcarPendientes.Location =
                new Point(
                    720,
                    122);

            btnMarcarPendientes.Click +=
                (_, _) =>
                    MarcarLineasPendientesSinAsignar();

            dgvLineas.Location =
                new Point(
                    20,
                    170);

            dgvLineas.Size =
                new Size(
                    tab.ClientSize.Width - 40,
                    tab.ClientSize.Height - 190);

            dgvLineas.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            tab.Controls.Add(
                titulo);

            tab.Controls.Add(
                lblLocalidad);

            tab.Controls.Add(
                cboLocalidadLineaHdc);

            tab.Controls.Add(
                lblLinea);

            tab.Controls.Add(
                cboLineaHdc);

            tab.Controls.Add(
                lblAccion);

            tab.Controls.Add(
                cboAccionLinea);

            tab.Controls.Add(
                lblDestino);

            tab.Controls.Add(
                cboLineaSistema);

            tab.Controls.Add(
                lblPlantaLinea);

            tab.Controls.Add(
                btnGuardar);

            tab.Controls.Add(
                btnMarcarPendientes);

            tab.Controls.Add(
                dgvLineas);
        }

        private void ConstruirTabDiagnostico(
            TabPage tab)
        {
            Label titulo =
                CrearTitulo(
                    "Diagnóstico de localidades HDC",
                    20,
                    18);

            Label ayuda =
                new Label
                {
                    Text =
                        "Ordena las localidades por cantidad de trabajadores y revisa " +
                        "turnos, líneas, departamentos y ejemplos antes de crear una equivalencia.",

                    AutoSize =
                        false,

                    Location =
                        new Point(
                            20,
                            47),

                    Size =
                        new Size(
                            Math.Max(
                                700,
                                tab.ClientSize.Width - 40),
                            36),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right,

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            9F)
                };

            dgvDiagnosticoLocalidades.Location =
                new Point(
                    20,
                    88);

            dgvDiagnosticoLocalidades.Size =
                new Size(
                    485,
                    Math.Max(
                        320,
                        tab.ClientSize.Height - 108));

            dgvDiagnosticoLocalidades.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            dgvDiagnosticoLocalidades.AutoGenerateColumns =
                false;

            dgvDiagnosticoLocalidades.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgvDiagnosticoLocalidades.ScrollBars =
                ScrollBars.Both;

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "LocalidadHdc",
                "LOCALIDAD HDC",
                105);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "Registros",
                "REGISTROS",
                78);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "PendientesPlanta",
                "SIN PLANTA",
                82);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "Turnos",
                "TURNOS",
                65);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "Lineas",
                "LÍNEAS",
                65);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "Departamentos",
                "DEPTOS.",
                72);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "Equivalencia",
                "EQUIVALENCIA",
                120);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoLocalidades,
                "EstadoEquivalencia",
                "ESTADO",
                90);

            dgvDiagnosticoLocalidades.SelectionChanged +=
                (_, _) =>
                    MostrarDetalleLocalidadSeleccionada();

            dgvDiagnosticoLocalidades.CellDoubleClick +=
                (_, e) =>
                {
                    if (e.RowIndex >= 0)
                    {
                        UsarLocalidadDiagnosticoEnPlantas();
                    }
                };

            Panel panelDetalle =
                new Panel
                {
                    Location =
                        new Point(
                            520,
                            88),

                    Size =
                        new Size(
                            Math.Max(
                                420,
                                tab.ClientSize.Width - 540),
                            Math.Max(
                                320,
                                tab.ClientSize.Height - 108)),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Bottom |
                        AnchorStyles.Left |
                        AnchorStyles.Right,

                    BackColor =
                        Color.White
                };

            lblDiagnosticoSeleccion.AutoSize =
                false;

            lblDiagnosticoSeleccion.Location =
                new Point(
                    0,
                    0);

            lblDiagnosticoSeleccion.Size =
                new Size(
                    panelDetalle.ClientSize.Width - 200,
                    28);

            lblDiagnosticoSeleccion.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            lblDiagnosticoSeleccion.ForeColor =
                AppColors.TextPrimary;

            lblDiagnosticoSeleccion.Font =
                AppFonts.Regular(
                    12F,
                    FontStyle.Bold);

            ButtonStyler.Apply(
                btnUsarLocalidadDiagnostico,
                "Usar en Plantas",
                AppColors.Secondary,
                icon: null,
                width: 175,
                height: 34);

            btnUsarLocalidadDiagnostico.Location =
                new Point(
                    panelDetalle.ClientSize.Width -
                    btnUsarLocalidadDiagnostico.Width,
                    0);

            btnUsarLocalidadDiagnostico.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnUsarLocalidadDiagnostico.Click +=
                (_, _) =>
                    UsarLocalidadDiagnosticoEnPlantas();

            ConfigurarLabelDiagnostico(
                lblDiagnosticoResumen,
                panelDetalle,
                35);

            ConfigurarLabelDiagnostico(
                lblDiagnosticoTurnos,
                panelDetalle,
                58);

            ConfigurarLabelDiagnostico(
                lblDiagnosticoLineas,
                panelDetalle,
                81);

            ConfigurarLabelDiagnostico(
                lblDiagnosticoDepartamentos,
                panelDetalle,
                104);

            Label lblFiltroTurnoDiagnostico =
                CrearEtiqueta(
                    "Cruzar turno HDC con asignación actual",
                    0,
                    132);

            cboDiagnosticoTurnoHdc.Location =
                new Point(
                    0,
                    153);

            cboDiagnosticoTurnoHdc.Size =
                new Size(
                    150,
                    30);

            cboDiagnosticoTurnoHdc.SelectedIndexChanged +=
                (_, _) =>
                    ActualizarCruceTurnoSeleccionado();

            lblDiagnosticoCruceTurno.AutoSize =
                false;

            lblDiagnosticoCruceTurno.Location =
                new Point(
                    165,
                    146);

            lblDiagnosticoCruceTurno.Size =
                new Size(
                    Math.Max(
                        250,
                        panelDetalle.ClientSize.Width - 165),
                    58);

            lblDiagnosticoCruceTurno.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            lblDiagnosticoCruceTurno.ForeColor =
                AppColors.TextSecondary;

            lblDiagnosticoCruceTurno.Font =
                AppFonts.Light(
                    8.7F);

            Label lblEjemplos =
                CrearEtiqueta(
                    "Trabajadores del turno seleccionado y asignación actual",
                    0,
                    211);

            dgvDetalleLocalidad.Location =
                new Point(
                    0,
                    235);

            dgvDetalleLocalidad.Size =
                new Size(
                    panelDetalle.ClientSize.Width,
                    panelDetalle.ClientSize.Height - 235);

            dgvDetalleLocalidad.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgvDetalleLocalidad.AutoGenerateColumns =
                false;

            dgvDetalleLocalidad.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgvDetalleLocalidad.ScrollBars =
                ScrollBars.Both;

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "Empleado",
                "EMPLEADO",
                85);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "Nombre",
                "NOMBRE",
                210);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "TurnoHdc",
                "TURNO HDC",
                78);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "TurnoSistemaActual",
                "TURNO ACTUAL",
                100);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "PlantaSistemaActual",
                "PLANTA ACTUAL",
                105);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "LineaHdc",
                "LÍNEA HDC",
                85);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "DepartamentoHdc",
                "DEPARTAMENTO",
                105);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "PuestoHdc",
                "PUESTO",
                95);

            AgregarColumnaDiagnostico(
                dgvDetalleLocalidad,
                "ProcesoHdc",
                "PROCESO",
                90);

            panelDetalle.Controls.Add(
                lblDiagnosticoSeleccion);

            panelDetalle.Controls.Add(
                btnUsarLocalidadDiagnostico);

            panelDetalle.Controls.Add(
                lblDiagnosticoResumen);

            panelDetalle.Controls.Add(
                lblDiagnosticoTurnos);

            panelDetalle.Controls.Add(
                lblDiagnosticoLineas);

            panelDetalle.Controls.Add(
                lblDiagnosticoDepartamentos);

            panelDetalle.Controls.Add(
                lblFiltroTurnoDiagnostico);

            panelDetalle.Controls.Add(
                cboDiagnosticoTurnoHdc);

            panelDetalle.Controls.Add(
                lblDiagnosticoCruceTurno);

            panelDetalle.Controls.Add(
                lblEjemplos);

            panelDetalle.Controls.Add(
                dgvDetalleLocalidad);

            tab.Controls.Add(
                titulo);

            tab.Controls.Add(
                ayuda);

            tab.Controls.Add(
                dgvDiagnosticoLocalidades);

            tab.Controls.Add(
                panelDetalle);
        }

        private void ConstruirTabDiagnosticoTurnosGlobal(
            TabPage tab)
        {
            Label titulo =
                CrearTitulo(
                    "Diagnóstico global de turnos HDC",
                    20,
                    18);

            Label ayuda =
                new Label
                {
                    Text =
                        "Compara automáticamente todas las combinaciones Localidad HDC + Turno HDC " +
                        "contra la asignación actual de los trabajadores existentes.",

                    AutoSize =
                        false,

                    Location =
                        new Point(
                            20,
                            47),

                    Size =
                        new Size(
                            Math.Max(
                                700,
                                tab.ClientSize.Width - 40),
                            36),

                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Left |
                        AnchorStyles.Right,

                    ForeColor =
                        AppColors.TextSecondary,

                    Font =
                        AppFonts.Light(
                            9F)
                };

            lblDiagnosticoTurnosGlobalResumen.AutoSize =
                false;

            lblDiagnosticoTurnosGlobalResumen.Location =
                new Point(
                    20,
                    82);

            lblDiagnosticoTurnosGlobalResumen.Size =
                new Size(
                    Math.Max(
                        600,
                        tab.ClientSize.Width - 240),
                    42);

            lblDiagnosticoTurnosGlobalResumen.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            lblDiagnosticoTurnosGlobalResumen.ForeColor =
                AppColors.TextSecondary;

            lblDiagnosticoTurnosGlobalResumen.Font =
                AppFonts.Light(
                    9F);

            ButtonStyler.Apply(
                btnUsarTurnoGlobal,
                "Usar en Turnos",
                AppColors.Secondary,
                icon: null,
                width: 170,
                height: 34);

            btnUsarTurnoGlobal.Location =
                new Point(
                    tab.ClientSize.Width -
                    btnUsarTurnoGlobal.Width -
                    20,
                    82);

            btnUsarTurnoGlobal.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnUsarTurnoGlobal.Click +=
                (_, _) =>
                    UsarTurnoGlobalEnEquivalencias();

            dgvDiagnosticoTurnosGlobal.Location =
                new Point(
                    20,
                    132);

            dgvDiagnosticoTurnosGlobal.Size =
                new Size(
                    tab.ClientSize.Width - 40,
                    tab.ClientSize.Height - 152);

            dgvDiagnosticoTurnosGlobal.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgvDiagnosticoTurnosGlobal.AutoGenerateColumns =
                false;

            dgvDiagnosticoTurnosGlobal.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            dgvDiagnosticoTurnosGlobal.ScrollBars =
                ScrollBars.Both;

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "LocalidadHdc",
                "LOCALIDAD HDC",
                110);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "TurnoHdc",
                "TURNO HDC",
                90);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "Registros",
                "REGISTROS",
                80);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "Existentes",
                "EXISTEN",
                75);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "NoExistentes",
                "NO EXISTEN",
                90);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "CoincidenciaPrincipal",
                "COINCIDENCIA PRINCIPAL",
                190);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "TurnosActuales",
                "TURNOS ACTUALES",
                220);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "PlantasActuales",
                "PLANTAS ACTUALES",
                180);

            AgregarColumnaDiagnostico(
                dgvDiagnosticoTurnosGlobal,
                "EquivalenciaActual",
                "EQUIVALENCIA",
                115);

            dgvDiagnosticoTurnosGlobal.SelectionChanged +=
                (_, _) =>
                {
                    btnUsarTurnoGlobal.Enabled =
                        dgvDiagnosticoTurnosGlobal.CurrentRow is not null;
                };

            dgvDiagnosticoTurnosGlobal.CellDoubleClick +=
                (_, e) =>
                {
                    if (e.RowIndex >= 0)
                    {
                        UsarTurnoGlobalEnEquivalencias();
                    }
                };

            tab.Controls.Add(
                titulo);

            tab.Controls.Add(
                ayuda);

            tab.Controls.Add(
                lblDiagnosticoTurnosGlobalResumen);

            tab.Controls.Add(
                btnUsarTurnoGlobal);

            tab.Controls.Add(
                dgvDiagnosticoTurnosGlobal);
        }

        private static void ConfigurarLabelDiagnostico(
            Label label,
            Control contenedor,
            int y)
        {
            label.AutoSize =
                false;

            label.Location =
                new Point(
                    0,
                    y);

            label.Size =
                new Size(
                    contenedor.ClientSize.Width,
                    21);

            label.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            label.AutoEllipsis =
                true;

            label.ForeColor =
                AppColors.TextSecondary;

            label.Font =
                AppFonts.Light(
                    9F);
        }

        private static void AgregarColumnaDiagnostico(
            DataGridView grid,
            string propiedad,
            string encabezado,
            int ancho)
        {
            grid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name =
                        propiedad,

                    DataPropertyName =
                        propiedad,

                    HeaderText =
                        encabezado,

                    Width =
                        ancho,

                    SortMode =
                        DataGridViewColumnSortMode.Automatic
                });
        }

        private void CargarDiagnosticoLocalidades()
        {
            DataTable equivalencias =
                HdcEquivalencia
                    .ConsultarPlantas();

            var mapaEquivalencias =
                equivalencias.Rows
                    .Cast<DataRow>()
                    .Where(
                        fila =>
                            !string.IsNullOrWhiteSpace(
                                Convert.ToString(
                                    fila["CodigoLocalidadHdc"])))
                    .GroupBy(
                        fila =>
                            Normalizar(
                                Convert.ToString(
                                    fila["CodigoLocalidadHdc"])),
                        StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(
                        grupo =>
                            grupo.Key,
                        grupo =>
                            Convert.ToString(
                                grupo.First()["NombrePlanta"])
                            ?.Trim()
                            ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase);

            DataTable tabla =
                CrearTablaDiagnosticoLocalidades();

            var grupos =
                resultado.Registros
                    .Where(
                        item =>
                            !string.IsNullOrWhiteSpace(
                                item.LocalidadHdc))
                    .GroupBy(
                        item =>
                            item.LocalidadHdc.Trim(),
                        StringComparer.OrdinalIgnoreCase)
                    .OrderByDescending(
                        grupo =>
                            grupo.Count())
                    .ThenBy(
                        grupo =>
                            grupo.Key,
                        StringComparer.OrdinalIgnoreCase);

            foreach (var grupo in grupos)
            {
                string clave =
                    Normalizar(
                        grupo.Key);

                bool configurada =
                    mapaEquivalencias.TryGetValue(
                        clave,
                        out string? nombrePlanta);

                DataRow fila =
                    tabla.NewRow();

                fila["LocalidadHdc"] =
                    grupo.Key;

                fila["Registros"] =
                    grupo.Count();

                fila["PendientesPlanta"] =
                    grupo.Count(
                        item =>
                            string.Equals(
                                item.Estado,
                                "Sin equivalencia de planta",
                                StringComparison.OrdinalIgnoreCase));

                fila["Turnos"] =
                    ContarDistintos(
                        grupo.Select(
                            item =>
                                item.TurnoHdc));

                fila["Lineas"] =
                    ContarDistintos(
                        grupo.Select(
                            item =>
                                item.LineaHdc));

                fila["Departamentos"] =
                    ContarDistintos(
                        grupo.Select(
                            item =>
                                item.DepartamentoHdc));

                fila["Equivalencia"] =
                    configurada
                        ? nombrePlanta ?? string.Empty
                        : string.Empty;

                fila["EstadoEquivalencia"] =
                    configurada
                        ? "Configurada"
                        : "Pendiente";

                tabla.Rows.Add(
                    fila);
            }

            dgvDiagnosticoLocalidades.DataSource =
                tabla;

            if (dgvDiagnosticoLocalidades.Rows.Count > 0)
            {
                dgvDiagnosticoLocalidades.ClearSelection();

                dgvDiagnosticoLocalidades.Rows[0]
                    .Selected =
                        true;

                dgvDiagnosticoLocalidades.CurrentCell =
                    dgvDiagnosticoLocalidades.Rows[0]
                        .Cells["LocalidadHdc"];
            }
            else
            {
                LimpiarDetalleDiagnostico();
            }
        }

        private static DataTable CrearTablaDiagnosticoLocalidades()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "LocalidadHdc",
                typeof(string));

            tabla.Columns.Add(
                "Registros",
                typeof(int));

            tabla.Columns.Add(
                "PendientesPlanta",
                typeof(int));

            tabla.Columns.Add(
                "Turnos",
                typeof(int));

            tabla.Columns.Add(
                "Lineas",
                typeof(int));

            tabla.Columns.Add(
                "Departamentos",
                typeof(int));

            tabla.Columns.Add(
                "Equivalencia",
                typeof(string));

            tabla.Columns.Add(
                "EstadoEquivalencia",
                typeof(string));

            return tabla;
        }

        private void MostrarDetalleLocalidadSeleccionada()
        {
            if (dgvDiagnosticoLocalidades.CurrentRow is null)
            {
                LimpiarDetalleDiagnostico();
                return;
            }

            string localidad =
                Convert.ToString(
                    dgvDiagnosticoLocalidades
                        .CurrentRow
                        .Cells["LocalidadHdc"]
                        .Value)
                ?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(
                    localidad))
            {
                LimpiarDetalleDiagnostico();
                return;
            }

            var registros =
                resultado.Registros
                    .Where(
                        item =>
                            string.Equals(
                                item.LocalidadHdc?.Trim(),
                                localidad,
                                StringComparison.OrdinalIgnoreCase))
                    .ToList();

            string equivalencia =
                Convert.ToString(
                    dgvDiagnosticoLocalidades
                        .CurrentRow
                        .Cells["Equivalencia"]
                        .Value)
                ?.Trim()
                ?? string.Empty;

            lblDiagnosticoSeleccion.Text =
                "Localidad HDC: " +
                localidad;

            lblDiagnosticoResumen.Text =
                "Registros: " +
                registros.Count.ToString("N0") +
                "   |   Equivalencia: " +
                (
                    string.IsNullOrWhiteSpace(
                        equivalencia)
                        ? "PENDIENTE"
                        : equivalencia
                );

            lblDiagnosticoTurnos.Text =
                "Turnos: " +
                CrearResumenFrecuencias(
                    registros.Select(
                        item =>
                            item.TurnoHdc),
                    6);

            lblDiagnosticoLineas.Text =
                "Líneas principales: " +
                CrearResumenFrecuencias(
                    registros.Select(
                        item =>
                            item.LineaHdc),
                    6);

            lblDiagnosticoDepartamentos.Text =
                "Departamentos principales: " +
                CrearResumenFrecuencias(
                    registros.Select(
                        item =>
                            item.DepartamentoHdc),
                    5);

            CargarTurnosDiagnosticoLocalidad(
                registros);

            ActualizarCruceTurno(
                localidad,
                registros);

            btnUsarLocalidadDiagnostico.Enabled =
                true;
        }

        private void CargarTurnosDiagnosticoLocalidad(
            System.Collections.Generic.IEnumerable<RegistroHdc> registros)
        {
            string seleccionAnterior =
                Convert.ToString(
                    cboDiagnosticoTurnoHdc.SelectedItem)
                ?.Trim()
                ?? string.Empty;

            string[] turnos =
                registros
                    .Select(
                        item =>
                            item.TurnoHdc?.Trim()
                            ?? string.Empty)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .OrderBy(
                        valor =>
                            valor,
                        StringComparer.OrdinalIgnoreCase)
                    .ToArray();

            cboDiagnosticoTurnoHdc.DataSource =
                null;

            cboDiagnosticoTurnoHdc.Items.Clear();

            cboDiagnosticoTurnoHdc.Items.Add(
                "TODOS");

            cboDiagnosticoTurnoHdc.Items.AddRange(
                turnos);

            int indice =
                cboDiagnosticoTurnoHdc.Items
                    .Cast<object>()
                    .Select(
                        (item, posicion) =>
                            new
                            {
                                Valor =
                                    Convert.ToString(
                                        item)
                                    ?.Trim()
                                    ?? string.Empty,
                                Posicion =
                                    posicion
                            })
                    .Where(
                        item =>
                            string.Equals(
                                item.Valor,
                                seleccionAnterior,
                                StringComparison.OrdinalIgnoreCase))
                    .Select(
                        item =>
                            item.Posicion)
                    .DefaultIfEmpty(
                        0)
                    .First();

            cboDiagnosticoTurnoHdc.Enabled =
                cboDiagnosticoTurnoHdc.Items.Count > 1;

            cboDiagnosticoTurnoHdc.SelectedIndex =
                Math.Max(
                    0,
                    indice);
        }

        private void ActualizarCruceTurnoSeleccionado()
        {
            if (dgvDiagnosticoLocalidades.CurrentRow is null)
            {
                return;
            }

            string localidad =
                Convert.ToString(
                    dgvDiagnosticoLocalidades
                        .CurrentRow
                        .Cells["LocalidadHdc"]
                        .Value)
                ?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(
                    localidad))
            {
                return;
            }

            var registros =
                resultado.Registros
                    .Where(
                        item =>
                            string.Equals(
                                item.LocalidadHdc?.Trim(),
                                localidad,
                                StringComparison.OrdinalIgnoreCase))
                    .ToList();

            ActualizarCruceTurno(
                localidad,
                registros);
        }

        private void ActualizarCruceTurno(
            string localidad,
            System.Collections.Generic.IEnumerable<RegistroHdc> registrosLocalidad)
        {
            string turnoSeleccionado =
                Convert.ToString(
                    cboDiagnosticoTurnoHdc.SelectedItem)
                ?.Trim()
                ?? "TODOS";

            var registros =
                registrosLocalidad
                    .Where(
                        item =>
                            string.Equals(
                                turnoSeleccionado,
                                "TODOS",
                                StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(
                                item.TurnoHdc?.Trim(),
                                turnoSeleccionado,
                                StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (registros.Count == 0)
            {
                lblDiagnosticoCruceTurno.Text =
                    "No hay registros para el turno seleccionado.";

                dgvDetalleLocalidad.DataSource =
                    CrearTablaDetalleLocalidad();

                return;
            }

            try
            {
                AsegurarIndiceTrabajadoresDiagnostico();

                int existentes =
                    0;

                int noExistentes =
                    0;

                List<string> turnosActuales =
                    new List<string>();

                List<string> plantasActuales =
                    new List<string>();

                DataTable detalle =
                    CrearTablaDetalleLocalidad();

                foreach (RegistroHdc registro
                         in registros
                             .OrderBy(
                                 item =>
                                     item.LineaHdc,
                                 StringComparer.OrdinalIgnoreCase)
                             .ThenBy(
                                 item =>
                                     item.DepartamentoHdc,
                                 StringComparer.OrdinalIgnoreCase)
                             .ThenBy(
                                 item =>
                                     item.Nombre,
                                 StringComparer.OrdinalIgnoreCase)
                             .Take(100))
                {
                    string clave =
                        Normalizar(
                            registro.Empleado);

                    string turnoActual =
                        "NO EXISTE";

                    string plantaActual =
                        "NO EXISTE";

                    if (trabajadoresDiagnosticoIndice is not null &&
                        trabajadoresDiagnosticoIndice.TryGetValue(
                            clave,
                            out DataRow? trabajadorActual))
                    {
                        existentes++;

                        turnoActual =
                            LeerTextoDiagnostico(
                                trabajadorActual,
                                "NombreTurno");

                        plantaActual =
                            LeerTextoDiagnostico(
                                trabajadorActual,
                                "NombrePlanta");

                        if (!string.IsNullOrWhiteSpace(
                                turnoActual))
                        {
                            turnosActuales.Add(
                                turnoActual);
                        }

                        if (!string.IsNullOrWhiteSpace(
                                plantaActual))
                        {
                            plantasActuales.Add(
                                plantaActual);
                        }
                    }
                    else
                    {
                        noExistentes++;
                    }

                    detalle.Rows.Add(
                        registro.Empleado,
                        registro.Nombre,
                        registro.TurnoHdc,
                        turnoActual,
                        plantaActual,
                        registro.LineaHdc,
                        registro.DepartamentoHdc,
                        registro.PuestoHdc,
                        registro.ProcesoHdc);
                }

                // El DataGridView muestra hasta 100 ejemplos, pero los conteos
                // deben considerar TODOS los registros del turno seleccionado.
                if (registros.Count > 100)
                {
                    existentes =
                        0;

                    noExistentes =
                        0;

                    turnosActuales.Clear();

                    plantasActuales.Clear();

                    foreach (RegistroHdc registro in registros)
                    {
                        string clave =
                            Normalizar(
                                registro.Empleado);

                        if (trabajadoresDiagnosticoIndice is not null &&
                            trabajadoresDiagnosticoIndice.TryGetValue(
                                clave,
                                out DataRow? trabajadorActual))
                        {
                            existentes++;

                            string turnoActual =
                                LeerTextoDiagnostico(
                                    trabajadorActual,
                                    "NombreTurno");

                            string plantaActual =
                                LeerTextoDiagnostico(
                                    trabajadorActual,
                                    "NombrePlanta");

                            if (!string.IsNullOrWhiteSpace(
                                    turnoActual))
                            {
                                turnosActuales.Add(
                                    turnoActual);
                            }

                            if (!string.IsNullOrWhiteSpace(
                                    plantaActual))
                            {
                                plantasActuales.Add(
                                    plantaActual);
                            }
                        }
                        else
                        {
                            noExistentes++;
                        }
                    }
                }

                string tituloTurno =
                    string.Equals(
                        turnoSeleccionado,
                        "TODOS",
                        StringComparison.OrdinalIgnoreCase)
                        ? "TODOS"
                        : turnoSeleccionado;

                lblDiagnosticoCruceTurno.Text =
                    "HDC " +
                    localidad +
                    " / " +
                    tituloTurno +
                    ": " +
                    registros.Count.ToString("N0") +
                    "   |   Ya existen: " +
                    existentes.ToString("N0") +
                    "   |   No existen: " +
                    noExistentes.ToString("N0") +
                    Environment.NewLine +
                    "Turno actual: " +
                    CrearResumenFrecuencias(
                        turnosActuales,
                        6) +
                    "   |   Planta actual: " +
                    CrearResumenFrecuencias(
                        plantasActuales,
                        5);

                dgvDetalleLocalidad.DataSource =
                    detalle;
            }
            catch (Exception ex)
            {
                lblDiagnosticoCruceTurno.Text =
                    "No fue posible cruzar el turno HDC con los trabajadores actuales: " +
                    ex.Message;

                dgvDetalleLocalidad.DataSource =
                    CrearTablaDetalleLocalidad();
            }
        }

        private void CargarDiagnosticoTurnosGlobal()
        {
            try
            {
                AsegurarIndiceTrabajadoresDiagnostico();

                DataTable equivalenciasTurno =
                    HdcEquivalencia
                        .ConsultarTurnos();

                Dictionary<string, string> mapaEquivalenciasTurno =
                    equivalenciasTurno.Rows
                        .Cast<DataRow>()
                        .Where(
                            fila =>
                                !string.IsNullOrWhiteSpace(
                                    Convert.ToString(
                                        fila["ValorTurnoHdc"])))
                        .GroupBy(
                            fila =>
                                Normalizar(
                                    Convert.ToString(
                                        fila["ValorTurnoHdc"])),
                            StringComparer.OrdinalIgnoreCase)
                        .ToDictionary(
                            grupo =>
                                grupo.Key,
                            grupo =>
                                LeerTextoDiagnostico(
                                    grupo.First(),
                                    "NombreTurno"),
                            StringComparer.OrdinalIgnoreCase);

                DataTable tabla =
                    CrearTablaDiagnosticoTurnosGlobal();

                int combinacionesConEvidencia =
                    0;

                int combinacionesSinEvidencia =
                    0;

                int coincidenciasExistentes =
                    0;

                var grupos =
                    resultado.Registros
                        .Where(
                            item =>
                                !string.IsNullOrWhiteSpace(
                                    item.LocalidadHdc) &&
                                !string.IsNullOrWhiteSpace(
                                    item.TurnoHdc))
                        .GroupBy(
                            item =>
                                Normalizar(
                                    item.LocalidadHdc) +
                                "|" +
                                Normalizar(
                                    item.TurnoHdc),
                            StringComparer.OrdinalIgnoreCase);

                foreach (var grupo in grupos)
                {
                    int existentes =
                        0;

                    List<string> turnosActuales =
                        new List<string>();

                    List<string> plantasActuales =
                        new List<string>();

                    foreach (RegistroHdc registro in grupo)
                    {
                        string clave =
                            Normalizar(
                                registro.Empleado);

                        if (trabajadoresDiagnosticoIndice is null ||
                            !trabajadoresDiagnosticoIndice.TryGetValue(
                                clave,
                                out DataRow? trabajadorActual))
                        {
                            continue;
                        }

                        existentes++;

                        string turnoActual =
                            LeerTextoDiagnostico(
                                trabajadorActual,
                                "NombreTurno");

                        string plantaActual =
                            LeerTextoDiagnostico(
                                trabajadorActual,
                                "NombrePlanta");

                        if (!string.IsNullOrWhiteSpace(
                                turnoActual))
                        {
                            turnosActuales.Add(
                                turnoActual);
                        }

                        if (!string.IsNullOrWhiteSpace(
                                plantaActual))
                        {
                            plantasActuales.Add(
                                plantaActual);
                        }
                    }

                    int total =
                        grupo.Count();

                    int noExistentes =
                        total -
                        existentes;

                    coincidenciasExistentes +=
                        existentes;

                    if (existentes > 0)
                    {
                        combinacionesConEvidencia++;
                    }
                    else
                    {
                        combinacionesSinEvidencia++;
                    }

                    string equivalenciaActual =
                        mapaEquivalenciasTurno.TryGetValue(
                            Normalizar(
                                grupo.First().TurnoHdc),
                            out string? turnoSistema)
                            ? turnoSistema
                            : string.Empty;

                    DataRow fila =
                        tabla.NewRow();

                    fila["LocalidadHdc"] =
                        grupo.First().LocalidadHdc.Trim();

                    fila["TurnoHdc"] =
                        grupo.First().TurnoHdc.Trim();

                    fila["Registros"] =
                        total;

                    fila["Existentes"] =
                        existentes;

                    fila["NoExistentes"] =
                        noExistentes;

                    fila["CoincidenciaPrincipal"] =
                        CrearCoincidenciaPrincipal(
                            turnosActuales,
                            existentes);

                    fila["TurnosActuales"] =
                        CrearResumenFrecuencias(
                            turnosActuales,
                            6);

                    fila["PlantasActuales"] =
                        CrearResumenFrecuencias(
                            plantasActuales,
                            5);

                    fila["EquivalenciaActual"] =
                        equivalenciaActual;

                    tabla.Rows.Add(
                        fila);
                }

                DataView vista =
                    tabla.DefaultView;

                vista.Sort =
                    "Existentes DESC, Registros DESC, LocalidadHdc ASC, TurnoHdc ASC";

                dgvDiagnosticoTurnosGlobal.DataSource =
                    vista;

                lblDiagnosticoTurnosGlobalResumen.Text =
                    "Combinaciones analizadas: " +
                    tabla.Rows.Count.ToString("N0") +
                    "   |   Con evidencia: " +
                    combinacionesConEvidencia.ToString("N0") +
                    "   |   Sin evidencia: " +
                    combinacionesSinEvidencia.ToString("N0") +
                    "   |   Coincidencias de trabajadores: " +
                    coincidenciasExistentes.ToString("N0") +
                    Environment.NewLine +
                    "La tabla se ordena primero por EXISTEN para mostrar arriba los cruces que sí pueden aportar evidencia.";

                btnUsarTurnoGlobal.Enabled =
                    dgvDiagnosticoTurnosGlobal.Rows.Count > 0;

                if (dgvDiagnosticoTurnosGlobal.Rows.Count > 0)
                {
                    dgvDiagnosticoTurnosGlobal.ClearSelection();

                    dgvDiagnosticoTurnosGlobal.Rows[0]
                        .Selected =
                            true;

                    dgvDiagnosticoTurnosGlobal.CurrentCell =
                        dgvDiagnosticoTurnosGlobal.Rows[0]
                            .Cells["TurnoHdc"];
                }
            }
            catch (Exception ex)
            {
                dgvDiagnosticoTurnosGlobal.DataSource =
                    null;

                lblDiagnosticoTurnosGlobalResumen.Text =
                    "No fue posible generar el diagnóstico global de turnos: " +
                    ex.Message;

                btnUsarTurnoGlobal.Enabled =
                    false;
            }
        }

        private static DataTable CrearTablaDiagnosticoTurnosGlobal()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "LocalidadHdc",
                typeof(string));

            tabla.Columns.Add(
                "TurnoHdc",
                typeof(string));

            tabla.Columns.Add(
                "Registros",
                typeof(int));

            tabla.Columns.Add(
                "Existentes",
                typeof(int));

            tabla.Columns.Add(
                "NoExistentes",
                typeof(int));

            tabla.Columns.Add(
                "CoincidenciaPrincipal",
                typeof(string));

            tabla.Columns.Add(
                "TurnosActuales",
                typeof(string));

            tabla.Columns.Add(
                "PlantasActuales",
                typeof(string));

            tabla.Columns.Add(
                "EquivalenciaActual",
                typeof(string));

            return tabla;
        }

        private static string CrearCoincidenciaPrincipal(
            IEnumerable<string> turnosActuales,
            int existentes)
        {
            if (existentes <= 0)
            {
                return "SIN EVIDENCIA";
            }

            var principal =
                turnosActuales
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Select(
                        valor =>
                            valor.Trim())
                    .GroupBy(
                        valor =>
                            valor,
                        StringComparer.OrdinalIgnoreCase)
                    .OrderByDescending(
                        grupo =>
                            grupo.Count())
                    .ThenBy(
                        grupo =>
                            grupo.Key,
                        StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault();

            if (principal is null)
            {
                return "SIN TURNO ACTUAL";
            }

            int coincidencias =
                principal.Count();

            decimal porcentaje =
                existentes == 0
                    ? 0M
                    : Math.Round(
                        coincidencias * 100M /
                        existentes,
                        1);

            return principal.Key +
                   " " +
                   coincidencias.ToString("N0") +
                   "/" +
                   existentes.ToString("N0") +
                   " (" +
                   porcentaje.ToString("0.#") +
                   "%)";
        }

        private void UsarTurnoGlobalEnEquivalencias()
        {
            if (dgvDiagnosticoTurnosGlobal.CurrentRow is null)
            {
                return;
            }

            string turnoHdc =
                Convert.ToString(
                    dgvDiagnosticoTurnosGlobal
                        .CurrentRow
                        .Cells["TurnoHdc"]
                        .Value)
                ?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(
                    turnoHdc))
            {
                return;
            }

            int indice =
                cboTurnoHdc.Items
                    .Cast<object>()
                    .Select(
                        (item, posicion) =>
                            new
                            {
                                Valor =
                                    Convert.ToString(
                                        item)
                                    ?.Trim()
                                    ?? string.Empty,

                                Posicion =
                                    posicion
                            })
                    .Where(
                        item =>
                            string.Equals(
                                item.Valor,
                                turnoHdc,
                                StringComparison.OrdinalIgnoreCase))
                    .Select(
                        item =>
                            item.Posicion)
                    .DefaultIfEmpty(
                        -1)
                    .First();

            if (indice >= 0)
            {
                cboTurnoHdc.SelectedIndex =
                    indice;
            }

            if (dgvDiagnosticoTurnosGlobal.Parent
                    is TabPage pagina &&
                pagina.Parent
                    is TabControl control)
            {
                control.SelectedIndex =
                    1;
            }
        }

        private void AsegurarIndiceTrabajadoresDiagnostico()
        {
            if (trabajadoresDiagnosticoIndice is not null)
            {
                return;
            }

            Cursor cursorAnterior =
                Cursor;

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                DataTable trabajadores =
                    Trabajador
                        .ConsultarTrabajadoresParaImportacionHdc();

                trabajadoresDiagnosticoIndice =
                    trabajadores.Rows
                        .Cast<DataRow>()
                        .Where(
                            fila =>
                                !string.IsNullOrWhiteSpace(
                                    LeerTextoDiagnostico(
                                        fila,
                                        "NoReloj")))
                        .GroupBy(
                            fila =>
                                Normalizar(
                                    LeerTextoDiagnostico(
                                        fila,
                                        "NoReloj")),
                            StringComparer.OrdinalIgnoreCase)
                        .ToDictionary(
                            grupo =>
                                grupo.Key,
                            grupo =>
                                grupo.First(),
                            StringComparer.OrdinalIgnoreCase);
            }
            finally
            {
                Cursor =
                    cursorAnterior;
            }
        }

        private static string LeerTextoDiagnostico(
            DataRow fila,
            string columna)
        {
            if (!fila.Table.Columns.Contains(
                    columna) ||
                fila[columna] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(
                fila[columna])
                ?.Trim()
                ?? string.Empty;
        }

        private static DataTable CrearTablaDetalleLocalidad()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "Empleado",
                typeof(string));

            tabla.Columns.Add(
                "Nombre",
                typeof(string));

            tabla.Columns.Add(
                "TurnoHdc",
                typeof(string));

            tabla.Columns.Add(
                "TurnoSistemaActual",
                typeof(string));

            tabla.Columns.Add(
                "PlantaSistemaActual",
                typeof(string));

            tabla.Columns.Add(
                "LineaHdc",
                typeof(string));

            tabla.Columns.Add(
                "DepartamentoHdc",
                typeof(string));

            tabla.Columns.Add(
                "PuestoHdc",
                typeof(string));

            tabla.Columns.Add(
                "ProcesoHdc",
                typeof(string));

            return tabla;
        }

        private void LimpiarDetalleDiagnostico()
        {
            lblDiagnosticoSeleccion.Text =
                "Seleccione una localidad.";

            lblDiagnosticoResumen.Text =
                string.Empty;

            lblDiagnosticoTurnos.Text =
                string.Empty;

            lblDiagnosticoLineas.Text =
                string.Empty;

            lblDiagnosticoDepartamentos.Text =
                string.Empty;

            cboDiagnosticoTurnoHdc.DataSource =
                null;

            cboDiagnosticoTurnoHdc.Items.Clear();

            cboDiagnosticoTurnoHdc.Enabled =
                false;

            lblDiagnosticoCruceTurno.Text =
                string.Empty;

            dgvDetalleLocalidad.DataSource =
                null;

            btnUsarLocalidadDiagnostico.Enabled =
                false;
        }

        private void UsarLocalidadDiagnosticoEnPlantas()
        {
            if (dgvDiagnosticoLocalidades.CurrentRow is null)
            {
                return;
            }

            string localidad =
                Convert.ToString(
                    dgvDiagnosticoLocalidades
                        .CurrentRow
                        .Cells["LocalidadHdc"]
                        .Value)
                ?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(
                    localidad))
            {
                return;
            }

            int indice =
                cboLocalidadHdc.Items
                    .Cast<object>()
                    .Select(
                        (item, posicion) =>
                            new
                            {
                                Valor =
                                    Convert.ToString(
                                        item)
                                    ?.Trim()
                                    ?? string.Empty,

                                Posicion =
                                    posicion
                            })
                    .Where(
                        item =>
                            string.Equals(
                                item.Valor,
                                localidad,
                                StringComparison.OrdinalIgnoreCase))
                    .Select(
                        item =>
                            item.Posicion)
                    .DefaultIfEmpty(
                        -1)
                    .First();

            if (indice >= 0)
            {
                cboLocalidadHdc.SelectedIndex =
                    indice;
            }

            if (dgvDiagnosticoLocalidades.Parent
                    is TabPage pagina &&
                pagina.Parent
                    is TabControl control)
            {
                control.SelectedIndex =
                    0;
            }
        }

        private static int ContarDistintos(
            System.Collections.Generic.IEnumerable<string> valores)
        {
            return valores
                .Where(
                    valor =>
                        !string.IsNullOrWhiteSpace(
                            valor))
                .Select(
                    valor =>
                        valor.Trim())
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .Count();
        }

        private static string CrearResumenFrecuencias(
            System.Collections.Generic.IEnumerable<string> valores,
            int maximo)
        {
            string[] resumen =
                valores
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Select(
                        valor =>
                            valor.Trim())
                    .GroupBy(
                        valor =>
                            valor,
                        StringComparer.OrdinalIgnoreCase)
                    .OrderByDescending(
                        grupo =>
                            grupo.Count())
                    .ThenBy(
                        grupo =>
                            grupo.Key,
                        StringComparer.OrdinalIgnoreCase)
                    .Take(
                        maximo)
                    .Select(
                        grupo =>
                            grupo.Key +
                            " (" +
                            grupo.Count().ToString("N0") +
                            ")")
                    .ToArray();

            return resumen.Length == 0
                ? "sin datos"
                : string.Join(
                    ", ",
                    resumen);
        }

        private static string Normalizar(
            string? valor)
        {
            return (valor ?? string.Empty)
                .Trim()
                .ToUpperInvariant();
        }

        private void CargarOrigenes()
        {
            string[] localidades =
                resultado.Registros
                    .Select(
                        item =>
                            item.LocalidadHdc)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .OrderBy(
                        valor =>
                            valor)
                    .ToArray();

            cboLocalidadHdc.Items.AddRange(
                localidades);

            cboLocalidadLineaHdc.Items.AddRange(
                localidades);

            string[] turnos =
                resultado.Registros
                    .Select(
                        item =>
                            item.TurnoHdc)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .OrderBy(
                        valor =>
                            valor)
                    .ToArray();

            cboTurnoHdc.Items.AddRange(
                turnos);

            if (cboLocalidadHdc.Items.Count > 0)
            {
                cboLocalidadHdc.SelectedIndex =
                    0;
            }

            if (cboLocalidadLineaHdc.Items.Count > 0)
            {
                cboLocalidadLineaHdc.SelectedIndex =
                    0;
            }

            if (cboTurnoHdc.Items.Count > 0)
            {
                cboTurnoHdc.SelectedIndex =
                    0;
            }
        }

        private void CargarCatalogos()
        {
            DataTable plantas =
                Planta.ConsultarPlantas(
                    string.Empty);

            cboPlantaSistema.DataSource =
                plantas;

            cboPlantaSistema.DisplayMember =
                "Nombre";

            cboPlantaSistema.ValueMember =
                "Id";

            DataTable turnos =
                Turno.ConsultarTurnos(
                    string.Empty);

            cboTurnoSistema.DataSource =
                turnos;

            cboTurnoSistema.DisplayMember =
                "Nombre";

            cboTurnoSistema.ValueMember =
                "Id";

            LocalidadLineaCambio();
        }

        private void CargarEquivalencias()
        {
            dgvPlantas.DataSource =
                HdcEquivalencia
                    .ConsultarPlantas();

            dgvTurnos.DataSource =
                HdcEquivalencia
                    .ConsultarTurnos();

            dgvLineas.DataSource =
                HdcEquivalencia
                    .ConsultarLineas();

            OcultarColumnasTecnicas(
                dgvPlantas);

            OcultarColumnasTecnicas(
                dgvTurnos);

            OcultarColumnasTecnicas(
                dgvLineas);

            CargarDiagnosticoLocalidades();

            CargarDiagnosticoTurnosGlobal();
        }

        private void GuardarPlanta()
        {
            string origen =
                Convert.ToString(
                    cboLocalidadHdc.SelectedItem)
                ?? string.Empty;

            int idPlanta =
                ObtenerValorEntero(
                    cboPlantaSistema);

            Mensaje respuesta =
                HdcEquivalencia
                    .GuardarPlanta(
                        origen,
                        idPlanta);

            MostrarResultado(
                respuesta);

            if (respuesta.Id == 1)
            {
                CargarEquivalencias();

                LocalidadLineaCambio();
            }
        }

        private void GuardarTurno()
        {
            string origen =
                Convert.ToString(
                    cboTurnoHdc.SelectedItem)
                ?? string.Empty;

            int idTurno =
                ObtenerValorEntero(
                    cboTurnoSistema);

            Mensaje respuesta =
                HdcEquivalencia
                    .GuardarTurno(
                        origen,
                        idTurno);

            MostrarResultado(
                respuesta);

            if (respuesta.Id == 1)
            {
                CargarEquivalencias();
            }
        }

        private void GuardarLinea()
        {
            string localidad =
                Convert.ToString(
                    cboLocalidadLineaHdc.SelectedItem)
                ?? string.Empty;

            string lineaHdc =
                Convert.ToString(
                    cboLineaHdc.SelectedItem)
                ?? string.Empty;

            string accion =
                Convert.ToString(
                    cboAccionLinea.SelectedItem)
                ?? string.Empty;

            int idLinea =
                ObtenerValorEntero(
                    cboLineaSistema);

            int? idLineaGuardar =
                accion == "MAPEAR"
                    ? idLinea
                    : null;

            Mensaje respuesta =
                HdcEquivalencia
                    .GuardarLinea(
                        localidad,
                        lineaHdc,
                        accion,
                        idLineaGuardar);

            MostrarResultado(
                respuesta);

            if (respuesta.Id == 1)
            {
                CargarEquivalencias();
            }
        }

        private void MarcarLineasPendientesSinAsignar()
        {
            string localidad =
                Convert.ToString(
                    cboLocalidadLineaHdc.SelectedItem)
                ?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(
                    localidad))
            {
                AppDialog.ShowWarning(
                    this,
                    "Localidad requerida",
                    "Seleccione una Localidad HDC antes de aplicar la acción masiva.");

                return;
            }

            DataTable equivalenciasPlanta =
                HdcEquivalencia
                    .ConsultarPlantas();

            bool tienePlanta =
                equivalenciasPlanta.Rows
                    .Cast<DataRow>()
                    .Any(
                        fila =>
                            string.Equals(
                                Convert.ToString(
                                    fila["CodigoLocalidadHdc"])
                                    ?.Trim(),
                                localidad,
                                StringComparison.OrdinalIgnoreCase));

            if (!tienePlanta)
            {
                AppDialog.ShowWarning(
                    this,
                    "Equivalencia de planta requerida",
                    "Primero configure la equivalencia de planta para " +
                    localidad +
                    ".");

                return;
            }

            string[] lineasOrigen =
                resultado.Registros
                    .Where(
                        item =>
                            string.Equals(
                                item.LocalidadHdc?.Trim(),
                                localidad,
                                StringComparison.OrdinalIgnoreCase))
                    .Select(
                        item =>
                            item.LineaHdc?.Trim()
                            ?? string.Empty)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .OrderBy(
                        valor =>
                            valor)
                    .ToArray();

            DataTable equivalenciasLinea =
                HdcEquivalencia
                    .ConsultarLineas();

            string[] lineasYaConfiguradas =
                equivalenciasLinea.Rows
                    .Cast<DataRow>()
                    .Where(
                        fila =>
                            string.Equals(
                                Convert.ToString(
                                    fila["CodigoLocalidadHdc"])
                                    ?.Trim(),
                                localidad,
                                StringComparison.OrdinalIgnoreCase))
                    .Select(
                        fila =>
                            Convert.ToString(
                                fila["ValorLineaHdc"])
                            ?.Trim()
                            ?? string.Empty)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .ToArray();

            string[] pendientes =
                lineasOrigen
                    .Where(
                        linea =>
                            !lineasYaConfiguradas.Contains(
                                linea,
                                StringComparer.OrdinalIgnoreCase))
                    .ToArray();

            if (pendientes.Length == 0)
            {
                AppDialog.ShowInfo(
                    this,
                    "Sin líneas pendientes",
                    "Todas las líneas HDC de " +
                    localidad +
                    " ya tienen una equivalencia configurada.");

                return;
            }

            bool confirmar =
                AppDialog.Confirm(
                    this,
                    "Marcar líneas pendientes",
                    "Se marcarán como SIN ASIGNAR " +
                    pendientes.Length.ToString("N0") +
                    " líneas HDC pendientes de " +
                    localidad +
                    "." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Las equivalencias que ya existen, incluyendo MAPEAR, " +
                    "SIN_ASIGNAR o IGNORAR, no se modificarán." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "¿Desea continuar?",
                    "Aplicar");

            if (!confirmar)
            {
                return;
            }

            int guardadas =
                0;

            int errores =
                0;

            string primerError =
                string.Empty;

            Cursor cursorAnterior =
                Cursor;

            try
            {
                Cursor =
                    Cursors.WaitCursor;

                foreach (string linea in pendientes)
                {
                    Mensaje respuesta =
                        HdcEquivalencia
                            .GuardarLinea(
                                localidad,
                                linea,
                                "SIN_ASIGNAR",
                                null);

                    if (respuesta.Id == 1)
                    {
                        guardadas++;

                        continue;
                    }

                    errores++;

                    if (string.IsNullOrWhiteSpace(
                            primerError))
                    {
                        primerError =
                            respuesta.Nombre
                            ?? string.Empty;
                    }
                }
            }
            finally
            {
                Cursor =
                    cursorAnterior;
            }

            CargarEquivalencias();

            if (errores == 0)
            {
                AppDialog.ShowInfo(
                    this,
                    "Equivalencias actualizadas",
                    "Se marcaron correctamente " +
                    guardadas.ToString("N0") +
                    " líneas HDC pendientes de " +
                    localidad +
                    " como SIN ASIGNAR." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Las equivalencias existentes se conservaron sin cambios.");

                return;
            }

            AppDialog.ShowWarning(
                this,
                "Actualización parcial",
                "Se guardaron " +
                guardadas.ToString("N0") +
                " equivalencias y ocurrieron " +
                errores.ToString("N0") +
                " errores." +
                (
                    string.IsNullOrWhiteSpace(
                        primerError)
                        ? string.Empty
                        : Environment.NewLine +
                          Environment.NewLine +
                          "Primer error:" +
                          Environment.NewLine +
                          primerError
                ));
        }

        private void LocalidadLineaCambio()
        {
            string localidad =
                Convert.ToString(
                    cboLocalidadLineaHdc.SelectedItem)
                ?? string.Empty;

            string[] lineasHdc =
                resultado.Registros
                    .Where(
                        item =>
                            string.Equals(
                                item.LocalidadHdc,
                                localidad,
                                StringComparison.OrdinalIgnoreCase))
                    .Select(
                        item =>
                            item.LineaHdc)
                    .Where(
                        valor =>
                            !string.IsNullOrWhiteSpace(
                                valor))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .OrderBy(
                        valor =>
                            valor)
                    .ToArray();

            cboLineaHdc.DataSource =
                null;

            cboLineaHdc.Items.Clear();

            cboLineaHdc.Items.AddRange(
                lineasHdc);

            if (cboLineaHdc.Items.Count > 0)
            {
                cboLineaHdc.SelectedIndex =
                    0;
            }

            DataTable equivalencias =
                HdcEquivalencia
                    .ConsultarPlantas();

            DataRow? equivalencia =
                equivalencias.Rows
                    .Cast<DataRow>()
                    .FirstOrDefault(
                        fila =>
                            string.Equals(
                                Convert.ToString(
                                    fila["CodigoLocalidadHdc"]),
                                localidad,
                                StringComparison.OrdinalIgnoreCase));

            if (equivalencia is null)
            {
                lblPlantaLinea.Text =
                    "Planta del sistema: sin equivalencia. Configure primero la pestaña Plantas.";

                cboLineaSistema.DataSource =
                    null;

                cboLineaSistema.Enabled =
                    false;

                return;
            }

            int idPlanta =
                Convert.ToInt32(
                    equivalencia["IdPlanta"]);

            string nombrePlanta =
                Convert.ToString(
                    equivalencia["NombrePlanta"])
                ?? string.Empty;

            lblPlantaLinea.Text =
                "Planta del sistema: " +
                nombrePlanta;

            DataTable lineasSistema =
                Linea.ConsultarLineasPorPlanta(
                    idPlanta);

            cboLineaSistema.DataSource =
                lineasSistema;

            cboLineaSistema.DisplayMember =
                "Nombre";

            cboLineaSistema.ValueMember =
                "Id";

            ActualizarEstadoLineaSistema();
        }

        private void ActualizarEstadoLineaSistema()
        {
            string accion =
                Convert.ToString(
                    cboAccionLinea.SelectedItem)
                ?? "MAPEAR";

            cboLineaSistema.Enabled =
                accion == "MAPEAR" &&
                cboLineaSistema.DataSource is not null;
        }

        private static int ObtenerValorEntero(
            ComboBox combo)
        {
            if (combo.SelectedValue is null ||
                combo.SelectedValue ==
                DBNull.Value)
            {
                return 0;
            }

            try
            {
                return Convert.ToInt32(
                    combo.SelectedValue);
            }
            catch
            {
                return 0;
            }
        }

        private void MostrarResultado(
            Mensaje respuesta)
        {
            if (respuesta.Id == 1)
            {
                AppDialog.ShowInfo(
                    this,
                    "Equivalencia guardada",
                    respuesta.Nombre);

                return;
            }

            AppDialog.ShowError(
                this,
                "No se pudo guardar",
                respuesta.Nombre);
        }

        private static void OcultarColumnasTecnicas(
            DataGridView grid)
        {
            foreach (string columna
                     in new[]
                     {
                         "Id",
                         "Activo",
                         "Comentario",
                         "IdPlanta",
                         "IdTurno",
                         "IdLinea"
                     })
            {
                if (grid.Columns.Contains(
                    columna))
                {
                    grid.Columns[columna]!
                        .Visible =
                            false;
                }
            }
        }

        private static Label CrearTitulo(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                Text =
                    texto,

                AutoSize =
                    true,

                Location =
                    new Point(
                        x,
                        y),

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Regular(
                        12F,
                        FontStyle.Bold)
            };
        }

        private static Label CrearEtiqueta(
            string texto,
            int x,
            int y)
        {
            return new Label
            {
                Text =
                    texto,

                AutoSize =
                    true,

                Location =
                    new Point(
                        x,
                        y),

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Regular(
                        9F,
                        FontStyle.Bold)
            };
        }

        private static ComboBox CrearCombo()
        {
            return new ComboBox
            {
                DropDownStyle =
                    ComboBoxStyle.DropDownList,

                FlatStyle =
                    FlatStyle.Flat,

                BackColor =
                    Color.White,

                ForeColor =
                    AppColors.TextPrimary,

                Font =
                    AppFonts.Light(
                        10F)
            };
        }

        private static DataGridView CrearGrid()
        {
            DataGridView grid =
                new DataGridView();

            DataGridViewStyler.ApplyCatalogStyle(
                grid);

            grid.ReadOnly =
                true;

            grid.AllowUserToAddRows =
                false;

            grid.AllowUserToDeleteRows =
                false;

            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            return grid;
        }
    }
}
