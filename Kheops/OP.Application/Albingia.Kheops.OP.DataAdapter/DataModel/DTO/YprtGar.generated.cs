using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTGAR
    public partial class YprtGar  {
             //YHRTGAR
             //YPRTGAR

            ///<summary>Public empty contructor</summary>
            public YprtGar() {}
            ///<summary>Public empty contructor</summary>
            public YprtGar(YprtGar copyFrom) 
            {
                  this.Jhipb= copyFrom.Jhipb;
                  this.Jhalx= copyFrom.Jhalx;
                  this.Jhavn= copyFrom.Jhavn;
                  this.Jhhin= copyFrom.Jhhin;
                  this.Jhrsq= copyFrom.Jhrsq;
                  this.Jhfor= copyFrom.Jhfor;
                  this.Jhgap= copyFrom.Jhgap;
                  this.Jhgar= copyFrom.Jhgar;
                  this.Jhcar= copyFrom.Jhcar;
                  this.Jhnat= copyFrom.Jhnat;
                  this.Jhgan= copyFrom.Jhgan;
                  this.Jhobn= copyFrom.Jhobn;
                  this.Jhob1= copyFrom.Jhob1;
                  this.Jhob2= copyFrom.Jhob2;
                  this.Jhob3= copyFrom.Jhob3;
                  this.Jhob4= copyFrom.Jhob4;
                  this.Jhob5= copyFrom.Jhob5;
                  this.Jhgav= copyFrom.Jhgav;
                  this.Jhrqv= copyFrom.Jhrqv;
                  this.Jhfov= copyFrom.Jhfov;
                  this.Jhrep= copyFrom.Jhrep;
                  this.Jhtax= copyFrom.Jhtax;
                  this.Jhlcv= copyFrom.Jhlcv;
                  this.Jhlca= copyFrom.Jhlca;
                  this.Jhlcw= copyFrom.Jhlcw;
                  this.Jhlcu= copyFrom.Jhlcu;
                  this.Jhlce= copyFrom.Jhlce;
                  this.Jhlc1= copyFrom.Jhlc1;
                  this.Jhlc2= copyFrom.Jhlc2;
                  this.Jhlc3= copyFrom.Jhlc3;
                  this.Jhlc4= copyFrom.Jhlc4;
                  this.Jhlc5= copyFrom.Jhlc5;
                  this.Jhlov= copyFrom.Jhlov;
                  this.Jhloa= copyFrom.Jhloa;
                  this.Jhlow= copyFrom.Jhlow;
                  this.Jhlou= copyFrom.Jhlou;
                  this.Jhloe= copyFrom.Jhloe;
                  this.Jhasv= copyFrom.Jhasv;
                  this.Jhasa= copyFrom.Jhasa;
                  this.Jhasw= copyFrom.Jhasw;
                  this.Jhasu= copyFrom.Jhasu;
                  this.Jhasn= copyFrom.Jhasn;
                  this.Jhfhv= copyFrom.Jhfhv;
                  this.Jhfha= copyFrom.Jhfha;
                  this.Jhfhw= copyFrom.Jhfhw;
                  this.Jhfhu= copyFrom.Jhfhu;
                  this.Jhfhe= copyFrom.Jhfhe;
                  this.Jhprv= copyFrom.Jhprv;
                  this.Jhpra= copyFrom.Jhpra;
                  this.Jhprw= copyFrom.Jhprw;
                  this.Jhpru= copyFrom.Jhpru;
                  this.Jhprp= copyFrom.Jhprp;
                  this.Jhpre= copyFrom.Jhpre;
                  this.Jhprf= copyFrom.Jhprf;
                  this.Jhcna= copyFrom.Jhcna;
                  this.Jhina= copyFrom.Jhina;
                  this.Jhefa= copyFrom.Jhefa;
                  this.Jhefm= copyFrom.Jhefm;
                  this.Jhefj= copyFrom.Jhefj;
                  this.Jhefh= copyFrom.Jhefh;
                  this.Jhfea= copyFrom.Jhfea;
                  this.Jhfem= copyFrom.Jhfem;
                  this.Jhfej= copyFrom.Jhfej;
                  this.Jhfeh= copyFrom.Jhfeh;
                  this.Jhefd= copyFrom.Jhefd;
                  this.Jhefu= copyFrom.Jhefu;
                  this.Jhtmc= copyFrom.Jhtmc;
                  this.Jhtff= copyFrom.Jhtff;
                  this.Jhcmc= copyFrom.Jhcmc;
                  this.Jhcht= copyFrom.Jhcht;
                  this.Jhctt= copyFrom.Jhctt;
                  this.Jhajt= copyFrom.Jhajt;
                  this.Jhdfg= copyFrom.Jhdfg;
                  this.Jhifc= copyFrom.Jhifc;
                  this.Jhcda= copyFrom.Jhcda;
                  this.Jhcdm= copyFrom.Jhcdm;
                  this.Jhcdj= copyFrom.Jhcdj;
                  this.Jhcfa= copyFrom.Jhcfa;
                  this.Jhcfm= copyFrom.Jhcfm;
                  this.Jhcfj= copyFrom.Jhcfj;
                  this.Jhave= copyFrom.Jhave;
                  this.Jhavf= copyFrom.Jhavf;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Jhipb { get; set; } 
            
            ///<summary> N° Aliment ou Connexe </summary>
            public int Jhalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Jhavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Jhhin { get; set; } 
            
            ///<summary> Identifiant Risque </summary>
            public int Jhrsq { get; set; } 
            
            ///<summary> Identifiant formule </summary>
            public int Jhfor { get; set; } 
            
            ///<summary> N° ordre présentation </summary>
            public int Jhgap { get; set; } 
            
            ///<summary> Code garantie </summary>
            public string Jhgar { get; set; } 
            
            ///<summary> Caractère de la garantie </summary>
            public string Jhcar { get; set; } 
            
            ///<summary> Nature provenant de la catégorie </summary>
            public string Jhnat { get; set; } 
            
            ///<summary> Nature de la garantie </summary>
            public string Jhgan { get; set; } 
            
            ///<summary> Nature spécifique pour objets </summary>
            public string Jhobn { get; set; } 
            
            ///<summary> Identifiant objet 1 </summary>
            public int Jhob1 { get; set; } 
            
            ///<summary> Identifiant objet 2 </summary>
            public int Jhob2 { get; set; } 
            
            ///<summary> Identifiant objet 3 </summary>
            public int Jhob3 { get; set; } 
            
            ///<summary> Identifiant objet 4 </summary>
            public int Jhob4 { get; set; } 
            
            ///<summary> Identifiant objet 5 </summary>
            public int Jhob5 { get; set; } 
            
            ///<summary> Garantie commune en renvoi (R / ' ') </summary>
            public string Jhgav { get; set; } 
            
            ///<summary> Id Risque si renvoi </summary>
            public int Jhrqv { get; set; } 
            
            ///<summary> Id Formule si renvoi </summary>
            public int Jhfov { get; set; } 
            
            ///<summary> % de répartition pour taxes </summary>
            public int Jhrep { get; set; } 
            
            ///<summary> Code taxe </summary>
            public string Jhtax { get; set; } 
            
            ///<summary> LCI : Valeur </summary>
            public Int64 Jhlcv { get; set; } 
            
            ///<summary> LCI : Valeur actualisée </summary>
            public Int64 Jhlca { get; set; } 
            
            ///<summary> W. LCI : Valeur de travail </summary>
            public Int64 Jhlcw { get; set; } 
            
            ///<summary> LCI : Unité </summary>
            public string Jhlcu { get; set; } 
            
            ///<summary> Expression complexe LCI </summary>
            public string Jhlce { get; set; } 
            
            ///<summary> LCI : Id objet 1 </summary>
            public int Jhlc1 { get; set; } 
            
            ///<summary> LCI : Id objet 2 </summary>
            public int Jhlc2 { get; set; } 
            
            ///<summary> LCI : Id objet 3 </summary>
            public int Jhlc3 { get; set; } 
            
            ///<summary> LCI : Id objet 4 </summary>
            public int Jhlc4 { get; set; } 
            
            ///<summary> LCI : Id objet 5 </summary>
            public int Jhlc5 { get; set; } 
            
            ///<summary> Spécif objet LCI : Valeur </summary>
            public Int64 Jhlov { get; set; } 
            
            ///<summary> Spécif Objet LCI : Valeur actualisée </summary>
            public Int64 Jhloa { get; set; } 
            
            ///<summary> W. Spécif objet LCI : Valeur travail </summary>
            public Int64 Jhlow { get; set; } 
            
            ///<summary> Spécif objet LCI : Unité </summary>
            public string Jhlou { get; set; } 
            
            ///<summary> Spécif objet LCI : Exp complexe </summary>
            public string Jhloe { get; set; } 
            
            ///<summary> Valeur de l'assiette Origine </summary>
            public Int64 Jhasv { get; set; } 
            
            ///<summary> Valeur de l'assiette Actualisé </summary>
            public Int64 Jhasa { get; set; } 
            
            ///<summary> W. Valeur de l'assiette (travail) </summary>
            public Int64 Jhasw { get; set; } 
            
            ///<summary> Assiette : Unité </summary>
            public string Jhasu { get; set; } 
            
            ///<summary> Nature de l'assiette </summary>
            public string Jhasn { get; set; } 
            
            ///<summary> Valeur de la franchise </summary>
            public int Jhfhv { get; set; } 
            
            ///<summary> Valeur de la franchise actualisée </summary>
            public int Jhfha { get; set; } 
            
            ///<summary> W. Valeur de la franchise (travail) </summary>
            public int Jhfhw { get; set; } 
            
            ///<summary> Unité de la franchise </summary>
            public string Jhfhu { get; set; } 
            
            ///<summary> Expression complexe Franchise </summary>
            public string Jhfhe { get; set; } 
            
            ///<summary> Valeur prime Origine </summary>
            public Decimal Jhprv { get; set; } 
            
            ///<summary> Valeur prime Actualisée </summary>
            public Decimal Jhpra { get; set; } 
            
            ///<summary> W. Valeur prime (travail) </summary>
            public Decimal Jhprw { get; set; } 
            
            ///<summary> Unité prime </summary>
            public string Jhpru { get; set; } 
            
            ///<summary> Type applicat.prime </summary>
            public string Jhprp { get; set; } 
            
            ///<summary> Type d'émission (Comptant Prorata) </summary>
            public string Jhpre { get; set; } 
            
            ///<summary> Dans montant de référence O/N </summary>
            public string Jhprf { get; set; } 
            
            ///<summary> Application CATNAT O/N </summary>
            public string Jhcna { get; set; } 
            
            ///<summary> Indexation O/N </summary>
            public string Jhina { get; set; } 
            
            ///<summary> Effet garantie : Année </summary>
            public int Jhefa { get; set; } 
            
            ///<summary> Effet garantie : Mois </summary>
            public int Jhefm { get; set; } 
            
            ///<summary> Effet garantie : Jour </summary>
            public int Jhefj { get; set; } 
            
            ///<summary> Effet garantie : Heure </summary>
            public int Jhefh { get; set; } 
            
            ///<summary> Fin effet garantie : Année </summary>
            public int Jhfea { get; set; } 
            
            ///<summary> Fin effet garantie : Mois </summary>
            public int Jhfem { get; set; } 
            
            ///<summary> Fin effet garantie : Jour </summary>
            public int Jhfej { get; set; } 
            
            ///<summary> Fin effet garantie : Heure </summary>
            public int Jhfeh { get; set; } 
            
            ///<summary> Effet garantie : Durée </summary>
            public int Jhefd { get; set; } 
            
            ///<summary> Effet garantie : Unité </summary>
            public string Jhefu { get; set; } 
            
            ///<summary> Total : Montant calculé </summary>
            public Decimal Jhtmc { get; set; } 
            
            ///<summary> Total : Montant forcé </summary>
            public Decimal Jhtff { get; set; } 
            
            ///<summary> Comptant : Montant calculé </summary>
            public Decimal Jhcmc { get; set; } 
            
            ///<summary> Comptant : Montant forcé HT </summary>
            public Decimal Jhcht { get; set; } 
            
            ///<summary> Comptant : Montant forcé TTC </summary>
            public Decimal Jhctt { get; set; } 
            
            ///<summary> Garantie ajoutée O/N </summary>
            public string Jhajt { get; set; } 
            
            ///<summary> Définition garantie (Maintenance ..) </summary>
            public string Jhdfg { get; set; } 
            
            ///<summary> Info complémentaire (Maintenance ..) </summary>
            public string Jhifc { get; set; } 
            
            ///<summary> W. Effet calculée : Année </summary>
            public int Jhcda { get; set; } 
            
            ///<summary> W. Effet calculée : Mois </summary>
            public int Jhcdm { get; set; } 
            
            ///<summary> W. Effet calculée : Jour </summary>
            public int Jhcdj { get; set; } 
            
            ///<summary> W. Fin effet calculée : Année </summary>
            public int Jhcfa { get; set; } 
            
            ///<summary> W. Fin effet calculée : Mois </summary>
            public int Jhcfm { get; set; } 
            
            ///<summary> W. Fin effet calculée : Jour </summary>
            public int Jhcfj { get; set; } 
            
            ///<summary> N° avenant de création </summary>
            public int Jhave { get; set; } 
            
            ///<summary> N° avenant de modification </summary>
            public int Jhavf { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtGar  x=this,  y=obj as YprtGar;
            if( y == default(YprtGar) ) return false;
            return (
                    x.Jhipb==y.Jhipb
                    && x.Jhalx==y.Jhalx
                    && x.Jhrsq==y.Jhrsq
                    && x.Jhfor==y.Jhfor
                    && x.Jhgap==y.Jhgap
                    && x.Jhgar==y.Jhgar
                    && x.Jhcar==y.Jhcar
                    && x.Jhnat==y.Jhnat
                    && x.Jhgan==y.Jhgan
                    && x.Jhobn==y.Jhobn
                    && x.Jhob1==y.Jhob1
                    && x.Jhob2==y.Jhob2
                    && x.Jhob3==y.Jhob3
                    && x.Jhob4==y.Jhob4
                    && x.Jhob5==y.Jhob5
                    && x.Jhgav==y.Jhgav
                    && x.Jhrqv==y.Jhrqv
                    && x.Jhfov==y.Jhfov
                    && x.Jhrep==y.Jhrep
                    && x.Jhtax==y.Jhtax
                    && x.Jhlcv==y.Jhlcv
                    && x.Jhlca==y.Jhlca
                    && x.Jhlcw==y.Jhlcw
                    && x.Jhlcu==y.Jhlcu
                    && x.Jhlce==y.Jhlce
                    && x.Jhlc1==y.Jhlc1
                    && x.Jhlc2==y.Jhlc2
                    && x.Jhlc3==y.Jhlc3
                    && x.Jhlc4==y.Jhlc4
                    && x.Jhlc5==y.Jhlc5
                    && x.Jhlov==y.Jhlov
                    && x.Jhloa==y.Jhloa
                    && x.Jhlow==y.Jhlow
                    && x.Jhlou==y.Jhlou
                    && x.Jhloe==y.Jhloe
                    && x.Jhasv==y.Jhasv
                    && x.Jhasa==y.Jhasa
                    && x.Jhasw==y.Jhasw
                    && x.Jhasu==y.Jhasu
                    && x.Jhasn==y.Jhasn
                    && x.Jhfhv==y.Jhfhv
                    && x.Jhfha==y.Jhfha
                    && x.Jhfhw==y.Jhfhw
                    && x.Jhfhu==y.Jhfhu
                    && x.Jhfhe==y.Jhfhe
                    && x.Jhprv==y.Jhprv
                    && x.Jhpra==y.Jhpra
                    && x.Jhprw==y.Jhprw
                    && x.Jhpru==y.Jhpru
                    && x.Jhprp==y.Jhprp
                    && x.Jhpre==y.Jhpre
                    && x.Jhprf==y.Jhprf
                    && x.Jhcna==y.Jhcna
                    && x.Jhina==y.Jhina
                    && x.Jhefa==y.Jhefa
                    && x.Jhefm==y.Jhefm
                    && x.Jhefj==y.Jhefj
                    && x.Jhefh==y.Jhefh
                    && x.Jhfea==y.Jhfea
                    && x.Jhfem==y.Jhfem
                    && x.Jhfej==y.Jhfej
                    && x.Jhfeh==y.Jhfeh
                    && x.Jhefd==y.Jhefd
                    && x.Jhefu==y.Jhefu
                    && x.Jhtmc==y.Jhtmc
                    && x.Jhtff==y.Jhtff
                    && x.Jhcmc==y.Jhcmc
                    && x.Jhcht==y.Jhcht
                    && x.Jhctt==y.Jhctt
                    && x.Jhajt==y.Jhajt
                    && x.Jhdfg==y.Jhdfg
                    && x.Jhifc==y.Jhifc
                    && x.Jhcda==y.Jhcda
                    && x.Jhcdm==y.Jhcdm
                    && x.Jhcdj==y.Jhcdj
                    && x.Jhcfa==y.Jhcfa
                    && x.Jhcfm==y.Jhcfm
                    && x.Jhcfj==y.Jhcfj
                    && x.Jhave==y.Jhave
                    && x.Jhavf==y.Jhavf  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Jhipb?? "").GetHashCode()
                      * 23 ) + (this.Jhalx.GetHashCode() ) 
                      * 23 ) + (this.Jhrsq.GetHashCode() ) 
                      * 23 ) + (this.Jhfor.GetHashCode() ) 
                      * 23 ) + (this.Jhgap.GetHashCode() ) 
                      * 23 ) + (this.Jhgar?? "").GetHashCode()
                      * 23 ) + (this.Jhcar?? "").GetHashCode()
                      * 23 ) + (this.Jhnat?? "").GetHashCode()
                      * 23 ) + (this.Jhgan?? "").GetHashCode()
                      * 23 ) + (this.Jhobn?? "").GetHashCode()
                      * 23 ) + (this.Jhob1.GetHashCode() ) 
                      * 23 ) + (this.Jhob2.GetHashCode() ) 
                      * 23 ) + (this.Jhob3.GetHashCode() ) 
                      * 23 ) + (this.Jhob4.GetHashCode() ) 
                      * 23 ) + (this.Jhob5.GetHashCode() ) 
                      * 23 ) + (this.Jhgav?? "").GetHashCode()
                      * 23 ) + (this.Jhrqv.GetHashCode() ) 
                      * 23 ) + (this.Jhfov.GetHashCode() ) 
                      * 23 ) + (this.Jhrep.GetHashCode() ) 
                      * 23 ) + (this.Jhtax?? "").GetHashCode()
                      * 23 ) + (this.Jhlcv.GetHashCode() ) 
                      * 23 ) + (this.Jhlca.GetHashCode() ) 
                      * 23 ) + (this.Jhlcw.GetHashCode() ) 
                      * 23 ) + (this.Jhlcu?? "").GetHashCode()
                      * 23 ) + (this.Jhlce?? "").GetHashCode()
                      * 23 ) + (this.Jhlc1.GetHashCode() ) 
                      * 23 ) + (this.Jhlc2.GetHashCode() ) 
                      * 23 ) + (this.Jhlc3.GetHashCode() ) 
                      * 23 ) + (this.Jhlc4.GetHashCode() ) 
                      * 23 ) + (this.Jhlc5.GetHashCode() ) 
                      * 23 ) + (this.Jhlov.GetHashCode() ) 
                      * 23 ) + (this.Jhloa.GetHashCode() ) 
                      * 23 ) + (this.Jhlow.GetHashCode() ) 
                      * 23 ) + (this.Jhlou?? "").GetHashCode()
                      * 23 ) + (this.Jhloe?? "").GetHashCode()
                      * 23 ) + (this.Jhasv.GetHashCode() ) 
                      * 23 ) + (this.Jhasa.GetHashCode() ) 
                      * 23 ) + (this.Jhasw.GetHashCode() ) 
                      * 23 ) + (this.Jhasu?? "").GetHashCode()
                      * 23 ) + (this.Jhasn?? "").GetHashCode()
                      * 23 ) + (this.Jhfhv.GetHashCode() ) 
                      * 23 ) + (this.Jhfha.GetHashCode() ) 
                      * 23 ) + (this.Jhfhw.GetHashCode() ) 
                      * 23 ) + (this.Jhfhu?? "").GetHashCode()
                      * 23 ) + (this.Jhfhe?? "").GetHashCode()
                      * 23 ) + (this.Jhprv.GetHashCode() ) 
                      * 23 ) + (this.Jhpra.GetHashCode() ) 
                      * 23 ) + (this.Jhprw.GetHashCode() ) 
                      * 23 ) + (this.Jhpru?? "").GetHashCode()
                      * 23 ) + (this.Jhprp?? "").GetHashCode()
                      * 23 ) + (this.Jhpre?? "").GetHashCode()
                      * 23 ) + (this.Jhprf?? "").GetHashCode()
                      * 23 ) + (this.Jhcna?? "").GetHashCode()
                      * 23 ) + (this.Jhina?? "").GetHashCode()
                      * 23 ) + (this.Jhefa.GetHashCode() ) 
                      * 23 ) + (this.Jhefm.GetHashCode() ) 
                      * 23 ) + (this.Jhefj.GetHashCode() ) 
                      * 23 ) + (this.Jhefh.GetHashCode() ) 
                      * 23 ) + (this.Jhfea.GetHashCode() ) 
                      * 23 ) + (this.Jhfem.GetHashCode() ) 
                      * 23 ) + (this.Jhfej.GetHashCode() ) 
                      * 23 ) + (this.Jhfeh.GetHashCode() ) 
                      * 23 ) + (this.Jhefd.GetHashCode() ) 
                      * 23 ) + (this.Jhefu?? "").GetHashCode()
                      * 23 ) + (this.Jhtmc.GetHashCode() ) 
                      * 23 ) + (this.Jhtff.GetHashCode() ) 
                      * 23 ) + (this.Jhcmc.GetHashCode() ) 
                      * 23 ) + (this.Jhcht.GetHashCode() ) 
                      * 23 ) + (this.Jhctt.GetHashCode() ) 
                      * 23 ) + (this.Jhajt?? "").GetHashCode()
                      * 23 ) + (this.Jhdfg?? "").GetHashCode()
                      * 23 ) + (this.Jhifc?? "").GetHashCode()
                      * 23 ) + (this.Jhcda.GetHashCode() ) 
                      * 23 ) + (this.Jhcdm.GetHashCode() ) 
                      * 23 ) + (this.Jhcdj.GetHashCode() ) 
                      * 23 ) + (this.Jhcfa.GetHashCode() ) 
                      * 23 ) + (this.Jhcfm.GetHashCode() ) 
                      * 23 ) + (this.Jhcfj.GetHashCode() ) 
                      * 23 ) + (this.Jhave.GetHashCode() ) 
                      * 23 ) + (this.Jhavf.GetHashCode() )                    );
           }
        }
    }
}
