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

    public  partial class  YprtGarRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JHIPB, JHALX, JHRSQ, JHFOR, JHGAP
, JHGAR, JHCAR, JHNAT, JHGAN, JHOBN
, JHOB1, JHOB2, JHOB3, JHOB4, JHOB5
, JHGAV, JHRQV, JHFOV, JHREP, JHTAX
, JHLCV, JHLCA, JHLCW, JHLCU, JHLCE
, JHLC1, JHLC2, JHLC3, JHLC4, JHLC5
, JHLOV, JHLOA, JHLOW, JHLOU, JHLOE
, JHASV, JHASA, JHASW, JHASU, JHASN
, JHFHV, JHFHA, JHFHW, JHFHU, JHFHE
, JHPRV, JHPRA, JHPRW, JHPRU, JHPRP
, JHPRE, JHPRF, JHCNA, JHINA, JHEFA
, JHEFM, JHEFJ, JHEFH, JHFEA, JHFEM
, JHFEJ, JHFEH, JHEFD, JHEFU, JHTMC
, JHTFF, JHCMC, JHCHT, JHCTT, JHAJT
, JHDFG, JHIFC, JHCDA, JHCDM, JHCDJ
, JHCFA, JHCFM, JHCFJ, JHAVE, JHAVF
 FROM YPRTGAR
WHERE JHIPB = :JHIPB
and JHALX = :JHALX
and JHRSQ = :JHRSQ
and JHFOR = :JHFOR
and JHGAR = :JHGAR
";
            const string update=@"UPDATE YPRTGAR SET 
JHIPB = :JHIPB, JHALX = :JHALX, JHRSQ = :JHRSQ, JHFOR = :JHFOR, JHGAP = :JHGAP, JHGAR = :JHGAR, JHCAR = :JHCAR, JHNAT = :JHNAT, JHGAN = :JHGAN, JHOBN = :JHOBN
, JHOB1 = :JHOB1, JHOB2 = :JHOB2, JHOB3 = :JHOB3, JHOB4 = :JHOB4, JHOB5 = :JHOB5, JHGAV = :JHGAV, JHRQV = :JHRQV, JHFOV = :JHFOV, JHREP = :JHREP, JHTAX = :JHTAX
, JHLCV = :JHLCV, JHLCA = :JHLCA, JHLCW = :JHLCW, JHLCU = :JHLCU, JHLCE = :JHLCE, JHLC1 = :JHLC1, JHLC2 = :JHLC2, JHLC3 = :JHLC3, JHLC4 = :JHLC4, JHLC5 = :JHLC5
, JHLOV = :JHLOV, JHLOA = :JHLOA, JHLOW = :JHLOW, JHLOU = :JHLOU, JHLOE = :JHLOE, JHASV = :JHASV, JHASA = :JHASA, JHASW = :JHASW, JHASU = :JHASU, JHASN = :JHASN
, JHFHV = :JHFHV, JHFHA = :JHFHA, JHFHW = :JHFHW, JHFHU = :JHFHU, JHFHE = :JHFHE, JHPRV = :JHPRV, JHPRA = :JHPRA, JHPRW = :JHPRW, JHPRU = :JHPRU, JHPRP = :JHPRP
, JHPRE = :JHPRE, JHPRF = :JHPRF, JHCNA = :JHCNA, JHINA = :JHINA, JHEFA = :JHEFA, JHEFM = :JHEFM, JHEFJ = :JHEFJ, JHEFH = :JHEFH, JHFEA = :JHFEA, JHFEM = :JHFEM
, JHFEJ = :JHFEJ, JHFEH = :JHFEH, JHEFD = :JHEFD, JHEFU = :JHEFU, JHTMC = :JHTMC, JHTFF = :JHTFF, JHCMC = :JHCMC, JHCHT = :JHCHT, JHCTT = :JHCTT, JHAJT = :JHAJT
, JHDFG = :JHDFG, JHIFC = :JHIFC, JHCDA = :JHCDA, JHCDM = :JHCDM, JHCDJ = :JHCDJ, JHCFA = :JHCFA, JHCFM = :JHCFM, JHCFJ = :JHCFJ, JHAVE = :JHAVE, JHAVF = :JHAVF

 WHERE 
JHIPB = :JHIPB and JHALX = :JHALX and JHRSQ = :JHRSQ and JHFOR = :JHFOR and JHGAR = :JHGAR";
            const string delete=@"DELETE FROM YPRTGAR WHERE JHIPB = :JHIPB AND JHALX = :JHALX AND JHRSQ = :JHRSQ AND JHFOR = :JHFOR AND JHGAR = :JHGAR";
            const string insert=@"INSERT INTO  YPRTGAR (
JHIPB, JHALX, JHRSQ, JHFOR, JHGAP
, JHGAR, JHCAR, JHNAT, JHGAN, JHOBN
, JHOB1, JHOB2, JHOB3, JHOB4, JHOB5
, JHGAV, JHRQV, JHFOV, JHREP, JHTAX
, JHLCV, JHLCA, JHLCW, JHLCU, JHLCE
, JHLC1, JHLC2, JHLC3, JHLC4, JHLC5
, JHLOV, JHLOA, JHLOW, JHLOU, JHLOE
, JHASV, JHASA, JHASW, JHASU, JHASN
, JHFHV, JHFHA, JHFHW, JHFHU, JHFHE
, JHPRV, JHPRA, JHPRW, JHPRU, JHPRP
, JHPRE, JHPRF, JHCNA, JHINA, JHEFA
, JHEFM, JHEFJ, JHEFH, JHFEA, JHFEM
, JHFEJ, JHFEH, JHEFD, JHEFU, JHTMC
, JHTFF, JHCMC, JHCHT, JHCTT, JHAJT
, JHDFG, JHIFC, JHCDA, JHCDM, JHCDJ
, JHCFA, JHCFM, JHCFJ, JHAVE, JHAVF

) VALUES (
:JHIPB, :JHALX, :JHRSQ, :JHFOR, :JHGAP
, :JHGAR, :JHCAR, :JHNAT, :JHGAN, :JHOBN
, :JHOB1, :JHOB2, :JHOB3, :JHOB4, :JHOB5
, :JHGAV, :JHRQV, :JHFOV, :JHREP, :JHTAX
, :JHLCV, :JHLCA, :JHLCW, :JHLCU, :JHLCE
, :JHLC1, :JHLC2, :JHLC3, :JHLC4, :JHLC5
, :JHLOV, :JHLOA, :JHLOW, :JHLOU, :JHLOE
, :JHASV, :JHASA, :JHASW, :JHASU, :JHASN
, :JHFHV, :JHFHA, :JHFHW, :JHFHU, :JHFHE
, :JHPRV, :JHPRA, :JHPRW, :JHPRU, :JHPRP
, :JHPRE, :JHPRF, :JHCNA, :JHINA, :JHEFA
, :JHEFM, :JHEFJ, :JHEFH, :JHFEA, :JHFEM
, :JHFEJ, :JHFEH, :JHEFD, :JHEFU, :JHTMC
, :JHTFF, :JHCMC, :JHCHT, :JHCTT, :JHAJT
, :JHDFG, :JHIFC, :JHCDA, :JHCDM, :JHCDJ
, :JHCFA, :JHCFM, :JHCFJ, :JHAVE, :JHAVF
)";
            const string select_GetByAffaire=@"SELECT
JHIPB, JHALX, JHRSQ, JHFOR, JHGAP
, JHGAR, JHCAR, JHNAT, JHGAN, JHOBN
, JHOB1, JHOB2, JHOB3, JHOB4, JHOB5
, JHGAV, JHRQV, JHFOV, JHREP, JHTAX
, JHLCV, JHLCA, JHLCW, JHLCU, JHLCE
, JHLC1, JHLC2, JHLC3, JHLC4, JHLC5
, JHLOV, JHLOA, JHLOW, JHLOU, JHLOE
, JHASV, JHASA, JHASW, JHASU, JHASN
, JHFHV, JHFHA, JHFHW, JHFHU, JHFHE
, JHPRV, JHPRA, JHPRW, JHPRU, JHPRP
, JHPRE, JHPRF, JHCNA, JHINA, JHEFA
, JHEFM, JHEFJ, JHEFH, JHFEA, JHFEM
, JHFEJ, JHFEH, JHEFD, JHEFU, JHTMC
, JHTFF, JHCMC, JHCHT, JHCTT, JHAJT
, JHDFG, JHIFC, JHCDA, JHCDM, JHCDJ
, JHCFA, JHCFM, JHCFJ, JHAVE, JHAVF
 FROM YPRTGAR
WHERE JHIPB = :JHIPB
and JHALX = :JHALX
";
            #endregion

            public YprtGarRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtGar Get(string JHIPB, int JHALX, int JHRSQ, int JHFOR, string JHGAR){
                return connection.Query<YprtGar>(select, new {JHIPB, JHALX, JHRSQ, JHFOR, JHGAR}).SingleOrDefault();
            }


            public void Insert(YprtGar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JHIPB",value.Jhipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHALX",value.Jhalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHRSQ",value.Jhrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHFOR",value.Jhfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHGAP",value.Jhgap, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHGAR",value.Jhgar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("JHCAR",value.Jhcar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHNAT",value.Jhnat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHGAN",value.Jhgan??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHOBN",value.Jhobn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHOB1",value.Jhob1, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB2",value.Jhob2, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB3",value.Jhob3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB4",value.Jhob4, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB5",value.Jhob5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHGAV",value.Jhgav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHRQV",value.Jhrqv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHFOV",value.Jhfov, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHREP",value.Jhrep, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHTAX",value.Jhtax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHLCV",value.Jhlcv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLCA",value.Jhlca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLCW",value.Jhlcw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLCU",value.Jhlcu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHLCE",value.Jhlce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHLC1",value.Jhlc1, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC2",value.Jhlc2, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC3",value.Jhlc3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC4",value.Jhlc4, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC5",value.Jhlc5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLOV",value.Jhlov, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLOA",value.Jhloa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLOW",value.Jhlow, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLOU",value.Jhlou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHLOE",value.Jhloe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHASV",value.Jhasv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHASA",value.Jhasa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHASW",value.Jhasw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHASU",value.Jhasu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHASN",value.Jhasn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHFHV",value.Jhfhv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHFHA",value.Jhfha, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHFHW",value.Jhfhw, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHFHU",value.Jhfhu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHFHE",value.Jhfhe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHPRV",value.Jhprv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 3);
                    parameters.Add("JHPRA",value.Jhpra, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 3);
                    parameters.Add("JHPRW",value.Jhprw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 3);
                    parameters.Add("JHPRU",value.Jhpru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHPRP",value.Jhprp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHPRE",value.Jhpre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHPRF",value.Jhprf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHCNA",value.Jhcna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHINA",value.Jhina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHEFA",value.Jhefa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHEFM",value.Jhefm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHEFJ",value.Jhefj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHEFH",value.Jhefh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHFEA",value.Jhfea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHFEM",value.Jhfem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHFEJ",value.Jhfej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHFEH",value.Jhfeh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHEFD",value.Jhefd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHEFU",value.Jhefu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHTMC",value.Jhtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHTFF",value.Jhtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHCMC",value.Jhcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHCHT",value.Jhcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHCTT",value.Jhctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHAJT",value.Jhajt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHDFG",value.Jhdfg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHIFC",value.Jhifc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHCDA",value.Jhcda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHCDM",value.Jhcdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHCDJ",value.Jhcdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHCFA",value.Jhcfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHCFM",value.Jhcfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHCFJ",value.Jhcfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHAVE",value.Jhave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHAVF",value.Jhavf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtGar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JHIPB",value.Jhipb);
                    parameters.Add("JHALX",value.Jhalx);
                    parameters.Add("JHRSQ",value.Jhrsq);
                    parameters.Add("JHFOR",value.Jhfor);
                    parameters.Add("JHGAR",value.Jhgar);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtGar value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JHIPB",value.Jhipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHALX",value.Jhalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHRSQ",value.Jhrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHFOR",value.Jhfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHGAP",value.Jhgap, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHGAR",value.Jhgar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("JHCAR",value.Jhcar??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHNAT",value.Jhnat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHGAN",value.Jhgan??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHOBN",value.Jhobn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHOB1",value.Jhob1, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB2",value.Jhob2, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB3",value.Jhob3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB4",value.Jhob4, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHOB5",value.Jhob5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHGAV",value.Jhgav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHRQV",value.Jhrqv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHFOV",value.Jhfov, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHREP",value.Jhrep, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHTAX",value.Jhtax??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHLCV",value.Jhlcv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLCA",value.Jhlca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLCW",value.Jhlcw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLCU",value.Jhlcu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHLCE",value.Jhlce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHLC1",value.Jhlc1, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC2",value.Jhlc2, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC3",value.Jhlc3, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC4",value.Jhlc4, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLC5",value.Jhlc5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JHLOV",value.Jhlov, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLOA",value.Jhloa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLOW",value.Jhlow, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHLOU",value.Jhlou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHLOE",value.Jhloe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHASV",value.Jhasv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHASA",value.Jhasa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHASW",value.Jhasw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JHASU",value.Jhasu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHASN",value.Jhasn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHFHV",value.Jhfhv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHFHA",value.Jhfha, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHFHW",value.Jhfhw, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JHFHU",value.Jhfhu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHFHE",value.Jhfhe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHPRV",value.Jhprv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 3);
                    parameters.Add("JHPRA",value.Jhpra, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 3);
                    parameters.Add("JHPRW",value.Jhprw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 3);
                    parameters.Add("JHPRU",value.Jhpru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHPRP",value.Jhprp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHPRE",value.Jhpre??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHPRF",value.Jhprf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHCNA",value.Jhcna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHINA",value.Jhina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHEFA",value.Jhefa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHEFM",value.Jhefm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHEFJ",value.Jhefj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHEFH",value.Jhefh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHFEA",value.Jhfea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHFEM",value.Jhfem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHFEJ",value.Jhfej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHFEH",value.Jhfeh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHEFD",value.Jhefd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHEFU",value.Jhefu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHTMC",value.Jhtmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHTFF",value.Jhtff, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHCMC",value.Jhcmc, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHCHT",value.Jhcht, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHCTT",value.Jhctt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("JHAJT",value.Jhajt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHDFG",value.Jhdfg??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHIFC",value.Jhifc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JHCDA",value.Jhcda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHCDM",value.Jhcdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHCDJ",value.Jhcdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHCFA",value.Jhcfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JHCFM",value.Jhcfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHCFJ",value.Jhcfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JHAVE",value.Jhave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHAVF",value.Jhavf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JHIPB",value.Jhipb);
                    parameters.Add("JHALX",value.Jhalx);
                    parameters.Add("JHRSQ",value.Jhrsq);
                    parameters.Add("JHFOR",value.Jhfor);
                    parameters.Add("JHGAR",value.Jhgar);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtGar> GetByAffaire(string JHIPB, int JHALX){
                    return connection.EnsureOpened().Query<YprtGar>(select_GetByAffaire, new {JHIPB, JHALX}).ToList();
            }
    }
}
