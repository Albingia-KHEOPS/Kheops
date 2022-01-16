using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KPODBLS
    public partial class Kpdobls  {
             //KPODBLS

            ///<summary>Public empty contructor</summary>
            public Kpdobls() {}
            ///<summary>Public empty contructor</summary>
            public Kpdobls(Kpdobls copyFrom) 
            {
                  this.Kafid= copyFrom.Kafid;
                  this.Kaftyp= copyFrom.Kaftyp;
                  this.Kafipb= copyFrom.Kafipb;
                  this.Kafalx= copyFrom.Kafalx;
                  this.Kafict= copyFrom.Kafict;
                  this.Kafsou= copyFrom.Kafsou;
                  this.Kafsaid= copyFrom.Kafsaid;
                  this.Kafsaih= copyFrom.Kafsaih;
                  this.Kafsit= copyFrom.Kafsit;
                  this.Kafsitd= copyFrom.Kafsitd;
                  this.Kafsith= copyFrom.Kafsith;
                  this.Kafsitu= copyFrom.Kafsitu;
                  this.Kafcrd= copyFrom.Kafcrd;
                  this.Kafcrh= copyFrom.Kafcrh;
                  this.Kafcru= copyFrom.Kafcru;
                  this.Kafact= copyFrom.Kafact;
                  this.Kafmot= copyFrom.Kafmot;
                  this.Kafin5= copyFrom.Kafin5;
                  this.Kafoct= copyFrom.Kafoct;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kafid { get; set; } 
            
            ///<summary> Type  O Offre </summary>
            public string Kaftyp { get; set; } 
            
            ///<summary> N° Offre </summary>
            public string Kafipb { get; set; } 
            
            ///<summary> N° version </summary>
            public int Kafalx { get; set; } 
            
            ///<summary> Identifiant courtier </summary>
            public int Kafict { get; set; } 
            
            ///<summary> Souscripteur </summary>
            public string Kafsou { get; set; } 
            
            ///<summary> Saisie Date </summary>
            public int Kafsaid { get; set; } 
            
            ///<summary> Saisie Heure </summary>
            public int Kafsaih { get; set; } 
            
            ///<summary> Situation A/X </summary>
            public string Kafsit { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kafsitd { get; set; } 
            
            ///<summary> Situation Heure </summary>
            public int Kafsith { get; set; } 
            
            ///<summary> Situation user </summary>
            public string Kafsitu { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kafcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kafcrh { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kafcru { get; set; } 
            
            ///<summary> Acte  App Ini, Ref refus Rem rempl </summary>
            public string Kafact { get; set; } 
            
            ///<summary> Motif </summary>
            public string Kafmot { get; set; } 
            
            ///<summary> Code interlocuteur sur 5 </summary>
            public int Kafin5 { get; set; } 
            
            ///<summary> Référence Contrat chez courtier </summary>
            public string Kafoct { get; set; } 
  


        public override bool Equals(object  obj)
        {
            Kpdobls  x=this,  y=obj as Kpdobls;
            if( y == default(Kpdobls) ) return false;
            return (
                    x.Kafid==y.Kafid
                    && x.Kaftyp==y.Kaftyp
                    && x.Kafipb==y.Kafipb
                    && x.Kafalx==y.Kafalx
                    && x.Kafict==y.Kafict
                    && x.Kafsou==y.Kafsou
                    && x.Kafsaid==y.Kafsaid
                    && x.Kafsaih==y.Kafsaih
                    && x.Kafsit==y.Kafsit
                    && x.Kafsitd==y.Kafsitd
                    && x.Kafsith==y.Kafsith
                    && x.Kafsitu==y.Kafsitu
                    && x.Kafcrd==y.Kafcrd
                    && x.Kafcrh==y.Kafcrh
                    && x.Kafcru==y.Kafcru
                    && x.Kafact==y.Kafact
                    && x.Kafmot==y.Kafmot
                    && x.Kafin5==y.Kafin5
                    && x.Kafoct==y.Kafoct  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((
                       17 + (this.Kafid.GetHashCode() ) 
                      * 23 ) + (this.Kaftyp?? "").GetHashCode()
                      * 23 ) + (this.Kafipb?? "").GetHashCode()
                      * 23 ) + (this.Kafalx.GetHashCode() ) 
                      * 23 ) + (this.Kafict.GetHashCode() ) 
                      * 23 ) + (this.Kafsou?? "").GetHashCode()
                      * 23 ) + (this.Kafsaid.GetHashCode() ) 
                      * 23 ) + (this.Kafsaih.GetHashCode() ) 
                      * 23 ) + (this.Kafsit?? "").GetHashCode()
                      * 23 ) + (this.Kafsitd.GetHashCode() ) 
                      * 23 ) + (this.Kafsith.GetHashCode() ) 
                      * 23 ) + (this.Kafsitu?? "").GetHashCode()
                      * 23 ) + (this.Kafcrd.GetHashCode() ) 
                      * 23 ) + (this.Kafcrh.GetHashCode() ) 
                      * 23 ) + (this.Kafcru?? "").GetHashCode()
                      * 23 ) + (this.Kafact?? "").GetHashCode()
                      * 23 ) + (this.Kafmot?? "").GetHashCode()
                      * 23 ) + (this.Kafin5.GetHashCode() ) 
                      * 23 ) + (this.Kafoct?? "").GetHashCode()                   );
           }
        }
    }
}
