using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess.Data;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.PGM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess
{
    public class ProgramAS400Repository : RepositoryBase
    {
        internal static readonly string PGM_KA000CL = "*PGM/KA000CL";
        internal static readonly string PGM_KDP025CL = "*PGM/KDP025CL";
        internal static readonly string PGM_KDP025CL1 = "*PGM/KDP025CL1";
        internal static readonly string PGM_KDP192CL = "*PGM/KDP192CL";
        internal static readonly string PGM_KDP021CL = "*PGM/KDP021CL";
        internal static readonly string PGM_KA050CL = "*PGM/KA050CL";
        internal static readonly string PGM_KA040CL = "*PGM/KA040CL";
        internal static readonly string GetInfoBaseContract = @"
SELECT PBEFA DATEEFFETA, PBEFM DATEEFFETM, PBEFJ DATEEFFETJ, JDAFR FRAISACC, JDATT ATTENTAT,JDAFC APPLICATIONACC,JDAFR MONTANTFRAIS
FROM YPOBASE 
INNER JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
WHERE PBIPB = :IPB AND PBALX = :ALX AND PBTYP = :TYP";
        internal static readonly string PGM_YDP100 = "*PGM/YDP100"; 
        internal static readonly string PGM_YDP101 = "*PGM/YDP101";
        internal static readonly string PGM_KPR290CL = "*PGM/KPR290CL";
        internal static readonly string PGM_KDP196 = "*PGM/KDP196"; 
        internal static readonly string PGM_KDP196CL = "*PGM/KDP196CL";
        internal static readonly string PGM_PRA606CLST = "*PGM/PRA606CLST";

        internal static readonly string PGM_KP081CL = "*PGM/KP081CL";
        internal static readonly string PGM_ALYDH105CL = "*PGM/ALYDH105CL";
        internal static readonly string PGM_KA010CL = "*PGM/KA010CL";
        internal static readonly string PGM_KA030CL = "*PGM/KA030CL";
        internal static readonly string PGM_KA032CL = "*PGM/KA032CL";
        internal static readonly string PGM_KA035CL = "*PGM/KA035CL";
        internal static readonly string PGM_KDA196 = "*PGM/KDA196";
        internal static readonly string PGM_KDA290CL = "*PGM/KDA290CL";
        internal static readonly string PGM_KDA300CL = "*PGM/KDA300CL";
        internal static readonly string PGM_KDA301CL = "*PGM/KDA301CL";
        internal static readonly string PGM_KDA302CL = "*PGM/KDA302CL";
        internal static readonly string StoreTempUser = @"INSERT INTO KPAS400 
(KHPTYP, KHPIPB, KHPALX, KHPAVN, KHPSUA, KHPNUM, KHPSBR, KHPACTG, KHPACID, KHPUSR, KHPUSED, KHPCRD, KHPCRH) 
VALUES (:typeDossier, :codeDossier, :iversion, :iAvenant, 0, 0, 0, :acteGestion, 0, :user, '', :date,:time) ;";
        internal static readonly string SuppressTempUser = "DELETE FROM KPAS400 WHERE KHPTYP = :typeDossier AND KHPIPB = :codeDossier AND KHPALX = :iversion AND KHPUSR = :user ;";

        public ProgramAS400Repository(IDbConnection connection) : base(connection) { }

        public void CalculateTauxPrimes(PGMFolder folder, string branche, string sousBranche, string categorie)
        {
            Call(folder, PGM_KP081CL, new[] {
                new EacParameter("P0RET", string.Empty),
                new EacParameter("P0TYP", DbType.AnsiStringFixedLength, 1) { Value = folder.Type },
                new EacParameter("P0IPB", DbType.AnsiStringFixedLength, 9) { Value = folder.CodeOffre.ToIPB() },
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0BRA", branche),
                new EacParameter("P0SBR", sousBranche),
                new EacParameter("P0CAT", categorie)
            });
        }

        public void ValidationAvenant(PGMFolder folder) {
            Call(folder, PGM_KDA290CL, new[] {
                new EacParameter("P0IPB", DbType.AnsiStringFixedLength, 9) { Value = folder.CodeOffre.ToIPB() },
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0AVN", DbType.Int32) { Value = folder.NumeroAvenant }
            });
        }

        public void SetStop(PGMFolder folder)
        {
            Call(folder, PGM_PRA606CLST, new[] {
                new EacParameter("PARET", string.Empty),
                new EacParameter("PAEXIT", "N"),
                new EacParameter("PAIPB", folder.CodeOffre.ToIPB()),
                new EacParameter("PAALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("PAPGM", folder.ActeGestion)
            });
        }

        //public (string ret, string periodeDebut, string periodeFin, string echeance) ChangeEcheancePrincipale(PGMFolder folder, DateTime? dateEffet, DateTime? dateFinEffet, DateTime? dateAvenant,
        //    string periodicite, DateTime? echeancePrincipale)
        //{
        //    var ret = new EacParameter("P0RET", string.Empty);
        //    var dpj = new EacParameter("P0DPJ", string.Empty);
        //    var dpm = new EacParameter("P0DPM", string.Empty);
        //    var dpa = new EacParameter("P0DPA", string.Empty);
        //    var fpj = new EacParameter("P0FPJ", string.Empty);
        //    var fpm = new EacParameter("P0FPM", string.Empty);
        //    var fpa = new EacParameter("P0FPA", string.Empty);
        //    var pej = new EacParameter("P0PEJ", string.Empty);
        //    var pem = new EacParameter("P0PEM", string.Empty);
        //    var pea = new EacParameter("P0PEA", string.Empty);

        //    var parameters = new[] {
        //        ret,
        //        new EacParameter("P0IPB", DbType.AnsiStringFixedLength, 9) { Value = folder.CodeOffre.ToIPB() },
        //        new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
        //        new EacParameter("P0AVN", DbType.Int32) { Value = folder.NumeroAvenant },
        //        new EacParameter("P0HIS",  DbType.AnsiStringFixedLength){ Value = "C"},
        //        new EacParameter("P0EFJ", DbType.Int32) { Value = dateEffet != null ? dateEffet.Value.Day : 0 },
        //        new EacParameter("P0EFM", DbType.Int32) { Value = dateEffet != null ? dateEffet.Value.Month : 0 },
        //        new EacParameter("P0EFA", DbType.Int32) { Value = dateEffet != null ? dateEffet.Value.Year : 0 },
        //        new EacParameter("P0FEJ", DbType.Int32) { Value = dateFinEffet != null ? dateFinEffet.Value.Day : 0 },
        //        new EacParameter("P0FEM", DbType.Int32) { Value = dateFinEffet != null ? dateFinEffet.Value.Month : 0 },
        //        new EacParameter("P0FEA", DbType.Int32) { Value = dateFinEffet != null ? dateFinEffet.Value.Year : 0 },
        //        new EacParameter("P0AVJ", DbType.Int32) { Value = dateAvenant != null ? dateAvenant.Value.Day : 0 },
        //        new EacParameter("P0AVM", DbType.Int32) { Value = dateAvenant != null ? dateAvenant.Value.Month : 0 },
        //        new EacParameter("P0AVA", DbType.Int32) { Value = dateAvenant != null ? dateAvenant.Value.Year : 0 },
        //        new EacParameter("P0PER",  DbType.AnsiStringFixedLength) { Value = periodicite},
        //        new EacParameter("P0ECM", DbType.Int32) { Value = echeancePrincipale != null ? echeancePrincipale.Value.Month : 0 },
        //        new EacParameter("P0ECJ", DbType.Int32) { Value = echeancePrincipale != null ? echeancePrincipale.Value.Day : 0 },
        //        dpj, dpm, dpa, fpj, fpm, fpa, pej, pem, pea
        //    };

        //    Call(folder, PGM_YDP100, parameters);
        //    if (string.IsNullOrEmpty(ret.Value as string))
        //    {
        //        var periodeDebut = dpj.Value.ToString().PadLeft(2, '0') + "/" + dpm.Value.ToString().PadLeft(2, '0') + "/" + dpa.Value.ToString();
        //        var periodeFin = fpj.Value.ToString().PadLeft(2, '0') + "/" + fpm.Value.ToString().PadLeft(2, '0') + "/" + fpa.Value.ToString();
        //        var echeance = pej.Value.ToString().PadLeft(2, '0') + "/" + pem.Value.ToString().PadLeft(2, '0') + "/" + pea.Value.ToString();

        //        return ("", periodeDebut, periodeFin, echeance);
        //    }
        //    return (ret.Value as string, "", "", "");
        //}

        public CommissionCourtierDto LoadCommissions(PGMFolder folder)
        {
            var xcm = new EacParameter("P0XCM", DbType.Int32) { Value = 0 };
            var cnc = new EacParameter("P0CNC", DbType.Int32) { Value = 0 };
            var ret = new EacParameter("P0RET", " ");
            var parameters = new[] {
                new EacParameter("P0TYP", DbType.AnsiStringFixedLength, 1) { Value = folder.Type },
                new EacParameter("P0IPB", DbType.AnsiStringFixedLength, 9) { Value = folder.CodeOffre.ToIPB() },
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0MNT", DbType.Int32) { Value = 0 },
                xcm, cnc, ret
            };

            Call(folder, PGM_KA032CL, parameters);

            if (ret.Value as string != "ERREUR")
            {
                return new CommissionCourtierDto
                {
                    Erreur = string.Empty,
                    TauxStandardHCAT = (double)xcm.Value,
                    TauxStandardCAT = (double)cnc.Value,
                    TauxContratHCAT = (double)xcm.Value,
                    TauxContratCAT = (double)cnc.Value,
                    IsStandardCAT = "O",
                    IsStandardHCAT = "O"
                };
            }
            else
            {
                return new CommissionCourtierDto
                {
                    Erreur = ret.Value as string
                };
            }
        }

        public RemiseEnVigueurDto InitRemiseEnVigueur(RemiseEnVigueurParams parameters)
        {
            var names = new Dictionary<string, string> {
                { nameof(parameters.Result), parameters.GetDisplayString(p => p.Result) },
                { nameof(parameters.Type), parameters.GetDisplayString(p => p.Type) },
                { nameof(parameters.CodeContrat), parameters.GetDisplayString(p => p.CodeContrat) },
                { nameof(parameters.Version), parameters.GetDisplayString(p => p.Version) },
                { nameof(parameters.PrimeReglee), parameters.GetDisplayString(p => p.PrimeReglee) },
                { nameof(parameters.PrimeReglementDate), parameters.GetDisplayString(p => p.PrimeReglementDate) },
                { nameof(parameters.SuspDebDate), parameters.GetDisplayString(p => p.SuspDebDate) },
                { nameof(parameters.SuspDebH), parameters.GetDisplayString(p => p.SuspDebH) },
                { nameof(parameters.SuspFinDate), parameters.GetDisplayString(p => p.SuspFinDate) },
                { nameof(parameters.SuspFinH), parameters.GetDisplayString(p => p.SuspFinH) },
                { nameof(parameters.ResileDebDate), parameters.GetDisplayString(p => p.ResileDebDate) },
                { nameof(parameters.ResileDebH), parameters.GetDisplayString(p => p.ResileDebH) },
            };
            var returnParams = new Dictionary<string, EacParameter> {
                { names[nameof(parameters.Result)], new EacParameter(names[nameof(parameters.Result)], DbType.AnsiStringFixedLength, 1) { Value = string.Empty, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.Type)], new EacParameter(names[nameof(parameters.Type)], DbType.AnsiStringFixedLength, 1) { Value = parameters.Type } },
                { names[nameof(parameters.CodeContrat)], new EacParameter(names[nameof(parameters.CodeContrat)], DbType.AnsiStringFixedLength, 9) { Value = parameters.CodeContrat.ToIPB() } },
                { names[nameof(parameters.Version)], new EacParameter(names[nameof(parameters.Version)], DbType.Int16) { Value = parameters.Version } },
                { names[nameof(parameters.PrimeReglee)], new EacParameter(names[nameof(parameters.PrimeReglee)], DbType.Int16) { Value = parameters.PrimeReglee, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.PrimeReglementDate)], new EacParameter(names[nameof(parameters.PrimeReglementDate)], DbType.Int32) { Value = parameters.PrimeReglementDate, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.SuspDebDate)], new EacParameter(names[nameof(parameters.SuspDebDate)], DbType.Int32) { Value = parameters.SuspDebDate, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.SuspDebH)], new EacParameter(names[nameof(parameters.SuspDebH)], DbType.Int16) { Value = parameters.SuspDebH, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.SuspFinDate)], new EacParameter(names[nameof(parameters.SuspFinDate)], DbType.Int32) { Value = parameters.SuspFinDate, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.SuspFinH)], new EacParameter(names[nameof(parameters.SuspFinH)], DbType.Int16) { Value = parameters.SuspFinH, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.ResileDebDate)], new EacParameter(names[nameof(parameters.ResileDebDate)], DbType.Int32) { Value = parameters.ResileDebDate, Direction = ParameterDirection.InputOutput } },
                { names[nameof(parameters.ResileDebH)], new EacParameter(names[nameof(parameters.ResileDebH)], DbType.Int16) { Value = parameters.ResileDebH, Direction = ParameterDirection.InputOutput } }
            };

            Call(new PGMFolder
            {
                CodeOffre = parameters.CodeContrat.ToIPB(),
                Version = parameters.Version,
                Type = parameters.Type,
                User = parameters.User,
                ActeGestion = parameters.ActeGestion
            }, PGM_KDA196, returnParams.Values);

            parameters.Result = returnParams[names[nameof(parameters.Result)]].Value.ToString();
            parameters.PrimeReglee = Convert.ToInt16(returnParams[names[nameof(parameters.PrimeReglee)]].Value);
            parameters.PrimeReglementDate = (int)returnParams[names[nameof(parameters.PrimeReglementDate)]].Value;
            parameters.SuspDebDate = (int)returnParams[names[nameof(parameters.SuspDebDate)]].Value;
            parameters.SuspDebH = (int)returnParams[names[nameof(parameters.SuspDebH)]].Value;
            parameters.SuspFinDate = (int)returnParams[names[nameof(parameters.SuspFinDate)]].Value;
            parameters.SuspFinH = (int)returnParams[names[nameof(parameters.SuspFinH)]].Value;
            parameters.ResileDebDate = (int)returnParams[names[nameof(parameters.ResileDebDate)]].Value;
            parameters.ResileDebH = (int)returnParams[names[nameof(parameters.ResileDebH)]].Value;
            return parameters;
        }

        /// <summary>
        /// Call of KDA301CL
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="codeRisque"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeGarantie"></param>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <returns></returns>
        public string ApplyMouvementsGarantie(PGMFolder folder, int codeRisque, int codeFormule, string codeGarantie, int dateDebut, int dateFin)
        {
            var outputParam = new EacParameter("P0RET", string.Empty);
            var errorParam = new EacParameter("P0ERR", string.Empty);
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                outputParam,
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0RSQ", DbType.Int32) { Value = codeRisque },
                new EacParameter("P0FOR", DbType.Int16) { Value = (short)codeFormule },
                new EacParameter("P0GARAN", codeGarantie),
                new EacParameter("P0DEB", DbType.Int32) { Value = dateDebut },
                new EacParameter("P0FIN", DbType.Int32) { Value = dateFin },
                errorParam,
                new EacParameter("P0DEBMAXI", DbType.Int32) { Value = 0 },
                new EacParameter("P0FINMAXI", DbType.Int32) { Value = 0 }
            };
            Call(folder, PGM_KDA301CL, parameters);
            return outputParam.Value.ToString() + errorParam.Value.ToString();
        }

        /// <summary>
        /// Call of KDA302
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="codeRisque"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeGarantie"></param>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <returns></returns>
        public string ApplyMouvementsGarantieRC(PGMFolder folder, int codeRisque, int codeFormule, int dateDebut, int dateFin) {
            var outputParam = new EacParameter("P0RET", string.Empty);
            //var errorParam = new EacParameter("P0ERR", string.Empty);
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                outputParam,
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0RSQ", DbType.Int32) { Value = codeRisque },
                new EacParameter("P0FOR", DbType.Int16) { Value = (short)codeFormule },
                new EacParameter("P0DEB", DbType.Int32) { Value = dateDebut },
                new EacParameter("P0FIN", DbType.Int32) { Value = dateFin }
            };
            Call(folder, PGM_KDA302CL, parameters);
            return outputParam.Value.ToString();
        }

        public int GetNextNumber(PGMFolder folder, string key)
        {
            var outputParam = new EacParameter("P0NUU", DbType.Int32) { Value = 0 };
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0CLE", key),
                new EacParameter("P0EXT", "***"),
                new EacParameter("P0ACT", "INC"),
                outputParam,
                new EacParameter("P0RET", " ")
            };
            Call(folder, PGM_ALYDH105CL, parameters);
            return Convert.ToInt32(outputParam.Value);
        }

        public string LoadEngagement(PGMFolder folder) {
            var outputParam = new EacParameter("P0RET", " ");
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                outputParam
            };
            Call(folder, PGM_KA010CL, parameters);
            return outputParam.Value?.ToString() ?? string.Empty;
        }

        public string InitMontantRef(PGMFolder folder, string action) {
            var outputTopParam = new EacParameter("P0TOP", " ");
            var outputParam = new EacParameter("P0RET", " ");
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0ACT", action),
                outputTopParam,
                outputParam
            };
            Call(folder, PGM_KA035CL, parameters);
            if ((outputParam.Value?.ToString()).IsEmptyOrNull()) {
                return outputTopParam.Value?.ToString() ?? string.Empty;
            }
            return outputParam.Value?.ToString() ?? string.Empty;
        }

        public string LoadPreavisResiliation(PGMFolder folder, DateTime? dateEffet, DateTime? dateFinEffet, DateTime? dateAvenant, string periodicite, DateTime? echeancePrincipale, string splitCharHtml) {
            var debutJourParam = new EacParameter("P0DPJ", DbType.Int32) { Value = 0 };
            var debutMoisParam = new EacParameter("P0DPM", DbType.Int32) { Value = 0 };
            var debutAnneeParam = new EacParameter("P0DPA", DbType.Int32) { Value = 0 };
            var finJourParam = new EacParameter("P0FPJ", DbType.Int32) { Value = 0 };
            var finMoisParam = new EacParameter("P0FPM", DbType.Int32) { Value = 0 };
            var finAnneeParam = new EacParameter("P0FPA", DbType.Int32) { Value = 0 };
            var echeanceJourParam = new EacParameter("P0PEJ", DbType.Int32) { Value = 0 };
            var echeanceMoisParam = new EacParameter("P0PEM", DbType.Int32) { Value = 0 };
            var echeanceAnneeParam = new EacParameter("P0PEA", DbType.Int32) { Value = 0 };
            var outputParam = new EacParameter("P0RET", DbType.Int32) { Value = 0 };
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                outputParam,
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0AVN", DbType.Int32) { Value = folder.NumeroAvenant },
                new EacParameter("P0HIS", "C"),
                new EacParameter("P0EFJ", DbType.Int32) { Value = dateEffet.HasValue ? dateEffet.Value.Day : 0 },
                new EacParameter("P0EFM", DbType.Int32) { Value = dateEffet.HasValue ? dateEffet.Value.Month : 0 },
                new EacParameter("P0EFA", DbType.Int32) { Value = dateEffet.HasValue ? dateEffet.Value.Year : 0 },
                new EacParameter("P0FEJ", DbType.Int32) { Value = dateFinEffet.HasValue ? dateFinEffet.Value.Day : 0 },
                new EacParameter("P0FEM", DbType.Int32) { Value = dateFinEffet.HasValue ? dateFinEffet.Value.Month : 0 },
                new EacParameter("P0FEA", DbType.Int32) { Value = dateFinEffet.HasValue ? dateFinEffet.Value.Year : 0 },
                new EacParameter("P0AVJ", DbType.Int32) { Value = dateAvenant.HasValue ? dateAvenant.Value.Day : 0 },
                new EacParameter("P0AVM", DbType.Int32) { Value = dateAvenant.HasValue ? dateAvenant.Value.Month : 0 },
                new EacParameter("P0AVA", DbType.Int32) { Value = dateAvenant.HasValue ? dateAvenant.Value.Year : 0 },
                new EacParameter("P0PER", periodicite),
                new EacParameter("P0ECM", DbType.Int32) { Value = echeancePrincipale.HasValue ? echeancePrincipale.Value.Month : 0 },
                new EacParameter("P0ECJ", DbType.Int32) { Value = echeancePrincipale.HasValue ? echeancePrincipale.Value.Day : 0 },
                debutJourParam,
                debutMoisParam,
                debutAnneeParam,
                finJourParam,
                finMoisParam,
                finAnneeParam,
                echeanceJourParam,
                echeanceMoisParam,
                echeanceAnneeParam
            };
            Call(folder, PGM_YDP100, parameters);
            if (string.IsNullOrEmpty(outputParam.Value.ToString())) {
                var periodDeb = debutJourParam.Value.ToString().PadLeft(2, '0') + "/" + debutMoisParam.Value.ToString().PadLeft(2, '0') + "/" + debutAnneeParam.Value.ToString();
                var periodFin = finJourParam.Value.ToString().PadLeft(2, '0') + "/" + finMoisParam.Value.ToString().PadLeft(2, '0') + "/" + finAnneeParam.Value.ToString();
                var echeance = (echeanceJourParam.Value.ToString() != "0" && echeanceMoisParam.Value.ToString() != "0" && echeanceAnneeParam.Value.ToString() != "0") ? echeanceJourParam.Value.ToString().PadLeft(2, '0') + "/" + echeanceMoisParam.Value.ToString().PadLeft(2, '0') + "/" + echeanceAnneeParam.Value.ToString() : string.Empty;
                return periodDeb + splitCharHtml + periodFin + splitCharHtml + echeance;
            }
            return outputParam.Value.ToString();
        }

        public string ControleEcheanche(PGMFolder folder, DateTime? prochEcheance, string periodicite, DateTime? echPrincipale)
        {
            var outputParam = new EacParameter("P0RET", DbType.Int32) { Value = 0 };
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                outputParam,
                new EacParameter("P0PEJ", DbType.Int32) { Value = prochEcheance.HasValue ? prochEcheance.Value.Day : 0 },
                new EacParameter("P0PEM", DbType.Int32) { Value = prochEcheance.HasValue ? prochEcheance.Value.Month : 0 },
                new EacParameter("P0PEA", DbType.Int32) { Value = prochEcheance.HasValue ? prochEcheance.Value.Year : 0 },
                new EacParameter("P0PER", periodicite),
                new EacParameter("P0ECM", DbType.Int32) { Value = echPrincipale.HasValue ? echPrincipale.Value.Month : 0 },
                new EacParameter("P0ECJ", DbType.Int32) { Value = echPrincipale.HasValue ? echPrincipale.Value.Day : 0 }
            };
            Call(folder, PGM_YDP101, parameters);
            return outputParam.Value.ToString();
        }
        
        public string LancementCalculAffaireNouvelle(PGMFolder folder, DateEffetEtFraisData data) {
            var outputParam = new EacParameter("P0RET", string.Empty);
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0ACT", "CALCUL"),
                new EacParameter("P0EFA", DbType.Int32) { Value = data.Date != default ? data.Date.Year : 0 },
                new EacParameter("P0EFM", DbType.Int32) { Value = data.Date != default ? data.Date.Month : 0 },
                new EacParameter("P0EFJ", DbType.Int32) { Value = data.Date != default ? data.Date.Day : 0 },
                new EacParameter("P0AFV", DbType.Decimal) { Value = data.Frais },
                new EacParameter("P0ATT", data.Att),
                outputParam,
                new EacParameter("P0AFC", data.Afc),
                new EacParameter("P0AFR", DbType.Decimal) { Value = data.Frais }
            };
            Call(folder, PGM_KDP025CL, parameters);
            return outputParam.Value?.ToString() ?? string.Empty;
        }

        public KDA300Result CheckAndGetPeriodesAttestation(PGMFolder folder, int? exercice, (DateTime? debut, DateTime? fin)? periode) {
            var parameters = BuildKDA300Parameters(folder, exercice, periode, true);
            Call(folder, PGM_KDA300CL, parameters.Values.ToArray());
            return BuildKDA300Result(parameters);
        }

        public KDA300Result CheckAndGetPeriodesRegularisation(PGMFolder folder, int? exercice, (DateTime? debut, DateTime? fin)? periode) {
            var parameters = BuildKDA300Parameters(folder, exercice, periode, false);
            Call(folder, PGM_KDA300CL, parameters.Values.ToArray());
            return BuildKDA300Result(parameters);
        }

        internal void Call(PGMFolder folder, string prgname, IEnumerable<DbParameter> parameters) {
            ExecuteWithUser(folder, () => {
                using (var options = new DbSPOptions(this.connection == null) {
                    DbConnection = this.connection,
                    Parameters = parameters,
                    SqlText = prgname
                })
                {
                    options.ExecStoredProcedure();
                }
            });
        }

        private IDictionary<string, EacParameter> BuildKDA300Parameters(PGMFolder folder, int? exercice, (DateTime? debut, DateTime? fin)? periode, bool isAttestation) {
            return new[] {
                new EacParameter("P0RET", string.Empty),
                new EacParameter("P0IPB", folder.CodeOffre.PadLeft(9, ' ')),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0EXE", DbType.Int32) { Value = exercice.GetValueOrDefault() },
                new EacParameter("P0DPA", DbType.Int32) { Value = periode?.debut?.Year ?? 0 },
                new EacParameter("P0DPM", DbType.Int32) { Value = periode?.debut?.Month ?? 0 },
                new EacParameter("P0DPJ", DbType.Int32) { Value = periode?.debut?.Day ?? 0 },
                new EacParameter("P0FPA", DbType.Int32) { Value = periode?.fin?.Year ?? 0 },
                new EacParameter("P0FPM", DbType.Int32) { Value = periode?.fin?.Month ?? 0 },
                new EacParameter("P0FPJ", DbType.Int32) { Value = periode?.fin?.Day ?? 0 },
                new EacParameter("P0ERR", string.Empty),
                new EacParameter("P0DAA", DbType.Int32) { Value = 0 },
                new EacParameter("P0DAM", DbType.Int32) { Value = 0 },
                new EacParameter("P0DAJ", DbType.Int32) { Value = 0 },
                new EacParameter("P0DAV", DbType.Int32) { Value = 0 },
                new EacParameter("P0ENC", string.Empty),
                new EacParameter("P0ICT", DbType.Int32) { Value = 0 },
                new EacParameter("P0DFI", string.Empty),
                new EacParameter("P0AVK", DbType.Int32) { Value = 0 },
                new EacParameter("P0ICC", DbType.Int32) { Value = 0 },
                new EacParameter("P0ORI", isAttestation ? "ATTEST" : string.Empty)
            }.ToDictionary(x => x.ParameterName);
        }

        private KDA300Result BuildKDA300Result(IDictionary<string, EacParameter> parameters) {
            return new KDA300Result {
                AnneeDebut = Convert.ToInt32(parameters["P0DPA"].Value),
                MoisDebut = Convert.ToInt32(parameters["P0DPM"].Value),
                JourDebut = Convert.ToInt32(parameters["P0DPJ"].Value),
                AnneeFin = Convert.ToInt32(parameters["P0FPA"].Value),
                MoisFin = Convert.ToInt32(parameters["P0FPM"].Value),
                JourFin = Convert.ToInt32(parameters["P0FPJ"].Value),
                CodeErreur = parameters["P0ERR"].Value as string,
                CodeCourtierPayeur = Convert.ToInt64(parameters["P0ICT"].Value),
                CodeCourtierCommission = Convert.ToInt64(parameters["P0ICC"].Value),
                CodeEncaissement = parameters["P0ENC"].Value as string,
                DernierAvn = Convert.ToInt32(parameters["P0DAV"].Value),
                MultiCourtier = Convert.ToInt64(parameters["P0ICT"].Value) != Convert.ToInt64(parameters["P0ICC"].Value) || Equals(parameters["P0DFI"].Value, "O")
            };
        }

        private void SupplyUser(PGMFolder folder)
        {
            using (var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = StoreTempUser,
                Parameters = MakeParams(
                    StoreTempUser,
                    folder.Type, folder.CodeOffre.ToIPB(), folder.Version, folder.NumeroAvenant,
                    folder.ActeGestion, folder.User, folder.Now.ToString("yyyyMMdd"), folder.Now.ToString("HHmmss"))
            }) { options.Exec(); }
        }

        private void RemoveSuppliedUser(PGMFolder folder)
        {
            using (var options = new DbExecOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SuppressTempUser,
                Parameters = MakeParams(SuppressTempUser, folder.Type, folder.CodeOffre.ToIPB(), folder.Version, folder.User)
            }) { options.Exec(); }
        }

        private T ExecuteWithUser<T>(PGMFolder folder, Func<T> functionAS400)
        {
            bool success = false;
            T result = default(T);
            try
            {
                SupplyUser(folder);
                result = functionAS400();
                success = true;
                return result;
            }
            finally
            {
                try
                {
                    RemoveSuppliedUser(folder);
                }
                catch
                {
                    if (success)
                    {
                        throw;
                    }
                }
            }
        }

        private void ExecuteWithUser(PGMFolder folder, Action actionAS400)
        {
            ExecuteWithUser(folder, () =>
            {
                actionAS400();
                return 0;
            });
        }

        public string UpdateCotisationsOffre(PGMFolder folder, string field, decimal? oldValue, decimal? value) {
            var outputParam = new EacParameter("P0RET", " ");
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0TYP", AlbConstantesMetiers.TYPE_OFFRE),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0ACT", field),
                new EacParameter("P0MNTC", DbType.Decimal) { Value = oldValue.GetValueOrDefault() },
                new EacParameter("P0MNTF", DbType.Decimal) { Value = value.GetValueOrDefault() },
                outputParam,
                new EacParameter("P0OBS", string.Empty)
            };
            Call(folder, PGM_KA030CL, parameters);
            return outputParam.Value.ToString();
        }

        #region Methods Contracts Kheops

        public string NewIPBContract(PGMFolder folder, string branche, string cible, string anneeEffet)
        {
            var outputParam = new EacParameter("P0IPB", " ");
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0TYP", "P"),
                new EacParameter("P0BRA", branche),
                new EacParameter("P0CIB", cible),
                new EacParameter("P0YEAR", anneeEffet),
                outputParam,
                new EacParameter("P0RET", " ")
            };
            Call(folder, PGM_KA000CL, parameters);
            return outputParam.Value.ToString();
        }

        public void PrepareCotisation(PGMFolder folder)
        {
            var parameters = new[]
            {
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0ACT", "AFFNV")
            };
            Call(folder, PGM_KDP025CL1, parameters);
        }

        public void CalculCotisation(PGMFolder folder, DateEffetEtFraisData fraisAcc)
        {
            string typeFrais  = string.IsNullOrEmpty(fraisAcc.HasPripes) ? fraisAcc.Afc : fraisAcc.Frais != 0 ? "O" : "N";
            string appliqueAtt = string.IsNullOrEmpty(fraisAcc.HasPripes) ? "N" : fraisAcc.Atm != 0 ? "O" : "N";

            var ret = new EacParameter("P0RET", " ");
            var parameters = new[]
            {
                ret,
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0AFC", "O"),
                new EacParameter("P0AFR", DbType.Int32) { Value = fraisAcc.Frais },
                new EacParameter("P0ATT", appliqueAtt),
                new EacParameter("P0AFV", DbType.Int32) { Value = 0 }
            };
            Call(folder, PGM_KDP192CL, parameters);
        }

        public void AlimStatContract(PGMFolder folder, DateEffetEtFraisData data)
        {
            var outputParam = new EacParameter("P0RET", " ");
            IEnumerable<DbParameter> parameters = new List<EacParameter> {
                new EacParameter("P0SEL", "N"),
                outputParam,
                new EacParameter("P0IPB", folder.CodeOffre),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0EFA", DbType.Int32) { Value = data.Date != default ? data.Date.Year : 0 },
                new EacParameter("P0EFM", DbType.Int32) { Value = data.Date != default ? data.Date.Month : 0 },
                new EacParameter("P0EFJ", DbType.Int32) { Value = data.Date != default ? data.Date.Day : 0 },
            };
            Call(folder, PGM_KDP021CL, parameters);
        }

        public void Validation400Contract(PGMFolder folder)
        {
            var outputParam = new EacParameter("P0RET", " ");
            var parameters = new[]
            {
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                outputParam
            };
            Call(folder, PGM_KA050CL, parameters);
        }

        public int GeneratePrimeContract(PGMFolder folder)
        {
            var outputParam = new EacParameter("P0RET", " ");
            var numPrime = new EacParameter("P0IPK", DbType.Int32) { Value = 0 };
            var parameters = new[]
            {
                outputParam,
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0AVN", DbType.Int32) { Value = 0 },
                new EacParameter("P0ACTG", folder.ActeGestion),
                numPrime
            };
            Call(folder, PGM_KPR290CL, parameters);
            return int.Parse(numPrime.Value.ToString());
        }

        public void SetMatriceContract(PGMFolder folder)
        {
            var outputParam = new EacParameter("P0RET", " ");
            var parameters = new[]
            {
                new EacParameter("P0TYP", folder.Type),
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                outputParam
            };
            Call(folder, PGM_KA040CL, parameters);
        }

        public void ForceMontant(PGMFolder folder, decimal mntCalc, decimal mntForce, decimal coef)
        {
            var outputParam = new EacParameter("P0RET", " ");
            var parameters = new[]
            {
                outputParam,
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0AVN", DbType.Int32) { Value = 0 },
                new EacParameter("P0ACO", "O"),
                new EacParameter("P0TYC", "P"),
                new EacParameter("P0RSQ", DbType.Int32) { Value = 0 },
                new EacParameter("P0FOR", DbType.Int32) { Value = 0 },
                new EacParameter("P0MNTC", DbType.Decimal) { Value = mntCalc },
                new EacParameter("P0MNTF", DbType.Decimal) { Value = mntForce },
                new EacParameter("P0HTTC", "H"),
                new EacParameter("P0COEF", DbType.Decimal) { Value = coef },
                new EacParameter("P0CAS", "C"),
                new EacParameter("P0MAJ", "O"),
            };
            Call(folder, PGM_KDP196, parameters);
        }

        public void CalculMontantForce(PGMFolder folder, bool attentat)
        {
            var outputParam = new EacParameter("P0RET", " ");
            var parameters = new[]
            {
                outputParam,
                new EacParameter("P0IPB", folder.CodeOffre.ToIPB()),
                new EacParameter("P0ALX", DbType.Int32) { Value = folder.Version },
                new EacParameter("P0ATT", attentat ? "O" : "N")
            };
            Call(folder, PGM_KDP196CL, parameters);
        }

        #endregion
    }
}
