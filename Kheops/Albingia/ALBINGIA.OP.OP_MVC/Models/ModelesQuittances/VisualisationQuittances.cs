using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VisualisationQuittances
    {      
        public List<AlbSelectListItem> TypesOperation { get; set; }
        public string TypeOperation { get; set; }
        public List<AlbSelectListItem> Situations { get; set; }
        public string Situation { get; set; }     
        public List<VisualisationQuittancesLigne> ListQuittances { get; set; }
        public DateTime? DateEmission { get; set; }
        public DateTime? PeriodeDebut { get; set; }
        public DateTime? PeriodeFin { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsOpenedFromHeader { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public string Etat { get; set; }
        public string NumAvn { get; set; }
        public string ReguleId { get; set; }
        public bool IsHisto { get; set; }

        public static explicit operator VisualisationQuittances(QuittanceVisualisationDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<QuittanceVisualisationDto, VisualisationQuittances>().Map(modelDto);
        }

        public static QuittanceVisualisationDto LoadDto(VisualisationQuittances modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<VisualisationQuittances, QuittanceVisualisationDto>().Map(modele);
        }

    }
}