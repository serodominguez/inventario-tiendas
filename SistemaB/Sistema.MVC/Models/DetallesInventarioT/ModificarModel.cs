using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class ModificarModel
    {
        public int PK_INVENTARIOT { get; set; }
        public string PK_ARTICULO { get; set; }
        public string CANAL { get; set; }
        public string TALLA { get; set; }
        public int CANTIDAD { get; set; }
        public string CALIDAD { get; set; }
        public string PLANES { get; set; }
        public string ESTANTE { get; set; }
    }
}
