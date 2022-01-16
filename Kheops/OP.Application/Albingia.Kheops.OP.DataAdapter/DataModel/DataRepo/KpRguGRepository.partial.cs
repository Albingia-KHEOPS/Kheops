using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KpRguGRepository {
        const string try_insert_MouvementsRC = "SP_ENSURELISTMOUVTGARRC";
        const string insert_select_Mouvements = "SP_GETLISTMOUVTGAR";

        public void EnsureInsertMouvementsRC(string codeOffre, int version, int numRisque, int numFormule, long idRegul, long idLot) {
            using (var command = this.connection.CreateCommand()) {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = try_insert_MouvementsRC;
                new Dictionary<string, object> {
                    ["P_CODEOFFRE"] = codeOffre,
                    ["P_VERSION"] = version,
                    ["P_TYPE"] = AffaireType.Contrat.AsCode(),
                    ["P_CODERSQ"] = numRisque,
                    ["P_CODEFOR"] = numFormule,
                    ["P_IDGAR"] = 0,
                    ["P_IDREGUL"] = idRegul,
                    ["P_IDLOT"] = idLot
                }.Select(x => { return MakeParam(x.Key, x.Value, command); })
                .ToList()
                .ForEach(x => command.Parameters.Add(x));
                
                command.ExecuteNonQuery();
            }
        }

        public void EnsureInsertMouvements(string codeOffre, int version, int numRisque, int numFormule, long idGarantie, long idRegul) {
            using (var command = this.connection.CreateCommand()) {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = insert_select_Mouvements;
                new Dictionary<string, object> {
                    ["P_CODEOFFRE"] = codeOffre,
                    ["P_VERSION"] = version,
                    ["P_TYPE"] = AffaireType.Contrat.AsCode(),
                    ["P_CODERSQ"] = numRisque,
                    ["P_CODEFOR"] = numFormule,
                    ["P_IDGAR"] = idGarantie,
                    ["P_IDREGUL"] = idRegul,
                    ["P_ISREADONLY"] = 0
                }.Select(x => { return MakeParam(x.Key, x.Value, command); })
                .ToList()
                .ForEach(x => command.Parameters.Add(x));

                command.ExecuteNonQuery();
            }
        }

        private static IDbDataParameter MakeParam(string name, object value, IDbCommand command, ParameterDirection dir = ParameterDirection.Input) {
            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            p.Direction = dir;
            return p;
        }
    }
}
