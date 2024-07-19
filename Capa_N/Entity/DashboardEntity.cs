using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace DashboardEntity
{
    public struct Ingresos
    {
        public string Fecha { get; set; }
        public float Total { get; set; }
    }

    public class DashboardEntity
    {
        private clsManejador manejadorBD;
        private DateTime StartDate;
        private DateTime EndDate;
        private int NumberDays;

        public int NumClientes { get; private set; }
        public List<KeyValuePair<string, int>> TopMateriales { get; private set; }
        public List<Ingresos> IngresosList { get; private set; }
        public int Facturas { get; private set; }
        public int Facturas_Pagar { get; private set; }
        public float IngresoActual { get; private set; }
        public float Deber { get; private set; }
        public float IngresosTotales { get; private set; }

        public DashboardEntity(clsManejador manejadorBD)
        {
            this.manejadorBD = manejadorBD;
            IngresosList = new List<Ingresos>();
        }

        private void ConsultarNumeroClientes()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            DataTable resultado = manejadorBD.consultas("ObtenerTotalClientes", parametros);

            if (resultado.Rows.Count > 0 && resultado.Rows[0][0] != DBNull.Value)
            {
                NumClientes = Convert.ToInt32(resultado.Rows[0][0]);
            }
            else
            {
                NumClientes = 0;
            }
        }

        private int ContarFacturasPagadas()
        {
            List<clsParametros> parametros = new List<clsParametros>
            {
                new clsParametros("@fromDate", StartDate.Date),
                new clsParametros("@ToDate", EndDate.Date)
            };
            DataTable resultado = manejadorBD.consultas("FacturasPagadas", parametros);

            if (resultado.Rows.Count > 0 && resultado.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToInt32(resultado.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

        private int ContarFacturasPendientes()
        {
            List<clsParametros> parametros = new List<clsParametros>
            {
                new clsParametros("@fromDate", StartDate),
                new clsParametros("@ToDate", EndDate)
            };
            DataTable resultado = manejadorBD.consultas("Facturas_Pagar", parametros);

            if (resultado.Rows.Count > 0 && resultado.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToInt32(resultado.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

        private float ObtenerTotal(string estadoPago)
        {
            try
            {
                List<clsParametros> parametros = new List<clsParametros>
        {
            new clsParametros("@fromDate", StartDate),  // Asegúrate de usar .Date para solo la parte de fecha sin la parte de tiempo
            new clsParametros("@ToDate", EndDate)
        };

                DataTable resultado = manejadorBD.consultas("ObtenerTotalPorPagar", parametros);

                if (resultado.Rows.Count > 0 && resultado.Rows[0]["TotalPorPagar"] != DBNull.Value)
                {
                    return Convert.ToSingle(resultado.Rows[0]["TotalPorPagar"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción aquí, por ejemplo, registra el error o lanza una excepción personalizada
                Console.WriteLine("Error al obtener total por pagar: " + ex.Message);
                throw;
            }
        }


        private void ConsultarIngresos()
        {
            List<clsParametros> parametros = new List<clsParametros>
            {
                new clsParametros("@fromDate", StartDate),
                new clsParametros("@ToDate", EndDate)
            };
            DataTable resultado = manejadorBD.consultas("ObtenerTotalFacturas", parametros);

            IngresosList.Clear();
            IngresosTotales = 0;

            foreach (DataRow row in resultado.Rows)
            {
                DateTime fecha = Convert.ToDateTime(row["Fecha_Emision"]);
                float total = Convert.ToSingle(row["TotalFacturas"]);

                IngresosList.Add(new Ingresos
                {
                    Fecha = NumberDays <= 1 ? fecha.ToString("hh tt") : NumberDays <= 30 ? fecha.ToString("dd MMM") : NumberDays <= 92 ? "Week " + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : NumberDays <= (365 * 2) ? fecha.ToString("MMM yyyy") : fecha.ToString("yyyy"),
                    Total = total
                });

                IngresosTotales += total;
            }
        }

        public bool LoadTime(DateTime StartDate, DateTime EndDate)
        {
            EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day,
                EndDate.Hour, EndDate.Minute, 59);

            if (StartDate != this.StartDate || EndDate != this.EndDate)
            {
                this.StartDate = StartDate;
                this.EndDate = EndDate;
                this.NumberDays = (EndDate - StartDate).Days;

                ConsultarNumeroClientes();
                Facturas = ContarFacturasPagadas();
                Facturas_Pagar = ContarFacturasPendientes();
                Deber = ObtenerTotal("Pendiente");
                ConsultarIngresos();

                Console.WriteLine("Data actualizada: {0} - {1}", StartDate.ToString(), EndDate.ToString());
                return true;
            }
            else
            {
                Console.WriteLine("Data sin actualizar, mismo query: {0} - {1}", StartDate.ToString(), EndDate.ToString());
                return false;
            }
        }
    }
}
