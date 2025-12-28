namespace Aseguranza.Ventanas
{
    partial class TrabajadoresVentana
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
            lblNoReloj = new Label();
            txtNoReloj = new TextBox();
            txtNombre = new TextBox();
            lblNombre = new Label();
            lblLocalidad = new Label();
            cbLocalidad = new ComboBox();
            cbTurno = new ComboBox();
            lblTurno = new Label();
            cbLinea = new ComboBox();
            lblLinea = new Label();
            cbPlanta = new ComboBox();
            lblPlanta = new Label();
            pictureBox1 = new PictureBox();
            btnIniciarCamara = new Button();
            btnSeleccionar = new Button();
            btnAceptar = new Button();
            btnRegresar = new Button();
            btnCapturar = new Button();
            cbCamaras = new ComboBox();
            lblCamaras = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblNoReloj
            // 
            lblNoReloj.AutoSize = true;
            lblNoReloj.Location = new Point(37, 34);
            lblNoReloj.Name = "lblNoReloj";
            lblNoReloj.Size = new Size(61, 15);
            lblNoReloj.TabIndex = 0;
            lblNoReloj.Text = "No. Reloj:";
            // 
            // txtNoReloj
            // 
            txtNoReloj.Location = new Point(104, 31);
            txtNoReloj.Name = "txtNoReloj";
            txtNoReloj.Size = new Size(275, 21);
            txtNoReloj.TabIndex = 1;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(104, 86);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(275, 21);
            txtNombre.TabIndex = 3;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(43, 89);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(55, 15);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            // 
            // lblLocalidad
            // 
            lblLocalidad.AutoSize = true;
            lblLocalidad.Location = new Point(34, 143);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(64, 15);
            lblLocalidad.TabIndex = 4;
            lblLocalidad.Text = "Localidad:";
            // 
            // cbLocalidad
            // 
            cbLocalidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLocalidad.FormattingEnabled = true;
            cbLocalidad.Location = new Point(104, 140);
            cbLocalidad.Name = "cbLocalidad";
            cbLocalidad.Size = new Size(275, 23);
            cbLocalidad.TabIndex = 5;
            // 
            // cbTurno
            // 
            cbTurno.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTurno.FormattingEnabled = true;
            cbTurno.Location = new Point(104, 198);
            cbTurno.Name = "cbTurno";
            cbTurno.Size = new Size(275, 23);
            cbTurno.TabIndex = 7;
            // 
            // lblTurno
            // 
            lblTurno.AutoSize = true;
            lblTurno.Location = new Point(56, 201);
            lblTurno.Name = "lblTurno";
            lblTurno.Size = new Size(42, 15);
            lblTurno.TabIndex = 6;
            lblTurno.Text = "Turno:";
            // 
            // cbLinea
            // 
            cbLinea.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLinea.FormattingEnabled = true;
            cbLinea.Location = new Point(104, 301);
            cbLinea.Name = "cbLinea";
            cbLinea.Size = new Size(275, 23);
            cbLinea.TabIndex = 9;
            // 
            // lblLinea
            // 
            lblLinea.AutoSize = true;
            lblLinea.Location = new Point(57, 304);
            lblLinea.Name = "lblLinea";
            lblLinea.Size = new Size(41, 15);
            lblLinea.TabIndex = 8;
            lblLinea.Text = "Linea:";
            // 
            // cbPlanta
            // 
            cbPlanta.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPlanta.FormattingEnabled = true;
            cbPlanta.Location = new Point(104, 246);
            cbPlanta.Name = "cbPlanta";
            cbPlanta.Size = new Size(275, 23);
            cbPlanta.TabIndex = 11;
            cbPlanta.SelectedIndexChanged += cbPlanta_SelectedIndexChanged;
            cbPlanta.SelectedValueChanged += cbPlanta_SelectedValueChanged;
            // 
            // lblPlanta
            // 
            lblPlanta.AutoSize = true;
            lblPlanta.Location = new Point(53, 249);
            lblPlanta.Name = "lblPlanta";
            lblPlanta.Size = new Size(45, 15);
            lblPlanta.TabIndex = 10;
            lblPlanta.Text = "Planta:";
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(385, 60);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(272, 209);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // btnIniciarCamara
            // 
            btnIniciarCamara.Location = new Point(385, 285);
            btnIniciarCamara.Name = "btnIniciarCamara";
            btnIniciarCamara.Size = new Size(88, 39);
            btnIniciarCamara.TabIndex = 13;
            btnIniciarCamara.Text = "Iniciar camara";
            btnIniciarCamara.UseVisualStyleBackColor = true;
            btnIniciarCamara.Click += btnTomar_Click;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Location = new Point(569, 285);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(88, 39);
            btnSeleccionar.TabIndex = 14;
            btnSeleccionar.Text = "Seleccionar...";
            btnSeleccionar.UseVisualStyleBackColor = true;
            btnSeleccionar.Click += btnSeleccionar_Click;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(240, 357);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 16;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(334, 357);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 15;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnCapturar
            // 
            btnCapturar.Location = new Point(477, 285);
            btnCapturar.Name = "btnCapturar";
            btnCapturar.Size = new Size(88, 39);
            btnCapturar.TabIndex = 17;
            btnCapturar.Text = "Capturar";
            btnCapturar.UseVisualStyleBackColor = true;
            btnCapturar.Click += btnCapturar_Click;
            // 
            // cbCamaras
            // 
            cbCamaras.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCamaras.FormattingEnabled = true;
            cbCamaras.Location = new Point(453, 31);
            cbCamaras.Name = "cbCamaras";
            cbCamaras.Size = new Size(204, 23);
            cbCamaras.TabIndex = 19;
            // 
            // lblCamaras
            // 
            lblCamaras.AutoSize = true;
            lblCamaras.Location = new Point(385, 34);
            lblCamaras.Name = "lblCamaras";
            lblCamaras.Size = new Size(62, 15);
            lblCamaras.TabIndex = 18;
            lblCamaras.Text = "Cámaras:";
            // 
            // TrabajadoresVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(669, 411);
            Controls.Add(cbCamaras);
            Controls.Add(lblCamaras);
            Controls.Add(btnCapturar);
            Controls.Add(btnAceptar);
            Controls.Add(btnRegresar);
            Controls.Add(btnSeleccionar);
            Controls.Add(btnIniciarCamara);
            Controls.Add(pictureBox1);
            Controls.Add(cbPlanta);
            Controls.Add(lblPlanta);
            Controls.Add(cbLinea);
            Controls.Add(lblLinea);
            Controls.Add(cbTurno);
            Controls.Add(lblTurno);
            Controls.Add(cbLocalidad);
            Controls.Add(lblLocalidad);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Controls.Add(txtNoReloj);
            Controls.Add(lblNoReloj);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TrabajadoresVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Trabajadores";
            FormClosed += TrabajadoresVentana_FormClosed;
            Load += TrabajadoresVentana_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNoReloj;
        private TextBox txtNoReloj;
        private TextBox txtNombre;
        private Label lblNombre;
        private Label lblLocalidad;
        private ComboBox cbLocalidad;
        private ComboBox cbTurno;
        private Label lblTurno;
        private ComboBox cbLinea;
        private Label lblLinea;
        private ComboBox cbPlanta;
        private Label lblPlanta;
        private PictureBox pictureBox1;
        private Button btnIniciarCamara;
        private Button btnSeleccionar;
        private Button btnAceptar;
        private Button btnRegresar;
        private Button btnCapturar;
        private ComboBox cbCamaras;
        private Label lblCamaras;
    }
}