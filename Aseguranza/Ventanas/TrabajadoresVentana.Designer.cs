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
            components = new System.ComponentModel.Container();
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
            cmsFoto = new ContextMenuStrip(components);
            tsmVerFotoGrande = new ToolStripMenuItem();
            tsmSeleccionarFoto = new ToolStripMenuItem();
            tsmQuitarFoto = new ToolStripMenuItem();
            btnIniciarCamara = new Button();
            btnSeleccionar = new Button();
            btnRegresar = new Button();
            btnCapturar = new Button();
            cbCamaras = new ComboBox();
            lblCamaras = new Label();
            lblTitulo = new Label();
            gbDatosTrabajador = new GroupBox();
            gbFotoTrabajador = new GroupBox();
            btnRecargarCamaras = new Button();
            btnQuitarFoto = new Button();
            lblEstadoFoto = new Label();
            pnlAcciones = new Panel();
            btnGuardar = new Button();
            ttAyuda = new ToolTip(components);
            epValidacion = new ErrorProvider(components);
            ssEstado = new StatusStrip();
            tsslEstado = new ToolStripStatusLabel();
            lblCamposObligatorios = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            cmsFoto.SuspendLayout();
            gbDatosTrabajador.SuspendLayout();
            gbFotoTrabajador.SuspendLayout();
            pnlAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)epValidacion).BeginInit();
            ssEstado.SuspendLayout();
            SuspendLayout();
            // 
            // lblNoReloj
            // 
            lblNoReloj.AutoSize = true;
            lblNoReloj.Location = new Point(10, 34);
            lblNoReloj.Name = "lblNoReloj";
            lblNoReloj.Size = new Size(69, 15);
            lblNoReloj.TabIndex = 0;
            lblNoReloj.Text = "No. Reloj: *";
            // 
            // txtNoReloj
            // 
            txtNoReloj.Location = new Point(77, 31);
            txtNoReloj.MaxLength = 10;
            txtNoReloj.Name = "txtNoReloj";
            txtNoReloj.Size = new Size(275, 21);
            txtNoReloj.TabIndex = 0;
            txtNoReloj.TextChanged += txtNoReloj_TextChanged;
            txtNoReloj.KeyPress += txtNoReloj_KeyPress;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(77, 74);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(275, 21);
            txtNombre.TabIndex = 1;
            txtNombre.TextChanged += txtNombre_TextChanged;
            txtNombre.KeyPress += txtNombre_KeyPress;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 77);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(63, 15);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre: *";
            // 
            // lblLocalidad
            // 
            lblLocalidad.AutoSize = true;
            lblLocalidad.Location = new Point(7, 124);
            lblLocalidad.Name = "lblLocalidad";
            lblLocalidad.Size = new Size(72, 15);
            lblLocalidad.TabIndex = 4;
            lblLocalidad.Text = "Localidad: *";
            // 
            // cbLocalidad
            // 
            cbLocalidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLocalidad.FormattingEnabled = true;
            cbLocalidad.Location = new Point(77, 121);
            cbLocalidad.Name = "cbLocalidad";
            cbLocalidad.Size = new Size(275, 23);
            cbLocalidad.TabIndex = 2;
            cbLocalidad.SelectedIndexChanged += cbLocalidad_SelectedIndexChanged;
            cbLocalidad.KeyPress += ComboBox_KeyPress_Avanzar;
            // 
            // cbTurno
            // 
            cbTurno.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTurno.FormattingEnabled = true;
            cbTurno.Location = new Point(77, 170);
            cbTurno.Name = "cbTurno";
            cbTurno.Size = new Size(275, 23);
            cbTurno.TabIndex = 3;
            cbTurno.SelectedIndexChanged += cbTurno_SelectedIndexChanged;
            cbTurno.KeyPress += ComboBox_KeyPress_Avanzar;
            // 
            // lblTurno
            // 
            lblTurno.AutoSize = true;
            lblTurno.Location = new Point(29, 173);
            lblTurno.Name = "lblTurno";
            lblTurno.Size = new Size(50, 15);
            lblTurno.TabIndex = 6;
            lblTurno.Text = "Turno: *";
            // 
            // cbLinea
            // 
            cbLinea.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLinea.FormattingEnabled = true;
            cbLinea.Location = new Point(77, 266);
            cbLinea.Name = "cbLinea";
            cbLinea.Size = new Size(275, 23);
            cbLinea.TabIndex = 5;
            cbLinea.SelectedIndexChanged += cbLinea_SelectedIndexChanged;
            cbLinea.KeyPress += ComboBox_KeyPress_Avanzar;
            // 
            // lblLinea
            // 
            lblLinea.AutoSize = true;
            lblLinea.Location = new Point(30, 269);
            lblLinea.Name = "lblLinea";
            lblLinea.Size = new Size(49, 15);
            lblLinea.TabIndex = 8;
            lblLinea.Text = "Linea: *";
            // 
            // cbPlanta
            // 
            cbPlanta.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPlanta.FormattingEnabled = true;
            cbPlanta.Location = new Point(77, 217);
            cbPlanta.Name = "cbPlanta";
            cbPlanta.Size = new Size(275, 23);
            cbPlanta.TabIndex = 4;
            cbPlanta.SelectedIndexChanged += cbPlanta_SelectedIndexChanged;
            cbPlanta.SelectedValueChanged += cbPlanta_SelectedValueChanged;
            cbPlanta.KeyPress += ComboBox_KeyPress_Avanzar;
            // 
            // lblPlanta
            // 
            lblPlanta.AutoSize = true;
            lblPlanta.Location = new Point(26, 220);
            lblPlanta.Name = "lblPlanta";
            lblPlanta.Size = new Size(53, 15);
            lblPlanta.TabIndex = 10;
            lblPlanta.Text = "Planta: *";
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.ContextMenuStrip = cmsFoto;
            pictureBox1.Location = new Point(6, 39);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(272, 209);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.DoubleClick += pictureBox1_DoubleClick;
            // 
            // cmsFoto
            // 
            cmsFoto.Items.AddRange(new ToolStripItem[] { tsmVerFotoGrande, tsmSeleccionarFoto, tsmQuitarFoto });
            cmsFoto.Name = "cmsFoto";
            cmsFoto.Size = new Size(160, 70);
            cmsFoto.Opening += cmsFoto_Opening;
            // 
            // tsmVerFotoGrande
            // 
            tsmVerFotoGrande.Name = "tsmVerFotoGrande";
            tsmVerFotoGrande.Size = new Size(159, 22);
            tsmVerFotoGrande.Text = "Ver en grande";
            tsmVerFotoGrande.Click += tsmVerFotoGrande_Click;
            // 
            // tsmSeleccionarFoto
            // 
            tsmSeleccionarFoto.Name = "tsmSeleccionarFoto";
            tsmSeleccionarFoto.Size = new Size(159, 22);
            tsmSeleccionarFoto.Text = "Seleccionar foto";
            tsmSeleccionarFoto.Click += tsmSeleccionarFoto_Click;
            // 
            // tsmQuitarFoto
            // 
            tsmQuitarFoto.Name = "tsmQuitarFoto";
            tsmQuitarFoto.Size = new Size(159, 22);
            tsmQuitarFoto.Text = "Quitar foto";
            tsmQuitarFoto.Click += tsmQuitarFoto_Click;
            // 
            // btnIniciarCamara
            // 
            btnIniciarCamara.Location = new Point(6, 277);
            btnIniciarCamara.Name = "btnIniciarCamara";
            btnIniciarCamara.Size = new Size(88, 39);
            btnIniciarCamara.TabIndex = 7;
            btnIniciarCamara.Text = "Iniciar camara";
            btnIniciarCamara.UseVisualStyleBackColor = true;
            btnIniciarCamara.Click += btnTomar_Click;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Location = new Point(190, 277);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(88, 39);
            btnSeleccionar.TabIndex = 9;
            btnSeleccionar.Text = "Seleccionar...";
            btnSeleccionar.UseVisualStyleBackColor = true;
            btnSeleccionar.Click += btnSeleccionar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(347, 3);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 11;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnCapturar
            // 
            btnCapturar.Location = new Point(98, 277);
            btnCapturar.Name = "btnCapturar";
            btnCapturar.Size = new Size(88, 39);
            btnCapturar.TabIndex = 8;
            btnCapturar.Text = "Capturar";
            btnCapturar.UseVisualStyleBackColor = true;
            btnCapturar.Click += btnCapturar_Click;
            // 
            // cbCamaras
            // 
            cbCamaras.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCamaras.FormattingEnabled = true;
            cbCamaras.Location = new Point(74, 14);
            cbCamaras.Name = "cbCamaras";
            cbCamaras.Size = new Size(204, 23);
            cbCamaras.TabIndex = 6;
            // 
            // lblCamaras
            // 
            lblCamaras.AutoSize = true;
            lblCamaras.Location = new Point(6, 17);
            lblCamaras.Name = "lblCamaras";
            lblCamaras.Size = new Size(62, 15);
            lblCamaras.TabIndex = 18;
            lblCamaras.Text = "Cámaras:";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Arial Rounded MT Bold", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.Navy;
            lblTitulo.Location = new Point(230, 4);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(217, 22);
            lblTitulo.TabIndex = 20;
            lblTitulo.Text = "Registro de trabajador";
            // 
            // gbDatosTrabajador
            // 
            gbDatosTrabajador.Controls.Add(lblCamposObligatorios);
            gbDatosTrabajador.Controls.Add(lblNoReloj);
            gbDatosTrabajador.Controls.Add(txtNoReloj);
            gbDatosTrabajador.Controls.Add(lblNombre);
            gbDatosTrabajador.Controls.Add(txtNombre);
            gbDatosTrabajador.Controls.Add(lblLocalidad);
            gbDatosTrabajador.Controls.Add(cbLocalidad);
            gbDatosTrabajador.Controls.Add(lblTurno);
            gbDatosTrabajador.Controls.Add(cbTurno);
            gbDatosTrabajador.Controls.Add(lblLinea);
            gbDatosTrabajador.Controls.Add(cbLinea);
            gbDatosTrabajador.Controls.Add(cbPlanta);
            gbDatosTrabajador.Controls.Add(lblPlanta);
            gbDatosTrabajador.Location = new Point(12, 29);
            gbDatosTrabajador.Name = "gbDatosTrabajador";
            gbDatosTrabajador.Size = new Size(389, 379);
            gbDatosTrabajador.TabIndex = 21;
            gbDatosTrabajador.TabStop = false;
            gbDatosTrabajador.Text = "Datos del trabajador";
            // 
            // gbFotoTrabajador
            // 
            gbFotoTrabajador.Controls.Add(btnRecargarCamaras);
            gbFotoTrabajador.Controls.Add(btnQuitarFoto);
            gbFotoTrabajador.Controls.Add(lblEstadoFoto);
            gbFotoTrabajador.Controls.Add(lblCamaras);
            gbFotoTrabajador.Controls.Add(pictureBox1);
            gbFotoTrabajador.Controls.Add(cbCamaras);
            gbFotoTrabajador.Controls.Add(btnIniciarCamara);
            gbFotoTrabajador.Controls.Add(btnSeleccionar);
            gbFotoTrabajador.Controls.Add(btnCapturar);
            gbFotoTrabajador.Location = new Point(407, 29);
            gbFotoTrabajador.Name = "gbFotoTrabajador";
            gbFotoTrabajador.Size = new Size(314, 379);
            gbFotoTrabajador.TabIndex = 22;
            gbFotoTrabajador.TabStop = false;
            gbFotoTrabajador.Text = "Fotografía del trabajador *";
            // 
            // btnRecargarCamaras
            // 
            btnRecargarCamaras.Font = new Font("Arial Rounded MT Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRecargarCamaras.Location = new Point(280, 14);
            btnRecargarCamaras.Name = "btnRecargarCamaras";
            btnRecargarCamaras.Size = new Size(28, 23);
            btnRecargarCamaras.TabIndex = 25;
            btnRecargarCamaras.Text = "↻";
            btnRecargarCamaras.UseVisualStyleBackColor = true;
            btnRecargarCamaras.Click += btnRecargarCamaras_Click;
            // 
            // btnQuitarFoto
            // 
            btnQuitarFoto.Location = new Point(98, 320);
            btnQuitarFoto.Name = "btnQuitarFoto";
            btnQuitarFoto.Size = new Size(88, 39);
            btnQuitarFoto.TabIndex = 21;
            btnQuitarFoto.Text = "Quitar foto";
            btnQuitarFoto.UseVisualStyleBackColor = true;
            btnQuitarFoto.Click += btnQuitarFoto_Click;
            // 
            // lblEstadoFoto
            // 
            lblEstadoFoto.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEstadoFoto.ForeColor = Color.DimGray;
            lblEstadoFoto.Location = new Point(6, 252);
            lblEstadoFoto.Name = "lblEstadoFoto";
            lblEstadoFoto.Size = new Size(272, 22);
            lblEstadoFoto.TabIndex = 20;
            lblEstadoFoto.Text = "Foto: sin fotografía";
            // 
            // pnlAcciones
            // 
            pnlAcciones.Controls.Add(btnGuardar);
            pnlAcciones.Controls.Add(btnRegresar);
            pnlAcciones.Location = new Point(12, 414);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(709, 46);
            pnlAcciones.TabIndex = 23;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(253, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(88, 39);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // epValidacion
            // 
            epValidacion.ContainerControl = this;
            // 
            // ssEstado
            // 
            ssEstado.Items.AddRange(new ToolStripItem[] { tsslEstado });
            ssEstado.Location = new Point(0, 464);
            ssEstado.Name = "ssEstado";
            ssEstado.Size = new Size(725, 22);
            ssEstado.TabIndex = 24;
            ssEstado.Text = "statusStrip1";
            // 
            // tsslEstado
            // 
            tsslEstado.Name = "tsslEstado";
            tsslEstado.Size = new Size(710, 17);
            tsslEstado.Spring = true;
            tsslEstado.Text = "Listo";
            tsslEstado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCamposObligatorios
            // 
            lblCamposObligatorios.AutoSize = true;
            lblCamposObligatorios.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCamposObligatorios.ForeColor = Color.DarkRed;
            lblCamposObligatorios.Location = new Point(123, 362);
            lblCamposObligatorios.Name = "lblCamposObligatorios";
            lblCamposObligatorios.Size = new Size(136, 14);
            lblCamposObligatorios.TabIndex = 11;
            lblCamposObligatorios.Text = "* Campos obligatorios";
            // 
            // TrabajadoresVentana
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnRegresar;
            ClientSize = new Size(725, 486);
            Controls.Add(ssEstado);
            Controls.Add(pnlAcciones);
            Controls.Add(gbFotoTrabajador);
            Controls.Add(gbDatosTrabajador);
            Controls.Add(lblTitulo);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TrabajadoresVentana";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Trabajadores";
            FormClosing += TrabajadoresVentana_FormClosing;
            FormClosed += TrabajadoresVentana_FormClosed;
            Load += TrabajadoresVentana_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            cmsFoto.ResumeLayout(false);
            gbDatosTrabajador.ResumeLayout(false);
            gbDatosTrabajador.PerformLayout();
            gbFotoTrabajador.ResumeLayout(false);
            gbFotoTrabajador.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)epValidacion).EndInit();
            ssEstado.ResumeLayout(false);
            ssEstado.PerformLayout();
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
        private Label lblTitulo;
        private GroupBox gbDatosTrabajador;
        private GroupBox gbFotoTrabajador;
        private Label lblEstadoFoto;
        private Panel pnlAcciones;
        private Button btnGuardar;
        private ToolTip ttAyuda;
        private ErrorProvider epValidacion;
        private Button btnQuitarFoto;
        private ContextMenuStrip cmsFoto;
        private ToolStripMenuItem tsmVerFotoGrande;
        private ToolStripMenuItem tsmSeleccionarFoto;
        private ToolStripMenuItem tsmQuitarFoto;
        private StatusStrip ssEstado;
        private ToolStripStatusLabel tsslEstado;
        private Button btnRecargarCamaras;
        private Label lblCamposObligatorios;
    }
}