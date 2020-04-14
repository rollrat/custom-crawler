using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDevs.ChromeDevTools
{
    public class CefChrome : RemoteChromeProcess
    {
        public CefChrome(Uri remoteDebuggingUri)
            : base(remoteDebuggingUri)
        { 
        }
    }
}
