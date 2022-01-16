using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleAnnulationQuittancesPage : MetaModelsBase
    {
        public List<AlbSelectListItem> TypesOperation { get; set; }
        public string TypeOperation { get; set; }
        public List<AlbSelectListItem> Situations { get; set; }
        public string Situation { get; set; }
        public List<VisualisationQuittancesLigne> ListQuittances { get; set; }
        public DateTime? DateEmission { get; set; }
        public DateTime? PeriodeDebut { get; set; }
        public DateTime? PeriodeFin { get; set; }
        public bool Reprise { get; set; }
        public bool IsChekedEcheance { get; set; }
        //public bool DisplayEditionQuittance { get; set; }
        public static explicit operator ModeleAnnulationQuittancesPage(QuittanceVisualisationDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceVisualisationDto, ModeleAnnulationQuittancesPage>().Map(modelDto);
        }

        public static QuittanceVisualisationDto LoadDto(ModeleAnnulationQuittancesPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAnnulationQuittancesPage, QuittanceVisualisationDto>().Map(modele);
        }
    }
}