using iTextSharp.awt.geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace Capa_P
{

    public partial class Ventas : Form
    {
        Productos productos = new Productos();
        Materiales materiales = new Materiales();
        Tipo_Madera tipo_Madera = new Tipo_Madera();

        int id_producto = 0;
        int id_Material = 0;
        int id_Madera = 0;
        int cantidad = 0;

        public Ventas()
        {
            InitializeComponent();

            CargarProductos();
            CargarMateriales();
            CargarMadera();
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
        public void SeleccionApanelado()
        {
            // Verifica si el tipo de puerta es "Semisólida" o "Apanelada"
            if (cbmTipoPuerta.Text == "Semisolida" || cbmTipoPuerta.Text == "Apanelada")
            {
                double incrementoApanelado = 0;

                // Aplica un incremento específico dependiendo del material seleccionado
                switch (cbmApanelado.Text)
                {
                    case "Plywood":
                        incrementoApanelado = 0.1; // Ejemplo: un incremento del 15%
                        break;
                    case "Madera":
                        incrementoApanelado = 0.20; // Ejemplo: un incremento del 20%
                        break;
                    case "Cristal":
                        incrementoApanelado = 0.45; // Ejemplo: un incremento del 25%
                        break;
                    default:
                        break;
                }

                // Calcula el precio total con el incremento del apanelado
                double totalConApanelado = precioAjustado * (1 + incrementoApanelado);

                // Actualiza el precio ajustado con el apanelado
                precioAjustado = totalConApanelado;

                // Actualiza el precio total en el formulario
                lblTotalln.Text = totalConApanelado.ToString("N2");
            }
        }


        private double porcentajeIncrementoTipoPuerta = 0; // Variable para almacenar el porcentaje de incremento del tipo de puerta

        public void SeleccionTipoPuerta()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmTipoPuerta.Text)
                {
                    case "Apanelada":
                        porcentajeIncrementoTipoPuerta = 0.08; // Ejemplo: un incremento del 10%
                        break;

                    case "Semisolida":
                        porcentajeIncrementoTipoPuerta = 0.30; // Ejemplo: un incremento del 20%
                        break;

                    case "Maciza":
                        porcentajeIncrementoTipoPuerta = 0.80; // Ejemplo: un incremento del 30%
                        break;

                    case "Chapada":
                        porcentajeIncrementoTipoPuerta = 0.25; // Ejemplo: un incremento del 15%
                        break;

                    default:
                        porcentajeIncrementoTipoPuerta = 0; // Sin incremento
                        break;
                }

                total = precioBase * (1 + porcentajeIncrementoTipoPuerta); // Calcula el precio total con el incremento
                lblTotalln.Text = total.ToString("N2");
                precioAjustado = total; // Actualiza el precio ajustado
            }
        }

        private bool calculoDoblesRealizado = false; // Variable para controlar si el cálculo para "Dobles" ya se realizó
        private double precioBase = 0; // Variable para almacenar el precio base
        private double precioAjustado = 0; // Variable para almacenar el precio ajustado con las jambas

        public void SeleccionJambas()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmJambas.Text)
                {
                    case "Frontales":
                        // Restablece el precio ajustado y permite nuevos cálculos de "Dobles"
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        calculoDoblesRealizado = false;
                        break;

                    case "Dobles":
                        // Solo ejecuta el cálculo si no se ha realizado anteriormente desde la última selección de "Dobles"
                        if (!calculoDoblesRealizado)
                        {
                            double incrementoJambas = precioBase * 0.20; // Incremento del 20% para jambas
                            total = precioAjustado + incrementoJambas; // Suma el incremento al precio ajustado
                            lblTotalln.Text = total.ToString("N2");
                            calculoDoblesRealizado = true; // Marca que el cálculo se ha realizado
                        }
                        else
                        {
                            lblTotalln.Text = precioAjustado.ToString("N2");
                        }
                        break;

                    default:
                        // Manejo para otras opciones si es necesario
                        break;
                }
            }
        }

        public void SeleccionTerminacion()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmTerminacion.Text)
                {
                    case "Lacado natural":
                        double incrementoLacadoNatural = precioBase * 0.15; // Incremento del 15% para lacado natural
                        total = precioAjustado + incrementoLacadoNatural; // Suma el incremento al precio ajustado
                        lblTotalln.Text = total.ToString("N2");
                        break;

                    case "Color":
                        double incrementoColor = precioBase * 0.20; // Incremento del 20% para color
                        total = precioAjustado + incrementoColor; // Suma el incremento al precio ajustado
                        lblTotalln.Text = total.ToString("N2");
                        break;

                    default:
                        // Manejo para otras opciones si es necesario
                        break;
                }
            }
        }

        public void Calcular()
        {
            // Crear una instancia de tu clase Precios
            Precios precios = new Precios();

            // Asignar valores a las propiedades de la clase
            precios.Tipo_Madera_id = id_Madera;

            if (float.TryParse(txtAncho.Text, out float ancho) && float.TryParse(txtLargo.Text, out float largo))
            {
                // Si se pudo convertir correctamente, asigna el valor a precios.Ancho
                precios.Ancho = ancho;
                precios.Largo = largo;
            }

            // Ejecutar el procedimiento almacenado y obtener el resultado en un DataTable
            DataTable resultado = precios.ObtenerPrecioPuerta();

            try
            {
                // Verificar si hay filas en el resultado
                if (resultado.Rows.Count > 0)
                {
                    // Obtener el precio de la primera fila (asumiendo que solo esperas un resultado)
                    float precio = Convert.ToSingle(resultado.Rows[0]["Precio"]);
                    cantidad = int.Parse(txtCantidad.Text);
                    precio = precio * cantidad;
                    double impuesto = precio * 0.18;

                    lblitbis.Text = impuesto.ToString("N2", CultureInfo.InvariantCulture);

                    // Asignar el precio al TextBox
                    precioBase = precio; // Asignar a la variable de precio base
                    lblTotalln.Text = precioBase.ToString("N2", CultureInfo.InvariantCulture);
                    lblactualtotal.Text = precioBase.ToString("N2", CultureInfo.InvariantCulture);

                    // Resetea el precio ajustado y la bandera de cálculo
                    precioAjustado = precioBase;
                    calculoDoblesRealizado = false;

                    

                    // Aplicar selección de tipo de puerta después de calcular el precio base
                    SeleccionTipoPuerta();

                    SeleccionApanelado();

                    // Aplicar selección de terminación después de ajustar por el tipo de puerta
                    SeleccionTerminacion();

                    // Aplicar selección de jambas después de ajustar por la terminación
                    SeleccionJambas();
                }
                else
                {
                    MessageBox.Show("No se encontraron resultados.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo obtener el precio, revise los valores ingresados", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            Calcular();
        }



        /*   private void Insertar()
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
           }*/

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
            // Insertar();
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

        /*  private void Limpiar()
          {
              cbmMadera.SelectedIndex = -1;
              cbmJambas.SelectedIndex = -1;
              cbmApanelado.SelectedIndex = -1;
              cbmSize.SelectedIndex = -1;
              txtServicio.Clear();
              lblTotalln.Text = "";
              txtCantidad.Clear();
              txtInstalacion.Clear();
          }*/

        private void btnLimpiarD_Click(object sender, EventArgs e)
        {
            //Limpiar();
        }

        private void Ventas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private bool posicionesIntercambiadas = false; // Bandera para controlar si las posiciones ya han sido intercambiadas

        private void cbmTipoPuerta_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Drawing.Point ubicacionCbmJamba = cbmJambas.Location;
            System.Drawing.Point ubicacionCbmApanelado = cbmApanelado.Location;
            System.Drawing.Point ubicacionTituloJamba = lblJambas.Location;
            System.Drawing.Point ubicacionTituloApanelado = lblApanelado.Location;

            if (cbmTipoPuerta.Text == "Semisolida" || cbmTipoPuerta.Text == "Apanelada")
            {
                lblApanelado.Visible = true;
                cbmApanelado.Visible = true;


                // Si las posiciones fueron intercambiadas previamente, restablece las ubicaciones
                if (posicionesIntercambiadas)
                {
                    cbmJambas.Location = ubicacionCbmApanelado;
                    cbmApanelado.Location = ubicacionCbmJamba;
                    lblJambas.Location = ubicacionTituloApanelado;
                    lblApanelado.Location = ubicacionTituloJamba;

                    posicionesIntercambiadas = false; // Restablece la bandera
                }
            }
            else
            {
                lblApanelado.Visible = false;
                cbmApanelado.Visible = false;

                // Si las posiciones no han sido intercambiadas, hazlo
                if (!posicionesIntercambiadas)
                {
                    // Intercambia las ubicaciones
                    cbmJambas.Location = ubicacionCbmApanelado;
                    cbmApanelado.Location = ubicacionCbmJamba;
                    lblJambas.Location = ubicacionTituloApanelado;
                    lblApanelado.Location = ubicacionTituloJamba;

                    posicionesIntercambiadas = true; // Marca que las posiciones han sido intercambiadas
                }
            }
        }

    }

}
