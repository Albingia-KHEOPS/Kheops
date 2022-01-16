using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KPCTRLA
    public partial class KpCtrlA  {
             //KPCTRLA

            ///<summary>Public empty contructor</summary>
            public KpCtrlA() {}
            ///<summary>Public empty contructor</summary>
            public KpCtrlA(KpCtrlA copyFrom) 
            {
                  this.Kgttyp= copyFrom.Kgttyp;
                  this.Kgtipb= copyFrom.Kgtipb;
                  this.Kgtalx= copyFrom.Kgtalx;
                  this.Kgtetape= copyFrom.Kgtetape;
                  this.Kgtlib= copyFrom.Kgtlib;
                  this.Kgtcru= copyFrom.Kgtcru;
                  this.Kgtcrd= copyFrom.Kgtcrd;
                  this.Kgtcrh= copyFrom.Kgtcrh;
                  this.Kgtmaju= copyFrom.Kgtmaju;
                  this.Kgtmajd= copyFrom.Kgtmajd;
                  this.Kgtmajh= copyFrom.Kgtmajh;
        
            }        
            
            ///<summary> TYP O/P </summary>
            public string Kgttyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kgtipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kgtalx { get; set; } 
            
            ///<summary> Etape de modification </summary>
            public string Kgtetape { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Kgtlib { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kgtcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kgtcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kgtcrh { get; set; } 
            
            ///<summary> Màj User </summary>
            public string Kgtmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kgtmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kgtmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCtrlA  x=this,  y=obj as KpCtrlA;
            if( y == default(KpCtrlA) ) return false;
            return (
                    x.Kgttyp==y.Kgttyp
                    && x.Kgtipb==y.Kgtipb
                    && x.Kgtalx==y.Kgtalx
                    && x.Kgtetape==y.Kgtetape
                    && x.Kgtlib==y.Kgtlib
                    && x.Kgtcru==y.Kgtcru
                    && x.Kgtcrd==y.Kgtcrd
                    && x.Kgtcrh==y.Kgtcrh
                    && x.Kgtmaju==y.Kgtmaju
                    && x.Kgtmajd==y.Kgtmajd
                    && x.Kgtmajh==y.Kgtmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((
                       17 + (this.Kgttyp?? "").GetHashCode()
                      * 23 ) + (this.Kgtipb?? "").GetHashCode()
                      * 23 ) + (this.Kgtalx.GetHashCode() ) 
                      * 23 ) + (this.Kgtetape?? "").GetHashCode()
                      * 23 ) + (this.Kgtlib?? "").GetHashCode()
                      * 23 ) + (this.Kgtcru?? "").GetHashCode()
                      * 23 ) + (this.Kgtcrd.GetHashCode() ) 
                      * 23 ) + (this.Kgtcrh.GetHashCode() ) 
                      * 23 ) + (this.Kgtmaju?? "").GetHashCode()
                      * 23 ) + (this.Kgtmajd.GetHashCode() ) 
                      * 23 ) + (this.Kgtmajh.GetHashCode() )                    );
           }
        }
    }
}
