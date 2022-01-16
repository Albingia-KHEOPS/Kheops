using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.KISMODL
    public partial class KISModl  {
             //KISMODL

            ///<summary>Public empty contructor</summary>
            public KISModl() {}
            ///<summary>Public empty contructor</summary>
            public KISModl(KISModl copyFrom) 
            {
                  this.Kgdid= copyFrom.Kgdid;
                  this.Kgdmodid= copyFrom.Kgdmodid;
                  this.Kgdnmid= copyFrom.Kgdnmid;
                  this.Kgdlib= copyFrom.Kgdlib;
                  this.Kgdnumaff= copyFrom.Kgdnumaff;
                  this.Kgdsautl= copyFrom.Kgdsautl;
                  this.Kgdmodi= copyFrom.Kgdmodi;
                  this.Kgdobli= copyFrom.Kgdobli;
                  this.Kgdsql= copyFrom.Kgdsql;
                  this.Kgdsqlord= copyFrom.Kgdsqlord;
                  this.Kgdscraffs= copyFrom.Kgdscraffs;
                  this.Kgdscrctrs= copyFrom.Kgdscrctrs;
                  this.Kgdparenid= copyFrom.Kgdparenid;
                  this.Kgdparenc= copyFrom.Kgdparenc;
                  this.Kgdparene= copyFrom.Kgdparene;
                  this.Kgdcru= copyFrom.Kgdcru;
                  this.Kgdcrd= copyFrom.Kgdcrd;
                  this.Kgdmju= copyFrom.Kgdmju;
                  this.Kgdmjd= copyFrom.Kgdmjd;
                  this.Kgdpres= copyFrom.Kgdpres;
                  this.Kgdsaid2= copyFrom.Kgdsaid2;
                  this.Kgdscid2= copyFrom.Kgdscid2;
                  this.Kgdnref= copyFrom.Kgdnref;
                  this.Kgdvucon= copyFrom.Kgdvucon;
                  this.Kgdvufam= copyFrom.Kgdvufam;
        
            }        
            
            ///<summary> ID unique </summary>
            public int Kgdid { get; set; } 
            
            ///<summary> Modèle lien avec KISMOD </summary>
            public string Kgdmodid { get; set; } 
            
            ///<summary> Nom zone Lien avec KISREF </summary>
            public string Kgdnmid { get; set; } 
            
            ///<summary> Libellé affiché </summary>
            public string Kgdlib { get; set; } 
            
            ///<summary> N° Ordre Affichage </summary>
            public Decimal Kgdnumaff { get; set; } 
            
            ///<summary> Saut de ligne </summary>
            public int Kgdsautl { get; set; } 
            
            ///<summary> Modifiable O/N </summary>
            public string Kgdmodi { get; set; } 
            
            ///<summary> Obligatoire O/N </summary>
            public string Kgdobli { get; set; } 
            
            ///<summary> SQL </summary>
            public string Kgdsql { get; set; } 
            
            ///<summary> SQL Order </summary>
            public int Kgdsqlord { get; set; } 
            
            ///<summary> Présence script Affichage spécifique O/N </summary>
            public string Kgdscraffs { get; set; } 
            
            ///<summary> Présence Script Contrôle spécifique  O/N </summary>
            public string Kgdscrctrs { get; set; } 
            
            ///<summary> Lien Champ Parent  - Zone précédente du modèle </summary>
            public int Kgdparenid { get; set; } 
            
            ///<summary> Si parent Comportement </summary>
            public string Kgdparenc { get; set; } 
            
            ///<summary> Si parent Evênement </summary>
            public string Kgdparene { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kgdcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kgdcrd { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kgdmju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kgdmjd { get; set; } 
            
            ///<summary> Type de présent:1-Titre1;2-Titre2 -3:champ </summary>
            public int Kgdpres { get; set; } 
            
            ///<summary> Script affichage - lien KHTSCRIPT </summary>
            public string Kgdsaid2 { get; set; } 
            
            ///<summary> Script de contrôle - lien KHTSCRIPT </summary>
            public string Kgdscid2 { get; set; } 
            
            ///<summary> N° de référence </summary>
            public int Kgdnref { get; set; } 
            
            ///<summary> unité concept </summary>
            public string Kgdvucon { get; set; } 
            
            ///<summary> unité famille </summary>
            public string Kgdvufam { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KISModl  x=this,  y=obj as KISModl;
            if( y == default(KISModl) ) return false;
            return (
                    x.Kgdid==y.Kgdid
                    && x.Kgdmodid==y.Kgdmodid
                    && x.Kgdnmid==y.Kgdnmid
                    && x.Kgdlib==y.Kgdlib
                    && x.Kgdnumaff==y.Kgdnumaff
                    && x.Kgdsautl==y.Kgdsautl
                    && x.Kgdmodi==y.Kgdmodi
                    && x.Kgdobli==y.Kgdobli
                    && x.Kgdsql==y.Kgdsql
                    && x.Kgdsqlord==y.Kgdsqlord
                    && x.Kgdscraffs==y.Kgdscraffs
                    && x.Kgdscrctrs==y.Kgdscrctrs
                    && x.Kgdparenid==y.Kgdparenid
                    && x.Kgdparenc==y.Kgdparenc
                    && x.Kgdparene==y.Kgdparene
                    && x.Kgdcru==y.Kgdcru
                    && x.Kgdcrd==y.Kgdcrd
                    && x.Kgdmju==y.Kgdmju
                    && x.Kgdmjd==y.Kgdmjd
                    && x.Kgdpres==y.Kgdpres
                    && x.Kgdsaid2==y.Kgdsaid2
                    && x.Kgdscid2==y.Kgdscid2
                    && x.Kgdnref==y.Kgdnref
                    && x.Kgdvucon==y.Kgdvucon
                    && x.Kgdvufam==y.Kgdvufam  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((((
                       17 + (this.Kgdid.GetHashCode() ) 
                      * 23 ) + (this.Kgdmodid?? "").GetHashCode()
                      * 23 ) + (this.Kgdnmid?? "").GetHashCode()
                      * 23 ) + (this.Kgdlib?? "").GetHashCode()
                      * 23 ) + (this.Kgdnumaff.GetHashCode() ) 
                      * 23 ) + (this.Kgdsautl.GetHashCode() ) 
                      * 23 ) + (this.Kgdmodi?? "").GetHashCode()
                      * 23 ) + (this.Kgdobli?? "").GetHashCode()
                      * 23 ) + (this.Kgdsql?? "").GetHashCode()
                      * 23 ) + (this.Kgdsqlord.GetHashCode() ) 
                      * 23 ) + (this.Kgdscraffs?? "").GetHashCode()
                      * 23 ) + (this.Kgdscrctrs?? "").GetHashCode()
                      * 23 ) + (this.Kgdparenid.GetHashCode() ) 
                      * 23 ) + (this.Kgdparenc?? "").GetHashCode()
                      * 23 ) + (this.Kgdparene?? "").GetHashCode()
                      * 23 ) + (this.Kgdcru?? "").GetHashCode()
                      * 23 ) + (this.Kgdcrd.GetHashCode() ) 
                      * 23 ) + (this.Kgdmju?? "").GetHashCode()
                      * 23 ) + (this.Kgdmjd.GetHashCode() ) 
                      * 23 ) + (this.Kgdpres.GetHashCode() ) 
                      * 23 ) + (this.Kgdsaid2?? "").GetHashCode()
                      * 23 ) + (this.Kgdscid2?? "").GetHashCode()
                      * 23 ) + (this.Kgdnref.GetHashCode() ) 
                      * 23 ) + (this.Kgdvucon?? "").GetHashCode()
                      * 23 ) + (this.Kgdvufam?? "").GetHashCode()                   );
           }
        }
    }
}
