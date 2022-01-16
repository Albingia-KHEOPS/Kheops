using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.YASSNOM
    public partial class YAssNom  {
             //YASSNOM

            ///<summary>Public empty contructor</summary>
            public YAssNom() {}
            ///<summary>Public empty contructor</summary>
            public YAssNom(YAssNom copyFrom) 
            {
                  this.Anias= copyFrom.Anias;
                  this.Aninl= copyFrom.Aninl;
                  this.Antnm= copyFrom.Antnm;
                  this.Anord= copyFrom.Anord;
                  this.Annom= copyFrom.Annom;
                  this.Antit= copyFrom.Antit;
        
            }        
            
            ///<summary> Identifiant Assuré 10/00 </summary>
            public int Anias { get; set; } 
            
            ///<summary> Code interlocuteur Assuré </summary>
            public int Aninl { get; set; } 
            
            ///<summary> Type de nom </summary>
            public string Antnm { get; set; } 
            
            ///<summary> N° ordre </summary>
            public int Anord { get; set; } 
            
            ///<summary> Nom </summary>
            public string Annom { get; set; } 
            
            ///<summary> Titre </summary>
            public string Antit { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YAssNom  x=this,  y=obj as YAssNom;
            if( y == default(YAssNom) ) return false;
            return (
                    x.Anias==y.Anias
                    && x.Aninl==y.Aninl
                    && x.Antnm==y.Antnm
                    && x.Anord==y.Anord
                    && x.Annom==y.Annom
                    && x.Antit==y.Antit  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((
                       17 + (this.Anias.GetHashCode() ) 
                      * 23 ) + (this.Aninl.GetHashCode() ) 
                      * 23 ) + (this.Antnm?? "").GetHashCode()
                      * 23 ) + (this.Anord.GetHashCode() ) 
                      * 23 ) + (this.Annom?? "").GetHashCode()
                      * 23 ) + (this.Antit?? "").GetHashCode()                   );
           }
        }
    }
}
