using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKMOD.YPLTGAL
    public partial class YpltGal  {
             //YPLTGAL

            ///<summary>Public empty contructor</summary>
            public YpltGal() {}
            ///<summary>Public empty contructor</summary>
            public YpltGal(YpltGal copyFrom) 
            {
                  this.C4seq= copyFrom.C4seq;
                  this.C4typ= copyFrom.C4typ;
                  this.C4bas= copyFrom.C4bas;
                  this.C4val= copyFrom.C4val;
                  this.C4unt= copyFrom.C4unt;
                  this.C4maj= copyFrom.C4maj;
                  this.C4obl= copyFrom.C4obl;
                  this.C4ala= copyFrom.C4ala;
        
            }        
            
            ///<summary> N° séquence garantie </summary>
            public Int64 C4seq { get; set; } 
            
            ///<summary> Type </summary>
            public string C4typ { get; set; } 
            
            ///<summary> Base </summary>
            public string C4bas { get; set; } 
            
            ///<summary> Valeur </summary>
            public Decimal C4val { get; set; } 
            
            ///<summary> Unité </summary>
            public string C4unt { get; set; } 
            
            ///<summary> Modifiable </summary>
            public string C4maj { get; set; } 
            
            ///<summary> Obligatoire </summary>
            public string C4obl { get; set; } 
            
            ///<summary>  </summary>
            public string C4ala { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpltGal  x=this,  y=obj as YpltGal;
            if( y == default(YpltGal) ) return false;
            return (
                    x.C4seq==y.C4seq
                    && x.C4typ==y.C4typ
                    && x.C4bas==y.C4bas
                    && x.C4val==y.C4val
                    && x.C4unt==y.C4unt
                    && x.C4maj==y.C4maj
                    && x.C4obl==y.C4obl
                    && x.C4ala==y.C4ala  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((
                       17 + (this.C4seq.GetHashCode() ) 
                      * 23 ) + (this.C4typ?? "").GetHashCode()
                      * 23 ) + (this.C4bas?? "").GetHashCode()
                      * 23 ) + (this.C4val.GetHashCode() ) 
                      * 23 ) + (this.C4unt?? "").GetHashCode()
                      * 23 ) + (this.C4maj?? "").GetHashCode()
                      * 23 ) + (this.C4obl?? "").GetHashCode()
                      * 23 ) + (this.C4ala?? "").GetHashCode()                   );
           }
        }
    }
}
