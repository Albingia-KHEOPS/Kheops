using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPINVED
    public partial class KpInveD  {
             //HPINVED
             //KPINVED

            ///<summary>Public empty contructor</summary>
            public KpInveD() {}
            ///<summary>Public empty contructor</summary>
            public KpInveD(KpInveD copyFrom) 
            {
                  this.Kbfid= copyFrom.Kbfid;
                  this.Kbftyp= copyFrom.Kbftyp;
                  this.Kbfipb= copyFrom.Kbfipb;
                  this.Kbfalx= copyFrom.Kbfalx;
                  this.Kbfavn= copyFrom.Kbfavn;
                  this.Kbfhin= copyFrom.Kbfhin;
                  this.Kbfkbeid= copyFrom.Kbfkbeid;
                  this.Kbfnumlgn= copyFrom.Kbfnumlgn;
                  this.Kbfdesc= copyFrom.Kbfdesc;
                  this.Kbfkadid= copyFrom.Kbfkadid;
                  this.Kbfsite= copyFrom.Kbfsite;
                  this.Kbfntli= copyFrom.Kbfntli;
                  this.Kbfcp= copyFrom.Kbfcp;
                  this.Kbfville= copyFrom.Kbfville;
                  this.Kbfadh= copyFrom.Kbfadh;
                  this.Kbfdatdeb= copyFrom.Kbfdatdeb;
                  this.Kbfdebheu= copyFrom.Kbfdebheu;
                  this.Kbfdatfin= copyFrom.Kbfdatfin;
                  this.Kbffinheu= copyFrom.Kbffinheu;
                  this.Kbfmnt1= copyFrom.Kbfmnt1;
                  this.Kbfmnt2= copyFrom.Kbfmnt2;
                  this.Kbfnbevn= copyFrom.Kbfnbevn;
                  this.Kbfnbper= copyFrom.Kbfnbper;
                  this.Kbfnom= copyFrom.Kbfnom;
                  this.Kbfpnom= copyFrom.Kbfpnom;
                  this.Kbfdatnai= copyFrom.Kbfdatnai;
                  this.Kbffonc= copyFrom.Kbffonc;
                  this.Kbfcdec= copyFrom.Kbfcdec;
                  this.Kbfcip= copyFrom.Kbfcip;
                  this.Kbfaccs= copyFrom.Kbfaccs;
                  this.Kbfavpr= copyFrom.Kbfavpr;
                  this.Kbfmsr= copyFrom.Kbfmsr;
                  this.Kbfcmat= copyFrom.Kbfcmat;
                  this.Kbfsex= copyFrom.Kbfsex;
                  this.Kbfmdq= copyFrom.Kbfmdq;
                  this.Kbfmda= copyFrom.Kbfmda;
                  this.Kbfactp= copyFrom.Kbfactp;
                  this.Kbfkadfh= copyFrom.Kbfkadfh;
                  this.Kbfext= copyFrom.Kbfext;
                  this.Kbfmnt3= copyFrom.Kbfmnt3;
                  this.Kbfmnt4= copyFrom.Kbfmnt4;
                  this.Kbfqua= copyFrom.Kbfqua;
                  this.Kbfren= copyFrom.Kbfren;
                  this.Kbfrlo= copyFrom.Kbfrlo;
                  this.Kbfpay= copyFrom.Kbfpay;
                  this.Kbfsit2= copyFrom.Kbfsit2;
                  this.Kbfadh2= copyFrom.Kbfadh2;
                  this.Kbfsit3= copyFrom.Kbfsit3;
                  this.Kbfadh3= copyFrom.Kbfadh3;
                  this.Kbfdes2= copyFrom.Kbfdes2;
                  this.Kbfdes3= copyFrom.Kbfdes3;
                  this.Kbfdes4= copyFrom.Kbfdes4;
                  this.Kbfmrq= copyFrom.Kbfmrq;
                  this.Kbfmod= copyFrom.Kbfmod;
                  this.Kbfimm= copyFrom.Kbfimm;
                  this.Kbfmim= copyFrom.Kbfmim;
        
            }        
            
            ///<summary> ID uniqye </summary>
            public Int64 Kbfid { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Kbftyp { get; set; } 
            
            ///<summary> N° de Police / Offre </summary>
            public string Kbfipb { get; set; } 
            
            ///<summary> N° Aliment ou Connexe </summary>
            public int Kbfalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kbfavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kbfhin { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kbfkbeid { get; set; } 
            
            ///<summary> N° de ligne </summary>
            public int Kbfnumlgn { get; set; } 
            
            ///<summary> Description </summary>
            public string Kbfdesc { get; set; } 
            
            ///<summary> Lien KPDESI </summary>
            public Int64 Kbfkadid { get; set; } 
            
            ///<summary> Nom du site </summary>
            public string Kbfsite { get; set; } 
            
            ///<summary> Nature du lieu </summary>
            public string Kbfntli { get; set; } 
            
            ///<summary> CP </summary>
            public int Kbfcp { get; set; } 
            
            ///<summary> Ville </summary>
            public string Kbfville { get; set; } 
            
            ///<summary> Lien YADRESS </summary>
            public int Kbfadh { get; set; } 
            
            ///<summary> Date début </summary>
            public int Kbfdatdeb { get; set; } 
            
            ///<summary> Heure début </summary>
            public int Kbfdebheu { get; set; } 
            
            ///<summary> Date Fin </summary>
            public int Kbfdatfin { get; set; } 
            
            ///<summary> Heure Fin </summary>
            public int Kbffinheu { get; set; } 
            
            ///<summary> Montant 1 </summary>
            public Decimal Kbfmnt1 { get; set; } 
            
            ///<summary> Montant 2 </summary>
            public Decimal Kbfmnt2 { get; set; } 
            
            ///<summary> Nb Evênements </summary>
            public int Kbfnbevn { get; set; } 
            
            ///<summary> Nb personnes </summary>
            public int Kbfnbper { get; set; } 
            
            ///<summary> Nom </summary>
            public string Kbfnom { get; set; } 
            
            ///<summary> Prénom </summary>
            public string Kbfpnom { get; set; } 
            
            ///<summary> Date de Naissance </summary>
            public int Kbfdatnai { get; set; } 
            
            ///<summary> Fonction </summary>
            public string Kbffonc { get; set; } 
            
            ///<summary> Capital Décès </summary>
            public Int64 Kbfcdec { get; set; } 
            
            ///<summary> Capital IP </summary>
            public Int64 Kbfcip { get; set; } 
            
            ///<summary> Accident seul O/N </summary>
            public string Kbfaccs { get; set; } 
            
            ///<summary> Avant Prod O/N </summary>
            public string Kbfavpr { get; set; } 
            
            ///<summary> N°de série </summary>
            public string Kbfmsr { get; set; } 
            
            ///<summary> Code matériel </summary>
            public string Kbfcmat { get; set; } 
            
            ///<summary> Sexe </summary>
            public string Kbfsex { get; set; } 
            
            ///<summary> Questionnaire médical </summary>
            public string Kbfmdq { get; set; } 
            
            ///<summary> Antécédents médicaux (Lien KPDESI) </summary>
            public Int64 Kbfmda { get; set; } 
            
            ///<summary> Activités prof O/N </summary>
            public string Kbfactp { get; set; } 
            
            ///<summary> lien KPDESI pour Franchise </summary>
            public Int64 Kbfkadfh { get; set; } 
            
            ///<summary> Code Extension </summary>
            public string Kbfext { get; set; } 
            
            ///<summary> Montant 3 </summary>
            public Decimal Kbfmnt3 { get; set; } 
            
            ///<summary> Montant 4 </summary>
            public Decimal Kbfmnt4 { get; set; } 
            
            ///<summary> Qualité de l'assuré      ALSPK-QJUR </summary>
            public string Kbfqua { get; set; } 
            
            ///<summary> Renonciation à recours   ALSPK-REN </summary>
            public string Kbfren { get; set; } 
            
            ///<summary> Risque locatif           KHEOP-BFRLO </summary>
            public string Kbfrlo { get; set; } 
            
            ///<summary> Code pays </summary>
            public string Kbfpay { get; set; } 
            
            ///<summary> Site 2                   Lien KPDESI </summary>
            public Int64 Kbfsit2 { get; set; } 
            
            ///<summary> Lien YADRESS 2 </summary>
            public int Kbfadh2 { get; set; } 
            
            ///<summary> Site 3                   Lien KPDESI </summary>
            public Int64 Kbfsit3 { get; set; } 
            
            ///<summary> Lien YADRESS 3 </summary>
            public int Kbfadh3 { get; set; } 
            
            ///<summary> Dési 2                   Lien KPDESI </summary>
            public Int64 Kbfdes2 { get; set; } 
            
            ///<summary> Dési 3                   Lien KPDESI </summary>
            public Int64 Kbfdes3 { get; set; } 
            
            ///<summary> Dési 4                   Lien KPDESI </summary>
            public Int64 Kbfdes4 { get; set; } 
            
            ///<summary> Marque </summary>
            public string Kbfmrq { get; set; } 
            
            ///<summary> Modèle </summary>
            public string Kbfmod { get; set; } 
            
            ///<summary> Immatriculation </summary>
            public string Kbfimm { get; set; } 
            
            ///<summary> Immatriculation </summary>
            public string Kbfmim { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpInveD  x=this,  y=obj as KpInveD;
            if( y == default(KpInveD) ) return false;
            return (
                    x.Kbfid==y.Kbfid
                    && x.Kbftyp==y.Kbftyp
                    && x.Kbfipb==y.Kbfipb
                    && x.Kbfalx==y.Kbfalx
                    && x.Kbfkbeid==y.Kbfkbeid
                    && x.Kbfnumlgn==y.Kbfnumlgn
                    && x.Kbfdesc==y.Kbfdesc
                    && x.Kbfkadid==y.Kbfkadid
                    && x.Kbfsite==y.Kbfsite
                    && x.Kbfntli==y.Kbfntli
                    && x.Kbfcp==y.Kbfcp
                    && x.Kbfville==y.Kbfville
                    && x.Kbfadh==y.Kbfadh
                    && x.Kbfdatdeb==y.Kbfdatdeb
                    && x.Kbfdebheu==y.Kbfdebheu
                    && x.Kbfdatfin==y.Kbfdatfin
                    && x.Kbffinheu==y.Kbffinheu
                    && x.Kbfmnt1==y.Kbfmnt1
                    && x.Kbfmnt2==y.Kbfmnt2
                    && x.Kbfnbevn==y.Kbfnbevn
                    && x.Kbfnbper==y.Kbfnbper
                    && x.Kbfnom==y.Kbfnom
                    && x.Kbfpnom==y.Kbfpnom
                    && x.Kbfdatnai==y.Kbfdatnai
                    && x.Kbffonc==y.Kbffonc
                    && x.Kbfcdec==y.Kbfcdec
                    && x.Kbfcip==y.Kbfcip
                    && x.Kbfaccs==y.Kbfaccs
                    && x.Kbfavpr==y.Kbfavpr
                    && x.Kbfmsr==y.Kbfmsr
                    && x.Kbfcmat==y.Kbfcmat
                    && x.Kbfsex==y.Kbfsex
                    && x.Kbfmdq==y.Kbfmdq
                    && x.Kbfmda==y.Kbfmda
                    && x.Kbfactp==y.Kbfactp
                    && x.Kbfkadfh==y.Kbfkadfh
                    && x.Kbfext==y.Kbfext
                    && x.Kbfmnt3==y.Kbfmnt3
                    && x.Kbfmnt4==y.Kbfmnt4
                    && x.Kbfqua==y.Kbfqua
                    && x.Kbfren==y.Kbfren
                    && x.Kbfrlo==y.Kbfrlo
                    && x.Kbfpay==y.Kbfpay
                    && x.Kbfsit2==y.Kbfsit2
                    && x.Kbfadh2==y.Kbfadh2
                    && x.Kbfsit3==y.Kbfsit3
                    && x.Kbfadh3==y.Kbfadh3
                    && x.Kbfdes2==y.Kbfdes2
                    && x.Kbfdes3==y.Kbfdes3
                    && x.Kbfdes4==y.Kbfdes4
                    && x.Kbfmrq==y.Kbfmrq
                    && x.Kbfmod==y.Kbfmod  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kbfid.GetHashCode() ) 
                      * 23 ) + (this.Kbftyp?? "").GetHashCode()
                      * 23 ) + (this.Kbfipb?? "").GetHashCode()
                      * 23 ) + (this.Kbfalx.GetHashCode() ) 
                      * 23 ) + (this.Kbfkbeid.GetHashCode() ) 
                      * 23 ) + (this.Kbfnumlgn.GetHashCode() ) 
                      * 23 ) + (this.Kbfdesc?? "").GetHashCode()
                      * 23 ) + (this.Kbfkadid.GetHashCode() ) 
                      * 23 ) + (this.Kbfsite?? "").GetHashCode()
                      * 23 ) + (this.Kbfntli?? "").GetHashCode()
                      * 23 ) + (this.Kbfcp.GetHashCode() ) 
                      * 23 ) + (this.Kbfville?? "").GetHashCode()
                      * 23 ) + (this.Kbfadh.GetHashCode() ) 
                      * 23 ) + (this.Kbfdatdeb.GetHashCode() ) 
                      * 23 ) + (this.Kbfdebheu.GetHashCode() ) 
                      * 23 ) + (this.Kbfdatfin.GetHashCode() ) 
                      * 23 ) + (this.Kbffinheu.GetHashCode() ) 
                      * 23 ) + (this.Kbfmnt1.GetHashCode() ) 
                      * 23 ) + (this.Kbfmnt2.GetHashCode() ) 
                      * 23 ) + (this.Kbfnbevn.GetHashCode() ) 
                      * 23 ) + (this.Kbfnbper.GetHashCode() ) 
                      * 23 ) + (this.Kbfnom?? "").GetHashCode()
                      * 23 ) + (this.Kbfpnom?? "").GetHashCode()
                      * 23 ) + (this.Kbfdatnai.GetHashCode() ) 
                      * 23 ) + (this.Kbffonc?? "").GetHashCode()
                      * 23 ) + (this.Kbfcdec.GetHashCode() ) 
                      * 23 ) + (this.Kbfcip.GetHashCode() ) 
                      * 23 ) + (this.Kbfaccs?? "").GetHashCode()
                      * 23 ) + (this.Kbfavpr?? "").GetHashCode()
                      * 23 ) + (this.Kbfmsr?? "").GetHashCode()
                      * 23 ) + (this.Kbfcmat?? "").GetHashCode()
                      * 23 ) + (this.Kbfsex?? "").GetHashCode()
                      * 23 ) + (this.Kbfmdq?? "").GetHashCode()
                      * 23 ) + (this.Kbfmda.GetHashCode() ) 
                      * 23 ) + (this.Kbfactp?? "").GetHashCode()
                      * 23 ) + (this.Kbfkadfh.GetHashCode() ) 
                      * 23 ) + (this.Kbfext?? "").GetHashCode()
                      * 23 ) + (this.Kbfmnt3.GetHashCode() ) 
                      * 23 ) + (this.Kbfmnt4.GetHashCode() ) 
                      * 23 ) + (this.Kbfqua?? "").GetHashCode()
                      * 23 ) + (this.Kbfren?? "").GetHashCode()
                      * 23 ) + (this.Kbfrlo?? "").GetHashCode()
                      * 23 ) + (this.Kbfpay?? "").GetHashCode()
                      * 23 ) + (this.Kbfsit2.GetHashCode() ) 
                      * 23 ) + (this.Kbfadh2.GetHashCode() ) 
                      * 23 ) + (this.Kbfsit3.GetHashCode() ) 
                      * 23 ) + (this.Kbfadh3.GetHashCode() ) 
                      * 23 ) + (this.Kbfdes2.GetHashCode() ) 
                      * 23 ) + (this.Kbfdes3.GetHashCode() ) 
                      * 23 ) + (this.Kbfdes4.GetHashCode() ) 
                      * 23 ) + (this.Kbfmrq?? "").GetHashCode()
                      * 23 ) + (this.Kbfmod?? "").GetHashCode()                   );
           }
        }
    }
}
