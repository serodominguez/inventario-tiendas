using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.ExistenciasT
{
    public class ModificarModel
    {
        public int PK_TIENDA { get; set; }
        public int PK_INVENTARIOT { get; set; }
        public List<ImportarModel> existencias { get; set; }
        public List<CategoriaSuperior.SeleccionarModel> categorias { get; set; }
    }
}
