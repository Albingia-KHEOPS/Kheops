using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // View
    public  partial class DocLot {
        ///<summary>Public empty contructor</summary>
        public DocLot() {}
        ///<summary>Public empty contructor</summary>
        public DocLot(DocLot copyFrom) 
        {
 
            this.Kemid= copyFrom.Kemid;
 
            this.Kemkelid= copyFrom.Kemkelid;
 
            this.Kemord= copyFrom.Kemord;
 
            this.Kemtypd= copyFrom.Kemtypd;
 
            this.Kemtypl= copyFrom.Kemtypl;
 
            this.Kemtyenv= copyFrom.Kemtyenv;
 
            this.Kemtamp= copyFrom.Kemtamp;
 
            this.Kemtyds= copyFrom.Kemtyds;
 
            this.Kemtyi= copyFrom.Kemtyi;
 
            this.Kemids= copyFrom.Kemids;
 
            this.Kemdstp= copyFrom.Kemdstp;
 
            this.Keminl= copyFrom.Keminl;
 
            this.Kemnbex= copyFrom.Kemnbex;
 
            this.Kemdoca= copyFrom.Kemdoca;
 
            this.Kemtydif= copyFrom.Kemtydif;
 
            this.Kemlmai= copyFrom.Kemlmai;
 
            this.Kemaemo= copyFrom.Kemaemo;
 
            this.Kemaem= copyFrom.Kemaem;
 
            this.Kemkesid= copyFrom.Kemkesid;
 
            this.Kemnta= copyFrom.Kemnta;
 
            this.Kemstu= copyFrom.Kemstu;
 
            this.Kemsit= copyFrom.Kemsit;
 
            this.Kemstd= copyFrom.Kemstd;
 
            this.Kemsth= copyFrom.Kemsth;
 
            this.Kemenvu= copyFrom.Kemenvu;
 
            this.Kemenvd= copyFrom.Kemenvd;
 
            this.Kemenvh= copyFrom.Kemenvh;
 
            this.Keltyp= copyFrom.Keltyp;
 
            this.Kelipb= copyFrom.Kelipb;
 
            this.Kelalx= copyFrom.Kelalx;
 
            this.Kelactg= copyFrom.Kelactg;
 
            this.Kelactn= copyFrom.Kelactn;
 
            this.Kelavn= copyFrom.Kelavn;
 
            this.Kellib= copyFrom.Kellib;
 
            this.Kelemi= copyFrom.Kelemi;
 
            this.Kelipk= copyFrom.Kelipk;
 
            this.Keqid= copyFrom.Keqid;
 
            this.Keqtyp= copyFrom.Keqtyp;
 
            this.Keqipb= copyFrom.Keqipb;
 
            this.Keqalx= copyFrom.Keqalx;
 
            this.Keqsua= copyFrom.Keqsua;
 
            this.Keqnum= copyFrom.Keqnum;
 
            this.Keqsbr= copyFrom.Keqsbr;
 
            this.Keqserv= copyFrom.Keqserv;
 
            this.Keqactg= copyFrom.Keqactg;
 
            this.Keqactn= copyFrom.Keqactn;
 
            this.Keqeco= copyFrom.Keqeco;
 
            this.Keqavn= copyFrom.Keqavn;
 
            this.Keqetap= copyFrom.Keqetap;
 
            this.Keqkemid= copyFrom.Keqkemid;
 
            this.Keqord= copyFrom.Keqord;
 
            this.Keqtgl= copyFrom.Keqtgl;
 
            this.Keqtdoc= copyFrom.Keqtdoc;
 
            this.Keqkesid= copyFrom.Keqkesid;
 
            this.Keqajt= copyFrom.Keqajt;
 
            this.Keqtrs= copyFrom.Keqtrs;
 
            this.Keqmait= copyFrom.Keqmait;
 
            this.Keqlien= copyFrom.Keqlien;
 
            this.Keqcdoc= copyFrom.Keqcdoc;
 
            this.Keqver= copyFrom.Keqver;
 
            this.Keqlib= copyFrom.Keqlib;
 
            this.Keqnta= copyFrom.Keqnta;
 
            this.Keqdacc= copyFrom.Keqdacc;
 
            this.Keqtae= copyFrom.Keqtae;
 
            this.Keqnom= copyFrom.Keqnom;
 
            this.Keqchm= copyFrom.Keqchm;
 
            this.Keqstu= copyFrom.Keqstu;
 
            this.Keqsit= copyFrom.Keqsit;
 
            this.Keqstd= copyFrom.Keqstd;
 
            this.Keqsth= copyFrom.Keqsth;
 
            this.Keqenvu= copyFrom.Keqenvu;
 
            this.Keqenvd= copyFrom.Keqenvd;
 
            this.Keqenvh= copyFrom.Keqenvh;
 
            this.Keqtedi= copyFrom.Keqtedi;
 
            this.Keqorid= copyFrom.Keqorid;
 
            this.Keqtyds= copyFrom.Keqtyds;
 
            this.Keqtyi= copyFrom.Keqtyi;
 
            this.Keqids= copyFrom.Keqids;
 
            this.Keqinl= copyFrom.Keqinl;
 
            this.Keqnbex= copyFrom.Keqnbex;
 
            this.Keqcru= copyFrom.Keqcru;
 
            this.Keqcrd= copyFrom.Keqcrd;
 
            this.Keqcrh= copyFrom.Keqcrh;
 
            this.Keqmaju= copyFrom.Keqmaju;
 
            this.Keqmajd= copyFrom.Keqmajd;
 
            this.Keqmajh= copyFrom.Keqmajh;
 
            this.Keqstg= copyFrom.Keqstg;
 
            this.Keqdimp= copyFrom.Keqdimp;
        
        }        



        ///<summary> ID unique </summary>
        public Int64 Kemid { get; set; } 
        ///<summary> Lien KPDOCL </summary>
        public Int64 Kemkelid { get; set; } 
        ///<summary> N° ordre </summary>
        public int Kemord { get; set; } 
        ///<summary> Type Document Généré/Externe </summary>
        public string Kemtypd { get; set; } 
        ///<summary> Lien KPDOC ou KPDOCEXT </summary>
        public Int64 Kemtypl { get; set; } 
        ///<summary> Type envoi (AR,Recommandé ...) </summary>
        public string Kemtyenv { get; set; } 
        ///<summary> Tampon: Original Copie Duplicata </summary>
        public string Kemtamp { get; set; } 
        ///<summary> Type de destinataire </summary>
        public string Kemtyds { get; set; } 
        ///<summary> Type Intervenant </summary>
        public string Kemtyi { get; set; } 
        ///<summary> Identifiant Destinataire </summary>
        public int Kemids { get; set; } 
        ///<summary> Destinataire principal O/N </summary>
        public string Kemdstp { get; set; } 
        ///<summary> Code interlocuteur </summary>
        public int Keminl { get; set; } 
        ///<summary> Nombre exemplaires </summary>
        public int Kemnbex { get; set; } 
        ///<summary> Accompagnant lien Document   KDOC </summary>
        public Int64 Kemdoca { get; set; } 
        ///<summary> Type diffusion (Cour,Mail...) </summary>
        public string Kemtydif { get; set; } 
        ///<summary> Mail Lot </summary>
        public int Kemlmai { get; set; } 
        ///<summary> Mail Objet </summary>
        public string Kemaemo { get; set; } 
        ///<summary> Mail Adresse messagerie si dif </summary>
        public string Kemaem { get; set; } 
        ///<summary> Mail Lien KPDOCTX Texte  Corps mail </summary>
        public Int64 Kemkesid { get; set; } 
        ///<summary> Nature de la génération (O/P/S) </summary>
        public string Kemnta { get; set; } 
        ///<summary> Situation User </summary>
        public string Kemstu { get; set; } 
        ///<summary> Situation Code </summary>
        public string Kemsit { get; set; } 
        ///<summary> Situation Date </summary>
        public int Kemstd { get; set; } 
        ///<summary> Situation Heure </summary>
        public int Kemsth { get; set; } 
        ///<summary> Envoi User </summary>
        public string Kemenvu { get; set; } 
        ///<summary> Envoi Date </summary>
        public int Kemenvd { get; set; } 
        ///<summary> Envoi Heure </summary>
        public int Kemenvh { get; set; } 
        ///<summary> Type O/P </summary>
        public string Keltyp { get; set; } 
        ///<summary> IPB </summary>
        public string Kelipb { get; set; } 
        ///<summary> ALX </summary>
        public int Kelalx { get; set; } 
        ///<summary> Acte de gestion </summary>
        public string Kelactg { get; set; } 
        ///<summary> N° acte de gestion </summary>
        public int Kelactn { get; set; } 
        ///<summary> N° Avenant </summary>
        public int Kelavn { get; set; } 
        ///<summary> Libellé </summary>
        public string Kellib { get; set; } 
        ///<summary> Type prime Comptant/X sans prime/Règ </summary>
        public string Kelemi { get; set; } 
        ///<summary> N° de prime si emission </summary>
        public int Kelipk { get; set; } 
        ///<summary> ID unique </summary>
        public Int64 Keqid { get; set; } 
        ///<summary> Type O/P </summary>
        public string Keqtyp { get; set; } 
        ///<summary> IPB </summary>
        public string Keqipb { get; set; } 
        ///<summary> ALX </summary>
        public int Keqalx { get; set; } 
        ///<summary> Sinistre AA </summary>
        public int Keqsua { get; set; } 
        ///<summary> Sinistre N° </summary>
        public int Keqnum { get; set; } 
        ///<summary> Sinistre Sbr </summary>
        public string Keqsbr { get; set; } 
        ///<summary> Service  (Produ,Sinistre...) </summary>
        public string Keqserv { get; set; } 
        ///<summary> Acte de gestion </summary>
        public string Keqactg { get; set; } 
        ///<summary> N° acte de gestion </summary>
        public int Keqactn { get; set; } 
        ///<summary> En cours O/N </summary>
        public string Keqeco { get; set; } 
        ///<summary> N° Avenant </summary>
        public int Keqavn { get; set; } 
        ///<summary> Etape </summary>
        public string Keqetap { get; set; } 
        ///<summary> Lien KALCELD </summary>
        public Int64 Keqkemid { get; set; } 
        ///<summary> N° Ordre </summary>
        public int Keqord { get; set; } 
        ///<summary> Type de Génération Généré ou Libre </summary>
        public string Keqtgl { get; set; } 
        ///<summary> Type Document LETTYP LIBRE CP ..... </summary>
        public string Keqtdoc { get; set; } 
        ///<summary> Si Complément   Lien KPDOCTX Texte </summary>
        public Int64 Keqkesid { get; set; } 
        ///<summary> Ajouté O/N </summary>
        public string Keqajt { get; set; } 
        ///<summary> Transformé O/N </summary>
        public string Keqtrs { get; set; } 
        ///<summary> Table Maître ou Origine (KCLAUSE... </summary>
        public string Keqmait { get; set; } 
        ///<summary> Lien table maître/Origine (KCLAUSE.. </summary>
        public Int64 Keqlien { get; set; } 
        ///<summary> Code document (si lettyp (3 x 5)) </summary>
        public string Keqcdoc { get; set; } 
        ///<summary> N°Version </summary>
        public int Keqver { get; set; } 
        ///<summary> Libellé </summary>
        public string Keqlib { get; set; } 
        ///<summary> Nature de la génération (O/P/S) </summary>
        public string Keqnta { get; set; } 
        ///<summary> Document Accompagnant O/N </summary>
        public string Keqdacc { get; set; } 
        ///<summary> Action enchainée </summary>
        public string Keqtae { get; set; } 
        ///<summary> Nom du document </summary>
        public string Keqnom { get; set; } 
        ///<summary> Chemin complet </summary>
        public string Keqchm { get; set; } 
        ///<summary> Situation User </summary>
        public string Keqstu { get; set; } 
        ///<summary> Situation Code </summary>
        public string Keqsit { get; set; } 
        ///<summary> Situation Date </summary>
        public int Keqstd { get; set; } 
        ///<summary> Situation Heure </summary>
        public int Keqsth { get; set; } 
        ///<summary> Envoi User </summary>
        public string Keqenvu { get; set; } 
        ///<summary> Envoi Date </summary>
        public int Keqenvd { get; set; } 
        ///<summary> Envoi Heure </summary>
        public int Keqenvh { get; set; } 
        ///<summary> Typo Edit Originale,Copie,Duplicata </summary>
        public string Keqtedi { get; set; } 
        ///<summary> Lien KPDOC original si Copie/Dup </summary>
        public Int64 Keqorid { get; set; } 
        ///<summary> Type de destinataire sur document </summary>
        public string Keqtyds { get; set; } 
        ///<summary> Type intervenant sur document </summary>
        public string Keqtyi { get; set; } 
        ///<summary> Id destinataire sur document </summary>
        public int Keqids { get; set; } 
        ///<summary> Code interlocuteur sur document </summary>
        public int Keqinl { get; set; } 
        ///<summary> Nombre d'exemplaires supplémentaires </summary>
        public int Keqnbex { get; set; } 
        ///<summary> Création User </summary>
        public string Keqcru { get; set; } 
        ///<summary> Création Date </summary>
        public int Keqcrd { get; set; } 
        ///<summary> Création Heure </summary>
        public int Keqcrh { get; set; } 
        ///<summary> Maj User </summary>
        public string Keqmaju { get; set; } 
        ///<summary> Maj Date </summary>
        public int Keqmajd { get; set; } 
        ///<summary> Maj Heure </summary>
        public int Keqmajh { get; set; } 
        ///<summary> Statut génération G à jour/ Modif ap </summary>
        public string Keqstg { get; set; } 
        ///<summary> Imprimable  O/N </summary>
        public string Keqdimp { get; set; } 
        }
}
