using GameOfDrones.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Core.Abstractions.Business
{
    public interface IGameService: IApplicationService<Game, int>
    {
        Task<Game> CreateGameFromPlayerNamesAsync(string player1Name, string player2Name, int roundsToWin = 3);
    }
}
