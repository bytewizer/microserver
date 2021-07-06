using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents the result of an identity operation.
    /// </summary>
    public class IdentityResult
    {
        private ArrayList _errors;

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="ArrayList"/> of <see cref="Exception"/> containing errors
        /// that occurred during the identity operation.
        /// </summary>
        public ArrayList Errors => _errors;

        /// <summary>
        /// Returns an <see cref="IdentityResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="IdentityResult"/> indicating a successful operation.</returns>
        public static IdentityResult Success => new IdentityResult { Succeeded = true };

        /// <summary>
        /// Creates an <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="error"/> if applicable.
        /// </summary>
        /// <param name="error">An <see cref="Exception"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="error"/> if applicable.</returns>
        public static IdentityResult Failed(Exception error)
        {
            return Failed(new Exception[1] { error });
        }

        /// <summary>
        /// Creates an <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An <see cref="Exception"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static IdentityResult Failed(ArrayList errors)
        {
            return Failed((Exception[])errors.ToArray(typeof(Exception)));
        }

        /// <summary>
        /// Creates an <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Exception"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static IdentityResult Failed(params Exception[] errors)
        {
            var result = new IdentityResult { Succeeded = false };
            if (errors != null)
            {
                result._errors = new ArrayList(errors);
            }
            return result;
        }
    }
}
