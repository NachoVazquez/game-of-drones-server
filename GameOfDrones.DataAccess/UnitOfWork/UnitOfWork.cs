using Dapper;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfDrones.DataAccess.UnitOfWork
{
    /// <summary>
    /// Implementation of the Unit of Work Pattern for SQL using EF
    /// </summary>
    public class SqlUnitOfWork : IUnitOfWork
    {
        public IGameRepository GameRepository { get; set; }

        public IPlayerRepository PlayerRepository { get; set; }

        public IRoundRepository RoundRepository { get; set; }


        /// <summary>
        /// Constructor for testing purposes
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="gameRepository"></param>
        /// <param name="playerRepository"></param>
        /// <param name="roundRepository"></param>
        public SqlUnitOfWork(
            GameOfDronesContext dbContext,
            IGameRepository gameRepository,
            IPlayerRepository playerRepository,
            IRoundRepository roundRepository)
        {
            DbContext = dbContext;
            GameRepository = gameRepository;
            PlayerRepository = playerRepository;
            RoundRepository = roundRepository;
        }

        public GameOfDronesContext DbContext { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}