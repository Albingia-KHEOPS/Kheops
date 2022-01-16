using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KFILTRE
    public partial class KFiltre  {
             //KFILTRE

            ///<summary>Public empty contructor</summary>
            public KFiltre() {}
            ///<summary>Public empty contructor</summary>
            public KFiltre(KFiltre copyFrom) 
            {
                  this.Kggid= copyFrom.Kggid;
                  this.Kggfilt= copyFrom.Kggfilt;
                  this.Kggdesc= copyFrom.Kggdesc;
                  this.Kggcru= copyFrom.Kggcru;
                  this.Kggcrd= copyFrom.Kggcrd;
                  this.Kggcrh= copyFrom.Kggcrh;
                  this.Kggmaju= copyFrom.Kggmaju;
                  this.Kggmajd= copyFrom.Kggmajd;
                  this.Kggmajh= copyFrom.Kggmajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kggid { get; set; } 
            
            ///<summary> Filtre </summary>
            public string Kggfilt { get; set; } 
            
            ///<summary> Description </summary>
            public string Kggdesc { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kggcru { get; set; } 
            
            ///<summary> Création Date  AAAAMMJJ </summary>
            public int Kggcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kggcrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kggmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kggmajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kggmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KFiltre  x=this,  y=obj as KFiltre;
            if( y == default(KFiltre) ) return false;
            return (
                    x.Kggid==y.Kggid
                    && x.Kggfilt==y.Kggfilt
                    && x.Kggdesc==y.Kggdesc
                    && x.Kggcru==y.Kggcru
                    && x.Kggcrd==y.Kggcrd
                    && x.Kggcrh==y.Kggcrh
                    && x.Kggmaju==y.Kggmaju
                    && x.Kggmajd==y.Kggmajd
                    && x.Kggmajh==y.Kggmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Kggid.GetHashCode() ) 
                      * 23 ) + (this.Kggfilt?? "").GetHashCode()
                      * 23 ) + (this.Kggdesc?? "").GetHashCode()
                      * 23 ) + (this.Kggcru?? "").GetHashCode()
                      * 23 ) + (this.Kggcrd.GetHashCode() ) 
                      * 23 ) + (this.Kggcrh.GetHashCode() ) 
                      * 23 ) + (this.Kggmaju?? "").GetHashCode()
                      * 23 ) + (this.Kggmajd.GetHashCode() ) 
                      * 23 ) + (this.Kggmajh.GetHashCode() )                    );
           }
        }
    }
}
