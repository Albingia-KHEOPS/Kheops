using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPCOPID
    public partial class KpCopid  {
             //KPCOPID

            ///<summary>Public empty contructor</summary>
            public KpCopid() {}
            ///<summary>Public empty contructor</summary>
            public KpCopid(KpCopid copyFrom) 
            {
                  this.Kfltyp= copyFrom.Kfltyp;
                  this.Kflipb= copyFrom.Kflipb;
                  this.Kflalx= copyFrom.Kflalx;
                  this.Kfltab= copyFrom.Kfltab;
                  this.Kflido= copyFrom.Kflido;
                  this.Kflidc= copyFrom.Kflidc;
        
            }        
            
            ///<summary> TYP O/P </summary>
            public string Kfltyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kflipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kflalx { get; set; } 
            
            ///<summary> Table </summary>
            public string Kfltab { get; set; } 
            
            ///<summary> ID à copier </summary>
            public Int64 Kflido { get; set; } 
            
            ///<summary> ID copie </summary>
            public Int64 Kflidc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCopid  x=this,  y=obj as KpCopid;
            if( y == default(KpCopid) ) return false;
            return (
                    x.Kfltyp==y.Kfltyp
                    && x.Kflipb==y.Kflipb
                    && x.Kflalx==y.Kflalx
                    && x.Kfltab==y.Kfltab
                    && x.Kflido==y.Kflido
                    && x.Kflidc==y.Kflidc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Kfltyp?? "").GetHashCode()
                      * 23 ) + (this.Kflipb?? "").GetHashCode()
                      * 23 ) + (this.Kflalx.GetHashCode() ) 
                      * 23 ) + (this.Kfltab?? "").GetHashCode()
                      * 23 ) + (this.Kflido.GetHashCode() ) 
                      * 23 ) + (this.Kflidc.GetHashCode() )                    );
           }
        }
    }
}
