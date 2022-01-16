using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPASSU
    public partial class YpoAssu  {
             //YHPASSU
             //YPOASSU

            ///<summary>Public empty contructor</summary>
            public YpoAssu() {}
            ///<summary>Public empty contructor</summary>
            public YpoAssu(YpoAssu copyFrom) 
            {
                  this.Pcipb= copyFrom.Pcipb;
                  this.Pcalx= copyFrom.Pcalx;
                  this.Pcavn= copyFrom.Pcavn;
                  this.Pchin= copyFrom.Pchin;
                  this.Pcias= copyFrom.Pcias;
                  this.Pcpri= copyFrom.Pcpri;
                  this.Pcql1= copyFrom.Pcql1;
                  this.Pcql2= copyFrom.Pcql2;
                  this.Pcql3= copyFrom.Pcql3;
                  this.Pcqld= copyFrom.Pcqld;
                  this.Pccnr= copyFrom.Pccnr;
                  this.Pcass= copyFrom.Pcass;
                  this.Pcscp= copyFrom.Pcscp;
                  this.Pctyp= copyFrom.Pctyp;
                  this.Pcdesi= copyFrom.Pcdesi;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Pcipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Pcalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Pcavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Pchin { get; set; } 
            
            ///<summary> Identifiant Assuré souscript. 10/00 </summary>
            public int Pcias { get; set; } 
            
            ///<summary> Souscripteur principal (O/N) </summary>
            public string Pcpri { get; set; } 
            
            ///<summary> Qualité 1 de l'assuré </summary>
            public string Pcql1 { get; set; } 
            
            ///<summary> Qualité 2 de l'assuré </summary>
            public string Pcql2 { get; set; } 
            
            ///<summary> Qualité 3 de l'assuré </summary>
            public string Pcql3 { get; set; } 
            
            ///<summary> Qualité Autre </summary>
            public string Pcqld { get; set; } 
            
            ///<summary> CNR O/N </summary>
            public string Pccnr { get; set; } 
            
            ///<summary> Assuré O/N </summary>
            public string Pcass { get; set; } 
            
            ///<summary> Souscripteur O/N </summary>
            public string Pcscp { get; set; } 
            
            ///<summary> Type  O Offre  P Police  E à établir </summary>
            public string Pctyp { get; set; } 
            
            ///<summary> Lien vers KPDESI </summary>
            public Int64 Pcdesi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoAssu  x=this,  y=obj as YpoAssu;
            if( y == default(YpoAssu) ) return false;
            return (
                    x.Pcipb==y.Pcipb
                    && x.Pcalx==y.Pcalx
                    && x.Pcias==y.Pcias
                    && x.Pcpri==y.Pcpri
                    && x.Pcql1==y.Pcql1
                    && x.Pcql2==y.Pcql2
                    && x.Pcql3==y.Pcql3
                    && x.Pcqld==y.Pcqld
                    && x.Pccnr==y.Pccnr
                    && x.Pcass==y.Pcass
                    && x.Pcscp==y.Pcscp
                    && x.Pctyp==y.Pctyp
                    && x.Pcdesi==y.Pcdesi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((
                       17 + (this.Pcipb?? "").GetHashCode()
                      * 23 ) + (this.Pcalx.GetHashCode() ) 
                      * 23 ) + (this.Pcias.GetHashCode() ) 
                      * 23 ) + (this.Pcpri?? "").GetHashCode()
                      * 23 ) + (this.Pcql1?? "").GetHashCode()
                      * 23 ) + (this.Pcql2?? "").GetHashCode()
                      * 23 ) + (this.Pcql3?? "").GetHashCode()
                      * 23 ) + (this.Pcqld?? "").GetHashCode()
                      * 23 ) + (this.Pccnr?? "").GetHashCode()
                      * 23 ) + (this.Pcass?? "").GetHashCode()
                      * 23 ) + (this.Pcscp?? "").GetHashCode()
                      * 23 ) + (this.Pctyp?? "").GetHashCode()
                      * 23 ) + (this.Pcdesi.GetHashCode() )                    );
           }
        }
    }
}
