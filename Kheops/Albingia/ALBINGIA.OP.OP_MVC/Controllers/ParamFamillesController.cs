using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFamilles;
using OP.WSAS400.DTO.ParametreFamilles;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamFamillesController : ControllersBase<ModeleParamFamillesPage>
    {
        #region Membres privés
        public static readonly string MODE_EDITION = "U";
        public static readonly string MODE_CREATION = "I";
        public static readonly string MODE_DUPLICATION = "D";
        #endregion
        #region Méthodes Publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index(string id)
        {
            //model.CodeConcept = id;

            if (id != null)
            {
                string[] tId = id.Split('_');
                if (tId.Length == 3)
                {
                    model.CodeConcept = HttpUtility.UrlDecode(tId[0]);
                    model.LibelleConcept = HttpUtility.UrlDecode(tId[1]);
                    model.RestrictionParam = string.Empty;
                    model.AdditionalParam = HttpUtility.UrlDecode(tId[2]);
                }
            }
            else
            {
                model.RestrictionParam = string.Empty;
                model.AdditionalParam = "**";
            }
            if (!string.IsNullOrEmpty(model.CodeConcept))
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var familles = new List<LigneFamille>();
                    var result = voletsBlocsCategoriesClient.GetFamilles(model.CodeConcept, string.Empty, string.Empty, model.AdditionalParam, model.RestrictionParam).ToList();
                    if (result != null && result.Count > 0)
                        result.ForEach(m => familles.Add((LigneFamille)m));
                    foreach (var famille in familles)
                    {
                        famille.AdditionalParam = model.AdditionalParam;
                        famille.RestrictionParam = model.RestrictionParam;
                    }
                    model.ListeFamilles = familles;
                }
            model.PageTitle = "Paramétrages des familles";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }
        [ErrorHandler]
        public ActionResult GetFamilles(string codeConcept, string codeFamille, string descriptionFamille, string additionalParam, string restrictionParam)
        {
            var ToReturn = new List<LigneFamille>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var result = voletsBlocsCategoriesClient.GetFamilles(codeConcept, codeFamille, descriptionFamille, additionalParam, restrictionParam).ToList();
                if (result != null && result.Count > 0)
                    result.ForEach(m => ToReturn.Add((LigneFamille)m));
                foreach (var famille in ToReturn)
                {
                    famille.AdditionalParam = additionalParam;
                    famille.RestrictionParam = restrictionParam;
                }
            }
            return PartialView("ParamListFamilles", ToReturn);
        }
        [ErrorHandler]
        public ActionResult EditFamille(string codeConcept, string libelleConcept, string codeFamille,
            string additionalParam, string restrictionParam, string modeOperation)
        {

            var famille = new Famille();
            if (!string.IsNullOrEmpty(codeFamille))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var result = voletsBlocsCategoriesClient.GetFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
                    if (result != null)
                        famille = (Famille)result;
                    var valeursResult = voletsBlocsCategoriesClient.GetValeursByFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
                    if (valeursResult != null)
                        famille.nbvaleur = valeursResult.Count;
                    else
                        famille.nbvaleur = 0;
                }
            }
            else
            {
                famille.TypeCode = "A";
            }
            famille.ModeOperation = modeOperation;
            famille.CodeConcept = codeConcept;
            famille.LibelleConcept = libelleConcept;
            famille.Longueurs = LoadDDLLongeurs();
            famille.TypesNum1 = LoadDDLTypes();
            famille.NbrDecimals1 = LoadDDLNbrDecimals();
            if (string.IsNullOrEmpty(famille.LibelleCourtNum1)) famille.NbrDecimal1 = -1;
            famille.TypesNum2 = LoadDDLTypes();
            famille.NbrDecimals2 = LoadDDLNbrDecimals();
            if (string.IsNullOrEmpty(famille.LibelleCourtNum2)) famille.NbrDecimal2 = -1;
            famille.Restrictions = LoadDDLRestrictions();
            return PartialView("EditFamille", famille);
        }
        [ErrorHandler]
        public ActionResult EditValeurs(string codeConcept, string libelleConcept, string codeFamille, string additionalParam, string restrictionParam)
        {
            var famille = new Famille();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var valeurs = new List<Valeur>();
                var result = voletsBlocsCategoriesClient.GetFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
                if (result != null)
                {
                    famille = (Famille)result;
                    famille.CodeConcept = codeConcept;
                    famille.LibelleConcept = libelleConcept;
                    famille.Longueurs = LoadDDLLongeurs();
                    var valeursResult = voletsBlocsCategoriesClient.GetValeursByFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
                    if (valeursResult != null && valeursResult.Count > 0)
                        valeursResult.ForEach(m => valeurs.Add((Valeur)m));
                    foreach (var v in valeurs)
                    {
                        v.AdditionalParam = additionalParam;
                        v.RestrictionParam = restrictionParam;
                    }
                    famille.Valeurs = valeurs;
                    famille.nbvaleur = valeurs.Count;
                }
            }
            return PartialView("EditValeurs", famille);
        }
        [ErrorHandler]
        public ActionResult EditerValeur(string codeConcept, string libelleConcept, string codeFamille, string libelleFamille,
            string libelleLongNum1, string typeNum1, string libelleLongNum2, string typeNum2, string libelleLongAlpha1, string libelleLongAlpha2, string codeValeur, string modeOperation, string additionalParam,
            int longueur, string typeCode, string restrictionFamille, string restrictionParam)
        {
            var valeur = new Valeur();
            if (modeOperation != MODE_CREATION)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var result = voletsBlocsCategoriesClient.GetValeur(codeConcept, codeFamille, codeValeur, additionalParam, restrictionParam);
                    if (result != null)
                        valeur = (Valeur)result;
                    valeur.CodeValeur = codeValeur;
                }

            }
            if (modeOperation == MODE_DUPLICATION || modeOperation == MODE_CREATION)
            {
                valeur.Restriction = restrictionFamille;
                valeur.CodeValeur = string.Empty;
            }
            valeur.Longueur = longueur;
            valeur.TypeCode = typeCode;

            valeur.LibelleLongNum1 = libelleLongNum1;
            valeur.TypeNum1 = typeNum1;

            valeur.LibelleLongNum2 = libelleLongNum2;
            valeur.TypeNum2 = typeNum2;

            valeur.LibelleLongAlpha1 = libelleLongAlpha1;
            valeur.LibelleLongAlpha2 = libelleLongAlpha2;
            valeur.CodeConcept = codeConcept;
            valeur.LibelleConcept = libelleConcept;
            valeur.CodeFamille = codeFamille;
            valeur.LibelleFamille = libelleFamille;
            valeur.Restrictions = LoadDDLRestrictions();
            valeur.Filtres = LoadDDLFiltres();
            valeur.ModeOperation = modeOperation;
            return PartialView("EditValeur", valeur);
        }
        [ErrorHandler]
        public ActionResult UpdateValeur(string codeConcept, string codeFamille, string codeValeur, string libelleCourt, string libelleLong,
                             string description1, string description2, string description3, string valeurNum1, string typeNum1, string valeurNum2, string typeNum2,
                              string valeurAlpha1, string valeurAlpha2, string restriction, string codeFiltre, int longueur, string typeCode, string modeOperation, string additionalParam, string restrictionParam, bool acceptNullValue)
        {
            if ((string.IsNullOrEmpty(codeValeur.Trim()) || codeValeur.Length > longueur) && acceptNullValue == false)
                throw new AlbFoncException("Le code n'est pas valide");
            var valeurs = new List<Valeur>();

            if (string.IsNullOrEmpty(libelleLong)) libelleLong = libelleCourt;
            double val1 = 0;
            double val2 = 0;
            if (!string.IsNullOrEmpty(valeurNum1))
            {
                if (typeNum1 == "D")
                    val1 = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(valeurNum1)).Value;
                else val1 = Convert.ToDouble(valeurNum1);

            }
            if (!string.IsNullOrEmpty(valeurNum2))
            {
                if (typeNum2 == "D")
                    val2 = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(valeurNum2)).Value;
                else val2 = Convert.ToDouble(valeurNum2);
            }

            var paramValeurDto = new ParamValeurDto
            {
                CodeConcpet = codeConcept,
                CodeFamille = codeFamille,
                CodeValeur = codeValeur.ToUpper(),
                LibelleValeur = libelleCourt,
                LibelleLongValeur = libelleLong,
                Description1 = description1,
                Description2 = description2,
                Description3 = description3,
                ValeurNum1 = val1,
                ValeurNum2 = val2,
                ValeurAlpha1 = valeurAlpha1,
                ValeurAlpha2 = valeurAlpha2,
                Restriction = restriction,
                CodeFiltre = codeFiltre
            };
            if (modeOperation == MODE_DUPLICATION) modeOperation = MODE_CREATION;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var retourMessage = voletsBlocsCategoriesClient.EnregistrerValeur(modeOperation, paramValeurDto, additionalParam);
                if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
                var valeursResult = voletsBlocsCategoriesClient.GetValeursByFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
                if (valeursResult != null && valeursResult.Count > 0)
                    valeursResult.ForEach(m => valeurs.Add((Valeur)m));
                foreach (var v in valeurs)
                {
                    v.AdditionalParam = additionalParam;
                    v.RestrictionParam = restrictionParam;
                }
            }
            return PartialView("ParamListValeurs", valeurs);
        }

        [ErrorHandler]
        public ActionResult SupprimerValeur(string codeConcept, string codeFamille, string codeValeur, string additionalParam, string restrictionParam)
        {
            var valeurs = new List<Valeur>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var valeur = new ParamValeurDto
                {
                    CodeConcpet = codeConcept,
                    CodeFamille = codeFamille,
                    CodeValeur = codeValeur
                };
                var retourMessage = voletsBlocsCategoriesClient.SupprimerValeur(valeur);
                if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
                var valeursResult = voletsBlocsCategoriesClient.GetValeursByFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
                if (valeursResult != null && valeursResult.Count > 0)
                    valeursResult.ForEach(m => valeurs.Add((Valeur)m));
                foreach (var v in valeurs)
                {
                    v.AdditionalParam = additionalParam;
                    v.RestrictionParam = restrictionParam;
                }
            }
            return PartialView("ParamListValeurs", valeurs);
        }
        [ErrorHandler]
        public ActionResult SupprimerFamille(string codeConcept, string codeFamille, string additionalParam, string restrictionParam)
        {
            var familles = new List<LigneFamille>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var famille = new ParamFamilleDto
                {
                    CodeConcpet = codeConcept,
                    CodeFamille = codeFamille
                };
                var retourMessage = voletsBlocsCategoriesClient.SupprimerFamille(famille);
                if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);

                var result = voletsBlocsCategoriesClient.GetFamilles(codeConcept, string.Empty, string.Empty, additionalParam, restrictionParam).ToList();
                if (result != null && result.Count > 0)
                    result.ForEach(m => familles.Add((LigneFamille)m));
                foreach (var f in familles)
                {
                    f.AdditionalParam = additionalParam;
                    f.RestrictionParam = restrictionParam;
                }
            }
            return PartialView("ParamListFamilles", familles);
        }


        [ErrorHandler]
        public ActionResult UpdateFamille(string codeConcept, string codeFamille, string libelleFamille, short longueur, string typeCode, bool acceptNullValue,
            string libelleCourtNum1, string libelleLongNum1, string typeNum1, short nbrDecimal1,
            string libelleCourtNum2, string libelleLongNum2, string typeNum2, short nbrDecimal2,
            string libelleCourtAlpha1, string libelleLongAlpha1, string libelleCourtAlpha2, string libelleLongAlpha2,
            string restriction, string modeOperation, string additionalParam, string restrictionParam)
        {
            var familles = new List<LigneFamille>();
            if (string.IsNullOrEmpty(codeFamille.Trim()))
                throw new AlbFoncException("Le code n'est pas valide ");
            else if (codeFamille.Length > 5)
                throw new AlbFoncException("Le code ne peut pas dépasser 5 caractères");
            //Num1
            if (string.IsNullOrEmpty(libelleCourtNum1) && !string.IsNullOrEmpty(libelleLongNum1)) libelleCourtNum1 = libelleLongNum1;
            if (string.IsNullOrEmpty(libelleLongNum1) && !string.IsNullOrEmpty(libelleCourtNum1)) libelleLongNum1 = libelleCourtNum1;
            if (!string.IsNullOrEmpty(libelleCourtNum1))
            {
                if (string.IsNullOrEmpty(typeNum1)) throw new AlbFoncException("Le type de Num1 est obligatoire");
                if (nbrDecimal1 == -1) throw new AlbFoncException("Le nombre Déc de Num1 est obligatoire");
            }
            else
            {
                nbrDecimal1 = 0;
                typeNum1 = string.Empty;
            }

            //Num2
            if (string.IsNullOrEmpty(libelleCourtNum2) && !string.IsNullOrEmpty(libelleLongNum2)) libelleCourtNum2 = libelleLongNum2;
            if (string.IsNullOrEmpty(libelleLongNum2) && !string.IsNullOrEmpty(libelleCourtNum2)) libelleLongNum2 = libelleCourtNum2;
            if (!string.IsNullOrEmpty(libelleCourtNum2))
            {
                if (string.IsNullOrEmpty(typeNum2)) throw new AlbFoncException("Le type de Num2 est obligatoire");
                if (nbrDecimal2 == -1) throw new AlbFoncException("Le nombre Déc de Num2 est obligatoire");
            }
            else
            {
                nbrDecimal2 = 0;
                typeNum2 = string.Empty;
            }

            //Alpha 1
            if (string.IsNullOrEmpty(libelleCourtAlpha1) && !string.IsNullOrEmpty(libelleLongAlpha1)) libelleCourtAlpha1 = libelleLongAlpha1;
            if (string.IsNullOrEmpty(libelleLongAlpha1) && !string.IsNullOrEmpty(libelleCourtAlpha1)) libelleLongAlpha1 = libelleCourtAlpha1;

            //Alpha 2
            if (string.IsNullOrEmpty(libelleCourtAlpha2) && !string.IsNullOrEmpty(libelleLongAlpha2)) libelleCourtAlpha2 = libelleLongAlpha2;
            if (string.IsNullOrEmpty(libelleLongAlpha2) && !string.IsNullOrEmpty(libelleCourtAlpha2)) libelleLongAlpha2 = libelleCourtAlpha2;

            var paramFamilleDto = new ParamFamilleDto
            {
                CodeConcpet = codeConcept,
                CodeFamille = codeFamille,
                LibelleFamille = libelleFamille,
                TypeCode = typeCode,
                Longueur = longueur,
                NullValue = acceptNullValue ? "O" : "N",
                LibelleCourtNum1 = libelleCourtNum1,
                LibelleLongNum1 = libelleLongNum1,
                TypeNum1 = typeNum1,
                NbrDecimal1 = nbrDecimal1,
                LibelleCourtNum2 = libelleCourtNum2,
                LibelleLongNum2 = libelleLongNum2,
                TypeNum2 = typeNum2,
                NbrDecimal2 = nbrDecimal2,
                LibelleCourtAlpha1 = libelleCourtAlpha1,
                LibelleLongAlpha1 = libelleLongAlpha1,
                LibelleCourtAlpha2 = libelleCourtAlpha2,
                LibelleLongAlpha2 = libelleLongAlpha2,
                Restriction = restriction

            };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;
                var retourMessage = voletsBlocsCategoriesClient.EnregistrerFamille(modeOperation, paramFamilleDto, additionalParam);
                if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);

                var result = voletsBlocsCategoriesClient.GetFamilles(codeConcept, string.Empty, string.Empty, additionalParam, restrictionParam).ToList();
                if (result != null && result.Count > 0)
                    result.ForEach(m => familles.Add((LigneFamille)m));
                foreach (var f in familles)
                {
                    f.AdditionalParam = additionalParam;
                    f.RestrictionParam = restrictionParam;
                }
            }
            return PartialView("ParamListFamilles", familles);
        }
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job)
        {
            if (!string.IsNullOrEmpty(cible) && !string.IsNullOrEmpty(job))
                return RedirectToAction(job, cible);
            throw new AlbTechException(new Exception("Redirection impossible"));
        }

        //[AjaxException]
        //public ActionResult UpdateListValeurs(string codeConcept, string codeFamille, string additionalParam, string restrictionParam)
        //{
        //    var valeurs = new List<Valeur>();
        //    using (var voletsBlocsCategoriesClient = new VoletsBlocsCategoriesClient())
        //    {               

        //        var valeursResult = voletsBlocsCategoriesClient.GetValeursByFamille(codeConcept, codeFamille, additionalParam, restrictionParam);
        //        if (valeursResult != null && valeursResult.Count > 0)
        //            valeursResult.ForEach(m => valeurs.Add((Valeur)m));                
        //    }
        //    return PartialView("ParamListValeurs", valeurs);
        //}
        #endregion
        #region Méthodes privées
        #region DropDowlLists
        public List<AlbSelectListItem> LoadDDLLongeurs()
        {
            var longueurs = new List<AlbSelectListItem>();
            longueurs.Insert(0, new AlbSelectListItem { Value = "1", Text = "1", Selected = true });
            longueurs.Insert(1, new AlbSelectListItem { Value = "2", Text = "2", Selected = false });
            longueurs.Insert(2, new AlbSelectListItem { Value = "3", Text = "3", Selected = false });
            longueurs.Insert(3, new AlbSelectListItem { Value = "4", Text = "4", Selected = false });
            longueurs.Insert(4, new AlbSelectListItem { Value = "5", Text = "5", Selected = false });
            longueurs.Insert(5, new AlbSelectListItem { Value = "6", Text = "6", Selected = false });
            longueurs.Insert(6, new AlbSelectListItem { Value = "7", Text = "7", Selected = false });
            longueurs.Insert(7, new AlbSelectListItem { Value = "8", Text = "8", Selected = false });
            longueurs.Insert(8, new AlbSelectListItem { Value = "9", Text = "9", Selected = false });
            longueurs.Insert(9, new AlbSelectListItem { Value = "10", Text = "10", Selected = false });
            longueurs.Insert(10, new AlbSelectListItem { Value = "11", Text = "11", Selected = false });
            longueurs.Insert(11, new AlbSelectListItem { Value = "12", Text = "12", Selected = false });
            longueurs.Insert(12, new AlbSelectListItem { Value = "13", Text = "13", Selected = false });
            longueurs.Insert(13, new AlbSelectListItem { Value = "14", Text = "14", Selected = false });
            longueurs.Insert(14, new AlbSelectListItem { Value = "15", Text = "15", Selected = false });
            return longueurs;
        }
        public List<AlbSelectListItem> LoadDDLTypes()
        {
            var types = new List<AlbSelectListItem>();
            types.Insert(0, new AlbSelectListItem { Value = "D", Text = "D", Selected = false });
            types.Insert(1, new AlbSelectListItem { Value = "M", Text = "M", Selected = false });
            return types;
        }
        public List<AlbSelectListItem> LoadDDLNbrDecimals()
        {
            var nbrDecimals = new List<AlbSelectListItem>();
            nbrDecimals.Insert(0, new AlbSelectListItem { Value = "-1", Text = "", Selected = true });
            nbrDecimals.Insert(1, new AlbSelectListItem { Value = "0", Text = "0", Selected = false });
            nbrDecimals.Insert(2, new AlbSelectListItem { Value = "1", Text = "1", Selected = false });
            nbrDecimals.Insert(3, new AlbSelectListItem { Value = "2", Text = "2", Selected = false });
            nbrDecimals.Insert(4, new AlbSelectListItem { Value = "3", Text = "3", Selected = false });
            return nbrDecimals;
        }
        public List<AlbSelectListItem> LoadDDLRestrictions()
        {
            var restrictions = new List<AlbSelectListItem>();
            string[] names = Enum.GetNames(typeof(AlbConstantesMetiers.RestrictionsEnum));
            if (names.Length > 0)
                restrictions.Insert(0, new AlbSelectListItem { Value = names[0], Text = names[0], Selected = true });
            for (int i = 1; i <= names.Length - 1; i++)
            {
                restrictions.Insert(i, new AlbSelectListItem { Value = names[i], Text = names[i], Selected = false });
            }
            return restrictions;
        }
        public List<AlbSelectListItem> LoadDDLFiltres()
        {
            var filtres = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient=client.Channel;

                var filtresResult = voletsBlocsCategoriesClient.LoadListFiltres(string.Empty, string.Empty, string.Empty);
                filtres = filtresResult.Select(
                     f => new AlbSelectListItem
                     {
                         Value = f.Code.ToString(),
                         Text = !string.IsNullOrEmpty(f.Code.ToString()) || !string.IsNullOrEmpty(f.Libelle) ? string.Format("{0} - {1}", f.Code, f.Libelle) : "",
                         Selected = false,
                         Title = !string.IsNullOrEmpty(f.Code.ToString()) || !string.IsNullOrEmpty(f.Libelle) ? string.Format("{0} - {1}", f.Code, f.Libelle) : "",
                     }).ToList();
            }
            return filtres;
        }

        #endregion
        #endregion

    }
}
