using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class Usuario
    {
        public int PK_USUARIO { get; set; }
        public string USUARIO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string CARNET { get; set; }
        public byte[] PASSWORDHASH { get; set; }
        public byte[] PASSWORDSALT { get; set; }
        public string ESTADO { get; set; }
        public int PK_ROL { get; set; }
        public Rol rol { get; set; }


    }
}
