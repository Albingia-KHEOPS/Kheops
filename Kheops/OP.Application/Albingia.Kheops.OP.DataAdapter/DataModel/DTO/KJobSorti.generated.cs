using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HJOBSORTI
    public partial class KJobSorti  {
             //HJOBSORTI
             //KJOBSORTI

            ///<summary>Public empty contructor</summary>
            public KJobSorti() {}
            ///<summary>Public empty contructor</summary>
            public KJobSorti(KJobSorti copyFrom) 
            {
                  this.Ipb= copyFrom.Ipb;
                  this.Alx= copyFrom.Alx;
                  this.Typ= copyFrom.Typ;
                  this.Avn= copyFrom.Avn;
                  this.Hin= copyFrom.Hin;
                  this.Rsq= copyFrom.Rsq;
                  this.Obj= copyFrom.Obj;
                  this.Form= copyFrom.Form;
                  this.Opt= copyFrom.Opt;
                  this.Garan= copyFrom.Garan;
                  this.Datedeb= copyFrom.Datedeb;
                  this.Heuredeb= copyFrom.Heuredeb;
                  this.Datefin= copyFrom.Datefin;
                  this.Heurefin= copyFrom.Heurefin;
                  this.Sorti= copyFrom.Sorti;
        
            }        
            
            ///<summary> IPB </summary>
            public string Ipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Alx { get; set; } 
            
            ///<summary> TYP </summary>
            public string Typ { get; set; } 
            
            ///<summary> AVN </summary>
            public int Avn { get; set; } 
            
            ///<summary> HIN </summary>
            public int? Hin { get; set; } 
            
            ///<summary> RSQ </summary>
            public int Rsq { get; set; } 
            
            ///<summary> OBJ </summary>
            public int Obj { get; set; } 
            
            ///<summary> FORM </summary>
            public int Form { get; set; } 
            
            ///<summary> OPT </summary>
            public int Opt { get; set; } 
            
            ///<summary> GARAN </summary>
            public Int64 Garan { get; set; } 
            
            ///<summary> DATEDEB </summary>
            public int Datedeb { get; set; } 
            
            ///<summary> HEUREDEB </summary>
            public int Heuredeb { get; set; } 
            
            ///<summary> DATEFIN </summary>
            public int Datefin { get; set; } 
            
            ///<summary> HEUREFIN </summary>
            public int Heurefin { get; set; } 
            
            ///<summary> SORTI </summary>
            public string Sorti { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KJobSorti  x=this,  y=obj as KJobSorti;
            if( y == default(KJobSorti) ) return false;
            return (
                    x.Ipb==y.Ipb
                    && x.Alx==y.Alx
                    && x.Typ==y.Typ
                    && x.Avn==y.Avn
                    && x.Rsq==y.Rsq
                    && x.Obj==y.Obj
                    && x.Form==y.Form
                    && x.Opt==y.Opt
                    && x.Garan==y.Garan
                    && x.Datedeb==y.Datedeb
                    && x.Heuredeb==y.Heuredeb
                    && x.Datefin==y.Datefin
                    && x.Heurefin==y.Heurefin
                    && x.Sorti==y.Sorti  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((
                       17 + (this.Ipb?? "").GetHashCode()
                      * 23 ) + (this.Alx.GetHashCode() ) 
                      * 23 ) + (this.Typ?? "").GetHashCode()
                      * 23 ) + (this.Avn.GetHashCode() ) 
                      * 23 ) + (this.Rsq.GetHashCode() ) 
                      * 23 ) + (this.Obj.GetHashCode() ) 
                      * 23 ) + (this.Form.GetHashCode() ) 
                      * 23 ) + (this.Opt.GetHashCode() ) 
                      * 23 ) + (this.Garan.GetHashCode() ) 
                      * 23 ) + (this.Datedeb.GetHashCode() ) 
                      * 23 ) + (this.Heuredeb.GetHashCode() ) 
                      * 23 ) + (this.Datefin.GetHashCode() ) 
                      * 23 ) + (this.Heurefin.GetHashCode() ) 
                      * 23 ) + (this.Sorti?? "").GetHashCode()                   );
           }
        }
    }
}
