using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Common
{
    public class CacheModels
    {
        #region Constantes

        public const string CODE_OFFRE = "CodeOffre";
        public const string VERSION = "Version";
        public const string CODE_FORMULE = "CodeFormule";
        public const string TYPE = "Type";
        public const string CODE_OPTION = "CodeOption";
        public const string CODE_CIBLE = "CodeCible";
        public const string BRANCHE = "Branche";
        public const string LIB_FORMULE = "LibFormule";
        public const string ISCHECKED = "IsChecked";
        public const string ALBNIVEAU = "albNiveau";
        public const string CODE_ALPHA = "CodeAlpha";
        #endregion

        #region Propriétes
        public static string UserId
        {
            get
            {
                return AlbSessionHelper.ConnectedUser ?? string.Empty;
            }
        }
        #endregion
        #region Cache Offre



        /// <summary>
        /// Récupère l'objet offre MetaModel du cahche -- Test des Perf avant utilisation de cache (Récupération automatique de la BD) 
        /// </summary>
        /// <param name="offreId">numéro offre</param>
        /// <param name="offreVersion">version del'offre</param>
        /// <param name="type"></param>
        /// <param name="modeNavig"></param>
        /// <returns>retourne l'objet Offre</returns>
        public static Offre_MetaModel GetOffreFromCache(string offreId, int offreVersion, string type, ModeConsultation modeNavig = ModeConsultation.Standard)
        {
            if (string.IsNullOrEmpty(UserId))
                return null;
          
            return GetOffreFromDb(offreId, offreVersion, type, modeNavig);
        }

        /// <summary>
        /// Mis à jour de l'objet offre dans le cache
        /// </summary>
        /// <param name="offreMetaModel">Objet Offre Model</param>
        /// <param name="numeroOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        public static void SetOffreCache(Offre_MetaModel offreMetaModel, string numeroOffre, string version, string type)
        {
            if (string.IsNullOrEmpty(UserId))
                return;
            CacheDictionnaryObjetOffre.CacheInstanceObjetOffre.SetOffre(UserId, offreMetaModel, MvcApplication.SPLIT_CONST_HTML, numeroOffre, version, type);
        }


        /// <summary>
        /// Met à jour le cahe avec l'objet offre
        /// </summary>
        /// <param name="offreId">numéro offre</param>
        /// <param name="offreVersion">version</param>
        /// <param name="type"></param>
        /// <param name="modeNavig"></param>
        /// <returns>Objet Offre_MetaModel</returns>
        public static Offre_MetaModel GetOffreFromDb(string offreId, int offreVersion, string type, ModeConsultation modeNavig)
        {
            if (string.IsNullOrEmpty(UserId))
                return null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServices=client.Channel;
                var offreMetaModel = new Offre_MetaModel();
                offreMetaModel.LoadOffre(policeServices.OffreGetDto(offreId, offreVersion, type, modeNavig));
                CacheDictionnaryObjetOffre.CacheInstanceObjetOffre.SetOffre(UserId, offreMetaModel, MvcApplication.SPLIT_CONST_HTML, offreId, Convert.ToString(offreVersion), type);
                return offreMetaModel;
            }
        }
        #endregion
        #region CacheFormulesGaranties
        

        private static string GetParamNatGar(string carac, string nature, bool isChecked)
        {
            var paramNatGar = MvcApplication.AlbLstParamNatGar.FirstOrDefault(el => el.Caractere == carac && el.Nature == nature);
            if (paramNatGar != null)
            {
                return isChecked ? paramNatGar.NatureParamChecked : paramNatGar.NatureParamNoChecked;
            }
            return string.Empty;
        }
        #endregion

        #region Méthodes Privées
        private static ModeleVolet GetVolet(string guid, ModeleFormuleGarantie modeleFormuleGarantie)
        {
            return modeleFormuleGarantie.Volets.FirstOrDefault(el => el.GuidId == guid);
        }

        private static ModeleBloc GetBloc(string guid, ModeleVolet volet)
        {
            return volet.Blocs.FirstOrDefault(el => el.GuidId == guid);
        }
        private static ModeleModele GetModele(string guid, ModeleBloc bloc)
        {
            return bloc.Modeles.FirstOrDefault(el => el.GuidId == guid);
        }
        private static ModeleGarantieNiveau1 GetNiv1(string guid, ModeleModele modele)
        {
            return modele.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleGarantieNiveau2 GetNiv2(string guid, ModeleGarantieNiveau1 niv1)
        {
            return niv1.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleGarantieNiveau3 GetNiv3(string guid, ModeleGarantieNiveau2 niv2)
        {
            return niv2.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleGarantieNiveau4 GetNiv4(string guid, ModeleGarantieNiveau3 niv3)
        {
            return niv3 == null ? null : niv3.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }

        private static string GetParameter(IEnumerable<KeyValuePair<string, string>> parameters, string paramName)
        {
            return parameters.FirstOrDefault(el => el.Key == paramName).Value;
        }
        private static ModeleFormuleGarantie GetFormuleGarantieFromDb(string offreId, string offreVersion, string type, string codeAvn
            , int codeFormule, int codeOption, int formGen, string codeCible, string codeAlpha, string branche, string libFormule, int appliqueA, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var formGarDto = serviceContext.FormulesGarantiesGet(offreId.PadLeft(9, ' '),
                                                                             offreVersion,
                                                                             type, codeAvn,
                                                                             codeFormule.ToString(
                                                                                 CultureInfo.
                                                                                     InvariantCulture),
                                                                             codeOption.ToString(
                                                                                 CultureInfo.CurrentCulture),
                                                                             formGen.ToString(
                                                                                 CultureInfo.CurrentCulture),
                                                                             codeCible, codeAlpha,
                                                                             branche, libFormule,
                                                                             AlbSessionHelper.ConnectedUser, appliqueA, false, modeNavig.ParseCode<ModeConsultation>());

                var formuleGarantieDto = formGarDto.FormuleGarantie;
                if (formuleGarantieDto.Volets != null && formuleGarantieDto.Volets.Any())
                {
                    var modeleFormuleGarantie = new ModeleFormuleGarantie();
                    formuleGarantieDto.Volets.ToList().ForEach(m =>
                                                                   {
                                                                       var volet = (ModeleVolet)m;

                                                                       modeleFormuleGarantie.Volets.Add(volet);
                                                                   });
                    modeleFormuleGarantie.CodeOption = formuleGarantieDto.CodeOption;
                    return modeleFormuleGarantie;
                }
                return null;

            }
        }
        [Obsolete]
        private static ConditionRisqueGarantieGetResultDto GetConditionsGarantiesFromDb(string codeOffre, string version, string type, string codeAvn, int codeFormule, int codeOption, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var query = new ConditionRisqueGarantieGetQueryDto { NumeroOffre = codeOffre, version = version, Type = type, CodeFormule = codeFormule.ToString(), CodeOption = codeOption.ToString() };

                return serviceContext.ObtenirConditions(query, codeAvn, modeNavig.ParseCode<ModeConsultation>(), true);
            }
        }

        #endregion
    }
}


