using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfDrones.Core.Domain.Base
{
    /// <summary>
    ///     Declare the properties to track the time of
    ///     the creation and modification of the entities.
    /// </summary>
    public abstract class TrackableEntity<TKey> : Entity<TKey>
    {
        /// <summary>
        ///     Gets or sets the time of creation of this entity
        /// </summary>
        public DateTime CreatedAt { get; set; }


        /// <summary>
        ///     Gets or sets the time of modification of this entity
        /// </summary>
        public DateTime ModifiedAt { get; set; }
    }
}