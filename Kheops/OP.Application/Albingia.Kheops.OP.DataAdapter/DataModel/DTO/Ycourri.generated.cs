using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YCOURRI
    public partial class Ycourri  {
             //YCOURRI

            ///<summary>Public empty contructor</summary>
            public Ycourri() {}
            ///<summary>Public empty contructor</summary>
            public Ycourri(Ycourri copyFrom) 
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
  


        public override bool Equals(object  obj)
        {
            Ycourri  x=this,  y=obj as Ycourri;
            if( y == default(Ycourri) ) return false;
            return (
                    x.Dacou==y.Dacou
                    && x.Daipb==y.Daipb
                    && x.Daalx==y.Daalx
                    && x.Daavn==y.Daavn
                    && x.Daasi==y.Daasi
                    && x.Dansi==y.Dansi
                    && x.Dassi==y.Dassi
                    && x.Daarc==y.Daarc
                    && x.Danrc==y.Danrc
                    && x.Datbr==y.Datbr
                    && x.Dasrv==y.Dasrv
                    && x.Dattr==y.Dattr
                    && x.Datlt==y.Datlt
                    && x.Dalet==y.Dalet
                    && x.Daver==y.Daver
                    && x.Dafml==y.Dafml
                    && x.Datds==y.Datds
                    && x.Datyi==y.Datyi
                    && x.Daids==y.Daids
                    && x.Dainl==y.Dainl
                    && x.Dasit==y.Dasit
                    && x.Dasta==y.Dasta
                    && x.Dastm==y.Dastm
                    && x.Dastj==y.Dastj
                    && x.Daspa==y.Daspa
                    && x.Daspm==y.Daspm
                    && x.Daspj==y.Daspj
                    && x.Dalbc==y.Dalbc
                    && x.Dator==y.Dator
                    && x.Datev==y.Datev
                    && x.Datae==y.Datae
                    && x.Dancp==y.Dancp
                    && x.Dasou==y.Dasou
                    && x.Dages==y.Dages
                    && x.Dabuc==y.Dabuc
                    && x.Dabus==y.Dabus
                    && x.Dacru==y.Dacru
                    && x.Dacra==y.Dacra
                    && x.Dacrm==y.Dacrm
                    && x.Dacrj==y.Dacrj
                    && x.Damju==y.Damju
                    && x.Damja==y.Damja
                    && x.Damjm==y.Damjm
                    && x.Damjj==y.Damjj
                    && x.Dales==y.Dales
                    && x.Daenv==y.Daenv
                    && x.Dacrh==y.Dacrh
                    && x.Damjh==y.Damjh
                    && x.Dacrp==y.Dacrp
                    && x.Damjp==y.Damjp
                    && x.Dalto==y.Dalto
                    && x.Danur==y.Danur
                    && x.Darfg==y.Darfg
                    && x.Dain5==y.Dain5  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Dacou.GetHashCode() ) 
                      * 23 ) + (this.Daipb?? "").GetHashCode()
                      * 23 ) + (this.Daalx.GetHashCode() ) 
                      * 23 ) + (this.Daavn.GetHashCode() ) 
                      * 23 ) + (this.Daasi.GetHashCode() ) 
                      * 23 ) + (this.Dansi.GetHashCode() ) 
                      * 23 ) + (this.Dassi?? "").GetHashCode()
                      * 23 ) + (this.Daarc.GetHashCode() ) 
                      * 23 ) + (this.Danrc.GetHashCode() ) 
                      * 23 ) + (this.Datbr?? "").GetHashCode()
                      * 23 ) + (this.Dasrv?? "").GetHashCode()
                      * 23 ) + (this.Dattr?? "").GetHashCode()
                      * 23 ) + (this.Datlt?? "").GetHashCode()
                      * 23 ) + (this.Dalet?? "").GetHashCode()
                      * 23 ) + (this.Daver.GetHashCode() ) 
                      * 23 ) + (this.Dafml?? "").GetHashCode()
                      * 23 ) + (this.Datds?? "").GetHashCode()
                      * 23 ) + (this.Datyi?? "").GetHashCode()
                      * 23 ) + (this.Daids.GetHashCode() ) 
                      * 23 ) + (this.Dainl.GetHashCode() ) 
                      * 23 ) + (this.Dasit?? "").GetHashCode()
                      * 23 ) + (this.Dasta.GetHashCode() ) 
                      * 23 ) + (this.Dastm.GetHashCode() ) 
                      * 23 ) + (this.Dastj.GetHashCode() ) 
                      * 23 ) + (this.Daspa.GetHashCode() ) 
                      * 23 ) + (this.Daspm.GetHashCode() ) 
                      * 23 ) + (this.Daspj.GetHashCode() ) 
                      * 23 ) + (this.Dalbc?? "").GetHashCode()
                      * 23 ) + (this.Dator?? "").GetHashCode()
                      * 23 ) + (this.Datev?? "").GetHashCode()
                      * 23 ) + (this.Datae?? "").GetHashCode()
                      * 23 ) + (this.Dancp.GetHashCode() ) 
                      * 23 ) + (this.Dasou?? "").GetHashCode()
                      * 23 ) + (this.Dages?? "").GetHashCode()
                      * 23 ) + (this.Dabuc?? "").GetHashCode()
                      * 23 ) + (this.Dabus?? "").GetHashCode()
                      * 23 ) + (this.Dacru?? "").GetHashCode()
                      * 23 ) + (this.Dacra.GetHashCode() ) 
                      * 23 ) + (this.Dacrm.GetHashCode() ) 
                      * 23 ) + (this.Dacrj.GetHashCode() ) 
                      * 23 ) + (this.Damju?? "").GetHashCode()
                      * 23 ) + (this.Damja.GetHashCode() ) 
                      * 23 ) + (this.Damjm.GetHashCode() ) 
                      * 23 ) + (this.Damjj.GetHashCode() ) 
                      * 23 ) + (this.Dales?? "").GetHashCode()
                      * 23 ) + (this.Daenv?? "").GetHashCode()
                      * 23 ) + (this.Dacrh.GetHashCode() ) 
                      * 23 ) + (this.Damjh.GetHashCode() ) 
                      * 23 ) + (this.Dacrp?? "").GetHashCode()
                      * 23 ) + (this.Damjp?? "").GetHashCode()
                      * 23 ) + (this.Dalto?? "").GetHashCode()
                      * 23 ) + (this.Danur.GetHashCode() ) 
                      * 23 ) + (this.Darfg?? "").GetHashCode()
                      * 23 ) + (this.Dain5.GetHashCode() )                    );
           }
        }
    }
}
