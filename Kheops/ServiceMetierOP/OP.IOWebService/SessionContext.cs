using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using ALBINGIA.Framework.Common.Tools;
using System.Web;

namespace OP.IOWebService
{
    internal class SessionContext : ISessionContext
    {
        public string SessionId => "1";
        public string UserId => WCFHelper.GetFromHeader("UserAS400") ?? WCFHelper.GetFromHeader("User");

        public int Timeout => HttpContext.Current?.Session?.Timeout??10;
    }
}