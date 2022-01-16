using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class Courtier {
        ///<summary>Public empty contructor</summary>
        public Courtier() {}
        ///<summary>Public empty contructor</summary>
        public Courtier(Courtier copyFrom) 
        {
 
            this.Tcict= copyFrom.Tcict;
 
            this.Tctyp= copyFrom.Tctyp;
 
            this.Tcici= copyFrom.Tcici;
 
            this.Tcad1= copyFrom.Tcad1;
 
            this.Tcad2= copyFrom.Tcad2;
 
            this.Tcdep= copyFrom.Tcdep;
 
            this.Tccpo= copyFrom.Tccpo;
 
            this.Tcvil= copyFrom.Tcvil;
 
            this.Tcpay= copyFrom.Tcpay;
 
            this.Tccom= copyFrom.Tccom;
 
            this.Tcreg= copyFrom.Tcreg;
 
            this.Tcbur= copyFrom.Tcbur;
 
            this.Tcfdc= copyFrom.Tcfdc;
 
            this.Tctel= copyFrom.Tctel;
 
            this.Tctlc= copyFrom.Tctlc;
 
            this.Tcbqe= copyFrom.Tcbqe;
 
            this.Tcgui= copyFrom.Tcgui;
 
            this.Tccpt= copyFrom.Tccpt;
 
            this.Tcrib= copyFrom.Tcrib;
 
            this.Tcicp= copyFrom.Tcicp;
 
            this.Tcori= copyFrom.Tcori;
 
            this.Tctin= copyFrom.Tctin;
 
            this.Tccii= copyFrom.Tccii;
 
            this.Tcapg= copyFrom.Tcapg;
 
            this.Tclig= copyFrom.Tclig;
 
            this.Tcrgc= copyFrom.Tcrgc;
 
            this.Tcenc= copyFrom.Tcenc;
 
            this.Tcman= copyFrom.Tcman;
 
            this.Tcdcp= copyFrom.Tcdcp;
 
            this.Tcraf= copyFrom.Tcraf;
 
            this.Tccha= copyFrom.Tccha;
 
            this.Tcgep= copyFrom.Tcgep;
 
            this.Tcprd= copyFrom.Tcprd;
 
            this.Tccra= copyFrom.Tccra;
 
            this.Tccrm= copyFrom.Tccrm;
 
            this.Tccrj= copyFrom.Tccrj;
 
            this.Tcfva= copyFrom.Tcfva;
 
            this.Tcfvm= copyFrom.Tcfvm;
 
            this.Tcfvj= copyFrom.Tcfvj;
 
            this.Tcrpl= copyFrom.Tcrpl;
 
            this.Tcusr= copyFrom.Tcusr;
 
            this.Tcmja= copyFrom.Tcmja;
 
            this.Tcmjm= copyFrom.Tcmjm;
 
            this.Tcmjj= copyFrom.Tcmjj;
 
            this.Tcold= copyFrom.Tcold;
 
            this.Tcape= copyFrom.Tcape;
 
            this.Tcins= copyFrom.Tcins;
 
            this.Tcspe= copyFrom.Tcspe;
 
            this.Tcyen= copyFrom.Tcyen;
 
            this.Tcaem= copyFrom.Tcaem;
 
            this.Tcioo= copyFrom.Tcioo;
 
            this.Tcioc= copyFrom.Tcioc;
 
            this.Tcioj= copyFrom.Tcioj;
 
            this.Tciom= copyFrom.Tciom;
 
            this.Tcioa= copyFrom.Tcioa;
 
            this.Tciag= copyFrom.Tciag;
 
            this.Tciaj= copyFrom.Tciaj;
 
            this.Tciam= copyFrom.Tciam;
 
            this.Tciaa= copyFrom.Tciaa;
 
            this.Tciii= copyFrom.Tciii;
 
            this.Tciij= copyFrom.Tciij;
 
            this.Tciim= copyFrom.Tciim;
 
            this.Tciia= copyFrom.Tciia;
 
            this.Tcimd= copyFrom.Tcimd;
 
            this.Tcimj= copyFrom.Tcimj;
 
            this.Tcimm= copyFrom.Tcimm;
 
            this.Tcima= copyFrom.Tcima;
 
            this.Tcadh= copyFrom.Tcadh;
 
            this.Tcrcv= copyFrom.Tcrcv;
 
            this.Tcrcs= copyFrom.Tcrcs;
 
            this.Tcinm= copyFrom.Tcinm;
 
            this.Tcsec= copyFrom.Tcsec;
 
            this.Tcirn= copyFrom.Tcirn;
 
            this.Tcirj= copyFrom.Tcirj;
 
            this.Tcirm= copyFrom.Tcirm;
 
            this.Tcira= copyFrom.Tcira;
 
            this.Tcrcn= copyFrom.Tcrcn;
 
            this.Tcap5= copyFrom.Tcap5;
 
            this.Tcedi= copyFrom.Tcedi;
 
            this.Tcidn= copyFrom.Tcidn;
 
            this.Tcist= copyFrom.Tcist;
 
            this.Tcgem= copyFrom.Tcgem;
 
            this.Tnict= copyFrom.Tnict;
 
            this.Tninl= copyFrom.Tninl;
 
            this.Tntnm= copyFrom.Tntnm;
 
            this.Tnord= copyFrom.Tnord;
 
            this.Tntyp= copyFrom.Tntyp;
 
            this.Tnnom= copyFrom.Tnnom;
 
            this.Tntit= copyFrom.Tntit;
 
            this.Tnxn5= copyFrom.Tnxn5;
 
            this.Abpcp6= copyFrom.Abpcp6;
 
            this.Abpdp6= copyFrom.Abpdp6;
 
            this.Abpvi6= copyFrom.Abpvi6;
 
            this.Abppay= copyFrom.Abppay;
 
            this.Abhsec= copyFrom.Abhsec;
 
            this.Abhdes= copyFrom.Abhdes;
 
            this.Abhlib= copyFrom.Abhlib;
 
            this.Abhord= copyFrom.Abhord;
 
            this.Abhins= copyFrom.Abhins;
 
            this.Aclins= copyFrom.Aclins;
 
            this.Acldes= copyFrom.Acldes;
 
            this.Acllib= copyFrom.Acllib;
 
            this.Aclord= copyFrom.Aclord;
 
            this.Acluin= copyFrom.Acluin;
        
        }        



        ///<summary> Identifiant courtier </summary>
        public int Tcict { get; set; } 
        ///<summary> Type de courtier : Prospect/Courtier </summary>
        public string Tctyp { get; set; } 
        ///<summary> Identifiant Compagnie (si Cie) </summary>
        public string Tcici { get; set; } 
        ///<summary> Adresse 1 </summary>
        public string Tcad1 { get; set; } 
        ///<summary> Adresse 2 </summary>
        public string Tcad2 { get; set; } 
        ///<summary> Département </summary>
        public string Tcdep { get; set; } 
        ///<summary> Code postal 3 caractères </summary>
        public string Tccpo { get; set; } 
        ///<summary> Ville </summary>
        public string Tcvil { get; set; } 
        ///<summary> Code pays </summary>
        public string Tcpay { get; set; } 
        ///<summary> Code commune Arrondissement </summary>
        public string Tccom { get; set; } 
        ///<summary> Code région </summary>
        public string Tcreg { get; set; } 
        ///<summary> Code Bureau </summary>
        public string Tcbur { get; set; } 
        ///<summary> Code FRANCE DOC   Transit courrier </summary>
        public string Tcfdc { get; set; } 
        ///<summary> Téléphone </summary>
        public string Tctel { get; set; } 
        ///<summary> Télécopie </summary>
        public string Tctlc { get; set; } 
        ///<summary> Code banque </summary>
        public int Tcbqe { get; set; } 
        ///<summary> Code guichet </summary>
        public int Tcgui { get; set; } 
        ///<summary> Compte </summary>
        public string Tccpt { get; set; } 
        ///<summary> Clé RIB </summary>
        public int Tcrib { get; set; } 
        ///<summary> Intitulé du compte </summary>
        public string Tcicp { get; set; } 
        ///<summary> Origine du prospect/courtier </summary>
        public string Tcori { get; set; } 
        ///<summary> Type intermédiaire </summary>
        public string Tctin { get; set; } 
        ///<summary> Compagnie principale Intermédiaire </summary>
        public string Tccii { get; set; } 
        ///<summary> Appartenance à un groupe </summary>
        public string Tcapg { get; set; } 
        ///<summary> Lien avec le groupe </summary>
        public string Tclig { get; set; } 
        ///<summary> Régime de commissionnement </summary>
        public string Tcrgc { get; set; } 
        ///<summary> Courtier ALbingia  O/N (Enc Direct) </summary>
        public string Tcenc { get; set; } 
        ///<summary> Code Mandat </summary>
        public string Tcman { get; set; } 
        ///<summary> Décalage paiement </summary>
        public int Tcdcp { get; set; } 
        ///<summary> Rattachement fiscal (code Courtier) </summary>
        public int Tcraf { get; set; } 
        ///<summary> Non utilisé </summary>
        public string Tccha { get; set; } 
        ///<summary> Gestion de prospection (Interdit ..) </summary>
        public string Tcgep { get; set; } 
        ///<summary> Producteur du Courtier/prospect </summary>
        public string Tcprd { get; set; } 
        ///<summary> Année de création </summary>
        public int Tccra { get; set; } 
        ///<summary> Mois de création </summary>
        public int Tccrm { get; set; } 
        ///<summary> Jour de création </summary>
        public int Tccrj { get; set; } 
        ///<summary> Année fin de validité si transfert </summary>
        public int Tcfva { get; set; } 
        ///<summary> Mois fin de validité si transfert </summary>
        public int Tcfvm { get; set; } 
        ///<summary> Jour fin de validité si transfert </summary>
        public int Tcfvj { get; set; } 
        ///<summary> Courtier de remplacement Transfert </summary>
        public int Tcrpl { get; set; } 
        ///<summary> User  Màj </summary>
        public string Tcusr { get; set; } 
        ///<summary> Année Màj </summary>
        public int Tcmja { get; set; } 
        ///<summary> Mois Màj </summary>
        public int Tcmjm { get; set; } 
        ///<summary> Jour Màj </summary>
        public int Tcmjj { get; set; } 
        ///<summary> Ancien code courtier pour corresp 36 </summary>
        public string Tcold { get; set; } 
        ///<summary> NON UTILISE  Ancien code APE </summary>
        public string Tcape { get; set; } 
        ///<summary> Code INSEE </summary>
        public string Tcins { get; set; } 
        ///<summary> Spécificité BOR Bordereau ..... </summary>
        public string Tcspe { get; set; } 
        ///<summary> Type encaissement par défaut D/A/C </summary>
        public string Tcyen { get; set; } 
        ///<summary> Adresse de messagerie (Mail) </summary>
        public string Tcaem { get; set; } 
        ///<summary> ORIAS Immatriculation </summary>
        public Int64 Tcioo { get; set; } 
        ///<summary> ORIAS Courtier O/N </summary>
        public string Tcioc { get; set; } 
        ///<summary> ORIAS Courtier JJ </summary>
        public int Tcioj { get; set; } 
        ///<summary> ORIAS Courtier MM </summary>
        public int Tciom { get; set; } 
        ///<summary> ORIAS Courtier AA </summary>
        public int Tcioa { get; set; } 
        ///<summary> ORIAS Agent O/N </summary>
        public string Tciag { get; set; } 
        ///<summary> ORIAS Agent JJ </summary>
        public int Tciaj { get; set; } 
        ///<summary> ORIAS Agent MM </summary>
        public int Tciam { get; set; } 
        ///<summary> ORIAS Agent AA </summary>
        public int Tciaa { get; set; } 
        ///<summary> ORIAS Intermédiaires O/N </summary>
        public string Tciii { get; set; } 
        ///<summary> ORIAS Intermédiaire JJ </summary>
        public int Tciij { get; set; } 
        ///<summary> ORIAS intermédiaire MM </summary>
        public int Tciim { get; set; } 
        ///<summary> ORIAS Intermédiaire AA </summary>
        public int Tciia { get; set; } 
        ///<summary> Mandat Alb O/N </summary>
        public string Tcimd { get; set; } 
        ///<summary> Mandat Alb Jour </summary>
        public int Tcimj { get; set; } 
        ///<summary> Mandat Alb Mois </summary>
        public int Tcimm { get; set; } 
        ///<summary> Mandat Alb Année </summary>
        public int Tcima { get; set; } 
        ///<summary> Lien adresse chrono </summary>
        public int Tcadh { get; set; } 
        ///<summary> RCS Ville </summary>
        public string Tcrcv { get; set; } 
        ///<summary> RCS N°Siren </summary>
        public string Tcrcs { get; set; } 
        ///<summary> Gest°intermédiation </summary>
        public string Tcinm { get; set; } 
        ///<summary> Code Secteur </summary>
        public string Tcsec { get; set; } 
        ///<summary> Renouvellement O/N </summary>
        public string Tcirn { get; set; } 
        ///<summary> ORIAS Renouvellement Jour </summary>
        public int Tcirj { get; set; } 
        ///<summary> ORIAS renouvellement Mois </summary>
        public int Tcirm { get; set; } 
        ///<summary> ORIAS Renouvellement Année </summary>
        public int Tcira { get; set; } 
        ///<summary> NIC SIRET </summary>
        public int Tcrcn { get; set; } 
        ///<summary> Code APE </summary>
        public string Tcap5 { get; set; } 
        ///<summary> Code EDI du courtier </summary>
        public string Tcedi { get; set; } 
        ///<summary> Notre id chez courtier </summary>
        public string Tcidn { get; set; } 
        ///<summary> Code Orias Situation </summary>
        public string Tcist { get; set; } 
        ///<summary> Prospection Motif </summary>
        public string Tcgem { get; set; } 
        ///<summary> Identifiant courtier </summary>
        public int Tnict { get; set; } 
        ///<summary> Code interlocuteur Courtier </summary>
        public int Tninl { get; set; } 
        ///<summary> Type de nom </summary>
        public string Tntnm { get; set; } 
        ///<summary> N° ordre </summary>
        public int Tnord { get; set; } 
        ///<summary> Type Prospect/Courtier </summary>
        public string Tntyp { get; set; } 
        ///<summary> Nom </summary>
        public string Tnnom { get; set; } 
        ///<summary> Titre </summary>
        public string Tntit { get; set; } 
        ///<summary> Code interlocuteur sur 5 </summary>
        public int Tnxn5 { get; set; } 
        ///<summary> Code postal  Ligne 6 </summary>
        public int Abpcp6 { get; set; } 
        ///<summary> Département ligne 6 </summary>
        public string Abpdp6 { get; set; } 
        ///<summary> Libellé acheminement Ligne 6 </summary>
        public string Abpvi6 { get; set; } 
        ///<summary> Code pays </summary>
        public string Abppay { get; set; } 
        ///<summary> Code Secteur </summary>
        public string Abhsec { get; set; } 
        ///<summary> Désignation </summary>
        public string Abhdes { get; set; } 
        ///<summary> Désignation abrégée </summary>
        public string Abhlib { get; set; } 
        ///<summary> N° Ordre </summary>
        public int Abhord { get; set; } 
        ///<summary> Code inspection </summary>
        public string Abhins { get; set; } 
        ///<summary> Code inspection </summary>
        public string Aclins { get; set; } 
        ///<summary> Désignation </summary>
        public string Acldes { get; set; } 
        ///<summary> Désignation abrégée </summary>
        public string Acllib { get; set; } 
        ///<summary> N° Ordre </summary>
        public int Aclord { get; set; } 
        ///<summary> User Inspecteur </summary>
        public string Acluin { get; set; } 
        }
}
