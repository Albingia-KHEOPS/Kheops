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

    public  partial class  YprtRsqRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JEIPB, JEALX, JERSQ, JECCH, JEMGD
, JEBRA, JESBR, JECAT, JERCS, JECCS
, JEVAL, JEVAA, JEVAW, JEVAT, JEVAU
, JEVAH, JETEM, JEVGD, JEVGU, JEVDA
, JEVDM, JEVDJ, JEVDH, JEVFA, JEVFM
, JEVFJ, JEVFH, JEOBJ, JEROJ, JERGT
, JETRR, JECNA, JEINA, JEIND, JEIXC
, JEIXF, JEIXL, JEIXP, JEGAU, JEGVL
, JEGUN, JEPBN, JEPBS, JEPBR, JEPBT
, JEPBC, JEPBP, JEPBA, JECLV, JEAGM
, JEIPM, JEIVX, JEDRO, JENBO, JELCV
, JELCA, JELCW, JELCU, JELCE, JEAVE
, JEAVA, JEAVM, JEAVJ, JERUL, JERUT
, JEAVF, JEEXT FROM YPRTRSQ
WHERE JEIPB = :numeroAffaire
and JEALX = :version
and JERSQ = :numRisque
";
            const string update=@"UPDATE YPRTRSQ SET 
JEIPB = :JEIPB, JEALX = :JEALX, JERSQ = :JERSQ, JECCH = :JECCH, JEMGD = :JEMGD, JEBRA = :JEBRA, JESBR = :JESBR, JECAT = :JECAT, JERCS = :JERCS, JECCS = :JECCS
, JEVAL = :JEVAL, JEVAA = :JEVAA, JEVAW = :JEVAW, JEVAT = :JEVAT, JEVAU = :JEVAU, JEVAH = :JEVAH, JETEM = :JETEM, JEVGD = :JEVGD, JEVGU = :JEVGU, JEVDA = :JEVDA
, JEVDM = :JEVDM, JEVDJ = :JEVDJ, JEVDH = :JEVDH, JEVFA = :JEVFA, JEVFM = :JEVFM, JEVFJ = :JEVFJ, JEVFH = :JEVFH, JEOBJ = :JEOBJ, JEROJ = :JEROJ, JERGT = :JERGT
, JETRR = :JETRR, JECNA = :JECNA, JEINA = :JEINA, JEIND = :JEIND, JEIXC = :JEIXC, JEIXF = :JEIXF, JEIXL = :JEIXL, JEIXP = :JEIXP, JEGAU = :JEGAU, JEGVL = :JEGVL
, JEGUN = :JEGUN, JEPBN = :JEPBN, JEPBS = :JEPBS, JEPBR = :JEPBR, JEPBT = :JEPBT, JEPBC = :JEPBC, JEPBP = :JEPBP, JEPBA = :JEPBA, JECLV = :JECLV, JEAGM = :JEAGM
, JEIPM = :JEIPM, JEIVX = :JEIVX, JEDRO = :JEDRO, JENBO = :JENBO, JELCV = :JELCV, JELCA = :JELCA, JELCW = :JELCW, JELCU = :JELCU, JELCE = :JELCE, JEAVE = :JEAVE
, JEAVA = :JEAVA, JEAVM = :JEAVM, JEAVJ = :JEAVJ, JERUL = :JERUL, JERUT = :JERUT, JEAVF = :JEAVF, JEEXT = :JEEXT
 WHERE 
JEIPB = :numeroAffaire and JEALX = :version and JERSQ = :numRisque";
            const string delete=@"DELETE FROM YPRTRSQ WHERE JEIPB = :numeroAffaire AND JEALX = :version AND JERSQ = :numRisque";
            const string insert=@"INSERT INTO  YPRTRSQ (
JEIPB, JEALX, JERSQ, JECCH, JEMGD
, JEBRA, JESBR, JECAT, JERCS, JECCS
, JEVAL, JEVAA, JEVAW, JEVAT, JEVAU
, JEVAH, JETEM, JEVGD, JEVGU, JEVDA
, JEVDM, JEVDJ, JEVDH, JEVFA, JEVFM
, JEVFJ, JEVFH, JEOBJ, JEROJ, JERGT
, JETRR, JECNA, JEINA, JEIND, JEIXC
, JEIXF, JEIXL, JEIXP, JEGAU, JEGVL
, JEGUN, JEPBN, JEPBS, JEPBR, JEPBT
, JEPBC, JEPBP, JEPBA, JECLV, JEAGM
, JEIPM, JEIVX, JEDRO, JENBO, JELCV
, JELCA, JELCW, JELCU, JELCE, JEAVE
, JEAVA, JEAVM, JEAVJ, JERUL, JERUT
, JEAVF, JEEXT
) VALUES (
:JEIPB, :JEALX, :JERSQ, :JECCH, :JEMGD
, :JEBRA, :JESBR, :JECAT, :JERCS, :JECCS
, :JEVAL, :JEVAA, :JEVAW, :JEVAT, :JEVAU
, :JEVAH, :JETEM, :JEVGD, :JEVGU, :JEVDA
, :JEVDM, :JEVDJ, :JEVDH, :JEVFA, :JEVFM
, :JEVFJ, :JEVFH, :JEOBJ, :JEROJ, :JERGT
, :JETRR, :JECNA, :JEINA, :JEIND, :JEIXC
, :JEIXF, :JEIXL, :JEIXP, :JEGAU, :JEGVL
, :JEGUN, :JEPBN, :JEPBS, :JEPBR, :JEPBT
, :JEPBC, :JEPBP, :JEPBA, :JECLV, :JEAGM
, :JEIPM, :JEIVX, :JEDRO, :JENBO, :JELCV
, :JELCA, :JELCW, :JELCU, :JELCE, :JEAVE
, :JEAVA, :JEAVM, :JEAVJ, :JERUL, :JERUT
, :JEAVF, :JEEXT)";
            const string select_Liste=@"SELECT
JEIPB, JEALX, JERSQ, JECCH, JEMGD
, JEBRA, JESBR, JECAT, JERCS, JECCS
, JEVAL, JEVAA, JEVAW, JEVAT, JEVAU
, JEVAH, JETEM, JEVGD, JEVGU, JEVDA
, JEVDM, JEVDJ, JEVDH, JEVFA, JEVFM
, JEVFJ, JEVFH, JEOBJ, JEROJ, JERGT
, JETRR, JECNA, JEINA, JEIND, JEIXC
, JEIXF, JEIXL, JEIXP, JEGAU, JEGVL
, JEGUN, JEPBN, JEPBS, JEPBR, JEPBT
, JEPBC, JEPBP, JEPBA, JECLV, JEAGM
, JEIPM, JEIVX, JEDRO, JENBO, JELCV
, JELCA, JELCW, JELCU, JELCE, JEAVE
, JEAVA, JEAVM, JEAVJ, JERUL, JERUT
, JEAVF, JEEXT FROM YPRTRSQ
WHERE JEIPB = :numAffaire
and JEALX = :version
FETCH FIRST 200 ROWS ONLY
";
            #endregion

            public YprtRsqRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtRsq Get(string numeroAffaire, int version, int numRisque){
                return connection.Query<YprtRsq>(select, new {numeroAffaire, version, numRisque}).SingleOrDefault();
            }


            public void Insert(YprtRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JEIPB",value.Jeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JEALX",value.Jealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JERSQ",value.Jersq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JECCH",value.Jecch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JEMGD",value.Jemgd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JEBRA",value.Jebra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JESBR",value.Jesbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JECAT",value.Jecat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JERCS",value.Jercs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JECCS",value.Jeccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JEVAL",value.Jeval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JEVAA",value.Jevaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JEVAW",value.Jevaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JEVAT",value.Jevat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JEVAU",value.Jevau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEVAH",value.Jevah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JETEM",value.Jetem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEVGD",value.Jevgd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVGU",value.Jevgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEVDA",value.Jevda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEVDM",value.Jevdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVDJ",value.Jevdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVDH",value.Jevdh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEVFA",value.Jevfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEVFM",value.Jevfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVFJ",value.Jevfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVFH",value.Jevfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEOBJ",value.Jeobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JEROJ",value.Jeroj??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JERGT",value.Jergt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JETRR",value.Jetrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JECNA",value.Jecna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEINA",value.Jeina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIND",value.Jeind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEIXC",value.Jeixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIXF",value.Jeixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIXL",value.Jeixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIXP",value.Jeixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEGAU",value.Jegau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEGVL",value.Jegvl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JEGUN",value.Jegun??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEPBN",value.Jepbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEPBS",value.Jepbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEPBR",value.Jepbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEPBT",value.Jepbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEPBC",value.Jepbc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEPBP",value.Jepbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEPBA",value.Jepba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JECLV",value.Jeclv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEAGM",value.Jeagm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEIPM",value.Jeipm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIVX",value.Jeivx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEDRO",value.Jedro, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JENBO",value.Jenbo, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JELCV",value.Jelcv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JELCA",value.Jelca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JELCW",value.Jelcw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JELCU",value.Jelcu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JELCE",value.Jelce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEAVE",value.Jeave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEAVA",value.Jeava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEAVM",value.Jeavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEAVJ",value.Jeavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JERUL",value.Jerul??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JERUT",value.Jerut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEAVF",value.Jeavf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEEXT",value.Jeext??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("numeroAffaire",value.Jeipb);
                    parameters.Add("version",value.Jealx);
                    parameters.Add("numRisque",value.Jersq);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtRsq value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JEIPB",value.Jeipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JEALX",value.Jealx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JERSQ",value.Jersq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JECCH",value.Jecch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JEMGD",value.Jemgd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JEBRA",value.Jebra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JESBR",value.Jesbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JECAT",value.Jecat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JERCS",value.Jercs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JECCS",value.Jeccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JEVAL",value.Jeval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JEVAA",value.Jevaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JEVAW",value.Jevaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JEVAT",value.Jevat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JEVAU",value.Jevau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEVAH",value.Jevah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JETEM",value.Jetem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEVGD",value.Jevgd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVGU",value.Jevgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEVDA",value.Jevda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEVDM",value.Jevdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVDJ",value.Jevdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVDH",value.Jevdh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEVFA",value.Jevfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEVFM",value.Jevfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVFJ",value.Jevfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEVFH",value.Jevfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEOBJ",value.Jeobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JEROJ",value.Jeroj??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JERGT",value.Jergt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JETRR",value.Jetrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JECNA",value.Jecna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEINA",value.Jeina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIND",value.Jeind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEIXC",value.Jeixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIXF",value.Jeixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIXL",value.Jeixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIXP",value.Jeixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEGAU",value.Jegau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEGVL",value.Jegvl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JEGUN",value.Jegun??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEPBN",value.Jepbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEPBS",value.Jepbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEPBR",value.Jepbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEPBT",value.Jepbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEPBC",value.Jepbc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEPBP",value.Jepbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEPBA",value.Jepba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JECLV",value.Jeclv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEAGM",value.Jeagm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEIPM",value.Jeipm??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEIVX",value.Jeivx??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEDRO",value.Jedro, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JENBO",value.Jenbo, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JELCV",value.Jelcv, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JELCA",value.Jelca, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JELCW",value.Jelcw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JELCU",value.Jelcu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JELCE",value.Jelce??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEAVE",value.Jeave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEAVA",value.Jeava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JEAVM",value.Jeavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JEAVJ",value.Jeavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JERUL",value.Jerul??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JERUT",value.Jerut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JEAVF",value.Jeavf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JEEXT",value.Jeext??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("numeroAffaire",value.Jeipb);
                    parameters.Add("version",value.Jealx);
                    parameters.Add("numRisque",value.Jersq);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtRsq> Liste(string numAffaire, int version){
                    return connection.EnsureOpened().Query<YprtRsq>(select_Liste, new {numAffaire, version}).ToList();
            }
    }
}
