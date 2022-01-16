using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPOFRSQ
    public partial class KpOfRsq  {
             //KPOFRSQ

            ///<summary>Public empty contructor</summary>
            public KpOfRsq() {}
            ///<summary>Public empty contructor</summary>
            public KpOfRsq(KpOfRsq copyFrom) 
            {
                  this.Kfipog= copyFrom.Kfipog;
                  this.Kfialg= copyFrom.Kfialg;
                  this.Kfiipb= copyFrom.Kfiipb;
                  this.Kfialx= copyFrom.Kfialx;
                  this.Kfichr= copyFrom.Kfichr;
                  this.Kfitye= copyFrom.Kfitye;
                  this.Kfirsq= copyFrom.Kfirsq;
                  this.Kfiobj= copyFrom.Kfiobj;
                  this.Kfiinv= copyFrom.Kfiinv;
                  this.Kfisel= copyFrom.Kfisel;
        
            }        
            
            ///<summary> N° de Contrat généré </summary>
            public string Kfipog { get; set; } 
            
            ///<summary> N° Aliment généré </summary>
            public int Kfialg { get; set; } 
            
            ///<summary> Offre (Code) </summary>
            public string Kfiipb { get; set; } 
            
            ///<summary> Offre (Version) </summary>
            public int Kfialx { get; set; } 
            
            ///<summary> N° Chrono Affichage ID unique </summary>
            public int Kfichr { get; set; } 
            
            ///<summary> Type enregistrement  Risque Objet </summary>
            public string Kfitye { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Kfirsq { get; set; } 
            
            ///<summary> Identifiant objet </summary>
            public int Kfiobj { get; set; } 
            
            ///<summary> inventaire  O/N </summary>
            public string Kfiinv { get; set; } 
            
            ///<summary> Sélection O/N </summary>
            public string Kfisel { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOfRsq  x=this,  y=obj as KpOfRsq;
            if( y == default(KpOfRsq) ) return false;
            return (
                    x.Kfipog==y.Kfipog
                    && x.Kfialg==y.Kfialg
                    && x.Kfiipb==y.Kfiipb
                    && x.Kfialx==y.Kfialx
                    && x.Kfichr==y.Kfichr
                    && x.Kfitye==y.Kfitye
                    && x.Kfirsq==y.Kfirsq
                    && x.Kfiobj==y.Kfiobj
                    && x.Kfiinv==y.Kfiinv
                    && x.Kfisel==y.Kfisel  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((
                       17 + (this.Kfipog?? "").GetHashCode()
                      * 23 ) + (this.Kfialg.GetHashCode() ) 
                      * 23 ) + (this.Kfiipb?? "").GetHashCode()
                      * 23 ) + (this.Kfialx.GetHashCode() ) 
                      * 23 ) + (this.Kfichr.GetHashCode() ) 
                      * 23 ) + (this.Kfitye?? "").GetHashCode()
                      * 23 ) + (this.Kfirsq.GetHashCode() ) 
                      * 23 ) + (this.Kfiobj.GetHashCode() ) 
                      * 23 ) + (this.Kfiinv?? "").GetHashCode()
                      * 23 ) + (this.Kfisel?? "").GetHashCode()                   );
           }
        }
    }
}
