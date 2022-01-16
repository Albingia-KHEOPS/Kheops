using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesInventaire;
using EmitMapper;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    [Serializable]
    public class Inventaire_MetaModel : MetaModelsBase
    {
       // public FileDescriptions FileDescriptionsMetaData { get; set; }

        public Int64 Code { get; set; }
        public string Description { get; set; }
        [Display(Name = "Libellé *")]
        public string Descriptif { get; set; }
        public Int64 InventaireType { get; set; }
        public List<ModeleInventaireGridRow> InventaireInfos { get; set; }

        public string NatureLieu { get; set; }
        public List<AlbSelectListItem> NaturesLieu { get; set; }
        public string CodeMateriel { get; set; }
        public List<AlbSelectListItem> CodesMateriel { get; set; }
        public string CodeExtension { get; set; }
        public List<AlbSelectListItem> CodesExtension { get; set; }

        public string CodeRisque { get; set; }
   
        public string CodeObjet { get; set; }
        public string Type { get; set; }

        public string DateDebStr { get; set; }
        public string HeureDebStr { get; set; }
        public string MinuteDebStr { get; set; }
        public string DateFinStr { get; set; }
        public string HeureFinStr { get; set; }
        public string MinuteFinStr { get; set; }

        public string NumDescription { get; set; }

        public bool FullScreen { get; set; }

        public double Valeur { get; set; }
        public string UniteLst { get; set; }
        public List<AlbSelectListItem> UniteLsts { get; set; }
        public string TypeLst { get; set; }
        public List<AlbSelectListItem> TypeLsts { get; set; }
        public string TaxeLst { get; set; }
        public List<AlbSelectListItem> TaxeLsts { get; set; }
        [Display(Name = "Activer report")]
        public bool ActiverReport { get; set; }

        public string ValeurObjet { get; set; }
        public string UniteObjet { get; set; }
        public string TypeObjet { get; set; }
        public string ValeurObjetHT { get; set; }

        public string Branche { get; set; }
        public string Cible { get; set; }
        #region propriétés concernant l'inventaire de garantie        
        public string CodeFormule { get; set; }
        public string CodeGarantie { get; set; }
        public string EcranProvenance { get; set; }
        public string CodeOption { get; set; }
        public string FormGen { get; set; }
        public string IdGarantie { get; set; }
        public string InventSpecifique { get; set; }
        public string TypeAlimentation { get; set; }

        public bool NewInven { get; set; }
        #endregion


        public string CodeQualite { get; set; }
        public string DescQualite { get; set; }
        public List<AlbSelectListItem> CodesQualite { get; set; }
        public string CodeRenonce { get; set; }
        public string DescRenonce { get; set; }
        public List<AlbSelectListItem> CodesRenonce { get; set; }
        public string CodeRsqLoc { get; set; }
        public string DescRsqLoc { get; set; }
        public List<AlbSelectListItem> CodesRsqLoc { get; set; }
        public decimal Mnt2 { get; set; }
        public decimal Contenu { get; set; }
        public decimal MatBur { get; set; }

        public string Pays { get; set; }
        public List<AlbSelectListItem> ListePays { get; set; }
  
        public bool? IsAvnDisabled { get; set; }
        public string CodePbmer { get; set; }
        public string CodePbr { get; set; }

        public string CodeContrat { get; set; }
        public string VersionContrat { get; set; }
        public string txtSaveCancel { get; set; }
        public string txtParamRedirect { get; set; }
        public string Observations { get; set; }
          /*public string UnitReport { get; set; }
          public string TypeReport { get; set; }
          public string TaxeReport { get; set; }
          public bool ActiveReport { get; set; }*/





        public static explicit operator Inventaire_MetaModel(InventaireDto inventaireDto)
        {
            var toReturn = ObjectMapperManager.DefaultInstance.GetMapper<InventaireDto, Inventaire_MetaModel>().Map(inventaireDto);
            toReturn.InventaireInfos = inventaireDto.InventaireInfos.Select(x => (ModeleInventaireGridRow)x).ToList();
            return toReturn;
        }

        public static InventaireDto LoadDto(Inventaire_MetaModel modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<Inventaire_MetaModel, InventaireDto>().Map(modele);
        }

        public Inventaire_MetaModel Load(InventaireDto inventaire)
        {
            var toReturn = new Inventaire_MetaModel
            {
                Code = inventaire.Code,
                Descriptif = inventaire.Descriptif ?? string.Empty,
                Description = inventaire.Description ?? string.Empty,
                Type = inventaire.Type.Code,
                NumDescription = inventaire.NumDescription ?? string.Empty
            };


            return toReturn;
        }

        
    }
}