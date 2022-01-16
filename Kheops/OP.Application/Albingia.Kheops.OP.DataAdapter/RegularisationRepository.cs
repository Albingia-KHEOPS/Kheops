using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Regularisation;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.Framework.Common.Constants;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain;
using OP.WSAS400.DTO.Regularisation;
using ALBINGIA.Framework.Common.Business;

namespace Albingia.Kheops.OP.DataAdapter {
    public class RegularisationRepository : IRegularisationRepository {
        private readonly IReferentialRepository referentialRepository;
        private readonly IGarantieRepository garantieRepository;
        private readonly ITraceRepository potraceRepository;
        private readonly KpRguRepository kpRguRepository;
        private readonly KpRguGRepository kpRgugRepository;
        private readonly KpSelWRepository kpSelWRepository;
        private readonly KpRguWpRepository kpRguWpRepository;
        private readonly KpRguRRepository kprguRRepository;
        private readonly YpoTracRepository ypoTracRepository;
        public RegularisationRepository(KpRguRepository kpRguRepository, KpRguWpRepository kpRguWpRepository, ITraceRepository potraceRepository, IReferentialRepository referentialRepository, IGarantieRepository garantieRepository, KpSelWRepository kpSelWRepository, KpRguGRepository kpRgugRepository, KpRguRRepository kprguRRepository, YpoTracRepository ypoTracRepository) {
            this.kpRguRepository = kpRguRepository;
            this.referentialRepository = referentialRepository;
            this.kpSelWRepository = kpSelWRepository;
            this.potraceRepository = potraceRepository;
            this.garantieRepository = garantieRepository;
            this.kpRgugRepository = kpRgugRepository;
            this.kpRguWpRepository = kpRguWpRepository;
            this.kprguRRepository = kprguRRepository;
            this.ypoTracRepository = ypoTracRepository;
        }
        public Regularisation GetById(long id) {
            var rgu = this.kpRguRepository.Get(id);
            return new Regularisation {
                Id = rgu.Khwid,
                Exercice = rgu.Khwexe,
                AffaireId = new AffaireId {
                    CodeAffaire = rgu.Khwipb,
                    NumeroAliment = rgu.Khwalx,
                    TypeAffaire = AffaireType.Contrat,
                    NumeroAvenant = rgu.Khwavn
                },
                DateDebut = Tools.MakeDateTime(rgu.Khwdebp),
                DateFin = Tools.MakeDateTime(rgu.Khwfinp),
                Encaissement = this.referentialRepository.GetValue<Quittancement>(rgu.Khwenc),
                IdCourtier = rgu.Khwict,
                IdCourtierCommission = rgu.Khwicc,
                Mode = this.referentialRepository.GetValue<ModeRegularisation>(rgu.Khwmrg),
                Motif = this.referentialRepository.GetValue<MotifRegularisation>(rgu.Khwmtf),
                Niveau = rgu.Khwacc.ParseCode<NiveauRegularisation>(),
                Situation = this.referentialRepository.GetValue<SituationRegularisation>(rgu.Khwsit),
                SuivieAvenant = rgu.Khwsuav != Booleen.Non.AsCode(),
                TauxCommission = rgu.Khwxcm,
                TauxCommissionCATNAT = rgu.Khwcnc,
                MontantFraisAcc = rgu.Khwafr,
                CodeApplicationFraisAcc = rgu.Khwafc,
                CodeEtat = rgu.Khweta
            };
        }

        public Regularisation GetByAffaire(AffaireId affaireId) {
            var rgu = this.kpRguRepository.GetByAffaire(AffaireType.Contrat.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant ?? 0).FirstOrDefault();
            return rgu is null ? null : new Regularisation {
                Id = rgu.Khwid,
                Exercice = rgu.Khwexe,
                AffaireId = new AffaireId {
                    CodeAffaire = rgu.Khwipb,
                    NumeroAliment = rgu.Khwalx,
                    TypeAffaire = AffaireType.Contrat,
                    NumeroAvenant = rgu.Khwavn
                },
                DateDebut = Tools.MakeDateTime(rgu.Khwdebp),
                DateFin = Tools.MakeDateTime(rgu.Khwfinp),
                Encaissement = this.referentialRepository.GetValue<Quittancement>(rgu.Khwenc),
                IdCourtier = rgu.Khwict,
                IdCourtierCommission = rgu.Khwicc,
                Mode = this.referentialRepository.GetValue<ModeRegularisation>(rgu.Khwmrg),
                Motif = this.referentialRepository.GetValue<MotifRegularisation>(rgu.Khwmtf),
                Niveau = rgu.Khwacc.ParseCode<NiveauRegularisation>(),
                Situation = this.referentialRepository.GetValue<SituationRegularisation>(rgu.Khwsit),
                SuivieAvenant = rgu.Khwsuav != Booleen.Non.AsCode(),
                TauxCommission = rgu.Khwxcm,
                TauxCommissionCATNAT = rgu.Khwcnc,
                MontantFraisAcc = rgu.Khwafr,
                CodeApplicationFraisAcc = rgu.Khwafc,
                CodeEtat = rgu.Khweta
            };
        }

        public Regularisation GetBNSByAffaire(AffaireId affaireId)
        {
            var rgu = this.kpRguRepository.GetByAffaire(AffaireType.Contrat.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant ?? 0).FirstOrDefault(x => x.Khwmrg == "BNS");
            return ConvertRgu(rgu);
        }

        public IEnumerable<RegulRisque> GetRegulRisques(Regularisation rg) {
            return this.kprguRRepository.GetByAffaire(AffaireType.Contrat.AsCode(), rg.AffaireId.CodeAffaire, rg.AffaireId.NumeroAliment)
                .Where(x => x.Kilkhwid == rg.Id)
                .Select(x => new RegulRisque {
                    AffaireId = new AffaireId { CodeAffaire = x.Kilipb, NumeroAliment = x.Kilalx, TypeAffaire = x.Kiltyp.ParseCode<AffaireType>() },
                    Id = x.Kilid,
                    IdRegul = x.Kilkhwid,
                    Numero = x.Kilrsq
                })
                .ToList();
        }

        public void ResetByLot(long idLot) {
            this.kpSelWRepository.DeleteLot(idLot);
        }

        public long InitLot() {
            return this.kpSelWRepository.NewId();
        }

        public void AddSelection(SelectionRegularisation selection) {
            this.kpSelWRepository.Insert(new KpSelW {
                Khvalx = selection.AffaireId.NumeroAliment,
                Khvavn = selection.AffaireId.NumeroAvenant.GetValueOrDefault(),
                Khvdeb = selection.DateDebut.ToIntYMD(),
                Khveco = selection.EnCours.HasValue ? (selection.EnCours == true ? Booleen.Oui.AsCode() : Booleen.Non.AsCode()) : string.Empty,
                Khvedtb = selection.TypeEdition,
                Khvfin = selection.DateFin.ToIntYMD(),
                Khvfor = selection.NumeroFormule,
                Khvid = selection.IdLot,
                Khvipb = selection.AffaireId.CodeAffaire,
                Khvkdeid = selection.IdGarantie,
                Khvobj = selection.NumeroObjet,
                Khvperi = selection.Perimetre.AsCode(),
                Khvrsq = selection.NumeroRisque,
                Khvtyp = AffaireType.Contrat.AsCode(),
                Khvkdegar = selection.CodeGarantie,
                Khvkdeseq = selection.SequenceGarantie
            });
        }

        public IEnumerable<SelectionRegularisation> GetSelection(long idLot) {
            return this.kpSelWRepository.GetByLot(idLot).Select(x => new SelectionRegularisation {
                IdLot = idLot,
                AffaireId = new AffaireId {
                    CodeAffaire = x.Khvipb,
                    NumeroAliment = x.Khvalx,
                    NumeroAvenant = x.Khvavn,
                    TypeAffaire = AffaireType.Contrat
                },
                NumeroFormule = x.Khvfor,
                NumeroRisque = x.Khvrsq,
                NumeroObjet = x.Khvobj,
                Perimetre = x.Khvperi.ParseCode<PerimetreSelectionRegul>(),
                IdGarantie = x.Khvkdeid,
                EnCours = x.Khveco == Booleen.Oui.AsCode(),
                TypeEdition = x.Khvedtb,
                DateDebut = Tools.MakeDateTime(x.Khvdeb),
                DateFin = Tools.MakeNullableDateTime(x.Khvfin),
                SequenceGarantie = x.Khvkdeseq,
                CodeGarantie = x.Khvkdegar
            });
        }

        public IEnumerable<SelectionRegularisation> GetSelectionGaranties(long idLot) {
            return GetSelection(idLot).Where(x => x.Perimetre == PerimetreSelectionRegul.Garantie);
        }

        public IEnumerable<PeriodeGarantie> GetPeriodesGaranties(long idRegul) {
            return this.kpRgugRepository.GetByRegul(idRegul).Select(x => new PeriodeGarantie {
                Id = x.Khxid,
                NumeroRisque = x.Khxrsq,
                NumeroFormule = x.Khxfor,
                BaseRegul = x.Khxbas,
                Definitif2 = x.Khxca2,
                Base = x.Khxbam,
                TauxBase = x.Khxbat,
                UniteTauxBase = x.Khxbau,
                Definitif2Prime = x.Khxcp2,
                Definitif2Taux = x.Khxct2,
                UniteTaux2 = x.Khxcu2,
                DateDebut = Tools.MakeDateTime(x.Khxdebp),
                DateFin = Tools.MakeDateTime(x.Khxfinp),
                IsValidated = x.Khxsit == SituationBase.Validee.AsCode(),
                MontantHTHorsCATNAT = x.Khxmht,
                CodeGarantie = x.Khxgaran
            });
        }

        public Regularisation Create(Regularisation rg, string user) {
            var kpRgu = GetDefaultRgu(user);
            kpRgu.Khwipb = rg.AffaireId.CodeAffaire;
            kpRgu.Khwalx = rg.AffaireId.NumeroAliment;
            kpRgu.Khwtyp = AffaireType.Contrat.AsCode();
            kpRgu.Khwavn = rg.AffaireId.NumeroAvenant.Value;
            kpRgu.Khwacc = rg.Niveau.AsCode();
            kpRgu.Khwttr = rg.SuivieAvenant ? AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF : AlbConstantesMetiers.TYPE_AVENANT_REGUL;
            kpRgu.Khwsuav = rg.SuivieAvenant ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
            kpRgu.Khwrgav = rg.IsAvenant ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
            kpRgu.Khwavnd = rg.DateAvenant.ToIntYMD();
            kpRgu.Khwexe = rg.Exercice;
            kpRgu.Khwdebp = rg.DateDebut.ToIntYMD();
            kpRgu.Khwfinp = rg.DateFin.ToIntYMD();
            kpRgu.Khwict = rg.IdCourtier;
            kpRgu.Khwicc = rg.IdCourtierCommission;
            kpRgu.Khwxcm = rg.TauxCommission;
            kpRgu.Khwcnc = rg.TauxCommissionCATNAT;
            kpRgu.Khwenc = rg.Encaissement?.Code ?? string.Empty;
            kpRgu.Khwmtf = rg.Motif?.Code ?? string.Empty;
            kpRgu.Khwmrg = rg.Mode?.Code ?? string.Empty;
            kpRgu.Khwafc = rg.CodeApplicationFraisAcc;
            kpRgu.Khwafr = rg.MontantFraisAcc;
            this.kpRguRepository.Insert(kpRgu);
            rg.Id = kpRgu.Khwid;
            return rg;
        }

        public RegulRisque CreateRegulRisque(RegulRisque risque) {
            var rgrsq = new KpRguR {
                Kilkhwid = risque.IdRegul,
                Kiltyp = AffaireType.Contrat.AsCode(),
                Kilipb = risque.AffaireId.CodeAffaire.ToIPB(),
                Kilalx = risque.AffaireId.NumeroAliment,
                Kilrsq = risque.Numero,
                Kilsit = " ",
                Kilfrc = " ",
                Kilfr0 = " "
            };
            this.kprguRRepository.Insert(rgrsq);
            risque.Id = rgrsq.Kilid;
            return risque;
        }

        public void Update(Regularisation regul, string user) {
            var rgu = this.kpRguRepository.Get(regul.Id);
            rgu.Khweta = regul.CodeEtat;
            rgu.Khwafc = regul.CodeApplicationFraisAcc;
            rgu.Khwafr = regul.MontantFraisAcc;
            rgu.Khwexe = regul.Exercice;
            rgu.Khwdebp = regul.DateDebut.ToIntYMD();
            rgu.Khwfinp = regul.DateFin.ToIntYMD();
            rgu.Khwicc = regul.IdCourtierCommission;
            rgu.Khwict = regul.IdCourtier;
            rgu.Khwenc = regul.Encaissement?.Code ?? string.Empty;
            rgu.Khwxcm = regul.TauxCommission;
            rgu.Khwcnc = regul.TauxCommissionCATNAT;
            rgu.Khwmtf = regul.Motif?.Code ?? string.Empty;
            rgu.Khwmrg = regul.Mode?.Code ?? string.Empty;
            rgu.Khwsuav = regul.SuivieAvenant ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
            rgu.Khwacc = regul.Niveau.AsCode();
            rgu.Khwstu = user;
            rgu.Khwstd = DateTime.Today.ToIntYMD();
            rgu.Khwsth = DateTime.Now.ToIntHMS();
            rgu.Khwmaju = user;
            rgu.Khwmajd = DateTime.Today.ToIntYMD();
            rgu.Khwrgav = regul.IsAvenant ? Booleen.Oui.AsCode() : Booleen.Non.AsCode();
            this.kpRguRepository.Update(rgu);
        }

        public void DeleteSelectionsRisques(long idLot, IEnumerable<int> numsRisques) {
            if (numsRisques?.Any() != true) {
                return;
            }
            this.kpSelWRepository.DeleteRisques(idLot, numsRisques);
        }

        public void DeleteSelectionsObjets(long idLot, int numRisque, IEnumerable<int> numsObjets) {
            if (numsObjets?.Any() != true || numRisque < 1) {
                return;
            }
            this.kpSelWRepository.DeleteObjets(idLot, numRisque, numsObjets);
        }

        public void DeleteSelectionsFormules(long idLot, IEnumerable<int> numsFormules) {
            if (numsFormules?.Any() != true) {
                return;
            }
            this.kpSelWRepository.DeleteFormules(idLot, numsFormules);
        }

        public int PurgeTempPeriodes(string codeOffre, int version) {
            return this.kpRguWpRepository.Purge(codeOffre, version);
        }

        public void InsertBlankTempPeriode(AffaireId affaireId, int risque, int formule, long idGarantie, string codeGarantie, DateTime dateDebut, DateTime dateFin, string typeRegulGarantie) {
            this.kpRguWpRepository.InsertBlank(
                affaireId.CodeAffaire,
                affaireId.NumeroAliment,
                risque,
                formule,
                idGarantie,
                codeGarantie,
                dateDebut.ToIntYMD(),
                dateFin.ToIntYMD(),
                typeRegulGarantie);
        }

        public IEnumerable<LigneRegularisationGarantieDto> GetDetailsInfoGarantie(long id, long idLot, long idRegul) {
            return this.kpSelWRepository.SelectInfoDetailsGarantie(id, idLot, idRegul);
        }

        public IEnumerable<MouvementGarantie> GetMouvementsGaranties(AffaireId affaireId, long idRegul, int numRisque, int numFormule, AccessMode mode) {
            if (affaireId.IsHisto || mode == AccessMode.CONSULT) {
                return this.kpRgugRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment)
                    .Where(x => x.Khxkhwid == idRegul)
                    .Select(x => new MouvementGarantie {
                        Assiette = x.Khxbas,
                        DateFin = Tools.MakeDateTime(x.Khxfinp),
                        DateDebut = Tools.MakeDateTime(x.Khxdebp),
                        MontantBase = x.Khxbam,
                        UniteTauxBase = x.Khxbau.ParseCode<UniteBase>(),
                        TauxBase = x.Khxbat,
                        IdGarantie = x.Khxkdeid,
                        CodeGarantie = x.Khxgaran
                    });
            }
            else {
                // idRegul is not usable in this context
                return this.kpRguWpRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment)
                    .Select(x => new MouvementGarantie {
                        Assiette = x.Khybas,
                        DateFin = Tools.MakeDateTime(x.Khyfinp),
                        DateDebut = Tools.MakeDateTime(x.Khydebp),
                        MontantBase = x.Khybam,
                        UniteTauxBase = x.Khybau.ParseCode<UniteBase>(),
                        TauxBase = x.Khybat,
                        IdGarantie = x.Khykdeid,
                        CodeGarantie = x.Khygaran
                    });
            }
        }

        public IEnumerable<MouvementGarantie> GetMouvementsGarantie(AffaireId affaireId, long idRegul, int numRisque, int numFormule, string codeGarantie) {
            if (affaireId.IsHisto) {
                var allMouvements = this.kpRgugRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
                return allMouvements.Where(x => x.Khxgaran == codeGarantie && x.Khxfor == numFormule && x.Khxrsq == numRisque)
                    .Where(x => x.Khxkhwid == idRegul)
                    .Select(x => new MouvementGarantie {
                        Assiette = x.Khxbas,
                        DateFin = Tools.MakeDateTime(x.Khxfinp),
                        DateDebut = Tools.MakeDateTime(x.Khxdebp),
                        MontantBase = x.Khxbam,
                        UniteTauxBase = x.Khxbau.ParseCode<UniteBase>(),
                        TauxBase = x.Khxbat
                    });
            }
            else {
                // idRegul is not usable in this context
                var allMouvements = this.kpRguWpRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
                return allMouvements.Where(x => x.Khygaran == codeGarantie && x.Khyfor == numFormule && x.Khyrsq == numRisque)
                    .Select(x => new MouvementGarantie {
                        Assiette = x.Khybas,
                        DateFin = Tools.MakeDateTime(x.Khyfinp),
                        DateDebut = Tools.MakeDateTime(x.Khydebp),
                        MontantBase = x.Khybam,
                        UniteTauxBase = x.Khybau.ParseCode<UniteBase>(),
                        TauxBase = x.Khybat
                    });
            }
        }

        public void EnsureInsertMouvementsRC(string codeOffre, int version, int numRisque, int numFormule, long idRegul, long idLot) {
            this.kpRgugRepository.EnsureInsertMouvementsRC(codeOffre, version, numRisque, numFormule, idRegul, idLot);
        }

        public void EnsureInsertMouvements(string codeOffre, int version, int numRisque, int numFormule, long idGarantie, long idRegul) {
            this.kpRgugRepository.EnsureInsertMouvements(codeOffre, version, numRisque, numFormule, idGarantie, idRegul);
        }

        public void TraceInfo(TraceContext traceContext) {
            this.ypoTracRepository.TraceInfo(
                traceContext.AffaireId.CodeAffaire,
                traceContext.AffaireId.NumeroAliment,
                traceContext.AffaireId.TypeAffaire.AsCode(),
                traceContext.AffaireId.NumeroAvenant.GetValueOrDefault(),
                traceContext.TypeTraitement,
                traceContext.TypeAction,
                traceContext.Ordre,
                traceContext.User,
                DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.ToIntHM(),
                traceContext.Libelle, "I", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.ToIntHM());
        }

        public void ResetRsqGar(long reguleId)
        {
            this.kprguRRepository.GetByRegul(reguleId).ToList().ForEach(x =>
            {
                x.Kilsit = "N";
                this.kprguRRepository.Update(x);
            });
            this.kpRgugRepository.GetByRegul(reguleId).ToList().ForEach(x =>
            {
                x.Khxsit = "N";
                x.Khxca2 = 0;
                x.Khxct2 = 0;
                x.Khxcu2 = "";
                this.kpRgugRepository.Update(x);
            });
        }

        public void Reprise(AffaireId id) {
            if (!id.TypeTraitement.IsIn(AlbConstantesMetiers.TYPE_AVENANT_REGUL, AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)) {
                return;
            }
            var list = this.kpRguRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value);
            if (list?.Any() ?? false) {
                var rg = list.First();
                if (rg.Khwmrg.ToUpper().IsIn(RegularisationMode.PB.AsCode(), RegularisationMode.Standard.AsCode())) {
                    this.kpRguWpRepository.Purge(id.CodeAffaire, id.NumeroAliment);
                    var listGug = this.kpRgugRepository.GetByRegul(rg.Khwid).ToList();
                    listGug.ForEach(g => this.kpRgugRepository.Delete(g));
                    var listGur = this.kprguRRepository.GetByRegul(rg.Khwid).ToList();
                    listGur.ForEach(r => this.kprguRRepository.Delete(r));
                    this.kpRguRepository.Delete(rg);
                    var w = this.kpSelWRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment);
                    w.Select(x => x.Khvid).Distinct().ToList().ForEach(x => this.kpSelWRepository.DeleteLot(x));
                }
            }
        }

        private static KpRgu GetDefaultRgu(string user) {
            return new KpRgu {
                Khwavp = 0,
                Khwdesi = 0,
                Khwobsv = 0,
                Khwobsc = 0,
                Khwtrgu = " ",
                Khwipk = 0,
                Khweta = "N",
                Khwsit = "A",
                Khwstu = user,
                Khwstd = DateTime.Today.ToIntYMD(),
                Khwsth = DateTime.Now.ToIntHMS(),
                Khwcru = user,
                Khwcrd = DateTime.Today.ToIntYMD(),
                Khwmaju = user,
                Khwmajd = DateTime.Today.ToIntYMD(),
                Khwatt = "N",
                Khwmht = 0,
                Khwcnh = 0,
                Khwgrg = 0,
                Khwttt = 0,
                Khwmtt = 0,
                Khwmin = 0,
                Khwbrnc = 0,
                Khwpbt = 0,
                Khwpba = 0,
                Khwpbs = 0,
                Khwpbr = 0,
                Khwpbp = 0,
                Khwria = 0,
                Khwriaf = 0,
                Khwemh = 0,
                Khwemhf = 0,
                Khwcot = 0,
                Khwbrnt = 0,
                Khwschg = 0,
                Khwsidf = 0,
                Khwsrec = 0,
                Khwspro = 0,
                Khwspre = 0,
                Khwsrep = 0,
                Khwsrim = 0,
                Khwmhc = 0,
                Khwpbtr = 0,
                Khwemd = 0,
                Khwspc = 0,
                Khwasv = 0,
                Khwnem = " "
            };
        }
        private Regularisation ConvertRgu(KpRgu rgu)
        {
            return rgu is null ? null : new Regularisation
            {
                Id = rgu.Khwid,
                Exercice = rgu.Khwexe,
                AffaireId = new AffaireId
                {
                    CodeAffaire = rgu.Khwipb,
                    NumeroAliment = rgu.Khwalx,
                    TypeAffaire = AffaireType.Contrat,
                    NumeroAvenant = rgu.Khwavn
                },
                DateDebut = Tools.MakeDateTime(rgu.Khwdebp),
                DateFin = Tools.MakeDateTime(rgu.Khwfinp),
                Encaissement = this.referentialRepository.GetValue<Quittancement>(rgu.Khwenc),
                IdCourtier = rgu.Khwict,
                IdCourtierCommission = rgu.Khwicc,
                Mode = this.referentialRepository.GetValue<ModeRegularisation>(rgu.Khwmrg),
                Motif = this.referentialRepository.GetValue<MotifRegularisation>(rgu.Khwmtf),
                Niveau = rgu.Khwacc.ParseCode<NiveauRegularisation>(),
                Situation = this.referentialRepository.GetValue<SituationRegularisation>(rgu.Khwsit),
                SuivieAvenant = rgu.Khwsuav != Booleen.Non.AsCode(),
                TauxCommission = rgu.Khwxcm,
                TauxCommissionCATNAT = rgu.Khwcnc,
                MontantFraisAcc = rgu.Khwafr,
                CodeApplicationFraisAcc = rgu.Khwafc,
                CodeEtat = rgu.Khweta
            };
        }
    }
}
