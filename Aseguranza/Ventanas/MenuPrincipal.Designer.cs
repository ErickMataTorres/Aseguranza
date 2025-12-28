namespace Aseguranza
{
    partial class MenuPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            certificacionesToolStripMenuItem = new ToolStripMenuItem();
            certificarToolStripMenuItem = new ToolStripMenuItem();
            localidadToolStripMenuItem = new ToolStripMenuItem();
            plantaToolStripMenuItem = new ToolStripMenuItem();
            lineaToolStripMenuItem = new ToolStripMenuItem();
            trabajadorToolStripMenuItem = new ToolStripMenuItem();
            certificacionToolStripMenuItem = new ToolStripMenuItem();
            procesoToolStripMenuItem = new ToolStripMenuItem();
            certificadorToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { certificacionesToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(784, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // certificacionesToolStripMenuItem
            // 
            certificacionesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { certificarToolStripMenuItem, localidadToolStripMenuItem, plantaToolStripMenuItem, lineaToolStripMenuItem, trabajadorToolStripMenuItem, certificadorToolStripMenuItem, certificacionToolStripMenuItem, procesoToolStripMenuItem });
            certificacionesToolStripMenuItem.Name = "certificacionesToolStripMenuItem";
            certificacionesToolStripMenuItem.Size = new Size(97, 20);
            certificacionesToolStripMenuItem.Text = "Certificaciones";
            // 
            // certificarToolStripMenuItem
            // 
            certificarToolStripMenuItem.Name = "certificarToolStripMenuItem";
            certificarToolStripMenuItem.Size = new Size(141, 22);
            certificarToolStripMenuItem.Text = "Localidad";
            certificarToolStripMenuItem.Click += certificarToolStripMenuItem_Click;
            // 
            // localidadToolStripMenuItem
            // 
            localidadToolStripMenuItem.Name = "localidadToolStripMenuItem";
            localidadToolStripMenuItem.Size = new Size(141, 22);
            localidadToolStripMenuItem.Text = "Turno";
            localidadToolStripMenuItem.Click += localidadToolStripMenuItem_Click;
            // 
            // plantaToolStripMenuItem
            // 
            plantaToolStripMenuItem.Name = "plantaToolStripMenuItem";
            plantaToolStripMenuItem.Size = new Size(141, 22);
            plantaToolStripMenuItem.Text = "Planta";
            plantaToolStripMenuItem.Click += plantaToolStripMenuItem_Click;
            // 
            // lineaToolStripMenuItem
            // 
            lineaToolStripMenuItem.Name = "lineaToolStripMenuItem";
            lineaToolStripMenuItem.Size = new Size(141, 22);
            lineaToolStripMenuItem.Text = "Linea";
            lineaToolStripMenuItem.Click += lineaToolStripMenuItem_Click;
            // 
            // trabajadorToolStripMenuItem
            // 
            trabajadorToolStripMenuItem.Name = "trabajadorToolStripMenuItem";
            trabajadorToolStripMenuItem.Size = new Size(141, 22);
            trabajadorToolStripMenuItem.Text = "Trabajador";
            trabajadorToolStripMenuItem.Click += trabajadorToolStripMenuItem_Click;
            // 
            // certificacionToolStripMenuItem
            // 
            certificacionToolStripMenuItem.Name = "certificacionToolStripMenuItem";
            certificacionToolStripMenuItem.Size = new Size(141, 22);
            certificacionToolStripMenuItem.Text = "Certificacion";
            // 
            // procesoToolStripMenuItem
            // 
            procesoToolStripMenuItem.Name = "procesoToolStripMenuItem";
            procesoToolStripMenuItem.Size = new Size(141, 22);
            procesoToolStripMenuItem.Text = "Proceso";
            // 
            // certificadorToolStripMenuItem
            // 
            certificadorToolStripMenuItem.Name = "certificadorToolStripMenuItem";
            certificadorToolStripMenuItem.Size = new Size(141, 22);
            certificadorToolStripMenuItem.Text = "Certificador";
            // 
            // MenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 441);
            Controls.Add(menuStrip1);
            Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MenuPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menu";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem certificacionesToolStripMenuItem;
        private ToolStripMenuItem certificarToolStripMenuItem;
        private ToolStripMenuItem localidadToolStripMenuItem;
        private ToolStripMenuItem plantaToolStripMenuItem;
        private ToolStripMenuItem lineaToolStripMenuItem;
        private ToolStripMenuItem trabajadorToolStripMenuItem;
        private ToolStripMenuItem certificacionToolStripMenuItem;
        private ToolStripMenuItem procesoToolStripMenuItem;
        private ToolStripMenuItem certificadorToolStripMenuItem;
    }
}
