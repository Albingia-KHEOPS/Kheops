using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.KPTRACE
    public partial class KPTrace  {
             //KPTRACE

            ///<summary>Public empty contructor</summary>
            public KPTrace() {}
            ///<summary>Public empty contructor</summary>
            public KPTrace(KPTrace copyFrom) 
            {
                  this.Kcctyp= copyFrom.Kcctyp;
                  this.Kccipb= copyFrom.Kccipb;
                  this.Kccalx= copyFrom.Kccalx;
                  this.Kccrsq= copyFrom.Kccrsq;
                  this.Kccobj= copyFrom.Kccobj;
                  this.Kccfor= copyFrom.Kccfor;
                  this.Kccopt= copyFrom.Kccopt;
                  this.Kccgar= copyFrom.Kccgar;
                  this.Kcccru= copyFrom.Kcccru;
                  this.Kcccrd= copyFrom.Kcccrd;
                  this.Kcccrh= copyFrom.Kcccrh;
                  this.Kcclib= copyFrom.Kcclib;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kcctyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kccipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kccalx { get; set; } 
            
            ///<summary> N° Risque </summary>
            public int Kccrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kccobj { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kccfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kccopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kccgar { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kcccru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kcccrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kcccrh { get; set; } 
            
            ///<summary> Lib variable mini 100 </summary>
            public string Kcclib { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KPTrace  x=this,  y=obj as KPTrace;
            if( y == default(KPTrace) ) return false;
            return (
                    x.Kcctyp==y.Kcctyp
                    && x.Kccipb==y.Kccipb
                    && x.Kccalx==y.Kccalx
                    && x.Kccrsq==y.Kccrsq
                    && x.Kccobj==y.Kccobj
                    && x.Kccfor==y.Kccfor
                    && x.Kccopt==y.Kccopt
                    && x.Kccgar==y.Kccgar
                    && x.Kcccru==y.Kcccru
                    && x.Kcccrd==y.Kcccrd
                    && x.Kcccrh==y.Kcccrh
                    && x.Kcclib==y.Kcclib  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kcctyp?? "").GetHashCode()
                      * 23 ) + (this.Kccipb?? "").GetHashCode()
                      * 23 ) + (this.Kccalx.GetHashCode() ) 
                      * 23 ) + (this.Kccrsq.GetHashCode() ) 
                      * 23 ) + (this.Kccobj.GetHashCode() ) 
                      * 23 ) + (this.Kccfor.GetHashCode() ) 
                      * 23 ) + (this.Kccopt.GetHashCode() ) 
                      * 23 ) + (this.Kccgar?? "").GetHashCode()
                      * 23 ) + (this.Kcccru?? "").GetHashCode()
                      * 23 ) + (this.Kcccrd.GetHashCode() ) 
                      * 23 ) + (this.Kcccrh.GetHashCode() ) 
                      * 23 ) + (this.Kcclib?? "").GetHashCode()                   );
           }
        }
    }
}
