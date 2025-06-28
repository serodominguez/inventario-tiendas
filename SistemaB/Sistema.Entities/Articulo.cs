using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class Articulo
    {
        public string PK_ARTICULO { get; set; }
        public string DESCRIPCION { get; set; }
        public string OPCION_TALLA { get; set; }
        public string ACTIVO { get; set; }
        public int PK_CATEGORIA { get; set; }
        public Categoria categoria { get; set; }
        public ICollection<ExistenciaT> existenciasT { get; set; }
    }
}
