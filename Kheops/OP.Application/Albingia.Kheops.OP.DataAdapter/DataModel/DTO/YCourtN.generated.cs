using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YCOURTN
    public partial class YCourtN  {
             //YCOURTN

            ///<summary>Public empty contructor</summary>
            public YCourtN() {}
            ///<summary>Public empty contructor</summary>
            public YCourtN(YCourtN copyFrom) 
            {
                  this.Tnict= copyFrom.Tnict;
                  this.Tninl= copyFrom.Tninl;
                  this.Tntnm= copyFrom.Tntnm;
                  this.Tnord= copyFrom.Tnord;
                  this.Tntyp= copyFrom.Tntyp;
                  this.Tnnom= copyFrom.Tnnom;
                  this.Tntit= copyFrom.Tntit;
                  this.Tnxn5= copyFrom.Tnxn5;
        
            }        
            
            ///<summary> Identifiant courtier </summary>
            public int Tnict { get; set; } 
            
            ///<summary> Code interlocuteur Courtier </summary>
            public int Tninl { get; set; } 
            
            ///<summary> Type de nom </summary>
            public string Tntnm { get; set; } 
            
            ///<summary> N° ordre </summary>
            public int Tnord { get; set; } 
            
            ///<summary> Type Prospect/Courtier </summary>
            public string Tntyp { get; set; } 
            
            ///<summary> Nom </summary>
            public string Tnnom { get; set; } 
            
            ///<summary> Titre </summary>
            public string Tntit { get; set; } 
            
            ///<summary> Code interlocuteur sur 5 </summary>
            public int Tnxn5 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YCourtN  x=this,  y=obj as YCourtN;
            if( y == default(YCourtN) ) return false;
            return (
                    x.Tnict==y.Tnict
                    && x.Tninl==y.Tninl
                    && x.Tntnm==y.Tntnm
                    && x.Tnord==y.Tnord
                    && x.Tntyp==y.Tntyp
                    && x.Tnnom==y.Tnnom
                    && x.Tntit==y.Tntit
                    && x.Tnxn5==y.Tnxn5  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((
                       17 + (this.Tnict.GetHashCode() ) 
                      * 23 ) + (this.Tninl.GetHashCode() ) 
                      * 23 ) + (this.Tntnm?? "").GetHashCode()
                      * 23 ) + (this.Tnord.GetHashCode() ) 
                      * 23 ) + (this.Tntyp?? "").GetHashCode()
                      * 23 ) + (this.Tnnom?? "").GetHashCode()
                      * 23 ) + (this.Tntit?? "").GetHashCode()
                      * 23 ) + (this.Tnxn5.GetHashCode() )                    );
           }
        }
    }
}
