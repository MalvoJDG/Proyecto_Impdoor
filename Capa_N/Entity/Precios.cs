using Capa_A; // Importa la capa de acceso a datos
using System.Collections.Generic;
using System.Data;

public class Precios
{
    public int Id { get; set; }
    public int Tipo_Madera_id { get; set; }
    public int Tipo_Material_id { get; set; }
    public int Producto_id { get; set; }
    public float Ancho { get; set; }
    public float Largo { get; set; }
    public float Precio { get; set; }

    private clsManejador m = new clsManejador();


    // Traer Precio
    public DataTable ObtenerPrecioPuerta()
    {
        List<clsParametros> parametros = new List<clsParametros>();
        parametros.Add(new clsParametros("p_Tipo_madera_id", Tipo_Madera_id));
        parametros.Add(new clsParametros("p_Tipo_material_id", Tipo_Material_id));
        parametros.Add(new clsParametros("p_producto_id", Producto_id));
        parametros.Add(new clsParametros("p_Ancho", Ancho));
        parametros.Add(new clsParametros("p_Largo", Largo));

        return m.consultas("ObtenerPrecioPuerta", parametros);
    }

}
