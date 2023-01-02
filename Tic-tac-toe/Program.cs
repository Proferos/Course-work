using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Tic_tac_toe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new game and start it
            TicTacToeGame game = new TicTacToeGame();
            game.Start();
        }
    }

    class TicTacToeGame
    {
        private Player playerX = new Player("leha", "nagibator", "pass1", 'X');
        private Player playerO = new Player("bodya", "zxc", "pass2", 'O');

        // Constants for the size of the board and the characters used to represent each player
        private const int BOARD_SIZE = 3;
        private const char PLAYER_X = 'X';
        private const char PLAYER_O = 'O';

        // The board and the current player
        private char[,] board = new char[BOARD_SIZE, BOARD_SIZE];
        private char currentPlayer = PLAYER_X;

        // A list of all of the winning combinations
        private List<int[]> winningCombinations = new List<int[]>
        {
            new int[] {0, 1, 2}, // rows
            new int[] {3, 4, 5},
            new int[] {6, 7, 8},
            new int[] {0, 3, 6}, // columns
            new int[] {1, 4, 7},
            new int[] {2, 5, 8},
            new int[] {0, 4, 8}, // diagonals
            new int[] {2, 4, 6}
        };

        public void Start()
        {
            // Initialize the board
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = ' ';
                }
            }

            // Main game loop
            while (true)
            {
                // Print the board
                PrintBoard();

                // Get the player's move
                int move = GetPlayerMove();

                // Update the board with the player's move
                UpdateBoard(move);

                // Check if the player has won or if the game is a draw
                if (CheckForWin())
                {
                    Console.WriteLine($"Player {currentPlayer} wins!");

                    // Create a new GameHistory object and add it to the players' histories
                    GameHistory history = new GameHistory(playerX, playerO, currentPlayer == PLAYER_X ? playerX : playerO);
                    playerX.GameHistories.Add(history);
                    playerO.GameHistories.Add(history);

                    break;
                }
                else if (CheckForDraw())
                {
                    Console.WriteLine("The game is a draw.");

                    // Create a new GameHistory object and add it to the players' histories
                    GameHistory history = new GameHistory(playerX, playerO, null);
                    playerX.GameHistories.Add(history);
                    playerO.GameHistories.Add(history);

                    break;
                }

                // Switch to the other player
                SwitchPlayer();
            }
            // Prompt the players to log in
            playerX = PromptForLogin(PLAYER_X);
            playerO = PromptForLogin(PLAYER_O);
        }

        private Player PromptForLogin(char marker)
        {
            while (true)
            {
                Console.Write($"Player {marker}, enter your username: ");
                string username = Console.ReadLine();

                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                // Hash the password using SHA-256
                using (SHA256 sha256 = SHA256.Create())
                {
                    string hashedPassword = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                    // Check if the username and password are valid (in this example, we just check if the username is "playerX" or "playerO")
                    if (username == $"player{marker}" && hashedPassword == $"{marker}password")
                    {
                        // Return the player object
                        return new Player($"Player {marker}", username, hashedPassword, marker);
                    }
                }

                Console.WriteLine("Invalid username or password. Try again.");
            }
        }

        private void PrintBoard()
        {
            // Clear the console
            Console.Clear();

            // Print the board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] == ' ' ? '-' : board[i, j]);
                    if (j < 2)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
                if (i < 2)
                {
                    Console.WriteLine("-+-+-");
                }
            }
        }

        private int GetPlayerMove()
        {
            while (true)
            {
                Console.Write($"Player {currentPlayer}, enter your move (row column): ");
                string input = Console.ReadLine();

                // Try to parse the input as a row and column
                string[] tokens = input.Split(' ');
                if (tokens.Length == 2)
                {
                    if (int.TryParse(tokens[0], out int row) && int.TryParse(tokens[1], out int column))
                    {
                        // Check if the move is valid (within the bounds of the board and not already taken)
                        if (row >= 0 && row < BOARD_SIZE && column >= 0 && column < BOARD_SIZE && board[row, column] == ' ')
                        {
                            return row * BOARD_SIZE + column;
                        }
                    }
                }

                Console.WriteLine("Invalid move. Try again.");
            }
        }

        private void UpdateBoard(int move)
        {
            int row = move / BOARD_SIZE;
            int column = move % BOARD_SIZE;
            board[row, column] = currentPlayer;
        }

        private bool CheckForWin()
        {
            // Check each winning combination
            foreach (int[] combination in winningCombinations)
            {
                char first = board[combination[0] / BOARD_SIZE, combination[0] % BOARD_SIZE];
                if (first != ' ' && first == board[combination[1] / BOARD_SIZE, combination[1] % BOARD_SIZE] && first == board[combination[2] / BOARD_SIZE, combination[2] % BOARD_SIZE])
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckForDraw()
        {
            // If there are any empty spaces on the board, the game is not a draw
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        return false;
                    }
                }
            }

            // Otherwise, the game is a draw
            return true;
        }

        private void SwitchPlayer()
        {
            if (currentPlayer == PLAYER_X)
            {
                currentPlayer = PLAYER_O;
            }
            else
            {
                currentPlayer = PLAYER_X;
            }
        }
    }
}