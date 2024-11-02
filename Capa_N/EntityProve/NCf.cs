using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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

    public string AsignarNCF(string IdFactura, out string nuevoNCF)
    {
        nuevoNCF = string.Empty; // Inicializa la variable para el nuevo NCF
        string msj = "";
        List<clsParametros> list = new List<clsParametros>();

        try
        {
            // Parámetros de entrada
            list.Add(new clsParametros("p_IdFactura", IdFactura));

            // Parámetros de salida
            list.Add(new clsParametros("p_NuevoNCF", MySqlDbType.VarChar, 20)); // Parámetro de salida para el nuevo NCF
            list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100)); // Parámetro de salida para el mensaje

            // Ejecutar el Stored Procedure
            m.EjecutarSp("AsignarNCF", list);

            // Obtener el nuevo NCF y el mensaje de salida
            nuevoNCF = list[1].valor.ToString(); // Suponiendo que el nuevo NCF está en la segunda posición
            msj = list[2].valor.ToString();      // Mensaje en la tercera posición
        }
        catch (Exception ex)
        {
            // Manejar la excepción según sea necesario
            throw ex;
        }

        return msj;
    }

    public string ObtenerNuevoNCF(out string nuevoNCF)
    {
        nuevoNCF = string.Empty; // Inicializa la variable para el nuevo NCF
        string msj = "";
        List<clsParametros> list = new List<clsParametros>();

        try
        {
            // Parámetros de salida
            list.Add(new clsParametros("p_NuevoNCF", MySqlDbType.VarChar, 20)); // Parámetro de salida para el nuevo NCF
            list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100)); // Parámetro de salida para el mensaje

            // Ejecutar el Stored Procedure que solo obtiene un NCF disponible
            m.EjecutarSp("ObtenerNuevoNCF", list);

            // Obtener el nuevo NCF y el mensaje de salida
            nuevoNCF = list[0].valor.ToString(); // El nuevo NCF está en la primera posición
            msj = list[1].valor.ToString();      // Mensaje en la segunda posición
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return msj;
    }










}