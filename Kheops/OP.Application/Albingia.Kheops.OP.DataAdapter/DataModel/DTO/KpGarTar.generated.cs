using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPGARTAR
    public partial class KpGarTar  {
             //HPGARTAR
             //KPGARTAR

            ///<summary>Public empty contructor</summary>
            public KpGarTar() {}
            ///<summary>Public empty contructor</summary>
            public KpGarTar(KpGarTar copyFrom) 
            {
                  this.Kdgid= copyFrom.Kdgid;
                  this.Kdgtyp= copyFrom.Kdgtyp;
                  this.Kdgipb= copyFrom.Kdgipb;
                  this.Kdgalx= copyFrom.Kdgalx;
                  this.Kdgavn= copyFrom.Kdgavn;
                  this.Kdghin= copyFrom.Kdghin;
                  this.Kdgfor= copyFrom.Kdgfor;
                  this.Kdgopt= copyFrom.Kdgopt;
                  this.Kdggaran= copyFrom.Kdggaran;
                  this.Kdgkdeid= copyFrom.Kdgkdeid;
                  this.Kdgnumtar= copyFrom.Kdgnumtar;
                  this.Kdglcimod= copyFrom.Kdglcimod;
                  this.Kdglciobl= copyFrom.Kdglciobl;
                  this.Kdglcivalo= copyFrom.Kdglcivalo;
                  this.Kdglcivala= copyFrom.Kdglcivala;
                  this.Kdglcivalw= copyFrom.Kdglcivalw;
                  this.Kdglciunit= copyFrom.Kdglciunit;
                  this.Kdglcibase= copyFrom.Kdglcibase;
                  this.Kdgkdiid= copyFrom.Kdgkdiid;
                  this.Kdgfrhmod= copyFrom.Kdgfrhmod;
                  this.Kdgfrhobl= copyFrom.Kdgfrhobl;
                  this.Kdgfrhvalo= copyFrom.Kdgfrhvalo;
                  this.Kdgfrhvala= copyFrom.Kdgfrhvala;
                  this.Kdgfrhvalw= copyFrom.Kdgfrhvalw;
                  this.Kdgfrhunit= copyFrom.Kdgfrhunit;
                  this.Kdgfrhbase= copyFrom.Kdgfrhbase;
                  this.Kdgkdkid= copyFrom.Kdgkdkid;
                  this.Kdgfmivalo= copyFrom.Kdgfmivalo;
                  this.Kdgfmivala= copyFrom.Kdgfmivala;
                  this.Kdgfmivalw= copyFrom.Kdgfmivalw;
                  this.Kdgfmiunit= copyFrom.Kdgfmiunit;
                  this.Kdgfmibase= copyFrom.Kdgfmibase;
                  this.Kdgfmavalo= copyFrom.Kdgfmavalo;
                  this.Kdgfmavala= copyFrom.Kdgfmavala;
                  this.Kdgfmavalw= copyFrom.Kdgfmavalw;
                  this.Kdgfmaunit= copyFrom.Kdgfmaunit;
                  this.Kdgfmabase= copyFrom.Kdgfmabase;
                  this.Kdgprimod= copyFrom.Kdgprimod;
                  this.Kdgpriobl= copyFrom.Kdgpriobl;
                  this.Kdgprivalo= copyFrom.Kdgprivalo;
                  this.Kdgprivala= copyFrom.Kdgprivala;
                  this.Kdgprivalw= copyFrom.Kdgprivalw;
                  this.Kdgpriunit= copyFrom.Kdgpriunit;
                  this.Kdgpribase= copyFrom.Kdgpribase;
                  this.Kdgmntbase= copyFrom.Kdgmntbase;
                  this.Kdgprimpro= copyFrom.Kdgprimpro;
                  this.Kdgtmc= copyFrom.Kdgtmc;
                  this.Kdgtff= copyFrom.Kdgtff;
                  this.Kdgcmc= copyFrom.Kdgcmc;
                  this.Kdgcht= copyFrom.Kdgcht;
                  this.Kdgctt= copyFrom.Kdgctt;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdgid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdgtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdgipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdgalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdgavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdghin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdgfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kdgopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kdggaran { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Kdgkdeid { get; set; } 
            
            ///<summary> Numéro TARIF </summary>
            public int Kdgnumtar { get; set; } 
            
            ///<summary> LCI Modifiable </summary>
            public string Kdglcimod { get; set; } 
            
            ///<summary> LCI obligatoire </summary>
            public string Kdglciobl { get; set; } 
            
            ///<summary> LCI valeur Origine </summary>
            public Decimal Kdglcivalo { get; set; } 
            
            ///<summary> LCI Valeur Actualisée </summary>
            public Decimal Kdglcivala { get; set; } 
            
            ///<summary> LCI Valeur Travail </summary>
            public Decimal Kdglcivalw { get; set; } 
            
            ///<summary> LCI Unité </summary>
            public string Kdglciunit { get; set; } 
            
            ///<summary> LCI Base </summary>
            public string Kdglcibase { get; set; } 
            
            ///<summary> Lien KPEXPLCI </summary>
            public Int64 Kdgkdiid { get; set; } 
            
            ///<summary> Franchise Modifiable </summary>
            public string Kdgfrhmod { get; set; } 
            
            ///<summary> Franchise Obligatoire </summary>
            public string Kdgfrhobl { get; set; } 
            
            ///<summary> Franchise Valeur Origine </summary>
            public Decimal Kdgfrhvalo { get; set; } 
            
            ///<summary> Franchise Valeur actualisée </summary>
            public Decimal Kdgfrhvala { get; set; } 
            
            ///<summary> Franchise Valeur W </summary>
            public Decimal Kdgfrhvalw { get; set; } 
            
            ///<summary> Franchise Unité </summary>
            public string Kdgfrhunit { get; set; } 
            
            ///<summary> Franchise Base </summary>
            public string Kdgfrhbase { get; set; } 
            
            ///<summary> Lien KPEXPFRH </summary>
            public Int64 Kdgkdkid { get; set; } 
            
            ///<summary> Franchise Minimum origine </summary>
            public Decimal Kdgfmivalo { get; set; } 
            
            ///<summary> Franchise Minimum valeur Actualisé </summary>
            public Decimal Kdgfmivala { get; set; } 
            
            ///<summary> Franchise Minimum Valeur travail </summary>
            public Decimal Kdgfmivalw { get; set; } 
            
            ///<summary> Franchise Minimum Unité </summary>
            public string Kdgfmiunit { get; set; } 
            
            ///<summary> Franchise minimum Base </summary>
            public string Kdgfmibase { get; set; } 
            
            ///<summary> Franchise maximum Valeur Origine </summary>
            public Decimal Kdgfmavalo { get; set; } 
            
            ///<summary> Franchise maximum Valeur actualisée </summary>
            public Decimal Kdgfmavala { get; set; } 
            
            ///<summary> Franchise Maximum Valeur de travail </summary>
            public Decimal Kdgfmavalw { get; set; } 
            
            ///<summary> Franchise Maximum Unité </summary>
            public string Kdgfmaunit { get; set; } 
            
            ///<summary> Franchise maximum Base </summary>
            public string Kdgfmabase { get; set; } 
            
            ///<summary> Prime Modifiable O/N </summary>
            public string Kdgprimod { get; set; } 
            
            ///<summary> Prime Obligatoire </summary>
            public string Kdgpriobl { get; set; } 
            
            ///<summary> Prime Valeur origine </summary>
            public Decimal Kdgprivalo { get; set; } 
            
            ///<summary> Prime Valeur Actualisée </summary>
            public Decimal Kdgprivala { get; set; } 
            
            ///<summary> Prime Valeur de travail </summary>
            public Decimal Kdgprivalw { get; set; } 
            
            ///<summary> Prime Unité </summary>
            public string Kdgpriunit { get; set; } 
            
            ///<summary> Prime Base </summary>
            public string Kdgpribase { get; set; } 
            
            ///<summary> Prime Montant de Base </summary>
            public Decimal Kdgmntbase { get; set; } 
            
            ///<summary> Prime Provisionnelle </summary>
            public Decimal Kdgprimpro { get; set; } 
            
            ///<summary> Total : Montant Calculé </summary>
            public Decimal Kdgtmc { get; set; } 
            
            ///<summary> Total Montant Forcé </summary>
            public Decimal Kdgtff { get; set; } 
            
            ///<summary> Comptant Montant calculé </summary>
            public Decimal Kdgcmc { get; set; } 
            
            ///<summary> Comptant Montant Forcé HT </summary>
            public Decimal Kdgcht { get; set; } 
            
            ///<summary> Comptant Mnt Forcé TTC </summary>
            public Decimal Kdgctt { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpGarTar  x=this,  y=obj as KpGarTar;
            if( y == default(KpGarTar) ) return false;
            return (
                    x.Kdgid==y.Kdgid
                    && x.Kdgtyp==y.Kdgtyp
                    && x.Kdgipb==y.Kdgipb
                    && x.Kdgalx==y.Kdgalx
                    && x.Kdgfor==y.Kdgfor
                    && x.Kdgopt==y.Kdgopt
                    && x.Kdggaran==y.Kdggaran
                    && x.Kdgkdeid==y.Kdgkdeid
                    && x.Kdgnumtar==y.Kdgnumtar
                    && x.Kdglcimod==y.Kdglcimod
                    && x.Kdglciobl==y.Kdglciobl
                    && x.Kdglcivalo==y.Kdglcivalo
                    && x.Kdglcivala==y.Kdglcivala
                    && x.Kdglcivalw==y.Kdglcivalw
                    && x.Kdglciunit==y.Kdglciunit
                    && x.Kdglcibase==y.Kdglcibase
                    && x.Kdgkdiid==y.Kdgkdiid
                    && x.Kdgfrhmod==y.Kdgfrhmod
                    && x.Kdgfrhobl==y.Kdgfrhobl
                    && x.Kdgfrhvalo==y.Kdgfrhvalo
                    && x.Kdgfrhvala==y.Kdgfrhvala
                    && x.Kdgfrhvalw==y.Kdgfrhvalw
                    && x.Kdgfrhunit==y.Kdgfrhunit
                    && x.Kdgfrhbase==y.Kdgfrhbase
                    && x.Kdgkdkid==y.Kdgkdkid
                    && x.Kdgfmivalo==y.Kdgfmivalo
                    && x.Kdgfmivala==y.Kdgfmivala
                    && x.Kdgfmivalw==y.Kdgfmivalw
                    && x.Kdgfmiunit==y.Kdgfmiunit
                    && x.Kdgfmibase==y.Kdgfmibase
                    && x.Kdgfmavalo==y.Kdgfmavalo
                    && x.Kdgfmavala==y.Kdgfmavala
                    && x.Kdgfmavalw==y.Kdgfmavalw
                    && x.Kdgfmaunit==y.Kdgfmaunit
                    && x.Kdgfmabase==y.Kdgfmabase
                    && x.Kdgprimod==y.Kdgprimod
                    && x.Kdgpriobl==y.Kdgpriobl
                    && x.Kdgprivalo==y.Kdgprivalo
                    && x.Kdgprivala==y.Kdgprivala
                    && x.Kdgprivalw==y.Kdgprivalw
                    && x.Kdgpriunit==y.Kdgpriunit
                    && x.Kdgpribase==y.Kdgpribase
                    && x.Kdgmntbase==y.Kdgmntbase
                    && x.Kdgprimpro==y.Kdgprimpro
                    && x.Kdgtmc==y.Kdgtmc
                    && x.Kdgtff==y.Kdgtff
                    && x.Kdgcmc==y.Kdgcmc
                    && x.Kdgcht==y.Kdgcht
                    && x.Kdgctt==y.Kdgctt  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((((((((((((((((((((((((((
                       17 + (this.Kdgid.GetHashCode() ) 
                      * 23 ) + (this.Kdgtyp?? "").GetHashCode()
                      * 23 ) + (this.Kdgipb?? "").GetHashCode()
                      * 23 ) + (this.Kdgalx.GetHashCode() ) 
                      * 23 ) + (this.Kdgfor.GetHashCode() ) 
                      * 23 ) + (this.Kdgopt.GetHashCode() ) 
                      * 23 ) + (this.Kdggaran?? "").GetHashCode()
                      * 23 ) + (this.Kdgkdeid.GetHashCode() ) 
                      * 23 ) + (this.Kdgnumtar.GetHashCode() ) 
                      * 23 ) + (this.Kdglcimod?? "").GetHashCode()
                      * 23 ) + (this.Kdglciobl?? "").GetHashCode()
                      * 23 ) + (this.Kdglcivalo.GetHashCode() ) 
                      * 23 ) + (this.Kdglcivala.GetHashCode() ) 
                      * 23 ) + (this.Kdglcivalw.GetHashCode() ) 
                      * 23 ) + (this.Kdglciunit?? "").GetHashCode()
                      * 23 ) + (this.Kdglcibase?? "").GetHashCode()
                      * 23 ) + (this.Kdgkdiid.GetHashCode() ) 
                      * 23 ) + (this.Kdgfrhmod?? "").GetHashCode()
                      * 23 ) + (this.Kdgfrhobl?? "").GetHashCode()
                      * 23 ) + (this.Kdgfrhvalo.GetHashCode() ) 
                      * 23 ) + (this.Kdgfrhvala.GetHashCode() ) 
                      * 23 ) + (this.Kdgfrhvalw.GetHashCode() ) 
                      * 23 ) + (this.Kdgfrhunit?? "").GetHashCode()
                      * 23 ) + (this.Kdgfrhbase?? "").GetHashCode()
                      * 23 ) + (this.Kdgkdkid.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmivalo.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmivala.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmivalw.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmiunit?? "").GetHashCode()
                      * 23 ) + (this.Kdgfmibase?? "").GetHashCode()
                      * 23 ) + (this.Kdgfmavalo.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmavala.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmavalw.GetHashCode() ) 
                      * 23 ) + (this.Kdgfmaunit?? "").GetHashCode()
                      * 23 ) + (this.Kdgfmabase?? "").GetHashCode()
                      * 23 ) + (this.Kdgprimod?? "").GetHashCode()
                      * 23 ) + (this.Kdgpriobl?? "").GetHashCode()
                      * 23 ) + (this.Kdgprivalo.GetHashCode() ) 
                      * 23 ) + (this.Kdgprivala.GetHashCode() ) 
                      * 23 ) + (this.Kdgprivalw.GetHashCode() ) 
                      * 23 ) + (this.Kdgpriunit?? "").GetHashCode()
                      * 23 ) + (this.Kdgpribase?? "").GetHashCode()
                      * 23 ) + (this.Kdgmntbase.GetHashCode() ) 
                      * 23 ) + (this.Kdgprimpro.GetHashCode() ) 
                      * 23 ) + (this.Kdgtmc.GetHashCode() ) 
                      * 23 ) + (this.Kdgtff.GetHashCode() ) 
                      * 23 ) + (this.Kdgcmc.GetHashCode() ) 
                      * 23 ) + (this.Kdgcht.GetHashCode() ) 
                      * 23 ) + (this.Kdgctt.GetHashCode() )                    );
           }
        }
    }
}
