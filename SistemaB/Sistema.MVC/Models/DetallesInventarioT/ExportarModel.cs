using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.MVC.Models.DetallesInventarioT
{
    public class ExportarModel
    {
        public int PK_INVENTARIOT { get; set; }
        public string PK_TIENDA { get; set; }
        public string CODIGO { get; set; }
        public string SEMANA { get; set; }
        public string FECHAINICIO { get; set; }
        public string FECHAFIN { get; set; }
        public string HORAINICIO { get; set; }
        public string HORAFIN { get; set; }
        public string ESTADO { get; set; }
        public string USUARIO { get; set; }


        public int ACCESORIOSINICIAL { get; set; }
        public int PARESINICIAL { get; set; }
        public decimal PRECIOINICIALA { get; set; }
        public decimal PRECIOINICIALP { get; set; }


        public int ACCESORIOSLECTURA { get; set; }
        public int PARESLECTURA { get; set; }
        public decimal PRECIOLECTURAA { get; set; }
        public decimal PRECIOLECTURAP { get; set; }


        public int ACCESORIOSDIFERENCIA { get; set; }
        public int PARESDIFERENCIA { get; set; }
        public decimal PRECIODIFERENCIAA { get; set; }
        public decimal PRECIODIFERENCIAP { get; set; }

        public int TOTALINICIALU { get; set; }
        public decimal TOTALINICIALP { get; set; }
        public int TOTALLECTURAU { get; set; }
        public decimal TOTALLECTURAP { get; set; }
        public int TOTALDIFERENCIAU { get; set; }
        public decimal TOTALDIFERENCIAP { get; set; }
    }
}
