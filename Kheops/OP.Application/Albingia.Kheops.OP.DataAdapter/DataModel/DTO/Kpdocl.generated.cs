using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPDOCL
    public partial class KpDocL  {
             //KPDOCL
             //KPDOCLW

            ///<summary>Public empty contructor</summary>
            public KpDocL() {}
            ///<summary>Public empty contructor</summary>
            public KpDocL(KpDocL copyFrom) 
            {
                  this.Kelid= copyFrom.Kelid;
                  this.Keltyp= copyFrom.Keltyp;
                  this.Kelipb= copyFrom.Kelipb;
                  this.Kelalx= copyFrom.Kelalx;
                  this.Kelsua= copyFrom.Kelsua;
                  this.Kelnum= copyFrom.Kelnum;
                  this.Kelsbr= copyFrom.Kelsbr;
                  this.Kelserv= copyFrom.Kelserv;
                  this.Kelactg= copyFrom.Kelactg;
                  this.Kelactn= copyFrom.Kelactn;
                  this.Kelavn= copyFrom.Kelavn;
                  this.Kellib= copyFrom.Kellib;
                  this.Kelstu= copyFrom.Kelstu;
                  this.Kelsit= copyFrom.Kelsit;
                  this.Kelstd= copyFrom.Kelstd;
                  this.Kelsth= copyFrom.Kelsth;
                  this.Kelcru= copyFrom.Kelcru;
                  this.Kelcrd= copyFrom.Kelcrd;
                  this.Kelcrh= copyFrom.Kelcrh;
                  this.Kelmaju= copyFrom.Kelmaju;
                  this.Kelmajd= copyFrom.Kelmajd;
                  this.Kelmajh= copyFrom.Kelmajh;
                  this.Kelemi= copyFrom.Kelemi;
                  this.Kelipk= copyFrom.Kelipk;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kelid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Keltyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kelipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kelalx { get; set; } 
            
            ///<summary> Sinistre AA </summary>
            public int Kelsua { get; set; } 
            
            ///<summary> Sinistre N° </summary>
            public int Kelnum { get; set; } 
            
            ///<summary> Sinistre Sbr </summary>
            public string Kelsbr { get; set; } 
            
            ///<summary> Service  (Produ,Sinistre...) </summary>
            public string Kelserv { get; set; } 
            
            ///<summary> Acte de gestion </summary>
            public string Kelactg { get; set; } 
            
            ///<summary> N° acte de gestion </summary>
            public int Kelactn { get; set; } 
            
            ///<summary> N° Avenant </summary>
            public int Kelavn { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Kellib { get; set; } 
            
            ///<summary> Situation User </summary>
            public string Kelstu { get; set; } 
            
            ///<summary> Situation Code </summary>
            public string Kelsit { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kelstd { get; set; } 
            
            ///<summary> Situation Heure </summary>
            public int Kelsth { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kelcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kelcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kelcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kelmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kelmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kelmajh { get; set; } 
            
            ///<summary> Type prime Comptant/X sans prime/Règ </summary>
            public string Kelemi { get; set; } 
            
            ///<summary> N° de prime si emission </summary>
            public int Kelipk { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpDocL  x=this,  y=obj as KpDocL;
            if( y == default(KpDocL) ) return false;
            return (
                    x.Kelid==y.Kelid
                    && x.Keltyp==y.Keltyp
                    && x.Kelipb==y.Kelipb
                    && x.Kelalx==y.Kelalx
                    && x.Kelsua==y.Kelsua
                    && x.Kelnum==y.Kelnum
                    && x.Kelsbr==y.Kelsbr
                    && x.Kelserv==y.Kelserv
                    && x.Kelactg==y.Kelactg
                    && x.Kelactn==y.Kelactn
                    && x.Kelavn==y.Kelavn
                    && x.Kellib==y.Kellib
                    && x.Kelstu==y.Kelstu
                    && x.Kelsit==y.Kelsit
                    && x.Kelstd==y.Kelstd
                    && x.Kelsth==y.Kelsth
                    && x.Kelcru==y.Kelcru
                    && x.Kelcrd==y.Kelcrd
                    && x.Kelcrh==y.Kelcrh
                    && x.Kelmaju==y.Kelmaju
                    && x.Kelmajd==y.Kelmajd
                    && x.Kelmajh==y.Kelmajh
                    && x.Kelemi==y.Kelemi
                    && x.Kelipk==y.Kelipk  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((
                       17 + (this.Kelid.GetHashCode() ) 
                      * 23 ) + (this.Keltyp?? "").GetHashCode()
                      * 23 ) + (this.Kelipb?? "").GetHashCode()
                      * 23 ) + (this.Kelalx.GetHashCode() ) 
                      * 23 ) + (this.Kelsua.GetHashCode() ) 
                      * 23 ) + (this.Kelnum.GetHashCode() ) 
                      * 23 ) + (this.Kelsbr?? "").GetHashCode()
                      * 23 ) + (this.Kelserv?? "").GetHashCode()
                      * 23 ) + (this.Kelactg?? "").GetHashCode()
                      * 23 ) + (this.Kelactn.GetHashCode() ) 
                      * 23 ) + (this.Kelavn.GetHashCode() ) 
                      * 23 ) + (this.Kellib?? "").GetHashCode()
                      * 23 ) + (this.Kelstu?? "").GetHashCode()
                      * 23 ) + (this.Kelsit?? "").GetHashCode()
                      * 23 ) + (this.Kelstd.GetHashCode() ) 
                      * 23 ) + (this.Kelsth.GetHashCode() ) 
                      * 23 ) + (this.Kelcru?? "").GetHashCode()
                      * 23 ) + (this.Kelcrd.GetHashCode() ) 
                      * 23 ) + (this.Kelcrh.GetHashCode() ) 
                      * 23 ) + (this.Kelmaju?? "").GetHashCode()
                      * 23 ) + (this.Kelmajd.GetHashCode() ) 
                      * 23 ) + (this.Kelmajh.GetHashCode() ) 
                      * 23 ) + (this.Kelemi?? "").GetHashCode()
                      * 23 ) + (this.Kelipk.GetHashCode() )                    );
           }
        }
    }
}
