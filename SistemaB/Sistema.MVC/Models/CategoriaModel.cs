using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models
{
    public class CategoriaModel
    {
        public int PK_CATEGORIA { get; set; }
        public string DESCRIPCION { get; set; }
        public string ACTIVO { get; set; }
        public string PK_CATEGORIA_SUP { get; set; }
    }
}
