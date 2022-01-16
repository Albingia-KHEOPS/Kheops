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
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class KpOptRepository
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
                        ["P_TYPE"] = formule.AffaireId.TypeAffaire.AsCode(),
                        ["P_CODEAVENANT"] = formule.AffaireId.NumeroAvenant ?? 0,
                        ["P_CODEFORMULE"] = formule.FormuleNumber,
                        ["P_CODEOPTION"] = option.OptionNumber,
                        ["P_LIBELLE"] = formule.Description,
                        ["P_DATEAVT"] = option.DateAvenant.AsDateNumber(),
                        ["P_USER"] = user,
                        ["P_DATENOW"] = DateTime.Now.Date.AsDateNumber(),
                        ["P_HEURENOW"] = DateTime.Now.Date.AsTimeNumber(),

                    }.Select(x => { return MakeParam(x.Key, x.Value, command); })
                    .ToList()
                    .ForEach(x => command.Parameters.Add(x));
                    var outParam = MakeParam("P_ERROR", "", command, ParameterDirection.Output, 5000);
                    command.Parameters.Add(outParam);
                    command.ExecuteNonQuery();
                    if (outParam.Value is String err && !String.IsNullOrWhiteSpace(err) && err.Trim() != "##;ERRORMSG;##") {
                        var errs = err.Split(';');
                        var codes = errs.Skip(2).Take(errs.Count() - 3).Select(x => x.Split('_').JoinString(" : ")).JoinString("\n");
                        throw new Exception($"Erreurs: \n {codes}");
                    }
                }

                var parameters = new Dictionary<string, object> {
                    ["type"] = formule.AffaireId.TypeAffaire.AsCode(),
                    ["ipb"] = formule.AffaireId.CodeAffaire.PadLeft(9, ' '),
                    ["alx"] = formule.AffaireId.NumeroAliment,
                    ["numformule"] = formule.FormuleNumber,
                    ["numoption"] = option.OptionNumber
                };

                using (var command = connection.CreateCommand()) {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"DELETE FROM KPCTRLE
WHERE KEVTYP = :type AND KEVIPB = :ipb AND KEVALX = :alx AND KEVETAPE = 'OPT'
AND KEVFOR = :numformule AND KEVOPT = :numoption AND KEVPERI = 'OPT'";
                    parameters
                        .Select(x => { return MakeParam(x.Key, x.Value, command); })
                        .ToList()
                        .ForEach(x => command.Parameters.Add(x));

                    command.ExecuteNonQuery();
                }

                DateTime dateNow = DateTime.Now;
                parameters = new Dictionary<string, object> {
                    ["P_TYPE"] = formule.AffaireId.TypeAffaire.AsCode(),
                    ["P_CODEOFFRE"] = formule.AffaireId.CodeAffaire.PadLeft(9, ' '),
                    ["P_VERSION"] = formule.AffaireId.NumeroAliment,
                    ["P_ETAPE"] = "OPT",
                    ["P_NUMETAPE"] = 50,
                    ["P_ORDRE"] = 1,
                    ["P_PERIMETRE"] = "OPT",
                    ["P_CODERSQ"] = option.Applications.First().NumRisque,
                    ["P_CODEOBJ"] = 0,
                    ["P_CODEINVEN"] = 0,
                    ["P_CODEFOR"] = formule.FormuleNumber,
                    ["P_CODEOPT"] = option.OptionNumber,
                    ["P_NIVCLSST"] = string.Empty,
                    ["P_USER"] = user,
                    ["P_DATESYSTEME"] = dateNow.AsDateNumber(),
                    ["P_HEURESYSTEME"] = dateNow.AsTimeNumber(),
                    ["P_PASSTAG"] = "O",
                    ["P_PASSTAGCLAUSE"] = "N"
                };

                using (var command = connection.CreateCommand()) {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_ARBRESV";
                    parameters
                        .Select(x => { return MakeParam(x.Key, x.Value, command); })
                        .ToList()
                        .ForEach(x => command.Parameters.Add(x));
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        private static IDbDataParameter MakeParam(string name, object value, IDbCommand command, ParameterDirection dir = ParameterDirection.Input, int? length= null)
        {
            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            p.Direction = dir;
            if (value?.GetType() == typeof(String)) {
                p.DbType = DbType.AnsiStringFixedLength;
            }
            if (length.HasValue) {
                p.Size = length.Value;
                if (p is EacParameter eac) {
                    eac.SetSize(length.Value);
                }
                
            }
            return p;
        }
    }
}
