using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ExclusionFormuleGarantieController : ControllersBase<ModeleExclusionFormuleGarantiePage>
    {

        #region Variables membres

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExclusionFormuleGarantieController));

        #endregion

        #region Propriété Publique

        #endregion

        #region Méthodes publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Exclusion d'un risque ou inventaire";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return PartialView(model);
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string id)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=channelClient.Channel;
                model.Bandeau = null;
                model.Offre = new Offre_MetaModel();

                string[] tId = id.Split('_');
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                 var policeServicesClient=chan.Channel;
                    model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                }
                if (model.Offre != null)
                {
                    model.AfficherBandeau = base.DisplayBandeau(true, id);
                    model.AfficherNavigation = model.AfficherBandeau;

                    //Affichage de la navigation latérale en arboresence
                    model.NavigationArbre = GetNavigationArbre(string.Empty);
                }

                if (model.AfficherBandeau)
                {
                    model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                    //Gestion des Etapes
                    model.Navigation = new Navigation_MetaModel();
                    model.Navigation.Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE;
                    model.Navigation.IdOffre = model.Offre.CodeOffre;
                    model.Navigation.Version = model.Offre.Version;
                }
                LoadInfoFormule(serviceContext, id);
            }
        }

        private void LoadInfoFormule(IRisquesGaranties serviceContext, string id)
        {
            string[] tabInfo = id.Split('_');
            string codeOffre = tabInfo[0];
            string version = tabInfo[1];
            string codeFormule = tabInfo[2];
            string codeOption = tabInfo[3];
            string codeGarantie = tabInfo[4];

            List<string> lstType = new List<string>();
            lstType.Add("RQ");
            lstType.Add("OB");

            //model.RisqueCoche = serviceContext.ObtenirRisqueObjetApplique(codeOption, lstType);//new string[] {"RQ", "OB"});
            if (string.IsNullOrEmpty(model.RisqueCoche))
            {
                //model.RisqueObjetCoche = serviceContext.ObtenirRisqueObjetApplique(codeOption, 'OB');
                //if (!model.RisqueObjetCoche)
                //{
                //retour
                //}
            }

            FormuleDto result = serviceContext.InitFormuleGarantie(codeOffre, version, model.Offre.Type, GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID), codeFormule, codeOption, string.Empty, model.ModeNavig.ParseCode<ModeConsultation>(), false, GetUser());
            //string descriptionGarantie = serviceContext.ObtenirLibGarantie(codeGarantie);

            model.CodeFormule = codeFormule;
            model.CodeOption = codeOption;

            model.LettreFormule = result.LettreLib;
            //model.Descriptif = descriptionGarantie;

            model.ObjetRisqueCode = "1;1";
            model.ObjetRisque = "à l'ensemble du risque";

            if (!string.IsNullOrEmpty(result.ObjetRisqueCode))
            {
                SetLibAppliqueA(result);
            }

            List<ModeleRisque> risques = RisqueGet(serviceContext, id);
            model.ObjetsRisques = new ModeleFormuleGarantieLstObjRsq
            {
                TableName = "ListRsqObj",
                Risques = risques
            };
        }

        private List<ModeleRisque> RisqueGet(IRisquesGaranties serviceContext, string id)
        {
            string CodeOffre = id.Split('_')[0];
            string Version = id.Split('_')[1];
            string CodeFormule = id.Split('_')[2];
            string CodeOption = id.Split('_')[3];

            List<ModeleRisque> toReturn = null;
            //List<RisqueDto> result = serviceContext.ListeRisquesGetByTerm(CodeOffre, Version).ToList();
            List<RisqueDto> result = new List<RisqueDto>();
            //result.Add(serviceContext.ObtenirRisqueApplique(CodeOffre, Version, CodeFormule, CodeOption));
            if (result != null && result.Count > 0)
            {
                toReturn = new List<ModeleRisque>();
                result.ForEach(m => toReturn.Add((ModeleRisque)m));
            }
            return toReturn;
        }

        private void SetLibAppliqueA(FormuleDto formuleDto)
        {
            string selectedRisque = string.Empty;
            string selectedObjet = string.Empty;

            if (!string.IsNullOrEmpty(formuleDto.ObjetRisqueCode))
            {
                int countSelectedObjet = formuleDto.ObjetRisqueCode.Split(';')[1].Split('_').Length;
                RisqueDto rsqDto = formuleDto.Risques.FirstOrDefault(m => m.Code.ToString(CultureInfo.CurrentCulture) == formuleDto.ObjetRisqueCode.Split(';')[0]);

                if (formuleDto.Risques.Count() == 1)
                {
                    if (rsqDto.Objets.Count() == 1 || rsqDto.Objets.Count() == countSelectedObjet)
                    {
                        foreach (ObjetDto obj in rsqDto.Objets)
                        {
                            selectedObjet += "_" + obj.Code;
                        }
                        if (!string.IsNullOrEmpty(selectedObjet))
                        {
                            model.ObjetRisqueCode = rsqDto.Code + ";" + selectedObjet.Substring(1);
                            model.ObjetRisque = "à l'ensemble du risque";
                        }
                    }
                    else
                    {
                        foreach (ObjetDto obj in rsqDto.Objets)
                        {
                            if (("_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_")
                            {
                                selectedObjet += "_" + obj.Code;
                            }
                        }
                        if (!string.IsNullOrEmpty(selectedObjet))
                        {
                            model.ObjetRisqueCode = rsqDto.Code + ";" + selectedObjet.Substring(1);
                            model.ObjetRisque = "à " + countSelectedObjet + " objets du risque";
                        }
                    }
                }
                else
                {
                    RisqueDto rsq = formuleDto.Risques.FirstOrDefault(m => m.Code.ToString(CultureInfo.CurrentCulture) == formuleDto.ObjetRisqueCode.Split(';')[0]);

                    if (rsq.Objets.Count() == 1 || rsq.Objets.Count() == countSelectedObjet)
                    {
                        foreach (ObjetDto obj in rsq.Objets)
                        {
                            selectedObjet += "_" + obj.Code;
                        }
                        if (!string.IsNullOrEmpty(selectedObjet))
                        {
                            model.ObjetRisqueCode = rsq.Code.ToString() + ";" + selectedObjet.Substring(1);
                            model.ObjetRisque = "au risque " + rsq.Code + " '" + rsq.Descriptif + "'";
                        }
                    }
                    else
                    {
                        foreach (ObjetDto obj in rsq.Objets)
                        {
                            if (("_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_")
                            {
                                selectedObjet += "_" + obj.Code;
                            }
                        }
                        if (!string.IsNullOrEmpty(selectedObjet))
                        {
                            model.ObjetRisqueCode = rsq.Code.ToString() + ";" + selectedObjet.Substring(1);
                            model.ObjetRisque = "à " + countSelectedObjet + " objets du risque " + rsq.Code + " '" + rsq.Descriptif + "'";
                        }
                    }
                }
            }
            else
            {
                foreach (ObjetDto obj in formuleDto.Risques[0].Objets)
                {
                    selectedObjet += "_" + obj.Code;
                }
                model.ObjetRisqueCode = formuleDto.Risques[0].Code + ";" + selectedObjet.Substring(1);
                if (formuleDto.Risques.Count() == 1)
                {
                    model.ObjetRisque = "à l'ensemble du risque";
                }
                else
                {
                    model.ObjetRisque = "au risque " + formuleDto.Risques[0].Code + " '" + formuleDto.Risques[0].Descriptif + "'";
                }
            }
        }

        #endregion
    }
}
