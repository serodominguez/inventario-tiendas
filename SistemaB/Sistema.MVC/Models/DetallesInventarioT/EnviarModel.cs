using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class EnviarModel
    {
        public string PK_ARTICULO { get; set; }
        public string TALLA { get; set; }
        public decimal VENTA { get; set; }
        public int CANTIDAD { get; set; }
    }
}
