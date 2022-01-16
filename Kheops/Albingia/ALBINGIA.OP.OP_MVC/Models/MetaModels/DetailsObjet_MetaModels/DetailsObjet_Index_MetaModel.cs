using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaData.DetailsRisque_MetaData;
using ALBINGIA.OP.OP_MVC.Models.MetaData;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsObjet;

namespace ALBINGIA.OP.OP_MVC.Models.MetaModels.DetailsObjet_MetaModels
{
    [Serializable]
    public class DetailsObjet_Index_MetaModel : MetaModelsBase
    {
        public string txtSaveCancel { get; set; }
        public string txtParamRedirect { get; set; }
        public string CodeRisque { get; set; }
        public string CodeObjet { get; set; }
        public int ChronoDesi { get; set; }
        public string DescrRisque { get; set; }
        public int CountObj { get; set; } //Connaitre le nombre d'objet du risque

        public InformationsGenerales_MetaData InformationsGeneralesMetaData { get; set; }
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

        public DetailsObjet_Index_MetaModel()
        {
            this.InformationsGeneralesMetaData = new InformationsGenerales_MetaData();
            this.ContactAdresse = new ModeleContactAdresse(11, true, true);
            this.ListInventaires = new ModeleDetailsObjetInventaire();
        }


    }
}