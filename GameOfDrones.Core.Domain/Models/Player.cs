using GameOfDrones.Core.Domain.Base;
using System.Collections.Generic;

namespace GameOfDrones.Core.Domain.Models
{
    public class Player : Entity<int>
    {
        public string UserName { get; set; }
        public long GamesWon { get; set; }
        public long GamesLost { get; set; }
        public long RoundsWon { get; set; }
        public long RoundsLost { get; set; }
        public List<Game> GamesAsPlayer1 { get; set; }
        public List<Game> GamesAsPlayer2 { get; set; }
    }
}
