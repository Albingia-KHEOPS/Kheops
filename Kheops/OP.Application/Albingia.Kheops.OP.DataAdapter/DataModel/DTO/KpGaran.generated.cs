using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPGARAN
    public partial class KpGaran  {
             //HPGARAN
             //KPGARAN

            ///<summary>Public empty contructor</summary>
            public KpGaran() {}
            ///<summary>Public empty contructor</summary>
            public KpGaran(KpGaran copyFrom) 
            {
                  this.Kdeid= copyFrom.Kdeid;
                  this.Kdetyp= copyFrom.Kdetyp;
                  this.Kdeipb= copyFrom.Kdeipb;
                  this.Kdealx= copyFrom.Kdealx;
                  this.Kdeavn= copyFrom.Kdeavn;
                  this.Kdehin= copyFrom.Kdehin;
                  this.Kdefor= copyFrom.Kdefor;
                  this.Kdeopt= copyFrom.Kdeopt;
                  this.Kdekdcid= copyFrom.Kdekdcid;
                  this.Kdegaran= copyFrom.Kdegaran;
                  this.Kdeseq= copyFrom.Kdeseq;
                  this.Kdeniveau= copyFrom.Kdeniveau;
                  this.Kdesem= copyFrom.Kdesem;
                  this.Kdese1= copyFrom.Kdese1;
                  this.Kdetri= copyFrom.Kdetri;
                  this.Kdenumpres= copyFrom.Kdenumpres;
                  this.Kdeajout= copyFrom.Kdeajout;
                  this.Kdecar= copyFrom.Kdecar;
                  this.Kdenat= copyFrom.Kdenat;
                  this.Kdegan= copyFrom.Kdegan;
                  this.Kdekdfid= copyFrom.Kdekdfid;
                  this.Kdedefg= copyFrom.Kdedefg;
                  this.Kdekdhid= copyFrom.Kdekdhid;
                  this.Kdedatdeb= copyFrom.Kdedatdeb;
                  this.Kdeheudeb= copyFrom.Kdeheudeb;
                  this.Kdedatfin= copyFrom.Kdedatfin;
                  this.Kdeheufin= copyFrom.Kdeheufin;
                  this.Kdeduree= copyFrom.Kdeduree;
                  this.Kdeduruni= copyFrom.Kdeduruni;
                  this.Kdeprp= copyFrom.Kdeprp;
                  this.Kdetypemi= copyFrom.Kdetypemi;
                  this.Kdealiref= copyFrom.Kdealiref;
                  this.Kdecatnat= copyFrom.Kdecatnat;
                  this.Kdeina= copyFrom.Kdeina;
                  this.Kdetaxcod= copyFrom.Kdetaxcod;
                  this.Kdetaxrep= copyFrom.Kdetaxrep;
                  this.Kdecravn= copyFrom.Kdecravn;
                  this.Kdecru= copyFrom.Kdecru;
                  this.Kdecrd= copyFrom.Kdecrd;
                  this.Kdemajavn= copyFrom.Kdemajavn;
                  this.Kdeasvalo= copyFrom.Kdeasvalo;
                  this.Kdeasvala= copyFrom.Kdeasvala;
                  this.Kdeasvalw= copyFrom.Kdeasvalw;
                  this.Kdeasunit= copyFrom.Kdeasunit;
                  this.Kdeasbase= copyFrom.Kdeasbase;
                  this.Kdeasmod= copyFrom.Kdeasmod;
                  this.Kdeasobli= copyFrom.Kdeasobli;
                  this.Kdeinvsp= copyFrom.Kdeinvsp;
                  this.Kdeinven= copyFrom.Kdeinven;
                  this.Kdewddeb= copyFrom.Kdewddeb;
                  this.Kdewhdeb= copyFrom.Kdewhdeb;
                  this.Kdewdfin= copyFrom.Kdewdfin;
                  this.Kdewhfin= copyFrom.Kdewhfin;
                  this.Kdetcd= copyFrom.Kdetcd;
                  this.Kdemodi= copyFrom.Kdemodi;
                  this.Kdepind= copyFrom.Kdepind;
                  this.Kdepcatn= copyFrom.Kdepcatn;
                  this.Kdepref= copyFrom.Kdepref;
                  this.Kdepprp= copyFrom.Kdepprp;
                  this.Kdepemi= copyFrom.Kdepemi;
                  this.Kdeptaxc= copyFrom.Kdeptaxc;
                  this.Kdepntm= copyFrom.Kdepntm;
                  this.Kdeala= copyFrom.Kdeala;
                  this.Kdepala= copyFrom.Kdepala;
                  this.Kdealo= copyFrom.Kdealo;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdeid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdetyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdeipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdealx { get; set; } 
            
            ///<summary> N?? avenant </summary>
            public int? Kdeavn { get; set; } 
            
            ///<summary> N?? histo par avenant </summary>
            public int? Kdehin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdefor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kdeopt { get; set; } 
            
            ///<summary> Lien KPOPTD </summary>
            public Int64 Kdekdcid { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kdegaran { get; set; } 
            
            ///<summary> S??quence </summary>
            public Int64 Kdeseq { get; set; } 
            
            ///<summary> Niveau </summary>
            public int Kdeniveau { get; set; } 
            
            ///<summary> S??quence garantie maitre </summary>
            public Int64 Kdesem { get; set; } 
            
            ///<summary> S??quence Niveau 1 </summary>
            public Int64 Kdese1 { get; set; } 
            
            ///<summary> Tri </summary>
            public string Kdetri { get; set; } 
            
            ///<summary> N?? de pr??sentation </summary>
            public Decimal Kdenumpres { get; set; } 
            
            ///<summary> Garantie Ajout??e O/N </summary>
            public string Kdeajout { get; set; } 
            
            ///<summary> Caract??re (Base Obligatoire ...) </summary>
            public string Kdecar { get; set; } 
            
            ///<summary> Nature param??trage </summary>
            public string Kdenat { get; set; } 
            
            ///<summary> Nature retenue </summary>
            public string Kdegan { get; set; } 
            
            ///<summary> Lien KPGARAP </summary>
            public Int64 Kdekdfid { get; set; } 
            
            ///<summary> D??finition garantie (Maintenance .. </summary>
            public string Kdedefg { get; set; } 
            
            ///<summary> Lien KPSPEC </summary>
            public Int64 Kdekdhid { get; set; } 
            
            ///<summary> Date d??but </summary>
            public int Kdedatdeb { get; set; } 
            
            ///<summary> Heure d??but </summary>
            public int Kdeheudeb { get; set; } 
            
            ///<summary> Fin de garantie Date </summary>
            public int Kdedatfin { get; set; } 
            
            ///<summary> Heure Fin </summary>
            public int Kdeheufin { get; set; } 
            
            ///<summary> Dur??e </summary>
            public int Kdeduree { get; set; } 
            
            ///<summary> Dur??e Unit?? </summary>
            public string Kdeduruni { get; set; } 
            
            ///<summary> Type Application </summary>
            public string Kdeprp { get; set; } 
            
            ///<summary> Type ??mission </summary>
            public string Kdetypemi { get; set; } 
            
            ///<summary> Alimentation mnt R??f??rence O/N </summary>
            public string Kdealiref { get; set; } 
            
            ///<summary> Application CATNAT </summary>
            public string Kdecatnat { get; set; } 
            
            ///<summary> Index??e O/N </summary>
            public string Kdeina { get; set; } 
            
            ///<summary> Code taxe </summary>
            public string Kdetaxcod { get; set; } 
            
            ///<summary> R??partition taxe </summary>
            public Decimal Kdetaxrep { get; set; } 
            
            ///<summary> Avenant de cr??ation </summary>
            public int Kdecravn { get; set; } 
            
            ///<summary> Cr??ation User </summary>
            public string Kdecru { get; set; } 
            
            ///<summary> Cr??ation Date </summary>
            public int Kdecrd { get; set; } 
            
            ///<summary> Avenant de mise ?? jour </summary>
            public int Kdemajavn { get; set; } 
            
            ///<summary> Assiette Valeur Origine </summary>
            public Decimal Kdeasvalo { get; set; } 
            
            ///<summary> Assiette Valeur actualis??e </summary>
            public Decimal Kdeasvala { get; set; } 
            
            ///<summary> Assiette Valeur de travail </summary>
            public Decimal Kdeasvalw { get; set; } 
            
            ///<summary> Assiette Unit?? </summary>
            public string Kdeasunit { get; set; } 
            
            ///<summary> Assiette Base </summary>
            public string Kdeasbase { get; set; } 
            
            ///<summary> Assiette Modifiable O/N </summary>
            public string Kdeasmod { get; set; } 
            
            ///<summary> Assiette Obligatoire </summary>
            public string Kdeasobli { get; set; } 
            
            ///<summary> Inventaire sp??cifique </summary>
            public string Kdeinvsp { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kdeinven { get; set; } 
            
            ///<summary> Date standard D??but </summary>
            public int Kdewddeb { get; set; } 
            
            ///<summary> Heure standard d??but </summary>
            public int Kdewhdeb { get; set; } 
            
            ///<summary> Date standard Fin </summary>
            public int Kdewdfin { get; set; } 
            
            ///<summary> Heure Standard Fin </summary>
            public int Kdewhfin { get; set; } 
            
            ///<summary> Type de contr??le date </summary>
            public string Kdetcd { get; set; } 
            
            ///<summary> Flag Modifi??  O/N </summary>
            public string Kdemodi { get; set; } 
            
            ///<summary> Param??trage Indexation O/N </summary>
            public string Kdepind { get; set; } 
            
            ///<summary> Param??trage CATNAT </summary>
            public string Kdepcatn { get; set; } 
            
            ///<summary> Param??trage Mnt Ref </summary>
            public string Kdepref { get; set; } 
            
            ///<summary> Param??trage Application </summary>
            public string Kdepprp { get; set; } 
            
            ///<summary> Param??trage Type ??mission </summary>
            public string Kdepemi { get; set; } 
            
            ///<summary> Param??trage Code Taxe </summary>
            public string Kdeptaxc { get; set; } 
            
            ///<summary> Param??trage Nature modifiable </summary>
            public string Kdepntm { get; set; } 
            
            ///<summary> Type Alimentation </summary>
            public string Kdeala { get; set; } 
            
            ///<summary> Param??trage Type Alimentation </summary>
            public string Kdepala { get; set; } 
            
            ///<summary> Alimentation Origine ' ' / Automatiq </summary>
            public string Kdealo { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpGaran  x=this,  y=obj as KpGaran;
            if( y == default(KpGaran) ) return false;
            return (
                    x.Kdeid==y.Kdeid
                    && x.Kdetyp==y.Kdetyp
                    && x.Kdeipb==y.Kdeipb
                    && x.Kdealx==y.Kdealx
                    && x.Kdefor==y.Kdefor
                    && x.Kdeopt==y.Kdeopt
                    && x.Kdekdcid==y.Kdekdcid
                    && x.Kdegaran==y.Kdegaran
                    && x.Kdeseq==y.Kdeseq
                    && x.Kdeniveau==y.Kdeniveau
                    && x.Kdesem==y.Kdesem
                    && x.Kdese1==y.Kdese1
                    && x.Kdetri==y.Kdetri
                    && x.Kdenumpres==y.Kdenumpres
                    && x.Kdeajout==y.Kdeajout
                    && x.Kdecar==y.Kdecar
                    && x.Kdenat==y.Kdenat
                    && x.Kdegan==y.Kdegan
                    && x.Kdekdfid==y.Kdekdfid
                    && x.Kdedefg==y.Kdedefg
                    && x.Kdekdhid==y.Kdekdhid
                    && x.Kdedatdeb==y.Kdedatdeb
                    && x.Kdeheudeb==y.Kdeheudeb
                    && x.Kdedatfin==y.Kdedatfin
                    && x.Kdeheufin==y.Kdeheufin
                    && x.Kdeduree==y.Kdeduree
                    && x.Kdeduruni==y.Kdeduruni
                    && x.Kdeprp==y.Kdeprp
                    && x.Kdetypemi==y.Kdetypemi
                    && x.Kdealiref==y.Kdealiref
                    && x.Kdecatnat==y.Kdecatnat
                    && x.Kdeina==y.Kdeina
                    && x.Kdetaxcod==y.Kdetaxcod
                    && x.Kdetaxrep==y.Kdetaxrep
                    && x.Kdecravn==y.Kdecravn
                    && x.Kdecru==y.Kdecru
                    && x.Kdecrd==y.Kdecrd
                    && x.Kdemajavn==y.Kdemajavn
                    && x.Kdeasvalo==y.Kdeasvalo
                    && x.Kdeasvala==y.Kdeasvala
                    && x.Kdeasvalw==y.Kdeasvalw
                    && x.Kdeasunit==y.Kdeasunit
                    && x.Kdeasbase==y.Kdeasbase
                    && x.Kdeasmod==y.Kdeasmod
                    && x.Kdeasobli==y.Kdeasobli
                    && x.Kdeinvsp==y.Kdeinvsp
                    && x.Kdeinven==y.Kdeinven
                    && x.Kdewddeb==y.Kdewddeb
                    && x.Kdewhdeb==y.Kdewhdeb
                    && x.Kdewdfin==y.Kdewdfin
                    && x.Kdewhfin==y.Kdewhfin
                    && x.Kdetcd==y.Kdetcd
                    && x.Kdemodi==y.Kdemodi
                    && x.Kdepind==y.Kdepind
                    && x.Kdepcatn==y.Kdepcatn
                    && x.Kdepref==y.Kdepref
                    && x.Kdepprp==y.Kdepprp
                    && x.Kdepemi==y.Kdepemi
                    && x.Kdeptaxc==y.Kdeptaxc
                    && x.Kdepntm==y.Kdepntm
                    && x.Kdeala==y.Kdeala
                    && x.Kdepala==y.Kdepala
                    && x.Kdealo==y.Kdealo  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kdeid.GetHashCode() ) 
                      * 23 ) + (this.Kdetyp?? "").GetHashCode()
                      * 23 ) + (this.Kdeipb?? "").GetHashCode()
                      * 23 ) + (this.Kdealx.GetHashCode() ) 
                      * 23 ) + (this.Kdefor.GetHashCode() ) 
                      * 23 ) + (this.Kdeopt.GetHashCode() ) 
                      * 23 ) + (this.Kdekdcid.GetHashCode() ) 
                      * 23 ) + (this.Kdegaran?? "").GetHashCode()
                      * 23 ) + (this.Kdeseq.GetHashCode() ) 
                      * 23 ) + (this.Kdeniveau.GetHashCode() ) 
                      * 23 ) + (this.Kdesem.GetHashCode() ) 
                      * 23 ) + (this.Kdese1.GetHashCode() ) 
                      * 23 ) + (this.Kdetri?? "").GetHashCode()
                      * 23 ) + (this.Kdenumpres.GetHashCode() ) 
                      * 23 ) + (this.Kdeajout?? "").GetHashCode()
                      * 23 ) + (this.Kdecar?? "").GetHashCode()
                      * 23 ) + (this.Kdenat?? "").GetHashCode()
                      * 23 ) + (this.Kdegan?? "").GetHashCode()
                      * 23 ) + (this.Kdekdfid.GetHashCode() ) 
                      * 23 ) + (this.Kdedefg?? "").GetHashCode()
                      * 23 ) + (this.Kdekdhid.GetHashCode() ) 
                      * 23 ) + (this.Kdedatdeb.GetHashCode() ) 
                      * 23 ) + (this.Kdeheudeb.GetHashCode() ) 
                      * 23 ) + (this.Kdedatfin.GetHashCode() ) 
                      * 23 ) + (this.Kdeheufin.GetHashCode() ) 
                      * 23 ) + (this.Kdeduree.GetHashCode() ) 
                      * 23 ) + (this.Kdeduruni?? "").GetHashCode()
                      * 23 ) + (this.Kdeprp?? "").GetHashCode()
                      * 23 ) + (this.Kdetypemi?? "").GetHashCode()
                      * 23 ) + (this.Kdealiref?? "").GetHashCode()
                      * 23 ) + (this.Kdecatnat?? "").GetHashCode()
                      * 23 ) + (this.Kdeina?? "").GetHashCode()
                      * 23 ) + (this.Kdetaxcod?? "").GetHashCode()
                      * 23 ) + (this.Kdetaxrep.GetHashCode() ) 
                      * 23 ) + (this.Kdecravn.GetHashCode() ) 
                      * 23 ) + (this.Kdecru?? "").GetHashCode()
                      * 23 ) + (this.Kdecrd.GetHashCode() ) 
                      * 23 ) + (this.Kdemajavn.GetHashCode() ) 
                      * 23 ) + (this.Kdeasvalo.GetHashCode() ) 
                      * 23 ) + (this.Kdeasvala.GetHashCode() ) 
                      * 23 ) + (this.Kdeasvalw.GetHashCode() ) 
                      * 23 ) + (this.Kdeasunit?? "").GetHashCode()
                      * 23 ) + (this.Kdeasbase?? "").GetHashCode()
                      * 23 ) + (this.Kdeasmod?? "").GetHashCode()
                      * 23 ) + (this.Kdeasobli?? "").GetHashCode()
                      * 23 ) + (this.Kdeinvsp?? "").GetHashCode()
                      * 23 ) + (this.Kdeinven.GetHashCode() ) 
                      * 23 ) + (this.Kdewddeb.GetHashCode() ) 
                      * 23 ) + (this.Kdewhdeb.GetHashCode() ) 
                      * 23 ) + (this.Kdewdfin.GetHashCode() ) 
                      * 23 ) + (this.Kdewhfin.GetHashCode() ) 
                      * 23 ) + (this.Kdetcd?? "").GetHashCode()
                      * 23 ) + (this.Kdemodi?? "").GetHashCode()
                      * 23 ) + (this.Kdepind?? "").GetHashCode()
                      * 23 ) + (this.Kdepcatn?? "").GetHashCode()
                      * 23 ) + (this.Kdepref?? "").GetHashCode()
                      * 23 ) + (this.Kdepprp?? "").GetHashCode()
                      * 23 ) + (this.Kdepemi?? "").GetHashCode()
                      * 23 ) + (this.Kdeptaxc?? "").GetHashCode()
                      * 23 ) + (this.Kdepntm?? "").GetHashCode()
                      * 23 ) + (this.Kdeala?? "").GetHashCode()
                      * 23 ) + (this.Kdepala?? "").GetHashCode()
                      * 23 ) + (this.Kdealo?? "").GetHashCode()                   );
           }
        }
    }
}
