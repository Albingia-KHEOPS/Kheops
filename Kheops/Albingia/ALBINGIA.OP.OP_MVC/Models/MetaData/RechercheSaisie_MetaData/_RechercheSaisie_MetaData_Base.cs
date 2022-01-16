using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.RechercheSaisie_MetaData
{
    public abstract class _RechercheSaisie_MetaData_Base : ModelsBase
    {
        #region Grille Principale

        public object DeleteColumnButton { get; set; }

        public virtual int i { get; set; }
        public virtual string Type { get; set; }
        [
            Display(Name = "N°"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string OffreContratNum { get; set; }
        [
            Display(Name = "V."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Version { get; set; }
        [
            Display(Name = "Date"),
            DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = ""),
            DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)
        ]
        public virtual Nullable<DateTime> Date { get; set; }
        [
            Display(Name = "B.", Description = "Branche"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Branche { get; set; }
        [
            Display(Name = "Pren. Ass."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string PreneurAssuranceNom { get; set; }
        [
            Display(Name = "C.P."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string PreneurAssuranceCP { get; set; }
        [
            Display(Name = "Etat"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Etat { get; set; }
        [
            Display(Name = "Sit."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Situation { get; set; }
        [
            Display(Name = "Qual."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Qualite { get; set; }
        [
            Display(Name = "Courtier Gest."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string CourtierGestionnaireNom { get; set; }
        [
            Display(Name = "C.P."),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string CourtierGestionnaireCP { get; set; }
        [
            Display(Name = "Descriptif risque"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string DescriptifRisque { get; set; }
        #endregion

        #region Sous Grille
        [
            Display(Name = "Date MAJ"),
            DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = ""),
            DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)
        ]
        public virtual Nullable<DateTime> DateDerniereMAJ { get; set; }
        [
            Display(Name = "Souscripteur"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Souscripteur { get; set; }
        [
            Display(Name = "N° Preneur"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string PreneurAssuranceNum { get; set; }
        [
            Display(Name = "Ville Preneur"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string PreneurAssuranceVille { get; set; }
        [
            Display(Name = "N° Courtier"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string CourtierGestionnaireNum { get; set; }
        [
            Display(Name = "Ville Courtier"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string CourtierGestionnaireVille { get; set; }
        [
            Display(Name = "Motif refus ou résil"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string MotifRefus { get; set; }
        #endregion

        public _RechercheSaisie_MetaData_Base()
        {
            this.OffreContratNum = string.Empty;
            this.Version = string.Empty;
            this.Date = null;
            this.Branche = string.Empty;
            this.PreneurAssuranceNom = string.Empty;
            this.PreneurAssuranceCP = string.Empty;
            this.Etat = string.Empty;
            this.Situation = string.Empty;
            this.Qualite = string.Empty;
            this.CourtierGestionnaireNom = string.Empty;
            this.CourtierGestionnaireCP = string.Empty;
            this.DescriptifRisque = string.Empty;
            this.DateDerniereMAJ = null;
            this.Souscripteur = string.Empty;
            this.PreneurAssuranceNum = string.Empty;
            this.PreneurAssuranceVille = string.Empty;
            this.CourtierGestionnaireNum = string.Empty;
            this.CourtierGestionnaireVille = string.Empty;
            this.MotifRefus = string.Empty;
        }
    }
}