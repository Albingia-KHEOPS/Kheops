using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using DataAccess.Helpers;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class PrimeRepository : IPrimeRepository {
        private readonly YPrimeRepository yPrimeRepository;
        private readonly YPriPGaRepository yPriPGaRepository;
        private readonly YPrimGaRepository yPrimGaRepository;
        private readonly IReferentialRepository referentialRepository;

        public PrimeRepository(YPrimeRepository yPrimeRepository, YPriPGaRepository yPriPGaRepository, YPrimGaRepository yPrimGaRepository, IReferentialRepository referentialRepository) {
            this.yPrimeRepository = yPrimeRepository;
            this.referentialRepository = referentialRepository;
            this.yPriPGaRepository = yPriPGaRepository;
            this.yPrimGaRepository = yPrimGaRepository;
        }

        public PagingList<(Affaire, Prime)> GetImpayes(string[] branches, int page = 0, int codeAssure = 0) {
            var pagingList = this.yPrimeRepository.SelectImpayes(branches, page, codeAssure);
            return new PagingList<(Affaire, Prime)> {
                List = BuildAffairePrime(pagingList),
                NbTotalLines = pagingList.NbTotalLines,
                PageNumber = pagingList.PageNumber,
                Totals = pagingList.Totals
            };
        }

        public PagingList<(Affaire affaire, Prime prime)> GetRetardsPaiement(string[] branches, int page = 0, int codeAssure = 0) {
            var pagingList = this.yPrimeRepository.SelectRetardsPaiement(branches, page, codeAssure);
            return new PagingList<(Affaire, Prime)> {
                List = BuildAffairePrime(pagingList),
                NbTotalLines = pagingList.NbTotalLines,
                PageNumber = pagingList.PageNumber,
                Totals = pagingList.Totals
            };
        }

        public IEnumerable<PrimeGarantie> GetPrimesGaranties(AffaireId affaireId, bool isReadonly = false) {
            if (affaireId.IsHisto || isReadonly) {
                var primesga = this.yPrimGaRepository.GetByAffaire(affaireId.CodeAffaire, affaireId.NumeroAliment);
                foreach (var pga in primesga) {
                    yield return new PrimeGarantie {
                        AffaireId = affaireId,
                        CodeGarantie = pga.Plgar,
                        MontantComptable = new Domain.Transverse.Montant {
                            Taxe = pga.Plkhx,
                            Valeur = pga.Plktt,
                            ValeurHorsTaxe = pga.Plkht
                        },
                        MontantDevise = new Domain.Transverse.Montant {
                            Taxe = pga.Plmtx,
                            Valeur = pga.Plmtt,
                            ValeurHorsTaxe = pga.Plmht
                        },
                        TypePart = pga.Pltye
                    };
                }
            }
            else {
                var pripga = this.yPriPGaRepository.GetByAffaire(affaireId.CodeAffaire, affaireId.NumeroAliment);
                foreach (var ppga in pripga) {
                    yield return new PrimeGarantie {
                        AffaireId = affaireId,
                        CodeGarantie = ppga.Plgar,
                        MontantComptable = new Domain.Transverse.Montant {
                            Taxe = ppga.Plkhx,
                            Valeur = ppga.Plktt,
                            ValeurHorsTaxe = ppga.Plkht
                        },
                        MontantDevise = new Domain.Transverse.Montant {
                            Taxe = ppga.Plmtx,
                            Valeur = ppga.Plmtt,
                            ValeurHorsTaxe = ppga.Plmht
                        },
                        TypePart = ppga.Pltye
                    };
                }
            }
        }

        private List<(Affaire, Prime)> BuildAffairePrime(PagingList<YpoPrimeRetard> pagingList) {
            return pagingList.List.Select(p => (
                new Affaire {
                    CodeAffaire = p.Pkipb,
                    NumeroAliment = p.Pkalx,
                    TypeAffaire = AffaireType.Contrat,
                    NumeroAvenant = p.Pbavn,
                    Branche = new Branche { Code = p.Pbbra },
                    CourtierGestionnaire = p.Tninl == 0 && p.Tntnm == "A" ? new Domain.Affaire.Courtier { Code = p.Pbict, Nom = p.Tnnom } : null,
                    DateValidation = p.DateValidation
                },
                new Prime {
                    Id = new PrimeId { CodeAffaire = p.Pkipb, NumeroAliment = p.Pkalx, TypeAffaire = AffaireType.Contrat, Numero = p.Pkipk, NumeroAvenant = p.Pkavn },
                    MontantHT = p.Pkmht,
                    MontantTT = p.Pkmtt,
                    NumeroEcheance = p.Pkavi,
                    DateEcheance = p.DateEcheance,
                    TypeRelance = this.referentialRepository.GetValue<TypeRelance>(p.Pkrlc)
                })).ToList();
        }
    }
}
