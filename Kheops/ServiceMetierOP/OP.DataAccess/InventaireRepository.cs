using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System.Data.Common;
using System.Data;
using ALBINGIA.Framework.Common.Data;
using System.Data.EasycomClient;
using OP.WSAS400.DTO.Validation;
using OP.WSAS400.DTO.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common;
using OP.WSAS400.DTO.ControleFin;
using ALBINGIA.Framework.Common.Extensions;
using System.Threading.Tasks;

namespace OP.DataAccess
{
    public class InventaireRepository
    {
        #region Méthodes Publiques

        public static int GetNewInventaireId(string codeOffre, string version, string type, string perimetre, string codeRsq, string codeObj, string codeFor, string codeGaran)
        {
            DbParameter[] param = new DbParameter[8];
            param[0] = new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", type);
            param[3] = new EacParameter("perimetre", perimetre.Trim());
            param[4] = new EacParameter("codeRsq", 0);
            param[4].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;
            param[5] = new EacParameter("codeObj", 0);
            param[5].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[6] = new EacParameter("codeFor", 0);
            param[6].Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0;
            param[7] = new EacParameter("codeGaran", codeGaran.Trim());

            var sql = @"SELECT KBGKBEID INT64RETURNCOL FROM KPINVAPP WHERE KBGIPB = :codeOffre AND KBGALX = :version AND KBGTYP = :type and TRIM(KBGPERI) = :perimetre
                            AND KBGRSQ = :codeRsq AND KBGOBJ = :codeObj AND KBGFOR = :codeFor AND TRIM(KBGGAR) = :codeGaran";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return Convert.ToInt32(result.FirstOrDefault().Int64ReturnCol);
            }

            return 0;
        }


        public static InventaireDto LoadInventaire(string codeOffre, int version, string type, string codeAvn, string ecranProvenance, int codeRisque, int codeObjet, int codeFormule, string codeGarantie, string typeInventaire, Int64 codeInven, string branche, string cible, ModeConsultation modeNavig)
        {

            CreateInventaire(codeOffre, version, type, ecranProvenance, codeRisque, codeObjet, codeFormule, codeGarantie, typeInventaire, codeInven, modeNavig);

            InventaireDto toReturn = new InventaireDto();

            NomsInternesEcran provenance = NomsInternesEcran.RisqueObjet;
            Enum.TryParse(ecranProvenance, out provenance);
            switch (provenance)
            {
                case NomsInternesEcran.RisqueObjet:
                    UpdateKpObj(codeOffre, version, type, codeRisque, codeObjet, codeInven);
                    toReturn = LoadInventaireRisqueObjet(codeOffre, version, type, codeAvn, codeRisque, codeObjet, toReturn, modeNavig);
                    break;

                case NomsInternesEcran.FormuleGarantie:
                    toReturn = LoadInventaireFormuleGarantie(codeOffre, version, type, codeAvn, codeFormule, codeGarantie, toReturn, modeNavig);
                    break;

            }

            if (toReturn == null)
                toReturn = new InventaireDto();
            toReturn.InventaireType = LoadTypeInventaire(typeInventaire);
            if (toReturn.Code != 0)
            {
                toReturn.InventaireInfos = LoadGridRowInventaire(toReturn.Code, toReturn.CodeBranche, toReturn.CodeCible, modeNavig);
                toReturn.InventaireInfos.ForEach(i =>
                {
                    i.DateDeb = AlbConvert.ConvertIntToDate(i.DateDebDB);
                    i.HeureDeb = AlbConvert.ConvertIntToTime(i.HeureDebDB);
                    i.DateFin = AlbConvert.ConvertIntToDate(i.DateFinDB);
                    i.HeureFin = AlbConvert.ConvertIntToTime(i.HeureFinDB);
                    i.AnneeNaissance = toReturn.InventaireType == 3 && i.DateNaissanceDB != 0 ? i.DateNaissanceDB.ToString() : string.Empty;
                    i.DateNaissance = AlbConvert.ConvertIntToDate(i.DateNaissanceDB);
                    i.AccidentSeul = (i.AccidentSeulDB == "1") ? true : false;
                    i.AvantProd = (i.AvantProdDB == "1") ? true : false;
                });
            }
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = -1 };
            Parallel.Invoke(parallelOptions,
                () =>
                {
                    toReturn.Unites = CommonRepository.GetParametres(branche, cible, "PRODU", "QCVAU");
                },
                () =>
                {
                    toReturn.Types = CommonRepository.GetParametres(branche, cible, "PRODU", "QCVAT");
                },
                () =>
                {
                    toReturn.NaturesLieu = CommonRepository.GetParametres(branche, cible, "ALSPK", "NLOC");
                },
                () =>
                {
                    toReturn.CodesMateriel = CommonRepository.GetParametres(branche, cible, "KHEOP", toReturn.InventaireType == 10 ? "AUPRO" : toReturn.InventaireType == 11 ? "AULOC" : toReturn.InventaireType == 12 ? "IMMO" : "MATRS");
                },
                () =>
                {
                    toReturn.ListePays = CommonRepository.GetParametres(branche, cible, "GENER", "CPAYS");
                },
                () =>
                {
                    if (toReturn.InventaireType == 5)//voir bug 1353
                        toReturn.CodesExtension = CommonRepository.GetParametres(branche, cible, "KHEOP", "INDAU");
                    else
                        toReturn.CodesExtension = CommonRepository.GetParametres(branche, cible, "KHEOP", "INDIS");
                },
                () =>
                {

                    toReturn.CodesQualite = toReturn.InventaireType == 16 ? CommonRepository.GetParametres(branche, cible, "KHEOP", "QASS") : CommonRepository.GetParametres(branche, cible, "ALSPK", "QJUR");
                },
                () =>
                {
                    toReturn.CodesRenonce = CommonRepository.GetParametres(branche, cible, "ALSPK", "REN");
                },
                () =>
                {
                    toReturn.CodesRsqLoc = CommonRepository.GetParametres(branche, cible, "KHEOP", "BFRLO");
                });


            return toReturn;
        }

        public static InventaireGridRowDto SaveLineInventaire(string codeOffre, int version, string type, int codeInven, int typeInven, InventaireGridRowDto inventaireLigne)
        {
            //int existInven = CheckLineInventaireExist(inventaireLigne.Code);

            List<EacParameter> param = new List<EacParameter>();
            param.Add(new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength) { Value = codeOffre.PadLeft(9, ' ') });
            param.Add(new EacParameter("P_VERSION", DbType.Int32) { Value = version });
            param.Add(new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = type });
            param.Add(new EacParameter("P_CODERSQ", DbType.Int32) { Value = 0 });
            param.Add(new EacParameter("P_CODEOBJ", DbType.Int32) { Value = 0 });
            param.Add(new EacParameter("P_TYPEINVENTAIRE", DbType.Int32) { Value = typeInven });
            param.Add(new EacParameter("P_CODEINVENTAIRE", DbType.Int32) { Value = codeInven });
            param.Add(new EacParameter("P_CODELIGNE", DbType.Int32) { Value = inventaireLigne.Code });
            param.Add(new EacParameter("P_NUMLGN", DbType.Int32) { Value = 0 });//ToDo ECM ???utilité???
            param.Add(new EacParameter("P_DESC", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Designation });
            param.Add(new EacParameter("P_DESINATUREMARCHANDISE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.DescNatureMarchandise });
            param.Add(new EacParameter("P_SITE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Lieu });
            param.Add(new EacParameter("P_NATLIEU", DbType.AnsiStringFixedLength) { Value = inventaireLigne.NatureLieu });
            param.Add(new EacParameter("P_CP", DbType.Int32) { Value = inventaireLigne.CodePostal });
            param.Add(new EacParameter("P_VILLE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Ville });
            param.Add(new EacParameter("P_LIENADH", DbType.Int32) { Value = 0 });//ToDo ECM ???utilité???
            param.Add(new EacParameter("P_DATEDEB", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(inventaireLigne.DateDeb) ?? 0 });
            param.Add(new EacParameter("P_HEUREDEB", DbType.Int32) { Value = AlbConvert.ConvertTimeToInt(inventaireLigne.HeureDeb) ?? 0 });
            param.Add(new EacParameter("P_DATEFIN", DbType.Int32) { Value = AlbConvert.ConvertDateToInt(inventaireLigne.DateFin) ?? 0 });
            param.Add(new EacParameter("P_HEUREFIN", DbType.Decimal) { Value = AlbConvert.ConvertTimeToInt(inventaireLigne.HeureFin) ?? 0 });
            param.Add(new EacParameter("P_MNT1", DbType.Decimal) { Value = inventaireLigne.Montant });
            param.Add(new EacParameter("P_MNT2", 0) { Value = inventaireLigne.Mnt2 });
            param.Add(new EacParameter("P_NBEVT", DbType.Int32) { Value = inventaireLigne.NbEvenement });
            param.Add(new EacParameter("P_NBPERS", DbType.Int32) { Value = inventaireLigne.NbPers });
            param.Add(new EacParameter("P_NOM", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Nom });
            param.Add(new EacParameter("P_PNOM", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Prenom });
            param.Add(new EacParameter("P_DATENAIS", DbType.Int32)
            {
                Value = !string.IsNullOrEmpty(inventaireLigne.AnneeNaissance) && inventaireLigne.AnneeNaissance != "0" ? Convert.ToInt32(inventaireLigne.AnneeNaissance) : AlbConvert.ConvertDateToInt(inventaireLigne.DateNaissance) ?? 0
            });
            param.Add(new EacParameter("P_FONC", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Fonction });
            param.Add(new EacParameter("P_CAPITALDC", DbType.Int32) { Value = inventaireLigne.CapitalDeces });
            param.Add(new EacParameter("P_CAPITALIP", DbType.Int32) { Value = inventaireLigne.CapitalIP });
            param.Add(new EacParameter("P_ACCIDENT", DbType.AnsiStringFixedLength) { Value = inventaireLigne.AccidentSeul ? "0" : "1" });
            param.Add(new EacParameter("P_AVTPROD", DbType.AnsiStringFixedLength) { Value = inventaireLigne.AvantProd ? "0" : "1" });
            param.Add(new EacParameter("P_NUMSERIE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.NumSerie });
            param.Add(new EacParameter("P_CODEMAT", DbType.AnsiStringFixedLength) { Value = inventaireLigne.CodeMateriel });
            param.Add(new EacParameter("P_SEXE", DbType.AnsiStringFixedLength) { Value = string.Empty });
            param.Add(new EacParameter("P_MEDIQUEST", DbType.AnsiStringFixedLength) { Value = string.Empty });
            param.Add(new EacParameter("P_MEDIANTE", DbType.Int32) { Value = 0 });
            param.Add(new EacParameter("P_PROACTIVITE", DbType.AnsiStringFixedLength) { Value = string.Empty });
            param.Add(new EacParameter("P_CODEEXT", DbType.AnsiStringFixedLength) { Value = inventaireLigne.CodeExtension });
            param.Add(new EacParameter("P_DESIFRANCHISE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Franchise });
            param.Add(new EacParameter("P_QUALITE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.CodeQualite });
            param.Add(new EacParameter("P_RENONCE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.CodeRenonce });
            param.Add(new EacParameter("P_RSQLOC", DbType.AnsiStringFixedLength) { Value = inventaireLigne.CodeRsqLoc });
            param.Add(new EacParameter("P_MNT3", DbType.Decimal) { Value = inventaireLigne.Contenu });
            param.Add(new EacParameter("P_MNT4", DbType.Decimal) { Value = inventaireLigne.MatBur });

            param.Add(new EacParameter("P_MODELE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Modele });
            param.Add(new EacParameter("P_MARQUE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Marque });
            param.Add(new EacParameter("P_IMMATRICULATION", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Immatriculation });
            param.Add(new EacParameter("P_DESIDEPART", DbType.AnsiStringFixedLength) { Value = inventaireLigne.DescDepart });
            param.Add(new EacParameter("P_DESIDESTINATION", DbType.AnsiStringFixedLength) { Value = inventaireLigne.DescDestination });
            param.Add(new EacParameter("P_DESIMODALITE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.DescModalite });

            param.Add(new EacParameter("P_PAYS", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Pays });

            param.Add(new EacParameter("P_ADRCHR", DbType.Int32) { Value = inventaireLigne.Adresse.NumeroChrono });
            param.Add(new EacParameter("P_ADRBATIMENT", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.Batiment });
            param.Add(new EacParameter("P_ADRNUMVOIE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.NumeroVoie  });
            param.Add(new EacParameter("P_ADRNUMVOIE2", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.NumeroVoie2 });
            param.Add(new EacParameter("P_ADREXTVOIE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.ExtensionVoie });
            param.Add(new EacParameter("P_ADRVOIE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.NomVoie });
            param.Add(new EacParameter("P_ADRBP", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.BoitePostale });
            param.Add(new EacParameter("P_ADRCP", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.CodePostal });
            param.Add(new EacParameter("P_ADRDEP", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.Departement });
            param.Add(new EacParameter("P_ADRVILLE", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.NomVille });
            param.Add(new EacParameter("P_ADRCPX", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.CodePostalCedex });
            param.Add(new EacParameter("P_ADRVILLEX", DbType.AnsiStringFixedLength) { Value = inventaireLigne.Adresse.NomCedex });
            param.Add(new EacParameter("P_ADRMATHEX", DbType.Int32) { Value = inventaireLigne.Adresse.MatriculeHexavia });
            param.Add(new EacParameter("P_ADRLATITUDE", DbType.Decimal) { Value = inventaireLigne.Adresse.Latitude });
            param.Add(new EacParameter("P_ADRLONGITUDE", DbType.Decimal) { Value = inventaireLigne.Adresse.Longitude });

            var tmp = string.Concat(inventaireLigne.Adresse.Batiment, inventaireLigne.Adresse.NumeroVoie, inventaireLigne.Adresse.ExtensionVoie, inventaireLigne.Adresse.NomVoie, inventaireLigne.Adresse.BoitePostale,
                inventaireLigne.Adresse.CodePostal, inventaireLigne.Adresse.Departement, inventaireLigne.Adresse.NomVille, inventaireLigne.Adresse.CodePostalCedex, inventaireLigne.Adresse.NomCedex);
            
            param.Add(new EacParameter("P_ISADDRESSEMPTY", DbType.Int32) { Value = tmp.Trim() == "" ? 1 : 0 });

            param.Add(new EacParameter("P_IDROWCODE", DbType.Int32) { Direction = ParameterDirection.InputOutput, Value = 0 });

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVELIGNEINVENTAIRE", param);

            inventaireLigne.Code = Convert.ToInt32(param[67].Value.ToString());

            #region Récupération de la branche/cible

            DbParameter[] paramBrc = new DbParameter[3];
            paramBrc[0] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
            paramBrc[1] = new EacParameter("version", 0);
            paramBrc[1].Value = version;
            paramBrc[2] = new EacParameter("type", type);

            string sqlBrc = @"SELECT PBBRA STRRETURNCOL, KAACIBLE STRRETURNCOL2
                                FROM YPOBASE
                                    INNER JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                                WHERE PBIPB = :codeAffaire AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlBrc, paramBrc);
            var branche = string.Empty;
            var cible = string.Empty;

            if (result != null && result.Any())
            {
                branche = result.FirstOrDefault().StrReturnCol;
                cible = result.FirstOrDefault().StrReturnCol2;
            }

            #endregion

            inventaireLigne.NaturesLieu = CommonRepository.GetParametres(branche, cible, "ALSPK", "NLOC");
            if (!string.IsNullOrEmpty(inventaireLigne.NatureLieu))
                inventaireLigne.DescNatLieu = CommonRepository.GetParametreByCode(branche, cible, "ALSPK", "NLOC", inventaireLigne.NatureLieu).Libelle;

            inventaireLigne.CodesMat = CommonRepository.GetParametres(branche, cible, "KHEOP", typeInven == 10 ? "AUPRO" : typeInven == 11 ? "AULOC" : typeInven == 12 ? "IMMO" : "MATRS");
            if (!string.IsNullOrEmpty(inventaireLigne.CodeMateriel))
                inventaireLigne.DescMat = CommonRepository.GetParametreByCode(branche, cible, "KHEOP", typeInven == 10 ? "AUPRO" : typeInven == 11 ? "AULOC" : typeInven == 12 ? "IMMO" : "MATRS", inventaireLigne.CodeMateriel).Libelle;

            inventaireLigne.CodesExtension = CommonRepository.GetParametres(branche, cible, "KHEOP", typeInven == 5 ? "INDAU" : "INDIS");
            //TODO : Ce test n'est valable que pour RS
            if (!string.IsNullOrEmpty(inventaireLigne.CodeExtension))
                inventaireLigne.DescExtension = CommonRepository.GetParametreByCode(branche, cible, "KHEOP", typeInven == 5 ? "INDAU" : "INDIS", inventaireLigne.CodeExtension).Libelle;

            inventaireLigne.CodesQualite = typeInven == 16 ? CommonRepository.GetParametres(branche, cible, "KHEOP", "QASS") : CommonRepository.GetParametres(branche, cible, "ALSPK", "QJUR");
            if (!string.IsNullOrEmpty(inventaireLigne.CodeQualite))
                inventaireLigne.DescQualite = typeInven == 16 ? CommonRepository.GetParametreByCode(branche, cible, "KHEOP", "QASS", inventaireLigne.CodeQualite).Libelle :
                                                                CommonRepository.GetParametreByCode(branche, cible, "ALSPK", "QJUR", inventaireLigne.CodeQualite).Libelle;

            inventaireLigne.CodesRenonce = CommonRepository.GetParametres(branche, cible, "ALSPK", "REN");
            if (!string.IsNullOrEmpty(inventaireLigne.CodeRenonce))
                inventaireLigne.DescRenonce = CommonRepository.GetParametreByCode(branche, cible, "ALSPK", "REN", inventaireLigne.CodeRenonce).Libelle;

            inventaireLigne.CodesRsqLoc = CommonRepository.GetParametres(branche, cible, "KHEOP", "BFRLO");
            if (!string.IsNullOrEmpty(inventaireLigne.CodeRsqLoc))
                inventaireLigne.DescRsqLoc = CommonRepository.GetParametreByCode(branche, cible, "KHEOP", "BFRLO", inventaireLigne.CodeRsqLoc).Libelle;

            inventaireLigne.ListPays = CommonRepository.GetParametres(branche, cible, "GENER", "CPAYS");
            if (!string.IsNullOrEmpty(inventaireLigne.Pays))
                inventaireLigne.DescPays = CommonRepository.GetParametreByCode(branche, cible, "GENER", "CPAYS", inventaireLigne.Pays).Libelle;

            return inventaireLigne;
        }

        public static void SaveInventaire(string codeOffre, string version, string type, string ecranProvenance, string codeRisque, string codeObjet, string codeInven,
            string descriptif, string description, string valReport, string unitReport, string typeReport, string taxeReport, bool activeReport, string typeAlim,
            string garantie, string codeFormule, string codeOption)
        {
             var idGar = !string.IsNullOrEmpty(garantie) && !string.IsNullOrEmpty(codeFormule) && !string.IsNullOrEmpty(codeOption) ?
                FormuleRepository.GetCodeGarantieBySeq(garantie, codeOffre, version, type, codeFormule, codeOption, ModeConsultation.Standard)
                : 0;

            DbParameter[] paramSP = new DbParameter[16];
            paramSP[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            paramSP[1] = new EacParameter("P_VERSION", 0);
            paramSP[1].Value = Convert.ToInt32(version);
            paramSP[2] = new EacParameter("P_TYPE", type);
            paramSP[3] = new EacParameter("P_CODERSQ", 0);
            paramSP[3].Value = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 0;
            paramSP[4] = new EacParameter("P_CODEOBJ", 0);
            paramSP[4].Value = !string.IsNullOrEmpty(codeObjet) ? Convert.ToInt32(codeObjet) : 0;
            paramSP[5] = new EacParameter("P_CODEINVEN", 0);
            paramSP[5].Value = !string.IsNullOrEmpty(codeInven) ? Convert.ToInt32(codeInven) : 0;
            paramSP[6] = new EacParameter("P_DESCRIPTIF", descriptif);
            paramSP[7] = new EacParameter("P_DESCRIPTION", description);
            paramSP[8] = new EacParameter("P_VALREPORT", 0);
            paramSP[8].Value = !string.IsNullOrEmpty(valReport) ? Convert.ToDouble(valReport) : 0;
            paramSP[9] = new EacParameter("P_UNITREPORT", unitReport);
            paramSP[10] = new EacParameter("P_TYPEREPORT", typeReport);
            paramSP[11] = new EacParameter("P_TAXEREPORT", taxeReport);
            paramSP[12] = new EacParameter("P_ACTIVEREPORT", activeReport ? "O" : "N");
            paramSP[13] = new EacParameter("P_TYPEALIM", typeAlim);
            paramSP[14] = new EacParameter("P_GARANTIE", 0);
            paramSP[14].Value = idGar;
            paramSP[15] = new EacParameter("P_PERIMETRE", PerimetreDesignation.Inventaire);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATEINVENTAIRE", paramSP);
        }

        public static void DeleteLineInventaire(string codeInven)
        {
            /* Suppresion de la désignation de la franchise */
            string sqlDesi = string.Format(@"SELECT KBFKADFH INT64RETURNCOL FROM KPINVED WHERE KBFID = {0}", codeInven);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDesi);
            if (result != null && result.Any() && result.FirstOrDefault().Int64ReturnCol != 0)
            {
                sqlDesi = string.Format(@"delete from kpdesi where kadchr = {0}", result.FirstOrDefault().Int64ReturnCol);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDesi);
            }

            /* Suppresion de l'adresse s'il y a */
            string sqlAdr = string.Format(@"SELECT KBFADH INT64RETURNCOL FROM KPINVED WHERE KBFID = {0}", codeInven);
            var resultAdr = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlAdr);
            if (resultAdr != null && resultAdr.Any() && resultAdr.FirstOrDefault().Int64ReturnCol != 0)
            {
                sqlAdr = string.Format(@"delete from yadress where abpchr = {0}", resultAdr.FirstOrDefault().Int64ReturnCol);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlAdr);
                sqlAdr = string.Format(@"delete from kgeoloc where kgeochr = {0}", resultAdr.FirstOrDefault().Int64ReturnCol);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlAdr);
            }

            DbParameter[] param = new DbParameter[1];
            string sql = @"DELETE FROM KPINVED
                        WHERE KBFID = :KBFID";
            param[0] = new EacParameter("KBFID", Convert.ToInt32(codeInven));
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static double GetSumBugdetInventaire(string codeInventaire, string typeInventaire)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeInven", 0);
            param[0].Value = Convert.ToInt32(codeInventaire);

            string sql = string.Empty;

            switch (typeInventaire)
            {
                case "10":
                case "11":
                case "12":
                    sql = @"SELECT CAST(SUM(KBFMNT2) AS NUMERIC(13,2)) MONTANT FROM KPINVED WHERE KBFKBEID = :codeInven";
                    break;
                default:
                    sql = @"SELECT CAST(SUM(KBFMNT1) AS NUMERIC(13,2)) MONTANT FROM KPINVED WHERE KBFKBEID = :codeInven";
                    break;
            }

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
                return result.FirstOrDefault().Montant;

            return 0;
        }


        #endregion

        #region Méthodes Privées

        private static void CreateInventaire(string codeOffre, int version, string type, string ecranProvenance, int codeRisque, int codeObjet, int codeFormule, string codeGarantie, string typeInventaire, Int64 codeInven, ModeConsultation modeNavig)
        {
            Int64 nbInven = CountInventaire(codeInven, modeNavig);
            if (nbInven == 0 && !CheckKpinvApp(codeOffre, version, type, ecranProvenance, codeRisque, codeObjet, codeFormule, codeGarantie) && (modeNavig != ModeConsultation.Historique))
            {
                InsertKpInven(codeOffre, version, type, typeInventaire, codeInven);
                InsertKpInvApp(codeOffre, version, type, ecranProvenance, codeRisque, codeObjet, codeFormule, codeGarantie, codeInven);
            }
        }

        public static void UpdateKpObj(string codeOffre, int version, string type, int codeRisque, int codeObjet, Int64 codeInven)
        {
            string sql = string.Format(@"UPDATE KPOBJ SET KACINVEN = {0} WHERE KACIPB = '{1}' AND KACALX = {2} AND KACTYP = '{3}' AND KACRSQ = {4} AND KACOBJ = {5}",
                                    codeInven, codeOffre.PadLeft(9, ' '), version, type, codeRisque, codeObjet);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        private static Int64 CountInventaire(Int64 codeInven, ModeConsultation modeNavig)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM {0} WHERE KBEID = :KBEID", CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"));
            param[0] = new EacParameter("KBEID", codeInven);

            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne;
        }

        private static void InsertKpInven(string codeOffre, int version, string type, string typeInventaire, Int64 codeInven)
        {
            InventaireDto invTyp = GetInvTyp(typeInventaire);

            DbParameter[] param = new DbParameter[8];
            string sql = @"INSERT INTO KPINVEN
                        (KBEID, KBETYP, KBEIPB, KBEALX, KBECHR, KBEDESC, KBEKAGID, KBEKADID)
                        VALUES
                        (:KBEID, :KBETYP, :KBEIPB, :KBEALX, :KBECHR, :KBEDESC, :KBEKAGID, :KBEKADID)";
            param[0] = new EacParameter("KBEID", codeInven);
            param[1] = new EacParameter("KBETYP", type);
            param[2] = new EacParameter("KBEIPB", codeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("KBEALX", version);
            param[4] = new EacParameter("KBECHR", GetNumInven(codeOffre, version, type));
            param[5] = new EacParameter("KBEDESC", invTyp.Description);
            param[6] = new EacParameter("KBEKAGID", invTyp.Code);
            param[7] = new EacParameter("KBEKADID", "0");

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static Int64 GetNumInven(string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT IFNULL(MAX(KBECHR), 0) + 1 NBLIGN FROM KPINVEN
                    WHERE KBEIPB = :KBEIPB AND KBEALX = :KBEALX AND KBETYP = :KBETYP";
            param[0] = new EacParameter("KBEIPB", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("KBEALX", version);
            param[2] = new EacParameter("KBETYP", type);
            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne;
        }

        private static InventaireDto GetInvTyp(string typeInventaire)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("KAGTYINV", typeInventaire);

            string sql = @"SELECT KAGID CODE, KAGDESC DESCRIPTION FROM KINVTYP
                    WHERE KAGTYINV = :KAGTYINV";

            return DbBase.Settings.ExecuteList<InventaireDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        private static void InsertKpInvApp(string codeOffre, int version, string type, string ecranProvenance, int codeRisque, int codeObjet, int codeFormule, string codeGarantie, Int64 codeInven)
        {
            NomsInternesEcran provenance = NomsInternesEcran.RisqueObjet;
            Enum.TryParse(ecranProvenance, out provenance);
            switch (provenance)
            {
                case NomsInternesEcran.RisqueObjet:
                    InsertKpInvAppRisqueObjet(codeOffre, version, type, codeRisque, codeObjet, codeInven);
                    break;

                case NomsInternesEcran.FormuleGarantie:
                    InsertKpInvAppFormuleGarantie(codeOffre, version, type, codeFormule, codeGarantie, codeInven);
                    break;
            }
        }

        private static Int64 GetNumInvApp(string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT IFNULL(MAX(KBGNUM), 0) + 1 NBLIGN FROM KPINVAPP
                    WHERE KBGIPB = :KBGIPB AND KBGALX = :KBGALX AND KBGTYP = :KBGTYP";
            param[0] = new EacParameter("KBGIPB", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("KBGALX", version);
            param[2] = new EacParameter("KBGTYP", type);
            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne;
        }

        private static bool CheckKpinvApp(string codeOffre, int version, string type, string ecranProvenance, int codeRisque, int codeObjet, int codeFormule, string codeGarantie)
        {

            NomsInternesEcran provenance = NomsInternesEcran.RisqueObjet;
            Enum.TryParse(ecranProvenance, out provenance);
            switch (provenance)
            {
                case NomsInternesEcran.RisqueObjet:
                    return CheckKpinvAppRisqueObjet(codeRisque, codeObjet, codeOffre, version, type);

                case NomsInternesEcran.FormuleGarantie:
                    return CheckKpinvAppFormuleGarantie(codeFormule, codeGarantie, codeOffre, version, type);

            }
            return true;
        }

        private static Int64 LoadTypeInventaire(string typeInventaire)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KAGTMAP TYPE FROM KINVTYP
                        WHERE KAGTYINV = :KAGTYINV";
            param[0] = new EacParameter("KAGTYINV", typeInventaire);

            var res = DbBase.Settings.ExecuteList<InventaireDto>(CommandType.Text, sql, param).FirstOrDefault();

            return res != null ? res.InventaireType : 0;
        }

        private static List<InventaireGridRowDto> LoadGridRowInventaire(Int64 codeInven, string codeBranche, string codeCible, ModeConsultation modeNavig)
        {
            List<InventaireGridRowDto> toReturn = new List<InventaireGridRowDto>();
            DbParameter[] param = new DbParameter[1];
            string sql = string.Format(@"SELECT KBFID CODE, KBFDESC DESIGNATION, KBFADH LIENADRESSE, KBFSITE LIEU, KBFCP CODEPOSTAL, KBFVILLE VILLE,
                            KBFDATDEB DATEDEB, KBFDEBHEU HEUREDEB, KBFDATFIN DATEFIN,  KBFFINHEU HEUREFIN, KBFMNT1 MONTANT,
                            KBFNBEVN NBEVEN, KBFNBPER NBPERS, KBFNOM NOM, KBFPNOM PRENOM, KBFDATNAI DATENAIS, KBFFONC FONCTION,
                            KBFCDEC CAPDECES, KBFCIP CAPIP, KBFACCS ACCIDENT, KBFAVPR AVTPROD, KBFMSR NUMSERIE, IFNULL(GENDESI.KADDESI, '') DESCRIPTION,
                            KBFNTLI NATLIEU, NATLIEU.TPLIB DESCNATLIEU, 
                            KBFCMAT CODEMAT, 
                            CASE KAGTMAP WHEN 10 THEN AUPRO.TPLIB WHEN 11 THEN AULOC.TPLIB WHEN 12 THEN IMMO.TPLIB ELSE MATERIEL.TPLIB END DESCMAT, 
                            KBFEXT CODEEXTENSION, 
                            CASE KAGTMAP WHEN 5 THEN EXTENSION.TPLIB ELSE EXTENSIONINDIS.TPLIB END DESCEXTENSION,
							KBFKADFH CODEFHDESI, IFNULL(TRIM(FHDESI.KADDESI), '') FRANCHISE,
                            KBFQUA CODEQUALITE, 
                            CASE KAGTMAP WHEN 16 THEN QUALITE2.TPLIB ELSE QUALITE.TPLIB END DESCQUALITE, 
                            KBFREN CODERENONCE, RENONCE.TPLIB DESCRENONCE, KBFRLO CODERSQLOC, RSQLOC.TPLIB DESCRSQLOC,
                            KBFMNT2 MNT2, KBFMNT3 CONTENU, KBFMNT4 MATBUR,KBFMOD MODELE, KBFMRQ MARQUE,KBFMIM IMMATRICULATION,KBFPAY PAYS, PARPAYS.TPLIB DESCPAYS,
                            KBFSIT2 DEPART,IFNULL(SIT2DESI.KADDESI, '') DESCDEPART,KBFSIT3 DESTINATION ,IFNULL(SIT3FHDESI.KADDESI, '') DESCDESTINATION,KBFDES2 MODALITE,IFNULL(MODDESI.KADDESI, '') DESCMODALITE,
                            KBFKADID NATUREMARCHANDISE,IFNULL(GENDESI.KADDESI, '') DescNatureMarchandise
                           
                        FROM {0}
                            LEFT JOIN {1} GENDESI ON KBFKADID = GENDESI.KADCHR
                            LEFT JOIN {1} FHDESI ON KBFKADFH = FHDESI.KADCHR
                 
                            LEFT JOIN {1} SIT2DESI ON KBFSIT2 = SIT2DESI.KADCHR
                            LEFT JOIN {1} SIT3FHDESI ON KBFSIT3 = SIT3FHDESI.KADCHR
                            LEFT JOIN {1} MODDESI ON KBFDES2 = MODDESI.KADCHR
                           
                            LEFT JOIN {6} ON KBFKBEID = KBEID
                            {2}
                            {3}
                            {4}
                            {5}
                            {7}
                            {8}
                            {9}
                            {10}
                            {11}
                            {12}
                            {13}
                            {14}
                            INNER JOIN KINVTYP ON KBEKAGID = KAGID
                        WHERE KBFKBEID = :KBFKBEID",
                        /*{0}*/CommonRepository.GetPrefixeHisto(modeNavig, "KPINVED"),
                        /*{1}*/CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                        /*{2}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "ALSPK", famille: "NLOC", alias: "NATLIEU", otherCriteria: " AND NATLIEU.TCOD = KBFNTLI"),
                        /*{3}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "MATRS", alias: "MATERIEL", otherCriteria: " AND MATERIEL.TCOD = KBFCMAT"),
                        /*{4}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "INDAU", alias: "EXTENSION", otherCriteria: " AND EXTENSION.TCOD = KBFEXT"),
                        /*{5}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "INDIS", alias: "EXTENSIONINDIS", otherCriteria: " AND EXTENSIONINDIS.TCOD = KBFEXT"),
                        /*{6}*/CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                        /*{7}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "ALSPK", famille: "QJUR", alias: "QUALITE", otherCriteria: " AND QUALITE.TCOD = KBFQUA"),
                        /*{8}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "ALSPK", famille: "REN", alias: "RENONCE", otherCriteria: " AND RENONCE.TCOD = KBFREN"),
                        /*{9}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "BFRLO", alias: "RSQLOC", otherCriteria: " AND RSQLOC.TCOD = KBFRLO"),
                        /*{10}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "AUPRO", alias: "AUPRO", otherCriteria: " AND AUPRO.TCOD = KBFCMAT"),
                        /*{11}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "AULOC", alias: "AULOC", otherCriteria: " AND AULOC.TCOD = KBFCMAT"),
                        /*{12}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "IMMO", alias: "IMMO", otherCriteria: " AND IMMO.TCOD = KBFCMAT"),
                        /*{13}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "GENER", famille: "CPAYS", alias: "PARPAYS", otherCriteria: " AND PARPAYS.TCOD = KBFPAY"),
                        /*{14}*/CommonRepository.BuildJoinYYYYPAR(branche: codeBranche, cible: codeCible, additionalParam: string.Empty, typeJoin: "LEFT", concept: "KHEOP", famille: "QASS", alias: "QUALITE2", otherCriteria: " AND QUALITE2.TCOD = KBFQUA"));

            param[0] = new EacParameter("KBFKBEID", codeInven);

            toReturn = DbBase.Settings.ExecuteList<InventaireGridRowDto>(CommandType.Text, sql, param);
            if (toReturn != null && toReturn.Any())
            {
                foreach (var inventaire in toReturn)
                {
                    if (inventaire.LienADH > 0)
                    {
                        inventaire.Adresse = AdresseRepository.ObtenirAdresse(inventaire.LienADH);
                    } else
                    {
                        inventaire.Adresse = new WSAS400.DTO.Adresses.AdressePlatDto();
                    }
                }
            }
            return toReturn;
        }

        private static int CheckLineInventaireExist(Int64 codeInventaire)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT COUNT(*) NBLGN FROM KPINVED
                        WHERE KBFID = :KBFID";
            param[0] = new EacParameter("KBFID", codeInventaire);

            var result = DbBase.Settings.ExecuteList<ValidationDto>(CommandType.Text, sql, param).FirstOrDefault();
            return result.NbLigne;
        }

        private static Int64 InsertLineInventaire(string codeOffre, int version, string type, int codeInven, InventaireGridRowDto inventaireLigne, string idDesiFranchise)
        {
            inventaireLigne.Code = CommonRepository.GetAS400Id("KBFID");

            DbParameter[] param = new DbParameter[37];
            string sql = string.Empty;

            sql = @"INSERT INTO KPINVED
                        (KBFID, KBFTYP, KBFIPB, KBFALX, KBFKBEID, KBFNUMLGN, KBFDESC, KBFKADID, KBFSITE, KBFNTLI, KBFCP, KBFVILLE, KBFADH, KBFDATDEB, KBFDEBHEU, KBFDATFIN, KBFFINHEU, KBFMNT1, KBFMNT2, KBFNBEVN, KBFNBPER, KBFNOM, KBFPNOM, KBFDATNAI, KBFFONC, KBFCDEC, KBFCIP, KBFACCS, KBFAVPR, KBFMSR, KBFCMAT, KBFSEX, KBFMDQ, KBFMDA, KBFACTP, KBFEXT, KBFKADFH)
                        VALUES
                        (:KBFID, :KBFTYP, :KBFIPB, :KBFALX, :KBFKBEID, :KBFNUMLGN, :KBFDESC, :KBFKADID, :KBFSITE, :KBFNTLI, :KBFCP, :KBFVILLE, :KBFADH, :KBFDATDEB, :KBFDEBHEU, :KBFDATFIN, :KBFFINHEU, :KBFMNT1, :KBFMNT2, :KBFNBEVN, :KBFNBPER, :KBFNOM, :KBFPNOM, :KBFDATNAI, :KBFFONC, :KBFCDEC, :KBFCIP, :KBFACCS, :KBFAVPR, :KBFMSR, :KBFCMAT, :KBFSEX, :KBFMDQ, :KBFMDA, :KBFACTP, :KBFEXT, :KBFKADFH)";

            param[0] = new EacParameter("KBFID", inventaireLigne.Code);
            param[1] = new EacParameter("KBFTYP", type);
            param[2] = new EacParameter("KBFIPB", codeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("KBFALX", version);
            param[4] = new EacParameter("KBFKBEID", codeInven);
            param[5] = new EacParameter("KBFNUMLGN", "0"); //ToDo ECM ???utilité???
            param[6] = new EacParameter("KBFDESC", inventaireLigne.Designation);
            param[7] = new EacParameter("KBFKADID", "0"); //ToDo ECM ???utilité???
            param[8] = new EacParameter("KBFSITE", inventaireLigne.Lieu);
            param[9] = new EacParameter("KBFNTLI", inventaireLigne.NatureLieu);
            param[10] = new EacParameter("KBFCP", inventaireLigne.CodePostal);
            param[11] = new EacParameter("KBFVILLE", inventaireLigne.Ville);
            param[12] = new EacParameter("KBFADH", "0"); //ToDo ECM ???utilité???
            param[13] = new EacParameter("KBFDATDEB", AlbConvert.ConvertDateToInt(inventaireLigne.DateDeb) ?? 0);
            param[14] = new EacParameter("KBFDEBHEU", AlbConvert.ConvertTimeToInt(inventaireLigne.HeureDeb) ?? 0);
            param[15] = new EacParameter("KBFDATFIN", AlbConvert.ConvertDateToInt(inventaireLigne.DateFin) ?? 0);
            param[16] = new EacParameter("KBFFINHEU", AlbConvert.ConvertTimeToInt(inventaireLigne.HeureFin) ?? 0);
            param[17] = new EacParameter("KBFMNT1", inventaireLigne.Montant);
            param[18] = new EacParameter("KBFMNT2", "0"); //ToDo ECM ???utilité???
            param[19] = new EacParameter("KBFNBEVN", inventaireLigne.NbEvenement);
            param[20] = new EacParameter("KBFNBPER", inventaireLigne.NbPers);
            param[21] = new EacParameter("KBFNOM", inventaireLigne.Nom);
            param[22] = new EacParameter("KBFPNOM", inventaireLigne.Prenom);
            param[23] = new EacParameter("KBFDATNAI", !string.IsNullOrEmpty(inventaireLigne.AnneeNaissance) && inventaireLigne.AnneeNaissance != "0" ? Convert.ToInt32(inventaireLigne.AnneeNaissance) : AlbConvert.ConvertDateToInt(inventaireLigne.DateNaissance) ?? 0);
            param[24] = new EacParameter("KBFFONC", inventaireLigne.Fonction);
            param[25] = new EacParameter("KBFCDEC", inventaireLigne.CapitalDeces);
            param[26] = new EacParameter("KBFCIP", inventaireLigne.CapitalIP);
            param[27] = new EacParameter("KBFACCS", inventaireLigne.AccidentSeul ? 0 : 1);
            param[28] = new EacParameter("KBFAVPR", inventaireLigne.AvantProd ? 0 : 1);
            param[29] = new EacParameter("KBFMSR", inventaireLigne.NumSerie);
            param[30] = new EacParameter("KBFCMAT", inventaireLigne.CodeMateriel);
            param[31] = new EacParameter("KBFSEX", string.Empty);
            param[32] = new EacParameter("KBFMDQ", string.Empty);
            param[33] = new EacParameter("KBFMDA", 0);
            param[33].Value = 0;
            param[34] = new EacParameter("KBFACTP", string.Empty);
            param[35] = new EacParameter("KBFEXT", string.Empty);
            param[36] = new EacParameter("KBFKADFH", 0);
            param[36].Value = !string.IsNullOrEmpty(idDesiFranchise) ? Convert.ToInt32(idDesiFranchise) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return inventaireLigne.Code;
        }

        private static Int64 UpdateLineInventaire(InventaireGridRowDto inventaireLigne, string idDesiFranchise)
        {
            DbParameter[] param = new DbParameter[29];
            string sql = string.Empty;

            sql = @"UPDATE KPINVED
                        SET KBFDESC = :KBFDESC, KBFSITE = :KBFSITE, KBFCP = :KBFCP, KBFVILLE = :KBFVILLE, KBFDATDEB = :KBFDATDEB, KBFDEBHEU = :KBFDEBHEU, KBFDATFIN = :KBFDATFIN, KBFFINHEU = :KBFFINHEU,
                            KBFMNT1 = :KBFMNT1, KBFNBEVN = :KBFNBEVN, KBFNBPER = :KBFNBPER, KBFNOM = :KBFNOM, KBFPNOM = :KBFPNOM, KBFDATNAI = :KBFDATNAI, KBFFONC = :KBFFONC, KBFCDEC = :KBFCDDEC,
                            KBFCIP = :KBFCIP, KBFACCS = :KBFACCS, KBFAVPR = :KBFAVPR, KBFMSR = :KBFMSR, KBFCMAT = :KBFCMAT, KBFNTLI = :KBFNTLI, KBFSEX = :KBFSEX, KBFMDQ = :KBFMDQ, KBFMDA = :KBFMDA, KBFACTP = :KBFACTP,
                            KBFEXT = :KBFEXT, KBFKADFH = :KBFKADFH
                        WHERE KBFID = :KBFID";

            param[0] = new EacParameter("KBFDESC", inventaireLigne.Designation);
            param[1] = new EacParameter("KBFSITE", inventaireLigne.Lieu);
            param[2] = new EacParameter("KBFCP", inventaireLigne.CodePostal);
            param[3] = new EacParameter("KBFVILLE", inventaireLigne.Ville);
            param[4] = new EacParameter("KBFDATDEB", AlbConvert.ConvertDateToInt(inventaireLigne.DateDeb) ?? 0);
            param[5] = new EacParameter("KBFDEBHEU", AlbConvert.ConvertTimeToInt(inventaireLigne.HeureDeb) ?? 0);
            param[6] = new EacParameter("KBFDATFIN", AlbConvert.ConvertDateToInt(inventaireLigne.DateFin) ?? 0);
            param[7] = new EacParameter("KBFFINHEU", AlbConvert.ConvertTimeToInt(inventaireLigne.HeureFin) ?? 0);
            param[8] = new EacParameter("KBFMNT1", inventaireLigne.Montant);
            param[9] = new EacParameter("KBFNBEVN", inventaireLigne.NbEvenement);
            param[10] = new EacParameter("KBFNBPER", inventaireLigne.NbPers);
            param[11] = new EacParameter("KBFNOM", inventaireLigne.Nom);
            param[12] = new EacParameter("KBFPNOM", inventaireLigne.Prenom);
            param[13] = new EacParameter("KBFDATNAI", !string.IsNullOrEmpty(inventaireLigne.AnneeNaissance) && inventaireLigne.AnneeNaissance != "0" ? Convert.ToInt32(inventaireLigne.AnneeNaissance) : AlbConvert.ConvertDateToInt(inventaireLigne.DateNaissance) ?? 0);
            param[14] = new EacParameter("KBFFONC", inventaireLigne.Fonction);
            param[15] = new EacParameter("KBFCDEC", inventaireLigne.CapitalDeces);
            param[16] = new EacParameter("KBFCIP", inventaireLigne.CapitalIP);
            param[17] = new EacParameter("KBFACCS", inventaireLigne.AccidentSeul ? 1 : 0);
            param[18] = new EacParameter("KBFAVPR", inventaireLigne.AvantProd ? 1 : 0);
            param[19] = new EacParameter("KBFMSR", inventaireLigne.NumSerie);
            param[20] = new EacParameter("KBFCMAT", inventaireLigne.CodeMateriel);
            param[21] = new EacParameter("KBFNTLI", inventaireLigne.NatureLieu);
            param[22] = new EacParameter("KBFSEX", string.Empty);
            param[23] = new EacParameter("KBFMDQ", string.Empty);
            param[24] = new EacParameter("KBFMDA", string.Empty);
            param[25] = new EacParameter("KBFACTP", 0);
            param[25].Value = 0;
            param[26] = new EacParameter("KBFEXT", inventaireLigne.CodeExtension);
            param[27] = new EacParameter("KBFKADFH", 0);
            param[27].Value = !string.IsNullOrEmpty(idDesiFranchise) ? Convert.ToInt32(idDesiFranchise) : 0;
            param[28] = new EacParameter("KBFID", inventaireLigne.Code);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return inventaireLigne.Code;
        }

        private static Int64 GetNumDesi(string codeInven)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KBEKADID NBLIGN FROM KPINVEN
                        WHERE KBEID = :KBEID";
            param[0] = new EacParameter("KBEID", Convert.ToInt32(codeInven));

            return DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne;
        }

        private static Int64 SaveDesi(string codeOffre, string version, string type, string codeRisque, string codeObjet, string description)
        {
            Int64 toReturn = CommonRepository.GetAS400Id("KADCHR");
            DbParameter[] param = new DbParameter[8];
            string sql = @"INSERT INTO KPDESI
                        (KADCHR, KADTYP, KADIPB, KADALX,KADPERI, KADRSQ, KADOBJ, KADDESI)
                        VALUES
                        (:KADCHR, :KADTYP, :KADIPB, :KADALX,:KADPERI, :KADRSQ, :KADOBJ, :KADDESI)";
            param[0] = new EacParameter("KADCHR", toReturn);
            param[1] = new EacParameter("KADTYP", type);
            param[2] = new EacParameter("KADIPB", codeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("KADALX", Convert.ToInt32(version));
            param[4] = new EacParameter("KADPERI", PerimetreDesignation.Inventaire.AsCode());
            param[5] = new EacParameter("KADRSQ", Convert.ToInt32(codeRisque));
            param[6] = new EacParameter("KADOBJ", Convert.ToInt32(codeObjet));
            param[7] = new EacParameter("KADDESI", !string.IsNullOrEmpty(description) ? description : string.Empty);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return toReturn;
        }

        private static void UpdateDesi(Int64 codeDesi, string description)
        {
            DbParameter[] param = new DbParameter[2];
            string sql = @"UPDATE KPDESI
                        SET KADDESI = :KADDESI
                        WHERE KADCHR = :KADCHR";
            param[0] = new EacParameter("KADDESI", !string.IsNullOrEmpty(description) ? description : string.Empty);
            param[1] = new EacParameter("KADCHR", Convert.ToInt32(codeDesi));
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        #region FormuleGarantie
        private static bool CheckKpinvAppFormuleGarantie(int codeFormule, string codeGarantie, string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[8];
            string sql = @"SELECT COUNT(*) NBLIGN 
                            FROM KPINVAPP 
                            WHERE KBGTYP = :KBGTYP AND KBGIPB = :KBGIPB AND KBGALX = :KBGALX AND KBGPERI = :KBGPERI
                                AND KBGRSQ = :KBGRSQ AND KBGOBJ = :KBGOBJ AND KBGFOR = :KBGFOR AND KBGGAR = :KBGGAR";
            param[0] = new EacParameter("KBGTYP", type);
            param[1] = new EacParameter("KBGIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KBGALX", version);
            param[3] = new EacParameter("KBGPERI", PerimetreInventaire.Garantie.AsCode());
            param[4] = new EacParameter("KBGRSQ", 0);
            param[5] = new EacParameter("KBGOBJ", 0);
            param[6] = new EacParameter("KBGFOR", codeFormule);
            param[7] = new EacParameter("KBGGAR", codeGarantie);
            return (DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne > 0);
        }
        private static void InsertKpInvAppFormuleGarantie(string codeOffre, int version, string type, int codeFormule, string codeGarantie, Int64 codeInven)
        {
            DbParameter[] param = new DbParameter[10];
            string sql = @"INSERT INTO KPINVAPP
                        (KBGTYP, KBGIPB, KBGALX, KBGNUM, KBGKBEID, KBGPERI, KBGRSQ, KBGOBJ, KBGFOR, KBGGAR)
                        VALUES
                        (:KBGTYP, :KBGIPB, :KBGALX, :KBGNUM, :KBGKBEID, :KBGPERI, :KBGRSQ, :KBGOBJ, :KBGFOR, :KBGGAR)";
            param[0] = new EacParameter("KBGTYP", type);
            param[1] = new EacParameter("KBGIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KBGALX", version);
            param[3] = new EacParameter("KBGNUM", GetNumInvApp(codeOffre, version, type));
            param[4] = new EacParameter("KBGKBEID", codeInven);
            param[5] = new EacParameter("KBGPERI", PerimetreInventaire.Garantie.AsCode());
            param[6] = new EacParameter("KBGRSQ", "0");
            param[7] = new EacParameter("KBGOBJ", "0");
            param[8] = new EacParameter("KBGFOR", codeFormule);
            param[9] = new EacParameter("KBGGAR", codeGarantie);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        private static InventaireDto LoadInventaireFormuleGarantie(string codeOffre, int version, string type, string codeAvn, int codeFormule, string codeGarantie, InventaireDto toReturn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("KBGFOR", codeFormule),
                new EacParameter("KBGGAR", codeGarantie),
                new EacParameter("KBETYP", type),
                new EacParameter("KBEALX", version),
                new EacParameter("KBEIPB", codeOffre.PadLeft(9, ' '))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT KBEID CODE, KBEDESC DESCRIPTIF, IFNULL(KADDESI, '') DESCRIPTION,
                            KBEREPVAL REPORTVAL, KBEVAL VALEUR, KBEVAT VALTYPE, KBEVAU VALUNIT, KBEVAH VALTAXE,
                            JEBRA BRANCHE, KABCIBLE CIBLE
                        FROM {0}
                            LEFT JOIN {1} ON KADCHR = KBEKADID
                            INNER JOIN {2} ON KBEID = KBGKBEID AND KBGFOR = :KBGFOR AND KBGGAR = :KBGGAR
                            INNER JOIN {4} ON KDDIPB = KBEIPB AND KDDALX = KBEALX AND KDDTYP = KBETYP AND KDDFOR = KBGFOR
                            INNER JOIN {5} ON JEIPB = KDDIPB AND JEALX = KDDALX AND JERSQ = KDDRSQ
                            INNER JOIN {6} ON KABIPB = KDDIPB AND KABALX = KDDALX AND KABTYP = KDDTYP AND KABRSQ = KDDRSQ
                        WHERE KBETYP = :KBETYP AND KBEALX = :KBEALX AND KBEIPB = :KBEIPB {3}",
                    /*0*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                    /*1*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
                    /*2*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPINVAPP"),
                    /*3*/    modeNavig == ModeConsultation.Historique ? " AND KBGAVN = :avn" : string.Empty,
                    /*4*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                    /*5*/    CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                    /*6*/    CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"));


            toReturn = DbBase.Settings.ExecuteList<InventaireDto>(CommandType.Text, sql, param).FirstOrDefault();
            return toReturn;
        }
        #endregion
        #region RisqueObjet
        private static bool CheckKpinvAppRisqueObjet(int codeRisque, int codeObjet, string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[8];
            string sql = @"SELECT COUNT(*) NBLIGN 
                            FROM KPINVAPP
                            WHERE KBGTYP = :KBGTYP AND KBGIPB = :KBGIPB AND KBGALX = :KBGALX AND KBGPERI = :KBGPERI
                                AND KBGRSQ = :KBGRSQ AND KBGOBJ = :KBGOBJ AND KBGFOR = :KBGFOR AND KBGGAR = :KBGGAR";
            param[0] = new EacParameter("KBGTYP", type);
            param[1] = new EacParameter("KBGIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KBGALX", version);
            param[3] = new EacParameter("KBGPERI", PerimetreInventaire.Objet.AsCode());
            param[4] = new EacParameter("KBGRSQ", codeRisque);
            param[5] = new EacParameter("KBGOBJ", codeObjet);
            param[6] = new EacParameter("KBGFOR", "0");
            param[7] = new EacParameter("KBGGAR", "");
            return (DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault().NbLigne > 0);
        }
        private static void InsertKpInvAppRisqueObjet(string codeOffre, int version, string type, int codeRisque, int codeObjet, Int64 codeInven)
        {
            DbParameter[] param = new DbParameter[10];
            string sql = @"INSERT INTO KPINVAPP
                        (KBGTYP, KBGIPB, KBGALX, KBGNUM, KBGKBEID, KBGPERI, KBGRSQ, KBGOBJ, KBGFOR, KBGGAR)
                        VALUES
                        (:KBGTYP, :KBGIPB, :KBGALX, :KBGNUM, :KBGKBEID, :KBGPERI, :KBGRSQ, :KBGOBJ, :KBGFOR, :KBGGAR)";
            param[0] = new EacParameter("KBGTYP", type);
            param[1] = new EacParameter("KBGIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KBGALX", version);
            param[3] = new EacParameter("KBGNUM", GetNumInvApp(codeOffre, version, type));
            param[4] = new EacParameter("KBGKBEID", codeInven);
            param[5] = new EacParameter("KBGPERI", PerimetreInventaire.Objet.AsCode());
            param[6] = new EacParameter("KBGRSQ", codeRisque);
            param[7] = new EacParameter("KBGOBJ", codeObjet);
            param[8] = new EacParameter("KBGFOR", "0");
            param[9] = new EacParameter("KBGGAR", "");

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        private static InventaireDto LoadInventaireRisqueObjet(string codeOffre, int version, string type, string codeAvn, int codeRisque, int codeObjet, InventaireDto toReturn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("KBGRSQ", codeRisque),
                new EacParameter("KBGOBJ", codeObjet) ,
                new EacParameter("JERSQ", codeRisque) ,
                new EacParameter("KBETYP", type)      ,
                new EacParameter("KBEALX", version)   ,
                new EacParameter("KBEIPB", codeOffre.PadLeft(9, ' '))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT KBEID CODE, KBEDESC DESCRIPTIF, IFNULL(KADDESI, '') DESCRIPTION,
                            KBEREPVAL REPORTVAL, KBEVAL VALEUR, KBEVAT VALTYPE, KBEVAU VALUNIT, KBEVAH VALTAXE,
                            JEBRA BRANCHE, KABCIBLE CIBLE,KBGRSQ CODERSQ ,PBAVN NUMAVN
                        FROM {0}
                            LEFT JOIN {1} ON KADCHR = KBEKADID
                            INNER JOIN {2} ON KBEID = KBGKBEID AND KBGRSQ = :KBGRSQ AND KBGOBJ = :KBGOBJ
                            INNER JOIN {4} ON JEIPB = KBEIPB AND JEALX = KBEALX AND JERSQ = :JERSQ
                            INNER JOIN {5} ON KABIPB = JEIPB AND KABALX = JEALX AND KABTYP = KBETYP AND KABRSQ = JERSQ
                            INNER JOIN YPOBASE ON PBIPB = JEIPB AND PBALX = JEALX AND PBTYP = KBETYP

                        WHERE KBETYP = :KBETYP AND KBEALX = :KBALX AND KBEIPB = :KBEIPB {3}",
               /*0*/ CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
               /*1*/ CommonRepository.GetPrefixeHisto(modeNavig, "KPDESI"),
               /*2*/ CommonRepository.GetPrefixeHisto(modeNavig, "KPINVAPP"),
               /*3*/ modeNavig == ModeConsultation.Historique ? " AND KBGAVN = :avn" : String.Empty,
               /*4*/ CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
               /*5*/ CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"));


            toReturn = DbBase.Settings.ExecuteList<InventaireDto>(CommandType.Text, sql, param).FirstOrDefault();
            return toReturn;
        }
        public static string SupprimerGarantieInventaire(string codeOffre, string version, string type, string codeFormule, string codeGarantie, string codeInventaire)
        {



            DbParameter[] param = new DbParameter[8];

            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_PERI", PerimetreInventaire.Garantie.AsCode());
            param[4] = new EacParameter("P_CODE_FORMULE", codeFormule);
            param[5] = new EacParameter("P_CODE_GARANTIE", codeGarantie);
            param[6] = new EacParameter("P_CODE_INVEN", 0);
            param[6].Value = Convert.ToInt32(codeInventaire);
            param[7] = new EacParameter("P_ERREUR", "");
            param[7].Direction = ParameterDirection.InputOutput;
            param[7].Size = 128;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELGARANTIEINVEN", param);
            return param[7].Value.ToString();
        }
        public static string SupprimerGarantieListInventaires(string codeOffre, string version, string type, string codeFormule, string codesGaranties, string codesInventaires)
        {
            DbParameter[] param = new DbParameter[8];

            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_PERI", PerimetreInventaire.Garantie.AsCode());
            param[4] = new EacParameter("P_CODE_FORMULE", codeFormule);
            param[5] = new EacParameter("P_CODE_GARANTIES", codesGaranties);
            param[6] = new EacParameter("P_CODE_INVENS", codesInventaires);
            param[7] = new EacParameter("P_ERREUR", "");
            param[7].Direction = ParameterDirection.InputOutput;
            param[7].Size = 128;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELGARLISTINVENTAIRES", param);
            return param[7].Value.ToString();
        }
        public static string SupprimerListInventairesByCodeInventaire(string codesInventaires)
        {
            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("P_CODE_INVENS", codesInventaires);
            param[1] = new EacParameter("P_ERREUR", "");
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].Size = 128;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELLISTINVENTAIRES", param);
            return param[1].Value.ToString();
        }
        #endregion
        #endregion
    }
}
