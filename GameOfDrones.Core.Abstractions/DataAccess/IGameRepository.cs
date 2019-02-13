using System.Threading.Tasks;
using GameOfDrones.Core.Domain.Models;

namespace GameOfDrones.Core.Abstractions.DataAccess
{
    public interface IGameRepository : IRepository<Game, int>
    {
        Task<Game> GetGameWithPlayersByIdAsync(int id);

        Task<long> GetGamesPlayedAsync();
    }
}