using System;

namespace Bytewizer.TinyCLR.IO.Compression
{
    internal sealed class InflateBlocks
    {
        internal static readonly int[] border = new int[]
            { 16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15 };

        private enum InflateBlockMode
        {
            TYPE = 0,                       // get type bits (3, including end bit)
            LENS = 1,                       // get lengths for stored
            STORED = 2,                     // processing stored block
            TABLE = 3,                      // get table lengths
            BTREE = 4,                      // get bit lengths tree for a dynamic block
            DTREE = 5,                      // get length, distance trees for a dynamic block
            CODES = 6,                      // processing fixed or dynamic block
            DRY = 7,                        // output remaining window bytes
            DONE = 8,                       // finished last block, done
            BAD = 9,                        // ot a data error--stuck here
        }

        //private const int MANY = 1440;


        private InflateBlockMode mode;

        internal int left;

        internal int table;

        internal int index;

        internal int[] blens;

        internal int[] bb = new int[1];

        internal int[] tb = new int[1];

        internal InflateCodes codes = new InflateCodes();

        internal int last;

        internal ZlibCodec _codec;

        internal int bitk;

        internal int bitb;

        internal int[] hufts;

        internal byte[] window;

        internal int end;

        internal int readAt;

        internal int writeAt;

        internal object checkfn;

        internal uint check;

        internal InfTree inftree = new InfTree();

        internal InflateBlocks(ZlibCodec codec, object checkfn, int w)
        {
            _codec = codec;
            hufts = new int[4320];
            window = new byte[w];
            end = w;
            this.checkfn = checkfn;
            mode = InflateBlockMode.TYPE;
            Reset();
        }

        internal uint Reset()
        {
            uint result = check;
            mode = InflateBlockMode.TYPE;
            bitk = 0;
            bitb = 0;
            readAt = (writeAt = 0);

            if (checkfn != null)
            {
                _codec._Adler32 = (check = Adler.Adler32(0u, null, 0, 0));
            }
            return result;
        }

        //internal int Process(int r)
        //{
        //    int num = _codec.NextIn;
        //    int num2 = _codec.AvailableBytesIn;
        //    int num3 = bitb;
        //    int i = bitk;
        //    int num4 = writeAt;
        //    int num5 = ((num4 < readAt) ? (readAt - num4 - 1) : (end - num4));
        //    while (true)
        //    {
        //        bool flag = true;
        //        switch (mode)
        //        {
        //            case InflateBlockMode.TYPE:
        //                {
        //                    for (; i < 3; i += 8)
        //                    {
        //                        if (num2 != 0)
        //                        {
        //                            r = 0;
        //                            num2--;
        //                            num3 |= (_codec.InputBuffer[num++] & 0xFF) << i;
        //                            continue;
        //                        }
        //                        bitb = num3;
        //                        bitk = i;
        //                        _codec.AvailableBytesIn = num2;
        //                        _codec.TotalBytesIn += num - _codec.NextIn;
        //                        _codec.NextIn = num;
        //                        writeAt = num4;
        //                        return Flush(r);
        //                    }
        //                    int num6 = num3 & 7;
        //                    last = num6 & 1;
        //                    switch ((uint)num6 >> 1)
        //                    {
        //                        case 0u:
        //                            num3 >>= 3;
        //                            i -= 3;
        //                            num6 = i & 7;
        //                            num3 >>= num6;
        //                            i -= num6;
        //                            mode = InflateBlockMode.LENS;
        //                            break;
        //                        case 1u:
        //                            {
        //                                int[] array = new int[1];
        //                                int[] array2 = new int[1];
        //                                int[][] array5 = new int[1][];
        //                                int[][] array6 = new int[1][];
        //                                InfTree.inflate_trees_fixed(array, array2, array5, array6, _codec);
        //                                codes.Init(array[0], array2[0], array5[0], 0, array6[0], 0);
        //                                num3 >>= 3;
        //                                i -= 3;
        //                                mode = InflateBlockMode.CODES;
        //                                break;
        //                            }
        //                        case 2u:
        //                            num3 >>= 3;
        //                            i -= 3;
        //                            mode = InflateBlockMode.TABLE;
        //                            break;
        //                        case 3u:
        //                            num3 >>= 3;
        //                            i -= 3;
        //                            mode = InflateBlockMode.BAD;
        //                            _codec.Message = "invalid block type";
        //                            r = -3;
        //                            bitb = num3;
        //                            bitk = i;
        //                            _codec.AvailableBytesIn = num2;
        //                            _codec.TotalBytesIn += num - _codec.NextIn;
        //                            _codec.NextIn = num;
        //                            writeAt = num4;
        //                            return Flush(r);
        //                    }
        //                    break;
        //                }
        //            case InflateBlockMode.LENS:
        //                for (; i < 32; i += 8)
        //                {
        //                    if (num2 != 0)
        //                    {
        //                        r = 0;
        //                        num2--;
        //                        num3 |= (_codec.InputBuffer[num++] & 0xFF) << i;
        //                        continue;
        //                    }
        //                    bitb = num3;
        //                    bitk = i;
        //                    _codec.AvailableBytesIn = num2;
        //                    _codec.TotalBytesIn += num - _codec.NextIn;
        //                    _codec.NextIn = num;
        //                    writeAt = num4;
        //                    return Flush(r);
        //                }
        //                if (((~num3 >> 16) & 0xFFFF) != (num3 & 0xFFFF))
        //                {
        //                    mode = InflateBlockMode.BAD;
        //                    _codec.Message = "invalid stored block lengths";
        //                    r = -3;
        //                    bitb = num3;
        //                    bitk = i;
        //                    _codec.AvailableBytesIn = num2;
        //                    _codec.TotalBytesIn += num - _codec.NextIn;
        //                    _codec.NextIn = num;
        //                    writeAt = num4;
        //                    return Flush(r);
        //                }
        //                left = num3 & 0xFFFF;
        //                num3 = (i = 0);
        //                mode = ((left != 0) ? InflateBlockMode.STORED : ((last != 0) ? InflateBlockMode.DRY : InflateBlockMode.TYPE));
        //                break;
        //            case InflateBlockMode.STORED:
        //                {
        //                    if (num2 == 0)
        //                    {
        //                        bitb = num3;
        //                        bitk = i;
        //                        _codec.AvailableBytesIn = num2;
        //                        _codec.TotalBytesIn += num - _codec.NextIn;
        //                        _codec.NextIn = num;
        //                        writeAt = num4;
        //                        return Flush(r);
        //                    }
        //                    if (num5 == 0)
        //                    {
        //                        if (num4 == end && readAt != 0)
        //                        {
        //                            num4 = 0;
        //                            num5 = ((num4 < readAt) ? (readAt - num4 - 1) : (end - num4));
        //                        }
        //                        if (num5 == 0)
        //                        {
        //                            writeAt = num4;
        //                            r = Flush(r);
        //                            num4 = writeAt;
        //                            num5 = ((num4 < readAt) ? (readAt - num4 - 1) : (end - num4));
        //                            if (num4 == end && readAt != 0)
        //                            {
        //                                num4 = 0;
        //                                num5 = ((num4 < readAt) ? (readAt - num4 - 1) : (end - num4));
        //                            }
        //                            if (num5 == 0)
        //                            {
        //                                bitb = num3;
        //                                bitk = i;
        //                                _codec.AvailableBytesIn = num2;
        //                                _codec.TotalBytesIn += num - _codec.NextIn;
        //                                _codec.NextIn = num;
        //                                writeAt = num4;
        //                                return Flush(r);
        //                            }
        //                        }
        //                    }
        //                    r = 0;
        //                    int num6 = left;
        //                    if (num6 > num2)
        //                    {
        //                        num6 = num2;
        //                    }
        //                    if (num6 > num5)
        //                    {
        //                        num6 = num5;
        //                    }
        //                    Array.Copy(_codec.InputBuffer, num, window, num4, num6);
        //                    num += num6;
        //                    num2 -= num6;
        //                    num4 += num6;
        //                    num5 -= num6;
        //                    if ((left -= num6) == 0)
        //                    {
        //                        mode = ((last != 0) ? InflateBlockMode.DRY : InflateBlockMode.TYPE);
        //                    }
        //                    break;
        //                }
        //            case InflateBlockMode.TABLE:
        //                {
        //                    for (; i < 14; i += 8)
        //                    {
        //                        if (num2 != 0)
        //                        {
        //                            r = 0;
        //                            num2--;
        //                            num3 |= (_codec.InputBuffer[num++] & 0xFF) << i;
        //                            continue;
        //                        }
        //                        bitb = num3;
        //                        bitk = i;
        //                        _codec.AvailableBytesIn = num2;
        //                        _codec.TotalBytesIn += num - _codec.NextIn;
        //                        _codec.NextIn = num;
        //                        writeAt = num4;
        //                        return Flush(r);
        //                    }
        //                    int num6 = (table = num3 & 0x3FFF);
        //                    if ((num6 & 0x1F) > 29 || ((num6 >> 5) & 0x1F) > 29)
        //                    {
        //                        mode = InflateBlockMode.BAD;
        //                        _codec.Message = "too many length or distance symbols";
        //                        r = -3;
        //                        bitb = num3;
        //                        bitk = i;
        //                        _codec.AvailableBytesIn = num2;
        //                        _codec.TotalBytesIn += num - _codec.NextIn;
        //                        _codec.NextIn = num;
        //                        writeAt = num4;
        //                        return Flush(r);
        //                    }
        //                    num6 = 258 + (num6 & 0x1F) + ((num6 >> 5) & 0x1F);
        //                    if (blens == null || blens.Length < num6)
        //                    {
        //                        blens = new int[num6];
        //                    }
        //                    else
        //                    {
        //                        Array.Clear(blens, 0, num6);
        //                    }
        //                    num3 >>= 14;
        //                    i -= 14;
        //                    index = 0;
        //                    mode = InflateBlockMode.BTREE;
        //                    goto case InflateBlockMode.BTREE;
        //                }
        //            case InflateBlockMode.BTREE:
        //                {
        //                    while (index < 4 + (table >> 10))
        //                    {
        //                        for (; i < 3; i += 8)
        //                        {
        //                            if (num2 != 0)
        //                            {
        //                                r = 0;
        //                                num2--;
        //                                num3 |= (_codec.InputBuffer[num++] & 0xFF) << i;
        //                                continue;
        //                            }
        //                            bitb = num3;
        //                            bitk = i;
        //                            _codec.AvailableBytesIn = num2;
        //                            _codec.TotalBytesIn += num - _codec.NextIn;
        //                            _codec.NextIn = num;
        //                            writeAt = num4;
        //                            return Flush(r);
        //                        }
        //                        blens[border[index++]] = num3 & 7;
        //                        num3 >>= 3;
        //                        i -= 3;
        //                    }
        //                    while (index < 19)
        //                    {
        //                        blens[border[index++]] = 0;
        //                    }
        //                    bb[0] = 7;
        //                    int num6 = inftree.inflate_trees_bits(blens, bb, tb, hufts, _codec);
        //                    if (num6 != 0)
        //                    {
        //                        r = num6;
        //                        if (r == -3)
        //                        {
        //                            blens = null;
        //                            mode = InflateBlockMode.BAD;
        //                        }
        //                        bitb = num3;
        //                        bitk = i;
        //                        _codec.AvailableBytesIn = num2;
        //                        _codec.TotalBytesIn += num - _codec.NextIn;
        //                        _codec.NextIn = num;
        //                        writeAt = num4;
        //                        return Flush(r);
        //                    }
        //                    index = 0;
        //                    mode = InflateBlockMode.DTREE;
        //                    goto case InflateBlockMode.DTREE;
        //                }
        //            case InflateBlockMode.DTREE:
        //                {
        //                    int num6;
        //                    while (true)
        //                    {
        //                        flag = true;
        //                        num6 = table;
        //                        if (index >= 258 + (num6 & 0x1F) + ((num6 >> 5) & 0x1F))
        //                        {
        //                            break;
        //                        }
        //                        for (num6 = bb[0]; i < num6; i += 8)
        //                        {
        //                            if (num2 != 0)
        //                            {
        //                                r = 0;
        //                                num2--;
        //                                num3 |= (_codec.InputBuffer[num++] & 0xFF) << i;
        //                                continue;
        //                            }
        //                            bitb = num3;
        //                            bitk = i;
        //                            _codec.AvailableBytesIn = num2;
        //                            _codec.TotalBytesIn += num - _codec.NextIn;
        //                            _codec.NextIn = num;
        //                            writeAt = num4;
        //                            return Flush(r);
        //                        }
        //                        num6 = hufts[(tb[0] + (num3 & InternalInflateConstants.InflateMask[num6])) * 3 + 1];
        //                        int num7 = hufts[(tb[0] + (num3 & InternalInflateConstants.InflateMask[num6])) * 3 + 2];
        //                        if (num7 < 16)
        //                        {
        //                            num3 >>= num6;
        //                            i -= num6;
        //                            blens[index++] = num7;
        //                            continue;
        //                        }
        //                        int num8 = ((num7 == 18) ? 7 : (num7 - 14));
        //                        int num9 = ((num7 == 18) ? 11 : 3);
        //                        for (; i < num6 + num8; i += 8)
        //                        {
        //                            if (num2 != 0)
        //                            {
        //                                r = 0;
        //                                num2--;
        //                                num3 |= (_codec.InputBuffer[num++] & 0xFF) << i;
        //                                continue;
        //                            }
        //                            bitb = num3;
        //                            bitk = i;
        //                            _codec.AvailableBytesIn = num2;
        //                            _codec.TotalBytesIn += num - _codec.NextIn;
        //                            _codec.NextIn = num;
        //                            writeAt = num4;
        //                            return Flush(r);
        //                        }
        //                        num3 >>= num6;
        //                        i -= num6;
        //                        num9 += num3 & InternalInflateConstants.InflateMask[num8];
        //                        num3 >>= num8;
        //                        i -= num8;
        //                        num8 = index;
        //                        num6 = table;
        //                        if (num8 + num9 > 258 + (num6 & 0x1F) + ((num6 >> 5) & 0x1F) || (num7 == 16 && num8 < 1))
        //                        {
        //                            blens = null;
        //                            mode = InflateBlockMode.BAD;
        //                            _codec.Message = "invalid bit length repeat";
        //                            r = -3;
        //                            bitb = num3;
        //                            bitk = i;
        //                            _codec.AvailableBytesIn = num2;
        //                            _codec.TotalBytesIn += num - _codec.NextIn;
        //                            _codec.NextIn = num;
        //                            writeAt = num4;
        //                            return Flush(r);
        //                        }
        //                        num7 = ((num7 == 16) ? blens[num8 - 1] : 0);
        //                        do
        //                        {
        //                            blens[num8++] = num7;
        //                        }
        //                        while (--num9 != 0);
        //                        index = num8;
        //                    }
        //                    tb[0] = -1;
        //                    int[] array = new int[1]
        //                    {
        //                9
        //                    };
        //                    int[] array2 = new int[1]
        //                    {
        //                6
        //                    };
        //                    int[] array3 = new int[1];
        //                    int[] array4 = new int[1];
        //                    num6 = table;
        //                    num6 = inftree.inflate_trees_dynamic(257 + (num6 & 0x1F), 1 + ((num6 >> 5) & 0x1F), blens, array, array2, array3, array4, hufts, _codec);
        //                    if (num6 != 0)
        //                    {
        //                        if (num6 == -3)
        //                        {
        //                            blens = null;
        //                            mode = InflateBlockMode.BAD;
        //                        }
        //                        r = num6;
        //                        bitb = num3;
        //                        bitk = i;
        //                        _codec.AvailableBytesIn = num2;
        //                        _codec.TotalBytesIn += num - _codec.NextIn;
        //                        _codec.NextIn = num;
        //                        writeAt = num4;
        //                        return Flush(r);
        //                    }
        //                    codes.Init(array[0], array2[0], hufts, array3[0], hufts, array4[0]);
        //                    mode = InflateBlockMode.CODES;
        //                    goto case InflateBlockMode.CODES;
        //                }
        //            case InflateBlockMode.CODES:
        //                bitb = num3;
        //                bitk = i;
        //                _codec.AvailableBytesIn = num2;
        //                _codec.TotalBytesIn += num - _codec.NextIn;
        //                _codec.NextIn = num;
        //                writeAt = num4;
        //                r = codes.Process(this, r);
        //                if (r != 1)
        //                {
        //                    return Flush(r);
        //                }
        //                r = 0;
        //                num = _codec.NextIn;
        //                num2 = _codec.AvailableBytesIn;
        //                num3 = bitb;
        //                i = bitk;
        //                num4 = writeAt;
        //                num5 = ((num4 < readAt) ? (readAt - num4 - 1) : (end - num4));
        //                if (last == 0)
        //                {
        //                    mode = InflateBlockMode.TYPE;
        //                    break;
        //                }
        //                mode = InflateBlockMode.DRY;
        //                goto case InflateBlockMode.DRY;
        //            case InflateBlockMode.DRY:
        //                writeAt = num4;
        //                r = Flush(r);
        //                num4 = writeAt;
        //                num5 = ((num4 < readAt) ? (readAt - num4 - 1) : (end - num4));
        //                if (readAt != writeAt)
        //                {
        //                    bitb = num3;
        //                    bitk = i;
        //                    _codec.AvailableBytesIn = num2;
        //                    _codec.TotalBytesIn += num - _codec.NextIn;
        //                    _codec.NextIn = num;
        //                    writeAt = num4;
        //                    return Flush(r);
        //                }
        //                mode = InflateBlockMode.DONE;
        //                goto case InflateBlockMode.DONE;
        //            case InflateBlockMode.DONE:
        //                r = 1;
        //                bitb = num3;
        //                bitk = i;
        //                _codec.AvailableBytesIn = num2;
        //                _codec.TotalBytesIn += num - _codec.NextIn;
        //                _codec.NextIn = num;
        //                writeAt = num4;
        //                return Flush(r);
        //            case InflateBlockMode.BAD:
        //                r = -3;
        //                bitb = num3;
        //                bitk = i;
        //                _codec.AvailableBytesIn = num2;
        //                _codec.TotalBytesIn += num - _codec.NextIn;
        //                _codec.NextIn = num;
        //                writeAt = num4;
        //                return Flush(r);
        //            default:
        //                r = -2;
        //                bitb = num3;
        //                bitk = i;
        //                _codec.AvailableBytesIn = num2;
        //                _codec.TotalBytesIn += num - _codec.NextIn;
        //                _codec.NextIn = num;
        //                writeAt = num4;
        //                return Flush(r);
        //        }
        //    }
        //}
        internal int Process(int r)
        {
            int t; // temporary storage
            int b; // bit buffer
            int k; // bits in bit buffer
            int p; // input data pointer
            int n; // bytes available there
            int q; // output window write pointer
            int m; // bytes to end of window or read pointer

            // copy input/output information to locals (UPDATE macro restores)

            p = _codec.NextIn;
            n = _codec.AvailableBytesIn;
            b = bitb;
            k = bitk;

            q = writeAt;
            m = (int)(q < readAt ? readAt - q - 1 : end - q);


            // process input based on current state
            while (true)
            {
                switch (mode)
                {
                    case InflateBlockMode.TYPE:

                        while (k < (3))
                        {
                            if (n != 0)
                            {
                                r = ZlibConstants.Z_OK;
                            }
                            else
                            {
                                bitb = b; bitk = k;
                                _codec.AvailableBytesIn = n;
                                _codec.TotalBytesIn += p - _codec.NextIn;
                                _codec.NextIn = p;
                                writeAt = q;
                                return Flush(r);
                            }

                            n--;
                            b |= (_codec.InputBuffer[p++] & 0xff) << k;
                            k += 8;
                        }
                        t = (int)(b & 7);
                        last = t & 1;

                        switch ((uint)t >> 1)
                        {
                            case 0:  // stored
                                b >>= 3; k -= (3);
                                t = k & 7; // go to byte boundary
                                b >>= t; k -= t;
                                mode = InflateBlockMode.LENS; // get length of stored block
                                break;

                            case 1:  // fixed
                                int[] bl = new int[1];
                                int[] bd = new int[1];
                                int[][] tl = new int[1][];
                                int[][] td = new int[1][];
                                InfTree.InflateTreesFixed(bl, bd, tl, td, _codec);
                                codes.Init(bl[0], bd[0], tl[0], 0, td[0], 0);
                                b >>= 3; k -= 3;
                                mode = InflateBlockMode.CODES;
                                break;

                            case 2:  // dynamic
                                b >>= 3; k -= 3;
                                mode = InflateBlockMode.TABLE;
                                break;

                            case 3:  // illegal
                                b >>= 3; k -= 3;
                                mode = InflateBlockMode.BAD;
                                _codec.Message = "invalid block type";
                                r = ZlibConstants.Z_DATA_ERROR;
                                bitb = b; bitk = k;
                                _codec.AvailableBytesIn = n;
                                _codec.TotalBytesIn += p - _codec.NextIn;
                                _codec.NextIn = p;
                                writeAt = q;
                                return Flush(r);
                        }
                        break;

                    case InflateBlockMode.LENS:

                        while (k < (32))
                        {
                            if (n != 0)
                            {
                                r = ZlibConstants.Z_OK;
                            }
                            else
                            {
                                bitb = b; bitk = k;
                                _codec.AvailableBytesIn = n;
                                _codec.TotalBytesIn += p - _codec.NextIn;
                                _codec.NextIn = p;
                                writeAt = q;
                                return Flush(r);
                            }
                            ;
                            n--;
                            b |= (_codec.InputBuffer[p++] & 0xff) << k;
                            k += 8;
                        }

                        if ((((~b) >> 16) & 0xffff) != (b & 0xffff))
                        {
                            mode = InflateBlockMode.BAD;
                            _codec.Message = "invalid stored block lengths";
                            r = ZlibConstants.Z_DATA_ERROR;

                            bitb = b; bitk = k;
                            _codec.AvailableBytesIn = n;
                            _codec.TotalBytesIn += p - _codec.NextIn;
                            _codec.NextIn = p;
                            writeAt = q;
                            return Flush(r);
                        }
                        left = (b & 0xffff);
                        b = k = 0; // dump bits
                        mode = left != 0 ? InflateBlockMode.STORED : (last != 0 ? InflateBlockMode.DRY : InflateBlockMode.TYPE);
                        break;

                    case InflateBlockMode.STORED:
                        if (n == 0)
                        {
                            bitb = b; bitk = k;
                            _codec.AvailableBytesIn = n;
                            _codec.TotalBytesIn += p - _codec.NextIn;
                            _codec.NextIn = p;
                            writeAt = q;
                            return Flush(r);
                        }

                        if (m == 0)
                        {
                            if (q == end && readAt != 0)
                            {
                                q = 0; m = (int)(q < readAt ? readAt - q - 1 : end - q);
                            }
                            if (m == 0)
                            {
                                writeAt = q;
                                r = Flush(r);
                                q = writeAt; m = (int)(q < readAt ? readAt - q - 1 : end - q);
                                if (q == end && readAt != 0)
                                {
                                    q = 0; m = (int)(q < readAt ? readAt - q - 1 : end - q);
                                }
                                if (m == 0)
                                {
                                    bitb = b; bitk = k;
                                    _codec.AvailableBytesIn = n;
                                    _codec.TotalBytesIn += p - _codec.NextIn;
                                    _codec.NextIn = p;
                                    writeAt = q;
                                    return Flush(r);
                                }
                            }
                        }
                        r = ZlibConstants.Z_OK;

                        t = left;
                        if (t > n)
                            t = n;
                        if (t > m)
                            t = m;
                        Array.Copy(_codec.InputBuffer, p, window, q, t);
                        p += t; n -= t;
                        q += t; m -= t;
                        if ((left -= t) != 0)
                            break;
                        mode = last != 0 ? InflateBlockMode.DRY : InflateBlockMode.TYPE;
                        break;

                    case InflateBlockMode.TABLE:

                        while (k < (14))
                        {
                            if (n != 0)
                            {
                                r = ZlibConstants.Z_OK;
                            }
                            else
                            {
                                bitb = b; bitk = k;
                                _codec.AvailableBytesIn = n;
                                _codec.TotalBytesIn += p - _codec.NextIn;
                                _codec.NextIn = p;
                                writeAt = q;
                                return Flush(r);
                            }

                            n--;
                            b |= (_codec.InputBuffer[p++] & 0xff) << k;
                            k += 8;
                        }

                        table = t = (b & 0x3fff);
                        if ((t & 0x1f) > 29 || ((t >> 5) & 0x1f) > 29)
                        {
                            mode = InflateBlockMode.BAD;
                            _codec.Message = "too many length or distance symbols";
                            r = ZlibConstants.Z_DATA_ERROR;

                            bitb = b; bitk = k;
                            _codec.AvailableBytesIn = n;
                            _codec.TotalBytesIn += p - _codec.NextIn;
                            _codec.NextIn = p;
                            writeAt = q;
                            return Flush(r);
                        }
                        t = 258 + (t & 0x1f) + ((t >> 5) & 0x1f);
                        if (blens == null || blens.Length < t)
                        {
                            blens = new int[t];
                        }
                        else
                        {
                            Array.Clear(blens, 0, t);
                            // for (int i = 0; i < t; i++)
                            // {
                            //     blens[i] = 0;
                            // }
                        }

                        b >>= 14;
                        k -= 14;


                        index = 0;
                        mode = InflateBlockMode.BTREE;
                        goto case InflateBlockMode.BTREE;

                    case InflateBlockMode.BTREE:
                        while (index < 4 + (table >> 10))
                        {
                            while (k < (3))
                            {
                                if (n != 0)
                                {
                                    r = ZlibConstants.Z_OK;
                                }
                                else
                                {
                                    bitb = b; bitk = k;
                                    _codec.AvailableBytesIn = n;
                                    _codec.TotalBytesIn += p - _codec.NextIn;
                                    _codec.NextIn = p;
                                    writeAt = q;
                                    return Flush(r);
                                }

                                n--;
                                b |= (_codec.InputBuffer[p++] & 0xff) << k;
                                k += 8;
                            }

                            blens[border[index++]] = b & 7;

                            b >>= 3; k -= 3;
                        }

                        while (index < 19)
                        {
                            blens[border[index++]] = 0;
                        }

                        bb[0] = 7;
                        t = inftree.InflateTreesBits(blens, bb, tb, hufts, _codec);
                        if (t != ZlibConstants.Z_OK)
                        {
                            r = t;
                            if (r == ZlibConstants.Z_DATA_ERROR)
                            {
                                blens = null;
                                mode = InflateBlockMode.BAD;
                            }

                            bitb = b; bitk = k;
                            _codec.AvailableBytesIn = n;
                            _codec.TotalBytesIn += p - _codec.NextIn;
                            _codec.NextIn = p;
                            writeAt = q;
                            return Flush(r);
                        }

                        index = 0;
                        mode = InflateBlockMode.DTREE;
                        goto case InflateBlockMode.DTREE;

                    case InflateBlockMode.DTREE:
                        while (true)
                        {
                            t = table;
                            if (!(index < 258 + (t & 0x1f) + ((t >> 5) & 0x1f)))
                            {
                                break;
                            }

                            int i, j, c;

                            t = bb[0];

                            while (k < t)
                            {
                                if (n != 0)
                                {
                                    r = ZlibConstants.Z_OK;
                                }
                                else
                                {
                                    bitb = b; bitk = k;
                                    _codec.AvailableBytesIn = n;
                                    _codec.TotalBytesIn += p - _codec.NextIn;
                                    _codec.NextIn = p;
                                    writeAt = q;
                                    return Flush(r);
                                }

                                n--;
                                b |= (_codec.InputBuffer[p++] & 0xff) << k;
                                k += 8;
                            }

                            t = hufts[(tb[0] + (b & InternalInflateConstants.InflateMask[t])) * 3 + 1];
                            c = hufts[(tb[0] + (b & InternalInflateConstants.InflateMask[t])) * 3 + 2];

                            if (c < 16)
                            {
                                b >>= t; k -= t;
                                blens[index++] = c;
                            }
                            else
                            {
                                // c == 16..18
                                i = c == 18 ? 7 : c - 14;
                                j = c == 18 ? 11 : 3;

                                while (k < (t + i))
                                {
                                    if (n != 0)
                                    {
                                        r = ZlibConstants.Z_OK;
                                    }
                                    else
                                    {
                                        bitb = b; bitk = k;
                                        _codec.AvailableBytesIn = n;
                                        _codec.TotalBytesIn += p - _codec.NextIn;
                                        _codec.NextIn = p;
                                        writeAt = q;
                                        return Flush(r);
                                    }

                                    n--;
                                    b |= (_codec.InputBuffer[p++] & 0xff) << k;
                                    k += 8;
                                }

                                b >>= t; k -= t;

                                j += (b & InternalInflateConstants.InflateMask[i]);

                                b >>= i; k -= i;

                                i = index;
                                t = table;
                                if (i + j > 258 + (t & 0x1f) + ((t >> 5) & 0x1f) || (c == 16 && i < 1))
                                {
                                    blens = null;
                                    mode = InflateBlockMode.BAD;
                                    _codec.Message = "invalid bit length repeat";
                                    r = ZlibConstants.Z_DATA_ERROR;

                                    bitb = b; bitk = k;
                                    _codec.AvailableBytesIn = n;
                                    _codec.TotalBytesIn += p - _codec.NextIn;
                                    _codec.NextIn = p;
                                    writeAt = q;
                                    return Flush(r);
                                }

                                c = (c == 16) ? blens[i - 1] : 0;
                                do
                                {
                                    blens[i++] = c;
                                }
                                while (--j != 0);
                                index = i;
                            }
                        }

                        tb[0] = -1;
                        {
                            int[] bl = new int[] { 9 };  // must be <= 9 for lookahead assumptions
                            int[] bd = new int[] { 6 }; // must be <= 9 for lookahead assumptions
                            int[] tl = new int[1];
                            int[] td = new int[1];

                            t = table;
                            t = inftree.InflateTreesDynamic(257 + (t & 0x1f), 1 + ((t >> 5) & 0x1f), blens, bl, bd, tl, td, hufts, _codec);

                            if (t != ZlibConstants.Z_OK)
                            {
                                if (t == ZlibConstants.Z_DATA_ERROR)
                                {
                                    blens = null;
                                    mode = InflateBlockMode.BAD;
                                }
                                r = t;

                                bitb = b; bitk = k;
                                _codec.AvailableBytesIn = n;
                                _codec.TotalBytesIn += p - _codec.NextIn;
                                _codec.NextIn = p;
                                writeAt = q;
                                return Flush(r);
                            }
                            codes.Init(bl[0], bd[0], hufts, tl[0], hufts, td[0]);
                        }
                        mode = InflateBlockMode.CODES;
                        goto case InflateBlockMode.CODES;

                    case InflateBlockMode.CODES:
                        bitb = b; bitk = k;
                        _codec.AvailableBytesIn = n;
                        _codec.TotalBytesIn += p - _codec.NextIn;
                        _codec.NextIn = p;
                        writeAt = q;

                        r = codes.Process(this, r);
                        if (r != ZlibConstants.Z_STREAM_END)
                        {
                            return Flush(r);
                        }

                        r = ZlibConstants.Z_OK;
                        p = _codec.NextIn;
                        n = _codec.AvailableBytesIn;
                        b = bitb;
                        k = bitk;
                        q = writeAt;
                        m = (int)(q < readAt ? readAt - q - 1 : end - q);

                        if (last == 0)
                        {
                            mode = InflateBlockMode.TYPE;
                            break;
                        }
                        mode = InflateBlockMode.DRY;
                        goto case InflateBlockMode.DRY;

                    case InflateBlockMode.DRY:
                        writeAt = q;
                        r = Flush(r);
                        q = writeAt; m = (int)(q < readAt ? readAt - q - 1 : end - q);
                        if (readAt != writeAt)
                        {
                            bitb = b; bitk = k;
                            _codec.AvailableBytesIn = n;
                            _codec.TotalBytesIn += p - _codec.NextIn;
                            _codec.NextIn = p;
                            writeAt = q;
                            return Flush(r);
                        }
                        mode = InflateBlockMode.DONE;
                        goto case InflateBlockMode.DONE;

                    case InflateBlockMode.DONE:
                        r = ZlibConstants.Z_STREAM_END;
                        bitb = b;
                        bitk = k;
                        _codec.AvailableBytesIn = n;
                        _codec.TotalBytesIn += p - _codec.NextIn;
                        _codec.NextIn = p;
                        writeAt = q;
                        return Flush(r);

                    case InflateBlockMode.BAD:
                        r = ZlibConstants.Z_DATA_ERROR;

                        bitb = b; bitk = k;
                        _codec.AvailableBytesIn = n;
                        _codec.TotalBytesIn += p - _codec.NextIn;
                        _codec.NextIn = p;
                        writeAt = q;
                        return Flush(r);


                    default:
                        r = ZlibConstants.Z_STREAM_ERROR;

                        bitb = b; bitk = k;
                        _codec.AvailableBytesIn = n;
                        _codec.TotalBytesIn += p - _codec.NextIn;
                        _codec.NextIn = p;
                        writeAt = q;
                        return Flush(r);
                }
            }
        }
        internal void Free()
        {
            Reset();
            window = null;
            hufts = null;
        }

        internal void SetDictionary(byte[] d, int start, int n)
        {
            Array.Copy(d, start, window, 0, n);
            readAt = (writeAt = n);
        }

        internal int SyncPoint()
        {
            return (mode == InflateBlockMode.LENS) ? 1 : 0;
        }

        internal int Flush(int r)
        {
            for (int i = 0; i < 2; i++)
            {
                int num = ((i != 0) ? (writeAt - readAt) : (((readAt <= writeAt) ? writeAt : end) - readAt));
                if (num == 0)
                {
                    if (r == -5)
                    {
                        r = 0;
                    }
                    return r;
                }
                if (num > _codec.AvailableBytesOut)
                {
                    num = _codec.AvailableBytesOut;
                }
                if (num != 0 && r == -5)
                {
                    r = 0;
                }
                _codec.AvailableBytesOut -= num;
                _codec.TotalBytesOut += num;
                if (checkfn != null)
                {
                    _codec._Adler32 = (check = Adler.Adler32(check, window, readAt, num));
                }
                Array.Copy(window, readAt, _codec.OutputBuffer, _codec.NextOut, num);
                _codec.NextOut += num;
                readAt += num;
                if (readAt == end && i == 0)
                {
                    readAt = 0;
                    if (writeAt == end)
                    {
                        writeAt = 0;
                    }
                }
                else
                {
                    i++;
                }
            }
            return r;
        }
    }
}
