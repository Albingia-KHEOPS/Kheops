using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.KPRGUWP
    public partial class KpRguWp  {
             //KPRGUWP

            ///<summary>Public empty contructor</summary>
            public KpRguWp() {}
            ///<summary>Public empty contructor</summary>
            public KpRguWp(KpRguWp copyFrom) 
            {
                  this.Khytyp= copyFrom.Khytyp;
                  this.Khyipb= copyFrom.Khyipb;
                  this.Khyalx= copyFrom.Khyalx;
                  this.Khyrsq= copyFrom.Khyrsq;
                  this.Khyfor= copyFrom.Khyfor;
                  this.Khykdeid= copyFrom.Khykdeid;
                  this.Khygaran= copyFrom.Khygaran;
                  this.Khydebp= copyFrom.Khydebp;
                  this.Khyfinp= copyFrom.Khyfinp;
                  this.Khytrg= copyFrom.Khytrg;
                  this.Khynpe= copyFrom.Khynpe;
                  this.Khyven= copyFrom.Khyven;
                  this.Khycaf= copyFrom.Khycaf;
                  this.Khycau= copyFrom.Khycau;
                  this.Khycae= copyFrom.Khycae;
                  this.Khydm1= copyFrom.Khydm1;
                  this.Khydt1= copyFrom.Khydt1;
                  this.Khydm2= copyFrom.Khydm2;
                  this.Khydt2= copyFrom.Khydt2;
                  this.Khycoe= copyFrom.Khycoe;
                  this.Khyca1= copyFrom.Khyca1;
                  this.Khyct1= copyFrom.Khyct1;
                  this.Khycu1= copyFrom.Khycu1;
                  this.Khycp1= copyFrom.Khycp1;
                  this.Khycx1= copyFrom.Khycx1;
                  this.Khyca2= copyFrom.Khyca2;
                  this.Khyct2= copyFrom.Khyct2;
                  this.Khycu2= copyFrom.Khycu2;
                  this.Khycp2= copyFrom.Khycp2;
                  this.Khycx2= copyFrom.Khycx2;
                  this.Khyaju= copyFrom.Khyaju;
                  this.Khylmr= copyFrom.Khylmr;
                  this.Khymba= copyFrom.Khymba;
                  this.Khyten= copyFrom.Khyten;
                  this.Khybrg= copyFrom.Khybrg;
                  this.Khybrl= copyFrom.Khybrl;
                  this.Khybas= copyFrom.Khybas;
                  this.Khybat= copyFrom.Khybat;
                  this.Khybau= copyFrom.Khybau;
                  this.Khybam= copyFrom.Khybam;
                  this.Khyxf1= copyFrom.Khyxf1;
                  this.Khyxb1= copyFrom.Khyxb1;
                  this.Khyxm1= copyFrom.Khyxm1;
                  this.Khyxf2= copyFrom.Khyxf2;
                  this.Khyxb2= copyFrom.Khyxb2;
                  this.Khyxm2= copyFrom.Khyxm2;
                  this.Khyxf3= copyFrom.Khyxf3;
                  this.Khyxb3= copyFrom.Khyxb3;
                  this.Khyxm3= copyFrom.Khyxm3;
                  this.Khyreg= copyFrom.Khyreg;
                  this.Khypei= copyFrom.Khypei;
                  this.Khycnh= copyFrom.Khycnh;
                  this.Khycnt= copyFrom.Khycnt;
                  this.Khygrm= copyFrom.Khygrm;
                  this.Khykea= copyFrom.Khykea;
                  this.Khypbp= copyFrom.Khypbp;
                  this.Khyktd= copyFrom.Khyktd;
                  this.Khyasv= copyFrom.Khyasv;
                  this.Khypbt= copyFrom.Khypbt;
                  this.Khysip= copyFrom.Khysip;
                  this.Khypbs= copyFrom.Khypbs;
                  this.Khyris= copyFrom.Khyris;
                  this.Khypbr= copyFrom.Khypbr;
                  this.Khyria= copyFrom.Khyria;
                  this.Khyavn= copyFrom.Khyavn;
        
            }        
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Khytyp { get; set; } 
            
            ///<summary> N° de Police </summary>
            public string Khyipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Khyalx { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Khyrsq { get; set; } 
            
            ///<summary> Id formule </summary>
            public int Khyfor { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Khykdeid { get; set; } 
            
            ///<summary> Code garantie </summary>
            public string Khygaran { get; set; } 
            
            ///<summary> Début période </summary>
            public int Khydebp { get; set; } 
            
            ///<summary> Fin de période </summary>
            public int Khyfinp { get; set; } 
            
            ///<summary> Type de régule </summary>
            public string Khytrg { get; set; } 
            
            ///<summary> Nature Régule </summary>
            public string Khynpe { get; set; } 
            
            ///<summary> Ventilation CA  en % </summary>
            public Decimal Khyven { get; set; } 
            
            ///<summary> Chiffre Affaire France </summary>
            public Int64 Khycaf { get; set; } 
            
            ///<summary> Chiffre d'Affaire USA/CANADA </summary>
            public Int64 Khycau { get; set; } 
            
            ///<summary> Chiffre d'Affaire Export </summary>
            public Int64 Khycae { get; set; } 
            
            ///<summary> Déja émis HT 1 </summary>
            public Decimal Khydm1 { get; set; } 
            
            ///<summary> Déja émis Taxes 1 </summary>
            public Decimal Khydt1 { get; set; } 
            
            ///<summary> Déja émis HT 2 </summary>
            public Decimal Khydm2 { get; set; } 
            
            ///<summary> Déja émis Taxes 2 </summary>
            public Decimal Khydt2 { get; set; } 
            
            ///<summary> Coefficient </summary>
            public Decimal Khycoe { get; set; } 
            
            ///<summary> MB&CA Prévisionnel OU CA 1 </summary>
            public Decimal Khyca1 { get; set; } 
            
            ///<summary> MB&CA Prévisionnel ou 1 Taux </summary>
            public Decimal Khyct1 { get; set; } 
            
            ///<summary> Unité taux 1 </summary>
            public string Khycu1 { get; set; } 
            
            ///<summary> MB&CA prévisionnel ou 1 Prime </summary>
            public Decimal Khycp1 { get; set; } 
            
            ///<summary> Code taxe  1 </summary>
            public string Khycx1 { get; set; } 
            
            ///<summary> MB&CA Définitif ou CA 2 </summary>
            public Decimal Khyca2 { get; set; } 
            
            ///<summary> MB&CA Définitif ou 2  Taux </summary>
            public Decimal Khyct2 { get; set; } 
            
            ///<summary> Unité taux 2 </summary>
            public string Khycu2 { get; set; } 
            
            ///<summary> MB&CA Définitif ou 2 Prime </summary>
            public Decimal Khycp2 { get; set; } 
            
            ///<summary> Code taxe 2 </summary>
            public string Khycx2 { get; set; } 
            
            ///<summary> % Ajustabilité </summary>
            public Decimal Khyaju { get; set; } 
            
            ///<summary> Limite ristourne </summary>
            public Decimal Khylmr { get; set; } 
            
            ///<summary> MB Assuré ou MB & tendance </summary>
            public Decimal Khymba { get; set; } 
            
            ///<summary> Tendance </summary>
            public Decimal Khyten { get; set; } 
            
            ///<summary> Code Base de régule </summary>
            public string Khybrg { get; set; } 
            
            ///<summary> Libellé définition </summary>
            public string Khybrl { get; set; } 
            
            ///<summary> Base Base de régule </summary>
            public Decimal Khybas { get; set; } 
            
            ///<summary> Base Taux sur Base </summary>
            public Decimal Khybat { get; set; } 
            
            ///<summary> Unité Taux sur Base </summary>
            public string Khybau { get; set; } 
            
            ///<summary> Base Montant </summary>
            public Decimal Khybam { get; set; } 
            
            ///<summary> Famille de taxe 1 </summary>
            public string Khyxf1 { get; set; } 
            
            ///<summary> Base Famille de taxe 1 </summary>
            public Decimal Khyxb1 { get; set; } 
            
            ///<summary> Montant de taxe Famille 1 </summary>
            public Decimal Khyxm1 { get; set; } 
            
            ///<summary> Famille de taxe 2 </summary>
            public string Khyxf2 { get; set; } 
            
            ///<summary> Base Famille de taxe 2 </summary>
            public Decimal Khyxb2 { get; set; } 
            
            ///<summary> Montant de taxe Famille 2 </summary>
            public Decimal Khyxm2 { get; set; } 
            
            ///<summary> Famille de taxe 3 </summary>
            public string Khyxf3 { get; set; } 
            
            ///<summary> Base Famille de taxe 3 </summary>
            public Decimal Khyxb3 { get; set; } 
            
            ///<summary> Montant de taxe Famille 3 </summary>
            public Decimal Khyxm3 { get; set; } 
            
            ///<summary> Garantie régulée </summary>
            public string Khyreg { get; set; } 
            
            ///<summary> Période Indemnisation en NB Mois </summary>
            public int Khypei { get; set; } 
            
            ///<summary> CATNAT montant HT </summary>
            public Decimal Khycnh { get; set; } 
            
            ///<summary> CATNAT Montant de taxe </summary>
            public Decimal Khycnt { get; set; } 
            
            ///<summary> GAREAT Montant HT Compris </summary>
            public Decimal Khygrm { get; set; } 
            
            ///<summary> PB Prime émise acquitée </summary>
            public Decimal Khykea { get; set; } 
            
            ///<summary> PB % de prime retenue </summary>
            public Decimal Khypbp { get; set; } 
            
            ///<summary> PB Prime technique due </summary>
            public Decimal Khyktd { get; set; } 
            
            ///<summary> PB Valeur de l'Assiette </summary>
            public Decimal Khyasv { get; set; } 
            
            ///<summary> PB taux Appel </summary>
            public Decimal Khypbt { get; set; } 
            
            ///<summary> PB Sinistres Payés </summary>
            public Decimal Khysip { get; set; } 
            
            ///<summary> PB seuil de rapport S/P </summary>
            public Decimal Khypbs { get; set; } 
            
            ///<summary> PB montant de Ristourne </summary>
            public Decimal Khyris { get; set; } 
            
            ///<summary> PB Ristourne en % </summary>
            public Decimal Khypbr { get; set; } 
            
            ///<summary> PB Ristourne Anticipée </summary>
            public Decimal Khyria { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Khyavn { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpRguWp  x=this,  y=obj as KpRguWp;
            if( y == default(KpRguWp) ) return false;
            return (
                    x.Khytyp==y.Khytyp
                    && x.Khyipb==y.Khyipb
                    && x.Khyalx==y.Khyalx
                    && x.Khyrsq==y.Khyrsq
                    && x.Khyfor==y.Khyfor
                    && x.Khykdeid==y.Khykdeid
                    && x.Khygaran==y.Khygaran
                    && x.Khydebp==y.Khydebp
                    && x.Khyfinp==y.Khyfinp
                    && x.Khytrg==y.Khytrg
                    && x.Khynpe==y.Khynpe
                    && x.Khyven==y.Khyven
                    && x.Khycaf==y.Khycaf
                    && x.Khycau==y.Khycau
                    && x.Khycae==y.Khycae
                    && x.Khydm1==y.Khydm1
                    && x.Khydt1==y.Khydt1
                    && x.Khydm2==y.Khydm2
                    && x.Khydt2==y.Khydt2
                    && x.Khycoe==y.Khycoe
                    && x.Khyca1==y.Khyca1
                    && x.Khyct1==y.Khyct1
                    && x.Khycu1==y.Khycu1
                    && x.Khycp1==y.Khycp1
                    && x.Khycx1==y.Khycx1
                    && x.Khyca2==y.Khyca2
                    && x.Khyct2==y.Khyct2
                    && x.Khycu2==y.Khycu2
                    && x.Khycp2==y.Khycp2
                    && x.Khycx2==y.Khycx2
                    && x.Khyaju==y.Khyaju
                    && x.Khylmr==y.Khylmr
                    && x.Khymba==y.Khymba
                    && x.Khyten==y.Khyten
                    && x.Khybrg==y.Khybrg
                    && x.Khybrl==y.Khybrl
                    && x.Khybas==y.Khybas
                    && x.Khybat==y.Khybat
                    && x.Khybau==y.Khybau
                    && x.Khybam==y.Khybam
                    && x.Khyxf1==y.Khyxf1
                    && x.Khyxb1==y.Khyxb1
                    && x.Khyxm1==y.Khyxm1
                    && x.Khyxf2==y.Khyxf2
                    && x.Khyxb2==y.Khyxb2
                    && x.Khyxm2==y.Khyxm2
                    && x.Khyxf3==y.Khyxf3
                    && x.Khyxb3==y.Khyxb3
                    && x.Khyxm3==y.Khyxm3
                    && x.Khyreg==y.Khyreg
                    && x.Khypei==y.Khypei
                    && x.Khycnh==y.Khycnh
                    && x.Khycnt==y.Khycnt
                    && x.Khygrm==y.Khygrm
                    && x.Khykea==y.Khykea
                    && x.Khypbp==y.Khypbp
                    && x.Khyktd==y.Khyktd
                    && x.Khyasv==y.Khyasv
                    && x.Khypbt==y.Khypbt
                    && x.Khysip==y.Khysip
                    && x.Khypbs==y.Khypbs
                    && x.Khyris==y.Khyris
                    && x.Khypbr==y.Khypbr
                    && x.Khyria==y.Khyria
                    && x.Khyavn==y.Khyavn  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Khytyp?? "").GetHashCode()
                      * 23 ) + (this.Khyipb?? "").GetHashCode()
                      * 23 ) + (this.Khyalx.GetHashCode() ) 
                      * 23 ) + (this.Khyrsq.GetHashCode() ) 
                      * 23 ) + (this.Khyfor.GetHashCode() ) 
                      * 23 ) + (this.Khykdeid.GetHashCode() ) 
                      * 23 ) + (this.Khygaran?? "").GetHashCode()
                      * 23 ) + (this.Khydebp.GetHashCode() ) 
                      * 23 ) + (this.Khyfinp.GetHashCode() ) 
                      * 23 ) + (this.Khytrg?? "").GetHashCode()
                      * 23 ) + (this.Khynpe?? "").GetHashCode()
                      * 23 ) + (this.Khyven.GetHashCode() ) 
                      * 23 ) + (this.Khycaf.GetHashCode() ) 
                      * 23 ) + (this.Khycau.GetHashCode() ) 
                      * 23 ) + (this.Khycae.GetHashCode() ) 
                      * 23 ) + (this.Khydm1.GetHashCode() ) 
                      * 23 ) + (this.Khydt1.GetHashCode() ) 
                      * 23 ) + (this.Khydm2.GetHashCode() ) 
                      * 23 ) + (this.Khydt2.GetHashCode() ) 
                      * 23 ) + (this.Khycoe.GetHashCode() ) 
                      * 23 ) + (this.Khyca1.GetHashCode() ) 
                      * 23 ) + (this.Khyct1.GetHashCode() ) 
                      * 23 ) + (this.Khycu1?? "").GetHashCode()
                      * 23 ) + (this.Khycp1.GetHashCode() ) 
                      * 23 ) + (this.Khycx1?? "").GetHashCode()
                      * 23 ) + (this.Khyca2.GetHashCode() ) 
                      * 23 ) + (this.Khyct2.GetHashCode() ) 
                      * 23 ) + (this.Khycu2?? "").GetHashCode()
                      * 23 ) + (this.Khycp2.GetHashCode() ) 
                      * 23 ) + (this.Khycx2?? "").GetHashCode()
                      * 23 ) + (this.Khyaju.GetHashCode() ) 
                      * 23 ) + (this.Khylmr.GetHashCode() ) 
                      * 23 ) + (this.Khymba.GetHashCode() ) 
                      * 23 ) + (this.Khyten.GetHashCode() ) 
                      * 23 ) + (this.Khybrg?? "").GetHashCode()
                      * 23 ) + (this.Khybrl?? "").GetHashCode()
                      * 23 ) + (this.Khybas.GetHashCode() ) 
                      * 23 ) + (this.Khybat.GetHashCode() ) 
                      * 23 ) + (this.Khybau?? "").GetHashCode()
                      * 23 ) + (this.Khybam.GetHashCode() ) 
                      * 23 ) + (this.Khyxf1?? "").GetHashCode()
                      * 23 ) + (this.Khyxb1.GetHashCode() ) 
                      * 23 ) + (this.Khyxm1.GetHashCode() ) 
                      * 23 ) + (this.Khyxf2?? "").GetHashCode()
                      * 23 ) + (this.Khyxb2.GetHashCode() ) 
                      * 23 ) + (this.Khyxm2.GetHashCode() ) 
                      * 23 ) + (this.Khyxf3?? "").GetHashCode()
                      * 23 ) + (this.Khyxb3.GetHashCode() ) 
                      * 23 ) + (this.Khyxm3.GetHashCode() ) 
                      * 23 ) + (this.Khyreg?? "").GetHashCode()
                      * 23 ) + (this.Khypei.GetHashCode() ) 
                      * 23 ) + (this.Khycnh.GetHashCode() ) 
                      * 23 ) + (this.Khycnt.GetHashCode() ) 
                      * 23 ) + (this.Khygrm.GetHashCode() ) 
                      * 23 ) + (this.Khykea.GetHashCode() ) 
                      * 23 ) + (this.Khypbp.GetHashCode() ) 
                      * 23 ) + (this.Khyktd.GetHashCode() ) 
                      * 23 ) + (this.Khyasv.GetHashCode() ) 
                      * 23 ) + (this.Khypbt.GetHashCode() ) 
                      * 23 ) + (this.Khysip.GetHashCode() ) 
                      * 23 ) + (this.Khypbs.GetHashCode() ) 
                      * 23 ) + (this.Khyris.GetHashCode() ) 
                      * 23 ) + (this.Khypbr.GetHashCode() ) 
                      * 23 ) + (this.Khyria.GetHashCode() ) 
                      * 23 ) + (this.Khyavn.GetHashCode() )                    );
           }
        }
    }
}
