using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YSINCOU
    public partial class Ysincou  {
             //YSINCOU

            ///<summary>Public empty contructor</summary>
            public Ysincou() {}
            ///<summary>Public empty contructor</summary>
            public Ysincou(Ysincou copyFrom) 
            {
                  this.Shsua= copyFrom.Shsua;
                  this.Shnum= copyFrom.Shnum;
                  this.Shsbr= copyFrom.Shsbr;
                  this.Shnuc= copyFrom.Shnuc;
                  this.Shtbr= copyFrom.Shtbr;
                  this.Shsrv= copyFrom.Shsrv;
                  this.Shttr= copyFrom.Shttr;
                  this.Shelg= copyFrom.Shelg;
                  this.Shcds= copyFrom.Shcds;
                  this.Shlet= copyFrom.Shlet;
                  this.Shnae= copyFrom.Shnae;
                  this.Shtev= copyFrom.Shtev;
                  this.Shtae= copyFrom.Shtae;
                  this.Shaff= copyFrom.Shaff;
                  this.Shlbc= copyFrom.Shlbc;
                  this.Shtlt= copyFrom.Shtlt;
                  this.Shtds= copyFrom.Shtds;
                  this.Shtyi= copyFrom.Shtyi;
                  this.Shids= copyFrom.Shids;
                  this.Shinl= copyFrom.Shinl;
                  this.Shsit= copyFrom.Shsit;
                  this.Shsta= copyFrom.Shsta;
                  this.Shstm= copyFrom.Shstm;
                  this.Shstj= copyFrom.Shstj;
                  this.Shspa= copyFrom.Shspa;
                  this.Shspm= copyFrom.Shspm;
                  this.Shspj= copyFrom.Shspj;
                  this.Shncp= copyFrom.Shncp;
                  this.Shajt= copyFrom.Shajt;
                  this.Shtvl= copyFrom.Shtvl;
                  this.Shcru= copyFrom.Shcru;
                  this.Shcra= copyFrom.Shcra;
                  this.Shcrm= copyFrom.Shcrm;
                  this.Shcrj= copyFrom.Shcrj;
                  this.Shmju= copyFrom.Shmju;
                  this.Shmja= copyFrom.Shmja;
                  this.Shmjm= copyFrom.Shmjm;
                  this.Shmjj= copyFrom.Shmjj;
                  this.Shles= copyFrom.Shles;
                  this.Shenv= copyFrom.Shenv;
                  this.Shchr= copyFrom.Shchr;
                  this.Shrcd= copyFrom.Shrcd;
                  this.Shrcc= copyFrom.Shrcc;
                  this.Shcou= copyFrom.Shcou;
                  this.Shcdo= copyFrom.Shcdo;
                  this.Shact= copyFrom.Shact;
                  this.Shtrf= copyFrom.Shtrf;
                  this.Shcop= copyFrom.Shcop;
                  this.Shrfg= copyFrom.Shrfg;
                  this.Shin5= copyFrom.Shin5;
        
            }        
            
            ///<summary> N° de sinistre : Année </summary>
            public int Shsua { get; set; } 
            
            ///<summary> N° Sin. Numéro </summary>
            public int Shnum { get; set; } 
            
            ///<summary> N° de sinistre : Sous-branche </summary>
            public string Shsbr { get; set; } 
            
            ///<summary> N° du Courrier attente </summary>
            public int Shnuc { get; set; } 
            
            ///<summary> Branche </summary>
            public string Shtbr { get; set; } 
            
            ///<summary> GEN: Type service (Prod.,sinist...) </summary>
            public string Shsrv { get; set; } 
            
            ///<summary> GEN: Type traitement Affnouv/avenant </summary>
            public string Shttr { get; set; } 
            
            ///<summary> GEN: Elément générateur </summary>
            public string Shelg { get; set; } 
            
            ///<summary> GEN: Codif destinataire </summary>
            public string Shcds { get; set; } 
            
            ///<summary> GEN: Code lettre type ou doc.Office </summary>
            public string Shlet { get; set; } 
            
            ///<summary> GEN: Nature générat° Obli/Propos/Sug </summary>
            public string Shnae { get; set; } 
            
            ///<summary> GEN: Type envoi (Lettre/Fax/Ar ...) </summary>
            public string Shtev { get; set; } 
            
            ///<summary> GEN: Type action enchaînée </summary>
            public string Shtae { get; set; } 
            
            ///<summary> GEN: Affichage à l'écran O/N </summary>
            public string Shaff { get; set; } 
            
            ///<summary> GEN: Libellé du courrier </summary>
            public string Shlbc { get; set; } 
            
            ///<summary> Type de courrier (T.texte ou Libre) </summary>
            public string Shtlt { get; set; } 
            
            ///<summary> Type de Destinataire </summary>
            public string Shtds { get; set; } 
            
            ///<summary> Type intervenant (Expert Avoc. Hui.. </summary>
            public string Shtyi { get; set; } 
            
            ///<summary> Identifiant destinataire </summary>
            public int Shids { get; set; } 
            
            ///<summary> Code Interloc. </summary>
            public int Shinl { get; set; } 
            
            ///<summary> Situation du courrier </summary>
            public string Shsit { get; set; } 
            
            ///<summary> Année Situation </summary>
            public int Shsta { get; set; } 
            
            ///<summary> Mois  Situation </summary>
            public int Shstm { get; set; } 
            
            ///<summary> Jour  Situation </summary>
            public int Shstj { get; set; } 
            
            ///<summary> AA Courrier suspendu Jusqu'au </summary>
            public int Shspa { get; set; } 
            
            ///<summary> MM courrier suspendu jusqu'au </summary>
            public int Shspm { get; set; } 
            
            ///<summary> JJ courrier suspendu jusqu'au </summary>
            public int Shspj { get; set; } 
            
            ///<summary> N° du courrier copié </summary>
            public int Shncp { get; set; } 
            
            ///<summary> Courrier ajouté O/N </summary>
            public string Shajt { get; set; } 
            
            ///<summary> Top validation ' ' / 'V' </summary>
            public string Shtvl { get; set; } 
            
            ///<summary> User  création </summary>
            public string Shcru { get; set; } 
            
            ///<summary> Année création </summary>
            public int Shcra { get; set; } 
            
            ///<summary> Mois  création </summary>
            public int Shcrm { get; set; } 
            
            ///<summary> Jour  création </summary>
            public int Shcrj { get; set; } 
            
            ///<summary> User  Màj </summary>
            public string Shmju { get; set; } 
            
            ///<summary> Année Màj </summary>
            public int Shmja { get; set; } 
            
            ///<summary> Mois  Màj </summary>
            public int Shmjm { get; set; } 
            
            ///<summary> Jour  Màj </summary>
            public int Shmjj { get; set; } 
            
            ///<summary> Destinat. Lésé O/N </summary>
            public string Shles { get; set; } 
            
            ///<summary> Environnement lettre </summary>
            public string Shenv { get; set; } 
            
            ///<summary> Règlement: Numéro chrono </summary>
            public int Shchr { get; set; } 
            
            ///<summary> RCD: Génération par trt alerte ass. </summary>
            public string Shrcd { get; set; } 
            
            ///<summary> RCD: Numéro chrono </summary>
            public int Shrcc { get; set; } 
            
            ///<summary> N° de Courrier Définitif </summary>
            public int Shcou { get; set; } 
            
            ///<summary> Copie dossier O/N </summary>
            public string Shcdo { get; set; } 
            
            ///<summary> Action (ex: ouvrir/supprimer/gérer) </summary>
            public string Shact { get; set; } 
            
            ///<summary> Lettre Type Transformée en Libre O/N </summary>
            public string Shtrf { get; set; } 
            
            ///<summary> Courrier de "copie à"            O/N </summary>
            public string Shcop { get; set; } 
            
            ///<summary> refus de Garantie O/N </summary>
            public string Shrfg { get; set; } 
            
            ///<summary> Code interlocuteur sur 5 </summary>
            public int Shin5 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Ysincou  x=this,  y=obj as Ysincou;
            if( y == default(Ysincou) ) return false;
            return (
                    x.Shsua==y.Shsua
                    && x.Shnum==y.Shnum
                    && x.Shsbr==y.Shsbr
                    && x.Shnuc==y.Shnuc
                    && x.Shtbr==y.Shtbr
                    && x.Shsrv==y.Shsrv
                    && x.Shttr==y.Shttr
                    && x.Shelg==y.Shelg
                    && x.Shcds==y.Shcds
                    && x.Shlet==y.Shlet
                    && x.Shnae==y.Shnae
                    && x.Shtev==y.Shtev
                    && x.Shtae==y.Shtae
                    && x.Shaff==y.Shaff
                    && x.Shlbc==y.Shlbc
                    && x.Shtlt==y.Shtlt
                    && x.Shtds==y.Shtds
                    && x.Shtyi==y.Shtyi
                    && x.Shids==y.Shids
                    && x.Shinl==y.Shinl
                    && x.Shsit==y.Shsit
                    && x.Shsta==y.Shsta
                    && x.Shstm==y.Shstm
                    && x.Shstj==y.Shstj
                    && x.Shspa==y.Shspa
                    && x.Shspm==y.Shspm
                    && x.Shspj==y.Shspj
                    && x.Shncp==y.Shncp
                    && x.Shajt==y.Shajt
                    && x.Shtvl==y.Shtvl
                    && x.Shcru==y.Shcru
                    && x.Shcra==y.Shcra
                    && x.Shcrm==y.Shcrm
                    && x.Shcrj==y.Shcrj
                    && x.Shmju==y.Shmju
                    && x.Shmja==y.Shmja
                    && x.Shmjm==y.Shmjm
                    && x.Shmjj==y.Shmjj
                    && x.Shles==y.Shles
                    && x.Shenv==y.Shenv
                    && x.Shchr==y.Shchr
                    && x.Shrcd==y.Shrcd
                    && x.Shrcc==y.Shrcc
                    && x.Shcou==y.Shcou
                    && x.Shcdo==y.Shcdo
                    && x.Shact==y.Shact
                    && x.Shtrf==y.Shtrf
                    && x.Shcop==y.Shcop
                    && x.Shrfg==y.Shrfg
                    && x.Shin5==y.Shin5  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Shsua.GetHashCode() ) 
                      * 23 ) + (this.Shnum.GetHashCode() ) 
                      * 23 ) + (this.Shsbr?? "").GetHashCode()
                      * 23 ) + (this.Shnuc.GetHashCode() ) 
                      * 23 ) + (this.Shtbr?? "").GetHashCode()
                      * 23 ) + (this.Shsrv?? "").GetHashCode()
                      * 23 ) + (this.Shttr?? "").GetHashCode()
                      * 23 ) + (this.Shelg?? "").GetHashCode()
                      * 23 ) + (this.Shcds?? "").GetHashCode()
                      * 23 ) + (this.Shlet?? "").GetHashCode()
                      * 23 ) + (this.Shnae?? "").GetHashCode()
                      * 23 ) + (this.Shtev?? "").GetHashCode()
                      * 23 ) + (this.Shtae?? "").GetHashCode()
                      * 23 ) + (this.Shaff?? "").GetHashCode()
                      * 23 ) + (this.Shlbc?? "").GetHashCode()
                      * 23 ) + (this.Shtlt?? "").GetHashCode()
                      * 23 ) + (this.Shtds?? "").GetHashCode()
                      * 23 ) + (this.Shtyi?? "").GetHashCode()
                      * 23 ) + (this.Shids.GetHashCode() ) 
                      * 23 ) + (this.Shinl.GetHashCode() ) 
                      * 23 ) + (this.Shsit?? "").GetHashCode()
                      * 23 ) + (this.Shsta.GetHashCode() ) 
                      * 23 ) + (this.Shstm.GetHashCode() ) 
                      * 23 ) + (this.Shstj.GetHashCode() ) 
                      * 23 ) + (this.Shspa.GetHashCode() ) 
                      * 23 ) + (this.Shspm.GetHashCode() ) 
                      * 23 ) + (this.Shspj.GetHashCode() ) 
                      * 23 ) + (this.Shncp.GetHashCode() ) 
                      * 23 ) + (this.Shajt?? "").GetHashCode()
                      * 23 ) + (this.Shtvl?? "").GetHashCode()
                      * 23 ) + (this.Shcru?? "").GetHashCode()
                      * 23 ) + (this.Shcra.GetHashCode() ) 
                      * 23 ) + (this.Shcrm.GetHashCode() ) 
                      * 23 ) + (this.Shcrj.GetHashCode() ) 
                      * 23 ) + (this.Shmju?? "").GetHashCode()
                      * 23 ) + (this.Shmja.GetHashCode() ) 
                      * 23 ) + (this.Shmjm.GetHashCode() ) 
                      * 23 ) + (this.Shmjj.GetHashCode() ) 
                      * 23 ) + (this.Shles?? "").GetHashCode()
                      * 23 ) + (this.Shenv?? "").GetHashCode()
                      * 23 ) + (this.Shchr.GetHashCode() ) 
                      * 23 ) + (this.Shrcd?? "").GetHashCode()
                      * 23 ) + (this.Shrcc.GetHashCode() ) 
                      * 23 ) + (this.Shcou.GetHashCode() ) 
                      * 23 ) + (this.Shcdo?? "").GetHashCode()
                      * 23 ) + (this.Shact?? "").GetHashCode()
                      * 23 ) + (this.Shtrf?? "").GetHashCode()
                      * 23 ) + (this.Shcop?? "").GetHashCode()
                      * 23 ) + (this.Shrfg?? "").GetHashCode()
                      * 23 ) + (this.Shin5.GetHashCode() )                    );
           }
        }
    }
}
