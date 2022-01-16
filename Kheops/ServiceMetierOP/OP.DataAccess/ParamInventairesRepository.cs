using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.ParametreInventaires;
using System.Data;
using OP.WSAS400.DTO.Common;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.DataAccess
{
    public class ParamInventairesRepository
    {
        #region Méthodes publiques
        #region Chargement & recherche Inventaire
        public static List<ParamInventairesDto> LoadInventaire(string codeInventaire, string descriptionInventaire, bool isAdmin = true)
        {
            bool emptyCodeInventaire = string.IsNullOrEmpty(codeInventaire);
            bool emptyLibelleInventaire = string.IsNullOrEmpty(descriptionInventaire);

            string sql = @"SELECT KAGID GUIDID, 
                                KAGTYINV CODE, 
                                KAGDESC LIBELLE,
                                KAGTMAP KAGTMAP,
                                KAGKGGID CODEFILTRE
                            FROM KINVTYP ";
            string whereAndOr = " WHERE";

            if (!emptyCodeInventaire)
            {
                sql += whereAndOr;
                sql += string.Format(" (TRIM(UPPER(KAGTYINV)) LIKE UPPER('%{0}%'))", codeInventaire.Trim());
                whereAndOr = (emptyLibelleInventaire ? string.Empty : " AND");
            }
            if (!emptyLibelleInventaire)
            {
                sql += whereAndOr;
                sql += string.Format(" (TRIM(UPPER(KAGDESC)) LIKE UPPER('%{0}%'))", descriptionInventaire.Trim());
            }
            sql += " ORDER BY KAGTYINV";

            return DbBase.Settings.ExecuteList<ParamInventairesDto>(CommandType.Text, sql);
        }
        #endregion
        #region Chargement detail Inventaire
        public static List<ParametreDto> GetFiltres()
        {
            List<ParametreDto> listFiltres = new List<ParametreDto>();

            var filtresResult = ParamConceptFamilleCodeRepository.LoadListFiltres(string.Empty, string.Empty, string.Empty);

            listFiltres = filtresResult.Select(
                     f => new ParametreDto
                     {
                         LongId = f.Id,
                         Code = f.Code,
                         Libelle = f.Libelle
                     }).ToList();

            return listFiltres;
        }
        #endregion
        #region Update
        public static ParamInventairesListDto ModifierInventaire(string guidIdInventaire, string codeInventaire, string descriptionInventaire, int kagtmap, string codeFiltre)
        {
            ParamInventairesListDto toReturn = new ParamInventairesListDto();


            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("P_IdInventaire", DbType.Int32);
            param[0].Value = guidIdInventaire;
            param[1] = new EacParameter("P_CodeInventaire", DbType.AnsiStringFixedLength);
            param[1].Value = codeInventaire;
            param[2] = new EacParameter("P_Description", DbType.AnsiStringFixedLength);
            param[2].Value = descriptionInventaire;
            param[3] = new EacParameter("p_kagtmap", DbType.Int32);
            param[3].Value = kagtmap;
            param[4] = new EacParameter("p_codefiltre", DbType.Int32);
            param[4].Value = codeFiltre;
            param[5] = new EacParameter("p_return", DbType.Int32);
            param[5].Direction = ParameterDirection.InputOutput;
            param[5].DbType = DbType.Int32;
            param[5].Value = 0;

            var result = DbBase.Settings.ExecuteList<ParamInventairesDto>(CommandType.StoredProcedure, "SP_MODIFINVENTAIRE", param);
            if (result != null && result.Any())
            {
                toReturn.ReturnValue = Convert.ToInt32(param[5].Value);
                toReturn.Inventaires = result;
            }

            return toReturn;
        }
        #endregion
        #region Add
        public static ParamInventairesListDto AjouterInventaire(string codeInventaire, string descriptionInventaire, int kagtmap, string codeFiltre)
        {
            ParamInventairesListDto toReturn = new ParamInventairesListDto();

            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("P_IDINVENTAIRE", DbType.Int32);
            param[0].Value = 0;           
            param[1] = new EacParameter("P_CODEINVENTAIRE", DbType.AnsiStringFixedLength);
            param[1].Value = codeInventaire;
            param[2] = new EacParameter("P_DESCRIPTION", DbType.AnsiStringFixedLength);
            param[2].Value = descriptionInventaire;
            param[3] = new EacParameter("P_KAGTMAP", DbType.Int32);
            param[3].Value = kagtmap;
            param[4] = new EacParameter("P_CODEFILTRE", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codeFiltre) ? Convert.ToInt32(codeFiltre) : 0;
            param[5] = new EacParameter("P_RETURN", DbType.Int32);
            param[5].Direction = ParameterDirection.InputOutput;
            param[5].DbType = DbType.Int32;
            param[5].Value = 0;

            var result = DbBase.Settings.ExecuteList<ParamInventairesDto>(CommandType.StoredProcedure, "SP_MODIFINVENTAIRE", param);
            if (result != null && result.Any())
            {
                toReturn.ReturnValue = Convert.ToInt32(param[5].Value);
                toReturn.Inventaires = result;
            }

            return toReturn;
        }
        #endregion
        #region Verif & Delete

        public static ParamInventairesListDto SupprimerInventaire(int guidIdInventaire)
        {
            ParamInventairesListDto toReturn = new ParamInventairesListDto();

            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("P_IdInventaire", DbType.Int32);
            param[0].Value = guidIdInventaire;
            param[1] = new EacParameter("p_return", DbType.Int32);
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].DbType = DbType.Int32;
            param[1].Value = 0;
            var result = DbBase.Settings.ExecuteList<ParamInventairesDto>(CommandType.StoredProcedure, "SP_SUPPRINVENTAIRE", param);
            if (result != null && result.Any())
            {
                toReturn.ReturnValue = Convert.ToInt32(param[1].Value);
                toReturn.Inventaires = result;
            }

            return toReturn;
        }
        #endregion
        #endregion
    }
}
