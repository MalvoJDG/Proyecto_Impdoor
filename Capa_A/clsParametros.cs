using MySql.Data.MySqlClient;
using System;
using System.Data;



namespace Capa_A
{
    public class clsParametros
    {
        //Parametros
        public string Nombre { get; set; }
        public Object valor { get; set; }
        public MySqlDbType TipoDato { get; set; }
        public Int32 Tamaño { get; set; }
        public ParameterDirection Direccion { get; set; }


        //Constructores
        //Entrada
        public clsParametros(string objNombre, object objValor)
        {
            Nombre = objNombre;
            valor = objValor;
            Direccion = ParameterDirection.Input;


        }

        //Salida
        public clsParametros(string objNombre, MySqlDbType objTipoDato, Int32 objTamaño)
        {
            Nombre = objNombre;
            TipoDato = objTipoDato;
            Tamaño = objTamaño;
            Direccion = ParameterDirection.Output;


        }


    }
}