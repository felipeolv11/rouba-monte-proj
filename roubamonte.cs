using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roubamonteproj
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
            int qtdBaralhos = 0;

            do
            {
                Console.WriteLine("Quantos jogadores irão participar? (2-4)");
                qtdJogadores = int.Parse(Console.ReadLine());

                if (qtdJogadores < 2 || qtdJogadores > 4)
                {
                    Console.WriteLine("Opção inválida! Tente novamente.");
                }

            } while (qtdJogadores < 2 || qtdJogadores > 4);

            do
            {
                Console.WriteLine("Quantos baralhos vão ser utilizados? (1-4)");
                qtdBaralhos = int.Parse(Console.ReadLine());

                if (qtdBaralhos < 1 || qtdBaralhos > 4)
                {
                    Console.WriteLine("Opção inválida! Tente novamente.");
                }

            } while (qtdBaralhos < 1 || qtdBaralhos > 4);

            jogadores = new Jogador[qtdJogadores];
            monte_de_compra = new Pilha(52 * qtdBaralhos);

            for (int i = 0; i < qtdJogadores; i++)
            {
                Console.Write($"Nome do jogador n{i + 1}: ");
                string nome = Console.ReadLine();
                jogadores[i] = new Jogador(nome);
            }

            Embaralhar(qtdBaralhos);
            Jogar();
        }

        public void Embaralhar(int qtdBaralhos)
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

                        case 2:
                            nomeCarta = "Dois";
                            break;

                        case 3:
                            nomeCarta = "Três";
                            break;

                        case 4:
                            nomeCarta = "Quatro";
                            break;

                        case 5:
                            nomeCarta = "Cinco";
                            break;

                        case 6:
                            nomeCarta = "Seis";
                            break;

                        case 7:
                            nomeCarta = "Sete";
                            break;

                        case 8:
                            nomeCarta = "Oito";
                            break;

                        case 9:
                            nomeCarta = "Nove";
                            break;

                        case 10:
                            nomeCarta = "Dez";
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

            List<Carta> baralhosMultiplicados = new List<Carta>();

            for (int i = 0; i < qtdBaralhos; i++)
            {
                foreach (var carta in baralho)
                {
                    baralhosMultiplicados.Add(carta);
                }
            }

            Random random = new Random();
            int n = baralhosMultiplicados.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Carta temp = baralhosMultiplicados[i];
                baralhosMultiplicados[i] = baralhosMultiplicados[j];
                baralhosMultiplicados[j] = temp;
            }

            foreach (var carta in baralhosMultiplicados)
            {
                monte_de_compra.Push(carta);
            }
        }

        public void Jogar()
        {
            bool jogoAtivo = true;

            while (jogoAtivo)
            {
                if (monte_de_compra.Topo == 0)
                {
                    Console.WriteLine("O monte de compra acabou! Fim do jogo.");
                    jogoAtivo = false;
                    break;
                }

                foreach (var jogador in jogadores)
                {
                    RodadaJogador(jogador);

                    if (monte_de_compra.Topo == 0)
                    {
                        Console.WriteLine("O monte de compra acabou! Fim do jogo.");
                        jogoAtivo = false;
                        break;
                    }
                }
            }

            FinalizarPartida();
        }

        public void FinalizarPartida()
        {
            for (int i = 0; i < jogadores.Count() - 1; i++)
            {
                for (int j = i + 1; j < jogadores.Count(); j++)
                {
                    if (jogadores[i].QtdCartas < jogadores[j].QtdCartas)
                    {
                        var temp = jogadores[i];
                        jogadores[i] = jogadores[j];
                        jogadores[j] = temp;
                    }
                }
            }

            Console.WriteLine("\nResultado da Partida:");
            for (int i = 0; i < jogadores.Count(); i++)
            {
                var jogador = jogadores[i];
                jogador.AtualizarPosicao(i + 1);
                Console.WriteLine($"Posição: {i + 1}° | Jogador: {jogador.Nome} | Cartas no Monte: {jogador.QtdCartas}");
            }

            string nomeJogador = "";
            do
            {
                Console.WriteLine("\nDeseja ver o histórico de posições de algum jogador? (Digite 'nao' caso não desejar.)");
                nomeJogador = Console.ReadLine();

                if (nomeJogador.ToLower() != "nao")
                {
                    bool jogadorEncontrado = false;

                    foreach (var jogador in jogadores)
                    {
                        if (jogador.Nome.ToLower() == nomeJogador.ToLower())
                        {
                            jogador.ExibirHistoricoPosicoes();
                            jogadorEncontrado = true;
                            break;
                        }
                    }

                    if (!jogadorEncontrado)
                    {
                        Console.WriteLine("Jogador não encontrado. Tente novamente.");
                    }
                }

            } while (nomeJogador.ToLower() != "nao");

            Console.WriteLine("Deseja jogar novamente? (sim/nao)");
            string resp = Console.ReadLine();

            if (resp.ToLower() == "sim")
            {
                Console.Clear();
                Iniciar();
            }
                
            else if (resp.ToLower() == "nao")
                Console.WriteLine("\nGAME OVER!!!");

                
            else
                Console.WriteLine("Resposta inválida! Tente novamente.");
        }

        public void RodadaJogador(Jogador jogador)
        {
            Console.Clear();
            Console.WriteLine("Jogador da Vez: {0}", jogador.Nome);

            while (true)
            {
                Carta cartaDaVez = monte_de_compra.Pop();
                Console.WriteLine("Carta da Vez: " + cartaDaVez);

                Carta propioTopoDoMonte = null;
                if (jogador.Monte.Count > 0)
                {
                    propioTopoDoMonte = jogador.Monte.Peek();
                }

                Console.WriteLine("\nMonte dos Jogadores:");
                MostrarMontesJogadores();

                Console.WriteLine("\nÁrea de Descarte:");
                MostrarAreaDescarte();

                if (VerificarMontesAdversarios(jogador, cartaDaVez))
                {
                    MostrarEstadoAtual();
                    Console.ReadLine();
                    break;
                }

                if (VerificarAreaDeDescarte(jogador, cartaDaVez))
                {
                    MostrarEstadoAtual();
                    Console.ReadLine();
                    break;
                }

                if (VerificarPropioMonte(jogador, cartaDaVez, propioTopoDoMonte))
                {
                    MostrarEstadoAtual();
                    Console.ReadLine();
                    break;
                }

                AdicionarCartaDescarte(cartaDaVez);
                Console.ReadLine();
                break;
            }
        }

        public bool VerificarMontesAdversarios(Jogador jogador, Carta cartaDaVez)
        {
            List<Jogador> candidatos = new List<Jogador>();
            int maiorTamanho = 0;

            foreach (var outroJogador in jogadores)
            {
                if (outroJogador.Nome != jogador.Nome)
                {
                    Carta topoDoMonte = null;
                    if (outroJogador.Monte.Count > 0)
                    {
                        topoDoMonte = outroJogador.Monte.Peek();
                    }

                    if (topoDoMonte != null && cartaDaVez.Numero == topoDoMonte.Numero)
                    {
                        if (outroJogador.Monte.Count > maiorTamanho)
                        {
                            candidatos.Clear();
                            maiorTamanho = outroJogador.Monte.Count;
                        }

                        if (outroJogador.Monte.Count == maiorTamanho)
                        {
                            candidatos.Add(outroJogador);
                        }
                    }
                }
            }

            if (candidatos.Count > 0)
            {
                Random random = new Random();

                int indiceAleatorio = random.Next(candidatos.Count);
                Jogador jogadorAlvo = candidatos[indiceAleatorio];

                Console.ReadLine();
                Console.WriteLine("\nResumo:");
                Console.WriteLine("Você roubou o monte do jogador '{0}'.", jogadorAlvo.Nome);

                RoubarMonte(jogador, jogadorAlvo, cartaDaVez);

                return true;
            }

            return false;
        }

        public bool VerificarAreaDeDescarte(Jogador jogador, Carta cartaDaVez)
        {
            if (area_de_descarte.ContainsValue(cartaDaVez.Numero))
            {
                Carta cartaDaArea = null;
                foreach (var carta in area_de_descarte)
                {
                    if (carta.Value == cartaDaVez.Numero)
                    {
                        cartaDaArea = new Carta(carta.Value, carta.Key.Naipe);
                        area_de_descarte.Remove(carta.Key);

                        break;
                    }
                }

                jogador.Monte.Push(cartaDaArea);
                jogador.Monte.Push(cartaDaVez);
                jogador.AtualizarQtdCartas();

                Console.ReadLine();
                Console.WriteLine("\nResumo:");
                Console.WriteLine("Você retirou uma carta da área de descarte!");

                return true;
            }

            return false;
        }

        public bool VerificarPropioMonte(Jogador jogador, Carta cartaDaVez, Carta propioTopoDoMonte)
        {
            if (propioTopoDoMonte != null && cartaDaVez.Numero == propioTopoDoMonte.Numero)
            {
                jogador.Monte.Push(cartaDaVez);
                jogador.AtualizarQtdCartas();

                Console.ReadLine();
                Console.WriteLine("\nResumo:");
                Console.WriteLine("A carta da vez foi adicionada ao seu próprio monte.");

                return true;
            }

            return false;
        }
        public void AdicionarCartaDescarte(Carta cartaDaVez)
        {
            area_de_descarte.Add(cartaDaVez, cartaDaVez.Numero);

            Console.ReadLine();
            Console.WriteLine("\nResumo:");
            Console.WriteLine("A carta da vez foi descartada.");
        }

        private void MostrarEstadoAtual()
        {
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Estado Atual do Jogo:");
            Console.WriteLine("\nMonte dos Jogadores:");
            MostrarMontesJogadores();
            Console.WriteLine("\nÁrea de Descarte:");
            MostrarAreaDescarte();
            Console.WriteLine();

            if (monte_de_compra.Topo != 0)
            {
                Console.WriteLine("Próxima Rodada!");
            }
        }

        public void MostrarAreaDescarte()
        {
            if (area_de_descarte.Count > 0)
            {
                foreach (var carta in area_de_descarte)
                {
                    Console.WriteLine("- " + carta.Key);
                }
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
                    Console.WriteLine("Topo do Monte: {0} (Tamanho: {1})", jogador.Monte.Peek(), jogador.QtdCartas);
                }

                else
                {
                    Console.WriteLine("Topo do Monte: [ VAZIO ]");
                }
            }
        }

        public void RoubarMonte(Jogador jogador1, Jogador jogador2, Carta cartaDaVez)
        {
            Stack<Carta> cartas = new Stack<Carta>();

            while (jogador2.Monte.Count > 0)
            {
                cartas.Push(jogador2.Monte.Pop());
            }

            while (cartas.Count > 0)
            {
                var carta = cartas.Pop();
                jogador1.Monte.Push(carta);
            }

            jogador1.Monte.Push(cartaDaVez);

            jogador1.AtualizarQtdCartas();
            jogador2.AtualizarQtdCartas();
        }
    }

    public class Jogador
    {
        private string nome;
        private int qtdCartas;
        private int posicao;
        private Queue<int> rank = new Queue<int>();
        private Stack<Carta> monte = new Stack<Carta>();

        public Jogador(string nome)
        {
            this.nome = nome;
            this.qtdCartas = 0;
            this.posicao = 0;
        }

        public void AtualizarQtdCartas()
        {
            qtdCartas = Monte.Count;
        }

        public void AtualizarPosicao(int posicao)
        {
            if (rank.Count == 5)
            {
                rank.Dequeue();
            }

            rank.Enqueue(posicao);
            this.posicao = posicao;
        }

        public void ExibirHistoricoPosicoes()
        {
            Console.WriteLine("Histórico de '{0}':", Nome);
            foreach (var pos in rank)
            {
                Console.WriteLine("Posição: {0}°", pos);
            }
        }

        public string Nome { get { return nome; } set { nome = value; } }
        public int QtdCartas { get { return qtdCartas; } set { qtdCartas = value; } }
        public int Posicao { get { return posicao; } set { posicao = value; } }
        public Queue<int> Rank { get { return rank; } set { rank = value; } }
        public Stack<Carta> Monte { get { return monte; } set { monte = value; } }
    }

    public class Carta
    {
        private int numero;
        private string naipe;

        public Carta(int numero, string naipe)
        {
            this.numero = numero;
            this.naipe = naipe;
        }

        public override string ToString()
        {
            return $"{Naipe}";
        }

        public int Numero { get { return numero; } set { numero = value; } }
        public string Naipe { get { return naipe; } set { naipe = value; } }
    }

    class Pilha
    {
        private Carta[] array;
        private int topo;

        public Pilha()
        {
            inicializar(0);
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
