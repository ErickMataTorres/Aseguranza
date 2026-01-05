namespace Aseguranza.Ventanas
{
    partial class Localidades
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
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnAgregar = new Button();
            btnModificar = new Button();
            btnBorrar = new Button();
            btnRegresar = new Button();
            dgvLocalidades = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvLocalidades).BeginInit();
            SuspendLayout();
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 9);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 1;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(67, 6);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(452, 21);
            txtBuscar.TabIndex = 0;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(12, 322);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 1;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(106, 322);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 39);
            btnModificar.TabIndex = 2;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(200, 322);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 3;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            btnBorrar.Click += btnBorrar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(431, 322);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 4;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // dgvLocalidades
            // 
            dgvLocalidades.AllowUserToAddRows = false;
            dgvLocalidades.AllowUserToDeleteRows = false;
            dgvLocalidades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLocalidades.Location = new Point(12, 33);
            dgvLocalidades.MultiSelect = false;
            dgvLocalidades.Name = "dgvLocalidades";
            dgvLocalidades.ReadOnly = true;
            dgvLocalidades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLocalidades.Size = new Size(507, 283);
            dgvLocalidades.TabIndex = 7;
            dgvLocalidades.TabStop = false;
            dgvLocalidades.CellDoubleClick += dgvLocalidades_CellDoubleClick;
            // 
            // Localidades
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(533, 371);
            Controls.Add(dgvLocalidades);
            Controls.Add(btnRegresar);
            Controls.Add(btnBorrar);
            Controls.Add(btnModificar);
            Controls.Add(btnAgregar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Localidades";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Localidades";
            Load += Localidades_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLocalidades).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnAgregar;
        private Button btnModificar;
        private Button btnBorrar;
        private Button btnRegresar;
        private DataGridView dgvLocalidades;
    }
}