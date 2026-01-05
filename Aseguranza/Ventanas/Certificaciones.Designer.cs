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
            btnRegresar = new Button();
            btnCertificaciones = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            dgvTrabajadores = new DataGridView();
            lblMostrarPor = new Label();
            cbMostrarPor = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).BeginInit();
            SuspendLayout();
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(713, 416);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 3;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnCertificaciones
            // 
            btnCertificaciones.Location = new Point(3, 416);
            btnCertificaciones.Name = "btnCertificaciones";
            btnCertificaciones.Size = new Size(104, 39);
            btnCertificaciones.TabIndex = 2;
            btnCertificaciones.Text = "Certificaciones";
            btnCertificaciones.UseVisualStyleBackColor = true;
            btnCertificaciones.Click += btnCertificaciones_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(446, 6);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(355, 21);
            txtBuscar.TabIndex = 0;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(391, 9);
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
            dgvTrabajadores.Location = new Point(3, 33);
            dgvTrabajadores.MultiSelect = false;
            dgvTrabajadores.Name = "dgvTrabajadores";
            dgvTrabajadores.ReadOnly = true;
            dgvTrabajadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrabajadores.Size = new Size(798, 377);
            dgvTrabajadores.TabIndex = 14;
            dgvTrabajadores.TabStop = false;
            dgvTrabajadores.CellDoubleClick += dgvTrabajadores_CellDoubleClick;
            dgvTrabajadores.CellFormatting += dgvTrabajadores_CellFormatting;
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
            cbMostrarPor.Items.AddRange(new object[] { "Todas", "Vigentes", "Por vencer", "Vencidas", "Sin certificar" });
            cbMostrarPor.Location = new Point(81, 6);
            cbMostrarPor.Name = "cbMostrarPor";
            cbMostrarPor.Size = new Size(304, 23);
            cbMostrarPor.TabIndex = 17;
            // 
            // Certificaciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 463);
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
    }
}