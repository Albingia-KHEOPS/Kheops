using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess.Data;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess {
    public partial class EngagementRepository : RepositoryBase {
        internal static readonly string Insert = @"INSERT INTO KPENG 
( KDODATD , KDODATF , KDOCRU , KDOCRD , KDOMAJU , KDOMAJD , KDOACT , KDOECO , KDOID ) 
VALUES ( :DEB , :FIN , :CRU , :CRD , :CRU , :CRD , :ACT , :ECO , :ID ) ;";
        internal static readonly string Update = @"UPDATE KPENG SET KDODATD = :DEB , KDODATF = :FIN , KDOMAJU = :MAJU , KDOMAJD = :MAJD , KDOACT = :ACT , KDOECO = :ECO WHERE KDOID = :ID ;";
        internal static readonly string InsertFamille = @"INSERT INTO KPENGFAM 
( KDPID , KDPKDOID , KDPFAM , KDPENA , KDPCRU , KDPCRD , KDPMAJU , KDPMAJD ) 
VALUES ( :ID , :KDOID , :FAM , :ENA , :CRU , :CRD , :CRU , :CRD ) ;";
        internal static readonly string UpdateFamille = "UPDATE KPENGFAM SET KDPENA = :ENA , KDPMAJU = :MAJU , KDPMAJD = :MAJD WHERE KDPKDOID = :ID AND KDPFAM  = :FAM ;";
        internal static readonly string SelectNumId = "SELECT PJCNX AS NUMERO , PJIDE AS ID FROM YPOCONX WHERE PJIPB = :IPB AND PJALX = :ALX AND PJTYP = :TYP AND PJCCX = '20' ;";
        internal static readonly string InsertConnexite = "INSERT INTO KPENGCNX ( KIEPJID , KIEORD , KIEKDOID ) VALUES ( :ID , :ORDRE , :KDOID ) ;";
        internal static readonly string SelectMaxOrderConnexite = "SELECT IFNULL ( MAX ( KIEORD ) , 0 ) FROM KPENGCNX WHERE KIEPJID = :ID ;";
        internal static readonly string DeleteEng = "DELETE FROM KPENG WHERE KDOID = :ID ;";
        internal static readonly string DeleteFamilles = "DELETE FROM KPENGFAM WHERE KDPKDOID = :ID ;";
        internal static readonly string DeleteConnexites = "DELETE FROM KPENGCNX WHERE KIEKDOID = :ID ;";
        internal static readonly string DeleteAllEng = "DELETE FROM KPENG WHERE KDOID IN ( SELECT KIEKDOID FROM KPENGCNX WHERE KIEPJID = :IDE ) ;";
        internal static readonly string DeleteAllFamilles = "DELETE FROM KPENGFAM WHERE KDPKDOID IN ( SELECT KIEKDOID FROM KPENGCNX WHERE KIEPJID = :IDE ) ;";
        internal static readonly string DeleteAllConnexites = "DELETE FROM KPENGCNX WHERE KIEPJID = :IDE ;";
        internal static readonly string SelectMontantsTraites = "SELECT KDPIPB IPB , KDPFAM CODE , KDPENA MONTANT FROM KPENGFAM WHERE  KDPIPB IN ( :IPB ) ;";
        internal static readonly string SelectPeriodes = @"SELECT 
KDOID CODEENGAGEMENT , KDODATD DATEDEBUT , KDODATF DATEFIN , KDPFAM CODETRAITE , KDPENA MONTANT , KDPENG MONTANTTOTAL , PJIPB CODEOFFRE , PJALX VERSION , PJTYP TYPE , KDOACT ACTIVE , KDOECO ENCOURS , KIEORD ORDRE
FROM YPOCONX 
INNER JOIN KPENGCNX ON PJIDE = KIEPJID 
AND PJCNX = :NUM AND PJCCX = '20' 
INNER JOIN KPENGFAM ON KIEKDOID = KDPKDOID 
INNER JOIN KPENG ON KDOIPB = '' AND KDOID = KDPKDOID ;";
        internal static readonly string SelectDetailsPeriode = "";
        internal static readonly string CountPeriodesUtilisees = @"SELECT KDOID, COUNT(*)
FROM KPENG
INNER JOIN YPORHMD ON HDSMP <> 0 AND KDOENGID <> 0 AND ( HDSMP = KDOENGID / 100000 ) AND HDSMO = MOD( KDOENGID , 100000 )
AND KDOID IN :IDS 
GROUP BY KDOID ;";

        public EngagementRepository(IDbConnection connection) : base(connection) {

        }

        public int AddOrUpdate(PeriodeConnexiteDto periode) {
            int now = DateTime.Now.ToIntYMD();
            bool isUpdating = true;
            if (periode.CodeEngagement < 1) {
                periode.CodeEngagement = GetNextId("KDOID");
                isUpdating = false;
            }

            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(isUpdating ? Update : Insert)
            }) {
                options.BuildParameters(
                    periode.DateDebut.ToIntYMD(),
                    periode.DateFin.ToIntYMD(),
                    periode.User, now,
                    periode.IsInactive ? Booleen.Non.AsCode() : Booleen.Oui.AsCode(),
                    periode.IsEnCours ? Booleen.Oui.AsCode() : Booleen.Non.AsCode(),
                    periode.CodeEngagement);
                options.Exec();
            }

            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(isUpdating ? UpdateFamille : InsertFamille)
            }) {
                foreach (var key in periode.Traites.Keys) {
                    if (isUpdating) {
                        options.BuildParameters(periode.Traites[key], periode.User, now, periode.CodeEngagement, key);
                    }
                    else {
                        options.BuildParameters(GetNextId("KDPID"), periode.CodeEngagement, key, periode.Traites[key], periode.User, now);
                    }
                    options.Exec();
                }
            }

            return (int)periode.CodeEngagement;
        }

        public void DeleteCNXOnly(int numeroEngagement) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(DeleteAllConnexites)
            }) {
                options.BuildParameters(numeroEngagement);
                options.Exec();
            }
        }

        public (int numero, int id, bool isIdNew) GetNumAndId(Folder folder, bool getNextIdeIfZero = false) {
            int num = 0;
            int id = 0;
            bool isIdNew = false;
            var result = Fetch<(int, int)>(SelectNumId, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
            if (result?.Any() ?? false) {
                (num, id) = result.First();
                if (id == 0 && getNextIdeIfZero) {
                    isIdNew = true;
                    id = GetNextId("KIEPJID");
                }
            }

            return (num, id, isIdNew);
        }

        public int GetMaxConnexiteOrder(int id) {
            using (var options = new DbSelectInt32Options(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(SelectMaxOrderConnexite)
            }) {
                options.BuildParameters(id);
                options.PerformSelect();
                return options.IntegerList?.FirstOrDefault() ?? default;
            }
        }

        public IEnumerable<PeriodeConnexiteData> GetPeriodes(int numeroConnexite) {
            return Fetch<PeriodeConnexiteData>(SelectPeriodes, numeroConnexite);
        }

        public void AddConnexite(int code, int id, int? order = null) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(InsertConnexite)
            }) {
                if (order.GetValueOrDefault() < 1) {
                    order = GetMaxConnexiteOrder(id) + 1;
                }
                options.BuildParameters(id, order.Value, code);
                options.Exec();
            }
        }

        public void Delete(int code) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(DeleteConnexites)
            }) {
                options.BuildParameters(code);
                options.Exec();
                options.SqlText = FormatQuery(DeleteFamilles);
                options.Exec();
                options.SqlText = FormatQuery(DeleteEng);
                options.Exec();
            }
        }

        public void DeleteAll(long ide) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = FormatQuery(DeleteAllFamilles)
            }) {
                options.BuildParameters(ide);
                options.Exec();
                options.SqlText = FormatQuery(DeleteAllEng);
                options.Exec();
                options.SqlText = FormatQuery(DeleteAllConnexites);
                options.Exec();
            }
        }

        public IEnumerable<(int id, int count)> GetPeriodesUsed(IEnumerable<int> ids) {
            return Fetch<(int, int)>(CountPeriodesUtilisees, ids);
        }

        public void Copy(Folder source, Folder target) {
            using (var options = new DbSPOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = "SP_COPYENGAGEMENT"
            }) {
                options.Parameters = new[] {
                    new EacParameter("P_CODEAFFAIRE", source.CodeOffre.ToIPB()),
                    new EacParameter("P_VERSION", DbType.Int32) { Value = source.Version },
                    new EacParameter("P_TYPE", source.Type),
                    new EacParameter("P_CODEAFFAIREDEST", target.CodeOffre.ToIPB()),
                    new EacParameter("P_VERSIONDEST", DbType.Int32) { Value = target.Version },
                    new EacParameter("P_TYPEDEST", target.Type)
                };
                options.ExecStoredProcedure();
            }
        }
    }
}