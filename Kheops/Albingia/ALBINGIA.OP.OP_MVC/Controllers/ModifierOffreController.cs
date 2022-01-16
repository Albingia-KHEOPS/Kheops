using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using EmitMapper;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Ecran.ModifierOffre;
using OP.WSAS400.DTO.FinOffre;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Indice;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Personnes;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ModifierOffreController : ControllersBase<ModifierOffre_Index_MetaModel>
    {

        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_DEV ? id : string.IsNullOrEmpty(id) ? HttpContext.Request["paramWinOpen"] : id;

            id = InitializeParams(id);

            model.PageTitle = "Informations générales Offre";
            string[] tId = id.Split('_');

            model.Offre = new Offre_MetaModel();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ISaisieOffre>())
            {
                var serviceContext = client.Channel;
                var query = new ModifierOffreGetQueryDto();
                query.CodeOffre = tId[0];
                query.Version = int.Parse(tId[1]);
                query.Type = tId[2];

                if (model.Offre != null)
                {
                    var result = serviceContext.ModifierOffreGet(query);

                    Offre_MetaModel tmpOffre = new Offre_MetaModel();
                    tmpOffre.LoadOffre(ObjectMapperManager.DefaultInstance.GetMapper<OffreDto, OffreDto>().Map(result.Offre));
                    model.Offre = tmpOffre;

                    if (result != null)
                    {
                        var motsCles1 = result.MotsCles.Select(
                            m => new AlbSelectListItem
                            {
                                Value = m.Code,
                                Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                                Selected = false,
                                Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                            }).ToList();
                        model.MotsClefs1 = motsCles1;

                        var motsCles2 = result.MotsCles.Select(
                            m => new AlbSelectListItem
                            {
                                Value = m.Code,
                                Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                                Selected = false,
                                Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                            }).ToList();
                        model.MotsClefs2 = motsCles2;

                        var motsCles3 = result.MotsCles.Select(
                            m => new AlbSelectListItem
                            {
                                Value = m.Code,
                                Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
                                Selected = false,
                                Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
                            }).ToList();
                        model.MotsClefs3 = motsCles3;

                        var devises = result.Devises.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                        model.Devises = devises;

                        var periodicites = result.Periodicites.Select(m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Descriptif = m.CodeTpcn1.ToString() }).ToList();
                        model.Periodicites = periodicites;
                        //List<AlbSelectListItem> periodicites = 
                        //model.Periodicites = periodicites;

                        var indices = result.Indices.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                        model.IndicesReference = indices;

                        var naturescontrat = result.NaturesContrat.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                        model.NaturesContrat = naturescontrat;

                        var durees = result.Durees.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                        model.Durees = durees;

                        var regimesTaxe = result.RegimesTaxe.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                        model.RegimesTaxe = regimesTaxe;
                    }

                }

                SetDataModifierOffre(model.CodePolicePage + "_" + model.VersionPolicePage + "_" + model.TypePolicePage);
            }

            model.IsReadOnly = model.Offre == null || GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);

            if (!model.IsReadOnly)
                UpdateEtatOffre();
            LoadDataFinOffre();
            return View(model);
        }
        private void LoadDataFinOffre()
        {
            ModeleFinOffrePage modele = new ModeleFinOffrePage();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                FinOffreDto result = null;
                if (model.Offre != null)
                    result = serviceContext.InitFinOffre(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, string.Empty, model.ModeNavig.ParseCode<ModeConsultation>());
                else if (model.Contrat != null)
                    result = serviceContext.InitFinOffre(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());

                if (result != null)
                    modele = ((ModeleFinOffrePage)result);

                model.ModeleFinOffreInfos = modele.ModeleFinOffreInfos;
                //  model.ModeleFinOffreAnnotation = modele.ModeleFinOffreAnnotation;
                if (model.Offre != null)
                    model.ModeleFinOffreInfos.Periodicite = model.Offre.Periodicite.Code;
                else if (model.Contrat != null)
                    model.ModeleFinOffreInfos.Periodicite = model.Contrat.PeriodiciteCode;
                List<AlbSelectListItem> antecedents = result.FinOffreInfosDto.Antecedents.Select(
                        m => new AlbSelectListItem
                        {
                            Value = m.Code,
                            Text = !string.IsNullOrEmpty(m.Code) && !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                            Selected = false,
                            Title = !string.IsNullOrEmpty(m.Code) && !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                        }
                    ).ToList();

                if (!string.IsNullOrEmpty(model.ModeleFinOffreInfos.Antecedent))
                {
                    var sItem = antecedents.FirstOrDefault(x => x.Value == model.ModeleFinOffreInfos.Antecedent);
                    if (sItem != null)
                        sItem.Selected = true;
                }
                model.ModeleFinOffreInfos.Antecedents = antecedents;
            }
            if (model.Offre != null)
            {
                if (model.Offre.DateEffetGarantie.HasValue)
                {
                    model.ModeleFinOffreInfos.DateDebStr = model.Offre.DateEffetGarantie.Value.ToString().Split(' ')[0];
                }

            }
            else if (model.Contrat != null)
            {

                if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                {
                    var timeDeb = AlbConvert.ConvertIntToTimeMinute(model.Contrat.DateEffetHeure);
                    model.ModeleFinOffreInfos.DateDebStr = new DateTime(model.Contrat.DateEffetAnnee,
                    model.Contrat.DateEffetMois,
                    model.Contrat.DateEffetJour,
                    timeDeb.HasValue ? timeDeb.Value.Hours : 0, timeDeb.HasValue ? timeDeb.Value.Minutes : 0, 0).ToString().Split(' ')[0];
                }
            }
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult ModifierOffreEnregistrer(ModifierOffre_Index_MetaModel model)
        {
            //Sauvegarde uniquement si l'écran n'est pas en readonly, ni en mode consultation d'historique
            if (!GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type, model.NumAvenantPage)
                && model.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard)
            {
                var idSessionLog = 0;
                var dateDeb = DateTime.Now;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    idSessionLog = client.Channel.SetTraceLog(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, idSessionLog, "DEB", "Save Gen_Offre", dateDeb.ToString(CultureInfo.InvariantCulture), "0");
                }

                var query = new ModifierOffreSetQueryDto();

                var version = model.Offre.Version.ToString();
                var type = model.Offre.Type;
                base.model.Offre = new Offre_MetaModel();
                // ZBO:Get Offre From Cache
                //model.Offre = CacheModels.GetOffreFromCache(model.Offre.CodeOffre, int.Parse(version), type);               
                BuildModifierOffre(model);

                // query.Offre = CastObjectServices.CastOffreToDblSaisie(model.Offre.ToOffreDto());
                query.Offre = base.model.Offre.ToOffreDto();

                // ZBO:Set Offre From Cache
                //CacheModels.SetOffreCache(model.Offre, model.Offre.CodeOffre, version, type);

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ISaisieOffre>())
                {
                    var serviceContext = client.Channel;
                    //MSL: il n'y a pas besoin de passer la liste de risques
                    var retourMsg = serviceContext.ModifierOffreSet(query, GetUser());
                    if (!string.IsNullOrEmpty(retourMsg))
                        throw new AlbFoncException(retourMsg);
                }

                //on n'execute pas les IS Branche, par conséquent on génère les clauses ici
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var retGenClause = chan.Channel.GenerateClause(type, model.Offre.CodeOffre, Convert.ToInt32(version),
                        new ParametreGenClauseDto
                        {
                            ActeGestion = "**",
                            Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale)
                        });

                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                    {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }
                /***Modif fin Offre ****/




                /******/
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var dateFin = DateTime.Now;
                    var diffDate = dateFin - dateDeb;
                    client.Channel.SetTraceLog(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, idSessionLog, "FIN", "Save Gen_Offre", dateFin.ToString(CultureInfo.InvariantCulture), diffDate.ToString());
                }
            }


            if (!string.IsNullOrEmpty(model.txtParamRedirect))
            {
                var tabParamRedirect = model.txtParamRedirect.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            var folder = new Folder { CodeOffre = model.Offre.CodeOffre, Version = model.Offre.Version.GetValueOrDefault(), Type = model.Offre.Type, NumeroAvenant = int.TryParse(model.NumAvenantPage, out int num) ? num : 0 };
            string id = AlbParameters.BuildFullId(
                folder,
                AlbOpConstants.IgnoreISBranche.AsBoolean() == true ? new[] { "¤ModifierOffre¤Index¤" + folder.BuildId("£") } : new[] { folder.NumeroAvenant.ToString() },
                model.TabGuid,
                model.AddParamValue,
                model.ModeNavig);
            #region Ajout des IS pour Offre / Contrat
            string controller = null;

            bool hasIS = InformationsSpecifiquesBrancheController.HasIS(
                new AffaireId
                {
                    CodeAffaire = model.Offre.CodeOffre,
                    NumeroAliment = model.Offre.Version.GetValueOrDefault(),
                    TypeAffaire = model.Offre.Type.ParseCode<AffaireType>(),
                    NumeroAvenant = int.TryParse(model.NumAvenantPage, out int numAvt) && numAvt >= 0 ? numAvt : default(int?)
                }, 0);

            //if (AlbOpConstants.IgnoreISBranche.AsBoolean() == true)
            //{
            if (hasIS)
            {
                controller = "InformationsSpecifiquesBranche";
                return RedirectToAction("Index", controller, new { id });
            }
            else
                controller = "ChoixClauses";
            //}
            #endregion

            return RedirectToAction("Index", controller, new { id });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            if (cible == "RechercheSaisie")
            {
                return RedirectToAction(job, cible);
            }
            else
            {
                return RedirectToAction(job, cible, new { id = codeOffre + "_O_" + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }
        }

        private void BuildModifierOffre(ModifierOffre_Index_MetaModel model)
        {
            // var model.Offre = new OffreDto();
            //Récupération du code de la cible
            string codeCible;
            if (!string.IsNullOrEmpty(model.Cible.Split(new[] { " - " }, StringSplitOptions.None)[0]))
                codeCible = model.Cible.Split(new[] { " - " }, StringSplitOptions.None)[0];
            else
                codeCible = model.Cible;

            base.model.Offre.CodeOffre = model.Offre.CodeOffre;
            base.model.Offre.Version = model.Offre.Version;
            base.model.Offre.Type = model.Offre.Type;
            base.model.Offre.Descriptif = model.Descriptif;
            base.model.Offre.Branche = new BrancheDto
            {
                Cible = new CibleDto
                {
                    Code = codeCible
                }
            };
            base.model.Offre.MotCle1 = model.MotClef1;
            base.model.Offre.MotCle2 = model.MotClef2;
            base.model.Offre.MotCle3 = model.MotClef3;
            if (!string.IsNullOrEmpty(model.Observations))
            {
                base.model.Offre.Observation = model.Observations.Replace("\r\n", "<br>");
            }
            else
            {
                base.model.Offre.Observation = string.Empty;
            }
            /**********************/
            base.model.Offre.ModeleFinOffreInfos = new FinOffreInfosDto
            {
                Antecedent = model.Antecedent,
                Description = model.Description,
                ValiditeOffre = model.ValiditeOffre,
                DateProjet = model.DateProjet,
                Relance = model.Relance,
                RelanceValeur = model.RelanceValeur,
                Preavis = model.Preavis
            };

            /***********************/

            base.model.Offre.Devise = new ParametreDto
            {
                Code = model.Devise
            };
            base.model.Offre.Periodicite = new ParametreDto
            {
                Code = model.Periodicite
            };
            base.model.Offre.EcheancePrincipale = model.EcheancePrincipale;
            if (model.EffetGaranties.HasValue)
            {
                if (model.HeureEffet.HasValue)
                    base.model.Offre.DateEffetGarantie = new DateTime(model.EffetGaranties.Value.Year, model.EffetGaranties.Value.Month, model.EffetGaranties.Value.Day, model.HeureEffet.Value.Hours, model.HeureEffet.Value.Minutes, 0);
                else
                    base.model.Offre.DateEffetGarantie = new DateTime(model.EffetGaranties.Value.Year, model.EffetGaranties.Value.Month, model.EffetGaranties.Value.Day, 0, 0, 0);
            }
            else
            {
                base.model.Offre.DateEffetGarantie = null;
            }
            if (model.FinEffet.HasValue)
            {
                if (model.HeureFinEffet.HasValue)
                    base.model.Offre.DateFinEffetGarantie = new DateTime(model.FinEffet.Value.Year, model.FinEffet.Value.Month, model.FinEffet.Value.Day, model.HeureFinEffet.Value.Hours, model.HeureFinEffet.Value.Minutes, 0);
                else
                    base.model.Offre.DateFinEffetGarantie = new DateTime(model.FinEffet.Value.Year, model.FinEffet.Value.Month, model.FinEffet.Value.Day, 0, 0, 0);
            }
            else
            {
                base.model.Offre.DateFinEffetGarantie = null;
            }
            if (model.DateStatistique.HasValue)
            {
                base.model.Offre.DateStatistique = model.DateStatistique.Value;
            }
            else
            {
                base.model.Offre.DateStatistique = null;
            }
            base.model.Offre.DureeGarantie = model.Duree;
            base.model.Offre.UniteDeTemps = new ParametreDto
            {
                Code = model.DureeString
            };

            if (base.model.Offre.DureeGarantie > 0)
            {
                base.model.Offre.DateFinEffetGarantie = AlbConvert.GetFinPeriode(base.model.Offre.DateEffetGarantie, base.model.Offre.DureeGarantie.Value, base.model.Offre.UniteDeTemps.Code);
            }

            base.model.Offre.IndiceReference = new ParametreDto
            {
                Code = model.IndiceReference
            };
            base.model.Offre.Valeur = (String.IsNullOrEmpty(model.Valeur)) ? 0 : Convert.ToDecimal(model.Valeur.Replace(".", ","));
            base.model.Offre.NatureContrat = new ParametreDto
            {
                Code = !string.IsNullOrEmpty(model.NatureContrat) ? model.NatureContrat : string.Empty
            };
            if (!String.IsNullOrEmpty(model.PartAlbingia))
            {
                base.model.Offre.PartAlbingia = Convert.ToDecimal(model.PartAlbingia.Replace(".", ","));
                base.model.Offre.PartAperiteur = 100 - base.model.Offre.PartAlbingia;
            }
            else
            {
                base.model.Offre.PartAlbingia = null;
                base.model.Offre.PartAperiteur = null;
            }

            base.model.Offre.Aperiteur = new AperiteurDto
            {
                Code = model.AperiteurCode,
                Nom = !string.IsNullOrEmpty(model.AperiteurNom) ? model.AperiteurNom.Split('-')[1].Trim() : string.Empty
            };

            base.model.Offre.Couverture = model.Couverture;
            if (!String.IsNullOrEmpty(model.FraisApe))
            {
                base.model.Offre.FraisAperition = Convert.ToDecimal(model.FraisApe.Replace(".", ","));
            }
            else
            {
                base.model.Offre.FraisAperition = null;
            }
            base.model.Offre.IntercalaireCourtier = model.Intercalaire;

            base.model.Offre.Souscripteur = new SouscripteurDto
            {
                Code = model.SouscripteurCode,
                Nom = model.SouscripteurNom.Split('-')[1].Trim()
            };
            base.model.Offre.Gestionnaire = new GestionnaireDto
            {
                Id = model.GestionnaireCode,
                Nom = model.GestionnaireNom.Split('-')[1].Trim()
            };

            base.model.Offre.CodeRegime = model.RegimeTaxe;
            base.model.Offre.SoumisCatNat = model.SoumisCatNat ? "O" : "N";
            base.model.Offre.LTA = model.LTA;
        }

        private void SetDataModifierOffre(string id)
        {
            model.Bandeau = null;
            model.AfficherBandeau = DisplayBandeau(true, id);
            model.AfficherNavigation = model.AfficherBandeau;
            model.AfficherArbre = true;
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault()); //new Models.MetaData.Bandeau_MetaData();
                //Gestion des Etapes
                model.Navigation = new Navigation_MetaModel();
                model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
                model.Navigation.IdOffre = model.Offre.CodeOffre;
                model.Navigation.Version = model.Offre.Version;
            }


            if (model.Offre.Branche != null)
            {
                if (model.Offre.Branche.Cible != null)
                {
                    model.Cible = model.Offre.Branche.Cible.Code;
                    model.CibleLib = model.Offre.Branche.Cible.Nom;
                }
            }
            model.Descriptif = model.Offre.Descriptif;
            model.Observations = model.Offre.Observation;
            model.MotClef1 = model.Offre.MotCle1;
            model.MotClef2 = model.Offre.MotCle2;
            model.MotClef3 = model.Offre.MotCle3;

            if (model.Offre.Devise != null)
            {
                model.Devise = model.Offre.Devise.Code;
            }
            if (model.Offre.Periodicite != null)
            {
                model.Periodicite = model.Offre.Periodicite.Code;
            }
            model.EcheancePrincipale = model.Offre.EcheancePrincipale;
            if (model.Offre.DateEffetGarantie != null)
            {
                model.EffetGaranties = model.Offre.DateEffetGarantie;
                model.HeureEffet = new TimeSpan(model.Offre.DateEffetGarantie.Value.Hour, model.Offre.DateEffetGarantie.Value.Minute, 0);
            }
            if (model.Offre.DateFinEffetGarantie != null)
            {
                model.FinEffet = model.Offre.DateFinEffetGarantie;
                model.HeureFinEffet = new TimeSpan(model.Offre.DateFinEffetGarantie.Value.Hour, model.Offre.DateFinEffetGarantie.Value.Minute, 0);
            }
            if (model.Offre.DateStatistique != null)
            {
                model.DateStatistique = model.Offre.DateStatistique;
            }
            else
            {
                model.DateStatistique = model.Offre.DateSaisie;
            }

            model.Duree = (model.Offre.DureeGarantie == 0) ? null : model.Offre.DureeGarantie;
            if (model.Offre.UniteDeTemps != null)
            {
                model.DureeString = model.Offre.UniteDeTemps.Code;
            }
            if (model.Offre.IndiceReference != null)
            {
                model.IndiceReference = model.Offre.IndiceReference.Code;
            }
            model.Valeur = model.Offre.Valeur.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
            if (model.Offre.NatureContrat != null)
            {
                model.NatureContrat = model.Offre.NatureContrat.Code;
            }
            if (model.Offre.PartAlbingia != null)
            {
                //model.PartAlbingia = model.Offre.PartAlbingia.ToString().Replace(",", ".");
                model.PartAlbingia = model.Offre.PartAlbingia.ToString();
            }
            if (model.Offre.Aperiteur != null)
            {
                model.AperiteurCode = model.Offre.Aperiteur.Code;
                model.AperiteurNom = model.Offre.Aperiteur.Code + " - " + model.Offre.Aperiteur.Nom;
            }
            model.Couverture = model.Offre.Couverture;
            if (model.Offre.FraisAperition != null)
            {
                //model.FraisApe = model.Offre.FraisAperition.ToString().Replace(",", ".");
                model.FraisApe = model.Offre.FraisAperition.ToString();
            }
            model.Intercalaire = model.Offre.IntercalaireCourtier;
            if (model.Offre.Souscripteur != null)
            {
                model.SouscripteurCode = model.Offre.Souscripteur.Code;
                model.SouscripteurNom = model.Offre.Souscripteur.Code + " - " + model.Offre.Souscripteur.Nom;
            }
            if (model.Offre.Gestionnaire != null)
            {
                model.GestionnaireCode = model.Offre.Gestionnaire.Id;
                model.GestionnaireNom = model.Offre.Gestionnaire.Id + " - " + model.Offre.Gestionnaire.Nom;
            }
            model.SoumisCatNat = model.Offre.SoumisCatNat == "O";
            model.IsMonoRisque = model.Offre.IsMonoRisque;
            model.RegimeTaxe = model.Offre.CodeRegime;
            //Affichage de la navigation latérale en arboresence
            model.NavigationArbre = GetNavigationArbre("InfoGen");
            model.PartBenef = model.Offre.PartBenef;

            model.OppBenef = model.Offre.OppBenef;
            model.LTA = model.Offre.LTA;
        }

        private void UpdateEtatOffre()
        {
            if (model.Offre.Etat == "A")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    serviceContext.UpdateEtatContrat(model.Offre.CodeOffre, model.Offre.Version ?? 0, model.Offre.Type, "N");
                }
                model.Offre.Etat = "N";
            }
        }

        #region Indice

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetValeurIndiceByCode(string indiceString, string dateEffet)
        {
            //if (string.IsNullOrEmpty(dateEffet))
            //{
            //    dateEffet = AlbConvert.ConvertDateToStr(DateTime.Now);
            //}

            var toReturn = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            toReturn.Data = GetValeurIndiceByCodeImplementation(indiceString, dateEffet);
            return toReturn;
        }
        private ModeleIndice GetValeurIndiceByCodeImplementation(string indiceString, string dateEffet)
        {
            return (ModeleIndice)IndiceGet(new IndiceGetQueryDto { Code = indiceString, DateEffet = dateEffet });
        }
        private IndiceGetResultDto IndiceGet(IndiceGetQueryDto query)
        {
            IndiceGetResultDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                toReturn = serviceContext.IndiceGet(query);
            }
            return toReturn;
        }

        #endregion

    }
}
