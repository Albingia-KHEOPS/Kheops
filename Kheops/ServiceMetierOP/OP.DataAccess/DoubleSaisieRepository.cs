using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Common;

namespace OP.DataAccess
{
    public class DoubleSaisieRepository
    {
        #region Méthodes Publiques

        public static void SauvegarderDoubleSaisie(CabinetAutreDto cabinet, string user)
        {
            DateTime date = DateTime.Now;

            DbParameter[] param = new DbParameter[19];
            param[0] = new EacParameter("id", CommonRepository.GetAS400Id("KAFID"));
            param[1] = new EacParameter("codeOffre", cabinet.CodeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("version", 0);
            param[2].Value = Convert.ToInt32(cabinet.Version);
            param[3] = new EacParameter("type", cabinet.Type);
            param[4] = new EacParameter("codeCourtier", 0);
            param[4].Value = Convert.ToInt32(cabinet.Code);
            param[5] = new EacParameter("souscripteur", string.IsNullOrEmpty(cabinet.CodeSouscripteur) ? string.Empty : cabinet.CodeSouscripteur);
            param[6] = new EacParameter("saisieDate", 0);
            param[6].Value = AlbConvert.ConvertDateToInt(cabinet.SaisieDate);
            param[7] = new EacParameter("saisieHeure", 0);
            param[7].Value = AlbConvert.ConvertTimeToInt(cabinet.SaisieHeure);
            param[8] = new EacParameter("situation", "A");
            param[9] = new EacParameter("situationDate", 0);
            param[9].Value = AlbConvert.ConvertDateToInt(date);
            param[10] = new EacParameter("situationHeure", 0);
            param[10].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[11] = new EacParameter("situationUser", user);
            param[12] = new EacParameter("creationDate", 0);
            param[12].Value = AlbConvert.ConvertDateToInt(date);
            param[13] = new EacParameter("creationHeure", 0);
            param[13].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[14] = new EacParameter("creationUser", user);
            param[15] = new EacParameter("action", cabinet.Action);
            param[16] = new EacParameter("motif", cabinet.Action == "REM" ? cabinet.Motif : string.Empty);
            param[17] = new EacParameter("interlocuteur", string.IsNullOrEmpty(cabinet.Interlocuteur) ? string.Empty : cabinet.Interlocuteur);
            param[18] = new EacParameter("reference", string.IsNullOrEmpty(cabinet.Reference) ? string.Empty : cabinet.Reference);

            string sql = @"INSERT INTO KPODBLS
                        (KAFID, KAFIPB, KAFALX, KAFTYP, KAFICT, KAFSOU, KAFSAID, KAFSAIH, 
                        KAFSIT, KAFSITD, KAFSITH, KAFSITU, KAFCRD, KAFCRH, KAFCRU, 
                        KAFACT, KAFMOT, KAFIN5, KAFOCT)
                            VALUES
                        (:id, :codeOffre, :version, :type, :codeCourtier, :souscripteur, :saisieDate, :saisieHeure, 
                        :situation, :situationDate, :situationHeure, :situationUser, :creationDate, :creationHeure, :creationUser,
                        :action, :motif, :interlocuteur, :reference)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (cabinet.Action == "REM")
                RemplaceCourtier(cabinet);
        }

        public static string GetNewVersionOffre(string codeOffre, string version, string type, string user)
        {
            var resNewVers = TraitementVarianteOffreRepository.CreationNouvelleVersionOffre(codeOffre, version, type, user, "V");
            if (!resNewVers)
                return string.Empty;
            else
            {
                string sql = string.Format(@"SELECT MAX(PBALX) INT32RETURNCOL FROM YPOBASE WHERE PBIPB = '{0}' AND PBTYP = '{1}'"
                                    , codeOffre.PadLeft(9, ' '), type);
                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                if (result != null && result.Any())
                    return result.FirstOrDefault().Int32ReturnCol.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region Méthodes Privées

        private static void RemplaceCourtier(CabinetAutreDto cabinet)
        {
            DateTime date = DateTime.Now;
            //Appel de la PROCSTOCK pour remplacer le courtier
            DbParameter[] param = new DbParameter[15];
            param[0] = new EacParameter("P_CODEOFFRE", cabinet.CodeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = cabinet.Version;
            param[2] = new EacParameter("P_TYPE", cabinet.Type);
            param[3] = new EacParameter("P_CODECOURTIER", 0);
            param[3].Value = Convert.ToInt32(cabinet.Code);
            param[4] = new EacParameter("P_CODESOUSCRIPTEUR", cabinet.CodeSouscripteur);
            param[5] = new EacParameter("P_REFERENCE", cabinet.Reference);
            param[6] = new EacParameter("P_CODEINTERLOCUTEUR", 0);
            param[6].Value = !string.IsNullOrEmpty(cabinet.Interlocuteur) ? Convert.ToInt32(cabinet.Interlocuteur) : 0;
            param[7] = new EacParameter("P_DAYSAISIE", 0);
            param[7].Value = cabinet.SaisieDate.Value.Day;
            param[8] = new EacParameter("P_MONTHSAISIE", 0);
            param[8].Value = cabinet.SaisieDate.Value.Month;
            param[9] = new EacParameter("P_YEARSAISIE", 0);
            param[9].Value = cabinet.SaisieDate.Value.Year;
            param[10] = new EacParameter("P_HOURSAISIE", 0);
            param[10].Value = AlbConvert.ConvertTimeToIntMinute(cabinet.SaisieHeure);
            param[11] = new EacParameter("P_DAYSAISIE", 0);
            param[11].Value = date.Day;
            param[12] = new EacParameter("P_MONTHSAISIE", 0);
            param[12].Value = date.Month;
            param[13] = new EacParameter("P_YEARSAISIE", 0);
            param[13].Value = date.Year;
            param[14] = new EacParameter("P_SOUSCRIPTEUR", string.IsNullOrEmpty(cabinet.Souscripteur) ? string.Empty : cabinet.Souscripteur);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_REPLACECOURTIER", param);

//            //Suppression de l'ancien enregistrement
//            string sql = string.Format(@"DELETE FROM YPOCOUR
//                        WHERE PFIPB = '{0}' AND PFALX = {1} AND PFTYP = '{2}' AND PFCTI = '{3}'
//                            AND PFICT = (SELECT PBCTA FROM YPOBASE WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}')",
//                        cabinet.CodeOffre.PadLeft(9, ' '), Convert.ToInt32(cabinet.Version), cabinet.Type, 'A');

//            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

//            //Mise à jour du courtier dans YPOBASE
//            sql = string.Format(@"UPDATE YPOBASE
//                        SET PBCTA = {3}, PBICT = {3}, PBSOU = '{4}', PBOCT = '{5}', PBIN5 = {6}
//                        WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'",
//                        cabinet.CodeOffre.PadLeft(9, ' '), Convert.ToInt32(cabinet.Version), cabinet.Type, Convert.ToInt32(cabinet.Code), cabinet.CodeSouscripteur, cabinet.Reference, !string.IsNullOrEmpty(cabinet.Interlocuteur) ? Convert.ToInt32(cabinet.Interlocuteur) : 0);

//            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

//            //Création enregistrement YPOCOUR
//            DbParameter[] param = new DbParameter[19];
//            param[0] = new EacParameter("codeOffre", cabinet.CodeOffre.PadLeft(9, ' '));
//            param[1] = new EacParameter("version", 0);
//            param[1].Value = Convert.ToInt32(cabinet.Version);
//            param[2] = new EacParameter("type", cabinet.Type);
//            param[3] = new EacParameter("typeCourtier", "A");
//            param[4] = new EacParameter("codeCourtier", 0);
//            param[4].Value = Convert.ToInt32(cabinet.Code);
//            param[5] = new EacParameter("anneeSaisie", 0);
//            param[5].Value = cabinet.SaisieDate.Value.Year;
//            param[6] = new EacParameter("moisSaisie", 0);
//            param[6].Value = cabinet.SaisieDate.Value.Month;
//            param[7] = new EacParameter("jourSaisie", 0);
//            param[7].Value = cabinet.SaisieDate.Value.Day;
//            param[8] = new EacParameter("heureSaisie", 0);
//            param[8].Value = AlbConvert.ConvertTimeToIntMinute(cabinet.SaisieHeure);
//            param[9] = new EacParameter("situation", "A");
//            param[10] = new EacParameter("anneeSituation", 0);
//            param[10].Value = date.Year;
//            param[11] = new EacParameter("moisSituation", 0);
//            param[11].Value = date.Month;
//            param[12] = new EacParameter("jourSituation", 0);
//            param[12].Value = date.Day;
//            param[13] = new EacParameter("gestionnaire", string.IsNullOrEmpty(cabinet.Souscripteur) ? string.Empty : cabinet.Souscripteur);
//            param[14] = new EacParameter("souscripteur", string.IsNullOrEmpty(cabinet.Souscripteur) ? string.Empty : cabinet.Souscripteur);
//            param[15] = new EacParameter("partcom", 0);
//            param[15].Value = 100;
//            param[16] = new EacParameter("refpolicecourt", "");
//            param[17] = new EacParameter("tauxcomhorscatnat", 0);
//            param[17].Value = 0;
//            param[18] = new EacParameter("tauxcomcatnat", 0);
//            param[18].Value = 0;

//            sql = @"INSERT INTO YPOCOUR
//                    (PFIPB, PFALX, PFTYP, PFCTI, PFICT, PFSAA, PFSAM, PFSAJ, PFSAH, PFSIT, 
//                    PFSTA, PFSTM, PFSTJ, PFGES, PFSOU, PFCOM, PFOCT, PFXCM, PFXCN)
//                    VALUES
//                    (:codeOffre, :version, :type, :typeCourtier, :codeCourtier, :anneeSaisie, :moisSaisie, :jourSaisie, :heureSaisie, :situation, 
//                    :anneeSituation, :moisSituation, :jourSituation, :gestionnaire, :souscripteur, :partcom, :refpolicecourt, :tauxcomhorscatnat, :tauxcomcatnat)";

//            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion
    }
}
