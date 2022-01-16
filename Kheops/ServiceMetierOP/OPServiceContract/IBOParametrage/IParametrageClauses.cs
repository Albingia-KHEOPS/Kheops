using System.Collections.Generic;
using System.ServiceModel;
using OP.WSAS400.DTO.ParametreClauses;

namespace OPServiceContract.IBOParametrage
{
    [ServiceContract]
    public interface IParametrageClauses
    {
        [OperationContract]
        ParamClausesDto InitParamClauses();

        [OperationContract]
        List<ParamListParamDto> LoadListActeGestion(string codeService);

        [OperationContract]
        ParamActeGestionEtapeDto LoadListEtapes(string codeService, string codeActGes);

        [OperationContract]
        ParamEtapeContexteDto LoadListContextes(string codeService, string codeActGes, string codeEtape);

        [OperationContract]
        ParamContexteEGDIDto LoadListEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte);

        [OperationContract]
        ParamRattachClauseDto OpenRattachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI);

        [OperationContract]
        ParamAjoutActeGestionDto LoadAjoutActeGestion(string codeService);

        [OperationContract]
        void AddActeGestion(string codeService, string codeActeGestion);

        [OperationContract]
        ParamAjoutEtapeDto LoadAjoutEtape(string codeService, string codeActGes);

        [OperationContract]
        void AddEtape(string codeService, string codeActeGestion, string codeEtape, int numOrdre);

        [OperationContract]
        ParamAjoutContexteDto LoadAjoutContexte(string codeService, string codeActGes, string codeEtape);

        [OperationContract]
        void AddContexte(string idContexte, string codeService, string codeActeGestion, string codeEtape, string codeContexte, bool emplacementModif, bool ajoutClausier, bool ajoutLibre, string scriptControle,
            string rubrique, string sousRubrique, string sequence, string emplacement, string sousEmplacement, string numOrdonnancement, string specificite);

        [OperationContract]
        ParamAjoutEGDIDto LoadAjoutEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI);

        [OperationContract]
        void AddEGDI(string codeService, string codeActeGestion, string codeEtape, string codeContext, string type, string codeEGDI, int numOrdre, string libelleEGDI, string commentaire, string code);

        [OperationContract]
        void DeleteParam(string etape, string codeParam);

        [OperationContract]
        ParamRattachClauseDto ReloadClauses(string codeEGDI, string restrict);

        [OperationContract]
        ParamRattachSaisieDto AttachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI, string type, string codeClause);

        [OperationContract]
        int SaveAttachClause(int codeRattach, string codeClause, string codeEGDI, int numOrdre, string nom1, string nom2, string nom3, string nature, string impressAnnexe, string codeAnnexe, string attribut, string version, string user);

        [OperationContract]
        void DeleteAttachClause(string codeEGDI, string codeClause);

        [OperationContract]
        ParamAjoutContexteDto LoadContexte(string codeService, string codeActGes, string codeEtape, string codeContexte);

        [OperationContract]
        void CopyParamClause(string src, string dest);
    }
}
