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
            components = new System.ComponentModel.Container();
            lblTitulo = new Label();
            lblComentario = new Label();
            txtComentario = new TextBox();
            dgvExpediente = new DataGridView();
            cmsExpediente = new ContextMenuStrip(components);
            tsmAbrir = new ToolStripMenuItem();
            tsmAbrirUbicacion = new ToolStripMenuItem();
            tsmReemplazar = new ToolStripMenuItem();
            tsmEliminar = new ToolStripMenuItem();
            tsmCopiarRuta = new ToolStripMenuItem();
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
            ttAyuda = new ToolTip(components);
            gbInformacion = new GroupBox();
            gbArchivos = new GroupBox();
            gbCamara = new GroupBox();
            gbAcciones = new GroupBox();
            lblResumenExpediente = new Label();
            lblArchivoSeleccionado = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvExpediente).BeginInit();
            cmsExpediente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbCamara).BeginInit();
            gbInformacion.SuspendLayout();
            gbArchivos.SuspendLayout();
            gbCamara.SuspendLayout();
            gbAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(15, 28);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(307, 29);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Expediente del trabajador";
            // 
            // lblComentario
            // 
            lblComentario.AutoSize = true;
            lblComentario.Location = new Point(15, 20);
            lblComentario.Name = "lblComentario";
            lblComentario.Size = new Size(75, 15);
            lblComentario.TabIndex = 1;
            lblComentario.Text = "Comentario:";
            // 
            // txtComentario
            // 
            txtComentario.Location = new Point(95, 18);
            txtComentario.Multiline = true;
            txtComentario.Name = "txtComentario";
            txtComentario.Size = new Size(330, 32);
            txtComentario.TabIndex = 2;
            // 
            // dgvExpediente
            // 
            dgvExpediente.AllowUserToAddRows = false;
            dgvExpediente.AllowUserToDeleteRows = false;
            dgvExpediente.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvExpediente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExpediente.ContextMenuStrip = cmsExpediente;
            dgvExpediente.Location = new Point(15, 45);
            dgvExpediente.MultiSelect = false;
            dgvExpediente.Name = "dgvExpediente";
            dgvExpediente.ReadOnly = true;
            dgvExpediente.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExpediente.Size = new Size(660, 300);
            dgvExpediente.TabIndex = 3;
            dgvExpediente.CellDoubleClick += dgvExpediente_CellDoubleClick;
            dgvExpediente.SelectionChanged += dgvExpediente_SelectionChanged;
            dgvExpediente.MouseDown += dgvExpediente_MouseDown;
            // 
            // cmsExpediente
            // 
            cmsExpediente.Items.AddRange(new ToolStripItem[] { tsmAbrir, tsmAbrirUbicacion, tsmReemplazar, tsmEliminar, tsmCopiarRuta });
            cmsExpediente.Name = "cmsExpediente";
            cmsExpediente.Size = new Size(156, 114);
            cmsExpediente.Opening += cmsExpediente_Opening;
            // 
            // tsmAbrir
            // 
            tsmAbrir.Name = "tsmAbrir";
            tsmAbrir.Size = new Size(155, 22);
            tsmAbrir.Text = "Abrir";
            tsmAbrir.Click += tsmAbrir_Click;
            // 
            // tsmAbrirUbicacion
            // 
            tsmAbrirUbicacion.Name = "tsmAbrirUbicacion";
            tsmAbrirUbicacion.Size = new Size(155, 22);
            tsmAbrirUbicacion.Text = "Abrir ubicación";
            tsmAbrirUbicacion.Click += tsmAbrirUbicacion_Click;
            // 
            // tsmReemplazar
            // 
            tsmReemplazar.Name = "tsmReemplazar";
            tsmReemplazar.Size = new Size(155, 22);
            tsmReemplazar.Text = "Reemplazar";
            tsmReemplazar.Click += tsmReemplazar_Click;
            // 
            // tsmEliminar
            // 
            tsmEliminar.Name = "tsmEliminar";
            tsmEliminar.Size = new Size(155, 22);
            tsmEliminar.Text = "Eliminar";
            tsmEliminar.Click += tsmEliminar_Click;
            // 
            // tsmCopiarRuta
            // 
            tsmCopiarRuta.Name = "tsmCopiarRuta";
            tsmCopiarRuta.Size = new Size(155, 22);
            tsmCopiarRuta.Text = "Copiar ruta";
            tsmCopiarRuta.Click += tsmCopiarRuta_Click;
            // 
            // btnAdjuntar
            // 
            btnAdjuntar.BackColor = Color.FromArgb(230, 240, 255);
            btnAdjuntar.Location = new Point(445, 17);
            btnAdjuntar.Name = "btnAdjuntar";
            btnAdjuntar.Size = new Size(88, 39);
            btnAdjuntar.TabIndex = 4;
            btnAdjuntar.Text = "Adjuntar";
            btnAdjuntar.UseVisualStyleBackColor = false;
            btnAdjuntar.Click += btnAdjuntar_Click;
            // 
            // btnAbrir
            // 
            btnAbrir.BackColor = Color.FromArgb(230, 240, 255);
            btnAbrir.Location = new Point(540, 17);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(88, 39);
            btnAbrir.TabIndex = 5;
            btnAbrir.Text = "Abrir";
            btnAbrir.UseVisualStyleBackColor = false;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnReemplazar
            // 
            btnReemplazar.BackColor = Color.FromArgb(230, 240, 255);
            btnReemplazar.Location = new Point(634, 17);
            btnReemplazar.Name = "btnReemplazar";
            btnReemplazar.Size = new Size(88, 39);
            btnReemplazar.TabIndex = 6;
            btnReemplazar.Text = "Reemplazar";
            btnReemplazar.UseVisualStyleBackColor = false;
            btnReemplazar.Click += btnReemplazar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(255, 235, 235);
            btnEliminar.Location = new Point(729, 17);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(88, 39);
            btnEliminar.TabIndex = 7;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRegresar.BackColor = SystemColors.ButtonFace;
            btnRegresar.Location = new Point(905, 17);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 8;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = false;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // cbCamaras
            // 
            cbCamaras.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbCamaras.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCamaras.FormattingEnabled = true;
            cbCamaras.Location = new Point(75, 25);
            cbCamaras.Name = "cbCamaras";
            cbCamaras.Size = new Size(220, 23);
            cbCamaras.TabIndex = 21;
            // 
            // lblCamaras
            // 
            lblCamaras.AutoSize = true;
            lblCamaras.Location = new Point(15, 28);
            lblCamaras.Name = "lblCamaras";
            lblCamaras.Size = new Size(55, 15);
            lblCamaras.TabIndex = 20;
            lblCamaras.Text = "Cámara:";
            // 
            // pbCamara
            // 
            pbCamara.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbCamara.BorderStyle = BorderStyle.FixedSingle;
            pbCamara.Location = new Point(15, 60);
            pbCamara.Name = "pbCamara";
            pbCamara.Size = new Size(280, 220);
            pbCamara.SizeMode = PictureBoxSizeMode.Zoom;
            pbCamara.TabIndex = 22;
            pbCamara.TabStop = false;
            // 
            // btnCapturar
            // 
            btnCapturar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCapturar.Location = new Point(110, 295);
            btnCapturar.Name = "btnCapturar";
            btnCapturar.Size = new Size(88, 39);
            btnCapturar.TabIndex = 24;
            btnCapturar.Text = "Capturar";
            btnCapturar.UseVisualStyleBackColor = true;
            btnCapturar.Click += btnCapturar_Click;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSeleccionar.Location = new Point(205, 295);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(88, 39);
            btnSeleccionar.TabIndex = 25;
            btnSeleccionar.Text = "Seleccionar...";
            btnSeleccionar.UseVisualStyleBackColor = true;
            btnSeleccionar.Click += btnSeleccionar_Click;
            // 
            // btnIniciarCamara
            // 
            btnIniciarCamara.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnIniciarCamara.Location = new Point(15, 295);
            btnIniciarCamara.Name = "btnIniciarCamara";
            btnIniciarCamara.Size = new Size(88, 39);
            btnIniciarCamara.TabIndex = 23;
            btnIniciarCamara.Text = "Iniciar camara";
            btnIniciarCamara.UseVisualStyleBackColor = true;
            btnIniciarCamara.Click += btnIniciarCamara_Click;
            // 
            // gbInformacion
            // 
            gbInformacion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbInformacion.Controls.Add(lblTitulo);
            gbInformacion.Location = new Point(12, 12);
            gbInformacion.Name = "gbInformacion";
            gbInformacion.Size = new Size(1010, 70);
            gbInformacion.TabIndex = 26;
            gbInformacion.TabStop = false;
            gbInformacion.Text = "Información del trabajador";
            // 
            // gbArchivos
            // 
            gbArchivos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gbArchivos.Controls.Add(lblArchivoSeleccionado);
            gbArchivos.Controls.Add(lblResumenExpediente);
            gbArchivos.Controls.Add(dgvExpediente);
            gbArchivos.Location = new Point(12, 90);
            gbArchivos.Name = "gbArchivos";
            gbArchivos.Size = new Size(690, 365);
            gbArchivos.TabIndex = 1;
            gbArchivos.TabStop = false;
            gbArchivos.Text = "Archivos del expediente";
            // 
            // gbCamara
            // 
            gbCamara.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            gbCamara.Controls.Add(lblCamaras);
            gbCamara.Controls.Add(cbCamaras);
            gbCamara.Controls.Add(pbCamara);
            gbCamara.Controls.Add(btnIniciarCamara);
            gbCamara.Controls.Add(btnCapturar);
            gbCamara.Controls.Add(btnSeleccionar);
            gbCamara.Location = new Point(710, 90);
            gbCamara.Name = "gbCamara";
            gbCamara.Size = new Size(312, 365);
            gbCamara.TabIndex = 27;
            gbCamara.TabStop = false;
            gbCamara.Text = "Cámara / Vista previa";
            // 
            // gbAcciones
            // 
            gbAcciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gbAcciones.Controls.Add(lblComentario);
            gbAcciones.Controls.Add(txtComentario);
            gbAcciones.Controls.Add(btnAdjuntar);
            gbAcciones.Controls.Add(btnAbrir);
            gbAcciones.Controls.Add(btnReemplazar);
            gbAcciones.Controls.Add(btnRegresar);
            gbAcciones.Controls.Add(btnEliminar);
            gbAcciones.Location = new Point(12, 462);
            gbAcciones.Name = "gbAcciones";
            gbAcciones.Size = new Size(1010, 60);
            gbAcciones.TabIndex = 28;
            gbAcciones.TabStop = false;
            gbAcciones.Text = "Acciones";
            // 
            // lblResumenExpediente
            // 
            lblResumenExpediente.AutoSize = true;
            lblResumenExpediente.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResumenExpediente.Location = new Point(15, 22);
            lblResumenExpediente.Name = "lblResumenExpediente";
            lblResumenExpediente.Size = new Size(70, 15);
            lblResumenExpediente.TabIndex = 4;
            lblResumenExpediente.Text = "Archivos: 0";
            // 
            // lblArchivoSeleccionado
            // 
            lblArchivoSeleccionado.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblArchivoSeleccionado.Location = new Point(14, 345);
            lblArchivoSeleccionado.Name = "lblArchivoSeleccionado";
            lblArchivoSeleccionado.Size = new Size(660, 20);
            lblArchivoSeleccionado.TabIndex = 5;
            lblArchivoSeleccionado.Text = "Archivo seleccionado: ninguno";
            // 
            // ExpedienteTrabajadorVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1034, 521);
            Controls.Add(gbAcciones);
            Controls.Add(gbCamara);
            Controls.Add(gbArchivos);
            Controls.Add(gbInformacion);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExpedienteTrabajadorVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Expediente del trabajador";
            FormClosing += ExpedienteTrabajadorVentana_FormClosing;
            Load += ExpedienteTrabajadorVentana_Load;
            ((System.ComponentModel.ISupportInitialize)dgvExpediente).EndInit();
            cmsExpediente.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbCamara).EndInit();
            gbInformacion.ResumeLayout(false);
            gbInformacion.PerformLayout();
            gbArchivos.ResumeLayout(false);
            gbArchivos.PerformLayout();
            gbCamara.ResumeLayout(false);
            gbCamara.PerformLayout();
            gbAcciones.ResumeLayout(false);
            gbAcciones.PerformLayout();
            ResumeLayout(false);
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
        private ContextMenuStrip cmsExpediente;
        private ToolStripMenuItem tsmAbrir;
        private ToolStripMenuItem tsmAbrirUbicacion;
        private ToolStripMenuItem tsmReemplazar;
        private ToolStripMenuItem tsmEliminar;
        private ToolStripMenuItem tsmCopiarRuta;
        private ToolTip ttAyuda;
        private GroupBox gbInformacion;
        private GroupBox gbArchivos;
        private GroupBox gbCamara;
        private GroupBox gbAcciones;
        private Label lblResumenExpediente;
        private Label lblArchivoSeleccionado;
    }
}