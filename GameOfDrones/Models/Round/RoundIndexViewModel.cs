using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOfDrones.Core.Domain;

namespace GameOfDrones.Models.Round
{
    public class RoundIndexViewModel: RoundBaseViewModel
    {
        public PlayerMoves Player1Move { get; set; }
        public PlayerMoves Player2Move { get; set; }

        public PlayerMoves WinnerMove { get; set; }
    }
}
