using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPENGGAR
    public partial class KpEngGar  {
             //HPENGGAR
             //KPENGGAR

            ///<summary>Public empty contructor</summary>
            public KpEngGar() {}
            ///<summary>Public empty contructor</summary>
            public KpEngGar(KpEngGar copyFrom) 
            {
                  this.Kdsid= copyFrom.Kdsid;
                  this.Kdstyp= copyFrom.Kdstyp;
                  this.Kdsipb= copyFrom.Kdsipb;
                  this.Kdsalx= copyFrom.Kdsalx;
                  this.Kdsavn= copyFrom.Kdsavn;
                  this.Kdshin= copyFrom.Kdshin;
                  this.Kdsrsq= copyFrom.Kdsrsq;
                  this.Kdsfam= copyFrom.Kdsfam;
                  this.Kdsven= copyFrom.Kdsven;
                  this.Kdskdrid= copyFrom.Kdskdrid;
                  this.Kdsgaran= copyFrom.Kdsgaran;
                  this.Kdsengok= copyFrom.Kdsengok;
                  this.Kdslci= copyFrom.Kdslci;
                  this.Kdssmp= copyFrom.Kdssmp;
                  this.Kdssmpf= copyFrom.Kdssmpf;
                  this.Kdssmpu= copyFrom.Kdssmpu;
                  this.Kdssmpr= copyFrom.Kdssmpr;
                  this.Kdscru= copyFrom.Kdscru;
                  this.Kdscrd= copyFrom.Kdscrd;
                  this.Kdsmaju= copyFrom.Kdsmaju;
                  this.Kdsmajd= copyFrom.Kdsmajd;
                  this.Kdskdoid= copyFrom.Kdskdoid;
                  this.Kdscat= copyFrom.Kdscat;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdsid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdstyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdsipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdsalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdsavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdshin { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kdsrsq { get; set; } 
            
            ///<summary> Famille de réassurance </summary>
            public string Kdsfam { get; set; } 
            
            ///<summary> Ventilation  (KREAVEN) </summary>
            public int Kdsven { get; set; } 
            
            ///<summary> Lien KPENGRSQ </summary>
            public Int64 Kdskdrid { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kdsgaran { get; set; } 
            
            ///<summary> Entre dans engagement O/N </summary>
            public string Kdsengok { get; set; } 
            
            ///<summary> LCI cpt 100% </summary>
            public Int64 Kdslci { get; set; } 
            
            ///<summary> SMP cpt 100% </summary>
            public Int64 Kdssmp { get; set; } 
            
            ///<summary> SMP Forcé cpt 100% </summary>
            public Int64 Kdssmpf { get; set; } 
            
            ///<summary> Unité SMP  % </summary>
            public string Kdssmpu { get; set; } 
            
            ///<summary> SMP retenu cpt 100% </summary>
            public Int64 Kdssmpr { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kdscru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdscrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdsmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdsmajd { get; set; } 
            
            ///<summary> Lien KPENG </summary>
            public Int64 Kdskdoid { get; set; } 
            
            ///<summary> Capitaux 100 % </summary>
            public Int64 Kdscat { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpEngGar  x=this,  y=obj as KpEngGar;
            if( y == default(KpEngGar) ) return false;
            return (
                    x.Kdsid==y.Kdsid
                    && x.Kdstyp==y.Kdstyp
                    && x.Kdsipb==y.Kdsipb
                    && x.Kdsalx==y.Kdsalx
                    && x.Kdsrsq==y.Kdsrsq
                    && x.Kdsfam==y.Kdsfam
                    && x.Kdsven==y.Kdsven
                    && x.Kdskdrid==y.Kdskdrid
                    && x.Kdsgaran==y.Kdsgaran
                    && x.Kdsengok==y.Kdsengok
                    && x.Kdslci==y.Kdslci
                    && x.Kdssmp==y.Kdssmp
                    && x.Kdssmpf==y.Kdssmpf
                    && x.Kdssmpu==y.Kdssmpu
                    && x.Kdssmpr==y.Kdssmpr
                    && x.Kdscru==y.Kdscru
                    && x.Kdscrd==y.Kdscrd
                    && x.Kdsmaju==y.Kdsmaju
                    && x.Kdsmajd==y.Kdsmajd
                    && x.Kdskdoid==y.Kdskdoid
                    && x.Kdscat==y.Kdscat  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((
                       17 + (this.Kdsid.GetHashCode() ) 
                      * 23 ) + (this.Kdstyp?? "").GetHashCode()
                      * 23 ) + (this.Kdsipb?? "").GetHashCode()
                      * 23 ) + (this.Kdsalx.GetHashCode() ) 
                      * 23 ) + (this.Kdsrsq.GetHashCode() ) 
                      * 23 ) + (this.Kdsfam?? "").GetHashCode()
                      * 23 ) + (this.Kdsven.GetHashCode() ) 
                      * 23 ) + (this.Kdskdrid.GetHashCode() ) 
                      * 23 ) + (this.Kdsgaran?? "").GetHashCode()
                      * 23 ) + (this.Kdsengok?? "").GetHashCode()
                      * 23 ) + (this.Kdslci.GetHashCode() ) 
                      * 23 ) + (this.Kdssmp.GetHashCode() ) 
                      * 23 ) + (this.Kdssmpf.GetHashCode() ) 
                      * 23 ) + (this.Kdssmpu?? "").GetHashCode()
                      * 23 ) + (this.Kdssmpr.GetHashCode() ) 
                      * 23 ) + (this.Kdscru?? "").GetHashCode()
                      * 23 ) + (this.Kdscrd.GetHashCode() ) 
                      * 23 ) + (this.Kdsmaju?? "").GetHashCode()
                      * 23 ) + (this.Kdsmajd.GetHashCode() ) 
                      * 23 ) + (this.Kdskdoid.GetHashCode() ) 
                      * 23 ) + (this.Kdscat.GetHashCode() )                    );
           }
        }
    }
}
