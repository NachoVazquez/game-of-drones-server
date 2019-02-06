using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfDrones.Core.Domain.Base
{
    /// <summary>
    ///     Declare the PK for all the entities of the system.
    /// </summary>
    /// <typeparam name="TKey">The type of the PK</typeparam>
    public abstract class Entity<TKey>
    {
        /// <summary>
        ///     Gets or sets the PK of the entity
        /// </summary>
        public TKey Id { get; set; }
    }
}