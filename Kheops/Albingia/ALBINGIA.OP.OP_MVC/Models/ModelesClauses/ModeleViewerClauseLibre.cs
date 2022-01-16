using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Concerts.Clause;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleViewerClauseLibre
    {
        public string ClauseType { get; set; }
        public string CreateClause { get; set; }
        public string Contexte { get; set; }
        public string LibContexte { get; set; }
        public string Emplacement { get; set; }
        public List<AlbSelectListItem> Emplacements { get; set; }
        public string SousEmplacement { get; set; }
        public Int32 Ordre { get; set; }
        public string AppliqueA { get; set; }
        public Int32 CodeRisque { get; set; }
        public string Risque { get; set; }
        public Int32 CodeObjet { get; set; }
        public Int64 IdDoc { get; set; }
        public string Titre { get; set; }
        public string UserAjt { get; set; }
        public ModeleObjetsRisque ObjetsRisqueAll { get; set; }
        public bool Modifiable { get; set; }
        public string TypeAjout { get; set; }
        public Int32 DateDebut { get; set; }
        public Int32 DateFin { get; set; }
        public DateTime? DebutEffet { get; set; }
        public DateTime? FinEffet { get; set; }
        public string DateDeb { get; set; }
        public string DateF { get; set; }

        public static explicit operator ModeleViewerClauseLibre(ClauseLibreViewerDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ClauseLibreViewerDto, ModeleViewerClauseLibre>().Map(modelDto);
        }

        public static ClauseLibreViewerDto LoadDto(ModeleViewerClauseLibre modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleViewerClauseLibre, ClauseLibreViewerDto>().Map(modele);
        }

    }
}