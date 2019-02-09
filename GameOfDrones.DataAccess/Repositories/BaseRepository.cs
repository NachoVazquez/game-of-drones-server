using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Base;
using GameOfDrones.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GameOfDrones.DataAccess.Repositories
{
    /// <summary>
    ///     Contains the implementation of the base
    ///     functionalities for the repositories
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of entity that the actual implementation
    ///     of this interface handles
    /// </typeparam>
    /// <typeparam name="TKey">
    ///     The type of the <typeparamref name="TEntity"/>'s Primary Key
    /// </typeparam>
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///     Gets dB representation of the object
        ///     of type TEntity.
        /// </summary>
        public DbSet<TEntity> Entities { get; }

        /// <summary>
        /// Gets the Actual DBContext
        /// </summary>
        public DbContext DbContext { get; }        

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The implementation of <see cref="DbContext"/></param>
        protected BaseRepository(GameOfDronesContext dbContext)
        {
            DbContext = dbContext;
            this.Entities = this.DbContext.Set<TEntity>();          
        }

        /// <summary>
        ///     Gets an instance of the <see cref="IDbConnection"/> using the ConenctionString from the context.
        /// </summary>
        protected IDbConnection DbConnection => DbContext.Database.GetDbConnection();

        /// <summary>
        ///     Asynchronously adds an object to the table
        /// </summary>
        /// <param name="obj">The object to be added</param>
        /// <returns>Returns the <paramref name="obj"/> after being inserted</returns>
        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The given object must not be null");
            }

            if (obj is TrackableEntity<object>)
            {
                var entity = obj as TrackableEntity<object>;
                entity.CreatedAt = DateTime.UtcNow;
                entity.ModifiedAt = DateTime.UtcNow;
            }

            await this.Entities.AddAsync(obj);
            return obj;
        }

        /// <summary>
        ///     Asynchronously adds the list of elements given in <paramref name="objs"/> to the Table
        /// <remarks>
        ///     Use this method instead of <see cref="IBaseRepository{TEntity,TKey}.Add"/> when possible
        /// </remarks>
        /// </summary>
        /// <param name="objs">The objects to be added</param>
        /// <returns>The given <paramref name="objs"/> after being inserted</returns>
        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> objs)
        {
            if (objs == null)
            {
                throw new ArgumentNullException(nameof(objs), "The given object must not be null");
            }

            if (!objs.Any())
            {
                throw new ArgumentException("The given param must contains at least on element", nameof(objs));
            }

            if (objs.First() is TrackableEntity<object>)
            {
                var entities = objs as IEnumerable<TrackableEntity<object>>;
                foreach (var trackableEntity in entities)
                {
                    trackableEntity.CreatedAt = DateTime.UtcNow;
                    trackableEntity.ModifiedAt = DateTime.UtcNow;
                }
            }

            await this.Entities.AddRangeAsync(objs);
            return objs;
        }


        /// <summary>
        ///     Asynchronously check if the given <paramref name="obj"/> exists in the table
        /// </summary>
        /// <param name="obj">The element to locate in the Table</param>
        /// <returns><value>True</value> if the given object exists, false otherwise</returns>
        public virtual async Task<bool> ExistsAsync(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The given object must not be null");
            }

            return await this.Entities.ContainsAsync(obj);
        }

        /// <summary>
        ///     Asynchronously check if there is an element with the given <value>Id</value> in the Table
        /// </summary>
        /// <param name="id">The PK to be checked</param>
        /// <returns><value>True</value> if the PK exists, false otherwise</returns>
        public virtual async Task<bool> ExistsAsync(TKey id)
        {
            return await this.Entities.AnyAsync(ent => ent.Id.Equals(id));
        }

        /// <summary>
        ///     Asynchronously checks if there is at least one element that satisfies the condition
        /// </summary>
        /// <param name="filter">The predicate to be applied for each element in the Table</param>
        /// <returns><value>True</value> if any element satisfies the condition; otherwise, false</returns>
        public virtual async Task<bool> ExistsAsync(Func<TEntity, bool> filter)
        {
            return await this.Entities.AnyAsync(ent => filter.Invoke(ent),
                cancellationToken: default(CancellationToken));
        }


        /// <summary>
        ///     Asynchronously filter the elements in the table based on
        ///     the given predicate
        /// </summary>
        /// <param name="filter">A function to be applied in each element of the table</param>
        /// <returns>The elements that satisfy the predicate <paramref name="filter"/></returns>
        public virtual async Task<IQueryable<TEntity>> ReadAllAsync(Func<TEntity, bool> filter)
        {
            return await Task.Factory.StartNew(() => { return this.Entities.Where(ent => filter.Invoke(ent)); });
        }


        /// <summary>
        ///     Asynchronously begins tracking all the Entities that satisfy
        ///     the predicate given in <paramref name="filter"/> in the
        ///     <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="DbLoggerCategory.Update"/> is called
        /// </summary>
        /// <param name="filter">A function to be applied in each element of the table</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task RemoveAsync(Func<TEntity, bool> filter)
        {
            await Task.Factory.StartNew(() => { return Entities.Where(ent => filter.Invoke(ent)); }).ContinueWith(
                antecedent =>
                {
                    var elementsToRemove = antecedent.Result;
                    Entities.RemoveRange(elementsToRemove);
                });
        }

        /// <summary>
        ///     Asynchronously begins tracking the given Entity
        ///     in the <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="DbLoggerCategory.Update"/> is called
        /// </summary>
        /// <param name="obj">The objects to be marked</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task RemoveAsync(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The given object must not be null");
            }

            await Task.Factory.StartNew(() => { Entities.Remove(obj); });
        }

        /// <summary>
        ///     Asynchronously begins tracking entity with the given
        ///     <value>Id</value> in the <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="DbLoggerCategory.Update"/> is called
        /// </summary>
        /// <param name="id">The <value>Id</value> of the Entity to remove</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task RemoveAsync(TKey id)
        {
            var objToDelete = await this.Entities.FirstOrDefaultAsync(ent => ent.Id.Equals(id));

            if (objToDelete == null)
            {
                throw new ArgumentException("Does not exist an element with the given key", nameof(id));
            }

            this.Entities.Remove(objToDelete);
        }


        /// <summary>
        ///     Asynchronously begins tracking the given Entities
        ///     in the <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="DbLoggerCategory.Update"/> is called
        /// </summary>
        /// <param name="objs">The objects to be marked</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> objs)
        {
            if (objs == null)
            {
                throw new ArgumentNullException(nameof(objs), "The given object must not be null");
            }

            if (!objs.Any())
            {
                return;
            }

            await Task.Factory.StartNew(() => { Entities.RemoveRange(objs); });
        }


        public virtual IRepository<TEntity1, TKey1> Set<TEntity1, TKey1>() where TEntity1 : Entity<TKey1>
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Asynchronously returns the only element of the table that satisfies the predicate
        ///     given in <paramref name="filter"/> or a default value if no element
        ///     exist.
        /// <exception cref="InvalidOperationException">
        ///     Throws an exception if more than one element satisfies
        ///     the condition
        /// </exception>
        /// </summary>
        /// <param name="filter">The predicate to be applied for each element in the table</param>
        /// <returns>The element that satisfies the given predicate</returns>
        public virtual async Task<TEntity> SingleOrDefaultAsync(Func<TEntity, bool> filter)
        {
            return await this.Entities.SingleOrDefaultAsync(ent => filter.Invoke(ent));
        }

        /// <summary>
        ///     Asynchronously returns the only element of the table with the given <value>Id</value>
        /// </summary>
        /// <param name="id">The Id of the desired element</param>
        /// <returns>The element with the given Id</returns>
        public virtual async Task<TEntity> SingleOrDefaultAsync(TKey id)
        {
            return await this.Entities.SingleOrDefaultAsync(ent => ent.Id.Equals(id));
        }


        /// <summary>
        ///     Asynchronously begins tracking the given param
        /// <remarks>
        ///     All the properties will be marked
        ///     as modified. To mark only some properties use the
        ///     <see cref="M:Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0)"/>
        /// </remarks>
        /// </summary>
        /// <param name="obj">The object to be marked</param>
        /// <returns>The given <paramref name="obj"/> after being inserted</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The given object must not be null");
            }

            if (!await this.ExistsAsync(obj.Id))
            {
                throw new ArgumentException("The given object does not exist in DB", nameof(obj));
            }

            if (obj is TrackableEntity<object>)
            {
                var entity = obj as TrackableEntity<object>;
                entity.ModifiedAt = DateTime.UtcNow;
            }

            await Task.Factory.StartNew(() => { this.Entities.Update(obj); });
            return obj;
        }


        /// <summary>
        ///     Asynchronously begins tracking objects given in <paramref name="objs"/>
        /// <remarks>
        ///     All the properties will be marked
        ///     as modified. To mark only some properties use the
        ///     <see cref="M:Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0)"/>
        /// </remarks>
        /// </summary>
        /// <param name="objs">The objects to be marked</param>
        /// <returns>The given <paramref name="objs"/> after being inserted</returns>
        public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> objs)
        {
            if (objs == null)
            {
                throw new ArgumentNullException(nameof(objs), "The given object must not be null");
            }

            if (!objs.Any())
            {
                throw new ArgumentException("The given param must contains at least on element", nameof(objs));
            }

            if (objs.First() is TrackableEntity<object>)
            {
                var entities = objs as IEnumerable<TrackableEntity<object>>;
                foreach (var trackableEntity in entities)
                {
                    trackableEntity.ModifiedAt = DateTime.UtcNow;
                }
            }

            await Task.Factory.StartNew(() => { this.Entities.UpdateRange(objs); });
            return objs;
        }

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


        #region IDisposable Support

        /// <summary>
        ///     Release the allocated resources of the <see cref="DbContext"/>
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Release the allocated resources of the <see cref="DbContext"/>
        /// <remarks>
        ///     If the derived classes use objects that could
        ///     manage resources outside the context, override it
        ///     and dispose those objects
        /// </remarks>
        /// </summary>
        /// <param name="disposing">True for disposing the object; otherwise, false</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.DbContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        #endregion
    }
}