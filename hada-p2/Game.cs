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
        private bool finPartida;

        /// <summary>
        /// Este metodo inicializa un tablero y unos barcos para que se le muestren al jugador
        /// y pueda desarrollar el juego, mostrando tambien los disparos y hundimientos.
        /// </summary>
        private void GameLoop()
        {
            // declaramos e inicializamos los barcos
            List<Barco> barcos = new List<Barco>
            {
                new Barco ("SANTA MARIA", 2, 'v', new Coordenada(2, 3)),
                new Barco ("LA NIÑA", 3, 'h', new Coordenada(6, 1)),
                new Barco ("LA PINTA", 1, 'v', new Coordenada(5, 5))
            };
            // declaramos el tablero
            Tablero tablero = new Tablero(9, barcos);
            int barcosTotales = barcos.Count;
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
