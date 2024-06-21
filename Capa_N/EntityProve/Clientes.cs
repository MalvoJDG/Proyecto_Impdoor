using Capa_A; // Importa la capa de acceso a datos
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Capa_N.EntityProv
{
    public class Cliente
    {
        // Atributos


        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Rnc { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Mensaje { get; set; }

        clsManejador m = new clsManejador(); //Regerencia para la clase clsManejador

        //Registrar Clientes

        public String RegistrarClientes()
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_Nombre", Nombre));
                list.Add(new clsParametros("p_Rnc", Rnc));

                list.Add(new clsParametros("p_Correo", Correo));
                list.Add(new clsParametros("p_Direccion", Direccion));
                list.Add(new clsParametros("p_Telefono", Telefono));



                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("CrearCliente", list);


                msj = list[5].valor.ToString();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return msj;
        }
        public DataTable ListadoClientes()
        {
            return m.consultas("ListadoCliente", null);
        }

        public void BuscarCliente(string nombreBusqueda)
        {
            List<clsParametros> list = new List<clsParametros>();
            list.Add(new clsParametros("p_nombre", nombreBusqueda));

            DataTable dt = m.consultas("BuscarClientes", list);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Nombre = row["Nombre"].ToString();
                Rnc = row["Rnc"].ToString();
                Correo = row["Correo"].ToString();
                Telefono = row["Telefono"].ToString();
                Direccion = row["Direccion"].ToString();
            }
        }


        public string borrarCliente(string Id)
        {


            String msj = "";
            List<clsParametros> list = new List<clsParametros>();


            try
            {
                // Parametros de entrada
                list.Add(new clsParametros("p_Id", Id));


                //Parametros de salida
                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("DeleteCliente", list);
                msj = list[1].valor.ToString();

            }
            catch (Exception ex)
            {
                throw ex;

            }

            return msj;

        }

        public string EditarCliente(string id)
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();


            try
            {
                // Parametros de entrada
                list.Add(new clsParametros("p_Id", Id));
                list.Add(new clsParametros("p_Nombre", Nombre));
                list.Add(new clsParametros("p_Rnc", Rnc));
                list.Add(new clsParametros("p_Correo", Correo));
                list.Add(new clsParametros("p_Direccion", Direccion));
                list.Add(new clsParametros("p_Telefono", Telefono));


                //Parametros de salida
                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("ActualizarCliente", list);
                msj = list[6].valor.ToString();

            }
            catch (Exception ex)
            {
                throw ex;

            }

            return msj;

        }
    }
}