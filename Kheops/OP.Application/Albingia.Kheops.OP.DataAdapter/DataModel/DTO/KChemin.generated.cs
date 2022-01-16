using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KCHEMIN
    public partial class KChemin  {
             //KCHEMIN

            ///<summary>Public empty contructor</summary>
            public KChemin() {}
            ///<summary>Public empty contructor</summary>
            public KChemin(KChemin copyFrom) 
            {
                  this.Khmcle= copyFrom.Khmcle;
                  this.Khmsrv= copyFrom.Khmsrv;
                  this.Khmrac= copyFrom.Khmrac;
                  this.Khmenv= copyFrom.Khmenv;
                  this.Khmdes= copyFrom.Khmdes;
                  this.Khmtch= copyFrom.Khmtch;
                  this.Khmchm= copyFrom.Khmchm;
                  this.Khmcru= copyFrom.Khmcru;
                  this.Khmcrd= copyFrom.Khmcrd;
                  this.Khmmju= copyFrom.Khmmju;
                  this.Khmmjd= copyFrom.Khmmjd;
        
            }        
            
            ///<summary> Clé document </summary>
            public string Khmcle { get; set; } 
            
            ///<summary> Serveur </summary>
            public string Khmsrv { get; set; } 
            
            ///<summary> Racine </summary>
            public string Khmrac { get; set; } 
            
            ///<summary> Environnement </summary>
            public string Khmenv { get; set; } 
            
            ///<summary> Désignation </summary>
            public string Khmdes { get; set; } 
            
            ///<summary> 'D' si répertoire Document </summary>
            public string Khmtch { get; set; } 
            
            ///<summary> Chemin </summary>
            public string Khmchm { get; set; } 
            
            ///<summary> Création user </summary>
            public string Khmcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Khmcrd { get; set; } 
            
            ///<summary> Mise à jour User </summary>
            public string Khmmju { get; set; } 
            
            ///<summary> Mise à jour Date </summary>
            public int Khmmjd { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KChemin  x=this,  y=obj as KChemin;
            if( y == default(KChemin) ) return false;
            return (
                    x.Khmcle==y.Khmcle
                    && x.Khmsrv==y.Khmsrv
                    && x.Khmrac==y.Khmrac
                    && x.Khmenv==y.Khmenv
                    && x.Khmdes==y.Khmdes
                    && x.Khmtch==y.Khmtch
                    && x.Khmchm==y.Khmchm
                    && x.Khmcru==y.Khmcru
                    && x.Khmcrd==y.Khmcrd
                    && x.Khmmju==y.Khmmju
                    && x.Khmmjd==y.Khmmjd  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((
                       17 + (this.Khmcle?? "").GetHashCode()
                      * 23 ) + (this.Khmsrv?? "").GetHashCode()
                      * 23 ) + (this.Khmrac?? "").GetHashCode()
                      * 23 ) + (this.Khmenv?? "").GetHashCode()
                      * 23 ) + (this.Khmdes?? "").GetHashCode()
                      * 23 ) + (this.Khmtch?? "").GetHashCode()
                      * 23 ) + (this.Khmchm?? "").GetHashCode()
                      * 23 ) + (this.Khmcru?? "").GetHashCode()
                      * 23 ) + (this.Khmcrd.GetHashCode() ) 
                      * 23 ) + (this.Khmmju?? "").GetHashCode()
                      * 23 ) + (this.Khmmjd.GetHashCode() )                    );
           }
        }
    }
}
