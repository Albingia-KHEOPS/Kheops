using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPOFTAR
    public partial class KpOfTar  {
             //KPOFTAR

            ///<summary>Public empty contructor</summary>
            public KpOfTar() {}
            ///<summary>Public empty contructor</summary>
            public KpOfTar(KpOfTar copyFrom) 
            {
                  this.Kfkpog= copyFrom.Kfkpog;
                  this.Kfkalg= copyFrom.Kfkalg;
                  this.Kfkipb= copyFrom.Kfkipb;
                  this.Kfkalx= copyFrom.Kfkalx;
                  this.Kfkfor= copyFrom.Kfkfor;
                  this.Kfkopt= copyFrom.Kfkopt;
                  this.Kfkgaran= copyFrom.Kfkgaran;
                  this.Kfknumtar= copyFrom.Kfknumtar;
                  this.Kfkkdgid= copyFrom.Kfkkdgid;
                  this.Kfksel= copyFrom.Kfksel;
        
            }        
            
            ///<summary> N° de contrat généré </summary>
            public string Kfkpog { get; set; } 
            
            ///<summary> N° Aliment généré </summary>
            public int Kfkalg { get; set; } 
            
            ///<summary> Offre (Code) </summary>
            public string Kfkipb { get; set; } 
            
            ///<summary> Offre (Version) </summary>
            public int Kfkalx { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kfkfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kfkopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kfkgaran { get; set; } 
            
            ///<summary> Numéro TARIF </summary>
            public int Kfknumtar { get; set; } 
            
            ///<summary> Lien KPGARTAR </summary>
            public Int64 Kfkkdgid { get; set; } 
            
            ///<summary> Sélection </summary>
            public string Kfksel { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOfTar  x=this,  y=obj as KpOfTar;
            if( y == default(KpOfTar) ) return false;
            return (
                    x.Kfkpog==y.Kfkpog
                    && x.Kfkalg==y.Kfkalg
                    && x.Kfkipb==y.Kfkipb
                    && x.Kfkalx==y.Kfkalx
                    && x.Kfkfor==y.Kfkfor
                    && x.Kfkopt==y.Kfkopt
                    && x.Kfkgaran==y.Kfkgaran
                    && x.Kfknumtar==y.Kfknumtar
                    && x.Kfkkdgid==y.Kfkkdgid
                    && x.Kfksel==y.Kfksel  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((
                       17 + (this.Kfkpog?? "").GetHashCode()
                      * 23 ) + (this.Kfkalg.GetHashCode() ) 
                      * 23 ) + (this.Kfkipb?? "").GetHashCode()
                      * 23 ) + (this.Kfkalx.GetHashCode() ) 
                      * 23 ) + (this.Kfkfor.GetHashCode() ) 
                      * 23 ) + (this.Kfkopt.GetHashCode() ) 
                      * 23 ) + (this.Kfkgaran?? "").GetHashCode()
                      * 23 ) + (this.Kfknumtar.GetHashCode() ) 
                      * 23 ) + (this.Kfkkdgid.GetHashCode() ) 
                      * 23 ) + (this.Kfksel?? "").GetHashCode()                   );
           }
        }
    }
}
