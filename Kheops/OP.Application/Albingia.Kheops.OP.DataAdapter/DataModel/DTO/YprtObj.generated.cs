using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTOBJ
    public partial class YprtObj  {
             //YHRTOBJ
             //YPRTOBJ

            ///<summary>Public empty contructor</summary>
            public YprtObj() {}
            ///<summary>Public empty contructor</summary>
            public YprtObj(YprtObj copyFrom) 
            {
                  this.Jgipb= copyFrom.Jgipb;
                  this.Jgalx= copyFrom.Jgalx;
                  this.Jgavn= copyFrom.Jgavn;
                  this.Jghin= copyFrom.Jghin;
                  this.Jgrsq= copyFrom.Jgrsq;
                  this.Jgobj= copyFrom.Jgobj;
                  this.Jgcch= copyFrom.Jgcch;
                  this.Jgigd= copyFrom.Jgigd;
                  this.Jgbra= copyFrom.Jgbra;
                  this.Jgsbr= copyFrom.Jgsbr;
                  this.Jgcat= copyFrom.Jgcat;
                  this.Jgrcs= copyFrom.Jgrcs;
                  this.Jgccs= copyFrom.Jgccs;
                  this.Jgval= copyFrom.Jgval;
                  this.Jgvaa= copyFrom.Jgvaa;
                  this.Jgvaw= copyFrom.Jgvaw;
                  this.Jgvat= copyFrom.Jgvat;
                  this.Jgvau= copyFrom.Jgvau;
                  this.Jgvah= copyFrom.Jgvah;
                  this.Jgnoj= copyFrom.Jgnoj;
                  this.Jgmmq= copyFrom.Jgmmq;
                  this.Jgmty= copyFrom.Jgmty;
                  this.Jgmsr= copyFrom.Jgmsr;
                  this.Jgmfa= copyFrom.Jgmfa;
                  this.Jgtem= copyFrom.Jgtem;
                  this.Jgvgd= copyFrom.Jgvgd;
                  this.Jgvgu= copyFrom.Jgvgu;
                  this.Jgvda= copyFrom.Jgvda;
                  this.Jgvdm= copyFrom.Jgvdm;
                  this.Jgvdj= copyFrom.Jgvdj;
                  this.Jgvdh= copyFrom.Jgvdh;
                  this.Jgvfa= copyFrom.Jgvfa;
                  this.Jgvfm= copyFrom.Jgvfm;
                  this.Jgvfj= copyFrom.Jgvfj;
                  this.Jgvfh= copyFrom.Jgvfh;
                  this.Jgrgt= copyFrom.Jgrgt;
                  this.Jgtrr= copyFrom.Jgtrr;
                  this.Jgcna= copyFrom.Jgcna;
                  this.Jgina= copyFrom.Jgina;
                  this.Jgind= copyFrom.Jgind;
                  this.Jgixc= copyFrom.Jgixc;
                  this.Jgixf= copyFrom.Jgixf;
                  this.Jgixl= copyFrom.Jgixl;
                  this.Jgixp= copyFrom.Jgixp;
                  this.Jgivo= copyFrom.Jgivo;
                  this.Jgiva= copyFrom.Jgiva;
                  this.Jgivw= copyFrom.Jgivw;
                  this.Jggau= copyFrom.Jggau;
                  this.Jggvl= copyFrom.Jggvl;
                  this.Jggun= copyFrom.Jggun;
                  this.Jgpbn= copyFrom.Jgpbn;
                  this.Jgpbs= copyFrom.Jgpbs;
                  this.Jgpbr= copyFrom.Jgpbr;
                  this.Jgpbt= copyFrom.Jgpbt;
                  this.Jgpbc= copyFrom.Jgpbc;
                  this.Jgpbp= copyFrom.Jgpbp;
                  this.Jgpba= copyFrom.Jgpba;
                  this.Jgclv= copyFrom.Jgclv;
                  this.Jgagm= copyFrom.Jgagm;
                  this.Jgave= copyFrom.Jgave;
                  this.Jgava= copyFrom.Jgava;
                  this.Jgavm= copyFrom.Jgavm;
                  this.Jgavj= copyFrom.Jgavj;
                  this.Jgrul= copyFrom.Jgrul;
                  this.Jgrut= copyFrom.Jgrut;
                  this.Jgavf= copyFrom.Jgavf;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Jgipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Jgalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Jgavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Jghin { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Jgrsq { get; set; } 
            
            ///<summary> Identifiant objet </summary>
            public int Jgobj { get; set; } 
            
            ///<summary> Code chronologique Objet </summary>
            public int Jgcch { get; set; } 
            
            ///<summary> Identifiant Matériel </summary>
            public int Jgigd { get; set; } 
            
            ///<summary> Branche </summary>
            public string Jgbra { get; set; } 
            
            ///<summary> Sous-branche </summary>
            public string Jgsbr { get; set; } 
            
            ///<summary> Catégorie </summary>
            public string Jgcat { get; set; } 
            
            ///<summary> Référence Conventions spéciales </summary>
            public string Jgrcs { get; set; } 
            
            ///<summary> Code référence CS </summary>
            public string Jgccs { get; set; } 
            
            ///<summary> Valeur de l'objet Origine </summary>
            public Int64 Jgval { get; set; } 
            
            ///<summary> Valeur Actualisé de l'objet </summary>
            public Int64 Jgvaa { get; set; } 
            
            ///<summary> W. Valeur de l'objet (travail) </summary>
            public Int64 Jgvaw { get; set; } 
            
            ///<summary> Type de valeur de l'objet </summary>
            public string Jgvat { get; set; } 
            
            ///<summary> Unité de la valeur </summary>
            public string Jgvau { get; set; } 
            
            ///<summary> HT ou TTC  H/T </summary>
            public string Jgvah { get; set; } 
            
            ///<summary> Nombre de matériel </summary>
            public int Jgnoj { get; set; } 
            
            ///<summary> Marque </summary>
            public string Jgmmq { get; set; } 
            
            ///<summary> Type de matériel </summary>
            public string Jgmty { get; set; } 
            
            ///<summary> Série de matériel </summary>
            public string Jgmsr { get; set; } 
            
            ///<summary> Année de fabrication </summary>
            public int Jgmfa { get; set; } 
            
            ///<summary> Garantie temporaire O/N </summary>
            public string Jgtem { get; set; } 
            
            ///<summary> Validité garantie : Durée </summary>
            public int Jgvgd { get; set; } 
            
            ///<summary> Validité garantie : Unité </summary>
            public string Jgvgu { get; set; } 
            
            ///<summary> Validité garantie : Année début </summary>
            public int Jgvda { get; set; } 
            
            ///<summary> Validité garantie : Mois début </summary>
            public int Jgvdm { get; set; } 
            
            ///<summary> Validité garantie : Jour début </summary>
            public int Jgvdj { get; set; } 
            
            ///<summary> Validité garantie : Heure début </summary>
            public int Jgvdh { get; set; } 
            
            ///<summary> Validité garantie : Année fin </summary>
            public int Jgvfa { get; set; } 
            
            ///<summary> Validité garantie : Mois fin </summary>
            public int Jgvfm { get; set; } 
            
            ///<summary> Validité garantie : Jour fin </summary>
            public int Jgvfj { get; set; } 
            
            ///<summary> Validité garantie : Heure fin </summary>
            public int Jgvfh { get; set; } 
            
            ///<summary> Régime de taxe </summary>
            public string Jgrgt { get; set; } 
            
            ///<summary> Code territorialité </summary>
            public string Jgtrr { get; set; } 
            
            ///<summary> Application CATNAT O/N </summary>
            public string Jgcna { get; set; } 
            
            ///<summary> Indexation (O/N) </summary>
            public string Jgina { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public string Jgind { get; set; } 
            
            ///<summary> Indexation des capitaux (O/N) </summary>
            public string Jgixc { get; set; } 
            
            ///<summary> Indexation Franchises </summary>
            public string Jgixf { get; set; } 
            
            ///<summary> Indexation LCI (O/N) </summary>
            public string Jgixl { get; set; } 
            
            ///<summary> Indexation Primes (O/N) </summary>
            public string Jgixp { get; set; } 
            
            ///<summary> Valeur de l'indice Origine </summary>
            public Decimal Jgivo { get; set; } 
            
            ///<summary> Valeur de l'indice actualisé </summary>
            public Decimal Jgiva { get; set; } 
            
            ///<summary> W. Valeur de l'indice (Travail) </summary>
            public Decimal Jgivw { get; set; } 
            
            ///<summary> Garantie automatique O/N </summary>
            public string Jggau { get; set; } 
            
            ///<summary> Garantie automatique : Valeur limite </summary>
            public int Jggvl { get; set; } 
            
            ///<summary> Garantie automatique : Unité limite </summary>
            public string Jggun { get; set; } 
            
            ///<summary> Participation bénéficiaire O/N </summary>
            public string Jgpbn { get; set; } 
            
            ///<summary> PB : Seuil de rapport S/P </summary>
            public int Jgpbs { get; set; } 
            
            ///<summary> PB : Montant de ristourne en % </summary>
            public int Jgpbr { get; set; } 
            
            ///<summary> PB : Taux appel de prime </summary>
            public int Jgpbt { get; set; } 
            
            ///<summary> PB : Complément taux d'appel </summary>
            public int Jgpbc { get; set; } 
            
            ///<summary> PB : % de prime retenue </summary>
            public int Jgpbp { get; set; } 
            
            ///<summary> PB : Nombre d'années </summary>
            public int Jgpba { get; set; } 
            
            ///<summary> Collision renversement O/N </summary>
            public string Jgclv { get; set; } 
            
            ///<summary> Age du matériel pour collision renv </summary>
            public int Jgagm { get; set; } 
            
            ///<summary> N° avenant de création </summary>
            public int Jgave { get; set; } 
            
            ///<summary> Année Effet avenant OBJ </summary>
            public int Jgava { get; set; } 
            
            ///<summary> Mois  Effet avenant OBJ </summary>
            public int Jgavm { get; set; } 
            
            ///<summary> Jour  Effet avenant OBJ </summary>
            public int Jgavj { get; set; } 
            
            ///<summary> A régulariser O/N </summary>
            public string Jgrul { get; set; } 
            
            ///<summary> Type de régularisation (A,C,E...) </summary>
            public string Jgrut { get; set; } 
            
            ///<summary> N° avenant de modification </summary>
            public int Jgavf { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtObj  x=this,  y=obj as YprtObj;
            if( y == default(YprtObj) ) return false;
            return (
                    x.Jgipb==y.Jgipb
                    && x.Jgalx==y.Jgalx
                    && x.Jgrsq==y.Jgrsq
                    && x.Jgobj==y.Jgobj
                    && x.Jgcch==y.Jgcch
                    && x.Jgigd==y.Jgigd
                    && x.Jgbra==y.Jgbra
                    && x.Jgsbr==y.Jgsbr
                    && x.Jgcat==y.Jgcat
                    && x.Jgrcs==y.Jgrcs
                    && x.Jgccs==y.Jgccs
                    && x.Jgval==y.Jgval
                    && x.Jgvaa==y.Jgvaa
                    && x.Jgvaw==y.Jgvaw
                    && x.Jgvat==y.Jgvat
                    && x.Jgvau==y.Jgvau
                    && x.Jgvah==y.Jgvah
                    && x.Jgnoj==y.Jgnoj
                    && x.Jgmmq==y.Jgmmq
                    && x.Jgmty==y.Jgmty
                    && x.Jgmsr==y.Jgmsr
                    && x.Jgmfa==y.Jgmfa
                    && x.Jgtem==y.Jgtem
                    && x.Jgvgd==y.Jgvgd
                    && x.Jgvgu==y.Jgvgu
                    && x.Jgvda==y.Jgvda
                    && x.Jgvdm==y.Jgvdm
                    && x.Jgvdj==y.Jgvdj
                    && x.Jgvdh==y.Jgvdh
                    && x.Jgvfa==y.Jgvfa
                    && x.Jgvfm==y.Jgvfm
                    && x.Jgvfj==y.Jgvfj
                    && x.Jgvfh==y.Jgvfh
                    && x.Jgrgt==y.Jgrgt
                    && x.Jgtrr==y.Jgtrr
                    && x.Jgcna==y.Jgcna
                    && x.Jgina==y.Jgina
                    && x.Jgind==y.Jgind
                    && x.Jgixc==y.Jgixc
                    && x.Jgixf==y.Jgixf
                    && x.Jgixl==y.Jgixl
                    && x.Jgixp==y.Jgixp
                    && x.Jgivo==y.Jgivo
                    && x.Jgiva==y.Jgiva
                    && x.Jgivw==y.Jgivw
                    && x.Jggau==y.Jggau
                    && x.Jggvl==y.Jggvl
                    && x.Jggun==y.Jggun
                    && x.Jgpbn==y.Jgpbn
                    && x.Jgpbs==y.Jgpbs
                    && x.Jgpbr==y.Jgpbr
                    && x.Jgpbt==y.Jgpbt
                    && x.Jgpbc==y.Jgpbc
                    && x.Jgpbp==y.Jgpbp
                    && x.Jgpba==y.Jgpba
                    && x.Jgclv==y.Jgclv
                    && x.Jgagm==y.Jgagm
                    && x.Jgave==y.Jgave
                    && x.Jgava==y.Jgava
                    && x.Jgavm==y.Jgavm
                    && x.Jgavj==y.Jgavj
                    && x.Jgrul==y.Jgrul
                    && x.Jgrut==y.Jgrut
                    && x.Jgavf==y.Jgavf  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Jgipb?? "").GetHashCode()
                      * 23 ) + (this.Jgalx.GetHashCode() ) 
                      * 23 ) + (this.Jgrsq.GetHashCode() ) 
                      * 23 ) + (this.Jgobj.GetHashCode() ) 
                      * 23 ) + (this.Jgcch.GetHashCode() ) 
                      * 23 ) + (this.Jgigd.GetHashCode() ) 
                      * 23 ) + (this.Jgbra?? "").GetHashCode()
                      * 23 ) + (this.Jgsbr?? "").GetHashCode()
                      * 23 ) + (this.Jgcat?? "").GetHashCode()
                      * 23 ) + (this.Jgrcs?? "").GetHashCode()
                      * 23 ) + (this.Jgccs?? "").GetHashCode()
                      * 23 ) + (this.Jgval.GetHashCode() ) 
                      * 23 ) + (this.Jgvaa.GetHashCode() ) 
                      * 23 ) + (this.Jgvaw.GetHashCode() ) 
                      * 23 ) + (this.Jgvat?? "").GetHashCode()
                      * 23 ) + (this.Jgvau?? "").GetHashCode()
                      * 23 ) + (this.Jgvah?? "").GetHashCode()
                      * 23 ) + (this.Jgnoj.GetHashCode() ) 
                      * 23 ) + (this.Jgmmq?? "").GetHashCode()
                      * 23 ) + (this.Jgmty?? "").GetHashCode()
                      * 23 ) + (this.Jgmsr?? "").GetHashCode()
                      * 23 ) + (this.Jgmfa.GetHashCode() ) 
                      * 23 ) + (this.Jgtem?? "").GetHashCode()
                      * 23 ) + (this.Jgvgd.GetHashCode() ) 
                      * 23 ) + (this.Jgvgu?? "").GetHashCode()
                      * 23 ) + (this.Jgvda.GetHashCode() ) 
                      * 23 ) + (this.Jgvdm.GetHashCode() ) 
                      * 23 ) + (this.Jgvdj.GetHashCode() ) 
                      * 23 ) + (this.Jgvdh.GetHashCode() ) 
                      * 23 ) + (this.Jgvfa.GetHashCode() ) 
                      * 23 ) + (this.Jgvfm.GetHashCode() ) 
                      * 23 ) + (this.Jgvfj.GetHashCode() ) 
                      * 23 ) + (this.Jgvfh.GetHashCode() ) 
                      * 23 ) + (this.Jgrgt?? "").GetHashCode()
                      * 23 ) + (this.Jgtrr?? "").GetHashCode()
                      * 23 ) + (this.Jgcna?? "").GetHashCode()
                      * 23 ) + (this.Jgina?? "").GetHashCode()
                      * 23 ) + (this.Jgind?? "").GetHashCode()
                      * 23 ) + (this.Jgixc?? "").GetHashCode()
                      * 23 ) + (this.Jgixf?? "").GetHashCode()
                      * 23 ) + (this.Jgixl?? "").GetHashCode()
                      * 23 ) + (this.Jgixp?? "").GetHashCode()
                      * 23 ) + (this.Jgivo.GetHashCode() ) 
                      * 23 ) + (this.Jgiva.GetHashCode() ) 
                      * 23 ) + (this.Jgivw.GetHashCode() ) 
                      * 23 ) + (this.Jggau?? "").GetHashCode()
                      * 23 ) + (this.Jggvl.GetHashCode() ) 
                      * 23 ) + (this.Jggun?? "").GetHashCode()
                      * 23 ) + (this.Jgpbn?? "").GetHashCode()
                      * 23 ) + (this.Jgpbs.GetHashCode() ) 
                      * 23 ) + (this.Jgpbr.GetHashCode() ) 
                      * 23 ) + (this.Jgpbt.GetHashCode() ) 
                      * 23 ) + (this.Jgpbc.GetHashCode() ) 
                      * 23 ) + (this.Jgpbp.GetHashCode() ) 
                      * 23 ) + (this.Jgpba.GetHashCode() ) 
                      * 23 ) + (this.Jgclv?? "").GetHashCode()
                      * 23 ) + (this.Jgagm.GetHashCode() ) 
                      * 23 ) + (this.Jgave.GetHashCode() ) 
                      * 23 ) + (this.Jgava.GetHashCode() ) 
                      * 23 ) + (this.Jgavm.GetHashCode() ) 
                      * 23 ) + (this.Jgavj.GetHashCode() ) 
                      * 23 ) + (this.Jgrul?? "").GetHashCode()
                      * 23 ) + (this.Jgrut?? "").GetHashCode()
                      * 23 ) + (this.Jgavf.GetHashCode() )                    );
           }
        }
    }
}
