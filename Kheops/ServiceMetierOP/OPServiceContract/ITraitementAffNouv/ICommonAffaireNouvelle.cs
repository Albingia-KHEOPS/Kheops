using System.Collections.Generic;
using System.ServiceModel;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.AssuresAdditionnels;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.GestionDocumentContrat;
using OP.WSAS400.DTO.DocumentGestion;

namespace OPServiceContract.ITraitementAffNouv
{
    [ServiceContract]
    public interface ICommonAffaireNouvelle
    {
        [OperationContract]
        AssuresAdditionnelsDto InitAssuresAdditionnels(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        List<ParametreDto> GetListQualite(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig);

        [OperationContract]
        AssuresRefDto GetAssureRef(string codeOffre, string version, string type, string codeAssu);

        [OperationContract]
        string LoadInfoAssure(string codeAssu);

        [OperationContract]
        AssuresAdditionnelsDto SaveAssureRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeAssure, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, string idDesi, string designation, ModeConsultation modeNavig, string user);

        [OperationContract]
        AssuresAdditionnelsDto DeleteAssureRef(string codeOffre, string version, string type, string codeAvenant, string codeAssu, ModeConsultation modeNavig, string user);

        [OperationContract]
        AssuresAdditionnelsDto SaveAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeQualite1, string codeQualite2, string codeQualite3, string qualiteAutre,
            string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user);

        [OperationContract]
        AssuresAdditionnelsDto DeleteAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user);

        #region Gestion document contrat

        [OperationContract]
        List<LigneDocumentDto> GetListeDocuments(string code, string version, string type, string user);

        [OperationContract]
        List<LignePieceJointeDto> GetListePiecesJointes(string idDocument);

        #endregion

        #region Param chemin document

        [OperationContract]
        List<LigneCheminDocumentDto> GetListeLignesDocumentChemin();

        [OperationContract]
        LigneCheminDocumentDto GetLigneDocumentChemin(string identifiant);

        [OperationContract]
        List<LigneCheminDocumentDto> AddLigneDocumentChemin(string identifiant, string libelle, string type, string chemin, string serveur, string racine, string environnement, string user);

        [OperationContract]
        List<LigneCheminDocumentDto> UpdateLigneDocumentChemin(string identifiant, string libelle, string chemin, string serveur, string racine, string user);

        [OperationContract]
        List<LigneCheminDocumentDto> DeleteLigneDocumentChemin(string identifiant);

        [OperationContract]
        List<ParametreDto> GetListeTypesChemin();

        [OperationContract]
        List<ParametreDto> GetListeTypologiesChemin();

        #endregion
    }
}
