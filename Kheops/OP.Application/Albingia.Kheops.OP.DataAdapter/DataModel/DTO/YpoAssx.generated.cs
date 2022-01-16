using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPASSX
    public partial class YpoAssx  {
             //YHPASSX
             //YPOASSX

            ///<summary>Public empty contructor</summary>
            public YpoAssx() {}
            ///<summary>Public empty contructor</summary>
            public YpoAssx(YpoAssx copyFrom) 
            {
                  this.Pdipb= copyFrom.Pdipb;
                  this.Pdalx= copyFrom.Pdalx;
                  this.Pdavn= copyFrom.Pdavn;
                  this.Pdhin= copyFrom.Pdhin;
                  this.Pdql1= copyFrom.Pdql1;
                  this.Pdql2= copyFrom.Pdql2;
                  this.Pdql3= copyFrom.Pdql3;
                  this.Pdqld= copyFrom.Pdqld;
                  this.Pdtyp= copyFrom.Pdtyp;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Pdipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pdalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Pdavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Pdhin { get; set; } 
            
            ///<summary> Qualité 1 de l'assuré </summary>
            public string Pdql1 { get; set; } 
            
            ///<summary> Qualité 2 de l'assuré </summary>
            public string Pdql2 { get; set; } 
            
            ///<summary> Qualité 3 de l'assuré </summary>
            public string Pdql3 { get; set; } 
            
            ///<summary> Qualité Autre </summary>
            public string Pdqld { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Pdtyp { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoAssx  x=this,  y=obj as YpoAssx;
            if( y == default(YpoAssx) ) return false;
            return (
                    x.Pdipb==y.Pdipb
                    && x.Pdalx==y.Pdalx
                    && x.Pdql1==y.Pdql1
                    && x.Pdql2==y.Pdql2
                    && x.Pdql3==y.Pdql3
                    && x.Pdqld==y.Pdqld
                    && x.Pdtyp==y.Pdtyp  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((
                       17 + (this.Pdipb?? "").GetHashCode()
                      * 23 ) + (this.Pdalx.GetHashCode() ) 
                      * 23 ) + (this.Pdql1?? "").GetHashCode()
                      * 23 ) + (this.Pdql2?? "").GetHashCode()
                      * 23 ) + (this.Pdql3?? "").GetHashCode()
                      * 23 ) + (this.Pdqld?? "").GetHashCode()
                      * 23 ) + (this.Pdtyp?? "").GetHashCode()                   );
           }
        }
    }
}
