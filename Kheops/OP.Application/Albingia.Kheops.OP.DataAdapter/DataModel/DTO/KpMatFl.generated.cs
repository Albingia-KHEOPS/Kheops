using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPMATFL
    public partial class KpMatFl  {
             //HPMATFL
             //KPMATFL

            ///<summary>Public empty contructor</summary>
            public KpMatFl() {}
            ///<summary>Public empty contructor</summary>
            public KpMatFl(KpMatFl copyFrom) 
            {
                  this.Kectyp= copyFrom.Kectyp;
                  this.Kecipb= copyFrom.Kecipb;
                  this.Kecalx= copyFrom.Kecalx;
                  this.Kecavn= copyFrom.Kecavn;
                  this.Kechin= copyFrom.Kechin;
                  this.Keckeachr= copyFrom.Keckeachr;
                  this.Keckebchr= copyFrom.Keckebchr;
                  this.Kecico= copyFrom.Kecico;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kectyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kecipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kecalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kecavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kechin { get; set; } 
            
            ///<summary> Lien KPMATFF </summary>
            public int Keckeachr { get; set; } 
            
            ///<summary> Lien KPMATFR </summary>
            public int Keckebchr { get; set; } 
            
            ///<summary> Icône C Complet S spécifique </summary>
            public string Kecico { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpMatFl  x=this,  y=obj as KpMatFl;
            if( y == default(KpMatFl) ) return false;
            return (
                    x.Kectyp==y.Kectyp
                    && x.Kecipb==y.Kecipb
                    && x.Kecalx==y.Kecalx
                    && x.Keckeachr==y.Keckeachr
                    && x.Keckebchr==y.Keckebchr
                    && x.Kecico==y.Kecico  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Kectyp?? "").GetHashCode()
                      * 23 ) + (this.Kecipb?? "").GetHashCode()
                      * 23 ) + (this.Kecalx.GetHashCode() ) 
                      * 23 ) + (this.Keckeachr.GetHashCode() ) 
                      * 23 ) + (this.Keckebchr.GetHashCode() ) 
                      * 23 ) + (this.Kecico?? "").GetHashCode()                   );
           }
        }
    }
}
