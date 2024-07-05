using System;
using System.Windows.Forms;



namespace Capa_P
{
    public partial class Layout : Form
    {
        public Layout()
        {
            InitializeComponent();
            abrirformhija(new Ventas());

        }

        private void Layout_Load(object sender, EventArgs e)
        {
            btnDashboard.Size = new System.Drawing.Size(230, 39);
            btnCliente.Size = new System.Drawing.Size(230, 39);
            btnVentas.Size = new System.Drawing.Size(230, 39);
            btnNCF.Size = new System.Drawing.Size(230, 39);
            btnFacturas.Size = new System.Drawing.Size(230, 39);
            btnMateriales.Size = new System.Drawing.Size(230, 39);
            btnUsuarios.Size = new System.Drawing.Size(230, 39);
        }

        public void abrirformhija(object formhija)
        {
            if (this.bunifuPanel4.Controls.Count > 0)

                this.bunifuPanel4.Controls.RemoveAt(0);
            Form fh = formhija as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.bunifuPanel4.Controls.Add(fh);
            this.bunifuPanel4.Tag = fh;

            fh.Show();

        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            abrirformhija(new Ventas());
        }

        private void Layout_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            abrirformhija(new Dashboard());
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            abrirformhija(new Clientes());
        }

        private void btnNCF_Click(object sender, EventArgs e)
        {
            abrirformhija(new NCF());
        }

        private void btnFabricacion_Click(object sender, EventArgs e)
        {
            abrirformhija(new Fabricacion());
        }

        private void btnFacturas_Click(object sender, EventArgs e)
        {
            abrirformhija(new Facturas());
        }   

        private void Responsie()
        {
             
        }


    }
}
