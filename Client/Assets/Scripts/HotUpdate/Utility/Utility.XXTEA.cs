using System;
using System.Text;

namespace GameFramework.Utility
{
    public static class XXTEA
    {
        public static void Test()
        {
            byte[] data = Encoding.UTF8.GetBytes("HelloModifiedXXTEA");
            byte[] key = Encoding.UTF8.GetBytes("Ks#ds%lqG*egwmwH");

            byte[] key2 = Encoding.UTF8.GetBytes("Ks#ds%lqG*egwmwH");

            byte[] encrypted = Encrypt(data, key);

            byte[] decrypted = Decrypt(encrypted, key2);

            // 验证
            //Debug.Log("原始数据: " + Encoding.UTF8.GetString(data));
            //Debug.Log("加密数据: " + Encoding.UTF8.GetString(encrypted));
            //Debug.Log("解密数据: " + Encoding.UTF8.GetString(decrypted));
        }

        public static byte[] Encrypt(byte[] Data, byte[] Key)
        {
            if (Data.Length == 0)
            {
                return Data;
            }
            return ToByteArray(Encrypt(ToUInt32Array(Data, true), ToUInt32Array(Key, false)), false);
        }
        public static byte[] Decrypt(byte[] Data, byte[] Key)
        {
            if (Data.Length == 0)
            {
                return Data;
            }
            return ToByteArray(Decrypt(ToUInt32Array(Data, false), ToUInt32Array(Key, false)), true);
        }

        public static uint[] Encrypt(uint[] v, uint[] k)
        {
            int n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                uint[] Key = new uint[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            uint z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
            int p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 3 & 3;
                for (p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    z = unchecked(v[p] += (z >> 6 ^ y << 3) + (y >> 4 ^ z << 5) ^ (sum ^ y) + (k[p & 3 ^ e] & z));
                }
                y = v[0];
                z = unchecked(v[n] += (z >> 6 ^ y << 3) + (y >> 4 ^ z << 5) ^ (sum ^ y) + (k[p & 3 ^ e] & z));
            }
            return v;
        }
        public static uint[] Decrypt(uint[] v, uint[] k)
        {
            int n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                uint[] Key = new uint[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            uint z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
            int p, q = 6 + 52 / (n + 1);
            sum = unchecked((uint)(q * delta));
            while (sum != 0)
            {
                e = sum >> 3 & 3;
                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    y = unchecked(v[p] -= (z >> 6 ^ y << 3) + (y >> 4 ^ z << 5) ^ (sum ^ y) + (k[p & 3 ^ e] & z));
                }
                z = v[n];
                y = unchecked(v[0] -= (z >> 6 ^ y << 3) + (y >> 4 ^ z << 5) ^ (sum ^ y) + (k[p & 3 ^ e] & z));
                sum = unchecked(sum - delta);
            }
            return v;
        }
        private static uint[] ToUInt32Array(byte[] Data, bool IncludeLength)
        {
            int n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
            uint[] Result;
            if (IncludeLength)
            {
                Result = new uint[n + 1];
                Result[n] = (uint)Data.Length;
            }
            else
            {
                Result = new uint[n];
            }
            n = Data.Length;
            for (int i = 0; i < n; i++)
            {
                Result[i >> 2] |= (uint)Data[i] << ((i & 3) << 3);
            }
            return Result;
        }
        private static byte[] ToByteArray(uint[] Data, bool IncludeLength)
        {
            int n;
            if (IncludeLength)
            {
                n = (int)Data[Data.Length - 1];
            }
            else
            {
                n = Data.Length << 2;
            }
            byte[] Result = new byte[n];
            for (int i = 0; i < n; i++)
            {
                Result[i] = (byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return Result;
        }
    }
}
