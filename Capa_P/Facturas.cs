﻿using Capa_N.Entity;
using DocumentFormat.OpenXml.Vml;
using System;
using System.Data;
using System.Windows.Forms;

namespace Capa_P
{
    public partial class Facturas : Form
    {
        public FacturaHeader facturaH = new FacturaHeader();
        FacturaDetalle facturaDetalle = new FacturaDetalle();
        public Facturas()
        {
            InitializeComponent();
            cargarHeader();
        }

        private void cargarHeader()
        {
            // Llamar la clase y el metodo
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

            // Calcular la posición de la primera fila visible en el BunifuCustomDataGrid
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

            // Actualizar la vista del BunifuCustomDataGrid para mostrar las filas visibles
            dtaFactura.FirstDisplayedScrollingRowIndex = primeraFilaVisible;
            dtaFactura.Refresh();
        }

        private void CargarDetalle()
        {
            try
            {
                string factura = dtaFactura.CurrentRow.Cells[0].Value.ToString();
                MessageBox.Show($"seleccionada: {factura}");
                facturaDetalle.Factura = factura;


                DataTable dt = facturaDetalle.BuscarPorFactura();

                dtaFactura.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se ha podido cargar el detalle de esta factura/cotizacion: {ex} ");
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
                // Convertir los valores de las celdas a float usando TryParse
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
            // Ocultar el Label por defecto
            lblPagado.Visible = false;

            // Verificar si las celdas no están vacías o nulas
            if (dtaFactura.CurrentRow.Cells[4].Value != null && dtaFactura.CurrentRow.Cells[8].Value != null)
            {
                string fullPagadoString = dtaFactura.CurrentRow.Cells[4].Value.ToString();
                string montoString = dtaFactura.CurrentRow.Cells[8].Value.ToString();

                if (!string.IsNullOrWhiteSpace(fullPagadoString) && !string.IsNullOrWhiteSpace(montoString))
                {
                    // Convertir los valores a float
                    float fullPagado;
                    float monto;

                    if (float.TryParse(fullPagadoString, out fullPagado) && float.TryParse(montoString, out monto))
                    {
                        float porcentajePagado = (monto / fullPagado) * 100;
                        lblPagado.Text = $"Se ha pagado un: {porcentajePagado.ToString("F2")}%";
                        lblPagado.Visible = true; // Mostrar el Label si el cálculo es válido
                    }
                }
            }
        }
    }
}
