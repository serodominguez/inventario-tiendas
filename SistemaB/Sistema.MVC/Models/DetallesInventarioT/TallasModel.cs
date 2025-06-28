using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class TallasModel
    {
        public string PK_ARTICULO { get; set; }
        public string PRECIO { get; set; }
        public string TALLA { get; set; }
        public string NUMERACION { get; set; }
        public string DESCRIPCION { get; set; }
        public string CANTIDAD { get; set; }
    }
}
