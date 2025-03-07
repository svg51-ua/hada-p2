using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    /// <summary>
    /// Esta clase tiene como función principal es almacenar las coordenadas del barco en el 
    /// tablero, registrar los disparos recibidos y comprobar si el barco se ha hundido.
    /// </summary>
    public class Barco
    {
        /// <summary>
        /// Diccionario que almacena las coordenadas y la etiqueta de cada coordenada
        /// </summary>
        public Dictionary<Coordenada, String> CoordenadasBarco { get; private set; }

        /// <summary>
        /// Nombre del barco
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Daños que tiene el barco
        /// </summary>
        public int NumDanyos { get; set; } 

        //declaracion de los eventos
        /// <summary>
        /// Evento que se dispara cuando un barco es tocado
        /// </summary>
        public event EventHandler<TocadoArgs> EventoTocado;

        /// <summary>
        /// Evento que se dispara cuando un barco es hundido
        /// </summary>
        public event EventHandler<HundidoArgs> EventoHundido;

        /// <summary>
        /// Este constructor crea mediante la información pasada por parametro un nuevo barco
        /// </summary>
        /// <param name="nombre">Nombre del barco</param>
        /// <param name="longitud">Longitud del barco</param>
        /// <param name="orientacion">Orientacion del barco</param>
        /// <param name="coordenadaInicio">Coordenada en la que comienza el barco</param>
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

        /// <summary>
        /// Este metodo marca y cuenta las coordenadas en las que se encuentra el barco que han recibido un 
        /// disparo añadiendo al final de su nombre un _T
        /// </summary>
        /// <param name="c">Coordenada a la que se efectua el disparo</param>
        public void Disparo (Coordenada c)
        {
            if (CoordenadasBarco.ContainsKey(c))
            {
                
                if (!CoordenadasBarco[c].EndsWith("_T")) //nose si hay que hacer esto
                {
                    CoordenadasBarco[c] += "_T";
                    NumDanyos++;
                    EventoTocado(this, new TocadoArgs(Nombre, c));
                }
                if (Hundido())
                {
                    EventoHundido(this, new HundidoArgs(Nombre));
                }

            }

        }

        /// <summary>
        /// Este metodo comprueba si el barco esta hundido, y devuelve true si asi es
        /// </summary>
        /// <returns>Verdadero si está hundido, falso en caso contrario</returns>
        public bool Hundido()
        {
            foreach (var hundido in CoordenadasBarco.Values)
            {
                if (!hundido.EndsWith("_T")) return false;
            }
            return true;
        }

        /// <summary>
        /// Este metodo devuelve una representacion en cadena del barco incluyendo datos como el nombre, el numero
        /// de disparos, si esta hundido y las coordenadas de ese barco.
        /// </summary>
        /// <returns>Cadena con datos del barco</returns>
        public override string ToString()
        {
            bool estado = Hundido();
            string resultado;
            resultado = $"[{Nombre}] - DAÑOS: [{NumDanyos}] - HUNDIDO: [{estado}] -  COORDENADAS:";
            foreach(var coord in CoordenadasBarco)
            {
                resultado += $" [({coord.Key.Fila}, {coord.Key.Columna}) :{Nombre}]";
            }
            Console.WriteLine(resultado);
            return resultado;
        }

    }
}
