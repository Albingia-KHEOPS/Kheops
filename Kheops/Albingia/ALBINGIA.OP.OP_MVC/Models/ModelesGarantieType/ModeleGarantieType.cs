using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using EmitMapper;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.ParametreGaranties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesGarantieType
{
    [Serializable]
    public class ModeleGarantieType
    {
        [Display(Name = "Numéro de séquence")]
        public long NumeroSeq { get; set; }
        [Display(Name = "Code modèle")]
        public string CodeModele { get; set; }
        [Display(Name = "Nom modèle")]
        public string NomModele { get; set; }
        [Display(Name = "Niveau")]
        public int Niveau { get; set; }
        [Display(Name = "Code garantie")]
        public string CodeGarantie { get; set; }
        [Display(Name = "Ordre")]
        public int Ordre { get; set; }
        [Display(Name = "Tri")]
        public string Tri { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Numéro de séquence Mère")]
        public long NumeroSeqM { get; set; }
        [Display(Name = "Numéro de séquence Niveau 1")]
        public long NumeroSeq1 { get; set; }
        [Display(Name = "Caractère")]
        public string Caractere { get; set; }
        [Display(Name = "Nature")]
        public string Nature { get; set; }
        [Display(Name = "Indexée")]
        public bool IsIndexee { get; set; }
        [Display(Name = "Soumis CATNAT")]
        public bool SoumisCATNAT { get; set; }
        [Display(Name = "Code Taxe")]
        public string CodeTaxe { get; set; }
        [Display(Name = "Groupe Alternative")]
        public int GroupeAlternative { get; set; }
        [Display(Name = "Conditionnement")]
        public string Conditionnement { get; set; }
        [Display(Name = "Type Prime")]
        public string TypePrime { get; set; }
        [Display(Name = "Type de controle de date")]
        public string TypeControleDate { get; set; }
        [Display(Name = "Dans montant de référence")]
        public bool IsMontantRef { get; set; }
        [Display(Name = "Nature modifiable")]
        public bool IsNatureModifiable { get; set; }
        [Display(Name = "Masquer dans CP")]
        public bool IsMasquerCP { get; set; }
        

        public bool ReadOnly { get; set; }
        public bool IsNew { get; set; }
        public bool IsModifiable { get; set; }
        public string CaractereLib { get; set; }
        public string NatureLib { get; set; }

        public List<ParamGarantieDto> ListGarantie { get; set; }
        public List<CaractereGarantie> ListCaractere { get; set; }
        public List<NatureGarantie> ListNature { get; set; }
        public List<TypeControleDate> ListTypeControleDate { get; set; }
        public List<BaseCapitaux> ListBaseAssiette { get; set; }
        public List<UniteCapitaux> ListUniteAssiette { get; set; }
        public List<BasePrime> ListBasePrime { get; set; }
        public List<UnitePrime> ListUnitePrime { get; set; }
        public List<BaseLCI> ListBaseLCI { get; set; }
        public List<UniteLCI> ListUniteLCI { get; set; }
        public List<BaseFranchise> ListBaseFranchise { get; set; }
        public List<BaseFranchise> ListBaseFranchiseMin { get; set; }
        public List<BaseFranchise> ListBaseFranchiseMax { get; set; }
        public List<UniteFranchise> ListUniteFranchise { get; set; }
        public List<Alimentation> ListAlimentation { get; set; }
        public List<ModeModifiable> ListModeModifiable { get; set; }

        public List<ModeleGarantieTypeLCI> ListLCI { get; set; }
        public List<ModeleGarantieType> ListSousGarantie { get; set; }

        //Lien entre garantie
        public List<ModeleLienGarantie> ListLien { get; set; }
        public List<ModeleGarantieType> ListGarantieType { get; set; }

        public ModeleGarantieType()
        {
            this.ListCaractere = new List<CaractereGarantie>();
            this.ListNature = new List<NatureGarantie>();
            this.ListSousGarantie = new List<ModeleGarantieType>();
            this.ListLCI = new List<ModeleGarantieTypeLCI>() {
                new ModeleGarantieTypeLCI() { Type = ((int)TypeDeValeur.Assiette).ToString() },
                new ModeleGarantieTypeLCI() { Type = ((int)TypeDeValeur.Prime).ToString() },
                new ModeleGarantieTypeLCI() { Type = ((int)TypeDeValeur.LCI).ToString() },
                new ModeleGarantieTypeLCI() { Type = ((int)TypeDeValeur.Franchise).ToString() },
                new ModeleGarantieTypeLCI() { Type = ((int)TypeDeValeur.FranchiseMin).ToString() },
                new ModeleGarantieTypeLCI() { Type = ((int)TypeDeValeur.FranchiseMax).ToString() },
            };
            this.ListLien = new List<ModeleLienGarantie>();
            this.ListGarantieType = new List<ModeleGarantieType>();
        }

        public static explicit operator ModeleGarantieType(GarantieTypeDto dtoGarantieType)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<GarantieTypeDto, ModeleGarantieType>().Map(dtoGarantieType);
        }

        public static GarantieTypeDto LoadDto(ModeleGarantieType garantieType)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleGarantieType, GarantieTypeDto>().Map(garantieType);
        }
    }

    public class ModeleGarantieTypeLCI
    {
        public string Type { get; set; }
        public string Base { get; set; }
        public string Unite { get; set; }
        public decimal Valeur { get; set; }
        public string Modi { get; set; }
        public bool Obl { get; set; }
        public string Alim { get; set; }
    }

    public class ModeleLienGarantie
    {
        public string Type { get; set; }
        public int GarantieA { get; set; }
        public int GarantieB { get; set; }
        public string NomGarantieLiee { get; set; }
        public string ModeleGarantieLiee { get; set; }
        public string NiveauGarantieLiee { get; set; }
    }
}