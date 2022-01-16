using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KEXPLCID
    public partial class KExpLciD  {
             //KEXPLCID

            ///<summary>Public empty contructor</summary>
            public KExpLciD() {}
            ///<summary>Public empty contructor</summary>
            public KExpLciD(KExpLciD copyFrom) 
            {
                  this.Khhid= copyFrom.Khhid;
                  this.Khhkhgid= copyFrom.Khhkhgid;
                  this.Khhordre= copyFrom.Khhordre;
                  this.Khhlcval= copyFrom.Khhlcval;
                  this.Khhlcvau= copyFrom.Khhlcvau;
                  this.Khhlcbase= copyFrom.Khhlcbase;
                  this.Khhloval= copyFrom.Khhloval;
                  this.Khhlovau= copyFrom.Khhlovau;
                  this.Khhlobase= copyFrom.Khhlobase;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Khhid { get; set; } 
            
            ///<summary> Lien KPEXPLCI </summary>
            public Int64 Khhkhgid { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public int Khhordre { get; set; } 
            
            ///<summary> Valeur </summary>
            public Decimal Khhlcval { get; set; } 
            
            ///<summary> Unité </summary>
            public string Khhlcvau { get; set; } 
            
            ///<summary> Base </summary>
            public string Khhlcbase { get; set; } 
            
            ///<summary> Concurrence Valeur </summary>
            public Decimal Khhloval { get; set; } 
            
            ///<summary> Concurrence Unité </summary>
            public string Khhlovau { get; set; } 
            
            ///<summary> Concurrence Base </summary>
            public string Khhlobase { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KExpLciD  x=this,  y=obj as KExpLciD;
            if( y == default(KExpLciD) ) return false;
            return (
                    x.Khhid==y.Khhid
                    && x.Khhkhgid==y.Khhkhgid
                    && x.Khhordre==y.Khhordre
                    && x.Khhlcval==y.Khhlcval
                    && x.Khhlcvau==y.Khhlcvau
                    && x.Khhlcbase==y.Khhlcbase
                    && x.Khhloval==y.Khhloval
                    && x.Khhlovau==y.Khhlovau
                    && x.Khhlobase==y.Khhlobase  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Khhid.GetHashCode() ) 
                      * 23 ) + (this.Khhkhgid.GetHashCode() ) 
                      * 23 ) + (this.Khhordre.GetHashCode() ) 
                      * 23 ) + (this.Khhlcval.GetHashCode() ) 
                      * 23 ) + (this.Khhlcvau?? "").GetHashCode()
                      * 23 ) + (this.Khhlcbase?? "").GetHashCode()
                      * 23 ) + (this.Khhloval.GetHashCode() ) 
                      * 23 ) + (this.Khhlovau?? "").GetHashCode()
                      * 23 ) + (this.Khhlobase?? "").GetHashCode()                   );
           }
        }
    }
}
