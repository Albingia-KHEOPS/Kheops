using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPENT
    public partial class KpEnt  {
             //HPENT
             //KPENT

            ///<summary>Public empty contructor</summary>
            public KpEnt() {}
            ///<summary>Public empty contructor</summary>
            public KpEnt(KpEnt copyFrom) 
            {
                  this.Kaatyp= copyFrom.Kaatyp;
                  this.Kaaipb= copyFrom.Kaaipb;
                  this.Kaaalx= copyFrom.Kaaalx;
                  this.Kaaavn= copyFrom.Kaaavn;
                  this.Kaahin= copyFrom.Kaahin;
                  this.Kaaboni= copyFrom.Kaaboni;
                  this.Kaabont= copyFrom.Kaabont;
                  this.Kaaanti= copyFrom.Kaaanti;
                  this.Kaadesi= copyFrom.Kaadesi;
                  this.Kaaobsv= copyFrom.Kaaobsv;
                  this.Kaalcivalo= copyFrom.Kaalcivalo;
                  this.Kaalcivala= copyFrom.Kaalcivala;
                  this.Kaalcivalw= copyFrom.Kaalcivalw;
                  this.Kaalciunit= copyFrom.Kaalciunit;
                  this.Kaalcibase= copyFrom.Kaalcibase;
                  this.Kaakdiid= copyFrom.Kaakdiid;
                  this.Kaafrhvalo= copyFrom.Kaafrhvalo;
                  this.Kaafrhvala= copyFrom.Kaafrhvala;
                  this.Kaafrhvalw= copyFrom.Kaafrhvalw;
                  this.Kaafrhunit= copyFrom.Kaafrhunit;
                  this.Kaafrhbase= copyFrom.Kaafrhbase;
                  this.Kaakdkid= copyFrom.Kaakdkid;
                  this.Kaaatglci= copyFrom.Kaaatglci;
                  this.Kaaatgklc= copyFrom.Kaaatgklc;
                  this.Kaaatgcap= copyFrom.Kaaatgcap;
                  this.Kaaatgkca= copyFrom.Kaaatgkca;
                  this.Kaaatgsur= copyFrom.Kaaatgsur;
                  this.Kaaatgbcn= copyFrom.Kaaatgbcn;
                  this.Kaaatgkbc= copyFrom.Kaaatgkbc;
                  this.Kaaatgcri= copyFrom.Kaaatgcri;
                  this.Kaaatgapt= copyFrom.Kaaatgapt;
                  this.Kaaatgf= copyFrom.Kaaatgf;
                  this.Kaaatgapr= copyFrom.Kaaatgapr;
                  this.Kaaatgtrt= copyFrom.Kaaatgtrt;
                  this.Kaaatgtrr= copyFrom.Kaaatgtrr;
                  this.Kaaatgtct= copyFrom.Kaaatgtct;
                  this.Kaaatgtcr= copyFrom.Kaaatgtcr;
                  this.Kaaatgtft= copyFrom.Kaaatgtft;
                  this.Kaaatgtcm= copyFrom.Kaaatgtcm;
                  this.Kaaatgtfa= copyFrom.Kaaatgtfa;
                  this.Kaaatgctx= copyFrom.Kaaatgctx;
                  this.Kaaatglcf= copyFrom.Kaaatglcf;
                  this.Kaaatgcaf= copyFrom.Kaaatgcaf;
                  this.Kaaatgsuf= copyFrom.Kaaatgsuf;
                  this.Kaaatgbcf= copyFrom.Kaaatgbcf;
                  this.Kaalciina= copyFrom.Kaalciina;
                  this.Kaaatgfrr= copyFrom.Kaaatgfrr;
                  this.Kaaatgcmt= copyFrom.Kaaatgcmt;
                  this.Kaaatgfat= copyFrom.Kaaatgfat;
                  this.Kaaatgbas= copyFrom.Kaaatgbas;
                  this.Kaaatgkba= copyFrom.Kaaatgkba;
                  this.Kaafrhina= copyFrom.Kaafrhina;
                  this.Kaaand= copyFrom.Kaaand;
                  this.Kaadprj= copyFrom.Kaadprj;
                  this.Kaadsta= copyFrom.Kaadsta;
                  this.Kaaobsf= copyFrom.Kaaobsf;
                  this.Kaaobsa= copyFrom.Kaaobsa;
                  this.Kaaobsc= copyFrom.Kaaobsc;
                  this.Kaaass= copyFrom.Kaaass;
                  this.Kaaafs= copyFrom.Kaaafs;
                  this.Kaaxcms= copyFrom.Kaaxcms;
                  this.Kaacncs= copyFrom.Kaacncs;
                  this.Kaacible= copyFrom.Kaacible;
                  this.Kaamaxa= copyFrom.Kaamaxa;
                  this.Kaamaxe= copyFrom.Kaamaxe;
                  this.Kaaide= copyFrom.Kaaide;
                  this.Kaaimed= copyFrom.Kaaimed;
                  this.Kaaimda= copyFrom.Kaaimda;
                  this.Kaaisin= copyFrom.Kaaisin;
                  this.Kaaavh= copyFrom.Kaaavh;
                  this.Kaaavds= copyFrom.Kaaavds;
                  this.Kaarcp= copyFrom.Kaarcp;
                  this.Kaapaq= copyFrom.Kaapaq;
                  this.Kaascat= copyFrom.Kaascat;
                  this.Kaaltasp= copyFrom.Kaaltasp;
                  this.Kaareldt= copyFrom.Kaareldt;
                  this.Kaaquach= copyFrom.Kaaquach;
                  this.Kaaquven= copyFrom.Kaaquven;
                  this.Kaaavao= copyFrom.Kaaavao;
                  this.Kaaaripk= copyFrom.Kaaaripk;
                  this.Kaaartyg= copyFrom.Kaaartyg;
                  this.Kaapkrd= copyFrom.Kaapkrd;
                  this.Kaasudd= copyFrom.Kaasudd;
                  this.Kaasudh= copyFrom.Kaasudh;
                  this.Kaasufd= copyFrom.Kaasufd;
                  this.Kaasufh= copyFrom.Kaasufh;
                  this.Kaarsdd= copyFrom.Kaarsdd;
                  this.Kaarsdh= copyFrom.Kaarsdh;
                  this.Kaaattdoc= copyFrom.Kaaattdoc;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kaatyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kaaipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kaaalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kaaavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kaahin { get; set; } 
            
            ///<summary> Bonification O/N </summary>
            public string Kaaboni { get; set; } 
            
            ///<summary> Bonification Taux </summary>
            public Decimal Kaabont { get; set; } 
            
            ///<summary> Anticipation O/N </summary>
            public string Kaaanti { get; set; } 
            
            ///<summary> Lien KPDESI Désignation </summary>
            public Int64 Kaadesi { get; set; } 
            
            ///<summary> Lien KPOBSV </summary>
            public Int64 Kaaobsv { get; set; } 
            
            ///<summary> LCI Valeur origine </summary>
            public Decimal Kaalcivalo { get; set; } 
            
            ///<summary> LCI Valeur actualisée </summary>
            public Decimal Kaalcivala { get; set; } 
            
            ///<summary> LCI Valeur de travail </summary>
            public Decimal Kaalcivalw { get; set; } 
            
            ///<summary> LCI Unité </summary>
            public string Kaalciunit { get; set; } 
            
            ///<summary> LCI base </summary>
            public string Kaalcibase { get; set; } 
            
            ///<summary> Lien KPEXPLCI </summary>
            public Int64 Kaakdiid { get; set; } 
            
            ///<summary> Franchise Valeur origine </summary>
            public Decimal Kaafrhvalo { get; set; } 
            
            ///<summary> Franchise Valeur Actualisée </summary>
            public Decimal Kaafrhvala { get; set; } 
            
            ///<summary> Franchise Valeur travail </summary>
            public Decimal Kaafrhvalw { get; set; } 
            
            ///<summary> Franchise unité </summary>
            public string Kaafrhunit { get; set; } 
            
            ///<summary> Franchise Base </summary>
            public string Kaafrhbase { get; set; } 
            
            ///<summary> Lien KPEXPFRH </summary>
            public Int64 Kaakdkid { get; set; } 
            
            ///<summary> ATG LCI dev calculée </summary>
            public Decimal Kaaatglci { get; set; } 
            
            ///<summary> ATG LCI cpt </summary>
            public Decimal Kaaatgklc { get; set; } 
            
            ///<summary> ATG Capitaux dev calculé </summary>
            public Decimal Kaaatgcap { get; set; } 
            
            ///<summary> ATG Capitaux cpt </summary>
            public Decimal Kaaatgkca { get; set; } 
            
            ///<summary> ATG Surface calculée </summary>
            public int Kaaatgsur { get; set; } 
            
            ///<summary> ATG Base CATNAT calculée </summary>
            public Decimal Kaaatgbcn { get; set; } 
            
            ///<summary> ATG Base CATNAT cpt </summary>
            public Decimal Kaaatgkbc { get; set; } 
            
            ///<summary> ATG Critère Théorique LciCapSurfCn </summary>
            public string Kaaatgcri { get; set; } 
            
            ///<summary> ATG Application Théorique O/N </summary>
            public string Kaaatgapt { get; set; } 
            
            ///<summary> ATG Information forcée  O/N </summary>
            public string Kaaatgf { get; set; } 
            
            ///<summary> ATG Application retenue O/N </summary>
            public string Kaaatgapr { get; set; } 
            
            ///<summary> ATG Tranche Théorique </summary>
            public string Kaaatgtrt { get; set; } 
            
            ///<summary> ATG Tranche retenue </summary>
            public string Kaaatgtrr { get; set; } 
            
            ///<summary> ATG Taux cession théorique </summary>
            public Decimal Kaaatgtct { get; set; } 
            
            ///<summary> ATG Taux cession retenu </summary>
            public Decimal Kaaatgtcr { get; set; } 
            
            ///<summary> ATG Taux de Frais Théorique </summary>
            public Decimal Kaaatgtft { get; set; } 
            
            ///<summary> ATG taux de commission retenu </summary>
            public Decimal Kaaatgtcm { get; set; } 
            
            ///<summary> ATG Taux Facturé retenu </summary>
            public Decimal Kaaatgtfa { get; set; } 
            
            ///<summary> ATG Code taxe </summary>
            public string Kaaatgctx { get; set; } 
            
            ///<summary> ATG LCI Forcée dev </summary>
            public Decimal Kaaatglcf { get; set; } 
            
            ///<summary> ATG Capitaux Forcés dev </summary>
            public Decimal Kaaatgcaf { get; set; } 
            
            ///<summary> ATG Surface Forcée </summary>
            public int Kaaatgsuf { get; set; } 
            
            ///<summary> ATG Base Catnat Forcée Dev </summary>
            public Decimal Kaaatgbcf { get; set; } 
            
            ///<summary> LCI indexée O/N </summary>
            public string Kaalciina { get; set; } 
            
            ///<summary> ATG Taux de frais retenu </summary>
            public Decimal Kaaatgfrr { get; set; } 
            
            ///<summary> ATG Taux commissions Théorique </summary>
            public Decimal Kaaatgcmt { get; set; } 
            
            ///<summary> ATG Taux facturé théorique </summary>
            public Decimal Kaaatgfat { get; set; } 
            
            ///<summary> ATG Base calcul dev </summary>
            public Decimal Kaaatgbas { get; set; } 
            
            ///<summary> ATG Base Calcul Cpt </summary>
            public Decimal Kaaatgkba { get; set; } 
            
            ///<summary> Franchise indexées O/N </summary>
            public string Kaafrhina { get; set; } 
            
            ///<summary> Antécédent Description (KPDESI) </summary>
            public Int64 Kaaand { get; set; } 
            
            ///<summary> Date projet </summary>
            public int Kaadprj { get; set; } 
            
            ///<summary> Date Statistique </summary>
            public int Kaadsta { get; set; } 
            
            ///<summary> Obsv mnt ref   ( lien KPOBSV) </summary>
            public Int64 Kaaobsf { get; set; } 
            
            ///<summary> ATG Observation  (Lien KPOBSV) </summary>
            public Int64 Kaaobsa { get; set; } 
            
            ///<summary> Observation Cotisation Lien KPOBSV </summary>
            public Int64 Kaaobsc { get; set; } 
            
            ///<summary> Preneur Assuré O/N </summary>
            public string Kaaass { get; set; } 
            
            ///<summary> Frais accessoire spécif cotisation </summary>
            public int Kaaafs { get; set; } 
            
            ///<summary> Commission HCN Standard O/N </summary>
            public string Kaaxcms { get; set; } 
            
            ///<summary> Commission CN standard O/N </summary>
            public string Kaacncs { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kaacible { get; set; } 
            
            ///<summary> Maxi garanti par assuré </summary>
            public Decimal Kaamaxa { get; set; } 
            
            ///<summary> Maxi garanti par évènement </summary>
            public Decimal Kaamaxe { get; set; } 
            
            ///<summary> Lien KPENG ENGID </summary>
            public Int64 Kaaide { get; set; } 
            
            ///<summary> Médecin conseil (INTYI = 'DR') </summary>
            public int Kaaimed { get; set; } 
            
            ///<summary> Médecin avis (INTYI = 'DR') </summary>
            public int Kaaimda { get; set; } 
            
            ///<summary> Correspondant sin </summary>
            public string Kaaisin { get; set; } 
            
            ///<summary> Avenant Heure Effet </summary>
            public int Kaaavh { get; set; } 
            
            ///<summary> Lien KPDESI Description Avenant </summary>
            public Int64 Kaaavds { get; set; } 
            
            ///<summary> RC PRO O/N </summary>
            public string Kaarcp { get; set; } 
            
            ///<summary> Montant Acquis = provisionnel O/N </summary>
            public string Kaapaq { get; set; } 
            
            ///<summary> Sous Catégorie </summary>
            public string Kaascat { get; set; } 
            
            ///<summary> NON UTILISE </summary>
            public Decimal Kaaltasp { get; set; } 
            
            ///<summary> Date Relance(LTA...) </summary>
            public int Kaareldt { get; set; } 
            
            ///<summary> Code Quotité achats </summary>
            public string Kaaquach { get; set; } 
            
            ///<summary> Code Quotité ventes </summary>
            public string Kaaquven { get; set; } 
            
            ///<summary> Délai(j) constat avarie occulte </summary>
            public int Kaaavao { get; set; } 
            
            ///<summary> Resouscription : Prime réglée </summary>
            public int Kaaaripk { get; set; } 
            
            ///<summary> Resouscription : Typ gestion V=REVseul M=avec màj </summary>
            public string Kaaartyg { get; set; } 
            
            ///<summary> Resouscription : Date règlement de la prime </summary>
            public int Kaapkrd { get; set; } 
            
            ///<summary> Resouscription : Date début susp </summary>
            public int Kaasudd { get; set; } 
            
            ///<summary> Resouscription : Heure début susp </summary>
            public int Kaasudh { get; set; } 
            
            ///<summary> Resouscription : Date fin susp </summary>
            public int Kaasufd { get; set; } 
            
            ///<summary> Resouscription : Heure fin susp </summary>
            public int Kaasufh { get; set; } 
            
            ///<summary> Resouscription : Date début résiliation </summary>
            public int Kaarsdd { get; set; } 
            
            ///<summary> Resouscription : Heure début résil </summary>
            public int Kaarsdh { get; set; } 
            
            ///<summary> Attente document (relance offre) </summary>
            public string Kaaattdoc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpEnt  x=this,  y=obj as KpEnt;
            if( y == default(KpEnt) ) return false;
            return (
                    x.Kaatyp==y.Kaatyp
                    && x.Kaaipb==y.Kaaipb
                    && x.Kaaalx==y.Kaaalx
                    && x.Kaaboni==y.Kaaboni
                    && x.Kaabont==y.Kaabont
                    && x.Kaaanti==y.Kaaanti
                    && x.Kaadesi==y.Kaadesi
                    && x.Kaaobsv==y.Kaaobsv
                    && x.Kaalcivalo==y.Kaalcivalo
                    && x.Kaalcivala==y.Kaalcivala
                    && x.Kaalcivalw==y.Kaalcivalw
                    && x.Kaalciunit==y.Kaalciunit
                    && x.Kaalcibase==y.Kaalcibase
                    && x.Kaakdiid==y.Kaakdiid
                    && x.Kaafrhvalo==y.Kaafrhvalo
                    && x.Kaafrhvala==y.Kaafrhvala
                    && x.Kaafrhvalw==y.Kaafrhvalw
                    && x.Kaafrhunit==y.Kaafrhunit
                    && x.Kaafrhbase==y.Kaafrhbase
                    && x.Kaakdkid==y.Kaakdkid
                    && x.Kaaatglci==y.Kaaatglci
                    && x.Kaaatgklc==y.Kaaatgklc
                    && x.Kaaatgcap==y.Kaaatgcap
                    && x.Kaaatgkca==y.Kaaatgkca
                    && x.Kaaatgsur==y.Kaaatgsur
                    && x.Kaaatgbcn==y.Kaaatgbcn
                    && x.Kaaatgkbc==y.Kaaatgkbc
                    && x.Kaaatgcri==y.Kaaatgcri
                    && x.Kaaatgapt==y.Kaaatgapt
                    && x.Kaaatgf==y.Kaaatgf
                    && x.Kaaatgapr==y.Kaaatgapr
                    && x.Kaaatgtrt==y.Kaaatgtrt
                    && x.Kaaatgtrr==y.Kaaatgtrr
                    && x.Kaaatgtct==y.Kaaatgtct
                    && x.Kaaatgtcr==y.Kaaatgtcr
                    && x.Kaaatgtft==y.Kaaatgtft
                    && x.Kaaatgtcm==y.Kaaatgtcm
                    && x.Kaaatgtfa==y.Kaaatgtfa
                    && x.Kaaatgctx==y.Kaaatgctx
                    && x.Kaaatglcf==y.Kaaatglcf
                    && x.Kaaatgcaf==y.Kaaatgcaf
                    && x.Kaaatgsuf==y.Kaaatgsuf
                    && x.Kaaatgbcf==y.Kaaatgbcf
                    && x.Kaalciina==y.Kaalciina
                    && x.Kaaatgfrr==y.Kaaatgfrr
                    && x.Kaaatgcmt==y.Kaaatgcmt
                    && x.Kaaatgfat==y.Kaaatgfat
                    && x.Kaaatgbas==y.Kaaatgbas
                    && x.Kaaatgkba==y.Kaaatgkba
                    && x.Kaafrhina==y.Kaafrhina
                    && x.Kaaand==y.Kaaand
                    && x.Kaadprj==y.Kaadprj
                    && x.Kaadsta==y.Kaadsta
                    && x.Kaaobsf==y.Kaaobsf
                    && x.Kaaobsa==y.Kaaobsa
                    && x.Kaaobsc==y.Kaaobsc
                    && x.Kaaass==y.Kaaass
                    && x.Kaaafs==y.Kaaafs
                    && x.Kaaxcms==y.Kaaxcms
                    && x.Kaacncs==y.Kaacncs
                    && x.Kaacible==y.Kaacible
                    && x.Kaamaxa==y.Kaamaxa
                    && x.Kaamaxe==y.Kaamaxe
                    && x.Kaaide==y.Kaaide
                    && x.Kaaimed==y.Kaaimed
                    && x.Kaaimda==y.Kaaimda
                    && x.Kaaisin==y.Kaaisin
                    && x.Kaaavh==y.Kaaavh
                    && x.Kaaavds==y.Kaaavds
                    && x.Kaarcp==y.Kaarcp
                    && x.Kaapaq==y.Kaapaq
                    && x.Kaascat==y.Kaascat
                    && x.Kaaltasp==y.Kaaltasp
                    && x.Kaareldt==y.Kaareldt
                    && x.Kaaquach==y.Kaaquach
                    && x.Kaaquven==y.Kaaquven
                    && x.Kaaavao==y.Kaaavao
                    && x.Kaaaripk==y.Kaaaripk
                    && x.Kaaartyg==y.Kaaartyg
                    && x.Kaapkrd==y.Kaapkrd
                    && x.Kaasudd==y.Kaasudd
                    && x.Kaasudh==y.Kaasudh
                    && x.Kaasufd==y.Kaasufd
                    && x.Kaasufh==y.Kaasufh
                    && x.Kaarsdd==y.Kaarsdd
                    && x.Kaarsdh==y.Kaarsdh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kaatyp?? "").GetHashCode()
                      * 23 ) + (this.Kaaipb?? "").GetHashCode()
                      * 23 ) + (this.Kaaalx.GetHashCode() ) 
                      * 23 ) + (this.Kaaboni?? "").GetHashCode()
                      * 23 ) + (this.Kaabont.GetHashCode() ) 
                      * 23 ) + (this.Kaaanti?? "").GetHashCode()
                      * 23 ) + (this.Kaadesi.GetHashCode() ) 
                      * 23 ) + (this.Kaaobsv.GetHashCode() ) 
                      * 23 ) + (this.Kaalcivalo.GetHashCode() ) 
                      * 23 ) + (this.Kaalcivala.GetHashCode() ) 
                      * 23 ) + (this.Kaalcivalw.GetHashCode() ) 
                      * 23 ) + (this.Kaalciunit?? "").GetHashCode()
                      * 23 ) + (this.Kaalcibase?? "").GetHashCode()
                      * 23 ) + (this.Kaakdiid.GetHashCode() ) 
                      * 23 ) + (this.Kaafrhvalo.GetHashCode() ) 
                      * 23 ) + (this.Kaafrhvala.GetHashCode() ) 
                      * 23 ) + (this.Kaafrhvalw.GetHashCode() ) 
                      * 23 ) + (this.Kaafrhunit?? "").GetHashCode()
                      * 23 ) + (this.Kaafrhbase?? "").GetHashCode()
                      * 23 ) + (this.Kaakdkid.GetHashCode() ) 
                      * 23 ) + (this.Kaaatglci.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgklc.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgcap.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgkca.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgsur.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgbcn.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgkbc.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgcri?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgapt?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgf?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgapr?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgtrt?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgtrr?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgtct.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgtcr.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgtft.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgtcm.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgtfa.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgctx?? "").GetHashCode()
                      * 23 ) + (this.Kaaatglcf.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgcaf.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgsuf.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgbcf.GetHashCode() ) 
                      * 23 ) + (this.Kaalciina?? "").GetHashCode()
                      * 23 ) + (this.Kaaatgfrr.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgcmt.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgfat.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgbas.GetHashCode() ) 
                      * 23 ) + (this.Kaaatgkba.GetHashCode() ) 
                      * 23 ) + (this.Kaafrhina?? "").GetHashCode()
                      * 23 ) + (this.Kaaand.GetHashCode() ) 
                      * 23 ) + (this.Kaadprj.GetHashCode() ) 
                      * 23 ) + (this.Kaadsta.GetHashCode() ) 
                      * 23 ) + (this.Kaaobsf.GetHashCode() ) 
                      * 23 ) + (this.Kaaobsa.GetHashCode() ) 
                      * 23 ) + (this.Kaaobsc.GetHashCode() ) 
                      * 23 ) + (this.Kaaass?? "").GetHashCode()
                      * 23 ) + (this.Kaaafs.GetHashCode() ) 
                      * 23 ) + (this.Kaaxcms?? "").GetHashCode()
                      * 23 ) + (this.Kaacncs?? "").GetHashCode()
                      * 23 ) + (this.Kaacible?? "").GetHashCode()
                      * 23 ) + (this.Kaamaxa.GetHashCode() ) 
                      * 23 ) + (this.Kaamaxe.GetHashCode() ) 
                      * 23 ) + (this.Kaaide.GetHashCode() ) 
                      * 23 ) + (this.Kaaimed.GetHashCode() ) 
                      * 23 ) + (this.Kaaimda.GetHashCode() ) 
                      * 23 ) + (this.Kaaisin?? "").GetHashCode()
                      * 23 ) + (this.Kaaavh.GetHashCode() ) 
                      * 23 ) + (this.Kaaavds.GetHashCode() ) 
                      * 23 ) + (this.Kaarcp?? "").GetHashCode()
                      * 23 ) + (this.Kaapaq?? "").GetHashCode()
                      * 23 ) + (this.Kaascat?? "").GetHashCode()
                      * 23 ) + (this.Kaaltasp.GetHashCode() ) 
                      * 23 ) + (this.Kaareldt.GetHashCode() ) 
                      * 23 ) + (this.Kaaquach?? "").GetHashCode()
                      * 23 ) + (this.Kaaquven?? "").GetHashCode()
                      * 23 ) + (this.Kaaavao.GetHashCode() ) 
                      * 23 ) + (this.Kaaaripk.GetHashCode() ) 
                      * 23 ) + (this.Kaaartyg?? "").GetHashCode()
                      * 23 ) + (this.Kaapkrd.GetHashCode() ) 
                      * 23 ) + (this.Kaasudd.GetHashCode() ) 
                      * 23 ) + (this.Kaasudh.GetHashCode() ) 
                      * 23 ) + (this.Kaasufd.GetHashCode() ) 
                      * 23 ) + (this.Kaasufh.GetHashCode() ) 
                      * 23 ) + (this.Kaarsdd.GetHashCode() ) 
                      * 23 ) + (this.Kaarsdh.GetHashCode() )                    );
           }
        }
    }
}
