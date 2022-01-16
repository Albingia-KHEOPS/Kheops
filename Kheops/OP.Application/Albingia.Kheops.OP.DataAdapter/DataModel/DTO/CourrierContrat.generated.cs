using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class CourrierContrat {
        ///<summary>Public empty contructor</summary>
        public CourrierContrat() {}
        ///<summary>Public empty contructor</summary>
        public CourrierContrat(CourrierContrat copyFrom) 
        {
 
            this.Dacou= copyFrom.Dacou;
 
            this.Daipb= copyFrom.Daipb;
 
            this.Daalx= copyFrom.Daalx;
 
            this.Daavn= copyFrom.Daavn;
 
            this.Daasi= copyFrom.Daasi;
 
            this.Dansi= copyFrom.Dansi;
 
            this.Dassi= copyFrom.Dassi;
 
            this.Daarc= copyFrom.Daarc;
 
            this.Danrc= copyFrom.Danrc;
 
            this.Datbr= copyFrom.Datbr;
 
            this.Dasrv= copyFrom.Dasrv;
 
            this.Dattr= copyFrom.Dattr;
 
            this.Datlt= copyFrom.Datlt;
 
            this.Dalet= copyFrom.Dalet;
 
            this.Daver= copyFrom.Daver;
 
            this.Dafml= copyFrom.Dafml;
 
            this.Datds= copyFrom.Datds;
 
            this.Datyi= copyFrom.Datyi;
 
            this.Daids= copyFrom.Daids;
 
            this.Dainl= copyFrom.Dainl;
 
            this.Dasit= copyFrom.Dasit;
 
            this.Dasta= copyFrom.Dasta;
 
            this.Dastm= copyFrom.Dastm;
 
            this.Dastj= copyFrom.Dastj;
 
            this.Daspa= copyFrom.Daspa;
 
            this.Daspm= copyFrom.Daspm;
 
            this.Daspj= copyFrom.Daspj;
 
            this.Dalbc= copyFrom.Dalbc;
 
            this.Dator= copyFrom.Dator;
 
            this.Datev= copyFrom.Datev;
 
            this.Datae= copyFrom.Datae;
 
            this.Dancp= copyFrom.Dancp;
 
            this.Dasou= copyFrom.Dasou;
 
            this.Dages= copyFrom.Dages;
 
            this.Dabuc= copyFrom.Dabuc;
 
            this.Dabus= copyFrom.Dabus;
 
            this.Dacru= copyFrom.Dacru;
 
            this.Dacra= copyFrom.Dacra;
 
            this.Dacrm= copyFrom.Dacrm;
 
            this.Dacrj= copyFrom.Dacrj;
 
            this.Damju= copyFrom.Damju;
 
            this.Damja= copyFrom.Damja;
 
            this.Damjm= copyFrom.Damjm;
 
            this.Damjj= copyFrom.Damjj;
 
            this.Dales= copyFrom.Dales;
 
            this.Daenv= copyFrom.Daenv;
 
            this.Dacrh= copyFrom.Dacrh;
 
            this.Damjh= copyFrom.Damjh;
 
            this.Dacrp= copyFrom.Dacrp;
 
            this.Damjp= copyFrom.Damjp;
 
            this.Dalto= copyFrom.Dalto;
 
            this.Danur= copyFrom.Danur;
 
            this.Darfg= copyFrom.Darfg;
 
            this.Dain5= copyFrom.Dain5;
 
            this.Lgid4= copyFrom.Lgid4;
 
            this.Lgchd= copyFrom.Lgchd;
 
            this.Lgdoc= copyFrom.Lgdoc;
 
            this.Lgext= copyFrom.Lgext;
 
            this.Lgfpv= copyFrom.Lgfpv;
 
            this.Lgcru= copyFrom.Lgcru;
        
        }        



        ///<summary> DACOU N° courrier </summary>
        public int Dacou { get; set; } 
        ///<summary> N° Police </summary>
        public string Daipb { get; set; } 
        ///<summary> N° Aliment </summary>
        public int Daalx { get; set; } 
        ///<summary> N° Avenant </summary>
        public int Daavn { get; set; } 
        ///<summary> N° Sinistre : Année </summary>
        public int Daasi { get; set; } 
        ///<summary> N° Sinistre : N° </summary>
        public int Dansi { get; set; } 
        ///<summary> N° Sinistre : Sous-branche </summary>
        public string Dassi { get; set; } 
        ///<summary> N° Recours : Année </summary>
        public int Daarc { get; set; } 
        ///<summary> N° Recours : N° </summary>
        public int Danrc { get; set; } 
        ///<summary> CO ou RT </summary>
        public string Datbr { get; set; } 
        ///<summary> Type de service (Product;Sinist ...) </summary>
        public string Dasrv { get; set; } 
        ///<summary> Type de traitement (Affnouv/Avenant) </summary>
        public string Dattr { get; set; } 
        ///<summary> Type de courrier (T.texte ou Libre) </summary>
        public string Datlt { get; set; } 
        ///<summary> Code Lettre type ou document </summary>
        public string Dalet { get; set; } 
        ///<summary> DAVER Version lettre </summary>
        public int Daver { get; set; } 
        ///<summary> Famille Lettre type </summary>
        public string Dafml { get; set; } 
        ///<summary> Type de destinataire (Assuré;Cie...) </summary>
        public string Datds { get; set; } 
        ///<summary> Type intervenant     (Expert;Avoc..) </summary>
        public string Datyi { get; set; } 
        ///<summary> DAIDS Identifiant destinataire </summary>
        public int Daids { get; set; } 
        ///<summary> DAINL Interlocuteur </summary>
        public int Dainl { get; set; } 
        ///<summary> Situation du courrier </summary>
        public string Dasit { get; set; } 
        ///<summary> Année Situation </summary>
        public int Dasta { get; set; } 
        ///<summary> Mois  Situation </summary>
        public int Dastm { get; set; } 
        ///<summary> Jour  Situation </summary>
        public int Dastj { get; set; } 
        ///<summary> AA Courrier suspendu Jusqu'au </summary>
        public int Daspa { get; set; } 
        ///<summary> MM courrier suspendu jusqu'au </summary>
        public int Daspm { get; set; } 
        ///<summary> JJ courrier suspendu jusqu'au </summary>
        public int Daspj { get; set; } 
        ///<summary> Libellé du courrier </summary>
        public string Dalbc { get; set; } 
        ///<summary> Top de génération (P si pgm) </summary>
        public string Dator { get; set; } 
        ///<summary> Type d'envoi (Lettre/Fax/Ar .....) </summary>
        public string Datev { get; set; } 
        ///<summary> DATAE Type action enchaînée </summary>
        public string Datae { get; set; } 
        ///<summary> N° de courrier copié </summary>
        public int Dancp { get; set; } 
        ///<summary> Souscripteur </summary>
        public string Dasou { get; set; } 
        ///<summary> Gestionnaire </summary>
        public string Dages { get; set; } 
        ///<summary> Code Bureau du courtier </summary>
        public string Dabuc { get; set; } 
        ///<summary> Code Bureau du souscripteur </summary>
        public string Dabus { get; set; } 
        ///<summary> User  création </summary>
        public string Dacru { get; set; } 
        ///<summary> Année création </summary>
        public int Dacra { get; set; } 
        ///<summary> Mois  création </summary>
        public int Dacrm { get; set; } 
        ///<summary> Jour  création </summary>
        public int Dacrj { get; set; } 
        ///<summary> User  Màj </summary>
        public string Damju { get; set; } 
        ///<summary> Année Màj </summary>
        public int Damja { get; set; } 
        ///<summary> Mois  Màj </summary>
        public int Damjm { get; set; } 
        ///<summary> Jour  Màj </summary>
        public int Damjj { get; set; } 
        ///<summary> Destinataire Lésé O/N </summary>
        public string Dales { get; set; } 
        ///<summary> Environnement lettre </summary>
        public string Daenv { get; set; } 
        ///<summary> Heure création </summary>
        public int Dacrh { get; set; } 
        ///<summary> Heure màj </summary>
        public int Damjh { get; set; } 
        ///<summary> Programme création </summary>
        public string Dacrp { get; set; } 
        ///<summary> Programme màj </summary>
        public string Damjp { get; set; } 
        ///<summary> Lettre type origine </summary>
        public string Dalto { get; set; } 
        ///<summary> Relance prime N° </summary>
        public int Danur { get; set; } 
        ///<summary> Refus de Garantie O/N </summary>
        public string Darfg { get; set; } 
        ///<summary> Code interlocuteur sur 5 </summary>
        public int Dain5 { get; set; } 
        ///<summary> Id. doc. 4 - Version </summary>
        public int Lgid4 { get; set; } 
        ///<summary> Chemin (sans *SERVEUR) pour accès do </summary>
        public string Lgchd { get; set; } 
        ///<summary> Code document </summary>
        public string Lgdoc { get; set; } 
        ///<summary> Extention (DOT/DOC/JPG ...) </summary>
        public string Lgext { get; set; } 
        ///<summary> Fond de page - Version </summary>
        public int Lgfpv { get; set; } 
        ///<summary> User création </summary>
        public string Lgcru { get; set; } 
        }
}
