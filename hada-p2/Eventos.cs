using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    public class TocadoArgs : EventArgs
    {
        public string Nombre { get;}
        public Coordenada CoordenadaImpacto { get;}

        public TocadoArgs(string nombre, Coordenada coordenadaImpacto)
        {
            Nombre = nombre;
            CoordenadaImpacto = coordenadaImpacto;
        }
    }

    public class HundidoArgs : EventArgs
    {
        public string Nombre { get; }

        public HundidoArgs(string nombre)
        {
            this.Nombre = nombre;
        }
    }
}
