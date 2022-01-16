using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPOPTAP
    public partial class KpOptAp  {
             //HPOPTAP
             //KPOPTAP

            ///<summary>Public empty contructor</summary>
            public KpOptAp() {}
            ///<summary>Public empty contructor</summary>
            public KpOptAp(KpOptAp copyFrom) 
            {
                  this.Kddid= copyFrom.Kddid;
                  this.Kddtyp= copyFrom.Kddtyp;
                  this.Kddipb= copyFrom.Kddipb;
                  this.Kddalx= copyFrom.Kddalx;
                  this.Kddavn= copyFrom.Kddavn;
                  this.Kddhin= copyFrom.Kddhin;
                  this.Kddfor= copyFrom.Kddfor;
                  this.Kddopt= copyFrom.Kddopt;
                  this.Kddkdbid= copyFrom.Kddkdbid;
                  this.Kddperi= copyFrom.Kddperi;
                  this.Kddrsq= copyFrom.Kddrsq;
                  this.Kddobj= copyFrom.Kddobj;
                  this.Kddinven= copyFrom.Kddinven;
                  this.Kddinvep= copyFrom.Kddinvep;
                  this.Kddcru= copyFrom.Kddcru;
                  this.Kddcrd= copyFrom.Kddcrd;
                  this.Kddmaju= copyFrom.Kddmaju;
                  this.Kddmajd= copyFrom.Kddmajd;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kddid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kddtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kddipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kddalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kddavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kddhin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kddfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kddopt { get; set; } 
            
            ///<summary> Lien KPOPT </summary>
            public Int64 Kddkdbid { get; set; } 
            
            ///<summary> Périmètre </summary>
            public string Kddperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kddrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kddobj { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kddinven { get; set; } 
            
            ///<summary> Poste inventaire </summary>
            public int Kddinvep { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kddcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kddcrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kddmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kddmajd { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOptAp  x=this,  y=obj as KpOptAp;
            if( y == default(KpOptAp) ) return false;
            return (
                    x.Kddid==y.Kddid
                    && x.Kddtyp==y.Kddtyp
                    && x.Kddipb==y.Kddipb
                    && x.Kddalx==y.Kddalx
                    && x.Kddfor==y.Kddfor
                    && x.Kddopt==y.Kddopt
                    && x.Kddkdbid==y.Kddkdbid
                    && x.Kddperi==y.Kddperi
                    && x.Kddrsq==y.Kddrsq
                    && x.Kddobj==y.Kddobj
                    && x.Kddinven==y.Kddinven
                    && x.Kddinvep==y.Kddinvep
                    && x.Kddcru==y.Kddcru
                    && x.Kddcrd==y.Kddcrd
                    && x.Kddmaju==y.Kddmaju
                    && x.Kddmajd==y.Kddmajd  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((
                       17 + (this.Kddid.GetHashCode() ) 
                      * 23 ) + (this.Kddtyp?? "").GetHashCode()
                      * 23 ) + (this.Kddipb?? "").GetHashCode()
                      * 23 ) + (this.Kddalx.GetHashCode() ) 
                      * 23 ) + (this.Kddfor.GetHashCode() ) 
                      * 23 ) + (this.Kddopt.GetHashCode() ) 
                      * 23 ) + (this.Kddkdbid.GetHashCode() ) 
                      * 23 ) + (this.Kddperi?? "").GetHashCode()
                      * 23 ) + (this.Kddrsq.GetHashCode() ) 
                      * 23 ) + (this.Kddobj.GetHashCode() ) 
                      * 23 ) + (this.Kddinven.GetHashCode() ) 
                      * 23 ) + (this.Kddinvep.GetHashCode() ) 
                      * 23 ) + (this.Kddcru?? "").GetHashCode()
                      * 23 ) + (this.Kddcrd.GetHashCode() ) 
                      * 23 ) + (this.Kddmaju?? "").GetHashCode()
                      * 23 ) + (this.Kddmajd.GetHashCode() )                    );
           }
        }
    }
}
