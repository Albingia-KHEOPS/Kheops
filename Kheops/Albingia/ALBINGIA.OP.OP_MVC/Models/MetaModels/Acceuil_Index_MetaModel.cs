namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    /// <summary>
    /// Métamodèle de la vue ConfirmationSaisie
    /// </summary>
    public class Acceuil_Index_MetaModel : MetaModelsBase
    {
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public Accueil_Model SpecificMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Acceuil_Index_MetaModel"/> class.
        /// </summary>
        public Acceuil_Index_MetaModel()
            : base()
        {
            this.SpecificMessage = new Accueil_Model();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Acceuil_Index_MetaModel"/> class.
        /// </summary>
        public Acceuil_Index_MetaModel(string specificMessage)
            : base(specificMessage)
        {
            this.SpecificMessage = new Accueil_Model();
        }

    }
}