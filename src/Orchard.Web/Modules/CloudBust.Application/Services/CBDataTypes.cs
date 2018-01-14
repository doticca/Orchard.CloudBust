using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace CloudBust.Application.Services
{
    public static class CBDataTypes
    {
        private static readonly char[] AvailableCharacters = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
          };

        private static readonly char[] AvailableNumbers = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
          };

        public static string StringFromType(CBType i)
        {
            switch (i)
            {
                case CBType.stringSetting:
                    return "string";
                case CBType.boolSetting:
                    return "bool";
                case CBType.doubleSetting:
                    return "double";
                case CBType.intSetting:
                    return "int";
                case CBType.datetimeSetting:
                    return "datetime";
                default:
                    return "string";
            }
        }
        public static CBType TypeFromString(string s)
        {
            switch (s)
            {
                case "string":
                    return CBType.stringSetting;
                case "bool":
                    return CBType.boolSetting;
                case "double":
                    return CBType.doubleSetting;
                case "int":
                    return CBType.intSetting;
                case "datetime":
                    return CBType.datetimeSetting;
                default:
                    return CBType.stringSetting;
            }
        }
        public static Type SharpTypeFromString(string s)
        {
            switch (s)
            {
                case "string":
                    return typeof(string);
                case "bool":
                    return typeof(bool);
                case "double":
                    return typeof(double);
                case "int":
                    return typeof(int);
                case "datetime":
                    return typeof(DateTime);
                default:
                    return typeof(string);
            }
        }
        public static string GenerateIdentifier(int length, bool useNumbers = false)
        {
            char[] identifier = new char[length];
            byte[] randomData = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomData);
            }

            for (int idx = 0; idx < identifier.Length; idx++)
            {
                int pos;
                if (!useNumbers)
                {
                    pos = randomData[idx] % AvailableCharacters.Length;
                    identifier[idx] = AvailableCharacters[pos];
                }
                else
                {
                    pos = randomData[idx] % AvailableNumbers.Length;
                    identifier[idx] = AvailableNumbers[pos];
                }

            }

            return new string(identifier);
        }
    }
    public enum CBType
    {
        stringSetting,
        boolSetting,
        doubleSetting,
        intSetting,
        datetimeSetting
    }
}