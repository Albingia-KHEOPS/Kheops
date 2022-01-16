using System.ServiceModel;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Common;

namespace OPServiceContract.ISaisieCreationOffre
{
    [ServiceContract]
    public interface IRechercheSaisie
    {
        [OperationContract]
        RechercheSaisieGetResultDto RechercheSaisieGet(RechercheSaisieGetQueryDto query);             

        [OperationContract]
        string DernierNumeroVersionOffreMotifSituation(string codeOffre, string type, string version);
        [OperationContract]
        bool CreationNouvelleVersionOffre(string codeOffre, string version, string type, string utilisateur, string traitement);

        [OperationContract]
        string CheckOffreCreate(string codeOffreCopy, string versionCopy, string type);

        [OperationContract]
        string CheckOffreCopy(string codeOffre, string version, string codeOffreCopy, string versionCopy, string type);

        [OperationContract]
        string GetOffreMere(string param);

        [OperationContract]
        RechercheOffresGetResultDto RechercherOffresContrat(ModeleParametresRechercheDto paramRecherche, ModeConsultation modeNavig);

        [OperationContract]
        string GetTypeAvenant(string paramOffre);

        [OperationContract]
        void ConfirmReprise(string codeOffre, string version, string type, string codeAvt, string typeAvt, string user);

        [OperationContract]
        bool GetHasPrimeSoldee(string codeAffaire, string version, string type, string codeAvn);

        [OperationContract]
        string GetEtatOffre(string codeOffre, string version, string type);

        [OperationContract]
        string CheckPrimeAvt(string codeContrat, string version, string type, string codeAvn);

        [OperationContract]
        long GetNumRegule(string codeAffaire, string version, string type, string codeAvn);

        [OperationContract]
        BlocageSaisieDto GetBlocageSaisieAS400();
    }
}
