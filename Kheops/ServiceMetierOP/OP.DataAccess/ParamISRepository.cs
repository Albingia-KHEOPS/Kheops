using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.ParamIS;
using System.Text;

namespace OP.DataAccess
{
    public class ParamISRepository
    {
        public static List<LigneModeleISDto> GetISReferenciel(string referentielId = "", bool modeAutocomplete = false)
        {
            string sql = string.Format(@"SELECT 
	                                        KGBNMID CODE,
	                                        KGBDESC DESCRIPTION,
	                                        KGBLIB LIBELLE,
                                            KGBTYPZ TYPEZONE,
                                            KGBLNGZ LONGUEURZONE,
                                            KGBMAPP MAPPAGE,
                                            KGBTYPC CONVERSION,
                                            KGBPRES PRESENTATION,
                                            KGBTYPU TYPEUI,
                                            KGBOBLI OBLIGATOIRE,
                                            KGBSCRAFF AFFICHAGE,
                                            KGBSCRCTR CONTROLE,
                                            KGBOBSV OBSERVATION,
                                            KGBDVAL VALEURDEFAUT,
                                            KGBVUCON TCON,
                                            KGBVUFAM TFAM
                                        FROM KISREF
                                        ");

            if (!string.IsNullOrEmpty(referentielId) && !modeAutocomplete)
            {
                sql += string.Format("WHERE KGBNMID='{0}'", referentielId);
            }
            else if (!string.IsNullOrEmpty(referentielId) && modeAutocomplete)
            {
                sql += string.Format("WHERE TRIM(LOWER(KGBNMID)) LIKE '%{0}%'", referentielId.Trim().ToLower());
            }
            return DbBase.Settings.ExecuteList<LigneModeleISDto>(CommandType.Text, sql);
        }

        public static string SaveISReferenciel(LigneModeleISDto reference)
        {
            string sql = string.Empty;
            EacParameter[] param = null;
            if (reference.TypeOperation == "Insert")
            {
                param = new EacParameter[17];
                param[0] = new EacParameter("code", DbType.AnsiStringFixedLength);
                param[0].Value = reference.Code;
                param[1] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[1].Value = reference.Description;
                param[2] = new EacParameter("libelleAffiche", DbType.AnsiStringFixedLength);
                param[2].Value = reference.LibelleAffiche;
                param[3] = new EacParameter("typeZone", DbType.AnsiStringFixedLength);
                param[3].Value = reference.TypeZone;
                param[4] = new EacParameter("longueurZone", DbType.AnsiStringFixedLength);
                param[4].Value = reference.LongueurZone;
                param[5] = new EacParameter("mappage", DbType.AnsiStringFixedLength);
                param[5].Value = reference.Mappage;
                param[6] = new EacParameter("conversion", DbType.AnsiStringFixedLength);
                param[6].Value = reference.Conversion;
                param[7] = new EacParameter("presentation", DbType.Int32);
                param[7].Value = reference.Presentation;
                param[8] = new EacParameter("typeUI", DbType.AnsiStringFixedLength);
                param[8].Value = reference.TypeUI;
                param[9] = new EacParameter("obligatoire", DbType.AnsiStringFixedLength);
                param[9].Value = reference.Obligatoire;
                param[10] = new EacParameter("affichage", DbType.AnsiStringFixedLength);
                param[10].Value = reference.Affichage;
                param[11] = new EacParameter("controle", DbType.AnsiStringFixedLength);
                param[11].Value = reference.Controle;
                param[12] = new EacParameter("observation", DbType.AnsiStringFixedLength);
                param[12].Value = reference.Observation;
                param[13] = new EacParameter("aValue", DbType.Int32);
                param[13].Value = 0;
                param[14] = new EacParameter("dValue", DbType.AnsiStringFixedLength);
                param[14].Value = reference.ValeurDefaut;
                param[15] = new EacParameter("tcon", DbType.AnsiStringFixedLength);
                param[15].Value = reference.Tcon;
                param[16] = new EacParameter("tfam", DbType.AnsiStringFixedLength);
                param[16].Value = reference.Tfam;
                sql = @"INSERT INTO KISREF
                    (KGBNMID, KGBDESC, KGBLIB, KGBTYPZ, KGBLNGZ, KGBMAPP, KGBTYPC,KGBPRES, 
                    KGBTYPU,KGBOBLI, KGBSCRAFF, KGBSCRCTR, KGBOBSV, KGBNREF, KGBDVAL, KGBVUCON, KGBVUFAM)
                    VALUES
                    (:code, :description, :libelleAffiche, :typeZone, :longueurZone,:mappage,:conversion,:presentation,:typeUI,:obligatoire,:affichage, :controle, :observation, :aValue, :dValue, :tcon, :tfam)";
            }
            else if (reference.TypeOperation == "Update")
            {
                param = new EacParameter[17];
                param[0] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[0].Value = reference.Description;
                param[1] = new EacParameter("libelleAffiche", DbType.AnsiStringFixedLength);
                param[1].Value = reference.LibelleAffiche;
                param[2] = new EacParameter("typeZone", DbType.AnsiStringFixedLength);
                param[2].Value = reference.TypeZone;
                param[3] = new EacParameter("longueurZone", DbType.AnsiStringFixedLength);
                param[3].Value = reference.LongueurZone;
                param[4] = new EacParameter("mappage", DbType.AnsiStringFixedLength);
                param[4].Value = reference.Mappage;
                param[5] = new EacParameter("conversion", DbType.AnsiStringFixedLength);
                param[5].Value = reference.Conversion;
                param[6] = new EacParameter("presentation", DbType.Int32);
                param[6].Value = reference.Presentation;
                param[7] = new EacParameter("typeUI", DbType.AnsiStringFixedLength);
                param[7].Value = reference.TypeUI;
                param[8] = new EacParameter("obligatoire", DbType.AnsiStringFixedLength);
                param[8].Value = reference.Obligatoire;
                param[9] = new EacParameter("affichage", DbType.AnsiStringFixedLength);
                param[9].Value = reference.Affichage;
                param[10] = new EacParameter("controle", DbType.AnsiStringFixedLength);
                param[10].Value = reference.Controle;
                param[11] = new EacParameter("observation", DbType.AnsiStringFixedLength);
                param[11].Value = reference.Observation;
                param[12] = new EacParameter("aValue", DbType.Int32);
                param[12].Value = 0;
                param[13] = new EacParameter("dValue", DbType.AnsiStringFixedLength);
                param[13].Value = reference.ValeurDefaut;
                param[14] = new EacParameter("tcon", DbType.AnsiStringFixedLength);
                param[14].Value = reference.Tcon;
                param[15] = new EacParameter("tfam", DbType.AnsiStringFixedLength);
                param[15].Value = reference.Tfam;
                param[16] = new EacParameter("code", DbType.AnsiStringFixedLength);
                param[16].Value = reference.Code;
                sql = @"UPDATE KISREF            
                                     SET  KGBDESC= :description,
                                           KGBLIB = :libelleAffiche,
                                           KGBTYPZ = :typeZone,
                                           KGBLNGZ = :longueurZone,
                                           KGBMAPP = :Mappage,
                                           KGBTYPC = :conversion,
                                           KGBPRES = :presentation,
                                           KGBTYPU = :typeUI,
                                           KGBOBLI = :obligatoire,
                                           KGBSCRAFF = :affichage,
                                           KGBSCRCTR = :controle,
                                           KGBOBSV =  :observation,
                                           KGBNREF = :aValue,
                                           KGBDVAL = :dValue,
                                           KGBVUCON = :tcon,
                                           KGBVUFAM = :tfam
                                      WHERE KGBNMID = :code";
            }

            return DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param).ToString(CultureInfo.InvariantCulture);
        }

        public static void SupprimerISReferenciel(string code)
        {
            string sql = string.Format(@"DELETE FROM KISREF WHERE KGBNMID = '{0}'", code);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static string SaveISModeleDetails(ModeleISDto modele, string user)
        {
            string sql = string.Empty;
            EacParameter[] param = null;
            if (modele.TypeOperation == "Insert")
            {
                param = new EacParameter[12];
                param[0] = new EacParameter("nomModele", DbType.AnsiStringFixedLength);
                param[0].Value = modele.NomModele;
                param[1] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[1].Value = modele.Description;
                param[2] = new EacParameter("dateDebut", DbType.Int32);
                param[2].Value = modele.DateDebut;
                param[3] = new EacParameter("dateFin", DbType.Int32);
                param[3].Value = modele.DateFin;
                param[4] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[4].Value = user;
                param[5] = new EacParameter("date", DbType.AnsiStringFixedLength);
                param[5].Value = DateTime.Now.ToString("yyyyMMdd");
                param[6] = new EacParameter("user2", DbType.AnsiStringFixedLength);
                param[6].Value = user;
                param[7] = new EacParameter("date2", DbType.AnsiStringFixedLength);
                param[7].Value = DateTime.Now.ToString("yyyyMMdd");
                
                sql = @"INSERT INTO KISMOD
                    (KGCMODID, KGCDESC, KGCDATD, KGCDATF, 
                    KGCCRU,KGCCRD, KGCMJU, KGCMJD )
                    VALUES
                    (:nomModele ,:description,:dateDebut,:dateFin,:user,:date,:user2,:date2)";
            }
            else if (modele.TypeOperation == "Update")
            {
                param = new EacParameter[12];

                param[0] = new EacParameter("description", DbType.AnsiStringFixedLength);
                param[0].Value = modele.Description;
                param[1] = new EacParameter("dateDebut", DbType.Int32);
                param[1].Value = modele.DateDebut;
                param[2] = new EacParameter("dateFin", DbType.Int32);
                param[2].Value = modele.DateFin;
                param[7] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[7].Value = user;
                param[8] = new EacParameter("date", DbType.AnsiStringFixedLength);
                param[8].Value = DateTime.Now.ToString("yyyyMMdd");
                param[9] = new EacParameter("nomModele", DbType.AnsiStringFixedLength);
                param[9].Value = modele.NomModele;
                sql = @"UPDATE KISMOD
                        SET  KGCDESC= :description,
                            KGCDATD = :dateDebut,
                            KGCDATF = :dateFin,
                            KGCMJU = :user,
                            KGCMJD = :date
                        WHERE KGCMODID = :nomModele";
            }
            return DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param).ToString();
        }

        public static string SaveISModeleLigne(ParamISLigneModeleDto ligne, string user)
        {
            string sql = string.Empty;
            EacParameter[] param = null;
            if (ligne.TypeOperation == "Insert")
            {
                int idLigne = CommonRepository.GetAS400Id("KGDID");

                param = new EacParameter[21];
                param[0] = new EacParameter("idLigne", DbType.Int32);
                param[0].Value = idLigne;
                param[1] = new EacParameter("modeleIS", DbType.AnsiStringFixedLength);
                param[1].Value = ligne.ModeleIS;
                param[2] = new EacParameter("referentiel", DbType.AnsiStringFixedLength);
                param[2].Value = ligne.Referentiel;
                param[3] = new EacParameter("libelle", DbType.AnsiStringFixedLength);
                param[3].Value = ligne.Libelle;
                param[4] = new EacParameter("numOrdreAffichage", DbType.Single);
                param[4].Value = ligne.NumOrdreAffichage;
                param[5] = new EacParameter("sautDeLigne", DbType.Int32);
                param[5].Value = ligne.SautDeLigne;
                param[6] = new EacParameter("modifiable", DbType.AnsiStringFixedLength);
                param[6].Value = ligne.Modifiable;
                param[7] = new EacParameter("obligatoire", DbType.AnsiStringFixedLength);
                param[7].Value = ligne.Obligatoire;
                param[8] = new EacParameter("tcon", DbType.AnsiStringFixedLength);
                param[8].Value = ligne.Tcon;
                param[9] = new EacParameter("tfam", DbType.AnsiStringFixedLength);
                param[9].Value = ligne.Tfam;
                param[10] = new EacParameter("affichage", DbType.AnsiStringFixedLength);
                param[10].Value = ligne.Affichage;
                param[11] = new EacParameter("controle", DbType.AnsiStringFixedLength);
                param[11].Value = ligne.Controle;
                param[12] = new EacParameter("lienParent", DbType.Int32);
                param[12].Value = ligne.LienParent;
                param[13] = new EacParameter("parentComportement", DbType.AnsiStringFixedLength);
                param[13].Value = ligne.ParentComportement;
                param[14] = new EacParameter("parentEvenement", DbType.AnsiStringFixedLength);
                param[14].Value = ligne.ParentEvenement;
                param[15] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[15].Value = user;
                param[16] = new EacParameter("date", DbType.AnsiStringFixedLength);
                param[16].Value = DateTime.Now.ToString("yyyyMMdd");
                param[17] = new EacParameter("user2", DbType.AnsiStringFixedLength);
                param[17].Value = user;
                param[18] = new EacParameter("date2", DbType.AnsiStringFixedLength);
                param[18].Value = DateTime.Now.ToString("yyyyMMdd");
                param[19] = new EacParameter("presentation", DbType.Int32);
                param[19].Value = ligne.Presentation;
                param[20] = new EacParameter("aValue", DbType.Int32);
                param[20].Value = 0;
                sql = @"INSERT INTO KISMODL
                        (KGDID, KGDMODID, KGDNMID, KGDLIB, KGDNUMAFF, KGDSAUTL, KGDMODI,KGDOBLI, 
                        KGDVUCON, KGDVUFAM, KGDSCRAFFS, KGDSCRCTRS,KGDPARENID, KGDPARENC, KGDPARENE, KGDCRU,
                        KGDCRD, KGDMJU, KGDMJD, KGDPRES, KGDNREF)
                        VALUES
                        (:idLigne,:modeleIS,:referentiel, :libelle, :numOrdreAffichage, :sautDeLigne, :modifiable, :obligatoire, :tcon, :tfam, :affichage, :controle,
                            :lienParent, :parentComportement, :parentEvenement,
                            :user , :date, :user2, :date2, :presentation, :aValue)";
            }
            else if (ligne.TypeOperation == "Update")
            {
                param = new EacParameter[21];
                param[0] = new EacParameter("modeleIS", DbType.AnsiStringFixedLength);
                param[0].Value = ligne.ModeleIS;
                param[1] = new EacParameter("referentiel", DbType.AnsiStringFixedLength);
                param[1].Value = ligne.Referentiel;
                param[2] = new EacParameter("libelle", DbType.AnsiStringFixedLength);
                param[2].Value = ligne.Libelle;
                param[3] = new EacParameter("numOrdreAffichage", DbType.Single);
                param[3].Value = ligne.NumOrdreAffichage;
                param[4] = new EacParameter("sautDeLigne", DbType.Int32);
                param[4].Value = ligne.SautDeLigne;
                param[5] = new EacParameter("modifiable", DbType.AnsiStringFixedLength);
                param[5].Value = ligne.Modifiable;
                param[6] = new EacParameter("obligatoire", DbType.AnsiStringFixedLength);
                param[6].Value = ligne.Obligatoire;
                param[7] = new EacParameter("tcon", DbType.AnsiStringFixedLength);
                param[7].Value = ligne.Tcon;
                param[8] = new EacParameter("tfam", DbType.AnsiStringFixedLength);
                param[8].Value = ligne.Tfam;
                param[9] = new EacParameter("affichage", DbType.AnsiStringFixedLength);
                param[9].Value = ligne.Affichage;
                param[10] = new EacParameter("controle", DbType.AnsiStringFixedLength);
                param[10].Value = ligne.Controle;
                param[11] = new EacParameter("lienParent", DbType.Int32);
                param[11].Value = ligne.LienParent;
                param[12] = new EacParameter("parentComportement", DbType.AnsiStringFixedLength);
                param[12].Value = ligne.ParentComportement;
                param[13] = new EacParameter("parentEvenement", DbType.AnsiStringFixedLength);
                param[13].Value = ligne.ParentEvenement;
                param[14] = new EacParameter("user", DbType.AnsiStringFixedLength);
                param[14].Value = user;
                param[15] = new EacParameter("date", DbType.AnsiStringFixedLength);
                param[15].Value = DateTime.Now.ToString("yyyyMMdd");
                param[16] = new EacParameter("date2", DbType.AnsiStringFixedLength);
                param[16].Value = user;
                param[17] = new EacParameter("date2", DbType.AnsiStringFixedLength);
                param[17].Value = DateTime.Now.ToString("yyyyMMdd");
                param[18] = new EacParameter("presentation", DbType.Int32);
                param[18].Value = ligne.Presentation;
                param[19] = new EacParameter("aValue", DbType.Int32);
                param[19].Value = 0;
                param[20] = new EacParameter("idLigne", DbType.Int32);
                param[20].Value = ligne.GuidId;

                sql = @"UPDATE KISMODL
                                      SET  KGDMODID= :modeleIS,
                                           KGDNMID = :referentiel,
                                           KGDLIB = :libelle,
                                           KGDNUMAFF = :numOrdreAffichage,
                                           KGDSAUTL = :sautDeLigne,
                                           KGDMODI = :modifiable,
                                           KGDOBLI = :obligatoire,
                                           KGDVUCON = :tcon,
                                           KGDVUFAM = :tfam,
                                           KGDSCRAFFS = :affichage,
                                           KGDSCRCTRS = :controle,
                                           KGDPARENID = :lienParent,
                                           KGDPARENC = :parentComportement,
                                           KGDPARENE = :parentEvenement,
                                           KGDCRU = :user,
                                           KGDCRD = :date,
                                           KGDMJU = :user2,
                                           KGDMJD = :date2,
                                           KGDPRES = :presentation,
                                           KGDNREF = :aValue
                                      WHERE KGDID = :idLigne";
            }
            else if (ligne.TypeOperation == "Delete")
            {
                param = new EacParameter[1];
                param[0] = new EacParameter("guidId", DbType.Int32);
                param[0].Value = ligne.GuidId;
                sql = @"DELETE FROM KISMODL
                                      WHERE KGDID = :guidId";
            }
            return !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param).ToString() : string.Empty;
        }

        public static void SupprimerModeleEtDependances(string modeleName)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_NOM_MODELE", DbType.AnsiStringFixedLength);
            param[0].Value = modeleName;
            var sql = "DELETE FROM KISMODL WHERE KGDMODID = :P_NOM_MODELE";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            sql = "DELETE FROM KISMOD WHERE KGCMODID = :P_NOM_MODELE";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static List<ModeleISDto> GetISModeles(string modeleId = "")
        {
            string sql = string.Format(@"SELECT 
	                                        KGCMODID NOM,
	                                        KGCDESC DESCRIPTION,
	                                        KGCDATD DATEDEB,
                                            KGCDATF DATEFIN,
                                            KGCSELECT SELECT,
                                            KGCINSERT INSERT,
                                            KGCUPDATE UPDATE,
                                            KGCEXIST EXIST,
                                            KGCSAID2 SCRIPTAFF,
                                            KGCSCID2 SCRIPTCTRL
                                        FROM KISMOD
                                        ");
            if (!string.IsNullOrEmpty(modeleId))
            {
                sql += string.Format("WHERE KGCMODID='{0}'", modeleId);
            }

            return DbBase.Settings.ExecuteList<ModeleISDto>(CommandType.Text, sql);
        }

        public static List<ParamISLigneModeleDto> GetISModeleLignes(string modeleId, string dbMapCol, string ligneId)
        {
            string sql = string.Format(@"SELECT 
	                                        BASE.KGDID ID,
                                            BASE.KGDMODID MODELE,
                                            BASE.KGDNMID REFERENTIEL,
                                            BASE.KGDLIB LIBELLE,
                                            BASE.KGDNUMAFF NUMAFFICHAGE,
                                            BASE.KGDSAUTL SAUTLIGNE,
                                            BASE.KGDMODI MODIFIABLE,
                                            BASE.KGDOBLI OBLIGATOIRE,
                                            BASE.KGDSQL SQLREQUEST,
                                            BASE.KGDSQLORD SQLORDER,
                                            BASE.KGDSCRAFFS AFFICHAGE,
                                            BASE.KGDSCRCTRS CONTROLE,
                                            BASE.KGDPARENID LIENPARENT,
                                            BASE.KGDPARENC PARENTCMPT,
                                            BASE.KGDPARENE PARENTEVT,
                                            BASE.KGDPRES PRESENTATION,
                                            KISREF.KGBTYPU TYPEUI,
                                            CHILD.KGDLIB LIENPARENTLIBELLE,
                                            BASE.KGDSAID2 SCRIPTAFF,
                                            BASE.KGDSCID2 SCRIPTCTRL
                                        FROM KISMODL BASE
                                        LEFT JOIN KISMODL CHILD ON BASE.KGDPARENID = CHILD.KGDID
                                        LEFT JOIN KISREF ON BASE.KGDNMID = KISREF.KGBNMID
                                        ");

            string whereAnd = "WHERE ";
            if (!string.IsNullOrEmpty(dbMapCol))
            {
                sql += string.Format(whereAnd + " KISREF.KGBNMBD='{0}'", dbMapCol);
                whereAnd = "AND ";
            }

            if (!string.IsNullOrEmpty(modeleId))
            {
                sql += string.Format(whereAnd + " BASE.KGDMODID='{0}'", modeleId);
            }

            if (!string.IsNullOrEmpty(ligneId))
            {
                sql += string.Format(whereAnd + " BASE.KGDID='{0}'", ligneId);
            }
            sql += " ORDER BY BASE.KGDNUMAFF";
            return DbBase.Settings.ExecuteList<ParamISLigneModeleDto>(CommandType.Text, sql);
        }

        public static string GetDbMapColFromLigneModele(string ligneModeleId)
        {
            string sql = string.Format(@"SELECT KGBNMBD STRRETURNCOL 
                                        FROM KISREF 
                                        INNER JOIN KISMODL 
                                        ON  KISMODL.KGDNMID = KISREF.KGBNMID 
                                            AND KISMODL.KGDID = '{0}'", ligneModeleId);
            return CommonRepository.GetStrValue(sql);
        }

        public static List<ParamISLigneInfoDto> GetParamISLignesInfo(string modeleId)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("modeleId", DbType.AnsiStringFixedLength);
            param[0].Value = modeleId;

            string sql = string.Format(@"
SELECT BASE.KGDMODID MODELEID,
    BASE.KGDNMID INTERNALPROPERTYNAME,
    BASE.KGDID CODE,
    KGBNMID DESCRIPTION,
    BASE.KGDLIB LIBELLE,
    KGBTYPZ TYPEZONE,
    KGBLNGZ LONGUEURZONE,
    KGBMAPP MAPPAGE,
    KGBTYPC CONVERSION,
    BASE.KGDPRES PRESENTATION,
    KGBTYPU TYPEUI,
    BASE.KGDOBLI OBLIGATOIRE,
    BASE.KGDSCRAFFS AFFICHAGE,
    BASE.KGDSCRCTRS CONTROLE,
    KGBOBSV OBSERVATION,
    KGBNMBD DBMAPCOL,
    BASE.KGDNUMAFF NUMAFFICHAGE,
    BASE.KGDSAUTL SAUTLIGNE,
    BASE.KGDMODI MODIFIABLE,
    BASE.KGDSQL SQLREQUEST,
    BASE.KGDSQLORD SQLORDER,
    P.KGDNMID LIENPARENT,
    BASE.KGDPARENC PARENTCMPT,
    BASE.KGDPARENE PARENTEVT 
FROM KISMODL BASE 
LEFT JOIN KISMODL P ON BASE.KGDPARENID = P.KGDID 
LEFT JOIN KISREF ON BASE.KGDNMID = KISREF.KGBNMID ", string.IsNullOrEmpty(modeleId) ? string.Empty : "WHERE BASE.KGDMODID = :modeleId");

            return string.IsNullOrEmpty(modeleId) ?
                 DbBase.Settings.ExecuteList<ParamISLigneInfoDto>(CommandType.Text, sql, param) :
                 DbBase.Settings.ExecuteList<ParamISLigneInfoDto>(CommandType.Text, sql);
        }

        public static Dictionary<string, string> GetISDefaultValueData(List<string> parametersIS)
        {
            StringBuilder nameListIS = new StringBuilder();

            foreach(var line in parametersIS)
            {
                if (nameListIS.ToString() != string.Empty)
                    nameListIS.Append(",");

                nameListIS.Append("'" + line + "'");
            }


            string sql = string.Format(@"SELECT  KGBNMID, KGBNMBD, KGBDVAL   FROM KISREF WHERE  KGBNMID IN ({0});", nameListIS.ToString());



            Dictionary<string,string> results = new Dictionary<string,string>();
            using (var reader = DbBase.Settings.ExecuteReader(CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    results.Add(string.Format("{0}||{1}", reader["KGBNMID"].ToString(), reader["KGBNMBD"].ToString()), reader["KGBDVAL"].ToString());
                }

                reader.Close();
            }

            return results;
        }
    }
}
