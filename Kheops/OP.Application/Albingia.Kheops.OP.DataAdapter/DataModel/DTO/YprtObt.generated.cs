using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTOBT
    public partial class YprtObt  {
             //YHRTOBT
             //YPRTOBT

            ///<summary>Public empty contructor</summary>
            public YprtObt() {}
            ///<summary>Public empty contructor</summary>
            public YprtObt(YprtObt copyFrom) 
            {
                  this.Kfipb= copyFrom.Kfipb;
                  this.Kfalx= copyFrom.Kfalx;
                  this.Kfavn= copyFrom.Kfavn;
                  this.Kfhin= copyFrom.Kfhin;
                  this.Kfrsq= copyFrom.Kfrsq;
                  this.Kfobj= copyFrom.Kfobj;
                  this.Kfduv= copyFrom.Kfduv;
                  this.Kfduu= copyFrom.Kfduu;
                  this.Kfdda= copyFrom.Kfdda;
                  this.Kfddm= copyFrom.Kfddm;
                  this.Kfddj= copyFrom.Kfddj;
                  this.Kfdfa= copyFrom.Kfdfa;
                  this.Kfdfm= copyFrom.Kfdfm;
                  this.Kfdfj= copyFrom.Kfdfj;
                  this.Kfesv= copyFrom.Kfesv;
                  this.Kfesu= copyFrom.Kfesu;
                  this.Kfeda= copyFrom.Kfeda;
                  this.Kfedm= copyFrom.Kfedm;
                  this.Kfedj= copyFrom.Kfedj;
                  this.Kfefa= copyFrom.Kfefa;
                  this.Kfefm= copyFrom.Kfefm;
                  this.Kfefj= copyFrom.Kfefj;
                  this.Kftdf= copyFrom.Kftdf;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Kfipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Kfalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kfavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Kfhin { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Kfrsq { get; set; } 
            
            ///<summary> Identifiant objet </summary>
            public int Kfobj { get; set; } 
            
            ///<summary> Durée des travaux : Valeur </summary>
            public int Kfduv { get; set; } 
            
            ///<summary> Durée des travaux: Unité </summary>
            public string Kfduu { get; set; } 
            
            ///<summary> Durée travaux : Année début </summary>
            public int Kfdda { get; set; } 
            
            ///<summary> Durée travaux : Mois début </summary>
            public int Kfddm { get; set; } 
            
            ///<summary> Durée travaux : Jour début </summary>
            public int Kfddj { get; set; } 
            
            ///<summary> Durée travaux : Année fin </summary>
            public int Kfdfa { get; set; } 
            
            ///<summary> Durée travaux : Mois fin </summary>
            public int Kfdfm { get; set; } 
            
            ///<summary> Durée travaux : Jour fin </summary>
            public int Kfdfj { get; set; } 
            
            ///<summary> Durée des Essais : Valeur </summary>
            public int Kfesv { get; set; } 
            
            ///<summary> Durée des Essais : Unité </summary>
            public string Kfesu { get; set; } 
            
            ///<summary> Essais : Année début </summary>
            public int Kfeda { get; set; } 
            
            ///<summary> Essais : Mois début </summary>
            public int Kfedm { get; set; } 
            
            ///<summary> Essais : Jour début </summary>
            public int Kfedj { get; set; } 
            
            ///<summary> Essais : Année Fin </summary>
            public int Kfefa { get; set; } 
            
            ///<summary> Essais : Mois Fin </summary>
            public int Kfefm { get; set; } 
            
            ///<summary> Essais : Jour Fin </summary>
            public int Kfefj { get; set; } 
            
            ///<summary> Date travaux définitive (O/N) </summary>
            public string Kftdf { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtObt  x=this,  y=obj as YprtObt;
            if( y == default(YprtObt) ) return false;
            return (
                    x.Kfipb==y.Kfipb
                    && x.Kfalx==y.Kfalx
                    && x.Kfrsq==y.Kfrsq
                    && x.Kfobj==y.Kfobj
                    && x.Kfduv==y.Kfduv
                    && x.Kfduu==y.Kfduu
                    && x.Kfdda==y.Kfdda
                    && x.Kfddm==y.Kfddm
                    && x.Kfddj==y.Kfddj
                    && x.Kfdfa==y.Kfdfa
                    && x.Kfdfm==y.Kfdfm
                    && x.Kfdfj==y.Kfdfj
                    && x.Kfesv==y.Kfesv
                    && x.Kfesu==y.Kfesu
                    && x.Kfeda==y.Kfeda
                    && x.Kfedm==y.Kfedm
                    && x.Kfedj==y.Kfedj
                    && x.Kfefa==y.Kfefa
                    && x.Kfefm==y.Kfefm
                    && x.Kfefj==y.Kfefj
                    && x.Kftdf==y.Kftdf  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((
                       17 + (this.Kfipb?? "").GetHashCode()
                      * 23 ) + (this.Kfalx.GetHashCode() ) 
                      * 23 ) + (this.Kfrsq.GetHashCode() ) 
                      * 23 ) + (this.Kfobj.GetHashCode() ) 
                      * 23 ) + (this.Kfduv.GetHashCode() ) 
                      * 23 ) + (this.Kfduu?? "").GetHashCode()
                      * 23 ) + (this.Kfdda.GetHashCode() ) 
                      * 23 ) + (this.Kfddm.GetHashCode() ) 
                      * 23 ) + (this.Kfddj.GetHashCode() ) 
                      * 23 ) + (this.Kfdfa.GetHashCode() ) 
                      * 23 ) + (this.Kfdfm.GetHashCode() ) 
                      * 23 ) + (this.Kfdfj.GetHashCode() ) 
                      * 23 ) + (this.Kfesv.GetHashCode() ) 
                      * 23 ) + (this.Kfesu?? "").GetHashCode()
                      * 23 ) + (this.Kfeda.GetHashCode() ) 
                      * 23 ) + (this.Kfedm.GetHashCode() ) 
                      * 23 ) + (this.Kfedj.GetHashCode() ) 
                      * 23 ) + (this.Kfefa.GetHashCode() ) 
                      * 23 ) + (this.Kfefm.GetHashCode() ) 
                      * 23 ) + (this.Kfefj.GetHashCode() ) 
                      * 23 ) + (this.Kftdf?? "").GetHashCode()                   );
           }
        }
    }
}
