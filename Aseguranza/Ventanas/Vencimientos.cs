using Aseguranza.Clases;
using Aseguranza.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public sealed class Vencimientos : Form
    {
        private sealed record CatalogItem(int? Id, string Name)
        {
            public override string ToString() => Name;
        }

        private readonly ComboBox _personal = new();
        private readonly ComboBox _mostrar = new();
        private readonly ComboBox _horizonte = new();
        private readonly ComboBox _planta = new();
        private readonly ComboBox _linea = new();
        private readonly ComboBox _turno = new();
        private readonly ComboBox _proceso = new();
        private readonly TextBox _buscar = new();
        private readonly DataGridView _grid = new();
        private readonly Label[] _metricas = new Label[4];
        private readonly Label _rango = new();
        private readonly Label _pagina = new();
        private readonly Button _anterior = new();
        private readonly Button _siguiente = new();
        private readonly Button _detalle = new();
        private bool _inicializando = true;
        private bool _vinculando;
        private int _paginaActual = 1;
        private int _totalPaginas;

        public Vencimientos()
        {
            SuspendLayout();
            FormStyler.ApplyBase(this, "Vencimientos", new Size(1180, 800));
            DoubleBuffered = true;
            FormStyler.CreateHeader(this, "Vencimientos",
                "Certificaciones vencidas y próximas a vencer");
            Panel contenido = FormStyler.CreateCard(this, new Point(20, 105),
                new Size(1140, 670));
            ConstruirFiltros(contenido);
            ConstruirMetricas(contenido);
            ConstruirGrid(contenido);
            ConstruirPaginador(contenido);
            ResumeLayout(false);
            Load += (_, _) => Inicializar();
            Shown += (_, _) =>
            {
                if (Controls["lblAppFooter"] is Label pie)
                    pie.Text = pie.Text.Replace("Desarrollado por Erick Mata",
                        "Hecho por: Erick Mata");
            };
        }

        private void ConstruirFiltros(Panel contenido)
        {
            Panel filtros = new()
            {
                Location = new Point(20, 18),
                Size = new Size(1100, 140),
                BackColor = AppColors.AlternateRow
            };
            RoundedControlHelper.ApplyRoundedRegion(filtros, 10);
            contenido.Controls.Add(filtros);
            CrearCombo(filtros, "Personal", _personal, 16, 3, 150);
            CrearCombo(filtros, "Mostrar", _mostrar, 186, 3, 150);
            CrearCombo(filtros, "Horizonte", _horizonte, 356, 3, 150);
            CrearCombo(filtros, "Planta", _planta, 526, 3, 210);
            CrearCombo(filtros, "Línea", _linea, 16, 70, 230);
            CrearCombo(filtros, "Turno", _turno, 266, 70, 150);
            CrearCombo(filtros, "Proceso", _proceso, 436, 70, 220);
            filtros.Controls.Add(new Label
            {
                Text = "Buscar trabajador / proceso",
                Location = new Point(676, 70),
                Size = new Size(260, 20),
                ForeColor = AppColors.TextPrimary,
                Font = AppFonts.Regular(9F, FontStyle.Bold)
            });
            Panel bordeBuscar = new()
            {
                Location = new Point(676, 91),
                Size = new Size(405, 42),
                BackColor = Color.White
            };
            _buscar.Location = new Point(12, 9);
            _buscar.Size = new Size(381, 25);
            _buscar.BorderStyle = BorderStyle.None;
            _buscar.BackColor = Color.White;
            _buscar.ForeColor = AppColors.TextPrimary;
            _buscar.Font = AppFonts.Light(11F);
            _buscar.MaxLength = 100;
            _buscar.PlaceholderText = "No. Reloj, nombre, proceso, planta...";
            InputStyler.ApplyOutlinedInput(bordeBuscar, _buscar);
            bordeBuscar.Controls.Add(_buscar);
            filtros.Controls.Add(bordeBuscar);

            foreach (ComboBox combo in new[]
                { _personal, _mostrar, _horizonte, _linea, _turno, _proceso })
                combo.SelectedIndexChanged += (_, _) => FiltroCambiado();
            _planta.SelectedIndexChanged += (_, _) =>
            {
                if (_inicializando) return;
                _inicializando = true;
                try { CargarLineas(); }
                finally { _inicializando = false; }
                FiltroCambiado();
            };
            _buscar.TextChanged += (_, _) => FiltroCambiado();
        }

        private static void CrearCombo(Panel parent, string titulo,
            ComboBox combo, int x, int y, int width)
        {
            parent.Controls.Add(new Label
            {
                Text = titulo,
                Location = new Point(x, y),
                Size = new Size(width, 20),
                ForeColor = AppColors.TextPrimary,
                Font = AppFonts.Regular(9F, FontStyle.Bold)
            });
            Panel borde = new()
            {
                Location = new Point(x, y + 21),
                Size = new Size(width, 42),
                BackColor = Color.White
            };
            ComboBoxStyler.ApplyOutlinedComboBox(borde, combo);
            borde.Controls.Add(combo);
            parent.Controls.Add(borde);
        }

        private void ConstruirMetricas(Panel contenido)
        {
            string[] titulos =
                { "Vencidas", "Próximas", "Trabajadores afectados", "Procesos afectados" };
            for (int i = 0; i < 4; i++)
            {
                Panel tarjeta = new()
                {
                    Location = new Point(20 + i * 280, 162),
                    Size = new Size(260, 72),
                    BackColor = AppColors.SectionBackground
                };
                RoundedControlHelper.ApplyRoundedRegion(tarjeta, 10);
                tarjeta.Controls.Add(new Label
                {
                    Text = titulos[i],
                    Location = new Point(12, 8),
                    Size = new Size(240, 22),
                    ForeColor = AppColors.TextSecondary,
                    Font = AppFonts.Regular(9.5F)
                });
                _metricas[i] = new Label
                {
                    Text = "0",
                    Location = new Point(12, 32),
                    Size = new Size(240, 32),
                    ForeColor = AppColors.Primary,
                    Font = AppFonts.Regular(15F, FontStyle.Bold)
                };
                tarjeta.Controls.Add(_metricas[i]);
                contenido.Controls.Add(tarjeta);
            }
        }

        private void ConstruirGrid(Panel contenido)
        {
            _grid.Location = new Point(20, 247);
            _grid.Size = new Size(1100, 315);
            DataGridViewStyler.ApplyCatalogStyle(_grid);
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            _grid.AutoGenerateColumns = false;
            AgregarColumna("NO. RELOJ", nameof(ExpirationEventRow.WorkerNumber), 105);
            AgregarColumna("TRABAJADOR", nameof(ExpirationEventRow.WorkerName), 185);
            AgregarColumna("PLANTA", nameof(ExpirationEventRow.PlantName), 95);
            AgregarColumna("LÍNEA / ÁREA", nameof(ExpirationEventRow.LineName), 125);
            AgregarColumna("TURNO", nameof(ExpirationEventRow.ShiftName), 90);
            AgregarColumna("PROCESO", nameof(ExpirationEventRow.ProcessName), 175);
            AgregarColumna("FECHA CERTIFICACIÓN", nameof(ExpirationEventRow.CertificationDate), 170)
                .DefaultCellStyle.Format = "dd/MM/yyyy";
            AgregarColumna("FECHA VENCIMIENTO", nameof(ExpirationEventRow.ExpirationDate), 160)
                .DefaultCellStyle.Format = "dd/MM/yyyy";
            AgregarColumna("DÍAS", nameof(ExpirationEventRow.DaysUntilExpiration), 130);
            AgregarColumna("ESTADO", nameof(ExpirationEventRow.EventState), 175);
            AgregarColumna("PERSONAL", nameof(ExpirationEventRow.Active), 95);
            _grid.CellFormatting += FormatearCelda;
            _grid.SelectionChanged += (_, _) =>
            {
                if (!_vinculando) _detalle.Enabled = FilaSeleccionada() is not null;
            };
            _grid.CellDoubleClick += (_, e) =>
            {
                if (e.RowIndex >= 0) AbrirDetalle();
            };
            contenido.Controls.Add(_grid);
        }

        private DataGridViewTextBoxColumn AgregarColumna(string titulo,
            string propiedad, int ancho)
        {
            DataGridViewTextBoxColumn columna = new()
            {
                HeaderText = titulo,
                DataPropertyName = propiedad,
                Width = ancho,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            _grid.Columns.Add(columna);
            return columna;
        }

        private void FormatearCelda(object? sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _grid.Rows.Count ||
                _grid.Rows[e.RowIndex].DataBoundItem is not ExpirationEventRow evento)
                return;
            if (e.CellStyle is DataGridViewCellStyle style)
            {
                style.BackColor = evento.EventState == ExpirationEventState.Vencida
                    ? Color.FromArgb(255, 199, 206) : Color.FromArgb(255, 235, 156);
                style.ForeColor = evento.EventState == ExpirationEventState.Vencida
                    ? Color.FromArgb(140, 30, 40) : Color.FromArgb(120, 82, 0);
                style.SelectionBackColor = AppColors.Selection;
                style.SelectionForeColor = AppColors.TextPrimary;
            }
            string propiedad = _grid.Columns[e.ColumnIndex].DataPropertyName;
            if (propiedad == nameof(ExpirationEventRow.LineName))
                e.Value = evento.LineName ?? "SIN ASIGNAR";
            else if (propiedad == nameof(ExpirationEventRow.DaysUntilExpiration))
                e.Value = evento.DaysUntilExpiration switch
                {
                    < 0 => $"{-evento.DaysUntilExpiration} día{(evento.DaysUntilExpiration == -1 ? "" : "s")} vencida",
                    0 => "Vence hoy",
                    1 => "1 día",
                    _ => $"{evento.DaysUntilExpiration} días"
                };
            else if (propiedad == nameof(ExpirationEventRow.EventState))
                e.Value = evento.EventState == ExpirationEventState.Vencida
                    ? "VENCIDA" : "PRÓXIMA A VENCER";
            else if (propiedad == nameof(ExpirationEventRow.Active))
                e.Value = evento.Active ? "Activo" : "Inactivo";
            else return;
            e.FormattingApplied = true;
        }

        private void ConstruirPaginador(Panel contenido)
        {
            _rango.Location = new Point(20, 570);
            _rango.Size = new Size(410, 38);
            _rango.TextAlign = ContentAlignment.MiddleLeft;
            _rango.Text = "Mostrando 0-0 de 0";
            _rango.ForeColor = AppColors.TextSecondary;
            _rango.Font = AppFonts.Regular(9.5F);
            _pagina.Location = new Point(815, 570);
            _pagina.Size = new Size(130, 38);
            _pagina.TextAlign = ContentAlignment.MiddleCenter;
            _pagina.Text = "Sin páginas";
            _pagina.ForeColor = AppColors.TextPrimary;
            _pagina.Font = AppFonts.Regular(9.5F, FontStyle.Bold);
            ButtonStyler.Apply(_anterior, "Anterior", AppColors.Neutral, null, 110, 34);
            _anterior.Location = new Point(695, 572);
            _anterior.Click += (_, _) => CambiarPagina(_paginaActual - 1);
            ButtonStyler.Apply(_siguiente, "Siguiente", AppColors.Neutral, null, 110, 34);
            _siguiente.Location = new Point(955, 572);
            _siguiente.Click += (_, _) => CambiarPagina(_paginaActual + 1);
            ButtonStyler.Apply(_detalle, "Ver certificaciones", AppColors.Primary,
                null, 190, 40);
            _detalle.Location = new Point(20, 620);
            _detalle.Enabled = false;
            _detalle.Click += (_, _) => AbrirDetalle();
            Button regresar = new();
            ButtonStyler.Apply(regresar, "Regresar", AppColors.Neutral, null, 125, 40);
            regresar.Location = new Point(995, 620);
            regresar.Click += (_, _) => Close();
            contenido.Controls.AddRange(new Control[]
                { _rango, _anterior, _pagina, _siguiente, _detalle, regresar });
        }

        private void Inicializar()
        {
            try
            {
                _personal.Items.AddRange(new object[] { "Activos", "Inactivos", "Todos" });
                _mostrar.Items.AddRange(new object[] { "Todas", "Vencidas", "Próximas" });
                _horizonte.Items.AddRange(new object[]
                    { "7 días", "15 días", "30 días", "60 días", "90 días" });
                LlenarCatalogo(_planta, Planta.ConsultarPlantas(""), "Todas");
                LlenarCatalogo(_turno, Turno.ConsultarTurnos(""), "Todos");
                LlenarCatalogo(_proceso, Proceso.ConsultarProcesos(""), "Todos");
                _personal.SelectedIndex = 0;
                _mostrar.SelectedIndex = 0;
                _horizonte.SelectedIndex = 2;
                CargarLineas();
                _inicializando = false;
                Recargar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                AppDialog.ShowError(this, "Error",
                    "No se pudieron cargar los vencimientos. Verifique la conexión y las migraciones de la base de datos.");
            }
        }

        private static void LlenarCatalogo(ComboBox combo, DataTable tabla,
            string etiquetaTodos)
        {
            combo.Items.Clear();
            combo.Items.Add(new CatalogItem(null, etiquetaTodos));
            foreach (DataRow fila in tabla.Rows)
                combo.Items.Add(new CatalogItem(Convert.ToInt32(fila["Id"]),
                    Convert.ToString(fila["Nombre"]) ?? string.Empty));
            combo.SelectedIndex = 0;
        }

        private void CargarLineas()
        {
            int? planta = IdSeleccionado(_planta);
            DataTable lineas = planta.HasValue
                ? Linea.ConsultarLineasPorPlanta(planta.Value)
                : Linea.ConsultarLineas("");
            LlenarCatalogo(_linea, lineas, "Todas");
        }

        private static int? IdSeleccionado(ComboBox combo) =>
            (combo.SelectedItem as CatalogItem)?.Id;

        private void FiltroCambiado()
        {
            if (_inicializando) return;
            _paginaActual = 1;
            Recargar();
        }

        private void CambiarPagina(int pagina)
        {
            if (pagina < 1 || pagina > _totalPaginas || pagina == _paginaActual)
                return;
            _paginaActual = pagina;
            Recargar();
        }

        private void Recargar(int? seleccionarCertificacion = null)
        {
            if (_inicializando) return;
            try
            {
                ExpirationDashboardRequest solicitud = new()
                {
                    Active = _personal.SelectedIndex switch
                    {
                        1 => false, 2 => null, _ => true
                    },
                    Category = (ExpirationEventCategory)Math.Max(0, _mostrar.SelectedIndex),
                    HorizonDays = new[] { 7, 15, 30, 60, 90 }
                        [Math.Max(0, _horizonte.SelectedIndex)],
                    PlantId = IdSeleccionado(_planta),
                    LineId = IdSeleccionado(_linea),
                    ShiftId = IdSeleccionado(_turno),
                    ProcessId = IdSeleccionado(_proceso),
                    Search = _buscar.Text.Trim(),
                    PageNumber = _paginaActual,
                    PageSize = 100
                };
                ExpirationDashboardResult resultado =
                    Certificacion.ConsultarVencimientos(solicitud);
                _vinculando = true;
                try
                {
                    _grid.DataSource = resultado.Items.ToList();
                    _grid.ClearSelection();
                    _grid.CurrentCell = null;
                    if (seleccionarCertificacion.HasValue)
                    {
                        DataGridViewRow? fila = _grid.Rows.Cast<DataGridViewRow>()
                            .FirstOrDefault(x => x.DataBoundItem is ExpirationEventRow e &&
                                e.CertificationId == seleccionarCertificacion.Value);
                        if (fila is not null)
                        {
                            fila.Selected = true;
                            _grid.CurrentCell = fila.Cells[0];
                        }
                    }
                }
                finally { _vinculando = false; }
                _paginaActual = resultado.PageNumber;
                _totalPaginas = resultado.TotalPages;
                _metricas[0].Text = resultado.Summary.ExpiredEvents.ToString("N0", CultureInfo.CurrentCulture);
                _metricas[1].Text = resultado.Summary.UpcomingEvents.ToString("N0", CultureInfo.CurrentCulture);
                _metricas[2].Text = resultado.Summary.AffectedWorkers.ToString("N0", CultureInfo.CurrentCulture);
                _metricas[3].Text = resultado.Summary.AffectedProcesses.ToString("N0", CultureInfo.CurrentCulture);
                int inicio = resultado.TotalCount == 0 ? 0 :
                    (_paginaActual - 1) * 100 + 1;
                int fin = resultado.TotalCount == 0 ? 0 :
                    inicio + resultado.Items.Count - 1;
                _rango.Text = $"Mostrando {inicio:N0}-{fin:N0} de {resultado.TotalCount:N0}";
                _pagina.Text = _totalPaginas == 0 ? "Sin páginas" :
                    $"Página {_paginaActual} de {_totalPaginas}";
                _anterior.Enabled = _paginaActual > 1;
                _siguiente.Enabled = _paginaActual < _totalPaginas;
                _detalle.Enabled = FilaSeleccionada() is not null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _vinculando = true;
                try
                {
                    _grid.DataSource = null;
                    _grid.ClearSelection();
                }
                finally { _vinculando = false; }
                _detalle.Enabled = false;
                _anterior.Enabled = false;
                _siguiente.Enabled = false;
                _totalPaginas = 0;
                _rango.Text = "Mostrando 0-0 de 0";
                _pagina.Text = "Sin páginas";
                foreach (Label metrica in _metricas) metrica.Text = "0";
                AppDialog.ShowError(this, "Error",
                    "No se pudieron consultar los vencimientos. Verifique la conexión y las migraciones de la base de datos.");
            }
        }

        private ExpirationEventRow? FilaSeleccionada() =>
            _grid.SelectedRows.Count == 1
                ? _grid.SelectedRows[0].DataBoundItem as ExpirationEventRow
                : null;

        private void AbrirDetalle()
        {
            ExpirationEventRow? evento = FilaSeleccionada();
            if (evento is null) return;
            try
            {
                Trabajador? trabajador = Trabajador.ConsultarTrabajador(evento.WorkerNumber);
                if (trabajador is null)
                {
                    AppDialog.ShowWarning(this, "Aviso",
                        "El trabajador ya no está disponible. Actualice la lista.");
                    Recargar();
                    return;
                }
                using CertificacionesVentana detalle = new(trabajador);
                detalle.ShowDialog(this);
                Recargar(evento.CertificationId);
                if (FilaSeleccionada() is not null) _grid.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                AppDialog.ShowError(this, "Error",
                    "No se pudo abrir o actualizar el detalle de certificaciones.");
            }
        }
    }
}
