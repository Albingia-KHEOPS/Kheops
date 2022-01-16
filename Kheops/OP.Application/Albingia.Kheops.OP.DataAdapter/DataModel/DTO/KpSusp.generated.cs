using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.KPSUSP
    public partial class KpSusp  {
             //KPSUSP

            ///<summary>Public empty contructor</summary>
            public KpSusp() {}
            ///<summary>Public empty contructor</summary>
            public KpSusp(KpSusp copyFrom) 
            {
                  this.Kicid= copyFrom.Kicid;
                  this.Kictyp= copyFrom.Kictyp;
                  this.Kicipb= copyFrom.Kicipb;
                  this.Kicalx= copyFrom.Kicalx;
                  this.Kictye= copyFrom.Kictye;
                  this.Kicipk= copyFrom.Kicipk;
                  this.Kicnur= copyFrom.Kicnur;
                  this.Kicori= copyFrom.Kicori;
                  this.Kicdebm= copyFrom.Kicdebm;
                  this.Kicdebd= copyFrom.Kicdebd;
                  this.Kicdebh= copyFrom.Kicdebh;
                  this.Kicfinm= copyFrom.Kicfinm;
                  this.Kicfind= copyFrom.Kicfind;
                  this.Kicfinh= copyFrom.Kicfinh;
                  this.Kicrsad= copyFrom.Kicrsad;
                  this.Kicrsah= copyFrom.Kicrsah;
                  this.Kicrevd= copyFrom.Kicrevd;
                  this.Kicrevh= copyFrom.Kicrevh;
                  this.Kiccru= copyFrom.Kiccru;
                  this.Kiccrd= copyFrom.Kiccrd;
                  this.Kiccrh= copyFrom.Kiccrh;
                  this.Kicavn= copyFrom.Kicavn;
                  this.Kicmju= copyFrom.Kicmju;
                  this.Kicmjd= copyFrom.Kicmjd;
                  this.Kicmjh= copyFrom.Kicmjh;
                  this.Kicsit= copyFrom.Kicsit;
                  this.Kicstu= copyFrom.Kicstu;
                  this.Kicstd= copyFrom.Kicstd;
                  this.Kicsth= copyFrom.Kicsth;
                  this.Kicaca= copyFrom.Kicaca;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kicid { get; set; } 
            
            ///<summary> Type  O Offre  P Police </summary>
            public string Kictyp { get; set; } 
            
            ///<summary> Police Offre </summary>
            public string Kicipb { get; set; } 
            
            ///<summary> Aliment vers </summary>
            public int Kicalx { get; set; } 
            
            ///<summary> S=Susp N=Non gar </summary>
            public string Kictye { get; set; } 
            
            ///<summary> prim </summary>
            public int Kicipk { get; set; } 
            
            ///<summary> Numéro chrono relance </summary>
            public int Kicnur { get; set; } 
            
            ///<summary> Origine </summary>
            public string Kicori { get; set; } 
            
            ///<summary> Code motif d'entrée </summary>
            public string Kicdebm { get; set; } 
            
            ///<summary> Début suspension Date </summary>
            public int Kicdebd { get; set; } 
            
            ///<summary> Début suspension Heure HHMN </summary>
            public int Kicdebh { get; set; } 
            
            ///<summary> Motif fin suspension (réSil/rGlt) </summary>
            public string Kicfinm { get; set; } 
            
            ///<summary> Fin de suspension Date </summary>
            public int Kicfind { get; set; } 
            
            ///<summary> Fin Suspension Heure HHMN </summary>
            public int Kicfinh { get; set; } 
            
            ///<summary> Date passation résile auto </summary>
            public int Kicrsad { get; set; } 
            
            ///<summary> résile auto heure </summary>
            public int Kicrsah { get; set; } 
            
            ///<summary> Date de Remise en vigueur </summary>
            public int Kicrevd { get; set; } 
            
            ///<summary> Remise en vigueur heure HHMNSS </summary>
            public int Kicrevh { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kiccru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kiccrd { get; set; } 
            
            ///<summary> Création Heure HHMNSS </summary>
            public int Kiccrh { get; set; } 
            
            ///<summary> N° Avenant courant à la suspension </summary>
            public int Kicavn { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kicmju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kicmjd { get; set; } 
            
            ///<summary> Maj Heure HHMNSS </summary>
            public int Kicmjh { get; set; } 
            
            ///<summary> Situation Code A=Actif N=non X=Annul </summary>
            public string Kicsit { get; set; } 
            
            ///<summary> User sit </summary>
            public string Kicstu { get; set; } 
            
            ///<summary> Situation Date </summary>
            public int Kicstd { get; set; } 
            
            ///<summary> Situation Heure HHMNSS </summary>
            public int Kicsth { get; set; } 
            
            ///<summary> Actualisation auto possible O/N </summary>
            public string Kicaca { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpSusp  x=this,  y=obj as KpSusp;
            if( y == default(KpSusp) ) return false;
            return (
                    x.Kicid==y.Kicid
                    && x.Kictyp==y.Kictyp
                    && x.Kicipb==y.Kicipb
                    && x.Kicalx==y.Kicalx
                    && x.Kictye==y.Kictye
                    && x.Kicipk==y.Kicipk
                    && x.Kicnur==y.Kicnur
                    && x.Kicori==y.Kicori
                    && x.Kicdebm==y.Kicdebm
                    && x.Kicdebd==y.Kicdebd
                    && x.Kicdebh==y.Kicdebh
                    && x.Kicfinm==y.Kicfinm
                    && x.Kicfind==y.Kicfind
                    && x.Kicfinh==y.Kicfinh
                    && x.Kicrsad==y.Kicrsad
                    && x.Kicrsah==y.Kicrsah
                    && x.Kicrevd==y.Kicrevd
                    && x.Kicrevh==y.Kicrevh
                    && x.Kiccru==y.Kiccru
                    && x.Kiccrd==y.Kiccrd
                    && x.Kiccrh==y.Kiccrh
                    && x.Kicavn==y.Kicavn
                    && x.Kicmju==y.Kicmju
                    && x.Kicmjd==y.Kicmjd
                    && x.Kicmjh==y.Kicmjh
                    && x.Kicsit==y.Kicsit
                    && x.Kicstu==y.Kicstu
                    && x.Kicstd==y.Kicstd
                    && x.Kicsth==y.Kicsth
                    && x.Kicaca==y.Kicaca  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((((((((
                       17 + (this.Kicid.GetHashCode() ) 
                      * 23 ) + (this.Kictyp?? "").GetHashCode()
                      * 23 ) + (this.Kicipb?? "").GetHashCode()
                      * 23 ) + (this.Kicalx.GetHashCode() ) 
                      * 23 ) + (this.Kictye?? "").GetHashCode()
                      * 23 ) + (this.Kicipk.GetHashCode() ) 
                      * 23 ) + (this.Kicnur.GetHashCode() ) 
                      * 23 ) + (this.Kicori?? "").GetHashCode()
                      * 23 ) + (this.Kicdebm?? "").GetHashCode()
                      * 23 ) + (this.Kicdebd.GetHashCode() ) 
                      * 23 ) + (this.Kicdebh.GetHashCode() ) 
                      * 23 ) + (this.Kicfinm?? "").GetHashCode()
                      * 23 ) + (this.Kicfind.GetHashCode() ) 
                      * 23 ) + (this.Kicfinh.GetHashCode() ) 
                      * 23 ) + (this.Kicrsad.GetHashCode() ) 
                      * 23 ) + (this.Kicrsah.GetHashCode() ) 
                      * 23 ) + (this.Kicrevd.GetHashCode() ) 
                      * 23 ) + (this.Kicrevh.GetHashCode() ) 
                      * 23 ) + (this.Kiccru?? "").GetHashCode()
                      * 23 ) + (this.Kiccrd.GetHashCode() ) 
                      * 23 ) + (this.Kiccrh.GetHashCode() ) 
                      * 23 ) + (this.Kicavn.GetHashCode() ) 
                      * 23 ) + (this.Kicmju?? "").GetHashCode()
                      * 23 ) + (this.Kicmjd.GetHashCode() ) 
                      * 23 ) + (this.Kicmjh.GetHashCode() ) 
                      * 23 ) + (this.Kicsit?? "").GetHashCode()
                      * 23 ) + (this.Kicstu?? "").GetHashCode()
                      * 23 ) + (this.Kicstd.GetHashCode() ) 
                      * 23 ) + (this.Kicsth.GetHashCode() ) 
                      * 23 ) + (this.Kicaca?? "").GetHashCode()                   );
           }
        }
    }
}
