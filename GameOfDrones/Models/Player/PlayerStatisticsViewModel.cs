using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfDrones.Models.Player
{
    public class PlayerStatisticsViewModel
    {
        public string PlayerName { get; set; }
        public long GamesWon { get; set; }
        public long GamesLost { get; set; }
        public long RoundsWon { get; set; }
        public long RoundsLost { get; set; }
    }
}