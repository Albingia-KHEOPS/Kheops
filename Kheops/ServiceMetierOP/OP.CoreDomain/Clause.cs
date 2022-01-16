using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    ////[Serializable]
    public class Clause
    {
        /// <summary>
        ///  Gets or sets the Id.
        /// </summary>
        /// <value>
        /// The Id.
        /// </value>
        public Int64 Id { get; set; }
        ///// <summary>
        ///// Gets or sets the code.
        ///// </summary>
        ///// <value>
        ///// The code.
        ///// </value>
        //public string Code { get; set; }    
        public Int32 CodeRisque { get; set; }
        public string DescRisque { get; set; }

        public Int32 CodeObjet { get; set; }
        public string DescObjet { get; set; }

        public Int32 CodeFormule { get; set; }
        public string DescFormule { get; set; }
        public string LettreFormule { get; set; }
        public string Rubrique { get; set; }
        public string SousRubrique { get; set; }
        public Int64 Sequence { get; set; }
        /// <summary>
        /// Gets or sets the titre.
        /// </summary>
        /// <value>
        /// The titre.
        /// </value>
        public string Titre { get; set; }
        /// <summary>
        /// Gets or sets the numero version.
        /// </summary>
        /// <value>
        /// The numero version.
        /// </value>
        public Int32 NumeroVersion { get; set; }
        ///// <summary>
        ///// Gets or sets the historique.
        ///// </summary>
        ///// <value>
        ///// The historique.
        ///// </value>
        //public List<Version> Historique { get; set; }
        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        public string Branche { get; set; }
        /// <summary>
        /// Gets or sets the date effet version debut.
        /// </summary>
        /// <value>
        /// The date effet version debut.
        /// </value>
        public DateTime DateEffetVersionDebut { get; set; }
        /// <summary>
        /// Gets or sets the date effet version fin.
        /// </summary>
        /// <value>
        /// The date effet version fin.
        /// </value>
        public DateTime DateEffetVersionFin { get; set; }
        /// <summary>
        /// Gets or sets the origine.
        /// </summary>
        /// <value>
        /// The origine.
        /// </value>
        public string Origine { get; set; }
        /// <summary>
        /// Gets or sets the etape.
        /// </summary>
        /// <value>
        /// The etape.
        /// </value>
        public string Etape { get; set; }
        /// <summary>
        /// Gets or sets the contexte.
        /// </summary>
        /// <value>
        /// The contexte.
        /// </value>
        public string Contexte { get; set; }
        /// <summary>
        /// Le label du contexte
        /// </summary>
        public string ContexteLabel { get; set; }
        /// <summary>
        /// Gets or sets the code situation.
        /// </summary>
        /// <value>
        /// The code situation.
        /// </value>
        public string CodeSituation { get; set; }
        /// <summary>
        /// Gets or sets the date situation.
        /// </summary>
        /// <value>
        /// The date situation.
        /// </value>
        public DateTime? DateSituation { get; set; }
        /// <summary>
        /// Gets or sets the numero avenant situation.
        /// </summary>
        /// <value>
        /// The numero avenant situation.
        /// </value>
        public string NumeroAvenantCreation { get; set; }
        /// <summary>
        /// Gets or sets the date avenant creation.
        /// </summary>
        /// <value>
        /// The date avenant creation.
        /// </value>
        public DateTime? DateAvenantCreation { get; set; }
        /// <summary>
        /// Gets or sets the numero avenant modification.
        /// </summary>
        /// <value>
        /// The numero avenant modification.
        /// </value>
        public string NumeroAvenantModification { get; set; }
        /// <summary>
        /// Gets or sets the date avenant modification.
        /// </summary>
        /// <value>
        /// The date avenant modification.
        /// </value>
        public DateTime? DateAvenantModification { get; set; }
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public string Document { get; set; }
        /// <summary>
        /// Gets or sets the chapitre.
        /// </summary>
        /// <value>
        /// The chapitre.
        /// </value>
        public string Chapitre { get; set; }
        /// <summary>
        /// Gets or sets the sous chapitre.
        /// </summary>
        /// <value>
        /// The sous chapitre.
        /// </value>
        public string SousChapitre { get; set; }
        /// <summary>
        /// Gets or sets the numero impression.
        /// </summary>
        /// <value>
        /// The numero impression.
        /// </value>
        public string NumeroImpression { get; set; }
        /// <summary>
        /// Gets or sets the numero ordre.
        /// </summary>
        /// <value>
        /// The numero ordre.
        /// </value>
        public string NumeroOrdre { get; set; }
        /// <summary>
        /// Gets or sets the impression annexe.
        /// </summary>
        /// <value>
        /// The impression annexe.
        /// </value>
        public string ImpressionAnnexe { get; set; }
        /// <summary>
        /// Gets or sets the code annexe.
        /// </summary>
        /// <value>
        /// The code annexe.
        /// </value>
        public string CodeAnnexe { get; set; }
        /// <summary>
        /// Gets or sets the action enchaine.
        /// </summary>
        /// <value>
        /// The action enchaine.
        /// </value>
        public string ActionEnchaine { get; set; }
        /// <summary>
        /// Gets or sets the etat titre.
        /// </summary>
        /// <value>
        /// The etat titre.
        /// </value>
        public string EtatTitre { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is check.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is check; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheck { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is check origine.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is check origine; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckOrigine { get; set; }
        /// <summary>
        /// Etat indiquant si la clause est une clause libre ou non
        /// </summary>
        public bool IsClauseLibre { get; set; }
        /// <summary>
        /// Contient le texte de la clause libre (si elle est libre)
        /// </summary>     
        public string TexteClauseLibre { get; set; }
        /// <summary>
        /// Gets or sets the attribut version.
        /// </summary>
        /// <value>
        /// The attribut version.
        /// </value>
        public string AttributVersion { get; set; }
        /// <summary>
        /// Gets or sets the garantie.
        /// </summary>
        /// <value>
        /// The garantie.
        /// </value>
        public string Garantie { get; set; }
        /// <summary>
        /// Gets or sets the edition.
        /// </summary>
        /// <value>
        /// The edition.
        /// </value>
        public string Edition { get; set; }
        /// <summary>
        /// Gets or sets the emplacement complet.
        /// </summary>
        /// <value>
        /// The emplacement complet.
        /// </value>
        public string EmplacementComplet { get; set; }

        //Ajouté par MSL le 07/05/2013
        public string CodeGarantie { get; set; }
        public string LibelleGarantie { get; set; }

        public Clause()
        {
            //Id = String.Empty;
            //Code = String.Empty;
            Titre = String.Empty;
            //NumeroVersion = String.Empty;
            Branche = String.Empty;
            DateEffetVersionDebut = DateTime.MinValue;
            DateEffetVersionFin = DateTime.MinValue;
            Origine = String.Empty;
            Etape = String.Empty;
            Contexte = String.Empty;
            CodeSituation = String.Empty;
            DateSituation = DateTime.MinValue;
            NumeroAvenantCreation = String.Empty;
            DateAvenantCreation = DateTime.MinValue;
            NumeroAvenantModification = String.Empty;
            DateAvenantModification = DateTime.MinValue;
            Document = String.Empty;
            Chapitre = String.Empty;
            SousChapitre = String.Empty;
            NumeroImpression = String.Empty;
            NumeroOrdre = String.Empty;
            ImpressionAnnexe = String.Empty;
            CodeAnnexe = String.Empty;
            ActionEnchaine = String.Empty;
            EtatTitre = String.Empty;
            AttributVersion = String.Empty;
            Garantie = String.Empty;
            Edition = String.Empty;
            EmplacementComplet = String.Empty;
        }
    }
}
