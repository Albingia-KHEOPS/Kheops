using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YCATEGO
    public partial class Ycatego  {
             //YCATEGO

            ///<summary>Public empty contructor</summary>
            public Ycatego() {}
            ///<summary>Public empty contructor</summary>
            public Ycatego(Ycatego copyFrom) 
            {
                  this.Cabra= copyFrom.Cabra;
                  this.Casbr= copyFrom.Casbr;
                  this.Cacat= copyFrom.Cacat;
                  this.Cades= copyFrom.Cades;
                  this.Cadea= copyFrom.Cadea;
                  this.Carcg= copyFrom.Carcg;
                  this.Carcs= copyFrom.Carcs;
                  this.Caafr= copyFrom.Caafr;
                  this.Catax= copyFrom.Catax;
                  this.Cargc= copyFrom.Cargc;
                  this.Capmi= copyFrom.Capmi;
                  this.Caatt= copyFrom.Caatt;
                  this.Cacnp= copyFrom.Cacnp;
                  this.Cacnc= copyFrom.Cacnc;
                  this.Casmp= copyFrom.Casmp;
                  this.Causr= copyFrom.Causr;
                  this.Camja= copyFrom.Camja;
                  this.Camjm= copyFrom.Camjm;
                  this.Camjj= copyFrom.Camjj;
                  this.Cacnx= copyFrom.Cacnx;
                  this.Caatx= copyFrom.Caatx;
                  this.Cavat= copyFrom.Cavat;
                  this.Casai= copyFrom.Casai;
                  this.Caina= copyFrom.Caina;
                  this.Caind= copyFrom.Caind;
                  this.Caixc= copyFrom.Caixc;
                  this.Caixf= copyFrom.Caixf;
                  this.Caixl= copyFrom.Caixl;
                  this.Caixp= copyFrom.Caixp;
                  this.Caipm= copyFrom.Caipm;
                  this.Caaut= copyFrom.Caaut;
                  this.Calib= copyFrom.Calib;
                  this.Carst= copyFrom.Carst;
                  this.Caapr= copyFrom.Caapr;
                  this.Cagri= copyFrom.Cagri;
                  this.Caedi= copyFrom.Caedi;
        
            }        
            
            ///<summary> Branche </summary>
            public string Cabra { get; set; } 
            
            ///<summary> Sous-branche </summary>
            public string Casbr { get; set; } 
            
            ///<summary> Catégorie </summary>
            public string Cacat { get; set; } 
            
            ///<summary> Désignation Catégorie </summary>
            public string Cades { get; set; } 
            
            ///<summary> Désignation Abrégée </summary>
            public string Cadea { get; set; } 
            
            ///<summary> Référence Conditions générales </summary>
            public string Carcg { get; set; } 
            
            ///<summary> Référence Conventions spéciales </summary>
            public string Carcs { get; set; } 
            
            ///<summary> Montant de frais accessoires </summary>
            public int Caafr { get; set; } 
            
            ///<summary> Code taxes frais accessoires </summary>
            public string Catax { get; set; } 
            
            ///<summary> Régime commission </summary>
            public string Cargc { get; set; } 
            
            ///<summary> Montant de prime minimum </summary>
            public int Capmi { get; set; } 
            
            ///<summary> Taxe attentat O/N </summary>
            public string Caatt { get; set; } 
            
            ///<summary> % de prime pour calcul Cat Nat </summary>
            public Decimal Cacnp { get; set; } 
            
            ///<summary> Taux de commission Cat Nat </summary>
            public Decimal Cacnc { get; set; } 
            
            ///<summary> % de SMP autorisé </summary>
            public int Casmp { get; set; } 
            
            ///<summary> User </summary>
            public string Causr { get; set; } 
            
            ///<summary> Année Màj </summary>
            public int Camja { get; set; } 
            
            ///<summary> Mois  Màj </summary>
            public int Camjm { get; set; } 
            
            ///<summary> Jour  Màj </summary>
            public int Camjj { get; set; } 
            
            ///<summary> Code taxe Catastrophes Naturelles </summary>
            public string Cacnx { get; set; } 
            
            ///<summary> Code taxe attentat </summary>
            public string Caatx { get; set; } 
            
            ///<summary> Type de valeur du risque par défaut </summary>
            public string Cavat { get; set; } 
            
            ///<summary> Type de saisie Bris/TRC </summary>
            public string Casai { get; set; } 
            
            ///<summary> Indexation O/N </summary>
            public string Caina { get; set; } 
            
            ///<summary> Code indice </summary>
            public string Caind { get; set; } 
            
            ///<summary> Indexation des capitaux O/N </summary>
            public string Caixc { get; set; } 
            
            ///<summary> Indexation des franchises O/N </summary>
            public string Caixf { get; set; } 
            
            ///<summary> Indexation de LCI O/N </summary>
            public string Caixl { get; set; } 
            
            ///<summary> Indexation de prime O/N </summary>
            public string Caixp { get; set; } 
            
            ///<summary> Impression valeur prime O/N </summary>
            public string Caipm { get; set; } 
            
            ///<summary> Autorisation accès police CO/RT/** </summary>
            public string Caaut { get; set; } 
            
            ///<summary> Libellé  pour bulletin souscription </summary>
            public string Calib { get; set; } 
            
            ///<summary> Restriction  1=Offre Spal </summary>
            public string Carst { get; set; } 
            
            ///<summary> GAREAT Application O/N </summary>
            public string Caapr { get; set; } 
            
            ///<summary> Grille de saisie SMP </summary>
            public string Cagri { get; set; } 
            
            ///<summary> Corresp YYYPAR EDI/CATEG </summary>
            public string Caedi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Ycatego  x=this,  y=obj as Ycatego;
            if( y == default(Ycatego) ) return false;
            return (
                    x.Cabra==y.Cabra
                    && x.Casbr==y.Casbr
                    && x.Cacat==y.Cacat
                    && x.Cades==y.Cades
                    && x.Cadea==y.Cadea
                    && x.Carcg==y.Carcg
                    && x.Carcs==y.Carcs
                    && x.Caafr==y.Caafr
                    && x.Catax==y.Catax
                    && x.Cargc==y.Cargc
                    && x.Capmi==y.Capmi
                    && x.Caatt==y.Caatt
                    && x.Cacnp==y.Cacnp
                    && x.Cacnc==y.Cacnc
                    && x.Casmp==y.Casmp
                    && x.Causr==y.Causr
                    && x.Camja==y.Camja
                    && x.Camjm==y.Camjm
                    && x.Camjj==y.Camjj
                    && x.Cacnx==y.Cacnx
                    && x.Caatx==y.Caatx
                    && x.Cavat==y.Cavat
                    && x.Casai==y.Casai
                    && x.Caina==y.Caina
                    && x.Caind==y.Caind
                    && x.Caixc==y.Caixc
                    && x.Caixf==y.Caixf
                    && x.Caixl==y.Caixl
                    && x.Caixp==y.Caixp
                    && x.Caipm==y.Caipm
                    && x.Caaut==y.Caaut
                    && x.Calib==y.Calib
                    && x.Carst==y.Carst
                    && x.Caapr==y.Caapr
                    && x.Cagri==y.Cagri
                    && x.Caedi==y.Caedi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((
                       17 + (this.Cabra?? "").GetHashCode()
                      * 23 ) + (this.Casbr?? "").GetHashCode()
                      * 23 ) + (this.Cacat?? "").GetHashCode()
                      * 23 ) + (this.Cades?? "").GetHashCode()
                      * 23 ) + (this.Cadea?? "").GetHashCode()
                      * 23 ) + (this.Carcg?? "").GetHashCode()
                      * 23 ) + (this.Carcs?? "").GetHashCode()
                      * 23 ) + (this.Caafr.GetHashCode() ) 
                      * 23 ) + (this.Catax?? "").GetHashCode()
                      * 23 ) + (this.Cargc?? "").GetHashCode()
                      * 23 ) + (this.Capmi.GetHashCode() ) 
                      * 23 ) + (this.Caatt?? "").GetHashCode()
                      * 23 ) + (this.Cacnp.GetHashCode() ) 
                      * 23 ) + (this.Cacnc.GetHashCode() ) 
                      * 23 ) + (this.Casmp.GetHashCode() ) 
                      * 23 ) + (this.Causr?? "").GetHashCode()
                      * 23 ) + (this.Camja.GetHashCode() ) 
                      * 23 ) + (this.Camjm.GetHashCode() ) 
                      * 23 ) + (this.Camjj.GetHashCode() ) 
                      * 23 ) + (this.Cacnx?? "").GetHashCode()
                      * 23 ) + (this.Caatx?? "").GetHashCode()
                      * 23 ) + (this.Cavat?? "").GetHashCode()
                      * 23 ) + (this.Casai?? "").GetHashCode()
                      * 23 ) + (this.Caina?? "").GetHashCode()
                      * 23 ) + (this.Caind?? "").GetHashCode()
                      * 23 ) + (this.Caixc?? "").GetHashCode()
                      * 23 ) + (this.Caixf?? "").GetHashCode()
                      * 23 ) + (this.Caixl?? "").GetHashCode()
                      * 23 ) + (this.Caixp?? "").GetHashCode()
                      * 23 ) + (this.Caipm?? "").GetHashCode()
                      * 23 ) + (this.Caaut?? "").GetHashCode()
                      * 23 ) + (this.Calib?? "").GetHashCode()
                      * 23 ) + (this.Carst?? "").GetHashCode()
                      * 23 ) + (this.Caapr?? "").GetHashCode()
                      * 23 ) + (this.Cagri?? "").GetHashCode()
                      * 23 ) + (this.Caedi?? "").GetHashCode()                   );
           }
        }
    }
}
