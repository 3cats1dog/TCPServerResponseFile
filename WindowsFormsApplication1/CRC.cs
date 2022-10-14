using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerResponseFile
{
   public class CRC
    {
        public ushort POLY16 = 0x1021;
        public uint POLY32 = 0xedb88320;

        uint[] table;

        public void InitCRC16(ushort poly)
        {
            POLY16 = poly;
        }

        public void InitCRC32()
        {
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ POLY32);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }

        public static ushort CalcCRC16(byte[] data, int startOffset = 0, bool lastIsCrc = true)
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

        public uint CalcCRC32(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }


        public byte[] CalcCRC32Bytes(byte[] bytes)
        {
            return BitConverter.GetBytes(CalcCRC32(bytes));
        }

        public static byte[] CalcCRC16Bytes(byte[] bytes, int startOffset = 0, bool lastIsCrc = true)
        {
            return BitConverter.GetBytes(CalcCRC16(bytes ,startOffset, lastIsCrc));
        }

    }
}
