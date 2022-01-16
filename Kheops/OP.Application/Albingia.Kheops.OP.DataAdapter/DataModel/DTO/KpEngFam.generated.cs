using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPENGFAM
    public partial class KpEngFam  {
             //HPENGFAM
             //KPENGFAM

            ///<summary>Public empty contructor</summary>
            public KpEngFam() {}
            ///<summary>Public empty contructor</summary>
            public KpEngFam(KpEngFam copyFrom) 
            {
                  this.Kdpid= copyFrom.Kdpid;
                  this.Kdptyp= copyFrom.Kdptyp;
                  this.Kdpipb= copyFrom.Kdpipb;
                  this.Kdpalx= copyFrom.Kdpalx;
                  this.Kdpavn= copyFrom.Kdpavn;
                  this.Kdphin= copyFrom.Kdphin;
                  this.Kdpkdoid= copyFrom.Kdpkdoid;
                  this.Kdpfam= copyFrom.Kdpfam;
                  this.Kdpeng= copyFrom.Kdpeng;
                  this.Kdpena= copyFrom.Kdpena;
                  this.Kdpcru= copyFrom.Kdpcru;
                  this.Kdpcrd= copyFrom.Kdpcrd;
                  this.Kdpmaju= copyFrom.Kdpmaju;
                  this.Kdpmajd= copyFrom.Kdpmajd;
                  this.Kdplct= copyFrom.Kdplct;
                  this.Kdplca= copyFrom.Kdplca;
                  this.Kdpcat= copyFrom.Kdpcat;
                  this.Kdpcaa= copyFrom.Kdpcaa;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdpid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdptyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdpipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdpalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdpavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdphin { get; set; } 
            
            ///<summary> Lien KPENG </summary>
            public Int64 Kdpkdoid { get; set; } 
            
            ///<summary> Famille de réassurance </summary>
            public string Kdpfam { get; set; } 
            
            ///<summary> Engagement cpt  100% </summary>
            public Int64 Kdpeng { get; set; } 
            
            ///<summary> Engagement ALB cpt </summary>
            public Int64 Kdpena { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kdpcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdpcrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdpmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdpmajd { get; set; } 
            
            ///<summary> LCI 100% </summary>
            public Int64 Kdplct { get; set; } 
            
            ///<summary> LCI Part Alb </summary>
            public Int64 Kdplca { get; set; } 
            
            ///<summary> Capitaux 100% </summary>
            public Int64 Kdpcat { get; set; } 
            
            ///<summary> Capitaux Part ALb </summary>
            public Int64 Kdpcaa { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpEngFam  x=this,  y=obj as KpEngFam;
            if( y == default(KpEngFam) ) return false;
            return (
                    x.Kdpid==y.Kdpid
                    && x.Kdptyp==y.Kdptyp
                    && x.Kdpipb==y.Kdpipb
                    && x.Kdpalx==y.Kdpalx
                    && x.Kdpkdoid==y.Kdpkdoid
                    && x.Kdpfam==y.Kdpfam
                    && x.Kdpeng==y.Kdpeng
                    && x.Kdpena==y.Kdpena
                    && x.Kdpcru==y.Kdpcru
                    && x.Kdpcrd==y.Kdpcrd
                    && x.Kdpmaju==y.Kdpmaju
                    && x.Kdpmajd==y.Kdpmajd
                    && x.Kdplct==y.Kdplct
                    && x.Kdplca==y.Kdplca
                    && x.Kdpcat==y.Kdpcat
                    && x.Kdpcaa==y.Kdpcaa  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((
                       17 + (this.Kdpid.GetHashCode() ) 
                      * 23 ) + (this.Kdptyp?? "").GetHashCode()
                      * 23 ) + (this.Kdpipb?? "").GetHashCode()
                      * 23 ) + (this.Kdpalx.GetHashCode() ) 
                      * 23 ) + (this.Kdpkdoid.GetHashCode() ) 
                      * 23 ) + (this.Kdpfam?? "").GetHashCode()
                      * 23 ) + (this.Kdpeng.GetHashCode() ) 
                      * 23 ) + (this.Kdpena.GetHashCode() ) 
                      * 23 ) + (this.Kdpcru?? "").GetHashCode()
                      * 23 ) + (this.Kdpcrd.GetHashCode() ) 
                      * 23 ) + (this.Kdpmaju?? "").GetHashCode()
                      * 23 ) + (this.Kdpmajd.GetHashCode() ) 
                      * 23 ) + (this.Kdplct.GetHashCode() ) 
                      * 23 ) + (this.Kdplca.GetHashCode() ) 
                      * 23 ) + (this.Kdpcat.GetHashCode() ) 
                      * 23 ) + (this.Kdpcaa.GetHashCode() )                    );
           }
        }
    }
}
