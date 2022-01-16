using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public  partial class  KpRguWpRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            const string select_GetByAffaire=@"SELECT
KHYTYP, KHYIPB, KHYALX, KHYRSQ, KHYFOR
, KHYKDEID, KHYGARAN, KHYDEBP, KHYFINP, KHYTRG
, KHYNPE, KHYVEN, KHYCAF, KHYCAU, KHYCAE
, KHYDM1, KHYDT1, KHYDM2, KHYDT2, KHYCOE
, KHYCA1, KHYCT1, KHYCU1, KHYCP1, KHYCX1
, KHYCA2, KHYCT2, KHYCU2, KHYCP2, KHYCX2
, KHYAJU, KHYLMR, KHYMBA, KHYTEN, KHYBRG
, KHYBRL, KHYBAS, KHYBAT, KHYBAU, KHYBAM
, KHYXF1, KHYXB1, KHYXM1, KHYXF2, KHYXB2
, KHYXM2, KHYXF3, KHYXB3, KHYXM3, KHYREG
, KHYPEI, KHYCNH, KHYCNT, KHYGRM, KHYKEA
, KHYPBP, KHYKTD, KHYASV, KHYPBT, KHYSIP
, KHYPBS, KHYRIS, KHYPBR, KHYRIA, KHYAVN
 FROM KPRGUWP
WHERE KHYTYP = :typeAffaire
and KHYIPB = :codeAffaire
and KHYALX = :version
";
            #endregion

            public KpRguWpRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
            public IEnumerable<KpRguWp> GetByAffaire(string typeAffaire, string codeAffaire, int version){
                    return connection.EnsureOpened().Query<KpRguWp>(select_GetByAffaire, new {typeAffaire, codeAffaire, version}).ToList();
            }
    }
}
