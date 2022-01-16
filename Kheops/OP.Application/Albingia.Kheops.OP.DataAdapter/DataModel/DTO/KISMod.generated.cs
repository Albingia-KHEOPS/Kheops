using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KISMOD
    public partial class KISMod  {
             //KISMOD

            ///<summary>Public empty contructor</summary>
            public KISMod() {}
            ///<summary>Public empty contructor</summary>
            public KISMod(KISMod copyFrom) 
            {
                  this.Kgcmodid= copyFrom.Kgcmodid;
                  this.Kgcdesc= copyFrom.Kgcdesc;
                  this.Kgcdatd= copyFrom.Kgcdatd;
                  this.Kgcdatf= copyFrom.Kgcdatf;
                  this.Kgcselect= copyFrom.Kgcselect;
                  this.Kgcinsert= copyFrom.Kgcinsert;
                  this.Kgcupdate= copyFrom.Kgcupdate;
                  this.Kgcexist= copyFrom.Kgcexist;
                  this.Kgccru= copyFrom.Kgccru;
                  this.Kgccrd= copyFrom.Kgccrd;
                  this.Kgcmju= copyFrom.Kgcmju;
                  this.Kgcmjd= copyFrom.Kgcmjd;
                  this.Kgcsaid2= copyFrom.Kgcsaid2;
                  this.Kgcscid2= copyFrom.Kgcscid2;
        
            }        
            
            ///<summary> Code modèle ID </summary>
            public string Kgcmodid { get; set; } 
            
            ///<summary> Description </summary>
            public string Kgcdesc { get; set; } 
            
            ///<summary> Date Début </summary>
            public int Kgcdatd { get; set; } 
            
            ///<summary> Date Fin </summary>
            public int Kgcdatf { get; set; } 
            
            ///<summary> Instruction SELECT </summary>
            public string Kgcselect { get; set; } 
            
            ///<summary> Instruction INSERT </summary>
            public string Kgcinsert { get; set; } 
            
            ///<summary> Instruction UPDATE </summary>
            public string Kgcupdate { get; set; } 
            
            ///<summary> INstruction EXIST </summary>
            public string Kgcexist { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kgccru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kgccrd { get; set; } 
            
            ///<summary>  </summary>
            public string Kgcmju { get; set; } 
            
            ///<summary> Maj User </summary>
            public int Kgcmjd { get; set; } 
            
            ///<summary> Script Affichage - lien KHTSCRIPT </summary>
            public string Kgcsaid2 { get; set; } 
            
            ///<summary> Script Condition - lien KHTSCRIPT </summary>
            public string Kgcscid2 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KISMod  x=this,  y=obj as KISMod;
            if( y == default(KISMod) ) return false;
            return (
                    x.Kgcmodid==y.Kgcmodid
                    && x.Kgcdesc==y.Kgcdesc
                    && x.Kgcdatd==y.Kgcdatd
                    && x.Kgcdatf==y.Kgcdatf
                    && x.Kgcselect==y.Kgcselect
                    && x.Kgcinsert==y.Kgcinsert
                    && x.Kgcupdate==y.Kgcupdate
                    && x.Kgcexist==y.Kgcexist
                    && x.Kgccru==y.Kgccru
                    && x.Kgccrd==y.Kgccrd
                    && x.Kgcmju==y.Kgcmju
                    && x.Kgcmjd==y.Kgcmjd
                    && x.Kgcsaid2==y.Kgcsaid2
                    && x.Kgcscid2==y.Kgcscid2  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((
                       17 + (this.Kgcmodid?? "").GetHashCode()
                      * 23 ) + (this.Kgcdesc?? "").GetHashCode()
                      * 23 ) + (this.Kgcdatd.GetHashCode() ) 
                      * 23 ) + (this.Kgcdatf.GetHashCode() ) 
                      * 23 ) + (this.Kgcselect?? "").GetHashCode()
                      * 23 ) + (this.Kgcinsert?? "").GetHashCode()
                      * 23 ) + (this.Kgcupdate?? "").GetHashCode()
                      * 23 ) + (this.Kgcexist?? "").GetHashCode()
                      * 23 ) + (this.Kgccru?? "").GetHashCode()
                      * 23 ) + (this.Kgccrd.GetHashCode() ) 
                      * 23 ) + (this.Kgcmju?? "").GetHashCode()
                      * 23 ) + (this.Kgcmjd.GetHashCode() ) 
                      * 23 ) + (this.Kgcsaid2?? "").GetHashCode()
                      * 23 ) + (this.Kgcscid2?? "").GetHashCode()                   );
           }
        }
    }
}
