using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace Tic_tac_toe
{
    class Player
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public char Marker { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public List<GameHistory> GameHistories { get; set; }
        public string PasswordHash { get; set; }


        public Player(string name, string username, string password, char marker)
        {
            Name = name;
            Username = username;
            Password = password;
            Marker = marker;
            Wins = 0;
            Losses = 0;
            GameHistories = new List<GameHistory>();
            using (SHA256 sha256 = SHA256.Create())
            {
                PasswordHash = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public void PrintGameHistory()
        {
            Console.WriteLine($"Game history for {Name}:");
            Console.WriteLine("Date            Opponent         Result");
            Console.WriteLine("==============================================");
            foreach (GameHistory history in GameHistories)
            {
                string opponentName = history.PlayerO == this ? history.PlayerX.Name : history.PlayerO.Name;
                string result = history.Winner == null ? "Draw" : (history.Winner == this ? "Win" : "Loss");
                Console.WriteLine($"{history.Date:yyyy-MM-dd}  {opponentName,-15}  {result}");
            }
        }
    }

}
