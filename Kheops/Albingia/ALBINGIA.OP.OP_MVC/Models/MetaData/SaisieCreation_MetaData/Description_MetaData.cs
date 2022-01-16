using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    /// <summary>
    /// 
    /// </summary>
    public class Description_MetaData : _SaisieCreation_MetaData_Base
    {
        /// <summary>
        /// Gets or sets the mots clefs.
        /// </summary>
        /// <value>
        /// The mots clefs.
        /// </value>
        [Display(Name = "Mots-clés")]
        public List<AlbSelectListItem> MotsClefs { get; set; }

        /// <summary>
        /// Gets or sets the mot clef1.
        /// </summary>
        /// <value>
        /// The mot clef1.
        /// </value>
        public string MotClef1 { get; set; }
        public List<AlbSelectListItem> MotsClefs1 { get; set; }

        /// <summary>
        /// Gets or sets the mot clef2.
        /// </summary>
        /// <value>
        /// The mot clef2.
        /// </value>
        public string MotClef2 { get; set; }
        public List<AlbSelectListItem> MotsClefs2 { get; set; }

        /// <summary>
        /// Gets or sets the mot clef3.
        /// </summary>
        /// <value>
        /// The mot clef3.
        /// </value>
        public string MotClef3 { get; set; }
        public List<AlbSelectListItem> MotsClefs3 { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [Display(Name = "Descriptif")]
        public string Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the observation.
        /// </summary>
        /// <value>
        /// The observation.
        /// </value>
        [Display(Name = "Observations")]
        public string Observation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Description_MetaData"/> class.
        /// </summary>
        public Description_MetaData()
        {
            MotsClefs = new List<AlbSelectListItem>();
            MotClef1 = String.Empty;
            MotClef2 = String.Empty;
            MotClef3 = String.Empty;
            Descriptif = String.Empty;
            Observation = String.Empty;
        }
    }
}