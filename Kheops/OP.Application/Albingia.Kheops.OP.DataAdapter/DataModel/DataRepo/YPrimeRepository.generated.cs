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

    public  partial class  YPrimeRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PKIPB, PKALX, PKIPK, PKAVN, PKAVC
, PKAVI, PKCAP, PKEHA, PKEHM, PKEHJ
, PKEMT, PKEMA, PKEMM, PKEMJ, PKOPE
, PKMVT, PKMVM, PKSIT, PKSTA, PKSTM
, PKSTJ, PKPRA, PKPNT, PKMHT, PKMTX
, PKAFR, PKAFT, PKATM, PKTTT, PKMTT
, PKIDV, PKMTR, PKPER, PKNPL, PKCOT
, PKCOM, PKDPA, PKDPM, PKDPJ, PKFPA
, PKFPM, PKPFJ, PKCPT, PKRLC, PKRLA
, PKRLM, PKRLJ, PKRLL, PKCRU, PKCRA
, PKCRM, PKCRJ, PKMJU, PKMJA, PKMJM
, PKMJJ, PKENC, PKMOT, PKCNH, PKCNT
, PKCNL, PKCNM, PKCNC, PKICT, PKDEV
, PKCPA, PKCPM, PKTAC, PKAFF, PKDVR
, PKKCA, PKKHT, PKKHX, PKKFA, PKKFT
, PKKAT, PKKTX, PKKTT, PKKTR, PKKCO
, PKKNH, PKKNT, PKKNL, PKKNM, PKAAR
, PKAXC, PKMCE, PKKCE, PKDCA, PKDCM
, PKDCJ, PKFCA, PKFCM, PKFCJ, PKDEM
, PKGRG, PKGRN, PKGRH, PKGRB, PKKRG
, PKKRN, PKKRH, PKKRB FROM YPRIMES
WHERE PKIPB = :PKIPB
and PKALX = :PKALX
and PKIPK = :PKIPK
";
            const string update=@"UPDATE YPRIMES SET 
PKIPB = :PKIPB, PKALX = :PKALX, PKIPK = :PKIPK, PKAVN = :PKAVN, PKAVC = :PKAVC, PKAVI = :PKAVI, PKCAP = :PKCAP, PKEHA = :PKEHA, PKEHM = :PKEHM, PKEHJ = :PKEHJ
, PKEMT = :PKEMT, PKEMA = :PKEMA, PKEMM = :PKEMM, PKEMJ = :PKEMJ, PKOPE = :PKOPE, PKMVT = :PKMVT, PKMVM = :PKMVM, PKSIT = :PKSIT, PKSTA = :PKSTA, PKSTM = :PKSTM
, PKSTJ = :PKSTJ, PKPRA = :PKPRA, PKPNT = :PKPNT, PKMHT = :PKMHT, PKMTX = :PKMTX, PKAFR = :PKAFR, PKAFT = :PKAFT, PKATM = :PKATM, PKTTT = :PKTTT, PKMTT = :PKMTT
, PKIDV = :PKIDV, PKMTR = :PKMTR, PKPER = :PKPER, PKNPL = :PKNPL, PKCOT = :PKCOT, PKCOM = :PKCOM, PKDPA = :PKDPA, PKDPM = :PKDPM, PKDPJ = :PKDPJ, PKFPA = :PKFPA
, PKFPM = :PKFPM, PKPFJ = :PKPFJ, PKCPT = :PKCPT, PKRLC = :PKRLC, PKRLA = :PKRLA, PKRLM = :PKRLM, PKRLJ = :PKRLJ, PKRLL = :PKRLL, PKCRU = :PKCRU, PKCRA = :PKCRA
, PKCRM = :PKCRM, PKCRJ = :PKCRJ, PKMJU = :PKMJU, PKMJA = :PKMJA, PKMJM = :PKMJM, PKMJJ = :PKMJJ, PKENC = :PKENC, PKMOT = :PKMOT, PKCNH = :PKCNH, PKCNT = :PKCNT
, PKCNL = :PKCNL, PKCNM = :PKCNM, PKCNC = :PKCNC, PKICT = :PKICT, PKDEV = :PKDEV, PKCPA = :PKCPA, PKCPM = :PKCPM, PKTAC = :PKTAC, PKAFF = :PKAFF, PKDVR = :PKDVR
, PKKCA = :PKKCA, PKKHT = :PKKHT, PKKHX = :PKKHX, PKKFA = :PKKFA, PKKFT = :PKKFT, PKKAT = :PKKAT, PKKTX = :PKKTX, PKKTT = :PKKTT, PKKTR = :PKKTR, PKKCO = :PKKCO
, PKKNH = :PKKNH, PKKNT = :PKKNT, PKKNL = :PKKNL, PKKNM = :PKKNM, PKAAR = :PKAAR, PKAXC = :PKAXC, PKMCE = :PKMCE, PKKCE = :PKKCE, PKDCA = :PKDCA, PKDCM = :PKDCM
, PKDCJ = :PKDCJ, PKFCA = :PKFCA, PKFCM = :PKFCM, PKFCJ = :PKFCJ, PKDEM = :PKDEM, PKGRG = :PKGRG, PKGRN = :PKGRN, PKGRH = :PKGRH, PKGRB = :PKGRB, PKKRG = :PKKRG
, PKKRN = :PKKRN, PKKRH = :PKKRH, PKKRB = :PKKRB
 WHERE 
PKIPB = :PKIPB and PKALX = :PKALX and PKIPK = :PKIPK";
            const string delete=@"DELETE FROM YPRIMES WHERE PKIPB = :PKIPB AND PKALX = :PKALX AND PKIPK = :PKIPK";
            const string insert=@"INSERT INTO  YPRIMES (
PKIPB, PKALX, PKIPK, PKAVN, PKAVC
, PKAVI, PKCAP, PKEHA, PKEHM, PKEHJ
, PKEMT, PKEMA, PKEMM, PKEMJ, PKOPE
, PKMVT, PKMVM, PKSIT, PKSTA, PKSTM
, PKSTJ, PKPRA, PKPNT, PKMHT, PKMTX
, PKAFR, PKAFT, PKATM, PKTTT, PKMTT
, PKIDV, PKMTR, PKPER, PKNPL, PKCOT
, PKCOM, PKDPA, PKDPM, PKDPJ, PKFPA
, PKFPM, PKPFJ, PKCPT, PKRLC, PKRLA
, PKRLM, PKRLJ, PKRLL, PKCRU, PKCRA
, PKCRM, PKCRJ, PKMJU, PKMJA, PKMJM
, PKMJJ, PKENC, PKMOT, PKCNH, PKCNT
, PKCNL, PKCNM, PKCNC, PKICT, PKDEV
, PKCPA, PKCPM, PKTAC, PKAFF, PKDVR
, PKKCA, PKKHT, PKKHX, PKKFA, PKKFT
, PKKAT, PKKTX, PKKTT, PKKTR, PKKCO
, PKKNH, PKKNT, PKKNL, PKKNM, PKAAR
, PKAXC, PKMCE, PKKCE, PKDCA, PKDCM
, PKDCJ, PKFCA, PKFCM, PKFCJ, PKDEM
, PKGRG, PKGRN, PKGRH, PKGRB, PKKRG
, PKKRN, PKKRH, PKKRB
) VALUES (
:PKIPB, :PKALX, :PKIPK, :PKAVN, :PKAVC
, :PKAVI, :PKCAP, :PKEHA, :PKEHM, :PKEHJ
, :PKEMT, :PKEMA, :PKEMM, :PKEMJ, :PKOPE
, :PKMVT, :PKMVM, :PKSIT, :PKSTA, :PKSTM
, :PKSTJ, :PKPRA, :PKPNT, :PKMHT, :PKMTX
, :PKAFR, :PKAFT, :PKATM, :PKTTT, :PKMTT
, :PKIDV, :PKMTR, :PKPER, :PKNPL, :PKCOT
, :PKCOM, :PKDPA, :PKDPM, :PKDPJ, :PKFPA
, :PKFPM, :PKPFJ, :PKCPT, :PKRLC, :PKRLA
, :PKRLM, :PKRLJ, :PKRLL, :PKCRU, :PKCRA
, :PKCRM, :PKCRJ, :PKMJU, :PKMJA, :PKMJM
, :PKMJJ, :PKENC, :PKMOT, :PKCNH, :PKCNT
, :PKCNL, :PKCNM, :PKCNC, :PKICT, :PKDEV
, :PKCPA, :PKCPM, :PKTAC, :PKAFF, :PKDVR
, :PKKCA, :PKKHT, :PKKHX, :PKKFA, :PKKFT
, :PKKAT, :PKKTX, :PKKTT, :PKKTR, :PKKCO
, :PKKNH, :PKKNT, :PKKNL, :PKKNM, :PKAAR
, :PKAXC, :PKMCE, :PKKCE, :PKDCA, :PKDCM
, :PKDCJ, :PKFCA, :PKFCM, :PKFCJ, :PKDEM
, :PKGRG, :PKGRN, :PKGRH, :PKGRB, :PKKRG
, :PKKRN, :PKKRH, :PKKRB)";
            const string select_GetByAffaire=@"SELECT
PKIPB, PKALX, PKIPK, PKAVN, PKAVC
, PKAVI, PKCAP, PKEHA, PKEHM, PKEHJ
, PKEMT, PKEMA, PKEMM, PKEMJ, PKOPE
, PKMVT, PKMVM, PKSIT, PKSTA, PKSTM
, PKSTJ, PKPRA, PKPNT, PKMHT, PKMTX
, PKAFR, PKAFT, PKATM, PKTTT, PKMTT
, PKIDV, PKMTR, PKPER, PKNPL, PKCOT
, PKCOM, PKDPA, PKDPM, PKDPJ, PKFPA
, PKFPM, PKPFJ, PKCPT, PKRLC, PKRLA
, PKRLM, PKRLJ, PKRLL, PKCRU, PKCRA
, PKCRM, PKCRJ, PKMJU, PKMJA, PKMJM
, PKMJJ, PKENC, PKMOT, PKCNH, PKCNT
, PKCNL, PKCNM, PKCNC, PKICT, PKDEV
, PKCPA, PKCPM, PKTAC, PKAFF, PKDVR
, PKKCA, PKKHT, PKKHX, PKKFA, PKKFT
, PKKAT, PKKTX, PKKTT, PKKTR, PKKCO
, PKKNH, PKKNT, PKKNL, PKKNM, PKAAR
, PKAXC, PKMCE, PKKCE, PKDCA, PKDCM
, PKDCJ, PKFCA, PKFCM, PKFCJ, PKDEM
, PKGRG, PKGRN, PKGRH, PKGRB, PKKRG
, PKKRN, PKKRH, PKKRB FROM YPRIMES
WHERE PKIPB = :PKIPB
and PKALX = :PKALX
and PKAVN = :PKAVN
";
            #endregion

            public YPrimeRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YPrime Get(string PKIPB, int PKALX, int PKIPK){
                return connection.Query<YPrime>(select, new {PKIPB, PKALX, PKIPK}).SingleOrDefault();
            }


            public void Insert(YPrime value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PKIPB",value.Pkipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PKALX",value.Pkalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKIPK",value.Pkipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKAVN",value.Pkavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKAVC",value.Pkavc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKAVI",value.Pkavi, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("PKCAP",value.Pkcap, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("PKEHA",value.Pkeha, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKEHM",value.Pkehm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKEHJ",value.Pkehj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKEMT",value.Pkemt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKEMA",value.Pkema, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKEMM",value.Pkemm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKEMJ",value.Pkemj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKOPE",value.Pkope, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMVT",value.Pkmvt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKMVM",value.Pkmvm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKSIT",value.Pksit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKSTA",value.Pksta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKSTM",value.Pkstm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKSTJ",value.Pkstj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKPRA",value.Pkpra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKPNT",value.Pkpnt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMHT",value.Pkmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKMTX",value.Pkmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKAFR",value.Pkafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PKAFT",value.Pkaft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKATM",value.Pkatm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKTTT",value.Pkttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKMTT",value.Pkmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKIDV",value.Pkidv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("PKMTR",value.Pkmtr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKPER",value.Pkper??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKNPL",value.Pknpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKCOT",value.Pkcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCOM",value.Pkcom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKDPA",value.Pkdpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKDPM",value.Pkdpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKDPJ",value.Pkdpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKFPA",value.Pkfpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKFPM",value.Pkfpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKPFJ",value.Pkpfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKCPT",value.Pkcpt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKRLC",value.Pkrlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKRLA",value.Pkrla, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKRLM",value.Pkrlm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKRLJ",value.Pkrlj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKRLL",value.Pkrll, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PKCRU",value.Pkcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PKCRA",value.Pkcra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKCRM",value.Pkcrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKCRJ",value.Pkcrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMJU",value.Pkmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PKMJA",value.Pkmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKMJM",value.Pkmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMJJ",value.Pkmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKENC",value.Pkenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKMOT",value.Pkmot??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKCNH",value.Pkcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNT",value.Pkcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNL",value.Pkcnl, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNM",value.Pkcnm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNC",value.Pkcnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("PKICT",value.Pkict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PKDEV",value.Pkdev??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKCPA",value.Pkcpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKCPM",value.Pkcpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKTAC",value.Pktac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKAFF",value.Pkaff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKDVR",value.Pkdvr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKKCA",value.Pkkca, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("PKKHT",value.Pkkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKHX",value.Pkkhx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKFA",value.Pkkfa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("PKKFT",value.Pkkft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKKAT",value.Pkkat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKKTX",value.Pkktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKTT",value.Pkktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKTR",value.Pkktr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKCO",value.Pkkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNH",value.Pkknh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNT",value.Pkknt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNL",value.Pkknl, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNM",value.Pkknm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKAAR",value.Pkaar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKAXC",value.Pkaxc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKMCE",value.Pkmce, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKCE",value.Pkkce, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKDCA",value.Pkdca, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKDCM",value.Pkdcm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKDCJ",value.Pkdcj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKFCA",value.Pkfca, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKFCM",value.Pkfcm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKFCJ",value.Pkfcj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKDEM",value.Pkdem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKGRG",value.Pkgrg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKGRN",value.Pkgrn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKGRH",value.Pkgrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKGRB",value.Pkgrb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRG",value.Pkkrg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRN",value.Pkkrn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRH",value.Pkkrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRB",value.Pkkrb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YPrime value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PKIPB",value.Pkipb);
                    parameters.Add("PKALX",value.Pkalx);
                    parameters.Add("PKIPK",value.Pkipk);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YPrime value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PKIPB",value.Pkipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PKALX",value.Pkalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKIPK",value.Pkipk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKAVN",value.Pkavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKAVC",value.Pkavc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKAVI",value.Pkavi, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("PKCAP",value.Pkcap, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("PKEHA",value.Pkeha, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKEHM",value.Pkehm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKEHJ",value.Pkehj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKEMT",value.Pkemt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKEMA",value.Pkema, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKEMM",value.Pkemm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKEMJ",value.Pkemj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKOPE",value.Pkope, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMVT",value.Pkmvt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKMVM",value.Pkmvm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKSIT",value.Pksit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKSTA",value.Pksta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKSTM",value.Pkstm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKSTJ",value.Pkstj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKPRA",value.Pkpra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKPNT",value.Pkpnt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMHT",value.Pkmht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKMTX",value.Pkmtx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKAFR",value.Pkafr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PKAFT",value.Pkaft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKATM",value.Pkatm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKTTT",value.Pkttt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKMTT",value.Pkmtt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKIDV",value.Pkidv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("PKMTR",value.Pkmtr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKPER",value.Pkper??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKNPL",value.Pknpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKCOT",value.Pkcot, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCOM",value.Pkcom, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKDPA",value.Pkdpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKDPM",value.Pkdpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKDPJ",value.Pkdpj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKFPA",value.Pkfpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKFPM",value.Pkfpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKPFJ",value.Pkpfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKCPT",value.Pkcpt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKRLC",value.Pkrlc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKRLA",value.Pkrla, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKRLM",value.Pkrlm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKRLJ",value.Pkrlj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKRLL",value.Pkrll, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PKCRU",value.Pkcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PKCRA",value.Pkcra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKCRM",value.Pkcrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKCRJ",value.Pkcrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMJU",value.Pkmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PKMJA",value.Pkmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKMJM",value.Pkmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKMJJ",value.Pkmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKENC",value.Pkenc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKMOT",value.Pkmot??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKCNH",value.Pkcnh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNT",value.Pkcnt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNL",value.Pkcnl, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNM",value.Pkcnm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKCNC",value.Pkcnc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 3);
                    parameters.Add("PKICT",value.Pkict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PKDEV",value.Pkdev??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKCPA",value.Pkcpa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKCPM",value.Pkcpm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKTAC",value.Pktac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKAFF",value.Pkaff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKDVR",value.Pkdvr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PKKCA",value.Pkkca, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:13, scale: 2);
                    parameters.Add("PKKHT",value.Pkkht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKHX",value.Pkkhx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKFA",value.Pkkfa, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("PKKFT",value.Pkkft, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKKAT",value.Pkkat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKKTX",value.Pkktx, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKTT",value.Pkktt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKTR",value.Pkktr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKCO",value.Pkkco, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNH",value.Pkknh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNT",value.Pkknt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNL",value.Pkknl, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKNM",value.Pkknm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKAAR",value.Pkaar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKAXC",value.Pkaxc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PKMCE",value.Pkmce, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKCE",value.Pkkce, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKDCA",value.Pkdca, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKDCM",value.Pkdcm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKDCJ",value.Pkdcj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKFCA",value.Pkfca, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PKFCM",value.Pkfcm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKFCJ",value.Pkfcj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PKDEM",value.Pkdem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PKGRG",value.Pkgrg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKGRN",value.Pkgrn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKGRH",value.Pkgrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKGRB",value.Pkgrb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRG",value.Pkkrg, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRN",value.Pkkrn, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRH",value.Pkkrh, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKKRB",value.Pkkrb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("PKIPB",value.Pkipb);
                    parameters.Add("PKALX",value.Pkalx);
                    parameters.Add("PKIPK",value.Pkipk);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YPrime> GetByAffaire(string PKIPB, int PKALX, int PKAVN){
                    return connection.EnsureOpened().Query<YPrime>(select_GetByAffaire, new {PKIPB, PKALX, PKAVN}).ToList();
            }
    }
}
