using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KCANEV
    public partial class KCanev  {
             //KCANEV

            ///<summary>Public empty contructor</summary>
            public KCanev() {}
            ///<summary>Public empty contructor</summary>
            public KCanev(KCanev copyFrom) 
            {
                  this.Kgoid= copyFrom.Kgoid;
                  this.Kgotyp= copyFrom.Kgotyp;
                  this.Kgocnva= copyFrom.Kgocnva;
                  this.Kgodesc= copyFrom.Kgodesc;
                  this.Kgokaiid= copyFrom.Kgokaiid;
                  this.Kgocdef= copyFrom.Kgocdef;
                  this.Kgocru= copyFrom.Kgocru;
                  this.Kgocrd= copyFrom.Kgocrd;
                  this.Kgocrh= copyFrom.Kgocrh;
                  this.Kgomaju= copyFrom.Kgomaju;
                  this.Kgomajd= copyFrom.Kgomajd;
                  this.Kgomajh= copyFrom.Kgomajh;
                  this.Kgosit= copyFrom.Kgosit;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kgoid { get; set; } 
            
            ///<summary> Type O Offre / P Contrat </summary>
            public string Kgotyp { get; set; } 
            
            ///<summary> Code canevas CNVA**** (IPB) </summary>
            public string Kgocnva { get; set; } 
            
            ///<summary> Description </summary>
            public string Kgodesc { get; set; } 
            
            ///<summary> Lien KCIBLEF </summary>
            public Int64 Kgokaiid { get; set; } 
            
            ///<summary> Canevas par défaut  O/N </summary>
            public string Kgocdef { get; set; } 
            
            ///<summary> Création user </summary>
            public string Kgocru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kgocrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kgocrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kgomaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kgomajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kgomajh { get; set; } 
            
            ///<summary> Code situation 'N' 'V' </summary>
            public string Kgosit { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KCanev  x=this,  y=obj as KCanev;
            if( y == default(KCanev) ) return false;
            return (
                    x.Kgoid==y.Kgoid
                    && x.Kgotyp==y.Kgotyp
                    && x.Kgocnva==y.Kgocnva
                    && x.Kgodesc==y.Kgodesc
                    && x.Kgokaiid==y.Kgokaiid
                    && x.Kgocdef==y.Kgocdef
                    && x.Kgocru==y.Kgocru
                    && x.Kgocrd==y.Kgocrd
                    && x.Kgocrh==y.Kgocrh
                    && x.Kgomaju==y.Kgomaju
                    && x.Kgomajd==y.Kgomajd
                    && x.Kgomajh==y.Kgomajh
                    && x.Kgosit==y.Kgosit  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((
                       17 + (this.Kgoid.GetHashCode() ) 
                      * 23 ) + (this.Kgotyp?? "").GetHashCode()
                      * 23 ) + (this.Kgocnva?? "").GetHashCode()
                      * 23 ) + (this.Kgodesc?? "").GetHashCode()
                      * 23 ) + (this.Kgokaiid.GetHashCode() ) 
                      * 23 ) + (this.Kgocdef?? "").GetHashCode()
                      * 23 ) + (this.Kgocru?? "").GetHashCode()
                      * 23 ) + (this.Kgocrd.GetHashCode() ) 
                      * 23 ) + (this.Kgocrh.GetHashCode() ) 
                      * 23 ) + (this.Kgomaju?? "").GetHashCode()
                      * 23 ) + (this.Kgomajd.GetHashCode() ) 
                      * 23 ) + (this.Kgomajh.GetHashCode() ) 
                      * 23 ) + (this.Kgosit?? "").GetHashCode()                   );
           }
        }
    }
}
