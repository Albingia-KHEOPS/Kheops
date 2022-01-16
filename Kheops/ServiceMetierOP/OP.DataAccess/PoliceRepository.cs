using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using DataAccess.Helpers;
using OP.DataAccess.Data;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.Ecran.ModifierOffre;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.LTA;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Personnes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OP.DataAccess
{
    public class PoliceRepository : RepositoryBase
    {
        internal static readonly string SelectDateEffetEtFrais = $@"SELECT JDATT ATT , JDAFC AFC , JDAFR FRAIS , 
        {OutilsHelper.MakeCastTimestamp("PBEF")} DATE 
        FROM YPOBASE 
        INNER JOIN YPRTENT ON ( JDIPB , JDALX ) = ( PBIPB , PBALX ) 
        AND ( PBIPB , PBALX , PBTYP ) = ( :IPB , :ALX , :TYP ) ;";
        internal static readonly string SelectTraitement = "SELECT PBTTR FROM YPOBASE WHERE ( PBIPB , PBALX , PBTYP , PBAVN ) = ( :IPB , :ALX , :TYP , :AVN ) ;";
        internal static readonly string SelectSbrCategoContract = "SELECT PBSBR, PBCAT FROM YPOBASE WHERE (PBIPB, PBALX, PBTYP) = (:IPB, :ALX, :TYP);";
        internal static readonly string SelectFraisAcc = $@"SELECT JDATT ATT, JDAFC AFC, CASE WHEN IFNULL(PKIPB, '') = '' THEN JDAFR ELSE PKAFR END FRAIS , 
        {OutilsHelper.MakeCastTimestamp("PBEF")} DATE ,
        IFNULL(PKIPB,'') HASPRIPES, PKATM ATM
        FROM YPOBASE
        INNER JOIN YPRTENT ON (JDIPB, JDALX) = (PBIPB, PBALX)
        INNER JOIN YPRIPES ON (PKIPB, PKALX, PKAVN) = (PBIPB, PBALX, PBAVN)
        WHERE (PBIPB, PBALX, PBTYP) = (:IPB, :ALX, :TYP)";

        public PoliceRepository(IDbConnection connection) : base(connection) { }

        public DateEffetEtFraisData GetDateEffetEtFrais(Folder folder)
        {
            return Fetch<DateEffetEtFraisData>(SelectDateEffetEtFrais, folder.CodeOffre.ToIPB(), folder.Version, folder.Type)?.FirstOrDefault();
        }

        public (string, string) GetSousBrancheCategorieContract(Folder folder)
        {
            return Fetch<(string, string)>(SelectSbrCategoContract, folder.CodeOffre.ToIPB(), folder.Version, folder.Type).FirstOrDefault();
        }

        public DateEffetEtFraisData GetFraisAccessoires(Folder folder)
        {
            return Fetch<DateEffetEtFraisData>(SelectFraisAcc, folder.CodeOffre.ToIPB(), folder.Version, folder.Type)?.FirstOrDefault();
        }

        public string GetTraitement(Folder folder)
        {
            using (var options = new DbSelectStringsOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SelectTraitement
            })
            {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant);
                options.PerformSelect();
                return options.StringList?.FirstOrDefault() ?? string.Empty;
            }
        }

        public static OffreDto Initialiser(DataRow ligne, bool fromRecherche = false)
        {
            OffreDto offre = null;
            if (OutilsHelper.ContientLeChampEtEstNonNull(ligne, "PBIPB"))
            {
                offre = new OffreDto();

                if (ligne.Table.Columns.Contains("PBIPB")) { offre.CodeOffre = ligne["PBIPB"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("PBREF")) { offre.Descriptif = ligne["PBREF"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("PBIN5")) { offre.Interlocuteur = InterlocuteurRepository.Initialiser(ligne); }
                if (ligne.Table.Columns.Contains("PBOCT")) { offre.RefCourtier = ligne["PBOCT"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("PBETA")) { offre.Etat = ligne["PBETA"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("ETATLIB")) { offre.EtatLib = ligne["ETATLIB"].ToString(); };
                if (ligne.Table.Columns.Contains("PBSIT")) { offre.Situation = ligne["PBSIT"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("SITLIB")) { offre.SituationLib = ligne["SITLIB"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("PBSTF")) { offre.MotifRefus = (ligne["PBSTF"].ToString().Trim() != "-") ? ligne["PBSTF"].ToString().Trim() : string.Empty; };
                if (ligne.Table.Columns.Contains("PBALX")) { offre.Version = int.Parse(ligne["PBALX"].ToString()); };
                if (ligne.Table.Columns.Contains("PBSTQ")) { offre.Qualite = ligne["PBSTQ"].ToString(); };
                if (ligne.Table.Columns.Contains("QUALITELIB")) { offre.QualiteLib = ligne["QUALITELIB"].ToString(); };
                if (ligne.Table.Columns.Contains("PBTYP")) { offre.Type = ligne["PBTYP"].ToString(); };
                if (ligne.Table.Columns.Contains("PBMO1")) { offre.MotCle1 = ligne["PBMO1"].ToString(); };
                if (ligne.Table.Columns.Contains("PBMO2")) { offre.MotCle2 = ligne["PBMO2"].ToString(); };
                if (ligne.Table.Columns.Contains("PBMO3")) { offre.MotCle3 = ligne["PBMO3"].ToString(); };
                if (ligne.Table.Columns.Contains("KAJOBSV")) { offre.Observation = ligne["KAJOBSV"].ToString(); };
                if (ligne.Table.Columns.Contains("PBMER")) { offre.ContratMere = ligne["PBMER"].ToString(); };
                if (ligne.Table.Columns.Contains("PBTTR")) { offre.TypeAvt = ligne["PBTTR"].ToString(); };
                if (ligne.Table.Columns.Contains("PBTAC")) { offre.TypeAccord = ligne["PBTAC"].ToString(); };
                if (ligne.Table.Columns.Contains("PBORK")) { offre.KheopsStatut = ligne["PBORK"].ToString(); };
                if (ligne.Table.Columns.Contains("GENERDOC")) { offre.GenerDoc = int.Parse(ligne["GENERDOC"].ToString()); };

                int numAvn = 0;
                if (ligne.Table.Columns.Contains("PBAVN")) { offre.NumAvenant = (int.TryParse(ligne["PBAVN"].ToString(), out numAvn) ? numAvn : 0); };
                int numAvnExt = 0;
                if (ligne.Table.Columns.Contains("PBAVK")) { offre.NumAvnExterne = (int.TryParse(ligne["PBAVK"].ToString(), out numAvnExt) ? numAvnExt : 0); };

                int idAdr = 0;
                if (ligne.Table.Columns.Contains("PBADH")) { offre.IdAdresseOffre = (int.TryParse(ligne["PBADH"].ToString(), out idAdr) ? idAdr : 0); };
                AjouterElementAssure(InitialiserElementAssure(ligne), offre);
                if (fromRecherche)
                {
                    offre.CabinetGestionnaire = CabinetCourtageRepository.Initialiser(ligne, "COURT_");
                    offre.CabinetApporteur = CabinetCourtageRepository.Initialiser(ligne, "COURT_");
                    offre.PreneurAssurance = AssureRepository.Initialiser(ligne, "ASSU_");
                }
                else
                {
                    offre.CabinetGestionnaire = CabinetCourtageRepository.Initialiser(ligne);
                    offre.CabinetApporteur = CabinetCourtageRepository.Initialiser(ligne);
                    offre.CabinetAutres = AlimenteAutreDoubleSaisie(offre.CodeOffre, offre.Version.ToString());
                    offre.PreneurAssurance = AssureRepository.Initialiser(ligne);
                }

                offre.Aperiteur = AperiteurRepository.Initialiser(ligne);
                offre.Interlocuteur = InterlocuteurRepository.Initialiser(ligne);
                offre.Gestionnaire = GestionnaireRepository.Initialiser(ligne);
                offre.DateSaisie = ObtenirDateSaisie(ligne);
                offre.DateEnregistrement = ObtenirDateEnregistrement(ligne);
                offre.EffetGarantie = ObtenirDateEffet(ligne);
                offre.DateEffetGarantie = ObtenirDateEffet(ligne);
                offre.DateFinEffetGarantie = ObtenirDateFinEffet(ligne);
                offre.DateCreation = ObtenirDateCreation(ligne);
                offre.DateMAJ = ObtenirDateMaJ(ligne);
                offre.Branche = BrancheRepository.Initialiser(ligne);
                offre.Souscripteur = SouscripteurRepository.Initialiser(ligne);
                AjouterElementAssure(InitialiserElementAssure(ligne), offre);

                offre.Devise = ReferenceRepository.InitialiserDevise(ligne);
                offre.Periodicite = ReferenceRepository.InitialiserPeriodicite(ligne);
                offre.EcheancePrincipale = ObtenirDateEchangePrincipale(ligne);
                if (ligne.Table.Columns.Contains("PBCTD"))
                {
                    offre.DureeGarantie = int.Parse(ligne["PBCTD"].ToString());
                }

                offre.UniteDeTemps = ReferenceRepository.InitialiserUniteTemps(ligne);
                offre.IndiceReference = ReferenceRepository.InitialiserIndiceReference(ligne);
                offre.NatureContrat = ReferenceRepository.InitialiserNatureContrat(ligne);

                //offre.

                if (ligne.Table.Columns.Contains("JDIVA"))
                {
                    string texte = ligne["JDIVA"].ToString();
                    if (!String.IsNullOrEmpty(texte))
                    {
                        offre.Valeur = decimal.Parse(texte);
                    }
                }

                if (ligne.Table.Columns.Contains("PBAPP"))
                {
                    string texte = ligne["PBAPP"].ToString();
                    if (!String.IsNullOrEmpty(texte))
                    {
                        offre.PartAlbingia = decimal.Parse(texte);
                    }
                }

                if (ligne.Table.Columns.Contains("PBPCV"))
                {
                    string texte = ligne["PBPCV"].ToString();
                    if (!String.IsNullOrEmpty(texte))
                    {
                        offre.Couverture = int.Parse(texte);
                    }
                }

                if (ligne.Table.Columns.Contains("PHTXF"))
                {
                    string texte = ligne["PHTXF"].ToString();
                    if (!String.IsNullOrEmpty(texte))
                    {
                        offre.FraisAperition = decimal.Parse(texte);
                    }
                }

                if (ligne.Table.Columns.Contains("JDITC"))
                {
                    string intercalaireExiste = ligne["JDITC"].ToString();
                    if (intercalaireExiste == "O")
                    {
                        offre.IntercalaireCourtier = true;
                    }
                    else
                    {
                        offre.IntercalaireCourtier = false;
                    }
                }
                #region Bandeau
                if (ligne.Table.Columns.Contains("PBRGT")) { offre.CodeRegime = ligne["PBRGT"].ToString(); };
                //if (ligne.Table.Columns.Contains("REGIMLIB")) { offre.LibelleRegime = ligne["REGIMLIB"].ToString(); };
                if (ligne.Table.Columns.Contains("JDCNA")) { offre.SoumisCatNat = ligne["JDCNA"].ToString(); };
                //if (ligne.Table.Columns.Contains("JDTFF") && ligne.Table.Columns.Contains("JDTMC"))
                //{
                //    string jdtff = ligne["JDTFF"].ToString();
                //    string jdtmc = ligne["JDTMC"].ToString();
                //    if (!string.IsNullOrEmpty(jdtff) && !string.IsNullOrEmpty(jdtmc))
                //        offre.MontantReference = decimal.Parse(jdtff) != 0 ? decimal.Parse(jdtff) : decimal.Parse(jdtmc);
                //};
                //if (ligne.Table.Columns.Contains("JDINA")) { offre.Indexation = ligne["JDINA"].ToString(); };
                //if (ligne.Table.Columns.Contains("JDIXL")) { offre.LCI = ligne["JDIXL"].ToString(); };
                //if (ligne.Table.Columns.Contains("JDIXC")) { offre.Assiette = ligne["JDIXC"].ToString(); };
                //if (ligne.Table.Columns.Contains("JDIXF")) { offre.Franchise = ligne["JDIXF"].ToString(); };
                //if (ligne.Table.Columns.Contains("JDDPV")) { offre.Preavis = int.Parse(ligne["JDDPV"].ToString()); };
                //if (ligne.Table.Columns.Contains("PBTTR")) { offre.CodeAction = ligne["PBTTR"].ToString(); };
                //if (ligne.Table.Columns.Contains("TRLIB")) { offre.LibelleAction = ligne["TRLIB"].ToString(); };
                //if (ligne.Table.Columns.Contains("SITLIB")) { offre.LibelleSituation = ligne["SITLIB"].ToString(); };
                //if (ligne.Table.Columns.Contains("PBSTM")) { offre.DateSituationMois = int.Parse(ligne["PBSTM"].ToString()); };
                //if (ligne.Table.Columns.Contains("PBSTA")) { offre.DateSituationAnnee = int.Parse(ligne["PBSTA"].ToString()); };
                //if (ligne.Table.Columns.Contains("PBDEU")) { offre.CodeUsrCreateur = ligne["PBDEU"].ToString(); };
                //if (ligne.Table.Columns.Contains("UCRNOM")) { offre.NomUsrCreateur = ligne["UCRNOM"].ToString(); };
                //if (ligne.Table.Columns.Contains("PBMJU")) { offre.CodeUsrModificateur = ligne["PBMJU"].ToString(); };
                //if (ligne.Table.Columns.Contains("UUPNOM")) { offre.NomUsrModificateur = ligne["UUPNOM"].ToString(); };
                #endregion
            }

            return offre;
        }

        public static ElementAssureContratDto InitialiserElementAssure(DataRow ligne)
        {
            ElementAssureContratDto elementAssure = new ElementAssureContratDto();
            elementAssure.ElementPrincipal = true;
            elementAssure.Adresse = AdresseRepository.Initialiser(ligne);
            return elementAssure;
        }

        public static bool IsEnteteContainAddress(string codeOffre, string version, string type, string codeAvn)
        {
            string sql = @"SELECT ABPCHR FROM YPOBASE
                            INNER JOIN YADRESS ON ABPCHR = PBADH
                            WHERE PBIPB = :CODEOFFRE AND PBALX = :VERSION AND PBTYP = :TYPE AND PBAVN = :CODEAVN AND PBADH <> 0;";

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("VERSION", DbType.Int32);
            param[1].Value = int.Parse(version);
            param[2] = new EacParameter("TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("CODEAVN", DbType.Int32);
            param[3].Value = int.Parse(codeAvn);


            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            return result != null && result.Any() ? true : false;
        }

        public static OffreDto Obtenir(string offreId, int? offreVersion = null, string offreType = "O")
        {
            OffreDto offre = new OffreDto();

            string sql = @"SELECT *
                        FROM V_INFOENTETEOFFRE  
                        WHERE CODEOFFRE = :codeOffre AND VERSIONOFFRE = :version AND TYPEOFFRE = :type";

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = offreId.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = offreVersion.HasValue ? offreVersion.Value : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = offreType;

            var offrePlatDto = DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (offrePlatDto != null)
                offre = GetOffre(offrePlatDto);

            offre.NbAssuresAdditionnels = GetNbAssuAdd(offreId, offreVersion.HasValue ? offreVersion.Value : 0, offreType);

            return offre;
        }

        public static int insertKP(int id, string value, string codeOffre, int version, string type, string table)
        {
            switch (table)
            {
                case "KPOBSV":
                    id = CommonRepository.GetAS400Id("KAJCHR");

                    string sqlInsertObsvKPOBSV = @"INSERT INTO KPOBSV (KAJCHR, KAJTYP, KAJIPB, KAJALX, KAJOBSV) 
                                                                                VALUES (:P_IDOBSV, :P_TYPE, :P_CODECONTRAT, :P_VERSION, :P_OBS)";

                    EacParameter[] paramKPOBSV = new EacParameter[5];
                    paramKPOBSV[0] = new EacParameter("P_IDOBSV", DbType.Int64);
                    paramKPOBSV[0].Value = id;
                    paramKPOBSV[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    paramKPOBSV[1].Value = type;
                    paramKPOBSV[2] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    paramKPOBSV[2].Value = codeOffre;
                    paramKPOBSV[3] = new EacParameter("P_VERSION", DbType.Int64);
                    paramKPOBSV[3].Value = version;
                    paramKPOBSV[4] = new EacParameter("P_OBS", DbType.AnsiStringFixedLength);
                    paramKPOBSV[4].Value = value;

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsertObsvKPOBSV, paramKPOBSV);
                    break;

                case "KPDESI":
                    id = CommonRepository.GetAS400Id("KADCHR");

                    string sqlInsertObsvKPDESI = @"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADDESI) 
                                                                                VALUES (:P_IDOBSV, :P_TYPE, :P_CODECONTRAT, :P_VERSION, :P_OBS)";

                    EacParameter[] paramKPDESI = new EacParameter[5];
                    paramKPDESI[0] = new EacParameter("P_IDOBSV", DbType.Int64);
                    paramKPDESI[0].Value = id;
                    paramKPDESI[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    paramKPDESI[1].Value = type;
                    paramKPDESI[2] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    paramKPDESI[2].Value = codeOffre;
                    paramKPDESI[3] = new EacParameter("P_VERSION", DbType.Int64);
                    paramKPDESI[3].Value = version;
                    paramKPDESI[4] = new EacParameter("P_OBS", DbType.AnsiStringFixedLength);
                    paramKPDESI[4].Value = value;

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsertObsvKPDESI, paramKPDESI);
                    break;

            }
            return id;
        }

        /*A modifier pour créer une requête de modification KPENT & KPOBSV*/
        public static void updateKP(int id, string value, string table)
        {
            string sql = $@"UPDATE {table}
                            SET {(table == "KPDESI" ? "KADDESI" : "KAJOBSV")} = :observation
                            WHERE {(table == "KPDESI" ? "KADCHR" : "KAJCHR")} = :numObsv";

            EacParameter[] paramObsv = new EacParameter[2];
            paramObsv[0] = new EacParameter("observation", DbType.AnsiStringFixedLength);
            paramObsv[0].Value = value;
            paramObsv[1] = new EacParameter("numObsv", DbType.Int32);
            paramObsv[1].Value = id;
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramObsv);
        }

        /*A modifier pour créer une requête de modification KPENT & KPOBSV*/
        public static void insertOrUpdateKpdesi(int id, string value, string codeOffre, int version, string type, string mode)
        {
            switch (mode)
            {
                case "insert":
                    string sqlInsertObsvRefKPDESI = @"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADPERI, KADRSQ, KADOBJ, KADDESI) 
                                                        VALUES (:P_ID, :P_TYPE, :P_CODECONTRAT, :P_VERSION, :P_PERI, :P_RSQ, :P_OBJ, :P_DESIOBS);";

                    EacParameter[] paramInsertKPDESI = new EacParameter[8];
                    paramInsertKPDESI[0] = new EacParameter("P_ID", DbType.Int64);
                    paramInsertKPDESI[0].Value = id;
                    paramInsertKPDESI[1] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    paramInsertKPDESI[1].Value = type;
                    paramInsertKPDESI[2] = new EacParameter("P_CODECONTRAT", DbType.AnsiStringFixedLength);
                    paramInsertKPDESI[2].Value = codeOffre;
                    paramInsertKPDESI[3] = new EacParameter("P_VERSION", DbType.Int64);
                    paramInsertKPDESI[3].Value = version;
                    paramInsertKPDESI[4] = new EacParameter("P_PERI", DbType.AnsiStringFixedLength);
                    paramInsertKPDESI[4].Value = "";
                    paramInsertKPDESI[5] = new EacParameter("P_RSQ", DbType.AnsiStringFixedLength);
                    paramInsertKPDESI[5].Value = 0;
                    paramInsertKPDESI[6] = new EacParameter("P_OBJ", DbType.Int64);
                    paramInsertKPDESI[6].Value = 0;
                    paramInsertKPDESI[7] = new EacParameter("P_DESIOBS", DbType.AnsiStringFixedLength);
                    paramInsertKPDESI[7].Value = value;

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsertObsvRefKPDESI, paramInsertKPDESI);
                    break;
                case "update":
                    string sql = @"UPDATE KPDESI
                            SET KADDESI = :observation
                            WHERE KADIPB = :codeP AND KADTYP = :typeP AND KADALX = :versionP AND KADCHR = :numObsv";

                    EacParameter[] paramObsv = new EacParameter[5];
                    paramObsv[0] = new EacParameter("observation", DbType.AnsiStringFixedLength);
                    paramObsv[0].Value = value;
                    paramObsv[1] = new EacParameter("codeP", DbType.AnsiStringFixedLength);
                    paramObsv[1].Value = codeOffre.PadLeft(9, ' ');
                    paramObsv[2] = new EacParameter("typeP", DbType.AnsiStringFixedLength);
                    paramObsv[2].Value = type;
                    paramObsv[3] = new EacParameter("versionP", DbType.Int32);
                    paramObsv[3].Value = version;
                    paramObsv[4] = new EacParameter("numObsv", DbType.Int32);
                    paramObsv[4].Value = id;
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramObsv);
                    break;
            }

        }


        public static int traitementObsv(int id, string obsv, string table, string codeOffre, int version, string type)
        {
            if (id == 0)
            {
                id = insertKP(id, obsv, codeOffre, version, type, table);
            }
            else
            {
                updateKP(id, obsv, table);
            }
            return id;
        }

        public static void UpdateObservations(string codeOffre, string type, int version, string obsvInfoGen, string obsvCotisation, string obsvEngagement, string obsvMntRef, string obsvRefGest)
        {
            #region Récuperation d'id des observations
            string idObsv = $@"SELECT DISTINCT KAAOBSV as OBSVGEN, KAAOBSC as OBSVCOMM, IFNULL(IFNULL(h.KDOID , k.KDOID), -1) as IDENG, IFNULL(IFNULL(h.KDOOBSV , k.KDOOBSV), -1) as OBSVENG, KAAOBSF as OBSVMNTREF, KAAAVDS as OBSVREFGEST   
                                    FROM KPENT k2 
                                    LEFT JOIN KPENG k ON k2.KAATYP = k.KDOTYP AND k2.KAAIPB = K.KDOIPB AND k2.KAAALX = k.KDOALX
                                    LEFT JOIN HPENG h ON h.KDOTYP = k2.KAATYP AND h.KDOALX = k2.KAAALX AND h.KDOIPB = k2.KAAIPB AND h.KDOAVN = 0
                                    WHERE KAAIPB = :codeOffreObsv AND KAATYP = :typeObsv AND KAAALX = :versionObsv";

            EacParameter[] numObsvParam = new EacParameter[3];
            numObsvParam[0] = new EacParameter("codeOffreObsv", DbType.AnsiStringFixedLength);
            numObsvParam[0].Value = codeOffre.PadLeft(9, ' ');
            numObsvParam[1] = new EacParameter("typeObsv", DbType.AnsiStringFixedLength);
            numObsvParam[1].Value = type;
            numObsvParam[2] = new EacParameter("versionObsv", DbType.Int32);
            numObsvParam[2].Value = version;

            var numObsvTest = DbBase.Settings.ExecuteList<VisuObservationsDto>(CommandType.Text, idObsv, numObsvParam);

            var testObsvInfoGen = int.Parse(numObsvTest[0].ObsvInfoGen);
            var testObsvCotisation = int.Parse(numObsvTest[0].ObsvCotisation);
            var testObsvEngagement = int.Parse(numObsvTest[0].ObsvEngagement);
            var testObsvMntRef = int.Parse(numObsvTest[0].ObsvMntRef);
            var testObsvRefGest = int.Parse(numObsvTest[0].ObsvRefGest);
            var testIdEngagement = int.Parse(numObsvTest[0].IdEng);
            #endregion

            #region Insert observation
            if (numObsvTest.Any())
            {
                testObsvInfoGen = traitementObsv(testObsvInfoGen, obsvInfoGen, "KPOBSV", codeOffre, version, type);
                testObsvCotisation = traitementObsv(testObsvCotisation, obsvCotisation, "KPOBSV", codeOffre, version, type);
                testObsvMntRef = traitementObsv(testObsvMntRef, obsvMntRef, "KPOBSV", codeOffre, version, type);
                testObsvRefGest = traitementObsv(testObsvRefGest, obsvRefGest, "KPDESI", codeOffre, version, type);

                //traitement KPENT
                string sqlUpdateObsvKPENT = @"UPDATE KPENT SET KAAOBSV = :P_IDINFOGENE, KAAOBSC = :P_IDCOTIS, KAAAND = :P_IDENG, KAAOBSF = :P_IDMNTREF, KAAAVDS = :P_IDREFGEST
                                                    WHERE KAAIPB = :P_IDCONTRAT";

                EacParameter[] paramKPENT = new EacParameter[6];
                paramKPENT[0] = new EacParameter("P_IDINFOGENE", DbType.Int64);
                paramKPENT[0].Value = testObsvInfoGen;
                paramKPENT[1] = new EacParameter("P_IDCOTIS", DbType.Int64);
                paramKPENT[1].Value = testObsvCotisation;
                paramKPENT[2] = new EacParameter("P_IDENG", DbType.Int64);
                paramKPENT[2].Value = testIdEngagement;
                paramKPENT[3] = new EacParameter("P_IDMNTREF", DbType.Int64);
                paramKPENT[3].Value = testObsvMntRef;
                paramKPENT[4] = new EacParameter("P_IDREFGEST", DbType.Int64);
                paramKPENT[4].Value = testObsvRefGest;
                paramKPENT[5] = new EacParameter("P_IDCONTRAT", DbType.Int64);
                paramKPENT[5].Value = codeOffre;

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateObsvKPENT, paramKPENT);

                if (testObsvEngagement != -1)
                {
                    testObsvEngagement = traitementObsv(testObsvEngagement, obsvEngagement, "KPOBSV", codeOffre, version, type);

                    //traitement KPENG
                    string sqlUpdateObsvKPENG = @"UPDATE KPENG SET KDOOBSV = :P_IDOBSVENG
                                                    WHERE KDOID = :P_IDENG";

                    EacParameter[] paramKPENG = new EacParameter[2];
                    paramKPENG[0] = new EacParameter("P_IDOBSVENG", DbType.Int64);
                    paramKPENG[0].Value = testObsvEngagement;
                    paramKPENG[1] = new EacParameter("P_IDENG", DbType.Int64);
                    paramKPENG[1].Value = testIdEngagement;

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateObsvKPENG, paramKPENG);
                }
            }
            #endregion
        }

        private static Int64 GetNbAssuAdd(string codeOffre, int version, string type)
        {
            string sql = @"SELECT COUNT (*) NBLIGN 
                        FROM YPOBASE 
                            INNER JOIN YPOASSU ON PBIPB = PCIPB AND PBALX = PCALX AND PBTYP = PCTYP AND PBIAS <> PCIAS 
                        WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            return result != null && result.Any() ? result.FirstOrDefault().NbLigne : 0;
        }
        public static List<RisqueDto> ObtenirIDRisques(string offreId, int? offreversion)
        {
            List<RisqueDto> toReturn = new List<RisqueDto>();

            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("offreId", DbType.AnsiStringFixedLength);
            param[0].Value = offreId.PadLeft(9, ' ');
            param[1] = new EacParameter("offreversion", DbType.Int32);
            param[1].Value = offreversion;
            string sql = @"SELECT JERSQ AS CODE FROM YPRTRSQ WHERE JEIPB= :offreId AND JEALX= :offreversion";

            toReturn = DbBase.Settings.ExecuteList<RisqueDto>(CommandType.Text, sql, param);
            return toReturn;
        }

        public static List<RisqueDto> ObtenirRisques(ModeConsultation modeNavig, string offreId, int? offreVersion = null, string type = "O", string codeAvn = "")
        {
            List<RisqueDto> toReturn = new List<RisqueDto>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"
SELECT TBLRISQUE.JERSQ CODE, KABDESC DESCRIPTIF, KABCIBLE CODECIBLE, CB.KAHDESC LIBELLECIBLE, KABDESI CHRONODESI, KADDESI LIBELLERISQUE,
    TBLRISQUE.JEVDJ ENTREEGARANTIEJOUR, TBLRISQUE.JEVDM ENTREEGARANTIEMOIS, TBLRISQUE.JEVDA ENTREEGARANTIEANNEE, TBLRISQUE.JEVDH ENTREEGARANTIEHEURE,
    TBLRISQUE.JEVFJ SORTIEGARANTIEJOUR, TBLRISQUE.JEVFM SORTIEGARANTIEMOIS,TBLRISQUE.JEVFA SORTIEGARANTIEANNEE, TBLRISQUE.JEVFH SORTIEGARANTIEHEURE,
    TBLRISQUEHISTO.JEVFJ SORTIEGARANTIEJOURHISTO, TBLRISQUEHISTO.JEVFM SORTIEGARANTIEMOISHISTO,TBLRISQUEHISTO.JEVFA SORTIEGARANTIEANNEEHISTO, TBLRISQUEHISTO.JEVFH SORTIEGARANTIEHEUREHISTO,
    TBLRISQUE.JEVAL VALEUR, TBLRISQUE.JEVAU CODEUNITE, TBLRISQUE.JEVAT CODETYPE, TBLRISQUE.JEVAH VALEURHT, TBLRISQUE.JEOBJ CODEOBJET, TBLRISQUE.JEINA ISRISQUEINDEXE,
    TBLRISQUE.JEIXL ISLCI, TBLRISQUE.JEIXF ISFRANCHISE, TBLRISQUE.JEIXC ISASSIETTE, TBLRISQUE.JERGT REGIMETAXE, TBLRISQUE.JECNA ISCATNAT, JFADH IDADRESSERISQUE, KABAPE CODEAPE, 
    KABNMC01 NOMENCLATURE1,KABNMC02 NOMENCLATURE2,KABNMC03 NOMENCLATURE3,KABNMC04 NOMENCLATURE4,KABNMC05 NOMENCLATURE5,
    TBLRISQUE.JETRR TERRITORIALITE, KABTRE CODETRE, TPLIB LIBTRE, KABCLASS CODECLASSE, KABMAND DATEDEBDESC, KABMANF DATEFINDESC,
    KABMANDH HEUREDEBDESC, KABMANFH HEUREFINDESC,
    TBLRISQUE.JEAVJ EFFETAVNJOUR, TBLRISQUE.JEAVM EFFETAVNMOIS, TBLRISQUE.JEAVA EFFETAVNANNEE,
    TBLRISQUE.JEAVE AVNCREATION, TBLRISQUE.JEAVF AVNMODIF,
    JDINA INDEXOFFRE,
    TBLRISQUE.JEPBT TAUXAPPEL, TBLRISQUE.JERUL ISREGUL, TBLRISQUE.JERUT TYPEREGUL,
    TBLRISQUE.JEPBN PARTBENEFRSQ, TBLRISQUE.JEPBN PARTBENEF, TBLRISQUE.JEPBA NBYEAR, TBLRISQUE.JEPBR RISTOURNE, TBLRISQUE.JEPBP COTISRET, TBLRISQUE.JEPBS SEUIL, TBLRISQUE.JEPBC TAUXCOMP,
    TBLRISQUE.JETEM ISRISQUETEMP,
    KABBRNT TAUXMAXI, KABBRNC PRIMEMAXI
FROM {0} 
    LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
    LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
    LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
    LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
    LEFT JOIN {3} ON KABDESI = KADCHR
    LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
    LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1);

            var listRisquePlatDto = DbBase.Settings.ExecuteList<RisquePlatDto>(CommandType.Text, sql);
            if (listRisquePlatDto != null && listRisquePlatDto.Any())
            {
                foreach (var risquePlatDto in listRisquePlatDto)
                {
                    RisqueDto risque = GetRisque(modeNavig, risquePlatDto, offreId, type, offreVersion, codeAvn);
                    if (risque.IdAdresseRisque > 0)
                    {
                        risque.AdresseRisque = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
                        risque.Objets = ObtenirObjet(modeNavig, offreId, risque.Code, type, offreVersion);
                    }
                    toReturn.Add(risque);
                }
            }
            return toReturn;
        }
        /// <summary>
        /// Récupération informations risque 
        /// Code , Cible et le cible au niveau de l'objet
        /// </summary>
        /// <param name="modeNavig"></param>
        /// <param name="offreId"></param>
        /// <param name="offreVersion"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <returns></returns>
        public static List<RisqueDto> GetRisquesBaseInfos(ModeConsultation modeNavig, string offreId, int? offreVersion = null, string type = "O", string codeAvn = "")
        {
            List<RisqueDto> toReturn = new List<RisqueDto>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"
SELECT TBLRISQUE.JERSQ CODE, KABCIBLE CODECIBLE
FROM {0} 
    LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
    LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
    LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
    LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
    LEFT JOIN {3} ON KABDESI = KADCHR
    LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
    LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1);

            var listRisquePlatDto = DbBase.Settings.ExecuteList<RisquePlatDto>(CommandType.Text, sql);
            if (listRisquePlatDto != null && listRisquePlatDto.Any())
            {
                foreach (var risquePlatDto in listRisquePlatDto)
                {
                    var risque = new RisqueDto { Code = risquePlatDto.Code, Cible = new CibleDto { Code = risquePlatDto.CodeCible } };
                    //if (risque.IdAdresseRisque > 0)
                    //{
                    //    risque.AdresseRisque = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
                    risque.Objets = GetObjetBaseInfos(modeNavig, offreId, risque.Code, type, offreVersion);
                    // }
                    toReturn.Add(risque);
                }
            }
            return toReturn;
        }
        //Perf conditions de garantie
        public static List<DtoCommon> ObtenirRisquesCodes(ModeConsultation modeNavig, string offreId, int? offreVersion = null, string type = "O", string codeAvn = "")
        {
            List<DtoCommon> toReturn = new List<DtoCommon>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"
SELECT TBLRISQUE.JERSQ CODE
FROM {0} 
    LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
    LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
    LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
    LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
    LEFT JOIN {3} ON KABDESI = KADCHR
    LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
    LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1);

            var listRisquePlatDto = DbBase.Settings.ExecuteList<RisquePlatDto>(CommandType.Text, sql);
            if (listRisquePlatDto != null && listRisquePlatDto.Any())
            {
                foreach (var risquePlatDto in listRisquePlatDto)
                {
                    //RisqueDto risque = GetRisque(modeNavig, risquePlatDto, offreId, type, offreVersion, codeAvn);
                    //if (risque.IdAdresseRisque > 0)
                    //{
                    //    risque.AdresseRisque = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
                    //    risque.Objets = ObtenirObjet(modeNavig, offreId, risque.Code, type, offreVersion);
                    //}
                    DtoCommon risque = new DtoCommon
                    {
                        Code = risquePlatDto.Code.ToString(),
                        NbLigne = ObtenirObjetNombre(modeNavig, offreId, risquePlatDto.Code, type, offreVersion)
                    };
                    toReturn.Add(risque);
                }
            }
            return toReturn;
        }

        public static long? NombreRisques(ModeConsultation modeNavig, string offreId, int? offreVersion = null, string type = "O", string codeAvn = "")
        {


            var iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"
SELECT COUNT(TBLRISQUE.JERSQ) NBLIGN
FROM {0} 
    LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
    LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
    LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
    LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
    LEFT JOIN {3} ON KABDESI = KADCHR
    LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
    LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1);
            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql).FirstOrDefault()?.NbLigne;


        }

        public static List<ObjetDto> ObtenirObjet(ModeConsultation modeNavig, string offreId, int risqueId, string type, int? offreVersion = null, string codeAvn = "")
        {
            List<ObjetDto> ToReturn = new List<ObjetDto>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"SELECT DISTINCT TBLOBJET.JGRSQ CODERISQUE, TBLOBJET.JGOBJ CODE, KACDESC DESCRIPTIF, KACCIBLE CODECIBLE,
                                   KACDESI CHRONODESI, KADDESI LIBELLEOBJET, TBLOBJET.JGIPB IDOFFRE, TBLOBJET.JGALX VERSIONOFFRE,
                                   TBLOBJET.JGVDJ ENTREEGARANTIEJOUR, TBLOBJET.JGVDM ENTREEGARANTIEMOIS, TBLOBJET.JGVDA ENTREEGARANTIEANNEE, TBLOBJET.JGVDH ENTREEGARANTIEHEURE, 
                                   TBLOBJET.JGVFJ SORTIEGARANTIEJOUR, TBLOBJET.JGVFM SORTIEGARANTIEMOIS, TBLOBJET.JGVFA SORTIEGARANTIEANNEE, TBLOBJET.JGVFH SORTIEGARANTIEHEURE,
                                   TBLOBJETHISTO.JGVFJ SORTIEGARANTIEJOURHISTO, TBLOBJETHISTO.JGVFM SORTIEGARANTIEMOISHISTO, TBLOBJETHISTO.JGVFA SORTIEGARANTIEANNEEHISTO, TBLOBJETHISTO.JGVFH SORTIEGARANTIEHEUREHISTO,
                                   TBLOBJET.JGVAL VALEUR, TBLOBJET.JGVAU CODEUNITE, TBLOBJET.JGVAT CODETYPE, TBLOBJET.JGVAH VALEURHT,
                                   TBLOBJET.JGAVA DATEMODIAVNANNEEOBJ,TBLOBJET.JGAVM DATEMODIAVNMOISOBJ,TBLOBJET.JGAVJ DATEMODIAVNJOUROBJ,
                                   ABPLG3 BATIMENT, ABPNUM NUMEROVOIE, ABPEXT EXTENSIONVOIE, ABPLG4 NOMVOIE, ABPLG5 BOITEPOSTALE, ABPLOC, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL,
                                   ABPVI6 NOMVILLE, ABPL4F, ABPL6F, ABPCHR NUMEROCHRONO,
                                   TBLOBJET.JGINA ISRISQUEINDEXE, TBLOBJET.JGIXL ISLCI, TBLOBJET.JGIXF ISFRANCHISE, TBLOBJET.JGIXC ISASSIETTE, TBLOBJET.JGRGT REGIMETAXE, TBLOBJET.JGCNA ISCATNAT,
                                   ABPCEX, ABPVIX, KACAPE CODEAPE, 
                                   KACNMC01 NOMENCLATURE1, KACNMC02 NOMENCLATURE2, KACNMC03 NOMENCLATURE3, KACNMC04 NOMENCLATURE4, KACNMC05 NOMENCLATURE5, KACCM2 COUTM2, 
                                   TBLOBJET.JGTRR TERRITORIALITE, KACTRE CODETRE,TPLIB LIBTRE, KACCLASS CODECLASSE,
                                   KGMTYVAL ENSEMBLETYPE, KACMAND DATEDEBDESC, KACMANF DATEFINDESC,
                                   KACMANDH HEUREDEBDESC, KACMANFH HEUREFINDESC,
                                   TBLYPOBASE.PBAVA DATEEFFETAVNANNEE, TBLYPOBASE.PBAVM DATEEFFETAVNMOIS, TBLYPOBASE.PBAVJ DATEEFFETAVNJOUR,
                                   TBLYPOBASE.PBFEA FINEFFETANNEE, TBLYPOBASE.PBFEM FINEFFETMOIS, TBLYPOBASE.PBFEJ FINEFFETJOUR,
                                   JDPEA ECHEANCEANNEE, JDPEM ECHEANCEMOIS, JDPEJ ECHEANCEJOUR,
                                   TBLOBJET.JGIVO INDICEORIGINE, TBLOBJET.JGIVA INDICEACTU,
                                   TBLOBJET.JGAVE AVNCREATION, TBLOBJET.JGAVF AVNMODIF,
                                   KBEREPVAL REPORTVAL,
                                   TBLOBJET.JGTEM ISRISQUETEMP,
                                  TBLYPOBASEHISTO.PBAVJ DATEEFFETAVNJOUROBJ ,TBLYPOBASEHISTO.PBAVM DATEEFFETAVNMOISOBJ ,TBLYPOBASEHISTO.PBAVA DATEEFFETAVNANNEEOBJ, KACOBJ
                               FROM {4} TBLYPOBASE
                                   LEFT JOIN {5} ON JDIPB = TBLYPOBASE.PBIPB AND JDALX = TBLYPOBASE.PBALX {13}
                                   LEFT JOIN {6} TBLOBJET ON TBLYPOBASE.PBIPB = TBLOBJET.JGIPB AND TBLYPOBASE.PBALX = TBLOBJET.JGALX {14}
                                   LEFT JOIN YHRTOBJ TBLOBJETHISTO ON TBLOBJET.JGIPB = TBLOBJETHISTO.JGIPB AND TBLOBJET.JGALX = TBLOBJETHISTO.JGALX AND TBLOBJET.JGRSQ = TBLOBJETHISTO.JGRSQ AND TBLOBJET.JGOBJ = TBLOBJETHISTO.JGOBJ AND TBLOBJETHISTO.JGAVN = {18}
                                   LEFT JOIN {7} ON TBLOBJET.JGIPB = KACIPB AND TBLOBJET.JGALX = KACALX AND TBLOBJET.JGRSQ = KACRSQ AND TBLOBJET.JGOBJ = KACOBJ {15}
                                   LEFT JOIN {8} ON KACDESI = KADCHR
                                   LEFT JOIN {9} ON TBLOBJET.JGIPB = JFIPB AND TBLOBJET.JGALX = JFALX AND TBLOBJET.JGRSQ = JFRSQ AND TBLOBJET.JGOBJ = JFOBJ {16}
                                   LEFT JOIN YADRESS ON JFADH = ABPCHR
                                   LEFT JOIN KTYPVALD ON KGMBASE = TBLOBJET.JGVAT
                                   LEFT JOIN {10} ON KBGTYP = TBLYPOBASE.PBTYP AND KBGIPB = TBLYPOBASE.PBIPB AND KBGALX = TBLYPOBASE.PBALX AND KBGRSQ = TBLOBJET.JGRSQ AND KBGOBJ = TBLOBJET.JGOBJ {17}
                                   LEFT JOIN {11} ON KBEID = KBGKBEID
                                                LEFT JOIN KCIBLE CB ON KACCIBLE = CB.KAHCIBLE
                                   LEFT JOIN YHPBASE TBLYPOBASEHISTO ON TBLYPOBASE.PBIPB=TBLYPOBASEHISTO.PBIPB AND   TBLYPOBASE.PBALX=TBLYPOBASEHISTO.PBALX   AND   TBLYPOBASE.PBTYP=TBLYPOBASEHISTO.PBTYP AND  TBLYPOBASEHISTO.PBAVN = TBLOBJET.JGAVE
                                    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KACTRE        
                                   WHERE TBLYPOBASE.PBIPB='{0}' AND TBLYPOBASE.PBTYP='{1}' AND TBLYPOBASE.PBALX='{2}' AND TBLOBJET.JGRSQ='{3}' {12} 
                                ORDER BY KACOBJ",
                           offreId.PadLeft(9, ' '), type, offreVersion.HasValue ? offreVersion.Value : 0, risqueId,
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPINVAPP"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                        modeNavig == ModeConsultation.Historique ? string.Format(" AND TBLYPOBASE.PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND JDAVN = TBLYPOBASE.PBAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = TBLYPOBASE.PBAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = KACAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = JFAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = KBGAVN" : string.Empty,
                        iCodeAvn - 1);

            var listObjetPlatDto = DbBase.Settings.ExecuteList<ObjetPlatDto>(CommandType.Text, sql);
            if (listObjetPlatDto != null && listObjetPlatDto.Any())
            {
                foreach (var objetPlatDto in listObjetPlatDto)
                {

                    ObjetDto objet = GetObjet(modeNavig, objetPlatDto, codeAvn);
                    ToReturn.Add(objet);

                }
            }

            return ToReturn;
        }

        public static List<ObjetDto> GetObjetBaseInfos(ModeConsultation modeNavig, string offreId, int risqueId, string type, int? offreVersion = null, string codeAvn = "")
        {
            List<ObjetDto> ToReturn = new List<ObjetDto>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"SELECT DISTINCT  TBLOBJET.JGOBJ CODE, KACCIBLE CODECIBLE ,KACOBJ,
                                    TBLOBJET.JGVAL VALEUR, TBLOBJET.JGVAU CODEUNITE, TBLOBJET.JGVAT CODETYPE, TBLOBJET.JGVAH VALEURHT
                               FROM {4} TBLYPOBASE
                                   LEFT JOIN {5} ON JDIPB = TBLYPOBASE.PBIPB AND JDALX = TBLYPOBASE.PBALX {13}
                                   LEFT JOIN {6} TBLOBJET ON TBLYPOBASE.PBIPB = TBLOBJET.JGIPB AND TBLYPOBASE.PBALX = TBLOBJET.JGALX {14}
                                   LEFT JOIN YHRTOBJ TBLOBJETHISTO ON TBLOBJET.JGIPB = TBLOBJETHISTO.JGIPB AND TBLOBJET.JGALX = TBLOBJETHISTO.JGALX AND TBLOBJET.JGRSQ = TBLOBJETHISTO.JGRSQ AND TBLOBJET.JGOBJ = TBLOBJETHISTO.JGOBJ AND TBLOBJETHISTO.JGAVN = {18}
                                   LEFT JOIN {7} ON TBLOBJET.JGIPB = KACIPB AND TBLOBJET.JGALX = KACALX AND TBLOBJET.JGRSQ = KACRSQ AND TBLOBJET.JGOBJ = KACOBJ {15}
                                   LEFT JOIN {8} ON KACDESI = KADCHR
                                   LEFT JOIN {9} ON TBLOBJET.JGIPB = JFIPB AND TBLOBJET.JGALX = JFALX AND TBLOBJET.JGRSQ = JFRSQ AND TBLOBJET.JGOBJ = JFOBJ {16}
                                   LEFT JOIN YADRESS ON JFADH = ABPCHR
                                   LEFT JOIN KTYPVALD ON KGMBASE = TBLOBJET.JGVAT
                                   LEFT JOIN {10} ON KBGTYP = TBLYPOBASE.PBTYP AND KBGIPB = TBLYPOBASE.PBIPB AND KBGALX = TBLYPOBASE.PBALX AND KBGRSQ = TBLOBJET.JGRSQ AND KBGOBJ = TBLOBJET.JGOBJ {17}
                                   LEFT JOIN {11} ON KBEID = KBGKBEID
                                                LEFT JOIN KCIBLE CB ON KACCIBLE = CB.KAHCIBLE
                                   LEFT JOIN YHPBASE TBLYPOBASEHISTO ON TBLYPOBASE.PBIPB=TBLYPOBASEHISTO.PBIPB AND   TBLYPOBASE.PBALX=TBLYPOBASEHISTO.PBALX   AND   TBLYPOBASE.PBTYP=TBLYPOBASEHISTO.PBTYP AND  TBLYPOBASEHISTO.PBAVN = TBLOBJET.JGAVE
                                    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KACTRE        
                                   WHERE TBLYPOBASE.PBIPB='{0}' AND TBLYPOBASE.PBTYP='{1}' AND TBLYPOBASE.PBALX='{2}' AND TBLOBJET.JGRSQ='{3}' {12} 
                                ORDER BY KACOBJ",
                           offreId.PadLeft(9, ' '), type, offreVersion.HasValue ? offreVersion.Value : 0, risqueId,
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPINVAPP"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                        modeNavig == ModeConsultation.Historique ? string.Format(" AND TBLYPOBASE.PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND JDAVN = TBLYPOBASE.PBAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = TBLYPOBASE.PBAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = KACAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = JFAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = KBGAVN" : string.Empty,
                        iCodeAvn - 1);

            DbBase.Settings.ExecuteList<ObjetPlatDto>(CommandType.Text, sql)
                           .ForEach(objetPlatDto => ToReturn.Add(
                               new ObjetDto
                               {
                                   Code = objetPlatDto.Code,
                                   Cible = new CibleDto { Code = objetPlatDto.CodeCible },
                                   Valeur = objetPlatDto.Valeur,
                                   Unite = new ParametreDto
                                   {
                                       Code = !string.IsNullOrEmpty(objetPlatDto.CodeUnite) ? objetPlatDto.CodeUnite.Trim() : string.Empty
                                   },
                                   Type = new ParametreDto
                                   {
                                       Code = !string.IsNullOrEmpty(objetPlatDto.CodeType) ? objetPlatDto.CodeType.Trim() : objetPlatDto.CodeType
                                   },
                                   ValeurHT = !string.IsNullOrEmpty(objetPlatDto.ValeurHT) ? objetPlatDto.ValeurHT.Trim() : string.Empty
                               }
                            ));

            return ToReturn;
        }
        //Perf conditions de garantie
        public static long ObtenirObjetNombre(ModeConsultation modeNavig, string offreId, int risqueId, string type, int? offreVersion = null, string codeAvn = "")
        {
            //List<ObjetDto> ToReturn = new List<ObjetDto>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"SELECT COUNT(DISTINCT TBLOBJET.JGOBJ) NBLIGN
                               FROM {4} TBLYPOBASE
                                   LEFT JOIN {5} ON JDIPB = TBLYPOBASE.PBIPB AND JDALX = TBLYPOBASE.PBALX {13}
                                   LEFT JOIN {6} TBLOBJET ON TBLYPOBASE.PBIPB = TBLOBJET.JGIPB AND TBLYPOBASE.PBALX = TBLOBJET.JGALX {14}
                                   LEFT JOIN YHRTOBJ TBLOBJETHISTO ON TBLOBJET.JGIPB = TBLOBJETHISTO.JGIPB AND TBLOBJET.JGALX = TBLOBJETHISTO.JGALX AND TBLOBJET.JGRSQ = TBLOBJETHISTO.JGRSQ AND TBLOBJET.JGOBJ = TBLOBJETHISTO.JGOBJ AND TBLOBJETHISTO.JGAVN = {18}
                                   LEFT JOIN {7} ON TBLOBJET.JGIPB = KACIPB AND TBLOBJET.JGALX = KACALX AND TBLOBJET.JGRSQ = KACRSQ AND TBLOBJET.JGOBJ = KACOBJ {15}
                                   LEFT JOIN {8} ON KACDESI = KADCHR
                                   LEFT JOIN {9} ON TBLOBJET.JGIPB = JFIPB AND TBLOBJET.JGALX = JFALX AND TBLOBJET.JGRSQ = JFRSQ AND TBLOBJET.JGOBJ = JFOBJ {16}
                                   LEFT JOIN YADRESS ON JFADH = ABPCHR
                                   LEFT JOIN KTYPVALD ON KGMBASE = TBLOBJET.JGVAT
                                   LEFT JOIN {10} ON KBGTYP = TBLYPOBASE.PBTYP AND KBGIPB = TBLYPOBASE.PBIPB AND KBGALX = TBLYPOBASE.PBALX AND KBGRSQ = TBLOBJET.JGRSQ AND KBGOBJ = TBLOBJET.JGOBJ {17}
                                   LEFT JOIN {11} ON KBEID = KBGKBEID
                                                LEFT JOIN KCIBLE CB ON KACCIBLE = CB.KAHCIBLE
                                   LEFT JOIN YHPBASE TBLYPOBASEHISTO ON TBLYPOBASE.PBIPB=TBLYPOBASEHISTO.PBIPB AND   TBLYPOBASE.PBALX=TBLYPOBASEHISTO.PBALX   AND   TBLYPOBASE.PBTYP=TBLYPOBASEHISTO.PBTYP AND  TBLYPOBASEHISTO.PBAVN = TBLOBJET.JGAVE
                                    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KACTRE        
                                   WHERE TBLYPOBASE.PBIPB='{0}' AND TBLYPOBASE.PBTYP='{1}' AND TBLYPOBASE.PBALX='{2}' AND TBLOBJET.JGRSQ='{3}' {12} ",
                           offreId.PadLeft(9, ' '), type, offreVersion.HasValue ? offreVersion.Value : 0, risqueId,
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPINVAPP"),
                           CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                        modeNavig == ModeConsultation.Historique ? string.Format(" AND TBLYPOBASE.PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND JDAVN = TBLYPOBASE.PBAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = TBLYPOBASE.PBAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = KACAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = JFAVN" : string.Empty,
                        modeNavig == ModeConsultation.Historique ? " AND TBLOBJET.JGAVN = KBGAVN" : string.Empty,
                        iCodeAvn - 1);

            //var listObjetPlatDto = DbBase.Settings.ExecuteList<ObjetPlatDto>(CommandType.Text, sql);
            //if (listObjetPlatDto != null && listObjetPlatDto.Any())
            //{
            //    foreach (var objetPlatDto in listObjetPlatDto)
            //    {

            //        ObjetDto objet = GetObjet(modeNavig, objetPlatDto, codeAvn);


            //        ToReturn.Add(objet);

            //    }
            //}
            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql).FirstOrDefault()?.NbLigne ?? 0;
        }

        public static List<InventaireDto> ObtenirInventaires(ModeConsultation modeNavig, int risqueId, int objetId, string offreId, int? offreVersion, string codeAvn)
        {
            var toReturn = new List<InventaireDto>();
            string sql = string.Format(@"SELECT KBEID CODE, KBEDESC DESCRIPTIF, KAGTYINV CODETYPE, KBGPERI PERIMETREAPPLICATION, KBEKADID NUMDESCRIPTION
                            FROM {4}
                            INNER JOIN {5} ON KBEKAGID = KAGID
                            INNER JOIN {6} ON KBEID = KBGKBEID
                            WHERE KBGIPB='{0}' AND KBGALX='{1}' AND KBGRSQ='{2}' AND KBGOBJ='{3}' {7}",
                             offreId.PadLeft(9, ' '), offreVersion, risqueId, objetId,
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                            CommonRepository.GetPrefixeHisto(modeNavig, "KINVTYP"),
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPINVAPP"),
                            modeNavig == ModeConsultation.Historique ? string.Format(" AND KBGAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty);
            var listInvPlatDto = DbBase.Settings.ExecuteList<InventairePlatDto>(CommandType.Text, sql);
            if (listInvPlatDto != null && listInvPlatDto.Any())
            {
                foreach (var invPlatDto in listInvPlatDto)
                {
                    InventaireDto inventaire = GetInventaire(invPlatDto);
                    toReturn.Add(inventaire);
                }
            }
            return toReturn;
        }

        public static BonificationsDto ObtenirBonification(string offreId, int? offreVersion, bool getValues = true)
        {
            BonificationsDto toReturn = null;
            if (!offreVersion.HasValue || string.IsNullOrEmpty(offreId))
                return toReturn;

            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = offreId.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = offreVersion.Value.ToString();
            string sql = @"SELECT KAATYP TYPE, KAABONI ISBONIFICATION, KAABONT TAUX, KAAANTI ISANTICIPE FROM KPENT WHERE KAAIPB= :codeOffre AND KAAALX= :version ";
            var bonificationDto = DbBase.Settings.ExecuteList<BonificationsDto>(CommandType.Text, sql, param);
            if (bonificationDto != null && bonificationDto.Any())
            {
                toReturn = bonificationDto.FirstOrDefault();
                toReturn.Bonification = toReturn.IsBonification == "O";
                toReturn.TauxBonification = toReturn.Taux.ToString();
                toReturn.Anticipe = toReturn.IsAnticipe == "O";
            }
            return toReturn;
        }

        public static string ObtenirValeurIndice(string codeIndice, string dateEffet)
        {
            string toReturn = "";
            DateTime dateRecherche;
            if (!DateTime.TryParse(dateEffet, out dateRecherche))
                dateRecherche = DateTime.Now;

            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeIndice", DbType.AnsiStringFixedLength);
            param[0].Value = codeIndice;
            param[1] = new EacParameter("dateRecherche", DbType.Int32);
            param[1].Value = AlbConvert.ConvertDateToInt(dateRecherche) == null ? 0 : AlbConvert.ConvertDateToInt(dateRecherche);
            string sql = @"SELECT COUNT(*) NBLIGN FROM YINDICE
                              WHERE GIIND= :codeIndice AND (GIIPA * 10000 + GIIPM * 100 + GIIPJ) <= :dateRecherche";
            if (CommonRepository.ExistRowParam(sql, param))
            {
                sql = @"SELECT GIIDV FROM YINDICE WHERE GIIND= :codeIndice AND (GIIPA * 10000 + GIIPM * 100 + GIIPJ) <= :dateRecherche
                          ORDER BY GIIPA * 10000 + GIIPM * 100 + GIIPJ DESC FETCH FIRST 1 ROWS ONLY";
                var result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
                if (result != null)
                    toReturn = result.ToString();
            }
            return toReturn;
        }

        public static void UpdateIndexation(string codeOffre, string version, string type)
        {
            int iVersion = 0;
            int.TryParse(version, out iVersion);

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = iVersion;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATEINDEXATION", param);
        }

        public static List<ParametreDto> ObtenirMotClef(string branche, string cible)
        {

            //return ReferenceRepository.RechercherParametres("PRODU", "POMOC").Select(x => new MotClef { Code = x.Code, Libelle = x.Libelle }).ToList();
            return CommonRepository.GetParametres(branche, cible, "PRODU", "POMOC");
        }

        private static string GetRechercheOrderByClause(string sortingName, string sortingOrder)
        {
            string chaineOrderBySQL = "";
            switch (sortingName)
            {
                case "OffreContratNum":
                    chaineOrderBySQL = "SUBSTR(PBIPB, 3) " + sortingOrder + ", PBALX DESC";

                    break;
                case "Version":
                    chaineOrderBySQL = "PBALX " + sortingOrder + ", SUBSTR(PBIPB, 3) DESC";
                    break;
                case "DateSaisie":
                    chaineOrderBySQL = "PBSAA * 10000 + PBSAM * 100 + PBSAJ " + sortingOrder + " ,SUBSTR(PBIPB, 3) DESC, PBALX DESC";

                    break;
                case "DateEffet":
                    chaineOrderBySQL = "PBEFA * 10000 + PBEFM * 100 + PBEFJ " + sortingOrder + " ,SUBSTR(PBIPB, 3) DESC, PBALX DESC";

                    break;
                case "DateMaj":
                    chaineOrderBySQL = "PBMJA * 10000 + PBMJM * 100 + PBMJJ " + sortingOrder + " ,SUBSTR(PBIPB, 3) DESC, PBALX DESC";

                    break;
                case "DateCreation":
                    chaineOrderBySQL = "PBCRA * 10000 + PBCRM * 100 + PBCRJ " + sortingOrder + " ,SUBSTR(PBIPB, 3) DESC, PBALX DESC";

                    break;
                case "PreneurAssuranceNom":
                    chaineOrderBySQL = "TRIM(UPPER(ANNOM))  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "PreneurAssuranceCP":
                    chaineOrderBySQL = "ASSU_ABPDP6";
                    chaineOrderBySQL += " " + sortingOrder + ", ";
                    chaineOrderBySQL += "ASSU_ABPCP6, SUBSTR(PBIPB, 3) DESC,  PBALX DESC";

                    break;
                case "Branche":
                    chaineOrderBySQL = "PBBRA  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "Etat":
                    chaineOrderBySQL = "PBETA  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "Situation":
                    chaineOrderBySQL = "PBSIT  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "Qualite":
                    chaineOrderBySQL = "PBSTQ  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "CourtierGestionnaireNom":
                    chaineOrderBySQL = "TRIM(UPPER(TNNOMCAB))  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "CourtierGestionnaireCP":
                    chaineOrderBySQL = "COURT_ABPDP6";
                    chaineOrderBySQL += " " + sortingOrder + ", ";
                    chaineOrderBySQL += "COURT_ABPCP6, SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                case "DescriptifRisque":
                    chaineOrderBySQL = "TRIM(UPPER(PBREF))  " + sortingOrder + " , SUBSTR(PBIPB, 3) DESC, PBALX DESC";
                    break;
                default:
                    chaineOrderBySQL = "PBSAA * 10000 + PBSAM * 100 + PBSAJ " + sortingOrder + ", SUBSTR(PBIPB, 3) DESC, PBALX DESC ";
                    break;
            }
            return chaineOrderBySQL;
        }

        public static bool TesterExistanceOffre(string offreId)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
            param[0].Value = offreId.PadLeft(9, ' ');
            string sql = @"SELECT Count (*) NBLIGN FROM YPOBASE WHERE PBIPB = :codeOffre";
            return CommonRepository.ExistRowParam(sql, param);
        }

        private static DateTime? ObtenirDateEchangePrincipale(DataRow row)
        {
            DateTime? date = null;
            if (row.Table.Columns.Contains("PBECJ") && row.Table.Columns.Contains("PBECM"))
            {
                int mois = Int32.Parse(row["PBECM"].ToString());
                int jour = Int32.Parse(row["PBECJ"].ToString());
                if (mois != 0 && jour != 0)
                {
                    int annee = DateTime.Now.Year;
                    date = new DateTime(annee, mois, jour);
                }
            }
            return date;
        }

        private static DateTime? ObtenirDateSaisie(DataRow row)
        {
            return ObtenirDate(row, "PBSA");
        }

        private static DateTime? ObtenirDateEnregistrement(DataRow row)
        {
            return ObtenirDate(row, "PBDE");
        }

        private static DateTime? ObtenirDateEffet(DataRow row)
        {
            return ObtenirDate(row, "PBEF");
        }

        private static DateTime? ObtenirDateFinEffet(DataRow row)
        {
            return ObtenirDate(row, "PBFE");
        }

        private static DateTime? ObtenirDateCreation(DataRow row)
        {
            return ObtenirDate(row, "PBCR");
        }

        private static DateTime? ObtenirDateMaJ(DataRow row)
        {
            return ObtenirDate(row, "PBMJ");
        }

        private static DateTime? ObtenirDate(DataRow row, string prefixe)
        {
            DateTime? date = null;
            if (row.Table.Columns.Contains(prefixe + "A") && row.Table.Columns.Contains(prefixe + "M") && row.Table.Columns.Contains(prefixe + "J"))
            {
                int annee = 0, mois = 0, jour = 0, horaire = 0;
                int.TryParse(row[prefixe + "A"].ToString(), out annee);
                int.TryParse(row[prefixe + "M"].ToString(), out mois);
                int.TryParse(row[prefixe + "J"].ToString(), out jour);
                if (row.Table.Columns.Contains(prefixe + "H")) { int.TryParse(row[prefixe + "H"].ToString(), out horaire); };
                date = AlbConvert.GetDate(annee, mois, jour, horaire);
            }
            return date;
        }

        public static void ChangerStatutOffre(string offreId, string codeSituation, ParametreDto motifRefus = null)
        {
            string sql = string.Empty;
            EacParameter[] param = null;
            if (codeSituation == "A" && motifRefus != null)
            {
                param = new EacParameter[10];
                param[0] = new EacParameter("codeSituation", DbType.AnsiStringFixedLength);
                param[0].Value = codeSituation;
                param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[1].Value = "N";
                param[2] = new EacParameter("day", DbType.Int32);
                param[2].Value = DateTime.Now.Day;
                param[3] = new EacParameter("month", DbType.Int32);
                param[3].Value = DateTime.Now.Month;
                param[4] = new EacParameter("year", DbType.AnsiStringFixedLength);
                param[4].Value = DateTime.Now.Year;
                param[5] = new EacParameter("day2", DbType.Int32);
                param[5].Value = DateTime.Now.Day;
                param[6] = new EacParameter("month2", DbType.Int32);
                param[6].Value = DateTime.Now.Month;
                param[7] = new EacParameter("year2", DbType.Int32);
                param[7].Value = DateTime.Now.Year;
                param[8] = new EacParameter("codeRefus", DbType.AnsiStringFixedLength);
                param[8].Value = motifRefus.Code;
                param[9] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[9].Value = offreId.PadLeft(9, ' ');

                sql = @"UPDATE YPOBASE 
                                    SET PBSIT = :codeSituation, PBETA = :type, PBSTJ = :day, 
                                    PBSTM = :month, PBSTA = :year, PBMJJ = :day2, PBMJM = :month2, PBMJA = :year2, PBSTF = :codeRefus
                                    WHERE PBIPB = :codeOffre ";
            }
            if (codeSituation == "A" && motifRefus == null)
            {
                param = new EacParameter[9];
                param[0] = new EacParameter("codeSituation", DbType.AnsiStringFixedLength);
                param[0].Value = codeSituation;
                param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[1].Value = "N";
                param[2] = new EacParameter("day", DbType.Int32);
                param[2].Value = DateTime.Now.Day;
                param[3] = new EacParameter("month", DbType.Int32);
                param[3].Value = DateTime.Now.Month;
                param[4] = new EacParameter("year", DbType.Int32);
                param[4].Value = DateTime.Now.Year;
                param[5] = new EacParameter("day2", DbType.Int32);
                param[5].Value = DateTime.Now.Day;
                param[6] = new EacParameter("month2", DbType.Int32);
                param[6].Value = DateTime.Now.Month;
                param[7] = new EacParameter("year2", DbType.Int32);
                param[7].Value = DateTime.Now.Year;
                param[8] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[8].Value = offreId.PadLeft(9, ' ');

                sql = @"UPDATE YPOBASE 
                                    SET PBSIT = :codeSituation, PBETA = :type, PBSTJ = :day, 
                                    PBSTM = :month, PBSTA = :year, PBMJJ = :day2, PBMJM = :month2, PBMJA = :year2
                                    WHERE PBIPB = :codeOffre ";
            }
            if (codeSituation != "A" && motifRefus != null)
            {
                param = new EacParameter[9];
                param[0] = new EacParameter("codeSituation", DbType.AnsiStringFixedLength);
                param[0].Value = codeSituation;
                param[1] = new EacParameter("day", DbType.Int32);
                param[1].Value = DateTime.Now.Day;
                param[2] = new EacParameter("month", DbType.Int32);
                param[2].Value = DateTime.Now.Month;
                param[3] = new EacParameter("year", DbType.Int32);
                param[3].Value = DateTime.Now.Year;
                param[4] = new EacParameter("day2", DbType.Int32);
                param[4].Value = DateTime.Now.Day;
                param[5] = new EacParameter("month2", DbType.Int32);
                param[5].Value = DateTime.Now.Month;
                param[6] = new EacParameter("year2", DbType.Int32);
                param[6].Value = DateTime.Now.Year;
                param[7] = new EacParameter("codeRefus", DbType.AnsiStringFixedLength);
                param[7].Value = motifRefus.Code;
                param[8] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[8].Value = offreId.PadLeft(9, ' ');

                sql = @"UPDATE YPOBASE 
                                    SET PBSIT = :codeSituation, PBSTJ = :day, 
                                    PBSTM = :month, PBSTA = :year, PBMJJ = :day2, PBMJM = :month2, PBMJA = :year2, PBSTF = :codeRefus
                                    WHERE PBIPB = :codeOffre ";

                //                Retour arrière sur le refus d'une offre V-W => N-W                
                //                sql = string.Format(@"UPDATE YPOBASE 
                //                                    SET PBSIT = '{0}', PBETA ='{1}', PBSTJ = '{2}', 
                //                                    PBSTM = '{3}', PBSTA = '{4}', PBMJJ = '{5}', PBMJM = '{6}', PBMJA = '{7}', PBSTF = '{8}'
                //                                    WHERE PBIPB = '{9}'",
                //                                       codeSituation, "V",
                //                                       DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year,
                //                                       motifRefus.Code, offreId.PadLeft(9, ' '));
            }
            if (codeSituation != "A" && motifRefus == null)
            {
                param = new EacParameter[8];
                param[0] = new EacParameter("codeSituation", DbType.AnsiStringFixedLength);
                param[0].Value = codeSituation;
                param[1] = new EacParameter("day", DbType.Int32);
                param[1].Value = DateTime.Now.Day;
                param[2] = new EacParameter("month", DbType.Int32);
                param[2].Value = DateTime.Now.Month;
                param[3] = new EacParameter("year", DbType.Int32);
                param[3].Value = DateTime.Now.Year;
                param[4] = new EacParameter("day2", DbType.Int32);
                param[4].Value = DateTime.Now.Day;
                param[5] = new EacParameter("month2", DbType.Int32);
                param[5].Value = DateTime.Now.Month;
                param[6] = new EacParameter("year2", DbType.Int32);
                param[6].Value = DateTime.Now.Year;
                param[7] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[7].Value = offreId.PadLeft(9, ' ');

                sql = @"UPDATE YPOBASE  
                                    SET PBSIT = :codeSituation, PBSTJ = :day, 
                                    PBSTM = :month, PBSTA = :year, PBMJJ = :day2, PBMJM = :month2, PBMJA = :year2
                                    WHERE PBIPB = :codeOffre";
            }
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (motifRefus != null && motifRefus.Code == "AAS")
            {
                param = new EacParameter[1];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = offreId.PadLeft(9, ' ');
                sql = @"UPDATE YPOBASE 
                                    SET PBETA = 'A'
                                    WHERE PBIPB = :codeOffre ";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        public static List<CabinetAutreDto> AlimenteAutreDoubleSaisie(string idOffre, string alimentOffre)
        {
            var result = new List<CabinetAutreDto>();

            //EacParameter[] param = new EacParameter[4];
            //param[0] = new EacParameter("join", DbType.AnsiStringFixedLength);
            //param[0].Value = CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "DBMOT", otherCriteria: " AND TCOD = KAFMOT");
            //param[1] = new EacParameter("idOffre", DbType.AnsiStringFixedLength);
            //param[1].Value = idOffre.PadLeft(9, ' ');
            //param[2] = new EacParameter("alimentOffre", DbType.Int32);
            //param[2].Value = int.Parse(alimentOffre);
            //param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
            //param[3].Value = "O";

            //string sql = @"SELECT KAFICT CODE, TNNOM NOM, BUDBU DELEGATION, KAFSOU SOUSCRIPTEUR, KAFSAID DATESAISIE, KAFSAIH HEURESAISIE, KAFCRD DATECREATION, KAFCRH HEURECREATION,
            //        KAFACT ACTION, KAFMOT MOTIF, TPLIB LIBMOTIF,UT.UTNOM SOUSCRIPTEURNOM
            //        FROM KPODBLS 
            //        LEFT JOIN YCOURTN ON KAFICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A' 
            //        LEFT JOIN YCOURTI ON TCICT = KAFICT 
            //        LEFT JOIN YBUREAU ON TCBUR = BUIBU 
            //        LEFT JOIN YUTILIS UT ON KAFSOU = UT.UTIUT AND UT.UTSOU ='O'
            //        :join
            //        WHERE KAFIPB= :idOffre AND KAFALX= :alimentOffre AND KAFTYP= :type ORDER BY KAFCRD, KAFCRH";

            //var listCabinetAutreDtoPlat = DbBase.Settings.ExecuteList<CabinetAutreDtoPlat>(CommandType.Text, sql, param);

            string sql = string.Format(@"SELECT KAFICT CODE, TNNOM NOM, BUDBU DELEGATION, KAFSOU SOUSCRIPTEUR, KAFSAID DATESAISIE, KAFSAIH HEURESAISIE, KAFCRD DATECREATION, KAFCRH HEURECREATION,
                    KAFACT ACTION, KAFMOT MOTIF, TPLIB LIBMOTIF,UT.UTNOM SOUSCRIPTEURNOM
                    FROM KPODBLS 
                    LEFT JOIN YCOURTN ON KAFICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A' 
                    LEFT JOIN YCOURTI ON TCICT = KAFICT 
                    LEFT JOIN YBUREAU ON TCBUR = BUIBU 
                    LEFT JOIN YUTILIS UT ON KAFSOU = UT.UTIUT AND UT.UTSOU ='O'
                    {0}
                    WHERE KAFIPB='{1}' AND KAFALX='{2}' AND KAFTYP='{3}' ORDER BY KAFCRD, KAFCRH", CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "DBMOT", otherCriteria: " AND TCOD = KAFMOT"),
                                                                         idOffre.PadLeft(9, ' '), int.Parse(alimentOffre), "O");
            var listCabinetAutreDtoPlat = DbBase.Settings.ExecuteList<CabinetAutreDtoPlat>(CommandType.Text, sql);

            if (listCabinetAutreDtoPlat != null && listCabinetAutreDtoPlat.Any())
            {
                foreach (var cabinetAutreDtoPlat in listCabinetAutreDtoPlat)
                {
                    DateTime? dateSaisie = AlbConvert.ConvertIntToDate(cabinetAutreDtoPlat.DateSaisie);
                    TimeSpan? timeSaisie;
                    if (cabinetAutreDtoPlat.HeureSaisie >= 10000)
                    {
                        timeSaisie = AlbConvert.ConvertIntToTime(cabinetAutreDtoPlat.HeureSaisie);
                    }
                    else
                    {
                        timeSaisie = AlbConvert.ConvertIntToTimeMinute(cabinetAutreDtoPlat.HeureSaisie);
                    }

                    DateTime? dateCreation = AlbConvert.ConvertIntToDate(cabinetAutreDtoPlat.DateCreation);
                    TimeSpan? timeCreation;
                    if (cabinetAutreDtoPlat.HeureCreation >= 10000)
                    {
                        timeCreation = AlbConvert.ConvertIntToTime(cabinetAutreDtoPlat.HeureCreation);
                    }
                    else
                    {
                        timeCreation = AlbConvert.ConvertIntToTimeMinute(cabinetAutreDtoPlat.HeureCreation);
                    }

                    CabinetAutreDto cabinetAutre = new CabinetAutreDto();
                    cabinetAutre.Code = cabinetAutreDtoPlat.Code.ToString();
                    cabinetAutre.Courtier = cabinetAutreDtoPlat.Nom;
                    cabinetAutre.Delegation = cabinetAutreDtoPlat.Delegation;
                    cabinetAutre.CodeSouscripteur = cabinetAutreDtoPlat.Souscripteur;
                    cabinetAutre.Souscripteur = cabinetAutreDtoPlat.SouscripteurNom;

                    if (dateSaisie != null)
                    {
                        cabinetAutre.SaisieDate = timeSaisie != null ? new DateTime(dateSaisie.Value.Year, dateSaisie.Value.Month, dateSaisie.Value.Day, timeSaisie.Value.Hours, timeSaisie.Value.Minutes, timeSaisie.Value.Seconds) : new DateTime(dateSaisie.Value.Year, dateSaisie.Value.Month, dateSaisie.Value.Day, 0, 0, 0);
                        cabinetAutre.SaisieHeure = new TimeSpan(cabinetAutre.SaisieDate.Value.Hour, cabinetAutre.SaisieDate.Value.Minute, cabinetAutre.SaisieDate.Value.Second);
                    }
                    if (dateCreation != null)
                    {
                        cabinetAutre.EnregistrementDate = timeCreation != null ? new DateTime(dateCreation.Value.Year, dateCreation.Value.Month, dateCreation.Value.Day, timeCreation.Value.Hours, timeCreation.Value.Minutes, timeCreation.Value.Seconds) : new DateTime(dateCreation.Value.Year, dateCreation.Value.Month, dateCreation.Value.Day, 0, 0, 0);
                        cabinetAutre.EnregistrementHeure = new TimeSpan(cabinetAutre.EnregistrementDate.Value.Hour, cabinetAutre.EnregistrementDate.Value.Minute, cabinetAutre.EnregistrementDate.Value.Second);
                    }
                    if (cabinetAutreDtoPlat.Action == "INI")
                        cabinetAutre.Motif = "Apporteur initial";
                    if (cabinetAutreDtoPlat.Action == "REF")
                        cabinetAutre.Motif = "Refus";
                    if (cabinetAutreDtoPlat.Action == "REM")
                        cabinetAutre.Motif = cabinetAutreDtoPlat.Motif;
                    cabinetAutre.LibelleMotif = cabinetAutreDtoPlat.LibelleMotif == null ? string.Empty : cabinetAutreDtoPlat.LibelleMotif;


                    result.Add(cabinetAutre);
                }
            }
            return result;
        }

        public static bool GetDoubleSaisie(string id, int version, string type)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
            param[0].Value = id.PadLeft(9, ' ');

            param[1] = new EacParameter("type", DbType.Int32);
            param[1].Value = type;

            param[2] = new EacParameter("codeoffreSubQuery", DbType.AnsiStringFixedLength);
            param[2].Value = id.PadLeft(9, ' ');

            param[3] = new EacParameter("typeSubQuery", DbType.Int32);
            param[3].Value = type;
            //param[1] = new EacParameter("version", 0);
            //param[1].Value = version;

            string sql = @"SELECT COUNT(*) NBLIGN FROM KPODBLS WHERE KAFIPB = :codeOffre AND KAFTYP = :type 
                         AND KAFALX = (SELECT IFNULL(MAX(PBALX), 0) FROM YPOBASE WHERE PBIPB = :codeoffreSubQuery AND PBTYP = :typeSubQuery)";
            Int64 nbRow = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne;

            return (nbRow > 1);
        }

        public static bool GetOppBenef(string codeOffre, int version, string type)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sqlOpp = "SELECT COUNT(*) NBLIGN FROM KPOPP WHERE KFPIPB = :codeOffre AND KFPALX = :version AND KFPTYP = :type";

            return CommonRepository.ExistRowParam(sqlOpp, param);
        }

        #region Modifier Offre

        public static string SauvegarderModifierOffre(OffreDto offre, string user)
        {

            string msgRetour = string.Empty;
            int numeroChronoObservation = CommonRepository.GetAS400Id("KAJCHR");

            #region Paramètres de la SPO

            EacParameter[] param = new EacParameter[70];
            param[0] = new EacParameter("P_CODE", DbType.AnsiStringFixedLength);
            param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = offre.Version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = offre.Type;
            param[3] = new EacParameter("P_REFERENCE", DbType.AnsiStringFixedLength);
            param[3].Value = !string.IsNullOrEmpty(offre.Descriptif) ? offre.Descriptif.Replace("'", "'") : string.Empty;
            param[4] = new EacParameter("P_MOTCLE1", DbType.AnsiStringFixedLength);
            param[4].Value = !string.IsNullOrEmpty(offre.MotCle1) ? offre.MotCle1 : string.Empty;
            param[5] = new EacParameter("P_MOTCLE2", DbType.AnsiStringFixedLength);
            param[5].Value = !string.IsNullOrEmpty(offre.MotCle2) ? offre.MotCle2 : string.Empty;
            param[6] = new EacParameter("P_MOTCLE3", DbType.AnsiStringFixedLength);
            param[6].Value = !string.IsNullOrEmpty(offre.MotCle3) ? offre.MotCle3 : string.Empty;
            param[7] = new EacParameter("P_CODEDEVISE", DbType.AnsiStringFixedLength);
            param[7].Value = offre.Devise != null ? offre.Devise.Code : string.Empty;
            param[8] = new EacParameter("P_PERIODICITE", DbType.AnsiStringFixedLength);
            param[8].Value = offre.Periodicite != null ? offre.Periodicite.Code : string.Empty;
            param[9] = new EacParameter("P_ECHEANCEJOUR", DbType.Int32);
            param[9].Value = offre.EcheancePrincipale != null ? offre.EcheancePrincipale.Value.Day : 0;
            param[10] = new EacParameter("P_ECHEANCEMOIS", DbType.Int32);
            param[10].Value = offre.EcheancePrincipale != null ? offre.EcheancePrincipale.Value.Month : 0;
            param[11] = new EacParameter("P_EFFETJOUR", DbType.Int32);
            param[11].Value = offre.DateEffetGarantie != null ? offre.DateEffetGarantie.Value.Day : 0;
            param[12] = new EacParameter("P_EFFETMOIS", DbType.Int32);
            param[12].Value = offre.DateEffetGarantie != null ? offre.DateEffetGarantie.Value.Month : 0;
            param[13] = new EacParameter("P_EFFETANNEE", DbType.Int32);
            param[13].Value = offre.DateEffetGarantie != null ? offre.DateEffetGarantie.Value.Year : 0;
            param[14] = new EacParameter("P_EFFETHEURE", DbType.Int32);
            param[14].Value = offre.DateEffetGarantie != null ? offre.DateEffetGarantie.Value.Hour * 100 + offre.DateEffetGarantie.Value.Minute : 0;
            param[15] = new EacParameter("P_FINEFFETJOUR", DbType.Int32);
            param[15].Value = offre.DateFinEffetGarantie != null ? offre.DateFinEffetGarantie.Value.Day : 0;
            param[16] = new EacParameter("P_FINEFFETMOIS", DbType.Int32);
            param[16].Value = offre.DateFinEffetGarantie != null ? offre.DateFinEffetGarantie.Value.Month : 0;
            param[17] = new EacParameter("P_FINEFFETANNEE", DbType.Int32);
            param[17].Value = offre.DateFinEffetGarantie != null ? offre.DateFinEffetGarantie.Value.Year : 0;
            param[18] = new EacParameter("P_FINEFFETHEURE", DbType.Int32);
            param[18].Value = offre.DateFinEffetGarantie != null ? offre.DateFinEffetGarantie.Value.Hour * 100 + offre.DateFinEffetGarantie.Value.Minute : 0;
            param[19] = new EacParameter("P_DUREE", DbType.Int32);
            param[19].Value = offre.DureeGarantie != null ? offre.DureeGarantie : 0;
            param[20] = new EacParameter("P_UNITEDUREE", DbType.AnsiStringFixedLength);
            param[20].Value = (offre.UniteDeTemps != null && offre.UniteDeTemps.Code != null) ? offre.UniteDeTemps.Code : string.Empty;
            param[21] = new EacParameter("P_NATURE", DbType.AnsiStringFixedLength);
            param[21].Value = offre.NatureContrat != null ? offre.NatureContrat.Code : string.Empty;
            param[22] = new EacParameter("P_APERITION", DbType.Int32);
            param[22].Value = offre.PartAlbingia;
            param[23] = new EacParameter("P_COUVERT", DbType.Int32);
            param[23].Value = offre.Couverture ?? 0;
            param[24] = new EacParameter("P_SOUSCRIPTEUR", DbType.AnsiStringFixedLength);
            param[24].Value = offre.Souscripteur != null ? offre.Souscripteur.Code : string.Empty;
            param[25] = new EacParameter("P_GESTIONNAIRE", DbType.AnsiStringFixedLength);
            param[25].Value = offre.Gestionnaire != null ? offre.Gestionnaire.Id : string.Empty;
            param[26] = new EacParameter("P_REGIMETAXE", DbType.AnsiStringFixedLength);
            param[26].Value = !string.IsNullOrEmpty(offre.CodeRegime) ? offre.CodeRegime : string.Empty;
            param[27] = new EacParameter("P_IDASSURE", DbType.Int32);
            param[27].Value = (offre.PreneurAssurance != null && !string.IsNullOrEmpty(offre.PreneurAssurance.Code)) ? Int32.Parse(offre.PreneurAssurance.Code) : 0;
            param[28] = new EacParameter("P_COURTIER", DbType.Int32);
            param[28].Value = offre.CabinetGestionnaire != null ? offre.CabinetGestionnaire.Code : 0;
            param[29] = new EacParameter("P_COURTIERAPP", DbType.Int32);
            param[29].Value = offre.CabinetApporteur != null ? offre.CabinetApporteur.Code : 0;
            param[30] = new EacParameter("P_REFCOURTIER", DbType.AnsiStringFixedLength);
            param[30].Value = offre.RefCourtier != null ? offre.RefCourtier.Replace("'", "'") : string.Empty;
            param[31] = new EacParameter("P_INTERLOCUTEUR", DbType.Int32);
            param[31].Value = (offre.Interlocuteur != null) ? offre.Interlocuteur.Id : 0;
            param[32] = new EacParameter("P_TYPEINTERLOCUTEUR", DbType.AnsiStringFixedLength);
            param[32].Value = offre.Interlocuteur != null ? "T" : string.Empty;
            param[33] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
            param[33].Value = offre.Branche != null ? !string.IsNullOrEmpty(offre.Branche.Code) ? offre.Branche.Code : string.Empty : string.Empty;
            param[34] = new EacParameter("P_ANNEESAISIE", DbType.Int32);
            param[34].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Year : 0;
            param[35] = new EacParameter("P_MOISSAISIE", DbType.Int32);
            param[35].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Month : 0;
            param[36] = new EacParameter("P_JOURSAISIE", DbType.Int32);
            param[36].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Day : 0;
            param[37] = new EacParameter("P_HEURESAISIE", DbType.Int32);
            param[37].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(offre.DateSaisie)) == null ? 0 : AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(offre.DateSaisie));
            param[38] = new EacParameter("P_ETAT", DbType.AnsiStringFixedLength);
            param[38].Value = "N";
            param[39] = new EacParameter("P_TYPETRAITEMENT", DbType.AnsiStringFixedLength);
            param[39].Value = AlbConstantesMetiers.TRAITEMENT_OFFRE;
            param[40] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
            param[40].Value = offre.Branche != null && offre.Branche.Cible != null ? offre.Branche.Cible.Code : string.Empty;
            param[41] = new EacParameter("P_LIENKPINVEN", DbType.Int32);
            param[41].Value = 0;
            param[42] = new EacParameter("P_LIENKPDESI", DbType.Int32);
            param[42].Value = 0;
            param[43] = new EacParameter("P_LIENKPOBSV", DbType.Int32);
            param[43].Value = 0;
            param[44] = new EacParameter("P_OBSERVATION", DbType.AnsiStringFixedLength);
            param[44].Value = !string.IsNullOrEmpty(offre.Observation) ? offre.Observation.Replace("'", "'") : string.Empty;
            param[45] = new EacParameter("P_OBSVCHRONO", DbType.Int32);
            param[45].Value = numeroChronoObservation;
            param[46] = new EacParameter("P_CODEINDICE", DbType.AnsiStringFixedLength);
            param[46].Value = (offre.IndiceReference != null && offre.IndiceReference.Code != null) ? offre.IndiceReference.Code : string.Empty;
            param[47] = new EacParameter("P_VALINDICEORIGINE", DbType.Int32);
            param[47].Value = offre.Valeur;
            param[48] = new EacParameter("P_VALINDICEACTUALISE", DbType.Int32);
            param[48].Value = offre.Valeur;
            param[49] = new EacParameter("P_INTERCALCOURTIER", DbType.AnsiStringFixedLength);
            param[49].Value = offre.IntercalaireCourtier ? "O" : "N";
            param[50] = new EacParameter("P_APPLICATNAT", DbType.AnsiStringFixedLength);
            param[50].Value = offre.SoumisCatNat;
            param[51] = new EacParameter("P_IDAPERITEUR", DbType.AnsiStringFixedLength);
            param[51].Value = offre.Aperiteur != null && !string.IsNullOrEmpty(offre.Aperiteur.Code) ? offre.Aperiteur.Code : string.Empty;
            param[52] = new EacParameter("P_POURCENTAPERITION", DbType.Int32);
            param[52].Value = offre.PartAperiteur;
            param[53] = new EacParameter("P_TAUXFRAISAPERITION", DbType.Int32);
            param[53].Value = offre.FraisAperition.HasValue ? offre.FraisAperition.Value : 0;
            param[54] = new EacParameter("P_AVENANT", DbType.Int32);
            param[54].Value = 0;
            param[55] = new EacParameter("P_TYPEACTEGESTION", DbType.AnsiStringFixedLength);
            param[55].Value = "G";
            param[56] = new EacParameter("P_TODAYYEAR", DbType.Int32);
            param[56].Value = DateTime.Now.Year;
            param[57] = new EacParameter("P_TODAYMONTH", DbType.Int32);
            param[57].Value = DateTime.Now.Month;
            param[58] = new EacParameter("P_TODAYDAY", DbType.Int32);
            param[58].Value = DateTime.Now.Day;
            param[59] = new EacParameter("P_TODAYDAY", DbType.Int32);
            param[59].Value = AlbConvert.ConvertTimeToIntMinute(DateTime.Now.TimeOfDay);
            param[60] = new EacParameter("P_USERTODAY", DbType.AnsiStringFixedLength);
            param[60].Value = user;
            param[61] = new EacParameter("P_DATESTATISTIQUE", DbType.Int32);
            param[61].Value = AlbConvert.ConvertDateToInt(offre.DateStatistique).HasValue ? AlbConvert.ConvertDateToInt(offre.DateStatistique).Value : 0;
            param[62] = new EacParameter("P_LTA", DbType.AnsiStringFixedLength);
            param[62].Value = offre.LTA ? "O" : "N";

            param[63] = new EacParameter("P_ANTECEDENT", DbType.AnsiStringFixedLength);
            param[63].Value = offre.ModeleFinOffreInfos.Antecedent;
            param[64] = new EacParameter("P_DESCRIPTION", DbType.AnsiStringFixedLength);
            param[64].Value = !string.IsNullOrEmpty(offre.ModeleFinOffreInfos.Description) ? offre.ModeleFinOffreInfos.Description.Replace("'", "''") : string.Empty;
            param[65] = new EacParameter("P_VALIDITEOFFRE", DbType.Int32);
            param[65].Value = offre.ModeleFinOffreInfos.ValiditeOffre;
            param[66] = new EacParameter("P_DATEPROJET", DbType.Int32);
            param[66].Value = AlbConvert.ConvertDateToInt(offre.ModeleFinOffreInfos.DateProjet);
            param[67] = new EacParameter("P_RELANCE", DbType.Int32);
            param[67].Value = AlbConvert.ConvertBoolToString(offre.ModeleFinOffreInfos.Relance);
            param[68] = new EacParameter("P_RELANCEVALEUR", DbType.Int32);
            param[68].Value = offre.ModeleFinOffreInfos.Relance ? offre.ModeleFinOffreInfos.RelanceValeur : 0;
            param[69] = new EacParameter("P_PREAVISRESIL", DbType.Int32);
            param[69].Value = offre.ModeleFinOffreInfos.Preavis;

            #endregion
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_MODIFIEROFFRE", param);

            return msgRetour;
        }

        #endregion

        #region Details Objet

        public static DateTime? GetDateDebObjHisto(string codeAffaire, string version, int codeRisque, int codeObjet, string codeAvn)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("codeRsq", DbType.Int32);
            param[2].Value = codeRisque;
            param[3] = new EacParameter("codeObj", DbType.Int32);
            param[3].Value = codeObjet;
            param[4] = new EacParameter("codeAvn", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) - 1 : 0;

            string sql = @"SELECT JGVDA * 100000000 + JGVDM * 1000000 + JGVDJ * 10000 + JGVDH DATEDEBRETURNCOL
                            FROM YHRTOBJ
                            WHERE JGIPB = :codeAffaire AND JGALX = :version AND JGRSQ = :codeRsq AND JGOBJ = :codeObj AND JGAVN = :codeAvn";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateDebReturnCol);
            }
            return null;
        }

        public static string SauvegarderDetailsObjetRisque(OffreDto offre, string user)
        {
            int numChronoAdresse = 0;
            string insAdresse = "N";
            if (offre.Risques != null && offre.Risques.Count > 0 && offre.Risques[0] != null && offre.Risques[0].Objets != null && offre.Risques[0].Objets.Count > 0 && offre.Risques[0].Objets[0].AdresseObjet != null)
            {
                if (offre.Risques[0].Objets[0].AdresseObjet.NumeroChrono == 0)
                {
                    insAdresse = "O";
                }
                else
                {
                    numChronoAdresse = offre.Risques[0].Objets[0].AdresseObjet.NumeroChrono;
                }

                int cp = 0;
                string codePostal = string.Empty;
                if (offre.Risques[0].Objets[0].AdresseObjet != null)
                {
                    if (offre.Risques[0].Objets[0].AdresseObjet.CodePostal.ToString().Length == 5)
                    {
                        int.TryParse(offre.Risques[0].Objets[0].AdresseObjet.CodePostal.ToString().Substring(2, 3), out cp);
                        codePostal = cp.ToString().PadLeft(3, '0');
                    }
                    else if (offre.Risques[0].Objets[0].AdresseObjet.CodePostal != -1)
                    {
                        codePostal = offre.Risques[0].Objets[0].AdresseObjet.CodePostal.ToString().PadLeft(3, '0');
                    }
                }

                string numeroVoie = string.Empty;
                if (offre.Risques[0].Objets[0].AdresseObjet.NumeroVoie != -1)
                {
                    numeroVoie = offre.Risques[0].Objets[0].AdresseObjet.NumeroVoie.ToString();
                }

                var dateNow = DateTime.Now;



                int cpCedex = 0;
                string codePostalCedex = string.Empty;
                if (offre.Risques[0].Objets[0].AdresseObjet.CodePostalCedex != 0)
                {
                    if (offre.Risques[0].Objets[0].AdresseObjet.CodePostalCedex.ToString().Length == 5)
                    {
                        Int32.TryParse(offre.Risques[0].Objets[0].AdresseObjet.CodePostalCedex.ToString().Substring(2, 3), out cpCedex);
                        codePostalCedex = cpCedex.ToString().PadLeft(3, '0');
                    }
                    else if (offre.Risques[0].Objets[0].AdresseObjet.CodePostalCedex != -1)
                    {
                        codePostalCedex = offre.Risques[0].Objets[0].AdresseObjet.CodePostalCedex.ToString().PadLeft(3, '0');
                    }
                }

                int isAddressEmpty = 0;

                EacParameter[] param = new EacParameter[74];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = offre.Version.Value;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = offre.Type;
                param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
                param[3].Value = offre.Risques[0].Code;
                param[4] = new EacParameter("P_CODEOBJ", DbType.Int32);
                param[4].Value = offre.Risques[0].Objets[0].Code;
                param[5] = new EacParameter("P_CHRONODESI", DbType.Int32);
                param[5].Value = offre.Risques[0].Objets[0].ChronoDesi;
                param[6] = new EacParameter("P_DESIGNATION", DbType.AnsiStringFixedLength);
                param[6].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Designation) ? offre.Risques[0].Objets[0].Designation.PadLeft(9, ' ') : string.Empty;
                param[7] = new EacParameter("P_ENTREEJOUR", DbType.Int32);
                param[7].Value = (offre.Risques[0].Objets[0].EntreeGarantie.HasValue ? offre.Risques[0].Objets[0].EntreeGarantie.Value.Day : 0);
                param[8] = new EacParameter("P_ENTREEMOIS", DbType.Int32);
                param[8].Value = (offre.Risques[0].Objets[0].EntreeGarantie.HasValue ? offre.Risques[0].Objets[0].EntreeGarantie.Value.Month : 0);
                param[9] = new EacParameter("P_ENTREEANNEE", DbType.Int32);
                param[9].Value = (offre.Risques[0].Objets[0].EntreeGarantie.HasValue ? offre.Risques[0].Objets[0].EntreeGarantie.Value.Year : 0);
                param[10] = new EacParameter("P_ENTREEHEURE", DbType.Int32);
                param[10].Value = (offre.Risques[0].Objets[0].EntreeGarantie.HasValue ? offre.Risques[0].Objets[0].EntreeGarantie.Value.Hour * 100 + offre.Risques[0].Objets[0].EntreeGarantie.Value.Minute : 0);
                param[11] = new EacParameter("P_SORTIEJOUR", DbType.Int32);
                param[11].Value = (offre.Risques[0].Objets[0].SortieGarantie.HasValue ? offre.Risques[0].Objets[0].SortieGarantie.Value.Day : 0);
                param[12] = new EacParameter("P_SORTIEMOIS", DbType.Int32);
                param[12].Value = (offre.Risques[0].Objets[0].SortieGarantie.HasValue ? offre.Risques[0].Objets[0].SortieGarantie.Value.Month : 0);
                param[13] = new EacParameter("P_SORTIEANNEE", DbType.Int32);
                param[13].Value = (offre.Risques[0].Objets[0].SortieGarantie.HasValue ? offre.Risques[0].Objets[0].SortieGarantie.Value.Year : 0);
                param[14] = new EacParameter("P_SORTIEHEURE", DbType.Int32);
                param[14].Value = (offre.Risques[0].Objets[0].SortieGarantie.HasValue ? offre.Risques[0].Objets[0].SortieGarantie.Value.Hour * 100 + offre.Risques[0].Objets[0].SortieGarantie.Value.Minute : 0);
                param[15] = new EacParameter("P_VALEUR", DbType.Int32);
                param[15].Value = (offre.Risques[0].Objets[0].Valeur.HasValue ? offre.Risques[0].Objets[0].Valeur.Value : 0);
                param[16] = new EacParameter("P_CODEUNITE", DbType.AnsiStringFixedLength);
                param[16].Value = offre.Risques[0].Objets[0].Unite != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Unite.Code) ? offre.Risques[0].Objets[0].Unite.Code : string.Empty;
                param[17] = new EacParameter("P_CODETYPE", DbType.AnsiStringFixedLength);
                param[17].Value = offre.Risques[0].Objets[0].Type != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Type.Code) ? offre.Risques[0].Objets[0].Type.Code : string.Empty;
                param[18] = new EacParameter("P_VALEURHT", DbType.AnsiStringFixedLength);
                param[18].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].ValeurHT) ? offre.Risques[0].Objets[0].ValeurHT : string.Empty;
                param[19] = new EacParameter("P_COUTM2", DbType.Int64);
                param[19].Value = offre.Risques[0].Objets[0].CoutM2.HasValue ? offre.Risques[0].Objets[0].CoutM2.Value : 0;
                param[20] = new EacParameter("P_CODEBRANCHE", DbType.AnsiStringFixedLength);
                param[20].Value = offre.Branche != null && !string.IsNullOrEmpty(offre.Branche.Code) ? offre.Branche.Code : string.Empty;
                param[21] = new EacParameter("P_CODEOBJET", DbType.Int32);
                param[21].Value = 1;
                param[22] = new EacParameter("P_DERNIEROBJET", DbType.Int32);
                param[22].Value = 1;
                param[23] = new EacParameter("P_NBOBJET", DbType.Int32);
                param[23].Value = 1;
                param[24] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
                param[24].Value = offre.Risques[0].Objets[0].Cible.Code;
                param[25] = new EacParameter("P_DESCRIPTIF", DbType.AnsiStringFixedLength);
                param[25].Value = offre.Risques[0].Objets[0].Descriptif;
                param[26] = new EacParameter("P_REPORTVALEUR", DbType.AnsiStringFixedLength);
                param[26].Value = !string.IsNullOrEmpty(offre.Risques[0].ReportValeur) ? offre.Risques[0].ReportValeur : string.Empty;
                param[27] = new EacParameter("P_REPORTOBLIG", DbType.AnsiStringFixedLength);
                param[27].Value = !string.IsNullOrEmpty(offre.Risques[0].ReportObligatoire) ? offre.Risques[0].ReportObligatoire : string.Empty;
                param[28] = new EacParameter("P_INSADR", DbType.AnsiStringFixedLength);
                param[28].Value = insAdresse;
                param[29] = new EacParameter("P_ADRBATIMENT", DbType.AnsiStringFixedLength);
                param[29].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.Batiment) ? offre.Risques[0].Objets[0].AdresseObjet.Batiment : string.Empty;
                param[30] = new EacParameter("P_ADRNUMVOIE", DbType.Int32);
                param[30].Value = numeroVoie; //Int32.Parse(offre.Risques[0].Objets[0].AdresseObjet.NumeroVoie.ToString());
                param[31] = new EacParameter("P_ADRNUMVOIE2", DbType.AnsiStringFixedLength);
                param[31].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.NumeroVoie2) ? offre.Risques[0].Objets[0].AdresseObjet.NumeroVoie2 : string.Empty;
                param[32] = new EacParameter("P_ADREXTVOIE", DbType.AnsiStringFixedLength);
                param[32].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.ExtensionVoie) ? offre.Risques[0].Objets[0].AdresseObjet.ExtensionVoie : string.Empty;
                param[33] = new EacParameter("P_ADRNOMVOIE", DbType.AnsiStringFixedLength);
                param[33].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.NomVoie) ? offre.Risques[0].Objets[0].AdresseObjet.NomVoie : string.Empty;
                param[34] = new EacParameter("P_ADRBP", DbType.AnsiStringFixedLength);
                param[34].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.BoitePostale) ? offre.Risques[0].Objets[0].AdresseObjet.BoitePostale : string.Empty;
                param[35] = new EacParameter("P_ADRCP", DbType.Int32);
                param[35].Value = codePostal;

                param[36] = new EacParameter("P_ADRDEP", DbType.AnsiStringFixedLength);
                param[36].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.Departement) ? offre.Risques[0].Objets[0].AdresseObjet.Departement : string.Empty;
                param[37] = new EacParameter("P_ADRVILLE", DbType.AnsiStringFixedLength);
                param[37].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.NomVille) ? offre.Risques[0].Objets[0].AdresseObjet.NomVille : offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.NomCedex) ? offre.Risques[0].Objets[0].AdresseObjet.NomCedex : string.Empty;



                param[38] = new EacParameter("P_ADRCPX", DbType.AnsiStringFixedLength);
                param[38].Value = codePostalCedex;
                param[39] = new EacParameter("P_ADRVILLEX", DbType.AnsiStringFixedLength);
                param[39].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.NomCedex) ? offre.Risques[0].Objets[0].AdresseObjet.NomCedex : string.Empty;
                param[40] = new EacParameter("P_ADRMATHEX", DbType.Int32);
                param[40].Value = offre.Risques[0].Objets[0].AdresseObjet != null && !string.IsNullOrEmpty(offre.Risques[0].Objets[0].AdresseObjet.MatriculeHexavia) ? Convert.ToInt32(offre.Risques[0].Objets[0].AdresseObjet.MatriculeHexavia) : 0;
                param[41] = new EacParameter("P_ADRNUMCHRONO", DbType.Int32);
                param[41].Value = numChronoAdresse;
                //Nomenclature d'objets
                param[42] = new EacParameter("P_APE", DbType.AnsiStringFixedLength);
                param[42].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].CodeApe) ? offre.Risques[0].Objets[0].CodeApe : string.Empty;
                param[43] = new EacParameter("P_NOMENCLATURE1", DbType.AnsiStringFixedLength);
                param[43].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Nomenclature1) ? offre.Risques[0].Objets[0].Nomenclature1 : string.Empty;
                param[44] = new EacParameter("P_NOMENCLATURE2", DbType.AnsiStringFixedLength);
                param[44].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Nomenclature2) ? offre.Risques[0].Objets[0].Nomenclature2 : string.Empty;
                param[45] = new EacParameter("P_NOMENCLATURE3", DbType.AnsiStringFixedLength);
                param[45].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Nomenclature3) ? offre.Risques[0].Objets[0].Nomenclature3 : string.Empty;
                param[46] = new EacParameter("P_NOMENCLATURE4", DbType.AnsiStringFixedLength);
                param[46].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Nomenclature4) ? offre.Risques[0].Objets[0].Nomenclature4 : string.Empty;
                param[47] = new EacParameter("P_NOMENCLATURE5", DbType.AnsiStringFixedLength);
                param[47].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Nomenclature5) ? offre.Risques[0].Objets[0].Nomenclature5 : string.Empty;
                param[48] = new EacParameter("P_TERRITORIALITE", DbType.AnsiStringFixedLength);
                param[48].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].Territorialite) ? offre.Risques[0].Objets[0].Territorialite : string.Empty;
                param[49] = new EacParameter("P_TRE", DbType.AnsiStringFixedLength);
                param[49].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].CodeTre) ? offre.Risques[0].Objets[0].CodeTre.Split('-')[0].Trim() : string.Empty;
                param[50] = new EacParameter("P_CLASSE", DbType.AnsiStringFixedLength);
                param[50].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].CodeClasse) ? offre.Risques[0].Objets[0].CodeClasse : string.Empty;
                param[51] = new EacParameter("P_TYPERISQUE", DbType.AnsiStringFixedLength);
                param[51].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].TypeRisque) ? offre.Risques[0].Objets[0].TypeRisque : string.Empty;
                param[52] = new EacParameter("P_TYPEMATERIEL", DbType.AnsiStringFixedLength);
                param[52].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].TypeMateriel) ? offre.Risques[0].Objets[0].TypeMateriel : string.Empty;
                param[53] = new EacParameter("P_NATURELIEUX", DbType.AnsiStringFixedLength);
                param[53].Value = !string.IsNullOrEmpty(offre.Risques[0].Objets[0].NatureLieux) ? offre.Risques[0].Objets[0].NatureLieux : string.Empty;
                param[54] = new EacParameter("P_DATEENTREEDESC", DbType.Int32);
                param[54].Value = offre.Risques[0].Objets[0].DateEntreeDescr.HasValue ? AlbConvert.ConvertDateToInt(offre.Risques[0].Objets[0].DateEntreeDescr) : 0;
                param[55] = new EacParameter("P_HEUREENTREEDESC", DbType.Int32);
                param[55].Value = offre.Risques[0].Objets[0].DateEntreeDescr != null ? offre.Risques[0].Objets[0].DateEntreeDescr.Value.Hour * 100 + offre.Risques[0].Objets[0].DateEntreeDescr.Value.Minute : 0;
                param[56] = new EacParameter("P_DATESORTIEDESC", DbType.Int32);
                param[56].Value = offre.Risques[0].Objets[0].DateSortieDescr.HasValue ? AlbConvert.ConvertDateToInt(offre.Risques[0].Objets[0].DateSortieDescr) : 0;
                param[57] = new EacParameter("P_HEURESORTIEDESC", DbType.Int32);
                param[57].Value = offre.Risques[0].Objets[0].DateSortieDescr != null ? offre.Risques[0].Objets[0].DateSortieDescr.Value.Hour * 100 + offre.Risques[0].Objets[0].DateSortieDescr.Value.Minute : 0;

                param[58] = new EacParameter("P_MODAVENANTLOCALE", DbType.AnsiStringFixedLength);
                param[58].Value = offre.Risques[0].Objets[0].IsAvenantModificationLocale ? "O" : "N";
                param[59] = new EacParameter("P_DATEEFFETAVNLOCALANNEE", DbType.Int32);
                param[59].Value = offre.Risques[0].Objets[0].DateEffetAvenantModificationLocale.HasValue ? offre.Risques[0].Objets[0].DateEffetAvenantModificationLocale.Value.Year : 0;
                param[60] = new EacParameter("P_DATEEFFETAVNLOCALMOIS", DbType.Int32);
                param[60].Value = offre.Risques[0].Objets[0].DateEffetAvenantModificationLocale.HasValue ? offre.Risques[0].Objets[0].DateEffetAvenantModificationLocale.Value.Month : 0;
                param[61] = new EacParameter("P_DATEEFFETAVNLOCALJOUR", DbType.Int32);
                param[61].Value = offre.Risques[0].Objets[0].DateEffetAvenantModificationLocale.HasValue ? offre.Risques[0].Objets[0].DateEffetAvenantModificationLocale.Value.Day : 0;
                param[62] = new EacParameter("P_ISRISQUETEMPORAIRE", DbType.AnsiStringFixedLength);
                param[62].Value = offre.Risques[0].Objets[0].IsRisqueTemporaire ? "O" : "N";
                param[63] = new EacParameter("P_DATESYSTEM", DbType.Int32);
                param[63].Value = DateTime.Now.ToString("yyyyMMdd");
                param[64] = new EacParameter("P_HEURESYSTEM", DbType.Int32);
                param[64].Value = DateTime.Now.ToString("HHmmss");
                param[65] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
                param[65].Value = user;
                param[66] = new EacParameter("P_ISADDRESSEMPTY", DbType.Int32);


                var tmp = string.Concat(param[29].Value.ToString(), param[30].Value.ToString(), param[31].Value.ToString(), param[32].Value.ToString(), param[33].Value.ToString(),
                param[34].Value.ToString(), param[35].Value.ToString(), param[36].Value.ToString(), param[37].Value.ToString(), param[38].Value.ToString(), param[39].Value.ToString());

                if (tmp.Trim() == "")
                {
                    isAddressEmpty = 1;
                }
                param[66].Value = isAddressEmpty;

                /***** Ajout Latitude et Longitude *****/
                param[67] = new EacParameter("P_LATITUDE", (offre.Risques[0].Objets[0].AdresseObjet.Latitude.HasValue) ? offre.Risques[0].Objets[0].AdresseObjet.Latitude.Value : 0);
                param[68] = new EacParameter("P_LONGITUDE", (offre.Risques[0].Objets[0].AdresseObjet.Longitude.HasValue) ? offre.Risques[0].Objets[0].AdresseObjet.Longitude : 0);
                /****** Fin  Ajout Latitude et Longitude ******/
                //********Date de mise a jour*****************/
                param[69] = new EacParameter("P_DATEMISEAJOUR", DbType.Int32);
                param[69].Value = offre.Risques[0].Objets[0].DateModificationObjetRisque.HasValue ? AlbConvert.ConvertDateToInt(offre.Risques[0].Objets[0].DateModificationObjetRisque) : 0;

                param[70] = new EacParameter("P_DATEMODIFAVNLOCALANNEE", DbType.Int32);
                param[70].Value = offre.Risques[0].Objets[0].DateModificationObjetRisque.HasValue ? offre.Risques[0].Objets[0].DateModificationObjetRisque.Value.Year : 0;
                param[71] = new EacParameter("P_DATEMODIFAVNLOCALMOIS", DbType.Int32);
                param[71].Value = offre.Risques[0].Objets[0].DateModificationObjetRisque.HasValue ? offre.Risques[0].Objets[0].DateModificationObjetRisque.Value.Month : 0;
                param[72] = new EacParameter("P_DATEMODIFAVNLOCALJOUR", DbType.Int32);
                param[72].Value = offre.Risques[0].Objets[0].DateModificationObjetRisque.HasValue ? offre.Risques[0].Objets[0].DateModificationObjetRisque.Value.Day : 0;


                param[73] = new EacParameter("P_OUTCODERSQOBJ", DbType.AnsiStringFixedLength);
                param[73].Value = "";
                param[73].DbType = DbType.String;
                param[73].Size = 50;
                param[73].Direction = ParameterDirection.InputOutput;



                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEOBJ", param);

                return param[73].Value.ToString();
            }
            else
                return string.Empty;
        }




        public static void DeleteDetailsObjet(OffreDto offre)
        {
            if (offre != null && offre.Risques != null && offre.Risques.Count > 0)
            {
                EacParameter[] param = new EacParameter[6];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = offre.Version.HasValue ? offre.Version.Value : 0;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = offre.Type;
                param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
                param[3].Value = offre.Risques[0].Code;
                param[4] = new EacParameter("P_CODEOBJ", DbType.Int32);
                param[4].Value = offre.Risques[0].Objets[0].Code;
                param[5] = new EacParameter("P_DELRSQ", DbType.Int32);
                param[5].Value = 0;

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELOBJETRISQUE", param);

            }
        }

        public static void SaveValeurModeAvn(string codeOffre, string version, string type, string valeur)
        {
            //TODO : sauvegarde en BDD
            //Appel PGM 400
        }
        #region Recherche Activites

        public static RechercheActiviteDto GetActivites(string code, string branche, string cible, string nom, int StartLine, int EndLine)
        {
            RechercheActiviteDto ToReturn = new RechercheActiviteDto();
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODE", DbType.AnsiStringFixedLength);
            param[0].Value = !string.IsNullOrEmpty(code) ? code : "";
            param[1] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
            param[1].Value = !string.IsNullOrEmpty(branche) ? branche.Replace("'", "''") : "";
            param[2] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
            param[2].Value = !string.IsNullOrEmpty(cible) ? cible.Replace("'", "''") : "";
            param[3] = new EacParameter("P_NOM", DbType.AnsiStringFixedLength);
            param[3].Value = !string.IsNullOrEmpty(nom) ? nom.Replace("'", "''") : "";
            param[4] = new EacParameter("P_STARTLINE", DbType.Int32);
            param[4].Value = StartLine;
            param[5] = new EacParameter("P_ENDLINE", DbType.Int32);
            param[5].Value = EndLine;
            param[6] = new EacParameter("P_REQUESTCOUNT", DbType.Int32);
            param[6].Direction = ParameterDirection.InputOutput;
            param[6].Value = 0;
            ToReturn.ListActivites = DbBase.Settings.ExecuteList<ActiviteDto>(CommandType.StoredProcedure, "SP_RECHERCHEACTIVITE", param);
            ToReturn.ResultCount = Convert.ToInt32(param[6].Value);
            return ToReturn;
        }
        public static bool ChekConceptFamille(string cible)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("cible", DbType.AnsiStringFixedLength);
            param[0].Value = cible;
            /* recuperation de la cile et la famille de la cible En cours  */
            string sql = @"select KAHCON STRRETURNCOL, KAHFAM STRRETURNCOL2 
                                            from KCIBLE
                                            WHERE KAHCIBLE= :cible";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            /** nombre */
            if (result != null && result.Any())
            {
                return !string.IsNullOrEmpty(result.FirstOrDefault().StrReturnCol) && !string.IsNullOrEmpty(result.FirstOrDefault().StrReturnCol2);
            }
            return false;
        }

        public static string LoadCodeClassByCible(string codeCible, string codeActivite)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
            param[0].Value = codeCible;
            param[1] = new EacParameter("codeActivite", DbType.AnsiStringFixedLength);
            param[1].Value = codeActivite;

            string sql = @"SELECT TPCA1 STRRETURNCOL FROM YYYYPAR
                                INNER JOIN KCIBLE ON KAHCON = TCON AND KAHFAM = TFAM AND KAHCIBLE = :codeCible
                            WHERE TCOD = :codeActivite";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
                return result.FirstOrDefault().StrReturnCol;
            return string.Empty;
        }

        #endregion

        #endregion
        #region Details Risque

        public static DateTime? GetDateDebRsqHisto(string codeAffaire, string version, int codeRisque, string codeAvn)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("codeRsq", DbType.Int32);
            param[2].Value = codeRisque;
            param[3] = new EacParameter("codeAvn", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) - 1 : 0;

            string sql = @"SELECT JEVDA * 100000000 + JEVDM * 1000000 + JEVDJ * 10000 + JEVDH DATEDEBRETURNCOL
                            FROM YHRTRSQ
                            WHERE JEIPB = :codeAffaire AND JEALX = :version AND JERSQ = :codeRsq AND JEAVN = :codeAvn";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateDebReturnCol);
            }
            return null;
        }

        public static string SauvegarderDetailsRisque(OffreDto offre, string user)
        {
            if (offre != null && offre.Risques != null && offre.Risques.Count > 0)
            {
                string voieComplete = string.Empty;
                string l6f = string.Empty;
                string codePostalCedex = string.Empty;
                string codePostal = string.Empty;
                string numeroVoie = string.Empty;

                int matricule = 0;
                int isAddressEmpty = 0;

                if (offre.Risques[0].AdresseRisque != null)
                {
                    int cp = 0;

                    if (offre.Risques[0].AdresseRisque.CodePostal.ToString().Length == 5)
                    {
                        int.TryParse(offre.Risques[0].AdresseRisque.CodePostal.ToString().Substring(2, 3), out cp);
                        codePostal = cp.ToString().PadLeft(3, '0');
                    }
                    else if (offre.Risques[0].AdresseRisque.CodePostal != -1)
                    {
                        codePostal = offre.Risques[0].AdresseRisque.CodePostal.ToString().PadLeft(3, '0');
                    }

                    l6f = string.Format(CultureInfo.CurrentCulture, "{0}{1} {2}", offre.Risques[0].AdresseRisque.Departement, codePostal, offre.Risques[0].AdresseRisque.NomVille);

                    if (offre.Risques[0].AdresseRisque.NumeroVoie != -1)
                    {
                        numeroVoie = offre.Risques[0].AdresseRisque.NumeroVoie.ToString();
                    }

                    int cpCedex = 0;

                    if (offre.Risques[0].AdresseRisque.CodePostalCedex != 0)
                    {
                        if (offre.Risques[0].AdresseRisque.CodePostalCedex.ToString().Length == 5)
                        {
                            Int32.TryParse(offre.Risques[0].AdresseRisque.CodePostalCedex.ToString().Substring(2, 3), out cpCedex);
                            codePostalCedex = cpCedex.ToString().PadLeft(3, '0');
                        }
                        else if (offre.Risques[0].AdresseRisque.CodePostalCedex != -1)
                        {
                            codePostalCedex = offre.Risques[0].AdresseRisque.CodePostalCedex.ToString().PadLeft(3, '0');
                        }
                    }


                }

                voieComplete = String.Format(CultureInfo.CurrentCulture, "{0} {1}", numeroVoie, offre.Risques[0].AdresseRisque.ExtensionVoie);
                voieComplete = String.Format(CultureInfo.CurrentCulture, "{0} {1}", voieComplete.Trim(), offre.Risques[0].AdresseRisque.NomVoie);

                //if (offre.Branche == null)
                //{
                //    offre.Branche = new BrancheDto { Code = offre.CodeOffre.Substring(0, 2) };
                //}

                EacParameter[] param = new EacParameter[69];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = offre.Version;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = offre.Type;
                param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
                param[3].Value = offre.Risques[0].Code;
                param[4] = new EacParameter("P_CHRONODESI", DbType.Int32);
                param[4].Value = offre.Risques[0].ChronoDesi;
                param[5] = new EacParameter("P_DESIGNATION", DbType.AnsiStringFixedLength);
                param[5].Value = !string.IsNullOrEmpty(offre.Risques[0].Designation) ? offre.Risques[0].Designation : string.Empty;
                param[6] = new EacParameter("P_ENTREEJOUR", DbType.Int32);
                param[6].Value = (offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Day : 0);
                param[7] = new EacParameter("P_ENTREEMOIS", DbType.Int32);
                param[7].Value = (offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Month : 0);
                param[8] = new EacParameter("P_ENTREEANNEE", DbType.Int32);
                param[8].Value = (offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Year : 0);
                param[9] = new EacParameter("P_ENTREEHEURE", DbType.Int32);
                param[9].Value = (offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Hour * 100 + offre.Risques[0].EntreeGarantie.Value.Minute : 0);
                param[10] = new EacParameter("P_SORTIEJOUR", DbType.Int32);
                param[10].Value = (offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Day : 0);
                param[11] = new EacParameter("P_SORTIEMOIS", DbType.Int32);
                param[11].Value = (offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Month : 0);
                param[12] = new EacParameter("P_SORTIEANNEE", DbType.Int32);
                param[12].Value = (offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Year : 0);
                param[13] = new EacParameter("P_SORTIEHEURE", DbType.Int32);
                param[13].Value = (offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Hour * 100 + offre.Risques[0].SortieGarantie.Value.Minute : 0);
                param[14] = new EacParameter("P_VALEUR", DbType.Int32);
                param[14].Value = (offre.Risques[0].Valeur != null ? offre.Risques[0].Valeur : 0);
                param[15] = new EacParameter("P_CODEUNITE", DbType.AnsiStringFixedLength);
                param[15].Value = offre.Risques[0].Unite != null && !string.IsNullOrEmpty(offre.Risques[0].Unite.Code) ? offre.Risques[0].Unite.Code : string.Empty;
                param[16] = new EacParameter("P_CODETYPE", DbType.AnsiStringFixedLength);
                param[16].Value = offre.Risques[0].Type != null && !string.IsNullOrEmpty(offre.Risques[0].Type.Code) ? offre.Risques[0].Type.Code : string.Empty;
                param[17] = new EacParameter("P_VALEURHT", DbType.AnsiStringFixedLength);
                param[17].Value = !string.IsNullOrEmpty(offre.Risques[0].ValeurHT) ? offre.Risques[0].ValeurHT : string.Empty;
                //param[18] = new EacParameter("P_COUTM2", DbType.Int64);
                //param[18].Value = (offre.Risques[0].CoutM2 != null ? offre.Risques[0].CoutM2 : 0);
                param[18] = new EacParameter("P_CODEBRANCHE", DbType.AnsiStringFixedLength);
                param[18].Value = offre.Branche != null && !string.IsNullOrEmpty(offre.Branche.Code) ? offre.Branche.Code : string.Empty;
                param[19] = new EacParameter("P_CODEOBJ", DbType.Int32);
                param[19].Value = 1;
                param[20] = new EacParameter("P_DERNIEROBJET", DbType.Int32);
                param[20].Value = 1;
                param[21] = new EacParameter("P_NBOBJET", DbType.Int32);
                param[21].Value = 1;
                param[22] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
                param[22].Value = offre.Risques[0].Cible != null ? (!string.IsNullOrEmpty(offre.Risques[0].Cible.Code) ? offre.Risques[0].Cible.Code : string.Empty) : string.Empty;
                param[23] = new EacParameter("P_DESCRIPTIF", DbType.AnsiStringFixedLength);
                param[23].Value = offre.Risques[0].Descriptif;
                param[24] = new EacParameter("P_REPORTVALEUR", DbType.AnsiStringFixedLength);
                param[24].Value = !string.IsNullOrEmpty(offre.Risques[0].ReportValeur) ? offre.Risques[0].ReportValeur : string.Empty;
                param[25] = new EacParameter("P_REPORTOBLIG", DbType.AnsiStringFixedLength);
                param[25].Value = !string.IsNullOrEmpty(offre.Risques[0].ReportObligatoire) ? offre.Risques[0].ReportObligatoire : string.Empty;

                //Nomenclature de risques
                param[26] = new EacParameter("P_APE", DbType.AnsiStringFixedLength);
                param[26].Value = !string.IsNullOrEmpty(offre.Risques[0].CodeApe) ? offre.Risques[0].CodeApe : string.Empty;
                param[27] = new EacParameter("P_NOMENCLATURE1", DbType.AnsiStringFixedLength);
                param[27].Value = !string.IsNullOrEmpty(offre.Risques[0].Nomenclature1) ? offre.Risques[0].Nomenclature1 : string.Empty;
                param[28] = new EacParameter("P_NOMENCLATURE2", DbType.AnsiStringFixedLength);
                param[28].Value = !string.IsNullOrEmpty(offre.Risques[0].Nomenclature2) ? offre.Risques[0].Nomenclature2 : string.Empty;
                param[29] = new EacParameter("P_NOMENCLATURE3", DbType.AnsiStringFixedLength);
                param[29].Value = !string.IsNullOrEmpty(offre.Risques[0].Nomenclature3) ? offre.Risques[0].Nomenclature3 : string.Empty;
                param[30] = new EacParameter("P_NOMENCLATURE4", DbType.AnsiStringFixedLength);
                param[30].Value = !string.IsNullOrEmpty(offre.Risques[0].Nomenclature4) ? offre.Risques[0].Nomenclature4 : string.Empty;
                param[31] = new EacParameter("P_NOMENCLATURE5", DbType.AnsiStringFixedLength);
                param[31].Value = !string.IsNullOrEmpty(offre.Risques[0].Nomenclature5) ? offre.Risques[0].Nomenclature5 : string.Empty;
                param[32] = new EacParameter("P_TERRITORIALITE", DbType.AnsiStringFixedLength);
                param[32].Value = !string.IsNullOrEmpty(offre.Risques[0].Territorialite) ? offre.Risques[0].Territorialite : string.Empty;
                param[33] = new EacParameter("P_TRE", DbType.AnsiStringFixedLength);
                param[33].Value = !string.IsNullOrEmpty(offre.Risques[0].CodeTre) ? offre.Risques[0].CodeTre.Split('-')[0] : string.Empty;
                param[34] = new EacParameter("P_CLASSE", DbType.AnsiStringFixedLength);
                param[34].Value = !string.IsNullOrEmpty(offre.Risques[0].CodeClasse) ? offre.Risques[0].CodeClasse : string.Empty;
                param[35] = new EacParameter("P_TYPERISQUE", DbType.AnsiStringFixedLength);
                param[35].Value = !string.IsNullOrEmpty(offre.Risques[0].TypeRisque) ? offre.Risques[0].TypeRisque : string.Empty;
                param[36] = new EacParameter("P_TYPEMATERIEL", DbType.AnsiStringFixedLength);
                param[36].Value = !string.IsNullOrEmpty(offre.Risques[0].TypeMateriel) ? offre.Risques[0].TypeMateriel : string.Empty;
                param[37] = new EacParameter("P_NATURELIEUX", DbType.AnsiStringFixedLength);
                param[37].Value = !string.IsNullOrEmpty(offre.Risques[0].NatureLieux) ? offre.Risques[0].NatureLieux : string.Empty;
                param[38] = new EacParameter("P_DATEENTREEDESC", DbType.Int32);
                param[38].Value = offre.Risques[0].DateEntreeDescr.HasValue ? AlbConvert.ConvertDateToInt(offre.Risques[0].DateEntreeDescr) : 0;
                param[39] = new EacParameter("P_HEUREENTREEDESC", DbType.Int32);
                param[39].Value = offre.Risques[0].DateEntreeDescr != null ? offre.Risques[0].DateEntreeDescr.Value.Hour * 100 + offre.Risques[0].DateEntreeDescr.Value.Minute : 0;
                param[40] = new EacParameter("P_DATESORTIEDESC", DbType.Int32);
                param[40].Value = offre.Risques[0].DateSortieDescr.HasValue ? AlbConvert.ConvertDateToInt(offre.Risques[0].DateSortieDescr) : 0;
                param[41] = new EacParameter("P_HEURESORTIEDESC", DbType.Int32);
                param[41].Value = offre.Risques[0].DateSortieDescr != null ? offre.Risques[0].DateSortieDescr.Value.Hour * 100 + offre.Risques[0].DateSortieDescr.Value.Minute : 0;
                param[42] = new EacParameter("P_BATIMENT", DbType.AnsiStringFixedLength);
                param[42].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.Batiment) ? offre.Risques[0].AdresseRisque.Batiment : string.Empty : string.Empty;
                param[43] = new EacParameter("P_NUMVOIE", DbType.Int32);
                param[43].Value = numeroVoie;
                param[44] = new EacParameter("P_NUMVOIE2", DbType.AnsiStringFixedLength);
                param[44].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.NumeroVoie2) ? offre.Risques[0].AdresseRisque.NumeroVoie2 : string.Empty : string.Empty;
                param[45] = new EacParameter("P_EXTVOIE", DbType.AnsiStringFixedLength);
                param[45].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.ExtensionVoie) ? offre.Risques[0].AdresseRisque.ExtensionVoie : string.Empty : string.Empty;
                param[46] = new EacParameter("P_NOMVOIE", DbType.AnsiStringFixedLength);
                param[46].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.NomVoie) ? offre.Risques[0].AdresseRisque.NomVoie : string.Empty : string.Empty;
                param[47] = new EacParameter("P_BP", DbType.AnsiStringFixedLength);
                param[47].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.BoitePostale) ? offre.Risques[0].AdresseRisque.BoitePostale : string.Empty : string.Empty;
                param[48] = new EacParameter("P_LOCALITE", DbType.AnsiStringFixedLength);
                param[48].Value = offre.Risques[0].AdresseRisque != null ? offre.Risques[0].AdresseRisque.CodePostal.ToString().Length == 5 ? offre.Risques[0].AdresseRisque.CodePostal.ToString() : offre.Risques[0].AdresseRisque.Departement + codePostal : string.Empty;
                param[49] = new EacParameter("P_DEP", DbType.AnsiStringFixedLength);
                param[49].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.Departement) ? offre.Risques[0].AdresseRisque.Departement : string.Empty : string.Empty;
                param[50] = new EacParameter("P_CP", DbType.Int32);
                param[50].Value = codePostal;
                param[51] = new EacParameter("P_NOMVILLE", DbType.AnsiStringFixedLength);
                param[51].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.NomVille) ? offre.Risques[0].AdresseRisque.NomVille : string.Empty : string.Empty;
                param[52] = new EacParameter("P_VOIECOMPLETE", DbType.AnsiStringFixedLength);
                param[52].Value = voieComplete;
                param[53] = new EacParameter("P_VILLECOMPLETE", DbType.AnsiStringFixedLength);
                param[53].Value = l6f;
                param[54] = new EacParameter("P_CPX", DbType.Int32);
                param[54].Value = codePostalCedex;
                param[55] = new EacParameter("P_NOMCEDEX", DbType.AnsiStringFixedLength);
                param[55].Value = offre.Risques[0].AdresseRisque != null ? !string.IsNullOrEmpty(offre.Risques[0].AdresseRisque.NomCedex) ? offre.Risques[0].AdresseRisque.NomCedex : string.Empty : string.Empty;
                param[56] = new EacParameter("P_MATHEX", DbType.Int32);
                param[56].Value = offre.Risques[0].AdresseRisque != null ? int.TryParse(offre.Risques[0].AdresseRisque.MatriculeHexavia, out matricule) ? matricule : matricule : 0;
                param[57] = new EacParameter("P_MODAVENANTLOCALE", DbType.AnsiStringFixedLength);
                param[57].Value = offre.Risques[0].IsTraceAvnExist ? "O" : "N";
                param[58] = new EacParameter("P_DATEEFFETAVNLOCALANNEE", DbType.Int32);
                param[58].Value = offre.Risques[0].DateEffetAvenantModificationLocale.HasValue ? offre.Risques[0].DateEffetAvenantModificationLocale.Value.Year : 0;
                param[59] = new EacParameter("P_DATEEFFETAVNLOCALMOIS", DbType.Int32);
                param[59].Value = offre.Risques[0].DateEffetAvenantModificationLocale.HasValue ? offre.Risques[0].DateEffetAvenantModificationLocale.Value.Month : 0;
                param[60] = new EacParameter("P_DATEEFFETAVNLOCALJOUR", DbType.Int32);
                param[60].Value = offre.Risques[0].DateEffetAvenantModificationLocale.HasValue ? offre.Risques[0].DateEffetAvenantModificationLocale.Value.Day : 0;
                param[61] = new EacParameter("P_ISRISQUETEMPORAIRE", DbType.AnsiStringFixedLength);
                param[61].Value = offre.Risques[0].IsRisqueTemporaire ? "O" : "N";
                param[62] = new EacParameter("P_DATESYSTEM", DbType.Int32);
                param[62].Value = DateTime.Now.ToString("yyyyMMdd");
                param[63] = new EacParameter("P_HEURESYSTEM", DbType.Int32);
                param[63].Value = DateTime.Now.ToString("HHmmss");
                param[64] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
                param[64].Value = user;
                param[65] = new EacParameter("P_ISADDRESSEMPTY", DbType.Int32);


                var tmp = string.Concat(param[42].Value.ToString(), param[43].Value.ToString(), param[44].Value.ToString(), param[45].Value.ToString(), param[46].Value.ToString(), param[47].Value.ToString(), param[48].Value.ToString(),
                param[49].Value.ToString(), param[50].Value.ToString(), param[51].Value.ToString(), param[53].Value.ToString(), param[54].Value.ToString(), param[55].Value.ToString());

                if (tmp.Trim() == "")
                {
                    isAddressEmpty = 1;
                }
                param[65].Value = isAddressEmpty;

                /***** Ajout Latitude et Longitude *****/
                param[66] = new EacParameter("P_LATITUDE", (offre.Risques[0].AdresseRisque.Latitude.HasValue) ? offre.Risques[0].AdresseRisque.Latitude : 0);
                param[67] = new EacParameter("P_LONGITUDE", (offre.Risques[0].AdresseRisque.Longitude.HasValue) ? offre.Risques[0].AdresseRisque.Longitude : 0);
                /****** Fin  Ajout Latitude et Longitude ******/

                param[68] = new EacParameter("P_OUTCODERSQ", DbType.Int32);
                param[68].DbType = DbType.Int32;
                param[68].Direction = ParameterDirection.InputOutput;
                param[68].Value = 0;

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVERSQ", param);

                return param[68].Value.ToString();
            }
            else
                return string.Empty;
        }

        public static void UpdateDetailsRisque_YPRTRSQ(OffreDto offre, bool infosDetails = false)
        {
            string sousBranche = string.Empty;
            string categorie = string.Empty;
            var infoBrancheCible = CommonRepository.GetSousBrancheCategorie(offre.Branche != null ? offre.Branche.Code : string.Empty, offre.Branche != null && offre.Branche.Cible != null ? offre.Branche.Cible.Code : string.Empty);
            if (infoBrancheCible != null)
            {
                sousBranche = infoBrancheCible.SousBranche;
                categorie = infoBrancheCible.Categorie;
            }

            string sql = string.Empty;
            if (offre.Risques is null)
            {
                EacParameter[] param = new EacParameter[2];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("versionValue", DbType.Int32);
                param[1].Value = offre.Version.HasValue ? offre.Version.Value : 0;

                sql = @"SELECT COUNT(*) NBLIGN FROM YPRTRSQ WHERE JEIPB = :codeOffre AND JEALX = :versionValue";
                if (CommonRepository.RowNumber(sql) == 0)
                {
                    var risques = new List<RisqueDto> { new RisqueDto { Code = 1 } };
                    offre.Risques = risques;
                }
            }

            if (offre.Risques?.Any() ?? false)
            {
                EacParameter[] param = new EacParameter[3];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.Int32);
                param[1].Value = offre.Version;
                param[2] = new EacParameter("risques", DbType.Int32);
                param[2].Value = offre.Risques[0].Code;
                string sqlCountRsq = @"SELECT COUNT(*) NBLIGN FROM YPRTRSQ
                                        WHERE JEIPB=:codeOffre AND JEALX=:version AND JERSQ= :risques";

                if (CommonRepository.ExistRowParam(sqlCountRsq, param))
                {
                    if (infosDetails)
                    {
                        EacParameter[] subParam = new EacParameter[14];
                        subParam[0] = new EacParameter("risqueIndexe", DbType.AnsiStringFixedLength);
                        subParam[0].Value = offre.Risques[0].RisqueIndexe ? "O" : String.Empty;
                        subParam[1] = new EacParameter("LCI", DbType.AnsiStringFixedLength);
                        subParam[1].Value = offre.Risques[0].LCI ? "O" : String.Empty;
                        subParam[2] = new EacParameter("franchise", DbType.AnsiStringFixedLength);
                        subParam[2].Value = offre.Risques[0].Franchise ? "O" : String.Empty;
                        subParam[3] = new EacParameter("assiette", DbType.AnsiStringFixedLength);
                        subParam[3].Value = offre.Risques[0].Assiette ? "O" : String.Empty;
                        subParam[4] = new EacParameter("regimeTaxe", DbType.AnsiStringFixedLength);
                        subParam[4].Value = offre.Risques[0].RegimeTaxe;
                        subParam[5] = new EacParameter("CATNAT", DbType.AnsiStringFixedLength);
                        subParam[5].Value = offre.Risques[0].CATNAT ? "O" : "N";
                        subParam[6] = new EacParameter("tauxAppel", DbType.Int32);
                        subParam[6].Value = offre.Risques[0].TauxAppel;
                        subParam[7] = new EacParameter("isRegularisable", DbType.AnsiStringFixedLength);
                        subParam[7].Value = offre.Risques[0].IsRegularisable;
                        subParam[8] = new EacParameter("typeRegularisation", DbType.AnsiStringFixedLength);
                        subParam[8].Value = offre.Risques[0].TypeRegularisation != null ? offre.Risques[0].TypeRegularisation : string.Empty;
                        subParam[9] = new EacParameter("partBenef", DbType.AnsiStringFixedLength);
                        subParam[9].Value = offre.Risques[0].PartBenef != null ? offre.Risques[0].PartBenef : string.Empty;
                        subParam[10] = new EacParameter("ristourne", DbType.Int32);
                        subParam[10].Value = offre.Risques[0].Ristourne;
                        subParam[11] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                        subParam[11].Value = offre.CodeOffre.PadLeft(9, ' ');
                        subParam[12] = new EacParameter("version", DbType.Int32);
                        subParam[12].Value = offre.Version;
                        subParam[13] = new EacParameter("code", DbType.Int32);
                        subParam[13].Value = offre.Risques[0].Code;

                        sql = @"UPDATE YPRTRSQ SET JEINA = :risqueIndexe, JEIXL = :LCI, JEIXF = :franchise, JEIXC = :assiette, JERGT = :regimeTaxe,
                                            JECNA = :CATNAT, JEPBT = :tauxAppel, JERUL = :isRegularisable, JERUT = :typeRegularisation, JEPBN = :partBenef, JEPBR = :ristourne
                                            WHERE JEIPB = :codeOffre AND JEALX = :version AND JERSQ = :code";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, subParam);
                    }
                    else
                    {
                        EacParameter[] subParam = new EacParameter[16];
                        subParam[0] = new EacParameter("dayIn", DbType.Int32);
                        subParam[0].Value = offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Day : 0;
                        subParam[1] = new EacParameter("monthIn", DbType.Int32);
                        subParam[1].Value = offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Month : 0;
                        subParam[2] = new EacParameter("yearIn", DbType.Int32);
                        subParam[2].Value = offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Year : 0;
                        subParam[3] = new EacParameter("timeIn", DbType.Int32);
                        subParam[3].Value = offre.Risques[0].EntreeGarantie != null ? offre.Risques[0].EntreeGarantie.Value.Hour * 100 + offre.Risques[0].EntreeGarantie.Value.Minute : 0;
                        subParam[4] = new EacParameter("dayOut", DbType.Int32);
                        subParam[4].Value = offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Day : 0;
                        subParam[5] = new EacParameter("monthOut", DbType.Int32);
                        subParam[5].Value = offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Month : 0;
                        subParam[6] = new EacParameter("yearOut", DbType.Int32);
                        subParam[6].Value = offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Year : 0;
                        subParam[7] = new EacParameter("timeOut", DbType.Int32);
                        subParam[7].Value = offre.Risques[0].SortieGarantie != null ? offre.Risques[0].SortieGarantie.Value.Hour * 100 + offre.Risques[0].SortieGarantie.Value.Minute : 0;
                        subParam[8] = new EacParameter("valeur", DbType.Int64);
                        subParam[8].Value = offre.Risques[0].Valeur != null ? offre.Risques[0].Valeur : 0;
                        subParam[9] = new EacParameter("uniteCode", DbType.AnsiStringFixedLength);
                        subParam[9].Value = (offre.Risques[0].Unite != null && offre.Risques[0].Unite.Code != null) ? offre.Risques[0].Unite.Code : string.Empty;
                        subParam[10] = new EacParameter("typeCode", DbType.AnsiStringFixedLength);
                        subParam[10].Value = (offre.Risques[0].Type != null && offre.Risques[0].Type.Code != null) ? offre.Risques[0].Type.Code : string.Empty;
                        subParam[11] = new EacParameter("valeurHT", DbType.AnsiStringFixedLength);
                        subParam[11].Value = !string.IsNullOrEmpty(offre.Risques[0].ValeurHT) ? offre.Risques[0].ValeurHT : string.Empty;
                        subParam[12] = new EacParameter("partBenef", DbType.AnsiStringFixedLength);
                        subParam[12].Value = offre.Risques[0].PartBenef != null ? offre.Risques[0].PartBenef : string.Empty;
                        subParam[13] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                        subParam[13].Value = offre.CodeOffre.PadLeft(9, ' ');
                        subParam[14] = new EacParameter("version", DbType.Int32);
                        subParam[14].Value = offre.Version;
                        subParam[15] = new EacParameter("code", DbType.Int32);
                        subParam[15].Value = offre.Risques[0].Code;

                        sql = @"
UPDATE YPRTRSQ
SET JEVDJ = :dayIn, JEVDM = :monthIn, JEVDA = :yearIn, JEVDH = :timeIn,
JEVFJ = :dayOut, JEVFM = :monthOut, JEVFA = :yearOut, JEVFH = :timeOut,
JEVAL = :valeur, JEVAA = :valeur, JEVAU = :uniteCode, JEVAT = :typeCode, JEVAH = :valeurHT, JEPBN = :partBenef 
WHERE JEIPB = :codeoffre AND JEALX = :version AND JERSQ = :code";
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, subParam);
                    }
                    //Si mono risque mettre à jour le Champ PBRGT de YPOBASE à partir de JERGT de YPRTRSQ et mettre à jour le champ  JDCNA  de YPRTENT à partir de JECNA de YPRTRSQ                               
                    string sqlCount = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRTRSQ WHERE JEIPB = '{0}' AND JEALX = {1}",
                        offre.CodeOffre.PadLeft(9, ' '), offre.Version.HasValue ? offre.Version.Value : 0);
                    if (CommonRepository.RowNumber(sqlCount) == 1)
                    {
                        UpdateRegimeTaxeCatnatOffre(offre);
                    }
                }
                else
                {
                    throw new AlbFoncException("Erreur lors de l'enregistrement du risque", true, true);
                }

                #region Part Bénéficiaire



                //Détermination du nombre de risques ayant une part bénéficiaire ou BNS
                string sqlCountPBRsq = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRTRSQ 
                                                          WHERE JEIPB = '{0}'
                                                          AND JEALX = {1}
                                                          AND (JEPBN = 'O')", offre.CodeOffre.PadLeft(9, ' '), offre.Version.HasValue ? offre.Version.Value : 0);
                string sqlCountBNSRsq = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRTRSQ 
                                                          WHERE JEIPB ='{0}'
                                                          AND JEALX = {1}
                                                          AND (JEPBN = 'B')", offre.CodeOffre.PadLeft(9, ' '), offre.Version.HasValue ? offre.Version.Value : 0);
                string sqlCountBurnerRsq = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPRTRSQ 
                                                          WHERE JEIPB ='{0}'
                                                          AND JEALX = {1}
                                                          AND (JEPBN = 'U')", offre.CodeOffre.PadLeft(9, ' '), offre.Version.HasValue ? offre.Version.Value : 0);
                bool isRaz = false;

                string newPBBNS = !string.IsNullOrEmpty(offre.Risques[0].PartBenef) ?
                                            CommonRepository.RowNumber(sqlCountPBRsq) > 0 ? "O" :
                                            CommonRepository.RowNumber(sqlCountBNSRsq) > 0 ? "B" :
                                            CommonRepository.RowNumber(sqlCountBurnerRsq) > 0 ? "U" : "N" : "N";

                if (newPBBNS == "N")
                    isRaz = true;

                EacParameter[] paramDetails = new EacParameter[9];
                paramDetails[0] = new EacParameter("tauxAppel", DbType.AnsiStringFixedLength);
                paramDetails[0].Value = isRaz ? offre.Risques[0].TauxAppel.ToString() : offre.Risques[0].TauxAppel > 0 ? offre.Risques[0].TauxAppel.ToString() : "JDPBT";
                paramDetails[1] = new EacParameter("nbYear", DbType.Int32);
                paramDetails[1].Value = isRaz ? 0 : offre.Risques[0].NbYear;
                paramDetails[2] = new EacParameter("ristourne", DbType.Int32);
                paramDetails[2].Value = isRaz ? 0 : offre.Risques[0].Ristourne;
                paramDetails[3] = new EacParameter("cotisRetenue", DbType.Int32);
                paramDetails[3].Value = isRaz ? 0 : offre.Risques[0].CotisRetenue;
                paramDetails[4] = new EacParameter("seuil", DbType.Int32);
                paramDetails[4].Value = isRaz ? 0 : offre.Risques[0].Seuil;
                paramDetails[5] = new EacParameter("tauxComp", DbType.Int32);
                paramDetails[5].Value = isRaz ? 0 : offre.Risques[0].TauxComp;
                paramDetails[6] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramDetails[6].Value = offre.CodeOffre.PadLeft(9, ' ');
                paramDetails[7] = new EacParameter("version", DbType.Int32);
                paramDetails[7].Value = offre.Version;
                paramDetails[8] = new EacParameter("code", DbType.Int32);
                paramDetails[8].Value = offre.Risques[0].Code;


                sql = @"UPDATE YPRTRSQ SET JEPBT = :tauxAppel, JEPBA = :nbYear, JEPBR = :ristourne, JEPBP = :cotisRetenue, JEPBS = :seuil, JEPBC = :tauxComp
                                            WHERE JEIPB = :codeOffre AND JEALX = :version AND JERSQ = :code";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramDetails);

                // Fin BUG 2506

                //Gestion des valeurs du Burner
                paramDetails = new EacParameter[5];
                paramDetails[0] = new EacParameter("tauxmaxi", DbType.Single);
                paramDetails[0].Value = isRaz ? 0 : offre.Risques[0].TauxMaxi;
                paramDetails[1] = new EacParameter("primemaxi", DbType.Double);
                paramDetails[1].Value = isRaz ? 0 : offre.Risques[0].PrimeMaxi;
                paramDetails[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramDetails[2].Value = offre.CodeOffre.PadLeft(9, ' ');
                paramDetails[3] = new EacParameter("version", DbType.Int32);
                paramDetails[3].Value = offre.Version;
                paramDetails[4] = new EacParameter("code", DbType.Int32);
                paramDetails[4].Value = offre.Risques[0].Code;

                sql = @"UPDATE KPRSQ SET KABBRNT = :tauxmaxi, KABBRNC = :primemaxi
                            WHERE KABIPB = :codeOffre AND KABALX = :version AND KABRSQ = :code";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramDetails);

                //Gestion mono objet
                string sqlCountObjet = string.Format(@"SELECT COUNT(*) NBLIGN 
                                                       FROM YPRTOBJ 
                                                       WHERE JGIPB = '{0}' AND JGALX = {1} AND JGRSQ = {2}", offre.CodeOffre.PadLeft(9, ' '), offre.Version, offre.Risques[0].Code);
                //Ajout des infos à tous les objets s'ils existent
                if (CommonRepository.RowNumber(sqlCountObjet) > 0)
                {
                    EacParameter[] subParam = new EacParameter[9];
                    subParam[0] = new EacParameter("tauxAppel", DbType.Int32);
                    subParam[0].Value = offre.Risques[0].TauxAppel;
                    subParam[1] = new EacParameter("nbYear", DbType.Int32);
                    subParam[1].Value = offre.Risques[0].NbYear;
                    subParam[2] = new EacParameter("ristourne", DbType.Int32);
                    subParam[2].Value = offre.Risques[0].Ristourne;
                    subParam[3] = new EacParameter("cotisRetenue", DbType.Int32);
                    subParam[3].Value = offre.Risques[0].CotisRetenue;
                    subParam[4] = new EacParameter("seuil", DbType.Int32);
                    subParam[4].Value = offre.Risques[0].Seuil;
                    subParam[5] = new EacParameter("tauxComp", DbType.Int32);
                    subParam[5].Value = offre.Risques[0].TauxComp;
                    subParam[6] = new EacParameter("codeOffre", DbType.Int32);
                    subParam[6].Value = offre.CodeOffre.PadLeft(9, ' ');
                    subParam[7] = new EacParameter("version", DbType.Int64);
                    subParam[7].Value = offre.Version;
                    subParam[8] = new EacParameter("code", DbType.Int64);
                    subParam[8].Value = offre.Risques[0].Code;

                    string sqlUpdateMonoObjet = @"UPDATE YPRTOBJ SET JGPBT = :tauxAppel, JGPBA = :nbYear, JGPBR = :ristourne, JGPBP = :cotisRetenue, JGPBS = :seuil, JGPBC = :tauxComp
                                                        WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :code";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateMonoObjet, subParam);

                    //Gestion des valeurs du Burner
                    subParam = new EacParameter[5];
                    subParam[0] = new EacParameter("tauxmaxi", DbType.Single);
                    subParam[0].Value = isRaz ? 0 : offre.Risques[0].TauxMaxi;
                    subParam[1] = new EacParameter("primemaxi", DbType.Double);
                    subParam[1].Value = isRaz ? 0 : offre.Risques[0].PrimeMaxi;
                    subParam[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                    subParam[2].Value = offre.CodeOffre.PadLeft(9, ' ');
                    subParam[3] = new EacParameter("version", DbType.Int32);
                    subParam[3].Value = offre.Version;
                    subParam[4] = new EacParameter("code", DbType.Int32);
                    subParam[4].Value = offre.Risques[0].Code;

                    sql = @"UPDATE KPOBJ SET KACBRNT = :tauxmaxi, KACBRNC = :primemaxi
                            WHERE KACIPB = :codeOffre AND KACALX = :version AND KACRSQ = :code";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, subParam);
                }
                #endregion
            }
        }

        public static void UpdateRegimeTaxeCatnatOffre(OffreDto offre)
        {
            if (offre.Risques != null)
            {
                EacParameter[] param = new EacParameter[4];
                param[0] = new EacParameter("regimeTaxe", DbType.AnsiStringFixedLength);
                param[0].Value = offre.Risques[0].RegimeTaxe;
                param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[1].Value = offre.CodeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = offre.Version.HasValue ? offre.Version.Value : 0;
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = offre.Type;

                string sql = @"UPDATE YPOBASE SET PBRGT= :regimeTaxe WHERE PBIPB= :codeOffre AND PBALX= :version AND PBTYP= :type";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                EacParameter[] param2 = new EacParameter[3];
                param2[0] = new EacParameter("CATNAT", DbType.AnsiStringFixedLength);
                param2[0].Value = offre.Risques[0].CATNAT ? "O" : "N";
                param2[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param2[1].Value = offre.CodeOffre.PadLeft(9, ' ');
                param2[2] = new EacParameter("version", DbType.Int32);
                param2[2].Value = offre.Version.HasValue ? offre.Version.Value : 0;

                sql = @"UPDATE YPRTENT SET JDCNA= :CATNAT WHERE JDIPB= :codeOffre AND JDALX= :version";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param2);
            }
        }

        public static void SupprimerRisqueInventaire(string codeOffre, int? version, string type, int codeRisque, int codeObjet, string codeInventaire, string numDescription)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = version.HasValue ? version.Value : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
            param[3].Value = codeRisque;
            param[4] = new EacParameter("P_CODEOBJ", DbType.Int32);
            param[4].Value = codeObjet;
            param[5] = new EacParameter("P_CODEINVEN", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(codeInventaire) ? Convert.ToInt32(codeInventaire) : 0;
            param[6] = new EacParameter("P_NUMDESCR", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(numDescription) ? Convert.ToInt32(numDescription) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELOBJETINVEN", param);
        }

        #region SuppressionRisque

        public static void DeleteDetailsRisque(OffreDto offre)
        {
            if (offre != null && !string.IsNullOrEmpty(offre.CodeOffre) && offre.Risques != null && offre.Risques.Count > 0)
            {
                foreach (var risque in offre.Risques)
                {
                    EacParameter[] param = new EacParameter[4];
                    param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                    param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
                    param[1] = new EacParameter("P_VERSION", DbType.Int32);
                    param[1].Value = offre.Version.HasValue ? offre.Version.Value : 0;
                    param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                    param[2].Value = offre.Type;
                    param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
                    param[3].Value = risque.Code;
                    DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELRSQ", param);
                }
            }
        }

        #endregion

        #endregion

        #region Rechercher offres/contrats

        public static RechercheOffresGetResultDto RechercherOffresContratsTri(ModeleParametresRechercheDto paramRecherche, ModeConsultation modeNavig)
        {
            RechercheOffresGetResultDto result = new RechercheOffresGetResultDto
            {
                LstOffres = new List<OffreDto>()
            };

            if (paramRecherche != null)
            {
                int cpCode = 0;
                int numAlm = 0;
                string typeDateRecherche = string.Empty;
                switch (paramRecherche.TypeDateRecherche)
                {
                    case AlbConstantesMetiers.TypeDateRecherche.Saisie:
                        typeDateRecherche = "Saisie";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Effet:
                        typeDateRecherche = "Effet";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.MAJ:
                        typeDateRecherche = "MAJ";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Creation:
                        typeDateRecherche = "Creation";
                        break;
                }

                #region Paramètres

                EacParameter[] param = new EacParameter[40];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = !string.IsNullOrEmpty(paramRecherche.CodeOffre) ? paramRecherche.CodeOffre.PadLeft(9, ' ').Replace("'", "''") : string.Empty;
                param[1] = new EacParameter("P_TYPEOFFRE", DbType.AnsiStringFixedLength);
                param[1].Value = !string.IsNullOrEmpty(paramRecherche.Type) ? paramRecherche.Type.Replace("'", "''") : string.Empty;
                param[2] = new EacParameter("P_ALIMENT", DbType.AnsiStringFixedLength);
                param[2].Value = !string.IsNullOrEmpty(paramRecherche.NumAliment) && (Int32.TryParse(paramRecherche.NumAliment, out numAlm)) ? paramRecherche.NumAliment : string.Empty;
                param[3] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
                param[3].Value = !string.IsNullOrEmpty(paramRecherche.Branche) ? paramRecherche.Branche.Replace("'", "''") : string.Empty;
                param[4] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
                param[4].Value = !string.IsNullOrEmpty(paramRecherche.Cible) ? paramRecherche.Cible.Replace("'", "''") : string.Empty;
                param[5] = new EacParameter("P_CABINETCOURTAGE_ID", DbType.Int32);
                param[5].Value = paramRecherche.CabinetCourtageId;
                param[6] = new EacParameter("P_CABINETCOURTAGE_NOM", DbType.AnsiStringFixedLength);
                param[6].Value = !string.IsNullOrEmpty(paramRecherche.CabinetCourtageNom) ? paramRecherche.CabinetCourtageNom.Replace("'", "''") : string.Empty;
                param[7] = new EacParameter("P_CABINETCOURTAGE_ISAPPORTEUR", DbType.Int32);
                param[7].Value = paramRecherche.CabinetCourtageIsApporteur ? 1 : 0;
                param[8] = new EacParameter("P_CABINETCOURTAGE_ISGESTIONNAIRE", DbType.Int32);
                param[8].Value = paramRecherche.CabinetCourtageIsGestionnaire ? 1 : 0;
                param[9] = new EacParameter("P_PRENEURASSURANCE_CODE", DbType.Int32);
                param[9].Value = paramRecherche.PreneurAssuranceCode;
                param[10] = new EacParameter("P_PRENEURASSURANCE_SIREN", DbType.Int32);
                param[10].Value = paramRecherche.PreneurAssuranceSIREN;
                param[11] = new EacParameter("P_PRENEURASSURANCE_NOM", DbType.AnsiStringFixedLength);
                param[11].Value = !string.IsNullOrEmpty(paramRecherche.PreneurAssuranceNom) ? paramRecherche.PreneurAssuranceNom.Replace("'", "''") : string.Empty;
                param[12] = new EacParameter("P_PRENEURASSURANCE_CP", DbType.AnsiStringFixedLength);
                param[12].Value = (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5) && Int32.TryParse(paramRecherche.PreneurAssuranceCP.Substring(2, 3), out cpCode)) ? paramRecherche.PreneurAssuranceCP.Substring(2, 3) : string.Empty;
                param[13] = new EacParameter("P_PRENEURASSURANCE_DEP", DbType.AnsiStringFixedLength);
                param[13].Value = (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5) && Int32.TryParse(paramRecherche.PreneurAssuranceCP.Substring(2, 3), out cpCode)) ? paramRecherche.PreneurAssuranceCP.Substring(0, 2) : string.Empty;
                param[14] = new EacParameter("P_PRENEURASSURANCE_DEPONLY", DbType.AnsiStringFixedLength);
                param[14].Value = !string.IsNullOrEmpty(paramRecherche.PreneurAssuranceDEP) ? paramRecherche.PreneurAssuranceDEP : (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && Int32.TryParse(paramRecherche.PreneurAssuranceCP, out cpCode)) ? paramRecherche.PreneurAssuranceCP : string.Empty;
                param[15] = new EacParameter("P_PRENEURASSURANCE_VILLE", DbType.AnsiStringFixedLength);
                param[15].Value = !string.IsNullOrEmpty(paramRecherche.PreneurAssuranceVille) ? paramRecherche.PreneurAssuranceVille.Replace("'", "''") : string.Empty;
                param[16] = new EacParameter("P_ADRESSEOFFRE_CP", DbType.AnsiStringFixedLength);
                param[16].Value = (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5) && Int32.TryParse(paramRecherche.AdresseRisqueCP, out cpCode)) ? paramRecherche.AdresseRisqueCP.Substring(2, 3) : string.Empty;
                param[17] = new EacParameter("P_ADRESSEOFFRE_DEP", DbType.AnsiStringFixedLength);
                param[17].Value = (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5) && Int32.TryParse(paramRecherche.AdresseRisqueCP, out cpCode)) ? paramRecherche.AdresseRisqueCP.Substring(0, 2) : (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && Int32.TryParse(paramRecherche.AdresseRisqueCP, out cpCode)) ? paramRecherche.AdresseRisqueCP : string.Empty;
                param[18] = new EacParameter("P_ADRESSEOFFRE_VILLE", DbType.AnsiStringFixedLength);
                param[18].Value = !string.IsNullOrEmpty(paramRecherche.AdresseRisqueVille) ? paramRecherche.AdresseRisqueVille.Replace("'", "''") : string.Empty;
                param[19] = new EacParameter("P_SOUSCRIPTEUR_CODE", DbType.AnsiStringFixedLength);
                param[19].Value = !string.IsNullOrEmpty(paramRecherche.SouscripteurCode) ? paramRecherche.SouscripteurCode.Replace("'", "''") : string.Empty;
                param[20] = new EacParameter("P_SOUSCRIPTEUR_NOM", DbType.AnsiStringFixedLength);
                param[20].Value = !string.IsNullOrEmpty(paramRecherche.SouscripteurNom) ? paramRecherche.SouscripteurNom.Replace("'", "''") : string.Empty;
                param[21] = new EacParameter("P_GESTIONNAIRE_CODE", DbType.Int32);
                param[21].Value = paramRecherche.GestionnaireCode;
                param[22] = new EacParameter("P_GESTIONNAIRE_NOM", DbType.AnsiStringFixedLength);
                param[22].Value = !string.IsNullOrEmpty(paramRecherche.GestionnaireNom) ? paramRecherche.GestionnaireNom.Trim().Replace("'", "''") : string.Empty;
                param[23] = new EacParameter("P_ETAT", DbType.AnsiStringFixedLength);
                param[23].Value = !string.IsNullOrEmpty(paramRecherche.Etat) ? paramRecherche.Etat.Replace("'", "''") : string.Empty;
                param[24] = new EacParameter("P_ETAT_SAUF", DbType.Int32);
                param[24].Value = paramRecherche.SaufEtat ? 1 : 0;
                param[25] = new EacParameter("P_SITUATION", DbType.AnsiStringFixedLength);
                param[25].Value = !string.IsNullOrEmpty(paramRecherche.Situation) ? paramRecherche.Situation.Replace("'", "''") : string.Empty;
                param[26] = new EacParameter("P_SITUATION_ISACTIF", DbType.Int32);
                param[26].Value = paramRecherche.IsActif ? 1 : 0;
                param[27] = new EacParameter("P_SITUATION_ISINACTIF", DbType.Int32);
                param[27].Value = paramRecherche.IsInactif ? 1 : 0;
                param[28] = new EacParameter("P_TYPE_DATE_RECHERCHE", DbType.AnsiStringFixedLength);
                param[28].Value = !string.IsNullOrEmpty(typeDateRecherche) ? typeDateRecherche.Replace("'", "''") : string.Empty;
                param[29] = new EacParameter("P_DATEDEBUT", DbType.Int32);
                param[29].Value = AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut).HasValue ? AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut).Value : 0;
                param[30] = new EacParameter("P_DATEFIN", DbType.Int32);
                param[30].Value = AlbConvert.ConvertDateToInt(paramRecherche.DDateFin).HasValue ? AlbConvert.ConvertDateToInt(paramRecherche.DDateFin).Value : 0;
                param[31] = new EacParameter("P_MOTS_CLES_LOWER", DbType.AnsiStringFixedLength);
                param[31].Value = !string.IsNullOrEmpty(paramRecherche.MotsClefs) ? paramRecherche.MotsClefs.ToLowerInvariant().Replace("'", "''") : string.Empty;
                param[32] = new EacParameter("P_MOTS_CLES_UPPER", DbType.AnsiStringFixedLength);
                param[32].Value = !string.IsNullOrEmpty(paramRecherche.MotsClefs) ? paramRecherche.MotsClefs.ToUpperInvariant().Replace("'", "''") : string.Empty;
                param[33] = new EacParameter("P_SORTINGBY", DbType.AnsiStringFixedLength);
                param[33].Value = GetRechercheOrderByClause(paramRecherche.SortingName, paramRecherche.SortingOrder);
                param[34] = new EacParameter("P_ISTEMPLATE", DbType.Int32);
                param[34].Value = paramRecherche.IsTemplate ? 1 : 0;
                param[35] = new EacParameter("P_LINECOUNT", DbType.Int32);
                param[35].Value = paramRecherche.LineCount;
                param[36] = new EacParameter("P_STARTLINE", DbType.Int32);
                param[36].Value = paramRecherche.StartLine;
                param[37] = new EacParameter("P_ENDLINE", DbType.Int32);
                param[37].Value = paramRecherche.EndLine;
                param[38] = new EacParameter("P_TYPECONTRAT", DbType.AnsiStringFixedLength);
                param[38].Value = paramRecherche.TypeContrat.Replace("'", "''");
                param[39] = new EacParameter("P_COUNT", DbType.Int64);
                param[39].Direction = ParameterDirection.InputOutput;
                param[39].DbType = DbType.Int64;
                param[39].Value = 0;

                #region pour debug de la requete

                //param[40] = new EacParameter("P_REQUEST_OUT", "");
                //param[40].Size = 8000;
                //param[40].Direction = ParameterDirection.Output;

                #endregion

                #endregion

                DataTable dataTable = new DataTable();
                var resultQuery = new List<OffreRechPlatDto>();
                if (modeNavig == ModeConsultation.Historique)
                {
                    resultQuery = DbBase.Settings.ExecuteList<OffreRechPlatDto>(CommandType.StoredProcedure, "SP_RECHERCHEGENERALEHIST", param);
                    //dataTable = DbBase.Settings.ExecuteDataTable(CommandType.StoredProcedure, "SP_RECHERCHEGENERALEHIST", param);
                    //foreach (DataRow ligne in dataTable.Rows)
                    //{
                    //    OffreDto offre = Initialiser(ligne, true);
                    //    result.LstOffres.Add(offre);
                    //}
                }
                else
                {
                    resultQuery = DbBase.Settings.ExecuteList<OffreRechPlatDto>(CommandType.StoredProcedure, "SP_RECHERCHEGENERALE", param);
                    //dataTable = DbBase.Settings.ExecuteDataTable(CommandType.StoredProcedure, "SP_RECHERCHEGENERALE", param);
                }

                if (resultQuery != null && resultQuery.Any())
                {
                    resultQuery.ForEach(el =>
                    {
                        int cpAssur = 0;
                        int cpCourt = 0;

                        int.TryParse(el.CpAss, out cpAssur);
                        int.TryParse(el.CpCourt, out cpCourt);


                        result.LstOffres.Add(new OffreDto
                        {
                            CodeOffre = el.CodeOffre,
                            Version = el.Version,
                            Type = el.Type,
                            NumAvenant = el.CodeAvn,
                            DateSaisie = AlbConvert.ConvertIntToDateHour(el.DateSaisie),
                            Branche = new BrancheDto { Code = el.CodeBranche, Nom = el.LibBranche, Cible = new CibleDto { Code = el.CodeCible, Nom = el.LibCible } },
                            Etat = el.CodeEtat,
                            EtatLib = el.LibEtat,
                            Situation = el.CodeSit,
                            SituationLib = el.LibSit,
                            Qualite = el.CodeQualite,
                            QualiteLib = el.LibQualite,
                            Descriptif = el.Descriptif,
                            PreneurAssurance = new AssureDto
                            {
                                Code = el.CodeAss.ToString(),
                                NomAssure = el.NomAss,
                                Adresse = new AdressePlatDto
                                {
                                    // CodePostal = cpAssur,
                                    NomVille = el.VilleAss,
                                    CodePostalString = el.CpAss
                                }
                            },
                            CabinetGestionnaire = new CabinetCourtageDto
                            {
                                Code = el.CodeCourt,
                                NomCabinet = el.NomCourt,
                                Adresse = new AdressePlatDto
                                {
                                    //CodePostal = cpCourt,
                                    NomVille = el.VilleCourt,
                                    CodePostalString = el.CpCourt
                                }
                            },
                            TypeAvt = el.TypeTraitement,
                            TypeAccord = el.TypeAccord,
                            KheopsStatut = el.StatutKheops,
                            NumAvnExterne = el.AvnExt,
                            GenerDoc = Convert.ToInt32(el.GenerDoc),
                            MotifRefus = el.MotifRefus.Trim() != "-" ? el.MotifRefus : string.Empty,
                            Periodicite = new ParametreDto { Code = el.CodePeriodicite },
                            ContratMere = el.TypePolice,
                            DateFinEffetGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateFinEffet)),
                            DateEffetGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateEffet)),
                            DateCreation = AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateCreation)),
                            HeureFin = AlbConvert.ConvertIntToTimeMinute(el.HeureFinEffet),
                            HasSusp = el.HasSusp > 0 ? true : false
                        });
                    });
                }


                result.NbCount = Convert.ToInt32(param[39].Value.ToString());
            }
            return result;
        }

        public static Int64 RechercherOffresContratsCount(ModeleParametresRechercheDto paramRecherche)
        {
            if (paramRecherche != null)
            {
                string sql = @"SELECT COUNT (*) NBLIGN 
                    FROM YPOBASE B1 
                        LEFT JOIN YASSNOM ON PBIAS = ANIAS AND ANINL = 0 AND ANTNM = 'A' 
                        LEFT JOIN YCOURTN ON PBICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A' 
                        LEFT JOIN YCOURTI ON TCICT = TNICT 
                        LEFT JOIN YASSURE ON PBIAS = ASIAS 
                        LEFT JOIN YADRESS court ON TCADH = court.ABPCHR 
                        LEFT JOIN YADRESS assu ON ASADH = assu.ABPCHR 
                        LEFT JOIN YADRESS offre ON PBADH = offre.ABPCHR 
                        LEFT JOIN KPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP 
                        LEFT JOIN KPOBSV ON KAJCHR = KAAOBSV 
                        LEFT JOIN YYYYPAR ON TCON = 'GENER' AND TFAM = 'BRCHE' AND TCOD = PBBRA AND TPCN2 = 1";

                string andWhere = " WHERE ";

                if (!String.IsNullOrEmpty(paramRecherche.CodeOffre))
                {
                    sql += andWhere;
                    sql += string.Format("PBIPB LIKE '%{0}%'", paramRecherche.CodeOffre.PadLeft(9, ' ').Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.NumAliment))
                {
                    sql += andWhere;
                    sql += string.Format("PBALX = '{0}'", paramRecherche.NumAliment.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.Type))
                {
                    sql += andWhere;
                    sql += string.Format("PBTYP = '{0}'", paramRecherche.Type.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.Branche))
                {
                    sql += andWhere;
                    sql += string.Format("PBBRA = '{0}'", paramRecherche.Branche.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.Cible))
                {
                    sql += andWhere;
                    sql += string.Format("KAACIBLE = '{0}'", paramRecherche.Cible.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.GestionnaireNom))
                {
                    sql += andWhere;
                    sql += string.Format("PBGES LIKE '{0}%'", paramRecherche.GestionnaireNom.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (paramRecherche.CabinetCourtageId > 0 && paramRecherche.CabinetCourtageIsGestionnaire && !paramRecherche.CabinetCourtageIsApporteur)
                {
                    sql += andWhere;
                    sql += string.Format("PBICT = {0}", paramRecherche.CabinetCourtageId);
                    andWhere = " AND ";
                }

                if (paramRecherche.CabinetCourtageId > 0 && paramRecherche.CabinetCourtageIsApporteur && !paramRecherche.CabinetCourtageIsGestionnaire)
                {
                    sql += andWhere;
                    sql += string.Format("PBCTA = {0}", paramRecherche.CabinetCourtageId);
                    andWhere = " AND ";
                }

                if (paramRecherche.CabinetCourtageId > 0 && paramRecherche.CabinetCourtageIsApporteur && paramRecherche.CabinetCourtageIsGestionnaire)
                {
                    sql += andWhere;
                    sql += string.Format("(PBCTA = {0} OR PBICT = {0})", paramRecherche.CabinetCourtageId);
                    andWhere = " AND ";
                }

                if (paramRecherche.PreneurAssuranceCode > 0)
                {
                    sql += andWhere;
                    sql += string.Format("PBIAS = {0}", paramRecherche.PreneurAssuranceCode);
                    andWhere = " AND ";
                }

                if (paramRecherche.PreneurAssuranceSIREN > 0)
                {
                    sql += andWhere;
                    sql += string.Format("ASSIR = {0}", paramRecherche.PreneurAssuranceSIREN);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceNom))
                {
                    sql += andWhere;
                    sql += string.Format("ANNOM like '{0}%'", paramRecherche.PreneurAssuranceNom.Replace("'", "''"));
                    andWhere = " AND ";
                }

                int cp = 0;
                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5) && (int.TryParse(paramRecherche.PreneurAssuranceCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("assu.ABPCP6 = {0} AND assu.ABPDP6 = '{1}'", paramRecherche.PreneurAssuranceCP.Substring(2, 3), paramRecherche.PreneurAssuranceCP.Substring(0, 2));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceDEP))
                {
                    sql += andWhere;
                    sql += string.Format(" assu.ABPDP6 = '{0}'", paramRecherche.PreneurAssuranceDEP);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length != 5) && (int.TryParse(paramRecherche.PreneurAssuranceCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("assu.ABPDP6 = '{0}'", paramRecherche.PreneurAssuranceCP);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceVille))
                {
                    sql += andWhere;
                    sql += string.Format("assu.ABPVI6 like '{0}%'", paramRecherche.PreneurAssuranceVille.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5) && (int.TryParse(paramRecherche.AdresseRisqueCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("offre.ABPCP6 = {0} AND offre.ABPDP6 = '{1}'", paramRecherche.AdresseRisqueCP.Substring(2, 3), paramRecherche.AdresseRisqueCP.Substring(0, 2));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length != 5) && (int.TryParse(paramRecherche.AdresseRisqueCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("offre.ABPDP6 = '{0}'", paramRecherche.AdresseRisqueCP);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueVille))
                {
                    sql += andWhere;
                    sql += string.Format("offre.ABPVI6 like '{0}%'", paramRecherche.AdresseRisqueVille.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.SouscripteurCode))
                {
                    sql += andWhere;
                    sql += string.Format("PBSOU = '{0}'", paramRecherche.SouscripteurCode.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.SouscripteurNom))
                {
                    sql += andWhere;
                    sql += string.Format("PBGES = '{0}'", paramRecherche.SouscripteurNom.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.Etat))
                {
                    if (paramRecherche.SaufEtat)
                    {
                        sql += andWhere;
                        sql += string.Format("PBETA <> '{0}'", paramRecherche.Etat.Replace("'", "''"));
                        andWhere = " AND ";
                    }
                    else
                    {
                        sql += andWhere;
                        sql += string.Format("PBETA = '{0}'", paramRecherche.Etat.Replace("'", "''"));
                        andWhere = " AND ";
                    }
                }

                if (!String.IsNullOrEmpty(paramRecherche.Situation))
                {
                    sql += andWhere;
                    sql += string.Format("PBSIT = '{0}'", paramRecherche.Situation.Replace("'", "''"));
                    andWhere = " AND ";
                }
                else if (paramRecherche.IsActif && !paramRecherche.IsInactif)
                {
                    sql += andWhere;
                    sql += " PBSIT <> 'X' AND PBSIT <> 'W' AND PBSIT <> 'N' ";
                    andWhere = " AND ";
                }
                else if (paramRecherche.IsInactif && !paramRecherche.IsActif)
                {
                    sql += andWhere;
                    sql += " (PBSIT = 'X' OR PBSIT = 'W' OR PBSIT = 'N') ";
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.TypeContrat))
                {
                    if (paramRecherche.TypeContrat == "M")
                    {
                        sql += andWhere;
                        sql += string.Format("PBMER = '{0}'", paramRecherche.TypeContrat.Replace("'", "''"));
                        andWhere = " AND ";
                    }
                    else if (paramRecherche.TypeContrat == "R")
                    {
                        sql += andWhere;
                        sql += "PBETA = 'V' AND PBSIT = 'X'";
                        andWhere = " AND ";
                    }
                }

                if (paramRecherche.DDateDebut.HasValue)
                {
                    switch (paramRecherche.TypeDateRecherche)
                    {
                        case AlbConstantesMetiers.TypeDateRecherche.Saisie:
                            sql += andWhere;
                            sql += string.Format("(PBSAA * 10000 + PBSAM * 100 + PBSAJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Effet:
                            sql += andWhere;
                            sql += string.Format("(PBEFA * 10000 + PBEFM * 100 + PBEFJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.MAJ:
                            sql += andWhere;
                            sql += string.Format("(PBMJA * 10000 + PBMJM * 100 + PBMJJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";
                            break;

                        case AlbConstantesMetiers.TypeDateRecherche.Creation:
                            sql += andWhere;
                            sql += string.Format("(PBCRA * 10000 + PBCRM * 100 + PBCRJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";
                            break;
                    }
                }
                if (!paramRecherche.DDateDebut.HasValue && paramRecherche.DDateFin.HasValue)
                {
                    switch (paramRecherche.TypeDateRecherche)
                    {
                        case AlbConstantesMetiers.TypeDateRecherche.Saisie:

                            sql += andWhere;
                            sql += string.Format("(PBSAA * 10000 + PBSAM * 100 + PBSAJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Effet:

                            sql += andWhere;
                            sql += string.Format("(PBEFA * 10000 + PBEFM * 100 + PBEFJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.MAJ:

                            sql += andWhere;
                            sql += string.Format("(PBMJA * 10000 + PBMJM * 100 + PBMJJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Creation:
                            sql += andWhere;
                            sql += string.Format("(PBCRA * 10000 + PBCRM * 100 + PBCRJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                    }
                }
                if (paramRecherche.DDateDebut.HasValue && paramRecherche.DDateFin.HasValue)
                {
                    switch (paramRecherche.TypeDateRecherche)
                    {
                        case AlbConstantesMetiers.TypeDateRecherche.Saisie:

                            sql += andWhere;
                            sql += string.Format("(PBSAA * 10000 + PBSAM * 100 + PBSAJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Effet:

                            sql += andWhere;
                            sql += string.Format("(PBEFA * 10000 + PBEFM * 100 + PBEFJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.MAJ:

                            sql += andWhere;
                            sql += string.Format("(PBMJA * 10000 + PBMJM * 100 + PBMJJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Creation:
                            sql += andWhere;
                            sql += string.Format("(PBCRA * 10000 + PBCRM * 100 + PBCRJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                    }
                }

                if (!String.IsNullOrEmpty(paramRecherche.MotsClefs))
                {
                    string rechercheLibreLow = paramRecherche.MotsClefs.ToLowerInvariant().Trim();
                    string rechercheLibreUp = paramRecherche.MotsClefs.ToUpperInvariant().Trim();

                    sql += andWhere;
                    sql += string.Format(@"(LOWER(PBMO1) LIKE '%{0}%' OR UPPER(PBMO1) LIKE '%{1}%' OR 
                                        LOWER(PBMO2) LIKE '%{0}%' OR UPPER(PBMO2) LIKE '%{1}%' OR
                                        LOWER(PBMO3) LIKE '%{0}%' OR UPPER(PBMO3) LIKE '%{1}%' OR
                                        LOWER(PBREF) LIKE '%{0}%' OR UPPER(PBREF) LIKE '%{1}%' OR
                                        LOWER(KAJOBSV) LIKE '%{0}%' OR UPPER(KAJOBSV) LIKE '%{1}%')", rechercheLibreLow.Replace("'", "''"), rechercheLibreUp.Replace("'", "''"));
                    andWhere = " AND ";
                }

                if (andWhere == " WHERE ")
                {
                    sql += andWhere;
                    sql += " 1 = 0 ";
                    andWhere = " AND ";
                }
                if (paramRecherche.IsTemplate)
                {
                    sql += " AND EXISTS ( SELECT * FROM KCANEV WHERE KGOCNVA = PBIPB) AND PBIPB LIKE 'CV%'";
                    sql += " AND NOT EXISTS ( SELECT * FROM KVERROU WHERE KAVIPB = PBIPB AND KAVALX = PBALX AND KAVTYP = PBTYP)";
                }
                else
                {
                    sql += " AND NOT EXISTS ( SELECT * FROM KCANEV WHERE KGOCNVA = PBIPB) AND PBIPB NOT LIKE 'CV%'";
                }



                return CommonRepository.RowNumber(sql);
            }
            else
            {
                return 0;
            }
        }

        public static Int64 RechercherOffresContratsHistCount(ModeleParametresRechercheDto paramRecherche)
        {
            if (paramRecherche != null)
            {
                string sql = @"SELECT COUNT (*) NBLIGN 
                                FROM YHPBASE B1 
                                    LEFT JOIN YASSNOM ON PBIAS = ANIAS AND ANINL = 0 AND ANTNM = 'A' 
                                    LEFT JOIN YCOURTN ON PBICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A' 
                                    LEFT JOIN YCOURTI ON TCICT = TNICT 
                                    LEFT JOIN YASSURE ON PBIAS = ASIAS 
                                    LEFT JOIN YADRESS court ON TCADH = court.ABPCHR 
                                    LEFT JOIN YADRESS assu ON ASADH = assu.ABPCHR 
                                    LEFT JOIN YADRESS offre ON PBADH = offre.ABPCHR 
                                    LEFT JOIN HPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP AND KAAAVN = PBAVN 
                                    LEFT JOIN HPOBSV ON KAJCHR = KAAOBSV 
                                    LEFT JOIN YYYYPAR ON TCON = 'GENER' AND TFAM = 'BRCHE' AND TCOD = PBBRA AND TPCN2 = 1";

                string andWhere = " WHERE ";

                #region WHERE
                if (!String.IsNullOrEmpty(paramRecherche.CodeOffre))
                {
                    sql += andWhere;
                    sql += string.Format("PBIPB LIKE '%{0}%'", paramRecherche.CodeOffre.PadLeft(9, ' '));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.NumAliment))
                {
                    sql += andWhere;
                    sql += string.Format("PBALX = '{0}'", paramRecherche.NumAliment);
                    andWhere = " AND ";
                }

                /*if (!String.IsNullOrEmpty(paramRecherche.Type))
                {
                    sql += andWhere;
                    sql += string.Format("PBTYP = '{0}'", paramRecherche.Type);
                    andWhere = " AND ";
                }*/

                if (!String.IsNullOrEmpty(paramRecherche.Branche))
                {
                    sql += andWhere;
                    sql += string.Format("PBBRA = '{0}'", paramRecherche.Branche);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.Cible))
                {
                    sql += andWhere;
                    sql += string.Format("KAACIBLE = '{0}'", paramRecherche.Cible);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.GestionnaireNom))
                {
                    sql += andWhere;
                    sql += string.Format("PBGES LIKE '{0}%'", paramRecherche.GestionnaireNom);
                    andWhere = " AND ";
                }

                if (paramRecherche.CabinetCourtageId > 0 && paramRecherche.CabinetCourtageIsGestionnaire && !paramRecherche.CabinetCourtageIsApporteur)
                {
                    sql += andWhere;
                    sql += string.Format("PBICT = {0}", paramRecherche.CabinetCourtageId);
                    andWhere = " AND ";
                }

                if (paramRecherche.CabinetCourtageId > 0 && paramRecherche.CabinetCourtageIsApporteur && !paramRecherche.CabinetCourtageIsGestionnaire)
                {
                    sql += andWhere;
                    sql += string.Format("PBCTA = {0}", paramRecherche.CabinetCourtageId);
                    andWhere = " AND ";
                }

                if (paramRecherche.CabinetCourtageId > 0 && paramRecherche.CabinetCourtageIsApporteur && paramRecherche.CabinetCourtageIsGestionnaire)
                {
                    sql += andWhere;
                    sql += string.Format("(PBCTA = {0} OR PBICT = {0})", paramRecherche.CabinetCourtageId);
                    andWhere = " AND ";
                }

                if (paramRecherche.PreneurAssuranceCode > 0)
                {
                    sql += andWhere;
                    sql += string.Format("PBIAS = {0}", paramRecherche.PreneurAssuranceCode);
                    andWhere = " AND ";
                }

                if (paramRecherche.PreneurAssuranceSIREN > 0)
                {
                    sql += andWhere;
                    sql += string.Format("ASSIR = {0}", paramRecherche.PreneurAssuranceSIREN);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceNom))
                {
                    sql += andWhere;
                    sql += string.Format("ANNOM like '{0}%'", paramRecherche.PreneurAssuranceNom);
                    andWhere = " AND ";
                }

                int cp = 0;
                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5) && (int.TryParse(paramRecherche.PreneurAssuranceCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("assu.ABPCP6 = {0} AND assu.ABPDP6 = '{1}'", paramRecherche.PreneurAssuranceCP.Substring(2, 3), paramRecherche.PreneurAssuranceCP.Substring(0, 2));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceDEP))
                {
                    sql += andWhere;
                    sql += string.Format(" assu.ABPDP6 = '{0}'", paramRecherche.PreneurAssuranceDEP);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length != 5) && (int.TryParse(paramRecherche.PreneurAssuranceCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("assu.ABPDP6 = '{0}'", paramRecherche.PreneurAssuranceCP);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceVille))
                {
                    sql += andWhere;
                    sql += string.Format("assu.ABPVI6 like '{0}%'", paramRecherche.PreneurAssuranceVille);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5) && (int.TryParse(paramRecherche.AdresseRisqueCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("offre.ABPCP6 = {0} AND offre.ABPDP6 = '{1}'", paramRecherche.AdresseRisqueCP.Substring(2, 3), paramRecherche.AdresseRisqueCP.Substring(0, 2));
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length != 5) && (int.TryParse(paramRecherche.AdresseRisqueCP, out cp)))
                {
                    sql += andWhere;
                    sql += string.Format("offre.ABPDP6 = '{0}'", paramRecherche.AdresseRisqueCP);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueVille))
                {
                    sql += andWhere;
                    sql += string.Format("offre.ABPVI6 like '{0}%'", paramRecherche.AdresseRisqueVille);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.SouscripteurCode))
                {
                    sql += andWhere;
                    sql += string.Format("PBSOU = '{0}'", paramRecherche.SouscripteurCode);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.SouscripteurNom))
                {
                    sql += andWhere;
                    sql += string.Format("PBGES = '{0}'", paramRecherche.SouscripteurNom);
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.Etat))
                {
                    if (paramRecherche.SaufEtat)
                    {
                        sql += andWhere;
                        sql += string.Format("PBETA <> '{0}'", paramRecherche.Etat);
                        andWhere = " AND ";
                    }
                    else
                    {
                        sql += andWhere;
                        sql += string.Format("PBETA = '{0}'", paramRecherche.Etat);
                        andWhere = " AND ";
                    }
                }

                if (!String.IsNullOrEmpty(paramRecherche.Situation))
                {
                    sql += andWhere;
                    sql += string.Format("PBSIT = '{0}'", paramRecherche.Situation);
                    andWhere = " AND ";
                }
                else if (paramRecherche.IsActif && !paramRecherche.IsInactif)
                {
                    sql += andWhere;
                    sql += " PBSIT <> 'X' AND PBSIT <> 'W' AND PBSIT <> 'N' ";
                    andWhere = " AND ";
                }
                else if (paramRecherche.IsInactif && !paramRecherche.IsActif)
                {
                    sql += andWhere;
                    sql += " (PBSIT = 'X' OR PBSIT = 'W' OR PBSIT = 'N') ";
                    andWhere = " AND ";
                }

                if (!String.IsNullOrEmpty(paramRecherche.TypeContrat))
                {
                    sql += andWhere;
                    sql += string.Format("PBMER = '{0}'", paramRecherche.TypeContrat);
                    andWhere = " AND ";
                }

                if (paramRecherche.DDateDebut.HasValue)
                {
                    switch (paramRecherche.TypeDateRecherche)
                    {
                        case AlbConstantesMetiers.TypeDateRecherche.Saisie:
                            sql += andWhere;
                            sql += string.Format("(PBSAA * 10000 + PBSAM * 100 + PBSAJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";


                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Effet:
                            sql += andWhere;
                            sql += string.Format("(PBEFA * 10000 + PBEFM * 100 + PBEFJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";


                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.MAJ:
                            sql += andWhere;
                            sql += string.Format("(PBMJA * 10000 + PBMJM * 100 + PBMJJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";
                            break;

                        case AlbConstantesMetiers.TypeDateRecherche.Creation:
                            sql += andWhere;
                            sql += string.Format("(PBCRA * 10000 + PBCRM * 100 + PBCRJ) >= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut));
                            andWhere = " AND ";
                            break;
                    }
                }
                if (!paramRecherche.DDateDebut.HasValue && paramRecherche.DDateFin.HasValue)
                {
                    switch (paramRecherche.TypeDateRecherche)
                    {
                        case AlbConstantesMetiers.TypeDateRecherche.Saisie:

                            sql += andWhere;
                            sql += string.Format("(PBSAA * 10000 + PBSAM * 100 + PBSAJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Effet:

                            sql += andWhere;
                            sql += string.Format("(PBEFA * 10000 + PBEFM * 100 + PBEFJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.MAJ:

                            sql += andWhere;
                            sql += string.Format("(PBMJA * 10000 + PBMJM * 100 + PBMJJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;

                        case AlbConstantesMetiers.TypeDateRecherche.Creation:
                            sql += andWhere;
                            sql += string.Format("(PBCRA * 10000 + PBCRM * 100 + PBCRJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                    }
                }
                if (paramRecherche.DDateDebut.HasValue && paramRecherche.DDateFin.HasValue)
                {
                    switch (paramRecherche.TypeDateRecherche)
                    {
                        case AlbConstantesMetiers.TypeDateRecherche.Saisie:

                            sql += andWhere;
                            sql += string.Format("(PBSAA * 10000 + PBSAM * 100 + PBSAJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Effet:

                            sql += andWhere;
                            sql += string.Format("(PBEFA * 10000 + PBEFM * 100 + PBEFJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.MAJ:

                            sql += andWhere;
                            sql += string.Format("(PBMJA * 10000 + PBMJM * 100 + PBMJJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                        case AlbConstantesMetiers.TypeDateRecherche.Creation:
                            sql += andWhere;
                            sql += string.Format("(PBCRA * 10000 + PBCRM * 100 + PBCRJ) <= {0}", AlbConvert.ConvertDateToInt(paramRecherche.DDateFin));
                            andWhere = " AND ";
                            break;
                    }
                }

                if (!String.IsNullOrEmpty(paramRecherche.MotsClefs))
                {
                    string rechercheLibreLow = paramRecherche.MotsClefs.ToLowerInvariant();
                    string rechercheLibreUp = paramRecherche.MotsClefs.ToUpperInvariant();

                    sql += andWhere;
                    sql += string.Format(@"(PBMO1 LIKE '%{0}%' OR PBMO1 LIKE '%{1}%' OR 
                                        PBMO2 LIKE '%{0}%' OR PBMO2 LIKE '%{1}%' OR
                                        PBMO3 LIKE '%{0}%' OR PBMO3 LIKE '%{1}%' OR
                                        PBREF LIKE '%{0}%' OR PBREF LIKE '%{1}%' OR
                                        KAJOBSV LIKE '%{0}%' OR KAJOBSV LIKE '%{1}%')", rechercheLibreLow, rechercheLibreUp);
                    andWhere = " AND ";
                }

                if (andWhere == " WHERE ")
                {
                    sql += andWhere;
                    sql += " 1 = 0 ";
                    andWhere = " AND ";
                }
                #endregion

                sql += " AND NOT EXISTS ( SELECT * FROM KCANEV WHERE KGOCNVA = PBIPB) AND PBIPB NOT LIKE 'CV%'";

                return CommonRepository.RowNumber(sql);
            }
            else
            {
                return 0;
            }
        }

        #endregion
        public static bool ValiderCatnat(OffreDto offre, string codeAvn, ModeConsultation modeNavig)
        {
            if (!offre.Risques[0].CATNAT)
            {
                return true;
            }
            var lstRisques = ObtenirRisques(modeNavig, offre.CodeOffre, offre.Version, offre.Type);
            if (lstRisques?.Any(x => x != null) ?? false)
            {
                var cible = lstRisques.First(x => x != null).Cible;
                string codeCible = cible != null ? cible.Code : string.Empty;
                var branche = CommonRepository.GetBrancheCible(offre.CodeOffre, offre.Version.ToString(), offre.Type, codeAvn, modeNavig);
                string codeBranche = branche != null ? branche.Code : string.Empty;

                EacParameter[] param = new EacParameter[2];
                param[0] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
                param[0].Value = codeCible;
                param[1] = new EacParameter("codeBranche", DbType.AnsiStringFixedLength);
                param[1].Value = codeBranche;

                string req = @"SELECT count(*) NBLIGN FROM YCATEGO INNER JOIN KCIBLEF ON CABRA=KAIBRA AND CASBR=KAISBR AND CACAT=KAICAT WHERE KAICIBLE= :codeCible AND KAIBRA = :codeBranche";
                if (CommonRepository.ExistRowParam(req, param))
                {
                    req = @"SELECT CACNP FROM YCATEGO INNER JOIN KCIBLEF ON CABRA=KAIBRA AND CASBR=KAISBR AND CACAT=KAICAT WHERE KAICIBLE= :codeCible AND KAIBRA = :codeBranche";
                    var result = DbBase.Settings.ExecuteScalar(CommandType.Text, req, param);
                    if (result != null)
                    {
                        return Convert.ToDecimal(result) != 0;
                    }
                }
            }
            return true;
        }
        public static OffreDto GetOffre(OffrePlatDto offrePlatDto, bool fromRecherche = false)
        {
            var offre = new OffreDto();
            var adresse = GetAdresse(offrePlatDto);
            var branche = new BrancheDto
            {
                Code = offrePlatDto.CodeBranche,
                Nom = offrePlatDto.NomBaranche,
                Cible = new CibleDto
                {
                    Code = offrePlatDto.CodeCible,
                    Nom = offrePlatDto.NomCible
                }
            };

            var cabinetCourtage = CabinetCourtageRepository.Obtenir(offrePlatDto.CodeCabinetCourtage);
            if (cabinetCourtage != null)
                cabinetCourtage.Delegation = DelegationRepository.Obtenir(offrePlatDto.CodeCabinetCourtage);

            var cabinetCourtageBis = CabinetCourtageRepository.Obtenir(offrePlatDto.CodeCabinetCourtageBis);
            if (cabinetCourtageBis != null)
                cabinetCourtageBis.Delegation = DelegationRepository.Obtenir(offrePlatDto.CodeCabinetCourtageBis);

            offre.PreneurAssurance = AssureRepository.Obtenir(offrePlatDto.CodeAssure);
            if (offre.PreneurAssurance != null)
            {
                offre.PreneurAssurance.PreneurEstAssure = string.IsNullOrEmpty(offrePlatDto.PreneurEstAssure)
                    ? false : (offrePlatDto.PreneurEstAssure == "O" ? true : false);
            }
            offre.CodeOffre = !string.IsNullOrEmpty(offrePlatDto.CodeOffre) ? offrePlatDto.CodeOffre.PadLeft(9, ' ') : string.Empty;
            offre.Descriptif = !string.IsNullOrEmpty(offrePlatDto.Descriptif) ? offrePlatDto.Descriptif.Trim() : string.Empty;
            offre.Interlocuteur = new InterlocuteurDto
            {
                Id = offrePlatDto.CodeInterlocuteur,
                CabinetCourtage = cabinetCourtage
            };
            offre.CodeInterlocuteur = offrePlatDto.CodeInterlocuteur.ToString();
            offre.NomInterlocuteur = offre.Interlocuteur.Id != 0 ? InterlocuteurRepository.RechercherNomInterlocuteur(offrePlatDto.CodeInterlocuteur, cabinetCourtage.Code) : string.Empty;
            if (string.IsNullOrEmpty(offre.NomInterlocuteur)) offre.CodeInterlocuteur = "0";

            offre.RefCourtier = !string.IsNullOrEmpty(offrePlatDto.RefCourtier) ? offrePlatDto.RefCourtier.Trim() : string.Empty;
            offre.RefChezCourtier = offre.RefCourtier;
            offre.Etat = !string.IsNullOrEmpty(offrePlatDto.Etat) ? offrePlatDto.Etat.Trim() : string.Empty;
            offre.Situation = !string.IsNullOrEmpty(offrePlatDto.Situation) ? offrePlatDto.Situation.Trim() : string.Empty;
            offre.Version = int.Parse(offrePlatDto.VersionOffre.ToString());
            offre.Type = offrePlatDto.TypeOffre;
            offre.MotCle1 = offrePlatDto.CodeMotsClef1;
            offre.MotCle2 = offrePlatDto.CodeMotsClef2;
            offre.MotCle3 = offrePlatDto.CodeMotsClef3;
            offre.Observation = offrePlatDto.Observation;
            offre.NumAvenant = offrePlatDto.NumAvenant;
            offre.MotifRefus = offrePlatDto.MotifRefus;
            offre.IdAdresseOffre = offrePlatDto.IdAdresseOffre;
            offre.AdresseOffre = adresse;

            var elementAssure = new ElementAssureContratDto
            {
                ElementPrincipal = true,
                Adresse = adresse
            };
            AjouterElementAssure(elementAssure, offre);

            offre.CabinetGestionnaire = cabinetCourtage;
            offre.CabinetApporteur = cabinetCourtageBis;

            if (!fromRecherche)
            {
                offre.CabinetAutres = AlimenteAutreDoubleSaisie(offre.CodeOffre, offre.Version.ToString());
                offre.CabinetAutres.ForEach(elm => elm.Type = "2");
            }
            if (!string.IsNullOrEmpty(offrePlatDto.CodeAperiteur))
                offre.Aperiteur = new AperiteurDto
                {
                    Code = offrePlatDto.CodeAperiteur,
                    Nom = offrePlatDto.NomAperiteur
                };
            if (!string.IsNullOrEmpty(offrePlatDto.CodeGestionnaire))
                offre.Gestionnaire = new GestionnaireDto
                {
                    Id = offrePlatDto.CodeGestionnaire,
                    Nom = offrePlatDto.NomGestionnaire

                };
            offre.DateSaisie = AlbConvert.GetDate(offrePlatDto.DateSaisieAnnee, offrePlatDto.DateSaisieMois, offrePlatDto.DateSaisieJour, offrePlatDto.DateSaisieHeure);
            offre.DateEnregistrement = AlbConvert.GetDate(offrePlatDto.DateEnregistrementAnnee, offrePlatDto.DateEnregistrementMois, offrePlatDto.DateEnregistrementJour, 0);


            if (offrePlatDto.DateEffetGarantie != 0)
                offre.DateEffetGarantie = AlbConvert.ConvertIntToDateHour((Convert.ToInt64(offrePlatDto.DateEffetGarantie) * 10000) + offrePlatDto.DateEffetGarantieHeure);

            if (offrePlatDto.FinEffetGarantie != 0)
            {
                offre.DateFinEffetGarantie = AlbConvert.ConvertIntToDateHour((Convert.ToInt64(offrePlatDto.FinEffetGarantie) * 10000) + offrePlatDto.DateFinEffetGarantieHeure);
            }
            offre.DateMAJ = AlbConvert.GetDate(offrePlatDto.DateMAJAnnee, offrePlatDto.DateMAJMois, offrePlatDto.DateMAJJour, 0);
            offre.DateStatistique = AlbConvert.ConvertIntToDate(offrePlatDto.DateStatistique);
            offre.Branche = branche;
            if (!string.IsNullOrEmpty(offrePlatDto.CodeSouscripteur))
                offre.Souscripteur = new SouscripteurDto
                {
                    Code = offrePlatDto.CodeSouscripteur,
                    Nom = offrePlatDto.NomSouscripteur,
                    Branche = branche
                };
            offre.Devise = new ParametreDto
            {
                Code = offrePlatDto.Devise
            };
            offre.Periodicite = new ParametreDto
            {
                Code = offrePlatDto.Periodicite
            };

            var echeancePrincipale = AlbConvert.ConvertIntToDate((int?)offrePlatDto.EcheancePrincipale);

            int mois = 0;
            int jour = 0;

            if (echeancePrincipale.HasValue)
            {
                mois = echeancePrincipale.Value.Month;
                jour = echeancePrincipale.Value.Day;
            }


            if (mois != 0 && jour != 0)
            {
                //int annee = DateTime.Now.Year;
                // ECM : 2012 en dur pour gérer les années bisextiles
                offre.EcheancePrincipale = new DateTime(2012, mois, jour);
            }
            else offre.EcheancePrincipale = null;
            offre.DureeGarantie = offrePlatDto.DureeGarantie;

            offre.UniteDeTemps = new ParametreDto
            {
                Code = offrePlatDto.UniteTemps
            };
            offre.IndiceReference = new ParametreDto
            {
                Code = offrePlatDto.IndiceReference
            };
            offre.NatureContrat = new ParametreDto
            {
                Code = offrePlatDto.CodeNatureContrat,
                Libelle = offrePlatDto.LibelleNatureContrat
            };

            offre.Valeur = Convert.ToDecimal(offrePlatDto.Valeur);
            offre.PartAlbingia = Convert.ToDecimal(offrePlatDto.PartAlbingia);
            offre.Couverture = Convert.ToInt32(offrePlatDto.Couverture);
            offre.FraisAperition = Convert.ToInt32(offrePlatDto.FraisAperition);
            offre.IntercalaireCourtier = offrePlatDto.IntercalaireExiste == "O";
            offre.PartBenef = offrePlatDto.PartBenef;
            #region Bandeau
            offre.CodeRegime = offrePlatDto.CodeRegime;
            offre.SoumisCatNat = offrePlatDto.SoumisCatNat;
            offre.EtatLib = offrePlatDto.LibelleEtat;
            #endregion
            offre.LTA = offrePlatDto.LTA == "O";

            return offre;
        }

        public static OffrePlatDto GetOffreInfosGen(string sql, ModifierOffreGetQueryDto query)
        {
            return DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql).FirstOrDefault();
        }

        public static OffrePlatDto GetOffreInfosGen(string sql, ModifierOffreGetQueryDto query, IEnumerable<DbParameter> parameters)
        {
            return DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql, parameters).FirstOrDefault();
        }

        private static AdressePlatDto GetAdresse(OffrePlatDto offrePlatDto)
        {
            if (offrePlatDto.IdAdresseOffre == 0) return null;
            int numVoie = 0;
            int cp = 0;
            var adresseCabinetCourtage = new AdressePlatDto
            {
                Batiment = offrePlatDto.Batiment,
                BoitePostale = offrePlatDto.BoitePostale,
                ExtensionVoie = offrePlatDto.ExtensionVoie,
                NomVoie = offrePlatDto.NomVoie,
                NumeroChrono = offrePlatDto.IdAdresseOffre,
                NumeroVoie = Int32.TryParse(offrePlatDto.NumeroVoie.ToString(), out numVoie) ? numVoie : numVoie,
                NumeroVoie2 = offrePlatDto.NumeroVoie2,
                CodePostal = Int32.TryParse(offrePlatDto.CodePostal.ToString().PadLeft(3, '0'), out cp) ? cp : cp,
                NomVille = offrePlatDto.NomVille,
                TypeCedex = offrePlatDto.TypeCedex,
                CodePays = offrePlatDto.CodePays,
                Departement = offrePlatDto.Departement,
                NomPays = offrePlatDto.NomPays,
                MatriculeHexavia = offrePlatDto.MatriculeHexavia.ToString(),
                Latitude = offrePlatDto.Latitude,
                Longitude = offrePlatDto.Longitude
            };
            if (adresseCabinetCourtage.CodePostalCedex == -1)
                adresseCabinetCourtage.CodePostalCedex = 0;
            if (adresseCabinetCourtage.NumeroVoie == -1)
                adresseCabinetCourtage.NumeroVoie = 0;
            if (adresseCabinetCourtage.CodePostal == -1)
                adresseCabinetCourtage.CodePostal = 0;
            return adresseCabinetCourtage;
        }

        private static RisqueDto GetRisque(ModeConsultation modeNavig, RisquePlatDto risquePlatDto, string offreId, string type, int? offreVersion = null, string codeAvn = "")
        {
            var risque = new RisqueDto();
            risque.Code = risquePlatDto.Code;
            risque.Descriptif = string.IsNullOrEmpty(risquePlatDto.Descriptif) ? string.Empty : risquePlatDto.Descriptif.Trim();
            risque.Cible = new CibleDto
            {
                Code = string.IsNullOrEmpty(risquePlatDto.CodeCible) ? string.Empty : risquePlatDto.CodeCible.Trim(),
                Nom = string.IsNullOrEmpty(risquePlatDto.LibelleCible) ? string.Empty : risquePlatDto.LibelleCible.Trim()
            };
            risque.Designation = risquePlatDto.Designation;
            risque.ChronoDesi = Convert.ToInt32(risquePlatDto.ChronoDesi);
            risque.EntreeGarantie = AlbConvert.GetDate(risquePlatDto.EntreeGarantieAnnee, risquePlatDto.EntreeGarantieMois, risquePlatDto.EntreeGarantieJour, risquePlatDto.EntreeGarantieHeure);
            risque.SortieGarantie = AlbConvert.GetDate(risquePlatDto.SortieGarantieAnnee, risquePlatDto.SortieGarantieMois, risquePlatDto.SortieGarantieJour, risquePlatDto.SortieGarantieHeure);
            risque.SortieGarantieHisto = AlbConvert.GetDate(risquePlatDto.SortieGarantieAnneeHisto, risquePlatDto.SortieGarantieMoisHisto, risquePlatDto.SortieGarantieJourHisto, risquePlatDto.SortieGarantieHeureHisto);
            risque.Valeur = risquePlatDto.Valeur;
            risque.Unite = new ParametreDto
            {
                Code = !string.IsNullOrEmpty(risquePlatDto.CodeUnite) ? risquePlatDto.CodeUnite.Trim() : string.Empty
            };
            risque.Type = new ParametreDto
            {
                Code = !string.IsNullOrEmpty(risquePlatDto.CodeType) ? risquePlatDto.CodeType.Trim() : string.Empty
            };
            risque.ValeurHT = !string.IsNullOrEmpty(risquePlatDto.ValeurHT) ? risquePlatDto.ValeurHT.Trim() : string.Empty;
            //risque.CoutM2 = risquePlatDto.CoutM2;
            risque.CodeObjet = risquePlatDto.CodeObjet;
            risque.RisqueIndexe = risquePlatDto.IsRisqueIndexe == "O";
            risque.LCI = risquePlatDto.IsLCI == "O";
            risque.Franchise = risquePlatDto.IsFranchise == "O";
            risque.Assiette = risquePlatDto.IsAssiette == "O";
            risque.RegimeTaxe = risquePlatDto.RegimeTaxe;
            risque.CATNAT = risquePlatDto.IsCATNAT == "O";
            risque.IdAdresseRisque = risquePlatDto.IdAdresseRisque;
            risque.CodeApe = risquePlatDto.CodeApe;
            risque.Nomenclature1 = risquePlatDto.Nomenclature1;
            risque.Nomenclature2 = risquePlatDto.Nomenclature2;
            risque.Nomenclature3 = risquePlatDto.Nomenclature3;
            risque.Nomenclature4 = risquePlatDto.Nomenclature4;
            risque.Nomenclature5 = risquePlatDto.Nomenclature5;
            risque.CodeTre = risquePlatDto.CodeTre;
            risque.LibTre = risquePlatDto.LibTre;
            risque.CodeClasse = risquePlatDto.CodeClasse;
            risque.Territorialite = risquePlatDto.Territorialite;

            risque.TypeRisque = risquePlatDto.TypeRisque;
            risque.TypeMateriel = risquePlatDto.TypeMateriel;
            risque.NatureLieux = risquePlatDto.NatureLieux;

            risque.TauxAppel = risquePlatDto.TauxAppel;
            risque.IsRegularisable = risquePlatDto.IsRegularisable;
            risque.TypeRegularisation = risquePlatDto.TypeRegularisation;

            risque.DateEntreeDescr = AlbConvert.ConvertIntToDate(risquePlatDto.DateDebDesc);
            risque.HeureEntreeDescr = AlbConvert.ConvertIntToTimeMinute(risquePlatDto.HeureDebDesc);
            risque.DateSortieDescr = AlbConvert.ConvertIntToDate(risquePlatDto.DateFinDesc);
            risque.HeureSortieDescr = AlbConvert.ConvertIntToTimeMinute(risquePlatDto.HeureFinDesc);

            if (modeNavig != ModeConsultation.Historique)
                risque.IsTraceAvnExist = CommonRepository.ExistTraceAvenant(offreId, offreVersion.HasValue ? offreVersion.Value.ToString() : "0", type, "RSQ", risque.Code.ToString(), string.Empty, string.Empty, string.Empty, "**********");
            else
            {
                EacParameter[] param = new EacParameter[3];
                param[0] = new EacParameter("offreId", DbType.AnsiStringFixedLength);
                param[0].Value = offreId;
                param[1] = new EacParameter("offreVersion", DbType.Int32);
                param[1].Value = offreVersion.Value;
                param[2] = new EacParameter("risqueCode", DbType.AnsiStringFixedLength);
                param[2].Value = risque.Code;
                string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM {0} WHERE JEIPB = :offreId AND JEALX = :offreVersion AND JERSQ = :risqueCode", CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"));
                risque.IsTraceAvnExist = CommonRepository.ExistRowParam(sql, param);
            }

            if (risquePlatDto.DateEffetAvnAnnee > 0 && risquePlatDto.DateEffetAvnJour > 0 && risquePlatDto.DateEffetAvnMois > 0
                && (risquePlatDto.AvnCreationRsq.ToString() == codeAvn || risquePlatDto.AvnModifRsq.ToString() == codeAvn))
                risque.DateEffetAvenantModificationLocale = new DateTime(risquePlatDto.DateEffetAvnAnnee, risquePlatDto.DateEffetAvnMois, risquePlatDto.DateEffetAvnJour);
            risque.AvnCreationRsq = risquePlatDto.AvnCreationRsq;
            risque.AvnModifRsq = risquePlatDto.AvnModifRsq;

            //SLA (02/06/2015) : vu avec JDA, on ne prend plus en compte l'indexation du contrat/offre
            //risque.isIndexe = (risquePlatDto.IsRisqueIndexe != "N" && risquePlatDto.OffreIndexe != "N");
            risque.isIndexe = risquePlatDto.IsRisqueIndexe == "O";


            risque.PartBenef = risquePlatDto.PartBenefRsq;
            risque.NbYear = risquePlatDto.NbYear;
            risque.Ristourne = risquePlatDto.Ristourne;
            risque.CotisRetenue = risquePlatDto.CotisRetenue;
            risque.Seuil = risquePlatDto.Seuil;
            risque.TauxComp = risquePlatDto.TauxCompl;
            risque.IsRisqueTemporaire = risquePlatDto.IsRisqueTemporaire == "O";

            risque.TauxMaxi = risquePlatDto.TauxMaxi;
            risque.PrimeMaxi = risquePlatDto.PrimeMaxi;

            risque.EchAnnee = risquePlatDto.EchAnnee;
            risque.EchMois = risquePlatDto.EchMois;
            risque.EchJour = risquePlatDto.EchJour;

            risque.Objets = ObtenirObjet(modeNavig, offreId, risque.Code, type, offreVersion, codeAvn);

            return risque;
        }
        private static ObjetDto GetObjet(ModeConsultation modeNavig, ObjetPlatDto objetPlatDto, string codeAvn)
        {
            var objet = new ObjetDto();
            objet.IsReportvaleur = objetPlatDto.ReportVal == "O";
            objet.Code = objetPlatDto.Code;
            objet.Descriptif = objetPlatDto.Descriptif;
            objet.Cible = new CibleDto
            {
                Code = objetPlatDto.CodeCible
            };
            objet.ChronoDesi = Convert.ToInt32(objetPlatDto.ChronoDesi);
            objet.Designation = objetPlatDto.Designation;
            objet.EntreeGarantie = AlbConvert.GetDate(objetPlatDto.EntreeGarantieAnnee, objetPlatDto.EntreeGarantieMois, objetPlatDto.EntreeGarantieJour, objetPlatDto.EntreeGarantieHeure);
            objet.SortieGarantie = AlbConvert.GetDate(objetPlatDto.SortieGarantieAnnee, objetPlatDto.SortieGarantieMois, objetPlatDto.SortieGarantieJour, objetPlatDto.SortieGarantieHeure);
            objet.SortieGarantieHisto = AlbConvert.GetDate(objetPlatDto.SortieGarantieAnneeHisto, objetPlatDto.SortieGarantieMoisHisto, objetPlatDto.SortieGarantieJourHisto, objetPlatDto.SortieGarantieHeureHisto);
            objet.Valeur = objetPlatDto.Valeur;
            objet.Unite = new ParametreDto
            {
                Code = !string.IsNullOrEmpty(objetPlatDto.CodeUnite) ? objetPlatDto.CodeUnite.Trim() : string.Empty
            };
            objet.Type = new ParametreDto
            {
                Code = !string.IsNullOrEmpty(objetPlatDto.CodeType) ? objetPlatDto.CodeType.Trim() : objetPlatDto.CodeType
            };
            objet.ValeurHT = !string.IsNullOrEmpty(objetPlatDto.ValeurHT) ? objetPlatDto.ValeurHT.Trim() : string.Empty;
            objet.CoutM2 = objetPlatDto.CoutM2;
            objet.AdresseObjet = AdresseRepository.ObtenirAdresse(objetPlatDto.NumeroChrono);//GetObjetAdresse(objetPlatDto);
            objet.RisqueIndexe = objetPlatDto.IsRisqueIndexe == "O";
            objet.LCI = objetPlatDto.IsLCI == "O";
            objet.Franchise = objetPlatDto.IsFranchise == "O";
            objet.Assiette = objetPlatDto.IsAssiette == "O";
            objet.RegimeTaxe = objetPlatDto.RegimeTaxe;
            objet.CATNAT = objetPlatDto.IsCATNAT == "O";
            objet.Inventaires = ObtenirInventaires(modeNavig, objetPlatDto.CodeRisque, objetPlatDto.Code, objetPlatDto.IdOffre, objetPlatDto.VersionOffre, codeAvn);
            objet.CodeApe = objetPlatDto.CodeApe;
            objet.Nomenclature1 = objetPlatDto.Nomenclature1;
            objet.Nomenclature2 = objetPlatDto.Nomenclature2;
            objet.Nomenclature3 = objetPlatDto.Nomenclature3;
            objet.Nomenclature4 = objetPlatDto.Nomenclature4;
            objet.Nomenclature5 = objetPlatDto.Nomenclature5;
            objet.CodeTre = objetPlatDto.CodeTre;
            objet.LibTre = objetPlatDto.LibTre;
            objet.CodeClasse = objetPlatDto.CodeClasse;
            objet.Territorialite = objetPlatDto.Territorialite;
            objet.EnsembleType = objetPlatDto.EnsembleType;

            objet.TypeRisque = objetPlatDto.TypeRisque;
            objet.TypeMateriel = objetPlatDto.TypeMateriel;
            objet.NatureLieux = objetPlatDto.NatureLieux;
            objet.DateModificationObjetRisque = AlbConvert.ConvertIntToDate(objetPlatDto.DateModificationObjetRisque);
            objet.DateEntreeDescr = AlbConvert.ConvertIntToDate(objetPlatDto.DateDebDesc);
            objet.DateSortieDescr = AlbConvert.ConvertIntToDate(objetPlatDto.DateFinDesc);
            objet.HeureEntreeDescr = AlbConvert.ConvertIntToTimeMinute(objetPlatDto.HeureDebDesc);
            objet.HeureSortieDescr = AlbConvert.ConvertIntToTimeMinute(objetPlatDto.HeureFinDesc);

            objet.IndiceOrigine = objetPlatDto.IndiceOrigine;
            objet.IndiceActualise = objetPlatDto.IndiceActualise;

            if (objetPlatDto.DateEffetAvenantAnneeobj > 0 && objetPlatDto.DateEffetAvenantJourobj > 0 && objetPlatDto.DateEffetAvenantMoisobj > 0
                && (objetPlatDto.AvnCreationObj.ToString() == codeAvn || objetPlatDto.AvnModifObj.ToString() == codeAvn))
                objet.DateEffetAvenantModificationLocale = new DateTime(objetPlatDto.DateEffetAvenantAnneeobj, objetPlatDto.DateEffetAvenantMoisobj, objetPlatDto.DateEffetAvenantJourobj);

            if (objetPlatDto.DateModifAvenantAnneeobj > 0 && objetPlatDto.DateModifAvenantMoisobj > 0 && objetPlatDto.DateModifAvenantJourobj > 0
                   && (objetPlatDto.AvnCreationObj.ToString() == codeAvn || objetPlatDto.AvnModifObj.ToString() == codeAvn))

                objet.DateModifAvenantModificationLocale = new DateTime(objetPlatDto.DateModifAvenantAnneeobj, objetPlatDto.DateModifAvenantMoisobj, objetPlatDto.DateModifAvenantJourobj);




            DateTime? dDateEffetAVN = null;
            if (objetPlatDto.DateEffetAvenantAnnee != 0 && objetPlatDto.DateEffetAvenantMois != 0 && objetPlatDto.DateEffetAvenantJour != 0)
            {
                dDateEffetAVN = new DateTime(objetPlatDto.DateEffetAvenantAnnee, objetPlatDto.DateEffetAvenantMois, objetPlatDto.DateEffetAvenantJour);
            }
            DateTime? dDateFinEffet = null;
            if (objetPlatDto.DateFinEffetAnnee != 0 && objetPlatDto.DateFinEffetMois != 0 && objetPlatDto.DateFinEffetJour != 0)
            {
                dDateFinEffet = new DateTime(objetPlatDto.DateFinEffetAnnee, objetPlatDto.DateFinEffetMois, objetPlatDto.DateFinEffetJour);
            }
            DateTime? dDateFinPeriode = null;
            if (objetPlatDto.DateEcheanceAnnee != 0 && objetPlatDto.DateEcheanceMois != 0 && objetPlatDto.DateEcheanceJour != 0)
            {
                dDateFinPeriode = new DateTime(objetPlatDto.DateEcheanceAnnee, objetPlatDto.DateEcheanceMois, objetPlatDto.DateEcheanceJour);
            }
            if (dDateEffetAVN.HasValue && dDateEffetAVN.Value != default(DateTime) && objet.EntreeGarantie.HasValue && objet.EntreeGarantie != default(DateTime))
            {
                if (objet.EntreeGarantie < dDateEffetAVN)
                    objet.IsAlertePeriode = true;
            }
            if (dDateFinPeriode.HasValue && dDateFinPeriode.Value != default(DateTime))
            {
                dDateFinPeriode = dDateFinPeriode.Value.AddDays(-1);
            }
            else
            {
                dDateFinPeriode = dDateFinEffet;
            }
            if (dDateFinPeriode.HasValue && dDateFinPeriode.Value != default(DateTime))
            {
                if (objet.SortieGarantie <= dDateFinPeriode)
                    objet.IsAlertePeriode = true;
                //2014-12-29 : si l'objet n'a pas de date de sortie, il ne peut être sorti de l'avenant
                if (!objet.SortieGarantie.HasValue)
                    objet.IsAlertePeriode = false;
            }
            objet.AvnCreationObj = objetPlatDto.AvnCreationObj;

            if (objet.AvnCreationObj.ToString() == codeAvn)
            {
                objet.DateEffetAvenantOBJ = dDateEffetAVN;
            }
            else
            {

                if (objetPlatDto.DateEffetAvenantAnneeobj != 0 && objetPlatDto.DateEffetAvenantMoisobj != 0 && objetPlatDto.DateEffetAvenantJourobj != 0)
                    objet.DateEffetAvenantOBJ = new DateTime(objetPlatDto.DateEffetAvenantAnneeobj, objetPlatDto.DateEffetAvenantMoisobj, objetPlatDto.DateEffetAvenantJourobj);

            }


            #region Détermination de la sortie ou non de l'objet et s'il doit être affiché
            //Par défaut on considère que l'objet est sorti : 
            objet.IsSortiAvenant = true;
            objet.IsAfficheAvenant = false;
            //Si la date d'entrée de l'objet est supérieure ou égale à la date d'avenant du contrat
            if (dDateEffetAVN.HasValue && objet.EntreeGarantie >= dDateEffetAVN.Value)
            {
                objet.IsSortiAvenant = false;
            }
            //Si la date de sortie de l'objet est supérieure ou égale à la date d'avenant du contrat
            if (dDateEffetAVN.HasValue && objet.SortieGarantie >= dDateEffetAVN.Value)
            {
                objet.IsSortiAvenant = false;
            }
            //Si la date de sortie de l'objet est égale à 0
            if (dDateEffetAVN.HasValue && objet.SortieGarantie == null)
            {
                objet.IsSortiAvenant = false;
            }
            //Si l'objet n'est pas sorti, on l'affiche
            objet.IsAfficheAvenant = !objet.IsSortiAvenant;
            //Si l'objet a été créé dans l'avenant, on l'affiche aussi
            if (objet.AvnCreationObj.ToString() == codeAvn)
            {
                objet.IsAfficheAvenant = true;
            }
            //Si l'objet a été modifié dans l'avenant, on l'affiche aussi
            if (objetPlatDto.AvnModifObj.ToString() == codeAvn)
            {
                objet.IsAfficheAvenant = true;
            }
            #endregion

            objet.IsRisqueTemporaire = objetPlatDto.IsRisqueTemporaire == "O";
            return objet;
        }
        private static InventaireDto GetInventaire(InventairePlatDto invPlatDto)
        {
            var inventaire = new InventaireDto();
            inventaire.Descriptif = invPlatDto.Descriptif;
            inventaire.Type = new ParametreDto { Code = invPlatDto.CodeType };
            inventaire.Code = invPlatDto.Code;
            inventaire.NumDescription = invPlatDto.NumDescription.ToString();
            inventaire.PerimetreApplication = invPlatDto.PerimetreApplication;
            return inventaire;
        }


        public static void AjouterElementAssure(ElementAssureContratDto argElementAssure, OffreDto offre)
        {
            if (offre.ElementAssures == null)
            {
                offre.ElementAssures = new List<ElementAssureContratDto>();
            }

            if (argElementAssure.ElementPrincipal)
            {
                offre.ElementAssures.Where(x => x.ElementPrincipal).Select(x => x.ElementPrincipal = false);
            }
            offre.ElementAssures.Add(argElementAssure);
        }

        #region Création Offre

        public static string SaveOffre(OffreDto offre, string user)
        {
            if (offre.Type != AlbConstantesMetiers.TYPE_OFFRE)
                throw new AlbFoncException(string.Format("Erreur d'enregistrement de l'offre n°", offre.CodeOffre.Trim()), true, true);


            CommonRepository.DeleteTraceFormule(offre.CodeOffre, offre.Version ?? 0, offre.Type,
                                    offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Year : 0,
                                    offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Month : 0,
                                    offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Day : 0,
                                    offre.DateSaisie.HasValue ? AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(offre.DateSaisie)) ?? 0 : 0);

            int cp = 0;
            string codePostal = string.Empty;
            if (offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.CodePostal.ToString()))
            {
                if (offre.AdresseOffre.CodePostal.ToString().Length > 3)
                {
                    int.TryParse(offre.AdresseOffre.CodePostal.ToString("D5").Substring(2, 3), out cp);
                    codePostal = cp.ToString().PadLeft(3, '0');
                }
                else if (offre.AdresseOffre.CodePostal != -1)
                {
                    codePostal = offre.AdresseOffre.CodePostal.ToString().PadLeft(3, '0');
                }
            }

            string numeroVoie = string.Empty;
            if (offre.AdresseOffre != null && offre.AdresseOffre.NumeroVoie != -1)
            {
                numeroVoie = offre.AdresseOffre.NumeroVoie.ToString();
            }

            var dateNow = DateTime.Now;

            int cpCedex = 0;
            string codePostalCedex = string.Empty;
            if (!string.IsNullOrEmpty(offre.AdresseOffre.CodePostalCedex.ToString()))
            {
                if (offre.AdresseOffre.CodePostalCedex.ToString().Length > 3)
                {
                    int.TryParse(offre.AdresseOffre.CodePostalCedex.ToString("D5").Substring(2, 3), out cpCedex);
                    codePostalCedex = cpCedex.ToString().PadLeft(3, '0');
                }
                else if (offre.AdresseOffre.CodePostalCedex != -1)
                {
                    codePostalCedex = offre.AdresseOffre.CodePostalCedex.ToString().PadLeft(3, '0');
                }
            }

            int isAddressEmpty = 0;

            EacParameter[] param = new EacParameter[58];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = offre.Version.HasValue ? offre.Version.Value : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = offre.Type;
            param[3] = new EacParameter("P_INTERLOCUTEUR", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(offre.CodeInterlocuteur) ? Convert.ToInt32(offre.CodeInterlocuteur) : 0;
            param[4] = new EacParameter("P_DESCRIPTIF", DbType.AnsiStringFixedLength);
            param[4].Value = string.IsNullOrEmpty(offre.Descriptif) ? string.Empty : offre.Descriptif;
            param[5] = new EacParameter("P_MOTCLE1", DbType.AnsiStringFixedLength);
            param[5].Value = string.IsNullOrEmpty(offre.MotCle1) ? string.Empty : offre.MotCle1;
            param[6] = new EacParameter("P_MOTCLE2", DbType.AnsiStringFixedLength);
            param[6].Value = string.IsNullOrEmpty(offre.MotCle2) ? string.Empty : offre.MotCle2;
            param[7] = new EacParameter("P_MOTCLE3", DbType.AnsiStringFixedLength);
            param[7].Value = string.IsNullOrEmpty(offre.MotCle3) ? string.Empty : offre.MotCle3;
            param[8] = new EacParameter("P_CODEPRENEURASSU", DbType.Int32);
            param[8].Value = offre.PreneurAssurance != null && !string.IsNullOrEmpty(offre.PreneurAssurance.Code) ? Convert.ToInt32(offre.PreneurAssurance.Code) : 0;
            param[9] = new EacParameter("P_COURTIERGESTCODE", DbType.Int32);
            param[9].Value = offre.CabinetGestionnaire != null ? offre.CabinetGestionnaire.Code : 0;
            param[10] = new EacParameter("P_COURTIERAPPCODE", DbType.Int32);
            param[10].Value = offre.CabinetApporteur != null ? offre.CabinetApporteur.Code : 0;
            param[11] = new EacParameter("P_REFCOURTIER", DbType.AnsiStringFixedLength);
            param[11].Value = offre.RefChezCourtier != null ? offre.RefChezCourtier : string.Empty;
            param[12] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
            param[12].Value = offre.Branche != null ? offre.Branche.Code : string.Empty;
            param[13] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[13].Value = user;
            param[14] = new EacParameter("P_YEARNOW", DbType.Int32);
            param[14].Value = dateNow.Year;
            param[15] = new EacParameter("P_MONTHNOW", DbType.Int32);
            param[15].Value = dateNow.Month;
            param[16] = new EacParameter("P_DAYNOW", DbType.Int32);
            param[16].Value = dateNow.Day;
            param[17] = new EacParameter("P_SOUSCRIPTEURCODE", DbType.AnsiStringFixedLength);
            param[17].Value = offre.Souscripteur != null ? string.IsNullOrEmpty(offre.Souscripteur.Code) ? string.Empty : offre.Souscripteur.Code : string.Empty;
            param[18] = new EacParameter("P_GESTIONNAIRECODE", DbType.AnsiStringFixedLength);
            param[18].Value = offre.Gestionnaire != null ? string.IsNullOrEmpty(offre.Gestionnaire.Id) ? string.Empty : offre.Gestionnaire.Id : string.Empty;
            param[19] = new EacParameter("P_YEARSAISIE", DbType.Int32);
            param[19].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Year : 0;
            param[20] = new EacParameter("P_MONTHSAISIE", DbType.Int32);
            param[20].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Month : 0;
            param[21] = new EacParameter("P_DAYSAISIE", DbType.Int32);
            param[21].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Day : 0;
            param[22] = new EacParameter("P_HOURSAISIE", DbType.Int32);
            param[22].Value = offre.DateSaisie.HasValue ? AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(offre.DateSaisie)) : 0;
            param[23] = new EacParameter("P_OBSERVATION", DbType.AnsiStringFixedLength);
            param[23].Value = string.IsNullOrEmpty(offre.Observation) ? string.Empty : offre.Observation;
            param[24] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
            param[24].Value = offre.Branche != null && offre.Branche.Cible != null ? offre.Branche.Cible.Code : string.Empty;
            param[25] = new EacParameter("P_INDICEREF", DbType.AnsiStringFixedLength);
            param[25].Value = offre.IndiceReference != null && offre.IndiceReference.Code != null ? offre.IndiceReference.Code : string.Empty;
            param[26] = new EacParameter("P_VALEUR", DbType.Int32);
            param[26].Value = offre.Valeur;
            param[27] = new EacParameter("P_INTERCALAIRE", DbType.AnsiStringFixedLength);
            param[27].Value = offre.IntercalaireCourtier ? "O" : "N";
            param[28] = new EacParameter("P_CATNAT", DbType.AnsiStringFixedLength);
            param[28].Value = !string.IsNullOrEmpty(offre.SoumisCatNat) ? offre.SoumisCatNat : string.Empty;
            param[29] = new EacParameter("P_DATENOW", DbType.Int32);
            param[29].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[30] = new EacParameter("P_HOURNOW", DbType.Int32);
            param[30].Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(dateNow));
            param[31] = new EacParameter("P_NATURECONTRAT", DbType.AnsiStringFixedLength);
            param[31].Value = offre.NatureContrat != null && offre.NatureContrat.Code != null ? offre.NatureContrat.Code : string.Empty;
            param[32] = new EacParameter("P_APERITEURCODE", DbType.AnsiStringFixedLength);
            param[32].Value = offre.Aperiteur != null && offre.Aperiteur.Code != null ? offre.Aperiteur.Code : string.Empty;
            param[33] = new EacParameter("P_PARTAPERITEUR", DbType.Int32);
            param[33].Value = offre.PartAperiteur.HasValue ? offre.PartAperiteur.Value : 0;
            param[34] = new EacParameter("P_FRAISAPERITION", DbType.Int32);
            param[34].Value = offre.FraisAperition.HasValue ? offre.FraisAperition.Value : 0;
            param[35] = new EacParameter("P_HASADRESSE", DbType.AnsiStringFixedLength);
            param[35].Value = offre.AdresseOffre != null ? "O" : "N";
            param[36] = new EacParameter("P_ADRCHRONO", DbType.Int32);
            param[36].Value = offre.AdresseOffre != null ? offre.AdresseOffre.NumeroChrono : 0;
            param[37] = new EacParameter("P_BATIMENT", DbType.AnsiStringFixedLength);
            param[37].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.Batiment) ? offre.AdresseOffre.Batiment : string.Empty;
            param[38] = new EacParameter("P_NUMVOIE", DbType.Int32);
            param[38].Value = numeroVoie;
            param[39] = new EacParameter("P_NUMVOIE2", DbType.AnsiStringFixedLength);
            param[39].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NumeroVoie2) ? offre.AdresseOffre.NumeroVoie2 : string.Empty;
            param[40] = new EacParameter("P_EXTVOIE", DbType.AnsiStringFixedLength);
            param[40].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.ExtensionVoie) ? offre.AdresseOffre.ExtensionVoie.Substring(0, 1) : string.Empty;
            param[41] = new EacParameter("P_NOMVOIE", DbType.AnsiStringFixedLength);
            param[41].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NomVoie) ? offre.AdresseOffre.NomVoie : string.Empty;
            param[42] = new EacParameter("P_BP", DbType.AnsiStringFixedLength);
            param[42].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.BoitePostale) ? offre.AdresseOffre.BoitePostale : string.Empty;
            param[43] = new EacParameter("P_LOC", DbType.AnsiStringFixedLength);
            param[43].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.CodePostal.ToString()) ? codePostal : string.Empty;
            param[44] = new EacParameter("P_DEPARTEMENT", DbType.AnsiStringFixedLength);
            param[44].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.Departement) && offre.AdresseOffre.Departement != "0" ? offre.AdresseOffre.Departement.PadLeft(2, '0') : string.Empty;
            param[45] = new EacParameter("P_CP", DbType.AnsiStringFixedLength);
            param[45].Value = codePostal;
            param[46] = new EacParameter("P_VILLE", DbType.AnsiStringFixedLength);
            param[46].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NomVille) ? offre.AdresseOffre.NomVille : string.Empty;
            param[47] = new EacParameter("P_VOIECOMPLETE", DbType.AnsiStringFixedLength);
            param[47].Value = offre.AdresseOffre != null ? string.Format("{0} {1} {2}", numeroVoie, offre.AdresseOffre.ExtensionVoie, offre.AdresseOffre.NomVoie) : string.Empty;
            param[48] = new EacParameter("P_VILLECOMPLETE", DbType.AnsiStringFixedLength);
            param[48].Value = offre.AdresseOffre != null ? string.Format("{0} {1}", (offre.AdresseOffre.CodePostal.ToString() != "0" && offre.AdresseOffre.CodePostal.ToString() != "-1" ? offre.AdresseOffre.CodePostalCedex.ToString() + offre.AdresseOffre.CodePostal.ToString() : string.Empty), offre.AdresseOffre.NomVille) : string.Empty;
            param[49] = new EacParameter("P_CPCDX", DbType.AnsiStringFixedLength);
            param[49].Value = codePostalCedex;
            param[50] = new EacParameter("P_VILLECDX", DbType.AnsiStringFixedLength);
            param[50].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NomCedex) ? offre.AdresseOffre.NomCedex : string.Empty;
            param[51] = new EacParameter("P_MATRICULEHEX", DbType.Int32);
            param[51].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.MatriculeHexavia) ? Convert.ToInt32(offre.AdresseOffre.MatriculeHexavia) : 0;
            param[52] = new EacParameter("P_DATESAISIE", DbType.Int32);
            param[52].Value = offre.DateSaisie.HasValue ? AlbConvert.ConvertDateToInt(offre.DateSaisie) : 0;
            param[53] = new EacParameter("P_PRENEURESTASSURE", DbType.Int32);
            param[53].Value = offre.PreneurAssurance.PreneurEstAssure ? "O" : "N";
            param[54] = new EacParameter("P_ISADDRESSEMPTY", DbType.Int32);

            var tmp = string.Concat(param[37].Value.ToString(), param[38].Value.ToString(), param[39].Value.ToString(), param[40].Value.ToString(), param[41].Value.ToString(),
                param[43].Value.ToString(), param[44].Value.ToString(), param[45].Value.ToString(), param[46].Value.ToString(), param[47].Value.ToString(),
                param[48].Value.ToString(), param[49].Value.ToString(), param[49].Value.ToString());

            if (tmp.Trim() == "")
            {
                isAddressEmpty = 1;
            }

            param[54].Value = isAddressEmpty;

            /***** Ajout Latitude et Longitude *****/
            param[55] = new EacParameter("P_LATITUDE", offre.AdresseOffre.Latitude != null ? offre.AdresseOffre.Latitude : 0);
            param[56] = new EacParameter("P_LONGITUDE", offre.AdresseOffre.Longitude != null ? offre.AdresseOffre.Longitude : 0);
            /****** Fin  Ajout Latitude et Longitude ******/



            param[57] = new EacParameter("P_MSGERROR", string.Empty);
            param[57].Direction = ParameterDirection.InputOutput;
            param[57].DbType = DbType.String;
            param[57].Size = 5000;

            var test = DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CREATEOFFRE", param);

            return param[57].Value.ToString().Trim();
        }

        #endregion

        public static List<UtilisateurDto> RechercherUtilisateurs(string name)
        {
            string sql = @"SELECT UTIUT CODE, UTNOM NOM, UTPNM PRENOM FROM YUTILIS
                                            WHERE UPPER(UTNOM) LIKE :name OR UPPER(UTIUT) LIKE :name";
            var result = DbBase.Settings.ExecuteList<UtilisateurDto>(
                CommandType.Text, sql, new[] { new EacParameter("name", value: $"%{name.ToUpper()}%") }
                );
            return result;
        }

        #region Prise Position

        public static void EnregistrerNouvellePosition(string codeOffre, string version, string type, string newEtat, string newSituation, string motif, string user)
        {
            EacParameter[] param = new EacParameter[10];
            param[0] = new EacParameter("newEtat", DbType.AnsiStringFixedLength);
            param[0].Value = newEtat;
            param[1] = new EacParameter("newSituation", DbType.AnsiStringFixedLength);
            param[1].Value = newSituation;
            param[2] = new EacParameter("user", DbType.AnsiStringFixedLength);
            param[2].Value = user;
            param[3] = new EacParameter("year", DbType.Int32);
            param[3].Value = DateTime.Now.Year;
            param[4] = new EacParameter("month", DbType.Int32);
            param[4].Value = DateTime.Now.Month;
            param[5] = new EacParameter("day", DbType.Int32);
            param[5].Value = DateTime.Now.Day;
            param[6] = new EacParameter("motif", DbType.AnsiStringFixedLength);
            param[6].Value = motif;
            param[7] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[7].Value = codeOffre.PadLeft(9, ' ');
            param[8] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[8].Value = version;
            param[9] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[9].Value = type;

            string sql = @"UPDATE YPOBASE
                                         SET PBETA = :newEtat,
                                         PBSIT = :newSituation,
                                         PBMJU= :user,
                                         PBMJA= :year,
                                         PBMJM= :month,
                                         PBMJJ= :day,
                                         PBSTF= :motif
                                         WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion


        #region Informations de base transverse

        public static void SaveInfoBase(OffreDto offre)
        {
            if (offre.Type != AlbConstantesMetiers.TYPE_OFFRE)
                throw new AlbFoncException(string.Format("Erreur d'enregistrement de l'offre n°", offre.CodeOffre.Trim()), true, true);

            int cp = 0;
            string codePostal = string.Empty;
            if (offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.CodePostal.ToString()))
            {
                if (offre.AdresseOffre.CodePostal.ToString().Length > 3)
                {
                    int.TryParse(offre.AdresseOffre.CodePostal.ToString("D5").Substring(2, 3), out cp);
                    codePostal = cp.ToString().PadLeft(3, '0');
                }
                else if (offre.AdresseOffre.CodePostal != -1)
                {
                    codePostal = offre.AdresseOffre.CodePostal.ToString().PadLeft(3, '0');
                }
            }

            string numeroVoie = string.Empty;
            if (offre.AdresseOffre != null && offre.AdresseOffre.NumeroVoie != -1)
            {
                numeroVoie = offre.AdresseOffre.NumeroVoie.ToString();
            }



            int cpCedex = 0;
            string codePostalCedex = string.Empty;
            if (!string.IsNullOrEmpty(offre.AdresseOffre.CodePostalCedex.ToString()))
            {
                if (offre.AdresseOffre.CodePostalCedex.ToString().Length > 3)
                {
                    int.TryParse(offre.AdresseOffre.CodePostalCedex.ToString("D5").Substring(2, 3), out cpCedex);
                    codePostalCedex = cpCedex.ToString().PadLeft(3, '0');
                }
                else if (offre.AdresseOffre.CodePostalCedex != -1)
                {
                    codePostalCedex = offre.AdresseOffre.CodePostalCedex.ToString().ToString().PadLeft(3, '0');
                }
            }

            EacParameter[] param = new EacParameter[36];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = offre.CodeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = offre.Version.HasValue ? offre.Version.Value : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = offre.Type;
            param[3] = new EacParameter("P_YEARSAISIE", DbType.Int32);
            param[3].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Year : 0;
            param[4] = new EacParameter("P_MONTHSAISIE", DbType.Int32);
            param[4].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Month : 0;
            param[5] = new EacParameter("P_DAYSAISIE", DbType.Int32);
            param[5].Value = offre.DateSaisie.HasValue ? offre.DateSaisie.Value.Day : 0;
            param[6] = new EacParameter("P_HOURSAISIE", DbType.Int32);
            param[6].Value = offre.DateSaisie.HasValue ? AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(offre.DateSaisie)) : 0;
            param[7] = new EacParameter("P_SOUSCRIPTEURCODE", DbType.AnsiStringFixedLength);
            param[7].Value = offre.Souscripteur != null ? string.IsNullOrEmpty(offre.Souscripteur.Code) ? string.Empty : offre.Souscripteur.Code : string.Empty;
            param[8] = new EacParameter("P_GESTIONNAIRECODE", DbType.AnsiStringFixedLength);
            param[8].Value = offre.Gestionnaire != null ? string.IsNullOrEmpty(offre.Gestionnaire.Id) ? string.Empty : offre.Gestionnaire.Id : string.Empty;
            param[9] = new EacParameter("P_INTERLOCUTEUR", DbType.Int32) { Value = int.TryParse(offre.CodeInterlocuteur, out int i) ? i : 0 };
            param[10] = new EacParameter("P_REFCOURTIER", DbType.AnsiStringFixedLength);
            param[10].Value = offre.RefCourtier != null ? offre.RefCourtier : string.Empty;
            param[11] = new EacParameter("P_CODEPRENEURASSU", DbType.Int32);
            param[11].Value = offre.PreneurAssurance != null && !string.IsNullOrEmpty(offre.PreneurAssurance.Code) ? Convert.ToInt32(offre.PreneurAssurance.Code) : 0;
            param[12] = new EacParameter("P_DESCRIPTIF", DbType.AnsiStringFixedLength);
            param[12].Value = string.IsNullOrEmpty(offre.Descriptif) ? string.Empty : offre.Descriptif;
            param[13] = new EacParameter("P_MOTCLE1", DbType.AnsiStringFixedLength);
            param[13].Value = string.IsNullOrEmpty(offre.MotCle1) ? string.Empty : offre.MotCle1;
            param[14] = new EacParameter("P_MOTCLE2", DbType.AnsiStringFixedLength);
            param[14].Value = string.IsNullOrEmpty(offre.MotCle2) ? string.Empty : offre.MotCle2;
            param[15] = new EacParameter("P_MOTCLE3", DbType.AnsiStringFixedLength);
            param[15].Value = string.IsNullOrEmpty(offre.MotCle3) ? string.Empty : offre.MotCle3;
            param[16] = new EacParameter("P_OBSERVATION", DbType.AnsiStringFixedLength);
            param[16].Value = string.IsNullOrEmpty(offre.Observation) ? string.Empty : offre.Observation;
            param[17] = new EacParameter("P_HASADRESSE", DbType.AnsiStringFixedLength);
            param[17].Value = offre.AdresseOffre != null ? "O" : "N";
            param[18] = new EacParameter("P_ADRCHRONO", DbType.Int32);
            param[18].Value = offre.AdresseOffre != null ? offre.AdresseOffre.NumeroChrono != 0 ? offre.AdresseOffre.NumeroChrono : 0 : 0;
            param[19] = new EacParameter("P_BATIMENT", DbType.AnsiStringFixedLength);
            param[19].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.Batiment) ? offre.AdresseOffre.Batiment : string.Empty;
            param[20] = new EacParameter("P_NUMVOIE", DbType.Int32);
            param[20].Value = numeroVoie;
            param[21] = new EacParameter("P_NUMVOIE2", DbType.AnsiStringFixedLength);
            param[21].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NumeroVoie2) ? offre.AdresseOffre.NumeroVoie2 : string.Empty;
            param[22] = new EacParameter("P_EXTVOIE", DbType.AnsiStringFixedLength);
            param[22].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.ExtensionVoie) ? offre.AdresseOffre.ExtensionVoie.Substring(0, 1) : string.Empty;
            param[23] = new EacParameter("P_NOMVOIE", DbType.AnsiStringFixedLength);
            param[23].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NomVoie) ? offre.AdresseOffre.NomVoie : string.Empty;
            param[24] = new EacParameter("P_BP", DbType.AnsiStringFixedLength);
            param[24].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.BoitePostale) ? offre.AdresseOffre.BoitePostale : string.Empty;
            param[25] = new EacParameter("P_LOC", DbType.AnsiStringFixedLength);
            param[25].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.CodePostal.ToString()) ? codePostal : string.Empty;
            param[26] = new EacParameter("P_DEPARTEMENT", DbType.AnsiStringFixedLength);
            param[26].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.Departement) ? offre.AdresseOffre.Departement : string.Empty;
            param[27] = new EacParameter("P_CP", DbType.Int32);
            param[27].Value = codePostal;
            param[28] = new EacParameter("P_VILLE", DbType.AnsiStringFixedLength);
            param[28].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NomVille) ? offre.AdresseOffre.NomVille : string.Empty;
            param[29] = new EacParameter("P_VOIECOMPLETE", DbType.AnsiStringFixedLength);
            param[29].Value = offre.AdresseOffre != null ? string.Format("{0} {1} {2}", numeroVoie, offre.AdresseOffre.ExtensionVoie, offre.AdresseOffre.NomVoie) : string.Empty;
            param[30] = new EacParameter("P_VILLECOMPLETE", DbType.AnsiStringFixedLength);
            param[30].Value = offre.AdresseOffre != null ? offre.AdresseOffre.Departement + codePostal + " " + offre.AdresseOffre.NomVille : string.Empty;
            param[31] = new EacParameter("P_CPCDX", DbType.Int32);
            param[31].Value = codePostalCedex;
            param[32] = new EacParameter("P_VILLECDX", DbType.AnsiStringFixedLength);
            param[32].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.NomCedex) ? offre.AdresseOffre.NomCedex : string.Empty;
            param[33] = new EacParameter("P_MATRICULEHEX", DbType.Int32);
            param[33].Value = offre.AdresseOffre != null && !string.IsNullOrEmpty(offre.AdresseOffre.MatriculeHexavia) ? Convert.ToInt32(offre.AdresseOffre.MatriculeHexavia) : 0;
            param[34] = new EacParameter("P_PRENEURESTASSURE", DbType.Int32);
            param[34].Value = offre.PreneurAssurance.PreneurEstAssure ? "O" : "N";
            param[35] = new EacParameter("P_ISADDRESSEMPTY", DbType.Int32);

            var tmp = string.Concat(param[19].Value.ToString(), param[20].Value.ToString(), param[21].Value.ToString(), param[22].Value.ToString(), param[23].Value.ToString(), param[24].Value.ToString(), param[25].Value.ToString(),
                param[26].Value.ToString(), param[27].Value.ToString(), param[28].Value.ToString(), param[29].Value.ToString(), param[30].Value.ToString(), param[31].Value.ToString(), param[32].Value.ToString());

            var isAddressEmpty = 0;
            if (tmp.Trim() == "")
            {
                isAddressEmpty = 1;
            }

            param[35].Value = isAddressEmpty;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEINFOBASE", param);

        }

        #endregion


        public static string GetOffreLastVersion(string codeOffre, string version, string type, string user)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("type", DbType.Int32);
            param[1].Value = type;
            string sql = @"SELECT MAX(PBALX) INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeOffre AND PBTYP = :type";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                var newversion = result.FirstOrDefault().Int32ReturnCol;
                //if (newversion < 0)
                //{
                //    //versionnning de l'offre
                //    var resNewVers = TraitementVarianteOffreRepository.CreationNouvelleVersionOffre(codeOffre, version, type, user, "V");
                //    if (!resNewVers)
                //        return string.Empty;
                //    else
                //    {
                //        result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
                //        if (result != null && result.Any())
                //            return result.FirstOrDefault().Int64ReturnCol.ToString();
                //    }
                //}
                //else
                return newversion.ToString();
            }
            return string.Empty;
        }


        public static RechercheOffresGetResultDto Test(ModeleParametresRechercheDto paramRecherche, ModeConsultation modeNavig)
        {
            RechercheOffresGetResultDto result = new RechercheOffresGetResultDto
            {
                LstOffres = new List<OffreDto>()
            };

            paramRecherche.CodeOffre = "     3217";
            paramRecherche.Type = "O";
            paramRecherche.TypeContrat = "";


            if (paramRecherche != null)
            {
                int cpCode = 0;
                int numAlm = 0;
                string typeDateRecherche = string.Empty;
                switch (paramRecherche.TypeDateRecherche)
                {
                    case AlbConstantesMetiers.TypeDateRecherche.Saisie:
                        typeDateRecherche = "Saisie";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Effet:
                        typeDateRecherche = "Effet";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.MAJ:
                        typeDateRecherche = "MAJ";
                        break;
                    case AlbConstantesMetiers.TypeDateRecherche.Creation:
                        typeDateRecherche = "Creation";
                        break;
                }

                #region Paramètres

                EacParameter[] param = new EacParameter[40];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = !string.IsNullOrEmpty(paramRecherche.CodeOffre) ? paramRecherche.CodeOffre.PadLeft(9, ' ').Replace("'", "''") : string.Empty;
                param[1] = new EacParameter("P_TYPEOFFRE", DbType.AnsiStringFixedLength);
                param[1].Value = !string.IsNullOrEmpty(paramRecherche.Type) ? paramRecherche.Type.Replace("'", "''") : string.Empty;
                param[2] = new EacParameter("P_ALIMENT", DbType.AnsiStringFixedLength);
                param[2].Value = !string.IsNullOrEmpty(paramRecherche.NumAliment) && (Int32.TryParse(paramRecherche.NumAliment, out numAlm)) ? paramRecherche.NumAliment : string.Empty;
                param[3] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
                param[3].Value = !string.IsNullOrEmpty(paramRecherche.Branche) ? paramRecherche.Branche.Replace("'", "''") : string.Empty;
                param[4] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
                param[4].Value = !string.IsNullOrEmpty(paramRecherche.Cible) ? paramRecherche.Cible.Replace("'", "''") : string.Empty;
                param[5] = new EacParameter("P_CABINETCOURTAGE_ID", DbType.Int32);
                param[5].Value = paramRecherche.CabinetCourtageId;
                param[6] = new EacParameter("P_CABINETCOURTAGE_NOM", DbType.AnsiStringFixedLength);
                param[6].Value = !string.IsNullOrEmpty(paramRecherche.CabinetCourtageNom) ? paramRecherche.CabinetCourtageNom.Replace("'", "''") : string.Empty;
                param[7] = new EacParameter("P_CABINETCOURTAGE_ISAPPORTEUR", DbType.Int32);
                param[7].Value = paramRecherche.CabinetCourtageIsApporteur ? 1 : 0;
                param[8] = new EacParameter("P_CABINETCOURTAGE_ISGESTIONNAIRE", DbType.Int32);
                param[8].Value = paramRecherche.CabinetCourtageIsGestionnaire ? 1 : 0;
                param[9] = new EacParameter("P_PRENEURASSURANCE_CODE", DbType.Int32);
                param[9].Value = paramRecherche.PreneurAssuranceCode;
                param[10] = new EacParameter("P_PRENEURASSURANCE_SIREN", DbType.Int32);
                param[10].Value = paramRecherche.PreneurAssuranceSIREN;
                param[11] = new EacParameter("P_PRENEURASSURANCE_NOM", DbType.AnsiStringFixedLength);
                param[11].Value = !string.IsNullOrEmpty(paramRecherche.PreneurAssuranceNom) ? paramRecherche.PreneurAssuranceNom.Replace("'", "''") : string.Empty;
                param[12] = new EacParameter("P_PRENEURASSURANCE_CP", string.Empty);
                param[12].Value = (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5) && Int32.TryParse(paramRecherche.PreneurAssuranceCP.Substring(2, 3), out cpCode)) ? paramRecherche.PreneurAssuranceCP.Substring(2, 3) : string.Empty;
                param[13] = new EacParameter("P_PRENEURASSURANCE_DEP", string.Empty);
                param[13].Value = (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5) && Int32.TryParse(paramRecherche.PreneurAssuranceCP.Substring(2, 3), out cpCode)) ? paramRecherche.PreneurAssuranceCP.Substring(0, 2) : string.Empty;
                param[14] = new EacParameter("P_PRENEURASSURANCE_DEPONLY", DbType.AnsiStringFixedLength);
                param[14].Value = !string.IsNullOrEmpty(paramRecherche.PreneurAssuranceDEP) ? paramRecherche.PreneurAssuranceDEP : (!String.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && Int32.TryParse(paramRecherche.PreneurAssuranceCP, out cpCode)) ? paramRecherche.PreneurAssuranceCP : string.Empty;
                param[15] = new EacParameter("P_PRENEURASSURANCE_VILLE", DbType.AnsiStringFixedLength);
                param[15].Value = !string.IsNullOrEmpty(paramRecherche.PreneurAssuranceVille) ? paramRecherche.PreneurAssuranceVille.Replace("'", "''") : string.Empty;
                param[16] = new EacParameter("P_ADRESSEOFFRE_CP", string.Empty);
                param[16].Value = (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5) && Int32.TryParse(paramRecherche.AdresseRisqueCP, out cpCode)) ? paramRecherche.AdresseRisqueCP.Substring(2, 3) : string.Empty;
                param[17] = new EacParameter("P_ADRESSEOFFRE_DEP", string.Empty);
                param[17].Value = (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5) && Int32.TryParse(paramRecherche.AdresseRisqueCP, out cpCode)) ? paramRecherche.AdresseRisqueCP.Substring(0, 2) : (!String.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && Int32.TryParse(paramRecherche.AdresseRisqueCP, out cpCode)) ? paramRecherche.AdresseRisqueCP : string.Empty;
                param[18] = new EacParameter("P_ADRESSEOFFRE_VILLE", DbType.AnsiStringFixedLength);
                param[18].Value = !string.IsNullOrEmpty(paramRecherche.AdresseRisqueVille) ? paramRecherche.AdresseRisqueVille.Replace("'", "''") : string.Empty;
                param[19] = new EacParameter("P_SOUSCRIPTEUR_CODE", DbType.AnsiStringFixedLength);
                param[19].Value = !string.IsNullOrEmpty(paramRecherche.SouscripteurCode) ? paramRecherche.SouscripteurCode.Replace("'", "''") : string.Empty;
                param[20] = new EacParameter("P_SOUSCRIPTEUR_NOM", DbType.AnsiStringFixedLength);
                param[20].Value = !string.IsNullOrEmpty(paramRecherche.SouscripteurNom) ? paramRecherche.SouscripteurNom.Replace("'", "''") : string.Empty;
                param[21] = new EacParameter("P_GESTIONNAIRE_CODE", DbType.AnsiStringFixedLength);
                param[21].Value = !string.IsNullOrEmpty(paramRecherche.GestionnaireCode) ? paramRecherche.GestionnaireCode.Replace("'", "''") : string.Empty;
                param[22] = new EacParameter("P_GESTIONNAIRE_NOM", DbType.AnsiStringFixedLength);
                param[22].Value = !string.IsNullOrEmpty(paramRecherche.GestionnaireNom) ? paramRecherche.GestionnaireNom.Trim().Replace("'", "''") : string.Empty;
                param[23] = new EacParameter("P_ETAT", DbType.AnsiStringFixedLength);
                param[23].Value = !string.IsNullOrEmpty(paramRecherche.Etat) ? paramRecherche.Etat.Replace("'", "''") : string.Empty;
                param[24] = new EacParameter("P_ETAT_SAUF", DbType.Int32);
                param[24].Value = paramRecherche.SaufEtat ? 1 : 0;
                param[25] = new EacParameter("P_SITUATION", DbType.AnsiStringFixedLength);
                param[25].Value = !string.IsNullOrEmpty(paramRecherche.Situation) ? paramRecherche.Situation.Replace("'", "''") : string.Empty;
                param[26] = new EacParameter("P_SITUATION_ISACTIF", DbType.Int32);
                param[26].Value = paramRecherche.IsActif ? 1 : 0;
                param[27] = new EacParameter("P_SITUATION_ISINACTIF", DbType.Int32);
                param[27].Value = paramRecherche.IsInactif ? 1 : 0;
                param[28] = new EacParameter("P_TYPE_DATE_RECHERCHE", DbType.AnsiStringFixedLength);
                param[28].Value = !string.IsNullOrEmpty(typeDateRecherche) ? typeDateRecherche.Replace("'", "''") : string.Empty;
                param[29] = new EacParameter("P_DATEDEBUT", DbType.Int32);
                param[29].Value = AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut).HasValue ? AlbConvert.ConvertDateToInt(paramRecherche.DDateDebut).Value : 0;
                param[30] = new EacParameter("P_DATEFIN", DbType.Int32);
                param[30].Value = AlbConvert.ConvertDateToInt(paramRecherche.DDateFin).HasValue ? AlbConvert.ConvertDateToInt(paramRecherche.DDateFin).Value : 0;
                param[31] = new EacParameter("P_MOTS_CLES_LOWER", DbType.AnsiStringFixedLength);
                param[31].Value = !string.IsNullOrEmpty(paramRecherche.MotsClefs) ? paramRecherche.MotsClefs.ToLowerInvariant().Replace("'", "''") : string.Empty;
                param[32] = new EacParameter("P_MOTS_CLES_UPPER", DbType.AnsiStringFixedLength);
                param[32].Value = !string.IsNullOrEmpty(paramRecherche.MotsClefs) ? paramRecherche.MotsClefs.ToUpperInvariant().Replace("'", "''") : string.Empty;
                param[33] = new EacParameter("P_SORTINGBY", DbType.AnsiStringFixedLength);
                param[33].Value = GetRechercheOrderByClause(paramRecherche.SortingName, paramRecherche.SortingOrder);
                param[34] = new EacParameter("P_ISTEMPLATE", DbType.Int32);
                param[34].Value = paramRecherche.IsTemplate ? 1 : 0;
                param[35] = new EacParameter("P_LINECOUNT", DbType.Int32);
                param[35].Value = paramRecherche.LineCount;
                param[36] = new EacParameter("P_STARTLINE", DbType.Int32);
                param[36].Value = paramRecherche.StartLine;
                param[37] = new EacParameter("P_ENDLINE", DbType.Int32);
                param[37].Value = paramRecherche.EndLine;
                param[38] = new EacParameter("P_TYPECONTRAT", DbType.AnsiStringFixedLength);
                param[38].Value = paramRecherche.TypeContrat.Replace("'", "''");
                param[39] = new EacParameter("P_COUNT", DbType.Int32);
                param[39].Direction = ParameterDirection.InputOutput;
                param[39].DbType = DbType.Int64;
                param[39].Value = 0;

                #region pour debug de la requete

                //param[40] = new EacParameter("P_REQUEST_OUT", "");
                //param[40].Size = 8000;
                //param[40].Direction = ParameterDirection.InputOutput;

                #endregion

                #endregion

                DataTable dataTable = new DataTable();
                var resultQuery = new List<OffreRechPlatDto>();
                if (modeNavig == ModeConsultation.Historique)
                {
                    resultQuery = DbBase.Settings.ExecuteList<OffreRechPlatDto>(CommandType.StoredProcedure, "SP_RECHERCHEGENERALEHIST", param);
                    //dataTable = DbBase.Settings.ExecuteDataTable(CommandType.StoredProcedure, "SP_RECHERCHEGENERALEHIST", param);
                    //foreach (DataRow ligne in dataTable.Rows)
                    //{
                    //    OffreDto offre = Initialiser(ligne, true);
                    //    result.LstOffres.Add(offre);
                    //}
                }
                else
                {
                    resultQuery = DbBase.Settings.ExecuteList<OffreRechPlatDto>(CommandType.StoredProcedure, "SP_RECHERCHEGENERALE", param);
                    //dataTable = DbBase.Settings.ExecuteDataTable(CommandType.StoredProcedure, "SP_RECHERCHEGENERALE", param);
                }

                if (resultQuery != null && resultQuery.Any())
                {
                    resultQuery.ForEach(el =>
                    {
                        int cpAssur = 0;
                        int cpCourt = 0;

                        int.TryParse(el.CpAss, out cpAssur);
                        int.TryParse(el.CpCourt, out cpCourt);


                        result.LstOffres.Add(new OffreDto
                        {
                            CodeOffre = el.CodeOffre,
                            Version = el.Version,
                            Type = el.Type,
                            NumAvenant = el.CodeAvn,
                            DateSaisie = AlbConvert.ConvertIntToDateHour(el.DateSaisie),
                            Branche = new BrancheDto { Code = el.CodeBranche, Nom = el.LibBranche, Cible = new CibleDto { Code = el.CodeCible, Nom = el.LibCible } },
                            Etat = el.CodeEtat,
                            EtatLib = el.LibEtat,
                            Situation = el.CodeSit,
                            SituationLib = el.LibSit,
                            Qualite = el.CodeQualite,
                            QualiteLib = el.LibQualite,
                            Descriptif = el.Descriptif,
                            PreneurAssurance = new AssureDto
                            {
                                Code = el.CodeAss.ToString(),
                                NomAssure = el.NomAss,
                                Adresse = new AdressePlatDto
                                {
                                    // CodePostal = cpAssur,
                                    NomVille = el.VilleAss,
                                    CodePostalString = el.CpAss
                                }
                            },
                            CabinetGestionnaire = new CabinetCourtageDto
                            {
                                Code = el.CodeCourt,
                                NomCabinet = el.NomCourt,
                                Adresse = new AdressePlatDto
                                {
                                    //CodePostal = cpCourt,
                                    NomVille = el.VilleCourt,
                                    CodePostalString = el.CpCourt
                                }
                            },
                            TypeAvt = el.TypeTraitement,
                            TypeAccord = el.TypeAccord,
                            KheopsStatut = el.StatutKheops,
                            NumAvnExterne = el.AvnExt,
                            GenerDoc = Convert.ToInt32(el.GenerDoc),
                            MotifRefus = el.MotifRefus.Trim() != "-" ? el.MotifRefus : string.Empty,
                            Periodicite = new ParametreDto { Code = el.CodePeriodicite },
                            ContratMere = el.TypePolice,
                            DateFinEffetGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateFinEffet)),
                            HeureFin = AlbConvert.ConvertIntToTimeMinute(el.HeureFinEffet),
                            HasSusp = el.HasSusp > 0 ? true : false
                        });
                    });
                }


                result.NbCount = Convert.ToInt32(param[39].Value.ToString());
            }
            return result;
        }

        public static List<RisqueDto> ObtenirSpecificRisque(ModeConsultation modeNavig, string offreId, int numRsq, int? offreVersion = null, string type = "O", string codeAvn = "")
        {
            List<RisqueDto> toReturn = new List<RisqueDto>();
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"
SELECT TBLRISQUE.JERSQ CODE, KABDESC DESCRIPTIF, KABCIBLE CODECIBLE, CB.KAHDESC LIBELLECIBLE, KABDESI CHRONODESI, KADDESI LIBELLERISQUE,
    TBLRISQUE.JEVDJ ENTREEGARANTIEJOUR, TBLRISQUE.JEVDM ENTREEGARANTIEMOIS, TBLRISQUE.JEVDA ENTREEGARANTIEANNEE, TBLRISQUE.JEVDH ENTREEGARANTIEHEURE,
    TBLRISQUE.JEVFJ SORTIEGARANTIEJOUR, TBLRISQUE.JEVFM SORTIEGARANTIEMOIS,TBLRISQUE.JEVFA SORTIEGARANTIEANNEE, TBLRISQUE.JEVFH SORTIEGARANTIEHEURE,
    TBLRISQUEHISTO.JEVFJ SORTIEGARANTIEJOURHISTO, TBLRISQUEHISTO.JEVFM SORTIEGARANTIEMOISHISTO,TBLRISQUEHISTO.JEVFA SORTIEGARANTIEANNEEHISTO, TBLRISQUEHISTO.JEVFH SORTIEGARANTIEHEUREHISTO,
    TBLRISQUE.JEVAL VALEUR, TBLRISQUE.JEVAU CODEUNITE, TBLRISQUE.JEVAT CODETYPE, TBLRISQUE.JEVAH VALEURHT, TBLRISQUE.JEOBJ CODEOBJET, TBLRISQUE.JEINA ISRISQUEINDEXE,
    TBLRISQUE.JEIXL ISLCI, TBLRISQUE.JEIXF ISFRANCHISE, TBLRISQUE.JEIXC ISASSIETTE, TBLRISQUE.JERGT REGIMETAXE, TBLRISQUE.JECNA ISCATNAT, JFADH IDADRESSERISQUE, KABAPE CODEAPE, 
    KABNMC01 NOMENCLATURE1,KABNMC02 NOMENCLATURE2,KABNMC03 NOMENCLATURE3,KABNMC04 NOMENCLATURE4,KABNMC05 NOMENCLATURE5,  
    TBLRISQUE.JETRR TERRITORIALITE, KABTRE CODETRE, TPLIB LIBTRE, KABCLASS CODECLASSE, KABMAND DATEDEBDESC, KABMANF DATEFINDESC,
    KABMANDH HEUREDEBDESC, KABMANFH HEUREFINDESC,
    TBLRISQUE.JEAVJ EFFETAVNJOUR, TBLRISQUE.JEAVM EFFETAVNMOIS, TBLRISQUE.JEAVA EFFETAVNANNEE,
    TBLRISQUE.JEAVE AVNCREATION, TBLRISQUE.JEAVF AVNMODIF,
    JDINA INDEXOFFRE,
    TBLRISQUE.JEPBT TAUXAPPEL, TBLRISQUE.JERUL ISREGUL, TBLRISQUE.JERUT TYPEREGUL,
    TBLRISQUE.JEPBN PARTBENEFRSQ, TBLRISQUE.JEPBN PARTBENEF, TBLRISQUE.JEPBA NBYEAR, TBLRISQUE.JEPBR RISTOURNE, TBLRISQUE.JEPBP COTISRET, TBLRISQUE.JEPBS SEUIL, TBLRISQUE.JEPBC TAUXCOMP,
    TBLRISQUE.JETEM ISRISQUETEMP,
    JDPEA ECHANNEE, JDPEM ECHMOIS, JDPEJ ECHJOUR
FROM {0} 
    LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
    LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
    LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
    LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
    LEFT JOIN {3} ON KABDESI = KADCHR
    LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
    LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10} AND KABRSQ = {16}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1, numRsq);

            var listRisquePlatDto = DbBase.Settings.ExecuteList<RisquePlatDto>(CommandType.Text, sql);
            if (listRisquePlatDto != null && listRisquePlatDto.Any())
            {
                foreach (var risquePlatDto in listRisquePlatDto)
                {
                    RisqueDto risque = GetRisque(modeNavig, risquePlatDto, offreId, type, offreVersion, codeAvn);
                    if (risque.IdAdresseRisque > 0)
                    {
                        risque.AdresseRisque = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
                        //risque.Objets = ObtenirObjet(offreId, risque.Code, type, offreVersion);
                    }
                    toReturn.Add(risque);
                }
            }
            return toReturn;
        }
        /// <summary>
        /// Information risque avec les adresses et la liste des objets risque 
        /// </summary>
        /// <param name="modeNavig"></param>
        /// <param name="offreId"></param>
        /// <param name="numRsq"></param>
        /// <param name="offreVersion"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <returns></returns>
        public static RisqueDto ObtenirRisque(ModeConsultation modeNavig, string offreId, int numRsq, int? offreVersion = null, string type = "O", string codeAvn = "")
        {
            RisqueDto risque = null;
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sql = string.Format(@"
SELECT TBLRISQUE.JERSQ CODE, KABDESC DESCRIPTIF, KABCIBLE CODECIBLE , PBBRA CODEBRANCHE, CB.KAHDESC LIBELLECIBLE, KABDESI CHRONODESI, KADDESI LIBELLERISQUE,
    TBLRISQUE.JEVDJ ENTREEGARANTIEJOUR, TBLRISQUE.JEVDM ENTREEGARANTIEMOIS, TBLRISQUE.JEVDA ENTREEGARANTIEANNEE, TBLRISQUE.JEVDH ENTREEGARANTIEHEURE,
    TBLRISQUE.JEVFJ SORTIEGARANTIEJOUR, TBLRISQUE.JEVFM SORTIEGARANTIEMOIS,TBLRISQUE.JEVFA SORTIEGARANTIEANNEE, TBLRISQUE.JEVFH SORTIEGARANTIEHEURE,
    TBLRISQUEHISTO.JEVFJ SORTIEGARANTIEJOURHISTO, TBLRISQUEHISTO.JEVFM SORTIEGARANTIEMOISHISTO,TBLRISQUEHISTO.JEVFA SORTIEGARANTIEANNEEHISTO, TBLRISQUEHISTO.JEVFH SORTIEGARANTIEHEUREHISTO,
    TBLRISQUE.JEVAL VALEUR, TBLRISQUE.JEVAU CODEUNITE, TBLRISQUE.JEVAT CODETYPE, TBLRISQUE.JEVAH VALEURHT, TBLRISQUE.JEOBJ CODEOBJET, TBLRISQUE.JEINA ISRISQUEINDEXE,
    TBLRISQUE.JEIXL ISLCI, TBLRISQUE.JEIXF ISFRANCHISE, TBLRISQUE.JEIXC ISASSIETTE, TBLRISQUE.JERGT REGIMETAXE, TBLRISQUE.JECNA ISCATNAT, JFADH IDADRESSERISQUE, KABAPE CODEAPE, 
    KABNMC01 NOMENCLATURE1,KABNMC02 NOMENCLATURE2,KABNMC03 NOMENCLATURE3,KABNMC04 NOMENCLATURE4,KABNMC05 NOMENCLATURE5,
    TBLRISQUE.JETRR TERRITORIALITE, KABTRE CODETRE, TPLIB LIBTRE, KABCLASS CODECLASSE, KABMAND DATEDEBDESC, KABMANF DATEFINDESC,
    KABMANDH HEUREDEBDESC, KABMANFH HEUREFINDESC,
    TBLRISQUE.JEAVJ EFFETAVNJOUR, TBLRISQUE.JEAVM EFFETAVNMOIS, TBLRISQUE.JEAVA EFFETAVNANNEE,
    TBLRISQUE.JEAVE AVNCREATION, TBLRISQUE.JEAVF AVNMODIF,
    JDINA INDEXOFFRE,
    TBLRISQUE.JEPBT TAUXAPPEL, TBLRISQUE.JERUL ISREGUL, TBLRISQUE.JERUT TYPEREGUL,
    TBLRISQUE.JEPBN PARTBENEFRSQ, TBLRISQUE.JEPBN PARTBENEF, TBLRISQUE.JEPBA NBYEAR, TBLRISQUE.JEPBR RISTOURNE, TBLRISQUE.JEPBP COTISRET, TBLRISQUE.JEPBS SEUIL, TBLRISQUE.JEPBC TAUXCOMP,
    TBLRISQUE.JETEM ISRISQUETEMP,
    JDPEA ECHANNEE, JDPEM ECHMOIS, JDPEJ ECHJOUR
FROM {0} 
    LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
    LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
    LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
    LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
    LEFT JOIN {3} ON KABDESI = KADCHR
    LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
    LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
    LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10} AND KABRSQ = {16}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1, numRsq);
            var risquePlatDto = DbBase.Settings.ExecuteList<RisquePlatDto>(CommandType.Text, sql).FirstOrDefault();
            if (risquePlatDto != null)
            {
                risque = GetRisque(modeNavig, risquePlatDto, offreId, type, offreVersion, codeAvn);
                if (string.IsNullOrEmpty(risque?.Cible?.CodeBranche))
                {
                    risque.Cible.CodeBranche = risquePlatDto.CodeBranche;
                }
                if (risque.IdAdresseRisque > 0)
                {
                    risque.AdresseRisque = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
                    risque.Objets = ObtenirObjet(modeNavig, offreId, risque.Code, type, offreVersion);
                }
            }
            return risque;
        }

        public static int CopierRisque(string offreId, int numRsq, string CBnsPb, string user, int? offreVersion = 0, string type = "O")
        {
            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = offreId.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = offreVersion.Value;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
            param[3].Value = numRsq;
            param[4] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[4].Value = user;
            param[5] = new EacParameter("P_DATESYSTEM", DbType.Int32);
            param[5].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[6] = new EacParameter("P_BNSPB", DbType.AnsiStringFixedLength);
            param[6].Value = CBnsPb;
            param[7] = new EacParameter("P_NEWCODERSQ", DbType.Int32);
            param[7].Value = 0;
            param[7].DbType = DbType.Int32;
            param[7].Direction = ParameterDirection.InputOutput;


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COPYRSQ", param);

            return (int)param[7].Value;
        }

        public static Int64 GetCountRSQ(ModeConsultation modeNavig, string offreId, int numRsq, int? offreVersion = null, string type = "O", string codeAvn = "")
        {
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            string sqlCount = string.Format(@"SELECT COUNT(*) NBLIGN 
                                         FROM {0} 
                                                LEFT JOIN {9} ON JDIPB = PBIPB AND JDALX = PBALX {11}
                                                LEFT JOIN {1} TBLRISQUE ON PBIPB = TBLRISQUE.JEIPB AND PBALX = TBLRISQUE.JEALX {12}
                                                LEFT JOIN YHRTRSQ TBLRISQUEHISTO ON TBLRISQUE.JEIPB = TBLRISQUEHISTO.JEIPB AND TBLRISQUE.JEALX = TBLRISQUEHISTO.JEALX AND TBLRISQUE.JERSQ = TBLRISQUEHISTO.JERSQ AND TBLRISQUEHISTO.JEAVN = {15}
                                                LEFT JOIN {2} ON TBLRISQUE.JEIPB = KABIPB AND TBLRISQUE.JEALX = KABALX AND TBLRISQUE.JERSQ = KABRSQ {13}
                                                LEFT JOIN {3} ON KABDESI = KADCHR
                                                LEFT JOIN KCIBLE CB ON KABCIBLE = CB.KAHCIBLE
                                                LEFT JOIN {4} ON JFIPB = PBIPB AND PBALX = JFALX AND JFRSQ = TBLRISQUE.JERSQ AND JFOBJ = 0 {14} 
                                                LEFT JOIN  YYYYPAR ON TCON=CB.KAHCON and TFAM = CB.KAHFAM and TCOD= KABTRE                                            
                                         WHERE {8}='{5}' AND PBIPB='{6}' AND PBALX='{7}' {10}",
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTADR"),
                  type, offreId.PadLeft(9, ' '), offreVersion.HasValue ? offreVersion.Value : 0,
                  modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                  CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                  modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0") : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JDAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = PBAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND TBLRISQUE.JEAVN = KABAVN" : string.Empty,
                  modeNavig == ModeConsultation.Historique ? " AND JFAVN = PBAVN" : string.Empty,
                  iCodeAvn - 1);

            Int64 nbRow = CommonRepository.RowNumber(sqlCount);
            return nbRow;
        }
        public static DetailsRisqueGetResultDto GetInfoDetailRsq(string codeOffre, string version, string type, string numRsq, string numObj, ModeConsultation modeNavig, string codeAvn, bool isAdmin, string codeBranche, string codeCible, bool isUserHorse)
        {
            var toReturn = new DetailsRisqueGetResultDto();

            var param = new EacParameter[4];
            param[0] = new EacParameter("numRsq", DbType.AnsiStringFixedLength);
            param[0].Value = numRsq;
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("version", DbType.Int32);
            param[2].Value = Convert.ToInt32(version);
            param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[3].Value = type;

            toReturn.Offre = new InfosBaseDto();
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = -1 };
            Parallel.Invoke(parallelOptions,
                () =>
                {
                    //GET RSQ
                    var rsq = ObtenirSpecificRisque(modeNavig, codeOffre, int.Parse(numRsq), int.Parse(version), type, codeAvn);

                    toReturn.Offre.Risques = rsq;
                    if (rsq != null && rsq.Any())
                    {
                        toReturn.EchAnnee = rsq.FirstOrDefault().EchAnnee;
                        toReturn.EchMois = rsq.FirstOrDefault().EchMois;
                        toReturn.EchJour = rsq.FirstOrDefault().EchJour;
                    }
                },
                () =>
                {
                    // GET COUNTRSQ
                    toReturn.Offre.CountRsq = GetCountRSQ(modeNavig, codeOffre, int.Parse(numRsq), int.Parse(version), type, codeAvn);
                },
                () =>
                {
                    toReturn.HasFormules = RisqueRepository.RisquesHasFormules(codeOffre, Convert.ToInt32(version), type, codeAvn, Convert.ToInt32(numRsq), modeNavig);
                    toReturn.Cibles = BrancheRepository.ObtenirCibles(codeBranche, isAdmin, isUserHorse);
                    toReturn.Unites = CommonRepository.GetParametres(codeBranche, codeCible, "PRODU", "QCVAU");
                },
                () =>
                {
                    //toReturn.Unites = ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirUniteValeur());
                    toReturn.Types = CommonRepository.GetParametres(codeBranche, codeCible, "PRODU", "QCVAT");
                    //toReturn.Types = ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirTypeValeur());
                    toReturn.TypesInventaire = ReferenceRepository.ObtenirTypeInventaire(codeBranche, codeCible);
                    toReturn.RegimesTaxe = CommonRepository.GetParametres(codeBranche, codeCible, "GENER", "TAXRG");
                },
                () =>
                {
                    //toReturn.RegimesTaxe = ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirRegimeTaxe());
                    //Nomenclature de risques
                    toReturn.CodesApe = new List<ParametreDto>();
                    toReturn.CodesTre = CommonRepository.GetParametres(codeBranche, codeCible, "KHEOP", "TREAC");
                    toReturn.Territorialites = CommonRepository.GetParametres(codeBranche, codeCible, "PRODU", "QATRR");
                },
                () =>
                {

                    //Récupération de l'ensemble des combos
                    var resultCombo = RisqueRepository.GetComboNomenclatures(codeOffre, Convert.ToInt32(version), type, Convert.ToInt32(numRsq), Convert.ToInt32(numObj), codeCible);
                    if (resultCombo != null && resultCombo.Count > 0)
                    {
                        toReturn.Nomenclatures1 = resultCombo.FindAll(elm => elm.NumeroCombo == 1);
                        toReturn.Nomenclatures2 = resultCombo.FindAll(elm => elm.NumeroCombo == 2);
                        toReturn.Nomenclatures3 = resultCombo.FindAll(elm => elm.NumeroCombo == 3);
                        toReturn.Nomenclatures4 = resultCombo.FindAll(elm => elm.NumeroCombo == 4);
                        toReturn.Nomenclatures5 = resultCombo.FindAll(elm => elm.NumeroCombo == 5);

                        if (toReturn.Nomenclatures1 != null)
                            toReturn.Nomenclatures1 = toReturn.Nomenclatures1.OrderBy(elm => elm.OrdreNomenclature).ToList();
                        if (toReturn.Nomenclatures2 != null)
                            toReturn.Nomenclatures2 = toReturn.Nomenclatures2.OrderBy(elm => elm.OrdreNomenclature).ToList();
                        if (toReturn.Nomenclatures3 != null)
                            toReturn.Nomenclatures3 = toReturn.Nomenclatures3.OrderBy(elm => elm.OrdreNomenclature).ToList();
                        if (toReturn.Nomenclatures4 != null)
                            toReturn.Nomenclatures4 = toReturn.Nomenclatures4.OrderBy(elm => elm.OrdreNomenclature).ToList();
                        if (toReturn.Nomenclatures5 != null)
                            toReturn.Nomenclatures5 = toReturn.Nomenclatures5.OrderBy(elm => elm.OrdreNomenclature).ToList();
                    }
                    else
                    {
                        toReturn.Nomenclatures1 = new List<NomenclatureDto>();
                        toReturn.Nomenclatures2 = new List<NomenclatureDto>();
                        toReturn.Nomenclatures3 = new List<NomenclatureDto>();
                        toReturn.Nomenclatures4 = new List<NomenclatureDto>();
                        toReturn.Nomenclatures5 = new List<NomenclatureDto>();
                    }
                },
                () =>
                {

                    toReturn.CodesClasse = new List<ParametreDto>();
                    toReturn.TypesRisque = CommonRepository.GetParametres(codeBranche, codeCible, "KHEOP", "RISRS");
                    toReturn.TypesMateriel = CommonRepository.GetParametres(codeBranche, codeCible, "KHEOP", "MATRS");
                },
                () =>
                {
                    toReturn.NaturesLieux = CommonRepository.GetParametres(codeBranche, codeCible, "ALSPK", "NLOC");
                    toReturn.IsExistLoupe = PoliceRepository.ChekConceptFamille(codeCible);

                    toReturn.DateDebHisto = PoliceRepository.GetDateDebRsqHisto(codeOffre, version.ToString(), Convert.ToInt32(numRsq), codeAvn);
                    //if (rsq != null && rsq.Any())
                    //{
                    //    toReturn.EchAnnee = rsq.FirstOrDefault().EchAnnee;
                    //    toReturn.EchMois = rsq.FirstOrDefault().EchMois;
                    //    toReturn.EchJour = rsq.FirstOrDefault().EchJour;
                    //}

                    if (numRsq != "0" && type == "P")
                    {
                        param = new EacParameter[3];
                        param[0] = new EacParameter("numRsq", DbType.AnsiStringFixedLength);
                        param[0].Value = numRsq;
                        param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                        param[1].Value = codeOffre.PadLeft(9, ' ');
                        param[2] = new EacParameter("version", DbType.Int32);
                        param[2].Value = Convert.ToInt32(version);

                        string sql = @"SELECT JEAVA DATMODYEAR, JEAVM DATMODMONTH, JEAVJ DATMODDAY FROM YPRTRSQ WHERE JERSQ = :numRsq AND JEIPB = :codeOffre AND JEALX = :version";

                        var res = DbBase.Settings.ExecuteList<DetailsRisqueGetResultDto>(CommandType.Text, sql, param).FirstOrDefault();
                        if (res != null)
                        {
                            if (res.DateModifAnnee != 0 && res.DateModifMois != 0 && res.DateModifJour != 0)
                                toReturn.DateModifRsqAvn = new DateTime(res.DateModifAnnee, res.DateModifMois, res.DateModifJour);
                            else
                                toReturn.DateModifRsqAvn = null;
                        }
                    }
                    else
                        toReturn.DateModifRsqAvn = null;
                });


            return toReturn;
        }

        public static OffreDto GetInfoCreationSaisie(string codeOffre, string version, string type)
        {
            var toReturn = new OffreDto();

            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            var sql = $@"SELECT PBIPB CODEOFFRE, PBALX VERSIONOFFRE, PBTYP TYPEOFFRE, PBBRA CODEBRANCHE,
        KAACIBLE CODECIBLE, KAHDESC NOMCIBLE,
        PBSOU CODESOUSCRIPTEUR, UT1.UTNOM NOMSOUSCRIPTEUR,
        PBGES CODEGESTIONNAIRE, UT2.UTNOM NOMGESTIONNAIRE,
        (PBSAA * 10000 + PBSAM * 100 + PBSAJ) DATESAISIE, PBSAH DATESAISIEHEURE,
        PBICT CODECABINETCOURTAGE,
        PBIAS CODEASSURE, KAAASS PRENEURESTASSURE,
        PBMO1 CODEMOTSCLEF1, PBMO2 CODEMOTSCLEF2, PBMO3 CODEMOTSCLEF3,
        PBREF DESCRIPTIF, KAJOBSV OBSERVATION, PBOCT REFCOURTIER,
    PBIN5 CODEINTERLOCUTEUR,
    ABPLG3 BATIMENT, ABPNUM NUMEROVOIE, ABPLBN NUMEROVOIE2, ABPEXT EXTENSIONVOIE, ABPLG4 NOMVOIE, ABPLG5 BOITEPOSTALE, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL,  
                        ABPVI6 NOMVILLE, ABPPAY CODEPAYS, ABPCDX TYPECEDEX, ABPMAT MATHEX, CPAYS.TPLIB NOMPAYS, PBADH NUMEROCHRONO, LATITUDE, LONGITUDE
FROM YPOBASE
    LEFT JOIN YADRESS ON PBADH = YADRESS.ABPCHR 
    LEFT JOIN KGEOLOC ON PBADH = KGEOLOC.{AdresseRepository.GeolocId} 
        LEFT JOIN KPENT ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP
        LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
        LEFT JOIN YUTILIS UT1 ON UT1.UTIUT = PBSOU
        LEFT JOIN YUTILIS UT2 ON UT2.UTIUT = PBGES
        LEFT JOIN KPOBSV ON KAAOBSV = KAJCHR
    LEFT JOIN YYYYPAR CPAYS ON CPAYS.TCON = 'GENER' AND CPAYS.TFAM = 'CPAYS' AND CPAYS.TCOD = ABPPAY
WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<OffrePlatDto>(CommandType.Text, sql, param);
            if (result == null || !result.Any()) return toReturn;

            var firstRes = result.FirstOrDefault();
            if (firstRes != null)
            {
                toReturn = new OffreDto
                {
                    CodeOffre = firstRes.CodeOffre,
                    Version = firstRes.VersionOffre,
                    Type = firstRes.TypeOffre,
                    Descriptif = firstRes.Descriptif,
                    Branche = new BrancheDto { Code = firstRes.CodeBranche, Cible = new CibleDto { Code = firstRes.CodeCible, Nom = firstRes.NomCible } },
                    MotCle1 = firstRes.CodeMotsClef1,
                    MotCle2 = firstRes.CodeMotsClef2,
                    MotCle3 = firstRes.CodeMotsClef3,
                    Observation = firstRes.Observation,
                    Souscripteur = new SouscripteurDto { Code = firstRes.CodeSouscripteur, Nom = firstRes.NomSouscripteur },
                    Gestionnaire = new GestionnaireDto { Id = firstRes.CodeGestionnaire, Nom = firstRes.NomGestionnaire },
                    DateSaisie = AlbConvert.ConvertIntToDateHour(firstRes.DateSaisie * 10000 + firstRes.DateSaisieHeure),
                    CabinetGestionnaire = CabinetCourtageRepository.Obtenir(firstRes.CodeCabinetCourtage),
                    PreneurAssurance = AssureRepository.Obtenir(firstRes.CodeAssure),
                    RefChezCourtier = firstRes.RefCourtier,

                    Interlocuteur = new InterlocuteurDto { Id = firstRes.CodeInterlocuteur },
                    AdresseOffre = GetAdresse(firstRes),
                    CodeInterlocuteur = firstRes.CodeInterlocuteur.ToString()
                };

                if (toReturn.PreneurAssurance != null)
                    toReturn.PreneurAssurance.PreneurEstAssure = firstRes.PreneurEstAssure.Equals("O", StringComparison.InvariantCulture);
            }

            toReturn.NomInterlocuteur = toReturn.Interlocuteur.Id != 0 ? InterlocuteurRepository.RechercherNomInterlocuteur(Convert.ToInt32(toReturn.CodeInterlocuteur), toReturn.CabinetGestionnaire.Code) : string.Empty;
            if (string.IsNullOrEmpty(toReturn.NomInterlocuteur)) toReturn.CodeInterlocuteur = "0";

            if (toReturn.CabinetGestionnaire != null)
            {
                toReturn.CabinetGestionnaire.Delegation = DelegationRepository.Obtenir(toReturn.CabinetGestionnaire.Code);
                if (toReturn.Interlocuteur != null)
                    toReturn.Interlocuteur.CabinetCourtage = toReturn.CabinetGestionnaire;

            }
            if (toReturn.PreneurAssurance != null)
                toReturn.PreneurAssurance.PreneurEstAssure = toReturn.PreneurAssurance.PreneurEstAssure;

            toReturn.NbAssuresAdditionnels = GetNbAssuAdd(codeOffre, Convert.ToInt32(version), type);

            return toReturn;
        }



        #region Modfi Hors Avn
        public static bool GetModifHorsAvnIsRegularisable(string codeOffre, int version, string type, int numAvn, string codeRsq)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[1].Value = type;
            param[2] = new EacParameter("version", DbType.Int32);
            param[2].Value = version;
            param[3] = new EacParameter("numAvn", DbType.Int32);
            param[3].Value = numAvn;
            param[4] = new EacParameter("codeRsq", DbType.AnsiStringFixedLength);
            param[4].Value = codeRsq;

            var sql = @"SELECT COUNT(*) NBLIGN
                                      FROM KPVALH 
                                      WHERE KIFIPB = :codeOffre AND KIFTYP  = :type  AND KIFALX = :version AND  KIFAVN = :numAvn AND  KIFPERI  = 'RSQ' AND KIFRSQ = :codeRsq
                                      AND KIFRUL = 'O'";


            return CommonRepository.ExistRowParam(sql, param);
        }

        #endregion

        public static long GetLastRegulId(string codeOffre, int version, string type, int codeAvn)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAvn", DbType.Int32);
            param[3].Value = codeAvn;

            var sql = @"SELECT KHWID ID
                                      FROM KPRGU
                                      WHERE KHWIPB = :codeOffre AND KHWALX = :version  AND KHWTYP = :type AND KHWAVN = :codeAvn";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                var first = result.FirstOrDefault();
                if (first != null)
                    return first.Id;
            }
            return 0;
        }

        public static LTADto GetInfoLTA(string codeAffaire, int version, string type, int avenant, ModeConsultation modeNavig)
        {
            var sql = string.Format(@"SELECT JDLDEB DATEDEB, JDLDEH HEUREDEB, JDLFIN DATEFIN, JDLFIH  HEUREFIN, JDLDUR DUREE, JDLDUU UNITEDUREE, JDLTASP SEUILSP
                            FROM {0}
                        WHERE JDIPB = :codeAffaire AND JDALX = :version {1}",
                        CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                        modeNavig == ModeConsultation.Historique && avenant > 0 ? " AND JDAVN = :avenant" : string.Empty);
            var param = new List<EacParameter>();

            param.Add(new EacParameter("codeAffaire", DbType.AnsiStringFixedLength) { Value = codeAffaire.PadLeft(9, ' ') });
            param.Add(new EacParameter("version", DbType.Int32) { Value = version });
            if (modeNavig == ModeConsultation.Historique && avenant > 0)
                param.Add(new EacParameter("avenant", DbType.Int32) { Value = version });

            var result = DbBase.Settings.ExecuteList<LTADto>(CommandType.Text, sql, param);
            var toReturn = result != null && result.Any() ? result.FirstOrDefault() : new LTADto();
            toReturn.Durees = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBCTU");

            return toReturn;
        }

        public static void SetInfoLTA(string codeAffaire, int version, string type, int avenant, LTADto dto)
        {
            var sql = @"update yprtent set
                            jdldeb = :dateDeb, jdldeh = :heureDeb,
                            jdlfin = :dateFin, jdlfih = :heureFin,
                            jdldur = :duree, jdlduu = :uniteDuree,
                            jdltasp = :seuilSp
                        where jdipb = :codeAffaire and jdalx = :version";

            var param = new List<EacParameter>();
            param.Add(new EacParameter("dateDeb", DbType.Int64) { Value = dto.DateDeb });
            param.Add(new EacParameter("heureDeb", DbType.Int64) { Value = dto.HeureDeb });
            param.Add(new EacParameter("dateFin", DbType.Int64) { Value = dto.DateFin });
            param.Add(new EacParameter("heureFin", DbType.Int64) { Value = dto.HeureFin });
            param.Add(new EacParameter("duree", DbType.Int16) { Value = dto.DureeLTA });
            param.Add(new EacParameter("uniteDuree", DbType.AnsiStringFixedLength) { Value = dto.DureeLTAString });
            param.Add(new EacParameter("seuilSp", DbType.Single) { Value = dto.SeuilLTA });
            param.Add(new EacParameter("codeAffaire", DbType.AnsiStringFixedLength) { Value = codeAffaire.PadLeft(9, ' ') });
            param.Add(new EacParameter("version", DbType.Int32) { Value = version });

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }


        /// <summary>
        /// Wi 3492 - T 3493
        /// Modification "Valeur Totale Risque"
        /// </summary>
        /// <param name="code"></param>
        /// <param name="version"></param>
        /// <param name="riskId"></param>
        /// <param name="numAvn"></param>
        /// <param name="totalValue"></param>
        /// <param name="unite"></param>
        /// <param name="riskType"></param>
        /// <param name="valueHt"></param>
        public static void UpdateRiskTotalValue(string code, int? version, int riskId, long? totalValue, string unite, string riskType, string valueHt)
        {
            try
            {
                EacParameter[] subParam = new EacParameter[7];
                subParam[0] = new EacParameter("valeur", DbType.Int64)
                {
                    Value = totalValue != null ? totalValue : 0
                };
                subParam[1] = new EacParameter("uniteCode", DbType.AnsiStringFixedLength)
                {
                    Value = unite != null ? unite : string.Empty
                };
                subParam[2] = new EacParameter("typeCode", DbType.AnsiStringFixedLength)
                {
                    Value = riskType != null ? riskType : string.Empty
                };
                subParam[3] = new EacParameter("valeurHT", DbType.AnsiStringFixedLength)
                {
                    Value = !string.IsNullOrEmpty(valueHt) ? valueHt : string.Empty
                };
                subParam[4] = new EacParameter("code", DbType.AnsiStringFixedLength)
                {
                    Value = code.ToIPB()
                };
                subParam[5] = new EacParameter("version", DbType.Int32)
                {
                    Value = version
                };
                subParam[6] = new EacParameter("riskId", DbType.Int32)
                {
                    Value = riskId
                };
                var sql = @"UPDATE YPRTRSQ
                        SET JEVAL = :valeur, JEVAA = :valeur, JEVAU = :uniteCode, JEVAT = :typeCode, JEVAH = :valeurHT
                        WHERE JEIPB = :code AND JEALX = :version AND JERSQ = :riskId";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, subParam);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static void ClasserContratsSansSuite(string codeAffaire, int version, string type, string user)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[3].Value = user;
            //param[5] = new EacParameter("P_DATESYSTEM", DbType.Int32);
            //param[5].Value = DateTime.Now.ToString("yyyyMMdd");

            //param[6] = new EacParameter("P_NEWCODERSQ", DbType.Int32);
            //param[6].Value = 0;
            //param[6].DbType = DbType.Int32;
            //param[6].Direction = ParameterDirection.InputOutput;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CLASSCONTSANSSUITE", param);

        }
    }
}

