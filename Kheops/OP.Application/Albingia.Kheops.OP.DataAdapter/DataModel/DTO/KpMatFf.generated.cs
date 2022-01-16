using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPMATFF
    public partial class KpMatFf  {
             //HPMATFF
             //KPMATFF

            ///<summary>Public empty contructor</summary>
            public KpMatFf() {}
            ///<summary>Public empty contructor</summary>
            public KpMatFf(KpMatFf copyFrom) 
            {
                  this.Keatyp= copyFrom.Keatyp;
                  this.Keaipb= copyFrom.Keaipb;
                  this.Keaalx= copyFrom.Keaalx;
                  this.Keaavn= copyFrom.Keaavn;
                  this.Keahin= copyFrom.Keahin;
                  this.Keachr= copyFrom.Keachr;
                  this.Keafor= copyFrom.Keafor;
                  this.Keaopt= copyFrom.Keaopt;
                  this.Keakdbid= copyFrom.Keakdbid;
                  this.Keakdaid= copyFrom.Keakdaid;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Keatyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Keaipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Keaalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Keaavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Keahin { get; set; } 
            
            ///<summary> N° Chrono Affichage ID unique </summary>
            public int Keachr { get; set; } 
            
            ///<summary> Formule </summary>
            public int Keafor { get; set; } 
            
            ///<summary> Option </summary>
            public int Keaopt { get; set; } 
            
            ///<summary> Lien KPOPT </summary>
            public Int64 Keakdbid { get; set; } 
            
            ///<summary> Lien KPFOR </summary>
            public Int64 Keakdaid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpMatFf  x=this,  y=obj as KpMatFf;
            if( y == default(KpMatFf) ) return false;
            return (
                    x.Keatyp==y.Keatyp
                    && x.Keaipb==y.Keaipb
                    && x.Keaalx==y.Keaalx
                    && x.Keachr==y.Keachr
                    && x.Keafor==y.Keafor
                    && x.Keaopt==y.Keaopt
                    && x.Keakdbid==y.Keakdbid
                    && x.Keakdaid==y.Keakdaid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((
                       17 + (this.Keatyp?? "").GetHashCode()
                      * 23 ) + (this.Keaipb?? "").GetHashCode()
                      * 23 ) + (this.Keaalx.GetHashCode() ) 
                      * 23 ) + (this.Keachr.GetHashCode() ) 
                      * 23 ) + (this.Keafor.GetHashCode() ) 
                      * 23 ) + (this.Keaopt.GetHashCode() ) 
                      * 23 ) + (this.Keakdbid.GetHashCode() ) 
                      * 23 ) + (this.Keakdaid.GetHashCode() )                    );
           }
        }
    }
}
