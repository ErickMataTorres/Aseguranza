namespace Aseguranza.Ventanas
{
    partial class Verificaciones
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
            webViewCertificacion = new Microsoft.Web.WebView2.WinForms.WebView2();
            lblRevisionReverso = new Label();
            dgvCertificaciones = new DataGridView();
            pnlReverso = new Panel();
            pbContec = new PictureBox();
            pictureBox1 = new PictureBox();
            lblRevision = new Label();
            pnlNombre = new Panel();
            lblMostrarNombre = new Label();
            lblNombre = new Label();
            lblProcesos = new Label();
            lblMostrarNoReloj = new Label();
            flowProcesosCertificados = new FlowLayoutPanel();
            pnlAdelante = new Panel();
            lblCertificacionProcesos = new Label();
            txtNombre = new TextBox();
            lblTrabajador = new Label();
            txtNoReloj = new TextBox();
            lblNoReloj = new Label();
            lblVerificador = new Label();
            btnImprimirHtml = new Button();
            ((System.ComponentModel.ISupportInitialize)webViewCertificacion).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCertificaciones).BeginInit();
            pnlReverso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbContec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlNombre.SuspendLayout();
            pnlAdelante.SuspendLayout();
            SuspendLayout();
            // 
            // webViewCertificacion
            // 
            webViewCertificacion.AllowExternalDrop = true;
            webViewCertificacion.CreationProperties = null;
            webViewCertificacion.DefaultBackgroundColor = Color.White;
            webViewCertificacion.Location = new Point(12, 138);
            webViewCertificacion.Name = "webViewCertificacion";
            webViewCertificacion.Size = new Size(1103, 477);
            webViewCertificacion.TabIndex = 65;
            webViewCertificacion.ZoomFactor = 1D;
            // 
            // lblRevisionReverso
            // 
            lblRevisionReverso.AutoSize = true;
            lblRevisionReverso.Location = new Point(3, 292);
            lblRevisionReverso.Name = "lblRevisionReverso";
            lblRevisionReverso.Size = new Size(229, 15);
            lblRevisionReverso.TabIndex = 65;
            lblRevisionReverso.Text = "EC06 I01 CTS MCH-35  REV.2 (Reverso)";
            // 
            // dgvCertificaciones
            // 
            dgvCertificaciones.AllowUserToAddRows = false;
            dgvCertificaciones.AllowUserToDeleteRows = false;
            dgvCertificaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCertificaciones.Location = new Point(3, 32);
            dgvCertificaciones.Name = "dgvCertificaciones";
            dgvCertificaciones.ReadOnly = true;
            dgvCertificaciones.Size = new Size(506, 257);
            dgvCertificaciones.TabIndex = 65;
            // 
            // pnlReverso
            // 
            pnlReverso.BackColor = Color.White;
            pnlReverso.BorderStyle = BorderStyle.FixedSingle;
            pnlReverso.Controls.Add(dgvCertificaciones);
            pnlReverso.Controls.Add(lblRevisionReverso);
            pnlReverso.Location = new Point(566, 654);
            pnlReverso.Name = "pnlReverso";
            pnlReverso.Size = new Size(514, 318);
            pnlReverso.TabIndex = 64;
            pnlReverso.Visible = false;
            // 
            // pbContec
            // 
            pbContec.BorderStyle = BorderStyle.FixedSingle;
            pbContec.Image = Properties.Resources.Contec;
            pbContec.Location = new Point(3, 62);
            pbContec.Name = "pbContec";
            pbContec.Size = new Size(60, 56);
            pbContec.SizeMode = PictureBoxSizeMode.StretchImage;
            pbContec.TabIndex = 62;
            pbContec.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(362, 62);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(128, 138);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 63;
            pictureBox1.TabStop = false;
            // 
            // lblRevision
            // 
            lblRevision.AutoSize = true;
            lblRevision.Location = new Point(3, 292);
            lblRevision.Name = "lblRevision";
            lblRevision.Size = new Size(172, 15);
            lblRevision.TabIndex = 66;
            lblRevision.Text = "EC06 I01 CTS MCH-35  REV.2\t\t\t\t";
            // 
            // pnlNombre
            // 
            pnlNombre.BorderStyle = BorderStyle.FixedSingle;
            pnlNombre.Controls.Add(lblMostrarNombre);
            pnlNombre.Controls.Add(lblNombre);
            pnlNombre.Location = new Point(3, 124);
            pnlNombre.Name = "pnlNombre";
            pnlNombre.Size = new Size(353, 76);
            pnlNombre.TabIndex = 65;
            // 
            // lblMostrarNombre
            // 
            lblMostrarNombre.AutoSize = true;
            lblMostrarNombre.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            lblMostrarNombre.ForeColor = Color.Navy;
            lblMostrarNombre.Location = new Point(3, 31);
            lblMostrarNombre.Name = "lblMostrarNombre";
            lblMostrarNombre.Size = new Size(119, 18);
            lblMostrarNombre.TabIndex = 68;
            lblMostrarNombre.Text = "MostrarNombre";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(3, 5);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(62, 15);
            lblNombre.TabIndex = 67;
            lblNombre.Text = "NOMBRE:";
            // 
            // lblProcesos
            // 
            lblProcesos.AutoSize = true;
            lblProcesos.Location = new Point(3, 203);
            lblProcesos.Name = "lblProcesos";
            lblProcesos.Size = new Size(169, 15);
            lblProcesos.TabIndex = 69;
            lblProcesos.Text = "PROCESOS CERTIFICADOS:";
            // 
            // lblMostrarNoReloj
            // 
            lblMostrarNoReloj.AutoSize = true;
            lblMostrarNoReloj.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMostrarNoReloj.ForeColor = Color.Navy;
            lblMostrarNoReloj.Location = new Point(136, 81);
            lblMostrarNoReloj.Name = "lblMostrarNoReloj";
            lblMostrarNoReloj.Size = new Size(164, 24);
            lblMostrarNoReloj.TabIndex = 69;
            lblMostrarNoReloj.Text = "MostrarNoReloj";
            // 
            // flowProcesosCertificados
            // 
            flowProcesosCertificados.Location = new Point(7, 221);
            flowProcesosCertificados.Name = "flowProcesosCertificados";
            flowProcesosCertificados.Size = new Size(483, 68);
            flowProcesosCertificados.TabIndex = 65;
            // 
            // pnlAdelante
            // 
            pnlAdelante.BackColor = Color.White;
            pnlAdelante.BorderStyle = BorderStyle.FixedSingle;
            pnlAdelante.Controls.Add(flowProcesosCertificados);
            pnlAdelante.Controls.Add(lblMostrarNoReloj);
            pnlAdelante.Controls.Add(lblProcesos);
            pnlAdelante.Controls.Add(pnlNombre);
            pnlAdelante.Controls.Add(lblRevision);
            pnlAdelante.Controls.Add(pictureBox1);
            pnlAdelante.Controls.Add(pbContec);
            pnlAdelante.Controls.Add(lblCertificacionProcesos);
            pnlAdelante.Location = new Point(46, 654);
            pnlAdelante.Name = "pnlAdelante";
            pnlAdelante.Size = new Size(514, 318);
            pnlAdelante.TabIndex = 62;
            pnlAdelante.Visible = false;
            // 
            // lblCertificacionProcesos
            // 
            lblCertificacionProcesos.AutoSize = true;
            lblCertificacionProcesos.BackColor = Color.Navy;
            lblCertificacionProcesos.Font = new Font("Arial", 17.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCertificacionProcesos.ForeColor = Color.White;
            lblCertificacionProcesos.Location = new Point(6, 32);
            lblCertificacionProcesos.Name = "lblCertificacionProcesos";
            lblCertificacionProcesos.Size = new Size(480, 27);
            lblCertificacionProcesos.TabIndex = 61;
            lblCertificacionProcesos.Text = "CERTIFICACION DE PROCESOS CRITICOS";
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.Navy;
            txtNombre.Font = new Font("Arial", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNombre.ForeColor = Color.White;
            txtNombre.Location = new Point(425, 95);
            txtNombre.Multiline = true;
            txtNombre.Name = "txtNombre";
            txtNombre.ReadOnly = true;
            txtNombre.Size = new Size(433, 37);
            txtNombre.TabIndex = 60;
            // 
            // lblTrabajador
            // 
            lblTrabajador.AutoSize = true;
            lblTrabajador.BackColor = Color.Navy;
            lblTrabajador.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTrabajador.ForeColor = Color.White;
            lblTrabajador.Location = new Point(227, 95);
            lblTrabajador.Name = "lblTrabajador";
            lblTrabajador.Size = new Size(192, 37);
            lblTrabajador.TabIndex = 59;
            lblTrabajador.Text = "Trabajador:";
            // 
            // txtNoReloj
            // 
            txtNoReloj.BackColor = Color.FromArgb(255, 255, 192);
            txtNoReloj.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNoReloj.Location = new Point(425, 58);
            txtNoReloj.MinimumSize = new Size(433, 37);
            txtNoReloj.Multiline = true;
            txtNoReloj.Name = "txtNoReloj";
            txtNoReloj.Size = new Size(433, 37);
            txtNoReloj.TabIndex = 58;
            txtNoReloj.TextChanged += txtNoReloj_TextChanged;
            txtNoReloj.KeyPress += txtNoReloj_KeyPress;
            // 
            // lblNoReloj
            // 
            lblNoReloj.AutoSize = true;
            lblNoReloj.BackColor = Color.Yellow;
            lblNoReloj.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNoReloj.ForeColor = Color.Black;
            lblNoReloj.Location = new Point(224, 58);
            lblNoReloj.Name = "lblNoReloj";
            lblNoReloj.Size = new Size(195, 37);
            lblNoReloj.TabIndex = 57;
            lblNoReloj.Text = "NO. RELOJ:";
            // 
            // lblVerificador
            // 
            lblVerificador.AutoSize = true;
            lblVerificador.BackColor = Color.Navy;
            lblVerificador.Font = new Font("Arial", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblVerificador.ForeColor = Color.White;
            lblVerificador.Location = new Point(266, 9);
            lblVerificador.Name = "lblVerificador";
            lblVerificador.Size = new Size(570, 46);
            lblVerificador.TabIndex = 56;
            lblVerificador.Text = "Verificador de certificaciones";
            // 
            // btnImprimirHtml
            // 
            btnImprimirHtml.Location = new Point(864, 95);
            btnImprimirHtml.Name = "btnImprimirHtml";
            btnImprimirHtml.Size = new Size(88, 39);
            btnImprimirHtml.TabIndex = 66;
            btnImprimirHtml.Text = "Imprimir";
            btnImprimirHtml.UseVisualStyleBackColor = true;
            btnImprimirHtml.Click += btnImprimirHtml_Click;
            // 
            // Verificaciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1127, 629);
            Controls.Add(btnImprimirHtml);
            Controls.Add(webViewCertificacion);
            Controls.Add(pnlReverso);
            Controls.Add(pnlAdelante);
            Controls.Add(txtNombre);
            Controls.Add(lblTrabajador);
            Controls.Add(txtNoReloj);
            Controls.Add(lblNoReloj);
            Controls.Add(lblVerificador);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Verificaciones";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Verificaciones";
            Load += Verificaciones_Load;
            ((System.ComponentModel.ISupportInitialize)webViewCertificacion).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCertificaciones).EndInit();
            pnlReverso.ResumeLayout(false);
            pnlReverso.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbContec).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlNombre.ResumeLayout(false);
            pnlNombre.PerformLayout();
            pnlAdelante.ResumeLayout(false);
            pnlAdelante.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webViewCertificacion;
        private Label lblRevisionReverso;
        private DataGridView dgvCertificaciones;
        private Panel pnlReverso;
        private PictureBox pbContec;
        private PictureBox pictureBox1;
        private Label lblRevision;
        private Panel pnlNombre;
        private Label lblMostrarNombre;
        private Label lblNombre;
        private Label lblProcesos;
        private Label lblMostrarNoReloj;
        private FlowLayoutPanel flowProcesosCertificados;
        private Panel pnlAdelante;
        private Label lblCertificacionProcesos;
        private TextBox txtNombre;
        private Label lblTrabajador;
        private TextBox txtNoReloj;
        private Label lblNoReloj;
        private Label lblVerificador;
        private Button btnImprimirHtml;
    }
}