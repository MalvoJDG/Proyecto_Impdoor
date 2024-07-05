using Capa_A; // Importa la capa de acceso a datos
using Capa_N;
using System.Data;

public class Materiales : ClsEstandar
{

    private clsManejador m = new clsManejador();


    // Listar Materiales
    public DataTable ListadoMateriales()
    {
        return m.consultas("Buscar_Materiales", null);
    }

}
