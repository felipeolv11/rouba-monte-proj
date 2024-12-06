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
            Console.WriteLine(".---.             .-.            .-..-.             .-.      \r\n: .; :            : :            : `' :            .' `.     \r\n:   .' .--. .-..-.: `-.  .--.    : .. : .--. ,-.,-.`. .'.--. \r\n: :.`.' .; :: :; :' .; :' .; ;   : :; :' .; :: ,. : : :' '_.'\r\n:_;:_;`.__.'`.__.'`.__.'`.__,_;  :_;:_;`.__.':_;:_; :_;`.__.'\r\n");

            Reiniciar();

            int qtdJogadores = 0;
            int qtdBaralhos = 0;

            do
            {
                Console.WriteLine("> Quantos jogadores irão participar? (2-4)");
                qtdJogadores = int.Parse(Console.ReadLine());

                if (qtdJogadores < 2 || qtdJogadores > 4)
                {
                    Console.WriteLine("Opção inválida! Tente novamente.");
                }

            } while (qtdJogadores < 2 || qtdJogadores > 4); ;

            do
            {
                Console.WriteLine("\n> Quantos baralhos vão ser utilizados? (1-4)");
                qtdBaralhos = int.Parse(Console.ReadLine());

                if (qtdBaralhos < 1 || qtdBaralhos > 4)
                {
                    Console.WriteLine("Opção inválida! Tente novamente.");
                }

            } while (qtdBaralhos < 1 || qtdBaralhos > 4);

            jogadores = new Jogador[qtdJogadores];
            monte_de_compra = new Pilha(52 * qtdBaralhos);

            Console.WriteLine();
            for (int i = 0; i < qtdJogadores; i++)
            {
                Console.Write($"Nome do jogador n{i + 1}: ");
                string nome = Console.ReadLine();

                jogadores[i] = new Jogador(nome);
            }

            Embaralhar(qtdBaralhos);
            Jogar();
        }

        private void Reiniciar()
        {
            monte_de_compra = new Pilha();
            area_de_descarte = new Dictionary<Carta, int>();
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
                    Console.WriteLine("\nO monte de compra acabou! Fim do jogo.");
                    Console.ReadLine();
                    jogoAtivo = false;
                    break;
                }

                foreach (var jogador in jogadores)
                {
                    RodadaJogador(jogador);

                    if (monte_de_compra.Topo == 0)
                    {
                        Console.WriteLine("\nO monte de compra acabou! Fim do jogo.");
                        Console.ReadLine();
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

            List<Jogador> vencedores = new List<Jogador>();
            int maiorQuantidade = jogadores[0].QtdCartas;
            foreach (var jogador in jogadores)
            {
                if (jogador.QtdCartas == maiorQuantidade)
                {
                    vencedores.Add(jogador);
                }
            }

            Console.Clear();
            Console.WriteLine(".---.             .-.            .-..-.             .-.      \r\n: .; :            : :            : `' :            .' `.     \r\n:   .' .--. .-..-.: `-.  .--.    : .. : .--. ,-.,-.`. .'.--. \r\n: :.`.' .; :: :; :' .; :' .; ;   : :; :' .; :: ,. : : :' '_.'\r\n:_;:_;`.__.'`.__.'`.__.'`.__,_;  :_;:_;`.__.':_;:_; :_;`.__.'\r\n");

            if (vencedores.Count == 1)
            {
                Console.WriteLine($"\nO grande vencedor é: {vencedores[0].Nome}, com {vencedores[0].QtdCartas} cartas!");
            }

            else
            {
                Console.WriteLine("Os grandes vencedores são:");
                foreach (var vencedor in vencedores)
                {
                    Console.WriteLine($"{vencedor.Nome}, com {vencedor.QtdCartas} cartas!");
                }
            }

            Console.ReadLine();
            Console.WriteLine("Resultado da Partida:");
            for (int i = 0; i < jogadores.Count(); i++)
            {
                var jogador = jogadores[i];
                jogador.AtualizarPosicao(i + 1);
                Console.WriteLine("Posição: {0}° | Jogador: {1} | Cartas no Monte: {2}", i + 1, jogador.Nome, jogador.QtdCartas);
            }

            Console.ReadLine();
            string nomeJogador = "";
            do
            {
                Console.WriteLine("> Deseja ver o histórico de posições de algum jogador? (Digite 'nao' para pular)");
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

            Console.WriteLine("\nDeseja jogar novamente? (sim/nao)");
            string resp = Console.ReadLine();

            if (resp.ToLower() == "sim")
            {
                Console.Clear();
                Iniciar();
            }

            else if (resp.ToLower() == "nao")
            {
                Console.Write("\nFIM DE JOGO!!!");
            }

            else
            {
                Console.WriteLine("Resposta inválida! Tente novamente.");
            }
        }

        public void RodadaJogador(Jogador jogador)
        {
            while (true)
            {
                if (monte_de_compra.Topo == 0)
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine(".---.             .-.            .-..-.             .-.      \r\n: .; :            : :            : `' :            .' `.     \r\n:   .' .--. .-..-.: `-.  .--.    : .. : .--. ,-.,-.`. .'.--. \r\n: :.`.' .; :: :; :' .; :' .; ;   : :; :' .; :: ,. : : :' '_.'\r\n:_;:_;`.__.'`.__.'`.__.'`.__,_;  :_;:_;`.__.':_;:_; :_;`.__.'\r\n");
                Console.WriteLine("-------------------------------------------------------------");


                Console.WriteLine("\nJogador da Vez: {0}", jogador.Nome);

                Carta cartaDaVez = monte_de_compra.Pop();
                Console.WriteLine("Carta da Vez: " + cartaDaVez);

                Console.WriteLine("\nMonte de Compra (Cartas restantes: {0})", monte_de_compra.Topo);

                Carta propioTopoDoMonte = null;
                if (jogador.Monte.Count > 0)
                {
                    propioTopoDoMonte = jogador.Monte.Peek();
                }

                Console.WriteLine("\n-------------------------------------------------------------");
                Console.WriteLine("\nMonte dos Jogadores:");
                MostrarMontesJogadores();

                Console.WriteLine("\nÁrea de Descarte:");
                MostrarAreaDescarte();

                if (VerificarMontesAdversarios(jogador, cartaDaVez))
                {
                    Console.ReadLine();
                    Console.Write("Com isso o jogador pode comprar mais uma carta!");

                    MostrarEstadoAtual();
                    Console.ReadLine();
                    continue;
                }

                if (VerificarAreaDeDescarte(jogador, cartaDaVez))
                {
                    Console.ReadLine();
                    Console.Write("Com isso o jogador pode comprar mais uma carta!");

                    MostrarEstadoAtual();
                    Console.ReadLine();
                    continue;
                }

                if (VerificarPropioMonte(jogador, cartaDaVez, propioTopoDoMonte))
                {
                    Console.ReadLine();
                    Console.Write("Com isso o jogador pode comprar mais uma carta!");

                    MostrarEstadoAtual();
                    Console.ReadLine();
                    continue;
                }

                AdicionarCartaDescarte(cartaDaVez);
                Console.ReadLine();

                Console.Write("O jogador passou a vez!");
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
                Console.WriteLine("Resumo:");
                Console.Write("Você roubou o monte do jogador {0}.", jogadorAlvo.Nome);

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
                Console.WriteLine("Resumo:");
                Console.Write("Você retirou uma carta da área de descarte!");

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
                Console.WriteLine("Resumo:");
                Console.Write("A carta da vez foi adicionada ao seu próprio monte.");

                return true;
            }

            return false;
        }
        public void AdicionarCartaDescarte(Carta cartaDaVez)
        {
            area_de_descarte.Add(cartaDaVez, cartaDaVez.Numero);

            Console.ReadLine();
            Console.WriteLine("Resumo:");
            Console.Write("A carta da vez foi descartada.");
        }

        private void MostrarEstadoAtual()
        {
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine(".---.             .-.            .-..-.             .-.      \r\n: .; :            : :            : `' :            .' `.     \r\n:   .' .--. .-..-.: `-.  .--.    : .. : .--. ,-.,-.`. .'.--. \r\n: :.`.' .; :: :; :' .; :' .; ;   : :; :' .; :: ,. : : :' '_.'\r\n:_;:_;`.__.'`.__.'`.__.'`.__,_;  :_;:_;`.__.':_;:_; :_;`.__.'\r\n");
            Console.WriteLine("-------------------------------------------------------------");

            Console.WriteLine("\nEstado Atual do Jogo:");
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("\nMonte dos Jogadores:");
            MostrarMontesJogadores();
            Console.WriteLine("\nÁrea de Descarte:");
            MostrarAreaDescarte();
            Console.WriteLine();

            Console.Write("aperte ENTER para continuar...");
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

        public void MostrarAreaDescarte()
        {
            Console.WriteLine();
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
            Console.WriteLine("\n-------------------------------------------------------------");
        }

        public void MostrarMontesJogadores()
        {
            Console.WriteLine();
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

                Console.WriteLine();
            }
            Console.WriteLine("-------------------------------------------------------------");
        }
    }

    public class Jogador
    {
        private string nome;
        private int qtdCartas;
        private int posicao;
        private Fila historicoPosicoes = new Fila();
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
            this.posicao = posicao;

            historicoPosicoes.Inserir(posicao);

            if (historicoPosicoes.Count() > 5)
            {
                historicoPosicoes.Remover();
            }
        }

        public void ExibirHistoricoPosicoes()
        {
            Console.WriteLine($"\nHistórico de {nome}:");

            var atual = historicoPosicoes.Primeiro.Prox;
            if (atual == null)
            {
                Console.WriteLine("Sem histórico de posições.");
                return;
            }

            while (atual != null)
            {
                Console.WriteLine($"- {atual.Elemento}° lugar");
                atual = atual.Prox;
            }
        }

        public string Nome { get { return nome; } set { nome = value; } }
        public int QtdCartas { get { return qtdCartas; } set { qtdCartas = value; } }
        public int Posicao { get { return posicao; } set { posicao = value; } }
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

    public class Pilha
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
                throw new Exception("Erro!");

            array[topo] = carta;
            topo++;
        }

        public Carta Pop()
        {
            if (topo == 0)
                throw new Exception("Erro!");

            topo = topo - 1;
            return array[topo];
        }

        public Carta[] Array { get { return array; } set { array = value; } }
        public int Topo { get { return topo; } set { topo = value; } }
    }

    public class Fila
    {
        private Celula primeiro, ultimo;

        public Celula Primeiro
        {
            get { return primeiro; }
            set { primeiro = value; }
        }

        public Celula Ultimo
        {
            get { return ultimo; }
            set { ultimo = value; }
        }

        public Fila()
        {
            primeiro = new Celula();
            ultimo = primeiro;
        }

        public void Inserir(int x)
        {
            ultimo.Prox = new Celula(x);
            ultimo = ultimo.Prox;
        }

        public int Remover()
        {
            if (primeiro == ultimo)
                throw new Exception("Erro!");

            Celula tmp = primeiro;
            primeiro = primeiro.Prox;
            int elemento = primeiro.Elemento;
            tmp.Prox = null;
            tmp = null;

            return elemento;
        }

        public int Count()
        {
            int contador = 0;
            var atual = Primeiro.Prox;
            while (atual != null)
            {
                contador++;
                atual = atual.Prox;
            }

            return contador;
        }
    }

    public class Celula
    {
        private int elemento;
        private Celula prox;

        public Celula()
        {
            this.elemento = 0;
            this.prox = null;
        }

        public Celula(int elemento)
        {
            this.elemento = elemento;
            this.prox = null;
        }

        public int Elemento
        {
            get { return this.elemento; }
            set { this.elemento = value; }
        }

        public Celula Prox
        {
            get { return this.prox; }
            set { this.prox = value; }
        }
    }
}