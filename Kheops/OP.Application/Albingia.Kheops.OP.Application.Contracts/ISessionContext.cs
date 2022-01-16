using System;

namespace Albingia.Kheops.OP.Application.Contracts {
    public interface ISessionContext
    {
        string UserId { get;  }
        string SessionId {get;}
        int Timeout { get; }
    }
}