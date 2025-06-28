using Sistema.MVC.Models.ExistenciasT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.InventariosT
{
    public class RegistrarModel
    {
        public int PK_INVENTARIOT { get; set; }
        public int PK_TIENDA { get; set; }
        public string PK_TIPO { get; set; }
        public string CODIGO { get; set; }
        public string SEMANA { get; set; }
        public string FECHAINICIO { get; set; }
        public string FECHAFIN { get; set; }
        public string ESTADO { get; set; }
        public string ESTANTE { get; set; }
        public string USUARIO { get; set; }
        public List<DetallesInventarioT.RegistarModel> detalles { get; set; }
        public List<ImportarModel> existencias { get; set; }
        public List<CategoriaSuperiorModel> categorias { get; set; }
    }
}
