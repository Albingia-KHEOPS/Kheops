using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class CabinetCourtage_JSON_MetaData
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the nom secondaires.
        /// </summary>
        /// <value>
        /// The nom secondaires.
        /// </value>
        public string[] NomSecondaires { get; set; }
        
        /// <summary>
        /// Gets or sets the rue.
        /// </summary>
        /// <value>
        /// The rue.
        /// </value>
        public string Rue { get; set; } // HLE le 13/12/11

        /// <summary>
        /// Gets or sets the code postal.
        /// </summary>
        /// <value>
        /// The code postal.
        /// </value>
        public string CodePostal { get; set; }

        /// <summary>
        /// Gets or sets the ville.
        /// </summary>
        /// <value>
        /// The ville.
        /// </value>
        public string Ville { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the nom interlocuteur.
        /// </summary>
        /// <value>
        /// The nom interlocuteur.
        /// </value>
        public string NomInterlocuteur { get; set; }

        /// <summary>
        /// Gets or sets the id interlocuteur.
        /// </summary>
        /// <value>
        /// The id interlocuteur.
        /// </value>
        public string IdInterlocuteur { get; set; }

        /// <summary>
        /// Gets or sets the valide interlocuteur.
        /// </summary>
        /// <value>
        /// The valide interlocuteur.
        /// </value>
        public bool ValideInterlocuteur { get; set; }

        /// <summary>
        /// Gets or sets the delegation.
        /// </summary>
        /// <value>
        /// The delegation.
        /// </value>
        public string Delegation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [est valide].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [est valide]; otherwise, <c>false</c>.
        /// </value>
        public bool EstValide { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtage_JSON_MetaData"/> class.
        /// </summary>
        public CabinetCourtage_JSON_MetaData()
        {
            Code = null;
            Nom = String.Empty;
            CodePostal = String.Empty;
            Ville = String.Empty;
            Type = String.Empty;
            NomInterlocuteur = String.Empty;
            IdInterlocuteur = String.Empty;
            EstValide = true;
            Delegation = String.Empty;
        }

    }
}