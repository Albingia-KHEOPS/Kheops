using EmitMapper;
using OP.WSAS400.DTO.ChoixClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleChoixClausePieceJointe
    {
        [Display(Name="Contexte")]
        public string Contexte { get; set; }
        [Display(Name = "Emplacement*")]
        public string Emplacement { get; set; }
        [Display(Name = "Sous emplacement*")]
        public string SousEmplacement { get; set; }
        [Display(Name = "Ordre")]
        public Int64 Ordre { get; set; }
        [Display(Name = "S'applique à")]
        public string AppliqueA { get; set; }

        public Int32 CodeRisque { get; set; }
        public string Risque { get; set; }
        public Int32 CodeObjet { get; set; }
        public ModeleObjetsRisque ObjetsRisqueAll { get; set; }

        public List<ModeleChoixClauseListePieceJointe> PiecesJointes { get; set; }

        public bool Modifiable { get; set; }


        public static explicit operator ModeleChoixClausePieceJointe(ChoixClausePieceJointeDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ChoixClausePieceJointeDto, ModeleChoixClausePieceJointe>().Map(modeleDto);
        }

        public static ChoixClausePieceJointeDto LoadDto(ModeleChoixClausePieceJointe modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleChoixClausePieceJointe, ChoixClausePieceJointeDto>().Map(modele);
        }

    }
}