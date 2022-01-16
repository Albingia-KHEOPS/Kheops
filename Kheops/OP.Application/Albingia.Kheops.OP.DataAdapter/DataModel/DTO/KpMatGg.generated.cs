using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPMATGG
    public partial class KpMatGg  {
             //HPMATGG
             //KPMATGG

            ///<summary>Public empty contructor</summary>
            public KpMatGg() {}
            ///<summary>Public empty contructor</summary>
            public KpMatGg(KpMatGg copyFrom) 
            {
                  this.Keetyp= copyFrom.Keetyp;
                  this.Keeipb= copyFrom.Keeipb;
                  this.Keealx= copyFrom.Keealx;
                  this.Keeavn= copyFrom.Keeavn;
                  this.Keehin= copyFrom.Keehin;
                  this.Keechr= copyFrom.Keechr;
                  this.Keetye= copyFrom.Keetye;
                  this.Keekdcid= copyFrom.Keekdcid;
                  this.Keevolet= copyFrom.Keevolet;
                  this.Keekakid= copyFrom.Keekakid;
                  this.Keebloc= copyFrom.Keebloc;
                  this.Keekaeid= copyFrom.Keekaeid;
                  this.Keegaran= copyFrom.Keegaran;
                  this.Keeseq= copyFrom.Keeseq;
                  this.Keeniv= copyFrom.Keeniv;
                  this.Keevid= copyFrom.Keevid;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Keetyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Keeipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Keealx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Keeavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Keehin { get; set; } 
            
            ///<summary> N° Chrono Affichage ID unique </summary>
            public int Keechr { get; set; } 
            
            ///<summary> Type enregistrement  Volet Bloc Gar </summary>
            public string Keetye { get; set; } 
            
            ///<summary> Lien KPOPTD </summary>
            public Int64 Keekdcid { get; set; } 
            
            ///<summary> Volet </summary>
            public string Keevolet { get; set; } 
            
            ///<summary> Lien KVOLET </summary>
            public Int64 Keekakid { get; set; } 
            
            ///<summary> Bloc </summary>
            public string Keebloc { get; set; } 
            
            ///<summary> Lien KBLOC </summary>
            public Int64 Keekaeid { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Keegaran { get; set; } 
            
            ///<summary> Séquence garantie </summary>
            public Int64 Keeseq { get; set; } 
            
            ///<summary> Niveau Garantie </summary>
            public int Keeniv { get; set; } 
            
            ///<summary> Non Affectée O/N </summary>
            public string Keevid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpMatGg  x=this,  y=obj as KpMatGg;
            if( y == default(KpMatGg) ) return false;
            return (
                    x.Keetyp==y.Keetyp
                    && x.Keeipb==y.Keeipb
                    && x.Keealx==y.Keealx
                    && x.Keechr==y.Keechr
                    && x.Keetye==y.Keetye
                    && x.Keekdcid==y.Keekdcid
                    && x.Keevolet==y.Keevolet
                    && x.Keekakid==y.Keekakid
                    && x.Keebloc==y.Keebloc
                    && x.Keekaeid==y.Keekaeid
                    && x.Keegaran==y.Keegaran
                    && x.Keeseq==y.Keeseq
                    && x.Keeniv==y.Keeniv
                    && x.Keevid==y.Keevid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((
                       17 + (this.Keetyp?? "").GetHashCode()
                      * 23 ) + (this.Keeipb?? "").GetHashCode()
                      * 23 ) + (this.Keealx.GetHashCode() ) 
                      * 23 ) + (this.Keechr.GetHashCode() ) 
                      * 23 ) + (this.Keetye?? "").GetHashCode()
                      * 23 ) + (this.Keekdcid.GetHashCode() ) 
                      * 23 ) + (this.Keevolet?? "").GetHashCode()
                      * 23 ) + (this.Keekakid.GetHashCode() ) 
                      * 23 ) + (this.Keebloc?? "").GetHashCode()
                      * 23 ) + (this.Keekaeid.GetHashCode() ) 
                      * 23 ) + (this.Keegaran?? "").GetHashCode()
                      * 23 ) + (this.Keeseq.GetHashCode() ) 
                      * 23 ) + (this.Keeniv.GetHashCode() ) 
                      * 23 ) + (this.Keevid?? "").GetHashCode()                   );
           }
        }
    }
}
