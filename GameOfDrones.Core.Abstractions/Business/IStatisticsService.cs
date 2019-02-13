using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameOfDrones.Core.Domain.Models;

namespace GameOfDrones.Core.Abstractions.Business
{
    public interface IStatisticsService
    {
        Task<long?> GetRegisteredPlayersAsync();
        Task<long?> GetGamesPlayed();
        Task<long?> GetRoundsPlayed();
        Task<Player> GetPlayerByUserNameAsync(string playerName);
    }
}