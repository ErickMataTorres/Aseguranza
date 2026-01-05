namespace Aseguranza.Ventanas
{
    partial class ProcesosVentana
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
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            txtVigencia = new TextBox();
            lblVigencia = new Label();
            SuspendLayout();
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(150, 173);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 39);
            btnAceptar.TabIndex = 3;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnRegresar
            // 
            btnRegresar.Location = new Point(244, 173);
            btnRegresar.Name = "btnRegresar";
            btnRegresar.Size = new Size(88, 39);
            btnRegresar.TabIndex = 4;
            btnRegresar.Text = "Regresar";
            btnRegresar.UseVisualStyleBackColor = true;
            btnRegresar.Click += btnRegresar_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(138, 28);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(326, 21);
            txtNombre.TabIndex = 0;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(77, 31);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(55, 15);
            lblNombre.TabIndex = 9;
            lblNombre.Text = "Nombre:";
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(56, 79);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(76, 15);
            lblDescripcion.TabIndex = 13;
            lblDescripcion.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(138, 76);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(326, 21);
            txtDescripcion.TabIndex = 1;
            // 
            // txtVigencia
            // 
            txtVigencia.Location = new Point(138, 124);
            txtVigencia.Name = "txtVigencia";
            txtVigencia.Size = new Size(326, 21);
            txtVigencia.TabIndex = 2;
            txtVigencia.KeyPress += textBox2_KeyPress;
            // 
            // lblVigencia
            // 
            lblVigencia.AutoSize = true;
            lblVigencia.Location = new Point(16, 127);
            lblVigencia.Name = "lblVigencia";
            lblVigencia.Size = new Size(116, 15);
            lblVigencia.TabIndex = 15;
            lblVigencia.Text = "Vigencia en meses:";
            // 
            // ProcesosVentana
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(496, 244);
            Controls.Add(txtVigencia);
            Controls.Add(lblVigencia);
            Controls.Add(txtDescripcion);
            Controls.Add(lblDescripcion);
            Controls.Add(btnAceptar);
            Controls.Add(btnRegresar);
            Controls.Add(txtNombre);
            Controls.Add(lblNombre);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProcesosVentana";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Procesos";
            Load += ProcesosVentana_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAceptar;
        private Button btnRegresar;
        private TextBox txtNombre;
        private Label lblNombre;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private TextBox txtVigencia;
        private Label lblVigencia;
    }
}