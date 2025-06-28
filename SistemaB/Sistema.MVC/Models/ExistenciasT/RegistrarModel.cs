using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.ExistenciasT
{
    public class RegistrarModel
    {
        public int PK_TIENDA { get; set; }
        public string PK_TIPO { get; set; }
        public string HORAINICIO { get; set; }
        public int USUARIO { get; set; }
        public List<CategoriaSuperiorModel> categorias { get; set; }
    }
}
