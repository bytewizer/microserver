//using System;
//using System.Text;

//using GHIElectronics.TinyCLR.Cryptography;

//namespace Bytewizer.TinyCLR.Identity
//{
//    /// <summary>
//    /// Extension methods for <see cref="UserStore"/> related to "A1" MD5 hashing.
//    /// </summary>
//    public static class UserStoreExtensions
//    {
//        /// <summary>
//        /// The default realm used by authentication store.
//        /// </summary>
//        public static string DefaultRealm = "TinyCLR";

//        /// <summary>
//        /// Creates the specified user using "A1" MD5 hashing in the user store.
//        /// </summary>
//        /// <param name="store">The <see cref="UserStore"/> store.</param>
//        /// <param name="username">The user name to create.</param>
//        /// <param name="password">The password used to create the new user.</param>
//        public static void Create(this UserStore store, string username, string password)
//        {
//            store.Create(username, DefaultRealm, password);
//        }

//        /// <summary>
//        /// Creates the specified user using "A1" MD5 hashing in the user store.
//        /// </summary>
//        /// <param name="store">The <see cref="UserStore"/> store.</param>
//        /// <param name="username">The user name to create.</param>
//        /// <param name="realm">The realm so they know which username and password to use.</param>
//        /// <param name="password">The password used to create the new user.</param>
//        public static void Create(this UserStore store, string username, string realm, string password)
//        {
//            if (store == null)
//            {
//                throw new ArgumentNullException(nameof(store));
//            }

//            if (string.IsNullOrEmpty(username))
//            {
//                throw new ArgumentNullException(nameof(username));
//            }

//            if (string.IsNullOrEmpty(realm))
//            {
//                throw new ArgumentNullException(nameof(realm));
//            }

//            if (string.IsNullOrEmpty(password))
//            {
//                throw new ArgumentNullException(nameof(password));
//            }

//            var encodedBytes = Encoding.UTF8.GetBytes($"{username}:{realm}:{password}");

//            var user = new IdentityUser()
//            {
//                Id = DateTime.Now.Ticks.ToString(), // TODO: Replace with GUID
//                UserName = username,
//                PasswordHash = ComputeHash(encodedBytes)
//            };

//            store.Create(user);
//        }

//        private static string ComputeHash(byte[] hash)
//        {
//            using (var md5 = MD5.Create())
//            {
//                var hashBytes = md5.ComputeHash(hash);

//                var sb = new StringBuilder(64);
//                foreach (var bytes in hashBytes)
//                {
//                    sb.Append($"{bytes:x02}");
//                }

//                return sb.ToString();
//            }
//        }
//    }
//}
