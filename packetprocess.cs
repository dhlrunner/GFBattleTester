using AC;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

class Packet
{
    static int time = 0;
    public static string init()
    {
        AuthCode.Init(new AuthCode.IntDelegate(GetCurrentTimeStamp));
        return GetCurrentTimeStamp().ToString();
    }
    public static string init(int t)
    {
        time = t;
        AuthCode.Init(new AuthCode.IntDelegate(GetCustomTimeStamp));
        return GetCurrentTimeStamp().ToString();
    }
   
    public static int GetCurrentTimeStamp()
    {
        return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - realtimeSinceLogin + loginTime);
    }
    public static int GetCustomTimeStamp()
    {
        return time;
    }
    public static string Decode(string m, string s)
    {
        try
        {
            if (m.StartsWith("#"))
            {
                using (MemoryStream stream = new MemoryStream(AuthCode.DecodeWithGzip(m.Substring(1), s)))
                {
                    using (Stream stream2 = new GZipInputStream(stream))
                    {
                        using (StreamReader reader = new StreamReader(stream2, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            return AuthCode.Decode(m, s);
        }
        catch (Exception e)
        {
            MessageBox.Show("Maybe Signtoken is incorrect. Check your signtoken.\n[Error Details]\n" + e.ToString(), "Decode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return e.ToString();
        }
    }

    public static string Encode(string data, string signcode)
    {
        return AuthCode.Encode(data, signcode);
    }

    public static string EncodeWithGzip(string m, string key)
    {
        byte[] raw = Encoding.UTF8.GetBytes(m);
        byte[] dest;
        using (MemoryStream stream = new MemoryStream())
        {
            using (GZipOutputStream stream1 = new GZipOutputStream(stream))
            {
                stream1.IsStreamOwner = false;
                stream1.Write(raw, 0, raw.Length);
                stream1.Flush();
                stream1.Finish();
                dest = stream.ToArray();
            }
        }

        byte[] bytes = new byte[dest.Length + 0x1a];
        Array.Copy(dest, 0, bytes, 0x1a, dest.Length);
        Array.Copy(Encoding.UTF8.GetBytes("0000000000"), 0, bytes, 0, 10);
        //1
        //还有26byte待解决
        int lengthOfACode = 0;
        key = AuthCode.MD5(key);
        string str = AuthCode.MD5(key.Substring(0x10, 0x10));
        string s = AuthCode.MD5(key.Substring(0x0, 0x10));
        string str3 = (lengthOfACode > 0) ? m.Substring(0, lengthOfACode) : "";
        string pass = str + AuthCode.MD5(str + str3);

        //1
        byte[] buffer4 = new byte[(bytes.Length - 0x1a) + s.Length];
        Array.Copy(dest, 0, buffer4, 0, dest.Length);
        Array.Copy(Encoding.UTF8.GetBytes(s), 0, buffer4, dest.Length, s.Length);
        Array.Copy(Encoding.UTF8.GetBytes(AuthCode.MD5(buffer4)), 0, bytes, 10, 0x10);

        byte[] buffer = RC4(bytes, pass);

        string result = Convert.ToBase64String(buffer);

        return "#" + new string(new char[lengthOfACode]) + result;
    }
    private static byte[] RC4(byte[] input, string pass)
    {
        if ((input == null) || (pass == null))
        {
            return null;
        }
        byte[] buffer = new byte[input.Length];
        byte[] key = GetKey(Encoding.UTF8.GetBytes(pass), 0x100);
        long num = 0L;
        long num2 = 0L;
        for (long i = 0L; i < input.Length; i += 1L)
        {
            num = (num + 1L) % ((long)key.Length);
            num2 = (num2 + key[(int)((IntPtr)num)]) % ((long)key.Length);
            byte num4 = key[(int)((IntPtr)num)];
            key[(int)((IntPtr)num)] = key[(int)((IntPtr)num2)];
            key[(int)((IntPtr)num2)] = num4;
            byte num5 = input[(int)((IntPtr)i)];
            byte num6 = key[(key[(int)((IntPtr)num)] + key[(int)((IntPtr)num2)]) % key.Length];
            buffer[(int)((IntPtr)i)] = (byte)(num5 ^ num6);
        }
        return buffer;
    }

    private static byte[] GetKey(byte[] pass, int kLen)
    {
        byte[] buffer = new byte[kLen];
        for (long i = 0L; i < kLen; i += 1L)
        {
            buffer[(int)((IntPtr)i)] = (byte)i;
        }
        long num = 0L;
        for (long j = 0L; j < kLen; j += 1L)
        {
            num = ((num + buffer[(int)((IntPtr)j)]) + pass[(int)((IntPtr)(j % ((long)pass.Length)))]) % ((long)kLen);
            byte num4 = buffer[(int)((IntPtr)j)];
            buffer[(int)((IntPtr)j)] = buffer[(int)((IntPtr)num)];
            buffer[(int)((IntPtr)num)] = num4;
        }
        return buffer;
    }
    public static int realtimeSinceLogin = 0;
    public static int loginTime = 0;
}