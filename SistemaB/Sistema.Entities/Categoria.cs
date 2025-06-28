using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class Categoria
    {
        public int PK_CATEGORIA { get; set; }
        public string DESCRIPCION { get; set; }
        public string ACTIVO { get; set; }
        public string PK_CATEGORIA_SUP { get; set; }
        public CategoriaSuperior categoriaSuperior { get; set; }

        public ICollection<Articulo> articulo { get; set; }
    }
}
