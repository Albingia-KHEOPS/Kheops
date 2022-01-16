using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{

    public partial class KpOptDRepository
    {
        public void UpdateDependentData(Formule formule, int optionNumber, string user)
        {
            const string procedure = "SP_SVOPTD";
            var option = formule.Options.Find(x => x.OptionNumber == optionNumber);
            if (option != null) {

                using (var command = connection.CreateCommand()) {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    new Dictionary<string, object> {

                        ["P_CODEOFFRE"] = formule.AffaireId.CodeAffaire,
                        ["P_VERSION"] = formule.AffaireId.NumeroAliment,
                        ["P_TYPE"] = formule.AffaireId.TypeAffaire.AsString(),
                        ["P_CODEAVENANT"] = formule.AffaireId.NumeroAvenant ?? 0,
                        ["P_CODEFORMULE"] = formule.FormuleNumber,
                        ["P_CODEOPTION"] = option.OptionNumber,
                        ["P_LIBELLE"] = formule.Description,
                        ["P_DATEAVT"] = option.DateAvenant,
                        ["P_USER"] = user,
                        ["P_DATENOW"] = DateTime.Now.Date.AsDateNumber(),
                        ["P_HEURENOW"] = DateTime.Now.Date.AsTimeNumber(),

                    }.Select(x => { return MakeParam(x.Key, x.Value, command); })
                    .ToList()
                    .ForEach(x => command.Parameters.Add(x));
                    var outParam = MakeParam("P_ERROR", "", command, ParameterDirection.Output);
                    command.Parameters.Add(outParam);
                    command.ExecuteNonQuery();
                    if (outParam.Value is String err && !String.IsNullOrWhiteSpace(err) && err.Trim() != "##;ERRORMSG;##") {
                        var errs = err.Split(';');
                        var codes = errs.Skip(2).Take(errs.Count() - 3).Select(x => x.Split('_').JoinString(" : ")).JoinString("\n");
                        throw new Exception($"Erreurs: \n {codes}");
                    }

                }
            }

        }
        private static IDbDataParameter MakeParam(string name, object value, IDbCommand command, ParameterDirection dir = ParameterDirection.Input)
        {
            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            p.Direction = dir;
            return p;
        }
    }
}

