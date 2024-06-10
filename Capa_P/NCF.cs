using System;
using System.Windows.Forms;

namespace Capa_P
{
    public partial class NCF : Form
    {
        public NCf ncf = new NCf();

        bool existent = false;
        public NCF()
        {
            InitializeComponent();
            CargarNCF();
        }

        private void Limpiar()
        {
            txtNumerosCreditoFiscal.Clear();
        }
        private void CargarNCF()
        {
            System.Data.DataTable dt = ncf.ListadoNCF();
            dtaFiscal.DataSource = dt;

            dtaFiscal.ClearSelection();
        }

        private void GuardarNCF()
        {
            existent = false;
            String msj = "";



            try
            {

                ncf.Codigo = txtNumerosCreditoFiscal.Text;







                if (!string.IsNullOrEmpty(ncf.Codigo))//si el campo ncf esta vacio no permite ingresar datos

                {




                    msj = ncf.RegistrarNCF();

                    MessageBox.Show(msj);



                }
                else
                {
                    MessageBox.Show("Termine de completar los campos Requeridos");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

            CargarNCF();
            Limpiar();


        }

        private void btnGuardarClienteR_Click(object sender, EventArgs e)
        {
            if (existent == false)
            {
                GuardarNCF();
            }

            else
            {
                EditarNCF();
                existent = false;
            }



        }

        private void dtaFiscal_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dtaFiscal.Rows[e.RowIndex];







                ncf.Id = dtaFiscal.CurrentRow.Cells[0].Value.ToString();

            }
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ncf.Id))
            {
                string mensaje = ncf.borrarCliente(ncf.Id);
                MessageBox.Show(mensaje);
                CargarNCF();
                Limpiar();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un registro para eliminar.");
            }
        }
        public void EditarNCF()
        {
            // Validar que los campos de teléfono y RNC contengan solo números si no están vacíos


            try
            {

                if (!string.IsNullOrEmpty(ncf.Id))
                {



                    // Actualizar los valores del cliente con los datos de los campos de texto
                    ncf.Codigo = txtNumerosCreditoFiscal.Text;

                    //condicionante para no ingresar nombres null


                    string mensaje = ncf.EditarNCF(ncf.Id);

                    MessageBox.Show(mensaje);

                    CargarNCF(); // Recargar los datos para reflejar los cambios
                    Limpiar();




                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un registro para editar.");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void RellenarCamposCliente()
        {

            txtNumerosCreditoFiscal.Text = dtaFiscal.CurrentRow.Cells["Codigo"].Value.ToString();

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            RellenarCamposCliente();
            existent = true;
        }

        private void dtaFiscal_SelectionChanged(object sender, EventArgs e)
        {
            if (existent == true)
            {
                DialogResult result = MessageBox.Show("¿Deseas continuar?", "Confirmación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    existent = false;
                    Limpiar();
                }
            }
        }
    }

}
