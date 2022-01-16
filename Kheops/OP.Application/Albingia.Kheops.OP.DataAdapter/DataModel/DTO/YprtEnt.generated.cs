using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTENT
    public partial class YprtEnt  {
             //YHRTENT
             //YPRTENT

            ///<summary>Public empty contructor</summary>
            public YprtEnt() {}
            ///<summary>Public empty contructor</summary>
            public YprtEnt(YprtEnt copyFrom) 
            {
                  this.Jdipb= copyFrom.Jdipb;
                  this.Jdalx= copyFrom.Jdalx;
                  this.Jdavn= copyFrom.Jdavn;
                  this.Jdhin= copyFrom.Jdhin;
                  this.Jdsht= copyFrom.Jdsht;
                  this.Jdenc= copyFrom.Jdenc;
                  this.Jditc= copyFrom.Jditc;
                  this.Jdval= copyFrom.Jdval;
                  this.Jdvaa= copyFrom.Jdvaa;
                  this.Jdvaw= copyFrom.Jdvaw;
                  this.Jdvat= copyFrom.Jdvat;
                  this.Jdvau= copyFrom.Jdvau;
                  this.Jdvah= copyFrom.Jdvah;
                  this.Jddrq= copyFrom.Jddrq;
                  this.Jdnbr= copyFrom.Jdnbr;
                  this.Jdtxl= copyFrom.Jdtxl;
                  this.Jdtrr= copyFrom.Jdtrr;
                  this.Jdxcm= copyFrom.Jdxcm;
                  this.Jdnex= copyFrom.Jdnex;
                  this.Jdnpa= copyFrom.Jdnpa;
                  this.Jdafc= copyFrom.Jdafc;
                  this.Jdafr= copyFrom.Jdafr;
                  this.Jdatt= copyFrom.Jdatt;
                  this.Jdcna= copyFrom.Jdcna;
                  this.Jdcnc= copyFrom.Jdcnc;
                  this.Jdina= copyFrom.Jdina;
                  this.Jdind= copyFrom.Jdind;
                  this.Jdixc= copyFrom.Jdixc;
                  this.Jdixf= copyFrom.Jdixf;
                  this.Jdixl= copyFrom.Jdixl;
                  this.Jdixp= copyFrom.Jdixp;
                  this.Jdivo= copyFrom.Jdivo;
                  this.Jdiva= copyFrom.Jdiva;
                  this.Jdivw= copyFrom.Jdivw;
                  this.Jdmht= copyFrom.Jdmht;
                  this.Jdrea= copyFrom.Jdrea;
                  this.Jdreb= copyFrom.Jdreb;
                  this.Jdreh= copyFrom.Jdreh;
                  this.Jddpv= copyFrom.Jddpv;
                  this.Jdgau= copyFrom.Jdgau;
                  this.Jdgvl= copyFrom.Jdgvl;
                  this.Jdgun= copyFrom.Jdgun;
                  this.Jdpbn= copyFrom.Jdpbn;
                  this.Jdpbs= copyFrom.Jdpbs;
                  this.Jdpbr= copyFrom.Jdpbr;
                  this.Jdpbt= copyFrom.Jdpbt;
                  this.Jdpbc= copyFrom.Jdpbc;
                  this.Jdpbp= copyFrom.Jdpbp;
                  this.Jdpba= copyFrom.Jdpba;
                  this.Jdrcg= copyFrom.Jdrcg;
                  this.Jdccg= copyFrom.Jdccg;
                  this.Jdrcs= copyFrom.Jdrcs;
                  this.Jdccs= copyFrom.Jdccs;
                  this.Jdclv= copyFrom.Jdclv;
                  this.Jdagm= copyFrom.Jdagm;
                  this.Jdlcv= copyFrom.Jdlcv;
                  this.Jdlca= copyFrom.Jdlca;
                  this.Jdlcw= copyFrom.Jdlcw;
                  this.Jdlcu= copyFrom.Jdlcu;
                  this.Jdlce= copyFrom.Jdlce;
                  this.Jddpa= copyFrom.Jddpa;
                  this.Jddpm= copyFrom.Jddpm;
                  this.Jddpj= copyFrom.Jddpj;
                  this.Jdfpa= copyFrom.Jdfpa;
                  this.Jdfpm= copyFrom.Jdfpm;
                  this.Jdfpj= copyFrom.Jdfpj;
                  this.Jdpea= copyFrom.Jdpea;
                  this.Jdpem= copyFrom.Jdpem;
                  this.Jdpej= copyFrom.Jdpej;
                  this.Jdacq= copyFrom.Jdacq;
                  this.Jdtmc= copyFrom.Jdtmc;
                  this.Jdtfo= copyFrom.Jdtfo;
                  this.Jdtft= copyFrom.Jdtft;
                  this.Jdtff= copyFrom.Jdtff;
                  this.Jdtfp= copyFrom.Jdtfp;
                  this.Jdpro= copyFrom.Jdpro;
                  this.Jdtmi= copyFrom.Jdtmi;
                  this.Jdtfm= copyFrom.Jdtfm;
                  this.Jdtma= copyFrom.Jdtma;
                  this.Jdtmg= copyFrom.Jdtmg;
                  this.Jdcmc= copyFrom.Jdcmc;
                  this.Jdcfo= copyFrom.Jdcfo;
                  this.Jdcft= copyFrom.Jdcft;
                  this.Jdcfh= copyFrom.Jdcfh;
                  this.Jdcht= copyFrom.Jdcht;
                  this.Jdctt= copyFrom.Jdctt;
                  this.Jdccp= copyFrom.Jdccp;
                  this.Jdehh= copyFrom.Jdehh;
                  this.Jdehc= copyFrom.Jdehc;
                  this.Jdsmp= copyFrom.Jdsmp;
                  this.Jdivx= copyFrom.Jdivx;
                  this.Jdtcr= copyFrom.Jdtcr;
                  this.Jdnpg= copyFrom.Jdnpg;
                  this.Jdedi= copyFrom.Jdedi;
                  this.Jdedn= copyFrom.Jdedn;
                  this.Jdeda= copyFrom.Jdeda;
                  this.Jdedm= copyFrom.Jdedm;
                  this.Jdedj= copyFrom.Jdedj;
                  this.Jdehi= copyFrom.Jdehi;
                  this.Jdiax= copyFrom.Jdiax;
                  this.Jdted= copyFrom.Jdted;
                  this.Jddoo= copyFrom.Jddoo;
                  this.Jdrua= copyFrom.Jdrua;
                  this.Jdrum= copyFrom.Jdrum;
                  this.Jdruj= copyFrom.Jdruj;
                  this.Jdecg= copyFrom.Jdecg;
                  this.Jdecp= copyFrom.Jdecp;
                  this.Jdapt= copyFrom.Jdapt;
                  this.Jdapr= copyFrom.Jdapr;
                  this.Jdaat= copyFrom.Jdaat;
                  this.Jdaar= copyFrom.Jdaar;
                  this.Jdacr= copyFrom.Jdacr;
                  this.Jdacv= copyFrom.Jdacv;
                  this.Jdaxt= copyFrom.Jdaxt;
                  this.Jdaxc= copyFrom.Jdaxc;
                  this.Jdaxf= copyFrom.Jdaxf;
                  this.Jdaxm= copyFrom.Jdaxm;
                  this.Jdaxg= copyFrom.Jdaxg;
                  this.Jdata= copyFrom.Jdata;
                  this.Jdatx= copyFrom.Jdatx;
                  this.Jdaut= copyFrom.Jdaut;
                  this.Jdauf= copyFrom.Jdauf;
                  this.Jdlta= copyFrom.Jdlta;
                  this.Jdltasp= copyFrom.Jdltasp;
                  this.Jdldeb= copyFrom.Jdldeb;
                  this.Jdldeh= copyFrom.Jdldeh;
                  this.Jdlfin= copyFrom.Jdlfin;
                  this.Jdlfih= copyFrom.Jdlfih;
                  this.Jdldur= copyFrom.Jdldur;
                  this.Jdlduu= copyFrom.Jdlduu;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Jdipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Jdalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Jdavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Jdhin { get; set; } 
            
            ///<summary> Saisie HT ou TTC (H/T) </summary>
            public string Jdsht { get; set; } 
            
            ///<summary> Code encaissement </summary>
            public string Jdenc { get; set; } 
            
            ///<summary> Intercalaire courtier (O/N) </summary>
            public string Jditc { get; set; } 
            
            ///<summary> Valeur Totale de la police Origine </summary>
            public Int64 Jdval { get; set; } 
            
            ///<summary> Valeur totale Actualisée police </summary>
            public Int64 Jdvaa { get; set; } 
            
            ///<summary> W. Valeur totale Police (travail) </summary>
            public Int64 Jdvaw { get; set; } 
            
            ///<summary> Type de valeur </summary>
            public string Jdvat { get; set; } 
            
            ///<summary> Unité valeur totale </summary>
            public string Jdvau { get; set; } 
            
            ///<summary> HT ou TTC   H/T </summary>
            public string Jdvah { get; set; } 
            
            ///<summary> Dernier N° de risque </summary>
            public int Jddrq { get; set; } 
            
            ///<summary> Nombre de risques </summary>
            public int Jdnbr { get; set; } 
            
            ///<summary> N° Entête libre </summary>
            public Int64 Jdtxl { get; set; } 
            
            ///<summary> Code territorialité </summary>
            public string Jdtrr { get; set; } 
            
            ///<summary> Taux de commission </summary>
            public Decimal Jdxcm { get; set; } 
            
            ///<summary> Nombre d'exemplaires  CP </summary>
            public int Jdnex { get; set; } 
            
            ///<summary> Nbr de page CP supplémentaires </summary>
            public int Jdnpa { get; set; } 
            
            ///<summary> Application frais accessoire O/N </summary>
            public string Jdafc { get; set; } 
            
            ///<summary> Montant de frais accessoires </summary>
            public int Jdafr { get; set; } 
            
            ///<summary> Application taxe Attentat </summary>
            public string Jdatt { get; set; } 
            
            ///<summary> Application des CATNAT (O/N) </summary>
            public string Jdcna { get; set; } 
            
            ///<summary> Taux de commission CATNAT </summary>
            public Decimal Jdcnc { get; set; } 
            
            ///<summary> Indexation (O /N /P partielle) </summary>
            public string Jdina { get; set; } 
            
            ///<summary> Code indice </summary>
            public string Jdind { get; set; } 
            
            ///<summary> Indexation des capitaux (O/N) </summary>
            public string Jdixc { get; set; } 
            
            ///<summary> Indexation des franchises </summary>
            public string Jdixf { get; set; } 
            
            ///<summary> Indexation de la LCI (O/N) </summary>
            public string Jdixl { get; set; } 
            
            ///<summary> Indexation des primes (O/N) </summary>
            public string Jdixp { get; set; } 
            
            ///<summary> Valeur de l'indice origine </summary>
            public Decimal Jdivo { get; set; } 
            
            ///<summary> Valeur de l'indice Actualisé </summary>
            public Decimal Jdiva { get; set; } 
            
            ///<summary> W.  Valeur de l'indice (Travail) </summary>
            public Decimal Jdivw { get; set; } 
            
            ///<summary> Montant total police HT </summary>
            public Decimal Jdmht { get; set; } 
            
            ///<summary> Réassurance O/N </summary>
            public string Jdrea { get; set; } 
            
            ///<summary> Base de réassurance </summary>
            public Int64 Jdreb { get; set; } 
            
            ///<summary> Base réassurance HT ou TTC (H/T) </summary>
            public string Jdreh { get; set; } 
            
            ///<summary> Durée de préavis résiliation (mois) </summary>
            public int Jddpv { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public string Jdgau { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdgvl { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public string Jdgun { get; set; } 
            
            ///<summary> Participation bénéficiaire </summary>
            public string Jdpbn { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdpbs { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdpbr { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdpbt { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdpbc { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdpbp { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdpba { get; set; } 
            
            ///<summary> Référence Conditions générales </summary>
            public string Jdrcg { get; set; } 
            
            ///<summary> Code référence CG </summary>
            public string Jdccg { get; set; } 
            
            ///<summary> Référence Conventions spéciales </summary>
            public string Jdrcs { get; set; } 
            
            ///<summary> Code référence CS </summary>
            public string Jdccs { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public string Jdclv { get; set; } 
            
            ///<summary> Non utilisé </summary>
            public int Jdagm { get; set; } 
            
            ///<summary> LCI : Valeur </summary>
            public Int64 Jdlcv { get; set; } 
            
            ///<summary> LCI : Valeur actualisée </summary>
            public Int64 Jdlca { get; set; } 
            
            ///<summary> W. LCI : Valeur de travail </summary>
            public Int64 Jdlcw { get; set; } 
            
            ///<summary> LCI : Unité </summary>
            public string Jdlcu { get; set; } 
            
            ///<summary> Expression complexe LCI </summary>
            public string Jdlce { get; set; } 
            
            ///<summary> Début de période : Année </summary>
            public int Jddpa { get; set; } 
            
            ///<summary> Début de période : Mois </summary>
            public int Jddpm { get; set; } 
            
            ///<summary> Début de période : Jour </summary>
            public int Jddpj { get; set; } 
            
            ///<summary> Fin de période   : Année </summary>
            public int Jdfpa { get; set; } 
            
            ///<summary> Fin de période   : Mois </summary>
            public int Jdfpm { get; set; } 
            
            ///<summary> Fin de période   : Jour </summary>
            public int Jdfpj { get; set; } 
            
            ///<summary> Prochaine Echéance : Année </summary>
            public int Jdpea { get; set; } 
            
            ///<summary> Prochaine Echéance : Mois </summary>
            public int Jdpem { get; set; } 
            
            ///<summary> Prochaine Echéance : Jour </summary>
            public int Jdpej { get; set; } 
            
            ///<summary> Montant acquis </summary>
            public Int64 Jdacq { get; set; } 
            
            ///<summary> Total : Montant calculé Référence </summary>
            public Decimal Jdtmc { get; set; } 
            
            ///<summary> Total : Montant référence forcé O/N </summary>
            public string Jdtfo { get; set; } 
            
            ///<summary> Total : Montant forcé par tarif O/N </summary>
            public string Jdtft { get; set; } 
            
            ///<summary> Total : Montant forcé Référence </summary>
            public Decimal Jdtff { get; set; } 
            
            ///<summary> Total : Coefficient de calcul forcé </summary>
            public Decimal Jdtfp { get; set; } 
            
            ///<summary> Prime provisionnelle O/N </summary>
            public string Jdpro { get; set; } 
            
            ///<summary> Montant forcé pour minimum O/N </summary>
            public string Jdtmi { get; set; } 
            
            ///<summary> Motif de Total forcé </summary>
            public string Jdtfm { get; set; } 
            
            ///<summary> Total : Montant Autre </summary>
            public Decimal Jdtma { get; set; } 
            
            ///<summary> Total : Total général </summary>
            public Decimal Jdtmg { get; set; } 
            
            ///<summary> Comptant : Montant calculé </summary>
            public Decimal Jdcmc { get; set; } 
            
            ///<summary> Comptant : Montant forcé O/N </summary>
            public string Jdcfo { get; set; } 
            
            ///<summary> Comptant : Montant forcé/tarif O/N </summary>
            public string Jdcft { get; set; } 
            
            ///<summary> Comptant : Forcé HT ou TTC (H/T) </summary>
            public string Jdcfh { get; set; } 
            
            ///<summary> Comptant : Montant forcé HT </summary>
            public Decimal Jdcht { get; set; } 
            
            ///<summary> Comptant : Montant forcé TTC </summary>
            public Decimal Jdctt { get; set; } 
            
            ///<summary> Comptant : Coefficient calcul forcé </summary>
            public Decimal Jdccp { get; set; } 
            
            ///<summary> Montant HT   Prochaine Echéance </summary>
            public Decimal Jdehh { get; set; } 
            
            ///<summary> Mnt HT CATNAT : Prochaine Echéance </summary>
            public Decimal Jdehc { get; set; } 
            
            ///<summary> Montant SMP </summary>
            public Int64 Jdsmp { get; set; } 
            
            ///<summary> Impression Inventaire en Annexe O/N </summary>
            public string Jdivx { get; set; } 
            
            ///<summary> Tacite Reconduct.O/N </summary>
            public string Jdtcr { get; set; } 
            
            ///<summary> Nombre de pages de CP </summary>
            public int Jdnpg { get; set; } 
            
            ///<summary> Edition Complète ou Partielle </summary>
            public string Jdedi { get; set; } 
            
            ///<summary> Edition complète : N° avenant </summary>
            public int Jdedn { get; set; } 
            
            ///<summary> Edition complète : Année </summary>
            public int Jdeda { get; set; } 
            
            ///<summary> Edition complète : Mois </summary>
            public int Jdedm { get; set; } 
            
            ///<summary> Edition complète : Jour </summary>
            public int Jdedj { get; set; } 
            
            ///<summary> Prochaine Echéance: Mnt Incendie HT </summary>
            public Decimal Jdehi { get; set; } 
            
            ///<summary> Inventaire complet(O/N) Avn partiel </summary>
            public string Jdiax { get; set; } 
            
            ///<summary> Type d'édition CP: STandard/OFfice </summary>
            public string Jdted { get; set; } 
            
            ///<summary> Document office si Type Edit OF </summary>
            public string Jddoo { get; set; } 
            
            ///<summary> Régularisation Année de fin </summary>
            public int Jdrua { get; set; } 
            
            ///<summary> Régularisation Mois de fin </summary>
            public int Jdrum { get; set; } 
            
            ///<summary> Régularisation Jour de fin </summary>
            public int Jdruj { get; set; } 
            
            ///<summary> Edition CG sur Bulletin O/N </summary>
            public string Jdecg { get; set; } 
            
            ///<summary> Edition CP sur Bulletin O/N </summary>
            public string Jdecp { get; set; } 
            
            ///<summary> GAREAT Application O/N théorique </summary>
            public string Jdapt { get; set; } 
            
            ///<summary> GAREAT Application retenue </summary>
            public string Jdapr { get; set; } 
            
            ///<summary> GAREAT Tranche Théorique </summary>
            public string Jdaat { get; set; } 
            
            ///<summary> GAREAT Tranche retenue </summary>
            public string Jdaar { get; set; } 
            
            ///<summary> GAREAT Nature Critère </summary>
            public string Jdacr { get; set; } 
            
            ///<summary> GAREAT Montant critère </summary>
            public Decimal Jdacv { get; set; } 
            
            ///<summary> GAREAT Taux Théorique </summary>
            public Decimal Jdaxt { get; set; } 
            
            ///<summary> GAREAT Taux Cession </summary>
            public Decimal Jdaxc { get; set; } 
            
            ///<summary> GAREAT Taux facturé </summary>
            public Decimal Jdaxf { get; set; } 
            
            ///<summary> GAREAT Taux de commission </summary>
            public Decimal Jdaxm { get; set; } 
            
            ///<summary> GAREAT Taux Frais gestion </summary>
            public Decimal Jdaxg { get; set; } 
            
            ///<summary> GAREAT Code taxe </summary>
            public string Jdata { get; set; } 
            
            ///<summary> GAREAT Taux de taxe </summary>
            public Decimal Jdatx { get; set; } 
            
            ///<summary> GAREAT Taux Total Théorique </summary>
            public Decimal Jdaut { get; set; } 
            
            ///<summary> GAREAT Taux total facturé </summary>
            public Decimal Jdauf { get; set; } 
            
            ///<summary> LTA  O/N </summary>
            public string Jdlta { get; set; } 
            
            ///<summary> LTA - seuil S/P </summary>
            public Decimal Jdltasp { get; set; } 
            
            ///<summary> LTA Date debut </summary>
            public int Jdldeb { get; set; } 
            
            ///<summary> LTA heure début </summary>
            public int Jdldeh { get; set; } 
            
            ///<summary> LTA Date Fin </summary>
            public int Jdlfin { get; set; } 
            
            ///<summary> LTA Heure Fin </summary>
            public int Jdlfih { get; set; } 
            
            ///<summary> LTA Durée nb </summary>
            public int Jdldur { get; set; } 
            
            ///<summary> LTA Durée unité </summary>
            public string Jdlduu { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtEnt  x=this,  y=obj as YprtEnt;
            if( y == default(YprtEnt) ) return false;
            return (
                    x.Jdipb==y.Jdipb
                    && x.Jdalx==y.Jdalx
                    && x.Jdsht==y.Jdsht
                    && x.Jdenc==y.Jdenc
                    && x.Jditc==y.Jditc
                    && x.Jdval==y.Jdval
                    && x.Jdvaa==y.Jdvaa
                    && x.Jdvaw==y.Jdvaw
                    && x.Jdvat==y.Jdvat
                    && x.Jdvau==y.Jdvau
                    && x.Jdvah==y.Jdvah
                    && x.Jddrq==y.Jddrq
                    && x.Jdnbr==y.Jdnbr
                    && x.Jdtxl==y.Jdtxl
                    && x.Jdtrr==y.Jdtrr
                    && x.Jdxcm==y.Jdxcm
                    && x.Jdnex==y.Jdnex
                    && x.Jdnpa==y.Jdnpa
                    && x.Jdafc==y.Jdafc
                    && x.Jdafr==y.Jdafr
                    && x.Jdatt==y.Jdatt
                    && x.Jdcna==y.Jdcna
                    && x.Jdcnc==y.Jdcnc
                    && x.Jdina==y.Jdina
                    && x.Jdind==y.Jdind
                    && x.Jdixc==y.Jdixc
                    && x.Jdixf==y.Jdixf
                    && x.Jdixl==y.Jdixl
                    && x.Jdixp==y.Jdixp
                    && x.Jdivo==y.Jdivo
                    && x.Jdiva==y.Jdiva
                    && x.Jdivw==y.Jdivw
                    && x.Jdmht==y.Jdmht
                    && x.Jdrea==y.Jdrea
                    && x.Jdreb==y.Jdreb
                    && x.Jdreh==y.Jdreh
                    && x.Jddpv==y.Jddpv
                    && x.Jdgau==y.Jdgau
                    && x.Jdgvl==y.Jdgvl
                    && x.Jdgun==y.Jdgun
                    && x.Jdpbn==y.Jdpbn
                    && x.Jdpbs==y.Jdpbs
                    && x.Jdpbr==y.Jdpbr
                    && x.Jdpbt==y.Jdpbt
                    && x.Jdpbc==y.Jdpbc
                    && x.Jdpbp==y.Jdpbp
                    && x.Jdpba==y.Jdpba
                    && x.Jdrcg==y.Jdrcg
                    && x.Jdccg==y.Jdccg
                    && x.Jdrcs==y.Jdrcs
                    && x.Jdccs==y.Jdccs
                    && x.Jdclv==y.Jdclv
                    && x.Jdagm==y.Jdagm
                    && x.Jdlcv==y.Jdlcv
                    && x.Jdlca==y.Jdlca
                    && x.Jdlcw==y.Jdlcw
                    && x.Jdlcu==y.Jdlcu
                    && x.Jdlce==y.Jdlce
                    && x.Jddpa==y.Jddpa
                    && x.Jddpm==y.Jddpm
                    && x.Jddpj==y.Jddpj
                    && x.Jdfpa==y.Jdfpa
                    && x.Jdfpm==y.Jdfpm
                    && x.Jdfpj==y.Jdfpj
                    && x.Jdpea==y.Jdpea
                    && x.Jdpem==y.Jdpem
                    && x.Jdpej==y.Jdpej
                    && x.Jdacq==y.Jdacq
                    && x.Jdtmc==y.Jdtmc
                    && x.Jdtfo==y.Jdtfo
                    && x.Jdtft==y.Jdtft
                    && x.Jdtff==y.Jdtff
                    && x.Jdtfp==y.Jdtfp
                    && x.Jdpro==y.Jdpro
                    && x.Jdtmi==y.Jdtmi
                    && x.Jdtfm==y.Jdtfm
                    && x.Jdtma==y.Jdtma
                    && x.Jdtmg==y.Jdtmg
                    && x.Jdcmc==y.Jdcmc
                    && x.Jdcfo==y.Jdcfo
                    && x.Jdcft==y.Jdcft
                    && x.Jdcfh==y.Jdcfh
                    && x.Jdcht==y.Jdcht
                    && x.Jdctt==y.Jdctt
                    && x.Jdccp==y.Jdccp
                    && x.Jdehh==y.Jdehh
                    && x.Jdehc==y.Jdehc
                    && x.Jdsmp==y.Jdsmp
                    && x.Jdivx==y.Jdivx
                    && x.Jdtcr==y.Jdtcr
                    && x.Jdnpg==y.Jdnpg
                    && x.Jdedi==y.Jdedi
                    && x.Jdedn==y.Jdedn
                    && x.Jdeda==y.Jdeda
                    && x.Jdedm==y.Jdedm
                    && x.Jdedj==y.Jdedj
                    && x.Jdehi==y.Jdehi
                    && x.Jdiax==y.Jdiax
                    && x.Jdted==y.Jdted
                    && x.Jddoo==y.Jddoo
                    && x.Jdrua==y.Jdrua
                    && x.Jdrum==y.Jdrum
                    && x.Jdruj==y.Jdruj
                    && x.Jdecg==y.Jdecg
                    && x.Jdecp==y.Jdecp
                    && x.Jdapt==y.Jdapt
                    && x.Jdapr==y.Jdapr
                    && x.Jdaat==y.Jdaat
                    && x.Jdaar==y.Jdaar
                    && x.Jdacr==y.Jdacr
                    && x.Jdacv==y.Jdacv
                    && x.Jdaxt==y.Jdaxt
                    && x.Jdaxc==y.Jdaxc
                    && x.Jdaxf==y.Jdaxf
                    && x.Jdaxm==y.Jdaxm
                    && x.Jdaxg==y.Jdaxg
                    && x.Jdata==y.Jdata
                    && x.Jdatx==y.Jdatx
                    && x.Jdaut==y.Jdaut
                    && x.Jdauf==y.Jdauf
                    && x.Jdlta==y.Jdlta
                    && x.Jdltasp==y.Jdltasp
                    && x.Jdldeb==y.Jdldeb
                    && x.Jdldeh==y.Jdldeh
                    && x.Jdlfin==y.Jdlfin
                    && x.Jdlfih==y.Jdlfih
                    && x.Jdldur==y.Jdldur
                    && x.Jdlduu==y.Jdlduu  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Jdipb?? "").GetHashCode()
                      * 23 ) + (this.Jdalx.GetHashCode() ) 
                      * 23 ) + (this.Jdsht?? "").GetHashCode()
                      * 23 ) + (this.Jdenc?? "").GetHashCode()
                      * 23 ) + (this.Jditc?? "").GetHashCode()
                      * 23 ) + (this.Jdval.GetHashCode() ) 
                      * 23 ) + (this.Jdvaa.GetHashCode() ) 
                      * 23 ) + (this.Jdvaw.GetHashCode() ) 
                      * 23 ) + (this.Jdvat?? "").GetHashCode()
                      * 23 ) + (this.Jdvau?? "").GetHashCode()
                      * 23 ) + (this.Jdvah?? "").GetHashCode()
                      * 23 ) + (this.Jddrq.GetHashCode() ) 
                      * 23 ) + (this.Jdnbr.GetHashCode() ) 
                      * 23 ) + (this.Jdtxl.GetHashCode() ) 
                      * 23 ) + (this.Jdtrr?? "").GetHashCode()
                      * 23 ) + (this.Jdxcm.GetHashCode() ) 
                      * 23 ) + (this.Jdnex.GetHashCode() ) 
                      * 23 ) + (this.Jdnpa.GetHashCode() ) 
                      * 23 ) + (this.Jdafc?? "").GetHashCode()
                      * 23 ) + (this.Jdafr.GetHashCode() ) 
                      * 23 ) + (this.Jdatt?? "").GetHashCode()
                      * 23 ) + (this.Jdcna?? "").GetHashCode()
                      * 23 ) + (this.Jdcnc.GetHashCode() ) 
                      * 23 ) + (this.Jdina?? "").GetHashCode()
                      * 23 ) + (this.Jdind?? "").GetHashCode()
                      * 23 ) + (this.Jdixc?? "").GetHashCode()
                      * 23 ) + (this.Jdixf?? "").GetHashCode()
                      * 23 ) + (this.Jdixl?? "").GetHashCode()
                      * 23 ) + (this.Jdixp?? "").GetHashCode()
                      * 23 ) + (this.Jdivo.GetHashCode() ) 
                      * 23 ) + (this.Jdiva.GetHashCode() ) 
                      * 23 ) + (this.Jdivw.GetHashCode() ) 
                      * 23 ) + (this.Jdmht.GetHashCode() ) 
                      * 23 ) + (this.Jdrea?? "").GetHashCode()
                      * 23 ) + (this.Jdreb.GetHashCode() ) 
                      * 23 ) + (this.Jdreh?? "").GetHashCode()
                      * 23 ) + (this.Jddpv.GetHashCode() ) 
                      * 23 ) + (this.Jdgau?? "").GetHashCode()
                      * 23 ) + (this.Jdgvl.GetHashCode() ) 
                      * 23 ) + (this.Jdgun?? "").GetHashCode()
                      * 23 ) + (this.Jdpbn?? "").GetHashCode()
                      * 23 ) + (this.Jdpbs.GetHashCode() ) 
                      * 23 ) + (this.Jdpbr.GetHashCode() ) 
                      * 23 ) + (this.Jdpbt.GetHashCode() ) 
                      * 23 ) + (this.Jdpbc.GetHashCode() ) 
                      * 23 ) + (this.Jdpbp.GetHashCode() ) 
                      * 23 ) + (this.Jdpba.GetHashCode() ) 
                      * 23 ) + (this.Jdrcg?? "").GetHashCode()
                      * 23 ) + (this.Jdccg?? "").GetHashCode()
                      * 23 ) + (this.Jdrcs?? "").GetHashCode()
                      * 23 ) + (this.Jdccs?? "").GetHashCode()
                      * 23 ) + (this.Jdclv?? "").GetHashCode()
                      * 23 ) + (this.Jdagm.GetHashCode() ) 
                      * 23 ) + (this.Jdlcv.GetHashCode() ) 
                      * 23 ) + (this.Jdlca.GetHashCode() ) 
                      * 23 ) + (this.Jdlcw.GetHashCode() ) 
                      * 23 ) + (this.Jdlcu?? "").GetHashCode()
                      * 23 ) + (this.Jdlce?? "").GetHashCode()
                      * 23 ) + (this.Jddpa.GetHashCode() ) 
                      * 23 ) + (this.Jddpm.GetHashCode() ) 
                      * 23 ) + (this.Jddpj.GetHashCode() ) 
                      * 23 ) + (this.Jdfpa.GetHashCode() ) 
                      * 23 ) + (this.Jdfpm.GetHashCode() ) 
                      * 23 ) + (this.Jdfpj.GetHashCode() ) 
                      * 23 ) + (this.Jdpea.GetHashCode() ) 
                      * 23 ) + (this.Jdpem.GetHashCode() ) 
                      * 23 ) + (this.Jdpej.GetHashCode() ) 
                      * 23 ) + (this.Jdacq.GetHashCode() ) 
                      * 23 ) + (this.Jdtmc.GetHashCode() ) 
                      * 23 ) + (this.Jdtfo?? "").GetHashCode()
                      * 23 ) + (this.Jdtft?? "").GetHashCode()
                      * 23 ) + (this.Jdtff.GetHashCode() ) 
                      * 23 ) + (this.Jdtfp.GetHashCode() ) 
                      * 23 ) + (this.Jdpro?? "").GetHashCode()
                      * 23 ) + (this.Jdtmi?? "").GetHashCode()
                      * 23 ) + (this.Jdtfm?? "").GetHashCode()
                      * 23 ) + (this.Jdtma.GetHashCode() ) 
                      * 23 ) + (this.Jdtmg.GetHashCode() ) 
                      * 23 ) + (this.Jdcmc.GetHashCode() ) 
                      * 23 ) + (this.Jdcfo?? "").GetHashCode()
                      * 23 ) + (this.Jdcft?? "").GetHashCode()
                      * 23 ) + (this.Jdcfh?? "").GetHashCode()
                      * 23 ) + (this.Jdcht.GetHashCode() ) 
                      * 23 ) + (this.Jdctt.GetHashCode() ) 
                      * 23 ) + (this.Jdccp.GetHashCode() ) 
                      * 23 ) + (this.Jdehh.GetHashCode() ) 
                      * 23 ) + (this.Jdehc.GetHashCode() ) 
                      * 23 ) + (this.Jdsmp.GetHashCode() ) 
                      * 23 ) + (this.Jdivx?? "").GetHashCode()
                      * 23 ) + (this.Jdtcr?? "").GetHashCode()
                      * 23 ) + (this.Jdnpg.GetHashCode() ) 
                      * 23 ) + (this.Jdedi?? "").GetHashCode()
                      * 23 ) + (this.Jdedn.GetHashCode() ) 
                      * 23 ) + (this.Jdeda.GetHashCode() ) 
                      * 23 ) + (this.Jdedm.GetHashCode() ) 
                      * 23 ) + (this.Jdedj.GetHashCode() ) 
                      * 23 ) + (this.Jdehi.GetHashCode() ) 
                      * 23 ) + (this.Jdiax?? "").GetHashCode()
                      * 23 ) + (this.Jdted?? "").GetHashCode()
                      * 23 ) + (this.Jddoo?? "").GetHashCode()
                      * 23 ) + (this.Jdrua.GetHashCode() ) 
                      * 23 ) + (this.Jdrum.GetHashCode() ) 
                      * 23 ) + (this.Jdruj.GetHashCode() ) 
                      * 23 ) + (this.Jdecg?? "").GetHashCode()
                      * 23 ) + (this.Jdecp?? "").GetHashCode()
                      * 23 ) + (this.Jdapt?? "").GetHashCode()
                      * 23 ) + (this.Jdapr?? "").GetHashCode()
                      * 23 ) + (this.Jdaat?? "").GetHashCode()
                      * 23 ) + (this.Jdaar?? "").GetHashCode()
                      * 23 ) + (this.Jdacr?? "").GetHashCode()
                      * 23 ) + (this.Jdacv.GetHashCode() ) 
                      * 23 ) + (this.Jdaxt.GetHashCode() ) 
                      * 23 ) + (this.Jdaxc.GetHashCode() ) 
                      * 23 ) + (this.Jdaxf.GetHashCode() ) 
                      * 23 ) + (this.Jdaxm.GetHashCode() ) 
                      * 23 ) + (this.Jdaxg.GetHashCode() ) 
                      * 23 ) + (this.Jdata?? "").GetHashCode()
                      * 23 ) + (this.Jdatx.GetHashCode() ) 
                      * 23 ) + (this.Jdaut.GetHashCode() ) 
                      * 23 ) + (this.Jdauf.GetHashCode() ) 
                      * 23 ) + (this.Jdlta?? "").GetHashCode()
                      * 23 ) + (this.Jdltasp.GetHashCode() ) 
                      * 23 ) + (this.Jdldeb.GetHashCode() ) 
                      * 23 ) + (this.Jdldeh.GetHashCode() ) 
                      * 23 ) + (this.Jdlfin.GetHashCode() ) 
                      * 23 ) + (this.Jdlfih.GetHashCode() ) 
                      * 23 ) + (this.Jdldur.GetHashCode() ) 
                      * 23 ) + (this.Jdlduu?? "").GetHashCode()                   );
           }
        }
    }
}
