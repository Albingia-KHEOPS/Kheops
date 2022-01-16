using Albingia.Kheops.OP.Application.Contracts;
using System.Web;

namespace ALBINGIA.OP.OP_MVC {
    internal class SessionContext : ISessionContext
    {
        public string SessionId => HttpContext.Current?.Session?.SessionID ?? string.Empty;
        public string UserId => (string)HttpContext.Current?.Session?["AS400User"] ?? string.Empty;

        public int Timeout => HttpContext.Current?.Session?.Timeout ?? 20;
    }

    internal class SharedContext : ISessionContext
    {
        public string SessionId => "Shared";
        public string UserId => string.Empty;

        public int Timeout => HttpContext.Current?.Session?.Timeout ?? 20;
    }
}