using Capa_N.EntityProv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capa_P
{
    public partial class Clientes : Form
    {
        // Instanciar las clases que se vayan a utiliar
        public Cliente cl = new Cliente();

        public Clientes()
        {
            InitializeComponent();
            CargarClinete();
        }

        private void CargarClinete()
        {
            // Llamar la clase y el metodo
            DataTable dt = cl.ListadoClientes();
            //Ponerlo como datasource
            dtaClientes.DataSource = dt;

        }

        private void btnGuardarClienteR_Click(object sender, EventArgs e)
        {
            String msj = "";

            try
            {

                cl.Nombre = txtCliente_NombreR.Text;
                cl.Rnc = txtRnc.Text;
                cl.Correo = txtCliente_CorreoR.Text;
                cl.Direccion = txtTelefono.Text;

                msj = cl.RegistrarClientes();

                MessageBox.Show(msj);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }
    }
}
