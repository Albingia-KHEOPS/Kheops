using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Models.ModelesContextMenu;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque;
using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Common
{
    public static class InformationsGenerales
    {
        public static ModeleInformationsGeneralesRisque SetInfosGenerales(InformationGeneraleTransverse infos)
        {
            var toReturn = new ModeleInformationsGeneralesRisque { Descriptif = infos.Descriptif, DisplayInfosValeur = true, ListesNomenclatures = new ModeleInformationsGeneralesRisqueNomenclatures() };
            toReturn.ListesNomenclatures.Nomenclature1 = new ModeleListeNomenclatures() { NumeroCombo = "1" };
            toReturn.ListesNomenclatures.Nomenclature2 = new ModeleListeNomenclatures() { NumeroCombo = "2" };
            toReturn.ListesNomenclatures.Nomenclature3 = new ModeleListeNomenclatures() { NumeroCombo = "3" };
            toReturn.ListesNomenclatures.Nomenclature4 = new ModeleListeNomenclatures() { NumeroCombo = "4" };
            toReturn.ListesNomenclatures.Nomenclature5 = new ModeleListeNomenclatures() { NumeroCombo = "5" };

            if (infos.Cible != null)
            {
                toReturn.Cible = infos.Cible.Code;
            }
            if (infos.EntreeGarantie != null)
            {
                toReturn.DateEntreeGarantie = infos.EntreeGarantie;
                toReturn.HeureEntreeGarantie = new TimeSpan(infos.EntreeGarantie.Value.Hour, infos.EntreeGarantie.Value.Minute, 0);
            }
            if (infos.SortieGarantie != null)
            {
                toReturn.DateSortieGarantie = infos.SortieGarantie;
                toReturn.HeureSortieGarantie = new TimeSpan(infos.SortieGarantie.Value.Hour, infos.SortieGarantie.Value.Minute, 0);
            }
            if (infos.SortieGarantieHisto != null)
            {
                toReturn.DateSortieGarantieHisto = infos.SortieGarantieHisto;
                toReturn.HeureSortieGarantieHisto = new TimeSpan(infos.SortieGarantieHisto.Value.Hour, infos.SortieGarantieHisto.Value.Minute, 0);
            }
            toReturn.Valeur = infos.Valeur;
            if (infos.Unite != null)
            {
                toReturn.Unite = infos.Unite.Code;
            }
            if (infos.Type != null)
            {
                toReturn.Type = infos.Type.Code;
            }

            toReturn.ValeurHT = infos.ValeurHT;
            toReturn.CoutM2 = infos.CoutM2;

            toReturn.Designation = infos.Designation;

            if (infos.DateEntreeDescr != null)
            {
                toReturn.DateEntreeDescr = infos.DateEntreeDescr;
                toReturn.HeureEntreeDescr = infos.HeureEntreeDescr;
            }
            if (infos.DateSortieDescr != null)
            {
                toReturn.DateSortieDescr = infos.DateSortieDescr;
                toReturn.HeureSortieDescr = infos.HeureSortieDescr;
            }

            toReturn.IsRisqueTemporaire = infos.IsRisqueTemporaire;

            return toReturn;
        }
    }
    public class InformationGeneraleTransverse
    {
        public String Descriptif { get; set; }
        public String Designation { get; set; }
        public ParametreDto Cible { get; set; }
        public DateTime? EntreeGarantie { get; set; }
        public DateTime? SortieGarantie { get; set; }
        public DateTime? SortieGarantieHisto { get; set; }
        public Int64? Valeur { get; set; }
        public ParametreDto Unite { get; set; }
        public ParametreDto Type { get; set; }
        public string ValeurHT { get; set; }
        public Int64? CoutM2 { get; set; }
        public DateTime? DateEntreeDescr { get; set; }
        public TimeSpan? HeureEntreeDescr { get; set; }
        public DateTime? DateSortieDescr { get; set; }
        public TimeSpan? HeureSortieDescr { get; set; }
        public bool IsRisqueTemporaire { get; set; }

        public static List<ParamNatGarDto> GetNaturesParamsGaranties()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                return serviceContext.GetParamNatGar();
            }
        }

        /// <summary>
        ///   Liste des contextes menus de tous les utilisateurs non hiérarchisé
        /// </summary>
        /// <param name="appl">Code application dans GDM</param>
        /// <returns></returns>
        public static List<ModeleListItem> GetContextMenuFromDb(string appl)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
                var allUsersContextMenu = client.Channel.GetAllUsersContextMenu(appl);
                if (allUsersContextMenu == null) return null;
                var resAllUsersContextMenu = new List<ModeleListItem>();
                allUsersContextMenu.ForEach(userContextMenus => userContextMenus.UserContextMenu.ForEach(usrCtx =>
                {
                    var modeleListItem = (ModeleListItem)usrCtx;
                    modeleListItem.Utilisateur = userContextMenus.Utilisateur;
                    if (modeleListItem.items != null)
                    {
                        modeleListItem.items.ForEach(suItem => suItem.Utilisateur = userContextMenus.Utilisateur);
                    }
                    resAllUsersContextMenu.Add(modeleListItem);
                }
                  ));
                return resAllUsersContextMenu;
            }
        }

        public static List<ModeleListItem> GetFlatContextMenu(string appl)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var flatList = serviceContext.GetAllUsersFlatContextMenu(appl);
                if (flatList == null) return null;
                if (flatList != null && flatList.Count > 0)
                {
                    var resAllUsersContextMenu = new List<ModeleListItem>();
                    flatList.ForEach(elm =>
                    {
                        var modeleListItem = (ModeleListItem)elm;
                        resAllUsersContextMenu.Add(modeleListItem);
                    });
                    return resAllUsersContextMenu;
                }
            }
            return null;
        }


        public static explicit operator InformationGeneraleTransverse(ObjetDto obj)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ObjetDto, InformationGeneraleTransverse>().Map(obj);
        }

        public static explicit operator InformationGeneraleTransverse(RisqueDto obj)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RisqueDto, InformationGeneraleTransverse>().Map(obj);
        }

        
        #region paramètre avenants
        public static string GetAddParamValue(string addParamValue, AlbParameterName nameVariable)
        {
            if (!String.IsNullOrEmpty(addParamValue) && !addParamValue.StartsWith("|"))
                addParamValue = string.Format("|{0}", addParamValue);

            string[] tParams = !string.IsNullOrEmpty(addParamValue) ? addParamValue.Split(new[] { "|" + nameVariable.ToString() + "|" }, 2 , StringSplitOptions.None) : null;

            if (tParams == null || tParams.Length != 2)
                return string.Empty;

            if (tParams[1].StartsWith("|"))
                return string.Empty;

            string[] values = tParams[1].Split(new[] { "|" }, 2, StringSplitOptions.None);

            return values[0];
        }

        public static void RemoveKeyFromAddParamValue(ref string param, AlbParameterName key)
        {
            SetAddParamValue(ref param, key);
        }

        public static void SetAddParamValue(ref string param, AlbParameterName key, string value = null)
        {
            if (string.IsNullOrEmpty(param) && value != null)
            {
                param = key + "|" + value;
                return;
            }


            var endStringPos = param.IndexOf("addParamtabGuid");
            var endString = "";

            if (param.Contains("addParamtabGuid"))
            {
                endString = param.Substring(endStringPos);
                param = param.Replace(endString, "");
            }

            


            var startStringPos = param.IndexOf("addParam");
            var startString = "";

            if (param.Contains("addParamtabGuid"))
            {
                startString = param.Substring(0, startStringPos + "addParam".Length);
                param = param.Replace(startString, "");
            }

            

            if (param.IndexOf("||") != 0 && !string.IsNullOrEmpty(param))
            {
                if (param.IndexOf("|") == 0)
                    param = "|" + param;
                else
                    param = "||" + param;
            }

            var spacedParams = param;
            spacedParams = spacedParams.Replace("|||", "| ||");

            if (spacedParams.Contains("||" + key + "|"))
            {
                string[] splitChar = { "||" };

                var splitedParamExceptKey = spacedParams.Split(splitChar, StringSplitOptions.None).Where(kV => !kV.StartsWith(key + "|"));

                spacedParams = String.Join("||", splitedParamExceptKey);
            }

            param = spacedParams.Replace("| ||", "|||");

            if (value != null)
                param += "||" + key + '|' + value;

            if (param.IndexOf("||") == 0)
                param = param.Substring(2);

            param = startString + param + endString;
        }

        public static string GetScreenType(string strProvider)
        {
            if (string.IsNullOrEmpty(strProvider)) {
                return string.Empty;
            }
            var param = strProvider;

            // Suppression du paramètre de modeNavig
            var paramPolice = param.Contains("modeNavig") ? param.Split(new[] { "modeNavig" }, StringSplitOptions.None)[0] : param;
            
            //Suppression du paramètre de addParam
            param = param.Contains("addParam") ? param.Split(new[] { "addParam" }, StringSplitOptions.None)[1] : string.Empty;
            var lstTypePolice = paramPolice.Split(new[] { "_" }, StringSplitOptions.None);
            if (lstTypePolice.Count() < 3) {
                return string.Empty;
            }
            var paramTypePolice = paramPolice.Split('_')[2][0].ToString();

            if (paramTypePolice.ToLower() == "o")
            {
                return AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
            }
            if (string.IsNullOrEmpty(param))
            {
                if (paramTypePolice.ToLower() == "p")
                    return AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                return string.Empty;
            }
            var tParam = HttpUtility.UrlDecode(param);
            var lstScreenType = tParam.Split(new[] { "|||" }, StringSplitOptions.None);
            if (lstScreenType.Count() < 2) {
                return string.Empty;
            }
            var typeScreen = GetAddParamValue(tParam.Split(new[] { "|||" }, StringSplitOptions.None)[1], AlbParameterName.AVNTYPE);

            switch (typeScreen)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    return AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    return AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR :
                    return AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                default:
                    return AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
            }

        }
        #endregion
    }



}


