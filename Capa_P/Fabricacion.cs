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
    public partial class Fabricacion : Form
    {
        public Fabricacion()
        {
            InitializeComponent();
        }

        private void Fabricacion_Load(object sender, EventArgs e)
        {

        }

        private void imprimir()
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                
                save.FileName = ".pdf";
              
                save.DefaultExt = "pdf";
                save.Filter = "Archivos PDF (*.pdf)|*.pdf";

                string plantilla_html = Properties.Resources.Ficha_Fabricacion.ToString();
  
                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

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

        private void btnGuardarClienteR_Click(object sender, EventArgs e)
        {
            imprimir();
        }
    }
}
