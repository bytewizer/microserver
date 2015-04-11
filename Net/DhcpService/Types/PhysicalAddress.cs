using System;

namespace MicroServer.Net.Dhcp
{
    public class PhysicalAddress : IComparable
    {
        private byte[] _address = new Byte[] { 0, 0, 0, 0, 0, 0 };

        public byte this[int index]
        {
            get { return this._address[index]; }
        }

        public PhysicalAddress(){}

        public PhysicalAddress(params byte[] address)
        {
            if (address == null || address.Length != 6)
            {
                _address = null;
            }

            address.CopyTo(this._address, 0);
        }

        public int CompareTo(Object obj)
        {
            PhysicalAddress other = obj as PhysicalAddress;

            if (other == null)
            {
                return 1;
            }

            for (Int32 i = 0; i < 6; i++)
            {
                if (this._address[i] > other._address[i])
                {
                    return -1;
                }
                else if (this._address[i] < other._address[i])
                {
                    return 1;
                }
            }

            return 0;
        }

        public override bool Equals(Object obj)
        {
            return this.Equals(obj as PhysicalAddress);
        }

        public bool Equals(PhysicalAddress other)
        {
            return other != null &&
                this._address[0] == other._address[0] &&
                this._address[1] == other._address[1] &&
                this._address[2] == other._address[2] &&
                this._address[3] == other._address[3] &&
                this._address[4] == other._address[4] &&
                this._address[5] == other._address[5];
        }

        public override int GetHashCode()
        {
            return _address.GetHashCode();
        }

        public InternetAddress Copy()
        {
            return new InternetAddress(_address[0], _address[1], _address[2], _address[3], _address[4], _address[5]);
        }

        public byte[] ToArray()
        {
            Byte[] array = new Byte[6];
            _address.CopyTo(array, 0);
            return array;
        }

        public override string ToString()
        {
            if (_address != null)
            {
                return MacToString(_address);
            }
            else
            {
                return "Null";
            }
        }

        public static PhysicalAddress Parse(String address)
        {
            PhysicalAddress physical = new PhysicalAddress(0, 0, 0, 0, 0, 0);

            Int32 index = 0;
            Byte currentValue = 0;
            Boolean first = true;

            foreach (Char c in address)
            {
                Byte digitValue;

                switch (c)
                {
                    case '0':
                        digitValue = 0;
                        break;
                    case '1':
                        digitValue = 1;
                        break;
                    case '2':
                        digitValue = 2;
                        break;
                    case '3':
                        digitValue = 3;
                        break;
                    case '4':
                        digitValue = 4;
                        break;
                    case '5':
                        digitValue = 5;
                        break;
                    case '6':
                        digitValue = 6;
                        break;
                    case '7':
                        digitValue = 7;
                        break;
                    case '8':
                        digitValue = 8;
                        break;
                    case '9':
                        digitValue = 9;
                        break;
                    case 'A':
                    case 'a':
                        digitValue = 10;
                        break;
                    case 'B':
                    case 'b':
                        digitValue = 11;
                        break;
                    case 'C':
                    case 'c':
                        digitValue = 12;
                        break;
                    case 'D':
                    case 'd':
                        digitValue = 13;
                        break;
                    case 'E':
                    case 'e':
                        digitValue = 14;
                        break;
                    case 'F':
                    case 'f':
                        digitValue = 15;
                        break;
                    default:
                        continue;
                }

                if (first)
                {
                    currentValue = (Byte)(digitValue << 4);
                    first = false;
                }
                else
                {
                    currentValue |= digitValue;
                    physical._address[index++] = currentValue;
                    first = true;
                }
            }

            return physical;
        }

        private string MacToString(byte[] macAddress)
        {
            string results = string.Empty;
            for (int i = 0; i < 6; i++) results += (i > 0 ? "-" : "") + (char)((macAddress[i] >> 4) > 9 ? (macAddress[i] >> 4) + 'A' - 10 : (macAddress[i] >> 4) + '0') + (char)((macAddress[i] & 0xF) > 9 ? (macAddress[i] & 0xF) + 'A' - 10 : (macAddress[i] & 0xF) + '0');
            return results;
        }
    }
}


