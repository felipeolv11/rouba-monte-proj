using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rouba_monte
{
    class Program
    {
        static void Main(string[] args)
        {
            Jogo rouba_monte = new Jogo();
            rouba_monte.Iniciar();

            Console.ReadLine();
        }
    }

    public class Jogo
    {
        private Jogador[] jogadores;
        private Pilha monte_de_compra = new Pilha();
        private Dictionary<Carta, int> area_de_descarte = new Dictionary<Carta, int>();

        public void Iniciar()
        {
            int qtdJogadores;

            do
            {
                Console.WriteLine("Quantos jogadores irão participar? (2-4)");
                qtdJogadores = int.Parse(Console.ReadLine());

                if (qtdJogadores < 2 || qtdJogadores > 4)
                {
                    throw new Exception("Opção inválida! Tente novamente.");
                }

            } while (qtdJogadores < 2 || qtdJogadores > 4);

            jogadores = new Jogador[qtdJogadores];

            for (int i = 0; i < qtdJogadores; i++)
            {
                Console.Write($"Nome do jogador n{i + 1}: ");
                string nome = Console.ReadLine();
                jogadores[i] = new Jogador(nome);
            }

            Embaralhar();
            Jogar();
            Ranking();

            Console.WriteLine("Deseja jogar uma nova partida? (sim/nao)");
            string resp = Console.ReadLine();

            if (resp == "sim" || resp == "SIM")
            {
                Iniciar();
            }

            else if (resp == "nao" || resp == "nao")
            {
                Console.WriteLine("FIM!!!");
            }

            else
            {
                Console.WriteLine("Resposta inválida!");
            }
        }

        public void Jogar()
        {
            Console.WriteLine("O jogo começou, que vença o melhor!");

            bool jogoAtivo = true;

            while (jogoAtivo)
            {
                if (monte_de_compra.topo > 0)
                {
                    foreach (var jogador in jogadores)
                    {
                        RodadaJogador(jogador);
                    }
                }

                else
                {
                    Console.WriteLine("Acabaram as cartas do monte de compras.");
                    Console.WriteLine("Agora, vamos ver quem está com o maior monte...");

                    // verificar se tem empate, senao mostrar vencedor.

                    jogoAtivo = false;
                }

                Console.WriteLine();
            }
        }

        public void Embaralhar()
        {
            List<Carta> baralho = new List<Carta>();

            string[] naipes = { "Copas", "Espadas", "Ouros", "Paus" };

            foreach (var naipe in naipes)
            {
                for (int i = 1; i <= 13; i++)
                {
                    string nomeCarta;

                    switch (i)
                    {
                        case 1:
                            nomeCarta = "Ás";
                            break;

                        case 11:
                            nomeCarta = "Valete";
                            break;

                        case 12:
                            nomeCarta = "Dama";
                            break;

                        case 13:
                            nomeCarta = "Rei";
                            break;

                        default:
                            nomeCarta = i.ToString();
                            break;
                    }

                    baralho.Add(new Carta(i, $"{nomeCarta} de {naipe}"));
                }
            }

            Random rand = new Random();
            int n = baralho.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);

                Carta temp = baralho[i];
                baralho[i] = baralho[j];
                baralho[j] = temp;
            }

            foreach (var carta in baralho)
            {
                monte_de_compra.Push(carta);
            }

            Console.WriteLine("Embaralhando cartas...");
            Console.WriteLine("Pronto!");
        }

        public void RodadaJogador(Jogador jogador)
        {
            /* Console.WriteLine("Vez do Jogador: {0}", jogador.nome);

            jogador.Monte.Push(monte_de_compra.Pop());
            jogador.AtualizarQtdCartas();

            Console.WriteLine("Comprando carta do monte...");
            Console.WriteLine("Carta da vez: {0}", jogador.Monte.Peek());


            Console.WriteLine();
            Console.WriteLine("Área de Descarte");
            MostrarAreaDescarte();

            Console.WriteLine();
            Console.WriteLine("Topo do monte dos jogadores");
            MostrarMontesJogadores(); */
        }

        public void MostrarAreaDescarte()
        {
            if (area_de_descarte.Count > 0)
            {
                Console.Write("[");
                foreach (var carta in area_de_descarte)
                {
                    Console.Write(carta.Key + " ");
                }
                Console.WriteLine("]");
            }

            else
            {
                Console.WriteLine("[ VAZIO ]");
            }
        }

        public void MostrarMontesJogadores()
        {
            foreach (var jogador in jogadores)
            {
                Console.WriteLine("Jogador: {0}", jogador.nome);

                if (jogador.Monte.Count > 0)
                {
                    Console.WriteLine("Topo do monte: {0}", jogador.Monte.Peek());
                }

                else
                {
                    Console.WriteLine("Topo do monte: VAZIO");
                }
            }
        }

        public void Ranking() { }
    }

    public class Jogador
    {
        public string nome { get; set; }
        private int qtdCartas { get; set; }
        private int pontuacao { get; set; }
        private int pontosRank { get; set; }

        public Stack<Carta> Monte { get; set; } = new Stack<Carta>();

        public Jogador(string nome)
        {
            this.nome = nome;
            this.qtdCartas = 0;
            this.pontuacao = 0;
            this.pontosRank = 0;
        }

        public void AtualizarQtdCartas()
        {
            qtdCartas = Monte.Count;
        }
    }

    public class Carta
    {
        private int valor { get; set; }
        private string nome { get; set; }

        public Carta(int valor, string nome)
        {
            this.valor = valor;
            this.nome = nome;
        }
    }

    class Pilha
    {
        private Carta[] array { get; set; }
        public int topo { get; set; }

        public Pilha()
        {
            inicializar(52);
        }

        public Pilha(int tamanho)
        {
            inicializar(tamanho);
        }

        private void inicializar(int tamanho)
        {
            this.array = new Carta[tamanho];
            this.topo = 0;
        }

        public void Push(Carta carta)
        {
            if (topo >= array.Length)
                throw new Exception("Erro! Pilha cheia.");

            array[topo] = carta;
            topo++;
        }

        public Carta Pop()
        {
            if (topo == 0)
                throw new Exception("Erro! Pilha vazia.");

            topo = topo - 1;

            return array[topo];
        }
    }
}