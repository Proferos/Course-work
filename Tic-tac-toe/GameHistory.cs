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
        public BasePlayer PlayerX { get; set; }
        public BasePlayer PlayerO { get; set; }
        public BasePlayer Winner { get; set; }
        public DateTime Date { get; set; }

        public GameHistory(BasePlayer playerX, BasePlayer playerO, BasePlayer winner)
        {
            Id = Guid.NewGuid();
            PlayerX = playerX;
            PlayerO = playerO;
            Winner = winner;
            Date = DateTime.Now;
        }
    }

}
