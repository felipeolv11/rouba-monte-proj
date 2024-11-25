using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
            int qtdJogadores = 0;

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

            Console.WriteLine("Deseja jogar novamente? (sim/nao)");
            string resp = Console.ReadLine();

            resp.ToLower();

            if (resp == "sim")
            {
                Iniciar();
            }

            else if (resp == "nao")
            {
                Console.WriteLine("GAME OVER!!!");
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
                if (monte_de_compra.Topo > 0)
                {
                    foreach (var jogador in jogadores)
                    {
                        RodadaJogador(jogador);
                    }
                }

                else
                {
                    Console.WriteLine("Acabaram as cartas do monte de compras.");
                    Console.WriteLine("Agora, vamos verificar que está com o maior monte...");

                    // ganhandor ou ganhadores, etc.

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
            Console.Clear();

            Carta cartaDaVez = monte_de_compra.Pop();
            string jogadorDaVez = jogador.Nome;

            foreach (var outroJogador in jogadores)
            {
                bool possui = false;

                int maiorQtdCartas = 0;

                Jogador jogadorMaiorQtdCartas1 = null;
                Jogador jogadorMaiorQtdCartas2 = null;

                if (cartaDaVez == outroJogador.Monte.Peek() && outroJogador.Nome != jogadorDaVez)
                {
                    possui = true;

                    if (maiorQtdCartas == outroJogador.QtdCartas)
                    {
                        jogadorMaiorQtdCartas2 = outroJogador;
                    }

                    else if (maiorQtdCartas < outroJogador.QtdCartas)
                    {
                        jogadorMaiorQtdCartas1 = outroJogador;

                    }
                }

                if (possui)
                {
                    Console.WriteLine("Vez do Jogador: {0}", jogador.Nome);
                    Console.WriteLine("Comprando carta do monte...");
                    Console.WriteLine("Carta da vez: " + cartaDaVez);

                    Console.WriteLine();
                    Console.WriteLine("Área de Descarte:");
                    MostrarAreaDescarte();

                    Console.WriteLine();
                    Console.WriteLine("Monte dos Jogadores:");
                    MostrarMontesJogadores();

                    Console.WriteLine();

                    if (jogadorMaiorQtdCartas1.QtdCartas == jogadorMaiorQtdCartas2.QtdCartas)
                    {
                        Console.WriteLine("A carta da vez é igual o monte de dois jogadores.");
                        Console.WriteLine("Logo, você tera que roubar um monte.");
                        Console.WriteLine("E ambos, possuem a mesma quantidade de cartas no monte,");
                        Console.WriteLine("Então, será decidido na sorte...");

                        Random random = new Random();
                        int sorte = random.Next(1, 10);

                        Jogador azarao = null;

                        if (sorte % 2 == 0)
                        {
                            azarao = jogadorMaiorQtdCartas1;
                        }

                        else
                        {
                            azarao = jogadorMaiorQtdCartas2;
                        }
                            
                        Console.WriteLine("O jogador que tera o monte roubado é: " + azarao.Nome);


                        foreach(var jogadorEscolhido in jogadores)
                        {
                            if (jogadorEscolhido.Nome == azarao.Nome && jogadorEscolhido.QtdCartas == azarao.QtdCartas)
                            {
                                RoubarMonte(jogador, jogadorEscolhido, cartaDaVez);

                                jogador.AtualizarQtdCartas();
                                jogadorEscolhido.AtualizarQtdCartas();
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("A carta da vez é igual o monte de um jogadores.");
                        Console.WriteLine("Logo, você tera que roubar um monte.");

                        Console.WriteLine("O jogador que tera o monte roubado é: " + jogadorMaiorQtdCartas1.Nome);

                        foreach (var jogadorEscolhido in jogadores)
                        {
                            if (jogadorEscolhido.Nome == jogadorMaiorQtdCartas1.Nome && jogadorEscolhido.QtdCartas == jogadorMaiorQtdCartas1.QtdCartas)
                            {
                                RoubarMonte(jogador, jogadorEscolhido, cartaDaVez);

                                jogador.AtualizarQtdCartas();
                                jogadorEscolhido.AtualizarQtdCartas();
                            }
                        }
                    }

                    Console.WriteLine("Monte roubado!");

                    Console.WriteLine();
                    Console.WriteLine("Monte dos Jogadores, pós roubo:");
                    MostrarMontesJogadores();

                    Console.WriteLine();
                    Console.WriteLine("Com isso, você compra mais uma carta.");
                    RodadaJogador(jogador);
                }
            }

            else if (area_de_descarte.ContainsValue(cartaDaVez.Valor))
            {
                Console.WriteLine("Vez do Jogador: {0}", jogador.Nome);
                Console.WriteLine("Comprando carta do monte...");
                Console.WriteLine("Carta da vez: " + cartaDaVez);

                Console.WriteLine();
                Console.WriteLine("Área de Descarte:");
                MostrarAreaDescarte();

                Console.WriteLine();
                Console.WriteLine("Monte dos Jogadores:");
                MostrarMontesJogadores();

                Console.WriteLine();
                Console.WriteLine("A carta da vez é diferente das cartas nos topos dos montes inimigos,");
                Console.WriteLine("Porém, é igual a da área de descarte,");
                Console.WriteLine("Logo, você a colocara no seu monte, juntamente com a carta da vez no topo.");

                Carta cartaDaArea = null;

                foreach (var carta in area_de_descarte)
                {
                    if (carta.Value == cartaDaVez.Valor)
                    {
                        cartaDaArea = new Carta(carta.Value, carta.Key.Nome);
                        area_de_descarte.Remove(carta.Key);

                        break;
                    }
                }

                jogador.Monte.Push(cartaDaArea);
                jogador.Monte.Push(cartaDaVez);
                jogador.AtualizarQtdCartas();

                Console.WriteLine();
                Console.WriteLine("Área de Descarte, pós retirada:");
                MostrarAreaDescarte();

                Console.WriteLine();
                Console.WriteLine("Com isso, você compra mais uma carta.");
                RodadaJogador(jogador);
            }

            else if (cartaDaVez.Valor == jogador.Monte.Peek().Valor)
            {
                Console.WriteLine("Vez do Jogador: {0}", jogador.Nome);
                Console.WriteLine("Comprando carta do monte...");
                Console.WriteLine("Carta da vez: " + cartaDaVez);

                Console.WriteLine();
                Console.WriteLine("Monte dos Jogadores:");
                MostrarMontesJogadores();

                Console.WriteLine();
                Console.WriteLine("Área de Descarte:");
                MostrarAreaDescarte();

                Console.WriteLine();
                Console.WriteLine("A carta da vez é diferente das cartas nos topos dos montes inimigos,");
                Console.WriteLine("Porém, é igual a do topo do seu  própio monte,");
                Console.WriteLine("Logo, você a colocara no topo do seu monte:");

                jogador.Monte.Push(cartaDaVez);
                jogador.AtualizarQtdCartas();

                Console.WriteLine();
                Console.WriteLine("Monte dos Jogadores, pós inserção:");
                MostrarMontesJogadores();

                Console.WriteLine();
                Console.WriteLine("Com isso, você compra mais uma carta.");
                RodadaJogador(jogador);
            }

            else
            {
                Console.WriteLine("Vez do Jogador: {0}", jogador.Nome);
                Console.WriteLine("Comprando carta do monte...");
                Console.WriteLine("Carta da vez: " + cartaDaVez);

                Console.WriteLine();
                Console.WriteLine("Área de Descarte:");
                MostrarAreaDescarte();

                Console.WriteLine();
                Console.WriteLine("Monte dos Jogadores:");
                MostrarMontesJogadores();

                Console.WriteLine();
                Console.WriteLine("A carta da vez é diferente das cartas nos topos dos montes,");
                Console.WriteLine("E é diferente das cartas na área de descarte,");
                Console.WriteLine("Logo, você a descartará na área de descarte:");
                
                area_de_descarte.Add(cartaDaVez, cartaDaVez.Valor);

                Console.WriteLine();
                Console.WriteLine("Área de Descarte, pós inserção:");
                MostrarAreaDescarte();
            }

            Console.WriteLine();
            Console.WriteLine("Jogada encerrada, próximo jogador!");
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
                Console.WriteLine("Jogador: {0}", jogador.Nome);

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

        public void RoubarMonte(Jogador jogador1, Jogador jogador2, Carta cartaDaVez)
        {
            Stack<Carta> cartas = new Stack<Carta>();

            foreach (var carta in jogador2.Monte)
            {
                cartas.Push(jogador2.Monte.Pop());
            }

            foreach (var carta in cartas)
            {
                jogador1.Monte.Push(cartas.Pop());
            }

            jogador1.Monte.Push(cartaDaVez);
        }

        public void Ranking() { }
    }

    public class Jogador
    {
        private string nome;
        private int qtdCartas;
        private int pontuacao;

        private Stack<Carta> monte = new Stack<Carta>();

        public Jogador(string nome)
        {
            this.nome = nome;
            this.qtdCartas = 0;
            this.pontuacao = 0;
        }

        public void AtualizarQtdCartas()
        {
            qtdCartas = Monte.Count;
        }

        public string Nome {  get { return nome; } set { nome = value; } }
        public int QtdCartas { get { return qtdCartas; } set { qtdCartas = value; } }
        public int Pontuacao { get { return pontuacao; } set { pontuacao = value; } }

        public Stack<Carta> Monte { get { return monte; } set { monte = value; } }
    }

    public class Carta
    {
        private int valor;
        private string nome;

        public Carta(int valor, string nome)
        {
            this.valor = valor;
            this.nome = nome;
        }

        public int Valor { get { return valor; } set { valor = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
    }

    class Pilha
    {
        private Carta[] array;
        private int topo;

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

        public Carta[] Array { get { return array; } set { array = value; } }
        public int Topo { get { return topo; } set { topo = value; } }
    }
}