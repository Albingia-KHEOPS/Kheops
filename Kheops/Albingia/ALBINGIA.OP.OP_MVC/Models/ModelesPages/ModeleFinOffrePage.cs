using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre;
using OP.WSAS400.DTO.FinOffre;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleFinOffrePage : MetaModelsBase
    {
        public ModeleFinOffreInfos ModeleFinOffreInfos { get; set; }
        public ModeleFinOffreAnnotation ModeleFinOffreAnnotation { get; set; }

        public ModeleFinOffrePage()
        {
            ModeleFinOffreInfos = new ModeleFinOffreInfos();
            ModeleFinOffreAnnotation = new ModeleFinOffreAnnotation();
        }

        public static explicit operator ModeleFinOffrePage(FinOffreDto finOffreDto)
        {
            ModeleFinOffrePage modeleFinOffrePage = new ModeleFinOffrePage();
            modeleFinOffrePage.ModeleFinOffreInfos = new ModeleFinOffreInfos();
            modeleFinOffrePage.ModeleFinOffreInfos.Antecedent = finOffreDto.FinOffreInfosDto.Antecedent;
            modeleFinOffrePage.ModeleFinOffreInfos.DateProjet = finOffreDto.FinOffreInfosDto.DateProjet;
            modeleFinOffrePage.ModeleFinOffreInfos.DateStatistique = finOffreDto.FinOffreInfosDto.DateStatistique;
            modeleFinOffrePage.ModeleFinOffreInfos.Description = finOffreDto.FinOffreInfosDto.Description;
            modeleFinOffrePage.ModeleFinOffreInfos.Preavis = finOffreDto.FinOffreInfosDto.Preavis;
            modeleFinOffrePage.ModeleFinOffreInfos.Relance = finOffreDto.FinOffreInfosDto.Relance;
            modeleFinOffrePage.ModeleFinOffreInfos.RelanceValeur = finOffreDto.FinOffreInfosDto.RelanceValeur;
            modeleFinOffrePage.ModeleFinOffreInfos.ValiditeOffre = finOffreDto.FinOffreInfosDto.ValiditeOffre;
            modeleFinOffrePage.ModeleFinOffreAnnotation = new ModeleFinOffreAnnotation();
            modeleFinOffrePage.ModeleFinOffreAnnotation.AnnotationFin = finOffreDto.FinOffreAnnotationDto.AnnotationFin;
            return modeleFinOffrePage;
        }

        public static FinOffreDto LoadDto(ModeleFinOffrePage modele)
        {
            FinOffreDto finOffreDto = new FinOffreDto();
            finOffreDto.FinOffreInfosDto = new FinOffreInfosDto();
            finOffreDto.FinOffreInfosDto.Antecedent = modele.ModeleFinOffreInfos.Antecedent;
            finOffreDto.FinOffreInfosDto.DateProjet = modele.ModeleFinOffreInfos.DateProjet;
            finOffreDto.FinOffreInfosDto.DateStatistique = modele.ModeleFinOffreInfos.DateStatistique;
            finOffreDto.FinOffreInfosDto.Description = modele.ModeleFinOffreInfos.Description;
            finOffreDto.FinOffreInfosDto.Preavis = modele.ModeleFinOffreInfos.Preavis;
            finOffreDto.FinOffreInfosDto.Relance = modele.ModeleFinOffreInfos.Relance;
            finOffreDto.FinOffreInfosDto.RelanceValeur = modele.ModeleFinOffreInfos.RelanceValeur;
            finOffreDto.FinOffreInfosDto.ValiditeOffre = modele.ModeleFinOffreInfos.ValiditeOffre;
            finOffreDto.FinOffreAnnotationDto = new FinOffreAnnotationDto();
            //finOffreDto.FinOffreAnnotationDto.AnnotationFin = modele.ModeleFinOffreAnnotation.AnnotationFin;
            if (!string.IsNullOrEmpty(modele.ModeleFinOffreAnnotation.AnnotationFin))
            {
                finOffreDto.FinOffreAnnotationDto.AnnotationFin = modele.ModeleFinOffreAnnotation.AnnotationFin.Replace("\r\n", "<br>").Replace("\n", "<br>");
            }
            else
            {
                finOffreDto.FinOffreAnnotationDto.AnnotationFin = string.Empty;
            }
            return finOffreDto;
        }
    }
}