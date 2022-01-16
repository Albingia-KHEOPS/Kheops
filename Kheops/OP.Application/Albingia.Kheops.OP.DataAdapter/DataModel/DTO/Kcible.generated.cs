using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KCIBLE
    public partial class Kcible  {
             //KCIBLE

            ///<summary>Public empty contructor</summary>
            public Kcible() {}
            ///<summary>Public empty contructor</summary>
            public Kcible(Kcible copyFrom) 
            {
                  this.Kahid= copyFrom.Kahid;
                  this.Kahcible= copyFrom.Kahcible;
                  this.Kahdesc= copyFrom.Kahdesc;
                  this.Kahcru= copyFrom.Kahcru;
                  this.Kahcrd= copyFrom.Kahcrd;
                  this.Kahcrh= copyFrom.Kahcrh;
                  this.Kahmaju= copyFrom.Kahmaju;
                  this.Kahmajd= copyFrom.Kahmajd;
                  this.Kahmajh= copyFrom.Kahmajh;
                  this.Kahnmg= copyFrom.Kahnmg;
                  this.Kahcon= copyFrom.Kahcon;
                  this.Kahfam= copyFrom.Kahfam;
                  this.Kahaut= copyFrom.Kahaut;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kahid { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kahcible { get; set; } 
            
            ///<summary> Description </summary>
            public string Kahdesc { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kahcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kahcrd { get; set; } 
            
            ///<summary> Création heure </summary>
            public int Kahcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kahmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kahmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kahmajh { get; set; } 
            
            ///<summary> Nomenclature : grille </summary>
            public string Kahnmg { get; set; } 
            
            ///<summary> Activité : Concept </summary>
            public string Kahcon { get; set; } 
            
            ///<summary> Activité : Famille </summary>
            public string Kahfam { get; set; } 
            
            ///<summary> Cible autorisée </summary>
            public string Kahaut { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kcible  x=this,  y=obj as Kcible;
            if( y == default(Kcible) ) return false;
            return (
                    x.Kahid==y.Kahid
                    && x.Kahcible==y.Kahcible
                    && x.Kahdesc==y.Kahdesc
                    && x.Kahcru==y.Kahcru
                    && x.Kahcrd==y.Kahcrd
                    && x.Kahcrh==y.Kahcrh
                    && x.Kahmaju==y.Kahmaju
                    && x.Kahmajd==y.Kahmajd
                    && x.Kahmajh==y.Kahmajh
                    && x.Kahnmg==y.Kahnmg
                    && x.Kahcon==y.Kahcon
                    && x.Kahfam==y.Kahfam
                    && x.Kahaut==y.Kahaut  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((
                       17 + (this.Kahid.GetHashCode() ) 
                      * 23 ) + (this.Kahcible?? "").GetHashCode()
                      * 23 ) + (this.Kahdesc?? "").GetHashCode()
                      * 23 ) + (this.Kahcru?? "").GetHashCode()
                      * 23 ) + (this.Kahcrd.GetHashCode() ) 
                      * 23 ) + (this.Kahcrh.GetHashCode() ) 
                      * 23 ) + (this.Kahmaju?? "").GetHashCode()
                      * 23 ) + (this.Kahmajd.GetHashCode() ) 
                      * 23 ) + (this.Kahmajh.GetHashCode() ) 
                      * 23 ) + (this.Kahnmg?? "").GetHashCode()
                      * 23 ) + (this.Kahcon?? "").GetHashCode()
                      * 23 ) + (this.Kahfam?? "").GetHashCode()
                      * 23 ) + (this.Kahaut?? "").GetHashCode()                   );
           }
        }
    }
}
