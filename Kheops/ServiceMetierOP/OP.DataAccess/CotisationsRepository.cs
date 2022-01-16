using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Cotisations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess
{
    public class CotisationsRepository
    {
        #region Méthodes Publiques
        public static CotisationsDto InitCotisations(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, bool onlyGarPorteuse = false, string typePart = "T")
        {
            CotisationsDto toReturn = new CotisationsDto
            {
                GarantiesInfo = new CotisationInfoGarantieDto()
            };
            //KAAATGBAS BASEATTENTAT, KAAATGTFA TAUXCOTISATTENTAT, KDMATGMHT COTISATTENTATHT, KDMATGMTX COTISATTENTATTAXE, KDMATGMTT COTISATTENTATTTC,
            string sql = string.Format(@"SELECT KDMGARMHT SUBCOTISHT, KDMGARMTX SUBCOTISTAXE, KDMGARMTT SUBCOTISTTC,
                                KDMCNABAS CATNAT, KDMCNAMHT COTISCATNATHT, KDMCNAMTX COTISCATNATTAXE, KDMCNAMTT COTISCATNATTTC,
                                KDMCOM TAUXCOMHORSCATNATSTD, KDMCNACNC TAUXCOMCATNATSTD, KDMCOT MONTANTCOMHORSCATNATSTD, KDMCNACNM MONTANTCOMCATNATSTD, KDMCNATXF TAUXCOMCATNATFOR,
                                KDMHFMHF TOTALHORSFRAISHT1, KDMHFMHT TOTALHORSFRAISHT2, KDMHFMTX TAXETOTALHORSFRAIS,
                                KDMHFMTT TOTALHORSFRAISTTC, KDMAFR FRAISHT, KDMAFT TAXEFRAIS, KDMFGA TAUXTAXEFGA, KDMFGA MONTANTTAXEFGA,
                                KDMMHT MONTANTTOTALHT1, KDMMHF MONTANTTOTALHT2, KDMMTX TAXEMONTANTTOTAL, KDMTTF MONTANTTOTALTTC1, KDMMTT MONTANTTOTALTTC2,
                                KDMCOF MONTANTCOMHORSCATNATFOR, KDMCMF TAUXCOMHORSCATNATFOR, KDMCNACMF MONTANTCOMCATNATFOR, IFNULL(KAJOBSV, '') COMMENTFORCE,
                                KDMTAP TYPEPART, PBPER TYPEPERIODE ,PBNPL NATURECONTRAT
                            FROM {0}
                                INNER JOIN {1} ON KDMIPB = KAAIPB AND KDMALX = KAAALX AND KDMTYP = KAATYP {8}
                                INNER JOIN YPOBASE ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                                LEFT JOIN {2} ON KDOIPB = KAAIPB AND KDOALX = KAAALX AND KDOTYP = KAATYP {9}
	                            LEFT JOIN {3} ON KAJCHR = KAAOBSC WHERE KDMIPB='{4}' AND KDMALX='{5}' AND KDMTYP='{6}' {7} AND KDMTAP = '{10}'",
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPCOTIS"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPENG"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOBSV"),
                                codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), type,
                                modeNavig == ModeConsultation.Historique ? string.Format(" AND KDMAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KDMAVN = KAAAVN" : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KDOAVN = KAAAVN" : string.Empty,
                                typePart);
            var result = DbBase.Settings.ExecuteList<CotisationsPlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var cotisationPlatDto = result.FirstOrDefault();
                toReturn.GarantiesInfo = GetCotisationInfoGarantieDto(codeOffre, version, type, cotisationPlatDto, onlyGarPorteuse);
                toReturn.MontantForce = cotisationPlatDto.MontantForce == "O";
                toReturn.CommentForce = cotisationPlatDto.CommentForce;
            }
            else
            {
                toReturn.GarantiesInfo = new CotisationInfoGarantieDto { TypePart = typePart, TypePeriode = "A" };
            }

            #region Trace RC

            DbParameter[] paramRC = new DbParameter[3];
            paramRC[0] = new EacParameter("codeAffaire", codeOffre.Trim().PadLeft(9, ' '));
            paramRC[1] = new EacParameter("version", 0);
            paramRC[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            paramRC[2] = new EacParameter("type", type);

            string sqlTraceRC = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC WHERE KHOIPB = :codeAffaire AND KHOALX = :version AND KHOTYP = :type AND KHOOEF = 'CC'";

            toReturn.TraceCC = CommonRepository.ExistRowParam(sqlTraceRC, paramRC);

            #endregion

            return toReturn;
        }

        public static void UpdateCotisations(string codeOffre, string version, string type, CotisationsDto cotisationsDto)
        {
            UpdateKpCotis(codeOffre, version, type, cotisationsDto);
            foreach (CotisationGarantieDto garantie in cotisationsDto.GarantiesInfo.Garanties)
            {
                if (garantie.Tarif != null)
                    UpdateKpCotga(codeOffre, version, type, garantie);
            }

            SaveCommentaireCotisation(codeOffre, version, type, cotisationsDto.CommentForce);

            UpdateEntete(codeOffre, version, type, cotisationsDto);
        }

        public static CotisationsInfoTarifDto LoadCotisationsTarif(string lienKpgaran)
        {
            CotisationsInfoTarifDto toReturn = new CotisationsInfoTarifDto();

            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("KDGKDEID", 0);
            param[0].Value = !string.IsNullOrEmpty(lienKpgaran) ? Convert.ToInt32(lienKpgaran) : 0;

            string sql = @"
SELECT KDEGARAN NOMGARANTIE, KDGNUMTAR TARIF, KDGLCIVALA LCIVALEUR, KDGLCIUNIT LCIUNITE, 
KDGFRHVALA FRANCHISEVALEUR, KDGFRHUNIT FRANCHISEUNITE, KDGPRIVALA PRIMEVALEUR, KDGPRIUNIT PRIMEUNITE
FROM KPGARTAR
INNER JOIN KPGARAN ON KDGKDEID = KDEID
WHERE KDGKDEID = :KDGKDEID";
            var cotisationsInfoTarifPlatDto = DbBase.Settings.ExecuteList<CotisationsInfoTarifPlatDto>(CommandType.Text, sql, param);
            if (cotisationsInfoTarifPlatDto.Any())
            {
                toReturn.NomGarantie = cotisationsInfoTarifPlatDto[0].NomGarantie;
                List<CotisationsTarifDto> tarifs = new List<CotisationsTarifDto>();
                cotisationsInfoTarifPlatDto.ForEach(cot => tarifs.Add(new CotisationsTarifDto
                {
                    CodeTarif = cot.CodeTarif.ToString(),
                    FranchiseUnite = cot.FranchiseUnite,
                    FranchiseValeur = cot.FranchiseValeur.ToString(),
                    LCIUnite = cot.LCIUnite,
                    LCIValeur = cot.LCIValeur.ToString(),
                    TauxUnite = cot.TauxUnite,
                    TauxValeur = cot.TauxValeur.ToString()
                }));
                toReturn.Tarifs = tarifs;
            }
            return toReturn;
        }

        public static List<CotisationGarantieDto> GetCotisationGaranties(string codeOffre, string version, string type, bool onlyGarPorteuse = false)
        {
            List<CotisationGarantieDto> toReturn = new List<CotisationGarantieDto>();

            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_ONLYGARPORTEUSE", onlyGarPorteuse ? "O" : "N");

            var cotisationGarantieExecListDto = DbBase.Settings.ExecuteList<CotisationGarantieExecListDto>(CommandType.StoredProcedure, "SP_GETCOTISATIONGARANTIES", param);

            if (cotisationGarantieExecListDto.Any())
                cotisationGarantieExecListDto.ForEach(cot => toReturn.Add(new CotisationGarantieDto
                {
                    AssietteUnite = cot.AssietteUnite,
                    AssietteValeur = cot.AssietteValeur.ToString(),
                    CodeFormule = cot.CodeFormule,
                    CodeGarantie = cot.CodeGarantie,
                    CodeRisque = cot.CodeRisque.ToString(),
                    CotisationHT = cot.CotisationHT.ToString(),
                    CotisationTaxe = cot.CotisationTaxe.ToString(),
                    CotisationTTC = cot.CotisationTTC.ToString(),
                    LCIUnite = cot.LCIUnite,
                    LCIValeur = cot.LCIValeur.ToString(),
                    LienKpgaran = cot.LienKpgaran.ToString(),
                    NomGarantie = cot.NomGarantie,
                    Tarif = cot.Tarif.ToString(),
                    TauxUnite = cot.TauxUnite,
                    TauxValeur = cot.TauxValeur.ToString()
                }));

            return toReturn;
        }

        public static void SaveCommentaireCotisation(string codeOffre, string version, string type, string commentaire)
        {
            string sql = string.Format(@"SELECT KAAOBSC ID FROM KPENT WHERE KAAIPB = '{0}' AND KAAALX = {1} AND KAATYP = '{2}'", codeOffre.PadLeft(9, ' '), version, type);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Count > 0)
            {
                if (result.FirstOrDefault().Id != 0)
                {
                    DbParameter[] paramUpd = new DbParameter[2];
                    paramUpd[0] = new EacParameter("KAJOBSV", commentaire);
                    paramUpd[1] = new EacParameter("KAJCHR", 0);
                    paramUpd[1].Value = result.FirstOrDefault().Id;

                    string sqlUpdate = @"UPDATE KPOBSV SET KAJOBSV = :KAJOBSV WHERE KAJCHR = :KAJCHR";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate, paramUpd);
                }
                else
                {
                    var newId = CommonRepository.GetAS400Id("KAJCHR");
                    DbParameter[] paramIns = new DbParameter[5];
                    paramIns[0] = new EacParameter("KAJCHR", 0);
                    paramIns[0].Value = newId;
                    paramIns[1] = new EacParameter("KAJIPB", codeOffre.PadLeft(9, ' '));
                    paramIns[2] = new EacParameter("KAJALX", 0);
                    paramIns[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                    paramIns[3] = new EacParameter("KAJTYP", type);
                    paramIns[4] = new EacParameter("KAJOBSV", commentaire);

                    string sqlInsert = @"INSERT INTO KPOBSV (KAJCHR, KAJIPB, KAJALX, KAJTYP, KAJOBSV) VALUES (:KAJCHR, :KAJIPB, :KAJALX, :KAJTYP, :KAJOBSV)";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsert, paramIns);

                    DbParameter[] paramUpd = new DbParameter[4];
                    paramUpd[0] = new EacParameter("KAAOBSC", 0);
                    paramUpd[0].Value = newId;
                    paramUpd[1] = new EacParameter("KAAIPB", codeOffre.PadLeft(9, ' '));
                    paramUpd[2] = new EacParameter("KAAALX", 0);
                    paramUpd[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                    paramUpd[3] = new EacParameter("KAATYP", type);

                    string sqlUpdate = @"UPDATE KPENT SET KAAOBSC = :KAAOBSC WHERE KAAIPB = :KAAIPB AND KAAALX = :KAAALX AND KAATYP = :KAATYP";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate, paramUpd);
                }
            }
        }

        public static void CheckCodeTaxeGarantie(string codeAffaire, string version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            param[0] = new EacParameter("P_CODEAFFAIRE", codeAffaire.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CHANGECODETAXE", param);
        }

        public static void SaveTraceArbreCotisationAffnouv(string codeOffre, string version, string type, string user)
        {
            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                NumeroOrdreDansEtape = 70,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
        }

        #endregion

        #region Méthodes Privées

        private static void UpdateEntete(string codeOffre, string version, string type, CotisationsDto cotisationsDto)
        {
            double tauxForce = 0;
            if (cotisationsDto.GarantiesInfo != null && cotisationsDto.GarantiesInfo.Commission != null && double.TryParse(cotisationsDto.GarantiesInfo.Commission.TauxForce, out tauxForce))
            {
                //                string sqlyprtent = string.Format(@"UPDATE YPRTENT 
                //                                             SET JDXCM = '{0}' 
                //                                             WHERE TRIM(JDIPB) = TRIM('{1}') AND JDALX = {2}", tauxForce.ToString().Replace(",", "."), codeOffre, version);
                //                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlyprtent);
                //                string sqlkpent = string.Format(@"UPDATE KPENT SET KAAXCMS = '{0}'
                //                                                  WHERE TRIM(KAAIPB) = TRIM('{1}') AND KAATYP = '{2}' AND KAAALX = {3}", tauxForce >= 0 ? "N" : "O", codeOffre, type, version);
                //                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlkpent);


                DbParameter[] paramYprtent = new DbParameter[3];
                paramYprtent[0] = new EacParameter("JDXCM", 0);
                paramYprtent[0].Value = tauxForce;
                paramYprtent[1] = new EacParameter("JDIPB", codeOffre.Trim().PadLeft(9, ' '));
                paramYprtent[2] = new EacParameter("JDALX", 0);
                paramYprtent[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;

                string sqlYprtent = @"UPDATE YPRTENT SET JDXCM = :JDXCM WHERE JDIPB = :JDIPB AND JDALX = :JDALX";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlYprtent, paramYprtent);

                DbParameter[] paramKpent = new DbParameter[4];
                paramKpent[0] = new EacParameter("TAUXFORCE", tauxForce);
                paramKpent[1] = new EacParameter("KAAIPB", codeOffre.Trim().PadLeft(9, ' '));
                paramKpent[2] = new EacParameter("KAAALX", 0);
                paramKpent[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                paramKpent[3] = new EacParameter("KAATYP", type);

                string sqlKpent = @"UPDATE KPENT SET KAAXCMS = (SELECT CASE WHEN  :TAUXFORCE <> c.KDMCOM   THEN 'N'
								                                ELSE 'O'
   								                                END FROM KPCOTIS C
 							                                    WHERE C.KDMIPB = KAAIPB AND C.KDMTYP = KAATYP AND  C.KDMALX = KAAALX AND C.KDMTAP = 'T') 
                                   WHERE KAAIPB = :KAAIPB AND KAAALX = :KAAALX AND KAATYP = :KAATYP ";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlKpent, paramKpent);
            }
        }

        private static void UpdateKpCotis(string codeOffre, string version, string type, CotisationsDto cotisationsDto)
        {
            string sql = @"UPDATE KPCOTIS SET KDMCOF = :montantForce, 
                                KDMHFMHF = :totalHFHT, KDMMHF = :totalHT, KDMTTF = :totalTTC
                                        WHERE KDMIPB = :codeOffre AND KDMALX = :version AND KDMTYP = :type AND KDMTAP = 'T'";

            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("montantForce", 0);
            param[0].Value = Convert.ToDecimal(cotisationsDto.GarantiesInfo.Commission.MontantForce);

            param[1] = new EacParameter("totalHFHT", 0);
            param[1].Value = Convert.ToDecimal(cotisationsDto.GarantiesInfo.TotalHorsFraisHT);
            param[2] = new EacParameter("totalHT", 0);
            param[2].Value = Convert.ToDecimal(cotisationsDto.GarantiesInfo.TotalHT);

            Decimal totalTTC = 0;
            param[3] = new EacParameter("totalTTC", 0);
            param[3].Value = Decimal.TryParse(cotisationsDto.GarantiesInfo.TotalTTC, out totalTTC) ? totalTTC : 0;

            param[4] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[5] = new EacParameter("version", 0);
            param[5].Value = Convert.ToInt32(version);
            param[6] = new EacParameter("type", type);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        private static void UpdateKpCotga(string codeOffre, string version, string type, CotisationGarantieDto garantiesDto)
        {
            DbParameter[] paramUpd = new DbParameter[5];
            paramUpd[0] = new EacParameter("KDNNUMTAR", 0);
            paramUpd[0].Value = !string.IsNullOrEmpty(garantiesDto.Tarif) ? Convert.ToInt32(garantiesDto.Tarif) : 0;
            paramUpd[1] = new EacParameter("KDNIPB", codeOffre.PadLeft(9, ' '));
            paramUpd[2] = new EacParameter("KDNALX", 0);
            paramUpd[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            paramUpd[3] = new EacParameter("KDNTYP", type);
            paramUpd[4] = new EacParameter("KDNGARAN", garantiesDto.CodeGarantie);

            string sqlUpd = @"UPDATE KPCOTGA SET KDNNUMTAR = :KDNNUMTAR WHERE KDNIPB = :KDNIPB AND KDNALX = :KDNALX AND KDNTYP = :KDNTYP AND KDNGARAN = :KDNGARAN";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd, paramUpd);
        }
        private static CotisationInfoGarantieDto GetCotisationInfoGarantieDto(string codeOffre, string version, string type, CotisationsPlatDto cotisationPlatDto, bool onlyGarPorteuse)
        {
            CotisationInfoGarantieDto cotisationInfoGarantieDto = new CotisationInfoGarantieDto();
            cotisationInfoGarantieDto.SubCotisationHT = cotisationPlatDto.SubCotisationHT.ToString();
            cotisationInfoGarantieDto.SubCotisationTaxe = cotisationPlatDto.SubCotisationTaxe.ToString();
            cotisationInfoGarantieDto.SubCotisationTTC = cotisationPlatDto.SubCotisationTTC.ToString();

            CotisationCanatGareatDto Catnat = new CotisationCanatGareatDto();
            Catnat.AssietteValeur = cotisationPlatDto.CatnatAssietteValeur.ToString();
            Catnat.CotisationHT = cotisationPlatDto.CatnatCotisationHT.ToString();
            Catnat.CotisationTaxe = cotisationPlatDto.CatnatCotisationTaxe.ToString();
            Catnat.CotisationTTC = cotisationPlatDto.CatnatCotisationTTC.ToString();
            cotisationInfoGarantieDto.Catnat = Catnat;

            //CotisationCanatGareatDto Gareat = new CotisationCanatGareatDto();
            //Gareat.AssietteValeur = cotisationPlatDto.GareatAssietteValeur.ToString();
            //Gareat.TauxValeur = cotisationPlatDto.GareatTauxValeur.ToString();
            //Gareat.TauxUnite = "%";
            //Gareat.CotisationHT = cotisationPlatDto.GareatCotisationHT.ToString();
            //Gareat.CotisationTaxe = cotisationPlatDto.GareatCotisationTaxe.ToString();
            //Gareat.CotisationTTC = cotisationPlatDto.GareatCotisationTTC.ToString();
            //cotisationInfoGarantieDto.Gareat = Gareat;

            CotisationCommissionDto Commission = new CotisationCommissionDto();
            Commission.TauxStd = cotisationPlatDto.CommissionTauxStd.ToString();
            Commission.MontantStd = Convert.ToDecimal(cotisationPlatDto.CommissionMontantStd);
            Commission.TauxForce = cotisationPlatDto.CommissionTauxForce.ToString();
            Commission.MontantForce = Convert.ToDecimal(cotisationPlatDto.ComissionMontantForce);
            Commission.TauxStdCatNat = cotisationPlatDto.ComissionTauxStdCatNat.ToString();
            Commission.MontantStdCatNat = Convert.ToDecimal(cotisationPlatDto.ComissionMontantStdCatNat);
            Commission.TauxForceCatNat = cotisationPlatDto.ComissionTauxForceCatNat.ToString();
            Commission.MontantForceCatNat = Convert.ToDecimal(cotisationPlatDto.ComissionMontantForceCatNat);
            cotisationInfoGarantieDto.Commission = Commission;

            cotisationInfoGarantieDto.CoefCom = cotisationPlatDto.CoefCom.ToString();
            cotisationInfoGarantieDto.TotalHorsFraisHT = Convert.ToDecimal(cotisationPlatDto.TotalHorsFraisHT1) != 0 ? Convert.ToDecimal(cotisationPlatDto.TotalHorsFraisHT1) : Convert.ToDecimal(cotisationPlatDto.TotalHorsFraisHT2);
            cotisationInfoGarantieDto.TotalHorsFraisTaxe = Convert.ToDecimal(cotisationPlatDto.TotalHorsFraisTaxe);
            cotisationInfoGarantieDto.TotalHorsFraisTTC = Convert.ToDecimal(cotisationPlatDto.TotalHorsFraisTTC);
            cotisationInfoGarantieDto.FraisHT = Convert.ToDecimal(cotisationPlatDto.FraisHT);
            cotisationInfoGarantieDto.FraisTaxe = Convert.ToDecimal(cotisationPlatDto.FraisTaxe);
            cotisationInfoGarantieDto.FGATaxe = Convert.ToDecimal(cotisationPlatDto.FGATaxe);
            cotisationInfoGarantieDto.FGATTC = cotisationPlatDto.FGATTC.ToString();
            cotisationInfoGarantieDto.TotalHT = Convert.ToDecimal(cotisationPlatDto.TotalHT1) != 0 ? Convert.ToDecimal(cotisationPlatDto.TotalHT1) : Convert.ToDecimal(cotisationPlatDto.TotalHT2);
            cotisationInfoGarantieDto.TotalTaxe = cotisationPlatDto.TotalTaxe.ToString();
            cotisationInfoGarantieDto.TotalTTC = cotisationPlatDto.TotalTTC1 != 0 ? cotisationPlatDto.TotalTTC1.ToString() : cotisationPlatDto.TotalTTC2.ToString();

            cotisationInfoGarantieDto.Garanties = GetCotisationGaranties(codeOffre, version, type, onlyGarPorteuse);
            cotisationInfoGarantieDto.TypePart = cotisationPlatDto.TypePart;
            cotisationInfoGarantieDto.TypePeriode = cotisationPlatDto.TypePeriode;
            cotisationInfoGarantieDto.NatureContrat = cotisationPlatDto.NatureContrat;
            return cotisationInfoGarantieDto;
        }

        public static void SetTauxCom(string codeOffre, string version, string type)
        {
            string sql = $@"SELECT KDMCOM TAUXCOMHORSCATNATSTD, KDMCMF TAUXCOMHORSCATNATFOR, KDMCNACNC TAUXCOMCATNATSTD  FROM KPCOTIS 
                            WHERE KDMIPB = '{codeOffre.ToIPB()}' AND KDMALX = {version} AND KDMTYP = '{type}'";
            var result = DbBase.Settings.ExecuteList<CotisationsPlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var cotisationPlatDto = result.FirstOrDefault();

                string sqlUpd = $@"UPDATE YPRTENT SET JDXCM = {(cotisationPlatDto.CommissionTauxForce != 0 ? cotisationPlatDto.CommissionTauxForce : cotisationPlatDto.CommissionTauxStd).ToString().Replace(",",".")}, JDCNC = {cotisationPlatDto.ComissionTauxStdCatNat.ToString().Replace(",", ".")}
                                WHERE JDIPB = '{codeOffre.ToIPB()}' AND JDALX = {version}";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd);
            }
        }

        #endregion

    }
}
