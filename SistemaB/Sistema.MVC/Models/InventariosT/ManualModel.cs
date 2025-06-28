using System.Collections.Generic;

namespace Sistema.MVC.Models.InventariosT
{
    public class ManualModel
    {
        public int PK_INVENTARIOT { get; set; }
        public string USUARIO { get; set; }
        public string ESTANTE { get; set; }
        public string HORAINICIO { get; set; }
        public string HORAFIN { get; set; }
        public List<DetallesInventarioT.ManualModel> productos { get; set; }
    }
}
