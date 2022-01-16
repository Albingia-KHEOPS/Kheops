using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YHPBASE
    public partial class YpoBase  {
             //YHPBASE
             //YPOBASE

            ///<summary>Public empty contructor</summary>
            public YpoBase() {}
            ///<summary>Public empty contructor</summary>
            public YpoBase(YpoBase copyFrom) 
            {
                  this.Pbipb= copyFrom.Pbipb;
                  this.Pbalx= copyFrom.Pbalx;
                  this.Pbavn= copyFrom.Pbavn;
                  this.Pbhin= copyFrom.Pbhin;
                  this.Pbhia= copyFrom.Pbhia;
                  this.Pbhim= copyFrom.Pbhim;
                  this.Pbhij= copyFrom.Pbhij;
                  this.Pbhih= copyFrom.Pbhih;
                  this.Pbhiu= copyFrom.Pbhiu;
                  this.Pbtbr= copyFrom.Pbtbr;
                  this.Pbias= copyFrom.Pbias;
                  this.Pbref= copyFrom.Pbref;
                  this.Pbbra= copyFrom.Pbbra;
                  this.Pbsbr= copyFrom.Pbsbr;
                  this.Pbcat= copyFrom.Pbcat;
                  this.Pbict= copyFrom.Pbict;
                  this.Pbtil= copyFrom.Pbtil;
                  this.Pbinl= copyFrom.Pbinl;
                  this.Pboct= copyFrom.Pboct;
                  this.Pbad1= copyFrom.Pbad1;
                  this.Pbad2= copyFrom.Pbad2;
                  this.Pbdep= copyFrom.Pbdep;
                  this.Pbcpo= copyFrom.Pbcpo;
                  this.Pbvil= copyFrom.Pbvil;
                  this.Pbpay= copyFrom.Pbpay;
                  this.Pbsaa= copyFrom.Pbsaa;
                  this.Pbsam= copyFrom.Pbsam;
                  this.Pbsaj= copyFrom.Pbsaj;
                  this.Pbsah= copyFrom.Pbsah;
                  this.Pbapr= copyFrom.Pbapr;
                  this.Pbapp= copyFrom.Pbapp;
                  this.Pbmo1= copyFrom.Pbmo1;
                  this.Pbmo2= copyFrom.Pbmo2;
                  this.Pbmo3= copyFrom.Pbmo3;
                  this.Pbcle= copyFrom.Pbcle;
                  this.Pbant= copyFrom.Pbant;
                  this.Pbctd= copyFrom.Pbctd;
                  this.Pbctu= copyFrom.Pbctu;
                  this.Pbapo= copyFrom.Pbapo;
                  this.Pbdur= copyFrom.Pbdur;
                  this.Pbrel= copyFrom.Pbrel;
                  this.Pbrld= copyFrom.Pbrld;
                  this.Pbatt= copyFrom.Pbatt;
                  this.Pbrmp= copyFrom.Pbrmp;
                  this.Pbvrf= copyFrom.Pbvrf;
                  this.Pbolv= copyFrom.Pbolv;
                  this.Pbfoa= copyFrom.Pbfoa;
                  this.Pbfom= copyFrom.Pbfom;
                  this.Pbcou= copyFrom.Pbcou;
                  this.Pbfoe= copyFrom.Pbfoe;
                  this.Pbdev= copyFrom.Pbdev;
                  this.Pbrgt= copyFrom.Pbrgt;
                  this.Pbtco= copyFrom.Pbtco;
                  this.Pbnat= copyFrom.Pbnat;
                  this.Pbdst= copyFrom.Pbdst;
                  this.Pblie= copyFrom.Pblie;
                  this.Pbges= copyFrom.Pbges;
                  this.Pbsou= copyFrom.Pbsou;
                  this.Pbori= copyFrom.Pbori;
                  this.Pbmai= copyFrom.Pbmai;
                  this.Pbefa= copyFrom.Pbefa;
                  this.Pbefm= copyFrom.Pbefm;
                  this.Pbefj= copyFrom.Pbefj;
                  this.Pboff= copyFrom.Pboff;
                  this.Pbofv= copyFrom.Pbofv;
                  this.Pbipa= copyFrom.Pbipa;
                  this.Pbala= copyFrom.Pbala;
                  this.Pbecm= copyFrom.Pbecm;
                  this.Pbecj= copyFrom.Pbecj;
                  this.Pbpcv= copyFrom.Pbpcv;
                  this.Pbcta= copyFrom.Pbcta;
                  this.Pbnrq= copyFrom.Pbnrq;
                  this.Pbnpl= copyFrom.Pbnpl;
                  this.Pbper= copyFrom.Pbper;
                  this.Pbavc= copyFrom.Pbavc;
                  this.Pbava= copyFrom.Pbava;
                  this.Pbavm= copyFrom.Pbavm;
                  this.Pbavj= copyFrom.Pbavj;
                  this.Pbrsc= copyFrom.Pbrsc;
                  this.Pbrsa= copyFrom.Pbrsa;
                  this.Pbrsm= copyFrom.Pbrsm;
                  this.Pbrsj= copyFrom.Pbrsj;
                  this.Pbmer= copyFrom.Pbmer;
                  this.Pbeta= copyFrom.Pbeta;
                  this.Pbsit= copyFrom.Pbsit;
                  this.Pbsta= copyFrom.Pbsta;
                  this.Pbstm= copyFrom.Pbstm;
                  this.Pbstj= copyFrom.Pbstj;
                  this.Pbstq= copyFrom.Pbstq;
                  this.Pbedt= copyFrom.Pbedt;
                  this.Pbtac= copyFrom.Pbtac;
                  this.Pbtaa= copyFrom.Pbtaa;
                  this.Pbtam= copyFrom.Pbtam;
                  this.Pbtaj= copyFrom.Pbtaj;
                  this.Pbcru= copyFrom.Pbcru;
                  this.Pbcra= copyFrom.Pbcra;
                  this.Pbcrm= copyFrom.Pbcrm;
                  this.Pbcrj= copyFrom.Pbcrj;
                  this.Pbmju= copyFrom.Pbmju;
                  this.Pbmja= copyFrom.Pbmja;
                  this.Pbmjm= copyFrom.Pbmjm;
                  this.Pbmjj= copyFrom.Pbmjj;
                  this.Pbdeu= copyFrom.Pbdeu;
                  this.Pbdea= copyFrom.Pbdea;
                  this.Pbdem= copyFrom.Pbdem;
                  this.Pbdej= copyFrom.Pbdej;
                  this.Pbctp= copyFrom.Pbctp;
                  this.Pbefh= copyFrom.Pbefh;
                  this.Pbfea= copyFrom.Pbfea;
                  this.Pbfem= copyFrom.Pbfem;
                  this.Pbfej= copyFrom.Pbfej;
                  this.Pbfeh= copyFrom.Pbfeh;
                  this.Pbstp= copyFrom.Pbstp;
                  this.Pbnva= copyFrom.Pbnva;
                  this.Pbfec= copyFrom.Pbfec;
                  this.Pbpor= copyFrom.Pbpor;
                  this.Pbcon= copyFrom.Pbcon;
                  this.Pbttr= copyFrom.Pbttr;
                  this.Pbavk= copyFrom.Pbavk;
                  this.Pbadh= copyFrom.Pbadh;
                  this.Pbstf= copyFrom.Pbstf;
                  this.Pbpim= copyFrom.Pbpim;
                  this.Pbin5= copyFrom.Pbin5;
                  this.Pbork= copyFrom.Pbork;
                  this.Pbhty= copyFrom.Pbhty;
                  this.Pbhna= copyFrom.Pbhna;
                  this.Pbhdt= copyFrom.Pbhdt;
                  this.Pbhza= copyFrom.Pbhza;
                  this.Pbtyp= copyFrom.Pbtyp;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Pbipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pbalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Pbavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Pbhin { get; set; } 
            
            ///<summary> Année Mise en historique </summary>
            public int? Pbhia { get; set; } 
            
            ///<summary> Mois  Mise en historique </summary>
            public int? Pbhim { get; set; } 
            
            ///<summary> Jour  Mise en Historique </summary>
            public int? Pbhij { get; set; } 
            
            ///<summary> Heure Mise en historique </summary>
            public int? Pbhih { get; set; } 
            
            ///<summary> User  Mise en Historique </summary>
            public string Pbhiu { get; set; } 
            
            ///<summary> CO ou RT </summary>
            public string Pbtbr { get; set; } 
            
            ///<summary> Identifiant Assuré 10/00 </summary>
            public int Pbias { get; set; } 
            
            ///<summary> Référence </summary>
            public string Pbref { get; set; } 
            
            ///<summary> Branche </summary>
            public string Pbbra { get; set; } 
            
            ///<summary> Sous-branche </summary>
            public string Pbsbr { get; set; } 
            
            ///<summary> Catégorie </summary>
            public string Pbcat { get; set; } 
            
            ///<summary> Courtier </summary>
            public int Pbict { get; set; } 
            
            ///<summary> Type interlocuteur </summary>
            public string Pbtil { get; set; } 
            
            ///<summary> Interlocuteur </summary>
            public int Pbinl { get; set; } 
            
            ///<summary> Référence chez courtier </summary>
            public string Pboct { get; set; } 
            
            ///<summary> Adresse 1 du risque </summary>
            public string Pbad1 { get; set; } 
            
            ///<summary> Adresse 2 du risque </summary>
            public string Pbad2 { get; set; } 
            
            ///<summary> Département du risque </summary>
            public string Pbdep { get; set; } 
            
            ///<summary> 3 dern.car.code postal du risque </summary>
            public string Pbcpo { get; set; } 
            
            ///<summary> Ville du risque </summary>
            public string Pbvil { get; set; } 
            
            ///<summary> Code pays du risque </summary>
            public string Pbpay { get; set; } 
            
            ///<summary> Année de saisie Cie ou accord </summary>
            public int Pbsaa { get; set; } 
            
            ///<summary> Mois de saisie ou accord </summary>
            public int Pbsam { get; set; } 
            
            ///<summary> Jour de saisie ou accord </summary>
            public int Pbsaj { get; set; } 
            
            ///<summary> Heure de saisie </summary>
            public int Pbsah { get; set; } 
            
            ///<summary> Apériteur </summary>
            public string Pbapr { get; set; } 
            
            ///<summary> % Apérition  Albingia </summary>
            public Decimal Pbapp { get; set; } 
            
            ///<summary> Mot clé 1 </summary>
            public string Pbmo1 { get; set; } 
            
            ///<summary> Mot clé 2 </summary>
            public string Pbmo2 { get; set; } 
            
            ///<summary> Mot clé 3 </summary>
            public string Pbmo3 { get; set; } 
            
            ///<summary> Gestion des clauses O/N </summary>
            public string Pbcle { get; set; } 
            
            ///<summary> Code antécédent </summary>
            public string Pbant { get; set; } 
            
            ///<summary> Durée de la police </summary>
            public int Pbctd { get; set; } 
            
            ///<summary> Unité de la durée police </summary>
            public string Pbctu { get; set; } 
            
            ///<summary> Offre adressé à l'apériteur O/N </summary>
            public string Pbapo { get; set; } 
            
            ///<summary> Offre Durée de l'offre en jour </summary>
            public int Pbdur { get; set; } 
            
            ///<summary> Offre Relance de l'offre O/N </summary>
            public string Pbrel { get; set; } 
            
            ///<summary> Offre Délai de la relance en jour </summary>
            public int Pbrld { get; set; } 
            
            ///<summary> Offre Délai attente de l'offre </summary>
            public int Pbatt { get; set; } 
            
            ///<summary> Offre Version remplacée O/N </summary>
            public string Pbrmp { get; set; } 
            
            ///<summary> OffreN° Version de référence </summary>
            public int Pbvrf { get; set; } 
            
            ///<summary> Offre N° Version modifiée </summary>
            public int Pbolv { get; set; } 
            
            ///<summary> Offre Action de fin d'offre </summary>
            public int Pbfoa { get; set; } 
            
            ///<summary> Offre Motif de l'action </summary>
            public string Pbfom { get; set; } 
            
            ///<summary> Offre N° de courrier </summary>
            public int Pbcou { get; set; } 
            
            ///<summary> Offre entête libre O/N let.Accomp. </summary>
            public string Pbfoe { get; set; } 
            
            ///<summary> Code devise </summary>
            public string Pbdev { get; set; } 
            
            ///<summary> Régime de taxe </summary>
            public string Pbrgt { get; set; } 
            
            ///<summary> Type de construction </summary>
            public string Pbtco { get; set; } 
            
            ///<summary> Nature des travaux </summary>
            public string Pbnat { get; set; } 
            
            ///<summary> Code destination </summary>
            public string Pbdst { get; set; } 
            
            ///<summary> Autres courtiers O/N Lien si refus </summary>
            public string Pblie { get; set; } 
            
            ///<summary> Gestionnaire </summary>
            public string Pbges { get; set; } 
            
            ///<summary> Souscripteur </summary>
            public string Pbsou { get; set; } 
            
            ///<summary> Code origine </summary>
            public string Pbori { get; set; } 
            
            ///<summary> Numéro de mailing </summary>
            public int Pbmai { get; set; } 
            
            ///<summary> Année Date effet police </summary>
            public int Pbefa { get; set; } 
            
            ///<summary> Mois Date effet police </summary>
            public int Pbefm { get; set; } 
            
            ///<summary> Jour Date effet police </summary>
            public int Pbefj { get; set; } 
            
            ///<summary> N° Offre si police </summary>
            public string Pboff { get; set; } 
            
            ///<summary> N° version de l'offre si police </summary>
            public int Pbofv { get; set; } 
            
            ///<summary> N° de Police Associée </summary>
            public string Pbipa { get; set; } 
            
            ///<summary> N° aliment Police associée </summary>
            public int Pbala { get; set; } 
            
            ///<summary> Echéance principale : Mois </summary>
            public int Pbecm { get; set; } 
            
            ///<summary> Echéance principale : jour </summary>
            public int Pbecj { get; set; } 
            
            ///<summary> % Couvert </summary>
            public int Pbpcv { get; set; } 
            
            ///<summary> Id courtier Apporteur </summary>
            public int Pbcta { get; set; } 
            
            ///<summary> Nature du risque </summary>
            public string Pbnrq { get; set; } 
            
            ///<summary> Nature Police </summary>
            public string Pbnpl { get; set; } 
            
            ///<summary> Code périodicité </summary>
            public string Pbper { get; set; } 
            
            ///<summary> Motif avenant </summary>
            public string Pbavc { get; set; } 
            
            ///<summary> Année effet avenant </summary>
            public int Pbava { get; set; } 
            
            ///<summary> Mois effet avenant </summary>
            public int Pbavm { get; set; } 
            
            ///<summary> Jour effet avenant </summary>
            public int Pbavj { get; set; } 
            
            ///<summary> Motif résiliation/Suspension </summary>
            public string Pbrsc { get; set; } 
            
            ///<summary> Année résiliation/suspension </summary>
            public int Pbrsa { get; set; } 
            
            ///<summary> Mois résiliation/suspension </summary>
            public int Pbrsm { get; set; } 
            
            ///<summary> Jour résiliation/suspension </summary>
            public int Pbrsj { get; set; } 
            
            ///<summary> Police mère ou aliment (M/A/ ) </summary>
            public string Pbmer { get; set; } 
            
            ///<summary> Etat police (V validé N non validé) </summary>
            public string Pbeta { get; set; } 
            
            ///<summary> Code situation </summary>
            public string Pbsit { get; set; } 
            
            ///<summary> Année de situation </summary>
            public int Pbsta { get; set; } 
            
            ///<summary> Mois de situation </summary>
            public int Pbstm { get; set; } 
            
            ///<summary> Jour de situation </summary>
            public int Pbstj { get; set; } 
            
            ///<summary> Qualité sit Police-Fini-Régul-Note.c </summary>
            public string Pbstq { get; set; } 
            
            ///<summary> Police éditée O/N </summary>
            public string Pbedt { get; set; } 
            
            ///<summary> Type accord S Signée N Non signée .. </summary>
            public string Pbtac { get; set; } 
            
            ///<summary> Année accord </summary>
            public int Pbtaa { get; set; } 
            
            ///<summary> Mois Accord </summary>
            public int Pbtam { get; set; } 
            
            ///<summary> Jour accord </summary>
            public int Pbtaj { get; set; } 
            
            ///<summary> User Création </summary>
            public string Pbcru { get; set; } 
            
            ///<summary> Année Création </summary>
            public int Pbcra { get; set; } 
            
            ///<summary> Mois création </summary>
            public int Pbcrm { get; set; } 
            
            ///<summary> Jour création </summary>
            public int Pbcrj { get; set; } 
            
            ///<summary> User Màj </summary>
            public string Pbmju { get; set; } 
            
            ///<summary> Année Màj </summary>
            public int Pbmja { get; set; } 
            
            ///<summary> Mois Màj </summary>
            public int Pbmjm { get; set; } 
            
            ///<summary> Jour Màj </summary>
            public int Pbmjj { get; set; } 
            
            ///<summary> Date Entrée en machine : User </summary>
            public string Pbdeu { get; set; } 
            
            ///<summary> Date Entrée en machine : Année </summary>
            public int Pbdea { get; set; } 
            
            ///<summary> Date Entrée en machine : Mois </summary>
            public int Pbdem { get; set; } 
            
            ///<summary> Date Entrée en machine : Jour </summary>
            public int Pbdej { get; set; } 
            
            ///<summary> Etablissement payeur (Courtier) </summary>
            public int Pbctp { get; set; } 
            
            ///<summary> Début Effet : Heure </summary>
            public int Pbefh { get; set; } 
            
            ///<summary> Date Fin Effet : Année </summary>
            public int Pbfea { get; set; } 
            
            ///<summary> Date Fin Effet : Mois </summary>
            public int Pbfem { get; set; } 
            
            ///<summary> Date Fin Effet : Jour </summary>
            public int Pbfej { get; set; } 
            
            ///<summary> Fin Effet : Heure </summary>
            public int Pbfeh { get; set; } 
            
            ///<summary> Top stop </summary>
            public string Pbstp { get; set; } 
            
            ///<summary> Motif de non Validation </summary>
            public string Pbnva { get; set; } 
            
            ///<summary> Cause de fin de garantie </summary>
            public string Pbfec { get; set; } 
            
            ///<summary> Remplacement de police O/' '  POCONX </summary>
            public string Pbpor { get; set; } 
            
            ///<summary> Code contentieux </summary>
            public string Pbcon { get; set; } 
            
            ///<summary> Type de traitement (Affnouv/avenant) </summary>
            public string Pbttr { get; set; } 
            
            ///<summary> N° Avenant externe Spal </summary>
            public int Pbavk { get; set; } 
            
            ///<summary> Numéro chrono Adresse </summary>
            public int Pbadh { get; set; } 
            
            ///<summary> Motif de situation (Refus ...) </summary>
            public string Pbstf { get; set; } 
            
            ///<summary> Code périmètre </summary>
            public string Pbpim { get; set; } 
            
            ///<summary> Code interlocuteur sur 5 </summary>
            public int Pbin5 { get; set; } 
            
            ///<summary> KHEOPS Statut '  ' 'REC' 'REP' 'KHE' </summary>
            public string Pbork { get; set; } 
            
            ///<summary> Typologie Histo   AVN TERme RCFrecon </summary>
            public string Pbhty { get; set; } 
            
            ///<summary> Histo intermédiare N° avn </summary>
            public string Pbhna { get; set; } 
            
            ///<summary> Histo interm  Date terme/reconf... </summary>
            public int? Pbhdt { get; set; } 
            
            ///<summary> Histo Interm  Zone alpha </summary>
            public string Pbhza { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Pbtyp { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoBase  x=this,  y=obj as YpoBase;
            if( y == default(YpoBase) ) return false;
            return (
                    x.Pbipb==y.Pbipb
                    && x.Pbalx==y.Pbalx
                    && x.Pbavn==y.Pbavn
                    && x.Pbtbr==y.Pbtbr
                    && x.Pbias==y.Pbias
                    && x.Pbref==y.Pbref
                    && x.Pbbra==y.Pbbra
                    && x.Pbsbr==y.Pbsbr
                    && x.Pbcat==y.Pbcat
                    && x.Pbict==y.Pbict
                    && x.Pbtil==y.Pbtil
                    && x.Pbinl==y.Pbinl
                    && x.Pboct==y.Pboct
                    && x.Pbad1==y.Pbad1
                    && x.Pbad2==y.Pbad2
                    && x.Pbdep==y.Pbdep
                    && x.Pbcpo==y.Pbcpo
                    && x.Pbvil==y.Pbvil
                    && x.Pbpay==y.Pbpay
                    && x.Pbsaa==y.Pbsaa
                    && x.Pbsam==y.Pbsam
                    && x.Pbsaj==y.Pbsaj
                    && x.Pbsah==y.Pbsah
                    && x.Pbapr==y.Pbapr
                    && x.Pbapp==y.Pbapp
                    && x.Pbmo1==y.Pbmo1
                    && x.Pbmo2==y.Pbmo2
                    && x.Pbmo3==y.Pbmo3
                    && x.Pbcle==y.Pbcle
                    && x.Pbant==y.Pbant
                    && x.Pbctd==y.Pbctd
                    && x.Pbctu==y.Pbctu
                    && x.Pbapo==y.Pbapo
                    && x.Pbdur==y.Pbdur
                    && x.Pbrel==y.Pbrel
                    && x.Pbrld==y.Pbrld
                    && x.Pbatt==y.Pbatt
                    && x.Pbrmp==y.Pbrmp
                    && x.Pbvrf==y.Pbvrf
                    && x.Pbolv==y.Pbolv
                    && x.Pbfoa==y.Pbfoa
                    && x.Pbfom==y.Pbfom
                    && x.Pbcou==y.Pbcou
                    && x.Pbfoe==y.Pbfoe
                    && x.Pbdev==y.Pbdev
                    && x.Pbrgt==y.Pbrgt
                    && x.Pbtco==y.Pbtco
                    && x.Pbnat==y.Pbnat
                    && x.Pbdst==y.Pbdst
                    && x.Pblie==y.Pblie
                    && x.Pbges==y.Pbges
                    && x.Pbsou==y.Pbsou
                    && x.Pbori==y.Pbori
                    && x.Pbmai==y.Pbmai
                    && x.Pbefa==y.Pbefa
                    && x.Pbefm==y.Pbefm
                    && x.Pbefj==y.Pbefj
                    && x.Pboff==y.Pboff
                    && x.Pbofv==y.Pbofv
                    && x.Pbipa==y.Pbipa
                    && x.Pbala==y.Pbala
                    && x.Pbecm==y.Pbecm
                    && x.Pbecj==y.Pbecj
                    && x.Pbpcv==y.Pbpcv
                    && x.Pbcta==y.Pbcta
                    && x.Pbnrq==y.Pbnrq
                    && x.Pbnpl==y.Pbnpl
                    && x.Pbper==y.Pbper
                    && x.Pbavc==y.Pbavc
                    && x.Pbava==y.Pbava
                    && x.Pbavm==y.Pbavm
                    && x.Pbavj==y.Pbavj
                    && x.Pbrsc==y.Pbrsc
                    && x.Pbrsa==y.Pbrsa
                    && x.Pbrsm==y.Pbrsm
                    && x.Pbrsj==y.Pbrsj
                    && x.Pbmer==y.Pbmer
                    && x.Pbeta==y.Pbeta
                    && x.Pbsit==y.Pbsit
                    && x.Pbsta==y.Pbsta
                    && x.Pbstm==y.Pbstm
                    && x.Pbstj==y.Pbstj
                    && x.Pbstq==y.Pbstq
                    && x.Pbedt==y.Pbedt
                    && x.Pbtac==y.Pbtac
                    && x.Pbtaa==y.Pbtaa
                    && x.Pbtam==y.Pbtam
                    && x.Pbtaj==y.Pbtaj
                    && x.Pbcru==y.Pbcru
                    && x.Pbcra==y.Pbcra
                    && x.Pbcrm==y.Pbcrm
                    && x.Pbcrj==y.Pbcrj
                    && x.Pbmju==y.Pbmju
                    && x.Pbmja==y.Pbmja
                    && x.Pbmjm==y.Pbmjm
                    && x.Pbmjj==y.Pbmjj
                    && x.Pbdeu==y.Pbdeu
                    && x.Pbdea==y.Pbdea
                    && x.Pbdem==y.Pbdem
                    && x.Pbdej==y.Pbdej
                    && x.Pbctp==y.Pbctp
                    && x.Pbefh==y.Pbefh
                    && x.Pbfea==y.Pbfea
                    && x.Pbfem==y.Pbfem
                    && x.Pbfej==y.Pbfej
                    && x.Pbfeh==y.Pbfeh
                    && x.Pbstp==y.Pbstp
                    && x.Pbnva==y.Pbnva
                    && x.Pbfec==y.Pbfec
                    && x.Pbpor==y.Pbpor
                    && x.Pbcon==y.Pbcon
                    && x.Pbttr==y.Pbttr
                    && x.Pbavk==y.Pbavk
                    && x.Pbadh==y.Pbadh
                    && x.Pbstf==y.Pbstf
                    && x.Pbpim==y.Pbpim
                    && x.Pbin5==y.Pbin5
                    && x.Pbork==y.Pbork
                    && x.Pbtyp==y.Pbtyp  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Pbipb?? "").GetHashCode()
                      * 23 ) + (this.Pbalx.GetHashCode() ) 
                      * 23 ) + (this.Pbavn.GetHashCode() ) 
                      * 23 ) + (this.Pbtbr?? "").GetHashCode()
                      * 23 ) + (this.Pbias.GetHashCode() ) 
                      * 23 ) + (this.Pbref?? "").GetHashCode()
                      * 23 ) + (this.Pbbra?? "").GetHashCode()
                      * 23 ) + (this.Pbsbr?? "").GetHashCode()
                      * 23 ) + (this.Pbcat?? "").GetHashCode()
                      * 23 ) + (this.Pbict.GetHashCode() ) 
                      * 23 ) + (this.Pbtil?? "").GetHashCode()
                      * 23 ) + (this.Pbinl.GetHashCode() ) 
                      * 23 ) + (this.Pboct?? "").GetHashCode()
                      * 23 ) + (this.Pbad1?? "").GetHashCode()
                      * 23 ) + (this.Pbad2?? "").GetHashCode()
                      * 23 ) + (this.Pbdep?? "").GetHashCode()
                      * 23 ) + (this.Pbcpo?? "").GetHashCode()
                      * 23 ) + (this.Pbvil?? "").GetHashCode()
                      * 23 ) + (this.Pbpay?? "").GetHashCode()
                      * 23 ) + (this.Pbsaa.GetHashCode() ) 
                      * 23 ) + (this.Pbsam.GetHashCode() ) 
                      * 23 ) + (this.Pbsaj.GetHashCode() ) 
                      * 23 ) + (this.Pbsah.GetHashCode() ) 
                      * 23 ) + (this.Pbapr?? "").GetHashCode()
                      * 23 ) + (this.Pbapp.GetHashCode() ) 
                      * 23 ) + (this.Pbmo1?? "").GetHashCode()
                      * 23 ) + (this.Pbmo2?? "").GetHashCode()
                      * 23 ) + (this.Pbmo3?? "").GetHashCode()
                      * 23 ) + (this.Pbcle?? "").GetHashCode()
                      * 23 ) + (this.Pbant?? "").GetHashCode()
                      * 23 ) + (this.Pbctd.GetHashCode() ) 
                      * 23 ) + (this.Pbctu?? "").GetHashCode()
                      * 23 ) + (this.Pbapo?? "").GetHashCode()
                      * 23 ) + (this.Pbdur.GetHashCode() ) 
                      * 23 ) + (this.Pbrel?? "").GetHashCode()
                      * 23 ) + (this.Pbrld.GetHashCode() ) 
                      * 23 ) + (this.Pbatt.GetHashCode() ) 
                      * 23 ) + (this.Pbrmp?? "").GetHashCode()
                      * 23 ) + (this.Pbvrf.GetHashCode() ) 
                      * 23 ) + (this.Pbolv.GetHashCode() ) 
                      * 23 ) + (this.Pbfoa.GetHashCode() ) 
                      * 23 ) + (this.Pbfom?? "").GetHashCode()
                      * 23 ) + (this.Pbcou.GetHashCode() ) 
                      * 23 ) + (this.Pbfoe?? "").GetHashCode()
                      * 23 ) + (this.Pbdev?? "").GetHashCode()
                      * 23 ) + (this.Pbrgt?? "").GetHashCode()
                      * 23 ) + (this.Pbtco?? "").GetHashCode()
                      * 23 ) + (this.Pbnat?? "").GetHashCode()
                      * 23 ) + (this.Pbdst?? "").GetHashCode()
                      * 23 ) + (this.Pblie?? "").GetHashCode()
                      * 23 ) + (this.Pbges?? "").GetHashCode()
                      * 23 ) + (this.Pbsou?? "").GetHashCode()
                      * 23 ) + (this.Pbori?? "").GetHashCode()
                      * 23 ) + (this.Pbmai.GetHashCode() ) 
                      * 23 ) + (this.Pbefa.GetHashCode() ) 
                      * 23 ) + (this.Pbefm.GetHashCode() ) 
                      * 23 ) + (this.Pbefj.GetHashCode() ) 
                      * 23 ) + (this.Pboff?? "").GetHashCode()
                      * 23 ) + (this.Pbofv.GetHashCode() ) 
                      * 23 ) + (this.Pbipa?? "").GetHashCode()
                      * 23 ) + (this.Pbala.GetHashCode() ) 
                      * 23 ) + (this.Pbecm.GetHashCode() ) 
                      * 23 ) + (this.Pbecj.GetHashCode() ) 
                      * 23 ) + (this.Pbpcv.GetHashCode() ) 
                      * 23 ) + (this.Pbcta.GetHashCode() ) 
                      * 23 ) + (this.Pbnrq?? "").GetHashCode()
                      * 23 ) + (this.Pbnpl?? "").GetHashCode()
                      * 23 ) + (this.Pbper?? "").GetHashCode()
                      * 23 ) + (this.Pbavc?? "").GetHashCode()
                      * 23 ) + (this.Pbava.GetHashCode() ) 
                      * 23 ) + (this.Pbavm.GetHashCode() ) 
                      * 23 ) + (this.Pbavj.GetHashCode() ) 
                      * 23 ) + (this.Pbrsc?? "").GetHashCode()
                      * 23 ) + (this.Pbrsa.GetHashCode() ) 
                      * 23 ) + (this.Pbrsm.GetHashCode() ) 
                      * 23 ) + (this.Pbrsj.GetHashCode() ) 
                      * 23 ) + (this.Pbmer?? "").GetHashCode()
                      * 23 ) + (this.Pbeta?? "").GetHashCode()
                      * 23 ) + (this.Pbsit?? "").GetHashCode()
                      * 23 ) + (this.Pbsta.GetHashCode() ) 
                      * 23 ) + (this.Pbstm.GetHashCode() ) 
                      * 23 ) + (this.Pbstj.GetHashCode() ) 
                      * 23 ) + (this.Pbstq?? "").GetHashCode()
                      * 23 ) + (this.Pbedt?? "").GetHashCode()
                      * 23 ) + (this.Pbtac?? "").GetHashCode()
                      * 23 ) + (this.Pbtaa.GetHashCode() ) 
                      * 23 ) + (this.Pbtam.GetHashCode() ) 
                      * 23 ) + (this.Pbtaj.GetHashCode() ) 
                      * 23 ) + (this.Pbcru?? "").GetHashCode()
                      * 23 ) + (this.Pbcra.GetHashCode() ) 
                      * 23 ) + (this.Pbcrm.GetHashCode() ) 
                      * 23 ) + (this.Pbcrj.GetHashCode() ) 
                      * 23 ) + (this.Pbmju?? "").GetHashCode()
                      * 23 ) + (this.Pbmja.GetHashCode() ) 
                      * 23 ) + (this.Pbmjm.GetHashCode() ) 
                      * 23 ) + (this.Pbmjj.GetHashCode() ) 
                      * 23 ) + (this.Pbdeu?? "").GetHashCode()
                      * 23 ) + (this.Pbdea.GetHashCode() ) 
                      * 23 ) + (this.Pbdem.GetHashCode() ) 
                      * 23 ) + (this.Pbdej.GetHashCode() ) 
                      * 23 ) + (this.Pbctp.GetHashCode() ) 
                      * 23 ) + (this.Pbefh.GetHashCode() ) 
                      * 23 ) + (this.Pbfea.GetHashCode() ) 
                      * 23 ) + (this.Pbfem.GetHashCode() ) 
                      * 23 ) + (this.Pbfej.GetHashCode() ) 
                      * 23 ) + (this.Pbfeh.GetHashCode() ) 
                      * 23 ) + (this.Pbstp?? "").GetHashCode()
                      * 23 ) + (this.Pbnva?? "").GetHashCode()
                      * 23 ) + (this.Pbfec?? "").GetHashCode()
                      * 23 ) + (this.Pbpor?? "").GetHashCode()
                      * 23 ) + (this.Pbcon?? "").GetHashCode()
                      * 23 ) + (this.Pbttr?? "").GetHashCode()
                      * 23 ) + (this.Pbavk.GetHashCode() ) 
                      * 23 ) + (this.Pbadh.GetHashCode() ) 
                      * 23 ) + (this.Pbstf?? "").GetHashCode()
                      * 23 ) + (this.Pbpim?? "").GetHashCode()
                      * 23 ) + (this.Pbin5.GetHashCode() ) 
                      * 23 ) + (this.Pbork?? "").GetHashCode()
                      * 23 ) + (this.Pbtyp?? "").GetHashCode()                   );
           }
        }
    }
}
