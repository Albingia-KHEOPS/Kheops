using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Albingia.Hexavia.Core.Helper;
using Albingia.Hexavia.CoreDomain;
using System.Runtime.Caching;
using Albingia.Hexavia.DataAccess.Helper;

namespace Albingia.Hexavia.DataAccess
{
    public class ReferenceRepository : BaseRepository
    {
        public MemoryCache MemoryCache { get; set; }
        public ReferenceRepository(DataAccessManager dataAccessManager, MemoryCache memoryCache)
            : base(dataAccessManager)
        {
            MemoryCache = memoryCache;
        }

        public List<Parametres> Parametres(string TCON, string TFAM, string TPCA1 = null, List<String> TCOD = null)
        {
            List<Parametres> result = null;
            CmdWrapper cmd = dataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT TCOD, TPLIB FROM YYYYPAR";
            cmd.Where("TCON", TCON, DbType.AnsiStringFixedLength);
            cmd.Where("TFAM", TFAM, DbType.AnsiStringFixedLength);
            cmd.Where("TPCA1", TPCA1, DbType.AnsiStringFixedLength);
            cmd.WhereIn("TCOD", TCOD, false);
            cmd.CommandText += " ORDER BY TPLIB";

            DataTable dataTable = dataAccessManager.ExecuteDataTable(cmd);
            if (dataTable.Rows.Count > 0)
            {
                result = new List<Parametres>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Parametres parametre = new Parametres
                    {
                        Code = row["TCOD"].ToString(),
                        Libelle = row["TPLIB"].ToString()
                    };
                    result.Add(parametre);
                }
            }
            return result;
        }
    }
}