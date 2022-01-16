using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKMOD.YPLMGA
    public partial class YPlmga  {
             //YPLMGA

            ///<summary>Public empty contructor</summary>
            public YPlmga() {}
            ///<summary>Public empty contructor</summary>
            public YPlmga(YPlmga copyFrom) 
            {
                  this.D1mga= copyFrom.D1mga;
                  this.D1lib= copyFrom.D1lib;
        
            }        
            
            ///<summary> Code modèle garantie </summary>
            public string D1mga { get; set; } 
            
            ///<summary> Libellé </summary>
            public string D1lib { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YPlmga  x=this,  y=obj as YPlmga;
            if( y == default(YPlmga) ) return false;
            return (
                    x.D1mga==y.D1mga
                    && x.D1lib==y.D1lib  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((
                       17 + (this.D1mga?? "").GetHashCode()
                      * 23 ) + (this.D1lib?? "").GetHashCode()                   );
           }
        }
    }
}
