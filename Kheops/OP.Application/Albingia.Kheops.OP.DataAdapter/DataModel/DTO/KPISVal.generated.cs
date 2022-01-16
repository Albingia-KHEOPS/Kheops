using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO
{
    // ZALBINKSPP.HPISVAL
    public partial class KPISVal
    {
        //HPISVAL
        //KPISVAL

        ///<summary>Public empty contructor</summary>
        public KPISVal() { }
        ///<summary>Public empty contructor</summary>
        public KPISVal(KPISVal copyFrom)
        {
            this.Kkcid = copyFrom.Kkcid;
            this.Kkctyp = copyFrom.Kkctyp;
            this.Kkcipb = copyFrom.Kkcipb;
            this.Kkcalx = copyFrom.Kkcalx;
            this.Kkcavn = copyFrom.Kkcavn;
            this.Kkchin = copyFrom.Kkchin;
            this.Kkcrsq = copyFrom.Kkcrsq;
            this.Kkcobj = copyFrom.Kkcobj;
            this.Kkcfor = copyFrom.Kkcfor;
            this.Kkcopt = copyFrom.Kkcopt;
            this.Kkckgbnmid = copyFrom.Kkckgbnmid;
            this.Kkcvdec = copyFrom.Kkcvdec;
            this.Kkcvun = copyFrom.Kkcvun;
            this.Kkcvdatd = copyFrom.Kkcvdatd;
            this.Kkcvheud = copyFrom.Kkcvheud;
            this.Kkcvdatf = copyFrom.Kkcvdatf;
            this.Kkcvheuf = copyFrom.Kkcvheuf;
            this.Kkcvtxt = copyFrom.Kkcvtxt;
            this.Kkckfbid = copyFrom.Kkckfbid;
            this.Kkcisval = copyFrom.Kkcisval;
        }

        ///<summary> Id unique Numéro chrono sur 15 </summary>
        public Int64 Kkcid { get; set; }

        ///<summary> Type  O Offre  P Police </summary>
        public string Kkctyp { get; set; }

        ///<summary> Offre Contrat </summary>
        public string Kkcipb { get; set; }

        ///<summary> Aliment/version </summary>
        public int Kkcalx { get; set; }

        ///<summary> avn </summary>
        public int Kkcavn { get; set; }

        ///<summary> N° historique par avenant </summary>
        public int Kkchin { get; set; }

        ///<summary> Rsq </summary>
        public int Kkcrsq { get; set; }

        ///<summary> obj </summary>
        public int Kkcobj { get; set; }

        ///<summary> formule </summary>
        public int Kkcfor { get; set; }

        ///<summary> Identifiant option </summary>
        public int Kkcopt { get; set; }

        ///<summary> Nom IS KISREF </summary>
        public string Kkckgbnmid { get; set; }

        ///<summary> Valeur numérique </summary>
        public Decimal Kkcvdec { get; set; }

        ///<summary> unité valeur KHEOP/KCVUN </summary>
        public string Kkcvun { get; set; }

        ///<summary> date début AAMJ </summary>
        public int Kkcvdatd { get; set; }

        ///<summary> heure début </summary>
        public int Kkcvheud { get; set; }

        ///<summary> date fin AAMJ </summary>
        public int Kkcvdatf { get; set; }

        ///<summary> heure fin </summary>
        public int Kkcvheuf { get; set; }

        ///<summary> valeur texte </summary>
        public string Kkcvtxt { get; set; }

        ///<summary> id désignation KPIDESI </summary>
        public int Kkckfbid { get; set; }

        ///<summary> Valeur </summary>
        public string Kkcisval { get; set; }



        public override bool Equals(object obj)
        {
            KPISVal x = this, y = obj as KPISVal;
            if (y == default(KPISVal)) return false;
            return (
                    x.Kkcid == y.Kkcid
                    && x.Kkctyp == y.Kkctyp
                    && x.Kkcipb == y.Kkcipb
                    && x.Kkcalx == y.Kkcalx
                    && x.Kkcavn == y.Kkcavn
                    && x.Kkchin == y.Kkchin
                    && x.Kkcrsq == y.Kkcrsq
                    && x.Kkcobj == y.Kkcobj
                    && x.Kkcfor == y.Kkcfor
                    && x.Kkcopt == y.Kkcopt
                    && x.Kkckgbnmid == y.Kkckgbnmid
                    && x.Kkcvdec == y.Kkcvdec
                    && x.Kkcvun == y.Kkcvun
                    && x.Kkcvdatd == y.Kkcvdatd
                    && x.Kkcvheud == y.Kkcvheud
                    && x.Kkcvdatf == y.Kkcvdatf
                    && x.Kkcvheuf == y.Kkcvheuf
                    && x.Kkcvtxt == y.Kkcvtxt
                    && x.Kkckfbid == y.Kkckfbid
                    && x.Kkcisval == y.Kkcisval
            );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return
                     ((((((((((((((((((((
                         17 + (this.Kkcid.GetHashCode())
                        * 23) + (this.Kkctyp ?? "").GetHashCode()
                        * 23) + (this.Kkcipb ?? "").GetHashCode()
                        * 23) + (this.Kkcalx.GetHashCode())
                        * 23) + (this.Kkcavn.GetHashCode())
                        * 23) + (this.Kkchin.GetHashCode())
                        * 23) + (this.Kkcrsq.GetHashCode())
                        * 23) + (this.Kkcobj.GetHashCode())
                        * 23) + (this.Kkcfor.GetHashCode())
                        * 23) + (this.Kkcopt.GetHashCode())
                        * 23) + (this.Kkckgbnmid ?? "").GetHashCode()
                        * 23) + (this.Kkcvdec.GetHashCode())
                        * 23) + (this.Kkcvun ?? "").GetHashCode()
                        * 23) + (this.Kkcvdatd.GetHashCode())
                        * 23) + (this.Kkcvheud.GetHashCode())
                        * 23) + (this.Kkcvdatf.GetHashCode())
                        * 23) + (this.Kkcvheuf.GetHashCode())
                        * 23) + (this.Kkcvtxt ?? "").GetHashCode()
                        * 23) + (this.Kkckfbid.GetHashCode())
                        * 23) + (this.Kkcisval ?? "").GetHashCode());
            }
        }
    }
}
