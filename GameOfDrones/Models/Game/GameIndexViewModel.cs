using System.Collections;
using System.Collections.Generic;
using GameOfDrones.Models.Round;

namespace GameOfDrones.Models.Game
{
    public class GameIndexViewModel : GameBaseViewModel
    {
        public int Id { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public long Player1RoundsWon { get; set; }
        public long Player2RoundsWon { get; set; }

        public int RoundsToWin { get; set; }

        public IEnumerable<RoundIndexViewModel> Rounds { get; set; }
    }
}