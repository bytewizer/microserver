namespace Bytewizer.TinyCLR.Http.Mvc
{
    public class BadRequestResult : StatusCodeResult
    {
        private const int DefaultStatusCode = StatusCodes.Status400BadRequest;

        /// <summary>
        /// Creates a new <see cref="BadRequestResult"/> instance.
        /// </summary>
        public BadRequestResult()
            : base(DefaultStatusCode)
        {
        }
    }
}
