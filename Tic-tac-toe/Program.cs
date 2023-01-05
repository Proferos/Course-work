﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.ExceptionServices;

namespace Tic_tac_toe
{
    class Program
    {

        static void Main(string[] args)
        {
            PremiumPlayer player = new PremiumPlayer("123","123","123");
            Console.WriteLine(player.Name);
            Console.WriteLine(player.Username);
            Console.WriteLine(player.Wins);
            Console.WriteLine(player.rating);

            /*while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome To The Main Menu Of The Greate Ti-Tac-Toe Game");
                Console.WriteLine("Please select one of the following options:");
                Console.WriteLine("[s] - to start a new game");
                Console.WriteLine("[r] - to call a registration form");
                Console.WriteLine("[h] - to call a game history of a certain player");
                Console.WriteLine("[x] - to close the game");

                char key = Console.ReadLine()[0];
                if (key == 's')
                {
                    // Create a new game and start it
                    Console.Clear();
                    TicTacToeGame game = new TicTacToeGame();
                    game.Start();
                }
                else if (key == 'r')
                {
                    Console.Clear();
                    TicTacToeGame.Registration();
                }
                else if (key == 'h')
                {
                    Console.Clear();
                    Console.Write("Print username of player: ");
                    string username = Console.ReadLine();
                    if (BasePlayer.exists(username))
                    {
                        BasePlayer.find(username).PrintGameHistory();
                        var a = Console.ReadLine();
                    }
                }
                else if (key == 'x')
                {
                    break;
                }

            }*/


        }
    }



    class TicTacToeGame
    {

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
            // Prompt the players to log in
            BasePlayer playerX = PromptForLogin(PLAYER_X);
            BasePlayer playerO = PromptForLogin(PLAYER_O);

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
                    if (currentPlayer == PLAYER_X) {
                        playerX.win();
                        playerO.lose();
                    }
                    else
                    {
                        playerO.win();
                        playerX.lose();
                    }
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
            
        }

        public static BasePlayer Registration()
        {
            while (true)
            {
                Console.WriteLine("Registration form");

                Console.Write($"Please, enter your name: ");
                string name = Console.ReadLine();

                Console.Write($"Please, enter your username: ");
                string username = Console.ReadLine();

                Console.Write("Enter your mark (X or O): ");
                char mark = Console.ReadLine()[0];

                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                Console.Write("Confirm your password: ");
                string password2 = Console.ReadLine();

                
                // add check for unique username
                if (password == password2 && !BasePlayer.exists(username)) 
                {
                    BasePlayer player = new BasePlayer(name, username, password);
                    BasePlayer.PlayersBase.Add(player);
                    return player;

                    break;
                }

                Console.WriteLine("Invalid username or password. Try again.");
            }
        }

        private static BasePlayer PromptForLogin(char marker)
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

                    if (BasePlayer.exists(username))
                    {
                        if (BasePlayer.find(username).Password == hashedPassword)
                        {
                            // Return the player object
                            return BasePlayer.find(username);
                        }

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
                    Console.WriteLine("");
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

       /* private void SaveGame()
        {
            using (StreamWriter writer = new StreamWriter("savedgame.txt"))
            {
                // Write game state to the file
                writer.WriteLine(playerX.Username);
                writer.WriteLine(playerO.Username);
                writer.WriteLine(currentPlayer.Username);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        writer.Write(board[i, j]);
                    }
                }
            }
        }*/


       /* private void LoadGame()
        {
            if (File.Exists("savedgame.txt"))
            {
                using (StreamReader reader = new StreamReader("savedgame.txt"))
                {
                    // Read game state from the file
                    string playerXUsername = reader.ReadLine();
                    string playerOUsername = reader.ReadLine();
                    string currentPlayerUsername = reader.ReadLine();
                    char[] boardState = reader.ReadToEnd().ToCharArray();

                    // Create the Player objects for the game
                    playerX = new Player("", playerXUsername, "", 'X');
                    playerO = new Player("", playerOUsername, "", 'O');
                    currentPlayer = playerXUsername == currentPlayerUsername ? playerX : playerO;

                    // Initialize the board
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            board[i, j] = boardState[i * 3 + j];
                        }
                    }
                }

                // Print the board
                PrintBoard();
            }
        }*/

    }
}