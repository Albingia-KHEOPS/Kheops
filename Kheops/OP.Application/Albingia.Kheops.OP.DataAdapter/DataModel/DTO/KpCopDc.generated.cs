using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPCOPDC
    public partial class KpCopDc  {
             //KPCOPDC

            ///<summary>Public empty contructor</summary>
            public KpCopDc() {}
            ///<summary>Public empty contructor</summary>
            public KpCopDc(KpCopDc copyFrom) 
            {
                  this.Khqtyp= copyFrom.Khqtyp;
                  this.Khqipb= copyFrom.Khqipb;
                  this.Khqalx= copyFrom.Khqalx;
                  this.Khqavn= copyFrom.Khqavn;
                  this.Khqoldc= copyFrom.Khqoldc;
                  this.Khqcode= copyFrom.Khqcode;
                  this.Khqnomd= copyFrom.Khqnomd;
                  this.Khqtable= copyFrom.Khqtable;
                  this.Khqoldid= copyFrom.Khqoldid;
        
            }        
            
            ///<summary> Type </summary>
            public string Khqtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Khqipb { get; set; } 
            
            ///<summary> Aliment/Version </summary>
            public int Khqalx { get; set; } 
            
            ///<summary> Avenant </summary>
            public int Khqavn { get; set; } 
            
            ///<summary> Chemin complet de l'ancien fichier </summary>
            public string Khqoldc { get; set; } 
            
            ///<summary> N° du nouveau document </summary>
            public Int64 Khqcode { get; set; } 
            
            ///<summary> Nom du nouveau document </summary>
            public string Khqnomd { get; set; } 
            
            ///<summary> Table cible de la copie </summary>
            public string Khqtable { get; set; } 
            
            ///<summary> Ancien ID </summary>
            public Int64 Khqoldid { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCopDc  x=this,  y=obj as KpCopDc;
            if( y == default(KpCopDc) ) return false;
            return (
                    x.Khqtyp==y.Khqtyp
                    && x.Khqipb==y.Khqipb
                    && x.Khqalx==y.Khqalx
                    && x.Khqavn==y.Khqavn
                    && x.Khqoldc==y.Khqoldc
                    && x.Khqcode==y.Khqcode
                    && x.Khqnomd==y.Khqnomd
                    && x.Khqtable==y.Khqtable
                    && x.Khqoldid==y.Khqoldid  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((
                       17 + (this.Khqtyp?? "").GetHashCode()
                      * 23 ) + (this.Khqipb?? "").GetHashCode()
                      * 23 ) + (this.Khqalx.GetHashCode() ) 
                      * 23 ) + (this.Khqavn.GetHashCode() ) 
                      * 23 ) + (this.Khqoldc?? "").GetHashCode()
                      * 23 ) + (this.Khqcode.GetHashCode() ) 
                      * 23 ) + (this.Khqnomd?? "").GetHashCode()
                      * 23 ) + (this.Khqtable?? "").GetHashCode()
                      * 23 ) + (this.Khqoldid.GetHashCode() )                    );
           }
        }
    }
}
