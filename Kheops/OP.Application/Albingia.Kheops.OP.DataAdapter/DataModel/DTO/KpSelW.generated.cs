using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKSPP.KPSELW
    public partial class KpSelW  {
             //KPSELW

            ///<summary>Public empty contructor</summary>
            public KpSelW() {}
            ///<summary>Public empty contructor</summary>
            public KpSelW(KpSelW copyFrom) 
            {
                  this.Khvid= copyFrom.Khvid;
                  this.Khvtyp= copyFrom.Khvtyp;
                  this.Khvipb= copyFrom.Khvipb;
                  this.Khvalx= copyFrom.Khvalx;
                  this.Khvperi= copyFrom.Khvperi;
                  this.Khvrsq= copyFrom.Khvrsq;
                  this.Khvobj= copyFrom.Khvobj;
                  this.Khvfor= copyFrom.Khvfor;
                  this.Khvkdeid= copyFrom.Khvkdeid;
                  this.Khvedtb= copyFrom.Khvedtb;
                  this.Khvdeb= copyFrom.Khvdeb;
                  this.Khvfin= copyFrom.Khvfin;
                  this.Khveco= copyFrom.Khveco;
                  this.Khvavn= copyFrom.Khvavn;
                  this.Khvkdeseq= copyFrom.Khvkdeseq;
                  this.Khvkdegar= copyFrom.Khvkdegar;
        
            }        
            
            ///<summary> ID lot de sélection </summary>
            public Int64 Khvid { get; set; } 
            
            ///<summary> Type </summary>
            public string Khvtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Khvipb { get; set; } 
            
            ///<summary> Aliment </summary>
            public int Khvalx { get; set; } 
            
            ///<summary> Périmètre   RQ OB FO GA </summary>
            public string Khvperi { get; set; } 
            
            ///<summary> N° de risque </summary>
            public int Khvrsq { get; set; } 
            
            ///<summary> N° objet </summary>
            public int Khvobj { get; set; } 
            
            ///<summary> Formule </summary>
            public int Khvfor { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Khvkdeid { get; set; } 
            
            ///<summary> Type Edition Tableau Libellé ou Comp </summary>
            public string Khvedtb { get; set; } 
            
            ///<summary> Plage Début </summary>
            public int Khvdeb { get; set; } 
            
            ///<summary> Plage Fin </summary>
            public int Khvfin { get; set; } 
            
            ///<summary> En-cours O/N </summary>
            public string Khveco { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int Khvavn { get; set; } 
            
            ///<summary> N° séquence garantie </summary>
            public Int64 Khvkdeseq { get; set; } 
            
            ///<summary> Code garantie </summary>
            public string Khvkdegar { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpSelW  x=this,  y=obj as KpSelW;
            if( y == default(KpSelW) ) return false;
            return (
                    x.Khvid==y.Khvid
                    && x.Khvtyp==y.Khvtyp
                    && x.Khvipb==y.Khvipb
                    && x.Khvalx==y.Khvalx
                    && x.Khvperi==y.Khvperi
                    && x.Khvrsq==y.Khvrsq
                    && x.Khvobj==y.Khvobj
                    && x.Khvfor==y.Khvfor
                    && x.Khvkdeid==y.Khvkdeid
                    && x.Khvedtb==y.Khvedtb
                    && x.Khvdeb==y.Khvdeb
                    && x.Khvfin==y.Khvfin
                    && x.Khveco==y.Khveco
                    && x.Khvavn==y.Khvavn
                    && x.Khvkdeseq==y.Khvkdeseq
                    && x.Khvkdegar==y.Khvkdegar  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((
                       17 + (this.Khvid.GetHashCode() ) 
                      * 23 ) + (this.Khvtyp?? "").GetHashCode()
                      * 23 ) + (this.Khvipb?? "").GetHashCode()
                      * 23 ) + (this.Khvalx.GetHashCode() ) 
                      * 23 ) + (this.Khvperi?? "").GetHashCode()
                      * 23 ) + (this.Khvrsq.GetHashCode() ) 
                      * 23 ) + (this.Khvobj.GetHashCode() ) 
                      * 23 ) + (this.Khvfor.GetHashCode() ) 
                      * 23 ) + (this.Khvkdeid.GetHashCode() ) 
                      * 23 ) + (this.Khvedtb?? "").GetHashCode()
                      * 23 ) + (this.Khvdeb.GetHashCode() ) 
                      * 23 ) + (this.Khvfin.GetHashCode() ) 
                      * 23 ) + (this.Khveco?? "").GetHashCode()
                      * 23 ) + (this.Khvavn.GetHashCode() ) 
                      * 23 ) + (this.Khvkdeseq.GetHashCode() ) 
                      * 23 ) + (this.Khvkdegar?? "").GetHashCode()                   );
           }
        }
    }
}
