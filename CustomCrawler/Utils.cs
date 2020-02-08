/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler
{
    /// <summary>
    /// Exists for compatibility with other older projects.
    /// Never use this class.
    /// </summary>
    public class NetCommon
    {
        public static WebClient GetDefaultClient()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");
            return wc;
        }

        public static string DownloadString(string url)
        {
            return GetDefaultClient().DownloadString(url);
        }
    }

    /// <summary>
    /// Contains all instance information.
    /// </summary>
    public class InstanceMonitor
    {
        public static Dictionary<string, object> Instances = new Dictionary<string, object>();
    }
    
    /// <summary>
    /// Class to make lazy instance easier to implement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ILazy<T>
        where T : new()
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() =>
        {
            T instance = new T();
            InstanceMonitor.Instances.Add(instance.GetType().Name.ToLower(), instance);
            return instance;
        });
        public static T Instance => instance.Value;
        public static bool IsValueCreated => instance.IsValueCreated;
    }

    public static class Strings
    {
        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(text);
                decodedText = encoding.GetString(textAsBytes);
                return true;
            }
            catch (Exception)
            {
                decodedText = null;
                return false;
            }
        }

        public static int ComputeLevenshteinDistance(this string a, string b)
        {
            int x = a.Length;
            int y = b.Length;
            int i, j;

            if (x == 0) return x;
            if (y == 0) return y;
            int[] v0 = new int[(y + 1) << 1];

            for (i = 0; i < y + 1; i++) v0[i] = i;
            for (i = 0; i < x; i++)
            {
                v0[y + 1] = i + 1;
                for (j = 0; j < y; j++)
                    v0[y + j + 2] = Math.Min(Math.Min(v0[y + j + 1], v0[j + 1]) + 1, v0[j] + ((a[i] == b[j]) ? 0 : 1));
                for (j = 0; j < y + 1; j++) v0[j] = v0[y + j + 1];
            }
            return v0[y + y + 1];
        }

        // https://stackoverflow.com/questions/1601834/c-implementation-of-or-alternative-to-strcmplogicalw-in-shlwapi-dll
        public class NaturalComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

                int lx = x.Length, ly = y.Length;

                for (int mx = 0, my = 0; mx < lx && my < ly; mx++, my++)
                {
                    if (char.IsDigit(x[mx]) && char.IsDigit(y[my]))
                    {
                        long vx = 0, vy = 0;

                        for (; mx < lx && char.IsDigit(x[mx]); mx++)
                            vx = vx * 10 + x[mx] - '0';

                        for (; my < ly && char.IsDigit(y[my]); my++)
                            vy = vy * 10 + y[my] - '0';

                        if (vx != vy)
                            return vx > vy ? 1 : -1;
                    }

                    if (mx < lx && my < ly && x[mx] != y[my])
                        return x[mx] > y[my] ? 1 : -1;
                }

                return lx - ly;
            }
        }
    }
}
