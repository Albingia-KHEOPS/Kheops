using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YHPINTE
    public partial class YpoInte  {
             //YHPINTE
             //YPOINTE

            ///<summary>Public empty contructor</summary>
            public YpoInte() {}
            ///<summary>Public empty contructor</summary>
            public YpoInte(YpoInte copyFrom) 
            {
                  this.Ppipb= copyFrom.Ppipb;
                  this.Ppalx= copyFrom.Ppalx;
                  this.Ppavn= copyFrom.Ppavn;
                  this.Pphin= copyFrom.Pphin;
                  this.Ppiin= copyFrom.Ppiin;
                  this.Pptyi= copyFrom.Pptyi;
                  this.Ppinl= copyFrom.Ppinl;
                  this.Pppol= copyFrom.Pppol;
                  this.Ppsym= copyFrom.Ppsym;
        
            }        
            
            ///<summary> N° de Police </summary>
            public string Ppipb { get; set; } 
            
            ///<summary> N° Aliment </summary>
            public int Ppalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Ppavn { get; set; } 
            
            ///<summary> N° historique par avenant </summary>
            public int? Pphin { get; set; } 
            
            ///<summary> Identifiant intervenant </summary>
            public int Ppiin { get; set; } 
            
            ///<summary> Type intervenant (Cie Expert Avoc..) </summary>
            public string Pptyi { get; set; } 
            
            ///<summary> Code interlocuteur </summary>
            public int Ppinl { get; set; } 
            
            ///<summary> Référence police chez intervenant </summary>
            public string Pppol { get; set; } 
            
            ///<summary> Nature mandat du Syndic </summary>
            public string Ppsym { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YpoInte  x=this,  y=obj as YpoInte;
            if( y == default(YpoInte) ) return false;
            return (
                    x.Ppipb==y.Ppipb
                    && x.Ppalx==y.Ppalx
                    && x.Ppiin==y.Ppiin
                    && x.Pptyi==y.Pptyi
                    && x.Ppinl==y.Ppinl
                    && x.Pppol==y.Pppol
                    && x.Ppsym==y.Ppsym  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((
                       17 + (this.Ppipb?? "").GetHashCode()
                      * 23 ) + (this.Ppalx.GetHashCode() ) 
                      * 23 ) + (this.Ppiin.GetHashCode() ) 
                      * 23 ) + (this.Pptyi?? "").GetHashCode()
                      * 23 ) + (this.Ppinl.GetHashCode() ) 
                      * 23 ) + (this.Pppol?? "").GetHashCode()
                      * 23 ) + (this.Ppsym?? "").GetHashCode()                   );
           }
        }
    }
}
