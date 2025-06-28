using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.ExistenciasT
{
    public class ImportarModel
    {
        public int PK_EXISTENCIAST { get; set; }
        public string PK_ARTICULO { get; set; }
        public string TALLA { get; set; }
        public int CANTIDAD { get; set; }
        public int PK_INVENTARIOT { get; set; }
    }
}
