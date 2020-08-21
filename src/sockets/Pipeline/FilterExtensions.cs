namespace Bytewizer.TinyCLR.Sockets
{
    public static class FilterExtensions
    {
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