using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YHRTRSQ
    public partial class YprtRsq  {
             //YHRTRSQ
             //YPRTRSQ

            ///<summary>Public empty contructor</summary>
            public YprtRsq() {}
            ///<summary>Public empty contructor</summary>
            public YprtRsq(YprtRsq copyFrom) 
            {
                  this.Jeipb= copyFrom.Jeipb;
                  this.Jealx= copyFrom.Jealx;
                  this.Jeavn= copyFrom.Jeavn;
                  this.Jehin= copyFrom.Jehin;
                  this.Jersq= copyFrom.Jersq;
                  this.Jecch= copyFrom.Jecch;
                  this.Jemgd= copyFrom.Jemgd;
                  this.Jebra= copyFrom.Jebra;
                  this.Jesbr= copyFrom.Jesbr;
                  this.Jecat= copyFrom.Jecat;
                  this.Jercs= copyFrom.Jercs;
                  this.Jeccs= copyFrom.Jeccs;
                  this.Jeval= copyFrom.Jeval;
                  this.Jevaa= copyFrom.Jevaa;
                  this.Jevaw= copyFrom.Jevaw;
                  this.Jevat= copyFrom.Jevat;
                  this.Jevau= copyFrom.Jevau;
                  this.Jevah= copyFrom.Jevah;
                  this.Jetem= copyFrom.Jetem;
                  this.Jevgd= copyFrom.Jevgd;
                  this.Jevgu= copyFrom.Jevgu;
                  this.Jevda= copyFrom.Jevda;
                  this.Jevdm= copyFrom.Jevdm;
                  this.Jevdj= copyFrom.Jevdj;
                  this.Jevdh= copyFrom.Jevdh;
                  this.Jevfa= copyFrom.Jevfa;
                  this.Jevfm= copyFrom.Jevfm;
                  this.Jevfj= copyFrom.Jevfj;
                  this.Jevfh= copyFrom.Jevfh;
                  this.Jeobj= copyFrom.Jeobj;
                  this.Jeroj= copyFrom.Jeroj;
                  this.Jergt= copyFrom.Jergt;
                  this.Jetrr= copyFrom.Jetrr;
                  this.Jecna= copyFrom.Jecna;
                  this.Jeina= copyFrom.Jeina;
                  this.Jeind= copyFrom.Jeind;
                  this.Jeixc= copyFrom.Jeixc;
                  this.Jeixf= copyFrom.Jeixf;
                  this.Jeixl= copyFrom.Jeixl;
                  this.Jeixp= copyFrom.Jeixp;
                  this.Jegau= copyFrom.Jegau;
                  this.Jegvl= copyFrom.Jegvl;
                  this.Jegun= copyFrom.Jegun;
                  this.Jepbn= copyFrom.Jepbn;
                  this.Jepbs= copyFrom.Jepbs;
                  this.Jepbr= copyFrom.Jepbr;
                  this.Jepbt= copyFrom.Jepbt;
                  this.Jepbc= copyFrom.Jepbc;
                  this.Jepbp= copyFrom.Jepbp;
                  this.Jepba= copyFrom.Jepba;
                  this.Jeclv= copyFrom.Jeclv;
                  this.Jeagm= copyFrom.Jeagm;
                  this.Jeipm= copyFrom.Jeipm;
                  this.Jeivx= copyFrom.Jeivx;
                  this.Jedro= copyFrom.Jedro;
                  this.Jenbo= copyFrom.Jenbo;
                  this.Jelcv= copyFrom.Jelcv;
                  this.Jelca= copyFrom.Jelca;
                  this.Jelcw= copyFrom.Jelcw;
                  this.Jelcu= copyFrom.Jelcu;
                  this.Jelce= copyFrom.Jelce;
                  this.Jeave= copyFrom.Jeave;
                  this.Jeava= copyFrom.Jeava;
                  this.Jeavm= copyFrom.Jeavm;
                  this.Jeavj= copyFrom.Jeavj;
                  this.Jerul= copyFrom.Jerul;
                  this.Jerut= copyFrom.Jerut;
                  this.Jeavf= copyFrom.Jeavf;
                  this.Jeext= copyFrom.Jeext;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Jeipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Jealx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Jeavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Jehin { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Jersq { get; set; } 
            
            ///<summary> Code chronologique Risque </summary>
            public int Jecch { get; set; } 
            
            ///<summary> Identifiant Matériel du risque </summary>
            public int Jemgd { get; set; } 
            
            ///<summary> Branche </summary>
            public string Jebra { get; set; } 
            
            ///<summary> Sous-branche </summary>
            public string Jesbr { get; set; } 
            
            ///<summary> Catégorie </summary>
            public string Jecat { get; set; } 
            
            ///<summary> Référence Conventions spéciales </summary>
            public string Jercs { get; set; } 
            
            ///<summary> Code référence CS </summary>
            public string Jeccs { get; set; } 
            
            ///<summary> Valeur du risque </summary>
            public Int64 Jeval { get; set; } 
            
            ///<summary> Valeur Actualisé du risque </summary>
            public Int64 Jevaa { get; set; } 
            
            ///<summary> W. Valeur du risque (Travail) </summary>
            public Int64 Jevaw { get; set; } 
            
            ///<summary> Type de valeur du risque </summary>
            public string Jevat { get; set; } 
            
            ///<summary> Unité de la valeur </summary>
            public string Jevau { get; set; } 
            
            ///<summary> HT ou TTC  H/T </summary>
            public string Jevah { get; set; } 
            
            ///<summary> Garantie temporaire O/N </summary>
            public string Jetem { get; set; } 
            
            ///<summary> Validité garantie : Durée </summary>
            public int Jevgd { get; set; } 
            
            ///<summary> Validité garantie : Unité </summary>
            public string Jevgu { get; set; } 
            
            ///<summary> Validité garantie : Année début </summary>
            public int Jevda { get; set; } 
            
            ///<summary> Validité garantie : Mois début </summary>
            public int Jevdm { get; set; } 
            
            ///<summary> Validité garantie : Jour début </summary>
            public int Jevdj { get; set; } 
            
            ///<summary> Validité garantie : Heure début </summary>
            public int Jevdh { get; set; } 
            
            ///<summary> Validité garantie : Année fin </summary>
            public int Jevfa { get; set; } 
            
            ///<summary> Validité garantie : Mois fin </summary>
            public int Jevfm { get; set; } 
            
            ///<summary> Validité garantie : Jour fin </summary>
            public int Jevfj { get; set; } 
            
            ///<summary> Validité garantie : Heure fin </summary>
            public int Jevfh { get; set; } 
            
            ///<summary> Id Objet si unique </summary>
            public int Jeobj { get; set; } 
            
            ///<summary> Formule par risque ou objet(O = Obj) </summary>
            public string Jeroj { get; set; } 
            
            ///<summary> Régime de taxe </summary>
            public string Jergt { get; set; } 
            
            ///<summary> Code territorialité </summary>
            public string Jetrr { get; set; } 
            
            ///<summary> Application CATNAT O/N </summary>
            public string Jecna { get; set; } 
            
            ///<summary> Indexation O/N </summary>
            public string Jeina { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public string Jeind { get; set; } 
            
            ///<summary> Indexation des capitaux O/N </summary>
            public string Jeixc { get; set; } 
            
            ///<summary> Indexation des franchises O/N </summary>
            public string Jeixf { get; set; } 
            
            ///<summary> Indexation de la LCI O/N </summary>
            public string Jeixl { get; set; } 
            
            ///<summary> Indexation des primes </summary>
            public string Jeixp { get; set; } 
            
            ///<summary> Garantie automatique O/N </summary>
            public string Jegau { get; set; } 
            
            ///<summary> Garantie automatique : Valeur limite </summary>
            public int Jegvl { get; set; } 
            
            ///<summary> Garantie automatique : Unité limite </summary>
            public string Jegun { get; set; } 
            
            ///<summary> Participation bénéficiaire O/N </summary>
            public string Jepbn { get; set; } 
            
            ///<summary> PB : Seuil de rapport S/P </summary>
            public int Jepbs { get; set; } 
            
            ///<summary> PB : Montant de ristourne en % </summary>
            public int Jepbr { get; set; } 
            
            ///<summary> PB : Taux appel de prime </summary>
            public int Jepbt { get; set; } 
            
            ///<summary> PB : Complément taux d'appel </summary>
            public int Jepbc { get; set; } 
            
            ///<summary> PB : % de prime retenue </summary>
            public int Jepbp { get; set; } 
            
            ///<summary> PB : Nombre d'années </summary>
            public int Jepba { get; set; } 
            
            ///<summary> Collision renversement O/N </summary>
            public string Jeclv { get; set; } 
            
            ///<summary> Age du matériel pour collision renv </summary>
            public int Jeagm { get; set; } 
            
            ///<summary> Impression valeur prime O/N </summary>
            public string Jeipm { get; set; } 
            
            ///<summary> Impression Inventaire en annexe O/N </summary>
            public string Jeivx { get; set; } 
            
            ///<summary> Dernier N° objet </summary>
            public int Jedro { get; set; } 
            
            ///<summary> Nombre d'objets </summary>
            public int Jenbo { get; set; } 
            
            ///<summary> LCI : Valeur </summary>
            public Int64 Jelcv { get; set; } 
            
            ///<summary> LCI : Valeur actualisée </summary>
            public Int64 Jelca { get; set; } 
            
            ///<summary> W. LCI : Valeur de travail </summary>
            public Int64 Jelcw { get; set; } 
            
            ///<summary> LCI : Unité </summary>
            public string Jelcu { get; set; } 
            
            ///<summary> Expression complexe LCI </summary>
            public string Jelce { get; set; } 
            
            ///<summary> N° avenant de création </summary>
            public int Jeave { get; set; } 
            
            ///<summary> Année Effet Avenant RSQ </summary>
            public int Jeava { get; set; } 
            
            ///<summary> Mois  Effet avenant RSQ </summary>
            public int Jeavm { get; set; } 
            
            ///<summary> Jour  Effet avenant RSQ </summary>
            public int Jeavj { get; set; } 
            
            ///<summary> A régulariser O/N </summary>
            public string Jerul { get; set; } 
            
            ///<summary> Type de régularisation (A,C,E...) </summary>
            public string Jerut { get; set; } 
            
            ///<summary> N° avenant de modification </summary>
            public int Jeavf { get; set; } 
            
            ///<summary> Extension O/N </summary>
            public string Jeext { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtRsq  x=this,  y=obj as YprtRsq;
            if( y == default(YprtRsq) ) return false;
            return (
                    x.Jeipb==y.Jeipb
                    && x.Jealx==y.Jealx
                    && x.Jersq==y.Jersq
                    && x.Jecch==y.Jecch
                    && x.Jemgd==y.Jemgd
                    && x.Jebra==y.Jebra
                    && x.Jesbr==y.Jesbr
                    && x.Jecat==y.Jecat
                    && x.Jercs==y.Jercs
                    && x.Jeccs==y.Jeccs
                    && x.Jeval==y.Jeval
                    && x.Jevaa==y.Jevaa
                    && x.Jevaw==y.Jevaw
                    && x.Jevat==y.Jevat
                    && x.Jevau==y.Jevau
                    && x.Jevah==y.Jevah
                    && x.Jetem==y.Jetem
                    && x.Jevgd==y.Jevgd
                    && x.Jevgu==y.Jevgu
                    && x.Jevda==y.Jevda
                    && x.Jevdm==y.Jevdm
                    && x.Jevdj==y.Jevdj
                    && x.Jevdh==y.Jevdh
                    && x.Jevfa==y.Jevfa
                    && x.Jevfm==y.Jevfm
                    && x.Jevfj==y.Jevfj
                    && x.Jevfh==y.Jevfh
                    && x.Jeobj==y.Jeobj
                    && x.Jeroj==y.Jeroj
                    && x.Jergt==y.Jergt
                    && x.Jetrr==y.Jetrr
                    && x.Jecna==y.Jecna
                    && x.Jeina==y.Jeina
                    && x.Jeind==y.Jeind
                    && x.Jeixc==y.Jeixc
                    && x.Jeixf==y.Jeixf
                    && x.Jeixl==y.Jeixl
                    && x.Jeixp==y.Jeixp
                    && x.Jegau==y.Jegau
                    && x.Jegvl==y.Jegvl
                    && x.Jegun==y.Jegun
                    && x.Jepbn==y.Jepbn
                    && x.Jepbs==y.Jepbs
                    && x.Jepbr==y.Jepbr
                    && x.Jepbt==y.Jepbt
                    && x.Jepbc==y.Jepbc
                    && x.Jepbp==y.Jepbp
                    && x.Jepba==y.Jepba
                    && x.Jeclv==y.Jeclv
                    && x.Jeagm==y.Jeagm
                    && x.Jeipm==y.Jeipm
                    && x.Jeivx==y.Jeivx
                    && x.Jedro==y.Jedro
                    && x.Jenbo==y.Jenbo
                    && x.Jelcv==y.Jelcv
                    && x.Jelca==y.Jelca
                    && x.Jelcw==y.Jelcw
                    && x.Jelcu==y.Jelcu
                    && x.Jelce==y.Jelce
                    && x.Jeave==y.Jeave
                    && x.Jeava==y.Jeava
                    && x.Jeavm==y.Jeavm
                    && x.Jeavj==y.Jeavj
                    && x.Jerul==y.Jerul
                    && x.Jerut==y.Jerut
                    && x.Jeavf==y.Jeavf
                    && x.Jeext==y.Jeext  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Jeipb?? "").GetHashCode()
                      * 23 ) + (this.Jealx.GetHashCode() ) 
                      * 23 ) + (this.Jersq.GetHashCode() ) 
                      * 23 ) + (this.Jecch.GetHashCode() ) 
                      * 23 ) + (this.Jemgd.GetHashCode() ) 
                      * 23 ) + (this.Jebra?? "").GetHashCode()
                      * 23 ) + (this.Jesbr?? "").GetHashCode()
                      * 23 ) + (this.Jecat?? "").GetHashCode()
                      * 23 ) + (this.Jercs?? "").GetHashCode()
                      * 23 ) + (this.Jeccs?? "").GetHashCode()
                      * 23 ) + (this.Jeval.GetHashCode() ) 
                      * 23 ) + (this.Jevaa.GetHashCode() ) 
                      * 23 ) + (this.Jevaw.GetHashCode() ) 
                      * 23 ) + (this.Jevat?? "").GetHashCode()
                      * 23 ) + (this.Jevau?? "").GetHashCode()
                      * 23 ) + (this.Jevah?? "").GetHashCode()
                      * 23 ) + (this.Jetem?? "").GetHashCode()
                      * 23 ) + (this.Jevgd.GetHashCode() ) 
                      * 23 ) + (this.Jevgu?? "").GetHashCode()
                      * 23 ) + (this.Jevda.GetHashCode() ) 
                      * 23 ) + (this.Jevdm.GetHashCode() ) 
                      * 23 ) + (this.Jevdj.GetHashCode() ) 
                      * 23 ) + (this.Jevdh.GetHashCode() ) 
                      * 23 ) + (this.Jevfa.GetHashCode() ) 
                      * 23 ) + (this.Jevfm.GetHashCode() ) 
                      * 23 ) + (this.Jevfj.GetHashCode() ) 
                      * 23 ) + (this.Jevfh.GetHashCode() ) 
                      * 23 ) + (this.Jeobj.GetHashCode() ) 
                      * 23 ) + (this.Jeroj?? "").GetHashCode()
                      * 23 ) + (this.Jergt?? "").GetHashCode()
                      * 23 ) + (this.Jetrr?? "").GetHashCode()
                      * 23 ) + (this.Jecna?? "").GetHashCode()
                      * 23 ) + (this.Jeina?? "").GetHashCode()
                      * 23 ) + (this.Jeind?? "").GetHashCode()
                      * 23 ) + (this.Jeixc?? "").GetHashCode()
                      * 23 ) + (this.Jeixf?? "").GetHashCode()
                      * 23 ) + (this.Jeixl?? "").GetHashCode()
                      * 23 ) + (this.Jeixp?? "").GetHashCode()
                      * 23 ) + (this.Jegau?? "").GetHashCode()
                      * 23 ) + (this.Jegvl.GetHashCode() ) 
                      * 23 ) + (this.Jegun?? "").GetHashCode()
                      * 23 ) + (this.Jepbn?? "").GetHashCode()
                      * 23 ) + (this.Jepbs.GetHashCode() ) 
                      * 23 ) + (this.Jepbr.GetHashCode() ) 
                      * 23 ) + (this.Jepbt.GetHashCode() ) 
                      * 23 ) + (this.Jepbc.GetHashCode() ) 
                      * 23 ) + (this.Jepbp.GetHashCode() ) 
                      * 23 ) + (this.Jepba.GetHashCode() ) 
                      * 23 ) + (this.Jeclv?? "").GetHashCode()
                      * 23 ) + (this.Jeagm.GetHashCode() ) 
                      * 23 ) + (this.Jeipm?? "").GetHashCode()
                      * 23 ) + (this.Jeivx?? "").GetHashCode()
                      * 23 ) + (this.Jedro.GetHashCode() ) 
                      * 23 ) + (this.Jenbo.GetHashCode() ) 
                      * 23 ) + (this.Jelcv.GetHashCode() ) 
                      * 23 ) + (this.Jelca.GetHashCode() ) 
                      * 23 ) + (this.Jelcw.GetHashCode() ) 
                      * 23 ) + (this.Jelcu?? "").GetHashCode()
                      * 23 ) + (this.Jelce?? "").GetHashCode()
                      * 23 ) + (this.Jeave.GetHashCode() ) 
                      * 23 ) + (this.Jeava.GetHashCode() ) 
                      * 23 ) + (this.Jeavm.GetHashCode() ) 
                      * 23 ) + (this.Jeavj.GetHashCode() ) 
                      * 23 ) + (this.Jerul?? "").GetHashCode()
                      * 23 ) + (this.Jerut?? "").GetHashCode()
                      * 23 ) + (this.Jeavf.GetHashCode() ) 
                      * 23 ) + (this.Jeext?? "").GetHashCode()                   );
           }
        }
    }
}
