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

    public  partial class  KpIrSGaRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
KFDTYP, KFDIPB, KFDALX, KFDAVN, KFDHIN
, KFDFOR, KFDOPT, KFDCRD, KFDCRH, KFDMAJU
, KFDMAJD, KFDMAJH, KFDAN02, KFDAN03, KFDBO01
, KFDBO02, KFDBO03, KFDIM08, KFDIM09, KFDIM10
, KFDNBGR, KFDEFFV, KFDCNVD, KFDFRDM, KFDSORN
, KFDSORD, KFDSORR, KFD05, KFD06, KFD07
, KFD08, KFD09, KFDIA01, KFDIA02, KFDIA03
, KFDIARA17, KFDRSAT, KFDRCPS, KFDRCPF, KFDRCPD
, KFDRASB, KFDRASL, KFDRASS, KFDCOTNB, KFDCOTMT
, KFDMNT, KFDMNTNB, KFDMNTMT, KFDRGU, KFDNBJI
, KFDAN04, KFDGARAV, KFDVOLAV, KFDVOLAP, KFDLMA
, KFDMXM, KFDRAY, KFDAN05, KFDRAY5, KFDAN06
, KFDAN07, KFDCLAL FROM KPIRSGA
WHERE KFDTYP = :parKFDTYP
and KFDIPB = :parKFDIPB
and KFDALX = :parKFDALX
and KFDFOR = :parKFDFOR
and KFDOPT = :parKFDOPT
";
            const string update=@"UPDATE KPIRSGA SET 
KFDTYP = :KFDTYP, KFDIPB = :KFDIPB, KFDALX = :KFDALX, KFDAVN = :KFDAVN, KFDHIN = :KFDHIN, KFDFOR = :KFDFOR, KFDOPT = :KFDOPT, KFDCRD = :KFDCRD, KFDCRH = :KFDCRH, KFDMAJU = :KFDMAJU
, KFDMAJD = :KFDMAJD, KFDMAJH = :KFDMAJH, KFDAN02 = :KFDAN02, KFDAN03 = :KFDAN03, KFDBO01 = :KFDBO01, KFDBO02 = :KFDBO02, KFDBO03 = :KFDBO03, KFDIM08 = :KFDIM08, KFDIM09 = :KFDIM09, KFDIM10 = :KFDIM10
, KFDNBGR = :KFDNBGR, KFDEFFV = :KFDEFFV, KFDCNVD = :KFDCNVD, KFDFRDM = :KFDFRDM, KFDSORN = :KFDSORN, KFDSORD = :KFDSORD, KFDSORR = :KFDSORR, KFD05 = :KFD05, KFD06 = :KFD06, KFD07 = :KFD07
, KFD08 = :KFD08, KFD09 = :KFD09, KFDIA01 = :KFDIA01, KFDIA02 = :KFDIA02, KFDIA03 = :KFDIA03, KFDIARA17 = :KFDIARA17, KFDRSAT = :KFDRSAT, KFDRCPS = :KFDRCPS, KFDRCPF = :KFDRCPF, KFDRCPD = :KFDRCPD
, KFDRASB = :KFDRASB, KFDRASL = :KFDRASL, KFDRASS = :KFDRASS, KFDCOTNB = :KFDCOTNB, KFDCOTMT = :KFDCOTMT, KFDMNT = :KFDMNT, KFDMNTNB = :KFDMNTNB, KFDMNTMT = :KFDMNTMT, KFDRGU = :KFDRGU, KFDNBJI = :KFDNBJI
, KFDAN04 = :KFDAN04, KFDGARAV = :KFDGARAV, KFDVOLAV = :KFDVOLAV, KFDVOLAP = :KFDVOLAP, KFDLMA = :KFDLMA, KFDMXM = :KFDMXM, KFDRAY = :KFDRAY, KFDAN05 = :KFDAN05, KFDRAY5 = :KFDRAY5, KFDAN06 = :KFDAN06
, KFDAN07 = :KFDAN07, KFDCLAL = :KFDCLAL
 WHERE 
KFDTYP = :parKFDTYP and KFDIPB = :parKFDIPB and KFDALX = :parKFDALX and KFDFOR = :parKFDFOR and KFDOPT = :parKFDOPT";
            const string delete=@"DELETE FROM KPIRSGA WHERE KFDTYP = :parKFDTYP AND KFDIPB = :parKFDIPB AND KFDALX = :parKFDALX AND KFDFOR = :parKFDFOR AND KFDOPT = :parKFDOPT";
            const string insert=@"INSERT INTO  KPIRSGA (
KFDTYP, KFDIPB, KFDALX, KFDAVN, KFDHIN
, KFDFOR, KFDOPT, KFDCRD, KFDCRH, KFDMAJU
, KFDMAJD, KFDMAJH, KFDAN02, KFDAN03, KFDBO01
, KFDBO02, KFDBO03, KFDIM08, KFDIM09, KFDIM10
, KFDNBGR, KFDEFFV, KFDCNVD, KFDFRDM, KFDSORN
, KFDSORD, KFDSORR, KFD05, KFD06, KFD07
, KFD08, KFD09, KFDIA01, KFDIA02, KFDIA03
, KFDIARA17, KFDRSAT, KFDRCPS, KFDRCPF, KFDRCPD
, KFDRASB, KFDRASL, KFDRASS, KFDCOTNB, KFDCOTMT
, KFDMNT, KFDMNTNB, KFDMNTMT, KFDRGU, KFDNBJI
, KFDAN04, KFDGARAV, KFDVOLAV, KFDVOLAP, KFDLMA
, KFDMXM, KFDRAY, KFDAN05, KFDRAY5, KFDAN06
, KFDAN07, KFDCLAL
) VALUES (
:KFDTYP, :KFDIPB, :KFDALX, :KFDAVN, :KFDHIN
, :KFDFOR, :KFDOPT, :KFDCRD, :KFDCRH, :KFDMAJU
, :KFDMAJD, :KFDMAJH, :KFDAN02, :KFDAN03, :KFDBO01
, :KFDBO02, :KFDBO03, :KFDIM08, :KFDIM09, :KFDIM10
, :KFDNBGR, :KFDEFFV, :KFDCNVD, :KFDFRDM, :KFDSORN
, :KFDSORD, :KFDSORR, :KFD05, :KFD06, :KFD07
, :KFD08, :KFD09, :KFDIA01, :KFDIA02, :KFDIA03
, :KFDIARA17, :KFDRSAT, :KFDRCPS, :KFDRCPF, :KFDRCPD
, :KFDRASB, :KFDRASL, :KFDRASS, :KFDCOTNB, :KFDCOTMT
, :KFDMNT, :KFDMNTNB, :KFDMNTMT, :KFDRGU, :KFDNBJI
, :KFDAN04, :KFDGARAV, :KFDVOLAV, :KFDVOLAP, :KFDLMA
, :KFDMXM, :KFDRAY, :KFDAN05, :KFDRAY5, :KFDAN06
, :KFDAN07, :KFDCLAL)";
            const string select_GetByAffaire=@"SELECT
KFDTYP, KFDIPB, KFDALX, KFDAVN, KFDHIN
, KFDFOR, KFDOPT, KFDCRD, KFDCRH, KFDMAJU
, KFDMAJD, KFDMAJH, KFDAN02, KFDAN03, KFDBO01
, KFDBO02, KFDBO03, KFDIM08, KFDIM09, KFDIM10
, KFDNBGR, KFDEFFV, KFDCNVD, KFDFRDM, KFDSORN
, KFDSORD, KFDSORR, KFD05, KFD06, KFD07
, KFD08, KFD09, KFDIA01, KFDIA02, KFDIA03
, KFDIARA17, KFDRSAT, KFDRCPS, KFDRCPF, KFDRCPD
, KFDRASB, KFDRASL, KFDRASS, KFDCOTNB, KFDCOTMT
, KFDMNT, KFDMNTNB, KFDMNTMT, KFDRGU, KFDNBJI
, KFDAN04, KFDGARAV, KFDVOLAV, KFDVOLAP, KFDLMA
, KFDMXM, KFDRAY, KFDAN05, KFDRAY5, KFDAN06
, KFDAN07, KFDCLAL FROM KPIRSGA
WHERE KFDTYP = :typeAffaire
and KFDIPB = :codeAffaire
and KFDALX = :version
";
            #endregion

            public KpIrSGaRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public KpIrSGa Get(string parKFDTYP, string parKFDIPB, int parKFDALX, int parKFDFOR, int parKFDOPT){
                return connection.Query<KpIrSGa>(select, new {parKFDTYP, parKFDIPB, parKFDALX, parKFDFOR, parKFDOPT}).SingleOrDefault();
            }


            public void Insert(KpIrSGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFDTYP",value.Kfdtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDIPB",value.Kfdipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFDALX",value.Kfdalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFDAVN",value.Kfdavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDHIN",value.Kfdhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDFOR",value.Kfdfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDOPT",value.Kfdopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDCRD",value.Kfdcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFDCRH",value.Kfdcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFDMAJU",value.Kfdmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFDMAJD",value.Kfdmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFDMAJH",value.Kfdmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFDAN02",value.Kfdan02??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDAN03",value.Kfdan03, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDBO01",value.Kfdbo01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDBO02",value.Kfdbo02, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KFDBO03",value.Kfdbo03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDIM08",value.Kfdim08??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDIM09",value.Kfdim09, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDIM10",value.Kfdim10??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDNBGR",value.Kfdnbgr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDEFFV",value.Kfdeffv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KFDCNVD",value.Kfdcnvd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDFRDM",value.Kfdfrdm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KFDSORN",value.Kfdsorn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDSORD",value.Kfdsord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFDSORR",value.Kfdsorr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 2);
                    parameters.Add("KFD05",value.Kfd05??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFD06",value.Kfd06??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFD07",value.Kfd07, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFD08",value.Kfd08??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFD09",value.Kfd09, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFDIA01",value.Kfdia01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDIA02",value.Kfdia02, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDIA03",value.Kfdia03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDIARA17",value.Kfdiara17, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDRSAT",value.Kfdrsat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KFDRCPS",value.Kfdrcps, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRCPF",value.Kfdrcpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRCPD",value.Kfdrcpd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRASB",value.Kfdrasb, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KFDRASL",value.Kfdrasl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KFDRASS",value.Kfdrass, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDCOTNB",value.Kfdcotnb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDCOTMT",value.Kfdcotmt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDMNT",value.Kfdmnt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDMNTNB",value.Kfdmntnb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KFDMNTMT",value.Kfdmntmt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRGU",value.Kfdrgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDNBJI",value.Kfdnbji, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDAN04",value.Kfdan04??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDGARAV",value.Kfdgarav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDVOLAV",value.Kfdvolav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDVOLAP",value.Kfdvolap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDLMA",value.Kfdlma, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDMXM",value.Kfdmxm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRAY",value.Kfdray, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDAN05",value.Kfdan05, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDRAY5",value.Kfdray5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDAN06",value.Kfdan06, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDAN07",value.Kfdan07, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDCLAL",value.Kfdclal??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(KpIrSGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("parKFDTYP",value.Kfdtyp);
                    parameters.Add("parKFDIPB",value.Kfdipb);
                    parameters.Add("parKFDALX",value.Kfdalx);
                    parameters.Add("parKFDFOR",value.Kfdfor);
                    parameters.Add("parKFDOPT",value.Kfdopt);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(KpIrSGa value){
                    var parameters = new DynamicParameters();
                    parameters.Add("KFDTYP",value.Kfdtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDIPB",value.Kfdipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("KFDALX",value.Kfdalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("KFDAVN",value.Kfdavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDHIN",value.Kfdhin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDFOR",value.Kfdfor, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDOPT",value.Kfdopt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDCRD",value.Kfdcrd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFDCRH",value.Kfdcrh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFDMAJU",value.Kfdmaju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("KFDMAJD",value.Kfdmajd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFDMAJH",value.Kfdmajh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("KFDAN02",value.Kfdan02??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDAN03",value.Kfdan03, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDBO01",value.Kfdbo01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDBO02",value.Kfdbo02, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KFDBO03",value.Kfdbo03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDIM08",value.Kfdim08??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDIM09",value.Kfdim09, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDIM10",value.Kfdim10??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDNBGR",value.Kfdnbgr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDEFFV",value.Kfdeffv, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("KFDCNVD",value.Kfdcnvd??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDFRDM",value.Kfdfrdm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:6, scale: 3);
                    parameters.Add("KFDSORN",value.Kfdsorn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDSORD",value.Kfdsord, dbType:DbType.Int32, direction:ParameterDirection.Input, size:8, scale: 0);
                    parameters.Add("KFDSORR",value.Kfdsorr, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:15, scale: 2);
                    parameters.Add("KFD05",value.Kfd05??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFD06",value.Kfd06??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFD07",value.Kfd07, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFD08",value.Kfd08??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFD09",value.Kfd09, dbType:DbType.Int64, direction:ParameterDirection.Input, size:15, scale: 0);
                    parameters.Add("KFDIA01",value.Kfdia01??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDIA02",value.Kfdia02, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDIA03",value.Kfdia03??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("KFDIARA17",value.Kfdiara17, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDRSAT",value.Kfdrsat, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KFDRCPS",value.Kfdrcps, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRCPF",value.Kfdrcpf, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRCPD",value.Kfdrcpd, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRASB",value.Kfdrasb, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KFDRASL",value.Kfdrasl, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("KFDRASS",value.Kfdrass, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDCOTNB",value.Kfdcotnb, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDCOTMT",value.Kfdcotmt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDMNT",value.Kfdmnt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDMNTNB",value.Kfdmntnb, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("KFDMNTMT",value.Kfdmntmt, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRGU",value.Kfdrgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDNBJI",value.Kfdnbji, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDAN04",value.Kfdan04??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("KFDGARAV",value.Kfdgarav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDVOLAV",value.Kfdvolav??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDVOLAP",value.Kfdvolap??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("KFDLMA",value.Kfdlma, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("KFDMXM",value.Kfdmxm, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:11, scale: 2);
                    parameters.Add("KFDRAY",value.Kfdray, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDAN05",value.Kfdan05, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDRAY5",value.Kfdray5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("KFDAN06",value.Kfdan06, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDAN07",value.Kfdan07, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("KFDCLAL",value.Kfdclal??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("parKFDTYP",value.Kfdtyp);
                    parameters.Add("parKFDIPB",value.Kfdipb);
                    parameters.Add("parKFDALX",value.Kfdalx);
                    parameters.Add("parKFDFOR",value.Kfdfor);
                    parameters.Add("parKFDOPT",value.Kfdopt);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<KpIrSGa> GetByAffaire(string typeAffaire, string codeAffaire, int version){
                    return connection.EnsureOpened().Query<KpIrSGa>(select_GetByAffaire, new {typeAffaire, codeAffaire, version}).ToList();
            }
    }
}
