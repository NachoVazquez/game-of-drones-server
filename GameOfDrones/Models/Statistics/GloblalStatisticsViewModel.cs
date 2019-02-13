using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfDrones.Models.Statistics
{
    public class GlobalStatisticsViewModel
    {
        public long RegisteredPlayers { get; set; }
        public long GamesPlayed { get; set; }
        public long RoundsPlayed { get; set; }
    }
}