namespace Aseguranza.Ventanas
{
    partial class Procesos
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
            dgvProcesos = new DataGridView();
            btnRegresar = new Button();
            btnBorrar = new Button();
            btnModificar = new Button();
            btnAgregar = new Button();
            txtBuscar = new TextBox();
            lblBuscar = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProcesos).BeginInit();
            SuspendLayout();
            // 
            // dgvProcesos
            // 
            dgvProcesos.AllowUserToAddRows = false;
            dgvProcesos.AllowUserToDeleteRows = false;
            dgvProcesos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProcesos.Location = new Point(12, 39);
            dgvProcesos.MultiSelect = false;
            dgvProcesos.Name = "dgvProcesos";
            dgvProcesos.ReadOnly = true;
            dgvProcesos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProcesos.Size = new Size(776, 283);
            dgvProcesos.TabIndex = 14;
            dgvProcesos.TabStop = false;
            dgvProcesos.CellDoubleClick += dgvProcesos_CellDoubleClick;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(700, 328);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 4;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(200, 328);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 3;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            btnBorrar.Click += btnBorrar_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(106, 328);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 39);
            btnModificar.TabIndex = 2;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(12, 328);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 1;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(67, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(721, 21);
            txtBuscar.TabIndex = 0;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 15);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 10;
            lblBuscar.Text = "Buscar:";
            // 
            // Procesos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 378);
            Controls.Add(dgvProcesos);
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
            Name = "Procesos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Procesos";
            Load += Procesos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProcesos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProcesos;
        private Button btnRegresar;
        private Button btnBorrar;
        private Button btnModificar;
        private Button btnAgregar;
        private TextBox txtBuscar;
        private Label lblBuscar;
    }
}