using Capa_N.Entity;
using Capa_N.EntityProv;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace Capa_P
{
    public partial class Fabricacion : Form
    {
        Cliente cl = new Cliente();
        FacturaHeader facturaHeader = new FacturaHeader();
        FacturaDetalle facturaDetalle = new FacturaDetalle();

        public Fabricacion()
        {
            InitializeComponent();
        }

        private void Fabricacion_Load(object sender, EventArgs e)
        {

        }

        private void CargarHeader()
        {
            try
            {
                facturaHeader.Cliente = txtCliente_NombreR.Text;

                DataTable dt = facturaHeader.BuscarPorCliente();

                dtaFacturas.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Este cliente no posee facturas o cotizaciones");
            }

        }

        private void CargarDetalle()
        {
            try
            {
                string factura = dtaFacturas.CurrentRow.Cells[0].Value.ToString();
                MessageBox.Show($"Factura seleccionada: {factura}"); 
                facturaDetalle.Factura = factura;


                DataTable dt = facturaDetalle.BuscarPorFactura();

                dtaFacturas.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se ha podido cargar el detalle de esta factura/cotizacion: {ex} ");
            }

        }

        private void BuscarCliente()
        {
            try
            {
                if (txtCliente_NombreR.Text != string.Empty)
                {
                    string nombreBusqueda = txtCliente_NombreR.Text.Trim();



                    cl.BuscarCliente(nombreBusqueda);

                    txtCliente_NombreR.Text = cl.Nombre;
                    txtTelefono.Text = cl.Telefono;
                    txtDireccion.Text = cl.Direccion;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void imprimir()
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();

                save.FileName = ".pdf";
                save.DefaultExt = "pdf";
                save.Filter = "Archivos PDF (*.pdf)|*.pdf";

                // Leer palabras clave desde un archivo de texto
                List<string> materiales = new List<string>();
                string path = Path.GetFullPath(@"..\..\Externo\Materiales.txt");

                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            materiales.Add(line.ToUpper());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El archivo de materiales no se encuentra.");
                    return;
                }

                // Crear un diccionario de materiales y cantidades
                Dictionary<string, int> materialesSeleccionados = new Dictionary<string, int>();

                foreach (DataGridViewRow row in dtaFacturas.Rows)
                {
                    string descripcion = row.Cells["Descripcion"].Value.ToString().ToUpper();
                    int cantidad = int.Parse(row.Cells["Cantidad"].Value.ToString());

                    // Buscar materiales en la descripción y actualizar el diccionario de cantidades
                    foreach (var material in materiales)
                    {
                        if (descripcion.Contains(material))
                        {
                            if (!materialesSeleccionados.ContainsKey(material))
                            {
                                materialesSeleccionados[material] = cantidad;
                            }
                            else
                            {
                                materialesSeleccionados[material] += cantidad;
                            }
                        }
                    }
                }

                string plantilla_html = Properties.Resources.Ficha_Fabricacion.ToString();

                // Lógica para reemplazar las cantidades en el HTML
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(plantilla_html);

                foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//table[@class='main-table']//tr"))
                {
                    HtmlNode materialNode = row.SelectSingleNode("td[1]");
                    if (materialNode != null)
                    {
                        string material = materialNode.InnerText.Trim().ToUpper();
                        if (materialesSeleccionados.ContainsKey(material))
                        {
                            HtmlNode signoNode = row.SelectSingleNode("td[2]");
                            if (signoNode != null)
                            {
                                signoNode.InnerHtml = "<span style='font-weight: bold; font-size: 14px; display: block; text-align: center;'>X</span>";


                                HtmlNode cantidadNode = row.SelectSingleNode("td[3]");
                                if (cantidadNode != null)
                                {
                                    // Verificación: Imprimir antes y después
                                    MessageBox.Show($"Material: {material}, Cantidad antes: {cantidadNode.InnerHtml}");
                                    cantidadNode.InnerHtml = materialesSeleccionados[material].ToString();
                                    MessageBox.Show($"Material: {material}, Cantidad después: {cantidadNode.InnerHtml}");
                                }
                            }
                        }
                    }
                }

                plantilla_html = doc.DocumentNode.OuterHtml;

                // Agregar filas de la tabla de facturas
                string filas = string.Empty;
                foreach (DataGridViewRow Row in dtaFacturas.Rows)
                {
                    filas += "<tr>";
                    filas += "<td style='font-size: 10px; width: 48%;'>" + Row.Cells["Descripcion"].Value.ToString() + "</td>";
                    filas += "<td align='center' style='font-size: 11; font-weight: bold; width: 15%;'>" + Row.Cells["Size"].Value.ToString() + "</td>";
                    filas += "<td align='center' style='width: 9%; font-weight: bold; font-size: 11px;'>" + Row.Cells["Cantidad"].Value.ToString() + "</td>";
                    filas += "</tr>";
                }
                plantilla_html = plantilla_html.Replace("@Lista", filas);

                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 15, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                        CustomHeader header = new CustomHeader(); // Crear instancia del header personalizado
                        writer.PageEvent = header; // Asignar el evento del header personalizado

                        pdfDoc.Open();
                        pdfDoc.Add(new Phrase(""));

                        PdfContentByte cb = writer.DirectContent;
                        ColumnText ct = new ColumnText(cb);
                        ct.SetSimpleColumn(new Rectangle(36, 36, 559, 806));

                        using (StringReader str = new StringReader(plantilla_html))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, str);

                            ct.Go();
                            float yPosition = ct.YLine;
                        }

                        pdfDoc.Close();
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir la factura: " + ex.Message);
            }
        }






        public class CustomHeader : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable header = new PdfPTable(1);
                header.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                header.DefaultCell.Border = 0;
                header.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                header.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                string imagePathHeader = Path.GetFullPath(@"..\..\img\impdoor_logo.png");
                if (!File.Exists(imagePathHeader))
                {
                    throw new FileNotFoundException($"No se encontró la imagen en la ruta especificada: {imagePathHeader}");
                }

                Image logoHeader = Image.GetInstance(imagePathHeader);


                // Ajustar la posición del header según el número de página
                if (writer.PageNumber == 1)
                {
                    logoHeader.ScaleToFit(200f, 55f);
                    PdfPCell imageCellHeader = new PdfPCell(logoHeader);
                    imageCellHeader.Border = 0;
                    imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    // Ajuste de posición para la primera página
                    header.AddCell(imageCellHeader);
                    float yPosition = document.PageSize.Height - document.TopMargin + 8; // Ajuste según sea necesario
                    header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                }
                else
                {
                    logoHeader.ScaleToFit(125f, 35);
                    PdfPCell imageCellHeader = new PdfPCell(logoHeader);
                    imageCellHeader.Border = 0;
                    imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    // Ajuste de posición para la primera página
                    header.AddCell(imageCellHeader);
                    float yPosition = document.PageSize.Height - document.TopMargin + 8; // Ajuste según sea necesario
                    header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                }
            }
        }

        private void btnGuardarClienteR_Click(object sender, EventArgs e)
        {
            imprimir();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            BuscarCliente();
            CargarHeader();
        }

        private void dtaFacturas_DoubleClick(object sender, EventArgs e)
        {
            CargarDetalle();
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
            if (primeraFilaVisible >= dtaFacturas.RowCount)
            {
                primeraFilaVisible = dtaFacturas.RowCount - 1;
            }

            // Actualizar la vista del BunifuCustomDataGrid para mostrar las filas visibles
            dtaFacturas.FirstDisplayedScrollingRowIndex = primeraFilaVisible;
            dtaFacturas.Refresh();
        }
    }
}
