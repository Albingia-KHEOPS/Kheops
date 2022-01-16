using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YPRIPGA
    public partial class YPriPGa  {
             //YPRIPGA

            ///<summary>Public empty contructor</summary>
            public YPriPGa() {}
            ///<summary>Public empty contructor</summary>
            public YPriPGa(YPriPGa copyFrom) 
            {
                  this.Plipb= copyFrom.Plipb;
                  this.Plalx= copyFrom.Plalx;
                  this.Pltye= copyFrom.Pltye;
                  this.Plgar= copyFrom.Plgar;
                  this.Plmht= copyFrom.Plmht;
                  this.Plmtx= copyFrom.Plmtx;
                  this.Pltax= copyFrom.Pltax;
                  this.Pltxv= copyFrom.Pltxv;
                  this.Pltxu= copyFrom.Pltxu;
                  this.Plmtt= copyFrom.Plmtt;
                  this.Plxf1= copyFrom.Plxf1;
                  this.Plmx1= copyFrom.Plmx1;
                  this.Plxf2= copyFrom.Plxf2;
                  this.Plmx2= copyFrom.Plmx2;
                  this.Plxf3= copyFrom.Plxf3;
                  this.Plmx3= copyFrom.Plmx3;
                  this.Plsup= copyFrom.Plsup;
                  this.Plgap= copyFrom.Plgap;
                  this.Plkht= copyFrom.Plkht;
                  this.Plkhx= copyFrom.Plkhx;
                  this.Plktt= copyFrom.Plktt;
                  this.Plkx1= copyFrom.Plkx1;
                  this.Plkx2= copyFrom.Plkx2;
                  this.Plkx3= copyFrom.Plkx3;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Plipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Plalx { get; set; } 
            
            ///<summary> Typ enreg: 1 Part Albing 2 Prime tot </summary>
            public string Pltye { get; set; } 
            
            ///<summary> Code garantie </summary>
            public string Plgar { get; set; } 
            
            ///<summary> Montant HT (Hors CATNAT) </summary>
            public Decimal Plmht { get; set; } 
            
            ///<summary> Montant de taxe (Hors CATNAT) </summary>
            public Decimal Plmtx { get; set; } 
            
            ///<summary> Code taxe </summary>
            public string Pltax { get; set; } 
            
            ///<summary> Valeur code taxe </summary>
            public Decimal Pltxv { get; set; } 
            
            ///<summary> Unité code taxe </summary>
            public string Pltxu { get; set; } 
            
            ///<summary> Montant TTC  (Hors CATNAT) </summary>
            public Decimal Plmtt { get; set; } 
            
            ///<summary> Famille de taxe 1 </summary>
            public string Plxf1 { get; set; } 
            
            ///<summary> Montant de taxe 1 </summary>
            public Decimal Plmx1 { get; set; } 
            
            ///<summary> Famille de taxe 2 </summary>
            public string Plxf2 { get; set; } 
            
            ///<summary> Montant de taxe 2 </summary>
            public Decimal Plmx2 { get; set; } 
            
            ///<summary> Famille de taxe 3 </summary>
            public string Plxf3 { get; set; } 
            
            ///<summary> Montant de taxe 3 </summary>
            public Decimal Plmx3 { get; set; } 
            
            ///<summary> Garantie supprimée O/N </summary>
            public string Plsup { get; set; } 
            
            ///<summary> N° ordre présentation </summary>
            public int Plgap { get; set; } 
            
            ///<summary> Montant HT (Hors CATNAT)     Dev Cpt </summary>
            public Decimal Plkht { get; set; } 
            
            ///<summary> Montant de taxe(Hors CATNAT) Dev Cpt </summary>
            public Decimal Plkhx { get; set; } 
            
            ///<summary> Montant TTC (hors CATNAT)    Dev Cpt </summary>
            public Decimal Plktt { get; set; } 
            
            ///<summary> Montant de taxe 1            Dev Cpt </summary>
            public Decimal Plkx1 { get; set; } 
            
            ///<summary> Montant de taxe 2            Dev Cpt </summary>
            public Decimal Plkx2 { get; set; } 
            
            ///<summary> Montant de taxe 3            Dev Cpt </summary>
            public Decimal Plkx3 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YPriPGa  x=this,  y=obj as YPriPGa;
            if( y == default(YPriPGa) ) return false;
            return (
                    x.Plipb==y.Plipb
                    && x.Plalx==y.Plalx
                    && x.Pltye==y.Pltye
                    && x.Plgar==y.Plgar
                    && x.Plmht==y.Plmht
                    && x.Plmtx==y.Plmtx
                    && x.Pltax==y.Pltax
                    && x.Pltxv==y.Pltxv
                    && x.Pltxu==y.Pltxu
                    && x.Plmtt==y.Plmtt
                    && x.Plxf1==y.Plxf1
                    && x.Plmx1==y.Plmx1
                    && x.Plxf2==y.Plxf2
                    && x.Plmx2==y.Plmx2
                    && x.Plxf3==y.Plxf3
                    && x.Plmx3==y.Plmx3
                    && x.Plsup==y.Plsup
                    && x.Plgap==y.Plgap
                    && x.Plkht==y.Plkht
                    && x.Plkhx==y.Plkhx
                    && x.Plktt==y.Plktt
                    && x.Plkx1==y.Plkx1
                    && x.Plkx2==y.Plkx2
                    && x.Plkx3==y.Plkx3  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((
                       17 + (this.Plipb?? "").GetHashCode()
                      * 23 ) + (this.Plalx.GetHashCode() ) 
                      * 23 ) + (this.Pltye?? "").GetHashCode()
                      * 23 ) + (this.Plgar?? "").GetHashCode()
                      * 23 ) + (this.Plmht.GetHashCode() ) 
                      * 23 ) + (this.Plmtx.GetHashCode() ) 
                      * 23 ) + (this.Pltax?? "").GetHashCode()
                      * 23 ) + (this.Pltxv.GetHashCode() ) 
                      * 23 ) + (this.Pltxu?? "").GetHashCode()
                      * 23 ) + (this.Plmtt.GetHashCode() ) 
                      * 23 ) + (this.Plxf1?? "").GetHashCode()
                      * 23 ) + (this.Plmx1.GetHashCode() ) 
                      * 23 ) + (this.Plxf2?? "").GetHashCode()
                      * 23 ) + (this.Plmx2.GetHashCode() ) 
                      * 23 ) + (this.Plxf3?? "").GetHashCode()
                      * 23 ) + (this.Plmx3.GetHashCode() ) 
                      * 23 ) + (this.Plsup?? "").GetHashCode()
                      * 23 ) + (this.Plgap.GetHashCode() ) 
                      * 23 ) + (this.Plkht.GetHashCode() ) 
                      * 23 ) + (this.Plkhx.GetHashCode() ) 
                      * 23 ) + (this.Plktt.GetHashCode() ) 
                      * 23 ) + (this.Plkx1.GetHashCode() ) 
                      * 23 ) + (this.Plkx2.GetHashCode() ) 
                      * 23 ) + (this.Plkx3.GetHashCode() )                    );
           }
        }
    }
}
