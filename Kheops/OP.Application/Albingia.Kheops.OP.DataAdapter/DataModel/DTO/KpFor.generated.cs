using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPFOR
    public partial class KpFor  {
             //HPFOR
             //KPFOR

            ///<summary>Public empty contructor</summary>
            public KpFor() {}
            ///<summary>Public empty contructor</summary>
            public KpFor(KpFor copyFrom) 
            {
                  this.Kdaid= copyFrom.Kdaid;
                  this.Kdatyp= copyFrom.Kdatyp;
                  this.Kdaipb= copyFrom.Kdaipb;
                  this.Kdaalx= copyFrom.Kdaalx;
                  this.Kdaavn= copyFrom.Kdaavn;
                  this.Kdahin= copyFrom.Kdahin;
                  this.Kdafor= copyFrom.Kdafor;
                  this.Kdacch= copyFrom.Kdacch;
                  this.Kdaalpha= copyFrom.Kdaalpha;
                  this.Kdabra= copyFrom.Kdabra;
                  this.Kdacible= copyFrom.Kdacible;
                  this.Kdakaiid= copyFrom.Kdakaiid;
                  this.Kdadesc= copyFrom.Kdadesc;
                  this.Kdacru= copyFrom.Kdacru;
                  this.Kdacrd= copyFrom.Kdacrd;
                  this.Kdamaju= copyFrom.Kdamaju;
                  this.Kdamajd= copyFrom.Kdamajd;
                  this.Kdafgen= copyFrom.Kdafgen;
        
            }        
            
            ///<summary> ID Unique </summary>
            public Int64 Kdaid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdatyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdaipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdaalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdaavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdahin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdafor { get; set; } 
            
            ///<summary> N° Chrono </summary>
            public int Kdacch { get; set; } 
            
            ///<summary> Code alpha </summary>
            public string Kdaalpha { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kdabra { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kdacible { get; set; } 
            
            ///<summary> Lien KCIBLEF </summary>
            public Int64 Kdakaiid { get; set; } 
            
            ///<summary> Description </summary>
            public string Kdadesc { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kdacru { get; set; } 
            
            ///<summary> Création date </summary>
            public int Kdacrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdamaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdamajd { get; set; } 
            
            ///<summary> Formule Générale O/N </summary>
            public string Kdafgen { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpFor  x=this,  y=obj as KpFor;
            if( y == default(KpFor) ) return false;
            return (
                    x.Kdaid==y.Kdaid
                    && x.Kdatyp==y.Kdatyp
                    && x.Kdaipb==y.Kdaipb
                    && x.Kdaalx==y.Kdaalx
                    && x.Kdafor==y.Kdafor
                    && x.Kdacch==y.Kdacch
                    && x.Kdaalpha==y.Kdaalpha
                    && x.Kdabra==y.Kdabra
                    && x.Kdacible==y.Kdacible
                    && x.Kdakaiid==y.Kdakaiid
                    && x.Kdadesc==y.Kdadesc
                    && x.Kdacru==y.Kdacru
                    && x.Kdacrd==y.Kdacrd
                    && x.Kdamaju==y.Kdamaju
                    && x.Kdamajd==y.Kdamajd
                    && x.Kdafgen==y.Kdafgen  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((
                       17 + (this.Kdaid.GetHashCode() ) 
                      * 23 ) + (this.Kdatyp?? "").GetHashCode()
                      * 23 ) + (this.Kdaipb?? "").GetHashCode()
                      * 23 ) + (this.Kdaalx.GetHashCode() ) 
                      * 23 ) + (this.Kdafor.GetHashCode() ) 
                      * 23 ) + (this.Kdacch.GetHashCode() ) 
                      * 23 ) + (this.Kdaalpha?? "").GetHashCode()
                      * 23 ) + (this.Kdabra?? "").GetHashCode()
                      * 23 ) + (this.Kdacible?? "").GetHashCode()
                      * 23 ) + (this.Kdakaiid.GetHashCode() ) 
                      * 23 ) + (this.Kdadesc?? "").GetHashCode()
                      * 23 ) + (this.Kdacru?? "").GetHashCode()
                      * 23 ) + (this.Kdacrd.GetHashCode() ) 
                      * 23 ) + (this.Kdamaju?? "").GetHashCode()
                      * 23 ) + (this.Kdamajd.GetHashCode() ) 
                      * 23 ) + (this.Kdafgen?? "").GetHashCode()                   );
           }
        }
    }
}
