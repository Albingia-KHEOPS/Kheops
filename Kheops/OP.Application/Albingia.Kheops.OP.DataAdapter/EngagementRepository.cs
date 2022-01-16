using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Engagements;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public partial class EngagementRepository : IEngagementRepository {
        private readonly KpEngRepository kpEngRepository;
        private readonly KpEngFamRepository kpEngfamRepository;
        private readonly KpEngGarRepository kpEnggarRepository;
        private readonly KpEngRsqRepository kpEngrsqRepository;
        private readonly KpEngVenRepository kpEngvenRepository;
        private readonly HpengRepository hpengRepository;
        private readonly HpengfamRepository hpengfamRepository;
        private readonly HpenggarRepository hpenggarRepository;
        private readonly HpengrsqRepository hpengrsqRepository;
        private readonly HpengvenRepository hpengvenRepository;

        public EngagementRepository(
            KpEngRepository kpEngRepository,
            KpEngFamRepository kpEngfamRepository,
            KpEngGarRepository kpEnggarRepository,
            KpEngRsqRepository kpEngrsqRepository,
            KpEngVenRepository kpEngvenRepository,
            HpengRepository hpengRepository,
            HpengfamRepository hpengfamRepository,
            HpenggarRepository hpenggarRepository,
            HpengrsqRepository hpengrsqRepository,
            HpengvenRepository hpengvenRepository) {
            this.kpEngRepository = kpEngRepository;
            this.kpEngfamRepository = kpEngfamRepository;
            this.kpEnggarRepository = kpEnggarRepository;
            this.kpEngrsqRepository = kpEngrsqRepository;
            this.kpEngvenRepository = kpEngvenRepository;
            this.hpengRepository = hpengRepository;
            this.hpengfamRepository = hpengfamRepository;
            this.hpenggarRepository = hpenggarRepository;
            this.hpengrsqRepository = hpengrsqRepository;
            this.hpengvenRepository = hpengvenRepository;
        }

        public Engagement GetEngagement(int id) {
            var eng = this.kpEngRepository.Get(id);
            return new Engagement {
                Id = (int)eng.Kdoid,
                Observation = new ALBINGIA.Framework.Business.Observation {
                    Folder = new ALBINGIA.Framework.Common.Folder(eng.Kdoipb, eng.Kdoalx, eng.Kdotyp[0]),
                    Id = (int)eng.Kdoobsv,
                    IsGenerale = false
                }
            };
        }

        public Engagement GetEngagement(string ipb, int alx, string type) {
            var eng = this.kpEngRepository.GetByAffaire(type, ipb.ToIPB(), alx).FirstOrDefault();
            return eng is null ? null : new Engagement {
                Id = (int)eng.Kdoid,
                Observation = new ALBINGIA.Framework.Business.Observation {
                    Folder = new ALBINGIA.Framework.Common.Folder(eng.Kdoipb, eng.Kdoalx, eng.Kdotyp[0]),
                    Id = (int)eng.Kdoobsv,
                    IsGenerale = false
                }
            };
        }

        public void UpdateTraiteSMP(string codeAffaire, int version, string type, IEnumerable<(long idVentilation, int codeRisque, long smp)> valeurs) {
            var ventilations = this.kpEngrsqRepository.GetByAffaire(type, codeAffaire, version);
            ventilations.ToList().ForEach(v => {
                var val = valeurs.FirstOrDefault(x => x.codeRisque == v.Kdrrsq && x.idVentilation == v.Kdrkdqid);
                if (val != default) {
                    if (v.Kdrsmp != val.smp) {
                        v.Kdrsmf = val.smp;
                        this.kpEngrsqRepository.Update(v);
                    }
                    else {
                        v.Kdrsmf = 0;
                        this.kpEngrsqRepository.Update(v);
                    }
                }
            });
        }
        public void UpdateTraiteSMPTotal(int id,int total)
        {
         
        }
        public void Reprise(AffaireId id) {
            var listEng = this.kpEngRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listEng.ForEach(x => this.kpEngRepository.Delete(x));
            var histoEng = this.hpengRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoEng.ForEach(x => this.hpengRepository.Delete(x));
            histoEng.ForEach(x => this.kpEngRepository.Insert(x));

            var listEngfam = this.kpEngfamRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listEngfam.ForEach(x => this.kpEngfamRepository.Delete(x));
            var histoEngfam = this.hpengfamRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoEngfam.ForEach(x => this.hpengfamRepository.Delete(x));
            histoEngfam.ForEach(x => this.kpEngfamRepository.Insert(x));

            var listEnggar = this.kpEnggarRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listEnggar.ForEach(x => this.kpEnggarRepository.Delete(x));
            var histoEnggar = this.hpenggarRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoEnggar.ForEach(x => this.hpenggarRepository.Delete(x));
            histoEnggar.ForEach(x => this.kpEnggarRepository.Insert(x));

            var listEngrsq = this.kpEngrsqRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listEngrsq.ForEach(x => this.kpEngrsqRepository.Delete(x));
            var histoEngrsq = this.hpengrsqRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoEngrsq.ForEach(x => this.hpengrsqRepository.Delete(x));
            histoEngrsq.ForEach(x => this.kpEngrsqRepository.Insert(x));

            var listEngven = this.kpEngvenRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            listEngven.ForEach(x => this.kpEngvenRepository.Delete(x));
            var histoEngven = this.hpengvenRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoEngven.ForEach(x => this.hpengvenRepository.Delete(x));
            histoEngven.ForEach(x => this.kpEngvenRepository.Insert(x));
        }
    }
}

