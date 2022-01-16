using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KCATVOLET 
    public partial class Kcatvolet  {
             //KCATVOLET 

            ///<summary>Public empty contructor</summary>
            public Kcatvolet() {}
            ///<summary>Public empty contructor</summary>
            public Kcatvolet(Kcatvolet copyFrom) 
            {
                  this.Kapid= copyFrom.Kapid;
                  this.Kapbra= copyFrom.Kapbra;
                  this.Kapcible= copyFrom.Kapcible;
                  this.Kapkaiid= copyFrom.Kapkaiid;
                  this.Kapvolet= copyFrom.Kapvolet;
                  this.Kapkakid= copyFrom.Kapkakid;
                  this.Kapcar= copyFrom.Kapcar;
                  this.Kapordre= copyFrom.Kapordre;
                  this.Kapcru= copyFrom.Kapcru;
                  this.Kapcrd= copyFrom.Kapcrd;
                  this.Kapcrh= copyFrom.Kapcrh;
                  this.Kapmaju= copyFrom.Kapmaju;
                  this.Kapmajd= copyFrom.Kapmajd;
                  this.Kapmajh= copyFrom.Kapmajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kapid { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kapbra { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kapcible { get; set; } 
            
            ///<summary> ID KCIBLEF </summary>
            public Int64 Kapkaiid { get; set; } 
            
            ///<summary> Volet </summary>
            public string Kapvolet { get; set; } 
            
            ///<summary> ID KVOLET </summary>
            public Int64 Kapkakid { get; set; } 
            
            ///<summary> Caractère </summary>
            public string Kapcar { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public Decimal Kapordre { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kapcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kapcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kapcrh { get; set; } 
            
            ///<summary> Maj USer </summary>
            public string Kapmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kapmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kapmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kcatvolet  x=this,  y=obj as Kcatvolet;
            if( y == default(Kcatvolet) ) return false;
            return (
                    x.Kapid==y.Kapid
                    && x.Kapbra==y.Kapbra
                    && x.Kapcible==y.Kapcible
                    && x.Kapkaiid==y.Kapkaiid
                    && x.Kapvolet==y.Kapvolet
                    && x.Kapkakid==y.Kapkakid
                    && x.Kapcar==y.Kapcar
                    && x.Kapordre==y.Kapordre
                    && x.Kapcru==y.Kapcru
                    && x.Kapcrd==y.Kapcrd
                    && x.Kapcrh==y.Kapcrh
                    && x.Kapmaju==y.Kapmaju
                    && x.Kapmajd==y.Kapmajd
                    && x.Kapmajh==y.Kapmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((
                       17 + (this.Kapid.GetHashCode() ) 
                      * 23 ) + (this.Kapbra?? "").GetHashCode()
                      * 23 ) + (this.Kapcible?? "").GetHashCode()
                      * 23 ) + (this.Kapkaiid.GetHashCode() ) 
                      * 23 ) + (this.Kapvolet?? "").GetHashCode()
                      * 23 ) + (this.Kapkakid.GetHashCode() ) 
                      * 23 ) + (this.Kapcar?? "").GetHashCode()
                      * 23 ) + (this.Kapordre.GetHashCode() ) 
                      * 23 ) + (this.Kapcru?? "").GetHashCode()
                      * 23 ) + (this.Kapcrd.GetHashCode() ) 
                      * 23 ) + (this.Kapcrh.GetHashCode() ) 
                      * 23 ) + (this.Kapmaju?? "").GetHashCode()
                      * 23 ) + (this.Kapmajd.GetHashCode() ) 
                      * 23 ) + (this.Kapmajh.GetHashCode() )                    );
           }
        }
    }
}
