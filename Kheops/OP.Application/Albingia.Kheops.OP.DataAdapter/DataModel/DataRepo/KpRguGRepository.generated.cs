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

    public  partial class  KpRguGRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKSPP
            const string select=@"SELECT
KHXID, KHXKHWID, KHXTYP, KHXIPB, KHXALX
, KHXRSQ, KHXFOR, KHXKDEID, KHXGARAN, KHXDEBP
, KHXFINP, KHXSIT, KHXTRG, KHXNPE, KHXVEN
, KHXCAF, KHXCAU, KHXCAE, KHXMHC, KHXFRC
, KHXFR0, KHXMHT, KHXMTX, KHXTTT, KHXMTT
, KHXCNH, KHXCNT, KHXGRM, KHXPRO, KHXECH
, KHXECT, KHXEMH, KHXEMT, KHXDM1, KHXDT1
, KHXDM2, KHXDT2, KHXCOE, KHXCA1, KHXCT1
, KHXCU1, KHXCP1, KHXCX1, KHXCA2, KHXCT2
, KHXCU2, KHXCP2, KHXCX2, KHXAJU, KHXLMR
, KHXMBA, KHXTEN, KHXHON, KHXHOX, KHXBRG
, KHXBRL, KHXBAS, KHXBAT, KHXBAU, KHXBAM
, KHXXF1, KHXXB1, KHXXM1, KHXXF2, KHXXB2
, KHXXM2, KHXXF3, KHXXB3, KHXXM3, KHXREG
, KHXPEI, KHXKEA, KHXPBP, KHXKTD, KHXASV
, KHXPBT, KHXSIP, KHXPBS, KHXRIS, KHXPBR
, KHXRIA, KHXAVN FROM KPRGUG
WHERE KHXID = :id
";
            const string update=@"UPDATE KPRGUG SET 
KHXID = :KHXID, KHXKHWID = :KHXKHWID, KHXTYP = :KHXTYP, KHXIPB = :KHXIPB, KHXALX = :KHXALX, KHXRSQ = :KHXRSQ, KHXFOR = :KHXFOR, KHXKDEID = :KHXKDEID, KHXGARAN = :KHXGARAN, KHXDEBP = :KHXDEBP
, KHXFINP = :KHXFINP, KHXSIT = :KHXSIT, KHXTRG = :KHXTRG, KHXNPE = :KHXNPE, KHXVEN = :KHXVEN, KHXCAF = :KHXCAF, KHXCAU = :KHXCAU, KHXCAE = :KHXCAE, KHXMHC = :KHXMHC, KHXFRC = :KHXFRC
, KHXFR0 = :KHXFR0, KHXMHT = :KHXMHT, KHXMTX = :KHXMTX, KHXTTT = :KHXTTT, KHXMTT = :KHXMTT, KHXCNH = :KHXCNH, KHXCNT = :KHXCNT, KHXGRM = :KHXGRM, KHXPRO = :KHXPRO, KHXECH = :KHXECH
, KHXECT = :KHXECT, KHXEMH = :KHXEMH, KHXEMT = :KHXEMT, KHXDM1 = :KHXDM1, KHXDT1 = :KHXDT1, KHXDM2 = :KHXDM2, KHXDT2 = :KHXDT2, KHXCOE = :KHXCOE, KHXCA1 = :KHXCA1, KHXCT1 = :KHXCT1
, KHXCU1 = :KHXCU1, KHXCP1 = :KHXCP1, KHXCX1 = :KHXCX1, KHXCA2 = :KHXCA2, KHXCT2 = :KHXCT2, KHXCU2 = :KHXCU2, KHXCP2 = :KHXCP2, KHXCX2 = :KHXCX2, KHXAJU = :KHXAJU, KHXLMR = :KHXLMR
, KHXMBA = :KHXMBA, KHXTEN = :KHXTEN, KHXHON = :KHXHON, KHXHOX = :KHXHOX, KHXBRG = :KHXBRG, KHXBRL = :KHXBRL, KHXBAS = :KHXBAS, KHXBAT = :KHXBAT, KHXBAU = :KHXBAU, KHXBAM = :KHXBAM
, KHXXF1 = :KHXXF1, KHXXB1 = :KHXXB1, KHXXM1 = :KHXXM1, KHXXF2 = :KHXXF2, KHXXB2 = :KHXXB2, KHXXM2 = :KHXXM2, KHXXF3 = :KHXXF3, KHXXB3 = :KHXXB3, KHXXM3 = :KHXXM3, KHXREG = :KHXREG
, KHXPEI = :KHXPEI, KHXKEA = :KHXKEA, KHXPBP = :KHXPBP, KHXKTD = :KHXKTD, KHXASV = :KHXASV, KHXPBT = :KHXPBT, KHXSIP = :KHXSIP, KHXPBS = :KHXPBS, KHXRIS = :KHXRIS, KHXPBR = :KHXPBR
, KHXRIA = :KHXRIA, KHXAVN = :KHXAVN
 WHERE 
KHXID = :id";
            const string delete=@"DELETE FROM KPRGUG WHERE KHXID = :id";
            const string insert=@"INSERT INTO  KPRGUG (
KHXID, KHXKHWID, KHXTYP, KHXIPB, KHXALX
, KHXRSQ, KHXFOR, KHXKDEID, KHXGARAN, KHXDEBP
, KHXFINP, KHXSIT, KHXTRG, KHXNPE, KHXVEN
, KHXCAF, KHXCAU, KHXCAE, KHXMHC, KHXFRC
, KHXFR0, KHXMHT, KHXMTX, KHXTTT, KHXMTT
, KHXCNH, KHXCNT, KHXGRM, KHXPRO, KHXECH
, KHXECT, KHXEMH, KHXEMT, KHXDM1, KHXDT1
, KHXDM2, KHXDT2, KHXCOE, KHXCA1, KHXCT1
, KHXCU1, KHXCP1, KHXCX1, KHXCA2, KHXCT2
, KHXCU2, KHXCP2, KHXCX2, KHXAJU, KHXLMR
, KHXMBA, KHXTEN, KHXHON, KHXHOX, KHXBRG
, KHXBRL, KHXBAS, KHXBAT, KHXBAU, KHXBAM
, KHXXF1, KHXXB1, KHXXM1, KHXXF2, KHXXB2
, KHXXM2, KHXXF3, KHXXB3, KHXXM3, KHXREG
, KHXPEI, KHXKEA, KHXPBP, KHXKTD, KHXASV
, KHXPBT, KHXSIP, KHXPBS, KHXRIS, KHXPBR
, KHXRIA, KHXAVN
) VALUES (
:KHXID, :KHXKHWID, :KHXTYP, :KHXIPB, :KHXALX
, :KHXRSQ, :KHXFOR, :KHXKDEID, :KHXGARAN, :KHXDEBP
, :KHXFINP, :KHXSIT, :KHXTRG, :KHXNPE, :KHXVEN
, :KHXCAF, :KHXCAU, :KHXCAE, :KHXMHC, :KHXFRC
, :KHXFR0, :KHXMHT, :KHXMTX, :KHXTTT, :KHXMTT
, :KHXCNH, :KHXCNT, :KHXGRM, :KHXPRO, :KHXECH
, :KHXECT, :KHXEMH, :KHXEMT, :KHXDM1, :KHXDT1
, :KHXDM2, :KHXDT2, :KHXCOE, :KHXCA1, :KHXCT1
, :KHXCU1, :KHXCP1, :KHXCX1, :KHXCA2, :KHXCT2
, :KHXCU2, :KHXCP2, :KHXCX2, :KHXAJU, :KHXLMR
, :KHXMBA, :KHXTEN, :KHXHON, :KHXHOX, :KHXBRG
, :KHXBRL, :KHXBAS, :KHXBAT, :KHXBAU, :KHXBAM
, :KHXXF1, :KHXXB1, :KHXXM1, :KHXXF2, :KHXXB2
, :KHXXM2, :KHXXF3, :KHXXB3, :KHXXM3, :KHXREG
, :KHXPEI, :KHXKEA, :KHXPBP, :KHXKTD, :KHXASV
, :KHXPBT, :KHXSIP, :KHXPBS, :KHXRIS, :KHXPBR
, :KHXRIA, :KHXAVN)";
            const string select_GetByAffaire=@"SELECT
KHXID, KHXKHWID, KHXTYP, KHXIPB, KHXALX
, KHXRSQ, KHXFOR, KHXKDEID, KHXGARAN, KHXDEBP
, KHXFINP, KHXSIT, KHXTRG, KHXNPE, KHXVEN
, KHXCAF, KHXCAU, KHXCAE, KHXMHC, KHXFRC
, KHXFR0, KHXMHT, KHXMTX, KHXTTT, KHXMTT
, KHXCNH, KHXCNT, KHXGRM, KHXPRO, KHXECH
, KHXECT, KHXEMH, KHXEMT, KHXDM1, KHXDT1
, KHXDM2, KHXDT2, KHXCOE, KHXCA1, KHXCT1
, KHXCU1, KHXCP1, KHXCX1, KHXCA2, KHXCT2
, KHXCU2, KHXCP2, KHXCX2, KHXAJU, KHXLMR
, KHXMBA, KHXTEN, KHXHON, KHXHOX, KHXBRG
, KHXBRL, KHXBAS, KHXBAT, KHXBAU, KHXBAM
, KHXXF1, KHXXB1, KHXXM1, KHXXF2, KHXXB2
, KHXXM2, KHXXF3, KHXXB3, KHXXM3, KHXREG
, KHXPEI, KHXKEA, KHXPBP, KHXKTD, KHXASV
, KHXPBT, KHXSIP, KHXPBS, KHXRIS, KHXPBR
, KHXRIA, KHXAVN FROM KPRGUG
WHERE KHXTYP = :parKHXTYP
and KHXIPB = :parKHXIPB
and KHXALX = :parKHXALX
";
            const string select_GetByRegul=@"SELECT
KHXID, KHXKHWID, KHXTYP, KHXIPB, KHXALX
, KHXRSQ, KHXFOR, KHXKDEID, KHXGARAN, KHXDEBP
, KHXFINP, KHXSIT, KHXTRG, KHXNPE, KHXVEN
, KHXCAF, KHXCAU, KHXCAE, KHXMHC, KHXFRC
, KHXFR0, KHXMHT, KHXMTX, KHXTTT, KHXMTT
, KHXCNH, KHXCNT, KHXGRM, KHXPRO, KHXECH
, KHXECT, KHXEMH, KHXEMT, KHXDM1, KHXDT1
, KHXDM2, KHXDT2, KHXCOE, KHXCA1, KHXCT1
, KHXCU1, KHXCP1, KHXCX1, KHXCA2, KHXCT2
, KHXCU2, KHXCP2, KHXCX2, KHXAJU, KHXLMR
, KHXMBA, KHXTEN, KHXHON, KHXHOX, KHXBRG
, KHXBRL, KHXBAS, KHXBAT, KHXBAU, KHXBAM
, KHXXF1, KHXXB1, KHXXM1, KHXXF2, KHXXB2
, KHXXM2, KHXXF3, KHXXB3, KHXXM3, KHXREG
, KHXPEI, KHXKEA, KHXPBP, KHXKTD, KHXASV
, KHXPBT, KHXSIP, KHXPBS, KHXRIS, KHXPBR
, KHXRIA, KHXAVN FROM KPRGUG
WHERE KHXKHWID = :regulID
";
            #endregion

            public KpRguGRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpRguG Get(Int64 id){
                return connection.Query<KpRguG>(select, new {id}).SingleOrDefault();
            }

            public int NewId () {
                return idGenerator.NewId("KHXID") ;
            }

            public void Insert(KpRguG value){
                    if(value.Khxid == default(Int64)) {
                        value.Khxid = NewId();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("KHXID",value.Khxid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHXKHWID",value.Khxkhwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHXTYP",value.Khxtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXIPB",value.Khxipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHXALX",value.Khxalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHXRSQ",value.Khxrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHXFOR",value.Khxfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHXKDEID",value.Khxkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHXGARAN",value.Khxgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHXDEBP",value.Khxdebp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHXFINP",value.Khxfinp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHXSIT",value.Khxsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXTRG",value.Khxtrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXNPE",value.Khxnpe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHXVEN",value.Khxven, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXCAF",value.Khxcaf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KHXCAU",value.Khxcau, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KHXCAE",value.Khxcae, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KHXMHC",value.Khxmhc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXFRC",value.Khxfrc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXFR0",value.Khxfr0??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXMHT",value.Khxmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXMTX",value.Khxmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXTTT",value.Khxttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXMTT",value.Khxmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXCNH",value.Khxcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXCNT",value.Khxcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXGRM",value.Khxgrm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPRO",value.Khxpro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXECH",value.Khxech, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXECT",value.Khxect, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXEMH",value.Khxemh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXEMT",value.Khxemt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDM1",value.Khxdm1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDT1",value.Khxdt1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDM2",value.Khxdm2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDT2",value.Khxdt2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXCOE",value.Khxcoe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KHXCA1",value.Khxca1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCT1",value.Khxct1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KHXCU1",value.Khxcu1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXCP1",value.Khxcp1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCX1",value.Khxcx1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXCA2",value.Khxca2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCT2",value.Khxct2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KHXCU2",value.Khxcu2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXCP2",value.Khxcp2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCX2",value.Khxcx2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXAJU",value.Khxaju, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXLMR",value.Khxlmr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXMBA",value.Khxmba, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXTEN",value.Khxten, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXHON",value.Khxhon, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXHOX",value.Khxhox??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXBRG",value.Khxbrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHXBRL",value.Khxbrl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KHXBAS",value.Khxbas, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXBAT",value.Khxbat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KHXBAU",value.Khxbau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXBAM",value.Khxbam, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXXF1",value.Khxxf1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXXB1",value.Khxxb1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXM1",value.Khxxm1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXF2",value.Khxxf2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXXB2",value.Khxxb2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXM2",value.Khxxm2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXF3",value.Khxxf3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXXB3",value.Khxxb3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXM3",value.Khxxm3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXREG",value.Khxreg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXPEI",value.Khxpei, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXKEA",value.Khxkea, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBP",value.Khxpbp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXKTD",value.Khxktd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXASV",value.Khxasv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBT",value.Khxpbt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KHXSIP",value.Khxsip, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBS",value.Khxpbs, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXRIS",value.Khxris, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBR",value.Khxpbr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXRIA",value.Khxria, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXAVN",value.Khxavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpRguG value){
                    var parameters = new DynamicParameters();
                    parameters.Add("id",value.Khxid);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpRguG value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KHXID",value.Khxid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHXKHWID",value.Khxkhwid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHXTYP",value.Khxtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXIPB",value.Khxipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KHXALX",value.Khxalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KHXRSQ",value.Khxrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHXFOR",value.Khxfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHXKDEID",value.Khxkdeid, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KHXGARAN",value.Khxgaran??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KHXDEBP",value.Khxdebp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHXFINP",value.Khxfinp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KHXSIT",value.Khxsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXTRG",value.Khxtrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXNPE",value.Khxnpe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KHXVEN",value.Khxven, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXCAF",value.Khxcaf, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KHXCAU",value.Khxcau, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KHXCAE",value.Khxcae, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KHXMHC",value.Khxmhc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXFRC",value.Khxfrc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXFR0",value.Khxfr0??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXMHT",value.Khxmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXMTX",value.Khxmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXTTT",value.Khxttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXMTT",value.Khxmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXCNH",value.Khxcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXCNT",value.Khxcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXGRM",value.Khxgrm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPRO",value.Khxpro, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXECH",value.Khxech, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXECT",value.Khxect, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXEMH",value.Khxemh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXEMT",value.Khxemt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDM1",value.Khxdm1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDT1",value.Khxdt1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDM2",value.Khxdm2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXDT2",value.Khxdt2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXCOE",value.Khxcoe, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KHXCA1",value.Khxca1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCT1",value.Khxct1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KHXCU1",value.Khxcu1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXCP1",value.Khxcp1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCX1",value.Khxcx1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXCA2",value.Khxca2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCT2",value.Khxct2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 4);
                    parameters.Add("KHXCU2",value.Khxcu2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXCP2",value.Khxcp2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXCX2",value.Khxcx2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXAJU",value.Khxaju, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXLMR",value.Khxlmr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXMBA",value.Khxmba, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXTEN",value.Khxten, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXHON",value.Khxhon, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXHOX",value.Khxhox??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXBRG",value.Khxbrg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KHXBRL",value.Khxbrl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KHXBAS",value.Khxbas, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXBAT",value.Khxbat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KHXBAU",value.Khxbau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXBAM",value.Khxbam, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("KHXXF1",value.Khxxf1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXXB1",value.Khxxb1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXM1",value.Khxxm1, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXF2",value.Khxxf2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXXB2",value.Khxxb2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXM2",value.Khxxm2, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXF3",value.Khxxf3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXXB3",value.Khxxb3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXXM3",value.Khxxm3, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXREG",value.Khxreg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KHXPEI",value.Khxpei, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KHXKEA",value.Khxkea, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBP",value.Khxpbp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXKTD",value.Khxktd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXASV",value.Khxasv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBT",value.Khxpbt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("KHXSIP",value.Khxsip, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBS",value.Khxpbs, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXRIS",value.Khxris, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXPBR",value.Khxpbr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KHXRIA",value.Khxria, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KHXAVN",value.Khxavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("id",value.Khxid);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpRguG> GetByAffaire(string parKHXTYP, string parKHXIPB, int parKHXALX){
                    return connection.EnsureOpened().Query<KpRguG>(select_GetByAffaire, new {parKHXTYP, parKHXIPB, parKHXALX}).ToList();
            }
            public IEnumerable<KpRguG> GetByRegul(Int64 regulID){
                    return connection.EnsureOpened().Query<KpRguG>(select_GetByRegul, new {regulID}).ToList();
            }
    }
}
