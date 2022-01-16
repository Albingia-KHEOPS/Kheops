using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPOBJ
    public partial class KpObj  {
             //HPOBJ
             //KPOBJ

            ///<summary>Public empty contructor</summary>
            public KpObj() {}
            ///<summary>Public empty contructor</summary>
            public KpObj(KpObj copyFrom) 
            {
                  this.Kactyp= copyFrom.Kactyp;
                  this.Kacipb= copyFrom.Kacipb;
                  this.Kacalx= copyFrom.Kacalx;
                  this.Kacavn= copyFrom.Kacavn;
                  this.Kachin= copyFrom.Kachin;
                  this.Kacrsq= copyFrom.Kacrsq;
                  this.Kacobj= copyFrom.Kacobj;
                  this.Kaccible= copyFrom.Kaccible;
                  this.Kacinven= copyFrom.Kacinven;
                  this.Kacdesc= copyFrom.Kacdesc;
                  this.Kacdesi= copyFrom.Kacdesi;
                  this.Kacobsv= copyFrom.Kacobsv;
                  this.Kacape= copyFrom.Kacape;
                  this.Kactre= copyFrom.Kactre;
                  this.Kacclass= copyFrom.Kacclass;
                  this.Kacnmc01= copyFrom.Kacnmc01;
                  this.Kacnmc02= copyFrom.Kacnmc02;
                  this.Kacnmc03= copyFrom.Kacnmc03;
                  this.Kacnmc04= copyFrom.Kacnmc04;
                  this.Kacnmc05= copyFrom.Kacnmc05;
                  this.Kacmand= copyFrom.Kacmand;
                  this.Kacmanf= copyFrom.Kacmanf;
                  this.Kacdspp= copyFrom.Kacdspp;
                  this.Kaclcivalo= copyFrom.Kaclcivalo;
                  this.Kaclcivala= copyFrom.Kaclcivala;
                  this.Kaclcivalw= copyFrom.Kaclcivalw;
                  this.Kaclciunit= copyFrom.Kaclciunit;
                  this.Kaclcibase= copyFrom.Kaclcibase;
                  this.Kackdiid= copyFrom.Kackdiid;
                  this.Kacfrhvalo= copyFrom.Kacfrhvalo;
                  this.Kacfrhvala= copyFrom.Kacfrhvala;
                  this.Kacfrhvalw= copyFrom.Kacfrhvalw;
                  this.Kacfrhunit= copyFrom.Kacfrhunit;
                  this.Kacfrhbase= copyFrom.Kacfrhbase;
                  this.Kackdkid= copyFrom.Kackdkid;
                  this.Kacnsir= copyFrom.Kacnsir;
                  this.Kacmandh= copyFrom.Kacmandh;
                  this.Kacmanfh= copyFrom.Kacmanfh;
                  this.Kacsurf= copyFrom.Kacsurf;
                  this.Kacvmc= copyFrom.Kacvmc;
                  this.Kacprol= copyFrom.Kacprol;
                  this.Kacpbi= copyFrom.Kacpbi;
                  this.Kacbrnt= copyFrom.Kacbrnt;
                  this.Kacbrnc= copyFrom.Kacbrnc;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kactyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kacipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kacalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kacavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kachin { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kacrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kacobj { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kaccible { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kacinven { get; set; } 
            
            ///<summary> Description </summary>
            public string Kacdesc { get; set; } 
            
            ///<summary> Lien KPDESI </summary>
            public Int64 Kacdesi { get; set; } 
            
            ///<summary> Lien KPOBSV </summary>
            public Int64 Kacobsv { get; set; } 
            
            ///<summary> Code APE </summary>
            public string Kacape { get; set; } 
            
            ///<summary> Code TRE ou Activité </summary>
            public string Kactre { get; set; } 
            
            ///<summary> Code Classe </summary>
            public string Kacclass { get; set; } 
            
            ///<summary> Nomenclature 01 </summary>
            public string Kacnmc01 { get; set; } 
            
            ///<summary> Nomenclature 02 </summary>
            public string Kacnmc02 { get; set; } 
            
            ///<summary> Nomenclature 03 </summary>
            public string Kacnmc03 { get; set; } 
            
            ///<summary> Nomenclature 04 </summary>
            public string Kacnmc04 { get; set; } 
            
            ///<summary> Nomenclature 05 </summary>
            public string Kacnmc05 { get; set; } 
            
            ///<summary> Date début manif </summary>
            public int Kacmand { get; set; } 
            
            ///<summary> Date fin manif </summary>
            public int Kacmanf { get; set; } 
            
            ///<summary> Lien Dispo Particulière (KPDESI) </summary>
            public Int64 Kacdspp { get; set; } 
            
            ///<summary> LCI valeur Origine </summary>
            public Decimal Kaclcivalo { get; set; } 
            
            ///<summary> LCI Valeur Actualisée </summary>
            public Decimal Kaclcivala { get; set; } 
            
            ///<summary> LCI Valeur de travail </summary>
            public Decimal Kaclcivalw { get; set; } 
            
            ///<summary> LCI Unité </summary>
            public string Kaclciunit { get; set; } 
            
            ///<summary> LCI Base (Type de valeur) </summary>
            public string Kaclcibase { get; set; } 
            
            ///<summary> Lien KPEXPLCI </summary>
            public Int64 Kackdiid { get; set; } 
            
            ///<summary> Franchise Valeur origine </summary>
            public Decimal Kacfrhvalo { get; set; } 
            
            ///<summary> Franchise Valeur Actualisée </summary>
            public Decimal Kacfrhvala { get; set; } 
            
            ///<summary> Franchise Valeur travail </summary>
            public Decimal Kacfrhvalw { get; set; } 
            
            ///<summary> Franchise Unité </summary>
            public string Kacfrhunit { get; set; } 
            
            ///<summary> Franchise Base </summary>
            public string Kacfrhbase { get; set; } 
            
            ///<summary> Lien KPEXPFRH </summary>
            public Int64 Kackdkid { get; set; } 
            
            ///<summary> N° SIRET </summary>
            public string Kacnsir { get; set; } 
            
            ///<summary> Manif Heure Début </summary>
            public int Kacmandh { get; set; } 
            
            ///<summary> Manif Heure de Fin </summary>
            public int Kacmanfh { get; set; } 
            
            ///<summary> Superficie                    FILLER </summary>
            public Decimal Kacsurf { get; set; } 
            
            ///<summary> Valeur au m²                  FILLER </summary>
            public Decimal Kacvmc { get; set; } 
            
            ///<summary> Profession libérale O/N (régTxFILLER </summary>
            public string Kacprol { get; set; } 
            
            ///<summary> PB/BNS Anticipé O/N </summary>
            public string Kacpbi { get; set; } 
            
            ///<summary> BURNER - taux maxi </summary>
            public Decimal Kacbrnt { get; set; } 
            
            ///<summary> BURNER - cotis maxi </summary>
            public Decimal Kacbrnc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpObj  x=this,  y=obj as KpObj;
            if( y == default(KpObj) ) return false;
            return (
                    x.Kactyp==y.Kactyp
                    && x.Kacipb==y.Kacipb
                    && x.Kacalx==y.Kacalx
                    && x.Kacrsq==y.Kacrsq
                    && x.Kacobj==y.Kacobj
                    && x.Kaccible==y.Kaccible
                    && x.Kacinven==y.Kacinven
                    && x.Kacdesc==y.Kacdesc
                    && x.Kacdesi==y.Kacdesi
                    && x.Kacobsv==y.Kacobsv
                    && x.Kacape==y.Kacape
                    && x.Kactre==y.Kactre
                    && x.Kacclass==y.Kacclass
                    && x.Kacnmc01==y.Kacnmc01
                    && x.Kacnmc02==y.Kacnmc02
                    && x.Kacnmc03==y.Kacnmc03
                    && x.Kacnmc04==y.Kacnmc04
                    && x.Kacnmc05==y.Kacnmc05
                    && x.Kacmand==y.Kacmand
                    && x.Kacmanf==y.Kacmanf
                    && x.Kacdspp==y.Kacdspp
                    && x.Kaclcivalo==y.Kaclcivalo
                    && x.Kaclcivala==y.Kaclcivala
                    && x.Kaclcivalw==y.Kaclcivalw
                    && x.Kaclciunit==y.Kaclciunit
                    && x.Kaclcibase==y.Kaclcibase
                    && x.Kackdiid==y.Kackdiid
                    && x.Kacfrhvalo==y.Kacfrhvalo
                    && x.Kacfrhvala==y.Kacfrhvala
                    && x.Kacfrhvalw==y.Kacfrhvalw
                    && x.Kacfrhunit==y.Kacfrhunit
                    && x.Kacfrhbase==y.Kacfrhbase
                    && x.Kackdkid==y.Kackdkid
                    && x.Kacnsir==y.Kacnsir
                    && x.Kacmandh==y.Kacmandh
                    && x.Kacmanfh==y.Kacmanfh
                    && x.Kacsurf==y.Kacsurf
                    && x.Kacvmc==y.Kacvmc
                    && x.Kacprol==y.Kacprol
                    && x.Kacpbi==y.Kacpbi
                    && x.Kacbrnt==y.Kacbrnt
                    && x.Kacbrnc==y.Kacbrnc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kactyp?? "").GetHashCode()
                      * 23 ) + (this.Kacipb?? "").GetHashCode()
                      * 23 ) + (this.Kacalx.GetHashCode() ) 
                      * 23 ) + (this.Kacrsq.GetHashCode() ) 
                      * 23 ) + (this.Kacobj.GetHashCode() ) 
                      * 23 ) + (this.Kaccible?? "").GetHashCode()
                      * 23 ) + (this.Kacinven.GetHashCode() ) 
                      * 23 ) + (this.Kacdesc?? "").GetHashCode()
                      * 23 ) + (this.Kacdesi.GetHashCode() ) 
                      * 23 ) + (this.Kacobsv.GetHashCode() ) 
                      * 23 ) + (this.Kacape?? "").GetHashCode()
                      * 23 ) + (this.Kactre?? "").GetHashCode()
                      * 23 ) + (this.Kacclass?? "").GetHashCode()
                      * 23 ) + (this.Kacnmc01?? "").GetHashCode()
                      * 23 ) + (this.Kacnmc02?? "").GetHashCode()
                      * 23 ) + (this.Kacnmc03?? "").GetHashCode()
                      * 23 ) + (this.Kacnmc04?? "").GetHashCode()
                      * 23 ) + (this.Kacnmc05?? "").GetHashCode()
                      * 23 ) + (this.Kacmand.GetHashCode() ) 
                      * 23 ) + (this.Kacmanf.GetHashCode() ) 
                      * 23 ) + (this.Kacdspp.GetHashCode() ) 
                      * 23 ) + (this.Kaclcivalo.GetHashCode() ) 
                      * 23 ) + (this.Kaclcivala.GetHashCode() ) 
                      * 23 ) + (this.Kaclcivalw.GetHashCode() ) 
                      * 23 ) + (this.Kaclciunit?? "").GetHashCode()
                      * 23 ) + (this.Kaclcibase?? "").GetHashCode()
                      * 23 ) + (this.Kackdiid.GetHashCode() ) 
                      * 23 ) + (this.Kacfrhvalo.GetHashCode() ) 
                      * 23 ) + (this.Kacfrhvala.GetHashCode() ) 
                      * 23 ) + (this.Kacfrhvalw.GetHashCode() ) 
                      * 23 ) + (this.Kacfrhunit?? "").GetHashCode()
                      * 23 ) + (this.Kacfrhbase?? "").GetHashCode()
                      * 23 ) + (this.Kackdkid.GetHashCode() ) 
                      * 23 ) + (this.Kacnsir?? "").GetHashCode()
                      * 23 ) + (this.Kacmandh.GetHashCode() ) 
                      * 23 ) + (this.Kacmanfh.GetHashCode() ) 
                      * 23 ) + (this.Kacsurf.GetHashCode() ) 
                      * 23 ) + (this.Kacvmc.GetHashCode() ) 
                      * 23 ) + (this.Kacprol?? "").GetHashCode()
                      * 23 ) + (this.Kacpbi?? "").GetHashCode()
                      * 23 ) + (this.Kacbrnt.GetHashCode() ) 
                      * 23 ) + (this.Kacbrnc.GetHashCode() )                    );
           }
        }
    }
}
