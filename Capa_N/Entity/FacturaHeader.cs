using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        public DateTime Entrada { get; set; } 
        public DateTime Salida { get; set; }  
        public string Proyecto { get; set; } 
        public string Titulo { get; set; }
        public string Entregada { get; set; }
        public string Asesor { get; set; }
        public float Transporte { get; set; }
        public float Descuento { get; set; }

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
                list.Add(new clsParametros("p_Entrada", Entrada));        
                list.Add(new clsParametros("p_Salida", Salida));           
                list.Add(new clsParametros("p_Proyecto", Proyecto));     
                list.Add(new clsParametros("p_Titulo", Titulo));
                list.Add(new clsParametros("p_Entregada", Entregada));
                list.Add(new clsParametros("p_Asesor", Asesor));
                list.Add(new clsParametros("P_transporte", Transporte));
                list.Add(new clsParametros("P_Descuento", Descuento));

                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("InsertarFactura", list);


                msj = list[17].valor.ToString();

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

        public List<FacturaHeader> ListadoFacturaHeaderComoLista()
        {
            // Creamos una lista vacía
            List<FacturaHeader> facturas = new List<FacturaHeader>();

            try
            {
                // Obtenemos el DataTable
                DataTable dt = m.consultas("GetInvoicesDueSoon", null);

                // Iteramos por cada fila del DataTable
                foreach (DataRow row in dt.Rows)
                {
                    // Creamos una nueva instancia de FacturaHeader y llenamos solo las propiedades Id y Factura
                    FacturaHeader factura = new FacturaHeader
                    {
                        Factura = row["Factura"].ToString()
                    };

                    // Añadimos el objeto a la lista
                    facturas.Add(factura);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de facturas: " + ex.Message);
            }

            return facturas; // Devolvemos la lista de facturas con solo Id y Factura
        }


        public DataTable ListadoFacturaHeader()
        {
            return m.consultas("SeleccionarFacturas", null);
        }

        public DataTable GetInvoicesDueSoon()
        {
            return m.consultas("GetInvoicesDueSoon", null);
        }


        public DataTable BuscarPorCliente()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            parametros.Add(new clsParametros("p_NombreCliente", Cliente));

            return m.consultas("BuscarHeaderFacturasCliente", parametros);
        }

        public DataTable ObtenerFacturaConDetalle()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            parametros.Add(new clsParametros("p_Factura", Factura));

            return m.consultas("ObtenerFacturaConDetalle", parametros);
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

        public void CrearNotificacion(string facturaid)
        {
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_FacturaId", facturaid));

                m.EjecutarSp("InsertarNotificacion", list);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertarNotificacion()
        {
            try
            {
                // Llama al procedimiento InsertarNotificacion, que ya maneja la lógica de inserción
                m.EjecutarSp("InsertarNotificacion", null);
            }
            catch (MySqlException ex)
            {
               throw new Exception($"Error en MySQL: {ex.Message}\nCódigo: {ex.Number}");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar notificaciones: " + ex.Message);
            }
        }

        public DataTable VerNotificaciones()
        {
            try
            {
                // Llama al procedimiento VerNotificaciones para obtener las notificaciones existentes
                return m.consultas("VerNotificaciones", null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las notificaciones: " + ex.Message);
            }
        }


        public bool ExisteNotificacionEnBD(string facturaId)
        {
            try
            {
                // Crear la lista de parámetros
                List<clsParametros> list = new List<clsParametros>();
            
                list.Add(new clsParametros("p_FacturaId", facturaId));


                // Ejecutar el procedimiento almacenado y obtener el resultado en un DataTable
                DataTable result = m.consultas("ExisteNotificacion", list);

                int count = Convert.ToInt32(result.Rows[0]["NotificacionCount"]);

                // Verificar si el resultado contiene filas
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false; // Si no hay filas, no existe la notificación
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar notificación: {ex.Message}");
                return false;
            }
        }
    }
}
