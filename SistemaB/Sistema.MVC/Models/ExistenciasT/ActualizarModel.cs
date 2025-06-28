using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.ExistenciasT
{
    public class ActualizarModel
    {
        public int PK_TIENDA { get; set; }
        public int PK_INVENTARIOT { get; set; }
        public List<CategoriaSuperiorModel> categorias { get; set; }
    }
}
