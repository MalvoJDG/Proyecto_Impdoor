using Capa_N.Entity;
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
    }
}
