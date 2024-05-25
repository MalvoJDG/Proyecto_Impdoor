﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Capa_A
{
    public class clsManejador
    {
        MySqlConnection cnx = new MySqlConnection("Server=localhost;Database=PruebaJ;Uid=root;");

        // Metodo para abrir Conexion
        void abrir_conexion()
        {
            if (cnx.State == ConnectionState.Closed)
            {
                cnx.Open();
            }
        }

        // Metodo para cerrar Conexion
        void Cerrar_Conexion()
        {
            if (cnx.State == ConnectionState.Open)
            {
                cnx.Close();
            }
        }

        // Metodos para ejecutar sp
        public void EjecutarSp(string Nombresp, List<clsParametros> lst)
        {
            MySqlCommand miquery;

            try
            {
                abrir_conexion();
                miquery = new MySqlCommand(Nombresp, cnx);
                miquery.CommandType = CommandType.StoredProcedure;

                if (lst != null)
                {
                    foreach (var parametro in lst)
                    {
                        if (parametro.Direccion == ParameterDirection.Input)
                        {
                            miquery.Parameters.AddWithValue(parametro.Nombre, parametro.valor);
                        }
                        else if (parametro.Direccion == ParameterDirection.Output)
                        {
                            miquery.Parameters.Add(parametro.Nombre, parametro.TipoDato, parametro.Tamaño).Direction = ParameterDirection.Output;
                        }
                    }

                    miquery.ExecuteNonQuery();

                    foreach (var parametro in lst)
                    {
                        if (parametro.Direccion == ParameterDirection.Output)
                        {
                            parametro.valor = miquery.Parameters[parametro.Nombre].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cerrar_Conexion();
            }
        }

        // Metodo para ejecutar Consultas
        public DataTable consultas(string Nombresp, List<clsParametros> lst)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter da;
            try
            {
                da = new MySqlDataAdapter(Nombresp, cnx);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (lst != null)
                {
                    foreach (var parametro in lst)
                    {
                        da.SelectCommand.Parameters.AddWithValue(parametro.Nombre, parametro.valor);
                    }
                }

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
    }
}
