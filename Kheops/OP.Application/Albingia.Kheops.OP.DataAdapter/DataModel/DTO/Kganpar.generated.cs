using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KGANPAR
    public partial class Kganpar  {
             //KGANPAR

            ///<summary>Public empty contructor</summary>
            public Kganpar() {}
            ///<summary>Public empty contructor</summary>
            public Kganpar(Kganpar copyFrom) 
            {
                  this.Kaucar= copyFrom.Kaucar;
                  this.Kaunat= copyFrom.Kaunat;
                  this.Kauaffi= copyFrom.Kauaffi;
                  this.Kaumodi= copyFrom.Kaumodi;
                  this.Kauganc= copyFrom.Kauganc;
                  this.Kaugannc= copyFrom.Kaugannc;
        
            }        
            
            ///<summary> Caractère </summary>
            public string Kaucar { get; set; } 
            
            ///<summary> Nature </summary>
            public string Kaunat { get; set; } 
            
            ///<summary> Affichage initiale Coché ou non </summary>
            public string Kauaffi { get; set; } 
            
            ///<summary> Modifiable O/N </summary>
            public string Kaumodi { get; set; } 
            
            ///<summary> Nature si garantie cochée </summary>
            public string Kauganc { get; set; } 
            
            ///<summary> Nature si garantie non cochée </summary>
            public string Kaugannc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kganpar  x=this,  y=obj as Kganpar;
            if( y == default(Kganpar) ) return false;
            return (
                    x.Kaucar==y.Kaucar
                    && x.Kaunat==y.Kaunat
                    && x.Kauaffi==y.Kauaffi
                    && x.Kaumodi==y.Kaumodi
                    && x.Kauganc==y.Kauganc
                    && x.Kaugannc==y.Kaugannc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Kaucar?? "").GetHashCode()
                      * 23 ) + (this.Kaunat?? "").GetHashCode()
                      * 23 ) + (this.Kauaffi?? "").GetHashCode()
                      * 23 ) + (this.Kaumodi?? "").GetHashCode()
                      * 23 ) + (this.Kauganc?? "").GetHashCode()
                      * 23 ) + (this.Kaugannc?? "").GetHashCode()                   );
           }
        }
    }
}
