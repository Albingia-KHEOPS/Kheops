using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPECHE
    public partial class YpoEche  {
             //YHPECHE
             //YPOECHE

            ///<summary>Public empty contructor</summary>
            public YpoEche() {}
            ///<summary>Public empty contructor</summary>
            public YpoEche(YpoEche copyFrom) 
            {
                  this.Piipb= copyFrom.Piipb;
                  this.Pialx= copyFrom.Pialx;
                  this.Piavn= copyFrom.Piavn;
                  this.Pihin= copyFrom.Pihin;
                  this.Pieha= copyFrom.Pieha;
                  this.Piehm= copyFrom.Piehm;
                  this.Piehj= copyFrom.Piehj;
                  this.Piehe= copyFrom.Piehe;
                  this.Pipcr= copyFrom.Pipcr;
                  this.Pipcc= copyFrom.Pipcc;
                  this.Pipmr= copyFrom.Pipmr;
                  this.Pipmc= copyFrom.Pipmc;
                  this.Piafr= copyFrom.Piafr;
                  this.Piipk= copyFrom.Piipk;
                  this.Piatt= copyFrom.Piatt;
                  this.Pityp= copyFrom.Pityp;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Piipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pialx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Piavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Pihin { get; set; } 
            
            ///<summary> Année d'échéance </summary>
            public int Pieha { get; set; } 
            
            ///<summary> Mois d'échéance </summary>
            public int Piehm { get; set; } 
            
            ///<summary> Jour d'échéance </summary>
            public int Piehj { get; set; } 
            
            ///<summary> A l'émission police 1 sinon 2 </summary>
            public string Piehe { get; set; } 
            
            ///<summary> % de répartition </summary>
            public int Pipcr { get; set; } 
            
            ///<summary> % de répartition calculé </summary>
            public Decimal Pipcc { get; set; } 
            
            ///<summary> Montant de répartition </summary>
            public Decimal Pipmr { get; set; } 
            
            ///<summary> Montant de répartition calculé </summary>
            public Decimal Pipmc { get; set; } 
            
            ///<summary> Montant de frais accessoires </summary>
            public int Piafr { get; set; } 
            
            ///<summary> N° de prime / Police </summary>
            public int Piipk { get; set; } 
            
            ///<summary> Application taxe Attentat </summary>
            public string Piatt { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Pityp { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoEche  x=this,  y=obj as YpoEche;
            if( y == default(YpoEche) ) return false;
            return (
                    x.Piipb==y.Piipb
                    && x.Pialx==y.Pialx
                    && x.Pieha==y.Pieha
                    && x.Piehm==y.Piehm
                    && x.Piehj==y.Piehj
                    && x.Piehe==y.Piehe
                    && x.Pipcr==y.Pipcr
                    && x.Pipcc==y.Pipcc
                    && x.Pipmr==y.Pipmr
                    && x.Pipmc==y.Pipmc
                    && x.Piafr==y.Piafr
                    && x.Piipk==y.Piipk
                    && x.Piatt==y.Piatt
                    && x.Pityp==y.Pityp  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((
                       17 + (this.Piipb?? "").GetHashCode()
                      * 23 ) + (this.Pialx.GetHashCode() ) 
                      * 23 ) + (this.Pieha.GetHashCode() ) 
                      * 23 ) + (this.Piehm.GetHashCode() ) 
                      * 23 ) + (this.Piehj.GetHashCode() ) 
                      * 23 ) + (this.Piehe?? "").GetHashCode()
                      * 23 ) + (this.Pipcr.GetHashCode() ) 
                      * 23 ) + (this.Pipcc.GetHashCode() ) 
                      * 23 ) + (this.Pipmr.GetHashCode() ) 
                      * 23 ) + (this.Pipmc.GetHashCode() ) 
                      * 23 ) + (this.Piafr.GetHashCode() ) 
                      * 23 ) + (this.Piipk.GetHashCode() ) 
                      * 23 ) + (this.Piatt?? "").GetHashCode()
                      * 23 ) + (this.Pityp?? "").GetHashCode()                   );
           }
        }
    }
}
