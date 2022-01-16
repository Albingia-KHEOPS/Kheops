using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPEXPLCID
    public partial class KpExpLCID  {
             //HPEXPLCID
             //KPEXPLCID

            ///<summary>Public empty contructor</summary>
            public KpExpLCID() {}
            ///<summary>Public empty contructor</summary>
            public KpExpLCID(KpExpLCID copyFrom) 
            {
                  this.Kdjid= copyFrom.Kdjid;
                  this.Kdjavn= copyFrom.Kdjavn;
                  this.Kdjhin= copyFrom.Kdjhin;
                  this.Kdjkdiid= copyFrom.Kdjkdiid;
                  this.Kdjordre= copyFrom.Kdjordre;
                  this.Kdjlcval= copyFrom.Kdjlcval;
                  this.Kdjlcvau= copyFrom.Kdjlcvau;
                  this.Kdjlcbase= copyFrom.Kdjlcbase;
                  this.Kdjloval= copyFrom.Kdjloval;
                  this.Kdjlovau= copyFrom.Kdjlovau;
                  this.Kdjlobase= copyFrom.Kdjlobase;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdjid { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdjavn { get; set; } 
            
            ///<summary> N° histo </summary>
            public int? Kdjhin { get; set; } 
            
            ///<summary> Lien KPEXPLCI </summary>
            public Int64 Kdjkdiid { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public int Kdjordre { get; set; } 
            
            ///<summary> Valeur </summary>
            public Decimal Kdjlcval { get; set; } 
            
            ///<summary> Unité </summary>
            public string Kdjlcvau { get; set; } 
            
            ///<summary> Base </summary>
            public string Kdjlcbase { get; set; } 
            
            ///<summary> Concurrence Valeur </summary>
            public Decimal Kdjloval { get; set; } 
            
            ///<summary> Concurrence Unité </summary>
            public string Kdjlovau { get; set; } 
            
            ///<summary> Concurrence Base </summary>
            public string Kdjlobase { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpExpLCID  x=this,  y=obj as KpExpLCID;
            if( y == default(KpExpLCID) ) return false;
            return (
                    x.Kdjid==y.Kdjid
                    && x.Kdjkdiid==y.Kdjkdiid
                    && x.Kdjordre==y.Kdjordre
                    && x.Kdjlcval==y.Kdjlcval
                    && x.Kdjlcvau==y.Kdjlcvau
                    && x.Kdjlcbase==y.Kdjlcbase
                    && x.Kdjloval==y.Kdjloval
                    && x.Kdjlovau==y.Kdjlovau
                    && x.Kdjlobase==y.Kdjlobase  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Kdjid.GetHashCode() ) 
                      * 23 ) + (this.Kdjkdiid.GetHashCode() ) 
                      * 23 ) + (this.Kdjordre.GetHashCode() ) 
                      * 23 ) + (this.Kdjlcval.GetHashCode() ) 
                      * 23 ) + (this.Kdjlcvau?? "").GetHashCode()
                      * 23 ) + (this.Kdjlcbase?? "").GetHashCode()
                      * 23 ) + (this.Kdjloval.GetHashCode() ) 
                      * 23 ) + (this.Kdjlovau?? "").GetHashCode()
                      * 23 ) + (this.Kdjlobase?? "").GetHashCode()                   );
           }
        }
    }
}
