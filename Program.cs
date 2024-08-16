using System;

namespace TicTacToeMinMax
{
    public class Program
    {
        static char[,] board = new char[3, 3];
        static int maxDepth = 9; // profundidade máxima da árvore de busca

        static bool isUserTurn = true;

        public static void Main(string[] args)
        {
            InitializeBoard();

            while (true)
            {
                PrintBoard();
                if (isUserTurn)
                {
                    MakeMove('X');
                    if (CheckWin('X')) 
                    {
                        PrintBoard();
                        Console.WriteLine("Você ganhou!");
                        break;
                    }
                }
                else
                {
                    MakeAIMove('O');
                    if (CheckWin('O')) 
                    {
                        PrintBoard();
                        Console.WriteLine("O computador ganhou!");
                        break;
                    }
                }
                isUserTurn = !isUserTurn;
                
                if (IsBoardFull())
                {
                    PrintBoard();
                    Console.WriteLine("Empate!");
                    break;
                }
            }

            Console.WriteLine("Fim do jogo!");
        }

        static void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        static void PrintBoard()
        {
            Console.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(" " + board[i, j] + " |");
                }
                Console.WriteLine();
                if (i < 2) Console.WriteLine(" ---------");
            }
        }

        static void MakeMove(char player)
        {
            Console.Write("Digite a posição da sua jogada (1-9): ");
            int pos = int.Parse(Console.ReadLine());
            int row = (pos - 1) / 3;
            int col = (pos - 1) % 3;
            if (board[row, col] == ' ')
            {
                board[row, col] = player;
            }
            else
            {
                Console.WriteLine("Posição inválida, tente novamente.");
                MakeMove(player);
            }
        }

        static void MakeAIMove(char player)
        {
            int bestMove = -1;
            int bestScore = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = player;
                        int score = CalculatePositionScore(i, j, player);
                        board[i, j] = ' ';
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = i * 3 + j;
                        }
                    }
                }
            }

            int row = bestMove / 3;
            int col = bestMove % 3;
            board[row, col] = player;
        }

        static int CalculatePositionScore(int row, int col, char player)
        {
            int score = 0;
            char opponent = (player == 'X') ? 'O' : 'X';

            // Mais 2 pontos se for posição central
            if (row == 1 && col == 1)
                score += 2;

            // Mais 1 ponto se for um dos cantos
            if ((row == 0 && col == 0) || (row == 0 && col == 2) ||
                (row == 2 && col == 0) || (row == 2 && col == 2))
                score += 1;

            // Menos 2 pontos se houver peças do adversário na linha, coluna ou diagonal
            if (HasOpponentInLineOrDiagonal(row, col, opponent))
                score -= 2;

            // Mais 4 pontos se a jogada impedir a vitória do adversário
            board[row, col] = player;
            if (CheckWin(player))
                score += 4;
            board[row, col] = ' '; // desfaz a jogada

            // Mais 4 pontos se a jogada levar à vitória
            board[row, col] = opponent;
            if (CheckWin(opponent))
                score += 4;
            board[row, col] = ' '; // desfaz a jogada

            return score;
        }

        static bool HasOpponentInLineOrDiagonal(int row, int col, char opponent)
        {
            // Verificar linhas, colunas e diagonais
            for (int i = 0; i < 3; i++)
            {
                if (board[row, i] == opponent || board[i, col] == opponent)
                    return true;
            }
            if (row == col)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, i] == opponent)
                        return true;
                }
            }
            if (row + col == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, 2 - i] == opponent)
                        return true;
                }
            }
            return false;
        }

        static bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ') return false;
                }
            }
            return true;
        }

        static bool CheckWin(char player)
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
            }

            // Check columns
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
            }

            // Check diagonals
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;

            return false;
        }
    }
}
