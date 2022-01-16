using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YCOURTI
    public partial class Ycourti  {
             //YCOURTI

            ///<summary>Public empty contructor</summary>
            public Ycourti() {}
            ///<summary>Public empty contructor</summary>
            public Ycourti(Ycourti copyFrom) 
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
  


        public override bool Equals(object  obj)
        {
            Ycourti  x=this,  y=obj as Ycourti;
            if( y == default(Ycourti) ) return false;
            return (
                    x.Tcict==y.Tcict
                    && x.Tctyp==y.Tctyp
                    && x.Tcici==y.Tcici
                    && x.Tcad1==y.Tcad1
                    && x.Tcad2==y.Tcad2
                    && x.Tcdep==y.Tcdep
                    && x.Tccpo==y.Tccpo
                    && x.Tcvil==y.Tcvil
                    && x.Tcpay==y.Tcpay
                    && x.Tccom==y.Tccom
                    && x.Tcreg==y.Tcreg
                    && x.Tcbur==y.Tcbur
                    && x.Tcfdc==y.Tcfdc
                    && x.Tctel==y.Tctel
                    && x.Tctlc==y.Tctlc
                    && x.Tcbqe==y.Tcbqe
                    && x.Tcgui==y.Tcgui
                    && x.Tccpt==y.Tccpt
                    && x.Tcrib==y.Tcrib
                    && x.Tcicp==y.Tcicp
                    && x.Tcori==y.Tcori
                    && x.Tctin==y.Tctin
                    && x.Tccii==y.Tccii
                    && x.Tcapg==y.Tcapg
                    && x.Tclig==y.Tclig
                    && x.Tcrgc==y.Tcrgc
                    && x.Tcenc==y.Tcenc
                    && x.Tcman==y.Tcman
                    && x.Tcdcp==y.Tcdcp
                    && x.Tcraf==y.Tcraf
                    && x.Tccha==y.Tccha
                    && x.Tcgep==y.Tcgep
                    && x.Tcprd==y.Tcprd
                    && x.Tccra==y.Tccra
                    && x.Tccrm==y.Tccrm
                    && x.Tccrj==y.Tccrj
                    && x.Tcfva==y.Tcfva
                    && x.Tcfvm==y.Tcfvm
                    && x.Tcfvj==y.Tcfvj
                    && x.Tcrpl==y.Tcrpl
                    && x.Tcusr==y.Tcusr
                    && x.Tcmja==y.Tcmja
                    && x.Tcmjm==y.Tcmjm
                    && x.Tcmjj==y.Tcmjj
                    && x.Tcold==y.Tcold
                    && x.Tcape==y.Tcape
                    && x.Tcins==y.Tcins
                    && x.Tcspe==y.Tcspe
                    && x.Tcyen==y.Tcyen
                    && x.Tcaem==y.Tcaem
                    && x.Tcioo==y.Tcioo
                    && x.Tcioc==y.Tcioc
                    && x.Tcioj==y.Tcioj
                    && x.Tciom==y.Tciom
                    && x.Tcioa==y.Tcioa
                    && x.Tciag==y.Tciag
                    && x.Tciaj==y.Tciaj
                    && x.Tciam==y.Tciam
                    && x.Tciaa==y.Tciaa
                    && x.Tciii==y.Tciii
                    && x.Tciij==y.Tciij
                    && x.Tciim==y.Tciim
                    && x.Tciia==y.Tciia
                    && x.Tcimd==y.Tcimd
                    && x.Tcimj==y.Tcimj
                    && x.Tcimm==y.Tcimm
                    && x.Tcima==y.Tcima
                    && x.Tcadh==y.Tcadh
                    && x.Tcrcv==y.Tcrcv
                    && x.Tcrcs==y.Tcrcs
                    && x.Tcinm==y.Tcinm
                    && x.Tcsec==y.Tcsec
                    && x.Tcirn==y.Tcirn
                    && x.Tcirj==y.Tcirj
                    && x.Tcirm==y.Tcirm
                    && x.Tcira==y.Tcira
                    && x.Tcrcn==y.Tcrcn
                    && x.Tcap5==y.Tcap5
                    && x.Tcedi==y.Tcedi
                    && x.Tcidn==y.Tcidn
                    && x.Tcist==y.Tcist
                    && x.Tcgem==y.Tcgem  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Tcict.GetHashCode() ) 
                      * 23 ) + (this.Tctyp?? "").GetHashCode()
                      * 23 ) + (this.Tcici?? "").GetHashCode()
                      * 23 ) + (this.Tcad1?? "").GetHashCode()
                      * 23 ) + (this.Tcad2?? "").GetHashCode()
                      * 23 ) + (this.Tcdep?? "").GetHashCode()
                      * 23 ) + (this.Tccpo?? "").GetHashCode()
                      * 23 ) + (this.Tcvil?? "").GetHashCode()
                      * 23 ) + (this.Tcpay?? "").GetHashCode()
                      * 23 ) + (this.Tccom?? "").GetHashCode()
                      * 23 ) + (this.Tcreg?? "").GetHashCode()
                      * 23 ) + (this.Tcbur?? "").GetHashCode()
                      * 23 ) + (this.Tcfdc?? "").GetHashCode()
                      * 23 ) + (this.Tctel?? "").GetHashCode()
                      * 23 ) + (this.Tctlc?? "").GetHashCode()
                      * 23 ) + (this.Tcbqe.GetHashCode() ) 
                      * 23 ) + (this.Tcgui.GetHashCode() ) 
                      * 23 ) + (this.Tccpt?? "").GetHashCode()
                      * 23 ) + (this.Tcrib.GetHashCode() ) 
                      * 23 ) + (this.Tcicp?? "").GetHashCode()
                      * 23 ) + (this.Tcori?? "").GetHashCode()
                      * 23 ) + (this.Tctin?? "").GetHashCode()
                      * 23 ) + (this.Tccii?? "").GetHashCode()
                      * 23 ) + (this.Tcapg?? "").GetHashCode()
                      * 23 ) + (this.Tclig?? "").GetHashCode()
                      * 23 ) + (this.Tcrgc?? "").GetHashCode()
                      * 23 ) + (this.Tcenc?? "").GetHashCode()
                      * 23 ) + (this.Tcman?? "").GetHashCode()
                      * 23 ) + (this.Tcdcp.GetHashCode() ) 
                      * 23 ) + (this.Tcraf.GetHashCode() ) 
                      * 23 ) + (this.Tccha?? "").GetHashCode()
                      * 23 ) + (this.Tcgep?? "").GetHashCode()
                      * 23 ) + (this.Tcprd?? "").GetHashCode()
                      * 23 ) + (this.Tccra.GetHashCode() ) 
                      * 23 ) + (this.Tccrm.GetHashCode() ) 
                      * 23 ) + (this.Tccrj.GetHashCode() ) 
                      * 23 ) + (this.Tcfva.GetHashCode() ) 
                      * 23 ) + (this.Tcfvm.GetHashCode() ) 
                      * 23 ) + (this.Tcfvj.GetHashCode() ) 
                      * 23 ) + (this.Tcrpl.GetHashCode() ) 
                      * 23 ) + (this.Tcusr?? "").GetHashCode()
                      * 23 ) + (this.Tcmja.GetHashCode() ) 
                      * 23 ) + (this.Tcmjm.GetHashCode() ) 
                      * 23 ) + (this.Tcmjj.GetHashCode() ) 
                      * 23 ) + (this.Tcold?? "").GetHashCode()
                      * 23 ) + (this.Tcape?? "").GetHashCode()
                      * 23 ) + (this.Tcins?? "").GetHashCode()
                      * 23 ) + (this.Tcspe?? "").GetHashCode()
                      * 23 ) + (this.Tcyen?? "").GetHashCode()
                      * 23 ) + (this.Tcaem?? "").GetHashCode()
                      * 23 ) + (this.Tcioo.GetHashCode() ) 
                      * 23 ) + (this.Tcioc?? "").GetHashCode()
                      * 23 ) + (this.Tcioj.GetHashCode() ) 
                      * 23 ) + (this.Tciom.GetHashCode() ) 
                      * 23 ) + (this.Tcioa.GetHashCode() ) 
                      * 23 ) + (this.Tciag?? "").GetHashCode()
                      * 23 ) + (this.Tciaj.GetHashCode() ) 
                      * 23 ) + (this.Tciam.GetHashCode() ) 
                      * 23 ) + (this.Tciaa.GetHashCode() ) 
                      * 23 ) + (this.Tciii?? "").GetHashCode()
                      * 23 ) + (this.Tciij.GetHashCode() ) 
                      * 23 ) + (this.Tciim.GetHashCode() ) 
                      * 23 ) + (this.Tciia.GetHashCode() ) 
                      * 23 ) + (this.Tcimd?? "").GetHashCode()
                      * 23 ) + (this.Tcimj.GetHashCode() ) 
                      * 23 ) + (this.Tcimm.GetHashCode() ) 
                      * 23 ) + (this.Tcima.GetHashCode() ) 
                      * 23 ) + (this.Tcadh.GetHashCode() ) 
                      * 23 ) + (this.Tcrcv?? "").GetHashCode()
                      * 23 ) + (this.Tcrcs?? "").GetHashCode()
                      * 23 ) + (this.Tcinm?? "").GetHashCode()
                      * 23 ) + (this.Tcsec?? "").GetHashCode()
                      * 23 ) + (this.Tcirn?? "").GetHashCode()
                      * 23 ) + (this.Tcirj.GetHashCode() ) 
                      * 23 ) + (this.Tcirm.GetHashCode() ) 
                      * 23 ) + (this.Tcira.GetHashCode() ) 
                      * 23 ) + (this.Tcrcn.GetHashCode() ) 
                      * 23 ) + (this.Tcap5?? "").GetHashCode()
                      * 23 ) + (this.Tcedi?? "").GetHashCode()
                      * 23 ) + (this.Tcidn?? "").GetHashCode()
                      * 23 ) + (this.Tcist?? "").GetHashCode()
                      * 23 ) + (this.Tcgem?? "").GetHashCode()                   );
           }
        }
    }
}
