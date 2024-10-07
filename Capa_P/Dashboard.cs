using Capa_A;
using System;
using System.Windows.Forms;

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

            // Mostrar las fechas en un MessageBox
            MessageBox.Show($"Fecha inicio: {StartDate.Value.ToString()}\nFecha fin: {EndDate.Value.ToString()}", "Fechas seleccionadas");

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
    }
}
