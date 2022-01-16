using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KDESI
    public partial class KDesi  {
             //KDESI

            ///<summary>Public empty contructor</summary>
            public KDesi() {}
            ///<summary>Public empty contructor</summary>
            public KDesi(KDesi copyFrom) 
            {
                  this.Kdwid= copyFrom.Kdwid;
                  this.Kdwdesi= copyFrom.Kdwdesi;
        
            }        
            
            ///<summary>  </summary>
            public int Kdwid { get; set; } 
            
            ///<summary>  </summary>
            public string Kdwdesi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KDesi  x=this,  y=obj as KDesi;
            if( y == default(KDesi) ) return false;
            return (
                    x.Kdwid==y.Kdwid
                    && x.Kdwdesi==y.Kdwdesi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((
                       17 + (this.Kdwid.GetHashCode() ) 
                      * 23 ) + (this.Kdwdesi?? "").GetHashCode()                   );
           }
        }
    }
}
