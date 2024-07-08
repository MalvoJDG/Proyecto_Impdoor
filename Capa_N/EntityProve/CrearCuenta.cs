using Capa_A;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Capa_N.EntityProve
{
    public class CrearCuentas
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string UserName { get; set; }
        public string Contraseña { get; set; }
        public string ConContraseña { get; set; }
        public int RoleId { get; set; }
        public byte[] Imagen { get; set; }

        clsManejador m = new clsManejador();

        public string RegistrarCuentas()
        {
            string msj = "";
            List<clsParametros> list = new List<clsParametros>();

            try
            {
                list.Add(new clsParametros("p_Nombre", Nombre));
                list.Add(new clsParametros("p_Username", UserName));
                list.Add(new clsParametros("p_Contraseña", Contraseña));
                list.Add(new clsParametros("p_Confir_Contraseña", ConContraseña));
                list.Add(new clsParametros("p_Roleid", RoleId));
                list.Add(new clsParametros("p_Imagen", Imagen));
                list.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("InsertarUsuario", list);

                msj = list[6].valor.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return msj;
        }

        public string IniciarSesion(string nombreUsuario, string contraseña)
        {
            string mensaje = "";
            List<clsParametros> parametros = new List<clsParametros>();

            try
            {
                parametros.Add(new clsParametros("p_NombreUsuario", nombreUsuario));
                parametros.Add(new clsParametros("p_Contraseña", contraseña));
                parametros.Add(new clsParametros("p_Mensaje", MySqlDbType.VarChar, 100));

                m.EjecutarSp("VerificarCredenciales", parametros);

              
                mensaje = parametros[2].valor.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return mensaje;
        }
    }
}
