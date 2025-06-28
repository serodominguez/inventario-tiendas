using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class ExistenciaT
    {
        public int PK_EXISTENCIAST { get; set; }
        public string PK_ARTICULO { get; set; }
        public string TALLA { get; set; }
        public int CANTIDAD { get; set; }

        public int PK_INVENTARIOT { get; set; }
        public InventarioT inventariosT { get; set; }
        public Articulo articulos { get; set; }
    }
}
