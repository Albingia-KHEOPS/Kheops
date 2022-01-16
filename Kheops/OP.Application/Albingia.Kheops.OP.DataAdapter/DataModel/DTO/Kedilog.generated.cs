using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KEDILOG
    public partial class Kedilog  {
             //KEDILOG

            ///<summary>Public empty contructor</summary>
            public Kedilog() {}
            ///<summary>Public empty contructor</summary>
            public Kedilog(Kedilog copyFrom) 
            {
                  this.Idsession= copyFrom.Idsession;
                  this.Typ= copyFrom.Typ;
                  this.Ipb= copyFrom.Ipb;
                  this.Alx= copyFrom.Alx;
                  this.Statut= copyFrom.Statut;
                  this.Methode= copyFrom.Methode;
                  this.Dateheure= copyFrom.Dateheure;
                  this.Info= copyFrom.Info;
                  this.Seq= copyFrom.Seq;
        
            }        
            
            ///<summary>  </summary>
            public string Idsession { get; set; } 
            
            ///<summary>  </summary>
            public string Typ { get; set; } 
            
            ///<summary>  </summary>
            public string Ipb { get; set; } 
            
            ///<summary>  </summary>
            public int Alx { get; set; } 
            
            ///<summary>  </summary>
            public string Statut { get; set; } 
            
            ///<summary>  </summary>
            public string Methode { get; set; } 
            
            ///<summary>  </summary>
            public string Dateheure { get; set; } 
            
            ///<summary>  </summary>
            public string Info { get; set; } 
            
            ///<summary>  </summary>
            public string Seq { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kedilog  x=this,  y=obj as Kedilog;
            if( y == default(Kedilog) ) return false;
            return (
                    ((x.Idsession??"")==(y.Idsession??""))
                    && x.Typ==y.Typ
                    && x.Ipb==y.Ipb
                    && x.Alx==y.Alx
                    && x.Statut==y.Statut
                    && x.Methode==y.Methode
                    && x.Dateheure==y.Dateheure
                    && x.Info==y.Info
                    && x.Seq==y.Seq  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Idsession?? "").GetHashCode()
                      * 23 ) + (this.Typ?? "").GetHashCode()
                      * 23 ) + (this.Ipb?? "").GetHashCode()
                      * 23 ) + (this.Alx.GetHashCode() ) 
                      * 23 ) + (this.Statut?? "").GetHashCode()
                      * 23 ) + (this.Methode?? "").GetHashCode()
                      * 23 ) + (this.Dateheure?? "").GetHashCode()
                      * 23 ) + (this.Info?? "").GetHashCode()
                      * 23 ) + (this.Seq?? "").GetHashCode()                   );
           }
        }
    }
}
