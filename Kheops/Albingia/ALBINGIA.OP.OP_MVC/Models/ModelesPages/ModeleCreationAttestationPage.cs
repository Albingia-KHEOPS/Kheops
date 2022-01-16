using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleCreationAttestationPage : MetaModelsBase
    {
        public DateTime? EffetGaranties { get; set; }
        public DateTime? FinEffet { get; set; }
        public TimeSpan? FinEffetHeure { get; set; }
        public DateTime? Echeance { get; set; }


        public string Exercice { get; set; }
        public Int32 PeriodeDebInt { get; set; }
        public Int32 PeriodeFinInt { get; set; }
        public bool IntegraliteContrat { get; set; }
        public string TypeAttes { get; set; }
        public List<AlbSelectListItem> TypesAttes { get; set; }
        public List<ModeleAvenantAlerte> Alertes { get; set; }

        public string LibTypeContrat { get; set; }

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

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public RegularisationBandeauContratDto BandeauContrat { get; set; }
    }
}