using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTFOR
    public partial class YprtFor  {
             //YHRTFOR
             //YPRTFOR

            ///<summary>Public empty contructor</summary>
            public YprtFor() {}
            ///<summary>Public empty contructor</summary>
            public YprtFor(YprtFor copyFrom) 
            {
                  this.Joipb= copyFrom.Joipb;
                  this.Joalx= copyFrom.Joalx;
                  this.Joavn= copyFrom.Joavn;
                  this.Johin= copyFrom.Johin;
                  this.Jorsq= copyFrom.Jorsq;
                  this.Jofor= copyFrom.Jofor;
                  this.Jodes= copyFrom.Jodes;
                  this.Jocch= copyFrom.Jocch;
                  this.Jofrv= copyFrom.Jofrv;
                  this.Jofsp= copyFrom.Jofsp;
                  this.Jorle= copyFrom.Jorle;
                  this.Joacq= copyFrom.Joacq;
                  this.Jotmc= copyFrom.Jotmc;
                  this.Jotff= copyFrom.Jotff;
                  this.Jotfp= copyFrom.Jotfp;
                  this.Jopro= copyFrom.Jopro;
                  this.Jotmi= copyFrom.Jotmi;
                  this.Jotfm= copyFrom.Jotfm;
                  this.Jotma= copyFrom.Jotma;
                  this.Jotmg= copyFrom.Jotmg;
                  this.Jocmc= copyFrom.Jocmc;
                  this.Jocfo= copyFrom.Jocfo;
                  this.Jocht= copyFrom.Jocht;
                  this.Joctt= copyFrom.Joctt;
                  this.Joccp= copyFrom.Joccp;
                  this.Jocpa= copyFrom.Jocpa;
                  this.Joval= copyFrom.Joval;
                  this.Jovaa= copyFrom.Jovaa;
                  this.Jovaw= copyFrom.Jovaw;
                  this.Jovat= copyFrom.Jovat;
                  this.Jovau= copyFrom.Jovau;
                  this.Jovah= copyFrom.Jovah;
                  this.Joivo= copyFrom.Joivo;
                  this.Joiva= copyFrom.Joiva;
                  this.Joivw= copyFrom.Joivw;
                  this.Joave= copyFrom.Joave;
                  this.Joava= copyFrom.Joava;
                  this.Joavm= copyFrom.Joavm;
                  this.Joavj= copyFrom.Joavj;
                  this.Jopaq= copyFrom.Jopaq;
                  this.Joehh= copyFrom.Joehh;
                  this.Joehc= copyFrom.Joehc;
                  this.Joehi= copyFrom.Joehi;
                  this.Jocos= copyFrom.Jocos;
                  this.Jotqu= copyFrom.Jotqu;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Joipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Joalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Joavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Johin { get; set; } 
            
            ///<summary> Identifiant Risque </summary>
            public int Jorsq { get; set; } 
            
            ///<summary> Identifiant formule </summary>
            public int Jofor { get; set; } 
            
            ///<summary> Désignation </summary>
            public string Jodes { get; set; } 
            
            ///<summary> Code formule chronologique </summary>
            public int Jocch { get; set; } 
            
            ///<summary> Renvoi : Id formule </summary>
            public int Jofrv { get; set; } 
            
            ///<summary> Formule specifique à Objets(s) O/N </summary>
            public string Jofsp { get; set; } 
            
            ///<summary> Lettre Formule de renvoi </summary>
            public string Jorle { get; set; } 
            
            ///<summary> Montant acquis </summary>
            public Int64 Joacq { get; set; } 
            
            ///<summary> Total : Montant calculé Référence </summary>
            public Decimal Jotmc { get; set; } 
            
            ///<summary> Total : Montant forcé Référence </summary>
            public Decimal Jotff { get; set; } 
            
            ///<summary> Total : Coefficient de calcul </summary>
            public Decimal Jotfp { get; set; } 
            
            ///<summary> Montant provisionnel O/N </summary>
            public string Jopro { get; set; } 
            
            ///<summary> Montant forcé pour minimum O/N </summary>
            public string Jotmi { get; set; } 
            
            ///<summary> Motif de total forcé </summary>
            public string Jotfm { get; set; } 
            
            ///<summary> Total : Montant Autre </summary>
            public Decimal Jotma { get; set; } 
            
            ///<summary> Total : Total général </summary>
            public Decimal Jotmg { get; set; } 
            
            ///<summary> Comptant : Montant calculé </summary>
            public Decimal Jocmc { get; set; } 
            
            ///<summary> Comptant : Montant forcé O/N </summary>
            public string Jocfo { get; set; } 
            
            ///<summary> Comptant : Montant forcé HT </summary>
            public Decimal Jocht { get; set; } 
            
            ///<summary> Comptant : Montant forcé TTC </summary>
            public Decimal Joctt { get; set; } 
            
            ///<summary> Comptant : Coefficient calcul forcé </summary>
            public Decimal Joccp { get; set; } 
            
            ///<summary> Coefficient de calcul </summary>
            public Decimal Jocpa { get; set; } 
            
            ///<summary> Valeur origine de la formule </summary>
            public Int64 Joval { get; set; } 
            
            ///<summary> Valeur Actualisée de la formule </summary>
            public Int64 Jovaa { get; set; } 
            
            ///<summary> W. Valeur de la formule(travail) </summary>
            public Int64 Jovaw { get; set; } 
            
            ///<summary> Type de valeur de la formule </summary>
            public string Jovat { get; set; } 
            
            ///<summary> Unité de la valeur </summary>
            public string Jovau { get; set; } 
            
            ///<summary> HT/TTC </summary>
            public string Jovah { get; set; } 
            
            ///<summary> Valeur de l'indice Origine </summary>
            public Decimal Joivo { get; set; } 
            
            ///<summary> Valeur de l'indice Actualisé </summary>
            public Decimal Joiva { get; set; } 
            
            ///<summary> W. Valeur de l'indice (travail) </summary>
            public Decimal Joivw { get; set; } 
            
            ///<summary> N° avenant de création </summary>
            public int Joave { get; set; } 
            
            ///<summary> Année Effet avenant FOR </summary>
            public int Joava { get; set; } 
            
            ///<summary> Mois  Effet avenant FOR </summary>
            public int Joavm { get; set; } 
            
            ///<summary> Jour  Effet avenant FOR </summary>
            public int Joavj { get; set; } 
            
            ///<summary> Montant acquis O/N </summary>
            public string Jopaq { get; set; } 
            
            ///<summary> Prochaine Echéance : Montant HT </summary>
            public Decimal Joehh { get; set; } 
            
            ///<summary> Prochaine Echéance : Mnt CATNAT HT </summary>
            public Decimal Joehc { get; set; } 
            
            ///<summary> Prochaine Echéance : Mnt Incendie HT </summary>
            public Decimal Joehi { get; set; } 
            
            ///<summary> Coeff Forcé ou calculé (F/C) </summary>
            public string Jocos { get; set; } 
            
            ///<summary> Traqueur O/N </summary>
            public string Jotqu { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtFor  x=this,  y=obj as YprtFor;
            if( y == default(YprtFor) ) return false;
            return (
                    x.Joipb==y.Joipb
                    && x.Joalx==y.Joalx
                    && x.Jorsq==y.Jorsq
                    && x.Jofor==y.Jofor
                    && x.Jodes==y.Jodes
                    && x.Jocch==y.Jocch
                    && x.Jofrv==y.Jofrv
                    && x.Jofsp==y.Jofsp
                    && x.Jorle==y.Jorle
                    && x.Joacq==y.Joacq
                    && x.Jotmc==y.Jotmc
                    && x.Jotff==y.Jotff
                    && x.Jotfp==y.Jotfp
                    && x.Jopro==y.Jopro
                    && x.Jotmi==y.Jotmi
                    && x.Jotfm==y.Jotfm
                    && x.Jotma==y.Jotma
                    && x.Jotmg==y.Jotmg
                    && x.Jocmc==y.Jocmc
                    && x.Jocfo==y.Jocfo
                    && x.Jocht==y.Jocht
                    && x.Joctt==y.Joctt
                    && x.Joccp==y.Joccp
                    && x.Jocpa==y.Jocpa
                    && x.Joval==y.Joval
                    && x.Jovaa==y.Jovaa
                    && x.Jovaw==y.Jovaw
                    && x.Jovat==y.Jovat
                    && x.Jovau==y.Jovau
                    && x.Jovah==y.Jovah
                    && x.Joivo==y.Joivo
                    && x.Joiva==y.Joiva
                    && x.Joivw==y.Joivw
                    && x.Joave==y.Joave
                    && x.Joava==y.Joava
                    && x.Joavm==y.Joavm
                    && x.Joavj==y.Joavj
                    && x.Jopaq==y.Jopaq
                    && x.Joehh==y.Joehh
                    && x.Joehc==y.Joehc
                    && x.Joehi==y.Joehi
                    && x.Jocos==y.Jocos
                    && x.Jotqu==y.Jotqu  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Joipb?? "").GetHashCode()
                      * 23 ) + (this.Joalx.GetHashCode() ) 
                      * 23 ) + (this.Jorsq.GetHashCode() ) 
                      * 23 ) + (this.Jofor.GetHashCode() ) 
                      * 23 ) + (this.Jodes?? "").GetHashCode()
                      * 23 ) + (this.Jocch.GetHashCode() ) 
                      * 23 ) + (this.Jofrv.GetHashCode() ) 
                      * 23 ) + (this.Jofsp?? "").GetHashCode()
                      * 23 ) + (this.Jorle?? "").GetHashCode()
                      * 23 ) + (this.Joacq.GetHashCode() ) 
                      * 23 ) + (this.Jotmc.GetHashCode() ) 
                      * 23 ) + (this.Jotff.GetHashCode() ) 
                      * 23 ) + (this.Jotfp.GetHashCode() ) 
                      * 23 ) + (this.Jopro?? "").GetHashCode()
                      * 23 ) + (this.Jotmi?? "").GetHashCode()
                      * 23 ) + (this.Jotfm?? "").GetHashCode()
                      * 23 ) + (this.Jotma.GetHashCode() ) 
                      * 23 ) + (this.Jotmg.GetHashCode() ) 
                      * 23 ) + (this.Jocmc.GetHashCode() ) 
                      * 23 ) + (this.Jocfo?? "").GetHashCode()
                      * 23 ) + (this.Jocht.GetHashCode() ) 
                      * 23 ) + (this.Joctt.GetHashCode() ) 
                      * 23 ) + (this.Joccp.GetHashCode() ) 
                      * 23 ) + (this.Jocpa.GetHashCode() ) 
                      * 23 ) + (this.Joval.GetHashCode() ) 
                      * 23 ) + (this.Jovaa.GetHashCode() ) 
                      * 23 ) + (this.Jovaw.GetHashCode() ) 
                      * 23 ) + (this.Jovat?? "").GetHashCode()
                      * 23 ) + (this.Jovau?? "").GetHashCode()
                      * 23 ) + (this.Jovah?? "").GetHashCode()
                      * 23 ) + (this.Joivo.GetHashCode() ) 
                      * 23 ) + (this.Joiva.GetHashCode() ) 
                      * 23 ) + (this.Joivw.GetHashCode() ) 
                      * 23 ) + (this.Joave.GetHashCode() ) 
                      * 23 ) + (this.Joava.GetHashCode() ) 
                      * 23 ) + (this.Joavm.GetHashCode() ) 
                      * 23 ) + (this.Joavj.GetHashCode() ) 
                      * 23 ) + (this.Jopaq?? "").GetHashCode()
                      * 23 ) + (this.Joehh.GetHashCode() ) 
                      * 23 ) + (this.Joehc.GetHashCode() ) 
                      * 23 ) + (this.Joehi.GetHashCode() ) 
                      * 23 ) + (this.Jocos?? "").GetHashCode()
                      * 23 ) + (this.Jotqu?? "").GetHashCode()                   );
           }
        }
    }
}
