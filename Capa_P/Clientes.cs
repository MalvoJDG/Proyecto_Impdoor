using Bunifu.Framework.UI;
using Bunifu.UI.WinForms;
using Bunifu.Utils;
using Capa_N.EntityProv;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;



namespace Capa_P
{
    public partial class Clientes : Form
    {
        // Instanciar las clases que se vayan a utiliar
        public Cliente cl = new Cliente();
        bool existent = false;
        

        private void Limpiar()
        {
            txtCliente_NombreR.Clear();
            txtCliente_CorreoR.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtRnc.Clear();

        }
        public Clientes()
        {
            InitializeComponent();
            CargarClinete();
           
        }   
        private void CargarClinete()
        {
            // Llamar la clase y el metodo
            System.Data.DataTable dt = cl.ListadoClientes();
            //Ponerlo como datasource
            dtaClientes.DataSource = dt;

            dtaClientes.ClearSelection();



        }



        private void GuardarCliente()
        {
            existent = false;
            String msj = "";



            try
            {
                // Validar que los campos de teléfono y RNC contengan solo números si no están vacíos
                if (!string.IsNullOrEmpty(txtTelefono.Text) && !Regex.IsMatch(txtTelefono.Text, @"^\d+$"))
                {
                    MessageBox.Show("El campo Teléfono solo debe contener números.");
                    return;
                }

                if (!string.IsNullOrEmpty(txtRnc.Text) && !Regex.IsMatch(txtRnc.Text, @"^\d+$"))
                {
                    MessageBox.Show("El campo RNC solo debe contener números.");
                    return;
                }

                cl.Nombre = txtCliente_NombreR.Text;
                cl.Rnc = txtRnc.Text;
                cl.Correo = txtCliente_CorreoR.Text;
                cl.Direccion = txtDireccion.Text;
                cl.Telefono = txtTelefono.Text;




                if (!string.IsNullOrEmpty(cl.Nombre))//si el campo nombre esta vacio no permite ingresar datos

                {




                    msj = cl.RegistrarClientes();

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

            CargarClinete();
            Limpiar();


        }




        private void btnGuardarClienteR_Click(object sender, EventArgs e)
        {
            if (existent == false)

            {
                GuardarCliente();

            }
            else
            {
                EditarCliente();

                existent = false;
            }





        }

        private void dtaClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {






        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnborrar_Click(object sender, EventArgs e)
        {



            if (!string.IsNullOrEmpty(cl.Id))//verifica si la fila seleccionada es nuka
            {
                string mensaje = cl.borrarCliente(cl.Id);
                MessageBox.Show(mensaje);
                CargarClinete(); // Recargar los datos para reflejar los cambios
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un registro para eliminar.");
            }



        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            

            

        }

        //Con el evento Cell mouse click se selecciona toda la fila para realizar el evento
        private void dtaClientes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            if (e.RowIndex >= 0)//con este tomamos el indice de la fila(rowIndex) previamente seleccionada, la fila empiezan en cero por tanto si es menor que cero no se ejecutara la accion
            {

                DataGridViewRow row = this.dtaClientes.Rows[e.RowIndex];







                cl.Id = dtaClientes.CurrentRow.Cells[0].Value.ToString();//aqui convertimos el valor row en un dato real de tipo string


            }


        }

        public void EditarCliente()
        {
            // Validar que los campos de teléfono y RNC contengan solo números si no están vacíos
            if (!string.IsNullOrEmpty(txtTelefono.Text) && !Regex.IsMatch(txtTelefono.Text, @"^\d+$"))
            {
                MessageBox.Show("El campo Teléfono solo debe contener números.");
                return;
            }

            if (!string.IsNullOrEmpty(txtRnc.Text) && !Regex.IsMatch(txtRnc.Text, @"^\d+$"))
            {
                MessageBox.Show("El campo RNC solo debe contener números.");
                return;
            }


            try
            {

                if (!string.IsNullOrEmpty(cl.Id))
                {



                    // Actualizar los valores del cliente con los datos de los campos de texto
                    cl.Nombre = txtCliente_NombreR.Text;
                    cl.Rnc = txtRnc.Text;
                    cl.Correo = txtCliente_CorreoR.Text;
                    cl.Direccion = txtDireccion.Text;
                    cl.Telefono = txtTelefono.Text;

                    //condicionante para no ingresar nombres null


                    string mensaje = cl.EditarCliente(cl.Id);

                    MessageBox.Show(mensaje);

                    CargarClinete(); // Recargar los datos para reflejar los cambios
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
        //Rellena los campos nuevamente XD
        public void RellenarCamposCliente()
        {

            txtCliente_NombreR.Text = dtaClientes.CurrentRow.Cells["Nombre"].Value.ToString();
            txtRnc.Text = dtaClientes.CurrentRow.Cells["Rnc"].Value.ToString();
            txtCliente_CorreoR.Text = dtaClientes.CurrentRow.Cells["Correo"].Value.ToString();
            txtDireccion.Text = dtaClientes.CurrentRow.Cells["Direccion"].Value.ToString();
            txtTelefono.Text = dtaClientes.CurrentRow.Cells["Telefono"].Value.ToString();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            RellenarCamposCliente();

            existent = true;
        }

        private void bunifuVScrollBar2_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            // Obtener la posición actual del scrollbar
            int scrollValue = e.Value;

            // Calcular la posición de la primera fila visible en el BunifuCustomDataGrid
            int primeraFilaVisible = scrollValue;

            // Verificar si la primera fila visible es menor que cero
            if (primeraFilaVisible < 0)
            {
                primeraFilaVisible = 0;
            }

            // Verificar si la primera fila visible excede el rango de filas visibles
            if (primeraFilaVisible >= dtaClientes.RowCount)
            {
                primeraFilaVisible = dtaClientes.RowCount - 1;
            }

            // Actualizar la vista del BunifuCustomDataGrid para mostrar las filas visibles
            dtaClientes.FirstDisplayedScrollingRowIndex = primeraFilaVisible;
            dtaClientes.Refresh();

        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {

            exportaraexcel(dtaClientes);

        }


        public void exportaraexcel(DataGridView tabla)

        {
            // Crear un nuevo libro de Excel
            using (var workbook = new XLWorkbook())
            {
                // Crear una nueva hoja de trabajo
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Agregar encabezados
                for (int i = 1; i <= tabla.Columns.Count; i++)
                {
                    worksheet.Cell(1, i).Value = tabla.Columns[i - 1].HeaderText;
                }

                // Agregar datos
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    for (int j = 0; j < tabla.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = tabla.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Mostrar el diálogo de guardado
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    Title = "Save an Excel File"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Guardar el archivo
                    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        workbook.SaveAs(stream);
                    }
                    MessageBox.Show("Export successful");
                }
            }

        }

        //este metodo es para validar que el numero solo tenga 10 digitos
        public static bool EsNumeroDeTelefonoValido(string telefono)
        {
            // Verificar que el teléfono contenga solo dígitos y tenga entre 1 y 10 dígitos
            if (Regex.IsMatch(telefono, @"^\d{10}$"))
            {
                return true;
            }
            return false;
        }

        private void dtaClientes_SelectionChanged(object sender, EventArgs e)
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

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        
    }

}


