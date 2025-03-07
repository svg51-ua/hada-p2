using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    /// <summary>
    /// Esta clase contiene los argumentos del evento cuando un objeto ha sido impactado.
    /// Hereda de EventArgs para ser utilizado en eventos relacionados con impactos.
    /// </summary>
    public class TocadoArgs : EventArgs
    {
        /// <summary>
        /// Nombre del objeto tocado
        /// </summary>
        public string Nombre { get;}

        /// <summary>
        /// Coordenada a la que se dirige el impacto
        /// </summary>
        public Coordenada CoordenadaImpacto { get;}

        /// <summary>
        /// Constructor sobrecargado que inicializa las propiedades por los parametros dados
        /// </summary>
        /// <param name="nombre">Nombre del objeto tocado</param>
        /// <param name="coordenadaImpacto">Coordenada impactada</param>
        public TocadoArgs(string nombre, Coordenada coordenadaImpacto)
        {
            Nombre = nombre;
            CoordenadaImpacto = coordenadaImpacto;
        }
    }

    /// <summary>
    /// Esta clase contiene los rrgumentos del evento cuando un objeto ha sido hundido.
    /// Hereda de EventArgs para ser utilizado en eventos relacionados con la destrucción de objetos.
    /// </summary>
    public class HundidoArgs : EventArgs
    {
        /// <summary>
        /// Nombre del objeto que ha sido hundido
        /// </summary>
        public string Nombre { get; }

        /// <summary>
        /// Constructor sobrecargado
        /// </summary>
        /// <param name="nombre">Nombre del objeto hundido</param>
        public HundidoArgs(string nombre)
        {
            this.Nombre = nombre;
        }
    }
}
