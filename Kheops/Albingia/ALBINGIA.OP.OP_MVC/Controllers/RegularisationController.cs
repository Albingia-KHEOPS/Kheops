using Albingia.Common;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using Newtonsoft.Json;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers.Regularisation
{
    public class RegularisationController : ControllersBase<ModeleRegularisationPage>
    {
        private static readonly Dictionary<string, RegularisationMode> RegularisationModeCodes = Enum.GetValues(typeof(RegularisationMode)).Cast<RegularisationMode>().ToDictionary(v => v.AsCode().ToUpper(), v => v);
        internal static RegularisationMode ParseMode(string mode)
        {
            if (mode == null)
            {
                return 0;
            }

            if (RegularisationModeCodes.TryGetValue(mode.ToUpper(), out RegularisationMode m))
            {
                return m;
            }
            else if (Enum.TryParse(mode, true, out m))
            {
                return m;
            }

            return 0;
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CheckListRisques(RegularisationContext context)
        {
            return GetView(context, "Choix des risques");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculPBContrat(RegularisationContext context)
        {
            return GetView(context, "Régularisation contrat PB");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculPBRisque(RegularisationContext context)
        {
            return GetView(context, "Régularisation risque PB");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculBNSContrat(RegularisationContext context)
        {
            return GetView(context, "BNS");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculBNSRisque(RegularisationContext context)
        {
            return GetView(context, "Régularisation risque BNS");
        }


        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculBurnerContrat(RegularisationContext context)
        {
            return GetView(context, "Régularisation contrat BURNER");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculBurnerRisque(RegularisationContext context)
        {
            return GetView(context, "Régularisation risque BURNER");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculPBTRContrat(RegularisationContext context)
        {
            return GetView(context, "Régularisation contrat PB");
        }

        [ErrorHandler]
        [AlbVerifLockedOffer("context")]
        public ActionResult CalculPBTRRisque(RegularisationContext context)
        {
            return GetView(context, "Régularisation risque PB");
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult GetRegulDataForCalculation(RegularisationContext context)
        {
            RegularisationComputeData data = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;

                AvenantRegularisationDto modeleAvtRegul = context.ModeleAvtRegul;
                long numAvt = -1;
                if (modeleAvtRegul != null)
                    numAvt = modeleAvtRegul.NumAvt;
                if (context.Scope == RegularisationScope.Contrat)
                {
                    data = serviceContext.GetInfosRegularisationContrat(context.Mode, context.RgId, numAvt);
                }
                else if (context.Scope == RegularisationScope.Risque)
                {
                    data = serviceContext.GetInfosRegularisationRisque(context.Mode, context.RgId, context.RsqId, numAvt);
                }
            }

            if (data is null)
            {
                throw new AlbException(new Exception("Données de regularisation introuvables"));
            }



            switch (context.Mode)
            {
                case RegularisationMode.BNS:
                    return GetRegulDataForCalculationBNS(context, data);

                case RegularisationMode.Burner:
                    return GetRegulDataForCalculationBurner(context, data);

                default:
                    return GetRegulDataForCalculationPB(context, data);
            }

        }

        private ViewResult GetView(RegularisationContext context, string title)
        {
            InitContentData(context, title);
            return View(this.model);
        }

        [ErrorHandler]
        private JsonResult GetRegulDataForCalculationBNS(RegularisationContext context, RegularisationComputeData data)
        {
            return new JsonResult
            {
                Data = new
                {
                    infosContrat = new
                    {
                        CodeOffre = context.IdContrat.CodeOffre,
                        TypeContrat = context.IdContrat.Type,
                        Version = context.IdContrat.Version.ToString(),
                        DebutPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateDebut)).Value.ToShortDateString(),
                        FinPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateFin)).Value.ToShortDateString(),
                        Type = string.Empty,//context.Mode.GetAttributeOfType<DisplayAttribute>().Name,
                        RegimeTaxe = data.RegimeTaxe,
                        DisplayTypeContrat = (!string.IsNullOrEmpty(context.IdContrat.LibTypeContrat) ? string.Format("{0} - {1}", context.IdContrat.TypeContrat, context.IdContrat.LibTypeContrat) : context.IdContrat.TypeContrat),
                        Title = data.Label.EmptyIfNull(),
                        Formule = data.Formule,
                        CodeTaxe = data.CodeTaxe
                    },
                    infosFormule = new InfosFormuleRegularisation
                    {
                        TauxAppel = data.TauxAppel.ToString() == "0" ? string.Empty : data.TauxAppel.ToString(),
                        Ristourne = data.Ristourne.ToString(),
                        InitialTauxAppelRetenu = data.TauxAppelRetenu == 0 ? 100 : data.TauxAppelRetenu,
                        InitialCotisationPeriode = data.CotisationPeriode
                    },
                    donneesRegul = new
                    {
                        ReguleMode = context.Mode.ToString(),
                        TauxAppel = data.TauxAppel,
                        TauxAppelRetenu = data.TauxAppelRetenu == 0 ? 100 : data.TauxAppelRetenu,
                        CotisationPeriode = data.CotisationPeriode,
                        CotisationsRetenues = data.CotisationsRetenues,
                        LibelleMontant = "Montant calculé",
                        SigneMontant = data.MontantCalcule < 0 ? "-" : string.Empty,
                        MontantCalcule = data.MontantCalcule,
                        MontantAffiche = Math.Abs(data.MontantCalcule),
                        Ristourne = data.Ristourne.ToString(),
                        PrimeCalculee = data.PrimeCalculee,
                        Etat = data.Etat.Trim().Equals("V"),
                        IsAnticipee = data.IsAnticipee
                    }
                }
            };
        }

        [ErrorHandler]
        private JsonResult GetRegulDataForCalculationBurner(RegularisationContext context, RegularisationComputeData data)
        {
            return new JsonResult
            {
                Data = new
                {
                    infosContrat = new
                    {
                        CodeOffre = context.IdContrat.CodeOffre,
                        TypeContrat = context.IdContrat.Type,
                        Version = context.IdContrat.Version.ToString(),
                        DebutPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateDebut)).Value.ToShortDateString(),
                        FinPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateFin)).Value.ToShortDateString(),
                        Type = (context.Mode == RegularisationMode.Standard ? context.Mode.AsCode() : ""),
                        RegimeTaxe = data.RegimeTaxe,
                        DisplayTypeContrat = (!string.IsNullOrEmpty(context.IdContrat.LibTypeContrat) ? string.Format("{0} - {1}", context.IdContrat.TypeContrat, context.IdContrat.LibTypeContrat) : context.IdContrat.TypeContrat),
                        Title = data.Label.EmptyIfNull(),
                        Formule = data.Formule,
                        CodeTaxe = data.CodeTaxe
                    },
                    infosFormule = new InfosFormuleRegularisation
                    {
                        TauxMaxi = data.TauxMaxi,
                        PrcSeuilSP = data.PrcSeuilSP.ToString()
                    },
                    donneesRegul = new
                    {
                        ReguleMode = context.Mode.ToString(),
                        ChargementSinistres = data.ChargementSinistres,
                        CotisationPeriode = data.CotisationPeriode,
                        SeuilSPRetenu = data.SeuilSPRetenu,
                        Assiette = data.Assiette,
                        TauxMaxi = data.TauxMaxi,
                        PrimeMaxi = data.PrimeMaxi,
                        LibelleMontant = "Montant calculé",
                        SigneMontant = data.MontantCalcule < 0 ? "-" : string.Empty,
                        MontantCalcule = data.MontantCalcule,
                        MontantAffiche = Math.Abs(data.MontantCalcule),
                        Etat = data.Etat.Trim().Equals("V"),
                        TauxAppel = data.TauxAppel,
                        IsAnticipee = data.IsAnticipee
                    }
                }
            };
        }

        [ErrorHandler]
        private JsonResult GetRegulDataForCalculationPB(RegularisationContext context, RegularisationComputeData data)
        {

            return new JsonResult
            {
                Data = new
                {
                    infosContrat = new
                    {
                        CodeOffre = context.IdContrat.CodeOffre,
                        TypeContrat = context.IdContrat.Type,
                        Version = context.IdContrat.Version.ToString(),
                        DebutPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateDebut)).Value.ToShortDateString(),
                        FinPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateFin)).Value.ToShortDateString(),
                        Type = string.Empty,//context.Mode.GetAttributeOfType<DisplayAttribute>().Name,
                        RegimeTaxe = data.RegimeTaxe,
                        DisplayTypeContrat = (!string.IsNullOrEmpty(context.IdContrat.LibTypeContrat) ? string.Format("{0} - {1}", context.IdContrat.TypeContrat, context.IdContrat.LibTypeContrat) : context.IdContrat.TypeContrat),
                        Title = data.Label.EmptyIfNull(),
                        Formule = data.Formule,
                        CodeTaxe = data.CodeTaxe
                    },
                    infosFormule = new InfosFormuleRegularisation
                    {
                        NbAnnees = data.NbAnnees,
                        PrcSeuilSP = data.PrcSeuilSP.ToString(),
                        TauxAppel = data.TauxAppel.ToString() == "0" ? string.Empty : data.TauxAppel.ToString(),
                        Ristourne = data.Ristourne.ToString(),
                        RistourneAnticipe = data.RistourneAnticipee.ToString(),
                        PrcCotisationsRetenues = data.PrcCotisationsRetenues.ToString()
                    },
                    donneesRegul = new
                    {
                        ReguleMode = context.Mode.ToString(),
                        ChargementSinistres = data.ChargementSinistres,
                        CotisationPeriode = data.CotisationPeriode,
                        LibelleMontant = "Montant calculé",
                        SigneMontant = data.MontantCalcule < 0 ? "-" : string.Empty,
                        MontantCalcule = data.MontantCalcule,
                        MontantAffiche = Math.Abs(data.MontantCalcule),
                        CotisationsRetenues = data.CotisationsRetenues,
                        PrcCotisationsRetenues = data.PrcCotisationsRetenues,
                        MontantRistourneAnticipee = data.MontantRistourneAnticipee,
                        Ristourne = data.Ristourne.ToString(),
                        RistourneAnticipee = data.RistourneAnticipee.ToString(),
                        Etat = data.Etat.Trim().Equals("V"),
                        MontantComptant = data.MontantComptant,
                        TauxAppel = data.TauxAppel,
                        IsAnticipee = data.IsAnticipee
                    }
                }
            };
        }



        [ErrorHandler]
        [HttpPost]
        public JsonResult GetDataToComputePBTR(RegularisationContext context)
        {
            RegularisationComputeData data = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                if (context.Scope == RegularisationScope.Contrat)
                {
                    AvenantRegularisationDto modeleAvtRegul = context.ModeleAvtRegul;
                    long numAvt = -1;
                    if (modeleAvtRegul != null)
                        numAvt = modeleAvtRegul.NumAvt;
                    data = serviceContext.GetInfosRegularisationContratTR(context.RgId, numAvt);
                }
                else if (context.Scope == RegularisationScope.Risque)
                {
                    data = serviceContext.GetInfosRegularisationRisqueTR(context.RgId, context.RsqId);
                }
            }

            if (data is null)
            {
                throw new AlbException(new Exception("Données de regularisation introuvables"));
            }

            var test = new JsonResult
            {
                Data = new
                {
                    infosContrat = new
                    {
                        CodeOffre = context.IdContrat.CodeOffre,
                        TypeContrat = context.IdContrat.Type,
                        Version = context.IdContrat.Version.ToString(),
                        DebutPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateDebut)).Value.ToShortDateString(),
                        FinPeriode = AlbConvert.ConvertIntToDate(Int32.Parse(context.DateFin)).Value.ToShortDateString(),
                        Type = string.Empty,//context.Mode.GetAttributeOfType<DisplayAttribute>().Name,
                        RegimeTaxe = data.RegimeTaxe,
                        DisplayTypeContrat = (!string.IsNullOrEmpty(context.IdContrat.LibTypeContrat) ? string.Format("{0} - {1}", context.IdContrat.TypeContrat, context.IdContrat.LibTypeContrat) : context.IdContrat.TypeContrat),
                        Title = data.Label.EmptyIfNull(),
                        Formule = data.Formule,
                        CodeTaxe = data.CodeTaxe
                    },
                    infosFormule = new InfosFormuleRegularisation
                    {
                        NbAnnees = data.NbAnnees,
                        PrcSeuilSP = data.PrcSeuilSP.ToString(),
                        TauxAppel = data.TauxAppel.ToString() == "0" ? string.Empty : data.TauxAppel.ToString(),
                        Ristourne = data.Ristourne.ToString(),
                        RistourneAnticipe = data.RistourneAnticipee.ToString(),
                        PrcCotisationsRetenues = data.PrcCotisationsRetenues.ToString()
                    },
                    donneesRegul = new
                    {
                        ChargementSinistres = data.ChargementSinistres,
                        CotisationPeriode = data.CotisationPeriode,
                        LibelleMontant = "Montant calculé",//data.LibelleMontant ?? "Montant dû",
                        SigneMontant = data.MontantCalcule < 0 ? "-" : string.Empty,
                        MontantCalcule = data.MontantCalcule,
                        MontantAffiche = Math.Abs(data.MontantCalcule),
                        CotisationsRetenues = data.CotisationsRetenues,
                        PrcCotisationsRetenues = data.PrcCotisationsRetenues,
                        MontantRistourneAnticipee = data.MontantRistourneAnticipee,
                        Ristourne = data.Ristourne.ToString(),
                        RistourneAnticipee = data.RistourneAnticipee.ToString(),
                        IndemnitesFrais = data.IndemnitesFrais,
                        Recours = data.Recours,
                        Provisions = data.Provisions,
                        Previsions = data.Previsions,
                        ReportChargesTrouve = data.ReportChargesTrouve,
                        ReportChargesRetenu = data.ReportChargesRetenu,
                        ReportChargeDateSituation = data.ReportChargeDateSituation == 0 ? "" : AlbConvert.ConvertIntToDate(data.ReportChargeDateSituation).Value.ToShortDateString(),
                        ReportChargesNouveau = data.ReportChargesNouveau,
                        Etat = data.Etat.Trim().Equals("V"),
                        ReguleMode = context.Mode.ToString(),
                        IdContrat = data.IdContrat,
                    }
                }
            };

            return test;
        }

        [HttpPost]
        public JsonResult GetCheckListRisques(RegularisationContext context)
        {
            PresetContext(ref context);
            List<SaisieRisqueInfoDto> list = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                list = client.Channel.GetListSaisieRisqueRegulatisation(context);
            }
            bool isModePB = context.Mode == RegularisationMode.PB;
            return new JsonResult
            {
                Data = new
                {
                    checkListRisques = new
                    {
                        title = "Liste des risques régularisables " + context.Mode.ToString() + " sur le contrat",
                        isPBMode = isModePB,
                        list = list.Select(e => new
                        {
                            isProcessed = e.IsProcessed,
                            label = e.Libelle,
                            cible = e.Cible,
                            dateDebut = e.DateDebutRsq,
                            dateFin = e.DateFinRsq,
                            nbAnnees = e.NbYears,
                            tauxAppel = e.TauxAppel,
                            cotisationRetenue = e.CotisationRetenue,
                            seuilSP = e.SeuilSp,
                            ristourne = e.Ristourne,
                            rsqId = e.CodeRsq,
                            //isPBMode = isModePB
                            tauxMaxi = Math.Round(e.TauxMaxi, 4),
                            primeMaxi = e.PrimeMaxi
                        })
                    }
                }
            };
        }

        [HttpPost]
        public JsonResult GetContratBandeau(IdContratDto contrat)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                model.BandeauContrat = client.Channel.GetBandeauContratInfo(contrat.CodeOffre, contrat.Version.ToString(), contrat.Type);
            }

            return new JsonResult
            {
                Data = new
                {
                    infosContrat = new
                    {
                        contrat.CodeOffre,
                        Version = contrat.Version.ToString(),
                        contrat.Type,
                        DisplayTypeContrat = (!string.IsNullOrEmpty(contrat.LibTypeContrat) ? string.Format("{0} - {1}", contrat.TypeContrat, contrat.LibTypeContrat) : model.BandeauContrat.TypeContrat),
                        Identification = model.BandeauContrat.Identification,
                        Assure = model.BandeauContrat.Assure,
                        Souscripteur = model.BandeauContrat.Souscripteur,
                        DateDebutEffet = (!string.IsNullOrEmpty(model.BandeauContrat.DateDebutEffet)) ? DateTime.Parse(model.BandeauContrat.DateDebutEffet).ToString("dd/MM/yyyy") : string.Empty,
                        DateFinEffet = (!string.IsNullOrEmpty(model.BandeauContrat.DateFinEffet)) ? DateTime.Parse(model.BandeauContrat.DateFinEffet).ToString("dd/MM/yyyy") : string.Empty,
                        Periodicite = model.BandeauContrat.Periodicite,
                        Echeance = (!string.IsNullOrEmpty(model.BandeauContrat.Echeance)) ? DateTime.Parse(model.BandeauContrat.Echeance).ToString("dd/MM/yyyy") : string.Empty,
                        NatureContrat = model.BandeauContrat.NatureContrat,
                        Courtier = model.BandeauContrat.Courtier,
                        RetourPiece = model.BandeauContrat.RetourPiece,
                        Observation = model.BandeauContrat.Observation,
                        Gestionnaire = model.BandeauContrat.Gestionnaire,
                        ContratMere = model.BandeauContrat.ContratMere,
                        IsLightVersion = true,
                        LblDebutEffet = (!string.IsNullOrEmpty(model.BandeauContrat.DateDebutEffet)) ? DateTime.Parse(model.BandeauContrat.DateDebutEffet).ToString("dd/MM/yyyy") : HttpUtility.HtmlEncode(AlbConstantesMetiers.DateVide),
                        LblFinEffet = (!string.IsNullOrEmpty(model.BandeauContrat.DateFinEffet)) ? DateTime.Parse(model.BandeauContrat.DateFinEffet).ToString("dd/MM/yyyy") : HttpUtility.HtmlEncode(AlbConstantesMetiers.DateVide),
                        //Type = context.Mode.GetAttributeOfType<DisplayAttribute>().Name,
                        //RegimeTaxe = context.RegimeTaxe,

                    }
                }
            };
        }

        [ErrorHandler]

        public ActionResult CalculGarantiesRC(string id)
        {
            RegularisationContext context = RebuildContextForRCFR(id);
            InitContentData(context, "Saisie régularisation");
            return View(model);
        }

        [ErrorHandler]
        [HttpPost]
        [AlbVerifLockedOffer("context")]
        public JsonResult GetHeaderGarantiesRC(RegularisationContext context)
        {
            SaisieGarantieInfosDto infosGlobal;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                infosGlobal = client.Channel.GetGarantiesRCFRHeader(context);
            }

            return new JsonResult
            {
                Data = new { infosGlobal }
            };
        }

        [ErrorHandler]
        [HttpPost]
        [AlbVerifLockedOffer("context")]
        public JsonResult GetGarantiesRC(RegularisationContext context)
        {
            List<ParametreDto> unites;
            List<ParametreDto> codesTaxes;
            ListeGarantiesRCDto garantiesRC;
            bool isSimplifiedRegule;

            //PresetContext(ref context);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                unites = client.Channel.GetAvailableUnites();
                codesTaxes = client.Channel.GetAvailableCodeTaxes();
                garantiesRC = client.Channel.GetGarantiesRCGroup(context);
                isSimplifiedRegule = client.Channel.IsSimplifiedReguleFlow(context);
            }

            FormatGarantiesRC(unites, codesTaxes, garantiesRC);

            return new JsonResult
            {
                Data = new
                {
                    IsSimplifiedRegule = isSimplifiedRegule,
                    garantiesRC.Garanties,
                    garantiesRC.TotalAmount,
                    ListeUnites = unites.Select(x => new { x.Code, Label = $"{x.Code} - {x.Libelle}" }),
                    ListeCodesTaxes = codesTaxes.Select(x => new { x.Code, Label = $"{x.Code} - {x.Libelle}" }),
                    garantiesRC.FirstAccess
                }
            };
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult ComputeGarantiesRC(RegularisationContext context, ListeGarantiesRCDto list)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                return new JsonResult
                {
                    Data = client.Channel.ComputeGarantiesRC(context, list)
                };
            }
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult ValidateGarantiesRC(RegularisationContext context, ListeGarantiesRCDto list)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                return new JsonResult
                {
                    Data = client.Channel.ValidateGarantiesRC(context, list)
                };
            }
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult GetRisqueApplique(IdContratDto contrat, int numeroAvt, string codeFormule)
        {
            RisqueDto risqueApplique;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                risqueApplique = client.Channel.GetAppliqueRegule(
                    contrat.CodeOffre,
                    contrat.Version.ToString(),
                    contrat.Type,
                    numeroAvt.ToString(),
                    codeFormule);
            }

            return new JsonResult
            {
                Data = new
                {
                    title = $"Risque {risqueApplique.Code} - {risqueApplique.Designation}",
                    objects = risqueApplique.Objets.Select(o => new { label = $"Objet {o.Code} - {o.Designation}" }).ToArray()
                }
            };
        }

        [ErrorHandler]
        [HttpPost]
        [AlbVerifLockedOffer("context")]
        public JsonResult NextStep(RegularisationContext context)
        {
            PresetContext(ref context);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                context = client.Channel.ValidateStepAndGetNext(context);
            }

            bool? checkOk = CheckStepErrors(context.Error);
            if (checkOk == false)
            {
                throw new AlbFoncException(context.Error.Label, true, true, true);
            }
            else if (checkOk == null)
            {
                throw new AlbException(new Exception(context.Error.Label));
            }

            return new JsonResult
            {
                Data = context
            };
        }

        [ErrorHandler]
        [HttpPost]
        [AlbVerifLockedOffer("context")]
        public JsonResult ReachStep(RegularisationContext context, RegularisationStep stepToRich)
        {
            PresetContext(ref context);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                context = client.Channel.ValidateStepAndRichOther(context, stepToRich);
            }

            bool? checkOk = CheckStepErrors(context.Error);
            if (checkOk == false)
            {
                throw new AlbFoncException(context.Error.Label, true, true, true);
            }
            else if (checkOk == null)
            {
                throw new AlbException(new Exception(context.Error.Label));
            }

            return new JsonResult
            {
                Data = context
            };
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult PreviousStep(RegularisationContext context)
        {
            PresetContext(ref context);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                context = serviceContext.GetPreviousStep(context);
            }

            return new JsonResult
            {
                Data = context
            };
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult RouteFromAjaxCall(RegularisationContext context)
        {
            if (context.IsSaveAndQuit)
            {
                string guid = context.KeyValues.FirstOrDefault(x => x.Contains(PageParamContext.TabGuidKey));
                if (guid.IsEmptyOrNull())
                {
                    return RedirectToAction("Index", "RechercheSaisie");
                }
                guid = guid.Split(new[] { PageParamContext.TabGuidKey }, StringSplitOptions.None).Skip(1).FirstOrDefault();
                return RedirectToAction("AutoUnlock", "Redirection", new { id = guid });
            }
            else
            {

                PresetContext(ref context);
                return RouteToStep(context);
            }
        }

        [ErrorHandler]
        [HttpPost]
        public ContentResult EnsureContext(RegularisationContext context)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                context = client.Channel.EnsureContext(context);
            }

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(context),
                ContentType = "application/json"
            };
        }

        [ErrorHandler]
        [HttpPost]
        public ContentResult Compute(RegularisationContext context, RegularisationComputeData figures)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { message = serviceContext.ComputeRegularisation(context, figures) }),
                    ContentType = "application/json"
                };
            }
        }

        [ErrorHandler]
        [HttpPost]
        public ContentResult ComputeTR(RegularisationContext context, RegularisationComputeData figures)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                var serviceContext = client.Channel;
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { message = serviceContext.ComputeRegularisationTR(context, figures) }),
                    ContentType = "application/json"
                };
            }
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult ComputeCotisations(RegularisationComputeData figures)
        {
            if (figures.ReguleMode != RegularisationMode.BNS)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
                {
                    figures = client.Channel.ComputeCotisations(figures);
                }
            }


            return new JsonResult
            {
                Data = figures
            };
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult CancelRegularisationRisque(long rgId, int rsqId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
            {
                client.Channel.CancelRegularisationRisque(rgId, rsqId);
            }

            return new JsonResult
            {
                Data = true
            };
        }

        protected override ModeleNavigationArbre GetNavigationArbreRegule(MetaModelsBase contentData, string etape)
        {
            contentData.NavigationArbre = new ModeleNavigationArbre();
            IdContratDto contrat = (contentData as ModeleRegularisationPage)?.Context?.IdContrat;
            if (contrat == null && contentData.Contrat != null && contentData.Contrat.CodeContrat != null)
            {
                contrat = new IdContratDto
                {
                    CodeOffre = contentData.Contrat.CodeContrat,
                    Version = (int)contentData.Contrat.VersionContrat,
                    Type = contentData.Contrat.Type
                };
            }

            var folder = new Folder(contrat.CodeOffre, contrat.Version, contrat.Type[0])
            {
                NumeroAvenant = int.TryParse(contentData.NumAvenantPage, out int i) && i > 0 ? i : default
            };
            contentData.NavigationArbre = BuildNavigationArbre(folder, contentData);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaire>())
            {
                if (contentData.NumAvenantPage.ParseInt().Value > 0)
                {
                    var alertesAvenant = client.Channel.GetListAlertesAvenant(new AffaireId
                    {
                        CodeAffaire = contentData.Contrat.CodeContrat,
                        IsHisto = contentData.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique,
                        NumeroAliment = (int)contentData.Contrat.VersionContrat,
                        TypeAffaire = AffaireType.Contrat,
                        NumeroAvenant = contentData.NumAvenantPage.ParseInt().Value
                    });
                    contentData.NavigationArbre.AlertesAvenant = CreationAvenantController.GetInfoAlertes(new AvenantDto { Alertes = alertesAvenant });
                }
            }
            contentData.NavigationArbre.Etape = etape;
            contentData.NavigationArbre.ModeNavig = contentData.ModeNavig;
            contentData.NavigationArbre.IsReadOnly = contentData.IsReadOnly;
            contentData.NavigationArbre.ScreenType = contentData.ScreenType;
            contentData.NavigationArbre.IsValidation = contentData.IsValidation;

            var data = contentData as ModeleRegularisationPage;
            RegularisationNavigator.Initialize(contentData.NavigationArbre, data.Context);

            return contentData.NavigationArbre;
        }

        private void PresetContext(ref RegularisationContext context)
        {
            if (context != null)
            {
                if (context.User.IsEmptyOrNull())
                {
                    context.User = GetUser();
                }

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>())
                {
                    context = client.Channel.EnsureContext(context);
                }

                if (context.KeyValues?.Any() == true)
                {
                    var paramVals = context.KeyValues.Last().ToParamDictionary();
                    string key = AlbParameterName.AVNMODE.ToString();
                    if (!paramVals.ContainsKey(key))
                    {
                        paramVals.InsetSecondToLast(key, context.AccessMode.ToString());
                    }
                    else if (paramVals[key] != context.AccessMode.ToString())
                    {
                        paramVals[key] = context.AccessMode.ToString();
                        context.KeyValues[context.KeyValues.Length - 1] = paramVals.RebuildAddParamString();
                    }
                }
            }
        }

        private void InitContentData(RegularisationContext context, string title)
        {
            PresetContext(ref context);
            string id = String.Join("_", context.KeyValues);
            this.model.PageTitle = title;
            string outId = InitializeParams(id);
            if (context.ModeleAvtRegul == null)
            {
                context.ModeleAvtRegul = new AvenantRegularisationDto
                {
                    TypeAvt = this.model.ActeGestion,
                    NumAvt = long.Parse(this.model.NumAvenantPage),
                    NumInterneAvt = long.Parse(this.model.NumAvenantPage)
                };
            }
            this.model.Context = context;
            LoadGlobalData(id);
        }

        private bool? CheckStepErrors(AlbErrorDto error)
        {
            if (error == null) return true;

            if (error.Code == "NORISKREGUL" || error.Code == "NOGARREGUL" || error.Code == "ERR_HISTO_REGUL")
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        private void LoadGlobalData(string id)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var infosBase = serviceContext.LoadInfosBase(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenantPage, model.ModeNavig);
                model.Contrat = new ContratDto()
                {
                    CodeContrat = infosBase.CodeOffre,
                    VersionContrat = Convert.ToInt64(infosBase.Version),
                    Type = infosBase.Type,
                    Branche = infosBase.Branche.Code,
                    BrancheLib = infosBase.Branche.Nom,
                    Cible = infosBase.Branche.Cible.Code,
                    CibleLib = infosBase.Branche.Cible.Nom,
                    CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                    Descriptif = infosBase.Descriptif,
                    CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                    NomInterlocuteur = infosBase.CabinetGestionnaire.Inspecteur,
                    CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                    NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                    PeriodiciteCode = infosBase.Periodicite,
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                };
            }

            SetContentData();
            SetBandeauNavigation(id);
        }

        private void SetContentData()
        {
            ContratDto contrat = model.Contrat;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetInfoRegulPage(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, model.NumAvenantPage);
                if (result != null)
                {
                    contrat.DateEffetAnnee = result.DateEffetAnnee;
                    contrat.DateEffetMois = result.DateEffetMois;
                    contrat.DateEffetJour = result.DateEffetJour;
                    contrat.FinEffetAnnee = result.FinEffetAnnee;
                    contrat.FinEffetMois = result.FinEffetMois;
                    contrat.FinEffetJour = result.FinEffetJour;
                    contrat.PeriodiciteCode = result.PeriodiciteCode;
                    contrat.PeriodiciteNom = result.PeriodiciteNom;
                    contrat.LibelleNatureContrat = result.LibelleNatureContrat;
                    contrat.PartAlbingia = result.PartAlbingia;
                    contrat.ProchaineEchAnnee = result.ProchaineEchAnnee;
                    contrat.ProchaineEchMois = result.ProchaineEchMois;
                    contrat.ProchaineEchJour = result.ProchaineEchJour;
                    contrat.CodeRegime = result.CodeRegime;
                    contrat.LibelleRegime = result.LibelleRegime;
                    contrat.Devise = result.Devise;
                    contrat.LibelleDevise = result.LibelleDevise;
                    contrat.CourtierGestionnaire = result.CourtierGestionnaire;
                    contrat.CourtierApporteur = result.CourtierApporteur;
                    contrat.NomCourtierGest = result.NomCourtierGest;
                    contrat.NomCourtierAppo = result.NomCourtierAppo;
                    contrat.SouscripteurCode = result.SouscripteurCode;
                    contrat.SouscripteurNom = result.SouscripteurNom;
                    contrat.GestionnaireCode = result.GestionnaireCode;
                    contrat.GestionnaireNom = result.GestionnaireNom;
                }


                model.Contrat.TypePolice = !string.IsNullOrEmpty(contrat.TypePolice) ? contrat.TypePolice : "S";
                if (contrat.DateEffetAnnee != 0 && contrat.DateEffetMois != 0 && contrat.DateEffetJour != 0)
                {
                    model.EffetGaranties = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour);
                }
                else model.EffetGaranties = null;
                if (contrat.FinEffetAnnee != 0 && contrat.FinEffetMois != 0 && contrat.FinEffetJour != 0)
                {
                    model.FinEffet = new DateTime(contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour);
                    model.FinEffetHeure = AlbConvert.ConvertIntToTimeMinute(contrat.FinEffetHeure);
                }
                else if (contrat.DureeGarantie > 0)
                {
                    model.FinEffet = AlbConvert.GetFinPeriode(model.EffetGaranties, contrat.DureeGarantie, contrat.UniteDeTemps);
                    model.FinEffetHeure = new TimeSpan(23, 59, 0);
                }
                else model.FinEffet = null;

                if (contrat.ProchaineEchAnnee != 0 && contrat.ProchaineEchMois != 0 && contrat.ProchaineEchJour != 0)
                {
                    model.Echeance = new DateTime(contrat.ProchaineEchAnnee, contrat.ProchaineEchMois, contrat.ProchaineEchJour);
                }

                var regule = GetInfoRegule(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage);
                model.Regularisations = regule != null && regule.Regularisations != null && regule.Regularisations.Any() ? regule.Regularisations : new List<ModeleLigneRegularisation>();
                model.Alertes = GetInfoAlertes(regule.Alertes);
                ParametreDto typeContrat = regule != null && regule.TypesContrat != null && regule.TypesContrat.Any() ? regule.TypesContrat.Find(el => el.Code == model.Contrat.TypePolice) : null;
                model.LibTypeContrat = typeContrat != null ? typeContrat.Descriptif : string.Empty;
            }
        }

        private void SetBandeauNavigation(string id)
        {
            var paramDico = this.model.AddParamValue.ToParamDictionary();
            this.model.AvnMode = paramDico.TryGetValue(AlbParameterName.AVNMODE.ToString(), out var mode) ? mode : AccessMode.CONSULT.ToString();
            this.model.IsReadOnly = this.model.AvnMode == AccessMode.CONSULT.ToString();
            this.model.AfficherBandeau = true;

            this.model.AfficherNavigation = this.model.AfficherBandeau;
            if (paramDico[AlbParameterName.REGULMOD.ToString()] == RegularisationMode.PB.ToString())
            {
                this.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
            }
            else if (paramDico[AlbParameterName.REGULMOD.ToString()] == RegularisationMode.BNS.ToString())
            {
                this.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBNS;
            }
            else if (paramDico[AlbParameterName.REGULMOD.ToString()] == RegularisationMode.Burner.ToString().ToUpper())
            {
                this.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER;
            }
            else
            {
                this.model.ScreenType = paramDico[AlbParameterName.AVNTYPE.ToString()] == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF : AlbConstantesMetiers.SCREEN_TYPE_REGUL;
            }

            this.model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            this.model.Bandeau.StyleBandeau = paramDico[AlbParameterName.AVNTYPE.ToString()] == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF : AlbConstantesMetiers.SCREEN_TYPE_REGUL;

            // Gestion des Etapes
            this.model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_REGULE,
                IdOffre = this.model.Context.IdContrat.CodeOffre,
                Version = int.Parse(this.model.Context.IdContrat.Version.ToString()),
            };

            this.model.NavigationArbre = GetNavigationArbreRegule(this.model, "Regule");
            if (this.model.NavigationArbre != null)
            {
                this.model.NavigationArbre.IsRegule = paramDico[AlbParameterName.AVNTYPE.ToString()] == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || paramDico[AlbParameterName.AVNTYPE.ToString()] == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                this.model.NavigationArbre.IsReadOnly = this.model.IsReadOnly;
            }
        }

        private ModeleRegularisation GetInfoRegule(string codeContrat, string version, string type, string codeAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetInfoRegularisation(codeContrat, version, type, codeAvn, GetUser());

                if (result != null)
                {
                    ModeleRegularisation regularisation = (ModeleRegularisation)result;
                    return regularisation;
                }
                return null;
            }
        }

        private List<ModeleAvenantAlerte> GetInfoAlertes(List<ModeleAvenantAlerte> Alertes)
        {
            GetLienAlerte(Alertes);
            return Alertes != null && Alertes.Any() ? Alertes : new List<ModeleAvenantAlerte>();
        }

        private static void GetLienAlerte(List<ModeleAvenantAlerte> Alertes)
        {
            if (Alertes != null && Alertes.Any())
            {
                foreach (ModeleAvenantAlerte elm in Alertes)
                {
                    switch (elm.TypeAlerte)
                    {
                        case AlbOpConstants.SUSPEN:
                            elm.LienMessage = "Visu des suspensions";
                            break;
                        case AlbOpConstants.QUITT:
                            elm.LienMessage = "Visu des quittances";
                            break;
                        default:
                            elm.LienMessage = "Voir";
                            break;
                    }
                }
            }
        }

        private RedirectToRouteResult RouteToStep(RegularisationContext context)
        {
            switch (context.Step)
            {
                case RegularisationStep.ChoixPeriodeCourtier:
                    if (context.Mode == RegularisationMode.PB)
                    {
                        return RedirectToAction("Step1_ChoixPeriode", "CreationPB", new { id = String.Join("_", context.KeyValues) });
                    }
                    else
                    {
                        return RedirectToAction("Step1_ChoixPeriode", "CreationRegularisation", new { id = String.Join("_", context.KeyValues) });
                    }
                case RegularisationStep.ChoixRisques:
                    return RouteToChoixRisques(context);
                case RegularisationStep.Regularisation:
                    return RouteToRegularisation(context);
                case RegularisationStep.Cotisations:
                    return RedirectToAction("Index", "Quittance", new { id = String.Join("_", context.KeyValues) });
                default:
                    break;
            }

            return null;
        }

        private RedirectToRouteResult RouteToChoixRisques(RegularisationContext context)
        {
            switch (context.Mode)
            {
                case RegularisationMode.Standard:
                    break;
                case RegularisationMode.Coassurance:
                    break;
                case RegularisationMode.PB:
                case RegularisationMode.BNS:
                case RegularisationMode.Burner:
                    return RedirectToAction("CheckListRisques", new { submit = true });
                default:
                    break;
            }

            return null;
        }

        private RedirectToRouteResult RouteToRegularisation(RegularisationContext context)
        {
            switch (context.Mode)
            {
                case RegularisationMode.Standard:
                    break;
                case RegularisationMode.Coassurance:
                    break;
                case RegularisationMode.PB:
                    //return RedirectToAction("CalculPB" + context.Scope.ToString(), new { context = JsonConvert.SerializeObject(context) });
                    return context.IdContrat.ToString().Substring(0, 2) == "TR" ? RedirectToAction("CalculPBTR" + context.Scope.ToString(), new { submit = true }) : RedirectToAction("CalculPB" + context.Scope.ToString(), new { submit = true });
                case RegularisationMode.BNS:
                    return RedirectToAction("CalculBNS" + context.Scope.ToString(), new { submit = true });
                case RegularisationMode.Burner:
                    return RedirectToAction("CalculBurner" + context.Scope.ToString(), new { submit = true });
                default:
                    break;
            }

            return null;
        }

        private static RegularisationContext RebuildContextForRCFR(string id)
        {
            var context = new RegularisationContext
            {
                KeyValues = HttpUtility.UrlDecode(id).Split('_'),
                Mode = RegularisationMode.Standard,
                Scope = RegularisationScope.Garantie,
                Step = RegularisationStep.Regularisation
            };

            var paramValues = context.KeyValues.Last().ToParamDictionary();
            context.RgId = long.Parse(paramValues[AlbParameterName.REGULEID.ToString()]);
            context.RsqId = long.Parse(paramValues[AlbParameterName.RSQID.ToString()]);
            context.RgGrId = long.Parse(paramValues[AlbParameterName.REGULGARID.ToString()]);
            context.RgHisto = paramValues[AlbParameterName.REGULAVN.ToString()][0];
            context.Type = paramValues[AlbParameterName.REGULTYP.ToString()];
            context.LotId = long.Parse(paramValues[AlbParameterName.LOTID.ToString()]);
            context.IsReadOnlyMode = paramValues.TryGetValue(AlbParameterName.AVNMODE.ToString(), out var mode) ? mode == AccessMode.CONSULT.ToString() : true;

            context.IdContrat = new IdContratDto
            {
                CodeOffre = context.KeyValues[0],
                Version = Int32.Parse(context.KeyValues[1]),
                Type = context.KeyValues[2]
            };

            return context;
        }

        private static void FormatGarantiesRC(List<ParametreDto> unites, List<ParametreDto> codesTaxes, ListeGarantiesRCDto garantiesRC)
        {
            garantiesRC.Garanties.ForEach(g =>
            {
                if (g.Definitif.BasicValues.CodeTaxes.Code.IsEmptyOrNull())
                {
                    g.Definitif.BasicValues.CodeTaxes.Code = string.Empty;
                    g.Definitif.BasicValues.CodeTaxes.Label = string.Empty;
                }
                else
                {
                    var c = codesTaxes.First(x => x.Code == g.Definitif.BasicValues.CodeTaxes.Code);
                    g.Definitif.BasicValues.CodeTaxes = new CodeTaxesDto() { Code = c.Code, Label = $"{c.Code} - {c.Libelle}" };
                }

                if (g.Previsionnel.BasicValues.CodeTaxes.Code.IsEmptyOrNull())
                {
                    g.Previsionnel.BasicValues.CodeTaxes.Code = string.Empty;
                    g.Previsionnel.BasicValues.CodeTaxes.Label = string.Empty;
                }
                else
                {
                    var c = codesTaxes.First(x => x.Code == g.Previsionnel.BasicValues.CodeTaxes.Code);
                    g.Previsionnel.BasicValues.CodeTaxes = new CodeTaxesDto() { Code = c.Code, Label = $"{c.Code} - {c.Libelle}" };
                }

                if (g.Definitif.BasicValues.Unite.Code.IsEmptyOrNull())
                {
                    g.Definitif.BasicValues.Unite.Code = string.Empty;
                    g.Definitif.BasicValues.Unite.Label = string.Empty;
                }
                else
                {
                    var u = unites.First(x => x.Code == g.Definitif.BasicValues.Unite.Code);
                    g.Definitif.BasicValues.Unite = new UniteTauxMontantDto() { Code = u.Code, Label = $"{u.Code} - {u.Libelle}" };
                }

                if (g.Previsionnel.BasicValues.Unite.Code.IsEmptyOrNull())
                {
                    g.Previsionnel.BasicValues.Unite.Code = string.Empty;
                    g.Previsionnel.BasicValues.Unite.Label = string.Empty;
                }
                else
                {
                    var u = unites.First(x => x.Code == g.Previsionnel.BasicValues.Unite.Code);
                    g.Previsionnel.BasicValues.Unite = new UniteTauxMontantDto() { Code = u.Code, Label = $"{u.Code} - {u.Libelle}" };
                }
            });
        }
    }
}
