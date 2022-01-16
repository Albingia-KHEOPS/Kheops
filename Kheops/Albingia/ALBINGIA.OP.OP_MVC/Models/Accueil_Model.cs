using System;
using System.Configuration;
namespace ALBINGIA.OP.OP_MVC.Models
{
    /// <summary>
    /// Modèle de la page d'accueil
    /// </summary>
    public class Accueil_Model : ModelBase
    {
        /// <summary>
        /// Gets or sets the message accueil.
        /// </summary>
        /// <value>
        /// The message accueil.
        /// </value>

        public string MessageAccueil { get; set; }

        /// <summary>
        /// Gets or sets the message pied de page.
        /// </summary>
        /// <value>
        /// The message pied de page.
        /// </value>
        public string MessagePiedDePage { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Accueil_Model"/> class.
        /// </summary>
        public Accueil_Model()
        {
            this.MessageAccueil = ConfigurationManager.AppSettings["MessageAccueil"] != null ? string.Format("Bienvenue sur le portail Outils de Production, nous sommes le {0:dd/MM/yyyy} !", DateTime.Now) : "#Erreur : Message d'accueil non renseigné";
            this.MessagePiedDePage = "© Albingia";
        }
    }
}