using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPRGUR
    public partial class KpRguR  {
             //KPRGUR

            ///<summary>Public empty contructor</summary>
            public KpRguR() {}
            ///<summary>Public empty contructor</summary>
            public KpRguR(KpRguR copyFrom) 
            {
                  this.Kilid= copyFrom.Kilid;
                  this.Kilkhwid= copyFrom.Kilkhwid;
                  this.Kiltyp= copyFrom.Kiltyp;
                  this.Kilipb= copyFrom.Kilipb;
                  this.Kilalx= copyFrom.Kilalx;
                  this.Kilrsq= copyFrom.Kilrsq;
                  this.Kildebp= copyFrom.Kildebp;
                  this.Kilfinp= copyFrom.Kilfinp;
                  this.Kilsit= copyFrom.Kilsit;
                  this.Kilmhc= copyFrom.Kilmhc;
                  this.Kilfrc= copyFrom.Kilfrc;
                  this.Kilfr0= copyFrom.Kilfr0;
                  this.Kilmht= copyFrom.Kilmht;
                  this.Kilmtx= copyFrom.Kilmtx;
                  this.Kilttt= copyFrom.Kilttt;
                  this.Kilmtt= copyFrom.Kilmtt;
                  this.Kilcnh= copyFrom.Kilcnh;
                  this.Kilcnt= copyFrom.Kilcnt;
                  this.Kilgrm= copyFrom.Kilgrm;
                  this.Kilpro= copyFrom.Kilpro;
                  this.Kilech= copyFrom.Kilech;
                  this.Kilect= copyFrom.Kilect;
                  this.Kilkea= copyFrom.Kilkea;
                  this.Kilktd= copyFrom.Kilktd;
                  this.Kilasv= copyFrom.Kilasv;
                  this.Kilpbt= copyFrom.Kilpbt;
                  this.Kilmin= copyFrom.Kilmin;
                  this.Kilbrnc= copyFrom.Kilbrnc;
                  this.Kilpba= copyFrom.Kilpba;
                  this.Kilpbs= copyFrom.Kilpbs;
                  this.Kilpbr= copyFrom.Kilpbr;
                  this.Kilpbp= copyFrom.Kilpbp;
                  this.Kilria= copyFrom.Kilria;
                  this.Kilspre= copyFrom.Kilspre;
                  this.Kilriaf= copyFrom.Kilriaf;
                  this.Kilemh= copyFrom.Kilemh;
                  this.Kilemt= copyFrom.Kilemt;
                  this.Kilemhf= copyFrom.Kilemhf;
                  this.Kilemtf= copyFrom.Kilemtf;
                  this.Kilcot= copyFrom.Kilcot;
                  this.Kilbrnt= copyFrom.Kilbrnt;
                  this.Kilschg= copyFrom.Kilschg;
                  this.Kilsidf= copyFrom.Kilsidf;
                  this.Kilsrec= copyFrom.Kilsrec;
                  this.Kilspro= copyFrom.Kilspro;
                  this.Kilsrep= copyFrom.Kilsrep;
                  this.Kilsrim= copyFrom.Kilsrim;
                  this.Kilpbtr= copyFrom.Kilpbtr;
                  this.Kilemd= copyFrom.Kilemd;
                  this.Kilspc= copyFrom.Kilspc;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kilid { get; set; } 
            
            ///<summary> Lien KPRGU   Entête </summary>
            public Int64 Kilkhwid { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Kiltyp { get; set; } 
            
            ///<summary> N° de Police </summary>
            public string Kilipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Kilalx { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Kilrsq { get; set; } 
            
            ///<summary> Début période </summary>
            public int Kildebp { get; set; } 
            
            ///<summary> Fin de période </summary>
            public int Kilfinp { get; set; } 
            
            ///<summary> Code situation  N/A/V </summary>
            public string Kilsit { get; set; } 
            
            ///<summary> Mnt régule HT Hors CN calculé </summary>
            public Decimal Kilmhc { get; set; } 
            
            ///<summary> Mnt régule forcé Forcé O/N </summary>
            public string Kilfrc { get; set; } 
            
            ///<summary> Si forcé forcé à zéro O/N </summary>
            public string Kilfr0 { get; set; } 
            
            ///<summary> Mnt régule HT Hors Catnat </summary>
            public Decimal Kilmht { get; set; } 
            
            ///<summary> Mnt régule Taxes Hors Catnat </summary>
            public Decimal Kilmtx { get; set; } 
            
            ///<summary> Total taxes </summary>
            public Decimal Kilttt { get; set; } 
            
            ///<summary> Mnt régule TTC Hors CN </summary>
            public Decimal Kilmtt { get; set; } 
            
            ///<summary> Mnt régule CATNAT montant HT </summary>
            public Decimal Kilcnh { get; set; } 
            
            ///<summary> Mnt régule CATNAT Montant de taxe </summary>
            public Decimal Kilcnt { get; set; } 
            
            ///<summary> Mnt régule GAREAT Montant HT Compris </summary>
            public Decimal Kilgrm { get; set; } 
            
            ///<summary> Montant prime provisionnelle </summary>
            public Decimal Kilpro { get; set; } 
            
            ///<summary> Déja émis calculé HT </summary>
            public Decimal Kilech { get; set; } 
            
            ///<summary> Déja émis calculé Taxes </summary>
            public Decimal Kilect { get; set; } 
            
            ///<summary> PB Prime émise acquitée </summary>
            public Decimal Kilkea { get; set; } 
            
            ///<summary> PB Prime technique due </summary>
            public Decimal Kilktd { get; set; } 
            
            ///<summary> PB Valeur de l'Assiette </summary>
            public Decimal Kilasv { get; set; } 
            
            ///<summary> taux Appel </summary>
            public Decimal Kilpbt { get; set; } 
            
            ///<summary> Prime Minimum </summary>
            public Decimal Kilmin { get; set; } 
            
            ///<summary> Prime maxi </summary>
            public Decimal Kilbrnc { get; set; } 
            
            ///<summary> PB/BURNER Nombre d'années </summary>
            public int Kilpba { get; set; } 
            
            ///<summary> PB/BURNER : Seuil de rapport S/P </summary>
            public int Kilpbs { get; set; } 
            
            ///<summary> PB/BNS  : %  de ristourne </summary>
            public int Kilpbr { get; set; } 
            
            ///<summary> PB : % de prime retenue </summary>
            public int Kilpbp { get; set; } 
            
            ///<summary> PB : Ristourne anticipée </summary>
            public int Kilria { get; set; } 
            
            ///<summary> Prévisions </summary>
            public Decimal Kilspre { get; set; } 
            
            ///<summary> PB : Ristourne anticipée Forcée </summary>
            public int Kilriaf { get; set; } 
            
            ///<summary> Déjà émis dur la période </summary>
            public Decimal Kilemh { get; set; } 
            
            ///<summary> Déjà émis  Mnt Taxe </summary>
            public Decimal Kilemt { get; set; } 
            
            ///<summary> Déjà émis dur la période  Forcé </summary>
            public Decimal Kilemhf { get; set; } 
            
            ///<summary> Déjà émis  Mnt Taxe Forcé </summary>
            public Decimal Kilemtf { get; set; } 
            
            ///<summary> Cotisation retenue </summary>
            public Decimal Kilcot { get; set; } 
            
            ///<summary> BURNER Taux maxi </summary>
            public Decimal Kilbrnt { get; set; } 
            
            ///<summary> Chargement sinistre </summary>
            public Decimal Kilschg { get; set; } 
            
            ///<summary> Indemnité+Frais </summary>
            public Decimal Kilsidf { get; set; } 
            
            ///<summary> Recours </summary>
            public Decimal Kilsrec { get; set; } 
            
            ///<summary> Provisions </summary>
            public Decimal Kilspro { get; set; } 
            
            ///<summary> Report de charges </summary>
            public Decimal Kilsrep { get; set; } 
            
            ///<summary> Ristourne Anticipée Montant </summary>
            public Decimal Kilsrim { get; set; } 
            
            ///<summary> PB/BNS : Taux appel de prime retenu </summary>
            public int Kilpbtr { get; set; } 
            
            ///<summary> Cotisation due sur la période </summary>
            public Decimal Kilemd { get; set; } 
            
            ///<summary>  S/P retenu </summary>
            public int Kilspc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpRguR  x=this,  y=obj as KpRguR;
            if( y == default(KpRguR) ) return false;
            return (
                    x.Kilid==y.Kilid
                    && x.Kilkhwid==y.Kilkhwid
                    && x.Kiltyp==y.Kiltyp
                    && x.Kilipb==y.Kilipb
                    && x.Kilalx==y.Kilalx
                    && x.Kilrsq==y.Kilrsq
                    && x.Kildebp==y.Kildebp
                    && x.Kilfinp==y.Kilfinp
                    && x.Kilsit==y.Kilsit
                    && x.Kilmhc==y.Kilmhc
                    && x.Kilfrc==y.Kilfrc
                    && x.Kilfr0==y.Kilfr0
                    && x.Kilmht==y.Kilmht
                    && x.Kilmtx==y.Kilmtx
                    && x.Kilttt==y.Kilttt
                    && x.Kilmtt==y.Kilmtt
                    && x.Kilcnh==y.Kilcnh
                    && x.Kilcnt==y.Kilcnt
                    && x.Kilgrm==y.Kilgrm
                    && x.Kilpro==y.Kilpro
                    && x.Kilech==y.Kilech
                    && x.Kilect==y.Kilect
                    && x.Kilkea==y.Kilkea
                    && x.Kilktd==y.Kilktd
                    && x.Kilasv==y.Kilasv
                    && x.Kilpbt==y.Kilpbt
                    && x.Kilmin==y.Kilmin
                    && x.Kilbrnc==y.Kilbrnc
                    && x.Kilpba==y.Kilpba
                    && x.Kilpbs==y.Kilpbs
                    && x.Kilpbr==y.Kilpbr
                    && x.Kilpbp==y.Kilpbp
                    && x.Kilria==y.Kilria
                    && x.Kilspre==y.Kilspre
                    && x.Kilriaf==y.Kilriaf
                    && x.Kilemh==y.Kilemh
                    && x.Kilemt==y.Kilemt
                    && x.Kilemhf==y.Kilemhf
                    && x.Kilemtf==y.Kilemtf
                    && x.Kilcot==y.Kilcot
                    && x.Kilbrnt==y.Kilbrnt
                    && x.Kilschg==y.Kilschg
                    && x.Kilsidf==y.Kilsidf
                    && x.Kilsrec==y.Kilsrec
                    && x.Kilspro==y.Kilspro
                    && x.Kilsrep==y.Kilsrep
                    && x.Kilsrim==y.Kilsrim
                    && x.Kilpbtr==y.Kilpbtr
                    && x.Kilemd==y.Kilemd
                    && x.Kilspc==y.Kilspc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kilid.GetHashCode() ) 
                      * 23 ) + (this.Kilkhwid.GetHashCode() ) 
                      * 23 ) + (this.Kiltyp?? "").GetHashCode()
                      * 23 ) + (this.Kilipb?? "").GetHashCode()
                      * 23 ) + (this.Kilalx.GetHashCode() ) 
                      * 23 ) + (this.Kilrsq.GetHashCode() ) 
                      * 23 ) + (this.Kildebp.GetHashCode() ) 
                      * 23 ) + (this.Kilfinp.GetHashCode() ) 
                      * 23 ) + (this.Kilsit?? "").GetHashCode()
                      * 23 ) + (this.Kilmhc.GetHashCode() ) 
                      * 23 ) + (this.Kilfrc?? "").GetHashCode()
                      * 23 ) + (this.Kilfr0?? "").GetHashCode()
                      * 23 ) + (this.Kilmht.GetHashCode() ) 
                      * 23 ) + (this.Kilmtx.GetHashCode() ) 
                      * 23 ) + (this.Kilttt.GetHashCode() ) 
                      * 23 ) + (this.Kilmtt.GetHashCode() ) 
                      * 23 ) + (this.Kilcnh.GetHashCode() ) 
                      * 23 ) + (this.Kilcnt.GetHashCode() ) 
                      * 23 ) + (this.Kilgrm.GetHashCode() ) 
                      * 23 ) + (this.Kilpro.GetHashCode() ) 
                      * 23 ) + (this.Kilech.GetHashCode() ) 
                      * 23 ) + (this.Kilect.GetHashCode() ) 
                      * 23 ) + (this.Kilkea.GetHashCode() ) 
                      * 23 ) + (this.Kilktd.GetHashCode() ) 
                      * 23 ) + (this.Kilasv.GetHashCode() ) 
                      * 23 ) + (this.Kilpbt.GetHashCode() ) 
                      * 23 ) + (this.Kilmin.GetHashCode() ) 
                      * 23 ) + (this.Kilbrnc.GetHashCode() ) 
                      * 23 ) + (this.Kilpba.GetHashCode() ) 
                      * 23 ) + (this.Kilpbs.GetHashCode() ) 
                      * 23 ) + (this.Kilpbr.GetHashCode() ) 
                      * 23 ) + (this.Kilpbp.GetHashCode() ) 
                      * 23 ) + (this.Kilria.GetHashCode() ) 
                      * 23 ) + (this.Kilspre.GetHashCode() ) 
                      * 23 ) + (this.Kilriaf.GetHashCode() ) 
                      * 23 ) + (this.Kilemh.GetHashCode() ) 
                      * 23 ) + (this.Kilemt.GetHashCode() ) 
                      * 23 ) + (this.Kilemhf.GetHashCode() ) 
                      * 23 ) + (this.Kilemtf.GetHashCode() ) 
                      * 23 ) + (this.Kilcot.GetHashCode() ) 
                      * 23 ) + (this.Kilbrnt.GetHashCode() ) 
                      * 23 ) + (this.Kilschg.GetHashCode() ) 
                      * 23 ) + (this.Kilsidf.GetHashCode() ) 
                      * 23 ) + (this.Kilsrec.GetHashCode() ) 
                      * 23 ) + (this.Kilspro.GetHashCode() ) 
                      * 23 ) + (this.Kilsrep.GetHashCode() ) 
                      * 23 ) + (this.Kilsrim.GetHashCode() ) 
                      * 23 ) + (this.Kilpbtr.GetHashCode() ) 
                      * 23 ) + (this.Kilemd.GetHashCode() ) 
                      * 23 ) + (this.Kilspc.GetHashCode() )                    );
           }
        }
    }
}
