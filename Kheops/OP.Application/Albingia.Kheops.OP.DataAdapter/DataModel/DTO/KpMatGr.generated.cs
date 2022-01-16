using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPMATGR
    public partial class KpMatGr  {
             //HPMATGR
             //KPMATGR

            ///<summary>Public empty contructor</summary>
            public KpMatGr() {}
            ///<summary>Public empty contructor</summary>
            public KpMatGr(KpMatGr copyFrom) 
            {
                  this.Kedtyp= copyFrom.Kedtyp;
                  this.Kedipb= copyFrom.Kedipb;
                  this.Kedalx= copyFrom.Kedalx;
                  this.Kedavn= copyFrom.Kedavn;
                  this.Kedhin= copyFrom.Kedhin;
                  this.Kedchr= copyFrom.Kedchr;
                  this.Kedrsq= copyFrom.Kedrsq;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kedtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kedipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kedalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kedavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kedhin { get; set; } 
            
            ///<summary> N° Chrono Affichage ID unique </summary>
            public int Kedchr { get; set; } 
            
            ///<summary> N° Risque </summary>
            public int Kedrsq { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpMatGr  x=this,  y=obj as KpMatGr;
            if( y == default(KpMatGr) ) return false;
            return (
                    x.Kedtyp==y.Kedtyp
                    && x.Kedipb==y.Kedipb
                    && x.Kedalx==y.Kedalx
                    && x.Kedchr==y.Kedchr
                    && x.Kedrsq==y.Kedrsq  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((
                       17 + (this.Kedtyp?? "").GetHashCode()
                      * 23 ) + (this.Kedipb?? "").GetHashCode()
                      * 23 ) + (this.Kedalx.GetHashCode() ) 
                      * 23 ) + (this.Kedchr.GetHashCode() ) 
                      * 23 ) + (this.Kedrsq.GetHashCode() )                    );
           }
        }
    }
}
