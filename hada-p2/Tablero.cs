using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    /// <summary>
    /// Esta clase representa el tablero del juego donde se colocan los barcos y se registran los
    /// disparos, también gestion los eventos de impacto y hundimiento de barcos. 
    /// </summary>
    public class Tablero
    {
        /// <summary>
        /// Campo de respaldo de TamTablero
        /// </summary>
        private int _tamTablero;

        /// <summary>
        /// Propiedad que representa el tamaño del tablero, cuyos valores deben estar entre 4 y 9
        /// </summary>
        public int TamTablero
        {
            get { return _tamTablero; }
            set
            {
                if (value < 4 || value > 9)
                {
                    throw new ArgumentException(
                        $"{nameof(value)} must be between 4 and 9.");
                }
                _tamTablero = value;
            }
        }

        /// <summary>
        /// Lista de coordenadas donde se han realizado disparos
        /// </summary>
        private List<Coordenada> coordenadasDisparadas;

        /// <summary>
        /// Lista de coordenadas donde se han registrado impactos en barcos
        /// </summary>
        private List<Coordenada> coordenadasTocadas;

        /// <summary>
        /// Lista de barcos que hay en el tablero.
        /// </summary>
        private List<Barco> barcos;

        /// <summary>
        /// Lista de barcos que han sido eliminados
        /// </summary>
        private List<Barco> barcosEliminados;

        /// <summary>
        /// Diccionario que asocia cada coordenada del tablero con su estado
        /// </summary>
        private Dictionary<Coordenada, string> casillasTablero;

        /// <summary>
        /// Evento que se dispara cuando un barco es impactado
        /// </summary>
        public event EventHandler<TocadoArgs> BarcoTocado;

        /// <summary>
        /// Evento que se dispara cuando un barco es hundido
        /// </summary>
        public event EventHandler<HundidoArgs> BarcoHundido;

        /// <summary>
        /// Evento que se dispara cuando se ha acabado la partida
        /// </summary>
        public event EventHandler<EventArgs> EventoFinPartida;

        /// <summary>
        /// Constructor sobrecargado
        /// </summary>
        /// <param name="tamTablero">Tamaño del tablero</param>
        /// <param name="barcos">Lista de barcos que se colocarán en el tablero</param>
        public Tablero(int tamTablero, List<Barco> barcos)
        {
            TamTablero = tamTablero;
            coordenadasDisparadas = new List<Coordenada>();
            coordenadasTocadas = new List<Coordenada>();

            if (barcos != null)
            {
                this.barcos = new List<Barco>();
                for(int i = 0; i < barcos.Count; i++)
                {
                    // suscribo los metodos (handlers) a los eventos. cuando un barco es tocado o 
                    // hundido se ejecuta el metodo correspondiente 
                    barcos[i].EventoTocado += cuandoEventoTocado;
                    barcos[i].EventoHundido += cuandoEventoHundido;
                    this.barcos.Add(barcos[i]);
                }
            }
            else
            {
                this.barcos = new List<Barco>();
            }

            barcosEliminados = new List<Barco>();
            casillasTablero = new Dictionary<Coordenada, string>();
            inicializaCasillasTablero();
        }

        /// <summary>
        /// Método que inicializa las casillas del tablero con AGUA o con el nombre del barco
        /// si hay alguno en esa posición
        /// </summary>
        private void inicializaCasillasTablero()
        {
            for(int i = 0; i < TamTablero; i++)
            {
                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada c = new Coordenada(i,j);
                    string posicion = "AGUA";
                    bool encontrado = false;

                    for (int k = 0; k < barcos.Count && !encontrado; k++)
                    {
                        if (barcos[k].CoordenadasBarco.ContainsKey(c))
                        {
                            posicion = barcos[k].Nombre;
                            encontrado = true;
                        }
                    }
                    casillasTablero[c] = posicion;
                }
            }
        }

        /// <summary>
        /// Método que efectua el disparo llamando al metodo correspondiente.
        /// En caso de introducir una coordenada no válida saldrá un eror
        /// </summary>
        /// <param name="c">coordenada a disparar</param>
        public void Disparar (Coordenada c)
        {
            if (c.Fila < 0 || c.Fila >= TamTablero || c.Columna < 0 || c.Columna >= TamTablero)
            {
                Console.WriteLine($"La coordenada {c} está fuera del tablero.");
            }
            else
            {
                coordenadasDisparadas.Add(c);
                for(int i = 0; i < barcos.Count; i++)
                {
                    barcos[i].Disparo(c);
                }
            }
            
        }

        /// <summary>
        /// Genera una cadena de texto que es una representacion del tablero, formatea el contenido de 
        /// cada celda con espacios para mantener la alineación. En caso de contener agua se imprime en azul.
        /// Si el barco está hundido se imprime en rojo
        /// </summary>
        /// <returns>Cadena de texto con el contenido final</returns>
        /*public string DibujarTablero()
        {
            int maxTam = 0;
            for (int i = 0; i < TamTablero; i++)
            {
                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada co = new Coordenada(i, j);
                    if (casillasTablero[co].Length > maxTam)
                    {
                        maxTam = casillasTablero[co].Length;
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < TamTablero; i++)
            {
                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada c = new Coordenada(i, j);
                    int total = maxTam - casillasTablero[c].Length;
                    int left = (total + 1) / 2;
                    int right = total / 2;
                    string contenido = new string(' ', left) + casillasTablero[c] + new string(' ', right);
                    
                    if (casillasTablero[c].ToLower() == "agua")
                    {
                        stringBuilder.Append("\u001b[36m" + "[" + contenido + "]" + "\u001b[0m"); // Azul claro con ANSI
                    }                   
                    else
                    {
                        bool esHundido = false;
                        foreach (var barco in barcos)
                        {                        
                            if (casillasTablero[c] == (barco.Nombre + "_T") && barco.Hundido())
                            {
                                esHundido = true;
                                break; // Ya sabemos que está hundido, podemos salir del bucle
                            }
                        }

                        // Si el barco en la casilla está hundido, lo imprimimos en rojo
                        if (esHundido)
                        {
                            stringBuilder.Append("\u001b[91m" + "[" + contenido + "]" + "\u001b[0m");
                        }
                        else
                        {
                            stringBuilder.Append("[" + contenido + "]"); 
                        }
                    }
                }
                
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
        */
        public string DibujarTablero()
        {
            int maxTam = 0;

            // Encontrar el tamaño máximo del contenido de una casilla
            for (int i = 0; i < TamTablero; i++)
            {
                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada co = new Coordenada(i, j);
                    if (casillasTablero[co].Length > maxTam)
                    {
                        maxTam = casillasTablero[co].Length;
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();

            // Agregar números de columna 
            stringBuilder.Append(" "); 
            for (int j = 0; j < TamTablero; j++)
            {
                stringBuilder.Append(j.ToString().PadLeft(maxTam + 2)); 
            }
            stringBuilder.AppendLine();

            // Dibujar el tablero con números de fila
            for (int i = 0; i < TamTablero; i++)
            {
                // Agregar número de fila al inicio de cada línea
                stringBuilder.Append(i.ToString().PadLeft(2) + " ");

                for (int j = 0; j < TamTablero; j++)
                {
                    Coordenada c = new Coordenada(i, j);
                    int total = maxTam - casillasTablero[c].Length;
                    int left = (total + 1) / 2;
                    int right = total / 2;
                    string contenido = new string(' ', left) + casillasTablero[c] + new string(' ', right);

                    if (casillasTablero[c].ToLower() == "agua")
                    {
                        stringBuilder.Append("\u001b[36m" + "[" + contenido + "]" + "\u001b[0m"); // Azul claro para agua
                    }
                    else
                    {
                        bool esHundido = false;
                        foreach (var barco in barcos)
                        {
                            if (casillasTablero[c] == (barco.Nombre + "_T") && barco.Hundido())
                            {
                                esHundido = true;
                                break; // Ya sabemos que está hundido, podemos salir del bucle
                            }
                        }

                        // Si el barco en la casilla está hundido, lo imprimimos en rojo
                        if (esHundido)
                        {
                            stringBuilder.Append("\u001b[91m" + "[" + contenido + "]" + "\u001b[0m"); // Rojo brillante
                        }
                        else
                        {
                            stringBuilder.Append("[" + contenido + "]");
                        }
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }



        /// <summary>
        /// Devuelve una representacion del estado del tablero y llama a su vez a dibujarTablero()
        /// para representarlo. Se incluye información sobre los disparos realizados.
        /// </summary>
        /// <returns>Cadena con el estado del tablero</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var barco in barcos)
            {
                barco.ToString();
            }
            stringBuilder.AppendLine();
            stringBuilder.Append("Coordenadas disparadas: ");
            stringBuilder.AppendLine(string.Join(" ", coordenadasDisparadas));
            stringBuilder.Append("Coordenadas tocadas: ");
            stringBuilder.AppendLine(string.Join(" ", coordenadasTocadas));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("CASILLAS TABLERO");
            stringBuilder.AppendLine("----------------");
            stringBuilder.Append(DibujarTablero());
            return stringBuilder.ToString();
        }
        
        /// <summary>
        /// Método manejador de eventos cuando un barco es tocado. Se actualiza en el tablero con
        /// la marca de tocado y se registra la coordenada impactada.
        /// </summary>
        /// <param name="sender">objeto que envía el evento</param>
        /// <param name="e">argumentos del evento que contienen el nombre del barco impactado y la coordenada</param>
        private void cuandoEventoTocado(object sender, TocadoArgs e)
        {
            casillasTablero[e.CoordenadaImpacto] = e.Nombre + "_T";
            if (!coordenadasTocadas.Contains(e.CoordenadaImpacto))
            {
                coordenadasTocadas.Add(e.CoordenadaImpacto);
                Console.WriteLine($"TABLERO: Barco {e.Nombre} tocado en Coordenada: [{e.CoordenadaImpacto}]");
            }
        }

        /// <summary>
        /// Método manejador de eventos cuando un barco es hundido. Comprueba si todos los 
        /// barcos han sido eliminados y finaliza la partida si es necesario.
        /// </summary>
        /// <param name="sender">objeto que envia el evento</param>
        /// <param name="e">argumentos del evento que contienen el nombre del barco hundido</param>
        private void cuandoEventoHundido(object sender, HundidoArgs e)
        {
            foreach (var barco in barcos)
            {
                if (barco.Nombre == e.Nombre)
                {
                    barcosEliminados.Add(barco);
                    break;
                }
            }
            Console.WriteLine($"TABLERO: Barco [{e.Nombre}] hundido!!");
            bool hundido = true;
            foreach (var barco in barcos)
            {
                if (!barco.Hundido())
                {
                    hundido = false;
                }
            }

            if (hundido)
            {
                Console.WriteLine("PARTIDA FINALIZADA!!\n");
                EventoFinPartida(this, EventArgs.Empty);
            }
        }



    }
}
