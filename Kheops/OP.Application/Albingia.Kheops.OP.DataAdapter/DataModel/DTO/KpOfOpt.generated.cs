using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPOFOPT
    public partial class KpOfOpt  {
             //KPOFOPT

            ///<summary>Public empty contructor</summary>
            public KpOfOpt() {}
            ///<summary>Public empty contructor</summary>
            public KpOfOpt(KpOfOpt copyFrom) 
            {
                  this.Kfjpog= copyFrom.Kfjpog;
                  this.Kfjalg= copyFrom.Kfjalg;
                  this.Kfjipb= copyFrom.Kfjipb;
                  this.Kfjalx= copyFrom.Kfjalx;
                  this.Kfjchr= copyFrom.Kfjchr;
                  this.Kfjteng= copyFrom.Kfjteng;
                  this.Kfjfor= copyFrom.Kfjfor;
                  this.Kfjopt= copyFrom.Kfjopt;
                  this.Kfjkdaid= copyFrom.Kfjkdaid;
                  this.Kfjkdbid= copyFrom.Kfjkdbid;
                  this.Kfjkakid= copyFrom.Kfjkakid;
                  this.Kfjsel= copyFrom.Kfjsel;
        
            }        
            
            ///<summary> N° de Contrat généré </summary>
            public string Kfjpog { get; set; } 
            
            ///<summary> N° Aliment généré </summary>
            public int Kfjalg { get; set; } 
            
            ///<summary> Offre (Code) </summary>
            public string Kfjipb { get; set; } 
            
            ///<summary> Offre (Version) </summary>
            public int Kfjalx { get; set; } 
            
            ///<summary> N° Chrono Affichage ID unique </summary>
            public int Kfjchr { get; set; } 
            
            ///<summary> Type enregistrement F/O/V </summary>
            public string Kfjteng { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kfjfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kfjopt { get; set; } 
            
            ///<summary> Lien KPFOR </summary>
            public Int64 Kfjkdaid { get; set; } 
            
            ///<summary> Lien KPOPT </summary>
            public Int64 Kfjkdbid { get; set; } 
            
            ///<summary> Lien KVOLET </summary>
            public Int64 Kfjkakid { get; set; } 
            
            ///<summary> Sélection  O/N </summary>
            public string Kfjsel { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOfOpt  x=this,  y=obj as KpOfOpt;
            if( y == default(KpOfOpt) ) return false;
            return (
                    x.Kfjpog==y.Kfjpog
                    && x.Kfjalg==y.Kfjalg
                    && x.Kfjipb==y.Kfjipb
                    && x.Kfjalx==y.Kfjalx
                    && x.Kfjchr==y.Kfjchr
                    && x.Kfjteng==y.Kfjteng
                    && x.Kfjfor==y.Kfjfor
                    && x.Kfjopt==y.Kfjopt
                    && x.Kfjkdaid==y.Kfjkdaid
                    && x.Kfjkdbid==y.Kfjkdbid
                    && x.Kfjkakid==y.Kfjkakid
                    && x.Kfjsel==y.Kfjsel  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kfjpog?? "").GetHashCode()
                      * 23 ) + (this.Kfjalg.GetHashCode() ) 
                      * 23 ) + (this.Kfjipb?? "").GetHashCode()
                      * 23 ) + (this.Kfjalx.GetHashCode() ) 
                      * 23 ) + (this.Kfjchr.GetHashCode() ) 
                      * 23 ) + (this.Kfjteng?? "").GetHashCode()
                      * 23 ) + (this.Kfjfor.GetHashCode() ) 
                      * 23 ) + (this.Kfjopt.GetHashCode() ) 
                      * 23 ) + (this.Kfjkdaid.GetHashCode() ) 
                      * 23 ) + (this.Kfjkdbid.GetHashCode() ) 
                      * 23 ) + (this.Kfjkakid.GetHashCode() ) 
                      * 23 ) + (this.Kfjsel?? "").GetHashCode()                   );
           }
        }
    }
}
