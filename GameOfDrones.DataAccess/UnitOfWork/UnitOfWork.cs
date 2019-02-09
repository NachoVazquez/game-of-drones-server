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



        /// <summary>
        /// Constructor for testing purposes
        /// </summary>
        /// <param name="dbContext"></param>
        public SqlUnitOfWork(GameOfDronesContext dbContext, IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            DbContext = dbContext;
            GameRepository = gameRepository;
            PlayerRepository = playerRepository;
        }

        public GameOfDronesContext DbContext { get; set; }

        protected string ConnectionStringName { get; set; }


        public IDbConnection OpenConnection(out bool closeManually)
        {
            var conn = DbContext.Database.GetDbConnection();
            closeManually = false;

            // Not sure here, should assume always opened??
            if (conn.State == ConnectionState.Open) return conn;

            conn.Open();
            closeManually = true;

            return conn;
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public async Task<ICollection<TResult>> RawQueryAsync<TResult>(string query, object queryParams = null)
        {
            var connection = OpenConnection(out bool closeConnection);
            var queryResult = await connection.QueryAsync<TResult>(query, queryParams);
            if (closeConnection)
            {
                connection.Close();
            }

            return queryResult.ToList();
        }

        public async Task<TResult> QueryFirstAsync<TResult>(string query, object queryParams = null)
        {
            var connection = OpenConnection(out bool closeConnection);
            var queryResult = await connection.QueryFirstAsync<TResult>(query, queryParams);
            if (closeConnection)
            {
                connection.Close();
            }

            return queryResult;
        }


        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}