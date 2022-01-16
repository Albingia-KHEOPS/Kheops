using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.VerouillageOffres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;

namespace OP.DataAccess
{
    public class VerouillageOffresRepository
    {
        public static void AjouterOffreVerouille(string sevice, string typ, string ipb, int alx, int avn,
          int kavsua, int kavnum, string kavsbr, string kavactg, string kavact, string kavverr, string user, string kavlib)
        {
            int guid = CommonRepository.GetAS400Id("KAVID");
            if (guid > 0)
            {
                EacParameter[] param = new EacParameter[16];
                string sql = @"INSERT INTO KVERROU
                        (KAVID, KAVSERV,KAVTYP,KAVIPB,KAVALX,KAVAVN,KAVSUA,KAVNUM,KAVSBR,KAVACTG,KAVACT,KAVVERR,KAVCRU,KAVCRD,KAVCRH,KAVLIB)
                        VALUES
                        (:KAVID, :KAVSERV,:KAVTYP,:KAVIPB,:KAVALX,:KAVAVN,:KAVSUA,:KAVNUM,:KAVSBR,:KAVACTG,:KAVACT,:KAVVERR,:KAVCRU,:KAVCRD,:KAVCRH,:KAVLIB)";
                param[0] = new EacParameter("KAHID", DbType.Int32);
                param[0].Value = guid;
                param[1] = new EacParameter("KAVSERV", DbType.AnsiStringFixedLength);
                param[1].Value = sevice;
                param[2] = new EacParameter("KAVTYP", DbType.AnsiStringFixedLength);
                param[2].Value = typ;
                param[3] = new EacParameter("KAVIPB", DbType.AnsiStringFixedLength);
                param[3].Value = ipb.PadLeft(9, ' ');
                param[4] = new EacParameter("KAVALX", DbType.Int32);
                param[4].Value = alx;
                param[5] = new EacParameter("KAVAVN", DbType.Int32);
                param[5].Value = avn;
                param[6] = new EacParameter("KAVSUA", DbType.Int32);
                param[6].Value = kavsua;
                param[7] = new EacParameter("kavnum", DbType.Int32);
                param[7].Value = kavnum;
                param[8] = new EacParameter("KAVSBR", DbType.AnsiStringFixedLength);
                param[8].Value = kavsbr;
                param[9] = new EacParameter("KAVACTG", DbType.AnsiStringFixedLength);
                param[9].Value = kavactg;
                param[10] = new EacParameter("KAVACT", DbType.AnsiStringFixedLength);
                param[10].Value = kavact;
                param[11] = new EacParameter("KAVVERR", DbType.AnsiStringFixedLength);
                param[11].Value = kavverr;
                param[12] = new EacParameter("KAVCRU", DbType.AnsiStringFixedLength);
                param[12].Value = user;


                int? dateCreation = AlbConvert.ConvertDateToInt(DateTime.Now);
                param[13] = new EacParameter("KAVCRD", DbType.Int32);
                param[13].Value = dateCreation;
                int? registredTime = AlbConvert.ConvertTimeToInt(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
                param[14] = new EacParameter("KAVCRH", DbType.Int32);
                param[14].Value = registredTime.Value;

                param[15] = new EacParameter("KAVLIB", DbType.AnsiStringFixedLength);
                param[15].Value = kavlib;
                
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else
            {
                throw new Exception("Erreur lors de l'insertion du paramètre.");
            }
        }

        // TODO : Changement des nom de paramètres (Norme non respectées)
        public static List<OffreVerouilleeDto> GetOffresVerouillees(bool typeOffre_O, bool typeOffre_P, string numOffre, string version, string numAvn, string utilisateur, string dateVerouillageDebut, string dateVerouillageFin)
        {
            string typeOffre = string.Empty;
            if (typeOffre_O && typeOffre_P)
                typeOffre = "(KAVTYP='O' OR KAVTYP='P')";
            else if (typeOffre_O)
                typeOffre = "KAVTYP='O'";
            else if (typeOffre_P)
                typeOffre = "KAVTYP='P'";

            string sql = @"SELECT 
                           KAVID,
                           KAVTYP,
                           KAVIPB,
                           KAVALX,
                           KAVCRU,
                           KAVCRD,
                           KAVCRH,
                           KAVAVN
                        FROM KVERROU ";
            if (!string.IsNullOrEmpty(typeOffre) || !string.IsNullOrEmpty(numOffre) ||
                !string.IsNullOrEmpty(version) || !string.IsNullOrEmpty(utilisateur) || !string.IsNullOrEmpty(dateVerouillageDebut) || !string.IsNullOrEmpty(dateVerouillageFin))
                sql += "WHERE";
            if (!string.IsNullOrEmpty(typeOffre))
                sql += " {0} ";

            if (!string.IsNullOrEmpty(typeOffre) && !string.IsNullOrEmpty(numOffre))
                sql += "AND";
            if (!string.IsNullOrEmpty(numOffre))
                sql += " KAVIPB='{1}' ";
            if ((!string.IsNullOrEmpty(typeOffre) || !string.IsNullOrEmpty(numOffre)) && !string.IsNullOrEmpty(version))
                sql += "AND";
            if (!string.IsNullOrEmpty(version))
                sql += " KAVALX='{2}' ";
            if (!string.IsNullOrEmpty(numAvn))
                sql += " KAVAVN='{3}' ";
            if ((!string.IsNullOrEmpty(typeOffre) || !string.IsNullOrEmpty(numOffre) ||
                !string.IsNullOrEmpty(version)) && !string.IsNullOrEmpty(utilisateur))
                sql += "AND";
            if (!string.IsNullOrEmpty(utilisateur))
                sql += " Lower(KAVCRU) LIKE Lower('%{4}%') ";
            if ((!string.IsNullOrEmpty(typeOffre) || !string.IsNullOrEmpty(numOffre) ||
                !string.IsNullOrEmpty(version) || !string.IsNullOrEmpty(utilisateur)) && !string.IsNullOrEmpty(dateVerouillageDebut))
                sql += "AND";
            if (!string.IsNullOrEmpty(dateVerouillageDebut))
                sql += " KAVCRD>='{5}' ";
            if ((!string.IsNullOrEmpty(typeOffre) || !string.IsNullOrEmpty(numOffre) ||
               !string.IsNullOrEmpty(version) || !string.IsNullOrEmpty(utilisateur) || !string.IsNullOrEmpty(dateVerouillageDebut)) && !string.IsNullOrEmpty(dateVerouillageFin))
                sql += "AND";
            if (!string.IsNullOrEmpty(dateVerouillageFin))
                sql += " KAVCRD<='{6}' ";


            int numVersion = 0;
            int dateVerouDebut = 0;
            int dateVerouFin = 0;

            version = !string.IsNullOrEmpty(version) && int.TryParse(version, out numVersion) ? numVersion.ToString() : string.Empty;
            dateVerouillageDebut = !string.IsNullOrEmpty(dateVerouillageDebut) && int.TryParse(dateVerouillageDebut, out dateVerouDebut) ? dateVerouDebut.ToString() : string.Empty;
            dateVerouillageFin = !string.IsNullOrEmpty(dateVerouillageFin) && int.TryParse(dateVerouillageFin, out dateVerouFin) ? dateVerouFin.ToString() : string.Empty;

            return DbBase.Settings.ExecuteList<OffreVerouilleeDto>(CommandType.Text, string.Format(sql, typeOffre, numOffre.Trim(), version, numAvn, utilisateur, dateVerouillageDebut, dateVerouillageFin));

        }

        public static List<OffreVerouilleeDto> GetALLOffresVerouillees()
        {
            string sql = @"SELECT 
                           KAVID,
                           KAVTYP,
                           KAVIPB,
                           KAVALX,
                           KAVCRU,
                           KAVCRD,
                           KAVCRH,
                           KAVAVN,
                        FROM KVERROU ";
            return DbBase.Settings.ExecuteList<OffreVerouilleeDto>(CommandType.Text, sql);
        }

        public static bool
          IsOffreVeruouille(string codeOffre, string version, string type, string numAvn)
        {
            string sql = string.Format("SELECT COUNT(*) NBLIGN FROM KVERROU WHERE KAVIPB='{0}'" +
                                       " and KAVALX={1} AND KAVTYP='{2}' AND KAVAVN={3}", codeOffre.Trim().PadLeft(9, ' '), version, type, numAvn);
            return CommonRepository.ExistRow(sql);

        }

        public static string GetUserVerrou(string codeOffre, string version, string type, string numAvn)
        {
            var sql = string.Format("SELECT KAVCRU STRRETURNCOL FROM KVERROU WHERE KAVIPB='{0}'" +
                                       " and KAVALX={1} AND KAVTYP='{2}' AND KAVAVN={3}", codeOffre.Trim().PadLeft(9, ' '), version, type, numAvn);
            return CommonRepository.GetStrValue(sql);

        }

        public static void SupprimerOffreVerouillee(string numOffre, string version, string type, string numAvn, string user, string acteGestion, bool isAlimStat, bool isModifHorsAvn, bool isAnnul = false)
        {
            if (string.IsNullOrEmpty(numOffre) || string.IsNullOrEmpty(version))
                return;

            var param = new EacParameter[3];
            param[0] = new EacParameter("numOffre", DbType.AnsiStringFixedLength);
            param[0].Value = numOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("numAvn", DbType.AnsiStringFixedLength);
            param[2].Value = numAvn;
            var sql = @"DELETE
                        FROM KVERROU 
                         WHERE KAVIPB=:numOffre AND KAVALX=:version AND KAVAVN=:numAvn";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (!isAnnul)
            {
                var param2 = new EacParameter[2];
                param2[0] = new EacParameter("numOffre", DbType.AnsiStringFixedLength);
                param2[0].Value = numOffre.PadLeft(9, ' ');
                param2[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                param2[1].Value = version;
                sql = @"UPDATE KPCTRLE SET KEVTAG = 'N' WHERE KEVIPB = :numOffre and KEVALX = :version";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param2);


                CommonRepository.AlimStatistiques(numOffre, version, user, acteGestion, numAvn == "0" & type == AlbConstantesMetiers.TYPE_CONTRAT ? "N" : "X");


                if (type == AlbConstantesMetiers.TYPE_OFFRE)
                    CommonRepository.CalculYAPRMANAS400(numOffre, version, type, numAvn, acteGestion, user);
            }
        }

        /// <summary>
        /// B3101
        /// Enregistrement du trace contrat en modif hors avenant
        /// B3203
        /// Enregistrement du trace regul
        /// </summary>
        /// <param name="numOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="numAvn"></param>
        /// <param name="user"></param>
        public static void TraceContratInfoModifHorsAvn(string numOffre, string version, string type, string numAvn, string user , string codeRisque = null , bool regulTrace = false)
        {

            var now = DateTime.Now;
            var date = AlbConvert.ConvertDateToInt(now).ToString();
            var time = AlbConvert.ConvertTimeMinuteToInt(AlbConvert.GetTimeFromDate(now));
            var parameterNumber = regulTrace ? 8:7;
            var param = new EacParameter[parameterNumber];
            param[0] = new EacParameter("user", DbType.AnsiStringFixedLength)
            {
                Value = user
            };
            param[1] = new EacParameter("date", DbType.AnsiStringFixedLength)
            {
                Value = date
            };
            param[2] = new EacParameter("time", DbType.Int32)
            {
                Value = time
            };
            if (regulTrace)
            {
                param[parameterNumber-5] = new EacParameter("codeRisque", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 1
                };
            }
           
            param[parameterNumber-4] = new EacParameter("type", DbType.AnsiStringFixedLength)
            {
                Value = type
            };
            param[parameterNumber-3] = new EacParameter("numOffre", DbType.AnsiStringFixedLength)
            {
                Value = numOffre.PadLeft(9, ' ')
            };
            param[parameterNumber-2] = new EacParameter("version", DbType.AnsiStringFixedLength)
            {
                Value = version
            };
            param[parameterNumber-1] = new EacParameter("numAvn", DbType.AnsiStringFixedLength)
            {
                Value = numAvn
            };
           

            var sql = @"INSERT INTO KPHAVH (KIGTYP,KIGIPB,KIGALX,KIGAVN,KIGPERI,KIGRSQ,KIGOBJ,KIGFOR,
	                                   KIGOPT,KIGCRU,KIGCRD,KIGCRH,KIGFEA,KIGFEM,KIGFEJ,
	                                   KIGCTD,KIGCTU,KIGRUL,KIGRUT)

                                        SELECT PBTYP,PBIPB,PBALX,PBAVN,'GEN',0,0,0,0, CAST(:user AS CHAR(10)) , CAST(:date AS INTEGER) ,CAST(:time AS INTEGER),PBFEA,PBFEM,PBFEJ,PBCTD,PBCTU,'',''
                                        FROM YPOBASE 
                                        WHERE PBTYP = :type AND  PBIPB = :numOffre AND PBALX = :version  AND PBAVN = :numAvn";

                                       
            if (regulTrace)
            {
                sql = @"INSERT INTO KPHAVH (KIGTYP,KIGIPB,KIGALX,KIGAVN,KIGPERI,KIGRSQ,KIGOBJ,KIGFOR,
	                                   KIGOPT,KIGCRU,KIGCRD,KIGCRH,KIGFEA,KIGFEM,KIGFEJ,
	                                   KIGCTD,KIGCTU,KIGRUL,KIGRUT)

                                        SELECT PBTYP,PBIPB,PBALX,PBAVN,'RSQ',JERSQ,0,0,0, CAST(:user AS CHAR(10)) ,CAST(:date AS INTEGER) ,CAST(:time AS INTEGER),0,0,0,0,'',JERUL,JERUT
                                        FROM YPOBASE 
	                                            LEFT JOIN YPRTRSQ ON JEIPB = PBIPB AND JEALX = PBALX  AND JERSQ = :codeRisque
                                        WHERE PBTYP = :type AND  PBIPB = :numOffre AND PBALX = :version  AND PBAVN = :numAvn";

                                       
            }
            

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);


        }

        /// <summary>
        /// B2568
        /// Trace résilisation en modif hors avenant
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="numAvn"></param>
        /// <param name="user"></param>
        /// <param name="TraceType"></param>
        public static void TraceResiliation(string contratId, string version, string type, string numAvn, string user, ResiliationTraceType TraceType)
        {
            try
            {
                var message = TraceType == ResiliationTraceType.Modification ? "Modif hors avt - Résil" : "Modif hors avt - Annul résil";
                var treatmentType = TraceType == ResiliationTraceType.Modification ? AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN : AlbConstantesMetiers.TRAITEMENT_RESILHORSAVN_ANNUL;
                CommonRepository.AjouterActeGestion(contratId, version, type, string.IsNullOrEmpty(numAvn) ? 0 : Convert.ToInt32(numAvn),
                    AlbConstantesMetiers.ACTEGESTION_GESTION, treatmentType, message, user);

            }
            catch (Exception)
            {

                throw;
            }
           

        }

    }
}
