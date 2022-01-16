using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKMOD.YPLTGAR
    public partial class YpltGar  {
             //YPLTGAR

            ///<summary>Public empty contructor</summary>
            public YpltGar() {}
            ///<summary>Public empty contructor</summary>
            public YpltGar(YpltGar copyFrom) 
            {
                  this.C2seq= copyFrom.C2seq;
                  this.C2mga= copyFrom.C2mga;
                  this.C2obe= copyFrom.C2obe;
                  this.C2niv= copyFrom.C2niv;
                  this.C2gar= copyFrom.C2gar;
                  this.C2ord= copyFrom.C2ord;
                  this.C2lib= copyFrom.C2lib;
                  this.C2sem= copyFrom.C2sem;
                  this.C2car= copyFrom.C2car;
                  this.C2nat= copyFrom.C2nat;
                  this.C2ina= copyFrom.C2ina;
                  this.C2cna= copyFrom.C2cna;
                  this.C2tax= copyFrom.C2tax;
                  this.C2alt= copyFrom.C2alt;
                  this.C2tri= copyFrom.C2tri;
                  this.C2se1= copyFrom.C2se1;
                  this.C2scr= copyFrom.C2scr;
                  this.C2prp= copyFrom.C2prp;
                  this.C2tcd= copyFrom.C2tcd;
                  this.C2mrf= copyFrom.C2mrf;
                  this.C2ntm= copyFrom.C2ntm;
                  this.C2mas= copyFrom.C2mas;
        
            }        
            
            ///<summary>  </summary>
            public Int64 C2seq { get; set; } 
            
            ///<summary>  </summary>
            public string C2mga { get; set; } 
            
            ///<summary>  </summary>
            public string C2obe { get; set; } 
            
            ///<summary>  </summary>
            public int C2niv { get; set; } 
            
            ///<summary>  </summary>
            public string C2gar { get; set; } 
            
            ///<summary>  </summary>
            public int C2ord { get; set; } 
            
            ///<summary>  </summary>
            public string C2lib { get; set; } 
            
            ///<summary>  </summary>
            public Int64 C2sem { get; set; } 
            
            ///<summary>  </summary>
            public string C2car { get; set; } 
            
            ///<summary>  </summary>
            public string C2nat { get; set; } 
            
            ///<summary>  </summary>
            public string C2ina { get; set; } 
            
            ///<summary>  </summary>
            public string C2cna { get; set; } 
            
            ///<summary>  </summary>
            public string C2tax { get; set; } 
            
            ///<summary>  </summary>
            public int C2alt { get; set; } 
            
            ///<summary>  </summary>
            public string C2tri { get; set; } 
            
            ///<summary>  </summary>
            public Int64 C2se1 { get; set; } 
            
            ///<summary>  </summary>
            public string C2scr { get; set; } 
            
            ///<summary>  </summary>
            public string C2prp { get; set; } 
            
            ///<summary>  </summary>
            public string C2tcd { get; set; } 
            
            ///<summary>  </summary>
            public string C2mrf { get; set; } 
            
            ///<summary>  </summary>
            public string C2ntm { get; set; } 
            
            ///<summary>  </summary>
            public string C2mas { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpltGar  x=this,  y=obj as YpltGar;
            if( y == default(YpltGar) ) return false;
            return (
                    x.C2seq==y.C2seq
                    && x.C2mga==y.C2mga
                    && x.C2obe==y.C2obe
                    && x.C2niv==y.C2niv
                    && x.C2gar==y.C2gar
                    && x.C2ord==y.C2ord
                    && x.C2lib==y.C2lib
                    && x.C2sem==y.C2sem
                    && x.C2car==y.C2car
                    && x.C2nat==y.C2nat
                    && x.C2ina==y.C2ina
                    && x.C2cna==y.C2cna
                    && x.C2tax==y.C2tax
                    && x.C2alt==y.C2alt
                    && x.C2tri==y.C2tri
                    && x.C2se1==y.C2se1
                    && x.C2scr==y.C2scr
                    && x.C2prp==y.C2prp
                    && x.C2tcd==y.C2tcd
                    && x.C2mrf==y.C2mrf
                    && x.C2ntm==y.C2ntm
                    && x.C2mas==y.C2mas  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((
                       17 + (this.C2seq.GetHashCode() ) 
                      * 23 ) + (this.C2mga?? "").GetHashCode()
                      * 23 ) + (this.C2obe?? "").GetHashCode()
                      * 23 ) + (this.C2niv.GetHashCode() ) 
                      * 23 ) + (this.C2gar?? "").GetHashCode()
                      * 23 ) + (this.C2ord.GetHashCode() ) 
                      * 23 ) + (this.C2lib?? "").GetHashCode()
                      * 23 ) + (this.C2sem.GetHashCode() ) 
                      * 23 ) + (this.C2car?? "").GetHashCode()
                      * 23 ) + (this.C2nat?? "").GetHashCode()
                      * 23 ) + (this.C2ina?? "").GetHashCode()
                      * 23 ) + (this.C2cna?? "").GetHashCode()
                      * 23 ) + (this.C2tax?? "").GetHashCode()
                      * 23 ) + (this.C2alt.GetHashCode() ) 
                      * 23 ) + (this.C2tri?? "").GetHashCode()
                      * 23 ) + (this.C2se1.GetHashCode() ) 
                      * 23 ) + (this.C2scr?? "").GetHashCode()
                      * 23 ) + (this.C2prp?? "").GetHashCode()
                      * 23 ) + (this.C2tcd?? "").GetHashCode()
                      * 23 ) + (this.C2mrf?? "").GetHashCode()
                      * 23 ) + (this.C2ntm?? "").GetHashCode()
                      * 23 ) + (this.C2mas?? "").GetHashCode()                   );
           }
        }
    }
}
