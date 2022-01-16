using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YPRIMES
    public partial class YPrime  {
             //YPRIMES

            ///<summary>Public empty contructor</summary>
            public YPrime() {}
            ///<summary>Public empty contructor</summary>
            public YPrime(YPrime copyFrom) 
            {
                  this.Pkipb= copyFrom.Pkipb;
                  this.Pkalx= copyFrom.Pkalx;
                  this.Pkipk= copyFrom.Pkipk;
                  this.Pkavn= copyFrom.Pkavn;
                  this.Pkavc= copyFrom.Pkavc;
                  this.Pkavi= copyFrom.Pkavi;
                  this.Pkcap= copyFrom.Pkcap;
                  this.Pkeha= copyFrom.Pkeha;
                  this.Pkehm= copyFrom.Pkehm;
                  this.Pkehj= copyFrom.Pkehj;
                  this.Pkemt= copyFrom.Pkemt;
                  this.Pkema= copyFrom.Pkema;
                  this.Pkemm= copyFrom.Pkemm;
                  this.Pkemj= copyFrom.Pkemj;
                  this.Pkope= copyFrom.Pkope;
                  this.Pkmvt= copyFrom.Pkmvt;
                  this.Pkmvm= copyFrom.Pkmvm;
                  this.Pksit= copyFrom.Pksit;
                  this.Pksta= copyFrom.Pksta;
                  this.Pkstm= copyFrom.Pkstm;
                  this.Pkstj= copyFrom.Pkstj;
                  this.Pkpra= copyFrom.Pkpra;
                  this.Pkpnt= copyFrom.Pkpnt;
                  this.Pkmht= copyFrom.Pkmht;
                  this.Pkmtx= copyFrom.Pkmtx;
                  this.Pkafr= copyFrom.Pkafr;
                  this.Pkaft= copyFrom.Pkaft;
                  this.Pkatm= copyFrom.Pkatm;
                  this.Pkttt= copyFrom.Pkttt;
                  this.Pkmtt= copyFrom.Pkmtt;
                  this.Pkidv= copyFrom.Pkidv;
                  this.Pkmtr= copyFrom.Pkmtr;
                  this.Pkper= copyFrom.Pkper;
                  this.Pknpl= copyFrom.Pknpl;
                  this.Pkcot= copyFrom.Pkcot;
                  this.Pkcom= copyFrom.Pkcom;
                  this.Pkdpa= copyFrom.Pkdpa;
                  this.Pkdpm= copyFrom.Pkdpm;
                  this.Pkdpj= copyFrom.Pkdpj;
                  this.Pkfpa= copyFrom.Pkfpa;
                  this.Pkfpm= copyFrom.Pkfpm;
                  this.Pkpfj= copyFrom.Pkpfj;
                  this.Pkcpt= copyFrom.Pkcpt;
                  this.Pkrlc= copyFrom.Pkrlc;
                  this.Pkrla= copyFrom.Pkrla;
                  this.Pkrlm= copyFrom.Pkrlm;
                  this.Pkrlj= copyFrom.Pkrlj;
                  this.Pkrll= copyFrom.Pkrll;
                  this.Pkcru= copyFrom.Pkcru;
                  this.Pkcra= copyFrom.Pkcra;
                  this.Pkcrm= copyFrom.Pkcrm;
                  this.Pkcrj= copyFrom.Pkcrj;
                  this.Pkmju= copyFrom.Pkmju;
                  this.Pkmja= copyFrom.Pkmja;
                  this.Pkmjm= copyFrom.Pkmjm;
                  this.Pkmjj= copyFrom.Pkmjj;
                  this.Pkenc= copyFrom.Pkenc;
                  this.Pkmot= copyFrom.Pkmot;
                  this.Pkcnh= copyFrom.Pkcnh;
                  this.Pkcnt= copyFrom.Pkcnt;
                  this.Pkcnl= copyFrom.Pkcnl;
                  this.Pkcnm= copyFrom.Pkcnm;
                  this.Pkcnc= copyFrom.Pkcnc;
                  this.Pkict= copyFrom.Pkict;
                  this.Pkdev= copyFrom.Pkdev;
                  this.Pkcpa= copyFrom.Pkcpa;
                  this.Pkcpm= copyFrom.Pkcpm;
                  this.Pktac= copyFrom.Pktac;
                  this.Pkaff= copyFrom.Pkaff;
                  this.Pkdvr= copyFrom.Pkdvr;
                  this.Pkkca= copyFrom.Pkkca;
                  this.Pkkht= copyFrom.Pkkht;
                  this.Pkkhx= copyFrom.Pkkhx;
                  this.Pkkfa= copyFrom.Pkkfa;
                  this.Pkkft= copyFrom.Pkkft;
                  this.Pkkat= copyFrom.Pkkat;
                  this.Pkktx= copyFrom.Pkktx;
                  this.Pkktt= copyFrom.Pkktt;
                  this.Pkktr= copyFrom.Pkktr;
                  this.Pkkco= copyFrom.Pkkco;
                  this.Pkknh= copyFrom.Pkknh;
                  this.Pkknt= copyFrom.Pkknt;
                  this.Pkknl= copyFrom.Pkknl;
                  this.Pkknm= copyFrom.Pkknm;
                  this.Pkaar= copyFrom.Pkaar;
                  this.Pkaxc= copyFrom.Pkaxc;
                  this.Pkmce= copyFrom.Pkmce;
                  this.Pkkce= copyFrom.Pkkce;
                  this.Pkdca= copyFrom.Pkdca;
                  this.Pkdcm= copyFrom.Pkdcm;
                  this.Pkdcj= copyFrom.Pkdcj;
                  this.Pkfca= copyFrom.Pkfca;
                  this.Pkfcm= copyFrom.Pkfcm;
                  this.Pkfcj= copyFrom.Pkfcj;
                  this.Pkdem= copyFrom.Pkdem;
                  this.Pkgrg= copyFrom.Pkgrg;
                  this.Pkgrn= copyFrom.Pkgrn;
                  this.Pkgrh= copyFrom.Pkgrh;
                  this.Pkgrb= copyFrom.Pkgrb;
                  this.Pkkrg= copyFrom.Pkkrg;
                  this.Pkkrn= copyFrom.Pkkrn;
                  this.Pkkrh= copyFrom.Pkkrh;
                  this.Pkkrb= copyFrom.Pkkrb;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Pkipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pkalx { get; set; } 
            
            ///<summary> N° de prime / Police </summary>
            public int Pkipk { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Pkavn { get; set; } 
            
            ///<summary> Code Avis d'échéance </summary>
            public string Pkavc { get; set; } 
            
            ///<summary> N° Avis échéance </summary>
            public int Pkavi { get; set; } 
            
            ///<summary> Montant capital </summary>
            public Int64 Pkcap { get; set; } 
            
            ///<summary> Echéance : Année </summary>
            public int Pkeha { get; set; } 
            
            ///<summary> Echéance : Mois </summary>
            public int Pkehm { get; set; } 
            
            ///<summary> Echéance : Jour </summary>
            public int Pkehj { get; set; } 
            
            ///<summary> Type d'émission  Comptant Terme </summary>
            public string Pkemt { get; set; } 
            
            ///<summary> Emission : Année </summary>
            public int Pkema { get; set; } 
            
            ///<summary> Emission : Mois </summary>
            public int Pkemm { get; set; } 
            
            ///<summary> Emission : Jour </summary>
            public int Pkemj { get; set; } 
            
            ///<summary> Code opération </summary>
            public int Pkope { get; set; } 
            
            ///<summary> Type de mouvement  (Af nouv Régul .) </summary>
            public string Pkmvt { get; set; } 
            
            ///<summary> Motif mouvement (Avenant) </summary>
            public string Pkmvm { get; set; } 
            
            ///<summary> Code situation </summary>
            public string Pksit { get; set; } 
            
            ///<summary> Situation : Année </summary>
            public int Pksta { get; set; } 
            
            ///<summary> Situation : Mois </summary>
            public int Pkstm { get; set; } 
            
            ///<summary> Situation : Jour </summary>
            public int Pkstj { get; set; } 
            
            ///<summary> N° de prime annulée ou remplacée </summary>
            public int Pkpra { get; set; } 
            
            ///<summary> Nature de la prime </summary>
            public int Pkpnt { get; set; } 
            
            ///<summary> Montant prime HT (Avec CATNAT) </summary>
            public Decimal Pkmht { get; set; } 
            
            ///<summary> Montant de taxe (Hors CATNAT) </summary>
            public Decimal Pkmtx { get; set; } 
            
            ///<summary> Montant de frais accessoires </summary>
            public int Pkafr { get; set; } 
            
            ///<summary> Taxe sur accessoire </summary>
            public Decimal Pkaft { get; set; } 
            
            ///<summary> Taxe attentat </summary>
            public Decimal Pkatm { get; set; } 
            
            ///<summary> Total taxes (Avec CATNAT) </summary>
            public Decimal Pkttt { get; set; } 
            
            ///<summary> Montant prime TTC (Avec CATNAT) </summary>
            public Decimal Pkmtt { get; set; } 
            
            ///<summary> Valeur de l'indice </summary>
            public Decimal Pkidv { get; set; } 
            
            ///<summary> Montant Total règlement       Cf DVR </summary>
            public Decimal Pkmtr { get; set; } 
            
            ///<summary> Code périodicité Police </summary>
            public string Pkper { get; set; } 
            
            ///<summary> Nature Police ( ' ' / C / A ) </summary>
            public string Pknpl { get; set; } 
            
            ///<summary> Mnt commissions (BM pour RT) </summary>
            public Decimal Pkcot { get; set; } 
            
            ///<summary> Taux de commission (BM pour RT) </summary>
            public Decimal Pkcom { get; set; } 
            
            ///<summary> Année Début de période </summary>
            public int Pkdpa { get; set; } 
            
            ///<summary> Mois  Début de période </summary>
            public int Pkdpm { get; set; } 
            
            ///<summary> Jour  Début de période </summary>
            public int Pkdpj { get; set; } 
            
            ///<summary> Année Fin de période </summary>
            public int Pkfpa { get; set; } 
            
            ///<summary> Mois  Fin de période </summary>
            public int Pkfpm { get; set; } 
            
            ///<summary> Jour  Fin de période </summary>
            public int Pkpfj { get; set; } 
            
            ///<summary> Comptabilisation O/N </summary>
            public string Pkcpt { get; set; } 
            
            ///<summary> Code  de dernière relance </summary>
            public string Pkrlc { get; set; } 
            
            ///<summary> Année de dernière relance </summary>
            public int Pkrla { get; set; } 
            
            ///<summary> Mois  de dernière relance </summary>
            public int Pkrlm { get; set; } 
            
            ///<summary> Jour  de dernière relance </summary>
            public int Pkrlj { get; set; } 
            
            ///<summary> N° de courrier de dernière relance </summary>
            public int Pkrll { get; set; } 
            
            ///<summary> User création </summary>
            public string Pkcru { get; set; } 
            
            ///<summary> Année création </summary>
            public int Pkcra { get; set; } 
            
            ///<summary> Mois création </summary>
            public int Pkcrm { get; set; } 
            
            ///<summary> Jour création </summary>
            public int Pkcrj { get; set; } 
            
            ///<summary> User Màj </summary>
            public string Pkmju { get; set; } 
            
            ///<summary> Année Màj </summary>
            public int Pkmja { get; set; } 
            
            ///<summary> Mois Màj </summary>
            public int Pkmjm { get; set; } 
            
            ///<summary> Jour Màj </summary>
            public int Pkmjj { get; set; } 
            
            ///<summary> Code encaissement </summary>
            public string Pkenc { get; set; } 
            
            ///<summary> Motif annulation/réémission </summary>
            public string Pkmot { get; set; } 
            
            ///<summary> CATNAT : Montant HT </summary>
            public Decimal Pkcnh { get; set; } 
            
            ///<summary> CATNAT : Montant de taxe </summary>
            public Decimal Pkcnt { get; set; } 
            
            ///<summary> CATNAT : Montant TTC </summary>
            public Decimal Pkcnl { get; set; } 
            
            ///<summary> CATNAT : Montant de commission </summary>
            public Decimal Pkcnm { get; set; } 
            
            ///<summary> CATNAT : Taux de commission </summary>
            public Decimal Pkcnc { get; set; } 
            
            ///<summary> Identifiant courtier </summary>
            public int Pkict { get; set; } 
            
            ///<summary> Code devise d'émission </summary>
            public string Pkdev { get; set; } 
            
            ///<summary> Année de comptabilisation </summary>
            public int Pkcpa { get; set; } 
            
            ///<summary> Mois de comptabilisation </summary>
            public int Pkcpm { get; set; } 
            
            ///<summary> Type accord S Signée N Non signée .. </summary>
            public string Pktac { get; set; } 
            
            ///<summary> A afficher O/N </summary>
            public string Pkaff { get; set; } 
            
            ///<summary> Code devise de règlement (Le 1er) </summary>
            public string Pkdvr { get; set; } 
            
            ///<summary> Montant capital              Dev Cpt </summary>
            public Decimal Pkkca { get; set; } 
            
            ///<summary> Mnt HT (avec CATNAT)         Dev Cpt </summary>
            public Decimal Pkkht { get; set; } 
            
            ///<summary> Mnt taxes (Hors CATNAT)      Dev Cpt </summary>
            public Decimal Pkkhx { get; set; } 
            
            ///<summary> Mnt frais accessoires        Dev Cpt </summary>
            public Decimal Pkkfa { get; set; } 
            
            ///<summary> Taxe sur accessoire          Dev Cpt </summary>
            public Decimal Pkkft { get; set; } 
            
            ///<summary> Taxe attentat                Dev Cpt </summary>
            public Decimal Pkkat { get; set; } 
            
            ///<summary> Total taxes (avec CATNAT)    Dev Cpt </summary>
            public Decimal Pkktx { get; set; } 
            
            ///<summary> Montant prime TTC (avec CN)  Dev Cpt </summary>
            public Decimal Pkktt { get; set; } 
            
            ///<summary> Mnt total Règlement          Dev Cpt </summary>
            public Decimal Pkktr { get; set; } 
            
            ///<summary> Mnt  commissions(BM en RT)   Dev Cpt </summary>
            public Decimal Pkkco { get; set; } 
            
            ///<summary> CATNAT : Montant HT          Dev Cpt </summary>
            public Decimal Pkknh { get; set; } 
            
            ///<summary> CATNAT : Mnt de taxe         Dev Cpt </summary>
            public Decimal Pkknt { get; set; } 
            
            ///<summary> CATNAT : Montant TTC         Dev Cpt </summary>
            public Decimal Pkknl { get; set; } 
            
            ///<summary> CATNAT : Mnt commissions     Dev Cpt </summary>
            public Decimal Pkknm { get; set; } 
            
            ///<summary> GAREAT Tranche </summary>
            public string Pkaar { get; set; } 
            
            ///<summary> GAREAT Taux de cession </summary>
            public Decimal Pkaxc { get; set; } 
            
            ///<summary> GAREAT Montant cession Devise Police </summary>
            public Decimal Pkmce { get; set; } 
            
            ///<summary> GAREAT Montant Cession       Dev Cpt </summary>
            public Decimal Pkkce { get; set; } 
            
            ///<summary> GAREAT Début Cession AA </summary>
            public int Pkdca { get; set; } 
            
            ///<summary> GAREAT Début Cession MM </summary>
            public int Pkdcm { get; set; } 
            
            ///<summary> GAREAT Début Cession JJ </summary>
            public int Pkdcj { get; set; } 
            
            ///<summary> GAREAT Fin Cession AA </summary>
            public int Pkfca { get; set; } 
            
            ///<summary> GAREAT Fin Cession MM </summary>
            public int Pkfcm { get; set; } 
            
            ///<summary> GAREAT Fin Cession JJ </summary>
            public int Pkfcj { get; set; } 
            
            ///<summary> Emission à la demande O/N </summary>
            public string Pkdem { get; set; } 
            
            ///<summary> GAREAT Recalcul Mnt GAREAT </summary>
            public Decimal Pkgrg { get; set; } 
            
            ///<summary> GAREAT Recalcul Montant CATNAT </summary>
            public Decimal Pkgrn { get; set; } 
            
            ///<summary> GAREAT Recalcul HT avec CN </summary>
            public Decimal Pkgrh { get; set; } 
            
            ///<summary> GAREAT Recalcul Base CN </summary>
            public Decimal Pkgrb { get; set; } 
            
            ///<summary> GAREAT Recalcul Mnt GAREAT   Dev CPT </summary>
            public Decimal Pkkrg { get; set; } 
            
            ///<summary> GAREAT Recalcul CATNAT       Dev CPT </summary>
            public Decimal Pkkrn { get; set; } 
            
            ///<summary> GAREAT Recalcul HT avec CN   Dev CPT </summary>
            public Decimal Pkkrh { get; set; } 
            
            ///<summary> GAREAT Recalcul Base CN      Dev CPT </summary>
            public Decimal Pkkrb { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YPrime  x=this,  y=obj as YPrime;
            if( y == default(YPrime) ) return false;
            return (
                    x.Pkipb==y.Pkipb
                    && x.Pkalx==y.Pkalx
                    && x.Pkipk==y.Pkipk
                    && x.Pkavn==y.Pkavn
                    && x.Pkavc==y.Pkavc
                    && x.Pkavi==y.Pkavi
                    && x.Pkcap==y.Pkcap
                    && x.Pkeha==y.Pkeha
                    && x.Pkehm==y.Pkehm
                    && x.Pkehj==y.Pkehj
                    && x.Pkemt==y.Pkemt
                    && x.Pkema==y.Pkema
                    && x.Pkemm==y.Pkemm
                    && x.Pkemj==y.Pkemj
                    && x.Pkope==y.Pkope
                    && x.Pkmvt==y.Pkmvt
                    && x.Pkmvm==y.Pkmvm
                    && x.Pksit==y.Pksit
                    && x.Pksta==y.Pksta
                    && x.Pkstm==y.Pkstm
                    && x.Pkstj==y.Pkstj
                    && x.Pkpra==y.Pkpra
                    && x.Pkpnt==y.Pkpnt
                    && x.Pkmht==y.Pkmht
                    && x.Pkmtx==y.Pkmtx
                    && x.Pkafr==y.Pkafr
                    && x.Pkaft==y.Pkaft
                    && x.Pkatm==y.Pkatm
                    && x.Pkttt==y.Pkttt
                    && x.Pkmtt==y.Pkmtt
                    && x.Pkidv==y.Pkidv
                    && x.Pkmtr==y.Pkmtr
                    && x.Pkper==y.Pkper
                    && x.Pknpl==y.Pknpl
                    && x.Pkcot==y.Pkcot
                    && x.Pkcom==y.Pkcom
                    && x.Pkdpa==y.Pkdpa
                    && x.Pkdpm==y.Pkdpm
                    && x.Pkdpj==y.Pkdpj
                    && x.Pkfpa==y.Pkfpa
                    && x.Pkfpm==y.Pkfpm
                    && x.Pkpfj==y.Pkpfj
                    && x.Pkcpt==y.Pkcpt
                    && x.Pkrlc==y.Pkrlc
                    && x.Pkrla==y.Pkrla
                    && x.Pkrlm==y.Pkrlm
                    && x.Pkrlj==y.Pkrlj
                    && x.Pkrll==y.Pkrll
                    && x.Pkcru==y.Pkcru
                    && x.Pkcra==y.Pkcra
                    && x.Pkcrm==y.Pkcrm
                    && x.Pkcrj==y.Pkcrj
                    && x.Pkmju==y.Pkmju
                    && x.Pkmja==y.Pkmja
                    && x.Pkmjm==y.Pkmjm
                    && x.Pkmjj==y.Pkmjj
                    && x.Pkenc==y.Pkenc
                    && x.Pkmot==y.Pkmot
                    && x.Pkcnh==y.Pkcnh
                    && x.Pkcnt==y.Pkcnt
                    && x.Pkcnl==y.Pkcnl
                    && x.Pkcnm==y.Pkcnm
                    && x.Pkcnc==y.Pkcnc
                    && x.Pkict==y.Pkict
                    && x.Pkdev==y.Pkdev
                    && x.Pkcpa==y.Pkcpa
                    && x.Pkcpm==y.Pkcpm
                    && x.Pktac==y.Pktac
                    && x.Pkaff==y.Pkaff
                    && x.Pkdvr==y.Pkdvr
                    && x.Pkkca==y.Pkkca
                    && x.Pkkht==y.Pkkht
                    && x.Pkkhx==y.Pkkhx
                    && x.Pkkfa==y.Pkkfa
                    && x.Pkkft==y.Pkkft
                    && x.Pkkat==y.Pkkat
                    && x.Pkktx==y.Pkktx
                    && x.Pkktt==y.Pkktt
                    && x.Pkktr==y.Pkktr
                    && x.Pkkco==y.Pkkco
                    && x.Pkknh==y.Pkknh
                    && x.Pkknt==y.Pkknt
                    && x.Pkknl==y.Pkknl
                    && x.Pkknm==y.Pkknm
                    && x.Pkaar==y.Pkaar
                    && x.Pkaxc==y.Pkaxc
                    && x.Pkmce==y.Pkmce
                    && x.Pkkce==y.Pkkce
                    && x.Pkdca==y.Pkdca
                    && x.Pkdcm==y.Pkdcm
                    && x.Pkdcj==y.Pkdcj
                    && x.Pkfca==y.Pkfca
                    && x.Pkfcm==y.Pkfcm
                    && x.Pkfcj==y.Pkfcj
                    && x.Pkdem==y.Pkdem
                    && x.Pkgrg==y.Pkgrg
                    && x.Pkgrn==y.Pkgrn
                    && x.Pkgrh==y.Pkgrh
                    && x.Pkgrb==y.Pkgrb
                    && x.Pkkrg==y.Pkkrg
                    && x.Pkkrn==y.Pkkrn
                    && x.Pkkrh==y.Pkkrh
                    && x.Pkkrb==y.Pkkrb  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Pkipb?? "").GetHashCode()
                      * 23 ) + (this.Pkalx.GetHashCode() ) 
                      * 23 ) + (this.Pkipk.GetHashCode() ) 
                      * 23 ) + (this.Pkavn.GetHashCode() ) 
                      * 23 ) + (this.Pkavc?? "").GetHashCode()
                      * 23 ) + (this.Pkavi.GetHashCode() ) 
                      * 23 ) + (this.Pkcap.GetHashCode() ) 
                      * 23 ) + (this.Pkeha.GetHashCode() ) 
                      * 23 ) + (this.Pkehm.GetHashCode() ) 
                      * 23 ) + (this.Pkehj.GetHashCode() ) 
                      * 23 ) + (this.Pkemt?? "").GetHashCode()
                      * 23 ) + (this.Pkema.GetHashCode() ) 
                      * 23 ) + (this.Pkemm.GetHashCode() ) 
                      * 23 ) + (this.Pkemj.GetHashCode() ) 
                      * 23 ) + (this.Pkope.GetHashCode() ) 
                      * 23 ) + (this.Pkmvt?? "").GetHashCode()
                      * 23 ) + (this.Pkmvm?? "").GetHashCode()
                      * 23 ) + (this.Pksit?? "").GetHashCode()
                      * 23 ) + (this.Pksta.GetHashCode() ) 
                      * 23 ) + (this.Pkstm.GetHashCode() ) 
                      * 23 ) + (this.Pkstj.GetHashCode() ) 
                      * 23 ) + (this.Pkpra.GetHashCode() ) 
                      * 23 ) + (this.Pkpnt.GetHashCode() ) 
                      * 23 ) + (this.Pkmht.GetHashCode() ) 
                      * 23 ) + (this.Pkmtx.GetHashCode() ) 
                      * 23 ) + (this.Pkafr.GetHashCode() ) 
                      * 23 ) + (this.Pkaft.GetHashCode() ) 
                      * 23 ) + (this.Pkatm.GetHashCode() ) 
                      * 23 ) + (this.Pkttt.GetHashCode() ) 
                      * 23 ) + (this.Pkmtt.GetHashCode() ) 
                      * 23 ) + (this.Pkidv.GetHashCode() ) 
                      * 23 ) + (this.Pkmtr.GetHashCode() ) 
                      * 23 ) + (this.Pkper?? "").GetHashCode()
                      * 23 ) + (this.Pknpl?? "").GetHashCode()
                      * 23 ) + (this.Pkcot.GetHashCode() ) 
                      * 23 ) + (this.Pkcom.GetHashCode() ) 
                      * 23 ) + (this.Pkdpa.GetHashCode() ) 
                      * 23 ) + (this.Pkdpm.GetHashCode() ) 
                      * 23 ) + (this.Pkdpj.GetHashCode() ) 
                      * 23 ) + (this.Pkfpa.GetHashCode() ) 
                      * 23 ) + (this.Pkfpm.GetHashCode() ) 
                      * 23 ) + (this.Pkpfj.GetHashCode() ) 
                      * 23 ) + (this.Pkcpt?? "").GetHashCode()
                      * 23 ) + (this.Pkrlc?? "").GetHashCode()
                      * 23 ) + (this.Pkrla.GetHashCode() ) 
                      * 23 ) + (this.Pkrlm.GetHashCode() ) 
                      * 23 ) + (this.Pkrlj.GetHashCode() ) 
                      * 23 ) + (this.Pkrll.GetHashCode() ) 
                      * 23 ) + (this.Pkcru?? "").GetHashCode()
                      * 23 ) + (this.Pkcra.GetHashCode() ) 
                      * 23 ) + (this.Pkcrm.GetHashCode() ) 
                      * 23 ) + (this.Pkcrj.GetHashCode() ) 
                      * 23 ) + (this.Pkmju?? "").GetHashCode()
                      * 23 ) + (this.Pkmja.GetHashCode() ) 
                      * 23 ) + (this.Pkmjm.GetHashCode() ) 
                      * 23 ) + (this.Pkmjj.GetHashCode() ) 
                      * 23 ) + (this.Pkenc?? "").GetHashCode()
                      * 23 ) + (this.Pkmot?? "").GetHashCode()
                      * 23 ) + (this.Pkcnh.GetHashCode() ) 
                      * 23 ) + (this.Pkcnt.GetHashCode() ) 
                      * 23 ) + (this.Pkcnl.GetHashCode() ) 
                      * 23 ) + (this.Pkcnm.GetHashCode() ) 
                      * 23 ) + (this.Pkcnc.GetHashCode() ) 
                      * 23 ) + (this.Pkict.GetHashCode() ) 
                      * 23 ) + (this.Pkdev?? "").GetHashCode()
                      * 23 ) + (this.Pkcpa.GetHashCode() ) 
                      * 23 ) + (this.Pkcpm.GetHashCode() ) 
                      * 23 ) + (this.Pktac?? "").GetHashCode()
                      * 23 ) + (this.Pkaff?? "").GetHashCode()
                      * 23 ) + (this.Pkdvr?? "").GetHashCode()
                      * 23 ) + (this.Pkkca.GetHashCode() ) 
                      * 23 ) + (this.Pkkht.GetHashCode() ) 
                      * 23 ) + (this.Pkkhx.GetHashCode() ) 
                      * 23 ) + (this.Pkkfa.GetHashCode() ) 
                      * 23 ) + (this.Pkkft.GetHashCode() ) 
                      * 23 ) + (this.Pkkat.GetHashCode() ) 
                      * 23 ) + (this.Pkktx.GetHashCode() ) 
                      * 23 ) + (this.Pkktt.GetHashCode() ) 
                      * 23 ) + (this.Pkktr.GetHashCode() ) 
                      * 23 ) + (this.Pkkco.GetHashCode() ) 
                      * 23 ) + (this.Pkknh.GetHashCode() ) 
                      * 23 ) + (this.Pkknt.GetHashCode() ) 
                      * 23 ) + (this.Pkknl.GetHashCode() ) 
                      * 23 ) + (this.Pkknm.GetHashCode() ) 
                      * 23 ) + (this.Pkaar?? "").GetHashCode()
                      * 23 ) + (this.Pkaxc.GetHashCode() ) 
                      * 23 ) + (this.Pkmce.GetHashCode() ) 
                      * 23 ) + (this.Pkkce.GetHashCode() ) 
                      * 23 ) + (this.Pkdca.GetHashCode() ) 
                      * 23 ) + (this.Pkdcm.GetHashCode() ) 
                      * 23 ) + (this.Pkdcj.GetHashCode() ) 
                      * 23 ) + (this.Pkfca.GetHashCode() ) 
                      * 23 ) + (this.Pkfcm.GetHashCode() ) 
                      * 23 ) + (this.Pkfcj.GetHashCode() ) 
                      * 23 ) + (this.Pkdem?? "").GetHashCode()
                      * 23 ) + (this.Pkgrg.GetHashCode() ) 
                      * 23 ) + (this.Pkgrn.GetHashCode() ) 
                      * 23 ) + (this.Pkgrh.GetHashCode() ) 
                      * 23 ) + (this.Pkgrb.GetHashCode() ) 
                      * 23 ) + (this.Pkkrg.GetHashCode() ) 
                      * 23 ) + (this.Pkkrn.GetHashCode() ) 
                      * 23 ) + (this.Pkkrh.GetHashCode() ) 
                      * 23 ) + (this.Pkkrb.GetHashCode() )                    );
           }
        }
    }
}
