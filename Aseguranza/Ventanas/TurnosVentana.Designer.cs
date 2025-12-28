namespace Aseguranza.Ventanas
{
    partial class TurnosVentana
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
            SuspendLayout();
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(107, 82);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 12;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(201, 82);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 11;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(78, 32);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(317, 21);
            txtNombre.TabIndex = 10;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(17, 38);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(55, 15);
            lblNombre.TabIndex = 9;
            lblNombre.Text = "Nombre:";
            // 
            // TurnosVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(412, 161);
            Controls.Add(btnAceptar);
            Controls.Add(btnRegresar);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TurnosVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Turno";
            Load += TurnosVentana_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAceptar;
        private Button btnRegresar;
        private TextBox txtNombre;
        private Label lblNombre;
    }
}