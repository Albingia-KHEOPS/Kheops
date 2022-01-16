using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KVERROU
    public partial class KVerrou  {
             //KVERROU

            ///<summary>Public empty contructor</summary>
            public KVerrou() {}
            ///<summary>Public empty contructor</summary>
            public KVerrou(KVerrou copyFrom) 
            {
                  this.Kavid= copyFrom.Kavid;
                  this.Kavserv= copyFrom.Kavserv;
                  this.Kavtyp= copyFrom.Kavtyp;
                  this.Kavipb= copyFrom.Kavipb;
                  this.Kavalx= copyFrom.Kavalx;
                  this.Kavavn= copyFrom.Kavavn;
                  this.Kavsua= copyFrom.Kavsua;
                  this.Kavnum= copyFrom.Kavnum;
                  this.Kavsbr= copyFrom.Kavsbr;
                  this.Kavactg= copyFrom.Kavactg;
                  this.Kavact= copyFrom.Kavact;
                  this.Kavverr= copyFrom.Kavverr;
                  this.Kavcru= copyFrom.Kavcru;
                  this.Kavcrd= copyFrom.Kavcrd;
                  this.Kavcrh= copyFrom.Kavcrh;
                  this.Kavlib= copyFrom.Kavlib;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kavid { get; set; } 
            
            ///<summary> Service (PRODU,SINIST...) </summary>
            public string Kavserv { get; set; } 
            
            ///<summary> Typ </summary>
            public string Kavtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kavipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kavalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Kavavn { get; set; } 
            
            ///<summary> N° de sinistre : AA surv </summary>
            public int Kavsua { get; set; } 
            
            ///<summary> N° de sinistre : N° </summary>
            public int Kavnum { get; set; } 
            
            ///<summary> N° de sinistre : Sous-branche </summary>
            public string Kavsbr { get; set; } 
            
            ///<summary> Acte de gestion </summary>
            public string Kavactg { get; set; } 
            
            ///<summary> Action </summary>
            public string Kavact { get; set; } 
            
            ///<summary> Verrouillage  O/N </summary>
            public string Kavverr { get; set; } 
            
            ///<summary> User </summary>
            public string Kavcru { get; set; } 
            
            ///<summary> Date </summary>
            public int Kavcrd { get; set; } 
            
            ///<summary> Heure </summary>
            public int Kavcrh { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Kavlib { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KVerrou  x=this,  y=obj as KVerrou;
            if( y == default(KVerrou) ) return false;
            return (
                    x.Kavid==y.Kavid
                    && x.Kavserv==y.Kavserv
                    && x.Kavtyp==y.Kavtyp
                    && x.Kavipb==y.Kavipb
                    && x.Kavalx==y.Kavalx
                    && x.Kavavn==y.Kavavn
                    && x.Kavsua==y.Kavsua
                    && x.Kavnum==y.Kavnum
                    && x.Kavsbr==y.Kavsbr
                    && x.Kavactg==y.Kavactg
                    && x.Kavact==y.Kavact
                    && x.Kavverr==y.Kavverr
                    && x.Kavcru==y.Kavcru
                    && x.Kavcrd==y.Kavcrd
                    && x.Kavcrh==y.Kavcrh
                    && x.Kavlib==y.Kavlib  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((
                       17 + (this.Kavid.GetHashCode() ) 
                      * 23 ) + (this.Kavserv?? "").GetHashCode()
                      * 23 ) + (this.Kavtyp?? "").GetHashCode()
                      * 23 ) + (this.Kavipb?? "").GetHashCode()
                      * 23 ) + (this.Kavalx.GetHashCode() ) 
                      * 23 ) + (this.Kavavn.GetHashCode() ) 
                      * 23 ) + (this.Kavsua.GetHashCode() ) 
                      * 23 ) + (this.Kavnum.GetHashCode() ) 
                      * 23 ) + (this.Kavsbr?? "").GetHashCode()
                      * 23 ) + (this.Kavactg?? "").GetHashCode()
                      * 23 ) + (this.Kavact?? "").GetHashCode()
                      * 23 ) + (this.Kavverr?? "").GetHashCode()
                      * 23 ) + (this.Kavcru?? "").GetHashCode()
                      * 23 ) + (this.Kavcrd.GetHashCode() ) 
                      * 23 ) + (this.Kavcrh.GetHashCode() ) 
                      * 23 ) + (this.Kavlib?? "").GetHashCode()                   );
           }
        }
    }
}
