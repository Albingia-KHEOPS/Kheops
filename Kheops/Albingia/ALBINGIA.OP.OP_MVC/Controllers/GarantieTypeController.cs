using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieType;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.GarantieModele;
using OPServiceContract.IClausesRisquesGaranties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Albingia.Kheops.OP.Application.Port.Driver;
using OPServiceContract.IAdministration;
using Services = Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GarantieTypeController : ControllersBase<ModeleGarantieTypePage>
    {
        //readonly IReferentielPort refService;

        //public GarantieTypeController(IReferentielPort refService)
        //{
        //    this.refService = refService;
        //}

        #region publique

        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index(string codeModele)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                model.PageTitle = "Garantie Type";
                model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
                model.RechercheGarantieType.CodeModele = codeModele;
                model.IsModifiable = !client.Channel.ExistGarantieModeleDansContrat(codeModele);
                DisplayBandeau();
            }
            return View(model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string codeModele)
        {
            List<ModeleGarantieType> ToReturn = new List<ModeleGarantieType>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                List<GarantieTypeDto> result = client.Channel.GarantieTypeGet(codeModele).ToList();

                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((ModeleGarantieType)m));
            }

            return PartialView("ListGarantieType", ToReturn);
        }

        [ErrorHandler]
        public ActionResult ConsultGarantieType(int seq, string codeModele, int ord, bool readOnly, bool isNew)
        {
            ModeleGarantieType toReturn;
            using (var clientgar = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            using (var clientref = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>())
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            using (var clientexp = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var paramGaranties = clientgar.Channel;
                var paramReferentiel = clientref.Channel;
                var paramExpression = clientexp.Channel;

                if (isNew)
                {
                    // si seq = 0 donc nouvelle garantie sinon sous-garantie
                    if (seq == 0)
                    {
                        toReturn = new ModeleGarantieType()
                        {
                            NumeroSeq = 0,
                            NumeroSeq1 = 0,
                            NumeroSeqM = 0,
                            Niveau = 1,
                            CodeModele = codeModele,
                            Ordre = ord + 5,
                            Tri = "000000000000",
                        };
                    }
                    else
                    {
                        var garMere = (ModeleGarantieType)client.Channel.GarantieTypeInfoGet(seq);
                        toReturn = new ModeleGarantieType()
                        {
                            NumeroSeq = 0,
                            NumeroSeq1 = garMere.Niveau == 1 ? garMere.NumeroSeq : garMere.NumeroSeq1,
                            NumeroSeqM = garMere.NumeroSeq,
                            Niveau = garMere.Niveau + 1,
                            CodeModele = garMere.CodeModele,
                            Ordre = ord + 5,
                            Tri = "000000000000",
                        };
                    }
                }
                else
                {
                    toReturn = (ModeleGarantieType)client.Channel.GarantieTypeInfoGet(seq);
                }

                toReturn.ReadOnly = readOnly;
                toReturn.IsNew = isNew;

                // Remplissage des listes déroulantes
                toReturn.ListGarantie = paramGaranties.GetGaranties(string.Empty, string.Empty, string.Empty, true);
                toReturn.ListCaractere = paramReferentiel.GetCaracteresGarantie().ToList();
                toReturn.ListNature = paramReferentiel.GetNaturesGarantie().ToList();
                toReturn.ListTypeControleDate = paramReferentiel.GetTypesControleDate().ToList();

                // Liste unité et base pour Assiette
                toReturn.ListBaseAssiette = paramReferentiel.GetBasesCapitaux().ToList();
                toReturn.ListBaseAssiette.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BaseCapitaux() { Code = "", Libelle = "" });
                toReturn.ListUniteAssiette = paramReferentiel.GetUnitesCapitaux().ToList();
                toReturn.ListUniteAssiette.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.UniteCapitaux() { Code = "", Libelle = "" });
                // Liste unité et base pour Prime
                toReturn.ListBasePrime = paramReferentiel.GetBasesPrime().ToList();
                toReturn.ListBasePrime.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BasePrime() { Code = "", Libelle = "" });
                toReturn.ListUnitePrime = paramReferentiel.GetUnitesPrimes().ToList();
                toReturn.ListUnitePrime.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.UnitePrime() { Code = "", Libelle = "" });
                // Liste unité et base pour LCI
                toReturn.ListBaseLCI = toReturn.ListLCI.FirstOrDefault(x => x.Type == ((int)TypeDeValeur.LCI).ToString()).Unite != "CPX" ? 
                    paramReferentiel.GetBasesLCI().ToList() :
                    paramExpression.LoadListExprComplexe("LCI").Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseLCI() { Code = x.Code, Libelle = x.Descriptif }).ToList();
                toReturn.ListBaseLCI.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BaseLCI() { Code = "", Libelle = "" });
                toReturn.ListUniteLCI = paramReferentiel.GetUnitesLCI().ToList();
                toReturn.ListUniteLCI.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.UniteLCI() { Code = "", Libelle = "" });
                // Liste unité et base pour Franchise
                toReturn.ListBaseFranchise = toReturn.ListLCI.FirstOrDefault(x => x.Type == ((int)TypeDeValeur.Franchise).ToString()).Unite != "CPX" ?
                    paramReferentiel.GetBasesFranchise().ToList() :
                    paramExpression.LoadListExprComplexe("Franchise").Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseFranchise() { Code = x.Code, Libelle = x.Descriptif }).ToList(); 
                toReturn.ListBaseFranchise.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BaseFranchise() { Code = "", Libelle = "" });
                toReturn.ListUniteFranchise = paramReferentiel.GetUnitesFranchise().ToList();
                toReturn.ListUniteFranchise.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.UniteFranchise() { Code = "", Libelle = "" });

                // Liste base pour Franchise min et max
                toReturn.ListBaseFranchiseMin = toReturn.ListLCI.FirstOrDefault(x => x.Type == ((int)TypeDeValeur.FranchiseMin).ToString()).Unite != "CPX" ?
                    paramReferentiel.GetBasesFranchise().ToList() :
                    paramExpression.LoadListExprComplexe("Franchise").Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseFranchise() { Code = x.Code, Libelle = x.Descriptif }).ToList();
                toReturn.ListBaseFranchiseMin.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BaseFranchise() { Code = "", Libelle = "" });
                toReturn.ListBaseFranchiseMax = toReturn.ListLCI.FirstOrDefault(x => x.Type == ((int)TypeDeValeur.FranchiseMax).ToString()).Unite != "CPX" ?
                    paramReferentiel.GetBasesFranchise().ToList() :
                    paramExpression.LoadListExprComplexe("Franchise").Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseFranchise() { Code = x.Code, Libelle = x.Descriptif }).ToList();
                toReturn.ListBaseFranchiseMax.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BaseFranchise() { Code = "", Libelle = "" });







                toReturn.ListAlimentation = paramReferentiel.GetAlimentations().ToList();
                toReturn.ListModeModifiable = paramReferentiel.GetModesModifiables().ToList();
                toReturn.ListModeModifiable.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.ModeModifiable() { Code = "", Libelle = "" });
            }
            return PartialView("EditorTemplates/ConsultGarantieType", toReturn);
        }

        [ErrorHandler]
        public ActionResult ConsultGarantieTypeLien(int seq, bool readOnly)
        {
            ModeleGarantieType toReturn;
            using (var clientgar = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                var paramGaranties = clientgar.Channel;

                toReturn = (ModeleGarantieType)client.Channel.GarantieTypeLienInfoGet(seq);

                toReturn.ReadOnly = readOnly;

                // Remplissage des listes déroulantes
                List<GarantieTypeDto> result = client.Channel.GarantieTypeGetAll().ToList();
                if (result != null && result.Count > 0)
                    result.Where(x => x.NumeroSeq != seq).ToList().ForEach(m => toReturn.ListGarantieType.Add((ModeleGarantieType)m));
            }
            return PartialView("EditorTemplates/ConsultGarantieTypeLien", toReturn);
        }

        [ErrorHandler]
        public JsonResult Enregistrer(ModeleGarantieType garType)
        {
            string toReturn = "";
            switch (garType.Niveau)
            {
                case 1:
                    garType.Tri = garType.Ordre.ToString().PadLeft(3, '0') + garType.Tri.Substring(3);
                    break;
                case 2:
                    garType.Tri = garType.Tri.Substring(0, 3) + garType.Ordre.ToString().PadLeft(3, '0') + garType.Tri.Substring(6);
                    break;
                case 3:
                    garType.Tri = garType.Tri.Substring(0, 6) + garType.Ordre.ToString().PadLeft(3, '0') + garType.Tri.Substring(9);
                    break;
                case 4:
                    garType.Tri = garType.Tri.Substring(0, 9) + garType.Ordre.ToString().PadLeft(3, '0');
                    break;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.EnregistrerGarantieType(ModeleGarantieType.LoadDto(garType), garType.IsNew, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        [ErrorHandler]
        public JsonResult Supprimer(int seq)
        {
            string toReturn = "";
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.SupprimerGarantieType(seq, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        [ErrorHandler]
        public JsonResult AjouterGarantieTypeLien(string type, long seqA, long seqB) {
            string toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.AjouterGarantieTypeLien(type, seqA, seqB, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        [ErrorHandler]
        public JsonResult SupprimerGarantieTypeLien(string type, long seqA, long seqB)
        {
            string toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IParametrageModelesPort>())
            {
                client.Channel.SupprimerGarantieTypeLien(type, seqA, seqB, out toReturn);
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }

        [ErrorHandler]
        public JsonResult GetBase(int type, string unite)
        {
            var toReturn = new List<Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            using (var clientref = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IReferentielPort>())
            {
                var paramReferentiel = clientref.Channel;
                if (unite != "CPX")
                {
                    switch ((TypeDeValeur)type)
                    {
                        case TypeDeValeur.Assiette:
                            toReturn = paramReferentiel.GetBasesCapitaux().Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = x.Code, Libelle = x.Libelle }).ToList();
                            break;
                        case TypeDeValeur.Prime:
                            toReturn = paramReferentiel.GetBasesPrime().Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = x.Code, Libelle = x.Libelle }).ToList();
                            break;
                        case TypeDeValeur.LCI:
                            toReturn = paramReferentiel.GetBasesLCI().Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = x.Code, Libelle = x.Libelle }).ToList();
                            break;
                        case TypeDeValeur.Franchise:
                        case TypeDeValeur.FranchiseMin:
                        case TypeDeValeur.FranchiseMax:
                            toReturn = paramReferentiel.GetBasesFranchise().Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = x.Code, Libelle = x.Libelle }).ToList();
                            break;
                    }
                    toReturn.Insert(0, new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = "", Libelle = "" });
                }
                else
                {
                    switch ((TypeDeValeur)type) {
                        case TypeDeValeur.LCI:
                            toReturn = client.Channel.LoadListExprComplexe("LCI").Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = x.Code, Libelle = x.Descriptif }).ToList();
                            break;
                        case TypeDeValeur.Franchise:
                        case TypeDeValeur.FranchiseMin:
                        case TypeDeValeur.FranchiseMax:
                            toReturn = client.Channel.LoadListExprComplexe("Franchise").Select(x => new Albingia.Kheops.OP.Domain.Referentiel.BaseDeCalcul() { Code = x.Code, Libelle = x.Descriptif }).ToList();
                            break;
                    }
                }
            }
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}