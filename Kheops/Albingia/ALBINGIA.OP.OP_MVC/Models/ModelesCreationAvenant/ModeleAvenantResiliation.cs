using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant
{
    public class ModeleAvenantResiliation
    {
        public string ErrorAvt { get; set; }
        public string TypeAvt { get; set; }
        public string ModeAvt { get; set; }
        public string LibelleAvt { get; set; }
        public bool AvenantEch { get; set; }
        public Int32 NumInterneAvt { get; set; }
        public DateTime? DateFinGarantie { get; set; }
        public TimeSpan? HeureFinGarantie { get; set; }
        public Int32 NumAvt { get; set; }
        public string MotifAvt { get; set; }
        public string LibMotifAvt { get; set; }
        public string DescriptionAvt { get; set; }
        public string ObservationsAvt { get; set; }
        public List<AlbSelectListItem> Motifs { get; set; }
        public string DateFin { get; set; }
        public List<AlbSelectListItem> DatesFin { get; set; }
        public bool AvenantEchPossible { get; set; }
        public DateTime? ProchaineEchHisto { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public DateTime? FinGarantieHisto { get; set; }

        public static explicit operator ModeleAvenantResiliation(AvenantResiliationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AvenantResiliationDto, ModeleAvenantResiliation>().Map(modeleDto);
        }
        public static AvenantResiliationDto LoadDto(ModeleAvenantResiliation modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAvenantResiliation, AvenantResiliationDto>().Map(modele);
        }
    }
}