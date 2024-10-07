using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Capa_N.Entity
{
    public class FacturaHeader
    {
        public int Id { get; set; }
        public string Factura { get; set; }
        public DateTime Fecha_Emision { get; set; }
        public float SubTotal { get; set; }
        public float ITBIS { get; set; }
        public float Total { get; set; }
        public string Credito_Fiscal { get; set; }
        public string Estado_Pago { get; set; }
        public string Cliente { get; set; }
        public string Rnc { get; set; }
        public float Pagado { get; set; }

        clsManejador m = new clsManejador();

        public String GuardarFacturaHeader()
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_Factura", Factura));
                list.Add(new clsParametros("p_Fecha_Emision", DateTime.Now));
                list.Add(new clsParametros("p_subTotal", SubTotal));
                list.Add(new clsParametros("p_ITBIS", ITBIS));
                list.Add(new clsParametros("p_Total", Total));
                list.Add(new clsParametros("p_Credito_Fiscal", Credito_Fiscal));
                list.Add(new clsParametros("p_Estado_Pago", Estado_Pago));
                list.Add(new clsParametros("p_Cliente", Cliente));
                list.Add(new clsParametros("p_Rnc", Rnc));


                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("InsertarFactura", list);


                msj = list[9].valor.ToString();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return msj;
        }

        public String ActualizarPago()
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_Factura", Factura));
                list.Add(new clsParametros("P_Estado", Estado_Pago));
                list.Add(new clsParametros("p_Pagado", Pagado));

                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("ActualizarPagado", list);


                msj = list[3].valor.ToString();

            }

            catch (Exception ex)
            {
                msj = "Error updating payment: " + ex.Message;
            }

            return msj;
        }

        public DataTable ListadoFacturaHeader()
        {
            return m.consultas("SeleccionarFacturas", null);
        }



        public DataTable BuscarPorCliente()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            parametros.Add(new clsParametros("p_NombreCliente", Cliente));

            return m.consultas("BuscarHeaderFacturasCliente", parametros);
        }

        public String EliminarFacturas()
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("P_factura", Factura));


                list.Add(new clsParametros("p_mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("DeleteFactura", list);


                msj = list[1].valor.ToString();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return msj;
        }

    }
}
