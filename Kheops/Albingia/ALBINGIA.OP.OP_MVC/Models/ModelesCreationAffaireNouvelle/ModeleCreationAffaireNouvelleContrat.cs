using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    public class ModeleCreationAffaireNouvelleContrat
    {
        [Display(Name = "N° contrat")]
        public string CodeContrat { get; set; }
        public Int32 VersionContrat { get; set; }
        [Display(Name = "Branche *")]
        public string Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        [Display(Name = "Type de contrat *")]
        public string TypeContrat { get; set; }
        public Int64 CodeAvn { get; set; }
        public string LibTypeContrat { get; set; }
        [Display(Name = "Date d'effet *")]
        public DateTime? DateEffet { get; set; }
        [Display(Name = "Heure d'effet *")]
        public TimeSpan? HeureEffet { get; set; }
        //public string MinuteEffet { get; set; }
        [Display(Name = "Date d'accord *")]
        public DateTime DateAccord { get; set; }
        [Display(Name = "Contrat remplacé")]
        public string CodeContratRemplace { get; set; }
        public bool EstContratRemplace { get; set; }
        public Int32 ContratRemplaceAliment { get; set; }
        [Display(Name = "Souscripteur *")]
        public string Souscripteur { get; set; }
        [Display(Name = "Gestionnaire *")]
        public string Gestionnaire { get; set; }
        public string GestionnaireCode { get; set; }
        [Display(Name = "Etat")]
        public string Etat { get; set; }
        [Display(Name = "Contrat mère")]
        public string ContratMere { get; set; }
        [Display(Name = "N° Aliment")]
        public string Aliment { get; set; }

        public List<AlbSelectListItem> TypesContrat { get; set; }

        public List<AlbSelectListItem> Souscripteurs { get; set; }
        public List<AlbSelectListItem> Gestionnaires { get; set; }


        public static explicit operator ModeleCreationAffaireNouvelleContrat(CreationAffaireNouvelleContratDto creationAffNouvContratDto)
        {
            ModeleCreationAffaireNouvelleContrat modele = ObjectMapperManager.DefaultInstance.GetMapper<CreationAffaireNouvelleContratDto, ModeleCreationAffaireNouvelleContrat>().Map(creationAffNouvContratDto);
            modele.TypesContrat.Clear();
            creationAffNouvContratDto.TypesContrat.ToList().ForEach(
                elem => modele.TypesContrat.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            return modele;
        }

        public static CreationAffaireNouvelleContratDto LoadDto(ModeleCreationAffaireNouvelleContrat modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleCreationAffaireNouvelleContrat, CreationAffaireNouvelleContratDto>().Map(modele);
        }

    }
}