using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPINVEN
    public partial class KpInven  {
             //HPINVEN
             //KPINVEN

            ///<summary>Public empty contructor</summary>
            public KpInven() {}
            ///<summary>Public empty contructor</summary>
            public KpInven(KpInven copyFrom) 
            {
                  this.Kbeid= copyFrom.Kbeid;
                  this.Kbetyp= copyFrom.Kbetyp;
                  this.Kbeipb= copyFrom.Kbeipb;
                  this.Kbealx= copyFrom.Kbealx;
                  this.Kbeavn= copyFrom.Kbeavn;
                  this.Kbehin= copyFrom.Kbehin;
                  this.Kbechr= copyFrom.Kbechr;
                  this.Kbedesc= copyFrom.Kbedesc;
                  this.Kbekagid= copyFrom.Kbekagid;
                  this.Kbekadid= copyFrom.Kbekadid;
                  this.Kberepval= copyFrom.Kberepval;
                  this.Kbeval= copyFrom.Kbeval;
                  this.Kbevaa= copyFrom.Kbevaa;
                  this.Kbevaw= copyFrom.Kbevaw;
                  this.Kbevat= copyFrom.Kbevat;
                  this.Kbevau= copyFrom.Kbevau;
                  this.Kbevah= copyFrom.Kbevah;
                  this.Kbeivo= copyFrom.Kbeivo;
                  this.Kbeiva= copyFrom.Kbeiva;
                  this.Kbeivw= copyFrom.Kbeivw;
        
            }        
            
            ///<summary> ID Unique </summary>
            public Int64 Kbeid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kbetyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kbeipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kbealx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kbeavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kbehin { get; set; } 
            
            ///<summary> N° Chrono </summary>
            public int Kbechr { get; set; } 
            
            ///<summary> Description </summary>
            public string Kbedesc { get; set; } 
            
            ///<summary> Lien KINVTYP </summary>
            public Int64 Kbekagid { get; set; } 
            
            ///<summary> Lien KPDESI </summary>
            public Int64 Kbekadid { get; set; } 
            
            ///<summary> Report Valeur dans Objet O/N </summary>
            public string Kberepval { get; set; } 
            
            ///<summary> Valeur d'origine </summary>
            public Decimal Kbeval { get; set; } 
            
            ///<summary> Valeur Actualisée </summary>
            public Decimal Kbevaa { get; set; } 
            
            ///<summary> Valeur de travail </summary>
            public Decimal Kbevaw { get; set; } 
            
            ///<summary> Type de valeur </summary>
            public string Kbevat { get; set; } 
            
            ///<summary> Unité de la valeur </summary>
            public string Kbevau { get; set; } 
            
            ///<summary> HT/TTC </summary>
            public string Kbevah { get; set; } 
            
            ///<summary> Valeur indice Origine </summary>
            public Decimal Kbeivo { get; set; } 
            
            ///<summary> Valeur indice Actualisée </summary>
            public Decimal Kbeiva { get; set; } 
            
            ///<summary> Valeur Indice de travail </summary>
            public Decimal Kbeivw { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpInven  x=this,  y=obj as KpInven;
            if( y == default(KpInven) ) return false;
            return (
                    x.Kbeid==y.Kbeid
                    && x.Kbetyp==y.Kbetyp
                    && x.Kbeipb==y.Kbeipb
                    && x.Kbealx==y.Kbealx
                    && x.Kbechr==y.Kbechr
                    && x.Kbedesc==y.Kbedesc
                    && x.Kbekagid==y.Kbekagid
                    && x.Kbekadid==y.Kbekadid
                    && x.Kberepval==y.Kberepval
                    && x.Kbeval==y.Kbeval
                    && x.Kbevaa==y.Kbevaa
                    && x.Kbevaw==y.Kbevaw
                    && x.Kbevat==y.Kbevat
                    && x.Kbevau==y.Kbevau
                    && x.Kbevah==y.Kbevah
                    && x.Kbeivo==y.Kbeivo
                    && x.Kbeiva==y.Kbeiva
                    && x.Kbeivw==y.Kbeivw  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((
                       17 + (this.Kbeid.GetHashCode() ) 
                      * 23 ) + (this.Kbetyp?? "").GetHashCode()
                      * 23 ) + (this.Kbeipb?? "").GetHashCode()
                      * 23 ) + (this.Kbealx.GetHashCode() ) 
                      * 23 ) + (this.Kbechr.GetHashCode() ) 
                      * 23 ) + (this.Kbedesc?? "").GetHashCode()
                      * 23 ) + (this.Kbekagid.GetHashCode() ) 
                      * 23 ) + (this.Kbekadid.GetHashCode() ) 
                      * 23 ) + (this.Kberepval?? "").GetHashCode()
                      * 23 ) + (this.Kbeval.GetHashCode() ) 
                      * 23 ) + (this.Kbevaa.GetHashCode() ) 
                      * 23 ) + (this.Kbevaw.GetHashCode() ) 
                      * 23 ) + (this.Kbevat?? "").GetHashCode()
                      * 23 ) + (this.Kbevau?? "").GetHashCode()
                      * 23 ) + (this.Kbevah?? "").GetHashCode()
                      * 23 ) + (this.Kbeivo.GetHashCode() ) 
                      * 23 ) + (this.Kbeiva.GetHashCode() ) 
                      * 23 ) + (this.Kbeivw.GetHashCode() )                    );
           }
        }
    }
}
