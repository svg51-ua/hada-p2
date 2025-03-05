using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    public class Coordenada
    {
        private int _fila;
        private int _columna;

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

        public Coordenada()
        {
            Fila = Columna = 0;
        }

        public Coordenada(int fila, int columna)
        {
            this.Fila = fila;
            this.Columna = columna;
        }

        public Coordenada(string fila, string columna)
        {
            this.Fila = int.Parse(fila);
            this.Columna = int.Parse(columna);
        }

        public Coordenada(Coordenada c)
        {
            Fila = c.Fila;
            Columna = c.Columna;
        }

        public override String ToString()
        {
            return "(" + Fila + "," + Columna + ")";
        }
        public override int GetHashCode()
        {
            return this.Fila.GetHashCode() ^ this.Columna.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Coordenada otra)
            {
                return this.Fila == otra.Fila && this.Columna == otra.Columna;
            }
            return false;

        }

        public bool Equals(Coordenada coordenada) 
        { 
            if (coordenada == null) return false;  
            return coordenada.Fila == this.Fila && coordenada.Columna == this.Columna;
        }
    }
}
