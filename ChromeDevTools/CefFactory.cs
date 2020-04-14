using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDevs.ChromeDevTools
{
    public class CefFactory : IChromeProcessFactory
    {
        public IChromeProcess Create(int port, bool headless)
        {
            return new CefChrome(new Uri("http://localhost:" + port));
        }
    }
}
