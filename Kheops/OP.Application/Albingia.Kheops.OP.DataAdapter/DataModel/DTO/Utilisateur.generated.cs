using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YUTILIS
    public partial class Utilisateur  {
             //YUTILIS

            ///<summary>Public empty contructor</summary>
            public Utilisateur() {}
            ///<summary>Public empty contructor</summary>
            public Utilisateur(Utilisateur copyFrom) 
            {
                  this.Utiut= copyFrom.Utiut;
                  this.Utnom= copyFrom.Utnom;
                  this.Utpnm= copyFrom.Utpnm;
                  this.Utgrp= copyFrom.Utgrp;
                  this.Utsgr= copyFrom.Utsgr;
                  this.Uthie= copyFrom.Uthie;
                  this.Utbur= copyFrom.Utbur;
                  this.Utbau= copyFrom.Utbau;
                  this.Utbra= copyFrom.Utbra;
                  this.Utiex= copyFrom.Utiex;
                  this.Utbs1= copyFrom.Utbs1;
                  this.Utbs2= copyFrom.Utbs2;
                  this.Utbs3= copyFrom.Utbs3;
                  this.Utbs4= copyFrom.Utbs4;
                  this.Utbs5= copyFrom.Utbs5;
                  this.Uttel= copyFrom.Uttel;
                  this.Utidl= copyFrom.Utidl;
                  this.Uttpg= copyFrom.Uttpg;
                  this.Utsou= copyFrom.Utsou;
                  this.Utgep= copyFrom.Utgep;
                  this.Utges= copyFrom.Utges;
                  this.Utusr= copyFrom.Utusr;
                  this.Utmja= copyFrom.Utmja;
                  this.Utmjm= copyFrom.Utmjm;
                  this.Utmjj= copyFrom.Utmjj;
                  this.Uttit= copyFrom.Uttit;
                  this.Utini= copyFrom.Utini;
                  this.Utouq= copyFrom.Utouq;
                  this.Utfut= copyFrom.Utfut;
                  this.Utfdp= copyFrom.Utfdp;
                  this.Utusc= copyFrom.Utusc;
                  this.Utdvv= copyFrom.Utdvv;
                  this.Utdge= copyFrom.Utdge;
                  this.Utfax= copyFrom.Utfax;
                  this.Utaem= copyFrom.Utaem;
                  this.Utpfx= copyFrom.Utpfx;
                  this.Utins= copyFrom.Utins;
                  this.Utrsv= copyFrom.Utrsv;
        
            }        
            
            ///<summary> Identifiant Utilisateur </summary>
            public string Utiut { get; set; } 
            
            ///<summary> Nom </summary>
            public string Utnom { get; set; } 
            
            ///<summary> Prénom </summary>
            public string Utpnm { get; set; } 
            
            ///<summary> Code groupe </summary>
            public string Utgrp { get; set; } 
            
            ///<summary> Code sous-groupe </summary>
            public string Utsgr { get; set; } 
            
            ///<summary> Hiérarchie de Droits (0 -> 9) </summary>
            public int Uthie { get; set; } 
            
            ///<summary> Code Bureau rattachement </summary>
            public string Utbur { get; set; } 
            
            ///<summary> Top Bureaux autorisés       O/N </summary>
            public string Utbau { get; set; } 
            
            ///<summary> Branche réservée </summary>
            public string Utbra { get; set; } 
            
            ///<summary> Top Inclusion exclusion I/E  SBR </summary>
            public string Utiex { get; set; } 
            
            ///<summary> Branche/sous-branche 1 </summary>
            public string Utbs1 { get; set; } 
            
            ///<summary> Branche/sous-branche 2 </summary>
            public string Utbs2 { get; set; } 
            
            ///<summary> Branche/sous-branche 3 </summary>
            public string Utbs3 { get; set; } 
            
            ///<summary> Branche/sous-branche 4 </summary>
            public string Utbs4 { get; set; } 
            
            ///<summary> Branche/sous-branche 5 </summary>
            public string Utbs5 { get; set; } 
            
            ///<summary> Téléphone </summary>
            public string Uttel { get; set; } 
            
            ///<summary> Identifiant Délégation </summary>
            public string Utidl { get; set; } 
            
            ///<summary> Top Droits /pgm/utilisateur O/N </summary>
            public string Uttpg { get; set; } 
            
            ///<summary> Top souscripteur O/N </summary>
            public string Utsou { get; set; } 
            
            ///<summary> Top gestionnaire Production  O/N </summary>
            public string Utgep { get; set; } 
            
            ///<summary> Top gestionnaire sinistre O/N </summary>
            public string Utges { get; set; } 
            
            ///<summary> User </summary>
            public string Utusr { get; set; } 
            
            ///<summary> Année dernière maj </summary>
            public int Utmja { get; set; } 
            
            ///<summary> Mois  dernière maj </summary>
            public int Utmjm { get; set; } 
            
            ///<summary> Jour  dernière maj </summary>
            public int Utmjj { get; set; } 
            
            ///<summary> Titre </summary>
            public string Uttit { get; set; } 
            
            ///<summary> Initiales de l'utilisateur </summary>
            public string Utini { get; set; } 
            
            ///<summary> Utilisateur OUTQ </summary>
            public string Utouq { get; set; } 
            
            ///<summary> Famille utilisateur </summary>
            public string Utfut { get; set; } 
            
            ///<summary> Code de fond de page </summary>
            public string Utfdp { get; set; } 
            
            ///<summary> Unité de Service </summary>
            public string Utusc { get; set; } 
            
            ///<summary> Droit Visa/Valid sinistre </summary>
            public string Utdvv { get; set; } 
            
            ///<summary> Direction générale </summary>
            public string Utdge { get; set; } 
            
            ///<summary> UTFAX Télécopie </summary>
            public string Utfax { get; set; } 
            
            ///<summary> UTAEM Adresse e-mail </summary>
            public string Utaem { get; set; } 
            
            ///<summary> Profil Citrix </summary>
            public string Utpfx { get; set; } 
            
            ///<summary> Inspection ** Tous DL délég IN inspe </summary>
            public string Utins { get; set; } 
            
            ///<summary> Top refus sans validation O/N </summary>
            public string Utrsv { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Utilisateur  x=this,  y=obj as Utilisateur;
            if( y == default(Utilisateur) ) return false;
            return (
                    x.Utiut==y.Utiut
                    && x.Utnom==y.Utnom
                    && x.Utpnm==y.Utpnm
                    && x.Utgrp==y.Utgrp
                    && x.Utsgr==y.Utsgr
                    && x.Uthie==y.Uthie
                    && x.Utbur==y.Utbur
                    && x.Utbau==y.Utbau
                    && x.Utbra==y.Utbra
                    && x.Utiex==y.Utiex
                    && x.Utbs1==y.Utbs1
                    && x.Utbs2==y.Utbs2
                    && x.Utbs3==y.Utbs3
                    && x.Utbs4==y.Utbs4
                    && x.Utbs5==y.Utbs5
                    && x.Uttel==y.Uttel
                    && x.Utidl==y.Utidl
                    && x.Uttpg==y.Uttpg
                    && x.Utsou==y.Utsou
                    && x.Utgep==y.Utgep
                    && x.Utges==y.Utges
                    && x.Utusr==y.Utusr
                    && x.Utmja==y.Utmja
                    && x.Utmjm==y.Utmjm
                    && x.Utmjj==y.Utmjj
                    && x.Uttit==y.Uttit
                    && x.Utini==y.Utini
                    && x.Utouq==y.Utouq
                    && x.Utfut==y.Utfut
                    && x.Utfdp==y.Utfdp
                    && x.Utusc==y.Utusc
                    && x.Utdvv==y.Utdvv
                    && x.Utdge==y.Utdge
                    && x.Utfax==y.Utfax
                    && x.Utaem==y.Utaem
                    && x.Utpfx==y.Utpfx
                    && x.Utins==y.Utins
                    && x.Utrsv==y.Utrsv  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((
                       17 + (this.Utiut?? "").GetHashCode()
                      * 23 ) + (this.Utnom?? "").GetHashCode()
                      * 23 ) + (this.Utpnm?? "").GetHashCode()
                      * 23 ) + (this.Utgrp?? "").GetHashCode()
                      * 23 ) + (this.Utsgr?? "").GetHashCode()
                      * 23 ) + (this.Uthie.GetHashCode() ) 
                      * 23 ) + (this.Utbur?? "").GetHashCode()
                      * 23 ) + (this.Utbau?? "").GetHashCode()
                      * 23 ) + (this.Utbra?? "").GetHashCode()
                      * 23 ) + (this.Utiex?? "").GetHashCode()
                      * 23 ) + (this.Utbs1?? "").GetHashCode()
                      * 23 ) + (this.Utbs2?? "").GetHashCode()
                      * 23 ) + (this.Utbs3?? "").GetHashCode()
                      * 23 ) + (this.Utbs4?? "").GetHashCode()
                      * 23 ) + (this.Utbs5?? "").GetHashCode()
                      * 23 ) + (this.Uttel?? "").GetHashCode()
                      * 23 ) + (this.Utidl?? "").GetHashCode()
                      * 23 ) + (this.Uttpg?? "").GetHashCode()
                      * 23 ) + (this.Utsou?? "").GetHashCode()
                      * 23 ) + (this.Utgep?? "").GetHashCode()
                      * 23 ) + (this.Utges?? "").GetHashCode()
                      * 23 ) + (this.Utusr?? "").GetHashCode()
                      * 23 ) + (this.Utmja.GetHashCode() ) 
                      * 23 ) + (this.Utmjm.GetHashCode() ) 
                      * 23 ) + (this.Utmjj.GetHashCode() ) 
                      * 23 ) + (this.Uttit?? "").GetHashCode()
                      * 23 ) + (this.Utini?? "").GetHashCode()
                      * 23 ) + (this.Utouq?? "").GetHashCode()
                      * 23 ) + (this.Utfut?? "").GetHashCode()
                      * 23 ) + (this.Utfdp?? "").GetHashCode()
                      * 23 ) + (this.Utusc?? "").GetHashCode()
                      * 23 ) + (this.Utdvv?? "").GetHashCode()
                      * 23 ) + (this.Utdge?? "").GetHashCode()
                      * 23 ) + (this.Utfax?? "").GetHashCode()
                      * 23 ) + (this.Utaem?? "").GetHashCode()
                      * 23 ) + (this.Utpfx?? "").GetHashCode()
                      * 23 ) + (this.Utins?? "").GetHashCode()
                      * 23 ) + (this.Utrsv?? "").GetHashCode()                   );
           }
        }
    }
}
