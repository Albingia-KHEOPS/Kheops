using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPMATFR
    public partial class KpMatFr  {
             //HPMATFR
             //KPMATFR

            ///<summary>Public empty contructor</summary>
            public KpMatFr() {}
            ///<summary>Public empty contructor</summary>
            public KpMatFr(KpMatFr copyFrom) 
            {
                  this.Kebtyp= copyFrom.Kebtyp;
                  this.Kebipb= copyFrom.Kebipb;
                  this.Kebalx= copyFrom.Kebalx;
                  this.Kebavn= copyFrom.Kebavn;
                  this.Kebhin= copyFrom.Kebhin;
                  this.Kebchr= copyFrom.Kebchr;
                  this.Kebtye= copyFrom.Kebtye;
                  this.Kebrsq= copyFrom.Kebrsq;
                  this.Kebobj= copyFrom.Kebobj;
                  this.Kebinv= copyFrom.Kebinv;
                  this.Kebvid= copyFrom.Kebvid;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kebtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kebipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kebalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kebavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kebhin { get; set; } 
            
            ///<summary> N° Chrono Affichage ID unique </summary>
            public int Kebchr { get; set; } 
            
            ///<summary> Type enregistrement  Risque Objet </summary>
            public string Kebtye { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Kebrsq { get; set; } 
            
            ///<summary> Identifiant objet </summary>
            public int Kebobj { get; set; } 
            
            ///<summary> inventaire  O/N </summary>
            public string Kebinv { get; set; } 
            
            ///<summary> Non affecté O/N </summary>
            public string Kebvid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpMatFr  x=this,  y=obj as KpMatFr;
            if( y == default(KpMatFr) ) return false;
            return (
                    x.Kebtyp==y.Kebtyp
                    && x.Kebipb==y.Kebipb
                    && x.Kebalx==y.Kebalx
                    && x.Kebchr==y.Kebchr
                    && x.Kebtye==y.Kebtye
                    && x.Kebrsq==y.Kebrsq
                    && x.Kebobj==y.Kebobj
                    && x.Kebinv==y.Kebinv
                    && x.Kebvid==y.Kebvid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Kebtyp?? "").GetHashCode()
                      * 23 ) + (this.Kebipb?? "").GetHashCode()
                      * 23 ) + (this.Kebalx.GetHashCode() ) 
                      * 23 ) + (this.Kebchr.GetHashCode() ) 
                      * 23 ) + (this.Kebtye?? "").GetHashCode()
                      * 23 ) + (this.Kebrsq.GetHashCode() ) 
                      * 23 ) + (this.Kebobj.GetHashCode() ) 
                      * 23 ) + (this.Kebinv?? "").GetHashCode()
                      * 23 ) + (this.Kebvid?? "").GetHashCode()                   );
           }
        }
    }
}
