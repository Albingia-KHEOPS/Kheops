using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YYYYPAR
    public partial class Ypar  {
             //YYYYPAR

            ///<summary>Public empty contructor</summary>
            public Ypar() {}
            ///<summary>Public empty contructor</summary>
            public Ypar(Ypar copyFrom) 
            {
                  this.Tcon= copyFrom.Tcon;
                  this.Tfam= copyFrom.Tfam;
                  this.Tcod= copyFrom.Tcod;
                  this.Tplib= copyFrom.Tplib;
                  this.Tpcn1= copyFrom.Tpcn1;
                  this.Tpcn2= copyFrom.Tpcn2;
                  this.Tpca1= copyFrom.Tpca1;
                  this.Tpca2= copyFrom.Tpca2;
                  this.Tptyp= copyFrom.Tptyp;
                  this.Tplil= copyFrom.Tplil;
                  this.Tfilt= copyFrom.Tfilt;
        
            }        
            
            ///<summary> Concept </summary>
            public string Tcon { get; set; } 
            
            ///<summary> Famille </summary>
            public string Tfam { get; set; } 
            
            ///<summary> Code </summary>
            public string Tcod { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Tplib { get; set; } 
            
            ///<summary> Complément numérique 1 </summary>
            public Decimal Tpcn1 { get; set; } 
            
            ///<summary> Complément numérique 3 </summary>
            public Decimal Tpcn2 { get; set; } 
            
            ///<summary> Complément Alpha 1 </summary>
            public string Tpca1 { get; set; } 
            
            ///<summary> Complément Alpha 2 </summary>
            public string Tpca2 { get; set; } 
            
            ///<summary> Type paramètre pour restriction </summary>
            public string Tptyp { get; set; } 
            
            ///<summary> Libellé long </summary>
            public string Tplil { get; set; } 
            
            ///<summary> Filtre </summary>
            public string Tfilt { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Ypar  x=this,  y=obj as Ypar;
            if( y == default(Ypar) ) return false;
            return (
                    x.Tcon==y.Tcon
                    && x.Tfam==y.Tfam
                    && x.Tcod==y.Tcod
                    && x.Tplib==y.Tplib
                    && x.Tpcn1==y.Tpcn1
                    && x.Tpcn2==y.Tpcn2
                    && x.Tpca1==y.Tpca1
                    && x.Tpca2==y.Tpca2
                    && x.Tptyp==y.Tptyp
                    && x.Tplil==y.Tplil
                    && x.Tfilt==y.Tfilt  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((
                       17 + (this.Tcon?? "").GetHashCode()
                      * 23 ) + (this.Tfam?? "").GetHashCode()
                      * 23 ) + (this.Tcod?? "").GetHashCode()
                      * 23 ) + (this.Tplib?? "").GetHashCode()
                      * 23 ) + (this.Tpcn1.GetHashCode() ) 
                      * 23 ) + (this.Tpcn2.GetHashCode() ) 
                      * 23 ) + (this.Tpca1?? "").GetHashCode()
                      * 23 ) + (this.Tpca2?? "").GetHashCode()
                      * 23 ) + (this.Tptyp?? "").GetHashCode()
                      * 23 ) + (this.Tplil?? "").GetHashCode()
                      * 23 ) + (this.Tfilt?? "").GetHashCode()                   );
           }
        }
    }
}
