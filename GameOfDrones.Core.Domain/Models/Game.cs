using GameOfDrones.Core.Domain.Base;
using System.Collections.Generic;

namespace GameOfDrones.Core.Domain.Models
{
    public class Game : TrackableEntity<int>
    {
        public int RoundsToWin { get; set; }

        public List<Round> Rounds { get; set; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int? Player1Id { get; set; }
        public int? Player2Id { get; set; }

        public long Player1RoundsWon { get; set; }
        public long Player2RoundsWon { get; set; }

    }
}
