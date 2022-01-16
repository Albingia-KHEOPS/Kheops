using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.AssuresAdditionnels;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.ITraitementAffNouv;
using OP.WSAS400.DTO.GestionDocumentContrat;
using OP.WSAS400.DTO.DocumentGestion;
using OP.DataAccess;
using OP.WSAS400.DTO.Offres.Branches;

namespace OP.Services.TraitementAffNouv
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CommonAffaireNouvelle : ICommonAffaireNouvelle
    {
        #region Méthodes Publiques

        public AssuresAdditionnelsDto InitAssuresAdditionnels(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return CommonAffNouvRepository.InitAssuresAdditionnels(codeOffre, version, type, codeAvn, modeNavig);
        }

        public List<ParametreDto> GetListQualite(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
            return CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "OJQLT");
        }

        public AssuresRefDto GetAssureRef(string codeOffre, string version, string type, string codeAssu)
        {
            return CommonAffNouvRepository.GetAssureRef(codeOffre, version, type, codeAssu);
        }

        public string LoadInfoAssure(string codeAssu)
        {
            return CommonAffNouvRepository.LoadInfoAssure(codeAssu);
        }

        public AssuresAdditionnelsDto SaveAssureRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeAssure, string codeQualite1, string codeQualite2,
            string codeQualite3, string qualiteAutre, string idDesi, string designation, ModeConsultation modeNavig, string user)
        {
            return CommonAffNouvRepository.SaveAssureRef(codeOffre, version, type, codeAvenant, modeEdit, codeAssure, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, idDesi, designation, modeNavig, user);
        }

        public AssuresAdditionnelsDto DeleteAssureRef(string codeOffre, string version, string type, string codeAvenant, string codeAssu, ModeConsultation modeNavig, string user)
        {
            return CommonAffNouvRepository.DeleteAssureRef(codeOffre, version, type, codeAvenant, codeAssu, modeNavig, user);
        }

        public AssuresAdditionnelsDto SaveAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string modeEdit, string codeQualite1, string codeQualite2, string codeQualite3, string qualiteAutre,
            string codeOldQualite1, string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user)
        {
            return CommonAffNouvRepository.SaveAssureNonRef(codeOffre, version, type, codeAvenant, modeEdit, codeQualite1, codeQualite2, codeQualite3, qualiteAutre, codeOldQualite1, codeOldQualite2, codeOldQualite3, oldQualiteAutre, modeNavig, user);
        }

        public AssuresAdditionnelsDto DeleteAssureNonRef(string codeOffre, string version, string type, string codeAvenant, string codeOldQualite1,
            string codeOldQualite2, string codeOldQualite3, string oldQualiteAutre, ModeConsultation modeNavig, string user)
        {
            return CommonAffNouvRepository.DeleteAssureNonRef(codeOffre, version, type, codeAvenant, codeOldQualite1, codeOldQualite2, codeOldQualite3, oldQualiteAutre, modeNavig, user);
        }

        #region Gestion Documents contrat

        public List<LigneDocumentDto> GetListeDocuments(string code, string version, string type, string user)
        {
            return GestionDocumentRepository.GetListeDocuments(code, version, type, user);
        }

        public List<LignePieceJointeDto> GetListePiecesJointes(string idDocument)
        {
            return GestionDocumentRepository.GetListePiecesJointes(idDocument);
        }

        #endregion

        #region Param chemin document

        public List<LigneCheminDocumentDto> GetListeLignesDocumentChemin()
        {
            return GestionDocumentRepository.GetListeLignesDocumentChemin();
        }

        public LigneCheminDocumentDto GetLigneDocumentChemin(string identifiant)
        {
            return GestionDocumentRepository.GetLigneDocumentChemin(identifiant);
        }

        public List<LigneCheminDocumentDto> AddLigneDocumentChemin(string identifiant, string libelle, string type, string chemin, string serveur, string racine, string environnement, string user)
        {
            return GestionDocumentRepository.AddLigneDocumentChemin(identifiant, libelle, type, chemin, serveur, racine, environnement, user);
        }

        public List<LigneCheminDocumentDto> UpdateLigneDocumentChemin(string identifiant, string libelle, string chemin, string serveur, string racine, string user)
        {
            return GestionDocumentRepository.UpdateLigneDocumentChemin(identifiant, libelle, chemin, serveur, racine, user);
        }

        public List<LigneCheminDocumentDto> DeleteLigneDocumentChemin(string identifiant)
        {
            return GestionDocumentRepository.DeleteLigneDocumentChemin(identifiant);
        }

        public List<ParametreDto> GetListeTypesChemin()
        {
            return GestionDocumentRepository.GetListeTypesChemin();
        }

        public List<ParametreDto> GetListeTypologiesChemin()
        {
            return GestionDocumentRepository.GetListeTypologiesChemin();
        }



        #endregion

        #endregion

        #region Méthodes Privées

        #endregion
    }
}
