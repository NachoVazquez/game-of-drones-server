using GameOfDrones.Core.Domain.Models;

namespace GameOfDrones.Core.Abstractions.DataAccess
{
    public interface IGameRepository : IRepository<Game, int>
    {
    }
}
