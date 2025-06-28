using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class ListarModel
    {
        public string PK_ARTICULO { get; set; }
        public string TALLA { get; set; }
        public string CATEGORIA { get; set; }
        public string EXISTENCIAS { get; set; }
        public string CONTADOS { get; set; }
        public string DIFERENCIAS { get; set; }
        public string CANTIDAD { get; set; }
        public decimal PRECIO { get; set; }
        public decimal TOTAL { get; set; }
    }
}
