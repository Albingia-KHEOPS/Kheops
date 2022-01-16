using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KEXPFRH
    public partial class KExpFrh  {
             //KEXPFRH

            ///<summary>Public empty contructor</summary>
            public KExpFrh() {}
            ///<summary>Public empty contructor</summary>
            public KExpFrh(KExpFrh copyFrom) 
            {
                  this.Kheid= copyFrom.Kheid;
                  this.Khefhe= copyFrom.Khefhe;
                  this.Khedesc= copyFrom.Khedesc;
                  this.Khedesi= copyFrom.Khedesi;
                  this.Khemodi= copyFrom.Khemodi;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kheid { get; set; } 
            
            ///<summary> Expression Complexe </summary>
            public string Khefhe { get; set; } 
            
            ///<summary> Description </summary>
            public string Khedesc { get; set; } 
            
            ///<summary> Lien KDESI </summary>
            public Int64 Khedesi { get; set; } 
            
            ///<summary> Modifiable O/N </summary>
            public string Khemodi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KExpFrh  x=this,  y=obj as KExpFrh;
            if( y == default(KExpFrh) ) return false;
            return (
                    x.Kheid==y.Kheid
                    && x.Khefhe==y.Khefhe
                    && x.Khedesc==y.Khedesc
                    && x.Khedesi==y.Khedesi
                    && x.Khemodi==y.Khemodi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((
                       17 + (this.Kheid.GetHashCode() ) 
                      * 23 ) + (this.Khefhe?? "").GetHashCode()
                      * 23 ) + (this.Khedesc?? "").GetHashCode()
                      * 23 ) + (this.Khedesi.GetHashCode() ) 
                      * 23 ) + (this.Khemodi?? "").GetHashCode()                   );
           }
        }
    }
}
