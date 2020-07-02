using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Productos
    {
        public int idProducto { get; set; }
        public int sucursal { get; set; }
        public string nombreProducto { get; set; }
        public int existencias { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public byte[] ilustracion { get; set; }
        public Decimal precio { get; set; }
        public bool isPromocion { get; set; }
    }
}
