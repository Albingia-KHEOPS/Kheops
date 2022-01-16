using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using ALBINGIA.OP.OP_MVC.WSExcelData;
using ALBINGIA.Framework.Common.Tools;
using LigneInfo = ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo;

namespace ALBINGIA.OP.OP_MVC.ExcelWeb.Common
{
  public class GenerationHTML
  {
    #region Variables membres

    /// <summary>
    /// Liste des lignes infos
    /// </summary>
    private readonly List<ALBINGIA.Framework.Common.ExcelXmlMap.LigneInfo> _lignesInfo;
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

    #endregion

    #region Constructeur

    public GenerationHTML(string nom, List<Framework.Common.ExcelXmlMap.LigneInfo> lignesInfo, string keyValueString, string splitString, string cutString, string strParameters)
    {
      _lignesInfo = lignesInfo;
      _splitString = splitString;
      _cutString = cutString;
      _strParameters = strParameters;
      _keyValuePairList = new List<KeyValuePair<string, string>>();
      SplitKeyValueString(keyValueString);
    }
    #endregion

    #region Méthodes publiques

    /// <summary>
    /// Génération de code HTML
    /// </summary>
    /// <returns></returns>
    public string Generate(string branche, string section, bool onlyPreview = false)
    {
      var tables = new List<HtmlTable>();

      //Récupération du contenu du fichier Template
      var html = "##HIDDEN##\n##HTML##";

      //Remplacement des hidden inputs
      var hiddenInputs = string.Empty;
      hiddenInputs += "<div>";
      hiddenInputs = _lignesInfo.FindAll(x => x.TypeUIControl.Equals("Hidden")).Aggregate(hiddenInputs, (current, ligneInfo) => current + ("<input id='map_" + ligneInfo.Lib + "' type='hidden' value='" + GetValue(ligneInfo.Lib) + "' />"));

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

      foreach (var ligneInfo in _lignesInfo.FindAll(x => !x.TypeUIControl.Equals("Hidden")))
      {
        if (ligneInfo.HierarchyOrder == 1)
        {
          table.Rows.Add(CreateTitre(ligneInfo,true));
        }
        if (ligneInfo.HierarchyOrder == 2)
        {
          table.Rows.Add(CreateTitre2(ligneInfo));
        }
        if (ligneInfo.HierarchyOrder == 3)
        {
          table.Rows.Add(CreateRow(ligneInfo, onlyPreview));
        }
        if (!ligneInfo.LineBreak.Equals("O")) continue;
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
      var exists = _keyValuePairList.Exists(x => x.Value.Equals(key));
      return exists ? _keyValuePairList.Find(x => x.Value.Equals(key)).Key : string.Empty;
    }
    #endregion

    #region Méthodes privées static

    /// <summary>
    /// Création d'un titre
    /// </summary>
    /// <param name="ligneInfo"></param>
    /// <param name="ancre">utilisatio d'un ancre</param>
    /// <returns></returns>
    private static HtmlTableRow CreateTitre(Framework.Common.ExcelXmlMap.LigneInfo ligneInfo, bool ancre=false)
    {
      using (var tr = new HtmlTableRow())
      {
        using (var td = new HtmlTableCell())
        {
          td.ColSpan = 2;
          td.Attributes.Add("class", "titre");
          if (ancre)
            td.InnerHtml = string.Format("<div id=\"lnk{0}\">{1}</div>", ligneInfo.Lib, ligneInfo.TextLabel);
            //td.InnerHtml = string.Format("<a id=\"lnk{0}\">{1}</a>", ligneInfo.Lib, ligneInfo.TextLabel);
          else
            td.InnerText=ligneInfo.TextLabel;
          tr.Cells.Add(td);
          return tr;
        }
      }
    }

    /// <summary>
    /// Création d'un titre2
    /// </summary>
    /// <param name="ligneInfo"></param>
    /// <returns></returns>
    private static HtmlTableRow CreateTitre2(Framework.Common.ExcelXmlMap.LigneInfo ligneInfo)
    {
      using (var tr = new HtmlTableRow())
      {
        using (var td = new HtmlTableCell())
        {
          td.ColSpan = 2;
          td.Attributes.Add("class", "titre2");
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
    /// <returns></returns>
    private HtmlTableRow CreateRow(Framework.Common.ExcelXmlMap.LigneInfo ligneInfo, bool onlyPreview = false)
    {
      using (var tr = new HtmlTableRow())
      {
        using (var td1 = new HtmlTableCell())
        {
          td1.Attributes.Add("style","width:300px");
          td1.InnerText = ligneInfo.TextLabel;
          tr.Cells.Add(td1);
          using (var td2 = new HtmlTableCell())
          {
            tr.Cells.Add(td2);
            var inputString = string.Empty;
            var disabled = (ligneInfo.Disabled.Equals("O") ? "disabled" : string.Empty);
            switch (ligneInfo.TypeUIControl)
            {
              case "Text": string cssClass = string.Empty;
                if (ligneInfo.Type.ToLower().Contains("int") || ligneInfo.Type.ToLower().Contains("double"))
                {
                  cssClass = "numerique inputSelect";
                }
                if (ligneInfo.Type.ToLower().Equals("decimal") || ligneInfo.Type.ToLower().Equals("single"))
                {
                  cssClass = "decimal inputSelect";
                }
                var valInputTxt = onlyPreview ? string.Empty : GetValue(ligneInfo.Lib).Replace(',', '.');
                inputString = "<input style=\"width:500px\" id='map_" + ligneInfo.Lib + "' " + ((!string.IsNullOrEmpty(cssClass)) ? string.Concat("class=", cssClass) : string.Empty) + " type='text' value='" + valInputTxt + "' albrequired='" + ligneInfo.Required + "' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled + " />"; break;
              case "Textarea":
                var valTxtArea = onlyPreview ? string.Empty : GetValue(ligneInfo.Lib);
                inputString = "<textarea class=\"isTxtArea\" rows=\"5\" cols=\"80\" id='map_" + ligneInfo.Lib + "' albrequired='" + ligneInfo.Required + "' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled + ">" + valTxtArea + "</textarea>"; break;
              case "Select":
                inputString = "<select class=\"inputSelect\" id='map_" + ligneInfo.Lib + "' albrequired='" + ligneInfo.Required + "'" + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled + ">";
                //Envoie SqlRequest au webservice
                using (var wsExcelData = new ExcelInfoSpecifClient())

                  // using (var gen = new BrancheRS())
                  if (!onlyPreview)
                  {
                    LibCodeDto[] options;
                    if (!string.IsNullOrEmpty(ligneInfo.SqlRequest))
                    {
                      var parameters = ExcelIOParam.GetParams(_strParameters, _splitString);
                      var hParam = ExcelIOParam.PrepareParameter(parameters);

                      options = wsExcelData.GetDropdownlist(ligneInfo.SqlRequest, hParam.ToArray());
                    }
                    else
                    {
                      options = new List<LibCodeDto>().ToArray();
                    }

                    inputString = options.Aggregate(inputString,
                                                    (current, option) =>
                                                    current +
                                                    ("<option value='" + option.Code + "' " +
                                                     ((option.Code == GetValue(ligneInfo.Lib))
                                                        ? "selected"
                                                        : string.Empty) + ">" + option.Libelle +
                                                     "</option>"));

                  }
                inputString += "</select>";
                break;
              case "Date":
                var date = DateTime.Now.ToShortDateString();
                if (!string.IsNullOrEmpty(onlyPreview ? string.Empty : GetValue(ligneInfo.Lib)))
                {
                  if (AlbConvert.ConvertIntToDate(int.Parse(GetValue(ligneInfo.Lib))) != null)
                  {
                    if (AlbConvert.ConvertIntToDate(int.Parse(GetValue(ligneInfo.Lib))).Value != null)
                    {
                      date = AlbConvert.ConvertIntToDate(int.Parse(GetValue(ligneInfo.Lib))).Value.ToShortDateString();
                    }
                  }
                }
                inputString = "<input id='map_" + ligneInfo.Lib + "' albrequired='" + ligneInfo.Required + "' class='datepicker' data-val='true' data-val-regex='La date doit suivre la forme 24/11/2030' data-val-regex-pattern='^\\d{2}/\\d{2}/\\d{4}$' type='text' value='" + date + "' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled + "/>"; break;
              case "Checkbox":
                var checkedInput = !onlyPreview && GetValue(ligneInfo.Lib).Equals("VRAI");
                inputString = "<input id='map_" + ligneInfo.Lib + "' albrequired='" + ligneInfo.Required + "' " + (checkedInput ? "checked" : string.Empty) + " type='checkbox' " + ((!string.IsNullOrEmpty(ligneInfo.LinkBehaviour)) ? "linkBehaviour='" + ligneInfo.LinkBehaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.Behaviour)) ? "behaviour='" + ligneInfo.Behaviour + "'" : "") + " " + ((!string.IsNullOrEmpty(ligneInfo.EventBehaviour)) ? "eventBehaviour='" + ligneInfo.EventBehaviour + "'" : "") + " " + disabled + "/>"; break;
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
    private static HtmlTable CreateTableMenuIs(IReadOnlyCollection<LigneInfo> ligneInfos)
    {
      if (ligneInfos == null || ligneInfos.Count <= 1)
        return null;
      using (var table = new HtmlTable())
      {
        table.Attributes.Add("name", "albISMenuTable");
        table.Attributes.Add("class", "nGradientSection FloatLeft");
        table.Attributes.Add("cellpadding", "10");

        foreach (var lineInfo in ligneInfos)
        {
          using (var tr = new HtmlTableRow())
          {
           
            using (var td = new HtmlTableCell())
            {

              td.Attributes.Add("id", value: lineInfo.Lib);
              td.Attributes.Add("class", value: "AlbNoWrap");
              td.Attributes.Add("style", value: "width:140px");
              //AlbNoWrap
              td.InnerHtml = string.Format("<span name=\"albISLink\" albISLink=\"lnk{0}\" title=\"{1}\" class=\"CursorPointer TxtLink\">{1}</span>", lineInfo.Lib, lineInfo.TextLabel);
              //td.InnerHtml = string.Format("<a href=\"#lnk{0}\" title=\"{1}\">{1}</a>", lineInfo.Lib, lineInfo.TextLabel);

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
        using (var textWriter = new HtmlTextWriter(stringWriter))
        {
          htmlTable.RenderControl(textWriter);
          textWriter.Flush();
          return stringWriter.ToString();
        }
      }
    }
    #endregion
  }
}