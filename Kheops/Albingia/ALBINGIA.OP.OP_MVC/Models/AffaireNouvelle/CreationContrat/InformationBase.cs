using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class InformationBase
    {
        [Required(ErrorMessage = "Veuillez choisir une branche.")]
        [Display(Name = "Branche *")]
        public String Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public Cible_InformationBase Cible_InformationBase { get; set; }
        public Template_InformationBase InformationTemplate { get; set; }
        public String SouscripteurCode { get; set; }
        //[Required(ErrorMessage = "Veuillez choisir un souscripteur.")]
        [Display(Name = "Souscripteur *")]
        public String SouscripteurNom { get; set; }

        public String GestionnaireCode { get; set; }
        //[Required(ErrorMessage = "Veuillez choisir un gestionnaire.")]
        [Display(Name = "Gestionnaire *")]
        public String GestionnaireNom { get; set; }

        [Display(Name = "Accord*")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateAccord { get; set; }

        [Display(Name = "Effet*")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateEffet { get; set; }

        [Display(Name = "Heure d'effet")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureEffet { get; set; }

        [Required(ErrorMessage = "Veuillez choisir un type de contrat.")]
        [Display(Name = "Type de contrat *")]
        public String TypeContrat { get; set; }
        public List<AlbSelectListItem> TypesContrat { get; set; }

        [Display(Name = "Contrat mère")]
        public String ContratMere { get; set; }

        [Display(Name = "N° Aliment")]
        public String NumAliment { get; set; }

        [Display(Name = "Contrat remplacé")]
        public bool ContratRemplace { get; set; }

        public String NumContratRemplace { get; set; }
        public String NumAlimentRemplace { get; set; }

        public bool EditMode { get; set; }
        public bool TemplateMode { get; set; }
        public bool CopyMode { get; set; }
        public bool LoadTemplateMode { get; set; }
        public bool IsReadOnly { get; set; }

        public string ContratMereParam { get; set; }

        [Display(Name="Identification *")]
        public string Descriptif { get; set; }

        public static explicit operator InformationBase(ContratInfoBaseDto ContratInfoBaseDto)
        {
            InformationBase modele = ObjectMapperManager.DefaultInstance.GetMapper<ContratInfoBaseDto, InformationBase>().Map(ContratInfoBaseDto);
            modele.TypesContrat.Clear();
            ContratInfoBaseDto.TypesContrat.ToList().ForEach(
                elem => modele.TypesContrat.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            modele.Branches.Clear();
            ContratInfoBaseDto.Branches.ToList().ForEach(
                elem => modele.Branches.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            return modele;
        }
    }

}