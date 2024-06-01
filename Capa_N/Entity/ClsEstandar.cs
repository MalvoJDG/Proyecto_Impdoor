using Capa_A; // Importa la capa de acceso a datos
using System.Data;



namespace Capa_N
{
    public class ClsEstandar
    {

        public int Id { get; set; }
        public string Nombre { get; set; }



        private clsManejador m = new clsManejador();

        public DataTable Listado(string procedure)
        {
            return m.consultas(procedure, null);
        }

    }




}
