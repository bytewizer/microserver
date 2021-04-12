using System;

namespace GHIElectronics.TinyCLR.Cryptography {
    /**
         * implementation of SHA-1 as outlined in "Handbook of Applied Cryptography", pages 346 - 349.
         *
         * It is interesting to ponder why the, apart from the extra IV, the other difference here from MD5
         * is the "endianness" of the word processing!
         */
    public class SHA1 {
        private const int DigestLength = 20;
        private byte[] hash;
        private uint H1, H2, H3, H4, H5;

        private uint[] X = new uint[80];

        private byte[] xBuf;

        private int xBufOff;
        private int xOff;
        private long byteCount;

        public byte[] Hash => this.hash;
        private SHA1() {
            this.xBuf = new byte[4];
            this.hash = new byte[DigestLength];

            this.Clear();
        }

        public static SHA1 Create() => new SHA1();

        internal void ProcessWord(
            byte[] input,
            int inOff) {
            this.X[this.xOff] = BE_To_UInt32(input, inOff);

            if (++this.xOff == 16) {
                this.ProcessBlock();
            }
        }

        internal void ProcessLength(long bitLength) {
            if (this.xOff > 14) {
                this.ProcessBlock();
            }

            this.X[14] = (uint)((ulong)bitLength >> 32);
            this.X[15] = (uint)((ulong)bitLength);
        }

        internal void Update(byte input) {
            this.xBuf[this.xBufOff++] = input;

            if (this.xBufOff == this.xBuf.Length) {
                this.ProcessWord(this.xBuf, 0);
                this.xBufOff = 0;
            }

            this.byteCount++;
        }

        public byte[] ComputeHash(byte[] buffer) => this.ComputeHash(buffer, 0, buffer.Length);

        public byte[] ComputeHash(byte[] buffer, int offset, int count) {
            if (buffer == null) {
                throw new ArgumentNullException();
            }

            if (offset + count > buffer.Length) {
                throw new ArgumentOutOfRangeException();
            }

            this.BlockUpdate(buffer, offset, count);

            this.DoFinal(this.hash, 0);

            return this.hash;
        }

        internal void Finish() {
            var bitLength = (this.byteCount << 3);

            //
            // add the pad bytes.
            //
            this.Update((byte)128);

            while (this.xBufOff != 0)
                this.Update((byte)0);

            this.ProcessLength(bitLength);
            this.ProcessBlock();
        }
        internal int DoFinal(byte[] output, int outOff) {
            this.Finish();

            UInt32_To_BE(this.H1, output, outOff + 0);
            UInt32_To_BE(this.H2, output, outOff + 4);
            UInt32_To_BE(this.H3, output, outOff + 8);
            UInt32_To_BE(this.H4, output, outOff + 12);
            UInt32_To_BE(this.H5, output, outOff + 16);

            this.Clear();

            return DigestLength;
        }

        internal void BlockUpdate(
            byte[] input,
            int inOff,
            int length) {
            length = System.Math.Max(0, length);

            //
            // fill the current word
            //
            var i = 0;
            if (this.xBufOff != 0) {
                while (i < length) {
                    this.xBuf[this.xBufOff++] = input[inOff + i++];
                    if (this.xBufOff == 4) {
                        this.ProcessWord(this.xBuf, 0);
                        this.xBufOff = 0;
                        break;
                    }
                }
            }

            //
            // process whole words.
            //
            var limit = ((length - i) & ~3) + i;
            for (; i < limit; i += 4) {
                this.ProcessWord(input, inOff + i);
            }

            //
            // load in the remainder.
            //
            while (i < length) {
                this.xBuf[this.xBufOff++] = input[inOff + i++];
            }

            this.byteCount += length;
        }

        /**
         * reset the chaining variables
         */
        public void Clear() {

            this.byteCount = 0;
            this.xBufOff = 0;
            Array.Clear(this.xBuf, 0, this.xBuf.Length);

            this.H1 = 0x67452301;
            this.H2 = 0xefcdab89;
            this.H3 = 0x98badcfe;
            this.H4 = 0x10325476;
            this.H5 = 0xc3d2e1f0;

            this.xOff = 0;
            Array.Clear(this.X, 0, this.X.Length);
        }

        //
        // Additive constants
        //
        private const uint Y1 = 0x5a827999;
        private const uint Y2 = 0x6ed9eba1;
        private const uint Y3 = 0x8f1bbcdc;
        private const uint Y4 = 0xca62c1d6;

        private static uint F(uint u, uint v, uint w) => (u & v) | (~u & w);

        private static uint H(uint u, uint v, uint w) => u ^ v ^ w;

        private static uint G(uint u, uint v, uint w) => (u & v) | (u & w) | (v & w);

        internal void ProcessBlock() {
            //
            // expand 16 word block into 80 word block.
            //
            for (var i = 16; i < 80; i++) {
                var t = this.X[i - 3] ^ this.X[i - 8] ^ this.X[i - 14] ^ this.X[i - 16];
                this.X[i] = t << 1 | t >> 31;
            }

            //
            // set up working variables.
            //
            var A = this.H1;
            var B = this.H2;
            var C = this.H3;
            var D = this.H4;
            var E = this.H5;

            //
            // round 1
            //
            var idx = 0;

            for (var j = 0; j < 4; j++) {
                // E = rotateLeft(A, 5) + F(B, C, D) + E + X[idx++] + Y1
                // B = rotateLeft(B, 30)
                E += (A << 5 | (A >> 27)) + F(B, C, D) + this.X[idx++] + Y1;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + F(A, B, C) + this.X[idx++] + Y1;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + F(E, A, B) + this.X[idx++] + Y1;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + F(D, E, A) + this.X[idx++] + Y1;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + F(C, D, E) + this.X[idx++] + Y1;
                C = C << 30 | (C >> 2);
            }

            //
            // round 2
            //
            for (var j = 0; j < 4; j++) {
                // E = rotateLeft(A, 5) + H(B, C, D) + E + X[idx++] + Y2
                // B = rotateLeft(B, 30)
                E += (A << 5 | (A >> 27)) + H(B, C, D) + this.X[idx++] + Y2;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + H(A, B, C) + this.X[idx++] + Y2;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + H(E, A, B) + this.X[idx++] + Y2;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + H(D, E, A) + this.X[idx++] + Y2;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + H(C, D, E) + this.X[idx++] + Y2;
                C = C << 30 | (C >> 2);
            }

            //
            // round 3
            //
            for (var j = 0; j < 4; j++) {
                // E = rotateLeft(A, 5) + G(B, C, D) + E + X[idx++] + Y3
                // B = rotateLeft(B, 30)
                E += (A << 5 | (A >> 27)) + G(B, C, D) + this.X[idx++] + Y3;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + G(A, B, C) + this.X[idx++] + Y3;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + G(E, A, B) + this.X[idx++] + Y3;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + G(D, E, A) + this.X[idx++] + Y3;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + G(C, D, E) + this.X[idx++] + Y3;
                C = C << 30 | (C >> 2);
            }

            //
            // round 4
            //
            for (var j = 0; j < 4; j++) {
                // E = rotateLeft(A, 5) + H(B, C, D) + E + X[idx++] + Y4
                // B = rotateLeft(B, 30)
                E += (A << 5 | (A >> 27)) + H(B, C, D) + this.X[idx++] + Y4;
                B = B << 30 | (B >> 2);

                D += (E << 5 | (E >> 27)) + H(A, B, C) + this.X[idx++] + Y4;
                A = A << 30 | (A >> 2);

                C += (D << 5 | (D >> 27)) + H(E, A, B) + this.X[idx++] + Y4;
                E = E << 30 | (E >> 2);

                B += (C << 5 | (C >> 27)) + H(D, E, A) + this.X[idx++] + Y4;
                D = D << 30 | (D >> 2);

                A += (B << 5 | (B >> 27)) + H(C, D, E) + this.X[idx++] + Y4;
                C = C << 30 | (C >> 2);
            }

            this.H1 += A;
            this.H2 += B;
            this.H3 += C;
            this.H4 += D;
            this.H5 += E;

            //
            // reset start of the buffer.
            //
            this.xOff = 0;

            Array.Clear(this.X, 0, 16);
        }

        internal static void UInt32_To_BE(uint n, byte[] bs, int off) {
            bs[off] = (byte)(n >> 24);
            bs[off + 1] = (byte)(n >> 16);
            bs[off + 2] = (byte)(n >> 8);
            bs[off + 3] = (byte)(n);
        }

        internal static uint BE_To_UInt32(byte[] bs, int off) => (uint)bs[off] << 24
                | (uint)bs[off + 1] << 16
                | (uint)bs[off + 2] << 8
                | (uint)bs[off + 3];
    }
}
