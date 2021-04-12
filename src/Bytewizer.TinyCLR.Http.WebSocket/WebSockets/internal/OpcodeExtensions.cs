using System;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    internal static class OpcodeExtensions
    {
        internal static bool IsData(this byte opcode)
        {
            return opcode == 0x1 || opcode == 0x2;
        }

        internal static bool IsData(this Opcode opcode)
        {
            return opcode == Opcode.Text || opcode == Opcode.Binary;
        }

        internal static bool IsControl(this byte opcode)
        {
            return opcode > 0x7 && opcode < 0x10;
        }

        internal static bool IsControl(this Opcode opcode)
        {
            return opcode >= Opcode.Close;
        }
        internal static bool IsSupported(this byte opcode)
        {
            return (opcode == (byte)Opcode.Binary)
                   || opcode == (byte)Opcode.Close
                   || opcode == (byte)Opcode.Cont
                   || opcode == (byte)Opcode.Ping
                   || opcode == (byte)Opcode.Pong
                   || opcode == (byte)Opcode.Text;
        }
    }
}