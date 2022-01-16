using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsObjet;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleDetailsObjetRisquePage : MetaModelsBase
    {
        #region Avenant
        public DateTime? DateEffetAvenant { get; set; }
        public TimeSpan? HeureEffetAvenant { get; set; }
        public bool IsAvenantModificationLocale { get; set; }
        public DateTime? DateEffetAvenantModificationLocale { get; set; }

        public DateTime? DateModifAvenantModificationLocale { get; set; }
        public DateTime? DateModificationObjetRisque { get; set; }
        public int AvnCreationObj { get; set; } //Numéro d'avenant dans lequel a été créé le objet courant
        public bool IsModeAvenant { get; set; }
        public bool IsRisqueIndexe { get; set; }
        public string RedirectRisque { get; set; } //Savoir la redirection à partir du risque (IS, add obj ou open obj)
        public bool? IsAvnDisabled { get; set; }
        #endregion

        public string txtParamRedirect { get; set; }
        public string CodeRisque { get; set; }
        public string CodeObjet { get; set; }
        public int ChronoDesi { get; set; }
        public string DescrRisque { get; set; }
        public int CountObj { get; set; } //Connaitre le nombre d'objet du risque
       
        public ModeleInformationsGeneralesRisque InformationsGenerales { get; set; }
        public ModeleContactAdresse ContactAdresse { get; set; }
        //public ContactAddresse_MetaData ContactAddresseMetaData { get; set; }
        public ModeleDetailsObjetInventaire ListInventaires { get; set; }

        public string DateDebStr { get; set; }
        public string HeureDebStr { get; set; }
        public string MinuteDebStr { get; set; }
        public string DateFinStr { get; set; }
        public string HeureFinStr { get; set; }
        public string MinuteFinStr { get; set; }

        public bool HasFormules { get; set; }

        public bool ModeCreationRisque { get; set; }

        public string ParamOpenInven { get; set; }

        public string DateDebHisto { get; set; }
        public string HeureDebHisto { get; set; }
        public string Branche { get; set; }
      

        public ModeleDetailsObjetRisquePage()
        {
            InformationsGenerales = new ModeleInformationsGeneralesRisque();
            InformationsGenerales.Cibles = new List<AlbSelectListItem>();
            InformationsGenerales.Types = new List<AlbSelectListItem>();
            InformationsGenerales.Unites = new List<AlbSelectListItem>();
            InformationsGenerales.ValeursHT = new List<AlbSelectListItem>();
            InformationsGenerales.DisplayInfosValeur = true;
            ListInventaires = new ModeleDetailsObjetInventaire();
            ContactAdresse = new ModeleContactAdresse(11, true, true);
        }
    }
}