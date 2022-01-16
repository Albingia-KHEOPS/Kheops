using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Condition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    [Serializable]
    public class ModeleConditionsLigneGarantie
    {
        public string Code { get; set; }
        public int? Id { get; set; }
        public string CodeGarantie { get; set; }

        public int IdGarantie {
            get {
                if (Code.IsEmptyOrNull() || !Code.Contains("_")) {
                    return default;
                }
                return int.TryParse(Code.Split('_')[1], out int i) && i > 0 ? i : default;
            }
        }
        public string NumeroTarif { get; set; }
        public int IndiceLigne { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:# ###}")]
        public string LCIValeur { get; set; }
        public string LCILectureSeule { get; set; }
        public string LCIObligatoire { get; set; }
        public string LCIUnite { get; set; }
        public List<AlbSelectListItem> LCIUnites { get; set; }
        public string LCIType { get; set; }
        public List<AlbSelectListItem> LCITypes { get; set; }
        public string LienLCIComplexe { get; set; }
        public string LibLCIComplexe { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:# ###}")]
        public string FranchiseValeur { get; set; }
        public string FranchiseLectureSeule { get; set; }
        public string FranchiseObligatoire { get; set; }
        public string FranchiseUnite { get; set; }
        public List<AlbSelectListItem> FranchiseUnites { get; set; }
        public string FranchiseType { get; set; }
        public List<AlbSelectListItem> FranchiseTypes { get; set; }
        public string FranchiseMini { get; set; }
        public string FranchiseMaxi { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FranchiseDebut { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FranchiseFin { get; set; }
        public string LienFRHComplexe { get; set; }
        public string LibFRHComplexe { get; set; }
        public string AssietteValeur { get; set; }
        public string AssietteLectureSeule { get; set; }
        public string AssietteObligatoire { get; set; }
        public string AssietteUnite { get; set; }
        public List<AlbSelectListItem> AssietteUnites { get; set; }
        public string AssietteType { get; set; }
        public List<AlbSelectListItem> AssietteTypes { get; set; }
        public string TauxForfaitHTValeur { get; set; }
        public string TauxForfaitHTLectureSeule { get; set; }
        public string TauxForfaitHTObligatoire { get; set; }
        public string TauxForfaitHTUnite { get; set; }
        public List<AlbSelectListItem> TauxForfaitHTUnites { get; set; }
        public string TauxForfaitHTMinimum { get; set; }
        public bool TauxPrimeAlim { get; set; }

        public string ConcurrenceValeur { get; set; }
        public string ConcurrenceUnite { get; set; }
        public List<AlbSelectListItem> ConcurrenceUnites { get; set; }
        public string ConcurrenceType { get; set; }
        public List<AlbSelectListItem> ConcurrenceTypes { get; set; }

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

        public string Type { get; set; }

        public bool IsEdition { get; set; }
        public string Niveau { get; set; }
        public string CVolet { get; set; }
        public string CBloc { get; set; }

        public bool ReadOnly { get; set; }

        /// <summary>
        /// "" aucune modification
        /// "U" update
        /// "I" insert
        /// </summary>
        public string MAJ { get; set; }


        public static explicit operator ModeleConditionsLigneGarantie(LigneGarantieDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneGarantieDto, ModeleConditionsLigneGarantie>().Map(modeleDto);
        }

        public static LigneGarantieDto LoadDto(ModeleConditionsLigneGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleConditionsLigneGarantie, LigneGarantieDto>().Map(modele);
        }


    }
}