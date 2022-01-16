using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YPRTPER
    public partial class YprtPer  {
             //YPRTPER

            ///<summary>Public empty contructor</summary>
            public YprtPer() {}
            ///<summary>Public empty contructor</summary>
            public YprtPer(YprtPer copyFrom) 
            {
                  this.Kaipb= copyFrom.Kaipb;
                  this.Kaalx= copyFrom.Kaalx;
                  this.Karsq= copyFrom.Karsq;
                  this.Kafor= copyFrom.Kafor;
                  this.Katyp= copyFrom.Katyp;
                  this.Kadpa= copyFrom.Kadpa;
                  this.Kadpm= copyFrom.Kadpm;
                  this.Kadpj= copyFrom.Kadpj;
                  this.Kafpa= copyFrom.Kafpa;
                  this.Kafpm= copyFrom.Kafpm;
                  this.Kapfj= copyFrom.Kapfj;
                  this.Katpe= copyFrom.Katpe;
                  this.Kaiva= copyFrom.Kaiva;
                  this.Kavaa= copyFrom.Kavaa;
                  this.Kacop= copyFrom.Kacop;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Kaipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Kaalx { get; set; } 
            
            ///<summary> Identifiant risque </summary>
            public int Karsq { get; set; } 
            
            ///<summary> Identifiant formule </summary>
            public int Kafor { get; set; } 
            
            ///<summary> Type Enregist.1 En cours 2 Proch Ech </summary>
            public int Katyp { get; set; } 
            
            ///<summary> Année Début de période </summary>
            public int Kadpa { get; set; } 
            
            ///<summary> Mois  Début de période </summary>
            public int Kadpm { get; set; } 
            
            ///<summary> Jour  Début de période </summary>
            public int Kadpj { get; set; } 
            
            ///<summary> Année Fin de période </summary>
            public int Kafpa { get; set; } 
            
            ///<summary> Mois  Fin de période </summary>
            public int Kafpm { get; set; } 
            
            ///<summary> Jour  Fin de période </summary>
            public int Kapfj { get; set; } 
            
            ///<summary> Type de période N afNouv;Terme;Ech P </summary>
            public string Katpe { get; set; } 
            
            ///<summary> Valeur de l'indice de la période </summary>
            public Decimal Kaiva { get; set; } 
            
            ///<summary> Valeur de l'assiette en cours </summary>
            public Int64 Kavaa { get; set; } 
            
            ///<summary> Période complète O/N </summary>
            public string Kacop { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YprtPer  x=this,  y=obj as YprtPer;
            if( y == default(YprtPer) ) return false;
            return (
                    x.Kaipb==y.Kaipb
                    && x.Kaalx==y.Kaalx
                    && x.Karsq==y.Karsq
                    && x.Kafor==y.Kafor
                    && x.Katyp==y.Katyp
                    && x.Kadpa==y.Kadpa
                    && x.Kadpm==y.Kadpm
                    && x.Kadpj==y.Kadpj
                    && x.Kafpa==y.Kafpa
                    && x.Kafpm==y.Kafpm
                    && x.Kapfj==y.Kapfj
                    && x.Katpe==y.Katpe
                    && x.Kaiva==y.Kaiva
                    && x.Kavaa==y.Kavaa
                    && x.Kacop==y.Kacop  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((
                       17 + (this.Kaipb?? "").GetHashCode()
                      * 23 ) + (this.Kaalx.GetHashCode() ) 
                      * 23 ) + (this.Karsq.GetHashCode() ) 
                      * 23 ) + (this.Kafor.GetHashCode() ) 
                      * 23 ) + (this.Katyp.GetHashCode() ) 
                      * 23 ) + (this.Kadpa.GetHashCode() ) 
                      * 23 ) + (this.Kadpm.GetHashCode() ) 
                      * 23 ) + (this.Kadpj.GetHashCode() ) 
                      * 23 ) + (this.Kafpa.GetHashCode() ) 
                      * 23 ) + (this.Kafpm.GetHashCode() ) 
                      * 23 ) + (this.Kapfj.GetHashCode() ) 
                      * 23 ) + (this.Katpe?? "").GetHashCode()
                      * 23 ) + (this.Kaiva.GetHashCode() ) 
                      * 23 ) + (this.Kavaa.GetHashCode() ) 
                      * 23 ) + (this.Kacop?? "").GetHashCode()                   );
           }
        }
    }
}
