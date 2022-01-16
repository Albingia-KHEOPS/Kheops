using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess
{
    public class BrancheRepository : RepositoryBase
    {

        internal static readonly string SelectProduit = "SELECT PBBRA , PBSBR , PBCAT FROM YPOBASE WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type ;";
        internal static readonly string SelectCibleBracheAvn = @"SELECT PBBRA , KAACIBLE , PBTTR , PBAVN 
FROM YPOBASE 
INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP 
WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type ;";
        internal static readonly string UpdateSousBranche = "UPDATE YPOBASE SET PBSBR = :sbranche WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type ;";

        public BrancheRepository(IDbConnection connection) : base(connection) { }

        #region Static Behavior
        public static BrancheDto Initialiser(DataRow ligne)
        {
            BrancheDto branche = new BrancheDto();
            //test si le tcod tplib correspond à une branche (GENER, BRCHE)
            if ((ligne.Table.Columns.Contains("GENER") && ligne["GENER"].ToString().Trim() == "GENER")
                && (ligne.Table.Columns.Contains("TFAM") && ligne["TFAM"].ToString().Trim() == "BRCHE"))
            {
                //branche = new Branche();
                if (ligne.Table.Columns.Contains("TCOD"))
                { branche.Code = ligne["TCOD"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("TPLIB"))
                { branche.Nom = ligne["TPLIB"].ToString().Trim(); };
            }

            //si on arrive avec une cle externe
            else if (ContientUnDesChampsEtEstNonNull(ligne, "UTBRA", "PBBRA"))
            {
                //branche = new Branche();
                if (ligne.Table.Columns.Contains("UTBRA"))
                { branche.Code = ligne["UTBRA"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("PBBRA"))
                { branche.Code = ligne["PBBRA"].ToString().Trim(); branche.Nom = ligne["PBBRA"].ToString().Trim(); }
                if (ligne.Table.Columns.Contains("BRALIB"))
                { branche.Nom = ligne["BRALIB"].ToString().Trim(); }

            }
            branche.Cible = InitialiserCible(ligne);
            return branche;
        }

        public static CibleDto InitialiserCible(DataRow ligne)
        {
            CibleDto cible = new CibleDto();

            if (ContientUnDesChampsEtEstNonNull(ligne, "KAHCIBLE", "KAICIBLE", "KACCIBLE", "KAACIBLE"))
            {
                //cible = new Cible();
                if (ligne.Table.Columns.Contains("KAHCIBLE"))
                { cible.Code = ligne["KAHCIBLE"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("KAICIBLE"))
                { cible.Code = ligne["KAICIBLE"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("KACCIBLE"))
                { cible.Code = ligne["KACCIBLE"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("KAACIBLE"))
                { cible.Code = ligne["KAACIBLE"].ToString().Trim(); }
                if (ligne.Table.Columns.Contains("KAHDESC"))
                { cible.Nom = ligne["KAHDESC"].ToString().Trim(); };
            }
            return cible;
        }

        public static List<BrancheDto> ObtenirBranches(string branche, string cible, bool isBO = false)
        {

            List<string> listBranche = new List<string> { "PP", "ZZ" };
            return CommonRepository.GetParametres(branche, cible, "GENER", "BRCHE", tCod: listBranche, notIn: true, isBO: isBO, tPcn2: "1").Select(x => new BrancheDto { Code = x.Code, Nom = x.Libelle }).ToList();
        }

        public static List<CibleDto> ObtenirCibles(string codeBranche, bool isAdmin = false, bool isUserHorse = false)
        {
            if (!string.IsNullOrEmpty(codeBranche))
            {
                EacParameter[] param = new EacParameter[1];
                param[0] = new EacParameter("codebranche", DbType.AnsiStringFixedLength);
                param[0].Value = codeBranche;

                string sql = @"SELECT TRIM(KAHCIBLE) CODE, TRIM(KAHDESC) DESCRIPTION 
                        FROM KCIBLE LEFT JOIN KCIBLEF ON KAHCIBLE = KAICIBLE 
                        WHERE KAIBRA = :codebranche AND KAHAUT <> 'N'";
                //codeBranche);
                if (!isAdmin)
                {
                    sql += " AND KAHCIBLE NOT LIKE 'RECUP%'";
                }
                if (!isUserHorse)
                {
                    sql += " AND KAHCIBLE NOT IN ('RCCEQ','GRCEQSAL','IMACEQ','GRCEQCLI','EQDOM')";
                }
                sql += " ORDER BY KAHCIBLE";
                return DbBase.Settings.ExecuteList<CibleDto>(CommandType.Text, sql, param);
            }
            return new List<CibleDto>();
        }

        private static readonly string[] CiblesHorse = new[] {
            "RCCEQ","GRCEQSAL","IMACEQ","GRCEQCLI","EQDOM"
        };
        public static List<ParametreDto> GetCibles(string codeBranche, bool loadAllIfNull = false, bool isAdmin = false, bool isUserHorse = false)
        {

            List<ParametreDto> toReturn = new List<ParametreDto>();

            string sql = @"SELECT DISTINCT KAHCIBLE CODE, KAHDESC LIBELLE
                            FROM KCIBLE 
                                LEFT JOIN KCIBLEF ON KAHCIBLE = KAICIBLE 
                            WHERE KAHAUT <> 'N' ";

            string where = " AND ";
            if (!string.IsNullOrEmpty(codeBranche))
            {
                sql += $" {where} KAIBRA = :codeBranche";
                where = " AND ";
            }
            sql += " ORDER BY KAHCIBLE";
            var param = MakeParamsDynamic(sql, new { codeBranche }, loose: true);
            if (!string.IsNullOrEmpty(codeBranche) || loadAllIfNull)
            {
                toReturn = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param).Where(x =>
                           (isAdmin || !x.Code.StartsWith("RECUP"))
                       && (isUserHorse || !CiblesHorse.Contains(x.Code))
                ).ToList();
            }

            return toReturn;
        }
        #endregion

        public (string branche, string cible, string acte, int avn) GetBrancheCibleAvn(Folder folder, string user) {
            return Fetch<(string, string, string, int)>(SelectCibleBracheAvn, folder.CodeOffre.ToIPB(), folder.Version, folder.Type).FirstOrDefault();
        }

        public (string b, string sousBranche, string categorie) GetProduit(Folder folder) {
            return Fetch<(string, string, string)>(SelectProduit, folder.CodeOffre.ToIPB(), folder.Version, folder.Type).FirstOrDefault();
        }

        public void ChangeSousBranche(Folder folder, string sousBranche) {
            var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateSousBranche
            };
            options.BuildParameters(sousBranche, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
            options.Exec();
        }
    }
}
