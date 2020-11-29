namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Contains extension methods for <see cref="FilterDelegate"/>.
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Inserts the <see cref="FilterDelegate"/> as the last node in the collection.
        /// </summary>
        /// <param name="array">The <see cref="FilterDelegate"/> array of filters.</param>
        /// <param name="filter">The <see cref="FilterDelegate"/> to insert.</param>
        public static FilterDelegate[] Append(this FilterDelegate[] array, FilterDelegate filter)
        {
            if (array == null)
            {
                return new FilterDelegate[] { filter };
            }

            FilterDelegate[] result = new FilterDelegate[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }

            result[array.Length] = filter;
            return result;
        }
    }
}