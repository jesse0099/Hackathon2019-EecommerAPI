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

    }
}
