using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KCATMODELE
    public partial class Kcatmodele  {
             //KCATMODELE

            ///<summary>Public empty contructor</summary>
            public Kcatmodele() {}
            ///<summary>Public empty contructor</summary>
            public Kcatmodele(Kcatmodele copyFrom) 
            {
                  this.Karid= copyFrom.Karid;
                  this.Karkaqid= copyFrom.Karkaqid;
                  this.Kardateapp= copyFrom.Kardateapp;
                  this.Kartypo= copyFrom.Kartypo;
                  this.Karmodele= copyFrom.Karmodele;
                  this.Karcru= copyFrom.Karcru;
                  this.Karcrd= copyFrom.Karcrd;
                  this.Karcrh= copyFrom.Karcrh;
                  this.Karmaju= copyFrom.Karmaju;
                  this.Karmajd= copyFrom.Karmajd;
                  this.Karmajh= copyFrom.Karmajh;
        
            }        
            
            ///<summary>  </summary>
            public int Karid { get; set; } 
            
            ///<summary>  </summary>
            public int Karkaqid { get; set; } 
            
            ///<summary>  </summary>
            public int Kardateapp { get; set; } 
            
            ///<summary>  </summary>
            public string Kartypo { get; set; } 
            
            ///<summary>  </summary>
            public string Karmodele { get; set; } 
            
            ///<summary>  </summary>
            public string Karcru { get; set; } 
            
            ///<summary>  </summary>
            public int Karcrd { get; set; } 
            
            ///<summary>  </summary>
            public int Karcrh { get; set; } 
            
            ///<summary>  </summary>
            public string Karmaju { get; set; } 
            
            ///<summary>  </summary>
            public int Karmajd { get; set; } 
            
            ///<summary>  </summary>
            public int Karmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kcatmodele  x=this,  y=obj as Kcatmodele;
            if( y == default(Kcatmodele) ) return false;
            return (
                    x.Karid==y.Karid
                    && x.Karkaqid==y.Karkaqid
                    && x.Kardateapp==y.Kardateapp
                    && x.Kartypo==y.Kartypo
                    && x.Karmodele==y.Karmodele
                    && x.Karcru==y.Karcru
                    && x.Karcrd==y.Karcrd
                    && x.Karcrh==y.Karcrh
                    && x.Karmaju==y.Karmaju
                    && x.Karmajd==y.Karmajd
                    && x.Karmajh==y.Karmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((
                       17 + (this.Karid.GetHashCode() ) 
                      * 23 ) + (this.Karkaqid.GetHashCode() ) 
                      * 23 ) + (this.Kardateapp.GetHashCode() ) 
                      * 23 ) + (this.Kartypo?? "").GetHashCode()
                      * 23 ) + (this.Karmodele?? "").GetHashCode()
                      * 23 ) + (this.Karcru?? "").GetHashCode()
                      * 23 ) + (this.Karcrd.GetHashCode() ) 
                      * 23 ) + (this.Karcrh.GetHashCode() ) 
                      * 23 ) + (this.Karmaju?? "").GetHashCode()
                      * 23 ) + (this.Karmajd.GetHashCode() ) 
                      * 23 ) + (this.Karmajh.GetHashCode() )                    );
           }
        }
    }
}
