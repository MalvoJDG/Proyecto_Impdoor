using Capa_N.Entity;
using Capa_N.EntityProv;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
                btnGuardarClienteR.Visible = false;
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
                btnGuardarClienteR.Visible = true ;
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

                // Leer palabras clave desde un recurso de texto
                List<string> materiales = new List<string>();

                string materialContent = Properties.Resources.Materiales; // Accede al recurso de texto
                if (!string.IsNullOrEmpty(materialContent))
                {
                    using (StringReader sr = new StringReader(materialContent))
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
                    MessageBox.Show("El archivo de materiales no se encuentra en los recursos.");
                    return;
                }

            // Resto del código para generar el PDF
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

                plantilla_html = plantilla_html.Replace("@Nombre", txtCliente_NombreR.Text);
                plantilla_html = plantilla_html.Replace("@Contacto", txtTelefono.Text);
                plantilla_html = plantilla_html.Replace("@Direccion", txtDireccion.Text);
                plantilla_html = plantilla_html.Replace("@No.Coti", dtaFacturas.CurrentRow.Cells[0].Value.ToString());
                plantilla_html = plantilla_html.Replace("@FechaEntrada", txtDireccion.Text);
                plantilla_html = plantilla_html.Replace("@FechaSalida", dtaFacturas.CurrentRow.Cells[0].Value.ToString());

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
                                signoNode.InnerHtml = "<span style='font-weight: bold; font-size: 12px; display: block; text-align: center;'>X</span>";

                                HtmlNode cantidadNode = row.SelectSingleNode("td[3]");
                                if (cantidadNode != null)
                                {
                                    cantidadNode.InnerHtml = materialesSeleccionados[material].ToString();
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

                // Reemplazar el marcador de posición con las imágenes
                string Imagenes = string.Empty;
                if (listBox1.Items.Count > 0)
                {
                    
              
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        string imagen = listBox1.Items[i].ToString();
                        Imagenes += $"<tr><td><table style='margin-top: 50pt;'><tr><td><div style='page-break-after: always; text-align: center; margin-top: 20pt;'><img src='{imagen}' style='vertical-align: middle; width: 400px height: 500px;' /></div></td></tr></table></td></tr>";
                    }
                }
                else
                {
                    Imagenes = $"<tr><td></td></tr>";
                }
                plantilla_html = plantilla_html.Replace("@ImagenesExtra", Imagenes);


                // Guardar la imagen de la firma desde los recursos en una ruta temporal
                string tempImagePath = Path.Combine(Path.GetTempPath(), "firma.png");
                Properties.Resources.firma.Save(tempImagePath);

                // Crear el HTML para la imagen
                string imagenHtml = $"<img src='file:///{tempImagePath.Replace('\\', '/')}' alt='Imagen de la factura' style='max-width:100%; height:auto; display:block; margin:auto;' />";
                plantilla_html = plantilla_html.Replace("@img", imagenHtml);

                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 15, 20);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                        pdfDoc.Open();

                        CustomHeader header = new CustomHeader(); // Crear instancia del header personalizado
                        writer.PageEvent = header; // Asignar el evento del header personalizado

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

                var logoHeader = Properties.Resources.impdoor_logo; // Accede al recurso de imagen
                if (logoHeader == null)
                {
                    throw new FileNotFoundException("No se encontró la imagen en los recursos del proyecto.");
                }


                // Convertir la imagen a un MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    logoHeader.Save(ms, ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);

                    // Convertir el MemoryStream a una instancia de iTextSharp.text.Image
                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(ms.ToArray());

                    // Ajustar la posición del header según el número de página
                    if (writer.PageNumber == 1)
                    {
                        pdfImage.ScaleToFit(200f, 55f);
                        PdfPCell imageCellHeader = new PdfPCell(pdfImage);
                        imageCellHeader.Border = 0;
                        imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                        imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        header.AddCell(imageCellHeader);
                        float yPosition = document.PageSize.Height - document.TopMargin - 5; // Ajuste según sea necesario
                        header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                    }
                    else
                    {
                        pdfImage.ScaleToFit(125f, 35f);
                        PdfPCell imageCellHeader = new PdfPCell(pdfImage);
                        imageCellHeader.Border = 0;
                        imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                        imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        header.AddCell(imageCellHeader);
                        float yPosition = document.PageSize.Height - document.TopMargin + 45; // Ajuste según sea necesario
                        header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                    }
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
            if (dtaFacturas.RowCount == 0)
            {
                // Si no hay filas, salir del método sin hacer nada
                return;
            }
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

        private void AbrirDialogo()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";


                // Añadir cada nombre de archivo al ListBox
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            AbrirDialogo();
        }

        private void txtFiltroNCF_TextChanged(object sender, EventArgs e)
        {
            if (txtFiltroNCF.Text != "")
            {
                dtaFacturas.CurrentCell = null;
                foreach (DataGridViewRow row in dtaFacturas.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Visible = false;
                    }
                }

                foreach (DataGridViewRow row in dtaFacturas.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && cell.Value.ToString().ToUpper().IndexOf(txtFiltroNCF.Text.ToUpper()) >= 0)
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
                CargarDetalle();
            }
        }

        private void txtFechaSalida_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
