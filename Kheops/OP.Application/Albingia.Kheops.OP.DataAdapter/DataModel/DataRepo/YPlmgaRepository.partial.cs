using ALBINGIA.Framework.Common.Data;
using Dapper;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using OP.WSAS400.DTO.GarantieModele;
using System;
using System.Collections.Generic;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class YPlmgaRepository
    {
        internal List<GarantieModelePlatDto> GetGarantieModele(string code)
        {
            string sql = string.Format(@"SELECT 0 GUID, D1MGA CODE, D1LIB DESCRIPTION, '' BRANCHE
                                                        , '' CIBLE, '' VOLET, '' BLOC, '' TYPOLOGIE 
                                                         FROM  YPLMGA G1
                                                        WHERE
                                                        NOT EXISTS
                                                        (
                                                        SELECT * FROM  YPLMGA D2
                                                        INNER JOIN KCATMODELE ON KARMODELE=D2.D1MGA 
                                                        INNER JOIN KCATBLOC ON KARKAQID = KAQID 
                                                        WHERE G1.D1MGA=D2.D1MGA
                                                        )
                                                        AND G1.D1MGA = '{0}'
                                                        UNION
                                                        SELECT KARID GUID, KARMODELE CODE, D1LIB DESCRIPTION, KAQBRA BRANCHE
                                                        , KAQCIBLE CIBLE, KAQVOLET VOLET, KAQBLOC BLOC, KARTYPO TYPOLOGIE 
                                                                                        FROM YPLMGA 
                                                        INNER JOIN KCATMODELE ON KARMODELE=D1MGA 
                                                        INNER JOIN KCATBLOC ON KARKAQID = KAQID
                                                        WHERE D1MGA = '{0}'", code);

            var c = this.connection.EnsureOpened();
            return c.Query<GarantieModelePlatDto>(sql).ToList();
            
        }

        internal void CopierGarantieModele(string code, string codeCopie)
        {
            using (var dbOptions = new DbSPOptions(this.connection == null)
            {
                SqlText = "SP_COPYMODELEGAR",
                DbConnection = this.connection
            })
            {
                dbOptions.Parameters = new[] {
                    new EacParameter("P_CODE", code),
                    new EacParameter("P_CODECOPIE", codeCopie),
                };
                dbOptions.ExecStoredProcedure();
            }
        }

        internal bool ExistDansContrat(string code)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPLMGA 
                                INNER JOIN KCATMODELE ON KARMODELE = D1MGA
                                INNER JOIN KPOPTD ON KARID = KDCKARID
                                INNER JOIN YPOBASE ON KDCIPB = PBIPB AND 
                                    CASE WHEN PBIPB LIKE 'CV%' THEN 
                                		CASE WHEN PBORK != 'KVS' THEN 1 ELSE 0
                                		END
                                	ELSE 1 
                                	END = 1
                                WHERE D1MGA = '{0}'", code);
            var c = this.connection.EnsureOpened();
            bool toReturn = c.QuerySingle<int>(sql) > 0;
            return toReturn;
        }
    }
}
