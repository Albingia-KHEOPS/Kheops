using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KEXPLCI
    public partial class KExpLci  {
             //KEXPLCI

            ///<summary>Public empty contructor</summary>
            public KExpLci() {}
            ///<summary>Public empty contructor</summary>
            public KExpLci(KExpLci copyFrom) 
            {
                  this.Khgid= copyFrom.Khgid;
                  this.Khglce= copyFrom.Khglce;
                  this.Khgdesc= copyFrom.Khgdesc;
                  this.Khgdesi= copyFrom.Khgdesi;
                  this.Khgmodi= copyFrom.Khgmodi;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Khgid { get; set; } 
            
            ///<summary> Expression complexe </summary>
            public string Khglce { get; set; } 
            
            ///<summary> Description </summary>
            public string Khgdesc { get; set; } 
            
            ///<summary> Lien KDESI </summary>
            public Int64 Khgdesi { get; set; } 
            
            ///<summary> Modifiable O/N </summary>
            public string Khgmodi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KExpLci  x=this,  y=obj as KExpLci;
            if( y == default(KExpLci) ) return false;
            return (
                    x.Khgid==y.Khgid
                    && x.Khglce==y.Khglce
                    && x.Khgdesc==y.Khgdesc
                    && x.Khgdesi==y.Khgdesi
                    && x.Khgmodi==y.Khgmodi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((
                       17 + (this.Khgid.GetHashCode() ) 
                      * 23 ) + (this.Khglce?? "").GetHashCode()
                      * 23 ) + (this.Khgdesc?? "").GetHashCode()
                      * 23 ) + (this.Khgdesi.GetHashCode() ) 
                      * 23 ) + (this.Khgmodi?? "").GetHashCode()                   );
           }
        }
    }
}
