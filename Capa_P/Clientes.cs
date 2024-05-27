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
        public Tipo_Madera tipomadera = new Tipo_Madera();

        public Clientes()
        {
            InitializeComponent();
            //Cargar La funcion
            CargarClinete();
        }

        private void CargarClinete()
        {
            // Llamar la clase y el metodo
            DataTable dt = tipomadera.ListadoMadera();
            //Ponerlo como datasource
            dtaClientes.DataSource = dt;
            
        }

    }
}
