using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.ParamIS;
using OPServiceContract.IS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common
{
    public class GenerationDbHTML
    {

        #region Variables membres

        /// <summary>
        /// Liste des lignes infos
        /// </summary>
        private readonly List<ParamISLigneInfo> _lignesInfo;
        /// <summary>
        /// Liste des valeurs
        /// </summary>
        private readonly List<KeyValuePair<string, string>> _keyValuePairList;

        /// <summary>
        /// Caractères de sépration entre les pairs clé-valeur
        /// </summary>
        private readonly string _splitString;

        /// <summary>
        /// Caractères de séparation entre clé et valeur
        /// </summary>
        private readonly string _cutString;
        /// <summary>
        /// paramètres de la requête
        /// </summary>
        private readonly string _strParameters;
        /// <summary>
        /// paramètre de retour d'erreur
        /// </summary>
        private readonly bool _paramErreur;

      /// <summary>
      /// Liste des titres à afficher
      /// </summary>
        private static List<AffichageISLineDto> _diplayListTitle;

        #endregion

        #region Constructeur

        public GenerationDbHTML(string nom, List<ParamISLigneInfo> lignesInfo, string keyValueString
          , string splitString, string cutString, string codeOffre, string version, string type
          , ParametreGenClauseDto paramConditionnement,string idModele, string strParameters, string modeNavig)
        {
            _lignesInfo = lignesInfo;
            _splitString = splitString;
            _cutString = cutString;
            _strParameters = strParameters;
            _keyValuePairList = new List<KeyValuePair<string, string>>();
            SplitKeyValueString(keyValueString);
            _paramErreur = false;
            if (modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique)
                return;
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var wsIsInfo=channelClient.Channel;
              _diplayListTitle = wsIsInfo.GetISDisplayConditions(codeOffre, type, version, idModele, paramConditionnement, out _paramErreur);
            }
        }
        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Génération de code HTML
        /// </summary>
        /// <returns></returns>
        public string Generate(string branche, string section, bool onlyPreview = false)
        {
         
           
            if (_diplayListTitle != null && _lignesInfo != null && !_lignesInfo.Exists(el => el.HierarchyOrder == 1 && _diplayListTitle.Exists(elm => elm.IdLigne == el.Code && elm.Afficher == "O")))
            {
                return string.Empty; 
            }
            if (_diplayListTitle != null && _lignesInfo != null && !_lignesInfo.Exists(el => el.HierarchyOrder == 2 && _diplayListTitle.Exists(elm => elm.IdLigne == el.Code && elm.Afficher == "O")))
            {
                return string.Empty;
            }

            if (_diplayListTitle != null )
          {
            var titlesHide = _diplayListTitle.FindAll(el => el.Afficher.ToUpper() == "N");
            if (titlesHide != null && titlesHide.Count == _diplayListTitle.Count)
              return string.Empty;
          }
         
          var tables = new List<HtmlTable>();

            //Récupération du contenu du fichier Template
            var html = "##HIDDEN##\n##HTML##";

            //Remplacement des hidden inputs
            var hiddenInputs = string.Empty;
            hiddenInputs += "<div>";
            hiddenInputs = _lignesInfo.FindAll(x => x.TypeUIControl.Equals("Hidden")).Aggregate(hiddenInputs, (current, ligneInfo) => current + ("<input id='map_" + ligneInfo.InternalPropertyName + "' type='hidden' value='" + GetValue(ligneInfo.InternalPropertyName) + "' />"));

            //Branche
            hiddenInputs += "<input id='branche' type='hidden' value='" + branche + "' />";

            //Section
            hiddenInputs += "<input id='section' type='hidden' value='" + section + "' />";

            //SplitString
            hiddenInputs += "<input id='jsSplitChar' type='hidden' value='" + _splitString + "' />";

            //Parameters
            hiddenInputs += "<input id='parameters' type='hidden' value='" + _strParameters + "' />";

            hiddenInputs += "</div>";
            html = html.Replace("##HIDDEN##", hiddenInputs);

            var lnInfoNiveau1 = _lignesInfo.FindAll(el => el.HierarchyOrder == 1);

            //Creation de la table de menu IS
            var tableMenu = CreateTableMenuIs(lnInfoNiveau1);

            // Création de la table des IS
            var table = CreateTable();
            tables.Add(table);
            int codeTitre1 = 0;
            int codeTitre2 = 0;
          bool display = true;
            foreach (var ligneInfo in _lignesInfo.FindAll(x => !x.TypeUIControl.Equals("Hidden")).OrderBy(s => s.NumOrdreAffichage ))
            {
                if (ligneInfo.HierarchyOrder == 1)
                {
                  codeTitre1 = ligneInfo.Code;
                  var elmCondition = _diplayListTitle==null?null:_diplayListTitle.FirstOrDefault(el => el.IdLigne == codeTitre1);
                  display = elmCondition == null || elmCondition.Afficher.ToUpper() == "O";
                  table.Rows.Add(CreateTitre(ligneInfo, display,true));
                }
                if (ligneInfo.HierarchyOrder == 2)
                {
                  codeTitre2 = ligneInfo.Code;
                    var vTitre1 = _diplayListTitle != null ? _diplayListTitle.FirstOrDefault(el => el.IdLigne == codeTitre1) : null;
                  var elmCondition = _diplayListTitle == null ? null : _diplayListTitle.FirstOrDefault(el => el.IdLigne == codeTitre2);
                  display = (vTitre1 == null || vTitre1.Afficher.ToUpper() == "O") && (elmCondition == null || (elmCondition.Afficher.ToUpper() == "O"));
                  table.Rows.Add(CreateTitre2(ligneInfo, display));
                }
                if (ligneInfo.HierarchyOrder == 3)
                {
                  table.Rows.Add(CreateRow(ligneInfo, onlyPreview, display));
                }
                if (ligneInfo.LineBreak <= 1) continue;
                table = CreateTable();
                tables.Add(table);
            }

            // Ajout des tables Menu et IS à la table globale
            var inputs = tables.Aggregate(string.Empty, (current, htmlTable) => current + HtmlTableToString(htmlTable));

            if (tableMenu == null)
            {
                html = html.Replace("##HTML##", inputs);

            }
            else
            {
                ////Création de la table globale
                var globalTable = CreateGlobalTable();

                globalTable.Rows[0].Cells[0].InnerHtml = string.Format("<div id=\"dvIsMenu\" style=\"position:absolute;width:160px\">{0}</div>", HtmlTableToString(tableMenu));
                globalTable.Rows[0].Cells[1].InnerHtml = string.Format("<table><tr><td>{0}</td></tr></table>", inputs);

                html = html.Replace("##HTML##", HtmlTableToString(globalTable));
            }
            return html;
        }

        #endregion


        #region Méthodes privées
        /// <summary>
        /// Split une chaine de caractère en KeyValuePair
        /// </summary>
        /// <param name="keyValueString"></param>
        private void SplitKeyValueString(string keyValueString)
        {
            if (string.IsNullOrEmpty(keyValueString))
                return;
            var splitArray = keyValueString.Split(new[] { _splitString }, StringSplitOptions.None);
            foreach (var split in splitArray)
            {
                if (!string.IsNullOrEmpty(split))
                {
                    var splitKeyValue = split.Split(new[] { _cutString }, StringSplitOptions.None);
                    if (splitKeyValue.Length == 2)
                    {
                        _keyValuePairList.Add(new KeyValuePair<string, string>(splitKeyValue[0], splitKeyValue[1]));
                    }
                    else
                    {
                        throw new Exception("Chaîne de pairs clé-valeur malformée");
                    }
                }
            }
        }

        /// <summary>
        /// Récupération de la valeur correspondante à une clé dans une chaine de caractère
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetValue(string key)
        {
            var exists = _keyValuePairList.Exists(x => x.Key.Equals(key));
            return exists ? _keyValuePairList.Find(x => x.Key.Equals(key)).Value : string.Empty;
        }

        #endregion
        #region Méthodes privées static

      /// <summary>
      /// Création d'un titre
      /// </summary>
      /// <param name="ligneInfo"></param>
      /// <param name="display"></param>
      /// <param name="ancre">utilisatio d'un ancre</param>
      /// <returns></returns>
      private static HtmlTableRow CreateTitre(ParamISLigneInfo ligneInfo,bool display, bool ancre = false)
        {
            using (var tr = new HtmlTableRow())
            {
              if (!display)
                tr.Attributes.Add("class", "None"); 
              using (var td = new HtmlTableCell())
                {
                    
                  
                  td.ColSpan = 2;
                  td.Attributes.Add("class", string.Format("titreIS{0}", !display ? " None" : string.Empty));
                    if (ancre && display)
                        td.InnerHtml = string.Format("<div id=\"lnk{0}\">{1}</div>", ligneInfo.InternalPropertyName, ligneInfo.TextLabel);
                    else
                        td.InnerText = ligneInfo.TextLabel;
                    tr.Cells.Add(td);
                    return tr;
                }
            }
        }

      /// <summary>
      /// Création d'un titre2
      /// </summary>
      /// <param name="ligneInfo"></param>
      /// <param name="display"></param>
      /// <returns></returns>
      private static HtmlTableRow CreateTitre2(ParamISLigneInfo ligneInfo, bool display)
        {
            using (var tr = new HtmlTableRow())
            {
              if (!display)
                tr.Attributes.Add("class", "None");  
              using (var td = new HtmlTableCell())
                {
                    td.ColSpan = 2;
                    td.Attributes.Add("class", string.Format("titre2{0}", !display ? " None" : string.Empty));
                    td.InnerText = ligneInfo.TextLabel;
                    tr.Cells.Add(td);
                    return tr;
                }
            }

        }

      /// <summary>
      /// Création d'une ligne de formulaire
      /// </summary>
      /// <param name="ligneInfo"></param>
      /// <param name="onlyPreview"></param>
      /// <param name="display"></param>
      /// <returns></returns>
      private HtmlTableRow CreateRow(ParamISLigneInfo ligneInfo, bool onlyPreview = false,bool display=true)
        {
            using (var tr = new HtmlTableRow())
            {
              if(!display) 
                tr.Attributes.Add("class","None");
              using (var td1 = new HtmlTableCell())
                {
                    td1.Attributes.Add("style", "width:300px");
                  td1.InnerText = display?ligneInfo.TextLabel:string.Empty;
                    tr.Cells.Add(td1);
                   var required = string.Format("{0}",display ? " albrequired='" + ligneInfo.Required + "'" : string.Empty);
                    using (var td2 = new HtmlTableCell())
                    {
                        tr.Cells.Add(td2);
                        var inputString = string.Empty;
                        var disabled = (ligneInfo.Disabled.Equals("N") ? "disabled" : string.Empty);
                        var maxLength=" maxlength='"+ligneInfo.LongueurType+"' " ;
                      var numMask=string.Empty;
                       
                        switch (ligneInfo.TypeUIControl)
                        {
                            case "Text":
                            string initHiddenValue = string.Empty;
                            string cssClass = string.Empty;
                            if ((ligneInfo.Type.ToLower().Contains("double") || ligneInfo.Type.ToLower().Contains("decimal") || ligneInfo.Type.ToLower().Contains("single")) && ligneInfo.LongueurType.Contains(":"))
                            {
                              var valLng = ligneInfo.LongueurType.Split(':');
                              int valLengthEnt;
                              int valLengthDec;
                             
                              numMask = " albMask= 'decimal' albEntier='" + (int.TryParse(valLng[0], out valLengthEnt)
                                                ? valLengthEnt
                                                : 10) + "' albDecimal='" + (int.TryParse(valLng[1], out valLengthDec) ? valLengthDec : 2) + "'";
                              maxLength = string.Empty;
                              initHiddenValue = "0";
                            }
                              if (ligneInfo.Type.ToLower().Contains("int"))
                                {
                                cssClass = string.Format("'numerique inputSelect {0}'",!display?"None":string.Empty);
                                  int valLengthEnt;
                                  numMask = " albMask= 'numeric' albEntier='" +
                                            (int.TryParse(ligneInfo.LongueurType, out valLengthEnt)
                                               ? valLengthEnt
                                               : 10) + "' ";
                                  initHiddenValue = "0";
                                }
                              if (ligneInfo.Type.ToLower().Equals("double") || ligneInfo.Type.ToLower().Equals("decimal") || ligneInfo.Type.ToLower().Equals("single"))
                                {
                                cssClass = string.Format("'isDecimal inputSelect{0}'",!display?" None":string.Empty);
                                }
                            if (!display)
                              cssClass = !cssClass.Contains("None") ? cssClass + " None" : cssClass;

                              var valInputTxt = onlyPreview ? string.Empty : display ? GetValue(ligneInfo.InternalPropertyName).Replace(',', '.') : initHiddenValue;
                                
                              inputString = "<input " + required + " style=\"width:500px\" id='map_" + ligneInfo.InternalPropertyName + "' " + ((!string.IsNullOrEmpty(cssClass)) ? string.Concat("class=", cssClass) : string.Empty) + " type='text' value='" + valInputTxt + "' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + disabled + maxLength + numMask + " " + "/>"; 
                                break;
                            case "TextArea":
                            var txtAreaClass = string.Format("class=\"isTxtArea{0}\" ",!display?" None":string.Empty);
                              var valTxtArea = onlyPreview ? string.Empty : display?GetValue(ligneInfo.InternalPropertyName):string.Empty;
                                inputString = "<textarea "+ required +" "+txtAreaClass+" rows=\"5\" cols=\"80\" id='map_" + ligneInfo.InternalPropertyName+"'" + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled +" "+ maxLength +">" + valTxtArea + "</textarea>"; break;
                            case "Select":
                              var selectClass = string.Format("class=\"inputSelect{0}\" ",!display?" None":string.Empty);
                              inputString = "<select " + required + " id='map_" + ligneInfo.InternalPropertyName + "' " + selectClass + " " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled + "'>";
                                //Envoie SqlRequest au webservice
                                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
                                {
                                    var wsIsData = chan.Channel;


                                    if (!onlyPreview)
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


                                        inputString = options.Aggregate(inputString,
                                                                        (current, option) =>
                                                                        current +
                                                                        ("<option value='" + option.Code + "' " +
                                                                         ((option.Code == GetValue(ligneInfo.InternalPropertyName))
                                                                            ? "selected"
                                                                            : string.Empty) + ">" + option.Libelle +
                                                                         "</option>"));

                                    }
                                    inputString += "</select>";
                                }
                                break;
                            case "Date":
                                //var date = DateTime.Now.ToShortDateString();
                                var date = string.Empty;
                                if (!string.IsNullOrEmpty(onlyPreview ? string.Empty : GetValue(ligneInfo.InternalPropertyName)))
                                {
                                    if (AlbConvert.ConvertIntToDate(int.Parse(GetValue(ligneInfo.InternalPropertyName))) != null)
                                    {
                                        if (AlbConvert.ConvertIntToDate(int.Parse(GetValue(ligneInfo.InternalPropertyName))).Value != null)
                                        {
                                            date = AlbConvert.ConvertIntToDate(int.Parse(GetValue(ligneInfo.InternalPropertyName))).Value.ToShortDateString();
                                        }
                                    }
                                }
                            var dateClass = string.Format("class='datepicker{0}'",!display?" None":"'");
                                inputString = "<input "+required+" id='map_" + ligneInfo.InternalPropertyName  + "' "+dateClass+" data-val='true' data-val-regex='La date doit suivre la forme 24/11/2030' data-val-regex-pattern='^\\d{2}/\\d{2}/\\d{4}$' type='text' value='" + date + "' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled +"/>"; break;
                            case "Checkbox":
                              var chekClass= string.Format("{0}",!display?"class='None' ":string.Empty); 
                              var checkedInput = !onlyPreview && display && GetValue(ligneInfo.InternalPropertyName).Equals("VRAI");
                                inputString = "<input "+required+" "+chekClass+" id='map_" + ligneInfo.InternalPropertyName + "' "+disabled + (checkedInput ? " checked" : string.Empty) + " type='checkbox' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "")  +"/>"; break;
                        }
                        td2.InnerHtml = inputString;
                        return tr;
                    }
                }
            }
        }

        /// <summary>
        /// Création d'une table
        /// </summary>
        /// <returns></returns>
        private static HtmlTable CreateTable()
        {
            using (var table = new HtmlTable())
            {
                table.Attributes.Add("name", "albISTable");
                table.Attributes.Add("class", "nGradientSection FloatLeft Padding");
                table.Attributes.Add("cellpadding", "10");
                return table;
            }
        }
        /// <summary>
        /// Création de la table Html globale
        /// </summary>
        /// <returns></returns>
        private static HtmlTable CreateGlobalTable()
        {
            using (var table = new HtmlTable())
            {
                table.Attributes.Add("name", "albISGlobalTable");
                table.Attributes.Add("class", "nGradientSection FloatLeft Padding");
                table.Attributes.Add("cellpadding", "10");
                using (var tr = new HtmlTableRow())
                {
                    tr.Attributes.Add("valign", "top");
                    for (var i = 0; i < 2; i++)
                    {

                        using (var td = new HtmlTableCell())
                        {
                            if (i == 0)
                            {

                                td.Attributes.Add("style", "width:150px");
                            }
                            td.Attributes.Add("id", value: "globalTable" + (i + 1).ToString(CultureInfo.CurrentCulture));
                            tr.Cells.Add(td);
                        }
                        table.Rows.Add(tr);
                    }
                }

                return table;
            }
        }


        /// <summary>
        /// Création de la table Html Menu IS
        /// </summary>
        /// <returns></returns>
        private static HtmlTable CreateTableMenuIs(IReadOnlyCollection<ParamISLigneInfo> ligneInfos)
        {
          if (_diplayListTitle == null)
            return null;
          var lngCondition = _diplayListTitle.FindAll(el => el.Afficher.ToUpper() == "O");
          if (ligneInfos == null || ligneInfos.Count <= 1 || (lngCondition != null && lngCondition.Count<=1))
                return null;
            using (var table = new HtmlTable())
            {
                table.Attributes.Add("name", "albISMenuTable");
                table.Attributes.Add("class", "nGradientSection FloatLeft");
                table.Attributes.Add("cellpadding", "10");

                foreach (var lineInfo in ligneInfos)
                {
                  var lineToDisplay = _diplayListTitle.FirstOrDefault(el => el.IdLigne == lineInfo.Code);
                  if(lineToDisplay!=null && lineToDisplay.Afficher.ToUpper()=="N")
                    continue;
                  
                  using (var tr = new HtmlTableRow())
                    {

                        using (var td = new HtmlTableCell())
                        {

                            td.Attributes.Add("id", value: lineInfo.InternalPropertyName);
                            td.Attributes.Add("class", value: "AlbNoWrap");
                            td.Attributes.Add("style", value: "width:140px");
                            //AlbNoWrap
                            td.InnerHtml = string.Format("<span name=\"albISLink\" albISLink=\"lnk{0}\" title=\"{1}\" class=\"CursorPointer TxtLink\">{1}</span>", lineInfo.InternalPropertyName, lineInfo.TextLabel);
                            

                            tr.Cells.Add(td);
                        }
                        table.Rows.Add(tr);
                    }
                }
                return table;
            }
        }

        /// <summary>
        /// Récupération du code html à partir d'une HtmlTable
        /// </summary>
        /// <param name="htmlTable"></param>
        /// <returns></returns>
        private static string HtmlTableToString(HtmlTable htmlTable)
        {
            using (var stringWriter = new StringWriter())
            {
              var textWriter = new HtmlTextWriter(stringWriter);     
              htmlTable.RenderControl(textWriter);
                    textWriter.Flush();
                    return stringWriter.ToString();
            }
        }
        #endregion

    }
}