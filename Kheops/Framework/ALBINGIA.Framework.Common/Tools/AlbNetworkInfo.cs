using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ALBINGIA.Framework.Common.Tools
{
    public static class AlbNetworkInfo
    {
        public static string GetMachineName(HttpRequestBase request)
        {
            var domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var ipAdress = request.UserHostAddress;
            string machineName = string.Empty;
            try
            {
                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(ipAdress);

                machineName = hostEntry.HostName;
            }
            catch (Exception)
            {
                // Machine not found...
            }

            return machineName.Replace("." + domainName, string.Empty);
        }

        public static string GetIpMachine()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

    }
}
