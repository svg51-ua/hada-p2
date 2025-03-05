using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    public class Tablero
    {
        private int _tamTablero;

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

        private List<Coordenada> coordenadasDisparadas;
        private List<Coordenada> coordenadasTocadas;
        private List<Barco> barcos;
        private List<Barco> barcosEliminados;
        private Dictionary<Coordenada, string> casillasTablero;

        public event EventHandler<TocadoArgs> BarcoTocado;
        public event EventHandler<HundidoArgs> BarcoHundido;
        public event EventHandler<EventArgs> EventoFinPartida;

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

        public void Disparar (Coordenada c)
        {
            if (c.Fila < 0 || c.Fila >= TamTablero || c.Columna < 0 || c.Columna >= TamTablero)
            {
                Console.WriteLine($"La coordenada {c} está fuera del tablero.");
            }
            coordenadasDisparadas.Add(c);
            for(int i = 0; i < barcos.Count; i++)
            {
                barcos[i].Disparo(c);
            }
        }

        public string DibujarTablero()
        {
            int maxTam = 4;
            for (int k = 0; k < barcos.Count; k++)
            {
                if (barcos[k].Nombre.Length > maxTam)
                {
                    maxTam = barcos[k].Nombre.Length;
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < TamTablero; i++)
            {
                for(int j = 0; j < TamTablero; j++)
                {
                    Coordenada c = new Coordenada(i, j);
                    int total = maxTam - casillasTablero[c].Length;
                    int left = (total + 1) / 2;
                    int right = total / 2;
                    string contenido = new string (' ', left) + casillasTablero[c] + new string (' ', right);
                    stringBuilder.Append("[" + contenido + "]");
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

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
            stringBuilder.Append(DibujarTablero());
            return stringBuilder.ToString();
        }
        
        private void cuandoEventoTocado(object sender, TocadoArgs e)
        {
            casillasTablero[e.CoordenadaImpacto] = e.Nombre + "_T";
            if (!coordenadasTocadas.Contains(e.CoordenadaImpacto))
            {
                coordenadasTocadas.Add(e.CoordenadaImpacto);
                Console.WriteLine($"TABLERO: Barco [{e.Nombre}] tocado en Coordenada: [{e.CoordenadaImpacto}]");
            }
        }

        private void cuandoEventoHundido(object sender, HundidoArgs e)
        {
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
                EventoFinPartida(this, EventArgs.Empty);
            }
        }


    }
}
