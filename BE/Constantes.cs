using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Constantes
    {
        public static string QGETALLCOMME = @"select c.idComercio,c.nombreComercio,c.logo,cc.categoria,c.estrellas,c.fechaAfiliacion,c.descripcion from Comercio as c
        inner join CategoriasComer cc on c.categoriaComer = cc.idCategoria";

        public static string QGETCATBYID = @"select c.idComercio,c.nombreComercio,c.logo,cc.categoria,c.estrellas,c.fechaAfiliacion,c.descripcion from Comercio as c
        inner join CategoriasComer cc on c.categoriaComer = cc.idCategoria
		where cc.categoria = ";

        public static string QGETPRODUCTBYCOMME= @"select SP.idProducto,SP.sucursal,SP.nombreProducto,SP.existencias,SP.descripcion,SP.fechaVencimiento,SP.ilustracion,SP.precio,SP.isPromocion
        from SucursalProductos as SP
        inner join SucursalesComercio as SC on SC.idSucursal = SP.sucursal
        inner join Comercio as C on C.idComercio = SC.comercio
        where C.idComercio =";

        public static string QGETSUCBYCOMME = @"select * from SucursalesComercio
        where comercio =";

        public static string QGETCATEGORIES = @"Select * from ECOMMER.dbo.CategoriasComer";
    }
}
