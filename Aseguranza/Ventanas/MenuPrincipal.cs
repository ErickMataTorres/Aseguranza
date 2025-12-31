namespace Aseguranza
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnCertificaciones_Click(object sender, EventArgs e)
        {
            Ventanas.Certificaciones ventana = new Ventanas.Certificaciones();
            ventana.ShowDialog();
        }

        private void certificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Localidades ventana = new Ventanas.Localidades();
            ventana.ShowDialog();
        }

        private void localidadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Turnos ventana = new Ventanas.Turnos();
            ventana.ShowDialog();
        }

        private void plantaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Plantas ventana = new Ventanas.Plantas();
            ventana.ShowDialog();
        }

        private void lineaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Lineas ventana = new Ventanas.Lineas();
            ventana.ShowDialog();
        }

        private void trabajadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Trabajadores ventana = new Ventanas.Trabajadores();
            ventana.ShowDialog();
        }

        private void certificadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventanas.Certificadores ventana = new Ventanas.Certificadores();
            ventana.ShowDialog();
        }
    }
}
