using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YSINIST
    public partial class YSinist  {
             //YSINIST

            ///<summary>Public empty contructor</summary>
            public YSinist() {}
            ///<summary>Public empty contructor</summary>
            public YSinist(YSinist copyFrom) 
            {
                  this.Sisua= copyFrom.Sisua;
                  this.Sinum= copyFrom.Sinum;
                  this.Sisbr= copyFrom.Sisbr;
                  this.Sisum= copyFrom.Sisum;
                  this.Sisuj= copyFrom.Sisuj;
                  this.Sioua= copyFrom.Sioua;
                  this.Sioum= copyFrom.Sioum;
                  this.Siouj= copyFrom.Siouj;
                  this.Siipb= copyFrom.Siipb;
                  this.Sialx= copyFrom.Sialx;
                  this.Siavn= copyFrom.Siavn;
                  this.Sitch= copyFrom.Sitch;
                  this.Sitbr= copyFrom.Sitbr;
                  this.Sitsb= copyFrom.Sitsb;
                  this.Siges= copyFrom.Siges;
                  this.Sirca= copyFrom.Sirca;
                  this.Sircm= copyFrom.Sircm;
                  this.Sircj= copyFrom.Sircj;
                  this.Sintr= copyFrom.Sintr;
                  this.Sicau= copyFrom.Sicau;
                  this.Sijja= copyFrom.Sijja;
                  this.Sijjm= copyFrom.Sijjm;
                  this.Sijjj= copyFrom.Sijjj;
                  this.Siref= copyFrom.Siref;
                  this.Siad1= copyFrom.Siad1;
                  this.Siad2= copyFrom.Siad2;
                  this.Sidep= copyFrom.Sidep;
                  this.Sicpo= copyFrom.Sicpo;
                  this.Sivil= copyFrom.Sivil;
                  this.Sipay= copyFrom.Sipay;
                  this.Siexp= copyFrom.Siexp;
                  this.Siine= copyFrom.Siine;
                  this.Sirex= copyFrom.Sirex;
                  this.Siexm= copyFrom.Siexm;
                  this.Sierp= copyFrom.Sierp;
                  this.Siejp= copyFrom.Siejp;
                  this.Sierd= copyFrom.Sierd;
                  this.Siejd= copyFrom.Siejd;
                  this.Sinju= copyFrom.Sinju;
                  this.Siavo= copyFrom.Siavo;
                  this.Siina= copyFrom.Siina;
                  this.Sirav= copyFrom.Sirav;
                  this.Siict= copyFrom.Siict;
                  this.Siinc= copyFrom.Siinc;
                  this.Sirco= copyFrom.Sirco;
                  this.Sidct= copyFrom.Sidct;
                  this.Sicct= copyFrom.Sicct;
                  this.Siapr= copyFrom.Siapr;
                  this.Sirap= copyFrom.Sirap;
                  this.Siapp= copyFrom.Siapp;
                  this.Sipcv= copyFrom.Sipcv;
                  this.Sinpl= copyFrom.Sinpl;
                  this.Singe= copyFrom.Singe;
                  this.Siacc= copyFrom.Siacc;
                  this.Sirfu= copyFrom.Sirfu;
                  this.Sicru= copyFrom.Sicru;
                  this.Sicra= copyFrom.Sicra;
                  this.Sicrm= copyFrom.Sicrm;
                  this.Sicrj= copyFrom.Sicrj;
                  this.Simju= copyFrom.Simju;
                  this.Simja= copyFrom.Simja;
                  this.Simjm= copyFrom.Simjm;
                  this.Simjj= copyFrom.Simjj;
                  this.Sisit= copyFrom.Sisit;
                  this.Sista= copyFrom.Sista;
                  this.Sistm= copyFrom.Sistm;
                  this.Sistj= copyFrom.Sistj;
                  this.Sieta= copyFrom.Sieta;
                  this.Sidre= copyFrom.Sidre;
                  this.Sidrn= copyFrom.Sidrn;
                  this.Sicvg= copyFrom.Sicvg;
                  this.Sirpa= copyFrom.Sirpa;
                  this.Sirpm= copyFrom.Sirpm;
                  this.Sirpj= copyFrom.Sirpj;
                  this.Sirda= copyFrom.Sirda;
                  this.Sirdm= copyFrom.Sirdm;
                  this.Sirdj= copyFrom.Sirdj;
                  this.Sihia= copyFrom.Sihia;
                  this.Sihim= copyFrom.Sihim;
                  this.Sihij= copyFrom.Sihij;
                  this.Sihih= copyFrom.Sihih;
                  this.Sihiu= copyFrom.Sihiu;
                  this.Sircd= copyFrom.Sircd;
                  this.Siihb= copyFrom.Siihb;
                  this.Siroa= copyFrom.Siroa;
                  this.Sirom= copyFrom.Sirom;
                  this.Siroj= copyFrom.Siroj;
                  this.Sirou= copyFrom.Sirou;
                  this.Sidra= copyFrom.Sidra;
                  this.Sidrm= copyFrom.Sidrm;
                  this.Sidrj= copyFrom.Sidrj;
                  this.Sidba= copyFrom.Sidba;
                  this.Sidbm= copyFrom.Sidbm;
                  this.Sidbj= copyFrom.Sidbj;
                  this.Sicvr= copyFrom.Sicvr;
                  this.Sis36= copyFrom.Sis36;
                  this.Simng= copyFrom.Simng;
                  this.Siclo= copyFrom.Siclo;
                  this.Siexn= copyFrom.Siexn;
                  this.Sirct= copyFrom.Sirct;
                  this.Simtb= copyFrom.Simtb;
                  this.Simts= copyFrom.Simts;
                  this.Siemi= copyFrom.Siemi;
                  this.Sioup= copyFrom.Sioup;
                  this.Sicna= copyFrom.Sicna;
                  this.Sispp= copyFrom.Sispp;
                  this.Sisps= copyFrom.Sisps;
                  this.Simfp= copyFrom.Simfp;
                  this.Sipf= copyFrom.Sipf;
                  this.Sicbc= copyFrom.Sicbc;
                  this.Sibcr= copyFrom.Sibcr;
                  this.Sibou= copyFrom.Sibou;
                  this.Sievn= copyFrom.Sievn;
                  this.Sigar= copyFrom.Sigar;
                  this.Sinnc= copyFrom.Sinnc;
                  this.Siejr= copyFrom.Siejr;
                  this.Siejs= copyFrom.Siejs;
                  this.Sirua= copyFrom.Sirua;
                  this.Sirum= copyFrom.Sirum;
                  this.Siruj= copyFrom.Siruj;
                  this.Si15a= copyFrom.Si15a;
                  this.Si15m= copyFrom.Si15m;
                  this.Si15j= copyFrom.Si15j;
                  this.Siadh= copyFrom.Siadh;
                  this.Silco= copyFrom.Silco;
                  this.Sifil= copyFrom.Sifil;
                  this.Sicok= copyFrom.Sicok;
                  this.Sinlc= copyFrom.Sinlc;
                  this.Sie1a= copyFrom.Sie1a;
                  this.Sie1m= copyFrom.Sie1m;
                  this.Sie1j= copyFrom.Sie1j;
                  this.Siin5= copyFrom.Siin5;
        
            }        
            
            ///<summary> N° sinistre : Année de survenance </summary>
            public int Sisua { get; set; } 
            
            ///<summary> N° sinistre : N° </summary>
            public int Sinum { get; set; } 
            
            ///<summary> N° sinistre : Sous-branche </summary>
            public string Sisbr { get; set; } 
            
            ///<summary> Survenance: Mois </summary>
            public int Sisum { get; set; } 
            
            ///<summary> Survenance: Jour </summary>
            public int Sisuj { get; set; } 
            
            ///<summary> Ouverture: Année </summary>
            public int Sioua { get; set; } 
            
            ///<summary> Ouverture: Mois </summary>
            public int Sioum { get; set; } 
            
            ///<summary> Ouverture: Jour </summary>
            public int Siouj { get; set; } 
            
            ///<summary> N°police </summary>
            public string Siipb { get; set; } 
            
            ///<summary> N°aliment ou Connexe et version </summary>
            public int Sialx { get; set; } 
            
            ///<summary> N°avenant </summary>
            public int Siavn { get; set; } 
            
            ///<summary> N°tranche </summary>
            public int Sitch { get; set; } 
            
            ///<summary> Branche. </summary>
            public string Sitbr { get; set; } 
            
            ///<summary> Sous-branche. </summary>
            public string Sitsb { get; set; } 
            
            ///<summary> Gestionnaire </summary>
            public string Siges { get; set; } 
            
            ///<summary> Réception: Année </summary>
            public int Sirca { get; set; } 
            
            ///<summary> Réception: Mois </summary>
            public int Sircm { get; set; } 
            
            ///<summary> Réception: Jour </summary>
            public int Sircj { get; set; } 
            
            ///<summary> Nature du sinistre </summary>
            public string Sintr { get; set; } 
            
            ///<summary> Cause </summary>
            public string Sicau { get; set; } 
            
            ///<summary> Année du jour J </summary>
            public int Sijja { get; set; } 
            
            ///<summary> Mois  du jour J </summary>
            public int Sijjm { get; set; } 
            
            ///<summary> Jour  du jour J </summary>
            public int Sijjj { get; set; } 
            
            ///<summary> Nom affaire (Référence opération) </summary>
            public string Siref { get; set; } 
            
            ///<summary> Adresse 1 du sinistre </summary>
            public string Siad1 { get; set; } 
            
            ///<summary> Adresse 2 du sinistre </summary>
            public string Siad2 { get; set; } 
            
            ///<summary> Département du sinistre </summary>
            public string Sidep { get; set; } 
            
            ///<summary> 3 dern.car.code postal du sinistre </summary>
            public string Sicpo { get; set; } 
            
            ///<summary> Ville du sinistre </summary>
            public string Sivil { get; set; } 
            
            ///<summary> Code pays du sinistre </summary>
            public string Sipay { get; set; } 
            
            ///<summary> Expert </summary>
            public int Siexp { get; set; } 
            
            ///<summary> Expert: Interlocuteur </summary>
            public int Siine { get; set; } 
            
            ///<summary> Expert: Référence </summary>
            public string Sirex { get; set; } 
            
            ///<summary> Expert: Extension de mission   (O/N) </summary>
            public string Siexm { get; set; } 
            
            ///<summary> Expert: Relance Rap.Prél. </summary>
            public string Sierp { get; set; } 
            
            ///<summary> Expert: Nb jours délai Rap.Prélimin. </summary>
            public int Siejp { get; set; } 
            
            ///<summary> Expert: Relance Rapport Définitif </summary>
            public string Sierd { get; set; } 
            
            ///<summary> Expert: Nb jours délai Rap.Définif </summary>
            public int Siejd { get; set; } 
            
            ///<summary> Nature judiciaire </summary>
            public string Sinju { get; set; } 
            
            ///<summary> Avocat </summary>
            public int Siavo { get; set; } 
            
            ///<summary> Avocat: Interlocuteur </summary>
            public int Siina { get; set; } 
            
            ///<summary> Avocat: Référence </summary>
            public string Sirav { get; set; } 
            
            ///<summary> Courtier </summary>
            public int Siict { get; set; } 
            
            ///<summary> Courtier: Interlocuteur </summary>
            public int Siinc { get; set; } 
            
            ///<summary> Courtier: Référence </summary>
            public string Sirco { get; set; } 
            
            ///<summary> Courtier: Déclarant              O/N </summary>
            public string Sidct { get; set; } 
            
            ///<summary> Courtier: Informé                O/N </summary>
            public string Sicct { get; set; } 
            
            ///<summary> Apériteur </summary>
            public string Siapr { get; set; } 
            
            ///<summary> Apériteur: Référence </summary>
            public string Sirap { get; set; } 
            
            ///<summary> Part ALBINGIA en % </summary>
            public Decimal Siapp { get; set; } 
            
            ///<summary> % Couvert. </summary>
            public int Sipcv { get; set; } 
            
            ///<summary> Nature Police                   /A/C </summary>
            public string Sinpl { get; set; } 
            
            ///<summary> Nature Gestion      Ext/Albin/Gerl/ </summary>
            public string Singe { get; set; } 
            
            ///<summary> Code acceptation du sinistre </summary>
            public string Siacc { get; set; } 
            
            ///<summary> Code refus </summary>
            public string Sirfu { get; set; } 
            
            ///<summary> Création: User </summary>
            public string Sicru { get; set; } 
            
            ///<summary> Création: Année </summary>
            public int Sicra { get; set; } 
            
            ///<summary> Création: Mois </summary>
            public int Sicrm { get; set; } 
            
            ///<summary> Création: Jour </summary>
            public int Sicrj { get; set; } 
            
            ///<summary> MàJ: User </summary>
            public string Simju { get; set; } 
            
            ///<summary> MàJ: Année </summary>
            public int Simja { get; set; } 
            
            ///<summary> MàJ: Mois </summary>
            public int Simjm { get; set; } 
            
            ///<summary> MàJ: Jour </summary>
            public int Simjj { get; set; } 
            
            ///<summary> Situation: Code </summary>
            public string Sisit { get; set; } 
            
            ///<summary> Situation: Année </summary>
            public int Sista { get; set; } 
            
            ///<summary> Situation: Mois </summary>
            public int Sistm { get; set; } 
            
            ///<summary> Situation: Jour </summary>
            public int Sistj { get; set; } 
            
            ///<summary> Validation: Code état </summary>
            public string Sieta { get; set; } 
            
            ///<summary> Cause abandon déjà saisi en gestion </summary>
            public string Sidre { get; set; } 
            
            ///<summary> Difficultés recours: Nature. </summary>
            public string Sidrn { get; set; } 
            
            ///<summary> Recours: Convention de gestion </summary>
            public string Sicvg { get; set; } 
            
            ///<summary> Rapport Préliminaire: Année récep. </summary>
            public int Sirpa { get; set; } 
            
            ///<summary> Rapport Préliminaire: Mois  récep. </summary>
            public int Sirpm { get; set; } 
            
            ///<summary> Rapport Préliminaire: Jour  récep. </summary>
            public int Sirpj { get; set; } 
            
            ///<summary> Rapport Définitif: Année récep. </summary>
            public int Sirda { get; set; } 
            
            ///<summary> Rapport Définitif: Mois  récep. </summary>
            public int Sirdm { get; set; } 
            
            ///<summary> Rapport Définitif: Jour  récep. </summary>
            public int Sirdj { get; set; } 
            
            ///<summary> Historique: Année </summary>
            public int Sihia { get; set; } 
            
            ///<summary> Historique: Mois </summary>
            public int Sihim { get; set; } 
            
            ///<summary> Historique: Jour </summary>
            public int Sihij { get; set; } 
            
            ///<summary> Historique: Heure </summary>
            public int Sihih { get; set; } 
            
            ///<summary> Historique: User </summary>
            public string Sihiu { get; set; } 
            
            ///<summary> RCD: Interrupt°délais gar.   R/F/L/N </summary>
            public string Sircd { get; set; } 
            
            ///<summary> Info systématique rglt Dir.Générale </summary>
            public string Siihb { get; set; } 
            
            ///<summary> Réouverture: Année </summary>
            public int Siroa { get; set; } 
            
            ///<summary> Réouverture: Mois </summary>
            public int Sirom { get; set; } 
            
            ///<summary> Réouverture: Jour </summary>
            public int Siroj { get; set; } 
            
            ///<summary> Réouverture: User </summary>
            public string Sirou { get; set; } 
            
            ///<summary> DROC: Année </summary>
            public int Sidra { get; set; } 
            
            ///<summary> DROC: Mois </summary>
            public int Sidrm { get; set; } 
            
            ///<summary> DROC: Jour </summary>
            public int Sidrj { get; set; } 
            
            ///<summary> Début travaux: Année </summary>
            public int Sidba { get; set; } 
            
            ///<summary> Début travaux: Mois </summary>
            public int Sidbm { get; set; } 
            
            ///<summary> Début travaux: Jour </summary>
            public int Sidbj { get; set; } 
            
            ///<summary> Recours : Convention de règlement </summary>
            public string Sicvr { get; set; } 
            
            ///<summary> Transfert info vers S36        O/N/T </summary>
            public string Sis36 { get; set; } 
            
            ///<summary> Montant non garanti (devise police) </summary>
            public Decimal Simng { get; set; } 
            
            ///<summary> Demande cloture (O/ ) </summary>
            public string Siclo { get; set; } 
            
            ///<summary> Expert: Numérotation sur rapport </summary>
            public string Siexn { get; set; } 
            
            ///<summary> Type récept°rapport Expert P/D </summary>
            public string Sirct { get; set; } 
            
            ///<summary> Montant base objet police survenance </summary>
            public Int64 Simtb { get; set; } 
            
            ///<summary> Montant objets sinistré </summary>
            public Int64 Simts { get; set; } 
            
            ///<summary> Expert missionné par courtier O/N </summary>
            public string Siemi { get; set; } 
            
            ///<summary> Sinistre ouvert sans police </summary>
            public string Sioup { get; set; } 
            
            ///<summary> Catastrophe naturelle O/N </summary>
            public string Sicna { get; set; } 
            
            ///<summary> S/P Montant primes </summary>
            public Int64 Sispp { get; set; } 
            
            ///<summary> S/P Montant sinistre </summary>
            public Int64 Sisps { get; set; } 
            
            ///<summary> Montant franchise provisionnée </summary>
            public Decimal Simfp { get; set; } 
            
            ///<summary> Préfixe -stats SPAL- </summary>
            public int Sipf { get; set; } 
            
            ///<summary> BCR: Code Cause interne </summary>
            public string Sicbc { get; set; } 
            
            ///<summary> BCR: O-oui, N-non </summary>
            public string Sibcr { get; set; } 
            
            ///<summary> Bourse / Hors Bourse </summary>
            public string Sibou { get; set; } 
            
            ///<summary> Evénement </summary>
            public string Sievn { get; set; } 
            
            ///<summary> Garantie principale </summary>
            public string Sigar { get; set; } 
            
            ///<summary> Avn non concrétisé: Numéro </summary>
            public int Sinnc { get; set; } 
            
            ///<summary> Délai J + 15 </summary>
            public int Siejr { get; set; } 
            
            ///<summary> Code Situation J + 15 </summary>
            public string Siejs { get; set; } 
            
            ///<summary> Rapport unique Année </summary>
            public int Sirua { get; set; } 
            
            ///<summary> Rapport Unique Mois </summary>
            public int Sirum { get; set; } 
            
            ///<summary> Rapport Unique Jour </summary>
            public int Siruj { get; set; } 
            
            ///<summary> Date J+15 Année </summary>
            public int Si15a { get; set; } 
            
            ///<summary> Date J+15 Mois </summary>
            public int Si15m { get; set; } 
            
            ///<summary> Date J+15 Jour </summary>
            public int Si15j { get; set; } 
            
            ///<summary> Adresse Numéro </summary>
            public int Siadh { get; set; } 
            
            ///<summary> Libellé concerne </summary>
            public string Silco { get; set; } 
            
            ///<summary> Filler </summary>
            public string Sifil { get; set; } 
            
            ///<summary> Côte comptable O/N </summary>
            public string Sicok { get; set; } 
            
            ///<summary> Nom locataire </summary>
            public string Sinlc { get; set; } 
            
            ///<summary> Date 1iere expertise Année </summary>
            public int Sie1a { get; set; } 
            
            ///<summary> Date 1ière expertise Mois </summary>
            public int Sie1m { get; set; } 
            
            ///<summary> Date 1ière expertise Jour </summary>
            public int Sie1j { get; set; } 
            
            ///<summary> Code Interlocuteur sur 5 </summary>
            public int Siin5 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YSinist  x=this,  y=obj as YSinist;
            if( y == default(YSinist) ) return false;
            return (
                    x.Sisua==y.Sisua
                    && x.Sinum==y.Sinum
                    && x.Sisbr==y.Sisbr
                    && x.Sisum==y.Sisum
                    && x.Sisuj==y.Sisuj
                    && x.Sioua==y.Sioua
                    && x.Sioum==y.Sioum
                    && x.Siouj==y.Siouj
                    && x.Siipb==y.Siipb
                    && x.Sialx==y.Sialx
                    && x.Siavn==y.Siavn
                    && x.Sitch==y.Sitch
                    && x.Sitbr==y.Sitbr
                    && x.Sitsb==y.Sitsb
                    && x.Siges==y.Siges
                    && x.Sirca==y.Sirca
                    && x.Sircm==y.Sircm
                    && x.Sircj==y.Sircj
                    && x.Sintr==y.Sintr
                    && x.Sicau==y.Sicau
                    && x.Sijja==y.Sijja
                    && x.Sijjm==y.Sijjm
                    && x.Sijjj==y.Sijjj
                    && x.Siref==y.Siref
                    && x.Siad1==y.Siad1
                    && x.Siad2==y.Siad2
                    && x.Sidep==y.Sidep
                    && x.Sicpo==y.Sicpo
                    && x.Sivil==y.Sivil
                    && x.Sipay==y.Sipay
                    && x.Siexp==y.Siexp
                    && x.Siine==y.Siine
                    && x.Sirex==y.Sirex
                    && x.Siexm==y.Siexm
                    && x.Sierp==y.Sierp
                    && x.Siejp==y.Siejp
                    && x.Sierd==y.Sierd
                    && x.Siejd==y.Siejd
                    && x.Sinju==y.Sinju
                    && x.Siavo==y.Siavo
                    && x.Siina==y.Siina
                    && x.Sirav==y.Sirav
                    && x.Siict==y.Siict
                    && x.Siinc==y.Siinc
                    && x.Sirco==y.Sirco
                    && x.Sidct==y.Sidct
                    && x.Sicct==y.Sicct
                    && x.Siapr==y.Siapr
                    && x.Sirap==y.Sirap
                    && x.Siapp==y.Siapp
                    && x.Sipcv==y.Sipcv
                    && x.Sinpl==y.Sinpl
                    && x.Singe==y.Singe
                    && x.Siacc==y.Siacc
                    && x.Sirfu==y.Sirfu
                    && x.Sicru==y.Sicru
                    && x.Sicra==y.Sicra
                    && x.Sicrm==y.Sicrm
                    && x.Sicrj==y.Sicrj
                    && x.Simju==y.Simju
                    && x.Simja==y.Simja
                    && x.Simjm==y.Simjm
                    && x.Simjj==y.Simjj
                    && x.Sisit==y.Sisit
                    && x.Sista==y.Sista
                    && x.Sistm==y.Sistm
                    && x.Sistj==y.Sistj
                    && x.Sieta==y.Sieta
                    && x.Sidre==y.Sidre
                    && x.Sidrn==y.Sidrn
                    && x.Sicvg==y.Sicvg
                    && x.Sirpa==y.Sirpa
                    && x.Sirpm==y.Sirpm
                    && x.Sirpj==y.Sirpj
                    && x.Sirda==y.Sirda
                    && x.Sirdm==y.Sirdm
                    && x.Sirdj==y.Sirdj
                    && x.Sihia==y.Sihia
                    && x.Sihim==y.Sihim
                    && x.Sihij==y.Sihij
                    && x.Sihih==y.Sihih
                    && x.Sihiu==y.Sihiu
                    && x.Sircd==y.Sircd
                    && x.Siihb==y.Siihb
                    && x.Siroa==y.Siroa
                    && x.Sirom==y.Sirom
                    && x.Siroj==y.Siroj
                    && x.Sirou==y.Sirou
                    && x.Sidra==y.Sidra
                    && x.Sidrm==y.Sidrm
                    && x.Sidrj==y.Sidrj
                    && x.Sidba==y.Sidba
                    && x.Sidbm==y.Sidbm
                    && x.Sidbj==y.Sidbj
                    && x.Sicvr==y.Sicvr
                    && x.Sis36==y.Sis36
                    && x.Simng==y.Simng
                    && x.Siclo==y.Siclo
                    && x.Siexn==y.Siexn
                    && x.Sirct==y.Sirct
                    && x.Simtb==y.Simtb
                    && x.Simts==y.Simts
                    && x.Siemi==y.Siemi
                    && x.Sioup==y.Sioup
                    && x.Sicna==y.Sicna
                    && x.Sispp==y.Sispp
                    && x.Sisps==y.Sisps
                    && x.Simfp==y.Simfp
                    && x.Sipf==y.Sipf
                    && x.Sicbc==y.Sicbc
                    && x.Sibcr==y.Sibcr
                    && x.Sibou==y.Sibou
                    && x.Sievn==y.Sievn
                    && x.Sigar==y.Sigar
                    && x.Sinnc==y.Sinnc
                    && x.Siejr==y.Siejr
                    && x.Siejs==y.Siejs
                    && x.Sirua==y.Sirua
                    && x.Sirum==y.Sirum
                    && x.Siruj==y.Siruj
                    && x.Si15a==y.Si15a
                    && x.Si15m==y.Si15m
                    && x.Si15j==y.Si15j
                    && x.Siadh==y.Siadh
                    && x.Silco==y.Silco
                    && x.Sifil==y.Sifil
                    && x.Sicok==y.Sicok
                    && x.Sinlc==y.Sinlc
                    && x.Sie1a==y.Sie1a
                    && x.Sie1m==y.Sie1m
                    && x.Sie1j==y.Sie1j
                    && x.Siin5==y.Siin5  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Sisua.GetHashCode() ) 
                      * 23 ) + (this.Sinum.GetHashCode() ) 
                      * 23 ) + (this.Sisbr?? "").GetHashCode()
                      * 23 ) + (this.Sisum.GetHashCode() ) 
                      * 23 ) + (this.Sisuj.GetHashCode() ) 
                      * 23 ) + (this.Sioua.GetHashCode() ) 
                      * 23 ) + (this.Sioum.GetHashCode() ) 
                      * 23 ) + (this.Siouj.GetHashCode() ) 
                      * 23 ) + (this.Siipb?? "").GetHashCode()
                      * 23 ) + (this.Sialx.GetHashCode() ) 
                      * 23 ) + (this.Siavn.GetHashCode() ) 
                      * 23 ) + (this.Sitch.GetHashCode() ) 
                      * 23 ) + (this.Sitbr?? "").GetHashCode()
                      * 23 ) + (this.Sitsb?? "").GetHashCode()
                      * 23 ) + (this.Siges?? "").GetHashCode()
                      * 23 ) + (this.Sirca.GetHashCode() ) 
                      * 23 ) + (this.Sircm.GetHashCode() ) 
                      * 23 ) + (this.Sircj.GetHashCode() ) 
                      * 23 ) + (this.Sintr?? "").GetHashCode()
                      * 23 ) + (this.Sicau?? "").GetHashCode()
                      * 23 ) + (this.Sijja.GetHashCode() ) 
                      * 23 ) + (this.Sijjm.GetHashCode() ) 
                      * 23 ) + (this.Sijjj.GetHashCode() ) 
                      * 23 ) + (this.Siref?? "").GetHashCode()
                      * 23 ) + (this.Siad1?? "").GetHashCode()
                      * 23 ) + (this.Siad2?? "").GetHashCode()
                      * 23 ) + (this.Sidep?? "").GetHashCode()
                      * 23 ) + (this.Sicpo?? "").GetHashCode()
                      * 23 ) + (this.Sivil?? "").GetHashCode()
                      * 23 ) + (this.Sipay?? "").GetHashCode()
                      * 23 ) + (this.Siexp.GetHashCode() ) 
                      * 23 ) + (this.Siine.GetHashCode() ) 
                      * 23 ) + (this.Sirex?? "").GetHashCode()
                      * 23 ) + (this.Siexm?? "").GetHashCode()
                      * 23 ) + (this.Sierp?? "").GetHashCode()
                      * 23 ) + (this.Siejp.GetHashCode() ) 
                      * 23 ) + (this.Sierd?? "").GetHashCode()
                      * 23 ) + (this.Siejd.GetHashCode() ) 
                      * 23 ) + (this.Sinju?? "").GetHashCode()
                      * 23 ) + (this.Siavo.GetHashCode() ) 
                      * 23 ) + (this.Siina.GetHashCode() ) 
                      * 23 ) + (this.Sirav?? "").GetHashCode()
                      * 23 ) + (this.Siict.GetHashCode() ) 
                      * 23 ) + (this.Siinc.GetHashCode() ) 
                      * 23 ) + (this.Sirco?? "").GetHashCode()
                      * 23 ) + (this.Sidct?? "").GetHashCode()
                      * 23 ) + (this.Sicct?? "").GetHashCode()
                      * 23 ) + (this.Siapr?? "").GetHashCode()
                      * 23 ) + (this.Sirap?? "").GetHashCode()
                      * 23 ) + (this.Siapp.GetHashCode() ) 
                      * 23 ) + (this.Sipcv.GetHashCode() ) 
                      * 23 ) + (this.Sinpl?? "").GetHashCode()
                      * 23 ) + (this.Singe?? "").GetHashCode()
                      * 23 ) + (this.Siacc?? "").GetHashCode()
                      * 23 ) + (this.Sirfu?? "").GetHashCode()
                      * 23 ) + (this.Sicru?? "").GetHashCode()
                      * 23 ) + (this.Sicra.GetHashCode() ) 
                      * 23 ) + (this.Sicrm.GetHashCode() ) 
                      * 23 ) + (this.Sicrj.GetHashCode() ) 
                      * 23 ) + (this.Simju?? "").GetHashCode()
                      * 23 ) + (this.Simja.GetHashCode() ) 
                      * 23 ) + (this.Simjm.GetHashCode() ) 
                      * 23 ) + (this.Simjj.GetHashCode() ) 
                      * 23 ) + (this.Sisit?? "").GetHashCode()
                      * 23 ) + (this.Sista.GetHashCode() ) 
                      * 23 ) + (this.Sistm.GetHashCode() ) 
                      * 23 ) + (this.Sistj.GetHashCode() ) 
                      * 23 ) + (this.Sieta?? "").GetHashCode()
                      * 23 ) + (this.Sidre?? "").GetHashCode()
                      * 23 ) + (this.Sidrn?? "").GetHashCode()
                      * 23 ) + (this.Sicvg?? "").GetHashCode()
                      * 23 ) + (this.Sirpa.GetHashCode() ) 
                      * 23 ) + (this.Sirpm.GetHashCode() ) 
                      * 23 ) + (this.Sirpj.GetHashCode() ) 
                      * 23 ) + (this.Sirda.GetHashCode() ) 
                      * 23 ) + (this.Sirdm.GetHashCode() ) 
                      * 23 ) + (this.Sirdj.GetHashCode() ) 
                      * 23 ) + (this.Sihia.GetHashCode() ) 
                      * 23 ) + (this.Sihim.GetHashCode() ) 
                      * 23 ) + (this.Sihij.GetHashCode() ) 
                      * 23 ) + (this.Sihih.GetHashCode() ) 
                      * 23 ) + (this.Sihiu?? "").GetHashCode()
                      * 23 ) + (this.Sircd?? "").GetHashCode()
                      * 23 ) + (this.Siihb?? "").GetHashCode()
                      * 23 ) + (this.Siroa.GetHashCode() ) 
                      * 23 ) + (this.Sirom.GetHashCode() ) 
                      * 23 ) + (this.Siroj.GetHashCode() ) 
                      * 23 ) + (this.Sirou?? "").GetHashCode()
                      * 23 ) + (this.Sidra.GetHashCode() ) 
                      * 23 ) + (this.Sidrm.GetHashCode() ) 
                      * 23 ) + (this.Sidrj.GetHashCode() ) 
                      * 23 ) + (this.Sidba.GetHashCode() ) 
                      * 23 ) + (this.Sidbm.GetHashCode() ) 
                      * 23 ) + (this.Sidbj.GetHashCode() ) 
                      * 23 ) + (this.Sicvr?? "").GetHashCode()
                      * 23 ) + (this.Sis36?? "").GetHashCode()
                      * 23 ) + (this.Simng.GetHashCode() ) 
                      * 23 ) + (this.Siclo?? "").GetHashCode()
                      * 23 ) + (this.Siexn?? "").GetHashCode()
                      * 23 ) + (this.Sirct?? "").GetHashCode()
                      * 23 ) + (this.Simtb.GetHashCode() ) 
                      * 23 ) + (this.Simts.GetHashCode() ) 
                      * 23 ) + (this.Siemi?? "").GetHashCode()
                      * 23 ) + (this.Sioup?? "").GetHashCode()
                      * 23 ) + (this.Sicna?? "").GetHashCode()
                      * 23 ) + (this.Sispp.GetHashCode() ) 
                      * 23 ) + (this.Sisps.GetHashCode() ) 
                      * 23 ) + (this.Simfp.GetHashCode() ) 
                      * 23 ) + (this.Sipf.GetHashCode() ) 
                      * 23 ) + (this.Sicbc?? "").GetHashCode()
                      * 23 ) + (this.Sibcr?? "").GetHashCode()
                      * 23 ) + (this.Sibou?? "").GetHashCode()
                      * 23 ) + (this.Sievn?? "").GetHashCode()
                      * 23 ) + (this.Sigar?? "").GetHashCode()
                      * 23 ) + (this.Sinnc.GetHashCode() ) 
                      * 23 ) + (this.Siejr.GetHashCode() ) 
                      * 23 ) + (this.Siejs?? "").GetHashCode()
                      * 23 ) + (this.Sirua.GetHashCode() ) 
                      * 23 ) + (this.Sirum.GetHashCode() ) 
                      * 23 ) + (this.Siruj.GetHashCode() ) 
                      * 23 ) + (this.Si15a.GetHashCode() ) 
                      * 23 ) + (this.Si15m.GetHashCode() ) 
                      * 23 ) + (this.Si15j.GetHashCode() ) 
                      * 23 ) + (this.Siadh.GetHashCode() ) 
                      * 23 ) + (this.Silco?? "").GetHashCode()
                      * 23 ) + (this.Sifil?? "").GetHashCode()
                      * 23 ) + (this.Sicok?? "").GetHashCode()
                      * 23 ) + (this.Sinlc?? "").GetHashCode()
                      * 23 ) + (this.Sie1a.GetHashCode() ) 
                      * 23 ) + (this.Sie1m.GetHashCode() ) 
                      * 23 ) + (this.Sie1j.GetHashCode() ) 
                      * 23 ) + (this.Siin5.GetHashCode() )                    );
           }
        }
    }
}
