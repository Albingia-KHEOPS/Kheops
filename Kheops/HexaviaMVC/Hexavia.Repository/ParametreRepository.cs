using Hexavia.Repository.Interfaces;
using Hexavia.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hexavia.Repository
{
    public class ParametreRepository : BaseRepository, IParametreRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParametreRepository));

        public ParametreRepository(DataAccessManager dataAccessManager)
            : base(dataAccessManager)
        {
        }

        public List<Parametre> Load(string TCON, string TFAM, string TPCA1 = null, List<String> TCOD = null)
        {
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT TCOD, TPLIB FROM YYYYPAR";
            cmd.Where("TCON", TCON, DbType.AnsiStringFixedLength);
            cmd.Where("TFAM", TFAM, DbType.AnsiStringFixedLength);
            cmd.Where("TPCA1", TPCA1, DbType.AnsiStringFixedLength);
            cmd.WhereIn("TCOD", TCOD, false);
            cmd.CommandText += " ORDER BY TPLIB";

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);
            if (dataTable.Rows.Count <= 0)
            {
                Logger.Warn("No country found.");
                return null;
            }

            var result = new List<Parametre>();
            foreach (DataRow row in dataTable.Rows)
            {
                var parametre = new Parametre
                {
                    Code = row["TCOD"].ToString(),
                    Libelle = row["TPLIB"].ToString()
                };
                result.Add(parametre);
            }

            return result;
        }
    }
}
