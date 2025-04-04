using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Capa_N.Entity
{
    public class Documento
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public byte[] Archivo { get; set; }

        clsManejador m = new clsManejador();

        public String GuardarDocumento()
        {
            String msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("pNumero", Numero));
                list.Add(new clsParametros("pArchivo", Archivo));

                list.Add(new clsParametros("pMensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("GuardarDocumento", list);


                msj = list[2].valor.ToString();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return msj;
        }

        public byte[] DescargarDocumento()
        {
            byte[] archivo = null;
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("PNumero", Numero));
                list.Add(new clsParametros("pArchivo", MySqlDbType.MediumBlob, 0) { Direccion = ParameterDirection.Output });
                list.Add(new clsParametros("pMensaje", MySqlDbType.VarChar, 100) { Direccion = ParameterDirection.Output });


                try
                {
                    m.EjecutarSp("DescargarDocumento", list);
                    // Depuración
                    archivo = list[1].valor as byte[]; // Verifica si es binario
                    string mensaje = list[2].valor?.ToString() ?? "Mensaje vacío";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al ejecutar el procedimiento: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }

            return archivo;
        }


    }
}
