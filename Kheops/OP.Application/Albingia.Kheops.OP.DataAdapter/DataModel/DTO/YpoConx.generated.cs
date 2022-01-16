using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPCONX
    public partial class YpoConx  {
             //YHPCONX
             //YPOCONX

            ///<summary>Public empty contructor</summary>
            public YpoConx() {}
            ///<summary>Public empty contructor</summary>
            public YpoConx(YpoConx copyFrom) 
            {
                  this.Pjtyp= copyFrom.Pjtyp;
                  this.Pjccx= copyFrom.Pjccx;
                  this.Pjcnx= copyFrom.Pjcnx;
                  this.Pjipb= copyFrom.Pjipb;
                  this.Pjalx= copyFrom.Pjalx;
                  this.Pjavn= copyFrom.Pjavn;
                  this.Pjhin= copyFrom.Pjhin;
                  this.Pjbra= copyFrom.Pjbra;
                  this.Pjsbr= copyFrom.Pjsbr;
                  this.Pjcat= copyFrom.Pjcat;
                  this.Pjobs= copyFrom.Pjobs;
                  this.Pjide= copyFrom.Pjide;
        
            }        
            
            ///<summary> Type  O Offre  P Police </summary>
            public string Pjtyp { get; set; } 
            
            ///<summary> Cause de connexité </summary>
            public string Pjccx { get; set; } 
            
            ///<summary> N° de connexité </summary>
            public int Pjcnx { get; set; } 
            
            ///<summary> N° de Police  connexe </summary>
            public string Pjipb { get; set; } 
            
            ///<summary> N° Aliment connexe </summary>
            public int Pjalx { get; set; } 
            
            ///<summary> N° Avenant </summary>
            public int? Pjavn { get; set; } 
            
            ///<summary> N° Historique </summary>
            public int? Pjhin { get; set; } 
            
            ///<summary> Branche </summary>
            public string Pjbra { get; set; } 
            
            ///<summary> Sous-branche </summary>
            public string Pjsbr { get; set; } 
            
            ///<summary> Catégorie </summary>
            public string Pjcat { get; set; } 
            
            ///<summary> Lien KPOBSV </summary>
            public Int64 Pjobs { get; set; } 
            
            ///<summary> Lien KPENG  ENGID  si connexe </summary>
            public Int64 Pjide { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoConx  x=this,  y=obj as YpoConx;
            if( y == default(YpoConx) ) return false;
            return (
                    x.Pjtyp==y.Pjtyp
                    && x.Pjccx==y.Pjccx
                    && x.Pjcnx==y.Pjcnx
                    && x.Pjipb==y.Pjipb
                    && x.Pjalx==y.Pjalx
                    && x.Pjbra==y.Pjbra
                    && x.Pjsbr==y.Pjsbr
                    && x.Pjcat==y.Pjcat
                    && x.Pjobs==y.Pjobs
                    && x.Pjide==y.Pjide  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((
                       17 + (this.Pjtyp?? "").GetHashCode()
                      * 23 ) + (this.Pjccx?? "").GetHashCode()
                      * 23 ) + (this.Pjcnx.GetHashCode() ) 
                      * 23 ) + (this.Pjipb?? "").GetHashCode()
                      * 23 ) + (this.Pjalx.GetHashCode() ) 
                      * 23 ) + (this.Pjbra?? "").GetHashCode()
                      * 23 ) + (this.Pjsbr?? "").GetHashCode()
                      * 23 ) + (this.Pjcat?? "").GetHashCode()
                      * 23 ) + (this.Pjobs.GetHashCode() ) 
                      * 23 ) + (this.Pjide.GetHashCode() )                    );
           }
        }
    }
}
