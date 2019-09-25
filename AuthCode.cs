//From https://github.com/xicojp/gf/blob/9c4265076ae3d6dfec99b023fe69d55902c9df8d/GF/AuthCode.cs

using System;
using System.Security.Cryptography;
using System.Text;

namespace AC
{
    public class AuthCode
    {
        private enum AuthcodeMode
        {
            Encode,
            Decode
        }

        public delegate int IntDelegate();

        private static Encoding encoding = Encoding.UTF8;

        private static int timeOffset;

        public static AuthCode.IntDelegate GetCurrentTimeStampMethod;

        public static void InitTimeData(int realtime, int loginTime)
        {
            AuthCode.timeOffset = loginTime - realtime;
        }

        public static void Init(AuthCode.IntDelegate method)
        {
            AuthCode.GetCurrentTimeStampMethod = method;
        }

        private static int GetCurrentTimeStamp()
        {
            return AuthCode.GetCurrentTimeStampMethod();
        }

        private static string CutString(string str, int startIndex, int length)
        {
            bool flag = startIndex >= 0;
            string result;
            if (flag)
            {
                bool flag2 = length < 0;
                if (flag2)
                {
                    length *= -1;
                    bool flag3 = startIndex - length < 0;
                    if (flag3)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                bool flag4 = startIndex > str.Length;
                if (flag4)
                {
                    result = "";
                    return result;
                }
            }
            else
            {
                bool flag5 = length < 0;
                if (flag5)
                {
                    result = "";
                    return result;
                }
                bool flag6 = length + startIndex > 0;
                if (!flag6)
                {
                    result = "";
                    return result;
                }
                length += startIndex;
                startIndex = 0;
            }
            bool flag7 = str.Length - startIndex < length;
            if (flag7)
            {
                length = str.Length - startIndex;
            }
            result = str.Substring(startIndex, length);
            return result;
        }

        private static string CutString(string str, int startIndex)
        {
            return AuthCode.CutString(str, startIndex, str.Length);
        }

        public static string MD5(string str)
        {
            byte[] bytes = AuthCode.encoding.GetBytes(str);
            return AuthCode.MD5(bytes);
        }

        public static string MD5(byte[] b)
        {
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string text = "";
            for (int i = 0; i < b.Length; i++)
            {
                text += b[i].ToString("x").PadLeft(2, '0');
            }
            return text;
        }

        private static byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] array = new byte[kLen];
            for (long num = 0L; num < (long)kLen; num += 1L)
            {
                array[(int)(checked((IntPtr)num))] = (byte)num;
            }
            long num2 = 0L;
            for (long num3 = 0L; num3 < kLen; num3 += 1L)
            {
                num2 = (num2 + array[(int)num3] + pass[(int)(num3 % pass.Length)]) % kLen;
                checked
                {
                    byte b = array[(int)((IntPtr)num3)];
                    array[(int)((IntPtr)num3)] = array[(int)((IntPtr)num2)];
                    array[(int)((IntPtr)num2)] = b;
                }
            }
            return array;
        }

        private static string RandomString(int lens)
        {
            char[] array = new char[]
            {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'h',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'u',
                'v',
                'w',
                'x',
                'y',
                'z',
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'J',
                'K',
                'L',
                'M',
                'N',
                'O',
                'P',
                'Q',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'X',
                'Y',
                'Z',
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            int maxValue = array.Length;
            string text = "";
            Random random = new Random();
            for (int i = 0; i < lens; i++)
            {
                text += array[random.Next(maxValue)].ToString();
            }
            return text;
        }

        public static string Encode(string source, string key)
        {
            return AuthCode.Authcode(source, key, AuthCode.AuthcodeMode.Encode, 3600);
        }

        public static string Decode(string source, string key)
        {
            return AuthCode.Authcode(source, key, AuthCode.AuthcodeMode.Decode, 3600);
        }

        private static string Authcode(string source, string key, AuthCode.AuthcodeMode operation, int expiry)
        {
            bool flag = source == null || key == null;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                int num = 0;
                key = AuthCode.MD5(key);
                string str = AuthCode.MD5(AuthCode.CutString(key, 16, 16));
                string str2 = AuthCode.MD5(AuthCode.CutString(key, 0, 16));
                string text = (num > 0) ? ((operation == AuthCode.AuthcodeMode.Decode) ? AuthCode.CutString(source, 0, num) : AuthCode.RandomString(num)) : "";
                string pass = str + AuthCode.MD5(str + text);
                bool flag2 = operation == AuthCode.AuthcodeMode.Decode;
                if (flag2)
                {
                    byte[] input;
                    try
                    {
                        input = Convert.FromBase64String(AuthCode.CutString(source, num));
                    }
                    catch
                    {
                        try
                        {
                            input = Convert.FromBase64String(AuthCode.CutString(source + "=", num));
                        }
                        catch
                        {
                            try
                            {
                                input = Convert.FromBase64String(AuthCode.CutString(source + "==", num));
                            }
                            catch
                            {
                                result = "";
                                return result;
                            }
                        }
                    }
                    string @string = AuthCode.encoding.GetString(AuthCode.RC4(input, pass));
                    long num2 = long.Parse(AuthCode.CutString(@string, 0, 10));
                    bool flag3 = (num2 == 0L || num2 - (long)AuthCode.GetCurrentTimeStamp() > 0L) && AuthCode.CutString(@string, 10, 16) == AuthCode.CutString(AuthCode.MD5(AuthCode.CutString(@string, 26) + str2), 0, 16);
                    if (flag3)
                    {
                        result = AuthCode.CutString(@string, 26);
                    }
                    else
                    {
                        result = "";
                    }
                }
                else
                {
                    source = ((expiry == 0) ? "0000000000" : (expiry + AuthCode.GetCurrentTimeStamp()).ToString()) + AuthCode.CutString(AuthCode.MD5(source + str2), 0, 16) + source;
                    byte[] inArray = AuthCode.RC4(AuthCode.encoding.GetBytes(source), pass);
                    result = text + Convert.ToBase64String(inArray);
                }
            }
            return result;
        }

        public static byte[] DecodeWithGzip(string source, string key)
        {
            return AuthCode.DecodeWithGzip(source, key, 3600);
        }

        public static byte[] DecodeWithGzip(string source, string key, int expiry)
        {
            bool flag = source == null || key == null;
            byte[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                int num = 0;
                key = AuthCode.MD5(key);
                string str = AuthCode.MD5(AuthCode.CutString(key, 16, 16));
                string text = AuthCode.MD5(AuthCode.CutString(key, 0, 16));
                string str2 = (num > 0) ? AuthCode.CutString(source, 0, num) : "";
                string pass = str + AuthCode.MD5(str + str2);
                byte[] input;
                try
                {
                    input = Convert.FromBase64String(AuthCode.CutString(source, num));
                }
                catch
                {
                    try
                    {
                        input = Convert.FromBase64String(AuthCode.CutString(source + "=", num));
                    }
                    catch
                    {
                        try
                        {
                            input = Convert.FromBase64String(AuthCode.CutString(source + "==", num));
                        }
                        catch
                        {
                            result = null;
                            return result;
                        }
                    }
                }
                byte[] array = AuthCode.RC4(input, pass);
                string @string = AuthCode.encoding.GetString(array);
                long num2 = long.Parse(AuthCode.CutString(@string, 0, 10));
                byte[] array2 = new byte[array.Length - 26];
                Array.Copy(array, 26, array2, 0, array.Length - 26);
                byte[] array3 = new byte[array.Length - 26 + text.Length];
                Array.Copy(array2, 0, array3, 0, array2.Length);
                Array.Copy(AuthCode.encoding.GetBytes(text), 0, array3, array2.Length, text.Length);
                bool flag2 = (num2 == 0L || num2 - (long)AuthCode.GetCurrentTimeStamp() > 0L) && AuthCode.CutString(@string, 10, 16) == AuthCode.CutString(AuthCode.MD5(array3), 0, 16);
                if (flag2)
                {
                    result = array2;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public static byte[] RC4(byte[] input, string pass)
        {
            bool flag = input == null || pass == null;
            byte[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                byte[] array = new byte[input.Length];
                byte[] key = AuthCode.GetKey(AuthCode.encoding.GetBytes(pass), 256);
                long num = 0L;
                long num2 = 0L;
                for (int i = 0; i < input.Length; i += 1)
                {
                    num = (num + 1L) % key.Length;
                    num2 = (num2 + key[num]) % key.Length;
                    checked
                    {
                        byte b = key[num];
                        key[num] = key[num2];
                        key[num2] = b;

                        byte b2 = input[i];
                        byte b3 = key[(key[num] + key[num2]) % key.Length];
                        array[i] = (byte)(b2 ^ b3);
                    }
                }
                result = array;
            }
            return result;
        }

        private static string AscArr2Str(byte[] b)
        {
            return Encoding.Unicode.GetString(Encoding.Convert(Encoding.ASCII, Encoding.Unicode, b));
        }

        public static string urlencode(string str)
        {
            string text = string.Empty;
            string text2 = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < str.Length; i++)
            {
                string text3 = str.Substring(i, 1);
                bool flag = text2.Contains(text3);
                if (flag)
                {
                    text += text3;
                }
                else
                {
                    byte[] bytes = AuthCode.encoding.GetBytes(text3);
                    byte[] array = bytes;
                    for (int j = 0; j < array.Length; j++)
                    {
                        byte b = array[j];
                        text = text + "%" + b.ToString("X");
                    }
                }
            }
            return text;
        }

        public static long time()
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}