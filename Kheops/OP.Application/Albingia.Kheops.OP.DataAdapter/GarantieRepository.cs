using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public class GarantieRepository : IGarantieRepository {
        private readonly IReferentialRepository referentielRepository;
        private readonly IParamRepository paramRepository;
        private readonly KpGaranRepository garRepository;
        private readonly KpGarTarRepository kpGarTarRepository;
        private readonly HpgartarRepository hpgartarRepository;
        private readonly HpgaranRepository hgarRepository;
        public GarantieRepository(IReferentialRepository referentielRepository, IParamRepository paramRepository, KpGaranRepository kpGaranRepository, HpgaranRepository hgarRepository, KGaranRepository kgaranRepository, KpGarTarRepository kpGarTarRepository, HpgartarRepository hpgartarRepository) {
            this.garRepository = kpGaranRepository;
            this.hgarRepository = hgarRepository;
            this.referentielRepository = referentielRepository;
            this.paramRepository = paramRepository;
            this.kpGarTarRepository = kpGarTarRepository;
            this.hpgartarRepository = hpgartarRepository;
        }
        public Garantie GetById(long id) {
            return MapGarantie(this.garRepository.Get(id));
        }
        public Garantie GetLatestById(long id) {
            return MapGarantie(this.hgarRepository.GetLatestById(id).FirstOrDefault());
        }
        public Garantie GetHistoById(long id, int numeroAvenant) {
            return MapGarantie(this.hgarRepository.Get(id, numeroAvenant));
        }
        public Garantie GetBySequence(GarantieUniqueId uniqueId) {
            KpGaran gar = null;
            if (uniqueId.AffaireId.NumeroAvenant >= 0) {
                gar = this.hgarRepository.GetBySequence(uniqueId.AffaireId.CodeAffaire, uniqueId.AffaireId.NumeroAliment, uniqueId.AffaireId.TypeAffaire.AsCode(), uniqueId.AffaireId.NumeroAvenant.Value, uniqueId.Sequence).FirstOrDefault();
            }
            if (gar is null) {
                gar = this.garRepository.GetBySequence(uniqueId.AffaireId.CodeAffaire, uniqueId.AffaireId.NumeroAliment, uniqueId.AffaireId.TypeAffaire.AsCode(), uniqueId.Sequence).FirstOrDefault();
            }
            if (gar is null) {
                return null;
            }
            return new Garantie {
                Id = gar.Kdeid,
                ParamGarantie = new Domain.Parametrage.Formules.ParamGarantieHierarchie {
                    Sequence = gar.Kdeseq
                },
                CodeGarantie = gar.Kdegaran,
                Formule = gar.Kdefor,
                Caractere = gar.Kdecar.AsEnum<CaractereSelection>(),
                Nature = gar.Kdenat.AsEnum<NatureValue>(),
                NumeroAvenant = gar.Kdeavn,
                TypeAlimentation = gar.Kdeala.AsEnum<AlimentationValue>(),
                Tri = gar.Kdetri,
                NumeroAvenantCreation = gar.Kdecravn,
                NumeroAvenantModif = gar.Kdemajavn,
                Taxe = this.referentielRepository.GetValue<Taxe>(gar.Kdetaxcod),
                DateDebut = Tools.MakeNullableDateTime(gar.Kdedatdeb, gar.Kdeheudeb, true),
                DateFinDeGarantie = Tools.MakeNullableDateTime(gar.Kdedatfin, gar.Kdeheufin, true),
                DefinitionGarantie = this.referentielRepository.GetValue<DefinitionGarantie>(gar.Kdedefg),
                Duree = string.IsNullOrWhiteSpace(gar.Kdeduruni) ? default(int?) : gar.Kdeduree,
                DureeUnite = this.referentielRepository.GetValue<UniteDuree>(gar.Kdeduruni),
                InventaireSpecifique = gar.Kdeinvsp.AsNullableBool(),
                IsAlimMontantReference = gar.Kdealiref.AsNullableBool(),
                IsApplicationCATNAT = gar.Kdecatnat.AsNullableBool(),
                IsFlagModifie = gar.Kdemodi.AsNullableBool(),
                IsGarantieAjoutee = gar.Kdeajout.AsBool(),
                IsIndexe = gar.Kdeina.AsNullableBool(),
                IsParameIndex = gar.Kdepind.AsNullableBool(),
                NatureRetenue = gar.Kdegan.AsEnum<NatureValue>(),
                NumDePresentation = gar.Kdenumpres.AsDecimal(),
                ParamCodetaxe = this.referentielRepository.GetValue<Taxe>(gar.Kdeptaxc),
                ParametrageCATNAT = gar.Kdepcatn.AsNullableBool(),
                ParametrageIsAlimMontantRef = gar.Kdepref.AsBool(),
                ParamIsNatModifiable = gar.Kdepntm.AsNullableBool(),
                ParamTypeAlimentation = gar.Kdepala.AsEnum<AlimentationValue>(),
                RepartitionTaxe = gar.Kdetaxrep.AsDecimal()
            };
        }

        public string GetRefLibelle(string code) {
            var paramsGar = this.paramRepository.GetAllGaranties();
            return paramsGar.FirstOrDefault(x => x.CodeGarantie == code)?.DesignationGarantie;
        }

        public IEnumerable<Garantie> GetGaranties(AffaireId affaireId) {
            IEnumerable<KpGaran> list = null;
            if (affaireId.IsHisto) {
                list = this.hgarRepository.GetByIpbAlx(affaireId.CodeAffaire, affaireId.NumeroAliment);
            }
            else {
                list = this.garRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            }

            return list.Select(x => MapGarantie(x));
        }

        public IEnumerable<Garantie> GetGarantiesWithTarifs(AffaireId affaireId) {
            IEnumerable<KpGaran> list = null;
            IEnumerable<KpGarTar> tarifs = null;
            if (affaireId.IsHisto) {
                list = this.hgarRepository.GetByIpbAlx(affaireId.CodeAffaire, affaireId.NumeroAliment);
                tarifs = this.hpgartarRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant.Value);
            }
            else {
                list = this.garRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
                tarifs = this.kpGarTarRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            }

            return list.Select(x => MapGarantieTarif(x, tarifs.FirstOrDefault(t => t.Kdgkdeid == x.Kdeid)));
        }

        public IEnumerable<Garantie> FindRCGaranties(AffaireId affaireId) {
            var list = this.garRepository.GetGarantieByCodeFilter(
                affaireId.CodeAffaire,
                affaireId.NumeroAliment,
                affaireId.TypeAffaire.AsCode(),
                new[] { AlbOpConstants.RCFrance, AlbOpConstants.RCExport, AlbOpConstants.RCUSA });

            var hlist = this.hgarRepository.GetGarantieByCodeFilter(
                affaireId.CodeAffaire,
                affaireId.NumeroAliment,
                new[] { AlbOpConstants.RCFrance, AlbOpConstants.RCExport, AlbOpConstants.RCUSA });

            var result = list.ToList();
            result.AddRange(hlist);
            return result.Select(x => MapGarantie(x, this.referentielRepository));
        }

        public IDictionary<(int idGar, string codeGar), (ValeursOptionsTarif assiete, TarifGarantie tarif)> GetConditionsGaranties(AffaireId affaireId, int option, int formule) {
            IEnumerable<KpGarTar> gartarList = null;
            IEnumerable<KpGaran> garList = null;
            if (affaireId.IsHisto) {
                garList = this.hgarRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant.Value);
                gartarList = this.hpgartarRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant.Value);
            }
            else {
                garList = this.garRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
                gartarList = this.kpGarTarRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            }

            var assiettes = garList.Where(g => g.Kdefor == formule && g.Kdeopt == option).ToDictionary(g => g.Kdeid, g => BuildAssieteCondition(g));
            return gartarList
                .Where(g => g.Kdgfor == formule && g.Kdgopt == option)
                .ToDictionary(trf => ((int)trf.Kdgkdeid, trf.Kdggaran), trf => (assiettes[trf.Kdgkdeid], BuildTarifCondition(trf)));
        }

        public IEnumerable<string> UpdateConditions(AffaireId affaireId, IDictionary<int, (ValeursOptionsTarif assiete, TarifGarantie tarif)> tarifs) {
            var gtarList = this.kpGarTarRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            foreach (var t in gtarList) {
                var cd = tarifs.FirstOrDefault(x => x.Value.tarif.Id == t.Kdgid);
                if (cd.Value.tarif != null && UpdateTarif(t, cd.Value.tarif)) {
                    yield return t.Kdggaran;
                }
            }
            var kpgarList = this.garRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            foreach (var k in kpgarList) {
                if (tarifs.TryGetValue((int)k.Kdeid, out var v)) {
                    if (UpdateAssiette(k, v.assiete, affaireId.NumeroAvenant.GetValueOrDefault())) {
                        yield return k.Kdegaran;
                    }
                }
            }
        }

        public bool TryUpdateTarif(TarifGarantie tarif) {
            var t = this.kpGarTarRepository.Get(tarif.Id);
            return UpdateTarif(t, tarif);
        }

        public ParamGarantie GetParamGarantie(string codeGarantie) {
            return this.paramRepository.GetGarantie(codeGarantie);
        }

        private ValeursOptionsTarif BuildAssieteCondition(KpGaran gar) {
            return new ValeursOptionsTarif {
                Base = this.referentielRepository.GetBase(Domain.Parametrage.Formules.TypeDeValeur.Assiette, gar.Kdeasbase),
                IsModifiable = gar.Kdeasmod.AsBoolean(),
                IsObligatoire = gar.Kdeasobli.AsBoolean(),
                Unite = this.referentielRepository.GetUnite(Domain.Parametrage.Formules.TypeDeValeur.Assiette, gar.Kdeasunit),
                ValeurActualise = gar.Kdeasvala,
                ValeurTravail = gar.Kdeasvalw,
                ValeurOrigine = gar.Kdeasvalo
            };
        }

        private TarifGarantie BuildTarifCondition(KpGarTar garTar) {
            return new TarifGarantie {
                Id = garTar.Kdgid,
                NumeroTarif = (short)garTar.Kdgnumtar,
                NumeroAvenant = garTar.Kdgavn,
                PrimeProvisionnelle = garTar.Kdgprimpro,
                LCI = new ValeursOptionsTarif {
                    Base = this.referentielRepository.GetBase(TypeDeValeur.LCI, garTar.Kdglcibase),
                    Unite = this.referentielRepository.GetUnite(TypeDeValeur.LCI, garTar.Kdglciunit),
                    IsModifiable = garTar.Kdglcimod.AsBoolean(),
                    IsObligatoire = garTar.Kdglciobl.AsBoolean(),
                    ValeurActualise = garTar.Kdglcivala,
                    ValeurTravail = garTar.Kdglcivalw,
                    ValeurOrigine = garTar.Kdglcivalo,
                    ExpressionComplexe = garTar.Kdgkdiid > 0 ? new ExpressionComplexeBase { Id = garTar.Kdgkdiid } : null
                },
                Franchise = new ValeursOptionsTarif {
                    Base = this.referentielRepository.GetBase(TypeDeValeur.Franchise, garTar.Kdgfrhbase),
                    Unite = this.referentielRepository.GetUnite(TypeDeValeur.Franchise, garTar.Kdgfrhunit),
                    IsModifiable = garTar.Kdgfrhmod.AsBoolean(),
                    IsObligatoire = garTar.Kdgfrhobl.AsBoolean(),
                    ValeurActualise = garTar.Kdgfrhvala,
                    ValeurTravail = garTar.Kdgfrhvalw,
                    ValeurOrigine = garTar.Kdgfrhvalo,
                    ExpressionComplexe = garTar.Kdgkdkid > 0 ? new ExpressionComplexeBase { Id = garTar.Kdgkdkid } : null
                },
                PrimeValeur = new ValeursOptionsTarif {
                    ValeurActualise = garTar.Kdgprivalo,
                    ValeurTravail = garTar.Kdgprivalw,
                    ValeurOrigine = garTar.Kdgprivala,
                    Unite = garTar.Kdgpriunit.IsEmptyOrNull() ? null : this.referentielRepository.GetUnite(TypeDeValeur.Prime, garTar.Kdgpriunit),
                    IsModifiable = garTar.Kdgprimod.AsBoolean(),
                    IsObligatoire = garTar.Kdgpriobl.AsBoolean()
                }
            };
        }

        private bool UpdateTarif(KpGarTar t, TarifGarantie tarif) {
            bool mustUpdate = HasTarifChanged(t, tarif);
            if (mustUpdate) {
                if (tarif.LCI != null) {
                    mustUpdate = true;
                    t.Kdglcivalo = tarif.LCI.ValeurOrigine;
                    t.Kdglcivalw = tarif.LCI.ValeurTravail;
                    t.Kdglcivala = tarif.LCI.ValeurActualise;
                    t.Kdglciunit = tarif.LCI.Unite.Code;
                    t.Kdglcibase = tarif.LCI.Base.Code;
                    if ((tarif.LCI.ExpressionComplexe is null || tarif.LCI.ExpressionComplexe.Id < 1) && tarif.LCI.Unite.Code != ValeursUnite.CodeUniteComplexe) {
                        t.Kdgkdiid = 0;
                    }
                    else {
                        t.Kdgkdiid = tarif.LCI.ExpressionComplexe.Id;
                    }
                }
                if (tarif.Franchise != null) {
                    t.Kdgfrhvalo = tarif.Franchise.ValeurOrigine;
                    t.Kdgfrhvalw = tarif.Franchise.ValeurTravail;
                    t.Kdgfrhvala = tarif.Franchise.ValeurActualise;
                    t.Kdgfrhunit = tarif.Franchise.Unite.Code;
                    t.Kdgfrhbase = tarif.Franchise.Base.Code;
                    if ((tarif.Franchise.ExpressionComplexe is null || tarif.Franchise.ExpressionComplexe.Id < 1) && tarif.Franchise.Unite.Code != ValeursUnite.CodeUniteComplexe) {
                        t.Kdgkdkid = 0;
                    }
                    else {
                        t.Kdgkdkid = tarif.Franchise.ExpressionComplexe.Id;
                    }
                }
                if (tarif.PrimeValeur != null) {
                    t.Kdgprivalo = tarif.PrimeValeur.ValeurOrigine;
                    t.Kdgprivalw = tarif.PrimeValeur.ValeurTravail;
                    t.Kdgprivala = tarif.PrimeValeur.ValeurActualise;
                    t.Kdgpriunit = tarif.PrimeValeur.Unite.Code;
                }
                t.Kdgprimpro = tarif.PrimeProvisionnelle;
                this.kpGarTarRepository.Update(t);
            }

            return mustUpdate;
        }

        internal bool HasTarifChanged(KpGarTar t, TarifGarantie tarif) {
            return t.Kdglcivalo != tarif.LCI.ValeurOrigine
                || t.Kdglcivalw != tarif.LCI.ValeurTravail
                || t.Kdglcivala != tarif.LCI.ValeurActualise
                || t.Kdglciunit != tarif.LCI.Unite.Code
                || t.Kdglcibase != tarif.LCI.Base.Code
                || ((tarif.LCI.ExpressionComplexe is null || tarif.LCI.ExpressionComplexe.Id < 1) && t.Kdgkdiid != 0)
                || (tarif.LCI.ExpressionComplexe?.Id ?? default) != t.Kdgkdiid
                || t.Kdgfrhvalo != tarif.Franchise.ValeurOrigine
                || t.Kdgfrhvalw != tarif.Franchise.ValeurTravail
                || t.Kdgfrhvala != tarif.Franchise.ValeurActualise
                || t.Kdgfrhunit != tarif.Franchise.Unite.Code
                || t.Kdgfrhbase != tarif.Franchise.Base.Code
                || ((tarif.Franchise.ExpressionComplexe is null || tarif.Franchise.ExpressionComplexe.Id < 1) && t.Kdgkdkid != 0)
                || (tarif.Franchise.ExpressionComplexe?.Id ?? default) != t.Kdgkdkid
                || t.Kdgprivalo != tarif.PrimeValeur.ValeurOrigine
                || t.Kdgprivalw != tarif.PrimeValeur.ValeurTravail
                || t.Kdgprivala != tarif.PrimeValeur.ValeurActualise;
        }

        internal bool HasAssietteChanged(KpGaran g, ValeursTarif assiette) {
            return g.Kdeasvalo != assiette.ValeurOrigine
                || g.Kdeasvalw != assiette.ValeurTravail
                || g.Kdeasvala != assiette.ValeurActualise
                || g.Kdeasunit != assiette.Unite.Code
                || g.Kdeasbase != assiette.Base.Code;
        }
        internal static Garantie MapGarantie(KpGaran kpGaran, IReferentialRepository referential) {
            return kpGaran is null ? null : new Garantie {
                Id = kpGaran.Kdeid,
                ParamGarantie = new Domain.Parametrage.Formules.ParamGarantieHierarchie {
                    Sequence = kpGaran.Kdeseq
                },
                CodeGarantie = kpGaran.Kdegaran,
                Formule = kpGaran.Kdefor,
                Caractere = kpGaran.Kdecar.AsEnum<CaractereSelection>(),
                Nature = kpGaran.Kdenat.AsEnum<NatureValue>(),
                NumeroAvenant = kpGaran.Kdeavn,
                TypeAlimentation = kpGaran.Kdeala.AsEnum<AlimentationValue>(),
                Tri = kpGaran.Kdetri,
                NumeroAvenantCreation = kpGaran.Kdecravn,
                NumeroAvenantModif = kpGaran.Kdemajavn,
                Taxe = referential?.GetValue<Taxe>(kpGaran.Kdetaxcod),
                DateDebut = Tools.MakeNullableDateTime(kpGaran.Kdedatdeb, kpGaran.Kdeheudeb, true),
                DateFinDeGarantie = Tools.MakeNullableDateTime(kpGaran.Kdedatfin, kpGaran.Kdeheufin, true),
                DefinitionGarantie = referential?.GetValue<DefinitionGarantie>(kpGaran.Kdedefg),
                Duree = string.IsNullOrWhiteSpace(kpGaran.Kdeduruni) ? default(int?) : kpGaran.Kdeduree,
                DureeUnite = referential?.GetValue<UniteDuree>(kpGaran.Kdeduruni),
                InventaireSpecifique = kpGaran.Kdeinvsp.AsNullableBool(),
                IsAlimMontantReference = kpGaran.Kdealiref.AsNullableBool(),
                IsApplicationCATNAT = kpGaran.Kdecatnat.AsNullableBool(),
                IsFlagModifie = kpGaran.Kdemodi.AsNullableBool(),
                IsGarantieAjoutee = kpGaran.Kdeajout.AsBool(),
                IsIndexe = kpGaran.Kdeina.AsNullableBool(),
                IsParameIndex = kpGaran.Kdepind.AsNullableBool(),
                NatureRetenue = kpGaran.Kdegan.AsEnum<NatureValue>(),
                NumDePresentation = kpGaran.Kdenumpres.AsDecimal(),
                ParamCodetaxe = referential?.GetValue<Taxe>(kpGaran.Kdeptaxc),
                ParametrageCATNAT = kpGaran.Kdepcatn.AsNullableBool(),
                ParametrageIsAlimMontantRef = kpGaran.Kdepref.AsBool(),
                ParamIsNatModifiable = kpGaran.Kdepntm.AsNullableBool(),
                ParamTypeAlimentation = kpGaran.Kdepala.AsEnum<AlimentationValue>(),
                RepartitionTaxe = kpGaran.Kdetaxrep.AsDecimal()
            };
        }

        private Garantie MapGarantie(KpGaran kpGaran) {
            return MapGarantie(kpGaran, this.referentielRepository);
        }

        private Garantie MapGarantieTarif(KpGaran kpGaran, KpGarTar kpGarTar) {
            var g = MapGarantie(kpGaran, this.referentielRepository);
            g.Tarif = BuildTarifCondition(kpGarTar);
            return g;
        }

        private bool UpdateAssiette(KpGaran kpgar, ValeursTarif assiette, int avn) {
            bool mustUpdate = HasAssietteChanged(kpgar, assiette);
            if (mustUpdate) {
                kpgar.Kdeasvalo = assiette.ValeurOrigine;
                kpgar.Kdeasvalw = assiette.ValeurTravail;
                kpgar.Kdeasvala = assiette.ValeurActualise;
                kpgar.Kdeasunit = assiette.Unite.Code;
                kpgar.Kdeasbase = assiette.Base.Code;
                kpgar.Kdemajavn = avn;
                this.garRepository.Update(kpgar);
            }
            return mustUpdate;
        }
    }
}
