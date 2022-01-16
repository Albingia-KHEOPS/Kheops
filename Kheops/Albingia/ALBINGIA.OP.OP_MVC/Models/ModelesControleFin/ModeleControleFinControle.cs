using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre
{
    [Serializable]
    public class ModeleControleFinControle
    {
        /// <summary>
        /// Etape de génération
        /// </summary>
        public string Etape { get; set; }
        /// <summary>
        /// Risque
        /// </summary>
        public string Risque { get; set; }
        /// <summary>
        /// Objet
        /// </summary>
        public string Objet { get; set; }
        /// <summary>
        /// ID KPINVEN
        /// </summary>
        public string IdInventaire { get; set; }
        /// <summary>
        /// Numéro de ligne inventaire
        /// </summary>
        public string NumeroLigneInventaire { get; set; }
        /// <summary>
        /// Formule
        /// </summary>
        public string Formule { get; set; }
        /// <summary>
        /// Option
        /// </summary>
        public string Option { get; set; }
        /// <summary>
        /// Garantie
        /// </summary>
        public string Garantie { get; set; }
        /// <summary>
        /// Texte du message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Niv classement Bloquant/NonVal/Avert
        /// </summary>
        public string Niveau { get; set; }
        /// <summary>
        /// Référence
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Lien référence
        /// </summary>
        public string LienReference { get; set; }

    }
}