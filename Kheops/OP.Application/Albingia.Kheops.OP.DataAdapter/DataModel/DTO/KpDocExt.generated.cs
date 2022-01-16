using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPDOCEXT
    public partial class KpDocExt  {
             //KPDOCEXT

            ///<summary>Public empty contructor</summary>
            public KpDocExt() {}
            ///<summary>Public empty contructor</summary>
            public KpDocExt(KpDocExt copyFrom) 
            {
                  this.Kerid= copyFrom.Kerid;
                  this.Kertyp= copyFrom.Kertyp;
                  this.Keripb= copyFrom.Keripb;
                  this.Keralx= copyFrom.Keralx;
                  this.Kersua= copyFrom.Kersua;
                  this.Kernum= copyFrom.Kernum;
                  this.Kersbr= copyFrom.Kersbr;
                  this.Kerserv= copyFrom.Kerserv;
                  this.Keractg= copyFrom.Keractg;
                  this.Keravn= copyFrom.Keravn;
                  this.Kerord= copyFrom.Kerord;
                  this.Kertypo= copyFrom.Kertypo;
                  this.Kerlib= copyFrom.Kerlib;
                  this.Kernom= copyFrom.Kernom;
                  this.Kerchm= copyFrom.Kerchm;
                  this.Kerstu= copyFrom.Kerstu;
                  this.Kersit= copyFrom.Kersit;
                  this.Kerstd= copyFrom.Kerstd;
                  this.Kersth= copyFrom.Kersth;
                  this.Kercru= copyFrom.Kercru;
                  this.Kercrd= copyFrom.Kercrd;
                  this.Kercrh= copyFrom.Kercrh;
                  this.Kerref= copyFrom.Kerref;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kerid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kertyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Keripb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Keralx { get; set; } 
            
            ///<summary> Sinistre AA </summary>
            public int Kersua { get; set; } 
            
            ///<summary> Sinistre N° </summary>
            public int Kernum { get; set; } 
            
            ///<summary> Sinistre Sbr </summary>
            public string Kersbr { get; set; } 
            
            ///<summary> Service  (Produ,Sinistre...) </summary>
            public string Kerserv { get; set; } 
            
            ///<summary> Acte de gestion </summary>
            public string Keractg { get; set; } 
            
            ///<summary> N° Avenant </summary>
            public int Keravn { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public Decimal Kerord { get; set; } 
            
            ///<summary> Typologie : inventaire ..... </summary>
            public string Kertypo { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Kerlib { get; set; } 
            
            ///<summary> Nom du document </summary>
            public string Kernom { get; set; } 
            
            ///<summary> Chemin complet </summary>
            public string Kerchm { get; set; } 
            
            ///<summary> Situation User </summary>
            public string Kerstu { get; set; } 
            
            ///<summary> Situation Code </summary>
            public string Kersit { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kerstd { get; set; } 
            
            ///<summary> Situation Heure </summary>
            public int Kersth { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kercru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kercrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kercrh { get; set; } 
            
            ///<summary> Référence </summary>
            public string Kerref { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpDocExt  x=this,  y=obj as KpDocExt;
            if( y == default(KpDocExt) ) return false;
            return (
                    x.Kerid==y.Kerid
                    && x.Kertyp==y.Kertyp
                    && x.Keripb==y.Keripb
                    && x.Keralx==y.Keralx
                    && x.Kersua==y.Kersua
                    && x.Kernum==y.Kernum
                    && x.Kersbr==y.Kersbr
                    && x.Kerserv==y.Kerserv
                    && x.Keractg==y.Keractg
                    && x.Keravn==y.Keravn
                    && x.Kerord==y.Kerord
                    && x.Kertypo==y.Kertypo
                    && x.Kerlib==y.Kerlib
                    && x.Kernom==y.Kernom
                    && x.Kerchm==y.Kerchm
                    && x.Kerstu==y.Kerstu
                    && x.Kersit==y.Kersit
                    && x.Kerstd==y.Kerstd
                    && x.Kersth==y.Kersth
                    && x.Kercru==y.Kercru
                    && x.Kercrd==y.Kercrd
                    && x.Kercrh==y.Kercrh
                    && x.Kerref==y.Kerref  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((((
                       17 + (this.Kerid.GetHashCode() ) 
                      * 23 ) + (this.Kertyp?? "").GetHashCode()
                      * 23 ) + (this.Keripb?? "").GetHashCode()
                      * 23 ) + (this.Keralx.GetHashCode() ) 
                      * 23 ) + (this.Kersua.GetHashCode() ) 
                      * 23 ) + (this.Kernum.GetHashCode() ) 
                      * 23 ) + (this.Kersbr?? "").GetHashCode()
                      * 23 ) + (this.Kerserv?? "").GetHashCode()
                      * 23 ) + (this.Keractg?? "").GetHashCode()
                      * 23 ) + (this.Keravn.GetHashCode() ) 
                      * 23 ) + (this.Kerord.GetHashCode() ) 
                      * 23 ) + (this.Kertypo?? "").GetHashCode()
                      * 23 ) + (this.Kerlib?? "").GetHashCode()
                      * 23 ) + (this.Kernom?? "").GetHashCode()
                      * 23 ) + (this.Kerchm?? "").GetHashCode()
                      * 23 ) + (this.Kerstu?? "").GetHashCode()
                      * 23 ) + (this.Kersit?? "").GetHashCode()
                      * 23 ) + (this.Kerstd.GetHashCode() ) 
                      * 23 ) + (this.Kersth.GetHashCode() ) 
                      * 23 ) + (this.Kercru?? "").GetHashCode()
                      * 23 ) + (this.Kercrd.GetHashCode() ) 
                      * 23 ) + (this.Kercrh.GetHashCode() ) 
                      * 23 ) + (this.Kerref?? "").GetHashCode()                   );
           }
        }
    }
}
