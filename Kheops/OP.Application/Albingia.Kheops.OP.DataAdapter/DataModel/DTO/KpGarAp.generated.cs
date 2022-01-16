using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    // ZALBINKQUA.HPGARAP
    public partial class KpGarAp  {
             //HPGARAP
             //KPGARAP

            ///<summary>Public empty contructor</summary>
            public KpGarAp() {}
            ///<summary>Public empty contructor</summary>
            public KpGarAp(KpGarAp copyFrom) 
            {
                  this.Kdfid= copyFrom.Kdfid;
                  this.Kdftyp= copyFrom.Kdftyp;
                  this.Kdfipb= copyFrom.Kdfipb;
                  this.Kdfalx= copyFrom.Kdfalx;
                  this.Kdfavn= copyFrom.Kdfavn;
                  this.Kdfhin= copyFrom.Kdfhin;
                  this.Kdffor= copyFrom.Kdffor;
                  this.Kdfopt= copyFrom.Kdfopt;
                  this.Kdfgaran= copyFrom.Kdfgaran;
                  this.Kdfkdeid= copyFrom.Kdfkdeid;
                  this.Kdfgan= copyFrom.Kdfgan;
                  this.Kdfperi= copyFrom.Kdfperi;
                  this.Kdfrsq= copyFrom.Kdfrsq;
                  this.Kdfobj= copyFrom.Kdfobj;
                  this.Kdfinven= copyFrom.Kdfinven;
                  this.Kdfinvep= copyFrom.Kdfinvep;
                  this.Kdfcru= copyFrom.Kdfcru;
                  this.Kdfcrd= copyFrom.Kdfcrd;
                  this.Kdfmaju= copyFrom.Kdfmaju;
                  this.Kdfmajd= copyFrom.Kdfmajd;
                  this.Kdfprv= copyFrom.Kdfprv;
                  this.Kdfpra= copyFrom.Kdfpra;
                  this.Kdfprw= copyFrom.Kdfprw;
                  this.Kdfpru= copyFrom.Kdfpru;
                  this.Kdftyc= copyFrom.Kdftyc;
                  this.Kdfmnt= copyFrom.Kdfmnt;
        
            }        
            
            ///<summary> ID unique </summary>
            public Int64 Kdfid { get; set; } 
            
            ///<summary> Type O/P </summary>
            public string Kdftyp { get; set; } 
            
            ///<summary> IPB </summary>
            public string Kdfipb { get; set; } 
            
            ///<summary> ALX </summary>
            public int Kdfalx { get; set; } 
            
            ///<summary> N° avenant </summary>
            public int? Kdfavn { get; set; } 
            
            ///<summary> N° histo par avenant </summary>
            public int? Kdfhin { get; set; } 
            
            ///<summary> Formule </summary>
            public int Kdffor { get; set; } 
            
            ///<summary> Option </summary>
            public int Kdfopt { get; set; } 
            
            ///<summary> Garantie </summary>
            public string Kdfgaran { get; set; } 
            
            ///<summary> Lien KPGARAN </summary>
            public Int64 Kdfkdeid { get; set; } 
            
            ///<summary> Nature  'E' Exclure   'A' Accordée </summary>
            public string Kdfgan { get; set; } 
            
            ///<summary> Périmètre </summary>
            public string Kdfperi { get; set; } 
            
            ///<summary> Risque </summary>
            public int Kdfrsq { get; set; } 
            
            ///<summary> Objet </summary>
            public int Kdfobj { get; set; } 
            
            ///<summary> Lien KPINVEN </summary>
            public Int64 Kdfinven { get; set; } 
            
            ///<summary> Poste inventaire </summary>
            public int Kdfinvep { get; set; } 
            
            ///<summary> Création User </summary>
            public string Kdfcru { get; set; } 
            
            ///<summary> Création Date </summary>
            public int Kdfcrd { get; set; } 
            
            ///<summary> Maj User </summary>
            public string Kdfmaju { get; set; } 
            
            ///<summary> Maj Date </summary>
            public int Kdfmajd { get; set; } 
            
            ///<summary> Prime Valeur origine </summary>
            public Decimal Kdfprv { get; set; } 
            
            ///<summary> Prime Valeur Actualisée </summary>
            public Decimal Kdfpra { get; set; } 
            
            ///<summary> Prime Valeur de Travail </summary>
            public Decimal Kdfprw { get; set; } 
            
            ///<summary> Prime Unité </summary>
            public string Kdfpru { get; set; } 
            
            ///<summary> Prime type de calcul </summary>
            public string Kdftyc { get; set; } 
            
            ///<summary> Prime Montant Calculé </summary>
            public Decimal Kdfmnt { get; set; } 
  


        public override bool Equals(object  obj)
        {
            KpGarAp  x=this,  y=obj as KpGarAp;
            if( y == default(KpGarAp) ) return false;
            return (
                    x.Kdfid==y.Kdfid
                    && x.Kdftyp==y.Kdftyp
                    && x.Kdfipb==y.Kdfipb
                    && x.Kdfalx==y.Kdfalx
                    && x.Kdffor==y.Kdffor
                    && x.Kdfopt==y.Kdfopt
                    && x.Kdfgaran==y.Kdfgaran
                    && x.Kdfkdeid==y.Kdfkdeid
                    && x.Kdfgan==y.Kdfgan
                    && x.Kdfperi==y.Kdfperi
                    && x.Kdfrsq==y.Kdfrsq
                    && x.Kdfobj==y.Kdfobj
                    && x.Kdfinven==y.Kdfinven
                    && x.Kdfinvep==y.Kdfinvep
                    && x.Kdfcru==y.Kdfcru
                    && x.Kdfcrd==y.Kdfcrd
                    && x.Kdfmaju==y.Kdfmaju
                    && x.Kdfmajd==y.Kdfmajd
                    && x.Kdfprv==y.Kdfprv
                    && x.Kdfpra==y.Kdfpra
                    && x.Kdfprw==y.Kdfprw
                    && x.Kdfpru==y.Kdfpru
                    && x.Kdftyc==y.Kdftyc
                    && x.Kdfmnt==y.Kdfmnt  
            );
        }

        public override int GetHashCode()
        {
            unchecked {
              return 
                   ((((((((((((((((((((((((
                       17 + (this.Kdfid.GetHashCode() ) 
                      * 23 ) + (this.Kdftyp?? "").GetHashCode()
                      * 23 ) + (this.Kdfipb?? "").GetHashCode()
                      * 23 ) + (this.Kdfalx.GetHashCode() ) 
                      * 23 ) + (this.Kdffor.GetHashCode() ) 
                      * 23 ) + (this.Kdfopt.GetHashCode() ) 
                      * 23 ) + (this.Kdfgaran?? "").GetHashCode()
                      * 23 ) + (this.Kdfkdeid.GetHashCode() ) 
                      * 23 ) + (this.Kdfgan?? "").GetHashCode()
                      * 23 ) + (this.Kdfperi?? "").GetHashCode()
                      * 23 ) + (this.Kdfrsq.GetHashCode() ) 
                      * 23 ) + (this.Kdfobj.GetHashCode() ) 
                      * 23 ) + (this.Kdfinven.GetHashCode() ) 
                      * 23 ) + (this.Kdfinvep.GetHashCode() ) 
                      * 23 ) + (this.Kdfcru?? "").GetHashCode()
                      * 23 ) + (this.Kdfcrd.GetHashCode() ) 
                      * 23 ) + (this.Kdfmaju?? "").GetHashCode()
                      * 23 ) + (this.Kdfmajd.GetHashCode() ) 
                      * 23 ) + (this.Kdfprv.GetHashCode() ) 
                      * 23 ) + (this.Kdfpra.GetHashCode() ) 
                      * 23 ) + (this.Kdfprw.GetHashCode() ) 
                      * 23 ) + (this.Kdfpru?? "").GetHashCode()
                      * 23 ) + (this.Kdftyc?? "").GetHashCode()
                      * 23 ) + (this.Kdfmnt.GetHashCode() )                    );
           }
        }
    }
}
