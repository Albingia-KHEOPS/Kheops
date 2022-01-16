using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Regularisation;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;

namespace Albingia.Kheops.OP.DataAdapter {
    public class AffaireRepository : IAffaireRepository {
        private readonly YpoBaseRepository yRepo;
        private readonly YpoTracRepository ypoTracRepository;
        private readonly YprtEntRepository rtRepo;
        private readonly KpEntRepository kRepo;
        private readonly YAssNomRepository ynrepo;
        private readonly YAssureRepository yarepo;
        private readonly YcourtiRepository ycourtiRepo;
        private readonly HpentRepository hkRepo;
        private readonly YhpbaseRepository hyRepo;
        private readonly YhrtentRepository hrtRepo;
        private readonly KpOfEntRepository kpOfEntRepository;
        private readonly KpOfOptRepository kpOfOptRepository;
        private readonly KpOfRsqRepository kpOfRsqRepository;
        private readonly KpOfTarRepository kpOfTarRepository;
        private readonly YprtPerRepository yprtPerRepository;
        private readonly KVerrouRepository kVerrouRepository;
        private readonly KpRguRepository kpRguRepository;
        private readonly KCanevRepository kCanevRepository;
        private readonly IDesignationRepository desiRepository;
        private readonly IObservationRepository obsvRepository;
        private readonly ITraceRepository traceRepository;
        private readonly IReferentialRepository refRepo;

        public AffaireRepository(
            YpoBaseRepository yRepo,
            YprtEntRepository rtRepo,
            KpEntRepository kRepo,
            YhpbaseRepository hyRepo,
            YhrtentRepository hrtRepo,
            HpentRepository hkRepo,
            YAssureRepository yarepo,
            YAssNomRepository ynrepo,
            YcourtiRepository ycourtiRepo,
            KpOfEntRepository kpOfEntRepository,
            KpOfOptRepository kpOfOptRepository,
            KpOfRsqRepository kpOfRsqRepository,
            KpOfTarRepository kpOfTarRepository,
            YprtPerRepository yprtPerRepository,
            KVerrouRepository kVerrouRepository,
            KpRguRepository kpRguRepository,
            YpoTracRepository ypoTracRepository,
            KCanevRepository kCanevRepository,
            IDesignationRepository desiRepository,
            IObservationRepository obsvRepository,
            ITraceRepository traceRepository,
            IReferentialRepository refRepo) {
            this.yRepo = yRepo;
            this.rtRepo = rtRepo;
            this.kRepo = kRepo;
            this.ynrepo = ynrepo;
            this.yarepo = yarepo;
            this.refRepo = refRepo;
            this.ycourtiRepo = ycourtiRepo;
            this.hkRepo = hkRepo;
            this.hyRepo = hyRepo;
            this.hrtRepo = hrtRepo;
            this.kpOfEntRepository = kpOfEntRepository;
            this.kpOfOptRepository = kpOfOptRepository;
            this.kpOfRsqRepository = kpOfRsqRepository;
            this.kpOfTarRepository = kpOfTarRepository;
            this.yprtPerRepository = yprtPerRepository;
            this.desiRepository = desiRepository;
            this.obsvRepository = obsvRepository;
            this.kCanevRepository = kCanevRepository;
            this.kVerrouRepository = kVerrouRepository;
            this.kpRguRepository = kpRguRepository;
            this.ypoTracRepository = ypoTracRepository;
            this.traceRepository = traceRepository;
        }

        public Affaire GetLightByIpbAlx(string ipb, int alx) {
            var ylist = this.yRepo.SelectByIpb(ipb.ToIPB());
            var y = ylist.Count() == 1 ? ylist.Single() : ylist.FirstOrDefault(x => x.Pbalx == alx);
            return y is null ? null : new Affaire {
                CodeAffaire = y.Pbipb,
                NumeroAliment = y.Pbalx,
                NumeroAvenant = y.Pbavn,
                TypeAffaire = y.Pbtyp.ParseCode<AffaireType>(),
                Branche = this.refRepo.GetValue<Branche>(y.Pbbra),
                DateAccord = MakeNullableDateTime(y.Pbsaa, y.Pbsam, y.Pbsaj),
                DateEffet = MakeNullableDateTime(y.Pbefa, y.Pbefm, y.Pbefj, y.Pbefh, 0, 0),
                DateEffetAvenant = MakeNullableDateTime(y.Pbava, y.Pbavm, y.Pbavj),
                DateFin = MakeNullableDateTime(y.Pbfea, y.Pbfem, y.Pbfej, y.Pbfeh, 0, 0),
                DateCreation = MakeDateTime(y.Pbdea, y.Pbdem, y.Pbdej),
                DateModification = MakeDateTime(y.Pbmja, y.Pbmjm, y.Pbmjj),
                EtatAffaire = this.refRepo.GetValue<Etat>(y.Pbeta),
                SituationAffaire = this.refRepo.GetValue<Situation>(y.Pbsit),
                TypeTraitement = this.refRepo.GetValue<TypeTraitement>(y.Pbttr)
            };
        }

        public Affaire GetById(AffaireId id) {
            return GetById(id.TypeAffaire, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant, id.IsHisto);
        }

        public AffaireId GetCurrentId(string codeAffaire, int version) {
            var ylist = this.yRepo.SelectByIpb(codeAffaire.ToIPB());
            var y = ylist.Count() == 1 ? ylist.Single() : ylist.FirstOrDefault(x => x.Pbalx == version);
            if (y is null) {
                return null;
            }
            return new AffaireId {
                IsHisto = false,
                CodeAffaire = y.Pbipb,
                NumeroAliment = y.Pbalx,
                NumeroAvenant = y.Pbavn,
                TypeAffaire = y.Pbtyp.ParseCode<AffaireType>(),
                TypeTraitement = y.Pbttr
            };
        }

        public AffaireId ValidateId(AffaireId affaireId) {
            if (affaireId.NumeroAvenant >= 0) {
                var currentId = GetCurrentId(affaireId.CodeAffaire, affaireId.NumeroAliment);
                if (currentId.NumeroAvenant > affaireId.NumeroAvenant) {
                    affaireId.IsHisto = true;
                }
                else {
                    affaireId.IsHisto = false;
                }
            }
            else if (affaireId.IsHisto) {
                throw new ArgumentException($"Missing {nameof(AffaireId.NumeroAvenant)}", nameof(affaireId.IsHisto));
            }
            return affaireId;
        }

        public AffaireId GetPriorId(string codeAffaire, int version, int avenant) {
            var currentId = GetCurrentId(codeAffaire, version);
            if (currentId is null) {
                return null;
            }
            var y = this.hyRepo.Get(currentId.TypeAffaire.AsCode(), currentId.CodeAffaire, currentId.NumeroAliment, avenant);
            if (y is null) {
                return null;
            }
            return new AffaireId {
                IsHisto = true,
                CodeAffaire = y.Pbipb,
                NumeroAliment = y.Pbalx,
                NumeroAvenant = y.Pbavn,
                TypeAffaire = y.Pbtyp.ParseCode<AffaireType>(),
                TypeTraitement = y.Pbttr
            };
        }

        public Affaire GetAffaireCanevas(int templateId) {
            var canevas = this.kCanevRepository.Get(templateId);
            var ypo = this.yRepo.Get(canevas.Kgotyp, canevas.Kgocnva.ToIPB(), 0);
            return ypo is null ? null : GetById(canevas.Kgotyp.ParseCode<AffaireType>(), canevas.Kgocnva.ToIPB(), 0);
        }

        public Affaire GetById(AffaireType type, string codeAffaire, int aliment, int? avenant = null, bool isHisto = false) {
            if (isHisto) {
                avenant = avenant.GetValueOrDefault();
            }
            var affaireId = new AffaireId { CodeAffaire = codeAffaire, NumeroAliment = aliment, NumeroAvenant = avenant, IsHisto = isHisto, TypeAffaire = type };
            string ipb = codeAffaire?.PadLeft(9, ' ') ?? string.Empty;
            if (codeAffaire.IsEmptyOrNull()) {
                return null;
            }
            YpoBase y, offreOrigine = null;
            YprtEnt rt;
            KpEnt k;
            KpRgu rg;
            TraceContext lastTrace;
            if (!isHisto) {
                // ~ 10% of Contrats have versioning
                // ~ 15% of Offres have versioning
                var poList = this.yRepo.SelectByIpb(ipb);
                y = poList.Count() == 1 ? poList.Single() : poList.FirstOrDefault(x => x.Pbalx == aliment);
                if (y is null) {
                    return null;
                }
                var kentList = this.kRepo.SelectByIpb(ipb);
                var yentList = this.rtRepo.SelectByIpb(ipb);


                if (!string.IsNullOrWhiteSpace(y.Pboff)) {
                    offreOrigine = this.yRepo.Get(AffaireType.Offre.AsCode(), y.Pboff, y.Pbofv);
                }
                rt = yentList.Count() == 1 ? yentList.Single() : yentList.FirstOrDefault(x => x.Jdalx == aliment);
                k = kentList.Count() == 1 ? kentList.Single() : kentList.FirstOrDefault(x => x.Kaaalx == aliment);

                int avn = y.Pbavn;
                do {
                    // try multiple if new avenant is creating
                    lastTrace = this.traceRepository.GetLastTrace(new AffaireId { CodeAffaire = ipb, NumeroAliment = aliment, NumeroAvenant = avn });
                    avn--;
                }
                while (lastTrace is null && avn >= 0);
            }
            else {
                y = this.hyRepo.Get(type.AsCode(), ipb, aliment, avenant.GetValueOrDefault());
                rt = this.hrtRepo.Get(ipb, aliment, avenant.GetValueOrDefault());
                k = this.hkRepo.Get(type.AsCode(), ipb, aliment, avenant.GetValueOrDefault());
                lastTrace = this.traceRepository.GetLastTrace(new AffaireId { CodeAffaire = ipb, NumeroAliment = aliment, NumeroAvenant = y.Pbavn });
            }

            var rgList = this.kpRguRepository.GetByAffaire(type.AsCode(), codeAffaire, aliment, avenant ?? 0).OrderByDescending(x => x.Khwid);
            rg = rgList.Count() > 0 ? rgList.FirstOrDefault() : null;

            if (y is null) {
                return null;
            }
            if (rt is null && y.Pbork != "KHE" && y.Pbtyp == AffaireType.Offre.AsCode()) {
                rt = new YprtEnt {
                    Jdenc = string.Empty,
                    Jdind = string.Empty,
                    Jdafc = string.Empty
                };
            }

            var a = new AffaireProxy(this) {
                Branche = refRepo.GetValue<Branche>(y.Pbbra),
                Categorie = y.Pbcat,
                NatureTravaux = this.refRepo.GetValue<NatureTravauxAffaire>(y.Pbnat),
                NatureContrat = refRepo.GetValue<NatureAffaire>(y.Pbnpl),
                Cible = k is null ? new Cible { Code = string.Empty } : this.refRepo.GetCible(k.Kaacible),
                CibleCategorie = k is null ? null : this.refRepo.GetCibleCatego(k.Kaacible, y.Pbbra),
                IsKheops = y.Pbork?.ToUpper() == "KHE" || y.Pbipb.Contains("CV"),
                Souscripteur = this.refRepo.GetUtilisateur(y.Pbsou),
                Gestionnaire = this.refRepo.GetUtilisateur(y.Pbges),
                SousBranche = y.Pbsbr,
                CodeAffaire = y.Pbipb,
                CodeOffre = y.Pboff,
                NumeroAliment = y.Pbalx,
                DateAccord = MakeNullableDateTime(y.Pbsaa, y.Pbsam, y.Pbsaj),
                DateEffet = MakeNullableDateTime(y.Pbefa, y.Pbefm, y.Pbefj, y.Pbefh, 0, 0),
                DateEffetAvenant = MakeNullableDateTime(y.Pbava, y.Pbavm, y.Pbavj),
                DateFin = MakeNullableDateTime(y.Pbfea, y.Pbfem, y.Pbfej, y.Pbfeh, 0, 0),
                Duree = y.Pbdur,
                UniteeDuree = this.refRepo.GetValue<UniteDuree>(y.Pbctu),
                DateCreation = MakeDateTime(y.Pbdea, y.Pbdem, y.Pbdej),
                DateModification = MakeDateTime(y.Pbmja, y.Pbmjm, y.Pbmjj),
                CodeUserCreate = y.Pbcru,
                CodeUserUpdate = y.Pbmju,
                DateSituation = MakeNullableDateTime(y.Pbsta, y.Pbstm, y.Pbstj),
                Periode = new Periode {
                    Debut = MakeNullableDateTime(rt.Jddpa, rt.Jddpm, rt.Jddpj),
                    Fin = MakeNullableDateTime(rt.Jdfpa, rt.Jdfpm, rt.Jdfpj)
                },
                ProchaineEcheance = new Echeance { Date = MakeNullableDateTime(rt.Jdpea, rt.Jdpem, rt.Jdpej) },
                Echeance = MakeNullableDateTime(2020, y.Pbecm, y.Pbecj),
                TypeAffaire = y.Pbtyp.ParseCode<AffaireType>(),
                TypePolice = this.refRepo.GetValue<TypePolice>(y.Pbmer.IsEmptyOrNull() ? "S" : y.Pbmer),
                Etat = y.Pbeta.ParseCode<EtatAffaire>(),
                Situation = y.Pbsit.ParseCode<SituationAffaire>(),
                EtatAffaire = this.refRepo.GetValue<Etat>(y.Pbeta),
                SituationAffaire = this.refRepo.GetValue<Situation>(y.Pbsit),
                CodeMotifSituation = y.Pbstf,
                MotifSituation = this.refRepo.GetValue<MotifSituation>(y.Pbstf),
                TypeTraitement = this.refRepo.GetValue<TypeTraitement>(y.Pbttr),
                DernierTraitement = lastTrace is null ? null : new TypeTraitement {
                    Code = lastTrace.TypeTraitement,
                    LibelleLong = lastTrace.Libelle,
                    Libelle = lastTrace.Libelle
                },
                RegimeTaxe = this.refRepo.GetValue<RegimeTaxe>(y.Pbrgt),
                CodeSTOP = this.refRepo.GetValue<CodeSTOP>(y.Pbstp),
                Preneur = GetAssureProxy(y.Pbias),
                IdAdresse = y.Pbadh,
                ValeurIndiceOrigine = rt.Jdivo,
                ValeurIndiceActualisee = rt.Jdiva,
                BaseCATNATCalculee = k?.Kaaatgbcn ?? default,
                SoumisCatNat = rt.Jdcna.AsBool(),
                ReferenceCourtier = y.Pboct,
                Metadata = new AffaireMetadata {
                    MotClef1 = y.Pbmo1,
                    MotClef2 = y.Pbmo2,
                    MotClef3 = y.Pbmo3
                },
                Encaissement = this.refRepo.GetValue<Encaissement>(rt.Jdenc),
                Descriptif = y.Pbref,
                DateSaisie = MakeNullableDateTime(y.Pbsaa, y.Pbsam, y.Pbsaj, y.Pbsah, 0, 0),
                OffreOrigine = offreOrigine is null ? null : new AffaireId { TypeAffaire = AffaireType.Offre, CodeAffaire = offreOrigine.Pbipb, NumeroAliment = offreOrigine.Pbalx },
                DateSaisieOffre = offreOrigine is null ? null : MakeNullableDateTime(offreOrigine.Pbsaa, offreOrigine.Pbsam, offreOrigine.Pbsaj, offreOrigine.Pbsah, 0, 0),
                Devise = refRepo.GetValue<Devise>(y.Pbdev),
                IndiceReference = refRepo.GetValue<Indice>(rt.Jdind),
                IntercalaireExiste = rt.Jditc.AsBool(),
                CourtierApporteur = GetCourtierProxy(y.Pbcta, 0),
                CourtierGestionnaire = GetCourtierProxy(y.Pbict, 0),
                CourtierPayeur = GetCourtierProxy(y.Pbctp, 0),
                NumeroAvenant = y.Pbavn,
                IsHisto = isHisto,
                NumChronoOsbv = k?.Kaaobsv ?? 0L,
                PreavisMois = rt.Jddpv,
                Periodicite = this.refRepo.GetValue<Periodicite>(y.Pbper),
                TauxCommission = rt.Jdxcm,
                TauxCommissionCATNAT = rt.Jdcnc,
                TypeAccord = this.refRepo.GetValue<TypeAccord>(y.Pbtac),
                DesignationAvenant = k is null ? string.Empty : this.desiRepository.GetDesignation(k.Kaaavds),
                Observations = (k?.Kaaobsv ?? 0) < 1 ? string.Empty : this.obsvRepository.GetObservation((int)k.Kaaobsv, isHisto ? avenant : null)?.Texte,
                FraisAccessoires = rt.Jdafr,
                ApplicationFraisAccessoire = this.refRepo.GetValue<ApplicationFraisAccessoire>(rt.Jdafc),
                PartAlbingia = y.Pbapp,
                MontantReference1 = rt.Jdtff,
                MontantReference2 = rt.Jdtmc,
                MotifAvenant = y.Pbavn > 0 ? this.refRepo.GetValue<MotifAvenant>(y.Pbavc) : null,
                MotifResiliation = y.Pbsit == "X" ? this.refRepo.GetValue<MotifResiliation>(y.Pbrsc) : null,
                Interlocuteur = new Intervenant { Interlocuteur = new Interlocuteur { Code = y.Pbin5 } },
                TarifAffaireLCI = k is null ? null : new TarifAffaire {
                    Base = this.refRepo.GetBase(Domain.Parametrage.Formules.TypeDeValeur.LCI, k.Kaalcibase),
                    IdExpCpx = k.Kaakdiid,
                    Indexe = k.Kaalciina.AsBoolean() ?? false,
                    Unite = this.refRepo.GetUnite(Domain.Parametrage.Formules.TypeDeValeur.LCI, k.Kaalciunit),
                    ValeurActualisee = k.Kaalcivala,
                    ValeurOrigine = k.Kaalcivalo
                },
                TarifAffaireFRH = k is null ? null : new TarifAffaire {
                    Base = this.refRepo.GetBase(Domain.Parametrage.Formules.TypeDeValeur.Franchise, k.Kaafrhbase),
                    IdExpCpx = k.Kaakdkid,
                    Indexe = k.Kaafrhina.AsBoolean() ?? false,
                    Unite = this.refRepo.GetUnite(Domain.Parametrage.Formules.TypeDeValeur.LCI, k.Kaafrhunit),
                    ValeurActualisee = k.Kaafrhvala,
                    ValeurOrigine = k.Kaafrhvalo
                },
                TypeMontantAssiette = rt.Jdsht.ParseCode<TypeMontant>(),
                GareatTheorique = new Gareat {
                    IsApplique = rt.Jdapt.AsBool(),
                    MontantCritere = rt.Jdacv,
                    NatureCritere = rt.Jdacr,
                    Tranche = rt.Jdaat,
                    TauxGareat = new TauxGareat {
                        Cession = rt.Jdaxc,
                        Valeur = rt.Jdaxt,
                        FraisGestion = rt.Jdaxg,
                        Commission = rt.Jdaxm,
                        Taxe = rt.Jdatx
                    }
                },
                GareatRetenu = new Gareat {
                    IsApplique = rt.Jdapr.AsBool(),
                    MontantCritere = rt.Jdacv,
                    NatureCritere = rt.Jdacr,
                    Tranche = rt.Jdaar,
                    TauxGareat = new TauxGareat {
                        Cession = rt.Jdaxc,
                        Valeur = rt.Jdaxf,
                        FraisGestion = rt.Jdaxg,
                        Commission = rt.Jdaxm,
                        Taxe = rt.Jdatx
                    }
                },
                IsAttenteDocumentsCourtier = k?.Kaaattdoc.AsBoolean()
            };

            if ((a.TypeTraitement?.Code).IsEmptyOrNull() || a.TypeTraitement.LibelleLong.IsEmptyOrNull()) {
                var traceTTR = this.refRepo.GetValue<TraceTraitement>(y.Pbttr);
                if (traceTTR != null) {
                    a.TypeTraitement = new TypeTraitement();
                    traceTTR.CopyTo(a.TypeTraitement);
                }
            }
            else if (lastTrace != null && (a.TypeTraitement.Code == AlbConstantesMetiers.TYPE_AVENANT_REGUL || (rg != null && rg.Khwmrg == AlbConstantesMetiers.TYPE_AVENANT_BNS))) {
                a.TypeTraitement = new TypeTraitement {
                    Code = lastTrace.TypeTraitement,
                    LibelleLong = lastTrace.Libelle,
                    Libelle = lastTrace.Libelle
                };
            }

            return a;
        }

        public Affaire GetByIdWithoutDependencies(string code, int aliment = 0, int? avenant = null) {
            string ipb = code?.ToIPB() ?? string.Empty;
            if (ipb.IsEmptyOrNull()) {
                throw new ArgumentException($"In {nameof(GetByIdWithoutDependencies)}: Invalid IPB", nameof(code));
            }
            int alx = aliment < 0 ? 0 : aliment;

            // use avn only for histo
            var currentId = GetCurrentId(ipb, alx);
            int? avn = avenant.HasValue ? avenant < 0 || currentId.NumeroAvenant == avenant ? null : avenant : null;

            YpoBase offreOrigine = null;
            YprtEnt yent;
            KpEnt kent;
            TraceContext lastTrace;

            var poList = this.yRepo.SelectByIpb(ipb);
            YpoBase pobase = poList.Count() == 1 ? poList.Single() : poList.FirstOrDefault(x => x.Pbalx == alx);
            if (pobase is null) {
                return null;
            }

            if (avn.HasValue) {
                // histo case
                pobase = this.hyRepo.Get(pobase.Pbtyp, ipb, alx, avn.Value);
                yent = this.hrtRepo.Get(ipb, alx, avn.Value);
                kent = this.hkRepo.Get(pobase.Pbtyp, ipb, alx, avn.Value);
                lastTrace = this.traceRepository.GetLastTrace(new AffaireId { CodeAffaire = ipb, NumeroAliment = alx, NumeroAvenant = avn });
            }
            else {
                var kentList = this.kRepo.SelectByIpb(ipb);
                var yentList = this.rtRepo.SelectByIpb(ipb);

                if (!string.IsNullOrWhiteSpace(pobase.Pboff)) {
                    offreOrigine = this.yRepo.Get(AffaireType.Offre.AsCode(), pobase.Pboff, pobase.Pbofv);
                }
                yent = yentList.Count() == 1 ? yentList.Single() : yentList.FirstOrDefault(x => x.Jdalx == alx);
                kent = kentList.Count() == 1 ? kentList.Single() : kentList.FirstOrDefault(x => x.Kaaalx == alx);
                int a = pobase.Pbavn;
                do {
                    // try multiple if new avenant is creating
                    lastTrace = this.traceRepository.GetLastTrace(new AffaireId { CodeAffaire = ipb, NumeroAliment = alx, NumeroAvenant = a-- });
                }
                while (lastTrace is null && a >= 0);
            }

            var affaire = new AffaireProxy(this) {
                Branche = this.refRepo.GetValue<Branche>(pobase.Pbbra),
                Categorie = pobase.Pbcat,
                NatureTravaux = this.refRepo.GetValue<NatureTravauxAffaire>(pobase.Pbnat),
                NatureContrat = this.refRepo.GetValue<NatureAffaire>(pobase.Pbnpl),
                Cible = this.refRepo.GetCible(kent?.Kaacible),
                CibleCategorie = this.refRepo.GetCibleCatego(kent?.Kaacible, pobase.Pbbra),
                Souscripteur = this.refRepo.GetUtilisateur(pobase.Pbsou),
                Gestionnaire = this.refRepo.GetUtilisateur(pobase.Pbges),
                SousBranche = pobase.Pbsbr,
                CodeAffaire = pobase.Pbipb,
                CodeOffre = pobase.Pboff,
                NumeroAliment = pobase.Pbalx,
                DateAccord = MakeNullableDateTime(pobase.Pbsaa, pobase.Pbsam, pobase.Pbsaj),
                DateEffet = MakeNullableDateTime(pobase.Pbefa, pobase.Pbefm, pobase.Pbefj, pobase.Pbefh, 0, 0),
                DateEffetAvenant = MakeNullableDateTime(pobase.Pbava, pobase.Pbavm, pobase.Pbavj),
                DateFin = MakeNullableDateTime(pobase.Pbfea, pobase.Pbfem, pobase.Pbfej, pobase.Pbfeh, 0, 0),
                DateCreation = MakeDateTime(pobase.Pbdea, pobase.Pbdem, pobase.Pbdej),
                DateModification = MakeDateTime(pobase.Pbmja, pobase.Pbmjm, pobase.Pbmjj),
                CodeUserCreate = pobase.Pbcru,
                CodeUserUpdate = pobase.Pbmju,
                DateSituation = MakeNullableDateTime(pobase.Pbsta, pobase.Pbstm, pobase.Pbstj),
                Periode = new Periode {
                    Debut = MakeNullableDateTime(yent.Jddpa, yent.Jddpm, yent.Jddpj),
                    Fin = MakeNullableDateTime(yent.Jdfpa, yent.Jdfpm, yent.Jdfpj)
                },
                ProchaineEcheance = new Echeance { Date = MakeNullableDateTime(yent.Jdpea, yent.Jdpem, yent.Jdpej) },
                TypeAffaire = pobase.Pbtyp.ParseCode<AffaireType>(),
                TypePolice = this.refRepo.GetValue<TypePolice>(pobase.Pbmer.IsEmptyOrNull() ? "S" : pobase.Pbmer),
                Etat = pobase.Pbeta.ParseCode<EtatAffaire>(),
                Situation = pobase.Pbsit.ParseCode<SituationAffaire>(),
                EtatAffaire = this.refRepo.GetValue<Etat>(pobase.Pbeta),
                SituationAffaire = this.refRepo.GetValue<Situation>(pobase.Pbsit),
                CodeMotifSituation = pobase.Pbstf,
                MotifSituation = this.refRepo.GetValue<MotifSituation>(pobase.Pbstf),
                TypeTraitement = this.refRepo.GetValue<TypeTraitement>(pobase.Pbttr),
                RegimeTaxe = this.refRepo.GetValue<RegimeTaxe>(pobase.Pbrgt),
                CodeSTOP = this.refRepo.GetValue<CodeSTOP>(pobase.Pbstp),
                Preneur = GetAssureProxy(pobase.Pbias),
                IdAdresse = pobase.Pbadh,
                ValeurIndiceOrigine = yent.Jdivo,
                ValeurIndiceActualisee = yent.Jdiva,
                SoumisCatNat = yent.Jdcna.AsBool(),
                ReferenceCourtier = pobase.Pboct,
                Metadata = new AffaireMetadata {
                    MotClef1 = pobase.Pbmo1,
                    MotClef2 = pobase.Pbmo2,
                    MotClef3 = pobase.Pbmo3
                },
                Encaissement = this.refRepo.GetValue<Encaissement>(yent.Jdenc),
                Descriptif = pobase.Pbref,
                DateSaisie = MakeNullableDateTime(pobase.Pbsaa, pobase.Pbsam, pobase.Pbsaj, pobase.Pbsah, 0, 0),
                OffreOrigine = offreOrigine is null ? null : new AffaireId { TypeAffaire = AffaireType.Offre, CodeAffaire = offreOrigine.Pbipb, NumeroAliment = offreOrigine.Pbalx },
                DateSaisieOffre = offreOrigine is null ? null : MakeNullableDateTime(offreOrigine.Pbsaa, offreOrigine.Pbsam, offreOrigine.Pbsaj, offreOrigine.Pbsah, 0, 0),
                Devise = this.refRepo.GetValue<Devise>(pobase.Pbdev),
                IndiceReference = this.refRepo.GetValue<Indice>(yent.Jdind),
                IntercalaireExiste = yent.Jditc.AsBool(),
                CourtierApporteur = new Domain.Affaire.Courtier { Code = pobase.Pbcta },
                CourtierGestionnaire = new Domain.Affaire.Courtier { Code = pobase.Pbict },
                CourtierPayeur = new Domain.Affaire.Courtier { Code = pobase.Pbctp },
                NumeroAvenant = pobase.Pbavn,
                IsHisto = avn.HasValue,
                NumChronoOsbv = kent?.Kaaobsv ?? 0,
                PreavisMois = yent.Jddpv,
                Periodicite = this.refRepo.GetValue<Periodicite>(pobase.Pbper),
                TauxCommission = yent.Jdxcm,
                TauxCommissionCATNAT = yent.Jdcnc,
                TypeAccord = this.refRepo.GetValue<TypeAccord>(pobase.Pbtac),
                DesignationAvenant = kent is null ? string.Empty : this.desiRepository.GetDesignation(kent.Kaaavds),
                Observations = (kent?.Kaaobsv ?? 0) < 1 ? string.Empty : this.obsvRepository.GetObservation((int)kent.Kaaobsv, avn)?.Texte,
                FraisAccessoires = yent.Jdafr,
                ApplicationFraisAccessoire = this.refRepo.GetValue<ApplicationFraisAccessoire>(yent.Jdafc),
                PartAlbingia = pobase.Pbapp,
                MontantReference1 = yent.Jdtff,
                MontantReference2 = yent.Jdtmc,
                MotifAvenant = pobase.Pbavn > 0 ? this.refRepo.GetValue<MotifAvenant>(pobase.Pbavc) : null,
                MotifResiliation = pobase.Pbsit == "X" ? this.refRepo.GetValue<MotifResiliation>(pobase.Pbrsc) : null,
                IsKheops = pobase.Pbork?.ToUpper() == "KHE",
                Interlocuteur = new Intervenant { Interlocuteur = new Interlocuteur { Code = pobase.Pbin5 } },
                TypeRemiseEnVigueur = kent?.Kaaartyg ?? string.Empty
            };

            if ((affaire.TypeTraitement?.Code).IsEmptyOrNull() || affaire.TypeTraitement.LibelleLong.IsEmptyOrNull()) {
                var traceTTR = this.refRepo.GetValue<TraceTraitement>(pobase.Pbttr);
                if (traceTTR != null) {
                    affaire.TypeTraitement = new TypeTraitement();
                    traceTTR.CopyTo(affaire.TypeTraitement);
                }
            }
            else if (lastTrace != null && affaire.TypeTraitement.Code == AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                affaire.TypeTraitement = new TypeTraitement {
                    Code = lastTrace.TypeTraitement,
                    LibelleLong = lastTrace.Libelle,
                    Libelle = lastTrace.Libelle
                };
            }

            return affaire;
        }

        public Assure GetAssureProxy(int code) {
            return new AssureProxy(code, 0, this.yarepo, this.ynrepo, this.refRepo);
        }

        /// <summary>
        /// Retrieves YHPBASE data only
        /// </summary>
        /// <param name="codeAffaire">IPB code</param>
        /// <param name="aliment">ALX code</param>
        /// <returns></returns>
        public IEnumerable<Affaire> GetFullHisto(string codeAffaire, int aliment) {
            var yhpList = this.hyRepo.SelectByIpbAlx(codeAffaire, aliment);
            return yhpList.Select(y => {
                var a = BuildFromYpoBase(y, this.refRepo);
                a.IsHisto = true;
                a.Preneur = GetAssureProxy(y.Pbias);
                return a;
            });
        }

        public void Reprise(AffaireId id) {
            var po = this.yRepo.Get(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment);
            this.yRepo.Delete(po);
            var hpo = this.hyRepo.Get(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            this.hyRepo.Delete(hpo);
            this.yRepo.Insert(hpo);

            var ent = this.kRepo.Get(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment);
            this.kRepo.Delete(ent);
            var hent = this.hkRepo.Get(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            this.hkRepo.Delete(hent);
            this.kRepo.Insert(hent);

            var yent = this.rtRepo.Get(id.CodeAffaire, id.NumeroAliment);
            this.rtRepo.Delete(yent);
            var hyent = this.hrtRepo.Get(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            this.hrtRepo.Delete(hyent);
            this.rtRepo.Insert(hyent);

            this.ypoTracRepository.DeleteByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value);
        }

        public IEnumerable<Affaire> GetListByIds(IEnumerable<AffaireId> idList) {
            return this.yRepo.SelectMany(idList.Select(x => (x.CodeAffaire, x.NumeroAliment))).Select(x => BuildFromYpoBase(x, this.refRepo));
        }

        private Domain.Affaire.Courtier GetCourtierProxy(int id, int code) {
            return new CourtierProxy(id, code, this.ycourtiRepo, this.refRepo);
        }

        public Affaire SaveAffaire(Affaire aff, User user) {
            YpoBase y = this.yRepo.Get(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment);
            YprtEnt r;
            KpEnt k;
            DateTime now = DateTime.Now;
            int
                year = now.Year,
                month = now.Month,
                day = now.Day;

            if (y != null) {
                r = this.rtRepo.Get(aff.CodeAffaire, aff.NumeroAliment);
                k = this.kRepo.Get(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment);
            }
            else {
                y = new YpoBase {
                    Pbipb = aff.CodeAffaire.AsIPB(),
                    Pbalx = aff.NumeroAliment,
                    Pbtyp = aff.TypeAffaire.AsCode(),
                    Pbdeu = "SPRINKS",
                    Pbttr = "OFFRE",
                    Pbcra = year,
                    Pbcrm = month,
                    Pbcrj = day,
                    Pbdea = year,
                    Pbdem = month,
                    Pbdej = day,
                    Pbper = aff.Periodicite?.Code
                };

                r = new YprtEnt {
                    Jdipb = aff.CodeAffaire.AsIPB(),
                    Jdalx = aff.NumeroAliment,
                    Jddrq = 1,
                    Jdnbr = 1,
                    Jdsht = "H",
                    Jdafc = "S"
                };
                k = new KpEnt {
                    Kaaipb = aff.CodeAffaire.AsIPB(),
                    Kaaalx = aff.NumeroAliment,
                    Kaatyp = aff.TypeAffaire.AsCode()
                };
            }
            UpdateY(aff, user, y, year, month, day);
            UpdateYRT(aff, r);
            UpdateKPEnt(aff, k);

            return this.GetById(aff.TypeAffaire, aff.CodeAffaire, aff.NumeroAliment);

        }

        public void SaveConditions(Affaire affaire, string user) {
            YpoBase y = this.yRepo.Get(affaire.TypeAffaire.AsCode(), affaire.CodeAffaire, affaire.NumeroAliment);
            YprtEnt r;
            KpEnt k;
            DateTime now = DateTime.Now;
            int
                year = now.Year,
                month = now.Month,
                day = now.Day;

            if (y != null) {
                r = this.rtRepo.Get(affaire.CodeAffaire, affaire.NumeroAliment);
                k = this.kRepo.Get(affaire.TypeAffaire.AsCode(), affaire.CodeAffaire, affaire.NumeroAliment);
                var id = GetCurrentId(affaire.CodeAffaire, affaire.NumeroAliment);
                if (k.Kaaavn < id.NumeroAvenant.GetValueOrDefault()) {
                    throw new BusinessValidationException(new ValidationError(id.TypeAffaire.AsCode(), id.AsAffaireKey(), string.Empty, "Mise à jour impossible, l'avenant est obsolète"));
                }

                k.Kaalcibase = affaire.TarifAffaireLCI?.Base?.Code ?? string.Empty;
                k.Kaalciina = (affaire.TarifAffaireLCI?.Indexe ?? false).ToYesNo();
                k.Kaalciunit = affaire.TarifAffaireLCI?.Unite;
                k.Kaalcivala = affaire.TarifAffaireLCI?.ValeurActualisee ?? default;
                k.Kaalcivalo = affaire.TarifAffaireLCI?.ValeurOrigine ?? default;
                k.Kaakdiid = affaire.TarifAffaireLCI?.IdExpCpx ?? default;

                k.Kaafrhbase = affaire.TarifAffaireFRH?.Base?.Code ?? string.Empty;
                k.Kaafrhina = (affaire.TarifAffaireFRH?.Indexe ?? false).ToYesNo();
                k.Kaafrhunit = affaire.TarifAffaireFRH?.Unite;
                k.Kaafrhvala = affaire.TarifAffaireFRH?.ValeurActualisee ?? default;
                k.Kaafrhvalo = affaire.TarifAffaireFRH?.ValeurOrigine ?? default;
                k.Kaakdkid = affaire.TarifAffaireFRH?.IdExpCpx ?? default;
                this.kRepo.Update(k);

                r.Jdsht = affaire.TypeMontantAssiette.AsCode();
                this.rtRepo.Update(r);
            }
        }

        public IEnumerable<NouvelleAffaire> GetTempAffairs(AffaireId id) {
            var entetes = this.kpOfEntRepository.GetByOffre(id.CodeAffaire, id.NumeroAliment);
            var formulesOptions = this.kpOfOptRepository.GetByOffre(id.CodeAffaire, id.NumeroAliment);
            var risquesObjets = this.kpOfRsqRepository.GetByOffre(id.CodeAffaire, id.NumeroAliment);
            var tarifsGaranties = this.kpOfTarRepository.GetByOffre(id.CodeAffaire, id.NumeroAliment);
            return entetes.Select(e => new NouvelleAffaire {
                Offre = id,
                CodeContrat = e.Kfhpog,
                VersionContrat = e.Kfhalg,
                Risques = risquesObjets.Where(ro => ro.Kfipog == e.Kfhpog && ro.Kfialg == e.Kfhalg).GroupBy(ro => ro.Kfirsq).Select(gro => new NouvelleAffaireRisque {
                    IsSelected = gro.First(x => x.Kfiobj == 0).Kfisel == Booleen.Oui.AsCode(),
                    Numero = gro.Key,
                    Objets = gro.Where(x => x.Kfiobj > 0).Select(x => new NouvelleAffaireObjet {
                        IsSelected = x.Kfisel == Booleen.Oui.AsCode(),
                        Numero = x.Kfiobj
                    }).ToList()
                }).ToList(),
                Formules = formulesOptions.Where(fo => fo.Kfjpog == e.Kfhpog && fo.Kfjalg == e.Kfhalg).GroupBy(fo => fo.Kfjfor).Select(gfor => new NouvelleAffaireFormule {
                    AffaireId = id,
                    IsSelected = gfor.First(o => o.Kfjopt == 0).Kfjsel == Booleen.Oui.AsCode(),
                    Numero = gfor.Key,
                    SelectedOptionNumber = gfor.FirstOrDefault(o => o.Kfjopt > 0 && o.Kfjsel == Booleen.Oui.AsCode())?.Kfjopt
                }).ToList()
            }).ToList();
        }
        public string GetAffaireTraitement(AffaireId id)
        {
            return this.yRepo.Get(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment).Pbttr;
        }

        public void SaveSelectionRisquesNouvelleAffaire(NouvelleAffaire nouvelleAffaire, int? numRisque = null) {
            var listOfKpOfRsq = this.kpOfRsqRepository.GetByContrat(nouvelleAffaire.CodeContrat, nouvelleAffaire.VersionContrat)
                .Where(r => numRisque.GetValueOrDefault() < 1 || r.Kfirsq == numRisque)
                .ToList();
            listOfKpOfRsq.ForEach(r => {
                if (r.Kfiobj == 0) {
                    var risque = nouvelleAffaire.Risques.First(x => x.Numero == r.Kfirsq);
                    r.Kfisel = risque.IsSelected ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
                }
                else {
                    var objet = nouvelleAffaire.Risques.First(x => x.Numero == r.Kfirsq).Objets.First(x => x.Numero == r.Kfiobj);
                    r.Kfisel = objet.IsSelected ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
                }
                this.kpOfRsqRepository.Update(r);
            });
        }

        public void SaveSelectionFormulesNouvelleAffaire(NouvelleAffaire nouvelleAffaire, int? numFormule = null) {
            var optionList = this.kpOfOptRepository.GetByContrat(nouvelleAffaire.CodeContrat, nouvelleAffaire.VersionContrat).Where(x => x.Kfjteng.IsIn("O", "F"));
            if (numFormule.GetValueOrDefault() > default(int)) {
                optionList = optionList.Where(x => x.Kfjfor == numFormule);
            }
            var formules = nouvelleAffaire.Formules.ToDictionary(x => x.Numero);
            int[] numbers = formules.Select(x => x.Key).ToArray();
            optionList.Where(x => x.Kfjfor.IsIn(numbers)).ToList().ForEach(x => {
                if (x.Kfjopt == 0) {
                    x.Kfjsel = formules[x.Kfjfor].IsSelected ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
                }
                else {
                    x.Kfjsel = formules[x.Kfjfor].SelectedOptionNumber == x.Kfjopt ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
                }
                this.kpOfOptRepository.Update(x);
            });
        }

        public void SaveNewAffair(Affaire affair, string user, string ipb, int alx = 0) {
            this.kRepo.SaveNewAffair(affair.CodeAffaire, affair.NumeroAliment, user, ipb, alx);
        }

        private static void UpdateY(Affaire aff, User user, YpoBase y, int year, int month, int day) {
            y.Pbork = aff.CodeAffaire.StartsWith("CV") ? "KVS" : "KHE";
            y.Pbsbr = aff.SousBranche;
            y.Pbcat = aff.Categorie;
            y.Pbref = aff.Descriptif;
            y.Pbges = aff.Gestionnaire.Code;
            y.Pbsou = aff.Souscripteur.Code;
            y.Pbcru = user.Login;
            y.Pbsta = year;
            y.Pbstm = month;
            y.Pbstj = day;
            y.Pbmja = year;
            y.Pbmjm = month;
            y.Pbmjj = day;
            y.Pbava = aff.DateEffetAvenant.NYear();
            y.Pbavm = aff.DateEffetAvenant.NMonth();
            y.Pbavj = aff.DateEffetAvenant.NDay();
            y.Pbefa = aff.DateEffet.NYear();
            y.Pbefm = aff.DateEffet.NMonth();
            y.Pbefj = aff.DateEffet.NDay();
            y.Pbeta = aff.Etat.AsCode();
            y.Pbsit = aff.Situation.AsCode();
            y.Pbstf = aff.CodeMotifSituation;
            y.Pbttr = aff.TypeTraitement.Code;
            y.Pbsaa = aff.DateSaisie.NYear();
            y.Pbsam = aff.DateSaisie.NMonth();
            y.Pbsaj = aff.DateSaisie.NDay();
            y.Pbsah = aff.DateSaisie.NTime();
            y.Pbadh = aff.IdAdresse;
            y.Pbbra = aff.Branche.Code;
            y.Pbtbr = aff.Branche.Code;
            y.Pbtil = aff.Interlocuteur != null ? "T" : "";
            y.Pbin5 = aff.Interlocuteur?.Id ?? 0;
            y.Pbict = aff.CourtierGestionnaire.Code;
            y.Pbcta = aff.CourtierApporteur.Code;
            y.Pbctp = aff.CourtierPayeur.Code;
            y.Pbias = aff.Preneur.Code;
            y.Pbavn = aff.NumeroAvenant;
            y.Pboct = aff.ReferenceCourtier;
            y.Pbmo1 = aff.Metadata.MotClef1;
            y.Pbmo2 = aff.Metadata.MotClef2;
            y.Pbmo3 = aff.Metadata.MotClef3;
            y.Pbdev = aff.Devise.Code;
        }

        public LockState GetLockState(AffaireId affaireId) {
            var verrou = this.kVerrouRepository.GetByLockKey(affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.TypeAffaire.AsCode()).FirstOrDefault();
            if (verrou is null) {
                return null;
            }
            return new LockState {
                User = verrou.Kavcru,
                Action = verrou.Kavact,
                ActeGestion = verrou.Kavactg,
                IsLocked = true
            };
        }

        public IDictionary<AffaireId, LockState> GetUserLocks(string user) {
            return this.kVerrouRepository.GetByUser(user).ToDictionary(
                x => new AffaireId { CodeAffaire = x.Kavipb, NumeroAliment = x.Kavalx, TypeAffaire = x.Kavtyp.ParseCode<AffaireType>() },
                x => new LockState {
                    User = x.Kavcru,
                    Action = x.Kavact,
                    ActeGestion = x.Kavactg,
                    IsLocked = true
                });
        }

        public void SetGareat(string ipb, int alx, string trancheMax, string flag) {
            var entete = this.rtRepo.Get(ipb.ToIPB(), alx);
            entete.Jdapr = flag;
            entete.Jdaar = trancheMax;
            this.rtRepo.Update(entete);
        }

        public void UpdateLCI(string codeAffaire, int version, ValeursOptionsTarif lci, bool isIndexee) {
            var kpent = this.kRepo.SelectByIpb(codeAffaire.ToIPB()).FirstOrDefault(e => e.Kaaalx == version);
            kpent.Kaalcivalo = lci.ValeurOrigine;
            kpent.Kaalcivala = lci.ValeurActualise;
            kpent.Kaalciunit = lci.Unite.Code;
            kpent.Kaalcibase = lci.Base.Code;
            kpent.Kaalciina = isIndexee.ToYesNo();
            kpent.Kaakdiid = lci.ExpressionComplexe?.Id > 0 ? lci.ExpressionComplexe.Id : 0;
            this.kRepo.Update(kpent);
        }

        public LockState SetLockState(AffaireId affaireId, LockState state) {
            if (!state.IsLocked) {
                var verrou = this.kVerrouRepository.GetByLockKey(affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.TypeAffaire.AsCode()).FirstOrDefault();
                if (verrou != null) {
                    if (verrou.Kavcru?.Trim().ToUpper() != state.User?.Trim().ToUpper()) {
                        throw new ArgumentException("Utilisateur incorrect");
                    }
                    this.kVerrouRepository.Delete(verrou);
                }
            }
            else {
                this.kVerrouRepository.Insert(new KVerrou {
                    Kavact = state.Action,
                    Kavalx = affaireId.NumeroAliment,
                    Kavactg = state.ActeGestion,
                    Kavavn = affaireId.NumeroAvenant.GetValueOrDefault(),
                    Kavcrd = DateTime.Now.ToIntYMD(),
                    Kavcrh = DateTime.Now.ToIntHMS(),
                    Kavcru = state.User,
                    Kavipb = affaireId.CodeAffaire,
                    Kavlib = $"Verrouillage {affaireId.TypeAffaire.ToString()} {affaireId.CodeAffaire} ({affaireId.NumeroAliment})",
                    Kavnum = 0,
                    Kavsbr = string.Empty,
                    Kavserv = "PRODU",
                    Kavsua = 0,
                    Kavtyp = affaireId.TypeAffaire.AsCode(),
                    Kavverr = Booleen.Oui.AsCode()
                });
            }
            return state;
        }

        private static void UpdateKPEnt(Affaire aff, KpEnt k) {
            k.Kaaass = aff.Preneur.PreneurEstAssure.ToYesNo();
        }

        private static void UpdateYRT(Affaire aff, YprtEnt r) {
            r.Jdafr = aff.CibleCategorie.Categorie.MontantFraisAccessoires; //.V_MONTFRAISACC;
            r.Jdatt = aff.CibleCategorie.Categorie.ATaxeAttentant.ToYesNo();
            r.Jdcnc = aff.CibleCategorie.Categorie.TauxComCatastropheNaturelle;
            r.Jdina = aff.CibleCategorie.Categorie.AIndexation.ToYesNo();
            r.Jdixc = aff.CibleCategorie.Categorie.AIndexationCapitaux.ToYesNo();
            r.Jdixf = aff.CibleCategorie.Categorie.AIndexatioFranchise.ToYesNo();
            r.Jdixl = aff.CibleCategorie.Categorie.AIndexationLci.ToYesNo();
            r.Jdixp = aff.CibleCategorie.Categorie.AIndexationPrime.ToYesNo();
            r.Jddpv = aff.PreavisMois;
            //r.Jdina = (aff.IndiceReference == null).ToYesNo();
            r.Jdind = aff.IndiceReference.Code;
            r.Jdivo = aff.ValeurIndiceOrigine;
            r.Jdiva = aff.ValeurIndiceActualisee;
            r.Jditc = aff.IntercalaireExiste.ToYesNo();
            r.Jdcna = aff.SoumisCatNat.ToYesNo();
            r.Jddpj = aff.Periode.Debut.NDay();
            r.Jddpm = aff.Periode.Debut.NMonth();
            r.Jddpa = aff.Periode.Debut.NYear();
            r.Jdfpj = aff.Periode.Fin.NDay();
            r.Jdfpm = aff.Periode.Fin.NMonth();
            r.Jdfpa = aff.Periode.Fin.NYear();
            r.Jdpej = aff.ProchaineEcheance.Date.NDay();
            r.Jdpem = aff.ProchaineEcheance.Date.NMonth();
            r.Jdpea = aff.ProchaineEcheance.Date.NYear();
        }

        /// <summary>
        /// Lazy loading proxy
        /// </summary>
        internal class AffaireProxy : Affaire {
            private readonly AffaireRepository parent;


            public AffaireProxy(AffaireRepository r) {
                this.parent = r;
            }
        }

        /// <summary>
        /// Lazy loading proxy
        /// </summary>
        internal class AssureProxy : Assure {
            private readonly Object syncRoot = new Object();
            private readonly YAssureRepository arepo;
            private readonly YAssNomRepository nrepo;
            private readonly IReferentialRepository refRepo;

            public AssureProxy(
                int code,
                int numero,
                YAssureRepository arepo,
                YAssNomRepository nrepo,
                IReferentialRepository refRepo
                ) {
                this.Code = code;
                this.Numero = numero;
                this.arepo = arepo;
                this.nrepo = nrepo;
                this.refRepo = refRepo;
            }
            bool isLoaded = false;

            private void Load() {
                if (!this.isLoaded) {
                    lock (this.syncRoot) {
                        if (!this.isLoaded && Code > 0) {
                            var a = this.arepo.Get(Code);
                            if (a is null) {
                                throw new Exception($"Le code {Code} ne correspond a aucun assuré");
                            }
                            var n = this.nrepo.GetByAssure(Code);
                            // code interlocuteur = 0  && type = 'A'
                            NomAssure = n.Where(x => x.Aninl == Numero && x.Antnm == "A").Select(x => x.Annom).FirstOrDefault();
                            Adresse = new Adresse {
                                // concatener CP
                                CodePostal = a.Asdep.PadLeft(2, '0') + a.Ascpo.PadLeft(3, '0'),
                                Ligne1 = a.Asad1,
                                Ligne2 = a.Asad2,
                                Pays = this.refRepo.GetValue<Pays>(a.Aspay),
                                Ville = a.Asvil
                            };
                            TelephoneBureau = a.Astel;
                            Siren = a.Assir.ToString("G", CultureInfo.InvariantCulture).PadLeft(8, '0');
                            NomSecondaires = n.Select(x => x.Annom).ToList();

                        }
                        this.isLoaded = true;
                    }
                }
            }

            public override Adresse Adresse { get { Load(); return base.Adresse; } }
            public override string NomAssure { get { Load(); return base.NomAssure; } }
            public override List<string> NomSecondaires { get { Load(); return base.NomSecondaires; } }
            public override bool PreneurEstAssure { get { Load(); return base.PreneurEstAssure; } }
            public override string Siren { get { Load(); return base.Siren; } }
            public override string TelephoneBureau { get { Load(); return base.TelephoneBureau; } }
        }

        internal static Affaire BuildFromYpoBase(YpoBase y, IReferentialRepository refRepo) {
            if (y is null) {
                return null;
            }
            return new Affaire {
                Branche = refRepo.GetValue<Branche>(y.Pbbra),
                Categorie = y.Pbcat,
                NatureTravaux = refRepo.GetValue<NatureTravauxAffaire>(y.Pbnat),
                NatureContrat = refRepo.GetValue<NatureAffaire>(y.Pbnpl),
                Souscripteur = refRepo.GetUtilisateur(y.Pbsou),
                Gestionnaire = refRepo.GetUtilisateur(y.Pbges),
                SousBranche = y.Pbsbr,
                CodeAffaire = y.Pbipb,
                CodeOffre = y.Pboff,
                NumeroAliment = y.Pbalx,
                DateAccord = MakeNullableDateTime(y.Pbsaa, y.Pbsam, y.Pbsaj),
                DateEffet = MakeNullableDateTime(y.Pbefa, y.Pbefm, y.Pbefj, y.Pbefh, 0, 0),
                DateEffetAvenant = MakeNullableDateTime(y.Pbava, y.Pbavm, y.Pbavj),
                DateFin = MakeNullableDateTime(y.Pbfea, y.Pbfem, y.Pbfej, y.Pbfeh, 0, 0),
                DateCreation = MakeDateTime(y.Pbdea, y.Pbdem, y.Pbdej),
                TypeAffaire = y.Pbtyp.ParseCode<AffaireType>(),
                Etat = y.Pbeta.ParseCode<EtatAffaire>(),
                Situation = y.Pbsit.ParseCode<SituationAffaire>(),
                CodeMotifSituation = y.Pbstf,
                TypeTraitement = refRepo.GetValue<TypeTraitement>(y.Pbttr),
                IdAdresse = y.Pbadh,
                ReferenceCourtier = y.Pboct,
                Metadata = new AffaireMetadata {
                    MotClef1 = y.Pbmo1,
                    MotClef2 = y.Pbmo2,
                    MotClef3 = y.Pbmo3
                },
                Descriptif = y.Pbref,
                DateSaisie = MakeNullableDateTime(y.Pbsaa, y.Pbsam, y.Pbsaj, y.Pbsah, 0, 0),
                Devise = refRepo.GetValue<Devise>(y.Pbdev),
                NumeroAvenant = y.Pbavn,
                Periodicite = new Periodicite() { Code = y.Pbper },
                TypeAccord = refRepo.GetValue<TypeAccord>(y.Pbtac),
                DelaisRelanceJours = y.Pbrld
            };
        }

        /// <summary>
        /// Lazy loading proxy
        /// </summary>
        internal class CourtierProxy : Domain.Affaire.Courtier {
            private readonly Object syncRoot = new Object();

            private int code;
            private YcourtiRepository ycourtiRepo;
            private IReferentialRepository refRepo;
            bool isLoaded = false;
            public CourtierProxy(
                int id,
                int code,
                YcourtiRepository ycourtiRepo,
                IReferentialRepository refRepo
                ) {
                this.Code = id;
                this.Numero = code;
                this.ycourtiRepo = ycourtiRepo;
                this.refRepo = refRepo;
            }
            private void Load() {
                if (!isLoaded) {
                    lock (this.syncRoot) {
                        if (!this.isLoaded) {
                            var c = this.ycourtiRepo.FullCourtier(Code, code).FirstOrDefault();
                            if (c != null) {
                                this.Adresse = new Adresse {
                                    Ligne1 = c.Tcad1,
                                    Ligne2 = c.Tcad2,
                                    CodePostal = c.Abpdp6.PadLeft(2, '0') + c.Abpcp6.ToString().PadLeft(3, '0'),
                                    Pays = refRepo.GetValue<Pays>(c.Abppay),
                                    Ville = c.Abpvi6
                                };
                                this.Email = c.Tcaem;
                                this.Nom = c.Tnnom;
                                this.TypeCourtier = c.Tctyp;
                            }
                            isLoaded = true;
                        }

                    }
                }
            }

            public override Adresse Adresse { get { Load(); return base.Adresse; } }
            public override string Email { get { Load(); return base.Email; } }
            public override string Nom { get { Load(); return base.Nom; } }
            public override string TypeCourtier { get { Load(); return base.TypeCourtier; } }
            public override string Inspecteur { get { Load(); return base.Inspecteur; } }

        }

    }


}
