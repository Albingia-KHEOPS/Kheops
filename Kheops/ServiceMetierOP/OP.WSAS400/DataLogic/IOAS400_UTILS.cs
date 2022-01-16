using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OP.CoreDomain;
using System.Data.EasycomClient;
using System.Data.Common;
using System.Data;

namespace OP.WSAS400.DataLogic
{
    public partial class IOAS400
    {
        /// <summary>
        /// Motifses the refus get.
        /// </summary>
        /// <returns></returns>
        public List<Parametre> MotifsRefusGet() {
            List<Parametre> toReturn = new List<Parametre>();
            Parametre toAdd;
            EacCommand oCmd = new EacCommand();
            DbDataReader oDr;

            //"PRODU", "PBSTF"

            string SQLQ = string.Empty;
            SQLQ += "SELECT TCOD, TPLIB FROM YYYYPAR";
            SQLQ += " WHERE TCON = 'PRODU' AND TFAM='PBSTF'";
            SQLQ += " ORDER BY TPLIB;";
            
            oCmd.CommandType = System.Data.CommandType.Text;
            oCmd.Connection = oConn;
            oCmd.CommandText = SQLQ;

            oDr = oCmd.ExecuteReader();

            while (oDr.Read()) {
                toAdd = new Parametre()
                {
                    Code = oDr.GetString(0),
                    Libelle = oDr.GetString(1)
                };
                toReturn.Add(toAdd);
            }
            if (toReturn.Count == 0) throw new DataException("La liste des motifs de refus est vide");
            
            return toReturn;
        }
    }
}