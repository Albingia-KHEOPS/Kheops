using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KEXPFRHD
    public partial class KExpFrhD  {
             //KEXPFRHD

            ///<summary>Public empty contructor</summary>
            public KExpFrhD() {}
            ///<summary>Public empty contructor</summary>
            public KExpFrhD(KExpFrhD copyFrom) 
            {
                  this.Khfid= copyFrom.Khfid;
                  this.Khfkheid= copyFrom.Khfkheid;
                  this.Khfordre= copyFrom.Khfordre;
                  this.Khffhval= copyFrom.Khffhval;
                  this.Khffhvau= copyFrom.Khffhvau;
                  this.Khfbase= copyFrom.Khfbase;
                  this.Khfind= copyFrom.Khfind;
                  this.Khfivo= copyFrom.Khfivo;
                  this.Khffhmini= copyFrom.Khffhmini;
                  this.Khffhmaxi= copyFrom.Khffhmaxi;
                  this.Khflimdeb= copyFrom.Khflimdeb;
                  this.Khflimfin= copyFrom.Khflimfin;
        
            }        
            
            ///<summary> ID Unique </summary>
            public Int64 Khfid { get; set; } 
            
            ///<summary> Lien KEXPFRH </summary>
            public Int64 Khfkheid { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public int Khfordre { get; set; } 
            
            ///<summary> Valeur </summary>
            public Decimal Khffhval { get; set; } 
            
            ///<summary> Unité </summary>
            public string Khffhvau { get; set; } 
            
            ///<summary> Base </summary>
            public string Khfbase { get; set; } 
            
            ///<summary> Code Indice </summary>
            public string Khfind { get; set; } 
            
            ///<summary> Indice Valeur </summary>
            public Decimal Khfivo { get; set; } 
            
            ///<summary> Franchise minimum </summary>
            public Decimal Khffhmini { get; set; } 
            
            ///<summary> Franchise Maximum </summary>
            public Decimal Khffhmaxi { get; set; } 
            
            ///<summary> Limite début </summary>
            public int Khflimdeb { get; set; } 
            
            ///<summary> Limite Fin </summary>
            public int Khflimfin { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KExpFrhD  x=this,  y=obj as KExpFrhD;
            if( y == default(KExpFrhD) ) return false;
            return (
                    x.Khfid==y.Khfid
                    && x.Khfkheid==y.Khfkheid
                    && x.Khfordre==y.Khfordre
                    && x.Khffhval==y.Khffhval
                    && x.Khffhvau==y.Khffhvau
                    && x.Khfbase==y.Khfbase
                    && x.Khfind==y.Khfind
                    && x.Khfivo==y.Khfivo
                    && x.Khffhmini==y.Khffhmini
                    && x.Khffhmaxi==y.Khffhmaxi
                    && x.Khflimdeb==y.Khflimdeb
                    && x.Khflimfin==y.Khflimfin  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Khfid.GetHashCode() ) 
                      * 23 ) + (this.Khfkheid.GetHashCode() ) 
                      * 23 ) + (this.Khfordre.GetHashCode() ) 
                      * 23 ) + (this.Khffhval.GetHashCode() ) 
                      * 23 ) + (this.Khffhvau?? "").GetHashCode()
                      * 23 ) + (this.Khfbase?? "").GetHashCode()
                      * 23 ) + (this.Khfind?? "").GetHashCode()
                      * 23 ) + (this.Khfivo.GetHashCode() ) 
                      * 23 ) + (this.Khffhmini.GetHashCode() ) 
                      * 23 ) + (this.Khffhmaxi.GetHashCode() ) 
                      * 23 ) + (this.Khflimdeb.GetHashCode() ) 
                      * 23 ) + (this.Khflimfin.GetHashCode() )                    );
           }
        }
    }
}
