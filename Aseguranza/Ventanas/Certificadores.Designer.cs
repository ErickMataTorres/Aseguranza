namespace Aseguranza.Ventanas
{
    partial class Certificadores
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
            btnRegresar = new Button();
            btnBorrar = new Button();
            btnAgregar = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            dgvCertificadores = new DataGridView();
            lblPlanta = new Label();
            lblLinea = new Label();
            lblTurno = new Label();
            lblLocalidad = new Label();
            lblNombre = new Label();
            txtNoReloj = new TextBox();
            lblNoReloj = new Label();
            lblMostrarNombre = new Label();
            lblMostrarLocalidad = new Label();
            lblMostrarTurno = new Label();
            lblMostrarPlanta = new Label();
            lblMostrarLinea = new Label();
            pictureBox1 = new PictureBox();
            btnBuscar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCertificadores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(722, 478);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 18;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(12, 478);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 17;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(370, 182);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 14;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(67, 229);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(743, 21);
            txtBuscar.TabIndex = 12;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 232);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 15;
            lblBuscar.Text = "Buscar:";
            // 
            // dgvCertificadores
            // 
            dgvCertificadores.AllowUserToAddRows = false;
            dgvCertificadores.AllowUserToDeleteRows = false;
            dgvCertificadores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCertificadores.Location = new Point(12, 260);
            dgvCertificadores.MultiSelect = false;
            dgvCertificadores.Name = "dgvCertificadores";
            dgvCertificadores.ReadOnly = true;
            dgvCertificadores.RowHeadersWidth = 51;
            dgvCertificadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCertificadores.Size = new Size(798, 212);
            dgvCertificadores.TabIndex = 13;
            dgvCertificadores.TabStop = false;
            // 
            // lblPlanta
            // 
            lblPlanta.AutoSize = true;
            lblPlanta.Location = new Point(30, 131);
            lblPlanta.Name = "lblPlanta";
            lblPlanta.Size = new Size(45, 15);
            lblPlanta.TabIndex = 26;
            lblPlanta.Text = "Planta:";
            // 
            // lblLinea
            // 
            lblLinea.AutoSize = true;
            lblLinea.Location = new Point(36, 160);
            lblLinea.Name = "lblLinea";
            lblLinea.Size = new Size(41, 15);
            lblLinea.TabIndex = 25;
            lblLinea.Text = "Linea:";
            // 
            // lblTurno
            // 
            lblTurno.AutoSize = true;
            lblTurno.Location = new Point(34, 104);
            lblTurno.Name = "lblTurno";
            lblTurno.Size = new Size(42, 15);
            lblTurno.TabIndex = 24;
            lblTurno.Text = "Turno:";
            // 
            // lblLocalidad
            // 
            lblLocalidad.AutoSize = true;
            lblLocalidad.Location = new Point(9, 78);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(64, 15);
            lblLocalidad.TabIndex = 23;
            lblLocalidad.Text = "Localidad:";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(19, 51);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(55, 15);
            lblNombre.TabIndex = 21;
            lblNombre.Text = "Nombre:";
            // 
            // txtNoReloj
            // 
            txtNoReloj.Location = new Point(89, 17);
            txtNoReloj.Name = "txtNoReloj";
            txtNoReloj.Size = new Size(275, 21);
            txtNoReloj.TabIndex = 20;
            txtNoReloj.KeyPress += txtNoReloj_KeyPress;
            // 
            // lblNoReloj
            // 
            lblNoReloj.AutoSize = true;
            lblNoReloj.Location = new Point(12, 20);
            lblNoReloj.Name = "lblNoReloj";
            lblNoReloj.Size = new Size(61, 15);
            lblNoReloj.TabIndex = 19;
            lblNoReloj.Text = "No. Reloj:";
            // 
            // lblMostrarNombre
            // 
            lblMostrarNombre.AutoSize = true;
            lblMostrarNombre.Location = new Point(89, 51);
            lblMostrarNombre.Name = "lblMostrarNombre";
            lblMostrarNombre.Size = new Size(106, 15);
            lblMostrarNombre.TabIndex = 27;
            lblMostrarNombre.Text = "lblMostrarNombre";
            // 
            // lblMostrarLocalidad
            // 
            lblMostrarLocalidad.AutoSize = true;
            lblMostrarLocalidad.Location = new Point(89, 78);
            lblMostrarLocalidad.Name = "lblMostrarLocalidad";
            lblMostrarLocalidad.Size = new Size(115, 15);
            lblMostrarLocalidad.TabIndex = 28;
            lblMostrarLocalidad.Text = "lblMostrarLocalidad";
            // 
            // lblMostrarTurno
            // 
            lblMostrarTurno.AutoSize = true;
            lblMostrarTurno.Location = new Point(89, 104);
            lblMostrarTurno.Name = "lblMostrarTurno";
            lblMostrarTurno.Size = new Size(93, 15);
            lblMostrarTurno.TabIndex = 29;
            lblMostrarTurno.Text = "lblMostrarTurno";
            // 
            // lblMostrarPlanta
            // 
            lblMostrarPlanta.AutoSize = true;
            lblMostrarPlanta.Location = new Point(89, 131);
            lblMostrarPlanta.Name = "lblMostrarPlanta";
            lblMostrarPlanta.Size = new Size(96, 15);
            lblMostrarPlanta.TabIndex = 30;
            lblMostrarPlanta.Text = "lblMostrarPlanta";
            // 
            // lblMostrarLinea
            // 
            lblMostrarLinea.AutoSize = true;
            lblMostrarLinea.Location = new Point(89, 160);
            lblMostrarLinea.Name = "lblMostrarLinea";
            lblMostrarLinea.Size = new Size(92, 15);
            lblMostrarLinea.TabIndex = 31;
            lblMostrarLinea.Text = "lblMostrarLinea";
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(538, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(272, 209);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 32;
            pictureBox1.TabStop = false;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(370, 12);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(88, 39);
            btnBuscar.TabIndex = 33;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // Certificadores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 532);
            Controls.Add(btnBuscar);
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
            Controls.Add(txtNoReloj);
            Controls.Add(lblNoReloj);
            Controls.Add(btnRegresar);
            Controls.Add(btnBorrar);
            Controls.Add(btnAgregar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(dgvCertificadores);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Certificadores";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Certificadores";
            Load += Certificadores_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCertificadores).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRegresar;
        private Button btnBorrar;
        private Button btnAgregar;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private DataGridView dgvCertificadores;
        private Label lblPlanta;
        private Label lblLinea;
        private Label lblTurno;
        private Label lblLocalidad;
        private Label lblNombre;
        private TextBox txtNoReloj;
        private Label lblNoReloj;
        private Label lblMostrarNombre;
        private Label lblMostrarLocalidad;
        private Label lblMostrarTurno;
        private Label lblMostrarPlanta;
        private Label lblMostrarLinea;
        private PictureBox pictureBox1;
        private Button btnBuscar;
    }
}