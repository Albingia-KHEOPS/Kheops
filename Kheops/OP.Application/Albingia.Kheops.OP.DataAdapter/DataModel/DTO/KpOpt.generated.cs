using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPOPT
    public partial class KpOpt  {
             //HPOPT
             //KPOPT

            ///<summary>Public empty contructor</summary>
            public KpOpt() {}
            ///<summary>Public empty contructor</summary>
            public KpOpt(KpOpt copyFrom) 
            {
                  this.Kdbid= copyFrom.Kdbid;
                  this.Kdbtyp= copyFrom.Kdbtyp;
                  this.Kdbipb= copyFrom.Kdbipb;
                  this.Kdbalx= copyFrom.Kdbalx;
                  this.Kdbavn= copyFrom.Kdbavn;
                  this.Kdbhin= copyFrom.Kdbhin;
                  this.Kdbfor= copyFrom.Kdbfor;
                  this.Kdbkdaid= copyFrom.Kdbkdaid;
                  this.Kdbopt= copyFrom.Kdbopt;
                  this.Kdbdesc= copyFrom.Kdbdesc;
                  this.Kdbforr= copyFrom.Kdbforr;
                  this.Kdbkdaidr= copyFrom.Kdbkdaidr;
                  this.Kdbspeid= copyFrom.Kdbspeid;
                  this.Kdbcru= copyFrom.Kdbcru;
                  this.Kdbcrd= copyFrom.Kdbcrd;
                  this.Kdbcrh= copyFrom.Kdbcrh;
                  this.Kdbmaju= copyFrom.Kdbmaju;
                  this.Kdbmajd= copyFrom.Kdbmajd;
                  this.Kdbmajh= copyFrom.Kdbmajh;
                  this.Kdbpaq= copyFrom.Kdbpaq;
                  this.Kdbacq= copyFrom.Kdbacq;
                  this.Kdbtmc= copyFrom.Kdbtmc;
                  this.Kdbtff= copyFrom.Kdbtff;
                  this.Kdbtfp= copyFrom.Kdbtfp;
                  this.Kdbpro= copyFrom.Kdbpro;
                  this.Kdbtmi= copyFrom.Kdbtmi;
                  this.Kdbtfm= copyFrom.Kdbtfm;
                  this.Kdbcmc= copyFrom.Kdbcmc;
                  this.Kdbcfo= copyFrom.Kdbcfo;
                  this.Kdbcht= copyFrom.Kdbcht;
                  this.Kdbctt= copyFrom.Kdbctt;
                  this.Kdbccp= copyFrom.Kdbccp;
                  this.Kdbval= copyFrom.Kdbval;
                  this.Kdbvaa= copyFrom.Kdbvaa;
                  this.Kdbvaw= copyFrom.Kdbvaw;
                  this.Kdbvat= copyFrom.Kdbvat;
                  this.Kdbvau= copyFrom.Kdbvau;
                  this.Kdbvah= copyFrom.Kdbvah;
                  this.Kdbivo= copyFrom.Kdbivo;
                  this.Kdbiva= copyFrom.Kdbiva;
                  this.Kdbivw= copyFrom.Kdbivw;
                  this.Kdbave= copyFrom.Kdbave;
                  this.Kdbavg= copyFrom.Kdbavg;
                  this.Kdbeco= copyFrom.Kdbeco;
                  this.Kdbava= copyFrom.Kdbava;
                  this.Kdbavm= copyFrom.Kdbavm;
                  this.Kdbavj= copyFrom.Kdbavj;
                  this.Kdbehh= copyFrom.Kdbehh;
                  this.Kdbehc= copyFrom.Kdbehc;
                  this.Kdbehi= copyFrom.Kdbehi;
                  this.Kdbasvalo= copyFrom.Kdbasvalo;
                  this.Kdbasvala= copyFrom.Kdbasvala;
                  this.Kdbasvalw= copyFrom.Kdbasvalw;
                  this.Kdbasunit= copyFrom.Kdbasunit;
                  this.Kdbasbase= copyFrom.Kdbasbase;
                  this.Kdbger= copyFrom.Kdbger;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdbid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdbtyp { get; set; } 
            
            ///<summary> IPB/ALX </summary>
            public string Kdbipb { get; set; } 
            
            ///<summary> Aliment/version </summary>
            public int Kdbalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdbavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdbhin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdbfor { get; set; } 
            
            ///<summary> ID KPFOR </summary>
            public Int64 Kdbkdaid { get; set; } 
            
            ///<summary> Option </summary>
            public int Kdbopt { get; set; } 
            
            ///<summary> Description </summary>
            public string Kdbdesc { get; set; } 
            
            ///<summary> Formule si renvoi </summary>
            public int Kdbforr { get; set; } 
            
            ///<summary> ID KPFOR </summary>
            public Int64 Kdbkdaidr { get; set; } 
            
            ///<summary> ID Lien KPIOPT </summary>
            public Int64 Kdbspeid { get; set; } 
            
            ///<summary> Création user </summary>
            public string Kdbcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdbcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kdbcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kdbmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kdbmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kdbmajh { get; set; } 
            
            ///<summary> Montant Acquis O/N </summary>
            public string Kdbpaq { get; set; } 
            
            ///<summary> Montant acquis </summary>
            public Decimal Kdbacq { get; set; } 
            
            ///<summary> Total : Montant Calculé Référence </summary>
            public Decimal Kdbtmc { get; set; } 
            
            ///<summary> Total : Montant Forcé Référence </summary>
            public Decimal Kdbtff { get; set; } 
            
            ///<summary> Total : Coefficient de calcul </summary>
            public Decimal Kdbtfp { get; set; } 
            
            ///<summary> Total : Montant Provisionnel O/N </summary>
            public string Kdbpro { get; set; } 
            
            ///<summary> Total : Mnt forcé pour Minimum O/N </summary>
            public string Kdbtmi { get; set; } 
            
            ///<summary> Total : Motif forcé </summary>
            public string Kdbtfm { get; set; } 
            
            ///<summary> Comptant : Montant Calculé </summary>
            public Decimal Kdbcmc { get; set; } 
            
            ///<summary> Comptant : Montant Forcé O/N </summary>
            public string Kdbcfo { get; set; } 
            
            ///<summary> Comptant : Montant Forcé HT </summary>
            public Decimal Kdbcht { get; set; } 
            
            ///<summary> Comptant : montant forcé TTC </summary>
            public Decimal Kdbctt { get; set; } 
            
            ///<summary> Comptant : Coefficient calcul forcé </summary>
            public Decimal Kdbccp { get; set; } 
            
            ///<summary> Valeur origine </summary>
            public Decimal Kdbval { get; set; } 
            
            ///<summary> Valeur Actualisée </summary>
            public Decimal Kdbvaa { get; set; } 
            
            ///<summary> Valeur de travail </summary>
            public Decimal Kdbvaw { get; set; } 
            
            ///<summary> Type de valeur </summary>
            public string Kdbvat { get; set; } 
            
            ///<summary> Unité de la valeur </summary>
            public string Kdbvau { get; set; } 
            
            ///<summary> HT/TTC </summary>
            public string Kdbvah { get; set; } 
            
            ///<summary> Valeur de l'indice Origine </summary>
            public Decimal Kdbivo { get; set; } 
            
            ///<summary> Valeur Indice Actualisé </summary>
            public Decimal Kdbiva { get; set; } 
            
            ///<summary> Valeur Indice de travail </summary>
            public Decimal Kdbivw { get; set; } 
            
            ///<summary> N° d'avenant de Création </summary>
            public int Kdbave { get; set; } 
            
            ///<summary> N° avenant de modification </summary>
            public int Kdbavg { get; set; } 
            
            ///<summary> Formule en-cours O/N </summary>
            public string Kdbeco { get; set; } 
            
            ///<summary> Année Effet Avenant Formule </summary>
            public int Kdbava { get; set; } 
            
            ///<summary> Mois  Effet Avenant Formule </summary>
            public int Kdbavm { get; set; } 
            
            ///<summary> Jour  Effet Avenant Formule </summary>
            public int Kdbavj { get; set; } 
            
            ///<summary> Prochaine Echéance HT </summary>
            public Decimal Kdbehh { get; set; } 
            
            ///<summary> Prochaine Echéance CATNAT </summary>
            public Decimal Kdbehc { get; set; } 
            
            ///<summary> Prochaine Echéance Incendie </summary>
            public Decimal Kdbehi { get; set; } 
            
            ///<summary> Assiette Valeur Origine </summary>
            public Decimal Kdbasvalo { get; set; } 
            
            ///<summary> Assiette Valeur Actualisée </summary>
            public Decimal Kdbasvala { get; set; } 
            
            ///<summary> Assiette Valeur de travail </summary>
            public Decimal Kdbasvalw { get; set; } 
            
            ///<summary> Assiette Unité </summary>
            public string Kdbasunit { get; set; } 
            
            ///<summary> Assiette Base (Type Valeur) </summary>
            public string Kdbasbase { get; set; } 
            
            ///<summary> Mnt Ref Montant forcé saisi </summary>
            public Decimal Kdbger { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOpt  x=this,  y=obj as KpOpt;
            if( y == default(KpOpt) ) return false;
            return (
                    x.Kdbid==y.Kdbid
                    && x.Kdbtyp==y.Kdbtyp
                    && x.Kdbipb==y.Kdbipb
                    && x.Kdbalx==y.Kdbalx
                    && x.Kdbfor==y.Kdbfor
                    && x.Kdbkdaid==y.Kdbkdaid
                    && x.Kdbopt==y.Kdbopt
                    && x.Kdbdesc==y.Kdbdesc
                    && x.Kdbforr==y.Kdbforr
                    && x.Kdbkdaidr==y.Kdbkdaidr
                    && x.Kdbspeid==y.Kdbspeid
                    && x.Kdbcru==y.Kdbcru
                    && x.Kdbcrd==y.Kdbcrd
                    && x.Kdbcrh==y.Kdbcrh
                    && x.Kdbmaju==y.Kdbmaju
                    && x.Kdbmajd==y.Kdbmajd
                    && x.Kdbmajh==y.Kdbmajh
                    && x.Kdbpaq==y.Kdbpaq
                    && x.Kdbacq==y.Kdbacq
                    && x.Kdbtmc==y.Kdbtmc
                    && x.Kdbtff==y.Kdbtff
                    && x.Kdbtfp==y.Kdbtfp
                    && x.Kdbpro==y.Kdbpro
                    && x.Kdbtmi==y.Kdbtmi
                    && x.Kdbtfm==y.Kdbtfm
                    && x.Kdbcmc==y.Kdbcmc
                    && x.Kdbcfo==y.Kdbcfo
                    && x.Kdbcht==y.Kdbcht
                    && x.Kdbctt==y.Kdbctt
                    && x.Kdbccp==y.Kdbccp
                    && x.Kdbval==y.Kdbval
                    && x.Kdbvaa==y.Kdbvaa
                    && x.Kdbvaw==y.Kdbvaw
                    && x.Kdbvat==y.Kdbvat
                    && x.Kdbvau==y.Kdbvau
                    && x.Kdbvah==y.Kdbvah
                    && x.Kdbivo==y.Kdbivo
                    && x.Kdbiva==y.Kdbiva
                    && x.Kdbivw==y.Kdbivw
                    && x.Kdbave==y.Kdbave
                    && x.Kdbavg==y.Kdbavg
                    && x.Kdbeco==y.Kdbeco
                    && x.Kdbava==y.Kdbava
                    && x.Kdbavm==y.Kdbavm
                    && x.Kdbavj==y.Kdbavj
                    && x.Kdbehh==y.Kdbehh
                    && x.Kdbehc==y.Kdbehc
                    && x.Kdbehi==y.Kdbehi
                    && x.Kdbasvalo==y.Kdbasvalo
                    && x.Kdbasvala==y.Kdbasvala
                    && x.Kdbasvalw==y.Kdbasvalw
                    && x.Kdbasunit==y.Kdbasunit
                    && x.Kdbasbase==y.Kdbasbase
                    && x.Kdbger==y.Kdbger  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kdbid.GetHashCode() ) 
                      * 23 ) + (this.Kdbtyp?? "").GetHashCode()
                      * 23 ) + (this.Kdbipb?? "").GetHashCode()
                      * 23 ) + (this.Kdbalx.GetHashCode() ) 
                      * 23 ) + (this.Kdbfor.GetHashCode() ) 
                      * 23 ) + (this.Kdbkdaid.GetHashCode() ) 
                      * 23 ) + (this.Kdbopt.GetHashCode() ) 
                      * 23 ) + (this.Kdbdesc?? "").GetHashCode()
                      * 23 ) + (this.Kdbforr.GetHashCode() ) 
                      * 23 ) + (this.Kdbkdaidr.GetHashCode() ) 
                      * 23 ) + (this.Kdbspeid.GetHashCode() ) 
                      * 23 ) + (this.Kdbcru?? "").GetHashCode()
                      * 23 ) + (this.Kdbcrd.GetHashCode() ) 
                      * 23 ) + (this.Kdbcrh.GetHashCode() ) 
                      * 23 ) + (this.Kdbmaju?? "").GetHashCode()
                      * 23 ) + (this.Kdbmajd.GetHashCode() ) 
                      * 23 ) + (this.Kdbmajh.GetHashCode() ) 
                      * 23 ) + (this.Kdbpaq?? "").GetHashCode()
                      * 23 ) + (this.Kdbacq.GetHashCode() ) 
                      * 23 ) + (this.Kdbtmc.GetHashCode() ) 
                      * 23 ) + (this.Kdbtff.GetHashCode() ) 
                      * 23 ) + (this.Kdbtfp.GetHashCode() ) 
                      * 23 ) + (this.Kdbpro?? "").GetHashCode()
                      * 23 ) + (this.Kdbtmi?? "").GetHashCode()
                      * 23 ) + (this.Kdbtfm?? "").GetHashCode()
                      * 23 ) + (this.Kdbcmc.GetHashCode() ) 
                      * 23 ) + (this.Kdbcfo?? "").GetHashCode()
                      * 23 ) + (this.Kdbcht.GetHashCode() ) 
                      * 23 ) + (this.Kdbctt.GetHashCode() ) 
                      * 23 ) + (this.Kdbccp.GetHashCode() ) 
                      * 23 ) + (this.Kdbval.GetHashCode() ) 
                      * 23 ) + (this.Kdbvaa.GetHashCode() ) 
                      * 23 ) + (this.Kdbvaw.GetHashCode() ) 
                      * 23 ) + (this.Kdbvat?? "").GetHashCode()
                      * 23 ) + (this.Kdbvau?? "").GetHashCode()
                      * 23 ) + (this.Kdbvah?? "").GetHashCode()
                      * 23 ) + (this.Kdbivo.GetHashCode() ) 
                      * 23 ) + (this.Kdbiva.GetHashCode() ) 
                      * 23 ) + (this.Kdbivw.GetHashCode() ) 
                      * 23 ) + (this.Kdbave.GetHashCode() ) 
                      * 23 ) + (this.Kdbavg.GetHashCode() ) 
                      * 23 ) + (this.Kdbeco?? "").GetHashCode()
                      * 23 ) + (this.Kdbava.GetHashCode() ) 
                      * 23 ) + (this.Kdbavm.GetHashCode() ) 
                      * 23 ) + (this.Kdbavj.GetHashCode() ) 
                      * 23 ) + (this.Kdbehh.GetHashCode() ) 
                      * 23 ) + (this.Kdbehc.GetHashCode() ) 
                      * 23 ) + (this.Kdbehi.GetHashCode() ) 
                      * 23 ) + (this.Kdbasvalo.GetHashCode() ) 
                      * 23 ) + (this.Kdbasvala.GetHashCode() ) 
                      * 23 ) + (this.Kdbasvalw.GetHashCode() ) 
                      * 23 ) + (this.Kdbasunit?? "").GetHashCode()
                      * 23 ) + (this.Kdbasbase?? "").GetHashCode()
                      * 23 ) + (this.Kdbger.GetHashCode() )                    );
           }
        }
    }
}
