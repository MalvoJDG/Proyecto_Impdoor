using System.Data;
using Capa_N;

public class Jambas: ClsEstandar
{

    public DataTable ObtenerListadoJambas()
    {
        string procedure = "Buscar_Jambas";
        return Listado(procedure);
    }

  

    // Método para registrar un nuevo usuario
    /* public string GuardarUsuario()
     {sadasdasda
         string mensaje = "";
         List<clsParametros> lst = new List<clsParametros>();

         try
         {
             lst.Add(new clsParametros("@Nombre", Nombre));
             lst.Add(new clsParametros("@Usuario", Usuario));
             lst.Add(new clsParametros("@Password", Password));
             lst.Add(new clsParametros("@Tipo_Usuario", Tipo_Usuario));
             lst.Add(new clsParametros("@Mensaje", SqlDbType.VarChar, 100));

             m.EjecutarSp("GuardarUsuario", lst);
             mensaje = lst[4].valor.ToString();
         }
         catch (Exception ex)
         {
             throw ex;
         }

         return mensaje;
     }

     // Método para obtener todos los usuarios
     public DataTable ObtenerTodosUsuarios()
     {
         return m.consultas("ObtenerTodosUsuarios", null);
     }

     // Método para obtener un usuario por su ID
     public DataTable ObtenerUsuarioPorId(int usuarioId)
     {
         List<clsParametros> parametros = new List<clsParametros>();
         parametros.Add(new clsParametros("@Id", usuarioId));

         return m.consultas("ObtenerUsuarioPorId", parametros);
     }

     // Método para autenticar usuario por nombre de usuario y contraseña
     public bool AutenticarUsuario(string username, string password)
     {
         List<clsParametros> parametros = new List<clsParametros>();
         parametros.Add(new clsParametros("@Usuario", username));
         parametros.Add(new clsParametros("@Password", password));

         DataTable dt = m.consultas("AutenticarUsuario", parametros);

         return dt != null && dt.Rows.Count > 0; // Devuelve true si se encuentra al menos un usuario con las credenciales proporcionadas
     }

     public int ObtenerTipo(string username)
     {
         int tipoUsuario = -1; // Valor predeterminado en caso de error o usuario no encontrado
         List<clsParametros> lst = new List<clsParametros>();

         try
         {
             // Parámetro de entrada para el nombre de usuario
             lst.Add(new clsParametros("@Usuario", username));

             // Ejecutar el procedimiento almacenado para obtener el tipo de usuario
             DataTable dt = m.consultas("ObtenerTipoUsuario", lst);

             // Verificar si se obtuvieron resultados y obtener el tipo de usuario
             if (dt != null && dt.Rows.Count > 0)
             {
                 // Obtener el tipo de usuario de la primera fila de resultados
                 tipoUsuario = Convert.ToInt32(dt.Rows[0]["Tipo_Usuario"]);
             }
         }
         catch (Exception ex)
         {
             // Manejar la excepción según tus necesidades
             Console.WriteLine("Error al obtener el tipo de usuario: " + ex.Message);
         }

         return tipoUsuario;
     }

     */

}
