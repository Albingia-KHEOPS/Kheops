using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData;
using ALBINGIA.OP.OP_MVC.Models.MetaData;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    /// <summary>
    /// meta model de l'index de la page creation de saisie.
    /// </summary>
    /// 
    public class CreationInformationSaisieDivJqueryUpdate_MetaData : ModelsBase
    {
        /// <summary>
        /// Gets or sets the information saisie meta data.
        /// </summary>
        /// <value>
        /// The information saisie meta data.
        /// </value>
        public InformationSaisie_MetaData InformationSaisieMetaData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreationInformationSaisieDivJqueryUpdate_MetaData"/> class.
        /// </summary>
        public CreationInformationSaisieDivJqueryUpdate_MetaData()
        {
            InformationSaisieMetaData = new InformationSaisie_MetaData();
        }
    }
}