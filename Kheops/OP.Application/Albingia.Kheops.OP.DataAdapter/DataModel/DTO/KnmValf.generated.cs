using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.KNMVALF
    public partial class KnmValf  {
             //KNMVALF

            ///<summary>Public empty contructor</summary>
            public KnmValf() {}
            ///<summary>Public empty contructor</summary>
            public KnmValf(KnmValf copyFrom) 
            {
                  this.Khkid= copyFrom.Khkid;
                  this.Khknmg= copyFrom.Khknmg;
                  this.Khktypo= copyFrom.Khktypo;
                  this.Khkordr= copyFrom.Khkordr;
                  this.Khkniv= copyFrom.Khkniv;
                  this.Khkmer= copyFrom.Khkmer;
                  this.Khkkhiid= copyFrom.Khkkhiid;
        
            }        
            
            ///<summary> ID Unique </summary>
            public Int64 Khkid { get; set; } 
            
            ///<summary> Grille </summary>
            public string Khknmg { get; set; } 
            
            ///<summary> Typologie </summary>
            public string Khktypo { get; set; } 
            
            ///<summary> N° Ordre </summary>
            public Decimal Khkordr { get; set; } 
            
            ///<summary> Niveau  de 1 à 5 </summary>
            public int Khkniv { get; set; } 
            
            ///<summary> Niveau mère </summary>
            public Int64 Khkmer { get; set; } 
            
            ///<summary> ID KNMREF 1 </summary>
            public Int64 Khkkhiid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KnmValf  x=this,  y=obj as KnmValf;
            if( y == default(KnmValf) ) return false;
            return (
                    x.Khkid==y.Khkid
                    && x.Khknmg==y.Khknmg
                    && x.Khktypo==y.Khktypo
                    && x.Khkordr==y.Khkordr
                    && x.Khkniv==y.Khkniv
                    && x.Khkmer==y.Khkmer
                    && x.Khkkhiid==y.Khkkhiid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((
                       17 + (this.Khkid.GetHashCode() ) 
                      * 23 ) + (this.Khknmg?? "").GetHashCode()
                      * 23 ) + (this.Khktypo?? "").GetHashCode()
                      * 23 ) + (this.Khkordr.GetHashCode() ) 
                      * 23 ) + (this.Khkniv.GetHashCode() ) 
                      * 23 ) + (this.Khkmer.GetHashCode() ) 
                      * 23 ) + (this.Khkkhiid.GetHashCode() )                    );
           }
        }
    }
}
