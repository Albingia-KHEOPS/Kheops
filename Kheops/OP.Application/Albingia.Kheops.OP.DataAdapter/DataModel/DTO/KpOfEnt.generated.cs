using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPOFENT
    public partial class KpOfEnt  {
             //KPOFENT

            ///<summary>Public empty contructor</summary>
            public KpOfEnt() {}
            ///<summary>Public empty contructor</summary>
            public KpOfEnt(KpOfEnt copyFrom) 
            {
                  this.Kfhpog= copyFrom.Kfhpog;
                  this.Kfhalg= copyFrom.Kfhalg;
                  this.Kfhipb= copyFrom.Kfhipb;
                  this.Kfhalx= copyFrom.Kfhalx;
                  this.Kfhnpo= copyFrom.Kfhnpo;
                  this.Kfhefd= copyFrom.Kfhefd;
                  this.Kfhsad= copyFrom.Kfhsad;
                  this.Kfhbra= copyFrom.Kfhbra;
                  this.Kfhcible= copyFrom.Kfhcible;
                  this.Kfhipr= copyFrom.Kfhipr;
                  this.Kfhalr= copyFrom.Kfhalr;
                  this.Kfhtypo= copyFrom.Kfhtypo;
                  this.Kfhipm= copyFrom.Kfhipm;
                  this.Khfsit= copyFrom.Khfsit;
                  this.Kfhstu= copyFrom.Kfhstu;
                  this.Kfhstd= copyFrom.Kfhstd;
        
            }        
            
            ///<summary> N° de Contrat généré </summary>
            public string Kfhpog { get; set; } 
            
            ///<summary> N° Aliment généré </summary>
            public int Kfhalg { get; set; } 
            
            ///<summary> Offre (Code) </summary>
            public string Kfhipb { get; set; } 
            
            ///<summary> Offre (Version) </summary>
            public int Kfhalx { get; set; } 
            
            ///<summary> N° chrono police </summary>
            public int Kfhnpo { get; set; } 
            
            ///<summary> Date effet </summary>
            public int Kfhefd { get; set; } 
            
            ///<summary> Date Accord </summary>
            public int Kfhsad { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kfhbra { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kfhcible { get; set; } 
            
            ///<summary> Police remplacée (Code) </summary>
            public string Kfhipr { get; set; } 
            
            ///<summary> Police remplacée (Aliment) </summary>
            public int Kfhalr { get; set; } 
            
            ///<summary> Typo Contrat  Aliment Mère Contrat </summary>
            public string Kfhtypo { get; set; } 
            
            ///<summary> Police mère </summary>
            public string Kfhipm { get; set; } 
            
            ///<summary> Situation Code (A ,V) </summary>
            public string Khfsit { get; set; } 
            
            ///<summary> Situation User </summary>
            public string Kfhstu { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kfhstd { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOfEnt  x=this,  y=obj as KpOfEnt;
            if( y == default(KpOfEnt) ) return false;
            return (
                    x.Kfhpog==y.Kfhpog
                    && x.Kfhalg==y.Kfhalg
                    && x.Kfhipb==y.Kfhipb
                    && x.Kfhalx==y.Kfhalx
                    && x.Kfhnpo==y.Kfhnpo
                    && x.Kfhefd==y.Kfhefd
                    && x.Kfhsad==y.Kfhsad
                    && x.Kfhbra==y.Kfhbra
                    && x.Kfhcible==y.Kfhcible
                    && x.Kfhipr==y.Kfhipr
                    && x.Kfhalr==y.Kfhalr
                    && x.Kfhtypo==y.Kfhtypo
                    && x.Kfhipm==y.Kfhipm
                    && x.Khfsit==y.Khfsit
                    && x.Kfhstu==y.Kfhstu
                    && x.Kfhstd==y.Kfhstd  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((
                       17 + (this.Kfhpog?? "").GetHashCode()
                      * 23 ) + (this.Kfhalg.GetHashCode() ) 
                      * 23 ) + (this.Kfhipb?? "").GetHashCode()
                      * 23 ) + (this.Kfhalx.GetHashCode() ) 
                      * 23 ) + (this.Kfhnpo.GetHashCode() ) 
                      * 23 ) + (this.Kfhefd.GetHashCode() ) 
                      * 23 ) + (this.Kfhsad.GetHashCode() ) 
                      * 23 ) + (this.Kfhbra?? "").GetHashCode()
                      * 23 ) + (this.Kfhcible?? "").GetHashCode()
                      * 23 ) + (this.Kfhipr?? "").GetHashCode()
                      * 23 ) + (this.Kfhalr.GetHashCode() ) 
                      * 23 ) + (this.Kfhtypo?? "").GetHashCode()
                      * 23 ) + (this.Kfhipm?? "").GetHashCode()
                      * 23 ) + (this.Khfsit?? "").GetHashCode()
                      * 23 ) + (this.Kfhstu?? "").GetHashCode()
                      * 23 ) + (this.Kfhstd.GetHashCode() )                    );
           }
        }
    }
}
