// This source code is a part of Custom Copy Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using CefSharp;
using CefSharp.Wpf;
using CustomCrawlerDynamics.Code;
using CustomCrawlerDynamics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawlerDynamics.cefw
{
    public class Init
    {
        public static void InitializeProgram()
        {
            CefSettings set = new CefSettings();
            Session.Settings(ref set);
            set.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = "http",
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });
            set.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = "https",
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });
            Cef.Initialize(set);
        }

        internal class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
        {
            public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
            {
                if (request.Url.Split('?')[0].EndsWith(".js") && !JsRequestManager.Instance.Contains(request.Url))
                {
                    JsRequestManager.Instance.Register(request.Url, NetCommon.DownloadString(request.Url));
                }
                return null;
            }
        }
    }
}
