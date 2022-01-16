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

    public  partial class  YhrtobjRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
JGIPB, JGALX, JGAVN, JGHIN, JGRSQ
, JGOBJ, JGCCH, JGIGD, JGBRA, JGSBR
, JGCAT, JGRCS, JGCCS, JGVAL, JGVAA
, JGVAW, JGVAT, JGVAU, JGVAH, JGNOJ
, JGMMQ, JGMTY, JGMSR, JGMFA, JGTEM
, JGVGD, JGVGU, JGVDA, JGVDM, JGVDJ
, JGVDH, JGVFA, JGVFM, JGVFJ, JGVFH
, JGRGT, JGTRR, JGCNA, JGINA, JGIND
, JGIXC, JGIXF, JGIXL, JGIXP, JGIVO
, JGIVA, JGIVW, JGGAU, JGGVL, JGGUN
, JGPBN, JGPBS, JGPBR, JGPBT, JGPBC
, JGPBP, JGPBA, JGCLV, JGAGM, JGAVE
, JGAVA, JGAVM, JGAVJ, JGRUL, JGRUT
, JGAVF FROM YHRTOBJ
WHERE JGIPB = :JGIPB
and JGALX = :JGALX
and JGAVN = :JGAVN
and JGHIN = :JGHIN
and JGRSQ = :JGRSQ
and JGOBJ = :JGOBJ
";
            const string update=@"UPDATE YHRTOBJ SET 
JGIPB = :JGIPB, JGALX = :JGALX, JGAVN = :JGAVN, JGHIN = :JGHIN, JGRSQ = :JGRSQ, JGOBJ = :JGOBJ, JGCCH = :JGCCH, JGIGD = :JGIGD, JGBRA = :JGBRA, JGSBR = :JGSBR
, JGCAT = :JGCAT, JGRCS = :JGRCS, JGCCS = :JGCCS, JGVAL = :JGVAL, JGVAA = :JGVAA, JGVAW = :JGVAW, JGVAT = :JGVAT, JGVAU = :JGVAU, JGVAH = :JGVAH, JGNOJ = :JGNOJ
, JGMMQ = :JGMMQ, JGMTY = :JGMTY, JGMSR = :JGMSR, JGMFA = :JGMFA, JGTEM = :JGTEM, JGVGD = :JGVGD, JGVGU = :JGVGU, JGVDA = :JGVDA, JGVDM = :JGVDM, JGVDJ = :JGVDJ
, JGVDH = :JGVDH, JGVFA = :JGVFA, JGVFM = :JGVFM, JGVFJ = :JGVFJ, JGVFH = :JGVFH, JGRGT = :JGRGT, JGTRR = :JGTRR, JGCNA = :JGCNA, JGINA = :JGINA, JGIND = :JGIND
, JGIXC = :JGIXC, JGIXF = :JGIXF, JGIXL = :JGIXL, JGIXP = :JGIXP, JGIVO = :JGIVO, JGIVA = :JGIVA, JGIVW = :JGIVW, JGGAU = :JGGAU, JGGVL = :JGGVL, JGGUN = :JGGUN
, JGPBN = :JGPBN, JGPBS = :JGPBS, JGPBR = :JGPBR, JGPBT = :JGPBT, JGPBC = :JGPBC, JGPBP = :JGPBP, JGPBA = :JGPBA, JGCLV = :JGCLV, JGAGM = :JGAGM, JGAVE = :JGAVE
, JGAVA = :JGAVA, JGAVM = :JGAVM, JGAVJ = :JGAVJ, JGRUL = :JGRUL, JGRUT = :JGRUT, JGAVF = :JGAVF
 WHERE 
JGIPB = :JGIPB and JGALX = :JGALX and JGAVN = :JGAVN and JGHIN = :JGHIN and JGRSQ = :JGRSQ and JGOBJ = :JGOBJ";
            const string delete=@"DELETE FROM YHRTOBJ WHERE JGIPB = :JGIPB AND JGALX = :JGALX AND JGAVN = :JGAVN AND JGHIN = :JGHIN AND JGRSQ = :JGRSQ AND JGOBJ = :JGOBJ";
            const string insert=@"INSERT INTO  YHRTOBJ (
JGIPB, JGALX, JGAVN, JGHIN, JGRSQ
, JGOBJ, JGCCH, JGIGD, JGBRA, JGSBR
, JGCAT, JGRCS, JGCCS, JGVAL, JGVAA
, JGVAW, JGVAT, JGVAU, JGVAH, JGNOJ
, JGMMQ, JGMTY, JGMSR, JGMFA, JGTEM
, JGVGD, JGVGU, JGVDA, JGVDM, JGVDJ
, JGVDH, JGVFA, JGVFM, JGVFJ, JGVFH
, JGRGT, JGTRR, JGCNA, JGINA, JGIND
, JGIXC, JGIXF, JGIXL, JGIXP, JGIVO
, JGIVA, JGIVW, JGGAU, JGGVL, JGGUN
, JGPBN, JGPBS, JGPBR, JGPBT, JGPBC
, JGPBP, JGPBA, JGCLV, JGAGM, JGAVE
, JGAVA, JGAVM, JGAVJ, JGRUL, JGRUT
, JGAVF
) VALUES (
:JGIPB, :JGALX, :JGAVN, :JGHIN, :JGRSQ
, :JGOBJ, :JGCCH, :JGIGD, :JGBRA, :JGSBR
, :JGCAT, :JGRCS, :JGCCS, :JGVAL, :JGVAA
, :JGVAW, :JGVAT, :JGVAU, :JGVAH, :JGNOJ
, :JGMMQ, :JGMTY, :JGMSR, :JGMFA, :JGTEM
, :JGVGD, :JGVGU, :JGVDA, :JGVDM, :JGVDJ
, :JGVDH, :JGVFA, :JGVFM, :JGVFJ, :JGVFH
, :JGRGT, :JGTRR, :JGCNA, :JGINA, :JGIND
, :JGIXC, :JGIXF, :JGIXL, :JGIXP, :JGIVO
, :JGIVA, :JGIVW, :JGGAU, :JGGVL, :JGGUN
, :JGPBN, :JGPBS, :JGPBR, :JGPBT, :JGPBC
, :JGPBP, :JGPBA, :JGCLV, :JGAGM, :JGAVE
, :JGAVA, :JGAVM, :JGAVJ, :JGRUL, :JGRUT
, :JGAVF)";
            const string select_GetByAffaire=@"SELECT
JGIPB, JGALX, JGAVN, JGHIN, JGRSQ
, JGOBJ, JGCCH, JGIGD, JGBRA, JGSBR
, JGCAT, JGRCS, JGCCS, JGVAL, JGVAA
, JGVAW, JGVAT, JGVAU, JGVAH, JGNOJ
, JGMMQ, JGMTY, JGMSR, JGMFA, JGTEM
, JGVGD, JGVGU, JGVDA, JGVDM, JGVDJ
, JGVDH, JGVFA, JGVFM, JGVFJ, JGVFH
, JGRGT, JGTRR, JGCNA, JGINA, JGIND
, JGIXC, JGIXF, JGIXL, JGIXP, JGIVO
, JGIVA, JGIVW, JGGAU, JGGVL, JGGUN
, JGPBN, JGPBS, JGPBR, JGPBT, JGPBC
, JGPBP, JGPBA, JGCLV, JGAGM, JGAVE
, JGAVA, JGAVM, JGAVJ, JGRUL, JGRUT
, JGAVF FROM YHRTOBJ
WHERE JGIPB = :JGIPB
and JGALX = :JGALX
and JGAVN = :JGAVN
";
            const string select_GetAllByIpbAlx=@"SELECT
JGIPB, JGALX, JGAVN, JGHIN, JGRSQ
, JGOBJ, JGCCH, JGIGD, JGBRA, JGSBR
, JGCAT, JGRCS, JGCCS, JGVAL, JGVAA
, JGVAW, JGVAT, JGVAU, JGVAH, JGNOJ
, JGMMQ, JGMTY, JGMSR, JGMFA, JGTEM
, JGVGD, JGVGU, JGVDA, JGVDM, JGVDJ
, JGVDH, JGVFA, JGVFM, JGVFJ, JGVFH
, JGRGT, JGTRR, JGCNA, JGINA, JGIND
, JGIXC, JGIXF, JGIXL, JGIXP, JGIVO
, JGIVA, JGIVW, JGGAU, JGGVL, JGGUN
, JGPBN, JGPBS, JGPBR, JGPBT, JGPBC
, JGPBP, JGPBA, JGCLV, JGAGM, JGAVE
, JGAVA, JGAVM, JGAVJ, JGRUL, JGRUT
, JGAVF FROM YHRTOBJ
WHERE JGIPB = :parJGIPB
and JGALX = :parJGALX
";
            #endregion

            public YhrtobjRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YprtObj Get(string JGIPB, int JGALX, int JGAVN, int JGHIN, int JGRSQ, int JGOBJ){
                return connection.Query<YprtObj>(select, new {JGIPB, JGALX, JGAVN, JGHIN, JGRSQ, JGOBJ}).SingleOrDefault();
            }


            public void Insert(YprtObj value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JGIPB",value.Jgipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JGALX",value.Jgalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGAVN",value.Jgavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGHIN",value.Jghin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGRSQ",value.Jgrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGOBJ",value.Jgobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGCCH",value.Jgcch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGIGD",value.Jgigd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JGBRA",value.Jgbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGSBR",value.Jgsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGCAT",value.Jgcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGRCS",value.Jgrcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JGCCS",value.Jgccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JGVAL",value.Jgval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JGVAA",value.Jgvaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JGVAW",value.Jgvaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JGVAT",value.Jgvat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGVAU",value.Jgvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGVAH",value.Jgvah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGNOJ",value.Jgnoj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGMMQ",value.Jgmmq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("JGMTY",value.Jgmty??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("JGMSR",value.Jgmsr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("JGMFA",value.Jgmfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGTEM",value.Jgtem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGVGD",value.Jgvgd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVGU",value.Jgvgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGVDA",value.Jgvda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGVDM",value.Jgvdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVDJ",value.Jgvdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVDH",value.Jgvdh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGVFA",value.Jgvfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGVFM",value.Jgvfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVFJ",value.Jgvfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVFH",value.Jgvfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGRGT",value.Jgrgt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGTRR",value.Jgtrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGCNA",value.Jgcna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGINA",value.Jgina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIND",value.Jgind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGIXC",value.Jgixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIXF",value.Jgixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIXL",value.Jgixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIXP",value.Jgixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIVO",value.Jgivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JGIVA",value.Jgiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JGIVW",value.Jgivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JGGAU",value.Jggau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGGVL",value.Jggvl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JGGUN",value.Jggun??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGPBN",value.Jgpbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGPBS",value.Jgpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGPBR",value.Jgpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGPBT",value.Jgpbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGPBC",value.Jgpbc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGPBP",value.Jgpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGPBA",value.Jgpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGCLV",value.Jgclv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGAGM",value.Jgagm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGAVE",value.Jgave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGAVA",value.Jgava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGAVM",value.Jgavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGAVJ",value.Jgavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGRUL",value.Jgrul??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGRUT",value.Jgrut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGAVF",value.Jgavf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YprtObj value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JGIPB",value.Jgipb);
                    parameters.Add("JGALX",value.Jgalx);
                    parameters.Add("JGAVN",value.Jgavn);
                    parameters.Add("JGHIN",value.Jghin);
                    parameters.Add("JGRSQ",value.Jgrsq);
                    parameters.Add("JGOBJ",value.Jgobj);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YprtObj value){
                    var parameters = new DynamicParameters();
                    parameters.Add("JGIPB",value.Jgipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JGALX",value.Jgalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGAVN",value.Jgavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGHIN",value.Jghin, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGRSQ",value.Jgrsq, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGOBJ",value.Jgobj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGCCH",value.Jgcch, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGIGD",value.Jgigd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("JGBRA",value.Jgbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGSBR",value.Jgsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGCAT",value.Jgcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGRCS",value.Jgrcs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JGCCS",value.Jgccs??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("JGVAL",value.Jgval, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JGVAA",value.Jgvaa, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JGVAW",value.Jgvaw, dbType:DbType.Int64, direction:ParameterDirection.Input, size:11, scale: 0);
                    parameters.Add("JGVAT",value.Jgvat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGVAU",value.Jgvau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGVAH",value.Jgvah??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGNOJ",value.Jgnoj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("JGMMQ",value.Jgmmq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("JGMTY",value.Jgmty??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("JGMSR",value.Jgmsr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:30, scale: 0);
                    parameters.Add("JGMFA",value.Jgmfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGTEM",value.Jgtem??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGVGD",value.Jgvgd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVGU",value.Jgvgu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGVDA",value.Jgvda, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGVDM",value.Jgvdm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVDJ",value.Jgvdj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVDH",value.Jgvdh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGVFA",value.Jgvfa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGVFM",value.Jgvfm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVFJ",value.Jgvfj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGVFH",value.Jgvfh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGRGT",value.Jgrgt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGTRR",value.Jgtrr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGCNA",value.Jgcna??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGINA",value.Jgina??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIND",value.Jgind??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGIXC",value.Jgixc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIXF",value.Jgixf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIXL",value.Jgixl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIXP",value.Jgixp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGIVO",value.Jgivo, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JGIVA",value.Jgiva, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JGIVW",value.Jgivw, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:7, scale: 2);
                    parameters.Add("JGGAU",value.Jggau??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGGVL",value.Jggvl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("JGGUN",value.Jggun??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGPBN",value.Jgpbn??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGPBS",value.Jgpbs, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGPBR",value.Jgpbr, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGPBT",value.Jgpbt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGPBC",value.Jgpbc, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGPBP",value.Jgpbp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGPBA",value.Jgpba, dbType:DbType.Int32, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGCLV",value.Jgclv??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGAGM",value.Jgagm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGAVE",value.Jgave, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGAVA",value.Jgava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("JGAVM",value.Jgavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGAVJ",value.Jgavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("JGRUL",value.Jgrul??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGRUT",value.Jgrut??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("JGAVF",value.Jgavf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("JGIPB",value.Jgipb);
                    parameters.Add("JGALX",value.Jgalx);
                    parameters.Add("JGAVN",value.Jgavn);
                    parameters.Add("JGHIN",value.Jghin);
                    parameters.Add("JGRSQ",value.Jgrsq);
                    parameters.Add("JGOBJ",value.Jgobj);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YprtObj> GetByAffaire(string JGIPB, int JGALX, int JGAVN){
                    return connection.EnsureOpened().Query<YprtObj>(select_GetByAffaire, new {JGIPB, JGALX, JGAVN}).ToList();
            }
            public IEnumerable<YprtObj> GetAllByIpbAlx(string parJGIPB, int parJGALX){
                    return connection.EnsureOpened().Query<YprtObj>(select_GetAllByIpbAlx, new {parJGIPB, parJGALX}).ToList();
            }
    }
}
