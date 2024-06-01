using Capa_A; // Importa la capa de acceso a datos
using System.Collections.Generic;
using System.Data;

public class Precios
{
    public int Id { get; set; }
    public float Precio { get; set; }
    public int Producto_id { get; set; }
    public int Material_id { get; set; }
    public int Tipo_Madera { get; set; }
    public int Apanelado_id { get; set; }
    public int Jambas_id { get; set; }
    public int Size_id { get; set; }

    private clsManejador m = new clsManejador();


    // Traer Precio
    public DataTable ObtenerPrecioProducto()
    {
        List<clsParametros> parametros = new List<clsParametros>();
        parametros.Add(new clsParametros("producto_id", Producto_id));
        parametros.Add(new clsParametros("material_id", Material_id));
        parametros.Add(new clsParametros("tipo_madera_id", Tipo_Madera));
        parametros.Add(new clsParametros("apanelado_id", Apanelado_id));
        parametros.Add(new clsParametros("jambas_id", Jambas_id));
        parametros.Add(new clsParametros("size_id", Size_id));

        return m.consultas("ObtenerPrecioProducto", parametros);
    }

    /* public string GuardarUsuario()
     {
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
