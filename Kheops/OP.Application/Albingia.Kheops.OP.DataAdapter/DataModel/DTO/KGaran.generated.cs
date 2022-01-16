using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KGARAN
    public partial class KGaran  {
             //KGARAN

            ///<summary>Public empty contructor</summary>
            public KGaran() {}
            ///<summary>Public empty contructor</summary>
            public KGaran(KGaran copyFrom) 
            {
                  this.Gagar= copyFrom.Gagar;
                  this.Gades= copyFrom.Gades;
                  this.Gadea= copyFrom.Gadea;
                  this.Gatax= copyFrom.Gatax;
                  this.Gacnx= copyFrom.Gacnx;
                  this.Gacom= copyFrom.Gacom;
                  this.Gacar= copyFrom.Gacar;
                  this.Gadfg= copyFrom.Gadfg;
                  this.Gaifc= copyFrom.Gaifc;
                  this.Gafam= copyFrom.Gafam;
                  this.Garge= copyFrom.Garge;
                  this.Gatrg= copyFrom.Gatrg;
                  this.Gainv= copyFrom.Gainv;
                  this.Gatyi= copyFrom.Gatyi;
                  this.Garut= copyFrom.Garut;
                  this.Gasta= copyFrom.Gasta;
                  this.Gaatg= copyFrom.Gaatg;
        
            }        
            
            ///<summary> Code garantie </summary>
            public string Gagar { get; set; } 
            
            ///<summary> Désignation garantie </summary>
            public string Gades { get; set; } 
            
            ///<summary> Abrégé garantie </summary>
            public string Gadea { get; set; } 
            
            ///<summary> Code taxe </summary>
            public string Gatax { get; set; } 
            
            ///<summary> Code taxe Catastrophe Naturelle </summary>
            public string Gacnx { get; set; } 
            
            ///<summary> Garantie pouvant être commune   O/N </summary>
            public string Gacom { get; set; } 
            
            ///<summary> Caractère de la garantie </summary>
            public string Gacar { get; set; } 
            
            ///<summary> Définition garantie (Maintenance ..) </summary>
            public string Gadfg { get; set; } 
            
            ///<summary> Info complémentaire (Maintenance ..) </summary>
            public string Gaifc { get; set; } 
            
            ///<summary> Famille Garantie </summary>
            public string Gafam { get; set; } 
            
            ///<summary> Régularisable O/N </summary>
            public string Garge { get; set; } 
            
            ///<summary> Type de Grille régularisation </summary>
            public string Gatrg { get; set; } 
            
            ///<summary> Inventaire possible O/N </summary>
            public string Gainv { get; set; } 
            
            ///<summary> Type Inventaire lien KINVTYP </summary>
            public string Gatyi { get; set; } 
            
            ///<summary> Type de Régularisation </summary>
            public string Garut { get; set; } 
            
            ///<summary> Affectation statistique YPAR(KHEOP/GASTA) </summary>
            public string Gasta { get; set; } 
            
            ///<summary> Garantie soumise au GAREAT (O/N) </summary>
            public string Gaatg { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KGaran  x=this,  y=obj as KGaran;
            if( y == default(KGaran) ) return false;
            return (
                    x.Gagar==y.Gagar
                    && x.Gades==y.Gades
                    && x.Gadea==y.Gadea
                    && x.Gatax==y.Gatax
                    && x.Gacnx==y.Gacnx
                    && x.Gacom==y.Gacom
                    && x.Gacar==y.Gacar
                    && x.Gadfg==y.Gadfg
                    && x.Gaifc==y.Gaifc
                    && x.Gafam==y.Gafam
                    && x.Garge==y.Garge
                    && x.Gatrg==y.Gatrg
                    && x.Gainv==y.Gainv
                    && x.Gatyi==y.Gatyi
                    && x.Garut==y.Garut
                    && x.Gasta==y.Gasta
                    && x.Gaatg==y.Gaatg  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((
                       17 + (this.Gagar?? "").GetHashCode()
                      * 23 ) + (this.Gades?? "").GetHashCode()
                      * 23 ) + (this.Gadea?? "").GetHashCode()
                      * 23 ) + (this.Gatax?? "").GetHashCode()
                      * 23 ) + (this.Gacnx?? "").GetHashCode()
                      * 23 ) + (this.Gacom?? "").GetHashCode()
                      * 23 ) + (this.Gacar?? "").GetHashCode()
                      * 23 ) + (this.Gadfg?? "").GetHashCode()
                      * 23 ) + (this.Gaifc?? "").GetHashCode()
                      * 23 ) + (this.Gafam?? "").GetHashCode()
                      * 23 ) + (this.Garge?? "").GetHashCode()
                      * 23 ) + (this.Gatrg?? "").GetHashCode()
                      * 23 ) + (this.Gainv?? "").GetHashCode()
                      * 23 ) + (this.Gatyi?? "").GetHashCode()
                      * 23 ) + (this.Garut?? "").GetHashCode()
                      * 23 ) + (this.Gasta?? "").GetHashCode()
                      * 23 ) + (this.Gaatg?? "").GetHashCode()                   );
           }
        }
    }
}
