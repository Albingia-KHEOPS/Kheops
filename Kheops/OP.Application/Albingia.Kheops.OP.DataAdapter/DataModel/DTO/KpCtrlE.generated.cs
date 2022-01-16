using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPCTRLE
    public partial class KpCtrlE  {
             //HPCTRLE
             //KPCTRLE

            ///<summary>Public empty contructor</summary>
            public KpCtrlE() {}
            ///<summary>Public empty contructor</summary>
            public KpCtrlE(KpCtrlE copyFrom) 
            {
                  this.Kevtyp= copyFrom.Kevtyp;
                  this.Kevipb= copyFrom.Kevipb;
                  this.Kevalx= copyFrom.Kevalx;
                  this.Kevavn= copyFrom.Kevavn;
                  this.Kevhin= copyFrom.Kevhin;
                  this.Kevetape= copyFrom.Kevetape;
                  this.Kevetord= copyFrom.Kevetord;
                  this.Kevordr= copyFrom.Kevordr;
                  this.Kevperi= copyFrom.Kevperi;
                  this.Kevrsq= copyFrom.Kevrsq;
                  this.Kevobj= copyFrom.Kevobj;
                  this.Kevkbeid= copyFrom.Kevkbeid;
                  this.Kevfor= copyFrom.Kevfor;
                  this.Kevopt= copyFrom.Kevopt;
                  this.Kevnivm= copyFrom.Kevnivm;
                  this.Kevcru= copyFrom.Kevcru;
                  this.Kevcrd= copyFrom.Kevcrd;
                  this.Kevcrh= copyFrom.Kevcrh;
                  this.Kevmaju= copyFrom.Kevmaju;
                  this.Kevmajd= copyFrom.Kevmajd;
                  this.Kevmajh= copyFrom.Kevmajh;
                  this.Kevtag= copyFrom.Kevtag;
                  this.Kevtagc= copyFrom.Kevtagc;
        
            }        
            
            ///<summary> TYP O/P </summary>
            public string Kevtyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kevipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kevalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kevavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kevhin { get; set; } 
            
            ///<summary> Etape de génération </summary>
            public string Kevetape { get; set; } 
            
            ///<summary> N° ordre Etape </summary>
            public int Kevetord { get; set; } 
            
            ///<summary> N° ordre dans Etape </summary>
            public int Kevordr { get; set; } 
            
            ///<summary> Périmètre  BASE RISQUE .... </summary>
            public string Kevperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kevrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kevobj { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kevkbeid { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kevfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kevopt { get; set; } 
            
            ///<summary> Niv classement Bloq/NonVal/Avert/Val </summary>
            public string Kevnivm { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kevcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kevcrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kevcrh { get; set; } 
            
            ///<summary> Màj User </summary>
            public string Kevmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kevmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kevmajh { get; set; } 
            
            ///<summary> tag de passage dans l'acte de gest° </summary>
            public string Kevtag { get; set; } 
            
            ///<summary> Tag de génération de clauses O/N </summary>
            public string Kevtagc { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpCtrlE  x=this,  y=obj as KpCtrlE;
            if( y == default(KpCtrlE) ) return false;
            return (
                    x.Kevtyp==y.Kevtyp
                    && x.Kevipb==y.Kevipb
                    && x.Kevalx==y.Kevalx
                    && x.Kevetape==y.Kevetape
                    && x.Kevetord==y.Kevetord
                    && x.Kevordr==y.Kevordr
                    && x.Kevperi==y.Kevperi
                    && x.Kevrsq==y.Kevrsq
                    && x.Kevobj==y.Kevobj
                    && x.Kevkbeid==y.Kevkbeid
                    && x.Kevfor==y.Kevfor
                    && x.Kevopt==y.Kevopt
                    && x.Kevnivm==y.Kevnivm
                    && x.Kevcru==y.Kevcru
                    && x.Kevcrd==y.Kevcrd
                    && x.Kevcrh==y.Kevcrh
                    && x.Kevmaju==y.Kevmaju
                    && x.Kevmajd==y.Kevmajd
                    && x.Kevmajh==y.Kevmajh
                    && x.Kevtag==y.Kevtag
                    && x.Kevtagc==y.Kevtagc  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((((
                       17 + (this.Kevtyp?? "").GetHashCode()
                      * 23 ) + (this.Kevipb?? "").GetHashCode()
                      * 23 ) + (this.Kevalx.GetHashCode() ) 
                      * 23 ) + (this.Kevetape?? "").GetHashCode()
                      * 23 ) + (this.Kevetord.GetHashCode() ) 
                      * 23 ) + (this.Kevordr.GetHashCode() ) 
                      * 23 ) + (this.Kevperi?? "").GetHashCode()
                      * 23 ) + (this.Kevrsq.GetHashCode() ) 
                      * 23 ) + (this.Kevobj.GetHashCode() ) 
                      * 23 ) + (this.Kevkbeid.GetHashCode() ) 
                      * 23 ) + (this.Kevfor.GetHashCode() ) 
                      * 23 ) + (this.Kevopt.GetHashCode() ) 
                      * 23 ) + (this.Kevnivm?? "").GetHashCode()
                      * 23 ) + (this.Kevcru?? "").GetHashCode()
                      * 23 ) + (this.Kevcrd.GetHashCode() ) 
                      * 23 ) + (this.Kevcrh.GetHashCode() ) 
                      * 23 ) + (this.Kevmaju?? "").GetHashCode()
                      * 23 ) + (this.Kevmajd.GetHashCode() ) 
                      * 23 ) + (this.Kevmajh.GetHashCode() ) 
                      * 23 ) + (this.Kevtag?? "").GetHashCode()
                      * 23 ) + (this.Kevtagc?? "").GetHashCode()                   );
           }
        }
    }
}
