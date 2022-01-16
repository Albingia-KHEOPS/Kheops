using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KFILTRL
    public partial class KFiltrL  {
             //KFILTRL

            ///<summary>Public empty contructor</summary>
            public KFiltrL() {}
            ///<summary>Public empty contructor</summary>
            public KFiltrL(KFiltrL copyFrom) 
            {
                  this.Kghid= copyFrom.Kghid;
                  this.Kghkggid= copyFrom.Kghkggid;
                  this.Kghfilt= copyFrom.Kghfilt;
                  this.Kghordr= copyFrom.Kghordr;
                  this.Kghactf= copyFrom.Kghactf;
                  this.Kghbra= copyFrom.Kghbra;
                  this.Kghcible= copyFrom.Kghcible;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kghid { get; set; } 
            
            ///<summary> Lien KFILTRE </summary>
            public Int64 Kghkggid { get; set; } 
            
            ///<summary> Filtre </summary>
            public string Kghfilt { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public int Kghordr { get; set; } 
            
            ///<summary> I Inclure E Exclure </summary>
            public string Kghactf { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kghbra { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kghcible { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KFiltrL  x=this,  y=obj as KFiltrL;
            if( y == default(KFiltrL) ) return false;
            return (
                    x.Kghid==y.Kghid
                    && x.Kghkggid==y.Kghkggid
                    && x.Kghfilt==y.Kghfilt
                    && x.Kghordr==y.Kghordr
                    && x.Kghactf==y.Kghactf
                    && x.Kghbra==y.Kghbra
                    && x.Kghcible==y.Kghcible  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((
                       17 + (this.Kghid.GetHashCode() ) 
                      * 23 ) + (this.Kghkggid.GetHashCode() ) 
                      * 23 ) + (this.Kghfilt?? "").GetHashCode()
                      * 23 ) + (this.Kghordr.GetHashCode() ) 
                      * 23 ) + (this.Kghactf?? "").GetHashCode()
                      * 23 ) + (this.Kghbra?? "").GetHashCode()
                      * 23 ) + (this.Kghcible?? "").GetHashCode()                   );
           }
        }
    }
}
