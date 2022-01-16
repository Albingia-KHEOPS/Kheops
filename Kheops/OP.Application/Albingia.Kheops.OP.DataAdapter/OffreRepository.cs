using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class OffreRepository : IOffreRepository {
        private readonly IReferentialRepository referentialRepository;
        private readonly IAffaireRepository affaireRepository;
        private readonly INomsCourtiersRepository nomsCourtiersRepository;
        private readonly YpoBaseRepository ypoBaseRepository;
        private readonly KpEntRepository kpEntRepository;
        private readonly KpCotisRepository KpCotisRepository;
        public OffreRepository(YpoBaseRepository ypoBaseRepository, KpEntRepository kpEntRepository, KpCotisRepository KpCotisRepository, IReferentialRepository referentialRepository, IAffaireRepository affaireRepository, INomsCourtiersRepository nomsCourtiersRepository) {
            this.ypoBaseRepository = ypoBaseRepository;
            this.referentialRepository = referentialRepository;
            this.affaireRepository = affaireRepository;
            this.nomsCourtiersRepository = nomsCourtiersRepository;
            this.KpCotisRepository = KpCotisRepository;
            this.kpEntRepository = kpEntRepository;
        }
        public PagingList<Affaire> GetRelancesByGESorSOU(string gestionnaire, string souscripteur, int page = 0) {
            var result = this.ypoBaseRepository.GetRelancesByAny(gestionnaire, souscripteur, page);
            var nomsCabinets = this.nomsCourtiersRepository.GetNomsCabinets(result.List.Select(x => x.pobase.Pbict).Distinct().Where(x => x > 0));
            return new PagingList<Affaire> {
                NbTotalLines = result.NbTotalLines,
                PageNumber = result.PageNumber,
                List = result.List.Select(x => new Affaire {
                    Branche = this.referentialRepository.GetValue<Branche>(x.pobase.Pbbra),
                    CodeAffaire = x.pobase.Pbipb,
                    NumeroAliment = x.pobase.Pbalx,
                    TypeAffaire = AffaireType.Offre,
                    NumeroAvenant = 0,
                    Descriptif = x.pobase.Pbref,
                    Situation = x.pobase.Pbsit.ParseCode<SituationAffaire>(),
                    MotifSituation = this.referentialRepository.GetValue<MotifSituation>(x.pobase.Pbstf ?? string.Empty),
                    DateValidation = x.pobase.Pbsta == 0 ? default(DateTime?) : new DateTime(x.pobase.Pbsta, x.pobase.Pbstm, x.pobase.Pbstj),
                    Souscripteur = this.referentialRepository.GetUtilisateur(x.pobase.Pbsou),
                    Gestionnaire = this.referentialRepository.GetUtilisateur(x.pobase.Pbges),
                    DelaisRelanceJours = x.pobase.Pbrld,
                    CourtierGestionnaire = x.pobase.Pbict < 1 ? null : new Courtier { Code = x.pobase.Pbict, Nom = nomsCabinets.TryGetValue(x.pobase.Pbict, out string s) ? s : string.Empty },
                    IsAttenteDocumentsCourtier = x.kpent.Kaaattdoc.AsBoolean()
                }).ToList()
            };
        }

        public CotisationOffre GetCotisationOffre(string code, int version) {
            var cotis = this.KpCotisRepository.GetByAffaire(AffaireType.Offre.AsCode(), code.ToIPB(), version).FirstOrDefault();
            return new CotisationOffre {
                CodeOffre = cotis.Kdmipb,
                VersionOffre = cotis.Kdmalx,
                TauxCommission = cotis.Kdmcmf
            };
        }

        public Affaire GetSingleRelance(AffaireId affaireId) {
            var y = this.ypoBaseRepository.Get(AffaireType.Offre.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            var k = this.kpEntRepository.Get(AffaireType.Offre.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            var nomsCabinets = y.Pbict > 0 ? this.nomsCourtiersRepository.GetNomsCabinets(new[] { y.Pbict }) : new Dictionary<int, string>();
            return new Affaire {
                Branche = this.referentialRepository.GetValue<Branche>(y.Pbbra),
                CodeAffaire = y.Pbipb,
                NumeroAliment = y.Pbalx,
                TypeAffaire = AffaireType.Offre,
                NumeroAvenant = 0,
                Descriptif = y.Pbref,
                Situation = y.Pbsit.ParseCode<SituationAffaire>(),
                MotifSituation = this.referentialRepository.GetValue<MotifSituation>(y.Pbstf ?? string.Empty),
                DateValidation = y.Pbsta == 0 ? default(DateTime?) : new DateTime(y.Pbsta, y.Pbstm, y.Pbstj),
                Souscripteur = this.referentialRepository.GetUtilisateur(y.Pbsou),
                Gestionnaire = this.referentialRepository.GetUtilisateur(y.Pbges),
                DelaisRelanceJours = y.Pbrld,
                CourtierGestionnaire = y.Pbict < 1 ? null : new Courtier { Code = y.Pbict, Nom = nomsCabinets.TryGetValue(y.Pbict, out string s) ? s : string.Empty },
                IsAttenteDocumentsCourtier = k.Kaaattdoc.AsBoolean()
            };
        }

        public void UpdateSituationsRelances(IEnumerable<Affaire> affaires, string user) {
            var poList = this.ypoBaseRepository.SelectMany(affaires.Select(x => (x.CodeAffaire, x.NumeroAliment)));
            var date = DateTime.Today;
            poList.ToList().ForEach(ypo => {
                var a = affaires.First(x => x.CodeAffaire == ypo.Pbipb && x.NumeroAliment == ypo.Pbalx && ypo.Pbtyp == AffaireType.Offre.AsCode());
                ypo.Pbsit = a.Situation.AsCode();
                ypo.Pbstf = a.CodeMotifSituation;
                ypo.Pbmju = user;
                ypo.Pbmja = date.Year;
                ypo.Pbmjm = date.Month;
                ypo.Pbmjj = date.Day;
                this.ypoBaseRepository.Update(ypo);
            });
        }

        public void UpdateDelaisRelances(IEnumerable<Affaire> affaires, string user) {
            var poList = this.ypoBaseRepository.SelectMany(affaires.Select(x => (x.CodeAffaire, x.NumeroAliment)));
            var date = DateTime.Today;
            poList.ToList().ForEach(ypo => {
                var a = affaires.First(x => x.CodeAffaire == ypo.Pbipb && x.NumeroAliment == ypo.Pbalx && ypo.Pbtyp == AffaireType.Offre.AsCode());
                ypo.Pbrld = a.DelaisRelanceJours;
                ypo.Pbmju = user;
                ypo.Pbmja = date.Year;
                ypo.Pbmjm = date.Month;
                ypo.Pbmjj = date.Day;
                this.ypoBaseRepository.Update(ypo);
            });
        }

        public void UpdateFlagAttenteCourtier(IEnumerable<(string ipb, int alx, bool? expecting)> flags, string userId) {
            flags.ToList().ForEach(x => {
                var kpent = this.kpEntRepository.Get(AffaireType.Offre.AsCode(), x.ipb, x.alx);
                kpent.Kaaattdoc = x.expecting.ToYesNo();
                this.kpEntRepository.Update(kpent);
            });
        }
    }
}
