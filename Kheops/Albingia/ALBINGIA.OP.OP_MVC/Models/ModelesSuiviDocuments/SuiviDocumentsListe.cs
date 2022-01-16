using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.SuiviDocuments;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments
{
    public class SuiviDocumentsListe
    {
        public List<SuiviDocumentsLot> SuiviDocumentsListeLot { get; set; }
        public List<SuiviDocumentsPlat> SuiviDocumentsPlat { get; set; }
        public Int32 CountLine { get; set; }
        public Int32 PageNumber { get; set; }
        public Int32 StartLine { get; set; }
        public Int32 EndLine { get; set; }


        public static explicit operator SuiviDocumentsListe(SuiviDocumentsListeDto modelDto)
        {
            var result = ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsListeDto, SuiviDocumentsListe>().Map(modelDto);
            if (result.SuiviDocumentsPlat != null)
            {
                foreach (var item in result.SuiviDocumentsPlat)
                {
                    string text = string.Empty;
                    string value = !string.IsNullOrEmpty(item.CodeSituation) ? AlbEnumInfoValue.GetEnumInfoSplit((AlbConstantesMetiers.EditionSituations)Enum.Parse(typeof(AlbConstantesMetiers.EditionSituations), item.CodeSituation), out text) : string.Empty;
                    item.LibSituation = text.Trim();

                    item.DateSit = AlbConvert.ConvertIntToDate(item.DateSituation); ;
                    item.DateCreateDoc = AlbConvert.ConvertIntToDate(item.CreateDoc);
                    item.DateModifDoc = AlbConvert.ConvertIntToDate(item.UpdateDoc);
                    item.Situation = (AlbConstantesMetiers.EditionSituations)Enum.Parse(typeof(AlbConstantesMetiers.EditionSituations), item.CodeSituation);
                    item.LinkSituation = /*item.Situation == AlbConstantesMetiers.EditionSituations.A || item.Situation == AlbConstantesMetiers.EditionSituations.N ||*/ item.Situation == AlbConstantesMetiers.EditionSituations.Z;
                    item.ColorSituation = /*item.Situation == AlbConstantesMetiers.EditionSituations.N || */item.Situation == AlbConstantesMetiers.EditionSituations.Z ? "situationRed" : /*item.Situation == AlbConstantesMetiers.EditionSituations.A ? "situationBlue" :*/ string.Empty;
                    item.TitleSituation = string.Concat(item.LibSituation.Trim(), "\n", (item.Situation == AlbConstantesMetiers.EditionSituations.E ?
                                    string.Format("Edité le {0} par {1}",
                                                        item.DateSit.HasValue ? string.Concat(item.DateSit.Value.Day.ToString().PadLeft(2, '0'), "/", item.DateSit.Value.Month.ToString().PadLeft(2, '0'), "/", item.DateSit.Value.Year) : string.Empty,
                                                        item.UtilisateurSituation.Trim()) : string.Empty));
                    item.TitleUtilisateur = string.Concat(item.PrenomUtilisateur.Trim(), " ", item.NomUtilisateur.Trim(), "\n", item.UniteService.Trim());
                    item.TitleDoc = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", "\n",
                        !string.IsNullOrEmpty(item.NomDoc.Trim()) ? item.NomDoc.Trim() : "test.docx", item.ServiceDoc.Trim(),
                        item.DateCreateDoc.HasValue ? string.Concat("Créé le ", item.DateCreateDoc.Value.Day.ToString().PadLeft(2, '0'), "/", item.DateCreateDoc.Value.Month.ToString().PadLeft(2, '0'), "/", item.DateCreateDoc.Value.Year) : string.Empty,
                        item.DateModifDoc.HasValue ? string.Concat("Modifié le ", item.DateModifDoc.Value.Day.ToString().PadLeft(2, '0'), "/", item.DateModifDoc.Value.Month.ToString().PadLeft(2, '0'), "/", item.DateModifDoc.Value.Year) : string.Empty,
                        item.TypeGenDoc.Trim());


                    item.ColorTypoEditDoc = !string.IsNullOrEmpty(item.TypoEditDoc) ? AlbEnumInfoValue.GetEnumInfo((AlbConstantesMetiers.ColorTypeEditDoc)Enum.Parse(typeof(AlbConstantesMetiers.ColorTypeEditDoc), item.TypoEditDoc.Trim())) : AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.ColorTypeEditDoc.A);
                    item.RefreshDoc = item.TypeGenDoc != "L" && Enum.IsDefined(typeof(AlbTypeDocRefresh), item.TypeDoc.Trim()) && item.CurAvn == item.NumInterne;
                }
        
            }
            return result;
        }

        public static SuiviDocumentsListeDto LoadDto(SuiviDocumentsListe modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsListe, SuiviDocumentsListeDto>().Map(modele);
        }
    }
}