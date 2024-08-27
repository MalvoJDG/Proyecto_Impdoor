﻿using Capa_A;
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
            PagPanel.Visible = false;
            NcfFacPanel.Visible = false;
            dtaNcfFac.AllowUserToAddRows = false;
            lblTotalPagado.Visible = false;
            lblTotalPendiente.Visible = false;
            dtaPCliente.CellFormatting += dtaPCliente_CellFormatting;
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
                string nombreCliente = txtBuscadorFacturas.Text.Trim();

                if (!string.IsNullOrEmpty(nombreCliente))
                {

                    DataTable dt = facturaDetalle.MostrarFacturasPorCliente(nombreCliente);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dtaNcfFac.DataSource = dt;
                        NcfFacPanel.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron facturas para el cliente ingresado.");
                        dtaNcfFac.DataSource = null;
                        NcfFacPanel.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un nombre de cliente válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar las facturas del cliente: {ex.Message}");
            }
        }


        private void MostrarPagosCliente()
        {


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
                    // Obtener el valor de la columna Credito_Fiscal
                    var creditoFiscal = dtaFactura.CurrentRow.Cells["NCF"].Value;

                    // Verificar si la columna Credito_Fiscal ya tiene un valor
                    if (creditoFiscal != null && !string.IsNullOrWhiteSpace(creditoFiscal.ToString()))
                    {
                        MessageBox.Show("ya tiene un valor asignado. No se puede agregar un nuevo NCF.");
                        return; // Salir del método si la columna ya tiene un valor
                    }

                    // Obtener el ID de la factura
                    string idFactura = dtaFactura.CurrentRow.Cells[0].Value.ToString();

                    // Llamar al método AsignarNCF
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
            PagPanel.Visible = false;
            NcfFacPanel.Visible = true;

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
            PagPanel.Visible = false;
            cargarHeader();
        }

        private void btnVerPagos_Click(object sender, EventArgs e)
        {
            NcfFacPanel.Visible = false;
            PagPanel.Location = new System.Drawing.Point(21, 34);
            PagPanel.Visible = true;
            PagPanel.BringToFront();
        }

        private void dtaNcfFac_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtBuscadorFacturas_TextChanged(object sender, EventArgs e)
        {

        }




        private void BuscarFacturas()
        {
            try
            {
                string nombreCliente = txtBuscadorFacturas.Text.Trim();

                if (!string.IsNullOrEmpty(nombreCliente))
                {

                    DataTable dt = facturaDetalle.MostrarFacturasPorCliente(nombreCliente);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dtaNcfFac.DataSource = dt;
                        NcfFacPanel.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron facturas para el cliente ingresado.");
                        dtaNcfFac.DataSource = null;
                        NcfFacPanel.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un nombre de cliente válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar las facturas del cliente: {ex.Message}");
            }
        }
        private void BuscarPagos()
        {
            try
            {
                // Obtener el nombre del cliente de la fila seleccionada
                string nombreCliente = txtBuscadorPagos.Text.Trim();

                if (!string.IsNullOrEmpty(nombreCliente))
                {
                    // Cargar las facturas del cliente seleccionado
                    DataTable dt = facturaDetalle.MostrarPagosPorCliente(nombreCliente);

                    // Verificar si el DataTable contiene filas
                    if (dt.Rows.Count == 0)
                    {
                        // No se encontraron datos para el nombre del cliente
                        MessageBox.Show("El nombre del cliente no se encuentra en los registros.");
                        // Limpiar el DataGridView y ocultar los totales
                        dtaPCliente.DataSource = null;
                        PagPanel.Visible = false;
                        lblTotalPagado.Visible = false;
                        lblTotalPendiente.Visible = false;
                        return; // Salir del método
                    }

                    // Asignar el DataTable al DataGridView
                    dtaPCliente.DataSource = dt;

                    // Desactivar la fila de nueva entrada
                    dtaPCliente.AllowUserToAddRows = false;

                    // Calcular los totales desde el DataGridView
                    float totalPagado = 0;
                    float totalPendiente = 0;

                    foreach (DataGridViewRow row in dtaPCliente.Rows)
                    {
                        if (row.IsNewRow) continue;

                        float total = 0;
                        float pagado = 0;

                        // Obtener valores de las columnas, manejando posibles errores de conversión
                        if (float.TryParse(row.Cells["Total"].Value?.ToString(), out total) &&
                            float.TryParse(row.Cells["Pagado"].Value?.ToString(), out pagado))
                        {
                            totalPagado += pagado;
                            float pendiente = total - pagado;
                            totalPendiente += (pendiente > 0) ? pendiente : 0;
                        }
                    }

                    // Mostrar el panel PPanel
                    PagPanel.Visible = true;
                    NcfFacPanel.Visible = false; // Asegúrate de ocultar el otro panel

                    // Mostrar los totales en las etiquetas
                    lblTotalPagado.Text = $"Total Pagado: {totalPagado:C}";
                    lblTotalPendiente.Text = $"Total Pendiente: {totalPendiente:C}";
                    lblTotalPendiente.Visible = true;
                    lblTotalPagado.Visible = true;
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa un nombre de cliente válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar las facturas del cliente: {ex.Message}");
            }


        }
        private void txtBuscadorFacturas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Evita que el sonido "ding" se reproduzca al presionar Enter
                e.SuppressKeyPress = true;

                // Llama al método de búsqueda
                BuscarFacturas();
            }
        }

        private void lblClienteP_Click(object sender, EventArgs e)
        {

        }

        private void txtBuscadorPagos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Evita que el sonido "ding" se reproduzca al presionar Enter
                e.SuppressKeyPress = true;

                // Llama al método de búsqueda
                BuscarPagos();

                // Autocompletar el TextBox con el texto encontrado
                AutocompletarBuscador();
            }

        }

        private void bunifuLabel4_Click_2(object sender, EventArgs e)
        {
        }

        private void PPanel_Click(object sender, EventArgs e)
        {

        }
        private void AutocompletarBuscador()
        {
            // Verifica si hay filas en el DataGridView
            if (dtaPCliente.Rows.Count > 0)
            {
                // Toma el primer valor de la primera fila
                string primerValor = dtaPCliente.Rows[0].Cells[0].Value?.ToString();

                // Establece el valor en el TextBox
                if (!string.IsNullOrEmpty(primerValor))
                {
                    txtBuscadorPagos.Text = primerValor;
                }
            }
        }

        private void dtaPCliente_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verifica si la columna es de interés
            if (dtaPCliente.Columns[e.ColumnIndex].Name == "Total" ||
                dtaPCliente.Columns[e.ColumnIndex].Name == "Pagado" ||
                dtaPCliente.Columns[e.ColumnIndex].Name == "Pendiente")
            {
                if (e.Value != null)
                {
                    // Intentar convertir el valor a un número float
                    if (float.TryParse(e.Value.ToString(), out float number))
                    {
                        // Para la columna "Pendiente", eliminar el signo negativo
                        if (dtaPCliente.Columns[e.ColumnIndex].Name == "Pendiente")
                        {
                            e.Value = Math.Abs(number).ToString("N2");
                        }
                        else
                        {
                            e.Value = number.ToString("N2");
                        }
                        e.FormattingApplied = true;
                    }
                }
            }
        }
    }
}

