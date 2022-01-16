using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPCTRL
    public partial class KpCtrl  {
             //HPCTRL
             //KPCTRL

            ///<summary>Public empty contructor</summary>
            public KpCtrl() {}
            ///<summary>Public empty contructor</summary>
            public KpCtrl(KpCtrl copyFrom) 
            {
                  this.Keuid= copyFrom.Keuid;
                  this.Keutyp= copyFrom.Keutyp;
                  this.Keuipb= copyFrom.Keuipb;
                  this.Keualx= copyFrom.Keualx;
                  this.Keuavn= copyFrom.Keuavn;
                  this.Keuhin= copyFrom.Keuhin;
                  this.Keuetape= copyFrom.Keuetape;
                  this.Keuetord= copyFrom.Keuetord;
                  this.Keuordr= copyFrom.Keuordr;
                  this.Keuperi= copyFrom.Keuperi;
                  this.Keursq= copyFrom.Keursq;
                  this.Keuobj= copyFrom.Keuobj;
                  this.Keuinven= copyFrom.Keuinven;
                  this.Keuinlgn= copyFrom.Keuinlgn;
                  this.Keufor= copyFrom.Keufor;
                  this.Keuopt= copyFrom.Keuopt;
                  this.Keugar= copyFrom.Keugar;
                  this.Keumsg= copyFrom.Keumsg;
                  this.Keunivm= copyFrom.Keunivm;
                  this.Keucru= copyFrom.Keucru;
                  this.Keucrd= copyFrom.Keucrd;
                  this.Keucrh= copyFrom.Keucrh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Keuid { get; set; } 
            
            ///<summary> TYP O/P </summary>
            public string Keutyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Keuipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Keualx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Keuavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Keuhin { get; set; } 
            
            ///<summary> Etape de génération </summary>
            public string Keuetape { get; set; } 
            
            ///<summary> N° ordre Etape </summary>
            public int Keuetord { get; set; } 
            
            ///<summary> N° ordre ID </summary>
            public Int64 Keuordr { get; set; } 
            
            ///<summary> Périmètre  BASE RISQUE .... </summary>
            public string Keuperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Keursq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Keuobj { get; set; } 
            
            ///<summary> ID KPINVEN </summary>
            public Int64 Keuinven { get; set; } 
            
            ///<summary> N° de ligne inventaire </summary>
            public int Keuinlgn { get; set; } 
            
            ///<summary> Formule </summary>
            public int Keufor { get; set; } 
            
            ///<summary> Option </summary>
            public int Keuopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Keugar { get; set; } 
            
            ///<summary> Texte du message </summary>
            public string Keumsg { get; set; } 
            
            ///<summary> Niv classement Bloquant/NonVal/Avert </summary>
            public string Keunivm { get; set; } 
            
            ///<summary> Création User </summary>
            public string Keucru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Keucrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Keucrh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCtrl  x=this,  y=obj as KpCtrl;
            if( y == default(KpCtrl) ) return false;
            return (
                    x.Keuid==y.Keuid
                    && x.Keutyp==y.Keutyp
                    && x.Keuipb==y.Keuipb
                    && x.Keualx==y.Keualx
                    && x.Keuetape==y.Keuetape
                    && x.Keuetord==y.Keuetord
                    && x.Keuordr==y.Keuordr
                    && x.Keuperi==y.Keuperi
                    && x.Keursq==y.Keursq
                    && x.Keuobj==y.Keuobj
                    && x.Keuinven==y.Keuinven
                    && x.Keuinlgn==y.Keuinlgn
                    && x.Keufor==y.Keufor
                    && x.Keuopt==y.Keuopt
                    && x.Keugar==y.Keugar
                    && x.Keumsg==y.Keumsg
                    && x.Keunivm==y.Keunivm
                    && x.Keucru==y.Keucru
                    && x.Keucrd==y.Keucrd
                    && x.Keucrh==y.Keucrh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((
                       17 + (this.Keuid.GetHashCode() ) 
                      * 23 ) + (this.Keutyp?? "").GetHashCode()
                      * 23 ) + (this.Keuipb?? "").GetHashCode()
                      * 23 ) + (this.Keualx.GetHashCode() ) 
                      * 23 ) + (this.Keuetape?? "").GetHashCode()
                      * 23 ) + (this.Keuetord.GetHashCode() ) 
                      * 23 ) + (this.Keuordr.GetHashCode() ) 
                      * 23 ) + (this.Keuperi?? "").GetHashCode()
                      * 23 ) + (this.Keursq.GetHashCode() ) 
                      * 23 ) + (this.Keuobj.GetHashCode() ) 
                      * 23 ) + (this.Keuinven.GetHashCode() ) 
                      * 23 ) + (this.Keuinlgn.GetHashCode() ) 
                      * 23 ) + (this.Keufor.GetHashCode() ) 
                      * 23 ) + (this.Keuopt.GetHashCode() ) 
                      * 23 ) + (this.Keugar?? "").GetHashCode()
                      * 23 ) + (this.Keumsg?? "").GetHashCode()
                      * 23 ) + (this.Keunivm?? "").GetHashCode()
                      * 23 ) + (this.Keucru?? "").GetHashCode()
                      * 23 ) + (this.Keucrd.GetHashCode() ) 
                      * 23 ) + (this.Keucrh.GetHashCode() )                    );
           }
        }
    }
}
