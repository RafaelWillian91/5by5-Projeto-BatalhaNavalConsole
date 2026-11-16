# 🚢 Batalha Naval — C# Console

Jogo de Batalha Naval desenvolvido em **C#**, com execução via console, como atividade prática do programa de estágio da **FiveByFive**.

O projeto tem como objetivo aplicar conceitos fundamentais de lógica de programação, utilizando estruturas condicionais, estruturas de repetição, matrizes e métodos.

## 🎮 Como funciona

O jogador enfrenta o computador em uma partida de Batalha Naval.

- O jogador escolhe o tamanho do tabuleiro, entre **5×5 e 10×10**.
- Cada participante possui seu próprio tabuleiro.
- O jogador posiciona seus navios manualmente.
- O computador posiciona seus navios aleatoriamente.
- Os ataques são realizados por coordenadas de linha e coluna.
- Os turnos são alternados entre jogador e computador.
- Vence quem atingir todas as posições ocupadas pelos navios adversários.

## ⚓ Tipos de navios

| Navio | Posições | Identificação |
|---|---|---|
| Contratorpedeiro | 1 | CP |
| Submarino | 2 | SB |
| Cruzador | 3 | CZ |

## 🛠️ Tecnologias utilizadas

- C#
- .NET
- Visual Studio
- Git e GitHub

## 📚 Conceitos aplicados

- Variáveis e constantes
- Estruturas condicionais (`if`, `else`)
- Estruturas de repetição (`for`, `do while`)
- Matrizes bidimensionais
- Métodos, parâmetros e valores de retorno
- Validação de coordenadas
- Geração de números aleatórios
- Decomposição funcional

## ▶️ Como executar

1. Clone o repositório:

   ```bash
   git clone https://github.com/RafaelWillian91/5by5-Projeto-BatalhaNavalConsole.git
   ```

2. Acesse a pasta do projeto:

   ```bash
   cd 5by5-Projeto-BatalhaNavalConsole/BatalhaNavalConsole
   ```

3. Execute a aplicação:

   ```bash
   dotnet run
   ```

É necessário ter o SDK do .NET compatível com o projeto instalado.
