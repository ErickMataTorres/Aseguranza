using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Aseguranza.UI
{
    public sealed class AppDatePickerDialog : Form
    {
        private static readonly string[] Meses =
        {
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre",
            "Octubre",
            "Noviembre",
            "Diciembre"
        };

        private readonly DateTime _fechaMinima;
        private readonly DateTime _fechaMaxima;

        private DateTime _fechaSeleccionada;
        private DateTime _mesVisible;
        private bool _actualizandoControles;

        private readonly ComboBox _cbMes;
        private readonly NumericUpDown _nudAnio;
        private readonly TableLayoutPanel _pnlDias;
        private readonly Button _btnAnterior;
        private readonly Button _btnSiguiente;
        private readonly Button _btnHoy;
        private readonly Button _btnCancelar;
        private readonly Button _btnSeleccionar;
        private readonly Label _lblFechaSeleccionada;

        private AppDatePickerDialog(
            DateTime fechaInicial,
            DateTime fechaMinima,
            DateTime fechaMaxima)
        {
            _fechaMinima = fechaMinima.Date;
            _fechaMaxima = fechaMaxima.Date;

            if (_fechaMaxima < _fechaMinima)
            {
                throw new ArgumentException(
                    "La fecha máxima no puede ser menor que la fecha mínima.");
            }

            _fechaSeleccionada =
                LimitarFecha(
                    fechaInicial.Date);

            _mesVisible =
                new DateTime(
                    _fechaSeleccionada.Year,
                    _fechaSeleccionada.Month,
                    1);

            Text = string.Empty;
            ClientSize = new Size(620, 570);
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            KeyPreview = true;
            BackColor = AppColors.BorderMedium;
            Padding = new Padding(1);
            Font = AppFonts.Regular(10F);

            KeyDown += AppDatePickerDialog_KeyDown;

            Panel pnlCard =
                new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = AppColors.CardBackground
                };

            Controls.Add(pnlCard);

            // =====================================================
            // CABECERA
            // =====================================================

            Panel pnlHeader =
                new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 60,
                    BackColor = AppColors.Primary
                };

            Label lblTitulo =
                new Label
                {
                    AutoSize = true,
                    Text = "Seleccionar fecha",
                    Location = new Point(24, 18),
                    ForeColor = Color.White,
                    Font = AppFonts.Regular(14F, FontStyle.Bold),
                    BackColor = Color.Transparent
                };

            pnlHeader.Controls.Add(lblTitulo);
            pnlCard.Controls.Add(pnlHeader);

            // =====================================================
            // NAVEGACIÓN MES / AÑO
            // =====================================================

            Panel pnlNavegacion =
                new Panel
                {
                    Location = new Point(28, 78),
                    Size = new Size(562, 52),
                    BackColor = AppColors.SectionBackground
                };

            RoundedControlHelper.ApplyRoundedRegion(
                pnlNavegacion,
                9);

            _btnAnterior =
                CrearBotonNavegacion("‹");

            _btnAnterior.Location = new Point(10, 7);
            _btnAnterior.Click += (_, _) => CambiarMes(-1);

            _cbMes =
                new ComboBox
                {
                    Location = new Point(64, 10),
                    Size = new Size(245, 32),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    FlatStyle = FlatStyle.Flat,
                    Font = AppFonts.Regular(11F),
                    BackColor = Color.White,
                    ForeColor = AppColors.TextPrimary
                };

            _cbMes.Items.AddRange(Meses);
            _cbMes.SelectedIndexChanged += (_, _) => ActualizarMesDesdeControles();

            _nudAnio =
                new NumericUpDown
                {
                    Location = new Point(323, 10),
                    Size = new Size(145, 32),
                    Minimum = _fechaMinima.Year,
                    Maximum = _fechaMaxima.Year,
                    Font = AppFonts.Regular(11F),
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.FixedSingle,
                    ThousandsSeparator = false
                };

            _nudAnio.ValueChanged += (_, _) => ActualizarMesDesdeControles();

            _btnSiguiente =
                CrearBotonNavegacion("›");

            _btnSiguiente.Location = new Point(510, 7);
            _btnSiguiente.Click += (_, _) => CambiarMes(1);

            pnlNavegacion.Controls.Add(_btnAnterior);
            pnlNavegacion.Controls.Add(_cbMes);
            pnlNavegacion.Controls.Add(_nudAnio);
            pnlNavegacion.Controls.Add(_btnSiguiente);

            pnlCard.Controls.Add(pnlNavegacion);

            // =====================================================
            // ENCABEZADOS DE DÍA
            // =====================================================

            string[] encabezados =
            {
                "Lun",
                "Mar",
                "Mié",
                "Jue",
                "Vie",
                "Sáb",
                "Dom"
            };

            int anchoColumna = 78;

            for (int i = 0; i < encabezados.Length; i++)
            {
                Label lblDia =
                    new Label
                    {
                        AutoSize = false,
                        Text = encabezados[i],
                        Location = new Point(37 + (i * anchoColumna), 145),
                        Size = new Size(anchoColumna, 24),
                        ForeColor = AppColors.TextSecondary,
                        Font = AppFonts.Regular(9F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.Transparent
                    };

                pnlCard.Controls.Add(lblDia);
            }

            // =====================================================
            // CUADRÍCULA DE DÍAS
            // =====================================================

            _pnlDias =
                new TableLayoutPanel
                {
                    Location = new Point(37, 172),
                    Size = new Size(546, 282),
                    ColumnCount = 7,
                    RowCount = 6,
                    BackColor = Color.Transparent,
                    Margin = Padding.Empty,
                    Padding = Padding.Empty
                };

            for (int i = 0; i < 7; i++)
            {
                _pnlDias.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent,
                        100F / 7F));
            }

            for (int i = 0; i < 6; i++)
            {
                _pnlDias.RowStyles.Add(
                    new RowStyle(
                        SizeType.Percent,
                        100F / 6F));
            }

            for (int i = 0; i < 42; i++)
            {
                Button btnDia = CrearBotonDia();
                btnDia.Click += BtnDia_Click;

                _pnlDias.Controls.Add(
                    btnDia,
                    i % 7,
                    i / 7);
            }

            pnlCard.Controls.Add(_pnlDias);

            // =====================================================
            // FECHA SELECCIONADA
            // =====================================================

            _lblFechaSeleccionada =
                new Label
                {
                    AutoSize = true,
                    Location = new Point(30, 472),
                    ForeColor = AppColors.TextSecondary,
                    Font = AppFonts.Regular(9F),
                    BackColor = Color.Transparent
                };

            pnlCard.Controls.Add(_lblFechaSeleccionada);

            Panel separador =
                new Panel
                {
                    Location = new Point(28, 495),
                    Size = new Size(562, 1),
                    BackColor = AppColors.Border
                };

            pnlCard.Controls.Add(separador);

            // =====================================================
            // BOTONES INFERIORES
            // =====================================================

            _btnHoy =
                CrearBotonAccion(
                    "Hoy",
                    AppColors.Secondary,
                    110);

            _btnHoy.Location = new Point(28, 512);
            _btnHoy.Click += BtnHoy_Click;

            _btnCancelar =
                CrearBotonAccion(
                    "Cancelar",
                    AppColors.Neutral,
                    125);

            _btnCancelar.Location = new Point(326, 512);
            _btnCancelar.DialogResult = DialogResult.Cancel;

            _btnSeleccionar =
                CrearBotonAccion(
                    "Seleccionar",
                    AppColors.Primary,
                    135);

            _btnSeleccionar.Location = new Point(455, 512);
            _btnSeleccionar.Click += BtnSeleccionar_Click;

            pnlCard.Controls.Add(_btnHoy);
            pnlCard.Controls.Add(_btnCancelar);
            pnlCard.Controls.Add(_btnSeleccionar);

            AcceptButton = _btnSeleccionar;
            CancelButton = _btnCancelar;

            Shown +=
                (_, _) =>
                {
                    RoundedControlHelper.ApplyRoundedRegion(this, 12);
                    _btnSeleccionar.Focus();
                };

            SincronizarControlesNavegacion();
            RenderizarCalendario();
        }

        public DateTime FechaSeleccionada =>
            _fechaSeleccionada;

        // =========================================================
        // API PÚBLICA
        // =========================================================

        public static bool TrySelectDate(
            Form owner,
            DateTime fechaInicial,
            out DateTime fechaSeleccionada,
            DateTime? fechaMinima = null,
            DateTime? fechaMaxima = null)
        {
            DateTime minima =
                (fechaMinima ?? new DateTime(1753, 1, 1)).Date;

            DateTime maxima =
                (fechaMaxima ?? new DateTime(9998, 12, 31)).Date;

            using AppDatePickerDialog dialog =
                new AppDatePickerDialog(
                    fechaInicial,
                    minima,
                    maxima);

            dialog.PosicionarSobre(owner);

            bool aceptado =
                dialog.ShowDialog(owner) == DialogResult.OK;

            fechaSeleccionada =
                aceptado
                    ? dialog.FechaSeleccionada
                    : fechaInicial.Date;

            return aceptado;
        }

        // =========================================================
        // NAVEGACIÓN
        // =========================================================

        private void ActualizarMesDesdeControles()
        {
            if (_actualizandoControles ||
                _cbMes.SelectedIndex < 0)
            {
                return;
            }

            int anio =
                decimal.ToInt32(
                    _nudAnio.Value);

            int mes =
                _cbMes.SelectedIndex + 1;

            _mesVisible =
                NormalizarMesVisible(
                    new DateTime(
                        anio,
                        mes,
                        1));

            SincronizarControlesNavegacion();
            RenderizarCalendario();
        }

        private void CambiarMes(
            int desplazamiento)
        {
            DateTime nuevoMes;

            try
            {
                nuevoMes =
                    _mesVisible.AddMonths(
                        desplazamiento);
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            nuevoMes =
                NormalizarMesVisible(
                    nuevoMes);

            if (nuevoMes == _mesVisible)
            {
                return;
            }

            _mesVisible = nuevoMes;

            SincronizarControlesNavegacion();
            RenderizarCalendario();
        }

        private DateTime NormalizarMesVisible(
            DateTime mes)
        {
            DateTime inicioMinimo =
                new DateTime(
                    _fechaMinima.Year,
                    _fechaMinima.Month,
                    1);

            DateTime inicioMaximo =
                new DateTime(
                    _fechaMaxima.Year,
                    _fechaMaxima.Month,
                    1);

            DateTime inicioMes =
                new DateTime(
                    mes.Year,
                    mes.Month,
                    1);

            if (inicioMes < inicioMinimo)
            {
                return inicioMinimo;
            }

            if (inicioMes > inicioMaximo)
            {
                return inicioMaximo;
            }

            return inicioMes;
        }

        private void SincronizarControlesNavegacion()
        {
            _actualizandoControles = true;

            try
            {
                _cbMes.SelectedIndex =
                    _mesVisible.Month - 1;

                _nudAnio.Value =
                    _mesVisible.Year;
            }
            finally
            {
                _actualizandoControles = false;
            }

            DateTime mesMinimo =
                new DateTime(
                    _fechaMinima.Year,
                    _fechaMinima.Month,
                    1);

            DateTime mesMaximo =
                new DateTime(
                    _fechaMaxima.Year,
                    _fechaMaxima.Month,
                    1);

            _btnAnterior.Enabled =
                _mesVisible > mesMinimo;

            _btnSiguiente.Enabled =
                _mesVisible < mesMaximo;

            _btnHoy.Enabled =
                DateTime.Today >= _fechaMinima &&
                DateTime.Today <= _fechaMaxima;
        }

        // =========================================================
        // CALENDARIO
        // =========================================================

        private void RenderizarCalendario()
        {
            DateTime primerDiaMes =
                new DateTime(
                    _mesVisible.Year,
                    _mesVisible.Month,
                    1);

            int diasMes =
                DateTime.DaysInMonth(
                    _mesVisible.Year,
                    _mesVisible.Month);

            int indicePrimerDia =
                ((int)primerDiaMes.DayOfWeek + 6) % 7;

            for (int i = 0; i < _pnlDias.Controls.Count; i++)
            {
                Button btnDia =
                    (Button)_pnlDias.Controls[i];

                int dia =
                    i - indicePrimerDia + 1;

                if (dia < 1 || dia > diasMes)
                {
                    btnDia.Text = string.Empty;
                    btnDia.Tag = null;
                    btnDia.Enabled = false;
                    btnDia.BackColor = Color.Transparent;
                    btnDia.FlatAppearance.BorderSize = 0;
                    continue;
                }

                DateTime fecha =
                    new DateTime(
                        _mesVisible.Year,
                        _mesVisible.Month,
                        dia);

                bool habilitado =
                    fecha >= _fechaMinima &&
                    fecha <= _fechaMaxima;

                bool seleccionado =
                    fecha == _fechaSeleccionada;

                bool esHoy =
                    fecha == DateTime.Today;

                btnDia.Text =
                    dia.ToString(
                        CultureInfo.InvariantCulture);

                btnDia.Tag = fecha;
                btnDia.Enabled = habilitado;

                if (!habilitado)
                {
                    btnDia.BackColor = AppColors.AppBackground;
                    btnDia.ForeColor = AppColors.DisabledText;
                    btnDia.FlatAppearance.BorderSize = 0;
                }
                else if (seleccionado)
                {
                    btnDia.BackColor = AppColors.Primary;
                    btnDia.ForeColor = Color.White;
                    btnDia.FlatAppearance.BorderSize = 0;
                }
                else if (esHoy)
                {
                    btnDia.BackColor = AppColors.Selection;
                    btnDia.ForeColor = AppColors.Primary;
                    btnDia.FlatAppearance.BorderSize = 1;
                    btnDia.FlatAppearance.BorderColor = AppColors.Primary;
                }
                else
                {
                    btnDia.BackColor = Color.White;
                    btnDia.ForeColor = AppColors.TextPrimary;
                    btnDia.FlatAppearance.BorderSize = 0;
                }
            }

            _lblFechaSeleccionada.Text =
                "Fecha seleccionada: " +
                _fechaSeleccionada.ToString(
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
        }

        private void BtnDia_Click(
            object? sender,
            EventArgs e)
        {
            if (sender is not Button button ||
                button.Tag is not DateTime fecha)
            {
                return;
            }

            _fechaSeleccionada = fecha.Date;
            RenderizarCalendario();
        }

        private void BtnHoy_Click(
            object? sender,
            EventArgs e)
        {
            DateTime hoy =
                LimitarFecha(
                    DateTime.Today);

            _fechaSeleccionada = hoy;
            _mesVisible =
                new DateTime(
                    hoy.Year,
                    hoy.Month,
                    1);

            SincronizarControlesNavegacion();
            RenderizarCalendario();
        }

        private void BtnSeleccionar_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        // =========================================================
        // HELPERS VISUALES
        // =========================================================

        private static Button CrearBotonNavegacion(
            string texto)
        {
            Button button =
                new Button
                {
                    Text = texto,
                    Size = new Size(42, 38),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = AppColors.Primary,
                    ForeColor = Color.White,
                    Font = AppFonts.Regular(17F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false,
                    TabStop = true
                };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(
                    AppColors.Primary,
                    0.05F);

            button.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(
                    AppColors.Primary,
                    0.10F);

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                7);

            return button;
        }

        private static Button CrearBotonDia()
        {
            Button button =
                new Button
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = AppColors.TextPrimary,
                    Font = AppFonts.Regular(10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false,
                    TabStop = true
                };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                AppColors.SectionBackground;

            button.FlatAppearance.MouseDownBackColor =
                AppColors.Selection;

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                7);

            return button;
        }

        private static Button CrearBotonAccion(
            string texto,
            Color color,
            int ancho)
        {
            Button button =
                new Button
                {
                    Text = texto,
                    Size = new Size(ancho, 42),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = color,
                    ForeColor = Color.White,
                    Font = AppFonts.Regular(10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false
                };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                ControlPaint.Dark(
                    color,
                    0.05F);

            button.FlatAppearance.MouseDownBackColor =
                ControlPaint.Dark(
                    color,
                    0.10F);

            RoundedControlHelper.ApplyRoundedRegion(
                button,
                7);

            return button;
        }

        private DateTime LimitarFecha(
            DateTime fecha)
        {
            if (fecha < _fechaMinima)
            {
                return _fechaMinima;
            }

            if (fecha > _fechaMaxima)
            {
                return _fechaMaxima;
            }

            return fecha.Date;
        }

        private void PosicionarSobre(
            Form owner)
        {
            Rectangle areaVisible =
                Screen
                    .FromControl(owner)
                    .WorkingArea;

            Rectangle ownerBounds =
                owner.Bounds;

            int x =
                ownerBounds.Left +
                ((ownerBounds.Width - Width) / 2);

            int y =
                ownerBounds.Top +
                ((ownerBounds.Height - Height) / 2);

            x =
                Math.Max(
                    areaVisible.Left,
                    Math.Min(
                        x,
                        areaVisible.Right - Width));

            y =
                Math.Max(
                    areaVisible.Top,
                    Math.Min(
                        y,
                        areaVisible.Bottom - Height));

            Location = new Point(x, y);
        }

        private void AppDatePickerDialog_KeyDown(
            object? sender,
            KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
            {
                return;
            }

            DialogResult = DialogResult.Cancel;
            Close();
            e.Handled = true;
        }
    }
}
