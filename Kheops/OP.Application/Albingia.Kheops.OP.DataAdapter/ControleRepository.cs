using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public class ControleRepository : IControleRepository {
        private readonly KpCtrlRepository kpCtrlRepository;
        private readonly HpctrlRepository hpctrlRepository;
        private readonly KpCtrlERepository kpCtrleRepository;
        private readonly HpctrleRepository hpctrleRepository;
        private readonly KpCtrlARepository kpCtrlARepository;

        public ControleRepository(
            KpCtrlRepository kpCtrlRepository,
            HpctrlRepository hpctrlRepository,
            KpCtrlERepository kpCtrleRepository,
            HpctrleRepository hpctrleRepository,
            KpCtrlARepository kpCtrlARepository) {
            this.kpCtrlRepository = kpCtrlRepository;
            this.hpctrlRepository = hpctrlRepository;
            this.kpCtrleRepository = kpCtrleRepository;
            this.kpCtrlARepository = kpCtrlARepository;
            this.hpctrleRepository = hpctrleRepository;
        }

        public void Reprise(AffaireId id) {
            var crtlList = this.kpCtrlRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            crtlList.ForEach(x => this.kpCtrlRepository.Delete(x));
            var hcrtlList = this.hpctrlRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hcrtlList.ForEach(x => this.hpctrlRepository.Delete(x));
            hcrtlList.ForEach(x => this.kpCtrlRepository.Insert(x));
            var ctrleList = this.kpCtrleRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            ctrleList.ForEach(x => this.kpCtrleRepository.Delete(x));
            var hpctrleList = this.hpctrleRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            this.hpctrleRepository.DeleteByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            hpctrleList.ForEach(x => this.kpCtrleRepository.Insert(x));
        }

        public void InsertControlAssiette(AffaireId affaireId, string user, string etape, string libelle) {
            var ctrla = this.kpCtrlARepository.Get(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            if (ctrla is null) {
                var ctrle = this.kpCtrleRepository.GetByAffaire(affaireId.CodeAffaire, affaireId.NumeroAliment).FirstOrDefault(x => x.Kevetape == "COT");
                if (ctrle is null) {
                    return;
                }
                this.kpCtrleRepository.Delete(ctrle);
                this.kpCtrlARepository.Insert(new DataModel.DTO.KpCtrlA {
                    Kgtalx = affaireId.NumeroAliment,
                    Kgtetape = etape.IsEmptyOrNull() ? "GAR" : etape,
                    Kgtlib = libelle.IsEmptyOrNull() ? "S'applique à" : libelle,
                    Kgttyp = affaireId.TypeAffaire.AsCode(),
                    Kgtipb = affaireId.CodeAffaire,
                    Kgtcru = user,
                    Kgtmaju = user
                });
            }
        }

        public void AnnulerEtapes(string codeAffaire, int version, IEnumerable<string> etapes, IEnumerable<int> risques = null) {
            var kpEtapes = this.kpCtrleRepository.GetByAffaire(codeAffaire, version);
            var etatpesRisques = (!risques?.Any() ?? true)
                ? Enumerable.Empty<KpCtrlE>()
                : kpEtapes.Where(e => risques.Contains(e.Kevrsq) && e.Kevrsq != 0 && etapes.Contains(e.Kevetape));
            // delete step marked with Risque
            etatpesRisques.ToList().ForEach(e => {
                this.kpCtrleRepository.DeleteByEtapeRisque(codeAffaire, version, e.Kevetape, e.Kevrsq);
            });
            kpEtapes.Where(e => etapes.Contains(e.Kevetape) && e.Kevrsq == 0).ToList().ForEach(e => {
                // for GAR step, delete only Formule filtrered by Risque (if any)
                if (!etatpesRisques.Any() || etatpesRisques.Any(x => x.Kevfor == e.Kevfor)) {
                    this.kpCtrleRepository.Delete(e);
                }
            });
        }
    }
}
