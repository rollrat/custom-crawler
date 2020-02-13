/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CustomCrawler
{
    public class URLHelper
    {
        Uri url;

        public URLHelper(string url)
        {
            this.url = new Uri(url);
        }

        Dictionary<string, string> buffer;

        public Dictionary<string, string> Parameters
        {
            get
            {
                if (buffer != null)
                    return buffer;
                var pp = HttpUtility.ParseQueryString(url.Query);
                var result = new Dictionary<string, string>();
                foreach (var key in pp.AllKeys)
                    result.Add(key, pp[key]);
                return buffer = result;
            }
        }

        public List<string> DiffParams(URLHelper url)
        {
            var p1 = Parameters;
            var p2 = url.Parameters;

            return p1.Keys.Intersect(p2.Keys).Where(x => p1[x] != p2[x]).ToList();
        }
    }
}
