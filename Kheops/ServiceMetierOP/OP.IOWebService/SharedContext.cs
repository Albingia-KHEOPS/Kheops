using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OP.IOWebService
{
    internal class SharedContext : ISessionContext
    {
        public string SessionId => "Shared";
        public string UserId => string.Empty;

        public int Timeout => HttpContext.Current?.Session?.Timeout ?? 10;
    }
}