using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ModeleCreationRegule : MetaModelsBase
    {
        public long LotId { get; set; }
        public long ReguleId { get; set; }

        public string Exercice { get; set; }
        public Int32? PeriodeDebInt { get; set; }
        public Int32? PeriodeFinInt { get; set; }
        public List<ModeleAvenantAlerte> Alertes { get; set; }

        public DateTime? PeriodeDeb
        {
            get
            {
                return AlbConvert.ConvertIntToDate(PeriodeDebInt);
            }
        }
        public DateTime? PeriodeFin
        {
            get
            {
                return AlbConvert.ConvertIntToDate(PeriodeFinInt);
            }
        }

        public Int32 NumAvt { get; set; }
        public string ModeAvt { get; set; }
        public string TypeAvt { get; set; }
        public string LibelleAvt { get; set; }
        public Int64 NumInterneAvt { get; set; }
        public string MotifAvt { get; set; }
        public string LibMotifAvt { get; set; }
        public string DescriptionAvt { get; set; }
        public string ObservationsAvt { get; set; }
        public string ErrorAvt { get; set; }

        public string CodeCourtier { get; set; }
        public string NomCourtier { get; set; }
        public double TauxHCATNAT { get; set; }
        public double TauxCATNAT { get; set; }
        public string CodeQuittancement { get; set; }
        public string LibQuittancement { get; set; }
        public bool MultiCourtier { get; set; }

        public string CodeCourtierCom { get; set; }
        public string NomCourtierCom { get; set; }

        public List<AlbSelectListItem> Courtiers { get; set; }
        public List<AlbSelectListItem> Quittancements { get; set; }
        public List<AlbSelectListItem> Motifs { get; set; }
        public string RetourPGM { get; set; }
        public string ErreurPGM { get; set; }
        public bool HasSelections { get; set; }


        public static explicit operator ModeleCreationRegule(RegularisationInfoDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RegularisationInfoDto, ModeleCreationRegule>().Map(modeleDto);
        }

        public static RegularisationInfoDto LoadDto(ModeleCreationRegule modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleCreationRegule, RegularisationInfoDto>().Map(modele);
        }
    }
}