namespace Aseguranza.Ventanas
{
    partial class CertificacionAnulacionVentana
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitulo = new Label();
            lblTrabajador = new Label();
            lblProceso = new Label();
            lblTipo = new Label();
            lblFechaInicio = new Label();
            lblFechaFin = new Label();
            cbTipoAnulacion = new ComboBox();
            dtpFechaInicio = new DateTimePicker();
            dtpFechaFin = new DateTimePicker();
            txtComentario = new TextBox();
            btnRegresar = new Button();
            btnGuardar = new Button();
            btnEliminar = new Button();
            lblEstadoAnulacion = new Label();
            gbInformacion = new GroupBox();
            gbDatosAnulacion = new GroupBox();
            gbComentario = new GroupBox();
            ttAyuda = new ToolTip(components);
            lblResumenAnulacion = new Label();
            gbInformacion.SuspendLayout();
            gbDatosAnulacion.SuspendLayout();
            gbComentario.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(15, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(146, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Anulación de certificación";
            // 
            // lblTrabajador
            // 
            lblTrabajador.AutoSize = true;
            lblTrabajador.Location = new Point(15, 48);
            lblTrabajador.Name = "lblTrabajador";
            lblTrabajador.Size = new Size(70, 15);
            lblTrabajador.TabIndex = 1;
            lblTrabajador.Text = "Trabajador:";
            // 
            // lblProceso
            // 
            lblProceso.AutoSize = true;
            lblProceso.Location = new Point(15, 72);
            lblProceso.Name = "lblProceso";
            lblProceso.Size = new Size(56, 15);
            lblProceso.TabIndex = 2;
            lblProceso.Text = "Proceso:";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(25, 32);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(105, 15);
            lblTipo.TabIndex = 3;
            lblTipo.Text = "Tipo de anulación";
            // 
            // lblFechaInicio
            // 
            lblFechaInicio.AutoSize = true;
            lblFechaInicio.Location = new Point(25, 70);
            lblFechaInicio.Name = "lblFechaInicio";
            lblFechaInicio.Size = new Size(76, 15);
            lblFechaInicio.TabIndex = 4;
            lblFechaInicio.Text = "Fecha inicio:";
            // 
            // lblFechaFin
            // 
            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new Point(25, 105);
            lblFechaFin.Name = "lblFechaFin";
            lblFechaFin.Size = new Size(60, 15);
            lblFechaFin.TabIndex = 5;
            lblFechaFin.Text = "Fecha fin:";
            // 
            // cbTipoAnulacion
            // 
            cbTipoAnulacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipoAnulacion.FormattingEnabled = true;
            cbTipoAnulacion.Location = new Point(150, 28);
            cbTipoAnulacion.Name = "cbTipoAnulacion";
            cbTipoAnulacion.Size = new Size(320, 23);
            cbTipoAnulacion.TabIndex = 7;
            cbTipoAnulacion.SelectedIndexChanged += cmbTipoAnulacion_SelectedIndexChanged;
            // 
            // dtpFechaInicio
            // 
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Location = new Point(150, 66);
            dtpFechaInicio.Name = "dtpFechaInicio";
            dtpFechaInicio.Size = new Size(150, 21);
            dtpFechaInicio.TabIndex = 8;
            dtpFechaInicio.ValueChanged += dtpFechaInicio_ValueChanged;
            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Location = new Point(150, 101);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.Size = new Size(150, 21);
            dtpFechaFin.TabIndex = 9;
            dtpFechaFin.ValueChanged += dtpFechaFin_ValueChanged;
            // 
            // txtComentario
            // 
            txtComentario.Location = new Point(15, 22);
            txtComentario.MaxLength = 500;
            txtComentario.Multiline = true;
            txtComentario.Name = "txtComentario";
            txtComentario.ScrollBars = ScrollBars.Vertical;
            txtComentario.Size = new Size(490, 45);
            txtComentario.TabIndex = 10;
            txtComentario.TextChanged += txtComentario_TextChanged;
            // 
            // btnRegresar
            // 
            btnRegresar.BackColor = SystemColors.ButtonFace;
            btnRegresar.Location = new Point(344, 408);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 12;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = false;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(230, 240, 255);
            btnGuardar.Location = new Point(104, 408);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(88, 39);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(255, 235, 235);
            btnEliminar.Location = new Point(224, 408);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(88, 39);
            btnEliminar.TabIndex = 13;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // lblEstadoAnulacion
            // 
            lblEstadoAnulacion.BorderStyle = BorderStyle.FixedSingle;
            lblEstadoAnulacion.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstadoAnulacion.ForeColor = Color.DarkGreen;
            lblEstadoAnulacion.Location = new Point(324, 12);
            lblEstadoAnulacion.Name = "lblEstadoAnulacion";
            lblEstadoAnulacion.Size = new Size(190, 34);
            lblEstadoAnulacion.TabIndex = 14;
            lblEstadoAnulacion.Text = "Estado: Sin anulación activa";
            lblEstadoAnulacion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbInformacion
            // 
            gbInformacion.Controls.Add(lblTitulo);
            gbInformacion.Controls.Add(lblEstadoAnulacion);
            gbInformacion.Controls.Add(lblTrabajador);
            gbInformacion.Controls.Add(lblProceso);
            gbInformacion.Location = new Point(12, 12);
            gbInformacion.Name = "gbInformacion";
            gbInformacion.Size = new Size(520, 105);
            gbInformacion.TabIndex = 15;
            gbInformacion.TabStop = false;
            gbInformacion.Text = "Información de la certificación";
            // 
            // gbDatosAnulacion
            // 
            gbDatosAnulacion.Controls.Add(lblTipo);
            gbDatosAnulacion.Controls.Add(cbTipoAnulacion);
            gbDatosAnulacion.Controls.Add(lblFechaInicio);
            gbDatosAnulacion.Controls.Add(dtpFechaInicio);
            gbDatosAnulacion.Controls.Add(lblFechaFin);
            gbDatosAnulacion.Controls.Add(dtpFechaFin);
            gbDatosAnulacion.Location = new Point(12, 125);
            gbDatosAnulacion.Name = "gbDatosAnulacion";
            gbDatosAnulacion.Size = new Size(520, 140);
            gbDatosAnulacion.TabIndex = 15;
            gbDatosAnulacion.TabStop = false;
            gbDatosAnulacion.Text = "Datos de anulación";
            // 
            // gbComentario
            // 
            gbComentario.Controls.Add(txtComentario);
            gbComentario.Location = new Point(12, 273);
            gbComentario.Name = "gbComentario";
            gbComentario.Size = new Size(520, 80);
            gbComentario.TabIndex = 10;
            gbComentario.TabStop = false;
            gbComentario.Text = "Motivo de la anulación";
            // 
            // lblResumenAnulacion
            // 
            lblResumenAnulacion.BackColor = Color.FromArgb(250, 250, 250);
            lblResumenAnulacion.BorderStyle = BorderStyle.FixedSingle;
            lblResumenAnulacion.Location = new Point(12, 355);
            lblResumenAnulacion.Name = "lblResumenAnulacion";
            lblResumenAnulacion.Size = new Size(520, 50);
            lblResumenAnulacion.TabIndex = 15;
            lblResumenAnulacion.Text = "Resumen: Sin datos capturados";
            lblResumenAnulacion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CertificacionAnulacionVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(544, 454);
            Controls.Add(lblResumenAnulacion);
            Controls.Add(gbComentario);
            Controls.Add(gbDatosAnulacion);
            Controls.Add(gbInformacion);
            Controls.Add(btnEliminar);
            Controls.Add(btnRegresar);
            Controls.Add(btnGuardar);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CertificacionAnulacionVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Anulación de certificación";
            Load += CertificacionAnulacionVentana_Load;
            gbInformacion.ResumeLayout(false);
            gbInformacion.PerformLayout();
            gbDatosAnulacion.ResumeLayout(false);
            gbDatosAnulacion.PerformLayout();
            gbComentario.ResumeLayout(false);
            gbComentario.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblTrabajador;
        private Label lblProceso;
        private Label lblTipo;
        private Label lblFechaInicio;
        private Label lblFechaFin;
        private ComboBox cbTipoAnulacion;
        private DateTimePicker dtpFechaInicio;
        private DateTimePicker dtpFechaFin;
        private TextBox txtComentario;
        private Button btnRegresar;
        private Button btnGuardar;
        private Button btnEliminar;
        private Label lblEstadoAnulacion;
        private GroupBox gbInformacion;
        private GroupBox gbDatosAnulacion;
        private GroupBox gbComentario;
        private ToolTip ttAyuda;
        private Label lblResumenAnulacion;
    }
}