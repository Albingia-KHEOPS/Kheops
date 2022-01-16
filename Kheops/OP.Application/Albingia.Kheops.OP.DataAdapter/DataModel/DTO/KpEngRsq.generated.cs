using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.HPENGRSQ
    public partial class KpEngRsq
    {
        //HPENGRSQ
        //KPENGRSQ

        ///<summary>Public empty contructor</summary>
        public KpEngRsq() { }
        ///<summary>Public empty contructor</summary>
        public KpEngRsq(KpEngRsq copyFrom)
        {
            this.Kdrid = copyFrom.Kdrid;
            this.Kdrtyp = copyFrom.Kdrtyp;
            this.Kdripb = copyFrom.Kdripb;
            this.Kdralx = copyFrom.Kdralx;
            this.Kdravn = copyFrom.Kdravn;
            this.Kdrhin = copyFrom.Kdrhin;
            this.Kdrrsq = copyFrom.Kdrrsq;
            this.Kdrkdqid = copyFrom.Kdrkdqid;
            this.Kdrfam = copyFrom.Kdrfam;
            this.Kdrven = copyFrom.Kdrven;
            this.Kdrlci = copyFrom.Kdrlci;
            this.Kdrsmp = copyFrom.Kdrsmp;
            this.Kdrengc = copyFrom.Kdrengc;
            this.Kdrengf = copyFrom.Kdrengf;
            this.Kdrengok = copyFrom.Kdrengok;
            this.Kdrcru = copyFrom.Kdrcru;
            this.Kdrcrd = copyFrom.Kdrcrd;
            this.Kdrmaju = copyFrom.Kdrmaju;
            this.Kdrmajd = copyFrom.Kdrmajd;
            this.Kdrkdoid = copyFrom.Kdrkdoid;
            this.Kdrcat = copyFrom.Kdrcat;
            this.Kdrsmf = copyFrom.Kdrsmf;

        }

        ///<summary> ID unique </summary>
        public Int64 Kdrid { get; set; }

        ///<summary> Type O/P </summary>
        public string Kdrtyp { get; set; }

        ///<summary> IPB </summary>
        public string Kdripb { get; set; }

        ///<summary> ALX </summary>
        public int Kdralx { get; set; }

        ///<summary> N° avenant </summary>
        public int? Kdravn { get; set; }

        ///<summary> N° histo par avenant </summary>
        public int? Kdrhin { get; set; }

        ///<summary> Risque </summary>
        public int Kdrrsq { get; set; }

        ///<summary> Lien KPENGVEN </summary>
        public Int64 Kdrkdqid { get; set; }

        ///<summary> Famille de réassurance </summary>
        public string Kdrfam { get; set; }

        ///<summary> Ventilation (KREAVEN) </summary>
        public int Kdrven { get; set; }

        ///<summary> LCI cpt 100% </summary>
        public Int64 Kdrlci { get; set; }

        ///<summary> SMP cpt 100% </summary>
        public Int64 Kdrsmp { get; set; }

        ///<summary> Engagement Calculé cpt  100% </summary>
        public Int64 Kdrengc { get; set; }

        ///<summary> Engagement Forcé cpt  100% </summary>
        public Int64 Kdrengf { get; set; }

        ///<summary> Entre dans engagement O/N </summary>
        public string Kdrengok { get; set; }

        ///<summary> Création User </summary>
        public string Kdrcru { get; set; }

        ///<summary> Création Date </summary>
        public int Kdrcrd { get; set; }

        ///<summary> MAJ User </summary>
        public string Kdrmaju { get; set; }

        ///<summary> MAJ Date </summary>
        public int Kdrmajd { get; set; }

        ///<summary> Lien KPENG </summary>
        public Int64 Kdrkdoid { get; set; }

        ///<summary> Capitaux 100% </summary>
        public Int64 Kdrcat { get; set; }

        ///<summary> SMP Forcé cpt  100% </summary>
        public Int64 Kdrsmf { get; set; }



        public override bool Equals(object obj)
        {
            KpEngRsq x = this, y = obj as KpEngRsq;
            if (y == default(KpEngRsq)) return false;
            return (
                    x.Kdrid == y.Kdrid
                    && x.Kdrtyp == y.Kdrtyp
                    && x.Kdripb == y.Kdripb
                    && x.Kdralx == y.Kdralx
                    && x.Kdrrsq == y.Kdrrsq
                    && x.Kdrkdqid == y.Kdrkdqid
                    && x.Kdrfam == y.Kdrfam
                    && x.Kdrven == y.Kdrven
                    && x.Kdrlci == y.Kdrlci
                    && x.Kdrsmp == y.Kdrsmp
                    && x.Kdrengc == y.Kdrengc
                    && x.Kdrengf == y.Kdrengf
                    && x.Kdrengok == y.Kdrengok
                    && x.Kdrcru == y.Kdrcru
                    && x.Kdrcrd == y.Kdrcrd
                    && x.Kdrmaju == y.Kdrmaju
                    && x.Kdrmajd == y.Kdrmajd
                    && x.Kdrkdoid == y.Kdrkdoid
                    && x.Kdrcat == y.Kdrcat
                    && x.Kdrsmf == y.Kdrsmf
            );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return
                     ((((((((((((((((((((
                         17 + (this.Kdrid.GetHashCode())
                        * 23) + (this.Kdrtyp ?? "").GetHashCode()
                        * 23) + (this.Kdripb ?? "").GetHashCode()
                        * 23) + (this.Kdralx.GetHashCode())
                        * 23) + (this.Kdrrsq.GetHashCode())
                        * 23) + (this.Kdrkdqid.GetHashCode())
                        * 23) + (this.Kdrfam ?? "").GetHashCode()
                        * 23) + (this.Kdrven.GetHashCode())
                        * 23) + (this.Kdrlci.GetHashCode())
                        * 23) + (this.Kdrsmp.GetHashCode())
                        * 23) + (this.Kdrengc.GetHashCode())
                        * 23) + (this.Kdrengf.GetHashCode())
                        * 23) + (this.Kdrengok ?? "").GetHashCode()
                        * 23) + (this.Kdrcru ?? "").GetHashCode()
                        * 23) + (this.Kdrcrd.GetHashCode())
                        * 23) + (this.Kdrmaju ?? "").GetHashCode()
                        * 23) + (this.Kdrmajd.GetHashCode())
                        * 23) + (this.Kdrkdoid.GetHashCode())
                        * 23) + (this.Kdrcat.GetHashCode())
                        * 23) + (this.Kdrsmf.GetHashCode()));
            }
        }
    }
}
