using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.PGM;
using System;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess {
    public class RemiseEnVigueurRepository : RepositoryBase {
        private readonly ProgramAS400Repository as400Repository;

        internal static readonly string SelectTypeGestion = $@"SELECT KAAARTYG 
FROM KPENT 
INNER JOIN YPOBASE ON  ( KAAIPB , KAAALX , KAATYP ) = ( PBIPB , PBALX , PBTYP ) AND PBTTR = '{AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR}' 
WHERE ( KAAIPB , KAAALX , KAATYP ) = ( :IPB , :ALX , :TYP ) ";
        internal static readonly string UpdateRemiseEnVigueur = @"
UPDATE KPENT
SET KAAARIPK = :NUMPRIME, KAAARTYG = :TYPEGESTION, KAAPKRD = :DATEREGLEMENT,
    KAASUDD = :DATEDEBSUSP, KAASUDH = :HEUREDEBSUSP, KAASUFD = :DATEFINSUSP, KAASUFH = :HEUREFINSUSP,
    KAARSDD = :DATEDEBRESIL, KAARSDH = :HEUREDEBRESIL
WHERE KAAIPB = :CODEAFFAIRE AND KAAALX = :VERSION AND KAATYP = :TYPE";
        internal static readonly string UpdateRemiseEnVigueurSansSuspension = @"
UPDATE KPENT 
SET KAAARTYG = :TYPEGESTION, KAARSDD = :DATEDEBRESIL, KAARSDH = :HEUREDEBRESIL 
WHERE KAAIPB = :CODEAFFAIRE AND KAAALX = :VERSION AND KAATYP = :TYPE ";

        internal static readonly string UpdateModifRemiseEnVigueur = @"
UPDATE KPENT
SET KAAARTYG = :TYPEGESTION
WHERE KAAIPB = :CODEAFFAIRE AND KAAALX = :VERSION AND KAATYP = :TYPE";

        public RemiseEnVigueurRepository(IDbConnection connection, ProgramAS400Repository as400Repository) : base(connection) {
            this.as400Repository = as400Repository;
        }

        /// <summary>
        /// appelle le PGM AS400 qui retourne les differentes valeurs servant à initialiser l'avenant de Remise en Vigueur
        /// </summary>
        /// <param name="parameters">pre initialisation des parametres de retour</param>
        public static RemiseEnVigueurDto CallKDA196(RemiseEnVigueurParams parameters)
        {
            const string programName = "*PGM/KDA196";
            EacParameter[] returnParams = new EacParameter[]
                {
                    new EacParameter(parameters.GetDisplayString(p => p.Result), DbType.AnsiStringFixedLength, 1) { Value = parameters.Result, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.Type), DbType.AnsiStringFixedLength, 1) { Value = parameters.Type },
                    new EacParameter(parameters.GetDisplayString(p => p.CodeContrat), DbType.AnsiStringFixedLength, 9) { Value = parameters.CodeContrat },
                    new EacParameter(parameters.GetDisplayString(p => p.Version), DbType.Int16) { Value = parameters.Version },
                    new EacParameter(parameters.GetDisplayString(p => p.PrimeReglee), DbType.Int16) { Value = parameters.PrimeReglee, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.PrimeReglementDate), DbType.Int32) { Value = parameters.PrimeReglementDate, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.SuspDebDate), DbType.Int32) { Value = parameters.SuspDebDate, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.SuspDebH), DbType.Int16) { Value = parameters.SuspDebH, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.SuspFinDate), DbType.Int32) { Value = parameters.SuspFinDate, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.SuspFinH), DbType.Int16) { Value = parameters.SuspFinH, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.ResileDebDate), DbType.Int32) { Value = parameters.ResileDebDate, Direction = ParameterDirection.InputOutput },
                    new EacParameter(parameters.GetDisplayString(p => p.ResileDebH), DbType.Int16) { Value = parameters.ResileDebH, Direction = ParameterDirection.InputOutput }
                };

            DbBase.Settings.ExecuteNonQuery(
                CommandType.StoredProcedure,
                programName,
                returnParams
            );

            parameters.Result = returnParams[0].Value.ToString();
            parameters.PrimeReglee = Convert.ToInt16(returnParams[4].Value);
            parameters.PrimeReglementDate = (int)returnParams[5].Value;
            parameters.SuspDebDate = (int)returnParams[6].Value;
            parameters.SuspDebH = (int)returnParams[7].Value;
            parameters.SuspFinDate = (int)returnParams[8].Value;
            parameters.SuspFinH = (int)returnParams[9].Value;
            parameters.ResileDebDate = (int)returnParams[10].Value;
            parameters.ResileDebH = (int)returnParams[11].Value;

            return parameters;
        }

        public RemiseEnVigueurDto InitRemiseEnVigueur(RemiseEnVigueurParams parameters) {
            return this.as400Repository.InitRemiseEnVigueur(parameters);
        }

        public void CreateRemiveEnVigeur(Folder folder, RemiseEnVigueurDto rmv, string typeGestion) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateRemiseEnVigueur
            }) {
                options.BuildParameters(
                    rmv.PrimeReglee,
                    typeGestion,
                    rmv.PrimeReglementDate,
                    rmv.SuspDebDate,
                    rmv.SuspDebH,
                    rmv.SuspFinDate,
                    rmv.SuspFinH,
                    rmv.ResileDebDate,
                    rmv.ResileDebH,
                    folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                options.Exec();
            }
        }

        public void CreateRemiveEnVigeurSansSsp(Folder folder, RemiseEnVigueurDto rmv, string typeGestion) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateRemiseEnVigueurSansSuspension
            }) {
                options.BuildParameters(typeGestion, rmv.ResileDebDate, rmv.ResileDebH, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                options.Exec();
            }
        }

        public void ModifyRemiseEnVigeur(Folder folder, string typeGestion) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = UpdateModifRemiseEnVigueur
            }) {
                options.BuildParameters(typeGestion, folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                options.Exec();
            }
        }

        public string GetTypeGestion(Folder folder) {
            using (var options = new DbSelectStringsOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = SelectTypeGestion
            }) {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type);
                options.PerformSelect();
                return options.StringList.FirstOrDefault() ?? string.Empty;
            }
        }
    }
}
