namespace Aseguranza.Ventanas
{
    partial class CertificacionesVentana
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
            pictureBox1 = new PictureBox();
            lblMostrarLinea = new Label();
            lblMostrarPlanta = new Label();
            lblMostrarTurno = new Label();
            lblMostrarLocalidad = new Label();
            lblMostrarNombre = new Label();
            lblPlanta = new Label();
            lblLinea = new Label();
            lblTurno = new Label();
            lblLocalidad = new Label();
            lblNombre = new Label();
            lblNoReloj = new Label();
            lblMostrarNoReloj = new Label();
            dgvCertificaciones = new DataGridView();
            btnRegresar = new Button();
            btnBorrar = new Button();
            btnModificar = new Button();
            btnAgregar = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            pbContec = new PictureBox();
            lblVerificador = new Label();
            btnImprimir = new Button();
            btnExpediente = new Button();
            btnAnular = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCertificaciones).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbContec).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(728, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(305, 209);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 45;
            pictureBox1.TabStop = false;
            // 
            // lblMostrarLinea
            // 
            lblMostrarLinea.AutoSize = true;
            lblMostrarLinea.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarLinea.ForeColor = SystemColors.ControlText;
            lblMostrarLinea.Location = new Point(84, 203);
            lblMostrarLinea.Name = "lblMostrarLinea";
            lblMostrarLinea.Size = new Size(0, 18);
            lblMostrarLinea.TabIndex = 44;
            // 
            // lblMostrarPlanta
            // 
            lblMostrarPlanta.AutoSize = true;
            lblMostrarPlanta.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarPlanta.ForeColor = SystemColors.ControlText;
            lblMostrarPlanta.Location = new Point(84, 174);
            lblMostrarPlanta.Name = "lblMostrarPlanta";
            lblMostrarPlanta.Size = new Size(0, 18);
            lblMostrarPlanta.TabIndex = 43;
            // 
            // lblMostrarTurno
            // 
            lblMostrarTurno.AutoSize = true;
            lblMostrarTurno.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarTurno.ForeColor = SystemColors.ControlText;
            lblMostrarTurno.Location = new Point(84, 146);
            lblMostrarTurno.Name = "lblMostrarTurno";
            lblMostrarTurno.Size = new Size(0, 18);
            lblMostrarTurno.TabIndex = 42;
            // 
            // lblMostrarLocalidad
            // 
            lblMostrarLocalidad.AutoSize = true;
            lblMostrarLocalidad.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarLocalidad.ForeColor = SystemColors.ControlText;
            lblMostrarLocalidad.Location = new Point(84, 121);
            lblMostrarLocalidad.Name = "lblMostrarLocalidad";
            lblMostrarLocalidad.Size = new Size(0, 18);
            lblMostrarLocalidad.TabIndex = 41;
            // 
            // lblMostrarNombre
            // 
            lblMostrarNombre.AutoSize = true;
            lblMostrarNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNombre.ForeColor = SystemColors.ControlText;
            lblMostrarNombre.Location = new Point(84, 94);
            lblMostrarNombre.Name = "lblMostrarNombre";
            lblMostrarNombre.Size = new Size(0, 18);
            lblMostrarNombre.TabIndex = 40;
            // 
            // lblPlanta
            // 
            lblPlanta.AutoSize = true;
            lblPlanta.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblPlanta.Location = new Point(25, 174);
            lblPlanta.Name = "lblPlanta";
            lblPlanta.Size = new Size(56, 18);
            lblPlanta.TabIndex = 39;
            lblPlanta.Text = "Planta:";
            // 
            // lblLinea
            // 
            lblLinea.AutoSize = true;
            lblLinea.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblLinea.Location = new Point(31, 203);
            lblLinea.Name = "lblLinea";
            lblLinea.Size = new Size(51, 18);
            lblLinea.TabIndex = 38;
            lblLinea.Text = "Linea:";
            // 
            // lblTurno
            // 
            lblTurno.AutoSize = true;
            lblTurno.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblTurno.Location = new Point(29, 147);
            lblTurno.Name = "lblTurno";
            lblTurno.Size = new Size(54, 18);
            lblTurno.TabIndex = 37;
            lblTurno.Text = "Turno:";
            // 
            // lblLocalidad
            // 
            lblLocalidad.AutoSize = true;
            lblLocalidad.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblLocalidad.Location = new Point(4, 121);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(80, 18);
            lblLocalidad.TabIndex = 36;
            lblLocalidad.Text = "Localidad:";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblNombre.Location = new Point(14, 95);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(68, 18);
            lblNombre.TabIndex = 35;
            lblNombre.Text = "Nombre:";
            // 
            // lblNoReloj
            // 
            lblNoReloj.AutoSize = true;
            lblNoReloj.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblNoReloj.Location = new Point(7, 67);
            lblNoReloj.Name = "lblNoReloj";
            lblNoReloj.Size = new Size(77, 18);
            lblNoReloj.TabIndex = 34;
            lblNoReloj.Text = "No. Reloj:";
            // 
            // lblMostrarNoReloj
            // 
            lblMostrarNoReloj.AutoSize = true;
            lblMostrarNoReloj.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNoReloj.ForeColor = SystemColors.ControlText;
            lblMostrarNoReloj.Location = new Point(84, 66);
            lblMostrarNoReloj.Name = "lblMostrarNoReloj";
            lblMostrarNoReloj.Size = new Size(0, 18);
            lblMostrarNoReloj.TabIndex = 46;
            // 
            // dgvCertificaciones
            // 
            dgvCertificaciones.AllowUserToAddRows = false;
            dgvCertificaciones.AllowUserToDeleteRows = false;
            dgvCertificaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCertificaciones.Location = new Point(11, 265);
            dgvCertificaciones.Name = "dgvCertificaciones";
            dgvCertificaciones.ReadOnly = true;
            dgvCertificaciones.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCertificaciones.Size = new Size(1022, 236);
            dgvCertificaciones.TabIndex = 47;
            dgvCertificaciones.TabStop = false;
            dgvCertificaciones.CellDoubleClick += dgvCertificaciones_CellDoubleClick;
            dgvCertificaciones.DataBindingComplete += dgvCertificaciones_DataBindingComplete_1;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(945, 507);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 5;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(200, 507);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 3;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            btnBorrar.Click += btnBorrar_Click_1;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(106, 507);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 39);
            btnModificar.TabIndex = 2;
            btnModificar.Text = "Modificar / Renovar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click_1;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(12, 507);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 1;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click_1;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(65, 238);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(968, 21);
            txtBuscar.TabIndex = 0;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(10, 241);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 53;
            lblBuscar.Text = "Buscar:";
            // 
            // pbContec
            // 
            pbContec.BorderStyle = BorderStyle.FixedSingle;
            pbContec.Image = Properties.Resources.Contec;
            pbContec.Location = new Point(21, 4);
            pbContec.Name = "pbContec";
            pbContec.Size = new Size(60, 56);
            pbContec.SizeMode = PictureBoxSizeMode.StretchImage;
            pbContec.TabIndex = 54;
            pbContec.TabStop = false;
            // 
            // lblVerificador
            // 
            lblVerificador.AutoSize = true;
            lblVerificador.BackColor = Color.Navy;
            lblVerificador.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblVerificador.ForeColor = Color.White;
            lblVerificador.Location = new Point(106, 12);
            lblVerificador.Name = "lblVerificador";
            lblVerificador.Size = new Size(469, 37);
            lblVerificador.TabIndex = 55;
            lblVerificador.Text = "Verificador de certificaciones";
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(599, 12);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(88, 39);
            btnImprimir.TabIndex = 56;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += button1_Click;
            // 
            // btnExpediente
            // 
            btnExpediente.Location = new Point(294, 507);
            btnExpediente.Name = "btnExpediente";
            btnExpediente.Size = new Size(88, 39);
            btnExpediente.TabIndex = 57;
            btnExpediente.Text = "Expediente";
            btnExpediente.UseVisualStyleBackColor = true;
            btnExpediente.Click += btnExpediente_Click;
            // 
            // btnAnular
            // 
            btnAnular.Location = new Point(388, 507);
            btnAnular.Name = "btnAnular";
            btnAnular.Size = new Size(88, 39);
            btnAnular.TabIndex = 58;
            btnAnular.Text = "Anular";
            btnAnular.UseVisualStyleBackColor = true;
            btnAnular.Click += btnAnular_Click;
            // 
            // CertificacionesVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 558);
            Controls.Add(btnAnular);
            Controls.Add(btnExpediente);
            Controls.Add(btnImprimir);
            Controls.Add(lblVerificador);
            Controls.Add(pbContec);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(btnBorrar);
            Controls.Add(btnModificar);
            Controls.Add(btnAgregar);
            Controls.Add(btnRegresar);
            Controls.Add(dgvCertificaciones);
            Controls.Add(lblMostrarNoReloj);
            Controls.Add(pictureBox1);
            Controls.Add(lblMostrarLinea);
            Controls.Add(lblMostrarPlanta);
            Controls.Add(lblMostrarTurno);
            Controls.Add(lblMostrarLocalidad);
            Controls.Add(lblMostrarNombre);
            Controls.Add(lblPlanta);
            Controls.Add(lblLinea);
            Controls.Add(lblTurno);
            Controls.Add(lblLocalidad);
            Controls.Add(lblNombre);
            Controls.Add(lblNoReloj);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CertificacionesVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Certificaciones";
            Load += CertificacionesVentana_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCertificaciones).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbContec).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblMostrarLinea;
        private Label lblMostrarPlanta;
        private Label lblMostrarTurno;
        private Label lblMostrarLocalidad;
        private Label lblMostrarNombre;
        private Label lblPlanta;
        private Label lblLinea;
        private Label lblTurno;
        private Label lblLocalidad;
        private Label lblNombre;
        private Label lblNoReloj;
        private Label lblMostrarNoReloj;
        private DataGridView dgvCertificaciones;
        private Button btnRegresar;
        private Button btnBorrar;
        private Button btnModificar;
        private Button btnAgregar;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private PictureBox pbContec;
        private Label lblVerificador;
        private Button btnImprimir;
        private Button btnExpediente;
        private Button btnAnular;
    }
}