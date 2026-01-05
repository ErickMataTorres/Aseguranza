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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCertificaciones).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(592, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(272, 209);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 45;
            pictureBox1.TabStop = false;
            // 
            // lblMostrarLinea
            // 
            lblMostrarLinea.AutoSize = true;
            lblMostrarLinea.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarLinea.ForeColor = SystemColors.ControlText;
            lblMostrarLinea.Location = new Point(90, 161);
            lblMostrarLinea.Name = "lblMostrarLinea";
            lblMostrarLinea.Size = new Size(0, 18);
            lblMostrarLinea.TabIndex = 44;
            // 
            // lblMostrarPlanta
            // 
            lblMostrarPlanta.AutoSize = true;
            lblMostrarPlanta.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarPlanta.ForeColor = SystemColors.ControlText;
            lblMostrarPlanta.Location = new Point(90, 132);
            lblMostrarPlanta.Name = "lblMostrarPlanta";
            lblMostrarPlanta.Size = new Size(0, 18);
            lblMostrarPlanta.TabIndex = 43;
            // 
            // lblMostrarTurno
            // 
            lblMostrarTurno.AutoSize = true;
            lblMostrarTurno.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarTurno.ForeColor = SystemColors.ControlText;
            lblMostrarTurno.Location = new Point(90, 104);
            lblMostrarTurno.Name = "lblMostrarTurno";
            lblMostrarTurno.Size = new Size(0, 18);
            lblMostrarTurno.TabIndex = 42;
            // 
            // lblMostrarLocalidad
            // 
            lblMostrarLocalidad.AutoSize = true;
            lblMostrarLocalidad.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarLocalidad.ForeColor = SystemColors.ControlText;
            lblMostrarLocalidad.Location = new Point(90, 79);
            lblMostrarLocalidad.Name = "lblMostrarLocalidad";
            lblMostrarLocalidad.Size = new Size(0, 18);
            lblMostrarLocalidad.TabIndex = 41;
            // 
            // lblMostrarNombre
            // 
            lblMostrarNombre.AutoSize = true;
            lblMostrarNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNombre.ForeColor = SystemColors.ControlText;
            lblMostrarNombre.Location = new Point(90, 52);
            lblMostrarNombre.Name = "lblMostrarNombre";
            lblMostrarNombre.Size = new Size(0, 18);
            lblMostrarNombre.TabIndex = 40;
            // 
            // lblPlanta
            // 
            lblPlanta.AutoSize = true;
            lblPlanta.Font = new Font("Arial", 11.25F);
            lblPlanta.Location = new Point(31, 132);
            lblPlanta.Name = "lblPlanta";
            lblPlanta.Size = new Size(53, 17);
            lblPlanta.TabIndex = 39;
            lblPlanta.Text = "Planta:";
            // 
            // lblLinea
            // 
            lblLinea.AutoSize = true;
            lblLinea.Font = new Font("Arial", 11.25F);
            lblLinea.Location = new Point(37, 161);
            lblLinea.Name = "lblLinea";
            lblLinea.Size = new Size(47, 17);
            lblLinea.TabIndex = 38;
            lblLinea.Text = "Linea:";
            // 
            // lblTurno
            // 
            lblTurno.AutoSize = true;
            lblTurno.Font = new Font("Arial", 11.25F);
            lblTurno.Location = new Point(35, 105);
            lblTurno.Name = "lblTurno";
            lblTurno.Size = new Size(49, 17);
            lblTurno.TabIndex = 37;
            lblTurno.Text = "Turno:";
            // 
            // lblLocalidad
            // 
            lblLocalidad.AutoSize = true;
            lblLocalidad.Font = new Font("Arial", 11.25F);
            lblLocalidad.Location = new Point(10, 79);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(74, 17);
            lblLocalidad.TabIndex = 36;
            lblLocalidad.Text = "Localidad:";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Arial", 11.25F);
            lblNombre.Location = new Point(20, 53);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 17);
            lblNombre.TabIndex = 35;
            lblNombre.Text = "Nombre:";
            // 
            // lblNoReloj
            // 
            lblNoReloj.AutoSize = true;
            lblNoReloj.Font = new Font("Arial", 11.25F);
            lblNoReloj.Location = new Point(13, 25);
            lblNoReloj.Name = "lblNoReloj";
            lblNoReloj.Size = new Size(71, 17);
            lblNoReloj.TabIndex = 34;
            lblNoReloj.Text = "No. Reloj:";
            // 
            // lblMostrarNoReloj
            // 
            lblMostrarNoReloj.AutoSize = true;
            lblMostrarNoReloj.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNoReloj.ForeColor = SystemColors.ControlText;
            lblMostrarNoReloj.Location = new Point(90, 24);
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
            dgvCertificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCertificaciones.Size = new Size(853, 193);
            dgvCertificaciones.TabIndex = 47;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(776, 464);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 48;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(198, 464);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 51;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(104, 464);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 39);
            btnModificar.TabIndex = 50;
            btnModificar.Text = "Modificar / Renovar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click_1;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(10, 464);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 49;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(65, 238);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(799, 21);
            txtBuscar.TabIndex = 52;
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
            // CertificacionesVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(876, 511);
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
    }
}