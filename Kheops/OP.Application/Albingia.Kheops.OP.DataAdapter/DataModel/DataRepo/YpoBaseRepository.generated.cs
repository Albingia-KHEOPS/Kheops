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

    public  partial class  YpoBaseRepository : BaseTableRepository {

            private IdentifierGenerator idGenerator;

            #region Query texts        
            //ZALBINKHEO
            const string select=@"SELECT
PBTYP, PBIPB, PBALX, PBTBR, PBIAS
, PBREF, PBBRA, PBSBR, PBCAT, PBICT
, PBTIL, PBINL, PBOCT, PBAD1, PBAD2
, PBDEP, PBCPO, PBVIL, PBPAY, PBSAA
, PBSAM, PBSAJ, PBSAH, PBAPR, PBAPP
, PBMO1, PBMO2, PBMO3, PBCLE, PBANT
, PBCTD, PBCTU, PBAPO, PBDUR, PBREL
, PBRLD, PBATT, PBRMP, PBVRF, PBOLV
, PBFOA, PBFOM, PBCOU, PBFOE, PBDEV
, PBRGT, PBTCO, PBNAT, PBDST, PBLIE
, PBGES, PBSOU, PBORI, PBMAI, PBEFA
, PBEFM, PBEFJ, PBOFF, PBOFV, PBIPA
, PBALA, PBECM, PBECJ, PBPCV, PBCTA
, PBNRQ, PBNPL, PBPER, PBAVN, PBAVC
, PBAVA, PBAVM, PBAVJ, PBRSC, PBRSA
, PBRSM, PBRSJ, PBMER, PBETA, PBSIT
, PBSTA, PBSTM, PBSTJ, PBSTQ, PBEDT
, PBTAC, PBTAA, PBTAM, PBTAJ, PBCRU
, PBCRA, PBCRM, PBCRJ, PBMJU, PBMJA
, PBMJM, PBMJJ, PBDEU, PBDEA, PBDEM
, PBDEJ, PBCTP, PBEFH, PBFEA, PBFEM
, PBFEJ, PBFEH, PBSTP, PBNVA, PBFEC
, PBPOR, PBCON, PBTTR, PBAVK, PBADH
, PBSTF, PBPIM, PBIN5, PBORK FROM YPOBASE
WHERE PBTYP = :typeAffaire
and PBIPB = :numeroAffaire
and PBALX = :version
";
            const string update=@"UPDATE YPOBASE SET 
PBTYP = :PBTYP, PBIPB = :PBIPB, PBALX = :PBALX, PBTBR = :PBTBR, PBIAS = :PBIAS, PBREF = :PBREF, PBBRA = :PBBRA, PBSBR = :PBSBR, PBCAT = :PBCAT, PBICT = :PBICT
, PBTIL = :PBTIL, PBINL = :PBINL, PBOCT = :PBOCT, PBAD1 = :PBAD1, PBAD2 = :PBAD2, PBDEP = :PBDEP, PBCPO = :PBCPO, PBVIL = :PBVIL, PBPAY = :PBPAY, PBSAA = :PBSAA
, PBSAM = :PBSAM, PBSAJ = :PBSAJ, PBSAH = :PBSAH, PBAPR = :PBAPR, PBAPP = :PBAPP, PBMO1 = :PBMO1, PBMO2 = :PBMO2, PBMO3 = :PBMO3, PBCLE = :PBCLE, PBANT = :PBANT
, PBCTD = :PBCTD, PBCTU = :PBCTU, PBAPO = :PBAPO, PBDUR = :PBDUR, PBREL = :PBREL, PBRLD = :PBRLD, PBATT = :PBATT, PBRMP = :PBRMP, PBVRF = :PBVRF, PBOLV = :PBOLV
, PBFOA = :PBFOA, PBFOM = :PBFOM, PBCOU = :PBCOU, PBFOE = :PBFOE, PBDEV = :PBDEV, PBRGT = :PBRGT, PBTCO = :PBTCO, PBNAT = :PBNAT, PBDST = :PBDST, PBLIE = :PBLIE
, PBGES = :PBGES, PBSOU = :PBSOU, PBORI = :PBORI, PBMAI = :PBMAI, PBEFA = :PBEFA, PBEFM = :PBEFM, PBEFJ = :PBEFJ, PBOFF = :PBOFF, PBOFV = :PBOFV, PBIPA = :PBIPA
, PBALA = :PBALA, PBECM = :PBECM, PBECJ = :PBECJ, PBPCV = :PBPCV, PBCTA = :PBCTA, PBNRQ = :PBNRQ, PBNPL = :PBNPL, PBPER = :PBPER, PBAVN = :PBAVN, PBAVC = :PBAVC
, PBAVA = :PBAVA, PBAVM = :PBAVM, PBAVJ = :PBAVJ, PBRSC = :PBRSC, PBRSA = :PBRSA, PBRSM = :PBRSM, PBRSJ = :PBRSJ, PBMER = :PBMER, PBETA = :PBETA, PBSIT = :PBSIT
, PBSTA = :PBSTA, PBSTM = :PBSTM, PBSTJ = :PBSTJ, PBSTQ = :PBSTQ, PBEDT = :PBEDT, PBTAC = :PBTAC, PBTAA = :PBTAA, PBTAM = :PBTAM, PBTAJ = :PBTAJ, PBCRU = :PBCRU
, PBCRA = :PBCRA, PBCRM = :PBCRM, PBCRJ = :PBCRJ, PBMJU = :PBMJU, PBMJA = :PBMJA, PBMJM = :PBMJM, PBMJJ = :PBMJJ, PBDEU = :PBDEU, PBDEA = :PBDEA, PBDEM = :PBDEM
, PBDEJ = :PBDEJ, PBCTP = :PBCTP, PBEFH = :PBEFH, PBFEA = :PBFEA, PBFEM = :PBFEM, PBFEJ = :PBFEJ, PBFEH = :PBFEH, PBSTP = :PBSTP, PBNVA = :PBNVA, PBFEC = :PBFEC
, PBPOR = :PBPOR, PBCON = :PBCON, PBTTR = :PBTTR, PBAVK = :PBAVK, PBADH = :PBADH, PBSTF = :PBSTF, PBPIM = :PBPIM, PBIN5 = :PBIN5, PBORK = :PBORK
 WHERE 
PBTYP = :typeAffaire and PBIPB = :numeroAffaire and PBALX = :version";
            const string delete=@"DELETE FROM YPOBASE WHERE PBTYP = :typeAffaire AND PBIPB = :numeroAffaire AND PBALX = :version";
            const string insert=@"INSERT INTO  YPOBASE (
PBTYP, PBIPB, PBALX, PBTBR, PBIAS
, PBREF, PBBRA, PBSBR, PBCAT, PBICT
, PBTIL, PBINL, PBOCT, PBAD1, PBAD2
, PBDEP, PBCPO, PBVIL, PBPAY, PBSAA
, PBSAM, PBSAJ, PBSAH, PBAPR, PBAPP
, PBMO1, PBMO2, PBMO3, PBCLE, PBANT
, PBCTD, PBCTU, PBAPO, PBDUR, PBREL
, PBRLD, PBATT, PBRMP, PBVRF, PBOLV
, PBFOA, PBFOM, PBCOU, PBFOE, PBDEV
, PBRGT, PBTCO, PBNAT, PBDST, PBLIE
, PBGES, PBSOU, PBORI, PBMAI, PBEFA
, PBEFM, PBEFJ, PBOFF, PBOFV, PBIPA
, PBALA, PBECM, PBECJ, PBPCV, PBCTA
, PBNRQ, PBNPL, PBPER, PBAVN, PBAVC
, PBAVA, PBAVM, PBAVJ, PBRSC, PBRSA
, PBRSM, PBRSJ, PBMER, PBETA, PBSIT
, PBSTA, PBSTM, PBSTJ, PBSTQ, PBEDT
, PBTAC, PBTAA, PBTAM, PBTAJ, PBCRU
, PBCRA, PBCRM, PBCRJ, PBMJU, PBMJA
, PBMJM, PBMJJ, PBDEU, PBDEA, PBDEM
, PBDEJ, PBCTP, PBEFH, PBFEA, PBFEM
, PBFEJ, PBFEH, PBSTP, PBNVA, PBFEC
, PBPOR, PBCON, PBTTR, PBAVK, PBADH
, PBSTF, PBPIM, PBIN5, PBORK
) VALUES (
:PBTYP, :PBIPB, :PBALX, :PBTBR, :PBIAS
, :PBREF, :PBBRA, :PBSBR, :PBCAT, :PBICT
, :PBTIL, :PBINL, :PBOCT, :PBAD1, :PBAD2
, :PBDEP, :PBCPO, :PBVIL, :PBPAY, :PBSAA
, :PBSAM, :PBSAJ, :PBSAH, :PBAPR, :PBAPP
, :PBMO1, :PBMO2, :PBMO3, :PBCLE, :PBANT
, :PBCTD, :PBCTU, :PBAPO, :PBDUR, :PBREL
, :PBRLD, :PBATT, :PBRMP, :PBVRF, :PBOLV
, :PBFOA, :PBFOM, :PBCOU, :PBFOE, :PBDEV
, :PBRGT, :PBTCO, :PBNAT, :PBDST, :PBLIE
, :PBGES, :PBSOU, :PBORI, :PBMAI, :PBEFA
, :PBEFM, :PBEFJ, :PBOFF, :PBOFV, :PBIPA
, :PBALA, :PBECM, :PBECJ, :PBPCV, :PBCTA
, :PBNRQ, :PBNPL, :PBPER, :PBAVN, :PBAVC
, :PBAVA, :PBAVM, :PBAVJ, :PBRSC, :PBRSA
, :PBRSM, :PBRSJ, :PBMER, :PBETA, :PBSIT
, :PBSTA, :PBSTM, :PBSTJ, :PBSTQ, :PBEDT
, :PBTAC, :PBTAA, :PBTAM, :PBTAJ, :PBCRU
, :PBCRA, :PBCRM, :PBCRJ, :PBMJU, :PBMJA
, :PBMJM, :PBMJJ, :PBDEU, :PBDEA, :PBDEM
, :PBDEJ, :PBCTP, :PBEFH, :PBFEA, :PBFEM
, :PBFEJ, :PBFEH, :PBSTP, :PBNVA, :PBFEC
, :PBPOR, :PBCON, :PBTTR, :PBAVK, :PBADH
, :PBSTF, :PBPIM, :PBIN5, :PBORK)";
            const string select_SelectCurrent=@"SELECT
PBTYP, PBIPB, PBALX, PBTBR, PBIAS
, PBREF, PBBRA, PBSBR, PBCAT, PBICT
, PBTIL, PBINL, PBOCT, PBAD1, PBAD2
, PBDEP, PBCPO, PBVIL, PBPAY, PBSAA
, PBSAM, PBSAJ, PBSAH, PBAPR, PBAPP
, PBMO1, PBMO2, PBMO3, PBCLE, PBANT
, PBCTD, PBCTU, PBAPO, PBDUR, PBREL
, PBRLD, PBATT, PBRMP, PBVRF, PBOLV
, PBFOA, PBFOM, PBCOU, PBFOE, PBDEV
, PBRGT, PBTCO, PBNAT, PBDST, PBLIE
, PBGES, PBSOU, PBORI, PBMAI, PBEFA
, PBEFM, PBEFJ, PBOFF, PBOFV, PBIPA
, PBALA, PBECM, PBECJ, PBPCV, PBCTA
, PBNRQ, PBNPL, PBPER, PBAVN, PBAVC
, PBAVA, PBAVM, PBAVJ, PBRSC, PBRSA
, PBRSM, PBRSJ, PBMER, PBETA, PBSIT
, PBSTA, PBSTM, PBSTJ, PBSTQ, PBEDT
, PBTAC, PBTAA, PBTAM, PBTAJ, PBCRU
, PBCRA, PBCRM, PBCRJ, PBMJU, PBMJA
, PBMJM, PBMJJ, PBDEU, PBDEA, PBDEM
, PBDEJ, PBCTP, PBEFH, PBFEA, PBFEM
, PBFEJ, PBFEH, PBSTP, PBNVA, PBFEC
, PBPOR, PBCON, PBTTR, PBAVK, PBADH
, PBSTF, PBPIM, PBIN5, PBORK FROM YPOBASE
WHERE PBIPB = :parPBIPB
and PBALX = :parPBALX
";
            const string select_GetId=@"SELECT
PBIPB AS PBIPB, PBALX AS PBALX, PBAVN AS PBAVN, PBTYP AS PBTYP FROM YPOBASE
WHERE PBIPB = :ipb
and PBALX = :alx
FETCH FIRST 1 ROWS ONLY
";
            const string select_SelectByIpb=@"SELECT
PBTYP, PBIPB, PBALX, PBTBR, PBIAS
, PBREF, PBBRA, PBSBR, PBCAT, PBICT
, PBTIL, PBINL, PBOCT, PBAD1, PBAD2
, PBDEP, PBCPO, PBVIL, PBPAY, PBSAA
, PBSAM, PBSAJ, PBSAH, PBAPR, PBAPP
, PBMO1, PBMO2, PBMO3, PBCLE, PBANT
, PBCTD, PBCTU, PBAPO, PBDUR, PBREL
, PBRLD, PBATT, PBRMP, PBVRF, PBOLV
, PBFOA, PBFOM, PBCOU, PBFOE, PBDEV
, PBRGT, PBTCO, PBNAT, PBDST, PBLIE
, PBGES, PBSOU, PBORI, PBMAI, PBEFA
, PBEFM, PBEFJ, PBOFF, PBOFV, PBIPA
, PBALA, PBECM, PBECJ, PBPCV, PBCTA
, PBNRQ, PBNPL, PBPER, PBAVN, PBAVC
, PBAVA, PBAVM, PBAVJ, PBRSC, PBRSA
, PBRSM, PBRSJ, PBMER, PBETA, PBSIT
, PBSTA, PBSTM, PBSTJ, PBSTQ, PBEDT
, PBTAC, PBTAA, PBTAM, PBTAJ, PBCRU
, PBCRA, PBCRM, PBCRJ, PBMJU, PBMJA
, PBMJM, PBMJJ, PBDEU, PBDEA, PBDEM
, PBDEJ, PBCTP, PBEFH, PBFEA, PBFEM
, PBFEJ, PBFEH, PBSTP, PBNVA, PBFEC
, PBPOR, PBCON, PBTTR, PBAVK, PBADH
, PBSTF, PBPIM, PBIN5, PBORK FROM YPOBASE
WHERE PBIPB = :parPBIPB
";
            #endregion

            public YpoBaseRepository (IDbConnection connection, IdentifierGenerator idGenerator) : base (connection) {
                this.idGenerator = idGenerator;
            }
 
            public YpoBase Get(string typeAffaire, string numeroAffaire, int version){
                return connection.Query<YpoBase>(select, new {typeAffaire, numeroAffaire, version}).SingleOrDefault();
            }


            public void Insert(YpoBase value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PBTYP",value.Pbtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBIPB",value.Pbipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PBALX",value.Pbalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBTBR",value.Pbtbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBIAS",value.Pbias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBREF",value.Pbref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("PBBRA",value.Pbbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSBR",value.Pbsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBCAT",value.Pbcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBICT",value.Pbict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBTIL",value.Pbtil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBINL",value.Pbinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBOCT",value.Pboct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PBAD1",value.Pbad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("PBAD2",value.Pbad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("PBDEP",value.Pbdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCPO",value.Pbcpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBVIL",value.Pbvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("PBPAY",value.Pbpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBSAA",value.Pbsaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBSAM",value.Pbsam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSAJ",value.Pbsaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSAH",value.Pbsah, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBAPR",value.Pbapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("PBAPP",value.Pbapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PBMO1",value.Pbmo1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBMO2",value.Pbmo2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBMO3",value.Pbmo3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBCLE",value.Pbcle??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBANT",value.Pbant??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBCTD",value.Pbctd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCTU",value.Pbctu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBAPO",value.Pbapo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBDUR",value.Pbdur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBREL",value.Pbrel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBRLD",value.Pbrld, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBATT",value.Pbatt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRMP",value.Pbrmp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBVRF",value.Pbvrf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBOLV",value.Pbolv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBFOA",value.Pbfoa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFOM",value.Pbfom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBCOU",value.Pbcou, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBFOE",value.Pbfoe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBDEV",value.Pbdev??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBRGT",value.Pbrgt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTCO",value.Pbtco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBNAT",value.Pbnat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBDST",value.Pbdst??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBLIE",value.Pblie??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBGES",value.Pbges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBSOU",value.Pbsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBORI",value.Pbori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBMAI",value.Pbmai, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBEFA",value.Pbefa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBEFM",value.Pbefm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBEFJ",value.Pbefj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBOFF",value.Pboff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PBOFV",value.Pbofv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBIPA",value.Pbipa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PBALA",value.Pbala, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBECM",value.Pbecm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBECJ",value.Pbecj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBPCV",value.Pbpcv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBCTA",value.Pbcta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBNRQ",value.Pbnrq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBNPL",value.Pbnpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBPER",value.Pbper??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBAVN",value.Pbavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBAVC",value.Pbavc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBAVA",value.Pbava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBAVM",value.Pbavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBAVJ",value.Pbavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRSC",value.Pbrsc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRSA",value.Pbrsa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBRSM",value.Pbrsm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRSJ",value.Pbrsj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBMER",value.Pbmer??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBETA",value.Pbeta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBSIT",value.Pbsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBSTA",value.Pbsta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBSTM",value.Pbstm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSTJ",value.Pbstj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSTQ",value.Pbstq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBEDT",value.Pbedt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTAC",value.Pbtac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTAA",value.Pbtaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBTAM",value.Pbtam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBTAJ",value.Pbtaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCRU",value.Pbcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBCRA",value.Pbcra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBCRM",value.Pbcrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCRJ",value.Pbcrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBMJU",value.Pbmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBMJA",value.Pbmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBMJM",value.Pbmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBMJJ",value.Pbmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBDEU",value.Pbdeu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBDEA",value.Pbdea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBDEM",value.Pbdem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBDEJ",value.Pbdej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCTP",value.Pbctp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBEFH",value.Pbefh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBFEA",value.Pbfea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBFEM",value.Pbfem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFEJ",value.Pbfej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFEH",value.Pbfeh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBSTP",value.Pbstp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBNVA",value.Pbnva??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFEC",value.Pbfec??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBPOR",value.Pbpor??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBCON",value.Pbcon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTTR",value.Pbttr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBAVK",value.Pbavk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBADH",value.Pbadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBSTF",value.Pbstf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBPIM",value.Pbpim??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBIN5",value.Pbin5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBORK",value.Pbork??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);

                    connection.EnsureOpened().Execute(insert,parameters);
            }
            public void Delete(YpoBase value){
                    var parameters = new DynamicParameters();
                    parameters.Add("typeAffaire",value.Pbtyp);
                    parameters.Add("numeroAffaire",value.Pbipb);
                    parameters.Add("version",value.Pbalx);
                        connection.EnsureOpened().Execute(delete, parameters);
            }

            public void Update(YpoBase value){
                    var parameters = new DynamicParameters();
                    parameters.Add("PBTYP",value.Pbtyp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBIPB",value.Pbipb??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PBALX",value.Pbalx, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBTBR",value.Pbtbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBIAS",value.Pbias, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBREF",value.Pbref??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:40, scale: 0);
                    parameters.Add("PBBRA",value.Pbbra??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSBR",value.Pbsbr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBCAT",value.Pbcat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBICT",value.Pbict, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBTIL",value.Pbtil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBINL",value.Pbinl, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBOCT",value.Pboct??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:25, scale: 0);
                    parameters.Add("PBAD1",value.Pbad1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("PBAD2",value.Pbad2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:32, scale: 0);
                    parameters.Add("PBDEP",value.Pbdep??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCPO",value.Pbcpo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBVIL",value.Pbvil??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:26, scale: 0);
                    parameters.Add("PBPAY",value.Pbpay??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBSAA",value.Pbsaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBSAM",value.Pbsam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSAJ",value.Pbsaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSAH",value.Pbsah, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBAPR",value.Pbapr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:6, scale: 0);
                    parameters.Add("PBAPP",value.Pbapp, dbType:DbType.Decimal, direction:ParameterDirection.Input, size:5, scale: 2);
                    parameters.Add("PBMO1",value.Pbmo1??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBMO2",value.Pbmo2??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBMO3",value.Pbmo3??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBCLE",value.Pbcle??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBANT",value.Pbant??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBCTD",value.Pbctd, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCTU",value.Pbctu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBAPO",value.Pbapo??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBDUR",value.Pbdur, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBREL",value.Pbrel??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBRLD",value.Pbrld, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBATT",value.Pbatt, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRMP",value.Pbrmp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBVRF",value.Pbvrf, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBOLV",value.Pbolv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBFOA",value.Pbfoa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFOM",value.Pbfom??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBCOU",value.Pbcou, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBFOE",value.Pbfoe??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBDEV",value.Pbdev??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBRGT",value.Pbrgt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTCO",value.Pbtco??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBNAT",value.Pbnat??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBDST",value.Pbdst??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBLIE",value.Pblie??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBGES",value.Pbges??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBSOU",value.Pbsou??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBORI",value.Pbori??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBMAI",value.Pbmai, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBEFA",value.Pbefa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBEFM",value.Pbefm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBEFJ",value.Pbefj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBOFF",value.Pboff??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PBOFV",value.Pbofv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBIPA",value.Pbipa??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:9, scale: 0);
                    parameters.Add("PBALA",value.Pbala, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBECM",value.Pbecm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBECJ",value.Pbecj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBPCV",value.Pbpcv, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBCTA",value.Pbcta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBNRQ",value.Pbnrq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBNPL",value.Pbnpl??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBPER",value.Pbper??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBAVN",value.Pbavn, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBAVC",value.Pbavc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBAVA",value.Pbava, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBAVM",value.Pbavm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBAVJ",value.Pbavj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRSC",value.Pbrsc??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRSA",value.Pbrsa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBRSM",value.Pbrsm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBRSJ",value.Pbrsj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBMER",value.Pbmer??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBETA",value.Pbeta??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBSIT",value.Pbsit??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBSTA",value.Pbsta, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBSTM",value.Pbstm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSTJ",value.Pbstj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBSTQ",value.Pbstq??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBEDT",value.Pbedt??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTAC",value.Pbtac??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTAA",value.Pbtaa, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBTAM",value.Pbtam, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBTAJ",value.Pbtaj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCRU",value.Pbcru??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBCRA",value.Pbcra, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBCRM",value.Pbcrm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCRJ",value.Pbcrj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBMJU",value.Pbmju??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBMJA",value.Pbmja, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBMJM",value.Pbmjm, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBMJJ",value.Pbmjj, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBDEU",value.Pbdeu??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:10, scale: 0);
                    parameters.Add("PBDEA",value.Pbdea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBDEM",value.Pbdem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBDEJ",value.Pbdej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBCTP",value.Pbctp, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBEFH",value.Pbefh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBFEA",value.Pbfea, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBFEM",value.Pbfem, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFEJ",value.Pbfej, dbType:DbType.Int32, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFEH",value.Pbfeh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:4, scale: 0);
                    parameters.Add("PBSTP",value.Pbstp??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBNVA",value.Pbnva??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBFEC",value.Pbfec??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBPOR",value.Pbpor??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBCON",value.Pbcon??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:1, scale: 0);
                    parameters.Add("PBTTR",value.Pbttr??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBAVK",value.Pbavk, dbType:DbType.Int32, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBADH",value.Pbadh, dbType:DbType.Int32, direction:ParameterDirection.Input, size:7, scale: 0);
                    parameters.Add("PBSTF",value.Pbstf??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("PBPIM",value.Pbpim??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:2, scale: 0);
                    parameters.Add("PBIN5",value.Pbin5, dbType:DbType.Int32, direction:ParameterDirection.Input, size:5, scale: 0);
                    parameters.Add("PBORK",value.Pbork??String.Empty, dbType:DbType.AnsiStringFixedLength, direction:ParameterDirection.Input, size:3, scale: 0);
                    parameters.Add("typeAffaire",value.Pbtyp);
                    parameters.Add("numeroAffaire",value.Pbipb);
                    parameters.Add("version",value.Pbalx);
                    
                    connection.EnsureOpened().Execute(update, parameters);
            }
    
            public IEnumerable<YpoBase> SelectCurrent(string parPBIPB, int parPBALX){
                    return connection.EnsureOpened().Query<YpoBase>(select_SelectCurrent, new {parPBIPB, parPBALX}).ToList();
            }
            public IEnumerable<YpoBase> GetId(string ipb, int alx){
                    return connection.EnsureOpened().Query<YpoBase>(select_GetId, new {ipb, alx}).ToList();
            }
            public IEnumerable<YpoBase> SelectByIpb(string parPBIPB){
                    return connection.EnsureOpened().Query<YpoBase>(select_SelectByIpb, new {parPBIPB}).ToList();
            }
    }
}
