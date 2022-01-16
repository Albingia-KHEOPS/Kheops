using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPIRSGA
    public partial class KpIrSGa  {
             //HPIRSGA
             //KPIRSGA

            ///<summary>Public empty contructor</summary>
            public KpIrSGa() {}
            ///<summary>Public empty contructor</summary>
            public KpIrSGa(KpIrSGa copyFrom) 
            {
                  this.Kfdtyp= copyFrom.Kfdtyp;
                  this.Kfdipb= copyFrom.Kfdipb;
                  this.Kfdalx= copyFrom.Kfdalx;
                  this.Kfdavn= copyFrom.Kfdavn;
                  this.Kfdhin= copyFrom.Kfdhin;
                  this.Kfdfor= copyFrom.Kfdfor;
                  this.Kfdopt= copyFrom.Kfdopt;
                  this.Kfdcrd= copyFrom.Kfdcrd;
                  this.Kfdcrh= copyFrom.Kfdcrh;
                  this.Kfdmaju= copyFrom.Kfdmaju;
                  this.Kfdmajd= copyFrom.Kfdmajd;
                  this.Kfdmajh= copyFrom.Kfdmajh;
                  this.Kfdan02= copyFrom.Kfdan02;
                  this.Kfdan03= copyFrom.Kfdan03;
                  this.Kfdbo01= copyFrom.Kfdbo01;
                  this.Kfdbo02= copyFrom.Kfdbo02;
                  this.Kfdbo03= copyFrom.Kfdbo03;
                  this.Kfdim08= copyFrom.Kfdim08;
                  this.Kfdim09= copyFrom.Kfdim09;
                  this.Kfdim10= copyFrom.Kfdim10;
                  this.Kfdnbgr= copyFrom.Kfdnbgr;
                  this.Kfdeffv= copyFrom.Kfdeffv;
                  this.Kfdcnvd= copyFrom.Kfdcnvd;
                  this.Kfdfrdm= copyFrom.Kfdfrdm;
                  this.Kfdsorn= copyFrom.Kfdsorn;
                  this.Kfdsord= copyFrom.Kfdsord;
                  this.Kfdsorr= copyFrom.Kfdsorr;
                  this.Kfd05= copyFrom.Kfd05;
                  this.Kfd06= copyFrom.Kfd06;
                  this.Kfd07= copyFrom.Kfd07;
                  this.Kfd08= copyFrom.Kfd08;
                  this.Kfd09= copyFrom.Kfd09;
                  this.Kfdia01= copyFrom.Kfdia01;
                  this.Kfdia02= copyFrom.Kfdia02;
                  this.Kfdia03= copyFrom.Kfdia03;
                  this.Kfdiara17= copyFrom.Kfdiara17;
                  this.Kfdrsat= copyFrom.Kfdrsat;
                  this.Kfdrcps= copyFrom.Kfdrcps;
                  this.Kfdrcpf= copyFrom.Kfdrcpf;
                  this.Kfdrcpd= copyFrom.Kfdrcpd;
                  this.Kfdrasb= copyFrom.Kfdrasb;
                  this.Kfdrasl= copyFrom.Kfdrasl;
                  this.Kfdrass= copyFrom.Kfdrass;
                  this.Kfdcotnb= copyFrom.Kfdcotnb;
                  this.Kfdcotmt= copyFrom.Kfdcotmt;
                  this.Kfdmnt= copyFrom.Kfdmnt;
                  this.Kfdmntnb= copyFrom.Kfdmntnb;
                  this.Kfdmntmt= copyFrom.Kfdmntmt;
                  this.Kfdrgu= copyFrom.Kfdrgu;
                  this.Kfdnbji= copyFrom.Kfdnbji;
                  this.Kfdan04= copyFrom.Kfdan04;
                  this.Kfdgarav= copyFrom.Kfdgarav;
                  this.Kfdvolav= copyFrom.Kfdvolav;
                  this.Kfdvolap= copyFrom.Kfdvolap;
                  this.Kfdlma= copyFrom.Kfdlma;
                  this.Kfdmxm= copyFrom.Kfdmxm;
                  this.Kfdray= copyFrom.Kfdray;
                  this.Kfdan05= copyFrom.Kfdan05;
                  this.Kfdray5= copyFrom.Kfdray5;
                  this.Kfdan06= copyFrom.Kfdan06;
                  this.Kfdan07= copyFrom.Kfdan07;
                  this.Kfdclal= copyFrom.Kfdclal;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kfdtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kfdipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kfdalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Kfdavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int Kfdhin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kfdfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kfdopt { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kfdcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kfdcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kfdmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kfdmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kfdmajh { get; set; } 
            
            ///<summary> ANNU Attentat Durée  45 Jours </summary>
            public string Kfdan02 { get; set; } 
            
            ///<summary> ANNU Attentat Autre durée (jour) </summary>
            public int Kfdan03 { get; set; } 
            
            ///<summary> BONI Bonification si non sinistre </summary>
            public string Kfdbo01 { get; set; } 
            
            ///<summary> BONI % de bonification </summary>
            public Decimal Kfdbo02 { get; set; } 
            
            ///<summary> BONI Bonification anticipée </summary>
            public string Kfdbo03 { get; set; } 
            
            ///<summary> INTEMP Code Fin de couvertture </summary>
            public string Kfdim08 { get; set; } 
            
            ///<summary> INTEMP N° Acte Fin de couverture </summary>
            public int Kfdim09 { get; set; } 
            
            ///<summary> INTEMP texte Fin de Couverture (heur </summary>
            public string Kfdim10 { get; set; } 
            
            ///<summary> Nb garanti de représentations </summary>
            public int Kfdnbgr { get; set; } 
            
            ///<summary> Prise effet Vol avant manif Nb Heur </summary>
            public Decimal Kfdeffv { get; set; } 
            
            ///<summary> Code Convention : Dépréc/Val Agréée </summary>
            public string Kfdcnvd { get; set; } 
            
            ///<summary> Frais Supp retour diff: budget max% </summary>
            public Decimal Kfdfrdm { get; set; } 
            
            ///<summary> Cl sortie - nb émissions </summary>
            public int Kfdsorn { get; set; } 
            
            ///<summary> Cl sortie - date lim </summary>
            public int Kfdsord { get; set; } 
            
            ///<summary> Cl sortie - ristourne </summary>
            public Decimal Kfdsorr { get; set; } 
            
            ///<summary> Barèmes (IPT) </summary>
            public string Kfd05 { get; set; } 
            
            ///<summary> Classe de risque </summary>
            public string Kfd06 { get; set; } 
            
            ///<summary> KPDESI Maladies exclues (CP) </summary>
            public Int64 Kfd07 { get; set; } 
            
            ///<summary> Champ application garanties (CP) </summary>
            public string Kfd08 { get; set; } 
            
            ///<summary> KPDESI Autres champs d'application </summary>
            public Int64 Kfd09 { get; set; } 
            
            ///<summary> Dérogation </summary>
            public string Kfdia01 { get; set; } 
            
            ///<summary> Age de dérogation </summary>
            public int Kfdia02 { get; set; } 
            
            ///<summary> ITT rachat franchise Accid./Maladie </summary>
            public string Kfdia03 { get; set; } 
            
            ///<summary> Taux d'infirmité pré existante </summary>
            public int Kfdiara17 { get; set; } 
            
            ///<summary> Taux régularisation sur salaires </summary>
            public Decimal Kfdrsat { get; set; } 
            
            ///<summary> Cotis prov. avant régul sur salaires </summary>
            public Decimal Kfdrcps { get; set; } 
            
            ///<summary> Cotis provi avant régul sur flotte </summary>
            public Decimal Kfdrcpf { get; set; } 
            
            ///<summary> Cotis. prov. avant declaration d'ass </summary>
            public Decimal Kfdrcpd { get; set; } 
            
            ///<summary> Base : nombre d'assurés </summary>
            public Int64 Kfdrasb { get; set; } 
            
            ///<summary> Nbre d'ass plancher pour le calcul </summary>
            public Int64 Kfdrasl { get; set; } 
            
            ///<summary> Cotisation supplémentaire par assuré </summary>
            public Decimal Kfdrass { get; set; } 
            
            ///<summary> Nombre de biens immobilliers </summary>
            public int Kfdcotnb { get; set; } 
            
            ///<summary> Montant cotisation </summary>
            public Decimal Kfdcotmt { get; set; } 
            
            ///<summary> Montants revente </summary>
            public string Kfdmnt { get; set; } 
            
            ///<summary> % du prix d'achat (CP) </summary>
            public Decimal Kfdmntnb { get; set; } 
            
            ///<summary> Limite par transaction (CP) </summary>
            public Decimal Kfdmntmt { get; set; } 
            
            ///<summary> Type de régularisation </summary>
            public string Kfdrgu { get; set; } 
            
            ///<summary> Nombre de jours Sites inaccessibles </summary>
            public int Kfdnbji { get; set; } 
            
            ///<summary> ANNU Attentat Durée  10 Jours </summary>
            public string Kfdan04 { get; set; } 
            
            ///<summary> Prise Effet Garantie AV  TEXTE </summary>
            public string Kfdgarav { get; set; } 
            
            ///<summary> VOL Prise Effet avant manif </summary>
            public string Kfdvolav { get; set; } 
            
            ///<summary> VOL Prise Effet avant manif </summary>
            public string Kfdvolap { get; set; } 
            
            ///<summary> IA:Report lim âge </summary>
            public int Kfdlma { get; set; } 
            
            ///<summary> RC:Val max m² </summary>
            public Decimal Kfdmxm { get; set; } 
            
            ///<summary> Rayon de couverture (km) </summary>
            public int Kfdray { get; set; } 
            
            ///<summary> ANNU attent- retrait autor dél </summary>
            public int Kfdan05 { get; set; } 
            
            ///<summary> ANNU attent- retrait autor rayon </summary>
            public int Kfdray5 { get; set; } 
            
            ///<summary> ANNU attent- retrait si menace dél </summary>
            public int Kfdan06 { get; set; } 
            
            ///<summary> ANNU Attentat Autre durée obligatoir </summary>
            public int Kfdan07 { get; set; } 
            
            ///<summary> Clause libre (accord DT) </summary>
            public string Kfdclal { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpIrSGa  x=this,  y=obj as KpIrSGa;
            if( y == default(KpIrSGa) ) return false;
            return (
                    x.Kfdtyp==y.Kfdtyp
                    && x.Kfdipb==y.Kfdipb
                    && x.Kfdalx==y.Kfdalx
                    && x.Kfdavn==y.Kfdavn
                    && x.Kfdhin==y.Kfdhin
                    && x.Kfdfor==y.Kfdfor
                    && x.Kfdopt==y.Kfdopt
                    && x.Kfdcrd==y.Kfdcrd
                    && x.Kfdcrh==y.Kfdcrh
                    && x.Kfdmaju==y.Kfdmaju
                    && x.Kfdmajd==y.Kfdmajd
                    && x.Kfdmajh==y.Kfdmajh
                    && x.Kfdan02==y.Kfdan02
                    && x.Kfdan03==y.Kfdan03
                    && x.Kfdbo01==y.Kfdbo01
                    && x.Kfdbo02==y.Kfdbo02
                    && x.Kfdbo03==y.Kfdbo03
                    && x.Kfdim08==y.Kfdim08
                    && x.Kfdim09==y.Kfdim09
                    && x.Kfdim10==y.Kfdim10
                    && x.Kfdnbgr==y.Kfdnbgr
                    && x.Kfdeffv==y.Kfdeffv
                    && x.Kfdcnvd==y.Kfdcnvd
                    && x.Kfdfrdm==y.Kfdfrdm
                    && x.Kfdsorn==y.Kfdsorn
                    && x.Kfdsord==y.Kfdsord
                    && x.Kfdsorr==y.Kfdsorr
                    && x.Kfd05==y.Kfd05
                    && x.Kfd06==y.Kfd06
                    && x.Kfd07==y.Kfd07
                    && x.Kfd08==y.Kfd08
                    && x.Kfd09==y.Kfd09
                    && x.Kfdia01==y.Kfdia01
                    && x.Kfdia02==y.Kfdia02
                    && x.Kfdia03==y.Kfdia03
                    && x.Kfdiara17==y.Kfdiara17
                    && x.Kfdrsat==y.Kfdrsat
                    && x.Kfdrcps==y.Kfdrcps
                    && x.Kfdrcpf==y.Kfdrcpf
                    && x.Kfdrcpd==y.Kfdrcpd
                    && x.Kfdrasb==y.Kfdrasb
                    && x.Kfdrasl==y.Kfdrasl
                    && x.Kfdrass==y.Kfdrass
                    && x.Kfdcotnb==y.Kfdcotnb
                    && x.Kfdcotmt==y.Kfdcotmt
                    && x.Kfdmnt==y.Kfdmnt
                    && x.Kfdmntnb==y.Kfdmntnb
                    && x.Kfdmntmt==y.Kfdmntmt
                    && x.Kfdrgu==y.Kfdrgu
                    && x.Kfdnbji==y.Kfdnbji
                    && x.Kfdan04==y.Kfdan04
                    && x.Kfdgarav==y.Kfdgarav
                    && x.Kfdvolav==y.Kfdvolav
                    && x.Kfdvolap==y.Kfdvolap
                    && x.Kfdlma==y.Kfdlma
                    && x.Kfdmxm==y.Kfdmxm
                    && x.Kfdray==y.Kfdray
                    && x.Kfdan05==y.Kfdan05
                    && x.Kfdray5==y.Kfdray5
                    && x.Kfdan06==y.Kfdan06
                    && x.Kfdan07==y.Kfdan07
                    && x.Kfdclal==y.Kfdclal  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kfdtyp?? "").GetHashCode()
                      * 23 ) + (this.Kfdipb?? "").GetHashCode()
                      * 23 ) + (this.Kfdalx.GetHashCode() ) 
                      * 23 ) + (this.Kfdavn.GetHashCode() ) 
                      * 23 ) + (this.Kfdhin.GetHashCode() ) 
                      * 23 ) + (this.Kfdfor.GetHashCode() ) 
                      * 23 ) + (this.Kfdopt.GetHashCode() ) 
                      * 23 ) + (this.Kfdcrd.GetHashCode() ) 
                      * 23 ) + (this.Kfdcrh.GetHashCode() ) 
                      * 23 ) + (this.Kfdmaju?? "").GetHashCode()
                      * 23 ) + (this.Kfdmajd.GetHashCode() ) 
                      * 23 ) + (this.Kfdmajh.GetHashCode() ) 
                      * 23 ) + (this.Kfdan02?? "").GetHashCode()
                      * 23 ) + (this.Kfdan03.GetHashCode() ) 
                      * 23 ) + (this.Kfdbo01?? "").GetHashCode()
                      * 23 ) + (this.Kfdbo02.GetHashCode() ) 
                      * 23 ) + (this.Kfdbo03?? "").GetHashCode()
                      * 23 ) + (this.Kfdim08?? "").GetHashCode()
                      * 23 ) + (this.Kfdim09.GetHashCode() ) 
                      * 23 ) + (this.Kfdim10?? "").GetHashCode()
                      * 23 ) + (this.Kfdnbgr.GetHashCode() ) 
                      * 23 ) + (this.Kfdeffv.GetHashCode() ) 
                      * 23 ) + (this.Kfdcnvd?? "").GetHashCode()
                      * 23 ) + (this.Kfdfrdm.GetHashCode() ) 
                      * 23 ) + (this.Kfdsorn.GetHashCode() ) 
                      * 23 ) + (this.Kfdsord.GetHashCode() ) 
                      * 23 ) + (this.Kfdsorr.GetHashCode() ) 
                      * 23 ) + (this.Kfd05?? "").GetHashCode()
                      * 23 ) + (this.Kfd06?? "").GetHashCode()
                      * 23 ) + (this.Kfd07.GetHashCode() ) 
                      * 23 ) + (this.Kfd08?? "").GetHashCode()
                      * 23 ) + (this.Kfd09.GetHashCode() ) 
                      * 23 ) + (this.Kfdia01?? "").GetHashCode()
                      * 23 ) + (this.Kfdia02.GetHashCode() ) 
                      * 23 ) + (this.Kfdia03?? "").GetHashCode()
                      * 23 ) + (this.Kfdiara17.GetHashCode() ) 
                      * 23 ) + (this.Kfdrsat.GetHashCode() ) 
                      * 23 ) + (this.Kfdrcps.GetHashCode() ) 
                      * 23 ) + (this.Kfdrcpf.GetHashCode() ) 
                      * 23 ) + (this.Kfdrcpd.GetHashCode() ) 
                      * 23 ) + (this.Kfdrasb.GetHashCode() ) 
                      * 23 ) + (this.Kfdrasl.GetHashCode() ) 
                      * 23 ) + (this.Kfdrass.GetHashCode() ) 
                      * 23 ) + (this.Kfdcotnb.GetHashCode() ) 
                      * 23 ) + (this.Kfdcotmt.GetHashCode() ) 
                      * 23 ) + (this.Kfdmnt?? "").GetHashCode()
                      * 23 ) + (this.Kfdmntnb.GetHashCode() ) 
                      * 23 ) + (this.Kfdmntmt.GetHashCode() ) 
                      * 23 ) + (this.Kfdrgu?? "").GetHashCode()
                      * 23 ) + (this.Kfdnbji.GetHashCode() ) 
                      * 23 ) + (this.Kfdan04?? "").GetHashCode()
                      * 23 ) + (this.Kfdgarav?? "").GetHashCode()
                      * 23 ) + (this.Kfdvolav?? "").GetHashCode()
                      * 23 ) + (this.Kfdvolap?? "").GetHashCode()
                      * 23 ) + (this.Kfdlma.GetHashCode() ) 
                      * 23 ) + (this.Kfdmxm.GetHashCode() ) 
                      * 23 ) + (this.Kfdray.GetHashCode() ) 
                      * 23 ) + (this.Kfdan05.GetHashCode() ) 
                      * 23 ) + (this.Kfdray5.GetHashCode() ) 
                      * 23 ) + (this.Kfdan06.GetHashCode() ) 
                      * 23 ) + (this.Kfdan07.GetHashCode() ) 
                      * 23 ) + (this.Kfdclal?? "").GetHashCode()                   );
           }
        }
    }
}
