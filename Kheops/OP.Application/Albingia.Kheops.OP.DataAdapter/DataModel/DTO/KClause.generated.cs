using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KCLAUSE
    public partial class KClause  {
             //KCLAUSE

            ///<summary>Public empty contructor</summary>
            public KClause() {}
            ///<summary>Public empty contructor</summary>
            public KClause(KClause copyFrom) 
            {
                  this.Kduid= copyFrom.Kduid;
                  this.Kdunm1= copyFrom.Kdunm1;
                  this.Kdunm2= copyFrom.Kdunm2;
                  this.Kdunm3= copyFrom.Kdunm3;
                  this.Kduver= copyFrom.Kduver;
                  this.Kdulib= copyFrom.Kdulib;
                  this.Kdulir= copyFrom.Kdulir;
                  this.Kdukdwid= copyFrom.Kdukdwid;
                  this.Kdukdvid= copyFrom.Kdukdvid;
                  this.Kdukdxid= copyFrom.Kdukdxid;
                  this.Kdudatd= copyFrom.Kdudatd;
                  this.Kdudatf= copyFrom.Kdudatf;
                  this.Kdudoc= copyFrom.Kdudoc;
                  this.Kdutdoc= copyFrom.Kdutdoc;
                  this.Kduserv= copyFrom.Kduserv;
                  this.Kduactg= copyFrom.Kduactg;
                  this.Kducru= copyFrom.Kducru;
                  this.Kducrd= copyFrom.Kducrd;
                  this.Kducrh= copyFrom.Kducrh;
                  this.Kdumaju= copyFrom.Kdumaju;
                  this.Kdumajd= copyFrom.Kdumajd;
                  this.Kdumajh= copyFrom.Kdumajh;
                  this.Kdurgp= copyFrom.Kdurgp;
                  this.Kduord= copyFrom.Kduord;
                  this.Kduanx= copyFrom.Kduanx;
                  this.Kduora= copyFrom.Kduora;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kduid { get; set; } 
            
            ///<summary> Nom 1 </summary>
            public string Kdunm1 { get; set; } 
            
            ///<summary> Nom 2 </summary>
            public string Kdunm2 { get; set; } 
            
            ///<summary> Nom 3 </summary>
            public int Kdunm3 { get; set; } 
            
            ///<summary> N° version </summary>
            public int Kduver { get; set; } 
            
            ///<summary> Libellé </summary>
            public string Kdulib { get; set; } 
            
            ///<summary> Libellé raccourci </summary>
            public string Kdulir { get; set; } 
            
            ///<summary> Lien avec Designation KDESI </summary>
            public Int64 Kdukdwid { get; set; } 
            
            ///<summary> Lien Emplacement KDOCEMP </summary>
            public Int64 Kdukdvid { get; set; } 
            
            ///<summary> Lien Mot Clé  KMOTCLE </summary>
            public Int64 Kdukdxid { get; set; } 
            
            ///<summary> Date validité Début </summary>
            public int Kdudatd { get; set; } 
            
            ///<summary> Date Validité Fin </summary>
            public int Kdudatf { get; set; } 
            
            ///<summary> Nom du document </summary>
            public string Kdudoc { get; set; } 
            
            ///<summary> Type de Document (CG,CS,CP,...) </summary>
            public string Kdutdoc { get; set; } 
            
            ///<summary> Service </summary>
            public string Kduserv { get; set; } 
            
            ///<summary> Acte de gestion </summary>
            public string Kduactg { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kducru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kducrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kducrh { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdumaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdumajd { get; set; } 
            
            ///<summary> MAJ Heure </summary>
            public int Kdumajh { get; set; } 
            
            ///<summary> Code regroupement clauses </summary>
            public string Kdurgp { get; set; } 
            
            ///<summary> Ordonnancement impression </summary>
            public int Kduord { get; set; } 
            
            ///<summary> Code annexe </summary>
            public string Kduanx { get; set; } 
            
            ///<summary> Ordonnancement Impression Annexe </summary>
            public int Kduora { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KClause  x=this,  y=obj as KClause;
            if( y == default(KClause) ) return false;
            return (
                    x.Kduid==y.Kduid
                    && x.Kdunm1==y.Kdunm1
                    && x.Kdunm2==y.Kdunm2
                    && x.Kdunm3==y.Kdunm3
                    && x.Kduver==y.Kduver
                    && x.Kdulib==y.Kdulib
                    && x.Kdulir==y.Kdulir
                    && x.Kdukdwid==y.Kdukdwid
                    && x.Kdukdvid==y.Kdukdvid
                    && x.Kdukdxid==y.Kdukdxid
                    && x.Kdudatd==y.Kdudatd
                    && x.Kdudatf==y.Kdudatf
                    && x.Kdudoc==y.Kdudoc
                    && x.Kdutdoc==y.Kdutdoc
                    && x.Kduserv==y.Kduserv
                    && x.Kduactg==y.Kduactg
                    && x.Kducru==y.Kducru
                    && x.Kducrd==y.Kducrd
                    && x.Kducrh==y.Kducrh
                    && x.Kdumaju==y.Kdumaju
                    && x.Kdumajd==y.Kdumajd
                    && x.Kdumajh==y.Kdumajh
                    && x.Kdurgp==y.Kdurgp
                    && x.Kduord==y.Kduord
                    && x.Kduanx==y.Kduanx
                    && x.Kduora==y.Kduora  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((
                       17 + (this.Kduid.GetHashCode() ) 
                      * 23 ) + (this.Kdunm1?? "").GetHashCode()
                      * 23 ) + (this.Kdunm2?? "").GetHashCode()
                      * 23 ) + (this.Kdunm3.GetHashCode() ) 
                      * 23 ) + (this.Kduver.GetHashCode() ) 
                      * 23 ) + (this.Kdulib?? "").GetHashCode()
                      * 23 ) + (this.Kdulir?? "").GetHashCode()
                      * 23 ) + (this.Kdukdwid.GetHashCode() ) 
                      * 23 ) + (this.Kdukdvid.GetHashCode() ) 
                      * 23 ) + (this.Kdukdxid.GetHashCode() ) 
                      * 23 ) + (this.Kdudatd.GetHashCode() ) 
                      * 23 ) + (this.Kdudatf.GetHashCode() ) 
                      * 23 ) + (this.Kdudoc?? "").GetHashCode()
                      * 23 ) + (this.Kdutdoc?? "").GetHashCode()
                      * 23 ) + (this.Kduserv?? "").GetHashCode()
                      * 23 ) + (this.Kduactg?? "").GetHashCode()
                      * 23 ) + (this.Kducru?? "").GetHashCode()
                      * 23 ) + (this.Kducrd.GetHashCode() ) 
                      * 23 ) + (this.Kducrh.GetHashCode() ) 
                      * 23 ) + (this.Kdumaju?? "").GetHashCode()
                      * 23 ) + (this.Kdumajd.GetHashCode() ) 
                      * 23 ) + (this.Kdumajh.GetHashCode() ) 
                      * 23 ) + (this.Kdurgp?? "").GetHashCode()
                      * 23 ) + (this.Kduord.GetHashCode() ) 
                      * 23 ) + (this.Kduanx?? "").GetHashCode()
                      * 23 ) + (this.Kduora.GetHashCode() )                    );
           }
        }
    }
}
