using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Comercio 
    {
        public int idComercio { get; set; }
        public string nombreComercio { get; set; }
        public byte[] logo { get; set; }
        public int categoriaComer { get; set; }
        public double estrellas { get; set; }
        public DateTime fechaAfiliacion { get; set; }
        public string descripcion { get; set; }

        public List<Sucursal> Sucursals { get; set; }
    }
}
