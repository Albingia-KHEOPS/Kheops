using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KNMREF
    public partial class KnmRef  {
             //KNMREF

            ///<summary>Public empty contructor</summary>
            public KnmRef() {}
            ///<summary>Public empty contructor</summary>
            public KnmRef(KnmRef copyFrom) 
            {
                  this.Khiid= copyFrom.Khiid;
                  this.Khitypo= copyFrom.Khitypo;
                  this.Khinmc= copyFrom.Khinmc;
                  this.Khidesi= copyFrom.Khidesi;
                  this.Khinord= copyFrom.Khinord;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Khiid { get; set; } 
            
            ///<summary> Typologie </summary>
            public string Khitypo { get; set; } 
            
            ///<summary> Code Nomenclature </summary>
            public string Khinmc { get; set; } 
            
            ///<summary> Désignation </summary>
            public string Khidesi { get; set; } 
            
            ///<summary> N° ordre </summary>
            public Decimal Khinord { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KnmRef  x=this,  y=obj as KnmRef;
            if( y == default(KnmRef) ) return false;
            return (
                    x.Khiid==y.Khiid
                    && x.Khitypo==y.Khitypo
                    && x.Khinmc==y.Khinmc
                    && x.Khidesi==y.Khidesi
                    && x.Khinord==y.Khinord  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((
                       17 + (this.Khiid.GetHashCode() ) 
                      * 23 ) + (this.Khitypo?? "").GetHashCode()
                      * 23 ) + (this.Khinmc?? "").GetHashCode()
                      * 23 ) + (this.Khidesi?? "").GetHashCode()
                      * 23 ) + (this.Khinord.GetHashCode() )                    );
           }
        }
    }
}
