using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    public class Barco
    {
        public Dictionary<Coordenada, String> CoordenadasBarco { get; private set; }
        public string Nombre { get; set; }
        public int NumDanyos { get; set; } 

        //declaracion de los eventos
        public event EventHandler<TocadoArgs> EventoTocado;
        public event EventHandler<HundidoArgs> EventoHundido;

        public Barco (string nombre, int longitud, char orientacion, Coordenada coordenadaInicio)
        {
            Nombre = nombre;
            NumDanyos = 0;
            CoordenadasBarco = new Dictionary<Coordenada, string>();

            for (int i = 0; i < longitud; i++)
            {
                Coordenada coord;
                if (orientacion == 'h')
                {
                    coord = new Coordenada(coordenadaInicio.Fila, coordenadaInicio.Columna + i);    
                }
                else
                {
                    coord = new Coordenada(coordenadaInicio.Fila + i, coordenadaInicio.Columna);
                }
                CoordenadasBarco[coord] = Nombre;
            }
        }

        public void Disparo (Coordenada c)
        {
            if (CoordenadasBarco.ContainsKey(c))
            {
                CoordenadasBarco[c] += "_T";
                NumDanyos++;
                EventoTocado(this, new TocadoArgs(Nombre, c));

                if (Hundido())
                {
                    EventoHundido(this, new HundidoArgs(Nombre));
                }
            }

        }

        public bool Hundido()
        {
            foreach (var hundido in CoordenadasBarco.Values)
            {
                if (!hundido.EndsWith("_T")) return false;
            }
            return true;
        }

        public override string ToString()
        {
            bool estado = Hundido();
            string resultado = $"[{Nombre}] - DAÑOS: [{NumDanyos}] - HUNDIDO: [{estado}] -  COORDENADAS:";
            foreach(var coord in CoordenadasBarco)
            {
                resultado += $" [({coord.Key.Fila}, {coord.Key.Columna}) :{Nombre}]";
            }
            resultado += $"\n";
            
            return resultado;

        }

    }
}
