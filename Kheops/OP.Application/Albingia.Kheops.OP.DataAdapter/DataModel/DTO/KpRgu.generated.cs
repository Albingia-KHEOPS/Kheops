using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KPRGU
    public partial class KpRgu  {
             //KPRGU

            ///<summary>Public empty contructor</summary>
            public KpRgu() {}
            ///<summary>Public empty contructor</summary>
            public KpRgu(KpRgu copyFrom) 
            {
                  this.Khwid= copyFrom.Khwid;
                  this.Khwtyp= copyFrom.Khwtyp;
                  this.Khwipb= copyFrom.Khwipb;
                  this.Khwalx= copyFrom.Khwalx;
                  this.Khwttr= copyFrom.Khwttr;
                  this.Khwrgav= copyFrom.Khwrgav;
                  this.Khwavn= copyFrom.Khwavn;
                  this.Khwavnd= copyFrom.Khwavnd;
                  this.Khwexe= copyFrom.Khwexe;
                  this.Khwdebp= copyFrom.Khwdebp;
                  this.Khwfinp= copyFrom.Khwfinp;
                  this.Khwtrgu= copyFrom.Khwtrgu;
                  this.Khwipk= copyFrom.Khwipk;
                  this.Khwict= copyFrom.Khwict;
                  this.Khwicc= copyFrom.Khwicc;
                  this.Khwxcm= copyFrom.Khwxcm;
                  this.Khwcnc= copyFrom.Khwcnc;
                  this.Khwenc= copyFrom.Khwenc;
                  this.Khwafc= copyFrom.Khwafc;
                  this.Khwafr= copyFrom.Khwafr;
                  this.Khwatt= copyFrom.Khwatt;
                  this.Khwmht= copyFrom.Khwmht;
                  this.Khwcnh= copyFrom.Khwcnh;
                  this.Khwgrg= copyFrom.Khwgrg;
                  this.Khwttt= copyFrom.Khwttt;
                  this.Khwmtt= copyFrom.Khwmtt;
                  this.Khweta= copyFrom.Khweta;
                  this.Khwsit= copyFrom.Khwsit;
                  this.Khwstu= copyFrom.Khwstu;
                  this.Khwstd= copyFrom.Khwstd;
                  this.Khwsth= copyFrom.Khwsth;
                  this.Khwcru= copyFrom.Khwcru;
                  this.Khwcrd= copyFrom.Khwcrd;
                  this.Khwmaju= copyFrom.Khwmaju;
                  this.Khwmajd= copyFrom.Khwmajd;
                  this.Khwavp= copyFrom.Khwavp;
                  this.Khwdesi= copyFrom.Khwdesi;
                  this.Khwobsv= copyFrom.Khwobsv;
                  this.Khwobsc= copyFrom.Khwobsc;
                  this.Khwmtf= copyFrom.Khwmtf;
                  this.Khwmrg= copyFrom.Khwmrg;
                  this.Khwacc= copyFrom.Khwacc;
                  this.Khwsuav= copyFrom.Khwsuav;
                  this.Khwnem= copyFrom.Khwnem;
                  this.Khwasv= copyFrom.Khwasv;
                  this.Khwmin= copyFrom.Khwmin;
                  this.Khwbrnc= copyFrom.Khwbrnc;
                  this.Khwpbt= copyFrom.Khwpbt;
                  this.Khwpba= copyFrom.Khwpba;
                  this.Khwpbs= copyFrom.Khwpbs;
                  this.Khwpbr= copyFrom.Khwpbr;
                  this.Khwpbp= copyFrom.Khwpbp;
                  this.Khwria= copyFrom.Khwria;
                  this.Khwriaf= copyFrom.Khwriaf;
                  this.Khwemh= copyFrom.Khwemh;
                  this.Khwemhf= copyFrom.Khwemhf;
                  this.Khwcot= copyFrom.Khwcot;
                  this.Khwbrnt= copyFrom.Khwbrnt;
                  this.Khwschg= copyFrom.Khwschg;
                  this.Khwsidf= copyFrom.Khwsidf;
                  this.Khwsrec= copyFrom.Khwsrec;
                  this.Khwspro= copyFrom.Khwspro;
                  this.Khwspre= copyFrom.Khwspre;
                  this.Khwsrep= copyFrom.Khwsrep;
                  this.Khwsrim= copyFrom.Khwsrim;
                  this.Khwmhc= copyFrom.Khwmhc;
                  this.Khwpbtr= copyFrom.Khwpbtr;
                  this.Khwemd= copyFrom.Khwemd;
                  this.Khwspc= copyFrom.Khwspc;
                  this.Khwmtx= copyFrom.Khwmtx;
                  this.Khwcnt= copyFrom.Khwcnt;
                  this.Khwect= copyFrom.Khwect;
                  this.Khwemt= copyFrom.Khwemt;
        
            }        
            
            ///<summary> ID Unique </summary>
            public Int64 Khwid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Khwtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Khwipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Khwalx { get; set; } 
            
            ///<summary> Type de traitement (Affnouv/avenant) </summary>
            public string Khwttr { get; set; } 
            
            ///<summary> La régularisation est un avenant O/N </summary>
            public string Khwrgav { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Khwavn { get; set; } 
            
            ///<summary> Date avenant </summary>
            public int Khwavnd { get; set; } 
            
            ///<summary> Exercice </summary>
            public int Khwexe { get; set; } 
            
            ///<summary> Début période </summary>
            public int Khwdebp { get; set; } 
            
            ///<summary> Fin de période </summary>
            public int Khwfinp { get; set; } 
            
            ///<summary> Typologie  de régularisation </summary>
            public string Khwtrgu { get; set; } 
            
            ///<summary> N° de prime de régularisation </summary>
            public int Khwipk { get; set; } 
            
            ///<summary> Id courtier  Adressé à </summary>
            public int Khwict { get; set; } 
            
            ///<summary> Id courtier Commission </summary>
            public int Khwicc { get; set; } 
            
            ///<summary> Taux de commission </summary>
            public Decimal Khwxcm { get; set; } 
            
            ///<summary> Taux de commission Cat nat </summary>
            public Decimal Khwcnc { get; set; } 
            
            ///<summary> Code encaissement </summary>
            public string Khwenc { get; set; } 
            
            ///<summary> Application Frais Accessoire </summary>
            public string Khwafc { get; set; } 
            
            ///<summary> Montant de frais accessoires </summary>
            public int Khwafr { get; set; } 
            
            ///<summary> Application Taxes Attentat </summary>
            public string Khwatt { get; set; } 
            
            ///<summary> Montant prime HT (y compris CATNAT) </summary>
            public Decimal Khwmht { get; set; } 
            
            ///<summary> CATNAT Montant  HT </summary>
            public Decimal Khwcnh { get; set; } 
            
            ///<summary> GAREAT Montant HT </summary>
            public Decimal Khwgrg { get; set; } 
            
            ///<summary> Total taxes </summary>
            public Decimal Khwttt { get; set; } 
            
            ///<summary> Montant prime TTC </summary>
            public Decimal Khwmtt { get; set; } 
            
            ///<summary> Etat N/A/V </summary>
            public string Khweta { get; set; } 
            
            ///<summary> Situation </summary>
            public string Khwsit { get; set; } 
            
            ///<summary> Situation User </summary>
            public string Khwstu { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Khwstd { get; set; } 
            
            ///<summary> Situation  heure </summary>
            public int Khwsth { get; set; } 
            
            ///<summary> Création User </summary>
            public string Khwcru { get; set; } 
            
            ///<summary> Création date </summary>
            public int Khwcrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Khwmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Khwmajd { get; set; } 
            
            ///<summary> N° avenant le + récent de le période </summary>
            public int Khwavp { get; set; } 
            
            ///<summary> Lien KPDESI Désignation </summary>
            public Int64 Khwdesi { get; set; } 
            
            ///<summary> Lien KPOBSV </summary>
            public Int64 Khwobsv { get; set; } 
            
            ///<summary> Observation Cotisation Lien KPOBSV </summary>
            public Int64 Khwobsc { get; set; } 
            
            ///<summary> Motif de régularisation </summary>
            public string Khwmtf { get; set; } 
            
            ///<summary> Mode de régularisation </summary>
            public string Khwmrg { get; set; } 
            
            ///<summary> Accès niveau  de régularisation  E Entête R Risque </summary>
            public string Khwacc { get; set; } 
            
            ///<summary> Suivi d'un avenant de modification O/N </summary>
            public string Khwsuav { get; set; } 
            
            ///<summary> Top : Sans émission Révision seule </summary>
            public string Khwnem { get; set; } 
            
            ///<summary> Assiette </summary>
            public Decimal Khwasv { get; set; } 
            
            ///<summary> Prime Minimum </summary>
            public Decimal Khwmin { get; set; } 
            
            ///<summary> Prime maxi </summary>
            public Decimal Khwbrnc { get; set; } 
            
            ///<summary> PB/BNS : Taux appel de prime </summary>
            public int Khwpbt { get; set; } 
            
            ///<summary> PB/BURNER Nombre d'années </summary>
            public int Khwpba { get; set; } 
            
            ///<summary> PB/BURNER : Seuil de rapport S/P </summary>
            public int Khwpbs { get; set; } 
            
            ///<summary> PB/BNS  : %  de ristourne </summary>
            public int Khwpbr { get; set; } 
            
            ///<summary> PB : % de prime retenue </summary>
            public int Khwpbp { get; set; } 
            
            ///<summary> PB : % Ristourne anticipée </summary>
            public int Khwria { get; set; } 
            
            ///<summary> PB : Ristourne anticipée Forcée </summary>
            public int Khwriaf { get; set; } 
            
            ///<summary> Déja émis dur la période </summary>
            public Decimal Khwemh { get; set; } 
            
            ///<summary> Déja émis dur la période  Forcé </summary>
            public Decimal Khwemhf { get; set; } 
            
            ///<summary> Cotisation retenue </summary>
            public Decimal Khwcot { get; set; } 
            
            ///<summary> BURNER Taux maxi </summary>
            public Decimal Khwbrnt { get; set; } 
            
            ///<summary> Chargement sinistre </summary>
            public Decimal Khwschg { get; set; } 
            
            ///<summary> Indemnité+Frais </summary>
            public Decimal Khwsidf { get; set; } 
            
            ///<summary> Recours </summary>
            public Decimal Khwsrec { get; set; } 
            
            ///<summary> Provisions </summary>
            public Decimal Khwspro { get; set; } 
            
            ///<summary> Prévisions </summary>
            public Decimal Khwspre { get; set; } 
            
            ///<summary> Report de charges </summary>
            public Decimal Khwsrep { get; set; } 
            
            ///<summary> Ristourne Anticipée Montant </summary>
            public Decimal Khwsrim { get; set; } 
            
            ///<summary> Montant HT Calculé </summary>
            public Decimal Khwmhc { get; set; } 
            
            ///<summary> PB/BNS : Taux appel de prime retenu </summary>
            public int Khwpbtr { get; set; } 
            
            ///<summary> Cotisation due sur la période </summary>
            public Decimal Khwemd { get; set; } 
            
            ///<summary>  S/P retenu </summary>
            public int Khwspc { get; set; } 
            
            ///<summary> Mnt régule Taxes Hors Catnat </summary>
            public Decimal Khwmtx { get; set; } 
            
            ///<summary> Mnt régule CATNAT Montant de taxe </summary>
            public Decimal Khwcnt { get; set; } 
            
            ///<summary> Déjà émis calculé Taxes </summary>
            public Decimal Khwect { get; set; } 
            
            ///<summary> Déjà émis retenu Mnt Taxes </summary>
            public Decimal Khwemt { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpRgu  x=this,  y=obj as KpRgu;
            if( y == default(KpRgu) ) return false;
            return (
                    x.Khwid==y.Khwid
                    && x.Khwtyp==y.Khwtyp
                    && x.Khwipb==y.Khwipb
                    && x.Khwalx==y.Khwalx
                    && x.Khwttr==y.Khwttr
                    && x.Khwrgav==y.Khwrgav
                    && x.Khwavn==y.Khwavn
                    && x.Khwavnd==y.Khwavnd
                    && x.Khwexe==y.Khwexe
                    && x.Khwdebp==y.Khwdebp
                    && x.Khwfinp==y.Khwfinp
                    && x.Khwtrgu==y.Khwtrgu
                    && x.Khwipk==y.Khwipk
                    && x.Khwict==y.Khwict
                    && x.Khwicc==y.Khwicc
                    && x.Khwxcm==y.Khwxcm
                    && x.Khwcnc==y.Khwcnc
                    && x.Khwenc==y.Khwenc
                    && x.Khwafc==y.Khwafc
                    && x.Khwafr==y.Khwafr
                    && x.Khwatt==y.Khwatt
                    && x.Khwmht==y.Khwmht
                    && x.Khwcnh==y.Khwcnh
                    && x.Khwgrg==y.Khwgrg
                    && x.Khwttt==y.Khwttt
                    && x.Khwmtt==y.Khwmtt
                    && x.Khweta==y.Khweta
                    && x.Khwsit==y.Khwsit
                    && x.Khwstu==y.Khwstu
                    && x.Khwstd==y.Khwstd
                    && x.Khwsth==y.Khwsth
                    && x.Khwcru==y.Khwcru
                    && x.Khwcrd==y.Khwcrd
                    && x.Khwmaju==y.Khwmaju
                    && x.Khwmajd==y.Khwmajd
                    && x.Khwavp==y.Khwavp
                    && x.Khwdesi==y.Khwdesi
                    && x.Khwobsv==y.Khwobsv
                    && x.Khwobsc==y.Khwobsc
                    && x.Khwmtf==y.Khwmtf
                    && x.Khwmrg==y.Khwmrg
                    && x.Khwacc==y.Khwacc
                    && x.Khwsuav==y.Khwsuav
                    && x.Khwnem==y.Khwnem
                    && x.Khwasv==y.Khwasv
                    && x.Khwmin==y.Khwmin
                    && x.Khwbrnc==y.Khwbrnc
                    && x.Khwpbt==y.Khwpbt
                    && x.Khwpba==y.Khwpba
                    && x.Khwpbs==y.Khwpbs
                    && x.Khwpbr==y.Khwpbr
                    && x.Khwpbp==y.Khwpbp
                    && x.Khwria==y.Khwria
                    && x.Khwriaf==y.Khwriaf
                    && x.Khwemh==y.Khwemh
                    && x.Khwemhf==y.Khwemhf
                    && x.Khwcot==y.Khwcot
                    && x.Khwbrnt==y.Khwbrnt
                    && x.Khwschg==y.Khwschg
                    && x.Khwsidf==y.Khwsidf
                    && x.Khwsrec==y.Khwsrec
                    && x.Khwspro==y.Khwspro
                    && x.Khwspre==y.Khwspre
                    && x.Khwsrep==y.Khwsrep
                    && x.Khwsrim==y.Khwsrim
                    && x.Khwmhc==y.Khwmhc
                    && x.Khwpbtr==y.Khwpbtr
                    && x.Khwemd==y.Khwemd
                    && x.Khwspc==y.Khwspc
                    && x.Khwmtx==y.Khwmtx
                    && x.Khwcnt==y.Khwcnt
                    && x.Khwect==y.Khwect
                    && x.Khwemt==y.Khwemt  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Khwid.GetHashCode() ) 
                      * 23 ) + (this.Khwtyp?? "").GetHashCode()
                      * 23 ) + (this.Khwipb?? "").GetHashCode()
                      * 23 ) + (this.Khwalx.GetHashCode() ) 
                      * 23 ) + (this.Khwttr?? "").GetHashCode()
                      * 23 ) + (this.Khwrgav?? "").GetHashCode()
                      * 23 ) + (this.Khwavn.GetHashCode() ) 
                      * 23 ) + (this.Khwavnd.GetHashCode() ) 
                      * 23 ) + (this.Khwexe.GetHashCode() ) 
                      * 23 ) + (this.Khwdebp.GetHashCode() ) 
                      * 23 ) + (this.Khwfinp.GetHashCode() ) 
                      * 23 ) + (this.Khwtrgu?? "").GetHashCode()
                      * 23 ) + (this.Khwipk.GetHashCode() ) 
                      * 23 ) + (this.Khwict.GetHashCode() ) 
                      * 23 ) + (this.Khwicc.GetHashCode() ) 
                      * 23 ) + (this.Khwxcm.GetHashCode() ) 
                      * 23 ) + (this.Khwcnc.GetHashCode() ) 
                      * 23 ) + (this.Khwenc?? "").GetHashCode()
                      * 23 ) + (this.Khwafc?? "").GetHashCode()
                      * 23 ) + (this.Khwafr.GetHashCode() ) 
                      * 23 ) + (this.Khwatt?? "").GetHashCode()
                      * 23 ) + (this.Khwmht.GetHashCode() ) 
                      * 23 ) + (this.Khwcnh.GetHashCode() ) 
                      * 23 ) + (this.Khwgrg.GetHashCode() ) 
                      * 23 ) + (this.Khwttt.GetHashCode() ) 
                      * 23 ) + (this.Khwmtt.GetHashCode() ) 
                      * 23 ) + (this.Khweta?? "").GetHashCode()
                      * 23 ) + (this.Khwsit?? "").GetHashCode()
                      * 23 ) + (this.Khwstu?? "").GetHashCode()
                      * 23 ) + (this.Khwstd.GetHashCode() ) 
                      * 23 ) + (this.Khwsth.GetHashCode() ) 
                      * 23 ) + (this.Khwcru?? "").GetHashCode()
                      * 23 ) + (this.Khwcrd.GetHashCode() ) 
                      * 23 ) + (this.Khwmaju?? "").GetHashCode()
                      * 23 ) + (this.Khwmajd.GetHashCode() ) 
                      * 23 ) + (this.Khwavp.GetHashCode() ) 
                      * 23 ) + (this.Khwdesi.GetHashCode() ) 
                      * 23 ) + (this.Khwobsv.GetHashCode() ) 
                      * 23 ) + (this.Khwobsc.GetHashCode() ) 
                      * 23 ) + (this.Khwmtf?? "").GetHashCode()
                      * 23 ) + (this.Khwmrg?? "").GetHashCode()
                      * 23 ) + (this.Khwacc?? "").GetHashCode()
                      * 23 ) + (this.Khwsuav?? "").GetHashCode()
                      * 23 ) + (this.Khwnem?? "").GetHashCode()
                      * 23 ) + (this.Khwasv.GetHashCode() ) 
                      * 23 ) + (this.Khwmin.GetHashCode() ) 
                      * 23 ) + (this.Khwbrnc.GetHashCode() ) 
                      * 23 ) + (this.Khwpbt.GetHashCode() ) 
                      * 23 ) + (this.Khwpba.GetHashCode() ) 
                      * 23 ) + (this.Khwpbs.GetHashCode() ) 
                      * 23 ) + (this.Khwpbr.GetHashCode() ) 
                      * 23 ) + (this.Khwpbp.GetHashCode() ) 
                      * 23 ) + (this.Khwria.GetHashCode() ) 
                      * 23 ) + (this.Khwriaf.GetHashCode() ) 
                      * 23 ) + (this.Khwemh.GetHashCode() ) 
                      * 23 ) + (this.Khwemhf.GetHashCode() ) 
                      * 23 ) + (this.Khwcot.GetHashCode() ) 
                      * 23 ) + (this.Khwbrnt.GetHashCode() ) 
                      * 23 ) + (this.Khwschg.GetHashCode() ) 
                      * 23 ) + (this.Khwsidf.GetHashCode() ) 
                      * 23 ) + (this.Khwsrec.GetHashCode() ) 
                      * 23 ) + (this.Khwspro.GetHashCode() ) 
                      * 23 ) + (this.Khwspre.GetHashCode() ) 
                      * 23 ) + (this.Khwsrep.GetHashCode() ) 
                      * 23 ) + (this.Khwsrim.GetHashCode() ) 
                      * 23 ) + (this.Khwmhc.GetHashCode() ) 
                      * 23 ) + (this.Khwpbtr.GetHashCode() ) 
                      * 23 ) + (this.Khwemd.GetHashCode() ) 
                      * 23 ) + (this.Khwspc.GetHashCode() ) 
                      * 23 ) + (this.Khwmtx.GetHashCode() ) 
                      * 23 ) + (this.Khwcnt.GetHashCode() ) 
                      * 23 ) + (this.Khwect.GetHashCode() ) 
                      * 23 ) + (this.Khwemt.GetHashCode() )                    );
           }
        }
    }
}
