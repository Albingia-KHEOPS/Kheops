using Dapper;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class YpltGarRepository
    {
        internal List<GarantieTypePlatDto> RechercherGarantieType(string codeModele)
        {
            string sql = string.Format(@"SELECT YPLTGAR.*, GADES, Y1.TPLIB C2CARLIB, Y2.TPLIB C2NATLIB
                        FROM YPLTGAR
                        LEFT JOIN KGARAN ON GAGAR = C2GAR
                        INNER JOIN YYYYPAR Y1 ON Y1.TCON = 'PRODU' AND Y1.TFAM = 'CBCAR' AND Y1.TCOD = C2CAR
                        INNER JOIN YYYYPAR Y2 ON Y2.TCON = 'PRODU' AND Y2.TFAM = 'CBNAT' AND Y2.TCOD = C2NAT
                        WHERE UPPER(C2MGA) = '{0}' ", codeModele.ToUpper().Replace("'", "''"));
            var c = this.connection.EnsureOpened();
            return c.Query<GarantieTypePlatDto>(sql).ToList();
        }

        internal List<GarantieTypePlatDto> GetGarantieTypeAll()
        {
            string sql = string.Format(@"SELECT YPLTGAR.*, GADES, Y1.TPLIB C2CARLIB, Y2.TPLIB C2NATLIB
                        FROM YPLTGAR
                        INNER JOIN KGARAN ON GAGAR = C2GAR
                        INNER JOIN YYYYPAR Y1 ON Y1.TCON = 'PRODU' AND Y1.TFAM = 'CBCAR' AND Y1.TCOD = C2CAR
                        INNER JOIN YYYYPAR Y2 ON Y2.TCON = 'PRODU' AND Y2.TFAM = 'CBNAT' AND Y2.TCOD = C2NAT
                        INNER JOIN YPLMGA ON D1MGA = C2MGA
                        LEFT JOIN KCATMODELE ON KARMODELE = C2MGA
                        LEFT JOIN KPOPTD ON KARID = KDCKARID
                        WHERE KDCKARID IS NULL");
            var c = this.connection.EnsureOpened();
            return c.Query<GarantieTypePlatDto>(sql).ToList();
        }

        internal List<GarantieTypePlatDto> GetInfo(long seq)
        {
            string sql = string.Format(@"SELECT YR.*, GADES,
                C4TYP, C4BAS, C4VAL, C4UNT , C4MAJ, C4OBL, C4ALA
                FROM YPLTGAR YR
                INNER JOIN KGARAN ON GAGAR = C2GAR
                INNER JOIN YPLTGAL ON C2SEQ = C4SEQ 
                WHERE C2SEQ = {0}", seq);
            var c = this.connection.EnsureOpened();
            return c.Query<GarantieTypePlatDto>(sql).ToList();
        }

        internal List<GarantieTypePlatDto> GetInfoLien(long seq)
        {
            string sql = string.Format(@"SELECT YR.*, GADES, YA.*, YR1.C2GAR GARLIEENOM, YR1.C2MGA GARLIEEMODELE, YR1.C2NIV GARLIEENIV
                FROM YPLTGAR YR
                INNER JOIN KGARAN ON GAGAR = YR.C2GAR
                LEFT JOIN YPLTGAA YA ON (YR.C2SEQ = C5SEM AND C5TYP IN ('A', 'I')) OR C5SEQ = YR.C2SEQ
                LEFT JOIN YPLTGAR YR1 ON (CASE WHEN C5SEQ = {0} THEN C5SEM ELSE C5SEQ END) = YR1.C2SEQ
                WHERE YR.C2SEQ = {0}", seq);
            var c = this.connection.EnsureOpened();
            return c.Query<GarantieTypePlatDto>(sql).ToList();
        }

        internal List<GarantieTypePlatDto> GetSousGarantie(long seq)
        {
            string sql = string.Format(@"SELECT * FROM YPLTGAR WHERE C2SEQ = {0} 
                    OR C2SEM = {0}
                    OR C2SEM IN (SELECT C2SEQ FROM YPLTGAR WHERE C2SEM = {0} AND C2SEQ != 0)
                    OR C2SE1 = {0}", seq);
            var c = this.connection.EnsureOpened();
            return c.Query<GarantieTypePlatDto>(sql).ToList();
        }

        internal  void SupprimerBySeq(long[] seqs)
        {
            string strSeqs = string.Join(",", seqs);
            string sql = "DELETE FROM YPLTGAR WHERE C2SEQ IN (" + strSeqs + ")";
            var test = this.connection.EnsureOpened().Execute(sql);
        }

        internal bool ExistCodeGarantie(string codeModele, string codeGarantie, long seqM)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPLTGAR 
                                WHERE C2MGA = '{0}' AND C2GAR = '{1}' AND C2SEM = {2}", codeModele, codeGarantie, seqM);
            var c = this.connection.EnsureOpened();
            return c.QuerySingle<int>(sql) > 0;
        }

        internal bool ExistLienModeleIncoherent(string type, long seqA, long seqB)
        {
            //Ajout possible : ajouter AND (YR1.C2CAR = 'O' OR YR2.C2CAR = 'O') dans la clause where si seulement garantie obligatoire
            string sql = string.Format(@"SELECT COUNT(*) FROM YPLTGAR YR1
            INNER JOIN YPLTGAA ON (YR1.C2SEQ = C5SEQ OR YR1.C2SEQ = C5SEM) AND C5TYP IN ({0})
            INNER JOIN YPLTGAR YR2 ON YR2.C2SEQ = (CASE WHEN YR1.C2SEQ = C5SEQ THEN C5SEM ELSE C5SEQ END)
            WHERE YR1.C2MGA = (SELECT C2MGA FROM YPLTGAR WHERE C2SEQ = {1})
            AND YR2.C2MGA = (SELECT C2MGA FROM YPLTGAR WHERE C2SEQ = {2})", type == "I" ? "'A','D'" : "'I'", seqA, seqB);
            var c = this.connection.EnsureOpened();
            return c.QuerySingle<int>(sql) > 0;
        }
    }
}
