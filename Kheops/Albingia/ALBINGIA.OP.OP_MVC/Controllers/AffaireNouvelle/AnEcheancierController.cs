using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesEcheance;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.NavigationArbre;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle
{
    public class AnEcheancierController : ControllersBase<ModeleEcheancierPage>
    {
        #region Méthodes publiques
        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Echéancier";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }
        /// <summary>
        /// Ouvre l'échéancier en div flottante
        /// </summary>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult OpenEcheancier(string id, string addParamType, string addParamValue, string modeNavig, string modeSaisie, string modeAffichage)
        {
            id = id + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig);
            id = InitializeParams(id);            
            LoadInfoPage(id, modeSaisie,modeAffichage);
          
            return PartialView("Index",model);
        }
        [ErrorHandler]
        public ActionResult LoadEcheance(int guid, int dateEcheance, string montantEcheance, string prime, string fraisAccess, bool taxeAttentat, string primeHT, string totalMontantEcheance, string montantRestant, string primeComptant)
        {
            double dMontantEcheance = 0;
            decimal dPrime = 0;
            decimal dFraisAcess = 0;
            double dPrimeHT = 0;
            double dTotalMontantEcheance = 0;
            double dMontantRestant = 0;
            double dPrimeComptant = 0;

            double.TryParse(montantEcheance.Replace(".", ","), out dMontantEcheance);
            decimal.TryParse(prime.Replace(".", ","), out dPrime);
            decimal.TryParse(fraisAccess.Replace(".", ","), out dFraisAcess);
            double.TryParse(primeHT.Replace(".", ","), out dPrimeHT);
            double.TryParse(totalMontantEcheance.Replace(".", ","), out dTotalMontantEcheance);
            double.TryParse(montantRestant.Replace(".", ","), out dMontantRestant);
            double.TryParse(primeComptant.Replace(".", ","), out dPrimeComptant);


            var montant = dPrime != 0 ? Math.Round(dPrimeHT * double.Parse(dPrime.ToString()) / 100, 2) : dMontantEcheance;
            double montantPropose = 0;
            double primePropose = 0;
            if (double.TryParse(totalMontantEcheance.Replace(".", ","), out dTotalMontantEcheance) && double.TryParse(montantRestant.Replace(".", ","), out dMontantRestant))
            {
                montantPropose = dMontantRestant - dTotalMontantEcheance >= 0 ? dMontantRestant - dTotalMontantEcheance : 0;
                primePropose = dPrimeHT > 0 ? Math.Round((montantPropose / dPrimeHT) * 100, 2) : 0;
            }
            ModeleEcheance echeance = null;
            if (guid != 0)
            {
                echeance = new ModeleEcheance
                {
                    Guid = guid,
                    EcheanceDate = AlbConvert.ConvertIntToDate(dateEcheance),
                    MontantEcheanceHT = montant,
                    PourcentagePrime = dPrime,
                    FraisAccessoire = dFraisAcess,
                    TaxeAttentat = taxeAttentat
                };
            }

            else
                echeance = new ModeleEcheance
                {
                    Guid = guid,
                    EcheanceDate = null,
                    MontantEcheanceHT = montantPropose > 0 && !double.TryParse(primeComptant, out dPrimeComptant) ? montantPropose : 0,
                    PourcentagePrime = decimal.Parse(primePropose.ToString()),
                    FraisAccessoire = dFraisAcess,
                    TaxeAttentat = false
                };

            return PartialView("UpdateEcheance", echeance);
        }
        [ErrorHandler]
        public void UpdateEcheance(string codeOffre, string version, string type, string codeAvn, string tabGuid, string dateEcheance, string primeHT, string primePourcent, string montantEcheance,
            string fraisAccessoires, bool taxeAttentat, string typeOperation, string dateDeb, string dateFin, string dateDerniereEcheance)
        {
            DateTime dDateEcheance;
            decimal dPrimeHT = 0;
            decimal dPrimePourcent = 0;
            decimal dMontantEcheance = 0;
            decimal dFraisAccessoire = 0;
            DateTime dDateDeb;
            DateTime dDateFin;
            DateTime dDateDerniereEcheance;

            DateTime.TryParse(dateEcheance, out dDateEcheance);
            decimal.TryParse(primeHT.Replace(".",","), out dPrimeHT);
            decimal.TryParse(primePourcent.Replace(".", ","), out dPrimePourcent);
            decimal.TryParse(montantEcheance.Replace(".", ","), out dMontantEcheance);
            decimal.TryParse(fraisAccessoires.Replace(".", ","), out dFraisAccessoire);
            DateTime.TryParse(dateDeb, out dDateDeb);
            DateTime.TryParse(dateFin, out dDateFin);
            DateTime.TryParse(dateDerniereEcheance, out dDateDerniereEcheance);


            decimal prime = decimal.TryParse(primePourcent.Replace(".", ","), out dPrimePourcent) ? dPrimePourcent : 0;
            //decimal montantCalcule = Math.Round((dPrimeHT / 100) * prime, 2);
            decimal montantCalcule = dMontantEcheance;
            decimal montant = decimal.TryParse(primePourcent.Replace(".", ","), out dPrimePourcent) ? 0 : decimal.TryParse(montantEcheance.Replace(".", ","), out dMontantEcheance) ? dMontantEcheance : 0;
            decimal frais = decimal.TryParse(fraisAccessoires.Replace(".", ","), out dFraisAccessoire) ? dFraisAccessoire : 0;
                       
            //if (prime == 0 && montant > 0 && dPrimeHT > 0)
            //{
            //    prime = (montant / dPrimeHT) * 100;
            //}

            if (typeOperation == "I" && !CheckDate(dDateEcheance, dDateDeb, dDateFin, dDateDerniereEcheance)) return;
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    string retourMessage = finOffreClient.UpdateEcheance(codeOffre, version, type, dDateEcheance, prime, montant, montantCalcule, frais, taxeAttentat, typeOperation, 2);
                    if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
                }
            }
        }
        [ErrorHandler]
        public void UpdateEcheancier(string codeOffre, string version, string type, string codeAvn, string tabGuid, string primeHT, string primePourcent, string comptant, string fraisAccessoires, bool taxeAttentat,
            string montantRestant, string TotalMontantEcheanceSansDerniere, string dateDerniereEcheance, string primeDerniereEcheance, string fraisDerniereEcheance,
            bool taxeAttentatDerniereEcheance, string modeNavig)
        {

            decimal dPrimeHT = 0;
            decimal dPrimePourcent = 0;
            decimal dComptant = 0;
            decimal dFraisAccessoires = 0;
            decimal dMontantRestant = 0;
            decimal dTotalMontantEcheanceSansDerniere = 0;
            DateTime dDateDerniereEcheance;
            decimal dPrimeDerniereEcheance = 0;
            decimal dFraisDerniereEcheance = 0;
            decimal.TryParse(primeHT, out dPrimeHT);
            decimal.TryParse(primePourcent, out dPrimePourcent);
            decimal.TryParse(comptant, out dComptant);
            decimal.TryParse(fraisAccessoires, out dFraisAccessoires);
            decimal.TryParse(montantRestant, out dMontantRestant);
            decimal.TryParse(TotalMontantEcheanceSansDerniere, out dTotalMontantEcheanceSansDerniere);
            DateTime.TryParse(dateDerniereEcheance, out dDateDerniereEcheance);
            decimal.TryParse(primeDerniereEcheance, out dPrimeDerniereEcheance);
            decimal.TryParse(fraisDerniereEcheance, out dFraisDerniereEcheance);
            decimal prime = decimal.TryParse(primePourcent, out dPrimePourcent) ? dPrimePourcent : 0;
            decimal montantCalcule = (dPrimeHT / 100) * prime;
            decimal montantDerniereEchCalcule = dMontantRestant - dTotalMontantEcheanceSansDerniere;

            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    // string retourMessage = finOffreClient.UpdateEcheance(codeOffre, version, type, dDateDerniereEcheance, dPrimeDerniereEcheance, 0, montantDerniereEchCalcule, dFraisDerniereEcheance, taxeAttentatDerniereEcheance, "U", 2);

                    //if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
                    //else
                    //{

                    string retour = finOffreClient.UpdateEcheance(codeOffre, version, type, null, prime, dComptant, montantCalcule, dFraisAccessoires, taxeAttentat, "C", 1);
                    if (!string.IsNullOrEmpty(retour)) throw new AlbFoncException(retour);
                    else
                    {
                        if (dPrimeHT > 0 && dPrimePourcent == 0)
                        {
                             retour = finOffreClient.UpdatePourcentCalcule(codeOffre, version, type, codeAvn, double.Parse(comptant.ToString()), double.Parse(primeHT.ToString()), modeNavig.ParseCode<ModeConsultation>());
                        }
                        else
                        {
                             retour = finOffreClient.UpdateMontantCalcule(codeOffre, version, type, codeAvn, primePourcent, double.Parse(comptant.ToString()), double.Parse(primeHT.ToString()), modeNavig.ParseCode<ModeConsultation>());                        
                        }
                        if (!string.IsNullOrEmpty(retour)) throw new AlbFoncException(retour);
                    }
                    //};
                }
            }
        }
        [ErrorHandler]
        public ActionResult UpdateListeEcheances(string codeOffre, string version, string type, string codeAvn, string modeNavig, string tabGuid)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                var echeancesDto = finOffreClient.GetEcheances(codeOffre, version, type, codeAvn, 2, modeNavig.ParseCode<ModeConsultation>());
                var echeances = ModeleEcheance.LoadEcheances(echeancesDto, GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn));
                return PartialView("/Views/AnEcheancier/ListeEcheances.cshtml", echeances);
            }
        }
        [ErrorHandler]
        public void SupprimerEcheance(string codeOffre, string version, string type, string codeAvn, string tabGuid, int dateEcheance)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    finOffreClient.SupprimerEcheance(codeOffre, version, type, AlbConvert.ConvertIntToDate(dateEcheance).Value);
                }
            }
        }
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue)
        {
            if (cible == "ChoixClauses")
            {
                var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
                if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn)
      && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var serviceContext=client.Channel;
                        serviceContext.SetTrace(new TraceDto
                        {
                            CodeOffre = codeOffre.PadLeft(9, ' '),
                            Version = Convert.ToInt32(version),
                            Type = type,
                            EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                            NumeroOrdreDansEtape = 64,
                            NumeroOrdreEtape = 1,
                            Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                            Risque = 0,
                            Objet = 0,
                            IdInventaire = 0,
                            Formule = 0,
                            Option = 0,
                            Niveau = string.Empty,
                            CreationUser = GetUser(),
                            PassageTag = "O",
                            PassageTagClause = string.Empty
                        });
                    }

                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                    {
                        var serviceontext=client.Channel;
                        RetGenClauseDto retGenClause = serviceontext.GenerateClause(type, codeOffre, Convert.ToInt32(version),
                          new ParametreGenClauseDto
                          {
                              ActeGestion = "**",
                              Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)
                          });
                        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                        {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }

                    return RedirectToAction("Index", "ChoixClauses", new { id = codeOffre + "_" + version + "_" + type + "_¤Quittance¤Index¤" + codeOffre + "£" + version + "£" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }
                else
                {
                    return RedirectToAction("Index", "ChoixClauses", new { id = codeOffre + "_" + version + "_" + type + "_¤Quittance¤Index¤" + codeOffre + "£" + version + "£" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }
            }
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) });
        }
        [ErrorHandler]
        public void SupprimerEcheancier(string codeOffre, string version, string type, string codeAvn, string tabGuid)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient=client.Channel;
                    finOffreClient.SupprimerEcheancier(codeOffre, version, type, codeAvn);
                }
            }
        }

        #endregion
        #region Méthodes privées
        private void LoadInfoPage(string id, string modeSaisie = "",string modeAffichage="")
        {
            string[] tId = id.Split('_');
            if (tId[2] == "P")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
                var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        break;
                    default:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                }
            }
            if (model.Contrat != null)
            {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
                //model.PeriodeDebut = tId.Length >= 4 ? tId[3] : string.Empty;

                //double primeHT = 0;
                //if (tId.Length >= 4 && !string.IsNullOrEmpty(tId[5]))
                //    double.TryParse(tId[5], out primeHT);
                //model.PrimeHT = primeHT;
                //decimal fraisAccessoiresHT = 0;
                //if (tId.Length >= 5 &&!string.IsNullOrEmpty(tId[6]))
                //    decimal.TryParse(tId[6], out fraisAccessoiresHT);
                //model.FraisAccessoiresHT = fraisAccessoiresHT;
                //decimal fgaTaxe = 0;
                //if (tId.Length >= 6 && !string.IsNullOrEmpty(tId[7]))
                //    decimal.TryParse(tId[7], out fgaTaxe);
                //model.TaxeAttentat = fgaTaxe != 0 ? true : false;
            }

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), tId[0] + "_" + tId[1] + "_" + tId[2],model.NumAvenantPage);

            if (modeAffichage == "Visu")
            {
                model.IsReadOnly = true;
                model.IsForceReadOnly = true;
            }

            //Echeances
            LoadEcheances(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig, model.IsReadOnly, modeSaisie);
        }
        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Contrat != null)
                {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_COTISATIONS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                    //model.NavigationArbre = GetNavigationArbreRegule(ContentData, "Regule");
                    //model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle(string.Empty);
            }
        }
        private void LoadEcheances(string codeOffre, string version, string type, string codeAvn, string modeNavig, bool isreadonly, string modeSaisieEcheancier="")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                var echeancierDto = finOffreClient.InitEcheancier(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), modeSaisieEcheancier);
                model.PrimeHT = echeancierDto.PrimeHT;
                model.FraisAccessoiresHT = echeancierDto.FraisAccessoiresHT;
                model.PeriodeDebut = echeancierDto.PeriodeDebut;
                model.TaxeAttentat = echeancierDto.TaxeAttentat;
                model.IsModeSaisieParMontant = echeancierDto.IsModeSaisieParMontant;

                model.ComptantHT = echeancierDto.ComptantHT;
                model.PrimeComptant = echeancierDto.PrimeComptant;
                model.MontantRestant = model.PrimeHT - model.ComptantHT;
                int frais = 0;
                int.TryParse(model.FraisAccessoiresHT.ToString(CultureInfo.InvariantCulture), out frais);
                model.FraisAccessoiresComptantHT = echeancierDto.FraisAccessoire != 0 ? echeancierDto.FraisAccessoire : frais;
                model.Echeances = ModeleEcheance.LoadEcheances(echeancierDto.Echeances, isreadonly);

            }

            var finEffetContrat = AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", model.Contrat.FinEffetJour, model.Contrat.FinEffetMois, model.Contrat.FinEffetAnnee));
            if (!finEffetContrat.HasValue)
            {
                finEffetContrat = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", model.Contrat.DateEffetJour, model.Contrat.DateEffetMois, model.Contrat.DateEffetAnnee)), model.Contrat.DureeGarantie, model.Contrat.UniteDeTemps);
            }
            model.PeriodeFin = finEffetContrat.HasValue ? finEffetContrat.Value.ToString("dd/MM/yyyy") : string.Empty;


        }
        private bool CheckDate(DateTime? dateEcheance, DateTime? dateDeb, DateTime? dateFin, DateTime? dateDerniereEcheance)
        {
            var toReturn = true;
            if (!dateEcheance.HasValue || !dateDeb.HasValue || !dateFin.HasValue) return false;
            if (AlbConvert.ConvertDateToInt(dateEcheance.Value) <= AlbConvert.ConvertDateToInt(DateTime.Now))
            {
                toReturn = false;
                throw new AlbFoncException("Date invalide : La date doit être supérieure à la date d'aujourd'hui");
            }
            if (AlbConvert.ConvertDateToInt(dateEcheance.Value) < AlbConvert.ConvertDateToInt(dateDeb.Value) || AlbConvert.ConvertDateToInt(dateEcheance.Value) > AlbConvert.ConvertDateToInt(dateFin.Value))
            {
                toReturn = false;
                throw new AlbFoncException("Date invalide : La date doit être incluse les dates du contrat");
            }
            if (dateDerniereEcheance.HasValue && AlbConvert.ConvertDateToInt(dateEcheance.Value) <= AlbConvert.ConvertDateToInt(dateDerniereEcheance.Value))
            {
                toReturn = false;
                throw new AlbFoncException("Date invalide : La date doit être postérieure à la dernière echéance");
            }


            return toReturn;
        }

        //private void CalculePoucentagePrime(string codeOffre, string version, string type,List<ModeleEcheance> echeances, double comptantHT,double primeHT)
        //{
        //    double totalPourcent = 0;
        //    double diff = 0;
        //    double maxMontant = 0;
        //    int maxIndex = 0;
        //    int i = 0;
        //    decimal primeComptantCalcule = 0;

        //    foreach (var ech in echeances)
        //    {

        //        ech.PourcentageCalcule = Math.Round(ech.MontantEcheanceHT != 0 ? decimal.Parse((ech.MontantEcheanceHT / primeHT).ToString()) : decimal.Parse((ech.MontantEcheanceCalcule / primeHT).ToString()), 6);
        //        totalPourcent += double.Parse(ech.PourcentageCalcule.ToString());

        //        if (ech.MontantEcheanceHT > maxMontant || ech.MontantEcheanceCalcule > maxMontant)
        //        {
        //            maxMontant = ech.MontantEcheanceHT != 0 ? ech.MontantEcheanceHT : ech.MontantEcheanceCalcule;
        //            maxIndex = i;
        //        }
        //        i++;
        //    }
        //    primeComptantCalcule = Math.Round(decimal.Parse((comptantHT / primeHT).ToString()), 6);
        //    totalPourcent += double.Parse(primeComptantCalcule.ToString());

        //    diff = 1 - totalPourcent;
        //    if (diff > 0)
        //    {
        //        if (comptantHT > maxMontant)
        //            primeComptantCalcule += decimal.Parse(diff.ToString());
        //        else
        //            echeances[maxIndex].PourcentageCalcule += decimal.Parse(diff.ToString());
        //    }
        //}
        #endregion
    }
}
