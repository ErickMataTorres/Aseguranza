namespace Aseguranza.Ventanas
{
    partial class CertificacionesTrabajadorVentana
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
            lblProceso = new Label();
            cbProcesos = new ComboBox();
            dtpFechaCertificacion = new DateTimePicker();
            lblFechaCertificacion = new Label();
            lblFechaVencimiento = new Label();
            dtpFechaVencimiento = new DateTimePicker();
            cbCertificadores = new ComboBox();
            lblCertificador = new Label();
            lblComentario = new Label();
            txtComentario = new TextBox();
            btnRegresar = new Button();
            btnAceptar = new Button();
            SuspendLayout();
            // 
            // lblProceso
            // 
            lblProceso.AutoSize = true;
            lblProceso.Location = new Point(83, 34);
            lblProceso.Name = "lblProceso";
            lblProceso.Size = new Size(56, 15);
            lblProceso.TabIndex = 0;
            lblProceso.Text = "Proceso:";
            // 
            // cbProcesos
            // 
            cbProcesos.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProcesos.FormattingEnabled = true;
            cbProcesos.Location = new Point(145, 31);
            cbProcesos.Name = "cbProcesos";
            cbProcesos.Size = new Size(271, 23);
            cbProcesos.TabIndex = 0;
            cbProcesos.SelectedIndexChanged += cbProcesos_SelectedIndexChanged;
            // 
            // dtpFechaCertificacion
            // 
            dtpFechaCertificacion.Location = new Point(145, 72);
            dtpFechaCertificacion.Name = "dtpFechaCertificacion";
            dtpFechaCertificacion.Size = new Size(271, 21);
            dtpFechaCertificacion.TabIndex = 1;
            dtpFechaCertificacion.ValueChanged += dtpFechaCertificacion_ValueChanged;
            // 
            // lblFechaCertificacion
            // 
            lblFechaCertificacion.AutoSize = true;
            lblFechaCertificacion.Location = new Point(27, 77);
            lblFechaCertificacion.Name = "lblFechaCertificacion";
            lblFechaCertificacion.Size = new Size(112, 15);
            lblFechaCertificacion.TabIndex = 3;
            lblFechaCertificacion.Text = "Fecha certificación:";
            // 
            // lblFechaVencimiento
            // 
            lblFechaVencimiento.AutoSize = true;
            lblFechaVencimiento.Location = new Point(26, 121);
            lblFechaVencimiento.Name = "lblFechaVencimiento";
            lblFechaVencimiento.Size = new Size(113, 15);
            lblFechaVencimiento.TabIndex = 5;
            lblFechaVencimiento.Text = "Fecha vencimiento:";
            // 
            // dtpFechaVencimiento
            // 
            dtpFechaVencimiento.Enabled = false;
            dtpFechaVencimiento.Location = new Point(145, 116);
            dtpFechaVencimiento.Name = "dtpFechaVencimiento";
            dtpFechaVencimiento.Size = new Size(271, 21);
            dtpFechaVencimiento.TabIndex = 4;
            dtpFechaVencimiento.TabStop = false;
            // 
            // cbCertificadores
            // 
            cbCertificadores.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCertificadores.FormattingEnabled = true;
            cbCertificadores.ItemHeight = 15;
            cbCertificadores.Location = new Point(145, 161);
            cbCertificadores.Name = "cbCertificadores";
            cbCertificadores.Size = new Size(271, 23);
            cbCertificadores.TabIndex = 2;
            // 
            // lblCertificador
            // 
            lblCertificador.AutoSize = true;
            lblCertificador.Location = new Point(66, 164);
            lblCertificador.Name = "lblCertificador";
            lblCertificador.Size = new Size(73, 15);
            lblCertificador.TabIndex = 6;
            lblCertificador.Text = "Certificador:";
            // 
            // lblComentario
            // 
            lblComentario.AutoSize = true;
            lblComentario.Location = new Point(64, 207);
            lblComentario.Name = "lblComentario";
            lblComentario.Size = new Size(75, 15);
            lblComentario.TabIndex = 8;
            lblComentario.Text = "Comentario:";
            // 
            // txtComentario
            // 
            txtComentario.Location = new Point(145, 204);
            txtComentario.Multiline = true;
            txtComentario.Name = "txtComentario";
            txtComentario.Size = new Size(271, 91);
            txtComentario.TabIndex = 3;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(223, 315);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 5;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(129, 315);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 4;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // CertificacionesTrabajadorVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(490, 376);
            Controls.Add(btnRegresar);
            Controls.Add(btnAceptar);
            Controls.Add(txtComentario);
            Controls.Add(lblComentario);
            Controls.Add(cbCertificadores);
            Controls.Add(lblCertificador);
            Controls.Add(lblFechaVencimiento);
            Controls.Add(dtpFechaVencimiento);
            Controls.Add(lblFechaCertificacion);
            Controls.Add(dtpFechaCertificacion);
            Controls.Add(cbProcesos);
            Controls.Add(lblProceso);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CertificacionesTrabajadorVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Certificación";
            Load += CertificacionesTrabajadorVentana_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProceso;
        private ComboBox cbProcesos;
        private DateTimePicker dtpFechaCertificacion;
        private Label lblFechaCertificacion;
        private Label lblFechaVencimiento;
        private DateTimePicker dtpFechaVencimiento;
        private ComboBox cbCertificadores;
        private Label lblCertificador;
        private Label lblComentario;
        private TextBox txtComentario;
        private Button btnRegresar;
        private Button btnAceptar;
    }
}