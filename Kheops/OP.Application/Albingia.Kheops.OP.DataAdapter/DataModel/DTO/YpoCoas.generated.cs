using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPCOAS
    public partial class YpoCoas  {
             //YHPCOAS
             //YPOCOAS

            ///<summary>Public empty contructor</summary>
            public YpoCoas() {}
            ///<summary>Public empty contructor</summary>
            public YpoCoas(YpoCoas copyFrom) 
            {
                  this.Phipb= copyFrom.Phipb;
                  this.Phalx= copyFrom.Phalx;
                  this.Phavn= copyFrom.Phavn;
                  this.Phhin= copyFrom.Phhin;
                  this.Phtap= copyFrom.Phtap;
                  this.Phcie= copyFrom.Phcie;
                  this.Phinl= copyFrom.Phinl;
                  this.Phpol= copyFrom.Phpol;
                  this.Phapp= copyFrom.Phapp;
                  this.Phcom= copyFrom.Phcom;
                  this.Phtxf= copyFrom.Phtxf;
                  this.Phafr= copyFrom.Phafr;
                  this.Phepa= copyFrom.Phepa;
                  this.Phepm= copyFrom.Phepm;
                  this.Phepj= copyFrom.Phepj;
                  this.Phfpa= copyFrom.Phfpa;
                  this.Phfpm= copyFrom.Phfpm;
                  this.Phfpj= copyFrom.Phfpj;
                  this.Phin5= copyFrom.Phin5;
                  this.Phtac= copyFrom.Phtac;
                  this.Phtaa= copyFrom.Phtaa;
                  this.Phtam= copyFrom.Phtam;
                  this.Phtaj= copyFrom.Phtaj;
                  this.Phtyp= copyFrom.Phtyp;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Phipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Phalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Phavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Phhin { get; set; } 
            
            ///<summary> Type A/C  Apériteur - Coassureur </summary>
            public string Phtap { get; set; } 
            
            ///<summary> Identifiant Compagnie </summary>
            public string Phcie { get; set; } 
            
            ///<summary> Code interlocuteur </summary>
            public int Phinl { get; set; } 
            
            ///<summary> Référence police chez apérit-Coas </summary>
            public string Phpol { get; set; } 
            
            ///<summary> % Apérition </summary>
            public Decimal Phapp { get; set; } 
            
            ///<summary> Part de commissionnement </summary>
            public Decimal Phcom { get; set; } 
            
            ///<summary> Taux de frais apérition </summary>
            public Decimal Phtxf { get; set; } 
            
            ///<summary> Montant de frais accessoires </summary>
            public int Phafr { get; set; } 
            
            ///<summary> Effet participation Année </summary>
            public int Phepa { get; set; } 
            
            ///<summary> Effet participation Mois </summary>
            public int Phepm { get; set; } 
            
            ///<summary> Effet participation Jour </summary>
            public int Phepj { get; set; } 
            
            ///<summary> Fin de participation Année </summary>
            public int Phfpa { get; set; } 
            
            ///<summary> Fin de participation Mois </summary>
            public int Phfpm { get; set; } 
            
            ///<summary> Fin de participation Jour </summary>
            public int Phfpj { get; set; } 
            
            ///<summary> Code interlocuteur sur 5 </summary>
            public int Phin5 { get; set; } 
            
            ///<summary> Type accord S Signée N Non signée .. </summary>
            public string Phtac { get; set; } 
            
            ///<summary> Année Accord </summary>
            public int Phtaa { get; set; } 
            
            ///<summary> Mois Accord </summary>
            public int Phtam { get; set; } 
            
            ///<summary> Jour Accord </summary>
            public int Phtaj { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Phtyp { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoCoas  x=this,  y=obj as YpoCoas;
            if( y == default(YpoCoas) ) return false;
            return (
                    x.Phipb==y.Phipb
                    && x.Phalx==y.Phalx
                    && x.Phtap==y.Phtap
                    && x.Phcie==y.Phcie
                    && x.Phinl==y.Phinl
                    && x.Phpol==y.Phpol
                    && x.Phapp==y.Phapp
                    && x.Phcom==y.Phcom
                    && x.Phtxf==y.Phtxf
                    && x.Phafr==y.Phafr
                    && x.Phepa==y.Phepa
                    && x.Phepm==y.Phepm
                    && x.Phepj==y.Phepj
                    && x.Phfpa==y.Phfpa
                    && x.Phfpm==y.Phfpm
                    && x.Phfpj==y.Phfpj
                    && x.Phin5==y.Phin5
                    && x.Phtac==y.Phtac
                    && x.Phtaa==y.Phtaa
                    && x.Phtam==y.Phtam
                    && x.Phtaj==y.Phtaj
                    && x.Phtyp==y.Phtyp  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((
                       17 + (this.Phipb?? "").GetHashCode()
                      * 23 ) + (this.Phalx.GetHashCode() ) 
                      * 23 ) + (this.Phtap?? "").GetHashCode()
                      * 23 ) + (this.Phcie?? "").GetHashCode()
                      * 23 ) + (this.Phinl.GetHashCode() ) 
                      * 23 ) + (this.Phpol?? "").GetHashCode()
                      * 23 ) + (this.Phapp.GetHashCode() ) 
                      * 23 ) + (this.Phcom.GetHashCode() ) 
                      * 23 ) + (this.Phtxf.GetHashCode() ) 
                      * 23 ) + (this.Phafr.GetHashCode() ) 
                      * 23 ) + (this.Phepa.GetHashCode() ) 
                      * 23 ) + (this.Phepm.GetHashCode() ) 
                      * 23 ) + (this.Phepj.GetHashCode() ) 
                      * 23 ) + (this.Phfpa.GetHashCode() ) 
                      * 23 ) + (this.Phfpm.GetHashCode() ) 
                      * 23 ) + (this.Phfpj.GetHashCode() ) 
                      * 23 ) + (this.Phin5.GetHashCode() ) 
                      * 23 ) + (this.Phtac?? "").GetHashCode()
                      * 23 ) + (this.Phtaa.GetHashCode() ) 
                      * 23 ) + (this.Phtam.GetHashCode() ) 
                      * 23 ) + (this.Phtaj.GetHashCode() ) 
                      * 23 ) + (this.Phtyp?? "").GetHashCode()                   );
           }
        }
    }
}
