using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class Rol
    {
        public int PK_ROL { get; set; }
        public string ROL { get; set; }
        public string ESTADO { get; set; }

        public ICollection<Usuario> usuario { get; set; }
    }
}
