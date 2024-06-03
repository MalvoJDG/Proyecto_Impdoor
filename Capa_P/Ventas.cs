using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;


namespace Capa_P
{

    public partial class Ventas : Form
    {
        Productos productos = new Productos();
        Materiales materiales = new Materiales();
        Tipo_Madera tipo_Madera = new Tipo_Madera();
        Apanelados apanelados = new Apanelados();
        Jambas jambas = new Jambas();
        Size size = new Size();

        int id_producto = 0;
        int id_Material = 0;
        int id_Madera = 0;
        int id_Apanelado = 0;
        int id_Size = 0;
        int id_Jambas = 0;

        public Ventas()
        {
            InitializeComponent();

            CargarProductos();
            CargarMateriales();
            CargarMadera();
            cargarApanelado();
            cargarSize();
            cargarJambas();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            cbmImpuesto.Text = "Si";
        }

        public void CargarProductos()
        {
            DataTable dt = productos.ListadoProductos();
            cbmProducto.DataSource = dt;
            cbmProducto.DisplayMember = "Nombre";
        }

        public void CargarMateriales()
        {
            DataTable dt = materiales.ListadoMateriales();
            cbmMaterial.DataSource = dt;
            cbmMaterial.DisplayMember = "Nombre";
        }


        public void CargarMadera()
        {
            DataTable dt = tipo_Madera.ListadoMadera();
            cbmMadera.DataSource = dt;
            cbmMadera.DisplayMember = "Nombre";
        }

        public void cargarApanelado()
        {
            DataTable dt = apanelados.ListadoApanelado();
            cbmApanelado.DataSource = dt;
            cbmApanelado.DisplayMember = "Nombre";
        }

        public void cargarJambas()
        {
            DataTable dt = jambas.ObtenerListadoJambas();
            cbmJambas.DataSource = dt;
            cbmJambas.DisplayMember = "Nombre";
        }

        public void cargarSize()
        {
            DataTable dt = size.ListadoSizes();
            cbmSize.DataSource = dt;
            cbmSize.DisplayMember = "Medida";

        }
        private void cbmProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbmProducto.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmProducto.SelectedValue != null)
            {
                id_producto = Convert.ToInt32(cbmProducto.SelectedValue);
            }
        }

        private void cbmMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {

            cbmMaterial.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmMaterial.SelectedValue != null)
            {
                id_Material = Convert.ToInt32(cbmMaterial.SelectedValue);
            }
        }

        private void cbmMadera_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbmMadera.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmMadera.SelectedValue != null)
            {
                id_Madera = Convert.ToInt32(cbmMadera.SelectedValue);
            }
        }

        private void cbmApanelado_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbmApanelado.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmApanelado.SelectedValue != null)
            {
                id_Apanelado = Convert.ToInt32(cbmApanelado.SelectedValue);

            }
        }

        private void cbmJambas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbmJambas.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmJambas.SelectedValue != null)
            {
                id_Jambas = Convert.ToInt32(cbmJambas.SelectedValue);
            }
        }

        private void cbmSize_SelectedIndexChanged(object sender, EventArgs e)
        {

            cbmSize.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmSize.SelectedValue != null)
            {
                id_Size = Convert.ToInt32(cbmSize.SelectedValue);
            }
        }


        public void Calcular()
        {
            // Crear una instancia de tu clase Precios
            Precios precios = new Precios();

            // Asignar valores a las propiedades de la clase
            precios.Producto_id = id_producto;
            precios.Material_id = id_Material;
            precios.Tipo_Madera = id_Madera;
            precios.Apanelado_id = id_Apanelado;
            precios.Jambas_id = id_Jambas;
            precios.Size_id = id_Size;

            // Ejecutar el procedimiento almacenado y obtener el resultado en un DataTable
            DataTable resultado = precios.ObtenerPrecioProducto();

            try
            {


                // Verificar si hay filas en el resultado
                if (resultado.Rows.Count > 0)
                {
                    // Obtener el precio de la primera fila (asumiendo que solo esperas un resultado)
                    float precio = Convert.ToSingle(resultado.Rows[0]["Precio"]);
                    int cantidad = int.Parse(txtCantidad.Text);
                    precio = precio * cantidad;
                    double impuesto = precio * 0.18;

                    lblitbis.Text = impuesto.ToString("N2", CultureInfo.InvariantCulture);

                    // Asignar el precio al TextBox
                    lblTotalln.Text = precio.ToString("N2", CultureInfo.InvariantCulture);
                }
                else
                {
                    MessageBox.Show("No se encontraron resultados.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo obtener el precio, revise los valores ingresados","error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            Calcular();
        }



        private void Insertar()
        {
            int xRows = dtaVentas.Rows.Add(); // Añade una nueva fila y obtiene el índice de la fila agregada
            double Totalln = double.Parse(lblTotalln.Text);
            double Itbis = double.Parse(lblitbis.Text);
            dtaVentas.Rows[xRows].Cells[0].Value = txtServicio.Text;
            dtaVentas.Rows[xRows].Cells[1].Value = cbmProducto.Text;
            dtaVentas.Rows[xRows].Cells[2].Value = cbmMaterial.Text;
            dtaVentas.Rows[xRows].Cells[3].Value = cbmMadera.Text;
            dtaVentas.Rows[xRows].Cells[4].Value = cbmApanelado.Text;
            dtaVentas.Rows[xRows].Cells[5].Value = cbmJambas.Text;
            dtaVentas.Rows[xRows].Cells[6].Value = cbmSize.Text;
            dtaVentas.Rows[xRows].Cells[7].Value = txtCantidad.Text;

            if (cbmImpuesto.Text == "Si")
            {
                dtaVentas.Rows[xRows].Cells[8].Value = Itbis.ToString("N2");
                dtaVentas.Rows[xRows].Cells[9].Value = Totalln.ToString("N2");
            }
            else
            {
                dtaVentas.Rows[xRows].Cells[8].Value = 0;
                dtaVentas.Rows[xRows].Cells[9].Value = Totalln.ToString("N2");
            }
        }

        private void TotalTot()
        {
            double xTotal = 0;
            double xSubtotal = 0;
            double xImpuesto = 0;


            lblTotal.Text = "";
            lblSubTotal.Text = "";
            lblImpuesto.Text = "";

            foreach (DataGridViewRow row in dtaVentas.Rows)
            {
                if (row != null && row.Cells[8] != null && row.Cells[9].Value != null)
                {
                    // Añade un bloque try-catch para capturar la excepción específica
                    try
                    {
                        if (double.TryParse(Convert.ToString(row.Cells[9].Value), NumberStyles.Any, CultureInfo.InvariantCulture, out double total) &&
                            double.TryParse(Convert.ToString(row.Cells[8].Value), NumberStyles.Any, CultureInfo.InvariantCulture, out double impuesto))
                        {


                            if (cbmImpuesto.Text == "Si")
                            {
                                xImpuesto += impuesto;
                            }

                            xSubtotal += total;
                            xTotal += total + impuesto;


                        }
                        else
                        {
                            MessageBox.Show($"Error al convertir el valor '{row.Cells[7].Value}' de la celda a número.", "Error de conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;  // Salir del método si la conversión falla
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al convertir el valor de la celda a número: {ex.Message}", "Error de conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;  // Salir del método en caso de excepción
                    }
                }
            }

            lblTotal.Text = xTotal.ToString("N2", CultureInfo.InvariantCulture);
            lblSubTotal.Text = xSubtotal.ToString("N2", CultureInfo.InvariantCulture);
            lblImpuesto.Text = xImpuesto.ToString("N2", CultureInfo.InvariantCulture);

        }

        private void btnIns_Click(object sender, EventArgs e)
        {
            Insertar();
            TotalTot();
        }

        private void imprimir()
        {
            SaveFileDialog save = new SaveFileDialog();
            if (swtipe.Checked == true)
            {
                save.FileName = lblFac.Text + ".pdf";
            }
            else
            {
                save.FileName = ".pdf";
            }
            save.DefaultExt = "pdf";
            save.Filter = "Archivos PDF (*.pdf)|*.pdf";

            string plantilla_html = Properties.Resources.plantilla.ToString();

            // Reemplazos de valores en la plantilla HTML
            plantilla_html = plantilla_html.Replace("@Cliente", txtNombre.Text);
            plantilla_html = plantilla_html.Replace("@Rnc", lblRnc.Text);
            plantilla_html = plantilla_html.Replace("@Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
            plantilla_html = plantilla_html.Replace("@SubTotal", "$" + lblSubTotal.Text);
            plantilla_html = plantilla_html.Replace("@Impuestos", "$" + lblImpuesto.Text);
            plantilla_html = plantilla_html.Replace("@Total", "$" + lblTotal.Text);

            // Reemplazos adicionales dependiendo de las opciones seleccionadas
            if (swtipe.Checked == true)
            {
                if (cbmNCF.Text == "Si")
                {
                    plantilla_html = plantilla_html.Replace("@CONT", lblNCF.Text);
                    plantilla_html = plantilla_html.Replace("@Factura", "FACTURA VALIDA PARA CREDITO FISCAL");
                    plantilla_html = plantilla_html.Replace("@NumFac", "FF00001");
                }
                else
                {
                    plantilla_html = plantilla_html.Replace("NCF:", "");
                    plantilla_html = plantilla_html.Replace("@CONT", "");
                    plantilla_html = plantilla_html.Replace("@Factura", "FACTURA");
                    plantilla_html = plantilla_html.Replace("@NumFac", "F00001");
                }

                if (cbmPago.Text == "Pagada")
                {
                    plantilla_html = plantilla_html.Replace("@Estado", "De contado");
                }
                else
                {
                    plantilla_html = plantilla_html.Replace("@Estado", "Credito");
                }
            }
            else
            {
                plantilla_html = plantilla_html.Replace("@Factura", "COTIZACION");
                plantilla_html = plantilla_html.Replace("NCF:", "");
                plantilla_html = plantilla_html.Replace("@CONT", "");
                plantilla_html = plantilla_html.Replace("Nro:", " ");
                plantilla_html = plantilla_html.Replace("@NumFac", " ");
                plantilla_html = plantilla_html.Replace("@Estado", "");
            }

            // Agregar filas de la tabla de ventas
            string filas = string.Empty;
            foreach (DataGridViewRow Row in dtaVentas.Rows)
            {
                filas += "<tr>";
                filas += "<td style='font-size: 11px; width: 53%;'>" + Row.Cells["Descripcion"].Value.ToString() + "</td>";
                filas += "<td align='center' style='font-size: 12px; font-weight: bold; width: 12%;'>" + "1.00 x 2.10" + "</td>";
                filas += "<td align='right' style='font-size: 12px; font-weight: bold; width: 12%;'>" + "$35,500.00" + "</td>";
                filas += "<td align='center' style='width: 8%; font-weight: bold; font-size: 12px;'>" + Row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td align='right' style='width: 15%; font-weight: bold; font-size: 12px;'>" + "$" + Row.Cells["Total_Linea"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            plantilla_html = plantilla_html.Replace("@Lista", filas);

            // Agregar precios incluidos y no incluidos
            string precioIn = string.Empty;
            foreach (string linea in txtPrecioIncluye.Lines)
            {
                precioIn += "<li>" + linea + "</li>";
            }
            plantilla_html = plantilla_html.Replace("@PrecioIn", precioIn);

            string precioNoIN = string.Empty;
            foreach (string linea in txtPrecioNoIncluye.Lines)
            {
                precioNoIN += "<li>" + linea + "</li>";
            }
            plantilla_html = plantilla_html.Replace("@PrecioNOIn", precioNoIN);

            // Ruta de la imagen de la firma
            string imagePath = Path.GetFullPath(@"..\..\img\firma.png");
            string imagenHtml = $"<img src='file:///{imagePath.Replace('\\', '/')}' alt='Imagen de la factura' style='max-width:100%; height:auto; display:block; margin:auto;' />";
            plantilla_html = plantilla_html.Replace("@img", imagenHtml);

            if (save.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 50); // Ajusta el margen inferior
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                    writer.PageEvent = new Footer();

                    pdfDoc.Open();
                    pdfDoc.Add(new Phrase(""));

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Properties.Resources.logo_impdoor, System.Drawing.Imaging.ImageFormat.Png);
                    img.ScaleToFit(100, 80);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;
                    img.SetAbsolutePosition(pdfDoc.LeftMargin + 5, pdfDoc.Top - 60);
                    pdfDoc.Add(img);

                    PdfContentByte cb = writer.DirectContent;
                    ColumnText ct = new ColumnText(cb);
                    ct.SetSimpleColumn(new Rectangle(36, 36, 559, 806));

                    using (StringReader str = new StringReader(plantilla_html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, str);

                        ct.Go();
                        float yPosition = ct.YLine;
                        float footerHeight = 50; // Ajusta esto según el tamaño real del footer
                        float minYPosition = pdfDoc.BottomMargin + footerHeight;

                        if (yPosition < minYPosition)
                        {
                            pdfDoc.NewPage();
                            ct.SetSimpleColumn(new Rectangle(36, 36, 559, 806));
                        }
                    }

                    pdfDoc.Close();
                    stream.Close();
                }
            }
        }

        public class Footer : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footer = new PdfPTable(2);
                footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                footer.DefaultCell.Border = 0;
                footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                footer.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                string imagePath = Path.GetFullPath(@"..\..\img\firma.png");
                Image logo = Image.GetInstance(imagePath);
                logo.ScaleAbsolute(200f, 200f); // Ajusta el tamaño de la imagen si es necesario
                PdfPCell imageCell = new PdfPCell(logo);
                imageCell.Border = 0;
                imageCell.HorizontalAlignment = Element.ALIGN_LEFT;
                footer.AddCell(imageCell);

                PdfPCell textCell = new PdfPCell(new Phrase("Este es el pie de página"));
                textCell.Border = 0;
                textCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                footer.AddCell(textCell);

                float xPosition = document.LeftMargin;
                float yPosition = document.BottomMargin - 15; // Ajusta la posición vertical si es necesario
                footer.WriteSelectedRows(0, -1, xPosition, yPosition + 50, writer.DirectContent);
            }
        }


        private void btnImprimir_Click(object sender, EventArgs e)
        {
            imprimir();
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            Calcular();
        }

        private void Limpiar()
        {
            cbmMadera.SelectedIndex = -1;
            cbmJambas.SelectedIndex = -1;
            cbmApanelado.SelectedIndex = -1;
            cbmSize.SelectedIndex = -1;
            txtServicio.Clear();
            lblTotalln.Text = "";
            txtCantidad.Clear();
            txtInstalacion.Clear();
        }
        private void btnLimpiarD_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Ventas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }

}
