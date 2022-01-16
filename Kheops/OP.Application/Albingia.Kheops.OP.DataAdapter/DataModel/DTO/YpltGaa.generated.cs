using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKMOD.YPLTGAA
    public partial class YpltGaa  {
             //YPLTGAA

            ///<summary>Public empty contructor</summary>
            public YpltGaa() {}
            ///<summary>Public empty contructor</summary>
            public YpltGaa(YpltGaa copyFrom) 
            {
                  this.C5typ= copyFrom.C5typ;
                  this.C5seq= copyFrom.C5seq;
                  this.C5sem= copyFrom.C5sem;
        
            }        
            
            ///<summary> Type </summary>
            public string C5typ { get; set; } 
            
            ///<summary> Séquence garantie 1 </summary>
            public Int64 C5seq { get; set; } 
            
            ///<summary> Séquence garantie 2 </summary>
            public Int64 C5sem { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpltGaa  x=this,  y=obj as YpltGaa;
            if( y == default(YpltGaa) ) return false;
            return (
                    x.C5typ==y.C5typ
                    && x.C5seq==y.C5seq
                    && x.C5sem==y.C5sem  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((
                       17 + (this.C5typ?? "").GetHashCode()
                      * 23 ) + (this.C5seq.GetHashCode() ) 
                      * 23 ) + (this.C5sem.GetHashCode() )                    );
           }
        }
    }
}
