using BE;
using CDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Procedimientos
    {
        public bool LoginClient(string user,string password)
        {
            List<IntType> returned = new List<IntType>();

            string formato = @"select count(*) from Cliente as C where C.userName   =  '{0}' " +
                                        "and C.userPassword =  '{1}' ";
            string query = string.Format(formato,user,password);

            returned = Dal.DbToList(new IntType(),query);

            if (returned.Count > 0)
                if (returned[0].intType>0)
                    return true;
            return false;
        }

        public ClientProfile GetClientProfile(string uniquename)
        {
            List<ClientProfile> returned = new List<ClientProfile>();

            returned = Dal.DbToList(new ClientProfile(), String.Format(@"select p.primerNombre,p.segundoNombre,p.apellido,p.segundApell,
            p.email, p.pp from Persona as p
            inner join Cliente as c on c.idPersona = p.idPersona
            where c.userName = '{0}'
            ",uniquename));

            if (returned.Count > 0)
                return returned[0];
            else
                return new ClientProfile();
            
        }

        #region Internal classes

            internal class BooleanType
            {
                public BooleanType()
                {
                    this.Bit = false;
                }
                public bool Bit { get; set; }
            }

            internal class IntType
            {
                public int intType { get; set; }
            }
        #endregion
    }
}
