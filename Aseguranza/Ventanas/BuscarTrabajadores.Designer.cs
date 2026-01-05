namespace Aseguranza.Ventanas
{
    partial class BuscarTrabajadores
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
            label1 = new Label();
            txtBuscar = new TextBox();
            dgvTrabajadores = new DataGridView();
            btnRegresar = new Button();
            btnAceptar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 0;
            label1.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(67, 6);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(721, 21);
            txtBuscar.TabIndex = 0;
            txtBuscar.KeyPress += txtBuscar_KeyPress;
            // 
            // dgvTrabajadores
            // 
            dgvTrabajadores.AllowUserToAddRows = false;
            dgvTrabajadores.AllowUserToDeleteRows = false;
            dgvTrabajadores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTrabajadores.Location = new Point(12, 33);
            dgvTrabajadores.Name = "dgvTrabajadores";
            dgvTrabajadores.ReadOnly = true;
            dgvTrabajadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrabajadores.Size = new Size(776, 343);
            dgvTrabajadores.TabIndex = 2;
            dgvTrabajadores.TabStop = false;
            dgvTrabajadores.CellDoubleClick += dgvTrabajadores_CellDoubleClick;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(390, 382);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 2;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(296, 382);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 1;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // BuscarTrabajadores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 430);
            Controls.Add(btnRegresar);
            Controls.Add(btnAceptar);
            Controls.Add(dgvTrabajadores);
            Controls.Add(txtBuscar);
            Controls.Add(label1);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BuscarTrabajadores";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Buscar trabajador";
            Load += BuscarTrabajadores_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTrabajadores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtBuscar;
        private DataGridView dgvTrabajadores;
        private Button btnRegresar;
        private Button btnAceptar;
    }
}