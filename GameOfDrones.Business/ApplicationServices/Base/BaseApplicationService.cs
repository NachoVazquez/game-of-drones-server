﻿using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfDrones.Business.ApplicationServices.Base
{
    /// <summary>
    ///     Contains the common functionalities of the Business layer
    ///     Through these object would be possible to communicate with the
    ///     DataAcces layer and apply businesses related operations
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for manipulate</typeparam>
    /// <typeparam name="TKey">The type of the key for that entity</typeparam>
    public abstract class BaseApplicationService<TEntity, TKey> : IApplicationService<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApplicationService{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The <see cref="IBaseRepository{TEntity,TKey}"/> for accessing to the
        /// functionalities of the DataAceess layer.</param>
        protected BaseApplicationService(IRepository<TEntity, TKey> repository, IUnitOfWork unitOfWork)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="BaseApplicationService{TEntity, TKey}"/> class.
        /// </summary>
        ~BaseApplicationService()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        /// Gets or Sets the Repository for handling the entity object
        /// </summary>
        public IRepository<TEntity, TKey> Repository { get; set; }

        /// <summary>
        /// Gets or sets the UnitOfWork of the app
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }


        /// <summary>
        ///     Asynchronously adds an object to the table
        /// </summary>
        /// <param name="obj">The object to be added</param>
        /// <returns>Returns the <paramref name="obj"/> after being inserted</returns>
        public async Task<TEntity> AddAsync(TEntity obj)
        {
            // Log.Information($"Adding a new entity of type: {nameof(obj)}");
            return await this.Repository.AddAsync(obj);
        }


        /// <summary>
        ///     Asynchronously adds the list of elements given in <paramref name="objs"/> to the Table
        /// <remarks>
        ///     Use this method instead of <see cref="IBaseApplicationService{TEntity,TKey}.Add"/> when possible
        /// </remarks>
        /// </summary>
        /// <param name="objs">The objects to be added</param>
        /// <returns>The given <paramref name="objs"/> after being inserted</returns>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> objs)
        {
            // Log.Information($"Adding {objs.Count()} new entities of type: {nameof(objs)}");
            return await this.Repository.AddRangeAsync(objs);
        }


        /// <summary>
        ///     Asynchronously check if the given <paramref name="obj"/> exists in the table
        /// </summary>
        /// <param name="obj">The element to locate in the Table</param>
        /// <returns><value>True</value> if the given object exists, false otherwise</returns>
        public async Task<bool> ExistsAsync(TEntity obj)
        {
            return await this.Repository.ExistsAsync(obj);
        }

        /// <summary>
        ///     Asynchronously check if there is an element with the given <value>Id</value> in the Table
        /// </summary>
        /// <param name="id">The PK to be checked</param>
        /// <returns><value>True</value> if the PK exists, false otherwise</returns>
        public async Task<bool> ExistsAsync(TKey id)
        {
            return await this.Repository.ExistsAsync(id);
        }

        /// <summary>
        ///     Asynchronously checks if there is at least one element that satisfies the condition
        /// </summary>
        /// <param name="filter">The predicate to be applied for each element in the Table</param>
        /// <returns><value>True</value> if any element satisfies the condition; otherwise, false</returns>
        public async Task<bool> ExistsAsync(Func<TEntity, bool> filter)
        {
            return await this.Repository.ExistsAsync(filter);
        }

        public Task<int> SaveChangesAsync()
        {
            return this.UnitOfWork.SaveChangesAsync();
        }


        /// <summary>
        ///     Asynchronously filter the elements in the table based on
        ///     the given predicate
        /// </summary>
        /// <param name="filter">A function to be applied in each element of the table</param>
        /// <returns>The elements that satisfy the predicate <paramref name="filter"/></returns>
        public async Task<IQueryable<TEntity>> ReadAllAsync(Func<TEntity, bool> filter)
        {
            return await this.Repository.ReadAllAsync(filter);
        }


        /// <summary>
        ///     Asynchronously begins tracking all the Entities that satisfy
        ///     the predicate given in <paramref name="filter"/> in the
        ///     <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="IBaseApplicationService{TEntity,TKey}.Update"/> is called
        /// </summary>
        /// <param name="filter">A function to be applied in each element of the table</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RemoveAsync(Func<TEntity, bool> filter)
        {
            await this.Repository.RemoveAsync(filter);
        }

        /// <summary>
        ///     Asynchronously begins tracking the given Entity
        ///     in the <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="IBaseApplicationService{TEntity,TKey}.Update"/> is called
        /// </summary>
        /// <param name="obj">The objects to be marked</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RemoveAsync(TEntity obj)
        {
            await this.Repository.RemoveAsync(obj);
        }

        /// <summary>
        ///     Asynchronously begins tracking entity with the given
        ///     <value>Id</value> in the <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="IBaseApplicationService{TEntity,TKey}.Update"/> is called
        /// </summary>
        /// <param name="id">The <value>Id</value> of the Entity to remove</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RemoveAsync(TKey id)
        {
            await this.Repository.RemoveAsync(id);
        }

        /// <summary>
        ///     Asynchronously begins tracking the given Entities
        ///     in the <see cref="F:Microsof.EntityFrameworkCore.EntityState.Deleted"/>
        ///     state such that it will be removed when <see cref="IBaseApplicationService{TEntity,TKey}.Update"/> is called
        /// </summary>
        /// <param name="objs">The objects to be marked</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RemoveRangeAsync(IEnumerable<TEntity> objs)
        {
            await this.Repository.RemoveRangeAsync(objs);
        }

        /// <summary>
        ///     Asynchronously returns the only element of the table that satisfies the predicate
        ///     given in <paramref name="filter"/> or a default value if no element
        ///     exist.
        /// <exception>
        ///     Throws an exception if more than one element satisfies
        ///     the condition
        /// </exception>
        /// </summary>
        /// <param name="filter">The predicate to be applied for each element in the table</param>
        /// <returns>The element that satisfies the given predicate</returns>
        public async Task<TEntity> SingleOrDefaultAsync(Func<TEntity, bool> filter)
        {
            return await this.Repository.SingleOrDefaultAsync(filter);
        }

        /// <summary>
        ///     Asynchronously returns the only element of the table with the given <value>Id</value>
        /// </summary>
        /// <param name="id">The Id of the desired element</param>
        /// <returns>The element with the given Id</returns>
        public async Task<TEntity> SingleOrDefaultAsync(TKey id)
        {
            return await this.Repository.SingleOrDefaultAsync(id);
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
        public async Task<TEntity> UpdateAsync(TEntity obj)
        {
            return await this.Repository.UpdateAsync(obj);
        }

        /// <summary>
        ///     Asynchronously begins tracking objects given in <paramref name="obj"/>
        /// <remarks>
        ///     All the properties will be marked
        ///     as modified. To mark only some properties use the
        ///     <see cref="M:Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0)"/>
        /// </remarks>
        /// </summary>
        /// <param name="obj">The objects to be marked</param>
        /// <returns>The given <paramref name="obj"/> after being inserted</returns>
        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> obj)
        {
            return await this.Repository.UpdateRangeAsync(obj);
        }

        #region IDisposable Support

        /// <summary>
        ///     Release the allocated resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Release the allocated resources
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
                    this.Repository.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                Repository = null;
                this.disposedValue = true;
            }
        }

        #endregion
    }
}