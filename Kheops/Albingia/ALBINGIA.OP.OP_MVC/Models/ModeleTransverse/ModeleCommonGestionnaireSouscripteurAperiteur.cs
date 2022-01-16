using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Personnes;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{
    public class ModeleCommonGestionnaireSouscripteurAperiteur
    {
        public string Code { get; set; }
        public Int64 CodeNum { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Delegation { get; set; }
        public string Active { get; set; }

        public ModeleCommonGestionnaireSouscripteurAperiteur()
        {
            Code = string.Empty;
            Nom = string.Empty;
            Prenom = string.Empty;
            Delegation = string.Empty;
            Active = string.Empty;
        }

        public static explicit operator ModeleCommonGestionnaireSouscripteurAperiteur(GestionnaireDto gestionnaireDto)
        {
            var toReturn = new ModeleCommonGestionnaireSouscripteurAperiteur() {
                Code = gestionnaireDto.Id,
                Nom = gestionnaireDto.Nom,
                Prenom = gestionnaireDto.Prenom,
                Delegation = gestionnaireDto.NomDelegation,
                Active = gestionnaireDto.IsGestionnaire
            };           
            return toReturn;
        }

        public static explicit operator ModeleCommonGestionnaireSouscripteurAperiteur(SouscripteurDto souscripteurDto)
        {
            var toReturn = new ModeleCommonGestionnaireSouscripteurAperiteur() {
                Code = souscripteurDto.Code,
                Nom = souscripteurDto.Nom,
                Prenom = souscripteurDto.Prenom,
                Delegation = souscripteurDto.NomDelegation,
                Active = souscripteurDto.IsSouscripteur
            };           
            return toReturn;
        }

        public static explicit operator ModeleCommonGestionnaireSouscripteurAperiteur(AperiteurDto aperiteurDto)
        {
            var toReturn = new ModeleCommonGestionnaireSouscripteurAperiteur() {
                Code = aperiteurDto.Code,
                CodeNum = aperiteurDto.CodeNum,
                Nom = aperiteurDto.Nom
            };           
            return toReturn;
        }

        public static explicit operator ModeleCommonGestionnaireSouscripteurAperiteur(UtilisateurDto utilisateurDto)
        {
            var toReturn = new ModeleCommonGestionnaireSouscripteurAperiteur()
            {
                Code = utilisateurDto.Code,
                Nom = utilisateurDto.Nom, 
                Prenom = utilisateurDto.Prenom
            };
            return toReturn;
        }
        

    }
}