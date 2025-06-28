using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class TotalModel
    {
        public decimal TOTAL { get; set; }
        public int CANTIDAD { get; set; }

        public int ACCESORIOS { get; set; }
        public int PARES { get; set; }
        public decimal PRECIOA { get; set; }
        public decimal PRECIOP { get; set; }
    }
}
