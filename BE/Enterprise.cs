using BE;
using System;

namespace BL
{
    public class Enterprise
    {
        public int  Id{ get; set; }
        public string Nombre { get; set; }
        public byte[] Logo{ get; set; }
        public Categoria Categoria { get; set; }
        public double Estrellas { get; set; }
        public DateTime FechaAfiliacion { get; set; }
        public string Descripcion { get; set; }


    }
}
