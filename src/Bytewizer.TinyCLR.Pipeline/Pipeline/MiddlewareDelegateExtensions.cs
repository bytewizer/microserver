namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Contains extension methods for <see cref="MiddlewareDelegate"/>.
    /// </summary>
    public static class MiddlewareDelegateExtensions
    {
        /// <summary>
        /// Inserts the <see cref="MiddlewareDelegate"/> as the last node in the collection.
        /// </summary>
        /// <param name="array">The <see cref="MiddlewareDelegate"/> array of filters.</param>
        /// <param name="filter">The <see cref="MiddlewareDelegate"/> to insert.</param>
        public static MiddlewareDelegate[] Append(this MiddlewareDelegate[] array, MiddlewareDelegate filter)
        {
            if (array == null)
            {
                return new MiddlewareDelegate[] { filter };
            }

            MiddlewareDelegate[] result = new MiddlewareDelegate[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }

            result[array.Length] = filter;
            return result;
        }
    }
}