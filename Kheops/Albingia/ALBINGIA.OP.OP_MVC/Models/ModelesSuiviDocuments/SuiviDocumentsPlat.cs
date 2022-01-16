using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.SuiviDocuments;
using System;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments
{
    public class SuiviDocumentsPlat
    {
        public Int64 CurAvn { get; set; }
        public Int64 LotId { get; set; }
        public string LotLibelle { get; set; }
        public string CodeUtilisateur { get; set; }
        public string NomUtilisateur { get; set; }
        public string PrenomUtilisateur { get; set; }
        public string UniteService { get; set; }
        public string TypeAffaire { get; set; }
        public string CodeOffre { get; set; }
        public Int32 Version { get; set; }
        public Int64 LotDetailId { get; set; }
        public string CodeSituation { get; set; }
        public string LibSituation { get; set; }
        public Int32 DateSituation { get; set; }
        public Int32 HeureSituation { get; set; }
        public string UtilisateurSituation { get; set; }
        public string ActeGestion { get; set; }
        public string ActeGestionLib { get; set; }
        public Int32 NumInterne { get; set; }
        public Int64 NumExterne { get; set; }
        public string NomDoc { get; set; }
        public string CheminDoc { get; set; }
        public Int64 DocId { get; set; }
        public string TypeDoc { get; set; }
        public Double EmptyLine { get; set; }
        public string CodeDoc { get; set; }
        public string ServiceDoc { get; set; }
        public Int32 CreateDoc { get; set; }
        public Int32 UpdateDoc { get; set; }
        public string TypeGenDoc { get; set; }
        public string TypoEditDoc { get; set; }
        public string TypeDocLib { get; set; }
        public string TypeDestinataire { get; set; }
        public string TypeIntervenant { get; set; }
        public Int32 CodeDestinataire { get; set; }
        public string NomDestinataire { get; set; }
        public Int32 CodeInterlocuteur { get; set; }
        public string NomInterlocuteur { get; set; }
        public string CodeDiffusion { get; set; }
        public string LibDiffusion { get; set; }
        public Int32 DocExterne { get; set; }
        public DateTime? DateSit { get; set; }
        public DateTime? DateCreateDoc { get; set; }
        public DateTime? DateModifDoc { get; set; }
        public AlbConstantesMetiers.EditionSituations Situation { get; set; }
        public bool LinkSituation { get; set; }
        public string ColorSituation { get; set; }
        public string TitleSituation { get; set; }
        public string TitleUtilisateur { get; set; }
        public string TitleDoc { get; set; }
        public string ColorTypoEditDoc { get; set; }
        public bool RefreshDoc { get; set; }

        public static explicit operator SuiviDocumentsPlat(SuiviDocumentsPlatDto modelDto)
        {
            var result = ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsPlatDto, SuiviDocumentsPlat>().Map(modelDto);

            //result.LotLibelle = !string.IsNullOrEmpty(result.LotLibelle) && !string.IsNullOrEmpty(result.LotLibelle.Trim()) ? result.LotLibelle.Trim() : string.Empty;
            //result.CodeUtilisateur = !string.IsNullOrEmpty(result.CodeUtilisateur) && !string.IsNullOrEmpty(result.CodeUtilisateur.Trim()) ? result.CodeUtilisateur.Trim() : string.Empty;
            //result.NomUtilisateur = !string.IsNullOrEmpty(result.NomUtilisateur) && !string.IsNullOrEmpty(result.NomUtilisateur.Trim()) ? result.NomUtilisateur.Trim() : string.Empty;
            //result.PrenomUtilisateur = !string.IsNullOrEmpty(result.PrenomUtilisateur) && !string.IsNullOrEmpty(result.PrenomUtilisateur.Trim()) ? result.PrenomUtilisateur.Trim() : string.Empty;
            //result.UniteService = !string.IsNullOrEmpty(result.UniteService) && !string.IsNullOrEmpty(result.UniteService.Trim()) ? result.UniteService.Trim() : string.Empty;

            string text = string.Empty;
            string value = !string.IsNullOrEmpty(result.CodeSituation) ? AlbEnumInfoValue.GetEnumInfoSplit((AlbConstantesMetiers.EditionSituations)Enum.Parse(typeof(AlbConstantesMetiers.EditionSituations), result.CodeSituation), out text) : string.Empty;
            result.LibSituation = text.Trim();

            result.DateSit = AlbConvert.ConvertIntToDate(result.DateSituation); ;
            result.DateCreateDoc = AlbConvert.ConvertIntToDate(result.CreateDoc);
            result.DateModifDoc = AlbConvert.ConvertIntToDate(result.UpdateDoc);
            result.Situation = (AlbConstantesMetiers.EditionSituations)Enum.Parse(typeof(AlbConstantesMetiers.EditionSituations), result.CodeSituation);
            result.LinkSituation = /*result.Situation == AlbConstantesMetiers.EditionSituations.A || result.Situation == AlbConstantesMetiers.EditionSituations.N ||*/ result.Situation == AlbConstantesMetiers.EditionSituations.Z;
            result.ColorSituation =/* result.Situation == AlbConstantesMetiers.EditionSituations.N ||*/ result.Situation == AlbConstantesMetiers.EditionSituations.Z ? "situationRed" : /*result.Situation == AlbConstantesMetiers.EditionSituations.A ? "situationBlue" :*/ string.Empty;
            result.TitleSituation = string.Concat(result.LibSituation.Trim(), "\n", (result.Situation == AlbConstantesMetiers.EditionSituations.E ?
                            string.Format("Edité le {0} par {1}",
                                                result.DateSit.HasValue ? string.Concat(result.DateSit.Value.Day.ToString().PadLeft(2, '0'), "/", result.DateSit.Value.Month.ToString().PadLeft(2, '0'), "/", result.DateSit.Value.Year) : string.Empty,
                                                result.UtilisateurSituation.Trim()) : string.Empty));
            result.TitleUtilisateur = string.Concat(result.PrenomUtilisateur.Trim(), " ", result.NomUtilisateur.Trim(), "\n", result.UniteService.Trim());
            result.TitleDoc = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", "\n",
                !string.IsNullOrEmpty(result.NomDoc.Trim()) ? result.NomDoc.Trim() : "test.docx", result.ServiceDoc.Trim(),
                result.DateCreateDoc.HasValue ? string.Concat("Créé le ", result.DateCreateDoc.Value.Day.ToString().PadLeft(2, '0'), "/", result.DateCreateDoc.Value.Month.ToString().PadLeft(2, '0'), "/", result.DateCreateDoc.Value.Year) : string.Empty,
                result.DateModifDoc.HasValue ? string.Concat("Modifié le ", result.DateModifDoc.Value.Day.ToString().PadLeft(2, '0'), "/", result.DateModifDoc.Value.Month.ToString().PadLeft(2, '0'), "/", result.DateModifDoc.Value.Year) : string.Empty,
                result.TypeGenDoc.Trim());


            result.ColorTypoEditDoc = !string.IsNullOrEmpty(result.TypoEditDoc) ? AlbEnumInfoValue.GetEnumInfo((AlbConstantesMetiers.ColorTypeEditDoc)Enum.Parse(typeof(AlbConstantesMetiers.ColorTypeEditDoc), result.TypoEditDoc.Trim())) : AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.ColorTypeEditDoc.A);
            result.RefreshDoc = result.TypeGenDoc != "L" && Enum.IsDefined(typeof(AlbTypeDocRefresh), result.TypeDoc.Trim());

            return result;
        }

        public static SuiviDocumentsPlatDto LoadDto(SuiviDocumentsPlat modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsPlat, SuiviDocumentsPlatDto>().Map(modele);
        }
    }
}