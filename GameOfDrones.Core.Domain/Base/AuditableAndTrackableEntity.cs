using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfDrones.Core.Domain.Base
{
    /// <summary>
    ///     Entity to track the creator, editor and the time
    ///     of creation and modification of the entities
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AuditableAndTrackableEntity<TKey> : Entity<TKey>
    {
        /// <summary>
        /// Get or sets the author of the Instance of the entity
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the editor of the instance of the entity
        /// </summary>
        public string ModifiedBy { get; set; }

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