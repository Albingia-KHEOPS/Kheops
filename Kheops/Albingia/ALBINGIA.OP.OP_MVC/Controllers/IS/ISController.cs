using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IS;

namespace ALBINGIA.OP.OP_MVC.Controllers.IS
{
    public class ISController : Controller
    {
        public class ISModel
        {
            public bool OnlyPreview { get; set; } = false;
            private List<AffichageISLineDto> diplayListTitle;
            private List<KeyValuePair<string, string>> keyValuePairList;

            public Dictionary<string, string> Values { get; private set; }
            public List<ParamISLigneInfo> _lignesInfo { get; set; }
            public List<KeyValuePair<string, string>> _keyValuePairList
            {
                get => keyValuePairList; set
                {
                    keyValuePairList = value;
                    this.Values = value.ToDictionary(x => x.Key, x => x.Value);
                }
            }
            public string _splitString { get; set; }
            public string _cutString { get; set; }
            public string _strParameters { get; set; }
            public bool _paramErreur { get; set; }
            public List<AffichageISLineDto> _diplayListTitle
            {
                get => diplayListTitle; set
                {
                    diplayListTitle = value;
                    TitlesbyId = value?.ToDictionary(x => x.IdLigne) ?? new Dictionary<int, AffichageISLineDto>();
                }
            }

            public Dictionary<int, AffichageISLineDto> TitlesbyId { get; private set; }

            public AffichageISLineDto GetTitleById(int id) => TitlesbyId.TryGetValue(id, out var res) ? res : null;
            public String GetValueByName(string name) => Values.TryGetValue(name, out var res) ? res : String.Empty;
            public IEnumerable<ParamISLigneInfo> hiddenInfos => _lignesInfo.Where(x => x.TypeUIControl.Equals("Hidden"));
            public IEnumerable<ParamISLigneInfo> shownInfos => _lignesInfo.Where(x => !x.TypeUIControl.Equals("Hidden")).OrderBy(s => s.NumOrdreAffichage);

            public string branche { get; set; }
            public string section { get; set; }

            public IEnumerable<(string id, string label, bool selected)> GetDDLValues(ParamISLigneInfo ligneInfo)
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
                {
                    var wsIsData = chan.Channel;
                    if (!OnlyPreview)
                    {
                        List<DtoCommon> options;
                        if (!string.IsNullOrEmpty(ligneInfo.SqlRequest))
                        {
                            var parameters = DbIOParam.GetParams(_strParameters, _splitString);
                            var hParam = DbIOParam.PrepareParameter(parameters).ToArray();

                            options = wsIsData.GetDropdownlist(ligneInfo.SqlRequest, hParam);
                        }
                        else
                        {
                            options = new List<DtoCommon>();
                        }


                        return options.Select(option => (option.Code, option.Libelle, (option.Code == GetValueByName(ligneInfo.InternalPropertyName))));

                    }
                    return Enumerable.Empty<(string, string, bool)>();
                }
            }


        }

        // GET: IS
        public ActionResult IS(
            string modeNavig,
            string codeObjet,
            string codeRisque,
            string codeFormule,
            string codeOption,
            string etapeIs,
            string codeOffre,
            string version,
            string type,
            string branche,
            string section,
            string cible,
            string additionalParams,
            string splitChars,
            string strParameters)
        {

            var idModele = DbIOParam.PrepareIsIdModele(branche, section);
            var parameters = DbIOParam.GetParams(HttpUtility.UrlDecode(strParameters), HttpUtility.UrlDecode(splitChars));


            var paramForGenIs = DbDataEntity.GetParamGen(etapeIs, idModele, codeRisque, codeObjet, codeFormule, codeOption);
            if (CacheIS.AllISEnteteModelesDto == null)
            {
                return new EmptyResult();
            }

            List<ModeleISDto> isModeleEntete = null;// MvcApplication.AllISEnteteModelesDto.FindAll(el => el.NomModele.ToLower() == idModele.ToLower());
            CacheIS.AllISEnteteModelesDto.ForEach(elm =>
            {
                if (isModeleEntete == null)
                {
                    isModeleEntete = new List<ModeleISDto>();
                }

                if (elm.NomModele.ToLower() == idModele.ToLower())
                    isModeleEntete.Add(elm);
            });

            // Test si on est en mode Historique et aucune donnée n'existe pour l'is alors pas de HTML à générer
            if (modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique)
            {
                if (!DbDataEntity.RowsExists(section, parameters, isModeleEntete))
                    return new EmptyResult();
            }
            var dataToMap = DbDataEntity.GetDbData(modeNavig, codeObjet, codeRisque, codeFormule, codeOption, etapeIs, codeOffre, version, type, idModele, HttpUtility.UrlDecode(section), parameters, paramForGenIs, idModele.ToLower(), isModeleEntete);
            //Appel de la méthode de génèration HTML

            List<ParamISLigneInfo> dbLigneInfo = DbIOParam.GetControlsFromDB(idModele).ParamISDBLignesInfo;
            if (dbLigneInfo == null || !dbLigneInfo.Any())
            {
                return new EmptyResult();
            }


            if (dataToMap == string.Empty)
            {
                if (dbLigneInfo.Any(it => !string.IsNullOrEmpty(it.InternalPropertyName)))
                    dataToMap = DbDataEntity.GetDbDefaultData(dbLigneInfo.Where(it => !string.IsNullOrEmpty(it.InternalPropertyName)).ToList(), MvcApplication.SPLIT_CONST_HTML, "||");
            }

            ISModel model = new ISModel();
            model._lignesInfo = new List<ParamISLigneInfo>(dbLigneInfo);
            model._splitString = MvcApplication.SPLIT_CONST_HTML;
            model._cutString = "||";
            model._strParameters = strParameters;
            model._keyValuePairList = new List<KeyValuePair<string, string>>();
            model._keyValuePairList = dataToMap.Split(new[] { model._splitString }, StringSplitOptions.None).Select(Cut(model._cutString)).ToList();
            model.branche = branche;
            model.section = section;

            bool _paramErreur;
            if (modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique)
                return new EmptyResult();

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var wsIsInfo = channelClient.Channel;
                model._diplayListTitle = wsIsInfo.GetISDisplayConditions(codeOffre, type, version, idModele, paramForGenIs, out _paramErreur);
            }

            if (model._diplayListTitle != null && model._lignesInfo != null && !model._lignesInfo.Exists(el => el.HierarchyOrder == 1 && model._diplayListTitle.Exists(elm => elm.IdLigne == el.Code && elm.Afficher == "O")))
            {
                return new EmptyResult();
            }
            if (model._diplayListTitle != null && model._lignesInfo != null && !model._lignesInfo.Exists(el => el.HierarchyOrder == 2 && model._diplayListTitle.Exists(elm => elm.IdLigne == el.Code && elm.Afficher == "O")))
            {
                return new EmptyResult();
            }

            if (model._diplayListTitle != null)
            {
                var titlesHide = model._diplayListTitle.FindAll(el => el.Afficher.ToUpper() == "N");
                if (titlesHide != null && titlesHide.Count == model._diplayListTitle.Count)
                    return new EmptyResult();
            }

            //return new GenerationDbHTML(HttpUtility.UrlDecode(branche), new List<ParamISLigneInfo>(dbLigneInfo),
            //dataToMap, MvcApplication.SPLIT_CONST_HTML, "||", codeOffre, version, type, paramForGenIs, idModele, HttpUtility.UrlDecode(strParameters), modeNavig).Generate(HttpUtility.UrlDecode(branche),
            //HttpUtility.UrlDecode(section));

            return View();
        }

        private static Func<string, KeyValuePair<string, string>> Cut(string _cutString)
        {
            return x =>
            {
                var t = x.Split(new[] { _cutString }, StringSplitOptions.None);
                if (t.Length != 2) throw new Exception("Chaîne de pairs clé-valeur malformée");
                return new KeyValuePair<string, string>(t[0], t[1]);
            };
        }
    }
}