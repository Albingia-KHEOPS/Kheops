using Hexavia.Models;
using Hexavia.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexavia.Repository
{
    public class ReferentielRepository : BaseRepository, IReferentielRepository
    {
        public ReferentielRepository(DataAccessManager dataAccessManager)
           : base(dataAccessManager)
        {
        }

        public List<CodeLibelle> GetAllBranches()
        {
            var result = new List<CodeLibelle>();
            var cmd = DataAccessManager.CmdWrapper;

            cmd.CommandText = string.Format(@"SELECT * FROM YYYYPAR");
            cmd.Where(@"TCON = 'GENER' AND TFAM = 'BRCHE' AND TCOD NOT IN ('PP', 'ZZ')");
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var branche = new CodeLibelle
                {
                    Code = row["TCOD"].ToString(),
                    Libelle = row["TPLIB"].ToString()
                };
                result.Add(branche);
            }
            return result;
        }

        public List<CodeLibelle> GetSituationByType(string type)
        {
            var result = new List<CodeLibelle>();
            var cmd = DataAccessManager.CmdWrapper;

            cmd.CommandText = string.Format(@"SELECT * FROM YYYYPAR");
            if (type == "O" || type == "P")
            {
                cmd.Where(@"TCON = 'PRODU' AND TFAM = 'PBSIT'");
            }
            else
            {
                cmd.Where(@"TCON = 'SINIS' AND TFAM = 'SISIT'");
            }

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var situation = new CodeLibelle
                {
                    Code = row["TCOD"].ToString(),
                    Libelle = row["TPLIB"].ToString()
                };
                result.Add(situation);
            }
            return result;
        }

        public List<CodeLibelle> GetEtatByType(string type)
        {
            var result = new List<CodeLibelle>();
            var cmd = DataAccessManager.CmdWrapper;

            cmd.CommandText = string.Format(@"SELECT * FROM YYYYPAR");
            if (type == "O" || type == "P")
            {
                cmd.Where(@"TCON = 'PRODU' AND TFAM = 'PBETA'");
            }
            else
            {
                cmd.Where(@"TCON = 'SINIS' AND TFAM = 'SIETA'");
            }

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var etat = new CodeLibelle
                {
                    Code = row["TCOD"].ToString(),
                    Libelle = row["TPLIB"].ToString()
                };
                result.Add(etat);
            }
            return result;
        }

        public List<CodeLibelle> GetEvenement()
        {
            var result = new List<CodeLibelle>();
            var cmd = DataAccessManager.CmdWrapper;

            cmd.CommandText = string.Format(@"SELECT * FROM YYYYPAR");
            cmd.Where(@"TCON = 'SINIS' AND TFAM = 'EVN'");

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                var evenement = new CodeLibelle
                {
                    Code = row["TCOD"].ToString(),
                    Libelle = row["TPLIB"].ToString()
                };
                result.Add(evenement);
            }
            return result.OrderBy(x => x.Libelle).ToList();
        }
    }
}
