using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Condition;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    [Serializable]
    public class ModeleConditionsGarantie
    {
        private decimal? primeGareat;

        public string Id { get; set; }
        public string IdSequence { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Couleur1 { get; set; }
        public string Couleur2 { get; set; }

        public string Couleur
        {
            get
            {
                switch (Couleur2)
                {
                    case "A":
                        return MvcApplication.GARA_GRANTED_COLOR;
                    case "C":
                        return MvcApplication.GARA_INCLUDE_COLOR;
                    case "E":
                        return MvcApplication.GARA_BASE_COLOR;
                    default:
                        return MvcApplication.GARA_BASE_COLOR;
                }
            }
        }
        
        public List<AlbSelectListItem> LCIUnites { get; set; }
        public List<AlbSelectListItem> LCITypes { get; set; }
        public List<AlbSelectListItem> FranchiseUnites { get; set; }
        public List<AlbSelectListItem> FranchiseTypes { get; set; }
        public List<AlbSelectListItem> AssietteUnites { get; set; }
        public List<AlbSelectListItem> AssietteTypes { get; set; }
        public List<AlbSelectListItem> TauxForfaitHTUnites { get; set; }

        public List<ModeleConditionsLigneGarantie> LstLigneGarantie { get; set; }

        public string Niveau { get; set; }
        public string Pere { get; set; }
        public string Sequence { get; set; }
        public string Origine { get; set; }

        public bool ReadOnly { get; set; }
        public string CVolet { get; set; }
        public string LVolet { get; set; }
        public string CBloc { get; set; }
        public string LBloc { get; set; }

        public bool IsGarantieSortie { get; set; }

        public bool IsAttentatGareat { get; set; }

        public decimal? PrimeGareat {
            get => IsAttentatGareat ? this.primeGareat : default;
            set => this.primeGareat = value;
        }

        public static explicit operator ModeleConditionsGarantie(EnsembleGarantieDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EnsembleGarantieDto, ModeleConditionsGarantie>().Map(modeleDto);
        }

        public static EnsembleGarantieDto LoadDto(ModeleConditionsGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleConditionsGarantie, EnsembleGarantieDto>().Map(modele);
        }

    }
}