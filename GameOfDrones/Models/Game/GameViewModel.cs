using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfDrones.Models
{
    public class GameIndexViewModel
    {
        public int? Player1Id { get; set; }
        public int? Player2Id { get; set; }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public long Player1RoundsWon { get; set; }
        public long Player2RoundsWon { get; set; }
    }
}
