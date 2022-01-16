using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KVOLET
    public partial class KVolet  {
             //KVOLET

            ///<summary>Public empty contructor</summary>
            public KVolet() {}
            ///<summary>Public empty contructor</summary>
            public KVolet(KVolet copyFrom) 
            {
                  this.Kakid= copyFrom.Kakid;
                  this.Kakvolet= copyFrom.Kakvolet;
                  this.Kakdesc= copyFrom.Kakdesc;
                  this.Kakcru= copyFrom.Kakcru;
                  this.Kakcrd= copyFrom.Kakcrd;
                  this.Kakcrh= copyFrom.Kakcrh;
                  this.Kakmaju= copyFrom.Kakmaju;
                  this.Kakmajd= copyFrom.Kakmajd;
                  this.Kakmajh= copyFrom.Kakmajh;
                  this.Kakbra= copyFrom.Kakbra;
                  this.Kakfgen= copyFrom.Kakfgen;
                  this.Kakpres= copyFrom.Kakpres;
        
            }        
            
            ///<summary> ID unique Volet </summary>
            public Int64 Kakid { get; set; } 
            
            ///<summary> Volet </summary>
            public string Kakvolet { get; set; } 
            
            ///<summary> Description </summary>
            public string Kakdesc { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kakcru { get; set; } 
            
            ///<summary> Date création </summary>
            public int Kakcrd { get; set; } 
            
            ///<summary> Création heure </summary>
            public int Kakcrh { get; set; } 
            
            ///<summary> Maj user </summary>
            public string Kakmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kakmajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kakmajh { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kakbra { get; set; } 
            
            ///<summary> O/N si O  => dans Formule générale </summary>
            public string Kakfgen { get; set; } 
            
            ///<summary> Présentation  par défaut </summary>
            public string Kakpres { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KVolet  x=this,  y=obj as KVolet;
            if( y == default(KVolet) ) return false;
            return (
                    x.Kakid==y.Kakid
                    && x.Kakvolet==y.Kakvolet
                    && x.Kakdesc==y.Kakdesc
                    && x.Kakcru==y.Kakcru
                    && x.Kakcrd==y.Kakcrd
                    && x.Kakcrh==y.Kakcrh
                    && x.Kakmaju==y.Kakmaju
                    && x.Kakmajd==y.Kakmajd
                    && x.Kakmajh==y.Kakmajh
                    && x.Kakbra==y.Kakbra
                    && x.Kakfgen==y.Kakfgen
                    && x.Kakpres==y.Kakpres  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kakid.GetHashCode() ) 
                      * 23 ) + (this.Kakvolet?? "").GetHashCode()
                      * 23 ) + (this.Kakdesc?? "").GetHashCode()
                      * 23 ) + (this.Kakcru?? "").GetHashCode()
                      * 23 ) + (this.Kakcrd.GetHashCode() ) 
                      * 23 ) + (this.Kakcrh.GetHashCode() ) 
                      * 23 ) + (this.Kakmaju?? "").GetHashCode()
                      * 23 ) + (this.Kakmajd.GetHashCode() ) 
                      * 23 ) + (this.Kakmajh.GetHashCode() ) 
                      * 23 ) + (this.Kakbra?? "").GetHashCode()
                      * 23 ) + (this.Kakfgen?? "").GetHashCode()
                      * 23 ) + (this.Kakpres?? "").GetHashCode()                   );
           }
        }
    }
}
