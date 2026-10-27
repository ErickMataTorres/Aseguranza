using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Collections.Generic;
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
        private const int TamanoPaginaVistaPrevia = 100;
        private readonly List<RegistroHdc> registrosFiltradosVistaPrevia = new();
        private readonly Button btnAnteriorVistaPrevia = new Button();
        private readonly Button btnSiguienteVistaPrevia = new Button();
        private readonly Label lblRangoVistaPrevia = new Label();
        private readonly Label lblPaginaVistaPrevia = new Label();
        private readonly Font fuenteEstadoVistaPrevia =
            AppFonts.Regular(9F, FontStyle.Bold);
        private int paginaVistaPrevia = 1;
        private readonly CheckBox chkConciliarAusencias = new CheckBox();
        private readonly CheckedListBox lstLocalidadesCompletas = new CheckedListBox();
        private bool actualizandoAlcance;

        private readonly Label lblTotal =
            CrearLabelResumen();

        private readonly Label lblNuevos =
            CrearLabelResumen();

        private readonly Label lblActualizar =
            CrearLabelResumen();

        private readonly Label lblReactivar =
            CrearLabelResumen();
        private readonly Label lblInactivar =
            CrearLabelResumen();

        private readonly Label lblSinCambios =
            CrearLabelResumen();

        private readonly Label lblSinPlanta =
            CrearLabelResumen();

        private readonly Label lblSinTurno =
            CrearLabelResumen();

        private readonly Label lblSinLinea =
            CrearLabelResumen();

        private readonly Label lblIgnorados =
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

            InvalidarVistaPrevia(reiniciarPagina: true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                fuenteEstadoVistaPrevia.Dispose();

            base.Dispose(disposing);
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
                lblReactivar,
                "Reactivar",
                "Mostrar solo trabajadores presentes en HDC que se reactivarán");
            ConfigurarFiltroResumen(
                lblInactivar,
                "Inactivar",
                "Mostrar solo trabajadores ausentes de localidades declaradas completas");

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
                lblIgnorados,
                "Ignorar",
                "Mostrar solo registros ignorados");

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
                resultadoActual is null || analizando)
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

            paginaVistaPrevia = 1;
            AplicarFiltroActual();
        }

        private void AplicarFiltroActual()
        {
            registrosFiltradosVistaPrevia.Clear();
            if (resultadoActual is null)
            {
                MostrarPaginaVistaPrevia();
                return;
            }

            string descripcionFiltro =
                "Todos";

            if (!string.IsNullOrWhiteSpace(
                    filtroEstadoActual))
            {
                if (filtroEstadoActual ==
                    "__REVISAR__")
                {
                    descripcionFiltro =
                        "Revisar / Duplicados";
                }
                else
                {
                    descripcionFiltro =
                        filtroEstadoActual;
                }
            }

            foreach (RegistroHdc registro in resultadoActual.Registros)
            {
                bool coincide = string.IsNullOrWhiteSpace(filtroEstadoActual) ||
                    (filtroEstadoActual == "__REVISAR__"
                        ? string.Equals(registro.Estado, "Revisar",
                              StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(registro.Estado, "Duplicado",
                              StringComparison.OrdinalIgnoreCase)
                        : string.Equals(registro.Estado, filtroEstadoActual,
                              StringComparison.OrdinalIgnoreCase));
                if (coincide)
                    registrosFiltradosVistaPrevia.Add(registro);
            }

            MostrarPaginaVistaPrevia();

            lblEstado.Text =
                $"Filtro: {descripcionFiltro}. " +
                $"Mostrando {registrosFiltradosVistaPrevia.Count:N0} de " +
                $"{resultadoActual.TotalRegistros + resultadoActual.Inactivados:N0} registros.";
        }

        private void MostrarPaginaVistaPrevia()
        {
            int total = registrosFiltradosVistaPrevia.Count;
            int totalPaginas = total == 0
                ? 0
                : (total - 1) / TamanoPaginaVistaPrevia + 1;
            paginaVistaPrevia = Math.Min(paginaVistaPrevia, Math.Max(1, totalPaginas));
            int inicio = (paginaVistaPrevia - 1) * TamanoPaginaVistaPrevia;
            List<RegistroHdc> pagina = registrosFiltradosVistaPrevia
                .Skip(inicio)
                .Take(TamanoPaginaVistaPrevia)
                .ToList();

            dgvRegistros.SuspendLayout();
            try
            {
                dgvRegistros.DataSource = pagina;
                dgvRegistros.ClearSelection();
                dgvRegistros.CurrentCell = null;
                if (dgvRegistros.Rows.Count > 0)
                {
                    dgvRegistros.Rows[0].Selected = true;
                    dgvRegistros.CurrentCell = dgvRegistros.Rows[0].Cells[0];
                }
            }
            finally
            {
                dgvRegistros.ResumeLayout();
            }

            lblRangoVistaPrevia.Text = total == 0
                ? "Mostrando 0 de 0"
                : $"Mostrando {inicio + 1:N0}-{inicio + pagina.Count:N0} de {total:N0}";
            lblPaginaVistaPrevia.Text = totalPaginas == 0
                ? "Página 0 de 0"
                : $"Página {paginaVistaPrevia:N0} de {totalPaginas:N0}";
            bool anteriorDisponible = paginaVistaPrevia > 1;
            bool siguienteDisponible = paginaVistaPrevia < totalPaginas;
            if (btnAnteriorVistaPrevia.Enabled != anteriorDisponible)
            {
                btnAnteriorVistaPrevia.Enabled = anteriorDisponible;
                ButtonStyler.UpdateEnabledState(btnAnteriorVistaPrevia, AppColors.Neutral);
            }
            if (btnSiguienteVistaPrevia.Enabled != siguienteDisponible)
            {
                btnSiguienteVistaPrevia.Enabled = siguienteDisponible;
                ButtonStyler.UpdateEnabledState(btnSiguienteVistaPrevia, AppColors.Neutral);
            }
        }

        private void CambiarPaginaVistaPrevia(int numeroPagina)
        {
            int totalPaginas = (registrosFiltradosVistaPrevia.Count +
                TamanoPaginaVistaPrevia - 1) / TamanoPaginaVistaPrevia;
            if (analizando || numeroPagina < 1 || numeroPagina > totalPaginas ||
                numeroPagina == paginaVistaPrevia)
                return;

            paginaVistaPrevia = numeroPagina;
            MostrarPaginaVistaPrevia();
        }

        private void InvalidarVistaPrevia(bool reiniciarPagina = false)
        {
            dgvRegistros.DataSource = null;
            registrosFiltradosVistaPrevia.Clear();
            if (reiniciarPagina)
                paginaVistaPrevia = 1;
            lblRangoVistaPrevia.Text = "Mostrando 0 de 0";
            lblPaginaVistaPrevia.Text = "Página 0 de 0";
            if (btnAnteriorVistaPrevia.Enabled)
            {
                btnAnteriorVistaPrevia.Enabled = false;
                ButtonStyler.UpdateEnabledState(btnAnteriorVistaPrevia, AppColors.Neutral);
            }
            if (btnSiguienteVistaPrevia.Enabled)
            {
                btnSiguienteVistaPrevia.Enabled = false;
                ButtonStyler.UpdateEnabledState(btnSiguienteVistaPrevia, AppColors.Neutral);
            }
        }

        private void ConfigurarVentana()
        {
            FormStyler.ApplyBase(
                this,
                "Importar HDC",
                new Size(
                    1360,
                    870));

            StartPosition =
                FormStartPosition.CenterParent;

            MinimumSize =
                new Size(
                    1160,
                    760);

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
                100);

            AgregarResumen(
                pnlResumen,
                lblNuevos,
                "Nuevos",
                ref x,
                90);

            AgregarResumen(
                pnlResumen,
                lblActualizar,
                "Actualizar",
                ref x,
                100);

            AgregarResumen(
                pnlResumen,
                lblReactivar,
                "Reactivar",
                ref x,
                100);

            AgregarResumen(
                pnlResumen,
                lblInactivar,
                "Inactivar",
                ref x,
                90);

            AgregarResumen(
                pnlResumen,
                lblSinCambios,
                "Sin cambios",
                ref x,
                100);

            AgregarResumen(
                pnlResumen,
                lblSinPlanta,
                "Sin planta",
                ref x,
                95);

            AgregarResumen(
                pnlResumen,
                lblSinTurno,
                "Sin turno",
                ref x,
                90);

            AgregarResumen(
                pnlResumen,
                lblSinLinea,
                "Sin línea",
                ref x,
                90);

            AgregarResumen(
                pnlResumen,
                lblIgnorados,
                "Ignorados",
                ref x,
                95);

            AgregarResumen(
                pnlResumen,
                lblRevisar,
                "Revisar",
                ref x,
                85);

            Panel pnlAlcance = new Panel
            {
                Location = new Point(20, 238),
                Size = new Size(pnlContenido.ClientSize.Width - 40, 155),
                BackColor = AppColors.SectionBackground,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            chkConciliarAusencias.Text = "Conciliar ausencias";
            chkConciliarAusencias.AutoSize = true;
            chkConciliarAusencias.Location = new Point(18, 12);
            chkConciliarAusencias.Enabled = false;
            chkConciliarAusencias.CheckedChanged +=
                async (_, _) => await ActualizarAlcanceAsync();
            Label lblAlcance = new Label
            {
                Text = "Localidades HDC con población completa (selección explícita)",
                AutoSize = true,
                Location = new Point(230, 9),
                ForeColor = AppColors.TextPrimary
            };
            lstLocalidadesCompletas.Location = new Point(230, 29);
            lstLocalidadesCompletas.Size =
                new Size(pnlAlcance.ClientSize.Width - 248, 120);
            lstLocalidadesCompletas.Anchor =
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lstLocalidadesCompletas.CheckOnClick = true;
            lstLocalidadesCompletas.Enabled = false;
            lstLocalidadesCompletas.ItemCheck += (_, e) =>
            {
                if (actualizandoAlcance) return;
                if (e.NewValue == CheckState.Checked &&
                    lstLocalidadesCompletas.Items[e.Index] is LocalidadHdcOpcion opcion &&
                    !opcion.Disponible)
                {
                    e.NewValue = CheckState.Unchecked;
                    AppDialog.ShowWarning(this, "Localidad no disponible",
                        "Esa localidad HDC no tiene una equivalencia de planta válida.");
                    return;
                }
                BeginInvoke(new Action(async () => await ActualizarAlcanceAsync()));
            };
            pnlAlcance.Controls.Add(chkConciliarAusencias);
            pnlAlcance.Controls.Add(lblAlcance);
            pnlAlcance.Controls.Add(lstLocalidadesCompletas);

            Panel pnlTabla =
                new Panel
                {
                    Location =
                        new Point(
                            20,
                            405),

                    Size =
                        new Size(
                            pnlContenido.ClientSize.Width - 40,
                            pnlContenido.ClientSize.Height - 522),

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

            Panel pnlPaginacion = new Panel
            {
                Location = new Point(20, pnlContenido.ClientSize.Height - 112),
                Size = new Size(pnlContenido.ClientSize.Width - 40, 38),
                BackColor = Color.White,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            lblRangoVistaPrevia.Location = new Point(14, 4);
            lblRangoVistaPrevia.Size = new Size(350, 34);
            lblRangoVistaPrevia.TextAlign = ContentAlignment.MiddleLeft;
            lblRangoVistaPrevia.ForeColor = AppColors.TextSecondary;
            lblRangoVistaPrevia.Font = AppFonts.Regular(9.5F);
            lblRangoVistaPrevia.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            ButtonStyler.Apply(btnAnteriorVistaPrevia, "Anterior", AppColors.Neutral,
                icon: null, width: 110, height: 34);
            btnAnteriorVistaPrevia.Location =
                new Point(pnlPaginacion.ClientSize.Width - 390, 4);
            btnAnteriorVistaPrevia.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAnteriorVistaPrevia.Click +=
                (_, _) => CambiarPaginaVistaPrevia(paginaVistaPrevia - 1);

            lblPaginaVistaPrevia.Location =
                new Point(pnlPaginacion.ClientSize.Width - 270, 4);
            lblPaginaVistaPrevia.Size = new Size(130, 34);
            lblPaginaVistaPrevia.TextAlign = ContentAlignment.MiddleCenter;
            lblPaginaVistaPrevia.ForeColor = AppColors.TextPrimary;
            lblPaginaVistaPrevia.Font = AppFonts.Regular(9.5F, FontStyle.Bold);
            lblPaginaVistaPrevia.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            ButtonStyler.Apply(btnSiguienteVistaPrevia, "Siguiente", AppColors.Neutral,
                icon: null, width: 110, height: 34);
            btnSiguienteVistaPrevia.Location =
                new Point(pnlPaginacion.ClientSize.Width - 130, 4);
            btnSiguienteVistaPrevia.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSiguienteVistaPrevia.Click +=
                (_, _) => CambiarPaginaVistaPrevia(paginaVistaPrevia + 1);

            pnlPaginacion.Controls.Add(lblRangoVistaPrevia);
            pnlPaginacion.Controls.Add(btnAnteriorVistaPrevia);
            pnlPaginacion.Controls.Add(lblPaginaVistaPrevia);
            pnlPaginacion.Controls.Add(btnSiguienteVistaPrevia);

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
            pnlContenido.Controls.Add(pnlAlcance);

            pnlContenido.Controls.Add(
                pnlTabla);
            pnlContenido.Controls.Add(pnlPaginacion);

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
                        DataGridViewColumnSortMode.NotSortable
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
            actualizandoAlcance = true;
            chkConciliarAusencias.Checked = false;
            chkConciliarAusencias.Enabled = false;
            lstLocalidadesCompletas.Items.Clear();
            lstLocalidadesCompletas.Enabled = false;
            actualizandoAlcance = false;

            filtroEstadoActual =
                null;

            InvalidarVistaPrevia(reiniciarPagina: true);

            btnEquivalencias.Enabled =
                false;

            btnImportar.Enabled =
                false;

            ActualizarResumen(
                null);

            lblEstado.Text =
                "Archivo seleccionado. Presione Analizar HDC.";
        }

        private void MostrarOpcionesAlcance(ResultadoAnalisisHdc resultado)
        {
            actualizandoAlcance = true;
            try
            {
                chkConciliarAusencias.Enabled = true;
                chkConciliarAusencias.Checked = resultado.ConciliarAusencias;
                lstLocalidadesCompletas.Items.Clear();
                foreach (LocalidadHdcOpcion opcion in resultado.OpcionesLocalidad
                    .OrderByDescending(o =>
                        resultado.LocalidadesCompletas.Contains(o.Codigo))
                    .ThenBy(o => o.Codigo, StringComparer.OrdinalIgnoreCase))
                {
                    int indice = lstLocalidadesCompletas.Items.Add(opcion);
                    if (resultado.LocalidadesCompletas.Contains(opcion.Codigo))
                    {
                        lstLocalidadesCompletas.SetItemChecked(indice, true);
                    }
                }
                if (lstLocalidadesCompletas.Items.Count > 0)
                    lstLocalidadesCompletas.TopIndex = 0;
                lstLocalidadesCompletas.Enabled = resultado.ConciliarAusencias;
            }
            finally
            {
                actualizandoAlcance = false;
            }
        }

        private async Task ActualizarAlcanceAsync()
        {
            if (actualizandoAlcance || analizando || resultadoActual is null)
                return;

            ResultadoAnalisisHdc resultado = resultadoActual;
            resultado.ConciliarAusencias = chkConciliarAusencias.Checked;
            resultado.LocalidadesCompletas.Clear();
            if (resultado.ConciliarAusencias)
            {
                foreach (LocalidadHdcOpcion opcion in
                    lstLocalidadesCompletas.CheckedItems)
                {
                    resultado.LocalidadesCompletas.Add(opcion.Codigo);
                }
            }
            analizando = true;
            btnImportar.Enabled = false;
            chkConciliarAusencias.Enabled = false;
            lstLocalidadesCompletas.Enabled = false;
            InvalidarVistaPrevia();
            try
            {
                await Task.Run(() => AnalizadorHdcSistema.Clasificar(resultado));
                MostrarResultado(resultado);
            }
            catch (Exception ex)
            {
                resultadoActual = null;
                InvalidarVistaPrevia(reiniciarPagina: true);
                ActualizarResumen(null);
                AppDialog.ShowError(this, "Conciliación HDC",
                    ex.Message + Environment.NewLine +
                    "Analice de nuevo el archivo antes de importar.");
            }
            finally
            {
                analizando = false;
                chkConciliarAusencias.Enabled = resultadoActual is not null;
                lstLocalidadesCompletas.Enabled =
                    resultadoActual?.ConciliarAusencias == true;
                ActualizarDisponibilidadImportar();
            }
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

            ResultadoAnalisisHdc? resultadoAnterior = resultadoActual;

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

            chkConciliarAusencias.Enabled = false;
            lstLocalidadesCompletas.Enabled = false;
            InvalidarVistaPrevia(reiniciarPagina: true);
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

                            if (resultadoAnterior is not null &&
                                analisis.HuellaSha256 ==
                                resultadoAnterior.HuellaSha256)
                            {
                                analisis.ConciliarAusencias =
                                    resultadoAnterior.ConciliarAusencias;
                                analisis.LocalidadesCompletas.UnionWith(
                                    resultadoAnterior.LocalidadesCompletas);
                            }

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

                InvalidarVistaPrevia(reiniciarPagina: true);

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

                chkConciliarAusencias.Enabled = resultadoActual is not null;
                lstLocalidadesCompletas.Enabled =
                    resultadoActual?.ConciliarAusencias == true;
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

                InvalidarVistaPrevia();
                AnalizadorHdcSistema
                    .Clasificar(
                        resultadoActual);

                MostrarResultado(
                    resultadoActual);
            }
            catch (Exception ex)
            {
                resultadoActual = null;
                btnImportar.Enabled = false;
                chkConciliarAusencias.Enabled = false;
                lstLocalidadesCompletas.Enabled = false;
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

            int reactivados =
                resultadoActual.Reactivados;
            int inactivados =
                resultadoActual.Inactivados;

            int sinCambios =
                resultadoActual.SinCambios;

            int cambiosPendientes =
                nuevos +
                actualizados +
                reactivados +
                inactivados;

            int pendientes =
                resultadoActual.SinEquivalenciaPlanta +
                resultadoActual.SinEquivalenciaTurno +
                resultadoActual.SinEquivalenciaLinea +
                resultadoActual.RegistrosRevisar +
                resultadoActual.RegistrosDuplicados;

            if (cambiosPendientes <= 0)
            {
                AppDialog.ShowInfo(
                    this,
                    "Sin cambios para importar",
                    "No hay cambios de trabajadores pendientes.");

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
                    $"Reactivar: {reactivados:N0}" +
                    Environment.NewLine +
                    $"Inactivar: {inactivados:N0}" +
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

            if (inactivados > 0 &&
                !AppDialog.Confirm(
                    this,
                    "Confirmar inactivación por ausencia",
                    "La conciliación de ausencias está ACTIVADA." +
                    Environment.NewLine +
                    "Localidades HDC declaradas con población completa: " +
                    string.Join(", ", resultadoActual.LocalidadesCompletas
                        .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)) +
                    Environment.NewLine +
                    $"Trabajadores que quedarán inactivos: {inactivados:N0}." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "No se borrará ningún trabajador. Se conservarán certificaciones, " +
                    "fotografías, expedientes e historial." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Confirme solo si el archivo contiene la población COMPLETA " +
                    "de cada localidad indicada.",
                    "Confirmar inactivación"))
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
                    paginaVistaPrevia = 1;

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
                    $"Reactivados: {importacion.Reactivados:N0}" +
                    Environment.NewLine +
                    $"Inactivados: {importacion.Inactivados:N0}" +
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

            int cambiosPendientes =
                resultadoActual.Nuevos +
                resultadoActual.Actualizados +
                resultadoActual.Reactivados +
                resultadoActual.Inactivados;

            btnImportar.Enabled =
                cambiosPendientes > 0 &&
                (!resultadoActual.ConciliarAusencias ||
                 resultadoActual.LocalidadesCompletas.Count > 0);
        }

        private void MostrarResultado(
            ResultadoAnalisisHdc resultado)
        {
            MostrarOpcionesAlcance(resultado);
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
                resultado.Reactivados +
                resultado.SinCambios;

            AplicarFiltroActual();

            if (string.IsNullOrWhiteSpace(filtroEstadoActual))
            {
                lblEstado.Text =
                    $"Análisis completado. Hoja: {resultado.NombreHoja}. " +
                    $"Preparados: {preparados:N0}. " +
                    $"Inactivar: {resultado.Inactivados:N0}. " +
                    $"Pendientes: {pendientes:N0}. " +
                    $"Ignorados: {resultado.Ignorados:N0}. " +
                    "No se realizó ningún cambio en Trabajador.";
            }
        }

        private void ActualizarResumen(
            ResultadoAnalisisHdc? resultado)
        {
            if (resultado is null)
            {
                lblTotal.Text = "0";
                lblNuevos.Text = "0";
                lblActualizar.Text = "0";
                lblReactivar.Text = "0";
                lblInactivar.Text = "0";
                lblSinCambios.Text = "0";
                lblSinPlanta.Text = "0";
                lblSinTurno.Text = "0";
                lblSinLinea.Text = "0";
                lblIgnorados.Text = "0";
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

            lblReactivar.Text =
                resultado.Reactivados
                    .ToString("N0");

            lblInactivar.Text =
                resultado.Inactivados
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

            lblIgnorados.Text =
                resultado.Ignorados
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

            if (dgvRegistros.Columns[e.ColumnIndex].Name == "NumeroFilaExcel" &&
                Convert.ToString(e.Value) == "0")
            {
                e.Value = "—";
                e.FormattingApplied = true;
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

            DataGridViewCellStyle cellStyle =
                e.CellStyle
                ?? new DataGridViewCellStyle();

            e.CellStyle =
                cellStyle;

            cellStyle.Font =
                fuenteEstadoVistaPrevia;

            if (estado == "Nuevo")
            {
                cellStyle.ForeColor =
                    Color.FromArgb(
                        0,
                        110,
                        45);
            }
            else if (estado == "Actualizar")
            {
                cellStyle.ForeColor =
                    Color.DarkOrange;
            }
            else if (estado == "Reactivar")
            {
                cellStyle.ForeColor =
                    Color.DarkOrange;
            }
            else if (estado == "Inactivar")
            {
                cellStyle.ForeColor =
                    Color.FromArgb(135, 45, 90);
            }
            else if (estado == "Sin cambios")
            {
                cellStyle.ForeColor =
                    AppColors.Primary;
            }
            else if (estado == "Ignorar")
            {
                cellStyle.ForeColor =
                    AppColors.TextSecondary;
            }
            else if (estado.StartsWith(
                         "Sin equivalencia",
                         StringComparison.OrdinalIgnoreCase))
            {
                cellStyle.ForeColor =
                    Color.DarkOrange;
            }
            else
            {
                cellStyle.ForeColor =
                    AppColors.Danger;
            }
        }
    }
}
