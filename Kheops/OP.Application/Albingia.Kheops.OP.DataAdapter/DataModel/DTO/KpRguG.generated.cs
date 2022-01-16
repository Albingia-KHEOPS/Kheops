using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.KPRGUG
    public partial class KpRguG  {
             //KPRGUG

            ///<summary>Public empty contructor</summary>
            public KpRguG() {}
            ///<summary>Public empty contructor</summary>
            public KpRguG(KpRguG copyFrom) 
            {
                  this.Khxid= copyFrom.Khxid;
                  this.Khxkhwid= copyFrom.Khxkhwid;
                  this.Khxtyp= copyFrom.Khxtyp;
                  this.Khxipb= copyFrom.Khxipb;
                  this.Khxalx= copyFrom.Khxalx;
                  this.Khxrsq= copyFrom.Khxrsq;
                  this.Khxfor= copyFrom.Khxfor;
                  this.Khxkdeid= copyFrom.Khxkdeid;
                  this.Khxgaran= copyFrom.Khxgaran;
                  this.Khxdebp= copyFrom.Khxdebp;
                  this.Khxfinp= copyFrom.Khxfinp;
                  this.Khxsit= copyFrom.Khxsit;
                  this.Khxtrg= copyFrom.Khxtrg;
                  this.Khxnpe= copyFrom.Khxnpe;
                  this.Khxven= copyFrom.Khxven;
                  this.Khxcaf= copyFrom.Khxcaf;
                  this.Khxcau= copyFrom.Khxcau;
                  this.Khxcae= copyFrom.Khxcae;
                  this.Khxmhc= copyFrom.Khxmhc;
                  this.Khxfrc= copyFrom.Khxfrc;
                  this.Khxfr0= copyFrom.Khxfr0;
                  this.Khxmht= copyFrom.Khxmht;
                  this.Khxmtx= copyFrom.Khxmtx;
                  this.Khxttt= copyFrom.Khxttt;
                  this.Khxmtt= copyFrom.Khxmtt;
                  this.Khxcnh= copyFrom.Khxcnh;
                  this.Khxcnt= copyFrom.Khxcnt;
                  this.Khxgrm= copyFrom.Khxgrm;
                  this.Khxpro= copyFrom.Khxpro;
                  this.Khxech= copyFrom.Khxech;
                  this.Khxect= copyFrom.Khxect;
                  this.Khxemh= copyFrom.Khxemh;
                  this.Khxemt= copyFrom.Khxemt;
                  this.Khxdm1= copyFrom.Khxdm1;
                  this.Khxdt1= copyFrom.Khxdt1;
                  this.Khxdm2= copyFrom.Khxdm2;
                  this.Khxdt2= copyFrom.Khxdt2;
                  this.Khxcoe= copyFrom.Khxcoe;
                  this.Khxca1= copyFrom.Khxca1;
                  this.Khxct1= copyFrom.Khxct1;
                  this.Khxcu1= copyFrom.Khxcu1;
                  this.Khxcp1= copyFrom.Khxcp1;
                  this.Khxcx1= copyFrom.Khxcx1;
                  this.Khxca2= copyFrom.Khxca2;
                  this.Khxct2= copyFrom.Khxct2;
                  this.Khxcu2= copyFrom.Khxcu2;
                  this.Khxcp2= copyFrom.Khxcp2;
                  this.Khxcx2= copyFrom.Khxcx2;
                  this.Khxaju= copyFrom.Khxaju;
                  this.Khxlmr= copyFrom.Khxlmr;
                  this.Khxmba= copyFrom.Khxmba;
                  this.Khxten= copyFrom.Khxten;
                  this.Khxhon= copyFrom.Khxhon;
                  this.Khxhox= copyFrom.Khxhox;
                  this.Khxbrg= copyFrom.Khxbrg;
                  this.Khxbrl= copyFrom.Khxbrl;
                  this.Khxbas= copyFrom.Khxbas;
                  this.Khxbat= copyFrom.Khxbat;
                  this.Khxbau= copyFrom.Khxbau;
                  this.Khxbam= copyFrom.Khxbam;
                  this.Khxxf1= copyFrom.Khxxf1;
                  this.Khxxb1= copyFrom.Khxxb1;
                  this.Khxxm1= copyFrom.Khxxm1;
                  this.Khxxf2= copyFrom.Khxxf2;
                  this.Khxxb2= copyFrom.Khxxb2;
                  this.Khxxm2= copyFrom.Khxxm2;
                  this.Khxxf3= copyFrom.Khxxf3;
                  this.Khxxb3= copyFrom.Khxxb3;
                  this.Khxxm3= copyFrom.Khxxm3;
                  this.Khxreg= copyFrom.Khxreg;
                  this.Khxpei= copyFrom.Khxpei;
                  this.Khxkea= copyFrom.Khxkea;
                  this.Khxpbp= copyFrom.Khxpbp;
                  this.Khxktd= copyFrom.Khxktd;
                  this.Khxasv= copyFrom.Khxasv;
                  this.Khxpbt= copyFrom.Khxpbt;
                  this.Khxsip= copyFrom.Khxsip;
                  this.Khxpbs= copyFrom.Khxpbs;
                  this.Khxris= copyFrom.Khxris;
                  this.Khxpbr= copyFrom.Khxpbr;
                  this.Khxria= copyFrom.Khxria;
                  this.Khxavn= copyFrom.Khxavn;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Khxid { get; set; } 
            
            ///<summary> Lien KPRGU   Entête </summary>
            public Int64 Khxkhwid { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Khxtyp { get; set; } 
            
            ///<summary> N° de Police </summary>
            public string Khxipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Khxalx { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Khxrsq { get; set; } 
            
            ///<summary> Id formule </summary>
            public int Khxfor { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Khxkdeid { get; set; } 
            
            ///<summary> Code garantie </summary>
            public string Khxgaran { get; set; } 
            
            ///<summary> Début période </summary>
            public int Khxdebp { get; set; } 
            
            ///<summary> Fin de période </summary>
            public int Khxfinp { get; set; } 
            
            ///<summary> Code situation  N/A/V </summary>
            public string Khxsit { get; set; } 
            
            ///<summary> Type de régule </summary>
            public string Khxtrg { get; set; } 
            
            ///<summary> Nature Régule </summary>
            public string Khxnpe { get; set; } 
            
            ///<summary> Ventilation CA  en % </summary>
            public Decimal Khxven { get; set; } 
            
            ///<summary> Chiffre Affaire France </summary>
            public Int64 Khxcaf { get; set; } 
            
            ///<summary> Chiffre d'Affaire USA/CANADA </summary>
            public Int64 Khxcau { get; set; } 
            
            ///<summary> Chiffre d'Affaire Export </summary>
            public Int64 Khxcae { get; set; } 
            
            ///<summary> Mnt régule HT Hors CN calculé </summary>
            public Decimal Khxmhc { get; set; } 
            
            ///<summary> Mnt régule forcé Forcé O/N </summary>
            public string Khxfrc { get; set; } 
            
            ///<summary> Si forcé forcé à zéro O/N </summary>
            public string Khxfr0 { get; set; } 
            
            ///<summary> Mnt régule HT Hors Catnat </summary>
            public Decimal Khxmht { get; set; } 
            
            ///<summary> Mnt régule Taxes Hors Catnat </summary>
            public Decimal Khxmtx { get; set; } 
            
            ///<summary> Total taxes </summary>
            public Decimal Khxttt { get; set; } 
            
            ///<summary> Mnt régule TTC Hors CN </summary>
            public Decimal Khxmtt { get; set; } 
            
            ///<summary> Mnt régule CATNAT montant HT </summary>
            public Decimal Khxcnh { get; set; } 
            
            ///<summary> Mnt régule CATNAT Montant de taxe </summary>
            public Decimal Khxcnt { get; set; } 
            
            ///<summary> Mnt régule GAREAT Montant HT Compris </summary>
            public Decimal Khxgrm { get; set; } 
            
            ///<summary> Montant prime provisionnelle </summary>
            public Decimal Khxpro { get; set; } 
            
            ///<summary> Déja émis calculé HT </summary>
            public Decimal Khxech { get; set; } 
            
            ///<summary> Déja émis calculé Taxes </summary>
            public Decimal Khxect { get; set; } 
            
            ///<summary> Déja émis retenu HT </summary>
            public Decimal Khxemh { get; set; } 
            
            ///<summary> Déja émis retenu Mnt Taxe </summary>
            public Decimal Khxemt { get; set; } 
            
            ///<summary> Déja émis HT 1 </summary>
            public Decimal Khxdm1 { get; set; } 
            
            ///<summary> Déja émis Taxes 1 </summary>
            public Decimal Khxdt1 { get; set; } 
            
            ///<summary> Déja émis HT 2 </summary>
            public Decimal Khxdm2 { get; set; } 
            
            ///<summary> Déja émis Taxes 2 </summary>
            public Decimal Khxdt2 { get; set; } 
            
            ///<summary> Coefficient </summary>
            public Decimal Khxcoe { get; set; } 
            
            ///<summary> MB&CA Prévisionnel OU CA 1 </summary>
            public Decimal Khxca1 { get; set; } 
            
            ///<summary> MB&CA Prévisionnel ou 1 Taux </summary>
            public Decimal Khxct1 { get; set; } 
            
            ///<summary> Unité taux 1 </summary>
            public string Khxcu1 { get; set; } 
            
            ///<summary> MB&CA prévisionnel ou 1 Prime </summary>
            public Decimal Khxcp1 { get; set; } 
            
            ///<summary> Code taxe  1 </summary>
            public string Khxcx1 { get; set; } 
            
            ///<summary> MB&CA Définitif ou CA 2 </summary>
            public Decimal Khxca2 { get; set; } 
            
            ///<summary> MB&CA Définitif ou 2  Taux </summary>
            public Decimal Khxct2 { get; set; } 
            
            ///<summary> Unité taux 2 </summary>
            public string Khxcu2 { get; set; } 
            
            ///<summary> MB&CA Définitif ou 2 Prime </summary>
            public Decimal Khxcp2 { get; set; } 
            
            ///<summary> Code taxe 2 </summary>
            public string Khxcx2 { get; set; } 
            
            ///<summary> % Ajustabilité </summary>
            public Decimal Khxaju { get; set; } 
            
            ///<summary> Limite ristourne </summary>
            public Decimal Khxlmr { get; set; } 
            
            ///<summary> MB Assuré ou MB & tendance </summary>
            public Decimal Khxmba { get; set; } 
            
            ///<summary> Tendance </summary>
            public Decimal Khxten { get; set; } 
            
            ///<summary> Montant Honoraire HT </summary>
            public Decimal Khxhon { get; set; } 
            
            ///<summary> Honoraire Code taxe </summary>
            public string Khxhox { get; set; } 
            
            ///<summary> Code Base de régule </summary>
            public string Khxbrg { get; set; } 
            
            ///<summary> Libellé définition </summary>
            public string Khxbrl { get; set; } 
            
            ///<summary> Base Base de régule </summary>
            public Decimal Khxbas { get; set; } 
            
            ///<summary> Base Taux sur Base </summary>
            public Decimal Khxbat { get; set; } 
            
            ///<summary> Unité Taux sur Base </summary>
            public string Khxbau { get; set; } 
            
            ///<summary> Base Montant </summary>
            public Decimal Khxbam { get; set; } 
            
            ///<summary> Famille de taxe 1 </summary>
            public string Khxxf1 { get; set; } 
            
            ///<summary> Base Famille de taxe 1 </summary>
            public Decimal Khxxb1 { get; set; } 
            
            ///<summary> Montant de taxe Famille 1 </summary>
            public Decimal Khxxm1 { get; set; } 
            
            ///<summary> Famille de taxe 2 </summary>
            public string Khxxf2 { get; set; } 
            
            ///<summary> Base Famille de taxe 2 </summary>
            public Decimal Khxxb2 { get; set; } 
            
            ///<summary> Montant de taxe Famille 2 </summary>
            public Decimal Khxxm2 { get; set; } 
            
            ///<summary> Famille de taxe 3 </summary>
            public string Khxxf3 { get; set; } 
            
            ///<summary> Base Famille de taxe 3 </summary>
            public Decimal Khxxb3 { get; set; } 
            
            ///<summary> Montant de taxe Famille 3 </summary>
            public Decimal Khxxm3 { get; set; } 
            
            ///<summary> Garantie régulée </summary>
            public string Khxreg { get; set; } 
            
            ///<summary> Période Indemnisation en NB Mois </summary>
            public int Khxpei { get; set; } 
            
            ///<summary> PB Prime émise acquitée </summary>
            public Decimal Khxkea { get; set; } 
            
            ///<summary> PB % de prime retenue </summary>
            public Decimal Khxpbp { get; set; } 
            
            ///<summary> PB Prime technique due </summary>
            public Decimal Khxktd { get; set; } 
            
            ///<summary> PB Valeur de l'Assiette </summary>
            public Decimal Khxasv { get; set; } 
            
            ///<summary> PB taux Appel </summary>
            public Decimal Khxpbt { get; set; } 
            
            ///<summary> PB Sinistres Payés </summary>
            public Decimal Khxsip { get; set; } 
            
            ///<summary> PB seuil de rapport S/P </summary>
            public Decimal Khxpbs { get; set; } 
            
            ///<summary> PB montant de Ristourne </summary>
            public Decimal Khxris { get; set; } 
            
            ///<summary> PB Ristourne en % </summary>
            public Decimal Khxpbr { get; set; } 
            
            ///<summary> PB Ristourne Anticipée </summary>
            public Decimal Khxria { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Khxavn { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpRguG  x=this,  y=obj as KpRguG;
            if( y == default(KpRguG) ) return false;
            return (
                    x.Khxid==y.Khxid
                    && x.Khxkhwid==y.Khxkhwid
                    && x.Khxtyp==y.Khxtyp
                    && x.Khxipb==y.Khxipb
                    && x.Khxalx==y.Khxalx
                    && x.Khxrsq==y.Khxrsq
                    && x.Khxfor==y.Khxfor
                    && x.Khxkdeid==y.Khxkdeid
                    && x.Khxgaran==y.Khxgaran
                    && x.Khxdebp==y.Khxdebp
                    && x.Khxfinp==y.Khxfinp
                    && x.Khxsit==y.Khxsit
                    && x.Khxtrg==y.Khxtrg
                    && x.Khxnpe==y.Khxnpe
                    && x.Khxven==y.Khxven
                    && x.Khxcaf==y.Khxcaf
                    && x.Khxcau==y.Khxcau
                    && x.Khxcae==y.Khxcae
                    && x.Khxmhc==y.Khxmhc
                    && x.Khxfrc==y.Khxfrc
                    && x.Khxfr0==y.Khxfr0
                    && x.Khxmht==y.Khxmht
                    && x.Khxmtx==y.Khxmtx
                    && x.Khxttt==y.Khxttt
                    && x.Khxmtt==y.Khxmtt
                    && x.Khxcnh==y.Khxcnh
                    && x.Khxcnt==y.Khxcnt
                    && x.Khxgrm==y.Khxgrm
                    && x.Khxpro==y.Khxpro
                    && x.Khxech==y.Khxech
                    && x.Khxect==y.Khxect
                    && x.Khxemh==y.Khxemh
                    && x.Khxemt==y.Khxemt
                    && x.Khxdm1==y.Khxdm1
                    && x.Khxdt1==y.Khxdt1
                    && x.Khxdm2==y.Khxdm2
                    && x.Khxdt2==y.Khxdt2
                    && x.Khxcoe==y.Khxcoe
                    && x.Khxca1==y.Khxca1
                    && x.Khxct1==y.Khxct1
                    && x.Khxcu1==y.Khxcu1
                    && x.Khxcp1==y.Khxcp1
                    && x.Khxcx1==y.Khxcx1
                    && x.Khxca2==y.Khxca2
                    && x.Khxct2==y.Khxct2
                    && x.Khxcu2==y.Khxcu2
                    && x.Khxcp2==y.Khxcp2
                    && x.Khxcx2==y.Khxcx2
                    && x.Khxaju==y.Khxaju
                    && x.Khxlmr==y.Khxlmr
                    && x.Khxmba==y.Khxmba
                    && x.Khxten==y.Khxten
                    && x.Khxhon==y.Khxhon
                    && x.Khxhox==y.Khxhox
                    && x.Khxbrg==y.Khxbrg
                    && x.Khxbrl==y.Khxbrl
                    && x.Khxbas==y.Khxbas
                    && x.Khxbat==y.Khxbat
                    && x.Khxbau==y.Khxbau
                    && x.Khxbam==y.Khxbam
                    && x.Khxxf1==y.Khxxf1
                    && x.Khxxb1==y.Khxxb1
                    && x.Khxxm1==y.Khxxm1
                    && x.Khxxf2==y.Khxxf2
                    && x.Khxxb2==y.Khxxb2
                    && x.Khxxm2==y.Khxxm2
                    && x.Khxxf3==y.Khxxf3
                    && x.Khxxb3==y.Khxxb3
                    && x.Khxxm3==y.Khxxm3
                    && x.Khxreg==y.Khxreg
                    && x.Khxpei==y.Khxpei
                    && x.Khxkea==y.Khxkea
                    && x.Khxpbp==y.Khxpbp
                    && x.Khxktd==y.Khxktd
                    && x.Khxasv==y.Khxasv
                    && x.Khxpbt==y.Khxpbt
                    && x.Khxsip==y.Khxsip
                    && x.Khxpbs==y.Khxpbs
                    && x.Khxris==y.Khxris
                    && x.Khxpbr==y.Khxpbr
                    && x.Khxria==y.Khxria
                    && x.Khxavn==y.Khxavn  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Khxid.GetHashCode() ) 
                      * 23 ) + (this.Khxkhwid.GetHashCode() ) 
                      * 23 ) + (this.Khxtyp?? "").GetHashCode()
                      * 23 ) + (this.Khxipb?? "").GetHashCode()
                      * 23 ) + (this.Khxalx.GetHashCode() ) 
                      * 23 ) + (this.Khxrsq.GetHashCode() ) 
                      * 23 ) + (this.Khxfor.GetHashCode() ) 
                      * 23 ) + (this.Khxkdeid.GetHashCode() ) 
                      * 23 ) + (this.Khxgaran?? "").GetHashCode()
                      * 23 ) + (this.Khxdebp.GetHashCode() ) 
                      * 23 ) + (this.Khxfinp.GetHashCode() ) 
                      * 23 ) + (this.Khxsit?? "").GetHashCode()
                      * 23 ) + (this.Khxtrg?? "").GetHashCode()
                      * 23 ) + (this.Khxnpe?? "").GetHashCode()
                      * 23 ) + (this.Khxven.GetHashCode() ) 
                      * 23 ) + (this.Khxcaf.GetHashCode() ) 
                      * 23 ) + (this.Khxcau.GetHashCode() ) 
                      * 23 ) + (this.Khxcae.GetHashCode() ) 
                      * 23 ) + (this.Khxmhc.GetHashCode() ) 
                      * 23 ) + (this.Khxfrc?? "").GetHashCode()
                      * 23 ) + (this.Khxfr0?? "").GetHashCode()
                      * 23 ) + (this.Khxmht.GetHashCode() ) 
                      * 23 ) + (this.Khxmtx.GetHashCode() ) 
                      * 23 ) + (this.Khxttt.GetHashCode() ) 
                      * 23 ) + (this.Khxmtt.GetHashCode() ) 
                      * 23 ) + (this.Khxcnh.GetHashCode() ) 
                      * 23 ) + (this.Khxcnt.GetHashCode() ) 
                      * 23 ) + (this.Khxgrm.GetHashCode() ) 
                      * 23 ) + (this.Khxpro.GetHashCode() ) 
                      * 23 ) + (this.Khxech.GetHashCode() ) 
                      * 23 ) + (this.Khxect.GetHashCode() ) 
                      * 23 ) + (this.Khxemh.GetHashCode() ) 
                      * 23 ) + (this.Khxemt.GetHashCode() ) 
                      * 23 ) + (this.Khxdm1.GetHashCode() ) 
                      * 23 ) + (this.Khxdt1.GetHashCode() ) 
                      * 23 ) + (this.Khxdm2.GetHashCode() ) 
                      * 23 ) + (this.Khxdt2.GetHashCode() ) 
                      * 23 ) + (this.Khxcoe.GetHashCode() ) 
                      * 23 ) + (this.Khxca1.GetHashCode() ) 
                      * 23 ) + (this.Khxct1.GetHashCode() ) 
                      * 23 ) + (this.Khxcu1?? "").GetHashCode()
                      * 23 ) + (this.Khxcp1.GetHashCode() ) 
                      * 23 ) + (this.Khxcx1?? "").GetHashCode()
                      * 23 ) + (this.Khxca2.GetHashCode() ) 
                      * 23 ) + (this.Khxct2.GetHashCode() ) 
                      * 23 ) + (this.Khxcu2?? "").GetHashCode()
                      * 23 ) + (this.Khxcp2.GetHashCode() ) 
                      * 23 ) + (this.Khxcx2?? "").GetHashCode()
                      * 23 ) + (this.Khxaju.GetHashCode() ) 
                      * 23 ) + (this.Khxlmr.GetHashCode() ) 
                      * 23 ) + (this.Khxmba.GetHashCode() ) 
                      * 23 ) + (this.Khxten.GetHashCode() ) 
                      * 23 ) + (this.Khxhon.GetHashCode() ) 
                      * 23 ) + (this.Khxhox?? "").GetHashCode()
                      * 23 ) + (this.Khxbrg?? "").GetHashCode()
                      * 23 ) + (this.Khxbrl?? "").GetHashCode()
                      * 23 ) + (this.Khxbas.GetHashCode() ) 
                      * 23 ) + (this.Khxbat.GetHashCode() ) 
                      * 23 ) + (this.Khxbau?? "").GetHashCode()
                      * 23 ) + (this.Khxbam.GetHashCode() ) 
                      * 23 ) + (this.Khxxf1?? "").GetHashCode()
                      * 23 ) + (this.Khxxb1.GetHashCode() ) 
                      * 23 ) + (this.Khxxm1.GetHashCode() ) 
                      * 23 ) + (this.Khxxf2?? "").GetHashCode()
                      * 23 ) + (this.Khxxb2.GetHashCode() ) 
                      * 23 ) + (this.Khxxm2.GetHashCode() ) 
                      * 23 ) + (this.Khxxf3?? "").GetHashCode()
                      * 23 ) + (this.Khxxb3.GetHashCode() ) 
                      * 23 ) + (this.Khxxm3.GetHashCode() ) 
                      * 23 ) + (this.Khxreg?? "").GetHashCode()
                      * 23 ) + (this.Khxpei.GetHashCode() ) 
                      * 23 ) + (this.Khxkea.GetHashCode() ) 
                      * 23 ) + (this.Khxpbp.GetHashCode() ) 
                      * 23 ) + (this.Khxktd.GetHashCode() ) 
                      * 23 ) + (this.Khxasv.GetHashCode() ) 
                      * 23 ) + (this.Khxpbt.GetHashCode() ) 
                      * 23 ) + (this.Khxsip.GetHashCode() ) 
                      * 23 ) + (this.Khxpbs.GetHashCode() ) 
                      * 23 ) + (this.Khxris.GetHashCode() ) 
                      * 23 ) + (this.Khxpbr.GetHashCode() ) 
                      * 23 ) + (this.Khxria.GetHashCode() ) 
                      * 23 ) + (this.Khxavn.GetHashCode() )                    );
           }
        }
    }
}
