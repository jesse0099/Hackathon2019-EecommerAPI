using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ClientProfile
    {
        public int ID { get; set; }
        public string PrimerNombre{ get; set; }
        public string SegundoNombre{ get; set; }
        public string Apellido{ get; set; }
        public string SegundoApellido{ get; set; }
        public string Email{ get; set; }
        public byte[] PP { get; set; }
        public DateTime Afiliado{ get; set; }
    }
}
