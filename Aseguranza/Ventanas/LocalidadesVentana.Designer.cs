namespace Aseguranza.Ventanas
{
    partial class LocalidadesVentana
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
            txtNombre = new TextBox();
            lblNombre = new Label();
            btnRegresar = new Button();
            btnAceptar = new Button();
            SuspendLayout();
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(73, 36);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(317, 21);
            txtNombre.TabIndex = 3;
            txtNombre.TextChanged += txtNombre_TextChanged;
            txtNombre.KeyPress += txtNombre_KeyPress;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(12, 42);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(55, 15);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            //lblNombre.Click += this.lblNombre_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(200, 89);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 7;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(106, 89);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 8;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            btnAceptar.KeyPress += btnAceptar_KeyPress;
            // 
            // LocalidadesVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(402, 161);
            Controls.Add(btnAceptar);
            Controls.Add(btnRegresar);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LocalidadesVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Localidad";
            Load += LocalidadesVentana_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNombre;
        private Label lblNombre;
        private Button btnRegresar;
        private Button btnAceptar;
    }
}