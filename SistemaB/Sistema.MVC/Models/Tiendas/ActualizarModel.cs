using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.Tiendas
{
    public class ActualizarModel
    {
        public int PK_TIENDA { get; set; }
        public string PK_TIPO_TDA { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string CIUDAD { get; set; }
        public string CONSIGNATARIO { get; set; }
        public string CARNET { get; set; }
        public string RAZONSOCIAL { get; set; }
        public string NIT { get; set; }
        public string ACTIVO { get; set; }
    }
}
