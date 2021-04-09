//using System;
//using System.Collections;
//using System.Text;
//using System.Threading;

//namespace Bytewizer.TinyCLR.Http.WebSockets.Internal
//{
//    internal static class HandshakeHelper
//    {
//    public static string CreateResponseKey(string requestKey)
//        {
//            // "The value of this header field is constructed by concatenating /key/, defined above in step 4
//            // in Section 4.2.2, with the string "258EAFA5- E914-47DA-95CA-C5AB0DC85B11", taking the SHA-1 hash of
//            // this concatenated value to obtain a 20-byte value and base64-encoding"
//            // https://tools.ietf.org/html/rfc6455#section-4.2.2

//            if (requestKey == null)
//            {
//                throw new ArgumentNullException(nameof(requestKey));
//            }

//            //using (var algorithm = SHA1.Create())
//            //{
//            //    string merged = requestKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
//            //    byte[] mergedBytes = Encoding.UTF8.GetBytes(merged);
//            //    byte[] hashedBytes = algorithm.ComputeHash(mergedBytes);
//            //    return Convert.ToBase64String(hashedBytes);
//            //}
//        }
    
//    }
//}
