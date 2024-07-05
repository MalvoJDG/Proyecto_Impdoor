using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Capa_P
{
    public partial class NCF : Form
    {
        public ncf ncf = new ncf();

        bool existent = false;
        public NCF()
        {
            InitializeComponent();
            CargarNCF();
            txtFiltroNCF.TextChanged += new EventHandler(bunifuTextBox1_TextChanged);
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

        private void GuardarSeparado()
        {
            // Obtener los números de crédito fiscal ingresados por el usuario
            string[] numerosCreditoFiscal = txtNumerosCreditoFiscal.Text.Split(new char[] { '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Guardar los números de crédito fiscal en la base de datos
            GuardarNCF(numerosCreditoFiscal);

            // Limpiar el cuadro de texto después de guardar los números
            txtNumerosCreditoFiscal.Clear();

            MessageBox.Show("Números de crédito fiscal guardados exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GuardarNCF(string[] numerosCreditoFiscal)
        {
            existent = false;
            String msj = "";

            try
            {
                foreach (string numero in numerosCreditoFiscal)
                {
                    ncf.Codigo = numero;

                    if (!string.IsNullOrEmpty(ncf.Codigo))//si el campo ncf esta vacio no permite ingresar datos
                    {

                        msj = ncf.RegistrarNCF();

                    }
                    else
                    {
                        MessageBox.Show("Termine de completar los campos Requeridos");
                    }
                }
                MessageBox.Show(msj);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            CargarNCF();
            Limpiar();
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
                    MessageBox.Show("Exportado con exito");
                }
            }
        }

        private void btnGuardarClienteR_Click(object sender, EventArgs e)
        {
            if (existent == false)
            {
                GuardarSeparado();
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

        //Lo que esta dentro de este bloque de codigo funciona con todos los Scrollbar
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
            if (primeraFilaVisible >= dtaFiscal.RowCount)
            {
                primeraFilaVisible = dtaFiscal.RowCount - 1;
            }

            // Actualizar la vista del BunifuCustomDataGrid para mostrar las filas visibles
            dtaFiscal.FirstDisplayedScrollingRowIndex = primeraFilaVisible;
            dtaFiscal.Refresh();
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFiltroNCF.Text;

            (dtaFiscal.DataSource as DataTable).DefaultView.RowFilter = string.Format("Codigo LIKE '%{0}%'", filterText);
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            exportaraexcel(dtaFiscal);  
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            
        }
    }

}
