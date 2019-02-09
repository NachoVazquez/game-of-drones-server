using System;
using System.Collections.Generic;
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
        /// Saves changes to the underlying store asynchronously
        /// </summary>
        /// <returns>The number of afected objects</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Executes a raw query to the underlying store asynchronously
        /// </summary>
        /// <typeparam name="TResult">The type of the entities returned by the query</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="queryParams">The query parameters</param>
        /// <returns>A collection of elements returned by the query</returns>
        Task<ICollection<TResult>> RawQueryAsync<TResult>(string query, object queryParams = null);

        Task<TResult> QueryFirstAsync<TResult>(string query, object queryParams = null);
    }
}