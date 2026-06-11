namespace Aseguranza.Ventanas
{
    partial class ExpedienteTrabajadorVentana
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
            lblComentario = new Label();
            txtComentario = new TextBox();
            dgvExpediente = new DataGridView();
            btnAdjuntar = new Button();
            btnAbrir = new Button();
            btnReemplazar = new Button();
            btnEliminar = new Button();
            btnRegresar = new Button();
            cbCamaras = new ComboBox();
            lblCamaras = new Label();
            pbCamara = new PictureBox();
            btnCapturar = new Button();
            btnSeleccionar = new Button();
            btnIniciarCamara = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvExpediente).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbCamara).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(404, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(147, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Expediente del trabajador";
            // 
            // lblComentario
            // 
            lblComentario.AutoSize = true;
            lblComentario.Location = new Point(12, 42);
            lblComentario.Name = "lblComentario";
            lblComentario.Size = new Size(75, 15);
            lblComentario.TabIndex = 1;
            lblComentario.Text = "Comentario:";
            // 
            // txtComentario
            // 
            txtComentario.Location = new Point(93, 39);
            txtComentario.Multiline = true;
            txtComentario.Name = "txtComentario";
            txtComentario.Size = new Size(358, 37);
            txtComentario.TabIndex = 2;
            // 
            // dgvExpediente
            // 
            dgvExpediente.AllowUserToAddRows = false;
            dgvExpediente.AllowUserToDeleteRows = false;
            dgvExpediente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExpediente.Location = new Point(12, 82);
            dgvExpediente.MultiSelect = false;
            dgvExpediente.Name = "dgvExpediente";
            dgvExpediente.ReadOnly = true;
            dgvExpediente.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExpediente.Size = new Size(666, 215);
            dgvExpediente.TabIndex = 3;
            // 
            // btnAdjuntar
            // 
            btnAdjuntar.Location = new Point(12, 319);
            btnAdjuntar.Name = "btnAdjuntar";
            btnAdjuntar.Size = new Size(88, 39);
            btnAdjuntar.TabIndex = 4;
            btnAdjuntar.Text = "Adjuntar";
            btnAdjuntar.UseVisualStyleBackColor = true;
            btnAdjuntar.Click += btnAdjuntar_Click;
            // 
            // btnAbrir
            // 
            btnAbrir.Location = new Point(106, 319);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(88, 39);
            btnAbrir.TabIndex = 5;
            btnAbrir.Text = "Abrir";
            btnAbrir.UseVisualStyleBackColor = true;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnReemplazar
            // 
            btnReemplazar.Location = new Point(200, 319);
            btnReemplazar.Name = "btnReemplazar";
            btnReemplazar.Size = new Size(88, 39);
            btnReemplazar.TabIndex = 6;
            btnReemplazar.Text = "Reemplazar";
            btnReemplazar.UseVisualStyleBackColor = true;
            btnReemplazar.Click += btnReemplazar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(294, 319);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(88, 39);
            btnEliminar.TabIndex = 7;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(388, 319);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 8;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // cbCamaras
            // 
            cbCamaras.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCamaras.FormattingEnabled = true;
            cbCamaras.Location = new Point(752, 42);
            cbCamaras.Name = "cbCamaras";
            cbCamaras.Size = new Size(204, 23);
            cbCamaras.TabIndex = 21;
            // 
            // lblCamaras
            // 
            lblCamaras.AutoSize = true;
            lblCamaras.Location = new Point(684, 45);
            lblCamaras.Name = "lblCamaras";
            lblCamaras.Size = new Size(62, 15);
            lblCamaras.TabIndex = 20;
            lblCamaras.Text = "Cámaras:";
            // 
            // pbCamara
            // 
            pbCamara.BorderStyle = BorderStyle.FixedSingle;
            pbCamara.Location = new Point(684, 82);
            pbCamara.Name = "pbCamara";
            pbCamara.Size = new Size(272, 215);
            pbCamara.SizeMode = PictureBoxSizeMode.Zoom;
            pbCamara.TabIndex = 22;
            pbCamara.TabStop = false;
            // 
            // btnCapturar
            // 
            btnCapturar.Location = new Point(776, 319);
            btnCapturar.Name = "btnCapturar";
            btnCapturar.Size = new Size(88, 39);
            btnCapturar.TabIndex = 24;
            btnCapturar.Text = "Capturar";
            btnCapturar.UseVisualStyleBackColor = true;
            btnCapturar.Click += btnCapturar_Click;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Location = new Point(868, 319);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(88, 39);
            btnSeleccionar.TabIndex = 25;
            btnSeleccionar.Text = "Seleccionar...";
            btnSeleccionar.UseVisualStyleBackColor = true;
            btnSeleccionar.Click += btnSeleccionar_Click;
            // 
            // btnIniciarCamara
            // 
            btnIniciarCamara.Location = new Point(684, 319);
            btnIniciarCamara.Name = "btnIniciarCamara";
            btnIniciarCamara.Size = new Size(88, 39);
            btnIniciarCamara.TabIndex = 23;
            btnIniciarCamara.Text = "Iniciar camara";
            btnIniciarCamara.UseVisualStyleBackColor = true;
            btnIniciarCamara.Click += btnIniciarCamara_Click;
            // 
            // ExpedienteTrabajadorVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 375);
            Controls.Add(btnCapturar);
            Controls.Add(btnSeleccionar);
            Controls.Add(btnIniciarCamara);
            Controls.Add(pbCamara);
            Controls.Add(cbCamaras);
            Controls.Add(lblCamaras);
            Controls.Add(btnRegresar);
            Controls.Add(btnEliminar);
            Controls.Add(btnReemplazar);
            Controls.Add(btnAbrir);
            Controls.Add(btnAdjuntar);
            Controls.Add(dgvExpediente);
            Controls.Add(txtComentario);
            Controls.Add(lblComentario);
            Controls.Add(lblTitulo);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExpedienteTrabajadorVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ExpedienteTrabajadorVentana";
            FormClosing += ExpedienteTrabajadorVentana_FormClosing;
            Load += ExpedienteTrabajadorVentana_Load;
            ((System.ComponentModel.ISupportInitialize)dgvExpediente).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbCamara).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblComentario;
        private TextBox txtComentario;
        private DataGridView dgvExpediente;
        private Button btnAdjuntar;
        private Button btnAbrir;
        private Button btnReemplazar;
        private Button btnEliminar;
        private Button btnRegresar;
        private ComboBox cbCamaras;
        private Label lblCamaras;
        private PictureBox pbCamara;
        private Button btnCapturar;
        private Button btnSeleccionar;
        private Button btnIniciarCamara;
    }
}