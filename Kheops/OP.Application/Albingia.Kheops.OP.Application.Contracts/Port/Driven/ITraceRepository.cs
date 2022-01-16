using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface ITraceRepository {
        TraceContext GetLastTrace(AffaireId affaireId);
        void TraceInfo(TraceContext traceContext);
        void Log(SimpleTrace trace);
        IEnumerable<SimpleTrace> FindLogsByCommentEndsWith(DateTime date, string endString);
        void DeleteTraceOldISTransfert(DateTime now, string guid);
    }
}
