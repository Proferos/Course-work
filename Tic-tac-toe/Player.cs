using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

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
        public static List<Player> PlayersBase = new List<Player>();
        public int number { get; set; }
        protected static int PlayerNumber = 0;


        public Player(string name, string username, string password, char marker)
        {
            Name = name;
            Username = username;
            Marker = marker;
            Wins = 0;
            Losses = 0;
            GameHistories = new List<GameHistory>();
            using (SHA256 sha256 = SHA256.Create())
            {
                PasswordHash = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
            Password = PasswordHash;
            number = PlayerNumber++;
        }


        public void PrintGameHistory()
        {
            Console.WriteLine($"Game history for {Name}:");
            Console.WriteLine("ID                 Date            Opponent         Result");
            Console.WriteLine("==============================================================");
            foreach (GameHistory history in GameHistories)
            {
                string opponentName = history.PlayerO == this ? history.PlayerX.Name : history.PlayerO.Name;
                string result = history.Winner == null ? "Draw" : (history.Winner == this ? "Win" : "Loss");
                Console.WriteLine($"{history.Id,-24}  {history.Date:dd-MM-yyyy}  {opponentName,-15}  {result}");
            }
        }

        public static Player find(string username) 
        {
            // finds an element in list PlyerBase by property Username
            return PlayersBase.Find(x => x.Username == username); 
        }

        /*public static void Save(string fileName)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Player>));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, PlayersBase);
                string json = Encoding.UTF8.GetString(stream.ToArray());
                File.WriteAllText(fileName, json);
            }
        }

        public static List<Player> Load(string fileName)
        {
            string json = File.ReadAllText("D:\\repos\\kpi_repos\\OOP\\Course-work\\Course-work\\Tic-tac-toe\\players\\players.json");
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Player>));
                List<Player> players = (List<Player>)serializer.ReadObject(stream);
                return players;
            }
        }*/

        /* public void SaveToFile()
         {
             if (!File.Exists("D:\\repos\\kpi_repos\\OOP\\Course-work\\Course-work\\Tic-tac-toe\\players\\player" + number + ".txt")) {
                 File.Create("D:\\repos\\kpi_repos\\OOP\\Course-work\\Course-work\\Tic-tac-toe\\players\\player" + number + ".txt");
             }
             string filePath = "D:\\repos\\kpi_repos\\OOP\\Course-work\\Course-work\\Tic-tac-toe\\players\\player" + number + ".txt";
             *//*FileStream file = new FileStream(filePath, FileMode.Append);
             StreamWriter writer = new StreamWriter(file);
             writer.WriteLine(Name);
             writer.WriteLine(Username);
             writer.WriteLine(Password);
             writer.WriteLine(Marker);
             writer.Close();
             file.Close();*//*
             using (StreamWriter writer = new StreamWriter(filePath))
             {
                 // Write player data to the file
                 writer.WriteLine(Name);
                 writer.WriteLine(Username);
                 writer.WriteLine(PasswordHash);
                 writer.WriteLine(Marker);
                 writer.WriteLine(Wins);
                 writer.WriteLine(Losses);

                 // Write game history to the file
                 foreach (GameHistory history in GameHistories)
                 {
                     writer.WriteLine(history.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                     writer.WriteLine(history.PlayerX.Username);
                     writer.WriteLine(history.PlayerO.Username);
                     writer.WriteLine(history.Winner?.Username ?? "");
                 }
                 writer.Close();
             }
         }

         public static Player LoadFromFile()
         {
             string filePath = "D:\\repos\\kpi_repos\\OOP\\Course-work\\Course-work\\Tic-tac-toe\\players.txt";

             using (StreamReader reader = new StreamReader(filePath))
             {
                 // Read player data from the file
                 string name = reader.ReadLine();
                 string username = reader.ReadLine();
                 string passwordHash = reader.ReadLine();
                 char marker = reader.ReadLine()[0];
                 int wins = int.Parse(reader.ReadLine());
                 int losses = int.Parse(reader.ReadLine());

                 Player player = new Player(name, username, passwordHash, marker);
                 player.Wins = wins;
                 player.Losses = losses;

                 // Read game history from the file
                 while (!reader.EndOfStream)
                 {
                     DateTime date = DateTime.Parse(reader.ReadLine());
                     string playerXUsername = reader.ReadLine();
                     string playerOUsername = reader.ReadLine();
                     string winnerUsername = reader.ReadLine();

                     // Create the Player objects for the game history entry
                     Player playerX = new Player("", playerXUsername, "", 'X');
                     Player playerO = new Player("", playerOUsername, "", 'O');
                     Player winner = string.IsNullOrEmpty(winnerUsername) ? null : new Player("", winnerUsername, "", 'X');

                     player.GameHistories.Add(new GameHistory(playerX, playerO, winner));
                 }

                 return player;
             }
         }*/

    }

}

