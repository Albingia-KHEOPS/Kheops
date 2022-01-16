using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess;
using System;
using System.Data;

namespace OP.Services
{
    public class TraceService {
        readonly IDbConnection connection;
        readonly TraceRepository repository;
        readonly PoliceRepository policeRepository;

        public TraceService(IDbConnection connection, TraceRepository repository, PoliceRepository policeRepository) {
            this.connection = connection;
            this.repository = repository;
            this.policeRepository = policeRepository;
        }

        public void TraceActeGestion(Folder folder, string user, string typeActeGestion, string traceLabel, string codeTraitement) {
            if (!codeTraitement.IsIn(AlbConstantesMetiers.TRAITEMENT_MODIFHORSAVN, AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN, AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN_ANNUL)) {
                codeTraitement = this.policeRepository.GetTraitement(folder);
            }
            if (typeActeGestion == "V") {
                int nbTraces = this.repository.GetNbTracesPrimeEmise(folder, codeTraitement == "REGUL" ? "NR" : "NG");
                if (nbTraces > 0) {
                    traceLabel += "Non émise";
                }
            }

            if (!codeTraitement.IsIn("REGUL", "AVNRG") || typeActeGestion == "V") {
                this.repository.CreateTraceYpo(folder, traceLabel, user, DateTime.Now, codeTraitement, typeActeGestion);
            }
        }
    }
}
