using Bunifu.Framework.UI;
using Bunifu.UI.WinForms;
using Bunifu.Utils;
using Capa_N.EntityProv;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
            // Añadir el evento TextChanged para el filtro
            txtFiltro.TextChanged += new EventHandler(txtFiltro_TextChanged);
            // Añadir el evento Click para label6
            this.label6.Click += new System.EventHandler(this.label6_Click);
        }

        private void CargarClinete()
        {
            // Llamar la clase y el metodo
            DataTable dt = cl.ListadoClientes();
            // Ponerlo como datasource
            dtaClientes.DataSource = dt;
            dtaClientes.ClearSelection();
        }

        private void GuardarCliente()
        {
            existent = false;
            String msj = "";

            try
            {
                cl.Nombre = txtCliente_NombreR.Text;
                cl.Rnc = txtRnc.Text;
                cl.Correo = txtCliente_CorreoR.Text;
                cl.Direccion = txtDireccion.Text;
                cl.Telefono = txtTelefono.Text;

                if (!string.IsNullOrEmpty(cl.Nombre)) //si el campo nombre esta vacio no permite ingresar datos
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

        private void btnborrar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cl.Id)) //verifica si la fila seleccionada es nula
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

        private void dtaClientes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) //con este tomamos el indice de la fila(rowIndex) previamente seleccionada, la fila empiezan en cero por tanto si es menor que cero no se ejecutara la accion
            {
                DataGridViewRow row = this.dtaClientes.Rows[e.RowIndex];
                cl.Id = dtaClientes.CurrentRow.Cells[0].Value.ToString(); //aqui convertimos el valor row en un dato real de tipo string
            }
        }

        public void EditarCliente()
        {
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

                    // Condicionante para no ingresar nombres null
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
                    MessageBox.Show("Exportado con exito");
                }
            }
        }

        // Este metodo es para validar que el numero solo tenga 10 digitos
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
            if (dtaClientes.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dtaClientes.SelectedRows[0];
                cl.Id = row.Cells["Id"].Value.ToString();
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {

            if (txtFiltro.Text != "")
            {
                dtaClientes.CurrentCell = null;
                foreach (DataGridViewRow row in dtaClientes.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Visible = false;
                    }
                }

                foreach (DataGridViewRow row in dtaClientes.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && cell.Value.ToString().ToUpper().IndexOf(txtFiltro.Text.ToUpper()) >= 0)
                            {
                                row.Visible = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                CargarClinete();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Label6 fue clickeado.");
        }

        private void txtTelefono_TextChange(object sender, EventArgs e)
        {
            
                /// Verificar si el texto tiene menos de 10 caracteres para evitar errores al acceder a índices fuera del rango
                if (txtTelefono.TextLength < 10)
                {
                    return;
                }

                // Eliminar guiones existentes para evitar duplicados
                string textoLimpio = txtTelefono.Text.Replace("-", "");

                // Verificar si el texto limpio tiene más de 10 caracteres
                if (textoLimpio.Length > 10)
                {
                    // Si tiene más de 10 caracteres, truncarlo a 10 caracteres
                    textoLimpio = textoLimpio.Substring(0, 10);
                }

                // Construir el número con guiones en las posiciones adecuadas
                StringBuilder numeroConGuiones = new StringBuilder();
                numeroConGuiones.Append(textoLimpio.Substring(0, 3)); // Primeros 3 dígitos
                numeroConGuiones.Append("-");
                numeroConGuiones.Append(textoLimpio.Substring(3, 3)); // Siguientes 3 dígitos
                numeroConGuiones.Append("-");
                numeroConGuiones.Append(textoLimpio.Substring(6)); // Últimos 4 dígitos

                // Mostrar el número con guiones en el TextBox
                txtTelefono.Text = numeroConGuiones.ToString();

                // Colocar el cursor al final del texto para mantener la posición correcta mientras se escribe
                txtTelefono.SelectionStart = txtTelefono.TextLength;
            
        }
    }
}
