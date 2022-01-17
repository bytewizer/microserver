using System.Collections;

namespace Bytewizer.TinyCLR.Terminal.Channel
{
    /// <summary>
    /// OutputBuffer provides a server side output buffer.
    /// </summary>
    /// <remarks>
    /// Used for paging, to avoid flooding users with too many lines in one go.
    /// </remarks>
    public class OutputBuffer
    {
        /// <summary>
        /// Current location in the buffer.
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// List of output rows.
        /// </summary>
        private readonly ArrayList _outputBuffer = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBuffer"/> class.
        /// </summary>
        public OutputBuffer() { }

        /// <summary>
        /// Gets the total length of the output buffer.
        /// </summary>
        public int Length
        {
            get { return _outputBuffer.Count; }
        }

        /// <summary>
        /// Gets the current location in the output buffer.
        /// </summary>
        public int CurrentLocation { get; private set; } = 0;

        /// <summary>
        /// Gets a value indicating whether there is more data available.
        /// </summary>
        public bool HasMoreData
        {
            get
            {
                lock (_lockObject)
                {
                    return CurrentLocation < _outputBuffer.Count;
                }
            }
        }

        public void Set(ArrayList newOutputRows)
        {
            lock (_lockObject)
            {
                _outputBuffer.Clear();
                _outputBuffer.AddRange(newOutputRows);
                CurrentLocation = 0;
            }
        }

        /// <summary>
        /// Returns the requested rows from the Output Buffer.
        /// </summary>
        /// <param name="bufferDirection">The direction to move in the buffer.</param>
        /// <param name="maxRows">The maximum rows to return.</param>
        /// <returns>Rows from the output buffer.</returns>
        public string[] GetRows(BufferDirection bufferDirection, int maxRows)
        {
            lock (_lockObject)
            {
                int start = CurrentLocation;
                int end = 0;

                switch (bufferDirection)
                {
                    case BufferDirection.Repeat:
                        start = start - maxRows;
                        if (start < 0)
                        {
                            start = 0;
                        }

                        end = start + maxRows;
                        break;
                    case BufferDirection.Backward:
                        start = start - (2 * maxRows);
                        if (start < 0)
                        {
                            start = 0;
                        }

                        end = start + maxRows;
                        break;
                    case BufferDirection.Forward:
                        end = start + maxRows;
                        break;
                    case BufferDirection.ForwardAllData:
                        end = _outputBuffer.Count;
                        break;
                }

                if (end > _outputBuffer.Count)
                {
                    end = _outputBuffer.Count;
                }

                string[] output = new string[end - start];

                for (int i = start; i < end; i++)
                {
                    output[i - start] = (string)_outputBuffer[i];
                }

                CurrentLocation = end;
                return output;
            }
        }
    }
}