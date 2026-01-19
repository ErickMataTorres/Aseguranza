using Aseguranza.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aseguranza.Ventanas
{
    public partial class CredencialCertificacion : Form
    {
        private Clases.Trabajador trabajadorActual;
        public CredencialCertificacion(Clases.Trabajador trabajador)
        {
            InitializeComponent();
            this.trabajadorActual = trabajador;
        }

        private void CredencialCertificacion_Load(object sender, EventArgs e)
        {
            lblMostrarNoReloj.Text = trabajadorActual.NoReloj!.ToString();
            lblMostrarNombre.Text = trabajadorActual.Nombre;

            if (File.Exists(trabajadorActual.RutaFoto))
            {
                pictureBox1.ImageLocation = trabajadorActual.RutaFoto;
            }
        }

        private void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(pnlCredencial.Width, pnlCredencial.Height);
            pnlCredencial.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            e.Graphics?.DrawImage(bmp, 0, 0);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }
    }
}
