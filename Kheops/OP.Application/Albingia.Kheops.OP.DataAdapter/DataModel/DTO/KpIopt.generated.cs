using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKHEO.HPIOPT
    public partial class KpIopt  {
             //HPIOPT
             //KPIOPT

            ///<summary>Public empty contructor</summary>
            public KpIopt() {}
            ///<summary>Public empty contructor</summary>
            public KpIopt(KpIopt copyFrom) 
            {
                  this.Kfcid= copyFrom.Kfcid;
                  this.Kfctyp= copyFrom.Kfctyp;
                  this.Kfcipb= copyFrom.Kfcipb;
                  this.Kfcalx= copyFrom.Kfcalx;
                  this.Kfcavn= copyFrom.Kfcavn;
                  this.Kfchin= copyFrom.Kfchin;
                  this.Kfcfor= copyFrom.Kfcfor;
                  this.Kfcopt= copyFrom.Kfcopt;
                  this.Kfckdbid= copyFrom.Kfckdbid;
                  this.Kfcrrcr= copyFrom.Kfcrrcr;
                  this.Kfcrrc= copyFrom.Kfcrrc;
                  this.Kfcmnte= copyFrom.Kfcmnte;
                  this.Kfcseui= copyFrom.Kfcseui;
                  this.Kfcseur= copyFrom.Kfcseur;
                  this.Kfcseuc= copyFrom.Kfcseuc;
                  this.Kfcperr= copyFrom.Kfcperr;
                  this.Kfcautm= copyFrom.Kfcautm;
                  this.Kfccrd= copyFrom.Kfccrd;
                  this.Kfccrh= copyFrom.Kfccrh;
                  this.Kfcmaju= copyFrom.Kfcmaju;
                  this.Kfcmajd= copyFrom.Kfcmajd;
                  this.Kfcmajh= copyFrom.Kfcmajh;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kfcid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kfctyp { get; set; } 
            
            ///<summary> IPB/ALX </summary>
            public string Kfcipb { get; set; } 
            
            ///<summary> Aliment/version </summary>
            public int Kfcalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kfcavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kfchin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kfcfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kfcopt { get; set; } 
            
            ///<summary> Lien KPOPT </summary>
            public Int64 Kfckdbid { get; set; } 
            
            ///<summary> Renonciation à Recours Réciproque </summary>
            public string Kfcrrcr { get; set; } 
            
            ///<summary> Renonciation à recours </summary>
            public string Kfcrrc { get; set; } 
            
            ///<summary> EDUCA : Montant par élève </summary>
            public Decimal Kfcmnte { get; set; } 
            
            ///<summary> EDUCA Seuil interruption </summary>
            public int Kfcseui { get; set; } 
            
            ///<summary> EDUCA Seuil  Redoublement </summary>
            public int Kfcseur { get; set; } 
            
            ///<summary> EDUCA Seuil Cours particuliers </summary>
            public int Kfcseuc { get; set; } 
            
            ///<summary> EDUCA Perte de revenus </summary>
            public string Kfcperr { get; set; } 
            
            ///<summary> Autre motif   ex lien KPIDESI </summary>
            public string Kfcautm { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kfccrd { get; set; } 
            
            ///<summary> Création Heure </summary>
            public int Kfccrh { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kfcmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kfcmajd { get; set; } 
            
            ///<summary> Maj Heure </summary>
            public int Kfcmajh { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpIopt  x=this,  y=obj as KpIopt;
            if( y == default(KpIopt) ) return false;
            return (
                    x.Kfcid==y.Kfcid
                    && x.Kfctyp==y.Kfctyp
                    && x.Kfcipb==y.Kfcipb
                    && x.Kfcalx==y.Kfcalx
                    && x.Kfcfor==y.Kfcfor
                    && x.Kfcopt==y.Kfcopt
                    && x.Kfckdbid==y.Kfckdbid
                    && x.Kfcrrcr==y.Kfcrrcr
                    && x.Kfcrrc==y.Kfcrrc
                    && x.Kfcmnte==y.Kfcmnte
                    && x.Kfcseui==y.Kfcseui
                    && x.Kfcseur==y.Kfcseur
                    && x.Kfcseuc==y.Kfcseuc
                    && x.Kfcperr==y.Kfcperr
                    && x.Kfcautm==y.Kfcautm
                    && x.Kfccrd==y.Kfccrd
                    && x.Kfccrh==y.Kfccrh
                    && x.Kfcmaju==y.Kfcmaju
                    && x.Kfcmajd==y.Kfcmajd
                    && x.Kfcmajh==y.Kfcmajh  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((
                       17 + (this.Kfcid.GetHashCode() ) 
                      * 23 ) + (this.Kfctyp?? "").GetHashCode()
                      * 23 ) + (this.Kfcipb?? "").GetHashCode()
                      * 23 ) + (this.Kfcalx.GetHashCode() ) 
                      * 23 ) + (this.Kfcfor.GetHashCode() ) 
                      * 23 ) + (this.Kfcopt.GetHashCode() ) 
                      * 23 ) + (this.Kfckdbid.GetHashCode() ) 
                      * 23 ) + (this.Kfcrrcr?? "").GetHashCode()
                      * 23 ) + (this.Kfcrrc?? "").GetHashCode()
                      * 23 ) + (this.Kfcmnte.GetHashCode() ) 
                      * 23 ) + (this.Kfcseui.GetHashCode() ) 
                      * 23 ) + (this.Kfcseur.GetHashCode() ) 
                      * 23 ) + (this.Kfcseuc.GetHashCode() ) 
                      * 23 ) + (this.Kfcperr?? "").GetHashCode()
                      * 23 ) + (this.Kfcautm?? "").GetHashCode()
                      * 23 ) + (this.Kfccrd.GetHashCode() ) 
                      * 23 ) + (this.Kfccrh.GetHashCode() ) 
                      * 23 ) + (this.Kfcmaju?? "").GetHashCode()
                      * 23 ) + (this.Kfcmajd.GetHashCode() ) 
                      * 23 ) + (this.Kfcmajh.GetHashCode() )                    );
           }
        }
    }
}
