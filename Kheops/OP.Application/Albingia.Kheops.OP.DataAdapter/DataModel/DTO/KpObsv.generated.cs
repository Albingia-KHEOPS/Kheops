using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPOBSV
    public partial class KpObsv  {
             //HPOBSV
             //KPOBSV

            ///<summary>Public empty contructor</summary>
            public KpObsv() {}
            ///<summary>Public empty contructor</summary>
            public KpObsv(KpObsv copyFrom) 
            {
                  this.Kajchr= copyFrom.Kajchr;
                  this.Kajtyp= copyFrom.Kajtyp;
                  this.Kajipb= copyFrom.Kajipb;
                  this.Kajalx= copyFrom.Kajalx;
                  this.Kajavn= copyFrom.Kajavn;
                  this.Kajhin= copyFrom.Kajhin;
                  this.Kajtypobs= copyFrom.Kajtypobs;
                  this.Kajobsv= copyFrom.Kajobsv;
        
            }        
            
            ///<summary>  </summary>
            public int Kajchr { get; set; } 
            
            ///<summary>  </summary>
            public string Kajtyp { get; set; } 
            
            ///<summary>  </summary>
            public string Kajipb { get; set; } 
            
            ///<summary>  </summary>
            public int Kajalx { get; set; } 
            
            ///<summary>  </summary>
            public int? Kajavn { get; set; } 
            
            ///<summary>  </summary>
            public int? Kajhin { get; set; } 
            
            ///<summary>  </summary>
            public string Kajtypobs { get; set; } 
            
            ///<summary>  </summary>
            public string Kajobsv { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpObsv  x=this,  y=obj as KpObsv;
            if( y == default(KpObsv) ) return false;
            return (
                    x.Kajchr==y.Kajchr
                    && x.Kajtyp==y.Kajtyp
                    && x.Kajipb==y.Kajipb
                    && x.Kajalx==y.Kajalx
                    && x.Kajtypobs==y.Kajtypobs
                    && x.Kajobsv==y.Kajobsv  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Kajchr.GetHashCode() ) 
                      * 23 ) + (this.Kajtyp?? "").GetHashCode()
                      * 23 ) + (this.Kajipb?? "").GetHashCode()
                      * 23 ) + (this.Kajalx.GetHashCode() ) 
                      * 23 ) + (this.Kajtypobs?? "").GetHashCode()
                      * 23 ) + (this.Kajobsv?? "").GetHashCode()                   );
           }
        }
    }
}
