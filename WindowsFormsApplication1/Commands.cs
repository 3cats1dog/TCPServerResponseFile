using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;
namespace WindowsFormsApplication1
{

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
        public const byte EQT = 0x04;
        public const byte ENQ = 0x05;
        public const byte ACK = 0x06;
        public const byte NAK = 0x15;

        public static int PackageSize = 1024;

        public static ushort CalcCRC16(byte[] data, bool lastIsCrc=true)
        {
            ushort crc = 0x0000;
            int dataLen = data.Length;
            if (lastIsCrc) dataLen -= 2;
            for (int i = 0; i < dataLen; i++)
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
            cmd[0] = SOH;
            cmd[1] = (byte)((UInt16) (package >> 8));
            cmd[2] = (byte)(package );
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
            ushort crc = CalcCRC16(cmd);
            cmd[PackageSize + 3] = (byte)((UInt16)(crc >> 8));
            cmd[PackageSize + 4] = (byte)(crc);
            return cmd;
        }
    }
}
