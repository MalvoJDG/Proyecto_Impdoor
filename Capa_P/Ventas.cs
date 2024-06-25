using Capa_N.EntityProv;
using DocumentFormat.OpenXml.Drawing.Charts;
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
        Cliente cl = new Cliente();

        int id_producto = 0;
        int id_Material = 0;
        int id_Madera = 0;
        int cantidad = 0;
        float preciou = 0;
        int pageNumber = 1;

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
            System.Data.DataTable dt = productos.ListadoProductos();
            cbmProducto.DataSource = dt;
            cbmProducto.DisplayMember = "Nombre";
        }

        public void CargarMateriales()
        {
            System.Data.DataTable dt = materiales.ListadoMateriales();
            cbmMaterial.DataSource = dt;
            cbmMaterial.DisplayMember = "Nombre";
        }

        public void CargarMadera()
        {
            System.Data.DataTable dt = tipo_Madera.ListadoMadera();
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
                        precioAjustado += incrementoLacadoNatural; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    case "Color":
                        double incrementoColor = precioBase * 0.20; // Incremento del 20% para color
                        precioAjustado += incrementoColor; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    default:
                        // Manejo para otras opciones si es necesario
                        break;
                }
            }
        }

        public void Calcular()
        {
            if(cbmProducto.Text != "Instalacion")
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
                System.Data.DataTable resultado = precios.ObtenerPrecioPuerta();

                try
                {
                    // Verificar si hay filas en el resultado
                    if (resultado.Rows.Count > 0)
                    {
                        // Obtener el precio de la primera fila (asumiendo que solo esperas un resultado)
                        float precio = Convert.ToSingle(resultado.Rows[0]["PrecioAjustado"]);
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
                    MessageBox.Show($"Error al obtener el precio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                cantidad = int.Parse(txtCantidad.Text);
                preciou = float.Parse(txtInstalacion.Text);

                double total = cantidad * preciou;
                lblTotalln.Text = total.ToString("N2", CultureInfo.InvariantCulture);
                double impuesto = total * 0.18;

                lblitbis.Text = impuesto.ToString("N2", CultureInfo.InvariantCulture);

            }
            
        }



        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            Calcular();
        }



        private void Insertar()
        {
            try
            {
                int xRows = dtaVentas.Rows.Add();

                double Totalln;
                if (!double.TryParse(lblTotalln.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out Totalln))
                {
                    MessageBox.Show("El campo Totalln no tiene un formato válido.");
                    return;
                }

                double Itbis;
                if (!double.TryParse(lblitbis.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out Itbis))
                {
                    MessageBox.Show("El campo Itbis no tiene un formato válido.");
                    return;
                }

                int cantidad;
                if (!int.TryParse(txtCantidad.Text, out cantidad))
                {
                    MessageBox.Show("El campo Cantidad no tiene un formato válido.");
                    return;
                }

                double Punidad = Totalln / cantidad;

                double pintalacion;
                if (!double.TryParse(txtInstalacion.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out pintalacion))
                {
                    MessageBox.Show("El campo Instalación no tiene un formato válido.");
                    return;
                }

                if (cbmProducto.Text != "Instalacion")
                {
                    dtaVentas.Rows[xRows].Cells[0].Value = txtServicio.Text;
                    dtaVentas.Rows[xRows].Cells[1].Value = cbmProducto.Text;
                    dtaVentas.Rows[xRows].Cells[2].Value = cbmMaterial.Text;
                    dtaVentas.Rows[xRows].Cells[3].Value = cbmMadera.Text;
                    dtaVentas.Rows[xRows].Cells[4].Value = cbmApanelado.Text;
                    dtaVentas.Rows[xRows].Cells[5].Value = cbmJambas.Text;
                    dtaVentas.Rows[xRows].Cells[6].Value = $"{txtAncho.Text} X {txtLargo.Text}";
                    dtaVentas.Rows[xRows].Cells[7].Value = txtCantidad.Text;
                    dtaVentas.Rows[xRows].Cells[8].Value = Punidad.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[9].Value = Itbis.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[10].Value = Totalln.ToString("N2");
                }
                else
                {
                    dtaVentas.Rows[xRows].Cells[0].Value = txtServicio.Text;
                    dtaVentas.Rows[xRows].Cells[1].Value = "";
                    dtaVentas.Rows[xRows].Cells[2].Value = "";
                    dtaVentas.Rows[xRows].Cells[3].Value = "";
                    dtaVentas.Rows[xRows].Cells[4].Value = "";
                    dtaVentas.Rows[xRows].Cells[5].Value = "";
                    dtaVentas.Rows[xRows].Cells[6].Value = "";
                    dtaVentas.Rows[xRows].Cells[7].Value = txtCantidad.Text;
                    dtaVentas.Rows[xRows].Cells[8].Value = pintalacion.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[9].Value = Itbis.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[10].Value = Totalln.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}");
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
                if (row != null && row.Cells[9] != null && row.Cells[10].Value != null)
                {
                    // Añade un bloque try-catch para capturar la excepción específica
                    try
                    {
                        if (double.TryParse(Convert.ToString(row.Cells[10].Value), NumberStyles.Any, CultureInfo.InvariantCulture, out double total) &&
                            double.TryParse(Convert.ToString(row.Cells[9].Value), NumberStyles.Any, CultureInfo.InvariantCulture, out double impuesto))
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
                            MessageBox.Show($"Error al convertir el valor '{row.Cells[9].Value}' de la celda a número.", "Error de conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
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
                double transporte = double.Parse(txtTransporte.Text);

                // Reemplazos de valores en la plantilla HTML
                plantilla_html = plantilla_html.Replace("@Cliente", txtNombre.Text);
                plantilla_html = plantilla_html.Replace("@Rnc", txtRnc.Text);
                plantilla_html = plantilla_html.Replace("@Email", txtCorreo.Text);
                plantilla_html = plantilla_html.Replace("@Direccion", txtDireccion.Text);
                plantilla_html = plantilla_html.Replace("@Contacto", txtTelefono.Text);
                plantilla_html = plantilla_html.Replace("@Proyecto", txtProyecto.Text);
                plantilla_html = plantilla_html.Replace("@ENTRADA", txtEntrada.Text);
                plantilla_html = plantilla_html.Replace("@Salida", txtSalida.Text);
                plantilla_html = plantilla_html.Replace("@Entregada", txtEntrega.Text);
                plantilla_html = plantilla_html.Replace("@TRANSPORTE", "$" + transporte.ToString("N2"));
                plantilla_html = plantilla_html.Replace("@TIEMPOENTREGA", txtTiempoEntrega.Text);
                plantilla_html = plantilla_html.Replace("@vigencia", txtVigencia.Text);

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
                    filas += "<td style='font-size: 12px; width: 53%;'>" + Row.Cells["Descripcion"].Value.ToString() + "</td>";
                    filas += "<td align='center' style='font-size: 13px; font-weight: bold; width: 12%;'>" + Row.Cells["Sizes"].Value.ToString() + "</td>";
                    filas += "<td align='right' style='font-size: 13px; font-weight: bold; width: 12%;'>" + "$" + Row.Cells["PrecioUnidad"].Value.ToString() + "</td>";
                    filas += "<td align='center' style='width: 8%; font-weight: bold; font-size: 13px;'>" + Row.Cells["Canti"].Value.ToString() + "</td>";
                    filas += "<td align='right' style='width: 15%; font-weight: bold; font-size: 13px;'>" + "$" + Row.Cells["Total_Linea"].Value.ToString() + "</td>";
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

                string condicion = string.Empty;
                foreach (string linea in txtCondicion.Lines)
                {
                    condicion += "<li>" + linea + "</li>";
                }
                plantilla_html = plantilla_html.Replace("@Condicion", condicion);

                string Imagenes = string.Empty;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    string imagen = listBox1.Items[i].ToString();
                    Imagenes += $"<img src='{imagen}' style='display: block; margin: 25px 25px 75px 195px; width: 400px height: 500px;' />";

                    if (i < listBox1.Items.Count - 1)
                    {
                        Imagenes += "<div style='page-break-after: always;' ></div>";
                    }
                }

                plantilla_html = plantilla_html.Replace("@imagenesExtra", Imagenes);


                // Ruta de la imagen de la firma
                string imagePath = Path.GetFullPath(@"..\..\img\firma.png");
                string imagenHtml = $"<img src='file:///{imagePath.Replace('\\', '/')}' alt='Imagen de la factura' style='max-width:100%; height:auto; display:block; margin:auto;' />";
                plantilla_html = plantilla_html.Replace("@img", imagenHtml);



                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 75, 195);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);


                        // Ajusta el margen inferior


                        CustomHeader header = new CustomHeader(); // Crear instancia del header personalizado
                        writer.PageEvent = header; // Asignar el evento del header personalizado

                        Footer footer = new Footer(); // Crear instancia del footer
                        writer.PageEvent = footer; // Asignar el evento del footer

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
                            float footerHeight = 200; // Ajusta esto según el tamaño real del footer
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
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir la factura: " + ex.Message);
            }
        }

        public class Footer : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footer = new PdfPTable(3); // Cambia a 3 columnas
                footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                footer.DefaultCell.Border = 0;
                footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                footer.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                footer.WidthPercentage = 100; // Asegura que la tabla ocupe el ancho completo

                // Primera imagen
                string imagePath1 = Path.GetFullPath(@"..\..\img\sello_solo_impdoor.png");
                Image logo1 = Image.GetInstance(imagePath1);
                logo1.ScaleAbsolute(350f, 120f); // Ajusta el tamaño de la imagen si es necesario (ancho x alto)
                PdfPCell imageCell1 = new PdfPCell(logo1);
                imageCell1.Border = 0;
                imageCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                imageCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                imageCell1.PaddingLeft = -100f; // Ajuste horizontal de la primera imagen
                footer.AddCell(imageCell1);

                // Celda vacía en el medio para separar las dos imágenes
                PdfPCell emptyCell = new PdfPCell();
                emptyCell.Border = 0;
                footer.AddCell(emptyCell);

                // Segunda imagen
                string imagePath2 = Path.GetFullPath(@"..\..\img\datosfooter.png"); // Ruta de la segunda imagen
                Image logo2 = Image.GetInstance(imagePath2);
                logo2.ScaleAbsolute(220f, 100f); // Ajusta el tamaño de la segunda imagen si es necesario (ancho x alto)
                PdfPCell imageCell2 = new PdfPCell(logo2);
                imageCell2.Border = 0;
                imageCell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                imageCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                imageCell2.PaddingRight = -10f; // Ajuste horizontal de la segunda imagen
                footer.AddCell(imageCell2);

                float xPosition = document.LeftMargin;

                // Ajuste de la posición vertical de la primera imagen
                float yPosition1 = document.BottomMargin + 40; // Ajuste vertical para la primera imagen
                footer.WriteSelectedRows(0, 1, xPosition, yPosition1, writer.DirectContent);

                // Ajuste de la posición vertical de la segunda imagen
                float yPosition2 = document.BottomMargin + 40; // Ajuste vertical para la segunda imagen
                footer.WriteSelectedRows(2, 3, xPosition, yPosition2, writer.DirectContent);
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
                    logoHeader.ScaleToFit(200f, 50f);
                    PdfPCell imageCellHeader = new PdfPCell(logoHeader);
                    imageCellHeader.Border = 0;
                    imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    // Ajuste de posición para la primera página
                    header.AddCell(imageCellHeader);
                    float yPosition = document.PageSize.Height - document.TopMargin - 5; // Ajuste según sea necesario
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
                    float yPosition = document.PageSize.Height - document.TopMargin + 45; // Ajuste según sea necesario
                    header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                }
            }
        }







        private void btnImprimir_Click(object sender, EventArgs e)
        {
            imprimir();
            // comentario para probar push
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if(cbmProducto.Text != "Instalacion")
            {
                Calcular();
            }
            
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

        private void AbrirDialogo()
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";


                // Añadir cada nombre de archivo al ListBox
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }
        private void BuscarCliente()
        {
            try
            {
                if(txtNombre.Text != string.Empty)
                {
                    string nombreBusqueda = txtNombre.Text.Trim();



                    cl.BuscarCliente(nombreBusqueda);

                    txtNombre.Text = cl.Nombre;
                    txtRnc.Text = cl.Rnc;
                    txtCorreo.Text = cl.Correo;
                    txtTelefono.Text = cl.Telefono;
                    txtDireccion.Text = cl.Direccion;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            AbrirDialogo();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            BuscarCliente();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInstalacion_Leave(object sender, EventArgs e)
        {
            Calcular();
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    // Pa poder hacer merge
}
