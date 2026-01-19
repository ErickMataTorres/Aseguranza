namespace Aseguranza.Ventanas
{
    partial class CredencialCertificacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredencialCertificacion));
            pnlCredencial = new Panel();
            pnlAdelante = new Panel();
            lblMostrarNombre = new Label();
            pbContec = new PictureBox();
            lblRevision = new Label();
            lblMostrarNoReloj = new Label();
            lblNombre = new Label();
            lblTitulo = new Label();
            pictureBox1 = new PictureBox();
            btnImprimir = new Button();
            btnVistaPrevia = new Button();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printPreviewDialog1 = new PrintPreviewDialog();
            pnlReverso = new Panel();
            lblRevisionReverso = new Label();
            pnlCredencial.SuspendLayout();
            pnlAdelante.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbContec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlReverso.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCredencial
            // 
            pnlCredencial.BorderStyle = BorderStyle.FixedSingle;
            pnlCredencial.Controls.Add(pnlReverso);
            pnlCredencial.Controls.Add(pnlAdelante);
            pnlCredencial.Controls.Add(btnImprimir);
            pnlCredencial.Controls.Add(btnVistaPrevia);
            pnlCredencial.Dock = DockStyle.Fill;
            pnlCredencial.Location = new Point(0, 0);
            pnlCredencial.Name = "pnlCredencial";
            pnlCredencial.Size = new Size(1084, 781);
            pnlCredencial.TabIndex = 0;
            // 
            // pnlAdelante
            // 
            pnlAdelante.BorderStyle = BorderStyle.FixedSingle;
            pnlAdelante.Controls.Add(lblMostrarNombre);
            pnlAdelante.Controls.Add(pbContec);
            pnlAdelante.Controls.Add(lblRevision);
            pnlAdelante.Controls.Add(lblMostrarNoReloj);
            pnlAdelante.Controls.Add(lblNombre);
            pnlAdelante.Controls.Add(lblTitulo);
            pnlAdelante.Controls.Add(pictureBox1);
            pnlAdelante.Location = new Point(11, 17);
            pnlAdelante.Name = "pnlAdelante";
            pnlAdelante.Size = new Size(583, 315);
            pnlAdelante.TabIndex = 57;
            // 
            // lblMostrarNombre
            // 
            lblMostrarNombre.AutoSize = true;
            lblMostrarNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNombre.ForeColor = SystemColors.ControlText;
            lblMostrarNombre.Location = new Point(89, 115);
            lblMostrarNombre.Name = "lblMostrarNombre";
            lblMostrarNombre.Size = new Size(119, 18);
            lblMostrarNombre.TabIndex = 57;
            lblMostrarNombre.Text = "MostrarNombre";
            // 
            // pbContec
            // 
            pbContec.BorderStyle = BorderStyle.FixedSingle;
            pbContec.Image = Properties.Resources.Contec;
            pbContec.Location = new Point(15, 43);
            pbContec.Name = "pbContec";
            pbContec.Size = new Size(60, 56);
            pbContec.SizeMode = PictureBoxSizeMode.StretchImage;
            pbContec.TabIndex = 55;
            pbContec.TabStop = false;
            // 
            // lblRevision
            // 
            lblRevision.AutoSize = true;
            lblRevision.Location = new Point(8, 277);
            lblRevision.Name = "lblRevision";
            lblRevision.Size = new Size(172, 15);
            lblRevision.TabIndex = 56;
            lblRevision.Text = "EC06 I01 CTS MCH-35  REV.2\t\t\t\t";
            // 
            // lblMostrarNoReloj
            // 
            lblMostrarNoReloj.AutoSize = true;
            lblMostrarNoReloj.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNoReloj.Location = new Point(437, 241);
            lblMostrarNoReloj.Name = "lblMostrarNoReloj";
            lblMostrarNoReloj.Size = new Size(120, 18);
            lblMostrarNoReloj.TabIndex = 40;
            lblMostrarNoReloj.Text = "MostrarNoReloj";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblNombre.Location = new Point(15, 115);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(68, 18);
            lblNombre.TabIndex = 41;
            lblNombre.Text = "Nombre:";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Navy;
            lblTitulo.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(92, 49);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(317, 18);
            lblTitulo.TabIndex = 49;
            lblTitulo.Text = "CERTIFICACION DE PROCESOS CRITICOS";
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(427, 49);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(140, 180);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 46;
            pictureBox1.TabStop = false;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(983, 62);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(88, 39);
            btnImprimir.TabIndex = 48;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnVistaPrevia
            // 
            btnVistaPrevia.Location = new Point(983, 17);
            btnVistaPrevia.Name = "btnVistaPrevia";
            btnVistaPrevia.Size = new Size(88, 39);
            btnVistaPrevia.TabIndex = 47;
            btnVistaPrevia.Text = "Vista previa";
            btnVistaPrevia.UseVisualStyleBackColor = true;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            // 
            // printDocument1
            // 
            printDocument1.PrintPage += printDocument1_PrintPage;
            // 
            // printPreviewDialog1
            // 
            printPreviewDialog1.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog1.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog1.ClientSize = new Size(400, 300);
            printPreviewDialog1.Enabled = true;
            printPreviewDialog1.Icon = (Icon)resources.GetObject("printPreviewDialog1.Icon");
            printPreviewDialog1.Name = "printPreviewDialog1";
            printPreviewDialog1.Visible = false;
            // 
            // pnlReverso
            // 
            pnlReverso.BorderStyle = BorderStyle.FixedSingle;
            pnlReverso.Controls.Add(lblRevisionReverso);
            pnlReverso.Location = new Point(11, 338);
            pnlReverso.Name = "pnlReverso";
            pnlReverso.Size = new Size(583, 315);
            pnlReverso.TabIndex = 58;
            // 
            // lblRevisionReverso
            // 
            lblRevisionReverso.AutoSize = true;
            lblRevisionReverso.Location = new Point(8, 277);
            lblRevisionReverso.Name = "lblRevisionReverso";
            lblRevisionReverso.Size = new Size(229, 15);
            lblRevisionReverso.TabIndex = 56;
            lblRevisionReverso.Text = "EC06 I01 CTS MCH-35  REV.2 (Reverso)";
            // 
            // CredencialCertificacion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1084, 781);
            Controls.Add(pnlCredencial);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CredencialCertificacion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Credencial";
            Load += CredencialCertificacion_Load;
            pnlCredencial.ResumeLayout(false);
            pnlAdelante.ResumeLayout(false);
            pnlAdelante.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbContec).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlReverso.ResumeLayout(false);
            pnlReverso.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCredencial;
        private Label lblNombre;
        private Label lblMostrarNoReloj;
        private PictureBox pictureBox1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private PrintPreviewDialog printPreviewDialog1;
        private Button btnVistaPrevia;
        private Button btnImprimir;
        private Label lblTitulo;
        private Label lblRevision;
        private PictureBox pbContec;
        private Panel pnlAdelante;
        private Label lblMostrarNombre;
        private Panel pnlReverso;
        private Label lblRevisionReverso;
    }
}