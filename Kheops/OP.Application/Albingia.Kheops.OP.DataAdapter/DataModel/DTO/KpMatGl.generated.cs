using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPMATGL
    public partial class KpMatGl  {
             //HPMATGL
             //KPMATGL

            ///<summary>Public empty contructor</summary>
            public KpMatGl() {}
            ///<summary>Public empty contructor</summary>
            public KpMatGl(KpMatGl copyFrom) 
            {
                  this.Keftyp= copyFrom.Keftyp;
                  this.Kefipb= copyFrom.Kefipb;
                  this.Kefalx= copyFrom.Kefalx;
                  this.Kefavn= copyFrom.Kefavn;
                  this.Kefhin= copyFrom.Kefhin;
                  this.Kefkedchr= copyFrom.Kefkedchr;
                  this.Kefkeechr= copyFrom.Kefkeechr;
                  this.Kefico= copyFrom.Kefico;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Keftyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kefipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kefalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kefavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kefhin { get; set; } 
            
            ///<summary> Lien KPMATGR </summary>
            public int Kefkedchr { get; set; } 
            
            ///<summary> Lien KPMATGG </summary>
            public int Kefkeechr { get; set; } 
            
            ///<summary> Icône C Complet S spécifique </summary>
            public string Kefico { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpMatGl  x=this,  y=obj as KpMatGl;
            if( y == default(KpMatGl) ) return false;
            return (
                    x.Keftyp==y.Keftyp
                    && x.Kefipb==y.Kefipb
                    && x.Kefalx==y.Kefalx
                    && x.Kefkedchr==y.Kefkedchr
                    && x.Kefkeechr==y.Kefkeechr
                    && x.Kefico==y.Kefico  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Keftyp?? "").GetHashCode()
                      * 23 ) + (this.Kefipb?? "").GetHashCode()
                      * 23 ) + (this.Kefalx.GetHashCode() ) 
                      * 23 ) + (this.Kefkedchr.GetHashCode() ) 
                      * 23 ) + (this.Kefkeechr.GetHashCode() ) 
                      * 23 ) + (this.Kefico?? "").GetHashCode()                   );
           }
        }
    }
}
