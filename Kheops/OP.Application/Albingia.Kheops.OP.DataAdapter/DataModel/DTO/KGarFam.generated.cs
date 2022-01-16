using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KGARFAM
    public partial class KGarFam  {
             //KGARFAM

            ///<summary>Public empty contructor</summary>
            public KGarFam() {}
            ///<summary>Public empty contructor</summary>
            public KGarFam(KGarFam copyFrom) 
            {
                  this.Gvgar= copyFrom.Gvgar;
                  this.Gvbra= copyFrom.Gvbra;
                  this.Gvsbr= copyFrom.Gvsbr;
                  this.Gvcat= copyFrom.Gvcat;
                  this.Gvfam= copyFrom.Gvfam;
        
            }        
            
            ///<summary> Code garantie </summary>
            public string Gvgar { get; set; } 
            
            ///<summary> Branche </summary>
            public string Gvbra { get; set; } 
            
            ///<summary> Sous-branche </summary>
            public string Gvsbr { get; set; } 
            
            ///<summary> Catégorie </summary>
            public string Gvcat { get; set; } 
            
            ///<summary> Famille Garantie </summary>
            public string Gvfam { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KGarFam  x=this,  y=obj as KGarFam;
            if( y == default(KGarFam) ) return false;
            return (
                    x.Gvgar==y.Gvgar
                    && x.Gvbra==y.Gvbra
                    && x.Gvsbr==y.Gvsbr
                    && x.Gvcat==y.Gvcat
                    && x.Gvfam==y.Gvfam  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((
                       17 + (this.Gvgar?? "").GetHashCode()
                      * 23 ) + (this.Gvbra?? "").GetHashCode()
                      * 23 ) + (this.Gvsbr?? "").GetHashCode()
                      * 23 ) + (this.Gvcat?? "").GetHashCode()
                      * 23 ) + (this.Gvfam?? "").GetHashCode()                   );
           }
        }
    }
}
