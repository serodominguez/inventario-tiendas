using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class StockModel
    {
        public string PK_ARTICULO { get; set; }
        public string CATEGORIA { get; set; }
        public decimal PRECIO { get; set; }
        public int CANTIDAD { get; set; }
        public string ESTANTE { get; set; }
        public string USUARIO { get; set; }
        public string INICIO { get; set; }
        public string FIN { get; set; }
    }
}
