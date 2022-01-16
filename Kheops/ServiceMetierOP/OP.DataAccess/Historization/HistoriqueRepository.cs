using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Historique;
using OP.WSAS400.DTO.PGM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using Consts = ALBINGIA.Framework.Common.Constants.AlbConstantesMetiers;
using Queries = OP.DataAccess.HistorizationQueries;

namespace OP.DataAccess
{
    public class HistoriqueRepository : IDisposable
    {
        private readonly ConnectionContext dbContext;
        private readonly HistorizationContext context;
        private HistorizationState state;

        public HistoriqueRepository(IDbConnection connection = null, HistorizationContext context = null)
        {
            this.dbContext = new ConnectionContext(connection);
            this.context = context;
        }

        public bool HasItsOwnContext => dbContext.HasItsOwnConnection;

        public static HistoriqueDto GetListHistorique(string codeAffaire, string version, string type, bool contractuel)
        {
            string sql = string.Format(@"
SELECT PBAVN NUMINTERNE, PBAVK NUMEXTERNE, IFNULL(PBREF, '') REFERENCE, IFNULL(KADDESI, '') DESIGNATION, 
	PBAVA * 10000 + PBAVM * 100 + PBAVJ DATEEFFET, PBCRA * 10000 + PBCRM * 100 + PBCRJ DATECREATION, 
	IFNULL(PBAVC, '') MOTIF, IFNULL(Y1.TPLIB, '') LIBMOTIF, IFNULL(PBETA, '') ETAT, IFNULL(Y2.TPLIB, '') LIBETAT, 
	IFNULL(PBSIT, '') SITUATION, IFNULL(Y3.TPLIB, '') LIBSITUATION, 
	IFNULL(PBSTQ, '') QUALITE, IFNULL(Y4.TPLIB, '') LIBQUALITE, 
	IFNULL(PBTAC, '') TYPERETOUR, IFNULL(Y5.TPLIB, '') LIBRETOUR,
	PBTAA * 10000 + PBTAM * 100 + PBTAJ DATERETOUR,
	IFNULL(PBTTR, '') TRAITEMENT, IFNULL(Y6.TPLIB, '') LIBTRAITEMENT
FROM YHPBASE
	INNER JOIN HPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP AND PBAVN = KAAAVN AND PBHIN = KAAHIN
	LEFT JOIN HPDESI ON KADCHR = KAADESI
	LEFT JOIN YYYYPAR Y1 ON Y1.TCON = 'PRODU' AND Y1.TFAM = 'PBAVC' AND Y1.TCOD = PBAVC
	LEFT JOIN YYYYPAR Y2 ON Y2.TCON = 'PRODU' AND Y2.TFAM = 'PBETA' AND Y2.TCOD = PBETA
	LEFT JOIN YYYYPAR Y3 ON Y3.TCON = 'PRODU' AND Y3.TFAM = 'PBSIT' AND Y3.TCOD = PBSIT
	LEFT JOIN YYYYPAR Y4 ON Y4.TCON = 'PRODU' AND Y4.TFAM = 'PBSTQ' AND Y4.TCOD = PBSTQ
	LEFT JOIN YYYYPAR Y5 ON Y5.TCON = 'PRODU' AND Y5.TFAM = 'PBTAC' AND Y5.TCOD = PBTAC
	LEFT JOIN YYYYPAR Y6 ON Y6.TCON = 'PRODU' AND Y6.TFAM = 'PBTTR' AND Y6.TCOD = PBTTR
WHERE PBIPB = '{0}' AND PBALX = {1} AND PBTYP = '{2}'", codeAffaire.PadLeft(9, ' '), version, type);

            if (contractuel)
            {
                sql += " AND PBHIN = 1";
            }
            sql += " ORDER BY PBAVN DESC";

            var result = DbBase.Settings.ExecuteList<HistoriqueLigneDto>(CommandType.Text, sql);

            HistoriqueDto model = new HistoriqueDto
            {
                IsContractuel = contractuel,
                ListHistorique = result
            };

            /* recupere reguleID */
            string sqlReguleId = string.Format(@"SELECT KHWID INT64RETURNCOL, KHWAVN INT32RETURNCOL2 FROM KPRGU WHERE KHWIPB = '{0}' AND KHWALX = {1} AND KHWTYP= '{2}'", codeAffaire.PadLeft(9, ' '), version, type);
            var resultReguleID = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlReguleId);


            for (int i = 0; i < resultReguleID.Count; i++)
            {
                var histo = model.ListHistorique.FirstOrDefault(el => el.NumInterne == resultReguleID[i].Int32ReturnCol2);
                if (histo != null) {
                    histo.ReguleId = resultReguleID[i].Int64ReturnCol.ToString();
                }
            }

            return model;
        }

        public HistorizationState SetState()
        {
            this.state = this.dbContext.Select<HistorizationState>(Queries.SelectBasicState, BuildFolderParams(false)).FirstOrDefault();
            return this.state;
        }

        public bool IsHistoAlreadyDone()
        {
            int count = dbContext.Count(Queries.CountExistingHisto, new DbParameter[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version },
                new EacParameter("TYPEAVT", DbType.AnsiStringFixedLength, 5) { Value = context.TypeAvenant },
                new EacParameter("MODEAVT", DbType.AnsiStringFixedLength, 1) { Value = context.Mode.ToString() },
                new EacParameter("NUMINTERNE", DbType.Int32) { Value = context.NumeroAvenantInterne },
                new EacParameter("AVN", DbType.Int32) { Value = state.NumeroAvenant }
            });
            return count > 0;
        }

        public void PurgeTraces()
        {
            dbContext.Execute(Queries.PurgeTraces, BuildFolderParams(false));
        }

        public void PurgeIds()
        {
            dbContext.Execute(Queries.PurgeHistoIds, BuildFolderParams(false));
        }

        public int GetCodeAdresse()
        {
            int? codeAdresse = this.dbContext.SelectFirstField<int>(Queries.GetCodeAdresse, BuildFolderParams(false));
            if (codeAdresse.GetValueOrDefault() == default(int))
            {
                return 0;
            }
            var outParam = new EacParameter("P_NEWCODEID", DbType.Int32) { Value = 0, Direction = ParameterDirection.InputOutput };
            this.dbContext.Execute(Queries.GetIdCopy, BuildFolderParams().Concat(new[] {
                new EacParameter("P_TABLE", DbType.AnsiStringFixedLength, 10) { Value = "YADRESS" },
                new EacParameter("P_CODEID", DbType.Int32) { Value = codeAdresse.Value },
                outParam
            }));
            int codeAdresseCopy = (outParam.Value == DBNull.Value || outParam.Value == null) ? default(int) : (int)outParam.Value;
            if (codeAdresseCopy == 0)
            {
                outParam = new EacParameter("P_SEQ", DbType.Int32) { Value = 0, Direction = ParameterDirection.InputOutput };
                this.dbContext.ExecuteSP("SP_YCHRONO", new[] {
                    new EacParameter("P_CHAMP", DbType.AnsiStringFixedLength, 40) { Value = "ADRESSE_CHRONO" },
                    outParam
                });
                codeAdresseCopy = (outParam.Value == DBNull.Value || outParam.Value == null) ? default(int) : (int)outParam.Value;
                this.dbContext.ExecuteSP("SP_INCOPID", BuildFolderParams().Concat(new[] {
                    new EacParameter("P_TABLE", DbType.AnsiStringFixedLength, 10) { Value = "YADRESS" },
                    new EacParameter("P_CODEID", DbType.Int32) { Value = codeAdresse.Value },
                    new EacParameter("P_NEWCODEID", DbType.Int32) { Value = codeAdresseCopy }
                }));
                this.dbContext.Execute(Queries.AddAdresseCopy, new[] {
                    new EacParameter("NEWCODEADR", DbType.Int32) { Value = codeAdresseCopy },
                    new EacParameter("LIENADR", DbType.Int32) { Value = codeAdresse.Value }
                });
            }

            return codeAdresseCopy;
        }

        public void SaveDataExtraTablesKheops()
        {
            this.dbContext.ExecuteSP(Queries.CopyExtraTablesKheops, BuildFolderParams().Concat(new[] {
                new EacParameter("P_AVN", DbType.Int32) { Value = state.NumeroAvenant },
                new EacParameter("P_HIN", DbType.Int32) { Value = 1 }
            }));
        }

        public void SaveAffaire()
        {
            var dateNow = DateTime.Now;
            this.dbContext.Execute(Queries.CopyYPOBASE, (new DbParameter[] {
                new EacParameter("AVN", DbType.Int32) { Value = this.state.NumeroAvenant },
                new EacParameter("HIA", DbType.Int32) { Value = dateNow.Year },
                new EacParameter("HIM", DbType.Int32) { Value = dateNow.Month },
                new EacParameter("HIJ", DbType.Int32) { Value = dateNow.Day },
                new EacParameter("HIH", DbType.Int32) { Value = AlbConvert.ToIntHM(dateNow) },
                new EacParameter("NEWCODEADR", DbType.Int32) { Value = this.state.NewCodeAdresse }
            }).Concat(BuildFolderParams(false)));
            this.dbContext.Execute(Queries.CopyYPRTENT, GetAvnIpbAlxParams());
            this.dbContext.Execute(Queries.CopyKPENT, GetAvnIpbAlxTypParams());

            SaveCoassureurs();
            SaveAssures();
            SaveAssuresNonDegignes();
            SaveEcheances();
            SaveIntervenants();
            SaveConnexites();
            SaveCourtiers();
        }

        public void SaveCoassureurs()
        {
            this.dbContext.Execute(Queries.CopyYPOCOAS, GetAvnIpbAlxParams());
        }

        public void SaveAssures()
        {
            this.dbContext.Execute(Queries.CopyYPOASSU, GetAvnIpbAlxParams());
        }

        public void SaveAssuresNonDegignes()
        {
            this.dbContext.Execute(Queries.CopyYPOASSX, GetAvnIpbAlxParams());
        }

        public void SaveEcheances()
        {
            this.dbContext.Execute(Queries.CopyYPOECHE, GetAvnIpbAlxParams());
        }

        public void SaveIntervenants()
        {
            this.dbContext.Execute(Queries.CopyYPOINTE, GetAvnIpbAlxParams());
        }

        public void SaveConnexites()
        {
            this.dbContext.Execute(Queries.CopyYPOCONX, GetAvnIpbAlxTypParams());
        }

        public void SaveCourtiers()
        {
            this.dbContext.Execute(Queries.CopyYPOCOUR, GetAvnIpbAlxParams());
        }

        public void SaveRisques()
        {
            this.dbContext.Execute(Queries.CopyYPRTRSQ, GetAvnIpbAlxParams());
            this.dbContext.Execute(Queries.CopyKPRSQ, GetAvnIpbAlxTypParams());
        }

        public void SaveIS() {
            this.dbContext.Execute(Queries.CopyKPISVAL, GetAvnIpbAlxTypParams());
        }

        public void SaveObjets()
        {
            this.dbContext.Execute(Queries.CopyYPRTOBJ, GetAvnIpbAlxParams());
            this.dbContext.Execute(Queries.CopyKPOBJ, GetAvnIpbAlxTypParams());
            this.dbContext.Execute(Queries.CopyKPIRSOB, GetAvnIpbAlxTypParams());
        }

        public void SaveFormules()
        {
            var affaireParams = GetAvnIpbAlxTypParams();
            this.dbContext.Execute(Queries.CopyKPFOR, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPOPT, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPIOPT, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPOPTD, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPOPTAP, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPGARAN, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPGARTAR, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPGARAP, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPIRSGA, affaireParams.ToArray());
        }

        public void SaveInventaires()
        {
            var affaireParams = GetAvnIpbAlxTypParams();
            this.dbContext.Execute(Queries.CopyKPINVEN, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPINVED, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPINVAPP, affaireParams.ToArray());
        }

        public void SaveClauses()
        {
            this.dbContext.Execute(Queries.CopyKPCLAUSE, GetAvnIpbAlxTypParams());
        }

        public void SaveCotisations()
        {
            this.dbContext.Execute(Queries.CopyKPCOTGA, GetAvnIpbAlxTypParams());
            this.dbContext.Execute(Queries.CopyKPCOTIS, GetAvnIpbAlxTypParams());
        }

        public void SaveControles()
        {
            this.dbContext.Execute(Queries.CopyKPCTRL, GetAvnIpbAlxTypParams());
            this.dbContext.Execute(Queries.CopyKPCTRLE, GetAvnIpbAlxTypParams());
        }

        public void SaveDesignations()
        {
            this.dbContext.Execute(Queries.CopyKPDESI, GetAvnIpbAlxTypParams());
        }

        public void SaveEngagements()
        {
            var affaireParams = GetAvnIpbAlxTypParams();
            this.dbContext.Execute(Queries.CopyKPENG, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPENGFAM, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPENGGAR, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPENGRSQ, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPENGVEN, affaireParams.ToArray());
        }

        public void SaveExpressionsComplexes()
        {
            var affaireParams = GetAvnIpbAlxTypParams();
            this.dbContext.Execute(Queries.CopyKPEXPFRH, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPEXPFRHD, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPEXPLCI, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPEXPLCID, affaireParams.ToArray());
        }

        public void SaveMatrice()
        {
            var affaireParams = GetAvnIpbAlxTypParams();
            this.dbContext.Execute(Queries.CopyKPMATFF, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPMATFL, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPMATFR, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPMATGG, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPMATGL, affaireParams.ToArray());
            this.dbContext.Execute(Queries.CopyKPMATGR, affaireParams.ToArray());
        }

        public void SaveObservations()
        {
            this.dbContext.Execute(Queries.CopyKPOBSV, GetAvnIpbAlxTypParams());
        }

        public void SaveDoublesSaisiesOffres()
        {
            this.dbContext.Execute(Queries.CopyKPODBLS, GetAvnIpbAlxTypParams());
        }

        public void SaveOppositions()
        {
            this.dbContext.Execute(Queries.CopyKPOPP, GetAvnIpbAlxTypParams());
            this.dbContext.Execute(Queries.CopyKPOPPAP, GetAvnIpbAlxTypParams());
        }

        public void SaveValidations()
        {
            this.dbContext.Execute(Queries.CopyKPVALID, GetAvnIpbAlxTypParams());
        }

        public void DeleteQuittancesAnnulees()
        {
            this.dbContext.Execute(Queries.PurgeQuittancesAnnulees, new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version }
            });
        }

        public void SaveAdresses()
        {
            this.dbContext.ExecuteSP(Queries.CopyAdresses, GetFolderAvnParams());
        }

        public void SaveJobSortis() {
            this.dbContext.Execute(Queries.CopyKJOBSORTI, GetAvnIpbAlxTypParams());
        }

        /// <summary>
        /// Remise à zero du flag de modification de la clause dans la version courante.
        /// Les clauses libres modifiée dans la version précédente ne doivent plus être affichées comme modifiées
        /// </summary>
        public void ResetClauses()
        {
            this.dbContext.Execute(Queries.CleanupActeGestionKPCLAUSE, BuildFolderParams(false));
            this.dbContext.Execute(Queries.ResetKPCLAUSE, BuildFolderParams(false));
        }

        public void UpdateAffaireAvenant()
        {
            var dateSit = state.DateSituation.GetValueOrDefault();
            var dateFinEffet = context.DateFinEffetREV.GetValueOrDefault();
            var param = new List<EacParameter> {
                new EacParameter("SIT", DbType.AnsiStringFixedLength, 1) { Value = state.SituationAffaire.ToString() },
                new EacParameter("NUMINTERNE", DbType.Int32) { Value = context.NumeroAvenantInterne },
                new EacParameter("TYPEAVT", DbType.AnsiStringFixedLength, 5) { Value = context.TypeAvenant },
                new EacParameter("YEARNOW", DbType.Int32) { Value = context.Now.Year },
                new EacParameter("MONTHNOW", DbType.Int32) { Value = context.Now.Month },
                new EacParameter("DAYNOW", DbType.Int32) { Value = context.Now.Day },
                new EacParameter("USER", DbType.AnsiStringFixedLength, 10) { Value = context.User },
                new EacParameter("YEAR", DbType.Int32) { Value = context.DateAvenant.Value.Year },
                new EacParameter("MONTH", DbType.Int32) { Value = context.DateAvenant.Value.Month },
                new EacParameter("DAY", DbType.Int32) { Value = context.DateAvenant.Value.Day },
                new EacParameter("NUMAVT", DbType.Int32) { Value = context.Folder.NumeroAvenant },
                new EacParameter("AVC", DbType.AnsiStringFixedLength, 2) { Value = context.TypeAvenant == Consts.TYPE_AVENANT_REGULMODIF ? "M3" : context.MotifAvenant },
                new EacParameter("YEARSIT", DbType.Int32) { Value = dateSit == default(DateTime) ? 0 : dateSit.Year },
                new EacParameter("MONTHSIT", DbType.Int32) { Value = dateSit == default(DateTime) ? 0 : dateSit.Month },
                new EacParameter("DAYSIT", DbType.Int32) { Value = dateSit == default(DateTime) ? 0 : dateSit.Day },
            };
            if (this.context.TypeAvenant == Consts.TYPE_AVENANT_REMISE_EN_VIGUEUR)
            {
                param.AddRange(new[] {
                    new EacParameter("PBFEA", DbType.Int32) { Value = dateFinEffet == default(DateTime) ? 0 : dateFinEffet.Year },
                    new EacParameter("PBFEM", DbType.Int32) { Value = dateFinEffet == default(DateTime) ? 0 : dateFinEffet.Month },
                    new EacParameter("PBFEJ", DbType.Int32) { Value = dateFinEffet == default(DateTime) ? 0 : dateFinEffet.Day },
                    new EacParameter("PBFEH", DbType.Int32) { Value = dateFinEffet == default(DateTime) ? 0 : dateFinEffet.Hour * 100 + dateFinEffet.Minute }
                });
            }

            param.AddRange(new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version },
                new EacParameter("TYPE", DbType.AnsiStringFixedLength, 1) { Value = context.Folder.Type }
            });

            this.dbContext.Execute(this.context.TypeAvenant == Consts.TYPE_AVENANT_REMISE_EN_VIGUEUR ? Queries.UpdateAvnREVYPOBASE : Queries.UpdateAvnYPOBASE, param);
            this.dbContext.Execute(Queries.UpdateAvnYPRTENT, new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version }
            });
        }

        public void UpdateAffaireRegul()
        {
            this.dbContext.Execute(Queries.UpdateRegulYPOBASE, new[] {
                new EacParameter("NUMINTERNE", DbType.Int32) { Value = context.NumeroAvenantInterne },
                new EacParameter("TYPEAVT", DbType.AnsiStringFixedLength, 5) { Value = context.TypeAvenant },
                new EacParameter("YEARNOW", DbType.Int32) { Value = context.Now.Year },
                new EacParameter("MONTHNOW", DbType.Int32) { Value = context.Now.Month },
                new EacParameter("DAYNOW", DbType.Int32) { Value = context.Now.Day },
                new EacParameter("USER", DbType.AnsiStringFixedLength, 10) { Value = context.User },
                new EacParameter("NUMAVT", DbType.Int32) { Value = context.Folder.NumeroAvenant },
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version },
                new EacParameter("TYPE", DbType.AnsiStringFixedLength, 1) { Value = context.Folder.Type }
            });

            this.dbContext.Execute(Queries.UpdateAvnYPRTENT, new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version }
            });
        }

        public void UpdateAffaireResil()
        {
            this.dbContext.Execute(Queries.UpdateResilYPOBASE, new[] {
                new EacParameter("NUMINTERNE", DbType.Int32) { Value = this.context.NumeroAvenantInterne },
                new EacParameter("TYPEAVT", DbType.AnsiStringFixedLength, 5) { Value = this.context.TypeAvenant },
                new EacParameter("YEARNOW", DbType.Int32) { Value = this.context.Now.Year },
                new EacParameter("MONTHNOW", DbType.Int32) { Value = this.context.Now.Month },
                new EacParameter("DAYNOW", DbType.Int32) { Value = this.context.Now.Day },
                new EacParameter("YEARRSL", DbType.Int32) { Value = (this.state.DateResiliation?.Year ?? 0) },
                new EacParameter("MONTHRSL", DbType.Int32) { Value = (this.state.DateResiliation?.Month ?? 0) },
                new EacParameter("DAYRSL", DbType.Int32) { Value = (this.state.DateResiliation?.Day ?? 0) },
                new EacParameter("HEURERSL", DbType.Int32) { Value = this.state.DateResiliation.ToIntHM() },
                new EacParameter("USER", DbType.AnsiStringFixedLength, 10) { Value = this.context.User },
                new EacParameter("YEARAVN", DbType.Int32) { Value = this.context.DateAvenant.Value.Year },
                new EacParameter("MONTHAVN", DbType.Int32) { Value = this.context.DateAvenant.Value.Month },
                new EacParameter("DAYAVN", DbType.Int32) { Value = this.context.DateAvenant.Value.Day },
                new EacParameter("NUMAVT", DbType.Int32) { Value = this.context.Folder.NumeroAvenant },
                new EacParameter("MOTIF", DbType.AnsiStringFixedLength, 2) { Value = this.context.MotifAvenant },
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = this.context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = this.context.Folder.Version },
                new EacParameter("TYPE", DbType.AnsiStringFixedLength, 1) { Value = this.context.Folder.Type }
            });

            this.dbContext.Execute(Queries.UpdateAvnYPRTENT, new[] {
                new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = this.context.Folder.CodeOffre.ToIPB() },
                new EacParameter("P_VERSION", DbType.Int32) { Value = this.context.Folder.Version }
            });
        }

        public void UpdateMotifResiliation() {
            this.dbContext.Execute(Queries.UpdateMotifResil, new[] {
                new EacParameter("MOTIF", this.context.MotifAvenant),
            }.Concat(BuildFolderParams(false)));
        }

        public void UpdateObservationsAvn()
        {
            var codes = this.dbContext.Select<(long kaaavds, long kaaobsv)>(Queries.GetKpentDcsObs, BuildFolderParams(false)).FirstOrDefault();
            codes.kaaavds = AddOrUpdateDescription(codes.kaaavds);
            this.dbContext.Execute(Queries.UpdateKpentDcsObs, (new[] {
                new EacParameter("CODEDESCR", DbType.Int32) { Value = codes.kaaavds },
            }).Concat(BuildFolderParams(false)));
        }

        public long AddOrUpdateDescription(long code)
        {
            if (code == 0)
            {
                code = this.dbContext.GetNextId("KADCHR");
                this.dbContext.Execute(Queries.AddDesignation, BuildFolderParams(false).Concat(new[] {
                    new EacParameter("CHR", DbType.Int32) { Value = (int)code },
                    new EacParameter("DESI", DbType.AnsiStringFixedLength, 5000) { Value = context.Description ?? string.Empty }
                }));
            }
            else
            {
                this.dbContext.Execute(Queries.UpdateDesignation, new[] {
                    new EacParameter("DESI", DbType.AnsiStringFixedLength, 5000) { Value = context.Description ?? string.Empty },
                    new EacParameter("CHR", DbType.Int32) { Value = (int)code }
                });
            }
            return code;
        }

        public long AddOrUpdateObservation(long code)
        {
            if (code == 0)
            {
                code = this.dbContext.GetNextId("KAJCHR");
                this.dbContext.Execute(Queries.AddObservations, BuildFolderParams(false).Concat(new[] {
                    new EacParameter("CHR", DbType.Int32) { Value = code },
                    new EacParameter("OBSERVATIONS", DbType.AnsiStringFixedLength, 5000) { Value = context.Observations ?? string.Empty }
                }));
            }
            else
            {
                this.dbContext.Execute(Queries.UpdateObservations, new[] {
                    new EacParameter("OBSERVATIONS", DbType.AnsiStringFixedLength, 5000) { Value = context.Observations ?? string.Empty },
                    new EacParameter("CHR", DbType.Int32) { Value = (int)code }
                });
            }
            return code;
        }

        public void UpdateFraisAvn()
        {
            int codeFrais = Convert.ToInt32(this.dbContext.SelectFirstField<decimal>(Queries.GetFrais, new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version }
            }));
            this.dbContext.Execute(Queries.UpdateKpentFrais, (new[] {
                new EacParameter("HEUREAVN", DbType.Int32) { Value = context.DateAvenant.ToIntHMS() },
                new EacParameter("CODEFRAIS", DbType.Int32) { Value = codeFrais }
            }).Concat(BuildFolderParams(false)));
        }

        public void UpdateKpentRemiseEnVigueur()
        {
            int codeFrais = Convert.ToInt32(this.dbContext.SelectFirstField<decimal>(Queries.GetFrais, new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version }
            }));
            this.dbContext.Execute(Queries.UpdateKpentRemiseEnVigueur, (new[] {
                new EacParameter("HEUREAVN", DbType.Int32) { Value = context.DateAvenant.ToIntHMS() },
                new EacParameter("CODEFRAIS", DbType.Int32) { Value = codeFrais },
                new EacParameter("DATEFINGARANTIE", DbType.Int32) { Value = context.DateFinGarantie.ToIntYMD() },
                new EacParameter("HEUREFINGARANTIE", DbType.Int32) { Value = context.DateFinGarantie.ToIntHM() }
            }).Concat(BuildFolderParams(false)));
        }

        public void UpdateDatesRsqObjGarAvn()
        {
            this.dbContext.ExecuteSP(Queries.SetDatesAvn, BuildFolderParams());
        }



        public void ReleaseGaranties()
        {
            this.dbContext.ExecuteSP(Queries.LibererGaranties, BuildFolderParams().Concat(new[] {
                new EacParameter("P_DATEDEBAVT", DbType.Int32) { Value = context.DateAvenant.Value.ToIntYMD() }
            }));
        }

        public void AdjustSuspensions(Folder folder)
        {
            //Call du PGM KDA196 pour mettre à jour la table de suspension KPSUSP
            var initParameter = new RemiseEnVigueurParams()
            {
                Result = "M",
                CodeContrat = folder.CodeOffre,
                Version = folder.Version,
                Type = folder.Type
            };
            RemiseEnVigueurRepository.CallKDA196(initParameter);
        }

        public void PurgeEcheanciers()
        {
            this.dbContext.Execute(Queries.PurgeEcheanciers, BuildFolderParams(false));
        }

        public void PurgeClausesRegul()
        {
            this.dbContext.Execute(Queries.PurgeClausesRegul, BuildFolderParams(false).Concat(new[] {
                new EacParameter("ETAPE", DbType.AnsiStringFixedLength, 10) { Value = Consts.TypeClauseRegul }
            }));
        }

        public void PurgePrimes()
        {
            this.dbContext.Execute(Queries.PurgePrimes, new[] {
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = context.Folder.Version }
            }, true);
        }

        public void PurgeTempWordDocs()
        {
            this.dbContext.Execute(Queries.PurgeTempWordDocs, GetIpbAlxTypAvnParams());
        }

        public void RollbackChanges()
        {
            this.dbContext.EndTransaction(false);
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        private IEnumerable<DbParameter> BuildFolderParams(bool isForSP = true)
        {
            string prefix = isForSP ? "P_" : string.Empty;
            return new DbParameter[] {
                new EacParameter($"{prefix}CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = this.context.Folder.CodeOffre.ToIPB() },
                new EacParameter($"{prefix}VERSION", DbType.Int32) { Value = this.context.Folder.Version },
                new EacParameter($"{prefix}TYPE", DbType.AnsiStringFixedLength, 1) { Value = this.context.Folder.Type }
            };
        }

        private IEnumerable<DbParameter> GetAvnIpbAlxParams()
        {
            return new DbParameter[] {
                new EacParameter("AVN", DbType.Int32) { Value = state.NumeroAvenant },
                new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength, 9) { Value = this.context.Folder.CodeOffre.ToIPB() },
                new EacParameter("VERSION", DbType.Int32) { Value = this.context.Folder.Version }
            };
        }

        private IEnumerable<DbParameter> GetAvnIpbAlxTypParams()
        {
            return (new DbParameter[] {
                new EacParameter("AVN", DbType.Int32) { Value = this.state.NumeroAvenant }
            }).Concat(BuildFolderParams(false));
        }

        private IEnumerable<DbParameter> GetFolderAvnParams()
        {
            ICollection<DbParameter> parameters = BuildFolderParams().ToList();
            parameters.Add(new EacParameter("P_AVN", DbType.Int32) { Value = this.state.NumeroAvenant });
            return parameters;
        }

        private IEnumerable<DbParameter> GetIpbAlxTypAvnParams()
        {
            return BuildFolderParams(false).Concat(new DbParameter[] {
                new EacParameter("AVN", DbType.Int32) { Value = this.state.NumeroAvenant }
            });
        }
    }
}
