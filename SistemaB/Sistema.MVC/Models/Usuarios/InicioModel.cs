using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sistema.MVC.Models.Usuarios
{
    public class InicioModel
    {
        [Required]
        public string USUARIO { get; set; }
        [Required]
        public string CLAVE { get; set; }
    }
}
