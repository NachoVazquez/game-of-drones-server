using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfDrones.Core.Domain.Base
{
    /// <summary>
    ///     Declare the properties for the AuditableEntities.
    ///     This is to track the the user that modify and create
    ///     some entity
    /// </summary>
    public abstract class AuditableEntity<TKey> : Entity<TKey>
    {
        /// <summary>
        /// Get or sets the author of the Instance of the entity
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the editor of the instance of the entity
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}