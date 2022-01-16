using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Clause
{
    public class ClauseExecListDto
    {

        [Column(Name = "ID")]
        public Int64 Id { get; set; }
        [Column(Name = "CODERISQUE")]
        public Int32 CodeRisque { get; set; }
        [Column(Name = "DESCRISQUE")]
        public string DescRisque { get; set; }
        [Column(Name = "CODEOBJET")]
        public Int32 CodeObjet { get; set; }
        [Column(Name = "DESCOBJET")]
        public string DescObjet { get; set; }
        [Column(Name = "CODEFORMULE")]
        public Int32 CodeFormule { get; set; }
        [Column(Name = "DESCFORMULE")]
        public string DescFormule { get; set; }
        [DataMember]
        [Column(Name = "LETTREFORMULE")]
        public string LettreFormule { get; set; }
        [DataMember]
        [Column(Name = "RUBRIQUE")]
        public string Rubrique { get; set; }
        [DataMember]
        [Column(Name = "SOUSRUBRIQUE")]
        public string SousRubrique { get; set; }
        [DataMember]
        [Column(Name = "SEQUENCE")]
        public Int64 Sequence { get; set; }
        [DataMember]
        [Column(Name = "LEDES")]
        public string Titre { get; set; }
        [DataMember]
        [Column(Name = "NUMEROVERSION")]
        public Int32 NumeroVersion { get; set; }
        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        [DataMember]
        public string Branche { get; set; }
        /// <summary>
        /// Gets or sets the date effet version debut.
        /// </summary>
        /// <value>
        /// The date effet version debut.
        /// </value>
        [DataMember]
        public DateTime? DateEffetVersionDebut { get; set; }
        [Column(Name = "COLUMNDATEEFFETVERSIONDEBUT")]
        public Int32 ColumnDateEffetVersionDebut { get; set; }
   
        /// <summary>
        /// Gets or sets the date effet version fin.
        /// </summary>
        /// <value>
        /// The date effet version fin.
        /// </value>
        [DataMember]
        public DateTime? DateEffetVersionFin { get; set; }
        [Column(Name = "COLUMNDATEEFFETVERSIONFIN")]
        public Int32 ColumnDateEffetVersionFin { get; set; }
       
        /// <summary>
        /// Gets or sets the origine.
        /// </summary>
        /// <value>
        /// The origine.
        /// </value>
        [DataMember]
        [Column(Name = "ORIGINE")]
        public string Origine { get; set; }
        /// <summary>
        /// Gets or sets the etape.
        /// </summary>
        /// <value>
        /// The etape.
        /// </value>
        [DataMember]
        [Column(Name = "ETAPE")]
        public string Etape { get; set; }
        /// <summary>
        /// Gets or sets the contexte.
        /// </summary>
        /// <value>
        /// The contexte.
        /// </value>
        [DataMember]
        [Column(Name = "CONTEXTE")]
        public string Contexte { get; set; }
        /// <summary>
        /// Label du contexte
        /// </summary>
        [DataMember]
        [Column(Name = "CTXLBL")]
        public string ContexteLabel { get; set; }
        /// <summary>
        /// Gets or sets the code situation.
        /// </summary>
        /// <value>
        /// The code situation.
        /// </value>
        [DataMember]
        [Column(Name = "CODESITUATION")]
        public string CodeSituation { get; set; }
        /// <summary>
        /// Gets or sets the date situation.
        /// </summary>
        /// <value>
        /// The date situation.
        /// </value>
        [DataMember]
        public DateTime? DateSituation { get; set; }
        [Column(Name = "DATESITUATION")]
        public Int32 ColumnDateSituation { get; set; }
        /// <summary>
        /// Gets or sets the numero avenant situation.
        /// </summary>
        /// <value>
        /// The numero avenant situation.
        /// </value>
        [DataMember]
        [Column(Name = "NUMEROAVENANTCREATION")]
        public Int32 NumeroAvenantCreation { get; set; }
        /// <summary>
        /// Gets or sets the date avenant creation.
        /// </summary>
        /// <value>
        /// The date avenant creation.
        /// </value>
        [DataMember]
        public DateTime? DateAvenantCreation { get; set; }
        [Column(Name = "DATEAVENANTCREATION")]
        public Int32 ColumnDateAvenantCreation { get; set; }
        /// <summary>
        /// Gets or sets the numero avenant modification.
        /// </summary>
        /// <value>
        /// The numero avenant modification.
        /// </value>
        [DataMember]
        [Column(Name = "NUMEROAVENANTMODIFICATION")]
        public Int32 NumeroAvenantModification { get; set; }
        /// <summary>
        /// Gets or sets the date avenant modification.
        /// </summary>
        /// <value>
        /// The date avenant modification.
        /// </value>
        [DataMember]
        public DateTime? DateAvenantModification { get; set; }
        [Column(Name = "NUMEROAVENANTMODIFICATION")]
        public Int32 ColumnDateAvenantModification { get; set; }
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        [DataMember]
        [Column(Name = "DOCUMENT")]
        public string Document { get; set; }
        /// <summary>
        /// Gets or sets the chapitre.
        /// </summary>
        /// <value>
        /// The chapitre.
        /// </value>
        [DataMember]
        [Column(Name = "CHAPITRE")]
        public string Chapitre { get; set; }
        /// <summary>
        /// Gets or sets the sous chapitre.
        /// </summary>
        /// <value>
        /// The sous chapitre.
        /// </value>
        [DataMember]
        [Column(Name = "SOUSCHAPITRE")]
        public string SousChapitre { get; set; }
        /// <summary>
        /// Gets or sets the numero impression.
        /// </summary>
        /// <value>
        /// The numero impression.
        /// </value>
        [DataMember]
        [Column(Name = "NUMEROIMPRESSION")]
        public Int64 NumeroImpression { get; set; }
        /// <summary>
        /// Gets or sets the numero ordre.
        /// </summary>
        /// <value>
        /// The numero ordre.
        /// </value>
        [DataMember]
        [Column(Name = "NUMEROORDRE")]
        public Single NumeroOrdre { get; set; }
        /// <summary>
        /// Gets or sets the impression annexe.
        /// </summary>
        /// <value>
        /// The impression annexe.
        /// </value>
        [DataMember]
        [Column(Name = "IMPRESSIONANNEXE")]
        public string ImpressionAnnexe { get; set; }
        /// <summary>
        /// Gets or sets the code annexe.
        /// </summary>
        /// <value>
        /// The code annexe.
        /// </value>
        [DataMember]
        [Column(Name = "CODEANNEXE")]
        public string CodeAnnexe { get; set; }
        /// <summary>
        /// Gets or sets the action enchaine.
        /// </summary>
        /// <value>
        /// The action enchaine.
        /// </value>
        [DataMember]
        [Column(Name = "ACTIONENCHAINE")]
        public string ActionEnchaine { get; set; }
        /// <summary>
        /// Gets or sets the etat titre.
        /// </summary>
        /// <value>
        /// The etat titre.
        /// </value>
        [DataMember]
        [Column(Name = "ETATTITRE")]
        public string EtatTitre { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is check.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is check; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsCheck { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is check origine.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is check origine; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsCheckOrigine { get; set; }
        /// <summary>
        /// Etat indiquant si la clause est une clause libre ou non
        /// </summary>
        [DataMember]
        public bool IsClauseLibre { get; set; }
        [Column(Name = "ISCLAUSELIBRE")]
        public Int32 ClauseLibre { get; set; }
        /// <summary>
        /// Contient le texte de la clause libre (si elle est libre)
        /// </summary>
        [DataMember]
        [Column(Name = "TEXTECLAUSELIBRE")]
        public string TexteClauseLibre { get; set; }
        /// <summary>
        /// Gets or sets the attribut version.
        /// </summary>
        /// <value>
        /// The attribut version.
        /// </value>
        [DataMember]
        [Column(Name = "ATTRIBUTVERSION")]
        public string AttributVersion { get; set; }
        /// <summary>
        /// Gets or sets the garantie.
        /// </summary>
        /// <value>
        /// The garantie.
        /// </value>
        [DataMember]
        public string Garantie { get; set; }
        /// <summary>
        /// Gets or sets the edition.
        /// </summary>
        /// <value>
        /// The edition.
        /// </value>
        [DataMember]
        public string Edition { get; set; }
        /// <summary>
        /// Gets or sets the emplacement complet.
        /// </summary>
        /// <value>
        /// The emplacement complet.
        /// </value>
        [DataMember]
        public string EmplacementComplet { get; set; }
        //Ajouté par MSL le 07/05/2013
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }
        [Column(Name = "LIBELLEGARANTIE")]
        public string LibelleGarantie { get; set; }
    }
}
