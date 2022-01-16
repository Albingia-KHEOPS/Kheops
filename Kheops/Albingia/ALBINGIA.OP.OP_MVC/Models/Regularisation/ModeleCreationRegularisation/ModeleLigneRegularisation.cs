using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ModeleLigneRegularisation 
    {
        public Int64 NumRegule { get; set; }
        public Int64 CodeAvn { get; set; }
        public string CodeTraitement { get; set; }
        public string LibTraitement { get; set; }
        public Int32 DateDeb { get; set; }
        public DateTime? PeriodeDebut { get { return AlbConvert.ConvertIntToDate(DateDeb); } }
        public Int32 DateFin { get; set; }
        public DateTime? PeriodeFin { get { return AlbConvert.ConvertIntToDate(DateFin); } }
        public string CodeEtat { get; set; }
        public string CodeSituation { get; set; }
        public string LibSituation { get; set; }
        public Int32 DateSit { get; set; }
        public DateTime? DateSituation { get { return AlbConvert.ConvertIntToDate(DateSit); } }
        public Int32 NumQuittance { get; set; }
        public string CodeEtatQuitt { get; set; }
        public string LibEtatQuitt { get; set; }
        public Int32 ? DateDebAvn { get; set; }
        public DateTime? DateDebutAvenant { get { return AlbConvert.ConvertIntToDate(DateDebAvn); } }
        public List<AlbSelectListItem> TypesTraitement { get; set; }
        public string Periodicite { get; set; }
        public Int64 Avis { get; set; }

        public string RegulMode { get; set; }
        public string RegulType { get; set; }
        public string RegulNiv { get; set; }
        public string RegulAvn { get; set; }

        public static explicit operator ModeleLigneRegularisation(LigneRegularisationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneRegularisationDto, ModeleLigneRegularisation>().Map(modeleDto);
        }

        public static LigneRegularisationDto LoadDto(ModeleLigneRegularisation modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleLigneRegularisation, LigneRegularisationDto>().Map(modele);
        }

    }
}