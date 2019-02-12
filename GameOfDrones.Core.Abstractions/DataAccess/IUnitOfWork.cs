using System;
using System.Threading.Tasks;

namespace GameOfDrones.Core.Abstractions.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Instance of <see cref="IGameRepository"/>
        /// </summary>
        IGameRepository GameRepository { get; set; }

        /// <summary>
        /// Instance of <see cref="IPlayerRepository"/>
        /// </summary>
        IPlayerRepository PlayerRepository { get; set; }

        /// <summary>
        /// Instance of <see cref="IRoundRepository"/>
        /// </summary>
        IRoundRepository RoundRepository { get; set; }

        /// <summary>
        /// Saves changes to the underlying store asynchronously
        /// </summary>
        /// <returns>The number of afected objects</returns>
        Task<int> SaveChangesAsync();

    }
}