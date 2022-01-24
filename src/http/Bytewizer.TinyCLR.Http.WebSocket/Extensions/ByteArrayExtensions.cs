using System;

namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for <see cref="byte"/> array object.
    /// </summary>
    public static class ByteArrayExtensions
    {
        ///// <summary>
        ///// Reverses the sequence of the elements in the entire one-dimensional Array.
        ///// </summary>
        ///// <param name="array">The one-dimensional Array to reverse.</param>
        //public static byte[] Reverse(this byte[] array)
        //{
        //    return array.Reverse(0, array.Length - 1);
        //}

        ///// <summary>
        ///// Reverses the sequence of a subset of the elements in the one-dimensional Array.
        ///// </summary>
        ///// <param name="array">The one-dimensional Array to reverse.</param>
        ///// <param name="startIndex">The starting index of the section to reverse.</param>
        ///// <param name="length">The number of elements in the section to reverse.</param>
        //public static byte[] Reverse(this byte[] array, int startIndex, int length)
        //{
        //    for (; startIndex < length; startIndex++, length--)
        //    {
        //        var temp = array[startIndex];
        //        array[startIndex] = array[length];
        //        array[length] = temp;
        //    }

        //    return array;
        //}

        /// <summary>
        /// Converts the order of elements in the specified byte array to
        /// host (this computer architecture) byte order.
        /// </summary>
        /// <returns>
        ///   <para>
        ///   An array of <see cref="byte"/> converted from
        ///   <paramref name="source"/>.
        ///   </para>
        ///   <para>
        ///   <paramref name="source"/> if the number of elements in
        ///   it is less than 2 or <paramref name="sourceOrder"/> is
        ///   same as host byte order.
        ///   </para>
        /// </returns>
        /// <param name="source">
        /// An array of <see cref="byte"/> to convert.
        /// </param>
        /// <param name="sourceOrder">
        ///   <para>
        ///   One of the <see cref="ByteOrder"/> enum values.
        ///   </para>
        ///   <para>
        ///   It specifies the order of elements in <paramref name="source"/>.
        ///   </para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        public static byte[] ToHostOrder(this byte[] source, ByteOrder sourceOrder)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Length < 2)
                return source;

            if (sourceOrder.IsHostOrder())
                return source;

            return source.Reverse();
        }

        /// <summary>
        /// Determines whether the specified byte order is host (this computer
        /// architecture) byte order.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <paramref name="order"/> is host byte order; otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <param name="order">
        /// One of the <see cref="ByteOrder"/> enum values to test.
        /// </param>
        public static bool IsHostOrder(this ByteOrder order)
        {
            // true: !(true ^ true) or !(false ^ false)
            // false: !(true ^ false) or !(false ^ true)
            return !(BitConverter.IsLittleEndian ^ (order == ByteOrder.Little));
        }

        /// <summary>
        /// Retrieves a sub-array from the specified array. A sub-array starts at
        /// the specified index in the array.
        /// </summary>
        /// <returns>
        /// An array of T that receives a sub-array.
        /// </returns>
        /// <param name="array">
        /// An array of T from which to retrieve a sub-array.
        /// </param>
        /// <param name="startIndex">
        /// An <see cref="int"/> that represents the zero-based index in the array
        /// at which retrieving starts.
        /// </param>
        /// <param name="length">
        /// An <see cref="int"/> that represents the number of elements to retrieve.
        /// </param>
        /// <typeparam name="T">
        /// The type of elements in the array.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>
        ///   <paramref name="startIndex"/> is less than zero.
        ///   </para>
        ///   <para>
        ///   -or-
        ///   </para>
        ///   <para>
        ///   <paramref name="startIndex"/> is greater than the end of the array.
        ///   </para>
        ///   <para>
        ///   -or-
        ///   </para>
        ///   <para>
        ///   <paramref name="length"/> is less than zero.
        ///   </para>
        ///   <para>
        ///   -or-
        ///   </para>
        ///   <para>
        ///   <paramref name="length"/> is greater than the number of elements from
        ///   <paramref name="startIndex"/> to the end of the array.
        ///   </para>
        /// </exception>
        public static byte[] SubArray(this byte[] array, int startIndex, int length)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            var len = array.Length;
            if (len == 0)
            {
                if (startIndex != 0)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));

                if (length != 0)
                    throw new ArgumentOutOfRangeException(nameof(length));

                return array;
            }

            if (startIndex < 0 || startIndex >= len)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (length < 0 || length > len - startIndex)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (length == 0)
                return new byte[0];

            if (length == len)
                return array;

            var subArray = new byte[length];
            Array.Copy(array, startIndex, subArray, 0, length);

            return subArray;
        }

        public static byte[] SubArray(this byte[] array, long startIndex, long length)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var len = (long)array.Length;
            if (len == 0)
            {
                if (startIndex != 0)
                    throw new ArgumentOutOfRangeException("startIndex");

                if (length != 0)
                    throw new ArgumentOutOfRangeException("length");

                return array;
            }

            if (startIndex < 0 || startIndex >= len)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length < 0 || length > len - startIndex)
                throw new ArgumentOutOfRangeException("length");

            if (length == 0)
                return new byte[0];

            if (length == len)
                return array;

            var subArray = new byte[length];
            
            //TODO:  This will throw an execption if data is larger then int  
            Array.Copy(array, (int)startIndex, subArray, 0, (int)length);

            return subArray;
        }


        internal static byte[] InternalToByteArray(this ushort value, ByteOrder order
)
        {
            var ret = BitConverter.GetBytes(value);

            if (!order.IsHostOrder())
            {
                ret.Reverse();
            }

            return ret;
        }

        internal static byte[] InternalToByteArray(this ulong value, ByteOrder order)
        {
            var ret = BitConverter.GetBytes(value);

            if (!order.IsHostOrder())
            {
                ret.Reverse();
            }

            return ret;
        }

        internal static ushort ToUInt16(this byte[] source, ByteOrder sourceOrder)
        {
            return BitConverter.ToUInt16(source.ToHostOrder(sourceOrder), 0);
        }

        internal static ulong ToUInt64(this byte[] source, ByteOrder sourceOrder)
        {
            return BitConverter.ToUInt64(source.ToHostOrder(sourceOrder), 0);
        }

        internal static bool IsReserved(this ushort code)
        {
            return code == 1004
                   || code == 1005
                   || code == 1006
                   || code == 1015;
        }

    }
}
