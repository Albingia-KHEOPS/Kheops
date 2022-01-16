using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KUSRDRT
    public partial class KUsrDrt  {
             //KUSRDRT

            ///<summary>Public empty contructor</summary>
            public KUsrDrt() {}
            ///<summary>Public empty contructor</summary>
            public KUsrDrt(KUsrDrt copyFrom) 
            {
                  this.Khrusr= copyFrom.Khrusr;
                  this.Khrbra= copyFrom.Khrbra;
                  this.Khrtyd= copyFrom.Khrtyd;
        
            }        
            
            ///<summary> User </summary>
            public string Khrusr { get; set; } 
            
            ///<summary> Branche </summary>
            public string Khrbra { get; set; } 
            
            ///<summary> Type de droit </summary>
            public string Khrtyd { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KUsrDrt  x=this,  y=obj as KUsrDrt;
            if( y == default(KUsrDrt) ) return false;
            return (
                    x.Khrusr==y.Khrusr
                    && x.Khrbra==y.Khrbra
                    && x.Khrtyd==y.Khrtyd  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((
                       17 + (this.Khrusr?? "").GetHashCode()
                      * 23 ) + (this.Khrbra?? "").GetHashCode()
                      * 23 ) + (this.Khrtyd?? "").GetHashCode()                   );
           }
        }
    }
}
