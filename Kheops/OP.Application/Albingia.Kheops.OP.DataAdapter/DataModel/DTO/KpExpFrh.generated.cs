using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPEXPFRH
    public partial class KpExpFrh  {
             //HPEXPFRH
             //KPEXPFRH

            ///<summary>Public empty contructor</summary>
            public KpExpFrh() {}
            ///<summary>Public empty contructor</summary>
            public KpExpFrh(KpExpFrh copyFrom) 
            {
                  this.Kdkid= copyFrom.Kdkid;
                  this.Kdktyp= copyFrom.Kdktyp;
                  this.Kdkipb= copyFrom.Kdkipb;
                  this.Kdkalx= copyFrom.Kdkalx;
                  this.Kdkavn= copyFrom.Kdkavn;
                  this.Kdkhin= copyFrom.Kdkhin;
                  this.Kdkfhe= copyFrom.Kdkfhe;
                  this.Kdkdesc= copyFrom.Kdkdesc;
                  this.Kdkdesi= copyFrom.Kdkdesi;
                  this.Kdkori= copyFrom.Kdkori;
                  this.Kdkmodi= copyFrom.Kdkmodi;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdkid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdktyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdkipb { get; set; } 
            
            ///<summary> Alx </summary>
            public int Kdkalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdkavn { get; set; } 
            
            ///<summary>  N° histo par avenant </summary>
            public int? Kdkhin { get; set; } 
            
            ///<summary> Expression Complexe </summary>
            public string Kdkfhe { get; set; } 
            
            ///<summary> Description </summary>
            public string Kdkdesc { get; set; } 
            
            ///<summary> Lien KPDESI </summary>
            public Int64 Kdkdesi { get; set; } 
            
            ///<summary> Origine R/S   Référentiel/ Saisie </summary>
            public string Kdkori { get; set; } 
            
            ///<summary> Modifiable O/N </summary>
            public string Kdkmodi { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpExpFrh  x=this,  y=obj as KpExpFrh;
            if( y == default(KpExpFrh) ) return false;
            return (
                    x.Kdkid==y.Kdkid
                    && x.Kdktyp==y.Kdktyp
                    && x.Kdkipb==y.Kdkipb
                    && x.Kdkalx==y.Kdkalx
                    && x.Kdkfhe==y.Kdkfhe
                    && x.Kdkdesc==y.Kdkdesc
                    && x.Kdkdesi==y.Kdkdesi
                    && x.Kdkori==y.Kdkori
                    && x.Kdkmodi==y.Kdkmodi  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Kdkid.GetHashCode() ) 
                      * 23 ) + (this.Kdktyp?? "").GetHashCode()
                      * 23 ) + (this.Kdkipb?? "").GetHashCode()
                      * 23 ) + (this.Kdkalx.GetHashCode() ) 
                      * 23 ) + (this.Kdkfhe?? "").GetHashCode()
                      * 23 ) + (this.Kdkdesc?? "").GetHashCode()
                      * 23 ) + (this.Kdkdesi.GetHashCode() ) 
                      * 23 ) + (this.Kdkori?? "").GetHashCode()
                      * 23 ) + (this.Kdkmodi?? "").GetHashCode()                   );
           }
        }
    }
}
