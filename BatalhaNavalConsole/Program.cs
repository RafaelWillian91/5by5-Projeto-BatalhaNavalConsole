using System.Security.Cryptography;

Console.WriteLine(@"
╔══════════════════════════════════════════╗
║                                          ║
║           BATALHA NAVAL                  ║
║                                          ║
║         PREPARE SUA FROTA!               ║
║                                          ║
╚══════════════════════════════════════════╝
");

const string AGUA = "A";
const string CRUZADOR = "CZ";
const string SUBMARINO = "SB";
const string CONTRATORPEDEIRO = "CP";
const string TORPEDO = "TP";

bool inicializar = true;
int dimensaoTabuleiro = 0;

string[,] tabuleiroJogador = new string[dimensaoTabuleiro, dimensaoTabuleiro];
string[,] tabuleiroComputador = new string[dimensaoTabuleiro, dimensaoTabuleiro];
void DefinirDimensaoTabuleiro()
{
    do
    {
        Console.Write("Informe a quantidade de linhas e colunas do tabuleiro (5 a 10): ");
        dimensaoTabuleiro = int.Parse(Console.ReadLine());
        inicializar = false;

        if (dimensaoTabuleiro < 5)
        {
            Console.WriteLine("Dimensão do tabuleiro inválida");
            inicializar = true;
        }
        else if (dimensaoTabuleiro > 10)
        {
            Console.WriteLine("Dimensão do tabuleiro inválida");
            inicializar = true;
        }
    } while (inicializar);
}



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
    PosicionarNavio("Contratorpedeiro", 1, "CP");
    PosicionarNavioComputador("Contratorpedeiro", 1, "CP");

    PosicionarNavio("Submarino", 2, "SB");
    PosicionarNavioComputador("Submarino", 2, "SB");

    PosicionarNavio("Cruzador", 3, "CZ");
    PosicionarNavioComputador("Cruzador", 3, "CZ");
}

// Posicionar qualquer tipo de navio
void PosicionarNavio(string nomeNavio, int tamanhoNavio, string apelindoNavio)
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
        tabuleiroJogador[novaX, novaY] = apelindoNavio;

        // Guardar a coordenada para comparação com a próxima jogada
        coordenadaAnteriorX = novaX;
        coordenadaAnteriorY = novaY;
    }
}

void PosicionarNavioComputador(string nomeNavio, int tamanhoNavio, string apelindoNavio)
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

            //adiciona +1 para saber se o navio não irá extrapolar o tabuleiro
            if (direcao == 0)
            {
                novaY = coluna + i;
            }
            else
            {
                novaX = linha + i;
            }

            // Verifica os limites do tabuleiro de acordo com o tamanho do navio
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
                    tabuleiroComputador[linha, coluna + i] = apelindoNavio;
                }
                else
                {
                    tabuleiroComputador[linha + i, coluna] = apelindoNavio;
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

bool TorpedoJogador()
{
    bool validarJogada = true;

    do
    {
        Console.WriteLine("Escolha um alvo em coordenadas. (Exemplo: 2,2): ");
        string[] coordenadas = Console.ReadLine().Split(',');

        int coordX = int.Parse(coordenadas[0]);
        int coordY = int.Parse(coordenadas[1]);

        if (coordX < 0 || coordX >= dimensaoTabuleiro ||
            coordY < 0 || coordY >= dimensaoTabuleiro)
        {
            Console.WriteLine("Coordenada fora do tabuleiro!");
        }
        else if (tabuleiroComputador[coordX, coordY] == TORPEDO)
        {
            Console.WriteLine("Você já atacou essa coordenada!");
        }
        else
        {
            if (tabuleiroComputador[coordX, coordY] == AGUA)
            {
                Console.WriteLine("Você errou o torpedo!");
            }
            else
            {
                Console.WriteLine("BOOOOM! Você acertou o navio inimigo!");
            }

            tabuleiroComputador[coordX, coordY] = TORPEDO;
            validarJogada = false;
        }

    } while (validarJogada);

    return VerificarTabuleiro(tabuleiroComputador);
}

bool TorpedoComputador()
{

    bool validarJogada = true;

    do
    {
        int linha = Random.Shared.Next(0, dimensaoTabuleiro);
        int coluna = Random.Shared.Next(0, dimensaoTabuleiro);

        string tiro = tabuleiroJogador[linha, coluna];

        if (tiro == AGUA)
        {
            Console.WriteLine("Seu oponente errou!!");
            validarJogada = false;
        }else if (tiro == TORPEDO)
        {
            validarJogada = true;
        }else {
            Console.WriteLine("BOOOOOMMMM!!!! Seu adversário acertou seu navio!!");
            validarJogada = false;
        }

        tabuleiroJogador[linha, coluna] = TORPEDO;
    } while (validarJogada);

    return VerificarTabuleiro(tabuleiroJogador);
}

bool VerificarTabuleiro(string[,] tabuleiro)
{
    for (int i = 0; i < dimensaoTabuleiro; i++)
    {
        for (int j = 0; j < dimensaoTabuleiro; j++)
        {
            if (tabuleiro[i, j] == CONTRATORPEDEIRO ||
                tabuleiro[i, j] == SUBMARINO ||
                tabuleiro[i, j] == CRUZADOR)
            {
                return true;
            }
        }
    }
    
    return false;
}

void MontarPartida()
{
    // Solicitar e validar a dimensão
    DefinirDimensaoTabuleiro();

    // Criar os tabuleiros com a dimensão escolhida
    tabuleiroJogador = new string[dimensaoTabuleiro, dimensaoTabuleiro];
    tabuleiroComputador = new string[dimensaoTabuleiro, dimensaoTabuleiro];

    // Inicializar e posicionar os navios
    MontarTabuleiroJogadores();
    PosicionarPecasJogadores();

    // Exibir tabuleiro do jogador
    MostrarTabuleiroJogador();

    // Modo teste
    // MostrarTabuleiroComputador();
}

void IniciarPartida()
{

    {
        MontarPartida();

        bool partidaAndamento = true;

        do
        {
            partidaAndamento = TorpedoJogador();

            if (!partidaAndamento)
            {
                Console.WriteLine("Parabéns! Você venceu!");
                break;
            }

            partidaAndamento = TorpedoComputador();

            if (!partidaAndamento)
            {
                Console.WriteLine("O computador venceu!");
                break;
            }

        } while (partidaAndamento);
    }
}