using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.CabinetCourtage_MetaData
{
    public abstract class _CabinetCourtage_MetaData_Base
    {
        public enum enTypesCabinetCourtage
        {
            Apporteur = 0,
            Gestionnaire = 1,
            Autres = 2,
            Demandeur = 3
        }

        public virtual enTypesCabinetCourtage TypeCabinetCourtage { get; set; }
        [
            Display(Name = "Code"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        [Required]
        public virtual string Code { get; set; }
        [
            Display(Name = "Courtier"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Courtier { get; set; }
        public virtual string ValidCourtier { get; set; }
        [
            Display(Name = "Délégation"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        public virtual string Delegation { get; set; }
        [
            Display(Name = "Souscripteur"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Text)
        ]
        [Required]
        public virtual string Souscripteur { get; set; }
        public virtual string SouscripteurValid { get; set; }
        [
            Display(Name = "Date"),
            DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = ""),
            DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)
        ]
        [Required]
        public virtual Nullable<DateTime> SaisieDate { get; set; }
        [
            Display(Name = "Heure"),
            DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}"),
            DataType(System.ComponentModel.DataAnnotations.DataType.Time),
            RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")
        ]
        public virtual Nullable<TimeSpan> SaisieHeure { get; set; }
        [
            Display(Name = "Date"),
            DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = ""),
            DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)
        ]
        public virtual Nullable<DateTime> EnregistrementDate { get; set; }
        [
            Display(Name = "Heure"),
            DisplayFormat(DataFormatString = "{0:hh:mm}", NullDisplayText = ""),
            DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)
        ]
        public virtual Nullable<TimeSpan> EnregistrementHeure { get; set; }

        public _CabinetCourtage_MetaData_Base(enTypesCabinetCourtage typeCabinetCourtage)
        {
            this.TypeCabinetCourtage = typeCabinetCourtage;
            this.Code = string.Empty;
            this.Courtier = string.Empty;
            this.Delegation = string.Empty;
            this.EnregistrementDate = null;
            this.EnregistrementHeure = null;
            this.SaisieDate = null;
            this.SaisieHeure = null;
            this.Souscripteur = string.Empty;
            this.SouscripteurValid = string.Empty;
            this.ValidCourtier = string.Empty;
        }
    }
}