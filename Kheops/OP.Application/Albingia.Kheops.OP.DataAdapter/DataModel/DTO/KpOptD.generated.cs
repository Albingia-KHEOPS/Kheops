using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPOPTD
    public partial class KpOptD  {
             //HPOPTD
             //KPOPTD

            ///<summary>Public empty contructor</summary>
            public KpOptD() {}
            ///<summary>Public empty contructor</summary>
            public KpOptD(KpOptD copyFrom) 
            {
                  this.Kdcid= copyFrom.Kdcid;
                  this.Kdctyp= copyFrom.Kdctyp;
                  this.Kdcipb= copyFrom.Kdcipb;
                  this.Kdcalx= copyFrom.Kdcalx;
                  this.Kdcavn= copyFrom.Kdcavn;
                  this.Kdchin= copyFrom.Kdchin;
                  this.Kdcfor= copyFrom.Kdcfor;
                  this.Kdcopt= copyFrom.Kdcopt;
                  this.Kdckdbid= copyFrom.Kdckdbid;
                  this.Kdcteng= copyFrom.Kdcteng;
                  this.Kdckakid= copyFrom.Kdckakid;
                  this.Kdckaeid= copyFrom.Kdckaeid;
                  this.Kdckaqid= copyFrom.Kdckaqid;
                  this.Kdcmodele= copyFrom.Kdcmodele;
                  this.Kdckarid= copyFrom.Kdckarid;
                  this.Kdccru= copyFrom.Kdccru;
                  this.Kdccrd= copyFrom.Kdccrd;
                  this.Kdcmaju= copyFrom.Kdcmaju;
                  this.Kdcmajd= copyFrom.Kdcmajd;
                  this.Kdcflag= copyFrom.Kdcflag;
                  this.Kdcordre= copyFrom.Kdcordre;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdcid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdctyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdcipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdcalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdcavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdchin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdcfor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kdcopt { get; set; } 
            
            ///<summary> Lien KPOPT </summary>
            public Int64 Kdckdbid { get; set; } 
            
            ///<summary> Type enregistrement Volet Bloc </summary>
            public string Kdcteng { get; set; } 
            
            ///<summary> Lien KVOLET </summary>
            public Int64 Kdckakid { get; set; } 
            
            ///<summary> Lien KBLOC </summary>
            public Int64 Kdckaeid { get; set; } 
            
            ///<summary> Lien KCATBLOC </summary>
            public Int64 Kdckaqid { get; set; } 
            
            ///<summary> Modèle </summary>
            public string Kdcmodele { get; set; } 
            
            ///<summary> Lien KCATMODELE </summary>
            public Int64 Kdckarid { get; set; } 
            
            ///<summary> Création user </summary>
            public string Kdccru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdccrd { get; set; } 
            
            ///<summary> MAJ User </summary>
            public string Kdcmaju { get; set; } 
            
            ///<summary> MAJ Date </summary>
            public int Kdcmajd { get; set; } 
            
            ///<summary> Flag modifié 1/0 </summary>
            public int Kdcflag { get; set; } 
            
            ///<summary> N° Ordre (KCATVOLET KCATBLOC) </summary>
            public Decimal Kdcordre { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpOptD  x=this,  y=obj as KpOptD;
            if( y == default(KpOptD) ) return false;
            return (
                    x.Kdcid==y.Kdcid
                    && x.Kdctyp==y.Kdctyp
                    && x.Kdcipb==y.Kdcipb
                    && x.Kdcalx==y.Kdcalx
                    && x.Kdcfor==y.Kdcfor
                    && x.Kdcopt==y.Kdcopt
                    && x.Kdckdbid==y.Kdckdbid
                    && x.Kdcteng==y.Kdcteng
                    && x.Kdckakid==y.Kdckakid
                    && x.Kdckaeid==y.Kdckaeid
                    && x.Kdckaqid==y.Kdckaqid
                    && x.Kdcmodele==y.Kdcmodele
                    && x.Kdckarid==y.Kdckarid
                    && x.Kdccru==y.Kdccru
                    && x.Kdccrd==y.Kdccrd
                    && x.Kdcmaju==y.Kdcmaju
                    && x.Kdcmajd==y.Kdcmajd
                    && x.Kdcflag==y.Kdcflag
                    && x.Kdcordre==y.Kdcordre  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   (((((((((((((((((((
                       17 + (this.Kdcid.GetHashCode() ) 
                      * 23 ) + (this.Kdctyp?? "").GetHashCode()
                      * 23 ) + (this.Kdcipb?? "").GetHashCode()
                      * 23 ) + (this.Kdcalx.GetHashCode() ) 
                      * 23 ) + (this.Kdcfor.GetHashCode() ) 
                      * 23 ) + (this.Kdcopt.GetHashCode() ) 
                      * 23 ) + (this.Kdckdbid.GetHashCode() ) 
                      * 23 ) + (this.Kdcteng?? "").GetHashCode()
                      * 23 ) + (this.Kdckakid.GetHashCode() ) 
                      * 23 ) + (this.Kdckaeid.GetHashCode() ) 
                      * 23 ) + (this.Kdckaqid.GetHashCode() ) 
                      * 23 ) + (this.Kdcmodele?? "").GetHashCode()
                      * 23 ) + (this.Kdckarid.GetHashCode() ) 
                      * 23 ) + (this.Kdccru?? "").GetHashCode()
                      * 23 ) + (this.Kdccrd.GetHashCode() ) 
                      * 23 ) + (this.Kdcmaju?? "").GetHashCode()
                      * 23 ) + (this.Kdcmajd.GetHashCode() ) 
                      * 23 ) + (this.Kdcflag.GetHashCode() ) 
                      * 23 ) + (this.Kdcordre.GetHashCode() )                    );
           }
        }
    }
}
