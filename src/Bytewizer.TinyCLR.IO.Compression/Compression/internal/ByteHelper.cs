namespace Bytewizer.TinyCLR.IO.Compression
{
	internal class ByteHelper
	{
		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}
	}
}
