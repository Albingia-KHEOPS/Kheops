using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Constants;
using OP.DataAccess;
using OP.WSAS400.DTO.AssuresAdditionnels;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.GestionDocumentContrat;
using OP.WSAS400.DTO.DocumentGestion;

namespace OP.Services.TraitementAffNouv.BLAffNouv
{
    public class InitCommonAffNouv
    {
        #region Méthodes Publiques

        //public static AssuresAdditionnelsDto InitAssuresAdditionnels(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        //{
        //    return CommonAffNouvRepository.InitAssuresAdditionnels(codeOffre, version, type, codeAvn, modeNavig);
        //}

        /// <summary>
        /// Récupère la liste des qualités pour 
        /// les assurés additionnels
        /// </summary>
        /// <returns></returns>
        //public static List<ParametreDto> GetListQualite(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        //{
        //    BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
        //    return CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "OJQLT");
        //}

        //public static AssuresRefDto GetAssureRef(string codeOffre, string version, string type, string codeAssu)
        //{
        //    return CommonAffNouvRepository.GetAssureRef(codeOffre, version, type, codeAssu);
        //}

        //public static string LoadInfoAssure(string codeAssu)
        //{
        //    return CommonAffNouvRepository.LoadInfoAssure(codeAssu);
        //}

        //public static AssuresAdditionnelsDto SaveAssureRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeAssure, string codeQualite1, string codeQualite2,
        //    string codeQualite3, string qualiteAutre, ModeConsultation modeNavig, string user)
        //{
        //    return CommonAffNouvRepository.SaveAssureRef(codeOffre, version, type, codeAvenant, modeEdit, codeAssure, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, modeNavig, user);
        //}

        //public static AssuresAdditionnelsDto DeleteAssureRef(string codeOffre, string version, string type, string codeAvenant, string codeAssu, ModeConsultation modeNavig,string user)
        //{
        //    return CommonAffNouvRepository.DeleteAssureRef(codeOffre, version, type, codeAvenant, codeAssu, modeNavig, user);
        //}

        //public static AssuresAdditionnelsDto SaveAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeQualite1, string codeQualite2, string codeQualite3, string qualiteAutre,
        //    string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user)
        //{
        //    return CommonAffNouvRepository.SaveAssureNonRef(codeOffre, version, type, codeAvenant, modeEdit, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, codeOldQualite1, codeOldQualite2, codeOldQualite3, oldQualiteAutre, modeNavig, user);
        //}

        //public static AssuresAdditionnelsDto DeleteAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string codeOldQualite1,
        //    string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user)
        //{
        //    return CommonAffNouvRepository.DeleteAssureNonRef(codeOffre, version, type, codeAvenant, codeOldQualite1, codeOldQualite2, codeOldQualite3, oldQualiteAutre, modeNavig, user);
        //}

        #region Gestion Documents contrat

        //public static List<LigneDocumentDto> GetListeDocuments(string code, string version, string type, string user)
        //{
        //    return GestionDocumentRepository.GetListeDocuments(code, version, type, user);
        //}

        //public static List<LignePieceJointeDto> GetListePiecesJointes(string idDocument)
        //{
        //    return GestionDocumentRepository.GetListePiecesJointes(idDocument);
        //}

        #endregion

        #region Param chemin document

        //public static List<LigneCheminDocumentDto> GetListeLignesDocumentChemin()
        //{
        //    return GestionDocumentRepository.GetListeLignesDocumentChemin();
        //}

        //public static LigneCheminDocumentDto GetLigneDocumentChemin(string identifiant)
        //{
        //    return GestionDocumentRepository.GetLigneDocumentChemin(identifiant);
        //}

        //public static List<LigneCheminDocumentDto> AddLigneDocumentChemin(string identifiant, string libelle, string type, string chemin, string serveur, string racine, string environnement, string user)
        //{
        //    return GestionDocumentRepository.AddLigneDocumentChemin(identifiant, libelle, type, chemin, serveur, racine, environnement, user);
        //}

        //public static List<LigneCheminDocumentDto> UpdateLigneDocumentChemin(string identifiant, string libelle, string chemin, string serveur, string racine, string user)
        //{
        //    return GestionDocumentRepository.UpdateLigneDocumentChemin(identifiant, libelle, chemin, serveur, racine, user);
        //}

        //public static List<LigneCheminDocumentDto> DeleteLigneDocumentChemin(string identifiant)
        //{
        //    return GestionDocumentRepository.DeleteLigneDocumentChemin(identifiant);
        //}

        //public static List<ParametreDto> GetListeTypesChemin()
        //{
        //    return GestionDocumentRepository.GetListeTypesChemin();
        //}

        //public static List<ParametreDto> GetListeTypologiesChemin()
        //{
        //    return GestionDocumentRepository.GetListeTypologiesChemin();
        //}

        #endregion

        #endregion

        #region Méthodes Privées

        #endregion
    }
}
