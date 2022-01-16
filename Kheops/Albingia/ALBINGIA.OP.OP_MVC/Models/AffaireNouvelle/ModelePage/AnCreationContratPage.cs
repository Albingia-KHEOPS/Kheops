using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage
{
    public class AnCreationContratPage : MetaModelsBase
    {
        public string CodeContrat { get; set; }
        public long VersionContrat { get; set; }
        public string Type { get; set; }
        public InformationBase InformationBase { get; set; }
        public CourtierApporteur CourtierApporteur { get; set; }
        //public CourtierGestionnaire CourtierGestionnaire { get; set; }
        //public CourtierPayeur CourtierPayeur { get; set; }     
        public CourtierGestionnairePayeur CourtierGestionnairePayeur { get; set; }
        public PreneurAssurance PreneurAssurance { get; set; }   
        public InformationContrat InformationContrat { get; set; }
        public ModeleContactAdresse ContactAdresse { get; set; }
        [Display(Name = "Apporteur et payeur identiques au gestionnaire")]
        public bool GpIdentiqueApporteur { get; set; }
        public bool EditMode { get; set; }
        public bool TemplateMode { get; set; }
       
        [Display(Name = "Encaissement *")]
        public String Encaissement { get; set; }
        public List<AlbSelectListItem> Encaissements { get; set; }
        public ModeleRecherchePage Recherche { get; set; }

        public bool CopyMode { get; set; }
        public string CodeContratCopy { get; set; }
        public string VersionCopy { get; set; }
        public bool LoadTemplateMode { get; set; }

        public ModeleRechercheAvancee ListCabinetCourtageGestion { get; set; }
        public ModeleRechercheAvancee ListCabinetCourtageApporteur { get; set; }
        public ModeleRechercheAvancee ListCabinetCourtagePayeur { get; set; }
    }
}