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
    [Serializable]
    class BasePlayer
    {
        protected string Name { get; set; }
        protected string Username { get; set; }
        public string Password { get; set; }
        protected int Wins { get; set; }
        protected int Losses { get; set; }
        public string PasswordHash { get; set; }
        protected int rating { get; set; }
        public List<GameHistory> GameHistories { get; set; }
        public static List<BasePlayer> PlayersBase = new List<BasePlayer>();

        virtual public void win()
        {
            rating += 50;
        }
        virtual public void lose()
        {
            rating -= 50;
            if (rating < 0) rating = 0;
        }

        public BasePlayer() { }


        public BasePlayer(string name, string username, string password)
        {
            Name = name;
            Username = username;
            Wins = 0;
            Losses = 0;
            rating = 0;
            GameHistories = new List<GameHistory>();
            using (SHA256 sha256 = SHA256.Create())
            {
                PasswordHash = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
            Password = PasswordHash;
        }


        public void PrintGameHistory()
        {
            Console.WriteLine($"Game history for {Name}:");
            Console.WriteLine($"Current Rating: {rating}");
            Console.WriteLine("ID                                    Date         Opponent       Result    Rating");
            Console.WriteLine("========================================================================================");
            foreach (GameHistory history in GameHistories)
            {
                string opponentName = history.PlayerO == this ? history.PlayerX.Name : history.PlayerO.Name;
                string result = history.Winner == null ? "Draw" : (history.Winner == this ? "Win" : "Loss");
                Console.WriteLine($"{history.Id,-24}  {history.Date:dd-MM-yyyy}  {opponentName,-15}  {result,-12} {history.Rating}");
            }
        }

        public static BasePlayer find(string username)
        {
            // finds an element in list PlyerBase by property Username
            return PlayersBase.Find(x => x.Username == username);
        }

        public static bool exists(string username)
        {
            return BasePlayer.PlayersBase.Exists(x => x.Username == username);
        }

        public static void save()
        {
            var binFormatter = new BinaryFormatter();

            using (var file = new FileStream("PlayerBase.bin", FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, PlayersBase);
            }
        }

        public static void load() 
        {
            var binFormatter = new BinaryFormatter();

            using (var file = new FileStream("PlayerBase.bin", FileMode.OpenOrCreate))
            {
                var newPlayerBase = binFormatter.Deserialize(file) as List<BasePlayer>;

                if (newPlayerBase != null)
                {
                    PlayersBase = newPlayerBase;
                }
            }
        }


        

    }

    [Serializable]
    class PremiumPlayer : BasePlayer
    {
        public PremiumPlayer(string name, string username, string password)
        {
            Name = name;
            Username = username;
            Wins = 0;
            Losses = 0;
            rating = 1000;
            GameHistories = new List<GameHistory>();
            using (SHA256 sha256 = SHA256.Create())
            {
                PasswordHash = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
            Password = PasswordHash;
        }
        public override void win()
        {
            rating += 70;
        }
        public override void lose()
        {
            rating -= 50;
            if (rating < 0) rating = 0;
        }
    }



    [Serializable]
    class VIPPlayer : BasePlayer
    {
        public VIPPlayer(string name, string username, string password)
        {
            Name = name;
            Username = username;
            Wins = 0;
            Losses = 0;
            rating = 5000;
            GameHistories = new List<GameHistory>();
            using (SHA256 sha256 = SHA256.Create())
            {
                PasswordHash = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
            Password = PasswordHash;
        }
        public override void win()
        {
            rating += 100;
        }
        public override void lose()
        {
            rating -= 50;
            if (rating < 0) rating = 0;
        }
    }
}

