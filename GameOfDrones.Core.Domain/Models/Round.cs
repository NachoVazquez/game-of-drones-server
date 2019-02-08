using GameOfDrones.Core.Domain.Base;

namespace GameOfDrones.Core.Domain.Models
{
    public class Round : TrackableEntity<int>
    {
        public PlayerMoves Player1Move { get; set; }
        public PlayerMoves Player2Move { get; set; }

        public PlayerMoves WinnerMove { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
