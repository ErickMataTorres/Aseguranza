namespace Aseguranza.Ventanas
{
    partial class Plantas
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
            dgvPlantas = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvPlantas).BeginInit();
            SuspendLayout();
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(431, 323);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 11;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnBorrar
            // 
            btnBorrar.Location = new Point(200, 323);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(88, 39);
            btnBorrar.TabIndex = 10;
            btnBorrar.Text = "Borrar";
            btnBorrar.UseVisualStyleBackColor = true;
            btnBorrar.Click += btnBorrar_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(106, 323);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 39);
            btnModificar.TabIndex = 9;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(12, 323);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(88, 39);
            btnAgregar.TabIndex = 7;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(67, 7);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(452, 21);
            txtBuscar.TabIndex = 5;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 10);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(49, 15);
            lblBuscar.TabIndex = 8;
            lblBuscar.Text = "Buscar:";
            // 
            // dgvPlantas
            // 
            dgvPlantas.AllowUserToAddRows = false;
            dgvPlantas.AllowUserToDeleteRows = false;
            dgvPlantas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPlantas.Location = new Point(12, 34);
            dgvPlantas.MultiSelect = false;
            dgvPlantas.Name = "dgvPlantas";
            dgvPlantas.ReadOnly = true;
            dgvPlantas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlantas.Size = new Size(507, 283);
            dgvPlantas.TabIndex = 6;
            dgvPlantas.TabStop = false;
            // 
            // Plantas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(531, 369);
            Controls.Add(btnRegresar);
            Controls.Add(btnBorrar);
            Controls.Add(btnModificar);
            Controls.Add(btnAgregar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(dgvPlantas);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Plantas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Plantas";
            Load += Plantas_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPlantas).EndInit();
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
        private DataGridView dgvPlantas;
    }
}