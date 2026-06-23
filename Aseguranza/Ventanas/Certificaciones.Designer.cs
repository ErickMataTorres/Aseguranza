namespace Aseguranza.Ventanas
{
    partial class Certificaciones
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
            btnRegresar = new Button();
            btnCertificaciones = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            dgvTrabajadores = new DataGridView();
            cmsTrabajadores = new ContextMenuStrip(components);
            tsmVerCertificaciones = new ToolStripMenuItem();
            tsmVistaPreviaCredencial = new ToolStripMenuItem();
            tsmImprimirCredencial = new ToolStripMenuItem();
            tsmModificarTrabajador = new ToolStripMenuItem();
            lblMostrarPor = new Label();
            cbMostrarPor = new ComboBox();
            lblResumenEstados = new Label();
            lblLeyendaEstados = new Label();
            lblTrabajadorSeleccionado = new Label();
            ttAyuda = new ToolTip(components);
            pnlLeyendaEstados = new Panel();
            lblTextoSinCertificar = new Label();
            lblColorSinCertificar = new Label();
            lblTextoVencida = new Label();
            lblColorVencida = new Label();
            lblTextoPorVencer = new Label();
            lblColorPorVencer = new Label();
            lblTextoVigente = new Label();
            lblColorVigente = new Label();
            btnAgregarTrabajador = new Button();
            btnModificarTrabajador = new Button();
            btnBorrarTrabajador = new Button();
            btnVistaPreviaCredencial = new Button();
            btnImprimirCredencial = new Button();
            lblAvisoLimite = new Label();
            btnLimpiarBusqueda = new Button();
            btnBuscar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).BeginInit();
            cmsTrabajadores.SuspendLayout();
            pnlLeyendaEstados.SuspendLayout();
            SuspendLayout();
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(945, 526);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 2;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnCertificaciones
            // 
            btnCertificaciones.Location = new Point(285, 526);
            btnCertificaciones.Name = "btnCertificaciones";
            btnCertificaciones.Size = new Size(109, 39);
            btnCertificaciones.TabIndex = 1;
            btnCertificaciones.Text = "Ver certificaciones";
            btnCertificaciones.UseVisualStyleBackColor = true;
            btnCertificaciones.Click += btnCertificaciones_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(534, 6);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(311, 21);
            txtBuscar.TabIndex = 0;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(479, 9);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 15;
            lblBuscar.Text = "Buscar:";
            // 
            // dgvTrabajadores
            // 
            dgvTrabajadores.AllowUserToAddRows = false;
            dgvTrabajadores.AllowUserToDeleteRows = false;
            dgvTrabajadores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrabajadores.ContextMenuStrip = cmsTrabajadores;
            dgvTrabajadores.Location = new Point(3, 94);
            dgvTrabajadores.MultiSelect = false;
            dgvTrabajadores.Name = "dgvTrabajadores";
            dgvTrabajadores.ReadOnly = true;
            dgvTrabajadores.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvTrabajadores.Size = new Size(1030, 390);
            dgvTrabajadores.TabIndex = 14;
            dgvTrabajadores.TabStop = false;
            dgvTrabajadores.CellDoubleClick += dgvTrabajadores_CellDoubleClick;
            dgvTrabajadores.CellFormatting += dgvTrabajadores_CellFormatting;
            dgvTrabajadores.CellMouseEnter += dgvTrabajadores_CellMouseEnter;
            dgvTrabajadores.CellMouseLeave += dgvTrabajadores_CellMouseLeave;
            dgvTrabajadores.SelectionChanged += dgvTrabajadores_SelectionChanged;
            dgvTrabajadores.MouseDown += dgvTrabajadores_MouseDown;
            // 
            // cmsTrabajadores
            // 
            cmsTrabajadores.Items.AddRange(new ToolStripItem[] { tsmVerCertificaciones, tsmVistaPreviaCredencial, tsmImprimirCredencial, tsmModificarTrabajador });
            cmsTrabajadores.Name = "cmsTrabajadores";
            cmsTrabajadores.Size = new Size(192, 92);
            cmsTrabajadores.Opening += cmsTrabajadores_Opening;
            // 
            // tsmVerCertificaciones
            // 
            tsmVerCertificaciones.Name = "tsmVerCertificaciones";
            tsmVerCertificaciones.Size = new Size(191, 22);
            tsmVerCertificaciones.Text = "Ver certificaciones";
            tsmVerCertificaciones.Click += tsmVerCertificaciones_Click;
            // 
            // tsmVistaPreviaCredencial
            // 
            tsmVistaPreviaCredencial.Name = "tsmVistaPreviaCredencial";
            tsmVistaPreviaCredencial.Size = new Size(191, 22);
            tsmVistaPreviaCredencial.Text = "Vista previa credencial";
            tsmVistaPreviaCredencial.Click += tsmVistaPreviaCredencial_Click;
            // 
            // tsmImprimirCredencial
            // 
            tsmImprimirCredencial.Name = "tsmImprimirCredencial";
            tsmImprimirCredencial.Size = new Size(191, 22);
            tsmImprimirCredencial.Text = "Imprimir credencial";
            tsmImprimirCredencial.Click += tsmImprimirCredencial_Click;
            // 
            // tsmModificarTrabajador
            // 
            tsmModificarTrabajador.Name = "tsmModificarTrabajador";
            tsmModificarTrabajador.Size = new Size(191, 22);
            tsmModificarTrabajador.Text = "Modificar trabajador";
            tsmModificarTrabajador.Click += tsmModificarTrabajador_Click;
            // 
            // lblMostrarPor
            // 
            lblMostrarPor.AutoSize = true;
            lblMostrarPor.Location = new Point(3, 9);
            lblMostrarPor.Name = "lblMostrarPor";
            lblMostrarPor.Size = new Size(72, 15);
            lblMostrarPor.TabIndex = 16;
            lblMostrarPor.Text = "Mostrar por:";
            // 
            // cbMostrarPor
            // 
            cbMostrarPor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMostrarPor.FormattingEnabled = true;
            cbMostrarPor.Items.AddRange(new object[] { "Todas", "Vigente", "Por vencer", "Vencida", "Sin certificar" });
            cbMostrarPor.Location = new Point(81, 6);
            cbMostrarPor.Name = "cbMostrarPor";
            cbMostrarPor.Size = new Size(392, 23);
            cbMostrarPor.TabIndex = 3;
            // 
            // lblResumenEstados
            // 
            lblResumenEstados.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResumenEstados.Location = new Point(3, 32);
            lblResumenEstados.Name = "lblResumenEstados";
            lblResumenEstados.Size = new Size(936, 25);
            lblResumenEstados.TabIndex = 17;
            lblResumenEstados.Text = "Total: 0 | Vigentes: 0 | Por vencer: 0 | Vencidas: 0 | Sin certificar: 0";
            // 
            // lblLeyendaEstados
            // 
            lblLeyendaEstados.Location = new Point(479, 69);
            lblLeyendaEstados.Name = "lblLeyendaEstados";
            lblLeyendaEstados.Size = new Size(554, 22);
            lblLeyendaEstados.TabIndex = 18;
            lblLeyendaEstados.Text = "Verde = Vigente          Amarillo = Por vencer          Rojo = Vencida          Gris = Sin certificar";
            lblLeyendaEstados.Visible = false;
            // 
            // lblTrabajadorSeleccionado
            // 
            lblTrabajadorSeleccionado.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTrabajadorSeleccionado.Location = new Point(3, 69);
            lblTrabajadorSeleccionado.Name = "lblTrabajadorSeleccionado";
            lblTrabajadorSeleccionado.Size = new Size(470, 22);
            lblTrabajadorSeleccionado.TabIndex = 19;
            lblTrabajadorSeleccionado.Text = "Seleccionado: ninguno";
            // 
            // pnlLeyendaEstados
            // 
            pnlLeyendaEstados.BackColor = Color.Transparent;
            pnlLeyendaEstados.Controls.Add(lblTextoSinCertificar);
            pnlLeyendaEstados.Controls.Add(lblColorSinCertificar);
            pnlLeyendaEstados.Controls.Add(lblTextoVencida);
            pnlLeyendaEstados.Controls.Add(lblColorVencida);
            pnlLeyendaEstados.Controls.Add(lblTextoPorVencer);
            pnlLeyendaEstados.Controls.Add(lblColorPorVencer);
            pnlLeyendaEstados.Controls.Add(lblTextoVigente);
            pnlLeyendaEstados.Controls.Add(lblColorVigente);
            pnlLeyendaEstados.Location = new Point(3, 490);
            pnlLeyendaEstados.Name = "pnlLeyendaEstados";
            pnlLeyendaEstados.Size = new Size(1030, 30);
            pnlLeyendaEstados.TabIndex = 20;
            // 
            // lblTextoSinCertificar
            // 
            lblTextoSinCertificar.AutoSize = true;
            lblTextoSinCertificar.Location = new Point(354, 8);
            lblTextoSinCertificar.Name = "lblTextoSinCertificar";
            lblTextoSinCertificar.Size = new Size(74, 15);
            lblTextoSinCertificar.TabIndex = 28;
            lblTextoSinCertificar.Text = "Sin certificar";
            // 
            // lblColorSinCertificar
            // 
            lblColorSinCertificar.BackColor = Color.LightGray;
            lblColorSinCertificar.BorderStyle = BorderStyle.FixedSingle;
            lblColorSinCertificar.Location = new Point(330, 6);
            lblColorSinCertificar.Name = "lblColorSinCertificar";
            lblColorSinCertificar.Size = new Size(18, 18);
            lblColorSinCertificar.TabIndex = 27;
            // 
            // lblTextoVencida
            // 
            lblTextoVencida.AutoSize = true;
            lblTextoVencida.Location = new Point(254, 8);
            lblTextoVencida.Name = "lblTextoVencida";
            lblTextoVencida.Size = new Size(50, 15);
            lblTextoVencida.TabIndex = 26;
            lblTextoVencida.Text = "Vencida";
            // 
            // lblColorVencida
            // 
            lblColorVencida.BackColor = Color.LightCoral;
            lblColorVencida.BorderStyle = BorderStyle.FixedSingle;
            lblColorVencida.Location = new Point(230, 6);
            lblColorVencida.Name = "lblColorVencida";
            lblColorVencida.Size = new Size(18, 18);
            lblColorVencida.TabIndex = 25;
            // 
            // lblTextoPorVencer
            // 
            lblTextoPorVencer.AutoSize = true;
            lblTextoPorVencer.Location = new Point(124, 8);
            lblTextoPorVencer.Name = "lblTextoPorVencer";
            lblTextoPorVencer.Size = new Size(65, 15);
            lblTextoPorVencer.TabIndex = 24;
            lblTextoPorVencer.Text = "Por vencer";
            // 
            // lblColorPorVencer
            // 
            lblColorPorVencer.BackColor = Color.Khaki;
            lblColorPorVencer.BorderStyle = BorderStyle.FixedSingle;
            lblColorPorVencer.Location = new Point(100, 6);
            lblColorPorVencer.Name = "lblColorPorVencer";
            lblColorPorVencer.Size = new Size(18, 18);
            lblColorPorVencer.TabIndex = 23;
            // 
            // lblTextoVigente
            // 
            lblTextoVigente.AutoSize = true;
            lblTextoVigente.Location = new Point(24, 8);
            lblTextoVigente.Name = "lblTextoVigente";
            lblTextoVigente.Size = new Size(48, 15);
            lblTextoVigente.TabIndex = 22;
            lblTextoVigente.Text = "Vigente";
            // 
            // lblColorVigente
            // 
            lblColorVigente.BackColor = Color.LightGreen;
            lblColorVigente.BorderStyle = BorderStyle.FixedSingle;
            lblColorVigente.Location = new Point(0, 6);
            lblColorVigente.Name = "lblColorVigente";
            lblColorVigente.Size = new Size(18, 18);
            lblColorVigente.TabIndex = 21;
            // 
            // btnAgregarTrabajador
            // 
            btnAgregarTrabajador.Location = new Point(3, 526);
            btnAgregarTrabajador.Name = "btnAgregarTrabajador";
            btnAgregarTrabajador.Size = new Size(88, 39);
            btnAgregarTrabajador.TabIndex = 21;
            btnAgregarTrabajador.Text = "Agregar";
            btnAgregarTrabajador.UseVisualStyleBackColor = true;
            btnAgregarTrabajador.Click += btnAgregarTrabajador_Click;
            // 
            // btnModificarTrabajador
            // 
            btnModificarTrabajador.Location = new Point(97, 526);
            btnModificarTrabajador.Name = "btnModificarTrabajador";
            btnModificarTrabajador.Size = new Size(88, 39);
            btnModificarTrabajador.TabIndex = 22;
            btnModificarTrabajador.Text = "Modificar";
            btnModificarTrabajador.UseVisualStyleBackColor = true;
            btnModificarTrabajador.Click += btnModificarTrabajador_Click;
            // 
            // btnBorrarTrabajador
            // 
            btnBorrarTrabajador.Location = new Point(191, 526);
            btnBorrarTrabajador.Name = "btnBorrarTrabajador";
            btnBorrarTrabajador.Size = new Size(88, 39);
            btnBorrarTrabajador.TabIndex = 23;
            btnBorrarTrabajador.Text = "Borrar";
            btnBorrarTrabajador.UseVisualStyleBackColor = true;
            btnBorrarTrabajador.Click += btnBorrarTrabajador_Click;
            // 
            // btnVistaPreviaCredencial
            // 
            btnVistaPreviaCredencial.Location = new Point(400, 526);
            btnVistaPreviaCredencial.Name = "btnVistaPreviaCredencial";
            btnVistaPreviaCredencial.Size = new Size(109, 39);
            btnVistaPreviaCredencial.TabIndex = 24;
            btnVistaPreviaCredencial.Text = "Vista previa credencial";
            btnVistaPreviaCredencial.UseVisualStyleBackColor = true;
            btnVistaPreviaCredencial.Click += btnVistaPreviaCredencial_Click;
            // 
            // btnImprimirCredencial
            // 
            btnImprimirCredencial.Location = new Point(515, 526);
            btnImprimirCredencial.Name = "btnImprimirCredencial";
            btnImprimirCredencial.Size = new Size(109, 39);
            btnImprimirCredencial.TabIndex = 25;
            btnImprimirCredencial.Text = "Imprimir credencial";
            btnImprimirCredencial.UseVisualStyleBackColor = true;
            btnImprimirCredencial.Click += btnImprimirCredencial_Click;
            // 
            // lblAvisoLimite
            // 
            lblAvisoLimite.ForeColor = Color.DimGray;
            lblAvisoLimite.Location = new Point(3, 47);
            lblAvisoLimite.Name = "lblAvisoLimite";
            lblAvisoLimite.Size = new Size(1030, 22);
            lblAvisoLimite.TabIndex = 26;
            // 
            // btnLimpiarBusqueda
            // 
            btnLimpiarBusqueda.Location = new Point(945, 5);
            btnLimpiarBusqueda.Name = "btnLimpiarBusqueda";
            btnLimpiarBusqueda.Size = new Size(88, 39);
            btnLimpiarBusqueda.TabIndex = 27;
            btnLimpiarBusqueda.Text = "Limpiar";
            btnLimpiarBusqueda.UseVisualStyleBackColor = true;
            btnLimpiarBusqueda.Click += btnLimpiarBusqueda_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(851, 5);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(88, 39);
            btnBuscar.TabIndex = 28;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // Certificaciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 577);
            Controls.Add(btnBuscar);
            Controls.Add(btnLimpiarBusqueda);
            Controls.Add(lblAvisoLimite);
            Controls.Add(btnImprimirCredencial);
            Controls.Add(btnVistaPreviaCredencial);
            Controls.Add(btnBorrarTrabajador);
            Controls.Add(btnModificarTrabajador);
            Controls.Add(btnAgregarTrabajador);
            Controls.Add(pnlLeyendaEstados);
            Controls.Add(lblTrabajadorSeleccionado);
            Controls.Add(lblLeyendaEstados);
            Controls.Add(lblResumenEstados);
            Controls.Add(cbMostrarPor);
            Controls.Add(lblMostrarPor);
            Controls.Add(btnRegresar);
            Controls.Add(btnCertificaciones);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(dgvTrabajadores);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Certificaciones";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Certificaciones";
            Load += Certificaciones_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).EndInit();
            cmsTrabajadores.ResumeLayout(false);
            pnlLeyendaEstados.ResumeLayout(false);
            pnlLeyendaEstados.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRegresar;
        private Button btnBorrar;
        private Button btnModificar;
        private Button btnCertificaciones;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private DataGridView dgvTrabajadores;
        private Label lblMostrarPor;
        private ComboBox cbMostrarPor;
        private Label lblResumenEstados;
        private Label lblLeyendaEstados;
        private Label lblTrabajadorSeleccionado;
        private ToolTip ttAyuda;
        private Panel pnlLeyendaEstados;
        private Label lblColorVigente;
        private Label lblTextoSinCertificar;
        private Label lblColorSinCertificar;
        private Label lblTextoVencida;
        private Label lblColorVencida;
        private Label lblTextoPorVencer;
        private Label lblColorPorVencer;
        private Label lblTextoVigente;
        private Button btnAgregarTrabajador;
        private Button btnModificarTrabajador;
        private Button btnBorrarTrabajador;
        private Button btnVistaPreviaCredencial;
        private Button btnImprimirCredencial;
        private ContextMenuStrip cmsTrabajadores;
        private ToolStripMenuItem tsmVerCertificaciones;
        private ToolStripMenuItem tsmVistaPreviaCredencial;
        private ToolStripMenuItem tsmImprimirCredencial;
        private ToolStripMenuItem tsmModificarTrabajador;
        private Label lblAvisoLimite;
        private Button btnLimpiarBusqueda;
        private Button btnBuscar;
    }
}