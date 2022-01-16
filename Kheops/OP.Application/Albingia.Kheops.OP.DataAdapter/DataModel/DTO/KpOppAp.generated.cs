using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPOPPAP
    public partial class KpOppAp  {
             //HPOPPAP
             //KPOPPAP

            ///<summary>Public empty contructor</summary>
            public KpOppAp() {}
            ///<summary>Public empty contructor</summary>
            public KpOppAp(KpOppAp copyFrom) 
            {
                  this.Kfqid= copyFrom.Kfqid;
                  this.Kfqtyp= copyFrom.Kfqtyp;
                  this.Kfqipb= copyFrom.Kfqipb;
                  this.Kfqalx= copyFrom.Kfqalx;
                  this.Kfqavn= copyFrom.Kfqavn;
                  this.Kfqhin= copyFrom.Kfqhin;
                  this.Kfqkfpid= copyFrom.Kfqkfpid;
                  this.Kfqperi= copyFrom.Kfqperi;
                  this.Kfqrsq= copyFrom.Kfqrsq;
                  this.Kfqobj= copyFrom.Kfqobj;
                  this.Kfqcru= copyFrom.Kfqcru;
                  this.Kfqcrd= copyFrom.Kfqcrd;
                  this.Kfqmaju= copyFrom.Kfqmaju;
                  this.Kfqmajd= copyFrom.Kfqmajd;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kfqid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kfqtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kfqipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kfqalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kfqavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kfqhin { get; set; } 
            
            ///<summary> Lien KPOPP </summary>
            public Int64 Kfqkfpid { get; set; } 
            
            ///<summary> Périmètre </summary>
            public string Kfqperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kfqrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kfqobj { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kfqcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kfqcrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kfqmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kfqmajd { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOppAp  x=this,  y=obj as KpOppAp;
            if( y == default(KpOppAp) ) return false;
            return (
                    x.Kfqid==y.Kfqid
                    && x.Kfqtyp==y.Kfqtyp
                    && x.Kfqipb==y.Kfqipb
                    && x.Kfqalx==y.Kfqalx
                    && x.Kfqkfpid==y.Kfqkfpid
                    && x.Kfqperi==y.Kfqperi
                    && x.Kfqrsq==y.Kfqrsq
                    && x.Kfqobj==y.Kfqobj
                    && x.Kfqcru==y.Kfqcru
                    && x.Kfqcrd==y.Kfqcrd
                    && x.Kfqmaju==y.Kfqmaju
                    && x.Kfqmajd==y.Kfqmajd  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kfqid.GetHashCode() ) 
                      * 23 ) + (this.Kfqtyp?? "").GetHashCode()
                      * 23 ) + (this.Kfqipb?? "").GetHashCode()
                      * 23 ) + (this.Kfqalx.GetHashCode() ) 
                      * 23 ) + (this.Kfqkfpid.GetHashCode() ) 
                      * 23 ) + (this.Kfqperi?? "").GetHashCode()
                      * 23 ) + (this.Kfqrsq.GetHashCode() ) 
                      * 23 ) + (this.Kfqobj.GetHashCode() ) 
                      * 23 ) + (this.Kfqcru?? "").GetHashCode()
                      * 23 ) + (this.Kfqcrd.GetHashCode() ) 
                      * 23 ) + (this.Kfqmaju?? "").GetHashCode()
                      * 23 ) + (this.Kfqmajd.GetHashCode() )                    );
           }
        }
    }
}
