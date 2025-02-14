using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Capa_N.Entity
{
    public class FacturaDetalle
    {
        public int Id { get; set; }
        public string Factura { get; set; }
        public string Descripcion { get; set; }
        public string Producto { get; set; }
        public string Material { get; set; }
        public string Madera { get; set; }
        public string Apanelado { get; set; }
        public string Jambas { get; set; }
        public string Size { get; set; }
        public int Cantidad { get; set; }
        public float PrecioUnit { get; set; }
        public float ITBIS { get; set; }
        public float Total { get; set; }


        clsManejador m = new clsManejador();

        public String GuardarFacturaDetalle()
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_Factura", Factura));
                list.Add(new clsParametros("p_Descripcion", Descripcion));
                list.Add(new clsParametros("p_Producto", Producto));
                list.Add(new clsParametros("p_Material", Material));
                list.Add(new clsParametros("p_Madera", Madera));
                list.Add(new clsParametros("p_Apanelado", Apanelado));
                list.Add(new clsParametros("p_Jambas", Jambas));
                list.Add(new clsParametros("p_Size", Size));
                list.Add(new clsParametros("p_Cantidad", Cantidad));
                list.Add(new clsParametros("p_PrecioUnit", PrecioUnit));
                list.Add(new clsParametros("p_ITBIS", ITBIS));
                list.Add(new clsParametros("p_Total", Total));

                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("InsertarDFactura", list);


                msj = list[12].valor.ToString();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return msj;
        }

        public DataTable BuscarPorFactura()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            parametros.Add(new clsParametros("p_Factura", Factura));

            return m.consultas("BuscarDfacturaOrden", parametros);
        }


        public DataTable EliminarDetalle()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            parametros.Add(new clsParametros("p_Factura", Factura));

            return m.consultas("Borrardetalle", parametros);
        }

        public DataTable Historial()
        {
            List<clsParametros> parametros = new List<clsParametros>();
            parametros.Add(new clsParametros("p_Factura", Factura));

            return m.consultas("SeleccionarDFactura", parametros);
        }

        public DataTable MostrarFacturasPorCliente(string nombreCliente)
        {
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_NombreCliente", nombreCliente));

                return m.consultas("MostrarFacturaPorCliente", list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable MostrarPagosPorCliente(string nombreCliente)
        {
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                // Agregar el parámetro del nombre del cliente
                list.Add(new clsParametros("p_NombreCliente", nombreCliente));

                // Ejecutar el stored procedure y obtener un DataTable
                return m.consultas("MostrarPagosPorCliente", list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

