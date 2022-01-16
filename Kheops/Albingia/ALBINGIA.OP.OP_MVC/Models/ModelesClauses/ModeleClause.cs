using EmitMapper;
using OP.WSAS400.DTO.Clause;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleClause
    {
        public bool IsReadOnlyMode { get; set; }
        public bool IsModifHorsAvenant { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public long Id { get; set; }
        public Int64 ClauseId { get; set; }
        ///// <summary>
        ///// Gets or sets the code.
        ///// </summary>
        ///// <value>
        ///// The code.
        ///// </value>
        //[Display(Name="Clause")]
        //public string Code { get; set; }
        public string CodeRisque { get; set; }
        public string DescRisque { get; set; }

        public string CodeObjet { get; set; }
        public string DescObjet { get; set; }

        public string CodeFormule { get; set; }
        public string DescFormule { get; set; }
        public string LettreFormule { get; set; }

        public string CodeGarantie { get; set; }
        public string LibelleGarantie { get; set; }

        public string Rubrique { get; set; }
        public string SousRubrique { get; set; }
        public string Sequence { get; set; }
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
        [Display(Name = "Version")]
        public string NumeroVersion { get; set; }
        /// <summary>
        /// Gets or sets the date effet version debut.
        /// </summary>
        /// <value>
        /// The date effet version debut.
        /// </value>
        public DateTime? DateEffetVersionDebut { get; set; }
        /// <summary>
        /// Gets or sets the date effet version fin.
        /// </summary>
        /// <value>
        /// The date effet version fin.
        /// </value>
        public DateTime? DateEffetVersionFin { get; set; }

        public DateTime? DateDetailAvnCreation { get; set; }
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
        [Display(Name = "Etape")]
        public string Etape { get; set; }
        /// <summary>
        /// Gets or sets the contexte.
        /// </summary>
        /// <value>
        /// The contexte.
        /// </value>
        [Display(Name = "Contexte")]
        public string Contexte { get; set; }
        /// <summary>
        /// Label du contexte
        /// </summary>
        public string ContexteLabel { get; set; }
        /// <summary>
        /// Gets or sets the code situation.
        /// </summary>
        /// <value>
        /// The code situation.
        /// </value>
        [Display(Name = "Situation")]
        public string CodeSituation { get; set; }
        /// <summary>
        /// Gets or sets the date situation.
        /// </summary>
        /// <value>
        /// The date situation.
        /// </value>
        public DateTime? DateSituation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [indique avenant specifique].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [indique avenant specifique]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Spécifique avenant")]
        public bool IndiqueAvenantSpecifique { get; set; }
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
        /// Gets or sets a value indicating whether [indique impression annexe].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [indique impression annexe]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Impression en annexe")]
        public bool IndiqueImpressionAnnexe { get; set; }
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
        /// Is this data archived to the history track ?
        /// </summary>
        /// <value>
        ///   <c>true</c> if we are reading archived data; otherwise, <c>false</c>.
        /// </value>
        public bool isModeHisto { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is check origine.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is check origine; otherwise, <c>false</c>.
        /// </value>
        public bool IsCheckHistorique { get; set; }
        /// <summary>
        /// Etat indiquant si la clause est une clause libre ou non
        /// </summary>
        public bool IsClauseLibre { get; set; }
        /// <summary>
        /// Contient le texte de la clause libre (si elle est libre)
        /// </summary>
        public string TexteClauseLibre { get; set; }
        /// <summary>
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

        public bool IsModif { get; set; }
        public bool IsModifAvenant { get; set; }
        //SAB24042016: Pagination clause 'Ajout NbCount'
         public int NbCount { get; set; }
        public string Risque { get; set; }
        public ModeleObjetsRisque ObjetsRisqueAll { get; set; }
        public int NbrObjets { get; set; }
        public string CodeRsq { get; set; }
        public string DescRsq { get; set; }
        public string LibApplique { get; set; }
        public string DescRsqObj { get; set; }
        public string IdObj { get; set; }
        public bool IsRsqSelected { get; set; }
        public string FullFileName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int NumAvenantPage { get; set; }
        public string NumeroAvenantHisto { get; set; }
        public string NumAvnDetailClauseAjout { get; set; }

        public static explicit operator ModeleClause(ClauseDto ligneDto)
        {
            ModeleClause toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ClauseDto, ModeleClause>().Map(ligneDto);
            if (!string.IsNullOrEmpty(ligneDto.CheminFichier) && File.Exists(ligneDto.CheminFichier))
            {
                var file = new FileInfo(ligneDto.CheminFichier);
                toReturn.FileName = file.Name;
                toReturn.FullFileName = file.FullName;
                toReturn.Extension = file.Extension;
            }
            return toReturn;
        }



    }
}
