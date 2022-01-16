using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.DataAccess;
using System;
using System.Data;

namespace OP.Services.BLServices
{
    public class CibleService {
        private readonly IDbConnection connection;
        private readonly TraceRepository traceRepository;
        private readonly BrancheRepository brancheRepository;
        private readonly GarantieModeleRepository garantieModeleRepository;
        private readonly AffaireNouvelleRepository affaireRepository;
        private readonly ControleFinRepository ctrlfinRepository;
        private readonly ProgramAS400Repository as400Repository;

        internal static readonly string BrancheResponsabiliteCivile = "RC";
        internal static readonly string CibleEntreprise = "ETS";
        internal static readonly string RisquePro = "RP";
        internal static readonly string Habitation = "H";

        public CibleService(
            IDbConnection connection,
            TraceRepository traceRepository,
            BrancheRepository brancheRepository,
            GarantieModeleRepository garantieModeleRepository,
            AffaireNouvelleRepository affaireRepository,
            ControleFinRepository ctrlfinRepository,
            ProgramAS400Repository as400Repository
         ) {
            this.connection = connection;
            this.traceRepository = traceRepository;
            this.brancheRepository = brancheRepository;
            this.garantieModeleRepository = garantieModeleRepository;
            this.affaireRepository = affaireRepository;
            this.ctrlfinRepository = ctrlfinRepository;
            this.as400Repository = as400Repository;
        }
        
        public void UpdateSousBranche(Folder folder, string user) {
            DateTime now = DateTime.Now;
            (string branche, string cible, string ttr, int avn) = this.brancheRepository.GetBrancheCibleAvn(folder, user);
            folder.NumeroAvenant = avn;
            if (branche == BrancheResponsabiliteCivile && cible == CibleEntreprise) {
                this.traceRepository.DeleteTraceCC(folder);
                string newSousBranche = DefineSousBranche(folder);
                (string b, string sousBranche, string categorie) = this.brancheRepository.GetProduit(folder);
                if (sousBranche != newSousBranche) {
                    this.brancheRepository.ChangeSousBranche(folder, newSousBranche);
                    var pgmFolder = new PGMFolder(folder) { ActeGestion = ttr, User = user };
                    this.as400Repository.CalculateTauxPrimes(pgmFolder, branche, sousBranche, categorie);
                    var commissions = this.as400Repository.LoadCommissions(pgmFolder);
                    double taux = commissions.TauxContratHCAT, tauxCAT = commissions.TauxContratCAT;
                    this.affaireRepository.ModifyTaux(folder, taux, tauxCAT);
                    this.traceRepository.CreateTraceAvn(folder, user, now, ttr);
                    this.traceRepository.CreateTraceYpo(folder, $"Changmnt sous-branche : {newSousBranche}", user, now, ttr, AlbConstantesMetiers.ACTEGESTION_GESTION);
                    this.ctrlfinRepository.RemoveEtapeCOT(folder);
                }
            }
        }

        internal string DefineSousBranche(Folder folder) {
            if (this.garantieModeleRepository.CountByDEFG(folder, "P") > 0) {
                return RisquePro;
            }

            return Habitation;
        }
    }
}
