using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.Framework.Common.Constants {
    /// <summary>
    /// Constatnes fonctionnelles
    /// </summary>
    public class AlbConstantesMetiers {
        #region Type DocumentWord
        public const string WDOC_CLAUSE = "Clause";
        public const string WDOC_CP = "CP";
        public const string WDOC_DOC = "DOC";
        public const string WDOC_BLOC = "BLOC";
        #endregion
        #region Génèration dynamiques des IS
        public const string BRANCHE_RS = "RS";
        public const string BRANCHE_IA = "IA";
        //-----Sections----------------
        public const string INFORMATIONS_ENTETE = "Entete";
        public const string INFORMATIONS_GARANTIES = "Garanties";
        public const string INFORMATIONS_RISQUES = "Risques";
        public const string INFORMATIONS_OBJETS = "Objets";
        public const string INFORMATIONS_OPTIONS = "Options";
        public const string INFORMATIONS_RECUP_OBJETS = "Recup-Objets";
        public const string INFORMATIONS_RECUP_GARANTIES = "Recup-Garanties";
        #endregion
        #region Types écrans
        public const string SCREEN_TYPE_OFFRE = "Offre";
        public const string SCREEN_TYPE_CONTRAT = "Contrat";
        public const string SCREEN_TYPE_AVNMD = "Avt Modif";
        public const string SCREEN_TYPE_AVNRS = "Avt Résil";
        public const string SCREEN_TYPE_REGUL = "Avt Régul";
        public const string SCREEN_TYPE_REGULMODIF = "Avt Régul Modif";
        public const string SCREEN_TYPE_ATTES = "Attestations";
        public const string SCREEN_TYPE_REMISEVIGUEUR = "Avt Rem en Vigueur";
        public const string SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF = "Avt Rem en Vigueur Sans Modif";
        public const string SCREEN_TYPE_REGULPB = "Avt PB";
        public const string SCREEN_TYPE_REGULBNS = "BNS";
        public const string SCREEN_TYPE_REGULBURNER = "BURNER";
        #endregion
        #region Selection possible
        public const string SELECTION_POSSIBLE_OUI = "O";
        public const string SELECTION_POSSIBLE_NON = "N";
        #endregion
        #region Modalite d'affichage
        public const string MODALITE_AFFICHAGE_DERNIERE_VERSION = "D";
        public const string MODALITE_AFFICHAGE_VERSION_ACTIVE = "A";
        #endregion
        #region Etapes Ecran
        public const string ETAPE_ECRAN_BACKOFFICE = "BO";
        #endregion
        #region Codification des expressions complexes
        public static readonly string[] EXPRCOMP_CODE_FIRST_LETTER = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static readonly string[] EXPRCOMP_CODE_LAST_LETTER = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        #endregion
        #region Familles,valeurs
        public enum RestrictionsEnum { Y, S }
        #endregion

        #region  LCI et Franchise
        public enum ExpressionComplexe {
            [BusinessCode("LCI")]
            LCI,
            [BusinessCode("FRH")]
            Franchise
        }
        public enum TypeAppel { Risque, Generale }
        #endregion
        #region Types offre/contrat
        public const string TYPE_OFFRE = "O";
        public const string TYPE_CONTRAT = "P";
        public const string TYPE_AVENANT_MODIF = "AVNMD";
        public const string TYPE_AVENANT_RESIL = "AVNRS";
        public const string TYPE_AVENANT_REGUL = "REGUL";
        public const string TYPE_AVENANT_REGULMODIF = "AVNRG";
        public const string TYPE_AVENANT_PB = "PB";
        public const string TYPE_AVENANT_BNS = "BNS";
        public const string TYPE_ATTESTATION = "ATTES";
        public const string TYPE_AVENANT_REMISE_EN_VIGUEUR = "AVNRM";
        #endregion
        #region Acte de gestion
        public const string ACTEGESTION_GESTION = "G";
        public const string ACTEGESTION_VALIDATION = "V";
        #endregion
        #region Type traitement
        public const string TRAITEMENT_OFFRE = "OFFRE"; //Offre
        public const string TRAITEMENT_OFDBL = "OFDBL";
        public const string TRAITEMENT_AFFNV = "AFFNV"; //Affaire nouvelle
        public const string TRAITEMENT_AVNMD = "AVNMD"; //Avenant Modification
        public const string TRAITEMENT_AVNRM = "AVNRM"; //Avenant Remise en vigueur
        public const string TRAITEMENT_BQDBQ = "BQDBQ"; //Blocage-Déblocage
        public const string TRAITEMENT_SELPO = "SELPO"; //Gestion retour
        public const string TRAITEMENT_AVNRG = "AVNRG"; //Avenant Regule + Modification
        public const string TRAITEMENT_REGUL = "REGUL";
        public const string TRAITEMENT_MODIFHORSAVN = "HAVMD"; //Modification Hors avenant
        public const string TRAITEMENT_RESILHORSAVN = "HAVXC"; // Modification Hors avenant :  résiliation à titre conservatoire avec une fin d’effet le xx/xx/xxxx (PBFEJ FEM FEA) 
        public const string TRAITEMENT_RESILHORSAVN_ANNUL = "HAVAX"; // Modification Hors avenant :  Annulation / résiliation à titre conservatoire 

        #endregion
        #region Libellé Reprise
        public const string LIBREPRISE_OFFRE = "Saisie";
        public const string LIBREPRISE_AFFNV = "Reprise Aff Nouvelle validée";
        public const string LIBREPRISE_AVNMD = "Reprise Avn Modif validé";
        public const string LIBREPRISE_REGUL = "Reprise d'une régularisation seule";
        public const string LIBREPRISE_AVNRG = "Reprise Avn Régularisation + Modification validé";
        #endregion

        #region Les Motifs
        public const string MOTIF_RC = "Régularisation Contractuelle";
        public const string MOTIF_RI = "Régularisation Inférieure";
        #endregion

        #region Type Fonctionnalité pour PGMGARANTIE

        public const string FONC_ATTEST = "ATTEST";
        public const string FONC_REGULE = "REGULE";

        #endregion

        #region Types offre/contrat/avenant pour menus  contextuels (Autocomplete)
        public const string TYPE_MENU_OFFRE = "O";
        public const string TYPE_MENU_POLICE = "P";
        public const string TYPE_MENU_CONTRAT = "C";
        #endregion
        #region Critères de recherche principale
        public enum CriterParam {
            Standard = 1,
            ContratOnly,
            CopyOffre
        }
        #endregion
        #region Étapes
        public enum Etapes {
            [AlbEnumInfoValue("")]
            None,
            [AlbEnumInfoValue("ATT")]
            MontantRef,
            [AlbEnumInfoValue("COASSUREUR")]
            CoAssureur,
            [AlbEnumInfoValue("COCOURTIER")]
            CoCourtier,
            [AlbEnumInfoValue("COT")]
            Cotisation,
            [AlbEnumInfoValue("DI")]
            DeclencheurIncond,
            [AlbEnumInfoValue("DOC")]
            Document,
            [AlbEnumInfoValue("ENG")]
            Engagement,
            [AlbEnumInfoValue("FIN")]
            Fin,
            [AlbEnumInfoValue("GAR")]
            Garantie,
            [AlbEnumInfoValue("GEN")]
            InfoGenerale,
            [AlbEnumInfoValue("INV")]
            Inventaire,
            [AlbEnumInfoValue("OBJ")]
            Objet,
            [AlbEnumInfoValue("OPT")]
            Option,
            [AlbEnumInfoValue("RSQ")]
            Risque,
            [AlbEnumInfoValue("SAISIE")]
            Saisie,
            [AlbEnumInfoValue("FOR")]
            Formule,
            [AlbEnumInfoValue("BASE")]
            Base,
            [AlbEnumInfoValue("ATTES")]
            Attestation,
            [AlbEnumInfoValue("REGUL")]
            Regule,
            [AlbEnumInfoValue("AVNRS")]
            Resiliation,
            [AlbEnumInfoValue("AVNRM")]
            RemiseEnVigueur
        }
        #endregion
        #region Traitement Procédures Stockées
        /// <summary>
        /// Variable P_TRAITEMENT dans les appels des procédures stockées
        /// </summary>
        public enum Traitement {
            [BusinessCode("C")]
            CopyOffre,
            [BusinessCode("F")]
            Formule,
            [BusinessCode("I")]
            InsertCondition,
            [BusinessCode("N")]
            CopyInNewOffre,
            [BusinessCode("P")]
            Police,
            [BusinessCode("U")]
            UpdateCondition,
            [BusinessCode("V")]
            VersionOffre
        }
        #endregion Type Inclus/Exclus
        /// <summary>
        /// Liste des actions possibles pour les inventaires de garantie
        /// </summary>
        public enum InvenExclusInclus {
            [AlbEnumInfoValue("A")]
            Accordé,
            [AlbEnumInfoValue("E")]
            Exclus
        }
        public enum TypesCal {
            [AlbEnumInfoValue("M")]
            Montant,
            [AlbEnumInfoValue("X")]
            Multiplier
        }

        public enum TypeDateRecherche {
            [Display(Name = "Saisie ou Accord")]
            Saisie,
            [Display(Name = "Effet")]
            Effet,
            [Display(Name = "Mise à jour")]
            MAJ,
            [Display(Name = "Création")]
            Creation
        }

        public static IEnumerable<(TypeDateRecherche code, string text)> GetTypesDatesRecherche() {
            return Enum.GetValues(typeof(TypeDateRecherche))
                .Cast<TypeDateRecherche>()
                .Select(x => (x, x.DisplayName()));
        }

        public enum TypeAccesRecherche {
            Standard,
            Rapide,
            BlackListed
        }
        #region Codes Situations
        public enum CodesSituation {
            [AlbEnumInfoValue("W")]
            Refus,
            [AlbEnumInfoValue("A")]
            Attente
        }
        #endregion
        #region Droits Blocage termes
        public enum DroitBlocageTerme {
            Niveau1 = 1,
            Niveau2,
            Niveau3
        }
        #endregion
        #region constantes type historique

        public const string TypeHisto = "P";

        #endregion
        #region Constantes Filtres
        public const string ToutesSaufObligatoires = "TSO";
        public const string Obligatoires = "O";
        public const string Toutes = "T";
        public const string Proposes = "P";
        public const string Suggerees = "S";
        public const string Ajoutees = "A";
        public const string Libres = "L";
        #endregion

        public enum Nature {
            [AlbEnumInfoValue("A")]
            Accordée,
            [AlbEnumInfoValue("C")]
            Comprise,
            [AlbEnumInfoValue("E")]
            Exclue
        }
        #region Types quittances

        public enum TypeQuittances {
            Toutes,
            IMPAYES
        }



        public const string QUITTANCE_PART_TOTALE = "T";
        public const string QUITTANCE_PART_ALBINGIA = "A";

        #endregion
        #region paramètre KChemin
        public enum TypologieDoc {
            Clause,
            ClauseResolu,
            CP,
            ClauseLibre
        }
        #endregion



        #region Type Affichage Suivi Documents

        public const string DISPLAY_AFFAIRE = "AFF";
        public const string DISPLAY_GENERAL = "GEN";

        #endregion

        #region Situations Editions
        public enum EditionSituations {
            [AlbEnumInfoValue("X_Non retenu")]
            X,
            //SLA (13.03.2015) : MAJ des situations d'après Flux gestion Doc et suivi doc.docx (DAN)
            //[AlbEnumInfoValue("N_Non généré")]
            //N,
            //[AlbEnumInfoValue("A_Généré éditable")]
            //A,
            [AlbEnumInfoValue("Z_En erreur")]
            Z,
            [AlbEnumInfoValue("E_Traité")]
            E
        }
        #endregion
        #region Color Typo Editions Doc
        public enum ColorTypeEditDoc {
            [AlbEnumInfoValue("blue")]
            O,
            [AlbEnumInfoValue("green")]
            C,
            [AlbEnumInfoValue("pink")]
            D,
            [AlbEnumInfoValue("grey")]
            A,
        }
        #endregion
        #region échéancier
        public enum TypesCalcul {
            [AlbEnumInfoValue("COMPTANT")]
            Comptant,
            [AlbEnumInfoValue("TOTAL")]
            Total
        }

        public enum ModeSaisieEcheancier {
            [AlbEnumInfoValue("M")]
            Montant,
            [AlbEnumInfoValue("P")]
            Pourcent
        }
        #endregion

        #region Retour erreur Attestations/Régularisation

        public enum ErrorAttesRegul {
            [AlbEnumInfoValue("01_Plage de dates invalide")]
            PlageDateInvalide,
            [AlbEnumInfoValue("02_Dernier avenant non validé")]
            DernierAvtNonValide,
            [AlbEnumInfoValue("03_Période > à la prochaine échéance")]
            PeriodeSuppProchEch,
            [AlbEnumInfoValue("04_Changement de nature du contrat dans la période")]
            ChangeNatureContrat,
            [AlbEnumInfoValue("05_Changement de part du contrat dans la période")]
            ChangePartContrat,
            [AlbEnumInfoValue("06_Changement de coassureurs dans la période")]
            ChangeCoassureur
        }

        #endregion

        #region LAB - Type de recherche
        public enum LABTypeRecherche {
            [AlbEnumInfoValue("TOUS_Tous")]
            TOUS,
            [AlbEnumInfoValue("ASS_Assurées")]
            ASS,
            [AlbEnumInfoValue("COMP_Compagnies")]
            COMP,
            [AlbEnumInfoValue("COU_Courtiers")]
            COU,
            [AlbEnumInfoValue("FOUR_Fournisseurs")]
            FOUR,
            [AlbEnumInfoValue("INT_Intervenants")]
            INT,
        }
        #endregion


        #region Clauses
        public const string TYPE_CLAUSE_PROVENANCE_FORMULE = "CreationFormuleGarantie";
        public const string TYPE_CLAUSE_PROVENANCE_CONDITION = "ConditionsGarantie";
        public const string TYPE_CLAUSE_PROVENANCE_QUITTANCE = "Quittance";
        public const string TypeClauseRegul = "REGUL";
        #endregion
        public const string DateVide = "     -     ";
    }
}
