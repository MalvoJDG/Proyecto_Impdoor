using System;
using System.Collections.Generic;
using System.Data;
using Capa_A; // Importa la capa de acceso a datos
using MySql.Data.MySqlClient;

namespace Capa_N.EntityProv
{
    public class Cliente
    {
        // Atributos
        public string Nombre { get; set; }

        public string Rnc { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
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


                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("CrearCliente", list);


                msj = list[4].valor.ToString();

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


    }
}