using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;
namespace TCPServerResponseFile
{

        public class Crc32
        {
            uint[] table;

            public uint ComputeChecksum(byte[] bytes)
            {
                uint crc = 0xffffffff;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                    crc = (uint)((crc >> 8) ^ table[index]);
                }
                return ~crc;
            }

            public byte[] ComputeChecksumBytes(byte[] bytes)
            {
                return BitConverter.GetBytes(ComputeChecksum(bytes));
            }

            public Crc32()
            {
                uint poly = 0xedb88320;
                table = new uint[256];
                uint temp = 0;
                for (uint i = 0; i < table.Length; ++i)
                {
                    temp = i;
                    for (int j = 8; j > 0; --j)
                    {
                        if ((temp & 1) == 1)
                        {
                            temp = (uint)((temp >> 1) ^ poly);
                        }
                        else
                        {
                            temp >>= 1;
                        }
                    }
                    table[i] = temp;
                }
            }
        }
     

    public class FileSend
    {
        public bool isSending;
        public byte[] raw;
        public int packageNo;
        public int maxPackage;

    }
    public class Commands
    {
        public const byte ENTER = (byte)'C';
        public const byte SOH = 0x01;
        public const byte STX = 0x02;
        public const byte ETX = 0x03;
        public const byte EOT = 0x04;
        public const byte ENQ = 0x05;
        public const byte ACK = 0x06;
        public const byte NAK = 0x15;

        public static int PackageSize = 1024;

        public static ushort CalcCRC16(byte[] data, int startOffset=0, bool lastIsCrc=true)
        {
            ushort crc = 0x0000;
            int dataLen = data.Length;
            if (lastIsCrc) dataLen -= 2;

            for (int i = startOffset; i < dataLen; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc;
        }

        public static byte[] CreateCommand(ref byte[] raw, int package)
        {
            byte[] cmd = new byte[PackageSize + 1 + 2 + 2];
            int rawLen = raw.Length;
            int sendPackageNo = package + 1;
            cmd[0] = SOH;
            cmd[1] = (byte)((UInt16) (sendPackageNo >> 8));
            cmd[2] = (byte)(sendPackageNo);
            for(int i=0;i<PackageSize;i++)
            {
                if (rawLen> i + package * PackageSize)
                {
                    cmd[i + 3] = raw[i + package * PackageSize];
                }
                else
                {
                    cmd[i + 3] = 0x0;
                }
            }
            ushort crc = CalcCRC16(cmd, 3);
            cmd[PackageSize + 3] = (byte)((UInt16)(crc >> 8));
            cmd[PackageSize + 4] = (byte)(crc);
            return cmd;
        }

        public static byte[] CreateEndCommand(ref byte[] raw)
        {
            byte[] cmd = new byte[ 1 + 4 + 2];
            CRC myCRC = new CRC();
            myCRC.InitCRC32();
            byte[] crc32 = myCRC.CalcCRC32Bytes(raw);
            cmd[0] = EOT;
            cmd[1] = crc32[0];
            cmd[2] = crc32[1];
            cmd[3] = crc32[2];
            cmd[4] = crc32[3];
            ushort crc = CalcCRC16(raw, 0,false);
            cmd[5] = (byte)((UInt16)(crc >> 8));
            cmd[6] = (byte)(crc);

            return cmd;
        }
    }
}
