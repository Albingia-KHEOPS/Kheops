using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KCATBLOC
    public partial class Kcatbloc  {
             //KCATBLOC

            ///<summary>Public empty contructor</summary>
            public Kcatbloc() {}
            ///<summary>Public empty contructor</summary>
            public Kcatbloc(Kcatbloc copyFrom) 
            {
                  this.Kaqid= copyFrom.Kaqid;
                  this.Kaqbra= copyFrom.Kaqbra;
                  this.Kaqcible= copyFrom.Kaqcible;
                  this.Kaqvolet= copyFrom.Kaqvolet;
                  this.Kaqkapid= copyFrom.Kaqkapid;
                  this.Kaqbloc= copyFrom.Kaqbloc;
                  this.Kaqkaeid= copyFrom.Kaqkaeid;
                  this.Kaqcar= copyFrom.Kaqcar;
                  this.Kaqordre= copyFrom.Kaqordre;
                  this.Kaqcru= copyFrom.Kaqcru;
                  this.Kaqcrd= copyFrom.Kaqcrd;
                  this.Kaqcrh= copyFrom.Kaqcrh;
                  this.Kaqmaju= copyFrom.Kaqmaju;
                  this.Kaqmajd= copyFrom.Kaqmajd;
                  this.Kaqmajh= copyFrom.Kaqmajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kaqid { get; set; } 
            
            ///<summary> Branche </summary>
            public string Kaqbra { get; set; } 
            
            ///<summary> Cible </summary>
            public string Kaqcible { get; set; } 
            
            ///<summary> Volet </summary>
            public string Kaqvolet { get; set; } 
            
            ///<summary> ID KCATVOLET </summary>
            public Int64 Kaqkapid { get; set; } 
            
            ///<summary> Bloc </summary>
            public string Kaqbloc { get; set; } 
            
            ///<summary> ID KBLOC </summary>
            public Int64 Kaqkaeid { get; set; } 
            
            ///<summary> Caractère </summary>
            public string Kaqcar { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public Decimal Kaqordre { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kaqcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kaqcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kaqcrh { get; set; } 
            
            ///<summary> MAJ user </summary>
            public string Kaqmaju { get; set; } 
            
            ///<summary> MAj Date </summary>
            public int Kaqmajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kaqmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kcatbloc  x=this,  y=obj as Kcatbloc;
            if( y == default(Kcatbloc) ) return false;
            return (
                    x.Kaqid==y.Kaqid
                    && x.Kaqbra==y.Kaqbra
                    && x.Kaqcible==y.Kaqcible
                    && x.Kaqvolet==y.Kaqvolet
                    && x.Kaqkapid==y.Kaqkapid
                    && x.Kaqbloc==y.Kaqbloc
                    && x.Kaqkaeid==y.Kaqkaeid
                    && x.Kaqcar==y.Kaqcar
                    && x.Kaqordre==y.Kaqordre
                    && x.Kaqcru==y.Kaqcru
                    && x.Kaqcrd==y.Kaqcrd
                    && x.Kaqcrh==y.Kaqcrh
                    && x.Kaqmaju==y.Kaqmaju
                    && x.Kaqmajd==y.Kaqmajd
                    && x.Kaqmajh==y.Kaqmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((
                       17 + (this.Kaqid.GetHashCode() ) 
                      * 23 ) + (this.Kaqbra?? "").GetHashCode()
                      * 23 ) + (this.Kaqcible?? "").GetHashCode()
                      * 23 ) + (this.Kaqvolet?? "").GetHashCode()
                      * 23 ) + (this.Kaqkapid.GetHashCode() ) 
                      * 23 ) + (this.Kaqbloc?? "").GetHashCode()
                      * 23 ) + (this.Kaqkaeid.GetHashCode() ) 
                      * 23 ) + (this.Kaqcar?? "").GetHashCode()
                      * 23 ) + (this.Kaqordre.GetHashCode() ) 
                      * 23 ) + (this.Kaqcru?? "").GetHashCode()
                      * 23 ) + (this.Kaqcrd.GetHashCode() ) 
                      * 23 ) + (this.Kaqcrh.GetHashCode() ) 
                      * 23 ) + (this.Kaqmaju?? "").GetHashCode()
                      * 23 ) + (this.Kaqmajd.GetHashCode() ) 
                      * 23 ) + (this.Kaqmajh.GetHashCode() )                    );
           }
        }
    }
}
