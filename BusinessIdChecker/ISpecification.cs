using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessIdentificationChecker
{
    /// <summary>
    /// Interface for business id check
    /// </summary>
    public interface ISpecification<in TEntity>
    {
        /// <summary>
        /// Read reasons for business id dissatisfactions.
        /// </summary>
        /// <returns>
        /// Returns IEnumerable string
        /// </returns>
        IEnumerable<string> ReasonsForDissatisfaction { get; }

        /// <summary>
        /// Method to check the satisfaction conditions
        /// </summary>
        /// <param name="entity">
        /// Parameter entity requires a TEntity argument.
        /// </param>
        /// <returns>
        /// The method returns a boolean.
        /// </returns>
        bool IsSatisfiedBy(TEntity entity);
    }
}
