using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ncf
{
    /* Ver como le aplicamos la POO
     * (nota mental)poner todos los store prosedure en ingles o en español
     * estoy sufriendo esquizofrenia
    */
    public string Id { get; set; } //esta vaina se repite demasiado hay que reducirlo

    public string Codigo { get; set; }

    clsManejador m = new clsManejador();



    public String RegistrarNCF()
    {
        String msj = "";
        List<clsParametros> list = new List<clsParametros>();

        try
        {
            list.Add(new clsParametros("p_Codigo", Codigo));



            list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

            m.EjecutarSp("CreateNcf", list);


            msj = list[1].valor.ToString();

        }

        catch (Exception ex)
        {
            throw ex;
        }

        return msj;
    }



    public DataTable ListadoNCF()
    {

        return m.consultas("ListadoNcf", null);

    }

    public DataTable SecuenciaFiscal()
    {
        return m.consultas("ListadoNcf", null);

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

            m.EjecutarSp("EliminarNcf", list);
            msj = list[1].valor.ToString();

        }
        catch (Exception ex)
        {
            throw ex;

        }

        return msj;

    }

    public string EditarNCF(string Id)
    {
        String msj = "";
        List<clsParametros> list = new List<clsParametros>();


        try
        {
            // Parametros de entrada
            list.Add(new clsParametros("p_Id", Id));
            list.Add(new clsParametros("p_Codigo", Codigo));


            //Parametros de salida
            list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

            m.EjecutarSp("ActualizarNcf", list);
            msj = list[2].valor.ToString();

        }
        catch (Exception ex)
        {
            throw ex;

        }

        return msj;

    }

    public string AsignarNCF(string IdFactura)
    {
        string msj = "";
        List<clsParametros> list = new List<clsParametros>();

        try
        {
            // Parámetros de entrada
            list.Add(new clsParametros("p_IdFactura", IdFactura));

            // Parámetros de salida
            list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

            // Ejecutar el Stored Procedure
            m.EjecutarSp("AsignarNCF", list);

            // Obtener el mensaje de salida
            msj = list[1].valor.ToString();
        }
        catch (Exception ex)
        {
            // Manejar la excepción según sea necesario
            throw ex;
        }

        return msj;
    }









}