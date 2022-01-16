using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant
{
    public class ModeleAvenantModification
    {
        public string ErrorAvt { get; set; }
        public string TypeAvt { get; set; }
        public string ModeAvt { get; set; }
        public string LibelleAvt { get; set; }
        public Int32 NumInterneAvt { get; set; }
        public DateTime? DateEffetAvt { get; set; }
        public TimeSpan? HeureEffetAvt { get; set; }
        public Int32 NumAvt { get; set; }
        public string MotifAvt { get; set; }
        public string LibMotifAvt { get; set; }
        public string DescriptionAvt { get; set; }
        public string ObservationsAvt { get; set; }
        public List<AlbSelectListItem> Motifs { get; set; }
        public DateTime? ProchaineEchHisto { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public string Etat { get; set; }
        public string Situation { get; set; }
        public string Periodicite { get; set; }

        public static explicit operator ModeleAvenantModification(AvenantModificationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AvenantModificationDto, ModeleAvenantModification>().Map(modeleDto);
        }

        public static AvenantModificationDto LoadDto(ModeleAvenantModification modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAvenantModification, AvenantModificationDto>().Map(modele);
        }

    }
}