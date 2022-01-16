using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KBLOC
    public partial class Kbloc  {
             //KBLOC

            ///<summary>Public empty contructor</summary>
            public Kbloc() {}
            ///<summary>Public empty contructor</summary>
            public Kbloc(Kbloc copyFrom) 
            {
                  this.Kaeid= copyFrom.Kaeid;
                  this.Kaebloc= copyFrom.Kaebloc;
                  this.Kaedesc= copyFrom.Kaedesc;
                  this.Kaecru= copyFrom.Kaecru;
                  this.Kaecrd= copyFrom.Kaecrd;
                  this.Kaecrh= copyFrom.Kaecrh;
                  this.Kaemaju= copyFrom.Kaemaju;
                  this.Kaemajd= copyFrom.Kaemajd;
                  this.Kaemajh= copyFrom.Kaemajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kaeid { get; set; } 
            
            ///<summary> Bloc </summary>
            public string Kaebloc { get; set; } 
            
            ///<summary> Description </summary>
            public string Kaedesc { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kaecru { get; set; } 
            
            ///<summary> Création Date  AAAAMMJJ </summary>
            public int Kaecrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kaecrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kaemaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kaemajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kaemajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kbloc  x=this,  y=obj as Kbloc;
            if( y == default(Kbloc) ) return false;
            return (
                    x.Kaeid==y.Kaeid
                    && x.Kaebloc==y.Kaebloc
                    && x.Kaedesc==y.Kaedesc
                    && x.Kaecru==y.Kaecru
                    && x.Kaecrd==y.Kaecrd
                    && x.Kaecrh==y.Kaecrh
                    && x.Kaemaju==y.Kaemaju
                    && x.Kaemajd==y.Kaemajd
                    && x.Kaemajh==y.Kaemajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Kaeid.GetHashCode() ) 
                      * 23 ) + (this.Kaebloc?? "").GetHashCode()
                      * 23 ) + (this.Kaedesc?? "").GetHashCode()
                      * 23 ) + (this.Kaecru?? "").GetHashCode()
                      * 23 ) + (this.Kaecrd.GetHashCode() ) 
                      * 23 ) + (this.Kaecrh.GetHashCode() ) 
                      * 23 ) + (this.Kaemaju?? "").GetHashCode()
                      * 23 ) + (this.Kaemajd.GetHashCode() ) 
                      * 23 ) + (this.Kaemajh.GetHashCode() )                    );
           }
        }
    }
}
