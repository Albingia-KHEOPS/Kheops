using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KNMGRI
    public partial class KnmGri  {
             //KNMGRI

            ///<summary>Public empty contructor</summary>
            public KnmGri() {}
            ///<summary>Public empty contructor</summary>
            public KnmGri(KnmGri copyFrom) 
            {
                  this.Khjnmg= copyFrom.Khjnmg;
                  this.Khjdesi= copyFrom.Khjdesi;
                  this.Khjtypo1= copyFrom.Khjtypo1;
                  this.Khjlib1= copyFrom.Khjlib1;
                  this.Khjlien1= copyFrom.Khjlien1;
                  this.Khjvalf1= copyFrom.Khjvalf1;
                  this.Khjtypo2= copyFrom.Khjtypo2;
                  this.Khjlib2= copyFrom.Khjlib2;
                  this.Khjlien2= copyFrom.Khjlien2;
                  this.Khjvalf2= copyFrom.Khjvalf2;
                  this.Khjtypo3= copyFrom.Khjtypo3;
                  this.Khjlib3= copyFrom.Khjlib3;
                  this.Khjlien3= copyFrom.Khjlien3;
                  this.Khjvalf3= copyFrom.Khjvalf3;
                  this.Khjtypo4= copyFrom.Khjtypo4;
                  this.Khjlib4= copyFrom.Khjlib4;
                  this.Khjlien4= copyFrom.Khjlien4;
                  this.Khjvalf4= copyFrom.Khjvalf4;
                  this.Khjtypo5= copyFrom.Khjtypo5;
                  this.Khjlib5= copyFrom.Khjlib5;
                  this.Khjlien5= copyFrom.Khjlien5;
                  this.Khjvalf5= copyFrom.Khjvalf5;
        
            }        
            
            ///<summary> Code Grille </summary>
            public string Khjnmg { get; set; } 
            
            ///<summary> Désignation </summary>
            public string Khjdesi { get; set; } 
            
            ///<summary> Typologie 1 </summary>
            public string Khjtypo1 { get; set; } 
            
            ///<summary> Libellé 1 </summary>
            public string Khjlib1 { get; set; } 
            
            ///<summary> Lien 1  I Indép 1 Mère 2,3..Fille </summary>
            public string Khjlien1 { get; set; } 
            
            ///<summary> Valeurs Filtrées1 O/N si Indépendant </summary>
            public string Khjvalf1 { get; set; } 
            
            ///<summary> Typologie 2 </summary>
            public string Khjtypo2 { get; set; } 
            
            ///<summary> Libellé 2 </summary>
            public string Khjlib2 { get; set; } 
            
            ///<summary> Lien 2 </summary>
            public string Khjlien2 { get; set; } 
            
            ///<summary> Valeurs Filtrées 2 </summary>
            public string Khjvalf2 { get; set; } 
            
            ///<summary> Typologie 3 </summary>
            public string Khjtypo3 { get; set; } 
            
            ///<summary> Libellé 3 </summary>
            public string Khjlib3 { get; set; } 
            
            ///<summary> Lien 3 </summary>
            public string Khjlien3 { get; set; } 
            
            ///<summary> Valeurs filtrées 3 </summary>
            public string Khjvalf3 { get; set; } 
            
            ///<summary> Typologie 4 </summary>
            public string Khjtypo4 { get; set; } 
            
            ///<summary> Libellé 4 </summary>
            public string Khjlib4 { get; set; } 
            
            ///<summary> Lien 4 </summary>
            public string Khjlien4 { get; set; } 
            
            ///<summary> Valeurs Filtrées 4 </summary>
            public string Khjvalf4 { get; set; } 
            
            ///<summary> Typologie 5 </summary>
            public string Khjtypo5 { get; set; } 
            
            ///<summary> Libellé 5 </summary>
            public string Khjlib5 { get; set; } 
            
            ///<summary> Lien 5 </summary>
            public string Khjlien5 { get; set; } 
            
            ///<summary> Valeurs Filtrées 5 </summary>
            public string Khjvalf5 { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KnmGri  x=this,  y=obj as KnmGri;
            if( y == default(KnmGri) ) return false;
            return (
                    x.Khjnmg==y.Khjnmg
                    && x.Khjdesi==y.Khjdesi
                    && x.Khjtypo1==y.Khjtypo1
                    && x.Khjlib1==y.Khjlib1
                    && x.Khjlien1==y.Khjlien1
                    && x.Khjvalf1==y.Khjvalf1
                    && x.Khjtypo2==y.Khjtypo2
                    && x.Khjlib2==y.Khjlib2
                    && x.Khjlien2==y.Khjlien2
                    && x.Khjvalf2==y.Khjvalf2
                    && x.Khjtypo3==y.Khjtypo3
                    && x.Khjlib3==y.Khjlib3
                    && x.Khjlien3==y.Khjlien3
                    && x.Khjvalf3==y.Khjvalf3
                    && x.Khjtypo4==y.Khjtypo4
                    && x.Khjlib4==y.Khjlib4
                    && x.Khjlien4==y.Khjlien4
                    && x.Khjvalf4==y.Khjvalf4
                    && x.Khjtypo5==y.Khjtypo5
                    && x.Khjlib5==y.Khjlib5
                    && x.Khjlien5==y.Khjlien5
                    && x.Khjvalf5==y.Khjvalf5  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((
                       17 + (this.Khjnmg?? "").GetHashCode()
                      * 23 ) + (this.Khjdesi?? "").GetHashCode()
                      * 23 ) + (this.Khjtypo1?? "").GetHashCode()
                      * 23 ) + (this.Khjlib1?? "").GetHashCode()
                      * 23 ) + (this.Khjlien1?? "").GetHashCode()
                      * 23 ) + (this.Khjvalf1?? "").GetHashCode()
                      * 23 ) + (this.Khjtypo2?? "").GetHashCode()
                      * 23 ) + (this.Khjlib2?? "").GetHashCode()
                      * 23 ) + (this.Khjlien2?? "").GetHashCode()
                      * 23 ) + (this.Khjvalf2?? "").GetHashCode()
                      * 23 ) + (this.Khjtypo3?? "").GetHashCode()
                      * 23 ) + (this.Khjlib3?? "").GetHashCode()
                      * 23 ) + (this.Khjlien3?? "").GetHashCode()
                      * 23 ) + (this.Khjvalf3?? "").GetHashCode()
                      * 23 ) + (this.Khjtypo4?? "").GetHashCode()
                      * 23 ) + (this.Khjlib4?? "").GetHashCode()
                      * 23 ) + (this.Khjlien4?? "").GetHashCode()
                      * 23 ) + (this.Khjvalf4?? "").GetHashCode()
                      * 23 ) + (this.Khjtypo5?? "").GetHashCode()
                      * 23 ) + (this.Khjlib5?? "").GetHashCode()
                      * 23 ) + (this.Khjlien5?? "").GetHashCode()
                      * 23 ) + (this.Khjvalf5?? "").GetHashCode()                   );
           }
        }
    }
}
