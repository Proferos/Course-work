using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_tac_toe
{
    class GameHistory
    {
        public Guid Id { get; set; }
        public Player PlayerX { get; set; }
        public Player PlayerO { get; set; }
        public Player Winner { get; set; }
        public DateTime Date { get; set; }

        public GameHistory(Player playerX, Player playerO, Player winner)
        {
            Id = Guid.NewGuid();
            PlayerX = playerX;
            PlayerO = playerO;
            Winner = winner;
            Date = DateTime.Now;
        }
    }

}
