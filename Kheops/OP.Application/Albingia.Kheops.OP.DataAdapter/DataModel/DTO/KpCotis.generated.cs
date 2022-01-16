using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPCOTIS
    public partial class KpCotis  {
             //HPCOTIS
             //KPCOTIS

            ///<summary>Public empty contructor</summary>
            public KpCotis() {}
            ///<summary>Public empty contructor</summary>
            public KpCotis(KpCotis copyFrom) 
            {
                  this.Kdmid= copyFrom.Kdmid;
                  this.Kdmtap= copyFrom.Kdmtap;
                  this.Kdmtyp= copyFrom.Kdmtyp;
                  this.Kdmipb= copyFrom.Kdmipb;
                  this.Kdmalx= copyFrom.Kdmalx;
                  this.Kdmavn= copyFrom.Kdmavn;
                  this.Kdmhin= copyFrom.Kdmhin;
                  this.Kdmatgmht= copyFrom.Kdmatgmht;
                  this.Kdmatgkht= copyFrom.Kdmatgkht;
                  this.Kdmatgmtx= copyFrom.Kdmatgmtx;
                  this.Kdmatgktx= copyFrom.Kdmatgktx;
                  this.Kdmatgmtt= copyFrom.Kdmatgmtt;
                  this.Kdmatgktt= copyFrom.Kdmatgktt;
                  this.Kdmatgcot= copyFrom.Kdmatgcot;
                  this.Kdmatgkco= copyFrom.Kdmatgkco;
                  this.Kdmcnabas= copyFrom.Kdmcnabas;
                  this.Kdmcnakbs= copyFrom.Kdmcnakbs;
                  this.Kdmcnamht= copyFrom.Kdmcnamht;
                  this.Kdmcnakht= copyFrom.Kdmcnakht;
                  this.Kdmcnamtx= copyFrom.Kdmcnamtx;
                  this.Kdmcnaktx= copyFrom.Kdmcnaktx;
                  this.Kdmcnamtt= copyFrom.Kdmcnamtt;
                  this.Kdmcnaktt= copyFrom.Kdmcnaktt;
                  this.Kdmcnacob= copyFrom.Kdmcnacob;
                  this.Kdmcnacnc= copyFrom.Kdmcnacnc;
                  this.Kdmcnatxf= copyFrom.Kdmcnatxf;
                  this.Kdmcnacnm= copyFrom.Kdmcnacnm;
                  this.Kdmcnacmf= copyFrom.Kdmcnacmf;
                  this.Kdmcnakcm= copyFrom.Kdmcnakcm;
                  this.Kdmgarmht= copyFrom.Kdmgarmht;
                  this.Kdmgarmtx= copyFrom.Kdmgarmtx;
                  this.Kdmgarmtt= copyFrom.Kdmgarmtt;
                  this.Kdmhfmht= copyFrom.Kdmhfmht;
                  this.Kdmhfflag= copyFrom.Kdmhfflag;
                  this.Kdmhfmhf= copyFrom.Kdmhfmhf;
                  this.Kdmhfmtx= copyFrom.Kdmhfmtx;
                  this.Kdmhfmtt= copyFrom.Kdmhfmtt;
                  this.Kdmafrb= copyFrom.Kdmafrb;
                  this.Kdmafr= copyFrom.Kdmafr;
                  this.Kdmkfa= copyFrom.Kdmkfa;
                  this.Kdmaft= copyFrom.Kdmaft;
                  this.Kdmkft= copyFrom.Kdmkft;
                  this.Kdmfga= copyFrom.Kdmfga;
                  this.Kdmkfg= copyFrom.Kdmkfg;
                  this.Kdmmht= copyFrom.Kdmmht;
                  this.Kdmmhflag= copyFrom.Kdmmhflag;
                  this.Kdmmhf= copyFrom.Kdmmhf;
                  this.Kdmkht= copyFrom.Kdmkht;
                  this.Kdmmtx= copyFrom.Kdmmtx;
                  this.Kdmktx= copyFrom.Kdmktx;
                  this.Kdmmtt= copyFrom.Kdmmtt;
                  this.Kdmmtflag= copyFrom.Kdmmtflag;
                  this.Kdmttf= copyFrom.Kdmttf;
                  this.Kdmktt= copyFrom.Kdmktt;
                  this.Kdmcob= copyFrom.Kdmcob;
                  this.Kdmcom= copyFrom.Kdmcom;
                  this.Kdmcmf= copyFrom.Kdmcmf;
                  this.Kdmcot= copyFrom.Kdmcot;
                  this.Kdmcof= copyFrom.Kdmcof;
                  this.Kdmkco= copyFrom.Kdmkco;
                  this.Kdmcoefc= copyFrom.Kdmcoefc;
        
            }        
            
            ///<summary> ID </summary>
            public Int64 Kdmid { get; set; } 
            
            ///<summary> Part T= Total A = Albingia </summary>
            public string Kdmtap { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdmtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdmipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdmalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdmavn { get; set; } 
            
            ///<summary> N°histo par avenant </summary>
            public int? Kdmhin { get; set; } 
            
            ///<summary> ATG Montant HT dev </summary>
            public Decimal Kdmatgmht { get; set; } 
            
            ///<summary> ATG Montant HT cpt </summary>
            public Decimal Kdmatgkht { get; set; } 
            
            ///<summary> ATG Montant  taxes dev </summary>
            public Decimal Kdmatgmtx { get; set; } 
            
            ///<summary> ATG montant taxes cpt </summary>
            public Decimal Kdmatgktx { get; set; } 
            
            ///<summary> ATG Montant TTC  dev </summary>
            public Decimal Kdmatgmtt { get; set; } 
            
            ///<summary> ATG Montant TTC cpt </summary>
            public Decimal Kdmatgktt { get; set; } 
            
            ///<summary> ATG Commissions dev </summary>
            public Decimal Kdmatgcot { get; set; } 
            
            ///<summary> ATG Commission  cpt </summary>
            public Decimal Kdmatgkco { get; set; } 
            
            ///<summary> CATNAT Base dev </summary>
            public Decimal Kdmcnabas { get; set; } 
            
            ///<summary> CATNAT Base cpt </summary>
            public Decimal Kdmcnakbs { get; set; } 
            
            ///<summary> CATNAT HT dev </summary>
            public Decimal Kdmcnamht { get; set; } 
            
            ///<summary> CATNAT HT cpt </summary>
            public Decimal Kdmcnakht { get; set; } 
            
            ///<summary> CATNAT Taxes dev </summary>
            public Decimal Kdmcnamtx { get; set; } 
            
            ///<summary> CATNAT Taxes cpt </summary>
            public Decimal Kdmcnaktx { get; set; } 
            
            ///<summary> CATNAT TTC dev </summary>
            public Decimal Kdmcnamtt { get; set; } 
            
            ///<summary> CATNAT TTC cpt </summary>
            public Decimal Kdmcnaktt { get; set; } 
            
            ///<summary> CATNAT Commission dans barème O/N </summary>
            public string Kdmcnacob { get; set; } 
            
            ///<summary> CATNAT Taux Commission </summary>
            public Decimal Kdmcnacnc { get; set; } 
            
            ///<summary> CATNAT Taux commission forcé </summary>
            public Decimal Kdmcnatxf { get; set; } 
            
            ///<summary> CATNAT Commission dev </summary>
            public Decimal Kdmcnacnm { get; set; } 
            
            ///<summary> CATNAT Commission forcée dev </summary>
            public Decimal Kdmcnacmf { get; set; } 
            
            ///<summary> CATNAT Commission cpt </summary>
            public Decimal Kdmcnakcm { get; set; } 
            
            ///<summary> HT hors CN Hors ATG dev </summary>
            public Decimal Kdmgarmht { get; set; } 
            
            ///<summary> Taxes Hors CN,ATG dev </summary>
            public Decimal Kdmgarmtx { get; set; } 
            
            ///<summary> TTC hors CN,ATG dev </summary>
            public Decimal Kdmgarmtt { get; set; } 
            
            ///<summary> HT hors Frais Calculé dev </summary>
            public Decimal Kdmhfmht { get; set; } 
            
            ///<summary> Flag HT hors Frais forcé  O/N </summary>
            public string Kdmhfflag { get; set; } 
            
            ///<summary> HT hors Frais Forcé dev </summary>
            public Decimal Kdmhfmhf { get; set; } 
            
            ///<summary> Taxes Hors Frais dev </summary>
            public Decimal Kdmhfmtx { get; set; } 
            
            ///<summary> TTC Hors Frais dev </summary>
            public Decimal Kdmhfmtt { get; set; } 
            
            ///<summary> Frais dans Barême O/N </summary>
            public string Kdmafrb { get; set; } 
            
            ///<summary> Frais HT dev </summary>
            public Decimal Kdmafr { get; set; } 
            
            ///<summary> Frais HT cpt </summary>
            public Decimal Kdmkfa { get; set; } 
            
            ///<summary> Frais taxes dev </summary>
            public Decimal Kdmaft { get; set; } 
            
            ///<summary> Frais Taxes cpt </summary>
            public Decimal Kdmkft { get; set; } 
            
            ///<summary> FGA Montant dev </summary>
            public Decimal Kdmfga { get; set; } 
            
            ///<summary> FGA Montant cpt </summary>
            public Decimal Kdmkfg { get; set; } 
            
            ///<summary> Total HT calculé dev </summary>
            public Decimal Kdmmht { get; set; } 
            
            ///<summary> Flag Total HT forcé O/N </summary>
            public string Kdmmhflag { get; set; } 
            
            ///<summary> Total HT forcé dev </summary>
            public Decimal Kdmmhf { get; set; } 
            
            ///<summary> Total HT cpt </summary>
            public Decimal Kdmkht { get; set; } 
            
            ///<summary> Total Taxes dev </summary>
            public Decimal Kdmmtx { get; set; } 
            
            ///<summary> Total Taxes cpt </summary>
            public Decimal Kdmktx { get; set; } 
            
            ///<summary> Total TTC dev </summary>
            public Decimal Kdmmtt { get; set; } 
            
            ///<summary> Flag Total TTC forcé O/N </summary>
            public string Kdmmtflag { get; set; } 
            
            ///<summary> Total TTC forcé dev </summary>
            public Decimal Kdmttf { get; set; } 
            
            ///<summary> Total TTC cpt </summary>
            public Decimal Kdmktt { get; set; } 
            
            ///<summary> Commission dans barême O/N </summary>
            public string Kdmcob { get; set; } 
            
            ///<summary> Taux de commission </summary>
            public Decimal Kdmcom { get; set; } 
            
            ///<summary> Taux de commission forcé </summary>
            public Decimal Kdmcmf { get; set; } 
            
            ///<summary> Commissions dev </summary>
            public Decimal Kdmcot { get; set; } 
            
            ///<summary> Commission Forcée dev </summary>
            public Decimal Kdmcof { get; set; } 
            
            ///<summary> Commissions cpt </summary>
            public Decimal Kdmkco { get; set; } 
            
            ///<summary> Coefficient commercial </summary>
            public Decimal Kdmcoefc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCotis  x=this,  y=obj as KpCotis;
            if( y == default(KpCotis) ) return false;
            return (
                    x.Kdmid==y.Kdmid
                    && x.Kdmtap==y.Kdmtap
                    && x.Kdmtyp==y.Kdmtyp
                    && x.Kdmipb==y.Kdmipb
                    && x.Kdmalx==y.Kdmalx
                    && x.Kdmatgmht==y.Kdmatgmht
                    && x.Kdmatgkht==y.Kdmatgkht
                    && x.Kdmatgmtx==y.Kdmatgmtx
                    && x.Kdmatgktx==y.Kdmatgktx
                    && x.Kdmatgmtt==y.Kdmatgmtt
                    && x.Kdmatgktt==y.Kdmatgktt
                    && x.Kdmatgcot==y.Kdmatgcot
                    && x.Kdmatgkco==y.Kdmatgkco
                    && x.Kdmcnabas==y.Kdmcnabas
                    && x.Kdmcnakbs==y.Kdmcnakbs
                    && x.Kdmcnamht==y.Kdmcnamht
                    && x.Kdmcnakht==y.Kdmcnakht
                    && x.Kdmcnamtx==y.Kdmcnamtx
                    && x.Kdmcnaktx==y.Kdmcnaktx
                    && x.Kdmcnamtt==y.Kdmcnamtt
                    && x.Kdmcnaktt==y.Kdmcnaktt
                    && x.Kdmcnacob==y.Kdmcnacob
                    && x.Kdmcnacnc==y.Kdmcnacnc
                    && x.Kdmcnatxf==y.Kdmcnatxf
                    && x.Kdmcnacnm==y.Kdmcnacnm
                    && x.Kdmcnacmf==y.Kdmcnacmf
                    && x.Kdmcnakcm==y.Kdmcnakcm
                    && x.Kdmgarmht==y.Kdmgarmht
                    && x.Kdmgarmtx==y.Kdmgarmtx
                    && x.Kdmgarmtt==y.Kdmgarmtt
                    && x.Kdmhfmht==y.Kdmhfmht
                    && x.Kdmhfflag==y.Kdmhfflag
                    && x.Kdmhfmhf==y.Kdmhfmhf
                    && x.Kdmhfmtx==y.Kdmhfmtx
                    && x.Kdmhfmtt==y.Kdmhfmtt
                    && x.Kdmafrb==y.Kdmafrb
                    && x.Kdmafr==y.Kdmafr
                    && x.Kdmkfa==y.Kdmkfa
                    && x.Kdmaft==y.Kdmaft
                    && x.Kdmkft==y.Kdmkft
                    && x.Kdmfga==y.Kdmfga
                    && x.Kdmkfg==y.Kdmkfg
                    && x.Kdmmht==y.Kdmmht
                    && x.Kdmmhflag==y.Kdmmhflag
                    && x.Kdmmhf==y.Kdmmhf
                    && x.Kdmkht==y.Kdmkht
                    && x.Kdmmtx==y.Kdmmtx
                    && x.Kdmktx==y.Kdmktx
                    && x.Kdmmtt==y.Kdmmtt
                    && x.Kdmmtflag==y.Kdmmtflag
                    && x.Kdmttf==y.Kdmttf
                    && x.Kdmktt==y.Kdmktt
                    && x.Kdmcob==y.Kdmcob
                    && x.Kdmcom==y.Kdmcom
                    && x.Kdmcmf==y.Kdmcmf
                    && x.Kdmcot==y.Kdmcot
                    && x.Kdmcof==y.Kdmcof
                    && x.Kdmkco==y.Kdmkco
                    && x.Kdmcoefc==y.Kdmcoefc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kdmid.GetHashCode() ) 
                      * 23 ) + (this.Kdmtap?? "").GetHashCode()
                      * 23 ) + (this.Kdmtyp?? "").GetHashCode()
                      * 23 ) + (this.Kdmipb?? "").GetHashCode()
                      * 23 ) + (this.Kdmalx.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgmht.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgkht.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgmtx.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgktx.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgmtt.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgktt.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgcot.GetHashCode() ) 
                      * 23 ) + (this.Kdmatgkco.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnabas.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnakbs.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnamht.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnakht.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnamtx.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnaktx.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnamtt.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnaktt.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnacob?? "").GetHashCode()
                      * 23 ) + (this.Kdmcnacnc.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnatxf.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnacnm.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnacmf.GetHashCode() ) 
                      * 23 ) + (this.Kdmcnakcm.GetHashCode() ) 
                      * 23 ) + (this.Kdmgarmht.GetHashCode() ) 
                      * 23 ) + (this.Kdmgarmtx.GetHashCode() ) 
                      * 23 ) + (this.Kdmgarmtt.GetHashCode() ) 
                      * 23 ) + (this.Kdmhfmht.GetHashCode() ) 
                      * 23 ) + (this.Kdmhfflag?? "").GetHashCode()
                      * 23 ) + (this.Kdmhfmhf.GetHashCode() ) 
                      * 23 ) + (this.Kdmhfmtx.GetHashCode() ) 
                      * 23 ) + (this.Kdmhfmtt.GetHashCode() ) 
                      * 23 ) + (this.Kdmafrb?? "").GetHashCode()
                      * 23 ) + (this.Kdmafr.GetHashCode() ) 
                      * 23 ) + (this.Kdmkfa.GetHashCode() ) 
                      * 23 ) + (this.Kdmaft.GetHashCode() ) 
                      * 23 ) + (this.Kdmkft.GetHashCode() ) 
                      * 23 ) + (this.Kdmfga.GetHashCode() ) 
                      * 23 ) + (this.Kdmkfg.GetHashCode() ) 
                      * 23 ) + (this.Kdmmht.GetHashCode() ) 
                      * 23 ) + (this.Kdmmhflag?? "").GetHashCode()
                      * 23 ) + (this.Kdmmhf.GetHashCode() ) 
                      * 23 ) + (this.Kdmkht.GetHashCode() ) 
                      * 23 ) + (this.Kdmmtx.GetHashCode() ) 
                      * 23 ) + (this.Kdmktx.GetHashCode() ) 
                      * 23 ) + (this.Kdmmtt.GetHashCode() ) 
                      * 23 ) + (this.Kdmmtflag?? "").GetHashCode()
                      * 23 ) + (this.Kdmttf.GetHashCode() ) 
                      * 23 ) + (this.Kdmktt.GetHashCode() ) 
                      * 23 ) + (this.Kdmcob?? "").GetHashCode()
                      * 23 ) + (this.Kdmcom.GetHashCode() ) 
                      * 23 ) + (this.Kdmcmf.GetHashCode() ) 
                      * 23 ) + (this.Kdmcot.GetHashCode() ) 
                      * 23 ) + (this.Kdmcof.GetHashCode() ) 
                      * 23 ) + (this.Kdmkco.GetHashCode() ) 
                      * 23 ) + (this.Kdmcoefc.GetHashCode() )                    );
           }
        }
    }
}
