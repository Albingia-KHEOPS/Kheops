using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KBLOREL
    public partial class Kblorel  {
             //KBLOREL

            ///<summary>Public empty contructor</summary>
            public Kblorel() {}
            ///<summary>Public empty contructor</summary>
            public Kblorel(Kblorel copyFrom) 
            {
                  this.Kgjid= copyFrom.Kgjid;
                  this.Kgjrel= copyFrom.Kgjrel;
                  this.Kgjidblo1= copyFrom.Kgjidblo1;
                  this.Kgjblo1= copyFrom.Kgjblo1;
                  this.Kgjidblo2= copyFrom.Kgjidblo2;
                  this.Kgjblo2= copyFrom.Kgjblo2;
                  this.Kgjcru= copyFrom.Kgjcru;
                  this.Kgjcrd= copyFrom.Kgjcrd;
                  this.Kgjcrh= copyFrom.Kgjcrh;
                  this.Kgjmaju= copyFrom.Kgjmaju;
                  this.Kgjmajd= copyFrom.Kgjmajd;
                  this.Kgjmajh= copyFrom.Kgjmajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kgjid { get; set; } 
            
            ///<summary> Type de relation  Incompat Associé </summary>
            public string Kgjrel { get; set; } 
            
            ///<summary> ID bloc  1  Lien KBLOC </summary>
            public Int64 Kgjidblo1 { get; set; } 
            
            ///<summary> Bloc  1 </summary>
            public string Kgjblo1 { get; set; } 
            
            ///<summary> ID Bloc 2  Lien KBLOC </summary>
            public Int64 Kgjidblo2 { get; set; } 
            
            ///<summary> Bloc  2 </summary>
            public string Kgjblo2 { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kgjcru { get; set; } 
            
            ///<summary> Date création </summary>
            public int Kgjcrd { get; set; } 
            
            ///<summary> Création heure </summary>
            public int Kgjcrh { get; set; } 
            
            ///<summary> Maj user </summary>
            public string Kgjmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kgjmajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kgjmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kblorel  x=this,  y=obj as Kblorel;
            if( y == default(Kblorel) ) return false;
            return (
                    x.Kgjid==y.Kgjid
                    && x.Kgjrel==y.Kgjrel
                    && x.Kgjidblo1==y.Kgjidblo1
                    && x.Kgjblo1==y.Kgjblo1
                    && x.Kgjidblo2==y.Kgjidblo2
                    && x.Kgjblo2==y.Kgjblo2
                    && x.Kgjcru==y.Kgjcru
                    && x.Kgjcrd==y.Kgjcrd
                    && x.Kgjcrh==y.Kgjcrh
                    && x.Kgjmaju==y.Kgjmaju
                    && x.Kgjmajd==y.Kgjmajd
                    && x.Kgjmajh==y.Kgjmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((
                       17 + (this.Kgjid.GetHashCode() ) 
                      * 23 ) + (this.Kgjrel?? "").GetHashCode()
                      * 23 ) + (this.Kgjidblo1.GetHashCode() ) 
                      * 23 ) + (this.Kgjblo1?? "").GetHashCode()
                      * 23 ) + (this.Kgjidblo2.GetHashCode() ) 
                      * 23 ) + (this.Kgjblo2?? "").GetHashCode()
                      * 23 ) + (this.Kgjcru?? "").GetHashCode()
                      * 23 ) + (this.Kgjcrd.GetHashCode() ) 
                      * 23 ) + (this.Kgjcrh.GetHashCode() ) 
                      * 23 ) + (this.Kgjmaju?? "").GetHashCode()
                      * 23 ) + (this.Kgjmajd.GetHashCode() ) 
                      * 23 ) + (this.Kgjmajh.GetHashCode() )                    );
           }
        }
    }
}
