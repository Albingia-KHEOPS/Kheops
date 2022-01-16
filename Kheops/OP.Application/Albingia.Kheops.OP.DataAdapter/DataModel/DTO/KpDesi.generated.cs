using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPDESI
    public partial class KpDesi  {
             //HPDESI
             //KPDESI

            ///<summary>Public empty contructor</summary>
            public KpDesi() {}
            ///<summary>Public empty contructor</summary>
            public KpDesi(KpDesi copyFrom) 
            {
                  this.Kadchr= copyFrom.Kadchr;
                  this.Kadtyp= copyFrom.Kadtyp;
                  this.Kadipb= copyFrom.Kadipb;
                  this.Kadalx= copyFrom.Kadalx;
                  this.Kadavn= copyFrom.Kadavn;
                  this.Kadhin= copyFrom.Kadhin;
                  this.Kadperi= copyFrom.Kadperi;
                  this.Kadrsq= copyFrom.Kadrsq;
                  this.Kadobj= copyFrom.Kadobj;
                  this.Kaddesi= copyFrom.Kaddesi;
        
            }        
            
            ///<summary>  </summary>
            public int Kadchr { get; set; } 
            
            ///<summary>  </summary>
            public string Kadtyp { get; set; } 
            
            ///<summary>  </summary>
            public string Kadipb { get; set; } 
            
            ///<summary>  </summary>
            public int Kadalx { get; set; } 
            
            ///<summary>  </summary>
            public int? Kadavn { get; set; } 
            
            ///<summary>  </summary>
            public int? Kadhin { get; set; } 
            
            ///<summary>  </summary>
            public string Kadperi { get; set; } 
            
            ///<summary>  </summary>
            public int Kadrsq { get; set; } 
            
            ///<summary>  </summary>
            public int Kadobj { get; set; } 
            
            ///<summary>  </summary>
            public string Kaddesi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpDesi  x=this,  y=obj as KpDesi;
            if( y == default(KpDesi) ) return false;
            return (
                    x.Kadchr==y.Kadchr
                    && x.Kadtyp==y.Kadtyp
                    && x.Kadipb==y.Kadipb
                    && x.Kadalx==y.Kadalx
                    && x.Kadperi==y.Kadperi
                    && x.Kadrsq==y.Kadrsq
                    && x.Kadobj==y.Kadobj
                    && x.Kaddesi==y.Kaddesi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((
                       17 + (this.Kadchr.GetHashCode() ) 
                      * 23 ) + (this.Kadtyp?? "").GetHashCode()
                      * 23 ) + (this.Kadipb?? "").GetHashCode()
                      * 23 ) + (this.Kadalx.GetHashCode() ) 
                      * 23 ) + (this.Kadperi?? "").GetHashCode()
                      * 23 ) + (this.Kadrsq.GetHashCode() ) 
                      * 23 ) + (this.Kadobj.GetHashCode() ) 
                      * 23 ) + (this.Kaddesi?? "").GetHashCode()                   );
           }
        }
    }
}
