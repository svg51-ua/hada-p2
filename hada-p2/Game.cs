using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    /// <summary>
    /// Esta clase se encarga de gestionar el flujo de Hundir la flota. Crea los barcos y el
    /// tablero, maneja las interacciones del jugador y controla la finalización del juego.
    /// </summary>
    public class Game
    {
        private static readonly Random random = new Random();
        private bool finPartida;

        /// <summary>
        /// Este metodo inicializa un tablero y unos barcos para que se le muestren al jugador
        /// y pueda desarrollar el juego, mostrando tambien los disparos y hundimientos.
        /// </summary>
        private void GameLoop()
        {
            int tamTablero = random.Next(4,10);
            

            List<Barco> barcos = generarBarcos(tamTablero);

            Tablero tablero = new Tablero(tamTablero, barcos);
            tablero.EventoFinPartida += cuandoEventoFinPartida;

            while (!finPartida)
            {                    
                Console.WriteLine(tablero.ToString());
                Console.Write("Introduce la cordenada a la que disparar FILA,COLUMNA ('S' para Salir): ");
                string entrada = Console.ReadLine();
                Console.WriteLine("\n");
                if (entrada.ToLower() == "s") // para salir del juego
                {
                    cuandoEventoFinPartida(this, EventArgs.Empty);

                }

                else
                {
                    string[] partes = entrada.Split(',');
                    if (partes.Length == 2 && int.TryParse(partes[0], out int fila) &&
                        int.TryParse(partes[1], out int columna))
                    {
                        Coordenada disparo = new Coordenada(fila, columna);
                        tablero.Disparar(disparo);
                    }
                    else
                    {

                        Console.WriteLine("Formato incorreto, use el formato FILA, COLUMNA. ");
                    }
                }                

            }
        }

        /// <summary>
        /// Genera una lista de barcos con posiciones, orientaciones y tamaños aleatorios, comprobando que
        /// no se superpongan entre ellos y estén siempre en el tablero.
        /// </summary>
        /// <param name="cantidad">cantidad de barcos a generar</param>
        /// <param name="tamTablero">tamaño del tablero</param>
        /// <returns>Lista final con todos los barcos generados</returns>
        public List<Barco> generarBarcos(int tamTablero)
        {
            List<string> nombres = new List<string> { "SANTA MARIA", "LA NIÑA", "LA PINTA", "LOKI", "THOR", "MAYA" };
            List<Barco> barcos = new List<Barco>();
            List<Coordenada> posicionesOcupadas = new List<Coordenada>();
            int numBarcos = random.Next(3, 6);

            for (int i = 0; i < numBarcos; i++)
            {
                // Elige un nombre de los disponibles, lo asigna y lo elimina de la lista para evitar duplicados
                // en la próxima vez que se genere un barco
                int randomNombre = random.Next(nombres.Count);
                string nombre = nombres[randomNombre];
                nombres.RemoveAt(randomNombre); 

                int tamBarco = random.Next(1, 4);

                // determina la orientación del barco
                char orientacion;
                if (random.Next(2) == 0)
                {
                    orientacion = 'h';
                }else
                {
                    orientacion = 'v';
                }

                // genera una coordenada aleatoria y comprueba que este disponible
                Coordenada pos;
                do
                {
                    int fila = random.Next(0, tamTablero);
                    int columna = random.Next(0, tamTablero);
                    pos = new Coordenada(fila, columna);
                } while (!comprobarCoordenada(pos, tamBarco, orientacion, tamTablero, posicionesOcupadas));

                // Añade un barco a la lista y registra las posiciones que ocupa.
                barcos.Add(new Barco(nombre, tamBarco, orientacion, pos));
                for (int j = 0; j < tamBarco; j++)
                {
                    int fila, columna;

                    if (orientacion == 'h')
                    {
                        fila = pos.Fila;
                        columna = pos.Columna + j;
                    } else
                    {
                        fila = pos.Fila + j;
                        columna = pos.Columna;
                    }
                    posicionesOcupadas.Add(new Coordenada(fila, columna));
                }

            }

            return barcos;
        }

        /// <summary>
        /// Método que comprueba si la coordenada en la que se quiere colocar un barco está disponible.
        /// Revisa si todas las casillas que ocuparía un barco están libres y dentro del tablero
        /// </summary>
        /// <param name="posicion">posicion inicial del barco</param>
        /// <param name="tamañoBarco">numero de casillas que ocupa el barco</param>
        /// <param name="orientacion">orientacion del barco</param>
        /// <param name="tamTablero">tamaño del tablero</param>
        /// <param name="ocupadas">lista con las posiciones uqe ya estan ocupadas</param>
        /// <returns></returns>
        private bool comprobarCoordenada(Coordenada posicion, int tamañoBarco, char orientacion, int tamTablero, List<Coordenada> ocupadas)
        {
            for (int i = 0; i < tamañoBarco; i++)
            {
                int fila, columna;

                if (orientacion == 'h')
                {
                    fila = posicion.Fila;
                    columna = posicion.Columna + i;
                } else
                {
                    fila = posicion.Fila + i;
                    columna = posicion.Columna;
                }

                if(fila >=tamTablero || columna >= tamTablero)
                {
                    return false;
                }

                Coordenada nuevaPos = new Coordenada(fila, columna);

                if (ocupadas.Contains(nuevaPos) || columna > tamTablero || fila > tamTablero)
                {
                    return false;
                }
            }
            return true;
        }

       
        /// <summary>
        /// Este metodo llama al bucle el juego y se utiliza para iniciar el juego.
        /// </summary>
        public Game()
        {
            finPartida = false;

            GameLoop();
        }

        /// <summary>
        /// Este metodo será llamado cuando se quiera finalizar el juego o todos los barcos 
        /// ya hayan sido hundidos
        /// </summary>
        /// <param name="sender">objeto que envia el evento</param>
        /// <param name="e">argumentos del evento</param>
        private void cuandoEventoFinPartida(object sender, EventArgs e)
        {
            finPartida = true;
        }
    }
}
