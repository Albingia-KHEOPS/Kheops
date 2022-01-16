using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPEXPFRHD
    public partial class KpExpFrhD  {
             //HPEXPFRHD
             //KPEXPFRHD

            ///<summary>Public empty contructor</summary>
            public KpExpFrhD() {}
            ///<summary>Public empty contructor</summary>
            public KpExpFrhD(KpExpFrhD copyFrom) 
            {
                  this.Kdlid= copyFrom.Kdlid;
                  this.Kdlavn= copyFrom.Kdlavn;
                  this.Kdlhin= copyFrom.Kdlhin;
                  this.Kdlkdkid= copyFrom.Kdlkdkid;
                  this.Kdlordre= copyFrom.Kdlordre;
                  this.Kdlfhval= copyFrom.Kdlfhval;
                  this.Kdlfhvau= copyFrom.Kdlfhvau;
                  this.Kdlfhbase= copyFrom.Kdlfhbase;
                  this.Kdlind= copyFrom.Kdlind;
                  this.Kdlivo= copyFrom.Kdlivo;
                  this.Kdlfhmini= copyFrom.Kdlfhmini;
                  this.Kdlfhmaxi= copyFrom.Kdlfhmaxi;
                  this.Kdllimdeb= copyFrom.Kdllimdeb;
                  this.Kdllimfin= copyFrom.Kdllimfin;
        
            }        
            
            ///<summary> ID Unique </summary>
            public Int64 Kdlid { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdlavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdlhin { get; set; } 
            
            ///<summary> Lien KPEXPFRH </summary>
            public Int64 Kdlkdkid { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public int Kdlordre { get; set; } 
            
            ///<summary> Valeur </summary>
            public Decimal Kdlfhval { get; set; } 
            
            ///<summary> Unité </summary>
            public string Kdlfhvau { get; set; } 
            
            ///<summary> Base </summary>
            public string Kdlfhbase { get; set; } 
            
            ///<summary> Code Indice </summary>
            public string Kdlind { get; set; } 
            
            ///<summary> Indice Valeur </summary>
            public Decimal Kdlivo { get; set; } 
            
            ///<summary> Franchise minimum </summary>
            public Decimal Kdlfhmini { get; set; } 
            
            ///<summary> Franchise Maximum </summary>
            public Decimal Kdlfhmaxi { get; set; } 
            
            ///<summary> Limite début </summary>
            public Decimal Kdllimdeb { get; set; } 
            
            ///<summary> Limite Fin </summary>
            public Decimal Kdllimfin { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpExpFrhD  x=this,  y=obj as KpExpFrhD;
            if( y == default(KpExpFrhD) ) return false;
            return (
                    x.Kdlid==y.Kdlid
                    && x.Kdlkdkid==y.Kdlkdkid
                    && x.Kdlordre==y.Kdlordre
                    && x.Kdlfhval==y.Kdlfhval
                    && x.Kdlfhvau==y.Kdlfhvau
                    && x.Kdlfhbase==y.Kdlfhbase
                    && x.Kdlind==y.Kdlind
                    && x.Kdlivo==y.Kdlivo
                    && x.Kdlfhmini==y.Kdlfhmini
                    && x.Kdlfhmaxi==y.Kdlfhmaxi
                    && x.Kdllimdeb==y.Kdllimdeb
                    && x.Kdllimfin==y.Kdllimfin  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kdlid.GetHashCode() ) 
                      * 23 ) + (this.Kdlkdkid.GetHashCode() ) 
                      * 23 ) + (this.Kdlordre.GetHashCode() ) 
                      * 23 ) + (this.Kdlfhval.GetHashCode() ) 
                      * 23 ) + (this.Kdlfhvau?? "").GetHashCode()
                      * 23 ) + (this.Kdlfhbase?? "").GetHashCode()
                      * 23 ) + (this.Kdlind?? "").GetHashCode()
                      * 23 ) + (this.Kdlivo.GetHashCode() ) 
                      * 23 ) + (this.Kdlfhmini.GetHashCode() ) 
                      * 23 ) + (this.Kdlfhmaxi.GetHashCode() ) 
                      * 23 ) + (this.Kdllimdeb.GetHashCode() ) 
                      * 23 ) + (this.Kdllimfin.GetHashCode() )                    );
           }
        }
    }
}
