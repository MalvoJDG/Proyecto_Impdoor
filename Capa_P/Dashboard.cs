using Capa_A;
using Capa_N.Entity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;


namespace Capa_P
{
    public partial class Dashboard : Form
    {

        private DashboardEntity.DashboardEntity dashboard;
        public Dashboard()
        {
            InitializeComponent();
            var manejadorBD = new clsManejador();
            dashboard = new DashboardEntity.DashboardEntity(manejadorBD);
            StartDate.Value = DateTime.Today.AddDays(-7);
            EndDate.Value = DateTime.Now;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
           // ProcesarFacturas();
        }


        private void LoadData()
        {
            var refreshData = dashboard.LoadTime(StartDate.Value, EndDate.Value);

            if (refreshData)
            {

                lblFacturas.Text = dashboard.Facturas.ToString();
                lblTotalIngreso.Text = "$" + dashboard.IngresosTotales.ToString("N2");
                lblTotalNeto.Text = "$" + dashboard.IngresoActual.ToString("N2");

                lblClientes.Text = dashboard.NumClientes.ToString();
                lblFacturaPendiente.Text = dashboard.Facturas_Pagar.ToString();
                lblDeber.Text = "$" + dashboard.Deber.ToString("N2");

                // Limpiar las series existentes en el gráfico


                // Agregar una nueva serie al gráfico

                // Configurar la serie del gráfico
                chart1.DataSource = dashboard.IngresosList;
                chart1.Series[0].XValueMember = "Fecha";
                chart1.Series[0].YValueMembers = "Total";
                chart1.DataBind();
            }
        }


        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnHoy_Click(object sender, EventArgs e)
        {
            StartDate.Value = DateTime.Today;
            EndDate.Value = DateTime.Now;

            LoadData();
            Customoff();
        }


        private void Customoff()
        {
            StartDate.Visible = false;
            EndDate.Visible = false;
            btnOk.Visible = false;
        }

        private void btn7days_Click(object sender, EventArgs e)
        {
            StartDate.Value = DateTime.Today.AddDays(-7);
            EndDate.Value = DateTime.Now;
            LoadData();
            Customoff();
        }

        private void btn30day_Click(object sender, EventArgs e)
        {
            StartDate.Value = DateTime.Today.AddDays(-30);
            EndDate.Value = DateTime.Now;
            LoadData();
            Customoff();
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            StartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate.Value = DateTime.Now;
            LoadData();
            Customoff();
        }

        private void btnManual_Click_1(object sender, EventArgs e)
        {
            StartDate.Visible = true;
            EndDate.Visible = true;
            btnOk.Visible = true;
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            // Crea un archivo Excel usando EPPlus
            using (var package = new ExcelPackage())
            {
                // Agregar una hoja al archivo
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                // Escribe los datos en las celdas (por ejemplo, desde los Labels)
                worksheet.Cells[1, 1].Value = "Cuentas por cobrar";
                worksheet.Cells[2, 1].Value = lblDeber.Text;

                worksheet.Cells[1, 3].Value = "Total de ingresos";
                worksheet.Cells[2, 3].Value = lblTotalIngreso.Text;

                worksheet.Cells[1, 5].Value = "Total neto";
                worksheet.Cells[2, 5].Value = lblTotalNeto.Text;

                worksheet.Cells[1, 7].Value = "Cotizaciones/Facturas";
                worksheet.Cells[2, 7].Value = lblFacturas.Text;

                worksheet.Cells[1, 10].Value = "Cotizaciones/Facturas pendientes";
                worksheet.Cells[2, 10].Value = lblFacturaPendiente.Text;

                worksheet.Cells[1, 14].Value = "Clientes Actuales";
                worksheet.Cells[2, 14].Value = lblClientes.Text;

                // Guardar el archivo
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Guardar archivo Excel"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                    MessageBox.Show("Archivo Excel exportado con éxito.", "Éxito");
                }
            }
        }

        public void EnviarCorreoNotificacion(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                // Configuración del cliente SMTP
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Puerto para conexión segura (TLS)
                    Credentials = new NetworkCredential("jimpdoornotification@gmail.com", "mkdz fdwm iphq qhmv"),
                    EnableSsl = true // Activar SSL para mayor seguridad
                };

                // Configuración del mensaje de correo
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("jimpdoornotification@gmail.com"),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true // Permitir contenido HTML
                };

                mailMessage.To.Add(destinatario); // Agregar destinatario

                // Enviar el correo
                smtpClient.Send(mailMessage);
                Console.WriteLine("Correo enviado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                throw; // Opcional: manejar el error según el caso
            }
        }


        public void ProcesarFacturas()
        {
            // Instancia de la clase que contiene el método
            var claseConLista = new FacturaHeader();

            // Obtenemos la lista de facturas
            List<FacturaHeader> listaFacturas = claseConLista.ListadoFacturaHeaderComoLista();

            // Ahora puedes trabajar con la lista en esta clase
            foreach (var factura in listaFacturas)
            {
            
                if (!claseConLista.ExisteNotificacionEnBD(factura.Factura))
                {
                    string destinatario = "nenubio09@gmail.com"; // Puedes obtenerlo de la factura o base de datos
                    string asunto = $"Aviso: Fecha de salida de factura ({factura.Factura})";
                    string cuerpo = $@"
                    <h1>Notificación de Factura</h1>
                    <p>Le informamos que su factura <strong>{factura.Factura}</strong> se encuentra a menos de 5 dias de su fecha de salida </p>
                    <p>Impdoor.</p>";

                    EnviarCorreoNotificacion(destinatario, asunto, cuerpo);
                    claseConLista.CrearNotificacion(factura.Factura);
                }

                

            }
        }
    }
}
