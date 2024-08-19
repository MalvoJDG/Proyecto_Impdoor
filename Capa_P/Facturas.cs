using Capa_A;
using Capa_N.Entity;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Capa_P
{
    public partial class Facturas : Form
    {
        public FacturaHeader facturaH = new FacturaHeader();
        FacturaDetalle facturaDetalle = new FacturaDetalle();
        ncf ncf = new ncf();

        public Facturas()
        {
            InitializeComponent();
            cargarHeader();
            PPanel.Visible = false;
            NcfFacPanel.Visible = false;
            dtaNcfFac.AllowUserToAddRows = false;
        }

        private void cargarHeader()
        {
            // Llamar la clase y el método
            DataTable dt = facturaH.ListadoFacturaHeader();
            // Ponerlo como datasource
            dtaFactura.DataSource = dt;
            dtaFactura.ClearSelection();
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (txtFiltro.Text != "")
            {
                dtaFactura.CurrentCell = null;
                foreach (DataGridViewRow row in dtaFactura.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Visible = false;
                    }
                }

                foreach (DataGridViewRow row in dtaFactura.Rows)
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
                cargarHeader();
            }
        }

        private void bunifuVScrollBar2_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            // Obtener la posición actual del scrollbar
            int scrollValue = e.Value;

            // Calcular la posición de la primera fila visible en el DataGridView
            int primeraFilaVisible = scrollValue;

            // Verificar si la primera fila visible es menor que cero
            if (primeraFilaVisible < 0)
            {
                primeraFilaVisible = 0;
            }

            // Verificar si la primera fila visible excede el rango de filas visibles
            if (primeraFilaVisible >= dtaFactura.RowCount)
            {
                primeraFilaVisible = dtaFactura.RowCount - 1;
            }

            // Actualizar la vista del DataGridView para mostrar las filas visibles
            dtaFactura.FirstDisplayedScrollingRowIndex = primeraFilaVisible;
            dtaFactura.Refresh();
        }

        private void CargarDetalle()
        {
            try
            {
                string factura = dtaFactura.CurrentRow.Cells[0].Value.ToString();
                MessageBox.Show($"Factura seleccionada: {factura}");
                facturaDetalle.Factura = factura;

                DataTable dt = facturaDetalle.BuscarPorFactura();
                dtaFactura.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se ha podido cargar el detalle de esta factura/cotización: {ex}");
            }
        }

        public void Borrar()
        {
            try
            {
                string factura = dtaFactura.CurrentRow.Cells[0].Value.ToString();
                facturaH.Factura = factura;
                string mensaje = facturaH.EliminarFacturas();
                MessageBox.Show(mensaje);
            }
            catch
            {
                MessageBox.Show("Por favor, selecciona un registro para eliminar.");
            }
        }

        private void dtaFactura_DoubleClick(object sender, EventArgs e)
        {
            CargarDetalle();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            cargarHeader();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Borrar();
            cargarHeader();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            MontoPanel.Visible = true;
            PorcentajePagado();
        }

        private void ActualizarPagado()
        {
            String msj = "";

            float total = 0;

            try
            {
                float Fullpagado;
                float guardado;
                float monto = float.Parse(Montotxt.Text);

                bool isFullpagadoValid = float.TryParse(dtaFactura.CurrentRow.Cells[4].Value.ToString(), out Fullpagado);
                bool isGuardadoValid = float.TryParse(dtaFactura.CurrentRow.Cells[8].Value.ToString(), out guardado);

                if (!isFullpagadoValid)
                {
                    MessageBox.Show("El valor de Fullpagado no es válido.");
                    return;
                }

                if (isGuardadoValid && guardado > 0)
                {
                    total = guardado + monto;
                }
                else
                {
                    total = monto;
                }

                facturaH.Factura = dtaFactura.CurrentRow.Cells[0].Value.ToString();
                facturaH.Pagado = monto;

                if (total >= Fullpagado)
                {
                    facturaH.Estado_Pago = "Pagada";
                }
                else
                {
                    facturaH.Estado_Pago = "Pendiente";
                }

                msj = facturaH.ActualizarPago();
                MessageBox.Show(msj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            ActualizarPagado();
            MontoPanel.Visible = false;
            cargarHeader();
            Montotxt.Clear();
        }

        private void PorcentajePagado()
        {
            lblPagado.Visible = false;

            if (dtaFactura.CurrentRow.Cells[4].Value != null && dtaFactura.CurrentRow.Cells[8].Value != null)
            {
                string fullPagadoString = dtaFactura.CurrentRow.Cells[4].Value.ToString();
                string montoString = dtaFactura.CurrentRow.Cells[8].Value.ToString();

                if (!string.IsNullOrWhiteSpace(fullPagadoString) && !string.IsNullOrWhiteSpace(montoString))
                {
                    float fullPagado;
                    float monto;

                    if (float.TryParse(fullPagadoString, out fullPagado) && float.TryParse(montoString, out monto))
                    {
                        float porcentajePagado = (monto / fullPagado) * 100;
                        lblPagado.Text = $"Se ha pagado un: {porcentajePagado.ToString("F2")}%";
                        lblPagado.Visible = true;
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MontoPanel.Visible = false;
            Montotxt.Clear();
        }
        private void MostrarFacturasCliente()
        {
            try
            {
                // Verificar que haya una fila seleccionada
                if (dtaFactura.CurrentRow != null)
                {
                    // Obtener el ID del cliente o factura de la fila seleccionada
                    string nombreCliente = dtaFactura.CurrentRow.Cells[6].Value.ToString();

                    lblCliente.Text = $"Cliente: {nombreCliente}";

                    // Cargar las facturas del cliente seleccionado usando la clase ncf
                    DataTable dt = facturaDetalle.MostrarFacturasPorCliente(nombreCliente);

                    // Asignar el DataTable al DataGridView del panel ncfFactura
                    dtaNcfFac.DataSource = dt;

                    // Mostrar el panel ncfFactura
                    NcfFacPanel.Visible = true;
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un cliente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar las facturas del cliente: {ex.Message}");
            }
        }

        private void MostrarPagosCliente()
        {

            try
            {
                // Verificar que haya una fila seleccionada
                if (dtaFactura.CurrentRow != null)
                {
                    // Obtener el nombre del cliente de la fila seleccionada
                    string nombreCliente = dtaFactura.CurrentRow.Cells[6].Value.ToString();

                    // Mostrar el nombre del cliente en el lblCliente
                    lblCliente.Text = $"Cliente: {nombreCliente}";

                    // Cargar las facturas del cliente seleccionado
                    DataTable dt = facturaDetalle.MostrarPagosPorCliente(nombreCliente);

                    // Asignar el DataTable al DataGridView
                    dtaPCliente.DataSource = dt;

                    // Calcular los totales desde el DataGridView
                    float totalPagado = 0;
                    float totalPendiente = 0;

                    foreach (DataGridViewRow row in dtaPCliente.Rows)
                    {
                        if (row.IsNewRow) continue;

                        float total = 0;
                        float pagado = 0;

                        // Obtener valores de las columnas, manejando posibles errores de conversión
                        if (float.TryParse(row.Cells["Total"].Value?.ToString(), out total))
                        {
                            // Obtener el valor de la columna "Pagado"
                            if (float.TryParse(row.Cells["Pagado"].Value?.ToString(), out pagado))
                            {
                                totalPagado += pagado;
                                // Ajustar el total pendiente para que no sea negativo
                                float pendiente = total - pagado;
                                totalPendiente += (pendiente > 0) ? pendiente : 0;
                            }
                        }
                    }

                    // Mostrar el panel PPanel
                    PPanel.Visible = true;
                    NcfFacPanel.Visible = false; // Asegúrate de ocultar el otro panel

                    // Mostrar los totales en las etiquetas
                    lblTotalPagado.Text = $"Total Pagado: {totalPagado:C}";
                    lblTotalPendiente.Text = $"Total Pendiente: {totalPendiente:C}";
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un cliente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar las facturas del cliente: {ex.Message}");
            }
        }
        private void dtaFactura_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void AsignarFacturas_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtaFactura.CurrentRow != null)
                {
                    string idFactura = dtaFactura.CurrentRow.Cells[0].Value.ToString();
                    string mensaje = ncf.AsignarNCF(idFactura);
                    MessageBox.Show(mensaje);
                    cargarHeader();
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona una factura.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar NCF: {ex.Message}");
            }
        }

        private void MontoPanel_Click(object sender, EventArgs e)
        {
           
        }

        private void bunifuLabel4_Click(object sender, EventArgs e)
        {
           
        }

        private void dtaNcfFac_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            NcfFacPanel.Visible = false;
            cargarHeader();
        }

        private void lblCliente_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            try
            {
                    MostrarFacturasCliente();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar el nombre del cliente: {ex.Message}");
            }

        }
        //esto se repite en clientes y ncf debo cambiarlo
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
        private void ExportarExcel_Click(object sender, EventArgs e)
        {
            exportaraexcel(dtaNcfFac);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuButton22_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuLabel4_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            PPanel.Visible = false;
            cargarHeader();
        }

        private void btnVerPagos_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarPagosCliente();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar el nombre del cliente: {ex.Message}");
            }
        }
    }
}

