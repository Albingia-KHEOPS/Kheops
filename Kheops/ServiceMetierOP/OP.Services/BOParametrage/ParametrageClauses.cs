using System.Collections.Generic;
using OPServiceContract.IBOParametrage;
using System.ServiceModel.Activation;
using OP.WSAS400.DTO.ParametreClauses;
using OP.Services.BLServices;
using System.Transactions;
using OP.DataAccess;

namespace OP.Services.BOParametrage
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ParametrageClauses : IParametrageClauses
    {


        #region Méthodes Publiques

        public ParamClausesDto InitParamClauses()
        {
            ParamClausesDto toReturn = new ParamClausesDto
            {
                Etape = 1,
                Service = "",
                Services = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "SERVI")
            };
            return toReturn;
            
            //return BLParamClauses.InitParamClauses();
        }

        public List<ParamListParamDto> LoadListActeGestion(string codeService)
        {
            return ParamClausesRepository.LoadListActeGestion(codeService);

            //return BLParamClauses.LoadListActeGestion(codeService);
        }

        public ParamActeGestionEtapeDto LoadListEtapes(string codeService, string codeActGes)
        {
            return ParamClausesRepository.LoadListEtapes(codeService, codeActGes);

            //return BLParamClauses.LoadListEtapes(codeService, codeActGes);
        }

        public ParamEtapeContexteDto LoadListContextes(string codeService, string codeActGes, string codeEtape)
        {
            return ParamClausesRepository.LoadListContextes(codeService, codeActGes, codeEtape);

            //return BLParamClauses.LoadListContextes(codeService, codeActGes, codeEtape);
        }

        public ParamContexteEGDIDto LoadListEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            return ParamClausesRepository.LoadListEGDI(codeService, codeActGes, codeEtape, codeContexte);

            //return BLParamClauses.LoadListEGDI(codeService, codeActGes, codeEtape, codeContexte);
        }

        public ParamRattachClauseDto OpenRattachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            return ParamClausesRepository.LoadRattachClause(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);

            //return BLParamClauses.LoadRattachClause(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
        }

        public ParamAjoutActeGestionDto LoadAjoutActeGestion(string codeService)
        {
            return ParamClausesRepository.LoadAjoutActeGestion(codeService);

            //return BLParamClauses.LoadAjoutActeGestion(codeService);
        }

        public void AddActeGestion(string codeService, string codeActeGestion)
        {
            ParamClausesRepository.AddActeGestion(codeService, codeActeGestion);

            //BLParamClauses.AddActeGestion(codeService, codeActeGestion);
        }

        public ParamAjoutEtapeDto LoadAjoutEtape(string codeService, string codeActGes)
        {
            return ParamClausesRepository.LoadAjoutEtape(codeService, codeActGes);

            //return BLParamClauses.LoadAjoutEtape(codeService, codeActGes);
        }

        /// <summary>
        /// Ajoute une étape à un acte de gestion
        /// </summary>
        /// <param name="codeService">Code service.</param>
        /// <param name="codeActeGestion">Code acte gestion.</param>
        /// <param name="codeEtape">Code etape.</param>
        /// <param name="numOrdre">Numéro d'ordre</param>
        public void AddEtape(string codeService, string codeActeGestion, string codeEtape, int numOrdre)
        {
            ParamClausesRepository.AddEtape(codeService, codeActeGestion, codeEtape, numOrdre);

            //BLParamClauses.AddEtape(codeService, codeActeGestion, codeEtape, numOrdre);
        }

        /// <summary>
        /// Charge les information pour ajouter
        /// un contexte à une étape.
        /// </summary>
        /// <param name="codeService">Code service.</param>
        /// <param name="codeActGes">Code acte de gestion</param>
        /// <param name="codeEtape">Code etape.</param>
        /// <returns></returns>
        public ParamAjoutContexteDto LoadAjoutContexte(string codeService, string codeActGes, string codeEtape)
        {
            return ParamClausesRepository.LoadAjoutContexte(codeService, codeActGes, codeEtape);

            //return BLParamClauses.LoadAjoutContexte(codeService, codeActGes, codeEtape);
        }

        /// <summary>
        /// Ajoute un contexte à une étape
        /// </summary>
        /// <param name="codeService">Code service</param>
        /// <param name="codeActeGestion">Code acte gestion</param>
        /// <param name="codeEtape">Code etape.</param>
        /// <param name="codeContexte">Code contexte.</param>
        public void AddContexte(string idContexte, string codeService, string codeActeGestion, string codeEtape, string codeContexte, bool emplacementModif, bool ajoutClausier, bool ajoutLibre, string scriptControle,
            string rubrique, string sousRubrique, string sequence, string emplacement, string sousEmplacement, string numOrdonnancement, string specificite)
        {
            ParamClausesRepository.AddContexte(idContexte, codeService, codeActeGestion, codeEtape, codeContexte, emplacementModif, ajoutClausier, ajoutLibre, scriptControle, rubrique, sousRubrique, sequence, emplacement, sousEmplacement, numOrdonnancement, specificite);

            //BLParamClauses.AddContexte(idContexte, codeService, codeActeGestion, codeEtape, codeContexte, emplacementModif, ajoutClausier, ajoutLibre, scriptControle, rubrique, sousRubrique, sequence, emplacement, sousEmplacement, numOrdonnancement, specificite);
        }

        public ParamAjoutEGDIDto LoadAjoutEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            return ParamClausesRepository.LoadAjoutEGDI(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);

            //return BLParamClauses.LoadAjoutEGDI(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
        }

        public void AddEGDI(string codeService, string codeActeGestion, string codeEtape, string codeContext, string type, string codeEGDI, int numOrdre, string libelleEGDI, string commentaire, string code)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                ParamClausesRepository.AddEGDI(codeService, codeActeGestion, codeEtape, codeContext, type, codeEGDI, numOrdre, libelleEGDI, commentaire, code);

                //BLParamClauses.AddEGDI(codeService, codeActeGestion, codeEtape, codeContext, type, codeEGDI, numOrdre, libelleEGDI, commentaire, code);
            }
        }

        /// <summary>
        /// Supprime un paramètre en fonction
        /// de l'étape de paramétrage et du code du paramètre.
        /// </summary>
        /// <param name="etape">Etape</param>
        /// <param name="codeParam">Code Param</param>
        public void DeleteParam(string etape, string codeParam)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                ParamClausesRepository.DeleteParam(etape, codeParam);

                //BLParamClauses.DeleteParam(etape, codeParam);
            }
        }

        public ParamRattachClauseDto ReloadClauses(string codeEGDI, string restrict)
        {
            return ParamClausesRepository.ReloadClauses(codeEGDI, restrict);

            //return BLParamClauses.ReloadClauses(codeEGDI, restrict);
        }

        public ParamRattachSaisieDto AttachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI, string type, string codeClause)
        {
            return ParamClausesRepository.AttachClause(codeService, codeActGes, codeEtape, codeContexte, codeEGDI, type, codeClause);

            //return BLParamClauses.AttachClause(codeService, codeActGes, codeEtape, codeContexte, codeEGDI, type, codeClause);
        }

        public int SaveAttachClause(int codeRattach, string codeClause, string codeEGDI, int numOrdre, string nom1, string nom2, string nom3, string nature, string impressAnnexe, string codeAnnexe, string attribut, string version, string user)
        {
            return ParamClausesRepository.SaveAttachClause(codeRattach, codeClause, codeEGDI, numOrdre, nom1, nom2, nom3, nature, impressAnnexe, codeAnnexe, attribut, version, user);

            //return BLParamClauses.SaveAttachClause(codeRattach, codeClause, codeEGDI, numOrdre, nom1, nom2, nom3, nature, impressAnnexe, codeAnnexe, attribut, version, user);
        }

        public void DeleteAttachClause(string codeEGDI, string codeClause)
        {
            ParamClausesRepository.DeleteAttachClause(codeEGDI, codeClause);

            //BLParamClauses.DeleteAttachClause(codeEGDI, codeClause);
        }

        public ParamAjoutContexteDto LoadContexte(string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            return ParamClausesRepository.LoadContexte(codeService, codeActGes, codeEtape, codeContexte);

            //return BLParamClauses.LoadContexte(codeService, codeActGes, codeEtape, codeContexte);
        }

        public void CopyParamClause(string src, string dest)
        {
            ParamClausesRepository.CopyParamClause(src, dest);
        }

        #endregion
    }
}
