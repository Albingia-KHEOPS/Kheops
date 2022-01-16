using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KINVTYP
    public partial class KInvTyp  {
             //KINVTYP

            ///<summary>Public empty contructor</summary>
            public KInvTyp() {}
            ///<summary>Public empty contructor</summary>
            public KInvTyp(KInvTyp copyFrom) 
            {
                  this.Kagid= copyFrom.Kagid;
                  this.Kagtyinv= copyFrom.Kagtyinv;
                  this.Kagdesc= copyFrom.Kagdesc;
                  this.Kagtmap= copyFrom.Kagtmap;
                  this.Kagtable= copyFrom.Kagtable;
                  this.Kagkggid= copyFrom.Kagkggid;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kagid { get; set; } 
            
            ///<summary> Code inventaire </summary>
            public string Kagtyinv { get; set; } 
            
            ///<summary> Description </summary>
            public string Kagdesc { get; set; } 
            
            ///<summary> Typologie Grille de saisie </summary>
            public int Kagtmap { get; set; } 
            
            ///<summary> Table de détail </summary>
            public string Kagtable { get; set; } 
            
            ///<summary> Lien KFILTRE </summary>
            public Int64 Kagkggid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KInvTyp  x=this,  y=obj as KInvTyp;
            if( y == default(KInvTyp) ) return false;
            return (
                    x.Kagid==y.Kagid
                    && x.Kagtyinv==y.Kagtyinv
                    && x.Kagdesc==y.Kagdesc
                    && x.Kagtmap==y.Kagtmap
                    && x.Kagtable==y.Kagtable
                    && x.Kagkggid==y.Kagkggid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Kagid.GetHashCode() ) 
                      * 23 ) + (this.Kagtyinv?? "").GetHashCode()
                      * 23 ) + (this.Kagdesc?? "").GetHashCode()
                      * 23 ) + (this.Kagtmap.GetHashCode() ) 
                      * 23 ) + (this.Kagtable?? "").GetHashCode()
                      * 23 ) + (this.Kagkggid.GetHashCode() )                    );
           }
        }
    }
}
