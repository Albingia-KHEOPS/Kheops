using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleClassementSansSuitePage : MetaModelsBase
    {
        public VisualisationQuittances Primes { get; set; }
        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }
        public RegularisationBandeauContratDto BandeauContrat { get; set; }
    }
}