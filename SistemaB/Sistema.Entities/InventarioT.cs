using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entities
{
    public class InventarioT
    {
        public int PK_INVENTARIOT { get; set; }
        public int PK_TIENDA { get; set; }
        public string CODIGO { get; set; }
        public string SEMANA { get; set; }
        public DateTime? FECHAINICIO { get; set; }
        public DateTime? FECHAFIN { get; set; }
        public int? PK_USUARIO { get; set; }
        public string ESTADO { get; set; }
        public ICollection<DetalleInventarioT> detallesInventario { get; set; }
        public ICollection<ExistenciaT> existenciasT { get; set; }
        public Tienda tiendas { get; set; }
    }
}
