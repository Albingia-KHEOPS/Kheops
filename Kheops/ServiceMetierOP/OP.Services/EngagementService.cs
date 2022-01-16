using ALBINGIA.Framework.Common;
using OP.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services.BLServices {
    public class EngagementService {
        private readonly IDbConnection connection;
        //private readonly TraceRepository traceRepository;
        private readonly EngagementRepository repository;
        //private readonly GarantieModeleRepository garantieModeleRepository;
        //private readonly AffaireNouvelleRepository affaireRepository;
        //private readonly ControleFinRepository ctrlfinRepository;
        private readonly ProgramAS400Repository as400Repository;

        public EngagementService(IDbConnection connection, EngagementRepository engagementRepository, ProgramAS400Repository as400Repository) {
            this.connection = connection;
            this.repository = engagementRepository;
            this.as400Repository = as400Repository;
        }

        public void InitCopyEngagements(Folder offre, Folder contrat, string acteGestion, string user) {
            var pgmFolder = new PGMFolder(contrat) { ActeGestion = acteGestion, User = user };
            this.as400Repository.LoadEngagement(pgmFolder);
            this.as400Repository.InitMontantRef(pgmFolder, "CALCUL");
            this.repository.Copy(offre, contrat);
            this.as400Repository.LoadEngagement(pgmFolder);
        }
    }
}
