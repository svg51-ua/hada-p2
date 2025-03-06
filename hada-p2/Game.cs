using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hada
{
    public class Game
    {
        private bool finPartida;
        private void GameLoop()
        {

            List<Barco> barcos = new List<Barco>
            {
                new Barco ("SANTA MARIA", 2, 'v', new Coordenada(2, 3)),
                new Barco ("LA NIÑA", 3, 'h', new Coordenada(6, 1)),
                new Barco ("lA PINTA", 1, 'v', new Coordenada(5, 5))
            };
            Tablero tablero = new Tablero(9, barcos); // esto hay que revisarlo
            while (!finPartida)
            {

                Console.Clear();
                Console.WriteLine(tablero.ToString());
                Console.Write("Introduce la cordenada a la que disparar FILA,COLUMNA ('S' para Salir): ");
                string entrada = Console.ReadLine();
                if (entrada.ToLower() == "s")
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
        public Game()
        {
            finPartida = false;

            GameLoop();
        }
        private void cuandoEventoFinPartida(object sender, EventArgs e)
        {
            Console.WriteLine("PARTIDA FINALIZADA!!\n");
            finPartida = true;
        }
    }
}
