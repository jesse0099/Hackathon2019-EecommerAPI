using BE;
using CDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace BL
{
    public class Procedimientos
    {
        #region SearchController
        public List<Categoria> GetCategories() {
            return Dal.DbToList(new Categoria(), $"{Constantes.QGETCATEGORIES}");
        }
        #endregion

        #region Client
        public bool LoginClient(string user, string password)
        {
            List<IntType> returned = new List<IntType>();

            string formato = @"select count(*) from Cliente as C where C.userName   =  '{0}' " +
                                        "and C.userPassword =  '{1}' ";
            string query = string.Format(formato, user, password);

            returned = Dal.DbToList(new IntType(), query);

            if (returned.Count > 0)
                if (returned[0].intType > 0)
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
            ", uniquename));

            if (returned.Count > 0)
                return returned[0];
            else
                return new ClientProfile();

        }

        public ClientProfile GetClientProfile(string pass,string user)
        {
            List<ClientProfile> returned = new List<ClientProfile>();

            returned = Dal.DbToList(new ClientProfile(), String.Format(@" select p.idPersona,p.primerNombre,p.segundoNombre,p.apellido,p.segundApell,
            p.email, p.pp,p.afiliado from Persona as p
            inner join Cliente as c on c.idPersona = p.idPersona
            where c.userName = '{0}'
			and c.userPassword ='{1}'
            ", user,pass));

            if (returned.Count > 0)
                return returned[0];
            else
                return new ClientProfile();

        }

        public ClientProfile UpdateClientProfile(ClientProfile newValue) {

            SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
            cnx.Open();
            string query = $@"Update Persona
                                set primerNombre =@primerNombre,
                                segundoNombre =@segundoNombre,
                                apellido =@apellido,
                                segundApell =@segundApell,
                                email = @email,
                                pp = @pp
                                where idPersona = @idPersona;";
            using (SqlCommand cmd = new SqlCommand(query,cnx)) {
                cmd.Parameters.Add(new SqlParameter("@primerNombre",newValue.PrimerNombre));
                cmd.Parameters.Add(new SqlParameter("@segundoNombre",newValue.SegundoNombre));
                cmd.Parameters.Add(new SqlParameter("@apellido",newValue.Apellido));
                cmd.Parameters.Add(new SqlParameter("@segundApell",newValue.SegundoApellido));
                cmd.Parameters.Add(new SqlParameter("@email",newValue.Email));
                cmd.Parameters.Add(new SqlParameter("@pp",newValue.PP));
                cmd.Parameters.Add(new SqlParameter("@idPersona",newValue.ID));
                cmd.ExecuteNonQuery();
            }
            cnx.Close();
            return newValue;
        }

        public ClientCredentials UpdateClientCredentials(ClientCredentials newValue) {
            string query = $@"Update Cliente
                            set userPassword = '{newValue.Password}'
                            where idCliente = (select C.idCliente from Cliente as C Inner join
                            Persona as P on P.idPersona = C.idPersona
                            where P.idPersona = {newValue.IdPersona})";
            Dal.ExecuteSql(query);
            return newValue;
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
        #endregion

        #region Comercios
        public List<Comercio> getAllComme() {
          return Dal.DbToList(new Comercio(),Constantes.QGETALLCOMME);
        }

        public List<Comercio> getByCat(string category) {
            return Dal.DbToList(new Comercio(),$@"{Constantes.QGETCATBYID} '{category}'");
        }
        public List<Sucursal> getSucByCommer(int idCommer) {
            return Dal.DbToList(new Sucursal(), $@"{Constantes.QGETSUCBYCOMME} {idCommer}");
        }
        #endregion

        #region Productos
        public List<Productos> getByCommer(int idComercio) {
            return Dal.DbToList(new Productos(),$@"{Constantes.QGETPRODUCTBYCOMME} {idComercio}");
        }
        #endregion

    }
}
