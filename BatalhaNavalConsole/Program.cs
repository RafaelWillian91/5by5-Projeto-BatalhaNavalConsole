using System.Security.Cryptography;

Console.WriteLine("******** Jogo Batalha Naval ********");

const string AGUA = "A";
const string CRUZADOR = "CZ";
const string SUBMARINO = "SB";
const string CONTRATORPEDEIRO = "CP";

Console.Write("Informe a quantidade de linhas e colunas do tabuleiro: ");
int dimensaoTabuleiro = int.Parse(Console.ReadLine());

string[,] tabuleiroJogador = new string[dimensaoTabuleiro, dimensaoTabuleiro];
string[,] tabuleiroComputador = new string[dimensaoTabuleiro, dimensaoTabuleiro];

IniciarPartida();

// Inicializar os tabuleiros
void MontarTabuleiroJogadores()
{
    for (int i = 0; i < dimensaoTabuleiro; i++)
    {
        for (int j = 0; j < dimensaoTabuleiro; j++)
        {
            tabuleiroJogador[i, j] = AGUA;
            tabuleiroComputador[i, j] = AGUA;
        }
    }
}

// Definir os navios que serão posicionados
//Decomposição funcional: divido um problema grande em funções menores
//Orquestração: um método coordena a execução de outros
void PosicionarPecasJogadores()
{
    PosicionarNavio("Contratorpedeiro", 1);
    PosicionarNavioComputador("Contratorpedeiro", 1);

    PosicionarNavio("Submarino", 2);
    PosicionarNavioComputador("Submarino", 2);

    PosicionarNavio("Cruzador", 3);
    PosicionarNavioComputador("Cruzador", 3);
}

// Posicionar qualquer tipo de navio
void PosicionarNavio(string nomeNavio, int tamanhoNavio)
{
    Console.WriteLine($"\nPosicione seu {nomeNavio} ({tamanhoNavio} posições).");

    //identifica qual tipo de navio é
    if (tamanhoNavio == 1)
    {
        Console.WriteLine("Informe a linha e a coluna. Exemplo: 3,4");
    }
    else
    {
        Console.WriteLine("Informe coordenadas consecutivas na horizontal ou vertical.");
        Console.WriteLine("Exemplo: 3,4 e 3,5");
    }

    int coordenadaAnteriorX = 0;
    int coordenadaAnteriorY = 0;

    // Controla quantas posições o navio ocupa
    for (int i = 0; i < tamanhoNavio; i++)
    {
        bool jogadaValida;
        int novaX;
        int novaY;

        do
        {
            Console.Write($"Informe a {i + 1}º posição do {nomeNavio}: ");

            string[] coordenadas = Console.ReadLine().Split(',');

            novaX = int.Parse(coordenadas[0]);
            novaY = int.Parse(coordenadas[1]);

            jogadaValida = true;

            // Validar limites do tabuleiro (borda)
            if (novaX < 0 || novaX >= dimensaoTabuleiro ||
                novaY < 0 || novaY >= dimensaoTabuleiro)
            {
                Console.WriteLine("Posição fora dos limites do tabuleiro!");
                jogadaValida = false;
            }
            // Verificar se a posição está ocupada
            else if (tabuleiroJogador[novaX, novaY] != AGUA)
            {
                Console.WriteLine("Essa posição já está ocupada!");
                jogadaValida = false;
            }
            // Validar se a posição é consecutiva apos uma jogada pelo menos
            else if (i > 0)
            {
                jogadaValida = ValidarJogada(
                    coordenadaAnteriorX,
                    coordenadaAnteriorY,
                    novaX,
                    novaY
                );

                if (!jogadaValida)
                {
                    Console.WriteLine("Posição inválida! Escolha uma coordenada consecutiva.");
                }
            }

        } while (!jogadaValida);

        // Registrar a posição no tabuleiro
        tabuleiroJogador[novaX, novaY] = nomeNavio;

        // Guardar a coordenada para comparação com a próxima jogada
        coordenadaAnteriorX = novaX;
        coordenadaAnteriorY = novaY;
    }
}

void PosicionarNavioComputador(string nomeNavio, int tamanhoNavio)
{
    bool jogadaValida = false;

    do
    {
        // Sorteia a primeira posição do navio
        int linha = Random.Shared.Next(0, dimensaoTabuleiro);
        int coluna = Random.Shared.Next(0, dimensaoTabuleiro);

        // 0 = horizontal | 1 = vertical
        int direcao = Random.Shared.Next(0, 2);

        jogadaValida = true;

        // Verifica se todas as posições estão disponíveis
        for (int i = 0; i < tamanhoNavio; i++)
        {
            int novaX = linha;
            int novaY = coluna;

            if (direcao == 0)
            {
                novaY = coluna + i;
            }
            else
            {
                novaX = linha + i;
            }

            // Verifica os limites do tabuleiro
            if (novaX >= dimensaoTabuleiro || novaY >= dimensaoTabuleiro)
            {
                jogadaValida = false;
                break;
            }

            // Verifica se a posição já está ocupada
            if (tabuleiroComputador[novaX, novaY] != AGUA)
            {
                jogadaValida = false;
                break;
            }
        }

        // Se todas as posições forem válidas, posiciona o navio
        if (jogadaValida)
        {
            for (int i = 0; i < tamanhoNavio; i++)
            {
                if (direcao == 0)
                {
                    tabuleiroComputador[linha, coluna + i] = nomeNavio;
                }
                else
                {
                    tabuleiroComputador[linha + i, coluna] = nomeNavio;
                }
            }
        }

    } while (!jogadaValida);
}

// Verificar se as coordenadas são consecutivas
bool ValidarJogada(
    int coordenadaAnteriorX,
    int coordenadaAnteriorY,
    int coordenadaNovaX,
    int coordenadaNovaY)
{
    bool verificarHorizontal =
        coordenadaAnteriorX == coordenadaNovaX &&
        Math.Abs(coordenadaAnteriorY - coordenadaNovaY) == 1;

    bool verificarVertical =
        Math.Abs(coordenadaAnteriorX - coordenadaNovaX) == 1 &&
        coordenadaAnteriorY == coordenadaNovaY;

    return verificarHorizontal || verificarVertical;
}

// Exibir tabuleiro do jogador
void MostrarTabuleiroJogador()
{
    Console.WriteLine("\n******** Tabuleiro do Jogador ********");

    for (int i = 0; i < dimensaoTabuleiro; i++)
    {
        for (int j = 0; j < dimensaoTabuleiro; j++)
        {
            Console.Write(tabuleiroJogador[i, j] + "\t");
        }

        Console.WriteLine();
    }
}

void MostrarTabuleiroComputador()
{
    Console.WriteLine("\n******** Tabuleiro do COMPUTADOR ********");

    for (int i = 0; i < dimensaoTabuleiro; i++)
    {
        for (int j = 0; j < dimensaoTabuleiro; j++)
        {
            Console.Write(tabuleiroComputador[i, j] + "\t");
        }

        Console.WriteLine();
    }
}


void IniciarPartida()
{
    // Execução principal
    MontarTabuleiroJogadores();
    PosicionarPecasJogadores();
    MostrarTabuleiroJogador();
    MostrarTabuleiroComputador();
}

