using System.Threading.Tasks;
using GameOfDrones.Core.Domain.Models;

namespace GameOfDrones.Core.Abstractions.DataAccess
{
    public interface IRoundRepository : IRepository<Round, int>
    {
        Task<long> GetRoundsPlayed();
    }
}