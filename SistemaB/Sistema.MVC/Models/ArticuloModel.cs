using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models
{
    public class ArticuloModel
    {
        public string PK_ARTICULO { get; set; }
        public string DESCRIPCION { get; set; }
        public string OPCION_TALLA { get; set; }
        public string ACTIVO { get; set; }
        public int PK_CATEGORIA { get; set; }
    }
}
