using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOfDrones.Core.Domain.Models;

namespace GameOfDrones.Models.Game
{
    public class GameEditViewModel : GameBaseViewModel
    {
        public int Id { get; set; }
        public long Player1RoundsWon { get; set; }
        public long Player2RoundsWon { get; set; }
    }
}