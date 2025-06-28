using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class CategoriaSuperior
    {
        public string PK_CATEGORIA_SUP { get; set; }
        public string DESCRIPCION { get; set; }
        public string ACTIVO { get; set; }
        public ICollection<Categoria> categoria { get; set; }
    }
}
