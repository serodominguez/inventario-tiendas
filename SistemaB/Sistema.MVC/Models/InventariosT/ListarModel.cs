using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.InventariosT
{
    public class ListarModel
    {
        public int PK_INVENTARIOT { get; set; }
        public int PK_TIENDA { get; set; }
        public string CODIGO { get; set; }
        public string SEMANA { get; set; }
        public string FECHAINICIO { get; set; }
        public string FECHAFIN { get; set; }
        public string ESTADO { get; set; }

        public string USUARIO { get; set; }
        public string ROL { get; set; }
    }
}
