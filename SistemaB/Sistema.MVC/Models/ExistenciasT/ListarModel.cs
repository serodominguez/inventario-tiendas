using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.ExistenciasT
{
    public class ListarModel
    {
        public int PK_INVENTARIOT { get; set; }
        public int PK_TIENDA { get; set; }
        public string CODIGO { get; set; }
        public string SEMANA { get; set; }
        public string HORAINICIO { get; set; }
        public string ESTANTE { get; set; }
        public string USUARIO { get; set; }
        public string ESTADO { get; set; }
        public string TIENDA { get; set; }
        public string CONSIGNATARIO { get; set; }
        public string ROL { get; set; }
    }
}
