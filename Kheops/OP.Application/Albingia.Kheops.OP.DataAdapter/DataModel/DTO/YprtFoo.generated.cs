using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTFOO
    public partial class YprtFoo  {
             //YHRTFOO
             //YPRTFOO

            ///<summary>Public empty contructor</summary>
            public YprtFoo() {}
            ///<summary>Public empty contructor</summary>
            public YprtFoo(YprtFoo copyFrom) 
            {
                  this.Jpipb= copyFrom.Jpipb;
                  this.Jpalx= copyFrom.Jpalx;
                  this.Jpavn= copyFrom.Jpavn;
                  this.Jphin= copyFrom.Jphin;
                  this.Jprsq= copyFrom.Jprsq;
                  this.Jpfor= copyFrom.Jpfor;
                  this.Jpobj= copyFrom.Jpobj;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Jpipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Jpalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Jpavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Jphin { get; set; } 
            
            ///<summary> Identifiant Risque </summary>
            public int Jprsq { get; set; } 
            
            ///<summary> Identifiant formule </summary>
            public int Jpfor { get; set; } 
            
            ///<summary> Id objet </summary>
            public int Jpobj { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtFoo  x=this,  y=obj as YprtFoo;
            if( y == default(YprtFoo) ) return false;
            return (
                    x.Jpipb==y.Jpipb
                    && x.Jpalx==y.Jpalx
                    && x.Jprsq==y.Jprsq
                    && x.Jpfor==y.Jpfor
                    && x.Jpobj==y.Jpobj  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((
                       17 + (this.Jpipb?? "").GetHashCode()
                      * 23 ) + (this.Jpalx.GetHashCode() ) 
                      * 23 ) + (this.Jprsq.GetHashCode() ) 
                      * 23 ) + (this.Jpfor.GetHashCode() ) 
                      * 23 ) + (this.Jpobj.GetHashCode() )                    );
           }
        }
    }
}
