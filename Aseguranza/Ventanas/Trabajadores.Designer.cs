namespace Aseguranza.Ventanas
{
    partial class Trabajadores
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
            btnModificar = new Button();
            btnAgregar = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            dgvTrabajadores = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).BeginInit();
            SuspendLayout();
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(724, 422);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 11;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(202, 422);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 10;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            btnBorrar.Click += btnBorrar_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(108, 422);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 39);
            btnModificar.TabIndex = 9;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(14, 422);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 7;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(69, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(743, 21);
            txtBuscar.TabIndex = 5;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(14, 15);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 8;
            lblBuscar.Text = "Buscar:";
            // 
            // dgvTrabajadores
            // 
            dgvTrabajadores.AllowUserToAddRows = false;
            dgvTrabajadores.AllowUserToDeleteRows = false;
            dgvTrabajadores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrabajadores.Location = new Point(14, 39);
            dgvTrabajadores.MultiSelect = false;
            dgvTrabajadores.Name = "dgvTrabajadores";
            dgvTrabajadores.ReadOnly = true;
            dgvTrabajadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrabajadores.Size = new Size(798, 377);
            dgvTrabajadores.TabIndex = 6;
            dgvTrabajadores.TabStop = false;
            // 
            // Trabajadores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(824, 468);
            Controls.Add(btnRegresar);
            Controls.Add(btnBorrar);
            Controls.Add(btnModificar);
            Controls.Add(btnAgregar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(dgvTrabajadores);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Trabajadores";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Trabajadores";
            Load += Trabajadores_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRegresar;
        private Button btnBorrar;
        private Button btnModificar;
        private Button btnAgregar;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private DataGridView dgvTrabajadores;
    }
}