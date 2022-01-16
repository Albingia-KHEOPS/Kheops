using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Condition;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    [Serializable]
    public class ModeleConditionsExprComplexeDetails
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Id { get; set; }
        public string Libelle { get; set; }
        public string Descriptif { get; set; }
        public List<ModeleConditionsLigneGarantie> LstLigneGarantie { get; set; }
        public string UniteLCINew { get; set; }
        public List<AlbSelectListItem> UnitesLCINew { get; set; }
        public string TypeLCINew { get; set; }
        public List<AlbSelectListItem> TypesLCINew { get; set; }
        public string UniteFranchiseNew { get; set; }
        public List<AlbSelectListItem> UnitesFranchiseNew { get; set; }
        public string TypeFranchiseNew { get; set; }
        public List<AlbSelectListItem> TypesFranchiseNew { get; set; }
        public string UniteConcurrence { get; set; }
        public List<AlbSelectListItem> UnitesConcurrence { get; set; }
        public string TypeConcurrence { get; set; }
        public List<AlbSelectListItem> TypesConcurrence { get; set; }
        public int FranchiseMini { get; set; }
        public int FranchiseMaxi { get; set; }
        public DateTime? FranchiseDebut { get; set; }
        public DateTime? FranchiseFin { get; set; }

        public bool IsReadOnly { get; set; }
        public bool Modifiable { get; set; }
        public string Origine { get; set; }
        public bool Utilise { get; set; }

        public static explicit operator ModeleConditionsExprComplexeDetails(ConditionComplexeDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ConditionComplexeDto, ModeleConditionsExprComplexeDetails>().Map(modeleDto);
        }

        public static ConditionComplexeDto LoadDto(ModeleConditionsExprComplexeDetails dataDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleConditionsExprComplexeDetails, ConditionComplexeDto>().Map(dataDto);
        }


    }
}