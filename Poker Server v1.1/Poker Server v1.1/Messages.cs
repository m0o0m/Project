using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker_Server_v1._1
{
    class Messages
    {
        public static bool decode(byte[] Bytes, out string decoded)
        {
            //decodin data that recieves from a web socket
            decoded = "";
            byte[] mask = new byte[4];
            int length, indexFirstMask, indexFirstData;
            if (Bytes.Length <= 0)
                return false;
            if (Bytes[0] != 129)
                return false;
            length = (Bytes[1] & 127);//may not be the actual length in the two special cases
            indexFirstMask = 2;
            if (length == 126)
                indexFirstMask = 4;
            else if (length == 127)
                indexFirstMask = 10;
            for (int i = 0; i < 4; i++)
                mask[i] = Bytes[indexFirstMask + i];
            indexFirstData = indexFirstMask + 4;
            length = Bytes.Length - indexFirstData;
            if (length < 0)
                return false;
            byte[] decodedByte = new byte[length];
            for (int i = 0; i < length; i++)
                decodedByte[i] = (byte)(Bytes[indexFirstData++] ^ mask[i % 4]);
            decoded = Encoding.UTF8.GetString(decodedByte);
            return true;
        }
        public static byte[] encode(string message)
        {
            //encoding data that we want to send
            byte[] data = Encoding.UTF8.GetBytes(message);
            int dataStart;
            byte[] bytesFormatted = new byte[10];

            bytesFormatted[0] = 129;

            if (data.Length <= 125)
            {
                bytesFormatted[1] = (byte)data.Length;

                dataStart = 2;
            }
            else if ((data.Length >= 126) & (data.Length <= 65535))
            {
                bytesFormatted[1] = 126;
                bytesFormatted[2] = (byte)((data.Length >> 8) & 255);
                bytesFormatted[3] = (byte)((data.Length) & 255);

                dataStart = 4;
            }
            else
            {
                bytesFormatted[1] = 127;
                bytesFormatted[2] = (byte)((data.Length >> 56) & 255);
                bytesFormatted[3] = (byte)((data.Length >> 48) & 255);
                bytesFormatted[4] = (byte)((data.Length >> 40) & 255);
                bytesFormatted[5] = (byte)((data.Length >> 32) & 255);
                bytesFormatted[6] = (byte)((data.Length >> 24) & 255);
                bytesFormatted[7] = (byte)((data.Length >> 16) & 255);
                bytesFormatted[8] = (byte)((data.Length >> 8) & 255);
                bytesFormatted[9] = (byte)((data.Length) & 255);

                dataStart = 10;
            }

            int byteLength = dataStart + data.Length;
            byte[] formatedData = new byte[byteLength];

            for (int i = 0; i < byteLength; i++)
            {
                if (i < dataStart)
                    formatedData[i] = bytesFormatted[i];
                else
                    formatedData[i] = data[i - dataStart];
            }
            return formatedData;
        }
    }
}
