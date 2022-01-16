using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.YADRESS
    public partial class YAdress  {
             //YADRESS

            ///<summary>Public empty contructor</summary>
            public YAdress() {}
            ///<summary>Public empty contructor</summary>
            public YAdress(YAdress copyFrom) 
            {
                  this.Abpchr= copyFrom.Abpchr;
                  this.Abplg3= copyFrom.Abplg3;
                  this.Abpnum= copyFrom.Abpnum;
                  this.Abpext= copyFrom.Abpext;
                  this.Abplbn= copyFrom.Abplbn;
                  this.Abplg4= copyFrom.Abplg4;
                  this.Abpl4f= copyFrom.Abpl4f;
                  this.Abplg5= copyFrom.Abplg5;
                  this.Abpdp6= copyFrom.Abpdp6;
                  this.Abpcp6= copyFrom.Abpcp6;
                  this.Abpvi6= copyFrom.Abpvi6;
                  this.Abpcdx= copyFrom.Abpcdx;
                  this.Abpcex= copyFrom.Abpcex;
                  this.Abpl6f= copyFrom.Abpl6f;
                  this.Abppay= copyFrom.Abppay;
                  this.Abploc= copyFrom.Abploc;
                  this.Abpmat= copyFrom.Abpmat;
                  this.Abpret= copyFrom.Abpret;
                  this.Abperr= copyFrom.Abperr;
                  this.Abpmju= copyFrom.Abpmju;
                  this.Abpmja= copyFrom.Abpmja;
                  this.Abpmjm= copyFrom.Abpmjm;
                  this.Abpmjj= copyFrom.Abpmjj;
                  this.Abpvix= copyFrom.Abpvix;
        
            }        
            
            ///<summary> N° Chrono unique </summary>
            public int Abpchr { get; set; } 
            
            ///<summary> Ligne 3 AFNOR </summary>
            public string Abplg3 { get; set; } 
            
            ///<summary> N° dans la voie </summary>
            public int Abpnum { get; set; } 
            
            ///<summary> Extension abrégée(Bis Ter ..... </summary>
            public string Abpext { get; set; } 
            
            ///<summary> Libellé N° si multi (109/111 109-111 </summary>
            public string Abplbn { get; set; } 
            
            ///<summary> Ligne 4 AFNOR </summary>
            public string Abplg4 { get; set; } 
            
            ///<summary> Ligne 4 formatée </summary>
            public string Abpl4f { get; set; } 
            
            ///<summary> Ligne 5 AFNOR </summary>
            public string Abplg5 { get; set; } 
            
            ///<summary> Département ligne 6 </summary>
            public string Abpdp6 { get; set; } 
            
            ///<summary> Code postal  Ligne 6 </summary>
            public int Abpcp6 { get; set; } 
            
            ///<summary> Libellé acheminement Ligne 6 </summary>
            public string Abpvi6 { get; set; } 
            
            ///<summary> Type Cedex Cidex BP </summary>
            public string Abpcdx { get; set; } 
            
            ///<summary> Code cedex </summary>
            public int Abpcex { get; set; } 
            
            ///<summary> Ligne 6 formatée </summary>
            public string Abpl6f { get; set; } 
            
            ///<summary> Code pays </summary>
            public string Abppay { get; set; } 
            
            ///<summary> Code INSEE Localité </summary>
            public string Abploc { get; set; } 
            
            ///<summary> Matricule Localité HEXAVIA </summary>
            public int Abpmat { get; set; } 
            
            ///<summary> Code retour (VAL/MUL/INC..... </summary>
            public string Abpret { get; set; } 
            
            ///<summary> Cause erreur </summary>
            public string Abperr { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Abpmju { get; set; } 
            
            ///<summary> Maj Année </summary>
            public int Abpmja { get; set; } 
            
            ///<summary> Maj Mois </summary>
            public int Abpmjm { get; set; } 
            
            ///<summary> Maj Jour </summary>
            public int Abpmjj { get; set; } 
            
            ///<summary> Ville  Cedex </summary>
            public string Abpvix { get; set; } 
  


        public override bool Equals(object  obj)
        {
            YAdress  x=this,  y=obj as YAdress;
            if( y == default(YAdress) ) return false;
            return (
                    x.Abpchr==y.Abpchr
                    && x.Abplg3==y.Abplg3
                    && x.Abpnum==y.Abpnum
                    && x.Abpext==y.Abpext
                    && x.Abplbn==y.Abplbn
                    && x.Abplg4==y.Abplg4
                    && x.Abpl4f==y.Abpl4f
                    && x.Abplg5==y.Abplg5
                    && x.Abpdp6==y.Abpdp6
                    && x.Abpcp6==y.Abpcp6
                    && x.Abpvi6==y.Abpvi6
                    && x.Abpcdx==y.Abpcdx
                    && x.Abpcex==y.Abpcex
                    && x.Abpl6f==y.Abpl6f
                    && x.Abppay==y.Abppay
                    && x.Abploc==y.Abploc
                    && x.Abpmat==y.Abpmat
                    && x.Abpret==y.Abpret
                    && x.Abperr==y.Abperr
                    && x.Abpmju==y.Abpmju
                    && x.Abpmja==y.Abpmja
                    && x.Abpmjm==y.Abpmjm
                    && x.Abpmjj==y.Abpmjj
                    && x.Abpvix==y.Abpvix  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((
                       17 + (this.Abpchr.GetHashCode() ) 
                      * 23 ) + (this.Abplg3?? "").GetHashCode()
                      * 23 ) + (this.Abpnum.GetHashCode() ) 
                      * 23 ) + (this.Abpext?? "").GetHashCode()
                      * 23 ) + (this.Abplbn?? "").GetHashCode()
                      * 23 ) + (this.Abplg4?? "").GetHashCode()
                      * 23 ) + (this.Abpl4f?? "").GetHashCode()
                      * 23 ) + (this.Abplg5?? "").GetHashCode()
                      * 23 ) + (this.Abpdp6?? "").GetHashCode()
                      * 23 ) + (this.Abpcp6.GetHashCode() ) 
                      * 23 ) + (this.Abpvi6?? "").GetHashCode()
                      * 23 ) + (this.Abpcdx?? "").GetHashCode()
                      * 23 ) + (this.Abpcex.GetHashCode() ) 
                      * 23 ) + (this.Abpl6f?? "").GetHashCode()
                      * 23 ) + (this.Abppay?? "").GetHashCode()
                      * 23 ) + (this.Abploc?? "").GetHashCode()
                      * 23 ) + (this.Abpmat.GetHashCode() ) 
                      * 23 ) + (this.Abpret?? "").GetHashCode()
                      * 23 ) + (this.Abperr?? "").GetHashCode()
                      * 23 ) + (this.Abpmju?? "").GetHashCode()
                      * 23 ) + (this.Abpmja.GetHashCode() ) 
                      * 23 ) + (this.Abpmjm.GetHashCode() ) 
                      * 23 ) + (this.Abpmjj.GetHashCode() ) 
                      * 23 ) + (this.Abpvix?? "").GetHashCode()                   );
           }
        }
    }
}
