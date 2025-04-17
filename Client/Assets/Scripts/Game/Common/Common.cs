using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public static class Common
    {
        private static int hash = 10000;
        private static Dictionary<string, int> str2hash = new Dictionary<string, int>();

        public static int StringToHash(string str)
        {
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            if (str2hash.ContainsKey(str))
            {
                return str2hash[str];
            }
            else
            {
                hash += 1;
                str2hash.Add(str, hash);
                return hash;
            }            
        }

        private static SnowflakeGenerator snowflakeGenerator = new SnowflakeGenerator(1L, 1L);
        public static long GenerateUid() 
        {
            return snowflakeGenerator.NextId();
        }
    }
}