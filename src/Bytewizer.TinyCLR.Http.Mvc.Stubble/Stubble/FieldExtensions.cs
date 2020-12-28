namespace Bytewizer.TinyCLR.Stubble
{
    internal static class FieldExtensions
    {
        public static int[] Append(this int[] array, int filter)
        {
            if (array == null)
            {
                return new int[] { filter };
            }

            int[] result = new int[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }

            result[array.Length] = filter;
            return result;
        }
    }
}
