using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    /// <summary>
    /// Representa una coordenada en un tablero con valores limitados entre 0 y 9.
    /// </summary>
    public class Coordenada
    {
        /// <summary>
        /// Campo de respaldo de la variable Fila
        /// </summary>
        private int _fila;

        /// <summary>
        /// Campo de respaldo de la variable Columna
        /// </summary>
        private int _columna;

        /// <summary>
        /// Obtiene o establece el valor de la fila dentro del rango de 0 a 9.
        /// Lanza una excepción si el valor está fuera de este rango.
        /// </summary>
        public int Fila
        {
            get { return _fila; }
            set
            {
                if (value < 0 || value > 9)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(value)} must be between 0 and 9.");
                }
                _fila = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el valor de la columna dentro del rango de 0 a 9.
        /// Lanza una excepción si el valor está fuera de este rango.
        /// </summary>
        public int Columna
        {
            get { return _columna; }
            set
            {
                if (value < 0 || value > 9)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(value)} must be between 0 and 9.");
                }
                _columna = value;
            }
        }
        /// <summary>
        /// Constructor por defecto que inicializa la coordenada a (0,0)
        /// </summary>
        public Coordenada()
        {
            Fila = Columna = 0;
        }

        /// <summary>
        /// Constructor sobrecargado que inicializa fila, columna
        /// </summary>
        /// <param name="fila">Valor de la fila</param>
        /// <param name="columna">valor de la columna</param>
        public Coordenada(int fila, int columna)
        {
            this.Fila = fila;
            this.Columna = columna;
        }

        /// <summary>
        /// Constructor que inicializa los valores pasandolos de string a int
        /// </summary>
        /// <param name="fila">valor de la fila en formato string</param>
        /// <param name="columna">valor de la columna en formato string</param>
        public Coordenada(string fila, string columna)
        {
            this.Fila = int.Parse(fila);
            this.Columna = int.Parse(columna);
        }

        /// <summary>
        /// Constructor de copia
        /// </summary>
        /// <param name="c"></param>
        public Coordenada(Coordenada c)
        {
            Fila = c.Fila;
            Columna = c.Columna;
        }

        /// <summary>
        /// Método sobreescrito ToString que devuelve una coordenada con el formato (fila,columna)
        /// </summary>
        /// <returns>cadena (fila,columna)</returns>
        public override String ToString()
        {
            return "(" + Fila + "," + Columna + ")";
        }

        /// <summary>
        /// Devuelve el código hash de la coordenada basado en la combinación de fila y columna.
        /// </summary>
        /// <returns>valor hash único para la coordenada</returns>
        public override int GetHashCode()
        {
            return this.Fila.GetHashCode() ^ this.Columna.GetHashCode();
        }

        /// <summary>
        /// Devuelve verdadero si la coordenada actual es igual a otro objeto, devuelve
        /// falso en caso contrario
        /// </summary>
        /// <param name="obj">objeto a comparar</param>
        /// <returns>booleano con el resultado de la comparacion</returns>
        public override bool Equals(object obj)
        {
            if (obj is Coordenada otra)
            {
                return this.Fila == otra.Fila && this.Columna == otra.Columna;
            }
            return false;

        }

        /// <summary>
        /// Comprueba si la coordenada actual es igual a otra coordenada específica
        /// </summary>
        /// <param name="coordenada">Coordenada a comparar</param>
        /// <returns>True si son iguales, falso en caso contrario</returns>
        public bool Equals(Coordenada coordenada) 
        { 
            if (coordenada == null) return false;  
            return coordenada.Fila == this.Fila && coordenada.Columna == this.Columna;
        }
    }
}
