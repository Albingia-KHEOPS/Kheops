using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using System;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess {
    public class TraceRepository : RepositoryBase {

        internal static readonly string AddTraceAvn = @"INSERT INTO KPAVTRC 
( KHOID , KHOTYP , KHOIPB , KHOALX , KHOPERI , KHORSQ , KHOOBJ , KHOFOR , KHOOPT , KHOETAPE , KHOCHAM , KHOACT , KHOANV , KHONVV , KHOAVO , KHOOEF , KHOCRU , KHOCRD , KHOCRH ) 
VALUES ( :khoid, :khotyp, :khoipb, :khoalx, :khoperi, :khorsq, :khoobj, :khofor, :khoopt, :khoetape, :khocham, :khoact, :khoanv, :khonvv, :khoavo, :khooef, :khocru, :khocrd, :khocrh ) ;";
        internal static readonly string AddTraceYpo = @"INSERT INTO YPOTRAC 
( PYTYP , PYIPB , PYALX , PYAVN , PYTTR , PYVAG , PYTRA , PYTRM , PYTRJ , PYTRH , PYLIB , PYINF , 
PYSDA , PYSDM , PYSDJ , PYSFA , PYSFM , PYSFJ , PYMJU , PYMJA , PYMJM , PYMJJ , PYMJH ) 
VALUES 
( :type, :codeAffaire, :version, :codeAvn, :ttr, :acteGes, :dateAnnee, :dateMois, :dateJour, :dateHeure, :libelle, 'I', 
0, 0, 0, 0, 0, 0, :user, :dateAnneeMAJ, :dateMoisMAJ, :dateJourMAJ, :dateHeureMAJ ) ;";
        internal static readonly string PurgeTraceCC = "DELETE FROM KPAVTRC WHERE KHOIPB = :codeAffaire AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'CC' ;";
        internal static readonly string CountPrimeEmise = @"SELECT COUNT(*) 
FROM KPAVTRC 
INNER JOIN YPOTRAC ON ( PYIPB , PYALX , PYTYP ) = ( KHOIPB , KHOALX , KHOTYP ) 
AND PYVAG = 'V' AND KHOPERI = 'COT' AND KHOOEF = :OEF 
AND ( KHOIPB , KHOALX , KHOTYP ) = ( :IPB , :ALX , :TYP ) ;";

        public TraceRepository(IDbConnection connection) : base(connection) { }

        public static bool ObtenirTraceByEtape(string codeOffre, string version, string type, string etape, string perimetre)
        {
            EacParameter[] param = new EacParameter[5];

            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("etape", DbType.AnsiStringFixedLength);
            param[3].Value = etape;
            param[4] = new EacParameter("perimetre", DbType.AnsiStringFixedLength);
            param[4].Value = perimetre;

            string sql = @"SELECT COUNT(*) NBLIGN FROM KPCTRLE WHERE 
KEVIPB = :codeOffre
AND KEVALX = :version
AND KEVTYP = :type
AND KEVETAPE = :etape
AND KEVPERI = :perimetre";

            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne > 0 ? true : false;
        }

        public void CreateTraceYpo(Folder folder, string libelle, string user, DateTime now, string ttr, string acteGestion) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = connection,
                Parameters = new[] {
                    new EacParameter("type", folder.Type),
                    new EacParameter("codeAffaire", folder.CodeOffre.PadLeft(9, ' ')),
                    new EacParameter("version", DbType.Int32) { Value = folder.Version },
                    new EacParameter("codeAvn", DbType.Int32) { Value = folder.NumeroAvenant },
                    new EacParameter("ttr", ttr),
                    new EacParameter("acteGes", acteGestion),
                    new EacParameter("dateAnnee", DbType.Int32) { Value = now.Year },
                    new EacParameter("dateMois", DbType.Int32) { Value = now.Month },
                    new EacParameter("dateJour", DbType.Int32) { Value = now.Day },
                    new EacParameter("dateHeure", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(now)) },
                    new EacParameter("libelle", libelle),
                    new EacParameter("user", user),
                    new EacParameter("dateAnneeMAJ", DbType.Int32) { Value = now.Year },
                    new EacParameter("dateMoisMAJ", DbType.Int32) { Value = now.Month },
                    new EacParameter("dateJourMAJ", DbType.Int32) { Value = now.Day },
                    new EacParameter("dateHeureMAJ", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(now)) }
                },
                SqlText = AddTraceYpo
            }) {
                options.Exec();
            }
        }

        public void CreateTraceAvn(Folder folder, string user, DateTime now, string acte) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = connection,
                Parameters = new[] {
                    new EacParameter("khoid", DbType.Int32) { Value = CommonRepository.GetAS400Id("KHOID", connection) },
                    new EacParameter("khotyp", folder.Type),
                    new EacParameter("khoipb", folder.CodeOffre.PadLeft(9, ' ')),
                    new EacParameter("khoalx", DbType.Int32) { Value = folder.Version },
                    new EacParameter("khoperi", "COT"),
                    new EacParameter("khorsq", DbType.Int32) { Value = 0 },
                    new EacParameter("khoobj", DbType.Int32) { Value = 0 },
                    new EacParameter("khofor", DbType.Int32) { Value = 0 },
                    new EacParameter("khoopt", DbType.Int32) { Value = 0 },
                    new EacParameter("khoetape", "RGU"),
                    new EacParameter("khocham", string.Empty),
                    new EacParameter("khoact", "M"),
                    new EacParameter("khoanv", string.Empty),
                    new EacParameter("khonvv", string.Empty),
                    new EacParameter("khoavo", "N"),
                    new EacParameter("khooef", "CC"),
                    new EacParameter("khocru", user),
                    new EacParameter("khocrd", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(now) },
                    new EacParameter("khocrh", DbType.Int32) { Value = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(now)) }
                },
                SqlText = AddTraceAvn
            }) {
                options.Exec();
            }
        }

        public void DeleteTraceCC(Folder folder) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = connection,
                SqlText = PurgeTraceCC,
                Parameters = MakeParams(PurgeTraceCC, folder.CodeOffre.ToIPB(), folder.Version, folder.Type)
            }) {
                options.Exec();
            }
        }

        public int GetNbTracesPrimeEmise(Folder folder, string operation) {
            using (var options = new DbCountOptions(this.connection == null) {
                DbConnection = connection,
                SqlText = CountPrimeEmise
            }) {
                options.BuildParameters(operation, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                options.PerformCount();
                return options.Count;
            }
        }
    }
}
