using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KCIBLEF
    public partial class Kcliblef  {
             //KCIBLEF

            ///<summary>Public empty contructor</summary>
            public Kcliblef() {}
            ///<summary>Public empty contructor</summary>
            public Kcliblef(Kcliblef copyFrom) 
            {
                  this.Kaiid= copyFrom.Kaiid;
                  this.Kaicible= copyFrom.Kaicible;
                  this.Kaikahid= copyFrom.Kaikahid;
                  this.Kaibra= copyFrom.Kaibra;
                  this.Kaisbr= copyFrom.Kaisbr;
                  this.Kaicat= copyFrom.Kaicat;
                  this.Kaicru= copyFrom.Kaicru;
                  this.Kaicrd= copyFrom.Kaicrd;
                  this.Kaicrh= copyFrom.Kaicrh;
                  this.Kaimaju= copyFrom.Kaimaju;
                  this.Kaimajd= copyFrom.Kaimajd;
                  this.Kaimajh= copyFrom.Kaimajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kaiid { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kaicible { get; set; } 
            
            ///<summary> ID KCIBLE </summary>
            public Int64 Kaikahid { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kaibra { get; set; } 
            
            ///<summary> Sous-Branche pour correspondance </summary>
            public string Kaisbr { get; set; } 
            
            ///<summary> Catégorie pour correspondance </summary>
            public string Kaicat { get; set; } 
            
            ///<summary> Création user </summary>
            public string Kaicru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kaicrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kaicrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kaimaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kaimajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kaimajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kcliblef  x=this,  y=obj as Kcliblef;
            if( y == default(Kcliblef) ) return false;
            return (
                    x.Kaiid==y.Kaiid
                    && x.Kaicible==y.Kaicible
                    && x.Kaikahid==y.Kaikahid
                    && x.Kaibra==y.Kaibra
                    && x.Kaisbr==y.Kaisbr
                    && x.Kaicat==y.Kaicat
                    && x.Kaicru==y.Kaicru
                    && x.Kaicrd==y.Kaicrd
                    && x.Kaicrh==y.Kaicrh
                    && x.Kaimaju==y.Kaimaju
                    && x.Kaimajd==y.Kaimajd
                    && x.Kaimajh==y.Kaimajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kaiid.GetHashCode() ) 
                      * 23 ) + (this.Kaicible?? "").GetHashCode()
                      * 23 ) + (this.Kaikahid.GetHashCode() ) 
                      * 23 ) + (this.Kaibra?? "").GetHashCode()
                      * 23 ) + (this.Kaisbr?? "").GetHashCode()
                      * 23 ) + (this.Kaicat?? "").GetHashCode()
                      * 23 ) + (this.Kaicru?? "").GetHashCode()
                      * 23 ) + (this.Kaicrd.GetHashCode() ) 
                      * 23 ) + (this.Kaicrh.GetHashCode() ) 
                      * 23 ) + (this.Kaimaju?? "").GetHashCode()
                      * 23 ) + (this.Kaimajd.GetHashCode() ) 
                      * 23 ) + (this.Kaimajh.GetHashCode() )                    );
           }
        }
    }
}
