namespace Aseguranza.Ventanas
{
    partial class LineasVentana
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
            btnAceptar = new Button();
            btnRegresar = new Button();
            txtNombre = new TextBox();
            lblNombre = new Label();
            lblPlanta = new Label();
            cbPlantas = new ComboBox();
            SuspendLayout();
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(111, 120);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(205, 120);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 3;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(78, 70);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(291, 21);
            txtNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(17, 73);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(55, 15);
            lblNombre.TabIndex = 9;
            lblNombre.Text = "Nombre:";
            // 
            // lblPlanta
            // 
            lblPlanta.AutoSize = true;
            lblPlanta.Location = new Point(27, 36);
            lblPlanta.Name = "lblPlanta";
            lblPlanta.Size = new Size(45, 15);
            lblPlanta.TabIndex = 13;
            lblPlanta.Text = "Planta:";
            // 
            // cbPlantas
            // 
            cbPlantas.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPlantas.FormattingEnabled = true;
            cbPlantas.Location = new Point(78, 33);
            cbPlantas.Name = "cbPlantas";
            cbPlantas.Size = new Size(291, 23);
            cbPlantas.TabIndex = 0;
            // 
            // LineasVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(404, 199);
            Controls.Add(cbPlantas);
            Controls.Add(lblPlanta);
            Controls.Add(btnAceptar);
            Controls.Add(btnRegresar);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LineasVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lineas";
            Load += LineasVentana_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAceptar;
        private Button btnRegresar;
        private TextBox txtNombre;
        private Label lblNombre;
        private Label lblPlanta;
        private ComboBox cbPlantas;
    }
}