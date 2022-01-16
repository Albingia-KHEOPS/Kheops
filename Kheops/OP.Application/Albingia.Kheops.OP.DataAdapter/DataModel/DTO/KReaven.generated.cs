using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KREAVEN
    public partial class KReaven  {
             //KREAVEN

            ///<summary>Public empty contructor</summary>
            public KReaven() {}
            ///<summary>Public empty contructor</summary>
            public KReaven(KReaven copyFrom) 
            {
                  this.Kgafam= copyFrom.Kgafam;
                  this.Kgaven= copyFrom.Kgaven;
                  this.Kgalibv= copyFrom.Kgalibv;
                  this.Kgasepa= copyFrom.Kgasepa;
        
            }        
            
            ///<summary> Famille de réassurance </summary>
            public string Kgafam { get; set; } 
            
            ///<summary> N° colonne ventilation </summary>
            public int Kgaven { get; set; } 
            
            ///<summary> Libellé ventilation </summary>
            public string Kgalibv { get; set; } 
            
            ///<summary> Séparation O/N </summary>
            public string Kgasepa { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KReaven  x=this,  y=obj as KReaven;
            if( y == default(KReaven) ) return false;
            return (
                    x.Kgafam==y.Kgafam
                    && x.Kgaven==y.Kgaven
                    && x.Kgalibv==y.Kgalibv
                    && x.Kgasepa==y.Kgasepa  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((
                       17 + (this.Kgafam?? "").GetHashCode()
                      * 23 ) + (this.Kgaven.GetHashCode() ) 
                      * 23 ) + (this.Kgalibv?? "").GetHashCode()
                      * 23 ) + (this.Kgasepa?? "").GetHashCode()                   );
           }
        }
    }
}
