using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.ControleFin;
using OP.WSAS400.DTO.Offres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess
{
    public class ControleFinRepository : RepositoryBase
    {
        internal static readonly string DeleteEtapeCOT = "DELETE FROM KPCTRLE WHERE KEVIPB = :codeAffaire AND KEVALX = :version AND KEVTYP = :type AND KEVETAPE = 'COT' ;";
        internal static readonly string CountCtrlModif = "SELECT COUNT ( * ) FROM KPCTRLA WHERE KGTIPB = :CODEOFFRE AND KGTALX = :VERSION AND KGTTYP = :TYPE ;";
        internal static readonly string PurgeCtrlEtapes = "DELETE FROM KPCTRLE WHERE KEVIPB = :CODEOFFRE AND KEVALX = :VERSION AND KEVTYP = :TYPE AND KEVETAPE = :ETAPE ; ";
        internal static readonly string AddCtrlDateAvn = @"INSERT INTO KPCTRLA 
( KGTIPB , KGTALX , KGTTYP , KGTETAPE , KGTLIB , KGTCRU , KGTCRD , KGTCRH , KGTMAJU , KGTMAJD , KGTMAJH ) 
VALUES 
( :CODEOFFRE , :VERSION, :TYPE , 'GEN' , 'Date d''avenant' , :USER , :DATENOW , 0 , :USER , :DATENOW , 0 ) ;";

        public ControleFinRepository(IDbConnection connection) : base(connection) { }

        #region Méthodes Publiques
        public static ControleFinDto InitControleFin(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            ControleFinDto toReturn = new ControleFinDto();

            string sql = string.Format(@"SELECT KEUETAPE ETAPE,KEURSQ RISQUE,KEUOBJ OBJET,KEUINVEN IDINVENTAIRE,KEUINLGN NUMEROLIGNEINVENTAIRE,
                                       KEUFOR FORMULE,KEUOPT OPTION,KEUGAR GARANTIE,KEUMSG MESSAGE,KEUNIVM NIVEAU, KDAALPHA LETTREFORMULE, PBTTR ACTEGESTION FROM {0}
                                       LEFT JOIN KPFOR ON KDAIPB = KEUIPB AND KEUTYP = KDATYP AND KEUALX = KDAALX AND KDAFOR = KEUFOR
                                       LEFT JOIN YPOBASE ON PBIPB = KEUIPB AND KEUTYP = PBTYP AND KEUALX = PBALX
                                       WHERE KEUTYP = '{1}' AND KEUIPB = '{2}' AND KEUALX ='{3}' 
                                       ORDER BY KEUETORD,KEUORDR",
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPCTRL"),
                                    type, codeOffre.PadLeft(9, ' '), version,
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND KEUAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            var listControleFinControle = DbBase.Settings.ExecuteList<ControleFinControleDto>(CommandType.Text, sql);
            if (listControleFinControle.Any())
            {
                foreach (var cf in listControleFinControle)
                {
                    switch (cf.Etape)
                    {
                        case "MATR":
                            cf.Reference = "Matrice risques";
                            cf.LienReference = "/MatriceRisque/Index/" + codeOffre + "_" + version + "_" + type;
                            break;
                        case "MATF":
                            cf.Reference = "Matrice formules";
                            cf.LienReference = "/MatriceFormule/Index/" + codeOffre + "_" + version + "_" + type;
                            break;
                        case "RSQ":
                            cf.Reference = "N° " + cf.Risque;
                            cf.LienReference = "/DetailsRisque/Index/" + codeOffre + "_" + version + "_" + type + "_" + cf.Risque;
                            break;
                        case "OBJ":
                            cf.Reference = cf.Objet + " du Risque N° " + cf.Risque;
                            cf.LienReference = "/DetailsObjetRisque/Index/" + codeOffre + "_" + version + "_" + type + "_" + cf.Risque + "_" + cf.Objet;
                            break;
                        case "INV":
                            cf.Reference = "Inventaire de l'objet " + cf.Objet + " du Risque N° " + cf.Risque;
                            break;
                        case "FOR":
                            cf.Reference = "Formule " + cf.LettreFormule;
                            break;
                        case "OPT":
                            cf.Reference = "Option " + cf.Option + " de la Formule " + cf.LettreFormule;
                            cf.LienReference = "/FormuleGarantie/Index/" + codeOffre + "_" + version + "_" + type + "_" + cf.Formule + "_" + cf.Option + "_" + 0;
                            break;
                        case "GAR":
                            cf.Reference = "Garantie " + cf.Garantie + " de l'Option " + cf.Option + " de la Formule " + cf.LettreFormule;
                            cf.LienReference = "/ConditionsGarantie/Index/" + codeOffre + "_" + version + "_" + type + "_" + cf.Formule + "_" + cf.Option + "_C";
                            break;
                        case "SBR":
                            cf.Reference = "Cotisations";
                            switch (type)
                            {
                                case "O":
                                    cf.LienReference = "/Cotisations/Index/" + codeOffre + "_" + version + "_" + type;
                                    break;
                                case "P":
                                    cf.LienReference = "/Quittance/Index/" + codeOffre + "_" + version + "_" + type;
                                    break;
                            }
                            break;
                        case "GEN":
                            switch (cf.ActeGestion)
                            {
                                case "AVNRM":
                                    cf.Reference = "Infos Avenant";
                                    cf.LienReference = "/CreationAvenant/Index/" + codeOffre + "_" + version + "_" + type;
                                    break;
                                default:
                                    cf.Reference = "Informations générales";
                                    cf.LienReference = (type == AlbConstantesMetiers.TYPE_OFFRE ? "/ModifierOffre/Index/" : "/AnInformationsGenerales/Index/") + codeOffre + "_" + version + "_" + type;
                                    break;
                            }
                            break;
                        case "SAISIE":
                            cf.Reference = "Informations de base";
                            cf.LienReference = "/AnCreationContrat/Index/" + codeOffre + "_" + version + "_" + type;
                            break;
                        case "ENG":
                            cf.Reference = "Engagements";
                            cf.LienReference = "/Engagements/Index/" + codeOffre + "_" + version + "_" + type;
                            break;
                        case "REGUL":
                            /*Recupere ReguleID */
                            //string sqlReguleId = string.Format(@" SELECT KHWID INT64RETURNCOL FROM KPRGU  WHERE TRIM(KHWIPB) = '{0}' AND KHWALX = '{3}' AND KHWTYP = '{1}' AND KHWAVN = '{2}' ", codeOffre, type, codeAvn, version);
                            //var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlReguleId);
                           
                            //if (result != null && result.Any())
                            //{
                                //var reguleId = result.FirstOrDefault().Int64ReturnCol.ToString();
                                cf.Reference = "Regule";
                                cf.LienReference = "/CreationRegularisation/Index/" + codeOffre + "_" + version + "_" + type;// +"_" + reguleId;
                                break;
                            //}
                            //break;
                        default:
                            cf.Reference = string.Empty;
                            break;
                    }
                }
                toReturn.ControleFinListeControleDto = listControleFinControle;
            }
            return toReturn;
        }
        public static void Alimentation(string codeOffre, string version, string type, string user, ModeConsultation modeNavig, bool isModifHorsAvn, bool isAvenant, string regulId)
        {
            int iVersion = 0;
            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_CODE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = int.TryParse(version, out iVersion) ? iVersion : iVersion;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_USER", user);
            param[4] = new EacParameter("P_DATE", 0);
            param[4].Value = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            param[5] = new EacParameter("P_HEURE", 0);
            param[5].Value = int.Parse(DateTime.Now.ToString("HHmmss"));
            param[6] = new EacParameter("P_ISMODIFHORSAVN", DbType.Int32);
            param[6].Value = Convert.ToInt32(isModifHorsAvn);
            param[7] = new EacParameter("P_ISAVENANT", DbType.Int32);
            param[7].Value = Convert.ToInt32(isAvenant);
            param[8] = new EacParameter("P_REGULID", DbType.Int32);
            param[8].Value = !string.IsNullOrEmpty(regulId) ? Convert.ToInt32(regulId) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ALIMCTRL", param);
        }

        public static void AlimentationAssure(string codeOffre, string version, string type, string user, bool isAvenant, int codeAssure)
        {
            InsertionKPCTRL(type, codeOffre, version, "SAISIE", "30", "SAISIE", "", "", "Le preneur d''assurance est inactif", isAvenant ? "A" : "B", user);
        }

        public static void UpdateEtatRegul(string regulId)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("p_regulid", DbType.Int64);
            param[0].Value = regulId;
            String sql = String.Format("UPDATE KPRGU SET KHWETA = 'A' WHERE KHWID = :p_regulid AND KHWETA='N'");
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static void CleanControl(string codeOffre, string type, string version)
        {
            int iVersion = 0;
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODE", codeOffre.PadLeft(9, ' '));
            param[0].Value = codeOffre;
            param[1] = new EacParameter("P_TYPE", type);
            param[2] = new EacParameter("P_VERSION", version);
            param[2].Value = int.TryParse(version, out iVersion) ? iVersion : iVersion;
            String sql = String.Format("DELETE FROM KPCTRL WHERE KEUIPB = :P_CODE AND KEUTYP = :P_TYPE AND KEUALX = :P_VERSION");
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion
        #region Méthodes privées
        private static void SuppressionKPCTRL(string codeOffre, string version, string type)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[0].Value = type;
            param[1] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("version", DbType.Int32);
            param[2].Value = Convert.ToInt32(version);

            string sql = @"DELETE FROM KPCTRL WHERE KEUTYP = :type AND KEUIPB = :codeoffre AND KEUALX = :version";
                 //type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version));
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static OffreInfoDto ObtenirOffre(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT PBEFA DATEEFFETA,PBEFM DATEEFFETM,PBEFJ DATEEFFETJ,PBFEA FINEFFETANNEE,PBFEM FINEFFETMOIS,PBFEJ FINEFFETJOUR,PBBRA CODEBRANCHE FROM YPOBASE
                                        WHERE PBTYP='{0}' AND PBIPB='{1}' AND PBALX='{2}'", type, codeOffre.PadLeft(9, ' '), Convert.ToInt32(version));
            return DbBase.Settings.ExecuteList<OffreInfoDto>(CommandType.Text, sql).FirstOrDefault();
        }

        private static bool DateRenseignee(string annee, string mois, string jour)
        {
            return (!annee.Equals("0") && !mois.Equals("0") && !jour.Equals("0"));
        }

        private static void InsertionKPCTRL(string type, string codeOffre, string version, string etapeGeneration, string numeroOrdreEtape, string perimetre, string risque, string objet, string message, string niveau, string user)
        {
            //Si le risque est vide
            if (string.IsNullOrEmpty(risque))
            {
                risque = "0";
            }
            //Si l'objet est vide
            if (string.IsNullOrEmpty(objet))
            {
                objet = "0";
            }
            string sql = @"INSERT INTO KPCTRL (KEUID,KEUTYP,KEUIPB,KEUALX,KEUETAPE,KEUETORD,KEUORDR,KEUPERI,KEURSQ,KEUOBJ,KEUMSG,KEUNIVM,KEUCRU,KEUCRD,KEUCRH)
                    VALUES (" + CommonRepository.GetAS400Id("KEUID") + ",'" + type + "','" + codeOffre.PadLeft(9, ' ') + "'," + version + ",'" + etapeGeneration + "'," + numeroOrdreEtape + "," + CommonRepository.GetAS400Id("KEUORDR") + ",'" + perimetre + "','" + risque + "','" + objet + "','" + message + "','" + niveau + "','" + user + "','" + DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + "','" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + "')";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        private static List<RisqueExecListDto> ObtenirRisques(string codeOffre, string version, string type)
        {
            string sql = string.Format(@"SELECT JEOBJ,JERSQ,JEVDA,JEVDM,JEVDJ,JEVFA,JEVFM,JEVFJ,JECNA,JEINA FROM YPRTRSQ WHERE JEIPB = '{0}' AND JEALX = '{1}' ORDER BY JECCH", codeOffre.PadLeft(9, ' '), version);
            return DbBase.Settings.ExecuteList<RisqueExecListDto>(CommandType.Text, sql);
        }

        //ObjetExecListDto
        private static ObjetExecListDto ObtenirMonoObjet(string codeOffre, string version, string type, string risque, string objet)
        {
            string sql = string.Format(@"SELECT JGVDA,JGVDM,JGVDJ,JGRSQ,JGVFA,JGVFM,JGVFJ,JGCNA,JGINA FROM YPRTOBJ
                                       WHERE JGIPB='{0}' AND JGALX='{1}' AND JGRSQ='{2}' AND JGOBJ='{3}'",
                                       codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), Convert.ToInt32(risque), Convert.ToInt32(objet));
            return DbBase.Settings.ExecuteList<ObjetExecListDto>(CommandType.Text, sql).FirstOrDefault();
        }

        private static List<ObjetExecListDto> ObtenirMultiObjet(string codeOffre, string version, string type, string risque)
        {
            string sql = @"SELECT JGOBJ,JGVDA,JGVDM,JGVDJ,JGRSQ,JGVFA,JGVFM,JGVFJ,JGCNA FROM YPRTOBJ
                                                            WHERE JGIPB  = '" + codeOffre.PadLeft(9, ' ') + "' AND JGALX = " + version + " AND JGRSQ = " + risque + " ORDER BY JGCCH";
            return DbBase.Settings.ExecuteList<ObjetExecListDto>(CommandType.Text, sql);
        }

        private static int Convert3PartsDateToInt(string annee, string mois, string jour)
        {
            return Convert.ToInt32(annee.PadLeft(4, '0') + mois.PadLeft(2, '0') + jour.PadLeft(2, '0'));
        }

        [Obsolete]
        private static void MonoObjet(string codeOffre, string version, string type, bool? dateDebutYPOBASERenseigne, bool? dateFinYPOBASERenseigne, string JERSQ, string JEOBJ, string PBEFA, string PBEFM, string PBEFJ, string PBFEA, string PBFEM, string PBFEJ, string user)
        {
            //récupération des objets
            var objet = ObtenirMonoObjet(codeOffre, version, type, JERSQ, JEOBJ);
            //DataTable dataTableControleEtapeRisqueObjetYPRTOBJ = ObtenirMonoObjet(codeOffre, version, type, JERSQ, JEOBJ);
            if (objet != null)
            {
                string JGVDA = objet.EntreeGarantieAnnee.ToString();
                string JGVDM = objet.EntreeGarantieMois.ToString();
                string JGVDJ = objet.EntreeGarantieJour.ToString();
                string JGRSQ = objet.CodeRisque.ToString();
                string JGVFA = objet.SortieGarantieAnnee.ToString();
                string JGVFM = objet.SortieGarantieMois.ToString();
                string JGVFJ = objet.SortieGarantieJour.ToString();
                string JGCNA = objet.CATNAT;
                string JGINA = objet.RisqueIndexe;
                //Si la date de début est renseignée
                if (DateRenseignee(JGVDA, JGVDM, JGVDJ))
                {
                    //Si la date d'effet de garantie est renseignée
                    if (dateDebutYPOBASERenseigne != null && dateDebutYPOBASERenseigne == true)
                    {
                        //Si la date de début est antérieure à la date d'effet de garantie
                        if (Convert3PartsDateToInt(JGVDA, JGVDM, JGVDJ) < Convert3PartsDateToInt(PBEFA, PBEFM, PBEFJ))
                        {
                            //Création d'un enregistrement dans KPCTRL
                            InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JGRSQ, "", "Incohérence Date début de garantie/Début Effet", "B", user);
                        }
                    }
                    //Si la date de fin de garantie est renseignée
                    if (dateFinYPOBASERenseigne != null && dateFinYPOBASERenseigne == true)
                    {
                        //Si la date de début est postérieure à la date de fin de garantie
                        if (Convert3PartsDateToInt(JGVDA, JGVDM, JGVDJ) > Convert3PartsDateToInt(PBFEA, PBFEM, PBFEJ))
                        {
                            //Création d'un enregistrement dans KPCTRL
                            InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JGRSQ, "", "Incohérence Date début de garantie/Fin Effet", "B", user);
                        }
                    }
                }
                //Si la date de fin est renseignée
                if (DateRenseignee(JGVFA, JGVFM, JGVFJ))
                {
                    //Si la date de fin de garantie est renseignée
                    if (dateFinYPOBASERenseigne != null && dateFinYPOBASERenseigne == true)
                    {
                        //Si la date de fin est postérieure à la date de fin de garantie
                        if (Convert3PartsDateToInt(JGVFA, JGVFM, JGVFJ) > Convert3PartsDateToInt(PBFEA, PBFEM, PBFEJ))
                        {
                            //Création d'un enregistrement dans KPCTRL
                            InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JGRSQ, "", "Incohérence Date Fin de garantie/Fin Effet", "B", user);
                        }
                    }
                    //Si la date d'effet de garantie est renseignée
                    if (dateDebutYPOBASERenseigne != null && dateDebutYPOBASERenseigne == true)
                    {
                        //Si la date de fin est antérieure à la date d'effet de garantie
                        if (Convert3PartsDateToInt(JGVFA, JGVFM, JGVFJ) < Convert3PartsDateToInt(PBEFA, PBEFM, PBEFJ))
                        {
                            //Création d'un enregistrement dans KPCTRL
                            InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JGRSQ, "", "Incohérence Date Fin de garantie/Début Effet", "B", user);
                        }
                    }
                }
                var offreEntete = ObtenirOffreEnteteDto(codeOffre, version);

                if (offreEntete != null)
                {
                    string JDCNA = offreEntete.SoumisCatnat;
                    string JDINA = offreEntete.Indexation;
                    //Catnat
                    if (JGCNA.Equals("O"))
                    {
                        if (JDCNA.Equals("N"))
                        {
                            //Création d'un enregistrement dans KPCTRL
                            InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JGRSQ, "", "Incohérence info CATNAT  Risque/Base", "B", user);
                        }
                    }
                    //Indexation
                    if (JGINA.Equals("O"))
                    {
                        if (JDINA.Equals("N"))
                        {
                            //Création d'un enregistrement dans KPCTRL
                            InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JGRSQ, "", "Incohérence info indexation  Risque/Base", "B", user);
                        }
                    }
                }
            }
        }

        private static OffreEnteteExecListDto ObtenirOffreEnteteDto(string codeOffre, string version)
        {
            string sql = string.Format(@"SELECT JDCNA,JDINA FROM YPRTENT WHERE JDIPB='{0}' AND JDALX='{1}'", codeOffre.PadLeft(9, ' '), Convert.ToInt32(version));
            var offreEntete = DbBase.Settings.ExecuteList<OffreEnteteExecListDto>(CommandType.Text, sql).FirstOrDefault();
            return offreEntete;
        }

        [Obsolete]
        private static void MultiObjet(string codeOffre, string version, string type, bool? dateDebutYPOBASERenseigne, bool? dateFinYPOBASERenseigne, string JERSQ, string JECNA, string JEINA, string JEVDA, string JEVDM, string JEVDJ, string JEVFA, string JEVFM, string JEVFJ, string PBEFA, string PBEFM, string PBEFJ, string PBFEA, string PBFEM, string PBFEJ, string user)
        {
            //Date Début
            if (DateRenseignee(JEVDA, JEVDM, JEVDJ))
            {
                if (dateDebutYPOBASERenseigne != null && dateDebutYPOBASERenseigne == true)
                {
                    if (Convert3PartsDateToInt(JEVDA, JEVDM, JEVDJ) < Convert3PartsDateToInt(PBEFA, PBEFM, PBEFJ))
                    {
                        //Création d'un enregistrement dans KPCTRL
                        InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JERSQ, "", "Incohérence Date début de garantie/Début Effet", "B", user);
                    }
                }
                if (dateFinYPOBASERenseigne != null && dateFinYPOBASERenseigne == true)
                {
                    if (Convert3PartsDateToInt(JEVDA, JEVDM, JEVDJ) > Convert3PartsDateToInt(PBFEA, PBFEM, PBFEJ))
                    {
                        //Création d'un enregistrement dans KPCTRL
                        InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JERSQ, "", "Incohérence Date début de garantie/Fin Effet", "B", user);
                    }
                }
            }
            //Date Fin
            if (DateRenseignee(JEVFA, JEVFM, JEVFJ))
            {
                if (dateFinYPOBASERenseigne != null && dateFinYPOBASERenseigne == true)
                {
                    if (Convert3PartsDateToInt(JEVFA, JEVFM, JEVFJ) > Convert3PartsDateToInt(PBFEA, PBFEM, PBFEJ))
                    {
                        //Création d'un enregistrement dans KPCTRL
                        InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JERSQ, "", "Incohérence Date Fin de garantie/Fin Effet", "B", user);
                    }
                }
                if (dateDebutYPOBASERenseigne != null && dateDebutYPOBASERenseigne == true)
                {
                    if (Convert3PartsDateToInt(JEVFA, JEVFM, JEVFJ) < Convert3PartsDateToInt(PBEFA, PBEFM, PBEFJ))
                    {
                        //Création d'un enregistrement dans KPCTRL
                        InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JERSQ, "", "Incohérence Date Fin de garantie/Début Effet", "B", user);
                    }
                }
            }
            var offreEntete = ObtenirOffreEnteteDto(codeOffre, version);

            if (offreEntete != null)
            {
                string JDCNA = offreEntete.SoumisCatnat;
                string JDINA = offreEntete.Indexation;
                //Catnat
                if (JECNA.Equals("O"))
                {
                    if (JDCNA.Equals("N"))
                    {
                        //Création d'un enregistrement dans KPCTRL
                        InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JERSQ, "", "Incohérence info CATNAT  Risque/Base", "B", user);
                    }
                }
                //Indexation
                if (JEINA.Equals("O"))
                {
                    if (JDINA.Equals("N"))
                    {
                        //Création d'un enregistrement dans KPCTRL
                        InsertionKPCTRL(type, codeOffre, version, "RSQ", "30", "RSQ", JERSQ, "", "Incohérence info indexation  Risque/Base", "B", user);
                    }
                }
            }

            var objets = ObtenirMultiObjet(codeOffre, version, type, JERSQ);
            if (objets.Any())
            {
                foreach (var objet in objets)
                {
                    string JGOBJ = objet.CodeObjet.ToString();
                    string JGVDA = objet.EntreeGarantieAnnee.ToString();
                    string JGVDM = objet.EntreeGarantieMois.ToString();
                    string JGVDJ = objet.EntreeGarantieJour.ToString();
                    string JGRSQ = objet.CodeRisque.ToString();
                    string JGVFA = objet.SortieGarantieAnnee.ToString();
                    string JGVFM = objet.SortieGarantieMois.ToString();
                    string JGVFJ = objet.SortieGarantieJour.ToString();
                    string JGCNA = objet.CATNAT;
                    //Date Début
                    if (DateRenseignee(JEVDA, JEVDM, JEVDJ))
                    {
                        if (DateRenseignee(JGVDA, JGVDM, JGVDJ))
                        {
                            if (Convert3PartsDateToInt(JEVDA, JEVDM, JEVDJ) < Convert3PartsDateToInt(JEVDA, JEVDM, JEVDJ))
                            {
                                //Création d'un enregistrement dans KPCTRL
                                InsertionKPCTRL(type, codeOffre, version, "OBJ", "40", "OBJ", JGRSQ, JGOBJ, "Incohérence Date début de garantie : Objet/Risque", "B", user);
                            }
                        }
                        if (DateRenseignee(JEVFA, JEVFM, JEVFJ))
                        {
                            if (Convert3PartsDateToInt(JGVDA, JGVDM, JGVDJ) > Convert3PartsDateToInt(JEVFA, JEVFM, JEVFJ))
                            {
                                //Création d'un enregistrement dans KPCTRL
                                InsertionKPCTRL(type, codeOffre, version, "OBJ", "40", "OBJ", JGRSQ, JGOBJ, "Incohérence Date début de garantie : Objet/Risque", "B", user);
                            }
                        }
                    }
                    //Date Fin
                    if (DateRenseignee(JEVFA, JEVFM, JEVFJ))
                    {
                        if (DateRenseignee(JGVFA, JGVFM, JGVFJ))
                        {
                            if (Convert3PartsDateToInt(JGVFA, JGVFM, JGVFJ) > Convert3PartsDateToInt(JEVFA, JEVFM, JEVFJ))
                            {
                                //Création d'un enregistrement dans KPCTRL
                                InsertionKPCTRL(type, codeOffre, version, "OBJ", "40", "OBJ", JGRSQ, JGOBJ, "Incohérence Date fin de garantie : Objet/Risque", "B", user);
                            }
                            if (DateRenseignee(JEVDA, JEVDM, JEVDJ))
                            {
                                if (Convert3PartsDateToInt(JGVFA, JGVFM, JGVFJ) < Convert3PartsDateToInt(JEVDA, JEVDM, JEVDJ))
                                {
                                    //Création d'un enregistrement dans KPCTRL
                                    InsertionKPCTRL(type, codeOffre, version, "OBJ", "40", "OBJ", JGRSQ, JGOBJ, "Incohérence Date fin de garantie : Objet/Risque", "B", user);
                                }
                            }
                        }
                    }
                    offreEntete = ObtenirOffreEnteteDto(codeOffre, version);
                    if (offreEntete != null)
                    {
                        string JDCNA = offreEntete.SoumisCatnat;
                        string JDINA = offreEntete.Indexation;
                        //Catnat
                        if (JECNA.Equals("O"))
                        {
                            if (JDCNA.Equals("N") || JGCNA.Equals("N"))
                            {
                                //Création d'un enregistrement dans KPCTRL
                                InsertionKPCTRL(type, codeOffre, version, "OBJ", "40", "OBJ", JGRSQ, JGOBJ, "Incohérence info CATNAT  Risque/Base", "B", user);
                            }
                        }
                        //Indexation
                        if (JEINA.Equals("O"))
                        {
                            if (JDINA.Equals("N") || JGCNA.Equals("N"))
                            {
                                //Création d'un enregistrement dans KPCTRL
                                InsertionKPCTRL(type, codeOffre, version, "OBJ", "40", "OBJ", JGRSQ, JGOBJ, "Incohérence info indexation  Risque/Base", "B", user);
                            }
                        }
                    }
                }
            }
        }

        private static void CheckModifDetailGaranties(string codeOffre, string version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CHDETGA", param);
        }
        #endregion

        public void RemoveEtapeCOT(Folder folder) {
            RemoveEtape(folder, "COT");
        }

        public void RemoveEtape(Folder folder, string etape) {
            var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = PurgeCtrlEtapes
            };
            options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, etape);
            options.Exec();
        }

        public int SelectNbCtrlModif(Folder folder) {
            var options = new DbCountOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = CountCtrlModif
            };
            options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
            options.PerformCount();
            return options.Count;
        }

        public void CreateEtapeDateAvenant(Folder folder, string user, DateTime now) {
            var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = AddCtrlDateAvn
            };
            options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, user, now.ToIntYMD());
            options.Exec();
        }
    }
}
