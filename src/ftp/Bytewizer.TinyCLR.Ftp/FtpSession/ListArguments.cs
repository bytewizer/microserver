//namespace Bytewizer.TinyCLR.Ftp
//{
//    public class ListArguments
//    {
//        public ListArguments(string arguments)
//        {
//            foreach (var arg in arguments.ToCharArray())
//            {                
//                if (arg.ToLower() == 'l')
//                {
//                    PreferLong = true;
//                }
//                if (arg.ToLower() == 'r')
//                {
//                    Recursive = true;
//                }
//                if (arg.ToLower() == 'a')
//                {
//                    All = true;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets a value indicating whether <c>LIST</c> returns all entries (including <c>.</c> and <c>..</c>).
//        /// </summary>
//        public bool All { get; }

//        /// <summary>
//        /// Gets a value indicating whether <c>LIST</c> returns all file system entries recursively.
//        /// </summary>
//        public bool Recursive { get; }

//        /// <summary>
//        /// Gets a value indicating whether the long output is preferred.
//        /// </summary>
//        public bool PreferLong { get; }
//    }
//}