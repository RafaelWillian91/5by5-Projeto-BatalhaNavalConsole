Console.WriteLine("******** Jogo Batalha Naval ********");

const string AGUA = "A";
const string CRUZADOR = "CZ";
const string SUBMARINO = "SB";
const string CONTRATORPEDEIRO = "CP";

Console.Write("Informe a quantidade de linhas e colunas do tabuleiro: ");
int dimensaoTabuleiro = int.Parse(Console.ReadLine());

string[,] tabuleiroJogador = new string[dimensaoTabuleiro, dimensaoTabuleiro];
string[,] tabuleiroComputador = new string[dimensaoTabuleiro, dimensaoTabuleiro];

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
void PosicionarPecas()
{
    PosicionarNavio("Contratorpedeiro", 1, CONTRATORPEDEIRO);

    PosicionarNavio("Submarino", 2, SUBMARINO);

    PosicionarNavio("Cruzador", 3, CRUZADOR);
}

// Posicionar qualquer tipo de navio
void PosicionarNavio(string nomeNavio, int tamanhoNavio, string simbolo)
{
    Console.WriteLine($"\nPosicione seu {nomeNavio} ({tamanhoNavio} posições).");

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
            Console.Write($"Informe a {i + 1}ª posição do {nomeNavio}: ");

            string[] coordenadas = Console.ReadLine().Split(',');

            novaX = int.Parse(coordenadas[0]);
            novaY = int.Parse(coordenadas[1]);

            jogadaValida = true;

            // Validar limites do tabuleiro
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
            // Validar se a posição é consecutiva
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
        tabuleiroJogador[novaX, novaY] = simbolo;

        // Guardar a coordenada para a próxima validação
        coordenadaAnteriorX = novaX;
        coordenadaAnteriorY = novaY;
    }
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

// Execução principal
MontarTabuleiroJogadores();
PosicionarPecas();
MostrarTabuleiroJogador();