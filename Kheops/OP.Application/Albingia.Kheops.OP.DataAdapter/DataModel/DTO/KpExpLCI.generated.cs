using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPEXPLCI
    public partial class KpExpLCI  {
             //HPEXPLCI
             //KPEXPLCI

            ///<summary>Public empty contructor</summary>
            public KpExpLCI() {}
            ///<summary>Public empty contructor</summary>
            public KpExpLCI(KpExpLCI copyFrom) 
            {
                  this.Kdiid= copyFrom.Kdiid;
                  this.Kdityp= copyFrom.Kdityp;
                  this.Kdiipb= copyFrom.Kdiipb;
                  this.Kdialx= copyFrom.Kdialx;
                  this.Kdiavn= copyFrom.Kdiavn;
                  this.Kdihin= copyFrom.Kdihin;
                  this.Kdilce= copyFrom.Kdilce;
                  this.Kdidesc= copyFrom.Kdidesc;
                  this.Kdidesi= copyFrom.Kdidesi;
                  this.Kdiori= copyFrom.Kdiori;
                  this.Kdimodi= copyFrom.Kdimodi;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdiid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdityp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdiipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdialx { get; set; } 
            
            ///<summary> n° avenant </summary>
            public int? Kdiavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdihin { get; set; } 
            
            ///<summary> Expression complexe </summary>
            public string Kdilce { get; set; } 
            
            ///<summary> Description </summary>
            public string Kdidesc { get; set; } 
            
            ///<summary> Lien KPDESI </summary>
            public Int64 Kdidesi { get; set; } 
            
            ///<summary> Origine R/S Référentiel/Saisie </summary>
            public string Kdiori { get; set; } 
            
            ///<summary> Modifiable O/N </summary>
            public string Kdimodi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpExpLCI  x=this,  y=obj as KpExpLCI;
            if( y == default(KpExpLCI) ) return false;
            return (
                    x.Kdiid==y.Kdiid
                    && x.Kdityp==y.Kdityp
                    && x.Kdiipb==y.Kdiipb
                    && x.Kdialx==y.Kdialx
                    && x.Kdilce==y.Kdilce
                    && x.Kdidesc==y.Kdidesc
                    && x.Kdidesi==y.Kdidesi
                    && x.Kdiori==y.Kdiori
                    && x.Kdimodi==y.Kdimodi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Kdiid.GetHashCode() ) 
                      * 23 ) + (this.Kdityp?? "").GetHashCode()
                      * 23 ) + (this.Kdiipb?? "").GetHashCode()
                      * 23 ) + (this.Kdialx.GetHashCode() ) 
                      * 23 ) + (this.Kdilce?? "").GetHashCode()
                      * 23 ) + (this.Kdidesc?? "").GetHashCode()
                      * 23 ) + (this.Kdidesi.GetHashCode() ) 
                      * 23 ) + (this.Kdiori?? "").GetHashCode()
                      * 23 ) + (this.Kdimodi?? "").GetHashCode()                   );
           }
        }
    }
}
