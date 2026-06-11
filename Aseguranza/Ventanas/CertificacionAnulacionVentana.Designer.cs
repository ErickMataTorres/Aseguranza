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
            lblTitulo = new Label();
            lblTrabajador = new Label();
            lblProceso = new Label();
            lblTipo = new Label();
            lblFechaInicio = new Label();
            lblFechaFin = new Label();
            lblComentario = new Label();
            cbTipoAnulacion = new ComboBox();
            dtpFechaInicio = new DateTimePicker();
            dtpFechaFin = new DateTimePicker();
            txtComentario = new TextBox();
            btnRegresar = new Button();
            btnGuardar = new Button();
            btnEliminar = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(150, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(146, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Anulación de certificación";
            // 
            // lblTrabajador
            // 
            lblTrabajador.AutoSize = true;
            lblTrabajador.Location = new Point(176, 44);
            lblTrabajador.Name = "lblTrabajador";
            lblTrabajador.Size = new Size(70, 15);
            lblTrabajador.TabIndex = 1;
            lblTrabajador.Text = "Trabajador:";
            // 
            // lblProceso
            // 
            lblProceso.AutoSize = true;
            lblProceso.Location = new Point(176, 76);
            lblProceso.Name = "lblProceso";
            lblProceso.Size = new Size(56, 15);
            lblProceso.TabIndex = 2;
            lblProceso.Text = "Proceso:";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(39, 117);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(105, 15);
            lblTipo.TabIndex = 3;
            lblTipo.Text = "Tipo de anulación";
            // 
            // lblFechaInicio
            // 
            lblFechaInicio.AutoSize = true;
            lblFechaInicio.Location = new Point(68, 162);
            lblFechaInicio.Name = "lblFechaInicio";
            lblFechaInicio.Size = new Size(76, 15);
            lblFechaInicio.TabIndex = 4;
            lblFechaInicio.Text = "Fecha inicio:";
            // 
            // lblFechaFin
            // 
            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new Point(84, 200);
            lblFechaFin.Name = "lblFechaFin";
            lblFechaFin.Size = new Size(60, 15);
            lblFechaFin.TabIndex = 5;
            lblFechaFin.Text = "Fecha fin:";
            // 
            // lblComentario
            // 
            lblComentario.AutoSize = true;
            lblComentario.Location = new Point(69, 235);
            lblComentario.Name = "lblComentario";
            lblComentario.Size = new Size(75, 15);
            lblComentario.TabIndex = 6;
            lblComentario.Text = "Comentario:";
            // 
            // cbTipoAnulacion
            // 
            cbTipoAnulacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipoAnulacion.FormattingEnabled = true;
            cbTipoAnulacion.Location = new Point(150, 114);
            cbTipoAnulacion.Name = "cbTipoAnulacion";
            cbTipoAnulacion.Size = new Size(266, 23);
            cbTipoAnulacion.TabIndex = 7;
            cbTipoAnulacion.SelectedIndexChanged += cmbTipoAnulacion_SelectedIndexChanged;
            // 
            // dtpFechaInicio
            // 
            dtpFechaInicio.Format = DateTimePickerFormat.Short;
            dtpFechaInicio.Location = new Point(150, 157);
            dtpFechaInicio.Name = "dtpFechaInicio";
            dtpFechaInicio.Size = new Size(183, 21);
            dtpFechaInicio.TabIndex = 8;
            dtpFechaInicio.ValueChanged += dtpFechaInicio_ValueChanged;
            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Format = DateTimePickerFormat.Short;
            dtpFechaFin.Location = new Point(150, 195);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.Size = new Size(183, 21);
            dtpFechaFin.TabIndex = 9;
            // 
            // txtComentario
            // 
            txtComentario.Location = new Point(150, 232);
            txtComentario.Multiline = true;
            txtComentario.Name = "txtComentario";
            txtComentario.Size = new Size(246, 65);
            txtComentario.TabIndex = 10;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(289, 318);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 12;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(101, 318);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(88, 39);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(195, 318);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(88, 39);
            btnEliminar.TabIndex = 13;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // CertificacionAnulacionVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 400);
            Controls.Add(btnEliminar);
            Controls.Add(btnRegresar);
            Controls.Add(btnGuardar);
            Controls.Add(txtComentario);
            Controls.Add(dtpFechaFin);
            Controls.Add(dtpFechaInicio);
            Controls.Add(cbTipoAnulacion);
            Controls.Add(lblComentario);
            Controls.Add(lblFechaFin);
            Controls.Add(lblFechaInicio);
            Controls.Add(lblTipo);
            Controls.Add(lblProceso);
            Controls.Add(lblTrabajador);
            Controls.Add(lblTitulo);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CertificacionAnulacionVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CertificacionAnulacionVentana";
            Load += CertificacionAnulacionVentana_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblTrabajador;
        private Label lblProceso;
        private Label lblTipo;
        private Label lblFechaInicio;
        private Label lblFechaFin;
        private Label lblComentario;
        private ComboBox cbTipoAnulacion;
        private DateTimePicker dtpFechaInicio;
        private DateTimePicker dtpFechaFin;
        private TextBox txtComentario;
        private Button btnRegresar;
        private Button btnGuardar;
        private Button btnEliminar;
    }
}