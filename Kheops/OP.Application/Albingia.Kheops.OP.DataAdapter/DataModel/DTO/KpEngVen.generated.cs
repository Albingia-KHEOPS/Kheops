using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPENGVEN
    public partial class KpEngVen  {
             //HPENGVEN
             //KPENGVEN

            ///<summary>Public empty contructor</summary>
            public KpEngVen() {}
            ///<summary>Public empty contructor</summary>
            public KpEngVen(KpEngVen copyFrom) 
            {
                  this.Kdqid= copyFrom.Kdqid;
                  this.Kdqtyp= copyFrom.Kdqtyp;
                  this.Kdqipb= copyFrom.Kdqipb;
                  this.Kdqalx= copyFrom.Kdqalx;
                  this.Kdqavn= copyFrom.Kdqavn;
                  this.Kdqhin= copyFrom.Kdqhin;
                  this.Kdqkdpid= copyFrom.Kdqkdpid;
                  this.Kdqfam= copyFrom.Kdqfam;
                  this.Kdqven= copyFrom.Kdqven;
                  this.Kdqengc= copyFrom.Kdqengc;
                  this.Kdqengf= copyFrom.Kdqengf;
                  this.Kdqengok= copyFrom.Kdqengok;
                  this.Kdqcru= copyFrom.Kdqcru;
                  this.Kdqcrd= copyFrom.Kdqcrd;
                  this.Kdqmaju= copyFrom.Kdqmaju;
                  this.Kdqmajd= copyFrom.Kdqmajd;
                  this.Kdqlct= copyFrom.Kdqlct;
                  this.Kdqcat= copyFrom.Kdqcat;
                  this.Kdqkdoid= copyFrom.Kdqkdoid;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdqid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdqtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdqipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdqalx { get; set; } 
            
            ///<summary> N°  avenant </summary>
            public int? Kdqavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdqhin { get; set; } 
            
            ///<summary> Lien KPENGFAM </summary>
            public Int64 Kdqkdpid { get; set; } 
            
            ///<summary> Famille de réassurance </summary>
            public string Kdqfam { get; set; } 
            
            ///<summary> Ventilation   (KREAVEN) </summary>
            public int Kdqven { get; set; } 
            
            ///<summary> Engagement Calculé cpt  100% </summary>
            public Int64 Kdqengc { get; set; } 
            
            ///<summary> Engagement Forcé cpt 100 % </summary>
            public Int64 Kdqengf { get; set; } 
            
            ///<summary> Entre dans engagement O/N </summary>
            public string Kdqengok { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kdqcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdqcrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdqmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdqmajd { get; set; } 
            
            ///<summary> LCI part Totale  100% </summary>
            public Int64 Kdqlct { get; set; } 
            
            ///<summary> Capitaux Part Totale 100% </summary>
            public Int64 Kdqcat { get; set; } 
            
            ///<summary> Lien KPENG </summary>
            public Int64 Kdqkdoid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpEngVen  x=this,  y=obj as KpEngVen;
            if( y == default(KpEngVen) ) return false;
            return (
                    x.Kdqid==y.Kdqid
                    && x.Kdqtyp==y.Kdqtyp
                    && x.Kdqipb==y.Kdqipb
                    && x.Kdqalx==y.Kdqalx
                    && x.Kdqkdpid==y.Kdqkdpid
                    && x.Kdqfam==y.Kdqfam
                    && x.Kdqven==y.Kdqven
                    && x.Kdqengc==y.Kdqengc
                    && x.Kdqengf==y.Kdqengf
                    && x.Kdqengok==y.Kdqengok
                    && x.Kdqcru==y.Kdqcru
                    && x.Kdqcrd==y.Kdqcrd
                    && x.Kdqmaju==y.Kdqmaju
                    && x.Kdqmajd==y.Kdqmajd
                    && x.Kdqlct==y.Kdqlct
                    && x.Kdqcat==y.Kdqcat
                    && x.Kdqkdoid==y.Kdqkdoid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((
                       17 + (this.Kdqid.GetHashCode() ) 
                      * 23 ) + (this.Kdqtyp?? "").GetHashCode()
                      * 23 ) + (this.Kdqipb?? "").GetHashCode()
                      * 23 ) + (this.Kdqalx.GetHashCode() ) 
                      * 23 ) + (this.Kdqkdpid.GetHashCode() ) 
                      * 23 ) + (this.Kdqfam?? "").GetHashCode()
                      * 23 ) + (this.Kdqven.GetHashCode() ) 
                      * 23 ) + (this.Kdqengc.GetHashCode() ) 
                      * 23 ) + (this.Kdqengf.GetHashCode() ) 
                      * 23 ) + (this.Kdqengok?? "").GetHashCode()
                      * 23 ) + (this.Kdqcru?? "").GetHashCode()
                      * 23 ) + (this.Kdqcrd.GetHashCode() ) 
                      * 23 ) + (this.Kdqmaju?? "").GetHashCode()
                      * 23 ) + (this.Kdqmajd.GetHashCode() ) 
                      * 23 ) + (this.Kdqlct.GetHashCode() ) 
                      * 23 ) + (this.Kdqcat.GetHashCode() ) 
                      * 23 ) + (this.Kdqkdoid.GetHashCode() )                    );
           }
        }
    }
}
