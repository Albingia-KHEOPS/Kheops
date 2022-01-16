using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPINVAPP
    public partial class KpInvApp  {
             //HPINVAPP
             //KPINVAPP

            ///<summary>Public empty contructor</summary>
            public KpInvApp() {}
            ///<summary>Public empty contructor</summary>
            public KpInvApp(KpInvApp copyFrom) 
            {
                  this.Kbgtyp= copyFrom.Kbgtyp;
                  this.Kbgipb= copyFrom.Kbgipb;
                  this.Kbgalx= copyFrom.Kbgalx;
                  this.Kbgavn= copyFrom.Kbgavn;
                  this.Kbghin= copyFrom.Kbghin;
                  this.Kbgnum= copyFrom.Kbgnum;
                  this.Kbgkbeid= copyFrom.Kbgkbeid;
                  this.Kbgperi= copyFrom.Kbgperi;
                  this.Kbgrsq= copyFrom.Kbgrsq;
                  this.Kbgobj= copyFrom.Kbgobj;
                  this.Kbgfor= copyFrom.Kbgfor;
                  this.Kbggar= copyFrom.Kbggar;
        
            }        
            
            ///<summary> Type O/P </summary>
            public string Kbgtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kbgipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kbgalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kbgavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kbghin { get; set; } 
            
            ///<summary> N° Ordre par Offre/Contrat </summary>
            public int Kbgnum { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kbgkbeid { get; set; } 
            
            ///<summary> Périmètre </summary>
            public string Kbgperi { get; set; } 
            
            ///<summary> N° Risque </summary>
            public int Kbgrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kbgobj { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kbgfor { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kbggar { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpInvApp  x=this,  y=obj as KpInvApp;
            if( y == default(KpInvApp) ) return false;
            return (
                    x.Kbgtyp==y.Kbgtyp
                    && x.Kbgipb==y.Kbgipb
                    && x.Kbgalx==y.Kbgalx
                    && x.Kbgnum==y.Kbgnum
                    && x.Kbgkbeid==y.Kbgkbeid
                    && x.Kbgperi==y.Kbgperi
                    && x.Kbgrsq==y.Kbgrsq
                    && x.Kbgobj==y.Kbgobj
                    && x.Kbgfor==y.Kbgfor
                    && x.Kbggar==y.Kbggar  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((
                       17 + (this.Kbgtyp?? "").GetHashCode()
                      * 23 ) + (this.Kbgipb?? "").GetHashCode()
                      * 23 ) + (this.Kbgalx.GetHashCode() ) 
                      * 23 ) + (this.Kbgnum.GetHashCode() ) 
                      * 23 ) + (this.Kbgkbeid.GetHashCode() ) 
                      * 23 ) + (this.Kbgperi?? "").GetHashCode()
                      * 23 ) + (this.Kbgrsq.GetHashCode() ) 
                      * 23 ) + (this.Kbgobj.GetHashCode() ) 
                      * 23 ) + (this.Kbgfor.GetHashCode() ) 
                      * 23 ) + (this.Kbggar?? "").GetHashCode()                   );
           }
        }
    }
}
