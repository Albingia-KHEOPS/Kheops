using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class SinistreRepository : ISinistreRepository {
        private readonly IReferentialRepository referentialRepository;
        private readonly YSinistRepository ySinistRepository;
        private readonly YSinCumRepository ySinCumRepository;
        public SinistreRepository(IReferentialRepository referentialRepository, YSinistRepository ySinistRepository, YSinCumRepository ySinCumRepository) {
            this.ySinistRepository = ySinistRepository;
            this.referentialRepository = referentialRepository;
            this.ySinCumRepository = ySinCumRepository;
        }

        public PagingList<Sinistre> GetAll(string[] branches, int page = 0, int codeAssure = 0, int nbAnnees = 3) {
            var pagingList = this.ySinistRepository.SelectAll(branches, page, codeAssure, nbAnnees);
            var list = pagingList.List.Select(s => {
                return new Sinistre {
                    Affaire = new Affaire {
                        CodeAffaire = s.Siipb,
                        NumeroAliment = s.Sialx,
                        TypeAffaire = AffaireType.Contrat,
                        NumeroAvenant = s.Pbavn,
                        DateEffet = s.PbEf,
                        DateEffetAvenant = s.PbEfAv,
                        Branche = this.referentialRepository.GetValue<Branche>(s.Pbbra)
                    },
                    Id = new SinistreId {
                        DateSurvenance = s.MakeDateTimeFromAMJH(nameof(s.Sisua)).Value,
                        Numero = s.Sinum,
                        SousBranche = new Branche() { Code = s.Sisbr }
                    },
                    DateOuverture = s.MakeDateTimeFromAMJH(nameof(s.Sioua)).Value,
                    MontantObjet = (ulong)s.Simts,
                    Situation = this.referentialRepository.GetValue<SituationSinistre>(s.Sisit),
                    IgnorerJour = s.Sisuj < 1
                };
            }).ToList();

            return new PagingList<Sinistre> {
                List = list,
                NbTotalLines = pagingList.NbTotalLines,
                PageNumber = pagingList.PageNumber,
                Totals = pagingList.Totals
            };
        }

        public IEnumerable<Sinistre> GetAllOfPreneur(int codePreneur) {
            return this.ySinistRepository.SelectAllOfPreneur(codePreneur, 0).Select(s => new Sinistre {
                Affaire = new Affaire {
                    CodeAffaire = s.Siipb,
                    NumeroAliment = s.Sialx,
                    TypeAffaire = AffaireType.Contrat,
                },
                Id = new SinistreId {
                    DateSurvenance = s.MakeDateTimeFromAMJH(nameof(s.Sisua)).Value,
                    Numero = s.Sinum,
                    SousBranche = new Branche() { Code = s.Sisbr }
                },
                DateOuverture = s.MakeDateTimeFromAMJH(nameof(s.Sioua)).Value,
                MontantObjet = (ulong)s.Simts,
                Situation = this.referentialRepository.GetValue<SituationSinistre>(s.Sisit)
            });
        }

        public decimal GetSumOfChargementsSinistresPreneur(int codePreneur) {
            var cumuls = this.ySinCumRepository.SelectCumulsPreneur(codePreneur);
            return cumuls?.Any() != true ? 0 : cumuls.Sum(c => c.Sutch);
        }
    }
}
