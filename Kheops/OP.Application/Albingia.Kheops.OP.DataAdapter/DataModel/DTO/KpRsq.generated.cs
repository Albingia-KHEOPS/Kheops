using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPRSQ
    public partial class KpRsq  {
             //HPRSQ
             //KPRSQ

            ///<summary>Public empty contructor</summary>
            public KpRsq() {}
            ///<summary>Public empty contructor</summary>
            public KpRsq(KpRsq copyFrom) 
            {
                  this.Kabtyp= copyFrom.Kabtyp;
                  this.Kabipb= copyFrom.Kabipb;
                  this.Kabalx= copyFrom.Kabalx;
                  this.Kabavn= copyFrom.Kabavn;
                  this.Kabhin= copyFrom.Kabhin;
                  this.Kabrsq= copyFrom.Kabrsq;
                  this.Kabcible= copyFrom.Kabcible;
                  this.Kabdesc= copyFrom.Kabdesc;
                  this.Kabdesi= copyFrom.Kabdesi;
                  this.Kabobsv= copyFrom.Kabobsv;
                  this.Kabrepval= copyFrom.Kabrepval;
                  this.Kabrepobl= copyFrom.Kabrepobl;
                  this.Kabape= copyFrom.Kabape;
                  this.Kabtre= copyFrom.Kabtre;
                  this.Kabclass= copyFrom.Kabclass;
                  this.Kabnmc01= copyFrom.Kabnmc01;
                  this.Kabnmc02= copyFrom.Kabnmc02;
                  this.Kabnmc03= copyFrom.Kabnmc03;
                  this.Kabnmc04= copyFrom.Kabnmc04;
                  this.Kabnmc05= copyFrom.Kabnmc05;
                  this.Kabmand= copyFrom.Kabmand;
                  this.Kabmanf= copyFrom.Kabmanf;
                  this.Kabdspp= copyFrom.Kabdspp;
                  this.Kablcivalo= copyFrom.Kablcivalo;
                  this.Kablcivala= copyFrom.Kablcivala;
                  this.Kablcivalw= copyFrom.Kablcivalw;
                  this.Kablciunit= copyFrom.Kablciunit;
                  this.Kablcibase= copyFrom.Kablcibase;
                  this.Kabkdiid= copyFrom.Kabkdiid;
                  this.Kabfrhvalo= copyFrom.Kabfrhvalo;
                  this.Kabfrhvala= copyFrom.Kabfrhvala;
                  this.Kabfrhvalw= copyFrom.Kabfrhvalw;
                  this.Kabfrhunit= copyFrom.Kabfrhunit;
                  this.Kabfrhbase= copyFrom.Kabfrhbase;
                  this.Kabkdkid= copyFrom.Kabkdkid;
                  this.Kabnsir= copyFrom.Kabnsir;
                  this.Kabmandh= copyFrom.Kabmandh;
                  this.Kabmanfh= copyFrom.Kabmanfh;
                  this.Kabsurf= copyFrom.Kabsurf;
                  this.Kabvmc= copyFrom.Kabvmc;
                  this.Kabprol= copyFrom.Kabprol;
                  this.Kabpbi= copyFrom.Kabpbi;
                  this.Kabbrnt= copyFrom.Kabbrnt;
                  this.Kabbrnc= copyFrom.Kabbrnc;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kabtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kabipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kabalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kabavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kabhin { get; set; } 
            
            ///<summary> N° Risque </summary>
            public int Kabrsq { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kabcible { get; set; } 
            
            ///<summary> Description </summary>
            public string Kabdesc { get; set; } 
            
            ///<summary> Lien KPDESI </summary>
            public Int64 Kabdesi { get; set; } 
            
            ///<summary> Lien KPOBSV </summary>
            public Int64 Kabobsv { get; set; } 
            
            ///<summary> Report Valeur O/N </summary>
            public string Kabrepval { get; set; } 
            
            ///<summary> Report obligatoire O/N </summary>
            public string Kabrepobl { get; set; } 
            
            ///<summary> Code APE </summary>
            public string Kabape { get; set; } 
            
            ///<summary> Code TRE ou Activité </summary>
            public string Kabtre { get; set; } 
            
            ///<summary> Code Classe </summary>
            public string Kabclass { get; set; } 
            
            ///<summary> Nomenclature 01 </summary>
            public string Kabnmc01 { get; set; } 
            
            ///<summary> Nomenclature 02 </summary>
            public string Kabnmc02 { get; set; } 
            
            ///<summary> Nomenclature 03 </summary>
            public string Kabnmc03 { get; set; } 
            
            ///<summary> Nomenclature 04 </summary>
            public string Kabnmc04 { get; set; } 
            
            ///<summary> Nomenclature 05 </summary>
            public string Kabnmc05 { get; set; } 
            
            ///<summary> Manif Date début </summary>
            public int Kabmand { get; set; } 
            
            ///<summary> Manif Date fin </summary>
            public int Kabmanf { get; set; } 
            
            ///<summary> Dispo particulère (KPDESI) </summary>
            public Int64 Kabdspp { get; set; } 
            
            ///<summary> LCI Valeur Origine </summary>
            public Decimal Kablcivalo { get; set; } 
            
            ///<summary> LCI Valeur Actualisée </summary>
            public Decimal Kablcivala { get; set; } 
            
            ///<summary> LCI Valeur de travail </summary>
            public Decimal Kablcivalw { get; set; } 
            
            ///<summary> LCI Unité </summary>
            public string Kablciunit { get; set; } 
            
            ///<summary> LCI Base (type de valeur) </summary>
            public string Kablcibase { get; set; } 
            
            ///<summary> Lien KPEXPLCI </summary>
            public Int64 Kabkdiid { get; set; } 
            
            ///<summary> Franchise Valeur origine </summary>
            public Decimal Kabfrhvalo { get; set; } 
            
            ///<summary> Franchise Valeur Actualisée </summary>
            public Decimal Kabfrhvala { get; set; } 
            
            ///<summary> Franchise Valeur travail </summary>
            public Decimal Kabfrhvalw { get; set; } 
            
            ///<summary> Franchise Unité </summary>
            public string Kabfrhunit { get; set; } 
            
            ///<summary> Franchise Base </summary>
            public string Kabfrhbase { get; set; } 
            
            ///<summary> Lien KPEXPFRH </summary>
            public Int64 Kabkdkid { get; set; } 
            
            ///<summary> N° SIRET  (9+5) </summary>
            public string Kabnsir { get; set; } 
            
            ///<summary> Manif Heure de début </summary>
            public int Kabmandh { get; set; } 
            
            ///<summary> Manif Heure de Fin </summary>
            public int Kabmanfh { get; set; } 
            
            ///<summary> Superficie                    FILLER </summary>
            public Decimal Kabsurf { get; set; } 
            
            ///<summary> Valeur au m²                  FILLER </summary>
            public Decimal Kabvmc { get; set; } 
            
            ///<summary> Profession libérale O/N (régTxFILLER </summary>
            public string Kabprol { get; set; } 
            
            ///<summary> PB/BNS Anticipé O/N </summary>
            public string Kabpbi { get; set; } 
            
            ///<summary> BURNER - taux maxi </summary>
            public Decimal Kabbrnt { get; set; } 
            
            ///<summary> BURNER - cotis maxi </summary>
            public Decimal Kabbrnc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpRsq  x=this,  y=obj as KpRsq;
            if( y == default(KpRsq) ) return false;
            return (
                    x.Kabtyp==y.Kabtyp
                    && x.Kabipb==y.Kabipb
                    && x.Kabalx==y.Kabalx
                    && x.Kabrsq==y.Kabrsq
                    && x.Kabcible==y.Kabcible
                    && x.Kabdesc==y.Kabdesc
                    && x.Kabdesi==y.Kabdesi
                    && x.Kabobsv==y.Kabobsv
                    && x.Kabrepval==y.Kabrepval
                    && x.Kabrepobl==y.Kabrepobl
                    && x.Kabape==y.Kabape
                    && x.Kabtre==y.Kabtre
                    && x.Kabclass==y.Kabclass
                    && x.Kabnmc01==y.Kabnmc01
                    && x.Kabnmc02==y.Kabnmc02
                    && x.Kabnmc03==y.Kabnmc03
                    && x.Kabnmc04==y.Kabnmc04
                    && x.Kabnmc05==y.Kabnmc05
                    && x.Kabmand==y.Kabmand
                    && x.Kabmanf==y.Kabmanf
                    && x.Kabdspp==y.Kabdspp
                    && x.Kablcivalo==y.Kablcivalo
                    && x.Kablcivala==y.Kablcivala
                    && x.Kablcivalw==y.Kablcivalw
                    && x.Kablciunit==y.Kablciunit
                    && x.Kablcibase==y.Kablcibase
                    && x.Kabkdiid==y.Kabkdiid
                    && x.Kabfrhvalo==y.Kabfrhvalo
                    && x.Kabfrhvala==y.Kabfrhvala
                    && x.Kabfrhvalw==y.Kabfrhvalw
                    && x.Kabfrhunit==y.Kabfrhunit
                    && x.Kabfrhbase==y.Kabfrhbase
                    && x.Kabkdkid==y.Kabkdkid
                    && x.Kabnsir==y.Kabnsir
                    && x.Kabmandh==y.Kabmandh
                    && x.Kabmanfh==y.Kabmanfh
                    && x.Kabsurf==y.Kabsurf
                    && x.Kabvmc==y.Kabvmc
                    && x.Kabprol==y.Kabprol
                    && x.Kabpbi==y.Kabpbi
                    && x.Kabbrnt==y.Kabbrnt
                    && x.Kabbrnc==y.Kabbrnc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kabtyp?? "").GetHashCode()
                      * 23 ) + (this.Kabipb?? "").GetHashCode()
                      * 23 ) + (this.Kabalx.GetHashCode() ) 
                      * 23 ) + (this.Kabrsq.GetHashCode() ) 
                      * 23 ) + (this.Kabcible?? "").GetHashCode()
                      * 23 ) + (this.Kabdesc?? "").GetHashCode()
                      * 23 ) + (this.Kabdesi.GetHashCode() ) 
                      * 23 ) + (this.Kabobsv.GetHashCode() ) 
                      * 23 ) + (this.Kabrepval?? "").GetHashCode()
                      * 23 ) + (this.Kabrepobl?? "").GetHashCode()
                      * 23 ) + (this.Kabape?? "").GetHashCode()
                      * 23 ) + (this.Kabtre?? "").GetHashCode()
                      * 23 ) + (this.Kabclass?? "").GetHashCode()
                      * 23 ) + (this.Kabnmc01?? "").GetHashCode()
                      * 23 ) + (this.Kabnmc02?? "").GetHashCode()
                      * 23 ) + (this.Kabnmc03?? "").GetHashCode()
                      * 23 ) + (this.Kabnmc04?? "").GetHashCode()
                      * 23 ) + (this.Kabnmc05?? "").GetHashCode()
                      * 23 ) + (this.Kabmand.GetHashCode() ) 
                      * 23 ) + (this.Kabmanf.GetHashCode() ) 
                      * 23 ) + (this.Kabdspp.GetHashCode() ) 
                      * 23 ) + (this.Kablcivalo.GetHashCode() ) 
                      * 23 ) + (this.Kablcivala.GetHashCode() ) 
                      * 23 ) + (this.Kablcivalw.GetHashCode() ) 
                      * 23 ) + (this.Kablciunit?? "").GetHashCode()
                      * 23 ) + (this.Kablcibase?? "").GetHashCode()
                      * 23 ) + (this.Kabkdiid.GetHashCode() ) 
                      * 23 ) + (this.Kabfrhvalo.GetHashCode() ) 
                      * 23 ) + (this.Kabfrhvala.GetHashCode() ) 
                      * 23 ) + (this.Kabfrhvalw.GetHashCode() ) 
                      * 23 ) + (this.Kabfrhunit?? "").GetHashCode()
                      * 23 ) + (this.Kabfrhbase?? "").GetHashCode()
                      * 23 ) + (this.Kabkdkid.GetHashCode() ) 
                      * 23 ) + (this.Kabnsir?? "").GetHashCode()
                      * 23 ) + (this.Kabmandh.GetHashCode() ) 
                      * 23 ) + (this.Kabmanfh.GetHashCode() ) 
                      * 23 ) + (this.Kabsurf.GetHashCode() ) 
                      * 23 ) + (this.Kabvmc.GetHashCode() ) 
                      * 23 ) + (this.Kabprol?? "").GetHashCode()
                      * 23 ) + (this.Kabpbi?? "").GetHashCode()
                      * 23 ) + (this.Kabbrnt.GetHashCode() ) 
                      * 23 ) + (this.Kabbrnc.GetHashCode() )                    );
           }
        }
    }
}
