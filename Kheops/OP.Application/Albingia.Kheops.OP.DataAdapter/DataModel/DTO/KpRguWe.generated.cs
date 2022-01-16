using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KPRGUWE
    public partial class KpRguWe  {
             //KPRGUWE

            ///<summary>Public empty contructor</summary>
            public KpRguWe() {}
            ///<summary>Public empty contructor</summary>
            public KpRguWe(KpRguWe copyFrom) 
            {
                  this.Khztyp= copyFrom.Khztyp;
                  this.Khzipb= copyFrom.Khzipb;
                  this.Khzalx= copyFrom.Khzalx;
                  this.Khzrsq= copyFrom.Khzrsq;
                  this.Khzfor= copyFrom.Khzfor;
                  this.Khzkdeid= copyFrom.Khzkdeid;
                  this.Khzgaran= copyFrom.Khzgaran;
                  this.Khzipk= copyFrom.Khzipk;
                  this.Khzmht= copyFrom.Khzmht;
                  this.Khzmtx= copyFrom.Khzmtx;
                  this.Khzaht= copyFrom.Khzaht;
        
            }        
            
            ///<summary> Type  O/P </summary>
            public string Khztyp { get; set; } 
            
            ///<summary> N° de Police </summary>
            public string Khzipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Khzalx { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Khzrsq { get; set; } 
            
            ///<summary> Formule </summary>
            public int Khzfor { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Khzkdeid { get; set; } 
            
            ///<summary> Code garantie </summary>
            public string Khzgaran { get; set; } 
            
            ///<summary> N° de prime / Police </summary>
            public int Khzipk { get; set; } 
            
            ///<summary> Montant HT (Hors CATNAT)       P.TOT </summary>
            public Decimal Khzmht { get; set; } 
            
            ///<summary> Montant de taxe (Hors CATNAT) </summary>
            public Decimal Khzmtx { get; set; } 
            
            ///<summary> Montant HT (Hors CATNAT)       P.ALB </summary>
            public Decimal Khzaht { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpRguWe  x=this,  y=obj as KpRguWe;
            if( y == default(KpRguWe) ) return false;
            return (
                    x.Khztyp==y.Khztyp
                    && x.Khzipb==y.Khzipb
                    && x.Khzalx==y.Khzalx
                    && x.Khzrsq==y.Khzrsq
                    && x.Khzfor==y.Khzfor
                    && x.Khzkdeid==y.Khzkdeid
                    && x.Khzgaran==y.Khzgaran
                    && x.Khzipk==y.Khzipk
                    && x.Khzmht==y.Khzmht
                    && x.Khzmtx==y.Khzmtx
                    && x.Khzaht==y.Khzaht  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((
                       17 + (this.Khztyp?? "").GetHashCode()
                      * 23 ) + (this.Khzipb?? "").GetHashCode()
                      * 23 ) + (this.Khzalx.GetHashCode() ) 
                      * 23 ) + (this.Khzrsq.GetHashCode() ) 
                      * 23 ) + (this.Khzfor.GetHashCode() ) 
                      * 23 ) + (this.Khzkdeid.GetHashCode() ) 
                      * 23 ) + (this.Khzgaran?? "").GetHashCode()
                      * 23 ) + (this.Khzipk.GetHashCode() ) 
                      * 23 ) + (this.Khzmht.GetHashCode() ) 
                      * 23 ) + (this.Khzmtx.GetHashCode() ) 
                      * 23 ) + (this.Khzaht.GetHashCode() )                    );
           }
        }
    }
}
