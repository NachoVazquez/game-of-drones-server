using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Core.Abstractions.Business
{
    public interface IStatisticsService
    {
        Task<long?> GetRegisteredPlayersAsync();
        Task<long?> GetGamesPlayed();
        Task<long?> GetRoundsPlayed();
    }
}