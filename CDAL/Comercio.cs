//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comercio
    {
        public Comercio()
        {
            this.SucursalesComercios = new HashSet<SucursalesComercio>();
        }
    
        public int idComercio { get; set; }
        public string nombreComercio { get; set; }
        public byte[] logo { get; set; }
        public Nullable<int> categoriaComer { get; set; }
        public Nullable<double> estrellas { get; set; }
        public Nullable<System.DateTime> fechaAfiliacion { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<SucursalesComercio> SucursalesComercios { get; set; }
    }
}
