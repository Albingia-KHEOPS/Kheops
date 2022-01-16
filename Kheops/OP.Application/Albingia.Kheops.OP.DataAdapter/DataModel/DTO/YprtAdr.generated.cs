using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHRTADR
    public partial class YprtAdr  {
             //YHRTADR
             //YPRTADR

            ///<summary>Public empty contructor</summary>
            public YprtAdr() {}
            ///<summary>Public empty contructor</summary>
            public YprtAdr(YprtAdr copyFrom) 
            {
                  this.Jfipb= copyFrom.Jfipb;
                  this.Jfalx= copyFrom.Jfalx;
                  this.Jfavn= copyFrom.Jfavn;
                  this.Jhhin= copyFrom.Jhhin;
                  this.Jfrsq= copyFrom.Jfrsq;
                  this.Jfobj= copyFrom.Jfobj;
                  this.Jfad1= copyFrom.Jfad1;
                  this.Jfad2= copyFrom.Jfad2;
                  this.Jfdep= copyFrom.Jfdep;
                  this.Jfcpo= copyFrom.Jfcpo;
                  this.Jfvil= copyFrom.Jfvil;
                  this.Jfpay= copyFrom.Jfpay;
                  this.Jfadh= copyFrom.Jfadh;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Jfipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Jfalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Jfavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Jhhin { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Jfrsq { get; set; } 
            
            ///<summary> Identifiant objet </summary>
            public int Jfobj { get; set; } 
            
            ///<summary> Adresse </summary>
            public string Jfad1 { get; set; } 
            
            ///<summary> Adresse </summary>
            public string Jfad2 { get; set; } 
            
            ///<summary> Département </summary>
            public string Jfdep { get; set; } 
            
            ///<summary> 3 derniers caractères code postal </summary>
            public string Jfcpo { get; set; } 
            
            ///<summary> Ville </summary>
            public string Jfvil { get; set; } 
            
            ///<summary> Code pays </summary>
            public string Jfpay { get; set; } 
            
            ///<summary> Numéro chrono Adresse </summary>
            public int Jfadh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtAdr  x=this,  y=obj as YprtAdr;
            if( y == default(YprtAdr) ) return false;
            return (
                    x.Jfipb==y.Jfipb
                    && x.Jfalx==y.Jfalx
                    && x.Jfrsq==y.Jfrsq
                    && x.Jfobj==y.Jfobj
                    && x.Jfad1==y.Jfad1
                    && x.Jfad2==y.Jfad2
                    && x.Jfdep==y.Jfdep
                    && x.Jfcpo==y.Jfcpo
                    && x.Jfvil==y.Jfvil
                    && x.Jfpay==y.Jfpay
                    && x.Jfadh==y.Jfadh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((
                       17 + (this.Jfipb?? "").GetHashCode()
                      * 23 ) + (this.Jfalx.GetHashCode() ) 
                      * 23 ) + (this.Jfrsq.GetHashCode() ) 
                      * 23 ) + (this.Jfobj.GetHashCode() ) 
                      * 23 ) + (this.Jfad1?? "").GetHashCode()
                      * 23 ) + (this.Jfad2?? "").GetHashCode()
                      * 23 ) + (this.Jfdep?? "").GetHashCode()
                      * 23 ) + (this.Jfcpo?? "").GetHashCode()
                      * 23 ) + (this.Jfvil?? "").GetHashCode()
                      * 23 ) + (this.Jfpay?? "").GetHashCode()
                      * 23 ) + (this.Jfadh.GetHashCode() )                    );
           }
        }
    }
}
