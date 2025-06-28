using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.Usuarios
{
    public class ActualizarModel
    {
        public int PK_USUARIO { get; set; }
        public string USUARIO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string CARNET { get; set; }
        public string ESTADO { get; set; }
        public string ROL { get; set; }
        public int PK_ROL { get; set; }
        public string CLAVE { get; set; }
        public bool ACTUALIZARPASSWORD { get; set; }
    }
}
